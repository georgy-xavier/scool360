using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr.Payroll
{
    public partial class PayrollType : System.Web.UI.Page
    {
        private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        
    private OdbcDataReader m_MyReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["StudId"] == null)
            //{
            //    Response.Redirect("PayrollHead.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            Mypay = MyUser.GetPayrollObj();
            if (Mypay == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(800))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    clear();
                    Btn_Save.Visible = false;
                    Btn_Add.Visible = true;
                    Btn_Cancel.Visible = false;
                    AddHeadToList();
                    LoadGrid();
                    //some initlization

                }
            }

        }

        private void clear()
        {
            Txt_BasicPay.Text = "";
            Txt_Cat.Text = "";
            Rdb_Daily.Checked = true;
            Rdb_Monthly.Checked = false;
            
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            General _GenObj = new General(Mypay.m_MysqlDb);
            int MaxId = 0;
            string _cat = Txt_Cat.Text.Trim();
            int _Bp = int.Parse(Txt_BasicPay.Text.ToString());
            string _Wage = "";
            if (Rdb_Daily.Checked)
            {
                _Wage = "Daily";
            }
            else
            {
                _Wage = "Monthly";
            }
            
            if (Mypay.IsPresentCategory(_cat))
            {
                WC_MessageBox.ShowMssage("Category Name Already Present");
            }
            else
            {
                Mypay.CreateTansationDb();
                try
                {

                    Mypay.AddCategory(_cat, _Bp, _Wage);
                    MaxId = GetTableMaxId("tblpay_category", "Id");
                    for (int i = 0; i < ChkBox_AllHead.Items.Count; i++)
                    {
                        if (ChkBox_AllHead.Items[i].Selected)
                        {
                            Mypay.AddHeadCatMap(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), MaxId);
                            ChkBox_AllHead.Items[i].Selected = false;
                            ChkBox_AllHead.Items.Remove(ChkBox_AllHead.Items[i]);
                            i--;
                        }
                    }
                    //m_DbLog.LogToDb(m_UserName, "Created Student", "Student Named " + _studname + " is created and approved", 1, m_TransationDb);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Payroll Type", "Payroll type " + _cat + " is created", 1, Mypay.m_TransationDb);

                    WC_MessageBox.ShowMssage("Successfully created");
                    
                    Mypay.EndSucessTansationDb();
                }

                catch
                {
                    Mypay.EndFailTansationDb();
                }
               
            }
            LoadGrid();
            ChkBox_AllHead.Items.Clear();
            AddHeadToList();
            clear();
        }

        private int GetTableMaxId(string _TableName, string _Field)
        {
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + "";

            m_MyReader =Mypay.m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                bool valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Id);
            }
            return Id;
        }

        private void AddHeadToList()
        {
            string _TableName = "tblpay_head";
            string _Field = "HeadName";
            OdbcDataReader MyPayReader = null;
            MyPayReader = Mypay.FillCheckList(_TableName, _Field);
            if (MyPayReader.HasRows)
            {
                Lbl_Message.Visible = false;
                Btn_Add.Enabled = true;
                while (MyPayReader.Read())
                {
                    ListItem li = new ListItem(MyPayReader.GetValue(1).ToString(), MyPayReader.GetValue(0).ToString());

                    ChkBox_AllHead.Items.Add(li);

                }
            }
            else
            {
                Btn_Add.Enabled = false;
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "No Heads found, Please add some heads";
            }
         
        }
        private void LoadGrid()
        {
            DataSet MyHead = Mypay.GetCat();
            if (MyHead != null && MyHead.Tables != null && MyHead.Tables[0].Rows.Count > 0)
            {
                Grd_PayCat.Columns[0].Visible = true;
                Grd_PayCat.DataSource = MyHead;
                Grd_PayCat.DataBind();
                Grd_PayCat.Columns[0].Visible = false;
                Pnl_PayCat.Visible = true;

            }
            else
            {
                Grd_PayCat.DataSource = null;
                Grd_PayCat.DataBind();
            }
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            clear();
            ChkBox_AllHead.Items.Clear();
            AddHeadToList();
            Btn_Add.Visible = true;
            Btn_Save.Visible = false;
            LoadGrid();
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            bool Message = false;
            OdbcDataReader MyPayReader = null;
            OdbcDataReader MyPayReader1 = null;
            OdbcDataReader My_empreader = null;
           
            Btn_Cancel.Visible = false;
            string _CatName = Txt_Cat.Text.Trim();
            string _BasicP = Txt_BasicPay.Text.Trim();
            string _WagesType = "";
            if (Rdb_Daily.Checked)
            {
                _WagesType = "Daily";
            }
            else
            {
                _WagesType = "Monthly";
            }
            int _Id = int.Parse(Grd_PayCat.Rows[Grd_PayCat.SelectedIndex].Cells[0].Text);
            if (Mypay.IsUpdatePresentCategory(_CatName, _Id))
            {
                WC_MessageBox.ShowMssage(" Category Name Already Present");
            }
            else
            {
                Mypay.CreateTansationDb();
                try
                {
                    Mypay.SaveCat(_Id, _CatName, _BasicP, _WagesType,MyUser.UserName);
                    //Mypay.DeleteChk(_Id);
                    for (int i = 0; i < ChkBox_AllHead.Items.Count; i++)
                    {
                        //if (!AlredyUsedHead(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), _Id))
                        //{
                        //}
                        if (ChkBox_AllHead.Items[i].Selected)
                        {
                            MyPayReader = Mypay.CheckHeadCatPresent(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), _Id);
                            if (!MyPayReader.HasRows)
                            {
                                Message = true;
                                Mypay.AddHeadCatMap(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), _Id);
                                MyPayReader1 = Mypay.GetEmpFrmMap(_Id);
                                if (MyPayReader1.HasRows)
                                {
                                    while (MyPayReader1.Read())
                                    {
                                        string EmpId = MyPayReader1.GetValue(0).ToString();
                                        My_empreader = Mypay.EmployeeAlreadymapped(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), EmpId, int.Parse(ChkBox_AllHead.Items[i].Value.ToString()));
                                        if (!My_empreader.HasRows)
                                        {
                                            string _EmpId = MyPayReader1.GetValue(0).ToString();
                                            Mypay.InsertEmpHeadMap(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), _Id, _EmpId);
                                        }
                                    }
                                }

                            }

                            
                        }
                        else
                        {
                            MyPayReader = Mypay.CheckHeadCatPresent(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()), _Id);
                            if (MyPayReader.HasRows)
                            {

                                Mypay.DeleteMappedHead(int.Parse(ChkBox_AllHead.Items[i].Value.ToString()),_Id);
                            }

                        }
                        ChkBox_AllHead.Items[i].Selected = false;
                        ChkBox_AllHead.Items.Remove(ChkBox_AllHead.Items[i]);
                        i--;

                    }
                    Mypay.EndSucessTansationDb();

                    Mypay.CreateTansationDb();

                    Mypay.UpdatePayEmployee(_Id);
                    Mypay.EndSucessTansationDb();

                    
                    if (Message)
                    {

                        WC_MessageBox.ShowMssage(" Some new heads have been added, These heads will show for all the employees under the seleted category ");
                    }

                   
                }
                catch
                {

                    Mypay.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Updation Failed,Please try later!");
                }

            }
            LoadGrid();
            clear();
            ChkBox_AllHead.Items.Clear();
            AddHeadToList();
            Btn_Save.Visible = false;
            Btn_Add.Visible = true;
        }

        //private bool AlredyUsedHead(int HeadId,int CatId)
        //{
        //    bool exist = true;
        //    string sql = "SELECT * from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId = " + HeadId + " and tblpay_employeeheadmap.CategoryId = "+CatId+" ";
        //    m_MyReader=
        //    return exist;
        //}
        protected void Grd_Cat_Selectedindexchanged(object sender, EventArgs e)
        {
            Btn_Cancel.Visible = true;
            ChkBox_AllHead.Items.Clear();
            AddHeadToList();
            Btn_Add.Visible = false;
            Btn_Save.Visible = true;
            Grd_PayCat.Columns[0].Visible = true;
            Txt_Cat.Text = Grd_PayCat.Rows[Grd_PayCat.SelectedIndex].Cells[1].Text;
            Txt_BasicPay.Text = Grd_PayCat.Rows[Grd_PayCat.SelectedIndex].Cells[2].Text;
            string _TypeId = Grd_PayCat.Rows[Grd_PayCat.SelectedIndex].Cells[3].Text;
            if (_TypeId == "Daily")
            {
                Rdb_Daily.Checked = true;
                Rdb_Monthly.Checked = false;
            }
            else
            {
                Rdb_Daily.Checked = false;
                Rdb_Monthly.Checked = true;
            }
            OdbcDataReader MyPayReader = null;

            MyPayReader = Mypay.FillCheck(int.Parse(Grd_PayCat.Rows[Grd_PayCat.SelectedIndex].Cells[0].Text.ToString()));
            if (MyPayReader.HasRows)
            {
                while (MyPayReader.Read())
                {
                    for (int i = 0; i < ChkBox_AllHead.Items.Count; i++)
                    {
                        if (ChkBox_AllHead.Items[i].Value== MyPayReader.GetValue(1).ToString())
                        {
                            ChkBox_AllHead.Items[i].Selected = true;
                        }
                    }
                }
            }



            Grd_PayCat.Columns[0].Visible = false;


        }

        protected void Grd_Cat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete this Record ')");
            }
        }
        protected void Grd_Cat_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //int _CatID = int.Parse(Grd_PayCat.DataKeys[e.RowIndex].Value.ToString());
            int _CatID = int.Parse(Grd_PayCat.Rows[e.RowIndex].Cells[0].Text);
            string CatName = Grd_PayCat.Rows[e.RowIndex].Cells[1].Text.ToString();
            if (Mypay.NotCatFixed(_CatID))
            {
                Grd_PayCat.Columns[0].Visible = true;
                Mypay.DeleteCat(_CatID);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete payroll type", "payrolltype " + CatName + " is deleted", 1);
                clear();
                ChkBox_AllHead.Items.Clear();
                AddHeadToList();
                Btn_Save.Visible = false;
                Btn_Add.Visible = true;
                LoadGrid();
                Grd_PayCat.Columns[0].Visible = false;
            }
            else
            {
                WC_MessageBox.ShowMssage("This category is assigned to few employees, hence cannot be deleted");
            }
        }

        
       

    }
}
