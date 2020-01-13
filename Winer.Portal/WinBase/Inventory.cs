using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WinBase
{
    public class Inventory : KnowinGen
    {
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        private CLogging logger = null;
        public string table_type;


        public Inventory(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = _Prntobj.LoginUserName;
            m_userid = _Prntobj.User_Id;
            m_MysqlDb = new MysqlClass(this);
            logger = CLogging.GetLogObject();
        }
        public Inventory(MysqlClass _Msqlobj)
        {
            m_MysqlDb = _Msqlobj;

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

        public System.Data.DataSet GetSearchItemDetails(string ItemName, int locationId, string searchitem, int minstkitem)
        {
            string sql = "";
            DataSet ItemDetailsDs = new DataSet();
            string Tempsql = "";
            OdbcDataReader categoryIdReader = null;
            int _categoryid = 0;
            string _catsql = "";
            string minstksql = "";
            if (minstkitem == 1)
            {
                minstksql = " and tblLocationItemStock.AvailableStock< tblLocationItemStock.MinQty";

            }
            if (ItemName != "")
            {
                if (searchitem == "2")
                {
                    _catsql = "SELECT tblcategory.Id FROM tblcategory WHERE tblcategory.CategoryName='" + ItemName + "'";
                    categoryIdReader = m_MysqlDb.ExecuteQuery(_catsql);
                    if (categoryIdReader.HasRows)
                    {
                        int.TryParse(categoryIdReader.GetValue(0).ToString(), out _categoryid);
                    }
                    categoryIdReader.Close();
                    Tempsql = "  and  tblLocationItemStock.CategoryId ='" + _categoryid + "'";

                }
                else
                {
                    Tempsql = "  and  tblLocationItemStock.ItemName ='" + ItemName + "'";

                }
            }
            sql = "select DISTINCT tblLocationItemStock.AvailableStock, tblLocationItemStock.Id,tblLocationItemStock.ItemId, tblLocationItemStock.ItemName,tblLocationItemStock.LocationId,tblLocationItemStock.Description ,tblLocationItemStock.MinQty, tblLocationItemStock.Price1,tblLocationItemStock.Price2,tblLocationItemStock.Price3,tblLocationItemStock.Price4,tblLocationItemStock.Price5,tblLocationItemStock.TotalStock, tblcategory.CategoryName, tbltaxtype.Name as TaxName,tblgroup.GroupName,tbltaxtype.TaxPercent,tblLocationItemStock.CountableStatus,tblLocationItemStock.CreatedDate as CreatedDate,tblLocationItemStock.SUKID,Defetive,Onrepair  from tblLocationItemStock inner join tblcategory on tblcategory.Id=tblLocationItemStock.CategoryId inner join tblgroup on tblgroup.Id=tblLocationItemStock.LocationId inner join tbltaxtype on tbltaxtype.Id= tblLocationItemStock.TaxTypeId  where  tblLocationItemStock.LocationId =" + locationId + "  " + Tempsql + " " + minstksql + "";
            ItemDetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ItemDetailsDs;
        }

        public DataSet GetSearchFixedItemDetails(int ItemType, int locationId, string searchitem)
        {
            string sql = "", itemtype = "", itemname = "";


            DataSet ItemDetailsDs = new DataSet();
            if (ItemType != 0)
            {


                itemtype = "AND tblserialnumber.ItemId = " + ItemType + "";
            }

            if (searchitem != "")
            {

                itemname = "AND tblserialnumber.SerialNumber = '" + searchitem + "'";
            }

            if (locationId != 0)
            {

                //sql = "select DISTINCT tblserialnumber.Id, tblserialnumber.SerialNumber, tblitemtype.ItemType, tblgroup.GroupName as Location, tblserialnumber.Description as Description from tblserialnumber INNER JOIN tblitemtype on tblItemtype.Id = tblserialnumber.ItemId inner join tblgroup on tblgroup.Id = tblserialnumber.LocationId WHERE tblserialnumber.LocationId = " + locationId + " " + itemtype + " " + itemname + "";
                sql = "select DISTINCT tblserialnumber.Id, tblserialnumber.SerialNumber,tblserialnumber.ItemId, tblitemtype.ItemType,  tblgroup.GroupName as Location, tblserialnumber.Description as Description from tblserialnumber INNER JOIN tblitemtype on tblItemtype.Id = tblserialnumber.ItemId inner join tblgroup on tblgroup.Id = tblserialnumber.LocationId WHERE tblserialnumber.LocationId = " + locationId + " " + itemtype + " " + itemname + "";

            }
            else
            {
                sql = "select DISTINCT tblserialnumber.Id, tblserialnumber.SerialNumber,tblserialnumber.ItemId, tblitemtype.ItemType, tblgroup.GroupName as Location, tblserialnumber.Description as Description from tblserialnumber INNER JOIN tblitemtype on tblItemtype.Id = tblserialnumber.ItemId inner join tblgroup on tblgroup.Id = tblserialnumber.LocationId WHERE tblserialnumber.LocationId <> " + locationId + " " + itemtype + " " + itemname + "";
            }
            ItemDetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ItemDetailsDs;
        }

        public void AddNewCategory(string CategoryName, string Description, int categorytype, out int MaxId)
        {
            string sql = "";
            MaxId = 0;
            OdbcDataReader MaxIdreader = null;
            sql = "insert into tblcategory(CategoryName,Description,CategoryType) values('" + CategoryName + "','" + Description + "'," + categorytype + ")";
            m_MysqlDb.ExecuteQuery(sql);
            sql = "select MAX(Id) as Id from tblcategory";
            MaxIdreader = m_MysqlDb.ExecuteQuery(sql);
            if (MaxIdreader.HasRows)
            {
                MaxId = int.Parse(MaxIdreader.GetValue(0).ToString());
            }
            MaxIdreader.Close();
            m_MysqlDb.CloseExistingConnection();

        }
        public void AddNewUnit(string UnitName,  out int MaxId)
        {
            string sql = "";
            MaxId = 0;
            OdbcDataReader MaxIdreader = null;
            sql = "insert into tblunit(Unit) values('" + UnitName + "')";
            m_MysqlDb.ExecuteQuery(sql);
            sql = "select MAX(Id) as Id from tblunit";
            MaxIdreader = m_MysqlDb.ExecuteQuery(sql);
            if (MaxIdreader.HasRows)
            {
                MaxId = int.Parse(MaxIdreader.GetValue(0).ToString());
            }
            MaxIdreader.Close();
            m_MysqlDb.CloseExistingConnection();

        }

        public DataSet GetCategory()
        {
            string sql = "";
            DataSet CategoryDs = new DataSet();
            sql = "select tblcategory.CategoryName,tblcategory.Id from tblcategory order by CategoryName";
            CategoryDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return CategoryDs;
        }

        public DataSet GetTaxType()
        {
            string sql = "";
            DataSet TaxDs = new DataSet();
            sql = "select tbltaxtype.Id ,tbltaxtype.Name from tbltaxtype";
            TaxDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return TaxDs;
        }

        public DataSet GetPriceType()
        {
            string sql = "";
            DataSet PriceDs = new DataSet();
            sql = " select DISTINCT tblpricetype.Id ,tblpricetype.Name,tblpricetype.Colname from tblpricetype where tblpricetype.status=1 ORDER  BY tblpricetype.ColName ASC ";
            PriceDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return PriceDs;

        }

        public DataSet GetUnitType()
        {
            string sql = "";
            DataSet UnitDs = new DataSet();
            sql = " select tblunit.Id ,tblunit.Unit from tblunit";
            UnitDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return UnitDs;
        }

        public void UpdateItemInDataBase(DataSet PriceTypeDs, string _itemName, int _category, double _minStock, string _description, int _unitId, int _taxtypeId, int UpdateAlllocation, int p, int needserialnumber, int countablestatus, int itemid, DateTime Createddate, string SukId)
        {
            string sql = "";

            sql = "Update tblitem set   Description='" + _description + "', MinQty=" + _minStock + ",TaxTypeId=" + _taxtypeId + ", CatgryId=" + _category + ", UnitID=" + _unitId + ",NeedSerialNumber=" + needserialnumber + ",CountableStatus=" + countablestatus + ",Status=1,CreatedDate='" + Createddate + "',SUKID='" + SukId + "'";
            if (PriceTypeDs != null && PriceTypeDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in PriceTypeDs.Tables[0].Rows)
                {
                    if (dr["Price"].ToString() != "")
                    {
                        sql = sql + "," + dr["Colname"].ToString() + "=" + double.Parse(dr["Price"].ToString()) + "";
                    }
                }
                //double.Parse(dr["Price"].ToString())
            }
            sql = sql + " Where Id=" + itemid + "";
            m_TransationDb.ExecuteQuery(sql);
        }
        public void InsertItemToDataBase(DataSet PriceTypeDs, string _itemName, int _category, double _minStock, string _description, int _unitId, int _taxtypeId, int UpdateAlllocation, int currentlocationId, int needserialnumber, int CountableStatus, DateTime createdime, string sukid)
        {
            string sql = "";
            string _tempSql = "";
            sql = "insert into tblitem(ItemName,CatgryId,UnitID,TaxTypeId,CountableStatus,CreatedDate,SUKID";
            if (PriceTypeDs != null && PriceTypeDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in PriceTypeDs.Tables[0].Rows)
                {
                    if (dr["Price"].ToString() != "")
                    {
                        sql = sql + "," + dr["Colname"].ToString();
                        _tempSql = _tempSql + double.Parse(dr["Price"].ToString()) + ",";
                    }
                }
            }
            sql = sql + ",Description,MinQty,NeedSerialNumber) values('" + _itemName + "'," + _category + "," + _unitId + "," + _taxtypeId + "," + CountableStatus + ",'" + createdime.ToString("s") + "','" + sukid + "'," + _tempSql + " '" + _description + "'," + _minStock + "," + needserialnumber + ")";
            m_TransationDb.ExecuteQuery(sql);

        }
        public void InsertPaymenttoDatabase(string date, int reference, double amount, string vendorname)
        {
            string sql = "";
            string _tempsql = "";
            sql = "insert into tblpaymenttransaction(Date,Reference,VendorName,Amount) values('" + date + "'," + reference + ",'" + vendorname + "'," + amount + ")";
            m_TransationDb.ExecuteQuery(sql);
        }
        public void InsertVehiclePaymenttodb(string date, int reference, double amount, string itemtype, string itemname, int litre)
        {
            string sql = "";
            string _tempsql = "";
            sql = "insert into tblpaymenttransaction(Date,Reference,Itemtype,ItemName,Litre,Amount) values('" + date + "'," + reference + ",'" + itemtype + "','" + itemname + "'," + litre + "," + amount + ")";
            m_TransationDb.ExecuteQuery(sql);
        }
        public void InsertOtherPaymenttodb(string date, int reference, double amount, string itemtype, string itemname)
        {
            string sql = "";
            string _tempsql = "";
            sql = "insert into tblpaymenttransaction(Date,Reference,Itemtype,ItemName,Amount) values('" + date + "'," + reference + ",'" + itemtype + "','" + itemname + "'," + amount + ")";
            m_TransationDb.ExecuteQuery(sql);
        }


        public void InsertItemTolocationDataBase(DataSet PriceTypeDs, string _itemName, int _category, double _minStock, string _description, int _unitId, int _taxtypeId, int UpdateAlllocation, int currentlocationId, int needserialnumber, int CountableStatus, DateTime createdtime, string sukid)
        {
            string sql = "";
            string _sql1 = "";
            OdbcDataReader MaxIdReader = null;
            int MaxId = 0;


            _sql1 = "select Id from tblitem WHERE ItemName='" + _itemName + "' AND CatgryId=" + _category + " AND Description='" + _description + "'";
            MaxIdReader = m_TransationDb.ExecuteQuery(_sql1);
            if (MaxIdReader.HasRows)
            {
                int.TryParse(MaxIdReader.GetValue(0).ToString(), out MaxId);

            }
            if (UpdateAlllocation == 1)
            {

                string _sql = "";
                DataSet LocationDs = new DataSet();

                _sql = "select tblgroup.Id from tblgroup";
                LocationDs = m_TransationDb.ExecuteQueryReturnDataSet(_sql);
                if (LocationDs != null && LocationDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in LocationDs.Tables[0].Rows)
                    {

                        string _tempsql1 = "";
                        // insert into tblLocationItemStock(ItemId,ItemName,LocationId,MinQty,Description,TaxTypeId,CategoryId,
                        sql = "insert into tblLocationItemStock(ItemName,CategoryId,UnitID,TaxTypeId,createddate,SUKID";
                        if (PriceTypeDs != null && PriceTypeDs.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow _dr in PriceTypeDs.Tables[0].Rows)
                            {
                                if (_dr["Price"].ToString() != "")
                                {
                                    sql = sql + "," + _dr["Colname"].ToString();
                                    _tempsql1 = _tempsql1 + double.Parse(_dr["Price"].ToString()) + ",";
                                }
                            }
                        }
                        sql = sql + ",Description,MinQty,LocationId,ItemId,NeedSerialNumber,CountableStatus) values('" + _itemName + "'," + _category + "," + _unitId + "," + _taxtypeId + ",'" + createdtime.ToString("s") + "','" + sukid + "'," + _tempsql1 + " '" + _description + "'," + _minStock + "," + int.Parse(dr["Id"].ToString()) + "," + MaxId + "," + needserialnumber + "," + CountableStatus + ")";
                        m_TransationDb.ExecuteQuery(sql);
                    }
                }
            }
            else
            {

                //currentlocationId
                string _tempsql1 = "";
                sql = "insert into tblLocationItemStock(ItemName,CategoryId,UnitID,TaxTypeId,CreatedDate";
                if (PriceTypeDs != null && PriceTypeDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in PriceTypeDs.Tables[0].Rows)
                    {
                        if (_dr["Price"].ToString() != "")
                        {
                            sql = sql + "," + _dr["Colname"].ToString();
                            _tempsql1 = _tempsql1 + double.Parse(_dr["Price"].ToString()) + ",";
                        }
                    }
                }
                sql = sql + ",Description,MinQty,LocationId,ItemId,NeedSerialNumber,CountableStatus) values('" + _itemName + "'," + _category + "," + _unitId + "," + _taxtypeId + ",'" + createdtime.ToString("s") + "'," + _tempsql1 + " '" + _description + "'," + _minStock + "," + currentlocationId + "," + MaxId + "," + needserialnumber + "," + CountableStatus + ")";
                m_TransationDb.ExecuteQuery(sql);



            }
            MaxIdReader.Close();
        }

        public void DeleteItemFromLocation(int Id, int ItemId, int Deletealllocationvalue)
        {
            string sql = "";
            if (Deletealllocationvalue == 0)
            {
                sql = "Delete from tblLocationItemStock where Id=" + Id + "";
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                sql = "Delete from tblLocationItemStock where ItemId=" + ItemId + "";
                m_TransationDb.ExecuteQuery(sql);
            }

        }
      

        public string GetAllLocations(int locationid)
        {
            string Location = "";
            OdbcDataReader locationreader = null;
            string sql = "";
            sql = "SELECT GroupName FROM tblgroup where  Id=" + locationid + "";
            locationreader = m_MysqlDb.ExecuteQuery(sql);
            if (locationreader.HasRows)
            {
                Location = locationreader.GetValue(0).ToString();

            }
            locationreader.Close();
            return Location;
        }

        public DataSet GetNonExistingItems(int currentlocationId, int categoryId, string _SearchData)
        {
            string sql = "";
            string tempsql = "";
            DataSet ItemDs = new DataSet();
            if (_SearchData != "")
            {
                tempsql = "tblitem.ItemName='" + _SearchData + "' and";
            }
            else if (categoryId > 0)
            {
                tempsql = "tblitem.CatgryId=" + categoryId + " and";
            }
            sql = "select tblitem.AvailableStock, tblitem.Id,tblitem.ItemName,tblitem.Description ,tblitem.MinQty, tblitem.Price1,tblitem.Price2,tblitem.Price3,tblitem.Price4,tblitem.Price5,tblitem.TotalStock, tblcategory.CategoryName, tbltaxtype.Name as TaxName,tbltaxtype.TaxPercent,tblitem.CreatedDate as CreatedDate,SUKID from tblitem inner join tblcategory on tblcategory.Id=tblitem.CatgryId  inner join tbltaxtype  on tbltaxtype.Id= tblitem.TaxTypeId where tblitem.Status=1 and " + tempsql + "  tblitem.Id not  in(select tblLocationItemStock.ItemId  from tblLocationItemStock inner join tblitem on tbllocationitemstock.ItemId=tblitem.Id where tblLocationItemStock.LocationId=" + currentlocationId + ")  ORDER BY tblitem.ItemName";
            ItemDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ItemDs;
        }

        public void AddExistingItemToLocation(int locationId, int itemId)
        {

            string _sql = "";
            //DataSet ExistingItemDs = new DataSet();
            //ExistingItemDs = GetExistingItemDetails(itemId);
            //if (ExistingItemDs != null && ExistingItemDs.Tables[0].Rows.Count > 0)
            //{
            _sql = "insert into tblLocationItemStock(ItemId,ItemName,CategoryId,UnitID,TaxTypeId,Price1,Price2,Price3,Price4,Price5,BarcodeID,SUKID,MinQty,MinOrderQty,BasicLeadTime,Description,NeedSerialNumber,MaxQty,LocationId,CreatedDate)(select  tblitem.Id,tblitem.ItemName,tblitem.CatgryId,tblitem.UnitID,tblitem.TaxTypeId,tblitem.Price1,tblitem.Price2,tblitem.Price3,tblitem.Price4,tblitem.Price5,tblitem.BarcodeID,tblitem.SUKID,tblitem.MinQty,tblitem.MinOrderQty,tblitem.BasicLeadTime,tblitem.Description,NeedSerialNumber, tblitem.MaxQty," + locationId + ",CreatedDate from tblitem where Id =" + itemId + ")";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(_sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(_sql);
            }

            // _sql = "insert into tblLocationItemStock(ItemId,ItemName,LocationId,CategoryId,UnitID,TaxTypeId,Price1,Price2,Price3,Price4,Price5,BarcodeID,SUKID,MinQty,MinOrderQty,BasicLeadTime,Description) values (" + int.Parse(ExistingItemDs.Tables[0].Rows[0]["Id"].ToString()) + ",'" + ExistingItemDs.Tables[0].Rows[0]["ItemName"].ToString() + "'," + locationId + "," + int.Parse(ExistingItemDs.Tables[0].Rows[0]["CatgryId"].ToString()) + "," + int.Parse(ExistingItemDs.Tables[0].Rows[0]["UnitID"].ToString()) + "," + int.Parse(ExistingItemDs.Tables[0].Rows[0]["TaxTypeId"].ToString()) + ", " + double.Parse(ExistingItemDs.Tables[0].Rows[0]["Price1"].ToString()) + "," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["Price2"].ToString()) + "," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["Price3"].ToString()) + "," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["Price4"].ToString()) + "," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["Price5"].ToString()) + ",'" + ExistingItemDs.Tables[0].Rows[0]["BarcodeID"].ToString() + "','" + ExistingItemDs.Tables[0].Rows[0]["SUKID"].ToString() + "'," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["MinQty"].ToString()) + "," + double.Parse(ExistingItemDs.Tables[0].Rows[0]["MinOrderQty"].ToString()) + "," + int.Parse(ExistingItemDs.Tables[0].Rows[0]["BasicLeadTime"].ToString()) + ",'" + ExistingItemDs.Tables[0].Rows[0]["Description"].ToString() + "')";
            // }

        }

        private DataSet GetExistingItemDetails(int itemId)
        {
            string sql = "";
            DataSet ExistingItemDs = new DataSet();
            sql = "select  tblitem.Id,tblitem.ItemName,tblitem.CatgryId,tblitem.BarcodeID,tblitem.SUKID,tblitem.TaxTypeId,tblitem.ItemName,tblitem.MinQty,tblitem.Description, tblitem.Price1,tblitem.Price2,tblitem.Price3,tblitem.Price4,tblitem.Price5,tblitem.MaxQty,tblitem.MinOrderQty,tblitem.UnitID,tblitem.BasicLeadTime,NeedSerialNumber,CountableStatus from tblitem where Id =" + itemId + "";
            if (m_TransationDb != null)
            {
                ExistingItemDs = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                ExistingItemDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            }
            return ExistingItemDs;
        }

        public DataSet GetEditItemDetails(int Id)
        {
            string sql = "";
            DataSet EditItemDetailsDs = new DataSet();
            sql = "select tblLocationItemStock.ItemName,tblLocationItemStock.Description ,tblLocationItemStock.MinQty, tblLocationItemStock.Price1,tblLocationItemStock.Price2,tblLocationItemStock.Price3,tblLocationItemStock.Price4,tblLocationItemStock.Price5,tblLocationItemStock.CategoryId,tblLocationItemStock.TaxTypeId,tblLocationItemStock.UnitID,NeedSerialNumber,ItemId,tblLocationItemStock.CreatedDate as CreatedDate,SUKID,tblLocationItemStock.CountableStatus from tblLocationItemStock where Id=" + Id + "";
            EditItemDetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return EditItemDetailsDs;
        }

        public DataSet GetManageItemDetails(int Id)
        {
            string sql = "";
            DataSet EditItemDetailsDs = new DataSet();
            sql = "select tblitem.ItemName,tblitem.Description ,tblitem.MinQty, tblitem.Price1,tblitem.Price2,tblitem.Price3,tblitem.Price4,tblitem.Price5,tblitem.CatgryId,tblitem.TaxTypeId,tblitem.UnitID,NeedSerialNumber,tblitem.CreatedDate as CreatedDate,SUKID,tblitem.CountableStatus from tblitem where Id=" + Id + "";
            EditItemDetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return EditItemDetailsDs;
        }


        public DataSet GetEditBasicItemDetails(int Id)
        {
            string sql = "";
            DataSet EditBasicItemDetailsDs = new DataSet();
            sql = "select tblitem.ItemName,tblitem.Description ,tblitem.MinQty, tblitem.Price1,tblitem.Price2,tblitem.Price3,tblitem.Price4,tblitem.Price5,tblitem.CatgryId,tblitem.TaxTypeId,tblitem.UnitID,CONVERT(VARCHAR(10),tblitem.CreatedDate,103) as CreatedDate,SUKID  from tblitem where Id=" + Id + "";
            EditBasicItemDetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return EditBasicItemDetailsDs;
        }

        public void UpdateItemFromserver(int Id, int ItemId)
        {
            DataSet ItemDs = new DataSet();
            string sql = "";
            //double Price1 = 0;
            //double Price2 = 0;
            //double Price3 = 0;
            //double Price4 = 0;
            //double Price5 = 0;
            //double MinQty = 0;
            //int TaxTypeId = 0;
            //int CategoryId = 0;
            //int UnitID = 0;
            //double AvgPurchaseCost = 0;
            //double MinOrderQty = 0;
            //int BasicLeadTime = 0;

            // double.TryParse(,out Price1);

            ItemDs = GetExistingItemDetails(ItemId);
            if (ItemDs != null && ItemDs.Tables[0].Rows.Count > 0)
            {
                sql = "Update tblLocationItemStock set ItemName='" + ItemDs.Tables[0].Rows[0]["ItemName"].ToString() + "',Description='" + ItemDs.Tables[0].Rows[0]["Description"].ToString() + "', MinQty=" + double.Parse(ItemDs.Tables[0].Rows[0]["MinQty"].ToString()) + ",TaxTypeId=" + int.Parse(ItemDs.Tables[0].Rows[0]["TaxTypeId"].ToString()) + ",CategoryId=" + int.Parse(ItemDs.Tables[0].Rows[0]["CatgryId"].ToString()) + ", UnitID=" + int.Parse(ItemDs.Tables[0].Rows[0]["UnitID"].ToString()) + ",Price1=" + double.Parse(ItemDs.Tables[0].Rows[0]["Price1"].ToString()) + ",Price2=" + double.Parse(ItemDs.Tables[0].Rows[0]["Price2"].ToString()) + ",Price3=" + double.Parse(ItemDs.Tables[0].Rows[0]["Price3"].ToString()) + ",Price4=" + double.Parse(ItemDs.Tables[0].Rows[0]["Price4"].ToString()) + ",Price5=" + double.Parse(ItemDs.Tables[0].Rows[0]["Price5"].ToString()) + ",SUKID='" + ItemDs.Tables[0].Rows[0]["SUKID"].ToString() + "',BarcodeID='" + ItemDs.Tables[0].Rows[0]["BarcodeID"].ToString() + "',MinOrderQty=" + double.Parse(ItemDs.Tables[0].Rows[0]["MinOrderQty"].ToString()) + ",BasicLeadTime=" + int.Parse(ItemDs.Tables[0].Rows[0]["BasicLeadTime"].ToString()) + ",NeedSerialNumber=" + int.Parse(ItemDs.Tables[0].Rows[0]["NeedSerialNumber"].ToString()) + ",CountableStatus=" + int.Parse(ItemDs.Tables[0].Rows[0]["CountableStatus"].ToString()) + "";

                sql = sql + " Where Id=" + Id + "";
                m_MysqlDb.ExecuteQuery(sql);
               // log("Item details updated", "Item " + ItemDs.Tables[0].Rows[0]["ItemName"].ToString() + " has updated  successfully in the server", MyUser.LOCATIONID);
                
            }
            m_MysqlDb.CloseExistingConnection();
        }



        public DataSet GetItemReport(int locationId, int CategoryId, int itemid)
        {
            string _sql = "";
            string _tempsql = "";
            DataSet ItemReportDs = new DataSet();
            if (locationId > 0)
            {
                _tempsql = " and tblLocationItemStock.LocationId =" + locationId + "";

            }
            if (CategoryId > 0)
            {

                _tempsql = _tempsql + " and tblLocationItemStock.CategoryId  =" + CategoryId + "";

            }
            if (itemid > 0)
            {

                _tempsql = _tempsql + " and tblLocationItemStock.ItemId  =" + itemid + "";

            }
            _sql = "select tblLocationItemStock.AvailableStock, tblLocationItemStock.Id,tblLocationItemStock.ItemId, tblLocationItemStock.ItemName,tblLocationItemStock.LocationId,tblLocationItemStock.Description ,tblLocationItemStock.MinQty, tblLocationItemStock.Price1,tblLocationItemStock.Price2,tblLocationItemStock.Price3,tblLocationItemStock.Price4,tblLocationItemStock.Price5,tblLocationItemStock.TotalStock, tblcategory.CategoryName, tbltaxtype.Name as TaxName,tblgroup.GroupName,tbltaxtype.TaxPercent from  tblLocationItemStock inner join tblcategory on tblcategory.Id=tblLocationItemStock.CategoryId  inner join tblgroup on tblgroup.Id=tblLocationItemStock.LocationId inner join tbltaxtype on  tbltaxtype.Id= tblLocationItemStock.TaxTypeId  where tblLocationItemStock.Status=1 " + _tempsql + " ";
            ItemReportDs = m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
            return ItemReportDs;
        }

        public DataSet GetAllItems(int CategoryId, int locationId)
        {
            string sql = "";
            DataSet ItemDS = new DataSet();
            string tempsql = "";
            if (locationId > 0)
            {
                tempsql = " and LocationId=" + locationId + "";
            }
            if (CategoryId > 0)
            {
                tempsql = tempsql + " and CatgryId=" + CategoryId + "";
            }

            sql = "SELECT Id as ItemId,ItemName FROM tblItem where status=1 " + tempsql + " ";
            ItemDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ItemDS;
        }

        public DataSet GetPurchaseReport(int categoryId, int locationid, int itemid, DateTime startdate, DateTime Enddate, int vendorid)
        {
            DataSet PurchaseReportDs = new DataSet();
            string sql = "";
            string tempsql = "";
            if (locationid > 0)
            {
                tempsql = " and tblpurchase.LocationId=" + locationid + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and tblpurchaseitems.ItemId=" + itemid + "";
            }
            if (categoryId > 0)
            {
                tempsql = tempsql + " and tblLocationItemStock.CategoryId=" + categoryId + "";
            }
            if (vendorid > 0)
            {
                tempsql = tempsql + " and tblpurchase.VendorId=" + vendorid + "";
            }
            //DATE_FORMAT( tblpurchase.`PurchaseDt`, '%d/%m/%Y') as PurchaseDt 

            sql = "select distinct tblpurchase.Id, tblpurchase.PONO,tblpurchase.VendorId,tblpurchase.LocationId, tblpurchase.PurchaseDt as PurchaseDt ,tblLocationItemStock.ItemName,tblpurchaseitems.GrossAmt,tblpurchaseitems.ItemId,tblpurchaseitems.SerialNumber,tblvendor.VendorName,tblpurchaseitems.ReceivedQty,tblgroup.GroupName from tblpurchaseitems inner join tblpurchase on tblpurchaseitems.PurchaseId=tblpurchase.Id inner join tblLocationItemStock on tblLocationItemStock.ItemId=tblpurchaseitems.ItemId  inner join tblvendor on tblvendor.Id=tblpurchase.VendorId inner join tblgroup on tblgroup.Id=tblpurchase.LocationId where tblpurchase.PurchaseDt between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' ";
            sql = sql + tempsql + " order by tblpurchase.PONO";
            PurchaseReportDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return PurchaseReportDs;
        }

        public DataSet GetSaleReport(int categoryId, int locationid, int itemid, DateTime startdate, DateTime Enddate)
        {
            string sql = "", tempsql = "";
            DataSet SaleReportDS = new DataSet();
            if (locationid > 0)
            {
                tempsql = " and tblbill.LocationId=" + locationid + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and tblbillitems.ItemId=" + itemid + "";
            }
            if (categoryId > 0)
            {
                tempsql = tempsql + " and tblLocationItemStock.CategoryId=" + categoryId + "";
            }
            //  CONVERT(VARCHAR(10),tblbill.BillDate,101) as BillDate,
            // tblbill.BillDate,
            sql = "select distinct tblbillitems.Id,tblbillitems.ItemName,tblbill.BillNo,tblbillitems.GrossAmount,tblbill.CustomerName,tblbillitems.Discount,tblbillitems.Quantity,tblbillitems.TaxAmt,tblbillitems.SerialNumber,tblbill.LocationId,tblgroup.GroupName,tblbillitems.ItemId,tblbill.BillDate as BillDate from tblbillitems inner join tblbill on tblbill.Id=tblbillitems.BillId inner join tblgroup on tblbill.LocationId=tblgroup.Id inner join tblLocationItemStock on tblLocationItemStock.ItemId=tblbillitems.ItemId    where tblbill.BillDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' ";
            sql = sql + tempsql + " order by BillDate";
            SaleReportDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return SaleReportDS;

        }

        public DataSet GetInwardOutwardReport(int categoryId, int locationid, int itemid, DateTime startdate, DateTime Enddate, int TypeId)
        {
            string sql = "", tempsql = "";
            DataSet ReportDS = new DataSet();
            if (locationid > 0)
            {
                tempsql = " and Viewtransaction.LocationId=" + locationid + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and Viewtransaction.ItemId=" + itemid + "";
            }
            if (categoryId > 0)
            {
                tempsql = tempsql + " and Viewtransaction.CategoryId=" + categoryId + "";
            }
            if (TypeId > 0)
            {
                tempsql = tempsql + " and Viewtransaction.Valuetype=" + TypeId + "";
            }
            sql = "select distinct Viewtransaction.Id,Viewtransaction.ItemName,Viewtransaction.ItemId,Viewtransaction.CustomerName,Viewtransaction.Quantity,Viewtransaction.SerialNumber,Viewtransaction.Comment,Viewtransaction.LocationId,tblgroup.GroupName,Viewtransaction.ActionDate as ActionDate,Viewtransaction.BillNo,Viewtransaction.Valuetype from Viewtransaction  inner join tblgroup on tblgroup.Id=Viewtransaction.LocationId where Viewtransaction.Valuetype in(3,4) and Viewtransaction.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' ";
            sql = sql + tempsql + " order by Viewtransaction.BillNo"; ;
            ReportDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ReportDS;
        }
        #region  COMMON
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
        #endregion COMMON

        public int GetBillItemCopunt(string Type)
        {
            string sql = "Select tblconfiguration.Value from tblconfiguration where Name='Count' and Module='" + Type + "'";
            DataSet CountDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (CountDS != null && CountDS.Tables != null && CountDS.Tables[0].Rows.Count > 0)
            {
                return int.Parse(CountDS.Tables[0].Rows[0][0].ToString());
            }
            else
                return 0;
        }

        public string SaveAdjustments(string[][] itemstring, int locationId, string createduser, int valuetype, int _UserId, string Customername, string CustomerId, string Faultstr, string FaultId, string Referance, string CallId, bool countdecrease, out string _msg)
        {
            _msg = "";
            string _BillNO = "";
            bool Success = true;
            int _BillCount = 0;
            int itemstatus = 0;
            string itemname, serialnumber, reason;
            int count, categoryId, itemid, locationtableid;


            try
            {
                m_MysqlDb.MyBeginTransaction();

                if (valuetype == 3)
                {

                    string sql = "SELECT MAX(Id) from tblinward";
                    OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                        int.TryParse(MyReader.GetValue(0).ToString(), out _BillCount);
                    MyReader.Close();

                    _BillCount = _BillCount + 1;

                    OdbcDataReader prefixreader = null;
                    string prefix = "";
                    sql = "select Name from tblconfiguration where Module='Inward Prefix'";
                    prefixreader = m_MysqlDb.ExecuteQuery(sql);
                    if (prefixreader.HasRows)
                        prefix = prefixreader.GetValue(0).ToString();
                    prefixreader.Close();
                    _BillNO = "" + prefix + "" + _BillCount.ToString();

                    sql = "insert into tblinward(BillNum,LocationId,ActionDate,Customername,CustomerId) values('" + _BillNO + "'," + locationId + ",'" + System.DateTime.Now.ToString("s") + "','" + Customername + "','" + CustomerId + "')";
                    m_MysqlDb.ExecuteQuery(sql);
                    for (int i = 0; i <= itemstring.GetUpperBound(0); i++)
                    {
                        //Itemname + '#S#' + count + '#S#' + serialnumberforitem + '#S#' + Reason + '#S#' + categoryId + '#S#' + ItemId + '#S#' + LocationTableId
                        //itemid = itemstring[i][5];
                        int.TryParse(itemstring[i][5], out itemid);
                        itemname = itemstring[i][0];
                        int.TryParse(itemstring[i][4], out categoryId);
                        int.TryParse(itemstring[i][6], out locationtableid);
                        int.TryParse(itemstring[i][1], out count);
                        serialnumber = itemstring[i][2];
                        reason = itemstring[i][3];
                        itemstatus = 1;
                        string description = "";
                        sql = "insert into tbltransaction(ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate,Comment,BillNo,Instatus,ReferenceType,ReferenceId,ReferenceCallId,FaultTypeId) values(" + itemid + ",'" + itemname + "'," + categoryId + "," + count + "," + locationId + ",'" + description + "','" + createduser + "','" + serialnumber + "'," + valuetype + ",'" + System.DateTime.Now.ToString("s") + "','" + reason + "','" + _BillNO + "'," + itemstatus + ",'" + Faultstr + "','" + Referance + "','" + CallId + "','" + FaultId + "')";
                        m_MysqlDb.ExecuteQuery(sql);
                        //sql = "Update tblLocationItemStock set AvailableStock=AvailableStock+" + count + ",TotalStock=TotalStock+" + count + " where Id=" + locationtableid;
                        sql = "Update tblLocationItemStock set AvailableStock=AvailableStock+" + count + ",TotalStock=TotalStock+" + count + " where ItemName='" + itemname+"' and LocationId="+locationId+"";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    m_MysqlDb.TransactionCommit();
                }
                else
                {
                    itemstatus = 0;

                    string sql = "SELECT MAX(Id) from tbloutward";
                    OdbcDataReader MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (MyReader.HasRows)
                        int.TryParse(MyReader.GetValue(0).ToString(), out _BillCount);
                    MyReader.Close();
                    _BillCount = _BillCount + 1;
                    OdbcDataReader prefixreader = null;
                    string prefix = "";
                    sql = "select Name from tblconfiguration where Module='Outward Prefix'";
                    prefixreader = m_MysqlDb.ExecuteQuery(sql);
                    if (prefixreader.HasRows)
                        prefix = prefixreader.GetValue(0).ToString();
                    prefixreader.Close();
                    _BillNO = "" + prefix + "" + _BillCount.ToString();

                    sql = "insert into  tbloutward(BillNum,LocationId,ActionDate,Customername,CustomerId) values('" + _BillNO + "'," + locationId + ",'" + System.DateTime.Now.ToString("s") + "','" + Customername + "','" + CustomerId + "')";
                    m_MysqlDb.ExecuteQuery(sql);
                    for (int i = 0; i <= itemstring.GetUpperBound(0); i++)
                    {

                        //itemid = itemstring[i][5];
                        int.TryParse(itemstring[i][5], out itemid);
                        itemname = itemstring[i][0];
                        int.TryParse(itemstring[i][4], out categoryId);
                        int.TryParse(itemstring[i][6], out locationtableid);
                        int.TryParse(itemstring[i][1], out count);
                        serialnumber = itemstring[i][2];
                        reason = itemstring[i][3];
                        // sql = "insert into tbloutward(BillNum,Count,ItemName,Reason,Customername,CustomerId) values('" + _BillNO + "'," + count + ",'" + itemname + "','" + reason + "','" + Customername + "','" + CustomerId + "')";
                        // m_MysqlDb.ExecuteQuery(sql);
                        string description = "";
                        if (serialnumber == "")
                        {

                            sql = "insert into tbltransaction(ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate,Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId) values(" + itemid + ",'" + itemname + "'," + categoryId + "," + count + "," + locationId + ",'" + description + "','" + createduser + "','" + serialnumber + "'," + valuetype + ",'" + System.DateTime.Now.ToString("s") + "','" + reason + "','" + _BillNO + "','" + Faultstr + "','" + Referance + "','" + CallId + "')";
                            m_MysqlDb.ExecuteQuery(sql);
                            sql = "insert into tbltransactionHistory(Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId )  select Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId from tbltransaction where ItemId=" + itemid + " and BillNo='" + _BillNO + "' and CreatedUser='" + createduser + "'";
                            m_MysqlDb.ExecuteQuery(sql);
                          //  sql = "delete from tbltransaction where ItemId=" + itemid + " and BillNo='" + _BillNO + "' and CreatedUser='" + createduser + "'";
                           // m_MysqlDb.ExecuteQuery(sql);

                        }
                        if (countdecrease)
                        {
                            sql = "Update tblLocationItemStock set AvailableStock=AvailableStock+" + (count * -1) + ",TotalStock=TotalStock+" + (count * -1) + " where Id=" + locationtableid;
                            m_MysqlDb.ExecuteQuery(sql);
                        }
                        if (serialnumber != "")
                        {
                            //sql = "Update tbltransaction set Instatus=" + itemstatus + ",OutwardReference='" + _BillNO + "',OutwardType='item Adjustment' where ItemId=" + itemid + " and SerialNumber='" + serialnumber + "'  and LocationId=" + locationId + " and Valuetype in(1,3)";
                            //m_MysqlDb.ExecuteQuery(sql);
                            //sql = "insert into tbltransactionHistory(Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId) select Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId from Viewtransaction where ItemId=" + itemid + " and SerialNumber='" + serialnumber + "'  and LocationId=" + locationId + " and Valuetype in(1,3) ";
                            //m_MysqlDb.ExecuteQuery(sql);
                            //sql = "delete from tbltransaction where ItemId=" + itemid + " and SerialNumber='" + serialnumber + "'  and LocationId=" + locationId + " and Valuetype in(1,3)";
                            //m_MysqlDb.ExecuteQuery(sql);
                            sql = "insert into tbltransaction(ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate,Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId) values(" + itemid + ",'" + itemname + "'," + categoryId + "," + count + "," + locationId + ",'" + description + "','" + createduser + "','" + serialnumber + "'," + valuetype + ",'" + System.DateTime.Now.ToString("s") + "','" + reason + "','" + _BillNO + "','" + Faultstr + "','" + Referance + "','" + CallId + "')";
                            m_MysqlDb.ExecuteQuery(sql);
                            //sql = "insert into tbltransactionHistory(Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId) select Id,ItemId,ItemName,CategoryId, Quantity,LocationId,Description,CreatedUser,SerialNumber,Valuetype,ActionDate, Comment,BillNo,ReferenceType,ReferenceId,ReferenceCallId from Viewtransaction where ItemId=" + itemid + "  and BillNo='" + _BillNO + "' and CreatedUser='" + createduser + "'";
                            //m_MysqlDb.ExecuteQuery(sql);
                            //sql = "delete from tbltransaction where ItemId=" + itemid + "  and BillNo='" + _BillNO + "' and CreatedUser='" + createduser + "'";
                            //m_MysqlDb.ExecuteQuery(sql);

                        }

                    }
                    m_MysqlDb.TransactionCommit();
                }

            }
            catch (Exception Ex)
            {
                m_MysqlDb.TransactionRollback();
                Success = false;
                _msg = Ex.Message;
                _BillNO = "";
            }
            return _BillNO;
        }

        public DataSet GetFaultType()
        {
            string sql = "";
            DataSet TypeDS = new DataSet();
            sql = "SELECT Id,Type FROM tblitemfaulttype";
            TypeDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return TypeDS;

        }

        public DataSet GetFaultTypeReport(int locationId, int TypeId, DateTime startdate, DateTime Enddate)
        {
            string sql = "";
            DataSet ReportDS = new DataSet();
            string tempsql = "";
            if (locationId > 0)
            {
                tempsql = " and tbltransaction.LocationId=" + locationId + "";
            }
            if (TypeId > 0)
            {
                tempsql = tempsql + " and tbltransaction.FaultTypeId=" + TypeId + "";
            }
            sql = "select tbltransaction.Id, tbltransaction.ItemId,tbltransaction.ItemName,tbltransaction.Comment,tbltransaction.CategoryId,tbltransaction.LocationId,tbltransaction.ActionDate as ActionDate,tbltransaction.Quantity,tbltransaction.SerialNumber,tbltransaction.CustomerName,tbltransaction.ReferenceType,tblgroup.GroupName,tblcategory.CategoryName from tbltransaction  inner join tblgroup on tblgroup.Id=tbltransaction.LocationId inner join tblcategory on tblcategory.Id=tbltransaction.CategoryId where tbltransaction.FaultTypeId<>'' and tbltransaction.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            sql = sql + tempsql;
            ReportDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ReportDS;
        }

        public void RemoveBill(int valuetype, string BillId)
        {
            string sql = "";
            OdbcDataReader BillNumReader = null;
            try
            {
                m_TransationDb.MyBeginTransaction();

                if (valuetype == 1)
                {

                    sql = "delete from tbltransaction where Valuetype=1 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tbltransactionHistory where Valuetype=1 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tblbill where BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "select Id from tblbill where BillNo='" + BillId + "'";
                    BillNumReader = m_TransationDb.ExecuteQuery(sql);
                    if (BillNumReader.HasRows)
                    {
                        sql = "delete from tblbillitems where BillId=" + int.Parse(BillNumReader.GetValue(0).ToString()) + "";
                        m_TransationDb.ExecuteQuery(sql);
                    }
                    BillNumReader.Close();
                }
                if (valuetype == 2)
                {

                    sql = "delete from tbltransaction where Valuetype=2 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tbltransactionHistory where Valuetype=2 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tblpurchase  where PONO='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "select Id from tblpurchase where PONO='" + BillId + "'";
                    BillNumReader = m_TransationDb.ExecuteQuery(sql);
                    if (BillNumReader.HasRows)
                    {
                        sql = "delete from tblpurchaseitems where PurchaseId=" + int.Parse(BillNumReader.GetValue(0).ToString()) + "";
                        m_TransationDb.ExecuteQuery(sql);
                    }
                    BillNumReader.Close();

                }
                if (valuetype == 3)
                {

                    sql = "delete from tbltransaction where Valuetype=3 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tbltransactionHistory where Valuetype=3 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tblinward where BillNum='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                }
                if (valuetype == 4)
                {
                    sql = "delete from tbltransaction where Valuetype=4 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tbltransactionHistory where Valuetype=4 and BillNo='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                    sql = "delete from tbloutward where BillNum='" + BillId + "'";
                    m_TransationDb.ExecuteQuery(sql);
                }
                m_TransationDb.TransactionCommit();
            }
            catch (Exception)
            {
                m_TransationDb.TransactionRollback();
            }
        }

        /* string m_stConnection = ConfigurationSettings.AppSettings["ConnectionInfo"];
            OdbcConnection con = new OdbcConnection(m_stConnection);
            OdbcCommand com = new OdbcCommand("[dbo].[Report]",con);
            OdbcDataReader oread = null;
            con.Open();
            com.CommandType = CommandType.StoredProcedure;
            oread= com.ExecuteReader();
            if (oread.HasRows)
            {
                
            }*/



        public DataSet GetTransactionReport(int categoryId, int itemid, DateTime startdate, DateTime Enddate, int locationid)
        {
            string sql = "";
            string tempsql = "";
            //DataSet userds = new DataSet();
            // userds = MyUser.MyAssociatedGroups();
            DataSet TransactionDS = new DataSet();
            if (categoryId > 0)
            {
                tempsql = " and Viewtransaction.CategoryId=" + categoryId + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and Viewtransaction.ItemId=" + itemid + "";
            }
            if (locationid > 0)
            {
                tempsql = tempsql + " and  Viewtransaction.LocationId=" + locationid + "";
            }
            //if (locationid == 0)
            //{
            //    if (userds != null && userds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in userds.Tables[0].Rows)
            //        {
            //            string location = dr[0].ToString();
            //        }
            //    }
            //}
            sql = "select Viewtransaction.Id, Viewtransaction.ActionDate as ActionDate, Viewtransaction.LocationId,Viewtransaction.CategoryId,Viewtransaction.ItemName,Viewtransaction.ItemId,tblgroup.GroupName,Viewtransaction.CustomerName,Viewtransaction.Valuetype,Viewtransaction.BillNo,Viewtransaction.Quantity,Viewtransaction.SerialNumber,Viewtransaction.TotalCost,Viewtransaction.VendorId,Viewtransaction.StockBal from Viewtransaction inner join tblgroup on Viewtransaction.LocationId=tblgroup.Id  where Viewtransaction.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            sql = sql + tempsql;
            TransactionDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return TransactionDS;
        }

        public DataSet GetTransactionReportall(int categoryId, int itemid, DateTime startdate, DateTime Enddate, int locationid,DataSet userds)
        {
            string sql = "";
            string tempsql = "";
            string sql1 = "";
            DataSet TransactionDS = new DataSet();
            if (categoryId > 0)
            {
                tempsql = " and Viewtransaction.CategoryId=" + categoryId + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and Viewtransaction.ItemId=" + itemid + "";
            }
            
            
            sql = "select Viewtransaction.Id, Viewtransaction.ActionDate as ActionDate, Viewtransaction.LocationId,Viewtransaction.CategoryId,Viewtransaction.ItemName,Viewtransaction.ItemId,tblgroup.GroupName,Viewtransaction.CustomerName,Viewtransaction.Valuetype,Viewtransaction.BillNo,Viewtransaction.Quantity,Viewtransaction.SerialNumber,Viewtransaction.TotalCost,Viewtransaction.VendorId,Viewtransaction.StockBal from Viewtransaction inner join tblgroup on Viewtransaction.LocationId=tblgroup.Id  where Viewtransaction.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "'";
            sql = sql + tempsql;

            if (locationid == 0)
            {
                if (userds != null && userds.Tables[0].Rows.Count > 0)
                {
                    string locationIds = "";
                    foreach (DataRow dr in userds.Tables[0].Rows)
                    {
                        string locationId = dr[0].ToString();
                        if (locationIds != "")
                            locationIds += ",";
                        locationIds += locationId;
                    }

                    sql = sql + " and Viewtransaction.LocationId in(" + locationIds + ")";
                }
            }

            TransactionDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            sql1 = "";
            return TransactionDS;
            
        }

        public DataSet GetAllVendor()
        {
            string sql = "";
            DataSet VendorDs = new DataSet();
            sql = "SELECT Id,VendorName FROM tblvendor";
            VendorDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return VendorDs;
        }



        public void ApproveItemRequsetDataBase(int Id, int Isapproved, int Isrejected)
        {
            string sql = "";
            sql = "UPDATE tblindent SET Isapproved=" + Isapproved + ",IsRejected=" + Isrejected + " WHERE Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetItemType()
        {
            string sql = "";
            DataSet ItemTypeDs = new DataSet();
            sql = "select distinct tblitemtype.Id,tblitemtype.ItemType from  tblitemtype";
            ItemTypeDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return ItemTypeDs;
        }

        public void ItemRequest(int userId, int ItemId, string reqno, string description, int count, DateTime reqdate, int tolocationid, int fromlocationid)
        {
            string sql = "";
            sql = "INSERT INTO tblindent(ItemId,UserId,Description,indentstatus,ReqNo,Count,ReqDate,FromLocationId,ToLocationId) VALUES(" + ItemId + "," + userId + ",'" + description + "',0,'" + reqno + "'," + count + ",'" + reqdate.ToString("s") + "'," + fromlocationid + "," + tolocationid + ")";
            m_TransationDb.ExecuteQuery(sql);

        }


        public void ApproveRequest(int Id, int Isapproved, int Isrejected)
        {
            MysqlClass _mysqlObj = new MysqlClass();
            Inventory My_Inventory = new Inventory(_mysqlObj);
            My_Inventory.ApproveItemRequsetDataBase(Id, Isapproved, Isrejected);
            My_Inventory = null;
        }


        public DataSet GetAllRequests(int status, string locationid, int id)
        {
            DataSet RequestDs = new DataSet();
            string needsn = "";
            string sql = "";
            string tempsql = "";
            if (id > 0)
            {
                tempsql = " and tblindent.Id=" + id + "";
            }

            if (locationid != "")
            {
                sql = "select distinct tblindent.Id,tblindent.Count,tblindent.Description,tblindent.indentstatus,tblindent.ItemId,tblindent.ReqDate as ReqDate,tblindent.ReqNo,tblindent.UserId,tbluser.UserName,tblLocationItemStock.ItemName,tblLocationItemStock.AvailableStock,tblindent.FromLocationId,tblgroup.GroupName,tblindent.ToLocationId  ,tblLocationItemStock.NeedSerialNumber,tblLocationItemStock.CategoryId from tblindent inner join tblLocationItemStock on tblLocationItemStock.ItemId=tblindent.ItemId and tblLocationItemStock.LocationId =tblindent.FromLocationId  inner join tbluser on tbluser.Id= tblindent.UserId inner join tblgroup on tblgroup.Id=tblindent.ToLocationId where tblindent.indentstatus=" + status + " and tblindent.FromLocationId=" + locationid;
            }
            else
            {
                sql = "select tblindent.Id,tblindent.Count,tblindent.Description,tblindent.indentstatus,tblindent.ItemId,tblindent.ReqDate as ReqDate,tblindent.ReqNo,tblindent.UserId,tbluser.UserName,tblLocationItemStock.ItemName,tblLocationItemStock.AvailableStock, tblLocationItemStock.Id  as locationtableId,tblindent.FromLocationId,tblgroup.GroupName,tblindent.ToLocationId  " + needsn + " from tblindent inner join tblLocationItemStock on tblLocationItemStock.ItemId=tblindent.ItemId and tblLocationItemStock.LocationId=tblindent.FromLocationId  inner join tbluser on tbluser.Id= tblindent.UserId inner join tblgroup on tblgroup.Id= tblindent.ToLocationId where tblindent.indentstatus=" + status + " ";
            }
            RequestDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return RequestDs;
        }

        public void SaveApprovedRequestToDataBase(int _reqid)
        {
            string sql = "";
            sql = "UPDATE tblindent SET indentstatus = 1 WHERE tblindent.Id=" + _reqid + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public void SaveRejectedRequestToDataBase(int _reqid)
        {
            string sql = "";
            sql = "UPDATE tblindent SET indentstatus = 2 WHERE tblindent.Id=" + _reqid + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public void SaveIssueDetailsToDataBase(int _id, int _itemId, int _userId, int _issuecount, string _reqNo, string _serialnumber, DateTime _issuedate, string itemname, int reqcount, string description, string createduser, int LocationTableId, string _BillNO, int _tolocation, int _fromlocation)
        {
            string insql = "";
            int categoryId = 0;
            OdbcDataReader Catreader = null;
            string _CustomerName = "";
            int newcount = reqcount - _issuecount;
            string[] serialnumberforeachitem = new string[0];
            string _tempsql = "";
            if (newcount == 0)
            {
                _tempsql = ",indentstatus=3";
            }
            insql = "UPDATE tblindent SET Count=" + newcount + ",IssuedQty=IssuedQty+" + _issuecount + " " + _tempsql + " where Id=" + _id + "";
            m_MysqlDb.ExecuteQuery(insql);


            string _catsql = "";
            _catsql = "select tblcategory.Id from tblcategory where tblcategory.Id in(select tblLocationItemStock.CategoryId from tblLocationItemStock where tblLocationItemStock.ItemId=" + _itemId + ")";
            Catreader = m_MysqlDb.ExecuteQuery(_catsql);
            if (Catreader.HasRows)
            {
                int.TryParse(Catreader.GetValue(0).ToString(), out categoryId);
            }

            insql = "INSERT INTO tblissueitems(Id,IssueId,ItemId,ItemName,SerialNumber,FromlocationId,ToLocationId,IndentNo,Count) VALUES()";
            m_MysqlDb.ExecuteQuery(insql);

            //

            //INSERT INTO tblissue(UserId,IssueDate,BillNo,UserName) VALUES()

        }

        public DataSet GetRepportDetails(int userid, DateTime Fromdate, DateTime Todate, string locationid)
        {
            string sql = "";
            DataSet IssueReportDs = new DataSet();
            int _locationid = 0;
            int.TryParse(locationid, out _locationid);
            string subsql = "";
            if (userid > 0)
            {

                subsql = " and tblissue.UserId=" + userid + "";
            }
            if (_locationid > 0)
            {
                subsql = subsql + "  and tblissueitems.FromLocationId=" + locationid + "";
            }
            sql = " select tblissue.BillNo, tblissueitems.Count,tblissue.Id,tblissueitems.IndentNo,tblissue.IssueDate as IssueDate  ,tblissueitems.ItemId,tblissue.UserId,tblissue.UserName,tblitem.ItemName,tblgroup.GroupName as tolocation, tblissueitems.SerialNumber,tblissueitems.IndentDate as IndentDate  from  tblissue inner join tblissueitems on tblissue.Id=tblissueitems.IssueId  inner join  tblitem on  tblitem.Id= tblissueitems.ItemId   inner join tblgroup on tblgroup.Id=tblissueitems.TolocationId   where  tblissue.IssueDate between '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'  " + subsql + " order by IssueDate ";
            IssueReportDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return IssueReportDs;
        }
        public DataSet GetLogDetails(string user, DateTime Fromdate, DateTime Todate, string locationid)
        {
            string sql = "";
            DataSet LogReportDs = new DataSet();
            int _locationid = 0;
            int.TryParse(locationid, out _locationid);
            string subsql = "";
            if (user!="All")
            {

                subsql = " and tbluseraction.UserName=" + user + "";
            }
            if (_locationid >0)
            {
                subsql = subsql + "  and tbluseraction.LocationId=" + locationid + "";
            }
            sql = " select tbluseraction.UserName as UserName, tbluseraction.Time as Time,tbluseraction.Action as Action,tbluseraction.Description,tblgroup.GroupName as Location from  tbluseraction  left join tblgroup on tblgroup.Id=tbluseraction.LocationId   where  tbluseraction.Time between '" + Fromdate.ToString("s") + "' and '" + Todate.ToString("s") + "'  " + subsql + " order by Time ";
            LogReportDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return LogReportDs;
        }

        public int getItemId(string ItemName, out int CategoryId)
        {
            CategoryId = 0;
            int ItemId = 0;
            string sql = "SELECT Id,CatgryId FROM tblitem WHERE ItemName='" + ItemName + "'";
            OdbcDataReader _myreader = m_MysqlDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int.TryParse(_myreader.GetValue(0).ToString(), out ItemId);
                int.TryParse(_myreader.GetValue(1).ToString(), out CategoryId);

            }
            _myreader = null;
            return ItemId;
        }

        public int getlocationTableId(int itemId, int LocationId)
        {

            int Id = 0;
            string sql = "SELECT Id FROM tblLocationItemStock WHERE LocationId=" + LocationId + " AND ItemId=" + itemId;
            OdbcDataReader _myreader = m_MysqlDb.ExecuteQuery(sql);
            if (_myreader.HasRows)
            {
                int.TryParse(_myreader.GetValue(0).ToString(), out Id);

            }
            _myreader = null;
            return Id;
        }

        public bool IsSerialNoPresent(int ItemId, string SerialNumber)
        {
            bool _valid = true;
            string sql = "select SerialNumber from tbltransaction where Instatus=1 and SerialNumber='" + SerialNumber + "' and ItemId=" + ItemId;
            DataSet _mydata = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_mydata != null && _mydata.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _mydata.Tables[0].Rows)
                {
                    string m_SerialNumber = dr[0].ToString();
                    if (m_SerialNumber == SerialNumber)
                    {
                        _valid = false;
                        break;
                    }
                }
            }

            return _valid;
        }

        public int DeleteItem(int itemID, int deletevalue, int locationid, out int stockvalue)
        {
            int value = 0;
            double totalstock = 0;
            DataSet ItemStockDs = new DataSet();
            stockvalue = 0;
            string sql = "", delsql = "";
            if (deletevalue == 1)
            {
                sql = "select tblLocationItemStock.TotalStock,Id from tblLocationItemStock where tblLocationItemStock.ItemId=" + itemID + "";
                ItemStockDs = m_TransationDb.ExecuteQueryReturnDataSet(sql);
                if (ItemStockDs != null && ItemStockDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ItemStockDs.Tables[0].Rows)
                    {
                        double.TryParse(dr["TotalStock"].ToString(), out totalstock);
                        if (totalstock <= 0)
                        {
                            delsql = "DELETE FROM tblLocationItemStock where Id=" + dr["Id"].ToString() + "";
                            m_TransationDb.ExecuteQuery(delsql);
                        }
                        else
                        {
                            stockvalue = 1;
                        }

                    }

                    if (stockvalue == 0)
                    {

                        delsql = "UPDATE tblitem SET Status=0 where Id=" + itemID + "";
                        m_TransationDb.ExecuteQuery(delsql);
                    }
                }
                else
                {
                    delsql = "UPDATE tblitem SET Status=0 where Id=" + itemID + "";
                    m_TransationDb.ExecuteQuery(delsql);
                }
            }
            else
            {
                OdbcDataReader stockreader = null;
                int totalstock1 = 0;
                string stocksql = "select tblLocationItemStock.TotalStock,Id from tblLocationItemStock where tblLocationItemStock.ItemId=" + itemID + " and locationid=" + locationid + "";
                stockreader = m_TransationDb.ExecuteQuery(stocksql);
                if (stockreader.HasRows)
                {
                    int.TryParse(stockreader.GetValue(0).ToString(), out totalstock1);
                    if (totalstock > 0)
                    {
                        delsql = "DELETE FROM tblLocationItemStock where ItemId=" + itemID + " and locationid=" + locationid + "";
                    }
                    else
                    {
                        stockvalue = 1;

                    }
                }
                stockreader.Close();
                m_TransationDb.ExecuteQuery(delsql);
            }

            return value;
        }



        public DataSet GetMoveItemReport(int categoryId, int locationid, int itemid, DateTime startdate, DateTime Enddate, int fromlocationid, int userid)
        {
            string sql = "", tempsql = "";
            DataSet ReportDS = new DataSet();
            if (locationid > 0)
            {
                tempsql = " and tblmoveitem.FromlocationId=" + locationid + "";
            }
            if (itemid > 0)
            {
                tempsql = tempsql + " and tblmoveitem.ItemId=" + itemid + "";
            }
            if (categoryId > 0)
            {
                tempsql = tempsql + " and tblmoveitem.CategoryId=" + categoryId + "";
            }

            sql = "SELECT Id,ItemId,ItemName,Quantity,SerialNumber,tblmoveitem.ActionDate as ActionDate,CreatedUser,FromlocationId,ToLocationId,CategoryId,Remark FROM tblmoveitem where tblmoveitem.ActionDate between '" + startdate.ToString("s") + "' and '" + Enddate.ToString("s") + "' " + tempsql + "";
            ReportDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ReportDS != null && ReportDS.Tables[0].Rows.Count > 0)
            {
                ReportDS.Tables[0].Columns.Add("Fromlocation");
                ReportDS.Tables[0].Columns.Add("ToLocation");

                OdbcDataReader locationreader = null;
                string sql1 = "SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + userid + " UNION SELECT DISTINCT tblgroup.Id, tblgroup.GroupName FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + userid + "";
                locationreader = m_MysqlDb.ExecuteQuery(sql1);
                if (locationreader.HasRows)
                {
                    while (locationreader.Read())
                    {
                        foreach (DataRow dr in ReportDS.Tables[0].Rows)
                        {
                            if (dr["FromlocationId"].ToString() == locationreader.GetValue(0).ToString())
                            {
                                dr["Fromlocation"] = locationreader.GetValue(1).ToString();
                            }

                            if (dr["ToLocationId"].ToString() == locationreader.GetValue(0).ToString())
                            {
                                dr["ToLocation"] = locationreader.GetValue(1).ToString();
                            }
                        }
                    }
                }
            }
            return ReportDS;
        }


        public DataSet GetAllRejectedItems(int userid, string locationid)
        {
            DataSet RequestDs = new DataSet();
            string sql = "";
            //   and tblindent.UserId=" + userid + " 
            sql = "select distinct tblindent.Id,tblindent.Count,tblindent.Description,tblindent.ItemId,tblindent.ReqDate as ReqDate,tblindent.ReqNo,tblindent.UserId,tbluser.UserName,tblLocationItemStock.ItemName,tblgroup.GroupName  from tblindent inner join tblLocationItemStock on  tblLocationItemStock.ItemId=tblindent.ItemId inner join tbluser on tbluser.Id= tblindent.UserId inner join tblgroup on tblgroup.Id= tblindent.FromLocationId where tblindent.indentstatus=2";
            RequestDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return RequestDs;
        }




        public void UpdateIssueLocation(string IndentId, string newlocatoinid)
        {
            string sql = "";
            sql = "UPDATE tblindent SET FromLocationId =" + newlocatoinid + " WHERE tblindent.Id=" + IndentId + "";
            m_TransationDb.ExecuteQuery(sql);
        }

        public bool SukIdMandatory()
        {
            string sql = "";
            bool mandatory = true;
            OdbcDataReader sukidconfigreader = null;
            sql = "select value from tblconfiguration where tblconfiguration.Name='sukmandatory'";
            sukidconfigreader = m_MysqlDb.ExecuteQuery(sql);
            if (sukidconfigreader.HasRows)
            {
                int configvalue;
                int.TryParse(sukidconfigreader.GetValue(0).ToString(), out configvalue);
                if (configvalue == 0)
                {
                    mandatory = false;
                }
            }
            sukidconfigreader.Close();
            sukidconfigreader = null;
            return mandatory;
        }

        public bool SukIdExist(string sukid, string ItemID)
        {
            bool exist = false;
            string sql = "";
            OdbcDataReader IdReader = null;
            if (ItemID == "")
            {
                sql = "select Id from tblitem where tblitem.SUKID='" + sukid + "'";

            }
            else
            {
                sql = "select Id from tblitem where tblitem.SUKID='" + sukid + "' and Id<>" + ItemID + "";
            }
            IdReader = m_MysqlDb.ExecuteQuery(sql);
            if (IdReader.HasRows)
            {
                exist = true;
            }
            IdReader.Close();
            return exist;
        }
        public bool serialnumberexist(string srlid)
        {
            bool exist = false;
            string sql = "";
            OdbcDataReader IdReader = null;
            sql = "select Id from tblserialnumber where tblserialnumber.SerialNumber='" + srlid + "'";
            
            IdReader = m_MysqlDb.ExecuteQuery(sql);
            if (IdReader.HasRows)
            {
                exist = true;
            }
            IdReader.Close();
            return exist;
        }

        public DataSet GetCategoryDetails()
        {
            string sql = "";
            sql = "select tblcategory.Id,tblcategory.CategoryName,tblcategory.Description,tblcategory.CategoryType as CategoryTypeId,case when  tblcategory.CategoryType=1 then 'consumable' when  tblcategory.CategoryType=2 then 'non consumable' end as type from tblcategory where tblcategory.Status=1";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }



        public void UpdateCategory(string Id, string categoryname, string description, int categorytype)
        {
            string sql = "Update tblcategory set CategoryName='" + categoryname + "',Description='" + description + "',CategoryType=" + categorytype + " where Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void UpdateItem(string Id, string itemname, string description, int itemtype,int location)
        {
            string sql = "Update tblserialnumber set SerialNumber='" + itemname + "',Description='" + description + "',ItemId=" + itemtype +  ",LocationId="+location+" where Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void SerialNumberExistForPurchaseAndAdjustment(string[] SerialNumber, string itemname, out DataSet serialjson)
        {
            string Purchaseserialnumber = "", sql = "", ser = "";
            int catid = 0, itemid = 0;
            serialjson = new DataSet();
            itemid = getItemId(itemname, out catid);
            for (int i = 0; i <= SerialNumber.GetUpperBound(0); i++)
            {
                Purchaseserialnumber = Purchaseserialnumber + "'" + SerialNumber[i].ToString() + "'";
                if (SerialNumber.Length > 1 && i != SerialNumber.Length - 1)
                {
                    Purchaseserialnumber = Purchaseserialnumber + ",";
                }
            }
            //union select  tbltransactionHistory.SerialNumber from tbltransactionHistory  where tbltransactionHistory.SerialNumber<>'' and tbltransactionHistory.SerialNumber<>'&nbsp;' and tbltransactionHistory.ItemId='" + itemid + "' and tbltransactionHistory.SerialNumber  in (" + Purchaseserialnumber + ")
            sql = "select tbltransaction.SerialNumber from tbltransaction  where tbltransaction.SerialNumber<>'' and tbltransaction.SerialNumber<>'&nbsp;' and tbltransaction.ItemId='" + itemid + "'    and tbltransaction.Instatus=1 and  tbltransaction.SerialNumber  in(" + Purchaseserialnumber + ") ";
            serialjson = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public void GetSerialNumberForSaleAndAdjustment(string locationid, out DataSet serialjsonDS)
        {
            string serialnumsql = "";
            serialjsonDS = new DataSet();
            serialnumsql = "select distinct itemId,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(SerialNumber,'/',' '),',',' '),'-',' '),'_',' '),'\',' '),'^',' '),'|',' '),'*',' '),'',' '),'\',' '),'(',' '),'[',' '),'.',' ') as SerialNumber,ItemName from tbltransaction where SerialNumber<>'' and tbltransaction.SerialNumber<>'&nbsp;' and Instatus=1 and LocationId=" + locationid + "";
            serialjsonDS = m_MysqlDb.ExecuteQueryReturnDataSet(serialnumsql);
        }

        public bool ItemCanSale(string ItemName, double qty, string locationid)
        {
            string sql = "";
           string itemname =ItemName;
            bool cansale = true;
            OdbcDataReader idreader = null;
            sql = "select tblLocationItemStock.Id,AvailableStock from tblLocationItemStock where ItemName='" + itemname + "' and AvailableStock>" + qty + " and LocationId=" + locationid + "";
            idreader = m_MysqlDb.ExecuteQuery(sql);
            if (idreader.HasRows)
            {
                cansale = true;
            }
            else cansale = false;

            return cansale;
        }
        public bool ItemCanSaleslno(string ItemName, double qty, string locationid)
        {
            string sql = "";
            string itemname = ItemName;
            bool cansale = true;
            OdbcDataReader idreader = null;
            sql = "select tblLocationItemStock.Id,AvailableStock from tblLocationItemStock where ItemId=" + itemname + " and AvailableStock>=" + qty + " and LocationId=" + locationid + "";
            idreader = m_MysqlDb.ExecuteQuery(sql);
            if (idreader.HasRows)
            {
                cansale = true;
            }
            else cansale = false;

            return cansale;
        }
        public DataSet GetScheduleTypeDataset()
        {
            string sql = "SELECT Id, Schedule FROM tblscheduletype";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public DataSet GetRemainderTypeDataset()
        {
            string sql = "SELECT Id, RemainderType FROM tblremainder";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public bool ScheduleReminder(int remainder, int scheduletype, DateTime Scheduledate, DateTime NextDate, string itemname, string itemtype)
        {
            string sql = "";
            DataSet collectremainderdata = new DataSet();
            sql = "(SELECT tblserialnumber.Id as ItemId, tblserialnumber.ItemId as ItemtypeId FROM tblserialnumber WHERE tblserialnumber.SerialNumber='" + itemname + "')(SELECT tblremainder.RemainderType as RemainderTypeName FROM tblremainder WHERE tblremainder.Id =" + remainder + ")(tblscheduletype.Schedule as ScheduleType FROM tblserialnumber WHERE tblscheduletype.Id = " + scheduletype + ")";
            collectremainderdata = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return true;
        }
        public DataSet GetItemsId(string itemname)
        {
            string sql = "SELECT Id as ItemId,ItemId as ItemtypeId FROM tblserialnumber WHERE SerialNumber = '" + itemname + "'";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public DataSet GetStoredRemainderDetails(string itemname, string itemtype)
        {
            string sql = "SELECT Id, EntityId, EntityName, EntityTypeId, EntityTypeName, RemainderTypeId, RemainderTypeName, ScheduleTypeId, ScheduleTypeName, ScheduleStart, NextDate,Active FROM tblremainderschedule WHERE EntityName = '" + itemname + "' AND EntityTypeName = '" + itemtype + "'";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public DataSet GetTransactionreportdetails(string itemname, int itemtype, string status, int location, DateTime startdate, DateTime enddate)
        {
            string sql, sql_itemname = "", sql_itemtype = "", sql_status = "", sql_location = "";
            if (itemname != "")
            {
                sql_itemname = "AND tblremaindertransaction.EntityName = '" + itemname + "' ";
            }
            if (itemtype != 0)
            {
                sql_itemtype = "AND tblremaindertransaction.EntityTypeId = " + itemtype + "";
            }
            if (status != "ALL")
            {
                sql_status = "AND tblremaindertransaction.Status = '" + status + "'";
            }
            if( location != 0)
            {
                sql_location = "AND tblremaindertransaction.LocationId = " + location + "";    
            }
            sql = "SELECT DISTINCT tblremaindertransaction.Id, tblremaindertransaction.ScheduleId, tblremaindertransaction.Date, tblremaindertransaction.EntityName, tblremaindertransaction.EntityTypeName, tblremaindertransaction.RemainderTypeName, tblremaindertransaction.ScheduleTypeName, tblgroup.GroupName as Location, tblremaindertransaction.NextDate, tblremaindertransaction.Status, tblremaindertransaction.Comment FROM tblremaindertransaction INNER JOIN tblgroup ON tblgroup.Id = tblremaindertransaction.LocationId WHERE tblremaindertransaction.Date BETWEEN '" + startdate.ToString("s") + "' AND '" + enddate.ToString("s") + "' " + sql_location + " " + sql_itemname + " " + sql_itemtype + " " + sql_status + " ";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
        public DataSet getpaymenttransaction(int locationid)
        {
           string sql = "SELECT Id,Date,Reference,Itemtype,ItemId,ItemName,VendorId,VendorName,Litre,Amount,Milage,LocationId from tblpaymenttransaction where LocationId="+locationid+" ";
           
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);

        }
        public bool RemainderAction(string status, string Comment, int Remainderid)
        {
            string sql = "UPDATE tblremaindertransaction SET Status='" + status + "', Comment='" + Comment + "' WHERE Id=" + Remainderid + "";
            m_MysqlDb.ExecuteQuery(sql);
            return true;
        }

        public void SaveFixedAssetsToDatabase(string _itemName, string _itemtype, int _category, double _minStock, string _description, int _unitId, int _taxtypeId, int Location, int needserialnumber, int countablestatus, string sukid, DateTime createddate)
        {
            string sql;
            DataSet itemDS = new DataSet();
            //if (extitem == 1)
            
            //{
            //    sql = "INSERT INTO tblitemtype (ItemType, Description) VALUES('" + _itemtype + "','" + _description + "')";
            //    m_TransationDb.ExecuteQuery(sql);
            //}
            sql = "SELECT tblitemtype.Id FROM tblitemtype WHERE tblitemtype.ItemType = '" + _itemtype + "'";
            int itemid = 0;
            itemDS = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            if (itemDS != null && itemDS.Tables[0].Rows.Count > 0)
            {
                itemid = int.Parse(itemDS.Tables[0].Rows[0][0].ToString());
                
            }

            sql = "SELECT tblLocationItemStock.LocationId, tblLocationItemStock.TotalStock FROM tblLocationItemStock WHERE tblLocationItemStock.ItemId = " + itemid + " AND  tblLocationItemStock.ItemName = '" + _itemtype + "'";
            itemDS = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            int itemlocation = 0, itemstock = 0;
            if(itemDS != null && itemDS.Tables[0].Rows.Count > 0)
            {
                itemlocation = int.Parse(itemDS.Tables[0].Rows[0][0].ToString());
                itemstock = int.Parse(itemDS.Tables[0].Rows[0][1].ToString());
                int totalstock = itemstock + 1;
                sql = "UPDATE tblLocationItemStock SET LocationId=" + itemlocation + ", TotalStock=" + totalstock + " WHERE tblLocationItemStock.ItemId = " + itemid + " AND  tblLocationItemStock.ItemName = '" + _itemtype + "'"; 
            }
            else
                sql = "INSERT INTO tblLocationItemStock (ItemId, ItemName, LocationId, SUKID, CategoryId, TotalStock, AvailableStock, Status, Price1, Price2, Price3, Price4, Price5, TaxTypeId, UnitID, AvgPurchaseCost, MinQty, MinOrderQty, MaxQty, NeedSerialNumber, CountableStatus, Onrepair, Defetive) VALUES(" + itemid + ",'" + _itemtype + "'," + Location + ",'" + sukid + "'," + _category + ", " + 1 + "," + 1 + "," + 1 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + _taxtypeId + "," + _unitId + "," + 0 + "," + _minStock + "," + 0 + "," + 0 + "," + needserialnumber + "," + countablestatus + "," + 0 + "," + 0 + ")";
            m_TransationDb.ExecuteQuery(sql);


            //sql = "INSERT INTO tbltransaction (ItemId, ItemName, CategoryId, Actiondate, Quantity, LocationId, VendorId, Valuetype, SerialNumber) VALUES(" + itemid + ", '" + _itemtype + "', " + _category + ",'" + createddate.ToString("s") + "'," + 1 + "," + Location + "," + 0 + "," + 0 + ",'" + _itemName + "')";
            //m_TransationDb.ExecuteQuery(sql);
            sql = "INSERT INTO tblserialnumber (ItemId, SerialNumber, LocationId, Description) VALUES (" + itemid + ",'" + _itemName + "'," + Location + ",'" + _description + "')";
            m_TransationDb.ExecuteQuery(sql);
            

        }
        public void logtodb(string action,string description, int LocationId)
        {
         
            DateTime dt = System.DateTime.UtcNow;
            string sql = "INSERT INTO tbluseraction(UserName,Action,Time,Description,LocationId) values('" + LoginUserName + "','" + action + "','" + dt.ToString("s") + "','" + description + "'," + LocationId + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void log(string action, string description, int LocationId)
        {
            DateTime dt = System.DateTime.UtcNow;
            string sql = "INSERT INTO tbluseraction(UserName,Action,Time,Description,LocationId) values('" + LoginUserName + "','" + action + "','" + dt.ToString("s") + "','" + description + "'," + LocationId + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }
        public void logdb(string action, string description)
        {
            DateTime dt = System.DateTime.UtcNow;
            string sql = "INSERT INTO tbluseraction(UserName,Action,Time,Description) values('" + LoginUserName + "','" + action + "','" + dt.ToString("s") + "','" + description + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void logdb1(string username,string action, string description, int LocationId)
        {

            DateTime dt = System.DateTime.UtcNow;
            string sql = "INSERT INTO tbluseraction(UserName,Action,Time,Description,LocationId) values('" + username + "','" + action + "','" + dt.ToString("s") + "','" + description + "'," + LocationId + ")";
            m_MysqlDb.ExecuteQuery(sql);
        }
        public void logdb2(string username,string action, string description)
        {
            DateTime dt = System.DateTime.UtcNow;
            string sql = "INSERT INTO tbluseraction(UserName,Action,Time,Description) values('" + username + "','" + action + "','" + dt.ToString("s") + "','" + description + "')";
            m_MysqlDb.ExecuteQuery(sql);
        }
    }
}
        