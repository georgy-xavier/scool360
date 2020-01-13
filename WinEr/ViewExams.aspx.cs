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
public partial class ViewExams : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyExamMang = MyUser.GetExamObj();
        if (MyExamMang == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
                //LoadExamType();
                LoadExamFrequency();
                viewexamlist.Visible = false;
                //LoadExams();                
                //some initlization
            }
        }
    }

    //private void LoadExamType()
    //{
    //    Drp_Exam_Type.Items.Clear();
    //    string sql = "SELECT Id, sbject_type FROM tblsubject_type";
    //    MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        Drp_Exam_Type.Items.Add(new ListItem("ALL", "0"));
    //        while (MyReader.Read())
    //        {
    //            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
    //            Drp_Exam_Type.Items.Add(li);
    //        }
    //        Drp_Exam_Type.SelectedIndex = 0;
    //    }
        
        
    //}

    private void LoadExamFrequency()
    {
        Drp_Exam_Frequency.Items.Clear();
        string sql = "SELECT Id, Name FROM tblfrequency where Name != 'Single Payment'";
        MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Drp_Exam_Frequency.Items.Add(new ListItem("ALL", "0"));
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Exam_Frequency.Items.Add(li);
            }
            Drp_Exam_Frequency.SelectedIndex = 0;
        }
        
    }

    protected void Btn_Search_Click(object sender, EventArgs e)
    {

        viewexamlist.Visible = true;
        LoadExams();  


    }

    //protected void Drp_Exam_Type_SelectedIndexChanged(object sender, EventArgs e)
    //{



    //}

    //protected void Drp_Exam_Frequency_SelectedIndexChanged(object sender, EventArgs e)
    //{



    //}

    protected void Grd_ExamList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
            }
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_ExamList, "Select$" + e.Row.RowIndex);
        }
    }

    protected void LoadExams()
    {
        string sql = "";
        //string examtypeid = Drp_Exam_Type.SelectedValue;
        string examfrequencyid = Drp_Exam_Frequency.SelectedValue;
        if (examfrequencyid == "0")
        {
            if (Txt_ExamName.Text == "")
            {
                sql = "SELECT tblexammaster.Id, tblexammaster.ExamName, tblfrequency.Name , tblsubject_type.sbject_type from tblexammaster inner JOIN tblfrequency on tblfrequency.Id = tblexammaster.PeriodTypeId inner join tblsubject_type on tblsubject_type.Id = tblexammaster.ExamTypeId where tblexammaster.Status=1";

            }
            else
            {
                sql = "SELECT tblexammaster.Id, tblexammaster.ExamName, tblfrequency.Name , tblsubject_type.sbject_type from tblexammaster inner JOIN tblfrequency on tblfrequency.Id = tblexammaster.PeriodTypeId inner join tblsubject_type on tblsubject_type.Id = tblexammaster.ExamTypeId where tblexammaster.Status=1 and tblexammaster.ExamName='" + Txt_ExamName.Text + "' ";

            }
        }
        else
        {
             sql = "SELECT tblexammaster.Id, tblexammaster.ExamName, tblfrequency.Name , tblsubject_type.sbject_type from tblexammaster inner JOIN tblfrequency on tblfrequency.Id = tblexammaster.PeriodTypeId inner join tblsubject_type on tblsubject_type.Id = tblexammaster.ExamTypeId where tblexammaster.Status=1 and  tblexammaster.PeriodTypeId=" + examfrequencyid + " ";
        }
        MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_ExamList.Columns[1].Visible = true;
            Grd_ExamList.DataSource = MydataSet;
            Grd_ExamList.DataBind();
            Grd_ExamList.Columns[1].Visible = false;
            //Pnl_ExamList.Visible = true;
            Lbl_note.Text = "";
        }
        else
        {
            Grd_ExamList.DataSource = null;
            Grd_ExamList.DataBind();
            Lbl_note.Text = "No Exams Found";
           // Pnl_ExamList.Visible = false;
            //this.MPE_MessageBox.Show();
        }
    }

   
    protected void Grd_ExamList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i_SelectedExamId = int.Parse(Grd_ExamList.SelectedRow.Cells[1].Text.ToString());
        Session["ExamId"] = i_SelectedExamId;
        Response.Redirect("ExamDetails.aspx");
    }



    protected void Grd_ExamList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grd_ExamList.PageIndex = e.NewPageIndex;
        LoadExams();
    }

}       