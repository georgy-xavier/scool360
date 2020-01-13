using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
using WinEr;
namespace WinEr
{
    public partial class CreateCertificates : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private int studId = 0;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            if (Session["StudType"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            studId = int.Parse(Session["StudId"].ToString());
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(830))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {

                    string _MenuStr;
                    _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    this.SubStudentMenu.InnerHtml = _MenuStr;
                    LoadExamName();
                    Pnl_Certificate.Visible = false;
                    EntryPart.Visible = false;
                    LoadStudentTopData();

                }
            }
        }
        private void LoadStudentTopData()
        {

          //  string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), MyUser.GetImageUrl("StudentImage", int.Parse(Session["StudId"].ToString())), int.Parse(Session["StudType"].ToString()));
            string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));

            this.StudentTopStrip.InnerHtml = _Studstrip;
        }


        private void LoadExamName()
        {
            Drp_Type.Items.Clear();
            string sql = "select tblcertificate_config.Id, tblcertificate_config.CertificateName from tblcertificate_config where tblcertificate_config.CertificateStatus=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            ListItem l1 = new ListItem("Select Type", "-1");
            Drp_Type.Items.Add(l1);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Type.Items.Add(li);
                }
                Drp_Type.SelectedValue = "-1";
            }
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            Txt_SchoolCode.Text = "";
            Txt_Estd.Text = "";
            Txt_Batch.Text = "";
            Txt_role.Text = "";
            Txt_Num.Text = "";

            Lbl_Err.Text = "";
            Btn_Print.Enabled = false;
            if (Drp_Type.SelectedValue != "-1")
            {
                EntryPart.Visible = true;
                Btn_Print.Enabled = true;
                Pnl_Certificate.Visible = true;
                string CertificateName = "", ExamName = "", BoardofExam = "", RollNum1 = "", RollNum2 = "", StudentBehaviourSentences = "", Footer1 = "", Footer2 = "", Footer3 = "";
                int CertificateType = -1, IsRollNumberNeed = -1, Need_Address = -1, Need_Estd = -1, Need_SchoolCode = -1;

                //School Details
                string SchoolName = MyUser.SchoolName;
                string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails where tblschooldetails.SchoolName='" + SchoolName + "'";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    ImgBtn.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
                    Lbl_CollegeName.Text = MyReader.GetValue(0).ToString();
                    Lbl_ClgAdd.Text = MyReader.GetValue(1).ToString();
                }

                Txt_Date.Text =General.GerFormatedDatVal(DateTime.Now.Date);
                //Student Details
                string Sql_StudDetails = "Select StudentName,GardianName,AdmitionNo,Date_Format( DOB,'%d/%m/%Y')as DOB,Address, tblstandard.Name from tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id inner join tblstandard on tblstandard.Id=tblview_studentclassmap.Standard  where tblview_student.Id=" + studId;
                OdbcDataReader StudReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql_StudDetails);

                if (StudReader.HasRows)
                { 
                    Lbl_AdmissionNum.Text = StudReader.GetValue(2).ToString();
                   
                }

                //CertificateDetails
                string Sql_Config = "select Id,CertificateName,CertificateType,ExamName,BoardofExam,IsRollNumberNeed,RollNum1,RollNum2,StudentBehaviourSentences,FooterLeft,FooterCenter,FooterRight,Need_Address,Need_Estd,Need_SchoolCode from tblcertificate_config where tblcertificate_config.Id=" + int.Parse(Drp_Type.SelectedValue.ToString());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql_Config);
                StringBuilder Certificates = new StringBuilder();
                if (MyReader.HasRows)
                { 
                   
                    CertificateName = MyReader.GetValue(1).ToString();
                    int.TryParse(MyReader.GetValue(2).ToString(), out  CertificateType);
                    ExamName = MyReader.GetValue(3).ToString();
                    BoardofExam = MyReader.GetValue(4).ToString();
                    int.TryParse(MyReader.GetValue(5).ToString(), out IsRollNumberNeed);
                    RollNum1 = MyReader.GetValue(6).ToString();
                    RollNum2 = MyReader.GetValue(7).ToString();
                    StudentBehaviourSentences = MyReader.GetValue(8).ToString();
                    Footer1 = MyReader.GetValue(9).ToString();
                    Footer2 = MyReader.GetValue(10).ToString();
                    Footer3 = MyReader.GetValue(11).ToString();
                    int.TryParse(MyReader.GetValue(12).ToString(), out Need_Address);
                    int.TryParse(MyReader.GetValue(13).ToString(), out Need_Estd);
                    int.TryParse(MyReader.GetValue(14).ToString(), out Need_SchoolCode);
                    certificateName.InnerText = CertificateName.ToUpper();
                    
                    if (CertificateType == 1)
                    {

                        Certificates.Append("Certified that  Sri/Miss. ");

                        Certificates.Append(" " + StudReader.GetValue(0).ToString() + " ");

                        Certificates.Append("Son/daughter of  Mr./Mrs/Late. ");

                        Certificates.Append(" " + StudReader.GetValue(1).ToString() + " ");
                       
                        if (Need_Address == 1)
                        {
                            Certificates.Append(" a resident of   " + StudReader.GetValue(4).ToString() + " ");
                        }
                        Certificates.Append(" <Span style=\"color:red\">' Entry 4 ' </span>  the " + ExamName + " , ");

                        Certificates.Append(" <Span style=\"color:red\">' Entry 5 ' </span>  of the " + BoardofExam + " ");
                        Certificates.Append(" as a regular/Private candidate from " + MyUser.SchoolName + "  and was placed in the ");

                        Certificates.Append("<Span style=\"color:red\">' Entry 8 ' </span> division. ");

                        Txt_Division.Text = StudReader.GetValue(5).ToString();
                        ImgE8.Visible = false;

                        Lbl_E4.Enabled = true;
                        Drp_Passed.Enabled = true;
                        ImgE4.Visible = false;
                        Lbl_E5.Enabled = true;
                        Txt_Batch.Enabled = true;
                        ImgE5.Visible = false;
                        Txt_Batch.Text = MyUser.CurrentBatchName;
                     
                        if (IsRollNumberNeed == 1)
                        {
                            string Des = " His/Her ";
                            string _Num1 = "";
                            string Num2 = "";
                            int temp = 0;
                            if (RollNum1 != "")
                            {
                                Des = Des + RollNum1;
                                temp = 1;

                                Txt_role.Enabled = true;
                                Lbl_E6.Enabled = true;
                                ImgE6.Visible = false;
                                _Num1 = "<Span style=\"color:red\">' Entry 6 ' </span> ";

                            }
                            else
                            {
                                Txt_role.Enabled = false;
                                ImgE6.Visible = true;
                                Lbl_E6.Enabled = false;
                            }

                            if (RollNum2 != "")
                            {
                                if (temp == 1)
                                    Des = Des + " and  ";
                                Des = Des + RollNum2;

                                Num2 = "<Span style=\"color:red\">' Entry 7 ' </span> ";
                                Txt_Num.Enabled = true;
                                Lbl_E7.Enabled = true;
                                ImgE7.Visible = false;
                            }
                            else
                            {
                                temp = 0;
                                Txt_Num.Enabled = false;
                                Lbl_E7.Enabled = false;
                                ImgE7.Visible = true;
                            }

                            
                            if (temp == 1)
                            {
                                Des = Des + "  respectively are ";

                            }
                            else
                            {
                                Des = Des + " is ";

                            }
                            if (_Num1 != "")
                            {
                                Des = Des + _Num1;
                                if (temp == 1)
                                    Des = Des + " and ";
                            }
                            if (Num2 != "")
                                Des = Des + Num2;
                            Certificates.Append(Des);

                        }
                        else
                        {

                            Txt_role.Enabled = false;
                            Txt_Num.Enabled = false;
                            ImgE7.Visible = true;
                            ImgE6.Visible = true;
                        }
                        

                        if (Need_Estd == 1)
                        {
                            lbl_Estd.Visible = true;
                            Txt_Estd.Text = "";
                            Txt_Estd.Enabled = true;
                            ImgE2.Visible = false;
                            Lbl_E2.Enabled = true;
                            Lbl_Entry2.Visible = true;
                        }
                        else
                        {
                            lbl_Estd.Visible = false;
                            Txt_Estd.Text = "";
                            Txt_Estd.Enabled = false;
                            Lbl_E2.Enabled = false;
                            ImgE2.Visible = true;
                            Lbl_Entry2.Visible = false;
                        }
                        if (Need_SchoolCode == 1)
                        {
                            Lbl_SchoolCode.Visible = true;
                            Txt_SchoolCode.Text = "";
                            Txt_SchoolCode.Enabled = true;
                            ImgE1.Visible = false;
                            Lbl_E1.Enabled = true;
                            lbl_Entry1.Visible = true;
                        }
                        else
                        {
                            Lbl_SchoolCode.Visible = false;
                            Txt_SchoolCode.Text = "";
                            Txt_SchoolCode.Enabled = false;
                            ImgE1.Visible = true;
                            Lbl_E1.Enabled = false;
                            lbl_Entry1.Visible = false;
                        }
                        Certificates.Append("<br/>His/her date of birth according to the record provided by his/her parents is ");
                        Certificates.Append(" " + StudReader.GetValue(3).ToString() + " .");

                        Txt_Behave.Text = StudentBehaviourSentences;
                        Txt_Behave.Visible = true; 


                    }
                    else if (CertificateType == 2)
                    {
                      
                        Lbl_E4.Enabled = false;
                        Drp_Passed.Enabled = false;
                        ImgE4.Visible = true;
                        Lbl_E5.Enabled = false;
                        Txt_Batch.Enabled = false;
                        ImgE5.Visible = true;
                        Txt_Behave.Text = "";

                        Txt_Behave.Visible = false;
                        Certificates.Append("Certified that  Sri/Miss. ");

                        Certificates.Append(" " + StudReader.GetValue(0).ToString() + " ");

                        Certificates.Append("Son/daughter of  Mr./Mrs/Late. ");

                        Certificates.Append(" " + StudReader.GetValue(1).ToString() + " ");

                        if (Need_Address == 1)
                        {
                            Certificates.Append(" a resident of   " + StudReader.GetValue(4).ToString() + " ");
                        }
                        Certificates.Append("is currently studying in this school  in class");
                        Certificates.Append(" <Span style=\"color:red\">' Entry 6 ' </span> .");

                        Txt_Division.Text = StudReader.GetValue(5).ToString();

                        Certificates.Append("<br/> His/her date of birth according to the record provided by his/her parents is ");
                        Certificates.Append(" " + StudReader.GetValue(3).ToString() + " .");

                        if (IsRollNumberNeed == 1)
                        {
                            string Des = " His/Her ";
                            string _Num1 = "";
                            string Num2 = "";
                            int temp = 0;
                            if (RollNum1 != "")
                            {
                                Des = Des + RollNum1;
                                temp = 1;

                                Txt_role.Enabled = true;
                                Lbl_E6.Enabled = true;
                                ImgE6.Visible = false;
                                _Num1 = "<Span style=\"color:red\">' Entry 6 ' </span> ";

                            }
                            else
                            {
                                Txt_role.Enabled = false;
                                Lbl_E6.Enabled = false;
                                ImgE6.Visible = true;
                            }

                            if (RollNum2 != "")
                            {
                                if (temp == 1)
                                    Des = Des + " and  ";
                                Des = Des + RollNum2;

                                Num2 = "<Span style=\"color:red\">' Entry 7 ' </span> ";
                                Txt_Num.Enabled = true;

                                Lbl_E7.Enabled = true;
                                ImgE7.Visible = false;
                            }
                            else
                            {
                                temp = 0;
                                Txt_Num.Enabled = false;
                                Lbl_E7.Enabled = false;
                                ImgE7.Visible = true;
                            }


                            if (temp == 1)
                            {
                                Des = Des + "  respectively are ";

                            }
                            else
                            {
                                Des = Des + " is ";

                            }
                            if (_Num1 != "")
                            {
                                Des = Des + _Num1;
                                if (temp == 1)
                                    Des = Des + " and ";
                            }
                            if (Num2 != "")
                                Des = Des + Num2;
                            Des = Des + " . "; 
                            Certificates.Append(Des);

                        }
                        else
                        {

                            Txt_role.Enabled = false;
                            Txt_Num.Enabled = false;
                            ImgE6.Visible = true;
                            ImgE7.Visible = true;
                        }
                        if (Need_Estd == 1)
                        {
                            lbl_Estd.Visible  = true;
                            Txt_Estd.Text = "";
                            Txt_Estd.Enabled = true;
                            ImgE2.Visible = false;
                            Lbl_E2.Enabled = true;
                            Lbl_Entry2.Visible = true;
                        }
                        else
                        {
                            lbl_Estd.Visible = false;
                            Txt_Estd.Text = "";
                            Txt_Estd.Enabled = false;
                            ImgE2.Visible =true;
                            Lbl_E2.Enabled = false;
                            Lbl_Entry2.Visible = false;
                        }
                        if (Need_SchoolCode == 1)
                        {
                            Lbl_SchoolCode.Visible = true;
                            Txt_SchoolCode.Text = "";
                            Txt_SchoolCode.Enabled = true;
                            ImgE1.Visible = false;
                            Lbl_E1.Enabled = true;
                            lbl_Entry1.Visible = true;
                        }
                        else
                        {
                            Lbl_SchoolCode.Visible = false;
                            Txt_SchoolCode.Text = "";
                            Txt_SchoolCode.Enabled = false;
                            ImgE1.Visible = true;
                            Lbl_E1.Enabled = false;
                            lbl_Entry1.Visible = false;
                        }
                    }

                    if (Footer1 != "")
                    {
                        Lbl_Footer1.Visible = true;
                       
                        Lbl_Footer1.Text = Footer1;
                    }
                    else
                    {
                        Lbl_Footer1.Visible = false;
                        
                        Lbl_Footer1.Text = "";
                    }
                    if (Footer2 != "")
                    {
                        Lbl_Footer2.Visible = true;
                        
                        Lbl_Footer2.Text = Footer2;
                    }
                    else
                    {
                        Lbl_Footer2.Visible = false;
                        
                        Lbl_Footer2.Text = "";
                    }
                    if (Footer3 != "")
                    {
                        Lbl_Footer3.Visible = true;
                        
                        Lbl_Footer3.Text = Footer3;
                    }
                    else
                    {
                        Lbl_Footer3.Visible = false;
                        
                        Lbl_Footer3.Text = "";
                    }
                   
                }
                Description.InnerHtml = Certificates.ToString();
            }
            else
            {
                Pnl_Certificate.Visible = false;
                Lbl_Err.Text = " Select certificate type";
                EntryPart.Visible = false;
            }
        }

        protected void Btn_Print_Click(object sender, EventArgs e)
        {
            if (ValidDatas())
            {

                string CertificateName = "", ExamName = "", BoardofExam = "", RollNum1 = "", RollNum2 = "", StudentBehaviourSentences = "", Footer1 = "", Footer2 = "", Footer3 = "", ImgUrl = "", EstdDate = "", SchoolCode = "";

                int IsRollNumberNeed = -1, Need_Address = -1, Need_Estd = -1, Need_SchoolCode = -1;

                string Sql_Config = "select Id,CertificateName,CertificateType,ExamName,BoardofExam,IsRollNumberNeed,RollNum1,RollNum2,StudentBehaviourSentences,FooterLeft,FooterCenter,FooterRight,Need_Address,Need_Estd,Need_SchoolCode from tblcertificate_config where tblcertificate_config.Id=" + int.Parse(Drp_Type.SelectedValue.ToString());
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql_Config);
                if (MyReader.HasRows)
                {
                    string CertificateType = MyReader.GetValue(2).ToString();
                    ExamName = MyReader.GetValue(3).ToString();
                    BoardofExam = MyReader.GetValue(4).ToString();
                    int.TryParse(MyReader.GetValue(5).ToString(), out IsRollNumberNeed);
                    RollNum1 = MyReader.GetValue(6).ToString();
                    RollNum2 = MyReader.GetValue(7).ToString();

                    Footer1 = MyReader.GetValue(9).ToString();
                    Footer2 = MyReader.GetValue(10).ToString();
                    Footer3 = MyReader.GetValue(11).ToString();
                    int.TryParse(MyReader.GetValue(12).ToString(), out Need_Address);
                    int.TryParse(MyReader.GetValue(13).ToString(), out Need_Estd);
                    int.TryParse(MyReader.GetValue(14).ToString(), out Need_SchoolCode);
                    StudentBehaviourSentences = "";
                    if (Need_Estd == 1)
                    {
                        EstdDate = Txt_Estd.Text;
                    }

                    if (Need_SchoolCode == 1)
                    {
                        SchoolCode = Txt_SchoolCode.Text;
                    }



                    StringBuilder SB = new StringBuilder();

                    string Sql_StudDetails = "Select StudentName,GardianName,AdmitionNo,Date_Format( DOB,'%d/%m/%Y')as DOB,Address, tblstandard.Name from tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id inner join tblstandard on tblstandard.Id=tblview_studentclassmap.Standard  where tblview_student.Id=" + studId;
                    OdbcDataReader StudReaderNew = MyStudMang.m_MysqlDb.ExecuteQuery(Sql_StudDetails);

                    if (StudReaderNew.HasRows)
                    {

                        if (CertificateType == "1")
                        {

                            SB.Append("Certified that  Sri/Miss. ");

                            SB.Append(" " + StudReaderNew.GetValue(0).ToString() + " ");

                            SB.Append("Son/Daughter of  Mr./Mrs/Late. ");

                            SB.Append(" " + StudReaderNew.GetValue(1).ToString() + " ");
                            if (Need_Address == 1)
                            {
                                SB.Append(" a resident of   " + StudReaderNew.GetValue(4).ToString() + " ");
                            }
                            SB.Append(" " + Drp_Passed.SelectedItem + "  the " + ExamName + " , ");

                            SB.Append(" " + Txt_Batch.Text + " of the " + BoardofExam + " ");
                            SB.Append(" as a regular / private candidate from " + MyUser.SchoolName + "  and was placed in the ");

                            SB.Append(" " + Txt_Division.Text + " division. ");


                           // StudReaderNew.GetValue(5).ToString()

                            if (IsRollNumberNeed == 1)
                            {
                                string Des = " His/Her ";
                                string _Num1 = "";
                                string Num2 = "";
                                int temp = 0;
                                if (RollNum1 != "")
                                {
                                    Des = Des + RollNum1;
                                    temp = 1;


                                    _Num1 = Txt_role.Text;

                                }

                                if (RollNum2 != "")
                                {
                                    if (temp == 1)
                                        Des = Des + " and  ";
                                    Des = Des + RollNum2;

                                    Num2 = Txt_Num.Text;
                                }
                                else
                                {
                                    temp = 0;
                                }
                                if (temp == 1)
                                {
                                    Des = Des + "  respectively are ";

                                }
                                else
                                {
                                    Des = Des + " is ";

                                }
                                if (_Num1 != "")
                                {
                                    Des = Des + _Num1;
                                    if (temp == 1)
                                        Des = Des + " and ";
                                }
                                if (Num2 != "")
                                    Des = Des + Num2;
                                SB.Append(Des);
                                
                            }
                            StudentBehaviourSentences = Txt_Behave.Text;
                            SB.Append(" His/her date of birth according to the record provided by his/her parents is ");
                            SB.Append(" " + StudReaderNew.GetValue(3).ToString() + ". ");


                        }
                        else if (CertificateType == "2")
                        {
                            Txt_Behave.Text = "";
                            SB.Append("  Certified that  Sri/Miss. ");

                            SB.Append(" " + StudReaderNew.GetValue(0).ToString() + " ");

                            SB.Append("Son/Daughter of  Mr./Mrs/Late. ");

                            SB.Append(" " + StudReaderNew.GetValue(1).ToString() + " ");
                            if (Need_Address == 1)
                            {
                                SB.Append(" a resident of " + StudReaderNew.GetValue(4).ToString() + " ");
                            }
                            SB.Append("is currently studying in this school  in class");
                            SB.Append(" " + Txt_Division.Text + ".");

                            //StudReaderNew.GetValue(5).ToString()

                            SB.Append(" His/her date of birth according to the record provided by his/her parents is ");
                            SB.Append(" " + StudReaderNew.GetValue(3).ToString() + ". ");
                        }
                    }

                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"PrintCertificate.aspx?C_Type=" + Drp_Type.SelectedValue + "&C_Name=" + Drp_Type.SelectedItem + "&C_Content=" + SB.ToString() + "&Behaviour=" + StudentBehaviourSentences + "&IsNeed_Estd=" + Need_Estd + "&ESTD=" + EstdDate + "&ISNeed_SchoolCode=" + Need_SchoolCode + "&SchoolCode=" + SchoolCode + "&Footer1=" + Footer1 + "&Footer2=" + Footer2 + "&Footer3=" + Footer3 + "&C_Date=" + Txt_Date.Text + "\");", true);
                }
            }
        }

        private bool ValidDatas()
        {
          if(Txt_SchoolCode.Enabled==true && Txt_SchoolCode.Text=="")
          {
              Lbl_Err.Text="Enter School Code";
              return false;
          }
            if(Txt_Estd.Enabled==true&& Txt_Estd.Text=="")
            {
                 Lbl_Err.Text="Enter School Estd. Year";
              return false;
            }
            if(Txt_Date.Enabled==true&& Txt_Date.Text=="" )
            {
                 Lbl_Err.Text="Enter Date";
                return false;
            }
            if(Txt_Batch.Enabled==true&& Txt_Batch.Text=="")
            {
                 Lbl_Err.Text="Enter Batch";
                return false;
            }
            if(Txt_role.Enabled==true&& Txt_role.Text=="")
            {
                 Lbl_Err.Text="Enter Roll";
                return false;
            }
             if(Txt_Num.Enabled==true&& Txt_Num.Text=="")
            {
                 Lbl_Err.Text="Enter Number";
                return false;
            }
             return true;
                 
        }
    }
}
