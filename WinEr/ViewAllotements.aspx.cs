using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;

namespace WinEr
{
    public partial class ViewAllotements : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private int m_BatchId;
        private int m_ClassId;
        private int m_StandardId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();

            if (MyStudMang == null)
            {

                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(607))
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (Request.QueryString["Batch"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else if (!int.TryParse(Request.QueryString["Batch"].ToString(), out m_BatchId))
            {
                Response.Redirect("Default.aspx");
            }
            else if (Request.QueryString["StandardID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else if (!int.TryParse(Request.QueryString["StandardID"].ToString(), out m_StandardId))
            {
                Response.Redirect("Default.aspx");
            }

            else
            {

                if (!IsPostBack)
                {

                    if (Request.QueryString["StandardID"] != null && Request.QueryString["ClassId"] != null)
                    {
                        LoadClassDetailsOnClass(int.Parse(Request.QueryString["ClassId"]));
                    }
                    else if (Request.QueryString["StandardID"] != null)
                    {
                        LoadClassDetails(m_StandardId);
                    }
                  

                }

            }
        }

        private void LoadClassDetailsOnClass(int ClassId)
        {
            string sql = "";
            int _NOF_Seats = 0;
            int _Nof_Alloted_Seats = 0;
            int _NOF_FreeSeats = 0;
            int _NOF_WaitingSeats = 0;

            int Nofseats = 0;
            int Allotedseats = 0;
            int waitingseats = 0;
            int freeseats = 0;
            int appliedstudnts = 0;
            OdbcDataReader ClassReader = null;
            StringBuilder _table = new StringBuilder();
            sql = "select tblclass.Id from tblclass where tblclass.Id=" + ClassId + "";
            ClassReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (ClassReader.HasRows)
            {
                while (ClassReader.Read())
                {
                    _NOF_Seats = GetTotalSeatCount(int.Parse(ClassReader.GetValue(0).ToString()));
                    int _NOF_PerSeats = GetTotalPermenentStudentCount(m_BatchId, int.Parse(ClassReader.GetValue(0).ToString()));
                    OdbcDataReader Appliedstudreader = null;

                    string appliedsql = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.`Status`=1 AND tbltempstdent.Class=" + int.Parse(ClassReader.GetValue(0).ToString()) + " AND tbltempstdent.JoiningBatch=" + m_BatchId + "  and AdmissionStatusId<>6";
                    Appliedstudreader = MyStudMang.m_MysqlDb.ExecuteQuery(appliedsql);
                    if (Appliedstudreader.HasRows)
                    {
                        int.TryParse(Appliedstudreader.GetValue(0).ToString(), out appliedstudnts);
                    }
                    _NOF_PerSeats = _NOF_PerSeats + appliedstudnts;
                    int _NOF_TempSeats = GetTotalTemperoryStudentsCount(m_BatchId, int.Parse(ClassReader.GetValue(0).ToString()));

                    string sql1 = "";
                    OdbcDataReader Waitingliststudreader = null;
                    sql1 = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.`Status`=1 AND tbltempstdent.Class=" + int.Parse(ClassReader.GetValue(0).ToString()) + " AND tbltempstdent.JoiningBatch=" + m_BatchId + " and AdmissionStatusId=6";
                    Waitingliststudreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql1);
                    if (Waitingliststudreader.HasRows)
                    {
                        int.TryParse(Waitingliststudreader.GetValue(0).ToString(), out _NOF_WaitingSeats);
                    }

                    _Nof_Alloted_Seats = 0;
                    _NOF_FreeSeats = 0;
                  //  _NOF_WaitingSeats = 0;
                    //if ((_NOF_PerSeats + _NOF_TempSeats) >= _NOF_Seats)
                    //{
                    //    _Nof_Alloted_Seats = _NOF_Seats;
                    //    _NOF_FreeSeats = 0;
                    //    //if (_NOF_PerSeats > _NOF_Seats)
                    //    //    _NOF_WaitingSeats = _NOF_TempSeats;
                    //    //else
                    //    //    _NOF_WaitingSeats = (_NOF_PerSeats + _NOF_TempSeats) - _NOF_Seats;
                    //}
                    //else
                    //{
                        // _Nof_Alloted_Seats = _NOF_PerSeats + _NOF_TempSeats;
                        // _NOF_WaitingSeats = 0;
                        _Nof_Alloted_Seats = _NOF_PerSeats;
                        _NOF_FreeSeats = _NOF_Seats - (_NOF_PerSeats );
                        //+ _NOF_TempSeats
                   // }


                    string _ClassName = GetClassName(int.Parse(ClassReader.GetValue(0).ToString()));
                    string _Batch = GetBatchName(m_BatchId);

                    string _sql = "";
                    OdbcDataReader StdReader = null;

                    _sql = "select tblstandard.Name from tblstandard where tblstandard.Id=" + m_StandardId + "";
                    StdReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                    if (StdReader.HasRows)
                    {
                        Lbl_CourseName.Text = StdReader.GetValue(0).ToString();
                        Lbl_Year.Text = "(" + _Batch + ")";
                    }
                    //(int.Parse(Lbl_TNoSeat.Text) + _NOF_Seats).ToString();
                    //(int.Parse(Lbl_TNOallotedSeats.Text) + _Nof_Alloted_Seats).ToString();
                    //(int.Parse(Lbl_TNoFreeStat.Text) + _NOF_FreeSeats).ToString();
                    //(int.Parse(Lbl_WaitingList.Text) + _NOF_WaitingSeats).ToString();

                    Lbl_TNoSeat1.Text = _NOF_Seats.ToString();
                    Nofseats = Nofseats + _NOF_Seats;
                    Hdn_Totalpermseat.Value = _NOF_PerSeats.ToString();
                    Lbl_TNOallotedSeats1.Text = _Nof_Alloted_Seats.ToString();
                    Allotedseats = Allotedseats + _Nof_Alloted_Seats;
                    Lbl_TNoFreeStat1.Text = _NOF_FreeSeats.ToString();
                    freeseats = freeseats + _NOF_FreeSeats;
                    Lbl_WaitingList1.Text = _NOF_WaitingSeats.ToString();
                   // waitingseats = waitingseats + _NOF_WaitingSeats;
                    _table = LoadAllotements(_table, int.Parse(ClassReader.GetValue(0).ToString()), _ClassName);
                    Lbl_WaitingList1.Text = Hdn_Watngstdnts.Value;
                    waitingseats = waitingseats + int.Parse(Hdn_Watngstdnts.Value);

                }

                LoadUnassignedClassStudentInWaitingList(m_StandardId);
                Lbl_TNoSeat.Text = Nofseats.ToString();
                Lbl_TNOallotedSeats.Text = Allotedseats.ToString();
                Lbl_TNoFreeStat.Text = freeseats.ToString();
                Lbl_WaitingList.Text = waitingseats.ToString();

                this.AllotedListDiv.InnerHtml = _table.ToString();
                if (Session["_TableWaiting"] != null)
                {
                    this.WaitingListDIV.InnerHtml = Session["_TableWaiting"].ToString();
                    Pnl_WaitingList_Area.Visible = true;
                    Session["_TableWaiting"] = null;
                    ViewState["Count"] = null;
                }
            }
        }

        private string GetFreeBox(int Count)
        {
            return "<td class=\"Freeseat\"><p ><span class=\"Num\">" + Count + "</span></p>Free</td>";

        }

        private string GetStudentBox(int _studId, int _TypeID, int Count, string _Name, string _CreatedUser, string _AllotedType,string TempId,int classId)
        {
            string _Style = "";
            string page = "SutdDetailsPupUp.aspx?StudId=" + _studId + "&Type=" + _TypeID;
            if (_TypeID == 1)
            {
                page = "RegisteredStudentDetails.aspx?TempStudId=" + TempId+"&ClassId="+classId+"";
            }
            if (_AllotedType == "PERMANENT")
                _Style = "Permenent";
            else if (_AllotedType == "ALLOTTED")
            {
                _Style = "Alloted";
            }
            else
            {
                _Style = "Urgent";
                OdbcDataReader ClassReader = null;
                if (classId > 0)
                {
                    string sql = "select tblclass.ClassName from tblclass where Id=" + classId + " ";
                    ClassReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (ClassReader.HasRows)
                    {
                        _CreatedUser = ClassReader.GetValue(0).ToString();
                    }
                }

            }
            if (_Name.Length > 10)
            {
                _Name = _Name.Substring(0, 10) + "..";
            }
            if (_CreatedUser.Length > 10)
            {
                _CreatedUser = _CreatedUser.Substring(0, 10) + "..";
            }

            return "<td class=\"" + _Style + "\"><p ><span class=\"Num\">" + Count + "</span><a href=\"" + page + "\"  class=\"name\" onclick=\"window.open(this.href, 'popupwindow', 'width=600,height=500,scrollbars,resizable');return false;\">" + _Name + "</a></p>" + _CreatedUser + "<br /></td>";
        }

        private void LoadClassDetails(int m_StandardId)
        {
            string sql = "";
            int _NOF_Seats = 0;
            int _Nof_Alloted_Seats = 0;
            int _NOF_FreeSeats = 0;
            int _NOF_WaitingSeats = 0;
            int appliedstudnts = 0;
            int Nofseats = 0;
            int Allotedseats = 0;
            int waitingseats = 0;
            int freeseats = 0;
            OdbcDataReader ClassReader = null;
            StringBuilder _table = new StringBuilder();
            sql = "select tblclass.Id from tblclass where tblclass.Standard=" + m_StandardId + "";
            ClassReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (ClassReader.HasRows)
            {
                while (ClassReader.Read())
                {
                     _NOF_Seats = GetTotalSeatCount(int.Parse(ClassReader.GetValue(0).ToString()));
                    int _NOF_PerSeats = GetTotalPermenentStudentCount(m_BatchId, int.Parse(ClassReader.GetValue(0).ToString()));
                    OdbcDataReader Appliedstudreader = null;

                    string appliedsql = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.`Status`=1 AND tbltempstdent.Class=" + int.Parse(ClassReader.GetValue(0).ToString()) + "  AND tbltempstdent.JoiningBatch=" + m_BatchId + " and AdmissionStatusId<>6";
                    Appliedstudreader = MyStudMang.m_MysqlDb.ExecuteQuery(appliedsql);
                    if (Appliedstudreader.HasRows)
                    {
                        int.TryParse(Appliedstudreader.GetValue(0).ToString(), out appliedstudnts);
                    }
                    _NOF_PerSeats = _NOF_PerSeats + appliedstudnts;
                    int _NOF_TempSeats = GetTotalTemperoryStudentsCount(m_BatchId, int.Parse(ClassReader.GetValue(0).ToString()));
                    string sql1 = "";
                    OdbcDataReader Waitingliststudreader = null;
                    sql1 = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.`Status`=1 AND tbltempstdent.Class=" + int.Parse(ClassReader.GetValue(0).ToString()) + " AND tbltempstdent.JoiningBatch=" + m_BatchId + " and AdmissionStatusId=6";
                    Waitingliststudreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql1);
                    if (Waitingliststudreader.HasRows)
                    {
                        int.TryParse(Waitingliststudreader.GetValue(0).ToString(), out _NOF_WaitingSeats);
                    }
                     _Nof_Alloted_Seats = 0;
                     _NOF_FreeSeats = 0;

                   //  _NOF_WaitingSeats = 0;
                    //if ((_NOF_PerSeats + _NOF_TempSeats) >= _NOF_Seats)
                    //{
                    //    _Nof_Alloted_Seats = _NOF_Seats;
                    //    _NOF_FreeSeats = 0;
                    //    _NOF_FreeSeats = _NOF_Seats - (_NOF_PerSeats);
                    //    //if (_NOF_PerSeats > _NOF_Seats)
                    //    //    _NOF_WaitingSeats = _NOF_TempSeats;
                    //    //else
                    //    //    _NOF_WaitingSeats = (_NOF_PerSeats + _NOF_TempSeats) - _NOF_Seats;
                    //}
                    //else
                    //{
                       // _Nof_Alloted_Seats = _NOF_PerSeats + _NOF_TempSeats;
                       // _NOF_WaitingSeats = 0;
                        _Nof_Alloted_Seats = _NOF_PerSeats;
                        _NOF_FreeSeats = _NOF_Seats - (_NOF_PerSeats );//+ _NOF_TempSeats
                   // }


                    string _ClassName = GetClassName(int.Parse(ClassReader.GetValue(0).ToString()));
                    string _Batch = GetBatchName(m_BatchId);

                    string _sql = "";
                    OdbcDataReader StdReader = null;

                    _sql = "select tblstandard.Name from tblstandard where tblstandard.Id=" + m_StandardId + "";
                    StdReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                    if (StdReader.HasRows)
                    {
                        Lbl_CourseName.Text = StdReader.GetValue(0).ToString();
                        Lbl_Year.Text = "(" + _Batch + ")";
                    }
                    //(int.Parse(Lbl_TNoSeat.Text) + _NOF_Seats).ToString();
                    //(int.Parse(Lbl_TNOallotedSeats.Text) + _Nof_Alloted_Seats).ToString();
                    //(int.Parse(Lbl_TNoFreeStat.Text) + _NOF_FreeSeats).ToString();
                    //(int.Parse(Lbl_WaitingList.Text) + _NOF_WaitingSeats).ToString();

                    Lbl_TNoSeat1.Text = _NOF_Seats.ToString();
                    Hdn_Totalpermseat.Value = _NOF_PerSeats.ToString();
                    Nofseats = Nofseats + _NOF_Seats;
                    Lbl_TNOallotedSeats1.Text = _Nof_Alloted_Seats.ToString();
                    Allotedseats = Allotedseats + _Nof_Alloted_Seats;
                    Lbl_TNoFreeStat1.Text = _NOF_FreeSeats.ToString();
                    freeseats = freeseats + _NOF_FreeSeats;
                    Lbl_WaitingList1.Text = _NOF_WaitingSeats.ToString();
                   // waitingseats = waitingseats + _NOF_WaitingSeats;
                    _table = LoadAllotements(_table, int.Parse(ClassReader.GetValue(0).ToString()), _ClassName);
                    Lbl_WaitingList1.Text = Hdn_Watngstdnts.Value;                 
                    waitingseats = waitingseats + int.Parse(Hdn_Watngstdnts.Value);
                }
                LoadUnassignedClassStudentInWaitingList(m_StandardId);
                Lbl_TNoSeat.Text = Nofseats.ToString();
                Lbl_TNOallotedSeats.Text = Allotedseats.ToString();
                Lbl_TNoFreeStat.Text = freeseats.ToString();
                int unaswtng=0;
                int.TryParse(Lbl_Data1.Text, out unaswtng);
                Lbl_WaitingList.Text =( waitingseats+unaswtng).ToString();

                this.AllotedListDiv.InnerHtml = _table.ToString();
                if (Session["_TableWaiting"] != null)
                {
                    this.WaitingListDIV.InnerHtml = Session["_TableWaiting"].ToString();
                    Pnl_WaitingList_Area.Visible = true;
                    Session["_TableWaiting"] = null;
                    ViewState["Count"] = null;
                }
            }
        }

        private void LoadUnassignedClassStudentInWaitingList(int m_StandardId)
        {
            string sql = "";
            int count1 = 0;
            int Unasndcount = 0;
            OdbcDataReader Standardreader = null;
            sql = " SELECT tbltempstdent.Id, tbltempstdent.Name, tbltempstdent.CreatedUserName,tbltempstdent.TempId,tbltempstdent.Class FROM tbltempstdent WHERE tbltempstdent.Standard=" + m_StandardId + " AND  tbltempstdent.JoiningBatch=" + m_BatchId + " AND tbltempstdent.`Status`=1 and AdmissionStatusId<>5";
            //sql = "  select tbltempstdent.Id,Name from tbltempstdent where Standard=" + m_StandardId + " and Class=0";
            Standardreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (Standardreader.HasRows)
            {
                StringBuilder _TableWaiting = new StringBuilder();
               // StringBuilder _HeaderTableWaiting = new StringBuilder();
                int _studId, _TypeID, Count;
                string _Name, _CreatedUser, _AllotedType;
               

                if (ViewState["Count"] != null)
                {
                    Count = int.Parse(ViewState["Count"].ToString());
                    count1 = int.Parse(ViewState["Count"].ToString());
                }
                else
                {
                    Count = 1;
                }
                if (Session["_TableWaiting"] == null)
                {
                    _TableWaiting.Append("<table  class=\"BoxDetails\">");
                    _TableWaiting.Append("<tr>");
                }
                else
                {
                    _TableWaiting = (StringBuilder)Session["_TableWaiting"];
                }
                if ((Count % 10) == 0)
                {
                    _TableWaiting.Append("</tr><tr>");
                }
                while (Standardreader.Read())
                {

                    if (Standardreader.GetValue(4).ToString() == "" || int.Parse(Standardreader.GetValue(4).ToString()) == 0)
                    {                       
                        _AllotedType = "WAITINGLIST";
                        _TypeID = 1;

                        _studId = int.Parse(Standardreader.GetValue(0).ToString());
                        _Name = Standardreader.GetValue(1).ToString();
                        _CreatedUser = Standardreader.GetValue(2).ToString();
                        string TempId = Standardreader.GetValue(3).ToString();
                        int ClassID;
                        int.TryParse(Standardreader.GetValue(4).ToString(), out ClassID);
                        if (ViewState["Count"] != null)
                        {

                            Count++;
                        }
                        _TableWaiting.Append(GetStudentBox(_studId, _TypeID, Count, _Name, _CreatedUser, _AllotedType, TempId, ClassID));
                        if ((Count % 10) == 0)
                        {
                            _TableWaiting.Append("</tr><tr>");
                        }
                        if (ViewState["Count"] == null)
                        {

                            Count++;
                        }
                        Unasndcount++;
                    }
                }

                Lbl_Data.Text = count1.ToString();
                Lbl_Data1.Text = Unasndcount.ToString();
                _TableWaiting.Append("</tr>");
                _TableWaiting.Append("</table>");
                Session["_TableWaiting"] = _TableWaiting;
            }

        }
    
        private StringBuilder LoadAllotements(StringBuilder _Table, int ClassId,string ClassName)
        {
            if (_Table == null)
            {

                _Table = new StringBuilder();
            }
            StringBuilder _TableWaiting;
            // StringBuilder HeadingReader = new StringBuilder();

            int _NOF_Seats = int.Parse(Lbl_TNoSeat1.Text);
            int _Nof_Alloted_Seats = int.Parse(Lbl_TNOallotedSeats1.Text);
            int _NOF_FreeSeats = int.Parse(Lbl_TNoFreeStat1.Text);
            int _NOF_WaitingSeats = int.Parse(Lbl_WaitingList1.Text);


            // _Table.Append("<tr>");
            //_Table.Append(" <td></td>");
            //_Table.Append(" <td align=\"right\" >Class Name:</td>");
            //_Table.Append("<td align=\"left\" style=\"font-weight:bold\">Test</td>");
            //_Table.Append(" <td></td>");
            //_Table.Append(" </tr>");

            _Table.Append("<center>");
            _Table.Append("<table width=\"200px\"> ");
            _Table.Append("<tr align=\"left\">");
            _Table.Append("<td  align=\"left\" ><b><u>Class Name:</u></b><b>" + ClassName + "</b></td>");
            _Table.Append("</tr></table>");
            _Table.Append(" <br />");

            _Table.Append("</center>");

           

            _Table.Append("<table  class=\"BoxDetails\">");
            _Table.Append("<tr>");

            int _studId, _TypeID, Count;
            string _Name, _CreatedUser, _AllotedType;
            Count = 1;
            int _i = 0;

            //Add permenent seats
            _AllotedType = "PERMANENT";
            _TypeID = 2;
            string sql = "SELECT tblstudent.Id,tblstudent.StudentName, tblstudent.CreatedUserName,LastClassId from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId = tblstudent.Id where tblstudentclassmap.BatchId=" + m_BatchId + " AND tblstudentclassmap.ClassId=" + ClassId + " and tblstudent.`Status`=1";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_Student in MyDataSet.Tables[0].Rows)
                {
                    _studId = int.Parse(dr_Student[0].ToString());
                    _Name = dr_Student[1].ToString();
                    _CreatedUser = dr_Student[2].ToString();
                   int ClassID;
                   int.TryParse(dr_Student[3].ToString(), out ClassID);
                   _Table.Append(GetStudentBox(_studId, _TypeID, Count, _Name, _CreatedUser, _AllotedType, "", ClassID));
                    if ((Count % 10) == 0)
                    {
                        _Table.Append("</tr><tr>");
                    }
                    Count++;
                }
            }


