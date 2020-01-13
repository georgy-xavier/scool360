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
    public partial class DynamicCeritiMaster : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private OdbcDataReader MyReader;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(896))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                
                if (!IsPostBack)
                {

                    //some initlization
                    LoadInitials();
                    LoadDynamicFields();
                }
            }
        }

        private void LoadInitials()
        {
            LoadCertificateHead();
            Load_Seperators();
            Load_CertificateBody(false);
        }

        private void LoadDynamicFields()
        {
            int dmaxid=0;
            
            string sql = "select MAX(Id) from tblcertificatesperator";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                dmaxid = int.Parse(MyReader.GetValue(0).ToString());
            }

            sql = "select tblstudentfieldconfi.DbColumanName, tblstudentfieldconfi.FieldName, tblstudentfieldconfi.DataTypeId, tblstudentfieldconfi.MaxLength, tblstudentfieldconfi.Ismandatory FROM tblstudentfieldconfi";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                    {

                        if (dynamicallreadyadded(MyReader.GetValue(1).ToString()))
                        {
                            int maxid = dmaxid + 1;
                            sql = "insert into tblcertificatesperator(Id,Description,SeperatorCode,SeperatorType,ActionValue,IsActive) values(" + maxid + ",'" + MyReader.GetValue(1).ToString() + "','" + "($" + MyReader.GetValue(1).ToString() + "$)" + "'," + "1" + ",'" + "Select " + MyReader.GetValue(0) + " from tblstudentdetails WHERE tblstudentdetails.StudentId=($StudentId$)" + "'," + "1)";
                            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                            dmaxid++;
                        }
                }
                MyReader.Close();
            }


        }
        private bool dynamicallreadyadded(string name)
        {
            string sql="select Description from tblcertificatesperator where Description='"+name+"'";
            DataSet dynamicdata = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (dynamicdata.Tables[0].Rows.Count> 0)
            {

                return false;
            }
               
                return true;
            
        }

        private void LoadCertificateHead()
        {
            Drp_CertificateType.Items.Clear();
            string sql = "SELECT DISTINCT tblcertificatemaster.Id,tblcertificatemaster.CertificateHead FROM tblcertificatemaster";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_CertificateType.Items.Add(new ListItem("Select Type", "-1"));
                while (MyReader.Read())
                {
                    Drp_CertificateType.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
                }
                Drp_CertificateType.SelectedIndex = 0;
            }
            else
            {
                Drp_CertificateType.Items.Add(new ListItem("No Type Found", "-1"));
            }
        }

        private void Load_Seperators()
        {
            string innerhtml = "<table cellspacing=\"10\">";
            
            DataSet dynamicFieldDetails = MyStudMang.GetCuestomFields();
           
            string sql = "SELECT DISTINCT tblcertificatesperator.Description,tblcertificatesperator.SeperatorCode FROM tblcertificatesperator WHERE tblcertificatesperator.IsActive=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    innerhtml = innerhtml + "<tr style=\"height:20px\"><td align=\"right\">" + MyReader.GetValue(0).ToString() + " : </td> <td  align=\"left\" class=\"new\"> " + MyReader.GetValue(1).ToString() + " </td></tr> ";
                }
            }
            


            innerhtml = innerhtml + "</table>";
            this.Seperators.InnerHtml = innerhtml;
        }


        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            Panel_CertificateBody.Visible = false;
            if (Drp_CertificateType.SelectedValue != "-1")
            {
               
                Load_CertificateDetails(Drp_CertificateType.SelectedValue);
            }
        }

        private void Load_CertificateDetails(string CertificateHeadId)
        {

            string sql = "SELECT tblcertificatemaster.CertificateHead,tblcertificatemaster.HtmlContent FROM tblcertificatemaster WHERE tblcertificatemaster.Id=" + CertificateHeadId;
             MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
             if (MyReader.HasRows)
             {
                 Load_CertificateBody(true);
                
                 Txt_CertificateName.Text = MyReader.GetValue(0).ToString();
                 Ceretificate_Body.Content = MyReader.GetValue(1).ToString();
                 Btn_Delete.Visible = true;
                 Drp_CertificateType.SelectedValue = CertificateHeadId;
             }
            
        }

        private void Load_CertificateBody(bool _visibility)
        {
            lbl_msg.Text = "";
            Txt_CertificateName.Text = "";
            Ceretificate_Body.Content = "";
            Panel_CertificateBody.Visible = _visibility;
            Btn_Load.Enabled = !_visibility;
            Drp_CertificateType.Enabled = !_visibility;
            Btn_AddNew.Enabled = !_visibility;
            Drp_CertificateType.SelectedIndex = 0;
            Hd_New.Value = "0";
            Btn_Delete.Visible = false;
        }

        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            Load_CertificateBody(false);
        }

        protected void Btn_AddNew_Click(object sender, EventArgs e)
        {
            Load_CertificateBody(true);
            Hd_New.Value = "1";
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            string _msg="";
            if (IsSavingPossible(out _msg))
            {

                try
                {
                    if (Hd_New.Value == "1")
                    {
                        string sql = "INSERT INTO tblcertificatemaster (CertificateHead,HtmlContent) VALUES ('" + Txt_CertificateName.Text.Trim() + "','" + Ceretificate_Body.Content.Replace("'", "").Replace("\\", "") + "')";
                        MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        string sql = "UPDATE tblcertificatemaster SET CertificateHead='" + Txt_CertificateName.Text.Trim().Replace("'", "").Replace("\\", "") + "',HtmlContent='" + Ceretificate_Body.Content.Replace("'", "").Replace("\\", "") + "' WHERE Id=" + Drp_CertificateType.SelectedValue;
                        MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    }
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Manage Certificate Head", "Certificate:" + Drp_CertificateType.SelectedItem.Text + " Head saved", 1);
                    WC_MsgBox.ShowMssage("Successfully saved");
                    LoadInitials();
                }
                catch (Exception Ex)
                {
                    lbl_msg.Text = "Error while saving. Message : "+Ex.Message;
                }
            }
            else
            {
                lbl_msg.Text = _msg;
            }
        }

        private bool IsSavingPossible(out string _msg)
        {
            bool _valid = true;
            _msg = "";

            if (Txt_CertificateName.Text.Trim() == "")
            {
                _msg = "Please enter certificate name";
                _valid = false;
            }

            if (_valid)
            {
                if (Ceretificate_Body.Content.ToString().Trim() == "")
                {
                    _msg = "Please create certificate body";
                    _valid = false;
                }
            }

            return _valid;
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("DynamicCeritiMaster.aspx");
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Hd_New.Value != "1")
                {
                    string sql = "DELETE FROM tblcertificatemaster WHERE Id=" + Drp_CertificateType.SelectedValue;
                    MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Manage Certificate Head", ""+Drp_CertificateType.SelectedItem.Text+" Deleted", 1);
                    WC_MsgBox.ShowMssage("Successfully Deleted");
                    LoadInitials();
                }
                else
                {
                    lbl_msg.Text = "Deletion not possible ";
                }

            }
            catch (Exception Ex)
            {
                lbl_msg.Text = "Error while deleting. Message : " + Ex.Message;
            }
        }
    }
}
