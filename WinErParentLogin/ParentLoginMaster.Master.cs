using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Text;
using System.Drawing;
namespace WinErParentLogin
{
    public partial class ParentLoginMaster : System.Web.UI.MasterPage
    {
        DataSet MyDataSet = null;
        private ParentInfoClass MyParentInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                FillStudentNameToDrpList();
                FillStudentDetails();
                FillMasterDetails();
            }
        }

        private void FillMasterDetails()
        {
            Lbl_LoginUser.Text = "Hi "+MyParentInfo.ParentName;
          
            SchoolDetails.InnerHtml = GetSchoolInfo(MyParentInfo.SCHOOLNAME);
            
        }

        public string GetSchoolInfo(string _schoolName)
        {
            StringBuilder School = new StringBuilder("");
            //System.Configuration.AppSettingsReader MyAppReader = new System.Configuration.AppSettingsReader();
            School.Append("<table><tr><td ><img height=\"30\" width=\"30\" alt=\"\" src=\"" + MyParentInfo.SCHOOLLOGO + "\" /> </td><td>" + _schoolName + "</td> </tr></table>");
            //MyAppReader = null;
            return School.ToString();
        }

        private void FillStudentDetails()
        {
            string HasVirtualFolder = System.Configuration.ConfigurationSettings.AppSettings["HasVirtualFolder"];
            if (HasVirtualFolder == "1")
            {
            
                Image_StudentIMG.ImageUrl = MyParentInfo.StudentImage;
                TRImgArea.Visible = true;
            }
            else
            {
                TRImgArea.Visible = false;
            }
            Lbl_Class.Text = MyParentInfo.CLASSNAME;
            Lbl_RollNo.Text = MyParentInfo.RollNO;
            Lbl_Age.Text = MyParentInfo.AGE.ToString();
            Lbl_AdmissionNo.Text = MyParentInfo.ADMISSIONNO;
            Lbl_TotalPoints.Text = MyParentInfo.POINT.ToString();
            Lbl_TotalRating.Text = MyParentInfo.RATING.ToString();
            if (MyParentInfo.POINT < 0)
            {
                Lbl_TotalPoints.ForeColor = Color.Red;
                Img_TotalPoints.ImageUrl = "Pics/Points red.png";
            }
            else
            {
                Lbl_TotalPoints.ForeColor = Color.FromName("#ddb104");
                Img_TotalPoints.ImageUrl = "Pics/Points.png";
            }
            if (MyParentInfo.RATING < 0)
            {
                Lbl_TotalRating.ForeColor = Color.Red;
                Img_TotalRating.ImageUrl = "Pics/Rating red.png";
                
            }
            else
            {
                Lbl_TotalRating.ForeColor = Color.FromName("#ddb104");
                Img_TotalRating.ImageUrl = "Pics/Rating.png";
            }
        }

        private void FillStudentNameToDrpList()
        {
            Drp_StudentName.Items.Clear();
            ListItem li;
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            MyDataSet = MyParent.GetMyStudent(MyParentInfo.ParentId);
            _mysqlObj.CloseConnection();
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Students in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(Dr_Students[0].ToString(), Dr_Students[1].ToString());
                    Drp_StudentName.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No student found", "-1");
                Drp_StudentName.Items.Add(li);
            }
            Drp_StudentName.SelectedValue = MyParentInfo.StudentId.ToString();
            _mysqlObj = null;
            MyParent = null;
            
        }

        protected void Drp_StudentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillStudentDetails();
            MyParentInfo.ChangeStudent(Drp_StudentName.SelectedItem.Text, int.Parse(Drp_StudentName.SelectedValue));
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            ParentLogin MyParent = new ParentLogin(_mysqlObj, MyParentInfo.SchoolObject);
            Incident MyIncident = new Incident(_mysqlObj);
            string _ClassName, _RollN0, _AdmissionN0, _studentImg,ClassId;
            int _Age, _Point, _rating;
            MyParent.GetStudentDetails(MyParentInfo.StudentId, out _ClassName, out _RollN0, out _Age, out _AdmissionN0, out _studentImg, out ClassId);
            //MyIncident.Get
            MyIncident.GetPointRating(MyParentInfo.StudentId, MyParentInfo.CurrentBatchId, out _Point, out _rating);
            MyParentInfo.SetStudentDetails(_ClassName, _RollN0, _Age, _AdmissionN0, _Point, _rating, _studentImg, ClassId);
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParent = null;
            MyIncident = null;
            Session["MyParentObj"] = MyParentInfo;
            Response.Redirect(GetCurrentPageName());

        }
        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

 

        protected void ImgBtn_Message_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MessagePage.aspx");
        }
    }
}
