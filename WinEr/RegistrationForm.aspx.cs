using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.Text;
using System.IO;
using System.Configuration;

namespace WinEr
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        public RegistrationClass ObjReg;
        private KnowinUser MyUser;

        string AbsoluteFilePath = ConfigurationSettings.AppSettings["OnlineStudentFilePath"];
        #region Events
        string tempId="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            ObjReg = new RegistrationClass(WinerUtlity.SingleSchoolConnectionString);
            if (ObjReg == null)
            {
                Response.Redirect("default.aspx");
            }
            if (!IsPostBack)
            {
                LoadValues();
                RownextBatch.Visible = false;
                if (Request.QueryString["TempId"] != null)
                {
                    DataSet StudentDs = new DataSet();
                    StudentDs = (DataSet)Session["ViewDs"];
                    tempId = Request.QueryString["TempId"].ToString();
                    if (StudentDs != null && StudentDs.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in StudentDs.Tables[0].Rows)
                        {
                            if (dr["ColumnName"].ToString() == "StudentName")
                            {
                                Txt_StudentName.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Dob")
                            {
                                Txt_Dob.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Age")
                            {
                                Txt_Age.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "PemanentAddress")
                            {
                                Txt_PermanentAddress.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "Presentaddress")
                            {
                                Txt_PresentAddress.Text = dr["Name"].ToString();

                            }
                            if (dr["ColumnName"].ToString() == "MobileNum")
                            {
                                Txt_MobileNumber.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "FathersEmaidID")
                            {
                                Txt_EmailId.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Standard")
                            {
                                Drp_standard.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "BatchId")
                            {
                                int BatchId = int.Parse(dr["Name"].ToString());

                                if (BatchId == MyUser.CurrentBatchId)
                                {
                                    Chk_CurrentBatch.Checked = true;
                                    RownextBatch.Visible = false;
                                }
                                else
                                {
                                    Chk_CurrentBatch.Checked = false;
                                    RownextBatch.Visible = true;
                                }

                            }
                            if (dr["ColumnName"].ToString() == "Gender")
                            {
                                Rbd_Gender.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "BloodGroup")
                            {
                                Drp_BloodGroup.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Nationality")
                            {
                                Txt_Nationality.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "MotherTongue")
                            {
                                Drp_MotherTongue.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "DrpReligion")
                            {
                                Drp_religion.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "OthrRelgn")
                            {
                                Txt_OtherReligion.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Caste")
                            {
                                Drp_Caste.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "GuardianName")
                            {
                                Txt_GuardianName.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "FathersOccupation")
                            {
                                txt_FatherProfession.Text = dr["Name"].ToString();

                            }
                            if (dr["ColumnName"].ToString() == "FathersAnnualIncome")
                            {
                                txt_AnnualIncome.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "FathersOfficeAddredd")
                            {
                                txt_FathersOffcaddress.Text = dr["Name"].ToString();
                            }


                            if (dr["ColumnName"].ToString() == "MothrName")
                            {
                                Txt_MotherName.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "MothersOccupation")
                            {
                                Txt_MothersProfession.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "MothersAnnualIncomne")
                            {
                                txt_mothersIncome.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "MothersEmaidID")
                            {
                                txt_Mothersemail.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "MothersOfficeAddress")
                            {
                                Txt_MothersOffcAddress.Text = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "State")
                            {
                                Txt_State.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Location")
                            {
                                Txt_Location.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Pin")
                            {
                                Txt_pin.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "ResidencePhno")
                            {
                                Txt_ResidencePhNo.Text = dr["Name"].ToString();
                            }
                            //if (dr["ColumnName"].ToString() == "FthrQlfn")
                            //{
                            //    Txt_FatherEduQualification.Text = dr["Name"].ToString();
                            //}
                            //if (dr["ColumnName"].ToString() == "MthrQlfn")
                            //{
                            //    Txt_MotherEduQualification.Text = dr["Name"].ToString();
                            //}

                            //if (dr["ColumnName"].ToString() == "BrotherNum")
                            //{
                            //    Txt_NumBrother.Text = dr["Name"].ToString();
                            //}
                            //if (dr["ColumnName"].ToString() == "SisterNum")
                            //{
                            //    Txt_NumSister.Text = dr["Name"].ToString();
                            //}
                            if (dr["ColumnName"].ToString() == "PlaceOfBirth")
                            {
                                Txt_PlaceOfBirth.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "IdentificationMark")
                            {
                                Txt_Identificationmark.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LastClassRemarks")
                            {
                                Txt_LastClassRemarks.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LeavingDate")
                            {
                                Txt_LeavingDate.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LastClass")
                            {
                                Txt_LastClass.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LeavingReason")
                            {
                                Txt_ReasonLeaving.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "TCDate")
                            {
                                Txt_TcDate.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "TCNum")
                            {
                                Txt_TcNumber.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LastSchoolAddress")
                            {
                                Txt_LastSchoolAddress.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "LastSchollName")
                            {
                                Txt_LastSchoolName.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Category")
                            {
                                Rbd_Category.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "Category")
                            {
                                Rbd_Category.SelectedValue = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "IsSiblingExist")
                            {
                                //if (dr["Name"].ToString() == "0")
                                //    rdo_sibling.SelectedIndex = 0;
                                //else
                                //    rdo_sibling.SelectedIndex = 1;
                                rdo_sibling.SelectedValue = dr["Name"].ToString();
                            }

                            if (dr["ColumnName"].ToString() == "SiblingAdmissionNum")
                            {
                                Txt_SiblingAdmissionNo.Text = dr["Name"].ToString();
                            }
                            if (dr["ColumnName"].ToString() == "TransportMode")
                            {
                                rdo_transport.SelectedValue = dr["Name"].ToString();

                                //if (dr["Name"].ToString() == "0")
                                //    rdo_sibling.SelectedValue = "0";
                                //else if (dr["Name"].ToString() == "1")
                                //    rdo_sibling.SelectedValue = "1";
                                //else if (dr["Name"].ToString() == "2")
                                //    rdo_sibling.SelectedValue = "2";

                            }

                        }
                        LoadPhoto();

                    }
                }

            }

        }


            private void LoadPhoto()
            {
                DataSet Dt = ObjReg.GetStudentDetailsfromTempId(tempId.ToString());
                StringBuilder sb = new StringBuilder();
                if (Dt != null && Dt.Tables[0].Rows.Count > 0)
                    foreach (DataRow dr in Dt.Tables[0].Rows)
                    {

                        if (AbsoluteFilePath == "")

                            sb.Append("<img src=\"ThumbnailImages/" + dr["StudentURL"].ToString() + "\" width=\"60px\" />");

                        else

                            sb.Append("<img src=\"" + AbsoluteFilePath + dr["StudentURL"].ToString() + "\" width=\"60px\" />");

                    }
                Photo.InnerHtml = sb.ToString();
            }

            protected void Drp_religion_SelectedIndexChanged(object sender, EventArgs e)
            {
                CheckIfOtherReligion();
                Drp_Caste.Focus();
            }

            protected void Img_Save_Click(object sender, ImageClickEventArgs e)
            {
                int BatchId = 0;
                int Rank = 0;
                string _TempId = Request.QueryString["TempId"].ToString();
                string studentName = Txt_StudentName.Text;
                DateTime Dob = General.GetDateTimeFromText(Txt_Dob.Text);
                string MobileNumber = Txt_MobileNumber.Text;
                string emailId = Txt_EmailId.Text;
                string GuardianName = Txt_GuardianName.Text;
                string GenderValue = Rbd_Gender.SelectedItem.Text;
                int standard = int.Parse(Drp_standard.SelectedValue);
                string Address = Txt_PermanentAddress.Text;
                if (Chk_CurrentBatch.Checked == true)
                {
                    BatchId = MyUser.CurrentBatchId;
                }
                else
                {
                    BatchId = MyUser.CurrentBatchId;
                    BatchId = BatchId + 1;

                }
                ObjReg.UpdateStudentDetails(studentName, Dob, MobileNumber, emailId, GuardianName, GenderValue, standard, Address, BatchId, _TempId);
                //Lbl_Message.Text = "Registered successfully,Login details will send to your email id";
                WC_MessageBox.ShowMssage("Updated successfully");
                //string XmlString= CreateXML(_TempId);
                DataSet StudentDetailsDs = new DataSet();
                StudentDetailsDs = GetstudentDs();
                string _StudentXmlString = CreateStudentXml(_TempId, StudentDetailsDs);
                ObjReg.UpdateXmlTextToTable(_TempId, _StudentXmlString);
            }

            protected void Chk_CurrentBatch_CheckedChanged(object sender, EventArgs e)
            {
                if (Chk_CurrentBatch.Checked == false)
                {
                    RownextBatch.Visible = true;
                    LoadBatch();
                }
                else
                {
                    RownextBatch.Visible = false;
                }
                if (Chk_CurrentBatch.Checked == true)
                {
                    Hdn_BatchId.Value = MyUser.CurrentBatchId.ToString();
                }
                else
                {
                    Hdn_BatchId.Value = Drp_NextBatch.SelectedValue;
                }
            }


        #endregion

            #region Methods

            private void ClearAll()
            {
                Txt_Dob.Text = "";
                //Txt_AnualIncome.Text = "";
                Txt_EmailId.Text = "";
                //Txt_FatherEduQualification.Text = "";
                //Txt_FatherOccupation.Text = "";
                Txt_GuardianName.Text = "";
                Txt_Identificationmark.Text = "";
                Txt_LastClass.Text = "";
                Txt_LastClassRemarks.Text = "";
                Txt_LastSchoolAddress.Text = "";
                Txt_LastSchoolName.Text = "";
                Txt_LeavingDate.Text = "";
                Txt_Location.Text = "";
                Txt_MobileNumber.Text = "";
                //Txt_MotherEduQualification.Text = "";
                Txt_MotherName.Text = "";
                Txt_Nationality.Text = "";
                //Txt_NumBrother.Text = "";
                //Txt_NumSister.Text = "";
                Txt_OtherReligion.Text = "";
                Txt_PermanentAddress.Text = "";
                Txt_pin.Text = "";
                Txt_PlaceOfBirth.Text = "";
                Txt_PresentAddress.Text = "";
                Txt_ReasonLeaving.Text = "";
                Txt_ResidencePhNo.Text = "";
                Txt_StudentName.Text = "";
                Txt_TcDate.Text = "";
                Txt_TcNumber.Text = "";
                Txt_SiblingAdmissionNo.Text = "";
                txt_FatherProfession.Text = "";
                txt_FathersOffcaddress.Text = "";
                txt_Mothersemail.Text = "";

                txt_mothersIncome.Text = "";
                Txt_MothersOffcAddress.Text = "";
                Txt_MothersProfession.Text = "";

            }

            private void LoadValues()
            {
                LoadReligion(0);
                CheckIfOtherReligion();
                LoadCast();
                LoadBloodGroup(16);
                LoadMotherTongue(0);
                LoadStandard(0);
                LoadBatch();
                ClearAll();


            }

            private void LoadBatch()
            {
                DataSet CurrentBatchDs = null;
                Drp_NextBatch.Items.Clear();
                ListItem li = new ListItem();
                int CurrentBatchId = 0;
                CurrentBatchDs = ObjReg.GetCurrentBatch();
                if (CurrentBatchDs != null && CurrentBatchDs.Tables[0].Rows.Count > 0)
                {
                    Hdn_BatchId.Value = CurrentBatchDs.Tables[0].Rows[0]["Id"].ToString();
                    CurrentBatchId = int.Parse(CurrentBatchDs.Tables[0].Rows[0]["Id"].ToString());
                }
                DataSet NextBatchDs = new DataSet();
                NextBatchDs = ObjReg.GetNextBatch(CurrentBatchId);
                if (NextBatchDs != null && NextBatchDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in NextBatchDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["BatchName"].ToString(), dr["Id"].ToString());
                        Drp_NextBatch.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_NextBatch.Items.Add(li);
                }
            }

            private void LoadStandard(int _index)
            {
                DataSet StandardDs = new DataSet();
                Drp_standard.Items.Clear();
                ListItem li = new ListItem();
                StandardDs = ObjReg.GetStandard();
                if (StandardDs != null && StandardDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in StandardDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Name"].ToString(), dr["Id"].ToString());
                        Drp_standard.Items.Add(li);
                    }
                    Drp_standard.SelectedIndex = _index;

                }
                else
                {
                    li = new ListItem("No Standard Found", "-1");
                    Drp_standard.Items.Add(li);
                }

            }

            private void LoadMotherTongue(int _index)
            {

                DataSet MotherTongueDs = new DataSet();
                Drp_MotherTongue.Items.Clear();
                ListItem li = new ListItem();
                MotherTongueDs = ObjReg.GetMotherTongue();
                if (MotherTongueDs != null && MotherTongueDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in MotherTongueDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Language"].ToString(), dr["Id"].ToString());
                        Drp_MotherTongue.Items.Add(li);
                    }
                    Drp_MotherTongue.SelectedIndex = _index;
                }
            }

            private void LoadBloodGroup(int _index)
            {
                DataSet BloodGroupDs = new DataSet();
                Drp_BloodGroup.Items.Clear();
                ListItem li = new ListItem();
                BloodGroupDs = ObjReg.GetBloodGroup();
                if (BloodGroupDs != null && BloodGroupDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in BloodGroupDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["GroupName"].ToString(), dr["Id"].ToString());
                        Drp_BloodGroup.Items.Add(li);
                    }
                    Drp_BloodGroup.SelectedIndex = _index;
                }
            }

            private void LoadCast()
            {
                DataSet CasteDs = new DataSet();
                Drp_Caste.Items.Clear();
                ListItem li = new ListItem();
                CasteDs = ObjReg.GetCaste();
                if (CasteDs != null && CasteDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in CasteDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["castname"].ToString(), dr["Id"].ToString());
                        Drp_Caste.Items.Add(li);
                    }

                }
            }

            private void LoadReligion(int _index)
            {
                DataSet ReligionDs = new DataSet();
                Drp_religion.Items.Clear();
                ListItem li = new ListItem();
                ReligionDs = ObjReg.GetReligion();
                if (ReligionDs != null && ReligionDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ReligionDs.Tables[0].Rows)
                    {
                        li = new ListItem(dr["Religion"].ToString(), dr["Id"].ToString());
                        Drp_religion.Items.Add(li);
                    }
                    Drp_religion.SelectedIndex = _index;
                }
                Drp_religion.Items.Add(new ListItem("Others", "-1"));

            }

            private void CheckIfOtherReligion()
            {
                if (Drp_religion.SelectedValue == "-1")
                {
                    RowOtherReligion.Visible = true;
                }
                else
                {
                    RowOtherReligion.Visible = false;

                }

            }

            private DataSet GetstudentDs()
            {
                DataSet StudentDetailsDs = new DataSet();
                DataTable dt;
                DataRow dr;
                StudentDetailsDs.Tables.Add("StudentDetails");
                dt = StudentDetailsDs.Tables["StudentDetails"];

                dt.Columns.Add("Name");
                dt.Columns.Add("DataType");
                dt.Columns.Add("DefaultValue");
                dt.Columns.Add("Ismandatory");
                dt.Columns.Add("WinErDataField");
                dt.Columns.Add("WinErDatatable");
                dt.Columns.Add("ColumnName");


                dr = dt.NewRow();
                dr["Name"] = Txt_StudentName.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "StudentName";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "StudentName";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_Dob.Text;
                dr["DataType"] = "Date";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "DOB";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Dob";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_Age.Text;
                dr["DataType"] = "Int";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "Age";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = Txt_PlaceOfBirth.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "PlaceOfBirth";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_PermanentAddress.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "false";
                dr["WinErDataField"] = "Address";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "PemanentAddress";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_PresentAddress.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Addresspresent";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Presentaddress";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_ResidencePhNo.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "ResidencePhno";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "ResidencePhno";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);
                dr = dt.NewRow();
                dr["Name"] = Txt_MobileNumber.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "ResidencePhNo";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MobileNum";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);




                dr = dt.NewRow();
                dr["Name"] = Drp_standard.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "JoinStandard";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Standard";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);



                dr = dt.NewRow();
                int BatchId;
                if (Chk_CurrentBatch.Checked == true)
                {
                    BatchId = int.Parse(Hdn_BatchId.Value);
                }
                else
                {
                    BatchId = int.Parse(Drp_NextBatch.SelectedValue);
                }
                dr["Name"] = BatchId.ToString();
                dr["DataType"] = "int";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "JoinBatch";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "BatchId";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = Rbd_Gender.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Sex";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Gender";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);



                dr = dt.NewRow();
                dr["Name"] = Drp_BloodGroup.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "BloodGroup";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "BloodGroup";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_Nationality.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Nationality";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Nationality";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Drp_MotherTongue.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "MotherTongue";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MotherTongue";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Drp_religion.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Religion";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "DrpReligion";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_OtherReligion.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "Religion";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "OthrRelgn";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Drp_Caste.SelectedValue;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Cast";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Caste";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Rbd_Category.SelectedValue;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "Category";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_GuardianName.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "GardianName";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "GuardianName";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = txt_FatherProfession.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "FatherOccupation";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "FathersOccupation";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = txt_AnnualIncome.Text;
                dr["DataType"] = "double";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "AnnualIncome";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "FathersAnnualIncome";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_EmailId.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "Email";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "FathersEmaidID";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = txt_FathersOffcaddress.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "FathersOfficeAddredd";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);



                dr = dt.NewRow();
                dr["Name"] = Txt_MotherName.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "MothersName";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MothrName";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_MothersProfession.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "MothersOccupation";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = txt_mothersIncome.Text;
                dr["DataType"] = "double";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "MothersAnnualIncome";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MothersAnnualIncomne";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = txt_Mothersemail.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "MothersEmail";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MothersEmaidID";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_MothersOffcAddress.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "True";
                dr["WinErDataField"] = "FatherOfcAdd";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "MothersOfficeAddress";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_State.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "State";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "State";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_Location.Text;
                dr["DataType"] = "string";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Location";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Location";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_pin.Text;
                dr["DataType"] = "int";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "Pin";
                dr["WinErDatatable"] = "tblstudent";
                dr["ColumnName"] = "Pin";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                //dr = dt.NewRow();
                //dr["Name"] = Txt_FatherEduQualification.Text;
                //dr["DataType"] = "string";
                //dr["DefaultValue"] = "";
                //dr["Ismandatory"] = "False";
                //dr["WinErDataField"] = "FatherEduQuali";
                //dr["WinErDatatable"] = "tblstudent";
                //dr["ColumnName"] = "FthrQlfn";
                //StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                //dr = dt.NewRow();
                //dr["Name"] = Txt_MotherEduQualification.Text;
                //dr["DataType"] = "string";
                //dr["DefaultValue"] = "";
                //dr["Ismandatory"] = "False";
                //dr["WinErDataField"] = "MotherEduQuali";
                //dr["WinErDatatable"] = "tblstudent";
                //dr["ColumnName"] = "MthrQlfn";
                //StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = Txt_LastSchoolName.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LastSchollName";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_LastSchoolAddress.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LastSchoolAddress";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_TcNumber.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "TCNum";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_TcDate.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "TCDate";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_ReasonLeaving.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LeavingReason";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_LastClass.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LastClass";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_LeavingDate.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LeavingDate";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_LastClassRemarks.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "LastClassRemarks";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = Txt_Identificationmark.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "IdentificationMark";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                dr = dt.NewRow();
                string sibling = rdo_sibling.SelectedValue;
                dr["Name"] = sibling;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "IsSiblingExist";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = Txt_SiblingAdmissionNo.Text;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "SiblingAdmissionNum";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = rdo_transport.SelectedValue;
                dr["DataType"] = "";
                dr["DefaultValue"] = "";
                dr["Ismandatory"] = "False";
                dr["WinErDataField"] = "";
                dr["WinErDatatable"] = "";
                dr["ColumnName"] = "TransportMode";
                StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);





                //dr = dt.NewRow();
                //dr["Name"] = Txt_NumBrother.Text;
                //dr["DataType"] = "int";
                //dr["DefaultValue"] = "";
                //dr["Ismandatory"] = "False";
                //dr["WinErDataField"] = "NumberofBrothers";
                //dr["WinErDatatable"] = "tblstudent";
                //dr["ColumnName"] = "BrotherNum";
                //StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                //dr = dt.NewRow();
                //dr["Name"] = Txt_NumSister.Text;
                //dr["DataType"] = "int";
                //dr["DefaultValue"] = "";
                //dr["Ismandatory"] = "False";
                //dr["WinErDataField"] = "NumberOfSysters";
                //dr["WinErDatatable"] = "tblstudent";
                //dr["ColumnName"] = "SisterNum";
                //StudentDetailsDs.Tables["StudentDetails"].Rows.Add(dr);

                return StudentDetailsDs;

            }

            private string CreateStudentXml(string ReferenceId, DataSet StudentDetailsDs)
            {
                string _StudentXml = "";
                StringWriter Studentwriter = new StringWriter();
                XmlTextWriter StudentXmlWriter = new XmlTextWriter(Studentwriter);
                StudentXmlWriter.WriteStartDocument();
                StudentXmlWriter.WriteStartElement("Student");
                StudentXmlWriter.WriteAttributeString("ReferenceId", ReferenceId);
                int Id = 1;

                foreach (DataRow dr in StudentDetailsDs.Tables[0].Rows)
                {
                    StudentXmlWriter.WriteStartElement("Column");
                    StudentXmlWriter.WriteAttributeString("Id", Id.ToString());
                    StudentXmlWriter.WriteAttributeString("Name", dr["Name"].ToString());
                    StudentXmlWriter.WriteAttributeString("DataType", dr["DataType"].ToString());
                    StudentXmlWriter.WriteAttributeString("DefaultValue", dr["DefaultValue"].ToString());
                    StudentXmlWriter.WriteAttributeString("Ismandatory", dr["Ismandatory"].ToString());
                    StudentXmlWriter.WriteAttributeString("WinErDataField", dr["WinErDataField"].ToString());
                    StudentXmlWriter.WriteAttributeString("WinErDatatable", dr["WinErDatatable"].ToString());
                    StudentXmlWriter.WriteAttributeString("ColumnName", dr["ColumnName"].ToString());
                    StudentXmlWriter.WriteEndElement();
                    Id++;
                }

                StudentXmlWriter.WriteEndElement();
                StudentXmlWriter.WriteEndDocument();
                StudentXmlWriter.Flush();
                _StudentXml = Studentwriter.ToString();
                return _StudentXml;
            }

            private string CreateXML(string _TempId)
            {
                string xmlString = "";
                StringWriter sw = new StringWriter();
                XmlTextWriter writer = new
                     XmlTextWriter(sw);

                // start writing!
                writer.WriteStartDocument();
                writer.WriteStartElement("Student");
                writer.WriteAttributeString("ReferenceId", _TempId);

                for (int i = 1; i < 10; i++)
                {
                    writer.WriteStartElement("Column");
                    writer.WriteAttributeString("FieldName", i.ToString());
                    writer.WriteAttributeString("Defaultvalue", i.ToString());
                    writer.WriteAttributeString("Field", i.ToString());
                    writer.WriteAttributeString("Field", i.ToString());
                    writer.WriteAttributeString("Field", i.ToString());
                    writer.WriteAttributeString("Field", i.ToString());
                    writer.WriteString("test");
                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                xmlString = sw.ToString();
                writer.Close();
                sw.Close();
                return xmlString;
            }


            #endregion 
    }
}
