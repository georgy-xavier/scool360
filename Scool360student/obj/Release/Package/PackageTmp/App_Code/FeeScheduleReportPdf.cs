using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.Diagnostics;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using WinBase;


namespace WinEr
{
    public class FeeScheduleReportPdf
    {
        private Font[] MyFonts = new Font[24];
        public MysqlClass m_MysqlDb;
        private SchoolClass m_objSchool = null;
        private OdbcDataReader m_MyReader = null;
        private OdbcDataReader m_MyReader1 = null;
        private OdbcDataReader m_MyReader2 = null;
        private OdbcDataReader m_MyReader3 = null;
        ////private OdbcDataReader m_MyReader4 = null;
        //private OdbcDataReader m_MyReader5 = null;
        //private OdbcDataReader m_MyReader6 = null;
        //private OdbcDataReader m_MyReader7 = null;
        //private OdbcDataReader MyReader1 = null;
        //private OdbcDataReader MyReader2 = null;
        //private OdbcDataReader MyReader3 = null;
        //private OdbcDataReader MyReader4 = null;

        private DataSet m_MyDataSet = null;

        private Font m_FntFeeItem = new Font();
        private Font m_FntFeeItemDesc = new Font(); 
        private string sql = "";
        private string m_NullStr = "NULL";
        private string _YourRef = "Asper your enquiry";
        private string m_AmtType = "RS";
        private string m_PdfType = "";

