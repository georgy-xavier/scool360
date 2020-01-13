using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

using System.Data;
using WinBase;
using System.Text;
using System.IO;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Drawing;
using System.Web.UI.HtmlControls;


namespace WinEr
{
    public partial class printbuffer : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private string m_PdfName = "";
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            
           
            else
            {
                MyUser = (KnowinUser)Session["UserObj"];
                MyFeeMang = MyUser.GetFeeObj();
                if (MyFeeMang == null)
                {
                    Response.Redirect("Default.aspx");
                    //no rights for this user.
                }
                else
                {
                    if (Request.QueryString["PDF"] != null)
                    {
                        m_PdfName = Request.QueryString["PDF"].ToString();
                    }
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
                        if (m_PdfName != "")
                        {
                            loadBillToPage(m_PdfName);
                        }
                        else
                        {
                            string BillNo = Request.QueryString["BillNo"].ToString();
                            string BillType = Request.QueryString["BillType"].ToString();
                            string StudentName = "", AdmissionNo = "", RollNo = "", ClassName = "";
                            string total = "";
                            string date = "";
                            string C_Name = "";
                            string Address = "";
                            string C_LogoUrl = "";

                            // Checkpaymentmode(BillNo, BillType);
                            MyFeeMang.GetStudentDetails(BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);

                            LoadTotal(BillNo, BillType, out total, out  date);
                            LoadSchoolDetails(out C_LogoUrl, out  C_Name, out  Address);
                            LoadFeeDetails(BillNo, BillType, StudentName, AdmissionNo, RollNo, ClassName, total, date, C_LogoUrl, C_Name, Address);
                        }
                    }
                }
            }
        }

        private void LoadTotal(string BillNo, string BillType, out string total, out string date)
        {
            string sql = "";
            total = "";
            date = "";

            sql = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + BillNo + "'";

            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                total = MyReader.GetValue(0).ToString();
                date = MyReader.GetValue(1).ToString();
            }
        }

        private void LoadFeeDetails(string BillNo, string BillType, string StudentName, string AdmissionNo, string RollNo, string ClassName, string total, string date, string C_LogoUrl, string C_Name, string Address)
        {
            /*Buffer Printing Parameters*/
           
            DataSet  TemplateSet;

            StringBuilder bufferString = new StringBuilder();
   
            string      sql         = "";
            string      Templates   = "";
            string      Err         = "";
            string font = "Lucida Console";
            string CharCount="100";
            string size="10";
            sql = "select tblfeebilltemplates.Template from tblfeebilltemplates where tblfeebilltemplates.IsActive=1 and TemplateName='BUFFER TEMPLATE' ";
            TemplateSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (TemplateSet != null && TemplateSet.Tables[0].Rows.Count > 0)
            {               
               Templates = TemplateSet.Tables[0].Rows[0][0].ToString();
               Templates= Templates.Replace("\n","");
               Templates=Templates.Replace("\r", "");
            }

            if(Templates!="")
            {
                if (CreateBufferFormat(Templates, bufferString, C_Name, Address, StudentName, ClassName, RollNo, AdmissionNo, BillNo, date, out  Err, out  font, out   CharCount, out size))
                {

                    Document document = LoadPDFPage(bufferString, font, CharCount, size);
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

                    filename = MyUser.FilePath + "\\PDF_Files\\Fee_" + StudentName + ".pdf";

                    pdfRenderer.PdfDocument.Save(filename);
                    // ...and start a viewer.

                  //  ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?ptintbuffer=Fee_" + StudentName + ".pdf\");", true);
                    Response.Redirect("printbuffer.aspx?PDF=Fee_" + StudentName + ".pdf", true);              
                 
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

            PgSt.PageWidth = 400;
            PgSt.PageHeight = 450; 

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
      
        private void LoadSchoolDetails(out string S_LogoUrl, out string s_Name, out string Address)
        {
            S_LogoUrl = ""; s_Name = ""; Address = "";
            string Sql = "SELECT SchoolName,Address,LogoUrl FROM tblschooldetails WHERE Id=1";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(Sql);

            if (MyReader.HasRows)
            {
                S_LogoUrl = MyReader.GetValue(2).ToString();
                s_Name = MyReader.GetValue(0).ToString();
                Address = MyReader.GetValue(1).ToString();
            }
        }

        private bool CreateBufferFormat(string Templates, StringBuilder bufferString, string C_Name, string Address, string StudentName, string ClassName, string RollNo, string AdmissionNo, string BillNo, string date, out string Err, out string font, out string CharCount, out string size)
        {
            StringBuilder _bufferString = new StringBuilder();

           
            string      ErrorMessage = "";
            string      _Batch=MyUser.CurrentBatchName;
            string      total="";
            string      amountinwords = "";

            string[]    SPLITMAIN = new string[] {" #SPLITMAIN# "};
            string[]    MainString=Templates.Split(SPLITMAIN, StringSplitOptions.RemoveEmptyEntries);                       

            int         mainStringLen = 0;

            bool        CreationFlow = true;
            
            font = "Lucida Console";
            CharCount = "100";
            size = "10";

            while (mainStringLen < MainString.Length)
            {
             
                string[] subString ;
                string[] SPLITSUB ;

                if (!MainString[mainStringLen].StartsWith("<repeat>"))
                {
                    SPLITSUB = new string[] { " #SPLITSUB# " };


                    subString = MainString[mainStringLen].Split(SPLITSUB, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    subString = new string[1];
                    subString[0]=MainString[mainStringLen];
                    CreationFlow = setRepeateItems(subString[0], CharCount, bufferString, out ErrorMessage, BillNo, out total, out amountinwords);

                }

                int substringLen = 0;
               
                   
                switch (subString[substringLen])
                {
                    case "<PARAMETER>":
                        CreationFlow = GetParameters(subString[1], out CharCount, out ErrorMessage, out font, out  size);                                                              
                        break;

                    case "<RepeatChar>":
                        CreationFlow = GetRepeatedItem(subString[1], CharCount, bufferString, out ErrorMessage);                           
                        break;

                    case "<Br>":
                        CreationFlow = GetNewLine(subString[0], CharCount, bufferString, out ErrorMessage);                           
                        break;
                     

                    case "<Seperator>":
                        CreationFlow = SetseprationWord(subString[1], CharCount, bufferString, out ErrorMessage, C_Name, Address, StudentName, ClassName, RollNo, AdmissionNo, BillNo, date, "", "", _Batch, "", "", "", total, amountinwords);
                        
                        break;

                    case "<Letter>":

                        CreationFlow = SetCharecter(subString[1], CharCount, bufferString, out ErrorMessage);
                       
                        break;

                    case "<repeat>":

                        CreationFlow = setRepeateItems(subString[0], CharCount, bufferString, out ErrorMessage, BillNo, out total, out amountinwords);

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

        private bool setRepeateItems(string sentance, string _pageWidth, StringBuilder bufferString, out string ErrorMessage, string BillNo,out string total,out string amountinwords)
        {
            ErrorMessage = "";
            double _total = 0;
            int count = 1;
            string[] SPLITSUB;
            string sql = "";
            string _Period = "", _FeeName = "",AccountName="",Amount="";

            

            sql = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId where  tblview_transaction.BillNo='" + BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
            
            DataSet data_Fee = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);


            if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
            {

                sentance=sentance.Replace("<repeat>", "");
                string[] SPLITMAIN = new string[] { " #REPEATMAIN# "};
              
                int         mainStringLen = 0;

                bool        CreationFlow = true;

                foreach (DataRow dr in data_Fee.Tables[0].Rows)
                {
                    string[] MainString = sentance.Split(SPLITMAIN, StringSplitOptions.RemoveEmptyEntries);                       

                    _Period = dr[0].ToString();
                    _FeeName = dr[1].ToString();

                    AccountName = dr[2].ToString();
                    Amount = dr[3].ToString();

                    if ("Discount" != dr[2].ToString())
                    {
                        _total = _total + double.Parse(dr[3].ToString());
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
                                CreationFlow = SetseprationWord(subString[1], _pageWidth, bufferString, out ErrorMessage, "", "", "", "", "", "", "", "", _Period, _FeeName, "", AccountName, Amount, count.ToString(), "", "");

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

                }

            }

            else
            {
                total = ""; amountinwords = "";
                ErrorMessage = "No items for printing";
                return false;

            }

           total = _total.ToString();
           amountinwords= MyFeeMang.Convert_Number_To_Words(int.Parse(_total.ToString()));

           return true;
        }

        private bool SetseprationWord(string sentance, string _pageWidth, StringBuilder bufferString, out string ErrorMessage, string C_Name, string Address, string StudentName, string ClassName, string RollNo, string AdmissionNo, string BillNo, string date, string _Period, string _FeeName, string _Batch, string AccountName, string Amount, string serialNo,  string total, string amountinwords)
        {
            ErrorMessage = "";

            string[] subString = sentance.Split(',');    
      
            switch(subString[0])
            {
                case "($clgname$)":

                    if (C_Name.Length > int.Parse(subString[1]))
                    {
                        C_Name = C_Name.Substring(0, int.Parse(subString[1]) - 1);
                    }
                    else if (C_Name.Length < int.Parse(subString[1]))
                    {
                        C_Name=addNewSpace(C_Name, int.Parse(subString[1]));
                    }

                    bufferString.Append(C_Name);
 
                break;

                case "($clgadd$)":

                    if (Address.Length > int.Parse(subString[1]))
                        Address = Address.Substring(0, int.Parse(subString[1]) - 1);
                    else if (Address.Length < int.Parse(subString[1]))
                    {
                        Address=addNewSpace(Address, int.Parse(subString[1]));
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
                       StudentName= addNewSpace(StudentName, int.Parse(subString[1]));
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
                        ClassName=addNewSpace(ClassName, int.Parse(subString[1]));
                    }

                    bufferString.Append(ClassName);

                break;

                case "($Receiptno$)":

                    if (BillNo.Length > int.Parse(subString[1]))
                        BillNo = BillNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (BillNo.Length < int.Parse(subString[1]))
                    {
                       BillNo= addNewSpace(BillNo, int.Parse(subString[1]));
                    }
                    bufferString.Append(BillNo);

                break;

                case "($AdmNo$)":

                    if (AdmissionNo.Length > int.Parse(subString[1]))
                        AdmissionNo = AdmissionNo.Substring(0, int.Parse(subString[1]) - 1);
                    else if (AdmissionNo.Length < int.Parse(subString[1]))
                    {
                        AdmissionNo=addNewSpace(AdmissionNo, int.Parse(subString[1]));
                    }
                    bufferString.Append(AdmissionNo);

                break;

                case "($Date$)":
                    if (date.Length > int.Parse(subString[1]))
                         date = date.Substring(0, int.Parse(subString[1]) - 1);
                    else if (date.Length < int.Parse(subString[1]))
                    {
                        date=addNewSpace(date, int.Parse(subString[1]));
                    }
                    bufferString.Append(date);

                break;

                case "($SINO$)":

                if (serialNo.Length > int.Parse(subString[1]))
                    serialNo = serialNo.Substring(0, int.Parse(subString[1]) - 1);
                else if (serialNo.Length < int.Parse(subString[1]))
                {
                    serialNo=addNewSpace(serialNo, int.Parse(subString[1]));
                }
                bufferString.Append(serialNo);

                break;

                case "($PARTICULARES$)":
                    if(AccountName!="")
                    _FeeName = _FeeName + "(" + AccountName + ")";

                if (_FeeName.Length > int.Parse(subString[1]))
                {
                    _FeeName = _FeeName.Substring(0, int.Parse(subString[1]) - 1);
                    if (AccountName != "")
                    _FeeName = _FeeName + ")";
                    else
                        _FeeName = _FeeName + " ";

                }
                else if (_FeeName.Length < int.Parse(subString[1]))
                {
                    _FeeName=addNewSpace(_FeeName, int.Parse(subString[1]));
                }

                bufferString.Append(_FeeName);

                break;

                case "($PERIOD$)":

                if (_Period.Length > int.Parse(subString[1]))
                    _Period = _Period.Substring(0, int.Parse(subString[1]) - 1);
                else if (_Period.Length < int.Parse(subString[1]))
                {
                    _Period=addNewSpace(_Period, int.Parse(subString[1]));
                }
                bufferString.Append(_Period);

                break;

                case "($AMOUNT$)":

                //if (_Period.Length > int.Parse(subString[1]))
                //    _Period = _Period.Substring(0, int.Parse(subString[1]) - 1);

                bufferString.Append(Amount);

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

        private bool GetRepeatedItem(string sentance, string _pageWidth,StringBuilder bufferString, out string ErrorMessage)
        {
            ErrorMessage = "";
            string[] subString = sentance.Split(',');     
            int repeateCount=0;

            if (subString[1] == ""||int.Parse(subString[1])>int.Parse(_pageWidth))
            {
                repeateCount=int.Parse(_pageWidth);
            }
            else
            {
                 repeateCount=int.Parse(subString[1]);
            }

            for (int i=0;  i<repeateCount; i++)
            { 
                if(subString[0]=="" ||subString[0]==" ")
                    bufferString.Append(" ");
                else
                    bufferString.Append(subString[0]);
                
            }

            return true;
           
        }

        private bool GetParameters(string sentance, out  string _pageWidth, out string message, out string font,  out string size)
        {
            string[] subString = sentance.Split(',');
            message = "";

            font = "Lucida Console";
            _pageWidth = "100";
            size = "10";

          
            if (subString[0]=="CHARCOUNT" && subString[1] != "")
            {
                _pageWidth = subString[1];
                return true;
            }
            else if (subString[0]=="FONTSIZE" && subString[1] != "")
            {
                size = subString[1];
                  return true;
            }
            else if (subString[0] == "FONT" && subString[1] != "")
            {
                font = subString[1];
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
                SubLinkDiv.InnerHtml = _Str;
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

    }

}
