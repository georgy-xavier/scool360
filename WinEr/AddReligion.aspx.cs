using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class AddReligion : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
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

            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(770))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadReligion();
                    LoadGrid();
                    Grd_Religion.Visible = true;
                }
            }
        }

        private void LoadGrid()
        {
            Lbl_ReligionErr.Text = "";
            Grd_Religion.Columns[0].Visible = true;
            MyDataSet = LoadReligion();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {

                Grd_Religion.DataSource = MyDataSet;
                Grd_Religion.DataBind();
            }
            else
            {
                Grd_Religion.DataSource = null;
                Grd_Religion.DataBind();
                Lbl_ReligionErr.Text = "No Religion found";
            }
            Grd_Religion.Columns[0].Visible = false;

        }

        private DataSet LoadReligion()
        {
            string sql = "select tblreligion.Id, tblreligion.Religion from tblreligion  order by tblreligion.Religion ASC ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return MyDataSet;
        }
        protected void Btn_AddReligion_click(object sendr, EventArgs e)
        {
            General MyGenObj = new General(MyStudMang.m_MysqlDb);
            int MaxId = MyGenObj.GetTableMaxId("tblreligion", "Id");
            if(Txt_Religion.Text!="")
            {
                string sql = "insert into tblreligion (tblreligion.Id,tblreligion.Religion) values(" + MaxId + ",'" + Txt_Religion.Text + "')";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Religion Manager", "" + Txt_Religion.Text + " Religion added", 1);
                Lbl_ReligionError.Text = "Religion successfully added";
                Txt_Religion.Text = "";
            }
            else
            {
                Lbl_ReligionError.Text = "Please enter religion name...";
            }

            LoadGrid();
        }
        protected void Grd_Religion_RowData_Delete(object sender, GridViewDeleteEventArgs e)
        {
            int _ReligionID = 0;
            Lbl_ReligionError.Text = "";
            int.TryParse(Grd_Religion.Rows[e.RowIndex].Cells[0].Text.ToString(), out _ReligionID);

            string sql = " select tblreligion.Id from tblreligion where tblreligion.Id=" + _ReligionID;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string sql_delete = "delete from tblreligion where tblreligion.Id=" + _ReligionID;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql_delete);
                LoadGrid();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Religion Manager", "Religion deleted", 1);
                WC_MessageBox.ShowMssage("Selected religion deleted successfully");
                

            }
            else
            {

                WC_MessageBox.ShowMssage("Selected Religion cannot deleted");
            }

        }
        protected void Grd_Religion_Category_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGrid();
            Grd_Religion.PageIndex = e.NewPageIndex;


        }
        protected void Grd_Religion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // LinkButton l = (LinkButton)e.Row.FindControl("lnk_DeleteCategory");
                ImageButton l = (ImageButton)e.Row.FindControl("ImgBtnDeleteReligion");
                l.Attributes.Add("onclick", "javascript:return " +
                     "confirm('Are you sure you want to delete the religion " +
                     DataBinder.Eval(e.Row.DataItem, "Religion") + " ')");
            }


        }
        protected void Grd_ReligionSelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            int ReliigonId = 0;
            int.TryParse(Grd_Religion.SelectedRow.Cells[0].Text.ToString(), out ReliigonId);
            string ReligionName = Grd_Religion.SelectedRow.Cells[1].Text.ToString();

            Lbl_ReligionID.Text = ReliigonId.ToString();
            Txt_EditReligionName.Text = ReligionName;

            MPE_EditReligion.Show();


        }

        private void clear()
        {
            Lbl_EditReliigon_Error.Text = "";

        }
        protected void Btn_UpdateReligion_Click(object sender, EventArgs e)
        {

            Lbl_EditReliigon_Error.Text = "";
            if (Txt_EditReligionName.Text.ToString().Trim() == "")
            {
                Lbl_EditReliigon_Error.Text = "Please enter the Religion Name";
            }
            else
            {
                string sql = "update tblreligion set tblreligion.Religion='" + Txt_EditReligionName.Text.ToString().Trim() + "' where tblreligion.Id=" + int.Parse(Lbl_ReligionID.Text.ToString().Trim());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Religion Manager", "" + Lbl_ReligionID.Text.ToString().Trim() + " Religion Updated", 1);
                Lbl_EditReliigon_Error.Text = "Religion Updated";
                MPE_EditReligion.Show();
            }
            LoadGrid();



        }




    }
}