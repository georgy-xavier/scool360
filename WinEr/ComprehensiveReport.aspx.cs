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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using WinBase;

namespace WinEr
{   

    public partial class ComprehensiveReport : System.Web.UI.Page
    {
        private iTextSharp.text.Font[] MyFonts = new iTextSharp.text.Font[24];
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private ExamManage MyExamMang;
        private OdbcDataReader MyReader = null;
        private OdbcDataReader MyReader1 = null;
        private DataSet MydataSet;

        private struct ToppersArray
        {
            public string StudentName;
            public string Mark;
        }

     
        private bool _NeedDisciplinary = true;
        private bool _NeedAccademic = true;
        private bool _NeedAttendance = true;
        private bool _NeedFeeDues = true;
        private bool _NeedSummary = true;
        private bool _NeedMedical = true;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            //if (Session["StudId"] == null)
            //{
            //    Response.Redirect("SearchStudent.aspx");
            //}
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (!MyUser.HaveActionRignt(821))
            {
                Response.Redirect("RoleErr.htm");
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
                    LoadMyFont();
                    LoadChartToPDFPage();                    
                }
            }
        }

        private void LoadChartToPDFPage()
        {
            double _Mark = 0;
            string _ExamDetailsString = GetExamDetailsString();

            ExamArray[,] _ExamArray = new ExamArray[6, 2];

            _ExamArray[0, 0].ExamName = "MONTHLY TEST";
            _Mark=(25.0 / 50.0) * 100.0;
            _ExamArray[0, 0].Mark = _Mark.ToString();// "25/50";
            _ExamArray[0, 0].Enable = true;
            _ExamArray[0, 1].ExamName = "I MID TERM";
            _Mark=(129.0 / 150.0) * 100.0;
            _ExamArray[0, 1].Mark = _Mark.ToString();// "129/150";
            _ExamArray[0, 1].Enable = true;

            _ExamArray[1, 0].ExamName = "MONTHLY TEST";
            _Mark=(30.0 / 50.0) * 100.0;
            _ExamArray[1, 0].Mark = _Mark.ToString();//"30/50";
            _ExamArray[1, 0].Enable = true;
            _ExamArray[1, 1].ExamName = "I MID TERM";
            _Mark = (37.0 / 150.0) * 100.0;
            _ExamArray[1, 1].Mark = _Mark.ToString();//"37/150";
            _ExamArray[1, 1].Enable = true;

            _ExamArray[2, 0].ExamName = "MONTHLY TEST";
            _Mark=(35.0 / 50.0) * 100.0;
            _ExamArray[2, 0].Mark = _Mark.ToString();//"35/50";
            _ExamArray[2, 0].Enable = true;
            _ExamArray[2, 1].ExamName = "I MID TERM";
            _Mark=(45.0 / 150.0) * 100.0;
            _ExamArray[2, 1].Mark = _Mark.ToString();//"45/150";
            _ExamArray[2, 1].Enable = true;

            _ExamArray[3, 0].ExamName = "MONTHLY TEST";
            _Mark=(42.0 / 50.0) * 100.0;
            _ExamArray[3, 0].Mark = _Mark.ToString();//"42/50";
            _ExamArray[3, 0].Enable = true;
            _ExamArray[3, 1].ExamName = "I MID TERM";
            _Mark= (76.0 / 100.0) * 100.0;
            _ExamArray[3, 1].Mark =_Mark.ToString();//"76/100";
            _ExamArray[3, 1].Enable = true;

            _ExamArray[4, 0].ExamName = "MONTHLY TEST";
            _Mark= (33.0 / 50.0) * 100.0;
            _ExamArray[4, 0].Mark =_Mark.ToString();//"33/50";
            _ExamArray[4, 0].Enable = true;
            _ExamArray[4, 1].ExamName = "I MID TERM";
            _Mark=(87.0 / 100.0) * 100.0;
            _ExamArray[4, 1].Mark = _Mark.ToString();//"87/100";
            _ExamArray[4, 1].Enable = true;

            _ExamArray[5, 0].ExamName = "MONTHLY TEST";
            _Mark=(44.0 / 50.0) * 100.0;
            _ExamArray[5, 0].Mark = _Mark.ToString();//"44/50";
            _ExamArray[5, 0].Enable = true;
            _ExamArray[5, 1].ExamName = "I MID TERM";
            _Mark=(90.0 / 100.0) * 100.0;
            _ExamArray[5, 1].Mark = _Mark.ToString();//"90/100";
            _ExamArray[5, 1].Enable = true;

            int _SelectedCondition = 1;// int.Parse(Drp_SelectList.SelectedValue);
            int _RowCount = _ExamArray.GetLength(0);
            int _ColumnCount = _ExamArray.GetLength(1);

            string _ExamList = " <table width=\"100%\">";
            for (int k = 0; k < _ColumnCount; k++)
            {
                _ExamList = _ExamList + "<tr><td class=\"CellStyle\">Exam " + (k + 1).ToString() + "</td><td class=\"CellStyle\">" + _ExamArray[_SelectedCondition, k].ExamName + "</td></tr>";
            }
            _ExamList = _ExamList + " </table>";

            float MaxVal = 100;
            float _val = 0;
            int _GraphCount = 0;
            string[] _GraphName = new string[_RowCount];
            for (int a = 0; a < _RowCount;a++ )
            {
                ChartControl chartcontrol_ExamChart = new ChartControl();                
                //chartcontrol_ExamChart.ID = chartcontrol_ExamChart0.ID;
                chartcontrol_ExamChart.HasChartLegend = false;

                ColumnChart chart_Bar = new ColumnChart();

                Chart chart_Line = new SmoothLineChart();

                ChartPointCollection chart_Line_data = chart_Line.Data;

                for (int i = 0; i < _ColumnCount; i++)
                {                    
                    if (float.TryParse(_ExamArray[a, i].Mark, out _val))
                    {
                        //DataColumn dc = _ExamArray[_SelectedCondition, i].ExamName;
                        string ColumnName = "Exam " + (i + 1).ToString();// _ExamArray[_SelectedCondition, i].ExamName;
                        if (MaxVal < _val)
                        {
                            MaxVal = _val;
                        }
                        chart_Line_data.Add(new ChartPoint(ColumnName, _val));
                        chart_Bar.Data.Add(new ChartPoint(ColumnName, _val));
                    }
                }

                chart_Line.Line.Width = 2;
                chart_Line.Line.Color = System.Drawing.Color.RoyalBlue;
                chart_Line.Legend = "1";// Drp_SelectList.SelectedValue;
                chartcontrol_ExamChart.Charts.Clear();
                chartcontrol_ExamChart.Charts.Add(chart_Line);

                //chart_Line.DataLabels.Visible = true;

                chartcontrol_ExamChart.YTitle.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                chartcontrol_ExamChart.YTitle.StringFormat.Alignment = StringAlignment.Near;


                if (MaxVal != 100)
                {
                    MaxVal = MaxVal + 10;
                }
                chart_Bar.Shadow.Visible = true;
                chart_Bar.DataLabels.Visible = true;
                chart_Bar.MaxColumnWidth = 20;
                chartcontrol_ExamChart.YCustomEnd = MaxVal;
                chartcontrol_ExamChart.Charts.Add(chart_Bar);
                chart_Bar.Fill.Color = System.Drawing.Color.RoyalBlue;
                chartcontrol_ExamChart.Background.Color = System.Drawing.Color.White;
                chartcontrol_ExamChart.RedrawChart();
                _GraphCount++;
                _GraphName[a] = chartcontrol_ExamChart.FileName;
            }
            if (_GraphCount > 0)
            {
                string _PhysicalPath = WinerUtlity.GetAbsoluteFilePath(objSchool,Server.MapPath(""));
                if (ExportGraphToPDF(_PhysicalPath, _GraphName, _GraphCount, _ExamDetailsString, _ExamArray))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", "window.open(\"OpenPdfPage.aspx?PdfName=Test.pdf\");", true);
                    //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"OpenPdfPage.aspx?PdfName=" + _PdfName + "\");", true);
                }
                //this.ExamNames.InnerHtml = _ExamList;
            }
        }

        private string GetExamDetailsString()
        {

            StringBuilder CTR = new StringBuilder();

            CTR.Append("<table runat=\"server\" width=\"100%\" style=\"border: thin solid #000000\">");

            CTR.Append("<tr>");

            CTR.Append("<td valign=\"top\"><table runat=\"server\" width=\"100%\"> <tr><td class=\"TableHeaderStyle\"><b>SUBJECTS</b></td>  <td class=\"TableHeaderStyle\"><b>MONTHLY TEST</b></td>  <td class=\"TableHeaderStyle\"><b>I MID TERM</b></td></tr> ");

          
            CTR.Append("<tr><td class=\"CellStyle\">ENGLISH</td>  <td class=\"CellStyle\">25/50</td> <td class=\"CellStyle\">129/150</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">KANNADA</td>  <td class=\"CellStyle\">30/50</td>  <td class=\"CellStyle\">37/50</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">HINDI</td>  <td class=\"CellStyle\">35/50</td>  <td class=\"CellStyle\">45/50</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">MATHS</td>  <td class=\"CellStyle\">42/50</td>  <td class=\"CellStyle\">76/100</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">SCIENCE</td>  <td class=\"CellStyle\">33/50</td>  <td class=\"CellStyle\">87/100</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">SOCIAL</td>  <td class=\"CellStyle\">44/50</td>  <td class=\"CellStyle\">90/100</td> </tr>");
           

            CTR.Append("<tr><td class=\"SubHeaderStyle\">Total Mark </td>  <td class=\"SubHeaderStyle\">199/300</td>  <td class=\"SubHeaderStyle\">464/550</td> </tr>");
            CTR.Append("<tr><td class=\"CellStyle\">Ranking(Section)</td>  <td class=\"CellStyle\">12/30</td>  <td class=\"CellStyle\">7/23</td> </tr>");
            CTR.Append("<tr><td class=\"SubHeaderStyle\">Ranking(Standard) </td>  <td class=\"SubHeaderStyle\">12/30</td>  <td class=\"SubHeaderStyle\">7/23</td> </tr>");

            CTR.Append("</table>");
            CTR.Append("</td>");
            CTR.Append("<td></td>");

            CTR.Append("</tr>");
            CTR.Append("</table>");

            return CTR.ToString();
        }

        private bool ExportGraphToPDF(string _PhysicalPath, string[] _GraphImage, int _GraphCount, string _ExamDetailsString, ExamArray[,] _ExamArray)
        {
            bool _Success = false;
            try
            {
                Document DocER = new Document(PageSize.A4, 50, 40, 20, 10);
                PdfWriter writerER = PdfWriter.GetInstance(DocER, new FileStream(_PhysicalPath + "\\PDF_Files\\Test.pdf", FileMode.Create));
              
                DocER.Open();

                iTextSharp.text.Table Tbl_CollegeDetails = ER_LoadCollegeDetailsToTable(_PhysicalPath);
                iTextSharp.text.Table Tbl_StudentDetails = ER_LoadStudentDetailsToTable();
                iTextSharp.text.Table Tbl_MarkDetails = ER_MarkDetailsToTable();
                iTextSharp.text.Table Tbl_Graph = ER_LoadGraphToTable(_GraphImage, _PhysicalPath, _GraphCount);

                DocER.Add(Tbl_CollegeDetails);
                DocER.Add(Tbl_StudentDetails);
                DocER.Add(Tbl_MarkDetails);
                DocER.Add(Tbl_Graph);

                if (_NeedAccademic == true || _NeedAttendance == true || _NeedDisciplinary == true || _NeedFeeDues == true || _NeedSummary == true)
                {
                    iTextSharp.text.Table Tbl_CommentDetails = ER_LoadCommentDetails();
                    DocER.Add(Tbl_CommentDetails);
                }

                DocER.Close();
                _Success = true;
            }
            catch
            {
                _Success = false;
            }
            return _Success;
        }

        private iTextSharp.text.Table ER_LoadCollegeDetailsToTable(string _PhysicalPath)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(3);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 60,20 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            OdbcDataReader m_MyReader1 = null;
            //_Tbl_CollageDetails.DefaultCell.BorderWidthBottom = 1;
            ImageUploaderClass imgobj = new ImageUploaderClass(objSchool);
            string sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader1 =MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {

                if (true)
                {
                    if (true)
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(imgobj.getImageBytes(1, "Logo"));
                        _ImgHeader.ScaleAbsolute(40, 40);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;
                        _Tbl_CollageDetails.DefaultCell.Colspan = 1;
                        //_Tbl_CollageDetails.DefaultCell.Rowspan = 2;

                        Cell cell = new Cell();
                        //cell.Rowspan = 2;
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        _Tbl_CollageDetails.AddCell(cell);
                       
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase("Your School Name", MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[2]));//Collage Address
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        //_Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase("Your School Name", MyFonts[1])); //Collage Name
                       // _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                   // _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase("Your School Name", MyFonts[1])); //Collage Name
                    //_Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            //_Tbl_CollageDetails.DefaultCell.Colspan = 2;
            //_Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
            //_Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[3])); //empty

            return _Tbl_CollageDetails;

        }

        private iTextSharp.text.Table ER_LoadCommentDetails()
        {
            iTextSharp.text.Table Tbl_Comments = new iTextSharp.text.Table(1);
            Tbl_Comments.Width = 100;
            float[] headerwidths = { 100 };
            Tbl_Comments.Widths = headerwidths;
            Tbl_Comments.Padding = 1;
            Tbl_Comments.Border = 0;
            Tbl_Comments.DefaultCell.Border = 0;
            Tbl_Comments.AutoFillEmptyCells = true;

            if (_NeedDisciplinary == true)
            {
                Tbl_Comments.AddCell(new Phrase("Disciplinary Reports", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("The student has misbehavioured in the class. Reported by:- Mr.Satheesh S. Incident Date: 02-02-2011", MyFonts[3]));
            }
            if (_NeedMedical == true)
            {
                Tbl_Comments.AddCell(new Phrase("Medical Reports", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("The student has taken into hospital due to fever. Reported by:- Ms.Shalini. Incident Date: 11-01-2011", MyFonts[3]));
            }
            if (_NeedAccademic == true)
            {
                Tbl_Comments.AddCell(new Phrase("Accademic Reports", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("The student has became the topper in the class for Unit Test(Jan-Feb). Reported by:- Ms.Anitha. Incident Date: 15-01-2011", MyFonts[3]));
            }
            if (_NeedAttendance == true)
            {
                Tbl_Comments.AddCell(new Phrase("Attendance Reports", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("Total Working Days: 75.  Present Days:73. Absent Days: 22-01-2011 and 23-01-2011", MyFonts[3]));
            }
            if (_NeedFeeDues == true)
            {
                Tbl_Comments.AddCell(new Phrase("Fee Dues Report", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("The student has to pay the following fees. Tution Fee:-1000 Rs. Tech-fest fee:-200 Rs.", MyFonts[3]));
            }
            if (_NeedSummary == true)
            {
                Tbl_Comments.AddCell(new Phrase("Summary", MyFonts[4]));
                Tbl_Comments.AddCell(new Phrase("Dear Parent, your ward is regular in the class and his performance in the I Mid Term exam was average. His performance in ENGLISH,SCIENCE and SOCIAL are better than the previous exam, but his performance in KANNADA,HINDI and MATHS are not good.His over all performance is almost same as that of the last exam. Please go through the complete Reports and revert the signed report on or before 15-02-2011", MyFonts[3]));
            }
            Tbl_Comments.AddCell(new Phrase("\n", MyFonts[6]));
            Tbl_Comments.DefaultCell.HorizontalAlignment = 1;
            Tbl_Comments.AddCell(new Phrase("Computer generated report by Scool360. Powered by Narayan Solutions", MyFonts[6]));

            return Tbl_Comments;
        }

        private iTextSharp.text.Table ER_LoadStudentDetailsToTable()
        {
            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 20, 25, 20, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.Border = 0;
            Tbl_Details.DefaultCell.Border = 0;
            Tbl_Details.AutoFillEmptyCells = true;

            Tbl_Details.DefaultCell.BorderWidthTop = 1;
            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.DefaultCell.HorizontalAlignment = 1;
            Tbl_Details.AddCell(new Phrase("I MID TERM - Report", MyFonts[5]));

            Tbl_Details.DefaultCell.BorderWidthTop = 0;
            Tbl_Details.DefaultCell.HorizontalAlignment = 0;
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase("VISRUTHA E", MyFonts[4]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Roll No  : ", MyFonts[3])); //Bil; date
            Tbl_Details.AddCell(new Phrase("35", MyFonts[4]));

            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Class  : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase("V-A1", MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Date  : ", MyFonts[3])); //Class name
            Tbl_Details.AddCell(new Phrase("02-Feb-2011", MyFonts[4]));           

            Tbl_Details.DefaultCell.Colspan = 5;
            Tbl_Details.AddCell(new Phrase("\n", MyFonts[3]));//empty

            return Tbl_Details;
        }

        private iTextSharp.text.Table ER_MarkDetailsToTable()
        {
            iTextSharp.text.Table _Tbl_MarkDetails = new iTextSharp.text.Table(3);
            _Tbl_MarkDetails.Width = 100;
            float[] headerwidths = { 40,30,30 };
            _Tbl_MarkDetails.Widths = headerwidths;
            _Tbl_MarkDetails.Padding = 1;
            //_Tbl_MarkDetails.Border = 0;
            _Tbl_MarkDetails.AutoFillEmptyCells = true;
            _Tbl_MarkDetails.DefaultCell.Border = 0;

            _Tbl_MarkDetails.DefaultCell.HorizontalAlignment = 0;

            _Tbl_MarkDetails.DefaultCell.Colspan = 3;
            _Tbl_MarkDetails.AddCell(new Phrase("COMPREHENSIVE PROGRESS REPORT", MyFonts[5]));

            _Tbl_MarkDetails.DefaultCell.Colspan = 1;
            _Tbl_MarkDetails.AddCell(new Phrase("SUBJECTS", MyFonts[5]));
            _Tbl_MarkDetails.AddCell(new Phrase("MONTHLY TEST", MyFonts[5]));
            _Tbl_MarkDetails.AddCell(new Phrase("I MID TERM", MyFonts[5]));

            _Tbl_MarkDetails.AddCell(new Phrase("ENGLISH", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("25/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("129/150", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("KANNADA", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("30/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("37/150", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("HINDI", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("35/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("45/150", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("MATHS", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("42/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("76/100", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("SCIENCE", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("33/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("87/100", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("SOCIAL", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("44/50", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("90/100", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("TOTAL MARK", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("199/300", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("464/550", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("RANK", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("12/30", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("7/23", MyFonts[4]));

            _Tbl_MarkDetails.AddCell(new Phrase("GRADE", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("B", MyFonts[4]));
            _Tbl_MarkDetails.AddCell(new Phrase("A", MyFonts[4]));

            return _Tbl_MarkDetails;
        }

        private iTextSharp.text.Table ER_LoadGraphToTable(string[] _GraphImage, string _PhysicalPath,int _GraphCount)
        {
            string[] _Subjects = new string[6];
            _Subjects[0] = "ENGLISH";
            _Subjects[1] = "KANNADA";
            _Subjects[2] = "HINDI";
            _Subjects[3] = "MATHS";
            _Subjects[4] = "SCIENCE";
            _Subjects[5] = "SOCIAL";

            ToppersArray[,] _TopperList = new ToppersArray[6,3];
            _TopperList[0, 0].StudentName = "ROHAN M";
            _TopperList[0, 0].Mark = "145";
            _TopperList[0, 1].StudentName = "MELVIN ANTHONY MILTAN";
            _TopperList[0, 1].Mark = "144";
            _TopperList[0, 2].StudentName = "MIQDAM KHALIL";
            _TopperList[0, 2].Mark = "144";

            _TopperList[1, 0].StudentName = "ROHAN M";
            _TopperList[1, 0].Mark = "145";
            _TopperList[1, 1].StudentName = "ANISH G";
            _TopperList[1, 1].Mark = "144";
            _TopperList[1, 2].StudentName = "MIQDAM KHALIL";
            _TopperList[1, 2].Mark = "144";

            _TopperList[2, 0].StudentName = "ROHAN M";
            _TopperList[2, 0].Mark = "145";
            _TopperList[2, 1].StudentName = "MIQDAM KHALIL";
            _TopperList[2, 1].Mark = "144";
            _TopperList[2, 2].StudentName = "ANISH G";
            _TopperList[2, 2].Mark = "144";

            _TopperList[3, 0].StudentName = "ROHAN M";
            _TopperList[3, 0].Mark = "98";
            _TopperList[3, 1].StudentName = "BRINDA MADHAV";
            _TopperList[3, 1].Mark = "97";
            _TopperList[3, 2].StudentName = "MIQDAM KHALIL";
            _TopperList[3, 2].Mark = "96";

            _TopperList[4, 0].StudentName = "ROHAN M";
            _TopperList[4, 0].Mark = "98";
            _TopperList[4, 1].StudentName = "RETHEESH R";
            _TopperList[4, 1].Mark = "96";
            _TopperList[4, 2].StudentName = "MIQDAM KHALIL";
            _TopperList[4, 2].Mark = "95";

            _TopperList[5, 0].StudentName = "ANISH G";
            _TopperList[5, 0].Mark = "99";
            _TopperList[5, 1].StudentName = "ROHAN M";
            _TopperList[5, 1].Mark = "98";
            _TopperList[5, 2].StudentName = "MIQDAM KHALIL";
            _TopperList[5, 2].Mark = "98";


            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 50,50 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;

            iTextSharp.text.Table _Tbl_Toppers = new iTextSharp.text.Table(2);
            _Tbl_Toppers.Width = 100;
            float[] headerwidths1 = { 30,70 };
            _Tbl_Toppers.Widths = headerwidths1;
            _Tbl_Toppers.Padding = 1;
           
       
            _Tbl_CollageDetails.DefaultCell.Colspan = 2;
            _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[4]));

            _Tbl_CollageDetails.AddCell(new Phrase("Subject-wise Analysis", MyFonts[2]));
            _Tbl_CollageDetails.DefaultCell.Colspan = 1;
            for (int i = 0; i < _GraphCount; i++)
            {
                string _HeaderImgName = _GraphImage[i];
                if (_HeaderImgName != "")
                {
                    string _ImagePathAndName = _HeaderImgName;

                    if (File.Exists(_ImagePathAndName))
                    {
                        _Tbl_Toppers.DeleteAllRows();
                        _Tbl_Toppers.AddCell(new Phrase("Marks", MyFonts[5]));
                        _Tbl_Toppers.AddCell(new Phrase("Student Name", MyFonts[5]));

                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i, 0].Mark, MyFonts[4]));
                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i,0].StudentName, MyFonts[4]));
                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i, 1].Mark, MyFonts[4]));
                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i, 1].StudentName, MyFonts[4]));
                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i, 2].Mark, MyFonts[4]));
                        _Tbl_Toppers.AddCell(new Phrase(_TopperList[i, 2].StudentName, MyFonts[4]));
                        


                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(_ImagePathAndName);
                        _ImgHeader.ScaleAbsolute(230, 135);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;

                        Cell cell = new Cell();
                        Cell cell1 = new Cell();
                        Cell cell2 = new Cell();
          
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        cell1.Add(_Tbl_Toppers);
                       
                        _Tbl_CollageDetails.DefaultCell.Colspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(_Subjects[i]+ " - Performance",MyFonts[4]));
                        _Tbl_CollageDetails.AddCell(new Phrase(_Subjects[i] + " - Toppers", MyFonts[4]));

                        _Tbl_CollageDetails.AddCell(cell);
                        _Tbl_CollageDetails.AddCell(cell1);
                        

                        //_Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        // _Tbl_Comments.AddCell(new Phrase("Performance of student in " + _Subjects[i] + " is good.He stands in the 7th position in the class.His performance is better than the last exam.", MyFonts[3]));
                        // cell2.Add(_Tbl_Comments);
                        // _Tbl_CollageDetails.AddCell(cell2);
                    }
                }
            }
            return _Tbl_CollageDetails;
        }


        private void LoadMyFont()
        {
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);//Title(FR

            //Collage Details
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD);// Bill and Student etails    
            MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);// for the Message (QTN ,DC  ,OA ,PI,TAX INVOICE       
            MyFonts[6] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);// used for Winceron adv
        }
    }
}
