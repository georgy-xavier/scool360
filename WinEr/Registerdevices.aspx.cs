using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class Registerdevices : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
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
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(3010))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadDeviceDetails();

                }
            }
        }

        private void LoadDeviceDetails()
        {
            lbl_errormsg.Text = "";
            DataSet MydataSet = GetDeviceDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Registered.Columns[0].Visible = true;
                Grid_Registered.Columns[1].Visible = true;
                Grid_Registered.DataSource = MydataSet;
                Grid_Registered.DataBind();
                Grid_Registered.Columns[0].Visible = false;
                Grid_Registered.Columns[1].Visible = false;
                Grid_Registered.Columns[3].Visible = false;
                Grid_Registered.Columns[7].Visible = false;
                Grid_Registered.Columns[9].Visible = false;
            }
            else
            {
                Grid_Registered.DataSource = null;
                Grid_Registered.DataBind();
                lbl_errormsg.Text = "No device found";
            }
        }

        private DataSet GetDeviceDataSet()
        {
            DataSet dataset = new DataSet();
            DataTable dt;
            DataRow dr;
            dataset.Tables.Add(new DataTable("Reqtbl"));
            dt = dataset.Tables["Reqtbl"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ISACTIVE");
            dt.Columns.Add("DeviceUniqueId");
            dt.Columns.Add("DeviceName");
            dt.Columns.Add("AddedUser");
            dt.Columns.Add("Registration Date");
            dt.Columns.Add("LastLogin");
            dt.Columns.Add("Status");
            dt.Columns.Add("ActivatedBy");
            dt.Columns.Add("DeviceType");

            string sql = "SELECT Id,DeviceUniqueId,DeviceName,AddedUser,`Registration Date`,LastLogin,IsActive,ActivatedBy,DeviceType FROM tbldevice";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    string DeviceType = MyReader.GetValue(8).ToString();
                    if (DeviceType.Trim() == "")
                    {
                        DeviceType = "UNKOWN DEVICE TYPE";
                    }

                    dr = dataset.Tables["Reqtbl"].NewRow();
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    dr["DeviceUniqueId"] = MyReader.GetValue(1).ToString();
                    dr["DeviceName"] = MyReader.GetValue(2).ToString();
                    dr["AddedUser"] = MyReader.GetValue(3).ToString();
                    DateTime _Registrationdate = new DateTime();
                    DateTime _LastLogindate = new DateTime();
                    DateTime _temp = new DateTime(2001,1,1);
                    DateTime.TryParse(MyReader.GetValue(4).ToString(), out _Registrationdate);
                    if (_Registrationdate != _temp)
                    {
                         dr["Registration Date"] = General.GerFormatedDatVal(_Registrationdate);
                    }
                    DateTime.TryParse(MyReader.GetValue(5).ToString(), out _LastLogindate);
                    if (_LastLogindate > _temp)
                    {
                        dr["LastLogin"] = General.GerFormatedDatVal(_LastLogindate);
                    }
                    int _status = 0;
                    int.TryParse(MyReader.GetValue(6).ToString().Trim(),out _status);
                    dr["ISACTIVE"] = MyReader.GetValue(6).ToString().Trim();
                    if (_status>0)
                    {
                        dr["Status"] = "ACTIVE";
                    }
                    else
                    {
                        dr["Status"] = "INACTIVE";
                    }
                    dr["ActivatedBy"] = MyReader.GetValue(7).ToString();
                    dr["DeviceType"] = DeviceType;
                    dataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }



            return dataset;
        }

        protected void Grid_Registered_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string Id = Grid_Registered.DataKeys[e.NewEditIndex].Values["Id"].ToString();
            string ISACTIVE = Grid_Registered.DataKeys[e.NewEditIndex].Values["ISACTIVE"].ToString();
            EditRegisterDevice(Id, ISACTIVE);
        }

        private void EditRegisterDevice(string Id, string ISACTIVE)
        {
            MPE_EditDevice.Show();
            Hd_Id.Value = Id;
            Hd_ISactive.Value = ISACTIVE;
            Drp_DeviceStatus.SelectedValue = ISACTIVE;
            Lbl_EditDeviceMSG.Text = "";
        }

        protected void Btn_UpdateDevice_Click(object sender, EventArgs e)
        {
            if (Drp_DeviceStatus.SelectedValue != Hd_ISactive.Value)
            {
                string sql = "UPDATE tbldevice SET IsActive=" + Drp_DeviceStatus.SelectedValue + " WHERE Id=" + Hd_Id.Value;
                MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Register Device", "Device details updated", 1);
                Lbl_EditDeviceMSG.Text = "Successfully updated";
                LoadDeviceDetails();
            }
            else
            {
                Lbl_EditDeviceMSG.Text = "You have selected same status for updating.";
            }
            MPE_EditDevice.Show();
        }


    }
}
