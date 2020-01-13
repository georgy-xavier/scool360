using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
namespace WinEr
{
    public partial class StudentCancelEnrollment : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private Incident MyIncident;
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
            MyStudMang = MyUser.GetStudentObj();
            MyIncident = MyUser.GetIncedentObj();
            if (MyIncident == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(450))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                   // string _MenuStr;
                  //  _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                  //  this.SubStudentMenu.InnerHtml = _MenuStr;
                   // LoadStudentTopData();
                    CheckFeesDue(Session["StudId"].ToString());
                    CheckReturnedAllBooks();
                }
            }
        }
      
        private void CheckReturnedAllBooks()
        {

            if (MyStudMang.HasPendingbooks(int.Parse(Session["StudId"].ToString())))
            {
                Lbl_Err.Text = "This student has not returned all the books";

            }
            else
            {
                Lbl_Err.Text = "";
            }
            
        }

        //private void LoadStudentTopData()
        //{

        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}

        private void CheckFeesDue(string _session)
        {
            double _FeeDue = GettoalFeeDueFortheStudent(int.Parse(_session));

            if (_FeeDue > 0)
            {
                lbl_ErrorMsg.Text = "Student has Fee Due of Rs."+_FeeDue.ToString();
                //Btn_Delete.Enabled = false;
                //Btn_Proceed.Visible = true;
            }
            else
            {
                lbl_ErrorMsg.Text = "";
                //Btn_Delete.Enabled = true;
                //Btn_Proceed.Visible = false;
            }

        }

        private double GettoalFeeDueFortheStudent(int _StudentId)
        {
            double _FeeDue = 0;
            string sql = "Select  sum(tblfeestudent.BalanceAmount) from tblfeestudent inner join tblfeeschedule on tblfeestudent.SchId=tblfeeschedule.Id where tblfeestudent.StudId=" + _StudentId + " AND tblfeeschedule.DueDate <= CURRENT_DATE() AND NOT tblfeestudent.Status='Paid'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                double.TryParse(MyReader.GetValue(0).ToString(), out _FeeDue);
            }
            return _FeeDue;
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            string _AdmissionNo = MyStudMang.GetAdmossionNo(int.Parse(Session["StudId"].ToString()));
            try
            {

                int _StudentId = int.Parse(Session["StudId"].ToString());

                MyIncident.CreateApprovedIncedent("Student Enrollment Cancelled", "Enrollment is Cancelled for the student  " + MyUser.m_DbLog.GetStudentName(_StudentId) + ".", General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", _StudentId, 2, 0, MyUser.CurrentBatchId, MyStudMang.GetClassId(_StudentId));
                MyStudMang.CreateTansationDb();
                UpdateTripAssignedToStudents(_StudentId);
                MyStudMang.UpdateInventoryDetails(_StudentId);
                MyStudMang.StoreIncedentCalcualtion(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId);
                MyStudMang.MoveStudentIncidentToHistory(int.Parse(Session["StudId"].ToString()));
                MyStudMang.RemoveStudentFromCurrentDBandComment(int.Parse(Session["StudId"].ToString()), Txt_Reason.Text, MyUser.UserId, _AdmissionNo);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Cancel Enrollment", "Delete the Student " + MyUser.m_DbLog.GetStudentName(int.Parse(Session["StudId"].ToString()), MyStudMang.m_TransationDb), 1, MyStudMang.m_TransationDb);
                MyStudMang.MoveStudentToHistory(int.Parse(Session["StudId"].ToString()), MyUser.CurrentBatchId, 3);

                MyStudMang.EndSucessTansationDb();
                MyUser.UpdateGroupMapDetails(_StudentId, 0);
                Txt_Reason.Text = "";
                WC_MessageBox.ShowMssage("Student has been transfered to history");
                Response.Redirect("SearchStudent.aspx");


            }
            catch (Exception exc)
            {
                MyStudMang.EndFailTansationDb();
                WC_MessageBox.ShowMssage(exc.Message);
            }
        }

       

        private void UpdateTripAssignedToStudents(int _StudentId)
        {
            string sql = "";
            sql = "select tbl_tr_studtripmap.ToTripId, tbl_tr_studtripmap.FromTripId from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + _StudentId + " and (tbl_tr_studtripmap.FromTripId<>0 or tbl_tr_studtripmap.ToTripId<>0)";
            MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int fromtripId = int.Parse(MyReader.GetValue(1).ToString());
                int totripId = int.Parse(MyReader.GetValue(0).ToString());
                sql = "delete from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId=" + _StudentId + "";
                MyStudMang.m_TransationDb.ExecuteQuery(sql);
                if (fromtripId != 0)
                {
                    sql = "select Occupied from tbl_tr_trips where tbl_tr_trips.Id=" + fromtripId + "";
                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        int newoccupied = int.Parse(MyReader.GetValue(0).ToString()) - 1;
                        sql = "Update tbl_tr_trips set Occupied=" + newoccupied + " where tbl_tr_trips.Id=" + fromtripId + "";
                        MyStudMang.m_TransationDb.ExecuteQuery(sql);
                    }

                }
                if (totripId != 0)
                {
                    sql = "select Occupied from tbl_tr_trips where tbl_tr_trips.Id=" + totripId + "";
                    MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        int newoccupied = int.Parse(MyReader.GetValue(0).ToString()) - 1;
                        sql = "Update tbl_tr_trips set Occupied=" + newoccupied + " where tbl_tr_trips.Id=" + totripId + "";
                        MyStudMang.m_TransationDb.ExecuteQuery(sql);
                    }

                }
            }
        }

        private void StoreIncedentCalcualtion(int StudentId)
        {
            int ClassId = 0;
            int _CurrentBatchid = MyUser.CurrentBatchId;
            int Current_Batch_Class_AVG = 0, Total_ClassPoints = 0, Total_No_Students = 0;
            int Current_Batch_StudentPoints = 0, CurrentBatch_StudentRating = 0;
            ClassId =MyStudMang.GetClassId(StudentId,MyUser.CurrentBatchId);
            Total_ClassPoints = GetTotal_ClassIncidentPoints(ClassId, _CurrentBatchid);
            Total_No_Students = GetTotal_StudentsInClass(ClassId, _CurrentBatchid);
            if (Total_No_Students > 0)
            {
                Current_Batch_Class_AVG = Total_ClassPoints / Total_No_Students;
                CurrentBatch_StudentRating = 0;
                Current_Batch_StudentPoints = GetTotal_StudentIncidentPoints(StudentId, ClassId, _CurrentBatchid);
                CurrentBatch_StudentRating = Current_Batch_StudentPoints - Current_Batch_Class_AVG;
                Insert_IncidentCalculations(StudentId, _CurrentBatchid, Current_Batch_StudentPoints, CurrentBatch_StudentRating, ClassId);

            }
        }

        private void Insert_IncidentCalculations(int StudentId, int _CurrentBatchid, int Current_Batch_StudentPoints, int CurrentBatch_StudentRating, int ClassId)
        {
            string sql = "insert into tblincidentcalculation(StudentId,PBP,PBR,BatchId,OldClassId) values(" + StudentId + "," + Current_Batch_StudentPoints + "," + CurrentBatch_StudentRating + "," + _CurrentBatchid + "," + ClassId + ")";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }

        private int GetTotal_StudentIncidentPoints(int StudentId, int ClassId, int _CurrentBatchid)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student'  AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid + " AND tblincedent.AssoUser=" + StudentId;
            MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        private int GetTotal_StudentsInClass(int ClassId, int _CurrentBatchid)
        {
            int count = 0;
            string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + _CurrentBatchid;
            MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }

        private int GetTotal_ClassIncidentPoints(int ClassId, int _CurrentBatchid)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(tblincedent.`Point`) FROM tblincedent WHERE tblincedent.UserType='student' AND tblincedent.Status='Approved' AND tblincedent.ClassId=" + ClassId + " AND tblincedent.BatchId=" + _CurrentBatchid;
            MyReader =MyStudMang.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Txt_Reason.Text = "";
        }
        protected void Btn1_Delete_Click(object sender, EventArgs e)
        {
                ModalPopupExtender1.Show();
            
        }
    }
}
