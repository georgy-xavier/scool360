using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;

namespace WinEr
{
    public partial class StaffAttdConfig : System.Web.UI.Page
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
            else if (!MyUser.HaveActionRignt(623))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    LoadStafftConfig();

                }
            }
        }

        private void LoadStafftConfig()
        {
            string sql = "SELECT Id,Name,Value FROM tblstaffattdconfig";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    string Name = MyReader.GetValue(1).ToString();
                    string value = MyReader.GetValue(2).ToString();
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

                    if (Name == "Start OutTime")
                    {
                        Txt_StartOutTime.Text = value;
                    }

                    if (Name == "End OutTime")
                    {
                        Txt_EndOutTime.Text = value;
                    }

                    if (Name == "Staff Id lowerlimit")
                    {
                        Txt_staffid_lower.Text = value;
                    }

                    if (Name == "Staff Id upperlimit")
                    {
                        Txt_staffid_upper.Text = value;
                    }


                }
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            string _msg = "";
            if (IsSavingPossible(out _msg))
            {

                try
                {
                    MyConfigMang.CreateTansationDb();

                    string sql = "UPDATE tblstaffattdconfig SET Value='" + Txt_StartIntime.Text + "' WHERE Name='Start InTime'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstaffattdconfig SET Value='" + Txt_EndInTime.Text + "' WHERE Name='End InTime'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstaffattdconfig SET Value='" + Txt_LateInTime.Text + "' WHERE Name='Late InTime'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    string StartOutTime = Txt_StartOutTime.Text;
                    string EndOutTime = Txt_EndOutTime.Text;

                    sql = "UPDATE tblstaffattdconfig SET Value='" + StartOutTime + "' WHERE Name='Start OutTime'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstaffattdconfig SET Value='" + EndOutTime + "' WHERE Name='End OutTime'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);


                    sql = "UPDATE tblstaffattdconfig SET Value='" + Txt_staffid_lower.Text + "' WHERE Name='Staff Id lowerlimit'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);

                    sql = "UPDATE tblstaffattdconfig SET Value='" + Txt_staffid_upper.Text + "' WHERE Name='Staff Id upperlimit'";
                    MyConfigMang.m_TransationDb.ExecuteQuery(sql);


                    MyConfigMang.EndSucessTansationDb();
                    lbl_Errormsg.Text = "Saved Successfully";

                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff RF-config saved successfully", "Staff RF-config saved successfully", 1);

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
            else if (Txt_staffid_lower.Text.Trim() == "")
            {
                _msg = "Enter staff ID lower limit";
                valid = false;
            }
            else if (Txt_staffid_upper.Text.Trim() == "")
            {
                _msg = "Enter staff ID upper limit";
                valid = false;
            }
            else if (int.Parse(Txt_staffid_upper.Text) < int.Parse(Txt_staffid_lower.Text))
            {
                _msg = "Enter greater upper limit for staff ID";
                valid = false;
            }
            else if (TimeSpan.Parse(Txt_LateInTime.Text) < TimeSpan.Parse(Txt_StartIntime.Text))
            {
                _msg = "Enter greater late intime";
                valid = false;
            }
            else if (TimeSpan.Parse(Txt_EndInTime.Text) < TimeSpan.Parse(Txt_StartIntime.Text))
            {
                _msg = "Enter greater end intime";
                valid = false;
            }
            else if (Txt_StartOutTime.Text.Trim() == "")
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
            else if (!IsStaffIdPossible(int.Parse(Txt_staffid_upper.Text), int.Parse(Txt_staffid_lower.Text), out _msg))
            {
                valid = false;
            }
            return valid;
        }

        private bool IsStaffIdPossible(int UpperId, int LowerId, out string _msg)
        {
            bool _valid = true;
            _msg = "";
            string sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.UserType='STAFF' AND tblexternalreff.Id<" + LowerId;
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _msg = "Staff ID less than " + LowerId + " already present";
                    _valid = false;
                }

            }

            if (_valid)
            {
                sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.UserType='STAFF' AND tblexternalreff.Id>" + UpperId;
                MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    int _count = 0;
                    int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                    if (_count > 0)
                    {
                        _msg = "Staff ID greater than " + UpperId + " already present";
                        _valid = false;
                    }

                }
            }
            return _valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StaffAttdConfig.aspx");
        }
    }
}
