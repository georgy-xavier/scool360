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
using WinEr;
using WebChart;
using System.Drawing;
using System.Text;

namespace WinEr
{
    public struct ExamArray
    {
        public int SubjectId;
        public int ExamId;
        public string ExamName;
        public string Mark;
        
        public bool Enable;
    }

    public partial class ExamReportPreviousBatch : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private DataSet MydataSet;
        public MysqlClass m_MysqlDb;

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
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(507))
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
                    //LoadStudentTopData();

                    LoadAllPreviousClassesToDropDown();
                    Img_Export.Visible = false;
                    LoadDetails();
                }
            }
        }

        private void LoadDetails()
        {
            if (int.Parse(Drp_Class.SelectedValue.ToString()) != -1)
            {

                int _ClassId = 0;

                if (int.Parse(Drp_Class.SelectedValue) > 0)
                {
                    _ClassId = MyStudMang.GetHistoryClassId(int.Parse(Session["StudId"].ToString()), int.Parse(Drp_Class.SelectedValue));
                }
                DataSet ExamDetails = GetExamDetails(_ClassId);
                if (ExamDetails != null)
                {


                    DataSet SubjectDetails = GetAllSubject(_ClassId, ExamDetails);

                    if (SubjectDetails != null && SubjectDetails.Tables[0].Rows.Count > 0)
                    {
                        LoadAllPreviousExamReportToGridNew(ExamDetails, SubjectDetails, _ClassId);


                        string CTR = GetExamReportDetailsToExcel(ExamDetails, SubjectDetails, _ClassId);
                        ViewState["ExcelFormat"] = CTR;
                        this.ExamReport.InnerHtml = CTR.ToString();
                    }
                    else
                    {
                        Pnl_ExamGraph.Visible = false;
                        this.MarkListArea.Visible = false;
                        this.MarkListArea1.Visible = false;
                        Lbl_indexammsg.Text = "Subject Details does not found";
                        Img_Export.Visible = false;
                        this.ExamNames.Visible = false;
                    }
                }
                else
                {
                    Pnl_ExamGraph.Visible = false;
                    this.MarkListArea.Visible = false;
                    this.MarkListArea1.Visible = false;
                    Lbl_indexammsg.Text = "No exam found";
                    Img_Export.Visible = false;
                    this.ExamNames.Visible = false;
                }

            }
            else
            {
                Pnl_ExamGraph.Visible = false;
                this.MarkListArea.Visible = false;
                this.MarkListArea1.Visible = false;
                Lbl_indexammsg.Text = "No class found";
                Img_Export.Visible = false;
                this.ExamNames.Visible = false;
            }
        }

        private string GetExamReportDetailsToExcel(DataSet ExamDetails, DataSet SubjectDetails, int _ClassId)
        {
            StringBuilder CTR = new StringBuilder();
            string studname="";
            string adno="";
            string classname = "";
            string rollno = "";
            string DOB = "";
            int studid = int.Parse(Session["StudId"].ToString());
            string Sql = "select tblview_student.StudentName, tblview_student.AdmitionNo, tblview_student.DOB,tblclass.ClassName,tblview_student.RollNo from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.Id=" + studid;
            OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            //MyReader = m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                studname = MyReader.GetValue(0).ToString();
                adno = MyReader.GetValue(1).ToString();
                classname = MyReader.GetValue(3).ToString();
                rollno = MyReader.GetValue(4).ToString();
                DOB = MyReader.GetValue(2).ToString();

            }
            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr>");
            CTR.Append("<tr><td><br/></td><td class=\"CellStyle\">Student Name: </td>  <td class=\"CellStyle\">" + studname + "</td>  </tr>");
            CTR.Append("<tr><td><br/></td><td class=\"CellStyle\">Admission No: </td>  <td class=\"CellStyle\">" + adno + "</td>  </tr>");
            CTR.Append("<tr><td><br/></td><td class=\"CellStyle\">DOB: </td>  <td class=\"CellStyle\">" + DOB + "</td>  </tr>");
            CTR.Append("<tr><td><br/></td><td class=\"CellStyle\">Class Name: </td>  <td class=\"CellStyle\">" + classname + "</td>  </tr>");
            CTR.Append("<tr><td><br/></td><td class=\"CellStyle\">Roll No: </td>  <td class=\"CellStyle\">" + rollno + "</td>  </tr>");
            
            CTR.Append("</tr></table>");

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr>");

            foreach (DataRow dr in ExamDetails.Tables[0].Rows)
            {
                string Colmns = "";

                foreach (DataRow dr1 in SubjectDetails.Tables[0].Rows)
                {
                    if (int.Parse(dr1["SchId"].ToString()) == int.Parse(dr["Id"].ToString()))
                    {
                        if (Colmns != "") Colmns = Colmns + ",";


                        Colmns = Colmns + dr1["MarkColumn"];
                    }
                }
                string sql2 = "", sql3 = "";

                if (int.Parse(Drp_Class.SelectedValue) > 0)
                {
                    if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                    {
                        sql2 = "select " + Colmns + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap_history.StudentId where tblstudentmark.ExamSchId=" + dr["Id"] + " and tblstudentmark.StudId=" + int.Parse(Session["StudId"].ToString());
                    }
                    else
                    {
                        sql2 = "select " + Colmns + ", tblstudentmark_history.TotalMax , tblstudentmark_history.TotalMark , tblstudentmark_history.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark_history on tblstudentmark_history.StudId = tblstudentclassmap_history.StudentId where tblstudentmark_history.ExamSchId=" + dr["Id"] + " and tblstudentmark_history.StudId=" + int.Parse(Session["StudId"].ToString());
                    }
                }
                else
                {
                    sql3 = "select " + Colmns + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap_history.StudentId where tblstudentmark.ExamSchId=" + dr["Id"] + " and tblstudentmark.StudId=" + int.Parse(Session["StudId"].ToString());

                    sql2 = "select " + Colmns + ", tblstudentmark_history.TotalMax , tblstudentmark_history.TotalMark , tblstudentmark_history.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark_history on tblstudentmark_history.StudId = tblstudentclassmap_history.StudentId where tblstudentmark_history.ExamSchId=" + dr["Id"] + " and tblstudentmark_history.StudId=" + int.Parse(Session["StudId"].ToString());

                    sql2 = "(" + sql2 + " ) union (" + sql3 + ")";
                }
                OdbcDataReader Reader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql2);
                if (Reader1.HasRows)
                {
                    double val = 0;


                    CTR.Append("<td valign=\"top\"><table runat=\"server\" width=\"100%\"><tr><td colspan=\"2\" class=\"TableHeaderStyle\"><b>" + dr["ClassName"] + ":" + dr["Batch"] + "</b></td></tr>  <tr><td colspan=\"2\" class=\"SubHeaderStyle\"><b>" + dr["ExamName"] + " (" + dr["Period"] + ")" + "</b></td></tr>  <tr><td colspan=\"2\"></td></tr>");

                    int j = 0;
                    foreach (DataRow dr1 in SubjectDetails.Tables[0].Rows)
                    {
                        if (int.Parse(dr1["SchId"].ToString()) == int.Parse(dr["Id"].ToString()))
                        {
                            double.TryParse(Reader1[dr1["MarkColumn"].ToString()].ToString(), out val);
                            if (val > 0)
                            {
                                CTR.Append("<tr><td class=\"CellStyle\">" + dr1["subname"] + "</td>  <td class=\"CellStyle\">" + val.ToString(".") + " / " + double.Parse(dr1["MaxMark"].ToString()).ToString(".") + "</td>  </tr>");
                            }
                            else
                            {
                                CTR.Append("<tr><td class=\"CellStyle\">" + dr1["subname"] + "</td>  <td class=\"CellStyle\">" + Reader1[dr1["MarkColumn"].ToString()].ToString() + "/" + double.Parse(dr1["MaxMark"].ToString()).ToString(".") + "</td>  </tr>");
                            }


                        }

                        j++;
                    }
                  

                    CTR.Append("<tr><td class=\"CellStyle\">Total Mark </td>  <td class=\"CellStyle\">" + Reader1["TotalMark"].ToString() + "</td>  </tr>");
                    CTR.Append("<tr><td class=\"CellStyle\">Total Maximum </td>  <td class=\"CellStyle\">" + Reader1["TotalMax"].ToString() + "</td>  </tr>");
                                        
                    //CTR.Append("<tr><td class=\"CellStyle\">Average </td>  <td class=\"CellStyle\">" + double.Parse(Reader1["Avg"].ToString()).ToString("0.00") + "</td>  </tr>");
                    if (Reader1["Avg"].ToString().Trim() != "")
                        CTR.Append("<tr><td class=\"CellStyle\">Average </td>  <td class=\"CellStyle\">" + double.Parse(Reader1["Avg"].ToString()).ToString("0.00") + "</td>  </tr>");
                    //else
                    //    CTR.Append("<tr><td class=\"CellStyle\">Average </td>  <td class=\"CellStyle\">" + "0.00" + "</td>  </tr>");


                    CTR.Append("<tr><td class=\"CellStyle\">Grade </td>  <td class=\"CellStyle\">" + Reader1["Grade"].ToString() + "</td>  </tr>");
                    CTR.Append("<tr><td class=\"CellStyle\">Result </td>  <td class=\"CellStyle\">" + Reader1["Result"].ToString() + "</td>  </tr>");
                    CTR.Append("<tr><td class=\"CellStyle\">Rank </td>  <td class=\"CellStyle\">" + Reader1["Rank"].ToString() + "</td>  </tr>");

                    CTR.Append("</table>");
                }






            }
            CTR.Append("</td></tr></table>");
            return CTR.ToString();
        }

        private DataSet GetAllSubject(int _ClassId, DataSet ExamDetails)
        {

            string sqlSubject = "", sqlSubjectCurrnet = "";

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubjectCurrnet = @"select distinct( tblexammark .SubjectId), tblsubjects.subject_name as subname, tblexammark.MarkColumn, 
                                    tblclassexamsubmap.MaxMark   ,  tblexamschedule.Id as SchId from tblexammark 
                                    inner join tblsubjects on  tblsubjects.id=tblexammark.SubjectId
                                    inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId 
                                    inner join  tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId and  tblclassexamsubmap.SubId= tblexammark .SubjectId  where ";
                }
                else
                {
                    sqlSubject = "select distinct(tblexammark_history.SubjectId), tblexammark_history.SubjectName as subname, tblexammark_history.MarkColumn, tblexammark_history.MaxMark   , tblexammark_history.ExamSchId as SchId   from tblexammark_history where tblexammark_history.ExamSchId in ( select tblexamschedule_history.Id from tblexamschedule_history where";
                }
            }
            else
            {
                sqlSubjectCurrnet = @"select distinct( tblexammark .SubjectId), tblsubjects.subject_name as subname, tblexammark.MarkColumn, 
                                    tblclassexamsubmap.MaxMark  ,  tblexamschedule.Id as SchId from tblexammark 
                                    inner join tblsubjects on  tblsubjects.id=tblexammark.SubjectId
                                    inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId 
                                    inner join  tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId and  tblclassexamsubmap.SubId= tblexammark .SubjectId  where ";

                sqlSubject = "select distinct(tblexammark_history.SubjectId), tblexammark_history.SubjectName as subname, tblexammark_history.MarkColumn, tblexammark_history.MaxMark , tblexammark_history.ExamSchId as SchId from tblexammark_history where tblexammark_history.ExamSchId in ( select tblexamschedule_history.Id from tblexamschedule_history where";
            }




            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubjectCurrnet = sqlSubjectCurrnet + @" tblexamschedule.BatchId =" + Drp_Class.SelectedValue + " and tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in    (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId= " + int.Parse(Session["StudId"].ToString()) + "))";

                }
                else
                {
                    sqlSubject = sqlSubject + " tblexamschedule_history.BatchId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ClassId=" + _ClassId + ")";
                }

            }
            else
            {
//                sqlSubjectCurrnet = sqlSubjectCurrnet + "  tblexamschedule.BatchId in     (select distinct(tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and    tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in  (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId   =" + int.Parse(Session["StudId"].ToString()) + "))";
                sqlSubjectCurrnet = sqlSubjectCurrnet + "     tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in  (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId   =" + int.Parse(Session["StudId"].ToString()) + "))";

                sqlSubject = sqlSubject + " tblexamschedule_history.BatchId in (select distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + "))";
            }

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubject = sqlSubjectCurrnet + " order by tblexammark.SubjectOrder";
                }
                else
                {
                    sqlSubject = sqlSubject + " order by tblexammark_history.SubjectOrder";
                }
            }
            else
            {
                sqlSubject = "(" + sqlSubject + " ) union ( " + sqlSubjectCurrnet + " order by tblexammark.SubjectOrder )";
            }



            DataSet Dt = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlSubject);
            return Dt;

        }

        private DataSet GetExamDetails(int _ClassId)
        {


            string sql1 = ""; string sql = "";

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sql1 = @"select tblexamschedule.Id, tblexammaster.ExamName, (select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule.PeriodId) as Period, 
                        tblclass.ClassName as ClassName, (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule.BatchId) as Batch from tblexamschedule 
                        inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId
                        inner join tblexammaster on tblexammaster.id= tblclassexam.ExamId
                        inner join tblclass on  tblclass.id= tblclassexam.ClassId   ";

                    sql1 = sql1 + " where tblexamschedule.BatchId =" + Drp_Class.SelectedValue + " and tblclass.id=" + _ClassId;

                    sql1 = sql1 + " order by tblexamschedule.Id ";
                    sql = sql1;
                }
                else
                {
                    sql = "select tblexamschedule_history.Id, tblexamschedule_history.ExamName, ( select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule_history.PeriodId) as Period, (SELECT tblclass.ClassName from tblclass where tblclass.Id= tblexamschedule_history.ClassId) as ClassName, (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule_history.BatchId) as Batch from tblexamschedule_history where";
                    if (_ClassId > 0)
                    {
                        sql = sql + " tblexamschedule_history.BatchId in (select Distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + "      and   tblexamschedule_history.ClassId =" + _ClassId + " and tblexamschedule_history.BatchId="+Drp_Class.SelectedValue+") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ")";
                    }
                    else
                    {
                        sql = sql + " tblexamschedule_history.BatchId in (select Distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ")";
                    }
                    sql = sql + " order by tblexamschedule_history.Id ";
                }

            }
            else
            {
                sql1 = @"select tblexamschedule.Id, tblexammaster.ExamName, (select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule.PeriodId) as Period, 
                        tblclass.ClassName as ClassName, (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule.BatchId) as Batch from tblexamschedule 
                        inner join tblclassexam on tblclassexam.Id= tblexamschedule.ClassExamId
                        inner join tblexammaster on tblexammaster.id= tblclassexam.ExamId
                        inner join tblclass on  tblclass.id= tblclassexam.ClassId  
                    inner join tblstudentclassmap_history on  tblstudentclassmap_history.ClassId= tblclassexam.ClassId ";

                sql1 = sql1 + " where  tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString());

                sql1 = sql1 + " order by tblexamschedule.Id ";


                sql = "select tblexamschedule_history.Id, tblexamschedule_history.ExamName, ( select tblperiod.Period from tblperiod where tblperiod.Id= tblexamschedule_history.PeriodId) as Period, (SELECT tblclass.ClassName from tblclass where tblclass.Id= tblexamschedule_history.ClassId) as ClassName, (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblexamschedule_history.BatchId) as Batch from tblexamschedule_history where";
                sql = sql + " tblexamschedule_history.BatchId in (select Distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString());
                sql = sql + " order by tblexamschedule_history.Id )";
                sql = "(" + sql + ") union (" + sql1 + ")";

            }


            DataSet Dt = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            return Dt;



        }

        private void LoadAllPreviousClassesToDropDown()
        {
            Drp_Class.Items.Clear();
            string sql = "select tblstudentclassmap_history.BatchId, CONCAT( (select tblclass.ClassName from tblclass where tblclass.Id=tblstudentclassmap_history.ClassId), ':', (select tblbatch.BatchName from tblbatch where tblbatch.Id= tblstudentclassmap_history.BatchId)) as ClassName from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + " order by tblstudentclassmap_history.BatchId";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Class.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                Drp_Class.Items.Add(new ListItem("No class found", "-1"));
            }
        }

        //private void LoadStudentTopData()
        //{
        //    //string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), MyUser.GetImageUrl("StudentImage", int.Parse(Session["StudId"].ToString())), int.Parse(Session["StudType"].ToString()));
        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
            
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}
        
        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            string _WorkSheetName = "ExamDetails";
            string FileName = "PreviousBatchExamReport";

            string _TableString = (string)ViewState["ExcelFormat"];// GetExamReportDetailsToExcel(); 

            if (!WinEr.ExcelUtility.ExportBuiltStringToExcel(FileName, _TableString, _WorkSheetName))
            {
                WC_MessageBox.ShowMssage("This function need Ms office");
            }
        }

        private void LoadAllPreviousExamReportToGridNew(DataSet ExamDetails, DataSet SubjectDetails, int _ClassId)
        {
            int i=0,examCount=0;
           
            ExamArray[,] _ExamArray = new ExamArray[SubjectDetails.Tables[0].Rows.Count, ExamDetails.Tables[0].Rows.Count];

            foreach (DataRow dr in ExamDetails.Tables[0].Rows)
            {
                string Colmns = "";

                foreach (DataRow dr1 in SubjectDetails.Tables[0].Rows)
                {
                    if (int.Parse(dr1["SchId"].ToString()) == int.Parse(dr["Id"].ToString()))
                    {
                        if (Colmns != "") Colmns = Colmns + ",";


                        Colmns = Colmns + dr1["MarkColumn"];
                    }
                }
                string sql2 = "", sql3 = "";

                if (int.Parse(Drp_Class.SelectedValue) > 0)
                {
                    if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                    {
                        sql2 = "select " + Colmns + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap_history.StudentId where tblstudentmark.ExamSchId=" + dr["Id"] + " and tblstudentmark.StudId=" + int.Parse(Session["StudId"].ToString());
                    }
                    else
                    {
                        sql2 = "select " + Colmns + ", tblstudentmark_history.TotalMax , tblstudentmark_history.TotalMark , tblstudentmark_history.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark_history on tblstudentmark_history.StudId = tblstudentclassmap_history.StudentId where tblstudentmark_history.ExamSchId=" + dr["Id"] + " and tblstudentmark_history.StudId=" + int.Parse(Session["StudId"].ToString());
                    }
                }
                else
                {
                    sql3 = "select " + Colmns + ", tblstudentmark.TotalMax , tblstudentmark.TotalMark , tblstudentmark.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark on tblstudentmark.StudId = tblstudentclassmap_history.StudentId where tblstudentmark.ExamSchId=" + dr["Id"] + " and tblstudentmark.StudId=" + int.Parse(Session["StudId"].ToString());

                    sql2 = "select " + Colmns + ", tblstudentmark_history.TotalMax , tblstudentmark_history.TotalMark , tblstudentmark_history.`Avg`, Grade, Result, Rank  from tblstudentclassmap_history inner join tblstudentmark_history on tblstudentmark_history.StudId = tblstudentclassmap_history.StudentId where tblstudentmark_history.ExamSchId=" + dr["Id"] + " and tblstudentmark_history.StudId=" + int.Parse(Session["StudId"].ToString());

                    sql2 = "(" + sql2 + " ) union (" + sql3 + ")";
                }
                OdbcDataReader Reader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql2);
                if (Reader1.HasRows)
                {
                    double val = 0;


                    
                    foreach (DataRow dr1 in SubjectDetails.Tables[0].Rows)
                    {
                     
                        if (int.Parse(dr1["SchId"].ToString()) == int.Parse(dr["Id"].ToString()))
                        {
                            double.TryParse(Reader1[dr1["MarkColumn"].ToString()].ToString(), out val);
                            if (val > 0)
                            {
                                _ExamArray[i, examCount].Mark = (double.Parse(val.ToString(".")) / (double.Parse(dr1["MaxMark"].ToString())) * 100).ToString(".");
                                _ExamArray[i, examCount].ExamName = dr["ExamName"].ToString();
                                _ExamArray[i, examCount].ExamId=int.Parse(dr["Id"].ToString());
                                _ExamArray[i, examCount].SubjectId = int.Parse(dr1["SubjectId"].ToString());
                            }
                            else
                            {
                                _ExamArray[i, examCount].Mark = "";

                            }
                            i = i + 1;

                        }

                          
                    }
                    i = 0;
                }
                examCount = examCount + 1;
            }

            Session["ExamReportArray"] = _ExamArray;
            Img_Export.Visible = true;
            Pnl_ExamGraph.Visible = true;
            this.MarkListArea.Visible = true;
            this.MarkListArea1.Visible = true;
            this.ExamNames.Visible = true;
            Lbl_indexammsg.Text = "";
            LoadConditionDropDownWithSubject();
            LoadPerformanceGraphWithExamData();  
        }

        private void LoadPerformanceGraphWithExamData()
        {
            //DataSet ExamDataSet = (DataSet)ViewState["ConsoldatedExamData"];
            //DataTable dt = ExamDataSet.Tables["Exam"];

            ExamArray[,] _ExamArray = (ExamArray[,])Session["ExamReportArray"];
            int _SelectedCondition = int.Parse(Drp_SelectList.SelectedValue);

            if (_SelectedCondition > 0)
            {
                int _RowCount = _ExamArray.GetLength(0);
                int _ColumnCount = _ExamArray.GetLength(1);

                string _ExamList = " <table width=\"100%\">";

                //for (int k = 0; k < _ColumnCount; k++)
                //{
                //    _ExamList = _ExamList + "<tr><td class=\"CellStyle\">Exam " + (k + 1).ToString() + "</td><td class=\"CellStyle\">" + _ExamArray[_SelectedCondition, k].ExamName + "</td></tr>";
                //}
                //_ExamList = _ExamList + " </table>";

                float MaxVal = 100;
                float _val = 0;
              
                if (_RowCount > 0)
                {
                    ColumnChart chart_Bar = new ColumnChart();

                    Chart chart_Line = new SmoothLineChart();

                    ChartPointCollection chart_Line_data = chart_Line.Data;

                    for (int i = 0; i < _ColumnCount; i++)
                    {
                        for (int j = 0; j < _RowCount; j++)
                        {
                            if ((_ExamArray[j, i].SubjectId == _SelectedCondition) && (float.TryParse(_ExamArray[j, i].Mark, out _val)))
                            {
                                //DataColumn dc = _ExamArray[_SelectedCondition, i].ExamName;
                                string ColumnName = "Exam " + (i + 1).ToString();// _ExamArray[_SelectedCondition, i].ExamName;
                                if (MaxVal < _val)
                                {
                                    MaxVal = _val;
                                }
                                chart_Line_data.Add(new ChartPoint(ColumnName, _val));
                                chart_Bar.Data.Add(new ChartPoint(ColumnName, _val));
                                _ExamList = _ExamList + "<tr><td class=\"CellStyle\">Exam " + (i + 1).ToString() + "</td><td class=\"CellStyle\">" + _ExamArray[j, i].ExamName + "</td></tr>";
                            }
                        }
                    }

                    chart_Line.Line.Width = 2;
                    chart_Line.Line.Color = Color.RoyalBlue;
                    chart_Line.Legend = Drp_SelectList.SelectedValue;
                    chartcontrol_ExamChart.Charts.Clear();
                    chartcontrol_ExamChart.Charts.Add(chart_Line);

                    //chart_Line.DataLabels.Visible = true;

                    chartcontrol_ExamChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    chartcontrol_ExamChart.YTitle.StringFormat.Alignment = StringAlignment.Near;


                    if (MaxVal != 100)
                    {
                        MaxVal = MaxVal + 10;
                    }
                    chart_Bar.Shadow.Visible = true;
                    chart_Bar.DataLabels.Visible = true;
                    chart_Bar.MaxColumnWidth = 20;
                    chartcontrol_ExamChart.YCustomEnd = MaxVal;
                    chartcontrol_ExamChart.Charts.Add(chart_Bar);
                    chart_Bar.Fill.Color = System.Drawing.Color.RoyalBlue;
                    chartcontrol_ExamChart.Background.Color = Color.White;
                    chartcontrol_ExamChart.RedrawChart();
                    _ExamList = _ExamList + " </table>";
                    this.ExamNames.InnerHtml = _ExamList;
                }
            }
        }

        private void LoadConditionDropDownWithSubject()
        {

            int _ClassId = 0;
            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                _ClassId = MyStudMang.GetHistoryClassId(int.Parse(Session["StudId"].ToString()), int.Parse(Drp_Class.SelectedValue));
            }

            string sqlSubject = "", sqlSubjectCurrnet = "";

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubjectCurrnet = @"select distinct( tblexammark .SubjectId), tblsubjects.subject_name  as subject_name     from tblexammark 
                                    inner join tblsubjects on  tblsubjects.id=tblexammark.SubjectId
                                    inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId 
                                    inner join  tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId and  tblclassexamsubmap.SubId= tblexammark .SubjectId  where ";
                }
                else
                {
                    sqlSubject = "select distinct(tblexammark_history.SubjectId), tblexammark_history.SubjectName as subject_name from tblexammark_history where tblexammark_history.ExamSchId in ( select tblexamschedule_history.Id from tblexamschedule_history where";
                }
            }
            else
            {
                sqlSubjectCurrnet = @"select distinct( tblexammark .SubjectId), tblsubjects.subject_name as subject_name  from tblexammark 
                                    inner join tblsubjects on  tblsubjects.id=tblexammark.SubjectId
                                    inner join tblexamschedule on tblexamschedule.Id=tblexammark.ExamSchId 
                                    inner join  tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblexamschedule.ClassExamId and  tblclassexamsubmap.SubId= tblexammark .SubjectId  where ";

                sqlSubject = "select distinct(tblexammark_history.SubjectId), tblexammark_history.SubjectName as subject_name from tblexammark_history where tblexammark_history.ExamSchId in ( select tblexamschedule_history.Id from tblexamschedule_history where";
            }




            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubjectCurrnet = sqlSubjectCurrnet + @" tblexamschedule.BatchId =" + Drp_Class.SelectedValue + " and tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in    (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId= " + int.Parse(Session["StudId"].ToString()) + "))";

                }
                else
                {
                    sqlSubject = sqlSubject + " tblexamschedule_history.BatchId=" + Drp_Class.SelectedValue + " and tblexamschedule_history.ClassId=" + _ClassId + ")";
                }

            }
            else
            {
                //     sqlSubjectCurrnet = sqlSubjectCurrnet + "  tblexamschedule.BatchId in     (select distinct(tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and    tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in  (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId   =" + int.Parse(Session["StudId"].ToString()) + "))";
                sqlSubjectCurrnet = sqlSubjectCurrnet + "     tblexamschedule.ClassExamId   in (select tblclassexam.Id  from tblclassexam where tblclassexam.ClassId in  (select  Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId   =" + int.Parse(Session["StudId"].ToString()) + "))";

              
                sqlSubject = sqlSubject + " tblexamschedule_history.BatchId in (select distinct( tblstudentclassmap_history.BatchId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + ") and tblexamschedule_history.ClassId in (select Distinct( tblstudentclassmap_history.ClassId) from tblstudentclassmap_history where tblstudentclassmap_history.StudentId=" + int.Parse(Session["StudId"].ToString()) + "))";
           
            }

            if (int.Parse(Drp_Class.SelectedValue) > 0)
            {
                if (int.Parse(Drp_Class.SelectedValue) == MyUser.CurrentBatchId)
                {
                    sqlSubject = sqlSubjectCurrnet + " order by tblexammark.SubjectOrder";
                }
                else
                {
                    sqlSubject = sqlSubject + " order by tblexammark_history.SubjectOrder";
                }
            }
            else
            {
                sqlSubject = "(" + sqlSubject + " ) union ( " + sqlSubjectCurrnet + " order by tblexammark.SubjectOrder )";
            }



            DataSet Dt = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlSubject);
            Drp_SelectList.Items.Clear();

            if (Dt != null && Dt.Tables[0].Rows.Count > 0)
            {
                int i = 0;


                foreach (DataRow Dr in Dt.Tables[0].Rows)
                {
                    ListItem li = new ListItem(Dr["subject_name"].ToString(), Dr["SubjectId"].ToString());
                    Drp_SelectList.Items.Add(li);
                    i++;
                }
            }
            else
            {
                Drp_SelectList.Items.Add(new ListItem("No Conditions found", "-1"));
            }


        }

        protected void Drp_SelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPerformanceGraphWithExamData();
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetails();
        }







    }
}


































