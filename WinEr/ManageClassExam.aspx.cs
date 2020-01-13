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

namespace WinEr
{
    public partial class ManageClassExam : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["ExamId"] == null)
            {
                Response.Redirect("ViewExams.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(76))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _ExamMenuStr;
                    _ExamMenuStr = MyExamMang.GetSubExamMangMenuString(MyUser.UserRoleId);
                    this.SubExammngMenu.InnerHtml = _ExamMenuStr;                    
                   
                    //some initlization
                    LoadCreateClassExamDetails();
                    LoadEditExamDetails();
                    LoadExamGeneralDetails();
                }
            }
        }
       
        private void LoadExamGeneralDetails()
        {
            string ExamNme, ExamType, examfreqncy;
            MyExamMang.GetExamDetail(int.Parse(Session["ExamId"].ToString()), out ExamNme, out ExamType, out examfreqncy);
            Lbl_ExamName.Text = ExamNme.ToString();
            Lbl_ExamType.Text = ExamType.ToString();
            Lbl_Frequency.Text = examfreqncy.ToString();
        }

        private void LoadEditExamDetails()
        {
            AddClassToEditList();
            if (AddSubjectToEditList())
            {
                GetClassExmId();
                AddToEditGrid();
                SetControls(Drp_Subj.SelectedValue);
            }
            else
            {
                RefreshPage();
            }           
        }       

        private void LoadCreateClassExamDetails()
        {
            AddClassToList();
            AddSubjectToList();  
           
        }


        //********************************************************************************************************
        //********************************************************************************************************
        //                                     CREATE EXAM
        //********************************************************************************************************
        //********************************************************************************************************
        
        #region MyRegion_CreateExam

        private void AddClassToList()
        {
            Drp_ClassName.Items.Clear();
            string sql = "SELECT tblclass.Id, tblclass.ClassName FROM tblclass INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id  WHERE tblclass.Id NOT IN (SELECT tblclassexam.ClassId FROM tblclassexam WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + ") AND tblclass.Id IN  (SELECT tblclass.Id from tblclass  where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblstandard.Id, tblclass.Id";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassName.Items.Add(li);
                }               
            }
            else
            {
                ListItem li = new ListItem("No class found", "-1");
                Drp_ClassName.Items.Add(li);
            }
        }

        protected void Drp_ClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grd_Exam.DataSource = null;
            Grd_Exam.DataBind();
            AddSubjectToList();
        }

        private void AddSubjectToList()
        {

            Drp_Sub.Items.Clear();
            string sqls = "";
            string sql="";
            OdbcDataReader Configvaluereader = null;
            int value = 0;
            sqls = "Select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='IndividualStudentSubject'";
            Configvaluereader = MyExamMang.m_MysqlDb.ExecuteQuery(sqls);
            if (Configvaluereader.HasRows)
            {
                //int.TryParse(Configvaluereader.GetValue(0).ToString(), out value);
                //  Hdn_ManStudId.Value = value.ToString();
                if (int.Parse(Configvaluereader.GetValue(0).ToString()) == 0)
                {
                     sql = "SELECT tblsubjects.Id, tblsubjects.subject_name, tblsubjects.SubjectCode FROM tblsubjects INNER JOIN tblclasssubmap ON tblsubjects.Id=tblclasssubmap.SubjectID WHERE tblclasssubmap.ClassId=" + int.Parse(Drp_ClassName.SelectedValue);
                }
                else
                {
                    // sql = " select tblsubjects.Id, tblsubjects.subject_name,tblsubjects.SubjectCode from tblclass join tblstudent_indiviualsubject join tblstudent join tblsubjects where  tblstudent_indiviualsubject.ClassName = tblclass.id and tblclass.id = tblstudent_indiviualsubject.ClassName and tblstudent_indiviualsubject.StudentId = tblstudent.id   and tblsubjects.id = tblstudent_indiviualsubject.SubjectId and tblclass.id = " + int.Parse(Drp_ClassName.SelectedValue) + " union Select tblsubjects.Id,tblsubjects.subject_name,tblsubjects.SubjectCode from tblsubjects inner join tblclasssubmap on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId =" + int.Parse(Drp_ClassName.SelectedValue);
                    sql= "select * from (select tblsubjects.Id, tblsubjects.subject_name,tblsubjects.SubjectCode from tblclass join tblstudent_indiviualsubject join tblstudent join tblsubjects where  tblstudent_indiviualsubject.ClassName = tblclass.id and tblclass.id = tblstudent_indiviualsubject.ClassName and tblstudent_indiviualsubject.StudentId = tblstudent.id   and tblsubjects.id = tblstudent_indiviualsubject.SubjectId and tblclass.id = " + int.Parse(Drp_ClassName.SelectedValue)+" union Select tblsubjects.Id,tblsubjects.subject_name,tblsubjects.SubjectCode from tblsubjects inner join tblclasssubmap on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId =" + int.Parse(Drp_ClassName.SelectedValue)+") AS t1";
                }



                //string sql = "Select tblsubjects.subject_name,tblsubjects.Id from tblsubjects inner join tblclasssubmap on tblsubjects.Id = tblclasssubmap.SubjectId where tblclasssubmap.ClassId = " + int.Parse(Drp_ClassName.SelectedValue) + " union select tblsubjects.subject_name,tblsubjects.Id from tblclass join tblstudent_indiviualsubject join tblstudent join tblsubjects where  tblstudent_indiviualsubject.ClassName = tblclass.id and tblclass.id = tblstudent_indiviualsubject.ClassName and tblstudent_indiviualsubject.StudentId = tblstudent.id   and tblsubjects.id = tblstudent_indiviualsubject.SubjectId and tblclass.id =" + int.Parse(Drp_ClassName.SelectedValue);

            }
          
                if (Grd_Exam.Rows.Count > 0)
                {
                    int size = Grd_Exam.Rows.Count;
                    int i = 0;
                    int[] f = new int[size];
                if (int.Parse(Configvaluereader.GetValue(0).ToString()) == 0)
                {
                    sql = sql + " AND tblsubjects.Id NOT IN (";
                }
                else
                {
                    sql = sql + " where t1.Id NOT IN (";
                }
                    if (size > 1)
                    {
                        foreach (GridViewRow gv in Grd_Exam.Rows)
                        {
                            f[i] = int.Parse(gv.Cells[1].Text.ToString());
                            i++;
                        }
                        for (int j = 0; j < size - 1; j++)
                        {
                            sql = sql + f[j] + ", ";
                        }
                        sql = sql + f[size - 1] + ")";
                    }
                    else
                    {
                        foreach (GridViewRow gv in Grd_Exam.Rows)
                        {
                            sql = sql + int.Parse(gv.Cells[1].Text.ToString()) + ")";
                        }
                    }
                
            }
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString() + "/" + MyReader.GetValue(2).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Sub.Items.Add(li);
                }
                Drp_Sub.SelectedIndex = 0;
            }
            else
            {
                string sub = "No Subject..";
                string sid = "-1";
                ListItem li = new ListItem(sub, sid);
                Drp_Sub.Items.Add(li);
            }
        }

        protected void Grd_Exam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i_SelectedUniqueId = Grd_Exam.SelectedRow.Cells[1].Text.ToString();            
            foreach (GridViewRow gv in Grd_Exam.Rows)
            {
                if (gv.Cells[1].Text.ToString() == i_SelectedUniqueId)
                {
                    RemoveCell(gv);
                }
            }
            if (!(Grd_Exam.Rows.Count > 0))
            {
                Btn_Cancel.Visible = false;
                Btn_Create.Visible = false;
            }
        }

        private void RemoveCell(GridViewRow _gv)
        {
            DataSet _Exmdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Exmdataset.Tables.Add(new DataTable("Exam"));
            dt = _Exmdataset.Tables["Exam"];
            dt.Columns.Add("Subject Id");
            dt.Columns.Add("Subject Name");
            dt.Columns.Add("Max Mark");
            dt.Columns.Add("Min Mark");
            dt.Columns.Add("Subject Group Name");
            foreach (GridViewRow gv in Grd_Exam.Rows)
            {
                if (_gv != gv)
                {
                    dr = _Exmdataset.Tables["Exam"].NewRow();
                    dr["Subject Id"] = int.Parse(gv.Cells[1].Text.ToString());
                    dr["Subject Name"] = gv.Cells[2].Text.ToString();
                    dr["Max Mark"] = float.Parse(gv.Cells[3].Text.ToString());
                    dr["Min Mark"] = float.Parse(gv.Cells[4].Text.ToString());
                    dr["Subject Group Name"] = gv.Cells[5].Text.ToString();
                    _Exmdataset.Tables["Exam"].Rows.Add(dr);
                }
            }
            Grd_Exam.Columns[1].Visible = true;
            Grd_Exam.DataSource = _Exmdataset;
            Grd_Exam.DataBind();
            Grd_Exam.Columns[1].Visible = false;
            AddSubjectToList();
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            if (Drp_Sub.SelectedValue != "-1")
            {
                if (Txt_Max.Text != "" && Txt_Min.Text != "")
                {
                    if (float.Parse(Txt_Max.Text.ToString()) > float.Parse(Txt_Min.Text.ToString()))
                    {
                        AddToGrid();
                        AddSubjectToList();
                        ClearControls();
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Maximum Marks should be greater than Minimum Marks.");
                        
                    }
                }
                else
                {
                     WC_MessageBox.ShowMssage("Enter The marks");
                    
                }
            }
            else
            {
                 WC_MessageBox.ShowMssage("Subject Not Selected..");
               
            }
           
        }

        private void AddToGrid()
        {
            Grd_Exam.Columns[1].Visible = true;
            if (Grd_Exam.Rows.Count > 0)
            {
                Grd_Exam.DataSource = AddPreviousItems();
                Grd_Exam.DataBind();
            }
            else
            {
                Grd_Exam.DataSource = AddItems();
                Grd_Exam.DataBind();
            }
            Grd_Exam.Columns[1].Visible = false;
            Btn_Cancel.Visible = true;
            Btn_Create.Visible = true;
        }        

        private object AddPreviousItems()
        {
            DataSet _Exmdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Exmdataset.Tables.Add(new DataTable("Exam"));
            dt = _Exmdataset.Tables["Exam"];
            dt.Columns.Add("Subject Id");
            dt.Columns.Add("Subject Name");
            dt.Columns.Add("Max Mark");
            dt.Columns.Add("Min Mark");
            dt.Columns.Add("Subject Group Name");
            foreach (GridViewRow gv in Grd_Exam.Rows)
            {
                dr = _Exmdataset.Tables["Exam"].NewRow();
                dr["Subject Id"] = int.Parse(gv.Cells[1].Text.ToString());
                dr["Subject Name"] = gv.Cells[2].Text.ToString();
                dr["Max Mark"] = float.Parse(gv.Cells[3].Text.ToString());
                dr["Min Mark"] = float.Parse(gv.Cells[4].Text.ToString());
                dr["Subject Group Name"] = gv.Cells[5].Text.ToString();
                _Exmdataset.Tables["Exam"].Rows.Add(dr);
            }
            dr = _Exmdataset.Tables["Exam"].NewRow();
            dr["Subject Id"] = int.Parse(Drp_Sub.SelectedValue);
            dr["Subject Name"] = Drp_Sub.SelectedItem.ToString();
            dr["Max Mark"] = float.Parse(Txt_Max.Text.ToString());
            dr["Min Mark"] = float.Parse(Txt_Min.Text.ToString());
            int subid = 0,classid=0;
            int.TryParse(Drp_Sub.SelectedValue.ToString(), out subid);
            int.TryParse(Drp_Clas.SelectedValue.ToString(), out classid);
            dr["Subject Group Name"] =Convert.ToString(GetSubjectGroupname(subid, classid));
            _Exmdataset.Tables["Exam"].Rows.Add(dr);
            return _Exmdataset;
        }

        private object GetSubjectGroupname(int subid, int classid)
        {
            object ob=null;
            string sql = "select tbltime_subgroup.Name from tbltime_subgroup INNER JOIN tblsubjects ON tblsubjects.sub_Catagory=tbltime_subgroup.Id where tblsubjects.Id="+subid;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
                ob = ds.Tables[0].Rows[0][0].ToString();
            return ob;

        }

        private DataSet AddItems()
        {
            DataSet _Exmdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Exmdataset.Tables.Add(new DataTable("Exam"));
            dt = _Exmdataset.Tables["Exam"];
            dt.Columns.Add("Subject Id");
            dt.Columns.Add("Subject Name");
            dt.Columns.Add("Max Mark");
            dt.Columns.Add("Min Mark");
            dt.Columns.Add("Subject Group Name");
            dr = _Exmdataset.Tables["Exam"].NewRow();
            dr["Subject Id"] = int.Parse(Drp_Sub.SelectedValue);
            dr["Subject Name"] = Drp_Sub.SelectedItem.Text;
            dr["Max Mark"] = float.Parse(Txt_Max.Text.ToString());
            dr["Min Mark"] = float.Parse(Txt_Min.Text.ToString());
            int subid = 0, classid = 0;
            int.TryParse(Drp_Sub.SelectedValue.ToString(), out subid);
            int.TryParse(Drp_Clas.SelectedValue.ToString(), out classid);
            dr["Subject Group Name"] = Convert.ToString(GetSubjectGroupname(subid, classid));
            _Exmdataset.Tables["Exam"].Rows.Add(dr);
            return _Exmdataset;
        }

        private void ClearControls()
        {
            Txt_Min.Text = "";
            Txt_Max.Text = "";
        }

        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            int _examID = int.Parse(Session["ExamId"].ToString());
            int _ClassId = int.Parse(Drp_ClassName.SelectedValue.ToString());
            if (Grd_Exam.Rows.Count > 0)
            {
                int classexamId = MyExamMang.CreateClassExamMaster(_ClassId, _examID, MyUser.UserName.ToString());
                foreach (GridViewRow gv in Grd_Exam.Rows)
                {
                    int _SubjectId = int.Parse(gv.Cells[1].Text.ToString());
                    double _Max = 0; double.TryParse(gv.Cells[3].Text.ToString(), out _Max);
                    double _Min = 0; double.TryParse(gv.Cells[4].Text.ToString(), out _Min);
                    MyExamMang.CreateClassExam(classexamId, _SubjectId, _Max, _Min);
                }
                 WC_MessageBox.ShowMssage(Lbl_ExamName.Text.ToString() + " is added to class " + Drp_ClassName.SelectedItem.ToString() + "");
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Add Exam", "One Exam " + Lbl_ExamName.Text.ToString() + " is added to class " + Drp_ClassName.SelectedItem.ToString(), 1);
                RefreshAll();
                AddClassToEditList();
                AddSubjectToList();
                LoadEditExamDetails();

                #region mani code
                double passmark = 0;
                if (Needforaggregateconfiguration(out passmark, classexamId, _ClassId))
                {
                    WC_Aggregategroup.ShowMssage(_ClassId, _examID, passmark);
                }
                else
                {
                    WC_MessageBox.ShowMssage(" Create Class exam subject map!");
                }
                #endregion

            }
            else
            {
                 WC_MessageBox.ShowMssage("No Data...");
                
            }
        }

        //mani code for aggregate
        private bool Needforaggregateconfiguration(out double passmark, int _examID, int _ClassId)
        {
            bool valid = false;
            passmark = 0;
            WC_Aggregategroup.EVNTSave += new EventHandler(WC_MISMESSAGEBOX_UPDATE);
            if (GetExamPassMark(_ClassId, _examID, out passmark))
                valid = true;
            return valid;
        }

        private void RefreshAll()
        {
            Grd_Exam.DataSource = null;
            Grd_Exam.DataBind();
            AddClassToList();
            Btn_Create.Visible = false;
            Btn_Cancel.Visible = false;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            RefreshAll();
            Response.Redirect("ExamDetails.aspx");
        } 
       
        #endregion

        //********************************************************************************************************
        //********************************************************************************************************
        //                                     MANAGE EXAM
        //********************************************************************************************************
        //********************************************************************************************************
       
        #region MyRegion_ManageExam

        private void AddClassToEditList()
        {
            Drp_Clas.Items.Clear();
            string sql = "SELECT tblclass.Id, tblclass.ClassName FROM tblclass INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id  WHERE tblclass.Id IN (SELECT tblclassexam.ClassId FROM tblclassexam WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) + ") AND tblclass.Id IN  (SELECT tblclass.Id from tblclass  where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblstandard.Id, tblclass.Id";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Clas.Items.Add(li);
                }
                EnableControls();
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Clas.Items.Add(li);
                Disablecontrols();
            }
        }

        private void EnableControls()
        {
            Lbl_Note.Text = "";
            PnlCntrls.Visible = true;
        }

        private void Disablecontrols()
        {
            Lbl_Note.Text = "No Class Present for selected exam!!";
            PnlCntrls.Visible = false;
        }

        protected void Drp_Clas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddSubjectToEditList())
            {
                GetClassExmId();
                AddToEditGrid();
                SetControls(Drp_Subj.SelectedValue);
            }
        }

        private void GetClassExmId()
        {
            string sql = "SELECT tblclassexam.Id FROM tblclassexam WHERE tblclassexam.ExamId=" + int.Parse(Session["ExamId"].ToString()) +" AND tblclassexam.ClassId="+ int.Parse(Drp_Clas.SelectedValue);
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_CXmId.Text = MyReader.GetValue(0).ToString();
            }
            else
            {
                Txt_CXmId.Text = "-1";
            }
        }

        private bool AddSubjectToEditList()
        {
            bool _ret=false;
            Drp_Subj.Items.Clear();
            string sql = "SELECT tblsubjects.Id, tblsubjects.subject_name,tblsubjects.SubjectCode   FROM tblsubjects INNER JOIN tblclasssubmap ON tblsubjects.Id=tblclasssubmap.SubjectID WHERE tblclasssubmap.ClassId=" + Drp_Clas.SelectedValue;
           
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString()+"/"+MyReader.GetValue(2).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Subj.Items.Add(li);
                }
                _ret = true;
            }
            else
            {
                string sub = "No Subject..";
                string sid = "-1";
                ListItem li = new ListItem(sub, sid);
                Drp_Subj.Items.Add(li);
            }
            return _ret;
        }

        private void AddToEditGrid()
        {
            Grd_EditExam.Columns[0].Visible = true;
            string sql = "SELECT tblclassexamsubmap.SubId, CONCAT(tblsubjects.subject_name,\" / \", tblsubjects.SubjectCode) as `subject_name` , tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark FROM tblclassexamsubmap INNER JOIN tblsubjects ON tblsubjects.Id=tblclassexamsubmap.SubId WHERE tblclassexamsubmap.ClassExamId=" + int.Parse(Txt_CXmId.Text);
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Grd_EditExam.DataSource = MyReader;
                Grd_EditExam.DataBind();
                //Lbl_Note.Text = "";                
            }
            else
            {
                Grd_EditExam.DataSource = null;
                Grd_EditExam.DataBind();

            }
            Grd_EditExam.Columns[0].Visible = false;
        }

        protected void Grd_EditExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i_SelectedSubId = Grd_EditExam.SelectedRow.Cells[1].Text.ToString();
            SetControls(i_SelectedSubId);
            Drp_Subj.SelectedValue = i_SelectedSubId;
        }      

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int scheduledID = 0;
            // for delete subjects from exam
            if (!MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Drp_Clas.SelectedValue, out scheduledID))
            {

                if (Grd_EditExam.Rows.Count == 1)
                {
                    MyExamMang.DeleteClassExam(int.Parse(Txt_CXmId.Text));
                    MyExamMang.DeleteClassExamMaster(int.Parse(Txt_CXmId.Text));
                    LoadEditExamDetails();
                    LoadCreateClassExamDetails();
                }
                else
                {
                    string i_SelectedSubId = Drp_Subj.SelectedValue;
                    string sql = "DELETE FROM tblclassexamsubmap WHERE ClassExamId=" + int.Parse(Txt_CXmId.Text) + " AND SubId=" + i_SelectedSubId;
                    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    AddToEditGrid();


                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam Subject Deleted", Drp_Subj.SelectedItem.Text + " deleted from  " + Lbl_ExamName.Text+ " from  " + Drp_Clas.SelectedItem.Text, 1);

                }
            }
            else
            {
                if (MyUser.HaveActionRignt(1128))
                {
                    if (Grd_EditExam.Rows.Count == 1)
                    {
                        string i_SelectedSubId = Grd_EditExam.Rows[0].Cells[1].Text.ToString();

                        MyExamMang.DeleteSubjectsFromscheduledExam(i_SelectedSubId, scheduledID, MyUser.CurrentBatchId);
                        MyExamMang.DeleteScheduledExam(i_SelectedSubId);
                        MyExamMang.DeleteClassExam(int.Parse(Txt_CXmId.Text));
                        MyExamMang.DeleteClassExamMaster(int.Parse(Txt_CXmId.Text));
                        LoadEditExamDetails();
                        LoadCreateClassExamDetails();
                    }
                    else
                    {
                        string i_SelectedSubId = Drp_Subj.SelectedValue;
                        MyExamMang.DeleteSubjectsFromscheduledExam(i_SelectedSubId, scheduledID, MyUser.CurrentBatchId);
                        string sql = "DELETE FROM tblclassexamsubmap WHERE ClassExamId=" + int.Parse(Txt_CXmId.Text) + " AND SubId=" + i_SelectedSubId;
                        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        AddToEditGrid();



                    }


                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam Subject Deleted", Drp_Subj.SelectedItem.Text + " deleted from  " + Lbl_ExamName.Text + " from  " + Drp_Clas.SelectedItem.Text, 1);

                }
                else
                {
                    WC_MessageBox.ShowMssage("Exam is Scheduled.. Cannot be deleted.");
                }
               
            }

        }

        protected void Btn_DeleteClass_Click(object sender, EventArgs e)
        {
            // for delete exam
            int scheduledId = 0;
            if (!MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Drp_Clas.SelectedValue,out scheduledId) )
            {

                if (Grd_EditExam.Rows.Count > 0)
                {
                    MyExamMang.DeleteClassExam(int.Parse(Txt_CXmId.Text));
                    MyExamMang.DeleteClassExamMaster(int.Parse(Txt_CXmId.Text));

                    LoadEditExamDetails();
                    LoadCreateClassExamDetails();
                     WC_MessageBox.ShowMssage("Exam Deleted..");
                     MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam Deleted",""+ Lbl_ExamName.Text + " deleted", 1);

                }
                else
                {
                     WC_MessageBox.ShowMssage("No Exams Found..");
                    
                }
            }
            else
            {
                 WC_MessageBox.ShowMssage("Exam is Scheduled.. Cannot be deleted...");
                
            }
        }

        protected void Btn_AddNew_Click(object sender, EventArgs e)
        {

            if (Drp_Subj.SelectedValue != "-1")
            {
                if (Txt_Maxm.Text != "" && Txt_Pass.Text != "")
                {
                    if (float.Parse(Txt_Maxm.Text.ToString()) > float.Parse(Txt_Pass.Text.ToString()))
                    {
                        int classexamId = int.Parse(Txt_CXmId.Text);
                        int _SubjectId = int.Parse(Drp_Subj.SelectedValue);
                        float _Max = float.Parse(Txt_Maxm.Text);
                        float _Min = float.Parse(Txt_Pass.Text);
                        
                        if (Btn_AddNew.Text == "Add")
                        {
                            int scheduledid = 0;
                            if (!MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Drp_Clas.SelectedValue, out scheduledid))
                            {
                                string sql = "SELECT tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark FROM tblclassexamsubmap WHERE tblclassexamsubmap.ClassExamId=" + classexamId + " AND tblclassexamsubmap.SubId=" + _SubjectId;
                                MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                               
                                if (MyReader.HasRows)
                                {
                                     WC_MessageBox.ShowMssage("Subject Present..");
                                    
                                    //ClearAll();
                                }
                                else
                                {

                                    MyExamMang.CreateClassExam(classexamId, _SubjectId, _Max, _Min);
                                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "New Subject for Exam", Drp_Subj.SelectedItem.Text + " is added to " +Lbl_ExamName.Text+" for" + Drp_ClassName.SelectedItem.Text, 1);
                  
                                    AddToEditGrid();
                                    Btn_AddNew.Text = "Save";
                                    //Btn_AddNew.CssClass = "graysave";
                                    WC_MessageBox.ShowMssage( "Subject Added..");
                                    
                                    //ClearAll();
                                }
                            }
                            else
                            {
                                if (MyUser.HaveActionRignt(1128))
                                {

                                    string sql = "SELECT tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark FROM tblclassexamsubmap WHERE tblclassexamsubmap.ClassExamId=" + classexamId + " AND tblclassexamsubmap.SubId=" + _SubjectId;
                                    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                                    if (MyReader.HasRows)
                                    {
                                        WC_MessageBox.ShowMssage("Subject Present..");

                                        //ClearAll();
                                    }

                                    else
                                    {

                                        MyExamMang.CreateClassExam(classexamId, _SubjectId, _Max, _Min);
                                        MyExamMang.UpdateInexammarkcolumn(classexamId, _SubjectId,MyUser.CurrentBatchId);
                                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "New Subject for Exam", Drp_Subj.SelectedItem.Text + " is added to " + Lbl_ExamName.Text + " for" + Drp_ClassName.SelectedItem.Text, 1);
                                        MyExamMang.UpdateExamScheduleStatusfromClassExamId(classexamId, "Updated", MyUser.CurrentBatchId);

                                        AddToEditGrid();
                                        Btn_AddNew.Text = "Save";
                                       // Btn_AddNew.CssClass = "graysave";
                                        WC_MessageBox.ShowMssage("Subject Added.");

                                    }
                                }
                                else
                                {

                                    WC_MessageBox.ShowMssage("Exam is Scheduled.Subject Cannot be added now...");
                                }

                            }
                        }
                        else
                        {
                            int scheduledid = 0;
                            if (!MyExamMang.ExamSchduled(int.Parse(Session["ExamId"].ToString()), MyUser.CurrentBatchId, Drp_Clas.SelectedValue, out scheduledid))
                            {
                                MyExamMang.UpdateClassExam(classexamId, _SubjectId, _Max, _Min);
                                AddToEditGrid();
                                WC_MessageBox.ShowMssage("Details Saved..");
                                
                                // ClearAll();
                            }
                            else
                            {

                                if (MyUser.HaveActionRignt(1128))
                                {

                                    MyExamMang.UpdateClassExam(classexamId, _SubjectId, _Max, _Min);
                                    MyExamMang.UpdatedEnterdMark(classexamId, _SubjectId, MyUser.CurrentBatchId);
                                    MyExamMang.UpdateExamScheduleStatusfromClassExamId(classexamId, "Updated", MyUser.CurrentBatchId);
                                    AddToEditGrid();
                                    WC_MessageBox.ShowMssage("Details Saved..");
                                }
                                else
                                {
                                    WC_MessageBox.ShowMssage("Exam is Scheduled.. Subject Cannot be updated now...");
                                }
                               
                            }
                        }
                    }
                    else
                    {
                        WC_MessageBox.ShowMssage("Maximum Marks should be greater than Minimum Marks.");
                        
                    }
                }
                else
                {
                     WC_MessageBox.ShowMssage("Enter the marks");
                    
                }
            }
            else
            {
                 WC_MessageBox.ShowMssage("Subject not selected..");
               
            }
        }
            
           
        

        private void ClearAll()
        {
            Txt_Maxm.Text = "";
            Txt_Pass.Text = "";
        }
        
       

        private void RefreshPage()
        {
            Grd_EditExam.Columns[0].Visible = true;
            Grd_EditExam.DataSource = null;
            Grd_EditExam.DataBind();
            Grd_EditExam.Columns[0].Visible = false;            
        }

        private void SetControls(string _subjectid)
        {
            string sql = "SELECT tblclassexamsubmap.MinMark, tblclassexamsubmap.MaxMark FROM tblclassexamsubmap WHERE tblclassexamsubmap.ClassExamId=" + int.Parse(Txt_CXmId.Text) + " AND tblclassexamsubmap.SubId=" + _subjectid;
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_Maxm.Text = MyReader.GetValue(1).ToString();
                Txt_Pass.Text = MyReader.GetValue(0).ToString();
                Btn_AddNew.Text = "Save";
               // Btn_AddNew.CssClass = "graysave";
                Btn_Delete.Enabled = true;
            }
            else
            {
                Txt_Maxm.Text ="";
                Txt_Pass.Text = "";
                Btn_AddNew.Text = "Add";
               // Btn_AddNew.CssClass = "grayadd";
                Btn_Delete.Enabled = false;
            }
        }

        protected void Drp_Subj_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControls(Drp_Subj.SelectedValue);
        }

        #endregion

        //protected void Btn_configgroup_Click(object sender, EventArgs e)
        //{
        //    WC_Aggregategroup.EVNTSave += new EventHandler(WC_MISMESSAGEBOX_UPDATE);
        //    int classid = 0;
        //    int.TryParse(Drp_ClassName.SelectedValue.ToString(), out classid);
        //    int _examID = int.Parse(Session["ExamId"].ToString());
        //    double passmark = 0;
        //    if (GetExamPassMark(classid, _examID, out passmark))
        //        WC_Aggregategroup.ShowMssage(classid, _examID, passmark);
        //    else
        //    {
        //        WC_MessageBox.ShowMssage(" Create Class exam subject map!");
              
        //    }
           
           
        //}

        private bool GetExamPassMark(int classid, int _examID, out double passmark)
        {
            bool valid = false;
            passmark = 0;
            string sql = "SELECT CASE WHEN  AVG(tblclassexamsubmap.MinMark) IS NULL OR  AVG(tblclassexamsubmap.MinMark)=''  THEN 0  ELSE AVG(tblclassexamsubmap.MinMark)  END AS Passmark  FROM tblclassexamsubmap INNER JOIN tblclasssubmap on tblclasssubmap.SubjectId=tblclassexamsubmap.SubId where tblclassexamsubmap.ClassExamId="+_examID+" AND tblclasssubmap.ClassId="+classid;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                double.TryParse(ds.Tables[0].Rows[0][0].ToString(), out passmark);
                if(passmark>0.0&&passmark!=0.00)
                    valid = true;
            }
            return valid;
        }

        protected void WC_MISMESSAGEBOX_UPDATE(object sender, EventArgs e)
        {
            //int classid = 0;
            //int.TryParse(Drp_ClassName.SelectedValue.ToString(), out classid);
            //int _examID = int.Parse(Session["ExamId"].ToString());
            //WC_Aggregategroup.ShowMssage(classid, _examID);
        }

        //protected void Chk_needaggrepassmark_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (Chk_needaggrepassmark.Checked)
        //        Btn_configgroup.Visible = true;
        //    else
        //        Btn_configgroup.Visible = false;
        //}
    
    }
}
