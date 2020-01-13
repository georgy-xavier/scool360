using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Text;
using System.Data.Odbc;
using System.Net.Mail;
using MigraDoc.DocumentObjectModel;
using System.IO;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace WinEr
{
    public partial class CCE_ConfigManager : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private SchoolClass objSchool = null;
        private SMSManager MysmsMang;
        private OdbcDataReader MyReader = null;
        private EmailManager Obj_Email;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            MysmsMang = MyUser.GetSMSMngObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                    //some initlization
                    MysmsMang.InitClass();
                    LoadTermDrpdown();
                    LoadGrid();
                    Err.Visible = false;

                }
            }
        }

        /// <summary>
        /// this function loading with current schedule exam show on the grid.
        /// it will show class wise
        /// what are the exam finished .it will show based on the selected term .
        /// it will check with exam status .
        /// </summary>
        
        private void LoadGrid()
        {
            Grd_exam.DataSource = null;
            Grd_exam.DataBind();

            int _termid = 0;
            int.TryParse(Drp_term.SelectedValue.ToString(), out _termid);
            if (_termid > 0)
            {
                try
                {
                    string sql = "SELECT tblcce_classgroupmap.GroupId as GroupId,tblclass.Id as Id,tblclass.ClassName as Classname from tblclass INNER join tblcce_classgroupmap on tblcce_classgroupmap.ClassId=tblclass.Id  where tblclass.Status=1 and tblcce_classgroupmap.Termid=" + _termid;
                    DataSet ds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Label1.Visible = false;
                        Grd_exam.Columns[0].Visible = true;
                        Grd_exam.Columns[1].Visible = true;
                        Grd_exam.DataSource = ds;
                        Grd_exam.DataBind();
                        Grd_exam.Columns[0].Visible = false;
                        Grd_exam.Columns[1].Visible = false;
                        div1.Visible = true;


                        int _groupid = 0, _classid = 0;
                        int ReportGeneratestatus = -1, Publishstatus = -1, smsstatus = -1, emilstatus = -1;
                        foreach (GridViewRow row in Grd_exam.Rows)
                        {
                            int.TryParse(row.Cells[0].Text.ToString(), out _groupid);
                            int.TryParse(row.Cells[1].Text.ToString(), out _classid);

                            Button Lnk_gr = (Button)row.Cells[3].FindControl("Btn_gr");
                            Button Lnk_publish = (Button)row.Cells[4].FindControl("Btn_publish");
                            Button Lnk_sms = (Button)row.Cells[5].FindControl("Btn_sms");
                            Button Lnk_email = (Button)row.Cells[6].FindControl("Btn_email");

                            #region configuration

                            sql = "SELECT tblcce_configmanager.reportsstatus,tblcce_configmanager.publishstatus,tblcce_configmanager.smsstatus,tblcce_configmanager.emilstatus from tblcce_configmanager where tblcce_configmanager.GroupId=" + _groupid + " AND tblcce_configmanager.Classid=" + _classid + " AND tblcce_configmanager.Termid=" + _termid;
                            DataSet Ds_config = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                            if (Ds_config.Tables[0].Rows.Count > 0)
                                foreach (DataRow dr in Ds_config.Tables[0].Rows)
                                {
                                    int.TryParse(dr[0].ToString(), out ReportGeneratestatus);
                                    int.TryParse(dr[1].ToString(), out Publishstatus);
                                    int.TryParse(dr[2].ToString(), out smsstatus);
                                    int.TryParse(dr[3].ToString(), out emilstatus);

                                    #region generate report
                                    Lnk_publish.Enabled = false;
                                    Lnk_sms.Enabled = false;
                                    Lnk_email.Enabled = false;
                                    if (ReportGeneratestatus == 1)
                                    {
                                        Lnk_gr.Text = "Wating for Generate";
                                        Lnk_gr.CssClass = "yello";
                                    }
                                    else if (ReportGeneratestatus == 2)
                                    {
                                        Lnk_gr.Text = "Generated";
                                        Lnk_gr.CssClass = "green";
                                        Lnk_publish.Enabled = true;
                                    }

                                    #endregion

                                    #region publish
                                    if (Publishstatus == 1)
                                    {
                                        Lnk_publish.Text = "Published";
                                        Lnk_publish.CssClass = "green";
                                        Lnk_sms.Enabled = true;
                                        Lnk_email.Enabled = true;

                                        if (smsstatus == 0)
                                            Lnk_sms.CssClass = "red";
                                        else
                                            Lnk_sms.CssClass = "green";

                                        if (emilstatus == 0)
                                            Lnk_email.CssClass = "red";
                                        else
                                            Lnk_email.CssClass = "green";
                                    }
                                    else
                                    {
                                        Lnk_publish.Text = "Publish";
                                        Lnk_sms.Enabled = false;
                                        Lnk_email.Enabled = false;
                                    }
                                    #endregion
                                }
                            else
                            {
                                Lnk_publish.Enabled = false;
                                Lnk_sms.Enabled = false;
                                Lnk_email.Enabled = false;
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        Label1.Text = "Selected term exams is not scheduled! ";
                        Label1.Visible = true;
                        div1.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    WC_MessageBox.ShowMssage(ex.Message);
                } 
            }
        }

        /// <summary>
        /// this function load with term drop down valus
        /// </summary>
        private void LoadTermDrpdown()
        {
            Drp_term.Items.Clear();
            string sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName FROM tblcce_term";
            DataSet Ds_term = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_term != null && Ds_term.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_term.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_term.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_term.Items.Add(li);
            }
        }

        protected void Drp_term_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Err.Visible = false;
            Btn_Staff_Update.Visible = false;
            grdResult.DataSource = null;
            LoadGrid();
        }

        /// <summary>
        /// check with schedule exam mark entered or not
        /// check with marks conversion done or not
        /// check with result published or not
        /// check with sms semd or not
        /// check with email send or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_courseRowCommand(object sender, GridViewCommandEventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            Session["groupid"] = "";
            Session["classid"] = "";
            Session["termid"] = "";
            //Session["publish"] = "";
            Label2.Text = "0";
            string Msg = "",sql = "";
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Button Lnk_gr = (Button)Grd_exam.Rows[index].Cells[3].FindControl("Btn_gr");
                Button Lnk_publish = (Button)Grd_exam.Rows[index].Cells[4].FindControl("Btn_publish");
                Button Lnk_sms = (Button)Grd_exam.Rows[index].Cells[5].FindControl("Btn_sms");
                Button Lnk_email = (Button)Grd_exam.Rows[index].Cells[6].FindControl("Btn_email"); 

                int _termid = 0, _groupid = 0, _classid = 0;
                int.TryParse(Drp_term.SelectedValue.ToString(),out _termid);
                int.TryParse(Grd_exam.Rows[index].Cells[0].Text.ToString(),out _groupid);
                int.TryParse(Grd_exam.Rows[index].Cells[1].Text.ToString(),out _classid);

                Session["groupid"] = _groupid.ToString();
                Session["classid"] = _classid.ToString();
                Session["termid"] = _termid.ToString();
                
                if (e.CommandName == "GenerateReport")
                {
                    #region generate report
                   
                    if (MarkEntryValidation(_groupid, _classid, _termid))
                        if (Lnk_gr.Text == "Generate")
                        {
                            sql = "insert into tblcce_configmanager (tblcce_configmanager.GroupId,tblcce_configmanager.Termid,tblcce_configmanager.Classid,tblcce_configmanager.reportsstatus,tblcce_configmanager.publishstatus) values (" + _groupid + "," + _termid + "," + _classid + ",1,0)";
                            MyUser.m_MysqlDb.ExecuteQuery(sql);
                            logger.LogToFile(" CCE config manager  ", Grd_exam.Rows[index].Cells[2].Text + " report generated ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", Grd_exam.Rows[index].Cells[2].Text + " report generated ", 1);
                            LoadGrid();
                        }
                        else if (Lnk_gr.Text == "Generated")
                        {
                            Label2.Text = "2";
                            logger.LogToFile(" CCE config manager  ", Grd_exam.Rows[index].Cells[2].Text + " report generated ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", Grd_exam.Rows[index].Cells[2].Text + " report re-generated ", 1);
                            Lbl_popupmsg.Text = "If you want to re-generate the report!.";
                            MPE_yesornoMessageBox.Show();
                        }
                   
                    #endregion
                }
                else if (e.CommandName == "Publish")
                {
                    #region publish
                    if (Lnk_publish.Text == "Publish")
                    {
                        sql = "update tblcce_configmanager set tblcce_configmanager.publishstatus=1 where tblcce_configmanager.GroupId=" + _groupid + " and tblcce_configmanager.Termid=" + _termid + " and tblcce_configmanager.Classid=" + _classid;
                        MyUser.m_MysqlDb.ExecuteQuery(sql);

                        logger.LogToFile(" CCE config manager  ", Grd_exam.Rows[index].Cells[2].Text + " result published ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", Grd_exam.Rows[index].Cells[2].Text + " result published ", 1);
                        LoadGrid();
                    }
                    else
                    {
                        if (Lnk_publish.Text == "Published")
                        {
                            logger.LogToFile(" CCE config manager  ", Grd_exam.Rows[index].Cells[2].Text + " result unpublished ", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName); Btn_no.Text = "NO";
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", Grd_exam.Rows[index].Cells[2].Text + " result Unpublished ", 1);
                            Label2.Text = "1";
                            Lbl_popupmsg.Text = "If you want to unpublish the selected class result!.";
                            MPE_yesornoMessageBox.Show();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region send sms and mail after publishing
                    if (!LoadDefaultDetails(out Msg, e.CommandName))
                    {
                        termdiv.Visible = true;
                        div1.Visible = true;
                        divemail.Visible = false;
                        Err.Visible = false;
                    }
                    else
                    {
                        Lbl_class.Text = Grd_exam.Rows[index].Cells[2].Text.ToString();
                        Lbl_term.Text = Drp_term.SelectedItem.Text.ToString();
                        divemail.Visible = true;
                        Session["eventobject"] = e.CommandName;
                        div1.Visible = false;
                        termdiv.Visible = false;
                        Err.Visible = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                logger.LogToFile("CCE config manager", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager","throws Error" + ex.Message, 1);
            }

            if (Msg != "")
                WC_MessageBox.ShowMssage(Msg);
        }

        /// <summary>
        /// sending mail and sms page desing defalut conrtol loading function
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="commandname"></param>
        /// <returns></returns>
        private bool LoadDefaultDetails(out string Msg, string commandname)
        {
            Msg = "";
            bool returnbool = true;
            try
            {
                string _columnqury = " tblsmsparentlist.PhoneNo as sourcevalue,tblsmsparentlist.Enabled as Enabled ";
                string innerqury = " left join tblsmsparentlist on tblsmsparentlist.Id=tblstudent.Id ";
                if (commandname == "sendmail")
                {
                    _columnqury = " tbl_emailparentlist.EmailId as sourcevalue,tbl_emailparentlist.Enabled as Enabled ";
                    innerqury = " left join tbl_emailparentlist on tbl_emailparentlist.Id=tblstudent.Id ";
                }
                string sql = "select tblstudent.Id as StudentId,tblstudent.StudentName as StudentName,tblstudent.GardianName as ParentName," + _columnqury + " from tblstudent left join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id" + innerqury + " where tblstudentclassmap.ClassId=" + int.Parse(Session["classid"].ToString()) + " AND tblstudentclassmap.BatchId ORDER by tblstudentclassmap.RollNo ASC";
                DataSet listds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (listds.Tables[0].Rows.Count > 0)
                {
                    Grid_stulist.Columns[1].Visible = true;
                    Grid_stulist.Columns[5].Visible = true;
                    Grid_stulist.Columns[6].Visible = true;
                    Grid_stulist.Columns[7].Visible = true;

                    Grid_stulist.DataSource = listds.Tables[0];
                    Grid_stulist.DataBind();

                    if (commandname == "sendmail")
                    {
                        Grid_stulist.Columns[6].Visible = false;
                        //Grid_stulist.Columns[4].HeaderText = "E-mail Address";
                        Btn_CheckConnection.Visible = false;
                    }
                    else
                    {
                        Grid_stulist.Columns[7].Visible = false;
                        //Grid_stulist.Columns[4].HeaderText = "Mobile No";
                        Btn_CheckConnection.Visible = true;
                    }

                    Grid_stulist.Columns[1].Visible = false;
                    Grid_stulist.Columns[5].Visible = false;

                    Grid_stulist.Visible = true;

                 
                    int _enable = 0;
                    foreach (GridViewRow dr in Grid_stulist.Rows)
                    {
                        int.TryParse(dr.Cells[5].Text.ToString(), out _enable);
                        if (_enable == 0)
                        {
                            dr.Enabled = false;
                            dr.BackColor = System.Drawing.Color.SandyBrown;
                        }
                    }
                  

                }
                else
                {
                    Grid_stulist.DataSource = null;
                    Grid_stulist.DataBind();
                    Grid_stulist.Visible = false;
                    returnbool = false;
                }
                
               
            }
            catch(Exception ex)
            {
                Msg = ex.Message;
                returnbool = false;
            }
            return returnbool;
        }

        #region remove

        private bool MarkEntryValidation(int _groupid, int _classid, int _termid)
        {
            #region no need
            //string Warring = "";
            //bool valid = false;
            //string sql = "SELECT tblcce_colconfig.ExamName,tblcce_colconfig.TableName,tblcce_colconfig.ColName  from tblcce_colconfig where tblcce_colconfig.GroupId =" + _groupid + " and tblcce_colconfig.TermId=" + _termid + " AND tblcce_colconfig.TableName='tblcce_mark'";
            //DataSet _resultds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            //Session["ColumnDataset"] = _resultds;
            //DataTable _ErrResultds = new DataTable();
            //_ErrResultds.Columns.Add("StudentId", typeof(string));
            //_ErrResultds.Columns.Add("SubjectId", typeof(string));
            //_ErrResultds.Columns.Add("StudentName", typeof(string));
            //_ErrResultds.Columns.Add("SubjectName", typeof(string));
            //string Columnname = "";
            //if (_resultds.Tables.Count > 0)
            //    if (_resultds.Tables[0].Rows.Count > 0 && _resultds != null)
            //    {
            //        int i = 0;
            //        foreach (DataRow dr_exam in _resultds.Tables[0].Rows)
            //        {
            //            _ErrResultds.Columns.Add(dr_exam["ExamName"].ToString(), typeof(string));
            //            Columnname += "," + dr_exam["TableName"].ToString() + "." + dr_exam["ColName"].ToString() + " as Mark" + i;
            //            i++;
            //        }
            //    }

            //string stu_sql = "SELECT tblstudentclassmap.StudentId,tblstudent.StudentName from tblstudentclassmap inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId where tblstudentclassmap.ClassId=" + _classid + " AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " ORDER by tblstudentclassmap.RollNo";
            //DataSet stu_ds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(stu_sql);

            //string sub_sql = "SELECT tblsubjects.Id,tblsubjects.subject_name from tblsubjects inner JOIN tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id where tblcce_classsubject.classid=" + _classid + " ORDER by tblcce_classsubject.SubjectOrder";
            //DataSet sub_ds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sub_sql);

            //int Stuid = 0, Subid = 0;
            //string Publish_sql = "";
            //if (stu_ds.Tables.Count > 0 && sub_ds.Tables.Count > 0)
            //{
            //    if (stu_ds.Tables[0].Rows.Count == 0 && stu_ds == null)
            //        Warring = "This class not have students!";
            //    else if (sub_ds.Tables[0].Rows.Count == 0 && sub_ds == null)
            //        Warring = "Subjects is not mapped from this class";
            //    else
            //    {
            //        foreach (DataRow stu_dr in stu_ds.Tables[0].Rows)
            //        {
            //            int.TryParse(stu_dr["StudentId"].ToString(), out Stuid);
            //            foreach (DataRow sub_dr in sub_ds.Tables[0].Rows)
            //            {
            //                int.TryParse(sub_dr["Id"].ToString(), out Subid);
            //                Publish_sql = "SELECT tblcce_mark.StudentId as t1,tblcce_mark.SubjectId as t2,(SELECT tblstudent.StudentName from tblstudent WHERE tblstudent.Id=t1) as StudentName,(SELECT tblsubjects.subject_name from tblsubjects WHERE tblsubjects.Id=t2) as SubjectName" + Columnname + " from tblcce_mark where tblcce_mark.StudentId=" + Stuid + " and tblcce_mark.SubjectId=" + Subid;
            //                DataSet Publish_ds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(Publish_sql);

            //                #region Error collection

            //                if (Publish_ds.Tables[0].Rows.Count > 0 && Publish_ds != null)
            //                {

            //                    foreach (DataRow pub_dr in Publish_ds.Tables[0].Rows)
            //                    {
            //                        bool empty = false;
            //                        DataRow Errrow = _ErrResultds.NewRow();
            //                        Errrow[0] = Stuid.ToString();
            //                        Errrow[1] = Subid.ToString();
            //                        Errrow[2] = stu_dr["StudentName"].ToString();
            //                        Errrow[3] = sub_dr["subject_name"].ToString();


            //                        for (int i = 4; i < _ErrResultds.Columns.Count; i++)
            //                        {
            //                            if (pub_dr[i].ToString() == "")
            //                                empty = true;

            //                            Errrow[i] = pub_dr[i].ToString();

            //                        }
            //                        if (empty)
            //                        {
            //                            _ErrResultds.Rows.Add(Errrow);
            //                            //Warring = "Student mark is not entered. You want to update student marks? Click 'YES'";
            //                            empty = false;
            //                        }
            //                    }

            //                }
            //                else
            //                    Warring = "This class students marks is not imported. Please import students marks.";

            //                #endregion

            //            }
            //        }
            //    }

            //}

            //#region Results

            //if (_ErrResultds.Rows.Count == 0 && Warring == "")
            //    valid = true;
            ////else if (_ErrResultds.Rows.Count == 0 && Warring != "")
            ////    WC_MessageBox.ShowMssage(Warring);
            //else
            //{
            //    Lbl_popupmsg.Text = Warring;
            //    Btn_no.Text = "Continue";
            //    Session["DataSet"] = _ErrResultds;
            //    MPE_yesornoMessageBox.Show();
            //}

            #endregion

            

            return true;
        }

        protected void TaskGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdResult.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void TaskGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdResult.EditIndex = -1;
            BindData();
            Btn_Staff_Update.Visible = false;
        }

        protected void TaskGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Btn_Staff_Update.Visible = true;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["DataSet"];
            GridViewRow row = grdResult.Rows[e.RowIndex];
            dt.Rows[row.DataItemIndex]["StudentId"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["SubjectId"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["StudentName"] = ((TextBox)(row.Cells[3].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["SubjectName"] = ((TextBox)(row.Cells[4].Controls[0])).Text;

            DataSet Column = (DataSet)Session["ColumnDataset"];
            int i = 5;
            foreach (DataRow dc in Column.Tables[0].Rows)
            {
                dt.Rows[row.DataItemIndex][dc["ExamName"].ToString()] = ((TextBox)(row.Cells[i].Controls[0])).Text;
                i++;
            }
            grdResult.EditIndex = -1;
            Session["DataSet"] = null;
            Session["DataSet"] = dt;
            BindData();

        }

        private void BindData()
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["DataSet"];
            if (ds.Rows.Count > 0)
            {
                grdResult.DataSource = ds;
                grdResult.DataBind();
                SetGridCellWidth();
            }
            else
            {
                Err.Visible = false;
                div1.Visible = true;
            }
        }

        private void SetGridCellWidth()
        {
            GridViewRow dr = grdResult.HeaderRow;
            int cellcount=dr.Cells.Count;
            double cellwidth=(100/(cellcount-2));
            int _cellwidth=int.Parse(Math.Round(cellwidth,0).ToString());

            for (int i = 0; i < dr.Cells.Count; i++)
            {
                if (i == 1 || i == 2)
                    dr.Cells[i].Visible = false;
                else
                {
                    if (dr.Cells.Count > 7)
                        dr.Cells[i].Width = System.Web.UI.WebControls.Unit.Pixel(_cellwidth);
                }
            }

            foreach (GridViewRow row in grdResult.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == 1 || i == 2)
                        row.Cells[i].Visible = false;
                    else
                    {
                        if (row.Cells.Count > 7)
                            row.Cells[i].Width = System.Web.UI.WebControls.Unit.Pixel(_cellwidth);
                    }
                }
            }
               
        }

        protected void Btn_back_Click(object sender, EventArgs e)
        {
            div1.Visible = true;
            Err.Visible = false;
        }

        #endregion

        #region yesorno popup

        protected void Btn_yes_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (Label2.Text=="1")
            {
                sql = "update tblcce_configmanager set tblcce_configmanager.publishstatus=0 where tblcce_configmanager.GroupId=" + int.Parse(Session["groupid"].ToString()) + " and tblcce_configmanager.Termid=" + int.Parse(Drp_term.SelectedValue.ToString()) + " and tblcce_configmanager.Classid=" + int.Parse(Session["classid"].ToString());
                MyUser.m_MysqlDb.ExecuteQuery(sql);
                LoadGrid();
            }
            else if (Label2.Text == "2")
            {
                sql = "update tblcce_configmanager set tblcce_configmanager.reportsstatus=1,tblcce_configmanager.publishstatus=0 where tblcce_configmanager.GroupId=" + int.Parse(Session["groupid"].ToString()) + " and tblcce_configmanager.Termid=" + int.Parse(Drp_term.SelectedValue.ToString()) + " and tblcce_configmanager.Classid=" + int.Parse(Session["classid"].ToString());
                MyUser.m_MysqlDb.ExecuteQuery(sql);
                LoadGrid();
            }
            else
            {
                div1.Visible = false;
                Err.Visible = true;
                DataTable _ErrResultds = (DataTable)Session["DataSet"];
                grdResult.DataSource = _ErrResultds;
                grdResult.DataBind();
                SetGridCellWidth();
            }

        }

        protected void Btn_no_Click(object sender, EventArgs e)
        {
            if (Label2.Text == "1" || Label2.Text == "2")
                MPE_yesornoMessageBox.Dispose();
            else
            {
                string sql = "";
                int _groupid = 0, _termid = 0, _classid = 0;
                int.TryParse(Session["groupid"].ToString(), out _groupid);
                int.TryParse(Session["termid"].ToString(), out _termid);
                int.TryParse(Session["classid"].ToString(), out _classid);
                sql = "update tblcce_configmanager set tblcce_configmanager.publishstatus=0 where tblcce_configmanager.GroupId=" +_groupid+ " and tblcce_configmanager.Termid=" +_termid+ " and tblcce_configmanager.Classid=" +_classid;
                MyUser.m_MysqlDb.ExecuteQuery(sql);
                LoadGrid();
            }
        }

        #endregion

        protected void Btn_Staff_Update_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet Column = (DataSet)Session["ColumnDataset"];
               

                string sql = "";
                DataTable dt = (DataTable)Session["DataSet"];
                foreach (GridViewRow dr in grdResult.Rows)
                {
                    string _columns = "";
                    string value="";
                    bool valiid = true;
                    for (int j = 5; j < dr.Cells.Count; j++)
                    {
                        if (dr.Cells[j].Text.ToString() == "" || dr.Cells[j].Text.ToString() == "&nbsp;")
                            valiid = false;
                    }

                    if (valiid)
                    {
                        int i = 5;
                        foreach (DataRow dc in Column.Tables[0].Rows)
                        {
                            if (dr.Cells[i].Text.ToString() != "" && dr.Cells[i].Text.ToString() != "&nbsp;")
                            {
                                value = dr.Cells[i].Text.ToString();
                                if (_columns == "")
                                    _columns = dc["TableName"].ToString() + "." + dc["ColName"].ToString() + "=" + value;
                                else
                                    _columns += "," + dc["TableName"].ToString() + "." + dc["ColName"].ToString() + "=" + value;
                            }
                            i++;
                        }
                        if (_columns != "")
                        {
                            sql = "update tblcce_mark set " + _columns + " where tblcce_mark.StudentId=" + int.Parse(dr.Cells[1].Text.ToString()) + " and tblcce_mark.SubjectId=" + int.Parse(dr.Cells[2].Text.ToString());
                            MyUser.m_MysqlDb.ExecuteQuery(sql);
                            foreach (DataRow m_dr in dt.Rows)
                            {
                                if (m_dr[0].ToString() == dr.Cells[1].Text.ToString() && m_dr[1].ToString() == dr.Cells[2].Text.ToString())
                                {
                                    dt.Rows.Remove(m_dr);
                                    break;
                                }
                            }
                        }
                    }
                }

                Session["DataSet"] = dt;
                BindData();
                Btn_Staff_Update.Visible = false;
            }
            catch (Exception ex)
            {
                WC_MessageBox.ShowMssage(ex.Message);
            }
        }

        /// <summary>
        /// this function sending email and sms particular student
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
          
            if (Session["eventobject"].ToString() == "sendmail")
            {
                #region sending Email
                if (!HaveModule(31))
                    WC_MessageBox.ShowMssage("Please activate EMAIL module!");
                else
                {
                    string EmailAddress = "", EmailSubject = "Exam Results", EmailBody = "", _Markhtmlstr = "", attach = "";
                    string SenderId = "";
                    EmailAddress = Grid_stulist.Rows[e.NewEditIndex].Cells[4].Text.ToString();
                    SenderId = Grid_stulist.Rows[e.NewEditIndex].Cells[1].Text.ToString();
                    Session["StudId"] = SenderId;

                    #region HTML mail design
                    EmailBody += "<div><br />&nbsp;Dear <span style=\"font-weight: bold;\">" + Grid_stulist.Rows[e.NewEditIndex].Cells[2].Text.ToString() + ".,</span><table style=\"width: 100%;\"><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;your childs result has been announced & marks taken by your child are as follows. Find the attachement for the report card.<br /></td> </tr>";
                    EmailBody += "</table><table style=\"width: 100%; height: 92px;\"><tr><td><span style=\"font-size:medium;font-weight: bold;\">Thanks & Regards</span><br/>" + objSchool.SchoolName.ToString()+ "</td></tr></table></div>";
                    #endregion

                    if (Getstudentprogressreport(out attach, out _Markhtmlstr))//Taking student exam mark 
                    {
                        string sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId,Attachment1,Attachment2,Attachment3) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "', 2 ,0,'" + SenderId + "','" + attach + "',' ',' ')";
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        logger.LogToFile("CCE config manager", "Email message has been sent to " + EmailAddress + "", 'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", "Email message has been sent to " + EmailAddress + "", 1);
                        WC_MessageBox.ShowMssage("Your message has been sent.");
                       
                    }
                    else
                        WC_MessageBox.ShowMssage("Please try later...........");

                   
                   
                }
                #endregion
            }
            else
            {
                #region sending sms
                string subjectStr = "", StudentName = "", ParentName = "", ExamName = "", PhoneNumber = "";
                bool _continue = true;
                if (!HaveModule(23))
                    WC_MessageBox.ShowMssage("Please activate SMS module!");
                else
                {
                    StudentName = Grid_stulist.Rows[e.NewEditIndex].Cells[2].Text.ToString();
                    ParentName = Grid_stulist.Rows[e.NewEditIndex].Cells[3].Text.ToString();
                    PhoneNumber = Grid_stulist.Rows[e.NewEditIndex].Cells[4].Text.ToString();
                    ExamName = Drp_term.SelectedItem.Text.ToString();
                    Session["StudId"] = Grid_stulist.Rows[e.NewEditIndex].Cells[1].Text.ToString();
                    GetStudentExamPerformanace(out _continue, out subjectStr);

                    if (_continue)
                    {
                        if (!sendsms(subjectStr, StudentName, ParentName, ExamName, PhoneNumber))
                            WC_MessageBox.ShowMssage("Please try Later.................");
                        else
                            WC_MessageBox.ShowMssage("Your message has been sent.");

                    }
                    else
                        WC_MessageBox.ShowMssage("Please try Later.................");

                   
                }
                #endregion
            }

        }
        /// <summary>
        /// checking givien module active 
        /// </summary>
        /// <param name="_moduleid"></param>
        /// <returns></returns>
        public bool HaveModule(int _moduleid)
        {
            bool _valide=false;
            string sql = "select Id from tblmodule where Id=" + _moduleid + " and IsActive=1";
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count>0)
                _valide = true;
            return _valide;
        }

        /// <summary>
        /// after clicking this event control will come to previous page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            div1.Visible = true;
            termdiv.Visible = true;
            divemail.Visible = false;
            Err.Visible = false;
        }

        /// <summary>
        /// sending sms and maill for all selected student
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            if (Session["eventobject"].ToString() == "sendmail")
            {
                #region sending email
                if (!HaveModule(31))
                    WC_MessageBox.ShowMssage("Please activate EMAIL module!");
                else
                {
                    string EmailAddress = "", EmailSubject = "Exam Results", EmailBody = "",_Markhtmlstr="", attach = "";
                    bool _continue = true;
                    string SenderId = "";

                    foreach (GridViewRow dr in Grid_stulist.Rows)
                    {
                        if (dr.Enabled)
                        {
                            EmailAddress = dr.Cells[4].Text.ToString();
                            SenderId = dr.Cells[1].Text.ToString();
                            Session["StudId"] = dr.Cells[1].Text.ToString();

                            #region HTML design
                            EmailBody += "<div><br />&nbsp;Dear <span style=\"font-weight: bold;\">" + dr.Cells[2].Text.ToString() + ".,</span><table style=\"width: 100%;\"><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;your childs result has been announced & marks taken by your child are as follows. Find the attachement for the report card.<br /></td> </tr>";
                            EmailBody += _Markhtmlstr;
                            EmailBody += "</table><table style=\"width: 100%; height: 92px;\"><tr><td><span style=\"font-size:medium;font-weight: bold;\">Thanks & Regards</span><br/>" + objSchool.SchoolName.ToString() + "</td></tr></table></div>";
                            #endregion
                            _continue =true;
                            if (Getstudentprogressreport(out attach, out _Markhtmlstr))
                            {
                                string sql = "Insert into tbl_autoemail(EmailAddress,EmailSubject,EmailBody,TimeTosend,`Type`,`Status`,SenderId,Attachment1,Attachment2,Attachment3) values('" + EmailAddress + "','" + EmailSubject + "','" + EmailBody + "','" + System.DateTime.Now.Date.ToString("s") + "', 2 ,0,'" + SenderId + "','" + attach + "',' ',' ')";
                                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                            else
                            {
                                _continue = false;
                                WC_MessageBox.ShowMssage("Please try later...........");
                                break;
                            }
                        }

                            
                    }
                    if (_continue)
                    {
                        string sql = "update tblcce_configmanager set tblcce_configmanager.emilstatus=1 where tblcce_configmanager.Classid=" + int.Parse(Session["classid"].ToString()) + " and tblcce_configmanager.Termid=" + int.Parse(Session["termid"].ToString()) + " and tblcce_configmanager.GroupId=" + int.Parse(Session["groupid"].ToString());
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        logger.LogToFile("CCE config manager", "Your message has been sent to "+EmailAddress+"",'I', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE config manager", "Your message has been sent to " + EmailAddress + "", 1);
                        WC_MessageBox.ShowMssage("Your message has been sent.");
                    }
                  


                }
                #endregion
            }
            else
            {
                #region sending sms
                string StudentName = "", ParentName = "", PhoneNumber = "", ExamName = "";
                if (!HaveModule(23))
                    WC_MessageBox.ShowMssage("Please activate SMS module!");
                else
                {
                    bool _continue = false;
                    string subjectStr = "";
                    foreach (GridViewRow dr in Grid_stulist.Rows)
                    {
                        if (dr.Enabled)
                        {
                            StudentName = dr.Cells[2].Text.ToString();
                            ParentName = dr.Cells[3].Text.ToString();
                            PhoneNumber = dr.Cells[4].Text.ToString();
                            ExamName = Drp_term.SelectedItem.Text.ToString();
                            Session["StudId"] = dr.Cells[1].Text.ToString();
                            GetStudentExamPerformanace(out _continue, out subjectStr);
                            _continue = true;

                            if (_continue)
                                if (!sendsms(subjectStr, StudentName, ParentName, ExamName, PhoneNumber))
                                {
                                    _continue = false;
                                    break;
                                }
                        }
                        
                    }
                    if (!_continue)
                        WC_MessageBox.ShowMssage("Please try Later............!");
                    else
                    {
                        string sql = "update tblcce_configmanager set tblcce_configmanager.emilstatus=1 where tblcce_configmanager.Classid=" + int.Parse(Session["classid"].ToString()) + " and tblcce_configmanager.Termid=" + int.Parse(Session["termid"].ToString()) + " and tblcce_configmanager.GroupId=" + int.Parse(Session["groupid"].ToString());
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        WC_MessageBox.ShowMssage("Your message has been sent.");
                    }

                }
                #endregion
            }
        }

        /// <summary>
        /// sending sms
        /// </summary>
        /// <param name="subjectStr"></param>
        /// <param name="StudentName"></param>
        /// <param name="ParentName"></param>
        /// <param name="ExamName"></param>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        private bool sendsms(string subjectStr,string StudentName,string ParentName,string ExamName,string PhoneNumber)
        {
            bool _continue = true;
            string sql = "", Message = "", Result = "", Grade = "",_subjeccodemark="",_subjectnamemark="";

            #region create sms format
            SMSManager mysms = MyUser.GetSMSMngObj();

            string _ShortName = "Onexamreport";//taking sms format configuration . format type
            string[] _remark = subjectStr.Split('$');
            _subjeccodemark = _remark[0];
            _subjectnamemark = _remark[1];

            string _FeeDueResult_WithheldSubvalue = "";
            string Need_FeeDueResult_Withheld = MyExamMang.GetConfigValue("FeeDueResult_Withheld", "Exam Report", out _FeeDueResult_WithheldSubvalue);
            if (Need_FeeDueResult_Withheld.Trim() == "1")
            {
                if (_FeeDueResult_WithheldSubvalue.ToLowerInvariant().Trim() == Result.ToLowerInvariant().Trim())
                    _ShortName = "OnexamreportWithheld";

            }

            sql = "SELECT `Format` FROM tblsmsoptionconfig WHERE `Enable`=1 AND ShortName='" + _ShortName + "'";
            MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
                Message = MyReader.GetValue(0).ToString();
            else
                _continue = false; ;

            if (_continue)
            {
                Message = Message.Replace("($Student$)", StudentName);
                Message = Message.Replace("($Parent$)", ParentName);
                Message = Message.Replace("($Exam$)", ExamName);
                Message = Message.Replace("($S_M$)", _subjeccodemark);
                Message = Message.Replace("($Grade$)", Grade);
                Message = Message.Replace("($Result$)", Result);
                Message = Message.Replace("($SN_M$)", _subjectnamemark);
            }

            if (_continue && PhoneNumber != "" && Message != "")
            {
                string failedList = "";
                if (mysms.SendBULKSms(PhoneNumber, Message, "90366450445", "WINER", true, out  failedList))
                {
                    _continue = true;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam SMS Report", "Message : " + Message, 1);
                }
                else
                    _continue =false;

            }

            #endregion

            return _continue;
        }

        #region email module

        /// <summary>
        /// this function return progress report card PDF path and student marks string 
        /// </summary>
        /// <param name="_Attachmentfilepath"></param>
        /// <param name="_Markhtmlstr"></param>
        /// <returns></returns>
        private bool Getstudentprogressreport(out string _Attachmentfilepath, out string _Markhtmlstr)
        {
            _Attachmentfilepath = "";
            _Markhtmlstr = "";
            bool _continue = true;
            try
            {

                #region reports

                string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\PDF_Files\\";//Path for PDF creation
                string _DefaultImgpath = Server.MapPath("") + "\\Pics";
                string _TempImgpath = Server.MapPath("") + "\\ThumbnailImages";
                
                Document PDFdocument = new Document();
                PageSetup pgSetup = PDFdocument.DefaultPageSetup;

                int _classid = 0;
                int.TryParse(Session["classid"].ToString(), out _classid);
                string xmlstr = ReadXMLfileFromDataBase(_classid);

                if (xmlstr == "")
                    _continue = false;
                if (_continue)
                    GetStudentExamPerformanace(out _continue, out _Markhtmlstr);
                if (_continue)
                {
                    Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();
                    Dataset_Dic = (Dictionary<string, DataSet>)Session["DataSetDictionary"];
                    //CCEUtility this class fille will be collect all student information link a attendance
                    CCEUtility objCCE = new CCEUtility(MyUser, objSchool, Dataset_Dic, int.Parse(Session["StudId"].ToString()));
                    objCCE.m_PdfPysicalPath = _PhysicalPath;
                    objCCE.BatchId = MyUser.CurrentBatchId;
                    objCCE.BatchName = MyUser.CurrentBatchName;
                    objCCE.ClassId = _classid;
                    objCCE.ClassName = Lbl_class.Text.ToString();
                    int.TryParse(Drp_term.SelectedValue.ToString(), out objCCE.TermId);
                    objCCE.TermName = Drp_term.SelectedItem.Text.ToString();
                    objCCE.m_DefaultImgpath = _DefaultImgpath;
                    objCCE.m_TempImgpath = _TempImgpath;

                    string _err = "";
                    objCCE.xmlstring = xmlstr;
                    if (!objCCE.Exporttopdfdocument(ref PDFdocument, pgSetup, out _err))//creating PDFdocument and Page setup
                        _continue = false;
                }
                #endregion

                #region create Pdf
                
                if (_continue)// if continue while taking mark error accor means pdf creating process will be break 
                {
                    string PdfPath = _PhysicalPath + Drp_term.SelectedItem.Text.ToString() + Session["StudId"].ToString() + ".pdf";

                    if (File.Exists(PdfPath))
                        File.Delete(PdfPath);

                    const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
                    const bool unicode = false;
                    PdfDocumentRenderer pdfrenderer = new PdfDocumentRenderer(unicode, embedding);
                    pdfrenderer.Document = PDFdocument;
                    pdfrenderer.RenderDocument();
                    pdfrenderer.PdfDocument.Save(PdfPath);
                    _Attachmentfilepath = PdfPath;
                }
               
                #endregion

                #region delete all temp files 

                string[] filePaths = Directory.GetFiles(_TempImgpath);
                foreach (string filePath in filePaths)
                    File.Delete(filePath);

                #endregion

            }
            catch 
            {
                _continue = false;
            }
            return _continue;

        }

        /// <summary>
        /// this function collecting student information and parts subject skills marks 
        /// saving from session
        /// </summary>
        /// <param name="_continue"></param>
        /// <param name="_subjectStr"></param>
        private void GetStudentExamPerformanace(out bool _continue, out string _subjectStr)
        {
            _subjectStr = " ";//taking subject name with mark
            _continue = false;
            try
            {
                Dictionary<string, DataSet> Dataset_Dic = new Dictionary<string, DataSet>();

                bool publish = true;
                bool result = true;
                string quryStr = "", _subjectcodemark = "", _subjectnamemark="";

                int StudentId = int.Parse(Session["StudId"].ToString());
                string sql = "";

                #region Marksubject Result

                if ((Drp_term.SelectedItem.Text.ToString()).ToLower() == "consolidate")//this query will taking schedule exam 
                    sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblcce_colconfig.TableName='tblcce_result' AND tblstudentclassmap.StudentId=" + StudentId;
                else
                    sql = "SELECT DISTINCT(tblcce_colconfig.ExamName),tblcce_colconfig.TableName,tblcce_colconfig.ColName from tblcce_colconfig INNER JOIN tblcce_classgroupmap on tblcce_classgroupmap.GroupId=tblcce_colconfig.GroupId inner join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classgroupmap.ClassId where tblcce_colconfig.TableName='tblcce_result' AND tblstudentclassmap.StudentId=" + StudentId + " and tblcce_colconfig.TermId=" + int.Parse(Drp_term.SelectedValue.ToString());

                DataSet Mark_Ds =MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (Mark_Ds.Tables[0].Rows.Count == 0)
                    publish = false;
                else
                {
                    foreach (DataRow C_dr in Mark_Ds.Tables[0].Rows)
                    {
                        string replacestr = C_dr[0].ToString();
                        quryStr = quryStr + "," + C_dr[1].ToString() + "." + C_dr[2].ToString() + " as " + "`" + replacestr + "`";
                    }
                    Mark_Ds = null;
                    sql = "SELECT tblcce_classsubject.subjectid as SINo,tblsubjects.subject_name as SUBJECT,tblsubjects.SubjectCode  " + quryStr + " from tblsubjects right join tblcce_classsubject on tblcce_classsubject.subjectid=tblsubjects.Id right join tblstudentclassmap on tblstudentclassmap.ClassId=tblcce_classsubject.classid right join tblcce_result on tblcce_result.StudentId=tblstudentclassmap.StudentId AND tblcce_result.SubjectId=tblcce_classsubject.subjectid where tblstudentclassmap.StudentId=" + StudentId + "  ORDER BY tblcce_classsubject.SubjectOrder";
                    Mark_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Mark_Ds.Tables[0].Rows.Count == 0)
                        publish = false;
                    else
                    {
                        foreach (DataRow M_dr in Mark_Ds.Tables[0].Rows)
                        {
                            for (int i = 3; i < Mark_Ds.Tables[0].Columns.Count; i++)
                            {
                                if (M_dr[i].ToString() == "-" || M_dr[i].ToString() == "AB" || M_dr[i].ToString() == "WITHHELD")
                                    result = false;
                            }
                            int columncount = Mark_Ds.Tables[0].Columns.Count;
                            _subjectcodemark = _subjectcodemark + " " + M_dr[2].ToString() + ":" + M_dr[columncount - 1].ToString();
                            _subjectnamemark = _subjectnamemark + " " + M_dr[1].ToString() + ":" + M_dr[columncount - 1].ToString();
                        }

                        _subjectStr += _subjectcodemark;
                        _subjectStr +="$"+ _subjectnamemark;
                    }

                    if (result == true && publish == true)
                    {
                        Dataset_Dic.Add("@@AcademicPerformance@@", SetDefaultColumnname(Mark_Ds));
                    }

                }
                #endregion

                #region Taking Discriptive mark

                if (publish == true && result == true)
                {
                    if (LoadDiscriptiveGreadereports(ref Dataset_Dic))
                        Session["DataSetDictionary"] = Dataset_Dic;
                    else
                        Session["DataSetDictionary"] = Dataset_Dic;


                    _continue = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                _continue = false;
            }

        }

        /// <summary>
        /// this function return xml string
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        private string ReadXMLfileFromDataBase(int classid)
        {
            string xmstring = "";
            string sql = "SELECT tblcce_classgroup.TermXMLFile,tblcce_classgroup.ConsoldateXMLFile from tblcce_classgroup inner join tblcce_classgroupmap ON tblcce_classgroupmap.GroupId=tblcce_classgroup.Id where tblcce_classgroupmap.ClassId=" + classid + " and tblcce_classgroupmap.Termid="+int.Parse(Drp_term.SelectedValue.ToString());
            DataSet Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds != null || Ds.Tables[0] != null || Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    if ((Drp_term.SelectedItem.Text.ToString()).ToLower() == "consolidate")
                        xmstring = dr[1].ToString();
                    else
                        xmstring = dr[0].ToString();
                    
                }
            }
            return xmstring;
        }

        /// <summary>
        /// converting default columns after reading marks this dataset convert column name like a column1,column2,column3.............
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private DataSet SetDefaultColumnname(DataSet result)//set column name Like a column1 column2....
        {
            int k = 1;
            for (int j = 0; j < result.Tables[0].Columns.Count; j++)
            {
                if (j == 0)
                    result.Tables[0].Columns[j].ColumnName = "SINo";
                else
                {
                    result.Tables[0].Columns[j].ColumnName = "Column" + k;
                    k++;
                }
            }
            return result;
        }

        /// <summary>
        /// collecting all discriptive marks 
        /// </summary>
        /// <param name="Dataset_Dic"></param>
        /// <returns></returns>
        private bool LoadDiscriptiveGreadereports(ref Dictionary<string, DataSet> Dataset_Dic)
        {
            bool valid = false;

            string Tablename = "tblcce_descriptive", Termcolumn = "";
            int Classid = int.Parse(Session["classid"].ToString());
            int Termid = 0;
            int.TryParse(Drp_term.SelectedValue.ToString(), out Termid);
            string Termname = Drp_term.SelectedItem.Text.ToString();
            int Batchid = MyUser.CurrentBatchId;

            string sql = "SELECT tblcce_parts.Id,tblcce_parts.Description,tblcce_parts.FooterDesc from tblcce_parts";
            DataSet Ds_Part = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds_Part.Tables[0].Rows.Count == 0)
                valid = false;
            else
            {
                foreach (DataRow drpart in Ds_Part.Tables[0].Rows)
                {
                    sql = "SELECT DISTINCT tblsubjects.Id,tblsubjects.subject_name from tblsubjects INNER JOIN tblcce_subjectskillmap  on tblsubjects.Id=tblcce_subjectskillmap.SubjectId WHERE tblcce_subjectskillmap.PartId=" + int.Parse(drpart["Id"].ToString()) + " AND tblcce_subjectskillmap.ClassId=" + Classid + "";
                    DataSet Ds_Subject = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Ds_Subject.Tables[0].Rows.Count == 0)
                        valid = false;
                    else
                    {
                        string partname = "@@" + drpart["Description"].ToString() + "-desc@@";
                        partname = partname.Replace(" ", "");
                        partname = partname.ToLower();
                        Dataset_Dic.Add(partname, GetPartFooterMessageDataSet(drpart["FooterDesc"].ToString()));
                        foreach (DataRow drsubject in Ds_Subject.Tables[0].Rows)
                        {
                            if (Termid == 1)
                                Termcolumn = "Term1";
                            else if (Termid == 2)
                                Termcolumn = "Term2";
                            else
                                Termcolumn = "Term3";

                            sql = "SELECT tblcce_subjectskillmap.SkillOrder as SINo,tblcce_subjectskills.SkillName," + Tablename + ".DescriptiveIndicator," + Tablename + "." + Termcolumn + " from " + Tablename + " inner JOIN tblcce_subjectskills on " + Tablename + ".SkillId=tblcce_subjectskills.Id inner JOIN tblcce_subjectskillmap on tblcce_subjectskillmap.SkillId=tblcce_subjectskills.Id where " + Tablename + ".SubjectId=" + int.Parse(drsubject["Id"].ToString()) + " AND " + Tablename + ".StudentId=" + int.Parse(Session["StudId"].ToString()) + " order by tblcce_subjectskillmap.SkillOrder";
                            string subjectname = "@@" + drsubject["subject_name"].ToString() + "@@";
                            subjectname = subjectname.Replace(" ", "");
                            subjectname = subjectname.ToLower();
                            Dataset_Dic.Add(subjectname, GetSubjectGrade(sql));
                        }
                        valid = true;
                    }

                }
            }
            return valid;
        }

        /// <summary>
        /// it will return grad subject marks
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataSet GetSubjectGrade(string sql)
        {
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return SetDefaultColumnname(ds);
        }

        /// <summary>
        /// it will return footer message dataset like a some time under the part discription will come 
        /// </summary>
        /// <param name="FooterDesc"></param>
        /// <returns></returns>
        private DataSet GetPartFooterMessageDataSet(string FooterDesc)
        {
            string[] _Message = Regex.Split(FooterDesc, "/nbsp");
            DataSet Obj_DS = new DataSet();
            DataTable Obj_Tbl = new DataTable();
            Obj_Tbl.Columns.Add("Column1", typeof(string));
            Obj_Tbl.Columns.Add("Column2", typeof(string));
            Obj_Tbl.Columns.Add("Column3", typeof(string));
            if (_Message[0].ToString() != "")
            {
                for (int i = 0; i < _Message.Count(); i++)
                {
                    Obj_Tbl.Rows.Add(_Message[i].ToString(), ":", _Message[i + 1].ToString());
                    i = i + 1;
                }
            }
            else
            {
                Obj_Tbl.Rows.Add("Data not found", ":", "Data not found");
            }
            Obj_DS.Tables.Add(Obj_Tbl);
            return Obj_DS;
        }

        #endregion

        #region sms module

        /// <summary>
        /// this function will return current sms account status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_CheckConnection_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (MysmsMang.CheckConnection(out msg))
            {
                MPE_Message.Show();
                DivMainMessage.InnerHtml = msg.Replace("\r", "<br/>");

            }
            else
            {
                MPE_Message.Show();
                DivMainMessage.InnerHtml = msg;
            }
        }

        #endregion

    }
}




