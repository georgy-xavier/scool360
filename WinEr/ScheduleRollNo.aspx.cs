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
public partial class ScheduleRollNo : System.Web.UI.Page
{
    private ClassOrganiser MyClassMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
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

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ClassId"] == null)
        {
            Response.Redirect("LoadClassDetails.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyClassMang = MyUser.GetClassObj();
        if (MyClassMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(27))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
                this.SubClassMenu.InnerHtml = _MenuStr;
                LoadDetails();
                //some initlization

            }
        }

    }

    private void LoadDetails()
    {
        Load_Drp_GenType();
        lbl_Batch.Text = MyUser.CurrentBatchName;
        lbl_Clasname.Text = MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString()));
        Panel1.Visible = false;
        Panel2.Visible = true;
        AddStudentDetails();
        FillRollNo();


    }

    private void Load_Drp_GenType()
    {
        Drp_GenType.Items.Clear();
        string sql = "SELECT Id,SchType FROM tblrollnotypes";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                Drp_GenType.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
            }
            Drp_GenType.SelectedIndex = 0;
            Drp_GenType.SelectedValue = GetSaved_RollGenType(int.Parse(Session["ClassId"].ToString())).ToString();
        }
    }

    private int GetSaved_RollGenType(int _ClassId)
    {
        int _id = 0;
        string sql = "SELECT RollNoSchTypeId FROM tblclass WHERE Id=" + _ClassId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(),out _id);
        }
        return _id;

    }

    private void FillRollNo()
    {
        string sql = "SELECT tblstudentclassmap.RollNo from tblstudentclassmap INNER JOIN tblstudent on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {

            foreach (GridViewRow gv in Grd_SchRollNo.Rows)
            {
                MyReader.Read();
                TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
                TxtRollNumber.Text = MyReader.GetValue(0).ToString();
                if (TxtRollNumber.Text == "-1")
                {
                    TxtRollNumber.Text = "";
                }
               

            }
        }
    }

    private void AddStudentDetails()
    {
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_SchRollNo.Columns[0].Visible = true;
            Grd_SchRollNo.DataSource = MydataSet;
            Grd_SchRollNo.DataBind();
            Grd_SchRollNo.Columns[0].Visible = false;
            Pnl_studlist.Visible = true;
          
        }
        else
        {
            Btn_AutoGenerate.Enabled = false;
            Btn_undo.Enabled = false;
            Btn_Update.Enabled = false;
            Drp_GenType.Enabled = false;
            Pnl_studlist.Visible = false;
            Lbl_msg.Text = "No Students found in the Class";
            this.MPE_MessageBox.Show();
        }
    }
    protected void Btn_AutoGenerate_Click(object sender, EventArgs e)
    {
        if (Drp_GenType.SelectedValue == "0")
        {
            GenereteRlNoByName();
        }
        else if (Drp_GenType.SelectedValue == "1")
        {
            GenereteRlNoByAdminNo();
        }
        else if (Drp_GenType.SelectedValue == "2" || Drp_GenType.SelectedValue == "3")
        {
            GenereteRlNoByGender(Drp_GenType.SelectedValue);
        }

        Drp_GenType.Enabled = false;
    }

    private void GenereteRlNoByAdminNo()
    {
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by  LENGTH(tblstudent.AdmitionNo),tblstudent.AdmitionNo ASC";
         MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_SchRollNo.Columns[0].Visible = true;
            Grd_SchRollNo.DataSource = MydataSet;
            Grd_SchRollNo.DataBind();
            Grd_SchRollNo.Columns[0].Visible = false;
        
        }
        int _Rollno = 1;
        foreach (GridViewRow gv in Grd_SchRollNo.Rows)
        {
            TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
            TxtRollNumber.Text = _Rollno.ToString();
            _Rollno++;

        }

       
    }

    private void GenereteRlNoByGender(string Type)
    {
        string SubStr = "DESC";
        if (Type == "3")
        {
            SubStr = "ASC";
        }

        string sql = "SELECT tblstudent.Id,trim(tblstudent.StudentName) as StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudent.Sex " + SubStr + " ,tblstudent.StudentName ASC";
        MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_SchRollNo.Columns[0].Visible = true;
            Grd_SchRollNo.DataSource = MydataSet;
            Grd_SchRollNo.DataBind();
            Grd_SchRollNo.Columns[0].Visible = false;
      
        }
        int _Rollno = 1;
        foreach (GridViewRow gv in Grd_SchRollNo.Rows)
        {
            TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
            TxtRollNumber.Text = _Rollno.ToString();
            _Rollno++;

        }
        
    }

    private void GenereteRlNoByName()
    {
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudent.StudentName ASC";   
        MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_SchRollNo.Columns[0].Visible = true;
            Grd_SchRollNo.DataSource = MydataSet;
            Grd_SchRollNo.DataBind();
            Grd_SchRollNo.Columns[0].Visible = false;
         
        }
        int _Rollno = 1;
        foreach (GridViewRow gv in Grd_SchRollNo.Rows)
        {
            TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
            TxtRollNumber.Text = _Rollno.ToString();
            _Rollno++;

        }
    }
    protected void Btn_undo_Click(object sender, EventArgs e)
    {
        Drp_GenType.Enabled = true;
        AddStudentDetails();
        FillRollNo();
 

    }
    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        string _message;
        if (ValidData(out _message))
        {
            UpadteRollNo();
            Drp_GenType.Enabled = true;
        }
        else
        {
            Lbl_msg.Text = _message;
            this.MPE_MessageBox.Show();
        }
    }

    private void UpadteRollNo()
    {
        try
        {
            MyClassMang.CreateTansationDb();
            DBLogClass _newDBLogClass = new DBLogClass(MyClassMang.m_TransationDb);
            foreach (GridViewRow gv in Grd_SchRollNo.Rows)
            {
                TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
                MyClassMang.UpdateRollNo(int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId, int.Parse(TxtRollNumber.Text.ToString()), int.Parse(gv.Cells[0].Text.ToString()));
                _newDBLogClass.LogToDb(MyUser.UserName, "Update RollNo", "The RollNo Of Student " + gv.Cells[1].Text.ToString() + "  is changed and updated.", 1);

            }
            Update_RollTypeINClass(int.Parse(Session["ClassId"].ToString()), Drp_GenType.SelectedValue);
            Lbl_msg.Text = "Successfully Saved";
            MyClassMang.EndSucessTansationDb();
        }
        catch (Exception ex)
        {
            MyClassMang.EndFailTansationDb();
            Lbl_msg.Text = "Error while saving. Message : "+ex.Message;
        }
       
        this.MPE_MessageBox.Show();
    }

    private void Update_RollTypeINClass(int _ClassId, string _TypeId)
    {
        string sql = "UPDATE tblclass SET RollNoSchTypeId = " + _TypeId + "  WHERE Id=" + _ClassId;
        MyClassMang.m_TransationDb.ExecuteQuery(sql);
    }

    private bool ValidData(out string _Message)
    {
        bool _valid = true;
        _Message = "";
        int _gridcount = Grd_SchRollNo.Rows.Count;
        int[] numbers = new int[_gridcount];
        int _i=0,_j,_k;
        try
        {
            foreach (GridViewRow gv in Grd_SchRollNo.Rows)
            {

                TextBox TxtRollNumber = (TextBox)gv.FindControl("Txt_RollNumber");
                if (TxtRollNumber.Text.Trim() == "")
                {
                    _Message = "RollNo Cannot be Empty";
                    _valid = false;
                    break;
                }
                else
                {
                    numbers[_i] = int.Parse(TxtRollNumber.Text.ToString());
                }
                _i++;

            }
            if (_valid)
            {
                for (_j = 0; _j < _gridcount; _j++)
                {
                    for (_k = _j + 1; _k < _gridcount; _k++)
                    {
                        if (numbers[_j] == numbers[_k])
                        {
                            _Message = "RollNo Cannot be repeated";
                            _valid = false;

                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            _Message = e.Message;
            _valid = false;
        }
        return _valid;
    }
}
