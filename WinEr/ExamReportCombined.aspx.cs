
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

public partial class ExamReportCombined : System.Web.UI.Page
{
    private ExamManage MyExamMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;

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
        MyExamMang = MyUser.GetExamObj();
        if (MyExamMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(250))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                LoadClasses();
                PageInitials();
            }
        }

    }

    private void PageInitials()
    {

       
        LoadMainExams();
        FillCombinations();

        Grd_Exam.DataSource = null;
        Grd_Exam.DataBind();
        grd_save.DataSource = null;
        grd_save.DataBind();
        btn_add.Enabled = false;
        lbl_exams.Visible = false;
        lbl_combined.Visible = false;
        Lnk_DeleteCombExam.Visible = false;
        imgbtn_delCmb.Visible = false;
    }

    private void LoadClasses()
    {
        dropClassName.Items.Clear();
        MyReader = MyExamMang.getclass();
        if (MyReader.HasRows)
        {
            ListItem li1 = new ListItem("Select Class", "0");
            dropClassName.Items.Add(li1);
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                dropClassName.Items.Add(li);
            }
        }
        else
        {
            ListItem li1 = new ListItem("No Class", "0");
            dropClassName.Items.Add(li1);
        }

    }

    private void LoadMainExams()
    {
            Drp_exam.Items.Clear();
            MyReader = MyExamMang.getExams();
            if (MyReader.HasRows)
            {
                ListItem li1 = new ListItem("Select Exam", "0");
                Drp_exam.Items.Add(li1);
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_exam.Items.Add(li);
                }
            }
            else
            {
                ListItem li1 = new ListItem("No Exams", "0");
                Drp_exam.Items.Add(li1);
            } 
    }

    protected void drp_exams_changed(object sender, EventArgs e)
    {
        if (grd_save.Rows.Count > 0)
        {
            lbl_combined.Visible = false;
        }

        Fill_Exam_Grid();
        if (Grd_Exam.Rows.Count > 0)
        {
            Grd_Exam.Visible = true;
            btn_add.Enabled = true;
            lbl_exams.Visible = false;
        }
        else
        {
            Grd_Exam.Visible = false;
            btn_add.Enabled = false;
            lbl_exams.Visible = true;
        }

    }

    private void Fill_Exam_Grid()
    {
        Grd_Exam.Columns[1].Visible = true;
        Grd_Exam.Columns[4].Visible = true;

        Grd_Exam.DataSource = null;
        Grd_Exam.DataBind();
        int _ExmId = int.Parse(Drp_exam.SelectedValue.ToString());

        DataSet MyDataSet = new DataSet();
        DataTable dt;
        DataRow dr;
        MyDataSet.Tables.Add(new DataTable("tblexams"));
        dt = MyDataSet.Tables["tblexams"];
        dt.Columns.Add("Id");
        dt.Columns.Add("ExamName");
        dt.Columns.Add("ExamPeriod");
        dt.Columns.Add("Abbreviation");
        MyReader = null;
        MyReader = MyExamMang.GetExamsWithPeriod(_ExmId);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                dr = MyDataSet.Tables["tblexams"].NewRow();
                dr["Id"] = MyReader.GetValue(0).ToString();
                dr["ExamName"] = Drp_exam.SelectedItem.Text.ToString();
                dr["ExamPeriod"] = MyReader.GetValue(1).ToString();
                dr["Abbreviation"] = Drp_exam.SelectedItem.Text.ToString() + " " + MyReader.GetValue(1).ToString();
                MyDataSet.Tables["tblexams"].Rows.Add(dr);
            }
        }
        Grd_Exam.DataSource = MyDataSet;
        Grd_Exam.DataBind();
        foreach (GridViewRow gvr in Grd_Exam.Rows)
        {
            TextBox txt = (TextBox)gvr.FindControl("txt_Abbrev");
            txt.Text = gvr.Cells[4].Text.ToString();
        }
        Grd_Exam.Columns[1].Visible = false;
        Grd_Exam.Columns[4].Visible = false;


        HideExamsifAlreadyPresent();
    }

    public void drp_class_changed(object sender, EventArgs e)
    {
        PageInitials();
    }

    private void FillCombinations()
    {
        int classid = 0;
        int.TryParse(dropClassName.SelectedItem.Value.ToString(), out classid);
        Lnk_DeleteCombExam.Visible = false;
        imgbtn_delCmb.Visible = false;
        drp_mainexam.Items.Clear();
        MyReader = null;
        if (classid != 0)
        {
            drp_mainexam.Enabled = true;
            MyReader = MyExamMang.getMainExams(classid);
            if (MyReader.HasRows)
            {
                ListItem li1 = new ListItem("Select Combination", "0");
                drp_mainexam.Items.Add(li1);
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    drp_mainexam.Items.Add(li);
                }
            }
            else
            {
                ListItem li1 = new ListItem("No Combined exams", "0");
                drp_mainexam.Items.Add(li1);
            }
        }
        else
        {
            ListItem li1 = new ListItem("Select Class", "0");
            drp_mainexam.Items.Add(li1);
            drp_mainexam.Enabled = false;
        }

    }

    protected void lnk_new_Click(object sender, EventArgs e)
    {
        if (int.Parse(dropClassName.SelectedValue.ToString()) != 0)
        {
            lbl_CombineError.Text = "";
            txt_combination.Text = "";
            MPE_newCombine.Show();
        }
    }

    protected void imgbtn_newCmb_Click(object sender, ImageClickEventArgs e)
    {
        if (int.Parse(dropClassName.SelectedValue.ToString()) != 0)
        {
            lbl_CombineError.Text = "";
            txt_combination.Text = "";
            MPE_newCombine.Show();
        }
    }

    protected void btn_newComb_click(object sender, EventArgs e)
    {
        string crUser = MyUser.UserName;
        DateTime crDate = DateTime.Now;
        int classId = 0;
        int.TryParse(dropClassName.SelectedItem.Value.ToString(),out classId);
       
        if (classId != 0)
        {
            
            string combination = txt_combination.Text.Trim().ToString(), _msg = "";
            if (combination != "")
            {
                MyExamMang.AddNewCombination(classId, combination, crUser, crDate, out _msg);
                if (_msg == "Already Exists..!")
                {
                    lbl_CombineError.Text = _msg;
                    MPE_newCombine.Show();
                }
                else
                {
                    PageInitials();
                    WC_MessageBox.ShowMssage(_msg);
                }
            }
            else
            {
                lbl_CombineError.Text = "Enter Combined Exam Name....!";
                MPE_newCombine.Show();
            }
        }
        else
        { 
            lbl_CombineError.Text = "Select Class...!";
            MPE_newCombine.Show();
        }
    }

    protected void drp_mainexam_SelectedIndexChanged(object sender, EventArgs e)
    {
        Drp_exam.Enabled = false;
        if (int.Parse(drp_mainexam.SelectedValue.ToString()) > 0)
        {           
            Drp_exam.Enabled = true;
            Grd_Exam.Enabled = true;
            int cmbExmID=0,classId = int.Parse(dropClassName.SelectedItem.Value.ToString());
            int.TryParse(drp_mainexam.SelectedItem.Value.ToString(), out cmbExmID);
            LoadmainGrd(classId, cmbExmID);
            Lnk_DeleteCombExam.Visible = true;
            imgbtn_delCmb.Visible = true;
        }
        else
        {
            Lnk_DeleteCombExam.Visible = false;
            imgbtn_delCmb.Visible=false;

            grd_save.Columns[0].Visible = true;

            grd_save.DataSource = null;
            grd_save.DataBind();

            grd_save.Columns[0].Visible = false;
        }
    }

    private void LoadmainGrd(int classId, int cmbExmID)
    {
        MyReader = null;
        grd_save.Columns[0].Visible = true;

        grd_save.DataSource = null;
        grd_save.DataBind();

        DataSet MyDataSet = new DataSet();
        DataTable dt;
        DataRow dr;
        MyDataSet.Tables.Add(new DataTable("tblexams"));
        dt = MyDataSet.Tables["tblexams"];
        dt.Columns.Add("Id");
        dt.Columns.Add("ExamName");
        dt.Columns.Add("ExamId");
        dt.Columns.Add("ExamPeriod");
        dt.Columns.Add("Abbreviation");

        MyReader = null;
        MyReader = MyExamMang.CombinedExamDtls(classId, cmbExmID);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                dr = MyDataSet.Tables["tblexams"].NewRow();
                dr["Id"] = MyReader.GetValue(0).ToString();
                dr["ExamName"] = MyReader.GetValue(1).ToString();
                dr["ExamPeriod"] = MyReader.GetValue(2).ToString();
                dr["Abbreviation"] = MyReader.GetValue(3).ToString();

                MyDataSet.Tables["tblexams"].Rows.Add(dr);
            }
        }

        ViewState["DSgrdSave"] = MyDataSet;
        grd_save.DataSource = MyDataSet;
        grd_save.DataBind();

        grd_save.Visible = true;
        grd_save.Columns[0].Visible = false;

        HideExamsifAlreadyPresent();
    }

    private void HideExamsifAlreadyPresent()
    {
        int flg = 0;
        Grd_Exam.Columns[1].Visible = true;
        Grd_Exam.Columns[4].Visible = true;
        DataSet tempDataSet = new DataSet();
        DataTable dt;
        DataRow dr;
        tempDataSet = new DataSet();
        tempDataSet.Tables.Add(new DataTable("tblexams"));
        dt = tempDataSet.Tables["tblexams"];
        dt.Columns.Add("Id");
        dt.Columns.Add("ExamName");
        dt.Columns.Add("ExamPeriod");
        dt.Columns.Add("Abbreviation");

     
        if (grd_save.Rows.Count > 0)
        {
            
            if (Grd_Exam.Rows.Count > 0)
            {
                foreach (GridViewRow gv in Grd_Exam.Rows)
                {
                    foreach (GridViewRow gvr in grd_save.Rows)
                    {
                        if ((gvr.Cells[1].Text.ToString() == gv.Cells[2].Text.ToString()) && (gvr.Cells[2].Text.ToString() == gv.Cells[3].Text.ToString()))
                        {
                            flg = 1;
                        }
                        else
                        {
                            
                        }
                    }
                    if (flg == 0)
                    {
                        dr = tempDataSet.Tables["tblexams"].NewRow();
                        dr["Id"] = gv.Cells[1].Text.ToString();
                        dr["ExamName"] = gv.Cells[2].Text.ToString();
                        dr["ExamPeriod"] = gv.Cells[3].Text.ToString();
                        dr["Abbreviation"] = gv.Cells[4].Text.ToString();
                        tempDataSet.Tables["tblexams"].Rows.Add(dr);
                    }
                    flg = 0;
                }
                    Grd_Exam.DataSource = tempDataSet;
                    Grd_Exam.DataBind();
                    foreach (GridViewRow gvr in Grd_Exam.Rows)
                    {
                        TextBox txt = (TextBox)gvr.FindControl("txt_Abbrev");
                        txt.Text = gvr.Cells[4].Text.ToString();
                    }
            } 

        }
        else
        {
 
        }

        Grd_Exam.Columns[1].Visible =false;
        Grd_Exam.Columns[4].Visible = false;
        grd_save.Columns[0].Visible = false;
    }

    protected void btn_add_click(object sender, EventArgs e)
    {
        int ClassId = 0, CmbExmId = 0, ExmId = 0, flg = 0;
        int.TryParse(dropClassName.SelectedValue.ToString(), out ClassId);
        int.TryParse(drp_mainexam.SelectedValue.ToString(), out CmbExmId);
        int.TryParse(Drp_exam.SelectedValue.ToString(), out ExmId);
        if ((ClassId > 0) && (ExmId > 0) && (CmbExmId > 0))
        {
            DataSet CmbExmDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            CmbExmDataSet = new DataSet();
            CmbExmDataSet.Tables.Add(new DataTable("tblcmbexms"));
            dt = CmbExmDataSet.Tables["tblcmbexms"];
            dt.Columns.Add("PeriodId");
            dt.Columns.Add("Abbreviation");
            dt.Columns.Add("ScheduleId");
            if (Grd_Exam.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in Grd_Exam.Rows)
                {
                    CheckBox chk = (CheckBox)gvr.FindControl("chk_examAdd");
                    if (chk.Checked == true)
                    {
                        flg = 1;
                        dr = CmbExmDataSet.Tables["tblcmbexms"].NewRow();
                        dr["PeriodId"] = gvr.Cells[1].Text.ToString();

                        TextBox txt = (TextBox)gvr.FindControl("txt_Abbrev");

                        if (txt.Text.Length > 20)
                        {
                            txt.Text = txt.Text.Substring(0, 19);
                        }
                        dr["Abbreviation"] = txt.Text.ToString();
                        dr["ScheduleId"] = MyExamMang.getScheduleId(int.Parse(Drp_exam.SelectedValue));
                        
                        CmbExmDataSet.Tables["tblcmbexms"].Rows.Add(dr);
                    }
                }
                if (flg == 1)
                {
                    MyExamMang.InsertCombinedExams(ClassId, CmbExmId, ExmId, CmbExmDataSet);
                    LoadmainGrd(ClassId, CmbExmId);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam combine", "Exams combinationed", 1);
                }
            }
        }
        else
        { }
    }

    protected void grd_save_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        int ClassId = 0, CmbExmId = 0;
        int.TryParse(dropClassName.SelectedValue.ToString(), out ClassId);
        int.TryParse(drp_mainexam.SelectedValue.ToString(), out CmbExmId);
        int id= int.Parse( grd_save.Rows[e.RowIndex].Cells[0].Text.ToString());
        
        MyExamMang.DeleteCombinedResult(id);
        LoadmainGrd(ClassId,CmbExmId);
        LoadMainExams();
        Grd_Exam.DataSource = null;
        Grd_Exam.DataBind();
        
        
    }

    protected void Lnk_DeleteCombExam_Click(object sender, EventArgs e)
    {
        lbl_DeleteError.Text = "";
        lbl_CombName.Text = drp_mainexam.SelectedItem.Text;
        MPE_Delete.Show();
    }

    protected void imgbtn_DelCmb_Click(object sender, ImageClickEventArgs e)
    {
        lbl_DeleteError.Text = "";
        lbl_CombName.Text = drp_mainexam.SelectedItem.Text;
        MPE_Delete.Show();
    }

    protected void Btn_DeleteYes_Click(object sender, EventArgs e)
    {
        int CombExmId = int.Parse(drp_mainexam.SelectedValue.ToString());
        MyExamMang.DeleteCombinedExam(CombExmId);
        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Exam combine", "Exam combination removed", 1);
        PageInitials();
        WC_MessageBox.ShowMssage("Exam Deleted..!");
    }

    protected void btn_cancel_click(object sender, EventArgs e)
    {
        PageInitials();
    }
   

}