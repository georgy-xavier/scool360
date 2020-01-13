using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace WinBase
{
    public class DateConversion
    {
        public static string GetFormatedDateVal(DateTime dt)
        {
            string d = "", m = "";
            if (dt.Date.Day < 10)
                d = "0";
            if (dt.Date.Month < 10)
                m = "0";
            return d + dt.Date.Day + "/" + m + dt.Date.Month + "/" + dt.Date.Year;
        }

        public static DateTime GetDateFromText(string _StrDate)
        {
            DateTime _outDate;
            string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
            int _Day, _Month, _Year;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            _outDate = new DateTime(_Year, _Month, _Day, 0, 0, 0);
            return _outDate;
        }
    }

    public class SupportTrackClass:KnowinGen
    {
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        private CLogging logger = null;
        public string table_type;

        public SupportTrackClass(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = _Prntobj.LoginUserName;
            m_userid = _Prntobj.User_Id;
            m_MysqlDb = new MysqlClass(this);
            logger = CLogging.GetLogObject();
        }

        ~SupportTrackClass()
        {

            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            } if (m_MyReader != null)
            {
                m_MyReader = null;

            }
            if (m_TransationDb != null)
            {
                m_TransationDb = null;

            }
            if (logger != null)
            {
                logger = null;

            } 

        }

        #region Transaction Area

        public void CreateTansationDb()
        {
            CLogging logger = CLogging.GetLogObject();
            logger.LogToFile("CreateTansationDb", "Starting New Fee Transaction", 'I', CLogging.PriorityEnum.LEVEL_LESS_IMPORTANT, LoginUserName);
            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(this);
            m_TransationDb.MyBeginTransaction();

        }
        public void EndSucessTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionCommit();
                m_TransationDb = null;
            }


        }
        public void EndFailTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

        }
       
        #endregion

        public string GetWarrantyId_From_ItemId(string ItemId)
        {
            //Dominic Updated
            MysqlClass MySqlObj = new MysqlClass();
            string warrantyId = "";
            string sql = "SELECT WarrantyId FROM tbl_spt_item WHERE Id=" + ItemId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                warrantyId = m_MyReader.GetValue(0).ToString();
            }
        
            MySqlObj.CloseConnection();
            return warrantyId;
        }

        public string GetWarrentyPeriod(string WarrantyId)
        {
            //Dominic Updated
            MysqlClass MySqlObj = new MysqlClass();
            string Periods = "";
            string sql = "SELECT Periods FROM tbl_spt_warranty WHERE Id=" + WarrantyId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Periods = m_MyReader.GetValue(0).ToString();
            }
            MySqlObj.CloseConnection();
            return Periods;
        }

        public string GetWarrentyPeriodUnit(string WarrantyId)
        {
            //Dominic Updated
            MysqlClass MySqlObj = new MysqlClass();
            string Periodunits = "";
            string sql = "SELECT PeriodUnit FROM tbl_spt_warranty WHERE Id=" + WarrantyId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Periodunits = m_MyReader.GetValue(0).ToString();
            }
            MySqlObj.CloseConnection();
            return Periodunits;
        }

        public void AddNewBuyer(string Name, string BuyerUniqueId, string Address1, string Address2, string Address3, string StateId, string CityId, string CountryId, string PinCode, string STD, string TelePhone, string Mobile, string Fax, string Email, string ContactPerson, out int BuyerId, MysqlClass _mysqldb)
        {
            // Arun changed parameter 20/11
            int Last_IncrementId = 0;
            if (BuyerUniqueId == "")
            {
                string Prefix = Get_BuyerIdPrefix(_mysqldb);
                Last_IncrementId = Get_BuyerLastIncrenetId(_mysqldb) + 1;
                BuyerUniqueId = Prefix + "-" + Last_IncrementId;
            }

            if (Name.Length > 100)
            {
                Name = Name.Substring(0, 98);
            }

            if (Address1.Length > 50)
            {
                Address1 = Address1.Substring(0, 48);
            }
            if (Address2.Length > 50)
            {
                Address2 = Address2.Substring(0, 48);
            }
            if (Address3.Length > 50)
            {
                Address3 = Address3.Substring(0, 48);
            }

            if (TelePhone.Length > 15)
            {
                TelePhone = TelePhone.Substring(0, 10);
            }

            if (Fax.Length > 15)
            {
                Fax = Fax.Substring(0, 13);
            }
            if (STD.Length > 6)
            {
                STD = STD.Substring(0, 5);
            }

            string sql = "";
            BuyerId = 0;
            sql = "INSERT INTO tbl_spt_buyer (Name,BuyerId,Address1,Address2,Address3,StateId,CityId,CountryId,PinCode,STD, TelePhone,Mobile,Fax,Email,ContactPerson,CreatedDate,IncrementIndex) VALUES ('" + Name.ToUpperInvariant().Trim().Replace("'", "") + "','" + BuyerUniqueId + "','" + Address1.ToUpperInvariant().Trim().Replace("'", "") + "', '" + Address2.ToUpperInvariant().Trim().Replace("'", "") + "','" + Address3.ToUpperInvariant().Trim().Replace("'", "") + "'," + StateId + "," + CityId + "," + CountryId + "," + PinCode + ",'" + STD + "','" + TelePhone + "','" + Mobile + "','" + Fax + "','" + Email + "','" + ContactPerson.ToUpperInvariant().Trim().Replace("'", "") + "','" + DateTime.Now.ToString("s") + "'," + Last_IncrementId + ")";
            _mysqldb.ExecuteQuery(sql);
            BuyerId = GetBuyerCreatedId(Name, BuyerUniqueId, CityId, _mysqldb);
        }

        private int GetBuyerCreatedId(string Name, string BuyerUniqueId, string CityId, MysqlClass _mysqldb)
        {
            //Dominic 20-10-2011
            // Changed Reader to DATASET

            // Arun Changed Parameter 20/11
            DataSet dt;
            int Id = 0;

            string sql = "SELECT Id AS Id FROM tbl_spt_buyer WHERE  Status=1 AND Name='" + Name + "' AND BuyerId='" + BuyerUniqueId + "' AND CityId=" + CityId;
            dt = _mysqldb.ExecuteQueryReturnDataSet(sql);

            if (dt != null && dt.Tables[0].Rows.Count>0)
            {
               int.TryParse(dt.Tables[0].Rows[0]["Id"].ToString(), out Id);
            }
            return Id;
        }


        #region Create Request Area
        public DataSet GetSerialNoSearchItem_DataSet(string _SearchText, int _SearchCondition)
        {
            DataSet _MydataSet;
            string sql;
            if (_SearchCondition == 0)
            {
                sql = "select tbl_spt_salesmaster.Id, tbl_spt_item.Name AS ItemName, tbl_spt_salesmaster.SerialNo, tbl_spt_sales.InvoiceNo, CONVERT(VARCHAR(10),tbl_spt_sales.InvoiceDate, 103) AS InvoiceDate, tbl_spt_buyer.Name AS BuyerName from tbl_spt_salesmaster inner join tbl_spt_item on tbl_spt_item.Id= tbl_spt_salesmaster.ItemId inner join tbl_spt_sales on tbl_spt_sales.MasterSaleId= tbl_spt_salesmaster.Id AND tbl_spt_sales.Level= tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id= tbl_spt_sales.BuyerId where tbl_spt_salesmaster.SerialNo='" + _SearchText + "'";
            }
            else if (_SearchCondition == 1)
            {
                sql = "select tbl_spt_salesmaster.Id, tbl_spt_item.Name AS ItemName, tbl_spt_salesmaster.SerialNo, tbl_spt_sales.InvoiceNo, CONVERT(VARCHAR(10), tbl_spt_sales.InvoiceDate, 103) AS InvoiceDate, tbl_spt_buyer.Name AS BuyerName from tbl_spt_salesmaster inner join tbl_spt_item on tbl_spt_item.Id= tbl_spt_salesmaster.ItemId inner join tbl_spt_sales on tbl_spt_sales.MasterSaleId= tbl_spt_salesmaster.Id AND tbl_spt_sales.Level= tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id= tbl_spt_sales.BuyerId where tbl_spt_sales.InvoiceNo='" + _SearchText + "'";
            }
            else if (_SearchCondition == 2)
            {
                sql = "select tbl_spt_salesmaster.Id, tbl_spt_item.Name AS ItemName, tbl_spt_salesmaster.SerialNo, tbl_spt_sales.InvoiceNo, CONVERT(VARCHAR(10), tbl_spt_sales.InvoiceDate , 103) AS InvoiceDate, tbl_spt_buyer.Name AS BuyerName from tbl_spt_salesmaster inner join tbl_spt_item on tbl_spt_item.Id= tbl_spt_salesmaster.ItemId inner join tbl_spt_sales on tbl_spt_sales.MasterSaleId= tbl_spt_salesmaster.Id AND tbl_spt_sales.Level= tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id= tbl_spt_sales.BuyerId where tbl_spt_buyer.Name='" + _SearchText + "'";
            }
            else
            {
                sql = "select tbl_spt_salesmaster.Id, tbl_spt_item.Name AS ItemName, tbl_spt_salesmaster.SerialNo, tbl_spt_sales.InvoiceNo, CONVERT(VARCHAR(10), tbl_spt_sales.InvoiceDate ,  103) AS InvoiceDate, tbl_spt_buyer.Name AS BuyerName from tbl_spt_salesmaster inner join tbl_spt_item on tbl_spt_item.Id= tbl_spt_salesmaster.ItemId inner join tbl_spt_sales on tbl_spt_sales.MasterSaleId= tbl_spt_salesmaster.Id AND tbl_spt_sales.Level= tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id= tbl_spt_sales.BuyerId where tbl_spt_buyer.BuyerId='" + _SearchText + "'";
            }

            _MydataSet = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
          
            return _MydataSet;
        }


        public int GetLastBuyerId(int _MasterSaleId)
        {
            MysqlClass _mysqldb = new MysqlClass();
            int _BuyerId = 0;
            string sql = "select tbl_spt_sales.BuyerId from tbl_spt_sales inner join tbl_spt_salesmaster on tbl_spt_salesmaster.Id= tbl_spt_sales.MasterSaleId AND tbl_spt_salesmaster.LastLevel= tbl_spt_sales.Level where tbl_spt_salesmaster.Id=" + _MasterSaleId;
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _BuyerId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return _BuyerId;
        }


        public string GetByerDetailHtml(int _BuyerId)
        {
            string _BuyerDetailString = "";

            _BuyerDetailString = UseRoundBox(GetByerDetaileTable(_BuyerId), "Buyer Details", "", "roundbox",350);
            return _BuyerDetailString;
        }


        private string UseRoundBox(string _InnerText, string _Heading, string _TopLink, string _RoundStyle,int _Minhight)
        {
            StringBuilder _RountBoxDetailString = new StringBuilder("");

            _RountBoxDetailString.Append("<div class=\"" + _RoundStyle + "\"><table width=\"100%\" cellspacing=\"0\"><tr><td class=\"topleft\"></td><td class=\"topmiddle\"></td><td class=\"topright\"></td></tr><tr><td class=\"centerleft\"></td><td class=\"centermiddle\">");
            _RountBoxDetailString.Append("<div style=\"min-height:" + _Minhight + "px;\">");
            _RountBoxDetailString.Append("<table width=\"100%\"><tr><td><h4>" + _Heading + "</h4></td><td style=\"text-align:right;\">" + _TopLink + "</td></tr></table>");

            _RountBoxDetailString.Append("<div class=\"linestyle\"></div>");

            _RountBoxDetailString.Append(_InnerText);
            _RountBoxDetailString.Append("</div>");
            _RountBoxDetailString.Append("</td><td class=\"centerright\"></td></tr><tr><td class=\"bottomleft\"></td><td class=\"bottommiddile\"></td><td class=\" bottomright\"></td></tr></table></div>");
            //"<a >Sales History>></a>
            return _RountBoxDetailString.ToString();
        }



        private string GetByerDetaileTable(int _BuyerId)
        {
            MysqlClass _mysqldb = new MysqlClass();

            StringBuilder _DetailString = new StringBuilder("");

            string sql = "select tbl_spt_buyer.Name, tbl_spt_buyer.BuyerId, tbl_spt_buyer.Address1, tbl_spt_buyer.Address2, tbl_spt_buyer.Address3, tblcity.City, tblstate.State, tblcountry.Country, tbl_spt_buyer.PinCode, tbl_spt_buyer.STD, tbl_spt_buyer.TelePhone, tbl_spt_buyer.Mobile, tbl_spt_buyer.Fax, tbl_spt_buyer.Email , tbl_spt_buyer.ContactPerson from tbl_spt_buyer inner join tblcity on tblcity.Id = tbl_spt_buyer.CityId inner join tblstate on tblstate.Id= tbl_spt_buyer.StateId inner join tblcountry on tblcountry.Id= tbl_spt_buyer.CountryId where tbl_spt_buyer.Id=" + _BuyerId;
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _DetailString.Append("<table class=\"tablelist\" >");
                _DetailString.Append(GetTRString("Buyer Name:",m_MyReader.GetValue(0).ToString()));
                _DetailString.Append(GetTRString("ID:", m_MyReader.GetValue(1).ToString()));
                _DetailString.Append(GetTRString("Address 1:", m_MyReader.GetValue(2).ToString()));
                _DetailString.Append(GetTRString("Address 2:", m_MyReader.GetValue(3).ToString()));
                _DetailString.Append(GetTRString("Address 3:", m_MyReader.GetValue(4).ToString()));
                _DetailString.Append(GetTRString("City:", m_MyReader.GetValue(5).ToString()));
                _DetailString.Append(GetTRString("State:", m_MyReader.GetValue(6).ToString()));
                _DetailString.Append(GetTRString("Countery:", m_MyReader.GetValue(7).ToString()));
                _DetailString.Append(GetTRString("Pin Code:", m_MyReader.GetValue(8).ToString()));
                _DetailString.Append(GetTRString("Telephone:",m_MyReader.GetValue(9).ToString()+"-"+m_MyReader.GetValue(10).ToString()));
                _DetailString.Append(GetTRString("Mobile:", m_MyReader.GetValue(11).ToString()));
                _DetailString.Append(GetTRString("Fax:", m_MyReader.GetValue(12).ToString()));
                _DetailString.Append(GetTRString("Email:", m_MyReader.GetValue(13).ToString()));
                _DetailString.Append(GetTRString("Contact Person:", m_MyReader.GetValue(14).ToString()));
           
                _DetailString.Append("</table>");


            }
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return _DetailString.ToString();

        }



        private string GetTRString(string _description, string _Value)
        {
            return "<tr> <td class=\"leftside\">" + _description + "</td><td class=\"rightside\">" + _Value + "</td></tr>";
        }


        private string GetTRColoredString(string _description, string _Value)
        {
            return "<tr> <td class=\"leftside\">" + _description + "</td><td class=\"rightside\" style=\"color:Red;\">" + _Value + "</td></tr>";
        }


        public string GetSalesDetailHtml(int _MasterSaleId)
        {
            string _SalesDetailString = "";
            
            _SalesDetailString = UseRoundBox(GetSalesDetaileTable(_MasterSaleId), "Sales Details", "<a href=\"SalesHistory.aspx?MasterSalesID=" + _MasterSaleId + "\" target = \"_blank\">Sales History>></a>", "roundbox", 350);
            return _SalesDetailString;
        }


        private string GetSalesDetaileTable(int _MasterSaleId)
        {
            MysqlClass _mysqldb = new MysqlClass();
            StringBuilder _DetailString = new StringBuilder("");

            string sql = "select tbl_spt_salesmaster.SerialNo, tbl_spt_item.Name, tbl_spt_warranty.Name, tbl_spt_warrantytype.Name, tbl_spt_sales.InvoiceNo, tbl_spt_salesmaster.FirstInvoiceDate, tbl_spt_salesmaster.ExpireDate from tbl_spt_salesmaster inner join tbl_spt_item on tbl_spt_item.Id= tbl_spt_salesmaster.ItemId inner join tbl_spt_warranty on tbl_spt_warranty.Id= tbl_spt_salesmaster.WarrentyId inner join  tbl_spt_warrantytype on tbl_spt_warrantytype.Id= tbl_spt_warranty.WarrantyTypeId inner join tbl_spt_sales on tbl_spt_sales.MasterSaleId= tbl_spt_salesmaster.Id AND tbl_spt_salesmaster.LastLevel= tbl_spt_sales.Level where tbl_spt_salesmaster.Id=" + _MasterSaleId;
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _DetailString.Append("<table class=\"tablelist\">");
                _DetailString.Append(GetTRString("Serial No:", m_MyReader.GetValue(0).ToString()));
                _DetailString.Append(GetTRString("Item Name:", m_MyReader.GetValue(1).ToString()));
                _DetailString.Append(GetTRString("Warranty:", m_MyReader.GetValue(2).ToString()));
                _DetailString.Append(GetTRString("Warranty Type:", m_MyReader.GetValue(3).ToString()));
                DateTime _ExpiyryDate=DateTime.Parse(m_MyReader.GetValue(6).ToString());
                string _WarrentyStatus="";
                if(_ExpiyryDate.Date<DateTime.Now.Date)
                   
                    _WarrentyStatus="OUT OF WARRANTY";
                else
                    _WarrentyStatus = "IN WARRANTY";
                
                _DetailString.Append(GetTRString("Warranty Status:",_WarrentyStatus));
                _DetailString.Append(GetTRString("Invoice No:", m_MyReader.GetValue(4).ToString()));
                _DetailString.Append(GetTRString("Invoice Date",DateConversion.GetFormatedDateVal(DateTime.Parse(m_MyReader.GetValue(5).ToString()))));
                _DetailString.Append(GetTRString("Expiry Date", DateConversion.GetFormatedDateVal(_ExpiyryDate)));
                _DetailString.Append(GetTRString_AMCRenewl(_MasterSaleId));
                _DetailString.Append("</table>");


            }
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return _DetailString.ToString();

        }

        private string GetTRString_AMCRenewl(int _MasterSaleId)
        {
            MysqlClass _mysqldb = new MysqlClass();
            string RenewalStr = "";
            string TRtr = "";
            string MainHeading = "AMC Renewals", Header1 = "Item", header2 = "Warranty", header3 = "Created Date";
            string sql = "SELECT tbl_spt_item.Name,tbl_spt_warranty.Name,tbl_spt_salesmaster.CreatedDate FROM tbl_spt_salesmaster INNER JOIN tbl_spt_item ON tbl_spt_item.Id=tbl_spt_salesmaster.ItemId INNER JOIN tbl_spt_warranty ON tbl_spt_warranty.Id=tbl_spt_salesmaster.WarrentyId WHERE tbl_spt_salesmaster.OldSaleId=" + _MasterSaleId + " ORDER BY tbl_spt_salesmaster.CreatedDate";
            OdbcDataReader Reader = _mysqldb.ExecuteQuery(sql);
            if (Reader.HasRows)
            {
                while (Reader.Read())
                {

                    string Name = Reader.GetValue(0).ToString();
                    string Warranty = Reader.GetValue(1).ToString();
                    DateTime CreatedOn = DateTime.Parse(Reader.GetValue(2).ToString());
                    TRtr = TRtr + "<tr>  <td class=\"CellStyle\">   " + Name + "   </td>   <td class=\"CellStyle\">   " + Warranty + "  </td>   <td class=\"CellStyle\">  " + DateConversion.GetFormatedDateVal(CreatedOn) + " </td> </tr>";
                
                }
                RenewalStr = "<tr>  <td colspan=\"2\">   <br />  <h4>" + MainHeading + "</h4>  <div class=\"linestyle\">   </div>   <div style=\"height:100px;overflow:auto\">  <table width=\"100%\">   <tr>   <td class=\"SubHeaderStyle\">  " + Header1 + "   </td>  <td class=\"SubHeaderStyle\">    " + header2 + "    </td>   <td class=\"SubHeaderStyle\">    " + header3 + "   </td>   </tr>   " + TRtr + "  </table>  </div> </td> </tr>";
            }
            Reader.Close();
            _mysqldb.CloseConnection();
            return RenewalStr;
        }


        public string GetSupportDetailHtml(int _MasterSaleId)
        {
            string _SupportDetailString = "";
            _SupportDetailString = UseRoundBox(GetSupportDetaileTable(_MasterSaleId), "Support Details", "", "roundbox",100);
            return _SupportDetailString;
        }



        private string GetSupportDetaileTable(int _MasterSaleId)
        {
            StringBuilder _DetailString = new StringBuilder("");


            _DetailString.Append("<table class=\"tablelist\">");
            _DetailString.Append(GetTRString("No Of Support Request:", GetN0OfSupportRequest(_MasterSaleId).ToString()));
            _DetailString.Append(GetTRString("No Of Replacements:", GetN0OfReplacements(_MasterSaleId).ToString()));
            _DetailString.Append(GetTRString("Total Cost:",Math.Round(GetTotalSupportCost(_MasterSaleId),2).ToString()));

            _DetailString.Append("</table>");

            return _DetailString.ToString();
        }



        private double GetTotalSupportCost(int _MasterSaleId)
        {
            double _Cost = 0;

            string sql = "select SUM(tbl_view_supportactions.Cost) from  tbl_view_supportrequest inner join tbl_view_supportactions on tbl_view_supportrequest.Id= tbl_view_supportactions.SupportRequestId where tbl_view_supportrequest.IsActive=1 AND tbl_view_supportrequest.MasterSalesId=" + _MasterSaleId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if(!double.TryParse(m_MyReader.GetValue(0).ToString(),out _Cost))
                {
                    _Cost = 0;
                }
               
            }
            m_MyReader.Close();
             m_MysqlDb.CloseExistingConnection();
            return _Cost;
        }



        private int GetN0OfReplacements(int _MasterSaleId)
        {
            int _nReplacements=0;

            string sql = "select COUNT(tbl_view_supportactions.ActionId) from  tbl_view_supportrequest inner join tbl_view_supportactions on tbl_view_supportrequest.Id= tbl_view_supportactions.SupportRequestId where tbl_view_supportactions.ActionId=2 AND tbl_view_supportrequest.IsActive=1 AND tbl_view_supportrequest.MasterSalesId=" + _MasterSaleId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (!int.TryParse(m_MyReader.GetValue(0).ToString(), out _nReplacements))
                {
                    _nReplacements = 0;
                }

            }
            m_MyReader.Close();
             m_MysqlDb.CloseExistingConnection();
            return _nReplacements;
        }



        private int GetN0OfSupportRequest(int _MasterSaleId)
        {
            int _nSupports = 0;
            string sql = "select COUNT(tbl_view_supportrequest.Id) from  tbl_view_supportrequest where  tbl_view_supportrequest.IsActive=1 AND tbl_view_supportrequest.MasterSalesId=" + _MasterSaleId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (!int.TryParse(m_MyReader.GetValue(0).ToString(), out _nSupports))
                {
                    _nSupports = 0;
                }

            }
            m_MyReader.Close();
             m_MysqlDb.CloseExistingConnection();
            return _nSupports;
        }

        #endregion




        public void Insert_OtherSales(int BuyerId, int Item_Id, DateTime Invoice_Date, string InvoiceNo, int Qty, int BranchId, int WarrantyId, DateTime Expire_Date, string CreatedBy)
        {
            if(InvoiceNo.Length>50)
            {
                InvoiceNo = InvoiceNo.Substring(0, 48);
            }
            string sql = "INSERT INTO tbl_spt_othersales (BuyerId,SerialNo,ItemId,FirstInvoiceDate,InvoiceNo,Quantity,BranchId,WarrentyId,LastLevel,ExpireDate,CreatedDate,CreatedUser) VALUES (" + BuyerId + ",0," + Item_Id + ",'" + Invoice_Date.ToString("s") + "','" + InvoiceNo + "'," + Qty + "," + BranchId + "," + WarrantyId + ",0,'" + Expire_Date.ToString("s") + "','" + DateTime.Now.ToString("s") + "','" + CreatedBy + "')";
            m_TransationDb.ExecuteQuery(sql);
        }


        public void Insert_SalesMaster(string SerialNo, string ItemId, DateTime FirstInvoiceDate, string WarrentyId, string CreatedBy, DateTime ExpireDate, out int MasterId, MysqlClass _mysqldb)
        {
            //Arun added parameter on 21/11/2011
            string sql = "";
            MasterId = 0;
            sql = "INSERT INTO tbl_spt_salesmaster (SerialNo,ItemId,FirstInvoiceDate,WarrentyId,LastLevel,CreatedDate,CreatedUser,ExpireDate) VALUES ('" + SerialNo + "'," + ItemId + ",'" + FirstInvoiceDate.ToString("s") + "'," + WarrentyId + ",0,'" + DateTime.Now.ToString("s") + "','" + CreatedBy + "','" + ExpireDate.ToString("s") + "')";
            _mysqldb.ExecuteQuery(sql);
            MasterId = GetSalesMaster_CreatedId(SerialNo, ItemId, CreatedBy, _mysqldb);
        }

        private int GetSalesMaster_CreatedId(string SerialNo, string ItemId, string CreatedBy,MysqlClass _mysqldb)
        {
            //Dominic 20-10-2011 Changed 
            // Updated M_reader To MYDATASET
            // Arun added parameter on 21/11/2011
            int Id = 0;
            string sql = "SELECT Id as Id FROM tbl_spt_salesmaster WHERE Status=1 AND SerialNo='" + SerialNo + "' AND ItemId=" + ItemId + " AND CreatedUser='" + CreatedBy + "'";
            DataSet dt = _mysqldb.ExecuteQueryReturnDataSet(sql);
            if (dt!=null && dt.Tables[0].Rows.Count>0)
            {
                int.TryParse(dt.Tables[0].Rows[0]["Id"].ToString(), out Id);
            }
            return Id;
        }

        public void Insert_Sale(int MasterSaleId, int BuyerId, string SerialNo, string InvoiceNo, DateTime InvoiceDate, string BranchId, string CreatedUser, MysqlClass _mysqldb)
        {
            // Arun Added parameter 20/11/2011
            string srtinvoicedate = "";
            if (InvoiceDate != new DateTime())
            {
                srtinvoicedate = InvoiceDate.ToString("s");
            }

            if(InvoiceNo.Length>50)
            {
                InvoiceNo = InvoiceNo.Substring(0, 48);
            }
            int Level = Get_SalesMaster_Level(MasterSaleId, _mysqldb) + 1;
            string sql = "INSERT INTO tbl_spt_sales (MasterSaleId,BuyerId,Level,SerialNo,InvoiceNo,InvoiceDate,BranchId,CreatedDate,CreatedUser) VALUES (" + MasterSaleId + "," + BuyerId + "," + Level + ",'" + SerialNo + "','" + InvoiceNo + "','" + srtinvoicedate + "'," + BranchId + ",'" + DateTime.Now.ToString("s") + "','" + CreatedUser + "')";
            _mysqldb.ExecuteQuery(sql);
            Update_Salesmaster_Level(MasterSaleId, Level, _mysqldb);
        }

        private void Update_Salesmaster_Level(int MasterSaleId, int Level, MysqlClass _mysqldb)
        {
            // Arun Added parameter 21/11/2011
            string sql = "Update tbl_spt_salesmaster Set LastLevel=" + Level + " WHERE Status=1 AND Id=" + MasterSaleId;
            _mysqldb.ExecuteQuery(sql);
        }

        public int Get_SalesMaster_Level(int MasterSaleId, MysqlClass _mysqldb)
        {
            //DOMINIC 20-10-2011
            // Changes:- Updated M_MyReader TO DATASET
            // Arun added parameter 21/11/2011
         
            int LastLevel = 0;
            string sql = "SELECT LastLevel as LastLevel FROM tbl_spt_salesmaster WHERE Status=1 AND Id=" + MasterSaleId;
            DataSet Dt = _mysqldb.ExecuteQueryReturnDataSet(sql);
            
            if (Dt != null && Dt.Tables[0].Rows.Count>0)
            {
                int.TryParse( Dt.Tables[0].Rows[0]["LastLevel"].ToString(), out LastLevel);
            }

            return LastLevel;
        }





        private int Get_BuyerLastIncrenetId(MysqlClass _mysqldb)
        {
            //Dominic  20-10-2011 Updated this query .
            //changed m_MyReader To Dataset
            // Arun Changed parameter 20/11

            DataSet Dt;
            int Lastid = 0;
            string sql = "SELECT Max(IncrementIndex)as maxId FROM tbl_spt_buyer";

            Dt = _mysqldb.ExecuteQueryReturnDataSet(sql);
           
            if (Dt != null && Dt.Tables!=null && Dt.Tables[0].Rows.Count>0)
            {
                int.TryParse(Dt.Tables[0].Rows[0]["maxId"].ToString(), out Lastid);
            }
            return Lastid;
        }

        public string Get_BuyerIdPrefix(MysqlClass _mysqldb)
        {
            //Dominic  20-10-2011 Updated this query .
            //changed m_MyReader To Dataset
            // Arun Added parameter 21/10

            DataSet Dt;
            string Id = "";
            string sql = "SELECT Value as Value FROM tblconfiguration WHERE Name='BuyerIdPrefix'";

            Dt = _mysqldb.ExecuteQueryReturnDataSet(sql);
            
       
            if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
            {
               Id =Dt.Tables[0].Rows[0]["Value"].ToString();
            }
            return Id;
        }

        public int Get_ResponseVariation()
        {
            //Dominic  20-10-2011 Updated this query .
            //changed m_MyReader To Dataset

            MysqlClass _mysqltemp = new MysqlClass();
            DataSet Dt;
            int Id = 0;
            string sql = "SELECT Value as Value FROM tblconfiguration WHERE Name='Response Variation'";
            if (m_TransationDb == null)
            {
                Dt = _mysqltemp.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Dt = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }

            if (Dt != null && Dt.Tables != null && Dt.Tables[0].Rows.Count > 0)
            {
                  int.TryParse( Dt.Tables[0].Rows[0]["Value"].ToString(),out Id);
            }
            _mysqltemp.CloseConnection();
           return Id;
        }
        public int Get_ResolutionVariation()
        {
            MysqlClass _mysqltemp = new MysqlClass();
            int Id = 0;
            string sql = "SELECT Value FROM tblconfiguration WHERE Name='Resolution Variation'";
            if (m_TransationDb == null)
            {
                m_MyReader = _mysqltemp.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                Id = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            m_MyReader.Close();
            _mysqltemp.CloseConnection();
            return Id;
        }

        public void CreateRequest(string CallId, string MasterSalesId, string WarrentyTypeId, string TargetResponceDate, string TargetResolutionDate, string ProblemComment, string CreatedUser, out int Requestid, int UserId, int GroupId)
        {
            DateTime ResponceDate = DateConversion.GetDateFromText(TargetResponceDate);
            DateTime ResolutionDate = DateConversion.GetDateFromText(TargetResolutionDate);
            Requestid = GetTableMaxId("tbl_view_supportrequest","Id",m_TransationDb);
            string sql = "INSERT INTO tbl_spt_supportrequest(Id,CallId,MasterSalesId,WarrentyTypeId,TargetResponceDate,TargetResolutionDate,ProblemComment,CreatedDate,CreatedUser,UserId,GroupId) VALUES (" + Requestid + "," + CallId + "," + MasterSalesId + "," + WarrentyTypeId + ",'" + ResponceDate.ToString("s") + "','" + ResolutionDate.ToString("s") + "','" + ProblemComment + "','" + DateTime.Now.ToString("s") + "','" + CreatedUser + "'," + UserId + "," + GroupId + ")";
            m_TransationDb.ExecuteQuery(sql);
            //Requestid = GetRequest_CreatedId(CallId, MasterSalesId, CreatedUser, DateTime.Now.ToString("s"), ResponceDate, ResolutionDate);
        }

        private int GetRequest_CreatedId(string CallId, string MasterSalesId, string CreatedUser, string Date, DateTime ResponceDate, DateTime ResolutionDate)
        {
            int Id = 0;
            string sql = "SELECT Id FROM tbl_view_supportrequest WHERE IsActive=1 AND RequestStatus=1 AND CallId=" + CallId + " AND TargetResponceDate='" + ResponceDate.ToString("s") + "' AND TargetResolutionDate='" + ResolutionDate.ToString("s") + "' AND MasterSalesId=" + MasterSalesId + " AND CreatedUser='" + CreatedUser + "' AND CreatedDate='" + Date + "'";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Id = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return Id;
        }


        public void Create_RequestAction(string CallId,int SupportRequestId, int ActionId, DateTime ActionDate, string Engineer, int Cost, string Comment, string CreateUser, string ActionSpecValue,int OldSerialId,int UserId,int GroupId)
        {
            string sql = "INSERT INTO tbl_spt_supportactions(CallId,SupportRequestId,ActionId,ActionDate,Engineer,Cost,Comment,CreateUser,CreatedDate,ActionSpecValue,OldSerialId,UserId,GroupId) VALUES (" + CallId + "," + SupportRequestId + "," + ActionId + ",'" + ActionDate.ToString("s") + "','" + Engineer + "'," + Cost + ",'" + Comment + "','" + CreateUser + "','" + DateTime.Now.ToString("s") + "','" + ActionSpecValue + "'," + OldSerialId + "," + UserId + "," + GroupId + ")";
            m_TransationDb.ExecuteQuery(sql);
            UpdateRequestStatus(SupportRequestId, ActionId);
       
        }

        private void UpdateRequestStatus(int SupportRequestId, int ActionId)
        {
            string sql = "Update tbl_spt_supportrequest Set RequestStatus=" + ActionId + " WHERE IsActive=1 AND Id=" + SupportRequestId;
            m_TransationDb.ExecuteQuery(sql);
        }



        public string GetRequestDetailHtml(int Requestid, int _MasterSaleId)
        {
            string _DetailString = "";

            _DetailString = UseRoundBox(GetRequestDetaileTable(Requestid), "Request Details", "<a href=\"RequestHistory.aspx?MasterSalesID=" + _MasterSaleId + "\" target = \"_blank\">More Support History>></a>", "roundbox", 350);
            return _DetailString;
        }

        private string GetRequestDetaileTable(int Requestid)
        {
            StringBuilder _DetailString = new StringBuilder("");
            MysqlClass _mysqldb = new MysqlClass();
            string sql = "SELECT tbl_view_supportrequest.CallId,CONVERT(VARCHAR(10),tbl_view_supportrequest.CreatedDate, 103),tbl_spt_warrantytype.Name,tbl_view_supportrequest.ProblemComment,CONVERT(VARCHAR(10),tbl_view_supportrequest.TargetResolutionDate, 103),tbl_spt_action.Name FROM tbl_view_supportrequest INNER JOIN tbl_spt_warrantytype ON tbl_spt_warrantytype.Id=tbl_view_supportrequest.WarrentyTypeId INNER JOIN tbl_spt_action ON tbl_spt_action.Id=tbl_view_supportrequest.RequestStatus WHERE tbl_view_supportrequest.Id=" + Requestid;
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _DetailString.Append("<table class=\"tablelist\">");
                _DetailString.Append(GetTRString("Request No:", m_MyReader.GetValue(0).ToString()));
                _DetailString.Append(GetTRString("Created Date:", m_MyReader.GetValue(1).ToString()));
                _DetailString.Append(GetTRString("Warranty Type:", m_MyReader.GetValue(2).ToString()));
                _DetailString.Append(GetTRString("Description:", m_MyReader.GetValue(3).ToString()));
                _DetailString.Append(GetTRString("Target Completion Date:", m_MyReader.GetValue(4).ToString()));
                _DetailString.Append(GetTRString("Status:", m_MyReader.GetValue(5).ToString()));
            }

            _DetailString.Append(GetTRColoredString("Service Cost:", Get_EachCostForRequest(Requestid, 3)));
            _DetailString.Append(GetTRColoredString("Replace Cost:", Get_EachCostForRequest(Requestid, 1)));
            _DetailString.Append(GetTRColoredString("Repair Cost:", Get_EachCostForRequest(Requestid, 2)));
            _DetailString.Append(GetTRColoredString("Miscellaneous Cost:", Get_EachCostForRequest(Requestid, 4)));
            _DetailString.Append(GetTRColoredString("Total Cost:", Get_TotalCostForRequest(Requestid)));
            _DetailString.Append("</table>");
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return _DetailString.ToString();
        }

        public string Get_TotalCostForRequest(int Requestid)
        {
            int cost = 0;
            MysqlClass _mysql = new MysqlClass();
            string costreturn = "0";
            string sql = "SELECT SUM(tbl_view_supportactions.Cost) FROM tbl_view_supportactions INNER JOIN tbl_spt_action ON  tbl_spt_action.Id=tbl_view_supportactions.ActionId WHERE tbl_view_supportactions.SupportRequestId=" + Requestid;
            m_MyReader = _mysql.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out cost))
                {
                    costreturn = cost.ToString();
                }
            }
            m_MyReader.Close();
            _mysql.CloseConnection();
            return costreturn;
        }

        public string Get_EachCostForRequest(int Requestid, int CostheadId)
        {
            MysqlClass _mysql = new MysqlClass();
            int cost = 0;
            string costreturn = "0";
            string sql = "SELECT SUM(tbl_view_supportactions.Cost) FROM tbl_view_supportactions INNER JOIN tbl_spt_action ON  tbl_spt_action.Id=tbl_view_supportactions.ActionId WHERE tbl_spt_action.CostHead=" + CostheadId + " AND tbl_view_supportactions.SupportRequestId=" + Requestid;
            m_MyReader = _mysql.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (int.TryParse(m_MyReader.GetValue(0).ToString(), out cost))
                {
                    costreturn = cost.ToString();
                }
            }
            m_MyReader.Close();
            _mysql.CloseConnection();
            return costreturn;
        }



        public void CloseRequest(int SupportRequestId, string InvoiceNo, string InvoiceDate)
        {
            string date = "";
            if (InvoiceDate != "")
            {
                date =",InvoiceDate='"+ DateConversion.GetDateFromText(InvoiceDate).ToString("s")+"'";
            }
            string sql = "Update tbl_spt_supportrequest Set ClosingDate='" + DateTime.Now.ToString("s") + "',InvoiceNo='" + InvoiceNo + "'" + date + " WHERE IsActive=1 AND Id=" + SupportRequestId;
            m_TransationDb.ExecuteQuery(sql);
        }

        public int GetLastDayOfMonth(DateTime _date)
        {
            //dominic Changed the function
            // Now Calculate the last day dirctly .
            int lastday = 0;
           
            DateTime today = DateTime.Today;
            DateTime lastDayOfThisMonth = new DateTime(_date.Year, _date.Month, 1).AddMonths(1).AddDays(-1);
            lastday = lastDayOfThisMonth.Date.Day;
                       
            return lastday;
        }


        public DateTime GetInvoiceDate(string SalesMasterId)
        {
            DateTime InvoiceDate = new DateTime();
            string sql = "SELECT tbl_spt_salesmaster.FirstInvoiceDate FROM tbl_spt_salesmaster WHERE tbl_spt_salesmaster.Id=" + SalesMasterId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
               DateTime.TryParse(m_MyReader.GetValue(0).ToString(),out InvoiceDate);
            }
            m_MyReader.Close();
             m_MysqlDb.CloseExistingConnection();
            return InvoiceDate;
        }

        #region AMC Management
        public OdbcDataReader Get_Category()
        {
            //string sql = "select name, id from tbl_spt_itemcategory where status=1 order by Name";
            string sql = "select type, id from tbl_spt_itemtype";
            m_MyReader=m_MysqlDb.ExecuteQuery(sql);
            return m_MyReader;
        }
        public OdbcDataReader Get_Items(int _TypeId)
        {
            string sql = "";
            if (_TypeId == 0)
                sql = "select Id, Name, Code from tbl_spt_item where status=1 order by Name";
            else
                sql = "select Id, Name, Code from tbl_spt_item where TypeId = " + _TypeId + " and status=1 order by Name";
            return m_MysqlDb.ExecuteQuery(sql);
        }
        public OdbcDataReader Get_AMC_Details(string AMCStatus,int _typeId, int _Id, DateTime sdate, DateTime edate, int i, string _customer, bool _ExpiredOnly)
        {
            //dominic changed the queries- Date Format

            string sql = "", customer = "", item = "", type = "";
            string expire = "where tbl_spt_salesmaster.ExpireDate between '" + sdate.ToString("yyyy-MM-dd") + "' and '" + edate.ToString("yyyy-MM-dd") + "'";

            if (i == 1)
            {
                sql = ""; customer = ""; item = ""; expire = "";
                customer = "where tbl_spt_buyer.Name = '" + _customer + "'";
            }
            else
            {
                customer = "";
                if (_typeId != 0)
                {
                    type = " and tbl_spt_ItemType.id =" + _typeId + "";
                }
                if (_Id != 0)
                {
                    item = "and tbl_spt_salesmaster.ItemId = " + _Id + "";
                }
                if (_ExpiredOnly)
                {
                    expire = "where tbl_spt_salesmaster.ExpireDate < '" + sdate.ToString("yyyy-MM-dd") + "' ";
                }
            }
            //sql = "select tbl_spt_salesmaster.Id, tbl_spt_item.Name, tbl_spt_item.Code , tbl_spt_item.Model, tbl_spt_itemtype.type, tbl_spt_buyer.Name,tbl_spt_amcstatus.Status , tbl_spt_salesmaster.SerialNo, date_format( tbl_spt_salesmaster.ExpireDate , '%d/%m/%Y') from tbl_spt_item inner join tbl_spt_salesmaster on tbl_spt_salesmaster.ItemId = tbl_spt_item.id inner join tbl_spt_itemtype on tbl_spt_item.TypeId = tbl_spt_item.TypeId inner join tbl_spt_sales on  tbl_spt_sales.MasterSaleId = tbl_spt_salesmaster.Id inner join tbl_spt_buyer on tbl_spt_buyer.Id = tbl_spt_sales.BuyerId inner join tbl_spt_amcstatus on tbl_spt_salesmaster.AMCStatus = tbl_spt_amcstatus.Id "+ expire  + customer + item +  " GROUP by tbl_spt_salesmaster.SerialNo";

            // Dominic changed the query
            sql = "select tbl_spt_salesmaster.Id,tbl_spt_buyer.Id, tbl_spt_item.Name, tbl_spt_item.Code , tbl_spt_item.Model, tbl_spt_itemtype.type, tbl_spt_buyer.Name,tbl_spt_amcstatus.Status , tbl_spt_salesmaster.SerialNo,  CONVERT(VARCHAR(10), tbl_spt_salesmaster.ExpireDate, 103) AS ExpireDate from tbl_spt_item inner join tbl_spt_salesmaster on tbl_spt_salesmaster.ItemId = tbl_spt_item.id inner join tbl_spt_itemtype on tbl_spt_item.TypeId = tbl_spt_itemtype.Id inner join tbl_spt_sales on  tbl_spt_sales.MasterSaleId = tbl_spt_salesmaster.Id and tbl_spt_sales.Level = tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id = tbl_spt_sales.BuyerId inner join tbl_spt_amcstatus on tbl_spt_salesmaster.AMCStatus = tbl_spt_amcstatus.Id " + expire + customer + item + type + " and tbl_spt_salesmaster.AMCStatus=" + AMCStatus + " and  tbl_spt_item.Status = 1 and tbl_spt_salesmaster.OldSaleId=0 GROUP by tbl_spt_salesmaster.SerialNo,tbl_spt_salesmaster.Id,tbl_spt_buyer.Id, tbl_spt_item.Name, tbl_spt_item.Code , tbl_spt_item.Model, tbl_spt_itemtype.type, tbl_spt_buyer.Name,tbl_spt_amcstatus.Status , tbl_spt_salesmaster.ExpireDate";
           
            //select tbl_spt_salesmaster.Id, tbl_spt_item.Name, tbl_spt_item.Code , tbl_spt_item.Model, tbl_spt_itemtype.type, tbl_spt_buyer.Name,tbl_spt_amcstatus.Status , tbl_spt_salesmaster.SerialNo, date_format( tbl_spt_salesmaster.ExpireDate , '%d/%m/%Y') from tbl_spt_item inner join tbl_spt_salesmaster on tbl_spt_salesmaster.ItemId = tbl_spt_item.id inner join tbl_spt_itemtype on tbl_spt_item.TypeId = tbl_spt_item.TypeId inner join tbl_spt_sales on  tbl_spt_sales.MasterSaleId = tbl_spt_salesmaster.Id and tbl_spt_sales.Level = tbl_spt_salesmaster.LastLevel inner join tbl_spt_buyer on tbl_spt_buyer.Id = tbl_spt_sales.BuyerId inner join tbl_spt_amcstatus on tbl_spt_salesmaster.AMCStatus = tbl_spt_amcstatus.Id
            return m_MysqlDb.ExecuteQuery(sql);
        }


        public void InsertToAmcLog(int _MasterSaleId, int _DoneStatus, string _uName, DateTime _crDt,MysqlClass _mysqldb)
        {
            // Arun added parameter on 21/10/2011
            string sql = "INSERT INTO tbl_spt_amcactionlog (MasterSaleId, DoneStatus, UserName, CreatedDate) VALUES (" + _MasterSaleId + "," + _DoneStatus + ",' " + _uName + "', '" + _crDt.ToString("s") + "')";
            _mysqldb.ExecuteQuery(sql);
            
            sql = "update tbl_spt_salesmaster set AMCStatus = " + _DoneStatus + " where Id = " + _MasterSaleId + "";
            _mysqldb.ExecuteQuery(sql);
        }

        public DataSet Get_AMCItemDtl(int _msId)
        {
            string sql = "select status,  CONVERT(VARCHAR(10), createddate, 103) AS  Date,  Username as User from  tbl_spt_amcactionlog inner join tbl_spt_amcstatus on tbl_spt_amcactionlog.DoneStatus =  tbl_spt_amcstatus.Id where mastersaleid = " + _msId + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        #endregion

 
        public bool Sales_Present(string SerialNo, string ItemId)
        {
            MysqlClass _mysqldb = new MysqlClass();
            bool valid = false;
            string sql = "SELECT COUNT(Id) FROM tbl_spt_salesmaster WHERE Status=1 AND ItemId='" + ItemId + "' AND SerialNo='" + SerialNo + "'";
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (int.Parse(m_MyReader.GetValue(0).ToString()) > 0)
                {
                    valid = true;
                }
            }
            if (!valid)
            {
                sql = "SELECT COUNT(tbl_spt_salesmaster.Id) FROM tbl_spt_oldserialno INNER JOIN tbl_spt_salesmaster ON tbl_spt_salesmaster.Id=tbl_spt_oldserialno.MasterSalesId WHERE tbl_spt_salesmaster.Status=1 AND tbl_spt_salesmaster.ItemId=" + ItemId + " AND tbl_spt_oldserialno.SerialNo='" + SerialNo + "'";
                m_MyReader = _mysqldb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    if (int.Parse(m_MyReader.GetValue(0).ToString()) > 0)
                    {
                        valid = true;
                    }
                }
            }
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return valid;
        }



        public bool HasOldSaleId(int _MasterSaleId, out int OldSaleId, out string OldSerialNo)
        {
            bool valid = false;
            OldSaleId = 0; OldSerialNo ="";
            string sql = "SELECT t1.OldSaleId,tbl_spt_sales.SerialNo FROM tbl_spt_salesmaster t1 INNER JOIN tbl_spt_sales ON tbl_spt_sales.MasterSaleId=t1.OldSaleId WHERE t1.OldSaleId!=0 AND t1.Id=" + _MasterSaleId;
            OdbcDataReader Reader = m_MysqlDb.ExecuteQuery(sql);
            if (Reader.HasRows)
            {
                int.TryParse(Reader.GetValue(0).ToString(), out OldSaleId);
                OldSerialNo = Reader.GetValue(1).ToString();
                if (OldSaleId > 0 && OldSerialNo!="")
                {
                    valid = true;
                }

            }
            Reader.Close();
             m_MysqlDb.CloseExistingConnection();
            return valid;
        }

        public void RemoveOtherSales(string OtherSaleId,MysqlClass _mysqldb)
        {
            //Arun added parameter on 21/10/2011
            string sql = "DELETE FROM tbl_spt_othersales WHERE Id=" + OtherSaleId;
            _mysqldb.ExecuteQuery(sql);
        }

        public void CreateCallLoggEntry(string CustomerName, string PhoneNumber, string Address, string LandMark, string ItemName, string ItemId, DateTime InvoiceDate, string Engineer, string Problem, DateTime ResponseDate, DateTime ResolutionDate, int IsReminder, string UserName, string ContactPerson, out int CallLoggId, MysqlClass _mysqldb, int UserId, int GroupId, int AssignedUserId)
        {

            CallLoggId = 0;
            string invoicedatestr = "Null";

            CallLoggId = GetTableMaxId("tbl_view_supportcalllogg", "Id", _mysqldb);
            if (InvoiceDate != new DateTime(100, 1, 1))
            {
                invoicedatestr = "'" + InvoiceDate.ToString("s") + "'";
            }
            string sql = "INSERT INTO tbl_spt_supportcalllogg(Id,Buyer,PhoneNo,Address,Landmark,Item,ItemId,InvoiceDate,AssignedEngineer,TargetResponceDate,TargetResolutionDate,ProblemComment,IsReminder,CreatedDate,CreatedUser,ContactPerson,UserId,GroupId,AssignedUserId) VALUES (" + CallLoggId + ",'" + CustomerName + "','" + PhoneNumber + "','" + Address + "','" + LandMark + "','" + ItemName + "'," + ItemId + "," + invoicedatestr + ",'" + Engineer + "','" + ResponseDate.ToString("s") + "','" + ResolutionDate.ToString("s") + "','" + Problem + "'," + IsReminder + ",'" + DateTime.Now.Date.ToString("s") + "','" + UserName + "','" + ContactPerson + "'," + UserId + "," + GroupId + "," + AssignedUserId + ")";
            _mysqldb.ExecuteQuery(sql);
            //CallLoggId = GetCallLoggEntryId(CustomerName, PhoneNumber, Address, Engineer, LandMark, IsReminder, ResponseDate, ResolutionDate, Problem, DateTime.Now.Date.ToString("s"), UserName, _mysqldb);
        }

        private int GetCallLoggEntryId(string CustomerName, string PhoneNumber, string Address, string Engineer, string LandMark, int IsReminder, DateTime ResponseDate, DateTime ResolutionDate, string Problem, string CreatedDate, string UserName, MysqlClass _mysqldb)
        {
            int Id = 0;
            string sql = "SELECT Id FROM tbl_view_supportcalllogg WHERE OpenStatus='OPEN' AND Buyer='" + CustomerName + "' AND PhoneNo='" + PhoneNumber + "' AND TargetResponceDate='" + ResponseDate.ToString("s") + "' AND TargetResolutionDate='" + ResolutionDate.ToString("s") + "' AND Address='" + Address + "' AND CreatedUser='" + UserName + "' AND CreatedDate='" + CreatedDate + "' AND Landmark='" + LandMark + "' AND AssignedEngineer='" + Engineer + "' AND ProblemComment='" + Problem + "' AND IsReminder=" + IsReminder;
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Id = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return Id;
        }

        public void CloseCallLogg(string CallId, string _comment, MysqlClass m_transaction)
        {
            // Arun added parameter 21/10/2011
            string sql = "UPDATE tbl_spt_supportcalllogg SET OpenStatus='CLOSED',ClosingComment='" + _comment + "' WHERE Id=" + CallId;
            m_transaction.ExecuteQuery(sql);


            //Move to history support actions
            sql = "INSERT INTO tbl_his_supportactions SELECT * FROM tbl_spt_supportactions WHERE tbl_spt_supportactions.CallId=" + CallId;
            m_transaction.ExecuteQuery(sql);


            sql = "DELETE FROM tbl_spt_supportactions WHERE CallId=" + CallId;
            m_transaction.ExecuteQuery(sql);



            //Move to history support requests
            sql = "INSERT INTO tbl_his_supportrequest SELECT * FROM tbl_spt_supportrequest WHERE tbl_spt_supportrequest.CallId=" + CallId;
            m_transaction.ExecuteQuery(sql);


            sql = "DELETE FROM tbl_spt_supportrequest WHERE CallId=" + CallId;
            m_transaction.ExecuteQuery(sql);


            //Move to history support Calls
            sql = "INSERT INTO tbl_his_supportcalllogg SELECT * FROM tbl_spt_supportcalllogg WHERE tbl_spt_supportcalllogg.Id=" + CallId;
            m_transaction.ExecuteQuery(sql);


            sql = "DELETE FROM tbl_spt_supportcalllogg WHERE Id=" + CallId;
            m_transaction.ExecuteQuery(sql);
        }


        public bool IsAll_InternalReqeustClosed(string CallId)
        {
            bool valid = true;
            MysqlClass mysqltemp = new MysqlClass();
            string sql = "SELECT COUNT(tbl_view_supportrequest.Id) FROM tbl_view_supportrequest WHERE tbl_view_supportrequest.RequestStatus!=5 AND tbl_view_supportrequest.CallId=" + CallId;
            m_MyReader = mysqltemp.ExecuteQuery(sql);

            if (m_MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
                if (_count > 0)
                {
                    valid = false;
                }

            }
            m_MyReader.Close();
            mysqltemp.CloseConnection();
            return valid;
        }

        public void CreateCallCost_WithoutInternalRequest(string CallId, int SupportRequestId, int ActionId, DateTime ActionDate, string Engineer, int Cost, string Comment, string CreateUser, string ActionSpecValue, int UserId, int GroupId)
        {
            string sql = "INSERT INTO tbl_spt_supportactions(CallId,SupportRequestId,ActionId,ActionDate,Engineer,Cost,Comment,CreateUser,CreatedDate,ActionSpecValue,UserId,GroupId) VALUES (" + CallId + "," + SupportRequestId + "," + ActionId + ",'" + ActionDate.ToString("s") + "','" + Engineer + "'," + Cost + ",'" + Comment + "','" + CreateUser + "','" + DateTime.Now.ToString("s") + "','" + ActionSpecValue + "'," + UserId + "," + GroupId + ")";
            m_TransationDb.ExecuteQuery(sql);
        }

        public void OpenCallLogg(string CallId, out string _msg)
        {
            _msg = "";
            try
            {
                m_MysqlDb.MyBeginTransaction();
                string sql = "UPDATE tbl_his_supportcalllogg SET OpenStatus='OPEN',ClosingComment='' WHERE Id=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);


                //Move to history support actions
                sql = "INSERT INTO tbl_spt_supportactions SELECT * FROM tbl_his_supportactions WHERE tbl_his_supportactions.CallId=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);


                sql = "DELETE FROM tbl_his_supportactions WHERE CallId=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);



                //Move to history support requests
                sql = "INSERT INTO tbl_spt_supportrequest SELECT * FROM tbl_his_supportrequest WHERE tbl_his_supportrequest.CallId=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);


                sql = "DELETE FROM tbl_his_supportrequest WHERE CallId=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);


                //Move to history support Calls
                sql = "INSERT INTO tbl_spt_supportcalllogg SELECT * FROM tbl_his_supportcalllogg WHERE tbl_his_supportcalllogg.Id=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);


                sql = "DELETE FROM tbl_his_supportcalllogg WHERE Id=" + CallId;
                m_MysqlDb.ExecuteQuery(sql);

                m_MysqlDb.TransactionCommit();
            }
            catch (Exception Ex)
            {
                _msg = "Error Message : " + Ex.Message;
                m_MysqlDb.TransactionRollback();
            }
             m_MysqlDb.CloseExistingConnection();
        }

        public string getEngineerId(string EngineerName)
        {
            MysqlClass _mysqldb = new MysqlClass();
            int Id = -1;
            string sql = "SELECT tbl_spt_engineer.Id FROM tbl_spt_engineer WHERE tbl_spt_engineer.Engineer='" + EngineerName + "'";
            m_MyReader = _mysqldb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(),out Id);
            }
            m_MyReader.Close();
            _mysqldb.CloseConnection();
            return Id.ToString();
        }

        public string GetLastComment_CallRequest(string ReqId)
        {
            MysqlClass mysqltemp = new MysqlClass();
            string LastComment = "";
            string sql = "SELECT Comment,CreatedDate FROM tbl_view_supportactions WHERE CallId=" + ReqId + " ORDER BY CreatedDate DESC ";
            OdbcDataReader NewReader = mysqltemp.ExecuteQuery(sql);
            if (NewReader.HasRows)
            {
                while (NewReader.Read())
                {
                    LastComment = NewReader.GetValue(0).ToString();
                    if (LastComment != "")
                    {
                        break;
                    }
                }
            }
            NewReader.Close();
            mysqltemp.CloseConnection();
            return LastComment;
        }

        public int Get_No_SalesLinkedWith_Call(string CallId)
        {
            int _count = 0;

            string sql = "SELECT COUNT(tbl_view_supportrequest.Id) FROm tbl_view_supportrequest WHERE tbl_view_supportrequest.CallId=" + CallId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            if (m_MyReader.HasRows)
            {
               
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);

            }
            m_MyReader.Close();
            m_MysqlDb.CloseExistingConnection();
            return _count;
        }

        public string GetCallClosedDate(string ReqId)
        {
            string Str="";
            DateTime ClosedDate;
            string sql = "SELECT CreatedDate FROM tbl_view_supportactions WHERE CallId=" + ReqId + " ORDER BY CreatedDate DESC ";
            OdbcDataReader NewReader = m_MysqlDb.ExecuteQuery(sql);
            if (NewReader.HasRows)
            {
                while (NewReader.Read())
                {
                   if (DateTime.TryParse(NewReader.GetValue(0).ToString(), out ClosedDate))
                    {
                        Str = DateConversion.GetFormatedDateVal(ClosedDate);
                        break;
                    }
                }
            }
            NewReader.Close();
            m_MysqlDb.CloseExistingConnection();
            return Str;
        }
        public int GetTableMaxId(string _TableName, string _Field, MysqlClass m_MysqlDb1)
        {          
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + "";

            OdbcDataReader m_MyReader5 = m_MysqlDb1.ExecuteQuery(sql);
            if (m_MyReader5.HasRows)
            {
                bool valid = int.TryParse(m_MyReader5.GetValue(0).ToString(), out Id);
            }
            Id = Id + 1;
            return Id;
        }

        public double Get_CallCost(string _CallId)
        {
            double _callcost = 0;
            MysqlClass _newsqldb = new MysqlClass();
            string sql = "SELECT SUM(tbl_view_supportactions.Cost) FROM tbl_view_supportactions INNER JOIN tbl_view_supportcalllogg ON tbl_view_supportcalllogg.Id=tbl_view_supportactions.CallId WHERE tbl_view_supportcalllogg.Id=" + _CallId;
            OdbcDataReader _myread = _newsqldb.ExecuteQuery(sql);
            if (_myread.HasRows)
            {
                double.TryParse(_myread.GetValue(0).ToString(), out _callcost);
            }
            _newsqldb.CloseConnection();
            return _callcost;
        }

        public bool IsLiveCall(int callId)
        {
            bool _valid = false;
            MysqlClass _newsqldb = new MysqlClass();
            string sql = "SELECT tbl_view_supportcalllogg.Live FROM tbl_view_supportcalllogg WHERE tbl_view_supportcalllogg.Id=" + callId;
            OdbcDataReader _myread = _newsqldb.ExecuteQuery(sql);
            if (_myread.HasRows)
            {
                int _live = 0;
                int.TryParse(_myread.GetValue(0).ToString(), out _live);
                if (_live == 1)
                {
                    _valid = true;
                }
            }
            _newsqldb.CloseConnection();
            return _valid;
        }

        public bool IsCallNoExist(string CallNo)
        {
            bool _valid = false;
            MysqlClass _newsqldb = new MysqlClass();
            string sql = "SELECT COUNT(tbl_view_supportcalllogg.Id) FROM tbl_view_supportcalllogg WHERE tbl_view_supportcalllogg.Id=" + CallNo;
            OdbcDataReader _myread = _newsqldb.ExecuteQuery(sql);
            if (_myread.HasRows)
            {
                int _count = 0;
                int.TryParse(_myread.GetValue(0).ToString(), out _count);
                if (_count >0)
                {
                    _valid = true;
                }
            }
            _newsqldb.CloseConnection();
            return _valid;
     
        }


        

    }
}
