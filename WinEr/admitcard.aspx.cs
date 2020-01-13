using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WinEr
{
    public partial class admitcard : System.Web.UI.Page
    {

        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private ExamManage MyExamMang;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            else if (Request.QueryString["ClassId"] == null)
            {
                //Response.Redirect("CollectFee.aspx");

            }

            else
            {
                MyUser = (KnowinUser)Session["UserObj"];
                MyExamMang = MyUser.GetExamObj();
                // MyFeeMang = MyUser.GetFeeObj();
                if (MyExamMang == null)
                {
                    Response.Redirect("Default.aspx");
                    //no rights for this user.
                }
                else
                {
                    if (!IsPostBack)
                    {
                        // string BillType = Request.QueryString["BillType"].ToString();
                        int class_id = int.Parse(Request.QueryString["ClassId"]);
                        int current_batch = MyUser.CurrentBatchId;
                        int exam_id = int.Parse(Request.QueryString["ExamId"]);
                        int period_id = int.Parse(Request.QueryString["PeriodId"]);
                        string exam_name = Request.QueryString["ExamName"];

                        //StudentAdmitCardDetails objStudentDetails = new StudentAdmitCardDetails();



                        StudentAdmitCardDetails[] objStudentDetails = Getstudentdetails(class_id, current_batch,exam_name);


                        ExamSchedule[] objExamSchedule = Getexamschedule(exam_id, period_id, class_id);


                        AdmitCardDetails objAdmitCardDetails = new AdmitCardDetails();
                        objAdmitCardDetails.objStudentDetails = objStudentDetails;
                        objAdmitCardDetails.objExamSchedule = objExamSchedule;

                        System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string sJSON = oSerializer.Serialize(objAdmitCardDetails);

                       
                        Hdn_admitcardJSON.Value = sJSON;
                        HiddenField1.Value = oSerializer.Serialize(objStudentDetails);
                        HiddenField2.Value = oSerializer.Serialize(objExamSchedule);
                        
                        string examjson = oSerializer.Serialize(objExamSchedule);

                        string examschedule = "";
                        
                        JArray examdetails = JArray.Parse(examjson);
                        for (int i = 0; i < examdetails.Count; i++)
                        {


                            string subname = (string)examdetails[i]["SubjectName"]; ;
                            string Date = (string)examdetails[i]["Date"];
                            //string Day = (string)examdetails[i]["Day"];
                            string StartTime = (string)examdetails[i]["StartTime"];
                            string EndTime = (string)examdetails[i]["EndTime"];


                            //JObject obj = JObject.Parse(sJSON);


                            examschedule = examschedule + "<tr><td > " + subname + "</td><td >" + Date + "</td><td>" + StartTime + " To " + EndTime + " </td> </tr>";
                            //examschedule = examschedule + "<tr><td > " + subname + "</td><td >" + Day + "</td><td >" + Date + "</td><td>" + StartTime + " To " + EndTime + " </td> </tr>";
                        }

                        //string heading = "<tr><td>Exam</td><td>Day</td><td>Date</td><td>Time</td></tr>";
                        string heading = "<tr><td>Exam</td><td>Date</td><td>Time</td></tr>";

                        Hdn_ExamJSON.Value = heading + examschedule;

                       


                        //SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                        //string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
                        //FilePath += "\\UpImage\\";

                        SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                        string FilePath = "FileRepository/" + objSchool.FilePath + "/UpImage/";
                        HdnFld_ServerPath.Value = FilePath;
                    }
                }
            }
        }

        private StudentAdmitCardDetails[] Getstudentdetails(int classid, int batchid, string ExamName)
        {
            //studentname = "";
            //sex = "";
            //DOB = "";
            //classname = "";
            //parentname = "";
            //rollno = "";
            //address = "";
            int count = 0;
            DataSet studentdata;
            StudentAdmitCardDetails[] group = null;


            string sql = "SELECT map.StudentId,stud.StudentName,stud.GardianName,date_format(stud.DOB,'%d/%m/%Y') as DOB,stud.Sex,stud.Address,tblclass.ClassName,stud.RollNo FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid inner join tblclass on tblclass.id = stud.LastClassId where stud.status=1 and  map.ClassId=" + classid + " and map.RollNo<>-1 and map.BatchId=" + batchid + " order by map.RollNo";
            studentdata = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (studentdata != null && studentdata.Tables != null && studentdata.Tables[0].Rows.Count > 0)
            {
                group = new StudentAdmitCardDetails[studentdata.Tables[0].Rows.Count];
                foreach (DataRow dr in studentdata.Tables[0].Rows)
                {
                    StudentAdmitCardDetails objstudentdetails = new StudentAdmitCardDetails();
                    objstudentdetails.StudentName = dr[1].ToString();
                    objstudentdetails.ParentName = dr[2].ToString();
                    objstudentdetails.DOB = dr[3].ToString();
                    objstudentdetails.sex = dr[4].ToString();
                    objstudentdetails.Address = dr[5].ToString();
                    objstudentdetails.ClassName = dr[6].ToString();
                    objstudentdetails.RollNo = dr[7].ToString();
                    objstudentdetails.ExamName = ExamName;
                    group[count] = objstudentdetails;
                    count += 1;
                }

            }
            return group;
        }

        private ExamSchedule[] Getexamschedule(int examid, int periodid, int classid)
        {
            //Subjectname = "";
            //day = "";
            //starttime = "";
            //endtime = "";
            //date = "";
            int count = 0;
            DataSet examdata;
            ExamSchedule[] lstgroup = null;

            string sql = "select tblsubjects.subject_name, tblsubjects.SubjectCode,tblexammark.ExamDate, tbltimeslot.StartTime, tbltimeslot.EndTime, tblclassexamsubmap.MinMark , tblclassexamsubmap.MaxMark from tblclassexam inner join tblexamschedule on tblclassexam.Id= tblexamschedule.ClassExamId AND tblexamschedule.BatchId=" + MyUser.CurrentBatchId + " AND tblexamschedule.PeriodId=" + periodid + " inner join tblclassexamsubmap on tblclassexamsubmap.ClassExamId= tblclassexam.Id inner join tblexammark on tblexammark.ExamSchId= tblexamschedule.Id AND tblexammark.SubjectId= tblclassexamsubmap.SubId inner join tbltimeslot on tbltimeslot.Id= tblexammark.TimeSlotId inner join tblsubjects on tblsubjects.Id= tblexammark.SubjectId where tblclassexam.ClassId=" + classid + " AND tblclassexam.ExamId=" + examid + " order by tblexammark.ExamDate";
            examdata = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (examdata != null && examdata.Tables != null && examdata.Tables[0].Rows.Count > 0)
            {
                lstgroup = new ExamSchedule[examdata.Tables[0].Rows.Count];
                foreach (DataRow dr in examdata.Tables[0].Rows)
                {

                    ExamSchedule objExamschedule = new ExamSchedule();

                    objExamschedule.SubjectName = dr[0].ToString();
                     
                    
                    DateTime da = Convert.ToDateTime(dr[2]);
                    objExamschedule.Date = da.ToString("dd/M/yyyy");
                    //objExamschedule.Day = da.ToString("dddd");

                    
                    objExamschedule.StartTime =dr[3].ToString();
                    objExamschedule.EndTime = dr[4].ToString();

                    lstgroup[count] = objExamschedule;
                    count += 1;
                }

            }
            return lstgroup;
        }




        //private void LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, string C_LogoUrl, string C_Name, string Address)
        //{
        //    string table = "";
        //    DataSet data_Fee, TemplateSet;

        //    string _Period = "";
        //    string _FeeName = "";
        //    double _total = 0;
        //    string _Batch = "";
        //    string sql = "";
        //    string Templates = "";
        //    sql = "select tblfeebilltemplates.Template from tblfeebilltemplates where tblfeebilltemplates.IsActive=1 and TemplateName='HTML TEMPLATE'";
        //    TemplateSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    if (TemplateSet != null && TemplateSet.Tables[0].Rows.Count > 0)
        //    {
        //        Templates = TemplateSet.Tables[0].Rows[0][0].ToString();
        //    }
        //    if (Templates != "")
        //    {

        //        Templates = Templates.Replace("($ClgName$)", C_Name);
        //        Templates = Templates.Replace("($ClgAddress$)", Address);
        //        Templates = Templates.Replace("($StudentName$)", StudentName);
        //        Templates = Templates.Replace("($Class$)", ClassName);
        //        Templates = Templates.Replace("($RollNo$)", RollNo);
        //        Templates = Templates.Replace("($AdmNo$)", AdmissionNo);
        //        Templates = Templates.Replace("($Receiptno$)", BillNo);
        //        Templates = Templates.Replace("($Date$)", date);



        //        int count = 1;
        //        sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
        //        data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        //        table = "<tr><td style=\"width:5%;border:#000 thin solid;\">SL.NO</td><td style=\"width:55%;border:#000 thin solid;\">FEE PARTICULARS</td><td style=\"width:20%;border:#000 thin solid;\">DURATION</td><td style=\"width:10%;border:#000 thin solid;\"> AMOUNT</td></tr>";
        //        if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in data_Fee.Tables[0].Rows)
        //            {

        //                if (_Period == "" && _FeeName == "" && _Batch == "")
        //                {
        //                    _Period = dr[0].ToString();
        //                    _FeeName = dr[1].ToString();
        //                    _Batch = dr[4].ToString();
        //                    table = table + "<tr><td style=\"border-right:#000 thin solid;border-left:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";



        //                    if ("Discount" != dr[2].ToString())
        //                    {
        //                        _total = double.Parse(dr[3].ToString());
        //                    }
        //                }
        //                else if (_Period == dr[0].ToString() && _FeeName == dr[1].ToString() && _Batch == dr[4].ToString())
        //                {
        //                    _Period = dr[0].ToString();
        //                    _FeeName = dr[1].ToString();
        //                    _Batch = dr[4].ToString();
        //                    table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";

        //                    if ("Discount" != dr[2].ToString())
        //                    {
        //                        _total = _total + double.Parse(dr[3].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    _Period = dr[0].ToString();
        //                    _FeeName = dr[1].ToString();
        //                    _Batch = dr[4].ToString();
        //                    //table = table + "<tr><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\"> &nbsp;&nbsp;&nbsp;&nbsp; </td><td class=\"tdStyle\">&nbsp;&nbsp;&nbsp;&nbsp;</td><td style=\"background-color:Silver\" class=\"tdStyle\"> TOTAL </td><td style=\"background-color:Silver\" class=\"tdStyle\">" + _total.ToString() + "</td></tr>";
        //                    table = table + "<tr><td style=\"border-right:#000 thin solid;\">" + count + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _FeeName + "-" + dr[2].ToString() + "</td><td style=\"border-right:#000 thin solid;\" align=\"left\">" + _Period + "</td><td style=\"border-right:#000 thin solid;\">" + dr[3].ToString() + "</td> </tr>";

        //                    if ("Discount" != dr[2].ToString())
        //                    {
        //                        _total = _total + double.Parse(dr[3].ToString());
        //                    }
        //                }
        //                count = count + 1;
        //            }
        //        }

        //        table = table + "   <tr> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\"> IN WORDS : ($amtinwords$) </td> <td style=\"border:#000 thin solid;\" align=\"right\">TOTAL </td> <td style=\"border:#000 thin solid;\" colspan=\"2\" align=\"left\">" + _total + "</td></tr>";


        //        string AmtInwords = MyFeeMang.Convert_Number_To_Words(int.Parse(_total.ToString()));
        //        table = table.Replace("($amtinwords$)", AmtInwords);

        //        Templates = Templates.Replace("($Batch$)", _Batch);
        //        Templates = Templates.Replace("($FeeContent$)", table);

        //        //this.FeeDetails.InnerHtml = Templates;

        //    }
        //    else
        //    {
        //        //this.FeeDetails.InnerHtml = "Error: Error in reading templates. Please contact to Support Team";
        //    }


        //}

        //private FeeDetails[] LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, out string TotalInWrds)
        //{
        //    DataSet data_Fee;
        //    string sql = "";
        //    int count = 0;
        //    double Total = 0;
        //    TotalInWrds = "";
        //    FeeDetails[] lstgroup = null;

        //    sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
        //    data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
        //    {
        //        lstgroup = new FeeDetails[data_Fee.Tables[0].Rows.Count];
        //        foreach (DataRow dr in data_Fee.Tables[0].Rows)
        //        {
        //            FeeDetails objFeedetails = new FeeDetails();

        //            objFeedetails.SlNo = count + 1;
        //            objFeedetails.FeeName = dr[1].ToString();
        //            objFeedetails.Period = dr[0].ToString();
        //            objFeedetails.AccountName = dr[2].ToString();
        //            objFeedetails.Amount = double.Parse(dr[3].ToString());
        //            objFeedetails.BatchName = dr[4].ToString();

        //            lstgroup[count] = objFeedetails;
        //            Total += objFeedetails.Amount;
        //            count += 1;
        //        }

        //        TotalInWrds = MyFeeMang.Convert_Number_To_Words(int.Parse(Total.ToString()));
        //    }
        //    return lstgroup;
        //}
    }

    class AdmitCardDetails
    {
        //public BasicInfo objBasicInfo;
        //public FeeDetails[] objFeeDetails;
        //public FeeTotalDetails objFeeTotalDetails;
        public StudentAdmitCardDetails[] objStudentDetails;
        public ExamSchedule[] objExamSchedule;
    }



    class StudentAdmitCardDetails
    {
        public int StudentId;
        public string StudentName;
        public string RollNo;
        public string AdmissionNo;
        public string ClassName;
        public string Batch;
        public string ParentName;
        public string DOB;
        public string sex;
        public string Address;
        public string ExamName;
        public string SchoolName;
        public string SchoolAddress;

    }

    class ExamSchedule
    {
        public string SubjectName;
        public string Day;
        public string Date;
        public string StartTime;
        public string EndTime;

    }

}
