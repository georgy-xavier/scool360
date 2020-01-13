using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Xml;
using WinBase;
using System.IO;
using AjaxControlToolkit;

namespace WinEr
{
    public partial class ViewRegisteredStudents : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private EmailManager Obj_Email;
        private SMSManager MysmsMang;
        private int CustfieldCount;
        private TextBox[] dynamicTextBoxes;
        private int[] Mandatoryflag;
        private string[] FealdName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(604))
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                Lbl_Msg.Text = "";
                if (!IsPostBack)
                {
                    MysmsMang.InitClass();
                    LoadAccedemicYearToDrpList();
                    AddStandardToDrpList();
                    //Pnl_TempList.Visible = false;
                    AddStandardToPopUpDrp();
                    AddClassTopopupDrp();
                    LoadBatchToPopUpDrpList();
                    LoadStatusToDropDown(0);
                    LoadGrid("", 0, 0, 0, 1);
                }
                Lbl_ErrMsg.Text = "";
                Colstaus.Visible = false;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();

            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields.Tables[0].Rows.Count != 0)
                {

                    if (Session["TempStudId"] != null)
                    {
                        string _TempStudId = Session["TempStudId"].ToString();
                        AddCoustomControls(_TempStudId);
                    }
                }

            }
        }
        private void LoadStatusToDropDown(int index)
        {
            DataSet StatusDs = new DataSet();
            Drp_Status.Items.Clear();
            ListItem li;
            StatusDs = MyStudMang.GetAllStatus();
            if (StatusDs != null && StatusDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("ALL", "0");
                Drp_Status.Items.Add(li);
                foreach (DataRow dr in StatusDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Status"].ToString().ToUpper(), dr["Id"].ToString());
                    Drp_Status.Items.Add(li);
                }
                Drp_Status.SelectedValue = index.ToString();
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Status.Items.Add(li);
            }
        }



        private void LoadAccedemicYearToDrpList()
        {

            Drp_EditYear.Items.Clear();
            string sql = "select tblbatch.BatchName ,tblbatch.Id from tblbatch where `status`=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());

                Drp_EditYear.Items.Add(li);
                int Nxtbatch = int.Parse(MyReader.GetValue(1).ToString()) + 1;
                sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id=" + Nxtbatch;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());

                    Drp_EditYear.Items.Add(li);
                }
            }
        }

        private void AddStandardToDrpList()
        {
            Drp_EditStd.Items.Clear();

            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_EditStd.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
            }
        }

        protected void Grd_studentlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyUser.HaveActionRignt(605))
            {
                if (Grd_studentlist.SelectedRow.Cells[6].Text.Replace("&nbsp;", "") != "")
                {
                    Response.Redirect("CollectJoiningFee.aspx?Studentid=" + Grd_studentlist.SelectedRow.Cells[1].Text.ToString());
                }
                else
                {
                    WC_MsgBox.ShowMssage("Sorry,Go to edit and enter class to view the fee details");
                }

            }
            else
            {
                WC_MsgBox.ShowMssage("You do not have sufficient rights to perform this action. Contact administrator");
                //Lbl_Message.Text = "You do not have sufficient rights to perform this action. Contact administrator";
            }
        }




        #region TEMPSTUDENTS LIST

        protected void Grd_studentlist_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            AddBloodGrpToDrpList(16);
            AddMotherTongueToDrpList(0);

            string _TempStudId = Grd_studentlist.Rows[e.RowIndex].Cells[3].Text.ToString();
            Hdn_TempId.Value = _TempStudId;
            string sql = "SELECT tbltempstdent.Name, tbltempstdent.Fathername,tbltempstdent.Gender, tbltempstdent.Standard, tbltempstdent.Class,  tbltempstdent.JoiningBatch, tbltempstdent.Rank, tbltempstdent.PhoneNumber, tbltempstdent.Address,Email,Date_Format(DOB,'%d/%m/%Y') as DOB ,MotherName,Location,Pin,State,Nationality,BloodGroup,MotherTongue,FatherEduQualification,MotherEduQualification,FatherOccupation,MotherOccupation,AnnualIncome,Remark,PersionalInterview,DateOfInterView,TeacherRemark,HMRemark,PrincipalRemark,Result,PreviousBoard from tbltempstdent where tbltempstdent.TempId='" + _TempStudId + "'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                txt_EditName.Text = MyReader.GetValue(0).ToString();
                txt_EditFatherName.Text = MyReader.GetValue(1).ToString();
                if (MyReader.GetValue(2).ToString() == "Male")
                {
                    Rdb_EditSex.SelectedValue = "0";
                }
                else
                {
                    Rdb_EditSex.SelectedValue = "1";
                }
                Drp_EditStd.SelectedValue = MyReader.GetValue(3).ToString();
                int Std;
                AddEditClassTopopupDrp(out Std);
                if (Std != 1)
                {
                    if (MyReader.GetValue(4).ToString() != "")
                    {

                        Drp_EditClass.SelectedValue = MyReader.GetValue(4).ToString();
                    }
                    else
                    {

                        Drp_EditClass.SelectedValue = "0";
                    }
                }
                else
                {

                }
                Drp_EditYear.SelectedValue = MyReader.GetValue(5).ToString();
                txt_EditRank.Text = MyReader.GetValue(6).ToString();
                txt_EditPhone.Text = MyReader.GetValue(7).ToString();
                txt_EditAddress.Text = MyReader.GetValue(8).ToString();
                Txt_Email.Text = MyReader["Email"].ToString();
                Txt_Dob.Text = MyReader["DOB"].ToString();
                txtMotherName.Text = MyReader["MotherName"].ToString();
                Txt_Location.Text = MyReader["Location"].ToString();
                Txt_PinCode.Text = MyReader["Pin"].ToString();
                Txt_State.Text = MyReader["State"].ToString();
                Txt_nationality.Text = MyReader["Nationality"].ToString();
                if (MyReader["BloodGroup"].ToString() != "")
                    Drp_BloodGrp.SelectedValue = MyReader["BloodGroup"].ToString();
                else
                    Drp_BloodGrp.SelectedValue = "17";
                if (MyReader["MotherTongue"].ToString() != "")

                    Drp_MotherTongue.SelectedValue = MyReader["MotherTongue"].ToString();

                Txt_father_educ.Text = MyReader["FatherEduQualification"].ToString();
                Txt_Mothers_educ.Text = MyReader["MotherEduQualification"].ToString();
                Txt_fathers_Ocuptn.Text = MyReader["FatherOccupation"].ToString();
                Txt_mothers_Ocuptn.Text = MyReader["MotherOccupation"].ToString();
                Txt_annual_incum.Text = MyReader["AnnualIncome"].ToString();
                txtStudRemark.Text = MyReader["Remark"].ToString();
                if (MyReader["PersionalInterview"].ToString() != "")
                    rdoInterView.SelectedValue = MyReader["PersionalInterview"].ToString();

                if (MyReader["PersionalInterview"].ToString() == "Attended")
                {
                    pnlInterviewDetails.Visible = true;

                    txtDOI.Text = MyReader["DateOfInterView"].ToString();
                    txtHMRemark.Text = MyReader["HMRemark"].ToString();
                    txtPrincipalRemark.Text = MyReader["PrincipalRemark"].ToString();
                    txtTeacherRemark.Text = MyReader["TeacherRemark"].ToString();
                    rdoResult.SelectedValue = MyReader["Result"].ToString();
                }
                else
                    pnlInterviewDetails.Visible = false;

                if (MyReader["PreviousBoard"].ToString() != "")
                    txtPreviousBoard.Text = MyReader["PreviousBoard"].ToString();

                //DataSet _CustomFields = MyStudMang.GetCuestomFields();
                //if (_CustomFields.Tables[0].Rows.Count != 0)
                //{
                //    AddCoustomControls(_TempStudId);
                //    Session["TempStudId"] = _TempStudId;
                //}
                // Session["TempStudId"] = null;
                Session["TempStudId"] = _TempStudId;

            }
            MPE_MessageBox.Show();
        }




        private void AddCoustomControls(string _TempStudId)
        {
            CustfieldCount = MyStudMang.CoustomFieldCount;
            if (CustfieldCount == 0)
            {
                Label lbicusnote = new Label();
                lbicusnote.ID = "lbicusnote";
                lbicusnote.Text = "No Coustom Fields Present.";
                myPlaceHolder.Controls.Add(lbicusnote);

            }
            else
            {

                dynamicTextBoxes = new TextBox[CustfieldCount];
                Mandatoryflag = new int[CustfieldCount];
                FealdName = new string[CustfieldCount];
                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();
                    tbl.Width = 500;
                    myPlaceHolder.Controls.Add(tbl);


                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc3 = new TableCell();
                        if (dr_fieldData[4].ToString() == "1")
                        {
                            tc1.Text = dr_fieldData[1].ToString() + "<span class=\"redcol\">*</span>";
                            RequiredFieldValidator ReqfldvalExt = new RequiredFieldValidator();
                            ReqfldvalExt.ID = "ReqfldvalExt" + i.ToString();
                            ReqfldvalExt.ControlToValidate = "myTextBox" + i.ToString();
                            ReqfldvalExt.ErrorMessage = "You Must enter " + dr_fieldData[1].ToString();
                            tc3.Controls.Add(ReqfldvalExt);
                        }
                        else
                        {
                            tc1.Text = dr_fieldData[1].ToString();
                        }
                        TextBox textBox = new TextBox();
                        textBox.MaxLength = int.Parse(dr_fieldData[3].ToString());
                        textBox.ID = "myTextBox" + i.ToString();
                        textBox.Text = MyStudMang.GetCustomFieldRegStdnt(dr_fieldData[0].ToString(), _TempStudId);
                        tc2.Controls.Add(textBox);
                        Mandatoryflag[i] = int.Parse(dr_fieldData[4].ToString());
                        FealdName[i] = dr_fieldData[0].ToString();
                        dynamicTextBoxes[i] = textBox;

                        FilteredTextBoxExtender FiltTxtbxExt = new FilteredTextBoxExtender();
                        FiltTxtbxExt.ID = "FiltTxtbxExt" + i.ToString();
                        if (dr_fieldData[2].ToString() == "1")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Numbers;

                        }
                        else if (dr_fieldData[2].ToString() == "2")
                        {

                            FiltTxtbxExt.FilterType = FilterTypes.Custom;
                            FiltTxtbxExt.FilterMode = FilterModes.InvalidChars;
                            FiltTxtbxExt.InvalidChars = "'/\\";
                        }
                        else
                        {
                            FiltTxtbxExt.ValidChars = ".";
                            FiltTxtbxExt.FilterType = FilterTypes.Numbers | FilterTypes.Custom;

                        }
                        FiltTxtbxExt.TargetControlID = "myTextBox" + i.ToString();
                        myPlaceHolder.Controls.Add(FiltTxtbxExt);



                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);
                        tr.Cells.Add(tc3);
                        tbl.Rows.Add(tr);
                        i++;
                    }
                }

            }
        }
        private bool UpDateCoustomFields(string Tempid)
        {
            bool valid = false;
            CustfieldCount = MyStudMang.CoustomFieldCount;
            // dynamicTextBoxes = new TextBox[CustfieldCount];
            FealdName = new string[CustfieldCount];
            if (CustfieldCount > 0)
            {
                int i = 0;
                string Fields, Values;
                Fields = "StudentId";
                Values = "'" + Tempid + "'";

                TextBox textBox = new TextBox();


                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    // dynamicTextBoxes[i] = textBox;

                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        FealdName[i] = dr_fieldData[0].ToString();
                        //dynamicTextBoxes[i] = textBox;

                    }
                }


                for (i = 0; i < CustfieldCount; i++)
                {

                    Fields = Fields + "," + FealdName[i];
                    Values = Values + "," + "'" + dynamicTextBoxes[i].Text + "'";
                }
                valid = MyStudMang.InsertTempStudentDetails(Fields, Values);
            }
            else
            {
                valid = true;
            }
            Session["TempStudId"] = null;
            return valid;
        }

















        private void AddBloodGrpToDrpList(int _intex)
        {
            Drp_BloodGrp.Items.Clear();

            string sql = "SELECT Id,GroupName FROM tblbloodgrp";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_BloodGrp.Items.Add(li);

                }
                Drp_BloodGrp.SelectedIndex = _intex;
            }
        }

        private void AddMotherTongueToDrpList(int _intex)
        {
            Drp_MotherTongue.Items.Clear();
            string sql = "SELECT Id,Language FROM tbllanguage";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_MotherTongue.Items.Add(li);

                }
                Drp_MotherTongue.SelectedIndex = _intex;
            }
        }
        protected void Btn_RankCancel_click(object sender, EventArgs e)
        {
            Response.Redirect("ViewRegisteredStudents.aspx");
        }


        protected void Btn_magok_Click(object sender, EventArgs e)
        {
            string _TempStudId = Hdn_TempId.Value.Trim();
            string _sql = "";
            XmlDocument xmlDoc = new XmlDocument();
            _sql = "select tbl_xmlstring.Id, tbl_xmlstring.XMLString from tbl_xmlstring  where tbl_xmlstring.TempId='" + _TempStudId + "'";
            OdbcDataReader XmlReader = null;
            XmlReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
            if (XmlReader.HasRows)
            {
                xmlDoc.LoadXml(XmlReader.GetValue(1).ToString());
                XmlNodeList XmlNidelst = xmlDoc.SelectNodes("Student//Column");
                for (int i = 0; i < XmlNidelst.Count; i++)
                {
                    XmlNode XmlRegNode = XmlNidelst.Item(i);
                    if (XmlRegNode.Attributes[7].Value == "StudentName")
                    {
                        XmlRegNode.Attributes[1].Value = txt_EditName.Text;
                    }
                    if (XmlRegNode.Attributes[7].Value == "GuardianName")
                    {
                        XmlRegNode.Attributes[1].Value = txt_EditFatherName.Text;
                    }
                    if (XmlRegNode.Attributes[7].Value == "Gender")
                    {
                        XmlRegNode.Attributes[1].Value = Rdb_EditSex.SelectedValue;
                    }

                    if (XmlRegNode.Attributes[7].Value == "BatchId")
                    {
                        XmlRegNode.Attributes[1].Value = Drp_EditYear.SelectedValue;
                    }
                    if (XmlRegNode.Attributes[7].Value == "MobileNum")
                    {
                        XmlRegNode.Attributes[1].Value = txt_EditPhone.Text;
                    }
                    if (XmlRegNode.Attributes[7].Value == "PemanentAddress")
                    {
                        XmlRegNode.Attributes[1].Value = txt_EditAddress.Text;
                    }
                    if (XmlRegNode.Attributes[7].Value == "Standard")
                    {
                        XmlRegNode.Attributes[1].Value = Drp_EditStd.SelectedValue;
                    }
                }

                StringWriter Studentwriter = new StringWriter();
                xmlDoc.Save(Studentwriter);
                string XmlString = Studentwriter.ToString();
                _sql = "Update tbl_xmlstring set XMLString='" + XmlString + "' where TempId='" + _TempStudId + "' ";
                MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
            }
            string pin = "0";
            if (Txt_PinCode.Text == "")
                pin = "0";
            else
                pin = Txt_PinCode.Text;
            string anualincome = "0";

            if (Txt_annual_incum.Text.Trim() == "")
                anualincome = "0";
            else
                anualincome = Txt_annual_incum.Text.Trim();

            string sql = "UPDATE tbltempstdent SET tbltempstdent.Name='" + txt_EditName.Text.Trim().ToUpperInvariant() + "', tbltempstdent.Fathername='" + txt_EditFatherName.Text.Trim().ToUpperInvariant() + "', tbltempstdent.Gender='" + Rdb_EditSex.SelectedItem.Text + "', tbltempstdent.Standard=" + Drp_EditStd.SelectedValue + ", tbltempstdent.Class=" + Drp_EditClass.SelectedValue + ", tbltempstdent.JoiningBatch=" + Drp_EditYear.SelectedValue + ",tbltempstdent.Rank=" + txt_EditRank.Text + ", tbltempstdent.PhoneNumber='" + txt_EditPhone.Text + "', tbltempstdent.Address='" + txt_EditAddress.Text.Trim().ToUpperInvariant() + "', MotherName ='" + txtMotherName.Text + "',Location='" + Txt_Location.Text + "',Pin=" + pin + ",State='" + Txt_State.Text + "',Nationality='" + Txt_nationality.Text + "',BloodGroup=" + Drp_BloodGrp.SelectedValue + ",MotherTongue=" + Drp_MotherTongue.Text + ",FatherEduQualification='" + Txt_father_educ.Text + "',MotherEduQualification='" + Txt_Mothers_educ.Text + "',FatherOccupation='" + Txt_fathers_Ocuptn.Text + "',MotherOccupation='" + Txt_mothers_Ocuptn.Text + "',AnnualIncome=" + anualincome + ",Remark='" + txtStudRemark.Text + "',PersionalInterview='" + rdoInterView.SelectedValue + "',DateOfInterView='" + txtDOI.Text + "',TeacherRemark='" + txtTeacherRemark.Text + "',HMRemark='" + txtHMRemark.Text + "',PrincipalRemark='" + txtPrincipalRemark.Text + "',Result='" + rdoResult.SelectedValue + "',PreviousBoard='" + txtPreviousBoard.Text + "',Email='" + Txt_Email.Text + "',DOB='" + MyUser.GetDareFromText(Txt_Dob.Text.Trim()).ToString("s") + "' where tbltempstdent.TempId='" + _TempStudId + "'";
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            UpdateCoustomFields(_TempStudId);
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Temporary Student Updation", "Details of Temporary-Student " + txt_EditName.Text + " has been updated", 2);
            Lbl_Msg.Text = "Updated Successfully";

            LoadGrid("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), int.Parse(Drp_batch.SelectedValue), int.Parse(Drp_Status.SelectedValue));
            WC_MsgBox.ShowMssage("Updated Successfully");
            Response.Redirect("ViewRegisteredStudents.aspx");

        }
        private void UpdateCoustomFields(string studentid)
        {
            int CustfieldCount = MyStudMang.CoustomFieldCount;

            if (CustfieldCount > 0)
            {
                if (MyStudMang.HaveCoustomTempStudentDetails(studentid))
                {
                    UpdateDetails(CustfieldCount, studentid);
                }
                else
                {
                    InsertCoustomDetails(CustfieldCount, studentid);
                }
            }


        }
        private void UpdateDetails(int CustfieldCount, string studentid)
        {
            int i;
            string Values = "";

            for (i = 0; i < CustfieldCount; i++)
            {
                if (i == 0)
                {
                    Values = FealdName[i] + "='" + dynamicTextBoxes[i].Text + "'";
                }
                else
                {
                    Values = Values + "," + FealdName[i] + "='" + dynamicTextBoxes[i].Text + "'";

                }


            }

            string sql = "UPDATE tbltempstudentdetails SET " + Values + " WHERE StudentId='" + studentid + "'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        }

        private bool InsertCoustomDetails(int CustfieldCount, string _userId)
        {
            bool valid = false;

            if (CustfieldCount > 0)
            {
                int i;
                string Fields, Values;
                Fields = "StudentId";
                Values = "'" + _userId + "'";
                for (i = 0; i < CustfieldCount; i++)
                {
                    Fields = Fields + "," + FealdName[i];
                    Values = Values + "," + "'" + dynamicTextBoxes[i].Text + "'";

                }
                valid = MyStudMang.InsertTempStudentDetails(Fields, Values);

            }
            else
            {
                valid = true;
            }
            return valid;
        }


        protected void Img_Export_Click(object sender, ImageClickEventArgs e)
        {
            MyDataSet = (DataSet)ViewState["TempStudentsList"];
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                MyDataSet.Tables[0].Columns.Remove(MyDataSet.Tables[0].Columns[0]);
                MyDataSet.Tables[0].Columns.Remove("CommentThreadId");
                MyDataSet.Tables[0].Columns.Remove("Class");
                //CommentThreadId
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyDataSet, "RegisteredStudentList.xls"))
                //{
                //   WC_MsgBox.ShowMssage( "This function need Ms office");
                //}
                string FileName = "RegisteredStudentList";
                string _ReportName = "RegisteredStudentList";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyDataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MsgBox.ShowMssage("MS Excel is missing. Please install");
                }
            }
            else
            {
                WC_MsgBox.ShowMssage("No students found");
            }
        }

        protected void Show_MoreInfo_Click(object sender, EventArgs e)
        {
            Panel_MoreInfo.Visible = true;
            Hide_MoreInfo.Visible = true;
            Show_MoreInfo.Visible = false;
            MPE_MessageBox.Show();
        }

        protected void Hide_MoreInfo_Click(object sender, EventArgs e)
        {
            Panel_MoreInfo.Visible = false;
            Hide_MoreInfo.Visible = false;
            Show_MoreInfo.Visible = true;
            MPE_MessageBox.Show();
        }
        protected void rdoInterView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoInterView.SelectedValue != "Attended")
                pnlInterviewDetails.Visible = false;
            else
                pnlInterviewDetails.Visible = true;
            MPE_MessageBox.Show();
        }

        protected void Btn_TempSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGrid(txt_TempName.Text.ToString().Trim(), int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), int.Parse(Drp_batch.SelectedValue), int.Parse(Drp_Status.SelectedValue));
            }
            catch
            {
            }
        }

        //protected void Lnl_show_Click(object sender, EventArgs e)
        //{
        //    if (Lnl_show.Text == "ShowList")
        //    {
        //        Pnl_TempList.Visible = true;
        //        Lnl_show.Text = "HideList";
        //    }
        //    else
        //    {
        //        Pnl_TempList.Visible = false;
        //        Lnl_show.Text = "ShowList";
        //    }
        //}


        protected void Grd_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_studentlist.PageIndex = e.NewPageIndex;
            LoadGrid("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), int.Parse(Drp_batch.SelectedValue), int.Parse(Drp_Status.SelectedValue));

        }

        private void AddClassTopopupDrp()
        {

            Drp_PopUp_Class.Items.Clear();
            if (int.Parse(Drp_PopUp_Standardlist.SelectedValue) != -1)
            {
                string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status AND tblclass.Standard='" + int.Parse(Drp_PopUp_Standardlist.SelectedValue.ToString()) + "' AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Drp_PopUp_Class.Items.Add(new ListItem("All", "0"));
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_PopUp_Class.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No Class Found", "-1");
                    Drp_PopUp_Class.Items.Add(li);
                }
            }

            else
            {
                Drp_PopUp_Class.Items.Clear();

                MyDataSet = MyUser.MyAssociatedClass();
                if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
                {
                    Drp_PopUp_Class.Items.Add(new ListItem("All", "0"));
                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                    {
                        ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_PopUp_Class.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No Class present", "-1");
                    Drp_PopUp_Class.Items.Add(li);
                }
            }
        }

        private void AddStandardToPopUpDrp()
        {
            Drp_PopUp_Standardlist.Items.Clear();
            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_PopUp_Standardlist.Items.Add(new ListItem("All Standard", "-1"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_PopUp_Standardlist.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_PopUp_Standardlist.Items.Add(li);
            }
        }

        protected void Drp_PopUpStd_SelectedIndex(object sender, EventArgs e)
        {
            AddClassTopopupDrp();
        }

        private void LoadBatchToPopUpDrpList()
        {

            Drp_batch.Items.Clear();

            string sql = "select tblbatch.BatchName ,tblbatch.Id from tblbatch where `status`=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_batch.Items.Add(new ListItem("All Batch", "-1"));
                ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                Drp_batch.Items.Add(li);
                int Nxtbatch = int.Parse(MyReader.GetValue(1).ToString()) + 1;
                sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id=" + Nxtbatch;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_batch.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Batch Found", "-1");
                Drp_batch.Items.Add(li);
            }

        }

        private void LoadGrid(string _TempStudentName, int _StdId, int _ClassId, int _Batch, int statusId)
        {
            Grd_studentlist.Columns[1].Visible = true;
            Grd_studentlist.Columns[11].Visible = true;
            //Grd_studentlist.Columns[17].Visible = true;
            // string subsql = "", innerjoinsql = "", cassname = "IFNULL( tbltempstdent.Class,'-') as Classname";
            string subsql = "", innerjoinsql = "", cassname = "IFNULL( tbltempstdent.Class,null) as Class";
            int status = 0;
            //tbltempstdent.Class as ClassName
            if (_TempStudentName != "")
            {
                subsql = subsql + " tbltempstdent.Name LIKE '" + _TempStudentName + "%'";
                status = 1;
            }
            if (int.Parse(Drp_Status.SelectedValue) > 0)
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.AdmissionStatusId=" + statusId;
                }
                else
                {
                    subsql = subsql + " tbltempstdent.AdmissionStatusId=" + statusId;
                    status = 1;
                }
            }
            if ((_StdId != 0) && (_StdId != -1))
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.Standard=" + _StdId;
                }
                else
                {
                    subsql = subsql + " tbltempstdent.Standard=" + _StdId;
                    status = 1;
                }

            }
            if ((_ClassId != 0) && (_ClassId != -1))
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.Class=" + _ClassId;
                }
                else
                {
                    subsql = subsql + " tbltempstdent.Class=" + _ClassId;
                    status = 1;
                }

                innerjoinsql = innerjoinsql + " inner join tblclass on tblclass.Id = tbltempstdent.Class ";
                cassname = "tblclass.ClassName";
            }
            if ((_Batch != 0) && (_Batch != -1))
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.JoiningBatch=" + _Batch;
                }
                else
                {
                    subsql = subsql + " tbltempstdent.JoiningBatch=" + _Batch;
                    status = 1;
                }

            }
            if (Rdb_Type.SelectedValue == "0")
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.`Status`=1";
                }
                else
                {
                    subsql = subsql + " tbltempstdent.`Status`=1";
                    status = 1;
                }

            }
            else
            {
                if (status == 1)
                {
                    subsql = subsql + " and tbltempstdent.`Status`=0";
                }
                else
                {
                    subsql = subsql + " tbltempstdent.`Status`=0";
                    status = 1;
                }

            }
            //tbltempstdent.PhoneNumber,
            string sql = "select tbltempstdent.Id, tbltempstdent.Name , tbltempstdent.TempId , tbltempstdent.Gender ,tbltempstdent.Fathername, tbltempstdent.Address,tbltempstdent.MotherName, tbltempstdent.Pin, tbltempstdent.State, tbltempstdent.Nationality, tblbloodgrp.GroupName,(select tbllanguage.Language from tbllanguage where tbllanguage.Id= tbltempstdent.MotherTongue)as MotherTongue, tbltempstdent.FatherEduQualification, tbltempstdent.MotherEduQualification, tbltempstdent.FatherOccupation, tbltempstdent.AnnualIncome, tbltempstdent.UseBus, tbltempstdent.UseHostel, tbltempstdent.MotherOccupation,  tblstandard.Name as standard,IFNULL( tbltempstdent.Class,null) as Class,tbltempstdent.Rank , (select tblbatch.BatchName from tblbatch where tblbatch.Id= tbltempstdent.JoiningBatch) as Batch, tbl_admissionstatus.`Status`,CommentThreadId,tbltempstdent.PhoneNumber,tbltempstdent.Email as EmailId from tbltempstdent  inner join tblstandard on tblstandard.Id = tbltempstdent.Standard inner join tblbloodgrp on tblbloodgrp.Id = tbltempstdent.BloodGroup inner join tbl_admissionstatus on tbl_admissionstatus.Id= tbltempstdent.AdmissionStatusId" + innerjoinsql + " where " + subsql + "  order by rank";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {

                if ((_ClassId == 0))
                {
                    MyDataSet = GetDetaildDs(MyDataSet);
                }
                else
                {
                    MyDataSet = GetDetailedDs(MyDataSet);
                }
                Img_Export.Visible = true;
                Grd_studentlist.DataSource = MyDataSet;
                Grd_studentlist.DataBind();
                Lbl_StudentCount.Text = "Total Students:" + Grd_studentlist.Rows.Count + "";
                lbl_TempError.Visible = false;
                ViewState["TempStudentsList"] = MyDataSet;
                if (Rdb_Type.SelectedValue == "0")
                {
                    Grd_studentlist.Columns[9].Visible = true;
                    //Grd_studentlist.Columns[11].Visible = true;
                    //Grd_studentlist.Columns[13].Visible = true;
                }
                else
                {
                    Grd_studentlist.Columns[9].Visible = false;
                    //Grd_studentlist.Columns[11].Visible = false;
                    //Grd_studentlist.Columns[13].Visible = false;
                }
            }
            else
            {
                Img_Export.Visible = false;
                Lbl_StudentCount.Text = "";
                Grd_studentlist.DataSource = null;
                Grd_studentlist.DataBind();
                lbl_TempError.Visible = true;
                lbl_TempError.Text = "No Student Exists";
            }
            Grd_studentlist.Columns[1].Visible = false;
            Grd_studentlist.Columns[11].Visible = false;
            //Grd_studentlist.Columns[17].Visible = false;
            if ((Drp_batch.SelectedValue != "-1") && (Drp_PopUp_Standardlist.SelectedValue != "-1") && (Drp_PopUp_Standardlist.SelectedValue != "0") && MyUser.HaveActionRignt(3))
            {
                Img_AltImage.Visible = true;
                Lnk_Allotment.Visible = true;
            }
            else
            {
                Lnk_Allotment.Visible = false;
                Img_AltImage.Visible = false;
            }
        }

        private DataSet GetDetaildDs(DataSet MyDataSet)
        {
            MyDataSet.Tables[0].Columns.Add("ClassName");
            OdbcDataReader Classreader = null;
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                string sql = "";
                if (dr["Class"].ToString() != "")
                {
                    sql = "select tblclass.ClassName from tblclass where tblclass.Id=" + int.Parse(dr["Class"].ToString()) + "";
                    Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (Classreader.HasRows)
                    {
                        dr["ClassName"] = Classreader.GetValue(0).ToString();
                    }
                }
                else
                {
                    dr["ClassName"] = "";
                }
            }
            return MyDataSet;
        }

        private DataSet GetDetailedDs(DataSet MyDataSet)
        {
            MyDataSet.Tables[0].Columns.Add("ClassName");
            OdbcDataReader Classreader = null;
            foreach (DataRow dr in MyDataSet.Tables[0].Rows)
            {
                string sql = "";
                if (dr["Class"].ToString() != "")
                {
                    sql = "select tblclass.ClassName from tblclass where tblclass.Id=" + int.Parse(dr["Class"].ToString()) + "";
                    Classreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (Classreader.HasRows)
                    {
                        dr["ClassName"] = Classreader.GetValue(0).ToString();
                    }
                }
                else
                {
                    dr["ClassName"] = "";
                }
            }
            return MyDataSet;
        }


        #endregion


        protected void Drp_EditStd_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Std = 0;
            AddEditClassTopopupDrp(out Std);
            Drp_EditClass.Focus();
            MPE_MessageBox.Show();
        }

        private void AddEditClassTopopupDrp(out int Std)
        {
            OdbcDataReader MyReader1 = null;
            ListItem li = new ListItem();
            Drp_EditClass.Items.Clear();
            Std = 0;
            if (Drp_EditStd.SelectedValue != "")
            {
                string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status AND tblclass.Standard='" + int.Parse(Drp_EditStd.SelectedValue.ToString()) + "' AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                MyReader1 = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader1.HasRows)
                {
                    Btn_magok.Enabled = true;
                    li = new ListItem("Select Class", "0");
                    Drp_EditClass.Items.Add(li);
                    while (MyReader1.Read())
                    {
                        li = new ListItem(MyReader1.GetValue(1).ToString(), MyReader1.GetValue(0).ToString());
                        Drp_EditClass.Items.Add(li);
                    }
                }
                else
                {
                    Std = 1;
                    li = new ListItem("No Class Found", "-1");
                    Drp_EditClass.Items.Add(li);
                    Btn_magok.Enabled = false;


                }
            }
        }

        protected void Grd_studentlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Hdn_DeleteTempId.Value = Grd_studentlist.Rows[e.NewEditIndex].Cells[3].Text;
            MPE_DeleteStudent.Show();
        }

        protected void Btn_DeleteStudent_Click(object sender, EventArgs e)
        {
            string studentname = "";
            OdbcDataReader studentreader = null;
            string sql = "select tbltempstdent.Name from tbltempstdent where tbltempstdent.TempId='" + Hdn_DeleteTempId.Value + "'";
            studentreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if(studentreader.HasRows)
            {
                studentname = studentreader.GetValue(0).ToString();
            }
            sql = "update tbltempstdent set tbltempstdent.`Status`=0,AdmissionStatusId=7 where tbltempstdent.TempId='" + Hdn_DeleteTempId.Value + "'";
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Temporary Student Deletion", "Temporary Student " + studentname + " has been deleted", 1, 1);

            LoadGrid("", 0, 0, 0, int.Parse(Drp_Status.SelectedValue));
        }

        protected void Grd_studentlist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewStudent")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Session["ClassId"] = null;
                string sql = "select tbltempstdent.Class from tbltempstdent where tbltempstdent.TempId='" + Grd_studentlist.Rows[index].Cells[3].Text + "'";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Session["StudId"] = Grd_studentlist.Rows[index].Cells[3].Text;
                    Session["ClassId"] = MyReader.GetValue(0).ToString();
                }
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"RegisteredStudentDetails.aspx?TempStudId=" + Grd_studentlist.Rows[index].Cells[3].Text + "&ClassId=" + MyReader.GetValue(0).ToString() + "\" , 'popupwindow', 'width=600,height=500,resizable');", true);
                //Response.Redirect("RegisteredStudentDetails.aspx?TempStudId=" + Grd_studentlist.Rows[index].Cells[0].Text);
            }
            if (e.CommandName == "Enroll")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                OdbcDataReader Xmlreader = null;
                string TempId = Grd_studentlist.Rows[index].Cells[1].Text;
                string TemIdForXml = Grd_studentlist.Rows[index].Cells[3].Text;
                Session["TempId"] = TemIdForXml.ToString();
                string sql = "select tbl_xmlstring.Id, tbl_xmlstring.XMLString from tbl_xmlstring  where tbl_xmlstring.TempId='" + TemIdForXml + "'";
                Xmlreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (Xmlreader.HasRows)
                {

                    XmlDocument doc = new XmlDocument();
                    string StrXml = Xmlreader.GetValue(1).ToString();
                    DataSet ReaderDs = new DataSet();
                    doc.LoadXml(StrXml);
                    XmlNodeReader Nodereader = new XmlNodeReader(doc);
                    Nodereader.MoveToContent();
                    while (Nodereader.Read())
                    {
                        ReaderDs.ReadXml(Nodereader);

                    }
                    Session["ViewDs"] = ReaderDs;

                    if (ReaderDs != null && ReaderDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ReaderDs.Tables[0].Rows)
                        {
                            if (dr["ColumnName"].ToString() == "BatchId")
                            {
                                int BatchID = int.Parse(dr["Name"].ToString());
                                if (BatchID == MyUser.CurrentBatchId)
                                {
                                    Response.Redirect("CreateNewStudent.aspx?");
                                }
                                else
                                {
                                    WC_MsgBox.ShowMssage("Admission for next batch,Cannot enroll this student!");
                                    Session["ViewDs"] = null;
                                    Session["TempId"] = null;
                                }
                            }
                        }
                    }

                }
                else
                {
                    string _sql = "";
                    OdbcDataReader BatchIdReader = null;
                    _sql = "Select tbltempstdent.JoiningBatch from tbltempstdent where Id='" + TempId + "'";
                    BatchIdReader = MyStudMang.m_MysqlDb.ExecuteQuery(_sql);
                    if (BatchIdReader.HasRows)
                    {
                        if (int.Parse(BatchIdReader.GetValue(0).ToString()) == MyUser.CurrentBatchId)
                        {
                            Response.Redirect("CreateNewStudent.aspx?TempStudId=" + Grd_studentlist.Rows[index].Cells[1].Text);
                        }
                        else
                        {
                            WC_MsgBox.ShowMssage("Admission for next batch,Cannot enroll this student!");
                            Session["ViewDs"] = null;
                            Session["TempId"] = null;
                        }
                    }


                }
            }
        }

        protected void ViewAloment_Click(object sender, EventArgs e)
        {
            int Batch = int.Parse(Drp_batch.SelectedValue);
            int ClassId = int.Parse(Drp_PopUp_Class.SelectedValue);
            int StdID = int.Parse(Drp_PopUp_Standardlist.SelectedValue);
            if (ClassId > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ViewAllotements.aspx?Batch=" + Batch + "&ClassId=" + ClassId + "&StandardID=" + StdID + "\", 'Info','width=1100,height=600,scrollbars,resizable');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ViewAllotements.aspx?Batch=" + Batch + "&StandardID=" + StdID + "\", 'Info','width=1100,height=600,scrollbars,resizable');", true);
            }

        }

        protected void Img_AltImage_Click(object sender, ImageClickEventArgs e)
        {
            int Batch = int.Parse(Drp_batch.SelectedValue);
            int ClassId = int.Parse(Drp_PopUp_Class.SelectedValue);
            int StdID = int.Parse(Drp_PopUp_Standardlist.SelectedValue);
            if (ClassId > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ViewAllotements.aspx?Batch=" + Batch + "&ClassId=" + ClassId + "&StandardID=" + StdID + "\", 'Info','width=1100,height=600,scrollbars,resizable');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ViewAllotements.aspx?Batch=" + Batch + "&StandardID=" + StdID + "\", 'Info','width=1100,height=600,scrollbars,resizable');", true);
            }
        }


        protected void Btn_ChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox Chk = new CheckBox();
                DataSet TempDs = new DataSet();
                Btn_SendMail.Visible = false;
                Btn_SendSms.Visible = false;
                Lbl_Msg.Text = "";
                Lbl_ErrMsg.Text = "";
                int chkCount = 0;
                foreach (GridViewRow gr in Grd_studentlist.Rows)
                {
                    Chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    if (Chk.Checked)
                    {
                        chkCount++;

                    }
                }
                if (chkCount == 0)
                {
                    Lbl_Msg.Text = "Select any student";
                }
                else
                {
                    LoadStatusToUpdateDropDown();
                    MPE_changeStatus.Show();
                }
            }
            catch
            {
            }
        }

        private void LoadStatusToUpdateDropDown()
        {
            DataSet StatusDs = new DataSet();
            Drp_ChangeStatus.Items.Clear();
            ListItem li;
            StatusDs = MyStudMang.GetAllStatus();
            if (StatusDs != null && StatusDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("ALL", "0");
                Drp_ChangeStatus.Items.Add(li);
                foreach (DataRow dr in StatusDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Status"].ToString().ToUpper(), dr["Id"].ToString());
                    Drp_ChangeStatus.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_ChangeStatus.Items.Add(li);
            }
        }


        protected void Btn_UpdateStatus_Click(object sender, EventArgs e)
        {
            int status = int.Parse(Drp_ChangeStatus.SelectedValue);
            string Msg = "";
            CheckBox Chk = new CheckBox();
            Lbl_ErrMsg.Text = "";
            int Success = 0;
            int chkCount = 0;

            if (status > 0)
            {
                try
                {
                    MyStudMang.m_MysqlDb.MyBeginTransaction();
                    foreach (GridViewRow gr in Grd_studentlist.Rows)
                    {
                        Chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                        if (Chk.Checked)
                        {
                            chkCount++;
                            DataSet TempDS = new DataSet();
                            string TempId = gr.Cells[3].Text;
                            Success = MyStudMang.UpdateStatusinViewStudents(TempId, status, out Msg);
                            if (Success == 1)
                            {
                                MyStudMang.m_MysqlDb.TransactionRollback();
                                WC_MsgBox.ShowMssage(Msg);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Success = 1;
                    MyStudMang.m_MysqlDb.TransactionRollback();
                    WC_MsgBox.ShowMssage(Msg);
                }
                if (chkCount == 0)
                {
                    MyStudMang.m_MysqlDb.TransactionRollback();
                    Lbl_ErrMsg.Text = "Select any student";
                }
                else if (Success == 0)
                {
                    MyStudMang.m_MysqlDb.TransactionCommit();
                    Lbl_ErrMsg.Text = "Updated Successfully";
                    LoadGrid("", int.Parse(Drp_PopUp_Standardlist.SelectedValue), int.Parse(Drp_PopUp_Class.SelectedValue), int.Parse(Drp_batch.SelectedValue), int.Parse(Drp_Status.SelectedValue));
                    MPE_changeStatus.Show();

                }


            }
            else
            {
                Lbl_ErrMsg.Text = "Select status";
                MPE_changeStatus.Show();

            }

        }

        protected void Drp_ChangeStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_ChangeStatus.SelectedValue) == 3)
            {
                Btn_SendMail.Visible = true;
                Btn_SendSms.Visible = false;

            }
            else
            {
                Btn_SendMail.Visible = false;
                Btn_SendSms.Visible = false;
            }
            MPE_changeStatus.Show();
        }

        protected void Img_Comment_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)(sender as ImageButton).Parent.Parent;
            int row = currentRow.RowIndex;
            string ComntType = "TempStudent";
            ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"CommentPannel.aspx?CommentId=" + int.Parse(Grd_studentlist.Rows[row].Cells[11].Text) + "&Type=" + ComntType + "&UserId=" + int.Parse(Grd_studentlist.Rows[row].Cells[1].Text) + "\" , 'popupwindow', 'width=600,height=500,resizable');", true);
        }

        protected void Btn_SendMail_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
        }

        protected void Btn_SendEmail_Click(object sender, EventArgs e)
        {
            int Success = 0, chkCount = 0;
            string Msg = "";
            try
            {
                if (Txt_EmailSubject.Text != "" && Editor_Body.Content != "")
                {
                    MyStudMang.m_MysqlDb.MyBeginTransaction();
                    CheckBox Chk = new CheckBox();
                    foreach (GridViewRow gr in Grd_studentlist.Rows)
                    {
                        Chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                        if (Chk.Checked)
                        {
                            chkCount++;
                            DataSet TempDS = new DataSet();
                            string TempId = gr.Cells[3].Text;
                            string Id = gr.Cells[1].Text;
                            Success = MyStudMang.UpdateStatusinViewStudents(TempId, 3, out Msg);
                            SendMail(Id);

                        }
                    }
                }
                else
                {
                    Success = 1;
                    WC_MsgBox.ShowMssage("Enter email subject and body");
                }
            }
            catch (Exception ex)
            {
                Success = 1;
                MyStudMang.m_MysqlDb.TransactionRollback();
                WC_MsgBox.ShowMssage("Cannot send mail.Please try again later");
            }
            if (Success == 0)
            {
                MyStudMang.m_MysqlDb.TransactionCommit();
                WC_MsgBox.ShowMssage("Mail has been sent successfully");
                Txt_EmailSubject.Text = "";
                Editor_Body.Content = "";
            }
            ModalPopupExtender1.Show();
        }

        private void SendMail(string Id)
        {

            OdbcDataReader emailreader = null;
            string sql = "";
            string email = "";
            sql = " select tbltempstdent.Email from tbltempstdent WHERE tbltempstdent.Id='" + Id + "'";
            emailreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (emailreader.HasRows)
            {

                email = emailreader.GetValue(0).ToString().Replace("&nbsp;", "");
                if (email != "")
                {
                    MyStudMang.InsertDataToAutoEmailList(email, Id, Txt_EmailSubject.Text, Editor_Body.Content.Replace("'", "").Replace("\\", ""), 4);
                }
            }
        }

        protected void Btn_SendMessage_Click(object sender, EventArgs e)
        {
            CheckBox Chk = new CheckBox();
            bool check = false;
            string phno = "", seperator = "";
            foreach (GridViewRow gr in Grd_studentlist.Rows)
            {
                Chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                if (Chk.Checked)
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                Load_DrpTamplate();
                Txt_Message.Text = "";
                Lbl_Messageerr.Text = "";
                Mpe_SendMEssage.Show();
            }
            else
            {
                Lbl_Msg.Text = "Please select any student";
            }

        }


        protected void Btn_MessageSend_Click(object sender, EventArgs e)
        {
            string phonelist = "";
            string msg = "";
            Lbl_Msg.Text = "";
            char symbol = MysmsMang.GetSMS_NumberSeperator_FromDatabase();
            if (Data_Complete(out msg))
            {

                phonelist = GetPhoneNumber();

                if (phonelist != "")
                {
                    //dominic sms
                    string failedList = "";
                    if (MysmsMang.SendBULKSms(phonelist, Txt_Message.Text, "90366450445", "WINER", true, out  failedList))
                    {
                        Lbl_Messageerr.Text = "SMS Circular has been Sent Successfully";

                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "SMS Circular", "Message : " + Txt_Message.Text, 1,1);
                        //  clearAll();
                    }
                    else
                    {
                        Lbl_Messageerr.Text = "Error In SMS Circulation";
                    }
                    //string _msg = "";
                    //if (SentAllMessages(phonelist, Txt_Message.Text,out _msg))
                    //{
                    //    MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Circular", "Message : " + Txt_Message.Text, 1);
                    //}
                }
                else
                {
                    Lbl_Messageerr.Text = "Number not available for sending the message";
                }

            }
            else
            {
                Lbl_Messageerr.Text = msg;
            }
            Mpe_SendMEssage.Show();


        }

        private string GetPhoneNumber()
        {
            CheckBox Chk = new CheckBox();
            string phno = "", seperator = "";
            foreach (GridViewRow gr in Grd_studentlist.Rows)
            {
                Chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                if (Chk.Checked)
                {
                    phno = phno + seperator + gr.Cells[10].Text.Replace("&nbsp;", "");
                    seperator = ",";
                }

            }
            return phno;
        }

        private void Load_DrpTamplate()
        {
            Drp_Template.Items.Clear();
            ListItem li;
            DataSet MydataSet = new DataSet();
            string sql = "SELECT Id,Name FROM tblsmstemplate WHERE SelectType=1";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select", "-1");
                Drp_Template.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Template.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Template Found", "-1");
                Drp_Template.Items.Add(li);
            }
            Drp_Template.SelectedIndex = 0;

        }



        private bool Data_Complete(out string msg)
        {
            bool valid = true;
            msg = "";

            if (Txt_Message.Text.Trim() == "")
            {
                msg = "Enter SMS Message";
                valid = false;
            }
            else if (Txt_Message.Text.Trim().Contains("($") || Txt_Message.Text.Trim().Contains("$)"))
            {
                msg = "Before sending, replace symbol ($ $) parts with correct data in template";
                valid = false;
            }
            else if (Txt_Message.Text.Trim().Contains("&"))
            {
                msg = "SMS message should avoid symbol '&'. Message with symbol '&' will not be sent";
                valid = false;
            }
            return valid;
        }


        private void Load_SelectedTemplate()
        {
            Txt_Message.Text = "";
            if (Drp_Template.SelectedValue != "-1")
            {
                Txt_Message.Text = MysmsMang.GetSMSTemplate(Drp_Template.SelectedValue);
            }
        }

        protected void Drp_Template_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_SelectedTemplate();
            Mpe_SendMEssage.Show();
        }


    }
}
