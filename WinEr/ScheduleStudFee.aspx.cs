using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
namespace WinEr
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private int MasterBatchId;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("ManageFeeAccount.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(38))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                //if (Rdo_Batch.SelectedValue == "0" )
                //    MasterBatchId = MyUser.CurrentBatchId;
                //else
                //    MasterBatchId = MyUser.CurrentBatchId + 1;
                if(Rdo_Batch1.SelectedValue == "0")
                    MasterBatchId = MyUser.CurrentBatchId;
                else
                    MasterBatchId = MyUser.CurrentBatchId + 1;
              
                if (!IsPostBack)
                {
                    if (MyFeeMang.HasNextBatchSchedule())
                    {
                        Label_NextBatch.Visible = true;
                        Rdo_Batch1.Visible = true;
                    }
                    else
                    {
                        Label_NextBatch.Visible = false;
                        Rdo_Batch1.Visible = false;
                    }
                    string _MenuStr;
                    //_MenuStr = MyFeeMang.GetFeeMangMenuString(MyUser.UserRoleId);
                    //this.FeeMenu.InnerHtml = _MenuStr;
                    _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                    this.SubFeeMenu.InnerHtml = _MenuStr;
                    LoadDetails();
                    //LoadClasswiseSchDetails();
                    //TabPanel1.Visible = false;
                    LoadAdvancedSecheduleLists();
                    //some initlization

                }
            }

        }

        protected void Grd_Amound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = e.Row.Cells[2].Text;
                if (value == "-1")
                {
                    e.Row.Cells[2].Text = "No Roll No.";
                }
            }
        }

        private void loadDeatilTocastDrpbox()
        {
            Drp_Cast.Items.Clear();
            ListItem li = new ListItem("Any", "0");
            Drp_Cast.Items.Add(li);
            string sql = "select tblcast.Id , tblcast.castname from tblcast";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    Drp_Cast.Items.Add(li);
                }

            }
            else
            {
               li = new ListItem("No Caste Found", "-1");

                Drp_Cast.Items.Add(li);
            }
            Drp_Cast.SelectedIndex = 0;
        }

        

        private void LoadAdvancedSecheduleLists()
        {
            if (AddClassToNewDrp())
            {
                BtnSch2.Enabled = true;
                AddPeriodToNewDrp();
                //AddStudtypeTodrp();
                Drp_SeatypeToDrpList();
                loadDeatilTocastDrpbox();

                LoadStudentListToGrid();
               // GetFoundStudCountString();
            }
            else
            {
                BtnSch2.Enabled = false;
            }
        }

        private void Drp_SeatypeToDrpList()
        {

            Drp_Seatype.Items.Clear();
            string sql = "select tblstudtype.Id, tblstudtype.TypeName from tblstudtype";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Any", "0");
                Drp_Seatype.Items.Add(li);
                while (MyReader.Read())
                {

                    li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    Drp_Seatype.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No data Found", "-1");

                Drp_Seatype.Items.Add(li);
            }
            Drp_Seatype.SelectedIndex = 0;
        }


        protected void Drp_Seatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }
        //private void GetFoundStudCountString()
        //{
        //    string _Countstring;
        //    int _count=0;
        //    string sql;
        //    if (Drp_className1.SelectedValue != "0")
        //    {
        //        sql = "select COUNT(tblstudent.Id) from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id= tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (SELECT tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_periodNew.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId1 + ") AND tblstudentclassmap.ClassId=" + Drp_className1.SelectedValue;
        //    }
        //    else
        //    {
        //        sql = "select COUNT(tblstudent.Id) from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id= tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (SELECT tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_periodNew.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId1 + ") AND tblstudentclassmap.ClassId IN (SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
        //    }
          

        //    if (Drp_Sex.SelectedValue != "Any")
        //    {
        //        sql = sql + " AND tblstudent.Sex='" + Drp_Sex.SelectedValue + "'";
        //    }
        //    if (Drp_StudType.SelectedValue != "0")
        //    {
        //        sql = sql + " AND tblstudent.StudTypeId=" + Drp_StudType.SelectedValue;
        //    }
        //    if (Drp_Seatype.SelectedValue != "0")
        //    {
        //        sql = sql + " AND tblstudent.AdmissionTypeId=" + Drp_Seatype.SelectedValue;
        //    }
        //    if(Drp_Cast.SelectedValue !="0")
        //    {
        //        sql = sql + " AND tblstudent.Cast='" + Drp_Cast.SelectedValue + "'";
        //    }
        //    if (int.Parse(Drp_CollegeBus.SelectedValue) > -1)
        //    {
        //        sql = sql + " AND tblstudent.UseBus=" + Drp_CollegeBus.SelectedValue;
        //    }
        //    if (int.Parse(Drp_Hostel.SelectedValue) > -1)
        //    {
        //        sql = sql + " AND tblstudent.UseHostel=" + Drp_Hostel.SelectedValue;
        //    }
        //    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        if (!int.TryParse(MyReader.GetValue(0).ToString(), out _count))
        //        {
        //            _count = 0;
        //        }
               
        //    }
        //    _Countstring = "<img src=\"Pics/user1.png\" width=\"25px\" height=\"22px\" /> <span style=\"color:#FF9900\"><b>" + _count .ToString()+ ", Students Found</b></span>";
        //    this.StudCountString.InnerHtml= _Countstring;
        //}
   
        private void LoadStudentListToGrid()
        {
            Lbl_Note1.Text = "";
            Grd_ScrechStud.Columns[0].Visible = true;
            Grd_ScrechStud.Columns[1].Visible = true;
            string sql;
            string _Feestudents = "-1";
            string feeID= Session["FeeId"].ToString();
            
                sql = "SELECT distinct tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_periodNew.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId + "";
                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                while (MyReader.Read())
                {
                    _Feestudents = _Feestudents + "," + MyReader.GetValue(0).ToString();
                }

                if (Drp_className1.SelectedValue != "0")
                {
                    sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex, tblstudtype.TypeName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (" + _Feestudents + ") AND tblstudentclassmap.ClassId=" + Drp_className1.SelectedValue;
                    //  sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex, tblstudtype.TypeName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (SELECT tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_periodNew.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId1 + ") AND tblstudentclassmap.ClassId=" + Drp_className1.SelectedValue;
                }
                else
                {
                    // sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex, tblstudtype.TypeName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (SELECT tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_periodNew.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId1 + ") AND tblstudentclassmap.ClassId IN (SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                    sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex, tblstudtype.TypeName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (" + _Feestudents + ") AND tblstudentclassmap.ClassId IN (SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                }

                if (Drp_Sex.SelectedValue != "Any")
                {
                    sql = sql + " AND tblstudent.Sex='" + Drp_Sex.SelectedValue + "'";
                }
                if (Drp_StudType.SelectedValue != "0")
                {
                    sql = sql + " AND tblstudent.AdmissionTypeId=" + Drp_StudType.SelectedValue;
                }
                if (Drp_Seatype.SelectedValue != "0")
                {
                    sql = sql + " AND tblstudent.StudTypeId =" + Drp_Seatype.SelectedValue;
                }
                if (Drp_Cast.SelectedValue != "0")
                {
                    sql = sql + " AND tblstudent.Cast='" + Drp_Cast.SelectedValue + "'";
                }
                if (int.Parse(Drp_CollegeBus.SelectedValue) > -1)
                {
                    sql = sql + " AND tblstudent.UseBus=" + Drp_CollegeBus.SelectedValue;
                }
                if (int.Parse(Drp_Hostel.SelectedValue) > -1)
                {
                    sql = sql + " AND tblstudent.UseHostel=" + Drp_Hostel.SelectedValue;
                }
               
                sql = sql + " Order by tblstudentclassmap.Standard ASC ,tblstudentclassmap.RollNo ASC ,tblstudent.StudentName ASC";
                MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_ScrechStud.DataSource = MydataSet;
                    Grd_ScrechStud.DataBind();
                    Grd_ScrechStud.Columns[0].Visible = false;
                    Grd_ScrechStud.Columns[1].Visible = false;
                    BtnSch2.Enabled = true;
                    Pnl_DataArea.Enabled = true;
                    Pnl_Studscreacharea.Visible = true;
                    string _Countstring = "<img src=\"Pics/user1.png\" width=\"25px\" height=\"22px\" /> <span style=\"color:#FF9900\"><b>" + MydataSet.Tables[0].Rows.Count.ToString() + ", Students Found</b></span>";
                    this.StudCountString.InnerHtml = _Countstring;
                   
                }
                else
                {
                    Grd_ScrechStud.DataSource = null;
                    Grd_ScrechStud.DataBind();
                    BtnSch2.Enabled = false;
                    Pnl_DataArea.Enabled = false;
                    Pnl_Studscreacharea.Visible = false;
                    string _Countstring = "<img src=\"Pics/user1.png\" width=\"25px\" height=\"22px\" /> <span style=\"color:#FF9900\"><b>0, Students Found</b></span>";
                    this.StudCountString.InnerHtml = _Countstring;
                }
            
        }
        private void AddStudtypeTodrp()
        {
            Drp_StudType.Items.Clear();
            string sql = "select tbladmisiontype.Id, tbladmisiontype.Name from tbladmisiontype";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Any", "0");
                Drp_StudType.Items.Add(li);
                while (MyReader.Read())
                {
                    
                    li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    Drp_StudType.Items.Add(li);
                }

            }
            else
            {
                ListItem li = new ListItem("No Types Found", "-1");

                Drp_StudType.Items.Add(li);


            }
            Drp_StudType.SelectedIndex = 0;
        }

       

        private void AddPeriodToNewDrp()
        {
            Drp_periodNew.Items.Clear();
            string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                    Drp_periodNew.Items.Add(li);
                }
                
            }
            else
            {
                ListItem li = new ListItem("No Periods Found", "-1");

                Drp_periodNew.Items.Add(li);
                

            }
            Drp_periodNew.SelectedIndex = 0;
        }

        private bool AddClassToNewDrp()
        {
            bool valid = false;
            Drp_className1.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_className1.Items.Add(li);

                }

                Drp_className1.Items.Add(new ListItem("All", "0"));
                valid = true;
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_className1.Items.Add(li);
                valid = false;
               
            }

            Drp_className1.SelectedIndex = 0;
            return valid;
        }
        //private void AddClassToDropDownClass()
        //{
        //    Drp_class2.Items.Clear();

        //    MydataSet = MyUser.MyAssociatedClass();
        //    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        //    {

        //        foreach (DataRow dr in MydataSet.Tables[0].Rows)
        //        {

        //            ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
        //            Drp_class2.Items.Add(li);

        //        }

               

        //    }
        //    else
        //    {
        //        ListItem li = new ListItem("No Class Present", "-1");
        //        Drp_class2.Items.Add(li);
               
        //    }

        //    Drp_class2.SelectedIndex = 0;

        //}
       
        //private void AddPeriodToDrp()
        //{

        //    Drp_Perod2.Items.Clear();
        //    string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
        //    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        //    if (MyReader.HasRows)
        //    {
        //        while (MyReader.Read())
        //        {

        //            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
        //            Drp_Perod2.Items.Add(li);
        //        }

        //    }
        //    else
        //    {
        //        ListItem li = new ListItem("No Periods Found", "-1");

        //        Drp_Perod2.Items.Add(li);


        //    }
        //    Drp_Perod2.SelectedIndex = 0;


        //}
        private void LoadDetails()
        {
            string sql = "SELECT tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_FeeName.Text = MyReader.GetValue(0).ToString();
                Lbl_Freq.Text = MyReader.GetValue(1).ToString();
                Lbl_asso.Text = MyReader.GetValue(2).ToString();

            }
            MyReader.Close();
        }
        //private void LoadStudentsToGrid()
        //{
        //    Lbl_note.Text = "";
        //    Grd_Amound.Columns[0].Visible = true;
        //    string sql = "select tblstudent.Id, tblstudent.StudentName,tblstudentclassmap.RollNo, tblstudent.Sex, tblstudtype.TypeName from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId inner join tblstudtype on tblstudtype.Id= tblstudent.StudTypeId where tblstudent.Status=1 And tblstudent.Id NOT IN (SELECT tblfeestudent.StudId from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=" + Session["FeeId"].ToString() + " AND tblfeeschedule.PeriodId=" + Drp_Perod2.SelectedValue + " AND tblfeeschedule.BatchId=" + MasterBatchId + ") AND tblstudentclassmap.ClassId=" + Drp_class2.SelectedValue + " Order by tblstudentclassmap.Standard ASC ,tblstudentclassmap.RollNo ASC ,tblstudent.StudentName ASC";
        //    MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        //    if (MydataSet.Tables[0].Rows.Count > 0)
        //    {
        //        Grd_Amound.DataSource = MydataSet;
        //        Grd_Amound.DataBind();
        //        Grd_Amound.Columns[0].Visible = false;
        //        Btn_Schdule1.Enabled = true;
        //        Pnl_AssStud.Visible = true;

        //        string _Countstring = "<img src=\"Pics/user1.png\" width=\"25px\" height=\"22px\" /> <span style=\"color:#FF9900\"><b>" + MydataSet.Tables[0].Rows.Count.ToString() + ", Students Found</b></span>";
        //        this.StudCountdiv.InnerHtml = _Countstring;
        //    }
        //    else
        //    {
        //        Grd_Amound.DataSource = null;
        //        Grd_Amound.DataBind();
        //        Btn_Schdule1.Enabled = false;
        //        Pnl_AssStud.Visible = false;
        //        string _Countstring = "<img src=\"Pics/user1.png\" width=\"25px\" height=\"22px\" /> <span style=\"color:#FF9900\"><b>0, Students Found</b></span>";
        //        this.StudCountdiv.InnerHtml = _Countstring;
        //    }

        //}
 
        //protected void Btn_Schdule1_Click(object sender, EventArgs e)
        //{

        //    int _flage = 0;
        //    string _message;
        //    if (ValidClassSchedule(out _message))
        //    {
        //        try
        //        {
        //            double _amount;
        //            int BatchId = MasterBatchId;
        //            int _feeid = int.Parse(Session["FeeId"].ToString());
        //            //DateTime _duedate = DateTime.Parse(Txt_DueStud.Text.ToString());
        //            //DateTime _lastdate = DateTime.Parse(Txt_LastStud.Text.ToString());
        //            DateTime _duedate = MyUser.GetDareFromText(Txt_DueStud.Text.ToString());
        //            DateTime _lastdate = MyUser.GetDareFromText(Txt_LastStud.Text.ToString());
        //            bool _continue = true;
        //            MyFeeMang.CreateTansationDb();
        //            MyFeeMang.ClearTempVariables();
        //            foreach (GridViewRow gv in Grd_Amound.Rows)
        //            {

        //                TextBox txtAmound = (TextBox)gv.FindControl("Txt_Amound");
        //                if (txtAmound.Text.Trim() != "" && double.TryParse(txtAmound.Text, out _amount))
        //                {
        //                    _flage = 1;
        //                    int StudentId = int.Parse(gv.Cells[0].Text.ToString());
        //                    int ClassId = int.Parse(Drp_class2.SelectedValue.ToString());
        //                    int PeriodId = int.Parse(Drp_Perod2.SelectedValue.ToString());
        //                    if (_continue)
        //                    {
        //                        _continue = MyFeeMang.ScheduleFeeToStudent(_feeid, StudentId, ClassId, PeriodId, BatchId, _duedate, _lastdate, _amount);

        //                    }
        //                }

        //            }
        //            if (_continue)
        //            {

        //                MyFeeMang.EndSucessTansationDb();
        //                if (_flage == 0)
        //                {
        //                    _message = "Please enter amount for schedule";
        //                }
        //                else
        //                {
        //                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Fee Schedule", "Rule Mapped for " + Lbl_FeeName.Text +" is Scheduled for Students in "+Drp_class2.SelectedItem.Text, 1);

        //                    LoadStudentsToGrid();
        //                   //GetStudentCount();
        //                    _message = "Fee Scheduled for the selected students";
        //                }


        //            }
        //            else
        //            {
        //                MyFeeMang.EndFailTansationDb();
        //                LoadStudentsToGrid();
        //                //GetStudentCount();
        //                _message = "Scheduled Failed, Please try again ";
        //            }

        //        }
        //        catch (Exception Ex)
        //        {
        //            MyFeeMang.EndFailTansationDb();
        //            LoadStudentsToGrid();
        //           // GetStudentCount();
        //            _message = Ex.Message;
        //        }

        //    }

        //    Lbl_note.Text = _message;



        //}

        //private bool ValidClassSchedule(out string _message)
        //{
        //    bool _valid = true;
        //    _message = "";
        //    if (Txt_DueStud.Text.Trim() == "" || Txt_LastStud.Text.Trim() == "")
        //    {
        //        _message = "One or more fields are Empty";
        //        _valid = false;

        //    }
        //    else
        //    {
        //        //DateTime _duedate = DateTime.Parse(Txt_DueStud.Text.ToString());
        //        //DateTime _lastdate = DateTime.Parse(Txt_LastStud.Text.ToString());
        //        DateTime _duedate = MyUser.GetDareFromText(Txt_DueStud.Text.ToString());
        //        DateTime _lastdate = MyUser.GetDareFromText(Txt_LastStud.Text.ToString());
        //        if (_lastdate < _duedate)
        //        {
        //            _message = "Last date cannot be less than due date";
        //            _valid = false;
        //        }
        //    }
        //    return _valid;
        //}

     

        protected void Btn_Cancel1_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("ManageFeeAccount.aspx");
        }

        //protected void Drp_class2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadStudentsToGrid();
        //   // GetStudentCount();

        //}

        protected void Drp_className1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }

        protected void Drp_Sex_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
           // GetFoundStudCountString();
        }

        protected void Drp_AdmType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
           // GetFoundStudCountString();
        }

        protected void Drp_StudType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }

        protected void Drp_periodNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }


        protected void Drp_Cast_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }

        protected void Btn_AddFee_Click(object sender, EventArgs e)
        {
            if (Txt_amount1.Text.Trim() != "")
            {
                foreach (GridViewRow gv in Grd_ScrechStud.Rows)
                {

                    TextBox TxtAmount = (TextBox)gv.FindControl("Txt_NewAmound");
                    TxtAmount.Text = Txt_amount1.Text;
                }
            }
            else
            {
                Lbl_Note1.Text = "Please enter the amount";
            }
        }

        protected void BtnSch2_Click(object sender, EventArgs e)
        {
            int _flage=0;
            string _message;
            if (ValidSchedule(out _message))
            {
                try
                {
                    double _amount;
                    int BatchId = MasterBatchId;
                    int _feeid = int.Parse(Session["FeeId"].ToString());
                    //DateTime _duedate = DateTime.Parse(Txt_NewDuetdt.Text.ToString());
                    //DateTime _lastdate = DateTime.Parse(Txt_NewLastdt.Text.ToString());
                    DateTime _duedate = MyUser.GetDareFromText(Txt_NewDuetdt.Text.ToString());
                    DateTime _lastdate = MyUser.GetDareFromText(Txt_NewLastdt.Text.ToString());
                    bool _continue = true;
                    MyFeeMang.CreateTansationDb();
                    MyFeeMang.ClearTempVariables();
                    foreach (GridViewRow gv in Grd_ScrechStud.Rows)
                    {

                        TextBox txtNewAmound = (TextBox)gv.FindControl("Txt_NewAmound");
                        if (txtNewAmound.Text.Trim() != "" && double.TryParse(txtNewAmound.Text, out _amount))
                        {
                            _flage = 1;
                            int StudentId = int.Parse(gv.Cells[0].Text.ToString());
                            int ClassId = int.Parse(gv.Cells[1].Text.ToString());
                            int PeriodId = int.Parse(Drp_periodNew.SelectedValue.ToString());
                            if (_continue)
                            {
                                _continue = MyFeeMang.ScheduleFeeToStudent(_feeid, StudentId, ClassId, PeriodId, BatchId, _duedate, _lastdate, _amount);
     
                            }
                        }

                    }
                    if (_continue)
                    {
                        MyFeeMang.EndSucessTansationDb();
                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Student Fee Scheduling", Lbl_FeeName.Text + " is Scheduled to " + Drp_className1.SelectedItem.Text, 1,1);

                        if (_flage == 0)
                        {
                            _message = "Please enter amount for schedule";
                        }
                        else
                        {
                            LoadStudentListToGrid();
                           // GetFoundStudCountString();
                            _message = "Fee Scheduled for the selected students";
                        }


                    }
                    else
                    {
                        MyFeeMang.EndFailTansationDb();
                        LoadStudentListToGrid();
                        //GetFoundStudCountString();
                        _message = "Scheduled Failed, Please try again ";
                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Student Fee Scheduling", Lbl_FeeName.Text + " Scheduling is failed for " + Drp_className1.SelectedItem.Text, 1,1);

                    }

                }
                catch (Exception Ex)
                {
                    MyFeeMang.EndFailTansationDb();
                    LoadStudentListToGrid();
                   // GetFoundStudCountString();
                    _message = Ex.Message;
                }

            }
            
            Lbl_Note1.Text = _message;
            
        }

        private bool ValidSchedule(out string _message)
        {
            bool _valid=true;
            _message = "";
            if (Txt_NewDuetdt.Text.Trim() == "" || Txt_NewLastdt.Text.Trim() == "")
            {
                _message = "One or more fields are Empty";
                _valid = false;

            }
            else
            {
                //DateTime _duedate = DateTime.Parse(Txt_NewDuetdt.Text.ToString());
                //DateTime _lastdate = DateTime.Parse(Txt_NewLastdt.Text.ToString());
                DateTime _duedate = MyUser.GetDareFromText(Txt_NewDuetdt.Text.ToString());
                DateTime _lastdate = MyUser.GetDareFromText(Txt_NewLastdt.Text.ToString());
                if (_lastdate < _duedate)
                {
                    _message = "Last date cannot be less than due date";
                    _valid = false;
                }
            }
            return _valid;
        }

        protected void BtnCnsl2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFeeAccount.aspx");
        }

        //protected void Drp_Perod2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadStudentsToGrid();
        //    //GetStudentCount();
        //}

        //protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadStudentsToGrid();
        //   // GetStudentCount();

        //}

        protected void Rdo_Batch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentListToGrid();
            //GetFoundStudCountString();
        }
    }
}
