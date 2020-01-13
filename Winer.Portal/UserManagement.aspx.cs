using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.Script.Serialization;
using System.Configuration;
using WinBase;
using System.Web.UI.HtmlControls;


namespace Winer.Portal
{
    public partial class UserManagement : System.Web.UI.Page
    {
        private OdbcDataReader MyReader = null;
        private KnowinEncryption Myencryption;
        private KnowinUser MyUser;
        public MysqlClass _MysqlObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PortalUserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["PortalUserObj"];
            Myencryption = new KnowinEncryption();

        }
        protected void Btn_create_Click(object sender, EventArgs e)
        {

            int flag = 0;
            int _userId;
            if (Txt_loginName.Value.Trim() == "")
            {
                lblmsguser.Text = "Login name can't be empty";
               
                Txt_loginName.Focus();
            }
            else if (Txt_surname.Value.Trim() == "")
            {
                lblmsguser.Text = "Login surname can't be empty";
                
                Txt_surname.Focus();
            }
            else if (Txt_pwd.Value.Trim() == "")
            {
                lblmsguser.Text = "Login Password can't be empty";
               
                Txt_pwd.Focus();

            }

            else if (Txt_pwd.Value.Length < 4 || Txt_pwd.Value.Length > 20)
            {
                lblmsguser.Text = "Login Password length should be minimum 4 to  maximum 20";
                
                Txt_pwd.Focus();
            }


            else if (Txt_pwd.Value != Txt_conpwd.Value)
            {
                lblmsguser.Text = "Password mismatch";
               
                Txt_pwd.Focus();
            }
            else if (Drp_roll.Value == "0")
            {
                lblmsguser.Text = "Please select a role.";
                
                Txt_pwd.Focus();
            }
            else
            {
                bool status = false;

                if (!CheckIfUserExitIn(Txt_loginName.Value.ToString(), out status))
                {
                   
                        if (!status)
                        {
                            InserIntoUserTbl(Txt_loginName.Value.ToString(), Txt_surname.Value.ToString(), Txt_pwd.Value.ToString(), Txt_email.Value.ToString(), Drp_roll.Value.ToString());
                            _userId = getUserId(Txt_loginName.Value.ToString());
                            
                            lblmsguser.Text = "User Created Successfuly";
                            //MyUser.StoreUserAction("User Created", "User : " + Txt_surname.Text + " has been created");
                            clearall();
                            // MPE_MessageBox.Show();
                        }
                        else if (status)
                        {
                            Hdn_Pwd.Value = Txt_pwd.Value;
                            lblmsguser.Text = "User name already exist,Do you want to continue?!";
                            //Mpe_ExtMsg.Show();
                        }


                   
                }
                else
                {
                    lblmsguser.Text = "User Name Already Exit";
                    Txt_loginName.Value = "";
                    Txt_loginName.Focus();
                    //MPE_MessageBox.Show();
                }

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "modalmessage();", true);

        }
        private void clearall()
        {
            Txt_conpwd.Value = "";
            Txt_email.Value = "";
            Txt_loginName.Value = "";
            Txt_pwd.Value = "";
            Txt_surname.Value = "";
            Hdn_Pwd.Value = "";
        }
        private int getUserId(string _userName)
        {
            int _userId = 0;
            string sql = "select tbluser.Id from tbluser where tbluser.UserName='" + _userName + "' and tbluser.organizationid=" + MyUser.m_orgId + "";
            MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _userId = int.Parse(MyReader.GetValue(0).ToString());
            }
            MyReader.Close();
            return _userId;
        }
        private void InserIntoUserTbl(string _loginName, string SurName, string _pwd, string Email, string _rollId)
        {
            string _encriptedpwd;
            _encriptedpwd = Myencryption.Encrypt(_pwd);
            DateTime _date = System.DateTime.Now;

            string sql = "insert into tbluser (name,username,status,actionrights,createtime,GmailId,password,CanLogin,organizationid) values ('" + SurName + "','" + _loginName + "',1," + _rollId + ",' " + _date.ToString("s") + "','" + Email + "','" + _encriptedpwd + "',1,"+ MyUser.m_orgId +")";
            MyUser.m_MysqlDb.ExecuteQuery(sql);


        }
        private bool CheckIfUserExitIn(string _username, out bool delstatus)
        {
            bool flag = false;
            delstatus = false;
            int status = 0;

            MysqlClass _MysqlObj = new MysqlClass();

            string sql = "select tbluser.Id,status from tbluser where  tbluser.username='" + _username + "' and tbluser.organizationid="+ MyUser.m_orgId +" ";
            MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(1).ToString(), out status);
                if (status == 1)
                {
                    flag = true;
                }
                else
                {
                    delstatus = true;
                  //  Hdn_UserID.Value = MyReader.GetValue(0).ToString();
                }
            }
            MyReader.Close();
            
            return flag;


        }
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {

            clearall();
            lblmsguser.Text = "";

        }
        //protected void Btn_Yes_Click(object sender, EventArgs e)
        //{


        //}
        
    }
}