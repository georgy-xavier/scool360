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
//using System.Data;
using System.Text;
using WinBase;
public partial class StaffDetails : System.Web.UI.Page
{
    private StaffManager MyStaffMang;
    private KnowinUser MyUser;
    private Incident Myincident;
    private OdbcDataReader MyReader = null;
    //private DataSet MydataSet;
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
        Myincident = MyUser.GetIncedentObj();
        if (MyStaffMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else
        {


            if (!IsPostBack)
            {
               
                string _SubMenuStr;
                _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                this.SubStaffMenu.InnerHtml = _SubMenuStr;
                AddStaffDetails();
                AddStaffImage();
                CheckViewIncidentRight(); 
                LoadSubjectsHandle();
                LoaduserTopData();
                if (MyUser.HaveModule(29))
                {
                    PnlPayrollYesNo.Visible = true;
                  
                    LoadPayrollActive();
                }
                else
                {
                    PnlPayrollYesNo.Visible = false;
                }
               
                //some initlization

            }
        }
    }
    private void LoaduserTopData()
    {

        string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
        this.StudentTopStrip.InnerHtml = _Studstrip;
    }

    protected void Page_init(object sender, EventArgs e)
    {
       
    }
    
    private void CheckViewIncidentRight()
    {
        if (MyUser.HaveActionRignt(71))
        {
            this.StaffIncidents.InnerHtml = Myincident.LoadIncidenceData(int.Parse(Session["StaffId"].ToString()), "Staff",MyUser.CurrentBatchId);
        }
        else
        {
            //this.TopTab.InnerHtml = "No incidents to view";
            this.TabPanel2.Visible = false;
            this.Tabs.ActiveTabIndex = 0;
        }
    }
 
    private void LoadSubjectsHandle()
    {
        StringBuilder Subject = new StringBuilder();
        string sql = "SELECT Id,subject_name FROM tblsubjects";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                if (MyStaffMang.IsStaffSubject(int.Parse(Session["StaffId"].ToString()), int.Parse(MyReader.GetValue(0).ToString())))
                {

                    Subject.Append("<b>"+MyReader.GetValue(1).ToString()+"</b><br/>");
                }
            }
        }
        Div_Subject.InnerHtml = Subject.ToString();
    }


    private void LoadPayrollActive()
    {
        string payroll="No";
        int status;
        if (MyUser.HaveModule(29))
        {
            string sql = "SELECT Id,`status` FROM tblpay_employee where StaffId = " + Session["StaffId"].ToString() + "";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                status =int.Parse(MyReader.GetValue(1).ToString());
                if (status == 1)
                {
                    payroll = "Yes";
                }
            }
        }
        lbl_payroll.Text = payroll;
    }
    private void AddStaffDetails()
    {
        DateTime Dob;
        DateTime JoiningDate;
        //int RoleId=-1;
       // String Sql = "SELECT tu.SurName,tr.RoleName,ts.Experience,ts.Designation,ts.JoiningDate,ts.Address,ts.Sex,ts.Dob,ts.ExpDescription,ts.PhoneNumber,tu.EmailId,tu.UserName,ts.EduQualifications FROM tbluser tu INNER JOIN tblstaffdetails ts INNER JOIN tblrole tr ON ts.UserId=tu.Id AND tr.Id=tu.Id WHERE tu.Id="+int.Parse( Session["StaffId"].ToString());
        string Sql = "SELECT UserName,SurName,EmailId  FROM tbluser WHERE Id=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            Lbl_StaffId.Text = MyReader.GetValue(0).ToString();
            Hdn_Name.Value = MyReader.GetValue(1).ToString();
            Lbl_Email.Text = MyReader.GetValue(2).ToString();
        }
        Sql = "SELECT tblrole.RoleName FROM tblrole INNER JOIN tbluser ON tblrole.Id =tbluser.RoleId WHERE tbluser.Id=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
           Hdn_Role.Value = MyReader.GetValue(0).ToString(); 
        }
        Sql = "SELECT Experience,Designation,JoiningDate,Address,Sex,Dob,PhoneNumber,EduQualifications,AadharNo,PanNo FROM tblstaffdetails WHERE UserId=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Hdn_Experience.Value = MyReader.GetValue(0).ToString();
            Hdn_Designation.Value = MyReader.GetValue(1).ToString();
            JoiningDate = DateTime.Parse(MyReader.GetValue(2).ToString());
            //JoiningDate = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());
            Lbl_Doj.Text = MyUser.GerFormatedDatVal(JoiningDate.Date);
            Div_Address.InnerHtml = MyReader.GetValue(3).ToString();
            Lbl_Gender.Text = MyReader.GetValue(4).ToString();
            Dob = DateTime.Parse(MyReader.GetValue(5).ToString());
            //Dob = MyUser.GetDareFromText(MyReader.GetValue(5).ToString());
            lbl_Age.Text = MyStaffMang.GetAge(Dob, DateTime.Now).ToString();
            Lbl_Dob.Text = MyUser.GerFormatedDatVal(Dob);

            Lbl_PhNo.Text = MyReader.GetValue(6).ToString();
            Lbl_EduQuali.Text = MyReader.GetValue(7).ToString();
            Lbl_Aadhar.Text = MyReader.GetValue(8).ToString();
            Lbl_PAN.Text = MyReader.GetValue(9).ToString();

        }
        int i = 0;
        string _Group = "";
        Sql = "select tblgroup.GroupName from tblgroup inner join tblgroupusermap on tblgroup.Id= tblgroupusermap.GroupId where tblgroupusermap.UserId=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                i++;
                if (i > 1)
                {
                    _Group = _Group + " / ";
                }
                _Group = _Group + MyReader.GetValue(0).ToString();                
            }
            lbl_GroupName.Text = _Group;
        }
    }

    private void AddStaffImage()
    {

        //Img_staff.ImageUrl = MyUser.GetImageUrl("StaffImage", int.Parse(Session["StaffId"].ToString()));
    }

    protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
    {
        string FileName = "StaffDetails";
        string _TableString = GetStaffDetailsForPrintOut();
      
        if (!WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName,_TableString,"StaffDetails"))
        {
            WC_MessageBox.ShowMssage("This function need Ms office");
        }
        
    }

    private string GetStaffDetailsForPrintOut()
    {
        StringBuilder CTR = new StringBuilder();

        CTR.Append("<table id=\"MyReport\" runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Name</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Hdn_Name.Value + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Sex</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_Gender.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">DOB</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_Dob.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Age</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + lbl_Age.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Date of Joining</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_Doj.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Address</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Div_Address.InnerHtml.ToString() + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Phone Number</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_PhNo.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Email</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_Email.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Group Name</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + lbl_GroupName.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Educational Qualification</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_EduQuali.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Aadhar Number</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_Aadhar.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">PAN Number</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_PAN.Text + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Subjects Handle</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Div_Subject.InnerHtml.ToString() + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Experience</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Hdn_Experience.Value + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Designation</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Hdn_Designation.Value + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");

        CTR.Append("<tr>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Role</td>");
        CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Hdn_Role.Value + "");
        CTR.Append("</td>");
        CTR.Append("</tr>");        

        CTR.Append("</table>");
        return CTR.ToString();
    }
   
}
