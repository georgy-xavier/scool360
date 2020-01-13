using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class UserGroupMapReport : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private GroupManager MyGroup;


        protected void Page_Load(object sender, EventArgs e)
        {
             if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyGroup = MyUser.GetGroupManagerObj();
            if (MyGroup == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(911))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadGroupToDropdown();
                    Pnl_Show.Visible = false;
                }
            }
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            BindReportToGrid();

        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["MapReport"];
            if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "User-Group Map Report.xls"))
            {
                Lbl_Msg.Text = "";
            }  
        }

        protected void Grd_UserGroupMapReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_UserGroupMapReport.PageIndex = e.NewPageIndex;
            BindReportToGrid();
        }


        private void BindReportToGrid()
        {
            Lbl_Msg.Text = "";
            int groupid = 0, type = 0;
            int.TryParse(Drp_Group.SelectedValue, out groupid);
            int.TryParse(Drp_Type.SelectedValue, out type);
            DataSet GroupMapDetailsDs = new DataSet();
            GroupMapDetailsDs = MyGroup.GetUserMapDetails(groupid, type);
            if (GroupMapDetailsDs != null && GroupMapDetailsDs.Tables[0].Rows.Count > 0)
            {
                Lbl_total.Text = "Total count: " + GroupMapDetailsDs.Tables[0].Rows.Count.ToString(); ;
                Pnl_Show.Visible = true;
                Grd_UserGroupMapReport.DataSource = GroupMapDetailsDs;
                Grd_UserGroupMapReport.DataBind();
            }
            else
            {
                Lbl_total.Text = "";
                Lbl_Msg.Text = "No report found";
                Pnl_Show.Visible = false;
                Grd_UserGroupMapReport.DataSource = null;
                Grd_UserGroupMapReport.DataBind();
            }
            ViewState["MapReport"] = GroupMapDetailsDs;
            
        }

        private void LoadGroupToDropdown()
        {
            DataSet GroupDs = new DataSet();
            ListItem li;
            GroupDs = MyGroup.GetAllGroup();
            if (GroupDs != null && GroupDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Group.Items.Add(li);
                foreach (DataRow dr in GroupDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                    Drp_Group.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None","-1");
                Drp_Group.Items.Add(li);
            }
        }
    }
}
