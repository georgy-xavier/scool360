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
using System.Text;
using GemBox.Spreadsheet;
using System.IO;
using WinEr;
using WinBase;
using System.Web.Services;
using Newtonsoft.Json;
using System.Runtime.Remoting.Contexts;

public partial class SearchStudent : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    private DataSet MydataSet2;
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
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
        
        }
    }
    public class GenClas
    {
      
        public string GetDataFromDb(string sql)
        {
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            DataSet MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); 
            ArrayList myArrayList = new ArrayList();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {

                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                string JsonDt = JsonConvert.SerializeObject(myArrayList);
                return JsonDt;
            }
            return null;
        }
        public string GetAllClsData()
        {
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            ArrayList myArrayList = new ArrayList();
            DataSet MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountData = MydataSet.Tables[0];
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {

                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                string JsonDt = JsonConvert.SerializeObject(myArrayList);
                return JsonDt;
            }
            return null;
        }
        public string[] FillStdntDt(string sql)
        {
            SchoolClass objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
            KnowinGen _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
            StudentManagerClass MyStudMang = new StudentManagerClass(_Prntobj);
            DataSet MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); //Using ExecuteQueryReturnDataSet from MySqlClass
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["RsltQry"] = sql;
                HttpContext.Current.Session["SortDirection"] = "ASC";
                HttpContext.Current.Session["SortExpression"] = "StudentName";
                //ViewState["StudentList"] = MydataSet;
                HttpContext.Current.Session["StudentList"] = MydataSet;
                string[] JsonDt = new string[2];
                DataTable dtAccountData = MydataSet.Tables[0];
                ArrayList myArrayList = new ArrayList();
                for (int i = 0; i < dtAccountData.Rows.Count; i++)
                {
                    string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                    myArrayList.Add(stringArr);
                }
                JsonDt[0] = JsonConvert.SerializeObject(myArrayList);
                JsonDt[1] = MydataSet.Tables[0].Rows[0]["Id"].ToString();
                return JsonDt;
            }
            return null;
        }
    }
    [WebMethod(EnableSession = true)]
    public static string[] LoadDfltstdntDt()
    {
        GenClas dd = new GenClas();
        string[] stDt = new string[2];
        if (HttpContext.Current.Session["RsltQry"] != null)
        {
            stDt = dd.FillStdntDt(HttpContext.Current.Session["RsltQry"].ToString());
            if (stDt != null)
                return stDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string GetCurrentUser()
    {
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        return MyUser.UserId.ToString();
    }
    [WebMethod(EnableSession = true)]
    public static string[] LoadSrchStdntDt(string Txt_Search, int Drp_SearchBy, string contextKey)
    {
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        string sql = "";
        string[] Chk_SearchList = contextKey.Split('\\');

        if (Drp_SearchBy == 0 && Txt_Search != "")
        {
            //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
            //NOTE : Student status is setting in below query not like as on DB...On student details student type will get based on this.
            //Student Type : 1-Live //2-History //3-Promotion List //4-Approval List //5-Registered List
            if (Chk_SearchList[2] == "1")//1-Live  
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[3] == "1")//2-History
            {
                //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
                //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
                sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblview_student.AdmitionNo= '" + Txt_Search + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
            }
            if (Chk_SearchList[4] == "1")//3-Promotion List
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AAdharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[5] == "1")//4-Approval List
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.AdmitionNo = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[6] == "1")//5-Registered List
                sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and tbltempstdent.TempId='" + Txt_Search + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
            sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')";
        }

        else if (Drp_SearchBy == 1 && Txt_Search != "")
        {
            if (Chk_SearchList[2] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType ,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.StudentName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[3] == "1")
            {
                //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.StudentName='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
                //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.StudentName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1  AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
                sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch where tblview_student.Status<>(1 or 2) AND tblview_student.StudentName ='" + Txt_Search + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc ,tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
            }
            if (Chk_SearchList[4] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.StudentName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[5] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType, 0 as RollNo,tblstudent.StudentId ,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.StudentName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[6] == "1")
                sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType, 0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tbltempstdent.Name='" + Txt_Search + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
            sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')";
        }

        else if (Drp_SearchBy == 2 && Txt_Search != "")
        {
            // sql = "SELECT s.Id,s.StudentName,s.AdmitionNo,c.ClassName,  DATE_FORMAT(s.DOB,'%d/%m/%Y') as DOB, s.Sex,s.GardianName, s.Address, s.Pin, s.ResidencePhNo,s.OfficePhNo,  s.Email, tblreligion.Religion,(select tblcast.castname from tblcast where tblcast.Id= s.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, s.FatherEduQuali as FathersQualification, s.FatherOccupation, round( s.AnnualIncome,2) as AnnualIncome, s.MothersName, s.MotherEduQuali as MothersQualification, s.Addresspresent, s.Nationality, s.NumberofBrothers, s.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= s.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= s.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= s.StudTypeId) as StudentType, DATE_FORMAT(s.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(s.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving from tblstudent s inner join tblstudentclassmap sc on s.Id=StudentId INNER JOIN tblclass c on sc.ClassId=c.Id  inner join tblbloodgrp on tblbloodgrp.Id = s.BloodGroup inner join tblreligion on tblreligion.Id = s.Religion where s.Status=1 AND  c.ClassName LIKE '%" + Txt_Search.Text + "%' AND sc.BatchId=" + MyUser.CurrentBatchId + " AND s.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + " )))";
            if (Chk_SearchList[2] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus ,'Current' as StudType,tblstudent.RollNo as RollNo,tblstudent.StudentId , tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblclass.ClassName LIKE '" + Txt_Search + "' AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[3] == "1")
            {
                //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblclass.ClassName LIKE '%" + Txt_Search.Text + "%'  AND tblstudent_history.Status<>1   AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
                //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<> AND tblclass.ClassName LIKE '%" + Txt_Search.Text + "%' AND tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
                sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId, tbladmisiontype.Name as 'admtyp',IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id inner join tbladmisiontype on tbladmisiontype.Id = tblview_student.AdmissionTypeId inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblclass.ClassName LIKE '" + Txt_Search + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.ClassId , tblview_student.StudentName) UNION ";
            }
            if (Chk_SearchList[4] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId, tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblclass.ClassName LIKE '" + Txt_Search + "' AND tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[5] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType, 0 as RollNo,0 as StudentId, tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblclass.ClassName LIKE '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[6] == "1")
                //sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType,0 as RollNo,0 as StudnetId from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
                sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType,0 as RollNo,'' as admtyp,0 as StudnetId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tblclass.ClassName LIKE '" + Txt_Search + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
            sql = sql + " (SELECT 0 as Id,'' as StudentName,'' as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as admtyp,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
        }
        else if (Drp_SearchBy == 3 && Txt_Search != "")
        {

            //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
            if (Chk_SearchList[2] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND JoiningBatch.BatchName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[3] == "1")
            {
                //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
                //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
                sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel ,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND JoiningBatch.BatchName = '" + Txt_Search + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
            }
            if (Chk_SearchList[4] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND JoiningBatch.BatchName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[5] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,0 as StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND JoiningBatch.BatchName = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[6] == "1")
                sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class   INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 AND JoiningBatch.BatchName = '" + Txt_Search + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
            sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
        }
        else if (Drp_SearchBy == 4 && Txt_Search != "")
        {

            //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
            if (Chk_SearchList[2] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.StudentId = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[3] == "1")
            {
                //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
                //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
                sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblview_student.StudentId= '" + Txt_Search + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
            }
            if (Chk_SearchList[4] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.StudentId = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[5] == "1")
                sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,0 as StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.StudentId = '" + Txt_Search + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
            if (Chk_SearchList[6] == "1")
                sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and tbltempstdent.TempId='" + Txt_Search + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
            sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType , 0 as RollNo ,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
        }

        else
        {
            sql = "SELECT Id,StudentName,AdmitionNo as AdmissionNo  FROM tblstudent where Id=-2";
        }
        GenClas dd = new GenClas();
        string[] stDt = new string[2];
        stDt = dd.FillStdntDt(sql);
        if (stDt != null)
            return stDt;
        return null;

    }
    [WebMethod(EnableSession = true)]
    public static string[] StudentDtlsview(string StdntId, string stdntStatus,string action)
    {
        if (action != null)
        {
           // int index = Convert.ToInt32(StdntId);
            KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
            StudentManagerClass MyStudMang = MyUser.GetStudentObj();
            string[] urlRedirect = new string[4];
            urlRedirect[0] = null; urlRedirect[1] = null; urlRedirect[2] = null; urlRedirect[3] = null;
            int StudentId = int.Parse(StdntId);
            int StudentStatus = int.Parse(stdntStatus);
            urlRedirect[2] = StudentId.ToString();
            urlRedirect[3] = StudentStatus.ToString();
            if (action == "view")
            {
                if (StudentStatus == 1 || StudentStatus == 2 || StudentStatus == 3)
                {
                    HttpContext.Current.Session["StudId"] = StudentId;
                    HttpContext.Current.Session["StudType"] = StudentStatus;
                    urlRedirect[0]="StudentDetails.aspx";
                    urlRedirect[1] = "";
                  //  Response.Redirect("StudentDetails.aspx");
                }
                else if (StudentStatus == 4)
                {
                    urlRedirect[0] = "SutdDetailsPupUp.aspx?StudId=" + StudentId + "'";
                    urlRedirect[1] = "Registered students can edit at View Registered Students Page.";
                    ////  ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('SutdDetailsPupUp.aspx?StudId=" + StudentId + "');", true);
                }
                else if (StudentStatus == 5)
                {
                    urlRedirect[0] = "ViewRegisteredStudents.aspx";
                    urlRedirect[1] = "Registered students can edit at View Registered Students Page.";
                }
                else if (MyUser.HaveActionRignt(606))
                {
                    //HiddenField Hdn = new HiddenField();
                    //Hdn = (HiddenField)Grd_StudentList.Rows[index].FindControl("Hdn_TempId");
                    //string TempId = Hdn.Value;
                    //int ClassId = 0;
                    //string sql = "";
                    //OdbcDataReader Classreader = null;

                    //sql = "Select Class from tbltempstdent where tbltempstdent.TempId='" + TempId + "'";
                    //Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    //if (Classreader.HasRows)
                    //{
                    //    int.TryParse(Classreader.GetValue(0).ToString(), out ClassId);
                    //}

                    //// ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegisteredStudentDetails.aspx?TempStudId=" + TempId + "&ClassId=" + ClassId + "');", true);
                }
                else
                    urlRedirect[1]="You do not have sufficient rights to perform this action. Contact administrator";
            }
            else if (action == "edit")
            {
                if (StudentStatus == 1)
                {
                    if (MyUser.HaveActionRignt(4))
                    {
                        int id = StudentId;
                        ////    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('ManageStudentBulk.aspx?StudId=" + id + "');", true);
                        HttpContext.Current.Session["StudId"] = StudentId;
                        HttpContext.Current.Session["StudType"] = StudentStatus;
                        urlRedirect[0] = "ManageStudent.aspx";
                        urlRedirect[1] = "";
                    }
                    else
                        urlRedirect[1] = "You do not have sufficient rights to perform this action. Contact administrator";

                }
                else if (StudentStatus == 5)
                {
                    urlRedirect[0] = "ViewRegisteredStudents.aspx";
                    urlRedirect[1] = "Registered students can edit at View Registered Students Page.";
                }
                else
                {
                    urlRedirect[1] = "Cannot edit the student";
                }
            }
            else if (action == "fees")
            {
                if (StudentStatus == 1 || StudentStatus == 3)
                {
                    if (MyUser.HaveActionRignt(2))
                    {
                        int RollNumber = -1;
                        int ClassID = MyStudMang.GetClassNroll(StudentId, MyUser.CurrentBatchId, out RollNumber);
                        
                        urlRedirect[0]="CollectFeeAccount.aspx?ClassId=" + ClassID + "&RollNumber=" + RollNumber + "&StudentId=" + StudentId + "";
                    }
                    else
                        urlRedirect[1]="You do not have sufficient rights to perform this action. Contact administrator";

                }
                else if (StudentStatus == 5)
                {
                    if (MyUser.HaveActionRignt(605))
                    {
                       urlRedirect[0] = "CollectJoiningFee.aspx?Studentid=" + StudentId;
                    }
                    else
                       urlRedirect[1]= "You do not have sufficient rights to perform this action. Contact administrator";
                }
                else
                {
                    urlRedirect[1] = "Not possible to collect fees";
                }
            }
            return urlRedirect;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string[] AdvancedSearch(string contextKey, string adv_srchItems)
    {
        string sql = "";
        string _sqlorderby_last = "";
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        string[] Chk_SearchList = contextKey.Split('\\');//Live//history//
        string[] advSrch = adv_srchItems.Split('\\');//adv. search 

        if (Chk_SearchList[2] == "1")
        {
            sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName  as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1  AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
            if (int.Parse(advSrch[0]) > 0)
            {
                sql = sql + " and tblstudentclassmap.ClassId=" + int.Parse(advSrch[0]);
            }
            if (advSrch[1] != "All")
            {
                sql = sql + " and tblstudent.Sex='" + advSrch[1] + "'";
            }
            if (int.Parse(advSrch[2]) > 0)
            {
                sql = sql + " and tblstudent.BloodGroup=" + int.Parse(advSrch[2]);
            }
            if (int.Parse(advSrch[3]) != 0)
            {
                sql = sql + " and tblstudent.Religion=" + int.Parse(advSrch[3]);
            }
            if (int.Parse(advSrch[4]) != 0)
            {
                sql = sql + " and tblstudent.Cast=" + int.Parse(advSrch[4]);
            }
            if (int.Parse(advSrch[5]) > 0)
            {
                sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(advSrch[5]);
            }
            if (int.Parse(advSrch[6]) > 0)
            {
                sql = sql + " and tblstudent.StudTypeId=" + int.Parse(advSrch[6]);
            }

            if (int.Parse(advSrch[7]) != -1)
            {
                sql = sql + " and tblstudent.UseHostel=" + int.Parse(advSrch[7]);
            }
            if (int.Parse(advSrch[8]) != -1)
            {
                sql = sql + " and tblstudent.UseBus=" + int.Parse(advSrch[8]);
            }
            if (int.Parse(advSrch[9]) < 0)
            {
                sql = sql + " and tblstudent.JoinBatch=" + int.Parse(advSrch[9]);
            }

            sql = sql + " UNION ";
            _sqlorderby_last = ",RollNo";
        }


        if (Chk_SearchList[3] == "1")
        {
            sql = sql + "SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblclass.Standard,tblview_student.StudentId,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id AND tblview_studentclassmap.BatchId IN (SELECT MAX(test.BatchId) FROM tblview_studentclassmap test WHERE test.StudentId=tblview_student.Id) inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Live=0   AND tblview_student.Id IN (SELECT tblview_studentclassmap.StudentId FROM tblview_studentclassmap WHERE tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
            if (int.Parse(advSrch[0]) > 0)
            {
                sql = sql + " and tblview_studentclassmap.ClassId=" + int.Parse(advSrch[0]);
            }
            if (advSrch[1] != "All")
            {
                sql = sql + " and tblview_student.Sex='" + advSrch[1] + "'";
            }
            if (int.Parse(advSrch[2]) > 0)
            {
                sql = sql + " and tblview_student.BloodGroup=" + int.Parse(advSrch[2]);
            }
            if (int.Parse(advSrch[3]) != 0)
            {
                sql = sql + " and tblview_student.Religion=" + int.Parse(advSrch[3]);
            }
            if (int.Parse(advSrch[4]) != 0)
            {
                sql = sql + " and tblview_student.Cast=" + int.Parse(advSrch[4]);
            }
            if (int.Parse(advSrch[5]) > 0)
            {
                sql = sql + " and tblview_student.AdmissionTypeId=" + int.Parse(advSrch[5]);
            }
            if (int.Parse(advSrch[6]) > 0)
            {
                sql = sql + " and tblview_student.StudTypeId=" + int.Parse(advSrch[6]);
            }
            if (int.Parse(advSrch[7]) != -1)
            {
                sql = sql + " and tblview_student.UseHostel=" + int.Parse(advSrch[7]);
            }
            if (int.Parse(advSrch[8]) != -1)
            {
                sql = sql + " and tblview_student.UseBus=" + int.Parse(advSrch[8]);
            }
            if (int.Parse(advSrch[9]) < 0)
            {
                sql = sql + " and tblstudent.JoinBatch=" + int.Parse(advSrch[9]);
            }

            sql = sql + " UNION ";
            _sqlorderby_last = ",RollNo";
        }
        if (Chk_SearchList[4] == "1")
        {
            sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) ";
            // sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
            if (int.Parse(advSrch[0]) > 0)
            {
                sql = sql + " and tblstudentclassmap_history.ClassId=" + int.Parse(advSrch[0]);
            }
            if (advSrch[1] != "All")
            {
                sql = sql + " and tblstudent.Sex='" + advSrch[1] + "'";
            }
            if (int.Parse(advSrch[2]) > 0)
            {
                sql = sql + " and tblstudent.BloodGroup=" + int.Parse(advSrch[2]);
            }
            if (int.Parse(advSrch[3]) != 0)
            {
                sql = sql + " and tblstudent.Religion=" + int.Parse(advSrch[3]);
            }
            if (int.Parse(advSrch[4]) != 0)
            {
                sql = sql + " and tblstudent.Cast=" + int.Parse(advSrch[4]);
            }
            if (int.Parse(advSrch[5]) > 0)
            {
                sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(advSrch[5]);
            }
            if (int.Parse(advSrch[6]) > 0)
            {
                sql = sql + " and tblstudent.StudTypeId=" + int.Parse(advSrch[6]);
            }
            if (int.Parse(advSrch[7]) != -1)
            {
                sql = sql + " and tblstudent.UseHostel=" + int.Parse(advSrch[7]);
            }
            if (int.Parse(advSrch[8]) != -1)
            {
                sql = sql + " and tblstudent.UseBus=" + int.Parse(advSrch[8]);
            }
            if (int.Parse(advSrch[9]) < 0)
            {
                sql = sql + " and tblstudent.JoinBatch=" + int.Parse(advSrch[9]);
            }
            sql = sql + " UNION ";
            _sqlorderby_last = ",RollNo";
        }
        if (Chk_SearchList[5] == "1")
        {
            sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType ,tblstudentclassmap.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2  AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
            if (int.Parse(advSrch[0]) > 0)
            {
                sql = sql + " and tblstudentclassmap.ClassId=" + int.Parse(advSrch[0]);
            }
            if (advSrch[1] != "All")
            {
                sql = sql + " and tblstudent.Sex='" + advSrch[1] + "'";
            }
            if (int.Parse(advSrch[2]) > 0)
            {
                sql = sql + " and tblstudent.BloodGroup=" + int.Parse(advSrch[2]);
            }
            if (int.Parse(advSrch[3]) != 0)
            {
                sql = sql + " and tblstudent.Religion=" + int.Parse(advSrch[3]);
            }
            if (int.Parse(advSrch[4]) != 0)
            {
                sql = sql + " and tblstudent.Cast=" + int.Parse(advSrch[4]);
            }
            if (int.Parse(advSrch[5]) > 0)
            {
                sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(advSrch[5]);
            }
            if (int.Parse(advSrch[6]) > 0)
            {
                sql = sql + " and tblstudent.StudTypeId=" + int.Parse(advSrch[6]);
            }
            if (int.Parse(advSrch[7]) != -1)
            {
                sql = sql + " and tblstudent.UseHostel=" + int.Parse(advSrch[7]);
            }
            if (int.Parse(advSrch[8]) != -1)
            {
                sql = sql + " and tblstudent.UseBus=" + int.Parse(advSrch[8]);
            }
            if (int.Parse(advSrch[9]) < 0)
            {
                sql = sql + " and tblstudent.JoinBatch=" + int.Parse(advSrch[9]);
            }
            sql = sql + " UNION ";
            _sqlorderby_last = ",RollNo";
        }
        if (Chk_SearchList[6] == "1")
        {
            sql = sql + "SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,DATE_FORMAT(tbltempstdent.DOB,'%d/%m/%Y')  as DOB,tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,tbltempstdent.Email as Email, '' as Religion,'' as Caste,tblbloodgrp.GroupName as BloodGroup ,tbltempstdent.FatherEduQualification as FathersQualification,tbltempstdent.FatherOccupation as FatherOccupation , 0 as AnnualIncome , tbltempstdent.MotherName as MothersName,tbltempstdent.MotherEduQualification as MothersQualification,tbltempstdent.MotherOccupation as MotherOccupation, '' as Addresspresent,tbltempstdent.Nationality as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tbltempstdent.MotherTongue) as MotherTongue, '' as FirstLanguage, '' as StudentType,'' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType ,0 as RollNo,tblclass.Standard,0 as StudentId,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class inner join tblbloodgrp on tblbloodgrp.Id = tbltempstdent.BloodGroup   INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch  where  tbltempstdent.Status = 1 and  tbltempstdent.Name<>'OIOUH23'  ";
            if (int.Parse(advSrch[0]) > 0)
            {
                sql = sql + " and tbltempstdent.Class=" + int.Parse(advSrch[0]);
            }
            if (advSrch[1] != "All")
            {
                sql = sql + " and tbltempstdent.Gender='" + advSrch[1] + "'";
            }
            sql = sql + " AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)  UNION ";
            _sqlorderby_last = ",StudentName ";
        }

        sql = sql + " SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,tblclass.Standard,0 as StudentId,'' as AadahrNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR' ORDER BY Standard,ClassName" + _sqlorderby_last;

        GenClas dd = new GenClas();
        string[] stDt = new string[2];
        stDt = dd.FillStdntDt(sql);
        if (stDt != null)
            return stDt;
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string[] ExportResultToExcel()
    {
        KnowinUser MyUser = (KnowinUser)HttpContext.Current.Session["UserObj"];
        DataSet MydataSet = (DataSet)HttpContext.Current.Session["StudentList"];
        //MydataSet.Tables[0].Columns.Remove("Id");
        //MydataSet.Tables[0].Columns.Remove("StudStatus");
         string[] JsonDt = new string[2];
        // Arun added  08-may-12
        if (MydataSet.Tables[0].Columns.Contains("Standard"))
        {
            MydataSet.Tables[0].Columns.Remove("Standard");
        }
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            string FileName = "StudentsList";
            string _ReportName = "StudentsList";
            DataTable dtAccountData = MydataSet.Tables[0];
            ArrayList myArrayList = new ArrayList();
            for (int i = 0; i < dtAccountData.Rows.Count; i++)
            {
                string[] stringArr = dtAccountData.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();
                myArrayList.Add(stringArr);
            }
            JsonDt[0] = FileName;
            JsonDt[1]=_ReportName;
            JsonDt[2] = JsonConvert.SerializeObject(myArrayList);

            return JsonDt;
           // WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader);
             
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static void ExportResultTExcelWithImg()
    {

    }
    #region load All Drpdwn For Adv. Search
    [WebMethod(EnableSession = true)]
    public static string LoadAllClass()
    {
       // KnowinUser MyUser = new KnowinUser();
        GenClas gtcls = new GenClas();
        string dt =  gtcls.GetAllClsData();
        if (dt.Length > 0)
        {
            return dt;
        }
        return dt;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllBloodGroup()
    {
        string sql = "SELECT Id,GroupName FROM tblbloodgrp";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllReligion()
    {
        string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllCaste()
    {
        string sql = "select tblcast.Id, tblcast.castname from tblcast ";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllBatch()
    {
        string sql = "select Id,BatchName from tblbatch where Created=1 ORDER BY BatchName";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllStudntType()
    {
        string sql = "select Id,TypeName from tblstudtype ";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllLanguages()
    {
        string sql = "select Id,tbllanguage.Language from tbllanguage";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAllGenderType()
    {
        string sql = "select Id,gentname from tblgender";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    [WebMethod(EnableSession = true)]
    public static string LoadAdmisnType()
    {
        string sql = "select Id,name from tbladmisiontype";
        GenClas dt = new GenClas();
        string objDt = dt.GetDataFromDb(sql);
        if (objDt != null)
        {
            return objDt;
        }
        return null;
    }
    #endregion

    private void SetDefaultSearchByType()
    {
        string sql = "SELECT Value FROM  tblconfiguration WHERE Name='Default Type'";
        MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
           // Drp_SearchBy.SelectedValue = MyReader.GetValue(0).ToString();
        }
    }
    protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
    {
        // MydataSet = (DataSet)ViewState["StudentList"];
        MydataSet = (DataSet)Session["StudentList"];
      //  MydataSet.Tables[0].Columns.Remove("StudStatus");
        if (MydataSet.Tables[0].Columns.Contains("Id"))
        {
            MydataSet.Tables[0].Columns.Remove("Id");
        }
        // Arun added  08-may-12
        if (MydataSet.Tables[0].Columns.Contains("Standard"))
        {
            MydataSet.Tables[0].Columns.Remove("Standard");
        }
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Students List.xls"))
            //{
            //}
            string FileName = "StudentsList";
            string _ReportName = "StudentsList";
            if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
            {

            }
        }
    }
    //manikandan Code Export image Grid to Excel added 06.06.2012
    [STAThread]
    protected void Img_ExcelwithStudentImage_Click(object sender, ImageClickEventArgs e)
    {
        string _err = "";
        try
        {
            MydataSet = new DataSet();

            #region check student limit

            //  DataSet MyappDs = (DataSet)ViewState["StudentList"];
            DataSet MyappDs = (DataSet)Session["StudentList"];
            DataTable objTable = null;

            if (MyappDs.Tables[0].Rows.Count > 20)
            {
                if (MyappDs.Tables[0].Rows.Count > 100)
                {
                    _err = "You cant export more then 100 students.because of our software only export below 100 students.";
                  //  WC_MessageBox.ShowMssage(_err);
                }
                else
                {
                    int rowcount = 0;
                    int Tblcount = 0;
                    int Tblrowcount = 1;
                    foreach (DataRow dr in MyappDs.Tables[0].Rows)
                    {
                        if (rowcount == 0)
                        {
                            objTable = new DataTable("Table" + Tblcount);
                            foreach (DataColumn dc in MyappDs.Tables[0].Columns)
                            {
                                objTable.Columns.Add(dc.ColumnName, typeof(string));
                            }
                            Tblcount++;
                        }
                        objTable.ImportRow(dr);
                        rowcount++;
                        if (rowcount == 20 || Tblrowcount == MyappDs.Tables[0].Rows.Count)
                        {
                            MydataSet.Tables.Add(objTable);
                            rowcount = 0;
                        }
                        Tblrowcount++;
                    }
                }
            }
            else
                MydataSet = MyappDs;

            #endregion

            if (_err == "")
            {

                #region export student details with image

                ExcelFile ef = new ExcelFile();
                string Exportxlpath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\ThumbnailImages\\";
                string Studentimgpath = HttpContext.Current.Server.MapPath("~/images/");
                string sql2 = "SELECT Address FROM tblschooldetails";
                MydataSet2 = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql2);
                int i = 0;
                foreach (DataTable Mydatatable in MydataSet.Tables)
                {
                    Mydatatable.Columns.Remove("StudStatus");
                    if (Mydatatable.Columns.Contains("Standard"))
                        Mydatatable.Columns.Remove("Standard");


                    #region colllect excel datas

                    ExcelWorksheet ws = ef.Worksheets.Add("Insert DataTable" + i);

                    ws.Cells[0, 0].Value = MyUser.SchoolName;

                    if (MydataSet2 != null && MydataSet2.Tables[0].Rows.Count > 0)
                        ws.Cells[1, 0].Value = MydataSet2.Tables[0].Rows[0][0].ToString();
                    ws.Cells[2, 0].Value = "StudentList";
                    ws.Cells[3, 0].Value = "Student image";

                    int _i = 4;
                    int _count = 3 + Mydatatable.Rows.Count;
                    int originalcolumn = Mydatatable.Rows.Count;
                    int j = 0;
                    int _stuId = 0;

                    //DataSet ds = new DataSet();
                    //DataSet deleteds = new DataSet();
                    //deleteds = MydataSet;
                    //DataSet _deleteds = deleteds;
                    //ds = MydataSet;

                    DataSet ImgDs = null;

                    foreach (DataRow innerow in Mydatatable.Rows)
                    {

                        string Img_name = "_temp_stdnt_img.png";
                        string Imgpath = "";
                        _stuId = int.Parse(innerow[0].ToString());

                        #region get img from database
                        string sql = "SELECT tblfileurl.FileBytes,tblfileurl.FilePath from tblfileurl where tblfileurl.UserId=" + _stuId + " and tblfileurl.Type='StudentImage'";
                        ImgDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                        if (ImgDs != null && ImgDs.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ImgDs.Tables[0].Rows)
                            {

                               // Img_name = dr[1].ToString();
                                byte[] image = (byte[])dr[0];
                                Imgpath = Studentimgpath + Img_name;
                                if (!GetImageLinkForBytes(image, Imgpath))
                                {
                                    if (!Directory.Exists(Studentimgpath))
                                        Directory.CreateDirectory(Studentimgpath);
                                    Imgpath = Studentimgpath + "_temp_stdnt_img.png";
                                }
                            }

                        }
                        else
                        {
                            if (!Directory.Exists(Studentimgpath))
                                Directory.CreateDirectory(Studentimgpath);
                            Imgpath = Studentimgpath + "_temp_stdnt_img_default.png";
                        }

                        #endregion

                        j = _i;
                        ws.Pictures.Add(Imgpath, PositioningMode.Move, new AnchorCell(ws.Columns[0], ws.Rows[j], 100000, 50000), new AnchorCell(ws.Columns[1], ws.Rows[j + 6], 500000, 200000));
                        _i = _i + 7;
                    }

                    #endregion

                    //Mydatatable.Columns.Remove("Id");
                    DataTable objDataTable = AddRowPadding(Mydatatable, 6);
                    ws.InsertDataTable(objDataTable, 3, 2, true);

                    #region delete all student imag
                    ImgDs = null;
                    foreach (DataRow innerow in Mydatatable.Rows)
                    {
                        int stuid = int.Parse(innerow[0].ToString());
                        string sql = "SELECT tblfileurl.FileBytes,tblfileurl.FilePath from tblfileurl where tblfileurl.UserId=" + stuid + " and tblfileurl.Type='StudentImage'";
                        ImgDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                        if (ImgDs != null && ImgDs.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ImgDs.Tables[0].Rows)
                            {
                                string deleteimgpath = HttpContext.Current.Server.MapPath("~/images/");
                                if (File.Exists(deleteimgpath + dr[1].ToString()))
                                    File.Delete(deleteimgpath + dr[1].ToString());
                            }

                        }
                    }
                    #endregion

                    i++;

                }

                #endregion

                #region result
                string outpath = Exportxlpath + "\\studentreport.xlsx";
                ef.SaveXlsx(outpath);
                System.IO.FileInfo file = new System.IO.FileInfo(outpath);



                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    Response.End();
                }
                #endregion

            }
        }
        catch (Exception ex)
        {
           // WC_MessageBox.ShowMssage(_err + "......." + ex.Message);
        }

    }

    #region Export img to Excel
    private bool GetImageLinkForBytes(byte[] _Imagebytes, string Studentimgpath)
    {
        bool valid = true;
        try
        {
            if (_Imagebytes != null)
            {
                System.Drawing.Image image;
                var inputStream = new MemoryStream(_Imagebytes);
                image = System.Drawing.Image.FromStream(inputStream);
                using (System.Drawing.Image Img = image)
                {
                    System.Drawing.Size ThumbNailSize = NewImageSize(Img.Height, Img.Width,400);
                    using (System.Drawing.Image ImgThnail =
                    new System.Drawing.Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                    {
                        ImgThnail.Save(Studentimgpath, Img.RawFormat);
                        ImgThnail.Dispose();
                    }
                    Img.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            valid = false;
        }

        return valid;
    }

    public static String GetDateString(DateTime _dt)
    {
        return _dt.Year + "_" + _dt.Month + "_" + _dt.Day + "_" + _dt.Hour + "_" + _dt.Minute + "_" + _dt.Second;
    }

    private static System.Drawing.Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
    {
        System.Drawing.Size NewSize;
        double tempval;

        if (OriginalHeight > FormatSize && OriginalWidth > FormatSize)
        {
            if (OriginalHeight > OriginalWidth)
                tempval = FormatSize / Convert.ToDouble(OriginalHeight);
            else
                tempval = FormatSize / Convert.ToDouble(OriginalWidth);

            NewSize = new System.Drawing.Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
        }
        else
            NewSize = new System.Drawing.Size(OriginalWidth, OriginalHeight); return NewSize;
    }

    #endregion

    private DataTable AddRowPadding(DataTable Mydatatable, int count)
    {
        DataTable _newtable = new DataTable();
        _newtable = Mydatatable.Copy();
        _newtable.Clear();


        foreach (DataRow dr in Mydatatable.Rows)
        {
            DataRow _drow = _newtable.NewRow();


            for (int j = 0; j < Mydatatable.Columns.Count; j++)
            {
                _drow[j] = dr[j];
            }
            _newtable.Rows.Add(_drow);

            for (int i = 0; i < count; i++)
            {
                _drow = _newtable.NewRow();
                _newtable.Rows.Add(_drow);
            }

        }
        return _newtable;
    }

    // manikandan added 06.06.2012
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}

    public string GetUrl(string imagepath)
    {
        string[] splits = Request.Url.AbsoluteUri.Split('/');
        if (splits.Length >= 2)
        {
            string url = splits[0] + "//";
            for (int i = 2; i < splits.Length - 1; i++)
            {
                url += splits[i];
                url += "/";
            }
            return url += imagepath;
        }
        return imagepath;
    }
}



#region old code before applaying webmethod
//protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
//{
//    GrdPAgeIndex(e.NewPageIndex);
//}
//private void GrdPAgeIndex(int pageindex)
//{
//    Session["PageIndex"] = pageindex;
//    Grd_StudentList.PageIndex = pageindex;
//    //FillGrid();

//    DataSet MyDataSetNew = (DataSet)ViewState["StudentList"];
//    Grd_StudentList.Columns[0].Visible = true;
//    Grd_StudentList.Columns[1].Visible = true;
//    Grd_StudentList.Columns[2].Visible = true;
//    Grd_StudentList.Columns[3].Visible = true;
//    Grd_StudentList.Columns[4].Visible = true;
//    Grd_StudentList.Columns[5].Visible = true;
//    Grd_StudentList.Columns[6].Visible = true;


//    DataTable dtAgent = MyDataSetNew.Tables[0];
//    DataView dataView = new DataView(dtAgent);
//    if (Session["SortDirection"] != null && Session["SortExpression"] != null)
//    {
//        dataView.Sort = (string)Session["SortExpression"] + " " + (string)Session["SortDirection"];
//    }

//    Grd_StudentList.DataSource = dataView;
//    Grd_StudentList.DataBind();

//    Grd_StudentList.Columns[0].Visible = false;
//    Grd_StudentList.Columns[1].Visible = false;
//    Grd_StudentList.Columns[2].Visible = false;
//    Grd_StudentList.Columns[3].Visible = false;
//    Grd_StudentList.Columns[4].Visible = false;
//    Grd_StudentList.Columns[5].Visible = false;
//    Grd_StudentList.Columns[6].Visible = false;

//    FillStudentImage();
//}
//protected void Btn_Search_Click(object sender, EventArgs e)
//{
//    Grd_StudentList.Columns[0].Visible = true;
//    Grd_StudentList.Columns[1].Visible = true;
//    Grd_StudentList.Columns[2].Visible = true;
//    Grd_StudentList.Columns[3].Visible = true;
//    Grd_StudentList.Columns[4].Visible = true;
//    Grd_StudentList.Columns[5].Visible = true;
//    Grd_StudentList.Columns[6].Visible = true;
//    Grd_StudentList.DataSource = null;
//    Grd_StudentList.DataBind();

//    Grd_StudentList.PageIndex = 0;
//    FillGrid();
//}
//private void FillGrid()
//{
//    string sql = "";
//    if (Drp_SearchBy.SelectedValue == "0" && Txt_Search.Text != "")
//    {

//        //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
//        if (Chk_SearchList.Items[0].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[1].Selected)
//        {
//            //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
//            //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//            sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblview_student.AdmitionNo= '" + Txt_Search.Text + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
//        }
//        if (Chk_SearchList.Items[2].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AAdharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[3].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[4].Selected)
//            sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and tbltempstdent.TempId='" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//        sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')";
//    }

//    else if (Drp_SearchBy.SelectedValue == "1" && Txt_Search.Text != "")
//    {
//        if (Chk_SearchList.Items[0].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType ,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.StudentName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[1].Selected)
//        {
//            //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.StudentName='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
//            //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.StudentName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1  AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//            sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch where tblview_student.Status<>(1 or 2) AND tblview_student.StudentName ='" + Txt_Search.Text + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc ,tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
//        }
//        if (Chk_SearchList.Items[2].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.StudentName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[3].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType, 0 as RollNo,tblstudent.StudentId ,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.StudentName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[4].Selected)
//            sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType, 0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tbltempstdent.Name='" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//        sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')";
//    }

//    else if (Drp_SearchBy.SelectedValue == "2" && Txt_Search.Text != "")
//    {
//        // sql = "SELECT s.Id,s.StudentName,s.AdmitionNo,c.ClassName,  DATE_FORMAT(s.DOB,'%d/%m/%Y') as DOB, s.Sex,s.GardianName, s.Address, s.Pin, s.ResidencePhNo,s.OfficePhNo,  s.Email, tblreligion.Religion,(select tblcast.castname from tblcast where tblcast.Id= s.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, s.FatherEduQuali as FathersQualification, s.FatherOccupation, round( s.AnnualIncome,2) as AnnualIncome, s.MothersName, s.MotherEduQuali as MothersQualification, s.Addresspresent, s.Nationality, s.NumberofBrothers, s.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= s.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= s.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= s.StudTypeId) as StudentType, DATE_FORMAT(s.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(s.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving from tblstudent s inner join tblstudentclassmap sc on s.Id=StudentId INNER JOIN tblclass c on sc.ClassId=c.Id  inner join tblbloodgrp on tblbloodgrp.Id = s.BloodGroup inner join tblreligion on tblreligion.Id = s.Religion where s.Status=1 AND  c.ClassName LIKE '%" + Txt_Search.Text + "%' AND sc.BatchId=" + MyUser.CurrentBatchId + " AND s.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + " )))";
//        if (Chk_SearchList.Items[0].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus ,'Current' as StudType,tblstudent.RollNo as RollNo,tblstudent.StudentId , tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[1].Selected)
//        {
//            //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblclass.ClassName LIKE '%" + Txt_Search.Text + "%'  AND tblstudent_history.Status<>1   AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
//            //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<> AND tblclass.ClassName LIKE '%" + Txt_Search.Text + "%' AND tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//            sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId, tbladmisiontype.Name as 'admtyp',IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id inner join tbladmisiontype on tbladmisiontype.Id = tblview_student.AdmissionTypeId inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblclass.ClassName LIKE '" + Txt_Search.Text + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.ClassId , tblview_student.StudentName) UNION ";
//        }
//        if (Chk_SearchList.Items[2].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId, tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[3].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType, 0 as RollNo,0 as StudentId, tbladmisiontype.Name as 'admtyp',IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch inner join tbladmisiontype on tbladmisiontype.Id = tblstudent.AdmissionTypeId where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[4].Selected)
//            //sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType,0 as RollNo,0 as StudnetId from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//            sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType,0 as RollNo,'' as admtyp,0 as StudnetId,'' as UseBus,'' as UseHostel from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and  tblclass.ClassName LIKE '" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//        sql = sql + " (SELECT 0 as Id,'' as StudentName,'' as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as admtyp,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
//    }
//    else if (Drp_SearchBy.SelectedValue == "3" && Txt_Search.Text != "")
//    {

//        //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
//        if (Chk_SearchList.Items[0].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND JoiningBatch.BatchName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[1].Selected)
//        {
//            //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
//            //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//            sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel ,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND JoiningBatch.BatchName = '" + Txt_Search.Text + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
//        }
//        if (Chk_SearchList.Items[2].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND JoiningBatch.BatchName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[3].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,0 as StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND JoiningBatch.BatchName = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[4].Selected)
//            sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class   INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 AND JoiningBatch.BatchName = '" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//        sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
//    }
//    else if (Drp_SearchBy.SelectedValue == "4" && Txt_Search.Text != "")
//    {

//        //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
//        if (Chk_SearchList.Items[0].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1 AND tblstudent.StudentId = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[1].Selected)
//        {
//            //sql = sql + " (SELECT tblstudent_history.Id,tblstudent_history.StudentName,tblstudent_history.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent_history.DOB,'%d/%m/%Y') as DOB, tblstudent_history.Sex, tblstudent_history.GardianName, tblstudent_history.Address, tblstudent_history.Pin, tblstudent_history.ResidencePhNo, tblstudent_history.OfficePhNo,  tblstudent_history.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent_history.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent_history.FatherEduQuali as FathersQualification, tblstudent_history.FatherOccupation, round( tblstudent_history.AnnualIncome,2) as AnnualIncome, tblstudent_history.MothersName, tblstudent_history.MotherEduQuali as MothersQualification, tblstudent_history.Addresspresent, tblstudent_history.Nationality, tblstudent_history.NumberofBrothers, tblstudent_history.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent_history.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent_history.StudTypeId) as StudentType, DATE_FORMAT(tblstudent_history.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent_history.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType ,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent_history inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent_history.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent_history.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent_history.Religion where  tblstudent_history.AdmitionNo='" + Txt_Search.Text + "' AND tblstudent_history.Status<>1   AND tblstudent_history.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))  order by tblstudentclassmap_history.ClassId , tblstudent_history.StudentName) UNION ";
//            //sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblstudentclassmap_history.RollNo as RollNo FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status<>1 AND tblstudent.AdmitionNo = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//            sql = sql + " (SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblview_student.StudentId,IF (tblview_student.UseBus=0 ,'No','Yes') as UseBus,IF (tblview_student.UseHostel=0 ,'No','Yes') as UseHostel,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Status<>(1 or 2) AND tblview_student.StudentId= '" + Txt_Search.Text + "'  AND tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")) AND tblview_studentclassmap.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblview_studentclassmap.BatchId desc , tblview_studentclassmap.ClassId , tblview_student.StudentName LIMIT 0,1) UNION ";
//        }
//        if (Chk_SearchList.Items[2].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.StudentId = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[3].Selected)
//            sql = sql + " (SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType,0 as RollNo,0 as StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2 AND tblstudent.StudentId = '" + Txt_Search.Text + "' AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId , tblstudent.StudentName) UNION ";
//        if (Chk_SearchList.Items[4].Selected)
//            sql = sql + " (SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,'' as DOB, tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus,'Registered' as StudType,0 as RollNo,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch where tbltempstdent.Status = 1 and tbltempstdent.TempId='" + Txt_Search.Text + "' AND tbltempstdent.TempId not in(select TempStudentId from tblstudent) order by tbltempstdent.Class , tbltempstdent.Name) UNION ";
//        sql = sql + " (SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation, '' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType , 0 as RollNo ,0 as StudentId,'' as UseBus,'' as UseHostel,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR')  order by StudentName Asc";
//    }

//    else
//    {
//        sql = "SELECT Id,StudentName,AdmitionNo as AdmissionNo  FROM tblstudent where Id=-2";
//    }

//    FillDatatoGrid(sql, true);
//}
//private void FillDatatoGrid(string sql, bool NeedSortingByName)
//{

//    MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
//    System.Threading.Thread.Sleep(500);
//    if (MydataSet.Tables[0].Rows.Count > 0)
//    {
//        Session["RsltQry"] = sql;
//        Grd_StudentList.Columns[0].Visible = true;
//        Grd_StudentList.Columns[1].Visible = true;
//        Grd_StudentList.Columns[2].Visible = true;
//        Grd_StudentList.Columns[3].Visible = true;
//        Grd_StudentList.Columns[4].Visible = true;
//        Grd_StudentList.Columns[5].Visible = true;
//        Grd_StudentList.Columns[6].Visible = true;
//        Grd_StudentList.Columns[7].Visible = true;


//        DataTable dtAgent = MydataSet.Tables[0];
//        DataView dataView = new DataView(dtAgent);
//        if (NeedSortingByName)
//        {
//            Session["SortDirection"] = "ASC";
//            Session["SortExpression"] = "StudentName";
//            if (Session["SortDirection"] != null && Session["SortExpression"] != null)
//            {
//                dataView.Sort = (string)Session["SortExpression"] + " " + (string)Session["SortDirection"];
//            }
//        }
//        //  ViewState["StudentList"] = MydataSet;
//        Session["StudentList"] = MydataSet;


//        //Grd_Student.DataSource = MydataSet;
//        //Grd_Student.DataBind();
//        Grd_StudentList.DataSource = dataView;
//        Grd_StudentList.DataBind();

//        // FillData();
//        Grd_StudentList.Columns[0].Visible = false;
//        Grd_StudentList.Columns[1].Visible = false;
//        Grd_StudentList.Columns[2].Visible = false;
//        Grd_StudentList.Columns[3].Visible = false;
//        Grd_StudentList.Columns[4].Visible = false;
//        Grd_StudentList.Columns[5].Visible = false;
//        Grd_StudentList.Columns[6].Visible = false;
//        Grd_StudentList.Columns[7].Visible = false;
//        FillStudentImage();

//        Pnl_Searchresult.Visible = true;
//        //Grd_Student.Focus();
//        img_export_Excel.Visible = true;
//        Img_ExcelwithStudentImage.Visible = true;
//    }
//    else
//    {
//        Grd_StudentList.DataSource = null;
//        Grd_StudentList.DataBind();
//        WC_MessageBox.ShowMssage("No Students Found");

//        Pnl_Searchresult.Visible = false;

//        img_export_Excel.Visible = false;
//        Img_ExcelwithStudentImage.Visible = false;
//        Session["RsltQry"] = null;
//    }
//}
//private void FillData()
//{
//    StringBuilder Text = new StringBuilder();
//    foreach (GridViewRow gv in Grd_StudentList.Rows)
//    {
//        string StudentImage = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[0].Text) + "&type=StudentImage";

//        //   MyUser.GetImageUrl("StudentImage", int.Parse(gv.Cells[0].Text));
//        HtmlGenericControl div = (HtmlGenericControl)gv.FindControl("DivDetailsInGrid");
//        Text.Append("<table width=\"100%\" ><tr><td align=\"center\"> <table width=\"100%\" > <tr><td style=\"width:15%;\"> <table><tr><td align=\"center\">");
//        Text.Append("<img src=" + StudentImage + " alt=\"No Pics\" style=\"width:75px;height:75px;border-style:solid;border-width:1px;border-color:Black;\" />");
//        Text.Append("</td> </tr> <tr> <td align=\"center\" rowspan=\"2\"></td></tr> </table></td>");
//        Text.Append("<td style=\"width:85%;\"  valign= \"top\"  > <table width=\"100%\"> <tr> <td style=\"text-decoration:none;color:Red; font-weight:bold; padding-left:2px\" >" + gv.Cells[2].Text + "</td><td style=\"text-decoration:none;color:Blue\" align=\"right\">&nbsp; &nbsp; (" + gv.Cells[3].Text + ")</td> </tr> </table>");
//        Text.Append(" <div class=\"linestyle\"></div>");

//        Text.Append("<table> <tr><td  style=\"padding-left:2px\">  Class   : </td>     <td style=\"text-decoration:none;color:Red; padding-left:2px\" align=\"left\">" + gv.Cells[4].Text + "</td>     <td valign=\"bottom\" align=\"right\" style=\"font-size:smaller;\" ></td></tr>");


//        Text.Append("<tr><td style=\"padding-left:2px\">  Roll.No : </td>      <td style=\"text-decoration:none;color:Red; padding-left:2px\" align=\"left\">6</td>      <td valign=\"bottom\" align=\"right\" style=\"font-size:smaller;\" ></td> </tr>");


//        Text.Append("<tr><td style=\"padding-left:2px\"> Ad.No   : </td><td style=\"text-decoration:none;color:Red; padding-left:2px\" align=\"left\">" + gv.Cells[5].Text + "</td> <td valign=\"bottom\" align=\"right\" style=\"font-size:smaller;\" ></td> </tr>");

//        Text.Append(" </table></td>    </tr>   </table>  </td>  </tr>  </table>");



//        div.InnerHtml = Text.ToString();
//    }
//}
//protected void Grd_studentlist_RowCommand(object sender, GridViewCommandEventArgs e)
//{
//    if (e.CommandName == "View" || e.CommandName == "EditStudent" || e.CommandName == "Fees")
//    {
//        int index = Convert.ToInt32(e.CommandArgument);
//        int StudentId = int.Parse(Grd_StudentList.Rows[index].Cells[0].Text);
//        int StudentStatus = int.Parse(Grd_StudentList.Rows[index].Cells[1].Text);
//        if (e.CommandName == "View")
//        {
//            if (StudentStatus == 1 || StudentStatus == 2 || StudentStatus == 3)
//            {
//                Session["StudId"] = StudentId;
//                Session["StudType"] = StudentStatus;
//                Response.Redirect("StudentDetails.aspx");

//            }
//            else if (StudentStatus == 4)
//            {
//                ////  ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('SutdDetailsPupUp.aspx?StudId=" + StudentId + "');", true);
//            }
//            else if (MyUser.HaveActionRignt(606))
//            {
//                HiddenField Hdn = new HiddenField();
//                Hdn = (HiddenField)Grd_StudentList.Rows[index].FindControl("Hdn_TempId");
//                string TempId = Hdn.Value;
//                int ClassId = 0;
//                string sql = "";
//                OdbcDataReader Classreader = null;

//                sql = "Select Class from tbltempstdent where tbltempstdent.TempId='" + TempId + "'";
//                Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
//                if (Classreader.HasRows)
//                {
//                    int.TryParse(Classreader.GetValue(0).ToString(), out ClassId);
//                }

//                //// ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegisteredStudentDetails.aspx?TempStudId=" + TempId + "&ClassId=" + ClassId + "');", true);
//            }
//            else
//                WC_MessageBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");
//        }
//        if (e.CommandName == "EditStudent")
//        {
//            if (StudentStatus == 1)
//            {
//                if (MyUser.HaveActionRignt(4))
//                {

//                    int id = StudentId;
//                    ////    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('ManageStudentBulk.aspx?StudId=" + id + "');", true);



//                    Session["StudId"] = StudentId;
//                    //Session["StudType"] = StudentStatus;
//                    //Response.Redirect("ManageStudent.aspx");
//                }
//                else
//                    WC_MessageBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");

//            }
//            else if (StudentStatus == 5)
//            {
//                WC_MessageBox.ShowMssage("Registered students can edit at View Registered Students Page.");
//            }
//            else
//            {
//                WC_MessageBox.ShowMssage("Cannot edit the student");
//            }

//        }
//        if (e.CommandName == "Fees")
//        {
//            if (StudentStatus == 1 || StudentStatus == 3)
//            {
//                if (MyUser.HaveActionRignt(2))
//                {
//                    int RollNumber = -1;
//                    int ClassID = MyStudMang.GetClassNroll(StudentId, MyUser.CurrentBatchId, out RollNumber);
//                    Response.Redirect("CollectFeeAccount.aspx?ClassId=" + ClassID + "&RollNumber=" + RollNumber + "&StudentId=" + StudentId + "");
//                }
//                else
//                    WC_MessageBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");

//            }
//            else if (StudentStatus == 5)
//            {
//                if (MyUser.HaveActionRignt(605))
//                {
//                    Response.Redirect("CollectJoiningFee.aspx?Studentid=" + StudentId);
//                }
//                else
//                    WC_MessageBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");
//            }
//            else
//            {
//                WC_MessageBox.ShowMssage("Not possible to collect fees");
//            }
//        }
//    }
//}
//protected void Grd_Student_Sorting(object sender, GridViewSortEventArgs e)
//{
//    Grd_Student.Columns[0].Visible = true;
//    Grd_Student.Columns[1].Visible = true;
//    Grd_Student.PageIndex = 0;

//    DataSet MydataSet = (DataSet)ViewState["StudentList"];


//    DataTable dtCust = MydataSet.Tables[0];
//    DataView dataView = new DataView(dtCust);

//    dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
//    Grd_Student.DataSource = dataView;
//    Grd_Student.DataBind();
//    FillStudentImage();
//    Grd_Student.Columns[1].Visible = false;

//}
//private string GetSortDirection(string column)
//{

//    // By default, set the sort direction to ascending.
//    string sortDirection = "ASC";

//    // Retrieve the last column that was sorted.
//    string sortExpression = Session["SortExpression"] as string;

//    if (sortExpression != null)
//    {
//        // Check if the same column is being sorted.
//        // Otherwise, the default value can be returned.
//        if (sortExpression == column)
//        {
//            string lastDirection = Session["SortDirection"] as string;
//            if ((lastDirection != null) && (lastDirection == "ASC"))
//            {
//                sortDirection = "DESC";
//            }
//        }
//    }

//    // Save new values in ViewState.
//    Session["SortDirection"] = sortDirection;
//    Session["SortExpression"] = column;

//    return sortDirection;
//}
//private void FillStudentImage()
//{


//    Grd_StudentList.Columns[0].Visible = true;
//    foreach (GridViewRow gv in Grd_StudentList.Rows)
//        if (gv.Cells[1].Text != "5")
//        {

//            Image Img_stud = (Image)gv.FindControl("Img_studImage");
//            //Img_stud.ImageUrl = MyUser.GetImageUrl("StudentImage", int.Parse(gv.Cells[0].Text.ToString()));
//            Img_stud.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[0].Text.ToString()) + "&type=StudentImage";
//        }
//    Grd_StudentList.Columns[0].Visible = false;
//}
//protected void button2_Click(object sender, EventArgs e)
//{
//    Response.Redirect("StudentDetails.aspx");
//}
//protected void Drp_SearchBy_SelectedIndexChanged(object sender, EventArgs e)
//{
//    SetContextKey();
//}
//private void SetContextKey()
//{

//    int Live = 0;
//    int History = 0;
//    int PromotionList = 0;
//    int AprList = 0;
//    int RegList = 0;
//    if (Chk_SearchList.Items[0].Selected)
//        Live = 1;
//    if (Chk_SearchList.Items[1].Selected)
//        History = 1;
//    if (Chk_SearchList.Items[2].Selected)
//        PromotionList = 1;
//    if (Chk_SearchList.Items[3].Selected)
//        AprList = 1;
//    if (Chk_SearchList.Items[4].Selected)
//        RegList = 1;
//    Txt_Search_AutoCompleteExtender.ContextKey = "" + Drp_SearchBy.SelectedValue + "\\" + MyUser.UserId.ToString() + "\\" + Live + "" + "\\" + History + "" + "\\" + PromotionList + "" + "\\" + AprList + "" + "\\" + RegList + "";
//}
//protected void Chk_SearchList_SelectedIndexChanged(object sender, EventArgs e)
//{
//    SetContextKey();
//}

//#region ADVANCED SEARCH FUNCTIONS

//private void LoadJoiningBatch()
//{
//    Drp_JoiningBatch.Items.Clear();
//    string sql = "select Id,BatchName from tblbatch where Created=1 ORDER BY BatchName";
//    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

//    if (MyReader.HasRows)
//    {
//        Drp_JoiningBatch.Items.Add(new ListItem("ALL", "-1"));
//        while (MyReader.Read())
//        {
//            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
//            Drp_JoiningBatch.Items.Add(li);
//        }
//        Drp_JoiningBatch.SelectedIndex = 0;
//    }
//    else
//    {
//        Drp_JoiningBatch.Items.Add(new ListItem("No Joining Batch Found", "-1"));
//        Drp_JoiningBatch.SelectedIndex = 0;
//    }
//}

//private void LoadAllCasteToDropDown()
//{
//    Drp_Caste.Items.Clear();
//    string sql = "select tblcast.Id, tblcast.castname from tblcast ";
//    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

//    if (MyReader.HasRows)
//    {
//        Drp_Caste.Items.Add(new ListItem("ALL", "-1"));
//        while (MyReader.Read())
//        {
//            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
//            Drp_Caste.Items.Add(li);
//        }
//    }

//}

//private void LoadAllStudentTypeDropDown()
//{
//    Drp_StudentType.Items.Clear();
//    string sql = "select Id,TypeName from tblstudtype ";
//    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

//    if (MyReader.HasRows)
//    {
//        Drp_StudentType.Items.Add(new ListItem("ALL", "0"));
//        while (MyReader.Read())
//        {
//            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
//            Drp_StudentType.Items.Add(li);
//        }
//    }
//}
//protected void Btn_AdvancedSearch_Click(object sender, EventArgs e)
//{
//    Grd_StudentList.PageIndex = 0;
//    FillGridWithAdvancedSearchValues();
//}

//private void FillGridWithAdvancedSearchValues()
//{
//    string sql = "";
//    string _sqlorderby_last = "";

//    if (Chk_SearchList.Items[0].Selected)
//    {
//        sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName  as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving,1 as StudStatus,'Current' as StudType,tblstudentclassmap.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1  AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
//        if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudentclassmap.ClassId=" + int.Parse(Drp_AdvancedClass.SelectedValue);
//        }
//        if (int.Parse(Drp_Gender.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.Sex='" + Drp_Gender.SelectedItem.Text + "'";
//        }
//        if (int.Parse(Drp_BloodGroup.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.BloodGroup=" + int.Parse(Drp_BloodGroup.SelectedValue);
//        }
//        if (int.Parse(Drp_Religion.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Religion=" + int.Parse(Drp_Religion.SelectedValue);
//        }
//        if (int.Parse(Drp_Caste.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Cast=" + int.Parse(Drp_Caste.SelectedValue);
//        }
//        if (int.Parse(Drp_AdmissionType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(Drp_AdmissionType.SelectedValue);
//        }
//        if (int.Parse(Drp_StudentType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.StudTypeId=" + int.Parse(Drp_StudentType.SelectedValue);
//        }

//        if (int.Parse(Drp_UsingHostel.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseHostel=" + int.Parse(Drp_UsingHostel.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingBus.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseBus=" + int.Parse(Drp_UsingBus.SelectedValue);
//        }
//        if (int.Parse(Drp_JoiningBatch.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.JoinBatch=" + int.Parse(Drp_JoiningBatch.SelectedValue);
//        }

//        sql = sql + " UNION ";
//        _sqlorderby_last = ",RollNo";
//    }


//    if (Chk_SearchList.Items[1].Selected)
//    {
//        sql = sql + "SELECT tblview_student.Id,tblview_student.StudentName,tblview_student.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblview_student.DOB,'%d/%m/%Y') as DOB, tblview_student.Sex, tblview_student.GardianName as GuardianName, tblview_student.Address, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo,  tblview_student.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblview_student.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblview_student.FatherEduQuali as FathersQualification, tblview_student.FatherOccupation, round( tblview_student.AnnualIncome,2) as AnnualIncome, tblview_student.MothersName, tblview_student.MotherEduQuali as MothersQualification,tblview_student.MotherOccupation as MotherOccupation, tblview_student.Addresspresent, tblview_student.Nationality, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblview_student.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblview_student.StudTypeId) as StudentType, DATE_FORMAT(tblview_student.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblview_student.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving, 2 as StudStatus ,'Alumni' as StudType,tblview_studentclassmap.RollNo as RollNo,tblclass.Standard,tblview_student.StudentId,tblview_student.AadharNumber as AadharNumber FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id AND tblview_studentclassmap.BatchId IN (SELECT MAX(test.BatchId) FROM tblview_studentclassmap test WHERE test.StudentId=tblview_student.Id) inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblview_student.BloodGroup inner join tblreligion on tblreligion.Id = tblview_student.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblview_student.JoinBatch  where tblview_student.Live=0   AND tblview_student.Id IN (SELECT tblview_studentclassmap.StudentId FROM tblview_studentclassmap WHERE tblview_studentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
//        if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
//        {
//            sql = sql + " and tblview_studentclassmap.ClassId=" + int.Parse(Drp_AdvancedClass.SelectedValue);
//        }
//        if (int.Parse(Drp_Gender.SelectedValue) > 0)
//        {
//            sql = sql + " and tblview_student.Sex='" + Drp_Gender.SelectedItem.Text + "'";
//        }
//        if (int.Parse(Drp_BloodGroup.SelectedValue) > 0)
//        {
//            sql = sql + " and tblview_student.BloodGroup=" + int.Parse(Drp_BloodGroup.SelectedValue);
//        }
//        if (int.Parse(Drp_Religion.SelectedValue) != -1)
//        {
//            sql = sql + " and tblview_student.Religion=" + int.Parse(Drp_Religion.SelectedValue);
//        }
//        if (int.Parse(Drp_Caste.SelectedValue) != -1)
//        {
//            sql = sql + " and tblview_student.Cast=" + int.Parse(Drp_Caste.SelectedValue);
//        }
//        if (int.Parse(Drp_AdmissionType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblview_student.AdmissionTypeId=" + int.Parse(Drp_AdmissionType.SelectedValue);
//        }
//        if (int.Parse(Drp_StudentType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblview_student.StudTypeId=" + int.Parse(Drp_StudentType.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingHostel.SelectedValue) != -1)
//        {
//            sql = sql + " and tblview_student.UseHostel=" + int.Parse(Drp_UsingHostel.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingBus.SelectedValue) != -1)
//        {
//            sql = sql + " and tblview_student.UseBus=" + int.Parse(Drp_UsingBus.SelectedValue);
//        }

//        sql = sql + " UNION ";
//        _sqlorderby_last = ",RollNo";
//    }
//    if (Chk_SearchList.Items[2].Selected)
//    {
//        sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) ";
//        // sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId FROM tblstudent inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
//        if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudentclassmap_history.ClassId=" + int.Parse(Drp_AdvancedClass.SelectedValue);
//        }
//        if (int.Parse(Drp_Gender.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.Sex='" + Drp_Gender.SelectedItem.Text + "'";
//        }
//        if (int.Parse(Drp_BloodGroup.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.BloodGroup=" + int.Parse(Drp_BloodGroup.SelectedValue);
//        }
//        if (int.Parse(Drp_Religion.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Religion=" + int.Parse(Drp_Religion.SelectedValue);
//        }
//        if (int.Parse(Drp_Caste.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Cast=" + int.Parse(Drp_Caste.SelectedValue);
//        }
//        if (int.Parse(Drp_AdmissionType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(Drp_AdmissionType.SelectedValue);
//        }
//        if (int.Parse(Drp_StudentType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.StudTypeId=" + int.Parse(Drp_StudentType.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingHostel.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseHostel=" + int.Parse(Drp_UsingHostel.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingBus.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseBus=" + int.Parse(Drp_UsingBus.SelectedValue);
//        }
//        sql = sql + " UNION ";
//        _sqlorderby_last = ",RollNo";
//    }
//    if (Chk_SearchList.Items[3].Selected)
//    {
//        sql = sql + "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving ,4 as StudStatus ,'InApprovalList' as StudType ,tblstudentclassmap.RollNo as RollNo,tblclass.Standard,tblstudent.StudentId,tblstudent.AadharNumber as AadharNumber FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch  where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=2  AND tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) ";
//        if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudentclassmap.ClassId=" + int.Parse(Drp_AdvancedClass.SelectedValue);
//        }
//        if (int.Parse(Drp_Gender.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.Sex='" + Drp_Gender.SelectedItem.Text + "'";
//        }
//        if (int.Parse(Drp_BloodGroup.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.BloodGroup=" + int.Parse(Drp_BloodGroup.SelectedValue);
//        }
//        if (int.Parse(Drp_Religion.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Religion=" + int.Parse(Drp_Religion.SelectedValue);
//        }
//        if (int.Parse(Drp_Caste.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.Cast=" + int.Parse(Drp_Caste.SelectedValue);
//        }
//        if (int.Parse(Drp_AdmissionType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.AdmissionTypeId=" + int.Parse(Drp_AdmissionType.SelectedValue);
//        }
//        if (int.Parse(Drp_StudentType.SelectedValue) > 0)
//        {
//            sql = sql + " and tblstudent.StudTypeId=" + int.Parse(Drp_StudentType.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingHostel.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseHostel=" + int.Parse(Drp_UsingHostel.SelectedValue);
//        }
//        if (int.Parse(Drp_UsingBus.SelectedValue) != -1)
//        {
//            sql = sql + " and tblstudent.UseBus=" + int.Parse(Drp_UsingBus.SelectedValue);
//        }
//        sql = sql + " UNION ";
//        _sqlorderby_last = ",RollNo";
//    }
//    if (Chk_SearchList.Items[4].Selected)
//    {
//        sql = sql + "SELECT tbltempstdent.Id,tbltempstdent.Name as StudentName,tbltempstdent.TempId as AdmissionNo,tblclass.ClassName,DATE_FORMAT(tbltempstdent.DOB,'%d/%m/%Y')  as DOB,tbltempstdent.Gender as Sex,tbltempstdent.Fathername as GuardianName,tbltempstdent.Address,0 as Pin,'' as ResidencePhNo,tbltempstdent.PhoneNumber as OfficePhNo,tbltempstdent.Email as Email, '' as Religion,'' as Caste,tblbloodgrp.GroupName as BloodGroup ,tbltempstdent.FatherEduQualification as FathersQualification,tbltempstdent.FatherOccupation as FatherOccupation , 0 as AnnualIncome , tbltempstdent.MotherName as MothersName,tbltempstdent.MotherEduQualification as MothersQualification,tbltempstdent.MotherOccupation as MotherOccupation, '' as Addresspresent,tbltempstdent.Nationality as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tbltempstdent.MotherTongue) as MotherTongue, '' as FirstLanguage, '' as StudentType,'' as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', '' as DateOfLeaving ,5 as StudStatus ,'Registered' as StudType ,0 as RollNo,tblclass.Standard,0 as StudentId,'' as AadharNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class inner join tblbloodgrp on tblbloodgrp.Id = tbltempstdent.BloodGroup   INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tbltempstdent.JoiningBatch  where  tbltempstdent.Status = 1 and  tbltempstdent.Name<>'OIOUH23'  ";
//        if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
//        {
//            sql = sql + " and tbltempstdent.Class=" + int.Parse(Drp_AdvancedClass.SelectedValue);
//        }
//        if (int.Parse(Drp_Gender.SelectedValue) > 0)
//        {
//            sql = sql + " and tbltempstdent.Gender='" + Drp_Gender.SelectedItem.Text + "'";
//        }
//        sql = sql + " AND tbltempstdent.TempId not in(select TempStudentId from tblstudent)  UNION ";
//        _sqlorderby_last = ",StudentName ";
//    }

//    sql = sql + " SELECT 0 as Id,'' as StudentName,''  as AdmissionNo,'' as ClassName,'' as DOB, '' as Sex,'' as GuardianName,'' as Address,0 as Pin,'' as ResidencePhNo,'' as OfficePhNo,'' as Email, '' as Religion, '' as Caste,'' as BloodGroup , '' as FathersQualification, '' as FatherOccupation , 0 as AnnualIncome , '' as MothersName, '' as MothersQualification,'' as MotherOccupation,'' as Addresspresent, '' as Nationality, 0 as NumberofBrothers, 0 as NoOfSisters, '' as MotherTongue, '' as FirstLanguage, '' as StudentType, '' as DateofJoining,'' as 'Joining Batch', '' as DateOfLeaving ,0 as StudStatus ,'None' as StudType,0 as RollNo,tblclass.Standard,0 as StudentId,'' as AadahrNumber from tbltempstdent inner join tblclass on tblclass.Id = tbltempstdent.Class where tbltempstdent.Name='XFRTFGFGFVTYR' ORDER BY Standard,ClassName" + _sqlorderby_last;


//    FillDatatoGrid(sql, false);


//}

//protected void Lnk_AdvancedSearch_Click(object sender, EventArgs e)
//{
//    Drp_AdvancedClass.SelectedIndex = 0;
//    Drp_Gender.SelectedIndex = 0;
//    Drp_BloodGroup.SelectedIndex = 0;
//    Drp_Religion.SelectedIndex = -1;
//    Drp_Caste.SelectedIndex = -1;
//    Drp_AdmissionType.SelectedIndex = 0;
//    Drp_StudentType.SelectedIndex = 0;
//    Drp_UsingHostel.SelectedIndex = 0;
//    Drp_UsingBus.SelectedIndex = 0;
//    MPE_AdvancedSearch.Show();
//}


//private void LoadAllReligionToDropDown()
//{
//    Drp_Religion.Items.Clear();
//    string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
//    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
//    if (MyReader.HasRows)
//    {
//        Drp_Religion.Items.Add(new ListItem("ALL", "-1"));
//        while (MyReader.Read())
//        {
//            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
//            Drp_Religion.Items.Add(li);

//        }
//    }

//}

//private void LoadAllBloodGroupsToDropDown()
//{
//    Drp_BloodGroup.Items.Clear();

//    string sql = "SELECT Id,GroupName FROM tblbloodgrp";
//    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
//    if (MyReader.HasRows)
//    {
//        ListItem li = new ListItem("ALL", "0");
//        Drp_BloodGroup.Items.Add(li);
//        while (MyReader.Read())
//        {
//            li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
//            Drp_BloodGroup.Items.Add(li);
//        }
//    }
//}

//private void LoadAllClassToDropDown()
//{
//    Drp_AdvancedClass.Items.Clear();

//    MydataSet = MyUser.MyAssociatedClass();
//    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
//    {
//        Btn_AdvancedSearch.Enabled = true;
//        Drp_AdvancedClass.Items.Add(new ListItem("ALL", "0"));
//        foreach (DataRow dr in MydataSet.Tables[0].Rows)
//        {
//            ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
//            Drp_AdvancedClass.Items.Add(li);
//        }
//    }
//    else
//    {
//        Btn_AdvancedSearch.Enabled = false;
//        ListItem li = new ListItem("No Class present", "-1");
//        Drp_AdvancedClass.Items.Add(li);
//    }
//}

//#endregion
//protected void Img_ExcelwithStudentImage_Click(object sender, ImageClickEventArgs e)
//    {
//        MydataSet = (DataSet)ViewState["StudentList"];
//        DataTable Mydatatable = MydataSet.Tables[0];
//       // MydataSet.Tables[0].Columns.Remove("Id");
//        MydataSet.Tables[0].Columns.Remove("StudStatus");
//        if (MydataSet.Tables[0].Columns.Contains("Standard"))
//        {
//            MydataSet.Tables[0].Columns.Remove("Standard");
//        }
//        if (MydataSet.Tables[0].Rows.Count > 0)
//        {
//            string pathsource = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"\\ThumbnailImages\\";
//            string pathsource_temp = Server.MapPath("images");
//            ExcelFile ef = new ExcelFile();

//            ExcelWorksheet ws = ef.Worksheets.Add("Insert DataTable");



//            string sql1 = "SELECT SchoolName FROM tblschooldetails";
//            MydataSet1 = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql1);
//            if (MydataSet1 != null && MydataSet1.Tables[0].Rows.Count > 0)
//                ws.Cells[0, 0].Value = MydataSet1.Tables[0].Rows[0][0].ToString();

//            string sql2 = "SELECT Address FROM tblschooldetails";
//            MydataSet2 = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql2);
//            if (MydataSet2 != null && MydataSet2.Tables[0].Rows.Count > 0)
//                ws.Cells[1, 0].Value = MydataSet2.Tables[0].Rows[0][0].ToString();

//            ws.Cells[2, 0].Value = "StudentList";

//            ws.Cells[3, 0].Value = "Student image";
//            int _i = 4;
//            int _count = 3 + Mydatatable.Rows.Count;
//            int originalcolumn = Mydatatable.Rows.Count;
//            int j = 0;
//            int _stuId = 0;
//            DataSet ds = new DataSet();
//            ds = MydataSet;
//            string _imagename = "";

//            foreach (DataRow innerow in ds.Tables[0].Rows)
//            {
//                _imagename = "";
//                _stuId = int.Parse(innerow[0].ToString());
//                //manikandan Change code 20.17.2012
//                string t_sql = "select tblfileurl.FilePath from tblfileurl inner join tblstudent on tblstudent.Id=tblfileurl.UserId where tblstudent.Id="+_stuId+" And tblfileurl.Type='StudentImage'";
//                MydataSet3 = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(t_sql);
//                if (MydataSet3 != null && MydataSet3.Tables[0].Rows.Count > 0)
//                    _imagename = MydataSet3.Tables[0].Rows[0][0].ToString();

//                if (_imagename != null && _imagename!= "")
//                {
//                    string _studentPath = Path.Combine(pathsource, _imagename);
//                    if (!File.Exists(Path.Combine(pathsource, _imagename)))
//                    {
//                        _studentPath = Path.Combine(pathsource_temp, "stdnt.png");
//                    }
//                    j = _i;
//                    ws.Pictures.Add(_studentPath, PositioningMode.Move, new AnchorCell(ws.Columns[0], ws.Rows[j], 100000, 50000), new AnchorCell(ws.Columns[1], ws.Rows[j + 6], 500000, 200000));
//                    _i = _i + 7;

//                }
//                else
//                {
//                    j = _i;
//                    ws.Pictures.Add(Path.Combine(pathsource_temp, "stdnt.png"), PositioningMode.Move, new AnchorCell(ws.Columns[0], ws.Rows[j], 100000, 50000), new AnchorCell(ws.Columns[1], ws.Rows[j + 6], 500000, 200000));
//                    _i = _i + 7;
//                }
//            }
//            MydataSet.Tables[0].Columns.Remove("Id");
//            Mydatatable = AddRowPadding(Mydatatable, 6);
//            ws.InsertDataTable(Mydatatable, 3, 2, true);
//            string outpath = pathsource + "\\studentreport.xlsx";
//            ef.SaveXlsx(outpath);
//            System.IO.FileInfo file = new System.IO.FileInfo(outpath);
//            if (file.Exists)
//            {
//                Response.Clear();
//                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
//                Response.AddHeader("Content-Length", file.Length.ToString());
//                Response.ContentType = "application/octet-stream";
//                Response.WriteFile(file.FullName);
//                Response.End();
//            }

//        }
//    }
//protected void Grd_Student_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        if (e.Row.RowState == DataControlRowState.Alternate)
//        {
//            e.Row.Attributes.Add("onmouseover","this.style.backgroundColor='gray';this.style.cursor='hand'");
//            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
//        }
//        else
//        {
//            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
//            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
//        }
//        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Student, "Select$" + e.Row.RowIndex);
//    }

//}

//protected void Grd_Student_SelectedIndexChanged(object sender, EventArgs e)
//{
//    int i_SelectedStudId = int.Parse(Grd_Student.SelectedRow.Cells[1].Text.ToString());
//    int StudentStatus = int.Parse(Grd_Student.SelectedRow.Cells[6].Text.ToString());
//    if (StudentStatus == 1 || StudentStatus == 2 || StudentStatus == 3)
//    {
//        Session["StudId"] = i_SelectedStudId;
//        Session["StudType"] = StudentStatus;
//        Response.Redirect("StudentDetails.aspx");
//    }
//    else if (StudentStatus == 4)
//    {
//        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('SutdDetailsPupUp.aspx?StudId=" + i_SelectedStudId + "');", true);        
//    }
//    else
//    {
//        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegisteredStudentDetails.aspx?TempStudId=" + i_SelectedStudId + "');", true);

//    }
//}
#endregion