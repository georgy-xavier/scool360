using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class WebForm24 : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private Incident MyIncedent;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;

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
            if (Session["StaffId"] == null)
            {
                Response.Redirect("ViewStaffs.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            MyIncedent = MyUser.GetIncedentObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(68))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;

                    AddIcidendTypetoDrpList(0);
                    Btn_CrAndApr.Visible = false;

                    CheckBtnVisibility();
                    LoaduserTopData();

                    LoadIncidentTitlesToDropDown();
                    LoadPointsForTitles();
                    Txt_Date.Text = MyUser.GerFormatedDatVal(DateTime.Now);                    
                }
            }
        }

        private void LoadPointsForTitles()
        {
            int _Points = 0;
            string sql = "select tblincedenthead.`Point`,tblincedenthead.NeedApproval from tblincedenthead where tblincedenthead.Id=" + Drp_Title.SelectedValue;
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Hdn_NeedApproval.Value = MyReader.GetValue(1).ToString();
                int.TryParse(MyReader.GetValue(0).ToString(), out  _Points);
                lbl_Points.Text = _Points.ToString();
                if (_Points >= 0)
                {
                    Img_Up.Visible = true;
                    Img_Down.Visible = false;
                }
                else
                {
                    Img_Up.Visible = false;
                    Img_Down.Visible = true;
                }
                if (Hdn_NeedApproval.Value == "YES")
                {
                    if (MyUser.HaveActionRignt(65))
                    {
                        Btn_CrAndApr.Visible = true;
                    }
                }
                else
                {
                    Btn_CrAndApr.Visible = false;
                }
            }
        }

        private void LoadIncidentTitlesToDropDown()
        {
            Drp_Title.Items.Clear();
            string sql = "select tblincedenthead.Title, tblincedenthead.Id from tblincedenthead where tblincedenthead.TypeId=" + Drp_InceType.SelectedValue + " and tblincedenthead.`Mode`='Manual' and tblincedenthead.UserType='Staff' and tblincedenthead.IsActive=1 order by tblincedenthead.Id";
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Create.Enabled = true;
                Btn_CrAndApr.Enabled = true;

                Img_Down.Visible = true;
                Img_Up.Visible = true;
                lbl_Points.Visible = true;
                lbl_PointText.Visible = true;

                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Title.Items.Add(li);
                }
            }
            else
            {
                Drp_Title.Items.Add(new ListItem("No Title found", "-1"));
                Btn_Create.Enabled = false;
                Btn_CrAndApr.Enabled = false;

                Img_Down.Visible = false;
                Img_Up.Visible = false;
                lbl_Points.Visible = false;
                lbl_PointText.Visible = false;
            }
        }

        private void LoaduserTopData()
        {
            //  string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), MyUser.GetImageUrl("StaffImage", int.Parse(Session["StaffId"].ToString())));
           string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
     
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }

        private void CheckBtnVisibility()
        {
            if (!MyIncedent.HasApprovalRight(MyUser.UserId))
            {
                Btn_Create.Visible = true;
                Btn_CrAndApr.Visible = false;
            }
            else
            {
                Btn_Create.Visible = true;
                Btn_CrAndApr.Visible = true;
            }
        }

        private void AddIcidendTypetoDrpList(int _intex)
        {
            Drp_InceType.Items.Clear();
            string sql = "select tblincedenttype.Type,tblincedenttype.Id from tblincedenttype  where  tblincedenttype.IncidentType='NORMAL' ";
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_InceType.Items.Add(li);
                }
                Drp_InceType.SelectedIndex = _intex;
            }
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);

            if (Hdn_NeedApproval.Value == "YES")
            {
                MyIncedent.CreateIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), 0, MyUser.UserId, "Staff", int.Parse(Session["StaffId"].ToString()), _Head, _Point, MyUser.CurrentBatchId, 0);
            }
            else
            {
                MyIncedent.CreateApprovedIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), MyUser.UserId, "Staff", int.Parse(Session["StaffId"].ToString()), _Head, _Point, MyUser.CurrentBatchId,0);
            }
            ClearAll();
            WC_MessageBox.ShowMssage("Incident is created ");
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Incident", "One Incident is created about staff ", 1);
        }

        protected void Btn_CrAndApr_Click(object sender, EventArgs e)
        {
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);

            MyIncedent.CreateApprovedIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), MyUser.UserId, "Staff", int.Parse(Session["StaffId"].ToString()), _Head, _Point, MyUser.CurrentBatchId,0);

            ClearAll();
            WC_MessageBox.ShowMssage("Incident is created and approved");
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Incident", "One Incident is created and aproved about staff ", 1);
        }

        private void ClearAll()
        {
            Txt_Dese.Text = "";
        }

        protected void Drp_InceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            LoadIncidentTitlesToDropDown();
            LoadPointsForTitles();
        }

        protected void Drp_Title_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            LoadPointsForTitles();
        }
    }
}
