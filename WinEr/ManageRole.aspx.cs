using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using System.Text;
public partial class ManageRole : System.Web.UI.Page
{
    private KnowinRole MyRole;
    private OdbcDataReader MyReader = null;
    private OdbcDataReader MyReader1 = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        KnowinUser User = (KnowinUser)Session["UserObj"];
        MyRole = User.GetRoleObj();
        if (MyRole == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
                //some initlization
                AddRoleToCbList(0);
                AddModuleToCbList(0);
                AddActionToCheckBoxList();
            }
        }
    }
 
    private void AddRoleToCbList(int _intex)
    {
        Drp_Role.Items.Clear();
        string sql = "SELECT Id,RoleName FROM tblrole";
        MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Role.Items.Add(li);
                Drp_Role.SelectedIndex = _intex;
            }
        }
    }
   
    private void AddModuleToCbList(int _intex)
    {

        Drp_Module.Items.Clear();
        string sql = "SELECT Id,ModuleName FROM tblmodule where IsActive=1 ";
        MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Module.Items.Add(li);
                Drp_Module.SelectedIndex = _intex;
            }
        }

    }
  
    private void AddActionToCheckBoxList()
    {

        ChkBoxRoleAction.Items.Clear();
        ChkBoxModuAction.Items.Clear();
        DataSet myDataset;
        DataSet myDataset1;
        string sql1 = "SELECT  tblaction.Id, tblaction.ActionName FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + int.Parse(Drp_Role.SelectedValue.ToString());
        string sql = "SELECT  tblaction.Id, tblaction.ActionName FROM tblaction INNER JOIN  tblmoduleactionmap ON tblaction.Id = tblmoduleactionmap.ActionId WHERE  tblmoduleactionmap.ModuleId=" + int.Parse(Drp_Module.SelectedValue.ToString());
        myDataset1 = MyRole.m_MysqlDb.ExecuteQuery(sql1,"RoleAction");
        myDataset = MyRole.m_MysqlDb.ExecuteQuery(sql, "ModuleActions");
        if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
        {
            
            foreach (DataRow dr in myDataset.Tables[0].Rows)
            {
                if (MyRole.IsRoleAction(myDataset1, int.Parse(dr[0].ToString())))
                {
                }
                else
                {
                ListItem li = new ListItem(dr[1].ToString(),dr[0].ToString());
                ChkBoxModuAction.Items.Add(li);
                }
            }
        }
        if (myDataset1 != null && myDataset1.Tables != null && myDataset1.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in myDataset1.Tables[0].Rows)
            {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    ChkBoxRoleAction.Items.Add(li);
            }
        }
               
    

    }

    protected void Drp_Module_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddActionToCheckBoxList();        
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddActionToCheckBoxList();
    }
  
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sql;
        for (int i = 0; i < ChkBoxModuAction.Items.Count; i++)
        {
            if (ChkBoxModuAction.Items[i].Selected)
            {
                sql = "INSERT INTO tblroleactionmap (RoleId,ActionId,ModuleId) VALUES (" + int.Parse(Drp_Role.SelectedValue.ToString()) + ", " + int.Parse(ChkBoxModuAction.Items[i].Value.ToString()) + ", " + int.Parse(Drp_Module.SelectedValue.ToString()) + ")";
                MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
                
            }
            
        }
        AddActionToCheckBoxList();
    }
  
    protected void Btn_Remove_Click(object sender, EventArgs e)
    {
        string sql;
        for (int i = 0; i < ChkBoxRoleAction.Items.Count; i++)
        {
            if (ChkBoxRoleAction.Items[i].Selected)
            {
                sql = "DELETE FROM tblroleactionmap WHERE RoleId =" + int.Parse(Drp_Role.SelectedValue.ToString()) + " AND ActionId=" + int.Parse(ChkBoxRoleAction.Items[i].Value.ToString());
                MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
                
            }
        }
        AddActionToCheckBoxList();
    }

    protected void Img_Excel_Click(object sender, ImageClickEventArgs e)
    {
        string FileName = "RoleActionDetails";
        // FileName = FileName + "Exam Report.xls";
        // ClassTeacherReport.InnerHtml = MyExamMang.GetClassTeacherReport(int.Parse(Drp_Class.SelectedValue), ExamId);
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
        Response.Write(GetRoleActionDetailsForPrintOut());
        Response.Write("</head>");
        Response.Flush();
        Response.End();
    }

    private string GetRoleActionDetailsForPrintOut()
    {
        StringBuilder CTR = new StringBuilder();

        CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

        CTR.Append("<tr>");
        string sql = "SELECT tblrole.RoleName, tblrole.Id from tblrole";
        MyReader = MyRole.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                CTR.Append("<td valign=\"top\"><table runat=\"server\" width=\"100%\"><tr><td><b>" + MyReader.GetValue(0).ToString() + "</b></td></tr><tr><td></td></tr>");

                string sql1 = "select tblaction.ActionName from tblaction inner join tblroleactionmap on tblaction.Id= tblroleactionmap.ActionId where tblroleactionmap.RoleId="+MyReader.GetValue(1).ToString();
                MyReader1 = MyRole.m_MysqlDb.ExecuteQuery(sql1);
                if (MyReader1.HasRows)
                {
                    while (MyReader1.Read())
                    {
                        CTR.Append("<tr><td>" + MyReader1.GetValue(0).ToString()+"</td></tr>");
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
}
