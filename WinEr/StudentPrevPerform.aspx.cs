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
using WinEr;
using WebChart;
using System.Drawing;
using System.Text;

public partial class StudentPrevPerform : System.Web.UI.Page
{
    private OdbcDataReader MyReader1 = null;
    
    private StudentManagerClass MyStudMang;
    private KnowinUser MyUser;
    private ExamManage MyExamMang;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["StudId"] == null)
        {
            Response.Redirect("SearchStudent.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStudMang = MyUser.GetStudentObj();
        if (MyStudMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(11))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
                string _MenuStr;
                _MenuStr = MyStudMang.GetSubStudentMangMenuString(MyUser.UserRoleId, int.Parse(Session["StudType"].ToString()));
                this.SubStudentMenu.InnerHtml = _MenuStr;
                LoadPrevClassDrp();
                LoadStudentTopData();

            }
        }
    }

    private void LoadStudentTopData()
    {

        string _Studstrip = MyStudMang.ToStripString(int.Parse(Session["StudId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StudId"].ToString()) + "&type=StudentImage", int.Parse(Session["StudType"].ToString()));
        this.StudentTopStrip.InnerHtml = _Studstrip;
    }


    private void LoadPrevClassDrp()
    {
        int studid = 0;
        int.TryParse(Session["StudId"].ToString(), out studid);
        drp_prevClass.Items.Clear();
        MyReader = MyStudMang.GetPrevClassBatchs(studid,0,0);
        if (MyReader.HasRows)
        {

            ListItem li = new ListItem("All", "0");
            drp_prevClass.Items.Add(li);

            while (MyReader.Read())
            {
                li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                drp_prevClass.Items.Add(li);
            }

        }
        else
        {
            drp_prevClass.Items.Add(new ListItem("No Previos Classes Found", "-1"));
        } 
    }



    protected void drp_prevClass_change(object sender, EventArgs e)
    {

    }

    protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
    {
        string FileName = "PreviousClassPerform";
        Response.ContentType = "application/force-download";
        Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
        Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
        Response.Write("<head>");
        Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
        Response.Write("<!--[if gte mso 9]><xml>");
        Response.Write("<x:ExcelWorkbook>");
        Response.Write("<x:ExcelWorksheets>");
        Response.Write("<x:ExcelWorksheet>");
        Response.Write("<x:Name>Student Details</x:Name>");
        Response.Write("<x:WorksheetOptions>");
        Response.Write("<x:Print>");
        Response.Write("<x:ValidPrinterInfo/>");
        Response.Write("</x:Print>");
        Response.Write("</x:WorksheetOptions>");
        Response.Write("</x:ExcelWorksheet>");
        Response.Write("</x:ExcelWorksheets>");
        Response.Write("</x:ExcelWorkbook>");
        Response.Write("</xml>");
        Response.Write("<![endif]--> ");
        Response.Write(GetPrevPerformDetails());
        Response.Write("</head>");
        Response.Flush();
        Response.End();
    }

    private string GetPrevPerformDetails()
    {
        OdbcDataReader MyReader_prevbatch = null;
        OdbcDataReader MyReader_subjname = null;
        OdbcDataReader MyReader_subjmark= null;
        OdbcDataReader MyReader_examnames = null;
        OdbcDataReader MyReader_examresult = null;
        string markcolum = "", subjname="", classbatchname="";
        int batchId = 0, classid = 0, exmschid=0,studid=0;
        int.TryParse(Session["StudId"].ToString(), out studid);
        if(int.Parse(drp_prevClass.SelectedItem.Value.ToString())>0)
        classbatchname=GetClassBatch(drp_prevClass.SelectedItem.Text.ToString(), out batchId, out classid);
        StringBuilder CTR = new StringBuilder();

        CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");
        CTR.Append("<tr>");
        if (classbatchname == "")
        {
            MyReader_prevbatch = MyStudMang.GetPrevClassBatchs(studid, 0, 0); 
        }
        else
        {
            MyReader_prevbatch = MyStudMang.GetPrevClassBatchs(studid, batchId, classid);
        }
        if (MyReader_prevbatch.HasRows)
        {
            while (MyReader_prevbatch.Read())
            {
                classbatchname = GetClassBatch(MyReader_prevbatch.GetValue(1).ToString(), out batchId, out classid);
                CTR.Append("<td valign=\"top\"><table runat=\"server\" width=\"100%\" ><tr><td colspan=\"2\"> <b>" + classbatchname + "</b></td></tr><tr><td></td></tr>");
                MyReader_examnames = MyStudMang.getPrevClassExamNames(classid, batchId);
                if (MyReader_examnames.HasRows)
                {
                    while (MyReader_examnames.Read())
                    {
                        CTR.Append("<tr><td colspan=\"2\"><b><i>" + MyReader_examnames.GetValue(1).ToString() + "(" + MyReader_examnames.GetValue(2).ToString() + ")</i></b></td></tr>");
                        int.TryParse(MyReader_examnames.GetValue(0).ToString(), out exmschid);
                        MyReader_subjname = MyStudMang.getPrevClassExamSubjNames(exmschid);
                        if (MyReader_subjname.HasRows)
                        {
                            while (MyReader_subjname.Read())
                            {
                                markcolum = MyReader_subjname.GetValue(0).ToString();
                                subjname = MyReader_subjname.GetValue(1).ToString();

                                MyReader_subjmark = MyStudMang.getPrevClassExamSubjMarks(markcolum, exmschid, studid);
                                if (MyReader_subjmark.HasRows)
                                {
                                    while (MyReader_subjmark.Read())
                                    {
                                        CTR.Append("<tr><td align=\"right\">" + subjname + "</td><td align=\"left\"> " + MyReader_subjmark.GetValue(0).ToString() + "</td></tr>");
                                    }
                                }
                            }
                        }

                        MyReader_examresult = MyStudMang.getPrevClassExamResult(studid, exmschid);
                        if (MyReader_examresult.HasRows)
                        {
                            while (MyReader_examresult.Read())
                            {
                                CTR.Append("<tr><td align=\"right\"> Total Marks:</td> <td align=\"left\">" + MyReader_examresult.GetValue(0).ToString() + "</td></tr>");
                                CTR.Append("<tr><td align=\"right\"> Max Marks: </td><td align=\"left\">" + MyReader_examresult.GetValue(1).ToString() + "</td></tr>");
                                CTR.Append("<tr><td align=\"right\"> Average: </td><td align=\"left\">" + MyReader_examresult.GetValue(2).ToString() + "</td></tr>");
                                CTR.Append("<tr><td align=\"right\"> Grade: </td><td align=\"left\">" + MyReader_examresult.GetValue(3).ToString() + "</td></tr>");
                                CTR.Append("<tr><td align=\"right\"> Result: </td><td align=\"left\">" + MyReader_examresult.GetValue(4).ToString() + "</td></tr>");
                                CTR.Append("<tr><td align=\"right\"> Rank: </td><td align=\"left\">" + MyReader_examresult.GetValue(5).ToString() + "</td></tr>");
                            }
                        }

                    }
                }
                CTR.Append("</table>");
                CTR.Append("</td>");
            }
        }
        CTR.Append("</tr>");
        CTR.Append("</table>");

        return CTR.ToString();
    }

    private string GetClassBatch(string classbatch, out int batchId, out int classid)
    {
        string classbatchname = "", batch = "", classname="";
        batchId = 0;
        classid = 0;
        string[] classbatcharay=classbatch.Split('@');
        classname = classbatcharay[0].ToString();
        batch = classbatcharay[1].ToString();
        classbatchname = classname + "  " + batch;
        batchId = MyStudMang.getBatchIdFromName(batch);
        classid = MyStudMang.getClassIdFromName(classname);
        return classbatchname;
    }
}

