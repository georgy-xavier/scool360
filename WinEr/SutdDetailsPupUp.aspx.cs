using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Text;

namespace WinEr
{
    public partial class SutdDetailsPupUp : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private int m_StudId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (!IsPostBack)
            {
                bool valid = false;
                PanelStudent.Visible = true;
                PanelError.Visible = true;
                if (Request.QueryString["StudId"] != null)
                {
                    if (int.TryParse(Request.QueryString["StudId"].ToString(), out m_StudId))
                    {
                        valid = true;
                    }
                }
                if (valid)
                {
                    LoadStudentDetails();
                    PanelError.Visible = false;
                }
                else
                {
                    PanelStudent.Visible = false;
                }
            }

        }

        private void LoadStudentDetails()
        {

            LoadPermenenteStudentDetails();

        }

        private void LoadPermenenteStudentDetails()
        {
            string sql = "SELECT  tblstudent.StudentName,tblstudent.GardianName,tblstudent.AdmitionNo,tblstudent.Sex,tblstudent.Address,tblstudent.ResidencePhNo,tblstudent.OfficePhNo,tblstudent.CreatedUserName,tblclass.ClassName ,tblstandard.Name ,tblbatch.BatchName FROM tblstudent INNER JOIN tblstudentclassmap ON tblstudentclassmap.StudentId=tblstudent.Id INNER JOIN tblclass ON tblclass.Id=tblstudentclassmap.ClassId INNER JOIN tblstandard ON tblstandard.Id=tblstudentclassmap.Standard INNER JOIN tblbatch ON tblbatch.Id=tblstudent.JoinBatch WHERE tblstudent.Id=" + m_StudId;
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                lbl_FullName.Text = MyReader.GetValue(0).ToString();
                lbl_GardianName.Text = MyReader.GetValue(1).ToString();
                Lbl_AdmitionNo.Text = MyReader.GetValue(2).ToString();
                lbl_Sex.Text = MyReader.GetValue(3).ToString();
                lblAdress.InnerHtml = MyReader.GetValue(4).ToString();
                lbl_ResidencePhNo.Text = MyReader.GetValue(5).ToString();
                lbl_OfficePhNo.Text = MyReader.GetValue(6).ToString();
                Lbl_CreatedBy.Text = MyReader.GetValue(7).ToString();
                lbl_Class.Text = MyReader.GetValue(8).ToString();
                lbl_standard.Text = MyReader.GetValue(9).ToString();
                Lbl_JoinBatch.Text = MyReader.GetValue(10).ToString();
                string url = "Handler/ImageReturnHandler.ashx?id=" + m_StudId + "&type=StudentImage";
        
                ImageStd.ImageUrl = url;
            }
            else
            {
                PanelStudent.Visible = false;
                PanelError.Visible = true;
            }
        }



        
    }
}
