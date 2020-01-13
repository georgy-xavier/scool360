using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Text;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Drawing;
using WinBase;
using System.Web.UI.HtmlControls;
using System.IO;

namespace WinEr
{
    public partial class invbufferbill : System.Web.UI.Page
    {
        private WinBase.Inventory Myinventory;
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        int studId;
        int classId;
        OdbcDataReader Myreader = null;
        int BillNum = 0;
        string billno = "";
        double totalcost = 0;
        private string m_PdfName = "";

        string     PAGEWIDTH   = "" ;
        int     ITEMCOUNT   = 0;
        string  FONTFAMILY  = "";
        string  FONTSIZE    = "";
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            Myinventory = MyUser.GetInventoryObj();
            

            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }


            if (MyConfigMang == null)
            {
                Response.Redirect("Default.aspx");
                //no rights for this user.
            }
            else
            {
                Err.InnerHtml="";
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (Request.QueryString["PDF"] != null)
                {
                    m_PdfName = Request.QueryString["PDF"].ToString();
                  
                    loadBillToPage(m_PdfName);
                  
                }
                else if (!IsPostBack)
                {

                    if (Request.QueryString["className"].ToString() != null && Request.QueryString["Type"].ToString() != null && Request.QueryString["StudName"].ToString() != null && Request.QueryString["studid"].ToString() != null && Request.QueryString["classId"].ToString() != null && Request.QueryString["totalcost"].ToString() != null)
                    {
                        string ClassName = Request.QueryString["className"].ToString();
                        int Type = int.Parse(Request.QueryString["Type"].ToString());
                        string studName = Request.QueryString["StudName"].ToString();
                        studId = int.Parse(Request.QueryString["studid"].ToString());
                        classId = int.Parse(Request.QueryString["classId"].ToString());
                        totalcost = double.Parse(Request.QueryString["totalcost"].ToString());
                        string S_LogoUrl; string s_Name; string Address;


                        Err.InnerHtml = "";

                        string BillId = LoadBillId();
                        LoadSchoolDetails(out  S_LogoUrl, out  s_Name, out  Address);
                        LoadBllDtails(ClassName, studName, studId, classId, totalcost, BillId, S_LogoUrl, s_Name, Address);
                       
                    }
                }
                else
                {
                    Err.InnerHtml = "Cannot create recept";
                }
            }
        }

        private void LoadSchoolDetails(out string S_LogoUrl, out string s_Name, out string Address)
        {
            S_LogoUrl = ""; s_Name = ""; Address = "";
            string Sql = "SELECT SchoolName,Address,LogoUrl FROM tblschooldetails WHERE Id=1";
           OdbcDataReader  MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(Sql);

            if (MyReader.HasRows)
            {
                S_LogoUrl = MyReader.GetValue(2).ToString();
                s_Name = MyReader.GetValue(0).ToString();
                Address = MyReader.GetValue(1).ToString();
            }
        }

        private string LoadBillId()
        {
            string sql = "select Max(Id) from tblinv_viewissuebill";
            Myreader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            string BillNumber = "";
            if (Myreader.HasRows)
            {
                int.TryParse(Myreader.GetValue(0).ToString(), out BillNum);
                BillNumber = "BL:" + BillNum;

            }
            else
            {
                BillNum = 1;
                BillNumber = "BL:" + BillNum;
            }
            return BillNumber;
        }

        private void LoadBllDtails(string ClassName, string studName, int studId, int classId, double totalcost, string BillId,string C_LogoUrl, string C_Name, string C_Address)
        {
            /*Buffer Printing Parameters*/

            DataSet TemplateSet;

            StringBuilder bufferString = new StringBuilder();

            string sql = "";
            string Templates = "";
            string Err = "";
            
            sql = "select tblinv_billtemplates.Template  from tblinv_billtemplates where tblinv_billtemplates.IsActive=1 and tblinv_billtemplates.TemplateName='INV BILL FORMAT' ";
            TemplateSet = MyConfigMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (TemplateSet != null && TemplateSet.Tables[0].Rows.Count > 0)
            {
                Templates = TemplateSet.Tables[0].Rows[0][0].ToString();
                Templates = Templates.Replace("\n", "");
                Templates = Templates.Replace("\r", "");
            }

            if (Templates != "")
            {
                if (CreateBufferFormat(Templates, bufferString, ClassName,studName, studId, classId, totalcost, BillId, out  Err,C_LogoUrl,  C_Name,  C_Address))
                {

                    Document document = LoadPDFPage(bufferString, FONTFAMILY, PAGEWIDTH, FONTSIZE);
                    //    {CreateDocument();
                    const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

                    // ----------------------------------------------------------------------------------------

                    const bool unicode = false;

                    // Create a renderer for the MigraDoc document.
                    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

                    // Associate the MigraDoc document with a renderer
                    pdfRenderer.Document = document;

                    // Layout and render document to PDF

                    pdfRenderer.RenderDocument();

                    // Save the document...
                    string filename = "", MainName = "";
                    MainName = "inv_" + studName + System.DateTime.Now.Second;
                    filename = MyUser.FilePath + "\\PDF_Files\\" + MainName + ".pdf";

                    pdfRenderer.PdfDocument.Save(filename);
                    string FilenameForsaveinDB=MainName + ".pdf";
                    // ...and start a viewer.
                    string sqlq = "insert into tblinv_viewissuebill(Report,Date,StudId,ClassId,BillType) values('" + FilenameForsaveinDB + "','" + System.DateTime.Now.Date.ToString("s") + "'," + studId + "," + classId + ",1)";
                    Myinventory.m_MysqlDb.ExecuteQuery(sqlq);

                    //  ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?ptintbuffer=Fee_" + StudentName + ".pdf\");", true);
                    Response.Redirect("invbufferbill.aspx?PDF=" + MainName + ".pdf", true);

                }
            }
            else
            {
                // this.FeeDetails.InnerHtml = "Error: Error in reading templates. Please contact to Support Team";
            }
        }
        
        private Document LoadPDFPage(StringBuilder bufferString, string fontfamily, string CharCount, string size)
        {
            Document document = new Document();
            PageSetup PgSt = document.DefaultPageSetup;

            PgSt.LeftMargin = 10;
            PgSt.RightMargin = 0;
            PgSt.TopMargin = 10;
            PgSt.BottomMargin = 0;

            PgSt.HeaderDistance = 0;
            PgSt.FooterDistance = 0;

            PgSt.PageWidth = 450;
            PgSt.PageHeight = 400;

            Section section = document.AddSection();

            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.Font.Color = MigraDoc.DocumentObjectModel.Color.FromCmyk(100, 30, 20, 50);

            FontFamily fontFamily = new FontFamily("Lucida Console");
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font(fontfamily, size);

            //  FontFamily.Families.   

            paragraph.Format.Font = font;
            char[] myCr = bufferString.ToString().ToCharArray();

            foreach (char _ch in myCr)
            {
                if (_ch == ' ')
                    paragraph.AddSpace(1);
                else
                    paragraph.AddChar(_ch);
            }

            return document;
        }

        private bool CreateBufferFormat(string Templates, StringBuilder bufferString, string ClassName, string studName, int studId, int classId, double totalcost, string BillId, out string Err, string C_LogoUrl, string C_Name, string C_Address)
        {
            StringBuilder _bufferString = new StringBuilder();
           

            string ErrorMessage = "";
            string total = "";
            string amountinwords = "";

            string[] SPLITMAIN = new string[] { " #SPLITMAIN# " };
            string[] MainString = Templates.Split(SPLITMAIN, StringSplitOptions.RemoveEmptyEntries);

            int mainStringLen = 0;
         
            bool CreationFlow = true;
  
            while (mainStringLen < MainString.Length)
            {

                string[] subString;
                string[] SPLITSUB;


                if (!MainString[mainStringLen].StartsWith("<repeat>"))
                {
                    SPLITSUB = new string[] { " #SPLITSUB# " };


                    subString = MainString[mainStringLen].Split(SPLITSUB, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    subString = new string[1];
                    subString[0] = MainString[mainStringLen];
                    CreationFlow = setRepeateItems(subString[0], PAGEWIDTH, bufferString, out ErrorMessage, out total, out amountinwords);

                }

                int substringLen = 0;


                switch (subString[substringLen])
                {
                    case "<PARAMETER>":
                        CreationFlow = GetParameters(subString[1], out ErrorMessage);
                        break;

                    case "<RepeatChar>":
                        CreationFlow = GetRepeatedItem(subString[1], PAGEWIDTH, bufferString, out ErrorMessage);
                        break;

                    case "<Br>":
                        CreationFlow = GetNewLine(subString[0], PAGEWIDTH, bufferString, out ErrorMessage);
                        break;


                    case "<Seperator>":
                        CreationFlow = SetseprationWord(subString[1], PAGEWIDTH, bufferString, out ErrorMessage, ClassName, studName, BillId, C_Name, C_Address, "", "", General.GerFormatedDatVal(System.DateTime.Now.Date), "", "", "", "", amountinwords, total);

                        break;

                    case "<Letter>":

                        CreationFlow = SetCharecter(subString[1], PAGEWIDTH, bufferString, out ErrorMessage);

                        break;

                    case "<repeat>":

                        CreationFlow = setRepeateItems(subString[0], PAGEWIDTH, bufferString, out ErrorMessage, out total, out amountinwords);
                     

                        break;

                    default:
                        break;

                }
                if (ErrorMessage != "" || CreationFlow == false)
                {
                    Err = ErrorMessage;
                    return false;

                }

                mainStringLen++;
            }
            Err = "";


         

            return true;
        }

        private bool SetseprationWord(string sentance, string CharCount, StringBuilder bufferString, out string ErrorMessage, string ClassName, string StudentName, string BillNo, string C_Name, string Address, string RollNo, string AdmissionNo, string date, string serialNo, string _ItemName, string IemQty, string ItemAmount, string amountinwords, string total)
        {
            ErrorMessage = "";

            string[] subString = sentance.Split(',');

            switch (subString[0])
            {
                case "($clgname$)":

                    if (C_Name.Length > int.Parse(subString[1]))
                    {
                        C_Name = C_Name.Substring(0, int.Parse(subString[1]) - 1);
                    }
                    else if (C_Name.Length < int.Parse(subString[1]))
                    {
                        C_Name = addNewSpace(C_Name, int.Parse(subString[1]));
                    }

                    bufferString.Append(C_Name);

                    break;

                case "($clgadd$)":

                    if (Address.Length > int.Parse(subString[1]))
                        Address = Address.Substring(0, int.Parse(subString[1]) - 1);
                    else if (Address.Length < int.Parse(subString[1]))
                    {
                        Address = addNewSpace(Address, int.Parse(subString[1]));
                    }
                    Address = Address.Replace("\r\n", ",");
                    Address = Address.Replace("\n", ",");
                    Address = Address.Replace("\r", ",");
                    Address = Address.Replace(",,", ",");
                    bufferString.Append(Address);

                    break;

                case "($StudentName$)":

                    if (StudentName.Length > int.Parse(subString[1]))
                        StudentName = StudentName.Substring(0, int.Parse(subString[1]) - 1);
                    else if (StudentName.Length < int.Parse(subString[1]))
                    {
                        StudentName = addNewSpace(StudentName, int.Parse(subString[1]));
                    }

                    bufferString.Append(StudentName);

                    break;

                case "($RollNo$)":

                    if (RollNo.Length > int.Parse(subString[1]))
                        RollNo = RollNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (RollNo.Length < int.Parse(subString[1]))
                    {
                        addNewSpace(RollNo, int.Parse(subString[1]));
                    }

                    bufferString.Append(RollNo);

                    break;

                case "($Class$)":

                    if (ClassName.Length > int.Parse(subString[1]))
                        ClassName = ClassName.Substring(0, int.Parse(subString[1]) - 1);
                    else if (ClassName.Length < int.Parse(subString[1]))
                    {
                        ClassName = addNewSpace(ClassName, int.Parse(subString[1]));
                    }

                    bufferString.Append(ClassName);

                    break;

                case "($ReceiptNo$)":

                    if (BillNo.Length > int.Parse(subString[1]))
                        BillNo = BillNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (BillNo.Length < int.Parse(subString[1]))
                    {
                        BillNo = addNewSpace(BillNo, int.Parse(subString[1]));
                    }
                    bufferString.Append(BillNo);

                    break;

                case "($AdmNo$)":

                    if (AdmissionNo.Length > int.Parse(subString[1]))
                        AdmissionNo = AdmissionNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (AdmissionNo.Length < int.Parse(subString[1]))
                    {
                        AdmissionNo = addNewSpace(AdmissionNo, int.Parse(subString[1]));
                    }
                    bufferString.Append(AdmissionNo);

                    break;

                case "($Date$)":
                    if (date.Length > int.Parse(subString[1]))
                        date = date.Substring(0, int.Parse(subString[1]) - 1);
                    else if (date.Length < int.Parse(subString[1]))
                    {
                        date = addNewSpace(date, int.Parse(subString[1]));
                    }
                    bufferString.Append(date);

                    break;

                case "($SINO$)":

                    if (serialNo.Length > int.Parse(subString[1]))
                        serialNo = serialNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (serialNo.Length < int.Parse(subString[1]))
                    {
                        serialNo = addNewSpace(serialNo, int.Parse(subString[1]));
                    }
                    bufferString.Append(serialNo);

                    break;

                case "($PARTICULARES$)":
                   
                    if (_ItemName.Length > int.Parse(subString[1]))
                    {
                        _ItemName = _ItemName.Substring(0, int.Parse(subString[1]) - 1);

                    }
                    else if (_ItemName.Length < int.Parse(subString[1]))
                    {
                        _ItemName = addNewSpace(_ItemName, int.Parse(subString[1]));
                    }

                    bufferString.Append(_ItemName);

                    break;

                case "($QTY$)":

                    if (IemQty.Length > int.Parse(subString[1]))
                        IemQty = IemQty.Substring(0, int.Parse(subString[1]) - 1);
                    else if (IemQty.Length < int.Parse(subString[1]))
                    {
                        IemQty = addNewSpace(IemQty, int.Parse(subString[1]));
                    }
                    bufferString.Append(IemQty);

                    break;

                case "($AMOUNT$)":

                    //if (_Period.Length > int.Parse(subString[1]))
                    //    _Period = _Period.Substring(0, int.Parse(subString[1]) - 1);

                    bufferString.Append(ItemAmount);

                    break;

                case "($IN WORDS$)":

                    bufferString.Append(amountinwords);

                    break;

                case "($total$)":

                    bufferString.Append(total);

                    break;

                case "($Batch$)":

                    bufferString.Append(MyUser.CurrentBatchName);

                    break;



            }
            return true;

        }

        private bool setRepeateItems(string sentance, string _pageWidth, StringBuilder bufferString,  out string ErrorMessage, out string total, out string amountinwords)
        {
            ErrorMessage = "";
            DataSet Issuereceipt = (DataSet)Session["IssueReceipt"];
            double _total=0;
            int count=1;
            string[] SPLITSUB;
            string IssuedCount = "", BookName = "", Cost = "";
            int TotalCount = 0;
            int BillType = PDFBILLType();
           

            if (Issuereceipt != null && Issuereceipt.Tables != null && Issuereceipt.Tables[0].Rows.Count > 0)
            {
                Issuereceipt = GetNewDataSet(Issuereceipt, BillType);
                TotalCount = Issuereceipt.Tables[0].Rows.Count;

               
                sentance = sentance.Replace("<repeat>", "");
                string[] SPLITMAIN = new string[] { " #REPEATMAIN# " };

                int mainStringLen = 0;

                bool CreationFlow = true;
                int tempcount=1;
                foreach (DataRow dr in Issuereceipt.Tables[0].Rows)
                {
                    if (tempcount <= ITEMCOUNT)
                    {
                        string[] MainString = sentance.Split(SPLITMAIN, StringSplitOptions.RemoveEmptyEntries);


                        BookName = dr["BookName"].ToString();

                        IssuedCount = dr["IssuedCount"].ToString();
                        Cost = dr["Cost"].ToString();


                        if ("Discount" != dr[2].ToString())
                        {
                            _total = _total + double.Parse(Cost.ToString());
                        }

                        while (mainStringLen < MainString.Length)
                        {

                            string[] subString;
                            SPLITSUB = new string[] { " #SPLITSUB# " };
                            subString = MainString[mainStringLen].Split(SPLITSUB, StringSplitOptions.RemoveEmptyEntries);


                            int substringLen = 0;



                            switch (subString[substringLen])
                            {

                                case "<RepeatChar>":
                                    CreationFlow = GetRepeatedItem(subString[1], _pageWidth, bufferString, out ErrorMessage);
                                    break;

                                case "<Br>":
                                case "</repeat>":
                                    CreationFlow = GetNewLine("<Br>", _pageWidth, bufferString, out ErrorMessage);
                                    break;


                                case "<Seperator>":
                                    CreationFlow = SetseprationWord(subString[1], _pageWidth, bufferString, out ErrorMessage, "", "", "", "", "", "", "", "", count.ToString(), BookName, IssuedCount, Cost, "", "");

                                    break;

                                case "<Letter>":

                                    CreationFlow = SetCharecter(subString[1], _pageWidth, bufferString, out ErrorMessage);

                                    break;

                                default:
                                    break;

                            }

                            mainStringLen++;

                        }
                        count = count + 1;
                        CreationFlow = GetNewLine("<Br>", _pageWidth, bufferString, out ErrorMessage);
                        mainStringLen = 0;
                        tempcount++;

                    }
                }
                if (ITEMCOUNT > TotalCount)
                {
                    for (int i = 0; i < (ITEMCOUNT - TotalCount); i++)
                    {
                        CreationFlow = GetNewLine("<Br>", _pageWidth, bufferString, out ErrorMessage);
                        CreationFlow = GetNewLine("<Br>", _pageWidth, bufferString, out ErrorMessage);
                    }
                }
                

            }

            else
            {
                total = ""; amountinwords = "";
                ErrorMessage = "No items for printing";
                return false;

            }




            total = _total.ToString();
            amountinwords = Convert_Number_To_Words(int.Parse(_total.ToString()));

            return true;
        }

        private DataSet GetNewDataSet(DataSet Issuereceipt, int BillType)
        {
            DataRow _dr;
            if (BillType == 1)
            {
                return Issuereceipt;
            }
            else if (BillType == 2)
            {
                DataSet NewDt = getDataSet();

                DataView dv = Issuereceipt.Tables[0].DefaultView; // DataSet to DataView 
                dv.Sort = "CategoryId DESC";  // We can able to add more columns in this
                DataTable dt = dv.Table;


                string CategoryName = "";
                int Count = 0;
                double Rate = 0;
                int TempCOunt = 0;
                int CategoryId = 0;
                int TempCategoryId = 0;
                int schedultedCount = 0;
                double BasicCost = 0;
                int temprow = 0;

                foreach (DataRowView dr in dv.Table.DefaultView)
                {
                    if (TempCategoryId != 0 && (TempCategoryId != Convert.ToInt32(dr["CategoryId"].ToString()) || dr["SpecialItem"].ToString() == "1"))
                    {
                        _dr = NewDt.Tables["IssueReceipt"].NewRow();

                        _dr["BookName"] = CategoryName;
                        _dr["ScheduledCount"] = schedultedCount;
                        _dr["IssuedCount"] = Count;
                        _dr["BasicCost"] = BasicCost;
                        _dr["Category"] = CategoryName;
                        _dr["Cost"] = Rate;
                        _dr["BookId"] = TempCOunt;
                        _dr["CategoryId"] = CategoryId;

                        NewDt.Tables["IssueReceipt"].Rows.Add(_dr);

                        TempCOunt = TempCOunt + 1;
                        CategoryName = "";
                        schedultedCount = 0;
                        Count = 0;
                        BasicCost = 0;
                        Rate = 0;

                        CategoryId = 0;
                    }
                    if (dr["SpecialItem"].ToString() == "1")
                    {
                        _dr = NewDt.Tables["IssueReceipt"].NewRow();

                        _dr["BookName"] = dr["BookName"].ToString();
                        _dr["ScheduledCount"] = dr["ScheduledCount"].ToString();
                        _dr["IssuedCount"] = dr["IssuedCount"].ToString();
                        _dr["BasicCost"] = dr["BasicCost"].ToString();
                        _dr["Category"] = dr["Category"].ToString();
                        _dr["Cost"] = dr["Cost"].ToString() ;
                        _dr["BookId"] = dr["BookId"].ToString();
                        _dr["CategoryId"] = dr["CategoryId"].ToString(); 

                        NewDt.Tables["IssueReceipt"].Rows.Add(_dr);
                        temprow = temprow + 1;
                    }

                    else
                    {

                        temprow = temprow + 1;
                        CategoryName = dr["Category"].ToString();
                        Count = Count + Convert.ToInt32(dr["IssuedCount"].ToString());
                        schedultedCount = schedultedCount + Convert.ToInt32(dr["ScheduledCount"].ToString());
                        Rate = Rate + Convert.ToDouble(dr["Cost"].ToString());
                        CategoryId = Convert.ToInt32(dr["CategoryId"].ToString());
                        BasicCost = BasicCost + Convert.ToDouble(dr["BasicCost"].ToString());
                    }

                    if (temprow == dv.Table.Rows.Count)
                    {

                        _dr = NewDt.Tables["IssueReceipt"].NewRow();

                        _dr["BookName"] = CategoryName;
                        _dr["ScheduledCount"] = schedultedCount;
                        _dr["IssuedCount"] = Count;
                        _dr["BasicCost"] = BasicCost;
                        _dr["Category"] = CategoryName;
                        _dr["Cost"] = Rate;
                        _dr["BookId"] = TempCOunt;
                        _dr["CategoryId"] = CategoryId;

                        NewDt.Tables["IssueReceipt"].Rows.Add(_dr);

                    }
                    TempCategoryId = CategoryId;
                }

                return NewDt;
            }
            else if (BillType == 3)
            {
                DataSet NewDt = getDataSet();

                foreach (DataRow dr in Issuereceipt.Tables[0].Rows)
                {
                    _dr = NewDt.Tables["IssueReceipt"].NewRow();

                    _dr["BookName"] = dr["BookName"] + "( " + dr["Category"] + ")";
                    _dr["ScheduledCount"] = dr["ScheduledCount"];
                    _dr["IssuedCount"] = dr["IssuedCount"];
                    _dr["BasicCost"] = dr["BasicCost"];
                    _dr["Category"] = dr["Category"];
                    _dr["Cost"] = dr["Cost"];
                    _dr["BookId"] = dr["BookId"];
                    _dr["CategoryId"] = dr["CategoryId"];

                    NewDt.Tables["IssueReceipt"].Rows.Add(_dr);
                }
                return NewDt;
            }
            else  return null;
        }
   

        private DataSet getDataSet()
        {
            DataSet IssuereportDs = new DataSet();
            DataTable _dt;
         
            IssuereportDs.Tables.Add(new DataTable("IssueReceipt"));
            _dt = IssuereportDs.Tables["IssueReceipt"];
            _dt.Columns.Add("BookName");
            _dt.Columns.Add("ScheduledCount");
            _dt.Columns.Add("IssuedCount");
            _dt.Columns.Add("TotalScheduledCount");
            _dt.Columns.Add("BookId");
            _dt.Columns.Add("BasicCost");
            _dt.Columns.Add("Cost");
            _dt.Columns.Add("Category");
            _dt.Columns.Add("CategoryId");
            return IssuereportDs;
        }

        private string addNewSpace(string String, int count)
        {
            if (String.Length < count)
            {
                int spacecount = (count - (String.Length));
                for (int i = 0; i < spacecount; i++)
                {
                    String = String + " ";
                }
            }
            else
            {
                if (String.Length > count)
                    String = String.Substring(0, count);
            }


            return String;
        }

        private bool SetCharecter(string sentance, string _pageWidth, StringBuilder bufferString, out string ErrorMessage)
        {
            ErrorMessage = "";

            if (sentance.Length > int.Parse(_pageWidth))
            {
                sentance = sentance.Substring(0, sentance.Length - 1);

            }

            bufferString.Append(sentance);

            return true;
        }

        private bool GetNewLine(string sentance, string _pageWidth, StringBuilder bufferString, out string ErrorMessage)
        {
            ErrorMessage = "";
            bufferString.Append("\n");

            return true;
        }

        private bool GetRepeatedItem(string sentance, string _pageWidth, StringBuilder bufferString, out string ErrorMessage)
        {
            ErrorMessage = "";
            string[] subString = sentance.Split(',');
            int repeateCount = 0;

            if (subString[1] == "" || int.Parse(subString[1]) > int.Parse(_pageWidth))
            {
                repeateCount = int.Parse(_pageWidth);
            }
            else
            {
                repeateCount = int.Parse(subString[1]);
            }

            for (int i = 0; i < repeateCount; i++)
            {
                if (subString[0] == "" || subString[0] == " ")
                    bufferString.Append(" ");
                else
                    bufferString.Append(subString[0]);

            }

            return true;

        }

        private bool GetParameters(string sentance, out string message)
        {
            string[] subString = sentance.Split(',');
            message = "";

         
         
            if (subString[0] == "CHARCOUNT" && subString[1] != "")
            {
                PAGEWIDTH = subString[1];
                return true;
            }
            else if (subString[0] == "FONTSIZE" && subString[1] != "")
            {
                FONTSIZE = subString[1];
                return true;
            }
            else if (subString[0] == "FONT" && subString[1] != "")
            {
                FONTFAMILY = subString[1];
                return true;
            }
            else if (subString[0] == "ITEMCOUNT" && subString[1] != "")
            {
                ITEMCOUNT = Convert.ToInt32(subString[1].ToString());
                return true;
            }
            else
            {
                message = "Some parameters are mission or wrong parameters";
                return false;
            }

        }

        private void loadBillToPage(string _PdfName)
        {

            string _physicalpath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath("")) + "\\PDF_Files\\" + _PdfName;
            string __pdfl = _physicalpath;
            string fileExtension = System.IO.Path.GetExtension(".pdf").ToLower();
            string ContentType = "application/msword";
            if (fileExtension == ".pdf")
            {
                ContentType = "application/pdf";
            }
            //string __pdfl = "PDF_Files/Quotation" + Lbl_qtId.Text + ".pdf";

            if (File.Exists(__pdfl))
            {
                if (!IsFileInUse(__pdfl))
                {
                    FileStream fs = File.Open(__pdfl, FileMode.Open, FileAccess.Read);
                    Stream output = Response.OutputStream;
                    int BUFFER_SIZE = (int)fs.Length + 500;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int read_count = fs.Read(buffer, 0, BUFFER_SIZE);
                    while (read_count > 0)
                    {
                        output.Write(buffer, 0, read_count);
                        read_count = fs.Read(buffer, 0, BUFFER_SIZE);
                    }

                    fs.Close();

                    Response.Clear();
                    Response.ContentType = ContentType;
                    //Response.AddHeader("Content-Disposition", "attachment; filename=form.pdf");
                    Response.OutputStream.Write(buffer, 0, BUFFER_SIZE);
                    Response.Output.Flush();
                    Response.End();
                    fs.Dispose();

                }
                else
                {
                    string _Str = "";
                    _Str = "<center><h3>The process cannot access the document.Another person is using the same document</h3></center>";
                    HtmlContainerControl SubLinkDiv = (HtmlContainerControl)this.FindControl("type");
                    SubLinkDiv.InnerHtml = _Str;
                }
            }
            else
            {
                string _Str = "";
                _Str = "<center><h3>Document Has Been Removed</h3></center>";
                HtmlContainerControl SubLinkDiv = (HtmlContainerControl)this.FindControl("type");
                SubLinkDiv.InnerHtml = _Str.ToString();
            }


        }

        private bool IsFileInUse(string __pdfl)
        {
            try
            {

                using (FileStream fs = new FileStream(__pdfl, FileMode.OpenOrCreate))
                {

                }
                return false;
            }
            catch (IOException ex)
            {
                string message = ex.Message.ToString();

                if (message.Contains("The process cannot access the file"))
                {

                    return true;
                }
                else
                    throw;
            }

        }

        private int PDFBILLType()
        {
            string sql = "select  tblconfiguration.Value from tblconfiguration where Module='INVENTORY' and  Name='PDFBill_Items'";
            OdbcDataReader MyReader = Myinventory.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
               
             return Convert.ToInt32(MyReader["Value"]);
               
            }
            else
                return 1;
        }

        public string Convert_Number_To_Words(int l)
        {
            int _temp = l;
            if (l < 0)
            {
                l = l * -1;
            }
            int r = 0, i = 0;
            string Words = "";
            string[] a = { " One ", " Two ", " Three ", " Four ", " Five ", " Six ", " Seven ", " Eight ", " Nine ", " Ten " };
            string[] b = { " Eleven ", " Twelve ", " Thirteen ", " Fourteen ", " Fifteen ", " Sixteen ", " Seventeen ", " Eighteen ", " Nineteen " };
            string[] c = { "Ten", " Twenty ", " Thirty ", " Fourty ", " Fifty ", " Sixty ", " Seventy ", " Eighty ", " Ninety ", " Hundred " };
            try
            {
                if (l > 99999)
                {
                    r = l / 100000;
                    if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {
                        r = r / 10;
                        Words = Words + c[r - 1] + " Lakh ";
                    }
                    else if (r > 0 && r < 10)
                    {
                        Words += a[r - 1] + " Lakh ";
                    }
                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;
                        Words = b[r - 1] + " Lakh ";
                    }
                    else
                    {
                        i = r / 10;
                        r = r % 10;
                        Words = Words + c[i - 1] + a[r - 1] + " Lakh ";
                    }
                    l = l % 100000;
                }
                if (l > 9999)
                {
                    r = l / 1000;
                    if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {
                        r = r / 10;
                        Words = Words + c[r - 1] + " Thousand ";
                    }
                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;
                        Words = Words + b[r - 1] + "Thousand ";
                    }
                    else
                    {
                        i = r / 10;
                        r = r % 10;
                        Words = Words + c[i - 1] + a[r - 1] + " Thousand ";
                    }
                    l = l % 1000;
                }
                if (l > 999)
                {
                    if (l == 1000)
                    {
                        Words += " Thousand ";
                        l = 0;
                    }
                    else
                    {
                        r = l / 1000;
                        Words += a[r - 1] + " Thousand ";
                        l = l % 1000;
                    }
                }

                if (l > 99)
                {
                    if (l == 100)
                    {
                        Words += " Hundred ";
                        l = 0;
                    }
                    else
                    {
                        r = l / 100;
                        Words += a[r - 1] + " Hundred ";
                        l = l % 100;
                    }
                }
                if (l > 10 && l < 20)
                {
                    r = l % 10;
                    if (Words == "")
                        Words += b[r - 1];
                    else
                        Words += " And " + b[r - 1];
                }
                if (l > 19 && l <= 100)
                {
                    r = l / 10;
                    i = l % 10;
                    if (Words == "")
                    {
                        if (i != 0)
                            Words += c[r - 1] + a[i - 1];
                        else
                            Words += c[r - 1];
                    }
                    else
                    {
                        if (i != 0)
                            Words += " And " + c[r - 1] + a[i - 1];
                        else
                            Words += " And " + c[r - 1];
                    }
                }
                if (l > 0 && l <= 10)
                {
                    if (Words == "")
                        Words += a[l - 1];
                    else
                        Words += " And " + a[l - 1];
                }
                if (_temp == 0)
                {
                    Words = "Zero";
                }
                else if (_temp < 0)
                {
                    Words = "(-ve) " + Words;
                }
                if (Words != "")
                    Words = Words + " Only.";

                return Words;
            }
            catch
            {
                return "Error in Conversion";
            }
        }

 
    }
}
