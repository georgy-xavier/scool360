using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;

namespace WinEr
{
    public partial class AdvFeeSettlement : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
 
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(891))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {

                    LoadDetails();
                    //some initlization

                }
            }
        }

        private void LoadDetails()
        {
            lbl_msg.Text = "";
            DataSet GridData = LoadStudeneAdvanceDetails();
            if (GridData != null && GridData.Tables[0].Rows.Count > 0)
            {
                Grd_advance.Columns[1].Visible = true;
                Grd_advance.DataSource = GridData;
                Grd_advance.DataBind();
                Grd_advance.Columns[1].Visible = false;

            }
            else
            {
                Grd_advance.DataSource = null;
                Grd_advance.DataBind();
                lbl_msg.Text = "No student advance found";
            }
        }

        private DataSet LoadStudeneAdvanceDetails()
        {
            DataSet _Studentdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Studentdataset.Tables.Add(new DataTable("students"));
            dt = _Studentdataset.Tables["students"];
            dt.Columns.Add("StudentId");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("AvailableAdvance");
            dt.Columns.Add("DueAmount");

            string sql = "SELECT tblstudentfeeadvance.StudentId,tblstudentfeeadvance.StudentName,tblclass.ClassName,Sum(tblstudentfeeadvance.Amount) as amount,(select SUM(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId  inner join tblfeeaccount on tblfeeaccount.Id = tblfeeschedule.FeeId where tblfeestudent.StudId=tblstudentfeeadvance.StudentId and tblfeeaccount.Status=1 and tblfeestudent.Status<>'Paid' and tblfeestudent.Status<>'fee Exemtion' and tblfeeschedule.DueDate <= CURRENT_DATE()) as due  FROM tblstudentfeeadvance INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblstudentfeeadvance.StudentId INNER JOIN tblclass ON tblstudentclassmap.ClassId=tblclass.Id Group By StudentId ORDER BY due,tblclass.Standard,tblclass.ClassName,tblstudentfeeadvance.StudentName";
            DataSet _newData = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_newData != null && _newData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drow in _newData.Tables[0].Rows)
                {

                    dr = _Studentdataset.Tables["students"].NewRow();

                    dr["StudentId"] = drow[0].ToString();
                    dr["StudentName"] = drow[1].ToString();
                    dr["ClassName"] = drow[2].ToString();
                    dr["AvailableAdvance"] = drow[3].ToString();
                    string DueAmount = "0";
                    if (drow[4].ToString() != "")
                    {
                        DueAmount = drow[4].ToString();
                    }
                    dr["DueAmount"] = DueAmount;

                    _Studentdataset.Tables["students"].Rows.Add(dr);

                }
            }
            return _Studentdataset;
        }



        protected void Btn_Settle_Click(object sender, EventArgs e)
        {
            string _msg = "";
            if (IsSettlementPossible(out _msg))
            {
                try
                {
                    MyFeeMang.CreateTansationDb();

                    foreach (GridViewRow gv in Grd_advance.Rows)
                    {
                        CheckBox _chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                        if (_chk.Checked)
                        {
                            string StudentId = gv.Cells[1].Text.ToString();
                            string DueAmount = gv.Cells[5].Text.ToString();
                            int _StudentId = 0;
                            double _DueAmount = 0;
                            int.TryParse(StudentId, out _StudentId);
                            double.TryParse(DueAmount, out _DueAmount);
                            if (_StudentId > 0)
                            {
                                if (_DueAmount > 0)
                                {
                                    MyFeeMang.SettleAdvance_WithDueFees(StudentId);


                                }
                            }
                        }
                    }
                    MyFeeMang.EndSucessTansationDb();
                    LoadDetails();
                    WC_MessageBox.ShowMssage("Settlement done for due amounts ", MSGTYPE.Message);
                }
                catch (Exception Ex)
                {
                    MyFeeMang.EndFailTansationDb();
                    WC_MessageBox.ShowMssage("Error Message : " + Ex.Message, MSGTYPE.Alert);
                }

            }
            else
            {
                WC_MessageBox.ShowMssage(_msg, MSGTYPE.Alert);
            }
         
            
        }

        private bool IsSettlementPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true,IsChecked=false;
            foreach (GridViewRow gv in Grd_advance.Rows)
            {

                CheckBox _chk = (CheckBox)gv.FindControl("CheckBoxUpdate");
                if (_chk.Checked)
                {
                    IsChecked = true;
                    break;
                }
            }

            if (!IsChecked)
            {
                _valid = false;
                _msg = "Please select student";
            }

            return _valid;
        }



        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdvFeeSettlement.aspx");
        }

        protected void Grd_advance_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StudentId = Grd_advance.SelectedRow.Cells[1].Text.ToString();
            string DueAmount = Grd_advance.SelectedRow.Cells[5].Text.ToString();
            int _StudentId = 0;
            double _DueAmount = 0;
            int.TryParse(StudentId, out _StudentId);
            double.TryParse(DueAmount, out _DueAmount);
            if (_StudentId > 0)
            {
                if (_DueAmount > 0)
                {
                    try
                    {
                        MyFeeMang.CreateTansationDb();


                        MyFeeMang.SettleAdvance_WithDueFees(StudentId);

                        MyFeeMang.EndSucessTansationDb();
                        LoadDetails();
                        WC_MessageBox.ShowMssage("Settlement done for due amounts ", MSGTYPE.Message);
                    }
                    catch(Exception Ex)
                    {
                        MyFeeMang.EndFailTansationDb();
                        WC_MessageBox.ShowMssage("Error Message : "+Ex.Message, MSGTYPE.Alert);
                    }
                }
                else
                {
                    WC_MessageBox.ShowMssage("No due to be settled", MSGTYPE.Alert);
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("Error while selecting student",MSGTYPE.Alert);
            }
        }

        protected void Grd_advance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_advance.PageIndex = e.NewPageIndex;
            LoadDetails();
        }
    }
}
