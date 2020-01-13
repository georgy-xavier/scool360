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
using System;

namespace Winer.Portal
{
    public partial class ChangePassword : System.Web.UI.Page
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
            }
        protected void Btn_create_Click(object sender, EventArgs e)
        {
            string _meassageError;
            if (IsCorrectOldPassword(MyUser.UserName.ToString(), Txt_pwd.Value.ToString(), out _meassageError))
            {
                if (Txt_NewPwd.Value != "")
                {
                    if (Txt_NewPwd.Value == Txt_conpwd.Value)
                    {
                        KnowinEncryption Myencryption = new KnowinEncryption();
                        string _encriptedpwd;
                        _encriptedpwd = Myencryption.Encrypt(Txt_conpwd.Value.ToString());

                        string sql = "update tbluser set password='" + _encriptedpwd + "' where tbluser.Id=" + MyUser.UserId;
                        MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
                        Lbl_msg.Text = "Password Updated";
                       
                       
                        
                        Txt_NewPwd.Visible = true;
                        Txt_conpwd.Visible = true;
                        
                        Btn_create.Visible = true;
                        Btn_Cancel.Visible = true;
                        Txt_pwd.Visible = true;

                        Txt_pwd.Value = "";
                        Txt_pwd.Focus();

                    }
                    else
                    {
                        Lbl_msg.Text = "password Mismatch";
                       
                    }
                }
                else
                {
                    Lbl_msg.Text = "Please Enter New Password";
                   
                    Txt_NewPwd.Focus();
                }

            }
            else
            {
                Lbl_msg.Text = "Please Enter the Correct OLD password";
              
                Txt_pwd.Focus();


                Txt_pwd.Value = "";
                Txt_pwd.Focus();
            }

        }

        private bool IsCorrectOldPassword(string sUsername, string sPassword, out string _message)
        {
            string sql;
            bool _valide;
            KnowinEncryption m_MyEncrypt = new KnowinEncryption();
            try
            {

                _valide = false;
                sql = "SELECT password,Id  FROM tbluser where username='" + sUsername + "' And CanLogin=1";


                MyReader = MyUser.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {

                    MyReader.Read();
                    if (m_MyEncrypt == null)
                    {
                        m_MyEncrypt = new KnowinEncryption();
                    }


                    if (sPassword == m_MyEncrypt.Decrypt(MyReader.GetValue(0).ToString()))
                    {

                        _valide = true;
                        _message = "sucess";
                    }
                    else
                    {
                        _message = "Invalid password";
                    }

                }
                else
                {
                    _message = "Invalid username";
                }


                MyReader.Close();

            }
            catch (Exception _ex)
            {

                _message = "Unable To Login";
                _valide = false;
            }
            return _valide;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Txt_NewPwd.Value = "";
            Txt_conpwd.Value = "";
            Txt_pwd.Value = "";
            Txt_pwd.Focus();
        }
    }
}