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
    public partial class SMSLogg : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
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
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(99))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    
                    //some initlization
                    MysmsMang.InitClass();
                    Fill_LoggGrid();
                }
            }


        }

        private void Fill_LoggGrid()
        {
            lbl_msg.Text = "";
            Btn_Cancel.Visible = true;
            Btn_Remove.Visible = true;
            Link_SelectAll.Visible = true;
            int _status=0;
            DataSet _Loggdataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Loggdataset.Tables.Add(new DataTable("Logg"));
            dt = _Loggdataset.Tables["Logg"];
            dt.Columns.Add("Id");
            dt.Columns.Add("PhoneNo");
            dt.Columns.Add("Message");
            dt.Columns.Add("ScheduledTime");
            dt.Columns.Add("Status");


            string sql = "SELECT Id,PhoneNumber,Message,`Status`,TimeToSend FROM tblautosms WHERE `Status`<>2 ORDER BY Id DESC";
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {

                while (MyReader.Read())
                {
                    dr = _Loggdataset.Tables["Logg"].NewRow();
                    dr["Id"] = MyReader.GetValue(0).ToString();
                    dr["PhoneNo"] = MyReader.GetValue(1).ToString();
                    dr["Message"] = MyReader.GetValue(2).ToString();
                    if (MyReader.GetValue(4).ToString() == "0")
                    {
                        dr["ScheduledTime"] = "Not Scheduled";
                    }
                    else
                    {
                        dr["ScheduledTime"] = MyReader.GetValue(4).ToString();
                    }
                    int.TryParse(MyReader.GetValue(3).ToString(), out _status);

                    if (_status == 1)
                    {
                        dr["Status"] = "Send";
                    }
                    else
                    {
                        dr["Status"] = "Waiting";
                    }



                    _Loggdataset.Tables["Logg"].Rows.Add(dr);
                }
                Grd_SMSLogg.Columns[1].Visible = true;
                Grd_SMSLogg.DataSource = _Loggdataset;
                Grd_SMSLogg.DataBind();
                Grd_SMSLogg.Columns[1].Visible = false;
                Color_Grid();
            }
            else
            {
                Grd_SMSLogg.DataSource = null;
                Grd_SMSLogg.DataBind();
                lbl_msg.Text = "No SMS Logs Present";
                Btn_Cancel.Visible = false;
                Btn_Remove.Visible = false;
                Link_SelectAll.Visible = false;
            }
            
        }

        private void Color_Grid()
        {
            foreach (GridViewRow gv in Grd_SMSLogg.Rows)
            {
                if (gv.Cells[5].Text.ToString() == "Send")
                {
                    gv.Cells[5].ForeColor = System.Drawing.Color.Green;
                }
                if (gv.Cells[5].Text.ToString() == "Waiting")
                {
                    gv.Cells[5].ForeColor = System.Drawing.Color.Brown;
                }
            }
        }

        protected void Grd_SMSLogg_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_SMSLogg.PageIndex = e.NewPageIndex;
            Fill_LoggGrid();
        }

        protected void Link_SelectAll_Click(object sender, EventArgs e)
        {
            lbl_msg.Text = "";
            foreach (GridViewRow gv in Grd_SMSLogg.Rows)
            {
                CheckBox Chk_sms = (CheckBox)gv.FindControl("Checksms");
                Chk_sms.Checked = true;
            }

        }

        protected void Btn_Remove_Click(object sender, EventArgs e)
        {
            try
            {
                MyStudMang.CreateTansationDb();
                foreach (GridViewRow gv in Grd_SMSLogg.Rows)
                {
                    CheckBox Chk_sms = (CheckBox)gv.FindControl("Checksms");
                    if (Chk_sms.Checked)
                    {
                        Delete_Sms(int.Parse( gv.Cells[1].Text.ToString()));
                    }
                }
                MyStudMang.EndSucessTansationDb();
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "SMS Log", "sms log removed", 1);
                lbl_msg.Text = "Successfully Removed";
            }
            catch
            {
                MyStudMang.EndFailTansationDb();
                lbl_msg.Text = "Error in removal";
            }
           Fill_LoggGrid();
        }

        private void Delete_Sms(int _Id)
        {
            string sql = "Delete from tblautosms WHERE Id=" + _Id;
            MyStudMang.m_TransationDb.ExecuteQuery(sql);
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            lbl_msg.Text = "";
            foreach (GridViewRow gv in Grd_SMSLogg.Rows)
            {
                CheckBox Chk_sms = (CheckBox)gv.FindControl("Checksms");
                Chk_sms.Checked = false;
            }
        }
    }
}
