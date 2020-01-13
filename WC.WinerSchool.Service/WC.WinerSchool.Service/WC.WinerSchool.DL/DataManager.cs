using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WC.WinerSchool.DL;
using WC.WinerSchool.BOL;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Data.Objects.SqlClient;
using System.Runtime.Serialization.Json;
namespace WC.WinerSchool.DL
{
    public class DataManager
    {

        private int School_Id = -1;
        private string FilePath = "";
        private string strETConnection = string.Empty;
        private string strADConnection = string.Empty;
        private string strSchoolConnectionString = string.Empty;

        public string EntityConnection
        {
            get
            {
                if (School_Id <= 0)
                    return null;
                else
                {
                    if (string.IsNullOrEmpty(strETConnection))
                    {
                        using (centraldbEntities cdb = new centraldbEntities())
                        {
                            tblschool_list objtblschool_list = cdb.tblschool_list.Where(s => s.Id == School_Id).FirstOrDefault();
                            if (objtblschool_list != null)
                            {
                                EntityConnectionStringBuilder ecb = new EntityConnectionStringBuilder();
                                ecb.Metadata = "res://*/WinerModel.csdl|res://*/WinerModel.ssdl|res://*/WinerModel.msl";
                                ecb.Provider = "MySql.Data.MySqlClient";
                                ecb.ProviderConnectionString = "Server=" + objtblschool_list.HostName + ";Database=" + objtblschool_list.DatabaseName + ";Uid=" + objtblschool_list.UserName + ";Pwd=" + objtblschool_list.Password + ";";
                                FilePath = objtblschool_list.FilePath;

                                strETConnection = ecb.ConnectionString;
                            }
                        }
                    }

                    return strETConnection;
                }
            }
        }

        public string ADOConnection
        {
            get
            {
                if (School_Id <= 0)
                    return null;
                else
                {
                    if (string.IsNullOrEmpty(strADConnection))
                    {
                        using (centraldbEntities cdb = new centraldbEntities())
                        {
                            tblschool_list objtblschool_list = cdb.tblschool_list.Where(s => s.Id == School_Id).FirstOrDefault();
                            if (objtblschool_list != null)
                            {
                                EntityConnectionStringBuilder ecb = new EntityConnectionStringBuilder();

                                ecb.Metadata = "res://*/WinerModel.csdl|res://*/WinerModel.ssdl|res://*/WinerModel.msl";
                                ecb.Provider = "MySql.Data.MySqlClient";
                                ecb.ProviderConnectionString = "server=" + objtblschool_list.HostName + ";User Id=" + objtblschool_list.UserName + ";password=" + objtblschool_list.Password + ";Persist Security Info=True;database=" + objtblschool_list.DatabaseName;

                                strADConnection = "Server=" + objtblschool_list.HostName + ";Database=" + objtblschool_list.DatabaseName + ";Uid=" + objtblschool_list.UserName + ";Pwd=" + objtblschool_list.Password + ";";
                            }
                        }
                    }

                    return strADConnection;
                }
            }
        }

        public string SchoolConnectionString
        {
            get
            {
                if (School_Id <= 0)
                    return null;
                else
                {
                    if (string.IsNullOrEmpty(strSchoolConnectionString))
                    {
                        using (centraldbEntities cdb = new centraldbEntities())
                        {
                            tblschool_list objtblschool_list = cdb.tblschool_list.Where(s => s.Id == School_Id).FirstOrDefault();
                            if (objtblschool_list != null)
                            {
                                strSchoolConnectionString = objtblschool_list.ConnectionString;
                            }
                        }
                    }

                    return strSchoolConnectionString;
                }
            }
        }

        public DataManager(int School_Id)
        {
            // TODO: Complete member initialization
            this.School_Id = School_Id;
        }

        public string SerializeJSON(object ObjRegisterdevice, Type _type)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(_type);
            MemoryStream stream1 = new MemoryStream();
            serializer.WriteObject(stream1, ObjRegisterdevice);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }

