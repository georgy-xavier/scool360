using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Web.Script.Services;
public partial class StudentDetails : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private ParentInfoClass MyParentInfo;
    private KnowinUser MyUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
       // MyParentInfo = (ParentInfoClass)Session["UserObj"];
        // MyStudMang = MyUser.GetStudentObj();
        string studentID = MyUser.User_Id.ToString();
        studentID = MyUser.UserId.ToString();
        //string studentID = MyUser.StudentId.ToString();
        string studentType = "1";
        Session["StudId"] = studentID;
        string _InnerHtml = " <script type=\"text/javascript\">document.cookie = \"$CUR$STDNT$ID$=" + studentID.ToString() + "\";document.cookie = \"$CUR$STDNT$TYPE$=" + studentType.ToString() + "\";</script>";
        this.javascriptId.InnerHtml = _InnerHtml;
        //if (MyStudMang == null)
        //{
        //    Response.Redirect("RoleErr.htm");
        //    //no rights for this user.
        //}        this.javascriptId.InnerHtml = _InnerHtml;

    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string[] loadGeneralDetails()
    {
        int Today = DateTime.Now.Year;
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        // Up to Nationality Field in tblstudent
        GenCls db = new GenCls();

        string[] JsonDt = new string[5];
        JsonDt[0] = studentID.ToString();
        string Sql = "SELECT tblstandard.Name,tblclass.ClassName from tblview_studentclassmap INNER join tblclass on tblview_studentclassmap.ClassId=tblclass.Id inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard where  tblview_studentclassmap.StudentId= " + studentID + " order by tblview_studentclassmap.BatchId desc";
        JsonDt[1] = db.readFromDb(Sql);
        Sql = "SELECT batch.BatchName FROM tblbatch batch INNER JOIN tblview_student stud on stud.JoinBatch = batch.Id AND stud.Id=" + studentID + "";
        JsonDt[2] = db.readFromDb(Sql);
        Sql = "SELECT tblstandard.Name FROM tblview_student INNER JOIN tblstandard ON tblview_student.JoinStandard=tblstandard.Id WHERE tblview_student.Id=" + studentID;
        JsonDt[3] = db.readFromDb(Sql);
        Sql = "select tblreligion.Religion, tblcast.castname from  tblview_student inner join tblreligion on tblreligion.Id= tblview_student.Religion inner join tblcast on tblcast.Id= tblview_student.`Cast` where tblview_student.Id=" + studentID;
        JsonDt[4] = db.readFromDb(Sql);
        if (JsonDt[1] != null)
            return JsonDt;
        return null;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string[] loadStdntInitDtls()
    {
        string[] JsonDt = new string[2];
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        GenCls db = new GenCls();
        JsonDt[0] = studentID.ToString();
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
        string sql = "SELECT GardianName,Address AS Permanent_address,Addresspresent AS Communication_addrs,ResidencePhNo,Location,State,Nationality,Pin,Email FROM tblstudent WHERE tblstudent.Id=" + studentID + "";
        JsonDt[1] = db.readFromDb(sql);//student_name,adm_No,Stdnt_Id,DOB,Gender,DOJ,gardian_name,Permanent_address,Communication_addrs,Res_Ph,Off_Ph,RollNo,Loc,State,nation,pin,mail
        if (JsonDt[1] != null)
            return JsonDt;
        return null;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string[] loadOtherDetails()
    {
        string[] JsonDt = new string[5];
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        GenCls db = new GenCls();
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
        string sql = "select  tblview_student.MothersName, tblview_student.FatherEduQuali, tblview_student.MotherEduQuali, tblview_student.FatherOccupation,tblview_student.MotherOccupation, tblview_student.AnnualIncome, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters, (select tbloptionmaster.`Option` from tbloptionmaster where tbloptionmaster.Id= tblview_student.UseBus) as UseBus, (select tbloptionmaster.`Option` from tbloptionmaster where tbloptionmaster.Id= tblview_student.UseHostel) as UseHostel,tblview_student.SecondaryNo,tblview_student.AadharNumber from  tblview_student where tblview_student.Id=" + studentID;
        JsonDt[0] = db.readFromDb(sql);//MothersName,FatherEduQuali,MotherEduQuali,FatherOccupation,MotherOccupation,AnnualIncome,BroCount,SisCOunt,bus,hostl,secc_mob,adhr.
        sql = "select tblbloodgrp.GroupName from  tblview_student inner join tblbloodgrp on tblbloodgrp.Id= tblview_student.BloodGroup  where tblview_student.Id=" + studentID;
        JsonDt[1] = db.readFromDb(sql);//bloodgroup
        sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.MotherTongue  where tblview_student.Id=" + studentID;
        JsonDt[2] = db.readFromDb(sql);//mothertongue
        sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.1stLanguage  where tblview_student.Id="+studentID;
        JsonDt[3] = db.readFromDb(sql);//first language
        sql = "select tblstudtype.TypeName from  tblview_student inner join tblstudtype on tblstudtype.Id= tblview_student.StudTypeId  where tblview_student.Id=" + studentID;
        JsonDt[4] = db.readFromDb(sql);//student category
        if (JsonDt[1] != null)
            return JsonDt;
        return null;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string loadExtraDetails()
    {
        string JsonDt = "";
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        GenCls db = new GenCls();
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);

        int CustfieldCount = MyStudMang.CoustomFieldCount;
        if (CustfieldCount == 0)
        {
            JsonDt = "<div>No extra information available</div>";
        }
        else
        {
            DataSet _CustomFields = MyStudMang.GetCuestomFields();
            if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                {
                    string fieldName = dr_fieldData[1].ToString();
                    string fieldData = MyStudMang.GetCuestomField(dr_fieldData[0].ToString(), studentID.ToString());
                    JsonDt = "<div class=\"row listItem\"><div class=\"col-md-6\">" + fieldName + "</div><div class=\"col-md-6 stdDetilsTxt\">" + fieldData + "</div></div>";
                    i++;
                }
            }

        }
        return JsonDt;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string loadIncidenceDetails()
    {
        string _MenuStr = "";
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        Incident inc = new Incident(_Prntobj);
        if (MyUser.HaveActionRignt(70))
        {
            _MenuStr = inc.LoadIncidenceData(studentID , "student", MyUser.CurrentBatchId);
        }
        else
        {

            _MenuStr = "Sorry, for seccurity & privacy you have no rights for view this.Please contact admin";
        }
        return _MenuStr;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string loadStdntCarrerDtls()
    {
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
        GenCls db = new GenCls();
        string JsonDt = "";
        DataSet MydataSet = db.GetDtls(MyStudMang.GetCarrierData(studentID)); //Using ExecuteQueryReturnDataSet from MySqlClass
        ArrayList myArrayList = new ArrayList();
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            DataTable dtAccountData = MydataSet.Tables[0];
            for (int i = 0; i < dtAccountData.Rows.Count; i++)
            {
                string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                myArrayList.Add(stringArr);
            }
            JsonDt = JsonConvert.SerializeObject(myArrayList);
        }
        else
        {
            JsonDt = null;
        }
        return JsonDt;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string loadSiblingDetails()
    {
        string sql = "", JsonDt = "";;
        DataSet SiblingsDs = new DataSet();
        ArrayList myArrayList = new ArrayList();
        OdbcDataReader Siblingsreder = null;
        int studentID = Convert.ToInt32(HttpContext.Current.Session["StudId"]);
        KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
        StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
        sql = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + studentID + "";
        OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            sql = "select tbl_siblingsmap.StudId from tbl_siblingsmap where tbl_siblingsmap.Id=" + int.Parse(MyReader.GetValue(0).ToString()) + " and StudId<>" + studentID + "";
            Siblingsreder = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (Siblingsreder.HasRows)
            {
                string StudId = "";
                int count = 0;
                while (Siblingsreder.Read())
                {
                    if (count == 0)
                    {
                        StudId = Siblingsreder.GetValue(0).ToString();
                        count = 1;
                    }
                    else
                    {
                        StudId = StudId + "," + Siblingsreder.GetValue(0).ToString();
                    }
                }
                string _sql = "select Id,StudentName,GardianName,sex,OfficePhNo from tblstudent where tblstudent.Id in(" + StudId + ")";
                SiblingsDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                if (SiblingsDs != null && SiblingsDs.Tables != null && SiblingsDs.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAccountData = SiblingsDs.Tables[0];
                    for (int i = 0; i < dtAccountData.Rows.Count; i++)
                    {
                        string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                        myArrayList.Add(stringArr);
                    }
                    JsonDt = JsonConvert.SerializeObject(myArrayList);
                }
                else
                {
                    JsonDt = null;
                }
                return JsonDt;
            }
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public static string StudentSibDtlsview(string StudId)
    {
        string urlRedirect = "StudentDetails.aspx";
        int StudentId = int.Parse(StudId);
        HttpContext.Current.Session["StudId"] = StudentId;
        return urlRedirect;
    }
    public class GenCls
    {
        public string readFromDb(string sql)
        {
            string JsonDt = null;
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            DataSet MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); //Using ExecuteQueryReturnDataSet from MySqlClass
            ArrayList myArrayList = new ArrayList();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {

                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                JsonDt = JsonConvert.SerializeObject(myArrayList);
                // return JsonDt;
            }
            else
            {
                JsonDt = null;
            }
            return JsonDt;
        }
        public DataSet GetDtls(DataSet MyDataset)
        {
            int ClassId = -1, PrevClassId = -1, rowId = 0;
            int studentID =  Convert.ToInt32(HttpContext.Current.Session["StudId"]);
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            if (MyDataset != null && MyDataset.Tables[0] != null && MyDataset.Tables[0].Rows.Count > 0)
            {
                DataTable dt = MyDataset.Tables[0];
                dt.Columns.Add("Result");
                foreach (DataRow dro in MyDataset.Tables[0].Rows)
                {
                    string result = "Passed";
                    ClassId = int.Parse(dro["ClassId"].ToString());
                    if (PrevClassId == ClassId)
                    {
                        int id = rowId - 1;
                        MyDataset.Tables[0].Rows[id]["Result"] = "Failed";
                    }
                    PrevClassId = ClassId;
                    dro["Result"] = result;
                    rowId++;
                }
                MyDataset.Tables[0].Rows[rowId - 1]["Result"] = MyStudMang.getStatus(studentID);
            }
            return MyDataset;
        }
    }


     

    //private void LoadSiblingDetails()
    //{
    //    string sql = "";
    //    DataSet SiblingsDs = new DataSet();
    //    OdbcDataReader Siblingsreder = null;
    //    sql = "select Id from tbl_siblingsmap where tbl_siblingsmap.StudId=" + int.Parse(Session["StudId"].ToString()) + "";
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
    //    if (MyReader.HasRows)
    //    {
    //        sql = "select tbl_siblingsmap.StudId from tbl_siblingsmap where tbl_siblingsmap.Id=" + int.Parse(MyReader.GetValue(0).ToString()) + " and StudId<>" + int.Parse(Session["StudId"].ToString())+ "";
    //        Siblingsreder = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
    //        if (Siblingsreder.HasRows)
    //        {
    //            string StudId = "";
    //            int count = 0;
    //            while (Siblingsreder.Read())
    //            {
    //                if (count == 0)
    //                {
    //                    StudId = Siblingsreder.GetValue(0).ToString();
    //                    count = 1;
    //                }
    //                else
    //                {
    //                    StudId = StudId + "," + Siblingsreder.GetValue(0).ToString();

    //                }


    //            }
    //            string _sql = "select Id,StudentName,GardianName from tblstudent where tblstudent.Id in(" + StudId + ")";
    //            SiblingsDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
    //            if (SiblingsDs != null && SiblingsDs.Tables[0].Rows.Count > 0)
    //            {
    //                Pnl_SibDisplay.Visible = true;
    //                GrdSiblings.Columns[0].Visible = true;
    //                GrdSiblings.DataSource = SiblingsDs;
    //                GrdSiblings.DataBind();
    //                GrdSiblings.Columns[0].Visible = false;
    //                Lbl_Sib.Text = "Siblings";
    //            }
    //            else
    //            {
    //                GrdSiblings.DataSource = null;
    //                GrdSiblings.DataBind();
    //                Pnl_SibDisplay.Visible = false;
    //                Lbl_Sib.Text = "";

    //            }

    //        }

    //    }

    //}
    private void LoadpupilTopData()
    {

        string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
       // this.StudentTopStrip.InnerHtml = _Studstrip;
    }
    //protected void GrdSiblings_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    if (e.CommandName == "View")
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        int StudentId = int.Parse(GrdSiblings.Rows[index].Cells[0].Text);
    //        //int StudentStatus = int.Parse(GrdSiblings.Rows[index].Cells[1].Text);
    //        if (e.CommandName == "View")
    //        {
    //            //if (StudentStatus == 1 || StudentStatus == 2 || StudentStatus == 3)
    //            //{
    //                Session["StudId"] = StudentId;
    //               // Session["StudType"] = StudentStatus;
    //                Response.Redirect("StudentDetails.aspx");

    //           // }

    //            //else if (MyUser.HaveActionRignt(606))
    //            //{
    //            //    HiddenField Hdn = new HiddenField();
    //            //    Hdn = (HiddenField)GrdSiblings.Rows[index].FindControl("Hdn_TempId");
    //            //    string TempId = Hdn.Value;
    //            //    int ClassId = 0;
    //            //    string sql = "";
    //            //    OdbcDataReader Classreader = null;

    //            //    sql = "Select Class from tbltempstdent where tbltempstdent.TempId='" + TempId + "'";
    //            //    Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
    //            //    if (Classreader.HasRows)
    //            //    {
    //            //        int.TryParse(Classreader.GetValue(0).ToString(), out ClassId);
    //            //    }

    //            //    //ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegisteredStudentDetails.aspx?TempStudId=" + TempId + "&ClassId=" + ClassId + "');", true);
    //            //}
    //            //else
    //            //WC_MessageBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");


    //        }
    //    }


    //}
    //private void LoadCoustomFields()
    //{
    //    int CustfieldCount = MyStudMang.CoustomFieldCount;
    //    if (CustfieldCount == 0)
    //    {
    //        Pnl_custumarea.Visible = false;
    //    }
    //    else
    //    {

    //        DataSet _CustomFields = MyStudMang.GetCuestomFields();
    //        if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
    //        {

    //            int i = 0;
    //            Table tbl = new Table();
                
    //            myPlaceHolder.Controls.Add(tbl);
    //            tbl.CssClass = "tablelist";

    //            foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
    //            {

    //                TableRow tr = new TableRow();
    //                TableCell tc1 = new TableCell();
    //                TableCell tc2 = new TableCell();
                    
    //                 tc1.Text = dr_fieldData[1].ToString()+":";
    //                 tc1.CssClass = "leftside";

    //                Label Lblcostom = new Label();
    //                Lblcostom.Text = MyStudMang.GetCuestomField(dr_fieldData[0].ToString(),Session["StudId"].ToString());
    //                Lblcostom.ForeColor = System.Drawing.Color.Black ;
    //                Lblcostom.Font.Bold = true;
    //                Lblcostom.ID = "myLbl" + i.ToString();
    //                tc2.Controls.Add(Lblcostom);
    //                tc2.CssClass = "rightside";

    //                tr.Cells.Add(tc1);
    //                tr.Cells.Add(tc2);
                   
    //                tbl.Rows.Add(tr);
    //                i++;
    //            }
    //        }

    //    }
    //}
    //private void LoadOtherDetails()
    //{
    //    string Sql = "select tblview_student.Nationality , tblview_student.MothersName, tblview_student.FatherEduQuali, tblview_student.MotherEduQuali, tblview_student.FatherOccupation,tblview_student.MotherOccupation, tblview_student.AnnualIncome, tblview_student.Addresspresent, tblview_student.Location, tblview_student.State, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo, tblview_student.Email, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters, (select tbloptionmaster.`Option` from tbloptionmaster where tbloptionmaster.Id= tblview_student.UseBus) as UseBus, (select tbloptionmaster.`Option` from tbloptionmaster where tbloptionmaster.Id= tblview_student.UseHostel) as UseHostel,tblview_student.SecondaryNo,tblview_student.AadharNumber from  tblview_student where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    // Up to Nationality Field in tblstudent
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_nat_ot.Text = MyReader.GetValue(0).ToString();
    //        Lbl_mothernane_ot.Text = MyReader.GetValue(1).ToString();

    //        Lbl_fatherqlif_ot.Text = MyReader.GetValue(2).ToString();
    //        Lbl_motherqlfi_ot.Text = MyReader.GetValue(3).ToString();
    //        Lbl_fatherocc_ot.Text = MyReader.GetValue(4).ToString();
    //        Lbl_motherocc_ot.Text = MyReader.GetValue(5).ToString();
    //        if (MyReader.GetValue(5).ToString()!="0")
    //        Lbl_annualincom_ot.Text = MyReader.GetValue(6).ToString();
    //        Txt_addresspresent_ot.Text = MyReader.GetValue(7).ToString();
    //        Lbl_location_ot.Text = MyReader.GetValue(8).ToString();
    //        Lbl_state_ot.Text = MyReader.GetValue(9).ToString();
    //        if (MyReader.GetValue(9).ToString() != "0")
    //        Lbl_pin_ot.Text = MyReader.GetValue(10).ToString();
    //        //if (MyReader.GetValue(10).ToString() != "0")
    //        Lbl_resdphn_ot.Text = MyReader.GetValue(11).ToString();
    //        if (MyReader.GetValue(11).ToString() != "0")
    //        Lbl_mob_ot.Text = MyReader.GetValue(12).ToString();
    //        Lbl_email_ot.Text = MyReader.GetValue(13).ToString();
    //        if (MyReader.GetValue(13).ToString() != "0")
    //        Lbl_nofobrot_ot.Text = MyReader.GetValue(14).ToString();
    //        if (MyReader.GetValue(14).ToString() != "0")
    //        Lbl_noofsist_ot.Text = MyReader.GetValue(15).ToString();
    //        lbl_CollegeBus.Text = MyReader.GetValue(16).ToString();
    //        lbl_Hostel.Text = MyReader.GetValue(17).ToString();
    //        Lbl_mob_second_ot.Text = MyReader.GetValue(18).ToString();
    //        Lbl_Aadharno.Text = MyReader.GetValue(19).ToString();
    //        MyReader.Close();
    //    }

    //    Sql = "select tblbloodgrp.GroupName from  tblview_student inner join tblbloodgrp on tblbloodgrp.Id= tblview_student.BloodGroup  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_blodgroup_ot.Text = MyReader.GetValue(0).ToString();
            
    //        MyReader.Close();
    //    }
    //    Sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.MotherTongue  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_mot_ot.Text = MyReader.GetValue(0).ToString();

    //        MyReader.Close();
    //    }
    //    Sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.1stLanguage  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_firstlng_ot.Text = MyReader.GetValue(0).ToString();

    //        MyReader.Close();
    //    }

    //    Sql = "select tblstudtype.TypeName from  tblview_student inner join tblstudtype on tblstudtype.Id= tblview_student.StudTypeId  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_studcat_ot.Text = MyReader.GetValue(0).ToString();

    //        MyReader.Close();
    //    }
    //}
    //private void LoadGeneralDetails()
    //{
    //    DateTime Dob;
    //    DateTime Doj;
    //    int Today = DateTime.Now.Year;
    //    string Sql = "select tblview_student.StudentName, tblview_student.Sex, tblview_student.DOB, tblview_student.GardianName, tblview_student.Address, tblview_student.DateofJoining,tblview_student.StudentId,tblview_student.AdmitionNo from tblview_student where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    // Up to Nationality Field in tblstudent
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_Name_gnl.Text = MyReader.GetValue(0).ToString();
    //        Lbl_sex_gnl.Text = MyReader.GetValue(1).ToString();
    //        Dob = DateTime.Parse(MyReader.GetValue(2).ToString());
    //        //Dob = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());

    //        Lbl_dob_gnl.Text =MyUser.GerFormatedDatVal(Dob);
    //        Lbl_father_gnl.Text = MyReader.GetValue(3).ToString();
    //        Txt_Address.Text = MyReader.GetValue(4).ToString();
    //        Doj = DateTime.Parse(MyReader.GetValue(5).ToString());
    //        //Doj = MyUser.GetDareFromText(MyReader.GetValue(5).ToString());
    //        Lbl_StudentId.Text = MyReader.GetValue(6).ToString();
    //        lbl_AdmissioinNo_gnl.Text = MyReader.GetValue(7).ToString();
    //        Lbl_doa_gnl.Text = MyUser.GerFormatedDatVal(Doj);
           
    //        MyReader.Close();
    //    }

    //    Sql = "SELECT tblstandard.Name,tblclass.ClassName from tblview_studentclassmap INNER join tblclass on tblview_studentclassmap.ClassId=tblclass.Id inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard where  tblview_studentclassmap.StudentId= " + int.Parse(Session["StudId"].ToString()) + " order by tblview_studentclassmap.BatchId desc";
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_std_gnl.Text = MyReader.GetValue(0).ToString();
    //        Lbl_class_gnl.Text = MyReader.GetValue(1).ToString();
    //        MyReader.Close();
    //    }
    //    Sql = "SELECT batch.BatchName FROM tblbatch batch INNER JOIN tblview_student stud on stud.JoinBatch = batch.Id AND stud.Id=" + int.Parse(Session["StudId"].ToString()) + "";
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_joinbatch_gnl.Text = MyReader.GetValue(0).ToString();
            
    //    }

    //    Sql = "SELECT tblstandard.Name FROM tblview_student INNER JOIN tblstandard ON tblview_student.JoinStandard=tblstandard.Id WHERE tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        lbl_joinstandard.Text = MyReader.GetValue(0).ToString();

    //    }


    //    Sql = "select tblreligion.Religion, tblcast.castname from  tblview_student inner join tblreligion on tblreligion.Id= tblview_student.Religion inner join tblcast on tblcast.Id= tblview_student.`Cast` where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
    //    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
    //    if (MyReader.HasRows)
    //    {
    //        MyReader.Read();
    //        Lbl_religion_gnl.Text = MyReader.GetValue(0).ToString();
    //        Lbl_cast_gnl.Text = MyReader.GetValue(1).ToString();
    //        MyReader.Close();
    //    }
       
    //}
    //private void CheckViewIncidentRight()
    //{
    //    if (MyUser.HaveActionRignt(70))
    //    {
    //        this.TopTab.InnerHtml = MyIncident.LoadIncidenceData(int.Parse(Session["StudId"].ToString()), "student",MyUser.CurrentBatchId);
    //    }
    //    else
    //    {
           
    //        this.TabPanel3.Visible = false;
    //    }
    //}



    //protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
    //{
    //    string FileName = "StudentDetails";
    //    // FileName = FileName + "Exam Report.xls";
    //    // ClassTeacherReport.InnerHtml = MyExamMang.GetClassTeacherReport(int.Parse(Drp_Class.SelectedValue), ExamId);
    //    Response.ContentType = "application/force-download";
    //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
    //    Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
    //    Response.Write("<head>");
    //    Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
    //    Response.Write("<!--[if gte mso 9]><xml>");
    //    Response.Write("<x:ExcelWorkbook>");
    //    Response.Write("<x:ExcelWorksheets>");
    //    Response.Write("<x:ExcelWorksheet>");
    //    Response.Write("<x:Name>Student Details</x:Name>");
    //    Response.Write("<x:WorksheetOptions>");
    //    Response.Write("<x:Print>");
    //    Response.Write("<x:ValidPrinterInfo/>");
    //    Response.Write("</x:Print>");
    //    Response.Write("</x:WorksheetOptions>");
    //    Response.Write("</x:ExcelWorksheet>");
    //    Response.Write("</x:ExcelWorksheets>");
    //    Response.Write("</x:ExcelWorkbook>");
    //    Response.Write("</xml>");
    //    Response.Write("<![endif]--> ");
    //    Response.Write(GetStudentDetailsForPrintOut());
    //    Response.Write("</head>");
    //    Response.Flush();
    //    Response.End();
    //}
    //private string GetStudentDetailsForPrintOut()
    //{
    //    StringBuilder CTR = new StringBuilder();

    //    CTR.Append("<table id=\"MyReport\" runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
        
    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Name</td>");           
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " +Lbl_Name_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Sex</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_sex_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">DOB</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_dob_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Father/Guardian Name</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_father_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Standard</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_std_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Class</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_class_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Address (Permanent)</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Txt_Address.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Joining Batch</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_joinbatch_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Date of Admission</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_doa_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Religion</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_religion_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Caste</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_cast_gnl.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Blood Group</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_blodgroup_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Nationality</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_nat_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Mother Tongue</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_mot_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Mother's Name</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_mothernane_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Father's Occupation</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_fatherocc_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Annual Income</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_annualincom_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>"); 

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Location</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_location_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">State</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_state_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">PIN</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_pin_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Phone</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_resdphn_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Mobile Number</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_mob_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Email</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_email_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">No of Brothers</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_nofobrot_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">No of Sisters</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_noofsist_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">1st Language Wishes to take</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_firstlng_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("<tr>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\">Student Category</td>");
    //    CTR.Append("<td class=\"TDSubject\" style=\" font-weight:bold; text-align:center; border: thin solid #000000\"> " + Lbl_studcat_ot.Text + "");
    //    CTR.Append("</td>");
    //    CTR.Append("</tr>");

    //    CTR.Append("</table>");
    //    return CTR.ToString();
    //}
}
