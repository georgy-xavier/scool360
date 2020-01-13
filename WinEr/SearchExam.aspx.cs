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
    public partial class WebForm18 : System.Web.UI.Page
    {
        private WinErSearch MySearchMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }

            else if (!MyUser.HaveActionRignt(49))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    string _MenuStr;
                    _MenuStr = MySearchMang.GetSearchMangMenuString(MyUser.UserRoleId);
                    this.ConfigMenu.InnerHtml = _MenuStr;
                    _MenuStr = MyUser.GetDetailsString(49);
                    this.ActionInfo.InnerHtml = _MenuStr;
                    LoadBatchToDrpList();
                    if (int.Parse(Drp_Batch1.SelectedValue.ToString()) != -1)
                    {
                        LoadClasstoDrpList();
                        LoadExmTypeToDrpList();
                    }
                    //some initialisations
                    Pnl_examlist.Visible = false;
                }
            }

        }

        private void LoadExmTypeToDrpList()
        {
            Drp_ExamType.Items.Clear();
            string sql = "SELECT Id,sbject_type FROM tblsubject_type";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_ExamType.Items.Add(new ListItem("Any", "0"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ExamType.Items.Add(li);
                }
            }
            else
            {
                Drp_ExamType.Items.Add(new ListItem("No ExamType Found", "-1"));
                Drp_Batch1.Enabled = false;
                Drp_Class.Enabled = false;
                Drp_ExamType.Enabled = false;
                Txt_ExamName.Enabled = false;
                Btn_Search.Enabled = false;
                Lbl_msg.Text = "No Exam is currently available";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadClasstoDrpList()
        {
            string sql = "";
            Drp_Class.Items.Clear();
            if (int.Parse(Drp_Batch1.SelectedValue.ToString()) == 0)
            {
                sql = "SELECT Distinct tblclass.ClassName,tblclass.Id FROM tblstudentclassmap INNER JOIN tblclass ON tblstudentclassmap.ClassId = tblclass.Id ";

            }
            else
            {
                sql = "SELECT Distinct tblclass.ClassName,tblclass.Id FROM tblstudentclassmap INNER JOIN tblclass ON tblstudentclassmap.ClassId = tblclass.Id WHERE tblstudentclassmap.BatchId=" + int.Parse(Drp_Batch1.SelectedValue.ToString()) + "";

            }
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Class.Items.Add(new ListItem("Any", "0"));
                Drp_Class.Enabled = true;               
                Drp_ExamType.Enabled = true;
                Txt_ExamName.Enabled = true;               
                Btn_Search.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                Drp_Class.Enabled = false;            
                Drp_ExamType.Enabled = false;
                Txt_ExamName.Enabled = false;
                Btn_Search.Enabled = false;
                Drp_Class.Items.Add(new ListItem("No class found", "-1"));
                Lbl_msg.Text = "No class is present";
                this.MPE_MessageBox.Show();
            }
        }

        private void LoadBatchToDrpList()
        {

            Drp_Batch1.Items.Clear();
            string sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Batch1.Items.Add(new ListItem("Any", "0"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Batch1.Items.Add(li);
                }
            }
            else
            {
                Drp_Batch1.Items.Add(new ListItem("No Batch Found", "-1"));
                Drp_Batch1.Enabled = false;
                Drp_Class.Enabled = false;            
                Drp_ExamType.Enabled = false;
                Txt_ExamName.Enabled = false;
                Btn_Search.Enabled = false;
                Lbl_msg.Text = "No batch is currently available";
                this.MPE_MessageBox.Show();
            }
        }
       

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WinSchoolHome.aspx");
        }

        protected void Drp_Batch1_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadClasstoDrpList();
            Grd_Exam.DataSource = null;
            Grd_Exam.DataBind();
            Pnl_examlist.Visible = false;
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            Grd_Exam.PageIndex = 0;
            FillGrid();
        }

        private void FillGrid()
        {
        
            string ExamName, sql;
            ExamName = Txt_ExamName.Text.Trim();

            if (ExamName != "" || Drp_Batch1.SelectedValue != "0" || Drp_Class.SelectedValue != "0" || Drp_ExamType.SelectedValue != "-1" )
            {
                Grd_Exam.Columns[0].Visible = true;
                sql = "SELECT tblexam.Id,tblexam.ExamName,tblbatch.BatchName FROM tblexamschedule INNER JOIN tblexam ON tblexamschedule.ExamId = tblexam.Id INNER JOIN tblbatch ON tblexamschedule.BatchId = tblbatch.Id";
                
                if (Drp_Batch1.SelectedValue != "0")
                {
                    sql = sql + "  AND tblbatch.Id=" + Drp_Batch1.SelectedValue;
                }
                if (Drp_Class.SelectedValue != "0")
                {
                    sql = sql + " INNER JOIN tblclass ON tblexam.ClassId = tblclass.Id AND tblclass.Id=" + Drp_Class.SelectedValue;
                }
                if (Drp_ExamType.SelectedValue != "0")
                {
                    sql = sql + " INNER JOIN tblsubject_type ON tblexam.TypeId = tblsubject_type.Id AND tblsubject_type.Id=" + Drp_ExamType.SelectedValue;
                }
                sql = sql + " WHERE tblexamschedule.Status ='Completed'";
                if (ExamName != "")
                {
                    sql = sql + " AND tblexam.ExamName LIKE '%" + ExamName + "%'";
                }
                MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_Exam.DataSource = MydataSet;
                    Grd_Exam.DataBind();
                    Grd_Exam.Columns[0].Visible = false;
                    FillExamDeatils();
                    Pnl_examlist.Visible = true;
                }
                else
                {
                    Grd_Exam.DataSource = null;
                    Grd_Exam.DataBind();
                    Pnl_examlist.Visible = false;
                    Lbl_msg.Text = "No exams found";
                    this.MPE_MessageBox.Show();

                }

            }
            else
            {
                Grd_Exam.DataSource = null;
                Grd_Exam.DataBind();
                Pnl_examlist.Visible = false;
                Lbl_msg.Text = "Enter name";
                this.MPE_MessageBox.Show();
            }

        }

        private void FillExamDeatils()
        {
 

            foreach (GridViewRow gv in Grd_Exam.Rows)
            {


                Label Lbl_ExamType = (Label)gv.FindControl("Lbl_ExamType");
                Label Lbl_Batch = (Label)gv.FindControl("Lbl_Batch");

                Lbl_ExamType.Text = MySearchMang.GetExamType(int.Parse(gv.Cells[1].Text.ToString()));
                //Lbl_Batch.Text = MySearchMang.GetExamBatch(int.Parse(gv.Cells[1].Text.ToString()));
                
            }
       
        }

        protected void Grd_Exam_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Exam, "Select$" + e.Row.RowIndex);
            }

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
             int i_SelectedExam = int.Parse(Grd_Exam.SelectedRow.Cells[1].Text.ToString());
             string BatchName = Grd_Exam.SelectedRow.Cells[3].Text.ToString().Trim();
             int Bach_Id=MySearchMang.GetBatchID(BatchName);

             Response.Redirect("ExamReportDetails.aspx?ExamId=" + i_SelectedExam + " &BatchId=" + Bach_Id + "");
        }
        protected void Grd_Exam_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Exam.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        
    }
}
