using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Reflection;
using System.Diagnostics;

namespace WinEr
{
    public partial class WinerAboutPage : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;

        public WinerAboutPage()
        {
            this.PreInit += new EventHandler(BaseInit);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
            {
                Response.Redirect("Default.aspx");
                
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadDefaultvalues();
                }
            }
        }

        private void LoadDefaultvalues()
        {
            string _LastChanges = " ", Date = " ",patch=" ";
            Lbl_db.Text = GetDBVersion(out _LastChanges,out Date,out patch);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            Lbl_winer.Text = version;
            DateTime t_date = new DateTime();
            // DateTime.TryParse(Date, out t_date);
          
            try
            {
                t_date = Convert.ToDateTime(Date);
                Lbl_date.Text = String.Format("{0:dd/MM/yyyy}", t_date);
            }
            catch
            {
                Lbl_date.Text = "";
            }
            
           
            Lbl_patch.Text = patch;
            int i = 0;
            if (_LastChanges != " ")
            {
                string[] listStr = _LastChanges.Split(',');
                string innerHtmlstr = "";
                innerHtmlstr = "<table class=\"tablelist\"><tr><td class=\"leftside\"></td><td class=\"rightside\"><ul type=\"square\">";
                foreach (string str in listStr)
                {
                    innerHtmlstr += "<li><font size=\"2\" color=\"red\">" + str + "</font></li>";
                    i++;
                }
                innerHtmlstr += "</ul></td></tr></table>";
                this.Table.InnerHtml = innerHtmlstr;
            }
          
        }

        private string GetDBVersion(out string _LastChanges,out string _date,out string _patch)
        {
            string str = " ";
            _LastChanges = " ";
            _date = " ";
            _patch = " ";
            string sql = " ";
            DataSet ds = new DataSet();
            try
            {

                #region Schoolds

                sql = "SELECT tblversiondetails.Sysversion,tblversiondetails.Patchversion,tblversiondetails.Date FROM tblversiondetails";
                ds = MyUser.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        str = dr[0].ToString();
                        _patch = dr[1].ToString();
                        //_date =dr[2].ToString();
                    }

                }

                #endregion

                ds = null;

                #region centerl db

                sql = "SELECT tblversiondetails.LastChanges,tblversiondetails.LastUpdate_Date from tblversiondetails where tblversiondetails.winerversion='" + str + "' and tblversiondetails.Patchversion='" + _patch + "'";
                ds = WinerUtlity.GetaboutsoftwareDetails(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        _LastChanges = dr[0].ToString();
                        _date =dr[1].ToString();
                    }
                }

                #endregion
            }
            catch
            {
                str = "Not Found";
                _LastChanges = "Date is Not Found or tblversiondetails table is not found!";
                _date = "Date is Not Found or tblversiondetails table is not found!";
                _patch = "Not Found";
            }
            return str;
        }

        void BaseInit(object sender, EventArgs e)
        {
            string Cond = Request["value1"];
            if (Cond == "a")
                this.Page.MasterPageFile = "~/WinerSchoolMaster.Master";
        }

    }
}
