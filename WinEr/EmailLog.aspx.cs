using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;


namespace WinEr
{
    public partial class EmailLog : System.Web.UI.Page
    {


        private StudentManagerClass MyStudMang;
        private EmailManager Obj_Email;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(877))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Rdb_Type.SelectedValue = "0";
                    loadstaff();
                    LoadRowType();
                    LoadParentDropDown();
                   // LoadParent();
                   // Pnl_LogDisplay.Visible = false;
                    LoadGridValues(1);
                }
                Lbl_Msg.Text = "";
            }
        }

        private void LoadParentDropDown()
        {
            string sql = "";
            Drp_Parent.Items.Clear();
            OdbcDataReader Studentreader = null;
            ListItem li;
            string _sql = "";
            DataSet ParentDs = new DataSet();
            sql = "select tblstudent.StudentName,Id from  tblstudent where  tblstudent.Id in(select tbl_emaillog.SenderId from tbl_emaillog where tbl_emaillog.Type=2)";
            ParentDs = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            //if (ParentDs != null && ParentDs.Tables[0].Rows.Count > 0)
            //{
            //    li = new ListItem("All", "0");
            //    Drp_Parent.Items.Add(li);
            //    foreach (DataRow dr in ParentDs.Tables[0].Rows)
            //    {
            //        li = new ListItem(dr["StudentName"].ToString(), dr["Id"].ToString());
            //        Drp_Parent.Items.Add(li);
            //    }
            //}
            _sql = "select tbltempstdent.Name,Id from  tbltempstdent where  tbltempstdent.Id in(select tbl_emaillog.SenderId from tbl_emaillog where tbl_emaillog.Type=4)";
            if (ParentDs != null && ParentDs.Tables[0].Rows.Count > 0)
            {
                Studentreader = Obj_Email.m_MysqlDb.ExecuteQuery(_sql);
                if (Studentreader.HasRows)
                {                   
                    int i = ParentDs.Tables[0].Rows.Count;
                    DataRow dr;
                    while (Studentreader.Read())
                    {
                        i++;
                       dr= ParentDs.Tables[0].NewRow();
                       // dr = MyDataSet.Tables["RouteDestination"].NewRow();
                       dr["StudentName"] = Studentreader.GetValue(0).ToString();
                       dr["Id"] = Studentreader.GetValue(1).ToString();
                       ParentDs.Tables[0].Rows.Add(dr);
                    }
                }
                if (ParentDs != null && ParentDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Parent.Items.Add(li);
                    foreach (DataRow dr in ParentDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["StudentName"].ToString(), dr["Id"].ToString());
                        Drp_Parent.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Parent.Items.Add(li);
                }
            }
            else
            {
                ParentDs = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                if (ParentDs != null && ParentDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Parent.Items.Add(li);
                    foreach (DataRow dr in ParentDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Name"].ToString(), dr["Id"].ToString());
                        Drp_Parent.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Parent.Items.Add(li);
                }
            }
        }
    
        private void loadstaff()
        {
            Drp_Staff.Items.Clear();
            ListItem li;
            string sql = "SELECT tbluser.Id,tbluser.UserName FROM tbluser WHERE tbluser.`Status`=1 AND tbluser.RoleId!=1 ORDER BY tbluser.UserName";
            MyDataSet = Obj_Email.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Staff.Items.Add(li);
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Staff.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Staff Found", "-2");
                Drp_Staff.Items.Add(li);
            }
            Drp_Staff.SelectedIndex = 0;
            
        }

        protected void Rdb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRowType();
                int type;
            if (Rdb_Type.SelectedValue == "0")
            {
                type=1;
            }
            else
            {
                type = 2;
            }
            LoadGridValues(type);
        }

        private void LoadRowType()
        {
            if (int.Parse(Rdb_Type.SelectedValue) == 0)
            {
                RowStaff.Visible = true;
                RowParent.Visible = false;
            }
            else if (int.Parse(Rdb_Type.SelectedValue) == 1)
            {
                RowStaff.Visible = false;
                RowParent.Visible = true;
            }
        }

        protected void Drp_Staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridValues(1);
        }

        private void LoadGridValues(int type)
        {
            DataSet LogDs = new DataSet();

            if (type== 1)
            {
                Grd_EmailLog.DataSource = null;
                Grd_EmailLog.DataBind();
                LogDs = Obj_Email.GetStaffLogDetails(int.Parse(Drp_Staff.SelectedValue));
                if (LogDs != null && LogDs.Tables[0].Rows.Count > 0)
                {
                    //LogDs = GetCorrectDs(LogDs);
                    Grd_EmailLog.Columns[1].Visible = true;
                    Grd_EmailLog.DataSource = LogDs;
                    Grd_EmailLog.DataBind();
                    Pnl_LogDisplay.Visible = true;
                    Grd_EmailLog.Columns[1].Visible = false;
                }
                else
                {
                    Grd_EmailLog.DataSource = null;
                    Grd_EmailLog.DataBind();
                    Pnl_LogDisplay.Visible = false;
                    Lbl_Msg.Text = "No log exist";
                }
            }
            else
            {
                Grd_EmailLog.DataSource = null;
                Grd_EmailLog.DataBind();
                LogDs = Obj_Email.GetParentLogDetails(Drp_Parent.SelectedValue);
                if (LogDs != null && LogDs.Tables[0].Rows.Count > 0)
                {
                   // LogDs = GetCorrectDs(LogDs);
                    Grd_EmailLog.DataSource = LogDs;
                    Grd_EmailLog.Columns[1].Visible = true;
                    Grd_EmailLog.DataBind();
                    Pnl_LogDisplay.Visible = true;
                    Grd_EmailLog.Columns[1].Visible = false;
                }
                else
                {
                    Grd_EmailLog.DataSource = null;
                    Grd_EmailLog.DataBind();
                    Pnl_LogDisplay.Visible = false;
                    Lbl_Msg.Text = "No log exist";
                }
            }
          
            
        }

        private DataSet GetCorrectDs(DataSet LogDs)
        {
            LogDs.Tables[0].Columns.Add("correctStatus");
            

           // XmlDocument doc = new XmlDocument();
            //string StrXml = Xmlreader.GetValue(1).ToString();
            //DataSet ReaderDs = new DataSet();

            //doc.LoadXml(StrXml);
            //XmlNodeReader Nodereader = new XmlNodeReader(doc);
            //Nodereader.MoveToContent();
            //while (Nodereader.Read())
            //{
            //    ReaderDs.ReadXml(Nodereader);

            //}
           

            foreach (DataRow dr in LogDs.Tables[0].Rows)
            {

                if (int.Parse(dr["SendStatus"].ToString()) == 0)
                {
                    dr["correctStatus"] = "Waiting";
                }
                else if (int.Parse(dr["SendStatus"].ToString()) == 1)
                {
                    dr["correctStatus"] = "Sent";
                }

            }
            return LogDs;
            
        }

        protected void Drp_Parent_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Type = "";
            LoadGridValues(2);
        }

        protected void Link_SelectAll_Click(object sender, EventArgs e)
        {
            if (Link_SelectAll.Text == "Select All")
            {
                Link_SelectAll.Text = "None";
                SelectStaffGrid();
            }
            else
            {
                Link_SelectAll.Text = "Select All";
                UnselectStaffGrid();
            }
        }

        private void UnselectStaffGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_EmailLog.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = false;
            }
        }

        private void SelectStaffGrid()
        {
            CheckBox Chk_Select;
            foreach (GridViewRow gv in Grd_EmailLog.Rows)
            {
                Chk_Select = (CheckBox)gv.FindControl("Checksms");
                Chk_Select.Checked = true;
            }
        }

        protected void Grd_EmailLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_EmailLog.Columns[1].Visible = true;
            StringBuilder Stb_EmailBody = new StringBuilder();
            int Id =int.Parse(Grd_EmailLog.SelectedRow.Cells[1].Text);
            string sql = "";
            sql = "select tbl_emaillog.EmailBody from tbl_emaillog WHERE tbl_emaillog.Id=" + Id + "";
            MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Stb_EmailBody.Append(MyReader.GetValue(0).ToString());
            }
            Grd_EmailLog.Columns[1].Visible = false;
            EmailBody.InnerHtml = Stb_EmailBody.ToString();
            MpE_ShowEmailBody.Show();
        }

        
    }
}