        public FeeScheduleReportPdf(MysqlClass _Mysqlobj, SchoolClass objSchool)
        {
            m_objSchool = objSchool;
            m_MysqlDb = _Mysqlobj;          
            LoadMyFont();

        }
        ~FeeScheduleReportPdf()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;
            }
            if (m_MyReader != null)
            {
                m_MyReader = null;

            }
            if (m_MyReader1 != null)
            {
                m_MyReader1 = null;

            }
            if (m_MyReader2 != null)
            {
                m_MyReader2 = null;

            }
            if (m_FntFeeItem != null)
            {
                m_FntFeeItem = null;

            }
            if (m_FntFeeItemDesc != null)
            {
                m_FntFeeItemDesc = null;

            }

        }


        internal bool CreateFeeScheduleReportPdf(int _ClassId, int _BatchId, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg)
        {
            _PdfName = "";
            _ErrorMsg = "";

            try
            {
                string _ClassName = "";
                sql = "SELECT tblclass.ClassName from tblclass where tblclass.Id=" + _ClassId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _ClassName = m_MyReader.GetValue(0).ToString();
                }
                _PdfName = "FeeScheduleReport_" + _ClassName + ".pdf";


                if (Delete_Existing_ExamReport(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocFEE = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerFee = PdfWriter.GetInstance(DocFEE, new FileStream(_physicalpath + "\\PDF_Files\\FeeScheduleReport_" + _ClassName + ".pdf", FileMode.Create));
                    DocFEE.Open();

                    iTextSharp.text.Table Tbl_CollageInformation = FEE_LoadCollageDetilsToTable(_physicalpath, _ClassName);
                    DocFEE.Add(Tbl_CollageInformation);
                    if (_ClassId != 0)
                    {
                        DocFEE = DocFEE_LoadFeeReportDetails(DocFEE, writerFee, _ClassId, _BatchId);
                    }
                    else
                    {
                        DocFEE = DocFEE_LoadFeeReportDetailsAllClass(DocFEE, writerFee,_BatchId);
                    }

                    
                    


                    DocFEE.Close();

                    sql = "Delete from tblfeereportpdf ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblfeereportpdf(tblfeereportpdf.ReportName) VALUES ('" + _PdfName + "')";
                    m_MysqlDb.ExecuteQuery(sql);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }
        }

        private Document DocFEE_LoadFeeReportDetailsAllClass(Document DocFEE, PdfWriter writerFee, int _BatchId)
        {
            int i = 1;
            double _Amount = 0;
            string sql = "", _FeeName = "";

            iTextSharp.text.Table Tbl_FeeDetails = new iTextSharp.text.Table(6);
            Tbl_FeeDetails.Width = 100;
            float[] headerwidths = { 10, 45, 10,15, 10, 10 };
            Tbl_FeeDetails.Widths = headerwidths;
            Tbl_FeeDetails.Padding = 1;
            Tbl_FeeDetails.AutoFillEmptyCells = true;
            Tbl_FeeDetails.DefaultCell.Border = 0;

            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_FeeDetails.AddCell(new Phrase("Sl No", MyFonts[4]));
            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
            Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Class Name", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));
            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 0;

            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
            Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

            sql = "SELECT tblfeeschedule.FeeId,tblfeeaccount.AccountName, tblfeeschedule.Amount, tblfeeschedule.LastDate, tblfeeschedule.Duedate,  tblclass.ClassName, tblclass.Id from tblfeeschedule inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblclass on tblfeeschedule.ClassId= tblclass.Id where tblfeeschedule.BatchId=" + _BatchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _Amount = double.Parse(m_MyReader.GetValue(2).ToString());
                    _FeeName = m_MyReader.GetValue(1).ToString();

                    Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                    Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                    Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName, MyFonts[3]));
                    Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                    Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                    Tbl_FeeDetails.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[3]));
                    Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                    Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                    if (writerFee.FitsPage(Tbl_FeeDetails))
                    {
                    }
                    else
                    {
                        Tbl_FeeDetails.DeleteLastRow();
                        DocFEE.Add(Tbl_FeeDetails);
                        Tbl_FeeDetails.DeleteAllRows();

                        DocFEE.NewPage();

                        Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_FeeDetails.AddCell(new Phrase("Sl No", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Class Name", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));

                        Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
                        Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

                        Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                        Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                        Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName, MyFonts[3]));
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                        Tbl_FeeDetails.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[3]));
                        Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                        Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                    }

                    string sql1 = "SELECT tblruleclassmap.RuleId, tblruleclassmap.`Values`, tblrules.Amount, tblrules.tblruleitemId, tblruleitem.name,tblrules.AssigMode, tblrules.FieldValue from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId= tblrules.Id inner join tblruleitem on tblruleitem.Id= tblrules.tblruleitemId where tblruleclassmap.classId=" + int.Parse(m_MyReader.GetValue(6).ToString()) + " and tblruleclassmap.feeTypeId=" + int.Parse(m_MyReader.GetValue(0).ToString());
                    m_MyReader1 = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader1.HasRows)
                    {
                        while (m_MyReader1.Read())
                        {
                            i++;
                            _Amount = _Amount + (-(double.Parse(m_MyReader1.GetValue(2).ToString())));
                            int _RuleId = int.Parse(m_MyReader1.GetValue(3).ToString());
                            string _AssignMode = m_MyReader1.GetValue(5).ToString();
                            string _FieldValue = m_MyReader1.GetValue(6).ToString();

                            string _RuleFeeName = GetRuledFeeName(_RuleId, _AssignMode, _FieldValue);


                            Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                            Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName +_RuleFeeName, MyFonts[3]));
                            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                            Tbl_FeeDetails.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[3]));
                            Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                            Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                            if (writerFee.FitsPage(Tbl_FeeDetails))
                            {
                            }
                            else
                            {
                                Tbl_FeeDetails.DeleteLastRow();
                                DocFEE.Add(Tbl_FeeDetails);
                                Tbl_FeeDetails.DeleteAllRows();

                                DocFEE.NewPage();

                                Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_FeeDetails.AddCell(new Phrase("Sl No", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Class Name", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));

                                Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
                                Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

                                Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                                Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                                Tbl_FeeDetails.AddCell(new Phrase(" " + _FeeName + _RuleFeeName, MyFonts[3]));
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                                Tbl_FeeDetails.AddCell(new Phrase(m_MyReader.GetValue(5).ToString(), MyFonts[3]));
                                Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                                Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                            }
                        }
                    }
                    i++;
                }
            }
            int _Empty = 0;
            while (writerFee.FitsPage(Tbl_FeeDetails))
            {
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                _Empty++;

            }
            if (_Empty > 0)
            {
                Tbl_FeeDetails.DeleteLastRow();
                if (_Empty > 1)
                {
                    Tbl_FeeDetails.DeleteLastRow();
                }
            }
            DocFEE.Add(Tbl_FeeDetails);
            return DocFEE;
        }

        private Document DocFEE_LoadFeeReportDetails(Document DocFEE, PdfWriter writerFee, int _ClassId,int _BatchId)
        {
            int i = 1;
            double _Amount = 0;
            string sql = "",_FeeName="";

            iTextSharp.text.Table Tbl_FeeDetails = new iTextSharp.text.Table(5);
            Tbl_FeeDetails.Width = 100;
            float[] headerwidths = { 10,45,15,15,15 };
            Tbl_FeeDetails.Widths = headerwidths;
            Tbl_FeeDetails.Padding = 1;
            Tbl_FeeDetails.AutoFillEmptyCells = true;
            Tbl_FeeDetails.DefaultCell.Border = 0;

            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_FeeDetails.AddCell(new Phrase("Sl No",MyFonts[4]));
            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
            Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
            Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));
            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 0;

            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
            Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

            sql = "SELECT tblfeeschedule.FeeId,tblfeeaccount.AccountName, tblfeeschedule.Amount, tblfeeschedule.LastDate, tblfeeschedule.Duedate,  tblclass.ClassName from tblfeeschedule inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblclass on tblfeeschedule.ClassId= tblclass.Id where tblfeeschedule.ClassId="+_ClassId+" AND tblfeeschedule.BatchId="+_BatchId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _Amount = double.Parse(m_MyReader.GetValue(2).ToString());
                    _FeeName = m_MyReader.GetValue(1).ToString();

                    Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                    Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                    Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                    Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName, MyFonts[3]));
                    Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                    Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                    Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                    Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                    if (writerFee.FitsPage(Tbl_FeeDetails))
                    {
                    }
                    else
                    {
                        Tbl_FeeDetails.DeleteLastRow();
                        DocFEE.Add(Tbl_FeeDetails);
                        Tbl_FeeDetails.DeleteAllRows();

                        DocFEE.NewPage();

                        Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_FeeDetails.AddCell(new Phrase("Sl No", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
                        Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));

                        Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
                        Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

                        Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                        Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                        Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName, MyFonts[3]));
                        Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                        Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                        Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                        Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                    }

                    string sql1 = "SELECT tblruleclassmap.RuleId, tblruleclassmap.`Values`, tblrules.Amount, tblrules.tblruleitemId, tblruleitem.name,tblrules.AssigMode, tblrules.FieldValue from tblruleclassmap inner join tblrules on tblruleclassmap.RuleId= tblrules.Id inner join tblruleitem on tblruleitem.Id= tblrules.tblruleitemId where tblruleclassmap.classId=" + _ClassId + " and tblruleclassmap.feeTypeId=" + int.Parse(m_MyReader.GetValue(0).ToString());
                    m_MyReader1 = m_MysqlDb.ExecuteQuery(sql1);
                    if (m_MyReader1.HasRows)
                    {
                        while (m_MyReader1.Read())
                        {
                            i++;
                            _Amount = _Amount + (-(double.Parse(m_MyReader1.GetValue(2).ToString())));
                            int _RuleId =int.Parse( m_MyReader1.GetValue(3).ToString());
                            string _AssignMode = m_MyReader1.GetValue(5).ToString();
                            string _FieldValue = m_MyReader1.GetValue(6).ToString();

                            string _RuleFeeName = GetRuledFeeName(_RuleId, _AssignMode, _FieldValue);

                            Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                            Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                            Tbl_FeeDetails.AddCell(new Phrase(" "+_FeeName + _RuleFeeName, MyFonts[3]));
                            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                            Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                            Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                            Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));

                            if (writerFee.FitsPage(Tbl_FeeDetails))
                            {
                            }
                            else
                            {
                                Tbl_FeeDetails.DeleteLastRow();
                                DocFEE.Add(Tbl_FeeDetails);
                                Tbl_FeeDetails.DeleteAllRows();

                                DocFEE.NewPage();

                                Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_FeeDetails.AddCell(new Phrase("Sl No", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Fee Name", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Amount", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Due Date", MyFonts[4]));
                                Tbl_FeeDetails.AddCell(new Phrase("Last Date", MyFonts[4]));

                                Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
                                Tbl_FeeDetails.DefaultCell.BorderWidthTop = 0;

                                Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[3]));
                                Tbl_FeeDetails.DefaultCell.BorderWidthLeft = 1;
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                                Tbl_FeeDetails.AddCell(new Phrase(" " + _FeeName +_RuleFeeName, MyFonts[3]));
                                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                                Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString(), MyFonts[3]));
                                Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(4).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                                Tbl_FeeDetails.AddCell(new Phrase((DateTime.Parse(m_MyReader.GetValue(3).ToString())).ToString("dd-MM-yyyy"), MyFonts[3]));
                            }
                        }
                    }
                    i++;
                }
            }
            int _Empty = 0;
            while(writerFee.FitsPage(Tbl_FeeDetails))
            {
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                Tbl_FeeDetails.AddCell(new Phrase("\n", MyFonts[3]));
                _Empty++;

            }
            if (_Empty > 0)
            {
                Tbl_FeeDetails.DeleteLastRow();
                if (_Empty > 1)
                {
                    Tbl_FeeDetails.DeleteLastRow();
                }
            }
            DocFEE.Add(Tbl_FeeDetails);
            return DocFEE;
        }

        private string GetRuledFeeName(int _RuleId, string _AssignMode, string _FieldValue)
        {
            string _RuledFeeName = "",sql="";
            if (_RuleId == 1)
            {
                sql = "SELECT tblgender.gentname from tblgender where tblgender.Id="+_FieldValue;
                m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader3.HasRows)
                {
                    _RuledFeeName =" (For Gender "+ m_MyReader3.GetValue(0).ToString()+")";
                }
            }
            else if (_RuleId == 2)
            {
                sql = "SELECT tblcast.castname from tblcast where tblcast.Id=" + _FieldValue;
                m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader3.HasRows)
                {
                    _RuledFeeName = " (For Caste " + m_MyReader3.GetValue(0).ToString()+")";
                }
            }
            else if (_RuleId == 3)
            {
                _RuledFeeName = " (For Mark " + _AssignMode + " " + _FieldValue + ")";
            }
            else if (_RuleId == 4)
            {
                _RuledFeeName = " (For Annual Income " + _AssignMode + " " + _FieldValue + ")";
            }
            else if (_RuleId == 5)
            {
                sql = "SELECT tbladmisiontype.Name from tbladmisiontype where tbladmisiontype.Id=" + _FieldValue;
                m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader3.HasRows)
                {
                    _RuledFeeName = " (For Admission Type " + m_MyReader3.GetValue(0).ToString()+")";
                }
            }
            else if (_RuleId == 6)
            {
                sql = "SELECT tblstudtype.TypeName from tblstudtype where tblstudtype.Id=" + _FieldValue;
                m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader3.HasRows)
                {
                    _RuledFeeName = " (For Student Type " + m_MyReader3.GetValue(0).ToString()+")";
                }
            }
            

            return _RuledFeeName;
        }

        private iTextSharp.text.Table FEE_LoadCollageDetilsToTable(string _physicalpath, string _ClassName)
        {
            string _ClassNameHead = "FEE SCHEDULE REPORT";
            if (_ClassName != "")
            {
                _ClassNameHead = _ClassNameHead + " - " + _ClassName.ToUpper();
            }
            else
            {
                _ClassNameHead = _ClassNameHead + " - " + "ALL CLASS";
            }

            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(2);
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 80 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
            sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                if (true)
                {
                    if (true)
                    {
                        iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(imgobj.getImageBytes(1, "Logo"));
                        _ImgHeader.ScaleAbsolute(50, 50);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 0;
                        _Tbl_CollageDetails.DefaultCell.Colspan = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 2;

                        Cell cell = new Cell();
                        cell.Rowspan = 2;
                        cell.Add(_ImgHeader);
                        cell.Border = 0;
                        _Tbl_CollageDetails.AddCell(cell);
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.DefaultCell.VerticalAlignment = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 2;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader1.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            _Tbl_CollageDetails.DefaultCell.Colspan = 2;
            _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
            _Tbl_CollageDetails.AddCell(new Phrase(_ClassNameHead, MyFonts[4]));//Head

            _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[3]));//Head

            return _Tbl_CollageDetails;

        }

        private bool Delete_Existing_ExamReport(string _PdfName, string _physicalpath, out string _ErrorMsg)
        {
            _ErrorMsg = "";
            bool _valid = false;
            try
            {
                sql = "SELECT tblfeereportpdf.ReportName from tblfeereportpdf";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        string _pdf = m_MyReader.GetValue(0).ToString();
                        File.Delete(_physicalpath + "\\PDF_Files\\" + _pdf);
                        _valid = true;
                    }

                }
                else
                {
                    _valid = true;
                }

            }
            catch
            {
                _ErrorMsg = "This File is Allready Used by another Person.Please try again later.";
                _valid = false;
            }
            return _valid;
        }



        #region COMMON FUNCTIONS
        private void LoadMyFont()
        {

            //NEW
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);//Title(FR

            //Collage Details
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD);// Bill and Student etails       
            //message

            //m_FntFeeItem = MyFonts[3];//empty table and Addone item functions
            //m_FntFeeItemDesc = MyFonts[4];//
            //MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);// for the Message (QTN ,DC  ,OA ,PI,TAX INVOICE       

            //// contents

            //MyFonts[6] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//Items List Heading(QTN ,DC  ,OA ,PI,TAX INVOICE  PACK LIST     
            //MyFonts[7] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);//Main Items List Content( QTN ,OA ,PI, TAX INVOICE         
            //MyFonts[8] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);//Accessory Items List Content( )

            //// contents FOR DC
            //MyFonts[9] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//MAIN PRODUCT  (DC  PacKLList      
            //MyFonts[10] = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);//Accessory Items ( DC, PacKLList

            //// condition
            //MyFonts[11] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);// Condition Heading( Qtn ,OA  ,PI,TAX INVOICE      
            //MyFonts[12] = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);//Condition Content (Qtn ,OA ,PI,TAX INVOICE


            ////sign

            //MyFonts[15] = FontFactory.GetFont(FontFactory.HELVETICA, 10); // Sign AREA AREA QTN ,DC ,OA ,PI,TAX INVOICE        
            //MyFonts[16] = FontFactory.GetFont(FontFactory.HELVETICA, 8); // Sign AREA AREA QTN ,DC  ,OA ,PI,TAX INVOICE
            //MyFonts[17] = FontFactory.GetFont(FontFactory.HELVETICA, 8); // Sign AREA AREA QTN ,DC  ,OA ,PI,TAX INVOICE

            ////total amount
            //MyFonts[18] = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);//Total Amount In Words( Qtn,OA  ,PI, TAX INVOICE            

            ////heaer and footer
            //MyFonts[19] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, Color.BLUE);//header logo
            //MyFonts[20] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);//header
            //MyFonts[21] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL);// for the footer 

        }

        #endregion

       
    }
}
