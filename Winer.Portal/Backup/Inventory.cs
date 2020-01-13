using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Net;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace WinBase
{
    public class Inventory : KnowinGen
    {
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        OdbcDataReader Myreader = null;

        public Inventory(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
        }

        ~Inventory()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            } if (m_MyReader != null)
            {
                m_MyReader = null;

            }

        }

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

        public void Addlocation(string _locationname, out string msg)
        {
            string _sql = "";
            msg = "";
            _sql = "select Id from tblinv_locationmaster where LocationName='" + _locationname + "'";
            Myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Myreader.HasRows)
            {
                msg = "Location name already exist";
            }
            else
            {
                _sql = "Insert into tblinv_locationmaster(LocationName) values('" + _locationname + "')";
                m_MysqlDb.ExecuteQuery(_sql);
                msg = "";
            }

        }

        public DataSet GetLocationName()
        {
            DataSet locationds = new DataSet();
            string _sql = "";
            _sql = "Select Id,Locationname from tblinv_locationmaster";
            locationds = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return locationds;

        }

        public bool ExistInTransaction(int _locationId)
        {
            bool _exist = false;
            string _sql = "";
            _sql = "Select * from tblinv_transaction where StoreId=" + _locationId + "";
            Myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Myreader.HasRows)
            {
                _exist = true;
            }
            return _exist;
        }

        public bool ExistInItemStock(int _locationId)
        {
            bool _exist = false;
            string _sql = "";
            _sql = "Select Stock from tblinv_locationitemstock where LocationId=" + _locationId + "";
            Myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Myreader.HasRows)
            {
                while (Myreader.Read())
                {
                    if (int.Parse(Myreader.GetValue(0).ToString()) > 0)
                    {
                        _exist = true;
                        break;
                    }
                }

            }
            return _exist;
        }

        public void DeleteLocation(int _locationId)
        {
            string _sql = "";
            // _sql = "Update tblinv_locationmaster set Status=0 where Id="+_locationId+"";
            _sql = "Delete from tblinv_locationmaster where Id=" + _locationId + "";
            m_MysqlDb.ExecuteQuery(_sql);
            _sql = "Delete from tblinv_locationitemstock where LocationId=" + _locationId + "";
            m_MysqlDb.ExecuteQuery(_sql);
            _sql = "select tblclassschedule.Id from tblclassschedule where tblclassschedule.ClassRoom=" + _locationId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
            if (m_MyReader.HasRows)
            {
                _sql = "DELETE from tblclassschedule where tblclassschedule.ClassRoom=" + _locationId + "";
                m_MysqlDb.ExecuteQuery(_sql);
            }
        }

        public void PurchaseItem(int _ItemId, string _ItemName, int _Category, int _NewStock, DateTime _PurchaseDate, int storeId, string _Description, string _CreatedUser, int spec, string comment, double totalcost, int supplier, int Stkbal,double purchasingcost)
        {
            string _sql = "";
            string stock = "";
            if (spec == 1)
            {
                stock = "-stock";
            }
            else
            {
                stock = "+stock";
            }
            _sql = "insert into  tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,TotalCost,ActionType,ActionDate,ReferenceId,ReferenceType,StoreId,Description,Comment,CreatedUser,VendorId,Valuetype,StockBal,PurchasingCost)  values(" + _ItemId + ",'" + _ItemName + "'," + _Category + "," + _NewStock + "," + totalcost + ",'" + stock + "','" + _PurchaseDate.ToString("s") + "','','',1,'" + _Description + "','" + comment + "','" + _CreatedUser + "'," + supplier + ",1," + Stkbal + "," + purchasingcost + ")";
            m_MysqlDb.ExecuteQuery(_sql);



        }

  

        public DataSet GetActiveLocationNameForMoveItem()
        {
            DataSet locationds = new DataSet();
            string _sql = "";
            _sql = "Select Id,Locationname from tblinv_locationmaster";
            locationds = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return locationds;
        }

        public void AddLocationstock(int _ItemId, int _NewStock)
        {
            string _sql = "";
            _sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and  LocationId=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    int oldstock = int.Parse(m_MyReader.GetValue(0).ToString());
                    _NewStock = _NewStock + oldstock;
                }
                _sql = "Update tblinv_locationitemstock set Stock=" + _NewStock + " where ItemId=" + _ItemId + " and  LocationId=1";
                m_MysqlDb.ExecuteQuery(_sql);

            }
            else
            {
                _sql = "insert into tblinv_locationitemstock(ItemId,LocationId,Stock) values(" + _ItemId + ",1," + _NewStock + ")";
                m_MysqlDb.ExecuteQuery(_sql);
            }
            _sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and  LocationId=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
            if (m_MyReader.HasRows)
            {
                _sql = "update tblinv_item set TotalStock=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " where Id=" + _ItemId + "";
                m_MysqlDb.ExecuteQuery(_sql);
            }


        }

        public DataSet FillItemToGrid(int _locationId)
        {
            DataSet ItemDs = new DataSet();
            string _sql = "";
            _sql = "select itemId,Stock,ItemName,Category from tblinv_locationitemstock inner join tblinv_item on itemId=tblinv_item.Id where LocationId=" + _locationId + "";
            ItemDs = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return ItemDs;
        }

        public void MoveItemToLocation(int _ItemId, string _ItemName, int _Category, int _OutStock, DateTime _OutFlowDate, int _TolocationId, string _Description, string _CreatedUser, int _fromlocationId, int Availstock, int opnqty)
        {
            string _sql = "";
            string sql = "";
            _sql = "Insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,ActionType,ActionDate,StoreId,Description,CreatedUser,Valuetype,StockBal) values(" + _ItemId + ",'" + _ItemName + "'," + _Category + "," + _OutStock + ",'+stock','" + _OutFlowDate.ToString("s") + "'," + _TolocationId + ",'" + _Description + "','" + _CreatedUser + "',4," + opnqty + ")";
            m_MysqlDb.ExecuteQuery(_sql);
            _sql = "Insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,ActionType,ActionDate,StoreId,Description,CreatedUser,Valuetype,StockBal) values(" + _ItemId + ",'" + _ItemName + "'," + _Category + "," + _OutStock + ",'-stock','" + _OutFlowDate.ToString("s") + "'," + _fromlocationId + ",'" + _Description + "','" + _CreatedUser + "',4," + opnqty + ")";
            m_MysqlDb.ExecuteQuery(_sql);
            _sql = "select Id,Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and LocationId=" + _TolocationId + "";
            Myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Myreader.HasRows)
            {
                int stock = int.Parse(Myreader.GetValue(1).ToString());
                int newstock = _OutStock + stock;
                sql = "Update tblinv_locationitemstock set Stock=" + newstock + " where ItemId=" + _ItemId + " and LocationId=" + _TolocationId + "";
                m_MysqlDb.ExecuteQuery(sql);
                _sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and  LocationId=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    _sql = "update tblinv_item set TotalStock=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " where Id=" + _ItemId + "";
                    m_MysqlDb.ExecuteQuery(_sql);
                }

            }
            else
            {
                sql = "insert into tblinv_locationitemstock(ItemId,LocationId,Stock) values(" + _ItemId + "," + _TolocationId + "," + _OutStock + ")";
                m_MysqlDb.ExecuteQuery(sql);
            }
            _sql = "select Id,Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and LocationId=" + _fromlocationId + "";
            Myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Myreader.HasRows)
            {
                int newstock = int.Parse(Myreader.GetValue(1).ToString());
                int addnewstock = newstock - _OutStock;
                sql = "Update tblinv_locationitemstock set Stock=" + addnewstock + " where ItemId=" + _ItemId + " and LocationId=" + _fromlocationId + " ";
                m_MysqlDb.ExecuteQuery(sql);
                //if (addnewstock == 0)
                //{
                //    sql = "Delete from tblinv_locationitemstock where  ItemId=" + _ItemId + " and LocationId=" + _fromlocationId + "";
                //    m_MysqlDb.ExecuteQuery(sql);
                //}
            }
            if (_fromlocationId == 1)
            {
                _sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemId + " and  LocationId=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    _sql = "update tblinv_item set TotalStock=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " where Id=" + _ItemId + "";
                    m_MysqlDb.ExecuteQuery(_sql);
                }
            }

        }

        public DataSet LoadCategoryType()
        {
            DataSet CategoryDs = new DataSet();
            string sql = "";
            sql = "Select Id,CategoryType from tblinv_categorytype";
            CategoryDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return CategoryDs;
        }

        public DataSet LoadCosumableItemsToGrid(int _locationId)
        {
            DataSet ConsumableDs = new DataSet();
            string _sql = "";

            _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where tblinv_category.Categorytype=1 and tblinv_locationitemstock.LocationId=" + _locationId + " ";
            ConsumableDs = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return ConsumableDs;
        }

        public void IssueItemToStaff(int _ItemID, int _CategoryId, int count, int _availstock, int _locationId, DateTime _issuedate, string StaffName, string ItemName, string _description, string _createdUser, int stockbal, int staffid)
        {
            string sql = "";
            string _sql = "";
            sql = "select tblinv_item.ItemName from tblinv_item where tblinv_item.Id=" + _ItemID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _sql = "Insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,ActionType,ActionDate,StoreId,Description,CreatedUser,Comment,Valuetype,StockBal,VendorId,IssueUserTypeId) values(" + _ItemID + ",'" + ItemName + "'," + _CategoryId + "," + count + ",'Issue','" + _issuedate.ToString("s") + "'," + _locationId + ",'" + _description + "','" + _createdUser + "','Item " + m_MyReader.GetValue(0).ToString() + " is issued to " + StaffName + " on " + _issuedate.Date + "' ,3," + stockbal + "," + staffid + ",1)";
                m_MysqlDb.ExecuteQuery(_sql);
                //int newstock = _availstock - count;
                //_sql = "Update tblinv_locationitemstock set Stock=" + newstock + " where ItemId=" + _ItemID + " and LocationId=" + _locationId + "";
                //m_MysqlDb.ExecuteQuery(_sql);
                //_sql = "Update tblinv_locationitemstock set Stock=" + newstock + " where ItemId=" + _ItemID + " and LocationId=" + _locationId + "";
                //m_MysqlDb.ExecuteQuery(_sql);
                //_sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemID + " and  LocationId=1";
                //m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                //if (m_MyReader.HasRows)
                //{

                //    _sql = "update tblinv_item set TotalStock=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " where Id=" + _ItemID + "";
                //    m_MysqlDb.ExecuteQuery(_sql);
                //}
            }
        }

        public DataSet LoadUnCosumableItemsToGrid(int _locationId)
        {
            DataSet UnConsumableDs = new DataSet();
            string _sql = "";
            _sql = "select ItemName,tblinv_item.id,Stock,tblinv_item.Category from tblinv_item inner join tblinv_category on tblinv_category.Id= tblinv_item.Category inner join tblinv_locationitemstock on tblinv_item.Id= tblinv_locationitemstock.ItemId  where tblinv_category.Categorytype=2 and tblinv_locationitemstock.LocationId=" + _locationId + "";
            UnConsumableDs = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return UnConsumableDs;
        }

        public void SaleItemDetails(int _ItemID, int _CategoryId, int count, int _availstock, int _locationId, double totalamount, DateTime _saledate, string  saleto, string ItemName, string _description, string _CreatedUser, int stockbal)
        {
            string _sql = "";
            string sql = "";
            sql = "select tblinv_item.ItemName from tblinv_item where tblinv_item.Id=" + _ItemID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _sql = "Insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,TotalCost,ActionType,ActionDate,StoreId,Description,CreatedUser,Comment,Valuetype,StockBal) values(" + _ItemID + ",'" + ItemName + "'," + _CategoryId + "," + count + "," + totalamount + ",'Sale','" + _saledate.ToString("s") + "'," + _locationId + ",'" + _description + "','" + _CreatedUser + "','Item  " + m_MyReader.GetValue(0).ToString() + " is sold to customer " + saleto + "',2," + stockbal + ")";
                m_MysqlDb.ExecuteQuery(_sql);
                //int newstock = _availstock - count;
                //_sql = "Update tblinv_locationitemstock set Stock=" + newstock + " where ItemId=" + _ItemID + " and LocationId=" + _locationId + "";
                //m_MysqlDb.ExecuteQuery(_sql);
                //_sql = "Select Stock from tblinv_locationitemstock where ItemId=" + _ItemID + " and  LocationId=1";
                //m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                //if (m_MyReader.HasRows)
                //{
                //    _sql = "update tblinv_item set TotalStock=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " where Id=" + _ItemID + "";
                //    m_MysqlDb.ExecuteQuery(_sql);
                //}
            }
        }

        public string GoodReceiptHeader()
        {
            string m_PaySlipHeader = "";
            string ColumnCount = "10";
            string sql = "SELECT SchoolName,Address FROM tblschooldetails";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_PaySlipHeader += "<table width=\"100%\"><tr>";
                m_PaySlipHeader += "<td colspan=\"" + ColumnCount + "\" style=\"font-size:24px;text-align:center;height:40px;font-weight:bold\">" + m_MyReader.GetValue(0).ToString() + "";
                m_PaySlipHeader += "</td></tr>";
                m_PaySlipHeader += "<tr><td colspan=\"" + ColumnCount + "\" style=\"font-size:20px;text-align:center;height:35px;font-weight:bold\">" + m_MyReader.GetValue(1).ToString() + "";
                m_PaySlipHeader += "</td></tr>";
                m_PaySlipHeader += "</table>";
            }
            m_MyReader.Close();

            return m_PaySlipHeader;

        }

        public DataSet GetItemDetails(int ClassId, out string ClassName)
        {
            ClassName = "";
            OdbcDataReader classreader = null;
            DataSet Inv_ds = new DataSet();
            DataSet Inventory_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            Inventory_Ds.Tables.Add(new DataTable("Inventory"));
            dt = Inventory_Ds.Tables["Inventory"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Description");
            dt.Columns.Add("Category");
            dt.Columns.Add("Stock");
            string sql = "";
            string _sql = "";
            string _Csql = "";
            sql = "select tblclassschedule.ClassRoom from tblclassschedule where tblclassschedule.ClassId=" + ClassId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Csql = "select tblclass.ClassName from tblclass where tblclass.Id=" + ClassId + "";
                classreader = m_MysqlDb.ExecuteQuery(_Csql);
                if (classreader.HasRows)
                {
                    ClassName = classreader.GetValue(0).ToString();
                }
                _sql = "select tblinv_Item.Category, tblinv_Item.ItemName, tblinv_Item.Id, tblinv_Item.Description, tblinv_locationitemstock.Stock from tblinv_Item inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId= tblinv_Item.Id where tblinv_Item.Id in(select tblinv_locationitemstock.ItemId from tblinv_locationitemstock where tblinv_locationitemstock.LocationId=" + int.Parse(m_MyReader.GetValue(0).ToString()) + ") and tblinv_locationitemstock.LocationId=" + int.Parse(m_MyReader.GetValue(0).ToString()) + "";
                Inv_ds = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                if (Inv_ds != null && Inv_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Inv_ds.Tables[0].Rows)
                    {
                        _sql = "select Category from tblinv_category where Id=" + dr["Category"] + "";
                        m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                        if (m_MyReader.HasRows)
                        {
                            _dr = Inventory_Ds.Tables["Inventory"].NewRow();
                            _dr["Id"] = dr["Id"];
                            _dr["ItemName"] = dr["ItemName"];
                            _dr["Description"] = dr["Description"];
                            _dr["Category"] = m_MyReader.GetValue(0).ToString();
                            _dr["Stock"] = dr["Stock"];
                            Inventory_Ds.Tables["Inventory"].Rows.Add(_dr);
                        }

                    }
                }

            }
            return Inventory_Ds;
        }

        public DataSet GetAllItems(int category, int locationId)
        {
            DataSet Item_Ds = new DataSet();
            string sql = "";
            locationId = 0;
            if (category > 0)
            {
                //inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId= tblinv_item.Id 
                sql = "select distinct tblinv_item.Id, tblinv_item.ItemName from tblinv_item where tblinv_item.Category=" + category + "";
                if (locationId > 0)
                {
                    sql = sql + " and tblinv_locationitemstock.LocationId=" + locationId + "";
                }
            }
            else
            {
                //inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId= tblinv_item.Id 
                sql = "select distinct tblinv_item.Id, tblinv_item.ItemName from tblinv_item ";
                if (locationId > 0)
                {
                    sql = sql + " where tblinv_locationitemstock.LocationId=" + locationId + "";
                }
            }
            Item_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Item_Ds;
        }

        public DataSet GetAllCategory(int locationId, int type)
        {
            DataSet Category_Ds = new DataSet();
            string sql = "";
            locationId = 0;
            if (locationId > 0)
            {
                sql = "select distinct tblinv_category.Id, tblinv_category.Category from tblinv_category inner join tblinv_item on tblinv_item.Category = tblinv_category.Id inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId = tblinv_item.Id where tblinv_locationitemstock.LocationId=" + locationId + "";

            }
            else
            {
                //inner join tblinv_item on tblinv_item.Category = tblinv_category.Id inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId = tblinv_item.Id
                sql = "select distinct tblinv_category.Id, tblinv_category.Category from tblinv_category  ";
                if (type > 0)
                {
                    sql = sql + " where tblinv_category.Categorytype=" + type + "";
                }
            }

            Category_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Category_Ds;
        }

        public DataSet GetStockReport(int itemId, int Category, int locationId)
        {
            DataSet StockReport_Ds = new DataSet();
            string sql = "";
            int count = 0;
            sql = "select distinct  tblinv_locationmaster.Id, tblinv_item.Id,tblinv_locationmaster.LocationName, tblinv_item.ItemName, tblinv_locationitemstock.Stock as 'AvailableStock',tblinv_item.OpnQuantity as 'TotalStock',tblinv_category.Category,tblinv_item.Cost as 'Price' from tblinv_locationmaster inner join tblinv_locationitemstock on tblinv_locationitemstock.LocationId= tblinv_locationmaster.Id inner join  tblinv_item on tblinv_item.Id =tblinv_locationitemstock.ItemId inner join tblinv_category on tblinv_category.Id= tblinv_item.Category";
            if (Category > 0)
            {
                sql = sql + " where tblinv_item.Category=" + Category + "";
                count = 1;
            }
            if (itemId > 0)
            {
                if (count == 0)
                {
                    sql = sql + " where tblinv_item.Id=" + itemId + "";
                    count = 1;
                }
                else
                {
                    sql = sql + " and tblinv_item.Id=" + itemId + "";
                }
            }
            if (locationId > 0)
            {
                if (count == 0)
                {
                    sql = sql + " where tblinv_locationitemstock.LocationId=" + locationId + "";
                    count = 1;
                }
                else
                {
                    sql = sql + " and tblinv_locationitemstock.LocationId=" + locationId + "";
                }
            }
            sql = sql + " Order by  tblinv_category.Category";
            StockReport_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return StockReport_Ds;
        }

        public void UpdateOpenQuantity(int _itemId, out int opnqty)
        {
            string sql = "";
            opnqty = 0;
            OdbcDataReader openqtyreader = null;
            sql = "select tblinv_item.OpnQuantity from tblinv_item where tblinv_item.Id=" + _itemId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                sql = "select sum(Stock) from tblinv_locationitemstock where tblinv_locationitemstock.ItemId=" + _itemId + "";
                openqtyreader = m_MysqlDb.ExecuteQuery(sql);
                if (openqtyreader.HasRows)
                {
                    opnqty = int.Parse(openqtyreader.GetValue(0).ToString());
                    sql = "Update tblinv_item set OpnQuantity=" + opnqty + " where Id=" + _itemId + "";
                    m_MysqlDb.ExecuteQuery(sql);
                }

            }
        }

        public DataSet GetAllItems(int categoryId,string locationid)
        {
            DataSet Item_Ds = new DataSet();
            string sql = "";
            string temsql = "";           

            if (categoryId > 0)
            {
                if (locationid != "")
                {
                    temsql = " and tblinv_locationitemstock.LocationId=" + locationid + "";
                }
                sql = "select distinct tblinv_item.Id, tblinv_item.ItemName from tblinv_item inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId= tblinv_item.Id where tblinv_item.Category=" + categoryId + " " + temsql + "";
            }
            else
            {
                if (locationid != "")
                {
                    temsql = " where tblinv_locationitemstock.LocationId=" + locationid + "";
                }
                sql = "select distinct tblinv_item.Id, tblinv_item.ItemName from tblinv_item inner join tblinv_locationitemstock on tblinv_locationitemstock.ItemId= tblinv_item.Id " + temsql + "";
            }
            Item_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Item_Ds;
        }

        public DataSet GetAllClassDetails()
        {
            throw new NotImplementedException();
        }

        public DataSet DisplayItemDetailstoGrid(int categoryId, int classId, int batchid)
        {
            OdbcDataReader issuereader = null;
            OdbcDataReader Schedulereader = null;
            DataSet ItemDetails_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            ItemDetails_Ds.Tables.Add(new DataTable("Inventory"));
            dt = ItemDetails_Ds.Tables["Inventory"];
            dt.Columns.Add("Id");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Category");
            dt.Columns.Add("TotalStock");
            dt.Columns.Add("ScheduledCount");
            dt.Columns.Add("Scheduledstatus");
            string sql = "";
            sql = "select tblinv_item.Id,ItemName,tblinv_category.Category,TotalStock from tblinv_item inner join  tblinv_category on tblinv_category.Id= tblinv_item.Category  where tblinv_item.Category=" + categoryId + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _dr = ItemDetails_Ds.Tables["Inventory"].NewRow();
                    _dr["Id"] = m_MyReader.GetValue(0).ToString();
                    _dr["ItemName"] = m_MyReader.GetValue(1).ToString();
                    _dr["Category"] = m_MyReader.GetValue(2).ToString();
                    _dr["TotalStock"] = m_MyReader.GetValue(3).ToString();
                    sql = "select Id,count,status from tblinv_bookscheduledetails where tblinv_bookscheduledetails.ClassId=" + classId + " and BookId=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " and BatchId=" + batchid + "";
                    Schedulereader = m_MysqlDb.ExecuteQuery(sql);
                    if (Schedulereader.HasRows)
                    {
                        _dr["ScheduledCount"] = Schedulereader.GetValue(1).ToString();
                        int status = int.Parse(Schedulereader.GetValue(2).ToString());
                        if (status == 1)
                        {
                            _dr["Scheduledstatus"] = "Scheduled";
                        }
                        else if (status == 0)
                        {
                            _dr["Scheduledstatus"] = "Cancelled";
                        }
                    }
                    else
                    {
                        _dr["Scheduledstatus"] = "Pending";
                    }

                    ItemDetails_Ds.Tables["Inventory"].Rows.Add(_dr);
                }
            }
            return ItemDetails_Ds;
        }

        public int GetStudentCount(int ClassId,int batchId)
        {
            string sql = "";
            int StudCount = 0;
            //sql = "select COUNT(tblstudent.Id) from tblstudent where tblstudent.LastClassId=" + ClassId + "";
            sql = "SELECT count(map.StudentId) FROM tblstudentclassmap map inner join tblstudent stud on stud.id= map.Studentid where stud.status=1 and  map.ClassId=" + ClassId + " and map.RollNo<>-1 and map.BatchId=" + batchId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                StudCount = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return StudCount;
        }

        public void ScheduleBookToStudents(int classid, int itemId, int batchId, int count,int studentId)
        {
            string sql = "";
            int Id=0;
            string _sql = "";
            OdbcDataReader counrreader = null;
            sql = "select tblinv_bookscheduledetails.`Id` from tblinv_bookscheduledetails WHERE tblinv_bookscheduledetails.BatchId=" + batchId + " and tblinv_bookscheduledetails.BookId=" + itemId + " and tblinv_bookscheduledetails.ClassId=" + classid + ""; ;
            counrreader = m_MysqlDb.ExecuteQuery(sql);
            if (counrreader.HasRows)
            {
                sql = "update tblinv_bookscheduledetails set `Count`=" + count + " , tblinv_bookscheduledetails.`Status`=1  where tblinv_bookscheduledetails.ClassId=" + classid + " and BookId=" + itemId + " and BatchId=" + batchId + " and Id=" + int.Parse(counrreader.GetValue(0).ToString()) + "";
                m_MysqlDb.ExecuteQuery(sql);

            }
            else
            {
                sql = "insert into tblinv_bookscheduledetails(ClassId,BookId,BatchId,Count,status) values(" + classid + "," + itemId + "," + batchId + "," + count + ",1)";
                m_MysqlDb.ExecuteQuery(sql);
            }

            _sql = "select tblinv_bookscheduledetails.`Id` from tblinv_bookscheduledetails WHERE tblinv_bookscheduledetails.BatchId=" + batchId + " and tblinv_bookscheduledetails.BookId=" + itemId + " and tblinv_bookscheduledetails.ClassId=" + classid + "";
            counrreader = m_MysqlDb.ExecuteQuery(_sql);
            if (counrreader.HasRows)
            {
                if (NotAlreadySCheduled(studentId, int.Parse(counrreader.GetValue(0).ToString()), out Id))
                {
                    sql = "insert into tblinv_bookschedule(BookId,StudId,ScheduleDate,Count,Status,ScheduleId) values (" + itemId + "," + studentId + ",'" + System.DateTime.Now.ToString("s") + "'," + count + ",1," + int.Parse(counrreader.GetValue(0).ToString()) + ")";
                    m_MysqlDb.ExecuteQuery(sql);
                }


            }

        }

        private bool NotAlreadySCheduled(int studentId, int ScheduleID,out int Id)
        {
            string sql = "";
            Id = 0;
            bool exist = true;
            sql = "select Id from tblinv_bookschedule where StudId=" + studentId + " and ScheduleId=" + ScheduleID + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                exist = false;
                int.TryParse(m_MyReader.GetValue(0).ToString(),out Id);
            }
            return exist;
        }

        public void MapLocationToClass(int roomId, int classID, int batchId)
        {
            string sql = "";
            sql = "select tblclassschedule.Id from tblclassschedule where ClassRoom=" + roomId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                //sql = "insert into tblclassschedule(ClassId,BatchId,ClassRoom) values(" + classID + "," + batchId + "," + roomId + ");";
                sql = "Update tblclassschedule set ClassId=" + classID + " where ClassRoom=" + roomId + " and tblclassschedule.Id=" + int.Parse(m_MyReader.GetValue(0).ToString()) + "";
            }
            else
            {
                sql = "insert into tblclassschedule(ClassId,BatchId,ClassRoom) values(" + classID + "," + batchId + "," + roomId + ");";
            }
            m_MysqlDb.ExecuteQuery(sql);
        }

        public string GetMappingLocation(int ClassId)
        {
            string className = "";
            string sql = "select tblclass.ClassName from tblclass where tblclass.Id in(select tblclassschedule.ClassId from tblclassschedule where tblclassschedule.ClassRoom=" + ClassId + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                className = m_MyReader.GetValue(0).ToString();
            }
            return className;
        }

        public bool NotMappedToAnyotherLocation(int roomId, int classID)
        {
            string sql = "";
            bool valid = true;
            sql = "select tblclassschedule.ClassRoom from tblclassschedule where tblclassschedule.ClassId=" + classID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                valid = false;
            }
            return valid;
        }

        public string GetLocationNameById(int _locationId)
        {
            string locationname = "";
            string sql = "";
            sql = "select tblinv_locationmaster.LocationName from tblinv_locationmaster where tblinv_locationmaster.Id=" + _locationId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                locationname = m_MyReader.GetValue(0).ToString();
            }
            return locationname;
        }

        public string GetClassNameByID(int classID)
        {
            string classname = "";
            string sql = "";
            sql = "select tblclass.ClassName from tblclass where tblclass.Id=" + classID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                classname = m_MyReader.GetValue(0).ToString();
            }
            return classname;
        }

        public DataSet GetScheduleBookreport(int classId, DateTime startdate, DateTime Enddate, int categoryId, int itemid)
        {

            DataSet Report_Ds = new DataSet();
            string sql = "";
            if (classId > 0)
            {
                sql = "select tblinv_bookschedule.BookId, tblinv_bookschedule.StudId, tblinv_item.ItemName,tblview_student.StudentName,  DATE_FORMAT( tblinv_bookschedule.ScheduleDate, '%d/%m/%Y') as ScheduleDate, tblinv_bookschedule.Count from  tblinv_bookschedule inner join   tblinv_bookscheduledetails on tblinv_bookscheduledetails.Id= tblinv_bookschedule.ScheduleId inner join  tblview_student on tblview_student.Id= tblinv_bookschedule.StudId  inner join tblinv_item on tblinv_item.Id=  tblinv_bookschedule.BookId   where tblinv_bookschedule.ScheduleDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' and tblinv_bookscheduledetails.ClassId=" + classId + "";
            }
            else
            {
                sql = "select tblinv_bookschedule.BookId, tblinv_bookschedule.StudId, tblinv_item.ItemName,tblview_student.StudentName, DATE_FORMAT( tblinv_bookschedule.ScheduleDate, '%d/%m/%Y') as ScheduleDate, tblinv_bookschedule.Count from tblinv_bookschedule inner join   tblinv_bookscheduledetails on tblinv_bookscheduledetails.Id= tblinv_bookschedule.ScheduleId inner join  tblview_student on tblview_student.Id= tblinv_bookschedule.StudId  inner join tblinv_item on tblinv_item.Id=  tblinv_bookschedule.BookId   where tblinv_bookschedule.ScheduleDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            }
            if (categoryId > 0)
            {
                sql = sql + " and  tblinv_item.Category=" + categoryId + "";
            }
            if (itemid > 0)
            {

                sql = sql + " and tblinv_item.Id=" + itemid + "";
            }
            sql = sql + " and tblinv_bookschedule.Id not in (select tblinv_bookschedule.Id  from tblinv_bookschedule where tblinv_bookschedule.`Count` =0 and `status`=0)";
            Report_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            Report_Ds = GetClassName(Report_Ds);
            return Report_Ds;


            //where tblinv_bookschedule.ScheduleDate between '2011-08-30' and '2011-08-30' and tblinv_bookschedule.ClassId=50 and tblinv_bookschedule.BookId=2
        }

        private DataSet GetClassName(DataSet Report_Ds)
        {
            if (Report_Ds != null && Report_Ds.Tables[0].Rows.Count > 0)
            {
                string sql = "";
                OdbcDataReader Classreader=null;
                Report_Ds.Tables[0].Columns.Add("ClassId");
                Report_Ds.Tables[0].Columns.Add("ClassName");

                foreach (DataRow dr in Report_Ds.Tables[0].Rows)
                {
                    sql = " select tblclass.ClassName,tblstudent.LastClassId from tblclass inner join tblstudent on tblstudent.LastClassId= tblclass.Id where tblstudent.Id=" + int.Parse(dr["StudId"].ToString()) + "";
                    Classreader=m_MysqlDb.ExecuteQuery(sql);
                    if(Classreader.HasRows)
                    {
                        dr["ClassName"] = Classreader.GetValue(0).ToString();
                        dr["ClassId"] = Classreader.GetValue(1).ToString();

                    }                   
                }            }
            return Report_Ds;
        }

        public void SaveVendordetails(string vendorname, string City, string Address, string Email, string Mobilenum)
        {
            string sql = "";
            sql = "insert into tblinv_vendor(Name,City,Address,Email,MobileNumber) values('" + vendorname + "','" + City + "','" + Address + "','" + Email + "','" + Mobilenum + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetVendordetails()
        {
            string sql = "";
            DataSet Vendor_Ds = new DataSet();
            sql = "select tblinv_vendor.Id, tblinv_vendor.Name from tblinv_vendor";
            Vendor_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Vendor_Ds;
        }

        public DataSet GetPurchaseReport(int categoryId, int itemid, int vendorid, DateTime Fromdate, DateTime Todate, int valuetype)
        {
            DataSet salereport_ds = new DataSet();
            string sql = "";
            sql = "select tblinv_transaction.Id, tblinv_transaction.ItemId, tblinv_transaction.ItemName, tblinv_category.Category, tblinv_transaction.TotalCost,tblinv_transaction.Quantity,tblinv_vendor.Name,  DATE_FORMAT( tblinv_transaction.ActionDate, '%d/%m/%Y') as ActionDate,tblinv_transaction.`Comment` from tblinv_transaction inner join tblinv_vendor on tblinv_vendor.Id = tblinv_transaction.VendorId inner join tblinv_category on tblinv_category.Id=  tblinv_transaction.CategoryId where tblinv_transaction.Valuetype=" + valuetype + " and tblinv_transaction.ActionDate BETWEEN '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'";
            if (categoryId > 0)
            {
                sql = sql + " and tblinv_transaction.CategoryId=" + categoryId + " ";
            }
            if (itemid > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId=" + itemid + "";

            }
            if (vendorid > 0)
            {
                sql = sql + " and tblinv_transaction.VendorId=" + vendorid + "";
            }
            salereport_ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            // and tblinv_transaction.CategoryId=10 and 
            // tblinv_transaction.ItemId=2 and tblinv_transaction.VendorId=3
            return salereport_ds;
        }

        public DataSet GetSaleReport(int categoryId, int itemid, DateTime Fromdate, DateTime Todate, int valuetype)
        {
            DataSet salereport_ds = new DataSet();
            string sql = "";
            sql = "select tblinv_transaction.Id, tblinv_transaction.ItemId, tblinv_transaction.ItemName, tblinv_category.Category, tblinv_transaction.TotalCost,tblinv_transaction.Quantity,  DATE_FORMAT( tblinv_transaction.ActionDate, '%d/%m/%Y') as ActionDate,tblinv_transaction.`Comment` from tblinv_transaction inner join tblinv_category on tblinv_category.Id=  tblinv_transaction.CategoryId where tblinv_transaction.Valuetype=" + valuetype + " and tblinv_transaction.ActionDate BETWEEN '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'";
            if (categoryId > 0)
            {
                sql = sql + " and tblinv_transaction.CategoryId=" + categoryId + " ";
            }
            if (itemid > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId=" + itemid + "";

            }

            salereport_ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

         
            

            // and tblinv_transaction.CategoryId=10 and 
            // tblinv_transaction.ItemId=2 and tblinv_transaction.VendorId=3
            return salereport_ds;
        }

        public DataSet TotalAmount(int categoryId, int itemid, DateTime Fromdate, DateTime Todate, int valuetype)
        {
            DataSet Amount_ds = new DataSet();
            string sql = "";
//            sql = "select SUM(tblinv_transaction.TotalCost*tblinv_transaction.Quantity) as `Amount` from tblinv_transaction inner join tblinv_category on tblinv_category.Id=  tblinv_transaction.CategoryId where tblinv_transaction.Valuetype=" + valuetype + " and tblinv_transaction.ActionDate BETWEEN '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'";
            sql = "select SUM(tblinv_transaction.TotalCost) as `Amount` from tblinv_transaction inner join tblinv_category on tblinv_category.Id=  tblinv_transaction.CategoryId where tblinv_transaction.Valuetype=" + valuetype + " and tblinv_transaction.ActionDate BETWEEN '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'";
            if (categoryId > 0)
            {
                sql = sql + " and tblinv_transaction.CategoryId=" + categoryId + " ";
            }
            if (itemid > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId=" + itemid + "";

            }
            Amount_ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);



            return Amount_ds;
 
        }


        public void AdjustItemCount(int chkvalue, int ItemId, int count,int locationID)
        {
            string sql = "";
            int totalstock = 0;
            OdbcDataReader locationstckreader = null;
            sql = "select tblinv_item.TotalStock from tblinv_item where tblinv_item.Id=" + ItemId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (chkvalue == 0)
                {
                    if (locationID == 1)
                    {
                        totalstock = int.Parse(m_MyReader.GetValue(0).ToString());
                        int newstock = totalstock + count;
                        sql = "Update tblinv_item set TotalStock=" + newstock + " where tblinv_item.Id=" + ItemId + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    sql = "select tblinv_locationitemstock.Stock from tblinv_locationitemstock where tblinv_locationitemstock.ItemId=" + ItemId + " and tblinv_locationitemstock.LocationId="+locationID+"";
                    locationstckreader = m_MysqlDb.ExecuteQuery(sql);
                    if (locationstckreader.HasRows)
                    {
                        int newlocatnstock = int.Parse(locationstckreader.GetValue(0).ToString()) + count;
                        sql = "update tblinv_locationitemstock set tblinv_locationitemstock.Stock =" + newlocatnstock + " where tblinv_locationitemstock.ItemId =" + ItemId + " and tblinv_locationitemstock.LocationId=" + locationID + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        sql = "insert into tblinv_locationitemstock(ItemId,LocationId,Stock) values("+ItemId+","+locationID+","+count+")";
                        m_MysqlDb.ExecuteQuery(sql);
                    }

                }
                else
                {
                    if (locationID == 1)
                    {
                        totalstock = int.Parse(m_MyReader.GetValue(0).ToString());
                        int newstock = totalstock - count;
                        sql = "Update tblinv_item set TotalStock=" + newstock + " where tblinv_item.Id=" + ItemId + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    sql = "select tblinv_locationitemstock.Stock from tblinv_locationitemstock where tblinv_locationitemstock.ItemId=" + ItemId + " and tblinv_locationitemstock.LocationId="+locationID+"";
                    locationstckreader = m_MysqlDb.ExecuteQuery(sql);
                    if (locationstckreader.HasRows)
                    {
                        int newlocatnstock = int.Parse(locationstckreader.GetValue(0).ToString()) - count;
                        sql = "update tblinv_locationitemstock set tblinv_locationitemstock.Stock =" + newlocatnstock + " where tblinv_locationitemstock.ItemId =" + ItemId + " and tblinv_locationitemstock.LocationId="+locationID+"";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    else
                    {
                        sql = "insert into tblinv_locationitemstock(ItemId,LocationId,Stock) values(" + ItemId + "," + locationID + "," + count + ")";
                        m_MysqlDb.ExecuteQuery(sql);
                    }

                }
                // Update tblinv_item set TotalStock=TotalStock-1 where tblinv_item.Id=1

                //update tblinv_locationitemstock set tblinv_locationitemstock.Stock stock-1
                // where tblinv_locationitemstock.ItemId =1 and tblinv_locationitemstock.LocationId=2

            }
        }

        public void EntryToTransaction(int ItemId, string itemname, int count, string reason, string actiontype, string createuser, int valuetype, int stockbal,int StoreId)
        {
            string sql = "";
            sql = "select category from tblinv_item where tblinv_item.Id=" + ItemId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                string category = m_MyReader.GetValue(0).ToString();
                sql = "insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,ActionType,ActionDate,StoreId,Description,CreatedUser,Valuetype,StockBal) values(" + ItemId + ",'" + itemname + "'," + category + "," + count + ",'" + actiontype + "','" + System.DateTime.Now.ToString("s") + "',"+StoreId+",'" + reason + "','" + createuser + "'," + valuetype + "," + stockbal + ")";
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public DataSet GetTransationData(DateTime startdate, DateTime Enddate, int itemId, int storeid, int ValueType, int categpryId)
        {
            string sql = "";
            DataSet Transaction_Ds = new DataSet();
            sql = "select Id,ItemId,ItemName,Quantity,TotalCost,ActionType,Valuetype,CreatedUser,StockBal,DATE_FORMAT( tblinv_transaction.Actiondate, '%d/%m/%Y') as Actiondate from tblinv_transaction where tblinv_transaction.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            if (storeid > 0)
            {
                sql = sql + " and tblinv_transaction.StoreId=" + storeid + "";
            }
            if (itemId > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId=" + itemId + "";

            }
            if (ValueType > 0)
            {
                sql = sql + " and tblinv_transaction.Valuetype=" + ValueType + "";
            }
            if (categpryId > 0)
            {
                sql = sql + " and tblinv_transaction.CategoryId=" + categpryId + "";
            }
            Transaction_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            //and tblinv_transaction.CategoryId="++" and tblinv_transaction.ItemId="++"";



            return Transaction_Ds;
        }

        public DataSet GetScheduleBookDetails(int ClassId, int studentId, int batchId,int categoryid,int locationid)
        {
            DataSet ScheduleBookDetails_ds = null;
            string sql = "";
            string _sql = "";
            OdbcDataReader Categoryreader = null;
            string _tempsql = "";
            if (categoryid > 0)
            {
                _tempsql = " and tblinv_item.Category="+categoryid+"";
            }
            sql = "select tblinv_item.ItemName, tblinv_bookschedule.`Count`, tblinv_bookschedule.BookId,tblinv_bookschedule.Id,tblinv_item.Cost,tblinv_item.Category as CategoryId  from tblinv_bookschedule  inner join tblinv_item on tblinv_item.Id = tblinv_bookschedule.BookId inner join tblinv_bookscheduledetails ON tblinv_bookscheduledetails.Id= tblinv_bookschedule.ScheduleId   where tblinv_bookschedule.StudId=" + studentId + " and tblinv_item.Id in(select tblinv_locationitemstock.ItemId from tblinv_locationitemstock where tblinv_locationitemstock.LocationId=" + locationid + ") " + _tempsql + " order by tblinv_bookschedule.`Count` desc ";
            //and tblinv_bookscheduledetails.ClassId=" + ClassId + " and tblinv_bookscheduledetails.BatchId=" + batchId + " 
            ScheduleBookDetails_ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ScheduleBookDetails_ds != null && ScheduleBookDetails_ds.Tables[0].Rows.Count > 0)
            {
                ScheduleBookDetails_ds.Tables[0].Columns.Add("Category");
               

                foreach (DataRow dr in ScheduleBookDetails_ds.Tables[0].Rows)
                {
                   
                    _sql = "select tblinv_category.Category from tblinv_category  where tblinv_category.Id in (" + dr["CategoryId"].ToString() + ")";
                    Categoryreader = m_MysqlDb.ExecuteQuery(_sql);
                    if (Categoryreader.HasRows)
                    {
                        dr["Category"] = Categoryreader.GetValue(0).ToString();
                    }
                }
            }
            return ScheduleBookDetails_ds;
        }

        public void SaveIsuueBookDetails(int classId, int batchid, DateTime issuedate, int count, int bookId, int studid, int scheduleId, double Cost, out int issuecount,string createduser,int stockbal)
        {
            int schedulecount;
            string sql = "";
            issuecount = 0;
            OdbcDataReader countreader = null;
            string _sql = "";
            double issuecost = 0;
            if (scheduleId != 0)
            {
                _sql = "select tblinv_bookissue.Id from tblinv_bookissue where tblinv_bookissue.ScheduleId=" + scheduleId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    sql = "select tblinv_bookissue.`Count`,Cost from tblinv_bookissue where tblinv_bookissue.Id=" + int.Parse(m_MyReader.GetValue(0).ToString()) + "";
                    countreader = m_MysqlDb.ExecuteQuery(sql);
                    if (countreader.HasRows)
                    {
                        issuecount = int.Parse(countreader.GetValue(0).ToString());
                        int newcount = issuecount + count;
                        issuecost = double.Parse(countreader.GetValue(1).ToString());
                        double newcost = issuecost + Cost;
                        sql = "update tblinv_bookissue set IssueDate='" + issuedate.ToString("s") + "',`Count`=" + newcount + ",Cost=" + newcost + " where Id=" + int.Parse(m_MyReader.GetValue(0).ToString()) + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                }
                else
                {
                    sql = "insert into tblinv_bookissue(BookId,StudId,BatchId,ClassId,IssueDate,`Count`,ScheduleId,Cost,IsSpcBook) values(" + bookId + "," + studid + ","+batchid+","+classId+",'" + issuedate.ToString("s") + "'," + count + "," + scheduleId + "," + Cost + ",0)";
                    m_MysqlDb.ExecuteQuery(sql);
                }
                sql = "SELECT tblinv_bookschedule.`Count`,tblinv_bookschedule.Id from tblinv_bookschedule  where tblinv_bookschedule.StudId=" + studid + " AND tblinv_bookschedule.BookId=" + bookId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    schedulecount = int.Parse(m_MyReader.GetValue(0).ToString());
                    int newcount = schedulecount - count;
                    sql = "update tblinv_bookschedule set tblinv_bookschedule.`Count`=" + newcount + " where tblinv_bookschedule.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + "";
                    m_MysqlDb.ExecuteQuery(sql);
                    if (newcount == 0)
                    {
                        sql = "update tblinv_bookschedule set tblinv_bookschedule.Status=0 where tblinv_bookschedule.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                }
            }
            else
            {
                sql = "insert into tblinv_bookissue(BookId,StudId,BatchId,ClassId,IssueDate,`Count`,ScheduleId,Cost,IsSpcBook) values(" + bookId + "," + studid + "," + batchid + "," + classId + ",'" + issuedate.ToString("s") + "'," + count + "," + scheduleId + "," + Cost + ",1)";
                m_MysqlDb.ExecuteQuery(sql);
            }
            sql = "";
            OdbcDataReader Bookreader = null;
            OdbcDataReader studreader = null;
            string studname = "";
            string studsql = "";
            studsql = "select  tblstudent.StudentName from  tblstudent where tblstudent.Id=" + studid + "";
            studreader = m_MysqlDb.ExecuteQuery(studsql);
            if (studreader.HasRows)
            {
                studname = studreader.GetValue(0).ToString();

            }
            sql = "select  tblinv_item.ItemName, tblinv_item.Category from tblinv_item where tblinv_item.Id=" + bookId + " ";
            Bookreader = m_MysqlDb.ExecuteQuery(sql);
            if (Bookreader.HasRows)
            {


                _sql = "Insert into tblinv_transaction(ItemId,ItemName,CategoryId,Quantity,ActionType,ActionDate,StoreId,Description,CreatedUser,Comment,Valuetype,StockBal,VendorId,TotalCost,IssueUserTypeId) values(" + bookId + ",'" + Bookreader.GetValue(0).ToString() + "'," + int.Parse(Bookreader.GetValue(1).ToString()) + "," + count + ",'Issue','" + issuedate.ToString("s") + "',1,'Item " + Bookreader.GetValue(0).ToString() + " issued to " + studname + "','" + createduser + "','Item " + m_MyReader.GetValue(0).ToString() + " is issued to " + studname + " on " + issuedate.Date + "' ,3," + stockbal + "," + studid + "," + Cost + ",0)";
                m_MysqlDb.ExecuteQuery(_sql);


            }

           
        }

        public void UpdateStockItem(int BookId, int count,int locationid)
        {
            string sql = "";
            sql = "select tblinv_item.TotalStock from tblinv_item where tblinv_item.Id=" + BookId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int stock = int.Parse(m_MyReader.GetValue(0).ToString());
                int newstock = stock - count;
                sql = "update tblinv_item set TotalStock=" + newstock + " where tblinv_item.Id=" + BookId + "";
                m_MysqlDb.ExecuteQuery(sql);
                sql = "update tblinv_locationitemstock set Stock=" + newstock + " where ItemId=" + BookId + " and LocationId=" + locationid + "";
                m_MysqlDb.ExecuteQuery(sql);


            }
        }       

        public void CancelSchedule(int batchId, int bookid, int classid,int studentID)
        {
            string sql = "";
            int Id = 0;
            sql = "select tblinv_bookscheduledetails.`Id` from tblinv_bookscheduledetails WHERE tblinv_bookscheduledetails.BatchId=" + batchId + " and tblinv_bookscheduledetails.BookId=" + bookid + " and tblinv_bookscheduledetails.ClassId=" + classid + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (!NotAlreadySCheduled(studentID, int.Parse(m_MyReader.GetValue(0).ToString()),out Id))
                {
                    sql = "delete from tblinv_bookschedule where tblinv_bookschedule.Id=" + Id + " and StudId=" + studentID + "";
                    m_MysqlDb.ExecuteQuery(sql);
                }

                //sql = "update tblinv_bookscheduledetails set tblinv_bookscheduledetails.`Status`=0 where tblinv_bookscheduledetails.BatchId=" + batchId + " and tblinv_bookscheduledetails.BookId=" + bookid + " and tblinv_bookscheduledetails.ClassId=" + classid + "";
                //m_MysqlDb.ExecuteQuery(sql);
               
            }

        }

        public DataSet GetAllItemsInLocation(int LocationId)
        {
            string sql = "";
            DataSet Item_Ds = null;
            if (LocationId == 0)
            {
                sql = "select distinct tblinv_transaction.ItemId, tblinv_transaction.ItemName from tblinv_transaction";
            }
            else
            {
                sql = "select distinct tblinv_transaction.ItemId, tblinv_transaction.ItemName from tblinv_transaction where tblinv_transaction.ItemId in (select tblinv_locationitemstock.ItemId from  tblinv_locationitemstock where tblinv_locationitemstock.LocationId=" + LocationId + ")";
            }

            Item_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Item_Ds;
        }

        public DataSet GetAllVendordetails()
        {
            string sql = "";
            DataSet vendor_Ds = new DataSet();
            sql = "select Id,Name,City,Address,Email,MobileNumber from tblinv_vendor";
            vendor_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return vendor_Ds;
        }

        public void UpdateVendorDetails(int Id, string vendorname, string city, string address, string email, string mobile)
        {
            string sql = "";
            sql = "update tblinv_vendor set Name='" + vendorname + "',City='" + city + "',Address='" + address + "',Email='" + email + "',MobileNumber='" + mobile + "' where Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);

        }

        public void deleteVendor(int VendorId)
        {
            string sql = "";
            sql = "delete from tblinv_vendor where Id=" + VendorId + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetAllCategoryDetails(int type)
        {
            string sql = "";
            DataSet category_Ds = new DataSet();
            sql = "select tblinv_category.Id, tblinv_category.Category ,tblinv_category.Categorytype as `type`, tblinv_categorytype.CategoryType from tblinv_categorytype inner join tblinv_category on tblinv_category.Categorytype = tblinv_categorytype.Id";
            if (type > 0)
            {
                sql = sql + " where tblinv_category.Categorytype=" + type + "";
            }
            category_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return category_Ds;
        }

        public void SaveNewCategory(string categoryName, int categorytype)
        {
            string sql = "";
            sql = "INSERT into tblinv_category(Category,CategoryType) VALUES ('" + categoryName + "'," + categorytype + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpadteCategory(int id, string categoryName, int categorytype)
        {
            string sql = "";
            sql = "Update tblinv_category set Category='" + categoryName + "',Categorytype=" + categorytype + " where Id=" + id + "";
            m_MysqlDb.ExecuteQuery(sql);

        }

        public DataSet GetBookIssuereport(int classId, DateTime startdate, DateTime Enddate, int StudId)
        {
            DataSet IssueReport_Ds = new DataSet();
            string sql = "";
            if (classId > 0)
            {
                //select  tblinv_viewissuebill.StudId, tblinv_viewissuebill.ClassId,tblview_student.StudentName, tblclass.ClassName, DATE_FORMAT( tblinv_viewissuebill.`Date`, '%d/%m/%Y') as IssueDate from tblinv_viewissuebill inner join tblclass  on tblclass.Id= tblinv_viewissuebill.ClassId inner join  tblview_student on tblview_student.Id= tblinv_viewissuebill.StudId where tblinv_viewissuebill.`Date` between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'
                sql = "select  tblinv_viewissuebill.StudId,tblinv_viewissuebill.Id, tblinv_viewissuebill.ClassId,tblview_student.StudentName, tblclass.ClassName, DATE_FORMAT( tblinv_viewissuebill.`Date`, '%d/%m/%Y') as IssueDate from tblinv_viewissuebill inner join tblclass  on tblclass.Id= tblinv_viewissuebill.ClassId inner join  tblview_student on tblview_student.Id= tblinv_viewissuebill.StudId where tblinv_viewissuebill.`Date` between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' and tblinv_viewissuebill.ClassId=" + classId + "";
            }
            else
            {
                sql = "select  tblinv_viewissuebill.StudId,tblinv_viewissuebill.Id, tblinv_viewissuebill.ClassId,tblview_student.StudentName, tblclass.ClassName, DATE_FORMAT( tblinv_viewissuebill.`Date`, '%d/%m/%Y') as IssueDate from tblinv_viewissuebill inner join tblclass  on tblclass.Id= tblinv_viewissuebill.ClassId inner join  tblview_student on tblview_student.Id= tblinv_viewissuebill.StudId where tblinv_viewissuebill.`Date` between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            }
            if (StudId > 0)
            {
                sql = sql + "  and   tblinv_viewissuebill.StudId=" + StudId + "";
            }
            //if (categoryId > 0)
            //{
            //    sql = sql + " and  tblinv_item.Category=" + categoryId + "";
            //}
            //if (itemId > 0)
            //{

            //    sql = sql + " and tblinv_item.Id=" + itemId + "";
            //}
            IssueReport_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            //select tblinv_viewissuebill.Report from tblinv_viewissuebill where tblinv_viewissuebill.StudId=998
            return IssueReport_Ds;
        }

        public int GetAvailableCountOfItem(int itemID)
        {
            int itemCount = 0;
            string sql = "select tblinv_item.TotalStock from tblinv_item where tblinv_item.Id=" + itemID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out itemCount);
            }

            return itemCount;
        }

        public DataSet CreateVendorReport(int vendorId, DateTime StartDate, DateTime EndDate,int ItemID)
        {
            string sql = "";
            DataSet VendorDs = new DataSet();                                                                           //DATE_FORMAT( tblinv_transaction.Actiondate, '%d/%m/%Y') as Actiondate 
            sql = "select tblinv_transaction.Id, tblinv_transaction.ItemId, tblinv_transaction.ItemName, DATE_FORMAT( tblinv_transaction.Actiondate, '%d/%m/%Y') as Actiondate , tblinv_transaction.Quantity,tblinv_transaction.PurchasingCost, tblinv_transaction.TotalCost, tblinv_transaction.CreatedUser, tblinv_vendor.Name from tblinv_transaction inner join tblinv_vendor on tblinv_vendor.Id = tblinv_transaction.VendorId where tblinv_transaction.ActionDate between '" + StartDate.ToString("s") + "' and '" + EndDate.ToString("s") + "' and tblinv_transaction.Valuetype=1";

            if (ItemID > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId="+ItemID+"";
            }
            if (vendorId > 0)
            {
                sql = sql + " and  tblinv_transaction.VendorId="+vendorId+"";
            }
            VendorDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return VendorDs;
        }

        public void GetConfigValue(out int configvalue)
        {
            string sql = "";
            OdbcDataReader Valuereader = null;
            configvalue = 0;
            sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='AllowNegative Entry' and tblconfiguration.Module='Inventory'";
            Valuereader = m_MysqlDb.ExecuteQuery(sql);
            if (Valuereader.HasRows)
            {
                int.TryParse(Valuereader.GetValue(0).ToString(), out configvalue);
            }
        }

        public DataSet GetStudentIssueReport(DateTime Startdate, DateTime Enddate, int ClassId, int StudentID,int ItemId)
        {
            DataSet StudentDs = new DataSet();
            string sql = "";
            if (ClassId > 0)
            {
                sql = "select tblinv_bookissue.BookId, tblinv_bookissue.StudId, tblinv_bookissue.ClassId, tblinv_item.ItemName,tblview_student.StudentName, tblclass.ClassName,DATE_FORMAT( tblinv_bookissue.IssueDate, '%d/%m/%Y') as IssueDate, tblinv_bookissue.`Count`,tblinv_bookissue.Cost  from tblinv_bookissue   inner join tblclass on tblclass.Id= tblinv_bookissue.ClassId    inner join  tblview_student  on tblview_student.Id= tblinv_bookissue.StudId inner join tblinv_item on tblinv_item.Id= tblinv_bookissue.BookId   where tblinv_bookissue.IssueDate between '" + Startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' and tblinv_bookissue.ClassId=" + ClassId + "";
            }
            else
            {
                sql = "select tblinv_bookissue.BookId, tblinv_bookissue.StudId, tblinv_bookissue.ClassId, tblinv_item.ItemName,tblview_student.StudentName, tblclass.ClassName,DATE_FORMAT( tblinv_bookissue.IssueDate, '%d/%m/%Y') as IssueDate, tblinv_bookissue.`Count`,tblinv_bookissue.Cost  from tblinv_bookissue inner join tblclass on tblclass.Id= tblinv_bookissue.ClassId inner join  tblview_student   on tblview_student.Id= tblinv_bookissue.StudId inner join tblinv_item on tblinv_item.Id= tblinv_bookissue.BookId where tblinv_bookissue.IssueDate between '" + Startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            }
            if (StudentID > 0)
            {
                sql = sql + " and tblinv_bookissue.StudId="+StudentID+"";
            }
            if (ItemId > 0)
            {
                sql = sql + " and tblinv_bookissue.BookId=" + ItemId + ""; 
            }
            StudentDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return StudentDs;
        }

        public DataSet GetStaffIssueReport(int staffId, DateTime Startdate, DateTime Enddate,int itemId)
        {
            DataSet StaffDs = new DataSet();
            string sql = "";
            string _sql = "";                                                                                         //DATE_FORMAT( tblinv_transaction.Actiondate, '%d/%m/%Y') as Actiondate                                                                        
            sql = "select tblinv_transaction.ItemId, tblinv_transaction.ItemName, tblinv_transaction.Quantity, DATE_FORMAT( tblinv_transaction.Actiondate, '%d/%m/%Y') as Actiondate , tblinv_transaction.Id, tblinv_transaction.VendorId from tblinv_transaction  where  tblinv_transaction.ActionDate between '" + Startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' and tblinv_transaction.Valuetype=3 and IssueUserTypeId=1";
            if (staffId > 0)
            {
                sql = sql + " and  tblinv_transaction.VendorId="+staffId+"";
            }
            if (itemId > 0)
            {
                sql = sql + " and tblinv_transaction.ItemId="+itemId+"";
            }
            StaffDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (StaffDs != null && StaffDs.Tables[0].Rows.Count > 0)
            {
                StaffDs.Tables[0].Columns.Add("StaffName");
                foreach (DataRow dr in StaffDs.Tables[0].Rows)
                {
                    _sql = " select tblview_user.UserName from tblview_user where Id=" + int.Parse(dr["VendorId"].ToString()) + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                    if (m_MyReader.HasRows)
                    {
                        dr["StaffName"] = m_MyReader.GetValue(0).ToString();
                    }
                }
            }
            return StaffDs;
        }
    }
}
