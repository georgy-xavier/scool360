using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using WinBase;

namespace WinEr
{
    public partial class NonTripStudentManagement : System.Web.UI.Page
    {
        private TransportationClass MyTransMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyTransMang = MyUser.GetTransObj();

            if (MyTransMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(202))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClass_Drp();
                    LoadGrid();
                }

            }
        }

        private void LoadGrid()
        {
            string _Err = "";
            grdResult.DataSource = null;
            Lbl_total.Text = "0";
            int _classid = 0;
            try
            {
                int.TryParse(Drp_class.SelectedValue.ToString(), out _classid);
                if (_classid < 0)
                    _Err = "Class is not Found!";
                else
                {
                    string EqualCondition = "="+_classid;
                    if(_classid==0)
                        EqualCondition = "!="+_classid;
                    string sql = "SELECT tblstudentclassmap.RollNo,tblstudent.StudentName,tblclass.ClassName,tblstudent.Sex,tblstudent.Address from tblstudentclassmap INNER join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId where tblstudentclassmap.StudentId not in(select tbl_tr_studtripmap.StudId from tbl_tr_studtripmap) AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND  tblstudentclassmap.ClassId" + EqualCondition + "  order by tblstudentclassmap.RollNo";
                    DataSet Ds = MyTransMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (Ds == null)
                        _Err = "Students not found!";
                    else
                    {
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            pnl_grid.Visible = true;
                            grdResult.DataSource = Ds.Tables[0];
                            grdResult.DataBind();
                            Lbl_total.Text = Ds.Tables[0].Rows.Count.ToString();
                            ViewState["StudentList"] = Ds;
                        }
                        else
                            _Err = "Students not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                _Err = ex.Message;
            }
            if (_Err != "")
            {
                pnl_grid.Visible = false;
                Lbl_total.Text = "0";
                ViewState["StudentList"] = null;
                WC_MessageBox.ShowMssage(_Err);
            }
        }

        private void LoadClass_Drp()
        {
            Drp_class.Items.Clear();
            MyDataSet = MyUser.MyAssociatedClass();
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count != 0)
            {
                ListItem li = new ListItem("All", "0");
                Drp_class.Items.Add(li);

                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_class.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Classes Found", "-1");
                Drp_class.Items.Add(li);
            }
        }

        protected void drp_class_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void img_export_Excel_Click(object sender, EventArgs e)
        {
            string classname = Drp_class.SelectedItem.Text.ToString();
            string filename = classname + "_NonTransportaionReport.xls";
            if (ViewState["StudentList"] != null)
            {
                DataSet MyExamData = (DataSet)ViewState["StudentList"];
                if (MyExamData.Tables.Count > 0)
                {
                    string _ReportName = "Non Transportation Report";
                    if (!WinEr.ExcelUtility.ExportDataToExcel(MyExamData, _ReportName, filename, MyUser.ExcelHeader))
                    {
                        WC_MessageBox.ShowMssage("This function need Ms office");
                    }
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("TripStudentManagement.aspx");
        }


    }
}