            //Add Temperory seats
            _AllotedType = "ALLOTTED";
            _TypeID = 1;
            int[] tempid=new int[0] ;
            sql = "SELECT tbltempstdent.Id, tbltempstdent.Name, tbltempstdent.CreatedUserName,tbltempstdent.TempId,tbltempstdent.Class FROM tbltempstdent WHERE tbltempstdent.Class=" + ClassId + " AND  tbltempstdent.JoiningBatch=" + m_BatchId + " AND tbltempstdent.`Status`=1 and AdmissionStatusId<>6 order by tbltempstdent.Rank ";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                tempid = new int[MyDataSet.Tables[0].Rows.Count];
                for (_i = 0; _i < MyDataSet.Tables[0].Rows.Count && Count <= _Nof_Alloted_Seats; _i++)
                {

                    _studId = int.Parse(MyDataSet.Tables[0].Rows[_i][0].ToString());
                    _Name = MyDataSet.Tables[0].Rows[_i][1].ToString();
                    _CreatedUser = MyDataSet.Tables[0].Rows[_i][2].ToString();
                   string  TempId = MyDataSet.Tables[0].Rows[_i][3].ToString();
                   int ClassID ;
                   int.TryParse(MyDataSet.Tables[0].Rows[_i][4].ToString(), out ClassID);
                   _Table.Append(GetStudentBox(_studId, _TypeID, Count, _Name, _CreatedUser, _AllotedType, TempId, ClassID));
                    if ((Count % 10) == 0)
                    {
                        _Table.Append("</tr><tr>");
                    }
                    Count++;
                    tempid[_i] = int.Parse(MyDataSet.Tables[0].Rows[_i][0].ToString());
                }
               

            }


            //Add Free seats
            for (int _j = 0; _j < _NOF_FreeSeats; _j++)
            {
                _Table.Append(GetFreeBox(Count));
                if ((Count % 10) == 0)
                {
                    _Table.Append("</tr><tr>");
                }
                Count++;
            }

            _Table.Append("</tr>");
            _Table.Append("</table>");

            //Add Waiting List seats
            Count = 1;
            int asndCount = 0;
            if (_NOF_WaitingSeats == 0)
            {
                Pnl_WaitingList_Area.Visible = false;
            }
            else
            {
                //Session["_TableWaiting"] = null;
                if (Session["_TableWaiting"] == null)
                {
                    _TableWaiting = new StringBuilder();
                }
                else
                {
                    _TableWaiting = (StringBuilder)Session["_TableWaiting"];
                }
                _TableWaiting.Append("<table  class=\"BoxDetails\">");
                _TableWaiting.Append("<tr>");
               
                _AllotedType = "WAITINGLIST";
                _TypeID = 1;
                DataSet WaitingListDs = new DataSet();
                string Sql2 = "SELECT tbltempstdent.Id, tbltempstdent.Name, tbltempstdent.CreatedUserName,tbltempstdent.TempId,tbltempstdent.Class FROM tbltempstdent WHERE tbltempstdent.Class=" + ClassId + " AND  tbltempstdent.JoiningBatch=" + m_BatchId + " AND tbltempstdent.`Status`=1 and AdmissionStatusId=6 order by tbltempstdent.Rank ";
                WaitingListDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(Sql2);
                {
                    for (_i = 0; _i < WaitingListDs.Tables[0].Rows.Count; _i++)
                    {

                        _studId = int.Parse(WaitingListDs.Tables[0].Rows[_i][0].ToString());
                        _Name = WaitingListDs.Tables[0].Rows[_i][1].ToString();
                        _CreatedUser = WaitingListDs.Tables[0].Rows[_i][2].ToString();
                        string TempId = WaitingListDs.Tables[0].Rows[_i][3].ToString();
                        int ClassID;
                        int.TryParse(WaitingListDs.Tables[0].Rows[_i][4].ToString(), out ClassID);
                        _TableWaiting.Append(GetStudentBox(_studId, _TypeID, Count, _Name, _CreatedUser, _AllotedType, TempId, ClassID));
                        if ((Count % 10) == 0)
                        {
                            _TableWaiting.Append("</tr><tr>");
                        }
                        asndCount++;
                        Count++;
                    }
                }

                if (ViewState["Count"]!=null)
                {
                    asndCount = int.Parse(ViewState["Count"].ToString()) + asndCount;
                }
                ViewState["Count"] = asndCount;
                //_TableWaiting.Append("</tr>");
                //_TableWaiting.Append("</table>");
                Session["_TableWaiting"] = _TableWaiting;
            }
            int perseats = 0;
            int.TryParse(Hdn_Totalpermseat.Value, out perseats);
            if (_NOF_Seats < perseats)
            {
            if(MyDataSet!=null && MyDataSet.Tables[0].Rows.Count>0)
            {

                if (Session["_TableWaiting"] == null)
                {
                    _TableWaiting = new StringBuilder();
                    _TableWaiting.Append("<table  class=\"BoxDetails\">");
                    _TableWaiting.Append("<tr>");
                }
                else
                {
                    _TableWaiting = (StringBuilder)Session["_TableWaiting"];
                }
               
                _AllotedType = "WAITINGLIST";
                _TypeID = 1;
                //for (int asignedtemp = 0; asignedtemp < tempid.Length; asignedtemp++)
                //{
                //    int RowCount = 0;
                //    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                //    {
                //        RowCount++;
                //        if (int.Parse(dr["Id"].ToString()) == tempid[asignedtemp])
                //        {
                //           MyDataSet.Tables[0].Rows[RowCount].Delete();
                //        }
                //        MyDataSet.Tables[0].AcceptChanges();

                //    }
                //}

                for (int i = MyDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = MyDataSet.Tables[0].Rows[i];

                    string dtitem = dr["Id"].ToString();
                    for (int asignedtemp = 0; asignedtemp < tempid.Length; asignedtemp++)
                    {
                        if (int.Parse(dtitem.ToString()) == tempid[asignedtemp])
                        {
                            MyDataSet.Tables[0].Rows.Remove(dr);
                           // MyDataSet.Tables[0].AcceptChanges();
                        }
                       
                    }
                }

                for (_i = 0; _i < MyDataSet.Tables[0].Rows.Count; _i++)
                {
                    _studId = int.Parse(MyDataSet.Tables[0].Rows[_i][0].ToString());
                    _Name = MyDataSet.Tables[0].Rows[_i][1].ToString();
                    _CreatedUser = MyDataSet.Tables[0].Rows[_i][2].ToString();
                    string TempId = MyDataSet.Tables[0].Rows[_i][3].ToString();
                    int ClassID;
                    int.TryParse(MyDataSet.Tables[0].Rows[_i][4].ToString(), out ClassID);
                    _TableWaiting.Append(GetStudentBox(_studId, _TypeID, Count, _Name, _CreatedUser, _AllotedType, TempId, ClassID));
                    if ((Count % 10) == 0)
                    {
                        _TableWaiting.Append("</tr><tr>");
                    }
                    asndCount++;
                    Count++;
                }
                ViewState["Count"] = asndCount;
                //_TableWaiting.Append("</tr>");
                //_TableWaiting.Append("</table>");
                Session["_TableWaiting"] = _TableWaiting;
                _NOF_WaitingSeats = asndCount;
            }
            }
            _Table.Append(" <br />");
            _Table.Append("<center>");
            _Table.Append("<table width=\"900px\">");
            _Table.Append(" <tr>");
            _Table.Append(" <td align=\"right\">Total No of seats:<b>" + _NOF_Seats + "</b></td>");
            _Table.Append(" <td align=\"right\" >Total No of free seats:<b>" + _NOF_FreeSeats + "</b></td>");
            _Table.Append("<td align=\"right\">Total No of allotted seats:<b>" + _Nof_Alloted_Seats + "</b></td>");
            _Table.Append("<td align=\"right\">Total No of Students in  Waiting List:<b>" + _NOF_WaitingSeats + "</b></td>");
            _Table.Append("</tr>");
            _Table.Append("</table>");
            _Table.Append("</center>");
            _Table.Append("<div class=\"linestyle\"></div>");

            Hdn_Watngstdnts.Value = _NOF_WaitingSeats.ToString();
            return _Table;
        }

        private string GetBatchName(int _BatchId)
        {
            string _BatchName = "";
            string sql = "select tblbatch.BatchName from tblbatch where tblbatch.Id=" + _BatchId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _BatchName = MyReader.GetValue(0).ToString();
            }

            return _BatchName;
        }

        private string GetClassName(int _ClassId)
        {
            string _Name = "";
            string sql = "SELECT tblclass.ClassName FROM tblclass WHERE tblclass.Id=" + _ClassId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _Name = MyReader.GetValue(0).ToString();
            }

            return _Name;
        }

        private int GetTotalTemperoryStudentsCount(int _BatchId, int _ClassId)
        {
            int _NOFTempSeats = 0;
            string sql = "SELECT COUNT(tbltempstdent.Id) FROM tbltempstdent WHERE tbltempstdent.`Status`=1 AND tbltempstdent.Class=" + _ClassId + " AND tbltempstdent.JoiningBatch=" + _BatchId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                if (!int.TryParse(MyReader.GetValue(0).ToString(), out _NOFTempSeats))
                {
                    _NOFTempSeats = 0;
                }

            }

            return _NOFTempSeats;
        }

        private int GetTotalPermenentStudentCount(int _BatchId, int _ClassId)
        {
            int _NOFperSeats = 0;
            string sql = "SELECT COUNT(tblstudentclassmap.StudentId) FROM tblstudentclassmap INNER JOIN tblstudent ON tblstudent.Id=tblstudentclassmap.StudentId WHERE tblstudentclassmap.ClassId=" + _ClassId + " AND tblstudentclassmap.BatchId=" + _BatchId + " AND tblstudent.`Status`=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                if (!int.TryParse(MyReader.GetValue(0).ToString(), out _NOFperSeats))
                {
                    _NOFperSeats = 0;
                }

            }

            return _NOFperSeats;
        }

        private int GetTotalSeatCount(int _ClassId)
        {
            int _NOFSeats = 0;
            OdbcDataReader SeatReader = null;
            string sql = "SELECT tblclass.TotalSeats FROM tblclass WHERE tblclass.Id=" + _ClassId;
            SeatReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (SeatReader.HasRows)
            {
                if (!int.TryParse(SeatReader.GetValue(0).ToString(), out _NOFSeats))
                {
                    _NOFSeats = 0;
                }

            }


            return _NOFSeats;
        }
    }
}
