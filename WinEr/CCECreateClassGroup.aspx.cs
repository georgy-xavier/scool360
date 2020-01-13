using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data;
using System.IO;
using System.Text;

namespace WinEr
{
    public partial class CCECreateClassGroup1 : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;
      
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
                //no rights for this user.
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
                    LoadDefaultValues();
                    Txt_groupname.Focus();
                   
                }
            }

        }

        /// <summary>
        /// this function collection all page controls values
        /// </summary>
        
        private void LoadDefaultValues()
        {
            Txt_groupname.Text = "";
            LoadConvertionType();
            LoadGradeMaster();
            string _Errmsg = "";
            if (!load_Grid(out _Errmsg))
            {
                WC_MessageBox.ShowMssage(_Errmsg);
            }
        }

        private bool load_Grid(out string _Errmsg)
        {
            _Errmsg = "";
            bool valid = true;
            try
            {
                Grd_CCE.DataSource = null;
                string sql = "SELECT tblcce_classgroup.Id as GroupId,tblcce_classgroup.GroupName as GroupName,tblcce_convertionmaster.Id as ConvertionTypeId,tblcce_convertionmaster.Name as ConvertionType,tblgrademaster.Id as GradeMasterId,tblgrademaster.GradeName as GradeMaster,CASE WHEN  TermXMLFile!='' THEN 'Avaiable' ELSE 'Not Avaiable' END AS TermXMLFile,CASE WHEN ConsoldateXMLFile!='' THEN 'Avaiable' ELSE 'Not Avaiable' END AS ConsoldateXMLFile FROM tblcce_classgroup inner join tblcce_convertionmaster on tblcce_convertionmaster.Id=tblcce_classgroup.ConvertionType inner JOIN tblgrademaster on tblgrademaster.Id=tblcce_classgroup.GradeMaster order by tblcce_classgroup.Id asc";
                MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_CCE.Visible = true;
                    Grd_CCE.Columns[0].Visible = true;
                    Grd_CCE.Columns[2].Visible = true;
                    Grd_CCE.Columns[4].Visible = true;
                    Grd_CCE.DataSource = MydataSet;
                    Grd_CCE.DataBind();
                    Grd_CCE.Columns[0].Visible = false;
                    Grd_CCE.Columns[2].Visible = false;
                    Grd_CCE.Columns[4].Visible = false;
                    divgrid.Visible = true;
                    Label1.Visible = false;
                }
                else
                {
                    divgrid.Visible = false;
                    Label1.Visible = true;
                    Label1.Text = "Group not found!.Create Group.";
                }

            }
            catch (Exception ex)
            {
                valid = false;
                _Errmsg = ex.Message;
            }
            return valid;
        }

        private void LoadConvertionType()
        {
            
                Drp_conversion.Items.Clear();
                string sql = "SELECT tblcce_convertionmaster.Id,tblcce_convertionmaster.Name from tblcce_convertionmaster";
                DataSet Ds_ConvertionType = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                ListItem li;
                if (Ds_ConvertionType != null && Ds_ConvertionType.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow DrConType in Ds_ConvertionType.Tables[0].Rows)
                    {
                        li = new ListItem(DrConType["Name"].ToString(), DrConType["Id"].ToString());
                        Drp_conversion.Items.Add(li);
                    }
                }
                else
                {
                    li = new ListItem("No Data Found", "-1");
                    Drp_conversion.Items.Add(li);
                }

            
        }

        private void LoadGradeMaster()
        {
            Drp_grademaster.Items.Clear();
            string sql = "SELECT tblgrademaster.Id,tblgrademaster.GradeName from tblgrademaster ORDER BY tblgrademaster.GradeName";
            DataSet GradeMaster = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (GradeMaster != null && GradeMaster.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow DrGradeMaster in GradeMaster.Tables[0].Rows)
                {
                    li = new ListItem(DrGradeMaster["GradeName"].ToString(), DrGradeMaster["Id"].ToString());
                    Drp_grademaster.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("No Data Found", "-1");
                Drp_grademaster.Items.Add(li);
            }
        }

        /// <summary>
        /// create group name update from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Errmsg = "";
            try
            {
                string Groupname = "";
                int conversiontype = -1, GradeMaster = -1;
                Groupname = Txt_groupname.Text.Trim();
                int.TryParse(Drp_conversion.SelectedItem.Value, out conversiontype);
                int.TryParse(Drp_grademaster.SelectedItem.Value, out GradeMaster);
                if (Groupname == "")
                    _Errmsg = "Enter the group name.";
                else if (conversiontype == -1)
                    _Errmsg = "Conversion type is not found!";
                else if (GradeMaster == -1)
                    _Errmsg = "Grad Master is not found!";
                else
                {
                    //Edited By Mobin
                    bool IsSucess = false;
                    _Errmsg = Config_CCE_Group(Groupname, conversiontype, GradeMaster, out IsSucess);
                    if (IsSucess)
                    {
                        logger.LogToFile("Group Configuration", Groupname +" created", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", Groupname + " created", 1);
                        LoadDefaultValues();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                _Errmsg = ex.Message;
                logger.LogToFile("Group Configuration", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", "throws Error" + ex.Message, 1);

            }

            WC_MessageBox.ShowMssage(_Errmsg);
            Txt_groupname.Focus();
        }

        public string Config_CCE_Group(string Groupname, int conversiontype, int GradeMaster, out bool IsSucess)
        {
            string _RtnMsg = "";
            string sql = "SELECT tblcce_classgroup.GroupName from tblcce_classgroup where tblcce_classgroup.GroupName='" + Groupname + "'";
            DataSet _MyDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_MyDs.Tables[0].Rows.Count == 0)
            {
                sql = "insert into tblcce_classgroup(GroupName,ConvertionType,GradeMaster) VALUES ('" + Groupname + "'," + conversiontype + "," + GradeMaster + ")";
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                IsSucess = true;
                _RtnMsg = Groupname + " is created sucessfully";
            }
            else
            {
                IsSucess = false;
                _RtnMsg = "Existing group name!";
            }
            return _RtnMsg;
        }

        #region edit popup
        /// <summary>
        /// after create group edit group items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_courseRowCommand(object sender, GridViewCommandEventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Errmsg = "";
            try
            {
                int _Groupid = 0, _ConversionId = 0, _GradeMasterId = 0;
                int Index = Convert.ToInt32(e.CommandArgument);
                _Groupid = int.Parse(Grd_CCE.Rows[Index].Cells[0].Text);
                _ConversionId = int.Parse(Grd_CCE.Rows[Index].Cells[2].Text);
                _GradeMasterId = int.Parse(Grd_CCE.Rows[Index].Cells[4].Text);
                Lbl_Err.Text = _Groupid.ToString();

                if (e.CommandName == "ConfigEdit")
                {
                    Txt_editgroupname.Text = Grd_CCE.Rows[Index].Cells[1].Text;
                    ViewState["groupid"] = Grd_CCE.Rows[Index].Cells[0].Text;
                    LoadEditModulePoup(_ConversionId, _GradeMasterId);
                    MPE_MessageBox.Show();
                }
                else
                {
                    _Groupid = int.Parse(Grd_CCE.Rows[Index].Cells[0].Text);
                    RemoveSelectedIndexGroupname(_Groupid, out _Errmsg);
                    logger.LogToFile("Group Configuration", Grd_CCE.Rows[Index].Cells[0].Text + " removed", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", Grd_CCE.Rows[Index].Cells[0].Text + " removed", 1);

                }
            }
            catch (Exception ex)
            {
                _Errmsg = ex.Message;
                logger.LogToFile("Group configuration", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", "throws Error" + ex.Message, 1);
            }
            if (_Errmsg != "")
                WC_MessageBox.ShowMssage(_Errmsg);
            
        }

        private void RemoveSelectedIndexGroupname(int _Groupid,out string _Errmsg)
        {
            _Errmsg = "";
            string sql = "";
            sql = "select tblcce_classgroupmap.ClassId from tblcce_classgroupmap where tblcce_classgroupmap.GroupId=" + _Groupid;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lbl_groupid.Text = _Groupid.ToString();
                MPE_yesornoMessageBox.Show();
                Btn_no.Focus();
            }
            else
            {
                sql = "DELETE from tblcce_colconfig where tblcce_colconfig.GroupId=" + _Groupid;
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                sql = "delete from tblcce_classgroupmap where tblcce_classgroupmap.GroupId=" + _Groupid;
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                sql = "delete from tblcce_classgroup where tblcce_classgroup.Id=" + _Groupid;
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                _Errmsg = "Selected group is deleted sucessfully!";
                LoadDefaultValues();
            }
        }
        /// <summary>
        /// after editing update group details from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Msg = "";
            int _Conversionid = -1, _Grademasterid = 0;
            try
            {
                int.TryParse(Drp_editconverisontype.SelectedItem.Value, out _Conversionid);
                int.TryParse(Drp_editgrademaster.SelectedItem.Value, out _Grademasterid);
                string sql = "", _XMLfile1 = "", _XMLfile2="";
                FileUpload f1 = FileUpload1;
                FileUpload f2 = FileUpload2;
                StreamReader reader1=null;
                StreamReader reader2=null;
                if (f1.HasFile)
                {
                    reader1 = new StreamReader(f1.FileContent);
                    _XMLfile1 = reader1.ReadToEnd();
                }
                if (f2.HasFile)
                {
                    reader2 = new StreamReader(f2.FileContent);
                    _XMLfile2= reader2.ReadToEnd();
                }

                if (Txt_editgroupname.Text == "")
                    _Msg = "Enter the Group name!";
                else if (_Conversionid <0)
                    _Msg = "Conversion type is not found!.Create conversion type.";
                else if (_Grademasterid == -1)
                    _Msg = "Grade Master is not found!.Create grade master name.";
                else if (CheckBox1.Checked == true && _XMLfile1 == "")
                    _Msg = "Please select termwise XML file!";
                else if (CheckBox2.Checked == true && _XMLfile2 == "")
                    _Msg = "Please select consolidate XML file!";
                else
                {
                    sql = "SELECT tblcce_classgroup.GroupName from tblcce_classgroup where tblcce_classgroup.GroupName='" + Txt_editgroupname.Text.Trim() + "' and tblcce_classgroup.Id!=" + int.Parse(ViewState["groupid"].ToString());
                    DataSet _MyDs = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (_MyDs.Tables[0].Rows.Count > 0)
                        _Msg = "This group name is already created!";
                    else
                    {
                        sql = "UPDATE tblcce_classgroup SET tblcce_classgroup.GroupName='" + Txt_editgroupname.Text + "',tblcce_classgroup.ConvertionType=" + _Conversionid + ",tblcce_classgroup.GradeMaster=" + _Grademasterid + "";
                        if (CheckBox1.Checked == true)
                            sql += ",tblcce_classgroup.TermXMLFile='" + _XMLfile1 + "'";
                        if (CheckBox2.Checked == true)
                            sql += ",tblcce_classgroup.ConsoldateXMLFile='" + _XMLfile2 + "'";
                        sql += " WHERE tblcce_classgroup.Id=" + int.Parse(ViewState["groupid"].ToString());
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        LoadDefaultValues();
                        _Msg = "Group Configuration updated sucessfully!";
                        logger.LogToFile("Group Configuration", Txt_groupname.Text + " updated to " + Txt_editgroupname.Text, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                        MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", Txt_groupname.Text + " updated to " + Txt_editgroupname.Text, 1);
                        Txt_groupname.Text = "";
                    }
                   
                }
            }
            catch (Exception ex)
            {
                _Msg = ex.Message;
                logger.LogToFile("Group configuration", "throws Error" + ex.Message, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Group Configuration", "throws Error" + ex.Message, 1);

            }
            LoadEditModulePoup(_Conversionid, _Grademasterid);
            WC_MessageBox.ShowMssage(_Msg);
        }

        /// <summary>
        /// this function collecting all editmodule popup control values
        /// </summary>
        /// <param name="_ConversionId"></param>
        /// <param name="_GradeMasterId"></param>
        private void LoadEditModulePoup(int _ConversionId, int _GradeMasterId)
        {
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            FileUpload1.Enabled = false;
            FileUpload2.Enabled = false;
            Load_EditDrpconversionType(_ConversionId);
            Load_EditGradeMaster(_GradeMasterId);
        }

        private void Load_EditGradeMaster(int _GradeMasterId)
        {
            Drp_editgrademaster.Items.Clear();
            string sql = "SELECT tblgrademaster.Id,tblgrademaster.GradeName from tblgrademaster ORDER BY tblgrademaster.GradeName";
            DataSet GradeMaster = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (GradeMaster != null && GradeMaster.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow DrGradeMaster in GradeMaster.Tables[0].Rows)
                {
                    li = new ListItem(DrGradeMaster["GradeName"].ToString(), DrGradeMaster["Id"].ToString());
                    Drp_editgrademaster.Items.Add(li);
                }
                Drp_editgrademaster.Items.FindByValue(_GradeMasterId.ToString()).Selected = true;
            }
            else
            {
                li = new ListItem("No Data Found", "-1");
                Drp_editgrademaster.Items.Add(li);
            }
        }

        private void Load_EditDrpconversionType(int _ConversionId)
        {
            Drp_editconverisontype.Items.Clear();
            string sql = "SELECT tblcce_convertionmaster.Id,tblcce_convertionmaster.Name from tblcce_convertionmaster ORDER BY tblcce_convertionmaster.Name";
            DataSet Ds_ConvertionType = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_ConvertionType != null && Ds_ConvertionType.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow DrConType in Ds_ConvertionType.Tables[0].Rows)
                {
                    li = new ListItem(DrConType["Name"].ToString(), DrConType["Id"].ToString());
                    Drp_editconverisontype.Items.Add(li);
                }
                Drp_editconverisontype.Items.FindByValue(_ConversionId.ToString()).Selected = true;
            }
            else
            {
                li = new ListItem("No Data Found", "-1");
                Drp_editconverisontype.Items.Add(li);
            }

        }

        protected void CheckBox1_CheckedChanged(Object sender, EventArgs args)
        {
            if (CheckBox1.Checked)
            {
                MPE_MessageBox.Show();
                
                FileUpload1.Enabled = true;
                
            }
            else
            {
                MPE_MessageBox.Show();
                Session["TermFilepath"] = "";
                FileUpload1.Enabled = false;
              
            }
        }
       
        protected void CheckBox2_CheckedChanged(Object sender, EventArgs args)
        {
            if (CheckBox2.Checked)
            {
                Session["TermFilepath"] =Path.GetFileName(FileUpload1.FileName);
                MPE_MessageBox.Show();
                FileUpload2.Enabled = true;
                FileUpload1.SaveAs(Session["TermFilepath"].ToString());
            }
            else
            {
                MPE_MessageBox.Show();
                Session["TermFilepath"] = "";
                FileUpload2.Enabled = false;
               
            }
        }
        
        #endregion 

        #region yesorno popup

        protected void Btn_yes_Click(object sender, EventArgs e)
        {
            string _Errmsg = "";
            int _Groupid = 0;
            if (Lbl_groupid.Text != "")
                int.TryParse(Lbl_groupid.Text.ToString(), out _Groupid);
            string sql = "";
            sql = "select tblcce_classgroupmap.ClassId from tblcce_classgroupmap where tblcce_classgroupmap.GroupId=" + _Groupid;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int _Classid = 0;
                foreach (DataRow _drclassid in ds.Tables[0].Rows)
                {
                    int.TryParse(_drclassid[0].ToString(), out _Classid);
                    sql = "SELECT tblstudent.Id from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _Classid;
                    DataSet StuId_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (StuId_Ds.Tables[0].Rows.Count > 0 && StuId_Ds != null)
                    {
                        int _StuId = 0;
                        foreach (DataRow dr in StuId_Ds.Tables[0].Rows)
                        {
                            int.TryParse(dr[0].ToString(), out _StuId);
                            sql = "DELETE from tblcce_descriptive where tblcce_descriptive.StudentId=" + _StuId;
                            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                            sql = "DELETE from tblcce_mark where tblcce_mark.StudentId=" + _StuId;
                            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                            sql = "DELETE from tblcce_result where tblcce_result.StudentId=" + _StuId;
                            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        }
                    }
                }

            }
            sql = "DELETE from tblcce_colconfig where tblcce_colconfig.GroupId=" + _Groupid;
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "delete from tblcce_classgroupmap where tblcce_classgroupmap.GroupId=" + _Groupid;
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            sql = "delete from tblcce_classgroup where tblcce_classgroup.Id=" + _Groupid;
            MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            _Errmsg = "Selected group is deleted sucessfully!";
            LoadDefaultValues();
            WC_MessageBox.ShowMssage(_Errmsg);
        }

        #endregion

        

    }
}



