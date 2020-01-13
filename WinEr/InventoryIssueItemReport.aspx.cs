using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using WinBase;
using System.Data;

namespace WinEr
{
    public partial class InventoryIssueItemReport : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private WinBase.Inventory Myinventory;
        private OdbcDataReader MyReader = null;
        private FeeManage MyFeeMang;

        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyConfigMang = MyUser.GetConfigObj();
                Myinventory = MyUser.GetInventoryObj();
                MyFeeMang = MyUser.GetFeeObj();
                if (MyConfigMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                }
                else if (!MyUser.HaveActionRignt(871))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {

                    if (!IsPostBack)
                    {
                        img_export_Excel.Visible = false;
                        Img_StaffExcel.Visible = false;
                        LoadPeriod();
                        LoadItemToDropDown();
                        LoadClassToDropDown();
                        LoadStudentToDropDown();
                        LoadStaffToDropDown();
                        LoadArea();
                    }
                }
            }

       

            protected void Btn_show_Click(object sender, EventArgs e)
            {
                GenareteReport();
            }

   
            protected void Grd_StudentIssuereport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {

                Grd_StudentIssuereport.PageIndex = e.NewPageIndex;
                GenareteReport();
            }

            protected void Drp_Period_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadPeriod();
            }

            protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadStudentToDropDown();
            }

            protected void Rbd_SelectType_SelectedIndexChanged(object sender, EventArgs e)
            {
                LoadArea();
                Lbl_Err.Text = "";
            }

            protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
            {
                DataSet MyData = (DataSet)ViewState["studentItemIssueReport"];
                MyData.Tables[0].Columns.Remove("BookId");
                MyData.Tables[0].Columns.Remove("StudId");
                MyData.Tables[0].Columns.Remove("ClassId");
                string Name = "StudentItemIssueReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "StudentItemIssueReport";
                string _ReportName = "StudentItemIssueReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }

            protected void Img_StaffExcel_Click(object sender, ImageClickEventArgs e)
            {
                // ViewState["staffItemIssueReport"] 

                DataSet MyData = (DataSet)ViewState["staffItemIssueReport"];
                MyData.Tables[0].Columns.Remove("Id");
                MyData.Tables[0].Columns.Remove("ItemId");
                MyData.Tables[0].Columns.Remove("VendorId");
                string Name = "staffItemIssueReport.xls";
                //if (!ExcelUtility.ExportDataSetToExcel(MyData, Name))
                //{

                //}
                string FileName = "staffItemIssueReport";
                string _ReportName = "staffItemIssueReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MyData, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }

            protected void Grd_StaffIssueReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grd_StaffIssueReport.PageIndex = e.NewPageIndex;
                GenareteReport();
            }


        #endregion

          
       
        #region Methods

            private void LoadItemToDropDown()
            {
                DataSet ItemDs = new DataSet();
                Drp_Item.Items.Clear();
                ListItem li = new ListItem();
                ItemDs = Myinventory.GetAllItems(0, 0);
                if (ItemDs != null && ItemDs.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Item.Items.Add(li);
                    foreach (DataRow dr in ItemDs.Tables[0].Rows)
                    {
                        //Id, tblinv_item.ItemName
                        li = new ListItem(dr["ItemName"].ToString(), dr["Id"].ToString());
                        Drp_Item.Items.Add(li);
                    }

                }
                else
                {
                    li = new ListItem("None", "-1");
                    Drp_Item.Items.Add(li);
                }
            }

            private void GenareteReport()
            {
                Lbl_Err.Text = "";
                int ItemId = int.Parse(Drp_Item.SelectedValue);
                int RbdType = int.Parse(Rbd_SelectType.SelectedValue);
                DateTime Startdate = General.GetDateTimeFromText(Txt_FromDate.Text);
                DateTime Enddate = General.GetDateTimeFromText(Txt_ToDate.Text);
                if (RbdType == 0)
                {

                    int ClassId = int.Parse(Drp_Class.SelectedValue);
                    int StudentID = int.Parse(Drp_Student.SelectedValue);
                    Pnl_DiaplayStaff.Visible = false;
                    DataSet StudentDs = new DataSet();
                    if (ClassId != -1 && StudentID != -1 && ItemId != -1)
                    {
                        StudentDs = Myinventory.GetStudentIssueReport(Startdate, Enddate, ClassId, StudentID, ItemId);
                        if (StudentDs != null && StudentDs.Tables[0].Rows.Count > 0)
                        {
                            double total = 0;
                            foreach (DataRow dr in StudentDs.Tables[0].Rows)
                            {
                                total = total + double.Parse(dr["Cost"].ToString());
                            }
                            Lbl_TotalCost.Text = "Total Amount Collected: " + total + "";
                            Grd_StudentIssuereport.Columns[0].Visible = true;
                            Grd_StudentIssuereport.Columns[1].Visible = true;
                            Grd_StudentIssuereport.Columns[2].Visible = true;
                            Pnl_DisplayStudent.Visible = true;
                            img_export_Excel.Visible = true;
                            Grd_StudentIssuereport.DataSource = StudentDs;
                            Grd_StudentIssuereport.DataBind();
                            ViewState["studentItemIssueReport"] = StudentDs;
                            Grd_StudentIssuereport.Columns[0].Visible = false;
                            Grd_StudentIssuereport.Columns[1].Visible = false;
                            Grd_StudentIssuereport.Columns[2].Visible = false;
                        }
                        else
                        {
                            Lbl_TotalCost.Text = "";
                            Lbl_Err.Text = "No report found";
                            Pnl_DisplayStudent.Visible = false;
                            img_export_Excel.Visible = false;
                            Grd_StudentIssuereport.DataSource = null;
                            Grd_StudentIssuereport.DataBind();
                        }
                    }
                }
                else
                {
                    Pnl_DisplayStudent.Visible = false;
                    int staffId = int.Parse(Drp_StaffName.SelectedValue);
                    DataSet StaffDs = new DataSet();
                    if (staffId != -1 && ItemId != -1)
                    {
                        StaffDs = Myinventory.GetStaffIssueReport(staffId, Startdate, Enddate, ItemId);
                        if (StaffDs != null && StaffDs.Tables[0].Rows.Count > 0)
                        {

                            Grd_StaffIssueReport.Columns[0].Visible = true;
                            Grd_StaffIssueReport.Columns[1].Visible = true;
                            Pnl_DiaplayStaff.Visible = true;
                            Img_StaffExcel.Visible = true;
                            Grd_StaffIssueReport.DataSource = StaffDs;
                            Grd_StaffIssueReport.DataBind();
                            ViewState["staffItemIssueReport"] = StaffDs;
                            Grd_StaffIssueReport.Columns[0].Visible = false;
                            Grd_StaffIssueReport.Columns[1].Visible = false;
                        }
                        else
                        {
                            Lbl_Err.Text = "No report found";
                            Pnl_DiaplayStaff.Visible = false;
                            Img_StaffExcel.Visible = false;
                            Grd_StudentIssuereport.DataSource = null;
                            Grd_StudentIssuereport.DataBind();
                        }
                    }


                }
            }

            private void LoadArea()
            {
                int RbdType = int.Parse(Rbd_SelectType.SelectedValue);
                if (RbdType == 0)
                {
                    RowStudent.Visible = true;
                    Drp_StaffName.Visible = false;
                    Lbl_Staff.Visible = false;
                }
                else
                {
                    RowStudent.Visible = false;
                    Drp_StaffName.Visible = true;
                    Lbl_Staff.Visible = true;
                }
            }

            private void LoadStaffToDropDown()
            {

                Drp_StaffName.Items.Clear();
            Drp_StaffName.Items.Add(new ListItem("All", "0"));
            string sql = "SELECT DISTINCT t.`Id`,t.`SurName` FROM tbluser t inner join tblrole r on r.`Id`=t.`RoleId` inner join tblgroupusermap g on t.`Id`=g.`UserId` where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ") ORDER BY t.`SurName`";
            MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_StaffName.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No staff found", "-1");
                Drp_StaffName.Items.Add(li);
            }  
            }

            private void LoadStudentToDropDown()
            {
                Drp_Student.Items.Clear();
                ListItem li = new ListItem();
                int classId = int.Parse(Drp_Class.SelectedValue);
                if (classId != -1)
                {
                    string sql = " SELECT map.StudentId,stud.StudentName FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1  and map.RollNo<>-1 and map.BatchId=" + MyUser.CurrentBatchId + " ";
                    if (classId > 0)
                    {
                        sql = sql + " and  map.ClassId=" + int.Parse(Drp_Class.SelectedValue.ToString()) + "";
                    }
                    sql = sql + " order by map.RollNo";
                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        li = new ListItem("All", "0");
                        Drp_Student.Items.Add(li);
                        while (MyReader.Read())
                        {
                            li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            Drp_Student.Items.Add(li);
                        }

                    }
                    else
                    {
                        li = new ListItem("No students found", "-1");
                        Drp_Student.Items.Add(li);
                    }

                    MyReader.Close();
                }
            }

            private void LoadClassToDropDown()
            {
                DataSet Class_Ds = new DataSet();
                ListItem li;
                Drp_Class.Items.Clear();
                Class_Ds = MyUser.MyAssociatedClass();
                if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count > 0)
                {
                    li = new ListItem("All", "0");
                    Drp_Class.Items.Add(li);
                    foreach (DataRow dr in Class_Ds.Tables[0].Rows)
                    {
                        li = new ListItem(dr[1].ToString(), dr[0].ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Class Found", "-1");
                    Drp_Class.Items.Add(li);
                }
            }

            private void LoadPeriod()
            {
                string _sdate = null, _edate = null;
                if (MyUser.HaveActionRignt(895))
                {
                    if (int.Parse(Drp_Period.SelectedValue) == 0)
                    {
                        DateTime _date = System.DateTime.Now;
                        //DateTime _Endtime = _date.AddDays(-1);
                        _edate = MyUser.GerFormatedDatVal(_date);

                        DateTime _start = System.DateTime.Now;
                        _sdate = MyUser.GerFormatedDatVal(new DateTime(_start.Year, _start.Month, 1));
                        Txt_FromDate.Enabled = false;
                        Txt_ToDate.Enabled = false;
                        Txt_FromDate.Text = _sdate;
                        Txt_ToDate.Text = _edate;
                    }
                    else if (int.Parse(Drp_Period.SelectedValue) == 1)
                    {

                        Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date.AddDays(-7));
                        Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                        Txt_FromDate.Enabled = false;
                        Txt_ToDate.Enabled = false;

                    }
                    else if (int.Parse(Drp_Period.SelectedValue) == 2)
                    {
                        Txt_FromDate.Enabled = true;
                        Txt_ToDate.Enabled = true;
                        Txt_FromDate.Text = "";
                        Txt_ToDate.Text = "";

                    }
                }
                else   // Only todays view rights
                {
                    Drp_Period.SelectedValue = "2";
                    Drp_Period.Enabled = false;
                    Txt_FromDate.Text = General.GerFormatedDatVal(System.DateTime.Now.Date);
                    Txt_ToDate.Text = General.GerFormatedDatVal(System.DateTime.Now);
                    Txt_FromDate.Enabled = false;
                    Txt_ToDate.Enabled = false;
                   

                }
            }


        #endregion

          
           

          

          
        

            
   
       
    }
}
