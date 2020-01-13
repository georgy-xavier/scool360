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
using System.Text;

namespace WinEr
{
    public partial class SearchIncident : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private Incident MyIncedent;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MyAccordion.SelectedIndex = -1;
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();

            MyIncedent = MyUser.GetIncedentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(650))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {

                    Pnl_Searchresult.Visible = false;
                    lbl_srch_ok.Visible = false;
                    img_export_Excel.Visible = false;
                    img_rslt.Visible = false;
                    lbl_srch_cnt.Visible = false;


                    Txt_Search_AutoCompleteExtender.ContextKey = "1\\" + MyUser.UserId.ToString();

                    if (Request.QueryString["Keyword"] != null)
                    {
                        Txt_Search.Text = Request.QueryString["Keyword"].ToString();
                        chk_history.Checked = true;
                        FillGrid(1);
                    }
                }
            }
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            //MyAccordion.SelectedIndex = -1;
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

        protected void Grd_Incedent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Incedent.PageIndex = e.NewPageIndex;
            //FillGrid();

            DataSet MyDataSetNew = (DataSet)ViewState["IncidentList"];
            Grd_Incedent.Columns[0].Visible = true;
            Grd_Incedent.Columns[1].Visible = true;
            Grd_Incedent.Columns[2].Visible = true;
            Grd_Incedent.Columns[3].Visible = true;
            Grd_Incedent.DataSource = MyDataSetNew;
            Grd_Incedent.DataBind();
            Grd_Incedent.Columns[0].Visible = false;
            Grd_Incedent.Columns[1].Visible = false;
            Grd_Incedent.Columns[2].Visible = false;
            Grd_Incedent.Columns[3].Visible = false;
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            FillGrid(0);
        }
      
        protected void Btn_DeepSearch_Click(object sender, EventArgs e)
        {
            FillGrid(1);
        }

        private void FillGrid(int type)
        {
            int history = 0;
            Grd_Incedent.PageIndex = 0;
            DataSet MydataSet = null;
            string keyword =  Txt_Search.Text.ToString();
            if (chk_history.Checked == true)
                history = 1;
            if (keyword.Trim() != "")
            {
                if (type == 0)
                {
                    MydataSet = MyIncedent.getIncedentInfo(keyword, history);
                }
                else if (type == 1)
                {
                    MydataSet = MyIncedent.getDeepInfo(keyword, history);
                }


                Grd_Incedent.Columns[0].Visible = true;
                Grd_Incedent.Columns[1].Visible = true;
                Grd_Incedent.Columns[2].Visible = true;
                Grd_Incedent.Columns[3].Visible = true;
                Grd_Incedent.DataSource = Finaldataset(MydataSet);
                Grd_Incedent.DataBind();
                Grd_Incedent.Columns[0].Visible = false;
                Grd_Incedent.Columns[1].Visible = false;
                Grd_Incedent.Columns[2].Visible = false;
                Grd_Incedent.Columns[3].Visible = false;
                Pnl_Searchresult.Visible = true;
                Grd_Incedent.Visible = true;
            }
            else
            {

                Pnl_Searchresult.Visible = true;
                lbl_srch_ok.Visible = false;
                img_export_Excel.Visible = false;
                img_rslt.Visible = false;
                lbl_srch_cnt.Visible = false;
                lbl_srch.Text = "Enter any Keyword... !";
            }
        }

        private DataSet Finaldataset(DataSet MydataSet)
        {
            string usertype = "";
            int point = 0;
            img_rslt.Visible = false;
                lbl_srch_ok.Visible=false;
                img_export_Excel.Visible=false;
                lbl_srch_cnt.Visible = false;
            lbl_srch.Text = "";
            string redirectpage = "", tblstrng = "",title="",description="", username="";
            string imagefile="\"images/stdnt.png\" alt=\"No Image\"";
            int userId = 0, incidentId=0;
            DataSet incidentDataset = new DataSet();
            DataTable dt = new DataTable();
            incidentDataset.Tables.Add(new DataTable("IncidentTable"));
            dt = incidentDataset.Tables["IncidentTable"];
            dt.Columns.Add("Id");
            dt.Columns.Add("StudId");
            dt.Columns.Add("Info");
            dt.Columns.Add("Type");
            dt.Columns.Add("Point");


            DataSet ExcelDataset = new DataSet();
            DataTable edt = new DataTable();
            ExcelDataset.Tables.Add(new DataTable("ExcelDataTable"));
            edt = ExcelDataset.Tables["ExcelDataTable"];
            edt.Columns.Add("Name");
            edt.Columns.Add("Incident");
            edt.Columns.Add("Description");
            edt.Columns.Add("Type");
            edt.Columns.Add("IncidentDate");
            edt.Columns.Add("CreatedDate");
            edt.Columns.Add("CreatedBy");
            edt.Columns.Add("Point");

            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow drow in MydataSet.Tables[0].Rows)
                {
                    DataRow dr = dt.NewRow();
                    DataRow edr = edt.NewRow();
                    string pnttbl = "";
                    incidentId = int.Parse(drow["Id"].ToString());
                    userId = int.Parse(drow["StudId"].ToString());
                    title = drow["title"].ToString();
                    description = drow["description"].ToString();
                    point = int.Parse(drow["Point"].ToString());
                    if (drow["UserType"].ToString().ToLowerInvariant() == "student")
                    {
                        //Student
                        redirectpage = "StudentSessionMaker.aspx?StudentId=" + userId.ToString() + "";
                      //  imagefile = MyUser.GetImageUrl("StudentImage", userId);
                        imagefile = "Handler/ImageReturnHandler.ashx?id=" + userId + "&type=StudentImage";
                        string type = "";
                        username = MyUser.getStudName(userId);
                        usertype = "student";
                    }
                    else if (drow["UserType"].ToString().ToLowerInvariant() == "staff")
                    {
                        //Staff
                        if (MyUser.HistoryUser(userId))
                        {
                            redirectpage = "StaffSearchDetails.aspx?StaffId=" + userId.ToString() + "";
                        }
                        else
                        {
                        redirectpage = "StaffSessionMaker.aspx?StaffId=" + userId.ToString() + "";
                        }
                    //    imagefile = MyUser.GetImageUrl("StaffImage", userId);
                        imagefile = "Handler/ImageReturnHandler.ashx?id=" + userId + "&type=StaffImage";
                        username = MyUser.getStaffName(userId);
                        usertype = "staff";
                    }
                    else if (drow["UserType"].ToString().ToLowerInvariant() == "class")
                    {
                        //Class
                        usertype = "Class";
                    }

                    dr["Id"] = drow["Id"].ToString();
                    dr["StudId"] = drow["StudId"].ToString();
                    dr["Info"] = getInfo(incidentId, imagefile, username, title, description, redirectpage, point, usertype, out pnttbl, drow["idate"].ToString()).ToString();
                    dr["Type"] = drow["UserType"].ToString();
                    dr["Point"] = pnttbl.ToString();
                    dt.Rows.Add(dr);

                    edr["Name"] = username.ToString();
                    edr["Incident"] = title;
                    edr["Description"] = description;
                    edr["Type"] = drow["UserType"].ToString();
                    edr["IncidentDate"] = drow["idate"].ToString();
                    edr["CreatedDate"] = drow["cdate"].ToString();
                    edr["CreatedBy"] = drow["CreatedBy"].ToString();
                    edr["Point"] = drow["Point"].ToString();
                    edt.Rows.Add(edr);


                }
                ViewState["IncidentList"] = incidentDataset;
                ViewState["ExcelData"] = ExcelDataset;

                lbl_srch_cnt.Text = incidentDataset.Tables[0].Rows.Count+ " ";
                lbl_srch_ok.Text = " Records Found";

                lbl_srch_ok.Visible=true;
                img_export_Excel.Visible=true;
                img_rslt.Visible = true;
                lbl_srch_cnt.Visible = true;
            }
            else
            {
                lbl_srch_ok.Visible=false;
                img_export_Excel.Visible=false;
                img_rslt.Visible = false;
                lbl_srch_cnt.Visible = false;
                lbl_srch.Text = "Your search - " + Txt_Search.Text + " - did not match any incidents... !";
            }
            return incidentDataset;
            
        }

        private string getInfo(int incidentId, string imagefile, string username, string title, string description, string redirectpage, int point, string usertype, out string pointtbl,string _IncidentDate)
        {
            string img = "";
            if (point > 0)
            {
                img = "<table><tr><td><img src=\"images/pt1_up.png\" alt=\"" + username + "\" style=\"width:30px;height:30px\" /></td><td  style=\" color:Green;\">" + point + " Points</td></tr></table>";
            }
            else
                if (point < 0)
                {
                    img = "<table><tr><td><img src=\"images/pt1_dwn.png\" alt=\"" + username + "\" style=\"width:30px;height:30px\" /></td><td  style=\" color:Red;\">" + point + " Points</td></tr></table>";
                }
                else
                {
                    img = "<table><tr><td></td><td>" + point + " Points</td></tr></table>";
                }
            pointtbl = img;
            StringBuilder infostring = new StringBuilder("");
            infostring.Append("  <table width=\"100%\" ><tr><td align=\"center\"> <table width=\"100%\" >    <tr>    <td style=\"width:15%;\">");
            infostring.Append("<table>    <tr> <td align=\"center\">        <a href= '" + redirectpage + "'> <img src=\"" + imagefile + "\" alt=\"" + username + "\" style=\"width:75px;height:75px;border-style:solid;border-width:1px;border-color:Black;\" /> </a>    </td>    </tr>");
            //infostring.Append("<tr>    <td align=\"center\">    <a href= '" + redirectpage + "'> " + username + " </a>    </td>    </tr>");
            infostring.Append("<tr>    <td align=\"center\">     <a href= '" + redirectpage + "&Type=1'> More Incidents </a>    </td>    </tr>    ");
            infostring.Append("</table>    </td>      ");
            infostring.Append("<td style=\"width:85%;\" align=\"left\" valign= \"middle\" >    ");
            infostring.Append("<table width=\"95%\" >  <tr>    <td  align=\"left\" style=\"width:50%;\">    <a style=\"text-decoration:none;color:Red \" href= '" + redirectpage + "'> <h4> " + username + " </h4> </a> </td> <td valign=\"bottom\" align=\"right\" style=\"font-size:smaller;\">  " + usertype + "   </td>   </tr>  <tr>    <td valign=\"top\" colspan=\"2\" >   <h4>" + title + "</h4>   <div id=\"Div5\" runat=\"server\">Incident Date: " + _IncidentDate + "</div> </td> </tr>");

            infostring.Append("<tr><td> <div id=\"Div3\" class=\"linestyle\" runat=\"server\"></div></td> </tr>");
            infostring.Append(" <tr>    <td colspan=\"2\" >     " + description + "   </td>     </tr>    </table>");
            infostring.Append("  </td>    </tr>    </table>    </td></tr></table>");
            return infostring.ToString();
        }

        protected void Grd_Incedent_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(Grd_Incedent.SelectedRow.Cells[0].Text.ToString());
            string type = Grd_Incedent.SelectedRow.Cells[2].Text.ToString();
            if (type.ToLowerInvariant() == "student")
            {
                Session["StudId"] = int.Parse(Grd_Incedent.SelectedRow.Cells[1].Text.ToString());
            }
            else if (type.ToLowerInvariant() == "staff")
            {
                Session["StaffId"] = int.Parse(Grd_Incedent.SelectedRow.Cells[1].Text.ToString());
                type = "Staff";
            }

            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('ViewIncidence.aspx?id=" + id + "&Type=" + type + "');", true);
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            MydataSet = (DataSet)ViewState["ExcelData"];
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                //if (!WinEr.ExcelUtility.ExportDataSetToExcel(MydataSet, "Incidents Report.xls"))
                //{
                //}
                string FileName = "Incidents-Report";
                string _ReportName = "Incidents-Report";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {

                }
            }
        }

    }
}
