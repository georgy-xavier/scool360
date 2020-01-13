using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using WinBase;

namespace Scool360student.AppWebServices
{
    /// <summary>
    /// Summary description for StudentDataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class StudentDataService : System.Web.Services.WebService
    {
        #region initializers
        KnowinUser MyUser = null;
        KnowinGen _Prntobj = null;
        StudentManagerClass MyStudMang = null;
        SchoolClass objSchool = null;
        DataSet MydataSet = null;
        int studentID = 0, studentType = 0;
        void InitilizeObjects()
        {
            MyUser = (KnowinUser)Session[WinerConstants.SessionUser];
            if (MyUser != null)
            {
                objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
                _Prntobj = (KnowinGen)HttpContext.Current.Session[WinerConstants.SessionUser];
                MyStudMang = new StudentManagerClass(_Prntobj);

                studentID = MyUser.m_StudentId;
                studentType = Convert.ToInt32(HttpContext.Current.Session["StudType"]);
            }
            //if (WinerUtlity.NeedCentrelDB())
            //  {
            //      if (Session[WinerConstants.SessionSchool] != null)
            //      {
            //          objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            //      }

            //  }

        }
        #endregion

        #region Utilities/Supporters

        public string makeSubQueryForDynamicField()
        {
            string sql = ",";

            DataSet _CustomFields = MyStudMang.GetCuestomFields();
            if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
            {
                int index = 0;
                foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                {
                    string sqlNm = string.Join("", dr_fieldData[1].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    sql += dr_fieldData[0].ToString() + " AS '" + sqlNm + "'";
                    if (index != (_CustomFields.Tables[0].Rows.Count - 1))
                        sql += ",";
                    index++;
                }
                return sql;
            }
            return null;

        }
        //public DataSet modifyStudentClassDt(DataSet MyDataset)
        //{
        //    int ClassId = -1, PrevClassId = -1, rowId = 0;

        //    if (MyDataset != null && MyDataset.Tables[0] != null && MyDataset.Tables[0].Rows.Count > 0)
        //    {
        //        DataTable dt = MyDataset.Tables[0];
        //        dt.Columns.Add("Result");
        //        foreach (DataRow dro in MyDataset.Tables[0].Rows)
        //        {
        //            string result = "Passed";
        //            ClassId = int.Parse(dro["ClassId"].ToString());
        //            if (PrevClassId == ClassId)
        //            {
        //                int id = rowId - 1;
        //                MyDataset.Tables[0].Rows[id]["Result"] = "Failed";
        //            }
        //            PrevClassId = ClassId;
        //            dro["Result"] = result;
        //            rowId++;
        //        }
        //        MyDataset.Tables[0].Rows[rowId - 1]["Result"] = MyStudMang.getStatus(studentID);
        //    }
        //    return MyDataset;
        //}
        public string getStudentDynamicFields(string dbDynamicTable)
        {
            string subSql = "", Sql = "";
            DataSet _CustomFields = MyStudMang.GetCuestomFields();
            if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
            {
                int index = 0;
                foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                {
                    string sqlNm = string.Join("", dr_fieldData[1].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    subSql += dr_fieldData[0].ToString() + " AS " + sqlNm;
                    if (index != (_CustomFields.Tables[0].Rows.Count - 1))
                        subSql += ",";
                    index++;
                }
            }
            if (string.IsNullOrEmpty(subSql) == false)
            {
                Sql += " SELECT " + subSql + " FROM " + dbDynamicTable + " WHERE StudentId =" + studentID + ";";
                MydataSet = readFromDb(Sql);
                string JsonDt = DataTableToJSON(MydataSet.Tables[0]);
                if (JsonDt != null)
                    return JsonDt;
            }
            return null;
        }
        public string getStudentClassDt(string DbTable)
        {
            string sql = "";
            sql += " SELECT tblstandard.Name AS Standard,tblclass.ClassName AS Class,tblview_studentclassmap.RollNo AS `Roll Number`,tblbatch.BatchName AS Batch,tblview_studentclassmap.ClassId ";
            sql += " FROM  " + DbTable + " ";
            sql += " INNER JOIN tblview_studentclassmap ON tblview_studentclassmap.StudentId = " + DbTable + ".Id INNER JOIN tblbatch ON  tblbatch.Id =  tblview_studentclassmap.BatchId INNER JOIN tblclass on tblclass.Id =  tblview_studentclassmap.ClassId INNER JOIN tblstandard ON tblstandard.Id = tblview_studentclassmap.Standard ";
            sql += " WHERE " + DbTable + ".Id =" + studentID + "  ORDER BY  tblview_studentclassmap.BatchId DESC ";
            MydataSet = readFromDb(sql);
            DataSet modifiedDt = modifyStudClasDtlsWithResults(MydataSet);
            string datainJSON = DataTableToJSON(modifiedDt.Tables[0]);
            if (datainJSON != null)
                return datainJSON;
            else
                return null;
        }
        public DataSet modifyStudClasDtlsWithResults(DataSet MyDataset)
        {
            int ClassId = -1, PrevClassId = -1, rowId = 0;
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
                MyDataset.Tables[0].Rows[0]["Result"] = MyStudMang.getStatus(studentID);
            }
            return MyDataset;
        }
        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table, Formatting.Indented);
            if (JSONString != null)
                return JSONString;
            return null;
        }
        public string buildJSONFromDataSet(DataSet MydataSet)
        {
            string JsonDt = null;
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
        public DataSet readFromDb(string sql)
        {
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql); //Using ExecuteQueryReturnDataSet from MySqlClass
            //if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            if (MydataSet != null && MydataSet.Tables != null)
                return MydataSet;
            return null;
        }
        #endregion
        #region All student data

