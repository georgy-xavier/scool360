using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class RFswipingRecord : System.Web.UI.Page
    {
        private StudentManagerClass MyStudentMang;
        private KnowinUser MyUser;
        private OdbcDataReader Myreader = null;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudentMang = MyUser.GetStudentObj();
            if (MyStudentMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(839))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {

                if (!IsPostBack)
                {
                    img_export_Excel.Visible = false;
                    LoadDrp();
                }
            }
        }

        #endregion

        private void LoadDrp()
        {
            LoadClassDrp();
            LoadStudentDrp();

        }

        private void LoadStudentDrp()
        {
            Drp_Student.Items.Clear();
            string sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + " and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " order by map.RollNo";
            Myreader = MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
            if (Myreader.HasRows)
            {
                while (Myreader.Read())
                {
                    ListItem li = new ListItem(Myreader.GetValue(1).ToString(), Myreader.GetValue(0).ToString());
                    Drp_Student.Items.Add(li);
                }

            }
            else
            {
                ListItem li = new ListItem("No students found", "-1");
                Drp_Student.Items.Add(li);
            }
            Myreader.Close();
        }

        private void LoadClassDrp()
        {
            Drp_Class.Items.Clear();
            DataSet MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedIndex = 0;
        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentDrp();
        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            Load_Grid();
           
        }

        private void Load_Grid()
        {
            lbl_error.Text = "";
            string sql = "SELECT tblexternalrfid.Location as RFReaderName,Date_Format(tblexternalattencence.ActionDate,'%d/%m/%Y %h:%i %p') as ActionDate,tblexternalattencence.RFReaderType as RFReaderType FROM tblexternalattencence INNER JOIN tblexternalrfid ON tblexternalattencence.RFReaderID=tblexternalrfid.EquipmentId INNER JOIN tblexternalreff ON tblexternalattencence.ExternalReffid=tblexternalreff.Id WHERE   tblexternalreff.UserType='STUDENT' and tblexternalreff.UserId=" + Drp_Student.SelectedValue;
            DataSet MyDataset = MyStudentMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataset != null && MyDataset.Tables[0].Rows.Count > 0)
            {
                img_export_Excel.Visible = true;
                Grd_SwipeLogg.DataSource = MyDataset;
                Grd_SwipeLogg.DataBind();
            }
            else
            {
                lbl_error.Text = "No report found";
                Grd_SwipeLogg.DataSource = null;
                Grd_SwipeLogg.DataBind();
                img_export_Excel.Visible = false;
            }
            ViewState["Report"] = MyDataset;
        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            DataSet MyData = (DataSet)ViewState["Report"];
            if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "Student-House Map Report.xls"))
            {
                lbl_error.Text = "";
            }  
        }

        protected void Grd_SwipeLogg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            Grd_SwipeLogg.PageIndex = e.NewPageIndex;
            Load_Grid();

        }

        protected void Btn_ClearAll_Click(object sender, EventArgs e)
        {
            string _msg = "";
            if (_IsDeletionPossible(out _msg))
            {
                try
                {
                    DateTime _date = General.GetDateTimeFromText(Txt_StartDate.Text);
                    string sql = "DELETE  tblexternalattencence from tblexternalattencence INNER JOIN tblexternalreff  WHERE tblexternalattencence.ActionDate<'" + _date.Date.ToString("s") + "' and tblexternalattencence.ExternalReffid=tblexternalreff.Id   and tblexternalreff.UserType='STUDENT'";
                    MyStudentMang.m_MysqlDb.ExecuteQuery(sql);
                    Load_Grid();
                    lbl_error.Text = "Deletion done successfully";
                }
                catch
                {
                    lbl_error.Text = "Error while deletion. Try later.";
                }
            }
            else
            {
                lbl_error.Text = _msg;
            }
        }

        private bool _IsDeletionPossible(out string _msg)
        {
            _msg = "";
            bool _valid = true;
            if (Txt_StartDate.Text == "")
            {
                _msg = "Please enter date till data can be deleted";
                _valid = false;
            }
            else if (General.GetDateTimeFromText(Txt_StartDate.Text) > DateTime.Now.Date.AddDays(-30))
            {
                    _msg = "Date should be less than 30 days";
                    _valid = false;
            }
            return _valid;
        }


    }
}
