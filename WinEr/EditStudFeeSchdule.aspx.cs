using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;
namespace WinEr
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private int MasterBatchId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (Session["FeeId"] == null)
            {
                Response.Redirect("ManageFeeAccount.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(43))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (Rdo_Batch.SelectedValue == "0")
                    MasterBatchId = MyUser.CurrentBatchId;
                else
                    MasterBatchId = MyUser.CurrentBatchId + 1;
               
                if (!IsPostBack)
                {
                    string _MenuStr;
                    //_MenuStr = MyFeeMang.GetFeeMangMenuString(MyUser.UserRoleId);
                    //this.FeeMenu.InnerHtml = _MenuStr;    
                   
                    _MenuStr = MyFeeMang.GetSubFeeMangMenuString(MyUser.UserRoleId, int.Parse(Session["FeeId"].ToString()));
                    this.SubFeeMenu.InnerHtml = _MenuStr;
                    if (MyFeeMang.HasNextBatchSchedule())
                    {
                        Label_NextBatch.Visible = true;
                        Rdo_Batch.Visible = true;
                    }
                    else
                    {
                        Label_NextBatch.Visible = false;
                        Rdo_Batch.Visible = false;
                    }
                    LoadDetails();
                    AddClassToDropDownClass();
                    AddPeriodToDrp();
                    LoadScheduleData();
                    //some initlization

                }
            }


        }

        protected void Grd_Amound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = e.Row.Cells[2].Text;
                if (value == "-1")
                {
                    e.Row.Cells[2].Text = "No Roll No.";
                }
            }
        }

        private void LoadScheduleData()
        {
            if (Drp_class2.SelectedValue != "-1" && Drp_Perod2.SelectedValue != "-1")
            {
                string sql = "select tblfeeschedule.Id,tblfeeschedule.Duedate,tblfeeschedule.LastDate from tblfeeschedule where FeeId=" + int.Parse(Session["FeeId"].ToString()) + " and ClassId=" + int.Parse(Drp_class2.SelectedValue.ToString()) + " and PeriodId=" + int.Parse(Drp_Perod2.SelectedValue.ToString()) + " and BatchId=" + MasterBatchId;
                //string sql = "select tblfeeschedule.Id,tblfeeschedule.Duedate,tblfeeschedule.LastDate from tblfee inner join tblfeeschedule on tblfeeschedule.FeeId=tblfee.FeeId where  tblfee.FeeId=" + int.Parse(Session["FeeId"].ToString()) + " AND tblfeeschedule.BatchId=" + MyUser.CurrentBatchId;

                MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    MyReader.Read();
                    Txt_schduleId.Text = MyReader.GetValue(0).ToString();
                    DateTime _dt = DateTime.Parse(MyReader.GetValue(1).ToString());
                    //DateTime _dt = MyUser.GetDareFromText(MyReader.GetValue(1).ToString());

                    //Txt_DueStud.Text = _dt.Date.ToString("dd/MM/yyyy");
                    Txt_DueStud.Text = _dt.Date.Day + "/" + _dt.Date.Month + "/" + _dt.Date.Year;

                    DateTime _dt1 = DateTime.Parse(MyReader.GetValue(2).ToString());
                    //DateTime _dt1 = MyUser.GetDareFromText(MyReader.GetValue(2).ToString());

                    //Txt_LastStud.Text = _dt1.Date.ToString("dd/MM/yyyy");
                    Txt_LastStud.Text = _dt1.Date.Day + "/" + _dt1.Date.Month + "/" + _dt1.Date.Year;


                    foreach (GridViewRow gv in Grd_Amound.Rows)
                    {
                        sql = "SELECT tblfeestudent.Amount,tblfeestudent.Status from tblfeestudent WHERE tblfeestudent.StudId=" + int.Parse(gv.Cells[0].Text.ToString()) + " AND tblfeestudent.SchId=" + int.Parse(Txt_schduleId.Text.ToString());
                        MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        TextBox TxtAmount = (TextBox)gv.FindControl("Txt_Amound");
                        TxtAmount.ReadOnly = false;
                        if (MyReader.HasRows)
                        {
                            MyReader.Read();

                            
                            TxtAmount.Text = MyReader.GetValue(0).ToString();
                            TextBox Txtstatus = (TextBox)gv.FindControl("Txt_Status");
                            Txtstatus.Text = MyReader.GetValue(1).ToString();
                            if (MyReader.GetValue(1).ToString() != "Scheduled")
                            {
                                TxtAmount.ReadOnly = true;
                            }
                        }
                        else
                        {
                            TextBox Txtstatus = (TextBox)gv.FindControl("Txt_Status");
                            Txtstatus.Text = "Not Scheduled";
                        }
                      
                    }


                }
                MyReader.Close();
            }
        }
        private void AddPeriodToDrp()
        {
            Drp_Perod2.Items.Clear();
            if (Drp_class2.SelectedValue != "-1")
            {
                if (!MyFeeMang.HaveStudentInClass(int.Parse(Drp_class2.SelectedValue.ToString()), MyUser.CurrentBatchId))
                {
                    Lbl_msg.Text = "No Students in the class";
                    this.MPE_MessageBox.Show();
                    Btn_Update.Enabled = false;
                    ListItem li = new ListItem("No Scheduled Periods", "-1");
                    Drp_Perod2.Items.Add(li);
                    ClearGrid();
                }
                //else if (MyFeeMang.HaveStudWithOutRollNoInClass(int.Parse(Drp_class2.SelectedValue.ToString()), MyUser.CurrentBatchId))
                //{
                //    Lbl_msg.Text = "Please assign roll number to all students before Editing";
                //    this.MPE_MessageBox.Show();
                //    Btn_Update.Enabled = false;
                //    ListItem li = new ListItem("No Scheduled Periods", "-1");
                //    Drp_Perod2.Items.Add(li);
                //    ClearGrid();
                //}
                else
                {

                    string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod inner join tblfeeaccount on tblfeeaccount.FrequencyId = tblperiod.FrequencyId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString()) + " AND tblperiod.Id IN (SELECT  tblfeeschedule.PeriodId from tblfeeschedule where tblfeeschedule.BatchId=" + MasterBatchId + " AND tblfeeschedule.FeeId=" + int.Parse(Session["FeeId"].ToString()) + " AND tblfeeschedule.ClassId=" + int.Parse(Drp_class2.SelectedValue.ToString()) + ")";
                    MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                    {
                        while (MyReader.Read())
                        {
                            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), int.Parse(MyReader.GetValue(0).ToString()).ToString());
                            Drp_Perod2.Items.Add(li);
                        }
                        Btn_Update.Enabled = true;
                        LoadStudentsToGrid();
                    }
                    else
                    {
                        ListItem li = new ListItem("No Scheduled Periods", "-1");
                        Btn_Update.Enabled = false;
                        Drp_Perod2.Items.Add(li);
                        ClearGrid();

                    }

                }
            }
            else
            {
                ClearGrid();
                ListItem li = new ListItem("No Periods", "-1");
                Drp_Perod2.Items.Add(li);
            }
        }
        private void LoadStudentsToGrid()
        {
            Grd_Amound.Columns[0].Visible = true;
            string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudentclassmap.RollNo,tblstudent.Sex FROM tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Drp_class2.SelectedValue.ToString()) + " Order by tblstudentclassmap.RollNo ASC";
            MydataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_Amound.DataSource = MydataSet;
                Grd_Amound.DataBind();
                // Grd_SchRollNo.Columns[0].Visible = false;

                Grd_Amound.Columns[0].Visible = false;
            }

        }
        private void ClearGrid()
        {
            Grd_Amound.Columns[0].Visible = true;
            Grd_Amound.DataSource = null;
            Grd_Amound.DataBind();
        }
        private void AddClassToDropDownClass()
        {
            Drp_class2.Items.Clear();

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_class2.Items.Add(li);

                }
                Btn_Update.Enabled = true;




            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                Drp_class2.Items.Add(li);
                Btn_Update.Enabled = false;
            }

            Drp_class2.SelectedIndex = 0;

        }
        private void LoadDetails()
        {
            string sql = "SELECT tblfeeaccount.AccountName, tblfeefrequencytype.FreequencyName, tblfeeasso.Name from tblfeeaccount inner join tblfeefrequencytype on tblfeefrequencytype.Id= tblfeeaccount.FrequencyId inner join tblfeeasso on tblfeeasso.Id = tblfeeaccount.AssociatedId where tblfeeaccount.Id=" + int.Parse(Session["FeeId"].ToString());
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                MyReader.Read();
                Lbl_FeeName.Text = MyReader.GetValue(0).ToString();
                Lbl_Freq.Text = MyReader.GetValue(1).ToString();
                Lbl_asso.Text = MyReader.GetValue(2).ToString();

            }
            MyReader.Close();
        }
        protected void Drp_class2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();
            LoadScheduleData();
        }
        private void ClearDetails()
        {
            foreach (GridViewRow gv in Grd_Amound.Rows)
            {

                TextBox TxtAmount = (TextBox)gv.FindControl("Txt_Amound");
                TxtAmount.Text = "";

            }

            Txt_DueStud.Text = "";
            Txt_LastStud.Text = "";
        }

        protected void Drp_Perod2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDetails();
            LoadScheduleData();
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            if (Txt_DueStud.Text.Trim() == "" || Txt_LastStud.Text.Trim() == "")
            {
                Lbl_msg.Text = "One or more fields are Empty";
            }
            else
            {
                //DateTime _duedate = DateTime.Parse(Txt_DueStud.Text.ToString());
                //DateTime _lastdate = DateTime.Parse(Txt_LastStud.Text.ToString());
                DateTime _duedate = MyUser.GetDareFromText(Txt_DueStud.Text.ToString());
                DateTime _lastdate = MyUser.GetDareFromText(Txt_LastStud.Text.ToString());
                if (_lastdate < _duedate)
                {
                    Lbl_msg.Text = "Last date cannot be less than due date";
                }
                else
                {
                    if (MyFeeMang.UpdateStudFeeSchedule(_duedate, _lastdate, int.Parse(Txt_schduleId.Text.ToString())))
                    {
                        foreach (GridViewRow gv in Grd_Amound.Rows)
                        {

                            TextBox TxtAmount = (TextBox)gv.FindControl("Txt_Amound");
                            TextBox Txtstatus = (TextBox)gv.FindControl("Txt_Status");
                            if (TxtAmount.Text != "" && !TxtAmount.ReadOnly)
                            {
                                MyFeeMang.UpdateScheduleStudFeeAmount(double.Parse(TxtAmount.Text.ToString()), int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Txt_schduleId.Text.ToString()));
                                
                                if(Txtstatus.Text == "Not Scheduled")
                                {
                                    Txtstatus.Text = "Scheduled";
                                }
                            }
                            else if (TxtAmount.Text.Trim() == "" && Txtstatus.Text == "Scheduled" && !TxtAmount.ReadOnly)
                            {
                                MyFeeMang.DeleteScheduleStudFeeAmount(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Txt_schduleId.Text.ToString()));

                                    Txtstatus.Text = "Not Scheduled";


                            }

                            Lbl_msg.Text = "Fee Schedule is Updated";
                        }
                        MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Updating Student Fee Schedule", Lbl_FeeName.Text + " Fee Schedule is updated ", 1,1);

                    }
                    else
                    {
                        Lbl_msg.Text = "Please try Again";
                    }
                }
            }
            this.MPE_MessageBox.Show();

        }

        protected void Btn_Cancel1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFeeAccount.aspx");

        }

        protected void Rdo_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddPeriodToDrp();

            LoadScheduleData();
        }
    }
}