        [WebMethod(EnableSession = true)]
        public string[] getAllStudentDt(int limit, int offset, string studentType)
        {
            //will provide all info about all students based on student status & row limit.
            //limit:max row to fetch // offset:starting rown umber // studentStatus : status of student group.
            //Status// 0:disabled/deleted,1:livestudent,2:ApprovalList(Live),3:HistoryStudent,4:Temporary.
            InitilizeObjects();
            // #region Make sql query for this condition
            //will provide all info about all a single student with student id and student type.
            //Here student type means that whether the student is a live/history/temp student
            //studentType// 0:disabled/deleted, 1:livestudent, 2:ApprovalList(Live), 3:HistoryStudent, 4:Temporary , 5:PromotionList(Live).
            // InitilizeObjects();
            string Sql = "", dbMainTable = "", dbDynamicTable = ""; DataSet Dtcount = null;
            //Make Sql Query for this condition
            if (studentType == "LIVE")//LIVE
            {
                //Reading data FROM tblstudent
                dbMainTable = "tblstudent";
                dbDynamicTable = "tblstudentdetails";
                Dtcount = readFromDb("SELECT COUNT(*) FROM " + dbMainTable + ";");
                Sql += " SELECT student.Id AS Id,StudentName,AdmitionNo,tblclass.ClassName AS `Class`,student.RollNo,DATE_FORMAT(DOB,'%d-%M-%Y')AS DOB,Sex AS Gender,tblbloodgrp.GroupName AS `BloodGroup`,OfficePhNo AS `MobileNumber`,tblreligion.Religion,tblcast.castname AS Caste,tblcast_category.CategoryName AS `Category`,Location,Pin AS PIN,State,Nationality,student.StudentId AS `ReferenceID`,AadharNumber AS `Social ID`,GardianName AS `Name Of Gardian`,Address AS `Permanent Address`,FrstLng.`Language` AS `First Language`,MthrTng.`Language` AS `Mother Tongue`,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS `Date Of Join`,tblbatch.BatchName AS `Joined Batch`,DATE_FORMAT(DateOfLeaving,'%d-%M-%Y') AS `Date Of Leaving`,tblstudtype.TypeName AS `Student Type`,tbladmisiontype.Name AS `Admission Type`,DATE_FORMAT(CreationTime,'%d-%M-%Y : %r') AS `Created Time`,CreatedUserName AS `Created By`,student.`Comment`,IF(UseHostel=1,'YES','NO') AS `Use Hostel`,IF(UseBus=1,'YES','NO') AS `Use Transportation`,ResidencePhNo AS `Phone Number (Secondary)`,tblsmsparentlist.PhoneNo AS `Phone Number(Alternative)`,Email,Addresspresent AS `Present Address`,MothersName AS `Name Of Mother`,MotherEduQuali AS `Mother's Educational Qualification`,MotherOccupation AS `Mother's Occupation`,FatherEduQuali AS `Father's Educational Qualification`,FatherOccupation AS `Father's Occupation`,AnnualIncome AS `Annual Income`,NumberofBrothers AS `Number Of Brother's`,NumberOfSysters AS `Number Of Sister's`,CanceledUser AS `Cancelled User` ";

                if (makeSubQueryForDynamicField() != null)
                {
                    Sql += makeSubQueryForDynamicField();
                    Sql += " FROM " + dbMainTable + " student ";
                    Sql += " LEFT JOIN " + dbDynamicTable + " ON " + dbDynamicTable + ".StudentId=student.Id ";
                }
                else
                {
                    Sql += " FROM " + dbMainTable + " student ";
                }
                Sql += " LEFT JOIN tblsmsparentlist ON tblsmsparentlist.Id = student.Id INNER JOIN tbllanguage FrstLng ON FrstLng.Id=student.`1stLanguage` INNER JOIN tbllanguage MthrTng ON MthrTng.Id=student.`MotherTongue` INNER JOIN tblbloodgrp ON tblbloodgrp.Id=student.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=student.Religion INNER JOIN tblcast ON tblcast.Id =  student.`Cast` INNER JOIN tblcast_category ON tblcast_category.Id = tblcast.CategoryId INNER JOIN tblstudtype ON tblstudtype.Id = student.StudTypeId INNER JOIN tbladmisiontype ON tbladmisiontype.Id = student.AdmissionTypeId INNER JOIN tblclass ON tblclass.Id = student.LastClassId INNER JOIN tblbatch ON tblbatch.Id = student.JoinBatch inner join tblstudentclassmap on tblstudentclassmap.StudentId=student.Id where student.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + "  AND student.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + ")))";
                Sql += " LIMIT " + limit + " OFFSET " + offset + " ;";
            }
            else if (studentType == "HISTORY")//HISTORY
            {
                //Reading data FROM tblstudent_history
                dbMainTable = "tblstudent_history"; dbDynamicTable = "tblstudentdetails_history";
                Dtcount = readFromDb("SELECT COUNT(*) FROM " + dbMainTable + ";");
                Sql += " SELECT student.Id AS Id,StudentName,AdmitionNo,tblclass.ClassName AS `Class`,student.RollNo,DATE_FORMAT(DOB,'%d-%M-%Y')AS DOB,Sex AS Gender,tblbloodgrp.GroupName AS `BloodGroup`,OfficePhNo AS `MobileNumber`,tblreligion.Religion,tblcast.castname AS Caste,tblcast_category.CategoryName AS `Category`,Location,Pin AS PIN,State,Nationality,student.StudentId AS `ReferenceID`,AadharNumber AS `Social ID`,GardianName AS `Name Of Gardian`,Address AS `Permanent Address`,FrstLng.`Language` AS `First Language`,MthrTng.`Language` AS `Mother Tongue`,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS `Date Of Join`,tblbatch.BatchName AS `Joined Batch`,DATE_FORMAT(DateOfLeaving,'%d-%M-%Y') AS `Date Of Leaving`,tblstudtype.TypeName AS `Student Type`,tbladmisiontype.Name AS `Admission Type`,DATE_FORMAT(CreationTime,'%d-%M-%Y : %r') AS `Created Time`,CreatedUserName AS `Created By`,student.`Comment`,IF(UseHostel=1,'YES','NO') AS `Use Hostel`,IF(UseBus=1,'YES','NO') AS `Use Transportation`,ResidencePhNo AS `Phone Number (Secondary)`,tblsmsparentlist.PhoneNo AS `Phone Number(Alternative)`,Email,Addresspresent AS `Present Address`,MothersName AS `Name Of Mother`,MotherEduQuali AS `Mother's Educational Qualification`,MotherOccupation AS `Mother's Occupation`,FatherEduQuali AS `Father's Educational Qualification`,FatherOccupation AS `Father's Occupation`,AnnualIncome AS `Annual Income`,NumberofBrothers AS `Number Of Brother's`,NumberOfSysters AS `Number Of Sister's`,CanceledUser AS `Cancelled User` ";

                if (makeSubQueryForDynamicField() != null)
                {
                    Sql += makeSubQueryForDynamicField();
                    Sql += " FROM " + dbMainTable + " student ";
                    Sql += " LEFT JOIN " + dbDynamicTable + " ON " + dbDynamicTable + ".StudentId=student.Id ";
                }
                else
                {
                    Sql += " FROM " + dbMainTable + " student ";
                }
                Sql += " LEFT JOIN tblsmsparentlist ON tblsmsparentlist.Id = student.Id INNER JOIN tbllanguage FrstLng ON FrstLng.Id=student.`1stLanguage` INNER JOIN tbllanguage MthrTng ON MthrTng.Id=student.`MotherTongue` INNER JOIN tblbloodgrp ON tblbloodgrp.Id=student.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=student.Religion INNER JOIN tblcast ON tblcast.Id =  student.`Cast` INNER JOIN tblcast_category ON tblcast_category.Id = tblcast.CategoryId INNER JOIN tblstudtype ON tblstudtype.Id = student.StudTypeId INNER JOIN tbladmisiontype ON tbladmisiontype.Id = student.AdmissionTypeId INNER JOIN tblclass ON tblclass.Id = student.ClassId INNER JOIN tblbatch ON tblbatch.Id = student.JoinBatch ";
                Sql += " LIMIT " + limit + " OFFSET " + offset + " ;";
            }
            else if (studentType == "TEMPORARY")//TEMPORARY-registered students
            {
                //TODO:
                dbMainTable = "tblstudent_history";
                dbDynamicTable = "tbltempstudentdetails";
                Dtcount = readFromDb("SELECT COUNT(*) FROM tbltempstdent;");
                Sql = " SELECT tbltempstdent.Id,tbltempstdent.Name,TempId,tbl_admissionstatus.`Status`,Result,tblclass.ClassName,tblbatch.BatchName AS JoiningBatch,DATE_FORMAT(DOB,'%d/%m/%Y')AS DOB,Gender,PhoneNumber,PersionalInterview,DATE_FORMAT(CreatedDate,'%d/%m/%Y')AS CreatationTime,email,PreviousBoard,Remark,DateOfInterView,TeacherRemark,HMRemark,PrincipalRemark,Fathername AS GardianName,MotherName,tblbloodgrp.GroupName AS BloodGroup,tbllanguage.`Language`,Location,State,CreatedUserName AS CreatedBy,AadharNumber AS SocialID,Address,MotherName,MotherEduQualification,MotherOccupation,FatherEduQualification,FatherOccupation,AnnualIncome ";
                if (makeSubQueryForDynamicField() != null)
                {
                    Sql += makeSubQueryForDynamicField();
                    Sql += " FROM tbltempstdent ";
                    Sql += " LEFT JOIN tbltempstudentdetails ON tbltempstudentdetails.StudentId=tbltempstdent.Id ";
                }
                else
                {
                    Sql += " FROM tbltempstdent ";
                }
                Sql += " INNER JOIN tbl_admissionstatus ON tbl_admissionstatus.Id=tbltempstdent.AdmissionStatusId INNER JOIN tblbatch ON tblbatch.Id=tbltempstdent.JoiningBatch INNER JOIN tblbloodgrp ON tblbloodgrp.Id=tbltempstdent.BloodGroup INNER JOIN tbllanguage ON tbllanguage.Id=tbltempstdent.MotherTongue INNER JOIN tblclass ON tblclass.Id=tbltempstdent.Class where tbltempstdent.Status=1 ";
                Sql += " LIMIT " + limit + " OFFSET " + offset + ";";
            }
            else if (studentType == "PROMOTION")//PROMOTION-Promotion list students
            {

                dbMainTable = "tblstudent";
                dbDynamicTable = "tblstudentdetails";
                Dtcount = readFromDb("SELECT COUNT(*) FROM tbltempstdent;");
                Sql = " SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo as AdmissionNo,tblclass.ClassName,  DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName as GuardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,  tblstudent.Email, tblreligion.Religion, (select tblcast.castname from tblcast where tblcast.Id= tblstudent.Cast) as Caste, tblbloodgrp.GroupName as BloodGroup, tblstudent.FatherEduQuali as FathersQualification, tblstudent.FatherOccupation, round( tblstudent.AnnualIncome,2) as AnnualIncome, tblstudent.MothersName, tblstudent.MotherEduQuali as MothersQualification,tblstudent.MotherOccupation as MotherOccupation, tblstudent.Addresspresent, tblstudent.Nationality, tblstudent.NumberofBrothers, tblstudent.NumberOfSysters as NoOfSisters, (select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.MotherTongue) as MotherTongue,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tblstudent.1stLanguage) as FirstLanguage, (select tblstudtype.TypeName from tblstudtype where tblstudtype.Id= tblstudent.StudTypeId) as StudentType, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining,JoiningBatch.BatchName as 'Joining Batch', DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving , 3 as StudStatus ,'InPromotionList' as StudType ,tblstudentclassmap_history.RollNo as RollNo,tblstudent.StudentId,IF (tblstudent.UseBus=0 ,'No','Yes') as UseBus,IF (tblstudent.UseHostel=0 ,'No','Yes') as UseHostel,tblstudent.AadharNumber as AadharNumber ";
                if (makeSubQueryForDynamicField() != null)
                {
                    Sql += makeSubQueryForDynamicField();
                    Sql += " FROM " + dbMainTable;
                    Sql += " LEFT JOIN " + dbDynamicTable + " ON " + dbDynamicTable + ".StudentId=tblstudent.Id ";
                }
                else
                {
                    Sql += " FROM " + dbMainTable;
                }
                Sql += " inner join tblstudentclassmap_history on tblstudentclassmap_history.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap_history.ClassId  inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion INNER JOIN tblbatch JoiningBatch ON JoiningBatch.Id=tblstudent.JoinBatch where tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudent.Status=1 AND tblstudent.Id IN (SELECT tblstudentclassmap_history.StudentId FROM tblstudentclassmap_history WHERE tblstudentclassmap_history.BatchId=" + (MyUser.CurrentBatchId - 1) + " AND tblstudentclassmap_history.ClassId IN ( SELECT tblclass.Id from tblclass))AND tblstudentclassmap_history.StudentId not in(select tblstudentclassmap.StudentId from tblstudentclassmap) order by tblstudentclassmap_history.ClassId , tblstudent.StudentName";
                //Sql += " LIMIT " + limit + " OFFSET " + offset + ";";
            }
            //read from db with created query
            try
            {

                DataSet MydataSet = readFromDb(Sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    string[] JsonDt = new string[2];
                    JsonDt[0] = Dtcount.Tables[0].Rows[0][0].ToString();
                    JsonDt[1] = DataTableToJSON(MydataSet.Tables[0]);
                    return JsonDt;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            return null;









            //if ((studentType == 1) || (studentType == 2) || (studentType == 5))///LIVE//approvalList//waitingforpermission
            //{
            //    Dtcount = readFromDb("SELECT COUNT(*) FROM tblstudent WHERE tblstudent.`Status`=1;");
            //    _sql = "SELECT tblstudent.Id,tblstudent.Status,StudentName,tblclass.ClassName,tblstudentclassmap.RollNo,AdmitionNo,DATE_FORMAT(DOB,'%d/%m/%Y')AS DOB,Sex AS Gender,tblbloodgrp.GroupName AS BloodGroup,GardianName,ResidencePhNo,OfficePhNo,tbllanguage.`Language`,Location,State,tblreligion.Religion,tblcast.castname,DATE_FORMAT(DateofJoining,'%d/%m/%Y') AS JoinDate,tblbatch.BatchName AS JoinedBatch,DATE_FORMAT(CreationTime,'%d/%m/%Y')AS CreatationTime,tblstudent.StudentId,tblstudtype.TypeName AS StudentType,tbladmisiontype.Name AS AdmissionType,CreatedUserName AS CreatedBy,Comment,AadharNumber AS SocialID,DateOfLeaving AS LeavingDate,Address AS PermanentAddress,Addresspresent AS CommunicationAddress,MothersName,MotherEduQuali,MotherOccupation,FatherEduQuali,FatherOccupation,AnnualIncome,NumberofBrothers,NumberOfSysters";
            //    if (makeSubQueryForDynamicField() != null)
            //    {
            //        _sql += makeSubQueryForDynamicField();
            //        _sql += " FROM tblstudent ";
            //        _sql += " LEFT JOIN tblstudentdetails ON tblstudentdetails.StudentId=tblstudent.Id ";
            //    }
            //    else
            //    {
            //        _sql += " FROM tblstudent ";
            //    }
            //    _sql += " INNER JOIN tblbatch ON tblbatch.Id=tblstudent.JoinBatch INNER JOIN tbladmisiontype ON tbladmisiontype.Id=tblstudent.AdmissionTypeId INNER JOIN tblstudtype ON tblstudtype.Id=tblstudent.StudTypeId  INNER JOIN tblbloodgrp ON tblbloodgrp.Id=tblstudent.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=tblstudent.Religion INNER JOIN tblcast ON tblcast.Id=tblstudent.Cast INNER JOIN tbllanguage ON tbllanguage.Id=tblstudent.MotherTongue INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap.ClassId ";
            //    _sql += " WHERE tblstudent.`Status`=" + studentType + "  ORDER BY StudentName ASC LIMIT " + limit + " OFFSET " + offset + ";";
            //}
            //else if (studentType == 3)///HISTORY
            //{
            //    Dtcount = readFromDb("SELECT COUNT(*) FROM tblstudent_history");
            //    _sql = " SELECT tblstudent_history.Id,tblstudent_history.Status,StudentName,tblclass.ClassName AS LastClass,tblstudentclassmap_history.RollNo AS LastRollNo,AdmitionNo,DATE_FORMAT(DOB,'%d/%m/%Y')AS DOB,Sex AS Gender,DateOfLeaving AS LeavingDate,tblbloodgrp.GroupName AS BloodGroup,GardianName,ResidencePhNo,OfficePhNo,tbllanguage.`Language`,Location,State,tblreligion.Religion,tblcast.castname,DATE_FORMAT(DateofJoining,'%d/%m/%Y') AS JoinDate,tblbatch.BatchName AS JoinedBatch,DATE_FORMAT(CreationTime,'%d/%m/%Y')AS CreatationTime,tblstudent_history.StudentId,tblstudtype.TypeName AS StudentType,tbladmisiontype.Name AS AdmissionType,CreatedUserName AS CreatedBy,Comment,AadharNumber AS SocialID,Address AS PermanentAddress,Addresspresent AS CommunicationAddress,MothersName,MotherEduQuali,MotherOccupation,FatherEduQuali,FatherOccupation,AnnualIncome,NumberofBrothers,NumberOfSysters ";
            //    if (makeSubQueryForDynamicField() != null)
            //    {
            //        _sql += makeSubQueryForDynamicField();
            //        _sql += " FROM tblstudent_history  ";
            //        _sql += " LEFT JOIN tblstudentdetails_history ON tblstudentdetails_history.StudentId=tblstudent_history.Id ";
            //    }
            //    else
            //    {
            //        _sql += " FROM tblstudent_history  ";
            //    }
            //    _sql += " INNER JOIN tblbatch ON tblbatch.Id=tblstudent_history.JoinBatch INNER JOIN tbladmisiontype ON tbladmisiontype.Id=tblstudent_history.AdmissionTypeId INNER JOIN tblstudtype ON tblstudtype.Id=tblstudent_history.StudTypeId INNER JOIN tblbloodgrp ON tblbloodgrp.Id=tblstudent_history.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=tblstudent_history.Religion INNER JOIN tblcast ON tblcast.Id=tblstudent_history.Cast INNER JOIN tbllanguage ON tbllanguage.Id=tblstudent_history.MotherTongue INNER JOIN tblstudentclassmap_history ON tblstudentclassmap_history.StudentId=tblstudent_history.Id INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap_history.ClassId ";
            //    _sql += " ORDER BY StudentName ASC LIMIT " + limit + " OFFSET " + offset + ";";
            //}
            //else if (studentType == 4)//TEMPORARY-registered students
            //{
            //    Dtcount = readFromDb("SELECT COUNT(*) FROM tbltempstdent;");
            //    _sql = " SELECT tbltempstdent.Id,tbltempstdent.Status.tbltempstdent.Name,TempId,tbl_admissionstatus.`Status`,Result,tblclass.ClassName,tblbatch.BatchName AS JoiningBatch,DATE_FORMAT(DOB,'%d/%m/%Y')AS DOB,Gender,PhoneNumber,PersionalInterview,DATE_FORMAT(CreatedDate,'%d/%m/%Y')AS CreatationTime,email,PreviousBoard,Remark,DateOfInterView,TeacherRemark,HMRemark,PrincipalRemark,Fathername AS GardianName,MotherName,tblbloodgrp.GroupName AS BloodGroup,tbllanguage.`Language`,Location,State,CreatedUserName AS CreatedBy,AadharNumber AS SocialID,Address,MotherName,MotherEduQualification,MotherOccupation,FatherEduQualification,FatherOccupation,AnnualIncome ";
            //    if (makeSubQueryForDynamicField() != null)
            //    {
            //        _sql += makeSubQueryForDynamicField();
            //        _sql += " FROM tbltempstdent ";
            //        _sql += " LEFT JOIN tbltempstudentdetails ON tbltempstudentdetails.StudentId=tbltempstdent.Id ";
            //    }
            //    else
            //    {
            //        _sql += " FROM tbltempstdent ";
            //    }
            //    _sql += " INNER JOIN tbl_admissionstatus ON tbl_admissionstatus.Id=tbltempstdent.AdmissionStatusId INNER JOIN tblbatch ON tblbatch.Id=tbltempstdent.JoiningBatch INNER JOIN tblbloodgrp ON tblbloodgrp.Id=tbltempstdent.BloodGroup INNER JOIN tbllanguage ON tbllanguage.Id=tbltempstdent.MotherTongue INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tbltempstdent.Id INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap.ClassId ";
            //    _sql += " ORDER BY tbltempstdent.Name ASC LIMIT " + limit + " OFFSET " + offset + ";";
            //}
            //#endregion

            //DataSet MydataSet = readFromDb(_sql);

            //if (MydataSet.Tables[0].Rows.Count > 0)
            //{
            //    string[] JsonDt = new string[2];
            //    string myDataList = DataTableToJSON(MydataSet.Tables[0]);
            //    JsonDt[0] = Dtcount.Tables[0].Rows[0][0].ToString();
            //    JsonDt[1] = myDataList;
            //    return JsonDt;
            //}
            //return null;
        }

        #endregion
        #region single student data

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string getSingleStudenIncident()
        {
            InitilizeObjects();
            Incident inc = new Incident(_Prntobj);
            string inciDt = "";
            if (MyUser.HaveActionRignt(70))
                inciDt = inc.LoadIncidenceData(studentID, "student", MyUser.CurrentBatchId);
            else
                inciDt = "Sorry, for seccurity & privacy you have no rights for view this.Please contact admin";
            return inciDt;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string[] getSingleStudentDt()
        {
            //will provide all info about all a single student with student id and student type.
            //Here student type means that whether the student is a live/history/temp student
            //studentType// 0:disabled/deleted, 1:livestudent, 2:ApprovalList(Live), 3:HistoryStudent, 4:Temporary , 5:PromotionList(Live).
            InitilizeObjects();
            string Sql = "", dbMainTable = "", dbDynamicTable = "";
            //Make Sql Query for this condition
            if ((studentType == 1) || (studentType == 3) || (studentType == 4))//LIVE
            {
                //Reading data FROM tblstudent
                dbMainTable = "tblstudent"; dbDynamicTable = "tblstudentdetails";
                Sql += " SELECT student.Id AS Id,StudentName AS Name,AdmitionNo AS `Admission Number`,student.StudentId AS `Reference ID`,DATE_FORMAT(DOB,'%d-%M-%Y')AS DOB,Sex AS Gender,tblbloodgrp.GroupName AS `Blood Group`,AadharNumber AS `Social ID`,GardianName AS `Name Of Gardian`,Address AS `Permanent Address`,OfficePhNo AS `Phone Number(Mobile)`,tblreligion.Religion,tblcast.castname AS Caste,tblcast_category.CategoryName AS `Caste Category`,FrstLng.`Language` AS `First Language`,MthrTng.`Language` AS `Mother Tongue`,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS `Date Of Join`,tblbatch.BatchName AS `Joined Batch`,DATE_FORMAT(DateOfLeaving,'%d-%M-%Y') AS `Date Of Leaving`,tblstudtype.TypeName AS `Student Type`,tbladmisiontype.Name AS `Admission Type`,DATE_FORMAT(CreationTime,'%d-%M-%Y : %r') AS `Created Time`,CreatedUserName AS `Created By`,student.`Comment`,IF(UseHostel=1,'YES','NO') AS `Use Hostel`,IF(UseBus=1,'YES','NO') AS `Use Transportation`,ResidencePhNo AS `Phone Number (Secondary)`,tblsmsparentlist.PhoneNo AS `Phone Number(Alternative)`,Email,Addresspresent AS `Present Address`,Location,Pin AS PIN,State,Nationality,MothersName AS `Name Of Mother`,MotherEduQuali AS `Mother's Educational Qualification`,MotherOccupation AS `Mother's Occupation`,FatherEduQuali AS `Father's Educational Qualification`,FatherOccupation AS `Father's Occupation`,AnnualIncome AS `Annual Income`,NumberofBrothers AS `Number Of Brother's`,NumberOfSysters AS `Number Of Sister's`,CanceledUser AS `Cancelled User`,student.`Status`,TempStudentId AS `Temporary Student ID` ";
                Sql += " FROM " + dbMainTable + " student ";
                Sql += " LEFT JOIN tblsmsparentlist ON tblsmsparentlist.Id = student.Id INNER JOIN tbllanguage FrstLng ON FrstLng.Id=student.`1stLanguage` INNER JOIN tbllanguage MthrTng ON MthrTng.Id=student.`MotherTongue` INNER JOIN tblbloodgrp ON tblbloodgrp.Id=student.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=student.Religion INNER JOIN tblcast ON tblcast.Id =  student.`Cast` INNER JOIN tblcast_category ON tblcast_category.Id = tblcast.CategoryId INNER JOIN tblstudtype ON tblstudtype.Id = student.StudTypeId INNER JOIN tbladmisiontype ON tbladmisiontype.Id = student.AdmissionTypeId INNER JOIN tblbatch ON tblbatch.Id = student.JoinBatch ";
                Sql += " WHERE student.Id = " + studentID + ";";
            }
            else if (studentType == 2)//HISTORY
            {
                //Reading data FROM tblstudent_history
                dbMainTable = "tblstudent_history"; dbDynamicTable = "tblstudentdetails_history";
                Sql += " SELECT student.Id AS Id,StudentName AS Name,AdmitionNo AS `Admission Number`,student.StudentId AS `Reference ID`,DATE_FORMAT(DOB,'%d-%M-%Y')AS DOB,Sex AS Gender,tblbloodgrp.GroupName AS `Blood Group`,tblclass.ClassName AS `Last Class`,tblclass.ClassName AS `Last Standard`,student.RollNo AS `Roll Number`,AadharNumber AS `Social ID`,GardianName AS `Name Of Gardian`,Address AS `Permanent Address`,OfficePhNo AS `Phone Number(Mobile)`,tblreligion.Religion,tblcast.castname AS Caste,tblcast_category.CategoryName AS `Caste Category`,FrstLng.`Language` AS `First Language`,MthrTng.`Language` AS `Mother Tongue`,DATE_FORMAT(DateofJoining,'%d-%M-%Y') AS `Date Of Join`,tblstandard.Name AS `Joined Standard`,tblbatch.BatchName AS `Joined Batch`,DATE_FORMAT(DateOfLeaving,'%d-%M-%Y') AS `Date Of Leaving`,tblstudtype.TypeName AS `Student Type`,tbladmisiontype.Name AS `Admission Type`,DATE_FORMAT(CreationTime,'%d-%M-%Y : %r') AS `Created Time`,CreatedUserName AS `Created By`,student.`Comment`,student.`Status`,IF(UseHostel=1,'YES','NO') AS `Use Hostel`,IF(UseBus=1,'YES','NO') AS `Use Transportation`,ResidencePhNo AS `Phone Number (Secondary)`,tblsmsparentlist.PhoneNo AS `Phone Number(Alternative)`,Email,Addresspresent AS `Present Address`,Location,Pin AS PIN,State,Nationality,MothersName AS `Name Of Mother`,MotherEduQuali AS `Mother's Educational Qualification`,MotherOccupation AS `Mother's Occupation`,FatherEduQuali AS `Father's Educational Qualification`,FatherOccupation AS `Father's Occupation`,AnnualIncome AS `Annual Income`,NumberofBrothers AS `Number Of Brother's`,NumberOfSysters AS `Number Of Sister's`,CanceledUser AS `Cancelled User`,TempStudentId AS `Temporary Student ID` ";
                Sql += " FROM " + dbMainTable + " student ";
                Sql += " LEFT JOIN tblsmsparentlist ON tblsmsparentlist.Id = student.Id INNER JOIN tbllanguage FrstLng ON FrstLng.Id=student.`1stLanguage` INNER JOIN tbllanguage MthrTng ON MthrTng.Id=student.`MotherTongue` INNER JOIN tblbloodgrp ON tblbloodgrp.Id=student.BloodGroup INNER JOIN tblreligion ON tblreligion.Id=student.Religion INNER JOIN tblcast ON tblcast.Id =  student.`Cast` INNER JOIN tblcast_category ON tblcast_category.Id = tblcast.CategoryId INNER JOIN tblstudtype ON tblstudtype.Id = student.StudTypeId INNER JOIN tbladmisiontype ON tbladmisiontype.Id = student.AdmissionTypeId INNER JOIN tblbatch ON tblbatch.Id = student.JoinBatch INNER JOIN tblstandard ON tblstandard.Id =student.JoinStandard INNER JOIN tblclass ON tblclass.Id = student.ClassId  ";
                Sql += " WHERE student.Id = " + studentID + ";";
            }
            else if (studentType == 5)//TEMPORARY-registered students
            {
                //TODO:
                dbMainTable = "tblstudent_history";
                dbDynamicTable = "tbltempstudentdetails";
            }
            //read from db with created query
            try
            {
                DataSet MydataSet = readFromDb(Sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    string[] JsonDt = new string[5];
                    string dataInJSON = DataTableToJSON(MydataSet.Tables[0]);
                    JsonDt[0] = studentID.ToString();//Student ID
                    JsonDt[1] = studentType.ToString();//StudentType
                    JsonDt[2] = dataInJSON;//Student Datas
                    JsonDt[3] = getStudentClassDt(dbMainTable);//Student Class Data
                    JsonDt[4] = getStudentDynamicFields(dbDynamicTable);//Dynamic fields
                    return JsonDt;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            return null;
        }

        #endregion
    }
}
