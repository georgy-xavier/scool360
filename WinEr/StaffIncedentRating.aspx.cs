using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Data;
using WebChart;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinEr
{
    public partial class StaffIncedentRating : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private Incident MyIncident;
        private DataSet MyDataSet;
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
            MyIncident = MyUser.GetIncedentObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyIncident == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(609))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _SubMenuStr;
                    _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                    this.SubStaffMenu.InnerHtml = _SubMenuStr;
                    int StaffId = 0;
                    bool Valid = false;
                    if (Session["StaffId"] != null)
                    {
                        if (int.TryParse(Session["StaffId"].ToString(), out StaffId))
                        {
                            if (StaffId > 0)
                            {
                                Valid = true;
                            }
                        }
                    }
                    if (Valid)
                    {
                        LoaduserTopData();
                        LoadIncidenceRating_Details(StaffId);
                        LoadRatingChart(StaffId);
                        LoadPointsChart(StaffId);
                    }
                   
                }
            }
        }

        private void LoaduserTopData()
        {

            string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
            this.StudentTopStrip.InnerHtml = _Studstrip;
        }


        private void LoadPointsChart(int Staff)
        {
            ChartPointCollection data1 = new ChartPointCollection();
            data1 = new ChartPointCollection();
            int Classhighest = 0, highest = 0;
            int Classlowest = this.LoadClassPoints(data1, Staff, out Classhighest);
            SmoothLineChart c1 = new SmoothLineChart(data1, Color.Green);
            c1.Legend = "Staff Average Points";
            c1.Fill.Color = Color.Green;
            c1.DataLabels.Visible = true;
            c1.DataLabels.ForeColor = Color.Green;
            c1.Fill.CenterColor = Color.Green;
            c1.LineMarker = new TriangleLineMarker(6, System.Drawing.Color.Blue, System.Drawing.Color.Blue);
            ChartPoints.Charts.Add(c1);

            ChartPointCollection data2 = new ChartPointCollection();
            data2 = new ChartPointCollection();
            int lowest = this.LoadStudentPoints(data2, Staff, out highest);
            SmoothLineChart c2 = new SmoothLineChart(data2, Color.Red);
            c2.Legend = "Staff Points";
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

            ChartPoints.YCustomStart = lowest - 10;
            ChartPoints.YCustomEnd = highest + 10;
            ChartPoints.RedrawChart();
        }

        private int LoadStudentPoints(ChartPointCollection data3, int StaffId, out int highest)
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
                    Points = MyIncident.GetMonthlyStaffsPoints(StaffId, MyUser.CurrentBatchId, _Start.Date.Month);
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

        private int LoadClassPoints(ChartPointCollection data2, int StaffId, out int highest)
        {
            int Points = 0, AvgPoints = 0;
            highest = 0;
            int lowest = 0;
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Octo", "Novem", "Dece" };
            int No_Staffs = MyIncident.GetTotal_Staffs();
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

                    Points = MyIncident.GetMonthly_AllStaffIncidentPoints( MyUser.CurrentBatchId, _Start.Date.Month);
                    if (No_Staffs > 0)
                    {
                        AvgPoints = Points / No_Staffs;
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

        private void LoadRatingChart(int StaffId)
        {
            ChartPointCollection d = new ChartPointCollection();
            int highest = 0;
            int lowest = this.LoadRatingData(d, StaffId, out highest);
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

        private int LoadRatingData(ChartPointCollection data2, int StaffId, out int highest)
        {
            int Ratings = 0;
            int lowest = new int();
            highest = 0;
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Octo", "Novem", "Dece" };
            string sql = "SELECT tblbatch.BatchName,tblincidentstaffcalculation.PBR FROM tblincidentstaffcalculation INNER JOIN tblbatch ON tblbatch.Id=tblincidentstaffcalculation.BatchId WHERE tblincidentstaffcalculation.BatchId<" + MyUser.CurrentBatchId + " AND tblincidentstaffcalculation.StaffId=" + StaffId;
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    Ratings = Ratings + int.Parse(MyReader.GetValue(1).ToString());
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
                    Ratings = Ratings + MyIncident.GetStaffMonthlyRating(_Start.Date.Month, StaffId, MyUser.CurrentBatchId);
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
                    data2.Add(new ChartPoint(months[_Start.Date.Date.Month - 1], Ratings));
                    _Start = _Start.AddMonths(1);
                }

            }
            return lowest;

        }
        private void LoadIncidenceRating_Details(int StaffId)
        {
            LoadTotalDetails(StaffId);
            Load_CurrentDetails(StaffId);
        }

        private void Load_CurrentDetails(int StaffId)
        {
            int Current_Batchpoints = 0;
            int Current_BatchRating = MyIncident.GetCurrentBatch_StaffIncidenceRating(StaffId, MyUser.CurrentBatchId, out Current_Batchpoints);
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

        private void LoadTotalDetails(int StaffId)
        {
            int TotalPoints = 0;
            int TotalRating = 0;
            string sql = "SELECT tblincidentstaffcalculation.PBR,tblincidentstaffcalculation.PBP FROM tblincidentstaffcalculation WHERE tblincidentstaffcalculation.BatchId<" + MyUser.CurrentBatchId + " AND tblincidentstaffcalculation.StaffId=" + StaffId;
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    TotalRating = TotalRating + int.Parse(MyReader.GetValue(0).ToString());
                    TotalPoints = TotalPoints + int.Parse(MyReader.GetValue(1).ToString());
                }
            }

            int Current_Batchpoints = 0;

            int Current_BatchRating = MyIncident.GetCurrentBatch_StaffIncidenceRating(StaffId, MyUser.CurrentBatchId, out Current_Batchpoints);
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

       
 
    }
}
