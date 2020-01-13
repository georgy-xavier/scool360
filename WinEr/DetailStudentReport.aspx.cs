using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class WebForm13 : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

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
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(2042))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                lblsuccess.Visible = false;
                if (!IsPostBack)
                {
                    LoadAllClassDetailsToDropDown();
                    LoadCastCategoryToDropDown();
                    Img_Excel.Visible = false;
                    Img_Excel.Enabled = false;
                    Panel_CastArea.Visible = false;
                }
            }

        }
        private void LoadAllClassDetailsToDropDown()
        {
            Drp_Class.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_Class.DataTextField="ClassName";
                Drp_Class.DataValueField="Id";
                Drp_Class.DataSource = MydataSet.Tables[0];
                Drp_Class.DataBind();

            }
            else
            {                
                Drp_Class.Items.Add(new ListItem("No Class present", "-1"));
            }
            Drp_Class.SelectedIndex = 0;
        }

        private void LoadCastCategoryToDropDown()
        {
            Drp_castcategory.Items.Clear();
            string sql = " select tblcast_category.Id , tblcast_category.CategoryName from tblcast_category ORDER BY tblcast_category.CategoryName ASC  ";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
           
            if (MyReader.HasRows)
            {
                Drp_castcategory.Items.Add(new ListItem("ALL", "0"));

                while (MyReader.Read())
                {                 
                    Drp_castcategory.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
                }
            }
            else
            {
                Drp_castcategory.Items.Add(new ListItem("No Religion", "-1"));
            }
        }

        private void loadstudentdetails()
        {
            DataSet CasteDataSet = new DataSet();
            DataSet StudentDataset = new DataSet();
            DataSet StudentDatasetcount = new DataSet();
            DataSet CountDataSet = new DataSet();
            DataTable dt;
            DataRow dr;
            CountDataSet.Tables.Add(new DataTable("CountList"));
            dt = CountDataSet.Tables["CountList"];
            dt.Columns.Add("CasteName");
            dt.Columns.Add("MALE");
            dt.Columns.Add("FEMALE");
            dt.Columns.Add("TOTAL");
           
            string sql1 = "select DISTINCT tblcast.id, tblcast.castname from tblcast inner join tblstudent on tblstudent.Cast= tblcast.Id ";
           
            if (int.Parse(Drp_castcategory.SelectedValue) > 0)
            {
                sql1 = sql1 + " where tblcast.CategoryId=" + Drp_castcategory.SelectedValue;
            }
            sql1 = sql1 + " order by tblcast.castname asc";
           
            CasteDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql1);

            if (CasteDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Caste in CasteDataSet.Tables[0].Rows)
                {
                    if (isstudentexistincast(int.Parse(Dr_Caste[0].ToString())))
                    {
                        int maleStudentTotal = 0, femalestudenttotal = 0;
                        dr = dt.NewRow();
                        dr["CasteName"] = Dr_Caste[1].ToString();
                        
                        string sqlmalecount = "SELECT tblstudent.Id, tblstudentclassmap.ClassId from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.`Cast`=" + Dr_Caste[0].ToString() + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 and tblstudentclassmap.ClassId='" + Drp_Class.SelectedValue + "'  and tblstudent.Sex='Male' order by tblstandard.Id, tblclass.ClassName";
                        StudentDataset = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlmalecount);

                        if (StudentDataset.Tables[0].Rows.Count > 0)
                        {
                            maleStudentTotal = StudentDataset.Tables[0].Rows.Count;
                        }                                                   
                         dr["MALE"] = maleStudentTotal;
                        string sqlfemalecount = "SELECT tblstudent.Id, tblstudentclassmap.ClassId from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.`Cast`=" + Dr_Caste[0].ToString() + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 and tblstudentclassmap.ClassId='" + Drp_Class.SelectedValue + "'  and tblstudent.Sex='Female' order by tblstandard.Id, tblclass.ClassName";
                        StudentDatasetcount = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlfemalecount);
                        if (StudentDatasetcount.Tables[0].Rows.Count > 0)
                        {
                            femalestudenttotal = StudentDatasetcount.Tables[0].Rows.Count;
                        }

                        dr["FEMALE"] = femalestudenttotal;
                        dr["TOTAL"] = maleStudentTotal + femalestudenttotal;
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (CountDataSet.Tables[0].Rows.Count > 0)
            {
                Grd_CountList.DataSource = CountDataSet;
                Grd_CountList.DataBind();
                Img_Excel.Enabled = true;
                Img_Excel.Visible = true;
                ViewState["CountList"] = CountDataSet;
                Img_Excel.Enabled = true;
                Panel_CastArea.Visible = true;
            }
            else
            {
                Grd_CountList.DataSource = null;
                Grd_CountList.DataBind();
                Img_Excel.Enabled = false;
                Img_Excel.Visible = false;
                Lbl_msg.Text = "No Students Exist";
                this.MPE_MessageBox.Show();
                Panel_CastArea.Visible = false;
            }
        }
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                loadstudentdetails();
                string classname = Drp_Class.SelectedItem.Text.ToString();
                Session["clsname"] = classname;
            }
            catch (Exception ex)
            {
                lblsuccess.Visible =true;
                lblsuccess.Text = ex.Message;
            }
        }
        private bool isstudentexistincast(int castid)
        {
            bool check = false;
            DataSet Studentcount = new DataSet();
            string sqlstudentcount = " SELECT tblstudent.Id from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.`Cast`=" + castid + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 and tblstudentclassmap.ClassId='" + Drp_Class.SelectedValue + "'";
            Studentcount = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sqlstudentcount);
            if (Studentcount != null && Studentcount.Tables[0] != null && Studentcount.Tables[0].Rows.Count > 0)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return check;
        }
        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            DataSet _CountReportList = new DataSet();
            _CountReportList = (DataSet)ViewState["CountList"];

            string FileName = "DetailStudentReport";
            string _ReportName = "<table><tr><td colspan=\"7\" style=\"text-align:center;\"><b>Detail Student Report of Class:" + Session["clsname"].ToString() + "</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td></td></tr></table>";
            if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
            {
                Lbl_msg.Text = "This function need Ms office";
                this.MPE_MessageBox.Show();
            }
        }
    }
}
