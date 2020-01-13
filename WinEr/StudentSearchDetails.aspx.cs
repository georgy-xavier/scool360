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
    public partial class WebForm19 : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private Incident MyIncident;
        private OdbcDataReader MyReader = null;
       // private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyIncident = MyUser.GetIncedentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(406))
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
                   
                        try
                        {
                           
                            LoadpupilTopData();
                            CheckViewIncidentRight();
                            LoadGeneralDetails();
                            LoadOtherDetails();
                            LoadCoustomFields();
                            //some initialisations

                        }
                        catch
                        {

                        }
                    

                }
            }

        }
        protected void Page_init(object sender, EventArgs e)
        {
           
        }
        private void LoadCoustomFields()
        {
            int CustfieldCount = MyStudMang.CoustomFieldCount;
            if (CustfieldCount == 0)
            {
                Pnl_custumarea.Visible = false;
            }
            else
            {

                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();

                    myPlaceHolder.Controls.Add(tbl);
                    tbl.CssClass = "style1";

                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();

                        tc1.Text = dr_fieldData[1].ToString() + ":";
                        tc1.CssClass = "leftside";

                        Label Lblcostom = new Label();
                        Lblcostom.Text = MyStudMang.GetHistoryCuestomField(dr_fieldData[0].ToString(), Session["StudId"].ToString());
                        Lblcostom.ForeColor = System.Drawing.Color.Black;
                        Lblcostom.Font.Bold = true;
                        Lblcostom.ID = "myLbl" + i.ToString();
                        tc2.Controls.Add(Lblcostom);


                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);

                        tbl.Rows.Add(tr);
                        i++;
                    }
                }

            }
        }

        private void LoadOtherDetails()
        {
            string Sql = "select tblview_student.Nationality , tblview_student.MothersName, tblview_student.FatherEduQuali, tblview_student.MotherEduQuali, tblview_student.FatherOccupation, tblview_student.AnnualIncome, tblview_student.Addresspresent, tblview_student.Location, tblview_student.State, tblview_student.Pin, tblview_student.ResidencePhNo, tblview_student.OfficePhNo, tblview_student.Email, tblview_student.NumberofBrothers, tblview_student.NumberOfSysters from  tblview_student where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            // Up to Nationality Field in tblstudent
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_nat_ot.Text = MyReader.GetValue(0).ToString();
                Lbl_mothernane_ot.Text = MyReader.GetValue(1).ToString();

                Lbl_fatherqlif_ot.Text = MyReader.GetValue(2).ToString();
                Lbl_motherqlfi_ot.Text = MyReader.GetValue(3).ToString();
                Lbl_fatherocc_ot.Text = MyReader.GetValue(4).ToString();
                if (MyReader.GetValue(5).ToString() != "0")
                    Lbl_annualincom_ot.Text = MyReader.GetValue(5).ToString();
                Txt_addresspresent_ot.Text = MyReader.GetValue(6).ToString();
                Lbl_location_ot.Text = MyReader.GetValue(7).ToString();
                Lbl_state_ot.Text = MyReader.GetValue(8).ToString();
                if (MyReader.GetValue(9).ToString() != "0")
                    Lbl_pin_ot.Text = MyReader.GetValue(9).ToString();
                if (MyReader.GetValue(10).ToString() != "0")
                    Lbl_resdphn_ot.Text = MyReader.GetValue(10).ToString();
                if (MyReader.GetValue(11).ToString() != "0")
                    Lbl_mob_ot.Text = MyReader.GetValue(11).ToString();
                Lbl_email_ot.Text = MyReader.GetValue(12).ToString();
                if (MyReader.GetValue(13).ToString() != "0")
                    Lbl_nofobrot_ot.Text = MyReader.GetValue(13).ToString();
                if (MyReader.GetValue(14).ToString() != "0")
                    Lbl_noofsist_ot.Text = MyReader.GetValue(14).ToString();
                MyReader.Close();
            }

            Sql = "select tblbloodgrp.GroupName from  tblview_student inner join tblbloodgrp on tblbloodgrp.Id= tblview_student.BloodGroup  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_blodgroup_ot.Text = MyReader.GetValue(0).ToString();

                MyReader.Close();
            }
            Sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.MotherTongue  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_mot_ot.Text = MyReader.GetValue(0).ToString();

                MyReader.Close();
            }
            Sql = "select tbllanguage.`Language` from  tblview_student inner join tbllanguage on tbllanguage.Id= tblview_student.1stLanguage  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_firstlng_ot.Text = MyReader.GetValue(0).ToString();

                MyReader.Close();
            }

            Sql = "select tblstudtype.TypeName from  tblview_student inner join tblstudtype on tblstudtype.Id= tblview_student.StudTypeId  where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_studcat_ot.Text = MyReader.GetValue(0).ToString();

                MyReader.Close();
            }
        }

        private void LoadGeneralDetails()
        {
            DateTime Dob;
            DateTime Doj;
            int Today = DateTime.Now.Year;
            string Sql = "select tblview_student.StudentName, tblview_student.Sex, tblview_student.DOB, tblview_student.GardianName, tblview_student.Address, tblview_student.DateofJoining from tblview_student where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            // Up to Nationality Field in tblstudent
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_Name_gnl.Text = MyReader.GetValue(0).ToString();
                Lbl_sex_gnl.Text = MyReader.GetValue(1).ToString();
                Dob = DateTime.Parse(MyReader.GetValue(2).ToString());
                //Dob = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());

                Lbl_dob_gnl.Text = Dob.Date.ToString("dd-MM-yyyy");
                Lbl_father_gnl.Text = MyReader.GetValue(3).ToString();
                Txt_Address.Text = MyReader.GetValue(4).ToString();
                Doj = DateTime.Parse(MyReader.GetValue(5).ToString());
                //Doj = MyUser.GetDareFromText(MyReader.GetValue(5).ToString());
                Lbl_doa_gnl.Text = Doj.Date.ToString("dd-MM-yyyy");
                MyReader.Close();
            }

            Sql = "SELECT tblstandard.Name,tblclass.ClassName from tblview_studentclassmap INNER join tblclass on tblview_studentclassmap.ClassId=tblclass.Id inner join tblstandard on tblstandard.Id = tblview_studentclassmap.Standard where tblview_studentclassmap.BatchId=" + MyUser.CurrentBatchId + " And tblview_studentclassmap.StudentId= " + int.Parse(Session["StudId"].ToString()) + "";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_std_gnl.Text = MyReader.GetValue(0).ToString();
                Lbl_class_gnl.Text = MyReader.GetValue(1).ToString();
                MyReader.Close();
            }
            Sql = "SELECT batch.BatchName FROM tblbatch batch INNER JOIN tblview_student stud on stud.JoinBatch = batch.Id AND stud.Id=" + int.Parse(Session["StudId"].ToString()) + " AND stud.Status=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_joinbatch_gnl.Text = MyReader.GetValue(0).ToString();

            }

            Sql = "select tblreligion.Religion, tblcast.castname from  tblview_student inner join tblreligion on tblreligion.Id= tblview_student.Religion inner join tblcast on tblcast.Id= tblview_student.`Cast` where tblview_student.Id=" + int.Parse(Session["StudId"].ToString());
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_religion_gnl.Text = MyReader.GetValue(0).ToString();
                Lbl_cast_gnl.Text = MyReader.GetValue(1).ToString();
                MyReader.Close();
            }

        }


        private void LoadpupilTopData()
        {

            string _studstrip = MyStudMang.ToHistoryStripString(int.Parse(Session["StudId"].ToString()), Get_student_History_ImageUrl("StudentImage", int.Parse(Session["StudId"].ToString())));
            this.StudentTopStrip.InnerHtml = _studstrip;
        }
        private string Get_student_History_ImageUrl(string Type, int studId)
        {
            string ImageUrl = "images/" + "img.png";
            string Sql = "SELECT tblfileurl_history.FilePath FROM tblfileurl_history WHERE tblfileurl_history.Type='" + Type + "' AND tblfileurl_history.UserId=" + studId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/" + MyReader.GetValue(0).ToString();
            }
            else
            {
                ImageUrl = "images/" + "img.png";
            }
            return ImageUrl;
        }
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentSearch.aspx");
        }      


        #region incedent details
        private void CheckViewIncidentRight()
        {
            if (MyUser.HaveActionRignt(70))
            {
                this.TopTab.InnerHtml = LoadIncidenceHistoryData(int.Parse(Session["StudId"].ToString()), "student");
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
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                _N_IncidentType = int.Parse(MyReader.GetValue(0).ToString());
            }
            sql = "select tblincedenttype.Id , tblincedenttype.`Type` from tblincedenttype where tblincedenttype.Visibility = 1  and  tblincedenttype.IncidentType='NORMAL'  order by tblincedenttype.`Order` ";
            _Mydata_IncidentType = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
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
            DataSet _Mydata_IncidentData = GetPupilIncidentDataFromHistory(_UserId, _Type, _IncTypeId);
            DateTime _IncidentDate;
            int _index = 1;
            if (_Mydata_IncidentData != null && _Mydata_IncidentData.Tables != null && _Mydata_IncidentData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_IncidentData in _Mydata_IncidentData.Tables[0].Rows)
                {
                    if (_index < 4)
                    {
                        _IncidentDate = DateTime.Parse(dr_IncidentData[3].ToString());
                        // _IncidentBlockData.Append("<b>" + _IncidentDate.Date.ToString("dd-MM-yyyy") + ")" + dr_IncidentData[1].ToString() + "...</b><br />&nbsp; <a href=\"javascript:openIncpopup('ViewIncidence.aspx?id=" + dr_IncidentData[0].ToString() + "&Type=" + _Type + "')\">" + dr_IncidentData[2].ToString() + "...</a><br />");
                        _IncidentBlockData.Append("<b>" + General.GerFormatedDatVal(_IncidentDate.Date) + "-" + dr_IncidentData[1].ToString() + "</b><br />&nbsp;" + dr_IncidentData[2].ToString() + "<br />");
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
        private DataSet GetPupilIncidentDataFromHistory(int _UserId, string _Type, int _IncTypeId)
        {
            string sql;
            DataSet _Mydata_IncidentData = null;
            if (_Type == "student")
            {
                sql = "select tblview_incident.Id ,tblview_incident.Title, tblview_incident.Description, tblview_incident.IncedentDate from tblview_incident inner join tblview_student on tblview_student.Id = tblview_incident.AssoUser where tblview_incident.UserType = '" + _Type + "' and tblview_incident.TypeId =" + _IncTypeId + "  and tblview_incident.`Status` ='Approved' and tblview_student.Id = " + _UserId + " order by tblview_incident.CreatedDate DESC";
                _Mydata_IncidentData = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            return _Mydata_IncidentData;
        }
        #endregion



    }
}
