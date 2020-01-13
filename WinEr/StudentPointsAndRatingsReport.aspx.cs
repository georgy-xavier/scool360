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
using WinBase;
namespace WinEr
{
    public partial class StudentPointsAndRatingsReport : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        private KnowinUser MyUser;
        private StudentManagerClass MyStudentMang;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private Incident Myincedent;

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
            MyStudentMang = MyUser.GetStudentObj();
            Myincedent = MyUser.GetIncedentObj();
            if (MyStudentMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (Myincedent == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(652))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["PointRateList"] = null;
                    Session["SortDirection"] = null;
                    Session["SortExpression"] = null;
                    LoadBatchToDropDown();
                    LoadStandardToDropDown();
                    LoadClassToDropDown();
                    LoadMonthToDropDown();
                    LoadPointRateToDropDown();
                    //lblmonth.Visible = false;
                    //Drp_month.Visible = false;

                }
            }

        }


        #region DROP DOWN FUNTIONS

        private void LoadBatchToDropDown()
        {
            Drp_Batch.Items.Clear();

            string sql = "SELECT tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id >= 37 and tblbatch.created =1 order by tblbatch.Id desc";
            MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL", "0");
                Drp_Batch.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Batch.Items.Add(li);
                }
                Drp_Batch.SelectedValue = MyUser.CurrentBatchId.ToString();
                Btn_Generate.Enabled = true;
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No Batch Found", "-1");
                Drp_Batch.Items.Add(li);
            }

        }

        private void LoadStandardToDropDown()
        {
            Drp_Standard.Items.Clear();

            string sql = "select tblstandard.Name, tblstandard.Id from tblstandard order by tblstandard.Id asc";
            MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL", "0");
                Drp_Standard.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Standard.Items.Add(li);
                }
                Btn_Generate.Enabled = true;
            }
            else
            {
                Btn_Generate.Enabled = false;
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_Standard.Items.Add(li);
            }
        }

        private void LoadClassToDropDown()
        {
            Drp_Class.Items.Clear();
            int _standard = int.Parse(Drp_Standard.SelectedIndex.ToString());
            if (_standard == 0)
            {
                Drp_Class.Enabled = false;
            }
            else
            {
                Drp_Class.Enabled = true;
                string sql = "select tblclass.ClassName, tblclass.Id from tblclass where tblclass.Status=1 and tblclass.Standard = " + _standard + " order by tblclass.Id asc";
                MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    ListItem li = new ListItem("ALL", "0");
                    Drp_Class.Items.Add(li);
                    while (MyReader.Read())
                    {
                        li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                        Drp_Class.Items.Add(li);
                    }
                    Btn_Generate.Enabled = true;
                }
                else
                {
                    Btn_Generate.Enabled = false;
                    ListItem li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                }
            }
        }

        private void LoadMonthToDropDown()
        {
            Drp_month.Items.Clear();
            ListItem li = new ListItem("ALL", "0");
            Drp_month.Items.Add(li);
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
                    string lli = "";
                    int month = _Start.Month;
                    switch (month)
                    {
                        case 1:
                            lli = "January";
                            break;
                        case 2:
                            lli = "February";
                            break;
                        case 3:
                            lli = "March";
                            break;
                        case 4:
                            lli = "April";
                            break;
                        case 5:
                            lli = "May";
                            break;
                        case 6:
                            lli = "June";
                            break;
                        case 7:
                            lli = "July";
                            break;
                        case 8:
                            lli = "August";
                            break;
                        case 9:
                            lli = "September";
                            break;
                        case 10:
                            lli = "October";
                            break;
                        case 11:
                            lli = "November";
                            break;
                        case 12:
                            lli = "December";
                            break;
                    }
                    //ListItem li = new ListItem(lli);
                    Drp_month.Items.Add(new ListItem(lli, month.ToString()));
                    _Start = _Start.AddMonths(1);


                }

            }

        }
        private void LoadPointRateToDropDown()
        {
            Drp_PointAndRating.Items.Clear();
            ListItem li = new ListItem("Points", "0");
            Drp_PointAndRating.Items.Add(li);
            li = new ListItem("Ratings", "1");
            Drp_PointAndRating.Items.Add(li);
        }

