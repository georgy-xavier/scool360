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
using WinBase;
namespace WinEr
{
    public partial class ViewBookHistory : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (Session["BookId"] == null)
            {
                Response.Redirect("LlbraryHome.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyLibMang = MyUser.GetLibObj();
            if (MyLibMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyLibMang.GetSubLibraryMangMenuString(MyUser.UserRoleId);
                    this.SubLibMenu.InnerHtml = _MenuStr;
                     LoadBookData();
                    CheckHistory();
                }
            }

        }

        private void CheckHistory()
        {
            if (MyLibMang.HistoryExists(Session["BookId"].ToString()))
            {
                LoadGrid();
            }
            else
            {
                Lbl_Message.Text = "No history found";
                Pnl_history.Visible = false;
            }
        }

        private void LoadGrid()
        {
            GrdBkHistory.Columns[0].Visible = true;
            string sql = "select Id,BookId, UserId, UserTypeId ,  TakenDate, DATE_FORMAT(ReturnedDate,'%d-%m-%Y') as ReturnedDate  ,round(FineAmount) as FineAmount  ,Comment from tblbookhistory where BookId='" + Session["BookId"].ToString() + "' and tblbookhistory.CurrentBatchId = " + MyUser.CurrentBatchId + " order by ReturnedDate Asc";
            MydataSet = MyLibMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                MydataSet = GetUsernameAndType(MydataSet);
                GrdBkHistory.DataSource = MydataSet;
                GrdBkHistory.DataBind();
            }
            else
            {
                GrdBkHistory.DataSource = null;
                GrdBkHistory.DataBind();
            }
            GrdBkHistory.Columns[0].Visible = false;
        }

        private DataSet GetUsernameAndType(DataSet MydataSet)
        {
            foreach (DataRow dr_values in MydataSet.Tables[0].Rows)
            {

                
                if (dr_values["UserTypeId"].ToString() == "1")
                {
                    dr_values["UserTypeId"]= "Student";
                    dr_values["UserId"] = MyLibMang.GetUserName("Student", dr_values["UserId"].ToString());
                }
                else
                {
                    dr_values["UserTypeId"] = "Staff";
                    dr_values["UserId"] = MyLibMang.GetUserName("Staff", dr_values["UserId"].ToString());
                }

    
            }
            return MydataSet;
        }

        private void LoadBookData()
        {
           string BookId =Session["BookId"].ToString();
           Lbl_BookName.Text = MyLibMang.GetBookName(BookId);
           Lbl_Status.Text = MyLibMang.GetbookData(BookId);
        }

        protected void Grd_Bkhistoy_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _message;
            int _HistoryID = int.Parse(GrdBkHistory.DataKeys[e.RowIndex].Value.ToString());
            MyLibMang.DeleteBookHistory(_HistoryID,out _message);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Library", "Deleted Book from History", 1);
            Lbl_Message.Text = _message;
            LoadGrid();

        }
        protected void Grd_Bkhistoy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Lbl_Message.Text = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure you want to delete the item ')");
            }
        }
    }
}
