using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using WinBase;
using AjaxControlToolkit;
namespace WinEr
{
    public partial class WebForm16 : System.Web.UI.Page
    {
        private WinErSearch MySearchMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(406))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {                    
                    LoadAllBloodGroupsToDropDown();
                    LoadAllReligionToDropDown();
                    LoadAllCastToDropDown();
                    
                    Drp_Class.Items.Clear();
                    Drp_Class.Items.Add(new ListItem("No Class Found", "-1"));

                    Btn_Search.Enabled = false;
                    LoadBatchToDrpList();
                    Pnl_studentlist.Visible = false;
                    Drp_Batch.Focus();
                    //some initialisations
                    Txt_Name_AutoCompleteExtender.ContextKey = "0\\0\\-1\\" + MyUser.UserId.ToString();
                    Txt_AdNo_AutoCompleteExtender.ContextKey = "0\\0\\-1\\" + MyUser.UserId.ToString();
                }
            }

        }

        

        private void LoadClasstoDrpList()
        {
            string sql = "";
            Drp_Class.Items.Clear();
            if (int.Parse(Drp_Batch.SelectedValue.ToString()) != 0)
            {
                sql = "SELECT Distinct tblclass.ClassName,tblclass.Id FROM tblview_studentclassmap INNER JOIN tblclass ON tblview_studentclassmap.ClassId = tblclass.Id INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id WHERE tblview_studentclassmap.BatchId=" + int.Parse(Drp_Batch.SelectedValue.ToString()) + " order by tblstandard.Id,tblclass.ClassName";
                MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Drp_Class.Items.Add(new ListItem("Select any class", "-1"));
                    Drp_Class.Enabled = true;
                    Lbl_Message.Text = "";
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                        Drp_Class.Items.Add(li);
                    }
                }
                else
                {
                    Btn_Search.Enabled = false;
                    Drp_Class.Items.Add(new ListItem("No class found", "-1"));
                    Lbl_Message.Text = "No class is present";
                }
            }
            else
            {
                Lbl_Message.Text = "select any batch";               
            }            
        }

        private void LoadBatchToDrpList()
        {
            Drp_Batch.Items.Clear();
            Drp_AdvanceBatch.Items.Clear();
            string sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_Message.Text = "";
                Drp_Batch.Items.Add(new ListItem("Select any batch", "0"));
                Drp_AdvanceBatch.Items.Add(new ListItem("ALL", "0"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Batch.Items.Add(li);
                    Drp_AdvanceBatch.Items.Add(li);
                }

                LoadAllClassToDropDown();
            }
            else
            {
                Drp_Batch.Items.Add(new ListItem("No Batch Found", "-1"));
                Drp_AdvanceBatch.Items.Add(new ListItem("No Batch Found", "-1"));
                Lbl_Message.Text = "No batch is currently available"; 
            }
        }

        protected void Drp_batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Batch.SelectedValue.ToString()) != 0)
            {
                Lbl_Message.Text = "";
                LoadClasstoDrpList();
                Txt_Name_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
                Txt_AdNo_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
                LoadSearchButtonStatus();
            }            
        }

        private void LoadSearchButtonStatus()
        {
            if (int.Parse(Drp_Batch.SelectedValue) > 0 && int.Parse(Drp_Class.SelectedValue) > 0 && int.Parse(Drp_Status.SelectedValue) > -1)
            {
                Btn_Search.Enabled = true;
            }
            else
            {
                Btn_Search.Enabled = false;
            }

            if (int.Parse(Drp_AdvanceBatch.SelectedValue) > -1 && int.Parse(Drp_AdvancedClass.SelectedValue) > -1)
            {
                Btn_AdvancedSearch.Enabled = true;
            }
            else
            {
                Btn_AdvancedSearch.Enabled = false;
            }
        }

        protected void Drp_CLass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Class.SelectedValue.ToString()) != 0)
            {
                Txt_Name_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
                Txt_AdNo_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
            }
            LoadSearchButtonStatus();
        }

        protected void Drp_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Drp_Status.SelectedValue.ToString()) != -1)
            {
                Txt_Name_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
                Txt_AdNo_AutoCompleteExtender.ContextKey = Drp_Batch.SelectedValue + "\\" + Drp_Class.SelectedValue + "\\" + Drp_Status.SelectedValue + "\\" + MyUser.UserId.ToString();
            }
            LoadSearchButtonStatus();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i_SelectedStudId = int.Parse(Grd_Student.SelectedRow.Cells[1].Text.ToString());
         
            Session["StudId"] = i_SelectedStudId;
            Response.Redirect("StudentSearchDetails.aspx");       
        }

        protected void Grd_Student_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gray';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F7F7DE';");
                }
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Grd_Student, "Select$" + e.Row.RowIndex);
            }

        }

        private void FillGrid()
        {
            Grd_Student.DataSource = null;
            Grd_Student.DataBind();
            string sql="";
            int _Status = int.Parse(Drp_Status.SelectedValue);
            int ClassId = int.Parse(Drp_Class.SelectedValue);
            int Batchid = int.Parse(Drp_Batch.SelectedValue);
            if (_Status == 1)
            {
                sql = "SELECT tblview_student.Id, tblview_student.StudentName , tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + ClassId + "  and tblview_studentclassmap.BatchId = " + Batchid + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId where  tblview_student.`Status`=1 ";
                
            }
            else
            {
                sql = "SELECT tblview_student.Id, tblview_student.StudentName , tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id  and tblview_studentclassmap.ClassId = " + ClassId + "  and tblview_studentclassmap.BatchId = " + Batchid + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId  where  tblview_student.ClassId <> 0 and  tblview_student.BatchId <> 0";
            }

            if (Txt_Name.Text.Trim() != "")
            {
                sql = sql + " AND tblview_student.StudentName Like '" + Txt_Name.Text.Trim() + "%' ";
            }
            if (Txt_AdNo.Text.Trim() != "")
            {
                sql = sql + " AND tblview_student.AdmitionNo Like '" + Txt_AdNo.Text.Trim() + "%' ";
            }
            sql = sql + " AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " )";
            //sql = sql + " order by tblview_student.StudentName";
            //sai changed for roll no order
            sql = sql + " Order by tblview_studentclassmap.RollNo ASC";
            MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            System.Threading.Thread.Sleep(500);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["StudentList"] = MydataSet;
                Grd_Student.Columns[0].Visible = true;
                Grd_Student.DataSource = MydataSet;
                Grd_Student.DataBind();
                Grd_Student.Columns[0].Visible = false;
                FillStudDeatils();
                Pnl_studentlist.Visible = true;
            }
            else
            {
                Grd_Student.DataSource = null;
                Grd_Student.DataBind();
                WC_MessageBox.ShowMssage("No students found");
                Pnl_studentlist.Visible = false;
            }
        }

        private void FillStudDeatils()
        {
            foreach (GridViewRow gv in Grd_Student.Rows)
            {
                Image Img_stud = (Image)gv.FindControl("Img_studImage");

                Img_stud.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[1].Text.ToString()) + "&type=StudentImage";
                          
            }
        }
       
        protected void Grd_Student_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Student.PageIndex = e.NewPageIndex;
            //FillGrid();
            DataSet _MyDayaSet = (DataSet)ViewState["StudentList"];
            Grd_Student.Columns[0].Visible = true;
            Grd_Student.DataSource = _MyDayaSet;
            Grd_Student.DataBind();
            Grd_Student.Columns[0].Visible = false;
            FillStudDeatils();
            Pnl_studentlist.Visible = true;
        }

        protected void Btn_Search_Click1(object sender, EventArgs e)
        {
            Grd_Student.PageIndex = 0;
            FillGrid();
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("WinSchoolHome.aspx");
        }




        #region ADVANCED SEARCH FUNCTIONS

        protected void Btn_AdvancedSearch_Click(object sender, EventArgs e)
        {
            Grd_Student.PageIndex = 0;
            FillGridWithAdvancedSearchValues();
        }

        private void FillGridWithAdvancedSearchValues()
        {
            string sql = "";
            int _Status = int.Parse(Drp_AdvanceStatus.SelectedValue);
            int _ClassId=int.Parse(Drp_AdvancedClass.SelectedValue);
            int _BatchId=int.Parse(Drp_AdvanceBatch.SelectedValue);
            if (_Status > -1)
            {
                if (_Status == 1)
                {
                    sql = "SELECT distinct tblview_student.Id, tblview_student.StudentName , tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id";
                  
                if(_ClassId>0)
                {
                    sql=sql+" and tblview_studentclassmap.ClassId = " + _ClassId;
                }
                if(_BatchId>0)
                {
                    sql = sql + " and tblview_studentclassmap.BatchId = " + _BatchId;
                }
                
                 sql=sql  + " inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId where  tblview_student.`Status`=1 ";
                }
                else
                {
                    sql = "SELECT distinct tblview_student.Id, tblview_student.StudentName , tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id";
                    if (_ClassId > 0)
                    {
                        sql = sql + " and tblview_studentclassmap.ClassId = " + _ClassId;
                    }
                    if (_BatchId > 0)
                    {
                        sql = sql + " and tblview_studentclassmap.BatchId = " + _BatchId;
                    }
                    sql = sql + "  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId  where  tblview_student.ClassId <> 0 and  tblview_student.BatchId <> 0";
                }
            }
            else
            {
                sql = "SELECT distinct tblview_student.Id, tblview_student.StudentName , tblview_student.AdmitionNo  FROM tblview_student inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_student.Id";
                if (_ClassId > 0)
                {
                    sql = sql + " and tblview_studentclassmap.ClassId = " + _ClassId;
                }
                if (_BatchId > 0)
                {
                    sql = sql + " and tblview_studentclassmap.BatchId = " + _BatchId;
                }
                sql = sql + "  inner join tblclass on tblclass.Id=tblview_studentclassmap.ClassId inner join tblbatch on tblbatch.Id= tblview_studentclassmap.BatchId";
            }

            if (int.Parse(Drp_Gender.SelectedValue) > 0)
            {
                sql = sql + " and tblview_student.Sex='" + Drp_Gender.SelectedItem.Text + "'";
            }
            if (int.Parse(Drp_BloodGroup.SelectedValue) > 0)
            {
                sql = sql + " and tblview_student.BloodGroup=" + int.Parse(Drp_BloodGroup.SelectedValue);
            }
            if (int.Parse(Drp_Religion.SelectedValue) != -1)
            {
                sql = sql + " and tblview_student.Religion=" + int.Parse(Drp_Religion.SelectedValue);
            }
            if (int.Parse(Drp_Caste.SelectedValue) != -1)
            {
                sql = sql + " and tblview_student.Cast=" + int.Parse(Drp_Caste.SelectedValue);
            }
            if (int.Parse(Drp_StudentType.SelectedValue) > 0)
            {
                sql = sql + " and tblview_student.StudTypeId=" + int.Parse(Drp_StudentType.SelectedValue);
            }
            sql = sql + " AND tblview_student.`Status`<>2 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " )";
            sql = sql + " order by tblview_student.StudentName";
            MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            System.Threading.Thread.Sleep(500);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                ViewState["StudentList"] = MydataSet;
                Grd_Student.Columns[0].Visible = true;
                Grd_Student.DataSource = MydataSet;
                Grd_Student.DataBind();
                Grd_Student.Columns[0].Visible = false;
                FillStudDeatils();
                Pnl_studentlist.Visible = true;
            }
            else
            {
                Grd_Student.DataSource = null;
                Grd_Student.DataBind();
                WC_MessageBox.ShowMssage("No students found");
                Pnl_studentlist.Visible = false;
            }

            //sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblclass.ClassName, DATE_FORMAT(tblstudent.DOB,'%d/%m/%Y') as DOB, tblstudent.Sex, tblstudent.GardianName, tblstudent.Address, tblstudent.Pin, tblstudent.ResidencePhNo,tblstudent.OfficePhNo,  tblstudent.Email, tbllang1.Language as 1stLanguage, tbllang2.Language AS MotherTongue, tblreligion.Religion, tblbloodgrp.GroupName as BloodGroup, DATE_FORMAT(tblstudent.DateofJoining,'%d/%m/%Y') as DateofJoining, DATE_FORMAT(tblstudent.DateOfLeaving,'%d/%m/%Y') as DateOfLeaving  FROM tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id  inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId   inner join tbllanguage as tbllang1 on tbllang1.Id = tblstudent.1stLanguage inner join tbllanguage as tbllang2 on tbllang2.Id = tblstudent.MotherTongue inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tblreligion on tblreligion.Id = tblstudent.Religion  where tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudent.Status=1";

            //if (int.Parse(Drp_AdvancedClass.SelectedValue) > 0)
            //{
            //    sql = sql + " and tblstudentclassmap.ClassId=" + int.Parse(Drp_AdvancedClass.SelectedValue);
            //}
            

            //sql = sql + " and tblstudent.Id IN (SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap WHERE tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId IN ( SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (" + MyUser.MyGroupString + "))) order by tblstudentclassmap.ClassId asc , tblstudent.StudentName asc";


            //MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            //System.Threading.Thread.Sleep(500);
            //if (MydataSet.Tables[0].Rows.Count > 0)
            //{
            //    ViewState["StudentList"] = MydataSet;
            //    Grd_Student.Columns[1].Visible = true;

            //    DataTable dtAgent = MydataSet.Tables[0];
            //    DataView dataView = new DataView(dtAgent);
            //    if (Session["SortDirection"] != null && Session["SortExpression"] != null)
            //    {
            //        dataView.Sort = (string)Session["SortExpression"] + " " + (string)Session["SortDirection"];
            //    }
            //    ViewState["StudentList"] = MydataSet;



            //    Grd_Student.DataSource = MydataSet;
            //    Grd_Student.DataBind();



            //    Grd_Student.Columns[1].Visible = false;
            //    FillStudentImage();
            //    Pnl_Searchresult.Visible = true;

            //    img_export_Excel.Visible = true;
            //    Grd_Student.Focus();
            //}
            //else
            //{
            //    Grd_Student.DataSource = null;
            //    Grd_Student.DataBind();
            //    WC_MessageBox.ShowMssage("No Students Found");

            //   // Pnl_Searchresult.Visible = false;

            //   // img_export_Excel.Visible = false;
            //}
        }

        protected void Lnk_AdvancedSearch_Click(object sender, EventArgs e)
        {
            Drp_AdvancedClass.SelectedIndex = 0;
            Drp_Gender.SelectedIndex = 0;
            Drp_BloodGroup.SelectedIndex = 0;
            Drp_Religion.SelectedIndex = -1;
            Drp_Caste.SelectedIndex = -1;
            Drp_AdvanceStatus.SelectedIndex = 0;
            Drp_AdvanceBatch.SelectedIndex = 0;
            Drp_StudentType.SelectedIndex = 0;

            LoadAllClassToDropDown();         

            MPE_AdvancedSearch.Show();
        }

        private void LoadAllCastToDropDown()
        {
            Drp_Caste.Items.Clear();
            string sql = "select tblcast.Id, tblcast.castname from tblcast order by tblcast.castname";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Caste.Items.Add(new ListItem("ALL", "-1"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Caste.Items.Add(li);
                }
            }            
        }

        private void LoadAllReligionToDropDown()
        {
            Drp_Religion.Items.Clear();
            string sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Religion.Items.Add(new ListItem("ALL", "-1"));
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Religion.Items.Add(li);
                }
            }            
        }

        private void LoadAllBloodGroupsToDropDown()
        {
            Drp_BloodGroup.Items.Clear();

            string sql = "SELECT Id,GroupName FROM tblbloodgrp";
            MyReader = MySearchMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                ListItem li = new ListItem("ALL", "0");
                Drp_BloodGroup.Items.Add(li);
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_BloodGroup.Items.Add(li);
                }
            }
        }

        private void LoadAllClassToDropDown()
        {
            Drp_AdvancedClass.Items.Clear();
          
            string sql = "SELECT Distinct tblclass.ClassName,tblclass.Id FROM tblview_studentclassmap INNER JOIN tblclass ON tblview_studentclassmap.ClassId = tblclass.Id INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id ";
           
            if (int.Parse(Drp_AdvanceBatch.SelectedValue) != 0)
            {
                sql = sql + "WHERE tblview_studentclassmap.BatchId=" + int.Parse(Drp_AdvanceBatch.SelectedValue.ToString());
            }
            sql = sql + " order by tblstandard.Id,tblclass.ClassName";

            MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_AdvancedClass.Items.Add(new ListItem("ALL", "0"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[0].ToString(), dr[1].ToString());
                    Drp_AdvancedClass.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                Drp_AdvancedClass.Items.Add(li);
            }

            LoadSearchButtonStatus();
        }

        protected void Drp_AdvanceBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllClassToDropDown();
            MPE_AdvancedSearch.Show();
        }

        #endregion

        protected void Grd_Student_Sorting(object sender, GridViewSortEventArgs e)
        {
            Grd_Student.Columns[0].Visible = true;
            Grd_Student.PageIndex = 0;
            DataSet MydataSet = (DataSet)ViewState["StudentList"];

            DataTable dtCust = MydataSet.Tables[0];
            DataView dataView = new DataView(dtCust);

            dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            Grd_Student.DataSource = dataView;
            Grd_Student.DataBind();
            FillStudDeatils();
            Pnl_studentlist.Visible = true;
            Grd_Student.Columns[0].Visible = false;
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = Session["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = Session["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            Session["SortDirection"] = sortDirection;
            Session["SortExpression"] = column;

            return sortDirection;
        }
    
    }
}
