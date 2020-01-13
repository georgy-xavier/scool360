using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Collections;
using WinBase;

namespace WinEr
{
    public partial class RejectionList : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private DataSet MyDataSet = null;
        private OdbcDataReader MyReader = null;

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
                Response.Redirect("RoleErr.htm");
            }            
            else if (!MyUser.HaveActionRignt(820))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadRejectedStudentsList();
                }
            }
        }

        private void LoadRejectedStudentsList()
        {
            string sql = "select tblrejectedstudent.Id, tblrejectedstudent.StudentName, tblrejectedstudent.Sex, tblrejectedstudent.GardianName, tblrejectedstudent.AdmitionNo,tblrejectedstudent.Comment, tblstudtype.TypeName, tblreligion.Religion, tblbatch.BatchName, tblclass.ClassName,tblclass.Id as ClassId, tblrejectedstudent.OfficePhNo, tblrejectedstudent.TempStudentId from tblrejectedstudent inner join tblstudtype on tblstudtype.Id = tblrejectedstudent.StudTypeId inner join tblreligion  on tblrejectedstudent.Religion = tblreligion.Id  inner join tblbatch  on tblrejectedstudent.JoinBatch = tblbatch.Id  inner join tblclass on tblclass.Id = tblrejectedstudent.LastClassId order by tblrejectedstudent.StudentName";
            MyDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["RejectionList"] = MyDataSet;
                Img_Excel.Enabled = true;
                lbl_RejectionMessage.Text = "";
                Grd_RejectedList.Columns[0].Visible = true;
                Grd_RejectedList.Columns[10].Visible = true;
                Grd_RejectedList.Columns[11].Visible = true;
                Grd_RejectedList.Columns[12].Visible = true;
                Grd_RejectedList.DataSource = MyDataSet;
                Grd_RejectedList.DataBind();
                Grd_RejectedList.Columns[0].Visible = false;
                Grd_RejectedList.Columns[10].Visible = false;
                Grd_RejectedList.Columns[11].Visible = false;
                Grd_RejectedList.Columns[12].Visible = false;
            }
            else
            {
                Img_Excel.Enabled = false;
                Grd_RejectedList.DataSource = null;
                Grd_RejectedList.DataBind();
                lbl_RejectionMessage.Text = "No students in rejected list..";
            }
        }

        protected void Grd_RejectedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedStudentDetails();
            MPE_StudentDetails.Show();
        }

        private void LoadSelectedStudentDetails()
        {
            int _StudentId = int.Parse(Grd_RejectedList.SelectedRow.Cells[0].Text.Trim());

            string sql = "SELECT tblrejectedstudent.StudentName, tblrejectedstudent.AdmitionNo, tblrejectedstudent.DOB, tblrejectedstudent.Address, tblcast.castname, tblrejectedstudent.DateofJoining, tblrejectedstudent.Email, tblrejectedstudent.Location, tblrejectedstudent.Pin, tblrejectedstudent.State, tblrejectedstudent.Nationality, tblrejectedstudent.FatherOccupation, tbllanguage.Language, tblrejectedstudent.ResidencePhNo, tblrejectedstudent.OfficePhNo, tblrejectedstudent.UseBus, tblrejectedstudent.UseHostel from tblrejectedstudent inner join tblcast on tblrejectedstudent.Cast= tblcast.Id inner join tbllanguage on tblrejectedstudent.MotherTongue= tbllanguage.Id where tblrejectedstudent.Id=" + _StudentId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                lbl_Name.Text = MyReader.GetValue(0).ToString();
                lbl_Admission.Text = MyReader.GetValue(1).ToString();
                lbl_DOB.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(2).ToString()));
                lbl_Address.Text = MyReader.GetValue(3).ToString();
                lbl_Caste.Text = MyReader.GetValue(4).ToString();
                lbl_JoinDate.Text = MyUser.GerFormatedDatVal(DateTime.Parse(MyReader.GetValue(5).ToString()));
                lbl_Email.Text = MyReader.GetValue(6).ToString();
                lbl_Location.Text = MyReader.GetValue(7).ToString();
                lbl_Pin.Text = MyReader.GetValue(8).ToString();
                lbl_State.Text = MyReader.GetValue(9).ToString();
                lbl_Nation.Text = MyReader.GetValue(10).ToString();
                lbl_Father.Text = MyReader.GetValue(11).ToString();
                lbl_MotherTongue.Text = MyReader.GetValue(12).ToString();
                lbl_Phone.Text = MyReader.GetValue(13).ToString();
                lbl_Mobile.Text = MyReader.GetValue(14).ToString();
                if (MyReader.GetValue(15).ToString() == "0")
                {
                    lbl_Bus.Text = "No";
                }
                else
                {
                    lbl_Bus.Text = "Yes";
                }
                if (MyReader.GetValue(16).ToString() == "0")
                {
                    lbl_Hostel.Text = "No";
                }
                else
                {
                    lbl_Hostel.Text = "Yes";
                }
            }
        }

        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _ExcelDataSet = null;
            string sql = "select tblrejectedstudent.StudentName, tblrejectedstudent.Sex, tblrejectedstudent.GardianName as GuardianName, tblrejectedstudent.AdmitionNo as AdmissionNumber,tblrejectedstudent.Comment as ReasonForReject , tblrejectedstudent.DOB, tblstudtype.TypeName, tblreligion.Religion, tblcast.castname as Caste, tblbatch.BatchName, tblclass.ClassName,  tblrejectedstudent.Address, tblrejectedstudent.DateofJoining, tblrejectedstudent.Email, tblrejectedstudent.Location, tblrejectedstudent.Pin, tblrejectedstudent.State, tblrejectedstudent.Nationality, tblrejectedstudent.FatherOccupation, tbllanguage.Language, tblrejectedstudent.ResidencePhNo, tblrejectedstudent.OfficePhNo as MobileNumber, tblrejectedstudent.TempStudentId from tblrejectedstudent inner join tblstudtype on tblstudtype.Id = tblrejectedstudent.StudTypeId inner join tblreligion  on tblrejectedstudent.Religion = tblreligion.Id  inner join tblbatch  on tblrejectedstudent.JoinBatch = tblbatch.Id  inner join tblclass on tblclass.Id = tblrejectedstudent.LastClassId inner join tblcast on tblrejectedstudent.Cast= tblcast.Id inner join tbllanguage on tblrejectedstudent.MotherTongue= tbllanguage.Id order by tblrejectedstudent.StudentName";
            _ExcelDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_ExcelDataSet.Tables[0].Rows.Count > 0)
            {              
                if(!ExcelUtility.ExportDataSetToExcel(_ExcelDataSet,"RejectedStudentsList"))
                {
                    lbl_RejectionMessage.Text="MS Excel is missing.Please install and try again..";
                }
            }
            else
            {
                lbl_RejectionMessage.Text = "No students in rejection list..";
            }
        }

    }
}
