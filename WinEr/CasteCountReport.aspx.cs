using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class CasteCountReport : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
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
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(503))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Img_Excel.Visible = false;
                    Img_Excel.Enabled = false;
                    Drp_classes.Visible =false;
                    lbl_class.Visible = false;
                    Panel_CastArea.Visible = false;
                    LoadAllReligionToDropDown();
                    load_classes_to_dropdown_list();
                }
            }
        }

        private void LoadAllReligionToDropDown()
        {
            Drp_Religion.Items.Clear();
           //  string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            string sql = " select tblcast_category.Id , tblcast_category.CategoryName from tblcast_category ORDER BY tblcast_category.CategoryName ASC  ";
          
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Btn_Search.Enabled = true;
                Drp_Religion.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Religion.Items.Add(li);
                }
            }
            else
            {
                Drp_Religion.Items.Add(new ListItem("No Religion", "-1"));
                Btn_Search.Enabled = false;
            }
        }

        private void LoadAllStudentCountBasedOnCasteInAllClasses()
        {
            DataSet ClassDataSet = new DataSet();
            DataSet CasteDataSet = new DataSet();
            DataSet _StudentDataset = new DataSet();
            DataSet CountDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            CountDataSet.Tables.Add(new DataTable("CountList"));
            dt = CountDataSet.Tables["CountList"];
            dt.Columns.Add("CasteName");
            ClassDataSet = MyUser.MyAssociatedClass();

            if (ClassDataSet != null && ClassDataSet.Tables != null && ClassDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Class in ClassDataSet.Tables[0].Rows)
                {
                    dt.Columns.Add(Dr_Class[1].ToString());
                }                
            }
            dt.Columns.Add("TOTAL");

            //string sql1 = "SELECT Distinct(tblstudent.`Cast`), tblcast.castname from tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId= tblstudent.Id inner join tblcast on tblstudent.Cast= tblcast.Id where tblstudentclassmap.ClassId in(SELECT tblclass.Id from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblstudent.`Cast`>0 and tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY tblstandard.Id,tblclass.ClassName) and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId;
            //if (int.Parse(Drp_Religion.SelectedValue) > 0)
            //{
            //    sql1 = sql1 + " and tblstudent.Religion="+Drp_Religion.SelectedValue;
            //}
            //sql1 = sql1 + " order by tblstudent.Cast";
            string sql1 = "select DISTINCT tblcast.id, tblcast.castname from tblcast inner join tblstudent on tblstudent.Cast= tblcast.Id ";
            if (int.Parse(Drp_Religion.SelectedValue) > 0)
            {
                sql1 = sql1 + " where tblcast.CategoryId=" + Drp_Religion.SelectedValue;
            }
            sql1 = sql1 + " order by tblcast.castname asc";

            CasteDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql1);
            if (CasteDataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow Dr_Caste in CasteDataSet.Tables[0].Rows)
                {
                    int _ClassCount = ClassDataSet.Tables[0].Rows.Count, _ClassId = 0;
                    int _ClassRowCount = 0, _StudentRowCount = 0, _StudentTotal = 0;

                    int _TotalStudents = 0,_TotalInCaste=0;
                    dr = dt.NewRow();
                    dr["CasteName"] = Dr_Caste[1].ToString();

                    string sql = "SELECT tblstudent.Id, tblstudentclassmap.ClassId from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.`Cast`=" + Dr_Caste[0].ToString() + " and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 order by tblstandard.Id, tblclass.ClassName";
                    _StudentDataset = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDataset.Tables[0].Rows.Count > 0)
                    {
                        _StudentTotal = _StudentDataset.Tables[0].Rows.Count;

                        while (_ClassCount > _ClassRowCount)
                        {                            
                            if (_StudentTotal > _StudentRowCount)
                            {
                                _ClassId = int.Parse(ClassDataSet.Tables[0].Rows[_ClassRowCount][0].ToString());

                                if (_ClassId == int.Parse(_StudentDataset.Tables[0].Rows[_StudentRowCount][1].ToString()))
                                {
                                    _TotalStudents++;
                                    _TotalInCaste++;
                                    _StudentRowCount++;
                                }
                                else
                                {
                                    dr[ClassDataSet.Tables[0].Rows[_ClassRowCount][1].ToString()] = _TotalStudents.ToString();
                                    _ClassRowCount++;
                                    _TotalStudents = 0;
                                }
                            }
                            else
                            {
                                dr[ClassDataSet.Tables[0].Rows[_ClassRowCount][1].ToString()] = _TotalStudents.ToString();
                                _ClassRowCount++;
                                _TotalStudents = 0;
                            }
                        }
                    }
                    dr["TOTAL"] = _TotalInCaste.ToString();
                    dt.Rows.Add(dr);
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
                Lbl_msg.Text = "No Student in this Caste";
                this.MPE_MessageBox.Show();
                Panel_CastArea.Visible = false;
            }
        }

        protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
        {
            if (int.Parse(Session["Report_type"].ToString()) == 0)
            {
                DataSet _CountReportList = new DataSet();
                _CountReportList = (DataSet)ViewState["CountList"];

                string FileName = "CasteCountReport";
                string _ReportName = "<table><tr><td colspan=\"7\" style=\"text-align:center;\"><b>Caste Count Report</b></td></tr><tr><td>Created Date:" + DateTime.Now.ToString() + "</td><td></td></tr></table>";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_msg.Text = "This function need Ms office";
                    this.MPE_MessageBox.Show();
                }
            }
            else
            {
                string cls_Name = Session["cls_name"].ToString();
                string total_count = Session["count"].ToString();
                DataSet _CountReportList = new DataSet();
                _CountReportList = (DataSet)ViewState["CountList"];

                string FileName = "Category Report";
                string _ReportName = "<table><tr><td colspan=\"7\" style=\"text-align:center;\"><b>Category Report</b></td></tr><tr><td colspan=\"7\" style=\"text-align:center;\"><b>" + cls_Name + "</b></td></tr><tr><td colspan=\"7\" style=\"text-align:center;\"><b>Total Student:" + total_count + "</b></td></tr></table>";
                if (!WinEr.ExcelUtility.ExportDataToExcel(_CountReportList, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    Lbl_msg.Text = "This function need Ms office";
                    this.MPE_MessageBox.Show();
                }
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (chkb_cast.Checked)
            {
                Get_Category_Report();
                Session["Report_type"] = 1;
            }
            else
            {
                LoadAllStudentCountBasedOnCasteInAllClasses();
                Session["Report_type"] = 0;
            }
        }

        //sai added for class wise cast category report

        protected void chkb_cast_CheckedChanged(object sender, EventArgs e)
        {
            if (chkb_cast.Checked)
            {
                Drp_classes.Visible = true;
                Drp_Religion.Enabled = false;
                lbl_class.Visible = true;
               
            }
            else
            {
                Drp_classes.Visible =false;
                Drp_Religion.Enabled =true;
                lbl_class.Visible = false;

            }
        }
        private void load_classes_to_dropdown_list()
        {
            Drp_classes.Items.Clear();
            DataSet ds_classes = new DataSet();
            ds_classes = MyUser.MyAssociatedClass();
            if (ds_classes != null && ds_classes.Tables[0] != null && ds_classes.Tables[0].Rows.Count > 0)
            {
                Drp_classes.DataTextField = ds_classes.Tables[0].Columns["ClassName"].ToString();
                Drp_classes.DataValueField = ds_classes.Tables[0].Columns["Id"].ToString();
                Drp_classes.DataSource = ds_classes.Tables[0];
                Drp_classes.DataBind();
            }
            else
            {
                Drp_classes.Items.Add(new ListItem("No class exist","-1"));
            }
        }
        private void Get_Category_Report()
        {
            //DataSet ClassDataSet = new DataSet();
            DataSet CasteDataSet = new DataSet();
            DataSet _StudentDataset = new DataSet();
            DataSet CountDataSet = new DataSet();
            DataTable dt;
            DataRow dr;

            CountDataSet.Tables.Add(new DataTable("CountList"));
            dt = CountDataSet.Tables["CountList"];
            dt.Columns.Add("Category_Name");
            dt.Columns.Add("Boys");
            dt.Columns.Add("Girls");
            string sql1 = "select DISTINCT tblcast_category.id, tblcast_category.CategoryName from tblcast_category ";
            sql1 = sql1 + " order by tblcast_category.CategoryName asc";

            CasteDataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql1);
            if (CasteDataSet.Tables[0].Rows.Count > 0)
            {
                int male_total = 0, female_total = 0, Main_Total = 0;

                foreach (DataRow Dr_Caste in CasteDataSet.Tables[0].Rows)
                {
                    int male_count = 0, female_count = 0, total = 0;
                    dr = dt.NewRow();
                    dr["Category_Name"] = Dr_Caste[1].ToString();

                    string sql = "SELECT tblstudent.Id from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.Cast in(SELECT DISTINCT tblcast.Id from tblcast where tblcast.CategoryId =" + int.Parse(Dr_Caste[0].ToString()) + ") and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 and tblstudentclassmap.ClassId =" + int.Parse(Drp_classes.SelectedValue) + " and tblstudent.Sex='Male' order by tblstandard.Id, tblclass.ClassName";
                    _StudentDataset = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDataset.Tables[0].Rows.Count > 0)
                    {
                        male_count = _StudentDataset.Tables[0].Rows.Count;
                    }
                    sql = "SELECT tblstudent.Id from tblstudent inner join tblstudentclassmap on tblstudent.Id= tblstudentclassmap.StudentId inner join tblclass ON tblstudentclassmap.ClassId =tblclass.Id inner join tblstandard on tblclass.Standard= tblstandard.Id where tblstudent.Cast in(SELECT DISTINCT tblcast.Id from tblcast where tblcast.CategoryId =" + int.Parse(Dr_Caste[0].ToString()) + ") and tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " and tblstudent.Status=1 and tblstudentclassmap.ClassId =" + int.Parse(Drp_classes.SelectedValue) + " and tblstudent.Sex='Female' order by tblstandard.Id, tblclass.ClassName";
                    _StudentDataset = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_StudentDataset.Tables[0].Rows.Count > 0)
                    {
                        female_count = _StudentDataset.Tables[0].Rows.Count;
                    }
                    total = male_count + female_count;
                    male_total = male_total + male_count;
                    female_total = female_total + female_count;
                    dr["Boys"] = male_count.ToString();
                    dr["Girls"] = female_count.ToString();
                    dt.Rows.Add(dr);
                }
                dr = dt.NewRow();
                dr["Category_Name"] = "Total";
                dr["Boys"] = male_total.ToString();
                dr["Girls"] = female_total.ToString();
                dt.Rows.Add(dr);
               

                // CountDataSet.Tables.Add(dt);
                if (CountDataSet.Tables[0].Rows.Count > 0)
                {
                    Session["cls_name"] = Drp_classes.SelectedItem.Text.ToString();
                    Session["count"] = male_total + female_total;
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
                    Lbl_msg.Text = "No Students exist";
                    this.MPE_MessageBox.Show();
                    Panel_CastArea.Visible = false;
                }
            }

        }
    }
}
