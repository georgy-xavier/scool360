using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
public partial class ViewStaffs : System.Web.UI.Page
{

    private StaffManager MyStaffMang;
    private KnowinUser MyUser;
    //private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStaffMang = MyUser.GetStaffObj();
        if (MyStaffMang == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {


            if (!IsPostBack)
            {

                Pnl_stafflist.Visible = false;

                img_export_Excel.Visible = false;
                //some initlization

                Session["SortExpression"] = null;
                Session["SortDirection"] = null;
                Txt_Search1_AutoCompleteExtender.ContextKey = Drp_SearchBy.SelectedValue + "/" + MyUser.UserId;
            }
        }

    }
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
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

    protected void Btn_Search_Click(object sender, EventArgs e)
    {
        Grd_StaffList.PageIndex = 0;
        FillGrid();
    }

    private void FillGrid()
    {
        string sql;
        string moredtls = ", DATE_FORMAT(tblstaffdetails.Dob, '%d/%m/%y') as DOB, tblstaffdetails.Sex, tblstaffdetails.Address, tblstaffdetails.PhoneNumber,t.EmailId as EmailId, tblstaffdetails.EduQualifications,tblstaffdetails.AadharNo,tblstaffdetails.PanNo, ROUND(tblstaffdetails.Experience,1) as Experience,  DATE_FORMAT(tblstaffdetails.JoiningDate, '%d/%m/%y') as DateofJoining";//tblstaffdetails.ExpDescription,
        if (Txt_Search1.Text.Trim() == "")
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId`  where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }
        else if (Drp_SearchBy.SelectedValue == "0")
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND t.`UserName` = '" + Txt_Search1.Text + "' AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }

        else if (Drp_SearchBy.SelectedValue == "1")
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND t.`SurName` = '" + Txt_Search1.Text + "' AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }

        else if (Drp_SearchBy.SelectedValue == "2")
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblstaffsubjectmap s on s.`StaffId`=t.`Id` inner join tblsubjects sb on sb.`Id`=s.`SubjectId` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND sb.`subject_name`='" + Txt_Search1.Text + "' AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }
        else if (Drp_SearchBy.SelectedValue == "3")
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id`  inner join tblstaffdetails sf on sf.`UserId`=t.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND sf.`Designation`='" + Txt_Search1.Text + "' AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }
        else if (Drp_SearchBy.SelectedValue == "4")
        {
            double _experience;
            bool _valide = double.TryParse(Txt_Search1.Text.ToString(), out _experience);
            if (_valide)
            {
                sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + ", t.`CanLogin`  as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id`  inner join tblstaffdetails sf on sf.`UserId`=t.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND sf.`Experience`>=" + _experience + " AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
            }
            else
            {
                sql = "SELECT * FROM tbluser where Id=-2"; ;
            }
        }
        else if (Drp_SearchBy.SelectedValue == "5" )
        {
            double _experience;
            bool _valide  = double.TryParse(Txt_Search1.Text.ToString(),out _experience );
            if (_valide)
            {
                sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin` as CanLogin FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id`  inner join tblstaffdetails sf on sf.`UserId`=t.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND sf.`Experience`<=" + _experience + " AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
            }
            else
            {
                sql = "SELECT * FROM tbluser where Id=-2";
            }
        }
        else
        {
            sql = "SELECT DISTINCT t.`Id`,t.`SurName`, t.`UserName`,r.`RoleName` " + moredtls + " , t.`CanLogin` as CanLogin  FROM tbluser t  inner join tblstaffdetails on tblstaffdetails.UserId = t.Id  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
        }
        MydataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_StaffList.Columns[1].Visible = true;
            DataTable dtAgent = MydataSet.Tables[0];
            DataView dataView = new DataView(dtAgent);
            if (Session["SortDirection"] != null && Session["SortExpression"] != null)
            {
                dataView.Sort = (string)Session["SortExpression"] + " " + (string)Session["SortDirection"];
            }

            Grd_StaffList.DataSource = MydataSet;
            Grd_StaffList.DataBind();

            Grd_StaffList.Columns[1].Visible = false;
            FillStaffDeatils();
            Pnl_stafflist.Visible = true;
            img_export_Excel.Visible = true;
            ViewState["Stafflist"] = MydataSet;
        }
        else
        {
            Grd_StaffList.DataSource = null;
            Grd_StaffList.DataBind();
            ViewState["Stafflist"] = null;
            WC_MessageBox.ShowMssage("No Staffs found");
            Pnl_stafflist.Visible = false;

            img_export_Excel.Visible = false;
            
        }
    }

    private void FillStaffDeatils()
    {
        foreach (GridViewRow gv in Grd_StaffList.Rows)
        {
            Image Img_staffImage = (Image)gv.FindControl("Img_staffImage");
            Img_staffImage.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[1].Text.ToString()) + "&type=StaffImage";
            //    MyUser.GetImageUrl("StaffImage", int.Parse(gv.Cells[1].Text.ToString()));
        }
    }
    protected void Grd_StaffList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i_SelectedStaffId = int.Parse(Grd_StaffList.SelectedRow.Cells[1].Text.ToString());
        Session["StaffId"] = i_SelectedStaffId;
        Response.Redirect("StaffDetails.aspx");
    }
    protected void Grd_StaffList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
            }
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_StaffList, "Select$" + e.Row.RowIndex);
        }

    }
  
    protected void Grd_StaffList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grd_StaffList.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void Grd_StaffList_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grd_StaffList.Columns[1].Visible = true;
        Grd_StaffList.PageIndex = 0;

        DataSet MydataSet = (DataSet)ViewState["Stafflist"];


        DataTable dtCust = MydataSet.Tables[0];
        DataView dataView = new DataView(dtCust);

        dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
        Grd_StaffList.DataSource = dataView;
        Grd_StaffList.DataBind();
        FillStaffDeatils();
        Grd_StaffList.Columns[1].Visible = false;
    }

    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = Session["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = Session["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        Session["SortDirection"] = sortDirection;
        Session["SortExpression"] = column;

        return sortDirection;
    }
    protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
    {
        MydataSet = (DataSet)ViewState["Stafflist"];
        MydataSet.Tables[0].Columns.Add("Subject Handled");
        MydataSet.Tables[0].Columns.Add("Login Permission");
        foreach (DataRow dr in MydataSet.Tables[0].Rows)
        {
            dr["Subject Handled"] = MyStaffMang.GetHandledSubject(dr["Id"].ToString());

            if( dr["CanLogin"].ToString()=="1")
                dr["Login Permission"] ="Yes";
            else
                dr["Login Permission"] = "No";
        }
        MydataSet.Tables[0].Columns.Remove("Id");
        MydataSet.Tables[0].Columns.Remove("CanLogin");
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Staffs List.xls"))
            //{
            //}

            string FileName = "Staffs-List";
            string _ReportName = "Staffs-List";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
            }
        }
    }

    protected void Drp_SearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Search1_AutoCompleteExtender.ContextKey = Drp_SearchBy.SelectedValue + "/" + MyUser.UserId;
    }
}
