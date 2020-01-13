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
    public partial class WebForm25 : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private Incident MyClassIncident=null ;
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
            if (Session["ClassId"] == null)
            {
                Response.Redirect("LoadClassDetails.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyClassIncident = MyUser.GetIncedentObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(69))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId,MyUser.SELECTEDMODE);
                    this.SubClassMenu.InnerHtml = _MenuStr;

                    AddIcidendTypetoDrpList(0);
                    Btn_CrAndApr.Visible = false;
                    CheckBtnVisibility();

                    LoadIncidentTitlesToDropDown();
                    LoadPointsForTitles();
                    Txt_Date.Text = MyUser.GerFormatedDatVal(DateTime.Now);    
                }
            }
        }

        private void LoadPointsForTitles()
        {
            int _Points = 0;
            string sql = "select tblincedenthead.`Point` from tblincedenthead where tblincedenthead.Id=" + Drp_Title.SelectedValue;
            MyReader = MyClassIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out  _Points);
                lbl_Points.Text = _Points.ToString();
                if (_Points > 0)
                {
                    Img_Up.Visible = true;
                    Img_Down.Visible = false;
                }
                else
                {
                    Img_Up.Visible = false;
                    Img_Down.Visible = true;
                }
            }
        }

        private void LoadIncidentTitlesToDropDown()
        {
            Drp_Title.Items.Clear();
            string sql = "select tblincedenthead.Title, tblincedenthead.Id from tblincedenthead where tblincedenthead.TypeId=" + Drp_InceType.SelectedValue + " and tblincedenthead.`Mode`='Manual' and tblincedenthead.UserType='student' and tblincedenthead.IsActive=1 order by tblincedenthead.Id";
            MyReader = MyClassIncident.m_MysqlDb.ExecuteQuery(sql);
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

        private void CheckBtnVisibility()
        {
            if (!MyClassIncident.HasApprovalRight(MyUser.UserId))
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
            {
                Drp_InceType.Items.Clear();
                string sql = "select tblincedenttype.Type,tblincedenttype.Id from tblincedenttype where  tblincedenttype.IncidentType='NORMAL' ";
                MyReader = MyClassIncident.m_MysqlDb.ExecuteQuery(sql);
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
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);
           
            string sql = "SELECT tblstudent.Id from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    MyClassIncident.CreateIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), 0, MyUser.UserId, "student", int.Parse(MyReader.GetValue(0).ToString()), _Head, _Point, MyUser.CurrentBatchId, int.Parse(Session["ClassId"].ToString()));
                }

                ClearAll();
                WC_MessageBox.ShowMssage("Incident is created ");
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Incedent", "One Incedent is reported for students of the class  " + MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString())) + " and is submit for approval.", 1);
            }
        }
       
        private void ClearAll()
        {
            Txt_Dese.Text = "";
        }

        protected void Btn_CrAndApr_Click(object sender, EventArgs e)
        {
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);

            string sql = "SELECT tblstudent.Id from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
            MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    MyClassIncident.CreateApprovedIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), MyUser.UserId, "student", int.Parse(MyReader.GetValue(0).ToString()), _Head, _Point, MyUser.CurrentBatchId, int.Parse(Session["ClassId"].ToString()));
                }

                ClearAll();
                WC_MessageBox.ShowMssage("Incident is created and Approved ");
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create & Approve Incedent", "One Incedent is reported for students of the class  " + MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString())) + " and is approved.", 1);
            }
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
