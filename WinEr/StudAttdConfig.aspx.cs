using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Net;
using System.Data;

namespace WinEr
{
    public partial class StudAttdConfig : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
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
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MyAttendance == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(623))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    AddParentGrpToDrpList();
                    Load_GeneralDetails();
                    LoadStudentConfig();
                    CheckStudentAttendance_TableFields();

                }
            }
        }

        private void Load_GeneralDetails()
        {
            Lbl_ErrorMasgForGroupDetails.Text = "";
            string sql = "SELECT Id,Name,Value FROM tblstudentattdconfig WHERE Name IN ('Student ID lower limit','Student ID upper limit','Need Diff Equipment for In and Out Time of student')";
            DataSet MydataSet = MyConfigMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    string Name = dr[1].ToString();
                    string value = dr[2].ToString();
                    if (Name == "Student ID lower limit")
                    {
                        Txt_stdid_lower.Text = value;
                    }
                    if (Name == "Student ID upper limit")
                    {
                        Txt_stdid_upper.Text = value;
                    }

                    if (Name == "Need Diff Equipment for In and Out Time of student")
                    {
                        if (value == "YES")
                        {
                            Rdb_needDiffEquipment.SelectedValue = "1";
                        }
                        else
                        {
                            Rdb_needDiffEquipment.SelectedValue = "0";
                        }
                    }
                }
            }
            else
            {
                lbl_Errormsg.Text = "No data Found";
            }
        }


        private void AddParentGrpToDrpList()
        {
            Drp_ParentGroup.Items.Clear();
            DataSet myDataset;
            myDataset = MyUser.MyAssociatedGroups();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_ParentGroup.Items.Add(li);

                }

            }
            else
            {

                ListItem li = new ListItem("No Groups", "-1");
                Drp_ParentGroup.Items.Add(li);

            }
            Drp_ParentGroup.SelectedIndex = 0;
        }



        private void LoadStudentConfig()
        {
            Lbl_ErrorMasgForGroupDetails.Text = "";
            lbl_Errormsg.Text = "";
            string sql = "SELECT Id,Name,Value FROM tblstudentattdconfig WHERE GroupId="+Drp_ParentGroup.SelectedValue;
            DataSet MydataSet = MyConfigMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    string Name = dr[1].ToString();
                    string value = dr[2].ToString();
                    if (Name == "Start InTime")
                    {
                        Txt_StartIntime.Text = value;
                    }

                    if (Name == "End InTime")
                    {
                        Txt_EndInTime.Text = value;
                    }

                    if (Name == "Late InTime")
                    {
                        Txt_LateInTime.Text = value;
                    }

                    if (Name == "Need OutTime")
                    {
                        if (value == "YES")
                        {
                            Rdb_needOOutTime.SelectedValue = "1";
                            Enable_NeedOutTime(true);
                        }
                        else
                        {
                            Rdb_needOOutTime.SelectedValue = "0";
                            Enable_NeedOutTime(false);
                        }
                    }

                    if (Name == "Start OutTime")
                    {
                        Txt_StartOutTime.Text = value;
                    }

                    if (Name == "End OutTime")
                    {
                        Txt_EndOutTime.Text = value;
                    }


                }
            }
            else
            {

                lbl_Errormsg.Text = "No data Found";

                if (Drp_ParentGroup.SelectedValue != "-1")
                {
                    try
                    {
                        MyAttendance.CreateTansationDb();
                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('Start InTime','00:00:00'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('End InTime','00:00:00'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('Late InTime','00:00:00'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('Need OutTime','NO'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('Start OutTime','00:00:00'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        sql = "INSERT INTO tblstudentattdconfig(Name,Value,GroupId) VALUES ('End OutTime','00:00:00'," + Drp_ParentGroup.SelectedValue + ")";
                        MyAttendance.m_TransationDb.ExecuteQuery(sql);

                        MyAttendance.EndSucessTansationDb();
                        LoadStudentConfig();
                    }
                    catch(Exception ex)
                    {
                        MyAttendance.EndFailTansationDb();
                        lbl_Errormsg.Text = "Error while inserting new group details. Message : "+ex.Message;
                    }
                }
            }
        }


        protected void Rdb_needOOutTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_needOOutTime.SelectedValue == "1")
            {
                Enable_NeedOutTime(true);
            }
            else
            {
                Enable_NeedOutTime(false);
            }
        }

        private void Enable_NeedOutTime(bool _boolvalue)
        {
            Txt_StartOutTime.Enabled = _boolvalue;
            MaskedEditExtender3.Enabled = _boolvalue;
            MaskedEditValidator3.Enabled = _boolvalue;

            Txt_EndOutTime.Enabled = _boolvalue;
            MaskedEditExtender4.Enabled = _boolvalue;
            MaskedEditValidator4.Enabled = _boolvalue;
        }




        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Lbl_ErrorMasgForGroupDetails.Text = "";
            string _msg = "";
            if (IsSavingPossible(out _msg))
            {

                try
                {
                    MyConfigMang.CreateTansationDb();

                    string NeedDiff_Equipment = "YES";

                    if (Rdb_needDiffEquipment.SelectedValue == "0")
                    {
                        NeedDiff_Equipment = "NO";
                    }

                    string sql = "UPDATE tblstudentattdconfig SET Value='" + NeedDiff_Equipment + "' WHERE Name='Need Diff Equipment for In and Out Time of student'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);


                    sql = "UPDATE tblstudentattdconfig SET Value='" + Txt_stdid_lower.Text + "' WHERE Name='Student ID lower limit'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstudentattdconfig SET Value='" + Txt_stdid_upper.Text + "' WHERE Name='Student ID upper limit'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);


                    MyConfigMang.EndSucessTansationDb();
                    lbl_Errormsg.Text = "Saved Successfully";

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student RF-config saved successfully", "Student RF-config saved successfully", 1);

                }
                catch
                {
                    MyConfigMang.EndFailTansationDb();
                    lbl_Errormsg.Text = "Error while saving. Try later";
                }


            }
            else
            {
                lbl_Errormsg.Text = _msg;
            }
        }


        private bool IsSavingPossible(out string _msg)
        {
            _msg = "";
            bool valid = true;
           if (Txt_stdid_lower.Text.Trim() == "")
            {
                _msg = "Enter student ID lower limit";
                valid = false;
            }
            else if (Txt_stdid_upper.Text.Trim() == "")
            {
                _msg = "Enter student ID upper limit";
                valid = false;
            }
            else if (int.Parse(Txt_stdid_upper.Text) < int.Parse(Txt_stdid_lower.Text))
            {
                _msg = "Enter greater upper limit for student ID";
                valid = false;
            }
            else if (!IsStudentIdPossible(int.Parse(Txt_stdid_upper.Text), int.Parse(Txt_stdid_lower.Text), out _msg))
            {
                valid = false;
            }


            return valid;
        }

        protected void Btn_SaveGroupConfigs_Click(object sender, EventArgs e)
        {
            Lbl_ErrorMasgForGroupDetails.Text = "";
            lbl_Errormsg.Text = "";
            string _msg = "";
            if (IsSavingPossibleForGroupDetails(out _msg))
            {

                try
                {
                    MyConfigMang.CreateTansationDb();

                    string sql = "UPDATE tblstudentattdconfig SET Value='" + Txt_StartIntime.Text + "' WHERE Name='Start InTime' AND GroupId="+Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstudentattdconfig SET Value='" + Txt_EndInTime.Text + "' WHERE Name='End InTime' AND GroupId=" + Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstudentattdconfig SET Value='" + Txt_LateInTime.Text + "' WHERE Name='Late InTime' AND GroupId=" + Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    string NeedOutTime = "YES";
                    string StartOutTime = Txt_StartOutTime.Text;
                    string EndOutTime = Txt_EndOutTime.Text;
                    if (Rdb_needOOutTime.SelectedValue == "0")
                    {
                        StartOutTime = "00:00:00";
                        EndOutTime = "00:00:00";
                        NeedOutTime = "NO";
                    }

                    sql = "UPDATE tblstudentattdconfig SET Value='" + NeedOutTime + "' WHERE Name='Need OutTime' AND GroupId=" + Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstudentattdconfig SET Value='" + StartOutTime + "' WHERE Name='Start OutTime' AND GroupId=" + Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstudentattdconfig SET Value='" + EndOutTime + "' WHERE Name='End OutTime' AND GroupId=" + Drp_ParentGroup.SelectedValue;
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);



                    MyConfigMang.EndSucessTansationDb();
                    Lbl_ErrorMasgForGroupDetails.Text = "Saved Successfully";

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student RF-config saved successfully", "Student RF-config for group " + Drp_ParentGroup.SelectedItem.Text + " saved successfully", 1);

                }
                catch
                {
                    MyConfigMang.EndFailTansationDb();
                    Lbl_ErrorMasgForGroupDetails.Text = "Error while saving. Try later";
                }


            }
            else
            {
                Lbl_ErrorMasgForGroupDetails.Text = _msg;
            }
        }




        private bool IsSavingPossibleForGroupDetails(out string _msg)
        {
            _msg = "";
            bool valid = true;
            if (Txt_StartIntime.Text.Trim() == "")
            {
                _msg = "Enter start intime";
                valid = false;
            }
            else if (Txt_EndInTime.Text.Trim() == "")
            {
                _msg = "Enter end intime";
                valid = false;
            }
            else if (Txt_LateInTime.Text.Trim() == "")
            {
                _msg = "Enter late intime";
                valid = false;
            }



            else if (TimeSpan.Parse(Txt_EndInTime.Text) < TimeSpan.Parse(Txt_StartIntime.Text))
            {
                _msg = "Enter greater end intime";
                valid = false;
            }
            else if (TimeSpan.Parse(Txt_LateInTime.Text) < TimeSpan.Parse(Txt_StartIntime.Text))
            {
                _msg = "Enter greater late intime";
                valid = false;
            }

            else if (Rdb_needOOutTime.SelectedValue == "1")
            {
                if (Txt_StartOutTime.Text.Trim() == "")
                {
                    _msg = "Enter start outtime";
                    valid = false;
                }
                else if (Txt_EndOutTime.Text.Trim() == "")
                {
                    _msg = "Enter end outtime";
                    valid = false;
                }
                else if (TimeSpan.Parse(Txt_EndOutTime.Text) < TimeSpan.Parse(Txt_StartOutTime.Text))
                {
                    _msg = "Enter greater end outtime";
                    valid = false;
                }
            }
          
            
            return valid;
        }

        private bool IsStudentIdPossible(int UpperId, int LowerId, out string _msg)
        {
            bool _valid=true;
            _msg = "";
            string sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.UserType='STUDENT' AND tblexternalreff.Id<" + LowerId;
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(MyReader.GetValue(0).ToString(),out _count);
                if (_count > 0)
                {
                    _msg = "Student ID less than " + LowerId + " already present"; 
                    _valid=false;
                }
            
            }

            if (_valid)
            {
                sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.UserType='STUDENT' AND tblexternalreff.Id>" + UpperId;
                MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int _count = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                    if (_count > 0)
                    {
                        _msg = "Student ID greater than " + UpperId + " already present";
                        _valid = false;
                    }

                }
            }
            return _valid;

        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudAttdConfig.aspx");
        }


        protected void Drp_ParentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentConfig();
        }

        #region Attendance Table Check

        private void CheckStudentAttendance_TableFields()
        {
            int BatchId = MyUser.CurrentBatchId;
            int StandardId = 0;
            try
            {
                MyAttendance.CreateTansationDb();
                string sql = "SELECT Id,Name FROM  tblstandard";
                OdbcDataReader m_MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {

                        StandardId = 0;
                        int.TryParse(m_MyReader.GetValue(0).ToString(), out StandardId);
                        if (StandardId > 0)
                        {
                            if (MyAttendance.AttendanceTables_Exits(StandardId.ToString(), BatchId))
                            {


                                string TableName = "tblattdstud_std" + StandardId + "yr" + BatchId.ToString();
                                if (!ISFeildExists_TAble(TableName, "InTime"))
                                {
                                    sql = "ALTER TABLE " + TableName + " ADD COLUMN  `InTime` varchar(11) DEFAULT NULL";
                                    MyAttendance.m_TransationDb.ExecuteQuery(sql);
                                }


                                if (!ISFeildExists_TAble(TableName, "OutTime"))
                                {
                                    sql = "ALTER TABLE " + TableName + " ADD COLUMN `OutTime` varchar(11) DEFAULT NULL";
                                    MyAttendance.m_TransationDb.ExecuteQuery(sql);
                                }

                                if (!ISFeildExists_TAble(TableName, "IsLate"))
                                {
                                    sql = "ALTER TABLE " + TableName + " ADD COLUMN `IsLate` tinyint(3) DEFAULT '0' COMMENT '1 means student is Late'";
                                    MyAttendance.m_TransationDb.ExecuteQuery(sql);
                                }

                                if (!ISFeildExists_TAble(TableName, "LateValue"))
                                {
                                    sql = "ALTER TABLE " + TableName + " ADD COLUMN `LateValue` varchar(11) DEFAULT NULL";
                                    MyAttendance.m_TransationDb.ExecuteQuery(sql);
                                }
                            }
                        }
                    }

                }

                MyAttendance.EndSucessTansationDb();
            }
            catch
            {
                MyAttendance.EndFailTansationDb();
            }
        }

        private bool ISFeildExists_TAble(string TableName, string FeildName)
        {
            bool valid = false;

            string sql = "SELECT column_name from information_schema.columns WHERE column_name LIKE '" + FeildName + "' AND table_name = '" + TableName + "';";
            MyReader = MyAttendance.m_TransationDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string ColumnName = MyReader.GetValue(0).ToString();
                if (ColumnName.ToLowerInvariant().ToString() == FeildName.ToLowerInvariant().ToString())
                {
                    valid = true;
                }
            }
            return valid;
        }

        # endregion

        
    }
}
