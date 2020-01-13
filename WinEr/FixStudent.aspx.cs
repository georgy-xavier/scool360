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
    public partial class FixStudent : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private StudentManagerClass MyStudentMang;
        private KnowinUser MyUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudentMang = MyUser.GetStudentObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (MyStudentMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }

            else
            {

                if (!IsPostBack)
                {

                }
            }
        }

        protected void Btn_Fix_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            if (Txt_Symbol.Text != "")
            {
                try
                {
                    FixStudentNAme();
                }
                catch (Exception ex)
                {
                    lbl_error.Text = "Error : " + ex.Message;
                }
            }
            else
            {
                lbl_error.Text = "Enter symbol";
            }
            Txt_Symbol.Text = "";
        }

        private void FixStudentNAme()
        {
            string _symbol = Txt_Symbol.Text.Trim();
            if (_symbol == "")
            {
                _symbol = " ";
            }
            string sql = "SELECT Id,StudentName FROM tblstudent WHERE StudentName LIKE '_" + _symbol + "%'";
            OdbcDataReader MyReader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int count = MyReader.RecordsAffected;
                while (MyReader.Read())
                {
                    string Id = MyReader.GetValue(0).ToString();
                    string StudentName = MyReader.GetValue(1).ToString();
                    string _newName = StudentName.Substring(2) + " " + StudentName.Substring(0, 1);
                    sql = "UPDATE tblstudent SET StudentName='" + _newName + "' WHERE Id=" + Id;
                    MyStudentMang.m_MysqlDb.ExecuteQuery(sql);

                }
                lbl_error.Text = count + " students name successfully fixed";
            }
            else
            {
                lbl_error.Text = "No student found";
            }
        }
    }

}
