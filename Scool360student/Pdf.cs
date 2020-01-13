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
using WinEr;

namespace Scool360student
{
    public class Pdf
    {
        private Font[] MyFonts = new Font[24];
        public MysqlClass m_MysqlDb;
        private SchoolClass m_objSchool = null;
       // private KnowinUser MyUser;
        private OdbcDataReader m_MyReader = null;
        private OdbcDataReader m_MyReader1 = null;
        private OdbcDataReader m_MyReader2 = null;
        private OdbcDataReader m_MyReader3 = null;
        //private OdbcDataReader m_MyReader4 = null;
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

        public Pdf(MysqlClass _Mysqlobj, SchoolClass objSchool)
        {
            m_objSchool = objSchool;
            m_MysqlDb = _Mysqlobj;          
            LoadMyFont();

        }

       
        ~Pdf()
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
        
   
        #region FEE Receipt

        internal bool CreateFeeReciptPdf(string _BillNo, string _physicalpath, out string _PdfName, out string _ErrorMsg,int _BillTypeValue)
        {
            _PdfName = "";
            _ErrorMsg = "";

            try
            {
               
                int _BillId = 0;// primary index in the Tblfeebill used for giving unique name for the pdf
                int _StudentId = 0;
                int _StudentType = 0;
                int _CourseId = 0;
                int _ClassId = 0;
                int _CollegeId = 0;
                int _BillType = 0;
                int _CreatedUserId = 0;
                string _BatchName="";
                int _BatchId = 0;
               
                DateTime _BillDate = new DateTime();
                string _CreatedDate = "";
                double _CashAmt = 0;
                double _ChequeAmt = 0;
                double _DDAmt = 0;
                //sai added
                double _NeftAmt = 0;
                //
                double _TotalAmount = 0;

                string _ChequeNo = "";
                string _ChequeBankName = "";
                string _DDNo = "";
                string _DDBankName = "";
                //sai added
                string _NeftNo = "";
                string _NeftBankName = "";
                //
                string _CreatedUser = "";
                                                 

                //  LOAD Bill Details to Variable DETAILD              
                FR_LoadBilldetailsToVariablesFromTblfeebill(_BillNo, out _BillId, out _CollegeId, out _CourseId, out _ClassId, out _StudentId, out _StudentType, out _BillType, out _BillDate, out _CashAmt, out _ChequeAmt, out _DDAmt,out _NeftAmt, out _TotalAmount, out _ChequeNo, out _ChequeBankName, out _DDNo, out _DDBankName,out _NeftNo,out _NeftBankName,out _CreatedUserId, out _CreatedDate, out _CreatedUser, out _BatchName,out _BatchId);
              
                // m_PdfType = _PdfType;
                _PdfName = "FR_" + _BillId + ".pdf";
                // delete all the exixsing pdf file

                if (Delete_Existing_Pdf(_PdfName, _physicalpath, out _ErrorMsg))
                {
                    Document DocFRTemp = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerFRTemp = PdfWriter.GetInstance(DocFRTemp, new FileStream(_physicalpath + "\\PDF_Files\\FRTemp" + _BillId + ".pdf", FileMode.Create));
                    Document DocFR = new Document(PageSize.A4, 50, 40, 20, 10);
                    PdfWriter writerFR = PdfWriter.GetInstance(DocFR, new FileStream(_physicalpath + "\\PDF_Files\\FR_" + _BillId + ".pdf", FileMode.Create));

                    DocFR.Open();

                    DocFRTemp.Open();


                    // LOAD ADDRESS DETAILD
                    iTextSharp.text.Table Tbl_CollageInfo = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);

                    //HEADING
                    // Bill And Student Details
                    iTextSharp.text.Table Tbl_BillAndStudentInfo = FR_LoadBillAndStudentInfoDetilsToTable(_BillNo, _BillDate, _StudentId, _BatchName, _ClassId);

                    iTextSharp.text.Table Tbl_SignAndTotal = getFeeItemTableStructureWithheaderDescription();
                    Tbl_SignAndTotal = FR_LoadSignAndTotalDetails(Tbl_SignAndTotal, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt,_NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName,_NeftNo,_NeftBankName, _CreatedUser, _CreatedDate);
                    Tbl_SignAndTotal.DeleteRow(0);
                    // center line
                    iTextSharp.text.Table Tbl_CenterLine = new iTextSharp.text.Table(1);
                    Tbl_CenterLine.DefaultCell.Border = 0;
                    Tbl_CenterLine.Border = 0;
                    
                    Tbl_CenterLine.Width = 100;
                    Tbl_CenterLine.DefaultCell.HorizontalAlignment = 1;
                    Tbl_CenterLine.AddCell(new Phrase("\n===============================================================================================================", MyFonts[3]));
                    // adding data 2 times
                    DocFRTemp.Add(Tbl_CollageInfo);
                    DocFRTemp.Add(LoadTitle("Fee Receipt (Student Copy)"));
                    DocFRTemp.Add(Tbl_BillAndStudentInfo);
                    DocFRTemp.Add(Tbl_SignAndTotal);

                    //if (NeedOfficeCopy())
                    if(_BillTypeValue==6)
                    {
                        DocFRTemp.Add(Tbl_CenterLine);
                        DocFRTemp.Add(Tbl_CollageInfo);
                        DocFRTemp.Add(LoadTitle("Fee Receipt (Student Copy)"));
                        DocFRTemp.Add(Tbl_BillAndStudentInfo);
                        DocFRTemp.Add(Tbl_SignAndTotal);
                    }
                    else if (_BillTypeValue == 11)
                    {

                        DocFRTemp.Add(Tbl_CenterLine);
                        DocFRTemp.Add(Tbl_CollageInfo);
                        DocFRTemp.Add(LoadTitle("Fee Receipt (Bank Copy)"));
                        DocFRTemp.Add(Tbl_BillAndStudentInfo);
                        DocFRTemp.Add(Tbl_SignAndTotal);

                        DocFRTemp.Add(Tbl_CenterLine);
                        DocFRTemp.Add(Tbl_CollageInfo);
                        DocFRTemp.Add(LoadTitle("Fee Receipt (Office Copy)"));
                        DocFRTemp.Add(Tbl_BillAndStudentInfo);
                        DocFRTemp.Add(Tbl_SignAndTotal);



                    }

                    int _EmptyRwCount = 0;
                    iTextSharp.text.Table Tbl_FeeDetailsT = getFeeItemTableStructureWithheaderDescription();

                    if (AlldataCanFixOnSamePage(ref DocFRTemp, writerFRTemp, _StudentId, _BillNo, _physicalpath, out  _EmptyRwCount, out Tbl_FeeDetailsT))
                    {
                        DocFRTemp.Add(Tbl_FeeDetailsT);


                        DocFR.Add(Tbl_CollageInfo);
                        DocFR.Add(LoadTitle("Fee Receipt (Student Copy)"));
                        DocFR.Add(Tbl_BillAndStudentInfo);
                        // loade fee details
                        iTextSharp.text.Table Tbl_FeeDetails = FR_LoadFeeDetails(_BillNo, _StudentId, _StudentType, _BillType, _EmptyRwCount - 1);
                        DocFR.Add(Tbl_FeeDetails);
                        DocFR.Add(Tbl_SignAndTotal);

                        //if (NeedOfficeCopy())
                        if (_BillTypeValue == 6)
                        {
                            DocFR.Add(Tbl_CenterLine);
                            // office copy

                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Office Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);
                            // loade fee details
                            DocFR.Add(Tbl_FeeDetails);
                            DocFR.Add(Tbl_SignAndTotal);
                        }
                        else if (_BillTypeValue == 11)
                        {

                            DocFR.Add(Tbl_CenterLine);
                            // office copy

                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Bank Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);
                            // loade fee details
                            DocFR.Add(Tbl_FeeDetails);
                            DocFR.Add(Tbl_SignAndTotal);

                            DocFR.Add(Tbl_CenterLine);
                            // office copy

                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Office Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);
                            // loade fee details
                            DocFR.Add(Tbl_FeeDetails);
                            DocFR.Add(Tbl_SignAndTotal);



                        }
                        
                    }
                    else
                    {
                        DocFR.Add(Tbl_CollageInfo);
                        DocFR.Add(LoadTitle("Fee Receipt (Student Copy)"));
                        DocFR.Add(Tbl_BillAndStudentInfo);
                        //double _CashAmt, double _ChequeAmt, double _DDAmt,_NeftAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName,_NeftNo,_NeftBankName, string _CreatedUser, DateTime _CreatedDate)

                        LoadFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt,_NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName,_NeftNo,_NeftBankName, _physicalpath, _CreatedUser, _CreatedDate);

                       // if (NeedOfficeCopy())
                        if (_BillTypeValue == 6)
                        {
                            DocFR.NewPage();
                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Office Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);

