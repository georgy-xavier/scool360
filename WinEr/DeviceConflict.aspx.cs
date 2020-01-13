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

namespace WinEr
{
    public partial class DeviceConflict : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(3011))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadConflits();
                }
            }
        }

        private void LoadConflits()
        {
            lbl_errormsg.Text = "";
            DataSet MydataSet = GetDeviceDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_Conflict.Columns[0].Visible = true;
                Grid_Conflict.DataSource = MydataSet;
                Grid_Conflict.DataBind();
                Grid_Conflict.Columns[0].Visible = false;

            }
            else
            {
                Grid_Conflict.DataSource = null;
                Grid_Conflict.DataBind();
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
            dt.Columns.Add("ConflictHead");
            dt.Columns.Add("DeviceType");
            dt.Columns.Add("DeviceName");
            dt.Columns.Add("ConflictDate");


            string sql = "SELECT tbldeviceuploadconflicts.Id,tbldeviceuploadconflicts.TableName,tbldeviceuploadconflicts.Conflict_Column,tbldeviceuploadconflicts.ConflictId,tbldeviceuploadconflicts.UpdateDate,tbldeviceuploadconflicts.RowDataServer,tbldeviceuploadconflicts.RowDataDevice,tbldevice.DeviceName,tbldevice.DeviceType FROM tbldeviceuploadconflicts INNER JOIN tbldevice ON tbldevice.Id=tbldeviceuploadconflicts.DeviceId";
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
                    string ConflictHead = GetConflictHead(MyReader.GetValue(1).ToString(), MyReader.GetValue(5).ToString(), MyReader.GetValue(6).ToString());
                    dr["ConflictHead"] = ConflictHead;
                    dr["DeviceType"] = DeviceType;
                    dr["DeviceName"] =  MyReader.GetValue(7).ToString();
                    dr["ConflictDate"] = MyReader.GetValue(4).ToString();
 
                    dataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }



            return dataset;
        }

        private string GetConflictHead(string TableName, string RowDataServer, string RowDataDevice)
        {
//            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
//new System.Web.Script.Serialization.JavaScriptSerializer();
//            RegisterationDetails objRegisterationDetails = oSerializer.Deserialize<RegisterationDetails>(json);
            string _ConflictHead = "";

            switch (TableName)
            {
                case "tblfeestudent":
                    _ConflictHead = "Fee Schedule";

                    break;
            }

            return _ConflictHead;
        }

        protected void Grid_Conflict_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }


    }
}
