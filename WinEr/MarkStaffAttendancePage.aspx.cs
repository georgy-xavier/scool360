using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinBase;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class MarkStaffAttendancePage : System.Web.UI.Page
    {

        private Attendance MyAttendance;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyAttendance = MyUser.GetAttendancetObj();
            if (MyAttendance == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }


            else
            {
                string _msg = "";
                bool _Valid = true;
                DateTime M_StartTime = new DateTime();

                # region Validation


                if (Request.QueryString["Date"] == null)
                {
                    _msg = "Invalid Start Time";
                    _Valid = false;
                }
                else
                {
                    if (!DateTime.TryParse(Request.QueryString["Date"].ToString(), out M_StartTime))
                    {
                        _msg = "Invalid Start Time";
                        _Valid = false;
                    }
                    else
                    {
                        Session["StaffDate"] = M_StartTime;
                    }
                }


                # endregion


                if (_Valid)
                {
                    if (!IsPostBack)
                    {
                        try
                        {

                            if (M_StartTime <= DateTime.Today)
                            {

                                if (_Valid)
                                {

                                    Load_TopDetails(M_StartTime);
                                    LadStaffDetails();
                                    Btn_DeleteMarking.Visible = false;
                                }
                            }
                        }
                        catch
                        {
                            Lbl_msgAlert.Text = "Error In Connection. Try Again";
                            MPE_MessageBox.Show();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pageresize();", true);
                    Lbl_msgAlert.Text = _msg;
                    MPE_MessageBox.Show();
                }
            }

        }

        private void LadStaffDetails()
        {
            lbl_msg.Text = "";
            MydataSet = GetStaffDataSet();
            if (MydataSet != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Staff.Columns[0].Visible = true;
                Grd_Staff.Columns[1].Visible = true;
                Grd_Staff.Columns[2].Visible = true;
                Grd_Staff.Columns[3].Visible = true;
                Grd_Staff.DataSource = MydataSet;
                Grd_Staff.DataBind();
                Grd_Staff.Columns[0].Visible = false;
                Grd_Staff.Columns[1].Visible = false;
                Grd_Staff.Columns[2].Visible = false;
                Grd_Staff.Columns[3].Visible = false;
                FillPresentStatus();
            }
            else
            {
                lbl_msg.Text = "No Staff Found";
                Grd_Staff.DataSource = null;
                Grd_Staff.DataBind();
            }
        }

        private void FillPresentStatus()
        {
            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                TextBox Txt_In = (TextBox)gv.FindControl("Txt_InTime");
                TextBox Txt_Out= (TextBox)gv.FindControl("Txt_OutTime");

                dr.Items.Clear();
                dr.Items.Add(new ListItem("Absent", "0"));
                dr.Items.Add(new ListItem("ForeNoon", "1"));
                dr.Items.Add(new ListItem("AfterNoon", "2"));
                dr.Items.Add(new ListItem("Full Day", "3"));
                dr.SelectedValue = gv.Cells[1].Text;
                Txt_In.Text = gv.Cells[2].Text;
                Txt_Out.Text = gv.Cells[3].Text;

                if (Txt_In.Text.Trim() == "" || Txt_In.Text.Trim() == "&nbsp;")
                {
                    Txt_In.Text = "00:00:00";
                }
                if (Txt_Out.Text.Trim() == "" || Txt_Out.Text.Trim() == "&nbsp;")
                {
                    Txt_Out.Text = "00:00:00";
                }
            }
        }

        private DataSet GetStaffDataSet()
        {
            DateTime _NewDate=General.GetDateTimeFromText(HiddenDate.Value);
            DataSet Staffdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            Staffdataset.Tables.Add(new DataTable("Reqtbl"));
            dt = Staffdataset.Tables["Reqtbl"];
            dt.Columns.Add("Id");
            dt.Columns.Add("PresentStatus");
            dt.Columns.Add("SurName");
            dt.Columns.Add("InTime");
            dt.Columns.Add("OutTime");

            string sql = "SELECT DISTINCT t.`Id`,t.`SurName`  FROM tbluser t  inner join tblrole r on t.`RoleId`=r.`Id` inner join tblgroupusermap g on t.`Id`=g.`UserId`  where t.`Status`=1 AND r.`Type`='Staff' AND g.`GroupId` IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    string Intime = "";
                    string OutTime = "";
                    string PresentHours = "";
                    dr = Staffdataset.Tables["Reqtbl"].NewRow();
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    dr["PresentStatus"] = MyAttendance.GetStaffAttedanceStatus_OneStaff(MyReader.GetValue(0).ToString(), _NewDate, out Intime, out OutTime, out PresentHours);
                    dr["SurName"] = MyReader.GetValue(1).ToString();
                    dr["InTime"] = Intime;
                    dr["OutTime"] = OutTime;
                    Staffdataset.Tables["Reqtbl"].Rows.Add(dr);
                }
            }



            return Staffdataset;
           
        }

        private void Load_TopDetails(DateTime M_StartTime)
        {
            lbl_Date.Text = MyUser.GerFormatedDatVal(M_StartTime);
            HiddenDate.Value = MyUser.GerFormatedDatVal(M_StartTime);
        }






        protected void Btn_DeleteMarking_Click(object sender, EventArgs e)
        {

            try
            {
                string sql = "DELETE FROM tblstaffattendance WHERE MarkDate='" + General.GetDateTimeFromText(HiddenDate.Value).ToString("s") + "'";
                MyAttendance.m_MysqlDb.ExecuteQuery(sql);
                MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Staff Attendance", "" + General.GetDateTimeFromText(HiddenDate.Value).ToString("s") + " Attendance deleted", 1,1);
                LadStaffDetails();

                lbl_msg.Text = "Attendance has been deleted successfully";
                
            }
            catch(Exception ex)
            {
                lbl_msg.Text = "Error while deletion. Please try later. Error : " + ex.Message;
            }
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            lbl_msg.Text = "";
            string _msg = "";
            if (IsUpdationPossible(out _msg))
            {
                try
                {
                    MyAttendance.CreateTansationDb();
                    foreach (GridViewRow gv in Grd_Staff.Rows)
                    {
                        DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                        TextBox Txt_In = (TextBox)gv.FindControl("Txt_InTime");
                        TextBox Txt_Out = (TextBox)gv.FindControl("Txt_OutTime");
                        

                        if (dr.SelectedValue == "0")
                        {
                            Txt_In.Text = "00:00:00";
                            Txt_Out.Text = "00:00:00";
                        }
                        string _LateValue = "00:00:00";
                        string _IsLate = "0";

                        TimeSpan _Time = TimeSpan.Parse(Txt_In.Text);
                        TimeSpan _LateIntime = TimeSpan.Parse(MyAttendance.GetStaffAttend_ConfigValue("Late InTime"));
                        if (_Time > _LateIntime)
                        {
                            TimeSpan LateValue = _Time - _LateIntime;
                            _IsLate = "1";
                            _LateValue = LateValue.ToString();
                        }

                        MyAttendance.SaveStaffAttendance(gv.Cells[0].Text, General.GetDateTimeFromText(HiddenDate.Value), dr.SelectedValue, Txt_In.Text, Txt_Out.Text, _IsLate, _LateValue);

                    }

                    
                    MyAttendance.EndSucessTansationDb();
                    LadStaffDetails();
                    MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Staff Attendance", "" + General.GetDateTimeFromText(HiddenDate.Value).ToString("s") + " Attendance updated", 1,1);
                    lbl_msg.Text = "Successfully updated";
                    Btn_markall.Visible = false;
                    Btn_DeleteMarking.Visible = true;
                }
                catch
                {
                    MyAttendance.EndFailTansationDb();
                    lbl_msg.Text = "Error while updating";

                }
            }
            else
            {
                lbl_msg.Text = _msg;
            }
            
        }

        private bool IsUpdationPossible(out string _msg)
        {
            bool valid = true,_change=false;
            _msg = "";

            foreach (GridViewRow gv in Grd_Staff.Rows)
            {
                DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                TextBox Txt_In = (TextBox)gv.FindControl("Txt_InTime");
                TextBox Txt_Out = (TextBox)gv.FindControl("Txt_OutTime");

                if (dr.SelectedValue != "0")
                {
                    if (Txt_In.Text.Trim() == "")
                    {
                        valid = false;
                        _msg = "Enter in time";
                        break;

                    }
                    else if (Txt_Out.Text.Trim() == "")
                    {
                        valid = false;
                        _msg = "Enter out time";
                         break;

                    }
                    else
                    {
                        TimeSpan InTime;
                        TimeSpan OutTime;
                        TimeSpan.TryParse(Txt_In.Text, out InTime);
                        TimeSpan.TryParse(Txt_Out.Text, out OutTime);

                        if (InTime > OutTime)
                        {
                            valid = false;
                            _msg = "Enter greater out time for " + gv.Cells[4].Text;
                            break;
                        }
                        else if (InTime == OutTime)
                        {
                            valid = false;
                            _msg = "Enter correct In time and Out time for " + gv.Cells[4].Text;
                            break;
                        }

                    }

                    if (dr.SelectedValue != gv.Cells[1].Text)
                    {
                        _change = true;
                    }
                    else if (Txt_In.Text != gv.Cells[2].Text)
                    {
                        _change = true;
                    }
                    else if (Txt_Out.Text != gv.Cells[3].Text)
                    {
                        _change = true;
                    }
                }

            }

            //if (valid)
            //{
            //    if (!_change)
            //    {
            //        valid = false;
            //        _msg = "No changes to update";
            //    }
            //}

            return valid;
        }
        //sai added code for mark all staff with full day attendance

        protected void Btn_markall_Click(object sender, EventArgs e)
        {
            string In_Time = "";
            string Out_Time = "";
            try
            {
                Get_Staff_Timings(out In_Time, out Out_Time);
                foreach (GridViewRow gv in Grd_Staff.Rows)
                {
                    DropDownList dr = (DropDownList)gv.FindControl("Drp_GridStatus");
                    TextBox Txt_In = (TextBox)gv.FindControl("Txt_InTime");
                    TextBox Txt_Out = (TextBox)gv.FindControl("Txt_OutTime");
                    Txt_In.Text = In_Time;
                    Txt_Out.Text = Out_Time;
                    dr.SelectedIndex = 3;


                }
            }
            catch (Exception ew)
            {
                lbl_msg.Text = ew.Message;
            }

        }
        private void Get_Staff_Timings(out string In_Time, out string Out_Time)
        {
            In_Time="00:00:00";
            Out_Time = "00:00:00";
            string sql = "select Value,SubValue from tblconfiguration where Module='MarkAttendance' and Name='Staff Attendance Timings'";
            MyReader = MyAttendance.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    In_Time = MyReader.GetValue(0).ToString();
                    Out_Time = MyReader.GetValue(1).ToString();
                }
            }
        }
    }
}
