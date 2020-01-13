using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class RFreaderRegisteration : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(821))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadRegisteredRFreader();
                    Load_Drp();
                }
            }
        }

        private void Load_Drp()
        {
            string _Diff_Equip_IN_Out = MyAttendance.GetStudentAttend_GeneralConfigValue("Need Diff Equipment for In and Out Time of student");
            if (_Diff_Equip_IN_Out == "YES")
            {
                Drp_Time.Items.Clear();
                Drp_Time.Items.Add(new ListItem("InTime", "0"));
                Drp_Time.Items.Add(new ListItem("OutTime", "1"));
                Drp_Time.Items.Add(new ListItem("Both", "2"));
                Drp_Time.SelectedValue = "2";

                Drp_EditTime.Items.Clear();
                Drp_EditTime.Items.Add(new ListItem("InTime", "0"));
                Drp_EditTime.Items.Add(new ListItem("OutTime", "1"));
                Drp_EditTime.Items.Add(new ListItem("Both", "2"));
                Drp_EditTime.SelectedValue = "2";
            }
            else
            {
                Drp_Time.Items.Clear();
                Drp_Time.Items.Add(new ListItem("Both", "2"));
                Drp_Time.SelectedValue = "2";

                Drp_EditTime.Items.Clear();
                Drp_EditTime.Items.Add(new ListItem("Both", "2"));
                Drp_EditTime.SelectedValue = "2";
            }
        }


        private void LoadRegisteredRFreader()
        {
            lbl_errormsg.Text = "";
            DataSet MydataSet = GetRegisteredDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Registered.DataSource = MydataSet;
                Grid_Registered.DataBind();
            }
            else
            {
                Grid_Registered.DataSource = null;
                Grid_Registered.DataBind();
                lbl_errormsg.Text = "No RF-Reader has been registered";
            }
        }

        private DataSet GetRegisteredDataSet()
        {
            DataSet dataset = new DataSet();
            DataTable dt;
            DataRow dr;
            dataset.Tables.Add(new DataTable("Reqtbl"));
            dt = dataset.Tables["Reqtbl"];
            dt.Columns.Add("EquipmentId");
            dt.Columns.Add("Location");
            dt.Columns.Add("Description");
            dt.Columns.Add("UseType");
            dt.Columns.Add("User");
            dt.Columns.Add("Status");
            dt.Columns.Add("Device");

            string sql = "SELECT EquipmentId,Location,Description,UseType,User,`Status`,Device FROM tblexternalrfid";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    string UseType = MyReader.GetValue(3).ToString();
                    string UseTypeStr = "BOTH";
                    if (UseType == "0")
                    {
                        UseTypeStr = "InTime";
                    }
                    else if(UseType == "1")
                    {
                        UseTypeStr = "OutTime";
                    }
                    dr = dataset.Tables["Reqtbl"].NewRow();
                    dr["EquipmentId"] = MyReader.GetValue(0).ToString();
                    dr["Location"] = MyReader.GetValue(1).ToString();
                    dr["Description"] = MyReader.GetValue(2).ToString();
                    dr["UseType"] = UseTypeStr;
                    dr["User"] = MyReader.GetValue(4).ToString();
                    dr["Status"] = MyReader.GetValue(5).ToString();
                    dr["Device"] = MyReader.GetValue(6).ToString();
                    dataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }



            return dataset;
        }

        protected void Lnk_Unregistered_Click(object sender, EventArgs e)
        {
            ShowUnregistered();
        }

        protected void Img_Unregistered_Click(object sender, ImageClickEventArgs e)
        {
            ShowUnregistered();
        }

        private void ShowUnregistered()
        {
            Load_UnregisteredGrid();
            MPE_Unregistered.Show();
        }

        private void Load_UnregisteredGrid()
        {
            lbl_unregistered_msg.Text = "";
            string sql = "SELECT DISTINCT RFReaderID FROM tblexternalunregisteredrfid";
            DataSet MydataSet = MyConfigMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Unregisterered.DataSource = MydataSet;
                Grid_Unregisterered.DataBind();
            }
            else
            {
                Grid_Unregisterered.DataSource = null;
                Grid_Unregisterered.DataBind();
                lbl_unregistered_msg.Text = "No unregistered RF-Reader found";
            }
        }

        protected void Grid_Unregisterered_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string RFReaderID = Grid_Unregisterered.DataKeys[e.NewEditIndex].Values["RFReaderID"].ToString();
            RegisterRFReader(RFReaderID);
        }

        private void RegisterRFReader(string RFReaderID)
        {
            lbl_unregistered_msg.Text = "";
            string _msg = "";
            if (isRegistrationPossible(RFReaderID, out _msg))
            {
                MPE_EnterEFDetails.Show();
                Lbl_EquipmentId.Text = RFReaderID;
                Txt_Location.Text = "";
                Txt_Description.Text = "";
                lbl_Details_msg.Text = "";
                Txt_IP.Text = "";
                Txt_Port.Text = "";
            }
            else
            {
                lbl_unregistered_msg.Text = _msg;
                MPE_Unregistered.Show();
            }
        }

        private bool isRegistrationPossible(string RFReaderID, out string _msg)
        {
            _msg = "";
            bool _valid = true;
            if (RFReaderID.Trim() == "")
            {
                _valid = false;
                _msg = "No RF-Reader found";
            }
            else if (_IsRFReaderAlreadyRegistered(RFReaderID.Trim()))
            {
                _valid = false;
                _msg = "RF-Reader already registered";
            }


            return _valid;
        }

        private bool _IsRFReaderAlreadyRegistered(string RFReaderID)
        {
            bool valid = false;
            string sql = "SELECT COUNT( tblexternalrfid.Id) FROM tblexternalrfid WHERE tblexternalrfid.EquipmentId='" + RFReaderID + "'";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int count = 0;
                int.TryParse(MyReader.GetValue(0).ToString(),out count);
                if (count > 0)
                {
                    valid = true;
                }
            }
            return valid;
        }

        protected void Btn_Register_Click(object sender, EventArgs e)
        {
            lbl_Details_msg.Text = "";
            string _msg = "";
            if (Is_SavingPossible(out _msg))
            {
                try
                {
                    MyConfigMang.CreateTansationDb();

                    string sql = "INSERT INTO tblexternalrfid (EquipmentId,Location,Description,UseType,User,IP,Port,LastTransaction,Device) VALUES ('" + Lbl_EquipmentId.Text + "','" + Txt_Location.Text + "','" + Txt_Description.Text + "','" + Drp_Time.SelectedValue + "','" + Drp_User.SelectedItem.Text.ToUpperInvariant() + "','" + Txt_IP.Text + "','" + Txt_Port.Text + "','" + DateTime.Now.ToString("s") + "','"+Txt_DeviceModel.Text.Trim()+"')";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "DELETE FROM tblexternalunregisteredrfid WHERE RFReaderID='"+Lbl_EquipmentId.Text+"'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    MyConfigMang.EndSucessTansationDb();
                    Load_UnregisteredGrid();
                    LoadRegisteredRFreader();
                    MPE_Unregistered.Show();
                    lbl_unregistered_msg.Text = "Selected RF-Reader registered successfully";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "RF-Reader registered successfully", "RF-Reader: " + Lbl_EquipmentId.Text + " registered successfully", 1);
                }
                catch
                {
                    MyConfigMang.EndFailTansationDb();
                    MPE_EnterEFDetails.Show();
                    lbl_Details_msg.Text = "Error while registering. Try later";
                }
            }
            else
            {
                MPE_EnterEFDetails.Show();
                lbl_Details_msg.Text = _msg;
            }
        }

        private bool Is_SavingPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;
            if (Txt_Location.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter location details";
            }
            else if (Txt_Description.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter description";
            }
            else if (Txt_Description.Text.Length>245)
            {
                _valid = false;
                _msg = "Enter shorter description";
            }
            else if (Txt_IP.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter IP";
            }
            else if (Txt_Port.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter Port";
            }
            else if (Txt_DeviceModel.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter Device Model";
            }
            return _valid;
        }

        protected void Grid_Registered_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string EquipmentId = Grid_Registered.DataKeys[e.NewEditIndex].Values["EquipmentId"].ToString();
            EditRegisterRFReader(EquipmentId);
        }

        private void EditRegisterRFReader(string EquipmentId)
        {
            MPE_EditRFreader.Show();
            Lbl_EditRFReaderId.Text = EquipmentId;
            string Location="",Description="",Time="",User="",IP="",Port="",Device="";
            GetRFReaderDetails(EquipmentId, out Location, out Description, out Time, out User, out IP, out Port, out Device);

            Txt_EditLocation.Text = Location;
            Txt_Edit_Description.Text = Description;
            Txt_EditIP.Text = IP;
            Txt_EditPort.Text = Port;
            Txt_EditDeviceModel.Text = Device;

            if (User.ToUpperInvariant() == "STUDENT")
            {
                Drp_EditUser.SelectedValue = "0";
            }
            else if (User.ToUpperInvariant() == "STAFF")
            {
                Drp_EditUser.SelectedValue = "1";
            }
            else
            {
                Drp_EditUser.SelectedValue = "2";
            }

            string _Diff_Equip_IN_Out = MyAttendance.GetStudentAttend_GeneralConfigValue("Need Diff Equipment for In and Out Time of student");
            if (_Diff_Equip_IN_Out == "YES")
            {
                Drp_EditTime.SelectedValue = Time;
            }
            else
            {
                Drp_EditTime.SelectedValue = "2";
            }

            Lbl_EditRFreaderMSG.Text = "";
        }

        private void GetRFReaderDetails(string EquipmentId, out string Location, out string Description, out string Time, out string User, out string IP, out string Port, out string Device)
        {
            Location = "";
            Description = "";
            Time = "";
            User = "";
            IP = "";
            Port = "";
            Device = "";
            string sql = "SELECT Location,Description,UseType,`User`,IP,Port,Device FROM tblexternalrfid WHERE EquipmentId='" + EquipmentId + "'";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if(MyReader.HasRows)
            {
                Location = MyReader.GetValue(0).ToString();
                Description = MyReader.GetValue(1).ToString();
                Time = MyReader.GetValue(2).ToString();
                User = MyReader.GetValue(3).ToString();
                IP = MyReader.GetValue(4).ToString();
                Port = MyReader.GetValue(5).ToString();
                Device = MyReader.GetValue(6).ToString();
            }
        }

        protected void Btn_UpdateRFreader_Click(object sender, EventArgs e)
        {
            Lbl_EditRFreaderMSG.Text = "";
            string _msg = "";
            if (IsUpdationPossible(out _msg))
            {
                try
                {
                    string sql = "UPDATE tblexternalrfid SET Location='" + Txt_EditLocation.Text + "',Description='" + Txt_Edit_Description.Text + "',UseType='" + Drp_EditTime.SelectedValue + "',User='" + Drp_EditUser.SelectedItem.Text.ToUpperInvariant() + "',IP='" + Txt_EditIP.Text + "',Port='" + Txt_EditPort.Text + "',Device='"+Txt_EditDeviceModel.Text+"'  WHERE EquipmentId='" + Lbl_EditRFReaderId.Text + "'";
                    MyConfigMang.m_MysqlDb.ExecuteQuery(sql);

                    LoadRegisteredRFreader();
                    lbl_errormsg.Text = "RF-Reader updated successfully";
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "RF-Reader updated successfully", "RF-Reader: " + Lbl_EditRFReaderId.Text + " updated successfully", 1);
                }
                catch
                {

                    MPE_EditRFreader.Show();
                    Lbl_EditRFreaderMSG.Text = "Error while updating. Try later";
                }
            }
            else
            {
                MPE_EditRFreader.Show();
                Lbl_EditRFreaderMSG.Text = _msg;
            }
        }

        private bool IsUpdationPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;
            if (Txt_EditLocation.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter location details";
            }
            else if (Txt_Edit_Description.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter description";
            }
            else if (Txt_Edit_Description.Text.Length > 245)
            {
                _valid = false;
                _msg = "Enter shorter description";
            }
            else if (Txt_EditPort.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter Port";
            }
            else if (Txt_EditIP.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter IP";
            }
            else if (Txt_EditDeviceModel.Text.Trim() == "")
            {
                _valid = false;
                _msg = "Enter Device Model";
            }
            return _valid;
        }

        protected void Btn_DeleteRFreader_Click(object sender, EventArgs e)
        {
            Lbl_EditRFreaderMSG.Text = "";

            try
            {
                string sql = "DELETE FROM tblexternalrfid WHERE EquipmentId='" + Lbl_EditRFReaderId.Text + "'";
                MyConfigMang.m_MysqlDb.ExecuteQuery(sql);

                LoadRegisteredRFreader();
                lbl_errormsg.Text = "RF-Reader deleted successfully";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "RF-Reader deleted successfully", "RF-Reader: " + Lbl_EditRFReaderId.Text + " deleted successfully", 1);
            }
            catch
            {

                MPE_EditRFreader.Show();
                Lbl_EditRFreaderMSG.Text = "Error while deleting. Try later";
            }

        }

    }
}
