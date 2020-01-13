using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebChart;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class MangmtDashboard : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudMang;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;


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
            MyClassMang = MyUser.GetClassObj();
            MyAttendance = MyUser.GetAttendancetObj();
            MyStudMang = MyUser.GetStudentObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(95))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    Load_Chart();

                }
            }
        }

        private void Load_Chart()
        {
            string[] months = new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };
            float[] Payable = new float[15];
            float[] Paid = new float[15];
            int[] Month = new int[15];
            int _count = 0;
            DateTime _Start, _End;
            MyUser.GetbatchDates(MyUser.CurrentBatchId, out _Start, out _End);
            if (DateTime.Now > _Start)
            {

                _Start = new DateTime(_Start.Year, _Start.Month, 1);
                _End = new DateTime(_End.Year, _End.Month, 1);

                while (_Start.Date <= _End.Date)
                {
                    Payable[_count] = _Start.Date.Date.Month - 1;
                    Paid[_count] = _Start.Date.Date.Month - 1+1;
                    Month[_count] = _Start.Date.Date.Month - 1;
                    //data2.Add(new ChartPoint(months[_Start.Date.Date.Month - 1], Ratings));
                    _Start = _Start.AddMonths(1);
                    _count++;
                }

            }

            if (_count > 0)
            {
                Chart c;
                ChartPointCollection d;
                c = new StackedColumnChart();
                c.Fill.Color = Color.LightGreen;
                c.Fill.ForeColor = Color.White;
                c.Fill.Type = InteriorType.Hatch;
                c.Fill.HatchStyle = HatchStyle.ForwardDiagonal;
                d = c.Data;
                for (int i = 0; i < _count; i++)
                {
                    d.Add(new ChartPoint(months[Month[i]], Paid[i]));
                }
                c.Legend = "Paid Amount";
                chart1.Charts.Add(c);

                c = new StackedColumnChart();
                c.Fill.Type = InteriorType.Hatch;
                c.Fill.HatchStyle = HatchStyle.BackwardDiagonal;
                c.Fill.ForeColor = Color.White;
                c.Fill.Color = Color.Gold;
            
                d = c.Data;
                for (int i = 0; i < _count; i++)
                {
                    d.Add(new ChartPoint(months[Month[i]], Payable[i]));
                }
                c.Legend = "Payable Amount";
                chart1.Charts.Add(c);
                chart1.RedrawChart();
            }
           
        }

    }
}