#endregion

        #region DROP DOWN SELECT CHANGE INDEX FUNCTIONS
        protected void Drp_Standard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _Standard = int.Parse(Drp_Standard.SelectedValue.ToString());
            if (_Standard == 0)
            {
                Btn_Generate.Enabled = true;
            }
            LoadClassToDropDown();
        }

        protected void Drp_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {

            int _batch = int.Parse(Drp_Batch.SelectedValue.ToString());
            if (_batch == MyUser.CurrentBatchId)
            {
                lblmonth.Visible = true;
                Drp_month.Visible = true;
                LoadMonthToDropDown();
            }
            else 
            {
                lblmonth.Visible = false;
                Drp_month.Visible = false;
            }
        }
        #endregion

        #region SHOW REPORT BUTTON FUNCTION

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {

            FillGrid();
        }



        private void FillGrid()
        {
            Grd_PointAndRating.Columns[5].Visible = true;
            Grd_PointAndRating.Columns[4].Visible = true;
            Grd_PointAndRating.Columns[2].Visible = true;
            Grd_PointAndRating.DataSource = null;
            Grd_PointAndRating.DataBind();
            DataSet MyDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            MyDataSet.Tables.Add(new DataTable("tblPointRate"));
            dt = MyDataSet.Tables["tblPointRate"];
            dt.Columns.Add("Rank",typeof(int));
            dt.Columns.Add("StudentName", typeof(string));
            dt.Columns.Add("BatchName", typeof(string));
            dt.Columns.Add("ClassName", typeof(string));
            dt.Columns.Add("Points", typeof(int));
            dt.Columns.Add("Ratings", typeof(int));
            MyReader = null;
            string sql;

            if (Drp_PointAndRating.SelectedIndex == 0)
            { 

             if (Drp_Batch.SelectedIndex == 0 && Drp_Standard.SelectedIndex == 0)
              {
                  sql = " select distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points   from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_student.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' GROUP BY AssoUser order by sum(tblview_incident.Point) desc";
                MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
              }
            else if (Drp_Batch.SelectedIndex == 1 && Drp_Standard.SelectedIndex == 0)
            {
                int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                
                if (Drp_month.SelectedIndex == 0)
                {
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= "+ _BatchVal +" GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    int _MonthVal = int.Parse(Drp_month.SelectedValue.ToString());
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and Month(tblincedent.IncedentDate) = "+ _MonthVal +" GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);

                }
            }
            else if (Drp_Batch.SelectedIndex == 0 && Drp_Standard.SelectedIndex > 0)
            {
                int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());

                if (Drp_Class.SelectedIndex == 0)
                {
                    sql = " select  distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points   from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_student.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' and tblview_incident.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " ) GROUP BY AssoUser order by sum(tblview_incident.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                    sql = " select  distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points   from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_student.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' and tblview_incident.ClassId  = " + _ClassVal + " and tblview_incident.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + ")GROUP BY AssoUser order by sum(tblview_incident.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
            }
            else if (Drp_Batch.SelectedIndex > 1 && Drp_Standard.SelectedIndex == 0)
            {
                int _BatchNewVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                sql = "select distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points   from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= "+_BatchNewVal +" GROUP BY AssoUser order by sum(tblincedent_history.Point) desc";
               MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            }
            else if (Drp_Batch.SelectedIndex == 1 && Drp_month.SelectedIndex == 0 && Drp_Standard.SelectedIndex > 0)
            {
                int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                if (Drp_Class.SelectedIndex == 0)
                {
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " ) GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId  = " + _ClassVal + "  GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
            }
            else if (Drp_Batch.SelectedIndex == 1 && Drp_month.SelectedIndex > 0 && Drp_Standard.SelectedIndex > 0)
            {
                int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                int _MonthVal = int.Parse(Drp_month.SelectedValue.ToString());
                if (Drp_Class.SelectedIndex == 0)
                {
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " )and Month(tblincedent.IncedentDate) = " + _MonthVal + " GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                    sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points   from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId  = " + _ClassVal + " and Month(tblincedent.IncedentDate) = " + _MonthVal + " GROUP BY AssoUser order by sum(tblincedent.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
            }
            else if (Drp_Batch.SelectedIndex > 1 && Drp_Standard.SelectedIndex > 0)
            {
                int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                if (Drp_Class.SelectedIndex == 0)
                {
                    sql = " select  distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points   from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= " + _BatchVal + " and tblincedent_history.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " )GROUP BY AssoUser order by sum(tblincedent_history.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                    sql = " select  distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points   from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= " + _BatchVal + " and tblincedent_history.ClassId = " + _ClassVal + " GROUP BY AssoUser order by sum(tblincedent_history.Point) desc";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                
                }
            }
            else
            {
                WC_MessageBox.ShowMssage("No Record Found!");
            }
             if (MyReader.HasRows)
             {
                 Pnl_Student_Point_Rank.Visible = true;
                 
                 int i = 0;
                 while (MyReader.Read())
                 {

                     dr = MyDataSet.Tables["tblPointRate"].NewRow();
                     dr["Rank"] = (i + 1).ToString();
                     dr["StudentName"] = MyReader.GetValue(1);
                     dr["BatchName"] = MyReader.GetValue(2);
                     dr["ClassName"] = MyReader.GetValue(3);
                     dr["Points"] = MyReader.GetValue(4);
                     dr["Ratings"] = 0;
                     MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                     i++;
                 }

             }
             else
             {
                 WC_MessageBox.ShowMssage("No Record Found!");
                 Pnl_Student_Point_Rank.Visible = false;
             }
             Grd_PointAndRating.DataSource = MyDataSet;
             Session["PointRateList"] = MyDataSet;
             Grd_PointAndRating.DataBind();
             Grd_PointAndRating.Columns[4].Visible = true;
             Grd_PointAndRating.Columns[5].Visible = false;
             Grd_PointAndRating.Columns[2].Visible = false;

        }

            else if (Drp_PointAndRating.SelectedIndex > 0)
            {

                if (Drp_Batch.SelectedIndex == 0 && Drp_Standard.SelectedIndex == 0)
                {

                    sql = "select distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points, tblview_incident.BatchId, tblview_incident.ClassId from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_student.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' GROUP BY AssoUser "; 
                 MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                 if (MyReader.HasRows)
                 {
                     int i = 0;
                     Pnl_Student_Point_Rank.Visible = true;
                     while (MyReader.Read())
                     {
                        
                         int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                         int _P = int.Parse(MyReader.GetValue(4).ToString());
                         int _Bid = MyUser.CurrentBatchId;
                         //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                         int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                         double _CurrentRate = 0;
                         string _table = "tblincedent";
                         int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                         int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                         if (_TotalStudents == 0 )
                         {
                              _CurrentRate = _P - _TotalPoints ;
                         }
                         else if (_TotalPoints == 0)
                         {
                              _CurrentRate = _P;
                         }
                         else 
                         {
                              _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                         }
                         

                         sql = " select tblincidentcalculation.StudentId, tblincidentcalculation.PBR, tblincidentcalculation.OldClassId from tblincidentcalculation where tblincidentcalculation.StudentId = "+_Sid + "";
                         MyReader1 = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                         double _FinalRate= 0;
                         if (MyReader1.HasRows)
                         {
                            
                             while (MyReader1.Read())
                             {
                                 int _PreRate = int.Parse(MyReader1.GetValue(1).ToString());
                                  _FinalRate = _PreRate + _CurrentRate;
                             }
                         }
                         dr = MyDataSet.Tables["tblPointRate"].NewRow();
                         dr["Rank"] = (i + 1).ToString();
                         dr["StudentName"] = MyReader.GetValue(1);
                         dr["BatchName"] = MyReader.GetValue(2);
                         dr["ClassName"] = MyReader.GetValue(3);
                         dr["Points"] = 0;
                         dr["Ratings"] = _FinalRate ;
                         MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                         i++;

                     }
                     
                 }
                 else
                 {
                     WC_MessageBox.ShowMssage("No Record Found!");
                     Pnl_Student_Point_Rank.Visible = false;
                 }
                 Grd_PointAndRating.DataSource = MyDataSet;
                 Session["PointRateList"] = MyDataSet;
                 Grd_PointAndRating.DataBind();
                 Grd_PointAndRating.Columns[4].Visible = false;
                 Grd_PointAndRating.Columns[5].Visible = true;
                 Grd_PointAndRating.Columns[2].Visible = false;
                 sortRating();
                }

                else if (Drp_Batch.SelectedIndex == 1 && Drp_Standard.SelectedIndex == 0)
                {
                    int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());

                    if (Drp_month.SelectedIndex == 0)
                    {
                        sql = "select distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' GROUP BY AssoUser "; 
                 MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                 if (MyReader.HasRows)
                 {
                     int i = 0;
                     Pnl_Student_Point_Rank.Visible = true;
                     while (MyReader.Read())
                     {
                        
                         int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                         int _P = int.Parse(MyReader.GetValue(4).ToString());
                         int _Bid = MyUser.CurrentBatchId;
                         //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                         int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                         double _CurrentRate = 0;
                         string _table = "tblincedent";
                         int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                         int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                         if (_TotalStudents == 0 )
                         {
                              _CurrentRate = _P - _TotalPoints ;
                         }
                         else if (_TotalPoints == 0)
                         {
                              _CurrentRate = _P;
                         }
                         else 
                         {
                              _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                         }
                         dr = MyDataSet.Tables["tblPointRate"].NewRow();
                         dr["Rank"] = (i + 1).ToString();
                         dr["StudentName"] = MyReader.GetValue(1);
                         dr["BatchName"] = MyReader.GetValue(2);
                         dr["ClassName"] = MyReader.GetValue(3);
                         dr["Points"] = 0;
                         dr["Ratings"] = _CurrentRate;
                         MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                         i++;

                     }

                 }
                 else
                 {
                     WC_MessageBox.ShowMssage("No Record Found!");
                     Pnl_Student_Point_Rank.Visible = false;
                 }
                 Grd_PointAndRating.DataSource = MyDataSet;
                 Session["PointRateList"] = MyDataSet;
                 Grd_PointAndRating.DataBind();
                 Grd_PointAndRating.Columns[4].Visible = false;
                 Grd_PointAndRating.Columns[5].Visible = true;
                 Grd_PointAndRating.Columns[2].Visible = false;
                 sortRating();
                    }

                    else
                    {
                        int _MonthVal = int.Parse(Drp_month.SelectedValue.ToString());
                        sql = "select distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' GROUP BY AssoUser "; 
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {
                                string _Type= "student";
                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                               
                                double _MonthRate = 0;
                                string _table = "tblincedent";
                                _MonthRate = GetMonthlyRating(_Type, _MonthVal, _Sid, _Cid, _Bid, _table);
                                
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _MonthRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                }
                else if (Drp_Batch.SelectedIndex == 0 && Drp_Standard.SelectedIndex > 0)
                {
                    int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());

                    if (Drp_Class.SelectedIndex == 0)
                    {
                        sql = "select distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points, tblview_incident.BatchId, tblview_incident.ClassId from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_incident.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' and tblview_incident.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " ) GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }


                                sql = " select tblincidentcalculation.StudentId, tblincidentcalculation.PBR, tblincidentcalculation.OldClassId from tblincidentcalculation where tblincidentcalculation.StudentId = " + _Sid + "";
                                MyReader1 = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                                double _FinalRate = 0;
                                if (MyReader1.HasRows)
                                {

                                    while (MyReader1.Read())
                                    {
                                        int _PreRate = int.Parse(MyReader1.GetValue(1).ToString());
                                        _FinalRate = _PreRate + _CurrentRate;
                                    }
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _FinalRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    
                    }
                    else
                    {
                        int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                        sql = "select distinct tblview_incident.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblview_incident.Point) as Points, tblview_incident.BatchId, tblview_incident.ClassId from tblview_incident inner join tblview_student on tblview_incident.AssoUser = tblview_student.Id inner join tblbatch on tblview_incident.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblview_incident.ClassId  where tblview_incident.UserType= 'student' and tblview_incident.Status = 'Approved' and tblview_incident.ClassId =" + _ClassVal + " GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }


                                sql = " select tblincidentcalculation.StudentId, tblincidentcalculation.PBR, tblincidentcalculation.OldClassId from tblincidentcalculation where tblincidentcalculation.StudentId = " + _Sid + "";
                                MyReader1 = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                                double _FinalRate = 0;
                                if (MyReader1.HasRows)
                                {

                                    while (MyReader1.Read())
                                    {
                                        int _PreRate = int.Parse(MyReader1.GetValue(1).ToString());
                                        _FinalRate = _PreRate + _CurrentRate;
                                    }
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _FinalRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                }
                else if (Drp_Batch.SelectedIndex > 1 && Drp_Standard.SelectedIndex == 0)
                {
                    int _BatchNewVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                    sql = "select distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points, tblincedent_history.BatchId, tblincedent_history.ClassId  from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= " + _BatchNewVal + " GROUP BY AssoUser ";
                    MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        int i = 0;
                        Pnl_Student_Point_Rank.Visible = true;
                        while (MyReader.Read())
                        {

                            int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                            int _P = int.Parse(MyReader.GetValue(4).ToString());
                            int _Bid = MyUser.CurrentBatchId;
                            //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                            int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                            double _CurrentRate = 0;
                            string _table = "tblincedent";
                            int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                            int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                            if (_TotalStudents == 0)
                            {
                                _CurrentRate = _P - _TotalPoints;
                            }
                            else if (_TotalPoints == 0)
                            {
                                _CurrentRate = _P;
                            }
                            else
                            {
                                _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                            }
                            dr = MyDataSet.Tables["tblPointRate"].NewRow();
                            dr["Rank"] = (i + 1).ToString();
                            dr["StudentName"] = MyReader.GetValue(1);
                            dr["BatchName"] = MyReader.GetValue(2);
                            dr["ClassName"] = MyReader.GetValue(3);
                            dr["Points"] = 0;
                            dr["Ratings"] = _CurrentRate;
                            MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                            i++;

                        }

                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("No Record Found!");
                        Pnl_Student_Point_Rank.Visible = false;
                    }
                    Grd_PointAndRating.DataSource = MyDataSet;
                    Session["PointRateList"] = MyDataSet;
                    Grd_PointAndRating.DataBind();
                    Grd_PointAndRating.Columns[4].Visible = false;
                    Grd_PointAndRating.Columns[5].Visible = true;
                    Grd_PointAndRating.Columns[2].Visible = false;
                    sortRating();

                }
                else if (Drp_Batch.SelectedIndex == 1 && Drp_month.SelectedIndex == 0 && Drp_Standard.SelectedIndex > 0)
                {
                    int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                    int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                    if (Drp_Class.SelectedIndex == 0)
                    {
                        sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " ) GROUP BY AssoUser";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _CurrentRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                    else
                    {
                        int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                        sql = " select distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId  = " + _ClassVal + "  GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _CurrentRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                }
                else if (Drp_Batch.SelectedIndex == 1 && Drp_month.SelectedIndex > 0 && Drp_Standard.SelectedIndex > 0)
                {
                    int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                    int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                    int _MonthVal = int.Parse(Drp_month.SelectedValue.ToString());
                    if (Drp_Class.SelectedIndex == 0)
                    {
                        sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " ) GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {
                                string _Type = "student";
                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());

                                double _MonthRate = 0;
                                string _table = "tblincedent";
                                _MonthRate = GetMonthlyRating(_Type, _MonthVal, _Sid, _Cid, _Bid, _table);

                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _MonthRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                    else
                    {
                        int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                        sql = " select  distinct tblincedent.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent.Point) as Points, tblincedent.BatchId, tblincedent.ClassId from tblincedent inner join tblview_student on tblincedent.AssoUser = tblview_student.Id inner join tblbatch on tblincedent.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent.ClassId  where tblincedent.UserType= 'student' and tblincedent.Status = 'Approved' and tblincedent.BatchId= " + _BatchVal + " and tblincedent.ClassId  = " + _ClassVal + "  GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {
                                string _Type = "student";
                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());

                                double _MonthRate = 0;
                                string _table = "tblincedent";
                                _MonthRate = GetMonthlyRating(_Type, _MonthVal, _Sid, _Cid, _Bid, _table);

                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _MonthRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                }
                else if (Drp_Batch.SelectedIndex > 1 && Drp_Standard.SelectedIndex > 0)
                {
                    int _StandardVal = int.Parse(Drp_Standard.SelectedValue.ToString());
                    int _BatchVal = int.Parse(Drp_Batch.SelectedValue.ToString());
                    if (Drp_Class.SelectedIndex == 0)
                    {
                        sql = " select  distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points, tblincedent_history.BatchId, tblincedent_history.ClassId from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= " + _BatchVal + " and tblincedent_history.ClassId in (select distinct tblclass.Id from tblclass where tblclass.Standard = " + _StandardVal + " )GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _CurrentRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                    else
                    {
                        int _ClassVal = int.Parse(Drp_Class.SelectedValue.ToString());
                        sql = " select  distinct tblincedent_history.AssoUser,tblview_student.StudentName, tblbatch.BatchName, tblclass.ClassName, sum(tblincedent_history.Point) as Points, tblincedent_history.BatchId, tblincedent_history.ClassId from tblincedent_history inner join tblview_student on tblincedent_history.AssoUser = tblview_student.Id inner join tblbatch on tblincedent_history.BatchId = tblbatch.Id inner join tblclass on tblclass.Id = tblincedent_history.ClassId  where tblincedent_history.UserType= 'student' and tblincedent_history.Status = 'Approved' and tblincedent_history.BatchId= " + _BatchVal + " and tblincedent_history.ClassId =" + _ClassVal + " GROUP BY AssoUser ";
                        MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                        if (MyReader.HasRows)
                        {
                            int i = 0;
                            Pnl_Student_Point_Rank.Visible = true;
                            while (MyReader.Read())
                            {

                                int _Sid = int.Parse(MyReader.GetValue(0).ToString());
                                int _P = int.Parse(MyReader.GetValue(4).ToString());
                                int _Bid = MyUser.CurrentBatchId;
                                //int _Bid = int.Parse(MyReader.GetValue(5).ToString());
                                int _Cid = int.Parse(MyReader.GetValue(6).ToString());
                                double _CurrentRate = 0;
                                string _table = "tblincedent";
                                int _TotalStudents = GetTotal_StudentsInClass(_Cid, _Bid);
                                int _TotalPoints = GetTotal_ClassIncidentPoints(_Cid, _Bid, _table);
                                if (_TotalStudents == 0)
                                {
                                    _CurrentRate = _P - _TotalPoints;
                                }
                                else if (_TotalPoints == 0)
                                {
                                    _CurrentRate = _P;
                                }
                                else
                                {
                                    _CurrentRate = _P - (_TotalPoints / _TotalStudents);
                                }
                                dr = MyDataSet.Tables["tblPointRate"].NewRow();
                                dr["Rank"] = (i + 1).ToString();
                                dr["StudentName"] = MyReader.GetValue(1);
                                dr["BatchName"] = MyReader.GetValue(2);
                                dr["ClassName"] = MyReader.GetValue(3);
                                dr["Points"] = 0;
                                dr["Ratings"] = _CurrentRate;
                                MyDataSet.Tables["tblPointRate"].Rows.Add(dr);
                                i++;

                            }

                        }
                        else
                        {
                            WC_MessageBox.ShowMssage("No Record Found!");
                            Pnl_Student_Point_Rank.Visible = false;
                        }
                        Grd_PointAndRating.DataSource = MyDataSet;
                        Session["PointRateList"] = MyDataSet;
                        Grd_PointAndRating.DataBind();
                        Grd_PointAndRating.Columns[4].Visible = false;
                        Grd_PointAndRating.Columns[5].Visible = true;
                        Grd_PointAndRating.Columns[2].Visible = false;
                        sortRating();
                    }
                }
           
            }
                
                
     }

        public void sortRating()
        {
            int _rank = 1, count = 0;
            Grd_PointAndRating.Columns[4].Visible = true;
            Grd_PointAndRating.Columns[5].Visible = true;

            Grd_PointAndRating.PageIndex = 0;

            DataSet MydataSet = (DataSet)Session["PointRateList"];
            
            DataTable dtCust = MydataSet.Tables[0];
            DataView dataView = new DataView(dtCust);

            dataView.Sort = "Ratings DESC";
            Grd_PointAndRating.DataSource = dataView;
            Grd_PointAndRating.DataBind();
            
            foreach (GridViewRow gv in Grd_PointAndRating.Rows)
            {

                MydataSet.Tables[0].Rows[count]["Rank"] = (_rank).ToString();
                MydataSet.Tables[0].Rows[count]["StudentName"] = gv.Cells[1].Text;
                MydataSet.Tables[0].Rows[count]["BatchName"] = gv.Cells[2].Text;
                MydataSet.Tables[0].Rows[count]["ClassName"] = gv.Cells[3].Text;
                MydataSet.Tables[0].Rows[count]["Points"] = 0;
                MydataSet.Tables[0].Rows[count]["Ratings"] = gv.Cells[5].Text;
                _rank++;
                count++;

            }
            Grd_PointAndRating.DataSource = MydataSet;
            Grd_PointAndRating.DataBind();
            Session["PointRateList"] = MydataSet;
            Grd_PointAndRating.Columns[4].Visible = false;
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            Session["SortDirection"] = sortDirection;
            Session["SortExpression"] = column;

            return sortDirection;
        }
        
        #endregion

        public string GetHeaderText()
        {
            string _header;
                _header= Drp_PointAndRating.SelectedItem.Text.ToString();
                return _header;
        }

        public int GetTotal_StudentsInClass(int ClassId, int CurrentBatchId)
        {
            int count = 0;
            string sql = "SELECT Count(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudent.Status=1 AND tblstudentclassmap.ClassId=" + ClassId + " AND tblstudentclassmap.BatchId=" + CurrentBatchId;
            m_MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out count);
            }
            return count;
        }
        private int GetTotal_ClassIncidentPoints(int ClassId, int _CurrentBatchid, string IncedentTable)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student'  AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + _CurrentBatchid;
            m_MyReader =MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }
        public int GetMonthlyRating(string Type, int Month, int StudentId, int ClassId, int BatchId, string IncedentTable)
        {
            int MonthlyPoints = 0;
            int MonthlyRating = 0;
            int Total_ClassPoints = GetMonthly_ClassIncidentPoints(ClassId, BatchId, Month, IncedentTable);
            int Total_No_User = GetTotal_StudentsInClass(ClassId, BatchId);
            if (Total_No_User > 0)
            {
                int Current_Batch_Class_AVG = Total_ClassPoints / Total_No_User;
                string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student' AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + BatchId + " AND " + IncedentTable + ".AssoUser=" + StudentId + " AND Month(" + IncedentTable + ".IncedentDate)=" + Month;
                m_MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out MonthlyPoints);
                }
                MonthlyRating = MonthlyPoints - Current_Batch_Class_AVG;
            }
            return MonthlyRating;
        }
        public int GetMonthly_ClassIncidentPoints(int ClassId, int BatchId, int Month, string IncedentTable)
        {
            int Totalpoints = 0;
            string sql = "SELECT SUM(" + IncedentTable + ".`Point`) FROM " + IncedentTable + " WHERE " + IncedentTable + ".UserType='student' AND " + IncedentTable + ".Status='Approved' AND " + IncedentTable + ".ClassId=" + ClassId + " AND " + IncedentTable + ".BatchId=" + BatchId + " AND Month(" + IncedentTable + ".IncedentDate)=" + Month;
            m_MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out Totalpoints);
            }
            return Totalpoints;
        }

        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ExportDataSet = (DataSet)Session["PointRateList"]; 

            string FileName = "PointRateReport";
            string _ReportName = "PointRateReport";
            if (!WinEr.ExcelUtility.ExportDataToExcel(ExportDataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_Message.Text = "This function need Ms office";
            }
        }

      
        protected void Grd_PointAndRating_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_PointAndRating.Columns[4].Visible = true;
            Grd_PointAndRating.Columns[5].Visible = true;

            Grd_PointAndRating.PageIndex = e.NewPageIndex;
            DataSet MyDataSet1 = (DataSet)Session["PointRateList"];

            Grd_PointAndRating.DataSource = MyDataSet1;
            Grd_PointAndRating.DataBind();
            Grd_PointAndRating.Columns[4].Visible = false;
            Grd_PointAndRating.Columns[5].Visible = false;
        }

        protected void Grd_PointAndRating_Sorting(object sender, GridViewSortEventArgs e)
        {
           bool _Col2,_Col4, _Col5;
           if (Grd_PointAndRating.Columns[2].Visible)
               _Col2 = true;
           else
               _Col2 = false;
           if (Grd_PointAndRating.Columns[4].Visible)
               _Col4 = true;
           else
               _Col4 = false;
           if (Grd_PointAndRating.Columns[5].Visible)
               _Col5 = true;
           else
               _Col5 = false;

            Grd_PointAndRating.Columns[2].Visible = true;
            Grd_PointAndRating.Columns[4].Visible = true;
            Grd_PointAndRating.Columns[5].Visible = true;

            
            DataSet MyDataSet = (DataSet)Session["PointRateList"];
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MyDataSet.Tables[0];
                DataView dataView = new DataView(dtAccountData);
                dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                Grd_PointAndRating.DataSource = dataView;
                Grd_PointAndRating.DataBind();
              
                DataSet ds = new DataSet();
                DataTable dTable = dataView.ToTable();
                ds.Tables.Add(dTable);
                Session["PointRateList"] = ds;

                if (!_Col2)
                    Grd_PointAndRating.Columns[2].Visible = false;
                if (!_Col4)
                    Grd_PointAndRating.Columns[4].Visible = false;
                if (!_Col5)
                    Grd_PointAndRating.Columns[5].Visible = false;
            }
        }

    }
}
