using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class StaffRFSwipingRecord : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StaffManager MyStaffMang;

        #region Events

        #endregion

            protected void Page_Load(object sender, EventArgs e)
            {

                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyStaffMang = MyUser.GetStaffObj();
                if (MyStaffMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (!MyUser.HaveActionRignt(908))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {

                    if (!IsPostBack)
                    {
                        LoadStaff();
                        img_export_Excel.Visible = false;
                    }
                }
            }     

            protected void Btn_Load_Click(object sender, EventArgs e)
            {
                Load_Grid();

            }      

            protected void Grd_SwipeLogg_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {

                Grd_SwipeLogg.PageIndex = e.NewPageIndex;
                Load_Grid();

            }


            protected void img_export_Excel_Click(object sender, EventArgs e)
            {
                DataSet MyData = (DataSet)ViewState["Report"];
                if (!WinEr.ExcelUtility.ExportDataSetToExcel(MyData, "RfRecord.xls"))
                {
                    lbl_error.Text = "";
                }
            }

            protected void Btn_DeleteAll_Click(object sender, EventArgs e)
            {
                string _msg = "";
                if (_IsDeletionPossible(out _msg))
                {
                    try
                    {
                        DateTime _date = General.GetDateTimeFromText(Txt_StartDate.Text);
                        string sql = "DELETE  tblexternalattencence from tblexternalattencence INNER JOIN tblexternalreff  WHERE tblexternalattencence.ActionDate<'" + _date.Date.ToString("s") + "' and tblexternalattencence.ExternalReffid=tblexternalreff.Id   and tblexternalreff.UserType='STAFF'";
                        MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
                        Load_Grid();
                        lbl_error.Text = "Deletion done successfully";
                    }
                    catch
                    {
                        lbl_error.Text = "Error while deletion. Try later.";
                    }
                }
                else
                {
                    lbl_error.Text = _msg;
                }
            }
         
        #region Methods

            private void LoadStaff()
            {
                Drp_Staff.Items.Clear();

                string sql = "select DISTINCT( tbluser.Id), tbluser.SurName from tbluser where tbluser.Status=1 ORDER BY tbluser.SurName";
                OdbcDataReader MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Btn_Load.Enabled = true;
                    lbl_error.Text = "";
                    while (MyReader.Read())
                    {
                        ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Staff.Items.Add(li);
                    }
                }
                else
                {
                    Btn_Load.Enabled = false;
                    ListItem li = new ListItem("No Staff present", "-1");
                    Drp_Staff.Items.Add(li);
                    lbl_error.Text = "No Staff found";
                }
            }

            private void Load_Grid()
            {
                lbl_error.Text = "";
                string sql = "SELECT tblexternalrfid.Location as RFReaderName,Date_Format(tblexternalattencence.ActionDate,'%d/%m/%Y %h:%i %p') as ActionDate,tblexternalattencence.RFReaderType as RFReaderType FROM tblexternalattencence INNER JOIN tblexternalrfid ON tblexternalattencence.RFReaderID=tblexternalrfid.EquipmentId INNER JOIN tblexternalreff ON tblexternalattencence.ExternalReffid=tblexternalreff.Id WHERE  tblexternalreff.UserType='STAFF' and tblexternalreff.UserId=" + Drp_Staff.SelectedValue;
                DataSet MyDataset = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MyDataset != null && MyDataset.Tables[0].Rows.Count > 0)
                {
                    Grd_SwipeLogg.DataSource = MyDataset;
                    Grd_SwipeLogg.DataBind();
                    img_export_Excel.Visible = true;
                }
                else
                {
                    lbl_error.Text = "No report found";
                    Grd_SwipeLogg.DataSource = null;
                    Grd_SwipeLogg.DataBind();
                    img_export_Excel.Visible = false;
                }

                ViewState["Report"] = MyDataset;
            }

            private bool _IsDeletionPossible(out string _msg)
            {
                _msg = "";
                bool _valid = true;
                if (Txt_StartDate.Text == "")
                {
                    _msg = "Please enter date till data can be deleted";
                    _valid = false;
                }
                else if (General.GetDateTimeFromText(Txt_StartDate.Text) > DateTime.Now.Date.AddDays(-30))
                {
                    _msg = "Date should be less than 30 days";
                    _valid = false;
                }
                return _valid;
            }

        #endregion

    }
}
