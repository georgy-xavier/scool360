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
using System.Xml;
using System.Text;
using System.IO;
using WinBase;
namespace WinEr
{
    public partial class PublishExamResult : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet _Mydata_Subjects = null;
        private DataSet MydataSet = null;
        private OdbcDataReader MyReader1 = null;
       // private OdbcDataReader MyReader2 = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();

            if (MyExamMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(50))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //string _MenuStr;
                    //_MenuStr = MyExamMang.GetExamMangMenuString(MyUser.UserRoleId);
                    //this.ExammngMenu.InnerHtml = _MenuStr;
                    //_MenuStr = MyUser.GetDetailsString(101);
                    //this.ActionInfo.InnerHtml = _MenuStr;
                    CheckExamReports();
                    GridSelected.Visible = false;
                    BtXML.Visible = false;
                }

            }
        }

        private void CheckExamReports()
        {
            if (MyExamMang.IsAnyReportGenerated())
            {
                LoadClassToDrpList();
            }
            else
            {
                Lbl_msg.Text = "No exam reports are available";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadClassToDrpList()
        {
            Drp_Class.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class.Items.Add(new ListItem("Select An Exam", "-1"));
                Drp_Class.Items.Add(new ListItem("All", "0"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);

                }

            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Class;
            bool _valid = int.TryParse(Drp_Class.SelectedValue.ToString(),out Class);
            if (_valid)
            {
                if (Class == 0)
                {

                    string sql = "select DISTINCT(ExamId) ,ExamName , tblclass.ClassName from tblstudentmark inner join tblexamschedule on tblstudentmark.ExamSchId=tblexamschedule.Id inner join tblexam on tblexam.Id=tblexamschedule.ExamId inner join tblclass on tblclass.Id= tblexam.ClassId where tblexamschedule.BatchId=" + MyUser.CurrentBatchId+ "";
                    MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null)
                    {
                        GridExam.DataSource = MydataSet;
                        GridExam.DataBind();

                    }
                }
                else
                {
                    string sql = "select DISTINCT(ExamId) ,ExamName , tblclass.ClassName from tblstudentmark inner join tblexamschedule on tblstudentmark.ExamSchId=tblexamschedule.Id inner join tblexam on tblexam.Id=tblexamschedule.ExamId inner join tblclass on tblclass.Id= tblexam.ClassId where tblclass.Id=" + int.Parse(Drp_Class.SelectedValue) + " and tblexamschedule.BatchId=" + MyUser.CurrentBatchId + "";
                    MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null)
                    {
                        GridExam.DataSource = MydataSet;
                        GridExam.DataBind();
                       
                    }
                }
            }
            else
            {
                Lbl_msg.Text = "Try again later";
                this.MPE_MessageBox.Show();
            
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            
            DataSet _Daydataset = new DataSet();
            DataTable dt;
            //DataRow dr;
            //int i;
            _Daydataset.Tables.Add(new DataTable("Exam"));
            dt = _Daydataset.Tables["Exam"];

            dt.Columns.Add("ExamId");
            dt.Columns.Add("ExamName");
            dt.Columns.Add("ClassName");
           

                    int flag = 0;
                    foreach (GridViewRow gv in GridExam.Rows)
                    {
                       
                            CheckBox cb = (CheckBox)gv.FindControl("cb");
                            if (cb.Checked)
                            {

                                flag = 1;



                                BtXML.Visible = true;
                                GridSelected.Visible = true;
                             
                            }
                        }
                    if ((flag == 1) && (Dataexists()))
                    {
                        checkgrids();
                    }
                   
                    else  if (flag == 0)
                        {
                            Lbl_msg.Text = "Select an Exam";
                            this.MPE_MessageBox.Show();
                        }
                    else if (!Dataexists())
                        {
                            Lbl_msg.Text = "Exam Already Added";
                            this.MPE_MessageBox.Show();
                        }
                       



        }

        private bool Dataexists()
        {
            bool valid = true;
            string status = "";
            string value ;
            foreach (GridViewRow gv in GridExam.Rows)
            {
                                CheckBox cb = (CheckBox)gv.FindControl("cb");
                                if (cb.Checked)
                                {
                                    value = gv.Cells[1].Text.ToString();
                                    status = dataexistinlowergrid(value);
                                    if (status == "1")
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                  
            }
            
            return valid;
        }

        private string dataexistinlowergrid(string p)
        {
            string status="" ;
            foreach (GridViewRow gv in GridView1.Rows)
            {

                if (gv.Cells[0].Text.ToString()==p)
                {
                    status="1";
                }
                
            }
            return status;
        }
  
        private void checkgrids()
        {
            DataSet _Daydataset = new DataSet();
            DataTable dt;
            DataRow dr;
            //int i = 0;
            _Daydataset.Tables.Add(new DataTable("Container"));
            dt = _Daydataset.Tables["Container"];

            dt.Columns.Add("ExamId");
            dt.Columns.Add("ExamName");
            dt.Columns.Add("ClassName");

                foreach (GridViewRow gv in GridView1.Rows)
                {

                    dr = _Daydataset.Tables["Container"].NewRow();
                    dr["ExamId"] = gv.Cells[0].Text.ToString();
                    dr["ExamName"] = gv.Cells[1].Text.ToString();
                    dr["ClassName"] = gv.Cells[2].Text.ToString();
                    _Daydataset.Tables["Container"].Rows.Add(dr);
                }
                foreach (GridViewRow gv in GridExam.Rows)
                {
                    CheckBox cb = (CheckBox)gv.FindControl("cb");
                 if (cb.Checked)
                    {
                        dr = _Daydataset.Tables["Container"].NewRow();
                        dr["ExamId"] = gv.Cells[1].Text.ToString();
                        dr["ExamName"] = gv.Cells[2].Text.ToString();
                        dr["ClassName"] = gv.Cells[3].Text.ToString();
                        _Daydataset.Tables["Container"].Rows.Add(dr);

                    }
                }

                GridView1.DataSource = _Daydataset;
                GridView1.DataBind();
            
         
        }

        protected void BtXML_Click(object sender, EventArgs e)
        {
           

            StringBuilder sql = new StringBuilder("SELECT * FROM tblstudentmark where ExamSchId=1 ");

            
            StringBuilder Marks = new StringBuilder("");
           
            int i = 1;
            int j = 1;
            DataSet marksDataset;
            DataSet myDs = new DataSet();
            OdbcDataAdapter Od = new OdbcDataAdapter();
            marksDataset = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql.ToString());
            if (marksDataset != null && marksDataset.Tables != null && marksDataset.Tables[0].Rows.Count > 0)
            {

                
                Marks.Append("<exam name=\"sem1\" class=\"five a\">");

                while (i <= 10)
                {
                    Marks.Append("<student name=\"jerin\" admissionNo=\"123\">");

                    while (j <= 10)
                    {
                        Marks.Append("<subject name=\"English\" maxmark=\"100\" passmark=\"50\" obtained=\"75\">");
                        Marks.Append("</subject>");

                        j++;
                    }
                    j = 1;
                    Marks.Append("</student>");
                    i++;
                }

                Marks.Append("</exam>");

                
            }

          
            FileStream myFs = new FileStream("c:\\myXmlData.xml", FileMode.OpenOrCreate, FileAccess.Write);
           
            StreamWriter sw = new StreamWriter(myFs);
            sw.Write(Marks);
            sw.Close();
           
        }
        protected void Grd_ExamDelete(object sender, GridViewDeleteEventArgs e)
        {
            string _temp =GridView1.Rows[e.RowIndex].Cells[0].Text.ToString();
            try
            {
                if (GridView1.Rows.Count > 1)
                {

                    GridView1.DataSource = Delete_Row(_temp);
                    GridView1.DataBind();
                

                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                 

                }

            }

            catch
            {
             

            }

        }

        private DataSet Delete_Row(string _temp)
        {
            DataSet _Daydataset = new DataSet();
            DataTable dt;
            DataRow dr;
            
            _Daydataset.Tables.Add(new DataTable("Exam"));
            dt = _Daydataset.Tables["Exam"];
          

            dt.Columns.Add("ExamId");
            dt.Columns.Add("ExamName");
            dt.Columns.Add("ClassName");
          
            foreach (GridViewRow gv in GridView1.Rows)
            {
                if (gv.Cells[0].Text.ToString() != _temp)
                {
                   
                    dr = _Daydataset.Tables["Exam"].NewRow();
                  
                    dr["ExamId"] = gv.Cells[0].Text.ToString();
                    dr["ExamName"] = gv.Cells[1].Text.ToString();
                    dr["ClassName"] = gv.Cells[2].Text.ToString();
                    _Daydataset.Tables["Exam"].Rows.Add(dr);
                }
            }
            return _Daydataset;
        }

        protected void BtXML_Click1(object sender, EventArgs e)
        {
            int ExmSchId = -1;
            StringBuilder Marks = new StringBuilder("");
          
            DataSet myDs = new DataSet();
            OdbcDataAdapter Od = new OdbcDataAdapter();
            string _temp;
            //int ExamCount = 0;
                Marks.Append("<result>");
                foreach (GridViewRow gv in GridView1.Rows)
                {
                    ExmSchId = MyExamMang.getScheduleId(int.Parse(gv.Cells[0].Text.ToString()));
                    Marks.Append("<exam name=\"" + gv.Cells[1].Text.ToString() + "\" class=\"" + gv.Cells[2].Text.ToString() + "\" scheduleId =\"" +ExmSchId +"\">");
                    string sql = "select DISTINCT(tblstudent.Id), tblstudent.StudentName from tblstudentmark inner join tblstudent on tblstudent.Id=tblstudentmark.StudId inner join tblexamschedule on tblexamschedule.Id=tblstudentmark.ExamSchId inner join tblexammark on tblexammark.ExamSchId=tblexamschedule.Id where tblexamschedule.ExamId=" + int.Parse(gv.Cells[0].Text.ToString()) + " and tblstudent.`Status` = 1";
                    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    while(MyReader.Read())
                    {
                        Marks.Append("<Student name=\"" + MyReader.GetValue(1).ToString() + "\">");

                        //sql = "select DISTINCT(tblsubjects.Id), subject_name ,PassMark,MaxMark  from tblsubjects inner join tblexamsubjectmap on tblexamsubjectmap.SubjectId=tblsubjects.Id inner join tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id = tblexammark.ExamSchId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblexamschedule.ExamId=" + int.Parse(gv.Cells[0].Text.ToString()) + " and tblstudentmark.StudId=" + int.Parse(MyReader.GetValue(0).ToString()) + "";


                        sql = "select tblsubjects.subject_name, tblexammark.SubjectId, tblexammark.MarkColumn, tblexammark.MaxMark, tblexammark.PassMark from tblexammark inner join tblsubjects on tblsubjects.Id=tblexammark.SubjectId where tblexammark.ExamSchId=" + ExmSchId + " order by tblexammark.SubjectOrder";
                      _Mydata_Subjects = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        if (_Mydata_Subjects != null && _Mydata_Subjects.Tables != null && _Mydata_Subjects.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr_subject in _Mydata_Subjects.Tables[0].Rows)
                                    {
                                        _temp = MyExamMang.GetMarkfromColumn(int.Parse(MyReader.GetValue(0).ToString()), int.Parse(gv.Cells[0].Text.ToString()), MyUser.CurrentBatchId, dr_subject[2].ToString());

                                        Marks.Append("<Subject Id=\"" + dr_subject[1].ToString() + "\" SubjectName=\"" + dr_subject[0].ToString() + "\" PassMark=\"" + dr_subject[4].ToString() + "\" MaxMark=\"" + dr_subject[3].ToString() + "\" MarksObtained=\"" + _temp + "\" >");
                                        Marks.Append("</Subject>");

                                    }

                             }
                        sql = "select DISTINCT(TotalMark),Avg,Grade,Result,Rank  from tblsubjects inner join tblexamsubjectmap on tblexamsubjectmap.SubjectId=tblsubjects.Id inner join tblexammark on tblexammark.SubjectId=tblsubjects.Id inner join tblexamschedule on tblexamschedule.Id = tblexammark.ExamSchId inner join tblstudentmark on tblstudentmark.ExamSchId = tblexamschedule.Id where tblexamschedule.ExamId=" + int.Parse(gv.Cells[0].Text.ToString()) + " and tblstudentmark.StudId=" + int.Parse(MyReader.GetValue(0).ToString()) + " order by tblexammark.SubjectOrder";
                        MyReader1 = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        while (MyReader1.Read())
                        {
                            Marks.Append("<OverallPerformance TotalMark=\"" + MyReader1.GetValue(0).ToString() + "\" Average=\"" + MyReader1.GetValue(1).ToString() + "\" Grade=\"" + MyReader1.GetValue(2).ToString() + "\" Result=\"" + MyReader1.GetValue(3).ToString() + "\" Rank=\"" + MyReader1.GetValue(4).ToString() + "\" >");
                            Marks.Append("</OverallPerformance>");
                        }
                        Marks.Append("</Student>");
                    }
                    Marks.Append("</exam>");
                }

                Marks.Append("</result>");
                
           
            FileStream myFs = new FileStream("c:\\myXmlData.xml", FileMode.OpenOrCreate, FileAccess.Write);
            
            StreamWriter sw = new StreamWriter(myFs);
            sw.Write(Marks);
            sw.Close();
            
        }

        protected void GridExam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
