using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
using WebChart;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WinEr
{
    public partial class StudentIncidentReport : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private Incident Myincident;
        private StudentManagerClass MyStudent;

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
            Myincident = MyUser.GetIncedentObj();
            MyStudent = MyUser.GetStudentObj();
            if (MyStudent == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (Myincident == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(602))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //string _MenuStr;
                    //_MenuStr = MyStudent.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                    //this.SubStudentMenu.InnerHtml = _MenuStr;
                    int StudentId=0;
                    bool Valid=false;
                    if (Session["StudId"] != null)
                    {
                        if(int.TryParse(Session["StudId"].ToString(),out StudentId))
                        {
                            if (StudentId > 0)
                            {
                                Valid = true;
                            }
                        }
                    }
                    if (Valid)
                    {
                        //LoadStudentTopData();
                        LoadIncidenceRating_Details(StudentId);
                        LoadRatingChart(StudentId);
                        LoadPointsChart(StudentId);
                    }
                    else
                    {
                        Lbl_msgAlert.Text = "Student Not Found";
                        MPE_MessageBox.Show();
                    }
                    ChartControl.PerformCleanUp();
                }
            }
        }
        private void LoadPointsChart(int StudentId)
        {
            ChartPointCollection data1 = new ChartPointCollection();
            data1 = new ChartPointCollection();
            int Classhighest = 0,highest=0;
            int Classlowest = this.LoadClassPoints(data1, StudentId, out Classhighest);
            SmoothLineChart c1 = new SmoothLineChart(data1, Color.Green);
            c1.Legend = "Class Average Points";
            c1.Fill.Color = Color.Green;
            c1.DataLabels.Visible = true;
            c1.DataLabels.ForeColor = Color.Green;
            c1.Fill.CenterColor = Color.Green;
            c1.LineMarker = new TriangleLineMarker(6, System.Drawing.Color.Blue, System.Drawing.Color.Blue);
            ChartPoints.Charts.Add(c1);

            ChartPointCollection data2 = new ChartPointCollection();
            data2 = new ChartPointCollection();
            int lowest = this.LoadStudentPoints(data2, StudentId, out highest);
            SmoothLineChart c2 = new SmoothLineChart(data2, Color.Red);
            c2.Legend = "Student Points";
            c2.Fill.Color = Color.Red;
            c2.DataLabels.Visible = true;
            c2.DataLabels.ForeColor = Color.CornflowerBlue;
            c2.Fill.CenterColor = Color.Red;
            c2.LineMarker = new TriangleLineMarker(6, System.Drawing.Color.Blue, System.Drawing.Color.Blue);
            ChartPoints.Charts.Add(c2);

            if (Classlowest < lowest)
            {
                lowest = Classlowest;
            }
            if (Classhighest > highest)
            {
                highest = Classhighest;
            }

            ChartPoints.YCustomStart = lowest-10;
            ChartPoints.YCustomEnd = highest + 10;
            ChartPoints.RedrawChart();
        }

        private int LoadStudentPoints(ChartPointCollection data3, int StudentId, out int highest)
        {
            int Points = 0;
            highest = 0;
            int lowest = 0;
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Octo", "Novem", "Dece" };
            DateTime _Start, _End;
            MyUser.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
            if (DateTime.Now > _Start)
            {

                if (_End > DateTime.Now)
                {
                    _End = DateTime.Now;
                }
                _Start = new DateTime(_Start.Year, _Start.Month, 1);
                _End = new DateTime(_End.Year, _End.Month, 1);
                while (_Start.Date <= _End.Date)
                {
                    Points = 0;
                    string IncedentTable = "tblincedent";
                    if (Session["StudType"].ToString() == "2")
                    {
                        IncedentTable = "tblincedent_history";
                    }
                    Points = Myincident.GetMonthlyStudent_Points(StudentId, MyUser.CurrentBatchId, _Start.Date.Month, IncedentTable);
                    data3.Add(new ChartPoint(months[_Start.Date.Date.Month - 1], Points));
                    if (Points < lowest)
                    {
                        lowest = Points;
                    }
                    if (Points > highest)
                    {
                        highest = Points;
                    }
                    _Start = _Start.AddMonths(1);
                }

            }

            return lowest;
        }

        private int LoadClassPoints(ChartPointCollection data2, int StudentId, out int highest)
        {
            int Points = 0,AvgPoints=0;
            highest = 0;
            int lowest = 0;
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Octo", "Novem", "Dece" };
            int classId = MyStudent.GetClassId(StudentId, MyUser.CurrentBatchId);
            int No_Students = Myincident.GetTotal_StudentsInClass(classId, MyUser.CurrentBatchId);
            DateTime _Start, _End;
            MyUser.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
            if (DateTime.Now > _Start)
            {

                if (_End > DateTime.Now)
                {
                    _End = DateTime.Now;
                }
                _Start = new DateTime(_Start.Year, _Start.Month, 1);
                _End = new DateTime(_End.Year, _End.Month, 1);
                while (_Start.Date <= _End.Date)
                {
                    AvgPoints = 0; Points = 0;
                    string IncedentTable = "tblincedent";
                    if (Session["StudType"].ToString() == "2")
                    {
                        classId = MyStudent.GetClassIdHistory(StudentId);
                        No_Students = Myincident.GetTotal_StudentsInClass(classId, MyUser.CurrentBatchId);
                        IncedentTable = "tblincedent_history";
                    }
                    Points = Myincident.GetMonthly_ClassIncidentPoints(classId, MyUser.CurrentBatchId, _Start.Date.Month, IncedentTable);
                    if (No_Students > 0)
                    {
                        AvgPoints = Points / No_Students;
                    }
                    data2.Add(new ChartPoint(months[_Start.Date.Date.Month - 1], AvgPoints));
                    if (AvgPoints < lowest)
                    {
                        lowest = AvgPoints;
                    }
                    if (AvgPoints > highest)
                    {
                        highest = AvgPoints;
                    }
                    _Start = _Start.AddMonths(1);
                }

            }

            return lowest;
        }

        private void LoadRatingChart(int StudentId)
        {
            ChartPointCollection d = new ChartPointCollection();
            int highest = 0;
            int lowest = this.LoadRatingData(d, StudentId, out highest);
            Chart c = new LineChart(d, Color.Red);
            AdjustableArrowCap myArrow = new AdjustableArrowCap(3, 3, true);
            //CustomLineCap customArrow = myArrow;
            c.Line.EndCap = LineCap.Custom;
            c.Line.CustomEndCap = myArrow;

            c.ShowLineMarkers = false;

            c.DataLabels.Visible = true;
            c.DataLabels.ForeColor = Color.CornflowerBlue;

            ChartRating.YCustomStart = lowest;
            ChartRating.YCustomEnd = highest + 5;
            ChartRating.Charts.Add(c);
            ChartRating.RedrawChart();
        }

        private int LoadRatingData(ChartPointCollection data2, int StudentId, out int highest)
        {
            int Ratings = 0;
            int lowest=new int();
            highest = 0;
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
            int classId = MyStudent.GetClassId(StudentId,MyUser.CurrentBatchId);
            string sql = "SELECT tblbatch.BatchName,tblincidentcalculation.PBR FROM tblincidentcalculation INNER JOIN tblbatch ON tblbatch.Id=tblincidentcalculation.BatchId WHERE tblincidentcalculation.BatchId<"+MyUser.CurrentBatchId+" AND tblincidentcalculation.StudentId=" + StudentId;
            MyReader = Myincident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    Ratings=Ratings + int.Parse(MyReader.GetValue(1).ToString());
                    if (Ratings < lowest)
                    {
                        lowest = Ratings;
                    }
                    if (Ratings > highest)
                    {
                        highest = Ratings;
                    }
                    data2.Add(new ChartPoint("Batch-" + MyReader.GetValue(0).ToString(), Ratings));
                }
            }
            DateTime _Start, _End;
            MyUser.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
            if (DateTime.Now > _Start)
            {
              
                if (_End > DateTime.Now)
                {
                    _End = DateTime.Now;
                }
                _Start = new DateTime(_Start.Year, _Start.Month, 1);
                _End = new DateTime(_End.Year, _End.Month, 1);
                while (_Start.Date <= _End.Date)
                {
                    string IncedentTable = "tblincedent";
                    if (Session["StudType"].ToString() == "2")
                    {
                        classId = MyStudent.GetClassIdHistory(StudentId);
                        IncedentTable = "tblincedent_history";
                    }
                    Ratings = Ratings + Myincident.GetMonthlyRating("Student", _Start.Date.Month, StudentId, classId, MyUser.CurrentBatchId, IncedentTable);
                    if (Ratings < lowest)
                    {
                        lowest = Ratings;
                    }
                    if (Ratings > highest)
                    {
                        highest = Ratings;
                    }
                    if (_Start.AddMonths(1).Date > _End.Date)
                    {
                        Ratings = int.Parse(Lbl_TotalRating.Text);
                    }
                    data2.Add(new ChartPoint(months[_Start.Date.Date.Month-1], Ratings));
                    _Start=_Start.AddMonths(1);
                }
                
            }
            return lowest;

        }


        private void LoadIncidenceRating_Details(int StudentId)
        {
            LoadTotalDetails(StudentId);
            Load_CurrentDetails(StudentId);

        }

        private void LoadTotalDetails(int StudentId)
        {
            int TotalPoints = 0;
            int TotalRating=0;
            string sql = "SELECT tblincidentcalculation.PBR,tblincidentcalculation.PBP FROM tblincidentcalculation WHERE tblincidentcalculation.BatchId<" + MyUser.CurrentBatchId + " AND tblincidentcalculation.StudentId=" + StudentId;
            MyReader = Myincident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    TotalRating = TotalRating + int.Parse(MyReader.GetValue(0).ToString());
                    TotalPoints = TotalPoints + int.Parse(MyReader.GetValue(1).ToString());
                }
            }
                    
            int Current_Batchpoints = 0;
            int ClassId = MyStudent.GetClassId(StudentId, MyUser.CurrentBatchId);
           
            string IncedentTable = "tblincedent";
            if (Session["StudType"].ToString() == "2")
            {
                ClassId = MyStudent.GetClassIdHistory(StudentId);
                IncedentTable = "tblincedent_history";
            }
            int Current_BatchRating = Myincident.GetCurrentBatch_IncidenceRating(StudentId, ClassId, MyUser.CurrentBatchId, IncedentTable, out Current_Batchpoints);
            TotalRating = TotalRating + Current_BatchRating;
            TotalPoints = TotalPoints + Current_Batchpoints;
            if (TotalPoints < 0)
            {
                Img_TotalPoints.Src = "Pics/Points red.png";
                Lbl_TotalPoints.ForeColor = System.Drawing.Color.Red;
            }
            if (TotalRating < 0)
            {
                Img_TotalRating.Src = "Pics/Rating red.png";
                Lbl_TotalRating.ForeColor = System.Drawing.Color.Red;
            }
            Lbl_TotalPoints.Text = TotalPoints.ToString();
            Lbl_TotalRating.Text = TotalRating.ToString();

        }

        private void Load_CurrentDetails(int StudentId)
        {
            int Current_Batchpoints = 0;
            int ClassId = MyStudent.GetClassId(StudentId, MyUser.CurrentBatchId);
            string IncedentTable = "tblincedent";
            if (Session["StudType"].ToString() == "2")
            {
                ClassId = MyStudent.GetClassIdHistory(StudentId);
                IncedentTable = "tblincedent_history";
            }
            int Current_BatchRating = Myincident.GetCurrentBatch_IncidenceRating(StudentId, ClassId, MyUser.CurrentBatchId, IncedentTable, out Current_Batchpoints);
            if (Current_Batchpoints < 0)
            {
                Img_CurrentPoints.Src = "Pics/Points red.png";
                Lbl_BatchPoints.ForeColor = System.Drawing.Color.Red;
            }
            if (Current_BatchRating < 0)
            {
                Img_CurrentRating.Src = "Pics/Rating red.png";
                Lbl_BatchRating.ForeColor = System.Drawing.Color.Red;
            }

            Lbl_BatchPoints.Text = Current_Batchpoints.ToString();
            Lbl_BatchRating.Text = Current_BatchRating.ToString();
        }
    }
}
