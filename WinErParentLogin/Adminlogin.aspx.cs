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
using WinBase;
using System.Collections.Generic;
using System.Data.Odbc;

namespace WinErParentLogin
{
    public partial class Adminlogin : System.Web.UI.Page
    {
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (WinerUtlity.NeedCentrelDB())
            {
                if (Request.QueryString[WinerConstants.SchoolId] != null)
                {
                    int intSchoolId;
                    if (int.TryParse(Request.QueryString[WinerConstants.SchoolId], out intSchoolId))
                    {
                        objSchool = WinerUtlity.GetSchoolObject(intSchoolId);
                        if (objSchool != null)
                            Session[WinerConstants.SessionSchool] = objSchool;
                    }

                }
                if (Session[WinerConstants.SessionSchool] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];

            }
            if (!IsPostBack)
            { 
            
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));

            KnowinUser UserObj;
            string _meassageError;
            Session["RsltQry"] = null;
            if (Session["UserObj"] == null)
            {

                UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
                Session["UserObj"] = UserObj;
            }
            else
            {

                UserObj = (KnowinUser)Session["UserObj"];
                if (UserObj.UserName != Login1.UserName.Trim())
                {

                    UserObj = new KnowinUser(WinerUtlity.GetConnectionString(objSchool), FilePath, WinerUtlity.GetRelativeFilePath(objSchool));
                    Session["UserObj"] = UserObj;
                }
            }

            if (UserObj.HaveActionRignt(3018))
            {
                if (UserObj.LoginUser(Login1.UserName.ToString(), Login1.Password.ToString(), 0, out _meassageError))
                {
                    UserObj.RecordLogin(Login1.UserName.ToString());
                    //UserObj.RecordLogin(Client_Login.UserName.ToString());
                    if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
                    {
                        SetApplicationUserObject(UserObj);
                    }

                    Load_StudentSelect();
                    // Response.Redirect(_meassageError);
                }
                else
                {
                    Login1.FailureText = _meassageError;
                }
            }
            else
                Login1.FailureText = "User doesn't have right to login";


            
        }

        private void Load_StudentSelect()
        {
            Load_DrpClass();
            Load_DrpStudent();
            MPE_SelectStudent.Show();
        }

        private void Load_DrpClass()
        {
            KnowinUser MyUser = (KnowinUser)Session["UserObj"];
            Drp_Class.Items.Clear();
            DataSet MydataSet = MyUser.MyAssociatedClass();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("Select Class", "-1");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }

        private void Load_DrpStudent()
        {
            KnowinUser MyUser = (KnowinUser)Session["UserObj"];
            Drp_Student.Items.Clear();
            string sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            OdbcDataReader MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("Select Student", "-1");
                Drp_Class.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Student.Items.Add(li);
                }

            }
            else
            {
                ListItem li = new ListItem("No students found", "-1");
                Drp_Student.Items.Add(li);
            }
            MyReader.Close();
        }

        # region Application Object Area Fee
        private void SetApplicationUserObject(KnowinUser UserObj)
        {
            List<UserInfoClass> _UaerList = new List<UserInfoClass>();
            if (Application["UserList"] == null)
            {
                Application.Lock();
                Application["UserList"] = _UaerList;
                Application.UnLock();
            }
            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
            _UaerList = (List<UserInfoClass>)Application["UserList"];
            int _UserIndex = WinerUtlity.GetUserIndex(UserObj.UserId, SchoolId, _UaerList);
            if (_UserIndex != -1)
            {
                _UaerList[_UserIndex].UserName = UserObj.UserName;
                _UaerList[_UserIndex].Allowlogin = true;
                _UaerList[_UserIndex].LoginTime = DateTime.Now;
                _UaerList[_UserIndex].NeedSessionOut = false;
                _UaerList[_UserIndex].IP = Request.UserHostAddress;
                _UaerList[_UserIndex].MachineName = Request.UserHostName;
            }
            else
            {
                UserInfoClass UserInfoClassObj = new UserInfoClass();
                UserInfoClassObj.SchoolId = SchoolId;
                UserInfoClassObj.UserId = UserObj.UserId;
                UserInfoClassObj.UserName = UserObj.UserName;
                UserInfoClassObj.Allowlogin = true;
                UserInfoClassObj.LoginTime = DateTime.Now;
                UserInfoClassObj.NeedSessionOut = false;
                UserInfoClassObj.IP = Request.UserHostAddress;
                UserInfoClassObj.MachineName = Request.UserHostName;
                _UaerList.Add(UserInfoClassObj);

            }
            Application.Lock();
            Application["UserList"] = _UaerList;
            Application.UnLock();
        }

        # endregion

        protected void Btn_Select_Click(object sender, EventArgs e)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(objSchool));

            ParentLogin MyParent = new ParentLogin(_mysqlObj, objSchool);
            int StudentId = int.Parse(Drp_Student.SelectedValue.ToString());
            ParentInfoClass MyParentObj;
            string _Message = "";
            //string BatchName = "";
            if (MyParent.ParentLoginSuccess(StudentId, out _Message, out MyParentObj))
            {

                MyParent.RecordLogin(Drp_Student.SelectedItem.Text);
                MyParentObj.SchoolObject = objSchool;
                //MyParentObj.SetBatchDetails(MyParent.GetBatchId(out BatchName), BatchName);
                Session["MyParentObj"] = MyParentObj;
                DBLogClass _DblogObj = new DBLogClass(_mysqlObj);

                _DblogObj.LogToDb(MyParentObj.ParentName, "Admin_Parent_UserLoggin ", "Admin logged into parent login portal of Student " + MyParentObj.StudentName , 1, 2);
                _mysqlObj = null;
                Session["ACtivationStatus"] = 0;
                Response.Redirect("Home.aspx");
            }
            else
            {

                Lbl_msg.Text = _Message;
                MPE_SelectStudent.Show();
            }
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyParentObj = null;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_DrpStudent();
            MPE_SelectStudent.Show();
        }

    }
}
