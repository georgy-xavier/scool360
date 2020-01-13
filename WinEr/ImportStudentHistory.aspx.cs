using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class ImportStudentHistory : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
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
                    AddClassToDrpList();
                    AddReligionToDrpList();
                    LoadCast();
                    LoadBatch();
                    lblerror.Text = "";
                    lblcorrect.Text = "";
                    lblincorrect.Text = "";
                    lblselectedrowslno.Text = "";
                    lbluncorrect.Text = "";
                    Lbl_DetailsError.Text = "";
                    pnlValidrecords.Visible = false;
                    pnlInvalidrecords.Visible = false;
                }


            }


        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ClearAll();
            string msg, _filename;
            string _RecodrDetails = "";
            RecordsDetails.InnerHtml = _RecodrDetails;
            if (validUploadFile(out msg))
            {
                if (SaveFile(out _filename))
                {
                    string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\UpImage\\" + _filename;
                    MydataSet = ExcelDataset(_physicalpath);
                    BuildDataset(MydataSet);
                    File.Delete(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + _filename);
                }
                else
                {
                    lblerror.Text = "Not Able To Upload The Excel File. Try Again Later";
                }
            }
            else
            {
                lblerror.Text = msg;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            // This is for update datas to database
            if (grdincorrectdetails.Rows.Count != 0)
            {
                lblerror.Text = "Student details contains wrong entries.Please correct the details and try again.";
            }
            else
                if (CheckDetailsAreExist())
                {
                    MPE_Details.Show();
                }
                else
                {
                    Upload_detailstoDataBase();
                }
        }

        protected void grdincorrectdetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // this is for update wrong entries
            lbluncorrect.Text = "";
            lblselectedrowslno.Text = grdincorrectdetails.SelectedRow.Cells[0].Text.ToString();
            fillpopupwindow();

            MPE_popup.Show();

        }

        protected void Btn_savetodatabase_Click(object sender, EventArgs e)
        {
            // this is from popup window. Save the edited datas to valid grid and remove that row from wrong entries
            string msg = null;
            lbluncorrect.Text = "";
            if (NewValuesAreValid(out msg))
            {
                MoveToCorrectGrid();
                DeleteSelectedRow(int.Parse(lblselectedrowslno.Text));
                ClearFields();
                string _RecodrDetails = "<img src=\"Pics/user3.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + ((grdcorrectdetails.Rows.Count) + (grdincorrectdetails.Rows.Count)) + "</span>&nbsp;Student Records&nbsp;&nbsp;<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdcorrectdetails.Rows.Count + "</span>&nbsp;Valid Records&nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdincorrectdetails.Rows.Count + "</span>&nbsp;Invalid Records&nbsp;&nbsp;";
                RecordsDetails.InnerHtml = _RecodrDetails;

                lbluncorrect.Text = "Student details updated successfully ";
                lblselectedrowslno.Text = "";

            }
            else
            {
                lbluncorrect.Text = msg;
            }

            MPE_popup.Show();
        }

        protected void Btn_Continue_Click(object sender, EventArgs e)
        {
            Upload_detailstoDataBase();
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int[] DeletedValue = new int[GrdStudlist.Rows.Count];
            int i = 0;
            foreach (GridViewRow gvr in GrdStudlist.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.FindControl("Chk_Select");
                if (Chk.Checked == true)
                {
                    DeletedValue[i] = int.Parse(gvr.Cells[1].Text);
                    i = i + 1;
                }

            }
            CreateNewSetOfStudents(DeletedValue);
            if (CheckDetailsAreExist())
            {
                if (GrdStudlist.Rows.Count > 0)
                {
                    Lbl_DetailsError.Text = "Selected students are deleted.";

                    MPE_Details.Show();
                }
            }
            else
            {

                if (grdcorrectdetails.Rows.Count <= 0)
                {
                    Lbl_Message.Text = "No students existing for creation.";
                    lblerror.Text = "No students exist for creating";
                    string _RecodrDetails1 = "<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdcorrectdetails.Rows.Count + "</span>&nbsp;Student records created &nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + "0" + "</span>&nbsp;Records failed&nbsp;&nbsp;";
                    RecordsDetails.InnerHtml = _RecodrDetails1;
                }
                else
                {
                    Lbl_Message.Text = "Selected Student records are deleted. You can click save button to update student details.";

                }
                MPE_MSG_Pop.Show();

            }






        }

        private void ClearFields()
        {


            lblerrordescription.Text = "";
            txtstudname.Text = "";
            RdBtnLstSex.SelectedIndex = 0;
            txt_Dob.Text = "";
            Tx_FatherName.Text = "";
            Drp_Religoin.SelectedValue = "0";
            Drp_Cast.SelectedValue = "0";
            txtadmissionno.Text = "";
            Txt_DOL.Text = "";
            Joining_Batch.SelectedValue = MyUser.CurrentBatchId.ToString();
            Drp_Class.SelectedValue = "0";
            Txt_Tcno.Text = "";
            Txt_reason.Text = "";
        }

        private void CreateNewSetOfStudents(int[] DeletedValue)
        {
            ShowValidGridColumns();

            DataSet newcorrectdatas = new DataSet();
            DataTable dtnewcorrectdatas;

            newcorrectdatas.Tables.Add(new DataTable("newcorrectexceldatas"));
            dtnewcorrectdatas = newcorrectdatas.Tables["newcorrectexceldatas"];

            dtnewcorrectdatas.Columns.Add("slno");
            dtnewcorrectdatas.Columns.Add("StudentName");
            dtnewcorrectdatas.Columns.Add("sex");
            dtnewcorrectdatas.Columns.Add("Date Of Birth");
            dtnewcorrectdatas.Columns.Add("Father/Guardian Name");
            dtnewcorrectdatas.Columns.Add("relogion");
            dtnewcorrectdatas.Columns.Add("caste");
            dtnewcorrectdatas.Columns.Add("Admission No");
            dtnewcorrectdatas.Columns.Add("Date Of Leaving");
            dtnewcorrectdatas.Columns.Add("Last Class");
            dtnewcorrectdatas.Columns.Add("Joining Batch");
            dtnewcorrectdatas.Columns.Add("Reason For Leaving");
            dtnewcorrectdatas.Columns.Add("TC Number");
            dtnewcorrectdatas.Columns.Add("JoiningBatchId");
            dtnewcorrectdatas.Columns.Add("JoiningClassId");
            dtnewcorrectdatas.Columns.Add("ReligionId");
            dtnewcorrectdatas.Columns.Add("CastId");
            DataRow drcorrect;

            int TempVal = 0;
            int j = 0;

            foreach (GridViewRow gvr in grdcorrectdetails.Rows)
            {
                TempVal = 0;
                j = j + 1;
                for (int i = 0; i < DeletedValue.Length; i++)
                {
                    if (j == DeletedValue[i])
                    {
                        TempVal = 1;
                    }
                }

                if (TempVal == 0)
                {

                    drcorrect = dtnewcorrectdatas.NewRow();
                    drcorrect["slno"] = gvr.Cells[0].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["StudentName"] = gvr.Cells[1].Text.Trim().Replace("&nbsp;", "").ToUpper();
                    drcorrect["sex"] = gvr.Cells[2].Text.Trim().Replace("&nbsp;", "").ToUpper();
                    drcorrect["Date Of Birth"] = gvr.Cells[3].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Father/Guardian Name"] = gvr.Cells[4].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["relogion"] = gvr.Cells[5].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["caste"] = gvr.Cells[6].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Admission No"] = gvr.Cells[7].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Date Of Leaving"] = gvr.Cells[8].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Last Class"] = gvr.Cells[9].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Joining Batch"] = gvr.Cells[10].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["TC Number"] = gvr.Cells[11].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["Reason For Leaving"] = gvr.Cells[12].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["JoiningBatchId"] = gvr.Cells[13].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["JoiningClassId"] = gvr.Cells[14].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["ReligionId"] = gvr.Cells[15].Text.Trim().Replace("&nbsp;", "");
                    drcorrect["CastId"] = gvr.Cells[16].Text.Trim().Replace("&nbsp;", "");
                    newcorrectdatas.Tables["newcorrectexceldatas"].Rows.Add(drcorrect);
                }
            }

            if (newcorrectdatas.Tables[0].Rows.Count > 0)
            {

                lblcorrect.Text = "";
                FillCorrectDetailsToGrid(newcorrectdatas);

            }
            else
            {

                FillCorrectDetailsToGrid(null);
                lblcorrect.Text = "Uploded files contains some wrong entries.";

            }

            HideInvalidGridColumns();

        }

        private void Upload_detailstoDataBase()
        {
            string message = "";
            int id = -1;
            ShowValidGridColumns();
            string StudentName = "", GardianName = "", AdmitionNo = "", Sex = "", Address = "", Email = "", Location = "", State = "", Nationality = "", FatherEduQuali = "", MothersName = "", MotherEduQuali = "", FatherOccupation = "", ResidencePhNo = "", OfficePhNo = "0", FirstLanguage = "1", TempStudentId = "0", Addresspresent = "", CreatedUserName = MyUser.LoginUserName, CreationTime = "", Comment = "", ReasonForLeaving = "", TCNumber = "0";

            DateTime DOB, DateofJoining = System.DateTime.Now;
            DateTime DateOfLeaving;
            int Status = 0, AnnualIncome = 0, NumberofBrothers = 0, NumberOfSysters = 0, JoinBatch = 0, StudTypeId = 1, AdmissionTypeId = 0, ClassId = 0, BatchId = 0, CanceledUser = 0, UseBus = 0, MotherTongue = 1, UseHostel = 0, RollNo = 0, Religion = 0, Cast = 0, BloodGroup = 17, Pin = 0;
            try
            {
                MyStudMang.CreateTansationDb();

                foreach (GridViewRow gvr in grdcorrectdetails.Rows)
                {

                    StudentName = gvr.Cells[1].Text.Trim().Replace("&nbsp;", "").Replace("'", "").ToUpper();
                    Sex = gvr.Cells[2].Text.Trim().Replace("&nbsp;", "").Replace("'", "").ToUpper();
                    DOB = MyUser.GetDareFromText(gvr.Cells[3].Text.Trim().Replace("'", "").Replace("&nbsp;", ""));
                    GardianName = gvr.Cells[4].Text.Trim().Replace("&nbsp;", "").Replace("'", "");
                    //gvr.Cells[5].Text.Trim().Replace("&nbsp;", "");  No need to enter religion ,enter only Id
                    //gvr.Cells[6].Text.Trim().Replace("&nbsp;", ""); No need to enter cast,Enter only Id
                    AdmitionNo = gvr.Cells[7].Text.Trim().Replace("&nbsp;", "").Replace("'", "");
                    DateOfLeaving = MyUser.GetDareFromText(gvr.Cells[8].Text.Trim().Replace("&nbsp;", "").Replace("'", ""));
                    //gvr.Cells[9].Text.Trim().Replace("&nbsp;", ""); No need to enter the class only Id
                    //gvr.Cells[10].Text.Trim().Replace("&nbsp;", ""); No need to enter the joining batch only id
                    ReasonForLeaving = gvr.Cells[11].Text.Trim().Replace("&nbsp;", "").Replace("'", "");
                    TCNumber = gvr.Cells[12].Text.Trim().Replace("&nbsp;", "").Replace("'", "");
                    JoinBatch = int.Parse(gvr.Cells[13].Text.Trim().Replace("&nbsp;", "").Replace("'", ""));
                    ClassId = int.Parse(gvr.Cells[14].Text.Trim().Replace("&nbsp;", "").Replace("'", ""));
                    Religion = int.Parse(gvr.Cells[15].Text.Trim().Replace("&nbsp;", "").Replace("'", ""));
                    Cast = int.Parse(gvr.Cells[16].Text.Trim().Replace("&nbsp;", "").Replace("'", ""));

                    id = MyStudMang.UpdateHistory(StudentName, GardianName, AdmitionNo, DOB, Sex, Address, BloodGroup, Religion, Cast, DateofJoining, DateOfLeaving, Status, Email, Location, Pin, State, Nationality, FatherEduQuali, MothersName, MotherEduQuali, FatherOccupation, AnnualIncome, MotherTongue, ResidencePhNo, OfficePhNo, FirstLanguage, NumberofBrothers, NumberOfSysters, JoinBatch, CreationTime, Addresspresent, StudTypeId, AdmissionTypeId, CreatedUserName, ClassId, BatchId, Comment, CanceledUser, TempStudentId, UseBus, UseHostel, RollNo, ReasonForLeaving, TCNumber, out message);
                    if (id == -1)
                    {
                        string _RecodrDetails = "<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + "0" + "</span>&nbsp;Student records created &nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdcorrectdetails.Rows.Count + "</span>&nbsp;Records failed&nbsp;&nbsp;";
                        RecordsDetails.InnerHtml = _RecodrDetails;
                        lblerror.Text = "Some Error Occurs";
                        break;
                    }

                }
                if (id != -1)
                {
                    string _RecodrDetails1 = "<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdcorrectdetails.Rows.Count + "</span>&nbsp;Student records created &nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + "0" + "</span>&nbsp;Records failed&nbsp;&nbsp;";
                    RecordsDetails.InnerHtml = _RecodrDetails1;
                    FillCorrectDetailsToGrid(null);
                    lblerror.Text = "Student Details are updated";
                    MyStudMang.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Import Student History", "Student Details are updated", 1);

                    MyStudMang.CreateApproved_History_Incedent("Student Created", "Student " + StudentName + " is admitted to " + MyStudMang.GetClassName(ClassId) + " on " + General.GerFormatedDatVal(DateTime.Now), General.GerFormatedDatVal(DateTime.Now), 1, MyUser.UserId, "student", id, 1, 0, JoinBatch, ClassId);

                }
                else
                {
                    MyStudMang.EndFailTansationDb();
                }
            }
            catch
            {
                string _RecodrDetails = "<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + "0" + "</span>&nbsp;Student records created &nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + grdcorrectdetails.Rows.Count + "</span>&nbsp;Records failed&nbsp;&nbsp;";
                RecordsDetails.InnerHtml = _RecodrDetails;
                MyStudMang.EndFailTansationDb();
                lblerror.Text = "Some Error Has Occured";

            }
            HideValidGridColumns();
        }

        private bool CheckDetailsAreExist()
        {
            GrdStudlist.DataSource = null;
            GrdStudlist.DataBind();
            ShowValidGridColumns();
            Lbl_DetailsError.Text = "";
            bool exist = false;
            DataSet ExistingDetails = new DataSet();
            string AdmissionNumber;
            //DateTime _DOB;
            DataTable DataTbl;
            int _no = 1;
            ExistingDetails.Tables.Add(new DataTable("DataTbl"));
            DataTbl = ExistingDetails.Tables["DataTbl"];
            DataTbl.Columns.Add("slno");
            DataTbl.Columns.Add("StudentName");
            DataTbl.Columns.Add("sex");
            DataTbl.Columns.Add("Date Of Birth");
            DataTbl.Columns.Add("Admission No");

            foreach (GridViewRow gvr in grdcorrectdetails.Rows)
            {
                AdmissionNumber = gvr.Cells[7].Text.Trim();
                string sql = "select StudentName,Sex,DOB from tblstudent_history where AdmitionNo='" + AdmissionNumber + "'";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        exist = true;
                        DataRow druncorrect = DataTbl.NewRow();
                        druncorrect["slno"] = _no;
                        druncorrect["StudentName"] = MyReader.GetValue(0).ToString();
                        druncorrect["sex"] = MyReader.GetValue(1).ToString();
                        druncorrect["Date Of Birth"] = MyReader.GetValue(2).ToString();
                        druncorrect["Admission No"] = gvr.Cells[7].Text.Trim();
                        ExistingDetails.Tables["DataTbl"].Rows.Add(druncorrect);
                    }
                }
                _no += 1;
            }
            if (ExistingDetails.Tables[0].Rows.Count > 0)
            {
                GrdStudlist.DataSource = ExistingDetails;
                GrdStudlist.DataBind();
                Pnl_Details.Visible = true;
            }
            HideValidGridColumns();
            return exist;
        }

        private bool validUploadFile(out string msg)
        {
            bool valid = true;
            msg = null;
            if (FileUploadstudent_excel.PostedFile == null)
            {
                valid = false;
                msg = "select a file to upload";
            }
            if (!validextension())
            {
                valid = false;
                msg = "Selected File is not in excel Format-[ Only .xlt and .xls formats are supportable]";
            }
            return valid;
        }

        private bool validextension()
        {
            bool valid = true;
            string fileextension = System.IO.Path.GetExtension(FileUploadstudent_excel.FileName).ToLower();
            string[] allowedextensions = { ".xlt", ".xls", ".xlsx" };
            for (int i = 0; i < allowedextensions.Length; i++)
            {
                if (fileextension == allowedextensions[i])
                {
                    valid = true;
                }
            }
            return valid;
        }

        private bool SaveFile(out string filename)
        {
            bool valid = true;
            filename = null;
            try
            {
                filename = FileUploadstudent_excel.FileName.ToString();
                FileUploadstudent_excel.SaveAs(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + filename);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        private DataSet ExcelDataset(string Physicalpath)
        {

            System.Data.DataTable dt = null;
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Physicalpath + "; Extended Properties=Excel 8.0;");
            try
            {
                con.Open();
                dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String[] excelsheet = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    excelsheet[i] = dr["TABLE_NAME"].ToString();
                    i++;
                }
                DataSet ds = new DataSet();
                foreach (string temp in excelsheet)
                {
                    string strquery = "select * from [" + temp + "]";
                    OleDbDataAdapter adpt = new OleDbDataAdapter(strquery, con);
                    adpt.Fill(ds, temp);
                }
                return ds;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                DataSet ds = null;
                return ds;
            }
            finally
            {
                con.Close();
            }

        }

        private void BuildDataset(DataSet Studdataset)
        {
            try
            {
                lblerror.Text = "";
                int arraysize = Studdataset.Tables[0].Rows.Count, _invalidcounts = 0, _validcounts = 0;
                int[] invalidrowId = new int[arraysize];
                int[] validrowId = new int[arraysize];
                string errormsg = "";
                string[,] invalidRows_description = new string[arraysize, 2];
                if (Studdataset != null && Studdataset.Tables != null && Studdataset.Tables[0].Rows.Count > 0)
                {
                    btnUpload.Visible = true;
                    for (int i = 0; i < Studdataset.Tables[0].Rows.Count; i++)
                    {
                        if (Studdataset.Tables[0].Rows[i][0].ToString() != "")
                        {
                            if (AllvaluesAreCorrect(i, Studdataset.Tables[0].Rows[i], out errormsg))
                            {
                                validrowId[_validcounts] = i;
                                _validcounts += 1;
                            }
                            else
                            {
                                invalidrowId[_invalidcounts] = i;
                                invalidRows_description[_invalidcounts, 1] = errormsg;
                                _invalidcounts += 1;
                            }
                        }
                        else
                            break;
                    }
                    string _RecodrDetails = "<img src=\"Pics/user3.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + Studdataset.Tables[0].Rows.Count + "</span>&nbsp;Student Records&nbsp;&nbsp;<img src=\"Pics/accept.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + _validcounts + "</span>&nbsp;Valid Records&nbsp;&nbsp;<img src=\"Pics/DeleteRed.png\" width=\"20px\" height=\"20px\" /> &nbsp; <span style=\"font-weight:bold\">" + _invalidcounts + "</span>&nbsp;Invalid Records&nbsp;&nbsp;";


                    RecordsDetails.InnerHtml = _RecodrDetails;


                    build_Valid_Invalid_Dataset(invalidrowId, _invalidcounts, _validcounts, validrowId, Studdataset, invalidRows_description);

                }
                else
                {
                    lblerror.Text = "Nothing To Display. The Excel May be empty";
                }
            }
            catch
            {
                lblerror.Text = "Unable To Read Data From  The  Selected Excel File";
            }
        }

        private void build_Valid_Invalid_Dataset(int[] invalidrowId, int _invalidcounts, int _validcounts, int[] validrowId, DataSet Studdataset, string[,] invalidRows_description)
        {
            string _joinBatch, _LastClass, errormsg, _Religion, _Cast;
            int _validrowId;
            int _joinclassid, JoiningBatchId, _ReligionId, _CastId;

            DataSet correctexceldata = new DataSet();
            DataTable dtcorrect;

            correctexceldata.Tables.Add(new DataTable("correctexceldatas"));
            dtcorrect = correctexceldata.Tables["correctexceldatas"];
            dtcorrect.Columns.Add("slno");
            dtcorrect.Columns.Add("StudentName");
            dtcorrect.Columns.Add("sex");
            dtcorrect.Columns.Add("Date Of Birth");
            dtcorrect.Columns.Add("Father/Guardian Name");
            dtcorrect.Columns.Add("relogion");
            dtcorrect.Columns.Add("caste");
            dtcorrect.Columns.Add("Admission No");
            dtcorrect.Columns.Add("Date Of Leaving");
            dtcorrect.Columns.Add("Last Class");
            dtcorrect.Columns.Add("Joining Batch");
            dtcorrect.Columns.Add("Reason For Leaving");
            dtcorrect.Columns.Add("TC Number");
            dtcorrect.Columns.Add("JoiningBatchId");
            dtcorrect.Columns.Add("JoiningClassId");
            dtcorrect.Columns.Add("ReligionId");
            dtcorrect.Columns.Add("CastId");

            for (int i = 0; i < _validcounts; i++)
            {
                _validrowId = validrowId[i];
                _LastClass = Studdataset.Tables[0].Rows[_validrowId][13].ToString().Replace("&nbsp;", "");
                _joinBatch = Studdataset.Tables[0].Rows[_validrowId][14].ToString().Replace("&nbsp;", "");
                _Religion = Studdataset.Tables[0].Rows[_validrowId][7].ToString().Replace("&nbsp;", "");
                _Cast = Studdataset.Tables[0].Rows[_validrowId][8].ToString().Replace("&nbsp;", "");

                if (AllDatasAreAlreadyExists(_LastClass, _joinBatch, _Religion, _Cast, out _joinclassid, out JoiningBatchId, out _ReligionId, out _CastId, out errormsg))
                {
                    DataRow drcorrect = dtcorrect.NewRow();
                    drcorrect["slno"] = Studdataset.Tables[0].Rows[_validrowId][0].ToString().Replace("&nbsp;", "");
                    drcorrect["StudentName"] = Studdataset.Tables[0].Rows[_validrowId][1].ToString().Replace("&nbsp;", "");
                    drcorrect["sex"] = Studdataset.Tables[0].Rows[_validrowId][2].ToString().Replace("&nbsp;", "");
                    drcorrect["Date Of Birth"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Studdataset.Tables[0].Rows[_validrowId][5].ToString()), int.Parse(Studdataset.Tables[0].Rows[_validrowId][4].ToString()), int.Parse(Studdataset.Tables[0].Rows[_validrowId][3].ToString())));
                    drcorrect["Father/Guardian Name"] = Studdataset.Tables[0].Rows[_validrowId][6].ToString().Replace("&nbsp;", "");
                    drcorrect["relogion"] = Studdataset.Tables[0].Rows[_validrowId][7].ToString().Replace("&nbsp;", "");
                    drcorrect["caste"] = Studdataset.Tables[0].Rows[_validrowId][8].ToString().Replace("&nbsp;", "");
                    drcorrect["Admission No"] = Studdataset.Tables[0].Rows[_validrowId][9].ToString().Replace("&nbsp;", "");


                    drcorrect["Date Of Leaving"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Studdataset.Tables[0].Rows[_validrowId][12].ToString()), int.Parse(Studdataset.Tables[0].Rows[_validrowId][11].ToString()), int.Parse(Studdataset.Tables[0].Rows[_validrowId][10].ToString())));


                    drcorrect["TC Number"] = Studdataset.Tables[0].Rows[_validrowId][15].ToString().Replace("&nbsp;", "");
                    drcorrect["Reason For Leaving"] = Studdataset.Tables[0].Rows[_validrowId][16].ToString().Replace("&nbsp;", "");

                    drcorrect["Last Class"] = _LastClass;
                    drcorrect["Joining Batch"] = _joinBatch;
                    drcorrect["JoiningBatchId"] = JoiningBatchId.ToString();
                    drcorrect["JoiningClassId"] = _joinclassid.ToString();
                    drcorrect["ReligionId"] = _ReligionId.ToString();
                    drcorrect["CastId"] = _CastId.ToString();
                    correctexceldata.Tables["correctexceldatas"].Rows.Add(drcorrect);
                }

            }
            if (correctexceldata.Tables[0].Rows.Count > 0)
            {

                lblcorrect.Text = "";
                FillCorrectDetailsToGrid(correctexceldata);
                pnlValidrecords.Visible = true;
            }
            else
            {

                FillCorrectDetailsToGrid(null);
                lblcorrect.Text = "Uploded files contains some wrong entries.";
                pnlValidrecords.Visible = false;
            }

            if (_invalidcounts > 0)
            {
                lblincorrect.Text = "";
                buildInvalidDataset(_invalidcounts, invalidrowId, Studdataset, invalidRows_description);
            }
            else
            {
                lblincorrect.Text = "All values are correct.Please click save button to create students";
            }

        }

        private bool AllDatasAreAlreadyExists(string _JoinClass, string _joinBatch, string _Religion, string _Cast, out int _joinclassid, out int JoiningBatchId, out int _ReligionId, out int _CastId, out string errormsg)
        {

            _joinclassid = 0;
            JoiningBatchId = MyUser.CurrentBatchId;
            _ReligionId = 0;
            _CastId = 0;
            errormsg = "";

            string sql = "";

            sql = "select distinct tblclass.Id,lower(tblclass.classname) from tblclass";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string _classname;
                while (MyReader.Read())
                {
                    _classname = MyReader.GetValue(1).ToString().Trim().ToLower();
                    if (_classname.ToLower() == _JoinClass.ToLower().Trim())
                    {
                        _joinclassid = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }

            }
            else
            {
                return false;
            }
            sql = "select distinct tblbatch.Id,tblbatch.BatchName from tblbatch";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string _batchname;
                while (MyReader.Read())
                {
                    _batchname = MyReader.GetValue(1).ToString();
                    if (_batchname == _joinBatch.ToString())
                    {
                        JoiningBatchId = int.Parse(MyReader.GetValue(0).ToString());

                        break;
                    }

                }
            }
            else
            {
                return false;
            }

            sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                string Religion = MyReader.GetValue(1).ToString();

                while (MyReader.Read())
                {
                    if (_Religion.ToLower() == Religion.ToString().ToLower())
                    {
                        _ReligionId = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }
            else
            {
                return false;
            }
            //domininc
            sql = "select tblcast.Id, tblcast.castname from tblcast";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                string Cast = MyReader.GetValue(1).ToString();

                while (MyReader.Read())
                {
                    if (_Cast.ToLower() == Cast.ToString().ToLower())
                    {
                        _CastId = int.Parse(MyReader.GetValue(0).ToString());
                        break;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;

        }

        private void buildInvalidDataset(int _invalidcounts, int[] invalidrowId, DataSet Studdataset, string[,] invalidRows_description)
        {
            int _invalidrowid = 0;
            int _slno = 0;
            string _joinBatch, _LastClass, errormsg, _Religion, _Cast;
            int _validrowId;
            int _joinclassid, JoiningBatchId, _ReligionId, _CastId;

            DataSet uncorrectexceldata = new DataSet();
            DataTable dtuncorrect;

            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];
            dtuncorrect.Columns.Add("SlNo");
            dtuncorrect.Columns.Add("StudentName");
            dtuncorrect.Columns.Add("sex");
            dtuncorrect.Columns.Add("Date Of Birth");
            dtuncorrect.Columns.Add("Father/Guardian Name");
            dtuncorrect.Columns.Add("relogion");
            dtuncorrect.Columns.Add("caste");
            dtuncorrect.Columns.Add("Admission No");
            dtuncorrect.Columns.Add("Date Of Leaving");
            dtuncorrect.Columns.Add("Last Class");
            dtuncorrect.Columns.Add("Joining Batch");
            dtuncorrect.Columns.Add("Reason For Leaving");
            dtuncorrect.Columns.Add("TC Number");
            dtuncorrect.Columns.Add("JoiningBatchId");
            dtuncorrect.Columns.Add("JoiningClassId");
            dtuncorrect.Columns.Add("Description");
            dtuncorrect.Columns.Add("ReligionId");
            dtuncorrect.Columns.Add("CastId");
            for (int i = 0; i < _invalidcounts; i++)
            {
                _invalidrowid = invalidrowId[i];
                DataRow druncorrect = dtuncorrect.NewRow();
                _slno += 1;
                _LastClass = Studdataset.Tables[0].Rows[_invalidrowid][13].ToString();
                _joinBatch = Studdataset.Tables[0].Rows[_invalidrowid][14].ToString();
                _Religion = Studdataset.Tables[0].Rows[_invalidrowid][7].ToString();
                _Cast = Studdataset.Tables[0].Rows[_invalidrowid][8].ToString();

                if (AllDatasAreAlreadyExists(_LastClass, _joinBatch, _Religion, _Cast, out _joinclassid, out JoiningBatchId, out _ReligionId, out _CastId, out errormsg))
                {
                    druncorrect["SlNo"] = Studdataset.Tables[0].Rows[_invalidrowid][0].ToString().Replace("&nbsp;", "");
                    druncorrect["StudentName"] = Studdataset.Tables[0].Rows[_invalidrowid][1].ToString().Replace("&nbsp;", "");
                    druncorrect["sex"] = Studdataset.Tables[0].Rows[_invalidrowid][2].ToString().Replace("&nbsp;", "").ToUpper();
                    druncorrect["Date Of Birth"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][5].ToString()), int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][4].ToString()), int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][3].ToString())));

                    druncorrect["Father/Guardian Name"] = Studdataset.Tables[0].Rows[_invalidrowid][6].ToString().Replace("&nbsp;", "");
                    druncorrect["relogion"] = Studdataset.Tables[0].Rows[_invalidrowid][7].ToString().Replace("&nbsp;", "");
                    druncorrect["caste"] = Studdataset.Tables[0].Rows[_invalidrowid][8].ToString().Replace("&nbsp;", "");
                    druncorrect["Admission No"] = Studdataset.Tables[0].Rows[_invalidrowid][9].ToString().Replace("&nbsp;", "");
                    druncorrect["Date Of Leaving"] = MyUser.GerFormatedDatVal(new DateTime(int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][12].ToString()), int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][11].ToString()), int.Parse(Studdataset.Tables[0].Rows[_invalidrowid][10].ToString())));

                    druncorrect["Last Class"] = Studdataset.Tables[0].Rows[_invalidrowid][13].ToString().Replace("&nbsp;", "");
                    druncorrect["Joining Batch"] = Studdataset.Tables[0].Rows[_invalidrowid][14].ToString().Replace("&nbsp;", "");
                    druncorrect["TC Number"] = Studdataset.Tables[0].Rows[_invalidrowid][15].ToString().Replace("&nbsp;", "");
                    druncorrect["Reason For Leaving"] = Studdataset.Tables[0].Rows[_invalidrowid][16].ToString().Replace("&nbsp;", "");

                    druncorrect["Description"] = invalidRows_description[i, 1];


                    druncorrect["JoiningBatchId"] = JoiningBatchId.ToString();
                    druncorrect["JoiningClassId"] = _joinclassid.ToString();
                    druncorrect["ReligionId"] = _ReligionId.ToString();
                    druncorrect["CastId"] = _CastId.ToString();

                    uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);
                }
            }

            if (uncorrectexceldata.Tables[0].Rows.Count > 0)
            {
                FillWrongDetailsToGrid(uncorrectexceldata);
                pnlInvalidrecords.Visible = true;
            }
            else
            {
                FillWrongDetailsToGrid(null);
                pnlInvalidrecords.Visible = false;
            }
        }

        private bool AllvaluesAreCorrect(int i, DataRow dataRow, out string errormsg)
        {
            DateTime tryparsedatetime;
            bool _validrow = true;
            errormsg = "";
            if (dataRow[1].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Enter Student Name.";
                return _validrow;
            }
            if (_validrow && dataRow[2].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = errormsg + "Enter Student Sex.";
                return _validrow;
            }
            else if (_validrow && !((dataRow[2].ToString().Trim().ToLower() == "male") || (dataRow[2].ToString().Trim().ToLower() == "female")))
            {
                _validrow = false;
                errormsg = "You are not entered sex.Please select from below.";
                return _validrow;
            }
            if (_validrow && dataRow[3].ToString().Trim() == "")
            {

                _validrow = false;
                errormsg = "Please Enter Date Of Birth - Day .";
                return _validrow;
            }

            if (_validrow && dataRow[4].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Please Enter Date Of Birth - Month .";
                return _validrow;
            }

            if (_validrow && dataRow[5].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Please Enter Date Of Birth - Year .";
                return _validrow;
            }


            if (_validrow)
            {
                try
                {
                    DateTime _new = new DateTime(int.Parse(dataRow[5].ToString().Trim()), int.Parse(dataRow[4].ToString().Trim()), int.Parse(dataRow[3].ToString().Trim()));
                }
                catch
                {
                    _validrow = false;
                    errormsg = "DOB entry is wrong";
                    return _validrow;
                }

            }

            if (dataRow[9].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "  Enter Admission Number. ";
                return _validrow;
            }

            else if (isAdmissionNoAlreadyPresent(dataRow[9].ToString().Trim()))
            {
                _validrow = false;
                errormsg = "Same Admission Number Already Exists. ";
                return _validrow;
            }


            if (_validrow && dataRow[10].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Please Enter Date Of Leaving- Day.";
                return _validrow;
            }

            if (_validrow && dataRow[11].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Please Enter Date Of Leaving- Month.";
                return _validrow;
            }

            if (_validrow && dataRow[12].ToString().Trim() == "")
            {
                _validrow = false;
                errormsg = "Please Enter Date Of Leaving- Year.";
                return _validrow;
            }


            if (_validrow)
            {
                if (dataRow[13].ToString().Trim() == "")
                {
                    _validrow = false;
                    errormsg = "Enter Last class. ";
                    return _validrow;
                }
                else
                {
                    string studentclass = dataRow[13].ToString().Trim();
                    string sql = "";
                    sql = "select tblclass.Standard   from tblclass where lower(tblclass.classname)='" + studentclass.ToLower() + "'";
                    MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        _validrow = true;
                    }
                    else
                    {
                        _validrow = false;
                        errormsg = "  Enter Correct Class Name. ";
                    }

                }
            }

            return _validrow;

        }

        private bool isAdmissionNoAlreadyPresent(string _Admission)
        {
            bool _valid = false;
            string sql = "SELECT COUNT(tblview_student.Id) FROM tblview_student WHERE tblview_student.AdmitionNo='" + _Admission + "'";
            OdbcDataReader _myreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int _count = 0;
                int.TryParse(_myreader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        private void ClearAll()
        {

            FillCorrectDetailsToGrid(null);
            FillWrongDetailsToGrid(null);

            this.pnlInvalidrecords.Visible = false;
            this.pnlValidrecords.Visible = false;
        }

        private void DeleteSelectedRow(int selectedrowno)
        {
            int _slno = 0;
            DataSet uncorrectexceldata = new DataSet();
            DataTable dtuncorrect;
            uncorrectexceldata.Tables.Add(new DataTable("uncorrectexceldatas"));
            dtuncorrect = uncorrectexceldata.Tables["uncorrectexceldatas"];

            dtuncorrect.Columns.Add("slno");
            dtuncorrect.Columns.Add("StudentName");
            dtuncorrect.Columns.Add("sex");
            dtuncorrect.Columns.Add("Date Of Birth");
            dtuncorrect.Columns.Add("Father/Guardian Name");
            dtuncorrect.Columns.Add("relogion");
            dtuncorrect.Columns.Add("caste");
            dtuncorrect.Columns.Add("Admission No");
            dtuncorrect.Columns.Add("Date Of Admission");
            dtuncorrect.Columns.Add("Date Of Leaving");
            dtuncorrect.Columns.Add("Last Class");
            dtuncorrect.Columns.Add("Joining Batch");
            dtuncorrect.Columns.Add("Reason For Leaving");
            dtuncorrect.Columns.Add("TC Number");
            dtuncorrect.Columns.Add("JoiningBatchId");
            dtuncorrect.Columns.Add("JoiningClassId");
            dtuncorrect.Columns.Add("Description");
            dtuncorrect.Columns.Add("ReligionId");
            dtuncorrect.Columns.Add("CastId");
            foreach (GridViewRow gvr in grdincorrectdetails.Rows)
            {
                int rowno = int.Parse(gvr.Cells[0].Text);
                if (rowno != selectedrowno)
                {
                    DataRow druncorrect = dtuncorrect.NewRow();
                    _slno += 1;
                    druncorrect["SlNo"] = _slno;

                    druncorrect["slno"] = gvr.Cells[0].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["StudentName"] = gvr.Cells[1].Text.Trim().Replace("&nbsp;", "").ToUpper();
                    druncorrect["Sex"] = gvr.Cells[2].Text.Trim().Replace("&nbsp;", "").ToUpper();
                    druncorrect["Date Of Birth"] = gvr.Cells[3].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Father/Guardian Name"] = gvr.Cells[4].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["relogion"] = gvr.Cells[5].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["caste"] = gvr.Cells[6].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Admission No"] = gvr.Cells[7].Text.Trim().Replace("&nbsp;", "");

                    druncorrect["Date Of Leaving"] = gvr.Cells[8].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Last Class"] = gvr.Cells[9].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Joining Batch"] = gvr.Cells[10].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Reason For Leaving"] = gvr.Cells[11].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["TC Number"] = gvr.Cells[12].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["JoiningBatchId"] = gvr.Cells[13].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["JoiningClassId"] = gvr.Cells[14].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["Description"] = gvr.Cells[15].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["ReligionId"] = gvr.Cells[16].Text.Trim().Replace("&nbsp;", "");
                    druncorrect["CastId"] = gvr.Cells[17].Text.Trim().Replace("&nbsp;", "");
                    uncorrectexceldata.Tables["uncorrectexceldatas"].Rows.Add(druncorrect);

                }
            }
            if (uncorrectexceldata.Tables[0].Rows.Count > 0)
            {

                lbluncorrect.Text = "";
                FillWrongDetailsToGrid(uncorrectexceldata);

                pnlInvalidrecords.Visible = true;
            }
            else
            {

                FillWrongDetailsToGrid(null);
                pnlInvalidrecords.Visible = false;
            }
        }

        private void MoveToCorrectGrid()
        {

            ShowValidGridColumns();

            lblcorrect.Text = "";
            DataSet newcorrectdatas = new DataSet();
            DataTable dtnewcorrectdatas;

            newcorrectdatas.Tables.Add(new DataTable("newcorrectexceldatas"));
            dtnewcorrectdatas = newcorrectdatas.Tables["newcorrectexceldatas"];

            dtnewcorrectdatas.Columns.Add("slno");
            dtnewcorrectdatas.Columns.Add("StudentName");
            dtnewcorrectdatas.Columns.Add("sex");
            dtnewcorrectdatas.Columns.Add("Date Of Birth");
            dtnewcorrectdatas.Columns.Add("Father/Guardian Name");
            dtnewcorrectdatas.Columns.Add("relogion");
            dtnewcorrectdatas.Columns.Add("caste");
            dtnewcorrectdatas.Columns.Add("Admission No");
            dtnewcorrectdatas.Columns.Add("Date Of Leaving");
            dtnewcorrectdatas.Columns.Add("Last Class");
            dtnewcorrectdatas.Columns.Add("Joining Batch");
            dtnewcorrectdatas.Columns.Add("Reason For Leaving");
            dtnewcorrectdatas.Columns.Add("TC Number");
            dtnewcorrectdatas.Columns.Add("JoiningBatchId");
            dtnewcorrectdatas.Columns.Add("JoiningClassId");
            dtnewcorrectdatas.Columns.Add("ReligionId");
            dtnewcorrectdatas.Columns.Add("CastId");
            DataRow drcorrect;

            foreach (GridViewRow gvr in grdcorrectdetails.Rows)
            {
                drcorrect = dtnewcorrectdatas.NewRow();
                drcorrect["slno"] = gvr.Cells[0].Text.Trim().Replace("&nbsp;", "");
                drcorrect["StudentName"] = gvr.Cells[1].Text.Trim().Replace("&nbsp;", "").ToUpper();
                drcorrect["sex"] = gvr.Cells[2].Text.Trim().Replace("&nbsp;", "").ToUpper();
                drcorrect["Date Of Birth"] = gvr.Cells[3].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Father/Guardian Name"] = gvr.Cells[4].Text.Trim().Replace("&nbsp;", "");
                drcorrect["relogion"] = gvr.Cells[5].Text.Trim().Replace("&nbsp;", "");
                drcorrect["caste"] = gvr.Cells[6].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Admission No"] = gvr.Cells[7].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Date Of Leaving"] = gvr.Cells[8].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Last Class"] = gvr.Cells[9].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Joining Batch"] = gvr.Cells[10].Text.Trim().Replace("&nbsp;", "");
                drcorrect["Reason For Leaving"] = gvr.Cells[11].Text.Trim().Replace("&nbsp;", "");
                drcorrect["TC Number"] = gvr.Cells[12].Text.Trim().Replace("&nbsp;", "");
                drcorrect["JoiningBatchId"] = gvr.Cells[13].Text.Trim().Replace("&nbsp;", "");
                drcorrect["JoiningClassId"] = gvr.Cells[14].Text.Trim().Replace("&nbsp;", "");
                drcorrect["ReligionId"] = gvr.Cells[15].Text.Trim().Replace("&nbsp;", "");
                drcorrect["CastId"] = gvr.Cells[16].Text.Trim().Replace("&nbsp;", "");
                newcorrectdatas.Tables["newcorrectexceldatas"].Rows.Add(drcorrect);
            }

            drcorrect = dtnewcorrectdatas.NewRow();
            drcorrect["slno"] = lblselectedrowslno.Text;
            drcorrect["StudentName"] = txtstudname.Text.ToUpper();
            drcorrect["sex"] = RdBtnLstSex.SelectedItem.Text.ToUpper();
            drcorrect["Date Of Birth"] = txt_Dob.Text; ;
            drcorrect["Admission No"] = txtadmissionno.Text; ;
            drcorrect["Father/Guardian Name"] = Tx_FatherName.Text.ToString().Trim();
            drcorrect["relogion"] = Drp_Religoin.SelectedItem.ToString();
            drcorrect["caste"] = Drp_Cast.SelectedItem.ToString(); ;
            drcorrect["Date Of Leaving"] = Txt_DOL.Text.ToString(); ;
            drcorrect["Last Class"] = Drp_Class.SelectedItem.ToString();
            drcorrect["Joining Batch"] = Joining_Batch.SelectedItem.ToString();
            drcorrect["Reason For Leaving"] = Txt_reason.Text.ToString().Trim();
            drcorrect["TC Number"] = Txt_Tcno.Text.ToString().Trim();
            drcorrect["JoiningBatchId"] = Joining_Batch.SelectedValue.ToString(); ;
            drcorrect["JoiningClassId"] = Drp_Class.SelectedValue.ToString();
            drcorrect["ReligionId"] = Drp_Religoin.SelectedValue.ToString();
            drcorrect["CastId"] = Drp_Cast.SelectedValue.ToString();
            newcorrectdatas.Tables["newcorrectexceldatas"].Rows.Add(drcorrect);

            if (newcorrectdatas.Tables[0].Rows.Count > 0)
            {
                pnlValidrecords.Visible = true;
                FillCorrectDetailsToGrid(newcorrectdatas);
            }
            else
            {
                pnlValidrecords.Visible = false;
                FillCorrectDetailsToGrid(null);
                lblcorrect.Text = "Student details does not found";
            }
        }

        private bool NewValuesAreValid(out string msg)
        {
            bool _validvalues = true;
            DateTime tryparsedatetime;
            msg = null;
            if (txtstudname.Text == "")
            {
                _validvalues = false;
                msg = "Student Name Is Empty";
            }
            if (txt_Dob.Text != "")
            {
                string _dob = txt_Dob.Text.ToString().Trim();
                if (!MyUser.TryGetDareFromText(_dob, out tryparsedatetime))
                {
                    msg = msg + "<br>Date Of Birth is Invalid";
                    _validvalues = false;
                }
            }
            else
            {
                msg = msg + "<br>Date Of Birth is Empty";
                _validvalues = false;
            }
            if (txtadmissionno.Text == "")
            {
                _validvalues = false;
                msg = msg + "<br>Admission Number Is Empty";
            }
            else if (isAdmissionNoAlreadyPresent(txtadmissionno.Text))
            {
                _validvalues = false;
                msg = msg + "<br>Same Admission Number Already Exists. ";
            }


            if (int.Parse(Drp_Class.SelectedValue) <= 0)
            {
                _validvalues = false;
                msg = msg + "<br> Select Correct Class.";
            }

            return _validvalues;
        }

        private void fillpopupwindow()
        {


            ShowInvalidGridColumns();

            int i = grdincorrectdetails.SelectedRow.RowIndex;
            DateTime tryparsedate;
            //show the error
            lblerrordescription.Text = "Error:  " + grdincorrectdetails.SelectedRow.Cells[15].Text.Trim().Replace("&nbsp;", "");
            //show name
            txtstudname.Text = grdincorrectdetails.SelectedRow.Cells[1].Text.Trim().Replace("&nbsp;", "").ToUpper();
            // show selected sex
            if (grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[2].Text.ToString().ToLower() == "female")
                RdBtnLstSex.SelectedIndex = 1;
            else
                RdBtnLstSex.SelectedIndex = 0;
            // DOB
            if ((grdincorrectdetails.SelectedRow.Cells[3].Text.Trim() != "") && (DateTime.TryParse(grdincorrectdetails.SelectedRow.Cells[3].Text.Trim(), out tryparsedate)))
                txt_Dob.Text = MyUser.GerFormatedDatVal(tryparsedate);
            else
                txt_Dob.Text = "";
            //Faher Name
            if (grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[4].Text.ToString().ToLower() == "unknown" || grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[4].Text.ToString().ToLower() == " ")
                Tx_FatherName.Text = "";
            else
                Tx_FatherName.Text = grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[4].Text.ToString().Replace("&nbsp;", "");

            //Religion

            Drp_Religoin.SelectedValue = grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[16].Text.ToString().ToLower();

            //cast
            Drp_Cast.SelectedValue = grdincorrectdetails.Rows[grdincorrectdetails.SelectedIndex].Cells[17].Text.ToString().ToLower();

            //admission number
            txtadmissionno.Text = grdincorrectdetails.SelectedRow.Cells[7].Text.Trim().Replace("&nbsp;", "");
            //date of leaving
            if ((grdincorrectdetails.SelectedRow.Cells[8].Text.Trim() != "") && (DateTime.TryParse(grdincorrectdetails.SelectedRow.Cells[8].Text.Trim(), out tryparsedate)))
                Txt_DOL.Text = MyUser.GerFormatedDatVal(tryparsedate);
            else
                Txt_DOL.Text = "";
            //LoadBatch
            Joining_Batch.SelectedValue = grdincorrectdetails.SelectedRow.Cells[13].Text.Trim().Replace("&nbsp;", "");
            //LoadClass
            Drp_Class.SelectedValue = grdincorrectdetails.SelectedRow.Cells[14].Text.Trim().Replace("&nbsp;", "");
            //TC no
            Txt_Tcno.Text = grdincorrectdetails.SelectedRow.Cells[12].Text.Trim().Replace("&nbsp;", "");
            // Reason 
            Txt_reason.Text = grdincorrectdetails.SelectedRow.Cells[11].Text.Trim().Replace("&nbsp;", "");
            HideInvalidGridColumns();
        }

        private void LoadBatch()
        {
            Joining_Batch.Items.Clear();
            string sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1 ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Joining_Batch.Items.Add(li);

                }
            }
        }

        private void AddClassToDrpList()
        {

            Drp_Class.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status=1 AND  tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Class.Items.Add(li);

                }
                ListItem l1 = new ListItem("No Class Selected", "0");
                Drp_Class.Items.Add(l1);
            }
            else
            {
                ListItem li = new ListItem("No Class Found", "-1");
                Drp_Class.Items.Add(li);
            }
        }

        private void AddReligionToDrpList()
        {
            Drp_Religoin.Items.Clear();
            string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Religoin.Items.Add(li);

                }
            }
        }

        private void LoadCast()
        {
            Drp_Cast.Items.Clear();
            string sql = "select tblcast.Id, tblcast.castname from tblcast  where tblcast.castname <>'Other' order by tblcast.castname ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Cast.Items.Add(li);

                }
            }
        }

        private void FillCorrectDetailsToGrid(DataSet _CorrectDataset)
        {
            ShowValidGridColumns();

            if (_CorrectDataset != null && _CorrectDataset.Tables[0].Rows.Count > 0)
            {
                pnlValidrecords.Visible = true;
                grdcorrectdetails.DataSource = _CorrectDataset;
                grdcorrectdetails.DataBind();
            }
            else
            {
                grdcorrectdetails.DataSource = null;

                grdcorrectdetails.DataBind();
                pnlValidrecords.Visible = false;
            }

            HideValidGridColumns();

        }

        private void FillWrongDetailsToGrid(DataSet _WrongDataset)
        {

            ShowInvalidGridColumns();

            if (_WrongDataset != null && _WrongDataset.Tables[0].Rows.Count > 0)
            {
                grdincorrectdetails.DataSource = _WrongDataset;
                grdincorrectdetails.DataBind();
            }
            else
            {
                grdincorrectdetails.DataSource = null;
                grdincorrectdetails.DataBind();

                pnlInvalidrecords.Visible = false;
            }

            HideInvalidGridColumns();

        }

        private void ShowValidGridColumns()
        {
            grdcorrectdetails.Columns[1].Visible = true;
            grdcorrectdetails.Columns[2].Visible = true;
            grdcorrectdetails.Columns[3].Visible = true;
            grdcorrectdetails.Columns[4].Visible = true;
            grdcorrectdetails.Columns[5].Visible = true;
            grdcorrectdetails.Columns[6].Visible = true;
            grdcorrectdetails.Columns[7].Visible = true;
            grdcorrectdetails.Columns[8].Visible = true;
            grdcorrectdetails.Columns[9].Visible = true;
            grdcorrectdetails.Columns[10].Visible = true;
            grdcorrectdetails.Columns[11].Visible = true;
            grdcorrectdetails.Columns[12].Visible = true;
            grdcorrectdetails.Columns[13].Visible = true;
            grdcorrectdetails.Columns[14].Visible = true;
            grdcorrectdetails.Columns[15].Visible = true;
            grdcorrectdetails.Columns[16].Visible = true;
        }

        private void HideValidGridColumns()
        {
            grdcorrectdetails.Columns[1].Visible = false;
            grdcorrectdetails.Columns[2].Visible = false;
            grdcorrectdetails.Columns[3].Visible = false;
            grdcorrectdetails.Columns[4].Visible = false;
            grdcorrectdetails.Columns[5].Visible = false;
            grdcorrectdetails.Columns[6].Visible = false;
            grdcorrectdetails.Columns[7].Visible = false;
            grdcorrectdetails.Columns[8].Visible = false;
            grdcorrectdetails.Columns[9].Visible = false;
            grdcorrectdetails.Columns[10].Visible = false;
            grdcorrectdetails.Columns[11].Visible = false;
            grdcorrectdetails.Columns[12].Visible = false;
            grdcorrectdetails.Columns[13].Visible = false;
            grdcorrectdetails.Columns[14].Visible = false;
            grdcorrectdetails.Columns[15].Visible = false;
            grdcorrectdetails.Columns[16].Visible = false;

        }

        private void ShowInvalidGridColumns()
        {

            grdincorrectdetails.Columns[1].Visible = true;
            grdincorrectdetails.Columns[2].Visible = true;
            grdincorrectdetails.Columns[3].Visible = true;
            grdincorrectdetails.Columns[4].Visible = true;
            grdincorrectdetails.Columns[5].Visible = true;
            grdincorrectdetails.Columns[6].Visible = true;
            grdincorrectdetails.Columns[7].Visible = true;
            grdincorrectdetails.Columns[8].Visible = true;
            grdincorrectdetails.Columns[9].Visible = true;
            grdincorrectdetails.Columns[10].Visible = true;
            grdincorrectdetails.Columns[11].Visible = true;
            grdincorrectdetails.Columns[12].Visible = true;
            grdincorrectdetails.Columns[13].Visible = true;
            grdincorrectdetails.Columns[14].Visible = true;
            grdincorrectdetails.Columns[15].Visible = true;
            grdincorrectdetails.Columns[16].Visible = true;
            grdincorrectdetails.Columns[17].Visible = true;
        }

        private void HideInvalidGridColumns()
        {

            grdincorrectdetails.Columns[1].Visible = false;
            grdincorrectdetails.Columns[2].Visible = false;
            grdincorrectdetails.Columns[3].Visible = false;
            grdincorrectdetails.Columns[4].Visible = false;
            grdincorrectdetails.Columns[5].Visible = false;
            grdincorrectdetails.Columns[6].Visible = false;
            grdincorrectdetails.Columns[7].Visible = false;
            grdincorrectdetails.Columns[8].Visible = false;
            grdincorrectdetails.Columns[9].Visible = false;
            grdincorrectdetails.Columns[10].Visible = false;
            grdincorrectdetails.Columns[11].Visible = false;
            grdincorrectdetails.Columns[12].Visible = false;
            grdincorrectdetails.Columns[13].Visible = false;
            grdincorrectdetails.Columns[14].Visible = false;
            grdincorrectdetails.Columns[15].Visible = false;
            grdincorrectdetails.Columns[16].Visible = false;
            grdincorrectdetails.Columns[17].Visible = false;


        }
    }
}

