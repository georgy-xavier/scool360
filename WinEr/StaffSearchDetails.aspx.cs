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
using System.Data.Odbc;
using WinBase;
using System.Text;

namespace WinEr
{
    public partial class WebForm20 : System.Web.UI.Page
    {
        private WinErSearch MySearchMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        //private DataSet MydataSet;
        private int StaffId = 0;

        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(48))
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
                    if (Request.QueryString["StaffId"].ToString() != "")
                    {
                        try
                        {
                            StaffId = int.Parse(Request.QueryString["StaffId"]);
                            LoadstaffTopData();
                            LoadStaffDatas(StaffId);
                            Loadstaff_incedenthistory();                          
           
                        }
                        catch
                        {

                        }
                    }
                    

                }
            }

        }
        private void LoadStaffDatas(int StaffId)
        {
            string sql = "select tbluserdetails_history.Address, tbluserdetails_history.JoiningDate, tbluserdetails_history.Experience, tbluserdetails_history.ExpDescription, tbluserdetails_history.PhoneNumber, tbluserdetails_history.EduQualifications, tbluserdetails_history.Dob, tbluser_history.EmailId   from tbluserdetails_history inner join  tbluser_history on tbluserdetails_history.UserId = tbluser_history.Id where tbluserdetails_history.UserId=" + StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_Address.Text = MyReader.GetValue(0).ToString();
                Lbl_joindate.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(1).ToString()));
                //Lbl_joindate.Text = MyUser.GetDareFromText(MyReader.GetValue(1).ToString()).ToString("yyyy-MM-dd");

                lbl_experience.Text = MyReader.GetValue(2).ToString();
                lbl_expdesc.Text = MyReader.GetValue(3).ToString();
                lbl_phno.Text = MyReader.GetValue(4).ToString();
                lbl_eduqualification.Text = MyReader.GetValue(5).ToString();
                lbl_dob.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(6).ToString()));
                //lbl_dob.Text = MyUser.GetDareFromText(MyReader.GetValue(6).ToString()).ToString("yyyy-MM-dd");

                lbl_email.Text = MyReader.GetValue(7).ToString();
            }
        }
        private void LoadstaffTopData()
        {
            string _imgurl = Get_staff_History_ImageUrl("StaffImage", StaffId);
            string _studstrip = TopHistoryStripString(StaffId, _imgurl);
            this.StudentTopStrip.InnerHtml = _studstrip;
        }
        private string Get_staff_History_ImageUrl(string Type, int StaffId)
        {
            string ImageUrl = "images/" + "img.png";
            string Sql = "SELECT tblfileurl_history.FilePath FROM tblfileurl_history WHERE tblfileurl_history.Type='" + Type + "' AND tblfileurl_history.UserId=" + StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                ImageUrl =  WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/" + MyReader.GetValue(0).ToString();
            }
            else
            {
                ImageUrl = "images/" + "img.png";
            }
            return ImageUrl;
        }
        private string TopHistoryStripString(int _staffid, string _imgurl)
        {
            DateTime _DOB;
            string _staffname = "", _sex = "", _age = "";
            int Year;
            int Today = DateTime.Now.Year;

            string Sql = "select tbluser_history.SurName, tbluserdetails_history.Sex, tbluserdetails_history.Dob from tbluser_history  inner join tbluserdetails_history on tbluser_history.id= tbluserdetails_history.UserId where tbluser_history.Id=" + _staffid;

            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                _staffname = MyReader.GetValue(0).ToString();

                _sex = MyReader.GetValue(1).ToString();
                _DOB = DateTime.Parse(MyReader.GetValue(2).ToString());
                //_DOB = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());

                Year = Today - _DOB.Year;
                _age = Year.ToString();
                MyReader.Close();
            }

            StringBuilder _pupilTopData = new StringBuilder("<div id=\"winschoolStudentStrip\"><table class=\"NewStudentStrip\" width=\"100%\"><tr><td class=\"left1\"></td><td class=\"middle1\"><table width=\"100%\"><tr><td><img alt=\"\" src=\"" + _imgurl + "\" width=\"82px\" height=\"76px\" /></td><td></td><td><table width=\"100%\"><tr><td class=\"attributeValue\">Name</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _staffname + "</td></tr><tr><td colspan=\"11\"><hr /></td></tr><tr><td class=\"attributeValue\">Sex</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _sex + "</td><td class=\"attributeValue\">Age</td><td></td><td>:</td><td></td><td class=\"DBvalue\">" + _age + "</td></tr></table></td></td></tr></table></td><td class=\"right1\"></td></tr></table></div>");


            return _pupilTopData.ToString();
        }

        #region incedentarea

        private void Loadstaff_incedenthistory()
        {
            if (MyUser.HaveActionRignt(70))
            {
                this.TopTab.InnerHtml = LoadIncidenceHistoryData(StaffId, "Staff");
            }
            else
            {

                this.TabPanel3.Visible = false;
            }
        }

        private string LoadIncidenceHistoryData(int _UserId, string _Type)
        {
            int _N_IncidentType = 0;
            StringBuilder _IncidentDat = new StringBuilder("");
            DataSet _Mydata_IncidentType;
            int MaxRows, MaxColumn = 3;
            int ExtraRow = 0, i, j;
            int _TypePtr = 0;
            DataRow dr_IncidentType;

            string sql = "select count(tblincedenttype.Id) from tblincedenttype  where  tblincedenttype.IncidentType='NORMAL' ";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _N_IncidentType = int.Parse(MyReader.GetValue(0).ToString());
            }
            sql = "select tblincedenttype.Id , tblincedenttype.`Type` from tblincedenttype where tblincedenttype.Visibility = 1  and   tblincedenttype.IncidentType='NORMAL'  order by tblincedenttype.`Order` ";
            _Mydata_IncidentType = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Mydata_IncidentType != null && _Mydata_IncidentType.Tables != null && _Mydata_IncidentType.Tables[0].Rows.Count > 0)
            {

                if (_N_IncidentType > 0)
                {

                    MaxRows = _N_IncidentType / 3;
                    ExtraRow = _N_IncidentType % 3;
                    if (ExtraRow > 0)
                    {
                        MaxRows = MaxRows + 1;
                    }
                    _IncidentDat.Append("<table width=\"100%\">");
                    for (i = 1; i <= MaxRows; i++)
                    {
                        _IncidentDat.Append("<tr>");
                        //Count = Count +1;
                        if (i == MaxRows)
                            MaxColumn = ExtraRow;
                        for (j = 0; j < MaxColumn; j++)
                        {

                            dr_IncidentType = _Mydata_IncidentType.Tables[0].Rows[_TypePtr];
                            _IncidentDat.Append(GetIncidentBlockStr(_UserId, _Type, int.Parse(dr_IncidentType[0].ToString()), dr_IncidentType[1].ToString()));

                            _TypePtr++;
                        }
                        _IncidentDat.Append("</tr>");
                    }
                    _IncidentDat.Append("</table>");
                }
            }
            return _IncidentDat.ToString();
        }
        private string GetIncidentBlockStr(int _UserId, string _Type, int _IncTypeId, string _IncType)
        {
            StringBuilder _IncidentBlockStruct = new StringBuilder("<td style=\"width:33%\"><div class=\"container skin1 \" > <table cellpadding=\"0\" cellspacing=\"0\" class=\"containerTable\"> <tr class=\"top\"><td class=\"no\"> </td> <td class=\"n\">  " + _IncType + " </td> <td class=\"ne\"> </td></tr><tr class=\"middle\"> <td class=\"o\"> </td> 	<td class=\"c\"> <div class =\"IncBlock\">");

            _IncidentBlockStruct.Append(GetBlockData(_UserId, _Type, _IncTypeId, _IncType));
            _IncidentBlockStruct.Append("</div></td> <td class=\"e\"> </td> </tr><tr class=\"bottom\"> <td class=\"so\"> </td> <td class=\"s\"></td> <td class=\"se\"> </td> </tr> 	</table> </div></td>");

            return _IncidentBlockStruct.ToString();

        }
        private string GetBlockData(int _UserId, string _Type, int _IncTypeId, string _IncType)
        {
            StringBuilder _IncidentBlockData = new StringBuilder("");
            DataSet _Mydata_IncidentData = GetStaffIncidentDataFromHistory(_UserId, _Type, _IncTypeId);
            DateTime _IncidentDate;
            int _index = 1;
            if (_Mydata_IncidentData != null && _Mydata_IncidentData.Tables != null && _Mydata_IncidentData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_IncidentData in _Mydata_IncidentData.Tables[0].Rows)
                {
                    if (_index < 4)
                    {
                        _IncidentDate = DateTime.Parse(dr_IncidentData[3].ToString());
                        //_IncidentBlockData.Append("<b>" + _IncidentDate.Date.ToString("dd-MM-yyyy") + ")" + dr_IncidentData[1].ToString() + "...</b><br />&nbsp; <a href=\"javascript:openIncpopup('ViewIncidence.aspx?id=" + dr_IncidentData[0].ToString() + "&Type=" + _Type + "')\">" + dr_IncidentData[2].ToString() + "...</a><br />");
                        _IncidentBlockData.Append("<b>" + _IncidentDate.Date.ToString("dd-MM-yyyy") + "-" + dr_IncidentData[1].ToString() + "</b><br />&nbsp;" + dr_IncidentData[2].ToString() + "<br />");
                    }
                    _index++;
                }
                //if (_Type == "student")
                //{
                //    _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewIncidenceByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                //}
                //else if (_Type == "Staff")
                //{
                //    _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewStaffIncidenceByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                //}
                //else if (_Type == "Class")
                //{
                //    _IncidentBlockData.Append("<div style=\"text-align:right\"><a href=\"ViewClassIncidentsByType.aspx?id= " + _IncTypeId + "&Type= " + _Type + "\">more...</a> </div>");
                //}

            }
            else
            {
                _IncidentBlockData.Append("<b>No incident reported</b>");
            }



            return _IncidentBlockData.ToString();
        }
        private DataSet GetStaffIncidentDataFromHistory(int _UserId, string _Type, int _IncTypeId)
        {
            string sql;
            DataSet _Mydata_IncidentData = null;

            if (_Type == "Staff")
            {
                sql = "select tblincedent_history.Id , tblincedent_history.Title, tblincedent_history.Description, tblincedent_history.IncedentDate from tblincedent_history inner join tbluser_history on tbluser_history.Id = tblincedent_history.AssoUser where tblincedent_history.UserType = '" + _Type + "' and tblincedent_history.TypeId =" + _IncTypeId + "  and tblincedent_history.`Status` ='Approved' and tbluser_history.Id = " + _UserId + " order by tblincedent_history.CreatedDate DESC";
                _Mydata_IncidentData = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }

            return _Mydata_IncidentData;
        }

        #endregion

     
        /*
        private void LoadSubjectsHandle(int StafId)
        {
            ChkBox_AssSub.Items.Clear();
            string sql = "SELECT Id,subject_name FROM tblsubjects";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    if (MySearchMang.IsStaffSubject(StafId,int.Parse(MyReader.GetValue(0).ToString())))
                    {
                        ChkBox_AssSub.Items.Add(li);
                    }
                }
            }
        }

        private void AddStaffDetails(int StaffId)
        {
            int Year;
            int Today = DateTime.Now.Year;
            DateTime Dob;
            DateTime JoiningDate;
            String Sql = "SELECT UserName,SurName,EmailId FROM tbluser_history WHERE Id="+StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Txt_StaffId.Text = MyReader.GetValue(0).ToString();
                Txt_StaffName.Text = MyReader.GetValue(1).ToString();
                Txt_Email0.Text = MyReader.GetValue(2).ToString();
            }
            Sql = "SELECT tblrole.RoleName FROM tblrole INNER JOIN tbluser_history ON tblrole.Id =tbluser_history.RoleId WHERE tbluser_history.Id="+StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Txt_Role.Text = MyReader.GetValue(0).ToString();
            }
            Sql = "SELECT Experience,Designation,JoiningDate,Address,Sex,Dob,ExpDescription,PhoneNumber,EduQualifications FROM tblstaffdetails WHERE UserId="+StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Txt_Experience.Text = MyReader.GetValue(0).ToString();
                Txt_Desig.Text = MyReader.GetValue(1).ToString();
                JoiningDate = DateTime.Parse(MyReader.GetValue(2).ToString());
                Txt_JoiningDate.Text = JoiningDate.Date.ToString("dd-MM-yyyy");
                Txt_Address.Text = MyReader.GetValue(3).ToString();
                Txt_Sex.Text = MyReader.GetValue(4).ToString();
                Dob = DateTime.Parse(MyReader.GetValue(5).ToString());
                Year = Today - Dob.Year;
                Txt_Age.Text = Year.ToString();
                Txt_Dob.Text = Dob.Date.ToString("dd-MM-yyyy");
                Txt_ExpDic.Text = MyReader.GetValue(6).ToString();
                Txt_PhNo0.Text = MyReader.GetValue(7).ToString();
                Txt_EduQuli.Text = MyReader.GetValue(8).ToString();

            }

        }
        private void LoadStaffPhoto(int StaffId)
        {
           /* String ImageUrl = "";
            String Sql = "SELECT FilePath FROM tblfileurl WHERE Type='StaffImage' AND UserId="+StaffId;
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                ImageUrl = MyReader.GetValue(0).ToString();
            }
            else
            {
                ImageUrl = "img.png";
            }

            Img_staff.ImageUrl = "ThumbnailImages/" + ImageUrl;*/
           /* Img_staff.ImageUrl = MyUser.GetImageUrl("StaffImage", StaffId);
        }
    */

    }
}
