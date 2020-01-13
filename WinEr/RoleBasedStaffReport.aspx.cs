using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class RoleBasedStaffReport : System.Web.UI.Page
    {
        //private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StaffManager MyStaffMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(504))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Img_Export.Enabled = false;
                    LoadAllRoleNamesToDropDown();
                }
            }
        }

        private void LoadAllRoleNamesToDropDown()
        {
            Drp_Role.Items.Clear();

            string sql = "select tblrole.RoleName, tblrole.Id from tblrole order by tblrole.Id asc";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL","0");
                Drp_Role.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Role.Items.Add(li);
                }
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No Roles Found","-1");
                Drp_Role.Items.Add(li);
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            string sql = "SELECT tbluser.SurName as StaffName, tblstaffdetails.Sex as Gender, tblstaffdetails.Address, tblrole.RoleName, tbluser.UserName, DATE_FORMAT( tblstaffdetails.Dob,'%d/%m/%Y') as DOB, DATE_FORMAT( tblstaffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate, tblstaffdetails.PhoneNumber as Phone from tblstaffdetails inner join tbluser on tblstaffdetails.UserId= tbluser.Id inner join tblrole on tbluser.RoleId= tblrole.Id";
           
            if (int.Parse(Drp_Role.SelectedValue) > 0)
            {
                sql = sql + " where tbluser.RoleId=" + Drp_Role.SelectedValue;
            }
            sql = sql + " order by tbluser.SurName";
            MyDataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_StaffDetails.PageIndex = 0;
                ViewState["StaffList"] = MyDataSet;
                Grd_StaffDetails.DataSource = MyDataSet;
                Grd_StaffDetails.DataBind();
                Img_Export.Enabled = true;
                Lbl_Message.Text = "";
                Pnl_StaffDetails.Visible = true;
            }
            else
            {
                Grd_StaffDetails.DataSource = null;
                Grd_StaffDetails.DataBind();
                Img_Export.Enabled = false;
                Lbl_Message.Text = "No Staff Exist";
                Pnl_StaffDetails.Visible = false;
            }
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = (DataSet)ViewState["StaffList"];
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(ExportDataSet, "RoleBasedStaffReport.xls"))
            //{
            //    Lbl_Message.Text = "This function need Ms office";
            //}
            string FileName = "RoleBasedStaffReport";
            string _ReportName = "RoleBased StaffReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }

        protected void Grd_StaffDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_StaffDetails.PageIndex = e.NewPageIndex;
            DataSet MyDataSetNew = (DataSet)ViewState["StaffList"];
            Grd_StaffDetails.DataSource = MyDataSetNew;
            Grd_StaffDetails.DataBind();
        }
    }
}
