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

    public partial class WebForm15 : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private Incident MyIncedent;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyIncedent = MyUser.GetIncedentObj();
            MyStudMang = MyUser.GetStudentObj();
            if ((MyIncedent == null) || (MyStudMang == null))
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(66))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //string _MenuStr;
                    //_MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    //this.SubStudentMenu.InnerHtml = _MenuStr;
                    Btn_Create.Visible = false;
                    Btn_CrAndApr.Visible = false;
                  //  LoadStudentTopData();
                    AddIncedentTypeToDrpList(0);
                   
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
            string sql = "select tblincedenthead.`Point`,tblincedenthead.NeedApproval from tblincedenthead where tblincedenthead.Id=" + Drp_Title.SelectedValue;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
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
            string sql = "select tblincedenthead.Title, tblincedenthead.Id from tblincedenthead where tblincedenthead.TypeId="+Drp_InceType.SelectedValue+" and tblincedenthead.`Mode`='Manual' and tblincedenthead.UserType='student' and tblincedenthead.IsActive=1 order by tblincedenthead.Id";
            MyReader = MyIncedent.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Create.Enabled = true;
                if (MyUser.HaveActionRignt(65))
                {
                Btn_CrAndApr.Enabled = true;
                }
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

        //private void LoadStudentTopData()
        //{

        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}
      
        private void CheckBtnVisibility()
        {
            if (!MyUser.HaveActionRignt(65))
            {
                Btn_Create.Visible=true;
                Btn_CrAndApr.Visible=false;
            }
            else
            {
                Btn_Create.Visible = true;
                Btn_CrAndApr.Visible=true;                
            }
        }

        private void AddIncedentTypeToDrpList(int _intex)
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
            Btn_Create.Enabled = false;
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);
            int _ClassId = MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId);
           
            if (Hdn_NeedApproval.Value == "YES")
            {
                MyIncedent.CreateIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), 0, MyUser.UserId, "student", int.Parse(Session["StudId"].ToString()), _Head, _Point, MyUser.CurrentBatchId,_ClassId);

                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create Incident", "Create an incident about student " + MyUser.m_DbLog.GetStudentName(int.Parse(Session["StudId"].ToString())) + " And submit for the approval", 1);
            }
            else
            {
                MyIncedent.CreateApprovedIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), MyUser.UserId, "student", int.Parse(Session["StudId"].ToString()), _Head, _Point, MyUser.CurrentBatchId,_ClassId);
             
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create&Approve Incident", "Create and approved an incident about student " + MyUser.m_DbLog.GetStudentName(int.Parse(Session["StudId"].ToString())) + "", 1);             
            }

            ClearAll();
            WC_MessageBox.ShowMssage("Incident is created ");    
            Btn_Create.Enabled = true;
        }

        private void ClearAll()
        {
            Txt_Dese.Text = "";
        }

        protected void Btn_CrAndApr_Click(object sender, EventArgs e)
        {
            int _Point = 0, _Head = int.Parse(Drp_Title.SelectedValue);
            int.TryParse(lbl_Points.Text.Trim(), out _Point);

            int _ClassId = MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId);

            MyIncedent.CreateApprovedIncedent(Drp_Title.SelectedItem.Text, Txt_Dese.Text.Trim(), Txt_Date.Text.Trim(), int.Parse(Drp_InceType.SelectedValue.ToString()), MyUser.UserId, "student", int.Parse(Session["StudId"].ToString()), _Head, _Point, MyUser.CurrentBatchId,_ClassId);
            ClearAll();
            WC_MessageBox.ShowMssage("Incident is Created and Approved ");
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Create&Approve Incident", "Create and approved an incident about student "+MyUser.m_DbLog.GetStudentName( int.Parse(Session["StudId"].ToString()))+"", 1);             
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
