using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;
using AjaxControlToolkit;
namespace WinEr
{
    public partial class RegisterTempStudent : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
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
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(124))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {
                    
                    //Pnl_TempList.Visible = false;
              


                    clearData();
                    AddStandardToDrpList();
                    AddClassTODrpList();
                    LoadAccedemicYearToDrpList();
                    AddBloodGrpToDrpList(16);
                    AddMotherTongueToDrpList(0);
                    pnlInterviewDetails.Visible = false;


                }
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
                AddCoustomControls();
                
            }
        }




        #region CREATE TEMP STUDENTS

        private void LoadAccedemicYearToDrpList()
        {
            Drp_AccYear.Items.Clear();

            string sql = "select tblbatch.BatchName ,tblbatch.Id from tblbatch where `status`=1";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                Drp_AccYear.Items.Add(li);

                int Nxtbatch = int.Parse(MyReader.GetValue(1).ToString()) + 1;
                sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id=" + Nxtbatch;
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_AccYear.Items.Add(li);

                }
            }
        }

        private void AddClassTODrpList()
        {
            Drp_Class.Items.Clear();
            if (Drp_Standard.SelectedValue != "")
            {
                string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status AND tblclass.Standard='" + int.Parse(Drp_Standard.SelectedValue.ToString()) + "' AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + "))";
                MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Btn_Save.Enabled = true;
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    ListItem li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                    Btn_Save.Enabled = false;
                }
            }
        }

        private void AddStandardToDrpList()
        {
            Drp_Standard.Items.Clear();


            string sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Save.Enabled = true;
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Standard.Items.Add(li);

                }
            }
            else
            {
                ListItem li = new ListItem("No Standard Found", "-1");
                Drp_Standard.Items.Add(li);
                Btn_Save.Enabled = false;
            }
        }

        protected void Drp_Standard_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddClassTODrpList();
            Drp_Class.Focus();
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            bool _continue = false;
            int Studentid = 0;
            int InterviewStatusId = 1;
            int status = 1;

            if (rdoInterView.SelectedValue == "Attended")
            {
                if (rdoResult.SelectedValue == "Selected")
                {
                    InterviewStatusId = 4;

                }
                else if (rdoResult.SelectedValue == "Hold")
                    InterviewStatusId = 6;
                else
                    InterviewStatusId = 5;
            }
            General _GenObj = new General(MyStudMang.m_MysqlDb);
            int interviewrank=0;
            int.TryParse(txt_InterviewMark.Text, out interviewrank);
            double anual_Income=0;
            double.TryParse(Txt_annual_incum.Text.Trim(), out anual_Income);

            int pin = 0;
            int.TryParse(Txt_PinCode.Text.Trim(), out pin);
            //string Tempid = MyStudMang.CreatetempStudent(Txt_Name.Text.Trim().ToUpperInvariant(), Txt_FatherName.Text.Trim().ToUpperInvariant(), RadioBtn_Sex.SelectedItem.Text, int.Parse(Drp_Standard.SelectedValue), int.Parse(Drp_Class.SelectedValue), int.Parse(Drp_AccYear.SelectedValue), TxtAddress.Text.Trim().ToUpperInvariant(), Txt_Ph.Text.Trim(), interviewrank, MyUser.UserName, Txt_Location.Text.Trim(), Txt_State.Text.Trim(),pin , int.Parse(Drp_BloodGrp.SelectedValue), Txt_nationality.Text.Trim(), int.Parse(Drp_MotherTongue.SelectedValue), Txt_father_educ.Text.Trim(), Txt_Mothers_educ.Text.Trim(), Txt_fathers_Ocuptn.Text.Trim(), anual_Income, Txt_Email.Text.Trim(), txtMotherName.Text, _GenObj.GetDateFromText(Txt_Dob.Text.Trim()), txtStudRemark.Text, txtPreviousBoard.Text, rdoInterView.SelectedValue, txtDOI.Text, txtTeacherRemark.Text, txtHMRemark.Text, txtPrincipalRemark.Text, rdoResult.SelectedValue.ToString(), InterviewStatusId, status, out _continue, out Studentid);
            string Tempid = MyStudMang.CreatetempStudent(Txt_Name.Text.Trim().ToUpperInvariant(), Txt_FatherName.Text.Trim().ToUpperInvariant(), RadioBtn_Sex.SelectedItem.Text, int.Parse(Drp_Standard.SelectedValue), int.Parse(Drp_Class.SelectedValue), int.Parse(Drp_AccYear.SelectedValue), TxtAddress.Text.Trim().ToUpperInvariant(), Txt_Ph.Text.Trim(), interviewrank, MyUser.UserName, Txt_Location.Text.Trim(), Txt_State.Text.Trim(), pin, int.Parse(Drp_BloodGrp.SelectedValue), Txt_nationality.Text.Trim(), int.Parse(Drp_MotherTongue.SelectedValue), Txt_father_educ.Text.Trim(), Txt_Mothers_educ.Text.Trim(), Txt_fathers_Ocuptn.Text.Trim(), anual_Income, Txt_Email.Text.Trim(), txtMotherName.Text, _GenObj.GetDateFromText(Txt_Dob.Text.Trim()), txtStudRemark.Text, txtPreviousBoard.Text, rdoInterView.SelectedValue, txtDOI.Text, txtTeacherRemark.Text, txtHMRemark.Text, txtPrincipalRemark.Text, rdoResult.SelectedValue.ToString(), InterviewStatusId, status, Txt_mothers_Ocuptn.Text, out _continue, out Studentid);
            if (_continue)
            {
                Hdn_studid.Value = Studentid.ToString();
                Hdn_Standid.Value = Drp_Standard.SelectedValue;

                UpDateCoustomFields(Tempid);
                Lbl_Message.Text = "The student " + Txt_Name.Text + " is added to temporary list";
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Temporary student Creation", "The student " + Txt_Name.Text + " is Created", 1,1);
                if (MyUser.HaveActionRignt(605))
                {

                    //MPE_Temp_Fee.Show();
                    Response.Redirect("CollectJoiningFee.aspx?Studentid=" + Hdn_studid.Value);
                }
                else
                {
                    Lbl_Message.Text = "The student " + Txt_Name.Text + " is added to temporary list. To collect fee Login as an authorized user";
                }
            }

            clearData();

        }

        private void clearData()
        {
            txt_InterviewMark.Text = "";
            Txt_FatherName.Text = "";
            Txt_Name.Text = "";
            Txt_Ph.Text = "0";
            TxtAddress.Text = "";
            txt_InterviewMark.Text = "";
            Txt_annual_incum.Text = "0";
            Txt_Email.Text = "";
            Txt_nationality.Text = "";
            Txt_father_educ.Text = "";
            Txt_fathers_Ocuptn.Text = "";
            Txt_Mothers_educ.Text = "";
            Txt_Location.Text = "";

            Txt_State.Text = "";
            Txt_PinCode.Text = "0";
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

        protected void Show_MoreInfo_Click(object sender, EventArgs e)
        {
            Panel_MoreInfo.Visible = true;
            Hide_MoreInfo.Visible = true;
            Show_MoreInfo.Visible = false;
           // AddCoustomControls();
        }

        protected void Hide_MoreInfo_Click(object sender, EventArgs e)
        {
            Panel_MoreInfo.Visible = false;
            Hide_MoreInfo.Visible = false;
            Show_MoreInfo.Visible = true;
        }

        protected void rdoInterView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoInterView.SelectedValue != "Attended")
                pnlInterviewDetails.Visible = false;
            else
                pnlInterviewDetails.Visible = true;
        }


        #endregion


        private void AddCoustomControls()
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
                        i++;

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
            return valid;
        }





    }
}