        public object DeserializeJSON(string str_JSON, Type _type)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(_type);
            MemoryStream stream1 = new MemoryStream();
            stream1 = new MemoryStream(Encoding.UTF8.GetBytes(str_JSON));
            stream1.Position = 0;
            return serializer.ReadObject(stream1);
        }

        public int Save_Device(RegisterationDetails objRegisterationDetails)
        {
            int intDevoceId = -1;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                tbldevice objtbldevice = null;
                objtbldevice = db.tbldevices.Where(d => d.Id == objRegisterationDetails.DeviceId).FirstOrDefault();
                if (objtbldevice == null)
                    objtbldevice = db.tbldevices.Where(d => d.DeviceUniqueId == objRegisterationDetails.DeviceUniqueId).FirstOrDefault();
                if (objtbldevice == null)
                {
                    objtbldevice = new tbldevice();
                    objtbldevice.DeviceUniqueId = objRegisterationDetails.DeviceUniqueId;
                    objtbldevice.DeviceName = objRegisterationDetails.DeviceName;
                    objtbldevice.AddedUser = objRegisterationDetails.AddedUser;
                    objtbldevice.DeviceType = objRegisterationDetails.DeviceType;
                    objtbldevice.Registration_Date = DateTime.UtcNow;
                    objRegisterationDetails.CreatedDate = DateTime.UtcNow;
                    objtbldevice.IsActive = false;
                    db.tbldevices.AddObject(objtbldevice);
                }
                else
                {
                    objtbldevice.DeviceUniqueId = objRegisterationDetails.DeviceUniqueId;
                    objtbldevice.DeviceName = objRegisterationDetails.DeviceName;
                    objtbldevice.AddedUser = objRegisterationDetails.AddedUser;
                    objtbldevice.DeviceType = objRegisterationDetails.DeviceType;
                    objtbldevice.IsActive = objRegisterationDetails.IsActive;
                    objtbldevice.Registration_Date = objRegisterationDetails.CreatedDate;
                }
                db.SaveChanges();
                intDevoceId = objtbldevice.Id;
            }
            return intDevoceId;
        }

        public RegisterationDetails ReadDetails(string _MAC_Address)
        {
            RegisterationDetails objDeviceDetails = new RegisterationDetails();
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                tbldevice objtbldevice = null;
                objtbldevice = db.tbldevices.Where(d => d.DeviceUniqueId == _MAC_Address).FirstOrDefault();
                if (objtbldevice != null)
                {
                    if (objtbldevice.Id != 0)
                        objDeviceDetails.DeviceId = objtbldevice.Id;
                    objDeviceDetails.DeviceName = objtbldevice.DeviceName;
                    objDeviceDetails.AddedUser = objtbldevice.AddedUser;
                    objDeviceDetails.DeviceType = objtbldevice.DeviceType;
                    objDeviceDetails.CreatedDate = Convert.ToDateTime(objtbldevice.Registration_Date);
                    objDeviceDetails.DeviceUniqueId = objtbldevice.DeviceUniqueId;
                    objDeviceDetails.IsActive = objtbldevice.IsActive;
                    objDeviceDetails.SchoolDetails = GetSchoolDetails();
                }
            }
            return objDeviceDetails;
        }

        public School_Details GetSchoolDetails()
        {
            School_Details objSchoolDetails = new School_Details();
            tblschooldetail objtblschool = new tblschooldetail();
            tblfileurl mm = new tblfileurl();
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objtblschool = db.tblschooldetails.Where(p => p.Id.Equals(1)).FirstOrDefault();
            }
            if (objtblschool != null)
            {
                objSchoolDetails.SchoolID = School_Id;
                objSchoolDetails.SchoolName = objtblschool.SchoolName;
                objSchoolDetails.Address = objtblschool.Address;
                objSchoolDetails.Syllabus = objtblschool.Syllabus;
                objSchoolDetails.Mediumofinstruction = objtblschool.MediumofInstruction;
                objSchoolDetails.Disc = objtblschool.Disc;
                if (objtblschool.Logo != null)
                    objSchoolDetails.SchoolLogo = objtblschool.Logo;
                if (objtblschool.SchoolImages != null)
                    objSchoolDetails.SchoolImage = objtblschool.SchoolImages;
                objSchoolDetails.Configuration = GetConfiguration();
                objSchoolDetails.MaxBillCount = GetMaxBillCount();
            }
            return objSchoolDetails;
        }

        private long GetMaxBillCount()
        {
            long Id = 0;
            string sql = "SELECT MAX(tblfeebill.Counter) FROM tblfeebill  WHERE tblfeebill.AccYear= (SELECT tblbatch.Id FROM tblbatch WHERE tblbatch.Status=1)";

            using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
            {
                objConnection.Open();
                using (MySqlCommand objCommand = new MySqlCommand())
                {
                    objCommand.Connection = objConnection;
                    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.CommandText = sql;
                    using (MySqlDataReader objReader = objCommand.ExecuteReader())
                    {
                        if (objReader.HasRows)
                            if (objReader.Read())
                            {
                                long.TryParse(objReader.GetValue(0).ToString(), out Id);
                            }
                    }
                }
            }
            return Id;
        }

        public ConfigurationDetails GetConfiguration()
        {
            ConfigurationDetails objConfigDetails = new ConfigurationDetails();
            tblconfiguration objtblconfig = new tblconfiguration();
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objtblconfig = db.tblconfigurations.Where(t => t.Id.Equals(15)).FirstOrDefault();
                if (objtblconfig != null)
                {
                    int ID = Convert.ToInt32(objtblconfig.Value);
                    var query = (from md in db.tblattendancemodes
                                 where md.Id == ID
                                 select new { md.Attmode }).FirstOrDefault();

                    if (query != null)
                    {
                        objConfigDetails.Id = (int)objtblconfig.Id;
                        objConfigDetails.ConfigName = objtblconfig.Name;
                        objConfigDetails.Value = query.Attmode;
                        objConfigDetails.Discription = objtblconfig.Disc;
                    }
                }
            }
            return objConfigDetails;
        }       

        public tbldevice CheckDevice(int DeviceId)
        {
            tbldevice objdevice = null;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objdevice = db.tbldevices.Where(p => p.Id.Equals(DeviceId)).FirstOrDefault();
            }
            return objdevice;
        }

        public tbluser GetUser(string strUserName)
        {
            tbluser objUser = null;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objUser = db.tblusers.Where(p => p.UserName.Equals(strUserName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            }
            return objUser;
        }

        public tblrole GetRole(int intRoleId)
        {
            tblrole objRole = null;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objRole = db.tblroles.Where(p => p.Id.Equals(intRoleId)).FirstOrDefault();
            }
            return objRole;
        }

        public List<ModuleAction> GetActions(int intRoleId, int intModuleId)
        {
            List<ModuleAction> _objLst = new List<ModuleAction>();

            using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
            {
                objConnection.Open();
                using (MySqlCommand objCommand = new MySqlCommand())
                {
                    objCommand.Connection = objConnection;
                    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.CommandText = "select tblroleactionmap.ActionId from tblroleactionmap where tblroleactionmap.ModuleId=" + intModuleId + " AND tblroleactionmap.RoleId=" + intRoleId;
                    using (MySqlDataReader objReader = objCommand.ExecuteReader())
                    {
                        if (objReader.HasRows)
                        {
                            while (objReader.Read())
                            {
                                _objLst.Add(new ModuleAction() { ActionId = int.Parse(objReader.GetValue(0).ToString()) });

                            }
                        }
                    }

                }
            }

            return _objLst;


        }

        public StudentDetails[] GetStudentList(StudentSyncInput objStudentSyncInput, out int count)
        {
            StudentDetails[] lstStud = null;
            count = 0;
            long syncdate = objStudentSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from st in db.tblstudents
                                  orderby st.SyncDate ascending
                                  select st);
                count = queryCount.Count();

                var query = (from st in db.tblstudents
                             join aty in db.tbladmisiontypes on (int)st.AdmissionTypeId equals aty.Id into joingrup_admtyp
                             from aty in joingrup_admtyp.DefaultIfEmpty()
                             join cst in db.tblcasts on st.Cast equals cst.Id into joingrup_cst
                             from cst in joingrup_cst.DefaultIfEmpty()
                             join rel in db.tblreligions on st.Religion equals rel.Id into joingrup_rlgn
                             from rel in joingrup_rlgn.DefaultIfEmpty()
                             join bld in db.tblbloodgrps on st.BloodGroup equals bld.Id into joingrup_bldgrp
                             from bld in joingrup_bldgrp.DefaultIfEmpty()
                             join mthrtng in db.tbllanguages on st.MotherTongue equals mthrtng.Id into joingrup_lngug
                             from mthrtng in joingrup_lngug.DefaultIfEmpty()
                             join Flang in db.tbllanguages on st.C1stLanguage equals Flang.Id into joingrup_1lngug
                             from Flang in joingrup_1lngug.DefaultIfEmpty()
                             join batch in db.tblbatches on st.JoinBatch equals batch.Id into joingrup_batch
                             from batch in joingrup_batch.DefaultIfEmpty()
                             join stdtyp in db.tblstudtypes on st.StudTypeId equals stdtyp.Id into joingrup_sttyp
                             from stdtyp in joingrup_sttyp.DefaultIfEmpty()
                             join cls in db.tblstudentclassmaps on st.Id equals cls.StudentId into joingrup_map
                             from cls in joingrup_map.DefaultIfEmpty()
                             where st.SyncDate >= syncdate
                             orderby st.SyncDate ascending
                             select new { st.Id, st.StudentName, st.AdmitionNo, st.DOB, st.Address, st.Sex, st.DateofJoining, MotherTongue = mthrtng.Language, JoinBatch = batch.BatchName, StudentType = stdtyp.TypeName, st.Status, st.Nationality, st.RollNo, st.SyncDate, AdmissionType = aty.Name, BloodGroup = bld.GroupName, Caste = cst.castname, Religion = rel.Religion, FirstLanguage = Flang.Language, ClassID = cls.ClassId }).ToList();//BloodGroup = bld.GroupName,
                if (query.Count() == 0)
                    return lstStud;

                List<StudentDetails> objLtStudentDetails = new List<StudentDetails>();
                int t_i = 0;
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        StudentDetails objStud = new StudentDetails();
                        objStud.Id = obj.Id;
                        objStud.StudentName = obj.StudentName;
                        objStud.AdmissionNo = obj.AdmitionNo;
                        objStud.Address = obj.Address;
                        objStud.SEX = obj.Sex;
                        objStud.MotherTongue = obj.MotherTongue;
                        objStud.JoinBatch = obj.JoinBatch;
                        objStud.StudentType = obj.StudentType;
                        objStud.Nationality = obj.Nationality;
                        objStud.AdmissionType = obj.AdmissionType;
                        objStud.BloodGroup = obj.BloodGroup;
                        //if (obj.Id != null)
                        //objStud.image = GetImage("StudentImage", obj.Id);// ARUN ADDED 
                        objStud.Caste = obj.Caste;
                        objStud.RollNo = (int)obj.RollNo;
                        objStud.Religion = obj.Religion;
                        objStud.FirstLanguage = obj.FirstLanguage;
                        if (obj.DOB != null)
                            objStud.DOB = FormatUtility.GetDateString((DateTime)obj.DOB);
                        if (obj.DateofJoining != null)
                            objStud.DOJ = FormatUtility.GetDateString((DateTime)obj.DateofJoining);
                        if (obj.SyncDate != null)
                            objStud.SyncDate = (long)obj.SyncDate;
                        if (obj.Status != null)
                            objStud.Status = (int)obj.Status;
                        if (obj.ClassID != null)
                            objStud.ClassId = (int)obj.ClassID;

                        objLtStudentDetails.Add(objStud);
                        t_i++;
                    }
                }

                if (objLtStudentDetails.Count() == 0)
                    return lstStud;

                lstStud = new StudentDetails[objLtStudentDetails.Count];
                int i = 0;
                foreach (StudentDetails obj in objLtStudentDetails)
                {
                    lstStud[i] = obj;
                    i++;
                }
            }
            return lstStud;
        }

        public byte[] GetImage(string FileType, int UserId)
        {
            byte[] Image = new byte[0];
            try
            {
                using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
                {
                    tblfileurl objtbl = db.tblfileurls.Where(K => K.UserId == UserId && K.Type == FileType).FirstOrDefault();
                    if (objtbl != null)
                        Image = objtbl.FileBytes;
                }
            }
            catch (Exception)
            {
                Image = null;
            }
            return Image;
        }

        public ClassDetails[] GetClassList(ClassSyncInput objSyncInput, out int count)
        {
            ClassDetails[] lstClass = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from st in db.tblstudents
                                  orderby st.SyncDate ascending
                                  select st);
                count = queryCount.Count();

                var query = (from cls in db.tblclasses
                             join std in db.tblstandards on cls.Standard equals std.Id
                             join thr in db.tblusers on (int)cls.ClassTeacher equals thr.Id into joingrup
                             from thr in joingrup.DefaultIfEmpty()
                             where cls.SyncDate >= syncdate && cls.Status == 1
                             orderby cls.SyncDate ascending
                             select new { cls.Id, cls.ClassName, StandarId = cls.Standard, Standard = std.Name, cls.Division, cls.TotalSeats, cls.PeriodCount, ClassTeacher = thr.SurName, cls.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstClass;

                List<ClassDetails> objLtClassDetails = new List<ClassDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        ClassDetails objcls = new ClassDetails();
                        objcls.ClassId = obj.Id;
                        objcls.ClassName = obj.ClassName;
                        objcls.Standard = obj.Standard;
                        objcls.Division = obj.Division;
                        objcls.ClassTeacher = obj.ClassTeacher;
                        if (obj.StandarId != null)
                            objcls.StandardId = (int)obj.StandarId;
                        if (obj.TotalSeats != null)
                            objcls.TotalSeats = (int)obj.TotalSeats;
                        if (obj.PeriodCount != null)
                            objcls.PeriodCount = (int)obj.PeriodCount;
                        if (obj.SyncDate != null)
                            objcls.SyncDate = (long)obj.SyncDate;

                        objLtClassDetails.Add(objcls);
                    }
                }

                if (objLtClassDetails.Count() == 0)
                    return lstClass;

                lstClass = new ClassDetails[objLtClassDetails.Count];
                int i = 0;
                foreach (ClassDetails obj in objLtClassDetails)
                {
                    lstClass[i] = obj;
                    i++;
                }
            }
            return lstClass;
        }

        public UserDetails[] GetUserList(UserSyncInput objSyncInput, out int count)
        {
            UserDetails[] lstUser = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from usr in db.tblusers
                                  orderby usr.SyncDate ascending
                                  select usr);
                count = queryCount.Count();

                var query = (from users in db.tblusers
                             join roles in db.tblroles on (int)users.RoleId equals roles.Id
                             where users.SyncDate >= syncdate && users.CanLogin == 1
                             orderby users.SyncDate ascending
                             select new { users.Id, users.UserName, users.Password, users.EmailId, users.SurName, users.RoleId, roles.RoleName, roles.Type, users.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstUser;

                List<UserDetails> objLtUserDetails = new List<UserDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        UserDetails objuser = new UserDetails();
                        objuser.Id = (int)obj.Id;
                        objuser.UserName = obj.UserName;
                        objuser.Password = obj.Password;
                        objuser.DispName = obj.SurName;
                        objuser.EmailId = obj.EmailId;
                        objuser.RoleId = (int)obj.RoleId;
                        objuser.RoleName = obj.RoleName;
                        objuser.RoleType = obj.Type;
                        objuser.SyncDate = (long)obj.SyncDate;
                        objLtUserDetails.Add(objuser);
                    }
                }

                if (objLtUserDetails.Count() == 0)
                    return lstUser;

                lstUser = new UserDetails[objLtUserDetails.Count];
                int i = 0;
                foreach (UserDetails obj in objLtUserDetails)
                {
                    lstUser[i] = obj;
                    i++;
                }
            }
            return lstUser;
        }

        public int[] GetDeletedDataList(long syncdate, string t_tablename)
        {
            int[] lstData = null;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var query = (from tb in db.tbldeleteditems
                             where tb.SyncDate >= syncdate && tb.TableName == t_tablename
                             orderby tb.SyncDate ascending
                             select new { tb.Id, tb.ItemId, tb.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstData;

                lstData = new int[query.Count()];
                int j = 0;
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        if (obj.ItemId != null)
                            lstData[j] = (int)obj.ItemId;
                    }
                    j++;
                }
            }
            return lstData;
        }

        public BatchDetails[] GetBatchList(BatchSyncInput objSyncInput, out int count)
        {
            BatchDetails[] lstBatch = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from bth in db.tblbatches
                             where bth.SyncDate >= syncdate
                             orderby bth.SyncDate ascending
                             select new { bth.Id, bth.BatchName, bth.StartDate, bth.EndDate, bth.LastbatchId, bth.NOfWorkingDays, bth.Status, bth.Created, bth.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstBatch;

                List<BatchDetails> objLtBatchDetails = new List<BatchDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        BatchDetails objbth = new BatchDetails();
                        objbth.Id = obj.Id;
                        objbth.BatchName = obj.BatchName;
                        if (obj.StartDate != null)
                            objbth.StartDate = (DateTime)obj.StartDate;
                        if (obj.EndDate != null)
                            objbth.EndDate = (DateTime)obj.EndDate;
                        if (obj.StartDate != null)
                            objbth.int_StartDate = FormatUtility.GetDateInt((DateTime)obj.StartDate);
                        if (obj.EndDate != null)
                            objbth.int_EndDate = FormatUtility.GetDateInt((DateTime)obj.EndDate);
                        if (obj.LastbatchId != null)
                            objbth.LastbatchId = (int)obj.LastbatchId;
                        if (obj.NOfWorkingDays != null)
                            objbth.NOfWorkingDays = (int)obj.NOfWorkingDays;
                        objbth.Status = (int)obj.Status;
                        objbth.IsCreated = (int)obj.Created;

                        if (obj.SyncDate != null)
                            objbth.SyncDate = (long)obj.SyncDate;

                        objLtBatchDetails.Add(objbth);
                    }
                }

                if (objLtBatchDetails.Count() == 0)
                    return lstBatch;

                lstBatch = new BatchDetails[objLtBatchDetails.Count];
                int i = 0;
                foreach (BatchDetails obj in objLtBatchDetails)
                {
                    lstBatch[i] = obj;
                    i++;
                }
            }
            return lstBatch;
        }

        public FeeDetails[] GetFeeDetailsList(FeeDetailsSyncInput objSyncInput, out int count)
        {
            FeeDetails[] lstFee = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from fee in db.tblfeestudents
                             join sch in db.tblfeeschedules on fee.SchId equals sch.Id
                             join account in db.tblfeeaccounts on sch.FeeId equals account.Id
                             where fee.SyncDate >= syncdate
                             orderby fee.SyncDate ascending
                             select new { Feename = account.AccountName, sch.BatchId, sch.FeeId, sch.Duedate, sch.LastDate, sch.PeriodId, fee.Id, fee.SchId, fee.StudId, fee.Amount, fee.BalanceAmount, fee.Status, FeeType = account.Type, fee.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstFee;

                List<FeeDetails> objLtFeeDetails = new List<FeeDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        FeeDetails objfee = new FeeDetails();
                        objfee.FeeStudentId = obj.Id;
                        objfee.Amount = (double)obj.Amount;

                        objfee.BalanceAmount = (double)obj.BalanceAmount;
                        objfee.BatchId = (int)obj.BatchId;
                        objfee.Duedate = (DateTime)obj.Duedate;
                        objfee.FeeId = (int)obj.FeeId;
                        objfee.Status = obj.Status;
                        objfee.FeeName = obj.Feename;
                        objfee.FeeScheduleId = (int)obj.SchId;
                        objfee.StudentId = (int)obj.StudId;
                        objfee.FeeType = (int)obj.FeeType;
                        objfee.Lastdate = (DateTime)obj.LastDate;
                        objfee.PeriodId = (int)obj.PeriodId;
                        if (obj.SyncDate != null)
                            objfee.SyncDate = (long)obj.SyncDate;

                        objLtFeeDetails.Add(objfee);
                    }
                }

                if (objLtFeeDetails.Count() == 0)
                    return lstFee;

                

                lstFee = new FeeDetails[objLtFeeDetails.Count];
                int i = 0;
                foreach (FeeDetails obj in objLtFeeDetails)
                {
                    lstFee[i] = obj;
                    i++;
                }
            }
            
            return lstFee;
        }

    

   

        public PeriodDetails[] GetPeriodDetailsList(PeriodDetailsSyncInput objSyncInput, out int count)
        {
            PeriodDetails[] lstPeriod = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from perd in db.tblperiods
                             where perd.SyncDate >= syncdate
                             orderby perd.SyncDate ascending
                             select new { perd.Id, perd.FrequencyId, perd.Period, perd.Order, perd.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstPeriod;

                List<PeriodDetails> objLtPeriodDetails = new List<PeriodDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        PeriodDetails objPeriod = new PeriodDetails();
                        objPeriod.Id = obj.Id;
                        objPeriod.PeriodName = obj.Period;
                        objPeriod.FrequencyId = obj.FrequencyId;
                        objPeriod.Order = (int)obj.Order;
                        if (obj.SyncDate != null)
                            objPeriod.SyncDate = (long)obj.SyncDate;

                        objLtPeriodDetails.Add(objPeriod);
                    }
                }

                if (objLtPeriodDetails.Count() == 0)
                    return lstPeriod;

                lstPeriod = new PeriodDetails[objLtPeriodDetails.Count];
                int i = 0;
                foreach (PeriodDetails obj in objLtPeriodDetails)
                {
                    lstPeriod[i] = obj;
                    i++;
                }
            }
            return lstPeriod;
        }

        public ConfigurationDetails[] GetConfigDetailsList(ConfigDetailsSyncInput objSyncInput, out int count)
        {
            ConfigurationDetails[] lstConfig = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from config in db.tblconfigurations
                             where config.SyncDate >= syncdate
                             orderby config.SyncDate ascending
                             select new { config.Id, config.Name, config.Value, config.SubValue, config.Disc, config.Module, config.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstConfig;

                List<ConfigurationDetails> objLtConfigDetails = new List<ConfigurationDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        ConfigurationDetails objConfig = new ConfigurationDetails();
                        objConfig.Id = obj.Id;
                        objConfig.ConfigName = obj.Name;
                        objConfig.Discription = obj.Disc;
                        objConfig.Module = obj.Module;
                        objConfig.Value = obj.Value;
                        objConfig.SubValue = obj.SubValue;
                        if (obj.SyncDate != null)
                            objConfig.SyncDate = (long)obj.SyncDate;

                        objLtConfigDetails.Add(objConfig);
                    }
                }

                if (objLtConfigDetails.Count() == 0)
                    return lstConfig;

                lstConfig = new ConfigurationDetails[objLtConfigDetails.Count];
                int i = 0;
                foreach (ConfigurationDetails obj in objLtConfigDetails)
                {
                    lstConfig[i] = obj;
                    i++;
                }
            }
            return lstConfig;
        }

        public OtherFeeDetails[] GetOtherFeeMasterList(OtherFeeMasterSyncInput objSyncInput, out int count)
        {
            OtherFeeDetails[] lstOtherFee = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from fee in db.tblfeeothermasters
                             where fee.SyncDate >= syncdate
                             orderby fee.SyncDate ascending
                             select new { fee.Id, fee.Name, fee.Description, fee.Refundable, fee.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstOtherFee;

                List<OtherFeeDetails> objLtOtherFeeDetails = new List<OtherFeeDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        OtherFeeDetails objotherfee = new OtherFeeDetails();
                        objotherfee.Id = obj.Id;
                        objotherfee.Name = obj.Name;
                        objotherfee.Description = obj.Description;
                        objotherfee.Refundable = (int)obj.Refundable;
                        if (obj.SyncDate != null)
                            objotherfee.SyncDate = (long)obj.SyncDate;

                        objLtOtherFeeDetails.Add(objotherfee);
                    }
                }

                if (objLtOtherFeeDetails.Count() == 0)
                    return lstOtherFee;

                lstOtherFee = new OtherFeeDetails[objLtOtherFeeDetails.Count];
                int i = 0;
                foreach (OtherFeeDetails obj in objLtOtherFeeDetails)
                {
                    lstOtherFee[i] = obj;
                    i++;
                }
            }
            return lstOtherFee;
        }

        public FeeAdvanceDetails[] GetFeeAdvanceDetailsList(FeeAdvanceDetailsSyncInput objSyncInput, out int count)
        {
            FeeAdvanceDetails[] lstFeeadvance = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from Feeadvance in db.tblstudentfeeadvances
                             where Feeadvance.SyncDate >= syncdate
                             orderby Feeadvance.SyncDate ascending
                             select new { Feeadvance.Id, Feeadvance.StudentId, Feeadvance.StudentName, Feeadvance.FeeName, Feeadvance.PeriodName, Feeadvance.BatchId, Feeadvance.Amount, Feeadvance.FeeId, Feeadvance.PeriodId, Feeadvance.TempId, Feeadvance.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstFeeadvance;

                List<FeeAdvanceDetails> objLtFeeadvanceDetails = new List<FeeAdvanceDetails>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        FeeAdvanceDetails objFeeadvance = new FeeAdvanceDetails();
                        objFeeadvance.Id = obj.Id;
                        if (obj.Amount != null)
                            objFeeadvance.Amount = (double)obj.Amount;
                        if (obj.BatchId != null)
                            objFeeadvance.BatchId = (int)obj.BatchId;
                        if (obj.FeeId != null)
                            objFeeadvance.FeeId = (int)obj.FeeId;
                        if (obj.FeeName != null)
                            objFeeadvance.FeeName = obj.FeeName;
                        if (obj.PeriodId != null)
                            objFeeadvance.PeriodId = (int)obj.PeriodId;
                        if (obj.PeriodName != null)
                            objFeeadvance.PeriodName = obj.PeriodName;
                        if (obj.StudentId != null)
                            objFeeadvance.StudentId = (int)obj.StudentId;
                        if (obj.StudentName != null)
                            objFeeadvance.StudentName = obj.StudentName;
                        if (obj.TempId != null)
                            objFeeadvance.TempId = obj.TempId;
                        if (obj.SyncDate != null)
                            objFeeadvance.SyncDate = (long)obj.SyncDate;

                        objLtFeeadvanceDetails.Add(objFeeadvance);
                    }
                }

                if (objLtFeeadvanceDetails.Count() == 0)
                    return lstFeeadvance;

                lstFeeadvance = new FeeAdvanceDetails[objLtFeeadvanceDetails.Count];
                int i = 0;
                foreach (FeeAdvanceDetails obj in objLtFeeadvanceDetails)
                {
                    lstFeeadvance[i] = obj;
                    i++;
                }
            }
            return lstFeeadvance;
        }

        public FeeBillFormat[] GetFeeBillFormatList(FeeBillFormatSyncInput objSyncInput, out int count)
        {
            FeeBillFormat[] lstFeeBillFormat = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from fee in db.tblfeebilltypes
                             where fee.SyncDate >= syncdate
                             orderby fee.SyncDate ascending
                             select new { fee.Id, fee.Name, fee.NeedOfficeCopy, fee.PageName, fee.IsPDF, fee.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstFeeBillFormat;

                List<FeeBillFormat> objLtFeeBillFormat = new List<FeeBillFormat>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        FeeBillFormat objFeeBillFormat = new FeeBillFormat();
                        objFeeBillFormat.Id = (int)obj.Id;
                        objFeeBillFormat.Name = obj.Name;
                        objFeeBillFormat.IsPDF = (int)obj.IsPDF;
                        if (obj.NeedOfficeCopy)
                        {
                            objFeeBillFormat.NeedOfficeCopy = 1;
                        }
                        else
                        {
                            objFeeBillFormat.NeedOfficeCopy = 0;
                        }
                        objFeeBillFormat.PageName = obj.PageName;
                        if (obj.SyncDate != null)
                            objFeeBillFormat.SyncDate = (long)obj.SyncDate;

                        objLtFeeBillFormat.Add(objFeeBillFormat);
                    }
                }

                if (objLtFeeBillFormat.Count() == 0)
                    return lstFeeBillFormat;

                lstFeeBillFormat = new FeeBillFormat[objLtFeeBillFormat.Count];
                int i = 0;
                foreach (FeeBillFormat obj in objLtFeeBillFormat)
                {
                    lstFeeBillFormat[i] = obj;
                    i++;
                }
            }
            return lstFeeBillFormat;
        }


        public FineSetting[] GetFineSettingList(FineSettingSyncInput objSyncInput, out int count)
        {
            FineSetting[] lstFineSetting = null;
            count = 0;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var query = (from fine in db.tblfines
                             where fine.SyncDate >= syncdate
                             orderby fine.SyncDate ascending
                             select new { fine.Id, fine.Amount, fine.FeeId, fine.FineAmounttype, fine.Finedate, fine.FineDuration, fine.Frequency, fine.Type, fine.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstFineSetting;

                List<FineSetting> objLtFineSetting = new List<FineSetting>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        FineSetting objFineSetting = new FineSetting();
                        objFineSetting.Id = (int)obj.Id;
                        objFineSetting.Amount = (double)obj.Amount;
                        objFineSetting.FeeId = (int)obj.FeeId;
                        objFineSetting.FineAmounttype = (int)obj.FineAmounttype;
                        objFineSetting.Finedate = (int)obj.Finedate;
                        objFineSetting.FineDuration = (int)obj.FineDuration;
                        objFineSetting.Frequency = (double)obj.Frequency;
                        objFineSetting.Type = obj.Type;
                        if (obj.SyncDate != null)
                            objFineSetting.SyncDate = (long)obj.SyncDate;

                        objLtFineSetting.Add(objFineSetting);
                    }
                }

                if (objLtFineSetting.Count() == 0)
                    return lstFineSetting;

                lstFineSetting = new FineSetting[objLtFineSetting.Count];
                int i = 0;
                foreach (FineSetting obj in objLtFineSetting)
                {
                    lstFineSetting[i] = obj;
                    i++;
                }
            }
            return lstFineSetting;
        }


        public bool SaveStudentAttendance(ClassAttendanceDetails objSyncInput)
        {
            #region GetStander,Batch id
            int StanderdId, BatchId;
            GetStanderdIDAndBatchID(objSyncInput.ClassId, out StanderdId, out BatchId);
            if (StanderdId <= 0 || BatchId <= 0)
                return false;
            #endregion

            #region Check Tables Exist

            if (!AttendanceTables_Exits(StanderdId, BatchId))
                CreateSingle_StudentAttendanceTables(StanderdId, BatchId);
            #endregion

            #region inserting and updating attendance
            using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
            {
                objConnection.Open();
                bool _needUpdate = false;
                int ClassAttendanceId = 0;
                using (MySqlCommand objCommand = new MySqlCommand())
                {
                    objCommand.Connection = objConnection;
                    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.CommandText = "SELECT Id FROM tblattdcls_std" + StanderdId + "yr" + BatchId + " WHERE ClassId =" + objSyncInput.ClassId + " and `Date`=Date('" + objSyncInput.Att_Date.ToString() + "')";
                    MySqlDataReader objReader = objCommand.ExecuteReader();
                    if (objReader.HasRows)
                        if (objReader.Read())
                        {
                            ClassAttendanceId = int.Parse(objReader.GetValue(0).ToString());
                            _needUpdate = true;
                        }
                    objReader.Dispose();
                    objReader = null;

                    if (_needUpdate == true)
                    {
                        objCommand.CommandText = "update tblattdcls_std" + StanderdId + "yr" + BatchId + " set LastModifiedDateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',LastModifiedUserId=" + objSyncInput.CreatedUserId + " where Id=" + ClassAttendanceId;
                        objReader = objCommand.ExecuteReader();
                        objReader.Dispose();
                        objReader = null;
                        objCommand.CommandText = "delete from tblattdstud_std" + StanderdId + "yr" + BatchId + " where ClassAttendanceId=" + ClassAttendanceId;
                        objReader = objCommand.ExecuteReader();
                    }
                    else
                    {
                        objCommand.CommandText = "INSERT INTO tblattdcls_std" + StanderdId + "yr" + BatchId + "(ClassId,Date,Status,LastModifiedDateTime,LastModifiedUserId,CreatedDateTime,CreatedUserId) VALUES(" + objSyncInput.ClassId + ",'" + objSyncInput.Att_Date.ToString() + "',3,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + objSyncInput.CreatedUserId + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + objSyncInput.CreatedUserId + "); select last_insert_id();";
                        objReader = objCommand.ExecuteReader();
                        if (objReader.HasRows)
                            if (objReader.Read())
                                ClassAttendanceId = int.Parse(objReader.GetValue(0).ToString());
                    }
                    objReader.Dispose();
                    objReader = null;
                    for (int i = 0; i < objSyncInput.AttendanceList.Count; i++)
                    {
                        int StudentStatus;
                        switch (objSyncInput.AttendanceList[i].Status)
                        {
                            case Constants.AttendanceStatus.Absent:
                                StudentStatus = 0;
                                break;
                            case Constants.AttendanceStatus.ForeNoon:
                                StudentStatus = 1;
                                break;
                            case Constants.AttendanceStatus.AfterNoon:
                                StudentStatus = 2;
                                break;
                            case Constants.AttendanceStatus.Present:
                                StudentStatus = 3;
                                break;
                            default:
                                StudentStatus = 0;
                                break;
                        }
                        objCommand.CommandText = "insert into tblattdstud_std" + StanderdId + "yr" + BatchId + "(ClassAttendanceId,StudentId,PresentStatus) values(" + ClassAttendanceId + "," + objSyncInput.AttendanceList[i].StudentID + "," + StudentStatus + ")";
                        MySqlDataReader objReader1 = objCommand.ExecuteReader();
                        objReader1.Dispose();
                        objReader1 = null;
                    }
                }
                objConnection.Close();
            }
            #endregion

            return true;
        }

        private void GetStanderdIDAndBatchID(int _ClassId, out int StanderdId, out int BatchId)
        {
            StanderdId = 0; BatchId = 0;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                #region GetBatchId
                tblbatch objBatchtbl = db.tblbatches.Where(K => K.Status == 1).FirstOrDefault();
                if (objBatchtbl != null)
                    if (objBatchtbl.Id > 0)
                        BatchId = (int)objBatchtbl.Id;
                #endregion

                #region StanderdId
                tblclass objClasstbl = db.tblclasses.Where(k => k.Id == _ClassId).FirstOrDefault();
                if (objClasstbl != null)
                    if (objClasstbl.Standard > 0)
                        StanderdId = (int)objClasstbl.Standard;
                #endregion
            }
        }

        public bool AttendanceTables_Exits(int StdId, int YearId)
        {
            bool valid = false;
            string _tablename = "";
            string End_Region = "std" + StdId + "yr" + YearId;
            string Sql = "show tables like 'tblattdcls_" + End_Region + "'";
            using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
            {
                objConnection.Open();
                using (MySqlCommand objCommand = new MySqlCommand())
                {
                    objCommand.Connection = objConnection;
                    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.CommandText = Sql;
                    using (MySqlDataReader objReader = objCommand.ExecuteReader())
                    {
                        if (objReader.HasRows)
                            if (objReader.Read())
                            {
                                _tablename = objReader.GetValue(0).ToString();
                                if (_tablename != "")
                                    valid = true;
                            }
                    }

                    if (valid)
                    {
                        _tablename = "";
                        valid = false;
                        objCommand.CommandText = "show tables like 'tblattdstud_" + End_Region + "'";
                        using (MySqlDataReader objReader = objCommand.ExecuteReader())
                        {
                            if (objReader.HasRows)
                                if (objReader.Read())
                                {
                                    _tablename = objReader.GetValue(0).ToString();
                                    if (_tablename != "")
                                        valid = true;
                                }
                        }
                    }
                }
                objConnection.Close();
            }
            return valid;
        }

        public void CreateSingle_StudentAttendanceTables(int StandardId, int BatchId)
        {
            if (StandardId > 0 && BatchId > 0)
            {
                using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
                {
                    objConnection.Open();
                    using (MySqlCommand objCommand = new MySqlCommand())
                    {
                        objCommand.Connection = objConnection;
                        objCommand.CommandType = System.Data.CommandType.Text;
                        objCommand.CommandText = "DROP TABLE IF EXISTS `tblattdcls_std" + StandardId + "yr" + BatchId + "`; CREATE TABLE `tblattdcls_std" + StandardId + "yr" + BatchId +
                            "` (`Id` int(15) NOT NULL AUTO_INCREMENT,`ClassId` int(11) DEFAULT NULL,`Date` date DEFAULT NULL,`Status` tinyint(4) DEFAULT NULL COMMENT '1:ForeNoon , 2:AfterNoon , 3:FullDay',`LastModifiedDateTime` datetime DEFAULT NULL,`LastModifiedUserId` int(11) DEFAULT NULL,`CreatedDateTime` datetime DEFAULT NULL,`CreatedUserId` int(11) DEFAULT NULL,PRIMARY KEY (`Id`),UNIQUE KEY `UniqueIndex` (`ClassId`,`Date`)); DROP TABLE IF EXISTS `tblattdstud_std" + StandardId + "yr" + BatchId + "`; CREATE TABLE `tblattdstud_std" + StandardId + "yr" + BatchId + "` (`Id` int(11) NOT NULL AUTO_INCREMENT,`ClassAttendanceId` int(15) DEFAULT NULL,`StudentId` bigint(20) DEFAULT NULL,`PresentStatus` tinyint(4) DEFAULT '0' COMMENT '0:Absent , 1:ForNoon , 2:AfterNoon , 3:FullDay',`ApproveStatus` tinyint(3) DEFAULT '0',`ApproveId` int(11) DEFAULT NULL, `InTime` varchar(11) DEFAULT NULL, `OutTime` varchar(11) DEFAULT NULL,`IsLate` tinyint(3) DEFAULT '0' COMMENT '1 means student is Late', `LateValue` varchar(11) DEFAULT NULL, PRIMARY KEY (`Id`));";
                        objCommand.ExecuteReader();

                    }
                    objConnection.Close();
                }
            }
        }

        public List<ClassAttendanceDetails> ReadAttendance(DownloadAttendanceSyncInput objKey)
        {
            int StanderdId, BatchId;
            List<ClassAttendanceDetails> objClassAttlist = new List<ClassAttendanceDetails>();
            List<tblstandard> objTblStndrd = null;

            #region LoadDetails
            GetStanderdIDAndBatchID(0, out StanderdId, out BatchId);
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                objTblStndrd = db.tblstandards.ToList();
            }
            #endregion

            if (objTblStndrd.Count > 0)
                using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
                {
                    for (int i = 0; i < objTblStndrd.Count; i++)
                    {
                        StanderdId = objTblStndrd[i].Id;
                        if (AttendanceTables_Exits(StanderdId, BatchId))
                        {
                            #region Read Attendance Taken Class List
                            objConnection.Open();
                            MySqlConnection adapter = new MySqlConnection();
                            string sql = "SELECT Id,ClassId,DATE_FORMAT(Date, '%Y-%m-%d') FROM tblattdcls_std" + StanderdId + "yr" + BatchId + " WHERE `Date`=Date('" + DateTime.Today.ToString("yyyy-MM-dd") + "')";
                            MySqlDataAdapter adr = new MySqlDataAdapter(sql, objConnection);
                            adr.SelectCommand.CommandType = CommandType.Text;
                            DataTable dt = new DataTable();
                            adr.Fill(dt);
                            #endregion

                            #region Read Student Attendance
                            foreach (DataRow dr in dt.Rows)
                            {
                                int ClassAttendanceId = 0;
                                ClassAttendanceDetails _objClassAtt = new ClassAttendanceDetails();

                                ClassAttendanceId = int.Parse(dr["Id"].ToString());
                                _objClassAtt.ClassId = int.Parse(dr["ClassId"].ToString());
                                _objClassAtt.Att_Date = dr["DATE_FORMAT(Date, '%Y-%m-%d')"].ToString();

                                if (ClassAttendanceId > 0)
                                {
                                    List<StudentAttendanceStatus> _objStudAttList = new List<StudentAttendanceStatus>();
                                    StudentAttendanceStatus _objStudAttStatus = new StudentAttendanceStatus();
                                    MySqlCommand objCommand1 = new MySqlCommand();
                                    objCommand1.Connection = objConnection;
                                    objCommand1.CommandType = System.Data.CommandType.Text;
                                    objCommand1.CommandText = "select StudentId,PresentStatus from tblattdstud_std" + StanderdId + "yr" + BatchId + " WHERE ClassAttendanceId =" + ClassAttendanceId;
                                    MySqlDataReader objReader1 = objCommand1.ExecuteReader();
                                    if (objReader1.HasRows)
                                        while (objReader1.Read())
                                        {
                                            _objStudAttStatus = new StudentAttendanceStatus();
                                            _objStudAttStatus.StudentID = int.Parse(objReader1.GetValue(0).ToString());
                                            switch (int.Parse(objReader1.GetValue(1).ToString()))
                                            {
                                                case 0:
                                                    _objStudAttStatus.Status = Constants.AttendanceStatus.Absent;
                                                    break;
                                                case 1:
                                                    _objStudAttStatus.Status = Constants.AttendanceStatus.ForeNoon;
                                                    break;
                                                case 2:
                                                    _objStudAttStatus.Status = Constants.AttendanceStatus.AfterNoon;
                                                    break;
                                                case 3:
                                                    _objStudAttStatus.Status = Constants.AttendanceStatus.Present;
                                                    break;
                                                default:
                                                    _objStudAttStatus.Status = _objStudAttStatus.Status = Constants.AttendanceStatus.Absent;
                                                    break;
                                            }
                                            _objStudAttList.Add(_objStudAttStatus);
                                        }
                                    _objClassAtt.AttendanceList = _objStudAttList;
                                    objClassAttlist.Add(_objClassAtt);
                                    objReader1.Dispose();
                                    objReader1 = null;
                                }
                            }
                            objConnection.Close();
                            #endregion
                        }
                    }
                }
            return objClassAttlist;
        }

        public ImageDetails[] ReadImageDetails(ImageSyncInput objSyncInput, out int count, out bool NeedSync)
        {
            ImageDetails[] lstImage = null;
            count = 0;
            NeedSync = true;
            long syncdate = objSyncInput.SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from st in db.tblfileurls
                                  orderby st.SyncDate ascending
                                  select st);
                count = queryCount.Count();

                var query = (from st in db.tblfileurls
                             where st.SyncDate > syncdate
                             orderby st.SyncDate ascending
                             select new { st.Id, st.Type, st.UserId, st.FileBytes, st.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lstImage;

                List<ImageDetails> objLtImageDetails = new List<ImageDetails>();

                foreach (var obj in query.Take(5))
                {
                    if (obj.SyncDate > syncdate)
                    {
                        ImageDetails objStud = new ImageDetails();
                        objStud.Id = obj.Id;
                        objStud.FileType = obj.Type;
                        objStud.Image = obj.FileBytes;
                        if (obj.UserId != null)
                            objStud.UserId = (int)obj.UserId;
                        if (obj.SyncDate != null)
                            objStud.SyncDate = (long)obj.SyncDate;

                        objLtImageDetails.Add(objStud);

                    }
                }

                for (int i = 5; i < query.Count; i++)
                {
                    if (objLtImageDetails[i - 1].SyncDate == query[i].SyncDate)
                    {
                        ImageDetails objStud = new ImageDetails();
                        objStud.Id = query[i].Id;
                        objStud.FileType = query[i].Type;
                        objStud.Image = query[i].FileBytes;
                        if (query[i].UserId != null)
                            objStud.UserId = (int)query[i].UserId;
                        if (query[i].SyncDate != null)
                            objStud.SyncDate = (long)query[i].SyncDate;

                        objLtImageDetails.Add(objStud);
                    }
                    else
                    {
                        break;
                    }

                }
                if (objLtImageDetails.Count == query.Count)
                    NeedSync = false;
                if (objLtImageDetails.Count() == 0)
                    return lstImage;

                lstImage = new ImageDetails[objLtImageDetails.Count];
                int j = 0;
                foreach (ImageDetails obj in objLtImageDetails)
                {
                    lstImage[j] = obj;
                    j++;
                }
            }
            return lstImage;
        }

        public bool UploadOfflineWinerData(OfflineUploadSyncInput objSyncInput, int DeviceId)
        {
            bool _valid = false;
            int Id = objSyncInput.Id;
            DateTime ChangeDate = objSyncInput.ChangeDate;
            string ColumnName = objSyncInput.ColumnName;
            int ColValue = objSyncInput.ColValue;
            string FieldStr = objSyncInput.FieldStr;
            string TableName = objSyncInput.TableName;
            string TransactionType = objSyncInput.TransactionType;
            if (Save_OfflineData(Id, TableName, ColumnName, ColValue, TransactionType, ChangeDate, FieldStr, DeviceId))
            {
                _valid = true;
            }
            return _valid;
        }

        private bool Save_OfflineData(int Id, string TableName, string ColumnName, int ColValue, string TransactionType, DateTime ChangeDate, string FieldStr, int DeviceId)
        {
            bool _valid = false;

            switch (TableName)
            {
                case "tbl_Feebill":
                    return StoreFeeBillMaster(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);

                case "tbl_FeeTransaction":
                    return StoreFeeBillTransaction(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);

                case "tbl_FeeDetails":
                    return StoreFeeDetails(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);

                case "tbl_FeeAdvancePaid":
                    return StoreFeeAdvancePaid(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);


                case "tbl_FeeAdvanceTransaction":
                    return StoreFeeAdvanceTransaction(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);


                case "tbl_FeebillClearance":
                    return StoreFeeBillClearanceMaster(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);


                case "tbl_FeeTransactionClearance":
                    return StoreFeeBillTransactionClearance(Id, TransactionType, ChangeDate, ColValue, FieldStr, DeviceId);


            }

            return _valid;
        }

        private bool StoreFeeDetails(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            bool _continue = true;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tblfeestudent _objtbl = new tblfeestudent();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    if (TransactionType == "UPDATE")
                    {
                        _objtbl = db.tblfeestudents.Where(s => s.Id == ColValue).FirstOrDefault();
                        if (_objtbl != null)
                        {
                            if (_objtbl.SyncDate > long.Parse(FieldData[12]))
                            {
                                _continue = false;
                                FeeDetails _objconflit = new FeeDetails();

                                if (FieldData[0] != "NULL")
                                    _objconflit.FeeStudentId = int.Parse(FieldData[0]);
                                if (FieldData[1] != "NULL")
                                    _objconflit.FeeScheduleId = int.Parse(FieldData[1]);
                                if (FieldData[2] != "NULL")
                                    _objconflit.StudentId = int.Parse(FieldData[2]);
                                if (FieldData[3] != "NULL")
                                    _objconflit.Amount = double.Parse(FieldData[3]);
                                if (FieldData[4] != "NULL")
                                    _objconflit.BalanceAmount = double.Parse(FieldData[4]);
                                if (FieldData[5] != "NULL")
                                    _objconflit.Status = FieldData[5];
                                if (FieldData[12] != "NULL")
                                    _objconflit.SyncDate = long.Parse(FieldData[12]);

                                if (FieldData[9] != "NULL")
                                    _objconflit.Duedate = FormatUtility.GetDate(FieldData[9]);
                                if (FieldData[10] != "NULL")
                                    _objconflit.Lastdate = FormatUtility.GetDate(FieldData[10]);

                                string RowDevice = SerializeJSON(_objconflit, typeof(FeeDetails));

                                FeeDetails _objserver = new FeeDetails();

                                _objserver.FeeStudentId = _objtbl.Id;
                                _objserver.FeeScheduleId = (int)_objtbl.SchId;
                                _objserver.StudentId = (int)_objtbl.StudId;
                                _objserver.Amount = (double)_objtbl.Amount;
                                _objserver.BalanceAmount = (double)_objtbl.BalanceAmount;
                                _objserver.Status = _objtbl.Status;
                                _objserver.SyncDate = (long)_objtbl.SyncDate;

                                if (FieldData[9] != "NULL")
                                    _objserver.Duedate = FormatUtility.GetDate(FieldData[9]);
                                if (FieldData[10] != "NULL")
                                    _objserver.Lastdate = FormatUtility.GetDate(FieldData[10]);

                                string RowServer = SerializeJSON(_objserver, typeof(FeeDetails));
                                DeviceUpdateConflicts("tblfeestudent", "Id", ColValue, DateTime.Now, RowServer, RowDevice, DeviceId, db);
                                StoreUpdateLogs("tblfeestudent", "Id", ColValue, TransactionType, ChangeDate, 0, FieldStr, "Table Conflit", DeviceId, db);
                            }
                        }
                    }

                    if (_continue)
                    {
                        if (FieldData[1] != "NULL")
                            _objtbl.SchId = int.Parse(FieldData[1]);
                        if (FieldData[2] != "NULL")
                            _objtbl.StudId = int.Parse(FieldData[2]);
                        if (FieldData[3] != "NULL")
                            _objtbl.Amount = double.Parse(FieldData[3]);
                        if (FieldData[4] != "NULL")
                            _objtbl.BalanceAmount = double.Parse(FieldData[4]);
                        if (FieldData[5] != "NULL")
                            _objtbl.Status = FieldData[5];

                        if (TransactionType == "INSERT")
                        {
                            db.tblfeestudents.AddObject(_objtbl);
                        }
                        StoreUpdateLogs("tblfeestudent", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                    }


                    db.SaveChanges();
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }

            }


            return _valid;
        }

        private bool StoreFeeAdvancePaid(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            bool _continue = true;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    int _BatchId = int.Parse(FieldData[4]);
                    int _FeeId = int.Parse(FieldData[6]);
                    int _PeriodId = int.Parse(FieldData[7]);
                    int _StudentId = int.Parse(FieldData[1]);

                    tblstudentfeeadvance _objtbl=new tblstudentfeeadvance();

                    if (TransactionType == "UPDATE")
                    {
                        if (FieldData[0] == "0")
                        {
                            _objtbl = db.tblstudentfeeadvances.Where(d => d.BatchId == _BatchId && d.FeeId == _FeeId && d.PeriodId == _PeriodId && d.TempId == FieldData[8]).FirstOrDefault();
                        }
                        else
                        {
                            _objtbl = db.tblstudentfeeadvances.Where(d => d.BatchId == _BatchId && d.FeeId == _FeeId && d.PeriodId == _PeriodId && d.StudentId ==_StudentId).FirstOrDefault();
                        }
                        if (_objtbl != null)
                        {
                            if (_objtbl.SyncDate > long.Parse(FieldData[10]))
                            {
                                _continue = false;

                                FeeAdvanceDetails _objconflit = new FeeAdvanceDetails();
                                if (FieldData[0] != "NULL")
                                    _objconflit.Id = int.Parse(FieldData[0]);
                                if (FieldData[1] != "NULL")
                                    _objconflit.StudentId = int.Parse(FieldData[1]);
                                if (FieldData[2] != "NULL")
                                    _objconflit.StudentName = FieldData[2];
                                if (FieldData[3] != "NULL")
                                    _objconflit.FeeName = FieldData[3];
                                if (FieldData[4] != "NULL")
                                    _objconflit.BatchId = int.Parse(FieldData[4]);
                                if (FieldData[5] != "NULL")
                                    _objconflit.Amount = double.Parse(FieldData[5]);
                                if (FieldData[6] != "NULL")
                                    _objconflit.FeeId = int.Parse(FieldData[6]);
                                if (FieldData[7] != "NULL")
                                    _objconflit.PeriodId = short.Parse(FieldData[7]);
                                if (FieldData[8] != "NULL")
                                    _objconflit.TempId = FieldData[8];
                                if (FieldData[9] != "NULL")
                                    _objconflit.PeriodName = FieldData[9];
                                if (FieldData[10] != "NULL")
                                    _objconflit.SyncDate = long.Parse(FieldData[10]);

                                string RowDevice = SerializeJSON(_objconflit, typeof(FeeAdvanceDetails));


                                FeeAdvanceDetails _objserver = new FeeAdvanceDetails();
                                _objserver.Id = _objtbl.Id;
                                _objserver.StudentId = (int)_objtbl.StudentId;
                                _objserver.StudentName = _objtbl.StudentName;
                                _objserver.FeeName = _objtbl.FeeName;
                                _objserver.BatchId = (int)_objtbl.BatchId;
                                _objserver.Amount = (double)_objtbl.Amount;
                                _objserver.FeeId = (int)_objtbl.FeeId;
                                _objserver.PeriodId = (int)_objtbl.PeriodId;
                                _objserver.TempId = _objtbl.TempId;
                                _objserver.SyncDate = (long)_objtbl.SyncDate;
                                string RowServer = SerializeJSON(_objserver, typeof(FeeAdvanceDetails));
                                DeviceUpdateConflicts("tblstudentfeeadvance", "Id", ColValue, DateTime.Now, RowServer, RowDevice, DeviceId, db);
                                StoreUpdateLogs("tblstudentfeeadvance", "Id", ColValue, TransactionType, ChangeDate, 0, FieldStr, "Table Conflit", DeviceId, db);
                            }
                        }
                    }

                    if (_continue)
                    {
                        if (FieldData[1] != "NULL")
                            _objtbl.StudentId = int.Parse(FieldData[1]);
                        if (FieldData[2] != "NULL")
                            _objtbl.StudentName = FieldData[2];
                        if (FieldData[3] != "NULL")
                            _objtbl.FeeName = FieldData[3];
                        if (FieldData[4] != "NULL")
                            _objtbl.BatchId = int.Parse(FieldData[4]);
                        if (FieldData[5] != "NULL")
                            _objtbl.Amount = double.Parse(FieldData[5]);
                        if (FieldData[6] != "NULL")
                            _objtbl.FeeId = int.Parse(FieldData[6]);
                        if (FieldData[7] != "NULL")
                            _objtbl.PeriodId = short.Parse(FieldData[7]);
                        if (FieldData[8] != "NULL")
                            _objtbl.TempId = FieldData[8];

                        if (FieldData[9] != "NULL")
                            _objtbl.PeriodName = FieldData[9];

                        if (TransactionType == "INSERT")
                        {
                            db.tblstudentfeeadvances.AddObject(_objtbl);
                        }
                        StoreUpdateLogs("tblstudentfeeadvance", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                    }
                    db.SaveChanges();
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }

            }


            return _valid;
        }

        private void DeviceUpdateConflicts(string TableName, string ColName, int ColValue, DateTime UpdateDate, string RowServer, string RowDevice, int DeviceId, WinErdbEntities db)
        {
            tbldeviceuploadconflict _objtbl = new tbldeviceuploadconflict();
            _objtbl.TableName = TableName;
            _objtbl.Conflict_Column = ColName;
            _objtbl.ConflictId = ColValue;
            _objtbl.DeviceId = DeviceId;
            _objtbl.RowDataServer = RowServer;
            _objtbl.RowDataDevice = RowDevice;
            _objtbl.UpdateDate = UpdateDate;
            db.tbldeviceuploadconflicts.AddObject(_objtbl);
        }

        private void StoreUpdateLogs(string TableName, string ColName, int ColValue, string TransactionType, DateTime ChangeDate, int IsUploaded, string FieldStr, string Comment, int DeviceId, WinErdbEntities db)
        {
            tbldeviceupdate _objtbl = new tbldeviceupdate();
            _objtbl.ChangeDate = ChangeDate;
            _objtbl.ColumnName = ColName;
            _objtbl.ColValue = ColValue;
            _objtbl.Comment = Comment;
            _objtbl.DeviceId = DeviceId;
            _objtbl.FieldStr = FieldStr;
            _objtbl.IsUpdated = IsUploaded;
            _objtbl.TableName = TableName;
            _objtbl.TransactionType = TransactionType;
            db.tbldeviceupdates.AddObject(_objtbl);
        }

        private bool StoreFeeAdvanceTransaction(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tblfeeadvancetransaction objtbl = new tblfeeadvancetransaction();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    if (FieldData[1] != "NULL")
                        objtbl.StudentId = int.Parse(FieldData[1]);
                    if (FieldData[2] != "NULL")
                        objtbl.StudentName = FieldData[2];
                    if (FieldData[3] != "NULL")
                        objtbl.FeeName = FieldData[3];
                    if (FieldData[4] != "NULL")
                        objtbl.PeriodName = FieldData[4];
                    if (FieldData[5] != "NULL")
                        objtbl.BatchId = int.Parse(FieldData[5]);
                    if (FieldData[6] != "NULL")
                        objtbl.Amount = double.Parse(FieldData[6]);
                    if (FieldData[7] != "NULL")
                        objtbl.Type = byte.Parse(FieldData[7]);
                    if (FieldData[8] != "NULL")
                        objtbl.BillNo = FieldData[8];
                    if (FieldData[9] != "NULL")
                        objtbl.FeeId = sbyte.Parse(FieldData[9]);
                    double AdvanceBal = GetAdvanceBalance(db);
                    AdvanceBal = AdvanceBal + double.Parse(FieldData[6]);
                    objtbl.AdvanceBalance = AdvanceBal;

                    tblfeeadvancetransaction _objtbl = null;
                    _objtbl = db.tblfeeadvancetransactions.Where(d => d.BillNo == objtbl.BillNo && d.FeeName == objtbl.FeeName && d.PeriodName == objtbl.PeriodName).FirstOrDefault();
                    if (_objtbl == null)
                    {
                        db.tblfeeadvancetransactions.AddObject(objtbl);
                        StoreUpdateLogs("tblfeeadvancetransaction", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                        db.SaveChanges();
                    }
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }
            }

            return _valid;
        }

        private double GetAdvanceBalance(WinErdbEntities db)
        {
            double _double = 0;
            tblfeeadvancetransaction objtbl = db.tblfeeadvancetransactions.OrderByDescending(u => u.Id).FirstOrDefault();
            if (objtbl != null)
            {
                _double = (double)objtbl.AdvanceBalance;
            }
            return _double;
        }

        private bool StoreFeeBillTransactionClearance(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tbltransactionclearence _objtbltransactionclr = new tbltransactionclearence();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    int _TranMaxId = GetTableMaxId("tblview_transaction", "TransationId");
                    _objtbltransactionclr.TransationId = _TranMaxId;
                    if (FieldData[1] != "NULL")
                        _objtbltransactionclr.UserId = int.Parse(FieldData[1]);
                    if (FieldData[2] != "NULL")
                        _objtbltransactionclr.PaymentElementId = int.Parse(FieldData[2]);
                    if (FieldData[3] != "NULL")
                        _objtbltransactionclr.BillNo = FieldData[3];
                    if (FieldData[4] != "NULL")
                        _objtbltransactionclr.PaidDate = FormatUtility.GetDate(FieldData[4]);
                    if (FieldData[5] != "NULL")
                        _objtbltransactionclr.Amount = double.Parse(FieldData[5]);
                    if (FieldData[6] != "NULL")
                        _objtbltransactionclr.AccountTo = int.Parse(FieldData[6]);
                    if (FieldData[7] != "NULL")
                        _objtbltransactionclr.AccountFrom = int.Parse(FieldData[7]);
                    if (FieldData[8] != "NULL")
                        _objtbltransactionclr.Type = FieldData[8];
                    if (FieldData[9] != "NULL")
                        _objtbltransactionclr.TransType = sbyte.Parse(FieldData[9]);
                    if (FieldData[10] != "NULL")
                        _objtbltransactionclr.Canceled = sbyte.Parse(FieldData[10]);
                    if (FieldData[11] != "NULL")
                        _objtbltransactionclr.BalanceAmount = double.Parse(FieldData[11]);
                    if (FieldData[12] != "NULL")
                        _objtbltransactionclr.RegularFee = sbyte.Parse(FieldData[12]);
                    if (FieldData[13] != "NULL")
                        _objtbltransactionclr.BatchId = int.Parse(FieldData[13]);
                    if (FieldData[14] != "NULL")
                        _objtbltransactionclr.ClassId = int.Parse(FieldData[14]);
                    if (FieldData[15] != "NULL")
                        _objtbltransactionclr.StudentName = FieldData[15];
                    if (FieldData[16] != "NULL")
                        _objtbltransactionclr.CollectedUser = FieldData[16];
                    if (FieldData[17] != "NULL")
                        _objtbltransactionclr.CollectionType = byte.Parse(FieldData[17]);
                    if (FieldData[18] != "NULL")
                        _objtbltransactionclr.FeeName = FieldData[18];
                    if (FieldData[19] != "NULL")
                        _objtbltransactionclr.PeriodName = FieldData[19];
                    if (FieldData[20] != "NULL")
                        _objtbltransactionclr.TempId = FieldData[20];
                    if (FieldData[21] != "NULL")
                        _objtbltransactionclr.PeriodId = int.Parse(FieldData[21]);
                    if (FieldData[22] != "NULL")
                        _objtbltransactionclr.FeeId = int.Parse(FieldData[22]);

                    tbltransactionclearence objtbltransactionclearences = null;
                    objtbltransactionclearences = db.tbltransactionclearences.Where(d => d.BillNo == _objtbltransactionclr.BillNo && d.FeeName == _objtbltransactionclr.FeeName && d.PeriodName == _objtbltransactionclr.PeriodName).FirstOrDefault();
                    if (objtbltransactionclearences == null)
                    {
                        db.tbltransactionclearences.AddObject(_objtbltransactionclr);
                        StoreUpdateLogs("tbltransactionclearence", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                        db.SaveChanges();
                    }
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }
            }

            return _valid;
        }

        private bool StoreFeeBillClearanceMaster(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tblfeebillclearence _objtblfeebill = new tblfeebillclearence();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    // _returnstr = GetStringFormat(_returnstr, _objtbl.Id.ToString());
                    int _FeeId = GetTableMaxId("tblfeebillclearence", "Id");
                    _objtblfeebill.Id = _FeeId;
                    if (FieldData[1] != "NULL")
                        _objtblfeebill.StudentID = FieldData[1];
                    if (FieldData[2] != "NULL")
                        _objtblfeebill.TotalAmount = double.Parse(FieldData[2]);
                    if (FieldData[3] != "NULL")
                        _objtblfeebill.Date = FormatUtility.GetDate(FieldData[3]);
                    if (FieldData[4] != "NULL")
                        _objtblfeebill.PaymentMode = FieldData[4];
                    if (FieldData[5] != "NULL")
                        _objtblfeebill.PaymentModeId = FieldData[5];
                    if (FieldData[6] != "NULL")
                        _objtblfeebill.BankName = FieldData[6];
                    if (FieldData[7] != "NULL")
                        _objtblfeebill.BillNo = FieldData[7];
                    if (FieldData[8] != "NULL")
                        _objtblfeebill.UserId = int.Parse(FieldData[8]);
                    if (FieldData[9] != "NULL")
                        _objtblfeebill.CreatedDateTime = FormatUtility.GetDate(FieldData[9]);
                    if (FieldData[10] != "NULL")
                        _objtblfeebill.Canceled = sbyte.Parse(FieldData[10]);
                    if (FieldData[11] != "NULL")
                        _objtblfeebill.Counter = int.Parse(FieldData[11]);
                    if (FieldData[12] != "NULL")
                        _objtblfeebill.AccYear = int.Parse(FieldData[12]);
                    if (FieldData[13] != "NULL")
                        _objtblfeebill.ClassID = int.Parse(FieldData[13]);
                    if (FieldData[14] != "NULL")
                        _objtblfeebill.StudentName = FieldData[14];
                    if (FieldData[15] != "NULL")
                        _objtblfeebill.RegularFee = sbyte.Parse(FieldData[15]);
                    if (FieldData[16] != "NULL")
                        _objtblfeebill.TempId = FieldData[16];
                    if (FieldData[17] != "NULL")
                        _objtblfeebill.OtherReference = FieldData[17];

                    tblfeebillclearence objtblfeebillclearence = null;
                    objtblfeebillclearence = db.tblfeebillclearences.Where(d => d.BillNo == _objtblfeebill.BillNo).FirstOrDefault();
                    if (objtblfeebillclearence == null)
                    {
                        db.tblfeebillclearences.AddObject(_objtblfeebill);
                        StoreUpdateLogs("tblfeebillclearence", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                        db.SaveChanges();
                    }
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }
            }

            return _valid;
        }

        private bool StoreFeeBillTransaction(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tbltransaction _objtbltransaction = new tbltransaction();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    int _TranMaxId = GetTableMaxId("tblview_transaction", "TransationId");
                    _objtbltransaction.TransationId = _TranMaxId;
                    if (FieldData[1] != "NULL")
                        _objtbltransaction.UserId = int.Parse(FieldData[1]);
                    if (FieldData[2] != "NULL")
                        _objtbltransaction.PaymentElementId = int.Parse(FieldData[2]);
                    if (FieldData[3] != "NULL")
                        _objtbltransaction.BillNo = FieldData[3];
                    if (FieldData[4] != "NULL")
                        _objtbltransaction.PaidDate = FormatUtility.GetDate(FieldData[4]);
                    if (FieldData[5] != "NULL")
                        _objtbltransaction.Amount = double.Parse(FieldData[5]);
                    if (FieldData[6] != "NULL")
                        _objtbltransaction.AccountTo = int.Parse(FieldData[6]);
                    if (FieldData[7] != "NULL")
                        _objtbltransaction.AccountFrom = int.Parse(FieldData[7]);
                    if (FieldData[8] != "NULL")
                        _objtbltransaction.Type = FieldData[8];
                    if (FieldData[9] != "NULL")
                        _objtbltransaction.TransType = sbyte.Parse(FieldData[9]);
                    if (FieldData[10] != "NULL")
                        _objtbltransaction.Canceled = sbyte.Parse(FieldData[10]);
                    if (FieldData[11] != "NULL")
                        _objtbltransaction.BalanceAmount = double.Parse(FieldData[11]);
                    if (FieldData[12] != "NULL")
                        _objtbltransaction.RegularFee = sbyte.Parse(FieldData[12]);
                    if (FieldData[13] != "NULL")
                        _objtbltransaction.BatchId = int.Parse(FieldData[13]);
                    if (FieldData[14] != "NULL")
                        _objtbltransaction.ClassId = int.Parse(FieldData[14]);
                    if (FieldData[15] != "NULL")
                        _objtbltransaction.StudentName = FieldData[15];
                    if (FieldData[16] != "NULL")
                        _objtbltransaction.CollectedUser = FieldData[16];
                    if (FieldData[17] != "NULL")
                        _objtbltransaction.CollectionType = sbyte.Parse(FieldData[17]);
                    if (FieldData[18] != "NULL")
                        _objtbltransaction.FeeName = FieldData[18];
                    if (FieldData[19] != "NULL")
                        _objtbltransaction.PeriodName = FieldData[19];
                    if (FieldData[20] != "NULL")
                        _objtbltransaction.TempId = FieldData[20];
                    if (FieldData[21] != "NULL")
                        _objtbltransaction.PeriodId = int.Parse(FieldData[21]);
                    if (FieldData[22] != "NULL")
                        _objtbltransaction.FeeId = int.Parse(FieldData[22]);

                    tbltransaction objtbltransaction = null;
                    objtbltransaction = db.tbltransactions.Where(d => d.BillNo == _objtbltransaction.BillNo && d.FeeName == _objtbltransaction.FeeName && d.PeriodName == _objtbltransaction.PeriodName).FirstOrDefault();
                    if (objtbltransaction == null)
                    {
                        db.tbltransactions.AddObject(_objtbltransaction);
                        StoreUpdateLogs("tbltransaction", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                        db.SaveChanges();
                    }
                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }
            }

            return _valid;
        }

        private bool StoreFeeBillMaster(int Id, string TransactionType, DateTime ChangeDate, int ColValue, string FieldStr, int DeviceId)
        {
            bool _valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                try
                {
                    tblfeebill _objtblfeebill = new tblfeebill();
                    string[] FieldData = FieldStr.Split(new string[] { "$s$" }, StringSplitOptions.None);
                    // _returnstr = GetStringFormat(_returnstr, _objtbl.Id.ToString());
                    int _FeeId = GetTableMaxId("tblview_feebill", "TransationId");
                    _objtblfeebill.Id = _FeeId;
                    if (FieldData[1] != "NULL")
                        _objtblfeebill.StudentID = FieldData[1];
                    if (FieldData[2] != "NULL")
                        _objtblfeebill.TotalAmount = double.Parse(FieldData[2]);
                    if (FieldData[3] != "NULL")
                        _objtblfeebill.Date = FormatUtility.GetDate(FieldData[3]);
                    if (FieldData[4] != "NULL")
                        _objtblfeebill.PaymentMode = FieldData[4];
                    if (FieldData[5] != "NULL")
                        _objtblfeebill.PaymentModeId = FieldData[5];
                    if (FieldData[6] != "NULL")
                        _objtblfeebill.BankName = FieldData[6];
                    if (FieldData[7] != "NULL")
                        _objtblfeebill.BillNo = FieldData[7];
                    if (FieldData[8] != "NULL")
                        _objtblfeebill.UserId = int.Parse(FieldData[8]);
                    if (FieldData[9] != "NULL")
                        _objtblfeebill.CreatedDateTime = FormatUtility.GetDate(FieldData[9]);
                    if (FieldData[10] != "NULL")
                        _objtblfeebill.Canceled = sbyte.Parse(FieldData[10]);
                    if (FieldData[11] != "NULL")
                        _objtblfeebill.Counter = int.Parse(FieldData[11]);
                    if (FieldData[12] != "NULL")
                        _objtblfeebill.AccYear = int.Parse(FieldData[12]);
                    if (FieldData[13] != "NULL")
                        _objtblfeebill.ClassID = int.Parse(FieldData[13]);
                    if (FieldData[14] != "NULL")
                        _objtblfeebill.StudentName = FieldData[14];
                    if (FieldData[15] != "NULL")
                        _objtblfeebill.RegularFee = sbyte.Parse(FieldData[15]);
                    if (FieldData[16] != "NULL")
                        _objtblfeebill.TempId = FieldData[16];
                    if (FieldData[17] != "NULL")
                        _objtblfeebill.OtherReference = FieldData[17];

                    tblfeebill objtblfeebill = null;
                    objtblfeebill = db.tblfeebills.Where(d => d.BillNo == _objtblfeebill.BillNo).FirstOrDefault();
                    if (objtblfeebill == null)
                    {
                        StoreUpdateLogs("tblfeebill", "Id", ColValue, TransactionType, ChangeDate, 1, FieldStr, "Table Updated", DeviceId, db);
                        db.tblfeebills.AddObject(_objtblfeebill);
                        db.SaveChanges();
                    }

                    _valid = true;
                }
                catch
                {
                    _valid = false;
                }
            }
            
            return _valid;
        }

        public int GetTableMaxId(string _TableName, string _Field)
        {
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + "";

            using (MySqlConnection objConnection = new MySqlConnection(ADOConnection))
            {
                objConnection.Open();
                using (MySqlCommand objCommand = new MySqlCommand())
                {
                    objCommand.Connection = objConnection;
                    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.CommandText = sql;
                    using (MySqlDataReader objReader = objCommand.ExecuteReader())
                    {
                        if (objReader.HasRows)
                            if (objReader.Read())
                            {
                                int.TryParse(objReader.GetValue(0).ToString(), out Id);
                            }
                    }
                }
            }

            Id = Id + 1;
            return Id;
        }

        public tbl_holidayconfig[] Get_tbl_holidayconfig_List(long SyncDate, out int count)
        {
            tbl_holidayconfig[] lst_tbl_holidayconfig = null;
            count = 0;
            long syncdate = SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from st in db.tblholidayconfigs
                                  orderby st.SyncDate ascending
                                  select st);
                count = queryCount.Count();

                var query = (from cls in db.tblholidayconfigs                                                                                       
                             where cls.SyncDate >= syncdate
                             orderby cls.SyncDate ascending
                             select new { cls.Id, cls.Day, cls.Status, cls.StaffStatus, cls.SyncDate}).ToList();
                if (query.Count() == 0)
                    return lst_tbl_holidayconfig;

                List<tbl_holidayconfig> objLtDetails = new List<tbl_holidayconfig>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        tbl_holidayconfig obj_holidayconfig = new tbl_holidayconfig();
                        obj_holidayconfig.Id = obj.Id;
                        obj_holidayconfig.day = obj.Day;                        
                        if (obj.Status != null)
                            obj_holidayconfig.StudentStatus = (int)obj.Status;
                        if (obj.StaffStatus != null)
                            obj_holidayconfig.StudentStatus= (int)obj.StaffStatus;
                        if (obj.SyncDate != null)
                            obj_holidayconfig.SyncDate = (long)obj.SyncDate;

                        objLtDetails.Add(obj_holidayconfig);
                    }
                }

                if (objLtDetails.Count() == 0)
                    return lst_tbl_holidayconfig;

                lst_tbl_holidayconfig = new tbl_holidayconfig[objLtDetails.Count];
                int i = 0;
                foreach (tbl_holidayconfig obj in objLtDetails)
                {
                    lst_tbl_holidayconfig[i] = obj;
                    i++;
                }
            }
            return lst_tbl_holidayconfig;
        }

        public tbl_holidays[] Get_tbl_holidays_List(long SyncDate, out int count)
        {
            tbl_holidays[] lst_tbl_holidays = null;
            count = 0;
            long syncdate = SyncDate;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                var queryCount = (from st in db.tblholidays
                                  orderby st.SyncDate ascending
                                  select st);
                count = queryCount.Count();

                var query = (from cls in db.tblholidays
                             join date in db.tblmasterdates on (int)cls.dateId equals date.Id
                             where cls.SyncDate >= syncdate 
                             orderby cls.SyncDate ascending
                             select new { cls.Id, Date=date.date, cls.Type, cls.Class_Id,cls.Desc,cls.GroupId, cls.SyncDate }).ToList();
                if (query.Count() == 0)
                    return lst_tbl_holidays;

                List<tbl_holidays> objLtDetails = new List<tbl_holidays>();
                foreach (var obj in query)
                {
                    if (obj.SyncDate > syncdate)
                    {
                        tbl_holidays obj_tblholidays = new tbl_holidays();
                        obj_tblholidays.Id = obj.Id;
                        obj_tblholidays.Date = (FormatUtility.GetDateOnlyString(DateTime.Parse(obj.Date.ToString()))).ToString();//obj.Date. ToString();
                        obj_tblholidays.Type = obj.Type;
                        obj_tblholidays.Desc = obj.Desc;
                        if (obj.Class_Id != null)
                            obj_tblholidays.ClassID = (int)obj.Class_Id;
                        if (obj.GroupId != null)
                            obj_tblholidays.GroupID = (int)obj.GroupId;
                        if (obj.SyncDate != null)
                            obj_tblholidays.SyncDate = (long)obj.SyncDate;

                        objLtDetails.Add(obj_tblholidays);
                    }
                }

                if (objLtDetails.Count() == 0)
                    return lst_tbl_holidays;

                lst_tbl_holidays = new tbl_holidays[objLtDetails.Count];
                int i = 0;
                foreach (tbl_holidays obj in objLtDetails)
                {
                    lst_tbl_holidays[i] = obj;
                    i++;
                }
            }
            return lst_tbl_holidays;
        }

        public string GetParentName(string phoneNumber)
        {
            string parentName = string.Empty;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {

                var student = (from std in db.tblstudents
                               where std.OfficePhNo.Equals(phoneNumber.Trim())
                               select new { std.GardianName }).FirstOrDefault();
                if (student != null)
                    parentName = student.GardianName;
            }

            return parentName;
        }

        public bool ActivateDevice(tbldevice device)
        {
            bool valid = false;
            using (WinErdbEntities db = new WinErdbEntities(EntityConnection))
            {
                tbldevice objtbldevice = null;
                objtbldevice = db.tbldevices.Where(d => d.Id == device.Id).FirstOrDefault();
                if (objtbldevice == null)
                    objtbldevice = db.tbldevices.Where(d => d.DeviceUniqueId == device.DeviceUniqueId).FirstOrDefault();
                if (objtbldevice != null)
                {
                    objtbldevice.IsActive = device.IsActive;
                    valid = true;
                }

                db.SaveChanges();
            }

            return valid;
        }
    }
}