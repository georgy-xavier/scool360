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
    public partial class CreateHouse : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private WinBase.HouseManager MyHouse;
        private KnowinUser MyUser;
        private OdbcDataReader Myreader = null;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfigMang = MyUser.GetConfigObj();
                MyHouse = MyUser.GetHouseObj();
                if (MyConfigMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(901))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        Lbl_err.Text = "";
                        LoadAllHouseToGrid();
                        Btn_Add.Visible = true;
                        Btn_Update.Visible = false;
                    }
                }
            }
       
            protected void Btn_Add_Click(object sender, EventArgs e)
            {
                Lbl_err.Text = "";
                try
                {
                    if (Txt_Housename.Text == "")
                    {
                        Lbl_err.Text = "Enter house name";
                    }
                    else
                    {
                        bool exist = false;
                        int Id=0;
                        if (!MyHouse.HouseExist(Txt_Housename.Text, out exist, out Id))
                        {
                            if (exist)
                            {
                                MyHouse.AddNewHouse(Txt_Housename.Text, exist, Id);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "New house:"+Txt_Housename.Text+" added", 1);
                                Lbl_err.Text = "New house added successfully!";
                            }
                            else
                            {
                                MyHouse.AddNewHouse(Txt_Housename.Text, exist, Id);
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "New house:" + Txt_Housename.Text + " added", 1);
                                Lbl_err.Text = "New house added successfully!";
                            }
                            Txt_Housename.Text = "";
                            LoadAllHouseToGrid();
                        }
                        else
                        {
                            Lbl_err.Text = "House name already exist,Cannot add!";
                        }
                    }
                }
                catch
                {
                    Lbl_err.Text = "Cannot add,Please try again later!";
                }
            }

            protected void Grd_House_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                int Id = 0;
                Lbl_err.Text = "";
                try
                {
                    int.TryParse(Grd_House.Rows[e.RowIndex].Cells[0].Text, out Id);
                    if (!HouseMapped(Id))
                    {
                        MyHouse.DeleteLocation(Id);
                        LoadAllHouseToGrid();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "house deleted", 1);
                        Lbl_err.Text = "Deleted successfully!";
                        Txt_Housename.Text = "";
                        Btn_Add.Visible = true;
                        Btn_Update.Visible = false;
                    }
                    else
                    {
                        Lbl_err.Text = "Can't delete,House is mapped to some students!";
                    }
                }
                catch
                {
                    Lbl_err.Text = "Can't delete,Please try again later!";
                }
                
                //MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Location", "Location " + _locationname + " is deleted", 1);

            }

            protected void Grd_House_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                    l.Attributes.Add("onclick", "javascript:return " +
                         "confirm('Are you sure you want to delete this Record ')");
                }
            }

            protected void Grd_House_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_House.PageIndex = e.NewPageIndex;
                LoadAllHouseToGrid();

            }

            protected void Grd_House_SelectedIndexChanged(object sender, EventArgs e)
            {
                Lbl_err.Text = "";
                Btn_Add.Visible = false;
                Btn_Update.Visible = true;
                Txt_Housename.Text = Grd_House.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
                Hdn_HouseId.Value = Grd_House.SelectedRow.Cells[0].Text.Replace("&nbsp;", "");
            }

            protected void Btn_Update_Click(object sender, EventArgs e)
            {
                Lbl_err.Text = "";
                string sql = "";
                sql = "UPDATE tblhousemaster SET HouseName='" + Txt_Housename.Text + "' WHERE Id=" + Hdn_HouseId.Value + "";
                MyHouse.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "House", "house :" + Txt_Housename.Text + " updated", 1);
                Lbl_err.Text = "House name updated successfully!";
                Txt_Housename.Text = "";
                Hdn_HouseId.Value = "";
                Btn_Add.Visible = true;
                Btn_Update.Visible = false;
                LoadAllHouseToGrid();
            }

        #endregion

        #region Methods

            private void LoadAllHouseToGrid()
            {
                DataSet HouseDs = new DataSet();
                try
                {
                    HouseDs = MyHouse.GetAllActiveHouse();
                    if (HouseDs != null && HouseDs.Tables[0].Rows.Count > 0)
                    {
                        Grd_House.Columns[0].Visible = true;
                        Pnl_Housedisplay.Visible = true;
                        Grd_House.DataSource = HouseDs;
                        Grd_House.DataBind();
                        Grd_House.Columns[0].Visible = false;

                    }
                    else
                    {
                        Grd_House.DataSource = null;
                        Grd_House.DataBind();
                        Pnl_Housedisplay.Visible = false;
                    }

                }
                catch (Exception ex)
                {
                    Lbl_err.Text = "Cannot load location";
                }
            }

            private bool HouseMapped(int Id)
            {
                string sql = "";
                bool valid = false;
                OdbcDataReader Mappingreader = null;
                sql = "select id from  tblhousestudentmap where HouseId=" + Id + "";
                Mappingreader = MyHouse.m_MysqlDb.ExecuteQuery(sql);
                if (Mappingreader.HasRows)
                {
                    valid = true;
                }
                return valid;

            }

        #endregion
    }
}
