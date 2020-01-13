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
    public partial class PeriodConfig : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];

            if (MyUser.SELECTEDMODE == 1)
            {
                this.MasterPageFile = "~/WinerStudentMaster.master";

            }
            else if (MyUser.SELECTEDMODE == 2)
            {

                this.MasterPageFile = "~/WinerSchoolMaster.master";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(623))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    Load_PeriodGrid();
                }
            }
        }

        private void Load_PeriodGrid()
        {
            string sql = "SELECT PeriodId,FrequencyName,FromTime,ToTime FROM tblattendanceperiod WHERE tblattendanceperiod.ModeId=3 ORDER BY count";
            DataSet Mydataset = MyConfigMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Mydataset != null && Mydataset.Tables[0].Rows.Count > 0)
            {
                Grid_Periods.Columns[0].Visible = true;
                Grid_Periods.DataSource = Mydataset;
                Grid_Periods.DataBind();
                Grid_Periods.Columns[0].Visible = false;
            }
        }

        protected void Grid_Periods_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string PeriodId = Grid_Periods.DataKeys[e.NewEditIndex].Values["PeriodId"].ToString();
            LoadPeriod_EditingDetails(PeriodId);
            Hd_PeriodId.Value = PeriodId;
            MPE_Edit.Show();
        }

        //protected void Grid_Periods_Selecetedindexchanging(object sender, GridViewSelectEventHandler e)
        //{



        //}



        private void LoadPeriod_EditingDetails(string PeriodId)
        {
            string sql = "SELECT FrequencyName,FromTime,ToTime FROM tblattendanceperiod WHERE tblattendanceperiod.PeriodId="+PeriodId;
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Txt_Period.Text = MyReader.GetValue(0).ToString();
                Txt_From.Text = MyReader.GetValue(1).ToString();
                Txt_To.Text = MyReader.GetValue(2).ToString();
            }
        }

        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            Lbl_EditError.Text = "";
            string msg = "";
            if (IsUpdatePossible(out msg))
            {
                try
                {

                    string sql = "UPDATE tblattendanceperiod SET FrequencyName='" + Txt_Period.Text + "',FromTime='" + Txt_From.Text + "',ToTime='" + Txt_To.Text + "' WHERE PeriodId=" + Hd_PeriodId.Value;
                    MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Period Config", "Period:" + Txt_Period.Text + " configuration updated", 1);
                    Load_PeriodGrid();
                }
                catch
                {
                    Lbl_EditError.Text = "Error while updating. Try Later";
                    MPE_Edit.Show();
                }

            }
            else
            {
                Lbl_EditError.Text = msg;
                MPE_Edit.Show();
            }

        }

        private bool IsUpdatePossible(out string msg)
        {
            bool valid = true;
            TimeSpan fromspan=new TimeSpan();
            TimeSpan Tospan=new TimeSpan();
            msg = "";
            if (Txt_Period.Text.Trim() == "")
            {
                msg = "Period name cannot be empty.";
                valid = false;
            }
            else if (!TimeSpan.TryParse(Txt_From.Text, out fromspan))
            {
                msg = "Enter correct from time.";
                valid = false;
            }
            else if(!TimeSpan.TryParse(Txt_To.Text, out Tospan))
            {
                msg = "Enter correct to time";
                valid = false;
            }
            else if (Tospan <= fromspan)
            {
                msg = "From time should be greater than to time";
                valid = false;
            }
            return valid;
        }

        protected void Lnk_Add_Click(object sender, EventArgs e)
        {
            LoadAddPopup();
        }

        protected void Img_Add_Click(object sender, ImageClickEventArgs e)
        {
            LoadAddPopup();
        }

        private void LoadAddPopup()
        {
            Txt_AddPeriod.Text = "";
            Txt_AddFromTime.Text = "";
            Txt_AddToTime.Text = "";
            Lbl_AddError.Text = "";
            MPE_Add.Show();
        }

        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Lbl_AddError.Text = "";
            string msg = "";
            if (IsAddPossible(out msg))
            {
                try
                {
                    int Order = GetLastCount_Period()+1;

                    string sql = "INSERT INTO tblattendanceperiod(ModeId,FrequencyName,count,FromTime,ToTime) VALUES(3, '" + Txt_AddPeriod.Text + "',"+Order+",'"+Txt_AddFromTime.Text+"','"+Txt_AddToTime.Text+"')";
                    MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Period Config", "Period:" + Txt_AddPeriod.Text + " configuration added", 1);
                    Load_PeriodGrid();
                }
                catch
                {
                    Lbl_AddError.Text = "Error while saving. Try Later";
                    MPE_Add.Show();
                }

            }
            else
            {
                Lbl_AddError.Text = msg;
                MPE_Add.Show();
            }
        }

        private int GetLastCount_Period()
        {
            int count = 0;
            string sql = "SELECT max(tblattendanceperiod.`count`) FROM tblattendanceperiod WHERE tblattendanceperiod.ModeId=3";
            MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(),out count);
            }
            return count;  
        }

        private bool IsAddPossible(out string msg)
        {
            bool valid = true;
            TimeSpan fromspan = new TimeSpan();
            TimeSpan Tospan = new TimeSpan();
            msg = "";
            if (Txt_AddPeriod.Text.Trim() == "")
            {
                msg = "Period name cannot be empty.";
                valid = false;
            }
            else if (!TimeSpan.TryParse(Txt_AddFromTime.Text, out fromspan))
            {
                msg = "Enter correct from time.";
                valid = false;
            }
            else if (!TimeSpan.TryParse(Txt_AddToTime.Text, out Tospan))
            {
                msg = "Enter correct to time";
                valid = false;
            }
            else if (Tospan <= fromspan)
            {
                msg = "From time should be greater than to time";
                valid = false;
            }
            return valid;
        }

    }
}
