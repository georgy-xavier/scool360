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
using WinBase;

namespace WinEr
{
    public partial class WebForm17 : System.Web.UI.Page
    {
        private WinErSearch MySearchMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
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

        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(48))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                    Pnl_stafflist.Visible = false;
                    //some initialisations

                }
            }

        }

  
   
   
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i_SelectedStaffid = int.Parse(Grd_Staff.SelectedRow.Cells[1].Text.ToString());
            Response.Redirect("StaffSearchDetails.aspx?StaffId=" + i_SelectedStaffid + "");
        }
        protected void Grd_Staff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Staff.PageIndex = e.NewPageIndex;  
            FillGrid();
        }
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WinSchoolHome.aspx");
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (Txt_StaffName.Text != "")
            {
                FillGrid();
            }
            else
            {
                Grd_Staff.DataSource = null;
                Grd_Staff.DataBind();
                WC_MessageBox.ShowMssage("No staffs found");
               
                Pnl_stafflist.Visible = false;
            }
        }

        private void FillGrid()
        {
            string StaffName,sql="";
            StaffName = Txt_StaffName.Text.Trim();

            
                   Grd_Staff.Columns[0].Visible = true;
                   sql = "SELECT DISTINCT tbluser_history.Id, tbluser_history.SurName, tblrole.RoleName as Designation  FROM tbluser_history INNER JOIN tblrole ON tbluser_history.RoleId = tblrole.Id AND tblrole.Type!='General' AND tbluser_history.SurName LIKE '%" + StaffName + "%'";

                     
                    MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                   if (MydataSet.Tables[0].Rows.Count > 0)
                     {
                            Grd_Staff.DataSource = MydataSet;
                            Grd_Staff.DataBind();
                            Grd_Staff.Columns[0].Visible = false;
                            FillStaffDeatils();
                            Pnl_stafflist.Visible = true;
                     }
                    else
                     {
                            Grd_Staff.DataSource = null;
                            Grd_Staff.DataBind();
                            WC_MessageBox.ShowMssage("No staffs found");
                            
                            Pnl_stafflist.Visible = false;
                     }
             
          

        }

        private void FillStaffDeatils()
        {

            foreach (GridViewRow gv in Grd_Staff.Rows)
            {

                Image Img_staffImage = (Image)gv.FindControl("Img_staffImage");

                Img_staffImage.ImageUrl = Get_staff_History_ImageUrl("StaffImage", int.Parse(gv.Cells[1].Text.ToString()));
                
                
            }
        }
        private string Get_staff_History_ImageUrl(string Type, int StaffId)
        {
            string ImageUrl = "images/" + "img.png";
            string Sql = "SELECT tblfileurl_history.FilePath FROM tblfileurl_history WHERE tblfileurl_history.Type='" + Type + "' AND tblfileurl_history.UserId=" + StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                ImageUrl =  WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/" + MyReader.GetValue(0).ToString();
            }
            else
            {
                ImageUrl =  "images/" + "img.png";
            }
            return ImageUrl;
        }
        protected void Grd_Staff_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Staff, "Select$" + e.Row.RowIndex);
            }

        }

    }
}
