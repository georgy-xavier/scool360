using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class SubjectExamReport : System.Web.UI.Page
    {
        //private OdbcDataReader m_MyReader = null;
        private ClassOrganiser MyClassMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        private DataSet MyExamDataSet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["ClassId"] == null)
            //{
            //    Response.Redirect("LoadClassDetails.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            MyClassMang = MyUser.GetClassObj();
            MyExamMang = MyUser.GetExamObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(305))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MyClassMang.GetClassMangMenuString(MyUser.UserRoleId);
                  
                    Drp_Subject.Enabled = false;
                    if (Session["ClassId"] != null)
                    {
                        // FillClass(int.Parse(Session["ClassId"].ToString()));
                        FillClass(0);
                    }
                    else
                    {
                        FillClass(0);
                    }
                    Img_Export.Enabled = false;
                }
            }
        }

        private void FillClass(int _ClassId)
        {
            Drp_ClassSelect.Items.Clear();

            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass order by tblclass.Standard, tblclass.ClassName";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select Class","0");
                Drp_ClassSelect.Items.Add(li);

                while (MyReader.Read())
                {
                     li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ClassSelect.Items.Add(li);

                }
                Drp_ClassSelect.SelectedValue = _ClassId.ToString();
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_ClassSelect.Items.Add(li);
            }

        }

        protected void Drp_ClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSubjects();
            InitalsAfterClassSelect();
            Drp_Subject.Enabled = true;

            DataTable dt;
            DataRow dr;
            MyDataSet = new DataSet();
            MyDataSet.Tables.Add(new DataTable("tblexams"));
            dt = MyDataSet.Tables["tblexams"];
            dt.Columns.Add("Type");
            dt.Columns.Add("Id");
            dt.Columns.Add("ExamName");

            MyReader = null;
            int classId = int.Parse(Drp_ClassSelect.SelectedValue.ToString());
            MyReader= MyClassMang.getCombinedExams(classId);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = MyDataSet.Tables["tblexams"].NewRow();
                    dr["Type"] = "1";
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    dr["ExamName"] = MyReader.GetValue(1).ToString();

                    MyDataSet.Tables["tblexams"].Rows.Add(dr);
                }
            }

            MyReader = null;
            MyReader = MyClassMang.getIndividualExams(classId);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    dr = MyDataSet.Tables["tblexams"].NewRow();
                    dr["Type"] = "0";
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    dr["ExamName"] = MyReader.GetValue(1).ToString() +" "+ MyReader.GetValue(2).ToString(); ;

                    MyDataSet.Tables["tblexams"].Rows.Add(dr);
                }
            }
            grd_exams.Columns[0].Visible = true;
            grd_exams.Columns[1].Visible = true;
            grd_exams.DataSource = null;
            grd_exams.DataBind();
            grd_exams.DataSource = MyDataSet;
            grd_exams.DataBind();
            grd_exams.Columns[0].Visible = false;
            grd_exams.Columns[1].Visible = false;
        }

        private void InitalsAfterClassSelect()
        {
            grd_exams.DataSource = null;
            grd_exams.DataBind();
            grdResult.DataSource = null;
            grdResult.DataBind();
            ViewState["Marks"] = null;
            Lbl_Message.Text = "";
            Lbl_msg.Text="";
        }

        private void FillSubjects()
        {
            Drp_Subject.Items.Clear();
            MyReader = null;
            int _classId = int.Parse( Drp_ClassSelect.SelectedValue.ToString());
            MyReader = MyClassMang.getSubjects(_classId);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select Subject", "0");
                Drp_Subject.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Subject.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Data", "-1");
                Drp_Subject.Items.Add(li);
            }
        }

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            int classId = 0, subId = 0;
            grdResult.DataSource = null;
            grdResult.DataBind();
            ViewState["Marks"] = null;
            Lbl_Message.Text = "";
            Lbl_msg.Text = "";
            string msg = "";
            int.TryParse(Drp_ClassSelect.SelectedValue.ToString(), out classId);
            int.TryParse(Drp_Subject.SelectedValue.ToString(), out subId);
            if (InitialValidation(classId, subId, out msg))
            {
                GenerateReport(classId, subId, out msg);
            }
            else
            {
                Img_Export.Enabled = false;
                Lbl_Message.Text = msg;
            }
        }

        private bool InitialValidation(int classId, int subId, out string msg)
        {
            msg="";
            bool valid = true;
            if((classId==0)||(classId==-1))
            {
                msg="No Class Selected";
                valid=false;
            }
            else   if((subId==0)||(subId==-1))
            {
                msg="No Subject Selected";
                valid=false;
            }
            else  
            {
                int chkflg=0;
                foreach( GridViewRow gvr in grd_exams.Rows)
                {
                    CheckBox chk=  (CheckBox)gvr.FindControl("chk_select");
                    if(chk.Checked==true)
                    {
                        chkflg=1;
                    }
                }
                if(chkflg==0)
                {
                    msg="No Exams Selected";
                    valid=false;
                }
            }
            return valid;
        }

        private void GenerateReport(int classId,int subId,out string msg)
        {
            int examId = 0, examtype = 0, exists = 0 ;
            msg="";
            string examname = "",lblmsg="";

            DataSet MarkDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            int i, j;
            string[] name = new string[100];

            MarkDataSet.Tables.Add(new DataTable("Exam"));
            dt = MarkDataSet.Tables["Exam"];

            dt.Columns.Add("Student Name");
            name[0] = "Student Name";
            i = 1;
            foreach (GridViewRow gvr in grd_exams.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("chk_select");
                if (chk.Checked == true)
                {
                    examtype = int.Parse(gvr.Cells[0].Text.ToString());
                    examId = int.Parse(gvr.Cells[1].Text.ToString());
                    examname = gvr.Cells[3].Text.ToString();

                    if (MyClassMang.IsSubjectExistsInExam(MyUser.CurrentBatchId, examtype, examId, subId, classId, out msg))
                    {
                        exists = 1;
                        dt.Columns.Add(examname);
                        name[i] = examname;
                        i++;
                    }
                    else
                    {
                        lblmsg = lblmsg + examname + ", ";
                    }
                }
            }
            if (lblmsg != "")
            {
                Lbl_Message.Text = "Marks for " + Drp_Subject.SelectedItem.Text.ToString() + " not found for - " + lblmsg;
            }

            double mark = 0;
            int studid = 0;
            DataSet StudentsDataset = MyClassMang.getStudents(classId);
            if (StudentsDataset != null && StudentsDataset.Tables != null && StudentsDataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow students in StudentsDataset.Tables[0].Rows)
                {
                    i = 0;
                    dr = MarkDataSet.Tables["Exam"].NewRow();
                    dr[name[i]] = students[1].ToString();
                    studid =int.Parse(students[0].ToString());

                    foreach (GridViewRow gvr in grd_exams.Rows)
                    {
                        CheckBox chk = (CheckBox)gvr.FindControl("chk_select");
                        if (chk.Checked == true)
                        {
                            examtype = int.Parse(gvr.Cells[0].Text.ToString());
                            examId = int.Parse(gvr.Cells[1].Text.ToString());
                            examname = gvr.Cells[3].Text.ToString();

                            if (MyClassMang.IsSubjectExistsInExam(MyUser.CurrentBatchId, examtype, examId, subId, classId, out msg))
                            {
                                i++;
                                exists = 1;
                                //***Make Dataset ***
                                //MakeReportDataset(examname, examtype, examId, subId, classId);
                                mark = MyClassMang.getExamMark(MyUser.CurrentBatchId, examtype, examId, studid, subId, classId);
                                dr[name[i]] = (Math.Round(mark, 2)).ToString();
                                if (mark == -1)
                                {
                                    dr[name[i]] = "A";
                                }
                            }
                        }
                    }
                    MarkDataSet.Tables["Exam"].Rows.Add(dr);
                }
            }
            int mj = MarkDataSet.Tables[0].Rows.Count;

            if (mj > 0)
            {
                grdResult.DataSource = MarkDataSet;
                grdResult.DataBind();
                ViewState["Marks"] = MarkDataSet;
                Img_Export.Enabled = true;
            }
            else
            {
                Img_Export.Enabled = false;
            }
        }

        protected void Img_Export_Click(object sender, EventArgs e)
        {
            string examname = Drp_Subject.SelectedItem.Text.ToString();
            string filename = examname + " Mark Sheet.xls";
            if (ViewState["Marks"] != null)
            {
                DataSet MyExamData = (DataSet)ViewState["Marks"];
                if (MyExamData.Tables.Count > 0)
                {

                    //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyExamData,filename))
                    //{
                    //    Lbl_Message.Text = "This function need Ms office";
                    //}
                    string FileName = examname + " Mark Sheet";
                    string _ReportName = examname + " Mark Sheet";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        Lbl_Message.Text = "This function need Ms office";
                    }
                }
            }
        }

    }        
}
