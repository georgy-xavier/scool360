using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Text;
using System.Data;

namespace WinEr
{
    public partial class TCConfig : System.Web.UI.Page
    {       
        private KnowinUser MyUser;
        private ConfigManager MyConfiMang;
        private OdbcDataReader MyReader;
        private StudentManagerClass MyStudMang;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
           
            if (MyConfiMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(898))
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadInitials();
                    LoadDynamicFields();
                }
            }
        }

        protected void Btn_Update_Click(Object Sender, EventArgs e)
        {
            Lbl_Err.Text = "";
            Lbl_UpdationError.Text = "";

            StringBuilder TCFormat = new StringBuilder();

            string _Sql_TCFormat = "";

            TCFormat.Append(Editor_TCFormat.Content);
            if (HasFormat())
            {
                if (Editor_TCFormat.Content != "")
                    _Sql_TCFormat = "Update tbltcformat  set TCFormat='" + Editor_TCFormat.Content.ToString().Replace("'"," " ).Replace("\\" ," " )+ "'";
                else
                    _Sql_TCFormat = "Delete From tbltcformat ";
            }
            else
            {
                _Sql_TCFormat = "insert into tbltcformat(TCFormat) Values('" + Editor_TCFormat.Content.ToString().Replace("'", " ").Replace("\\", " ") + "')";
            }
         
            MyConfiMang.m_MysqlDb.ExecuteQuery(_Sql_TCFormat);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "TC Config", "New TC Format Updated", 1);
            Lbl_UpdationError.Text = "New TC Format Updated";
        }

        private bool HasFormat()
        {
            string sql = "select tbltcformat.TCFormat from tbltcformat ";
            OdbcDataReader MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)

                return true;

            else

                return false;
        }

        private void LoadInitials()
        {
            LoadTCFormat();
            Load_Seperators();
        }

        private void LoadTCFormat()
        {
            Lbl_Err.Text = "";
            string _ReportName = "TCFormat";

            string sql = "select tbltcformat.TCFormat from tbltcformat";
            OdbcDataReader MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                Editor_TCFormat.Content = MyReader.GetValue(0).ToString();
            }
            else
            {
                MyReader = MyConfiMang.GetReportFormat(_ReportName);

                if (MyReader.HasRows)
                {
                    Editor_TCFormat.Content = MyReader.GetValue(0).ToString();
                    Lbl_Err.Text = "Default TC Format";
                }
                else
                {
                    Lbl_Err.Text = "Default TC format is not available. Contact with Configuration Manager";
                }
            }

        }

        private void Load_Seperators()
        {
            string innerhtml = "<table >";
            string sql = "select tbltcseperators.FormatName, tbltcseperators.Seperator from tbltcseperators ";
            OdbcDataReader MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    innerhtml = innerhtml + "<tr style=\"height:20px\"><td align=\"right\">" + MyReader.GetValue(0).ToString() + " </td> <td style=\"color:red\"> : </td> <td  align=\"left\" class=\"new\"> " + MyReader.GetValue(1).ToString() + " </td></tr> ";
                }
            }
            innerhtml = innerhtml + "</table>";

            this.Sepretor.InnerHtml = innerhtml;
        }
        private void LoadDynamicFields()
        {
            int dmaxid = 0;

            string sql = "select MAX(Id) from tbltcseperators";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                dmaxid = int.Parse(MyReader.GetValue(0).ToString());
            }

            sql = "select tblstudentfieldconfi.DbColumanName, tblstudentfieldconfi.FieldName, tblstudentfieldconfi.DataTypeId, tblstudentfieldconfi.MaxLength, tblstudentfieldconfi.Ismandatory FROM tblstudentfieldconfi";
            MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {

                    if (dynamicallreadyadded(MyReader.GetValue(1).ToString()))
                    {
                        int maxid = dmaxid + 1;
                        sql = "insert into tbltcseperators(Id,FormatName,Seperator,ActionValue,IsDynamic) values(" + maxid + ",'" + MyReader.GetValue(1).ToString() + "','" + "($" + MyReader.GetValue(1).ToString() + "$)" + "','" + "Select " + MyReader.GetValue(0) + " from tblstudentdetails WHERE tblstudentdetails.StudentId=($StudentId$)" + "',"+ 1 +")";
                        MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
                        dmaxid++;
                    }
                }
                MyReader.Close();
            }


        }
        private bool dynamicallreadyadded(string name)
        {
            string sql = "select FormatName from tbltcseperators where FormatName='" + name + "'";
            DataSet dynamicdata = MyConfiMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dynamicdata.Tables[0].Rows.Count > 0)
            {

                return false;
            }

            return true;

        }

    }
}
