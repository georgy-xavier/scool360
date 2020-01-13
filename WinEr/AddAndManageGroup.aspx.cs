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
    public partial class AddAndManageGroup : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private GroupManager MyGroup;

        #region Events

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
                else if (!MyUser.HaveActionRignt(909))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        LoadGroup();
                        Btn_Add.Visible = true;
                        Btn_Update.Visible = false;
                        Pnl_AddNewGroup.Visible = false;
                        Lnk_AddNewGroup.Visible = true;
                        Img_AddUser.Visible = true;
                    }
                }
            }
        protected void Grd_Group_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Group.PageIndex = e.NewPageIndex;
            LoadGroup();
        }

        protected void Grd_Group_SelectedIndexChanged(object sender, EventArgs e)
            {
                Txt_GroupName.Text = "";
                Btn_Add.Visible = false;
                Btn_Update.Visible = true;
                Lnk_AddNewGroup.Visible = true;
                Img_AddUser.Visible = true;
                Pnl_AddNewGroup.Visible = true;
                Chk_AddMoreGroup.Visible = false;
                Grd_Group.Columns[0].Visible = true;
                Txt_GroupName.Text = Grd_Group.SelectedRow.Cells[1].Text.Replace("&nbsp;","");
                Txt_GroupDescription.Text = Grd_Group.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
                Hdn_GroupId.Value = Grd_Group.SelectedRow.Cells[0].Text;
                Grd_Group.Columns[0].Visible = false;
            }

            protected void Btn_Update_Click(object sender, EventArgs e)
            {
                string sql = "";
                try
                {
                    if (Txt_GroupName.Text != "")
                    {
                        if (!MyGroup.GroupExist(Txt_GroupName.Text, Hdn_GroupId.Value))
                        {
                            sql = "Update tbl_gr_master set GroupName='" + Txt_GroupName.Text + "',Description='"+Txt_GroupDescription.Text+"' where id=" + Hdn_GroupId.Value + "";
                            MyGroup.m_MysqlDb.ExecuteQuery(sql);
                            LoadGroup();
                            Hdn_GroupId.Value = "";
                            Btn_Add.Visible = true;
                            Btn_Update.Visible = false;
                            Txt_GroupName.Text = "";
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "Group:" + Txt_GroupName.Text + " updated", 1);
                            WC_MessageBox.ShowMssage("Group name updated successfully");
                            Pnl_AddNewGroup.Visible = false;
                            Lnk_AddNewGroup.Visible = true;
                            Img_AddUser.Visible = true;
                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("Cannot update,Group name already exist!");
                        }
                    }
                }
                catch(Exception)
                {
                    WC_MessageBox.ShowMssage("Cannot delete,Please try later!");
                }

            }

            protected void Grd_Group_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                int Id = 0;
                try
                {
                    int.TryParse(Grd_Group.Rows[e.RowIndex].Cells[0].Text, out Id);
                    if (!MyGroup.UserMappedToGroup(Id))
                    {
                        MyGroup.DeleteGroup(Id);
                        LoadGroup();
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "Group deleted", 1);
                        WC_MessageBox.ShowMssage("Deleted successfully!");
                        Txt_GroupName.Text = "";
                        Btn_Add.Visible = true;
                        Btn_Update.Visible = false;
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Can't delete,Some users mapped to this group!");
                    }
                }
                catch
                {
                    WC_MessageBox.ShowMssage("Can't delete,Please try again later!");
                }

                //MyUser.m_DbLog.LogToDb(MyUser.UserName, "Delete Location", "Location " + _locationname + " is deleted", 1);

            }

            protected void Btn_Add_Click(object sender, EventArgs e)
            {
                string groupname = "",description="";
                groupname = Txt_GroupName.Text;
                description = Txt_GroupDescription.Text;
                try
                {
                    if (groupname == "")
                    {
                        WC_MessageBox.ShowMssage("Enter group name");
                    }
                    else
                    {
                        if (!MyGroup.GroupExist(groupname, ""))
                        {
                            AddGroup(groupname,description);
                            LoadGroup();
                            WC_MessageBox.ShowMssage("Group added successfully!");
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group", "Group:" + groupname + " added", 1);
                            Txt_GroupName.Text = "";
                            Txt_GroupDescription.Text = "";
                            if (Chk_AddMoreGroup.Checked)
                            {
                                Pnl_AddNewGroup.Visible = true;
                                Lnk_AddNewGroup.Visible = false;
                                Img_AddUser.Visible = false;
                            }
                            else
                            {
                                Pnl_AddNewGroup.Visible = false;
                                Lnk_AddNewGroup.Visible = true;
                                Img_AddUser.Visible = true;
                            }
                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("Cannot add,Group name already exist!");
                        }
                    }
                }
                catch (Exception)
                {
                    WC_MessageBox.ShowMssage("Cannot Add,Please try later!");
                }
            }

            protected void Lnk_AddNewGroup_Click(object sender, EventArgs e)
            {
                Pnl_AddNewGroup.Visible = true;
                Lnk_AddNewGroup.Visible = false;
                Img_AddUser.Visible = false;
                Btn_Add.Visible = true;
                Chk_AddMoreGroup.Visible = true;
                Btn_Update.Visible = false;
                Txt_GroupDescription.Text = "";
                Txt_GroupName.Text = "";

            }

        #endregion

        #region Methods
        
            private void LoadGroup()
            {
                DataSet Group_Ds = new DataSet();
                Group_Ds = MyGroup.GetAllGroup();
                if (Group_Ds != null && Group_Ds.Tables[0].Rows.Count > 0)
                {
                    Grd_Group.Columns[0].Visible = true;
                    Grd_Group.DataSource = Group_Ds;
                    Grd_Group.DataBind();
                    Grd_Group.Columns[0].Visible = false;
                }
                else
                {
                    Grd_Group.DataSource = null;
                    Grd_Group.DataBind();

                }
            }

            private void AddGroup(string _groupname,string description)
            {
                string sql = "";
                sql = "insert into tbl_gr_master(GroupName,Description) values('" + _groupname + "','"+description+"')";
                MyGroup.m_MysqlDb.ExecuteQuery(sql);
            }

        #endregion
    }
}
