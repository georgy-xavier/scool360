using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Odbc;
using WinBase;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace WinEr
{
    public partial class CertficateManager : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("SearchStudent.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(830))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {
                   // string _MenuStr;
                   // _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                   // this.SubStudentMenu.InnerHtml = _MenuStr;
                  //  LoadpupilTopData();
                    //some initlization
                    LoadInitails();
                }
            }
        }

        //private void LoadpupilTopData()
        //{

        //  //  string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), MyUser.GetImageUrl("StudentImage", int.Parse(Session["StudId"].ToString())), int.Parse(Session["StudType"].ToString()));
        //    string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
     
        //    this.StudentTopStrip.InnerHtml = _Studstrip;
        //}

        private void LoadInitails()
        {
            LoadCertificateDrp();
        }

        private void LoadCertificateDrp()
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

        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            Session["CertificateData"] = null;
            Hd_txtno.Value = "0";
            Panel_Content.Visible = false;
            this.DivCertificateContent.InnerHtml = "";
            if (Drp_CertificateType.SelectedValue != "-1")
            {
                Load_CertificateDetails(Drp_CertificateType.SelectedValue);
            }
        }

        private void Load_CertificateDetails(string CertificateHeadId)
        {
            string _Template = "";
            string sql = "SELECT tblcertificatemaster.CertificateHead,tblcertificatemaster.HtmlContent FROM tblcertificatemaster WHERE tblcertificatemaster.Id=" + CertificateHeadId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                _Template = MyReader.GetValue(1).ToString();
               
            }


            if (_Template != "")
            {
                Panel_Content.Visible = true;
                this.DivCertificateContent.InnerHtml = GetParsedTemplate(_Template);
            }

        }





        private string GetParsedTemplate(string _Template)
        {
            string _ResultTEmplate = _Template;
            string sql = "SELECT Description,SeperatorCode,Seperatortype,ActionValue,SubAction FROM tblcertificatesperator WHERE IsActive=1";
            DataSet _mydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_mydataSet != null && _mydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _mydataSet.Tables[0].Rows)
                {
                    string _Seperator = dr[1].ToString();
                    string _Seperatortype = dr[2].ToString();
                    string _ActionValue = dr[3].ToString();
                    string _SubAction = dr[4].ToString();
                    if (_Template.Contains(_Seperator))
                    {
                        string _newReplacingData = GetReplaceData(_Seperatortype, _ActionValue, _SubAction);
                        _ResultTEmplate = _ResultTEmplate.Replace(_Seperator, _newReplacingData);
                    }

                }
            }

            return _ResultTEmplate;
        }
        private string GetReplaceData(string _Seperatortype, string _ActionValue, string _SubAction)
        {

            string _data = "";

            if (_Seperatortype == "1")
            {
                _data = ReturnQueryResult(_ActionValue); 
            }
            else if (_Seperatortype == "2")
            {
                _data =ReturnTextbox( _ActionValue);
            }
            else
            {
                _data = GetDirectAction(_ActionValue, _SubAction);
            }
            return _data;
        }

        private string ReturnTextbox(string _ActionValue)
        {
            string Txt= _ActionValue.Replace("($textbox$)","txt_"+getNEWtextboxnumber());
            return Txt;
        }

        private string getNEWtextboxnumber()
        {
            int _count = 0;
            int.TryParse(Hd_txtno.Value, out _count);
            _count = _count+1;
            Hd_txtno.Value = _count.ToString();
            return Hd_txtno.Value;
        }

        private string GetDirectAction(string _ActionValue, string _SubAction)
        {
            switch (_SubAction.ToUpperInvariant())
            {
                case "LOGO":
                    return GetLogo(_ActionValue);
                case "STUDENTIMAGE":
                    return GetStudentImage(_ActionValue);
                default:
                    return _ActionValue;
            }
        }

        private string GetStudentImage(string _ActionValue)
        {
            return _ActionValue.Replace("($logo$)", "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage");
        }

        private string GetLogo(string _ActionValue)
        {



            return _ActionValue.Replace("($logo$)", "Handler/ImageReturnHandler.ashx?id=1&type=Logo");


        }




        private string ReturnQueryResult(string _ActionValue)
        {
            string _result = "";
            _ActionValue = _ActionValue.Replace("($StudentId$)", Session["StudId"].ToString());
            _ActionValue = _ActionValue.Replace("($ClassId$)", MyStudMang.GetClassId(int.Parse(Session["StudId"].ToString())).ToString());
            _ActionValue = _ActionValue.Replace("($StandardId$)", MyStudMang.GetStandard(Session["StudId"].ToString()).ToString());
            _ActionValue = _ActionValue.Replace("($BatchId$)", MyUser.CurrentBatchId.ToString());

            string sql = _ActionValue;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _result = MyReader.GetValue(0).ToString();
            }
            return _result;
        }

        protected void Img_Print_Click(object sender, ImageClickEventArgs e)
        {
            int class_Id = 0;
            string student_name = "";
            string cls_name = "";
            
            class_Id = get_classid(int.Parse(Session["StudId"].ToString()),out student_name);
            cls_name = get_className(class_Id);
            if (this.DivCertificateContent.InnerHtml != "")
            {
                //KnowinEncryption My_Encryption = new KnowinEncryption();
                string _newData = GetTextBoxRemoved(this.DivCertificateContent.InnerHtml);
                Session["CertificateData"] = _newData;
                //string _Encrypt_STR = My_Encryption.Encrypt(_newData);
                //string sss = HttpUtility.UrlEncode(_Encrypt_STR.Replace("%", "@per@").Replace("+", "@plus@"));
                string _needboarder = "NO";
                if (NeedBoarder.Checked)
                {
                    _needboarder = "YES";
                }
                //sai added
                string sql = "Insert into tblcertificatemanager (Student_Id,Student_Name,Class_Id,Class_Name,Certificatetype_Id,Certificate_Name,Certificate_Text,Created_Time,Need_Border,Created_User) values(" + int.Parse(Session["StudId"].ToString()) + ",'" + student_name + "'," + class_Id + ",'" + cls_name + "'," + int.Parse(Drp_CertificateType.SelectedValue.ToString()) + ",'" + Drp_CertificateType.SelectedItem.Text.ToString() + "','" + _newData + "','" + System.DateTime.Now.ToString("s") + "','" + _needboarder + "','" + MyUser.UserName + "')";
                MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Manage Certificate", "" + Drp_CertificateType.SelectedItem.Text.ToString() + " Issued for Student: " + student_name + "", 1);
                //
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "window.open(\"PrintCertificate.aspx?NeedBoarder=" + _needboarder + "\");", true);
            }
        }
        //sai added for class_id,student name
        private int get_classid(int student_Id,out string student_name)
        {
            int class_Id = 0;
            student_name = "";
            string sql = "SELECT tblview_student.ClassId,tblview_student.StudentName FROM tblview_student where Id=" + student_Id + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    class_Id = int.Parse(MyReader.GetValue(0).ToString());
                    student_name = MyReader.GetValue(1).ToString();
                }
            }
            return class_Id;
        }
        //sai added for class name
        private string get_className(int cls_Id)
        {
            string class_name = "";
            string sql = "SELECT ClassName FROM tblclass where Id=" + cls_Id + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    class_name = MyReader.GetValue(0).ToString();
                }
            }
            return class_name;
        }
        private string GetTextBoxRemoved(string _rawdata)
        {
            string _newdata=_rawdata;
            while (_newdata.Contains("<input"))
            {
                int _startposition = _newdata.IndexOf("<input");
                int _endposition = _newdata.IndexOf("</input>");
                string substring = _newdata.Substring(_startposition, _endposition - _startposition + 8);
                string _Id = GetId(substring);
                string _value = GetValue(_Id);

                _newdata = _newdata.Replace(substring, _value);


            }
            return _newdata;
        }

        private string GetValue(string _Id)
        {
            string _result = "";
            string[] splitstr = new string[] { "@#@" };
            string[] commonstring = Hd_Textvalues.Value.Split(splitstr, StringSplitOptions.RemoveEmptyEntries);
            splitstr = new string[] { "#$#" };
            string[] _idstring = commonstring[0].Split(splitstr, StringSplitOptions.None);
            if (commonstring.Length == 2)
            {
                string[] _valuestring = commonstring[1].Split(splitstr, StringSplitOptions.None);

                for (int i = 0; i < _idstring.Length; i++)
                {
                    if (_idstring[i] == _Id)
                    {
                        _result = _valuestring[i];
                        break;
                    }

                }
            }
            return _result;
        }

        private string GetId(string _substring)
        {
            
            int _startposition = _substring.IndexOf(" id=");
            int _endposition = _substring.IndexOf(" value=");
            string _value = _substring.Substring(_startposition + 5, _endposition - 1 - (_startposition + 5));
            return _value;
        }

    }
}