                            LoadFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt,_NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName,_NeftNo,_NeftBankName, _physicalpath, _CreatedUser, _CreatedDate);

                        }
                        else if (_BillTypeValue == 11)
                        {
                            DocFR.NewPage();
                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Bank Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);

                            LoadFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _NeftNo, _NeftBankName, _physicalpath, _CreatedUser, _CreatedDate);


                            DocFR.NewPage();
                            DocFR.Add(Tbl_CollageInfo);
                            DocFR.Add(LoadTitle("Fee Receipt (Office Copy)"));
                            DocFR.Add(Tbl_BillAndStudentInfo);

                            LoadFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _NeftNo, _NeftBankName, _physicalpath, _CreatedUser, _CreatedDate);

                        }
                    }
                    DocFRTemp.Close();
                    DocFR.Close();
                    File.Delete(_physicalpath + "\\PDF_Files\\FRTemp" + _BillId + ".pdf");

                    sql = "Delete from tblcreatedbill ";
                    m_MysqlDb.ExecuteQuery(sql);
                    sql = "INSERT into tblcreatedbill(BillName) VALUES('" + _PdfName + "')";
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

       
        private void LoadFeeDetailsAndSignArea(ref Document DocFR, ref PdfWriter writerFR, string _BillNo, int _StudentId, int _StudentType, int _BillType, int _CollegeId, double _TotalAmount, double _CashAmt, double _ChequeAmt, double _DDAmt,double _NeftAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName,string _NeftNo,string _NeftBankName,string _physicalpath, string _CreatedUser, string _CreatedDate)
        {
            // loade fee details
            iTextSharp.text.Table Tbl_FeeDetails = FR_LoadFeeDetailsOnFullPage(ref DocFR, writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _NeftNo, _NeftBankName,_physicalpath, _CreatedUser, _CreatedDate);
            

            iTextSharp.text.Table _TblSignArea = getFeeItemTableStructureWithheaderDescription();//TABLE WITH HEADING
            //_LastRwAndSign.DeleteRow(0);// DELETE THE HEADING PART OF THE CONTENT TABLE
            _TblSignArea = FR_LoadSignAndTotalDetails(_TblSignArea, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt,_NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName,_NeftNo,_NeftBankName,_CreatedUser, _CreatedDate);

            iTextSharp.text.Table _EmptyFeeTable = getFeeItemTableStructureWithheaderDescription();
            _EmptyFeeTable.DeleteRow(0);//delete header
            _EmptyFeeTable = AddOneItemToTableFeeDetails(_EmptyFeeTable,-1, "\n",-1, -1, -1,-1);

           
            while (writerFR.FitsPage(_TblSignArea))
            {
               
                // EmptyRwContTbl = II_LoadData_to_Table(EmptyRwContTbl, "", "", "", "", "", 0, MyFonts[12], -2, _EmptyRwCount);
                _EmptyFeeTable.BorderWidthBottom = 0;
                _EmptyFeeTable.BorderWidthTop = 0;
                DocFR.Add(_EmptyFeeTable);
                //ADD EMPTY ROW TO CONTENT AREA                        
            }

            _TblSignArea.DeleteRow(0);// to delete the description

            DocFR.Add(_TblSignArea);

                   
        }

        private iTextSharp.text.Table FR_LoadFeeDetailsOnFullPage(ref Document DocFR, PdfWriter writerFR, string _BillNo, int _StudentId, int _StudentType, int _BillType, int _CollegeId, double _TotalAmount, double _CashAmt, double _ChequeAmt, double _DDAmt,double _NeftAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName,string _NeftNo,string _NeftBankName,string _physicalpath, string _CreatedUser, string _CreatedDate)
        {
            string _FeeName = "";
            double _Amount = 0;
            double _DueBalance = 0,_BalanceAmount=0;
            double _Discount = 0;
            int _TransType = 0;
            int _AccountType = 0;
            int _ScheduleId = 0;
            int i = 0;
            int _total_item_count = 0;
            iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();

            //sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId + " and tbltransaction.AccountTo !=3 ";
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            sql = "SELECT  tblview_transaction.FeeName as AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            //DataSet m_MyDataSet1 = null;
            // sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId;
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            //m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

           
            _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
            if (_total_item_count > 0)
            {
                
                ////sort to make discount at bottom
                //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

                for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
                {
                    i++;
                    _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString() + " (" + m_MyDataSet.Tables[0].Rows[k][5].ToString() + ":" + m_MyDataSet.Tables[0].Rows[k][6].ToString() + ")";

                    _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
                    _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
                    _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
                    //_ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _BalanceAmount = double.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _Discount = 0;
                    //if (_AccountType == 1)//feee
                    //{
                    //    _Discount = 0; getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

                    //}
                    if (_AccountType == 4)
                    {
                        _FeeName = "Fine For " + _FeeName;
                    }

                    if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
                    {
                        //_DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);
                        _DueBalance = _BalanceAmount;
                    }
                    else
                    {
                        _DueBalance = 0;
                    }
                   
                    Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);
                   
                    if (i < _total_item_count)
                    {
                        if (!writerFR.FitsPage(Tbl_FeeDetails))// we canot fix last row
                        {
                            Tbl_FeeDetails.DeleteLastRow();
                            Tbl_FeeDetails.BorderWidthBottom = 1;
                            DocFR.Add(Tbl_FeeDetails);
                            DocFR.NewPage();
                            Tbl_FeeDetails = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);
                            Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
                            Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);

                        }
                    }
                    else if (i == _total_item_count)// Last RW // CHECK IT IS POSIBLE TO FIX LAST AND SIGN AREA ON THAT PAGE(last rw is allresy in the table ) , IF POSIBLE FIX AT THE END OF THE PAGE ELSE MOVE TO NEST PAGE
                    {

                        if (!writerFR.FitsPage(Tbl_FeeDetails))// we can,t fix last row
                        {
                            Tbl_FeeDetails.DeleteLastRow();
                            DocFR.Add(Tbl_FeeDetails);//// CHK WETHER BOTTOM BOARDER IS NEEDED
                            DocFR.NewPage();
                            Tbl_FeeDetails = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);
                            Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
                            Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);
                            Tbl_FeeDetails.BorderWidthBottom = 0;// fix te lat rw with out boared bottom
                            DocFR.Add(Tbl_FeeDetails);

                        }

                    //}
                        else //we can fix last row so now check whether we can fix the sign area in the same page
                        {
                            Tbl_FeeDetails.DeleteLastRow();
                            Tbl_FeeDetails.BorderWidthBottom = 0;
                            DocFR.Add(Tbl_FeeDetails); //NOW FIXED UPTO LAST RW AND SPACE IS THER TO FIX LAST RW


                            //fix last rw
                            iTextSharp.text.Table Tbl_LastRowAndSign = getFeeItemTableStructureWithheaderDescription();
                            Tbl_LastRowAndSign.DeleteRow(0);//Deleted header
                            //LAST RW DATA
                            Tbl_LastRowAndSign = AddOneItemToTableFeeDetails(Tbl_LastRowAndSign, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);

                            Tbl_LastRowAndSign = FR_LoadSignAndTotalDetails(Tbl_LastRowAndSign, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt,_NeftAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName,_NeftNo,_NeftBankName, _CreatedUser, _CreatedDate);
                            if (writerFR.FitsPage(Tbl_LastRowAndSign))
                            {

                                Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
                                Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);
                                Tbl_FeeDetails.BorderWidthBottom = 0;// fix te lat rw with out boared bottom
                                Tbl_FeeDetails.BorderWidthTop = 0;
                                Tbl_FeeDetails.DeleteRow(0);//delete header
                                DocFR.Add(Tbl_FeeDetails);

                            }
                            else
                            {

                                //MOVE DOWN AND FIX A SMALL EMPTY RW WITH BOARDE WIDTH BOTTOM=1                           
                                Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
                                //ADDING EMPTY RW TO THE TABLE -20IS FOR THE EMPTY RW AND 1 FOR THE NO OF RW
                                //int _EmptyRwCount = 1;
                                Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

                                //_Tbl_PackList = II_LoadData_to_Table(_Tbl_PackList, _PackNo, _PackKind, _Description, _ProQty, _ProRemark, -2, _EmptyRwCount, 1, _Font);// -2 is used for EMPTY RW STATUS, 1(empty rw count); 1 simply given thedefault row span
                                Tbl_FeeDetails.DeleteRow(0);// deleteing the header

                                while (writerFR.FitsPage(Tbl_FeeDetails))
                                {
                                    //_EmptyRwCount++;
                                    Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

                                }
                                if (Tbl_FeeDetails.Size > 1)// _TblEmptyContent can fix and contain more than one rw
                                {
                                    Tbl_FeeDetails.DeleteLastRow();
                                }
                                Tbl_FeeDetails.BorderWidthTop = 0;// NO NEED FOR THE TOP BOARDER BUT ENED BOTTOM BOARDER
                                DocFR.Add(Tbl_FeeDetails);
                                DocFR.NewPage();
                                // Fix the LAST rw 
                                Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
                                Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName,_Discount, _DueBalance, _Amount,_AccountType);
                                Tbl_FeeDetails.BorderWidthBottom = 0;// BECAUSE IF THE LAST RW DATA IS COMMING ON THE TOP THEN SIGN AREA WILL COME ON THE BOTTOM
                                DocFR.Add(Tbl_FeeDetails);
                            }
                        }
                    }
                }
            }
            m_MyDataSet = null;

            return Tbl_FeeDetails;
        }

        private iTextSharp.text.Table FR_LoadFeeDetails(string _BillNo, int _StudentId, int _StudentType, int _BillType, int _EmptyRwCount)
        {
            string _FeeName = "";
            double _Amount = 0;
            double _DueBalance = 0, _BalanceAmount=0;
            double _Discount = 0;
            int _TransType = 0;
            int _AccountType = 0;
            int _ScheduleId = 0;
            int i = 0;
            int _total_item_count = 0;
          
            iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();

            //sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId + " and tbltransaction.AccountTo !=3 ";
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            sql = "SELECT  tblview_transaction.FeeName as AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

             //DataSet m_MyDataSet1 = null;
            // sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId;
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            //m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
            if (_total_item_count > 0)
            {

                ////sort to make discount at bottom
                //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

                for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
                {
                    i++;
                    _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString() + " (" + m_MyDataSet.Tables[0].Rows[k][5].ToString() + ":" + m_MyDataSet.Tables[0].Rows[k][6].ToString() + ")";

                    _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
                    _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
                    _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
                    //_ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _BalanceAmount = double.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _Discount = 0;
                    //if (_AccountType == 1)//feee
                    //{
                    //    _Discount = getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

                    //} 
                    if (_AccountType == 4)
                    {
                        _FeeName = "Fine For " + _FeeName;
                    }

                    if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
                    {
                        //_DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);
                        _DueBalance = _BalanceAmount;

                    }
                    else
                    {
                        _DueBalance = 0;
                    }

                    if (_AccountType != 3)
                    {
                        Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
                    }

                }
                // add empty row
                if (_EmptyRwCount > 0)
                {
                    Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, _EmptyRwCount);

                }
            }
            m_MyDataSet = null;
            return Tbl_FeeDetails;
        }

        private bool AlldataCanFixOnSamePage(ref Document DocFR, PdfWriter writerFR, int _StudentId, string _BillNo, string _physicalpath,  out int _EmptyRwCount, out  iTextSharp.text.Table Tbl_FeeDetails)
        {
            string _FeeName = "";
            bool _Valid = true;
            double _Amount = 0;
            double _DueBalance = 0,_BalanceAmount=0;
            double _Discount = 0;
            int _TransType = 0;
            int _AccountType = 0;
            int _ScheduleId = 0;
            int i = 0;
            int _total_item_count = 0;
            Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
            Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, "_FeeName",_Discount, _DueBalance, _Amount,_AccountType);


            //iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        
            //sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId + " and tbltransaction.AccountTo !=3 ";
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            sql = "SELECT  tblview_transaction.FeeName as AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " and tblview_transaction.AccountTo !=3 order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            //DataSet m_MyDataSet1 = null;
            // sql = "  SELECT tblfeeaccount.AccountName,tbltransaction.Amount, tbltransaction.TransType, tbltransaction.AccountTo, tblfeeschedule.Id,tbltransaction.BalanceAmount FROM tbltransaction inner JOIN tblfeeschedule on tbltransaction.PaymentElementId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id where tbltransaction.BillNo='" + _BillNo + "' and tbltransaction.UserId=" + _StudentId;
            //sql = "SELECT  tblfeeaccount.AccountName,tblview_transaction.Amount, tblview_transaction.TransType, tblview_transaction.AccountTo, tblfeeschedule.Id,tblview_transaction.BalanceAmount, tblview_transaction.PeriodName, tblbatch.BatchName FROM tblview_transaction inner JOIN tblfeeschedule on tblview_transaction.FeeId= tblfeeschedule.Id inner join tblfeeaccount on tblfeeschedule.FeeId= tblfeeaccount.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id inner join tblperiod on tblview_transaction.PeriodId= tblperiod.Id where tblview_transaction.BillNo='" + _BillNo + "' and tblview_transaction.UserId=" + _StudentId + " order by  tblbatch.BatchName asc ,tblview_transaction.FeeId, tblperiod.`Order` asc";
            //m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);


            _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
            if (_total_item_count > 0)
            {

                ////sort to make discount at bottom
                //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

                for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
                {
                    i++;
                    _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString() + " (" + m_MyDataSet.Tables[0].Rows[k][5].ToString() + ":" + m_MyDataSet.Tables[0].Rows[k][6].ToString() + ")";

                    _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
                    _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
                    _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
                    //_ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _BalanceAmount = double.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
                    _Discount = 0;
                    //if (_AccountType == 1)//feee
                    //{
                    //    _Discount = getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

                    //} 
                    if (_AccountType == 4)
                    {
                        _FeeName = "Fine For " + _FeeName;
                    }

                    if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
                    {
                        //_DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);
                        _DueBalance = _BalanceAmount;
                    }
                    else
                    {
                        _DueBalance = 0;
                    }
                    if (_AccountType != 3)
                    {
                        Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
                    
                     //a dded 2 times
                    //Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
                    Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
                    }

                    if (!writerFR.FitsPage(Tbl_FeeDetails))// not posible to fix
                    {
                        _Valid = false;
                       
                    }

                }
            }
            m_MyDataSet = null;

            // check how many no of empty rw can be add on the same page
          Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails,2);
         // Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

           _EmptyRwCount = 0;
          if (writerFR.FitsPage(Tbl_FeeDetails))//  posible to fix
          {              
              while (writerFR.FitsPage(Tbl_FeeDetails))//  posible to fix
              {
                  _EmptyRwCount++;
                  Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 2);
                 // Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);


              }
              
          }
          Tbl_FeeDetails.DeleteLastRow();
          Tbl_FeeDetails.DeleteLastRow();
            return _Valid;

        }

        private iTextSharp.text.Table FR_LoadBillAndStudentInfoDetilsToTable(string _BillNo, DateTime _BillDate, int _StudentId, string _BatchName, int _ClassId)
        {
            string _AdNO = "";
            string _Name = "";
            string _ClassName = "";

            iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
            Tbl_Details.Width = 100;
            float[] headerwidths = { 15, 20, 30, 15, 20 };
            Tbl_Details.Widths = headerwidths;
            Tbl_Details.Padding = 1;
            Tbl_Details.DefaultCell.Border = 0;

            if (_StudentId != 0)
            {
                sql = "select tblview_student.StudentName, tblview_student.AdmitionNo, tblclass.ClassName,' ' as BatchName from tblview_student inner join tblclass on   tblclass.Id=" + _ClassId + " AND tblview_student.Id=" + _StudentId;
            }
            else
            {
                sql = "select distinct(tblview_transaction.StudentName), tblview_feebill.TempId, tblclass.ClassName, tblbatch.BatchName from tblview_transaction inner join tblview_feebill on tblview_transaction.BillNo= tblview_feebill.BillNo inner join tblclass ON tblview_transaction.ClassId= tblclass.Id inner join tblbatch on tblview_transaction.BatchId= tblbatch.Id where tblview_transaction.BillNo='"+_BillNo+"'";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Name = m_MyReader.GetValue(0).ToString();
                _AdNO = m_MyReader.GetValue(1).ToString();
                _ClassName = m_MyReader.GetValue(2).ToString();
                if (_StudentId == 0)
                {
                    _BatchName = m_MyReader.GetValue(3).ToString();
                }
            }


            // fill data to table
            // first row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_Name.ToUpper(), MyFonts[4]));// bold
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Bill Number  : ", MyFonts[3])); //Bil no
            Tbl_Details.AddCell(new Phrase(_BillNo, MyFonts[4]));//
            // Second row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Admission No  : ", MyFonts[3])); //Addmission No
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_AdNO, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Bill Date  : ", MyFonts[3])); //Bil; date
            Tbl_Details.AddCell(new Phrase(_BillDate.ToString("dd-MM-yyyy"), MyFonts[4]));

            // Third row
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("     Class  : ", MyFonts[3])); //Class name
            Tbl_Details.DefaultCell.Colspan = 2;
            Tbl_Details.AddCell(new Phrase(_ClassName, MyFonts[4]));
            Tbl_Details.DefaultCell.Colspan = 1;
            Tbl_Details.AddCell(new Phrase("  Batch  : ", MyFonts[3])); //Batch of student
            Tbl_Details.AddCell(new Phrase(_BatchName, MyFonts[4]));

            return Tbl_Details;
        }
      
        private void FR_LoadBilldetailsToVariablesFromTblfeebill(string _BillNo, out int _BillId, out int _CollegeId, out int _CourseId, out int _ClassId, out int _StudentId, out int _StudentType, out int _BillType, out DateTime _BillDate, out double _CashAmt, out double _ChequeAmt, out double _DDAmt,out double _NeftAmt ,out double _TotalAmount, out string _ChequeNo, out string _ChequeBankName, out string _DDNo, out string _DDBankName,out string _NeftNo,out string _NeftBankName, out int _CreatedUserId, out string _CreatedDate, out string _CreatedUser,out string _BatchName,out int _BatchId)
        {// initializing the value
            string _PaymentMode = "";
             _BillId = 0;// primary index in the Tblfeebill used for giving unique name for the pdf
             _StudentId = 0;
             _StudentType = 0;
             _CourseId = 0;
             _ClassId = 0;
             _CollegeId = 0;
             _BatchId = 0;
             _BillType = 0;
             _CreatedUserId = 0;
             _BillDate = new DateTime();
             _CreatedDate = "";
             _CashAmt = 0;
             _ChequeAmt = 0;
             _DDAmt = 0;
             _NeftAmt = 0;
             _TotalAmount = 0;

             _ChequeNo = "";
             _ChequeBankName = "";
             _DDNo = "";
             _DDBankName = "";
             _NeftNo = "";
             _NeftBankName = "";
             _CreatedUser = "";
             _BatchName = "";

             //sql = "select tblview_feebill.TransationId, tblview_feebill.StudentId,tblview_feebill.TotalAmount, tblview_feebill.CollectedUser, tblview_user.UserName,tblview_feebill.`Date`, tblview_studentclassmap.ClassId,tblview_feebill.`Date`, tblview_feebill.BatchId, tblbatch.BatchName from tblview_feebill INNER JOIN tblview_user on tblview_feebill.CollectedUser= tblview_user.Id inner join tblview_studentclassmap on tblview_studentclassmap.StudentId=tblview_feebill.StudentId inner join tblbatch on tblview_feebill.BatchId= tblbatch.Id where tblview_feebill.BillNo='" + _BillNo + "'";
             sql = "select tblview_feebill.TransationId,tblview_feebill.StudentId,tblview_feebill.TotalAmount, tblview_feebill.CollectedUser, tblview_user.UserName,tblview_feebill.`Date`,tblview_feebill.ClassId, DATE_FORMAT(tblview_feebill.CreatedDateTime,'%d-%m-%Y %h:%i:%s %p') as CreatedDateTime, tblview_feebill.BatchId, tblbatch.BatchName,tblview_feebill.PaymentMode, tblview_feebill.TotalAmount, tblview_feebill.PaymentModeId, tblview_feebill.BankName from tblview_feebill INNER JOIN tblview_user on tblview_feebill.CollectedUser= tblview_user.Id  inner join tblbatch on tblview_feebill.BatchId= tblbatch.Id where tblview_feebill.BillNo='" + _BillNo + "'";
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             { 
              
                 int.TryParse(m_MyReader.GetValue(0).ToString(), out _BillId);// primary index in the Tblfeebill used for giving unique name for the pdf
                 int.TryParse(m_MyReader.GetValue(1).ToString(), out _StudentId);
                 Double.TryParse(m_MyReader.GetValue(2).ToString(), out _TotalAmount);
                 int.TryParse(m_MyReader.GetValue(3).ToString(), out _CreatedUserId);
                 _CreatedDate = m_MyReader.GetValue(7).ToString();
                 _BillDate = DateTime.Parse(m_MyReader.GetValue(5).ToString());  
                 _CreatedUser = m_MyReader.GetValue(4).ToString();
                 int.TryParse(m_MyReader.GetValue(6).ToString(), out _ClassId);
                 int.TryParse(m_MyReader.GetValue(8).ToString(), out _BatchId);
                 _BatchName = m_MyReader.GetValue(9).ToString();
                 _PaymentMode = m_MyReader.GetValue(10).ToString();

                 if (_PaymentMode == "Cash")
                 {
                     Double.TryParse(m_MyReader.GetValue(11).ToString(), out _CashAmt);
                 }
                 if (_PaymentMode == "Cheque")
                 {
                     Double.TryParse(m_MyReader.GetValue(11).ToString(), out _ChequeAmt);
                     _ChequeNo = m_MyReader.GetValue(12).ToString();
                     _ChequeBankName = m_MyReader.GetValue(13).ToString();
                 }
                 if (_PaymentMode == "Demand Draft")
                 {
                     Double.TryParse(m_MyReader.GetValue(11).ToString(), out _DDAmt);
                     _DDNo = m_MyReader.GetValue(12).ToString();
                     _DDBankName = m_MyReader.GetValue(13).ToString();
                 }
                 if (_PaymentMode == "NEFT")
                 {
                     Double.TryParse(m_MyReader.GetValue(11).ToString(), out _NeftAmt);
                     _NeftNo = m_MyReader.GetValue(12).ToString();
                     _NeftBankName = m_MyReader.GetValue(13).ToString();
                 }
             }               
        }

            
        #endregion  FEE Receipt


        #region COMMON FUNCTIONS FOR REGULAR AND JOINING FEE BILL

        private bool Delete_Existing_Pdf(string _PdfName, string _physicalpath, out string _ErrorMsg)
        {
            _ErrorMsg = "";
            bool _valid = false;
            try
            {
                sql = "SELECT tblcreatedbill.BillName from tblcreatedbill";
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

        private double getDueDiscountFromMyDataSet(DataSet m_MyDataSet, int _ScheduleId)
        {
            double _Dicount = 0;

            for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
            {
                // Cheking the schedule fee id and account type
                if ((int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString()) == _ScheduleId) && (int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString()) == 3))
                {
                    _Dicount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
                }

            }
            return _Dicount;
        }

        private DataSet sortMyDataSetToMakeDiscountValueAtBottom(DataSet m_MyDataSet)
        {
            DataSet _DataSetTemp = null;
            int j = 0;
            // fill details except  discount details
            foreach (DataRow _Dr1 in m_MyDataSet.Tables[0].Rows)
            {
                if ((int.Parse(_Dr1[3].ToString()) != 3))// 3=account type for deduction
                {

                    _DataSetTemp.Tables[0].Rows[j][0] = _Dr1[0].ToString();
                    _DataSetTemp.Tables[0].Rows[j][1] = _Dr1[1].ToString();
                    _DataSetTemp.Tables[0].Rows[j][2] = _Dr1[2].ToString();
                    _DataSetTemp.Tables[0].Rows[j][3] = _Dr1[3].ToString();
                    _DataSetTemp.Tables[0].Rows[j][4] = _Dr1[4].ToString();
                    j++;
                }
            }

            // fill discount details
            foreach (DataRow _Dr2 in m_MyDataSet.Tables[0].Rows)
            {
                if ((int.Parse(_Dr2[3].ToString()) == 3))// 3=account type for deduction
                {

                    _DataSetTemp.Tables[0].Rows[j][0] = _Dr2[0].ToString();
                    _DataSetTemp.Tables[0].Rows[j][1] = _Dr2[1].ToString();
                    _DataSetTemp.Tables[0].Rows[j][2] = _Dr2[2].ToString();
                    _DataSetTemp.Tables[0].Rows[j][3] = _Dr2[3].ToString();
                    _DataSetTemp.Tables[0].Rows[j][4] = _Dr2[4].ToString();
                    j++;
                }
            }
            return _DataSetTemp;
        }

        private iTextSharp.text.Table getEmptyFeeItemTable(iTextSharp.text.Table Tbl_FeeDetails, int _RwCount)
        {
            for (int i = 0; i < _RwCount; i++)
            {
                Tbl_FeeDetails.AutoFillEmptyCells = true;
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                //Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
            }

            return Tbl_FeeDetails;


        }

        private iTextSharp.text.Table AddOneItemToTableFeeDetails(iTextSharp.text.Table Tbl_FeeDetails, int i, string _FeeName, double _Discount, double _DueBalance, double _Amount, int _AccountType)
        {
            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 0;
            if (i == -1)
            {

                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                //Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
                Tbl_FeeDetails.AddCell(new Phrase("\n", m_FntFeeItem));
            }
            else
            {

                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
                Tbl_FeeDetails.AddCell(new Phrase(i.ToString(), m_FntFeeItem));
                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
                Tbl_FeeDetails.AddCell(new Phrase("  " + _FeeName, m_FntFeeItem));
                Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 2;
                if (_AccountType == 1)
                {
                    // disc
                    //if (_Discount == 0)
                    //    Tbl_FeeDetails.AddCell(new Phrase("--  ", m_FntFeeItem));
                    //else
                    //    Tbl_FeeDetails.AddCell(new Phrase(_Discount.ToString() + "  ", m_FntFeeItem));

                    if (_DueBalance == 0)
                        Tbl_FeeDetails.AddCell(new Phrase("--  ", m_FntFeeItem));
                    else
                        Tbl_FeeDetails.AddCell(new Phrase(_DueBalance.ToString() + "  ", m_FntFeeItem));

                    Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString() + "  ", m_FntFeeItem));
                }

                if (_AccountType == 4)
                {
                    //Tbl_FeeDetails.AddCell(new Phrase("--  ", m_FntFeeItem));
                    Tbl_FeeDetails.AddCell(new Phrase("--  ", m_FntFeeItem));
                    Tbl_FeeDetails.AddCell(new Phrase(_Amount.ToString() + "  ", m_FntFeeItem));
                }


            }
            return Tbl_FeeDetails;
        }

        private iTextSharp.text.Table getFeeItemTableStructureWithheaderDescription()
        {
            iTextSharp.text.Table Tbl_FeeDetails = new iTextSharp.text.Table(4);
            Tbl_FeeDetails.Width = 100;
            Tbl_FeeDetails.AutoFillEmptyCells = true;
            //float[] headerwidths = { 10, 60, 10, 10,10 };
            float[] headerwidths = { 10, 70, 10, 10 };
            Tbl_FeeDetails.Widths = headerwidths;
            Tbl_FeeDetails.Padding = 2;
            Tbl_FeeDetails.DefaultCell.Border = 0;

            Tbl_FeeDetails.DefaultCell.Colspan = 1;
            Tbl_FeeDetails.DefaultCell.BorderWidthBottom = 1;
            Tbl_FeeDetails.DefaultCell.BorderWidthRight = 1;
            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
            Tbl_FeeDetails.AddCell(new Phrase(" SL No ", m_FntFeeItemDesc));
            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 0;
            Tbl_FeeDetails.AddCell(new Phrase(" Fee Particulars ", m_FntFeeItemDesc));
            Tbl_FeeDetails.DefaultCell.HorizontalAlignment = 1;
            // Tbl_FeeDetails.AddCell(new Phrase(" Concession ", m_FntFeeItemDesc));
            Tbl_FeeDetails.AddCell(new Phrase(" Balance Amount ", m_FntFeeItemDesc));
            Tbl_FeeDetails.AddCell(new Phrase(" Amount Paid ", m_FntFeeItemDesc));

            return Tbl_FeeDetails;
        }

        private double getDueBalanceFromTblstudentfeeschedule(string _BillNo, int _StudentId, int _ScheduleId)
        {
            double _BalanceAmt = 0;
            sql = "SELECT tblfeestudent.BalanceAmount from tblfeestudent where tblfeestudent.SchId=" + _ScheduleId + " and tblfeestudent.StudId=" + _StudentId;
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                while (m_MyReader1.Read())
                {
                    _BalanceAmt = double.Parse(m_MyReader1.GetValue(0).ToString());
                }
            }
            m_MyReader1.Close();
            return _BalanceAmt;
        }
     
        private iTextSharp.text.Table FR_LoadSignAndTotalDetails(iTextSharp.text.Table Tbl_SignAndTotal, double _TotalAmount, double _CashAmt, double _ChequeAmt, double _DDAmt,double _NeftAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName,string _NeftNo,string _NeftBankName, string _CreatedUser, string _CreatedDate)
        {
            // iTextSharp.text.Table Tbl_SignAndTotal = new iTextSharp.text.Table(4);
            Tbl_SignAndTotal.Width = 100;
            float[] headerwidths = { 70, 10, 10, 10 };
            Tbl_SignAndTotal.Widths = headerwidths;
            Tbl_SignAndTotal.Padding = 2;
            Tbl_SignAndTotal.DefaultCell.Border = 0;

            Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 2;
            Tbl_SignAndTotal.DefaultCell.BorderWidthBottom = 1;
            Tbl_SignAndTotal.DefaultCell.Colspan = 2;
            Tbl_SignAndTotal.AddCell(new Phrase("\n", MyFonts[3])); // Label

            Tbl_SignAndTotal.DefaultCell.Colspan = 1;
            Tbl_SignAndTotal.AddCell(new Phrase(" Total  ", MyFonts[4]));// Label
            Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
            Tbl_SignAndTotal.AddCell(new Phrase(_TotalAmount.ToString() + "  ", MyFonts[3])); //Total Amt
            //2 nd rw  cash
            if (_CashAmt > 0)
            {
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 2;
                Tbl_SignAndTotal.DefaultCell.BorderWidthBottom = 1;
                Tbl_SignAndTotal.DefaultCell.Colspan = 2;
                Tbl_SignAndTotal.AddCell(new Phrase("\n", MyFonts[3])); // Label

                Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 0;
                Tbl_SignAndTotal.DefaultCell.Colspan = 1;
                Tbl_SignAndTotal.AddCell(new Phrase(" By Cash  ", MyFonts[4]));// Label
                Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
                Tbl_SignAndTotal.AddCell(new Phrase(_CashAmt.ToString() + "  ", MyFonts[3])); //Total Cas Amt
            }


            //3rd rw Cheque
            if (_ChequeAmt > 0)
            {
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 0;
                Tbl_SignAndTotal.DefaultCell.BorderWidthBottom = 1;
                Tbl_SignAndTotal.DefaultCell.Colspan = 2;
                Tbl_SignAndTotal.AddCell(new Phrase("    Cheque No :" + _ChequeNo + "      Bank : " + _ChequeBankName, MyFonts[3])); // Label

                Tbl_SignAndTotal.DefaultCell.Colspan = 1;
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 2;
                Tbl_SignAndTotal.AddCell(new Phrase(" By Cheque  ", MyFonts[4]));// Label
                Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
                Tbl_SignAndTotal.AddCell(new Phrase(_ChequeAmt.ToString() + "  ", MyFonts[3])); //Total Cheque Amt
            }
            if (_DDAmt > 0)
            {
                //4th rw DD
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 0;
                Tbl_SignAndTotal.DefaultCell.BorderWidthBottom = 1;
                Tbl_SignAndTotal.DefaultCell.Colspan = 2;
                Tbl_SignAndTotal.AddCell(new Phrase("    DD No    :" + _DDNo + "      Bank : " + _DDBankName, MyFonts[3])); // Label

                Tbl_SignAndTotal.DefaultCell.Colspan = 1;
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 2;
                Tbl_SignAndTotal.AddCell(new Phrase(" By DD  ", MyFonts[4]));// Label
                Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
                Tbl_SignAndTotal.AddCell(new Phrase(_DDAmt.ToString() + "  ", MyFonts[3])); //Total DD Amt
            }
            //sai added
            if (_NeftAmt > 0)
            {
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 0;
                Tbl_SignAndTotal.DefaultCell.BorderWidthBottom = 1;
                Tbl_SignAndTotal.DefaultCell.Colspan = 2;
                Tbl_SignAndTotal.AddCell(new Phrase("    Transaction No    :" + _NeftNo + "      Bank : " + _NeftBankName, MyFonts[3])); // Label

                Tbl_SignAndTotal.DefaultCell.Colspan = 1;
                Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 2;
                Tbl_SignAndTotal.AddCell(new Phrase(" By NEFT  ", MyFonts[4]));// Label
                Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
                Tbl_SignAndTotal.AddCell(new Phrase(_NeftAmt.ToString() + "  ", MyFonts[3])); //Total DD Amt
            }
            //
            //5 th
            Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 0;
            long _totalamt = long.Parse(_TotalAmount.ToString());
            string _total = Convert_Number_To_Words(_totalamt).ToString();

            Tbl_SignAndTotal.DefaultCell.Colspan = 4;
            Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 0;

            Tbl_SignAndTotal.AddCell(new Phrase("    Total Amount in Words :     " + _total, MyFonts[3])); // Label

            Tbl_SignAndTotal.DefaultCell.Colspan = 1;
            Tbl_SignAndTotal.AddCell(new Phrase("  Condition :\n * This receipt is valid only with the seal and signature of the authorised person of the above institution. ", MyFonts[3])); // Label

            Tbl_SignAndTotal.DefaultCell.Colspan = 3;
            Tbl_SignAndTotal.DefaultCell.Rowspan = 2;
            Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
            Tbl_SignAndTotal.DefaultCell.VerticalAlignment = 1;
            Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 1;
            Tbl_SignAndTotal.AddCell(new Phrase(" Authorised  Signature   ", MyFonts[3]));
            Tbl_SignAndTotal.DefaultCell.VerticalAlignment = 0;
            Tbl_SignAndTotal.DefaultCell.HorizontalAlignment = 0;
            Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 0;

            Tbl_SignAndTotal.DefaultCell.Colspan = 1;
            Tbl_SignAndTotal.DefaultCell.Rowspan = 1;
            Tbl_SignAndTotal.DefaultCell.BorderWidthLeft = 1;
            Tbl_SignAndTotal.AddCell(new Phrase(" Bill Issued By: " + _CreatedUser + "         Issued Date: " + _CreatedDate, MyFonts[3]));


            return Tbl_SignAndTotal;
        }

        private iTextSharp.text.Table FR_LoadCollageDetilsToTable(int _CollegeId, string _physicalpath)
        {
            iTextSharp.text.Table _Tbl_CollageDetails = new iTextSharp.text.Table(3);
            
            _Tbl_CollageDetails.Width = 100;
            float[] headerwidths = { 20, 70,10 };
            _Tbl_CollageDetails.Widths = headerwidths;
            _Tbl_CollageDetails.Padding = 1;
            _Tbl_CollageDetails.Border = 0;
            _Tbl_CollageDetails.AutoFillEmptyCells = true;
            _Tbl_CollageDetails.DefaultCell.Border = 0;
            ImageUploaderClass imgobj = new ImageUploaderClass(m_objSchool);
            sql = "select tblschooldetails.SchoolName, tblschooldetails.Address from tblschooldetails";
            //GroupName(0),Address(1)
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
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
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[2]));//empty
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                        _Tbl_CollageDetails.AddCell(new Phrase("\n", MyFonts[2]));//empty
                    }
                    else
                    {
                        _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                        _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                        _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                        _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                    }


                }
                else
                {
                    _Tbl_CollageDetails.DefaultCell.Colspan = 3;
                    _Tbl_CollageDetails.DefaultCell.HorizontalAlignment = 1;
                    _Tbl_CollageDetails.DefaultCell.Rowspan = 1;
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(0).ToString(), MyFonts[1])); //Collage Name
                    _Tbl_CollageDetails.AddCell(new Phrase(m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Collage Address

                }

            }
            return _Tbl_CollageDetails;

        }

        private bool NeedOfficeCopy()
        {
            bool _valid = false;
            string OfficeCopy;
            string sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='OfficeCopy'";
            m_MyReader3 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader3.HasRows)
            {
                OfficeCopy = m_MyReader3.GetValue(0).ToString();
                if (OfficeCopy == "1")
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        public string Convert_Number_To_Words(long _InpNo)
        {
            long _temp = _InpNo;
            if (_InpNo < 0)
            {
                _InpNo = _InpNo * -1;
            }

            long r = 0, i = 0;
            string Words = "";


            string[] a = { " One ", " Two ", " Three ", " Four ", " Five ", " Six ", " Seven ", " Eight ", " Nine ", " Ten " };

            string[] b = { " Eleven ", " Twelve ", " Thirteen ", " Fourteen ", " Fifteen ", " Sixteen ", " Seventeen ", " Eighteen ", " Nineteen " };

            string[] c = { "Ten", " Twenty ", " Thirty ", " Fourty ", " Fifty ", " Sixty ", " Seventy ", " Eighty ", " Ninety ", " Hundred " };
            try
            {
                if (_InpNo > 9999999)
                {

                    r = _InpNo / 10000000;
                    if (r > 100)
                    {
                        Words = Convert_Number_To_Words(r);
                        Words = Words + "Crore";
                    }
                    else if (r == 10 || r == 20 || r == 30 || r == 40 || r == 50 || r == 60 || r == 70 || r == 80 || r == 90 || r == 100)
                    {

                        r = r / 10;

                        Words = Words + c[r - 1] + " Crore ";


                    }
                    else if (r > 0 && r < 10)
                    {

                        Words += a[r - 1] + " Crore ";


                    }
                    else if (r > 10 && r < 20)
                    {
                        r = r % 10;

                        Words = b[r - 1] + " Crore ";


                    }
                    else
                    {

                        i = r / 10;

                        r = r % 10;

                        Words = Words + c[i - 1] + a[r - 1] + " Crore ";

                    }

                    _InpNo = _InpNo % 10000000;
                }
                if (_InpNo > 99999)
                {

                    r = _InpNo / 100000;
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

                    _InpNo = _InpNo % 100000;
                }
                if (_InpNo > 9999)
                {

                    r = _InpNo / 1000;
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

                    _InpNo = _InpNo % 1000;

                }

                if (_InpNo > 999)
                {
                    if (_InpNo == 1000)
                    {
                        Words += " Thousand ";
                        _InpNo = 0;
                    }
                    else
                    {
                        r = _InpNo / 1000;

                        Words += a[r - 1] + " Thousand ";



                        _InpNo = _InpNo % 1000;
                    }
                }

                if (_InpNo > 99)
                {
                    if (_InpNo == 100)
                    {
                        Words += "One Hundred ";
                        _InpNo = 0;
                    }
                    else
                    {
                        r = _InpNo / 100;

                        Words += a[r - 1] + " Hundred ";

                        _InpNo = _InpNo % 100;
                    }

                }

                if (_InpNo > 10 && _InpNo < 20)
                {

                    r = _InpNo % 10;
                    if (Words == "")
                        Words += b[r - 1];
                    else
                        Words += " And " + b[r - 1];
                }

                if (_InpNo > 19 && _InpNo <= 100)
                {

                    r = _InpNo / 10;

                    i = _InpNo % 10;
                    //i=r;
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

                if (_InpNo > 0 && _InpNo <= 10)
                {
                    if (Words == "")
                        Words += a[_InpNo - 1];
                    else
                        Words += " And " + a[_InpNo - 1];

                }


                if (_temp == 0)
                {
                    Words = "Zero";
                }
                else if (_temp < 0)
                {
                    Words = "(-ve) " + Words;
                }


                return Words + " Only";
            }
            catch (Exception)
            {
                return "------------------------------------------------------";
            }

        }

        #endregion


        //#region TEMPORARY FEE BILL


        //internal bool CreateJoiningFeeReciptPdf(string _BillNo, string _BatchName, string _physicalpath, out string _PdfName, out string _ErrorMsg,int _BillTypeValue)
        //{
        //    _PdfName = "";
        //    _ErrorMsg = "";

        //    try
        //    {

        //        int _BillId = 0;// primary index in the Tblfeebill used for giving unique name for the pdf
        //        int _StudentId = 0;
        //        int _StudentType = 0;
        //        int _CourseId = 0;
        //        int _ClassId = 0;
        //        int _CollegeId = 0;
        //        int _BillType = 0;
        //        int _CreatedUserId = 0;

        //        DateTime _BillDate = new DateTime();
        //        string _CreatedDate = "";
        //        double _CashAmt = 0;
        //        double _ChequeAmt = 0;
        //        double _DDAmt = 0;
        //        double _TotalAmount = 0;

        //        string _ChequeNo = "";
        //        string _ChequeBankName = "";
        //        string _DDNo = "";
        //        string _DDBankName = "";
        //        string _CreatedUser = "";


        //        //  LOAD Bill Details to Variable DETAILD              
        //        FR_LoadJoiningBilldetailsToVariablesFromTblfeebill(_BillNo, out _BillId, out _CollegeId, out _CourseId, out _ClassId, out _StudentId, out _StudentType, out _BillType, out _BillDate, out _CashAmt, out _ChequeAmt, out _DDAmt, out _TotalAmount, out _ChequeNo, out _ChequeBankName, out _DDNo, out _DDBankName, out _CreatedUserId, out _CreatedDate, out _CreatedUser);

        //        // m_PdfType = _PdfType;
        //        _PdfName = "FR_" + _BillId + ".pdf";
        //        // delete all the exixsing pdf file

        //        if (Delete_Existing_Pdf(_PdfName, _physicalpath, out _ErrorMsg))
        //        {
        //            Document DocFRTemp = new Document(PageSize.A4, 50, 40, 20, 10);
        //            PdfWriter writerFRTemp = PdfWriter.GetInstance(DocFRTemp, new FileStream(_physicalpath + "\\PDF_Files\\FRTemp" + _BillId + ".pdf", FileMode.Create));
        //            Document DocFR = new Document(PageSize.A4, 50, 40, 20, 10);
        //            PdfWriter writerFR = PdfWriter.GetInstance(DocFR, new FileStream(_physicalpath + "\\PDF_Files\\FR_" + _BillId + ".pdf", FileMode.Create));

        //            DocFR.Open();

        //            DocFRTemp.Open();


        //            // LOAD ADDRESS DETAILD
        //            iTextSharp.text.Table Tbl_CollageInfo = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);

        //            //HEADING
        //            // Bill And Student Details
        //            iTextSharp.text.Table Tbl_BillAndStudentInfo = FR_LoadBillAndTempStudentInfoDetilsToTable(_BillNo, _BillDate, _StudentId, _BatchName, _ClassId);

        //            iTextSharp.text.Table Tbl_SignAndTotal = getFeeItemTableStructureWithheaderDescription();
        //            Tbl_SignAndTotal = FR_LoadSignAndTotalDetails(Tbl_SignAndTotal, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _CreatedUser, _CreatedDate);
        //            Tbl_SignAndTotal.DeleteRow(0);
        //            // center line
        //            iTextSharp.text.Table Tbl_CenterLine = new iTextSharp.text.Table(1);
        //            Tbl_CenterLine.DefaultCell.Border = 0;
        //            Tbl_CenterLine.Border = 0;
        //            Tbl_CenterLine.Width = 100;
        //            Tbl_CenterLine.DefaultCell.HorizontalAlignment = 1;
        //            Tbl_CenterLine.AddCell(new Phrase("\n===============================================================================================================", MyFonts[3]));
        //            // adding data 2 times
        //            DocFRTemp.Add(Tbl_CollageInfo);
        //            DocFRTemp.Add(LoadTitle("Fee Recipt (Student Copy)"));
        //            DocFRTemp.Add(Tbl_BillAndStudentInfo);
        //            DocFRTemp.Add(Tbl_SignAndTotal);

        //            //if (NeedOfficeCopy())
        //            if(_BillTypeValue==6)
        //            {
        //                DocFRTemp.Add(Tbl_CenterLine);
        //                DocFRTemp.Add(Tbl_CollageInfo);
        //                DocFRTemp.Add(LoadTitle("Fee Recipt (Student Copy)"));
        //                DocFRTemp.Add(Tbl_BillAndStudentInfo);
        //                DocFRTemp.Add(Tbl_SignAndTotal);
        //            }

        //            int _EmptyRwCount = 0;
        //            iTextSharp.text.Table Tbl_FeeDetailsT = getFeeItemTableStructureWithheaderDescription();

        //            if (AllJoindataCanFixOnSamePage(ref DocFRTemp, writerFRTemp, _StudentId, _BillNo, _physicalpath, out  _EmptyRwCount, out Tbl_FeeDetailsT))
        //            {
        //                DocFRTemp.Add(Tbl_FeeDetailsT);


        //                DocFR.Add(Tbl_CollageInfo);
        //                DocFR.Add(LoadTitle("Fee Recipt (Student Copy)"));
        //                DocFR.Add(Tbl_BillAndStudentInfo);
        //                // loade fee details
        //                iTextSharp.text.Table Tbl_FeeDetails = FR_LoadJoinFeeDetails(_BillNo, _StudentId, _StudentType, _BillType, _EmptyRwCount - 1);
        //                DocFR.Add(Tbl_FeeDetails);
        //                DocFR.Add(Tbl_SignAndTotal);

        //               // if (NeedOfficeCopy())
        //                if(_BillTypeValue==6)
        //                {
        //                    DocFR.Add(Tbl_CenterLine);
        //                    // office copy

        //                    DocFR.Add(Tbl_CollageInfo);
        //                    DocFR.Add(LoadTitle("Fee Recipt (Office Copy)"));
        //                    DocFR.Add(Tbl_BillAndStudentInfo);
        //                    // loade fee details
        //                    DocFR.Add(Tbl_FeeDetails);
        //                    DocFR.Add(Tbl_SignAndTotal);
        //                }
        //            }
        //            else
        //            {
        //                DocFR.Add(Tbl_CollageInfo);
        //                DocFR.Add(LoadTitle("Fee Recipt (Student Copy)"));
        //                DocFR.Add(Tbl_BillAndStudentInfo);
        //                //double _CashAmt, double _ChequeAmt, double _DDAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName, string _CreatedUser, DateTime _CreatedDate)

        //                LoadJoinFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _physicalpath, _CreatedUser, _CreatedDate);

        //                //if (NeedOfficeCopy())
        //                if(_BillTypeValue==6)
        //                {
        //                    DocFR.NewPage();

        //                    DocFR.Add(Tbl_CollageInfo);
        //                    DocFR.Add(LoadTitle("Fee Recipt (Office Copy)"));
        //                    DocFR.Add(Tbl_BillAndStudentInfo);

        //                    LoadFeeDetailsAndSignArea(ref DocFR, ref writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _physicalpath, _CreatedUser, _CreatedDate);

        //                }
        //            }
        //            DocFRTemp.Close();
        //            DocFR.Close();
        //            File.Delete(_physicalpath + "\\PDF_Files\\FRTemp" + _BillId + ".pdf");

        //            sql = "Delete from tblcreatedbill ";
        //            m_MysqlDb.ExecuteQuery(sql);
        //            sql = "INSERT into tblcreatedbill(BillName) VALUES('" + _PdfName + "')";
        //            m_MysqlDb.ExecuteQuery(sql);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }


        //    }
        //    catch (Exception e)
        //    {

        //        if (e.Message == "The process cannot access the file")
        //            _ErrorMsg = "This Bill is used by another person.Try again later";
        //        return false;
        //    }
        //}
       
        //private void LoadJoinFeeDetailsAndSignArea(ref Document DocFR, ref PdfWriter writerFR, string _BillNo, int _StudentId, int _StudentType, int _BillType, int _CollegeId, double _TotalAmount, double _CashAmt, double _ChequeAmt, double _DDAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName, string _physicalpath, string _CreatedUser, DateTime _CreatedDate)
        //{
        //    // loade fee details
        //    iTextSharp.text.Table Tbl_FeeDetails = FR_LoadJoiningFeeDetailsOnFullPage(ref DocFR, writerFR, _BillNo, _StudentId, _StudentType, _BillType, _CollegeId, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _physicalpath, _CreatedUser, _CreatedDate);


        //    iTextSharp.text.Table _TblSignArea = getFeeItemTableStructureWithheaderDescription();//TABLE WITH HEADING
        //    //_LastRwAndSign.DeleteRow(0);// DELETE THE HEADING PART OF THE CONTENT TABLE
        //    _TblSignArea = FR_LoadSignAndTotalDetails(_TblSignArea, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _CreatedUser, _CreatedDate);

        //    iTextSharp.text.Table _EmptyFeeTable = getFeeItemTableStructureWithheaderDescription();
        //    _EmptyFeeTable.DeleteRow(0);//delete header
        //    _EmptyFeeTable = AddOneItemToTableFeeDetails(_EmptyFeeTable, -1, "\n", -1, -1, -1, -1);


        //    while (writerFR.FitsPage(_TblSignArea))
        //    {

        //        // EmptyRwContTbl = II_LoadData_to_Table(EmptyRwContTbl, "", "", "", "", "", 0, MyFonts[12], -2, _EmptyRwCount);
        //        _EmptyFeeTable.BorderWidthBottom = 0;
        //        _EmptyFeeTable.BorderWidthTop = 0;
        //        DocFR.Add(_EmptyFeeTable);
        //        //ADD EMPTY ROW TO CONTENT AREA                        
        //    }

        //    _TblSignArea.DeleteRow(0);// to delete the description

        //    DocFR.Add(_TblSignArea);

        //}

        //private iTextSharp.text.Table FR_LoadJoiningFeeDetailsOnFullPage(ref Document DocFR, PdfWriter writerFR, string _BillNo, int _StudentId, int _StudentType, int _BillType, int _CollegeId, double _TotalAmount, double _CashAmt, double _ChequeAmt, double _DDAmt, string _ChequeNo, string _ChequeBankName, string _DDNo, string _DDBankName, string _physicalpath, string _CreatedUser, DateTime _CreatedDate)
        //{
        //    string _FeeName = "";
        //    double _Amount = 0;
        //    double _DueBalance = 0;
        //    double _Discount = 0;
        //    int _TransType = 0;
        //    int _AccountType = 0;
        //    int _ScheduleId = 0;
        //    int i = 0;
        //    int _total_item_count = 0;
        //    iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();

        //    sql = " SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId + " and tbljoining_transaction.AccountTo !=3 ";
        //    m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    DataSet m_MyDataSet1 = null;
        //    sql = " SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId;
        //    m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
        //    if (_total_item_count > 0)
        //    {

        //        ////sort to make discount at bottom
        //        //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

        //        for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
        //        {
        //            i++;
        //            _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString();

        //            _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
        //            _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
        //            _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
        //            _ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
        //            _Discount = 0;
        //            if (_AccountType == 1)//feee
        //            {
        //                _Discount = getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

        //            }
        //            if (_AccountType == 4)
        //            {
        //                _FeeName = "Fine For " + _FeeName;
        //            }

        //            if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
        //            {
        //                _DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);
        //            }
        //            else
        //            {
        //                _DueBalance = 0;
        //            }

        //            Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);

        //            if (i < _total_item_count)
        //            {
        //                if (!writerFR.FitsPage(Tbl_FeeDetails))// we canot fix last row
        //                {
        //                    Tbl_FeeDetails.DeleteLastRow();
        //                    Tbl_FeeDetails.BorderWidthBottom = 1;
        //                    DocFR.Add(Tbl_FeeDetails);
        //                    DocFR.NewPage();
        //                    Tbl_FeeDetails = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);
        //                    Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //                    Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);

        //                }
        //            }
        //            else if (i == _total_item_count)// Last RW // CHECK IT IS POSIBLE TO FIX LAST AND SIGN AREA ON THAT PAGE(last rw is allresy in the table ) , IF POSIBLE FIX AT THE END OF THE PAGE ELSE MOVE TO NEST PAGE
        //            {

        //                if (!writerFR.FitsPage(Tbl_FeeDetails))// we can,t fix last row
        //                {
        //                    Tbl_FeeDetails.DeleteLastRow();
        //                    DocFR.Add(Tbl_FeeDetails);//// CHK WETHER BOTTOM BOARDER IS NEEDED
        //                    DocFR.NewPage();
        //                    Tbl_FeeDetails = FR_LoadCollageDetilsToTable(_CollegeId, _physicalpath);
        //                    Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //                    Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //                    Tbl_FeeDetails.BorderWidthBottom = 0;// fix te lat rw with out boared bottom
        //                    DocFR.Add(Tbl_FeeDetails);

        //                }

        //            //}
        //                else //we can fix last row so now check whether we can fix the sign area in the same page
        //                {
        //                    Tbl_FeeDetails.DeleteLastRow();
        //                    Tbl_FeeDetails.BorderWidthBottom = 0;
        //                    DocFR.Add(Tbl_FeeDetails); //NOW FIXED UPTO LAST RW AND SPACE IS THER TO FIX LAST RW


        //                    //fix last rw
        //                    iTextSharp.text.Table Tbl_LastRowAndSign = getFeeItemTableStructureWithheaderDescription();
        //                    Tbl_LastRowAndSign.DeleteRow(0);//Deleted header
        //                    //LAST RW DATA
        //                    Tbl_LastRowAndSign = AddOneItemToTableFeeDetails(Tbl_LastRowAndSign, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);

        //                    Tbl_LastRowAndSign = FR_LoadSignAndTotalDetails(Tbl_LastRowAndSign, _TotalAmount, _CashAmt, _ChequeAmt, _DDAmt, _ChequeNo, _ChequeBankName, _DDNo, _DDBankName, _CreatedUser, _CreatedDate);
        //                    if (writerFR.FitsPage(Tbl_LastRowAndSign))
        //                    {

        //                        Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //                        Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //                        Tbl_FeeDetails.BorderWidthBottom = 0;// fix te lat rw with out boared bottom
        //                        Tbl_FeeDetails.BorderWidthTop = 0;
        //                        Tbl_FeeDetails.DeleteRow(0);//delete header
        //                        DocFR.Add(Tbl_FeeDetails);

        //                    }
        //                    else
        //                    {

        //                        //MOVE DOWN AND FIX A SMALL EMPTY RW WITH BOARDE WIDTH BOTTOM=1                           
        //                        Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //                        //ADDING EMPTY RW TO THE TABLE -20IS FOR THE EMPTY RW AND 1 FOR THE NO OF RW
        //                        //int _EmptyRwCount = 1;
        //                        Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

        //                        //_Tbl_PackList = II_LoadData_to_Table(_Tbl_PackList, _PackNo, _PackKind, _Description, _ProQty, _ProRemark, -2, _EmptyRwCount, 1, _Font);// -2 is used for EMPTY RW STATUS, 1(empty rw count); 1 simply given thedefault row span
        //                        Tbl_FeeDetails.DeleteRow(0);// deleteing the header

        //                        while (writerFR.FitsPage(Tbl_FeeDetails))
        //                        {
        //                            //_EmptyRwCount++;
        //                            Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

        //                        }
        //                        if (Tbl_FeeDetails.Size > 1)// _TblEmptyContent can fix and contain more than one rw
        //                        {
        //                            Tbl_FeeDetails.DeleteLastRow();
        //                        }
        //                        Tbl_FeeDetails.BorderWidthTop = 0;// NO NEED FOR THE TOP BOARDER BUT ENED BOTTOM BOARDER
        //                        DocFR.Add(Tbl_FeeDetails);
        //                        DocFR.NewPage();
        //                        // Fix the LAST rw 
        //                        Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //                        Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //                        Tbl_FeeDetails.BorderWidthBottom = 0;// BECAUSE IF THE LAST RW DATA IS COMMING ON THE TOP THEN SIGN AREA WILL COME ON THE BOTTOM
        //                        DocFR.Add(Tbl_FeeDetails);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    m_MyDataSet = null;

        //    return Tbl_FeeDetails;
        //}

        //private iTextSharp.text.Table FR_LoadJoinFeeDetails(string _BillNo, int _StudentId, int _StudentType, int _BillType, int _EmptyRwCount)
        //{
        //    string _FeeName = "";
        //    double _Amount = 0;
        //    double _DueBalance = 0;
        //    double _Discount = 0;
        //    int _TransType = 0;
        //    int _AccountType = 0;
        //    int _ScheduleId = 0;
        //    int i = 0;
        //    int _total_item_count = 0;

        //    iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();

        //    sql = "  SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId + " and tbljoining_transaction.AccountTo !=3 ";
        //    m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    DataSet m_MyDataSet1 = null;
        //    sql = "  SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId;
        //    m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
        //    if (_total_item_count > 0)
        //    {

        //        ////sort to make discount at bottom
        //        //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

        //        for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
        //        {
        //            i++;
        //            _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString();

        //            _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
        //            _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
        //            _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
        //            _ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
        //            _Discount = 0;
        //            if (_AccountType == 1)//feee
        //            {
        //                _Discount = getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

        //            }
        //            if (_AccountType == 4)
        //            {
        //                _FeeName = "Fine For " + _FeeName;
        //            }


        //            if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
        //            {
        //                _DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);

        //            }
        //            else
        //            {
        //                _DueBalance = 0;
        //            }

        //            if (_AccountType != 3)
        //            {
        //                Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //            }

        //        }
        //        // add empty rw

        //        if (_EmptyRwCount > 0)
        //        {
        //            Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, _EmptyRwCount);

        //        }
        //    }
        //    m_MyDataSet = null;
        //    return Tbl_FeeDetails;
        //}

        //private bool AllJoindataCanFixOnSamePage(ref Document DocFRTemp, PdfWriter writerFRTemp, int _StudentId, string _BillNo, string _physicalpath, out int _EmptyRwCount, out iTextSharp.text.Table Tbl_FeeDetails)
        //{
        //    string _FeeName = "";
        //    bool _Valid = true;
        //    double _Amount = 0;
        //    double _DueBalance = 0;
        //    double _Discount = 0;
        //    int _TransType = 0;
        //    int _AccountType = 0;
        //    int _ScheduleId = 0;
        //    int i = 0;
        //    int _total_item_count = 0;
        //    Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();
        //    Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, "_FeeName", _Discount, _DueBalance, _Amount, _AccountType);


        //    //iTextSharp.text.Table Tbl_FeeDetails = getFeeItemTableStructureWithheaderDescription();

        //    sql = "  SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId + " and tbljoining_transaction.AccountTo !=3 ";
        //    m_MyDataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    DataSet m_MyDataSet1 = null;
        //    sql = "  SELECT tblfeeaccount.AccountName, tbljoining_transaction.Amount, tbljoining_transaction.TransType, tbljoining_transaction.AccountTo, tbljoining_feeschedule.Id FROM tbljoining_transaction inner JOIN tbljoining_feeschedule on tbljoining_transaction.PaymentElementId= tbljoining_feeschedule.Id inner join tblfeeaccount on tbljoining_feeschedule.FeeId= tblfeeaccount.Id where tbljoining_transaction.BillNo='" + _BillNo + "' and tbljoining_transaction.UserId=" + _StudentId;
        //    m_MyDataSet1 = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        //    _total_item_count = m_MyDataSet.Tables[0].Rows.Count;
        //    if (_total_item_count > 0)
        //    {

        //        ////sort to make discount at bottom
        //        //m_MyDataSet = sortMyDataSetToMakeDiscountValueAtBottom(m_MyDataSet);

        //        for (int k = 0; k < m_MyDataSet.Tables[0].Rows.Count; k++)
        //        {
        //            i++;
        //            _FeeName = m_MyDataSet.Tables[0].Rows[k][0].ToString();

        //            _Amount = double.Parse(m_MyDataSet.Tables[0].Rows[k][1].ToString());
        //            _TransType = int.Parse(m_MyDataSet.Tables[0].Rows[k][2].ToString());
        //            _AccountType = int.Parse(m_MyDataSet.Tables[0].Rows[k][3].ToString());
        //            _ScheduleId = int.Parse(m_MyDataSet.Tables[0].Rows[k][4].ToString());
        //            _Discount = 0;
        //            if (_AccountType == 1)//feee
        //            {
        //                _Discount = getDueDiscountFromMyDataSet(m_MyDataSet1, _ScheduleId);//dataset1 for discount.

        //            }
        //            if (_AccountType == 4)
        //            {
        //                _FeeName = "Fine For " + _FeeName;
        //            }

        //            if (_TransType != 1 && _AccountType == 1)//Regular student(1 meanse temp
        //            {
        //                _DueBalance = getDueBalanceFromTblstudentfeeschedule(_BillNo, _StudentId, _ScheduleId);
        //            }
        //            else
        //            {
        //                _DueBalance = 0;
        //            }
        //            if (_AccountType != 3)
        //            {
        //                Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);

        //                //a dded 2 times
        //                //Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //                Tbl_FeeDetails = AddOneItemToTableFeeDetails(Tbl_FeeDetails, i, _FeeName, _Discount, _DueBalance, _Amount, _AccountType);
        //            }

        //            if (!writerFRTemp.FitsPage(Tbl_FeeDetails))// not posible to fix
        //            {
        //                _Valid = false;

        //            }

        //        }
        //    }
        //    m_MyDataSet = null;

        //    // check how many no of empty rw can be add on the same page
        //    Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 2);
        //    // Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);

        //    _EmptyRwCount = 0;
        //    if (writerFRTemp.FitsPage(Tbl_FeeDetails))//  posible to fix
        //    {
        //        while (writerFRTemp.FitsPage(Tbl_FeeDetails))//  posible to fix
        //        {
        //            _EmptyRwCount++;
        //            Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 2);
        //            // Tbl_FeeDetails = getEmptyFeeItemTable(Tbl_FeeDetails, 1);


        //        }

        //    }
        //    Tbl_FeeDetails.DeleteLastRow();
        //    Tbl_FeeDetails.DeleteLastRow();
        //    return _Valid;
        //}

        //private iTextSharp.text.Table FR_LoadBillAndTempStudentInfoDetilsToTable(string _BillNo, DateTime _BillDate, int _StudentId, string _BatchName, int _ClassId)
        //{
        //    string _AdNO = "";
        //    string _Name = "";
        //    string _ClassName = "";

        //    iTextSharp.text.Table Tbl_Details = new iTextSharp.text.Table(5);
        //    Tbl_Details.Width = 100;
        //    float[] headerwidths = { 15, 20, 30, 15, 20 };
        //    Tbl_Details.Widths = headerwidths;
        //    Tbl_Details.Padding = 1;
        //    Tbl_Details.DefaultCell.Border = 0;


           
        //    //sql = "select tblstudent.StudentName, tblstudent.AdmitionNo, tblclass.ClassName from tblstudent inner join tblclass on   tblclass.Id=" + _ClassId + " AND tblstudent.Id=" + _StudentId;
        //    sql = " select tbltempstdent.Name, tbltempstdent.TempId, tblclass.ClassName from tbltempstdent inner join tblclass on  tblclass.Id=" + _ClassId + " AND tbltempstdent.Id=" + _StudentId;
          
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //    if (m_MyReader.HasRows)
        //    {
        //        m_MyReader.Read();
        //        _Name = m_MyReader.GetValue(0).ToString();
        //        _AdNO = m_MyReader.GetValue(1).ToString();
        //        _ClassName = m_MyReader.GetValue(2).ToString();

        //    }


        //    // fill data to table
        //    // first row
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("     Student Name  : ", MyFonts[3])); // Name
        //    Tbl_Details.DefaultCell.Colspan = 2;
        //    Tbl_Details.AddCell(new Phrase(_Name, MyFonts[4]));// bold
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("  Bill Number  : ", MyFonts[3])); //Bil no
        //    Tbl_Details.AddCell(new Phrase(_BillNo, MyFonts[4]));//
        //    // Second row
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("     Temporary ID  : ", MyFonts[3])); //Addmission No
        //    Tbl_Details.DefaultCell.Colspan = 2;
        //    Tbl_Details.AddCell(new Phrase(_AdNO, MyFonts[4]));
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("  Bill Date  : ", MyFonts[3])); //Bil; date
        //    Tbl_Details.AddCell(new Phrase(_BillDate.ToString("dd-MM-yyyy"), MyFonts[4]));

        //    // Third row
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("     Class  : ", MyFonts[3])); //Class name
        //    Tbl_Details.DefaultCell.Colspan = 2;
        //    Tbl_Details.AddCell(new Phrase(_ClassName, MyFonts[4]));
        //    Tbl_Details.DefaultCell.Colspan = 1;
        //    Tbl_Details.AddCell(new Phrase("  Batch  : ", MyFonts[3])); //Batch of student
        //    Tbl_Details.AddCell(new Phrase(_BatchName, MyFonts[4]));

        //    return Tbl_Details;
        //}

        //private void FR_LoadJoiningBilldetailsToVariablesFromTblfeebill(string _BillNo, out int _BillId, out int _CollegeId, out int _CourseId, out int _ClassId, out int _StudentId, out int _StudentType, out int _BillType, out DateTime _BillDate, out double _CashAmt, out double _ChequeAmt, out double _DDAmt, out double _TotalAmount, out string _ChequeNo, out string _ChequeBankName, out string _DDNo, out string _DDBankName, out int _CreatedUserId, out string _CreatedDate, out string _CreatedUser)
        //{
        //    // initializing the value
        //    string _PaymentMode = "";
        //    _BillId = 0;// primary index in the Tblfeebill used for giving unique name for the pdf
        //    _StudentId = 0;
        //    _StudentType = 0;
        //    _CourseId = 0;
        //    _ClassId = 0;
        //    _CollegeId = 0;
        //    _BillType = 0;
        //    _CreatedUserId = 0;
        //    _BillDate = new DateTime();
        //    _CreatedDate = "";
        //    _CashAmt = 0;
        //    _ChequeAmt = 0;
        //    _DDAmt = 0;
        //    _TotalAmount = 0;

        //    _ChequeNo = "";
        //    _ChequeBankName = "";
        //    _DDNo = "";
        //    _DDBankName = "";
        //    _CreatedUser = "";

        //    sql = "SELECT tbljoining_feebill.Id, tbljoining_feebill.StudentID, tbljoining_feebill.TotalAmount, tbljoining_feebill.UserId, tbluser.UserName, tbljoining_feebill.`Date`, tbltempstdent.Class, tbljoining_feebill.CreatedDateTime from tbljoining_feebill INNER JOIN tbluser on tbljoining_feebill.UserId= tbluser.Id inner join tbltempstdent on tbltempstdent.Id= tbljoining_feebill.StudentID where tbljoining_feebill.BillNo='" + _BillNo + "'";
        //    //Id(0),CollegeId(1),CourseId(2),ClassId(3),StudentId(4),StudentType(5),BillType(6),BillDate(7),CashAmt(8),ChequeAmt(9),DDAmt(10),TotalAmount(11),ChequeNo(12),ChequeBankName(13),DDNo(14),DDBankName(15),UserId(16),CreatedDate(17)
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //    if (m_MyReader.HasRows)
        //    {

        //        int.TryParse(m_MyReader.GetValue(0).ToString(), out _BillId);// primary index in the Tblfeebill used for giving unique name for the pdf
        //        int.TryParse(m_MyReader.GetValue(1).ToString(), out _StudentId);
        //        Double.TryParse(m_MyReader.GetValue(2).ToString(), out _TotalAmount);
        //        int.TryParse(m_MyReader.GetValue(3).ToString(), out _CreatedUserId);
        //        _CreatedDate =m_MyReader.GetValue(7).ToString();
        //        _BillDate = DateTime.Parse(m_MyReader.GetValue(5).ToString());
        //        _CreatedUser = m_MyReader.GetValue(4).ToString();
        //        int.TryParse(m_MyReader.GetValue(6).ToString(), out _ClassId);

        //        // int.TryParse(m_MyReader.GetValue(1).ToString(), out _CollegeId);
        //        //int.TryParse(m_MyReader.GetValue(2).ToString(), out _CourseId);
        //        // int.TryParse(m_MyReader.GetValue(5).ToString(), out _StudentType);
        //        // int.TryParse(m_MyReader.GetValue(6).ToString(), out _BillType);



        //        string sql1 = "SELECT  tbljoining_feebill.PaymentMode, tbljoining_feebill.TotalAmount, tbljoining_feebill.PaymentModeId, tbljoining_feebill.BankName FROM tbljoining_feebill where tbljoining_feebill.BillNo='" + _BillNo + "'";
        //        m_MyReader2 = m_MysqlDb.ExecuteQuery(sql1);
        //        if (m_MyReader2.HasRows)
        //        {
        //            _PaymentMode = m_MyReader2.GetValue(0).ToString();
        //            if (_PaymentMode == "Cash")
        //            {
        //                Double.TryParse(m_MyReader2.GetValue(1).ToString(), out _CashAmt);
        //            }
        //            if (_PaymentMode == "Cheque")
        //            {
        //                Double.TryParse(m_MyReader2.GetValue(1).ToString(), out _ChequeAmt);
        //                _ChequeNo = m_MyReader2.GetValue(2).ToString();
        //                _ChequeBankName = m_MyReader2.GetValue(3).ToString();
        //            }
        //            if (_PaymentMode == "Demand Draft")
        //            {
        //                Double.TryParse(m_MyReader2.GetValue(1).ToString(), out _DDAmt);
        //                _DDNo = m_MyReader2.GetValue(2).ToString();
        //                _DDBankName = m_MyReader2.GetValue(3).ToString();
        //            }
        //        }

        //    }
        //}

        //#endregion



        #region COMMON
        private string getHSCodeOfEachProductFromTblProduct(int _ItemId)// used for the PI And TI
        {
            string _HSCode = "";
            sql = " SELECT tblproduct.HSCode from tblproduct where tblproduct.Id=" + _ItemId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                _HSCode = m_MyReader.GetValue(0).ToString();
            }
            m_MyReader.Close();
            return _HSCode;
        }

        private HeaderFooter LoadHeaderIfExist(int _GroupId, string _physicalpath)
        {
            HeaderFooter DocHeader = null;
            // FOR HEADER
            string sql = "SELECT tblconfig.Value1 from tblconfig where tblconfig.Id=9";//ID 9 FOR HEADER
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int _temp = 0;
                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out _temp))
                {
                    if (_temp == 1)
                    {

                        Paragraph para_Header = new Paragraph();
                        iTextSharp.text.Table Tbl_Header = new iTextSharp.text.Table(1);
                        Tbl_Header.Width = 100;
                        Tbl_Header.Border = 0;
                        m_MyReader.Close();
                        sql = "select tblheader.HeaderImage from tblheader WHERE tblheader.GroupId=" + _GroupId;
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {
                            string _HeaderImgName = m_MyReader.GetValue(0).ToString();
                            if (_HeaderImgName != "")
                            {
                                string _ImagePathAndName = _physicalpath + "\\images\\" + _HeaderImgName;
                                iTextSharp.text.Image _ImgHeader = iTextSharp.text.Image.GetInstance(_ImagePathAndName);
                                Cell cell = new Cell();
                                cell.Add(_ImgHeader);
                                cell.Border = 0;
                                Tbl_Header.AddCell(cell);

                            }


                        }
                        m_MyReader.Close();
                        para_Header.Add(Tbl_Header);
                        DocHeader = new HeaderFooter(new Paragraph(para_Header), false);
                        DocHeader.Border = Rectangle.NO_BORDER;

                    }
                }
            }
            return DocHeader;

        }
       
        private iTextSharp.text.Table LoadTitle(string _Title)
        {
            iTextSharp.text.Table TblHeahing = new iTextSharp.text.Table(1);          
            TblHeahing.BorderWidth = 0;
            TblHeahing.Padding = 1;
            Cell HeadData = (new Cell(new Phrase("" + _Title, MyFonts[0])));
            TblHeahing.DefaultCell.HorizontalAlignment = 1;
            TblHeahing.DefaultCell.Border = 0;
            TblHeahing.AddCell(HeadData);
            return TblHeahing;
        }

        private iTextSharp.text.Table LoadCustomerDetails(iTextSharp.text.Table Tbl_Left, int _CustomerId)
        {

            string sql = "select tblcustomer.Name, tblcustomer.Address, tblcustomer.City, tblstate.State, tblcountry.Country, tblcustomer.Ph_Off,tblcustomer.Ph_Off2, tblcustomer.Ph_Fax,tblcustomer.Ph_Mob, tblcustomer.Email,tblcustomer.Website,tblcustomer.CstNo,tblcustomer.TinNo from tblcustomer  inner join  tblcountry on tblcountry.Id= tblcustomer.CountryId inner join tblstate on tblstate.Id= tblcustomer.StateId where tblcustomer.Id=" + _CustomerId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                string _PhOffice1and2 = "";
                string _FaxAndMobile = "";
                string _City = "";
                if (m_MyReader.GetValue(2).ToString() != "")// city
                {
                    _City = "  " + m_MyReader.GetValue(2).ToString() + " ,  ";
                }
                if (m_MyReader.GetValue(6).ToString() != "")//office ph 2
                {
                    _PhOffice1and2 = m_MyReader.GetValue(5).ToString() + " , " + m_MyReader.GetValue(6).ToString();
                }

                //  checking empty fax

                if (m_MyReader.GetValue(7).ToString() != "")
                {
                    _FaxAndMobile = "Fax  " + m_MyReader.GetValue(7).ToString();
                }
                if (m_MyReader.GetValue(8).ToString() != "")
                {
                    if (_FaxAndMobile != "")
                        _FaxAndMobile = _FaxAndMobile + "  , Mobile  " + m_MyReader.GetValue(8).ToString();
                    else
                        _FaxAndMobile = "  " + "Mobile  " + m_MyReader.GetValue(8).ToString();
                }

                Tbl_Left.DefaultCell.BorderWidthTop = 0;  //TO MAKE THE BOTOM LINE AFTER CMPNY ADDRESS 
                Tbl_Left.DefaultCell.BorderWidthBottom = 0;   //TO HIDE ALL  THE  BOTOM LINE IN CUSTOMER  ADDRESS                
                Tbl_Left.AddCell(new Phrase("  " + m_MyReader.GetValue(0).ToString(), MyFonts[1]));    //CUSTOMER NAME          
                Tbl_Left.DefaultCell.BorderWidthRight = 1;
                Tbl_Left.DefaultCell.BorderWidthTop = 0;
                if (m_MyReader.GetValue(1).ToString() != "")
                    Tbl_Left.AddCell(new Phrase("  " + m_MyReader.GetValue(1).ToString(), MyFonts[2]));//Local address


                Tbl_Left.AddCell(new Phrase((_City + m_MyReader.GetValue(3).ToString() + " , " + m_MyReader.GetValue(4).ToString()), MyFonts[2]));//CITY ,STATE ,COUNTRY

                if (_PhOffice1and2 != "")
                    Tbl_Left.AddCell(new Phrase("  " + _PhOffice1and2, MyFonts[2]));//PH OFF FAX

                if (_FaxAndMobile != "")
                    Tbl_Left.AddCell(new Phrase("  " + _FaxAndMobile, MyFonts[2]));

                // Tbl_Left.Cellspacing = 2;
                if (m_MyReader.GetValue(9).ToString() != "")
                    Tbl_Left.AddCell(new Phrase("  Email: " + m_MyReader.GetValue(9).ToString(), MyFonts[2]));
                if (m_MyReader.GetValue(10).ToString() != "")
                    Tbl_Left.AddCell(new Phrase("  Website: " + m_MyReader.GetValue(10).ToString(), MyFonts[2]));

                string _CustomerCstNoAndTinNo = "";

                if (m_MyReader.GetValue(11).ToString() != "")
                {
                    _CustomerCstNoAndTinNo = "  C.S.T NO : " + m_MyReader.GetValue(11).ToString() + " ;       ";
                }
                if (m_MyReader.GetValue(12).ToString() != "")
                {
                    _CustomerCstNoAndTinNo = _CustomerCstNoAndTinNo + "  TIN NO : " + m_MyReader.GetValue(12).ToString();
                }


                Tbl_Left.BorderWidthTop = 1;
                Tbl_Left.AddCell(new Phrase(_CustomerCstNoAndTinNo, MyFonts[3]));
                Tbl_Left.BorderWidthTop = 0;
            }
            m_MyReader.Close();
            return Tbl_Left;

        }
        private iTextSharp.text.Table LoadCompanyDetails(iTextSharp.text.Table Tbl_Left,int _CompanyId)
        {

            string sql = "select tblcompanydetails.Name, tblcompanydetails.Address, tblcompanydetails.City, tblcompanydetails.Pin, tblcompanydetails.State,tblcompanydetails.CountryName, tblcompanydetails.Ph_Off, tblcompanydetails.Ph_Fax,tblcompanydetails.Ph_Mob, tblcompanydetails.Email from tblcompanydetails where tblcompanydetails.Id=" + _CompanyId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                string _PhAndMobile = "";
                Tbl_Left.AddCell(new Phrase("  " + m_MyReader.GetValue(0).ToString(), MyFonts[1]));
                if(m_MyReader.GetValue(1).ToString()!="")
                Tbl_Left.AddCell(new Phrase("  " + m_MyReader.GetValue(1).ToString(), MyFonts[12]));
                if ((m_MyReader.GetValue(2).ToString() != "")|| (m_MyReader.GetValue(3).ToString()!="")||(m_MyReader.GetValue(4).ToString()!="")||(m_MyReader.GetValue(5).ToString()!=""))
                Tbl_Left.AddCell(new Phrase("  " + m_MyReader.GetValue(2).ToString() + " - " + m_MyReader.GetValue(3).ToString() + " , " + m_MyReader.GetValue(4).ToString() + " , " + m_MyReader.GetValue(5).ToString(), MyFonts[12]));

                if (m_MyReader.GetValue(6).ToString() != "")
                {
                    _PhAndMobile = "Phone  " + m_MyReader.GetValue(6).ToString();
                }
                if (m_MyReader.GetValue(7).ToString() != "")
                {
                    if (_PhAndMobile != "")
                        _PhAndMobile = _PhAndMobile + "  , Fax  " + m_MyReader.GetValue(7).ToString();
                    else
                        _PhAndMobile = "  " + "Fax  " + m_MyReader.GetValue(7).ToString();
                }
                if (_PhAndMobile != "")
                    Tbl_Left.AddCell(new Phrase("  " + _PhAndMobile, MyFonts[12]));          
                Tbl_Left.AddCell(new Phrase("  Mobile: " + m_MyReader.GetValue(8).ToString(), MyFonts[12]));
               // Tbl_Left.DefaultCell.BorderWidthBottom = 1;
                Tbl_Left.AddCell(new Phrase("  Email: " + m_MyReader.GetValue(9).ToString(), MyFonts[12]));


            }
            return Tbl_Left;
        }
        private iTextSharp.text.Table LoadAdvancePaymentDetails(int _CustomerId, int _transId)//OA
        {
            iTextSharp.text.Table Tbl_AdvancePayment = new iTextSharp.text.Table(3);
            Tbl_AdvancePayment.Width = 100;
            Tbl_AdvancePayment.Padding = 1;
            if (_transId > 0)
            {
                string sql = "select `Type`, tbltransaction.TransNo,tbltransaction.BankName,tbltransaction.Amount, tbltransaction.ReceivedDate,tbltransaction.ChequeDate from tbltransaction  where tbltransaction.Id=" + _transId;
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int _temp = 0;
                    if (int.TryParse(m_MyReader.GetValue(0).ToString(), out _temp))
                    {
                        string _type = "";

                        if (_temp == 0)
                        {
                            _type = "Cash";
                        }
                        else if (_temp == 1)
                        {
                            _type = "Cheque";
                        }
                        else if (_temp == 2)
                        {
                            _type = "Draft";
                        }
                        else if (_temp == 3)
                        {
                            _type = "PDC Cheque";
                        }
                        float[] width = { 30, 40, 30 };
                        Tbl_AdvancePayment.Widths = width;
                        Tbl_AdvancePayment.DefaultCellBorder = 0;

                        DateTime PaymentDt = DateTime.Parse(m_MyReader.GetValue(4).ToString());

                        Tbl_AdvancePayment.AddCell(new Phrase("Advance Received By " + _type, MyFonts[8]));
                        Tbl_AdvancePayment.AddCell(new Phrase(m_AmtType + ". " + m_MyReader.GetValue(3).ToString(), MyFonts[8]));
                        Tbl_AdvancePayment.AddCell(new Phrase("Payment Date : " + PaymentDt.ToString("dd-MM-yyyy"), MyFonts[8]));
                        if (_temp > 0)
                        {

                            Tbl_AdvancePayment.AddCell(new Phrase(_type + " No : " + m_MyReader.GetValue(1).ToString(), MyFonts[8]));

                            Tbl_AdvancePayment.AddCell(new Phrase("Bank Name : " + m_MyReader.GetValue(2).ToString(), MyFonts[8]));
                            if (_temp == 3)
                            {
                                DateTime chqDt = DateTime.Parse(m_MyReader.GetValue(5).ToString());
                                Tbl_AdvancePayment.AddCell(new Phrase("Cheque Date : " + chqDt.ToString("dd-MM-yyyy"), MyFonts[8]));
                            }
                        }

                    }

                }
                m_MyReader.Close();
            }
            else
            {
                Tbl_AdvancePayment.Border = 0;
                Tbl_AdvancePayment = null;
            }


            return Tbl_AdvancePayment;
        }

        private HeaderFooter LoadFooterIfExist(int _GroupId, string _physicalpath)
        {
            // FOR fOOTER
            HeaderFooter DocFooter = null;

            string sql = "SELECT tblconfig.Value1 from tblconfig where tblconfig.Id=10";//ID 10 FOR FOOTER
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int _temp = 0;
                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out _temp))
                {
                    if (_temp == 1)
                    {
                        Paragraph para_Footer = new Paragraph();
                        m_MyReader.Close();
                        sql = "select tblfooter.FooterImage, tblfooter.R1, tblfooter.R2 from tblfooter WHERE tblfooter.GroupId=" + _GroupId;
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {
                            string _FooterImgName = m_MyReader.GetValue(0).ToString();
                            if (_FooterImgName != "")
                            {
                                string _ImagePathAndName = _physicalpath + "\\images\\" + _FooterImgName;
                                iTextSharp.text.Image _ImgFooter = iTextSharp.text.Image.GetInstance(_ImagePathAndName);

                             
                                Chunk ckImg = new Chunk(_ImgFooter, 0, 0);
                                Chunk cknewline = new Chunk("\n");
                                Chunk ckfooter = new Chunk(m_MyReader.GetValue(1).ToString(), MyFonts[19]);
                                Chunk ckfooter1 = new Chunk(m_MyReader.GetValue(2).ToString(), MyFonts[19]);
                                Phrase p1 = new Phrase("");
                                p1.Add(cknewline);
                                p1.Add(ckfooter);
                                p1.Add(ckfooter1);
                                p1.Add(ckImg);
                                DocFooter = new HeaderFooter(p1, false);

                            }


                        }




                    }
                }
            }
            m_MyReader.Close();
            DocFooter.Alignment = Element.ALIGN_CENTER;
            DocFooter.Border = Rectangle.NO_BORDER;
            return DocFooter;


        }

        private iTextSharp.text.Table LoadMessageTable(string _MessageName, string _Message)
        {

            iTextSharp.text.Table Tbl_MsgTable = new iTextSharp.text.Table(1);
            Tbl_MsgTable.Width = 100;
            Tbl_MsgTable.DefaultCellBorder = 0;
            Tbl_MsgTable.Padding = 1;

            Tbl_MsgTable.AddCell(new Phrase(_MessageName, MyFonts[5]));
            Tbl_MsgTable.DefaultCell.HorizontalAlignment = 1;
            Tbl_MsgTable.AddCell(new Phrase("" + _Message, MyFonts[5]));
            Tbl_MsgTable.Border = 0;
            return Tbl_MsgTable;
        }

        private double FindTotalAmountFromGrid(GridView Grd_Products, int _CollCount)
        {
            double _TotalAmt = 0;
            double _TempTotalAmt = 0;

            if (Grd_Products.Rows.Count > 0)
            {
                foreach (GridViewRow gv in Grd_Products.Rows)
                {
                    double.TryParse(gv.Cells[_CollCount].Text.ToString(),out _TempTotalAmt);
                     _TotalAmt += _TempTotalAmt;
                }
            }

            _TotalAmt = Math.Round(_TotalAmt, 0);
            return _TotalAmt;
        }

        private iTextSharp.text.Table LoadSignAreaToTblConditiontable(iTextSharp.text.Table Tbl_Condition, int _ColCount)// USED FOR QTN,DC,PI, TI
        {

            string sql = "SELECT tblcompanydetails.SignTitle1, tblcompanydetails.AuthorityName1 FROM tblcompanydetails WHERE tblcompanydetails.Id=1;";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Cell EmptyRowForSign = new Cell(new Phrase("\n", MyFonts[15]));// empty row
                EmptyRowForSign.Colspan = _ColCount;
                EmptyRowForSign.BorderWidthTop = 0;
                EmptyRowForSign.BorderWidthBottom = 0;
                EmptyRowForSign.HorizontalAlignment = 2;


                Cell RowForSign1 = new Cell(new Phrase(m_MyReader.GetValue(0).ToString() + "   ", MyFonts[16]));// Company details
                RowForSign1.Colspan = _ColCount;
                RowForSign1.HorizontalAlignment = 2;
                RowForSign1.BorderWidthTop = 0;
                RowForSign1.BorderWidthBottom = 0;

                Cell RowForSign2 = new Cell(new Phrase("   " + m_MyReader.GetValue(1).ToString() + "   ", MyFonts[17]));
                RowForSign2.Colspan = _ColCount;
                RowForSign2.BorderWidthTop = 0;
                RowForSign2.BorderWidthBottom = 0;
                RowForSign2.HorizontalAlignment = 2;

                //ADDIN ROWTO TABLE
                RowForSign1.BorderWidthTop = 1;
                Tbl_Condition.AddCell(RowForSign1);

                Tbl_Condition.AddCell(EmptyRowForSign);

                Tbl_Condition.AddCell(RowForSign2);

                Tbl_Condition.AddCell(EmptyRowForSign);


            }
            m_MyReader.Close();
            return Tbl_Condition;

        }
        private void LoadMyFont()
        {

            //NEW
            MyFonts[0] = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);//Title(FR

            //Collage Details
            MyFonts[1] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15, iTextSharp.text.Font.BOLD);// CollageName  (RF
            MyFonts[2] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);// Collage ADDRESS (RF
            MyFonts[3] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);// Bill and Student etails              
            MyFonts[4] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD);// Bill and Student etails     
            MyFonts[5] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD);// Bill and Student etails       
            MyFonts[6] = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD);// Bill and Student etails       
            //message

            m_FntFeeItem = MyFonts[3];//empty table and Addone item functions
            m_FntFeeItemDesc = MyFonts[4];//
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
        #endregion OA_COMMON

        #region FeeRecipt





        internal bool CreateFeeRecipt(string _BillNo, string _physicalpath, out string _PdfName, out string _ErrorMsg, int _Value, FeeManage MyFeeMang, int _count)
        {
         
            bool _Created = false;

            _PdfName = "";
            _ErrorMsg = "";

            try
            {

                
                string StudentName = "", AdmissionNo = "", RollNo = "", ClassName = "",date="";
                MyFeeMang.GetStudentDetails(_BillNo, out StudentName, out AdmissionNo, out RollNo, out ClassName);

                string sql = "select distinct Date_Format(tblview_transaction.PaidDate,'%d-%m-%y') as PaidDate from tblview_transaction where tblview_transaction.BillNo='" + _BillNo + "'";
                
                OdbcDataReader MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                   date= MyReader.GetValue(0).ToString();
                }

                iTextSharp.text.Rectangle _pageSise = GetpageSizeFromTblTill();

                Document DocBill = new Document(_pageSise, 1, 1, 23, 1);
                //_PdfName = "R_"+_BillNo + ".pdf";

               
                sql = "select tblview_feebill.TransationId from tblview_feebill where tblview_feebill.BillNo='" + _BillNo + "'";
                m_MyReader =  MyFeeMang.m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                    _PdfName ="T_"+m_MyReader.GetValue(0).ToString() + ".pdf";
                else
                    _PdfName = "Temp.pdf";


                PdfWriter writerBill = PdfWriter.GetInstance(DocBill, new FileStream(_physicalpath + "\\PDF_Files\\" + _PdfName , FileMode.Create));
                
                DocBill.Open();
                //dominic 
                //iTextSharp.text.Table Tbl_Heade = new Table(4, 6);
                iTextSharp.text.Table Tbl_Heade = new iTextSharp.text.Table(4, 6);
                float[] _Width = { 5, 60, 16, 19 };
                Tbl_Heade.Widths = _Width;
                Tbl_Heade.DefaultCell.Border = 0;
                Tbl_Heade.Border = 0;
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell(new Phrase(StudentName, MyFonts[5]));
                
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell(new Phrase(_BillNo, MyFonts[5]));
                
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell(new Phrase( ClassName, MyFonts[5]));
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell(new Phrase(date, MyFonts[5]));


                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");
                Tbl_Heade.AddCell("\n");

           
                iTextSharp.text.Table FeeDetails = new iTextSharp.text.Table(4);
                float[] _Width1 = { 5, 65, 19, 11 };
                FeeDetails.Widths = _Width1;
                FeeDetails.DefaultCell.Border = 0;
                FeeDetails.Border = 0;
               
                  
               

                FeeDetails.AddCell("\n");
                FeeDetails.AddCell("\n");
                FeeDetails.AddCell("\n");
                FeeDetails.AddCell("\n");
                string sql1 = "SELECT  tblview_transaction.PeriodName as Period,tblview_transaction.FeeName AS 'Fee Name',tblaccount.AccountName,tblview_transaction.Amount,tblbatch.BatchName from tblview_transaction inner join tblaccount on tblaccount.Id= tblview_transaction.AccountTo inner join tblbatch on tblbatch.Id = tblview_transaction.BatchId inner join tblperiod on tblperiod.Id= tblview_transaction.PeriodId  where  tblview_transaction.BillNo='" + _BillNo + "' Order by tblbatch.BatchName ASC ,tblview_transaction.FeeId,tblperiod.`Order` ASC, tblaccount.AccountName DESC";
                DataSet data_Fee = MyFeeMang.m_TransationDb.ExecuteQueryReturnDataSet(sql1);
                int j = 0;
                if (data_Fee != null && data_Fee.Tables != null && data_Fee.Tables[0].Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in data_Fee.Tables[0].Rows)
                    {
                        FeeDetails.AddCell(new Phrase(i.ToString(), MyFonts[5]));
                        FeeDetails.AddCell(new Phrase(dr[1].ToString(), MyFonts[5]));
                        FeeDetails.AddCell(new Phrase(dr[0].ToString(), MyFonts[5]));
                        FeeDetails.AddCell(new Phrase(dr[3].ToString(), MyFonts[5]));
                        i++;j++;                        
                    }                      
                }

                for (int k=j; k < _count;k++)
                {
                    FeeDetails.AddCell("\n");
                    FeeDetails.AddCell("\n");
                    FeeDetails.AddCell("\n");
                    FeeDetails.AddCell("\n");
                }
               

                iTextSharp.text.Table Tbl_Footer = new iTextSharp.text.Table(4);
                float[] Footer_Width = { 12, 73, 7, 13 };
                Tbl_Footer.Widths = Footer_Width;
                Tbl_Footer.DefaultCell.Border = 0;
                Tbl_Footer.Border = 0;
                 string sql2 = "";

                sql2 = "select tblview_feebill.TotalAmount as Total ,date_format(tblview_feebill.Date,'%d-%m-%Y') AS 'PaidDate' from tblview_feebill where tblview_feebill.BillNo='" + _BillNo + "'";

                MyReader = MyFeeMang.m_TransationDb.ExecuteQuery(sql2);
                if (MyReader.HasRows)
                {
                    string _Amount = Convert_Number_To_Words(long.Parse(MyReader.GetValue(0).ToString()));
                    Tbl_Footer.AddCell("\n");
                    Tbl_Footer.AddCell( new Phrase(_Amount, MyFonts[6]));
                    
                    Tbl_Footer.AddCell("\n");
                    Tbl_Footer.AddCell(new Phrase(MyReader.GetValue(0).ToString(), MyFonts[6]));

                }


                DocBill.SetMargins(0, 0, 0, 0);
                DocBill.Add(Tbl_Heade);
                DocBill.Add(FeeDetails);
                DocBill.Add(Tbl_Footer);

              


                DocBill.Close();
              
            }
            catch(Exception e)
            {

                if (e.Message == "The process cannot access the file")
                    _ErrorMsg = "This Bill is used by another person.Try again later";
                return false;
            }
            
            return true;
        }



        private iTextSharp.text.Rectangle GetpageSizeFromTblTill()
        {
            int _height = 430;
            int _width = 430;
            iTextSharp.text.Rectangle _pageSize = new iTextSharp.text.Rectangle(_width, _height);
            return _pageSize;
        }

       
        #endregion FeeRecipt

       
    }
}
