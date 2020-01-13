using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;
using WinBase;
using WinEr;
public partial class CurriculumDetails : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    private Incident MyIncident;
    private OdbcDataReader MyReader = null;
    private DataSet MyDataset = new DataSet();
    private int studId = 0;
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (Session["StudId"] == null)
        {
            Response.Redirect("SearchStudent.aspx");
        }
        if (Session["StudType"] == null)
        {
            Response.Redirect("SearchStudent.aspx");
        }
        studId = int.Parse(Session["StudId"].ToString());
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        MyIncident = MyUser.GetIncedentObj();
        if (MyStudMang == null)
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

             //   string _MenuStr;
             //   _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
             //   this.SubStudentMenu.InnerHtml = _MenuStr;
                Pnl_Tc.Visible = false;
              //  LoadpupilTopData();
                LoadCurriculumDetails();
            }
        }
    }
    private void LoadCurriculumDetails()
    {
        Grd_Carrier.DataSource = null;
        Grd_Carrier.DataBind();
        MyReader = MyStudMang.GetAdmissionDtls(studId);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                lbl_DOJ.Text = MyReader.GetValue(0).ToString();
                lbl_JnBatch.Text = MyReader.GetValue(1).ToString();
                lbl_JnStd.Text = MyReader.GetValue(2).ToString();
                lbl_JnClass.Text = MyReader.GetValue(3).ToString();
                lbl_CrUser.Text = MyReader.GetValue(4).ToString();
                lbl_crDate.Text = MyReader.GetValue(5).ToString();
                lbl_TempID.Text = MyReader.GetValue(6).ToString();
                if (MyReader.GetValue(6).ToString() == "0")
                    lbl_TempID.Text = "Nil";
            }
        }
        Grd_Carrier.DataSource = GetDtls(MyStudMang.GetCarrierData(studId));
        Grd_Carrier.DataBind();
        if (Grd_Carrier.Rows.Count > 0)
        {
            ModifyRowStyle();
        }
        MyReader = null;
        MyReader = MyStudMang.GetTCDetails(studId);
        if (MyReader.HasRows)
        {
            Pnl_Tc.Visible = true;
            while (MyReader.Read())
            {
                lbl_TcNo.Text = MyReader.GetValue(0).ToString();
                lbl_DOL.Text = MyReader.GetValue(1).ToString();
                lbl_LastClass.Text = MyReader.GetValue(3).ToString();
                lbl_LastBatch.Text = MyReader.GetValue(2).ToString();
            }
        }
        else
        {
            Pnl_Tc.Visible = false;
        }
    }

    private DataSet GetDtls(DataSet MyDataset)
    {
        int ClassId = -1, PrevClassId = -1, rowId = 0;
        if (MyDataset != null && MyDataset.Tables[0] != null && MyDataset.Tables[0].Rows.Count > 0)
        {
            DataTable dt = MyDataset.Tables[0];
            dt.Columns.Add("Result");
            foreach (DataRow dro in MyDataset.Tables[0].Rows)
            {
                string result = "Passed";
                ClassId = int.Parse(dro["ClassId"].ToString());
                if (PrevClassId == ClassId)
                {
                    int id = rowId - 1;
                    MyDataset.Tables[0].Rows[id]["Result"] = "Failed";
                }
                PrevClassId = ClassId;
                dro["Result"] = result;
                rowId++;
            }
            MyDataset.Tables[0].Rows[rowId - 1]["Result"] = MyStudMang.getStatus(studId);
        }
        return MyDataset;
    }

    private void ModifyRowStyle()
    {
        foreach (GridViewRow gv in Grd_Carrier.Rows)
        {
            if (gv.Cells[3].Text == "Passed")
            {
                gv.Cells[3].ForeColor=System.Drawing.Color.Green;
            }
            else if (gv.Cells[3].Text == "Failed")
            {
                gv.Cells[3].ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (gv.Cells[3].Text == "Ongoing")
            {
                gv.Cells[3].ForeColor = System.Drawing.Color.Brown;
            }
            else 
            {
 
            }
        }
    }

    protected void Btn_View_Click(object sender, EventArgs e)
    {
        TCpdf MyPdf = new TCpdf(MyStudMang.m_MysqlDb, objSchool);
        string _ErrorMsg = "";
        string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));// +"\\PDF_Files\\Invoice" + _InvoiceID + ".pdf";
        string _PdfName = "";
        int _StudentId = studId;

        if (MyPdf.GenerateStudentTCPdf(_StudentId, _physicalpath, out _PdfName, out _ErrorMsg))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
        }
        else
        { }
    }
}