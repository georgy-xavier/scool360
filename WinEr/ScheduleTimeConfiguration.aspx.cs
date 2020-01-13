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
    public partial class ScheduleTimeConfiguration : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private OdbcDataReader m_Myreader = null;
        private FeeManage MyFeeMang;
        protected void Page_Load(object sender, EventArgs e)
        {
            WC_MISMESSAGEBOX.EVNTSave += new EventHandler(WC_MISMESSAGEBOX_UPDATE);
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            if (!IsPostBack)
            {
                Lbl_msg.Text = "";
              
                LoadCheckBoxReportName();
                LoadGridDetails();
            }

        }

        protected void WC_MISMESSAGEBOX_UPDATE(object sender, EventArgs e)
        {
            Lbl_msg.Text = "";
            Lbl_msg.Text = "Updated Sucessfully";
   
            LoadCheckBoxReportName();
            LoadGridDetails();
        }

        private void LoadCheckBoxReportName()
        {
            Chkboxlist_reportname.Items.Clear();
            string sql = "SELECT Id,ReportName from tblmisschedulereport";
            m_Myreader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            ListItem li;
            if (m_Myreader.HasRows)
            {
                while (m_Myreader.Read())
                {

                    //if (Chkboxlist_reportname.Items.Count == 0)
                    //{
                    //    li = new ListItem("All", "0");
                    //    Chkboxlist_reportname.Items.Add(li);
                    //}
                    li = new ListItem(m_Myreader.GetValue(1).ToString(), m_Myreader.GetValue(0).ToString());
                    Chkboxlist_reportname.Items.Add(li);

                }

            }
            else
            {
                li = new ListItem("Item Not Found", "0");
                Chkboxlist_reportname.Items.Add(li);
            }


        }

        protected void Btn_Chedule_Click(object sender, EventArgs e)
        {
            Pnl_scheduleGrid.Visible = false;
            string Err = "";
            Lbl_msg.Text = "";
            try
            {
                string sql = "";
                string lastactiondate = DateTime.Now.Date.ToString("s");
                string NextActionDate = DateTime.Now.Date.ToString("s");
                string Peroidtype = Radiobtn_periodtype.SelectedValue;
                int peroidvalue = -1;
                if (Validation(out Err, Peroidtype, out peroidvalue))
                {

                    sql = "SELECT MAX(ScheduleId) as MaxId  from tblmisschedule";
                    int _MaxscheduleId = 1;

                    DataSet ds = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string val = ds.Tables[0].Rows[0]["MaxId"].ToString();
                        if (val != "")
                            _MaxscheduleId = int.Parse(ds.Tables[0].Rows[0]["MaxId"].ToString()) + 1;

                    }

                    sql = "insert into tblmisschedule (ScheduleId,ScheduleName,LastactionDate,NextActionDate,EmailId,PeroidType) values (" + _MaxscheduleId + ",'" + Txt_misschedulename.Text.Trim() + "','" + lastactiondate + "','" + NextActionDate + "','" + Txt_emailaddress.Text.Trim() + "','" + peroidvalue + "')";
                    MyFeeMang.m_MysqlDb.ExecuteQuery(sql);



                    for (int i = 0; i < Chkboxlist_reportname.Items.Count; i++)
                    {
                        if (Chkboxlist_reportname.Items[i].Selected == true)
                        {
                            int reportid = int.Parse(Chkboxlist_reportname.Items[i].Value.ToString());
                            sql = "insert into tblmisschedulereportmap (ScheduleId,ReportId) values (" + _MaxscheduleId + "," + reportid + ")";
                            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

                        }
                    }
                    Radiobtn_periodtype.Items[0].Selected = true;
                    Txt_emailaddress.Text = "";
                    Txt_misschedulename.Text = "";
                    CheckBoxValidation(0);
                
                    Lbl_msg.Text = "New MIS Schedule successfully configured ";
                    LoadGridDetails();



                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
            if (Err != "")
            {
                WC_MessageBox.ShowMssage(Err);
                Lbl_msg.Text = "";
                
            }

        }

        private void LoadGridDetails()
        {
            Grid_Schedule.Columns[0].Visible = true;
            Grid_Schedule.Columns[1].Visible = true;

            Grid_Schedule.DataSource = null;
            string DateFormate="%d/%m/%Y";
            string sql = "SELECT id,ScheduleId ,ScheduleName ,DATE_FORMAT(LastactionDate,'"+DateFormate+"' ) as LastactionDate ,DATE_FORMAT(NextActionDate, '"+DateFormate+"')as NextActionDate,CASE WHEN PeroidType IN ('1') THEN 'Daily' WHEN PeroidType = '2' THEN 'Weekly' ELSE 'Mothly' END AS PeroidType FROM tblmisschedule";
            DataSet Ds = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds != null && Ds.Tables != null && Ds.Tables[0].Rows.Count > 0)
            {

                Grid_Schedule.DataSource = Ds;
                Grid_Schedule.DataBind();
                Pnl_scheduleGrid.Visible = true;

            }
            else
            {
                Grid_Schedule.DataSource = null;
                Grid_Schedule.DataBind();
            }

            Grid_Schedule.Columns[0].Visible = false;
            Grid_Schedule.Columns[1].Visible = false;

        }


        protected void Grid_Schedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
           
        }
       
        protected void Grid_Schedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Grid_Schedule.Columns[0].Visible = true;
            Grid_Schedule.Columns[1].Visible = true;
            if (e.CommandName == "View" || e.CommandName == "Edit" || e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int scheduleid = int.Parse(Grid_Schedule.Rows[index].Cells[0].Text);
               
                if (e.CommandName == "Edit")
                {
                    WC_MISMESSAGEBOX.ShowMssage(scheduleid,"Edit");
                  

                }
                if (e.CommandName == "View")
                {
                    WC_MISMESSAGEBOX.ShowMssage(scheduleid, "View");

                }
                if (e.CommandName == "Delete")
                {
                    DeleteSchedule(scheduleid);
                   
                }

            }
            Grid_Schedule.Columns[0].Visible = false;
            Grid_Schedule.Columns[1].Visible = false;

        }

        private void DeleteSchedule(int scheduleid)
        {
            string sql = "delete from tblmisschedule where ScheduleId=" + scheduleid;
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            
            sql = "delete from tblmisschedulereportmap where ScheduleId=" + scheduleid;
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            Lbl_msg.Text = "Selected schedule deleted successfully.";
            LoadGridDetails();
        }

        private bool Validation(out string Err, string Peroidtype, out int peroidvalue)
        {
            peroidvalue = -1;
            Err = "";
            bool validation = true;
            int checkboxvalue = -1;
            switch (Peroidtype)
            {
                case "Daily":
                    peroidvalue = 1;
                    break;
                case "Weekly":
                    peroidvalue = 2;
                    break;
                case "Mothly":
                    peroidvalue = 3;
                    break;
            }
            if (Txt_misschedulename.Text.Trim() == null)
            {
                Err = "Enter the schedule name!";
                validation = false;
            }
            if (peroidvalue == -1)
            {
                Err = "Please Select Peroid Type!";
                validation = false;
            }
            if (Txt_emailaddress.Text.Trim() == null)
            {
                Err = "Enter the email address!";
                validation = false;
            }
            for (int i = 0; i < Chkboxlist_reportname.Items.Count; i++)
            {
                if (Chkboxlist_reportname.Items[i].Selected == true)
                    checkboxvalue = int.Parse(Chkboxlist_reportname.Items[i].Value);
            }
            if (checkboxvalue == -1)
            {
                Err = "Please Select Report Name!";
                validation = false;
            }

            return validation;
        }

        protected void Chkboxlist_reportname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Chkboxlist_reportname.Items[0].Selected == true)
            {
                CheckBoxValidation(1);
            }
            if (Chkboxlist_reportname.Items[0].Selected == false)
            {
                CheckBoxValidation(0);
            }
        }

        private void CheckBoxValidation(int value)
        {
            for (int i = 0; i < Chkboxlist_reportname.Items.Count; i++)
            {
                if (value == 0)
                    Chkboxlist_reportname.Items[i].Selected = false;
                else
                    Chkboxlist_reportname.Items[i].Selected = true;
            }
        }


        //remove items
        protected void Grid_Schedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _TempStudId = "";
                
        }
        protected void Grid_Schedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _TempStudId = "";
        }

    }
}
