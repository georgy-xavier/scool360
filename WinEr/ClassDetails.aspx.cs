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
using System.IO;
using WinBase;
using System.Collections.Generic;

//Web.Generic.DataGridTools 

public partial class ClassDetails : System.Web.UI.Page
{
    private ClassOrganiser MyClassMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    private Incident Myincident;
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
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ClassId"] == null)
        {
            Response.Redirect("LoadClassDetails.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        if (MyUser == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        MyClassMang = MyUser.GetClassObj();
        Myincident = MyUser.GetIncedentObj();
        if (MyClassMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
                this.SubClassMenu.InnerHtml = _MenuStr;
                //some initlization
                AddDeatilaToClass();
                //CheckViewIncidentRight(); 
                LoadStudents();

                if (!MyUser.HaveActionRignt(600))
                {
                    Img_StudentPreview.Visible = false;
                }
                if (!MyUser.HaveActionRignt(600))
                {
                    img_editstud.Visible = false;
                }

                if (MyUser.SELECTEDMODE == 1)
                {
                    this.TabPanel3.Visible = true;

                }
                else if (MyUser.SELECTEDMODE == 2)
                {
                    this.TabPanel3.Visible = false;
                }
            }
        }
    }
    protected void Page_init(object sender, EventArgs e)
    {

    }
    //private void CheckViewIncidentRight()
    //{
    //    if (MyUser.HaveActionRignt(72))
    //    {
    //        this.ClassIncident.InnerHtml = Myincident.LoadIncidenceData(int.Parse(Session["ClassId"].ToString()), "Class");
    //    }
    //    else
    //    {
    //        //this.TopTab.InnerHtml = "No incidents to view";
    //        this.Tab_Incident.Visible = false;
    //        this.Tabs.ActiveTabIndex = 1;
    //    }
    //}
    private void LoadStudents()
    {
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            Grd_Students.Columns[0].Visible = true;
            Grd_Students.DataSource = MydataSet;
            Grd_Students.DataBind();
            Grd_Students.Columns[0].Visible = false;
            Pnl_studlist.Visible = true;
            LoadOtherDetails();
        }
        else
        {


            Pnl_studlist.Visible = false;
            //Lbl_msg.Text = "No Students found in the Class";
            //this.MPE_MessageBox.Show();
        }
    }

    private void LoadOtherDetails()
    {
        foreach (GridViewRow gv in Grd_Students.Rows)
        {

            Image Img_stud = (Image)gv.FindControl("Img_studImage");
            Label Lbl_RollNumber = (Label)gv.FindControl("Lbl_RollNumber");

            Img_stud.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[0].Text) + "&type=StudentImage";
            //    MyUser.GetImageUrl("StudentImage", int.Parse(gv.Cells[0].Text.ToString()));
            Lbl_RollNumber.Text = MyClassMang.GetRollnumber(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId);
        }
    }

    private void AddDeatilaToClass()
    {
        string sql = "select tblclass.ClassName, tblstandard.Name from  tblclass inner join tblstandard on tblstandard.Id = tblclass.Standard where tblclass.Id=" + Session["ClassId"] + "";
        // string sql = "select cls.ClassName,cls.Standard,count(sc.StudentId) as totalStud from  tblclass cls inner join tblstudentclassmap sc on sc.ClassId=cls.Id  inner join tblstudent stud on stud.Id=sc.StudentId where cls.Id=" + Session["ClassId"] + " and stud.Status=1 AND sc.batchId=" + MyUser.CurrentBatchId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        MyReader.Read();
        if (MyReader.HasRows)
        {
            Lbl_Class.Text = MyReader.GetValue(0).ToString();
            Lbl_Standard.Text = MyReader.GetValue(1).ToString();

        }

        sql = "select count(tblstudentclassmap.StudentId) as totalStud from  tblclass  inner join tblstudentclassmap  on tblstudentclassmap.ClassId=tblclass.Id  inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId inner join tblstandard on tblstandard.Id = tblclass.Standard where tblclass.Id=" + Session["ClassId"] + " and tblstudent.Status=1 AND tblstudentclassmap.batchId=" + MyUser.CurrentBatchId;
        // string sql = "select cls.ClassName,cls.Standard,count(sc.StudentId) as totalStud from  tblclass cls inner join tblstudentclassmap sc on sc.ClassId=cls.Id  inner join tblstudent stud on stud.Id=sc.StudentId where cls.Id=" + Session["ClassId"] + " and stud.Status=1 AND sc.batchId=" + MyUser.CurrentBatchId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        MyReader.Read();
        if (MyReader.HasRows)
        {

            Lbl_NoStud.Text = MyReader.GetValue(0).ToString();
        }
        sql = "select tbluser.SurName from tbluser inner join tblclass on tblclass.ClassTeacher= tbluser.Id  where tblclass.Id=" + Session["ClassId"];
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        MyReader.Read();
        if (MyReader.HasRows)
        {
            Lbl_ClassTeacher.Text = MyReader.GetValue(0).ToString();
        }
        sql = "select count(stud.Sex) as totalMale from tblstudent stud inner join tblstudentclassmap map on map.StudentId=stud.Id  where stud.Sex='Male' and map.ClassId=" + Session["ClassId"] + " and stud.Status=1 and map.BatchId=" + MyUser.CurrentBatchId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        MyReader.Read();
        if (MyReader.HasRows)
        {
            Lbl_NoBoys.Text = MyReader.GetValue(0).ToString();
            Lbl_nogirls.Text = (int.Parse(Lbl_NoStud.Text) - int.Parse(MyReader.GetValue(0).ToString())).ToString();
        }
        sql = "Select tblsubjects.subject_name,tblsubjects.Id from tblsubjects inner join tblclasssubmap on tblsubjects.Id=tblclasssubmap.SubjectId where tblclasssubmap.ClassId=" + Session["ClassId"];
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        while (MyReader.Read())
        {
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                LstSubjects.Items.Add(li);
                li = null;
            }
        }
        MyReader.Close();
    }

    protected void Btn_Export_Click(object sender, EventArgs e)
    {
        string ClassName = MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString()));
        ClassName = ClassName + "-StudentList";

        string sql = "SELECT tblstudentclassmap.RollNo , tblclass.ClassName ,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex, tblstudent.GardianName , DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y'), tblreligion.Religion, tblcast.castname, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y')  , tblstudent.ResidencePhNo , tblstudent.OfficePhNo , tblstudent.Address , tblstudent.State , tblstudent.Pin,tblstudent.nationality,tblstudent.fathereduquali,tblstudent.mothersname,tblstudent.mothereduquali,tblstudent.fatheroccupation,tblstudent.motheroccupation,tblstudent.annualincome,tblbloodgrp.GroupName,tblstudent.email,tblstudent.aadharnumber,tbllanguage.Language,tblstudent.Id from tblstudent inner join tbllanguage on tbllanguage.Id = tblstudent.MotherTongue INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId inner join tblreligion on tblreligion.Id = tblstudent.Religion inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblcast on tblcast.Id = tblstudent.cast WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudent.Studentname ASC";

        DataSet StudentsList = new DataSet();
        DataTable dt;
        DataRow dr;
        StudentsList.Tables.Add(new DataTable("Student"));
        dt = StudentsList.Tables["Student"];
        dt.Columns.Add("RollNo");
        dt.Columns.Add("ClassName");
        dt.Columns.Add("StudentName");
        dt.Columns.Add("Admission No");
        dt.Columns.Add("Sex");
        dt.Columns.Add("Guardian");
        dt.Columns.Add("DOB");
        dt.Columns.Add("BloodGroup");
        dt.Columns.Add("Religion");
        dt.Columns.Add("DateofJoining");
        dt.Columns.Add("ResidencePhNo");
        dt.Columns.Add("OfficePhNo");
        dt.Columns.Add("Address");
        dt.Columns.Add("State");
        dt.Columns.Add("Caste");
        dt.Columns.Add("Pin");
        dt.Columns.Add("Nationality");
        dt.Columns.Add("FathersEduQuali");
        dt.Columns.Add("MothersName");
        dt.Columns.Add("MothersEduQuali");
        dt.Columns.Add("FatherOccupation");
        dt.Columns.Add("MotherOccupation");
        dt.Columns.Add("MotherTongue");
        dt.Columns.Add("AnnualIncome");
        dt.Columns.Add("Email");
        dt.Columns.Add("AadharNumber");


        //Checking any Cutome Fields is available
        DataSet _Reader = null;
        List<string> _FieldLst = null;
        bool CustomFieldExisting = IsCustomFieldExisting(ref _Reader, ref _FieldLst);
        if (CustomFieldExisting)
        {
            for (int i = 0; i < _FieldLst.Count; i++)
            {
                dt.Columns.Add(_FieldLst[i].ToString());
            }
        }

        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                int studID = int.Parse(MyReader.GetValue(26).ToString());
                dr = StudentsList.Tables["Student"].NewRow();
                dr["RollNo"] = MyReader.GetValue(0).ToString();
                dr["ClassName"] = MyReader.GetValue(1).ToString();
                dr["StudentName"] = MyReader.GetValue(2).ToString();
                dr["Admission No"] = MyReader.GetValue(3).ToString();
                dr["Sex"] = MyReader.GetValue(4).ToString();
                dr["Guardian"] = MyReader.GetValue(5).ToString();
                dr["DOB"] = MyReader.GetValue(6).ToString();
                dr["Religion"] = MyReader.GetValue(7).ToString();
                dr["DateofJoining"] = MyReader.GetValue(9).ToString();
                dr["ResidencePhNo"] = MyReader.GetValue(10).ToString();
                dr["OfficePhNo"] = MyReader.GetValue(11).ToString();
                dr["Address"] = MyReader.GetValue(12).ToString();
                dr["State"] = MyReader.GetValue(13).ToString();
                dr["BloodGroup"] = MyReader.GetValue(22).ToString();
                dr["Caste"] = MyReader.GetValue(8).ToString();
                dr["Pin"] = MyReader.GetValue(14).ToString();
                dr["Nationality"] = MyReader.GetValue(15).ToString();
                dr["FathersEduQuali"] = MyReader.GetValue(16).ToString();
                dr["MothersName"] = MyReader.GetValue(17).ToString();
                dr["MothersEduQuali"] = MyReader.GetValue(18).ToString();
                dr["FatherOccupation"] = MyReader.GetValue(19).ToString();
                dr["MotherOccupation"] = MyReader.GetValue(20).ToString();
                dr["MotherTongue"] = MyReader.GetValue(25).ToString();
                dr["AnnualIncome"] = MyReader.GetValue(21).ToString();
                dr["Email"] = MyReader.GetValue(23).ToString();
                dr["AadharNumber"] = MyReader.GetValue(24).ToString();


                if (CustomFieldExisting)
                    for (int row = 0; row < _Reader.Tables[0].Rows.Count; row++)
                        if (Convert.ToInt32(_Reader.Tables[0].Rows[row]["StudentId"].ToString()) == studID)
                        {
                            for (int i = 0; i < _FieldLst.Count; i++)
                                dr[_FieldLst[i].ToString()] = _Reader.Tables[0].Rows[row][(i + 1)];
                            break;
                        }
                StudentsList.Tables["Student"].Rows.Add(dr);
            }
        }

        if (StudentsList != null && StudentsList.Tables[0] != null && StudentsList.Tables[0].Rows.Count > 0)
        {
            string _ReportName = ClassName;
            string FileName = "StudentList";
            if (!WinEr.ExcelUtility.ExportDataToExcel(StudentsList, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_msg.Text = "You may missing Excel or try again later";
                this.MPE_MessageBox.Show();
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void Img_StudentPreview_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("StudentImageSlider.aspx");
    }

    protected void Img_StudentEdit_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ManageBulkStudent.aspx");
    }

    private bool IsCustomFieldExisting(ref DataSet _Reader, ref List<string> FieldLst)
    {
        bool isExisting = false;
        FieldLst = new List<string>();
        string sqlQury = "select * from tblstudentfieldconfi";
        string dataQuery = "select StudentId";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sqlQury);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                FieldLst.Add(MyReader.GetValue(3).ToString());
                dataQuery = dataQuery + "," + MyReader.GetValue(1);
            }
            dataQuery = dataQuery + " from tblstudentdetails";
            _Reader = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(dataQuery);

            if (_Reader.Tables[0].Rows.Count > 0)
            {
                isExisting = true;
            }
        }
        return isExisting;
    }

    protected void Img_GovtExport_Click(object sender, ImageClickEventArgs e)
    {
        string ClassName = MyClassMang.GetClassname(int.Parse(Session["ClassId"].ToString()));
        ClassName = ClassName + "-StudentList";
        int SerialNO = 1;
        string sql = "SELECT tblstudentclassmap.RollNo , tblclass.ClassName ,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex, tblstudent.GardianName , DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y'), tblreligion.Religion, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y')  , tblstudent.ResidencePhNo , tblstudent.OfficePhNo , tblstudent.Address , tblstudent.State , tblbloodgrp.GroupName,tblstudent.MothersName,tblstudent.Id from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id inner join tblclass on tblclass.Id = tblstudentclassmap.ClassId inner join tblreligion on tblreligion.Id = tblstudent.Religion inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
        DataSet StudentsList = new DataSet();
        DataTable dt;
        DataRow dr;
        StudentsList.Tables.Add(new DataTable("Student"));
        dt = StudentsList.Tables["Student"];
        dt.Columns.Add("SerialNo");
        dt.Columns.Add("StudentName");
        dt.Columns.Add("Father's Name");
        dt.Columns.Add("Mother's Name");
        dt.Columns.Add("Address");
        dt.Columns.Add("St. No.");
        dt.Columns.Add("DOB");
        dt.Columns.Add("Ad.D");
        dt.Columns.Add("Ad. No.");
        dt.Columns.Add("Sex");
        dt.Columns.Add("Social");
        dt.Columns.Add("Religion");
        dt.Columns.Add("BPL Card");
        dt.Columns.Add("Fac");
        dt.Columns.Add("Free RTE");
        dt.Columns.Add("PRS");
        dt.Columns.Add("Last");
        dt.Columns.Add("Sati");
        dt.Columns.Add("Attd.");
        dt.Columns.Add("Medium");
        dt.Columns.Add("PH");
        dt.Columns.Add("F/o F");
        dt.Columns.Add("Book");
        dt.Columns.Add("Uni");
        dt.Columns.Add("Free Transport");
        dt.Columns.Add("Free Guidance");
        dt.Columns.Add("Free fac");
        dt.Columns.Add("Extra Course");
        dt.Columns.Add("Residnce");

        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                int studID = int.Parse(MyReader.GetValue(15).ToString());
                //int studID = MyReader.GetInt32(15);
                dr = StudentsList.Tables["Student"].NewRow();
                dr["SerialNo"] = SerialNO;
                dr["St. No."] = MyReader.GetValue(0).ToString();
                dr["Last"] = MyReader.GetValue(1).ToString();
                dr["StudentName"] = MyReader.GetValue(2).ToString();
                dr["Ad. No."] = MyReader.GetValue(3).ToString();
                dr["Sex"] = MyReader.GetValue(4).ToString();
                dr["Father's Name"] = MyReader.GetValue(5).ToString();
                dr["DOB"] = MyReader.GetValue(6).ToString();
                dr["Religion"] = MyReader.GetValue(7).ToString();
                dr["Ad.D"] = MyReader.GetValue(8).ToString();
                dr["Address"] = MyReader.GetValue(11).ToString();
                dr["Mother's Name"] = MyReader.GetValue(14).ToString();
                dr["Social"] = "0";
                dr["BPL Card"] = "2";
                dr["Fac"] = "0";
                dr["Free RTE"] = "2";

                dr["PRS"] = "1";

                dr["Sati"] = "";
                dr["Attd."] = "";
                dr["Medium"] = "19";
                dr["PH"] = "0";
                dr["F/o F"] = "0";
                dr["Book"] = "2";
                dr["Uni"] = "0";
                dr["Free Transport"] = "0";
                dr["Free Guidance"] = "0";
                dr["Free fac"] = "2";
                dr["Extra Course"] = "0";
                dr["Residnce"] = "0";


                SerialNO++;
                StudentsList.Tables["Student"].Rows.Add(dr);
            }
        }
        if (StudentsList != null && StudentsList.Tables[0] != null && StudentsList.Tables[0].Rows.Count > 0)
        {
            string _ReportName = ClassName;
            string FileName = "StudentList";
            if (!WinEr.ExcelUtility.ExportDataToExcel(StudentsList, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_msg.Text = "You may missing Excel or try again later";
                this.MPE_MessageBox.Show();

            }

            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(StudentsList, ClassName+".xls"))
            //{
            //    Lbl_msg.Text = "You may missing Excel or try again later";
            //    this.MPE_MessageBox.Show();

            //}
        }
    }
}
