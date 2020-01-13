using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class StaffExternalReff : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_PreInit(Object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            else
            {
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

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(81))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    Load_Details();
                }
            }

        }

        private void Load_Details()
        {
            Lbl_msg.Text = "";
            string sql = "SELECT DISTINCT t.`Id` as StaffId,t.`SurName`, t.`UserName`,r.`RoleName`,tblexternalreff.Id  FROM tbluser t  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId` LEFT OUTER JOIN  tblexternalreff ON t.`Id`=tblexternalreff.UserId AND tblexternalreff.UserType='STAFF' where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
            MydataSet = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Staff.Columns[0].Visible = true;
                Grd_Staff.DataSource = MydataSet;
                Grd_Staff.DataBind();
                Grd_Staff.Columns[0].Visible = false;
                LoadReffNumber();
            }
            else
            {
                Grd_Staff.DataSource = null;
                Grd_Staff.DataBind();
                Lbl_msg.Text = "No Students found in the Class";
            }
        }

        private void LoadReffNumber()
        {
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                string _staffId = gv.Cells[0].Text;
                string ReffNumber = GetStaffReffNo(_staffId);
                Txt_ReferanceNumber.Text = ReffNumber;
            }
        }


        private string GetStaffReffNo(string _staffId)
        {
            string RffId = "";
            string sql = "SELECT tblexternalreff.ExternalReffId FROM tblexternalreff WHERE tblexternalreff.UserType='STAFF' AND tblexternalreff.UserId=" + _staffId;
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                RffId = MyReader.GetValue(0).ToString();
            }
            return RffId;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {

            try
            {
                string _outMSG = "";
                if (IsSavingPossible(out _outMSG))
                {
                    string _msg = "";
                    MyStaffMang.CreateTansationDb();
                    foreach (GridViewRow gv in Grd_Staff.Rows)
                    {

                        TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                        string _staffId = gv.Cells[0].Text;
                        string _Id = gv.Cells[1].Text;
                        string _staffName = gv.Cells[2].Text;
                        string ReffNumber = Txt_ReferanceNumber.Text;
                        if (ReffNumber != "")
                        {

                            if (_Id.Trim() == "" || _Id.Trim() == "&nbsp;")
                            {
                                _Id = GetStaffReffId();
                            }
                            if (_Id != "0")
                            {
                                StoreStaffRefferanceNumber(_staffId, ReffNumber, _staffName, _Id);
                            }
                            else
                            {
                                _msg = "Staff Id Limit Exceeded";
                                break;

                            }
                        }

                    }
                    MyStaffMang.EndSucessTansationDb();

                    Load_Details();
                    Lbl_msg.Text = "Successfully Saved. " + _msg;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff Reference Number Saved", "Staff RF Reference Number Saved", 1);
                }
                else
                {
                    Lbl_msg.Text = _outMSG;
                }
            }
            catch
            {
                MyStaffMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while Saving. Try later";
            }

        }


        private bool IsSavingPossible(out string _outMSG)
        {
            bool _valid = true;
            _outMSG = "";

            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                string _Id = gv.Cells[1].Text;
                string ReffNumber = Txt_ReferanceNumber.Text;
                if (ReffNumber != "")
                {
                    if (_Id.Trim() == "" || _Id.Trim() == "&nbsp;")
                    {
                        _Id = "0";
                    }
                    if (IsReferanceNumber_AlreadyEntered(_Id, ReffNumber))
                    {
                        _outMSG = "Reff Number : " + ReffNumber + " already assigned";
                        _valid = false;
                        break;
                    }
                }
            }
            return _valid;
        }

        private bool IsReferanceNumber_AlreadyEntered(string _Id, string ReffNumber)
        {
            bool _valid = false;

            string sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.ExternalReffId='" + ReffNumber + "' AND tblexternalreff.Id<>" + _Id;
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _valid = true;
                }
            }

            return _valid;
        }

        private string GetStaffReffId()
        {
            int finalId = 0;

            int _lowervalue = int.Parse(GetStaffAttend_ConfigValue("Staff Id lowerlimit"));
            int _uppervalue = int.Parse(GetStaffAttend_ConfigValue("Staff Id upperlimit"));
            while (ReffId_INUse(_lowervalue))
            {
                _lowervalue++;
            }

            if (_lowervalue <= _uppervalue)
            {
                finalId = _lowervalue;
            }

            return finalId.ToString();
        }


        private bool ReffId_INUse(int _Idvalue)
        {
            bool _valid = false;
            int _count = 0;
            string sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.ISActive=1 AND tblexternalreff.UserType='STAFF' AND tblexternalreff.Id='" + _Idvalue + "'";
            if (MyStaffMang.m_TransationDb != null)
            {
                MyReader = MyStaffMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            }
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }






        public string GetStaffAttend_ConfigValue(string _Type)
        {
            OdbcDataReader newreader1;
            string _value = "0";
            string sql = "SELECT tblstaffattdconfig.Value FROM tblstaffattdconfig WHERE tblstaffattdconfig.Name='" + _Type + "'";
            if (MyStaffMang.m_TransationDb != null)
            {
                newreader1 = MyStaffMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }
        private void StoreStaffRefferanceNumber(string _staffId, string ReffNumber, string _staffName, string _Id)
        {
            string sql = "DELETE FROM tblexternalreff WHERE Id='" + _Id + "' AND UserType='STAFF'";
            MyStaffMang.m_TransationDb.ExecuteQuery(sql);

            sql = "REPLACE INTO tblexternalreff (ExternalReffId,UserId,UserName,UserType,Id) VALUES('" + ReffNumber + "'," + _staffId + ",'" + _staffName + "','STAFF'," + _Id + ")";
            MyStaffMang.m_TransationDb.ExecuteQuery(sql);
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("StaffExternalReff.aspx");
        }

        protected void Grd_Staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Id = Grd_Staff.SelectedRow.Cells[1].Text.ToString();

            lbl_Id.Text = Id;
            MPE_SingleDelete.Show();
          
 
        }


        protected void Btn_Yes1_Click(object sender, EventArgs e)
        {
            string Id = lbl_Id.Text;
            try
            {
                MyStaffMang.CreateTansationDb();
                if (Id.Trim() != "" && Id.Trim() != "&nbsp;")
                {
                    RemoveStaffRFId(Id);
                }
               
                MyStaffMang.EndSucessTansationDb();
                Load_Details();
                Lbl_msg.Text = "Successfully Removed";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff Reference Number Removed", "Staff RF Reference Number Removed", 1);
            }
            catch
            {
                MyStaffMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while removing. Try later";
            }
        }

        private void RemoveStaffRFId(string Id)
        {
            string sql = "DELETE FROM tblexternalreff WHERE UserType='STAFF' AND Id=" + Id;
            MyStaffMang.m_TransationDb.ExecuteQuery(sql);
        }


       
        protected void Btn_RemoveAll_Click(object sender, EventArgs e)
        {
            MPE_DeleteAll.Show();
        }

        protected void Btn_YesAll_Click(object sender, EventArgs e)
        {
            try
            {
                MyStaffMang.CreateTansationDb();
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    if (gv.Cells[1].Text.ToString().Trim() != "" && gv.Cells[1].Text.ToString().Trim() != "&nbsp;")
                    {
                        RemoveStaffRFId(gv.Cells[1].Text.ToString());
                    }

                }

                MyStaffMang.EndSucessTansationDb();
                Load_Details();
                Lbl_msg.Text = "Successfully Removed";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Staff Reference Number Removed", "Staff RF Reference Number Removed", 1);
            }
            catch
            {
                MyStaffMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while removing. Try later";
            }
        }

    }
}
