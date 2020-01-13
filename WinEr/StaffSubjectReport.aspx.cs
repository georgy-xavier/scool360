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
    public partial class StaffSubjectReport : System.Web.UI.Page
    {
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader myReader = null;
        private OdbcDataReader myReader1 = null;
        private DataSet myDataset = null;

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
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(608))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Load_SubjectDrp();
                    loadSubjectDetails();
                }
            }
        }

        private void Load_SubjectDrp()
        {
            Drp_Subjects.Items.Clear();
            string sql = "Select tblsubjects.Id As 'SubId', tblsubjects.subject_name FROM tblsubjects ORDER BY tblsubjects.subject_name";
            myReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (myReader.HasRows)
            {
                ListItem li = new ListItem("All Subject", "-2");
                Drp_Subjects.Items.Add(li);
                while (myReader.Read())
                {
                    li = new ListItem(myReader.GetValue(1).ToString(), myReader.GetValue(0).ToString());
                    Drp_Subjects.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Subject Found", "-1");
                Drp_Subjects.Items.Add(li);
            }
        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            loadSubjectDetails();
        }

        private void loadSubjectDetails()
        {
            Label1.Text = "";
            lbl_gridmsg.Text = "";
            this.DivGrid.InnerHtml = "";
            Img_Export.Visible = false;
            if (Drp_Subjects.SelectedValue == "-2")
            {
                Load_AllSubjectsForExcel();
                Load_AllSubjectView();
                
            }
            else if (Drp_Subjects.SelectedValue != "-1")
            {
                string TString = "";
                TString = "<table width=\"100%\" cellspacing=\"0\"> <tr> <td colspan=\"2\" class=\"TableHeaderStyle\"> Subject : " + Drp_Subjects.SelectedItem.Text + " </td> </tr> <tr> <td class=\"SubHeaderStyle\">  Staff Name </td> <td  class=\"SubHeaderStyle\"> Class Name </td>  </tr> ";
                string Tr = "";
                string StaffId = "", staffName = "", ClassName = "";
                string Subjectid = Drp_Subjects.SelectedValue;
                string sql = "SELECT tbluser.Id,tbluser.SurName FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + Subjectid;
                myReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (myReader.HasRows)
                {
                    Img_Export.Visible = true;
                    while (myReader.Read())
                    {
                        StaffId = myReader.GetValue(0).ToString();
                        staffName = myReader.GetValue(1).ToString();
                        sql = "SELECT DISTINCT tblclass.ClassName FROM tblclassstaffmap INNER JOIN tblclass ON tblclass.Id=tblclassstaffmap.ClassId WHERE tblclassstaffmap.StaffId=" + StaffId + " AND tblclassstaffmap.SubjectId=" + Subjectid;
                        myReader1 = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                        if (myReader1.HasRows)
                        {
                            while (myReader1.Read())
                            {
                                ClassName = myReader1.GetValue(0).ToString();
                                Tr = Tr + "<tr> <td class=\"CellStyle\">  " + staffName + " </td><td  class=\"CellStyle\"> " + ClassName + " </td> </tr>";
                            }
                        }
                        else
                        {
                            Tr = Tr + "<tr> <td class=\"CellStyle\">  " + staffName + " </td><td  class=\"CellStyle\"> Not Assigned To Class </td> </tr>";
                        }
                    }
                    TString = TString + Tr + "</table>";
                    this.DivGrid.InnerHtml = TString;
                }
                else
                {
                    lbl_gridmsg.Text = "No staff is assigned for selected subject";
                }

            }
            else
            {
                lbl_gridmsg.Text = "Select Subject";
            }
        }

        private void Load_AllSubjectView()
        {

            string Main = "<div id=\"page-wrap\">  <table width=\"100%\">    <tr>  <td align=\"center\" style=\"padding-left:40px;width:20%\" valign=\"top\">  <table cellspacing=\"20px\"> <tr>  <td id=\"class1-button\" class=\"button\" align=\"center\" > <p>Pre-KG</p> </td>    </tr> <tr>  <td id=\"class2-button\" class=\"button\" align=\"center\" > <p>LKG A</p> </td>   </tr>  <tr>  <td id=\"class3-button\" class=\"button\" align=\"center\" > <p>LKG A</p> </td>       </tr>  </table>   </td>  <td align=\"left\"  style=\"padding-left:10px;padding-top:20px\" valign=\"top\">   <div id=\"content\" style=\"padding-left:0px\">   <div id=\"class1\" >  <table width=\"100%\" cellspacing=\"10\">  <tr>  <td class=\"StaffDetails\">  Staff1  </td>  </tr>  <tr> <td class=\"StaffDetails\">  Staff2(UKG A)  </td>  </tr>  </table> </div>  <div id=\"class2\" > <table width=\"100%\" cellspacing=\"10\">  <tr>  <td class=\"StaffDetails\">  class2 </td> </tr>  <tr> <td class=\"StaffDetails\">  class21 </td> </tr> </table>  </div> <div id=\"class3\" > class3  </div> </div></td> </tr>  </table>  </div>";
            string TSubjStr = "";
            string TStaffStr = "";

            for (int i = 1; i < Drp_Subjects.Items.Count; i++)
            {
                string StaffId = "", staffName = "", ClassName = "";
                string Subjectid = Drp_Subjects.Items[i].Value;
                TSubjStr = TSubjStr + "<tr>  <td id=\"class" + i + "-button\" class=\"button\" align=\"center\" > <p>" + Drp_Subjects.Items[i].Text + "</p> </td>    </tr>";
                TStaffStr = TStaffStr + "<div id=\"class"+i+"\" >  <table width=\"100%\" cellspacing=\"10\"> ";
                string sql = "SELECT tbluser.Id,tbluser.SurName FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + Subjectid;
                myReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (myReader.HasRows)
                {

                    while (myReader.Read())
                    {
                        StaffId = myReader.GetValue(0).ToString();
                        staffName = myReader.GetValue(1).ToString();
                        sql = "SELECT tblclass.ClassName FROM tblclassstaffmap INNER JOIN tblclass ON tblclass.Id=tblclassstaffmap.ClassId WHERE tblclassstaffmap.StaffId=" + StaffId + " AND tblclassstaffmap.SubjectId=" + Subjectid;
                        myReader1 = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                        if (myReader1.HasRows)
                        {
                            while (myReader1.Read())
                            {
                                ClassName = myReader1.GetValue(0).ToString();
                                TStaffStr = TStaffStr + "<tr>  <td class=\"StaffDetails\">   " + staffName + " (" + ClassName + ")  </td>  </tr>";

                            }
                        }
                        else
                        {
                            TStaffStr = TStaffStr + "<tr>  <td class=\"StaffDetails\">   " + staffName + "  </td>  </tr>";

                        }
                    }
                    TStaffStr = TStaffStr + "</table> </div>";

                }
                else
                {
                    TStaffStr = TStaffStr + "<tr> <td class=\"StaffDetails\">   No staff is assigned for " + Drp_Subjects.Items[i].Text + " </td> </tr> </table> </div>";


                }
               
            }
            string TMainStr = "<div id=\"page-wrap\">  <table width=\"100%\">    <tr>  <td align=\"center\" style=\"padding-left:40px;width:40%\" valign=\"top\"> <div class=\"buttongroup\"> <table cellspacing=\"5px\"> " + TSubjStr + " </table> </div>  </td>  <td align=\"left\"  style=\"padding-left:10px;padding-top:20px\" valign=\"top\">   <div id=\"content\" style=\"padding-left:0px\"> " + TStaffStr + " </div></td> </tr>  </table>  </div>";

            this.DivGrid.InnerHtml = TMainStr;
            ScriptManager.RegisterClientScriptBlock(this.updatepanel, this.updatepanel.GetType(), "AnyScript", "Onload();", true);
        }
        
        
       

        private void Load_AllSubjectsForExcel()
        {
            
            string TMain = "<table width=\"100%\" cellspacing=\"0\"> <tr> ";
            string TmainTrows = "";
            string TString = "";
            for (int i = 1; i < Drp_Subjects.Items.Count; i++)
            {
                TmainTrows = TmainTrows + "<td valign=\"top\">";
                TString = "<table width=\"100%\" cellspacing=\"0\"> <tr> <td colspan=\"2\" class=\"TableHeaderStyle\"> Subject : " + Drp_Subjects.Items[i].Text + " </td> </tr> <tr> <td class=\"SubHeaderStyle\">  Staff Name </td> <td  class=\"SubHeaderStyle\"> Class Name </td>  </tr> ";
                string Tr = "";
                string StaffId = "", staffName = "", ClassName = "";
                string Subjectid = Drp_Subjects.Items[i].Value;
                string sql = "SELECT tbluser.Id,tbluser.SurName FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + Subjectid;
                myReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                if (myReader.HasRows)
                {
                    Img_Export.Visible = true;
                    while (myReader.Read())
                    {
                        StaffId = myReader.GetValue(0).ToString();
                        staffName = myReader.GetValue(1).ToString();
                        sql = "SELECT tblclass.ClassName FROM tblclassstaffmap INNER JOIN tblclass ON tblclass.Id=tblclassstaffmap.ClassId WHERE tblclassstaffmap.StaffId=" + StaffId + " AND tblclassstaffmap.SubjectId=" + Subjectid;
                        myReader1 = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                        if (myReader1.HasRows)
                        {
                            while (myReader1.Read())
                            {
                                ClassName = myReader1.GetValue(0).ToString();
                                Tr = Tr + "<tr> <td class=\"CellStyle\">  " + staffName + " </td><td  class=\"CellStyle\"> " + ClassName + " </td> </tr>";
                            }
                        }
                        else
                        {
                            Tr = Tr + "<tr> <td class=\"CellStyle\">  " + staffName + " </td><td  class=\"CellStyle\"> Not Assigned To Class </td> </tr>";
                        }
                    }
                    TString = TString + Tr + "</table>";

                }
                else
                {
                    TString = "<table width=\"100%\" cellspacing=\"0\"> <tr> <td class=\"TableHeaderStyle\"> Subject : " + Drp_Subjects.Items[i].Text + " </td> </tr><tr><td class=\"CellStyle\" > No staff is assigned for " + Drp_Subjects.Items[i].Text + "</td></tr></table>";

                }
                TmainTrows = TmainTrows + TString + "</td>";
            }
            TMain = TMain + TmainTrows + "</tr></table>";
            Label1.Text = TMain;
        }




        protected void Img_Export_Click(object sender, EventArgs e)
        {
            if (Label1.Text != "")
            {
                 ExcelUtility.ExportBuiltStringToExcel("SubjectReport", Label1.Text, "Subject Report");               
            }
            else
            {
                if (this.DivGrid.InnerHtml == "")
                {
                    lbl_gridmsg.Text = "No Data Found For Exporting To XSL";
                }
                else
                {
                    ExcelUtility.ExportBuiltStringToExcel("SubjectReport", this.DivGrid.InnerHtml, "Subject Report");
                }
            }
        }

    }
}