#region msg
 //public string GetheadingString()
 //       {
 //           string Str = "<div style=\"border:solid 2px #F60 ; width:100%;\">";
 //           Str += "<div align=\"center\" >";
 //           Str += "<h2 style=\"color:Black; font-size:18px;\">" + MyUser.SchoolName+ "</h2>";
 //           Str += "<h3 style=\"color:Black; font-size:18px;\">" +MyUser.GetSchoolSmallAdress()+ "</h3>";
 //           Str += "<hr /></div>";
 //           return Str;
 //       }

  #region create email format
                        //StringBuilder _emaildynamichtml = new StringBuilder();
                        //if (Session["eventobject"].ToString() == "sendmail" && _continue)
                        //{
                        //    _emaildynamichtml.Append(GetheadingString());//header of the email

                        //    //exam name
                        //    _emaildynamichtml.Append("<div style=\"padding-left:2%;\">");
                        //    _emaildynamichtml.Append("<u> <b style=\"font-size:14px;  margin:10px; color:Maroon;\">" + Drp_term.SelectedItem.Text.ToString() + "</b></u>");
                        //    _emaildynamichtml.Append("</div>");

                        //    //student information of the email
                        //    _emaildynamichtml.Append("<div align=\"center;\" style=\"border:solid 2px #F60 ;\">");
                        //    _emaildynamichtml.Append("<table  width=\"100%\">");
                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");
                        //    _emaildynamichtml.Append("<tr><td align=\"center\" style=\"width:20%\">Class:</td> <td align=\"left\" style=\"width:40%\">" + Lbl_class.Text.ToString() + "</td> <td align=\"right\" style=\"width:40%\">Year &nbsp;:&nbsp; " + MyUser.CurrentBatchName + "&nbsp;&nbsp;</td></tr>");
                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");
                        //    _emaildynamichtml.Append("<tr><td align=\"center\" style=\"width:20%\">Student Name:</td> <td align=\"left\" style=\"width:40%\">" + dr.Cells[2].Text.ToString() + "</td> <td align=\"right\" style=\"width:40%\"></td></tr>");

                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\" align=\"center\"><h4><u>Accadamic Subjects</u></h4></tr>");
                            
                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");

                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\">" + _emailmsg .ToString()+ "</tr>");

                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");
                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");//result text
                        //    _emaildynamichtml.Append("<tr><td colspan=\"3\" style=\"width:100%\"></tr>");
                        //    _emaildynamichtml.Append("</table>");
                        //    //end of the student info

                        //    _emaildynamichtml.Append("</div>");
                        //}
                        #endregion
#endregion
