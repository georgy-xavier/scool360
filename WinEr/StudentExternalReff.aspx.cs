using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class StudentExternalReff : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_PreInit(Object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
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
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(822))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    AddClassToDropDownClass();
                    Load_StudentDetails(DropDownClass.SelectedValue);
                }
            }
        }



        private void AddClassToDropDownClass()
        {
            DropDownClass.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    DropDownClass.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                DropDownClass.Items.Add(li);
            }
            DropDownClass.SelectedIndex = 0;

        }



        private void Load_StudentDetails(string ClassId)
        {
            Lbl_msg.Text = "";
            string sql = "SELECT tblstudent.Id as StudentId,tblstudent.StudentName,tblstudentclassmap.RollNo,tblexternalreff.Id from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id LEFT OUTER JOIN  tblexternalreff ON tblstudent.Id=tblexternalreff.UserId AND tblexternalreff.UserType='STUDENT' WHERE tblstudent.`Status`=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + DropDownClass.SelectedValue + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet!=null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Students.Columns[0].Visible = true;
                Grd_Students.DataSource = MydataSet;
                Grd_Students.DataBind();
                Grd_Students.Columns[0].Visible = false;
                LoadReffNumber();
            }
            else
            {
                Grd_Students.DataSource = null;
                Grd_Students.DataBind();
                Lbl_msg.Text = "No Students found"; 
            }
        }



        private void LoadReffNumber()
        {
            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                string _studentId = gv.Cells[0].Text;
                string ReffNumber = GetStudentReffNo(_studentId);
                Txt_ReferanceNumber.Text = ReffNumber;
            }
        }



        private string GetStudentReffNo(string _studentId)
        {
            string RffId="";
            string sql = "SELECT tblexternalreff.ExternalReffId FROM tblexternalreff WHERE tblexternalreff.UserType='STUDENT' AND tblexternalreff.UserId=" + _studentId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                RffId = MyReader.GetValue(0).ToString();   
            }
            return RffId;
        }



        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Lbl_msg.Text = "";
            try
            {
                string _outMSG = "";
                if (IsSavingPossible(out _outMSG))
                {
                    string _msg = "";
                    MyStudMang.CreateTansationDb();
                    foreach (GridViewRow gv in Grd_Students.Rows)
                    {

                        TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                        string _studentId = gv.Cells[0].Text;
                        string _Id = gv.Cells[2].Text;
                        string _studentName = gv.Cells[3].Text;
                        string ReffNumber = Txt_ReferanceNumber.Text;
                        if (ReffNumber != "")
                        {
                            if (_Id.Trim() == "" || _Id.Trim() == "&nbsp;")
                            {
                                _Id = GetStudentReffId();
                            }
                            if (_Id != "0")
                            {
                                StoreStudentRefferanceNumber(_studentId, ReffNumber, _studentName, _Id);
                            }
                            else
                            {
                                _msg = "Student Id Limit Exceeded";
                                break;

                            }
                        }
                    }
                    MyStudMang.EndSucessTansationDb();

                    Load_StudentDetails(DropDownClass.SelectedValue);
                    Lbl_msg.Text = "Successfully Saved. " + _msg;
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Reference Number Saved", "Student RF Reference Number Saved", 1);
                }
                else
                {
                    Lbl_msg.Text = _outMSG;
                }
            }
            catch
            {
                MyStudMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while Saving. Try later";
            }
        }

        private bool IsSavingPossible(out string _outMSG)
        {
            bool _valid = true;
            _outMSG = "";

            foreach (GridViewRow gv in Grd_Students.Rows)
            {
                TextBox Txt_ReferanceNumber = (TextBox)gv.FindControl("Txt_ReferanceNumber");
                string _Id = gv.Cells[2].Text;
                string ReffNumber = Txt_ReferanceNumber.Text;
                if (ReffNumber != "")
                {
                    if (_Id.Trim() == "" || _Id.Trim() == "&nbsp;")
                    {
                        _Id = "0";
                    }
                    if (IsReferanceNumber_AlreadyEntered(_Id, ReffNumber))
                    {
                        _outMSG = "Reff Number : " + ReffNumber+" already assigned";
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
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
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


        private string GetStudentReffId()
        {
            int finalId = 0;

            int _lowervalue = int.Parse(GetStudentAttend_ConfigValue("Student ID lower limit"));
            int _uppervalue = int.Parse(GetStudentAttend_ConfigValue("Student ID upper limit"));

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
            string sql = "SELECT COUNT(tblexternalreff.Id) FROM tblexternalreff WHERE tblexternalreff.ISActive=1 AND tblexternalreff.UserType='STUDENT' AND tblexternalreff.Id='" + _Idvalue + "'";
            if (MyStudMang.m_TransationDb != null)
            {
                MyReader = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
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




        public string GetStudentAttend_ConfigValue(string _Type)
        {
            OdbcDataReader newreader1;
            string _value = "0";
            string sql = "SELECT tblstudentattdconfig.Value FROM tblstudentattdconfig WHERE tblstudentattdconfig.Name='" + _Type + "'";
            if (MyStudMang.m_TransationDb != null)
            {
                newreader1 = MyStudMang.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                newreader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            }
            if (newreader1.HasRows)
            {
                _value = newreader1.GetValue(0).ToString();
            }
            return _value;
        }

        private void StoreStudentRefferanceNumber(string _studentId, string ReffNumber, string _studentName, string _Id)
        {

            string sql = "DELETE FROM tblexternalreff WHERE Id='" + _Id + "' AND UserType='STUDENT'";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);

             sql = "INSERT INTO tblexternalreff (ExternalReffId,UserId,UserName,UserType,Id) VALUES('" + ReffNumber + "'," + _studentId + ",'" + _studentName + "','STUDENT'," + _Id + ")";
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }


        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentExternalReff.aspx");
        }

        protected void DropDownClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_StudentDetails(DropDownClass.SelectedValue);
        }

        protected void Grd_Students_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Id = Grd_Students.SelectedRow.Cells[2].Text.ToString();
            lbl_Id.Text = Id;
            MPE_SingleDelete.Show();
        }

        private void RemoveStudentRFId(string Id)
        {
            string sql = "DELETE FROM tblexternalreff WHERE UserType='STUDENT' AND Id=" + Id;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }

        protected void Btn_Yes1_Click(object sender, EventArgs e)
        {
            string Id = lbl_Id.Text;
            try
            {
                MyStudMang.CreateTansationDb();
                if (Id.Trim() != "" && Id.Trim() != "&nbsp;")
                {
                    RemoveStudentRFId(Id);
                }
                MyStudMang.EndSucessTansationDb();
                Load_StudentDetails(DropDownClass.SelectedValue);
                Lbl_msg.Text = "Successfully Removed";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Reference Number Deleted", "Student RF Reference Number Deleted", 1);
            }
            catch
            {
                MyStudMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while removing. Try later";
            }
        }

        protected void Btn_RemoveAll_Click(object sender, EventArgs e)
        {
            MPE_DeleteAll.Show();
        }

        protected void Btn_YesAll_Click(object sender, EventArgs e)
        {
            try
            {
                MyStudMang.CreateTansationDb();
                foreach (GridViewRow gv in Grd_Students.Rows)
                {
                    if (gv.Cells[2].Text.ToString().Trim() != "" && gv.Cells[2].Text.ToString().Trim() != "&nbsp;")
                    {
                        RemoveStudentRFId(gv.Cells[2].Text.ToString());
                    }
                    
                }
                
                MyStudMang.EndSucessTansationDb();
                Load_StudentDetails(DropDownClass.SelectedValue);
                Lbl_msg.Text = "Successfully Removed";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Student Reference Number Deleted", "Student RF Reference Number Deleted", 1);
            }
            catch
            {
                MyStudMang.EndFailTansationDb();
                Lbl_msg.Text = "Error while removing. Try later";
            }
        }

       
    }
}
