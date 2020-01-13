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
using WinBase;

namespace WinEr
{
    public partial class Rackwisereport : System.Web.UI.Page
    {
        private LibraryManagClass MyLibMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyLibMang = MyUser.GetLibObj();

            if (MyLibMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(3029))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadRack();
                }
            }
        }

        private void LoadRack()
        {

            MyReader = MyLibMang.GetRack();
            Drp_Rack.Items.Clear();
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Rack.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Rack found", "-1");
                Drp_Rack.Items.Add(li);
            }
        }
        protected void Btn_ShowReport_Click(object sender, EventArgs e)
        {
            int RackId = -1;
            int.TryParse(Drp_Rack.SelectedValue.ToString(), out RackId);        
            LoadBookDetails(RackId);
        }
        protected void GrdVew_Books_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdVew_Books.PageIndex = e.NewPageIndex;
            GrdVew_Books.DataBind();
        }

        protected void img_export_Excel_Click(object sender, ImageClickEventArgs e)
        {
            MydataSet = (DataSet)Session["BookDetails"];
            if (MydataSet.Tables[0].Columns.Contains("Id"))
            {
                MydataSet.Tables[0].Columns.Remove("Id");
            }
            if (MydataSet != null && MydataSet.Tables[0] != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                string _ReportName = "RackwiseReport";
                string FileName = "RackwiseReport";
                if (!WinEr.ExcelUtility.ExportDataToExcel(MydataSet, _ReportName, FileName, MyUser.ExcelHeader))
                {
                    WC_MessageBox.ShowMssage("Please try again...");
                }
            }
         
        }
        private void LoadBookDetails(int RackId)
        {
            DataSet BooksName = MyLibMang.GetBooksFromRackId(RackId);

            DataSet BookDetails = new DataSet();
            DataTable dt;
            DataRow dr;

            BookDetails.Tables.Add(new DataTable("Bookdata"));
            dt = BookDetails.Tables["Bookdata"];
            dt.Columns.Add("Id");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Publisher");
            dt.Columns.Add("Year");
            dt.Columns.Add("RackName");
            if (BooksName != null && BooksName.Tables != null && BooksName.Tables[0].Rows.Count > 0)
            {
               
                foreach (DataRow dr0 in BooksName.Tables[0].Rows)
                {
                    dr = dt.NewRow();
                    dr["Id"] = dr0["Id"].ToString();
                    dr["BookName"] = dr0["BookName"].ToString();
                    string Count = MyLibMang.GetBookCount(int.Parse(dr["Id"].ToString()));
                    dr["Author"] = dr0["Author"].ToString(); ;
                    dr["Publisher"] = dr0["Publisher"].ToString(); ;
                    dr["Year"] = dr0["Year"].ToString(); ;
                    dr["RackName"] = dr0["RackName"].ToString(); ;
                    BookDetails.Tables["Bookdata"].Rows.Add(dr);
                }
                GrdVew_Books.DataSource = BookDetails;
                HttpContext.Current.Session["BookDetails"] = BookDetails;
                GrdVew_Books.DataBind();
            }
            else
            {
                Lbl_CategoryErr.Text = "No Books found";
                GrdVew_Books.DataSource = null;
                GrdVew_Books.DataBind();
            }

           
        }
       
      
    }
}
