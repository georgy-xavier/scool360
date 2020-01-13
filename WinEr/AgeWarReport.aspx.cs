using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Text;
using System.Data;

namespace WinEr
{
    public partial class AgeWarReport : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        //private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private OdbcDataReader Myreader = null;
        private StudentManagerClass MyStudMang;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                    {
                        Response.Redirect("sectionerr.htm");
                    }
                    MyUser = (KnowinUser)Session["UserObj"];
                    MyStaffMang = MyUser.GetStaffObj();
                    MyStudMang = MyUser.GetStudentObj();
                    if (MyStaffMang == null)
                    {
                        Response.Redirect("RoleErr.htm");
                        //no rights for this user.
                    }
                    else if (!MyUser.HaveActionRignt(838))
                    {
                        Response.Redirect("RoleErr.htm");
                    }
                    else
                    {

                        if (!IsPostBack)
                        {
                            pnl_agewarreport.Visible = false;
                            LoadClassDetails();
                            LoadDate();
                        }
                    }
            }

            protected void btn_Ok_Click(object sender, EventArgs e)
            {
                if (drp_class.SelectedItem.ToString() == "Select class")
                {
                    lbl_err.Text = "Select a class";
                    pnl_agewarreport.Visible = false;
                }
                else
                {
                    pnl_agewarreport.Visible = true;
                    DataSet studs = GetCount();
                    GenerateReport(studs);
                }

            }

            protected void btn_excel_Click(object sender, EventArgs e)
            {
                DataSet ExcelDataSet = GetCount();
                StringBuilder strexcel = GetExcelreport(ExcelDataSet);
                if (!WinEr.ExcelUtility.ExportBuiltStringToExcel("Agewarreport", strexcel.ToString(), "Agewar.xls"))
                {

                }
                else
                {
                    lbl_err.Text = "No Data Exists To Generate Excel Format";
                }
            }
           
        #endregion


        #region Functions

            private void LoadDate()
            {
                DateTime date = System.DateTime.Now;
                int year =int.Parse(date.Year.ToString());
                DateTime _date = new DateTime(year, 9, 30,0,0,0);
                string textdate = General.GerFormatedDatVal(_date);
                txt_date.Text = textdate;
                    
            }

            private void LoadClassDetails()
            {
                string _sql = "select tblclass.ClassName, tblclass.Id as ClassId from tblclass where tblclass.Status=1 order by tblclass.Standard,tblclass.ClassName";
               Myreader= MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);
               if (Myreader.HasRows)
               {
                   drp_class.Items.Add(new ListItem("Select class", "0"));
                   while (Myreader.Read())
                   {

                       drp_class.Items.Add(new ListItem(Myreader.GetValue(0).ToString(),Myreader.GetValue(1).ToString()));
                   }
               }
               else
               {

                   drp_class.Items.Add(new ListItem("No class found", "-1"));
               }
            }

            private void GenerateReport(DataSet studs)
            {
                StringBuilder Content = new StringBuilder();
                int total = 0;
                if (studs != null && studs.Tables[0].Rows.Count > 0)
                {
                    pnl_agewarreport.Visible = true;
                    lbl_err.Text = "";
                    Content.AppendLine("<center>");

                    Content.AppendLine("<table border=\"1px\"  width=\"400px\"><tr>");
                    foreach (DataRow dr in studs.Tables[0].Rows)
                    {
                        total = int.Parse(dr["malecount"].ToString()) + int.Parse(dr["femalecount"].ToString());
                        Content.AppendLine("<td align=\"center\" colspan=\"4\"><b>" + dr["age"].ToString() + " -Year old students</b></td>");
                        Content.AppendLine("</tr>");
                        Content.AppendLine("<tr valign=\"middle\"><td align=\"center\">Total " + total + " students</td><td align=\"center\"><table><tr><td  valign=\"middle\"> <img width=\"20px\" src=\"Pics/user5.png\"  alt=\"Boys\"/ ></td><td valign=\"middle\">" + dr["malecount"].ToString() + "</td></tr></table></td><td align=\"center\"> <table><tr><td  valign=\"middle\">  <img width=\"20px\" src=\"Pics/she_user.png\"  alt=\"Girls\"/ ></td><td valign=\"middle\">" + dr["femalecount"].ToString() + "</td></tr></table></td>");
                        Content.AppendLine("</tr>");
                    }
                    Content.AppendLine("</table>");
                    Content.AppendLine("</center>");
                    div_agewarreport.InnerHtml = Content.ToString();
                }
                else
                {
                    lbl_err.Text = "No report found";
                    pnl_agewarreport.Visible = false;
                }

            }

            private DataSet GetCount()
            {
                //MyStudMang = new StudentManagerClass();
                DateTime date = General.GetDateTimeFromText(txt_date.Text);
                int age = 0;
                DateTime dob;
                string sex = "";
                string _sql = "";
                DataSet Countds = new DataSet();
                DataRow _dr;
                DataTable _dt;
                Countds.Tables.Add("StudentCount");
                _dt = Countds.Tables["StudentCount"];
                _dt.Columns.Add("age");
                _dt.Columns.Add("malecount");
                _dt.Columns.Add("femalecount");
                int gc = 0, bc = 0;
                _sql = "select sex, tblstudent.StudentName,tblstudent.DOB,tblclass.ClassName from tblstudent inner join tblclass on tblclass.Id= tblstudent.LastClassId where tblstudent.Id in(select tblstudentclassmap.StudentId from tblstudentclassmap) and tblclass.Id=" + drp_class.SelectedValue + " and tblstudent.`Status`=1";
                Myreader = MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);
                if (Myreader.HasRows)
                {
                    while (Myreader.Read())
                    {
                        sex = Myreader.GetValue(0).ToString();
                        dob = DateTime.Parse(Myreader.GetValue(2).ToString());
                        //age = date.Year - dob.Year;
                        age = MyStudMang.GetAge(dob, date);
                        gc = 0;
                        bc = 0;
                        if (sex == "Female")
                        {

                            gc++;
                        }
                        else
                        {
                            bc++;
                        }
                        int temp = 0;
                        if (Countds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in Countds.Tables[0].Rows)
                            {
                                if (int.Parse(dr["age"].ToString()) == age)
                                {
                                    temp = 1;
                                    if (sex == "Female")
                                    {

                                        dr["femalecount"] = int.Parse(dr["femalecount"].ToString()) + gc;
                                    }
                                    else
                                    {
                                        dr["malecount"] = int.Parse(dr["malecount"].ToString()) + bc;
                                    }

                                }
                            }
                            if (temp == 0)
                            {



                                _dr = _dt.NewRow();
                                _dr["age"] = age;
                                _dr["malecount"] = bc.ToString();
                                _dr["femalecount"] = gc.ToString();
                                Countds.Tables["StudentCount"].Rows.Add(_dr);

                            }

                        }
                        else
                        {


                            _dr = _dt.NewRow();
                            _dr["age"] = age;
                            _dr["malecount"] = bc.ToString();
                            _dr["femalecount"] = gc.ToString();
                            Countds.Tables["StudentCount"].Rows.Add(_dr);

                        }


                    }

                }
                return Countds;
            }

            private StringBuilder GetExcelreport(DataSet studs)
            {
                StringBuilder Content = new StringBuilder();
                int total = 0;
                if (studs != null && studs.Tables[0].Rows.Count > 0)
                {


                    Content.AppendLine("<center>");
                    Content.AppendLine("<table border=\"1px\"  width=\"400px\"><tr>");
                    Content.AppendLine("<td align=\"center\" colspan=\"4\">This excel report is for class " + drp_class.SelectedItem + " based on  " + txt_date.Text + "</td></tr>");
                    foreach (DataRow dr in studs.Tables[0].Rows)
                    {
                        total = int.Parse(dr["malecount"].ToString()) + int.Parse(dr["femalecount"].ToString());
                        Content.AppendLine("<tr>");
                        Content.AppendLine("<td align=\"center\" colspan=\"4\"><b>" + dr["age"].ToString() + " -Year old students</b></td>");
                        Content.AppendLine("</tr>");
                        Content.AppendLine("<tr valign=\"middle\"><td align=\"center\">Total " + total + " students</td><td style=\"width:15%\" align=\"center\">  Boys   " + dr["malecount"].ToString() + " </td><td align=\"center\"  style=\"width:15%\">  Girls   " + dr["femalecount"].ToString() + "</td>");
                        Content.AppendLine("</tr>");
                    }
                    Content.AppendLine("</table>");
                    Content.AppendLine("</center>");
                }
                return Content;
            }

        #endregion

         

           
    }
}
