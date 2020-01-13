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
using DayPilot;
using DayPilot.Utils;
using System.Drawing;
namespace WinEr
{
    public partial class TimeTableWizard : System.Web.UI.Page
    {
        private ClassOrganiser MyClassMang;
        private TimeTable MyTimeTable;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private static int[] Perod = {4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40};//upto 13
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyTimeTable = MyUser.GetTimeTableObj();
            MyClassMang = MyUser.GetClassObj();
            if (MyTimeTable == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyClassMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(140))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadGenRules();

                    LoadSchoolConfig();
                    LoadClassToDrpList();
                    LoadClassSettings();
                    LoadSubjectGroups();

                    Load_StaffDrp();
                    Load_SubjectConfiguration();

                    Load_FinalClassDrp();
                    WC_TimeTableControl.ClassId = Drp_WzdStep7Class.SelectedValue;

                    if (MyTimeTable.IsTimeTableGenerated())
                        WC_MessageBox.ShowMssage("Time table is generated already.If you change any configuration you have to regenerate the time table again");
                    //some initlization

                }

                Load_ClassFilled();
            }
        }

       

        # region Genrules

        private void LoadGenRules()
        {
            string[] Configvals = new string[10];
            string sql = "select value1 from tbltime_config";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            int i = 0;
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    Configvals[i] = MyReader.GetValue(0).ToString();
                    i++;
                }
            }
            Txt_PeridDay.Text = Configvals[0].ToString();
            if (Configvals[1].ToString() == "1")
            {
                Chk_FirsrClassT.Checked = true;
            }
            else
            {
                Chk_FirsrClassT.Checked = false;
            }
            Txt_MaxConsecutive.Text = Configvals[2].ToString();
            Txt_MaxteacherSub.Text = Configvals[3].ToString();
            Txt_MaxStaffWeekperiod.Text = Configvals[4].ToString();
            Txt_MaxStaffDayperiod.Text = Configvals[5].ToString();
        }


        //protected void Img_RuleSave_Click(object sender, ImageClickEventArgs e)
        //{
        //    int ClassTeacher=0;
        //    if (Chk_FirsrClassT.Checked)
        //    {
        //        ClassTeacher=1;
        //    }
        //    if (MyTimeTable.IsTimeTableGenerated())
        //    {
        //        MyTimeTable.DeleteTimeTableMaster();
        //        MyTimeTable.SaveGenRules(Txt_PeridDay.Text.Trim(), ClassTeacher, Txt_MaxConsecutive.Text.Trim(), Txt_MaxteacherSub.Text.Trim(), Txt_MaxStaffWeekperiod.Text.Trim(), Txt_MaxStaffDayperiod.Text.Trim());
        //    }
        //    else
        //    {
        //        MyTimeTable.SaveGenRules(Txt_PeridDay.Text.Trim(), ClassTeacher, Txt_MaxConsecutive.Text.Trim(), Txt_MaxteacherSub.Text.Trim(), Txt_MaxStaffWeekperiod.Text.Trim(), Txt_MaxStaffDayperiod.Text.Trim());
        //    }
        //    WC_MessageBox.ShowMssage("General rules saved successfully");
        //}

        # endregion
      
        # region school Config


        private void LoadClassToDrpList()
        {

            Drp_Wzd2Class.Items.Clear();
           
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());

                    Drp_Wzd2Class.Items.Add(li);
               

                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");

                Drp_Wzd2Class.Items.Add(li);
               
            }
            Drp_Wzd2Class.SelectedIndex = 0;
           
        }


     

        private void LoadSchoolConfig()
        {
            Grd_SchoolConfig.Columns[0].Visible = true;
            string sql = "select PeriodId,FrequencyName from tblattendanceperiod where ModeId=3 ";
            MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_SchoolConfig.DataSource = MydataSet;
                Grd_SchoolConfig.DataBind();
                CheckPeriods(-1, "tbltime_generalperiod");
                Grd_SchoolConfig.Columns[0].Visible = true;
            }
        }

        protected void Lnk_ClearAll_Click(object sender, EventArgs e)
        {
            CheckBox Chk_Day;
            CheckBox Chk_Nxtbreak;
            foreach (GridViewRow gv in Grd_SchoolConfig.Rows)
            {
                Chk_Nxtbreak = (CheckBox)gv.FindControl("Chk_Nxtbrk");
                Chk_Nxtbreak.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_MOn");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Tues");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Wed");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Thur");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Fri");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Sat");
                Chk_Day.Checked = false;
                Chk_Day = (CheckBox)gv.FindControl("Chk_Sun");
                Chk_Day.Checked = false;
            }
        }


   

        protected void Drp_Wzd2Class_OnselectedIndexChanged(object sender ,EventArgs e )
        {

            LoadClassSettings();
           
        }

        private void LoadClassSettings()
        {
            DataSet MyDay;
            int PeriodId = 0;
            CheckBox Chk_Day;
            CheckBox Chk_Nxtbreak;
            bool Flag = false;
            string _Table = "tbltime_classperiod";
            int _ClassId = int.Parse(Drp_Wzd2Class.SelectedValue);
            Grd_SchoolConfig.Columns[0].Visible = true;
            int i = 0;
            foreach (GridViewRow gv in Grd_SchoolConfig.Rows)
            {

                //PeriodId = int.Parse(gv.Cells[0].Text.ToString());
                PeriodId = Perod[i];
                Chk_Nxtbreak = (CheckBox)gv.FindControl("Chk_Nxtbrk");
                Chk_Nxtbreak.Checked = MyTimeTable.GetCheckSatusFor(PeriodId, _ClassId, _Table);
                MyDay = MyTimeTable.GetDayIds(PeriodId, _ClassId, _Table);
                if (MyDay != null && MyDay.Tables != null && MyDay.Tables[0].Rows.Count > 0)
                {
                    bool MonDay = false, Tuesday = false, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = false;
                    foreach (DataRow Dr_Day in MyDay.Tables[0].Rows)
                    {

                        if (Dr_Day[0].ToString() == "1")
                        {
                            MonDay = true;
                        }

                        if (Dr_Day[0].ToString() == "2")
                        {
                            Tuesday = true;
                        }

                        if (Dr_Day[0].ToString() == "3")
                        {
                            Wednesday = true;
                        }

                        if (Dr_Day[0].ToString() == "4")
                        {
                            Thursday = true;
                        }

                        if (Dr_Day[0].ToString() == "5")
                        {
                            Friday = true;
                        }

                        if (Dr_Day[0].ToString() == "6")
                        {
                            Saturday = true;
                        }

                        if (Dr_Day[0].ToString() == "7")
                        {
                            Sunday = true;
                        }

                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_MOn");
                    if (MonDay)
                    {
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Tues");
                    if (Tuesday)
                    {
                       
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Wed");
                    if (Wednesday)
                    {
                        
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Thur");
                    if (Thursday)
                    {
                        
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Fri");
                    if (Friday)
                    {
                        
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sat");
                    if (Saturday)
                    {
                        
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sun");
                    if (Sunday)
                    {
                       
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                }
                else
                {
                    Chk_Day = (CheckBox)gv.FindControl("Chk_MOn");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Tues");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Wed");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Thur");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Fri");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sat");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sun");
                    Chk_Day.Checked = false;
                }
                i++;
            }
            Grd_SchoolConfig.Columns[0].Visible = false;
        }


      

        private void CheckPeriods(int _ClassId,string _Table)
        {
            DataSet MyDay;
            int PeriodId = 0;
            CheckBox Chk_Day;
            CheckBox Chk_Nxtbreak;
            int i = 0;
            foreach (GridViewRow gv in Grd_SchoolConfig.Rows)
            {
                
                //PeriodId = int.Parse(gv.Cells[0].Text.ToString());
                PeriodId = Perod[i];
                Chk_Nxtbreak = (CheckBox)gv.FindControl("Chk_Nxtbrk");
                Chk_Nxtbreak.Checked = MyTimeTable.GetCheckSatusFor(PeriodId, _ClassId, _Table);
                MyDay = MyTimeTable.GetDayIds(PeriodId, _ClassId, _Table);
                if (MyDay != null && MyDay.Tables != null && MyDay.Tables[0].Rows.Count > 0)
                {
                    bool MonDay = false, Tuesday = false, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = false;
                    foreach (DataRow Dr_Day in MyDay.Tables[0].Rows)
                    {

                        if (Dr_Day[0].ToString() == "1")
                        {
                            MonDay = true;
                        }

                        if (Dr_Day[0].ToString() == "2")
                        {
                            Tuesday = true;
                        }

                        if (Dr_Day[0].ToString() == "3")
                        {
                            Wednesday = true;
                        }

                        if (Dr_Day[0].ToString() == "4")
                        {
                            Thursday = true;
                        }

                        if (Dr_Day[0].ToString() == "5")
                        {
                            Friday = true;
                        }

                        if (Dr_Day[0].ToString() == "6")
                        {
                            Saturday = true;
                        }

                        if (Dr_Day[0].ToString() == "7")
                        {
                            Sunday = true;
                        }

                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_MOn");
                    if (MonDay)
                    {
                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Tues");
                    if (Tuesday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Wed");
                    if (Wednesday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Thur");
                    if (Thursday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Fri");
                    if (Friday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sat");
                    if (Saturday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sun");
                    if (Sunday)
                    {

                        Chk_Day.Checked = true;
                    }
                    else
                    {
                        Chk_Day.Checked = false;
                    }
                }
                else
                {
                    Chk_Day = (CheckBox)gv.FindControl("Chk_MOn");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Tues");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Wed");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Thur");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Fri");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sat");
                    Chk_Day.Checked = false;
                    Chk_Day = (CheckBox)gv.FindControl("Chk_Sun");
                    Chk_Day.Checked = false;
                }
                i++;

            }
        }

        




        protected void Img_ApplyAll_Click(object sender, EventArgs e)
        {
            if (MyTimeTable.IsTimeTableGenerated())
            {
                MyTimeTable.DeleteTimeTableMaster();
            }
            MyTimeTable.DeleteAllClassSubjectStaff();
            MyTimeTable.DeleteAllClassEntries();
            SaveSchoolGeneralConfiguration(0);
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_Class in MydataSet.Tables[0].Rows)
                {
                    MyTimeTable.REstoreDefaultsToClass(int.Parse(dr_Class[0].ToString()));
                }
            }
            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Time Table", "General Settings applied to all classes", 1);
            WC_MessageBox.ShowMssage("General Settings applied to all classes successfully");
            Load_FinalClassDrp();
        }

     

        protected void Img_SaveSchoolConfig_Click(object sender, EventArgs e)
        {
            if (MyTimeTable.IsTimeTableGenerated())
            {
                MyTimeTable.DeleteTimeTableMaster_Class(Drp_Wzd2Class.SelectedValue);
              
            }
            MyTimeTable.DeleteAllClassSubjectStaff_Class(Drp_Wzd2Class.SelectedValue);
            SaveSchoolGeneralConfiguration(1);
           //// MydataSet = MyUser.MyAssociatedClass();
           //// if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
           // //{
           //     //foreach (DataRow dr_Class in MydataSet.Tables[0].Rows)
           //     //{
           //         MyTimeTable.REstoreDefaultsToClass(int.Parse(Drp_Wzd2Class.SelectedValue.ToString()));
           //     //}
           //// }
            WC_MessageBox.ShowMssage("Saved successfully");
            Load_FinalClassDrp();
        }

        private void SaveSchoolGeneralConfiguration(int IndexStatus)
        {
            int PeriodId = 0;
            CheckBox Chk_Monday;
            CheckBox Chk_TuesDay;
            CheckBox Chk_WednesDay;
            CheckBox Chk_ThursDay;
            CheckBox Chk_FriDay;
            CheckBox Chk_SaturDay;
            CheckBox Chk_SunDay;
            CheckBox Chk_Nxtbreak;
            Grd_SchoolConfig.Columns[0].Visible = true;
            int i = 0;
            foreach (GridViewRow gv in Grd_SchoolConfig.Rows)
            {
                //PeriodId = int.Parse(gv.Cells[0].Text.ToString());
                PeriodId = Perod[i];
                Chk_Monday = (CheckBox)gv.FindControl("Chk_MOn");
                Chk_TuesDay = (CheckBox)gv.FindControl("Chk_Tues");
                Chk_WednesDay = (CheckBox)gv.FindControl("Chk_Wed");
                Chk_ThursDay = (CheckBox)gv.FindControl("Chk_Thur");
                Chk_FriDay = (CheckBox)gv.FindControl("Chk_Fri");
                Chk_SaturDay = (CheckBox)gv.FindControl("Chk_Sat");
                Chk_SunDay = (CheckBox)gv.FindControl("Chk_Sun");
                Chk_Nxtbreak = (CheckBox)gv.FindControl("Chk_Nxtbrk");
                // if index is 0 then use delete inside below funtion if it is 1 then dont use delete
                MyTimeTable.UpdateSchoolConfig(Chk_Monday.Checked, Chk_TuesDay.Checked, Chk_WednesDay.Checked, Chk_ThursDay.Checked, Chk_FriDay.Checked, Chk_SaturDay.Checked, Chk_SunDay.Checked, Chk_Nxtbreak.Checked, PeriodId, IndexStatus);

                if (IndexStatus == 1)
                {
                    MyTimeTable.UpdateClassConfig(Chk_Monday.Checked, Chk_TuesDay.Checked, Chk_WednesDay.Checked, Chk_ThursDay.Checked, Chk_FriDay.Checked, Chk_SaturDay.Checked, Chk_SunDay.Checked, Chk_Nxtbreak.Checked, PeriodId, int.Parse(Drp_Wzd2Class.SelectedValue));
                }
                i++;
            }
            string Table = "tbltime_generalperiod";
            int ClassId = -1;
            if (IndexStatus == 1)
            {
                Table = "tbltime_classperiod";
                ClassId = int.Parse(Drp_Wzd2Class.SelectedValue);
            }
            CheckPeriods(ClassId, Table);
            Grd_SchoolConfig.Columns[0].Visible = false;
        }


       

        # endregion

        # region Subject Configuration


        protected void Lnk_SubjectGroups_Click(object sender, EventArgs e)
        {
            Lnk_SubjectGroups.Visible = Panel_SubjectGrouping.Visible;
            Panel_SubjectGrouping.Visible = !Panel_SubjectGrouping.Visible;
            
        }


        protected void Image_Pnael_SubjectGrouping_Click(object sender, ImageClickEventArgs e)
        {
            Lnk_SubjectGroups.Visible = Panel_SubjectGrouping.Visible;
            Panel_SubjectGrouping.Visible = !Panel_SubjectGrouping.Visible;
        }



        private void LoadSubjectGroups()
        {
            DataSet MySuGroups;

            string sql = "select Id,Name,AdjPeriods,MaxPeriodWeek,MinPeriodWeek from tbltime_subgroup";
            MySuGroups = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MySuGroups != null && MySuGroups.Tables != null && MySuGroups.Tables[0].Rows.Count > 0)
            {
                Grd_SubGrp.Columns[0].Visible = true;
                Grd_SubGrp.DataSource = MySuGroups;
                Grd_SubGrp.DataBind();
                Grd_SubGrp.Columns[0].Visible = false;
            }

            Load_SubjectConfiguration();
        }


        protected void GrdSubGrp_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            //int GrpId = int.Parse(Grd_SubGrp.SelectedRow.Cells[0].Text.ToString());
            int GrpId = int.Parse(Grd_SubGrp.DataKeys[e.RowIndex].Values["Id"].ToString());
            Load_Drp_RepalcementGroup(GrpId);
            Hidden_DeleteGroupId.Value = GrpId.ToString();
            Lbl_GroupDeleteMsg.Text = "";
            MPE_DeleteGroup.Show();
          
        }

        private void Load_Drp_RepalcementGroup(int GrpId)
        {
            Drp_ReplaceGroup.Items.Clear();
            ListItem li = new ListItem("Select Group", "-1");
            Drp_ReplaceGroup.Items.Add(li);
            string sql = " select distinct Id,Name from tbltime_subgroup WHERE Id!=" + GrpId;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_ReplaceGroup.Items.Add(li);
                }
            }
            else
            {
                Drp_ReplaceGroup.Items.Clear();
                li = new ListItem("No Class present", "-1");
                Drp_ReplaceGroup.Items.Add(li);
            }
        }

        protected void Button_DeleteGroup_Click(object sender, EventArgs e)
        {
            Lbl_GroupDeleteMsg.Text = "";
            if (Drp_ReplaceGroup.SelectedValue != "-1")
            {
                try
                {
                    MyTimeTable.CreateTansationDb();

                    string sql = "UPDATE tblsubjects SET sub_Catagory="+Drp_ReplaceGroup.SelectedValue+" WHERE sub_Catagory="+Hidden_DeleteGroupId.Value;
                    MyTimeTable.m_TransationDb.ExecuteQuery(sql);

                    sql = "DELETE FROM tbltime_subgroup WHERE Id=" + Hidden_DeleteGroupId.Value;
                    MyTimeTable.m_TransationDb.ExecuteQuery(sql);

                    MyTimeTable.EndSucessTansationDb();

                    LoadSubjectGroups();
                }
                catch
                {
                    MyTimeTable.EndFailTansationDb();
                }

            }
            else
            {
                Lbl_GroupDeleteMsg.Text = "Please select replacement group";
                MPE_DeleteGroup.Show();
            }

        }

       


        protected void Grd_studentlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int GrpId = int.Parse(Grd_SubGrp.DataKeys[e.NewEditIndex].Values["Id"].ToString());
            LoadDetails(GrpId);
            LoadSubjectgrid(GrpId);
            Hdn_Grpid.Value = GrpId.ToString();
            Hdn_SubGrp.Value = "1";//Edit
            MPE_EdtSubGrp.Show();

        }
        private void LoadSubjectgrid(int _GrpId)
        {
            Txt_SubName.Text = "";
            Txt_SubCode.Text = "";
            DataSet Subjectlist;
            Grd_Subject.Columns[3].Visible = true;
            string sql = "select subject_name as SubName,SubjectCode as SubCode from tblsubjects where sub_Catagory=" + _GrpId;
            Subjectlist = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            Grd_Subject.DataSource = Subjectlist;
            Grd_Subject.DataBind();
            LoadSubgroups();
            Grd_Subject.Columns[2].Visible = true;
            Grd_Subject.Columns[3].Visible = false;
        }

      

        private void LoadSubgroups()
        {
            DropDownList DrpSubGrp;
            string SubName = "";
           
            foreach (GridViewRow gv in Grd_Subject.Rows)
            {
                DrpSubGrp = (DropDownList)gv.FindControl("Grd_Subject_DrpSubject");
                SubName = gv.Cells[0].ToString();
                LoadSubjectGroups(DrpSubGrp);

                DrpSubGrp.SelectedValue = Hdn_Grpid.Value;
                
            }
        }

        private void LoadSubjectGroups(DropDownList DrpSubGrp)
        {
            DrpSubGrp.Items.Clear();
            string sql=" select distinct Id,Name from tbltime_subgroup";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    DrpSubGrp.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class present", "-1");
                DrpSubGrp.Items.Add(li);
            }
           
        }

        private void LoadDetails(int GrpId)
        {
            Lbl_EdtSubGrpMessage.Text = "";
            string sql = "select Name,AdjPeriods,MaxPeriodWeek,MinPeriodWeek from tbltime_subgroup where Id="+ GrpId;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Hdn_SubGrpName.Value = MyReader.GetValue(0).ToString();
                Txt_SubGrpName.Text = MyReader.GetValue(0).ToString();
                Txt_SubGrpAdjPeriods.Text = MyReader.GetValue(1).ToString();
                Txt_SubGrpMaxPrd.Text = MyReader.GetValue(2).ToString();
                Txt_SubGrpMinPrd.Text = MyReader.GetValue(3).ToString();
                Hdn_Grpid.Value = GrpId.ToString();
            }
        }
        protected void Btn_SubGrp_AddSub_Click(object sender, EventArgs e)
        {
            if (MyTimeTable.IsTimeTableGenerated())
            {
                MyTimeTable.DeleteTimeTableMaster();
                MyTimeTable.DeleteAllClassSubjectStaff();
            }
            string Message="";
            Lbl_EdtSubGrpMessage.Text = "";
            if (MyTimeTable.ValidSubjects(Txt_SubName.Text.Trim(),Txt_SubCode.Text.Trim(),out Message))
            {
                if (NotAddedtoGrid(Txt_SubName.Text.Trim(), Txt_SubCode.Text.Trim(), out Message))
                {
                    DataSet _Subdataset = new DataSet();
                    DataTable dt;
                    DataRow dr;
                    if (Grd_Subject.Rows.Count > 0)
                    {
                        _Subdataset.Tables.Add(new DataTable("Subjects"));
                        dt = _Subdataset.Tables["Subjects"];
                        dt.Columns.Add("SubName");
                        dt.Columns.Add("SubCode");

                        foreach (GridViewRow gv in Grd_Subject.Rows)
                        {
                            dr = _Subdataset.Tables["Subjects"].NewRow();
                            dr["SubName"] = gv.Cells[0].Text.ToString();
                            dr["SubCode"] = gv.Cells[1].Text.ToString();
                            _Subdataset.Tables["Subjects"].Rows.Add(dr);
                        }

                        dr = _Subdataset.Tables["Subjects"].NewRow();
                        dr["SubName"] = Txt_SubName.Text.Trim();
                        dr["SubCode"] = Txt_SubCode.Text.Trim();
                        _Subdataset.Tables["Subjects"].Rows.Add(dr);
                        Grd_Subject.DataSource = _Subdataset;
                        Grd_Subject.DataBind();
                        if (Hdn_SubGrp.Value == "1")
                        {
                            LoadSubgroups();
                            Grd_Subject.Columns[2].Visible = true;
                        }
                        else
                        {
                            Grd_Subject.Columns[2].Visible = false;
                        }
                    }
                    else
                    {
                        _Subdataset.Tables.Add(new DataTable("Subjects"));
                        dt = _Subdataset.Tables["Subjects"];
                        dt.Columns.Add("SubName");
                        dt.Columns.Add("SubCode");
                        dr = _Subdataset.Tables["Subjects"].NewRow();
                        dr["SubName"] = Txt_SubName.Text.Trim();
                        dr["SubCode"] = Txt_SubCode.Text.Trim();
                        _Subdataset.Tables["Subjects"].Rows.Add(dr);
                        Grd_Subject.DataSource = _Subdataset;
                        Grd_Subject.DataBind();
                        if (Hdn_SubGrp.Value == "1")
                        {
                            LoadSubgroups();
                            Grd_Subject.Columns[2].Visible = true;
                        }
                        else
                        {
                            Grd_Subject.Columns[2].Visible = false;
                        }
                    }
                    Txt_SubName.Text = "";
                    Txt_SubCode.Text = "";
                }
                else
                {
                    Lbl_EdtSubGrpMessage.Text = Message;
                }
            }
            else
            {
                Lbl_EdtSubGrpMessage.Text = Message;
            }
            MPE_EdtSubGrp.Show();
        }

        private bool NotAddedtoGrid(string _SubName, string _SubCode, out string Message)
        {
            Message = "";
            bool valid = true;
            foreach (GridViewRow gv in Grd_Subject.Rows)
            {
                if (gv.Cells[0].Text.ToLower().Trim() == _SubName.ToLower())
                {
                    Message = "Subject name cannot be repeted";
                    valid = false;
                }
                else if (gv.Cells[1].Text.ToLower().Trim() == _SubCode.ToLower())
                {
                    Message = "Subject code cannot be repeted";
                    valid = false;
                }
            }
            return valid;
        }

       
        protected void DeleteSubjects(object sender, GridViewDeleteEventArgs e)
        {
            string Subname = Grd_Subject.Rows[e.RowIndex].Cells[0].Text.Trim();
            DataSet _Subdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Subdataset.Tables.Add(new DataTable("Subjects"));
            dt = _Subdataset.Tables["Subjects"];
            dt.Columns.Add("SubName");
            dt.Columns.Add("SubCode");
            foreach (GridViewRow gv in Grd_Subject.Rows)
            {
                if (gv.Cells[0].Text.ToLower().Trim() != Subname.ToLower())
                {
                    dr = _Subdataset.Tables["Subjects"].NewRow();
                    dr["SubName"] = gv.Cells[0].Text.ToString();
                    dr["SubCode"] = gv.Cells[1].Text.ToString();
                    _Subdataset.Tables["Subjects"].Rows.Add(dr);
                }
            }
            Grd_Subject.DataSource = _Subdataset;
            Grd_Subject.DataBind();
            Grd_Subject.Columns[2].Visible = false;
            MPE_EdtSubGrp.Show();
        }


        protected void Img_EditsubGrp_Click(object sender, EventArgs e)
        {
            MPE_EdtSubGrp.Show();
            Hdn_SubGrp.Value = "2";//New
            ClearAll();
        }

        private void ClearAll()
        {
            Txt_SubGrpName.Text = "";
            Txt_SubGrpAdjPeriods.Text = "";
            Txt_SubGrpMaxPrd.Text = "";
            Txt_SubGrpMinPrd.Text = "";
            Grd_Subject.DataSource = null;
            Grd_Subject.DataBind();
        }


        protected void Btn_EditSubjectGrpSave_Click(object sender, EventArgs e)
        {
            DropDownList DrpGroup;
            if (MyTimeTable.IsTimeTableGenerated())
            {
                MyTimeTable.DeleteTimeTableMaster();
                //MyTimeTable.DeleteAllClassSubjectStaff(); Arun Changed
            }
            if (Hdn_SubGrp.Value == "2")//New Group
            {
                if (ValidSubGroup()&&!MyTimeTable.SubGrpExists(Txt_SubGrpName.Text.Trim()))
                {
                    int GrpId = MyTimeTable.SaveSubGroup(Txt_SubGrpName.Text.Trim(), Txt_SubGrpAdjPeriods.Text.Trim(), Txt_SubGrpMaxPrd.Text.Trim(), Txt_SubGrpMinPrd.Text.Trim());
                    if (Grd_Subject.Rows.Count > 0)
                    {
                        foreach (GridViewRow gv in Grd_Subject.Rows)
                        {
                            MyTimeTable.CreateSubject(gv.Cells[0].Text.ToString(), gv.Cells[1].Text.ToString(), GrpId);
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Time Table", "" + gv.Cells[0].Text.ToString() + " Subject group added", 1);
                        }
                    }

                   
                    WC_MessageBox.ShowMssage("Subject group saved");
                }
                else
                {
                    WC_MessageBox.ShowMssage("Subject group already exists");
                }
            }
            else//Edit Group
            {
                if (ValidSubGroup()&&ValidGroup())
                {
                        MyTimeTable.UpdateSubGroup(Txt_SubGrpName.Text.Trim(), Txt_SubGrpAdjPeriods.Text.Trim(), Txt_SubGrpMaxPrd.Text.Trim(), Txt_SubGrpMinPrd.Text.Trim(), Hdn_Grpid.Value);
                        foreach (GridViewRow gv in Grd_Subject.Rows)
                        {
                            DrpGroup = (DropDownList)gv.FindControl("Grd_Subject_DrpSubject");
                            if (MyTimeTable.NewSubject(gv.Cells[0].Text.ToString()))
                            {
                                MyTimeTable.CreateSubject(gv.Cells[0].Text.ToString(), gv.Cells[1].Text.ToString(), int.Parse(DrpGroup.SelectedValue));
                            }
                            else
                            {
                                MyTimeTable.ChangeGroupForSubject(gv.Cells[0].Text.ToString(), int.Parse(DrpGroup.SelectedValue));
                            }
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Time Table", ""+gv.Cells[0].Text.ToString()+" Subject group edited", 1);
                        }
                       
                        LoadSubjectgrid(int.Parse(Hdn_Grpid.Value));
                        WC_MessageBox.ShowMssage("Details saved");
                }
              
            }
            LoadSubjectGroups();
            MPE_EdtSubGrp.Show();
        }

        private bool ValidSubGroup()
        {
            bool _valid = true;
            if( (Txt_SubGrpName.Text.Trim()=="") || (Txt_SubGrpAdjPeriods.Text.Trim()=="") || ( Txt_SubGrpMaxPrd.Text.Trim()=="") || ( Txt_SubGrpMinPrd.Text.Trim()==""))
            {
                _valid = false;
            }
            return _valid;
        }

        

        private bool ValidGroup()//Checking Whether Group Already Exists if name has changed
        {
            bool valid = true;
            if (Hdn_SubGrpName.Value != Txt_SubGrpName.Text)
            {
                if (MyTimeTable.SubGrpExists(Txt_SubGrpName.Text.Trim()))
                {
                    valid = false;
                }
            }
            return valid;
        }





        /// Subject Grid


       


        private void Load_StaffDrp()
        {
            Drp_AddStaff.Items.Clear();
            MydataSet = MyTimeTable.GetStaffs();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Drp_AddStaff.Items.Add(new ListItem("Select Staff", "-1"));
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    Drp_AddStaff.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
            }
            else
            {
                Drp_AddStaff.Items.Add(new ListItem("No Staff Found", "-1"));
            }
        }


        private void Load_SubjectConfiguration()
        {
            MydataSet = Get_SubjectConfigurationDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grid_SubjectConfig.Columns[0].Visible = true;
                Grid_SubjectConfig.Columns[1].Visible = true;
                Grid_SubjectConfig.DataSource = MydataSet;
                Grid_SubjectConfig.DataBind();
                Grid_SubjectConfig.Columns[0].Visible = false;
                Grid_SubjectConfig.Columns[1].Visible = false;
                LoadColor_SubjectConfig();
            }
        }

        private void LoadColor_SubjectConfig()
        {
            foreach (GridViewRow gv in Grid_SubjectConfig.Rows)
            {
                int EstimatedStaffs = 0, AssignedStaffs = 0;
                int.TryParse(gv.Cells[6].Text, out EstimatedStaffs);
                int.TryParse(gv.Cells[7].Text, out AssignedStaffs);
                if (EstimatedStaffs == AssignedStaffs)
                {
                    gv.Cells[7].ForeColor = Color.Green;
                }
                else if (EstimatedStaffs < AssignedStaffs)
                {
                    gv.Cells[7].ForeColor = Color.Orange;
                }
                else
                {
                    gv.Cells[7].ForeColor = Color.Red;
                }
            }
        }

        private DataSet Get_SubjectConfigurationDataSet()
        {
            string SubjectID = "", SubjectName = "", Group = "",GroupId="";
            int NoOfClass = 0;
            double EstimatedStaffs = 0, EstimatedPeriods = 0, AssignedStaffs = 0;
            DataSet _Subdataset = new DataSet();
            DataTable dt;
            DataRow dr;

            _Subdataset.Tables.Add(new DataTable("Subjects"));
            dt = _Subdataset.Tables["Subjects"];
            dt.Columns.Add("SubjectId");
            dt.Columns.Add("GroupId");
            dt.Columns.Add("Subject");
            dt.Columns.Add("Group");
            dt.Columns.Add("NoOfClass");
            dt.Columns.Add("EstimatedPeriods");
            dt.Columns.Add("EstimatedStaffs");
            dt.Columns.Add("AssignedStaffs");

            string sql = "SELECT tblsubjects.Id,tblsubjects.subject_name,tbltime_subgroup.Name,tbltime_subgroup.Id FROM tblsubjects INNER JOIN tbltime_subgroup ON tblsubjects.sub_Catagory=tbltime_subgroup.Id";
            OdbcDataReader OuterReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (OuterReader.HasRows)
            {
                while (OuterReader.Read())
                {
                    SubjectID = OuterReader.GetValue(0).ToString();
                    SubjectName = OuterReader.GetValue(1).ToString().Replace("amp;", "");
                    Group = OuterReader.GetValue(2).ToString();
                    GroupId = OuterReader.GetValue(3).ToString();
                    NoOfClass = MyTimeTable.TotalClassCount_Subject(SubjectID);
                    EstimatedPeriods = MyTimeTable.GetEstimatedPeriods_Subject(SubjectID);
                    int maxPeriodsPerWeek = 0;
                    EstimatedStaffs = 0;
                    int.TryParse(MyTimeTable.GetTeacherMaxPeriod(), out maxPeriodsPerWeek);
                    if (maxPeriodsPerWeek > 0)
                    {
                        EstimatedStaffs = EstimatedPeriods / (double)maxPeriodsPerWeek;
                    }
                    EstimatedStaffs = Math.Round(Math.Ceiling(EstimatedStaffs), 0);
                    AssignedStaffs = MyTimeTable.GetTotalStaff_Subject(SubjectID);
                    dr = _Subdataset.Tables["Subjects"].NewRow();
                    dr["SubjectId"] = SubjectID;
                    dr["GroupId"] = GroupId;
                    dr["Subject"] = SubjectName;
                    dr["Group"] = Group;
                    dr["NoOfClass"] = NoOfClass;
                    dr["EstimatedPeriods"] = EstimatedPeriods;
                    dr["EstimatedStaffs"] = EstimatedStaffs;
                    dr["AssignedStaffs"] = AssignedStaffs;
                    _Subdataset.Tables["Subjects"].Rows.Add(dr);



                }
            }
            return _Subdataset;
        }


        protected void Grid_SubjectConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string SubjectId = Grid_SubjectConfig.DataKeys[e.NewEditIndex].Values["SubjectId"].ToString();
            string GroupId = Grid_SubjectConfig.DataKeys[e.NewEditIndex].Values["GroupId"].ToString();
            Hidden_SelectedSubject.Value = SubjectId;
            Hidden_SelectedGroupId.Value = GroupId;
            Load_ChangeGroup_PopUp();

        }

        private void Load_ChangeGroup_PopUp()
        {
            Lbl_ChangeGroupError.Text = "";
            Load_Group();
            MPE_ChangeGroup.Show();
        }

        private void Load_Group()
        {
            Drp_ChangeGroup.Items.Clear();
            string sql = "SELECT tbltime_subgroup.Id,tbltime_subgroup.Name FROM tbltime_subgroup";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    Drp_ChangeGroup.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
                }

                Drp_ChangeGroup.SelectedValue = Hidden_SelectedGroupId.Value;
            }
            else
            {
                Drp_ChangeGroup.Items.Add(new ListItem("No group found", "-1"));
            }
        }

        protected void Btn_ChangeGroup_Click(object sender, EventArgs e)
        {
            Lbl_ChangeGroupError.Text = "";
            string msg = "";
            if (IsChangePossible(out msg))
            {
                try
                {

                    MyTimeTable.ChangeGroupForSubjectId(Hidden_SelectedSubject.Value, int.Parse(Drp_ChangeGroup.SelectedValue));
                    Load_SubjectConfiguration();
                }
                catch
                {
                    Lbl_ChangeGroupError.Text = "Error while changing group. Try later";
                    MPE_ChangeGroup.Show();
                }
            }
            else
            {
                Lbl_ChangeGroupError.Text = msg;
                MPE_ChangeGroup.Show();
            }
        }

        private bool IsChangePossible(out string msg)
        {
            bool valid = true;
            msg = "";

            if (Drp_ChangeGroup.SelectedValue == Hidden_SelectedGroupId.Value)
            {
                msg = "You have selected same group";
                valid = false;
            }
            else if (Drp_ChangeGroup.SelectedValue=="-1")
            {
                msg = "Please select a group";
                valid = false;
            }

            return valid;

        }

        protected void Grid_SubjectConfig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string SubjectId = Grid_SubjectConfig.DataKeys[e.RowIndex].Values["SubjectId"].ToString();
            Hidden_SelectedSubject.Value = SubjectId;
            Load_AssignedStaffs_Subject();
        }

        private void Load_AssignedStaffs_Subject()
        {
            Lbl_StaffErrorMsg.Text = "";
            string sql = "SELECT tblstaffsubjectmap.StaffId,tbluser.SurName FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + Hidden_SelectedSubject.Value;
            MydataSet = MyTimeTable.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_AssignedStaffs.Columns[0].Visible = true;
                Grd_AssignedStaffs.DataSource = MydataSet;
                Grd_AssignedStaffs.DataBind();
                Grd_AssignedStaffs.Columns[0].Visible = false;
            }
            else
            {
                Lbl_StaffErrorMsg.Text = "No staff assigned for selected subject";
                Grd_AssignedStaffs.DataSource = null;
                Grd_AssignedStaffs.DataBind();
            }

            MPE_AssignedStaff.Show();
        }

        protected void Btn_AddStaff_Click(object sender, EventArgs e)
        {
            string msg = "";
            Lbl_StaffErrorMsg.Text = "";
            if (isStaffAdditionPossible(out msg))
            {
                try
                {
                    MyTimeTable.AddSubjectsToStaff(int.Parse(Hidden_SelectedSubject.Value), int.Parse(Drp_AddStaff.SelectedValue));
                    Load_AssignedStaffs_Subject();
                    Load_SubjectConfiguration();
                }
                catch
                {
                    Lbl_StaffErrorMsg.Text = "Error while adding. Try again";
                    MPE_AssignedStaff.Show();
                }

            }
            else
            {
                Lbl_StaffErrorMsg.Text = msg;
                MPE_AssignedStaff.Show();
            }
        }

        private bool isStaffAdditionPossible(out string msg)
        {
            bool Valid = true;
            msg = "";
            if (Drp_AddStaff.SelectedValue == "-1")
            {
                Valid = false;
                msg = "Please select staff";
            }
            else
            {

                foreach (GridViewRow gv in Grd_AssignedStaffs.Rows)
                {
                    if (Drp_AddStaff.SelectedValue == gv.Cells[0].Text)
                    {
                        Valid = false;
                        msg = Drp_AddStaff.SelectedItem.Text + " already assigned for the subject";
                        break;
                    }
                }
            }
            return Valid;
        }

        protected void Grd_AssignedStaffs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string StaffId = Grd_AssignedStaffs.DataKeys[e.RowIndex].Values["StaffId"].ToString();
            try
            {
                MyTimeTable.CreateTansationDb();
                string sql = "DELETE FROM tblstaffsubjectmap WHERE StaffId=" + StaffId + " AND SubjectId=" + Hidden_SelectedSubject.Value;
                MyTimeTable.m_TransationDb.ExecuteQuery(sql);

                sql = "DELETE FROM tblclassstaffmap WHERE StaffId=" + StaffId + " AND SubjectId=" + Hidden_SelectedSubject.Value;
                MyTimeTable.m_TransationDb.ExecuteQuery(sql);

                sql = "DELETE FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId in( SELECT tblclasssubmap.Id FROM tblclasssubmap WHERE tblclasssubmap.SubjectId=" + Hidden_SelectedSubject.Value + ") AND tbltime_classsubjectstaff.StaffId=" + StaffId;
                MyTimeTable.m_TransationDb.ExecuteQuery(sql);

                MyTimeTable.EndSucessTansationDb();
                Load_AssignedStaffs_Subject();
                Load_SubjectConfiguration();
            }
            catch
            {
                MyTimeTable.EndFailTansationDb();
                Lbl_StaffErrorMsg.Text = "Error while deleting. Try Later";
                MPE_AssignedStaff.Show();
            }
        }


        # endregion

        # region Class Subject Allotment


        private void Load_ClassFilled()
        {
            bool disable = false;
            PlaceHolder1.Controls.Clear();
            int Count = 1;
            Table tableList = new Table();
            tableList.ControlStyle.CssClass = "ClassTable";
            tableList.CellSpacing = 5;
            TableRow tblRow;
            TableCell tblCell;
            HtmlAnchor link;
            tblRow = new TableRow();
            tblRow.ControlStyle.CssClass = "ClassTR";
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    disable = false;
                    //ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    tblCell = new TableCell();
                    link = new HtmlAnchor();
                    link.InnerText = dr[1].ToString();
                    link.ID = dr[0].ToString();
                    //link.Style.Value = "AnchorStyle";
                    //link.ServerClick += new System.EventHandler(this.Anchor_Command);
                    //tblCell.Controls.Add(link);
                    if (!MyTimeTable.IsClassAvailibilityStored(dr[0].ToString()))
                    {
                        tblCell.ControlStyle.CssClass = "YellowCell";

                        disable = true;
                    }
                    else if (HasAllPeriodsFilled_Class(dr[0].ToString()))
                    {
                        tblCell.ControlStyle.CssClass = "GreenCell";
                    }
                    else
                    {
                        tblCell.ControlStyle.CssClass = "RedCell";
                    }

                    link.Style.Value = "AnchorStyle";
                    if (!disable)
                    {
                        link.ServerClick += new System.EventHandler(this.Anchor_Command);
                    }
                    tblCell.Controls.Add(link);
                    tblRow.Cells.Add(tblCell);
                    if (Count % 6 == 0)
                    {
                        tableList.Rows.Add(tblRow);
                        tblRow = new TableRow();
                        tblRow.ControlStyle.CssClass = "ClassTR";
                    }
                    Count++;
                }
                tableList.Rows.Add(tblRow);
            }

            PlaceHolder1.Controls.Add(tableList);
        }

        private bool HasAllPeriodsFilled_Class(string ClassId)
        {
            bool FullPeriodFilled = false;
            bool PeriodExists = false;
            string ClassSubMapId = "";
            int StaffId = 0, PeriodCount = 0, TotalPeriodCount = 0;
            string sql = "SELECT tblclasssubmap.Id,tbltime_subgroup.MinPeriodWeek FROM tblclasssubmap INNER JOIN tblsubjects ON tblsubjects.Id=tblclasssubmap.SubjectId INNER JOIN tbltime_subgroup ON tbltime_subgroup.Id=tblsubjects.sub_Catagory WHERE tblclasssubmap.ClassId=" + ClassId;
            OdbcDataReader OuterReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (OuterReader.HasRows)
            {
                while (OuterReader.Read())
                {
                    StaffId = 0;
                    PeriodCount = 0;
                    PeriodExists = false;
                    ClassSubMapId = OuterReader.GetValue(0).ToString();
                  
                    int.TryParse(OuterReader.GetValue(1).ToString(), out PeriodCount);
                    sql = "SELECT tbltime_classsubjectstaff.StaffId,tbltime_classsubjectstaff.PeriodCount FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId=" + ClassSubMapId;
                    OdbcDataReader InnerReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                    if (InnerReader.HasRows)
                    {
                        while (InnerReader.Read())
                        {
                            StaffId = 0;

                            if (int.TryParse(InnerReader.GetValue(0).ToString(), out StaffId) && int.TryParse(InnerReader.GetValue(1).ToString(), out PeriodCount))
                            {
                                PeriodExists = true;
                                TotalPeriodCount = TotalPeriodCount + PeriodCount;
                            }
                        }
                    }


                    if (!PeriodExists)
                    {
                        TotalPeriodCount = TotalPeriodCount + PeriodCount;

                    }

                }
            }

            int MaxPeriodCount = int.Parse(MyTimeTable.GetTotalClassAllocationPeriods(ClassId));
            if (MaxPeriodCount == TotalPeriodCount)
            {
                FullPeriodFilled = true;
            }

            return FullPeriodFilled;
        }


        void Anchor_Command(object sender, EventArgs e)
        {
            var control = sender as HtmlAnchor;
            string ClassId = control.ID;
            HdnSelectedClassId.Value = ClassId;
            Load_Subjects_SelectedClass(ClassId);
            Lbl_SlectedClassName.Text = control.InnerText;
            Show_SelectedSubjectsPopup();

        }

        private void Load_Subjects_SelectedClass(string ClassId)
        {

            Txt_MaxPeriods.Value = MyTimeTable.GetTotalClassAllocationPeriods(HdnSelectedClassId.Value);
            Lbl_GridSubjectsError.Text = "";
            MydataSet = GetSubjectDataSet(ClassId);
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Panel_SelectedSubjects.Visible = true;
                Grd_SelectedSubjects.Columns[0].Visible = true;
                Grd_SelectedSubjects.Columns[1].Visible = true;
                Grd_SelectedSubjects.Columns[2].Visible = true;
                Grd_SelectedSubjects.Columns[3].Visible = true;

                Grd_SelectedSubjects.DataSource = MydataSet;
                Grd_SelectedSubjects.DataBind();

                Grd_SelectedSubjects.Columns[0].Visible = false;
                Grd_SelectedSubjects.Columns[1].Visible = false;
                Grd_SelectedSubjects.Columns[2].Visible = false;
                Grd_SelectedSubjects.Columns[3].Visible = false;

                Load_OtherGridDetails();
            }
            else
            {

                Panel_SelectedSubjects.Visible = false;
                Lbl_GridSubjectsError.Text = "No Subjects Selected For Class";
                Grd_SelectedSubjects.DataSource = null;
                Grd_SelectedSubjects.DataBind();
            }
        }

        private void Load_OtherGridDetails()
        {
            string ClassSubId = "", SubjectId="", StaffId="", PerodCount="";
            foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
            {
                ClassSubId = gv.Cells[0].Text.ToString();
                SubjectId = gv.Cells[1].Text.ToString();
                StaffId = gv.Cells[2].Text.ToString();
                PerodCount = gv.Cells[3].Text.ToString();
                DropDownList DrpStaff = (DropDownList)gv.FindControl("Drp_GridStaff");
                TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                Load_DrpStaff(DrpStaff, SubjectId);
                Txt_PeriodCount.Text = PerodCount;
                if (int.Parse(StaffId) > 0)
                {
                    DrpStaff.SelectedValue = StaffId;
                }
                if (int.Parse(PerodCount) == 0)
                {
                    Txt_PeriodCount.Text = MyTimeTable.GetMinPeriod(int.Parse(SubjectId)).ToString();
                }
              
            }
             
        }

        private void Load_DrpStaff(DropDownList DrpStaff,string SubjectId)
        {
            DrpStaff.Items.Clear();
            string sql = " SELECT tbluser.SurName,tblstaffsubjectmap.StaffId FROM tblstaffsubjectmap INNER JOIN tbluser ON tbluser.Id=tblstaffsubjectmap.StaffId WHERE tblstaffsubjectmap.SubjectId=" + SubjectId;
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                DrpStaff.Items.Add(new ListItem("Any", "-1"));
                while (MyReader.Read())
                {
                    DrpStaff.Items.Add(new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString()));
                }
                DrpStaff.SelectedIndex = 0;
            }
            else
            {
                DrpStaff.Items.Add(new ListItem("No Staff Assigned", "-1"));
            }
        }

        private DataSet GetSubjectDataSet(string ClassId)
        {
            bool PeriodExists = false;
            string ClassSubMapId = "", SubjectsId = "", SubjectName = "";
            int StaffId = 0, PeriodCount = 0;
            DataSet _Subdataset = new DataSet();
            DataTable dt;
            DataRow dr;

            _Subdataset.Tables.Add(new DataTable("Subjects"));
            dt = _Subdataset.Tables["Subjects"];
            dt.Columns.Add("ClassSubId");
            dt.Columns.Add("SubjectId");
            dt.Columns.Add("StaffId");
            dt.Columns.Add("PerodCount");
            dt.Columns.Add("SubName");

            string sql = "SELECT tblclasssubmap.Id,tblclasssubmap.SubjectId,tblsubjects.subject_name,tbltime_subgroup.MinPeriodWeek FROM tblclasssubmap INNER JOIN tblsubjects ON tblsubjects.Id=tblclasssubmap.SubjectId INNER JOIN tbltime_subgroup ON tbltime_subgroup.Id=tblsubjects.sub_Catagory WHERE tblclasssubmap.ClassId=" + ClassId;
            OdbcDataReader OuterReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (OuterReader.HasRows)
            {
                while (OuterReader.Read())
                {
                    StaffId = 0;
                    PeriodCount = 0;
                    PeriodExists = false;
                    ClassSubMapId = OuterReader.GetValue(0).ToString();
                    SubjectsId = OuterReader.GetValue(1).ToString();
                    SubjectName = OuterReader.GetValue(2).ToString().Replace("amp;", "");
                    int.TryParse(OuterReader.GetValue(3).ToString(), out PeriodCount);
                    sql = "SELECT tbltime_classsubjectstaff.StaffId,tbltime_classsubjectstaff.PeriodCount FROM tbltime_classsubjectstaff WHERE tbltime_classsubjectstaff.ClassSubjectId=" + ClassSubMapId;
                    OdbcDataReader InnerReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
                    if (InnerReader.HasRows)
                    {
                        while (InnerReader.Read())
                        {
                            StaffId = 0;
                            
                            if (int.TryParse(InnerReader.GetValue(0).ToString(), out StaffId) && int.TryParse(InnerReader.GetValue(1).ToString(), out PeriodCount))
                            {

                                dr = _Subdataset.Tables["Subjects"].NewRow();
                                dr["ClassSubId"] = ClassSubMapId;
                                dr["SubjectId"] = SubjectsId;
                                dr["StaffId"] = StaffId;
                                dr["PerodCount"] = PeriodCount;
                                dr["SubName"] = SubjectName;
                                _Subdataset.Tables["Subjects"].Rows.Add(dr);
                                PeriodExists = true;
                            }


                        }
                    }


                    if (!PeriodExists)
                    {

                        dr = _Subdataset.Tables["Subjects"].NewRow();
                        dr["ClassSubId"] = ClassSubMapId;
                        dr["SubjectId"] = SubjectsId;
                        dr["StaffId"] = StaffId;
                        dr["PerodCount"] = PeriodCount;
                        dr["SubName"] = SubjectName;
                        _Subdataset.Tables["Subjects"].Rows.Add(dr);
                    }

                }
            }

            return _Subdataset;
        }


        protected void Btn_SelectedSubjects_Save_Click(object sender, EventArgs e)
        {
            Lbl_GridSubjectsError.Text = "";
            string msg = "";
            if (SavePossible(out msg))
            {

                string ClassSubId = "", SubjectId = "", StaffId = "", PerodCount = "", NewStaffId = "",NewPeriodCount ="";
                try
                {
                    MyTimeTable.CreateTansationDb();
                    foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
                    {
                        ClassSubId = gv.Cells[0].Text.ToString();
                        SubjectId = gv.Cells[1].Text.ToString();
                        StaffId = gv.Cells[2].Text.ToString();
                        PerodCount = gv.Cells[3].Text.ToString();
                        DropDownList DrpStaff = (DropDownList)gv.FindControl("Drp_GridStaff");
                        TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                        NewStaffId = DrpStaff.SelectedValue;
                        NewPeriodCount = Txt_PeriodCount.Text;
                        if (NewPeriodCount != PerodCount || StaffId!=NewStaffId)
                        {
                            if (NewStaffId != "-1")
                            {
                                if (StaffId != NewStaffId)
                                {
                                    RemoveClassFromStaff_WithSubject(StaffId, HdnSelectedClassId.Value,SubjectId);
                                    AssignClassToStaff_WithSubject(NewStaffId, HdnSelectedClassId.Value, SubjectId);
                                    MyTimeTable.DeleteClassSubjectStaff_ParticularStaff(int.Parse(ClassSubId),int.Parse(StaffId));
                                    InsertClassSubjectStaff_Entry(ClassSubId, NewStaffId, NewPeriodCount);
                                }
                                else
                                {
                                    Update_ClassSubjectStaff_Entry(ClassSubId, NewStaffId, NewPeriodCount);
                                }
                               
                            }

                        }
                    }
                    
                    MyTimeTable.EndSucessTansationDb();
                    Load_ClassFilled();
                }
                catch
                {
                    MyTimeTable.EndFailTansationDb();
                    Lbl_GridSubjectsError.Text = "Error In Saving";
                    Show_SelectedSubjectsPopup();
                }
            }
            else
            {
                Lbl_GridSubjectsError.Text = msg;
                Show_SelectedSubjectsPopup();
            }
           
        }

        private void AssignClassToStaff_WithSubject(string _StaffId, string _ClassId, string _SubjectId)
        {
            string sql = "INSERT INTO tblclassstaffmap(ClassId,SubjectId,StaffId) VALUES (" + _ClassId + "," + _SubjectId + ", " + _StaffId + ")";
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }

        private void RemoveClassFromStaff_WithSubject(string _StaffId, string _ClassId, string _SubjectId)
        {
            string sql = "DELETE FROM tblclassstaffmap WHERE ClassId=" + _ClassId + " and SubjectId=" + _SubjectId + " and StaffId=" + _StaffId;
            if (MyTimeTable.m_TransationDb != null)
            {
                MyTimeTable.m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            }
        }

        private void Update_ClassSubjectStaff_Entry(string ClassSubId, string StaffId, string PeriodCount)
        {
            string sql = "update tbltime_classsubjectstaff set PeriodCount=" + PeriodCount + "  where  ClassSubjectId=" + ClassSubId + " and StaffId=" + StaffId;
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }

        private void InsertClassSubjectStaff_Entry(string ClassSubId, string StaffId, string PeriodCount)
        {
            string sql = "insert into  tbltime_classsubjectstaff (ClassSubjectId,StaffId,PeriodCount) values(" + ClassSubId + " , " + StaffId + " ," + PeriodCount + ")";
            MyTimeTable.m_TransationDb.ExecuteQuery(sql);
        }



        private bool SavePossible(out string msg)
        {
            msg = "";
            bool valid = true;
            string ClassSubId = "", SubjectId = "", StaffId = "", PerodCount = "",NewStaffId="";
            int NewPeriodCount=0,TotalPeriodCount=0;
            foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
            {
                ClassSubId = gv.Cells[0].Text.ToString();
                SubjectId = gv.Cells[1].Text.ToString();
                StaffId = gv.Cells[2].Text.ToString();
                PerodCount = gv.Cells[3].Text.ToString();
                DropDownList DrpStaff = (DropDownList)gv.FindControl("Drp_GridStaff");
                TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                NewStaffId = DrpStaff.SelectedValue;
                NewPeriodCount = int.Parse( Txt_PeriodCount.Text);
                if (NewPeriodCount <= 0)
                {
                    valid = false;
                    msg = "Period count should not be less than 1";
                    break;
                }
                if (NewPeriodCount != int.Parse(PerodCount))
                {
                    if (NewStaffId == "-1")
                    {
                        valid = false;
                        msg = "Select Staff for Subject " + gv.Cells[4].Text.ToString();
                        break;
                    }
                }
                TotalPeriodCount = TotalPeriodCount + NewPeriodCount;

            }

            if (valid)
            {
                if (TotalPeriodCount > int.Parse(Txt_MaxPeriods.Value))
                {
                    valid = false;
                    msg = "Number of allotted periods should not be greater than maximum possible";
                }
            }

            return valid;
        }


        protected void Btn_AddSubject_Click(object sender, EventArgs e)
        {
            string Msg="";
            Lbl_AddSubjectError.Text = "";
            
            if (AdditionPossible(out Msg))
            {
                int id = 0;
                string[] Periods = GetAllExistingPeriodsNo_ToArray();
                MyClassMang.AddSujectToClass(int.Parse(Drp_AddSubjects.SelectedValue), int.Parse(HdnSelectedClassId.Value));
                Load_Subjects_SelectedClass(HdnSelectedClassId.Value);
                foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
                {
                    TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                    if (Periods.Length > id)
                    {
                        Txt_PeriodCount.Text = Periods[id];
                        id++;
                    }
                    else
                    {
                        break;
                    }
                }
                Show_SelectedSubjectsPopup();
            }
            else
            {
                Lbl_AddSubjectError.Text = Msg;
                MPE_AddSubject.Show();
            }
        }

        private string[] GetAllExistingPeriodsNo_ToArray()
        {
            string[] PeriodCount = new string [Grd_SelectedSubjects.Rows.Count];
            int id = 0;
            foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
            {
                TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                PeriodCount[id] = Txt_PeriodCount.Text;
                id++;
            }
            return PeriodCount;
        }

        



        protected void Btn_AddSubject_Close_Click(object sender, EventArgs e)
        {
            Show_SelectedSubjectsPopup();
        }

        protected void Btn_SelectedSubjects_Close_Click(object sender, EventArgs e)
        {
            MPE_SelectedSubjects.Hide();
        }


        private void Show_SelectedSubjectsPopup()
        {

            Load_SubjectConfiguration();
            MPE_SelectedSubjects.Show();
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "PeriodCount();", true);
          
        }

        private bool AdditionPossible(out string Msg)
        {
            bool Valid = true;
            Msg = "";
            int MaxSubjectsCount = 2,count=0;
            if (Drp_AddSubjects.SelectedValue == "-1")
            {
                Valid = false;
                Msg = "Please select subject for addition";
            }
            else
            {
                foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
                {

                    string SubjectId = gv.Cells[1].Text.ToString();
                    if (SubjectId == Drp_AddSubjects.SelectedValue)
                    {
                        count++;
                    }

                }

                if (count >= MaxSubjectsCount)
                {
                    Valid = false;
                    Msg = "One Subject should not be taken more than " + MaxSubjectsCount + " times for a particular class";
                }
            }

            return Valid;
        }

       

        protected void Lnk_AddSubject_Click(object sender, EventArgs e)
        {
            Lbl_AddSubjectError.Text = "";
            Load_Drp_AddSubject();
            MPE_AddSubject.Show();
        }


        protected void Img_AddSubject_Click1(object sender, ImageClickEventArgs e)
        {
            Lbl_AddSubjectError.Text = "";
            Load_Drp_AddSubject();
            MPE_AddSubject.Show();
        }

      
        private void Load_Drp_AddSubject()
        {
            Drp_AddSubjects.Items.Clear();
            string sql = "SELECT tblsubjects.Id,tblsubjects.subject_name FROM tblsubjects ORDER BY tblsubjects.subject_name";
            MyReader = MyTimeTable.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_AddSubjects.Items.Add(new ListItem("Select Subject", "-1"));
                while (MyReader.Read())
                {
                    Drp_AddSubjects.Items.Add(new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString()));
                }
                Drp_AddSubjects.SelectedIndex = 0;
            }
            else
            {
                Drp_AddSubjects.Items.Add(new ListItem("No Subject Found", "-1"));
            }
        }

        protected void Grd_SelectedSubjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ClassSubId = int.Parse(Grd_SelectedSubjects.DataKeys[e.RowIndex].Values["ClassSubId"].ToString());
            int SubjectId = int.Parse(Grd_SelectedSubjects.DataKeys[e.RowIndex].Values["SubjectId"].ToString());
            int StaffId = int.Parse(Grd_SelectedSubjects.DataKeys[e.RowIndex].Values["StaffId"].ToString());
            int id = 0;
            string[] Periods = GetAllExistingPeriodsNo_ToArray();


           
              
            MyTimeTable.DeleteClassSubjectStaff_ParticularStaff(ClassSubId, StaffId);

            MyTimeTable.RemoveClassSubjectMap(ClassSubId, SubjectId, int.Parse(HdnSelectedClassId.Value));

            RemoveClassFromStaff_WithSubject(StaffId.ToString(), HdnSelectedClassId.Value, SubjectId.ToString());
            Load_Subjects_SelectedClass(HdnSelectedClassId.Value);


            foreach (GridViewRow gv in Grd_SelectedSubjects.Rows)
            {
                TextBox Txt_PeriodCount = (TextBox)gv.FindControl("Txt_GridPeriodCount");
                if (id == e.RowIndex)
                {
                    id++;
                }

                    Txt_PeriodCount.Text = Periods[id];
                    id++;
                
            }
            Show_SelectedSubjectsPopup();
            Load_ClassFilled();
        }

      


        # endregion

             

        private void StaffPeriodAllocation()
        {
            string PreSubject = "0";
            DataSet StaffAllocation = new DataSet();
            DataTable Dt;
            StaffAllocation.Tables.Add(new DataTable("ClassStafflist"));
            StaffAllocation.Tables.Add(new DataTable("AllStaffList"));
            StaffAllocation.Tables.Add(new DataTable("SelectedStaffList"));
            Dt = StaffAllocation.Tables["SelectedStaffList"];
            Dt.Columns.Add("StaffId");
            Dt.Columns.Add("AllocationCount", typeof(int));
            Dt.Columns.Add("TeachermaxPeriod");
            Dt.Columns.Add("AllocatedPeriods", typeof(int));
            FillSubjectTableAndAllStaffTable(ref  StaffAllocation); // Fills AllSubjects And Staffs

            foreach (DataRow Dr_Subject in StaffAllocation.Tables[0].Rows)
            {
                if (Dr_Subject[0].ToString() == "0") //if not already alloacted
                {
                    if (PreSubject != Dr_Subject[1].ToString()) // if not the same subject then filling table 3 again
                    {
                        PreSubject = Dr_Subject[1].ToString();
                        StaffAllocation.Tables[2].Clear();
                        FillSpecificStaffForSubject(ref StaffAllocation, PreSubject); // Fills Staff for Specific Subjects Dr_Subject
                    }
                    AllocateStaffsForSubjects(ref StaffAllocation, Dr_Subject);
                    SortAllStaff(ref StaffAllocation);
                    SortSpecStaff(ref StaffAllocation);
                }
            }
            UpDateStaffClassSubjectTable(ref StaffAllocation);


            //Grd_T1.DataSource = null;
            //Grd_T1.DataBind();
            //Grd_T1.DataSource = StaffAllocation.Tables[0];
            //Grd_T1.DataBind();

            //Grd_T2.DataSource = null;
            //Grd_T2.DataBind();
            //Grd_T2.DataSource = StaffAllocation.Tables[1];
            //Grd_T2.DataBind();

            //GrdSpecStaff.DataSource = null;
            //GrdSpecStaff.DataBind();
            //GrdSpecStaff.DataSource = StaffAllocation.Tables[2];
            //GrdSpecStaff.DataBind();
        }


        private void SortAllStaff(ref DataSet StaffAllocation)
        {
            DataView dataView = new DataView(StaffAllocation.Tables[1]);
            dataView.Sort = "AllocationCount,AllocatedPeriods" + " " + "ASC";
            DataTable dt = StaffAllocation.Tables[1];
            dt = dataView.ToTable();

            // StaffAllocation.Tables[1].Clear();
            // StaffAllocation.Tables.RemoveAt(1);
            // StaffAllocation.Tables.Add(dt);
        }

        private void SortSpecStaff(ref DataSet StaffAllocation)
        {
            DataView dataView = new DataView(StaffAllocation.Tables[2]);
            dataView.Sort = "AllocationCount,AllocatedPeriods" + " " + "ASC";
            DataTable dt = StaffAllocation.Tables[2];
            dt = dataView.ToTable();

            // StaffAllocation.Tables[2].Clear();
            //// StaffAllocation.Tables.RemoveAt(2);
            // StaffAllocation.Tables.Add(dt);
        }

        private void AllocateStaffsForSubjects(ref DataSet StaffAllocation, DataRow Dr_Subject)
        {
            int TeacherMax = 0;
            bool valid;
            int AltPeriod = 0;
            DataRow Dr_SpecStaff;
            bool Allocation = false;
            int RemainingStaffPeriods = 0;
            int Count = 0;
            int _MinSubPeriod = MyTimeTable.GetMinPeriod(int.Parse(Dr_Subject["SubId"].ToString()));
            for (int i = 0; i < StaffAllocation.Tables["SelectedStaffList"].Rows.Count; i++)
            {
                Dr_SpecStaff = StaffAllocation.Tables["SelectedStaffList"].Rows[i];
                valid = int.TryParse(Dr_SpecStaff["TeachermaxPeriod"].ToString(), out TeacherMax);
                valid = int.TryParse(Dr_SpecStaff["AllocatedPeriods"].ToString(), out AltPeriod);
                //Loop:
                if ((TeacherMax - AltPeriod) >= _MinSubPeriod)// if within Teacher Period limit...   
                {
                    MyTimeTable.UpdateMainTable(ref Dr_SpecStaff, ref StaffAllocation, ref Dr_Subject);
                    MyTimeTable.UpdateStaffAndSpecificStaff(ref Dr_SpecStaff, ref StaffAllocation, Dr_Subject[3].ToString());
                    Allocation = true;
                    break;
                }
                //else
                //{
                //    _MaxSubPeriod--;
                //    goto Loop;
                //}
            }
            if (!Allocation)  // No teacher is available with maximum periods
            {

                SortSpecStaff(ref StaffAllocation);// sorting to get the staff with more free periods
                for (int i = 0; i < StaffAllocation.Tables["SelectedStaffList"].Rows.Count; i++)
                {
                    if (Count < 2)  // Only Staff1 and Staff2 are allowed
                    {
                        Dr_SpecStaff = StaffAllocation.Tables["SelectedStaffList"].Rows[i];
                        valid = int.TryParse(Dr_SpecStaff["TeachermaxPeriod"].ToString(), out TeacherMax);
                        valid = int.TryParse(Dr_SpecStaff["AllocatedPeriods"].ToString(), out AltPeriod);
                        RemainingStaffPeriods = TeacherMax - AltPeriod;
                        if (RemainingStaffPeriods > _MinSubPeriod) // if staffs remaining period is greater than subject maximum then making it same as subject maximum
                        {
                            RemainingStaffPeriods = MyTimeTable.SetNumberofPeriods(RemainingStaffPeriods, _MinSubPeriod);
                        }
                        if ((Count == 0) && (RemainingStaffPeriods > 0))
                        {
                            MyTimeTable.UpdateMainTable(Dr_Subject, Dr_SpecStaff[0].ToString(), RemainingStaffPeriods, 1);
                            MyTimeTable.UpdateStaffAndSpecificStaff(ref Dr_SpecStaff, ref StaffAllocation, RemainingStaffPeriods.ToString());
                        }
                        Allocation = true;
                        Count++;
                    }
                }
            }
        }



        private void FillSubjectTableAndAllStaffTable(ref DataSet StaffAllocation)
        {
            DataSet MyClass = new DataSet();
            DataSet MySubjects = new DataSet();
            DataSet MyStaff = new DataSet();
            //  DataSet StaffAllocation = new DataSet();
            DataRow Dr;
            DataTable Dt;
            string Staff2 = "";
            int ClassSubId = 0;

            Dt = StaffAllocation.Tables["ClassStafflist"];
            Dt.Columns.Add("Flag");
            Dt.Columns.Add("SubId");
            Dt.Columns.Add("ClassId");
            Dt.Columns.Add("Period");
            Dt.Columns.Add("Staff1");
            Dt.Columns.Add("Staff2");
            Dt.Columns.Add("Period1");
            Dt.Columns.Add("Period2");

            //Loading Subjects
            MySubjects = MyTimeTable.GetAllSubjects();
            MyStaff = MyTimeTable.GetStaffs();
            if (MySubjects != null && MySubjects.Tables != null && MySubjects.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Subject in MySubjects.Tables[0].Rows)
                {
                    //Loading Classes for the specific subject
                    MyClass = MyTimeTable.GetAllClassForSubject(Dr_Subject[0].ToString());
                    if (MyClass != null && MyClass.Tables != null && MyClass.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr_Class in MyClass.Tables[0].Rows)
                        {
                            Dr = StaffAllocation.Tables["ClassStafflist"].NewRow();
                            Dr["Flag"] = MyTimeTable.GetAllotedStatus(Dr_Subject[0].ToString(), Dr_Class[0].ToString(), out ClassSubId);
                            Dr["SubId"] = Dr_Subject[0].ToString();
                            Dr["ClassId"] = Dr_Class[0].ToString();
                            Dr["Period"] = MyTimeTable.GetMinPeriod(int.Parse(Dr_Subject[0].ToString()));
                            Dr["Staff1"] = MyTimeTable.CLassSubjectStaff(ClassSubId, out Staff2);
                            Dr["Staff2"] = Staff2;
                            Dr["Period1"] = MyTimeTable.GetAllotedPeriodForStaff(ClassSubId, Dr["Staff1"].ToString());
                            Dr["Period2"] = MyTimeTable.GetAllotedPeriodForStaff(ClassSubId, Staff2);
                            StaffAllocation.Tables["ClassStafflist"].Rows.Add(Dr);
                        }
                    }
                }
            }


            Dt = StaffAllocation.Tables["AllStaffList"];
            Dt.Columns.Add("StaffId");
            Dt.Columns.Add("AllocationCount", typeof(int));
            Dt.Columns.Add("TeachermaxPeriod");
            Dt.Columns.Add("AllocatedPeriods", typeof(int));

            if (MyStaff != null && MyStaff.Tables != null && MyStaff.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Dr_Staff in MyStaff.Tables[0].Rows)
                {
                    Dr = StaffAllocation.Tables["AllStaffList"].NewRow();
                    Dr["StaffId"] = Dr_Staff[0].ToString();
                    Dr["AllocationCount"] = int.Parse(MyTimeTable.GetAllocationCount(Dr_Staff[0].ToString()));
                    Dr["TeachermaxPeriod"] = MyTimeTable.GetTeacherMaxPeriod();
                    Dr["AllocatedPeriods"] = int.Parse(MyTimeTable.GetAllocatedPeriods(Dr_Staff[0].ToString()));
                    StaffAllocation.Tables["AllStaffList"].Rows.Add(Dr);
                }
            }

        }

        private void FillSpecificStaffForSubject(ref DataSet _StaffAllocation, string SubjectId)
        {
            DataRow Dr;
            foreach (DataRow Dr_Staff in _StaffAllocation.Tables[1].Rows)
            {
                if (MyTimeTable.SubjectHandlesbyStaff(SubjectId, Dr_Staff[0].ToString()))//Teacher Teaches the subject Then storing into an array
                {
                    Dr = _StaffAllocation.Tables["SelectedStaffList"].NewRow();
                    Dr["StaffId"] = Dr_Staff[0].ToString();
                    Dr["AllocationCount"] = int.Parse(Dr_Staff[1].ToString());
                    Dr["TeachermaxPeriod"] = Dr_Staff[2].ToString();
                    Dr["AllocatedPeriods"] = int.Parse(Dr_Staff[3].ToString());
                    _StaffAllocation.Tables["SelectedStaffList"].Rows.Add(Dr);
                }
            }
            DataView dataView = new DataView(_StaffAllocation.Tables[1]);
            dataView.Sort = "AllocationCount,AllocatedPeriods" + " " + "ASC";
            DataTable dt = _StaffAllocation.Tables[1];
            dt = dataView.ToTable();


            // _StaffAllocation.Tables[1].Clear();
            //_StaffAllocation.Tables.RemoveAt(1);
            //_StaffAllocation.Tables.Add(dt);
        }

        private void UpDateStaffClassSubjectTable(ref DataSet StaffAllocation)
        {
            int ClassSubMap = 0;
            foreach (DataRow Dr_Subject in StaffAllocation.Tables[0].Rows)
            {
                if (Dr_Subject[0].ToString() == "0") //if not already alloacted
                {

                    ClassSubMap = MyTimeTable.GetClassSubjectMap(Dr_Subject[1].ToString(), Dr_Subject[2].ToString());
                    if (Dr_Subject[4].ToString() != "")
                    {
                        MyTimeTable.AssignClassToStaff(Dr_Subject[4].ToString(), Dr_Subject[2].ToString());
                        MyTimeTable.InsertClassSubjectStaff(int.Parse(Dr_Subject[6].ToString()), ClassSubMap, int.Parse(Dr_Subject[4].ToString()));
                    }
                    if (Dr_Subject[5].ToString() != "")
                        MyTimeTable.InsertClassSubjectStaff(int.Parse(Dr_Subject[6].ToString()), ClassSubMap, int.Parse(Dr_Subject[5].ToString()));
                }
            }
        }

  

        //# endregion

        # region TimeTableGen

        protected void Img_Verify_Click(object sender, EventArgs e)
        {
            //DataSet TimeTable = new DataSet();
            //DataTable dt;
            //DataSet MyClass = MyTimeTable.GetMyClass();
            //TimeTable.Tables.Add(new DataTable("TimeMaster"));
            //dt = TimeTable.Tables["TimeMaster"];
            //dt.Columns.Add("Alloted");
            //dt.Columns.Add("ClassPeriodId");
            //dt.Columns.Add("Period");
            //dt.Columns.Add("Day");
            //dt.Columns.Add("Class");
            //dt.Columns.Add("Staff");
            //dt.Columns.Add("Subject");
            //dt.Columns.Add("NxtBrk");
            //MyTimeTable.FillTimeTableGenEntry(ref TimeTable);

            //if (MyTimeTable.FirstPeriodClassteacher())
            //{
            //    MyTimeTable.FillClassTeacher(ref TimeTable, MyClass);
            //}

            //MyTimeTable.FillAdjacentPeriodMoreSubjects(ref TimeTable, MyClass);
            //MyTimeTable.FillCombinedSubjects(ref TimeTable, MyClass);

            //Grd_T2.DataSource = TimeTable;
            //Grd_T2.DataBind();
            //GrdSpecStaff.DataSource = TimeTable.Tables[1];
            //GrdSpecStaff.DataBind();
            //MyTimeTable.SetClassTeacherPeriod();  
           

            
        }

        
        # endregion


        # region NextButtonClick

        protected void TimeTable_OnNextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.CurrentStepIndex == 0)//GENERAL RULES SAVE
            {
                int ClassTeacher = 0;
              
                if (Chk_FirsrClassT.Checked)
                {
                    ClassTeacher = 1;
                }
                if (Chk_DeleteAll.Checked)
                {

                    MyTimeTable.DeleteTimeTableMaster();
                    MyTimeTable.DeleteAllClassSubjectStaff();
                }
                LoadClassSettings();

                MyTimeTable.SaveGenRules(Txt_PeridDay.Text.Trim(), ClassTeacher, Txt_MaxConsecutive.Text.Trim(), Txt_MaxteacherSub.Text.Trim(), Txt_MaxStaffWeekperiod.Text.Trim(), Txt_MaxStaffDayperiod.Text.Trim());

                    
            }

            if (e.CurrentStepIndex == 1)//SCHOOL CONFIGURATIONS SAVE
            {
                Load_SubjectConfiguration();
            }
            if (e.CurrentStepIndex == 2)
            {
               
            }

            if (e.CurrentStepIndex == 3)
            {
                Load_FinalClassDrp();
                WC_TimeTableControl.LoadTimeTableControl( Drp_WzdStep7Class.SelectedValue);
            }
        }
        # endregion



        # region ManualFix


        private void Load_FinalClassDrp()
        {
            Drp_WzdStep7Class.Items.Clear();
            MydataSet = MyTimeTable.GetAllClassList(1);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());

                    Drp_WzdStep7Class.Items.Add(li);


                }
            }
            else
            {
                ListItem li = new ListItem("No Class Configured", "-1");

                Drp_WzdStep7Class.Items.Add(li);

            }
            Drp_WzdStep7Class.SelectedIndex = 0;

        }


        protected void Drp_WzdStep7Class_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            WC_TimeTableControl.LoadTimeTableControl(Drp_WzdStep7Class.SelectedValue);            
        }
        

        # endregion


        protected void TimeTable_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            Mpe_Finishpopup.Show();
        }

        protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {

            if (e.CurrentStepIndex == 1)//GENERAL RULES SAVE
            {

            }

            else if (e.CurrentStepIndex == 2)//SCHOOL CONFIGURATIONS SAVE
            {
               
            }
            else if (e.CurrentStepIndex == 3)
            {
                Load_SubjectConfiguration();
              // // LoadSubjects();
            }
            else if (e.CurrentStepIndex == 4)
            {
                Load_FinalClassDrp();
            }

        }

       
        protected void Btn_AutoFix_Click(object sender, EventArgs e)
        {

            string _Msg;
            //General Period allocation
            StaffPeriodAllocation();
            if (MyTimeTable.ValidateInputData(out _Msg) && MyTimeTable.GenerateTimtable(out _Msg))
            {
                Lbl_msg.Text = _Msg;
                this.MPE_MessageBox.Show();
                WC_TimeTableControl.LoadTimeTableControl(Drp_WzdStep7Class.SelectedValue);       
            }
            else
            {
                Lbl_msg.Text = _Msg;
                this.MPE_MessageBox.Show();
            }

        }



        protected void Btn_FinishWizard_Click(object sender, EventArgs e)
        {
            Response.Redirect("TimeTableHome.aspx");
        }

        protected void Lnk_StaffWorkLoad_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScript", "window.open('StaffWorkLoadReport.aspx', 'Info');", true);
        }

       

  

        

    }
}