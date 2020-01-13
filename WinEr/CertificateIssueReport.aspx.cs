using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class CertificateIssueReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private ConfigManager MyConfiMang;
        private KnowinUser MyUser;
        private OdbcDataReader myReader = null;
        private DataSet myDataset = null;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfiMang = MyUser.GetConfigObj();
            MyStudMang = MyUser.GetStudentObj();
            if (MyConfiMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(3016))
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
                    load_initials();

                }
            }

        }
        private void load_initials()
        {
            LoadAllClassToDropDown();
            Load_Issue_User();
            LoadDateInTextBox();
            LoadCertificateDrp();
            Pnl_Report.Visible = false;
            Lbl_Err.Text = "";
            Btn_export.Visible = false;
        }
        private void LoadAllClassToDropDown()
        {
            Drp_Class.Items.Clear();
            myDataset = MyUser.MyAssociatedClass();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in myDataset.Tables[0].Rows)
                {

                    ListItem li2 = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li2);

                }
            }
            else
            {
                ListItem li3 = new ListItem("No Class present", "0");
                Drp_Class.Items.Add(li3);
            }
            Drp_Class.SelectedIndex = 0;
        }
        private void LoadCertificateDrp()
        {
            Drp_Certificate.Items.Clear();
            string sql = "SELECT DISTINCT tblcertificatemaster.Id,tblcertificatemaster.CertificateHead FROM tblcertificatemaster";
            myReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (myReader.HasRows)
            {
                Drp_Certificate.Items.Add(new ListItem("All", "0"));
                while (myReader.Read())
                {
                    Drp_Certificate.Items.Add(new ListItem(myReader.GetValue(1).ToString(), myReader.GetValue(0).ToString()));
                }
              
            }
            else
            {
                Drp_Certificate.Items.Add(new ListItem("No Type Found", "0"));
            }
            Drp_Certificate.SelectedIndex = 0;
        }

        private void Load_Issue_User()
        {

            Drp_User.Items.Clear();
            DataSet ds_users = new DataSet();
            string sql = "select Id,UserName from tbluser ";
            ds_users = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_users != null && ds_users.Tables != null && ds_users.Tables[0].Rows.Count > 0)
            {
                ListItem li = new ListItem("All", "0");
                Drp_User.Items.Add(li);
                foreach (DataRow dr in ds_users.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_User.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No user found", "0");
                Drp_User.Items.Add(li);
            }
            Drp_User.SelectedIndex = 0;
        }

        protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDateInTextBox();
        }
        private void LoadDateInTextBox()
        {
            string _sdate = null, _edate = null;
            if (Drp_Period.SelectedIndex == 0)
            {
                DateTime _date = System.DateTime.Today;
                _sdate = MyUser.GerFormatedDatVal(_date);

                Txt_SDate.Enabled = false;
                Txt_EDate.Enabled = false;
                Txt_SDate.Text = _sdate;
                Txt_EDate.Text = _sdate;
            }
            if (Drp_Period.SelectedIndex  == 1)
            {
                DateTime _date = System.DateTime.Now;
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = _date.AddDays(-7);
                _sdate = MyUser.GerFormatedDatVal(_start);

                Txt_SDate.Enabled = false;
                Txt_EDate.Enabled = false;
                Txt_SDate.Text = _sdate;
                Txt_EDate.Text = _edate;
            }
            if (Drp_Period.SelectedIndex == 2)
            {
                DateTime _date = System.DateTime.Now;
                //_edate = _date.ToString("dd/MM/yyyy");
                _edate = MyUser.GerFormatedDatVal(_date);

                DateTime _start = System.DateTime.Now;
                //_sdate = _start.Date.ToString("01/MM/yyyy");
                _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                Txt_EDate.Enabled = false;
                Txt_SDate.Enabled = false;
                Txt_SDate.Text = _sdate;
                Txt_EDate.Text = _edate;

            }
            if (Drp_Period.SelectedIndex == 3)
            {
                Txt_EDate.Enabled = true;
                Txt_SDate.Enabled = true;
                Txt_SDate.Text = "";
                Txt_EDate.Text = "";
            }
        }

        protected void Btn_Show_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                if (_validations(out msg))
                {
                    Load_Grid(out msg);
                }
                Lbl_Err.Text = msg;
            }
            catch (Exception et)
            {
                Lbl_Err.Text = et.Message;
            }

        }
        private bool _validations(out string msg)
        {
            bool check = true;
            msg = "";
            DateTime _from = MyUser.GetDareFromText(Txt_SDate.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_EDate.Text.ToString());
            if (_from > _To)
            {
                msg = "From date should not be greater than to Date";
                check = false;
            }
            return check;
        }
        private void Load_Grid(out string _msg)
        {
            _msg = "";
            Lbl_Err.Text = "";
            string temp_sql="";
            Grd_Report.Columns[0].Visible = true;
            Grd_Report.Columns[1].Visible = true;
            DateTime _from = MyUser.GetDareFromText(Txt_SDate.Text.ToString());
            DateTime _To = MyUser.GetDareFromText(Txt_EDate.Text.ToString());
            if (_To == DateTime.Parse(DateTime.Now.ToString("d")))
            {
                _To = DateTime.Now;
            }
            DataSet ds_report = new DataSet();
            string sql = "select Id,Student_Id,Student_Name,Class_Name,Certificate_Name,Created_Time,Created_User from tblcertificatemanager where  Created_Time >='" + _from.Date.ToString("s") + "' AND  Created_Time <='" + _To.ToString("s") + "'";
            if (Drp_Class.SelectedIndex !=0)
            {

                temp_sql = temp_sql + " and Class_Id=" + int.Parse(Drp_Class.SelectedIndex.ToString()) + "";

            }
            if (Drp_User.SelectedIndex != 0)
            {

                temp_sql = temp_sql + " and Created_User='" + Drp_User.SelectedItem.Text.ToString() + "'";
            }
            if (Drp_Certificate.SelectedIndex != 0)
            {
                temp_sql = temp_sql + " and Certificatetype_Id=" + int.Parse(Drp_Certificate.SelectedIndex.ToString()) + "";
            }
            sql = sql + temp_sql;
            ds_report = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_report != null && ds_report.Tables[0] != null && ds_report.Tables[0].Rows.Count > 0)
            {
                Pnl_Report.Visible = true;
                Btn_export.Visible = true;
                Session["ds_excel"] = ds_report;
                Grd_Report.DataSource = ds_report;
                Grd_Report.DataBind();
                Grd_Report.Columns[0].Visible = false;
                Grd_Report.Columns[1].Visible = false;
            }
            else
            {
                Pnl_Report.Visible = false;
                Btn_export.Visible = false;
                _msg = "No Data Exists";
            }

        }
        protected void Grd_Report_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            string Certify_Text = "";
            string Need_Boarder = "";
            Grd_Report.Columns[0].Visible = true;
            Grd_Report.Columns[1].Visible = true;
            try
            {
                id = int.Parse(Grd_Report.SelectedRow.Cells[0].Text.ToString());
                Get_certifydetails(id,out Certify_Text,out Need_Boarder);
                Session["CertificateData"] = Certify_Text;
                Session["StudId"] = int.Parse(Grd_Report.SelectedRow.Cells[1].Text.ToString());
                Grd_Report.Columns[0].Visible = false;
                Grd_Report.Columns[1].Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "window.open(\"PrintCertificate.aspx?NeedBoarder=" + Need_Boarder + "\");", true);

            }

            catch
            {
                Grd_Report.Columns[0].Visible = false;
                Lbl_Err.Text = "can't print this time try again";
            }
        }
        private void Get_certifydetails(int Id,out string Certify_Text,out string Need_border)
        {
            Certify_Text = "";
            Need_border = "";
            string sql = "SELECT Certificate_Text,Need_Border FROM tblcertificatemanager where Id=" + Id + "";
            myReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (myReader.HasRows)
            {
                Certify_Text=myReader.GetValue(0).ToString();
                Need_border=myReader.GetValue(1).ToString();
            }
        }
        protected void Grd_Report_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string _msg="";
            Grd_Report.PageIndex = e.NewPageIndex;
            Load_Grid(out _msg);

        }
        protected void Btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = "CertificateIssueReport";
                string _ReportName = "CertificateIssueReport";
                DataSet ds_excels = new DataSet();
                ds_excels = buildDataset();
                if (ds_excels != null && ds_excels.Tables[0] != null && ds_excels.Tables[0].Rows.Count > 0)
                {
                    if (!WinEr.ExcelUtility.ExportDataToExcel(ds_excels, _ReportName, FileName, MyUser.ExcelHeader))
                    {
                        Lbl_Err.Text = "This function need Ms office";
                    }
                }
                else
                {
                    Lbl_Err.Text = "Export to Excel fail,try again";
                }
            }
            catch
            {
                Lbl_Err.Text = "Export to Excel fail,try again";
            }
        }
        private DataSet buildDataset()
        {
            DataSet dataset = new DataSet();
            DataTable dt;
            DataRow dr;
            dataset.Tables.Add(new DataTable("Reqtbl"));
            dt = dataset.Tables["Reqtbl"];
            dt.Columns.Add("Student_Name");
            dt.Columns.Add("Class_Name");
            dt.Columns.Add("Certificate_Name");
            dt.Columns.Add("Created_Date");
            dt.Columns.Add("Created_User");
            DataSet ds_excel = (DataSet)Session["ds_excel"];
            if (ds_excel != null && ds_excel.Tables[0] != null && ds_excel.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr2 in ds_excel.Tables[0].Rows)
                {

                    dr = dataset.Tables["Reqtbl"].NewRow();
                    dr["Student_Name"] = dr2["Student_Name"].ToString();
                    dr["Class_Name"] =  dr2["Class_Name"].ToString();
                    dr["Certificate_Name"] =  dr2["Certificate_Name"].ToString();
                    dr["Created_Date"] = dr2["Created_Time"].ToString();
                    dr["Created_User"] = dr2["Created_User"].ToString();
                    dataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }
            return dataset;
        }

    }
}
