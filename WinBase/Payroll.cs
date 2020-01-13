using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Net;
using System.Data;
using System.Web;


namespace WinBase
{
    public class Payroll : KnowinGen
    {
        private KnowinUser MyUser;
        public MysqlClass m_MysqlDb;
        public DBLogClass m_DbLog = null;
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_TransationDb = null;
        private string m_PayrollMenuStr;
        private string m_SubPayrollMenuStr;

        
        public Payroll(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
            m_DbLog = new DBLogClass(m_MysqlDb);
            m_PayrollMenuStr = "";
            m_SubPayrollMenuStr = "";
        }

        ~Payroll()
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




        public void AddHead(string _head, string _type, string _comment, int Decrease,string UserName)
        {
            string sql = " insert into tblpay_head (HeadName,Type,Comment, DecreaseType) values ('" + _head + "','" + _type + "','" + _comment + "'," + Decrease + ")";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
                m_DbLog.LogToDb(UserName, "Create payroll head", "Payroll head " + _head + " is created", 1, m_TransationDb);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
               m_DbLog.LogToDb(UserName, "Create payroll head", "Payroll head " + _head + " is created", 1);
            }

        }
     

        public void AddCategory(string _Category, int _BasicPay, string _WagesType)
        {
            string sql = " insert into tblpay_category (CategoryName,BasicPay,WagesType) values ('" + _Category + "'," + _BasicPay + ",'" + _WagesType + "')";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }

        public DataSet GetHeads()
        {
            DataSet _Head = new DataSet();
            string sql = "select tblpay_head.Id, tblpay_head.HeadName,tblpay_head.Type, tblpay_head.Comment , tblpay_head.DecreaseType from tblpay_head ";
           _Head = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (_Head.Tables[0].Rows.Count > 0)
           {
               
               DataTable dtAgent = _Head.Tables[0];
               DataView dataView = new DataView(dtAgent);
               
           }
          
            return _Head;
        }

        public OdbcDataReader Fill(int _Id)
        {
            int _PayId = _Id;
            string sql = "select tblpay_head.Id, tblpay_head.HeadName,tblpay_head.Type, tblpay_head.Comment from tblpay_head where tblpay_head.Id= " + _PayId + " ";
           return m_MysqlDb.ExecuteQuery(sql);

        }

        public void SaveHead(int _PayId, string _head, string _TypeId, string _comment, int _DecType,string username)
        {
            string sql = " Update tblpay_head SET tblpay_head.HeadName= '" + _head + "', tblpay_head.Type= '" + _TypeId + "', tblpay_head.Comment= '" + _comment + "', tblpay_head.DecreaseType = " + _DecType + " Where tblpay_head.Id= " + _PayId + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_DbLog.LogToDb(username, "Update payroll head", "payroll head " + _head + " is updated", 1);

        }

        public void DeleteHead(int _HeadID)
        {
            int _Id = _HeadID;
            string sql = "delete from tblpay_head where tblpay_head.Id = " + _Id + "";
            m_MysqlDb.ExecuteQuery(sql);
            sql = " delete from tblpay_headcategorymap where tblpay_headcategorymap.HeadId = " + _Id + "";
            m_MysqlDb.ExecuteQuery(sql);
            sql = " delete from tblpay_monthempheadmap where tblpay_monthempheadmap.HeadId = " + _Id + "";
            m_MysqlDb.ExecuteQuery(sql);
            sql = " delete from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId = " + _Id + "";
            m_MysqlDb.ExecuteQuery(sql);


        }

        public void AddHeadCatMap(int _ChkVal, int _MaxId)
        {
            string sql = " insert into tblpay_headcategorymap (CategoryId,HeadId) values (" + _MaxId + "," + _ChkVal + ")";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }

        public OdbcDataReader FillCheckList(string TableName, string FieldName)
        {

            string sql = "SELECT " + TableName + ".Id," + TableName + "." + FieldName + " FROM " + TableName + "";
            return m_MysqlDb.ExecuteQuery(sql);
            
        }

        public DataSet GetCat()
        {
            DataSet _Cat = new DataSet();
            string sql = "select tblpay_category.Id,tblpay_category.CategoryName,tblpay_category.BasicPay, tblpay_category.WagesType from tblpay_category ";
            _Cat = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Cat.Tables[0].Rows.Count > 0)
            {
                DataTable dtAgent = _Cat.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _Cat;
        }

        public void DeleteCat(int _CatID)
        {
            string sql = "delete from tblpay_category where tblpay_category.Id = " + _CatID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

            sql = "delete from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId = " + _CatID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

       

        public OdbcDataReader FillCheck(int _SelectId)
        {
            string sql = " select tblpay_headcategorymap.CategoryId, tblpay_headcategorymap.HeadId from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId = "+ _SelectId +"";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void SaveCat(int _Id, string _CatName, string _BasicPay, string _WagesType,string username)
        {
          
            string sql = " Update tblpay_category SET tblpay_category.CategoryName= '" + _CatName + "', tblpay_category.BasicPay= '" + _BasicPay + "', tblpay_category.WagesType= '" + _WagesType + "' Where tblpay_category.Id= " + _Id + " ";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
           m_DbLog.LogToDb(username, "Update payroll type", "PayrollType " + _CatName + " is updated", 1, m_TransationDb);


            
            //select tblpay_employee.EmpId from tblpay_employee where tblpay_employee.PayrollType=1
        }


      

        public void DeleteChk(int _SaveCatId)
        {
            int _Id = _SaveCatId;
            string sql = "delete from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId = " + _Id + "";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }



        public DataSet FillDrp()
        {
            DataSet _DrpCat = new DataSet();
            string sql = "select tblpay_category.Id,tblpay_category.CategoryName from tblpay_category order by  tblpay_category.CategoryName";
            _DrpCat = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpCat;
        }

        public OdbcDataReader GetEmp()
        {
            //DataSet _Emp = new DataSet();
            string sql = "select DISTINCT tblpay_employee.Id, tblpay_employee.Surname,tblpay_employee.BasicPay,tblpay_employee.Gross,tblpay_employee.Deduction,tblpay_employee.NetAmt,tblpay_employee.Approval, tblpay_employee.PayrollType, tblpay_employee.EmpId, tblpay_employee.Reject from tblpay_employee  where tblpay_employee.PayrollType in (select tblpay_category.Id from tblpay_category)  and  tblpay_employee.`Status`=1";
            //string sql = "select tblpay_employee.Id, tblpay_employee.Surname,tblpay_employee.BasicPay,tblpay_employee.Gross,tblpay_employee.Deduction,tblpay_employee.NetAmt,tblpay_employee.Approval, tblpay_employee.PayrollType, tblpay_employee.EmpId, tblpay_employee.Reject from tblpay_employee inner join tblpay_employeeheadmap on tblpay_employee.EmpId= tblpay_employeeheadmap.EmployeeId where tblpay_employeeheadmap.CategoryId in (select tblpay_category.Id from tblpay_category)";
            //inner join tbluser on tblpay_employee.StaffId= tbluser.Id
            return m_MysqlDb.ExecuteQuery(sql);
            //if (_Emp.Tables[0].Rows.Count > 0)
            //{

            //    DataTable dtAgent = _Emp.Tables[0];
            //    DataView dataView = new DataView(dtAgent);
            //}
            //return _Emp;
           
        }

        public OdbcDataReader LoadEditPopUp(string  _EmpId)
        {
            string sql = "select tblpay_employee.Surname, tblpay_employee.BasicPay, tblpay_employee.PAN,  tblpay_employee.AccNo, tblpay_employee.Gross,tblpay_employee.Deduction,tblpay_employee.NetAmt, tblpay_employee.Comment from tblpay_employee where  tblpay_employee.EmpId = '" + _EmpId + "'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void SaveEmp(int _PayId, int _Bp, int _Amt, int _PayrollId)
        {
            string sql = " Update tblpay_employee SET tblpay_employee.BasicPay= " + _Bp + ", tblpay_employee.NetAmt= " + _Amt + ", tblpay_employee.PayrollType= " + _PayrollId + " Where tblpay_employee.Id= " + _PayId + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }


        public DataSet GetEarningEmp(string  _EmpId)
        {
            DataSet _Earn = new DataSet();
            string sql = "select tblpay_head.Id, tblpay_head.HeadName from tblpay_head inner join tblpay_employee on tblpay_employee.Id= '" + _EmpId + "' where tblpay_head.Type = 'Earnings' and tblpay_head.Id in (select distinct tblpay_headcategorymap.HeadId from tblpay_headcategorymap inner join tblpay_employee on tblpay_employee.PayrollType = tblpay_headcategorymap.CategoryId)";
            _Earn = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Earn.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _Earn.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _Earn;
        }

        public DataSet GetDeductionEmp(string  _EmpId)
        {
            DataSet _Ded = new DataSet();
            string sql = "select tblpay_head.Id, tblpay_head.HeadName from tblpay_head inner join tblpay_employee on tblpay_employee.Id= '" + _EmpId + "' where tblpay_head.Type = 'Deductions' and tblpay_head.Id in (select distinct tblpay_headcategorymap.HeadId from tblpay_headcategorymap inner join tblpay_employee on tblpay_employee.PayrollType = tblpay_headcategorymap.CategoryId)";
            _Ded = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_Ded.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _Ded.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _Ded;
        }

        public DataSet FillEarnDrp()
        {
            DataSet _DrpEarn = new DataSet();
            string sql = "select tblpay_head.Id,tblpay_head.HeadName from tblpay_head where tblpay_head.Type='Earnings'";
            _DrpEarn = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpEarn;
        }

        public DataSet FillDedDrp()
        {
            DataSet _DrpDed = new DataSet();
            string sql = "select tblpay_head.Id,tblpay_head.HeadName from tblpay_head where tblpay_head.Type='Deductions'";
            _DrpDed = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpDed;
        }

        public DataSet AddPreviousItems(int EarnId, string EarnText)
        {
            
           // TextBox = new te Txt_SDate, Txt_EDate;
            DataSet _Earndataset = new DataSet();
            DataTable dt;
            DataRow dr;
            _Earndataset.Tables.Add(new DataTable("EarnHead"));
            dt = _Earndataset.Tables["EarnHead"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Earnings");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Remove");

            dr = _Earndataset.Tables["EarnHead"].NewRow();
            dr["Id"] = EarnId;
            dr["Earnings"] = EarnText;
            //dr["Amount"] = Txt_Quantity.Text;
            //dr["Remove"] = 
                 
           // Txt_SDate = (TextBox)gv.FindControl("Txt_SDate");
            _Earndataset.Tables["EarnHead"].Rows.Add(dr);
            return _Earndataset;
        }

        public void AddNewEarn(int _HeadId , string  EmpId, int _CatId, int headAmount)
        {
            string sql = "insert into tblpay_employeeheadmap(EmployeeId,CategoryId,HeadId,HeadAmount) values('" + EmpId + "'," + _CatId + ", " + _HeadId + ", " + headAmount + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public bool NotHeadPresent(int _NewId, string  EmplId)
        {
            bool _head = false;
            string sql = "select * from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId = " + _NewId + " and tblpay_employeeheadmap.EmployeeId='" + EmplId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (!m_MyReader.HasRows)
            {
                _head = true;
            }
            return _head;
           

           
        }

        public DataSet GetNewEarningEmp(string  _newEarnId)
        {
            DataSet _NewEarn = new DataSet();
            string sql = "select DISTINCT tblpay_employeeheadmap.Id,tblpay_employeeheadmap.HeadId, tblpay_head.HeadName, tblpay_employeeheadmap.HeadAmount, tblpay_head.DecreaseType from tblpay_employeeheadmap inner join tblpay_head on tblpay_employeeheadmap.HeadId= tblpay_head.Id where tblpay_employeeheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Earnings') and tblpay_employeeheadmap.EmployeeId = '" + _newEarnId + "'";
            _NewEarn = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_NewEarn.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewEarn.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewEarn;
        }

        public DataSet GetNewDeductionEmp(string  _newDedId)
        {
            DataSet _NewDed = new DataSet();
            string sql = "select DISTINCT tblpay_employeeheadmap.Id,tblpay_employeeheadmap.HeadId, tblpay_head.HeadName, tblpay_employeeheadmap.HeadAmount, tblpay_head.DecreaseType from tblpay_employeeheadmap inner join tblpay_head on tblpay_employeeheadmap.HeadId= tblpay_head.Id where tblpay_employeeheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Deductions') and tblpay_employeeheadmap.EmployeeId = '" + _newDedId + "'";
            _NewDed = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_NewDed.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewDed.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewDed;
        }



        public DataSet GetNewTEarningEmp(int _newEarnId)
        {
            DataSet _NewEarn = new DataSet();
            string sql = "select DISTINCT tblpay_employeeheadmap.Id,tblpay_employeeheadmap.HeadId, tblpay_head.HeadName, tblpay_employeeheadmap.HeadAmount, tblpay_head.DecreaseType from tblpay_employeeheadmap inner join tblpay_head on tblpay_employeeheadmap.HeadId= tblpay_head.Id where tblpay_employeeheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Earnings') and tblpay_employeeheadmap.EmployeeId = " + _newEarnId + "";
            _NewEarn = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            if (_NewEarn.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewEarn.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewEarn;
        }
        public DataSet GetNewTDeductionEmp(int _newDedId)
        {
            DataSet _NewDed = new DataSet();
            string sql = "select DISTINCT tblpay_employeeheadmap.Id,tblpay_employeeheadmap.HeadId, tblpay_head.HeadName, tblpay_employeeheadmap.HeadAmountm, tblpay_head.DecreaseType from tblpay_employeeheadmap inner join tblpay_head on tblpay_employeeheadmap.HeadId= tblpay_head.Id where tblpay_employeeheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Deductions') and tblpay_employeeheadmap.EmployeeId = " + _newDedId + "";
            _NewDed = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            if (_NewDed.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewDed.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewDed;
        }

        public void AddNewHeadMap(int NewDedId, string  EmplId, int CatDedId, double HeadAmount)
        {
            string sql = "insert into tblpay_employeeheadmap(EmployeeId,CategoryId,HeadId,HeadAmount) values('" + EmplId + "'," + CatDedId + ", " + NewDedId + ", " + HeadAmount + ")";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }



        public void DeleteHead(int _HeadID, string  EmplId)
        {
            string sql = "delete from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId=" + _HeadID + " and tblpay_employeeheadmap.EmployeeId='" + EmplId + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }



        public void UpdateEmp(int EGridId, string _Surname, int _CatId, string _Pan, double  _Bank, double _gross, double _netpay, double _total, double _BasicPay, string _Comment)
        {
            string sql = " Update tblpay_employee SET tblpay_employee.Surname= '" + _Surname + "', tblpay_employee.Gross= " + _gross + ", tblpay_employee.Deduction= " + _total + ",tblpay_employee.AccNo=" + _Bank + ",tblpay_employee.NetAmt=" + _netpay + ",tblpay_employee.PAN='" + _Pan + "', tblpay_employee.PayrollType=" + _CatId + " , tblpay_employee.BasicPay=" + _BasicPay + ", tblpay_employee.Comment = '"+ _Comment+"' Where tblpay_employee.Id= " + EGridId + " ";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);

        }

        public void UpdateHead(string  EmplId, int Head, int Amt)
        {
            string sql = "Update tblpay_employeeheadmap SET tblpay_employeeheadmap.HeadAmount=" + Amt + " where tblpay_employeeheadmap.EmployeeId = '" + EmplId + "' and tblpay_employeeheadmap.HeadId=" + Head + " ";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }

        public bool IsPresentCategory(string _cat)
        {
            bool _Cat = true;
            string sql = "select * from tblpay_category where tblpay_category.CategoryName = '" + _cat + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (!m_MyReader.HasRows)
            {
                _Cat = false;
            }
            return _Cat;
        }

        public bool IsPresentHead(string _head)
        {
            bool _Head = true;
            string sql = "select * from tblpay_head where tblpay_head.HeadName = '" + _head + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (!m_MyReader.HasRows)
            {
                _Head = false;
            }
            return _Head;
            
        }

        public OdbcDataReader CheckFixed(int _HeadID)
        {
            string sql = "select * from tblpay_employeeheadmap where  tblpay_employeeheadmap.HeadId = " + _HeadID + "";
            return m_MysqlDb.ExecuteQuery(sql);
        }



        public DataSet GetEarnHeadOfCatId(int _PayCatId)
        {
            DataSet _NewEarn = new DataSet();
            string sql = "select DISTINCT tblpay_head.Id as HeadId,tblpay_head.HeadName, tblpay_head.Amount as HeadAmount, tblpay_head.DecreaseType from tblpay_head inner join tblpay_headcategorymap on tblpay_head.Id in(select tblpay_headcategorymap.HeadId from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId = " + _PayCatId + ") where tblpay_head.Type ='Earnings'";
            _NewEarn = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            if (_NewEarn.Tables[0].Rows.Count > 0)
            {

                DataTable dt = _NewEarn.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return _NewEarn;
        }

        public DataSet GetDedHeadOfCatId(int _PayCatId)
        {
            DataSet _NewDed = new DataSet();
            string sql = "select DISTINCT tblpay_head.Id as HeadId,tblpay_head.HeadName, tblpay_head.Amount as HeadAmount, tblpay_head.DecreaseType from tblpay_head inner join tblpay_headcategorymap on tblpay_head.Id in(select tblpay_headcategorymap.HeadId from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId = " + _PayCatId + ") where tblpay_head.Type ='Deductions'";
            _NewDed = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            if (_NewDed.Tables[0].Rows.Count > 0)
            {

                DataTable dt = _NewDed.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return _NewDed;
        }
        public void DeleteOldHead(int _HeadID, string  EmplId)
        {
            string sql = "delete from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId=" + _HeadID + " and tblpay_employeeheadmap.EmployeeId='" + EmplId + "' ";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);
        }

        public int GetTableMaxId(string _TableName, string _Field)
        {
            int Id = 0;
            string sql = "select max(" + _TableName + "." + _Field + ") from " + _TableName + "";

            m_MyReader = m_TransationDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                bool valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Id);
            }
           
            return Id;
        }

        public void DeleteCatHead(string  EmplId)
        {
            string sql = "delete from tblpay_employeeheadmap where  tblpay_employeeheadmap.EmployeeId='" + EmplId + "' ";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }



        public DataSet AddHeadToGrid(int _NewHeadId)
        {
            DataSet _NewHead = new DataSet();
            string sql = "select tblpay_head.Id as HeadId, tblpay_head.HeadName, tblpay_head.Amount as HeadAmount from tblpay_head where tblpay_head.Id= " + _NewHeadId + "";
            if (m_TransationDb != null)
            {
                _NewHead = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
               _NewHead =  m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (_NewHead.Tables[0].Rows.Count > 0)
            {

                DataTable dt = _NewHead.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return _NewHead;
        }

        public DataSet AddDedToGrid(int _NewDedId)
        {
            DataSet _NewHead = new DataSet();
            string sql = "select tblpay_head.Id as HeadId, tblpay_head.HeadName, tblpay_head.Amount as HeadAmount, tblpay_head.DecreaseType from tblpay_head where tblpay_head.Id= " + _NewDedId + "";
            if (m_TransationDb != null)
            {
                _NewHead = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                _NewHead = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (_NewHead.Tables[0].Rows.Count > 0)
            {

                DataTable dt = _NewHead.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return _NewHead;
        }

        public DataSet GetApprovedEmp()
        {
            DataSet Approved = new DataSet();
            string sql = "select tblpay_employee.Id, tblpay_employee.Surname, tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.Comment, tblpay_employee.NetAmt, tblpay_employee.EmpId from tblpay_employee where tblpay_employee.Approval=1 and tblpay_employee.Reject=0 and tblpay_employee.`Status`=1";
            if (m_TransationDb != null)
            {
                Approved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Approved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Approved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Approved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Approved;
           
        }

        public DataSet GetRejectededEmp()
        {
            DataSet Rejected = new DataSet();
            string sql = "select tblpay_employee.Id, tblpay_employee.Surname, tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.Comment, tblpay_employee.NetAmt, tblpay_employee.EmpId from tblpay_employee where  tblpay_employee.Reject=1 and tblpay_employee.`Status`=1";
            if (m_TransationDb != null)
            {
                Rejected = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Rejected = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Rejected.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Rejected.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Rejected;
        }

       

        public void UpdateEmpSal(int EmpGridId, bool status,string username,string employeename)
        {
            if (status)
            {
                string sql = "Update tblpay_employee SET tblpay_employee.Approval= 1 , tblpay_employee.Reject= 0 where tblpay_employee.Id = " + EmpGridId + " ";
                m_MysqlDb.ExecuteQuery(sql);
                m_DbLog.LogToDb(username, "Employee Salary Approval", "Payroll details of employee " + employeename + " is approved", 1);
            }
            else
            {
                string sql = "Update tblpay_employee SET tblpay_employee.Approval= 0 , tblpay_employee.Reject= 1 where tblpay_employee.Id = " + EmpGridId + " ";
                m_MysqlDb.ExecuteQuery(sql);
                m_DbLog.LogToDb(username, "Employee Salary Approval", "Payroll details of employee " + employeename + " is rejected", 1);
            }
            
        }

        public DataSet FillMonthDrp()
        {
            DataSet _DrpMonth = new DataSet();
            string sql = "select tblmonth.Id,tblmonth.Month from tblmonth";
            _DrpMonth = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpMonth;
        }



        public DataSet FillEmpDrp(int _CatId,int year,int month)
        {
            int Id = _CatId;
            int _PayRollTypeId=0;
            string _sql = "";
            bool _valid=true;
            DataSet EmpId_DS = null;
            DataSet EmpDetails_DS = null;
            EmpDetails_DS = new DataSet();
            DataTable dt;
            DataRow _dr;
            EmpDetails_DS.Tables.Add(new DataTable("Emp"));
            dt = EmpDetails_DS.Tables["Emp"];
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Surname");
            dt.Columns.Add("StaffId");
            dt.Columns.Add("JoiningDate");

            DataSet unresignedds = null;

            // jisha Get all emp ids from tblpay_employee (is APPROVED =1)
            // create a dataset for display details

            EmpId_DS = GetAllEMPId();
            // check if dataset is exist
            if (EmpId_DS != null && EmpId_DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow EmpDr in EmpId_DS.Tables[0].Rows)
                {
                    if (EpExistInSalConfiguration(EmpDr["EmpId"].ToString(), year, month, _CatId, out _valid))
                    {
                        if(_valid)
                        EmpDetails_DS=GetEmpDetails(EmpDetails_DS, EmpDr["EmpId"].ToString(), _CatId, 1 );
                    }
                    else
                    {
                        int status;
                        GetEmployeeStatus(EmpDr["EmpId"].ToString(), out status);
                        if (status == 1)
                        {
                            EmpDetails_DS = GetEmpDetails(EmpDetails_DS, EmpDr["EmpId"].ToString(), _CatId, 0);
                        }

                    }
                }
            }
                    if (EmpDetails_DS.Tables[0].Rows.Count > 0)
                    {                        
                        DataSet Empds = GetCorrectJoinees(EmpDetails_DS, year, month);
                        unresignedds = GetUnresigndata(Empds, year, month);
                    }
              
            
         
            return unresignedds;
        }

        private void GetEmployeeStatus(string EmpId, out int status)
        {
            status = 0;
            string sql = "";
            sql = "select Status from tblpay_employee where tblpay_employee.EmpId='"+EmpId+"'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                status = int.Parse(m_MyReader.GetValue(0).ToString());
            }
        }

        private DataSet GetEmpDetails(DataSet EmpDetails_DS, string EMPID, int _CatId, int status)
        {
            DataRow _dr;
            string _sql = "";
           
            if (status == 0)
            {
                _sql = "select  tblpay_employee.Surname,tblpay_employee.StaffId,DATE_FORMAT(tblview_staffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate  from tblpay_employee inner join tblview_staffdetails on tblview_staffdetails.UserId= tblpay_employee.StaffId where tblpay_employee.PayrollType=" + _CatId + "   and tblpay_employee.Approval = 1 and tblpay_employee.EmpId='" + EMPID + "'order by  tblpay_employee.Surname";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {

                        _dr = EmpDetails_DS.Tables["Emp"].NewRow();
                        _dr["EmpId"] = EMPID;
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString(); 
                        _dr["StaffId"] = m_MyReader.GetValue(1).ToString(); 
                        _dr["JoiningDate"] = m_MyReader.GetValue(2).ToString();
                        EmpDetails_DS.Tables["Emp"].Rows.Add(_dr);
                    }
                }
            }
            else if (status == 1)
            {
                _sql = "select  tblpay_employee.Surname,tblpay_employee.StaffId,DATE_FORMAT(tblview_staffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate  from tblpay_employee inner join tblview_staffdetails on tblview_staffdetails.UserId= tblpay_employee.StaffId where tblpay_employee.EmpId='" + EMPID + "' order by  tblpay_employee.Surname";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {

                        _dr = EmpDetails_DS.Tables["Emp"].NewRow();
                        _dr["EmpId"] = EMPID;
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString();
                        _dr["StaffId"] = m_MyReader.GetValue(1).ToString();
                        _dr["JoiningDate"] = m_MyReader.GetValue(2).ToString();
                        EmpDetails_DS.Tables["Emp"].Rows.Add(_dr);
                    }
                }
            }
            return EmpDetails_DS;

        }

        private bool EpExistInSalConfiguration(string _EMPID, int year, int month, int _CatId, out bool _valid)
        {
            int _payrolltype = 0;
            bool exist = false;
            _valid = false;
            string sql = "select  CatId from tblpay_empmonthlysalconfig where EmpId='" + _EMPID + "' and Year=" + year + " and MonthId=" + month + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            { 
                exist = true;
                _payrolltype = int.Parse(m_MyReader.GetValue(0).ToString());
                if (_CatId == _payrolltype)
                {
                    _valid = true;
                  
                }

            }
            else
            {

                exist = false;
            }
          
            return exist;
        }

        private DataSet GetAllEMPId()
        {
            DataSet EmpId_Ds = new DataSet();
            string sql = "";
            sql = "select distinct EmpId from tblpay_employee where tblpay_employee.Approval=1";
            EmpId_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return EmpId_Ds;
        }    

     

        private DataSet GetCorrectJoinees(DataSet EmployeeDs,int year,int month)
        {
            DateTime _resndate = System.DateTime.Now.Date;
            string _resignationdate = General.GerFormatedDatVal(_resndate);
            DataSet correctjoinees = new DataSet();
            DataTable dt;
            DataRow _dr;
            correctjoinees.Tables.Add(new DataTable("Emp"));
            dt = correctjoinees.Tables["Emp"];
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Surname");
            dt.Columns.Add("StaffId");

            if (EmployeeDs != null && EmployeeDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in EmployeeDs.Tables[0].Rows)
                {
                    DateTime joindate =General.GetDateTimeFromText(dr["JoiningDate"].ToString());
                    int monthid = joindate.Month;
                    int yearvalue = joindate.Year;
                    if (yearvalue < year)
                    {

                        _dr = correctjoinees.Tables["Emp"].NewRow();
                        _dr["EmpId"] = dr["EmpId"];
                        _dr["Surname"] = dr["Surname"];
                        _dr["StaffId"] = dr["StaffId"];

                        correctjoinees.Tables["Emp"].Rows.Add(_dr);
                    }
                    if (year == yearvalue)
                    {
                        if (month >= monthid)
                        {
                            _dr = correctjoinees.Tables["Emp"].NewRow();
                            _dr["EmpId"] = dr["EmpId"];
                            _dr["Surname"] = dr["Surname"];
                            _dr["StaffId"] = dr["StaffId"];

                            correctjoinees.Tables["Emp"].Rows.Add(_dr);
                        }
                    }

                }
            }
            return correctjoinees;

        }

        private DataSet GetUnresigndata(DataSet ds,int year,int month)
        {
            DateTime _resndate = System.DateTime.Now.Date;
            string _resignationdate = General.GerFormatedDatVal(_resndate);
            DataSet resds = new DataSet();
            DataTable dt;
            DataRow _dr;
            resds.Tables.Add(new DataTable("Emp"));
            dt = resds.Tables["Emp"];
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Surname");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sqlrs = "select  tbluserdetails_history.UserId, DATE_FORMAT(tbluserdetails_history.ResignDate,'%d/%m/%Y') from tbluserdetails_history where tbluserdetails_history.UserId =" + dr["StaffId"] + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sqlrs);
                    if (m_MyReader.HasRows)
                    {
                        DateTime resdate = General.GetDateTimeFromText(m_MyReader.GetValue(1).ToString());
                       int monthid= resdate.Month;
                       int yearvalue = resdate.Year;
                        while (m_MyReader.Read())
                        {
                            if (dr["StaffId"].ToString() == m_MyReader.GetValue(0).ToString())
                            {
                                if (year < yearvalue )
                                {
                                    
                                    //&& 
                                    _dr = resds.Tables["Emp"].NewRow();
                                    _dr["EmpId"] = dr["EmpId"];
                                    _dr["Surname"] = dr["Surname"];
                                    resds.Tables["Emp"].Rows.Add(_dr);

                                }
                                else if (year == yearvalue)
                                {
                                    if (month <= monthid)
                                    {
                                        _dr = resds.Tables["Emp"].NewRow();
                                        _dr["EmpId"] = dr["EmpId"];
                                        _dr["Surname"] = dr["Surname"];
                                        resds.Tables["Emp"].Rows.Add(_dr);

                                    }
                                }

                            }
                           
                        }
                    }
                    else
                    {
                        _dr = resds.Tables["Emp"].NewRow();
                        _dr["EmpId"] = dr["EmpId"];
                        _dr["Surname"] = dr["Surname"];
                        resds.Tables["Emp"].Rows.Add(_dr);
                    }
                }
            }
            return resds;
        }

        public DataSet FillAllEmp(int _Cat)
        {
            DataSet _DrpEmp = new DataSet();
            string sql = "select tblpay_employee.Id, tblpay_employee.Surname,tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.NetAmt, tblpay_employee.EmpId from tblpay_employee where tblpay_employee.EmpId in (select Distinct tblpay_employeeheadmap.EmployeeId from tblpay_employeeheadmap where tblpay_employeeheadmap.CategoryId = " + _Cat + ")and tblpay_employee.Approval = 1";
            _DrpEmp = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpEmp;
        }

        public OdbcDataReader FillEmp(string  _Emp, int _Cat)
        {
            string sql = "select tblpay_employee.Id, tblpay_employee.Surname, tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.NetAmt, tblpay_employee.EmpId, tblpay_employee.Configured,DATE_FORMAT(tblview_staffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate,tblpay_employee.`Status` from tblpay_employee inner join tblview_staffdetails on tblview_staffdetails.UserId= tblpay_employee.StaffId  where tblpay_employee.PayrollType=" + _Cat + "  and  tblpay_employee.Approval = 1 and tblpay_employee.EmpId='" + _Emp + "'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateMonthEmp(int Year,int MonthId, double _BasicPay, string  EmplId, double _gross, double _total, double _netpay, int _TotWorking, int _TotWorked, string Comment)
        {
            string sql = "Update tblpay_empmonthlysalconfig SET tblpay_empmonthlysalconfig.Year=" + Year + ", tblpay_empmonthlysalconfig.TotalGross=" + _gross + ", tblpay_empmonthlysalconfig.BasicPay = " + _BasicPay + " , tblpay_empmonthlysalconfig.NetPay=" + _netpay + ", tblpay_empmonthlysalconfig.TotalDeduction=" + _total + ",tblpay_empmonthlysalconfig.TotalWorking=" + _TotWorking + ",tblpay_empmonthlysalconfig.TotalWorked=" + _TotWorked + ",tblpay_empmonthlysalconfig.Comment = '" + Comment + "'  where tblpay_empmonthlysalconfig.MonthId = " + MonthId + " and tblpay_empmonthlysalconfig.EmpId = '" + EmplId + "' and tblpay_empmonthlysalconfig.`status`=1";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
            
        }

        public void DeleteMonthhead(string  EmplId, int MonthId, int Year)
        {
            string sql = "Delete from tblpay_monthempheadmap where tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId= " + MonthId + " and tblpay_monthempheadmap.EmpId = '" + EmplId + "'";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public void AddNewMonthHeadMap(int MonthId, string  EmplId, int Head, int _CatId, double Amt, int Year)
        {
            string sql = "insert into tblpay_monthempheadmap(Year,MonthId,EmpId,CatId,HeadId,HeadAmount) values("+Year+"," + MonthId + ",'" + EmplId + "', " + _CatId + ", " + Head + ", " + Amt + ")";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public void AddMonthEmpSal(int Yr, int _Month, double _BasicPay, string  EmpId, double Gross, double Deduction, double Netpay, int CatId, double advSal,int Towrkng,int towrkd)
        {
            string sql = "insert into tblpay_empmonthlysalconfig(Year, MonthId, BasicPay, EmpId,CatId,TotalGross,TotalDeduction,NetPay,AdvSal,TotalWorking,TotalWorked,status) values(" + Yr + "," + _Month + "," + _BasicPay + ",'" + EmpId + "', " + CatId + ", " + Gross + ", " + Deduction + ", " + Netpay + "," + advSal + ","+Towrkng+","+towrkd+",1)";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public bool NotEmpPresent(string  EmpId, int _Month,int year)
        {
            bool _Notpresent = true;
            string sql = "select * from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.MonthId=" + _Month + " and `year`=" + year + "";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
           
            if (m_MyReader.HasRows)
            {
                _Notpresent = false;
            }
            return _Notpresent;
            
        }

        public DataSet GetDefaultEmp(int _Cat, int year, int month)
        {
            DataSet EmpId_DS = null;
            EmpId_DS = GetAllEMPId();
            DataSet EmpDetails_DS = new DataSet();
            DataTable dt;
            DataRow dr;
            EmpDetails_DS.Tables.Add(new DataTable("Emp"));
            dt = EmpDetails_DS.Tables["Emp"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Surname");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Gross");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("NetAmt");
            dt.Columns.Add("Configured");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("JoiningDate");

            bool _valid = true;
            if (EmpId_DS != null && EmpId_DS.Tables[0].Rows.Count > 0)
            {
                if (EmpId_DS != null && EmpId_DS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow EmpDr in EmpId_DS.Tables[0].Rows)
                    {
                        if (EpExistInSalConfiguration(EmpDr["EmpId"].ToString(), year, month, _Cat, out _valid))
                        {
                            if (_valid)
                                EmpDetails_DS = GetEmpDetailsforDisplay(EmpDetails_DS, EmpDr["EmpId"].ToString(), _Cat, 1);
                        }
                        else
                        {
                            int Status;
                            GetEmployeeStatus(EmpDr["EmpId"].ToString(), out Status);
                            if (Status == 1)
                            {
                                EmpDetails_DS = GetEmpDetailsforDisplay(EmpDetails_DS, EmpDr["EmpId"].ToString(), _Cat, 0);
                            }

                        }
                    }
                }
            }

            return EmpDetails_DS;
        }

        private DataSet GetEmpDetailsforDisplay(DataSet EmpDetails_DS, string EMPID, int _Cat, int status)
        {
            DataRow _dr;
            string sql = "";         

            if (status == 0)
            {
                sql = "select tblpay_employee.Id, tblpay_employee.Surname,tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.NetAmt, tblpay_employee.Configured,DATE_FORMAT(tblview_staffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate from tblpay_employee inner join tblview_staffdetails on tblview_staffdetails.UserId= tblpay_employee.StaffId  where tblpay_employee.PayrollType=" + _Cat + "  and  tblpay_employee.Approval = 1 and tblpay_employee.EmpId='" + EMPID + "'";
                 m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {

                        _dr = EmpDetails_DS.Tables["Emp"].NewRow();
                        _dr["Id"] = m_MyReader.GetValue(0).ToString();
                        _dr["Surname"] = m_MyReader.GetValue(1).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(2).ToString();
                        _dr["Gross"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = m_MyReader.GetValue(4).ToString();
                        _dr["NetAmt"] = m_MyReader.GetValue(5).ToString();
                        _dr["EmpId"] = EMPID;
                        _dr["Configured"] = m_MyReader.GetValue(6).ToString();                        
                        _dr["JoiningDate"] = m_MyReader.GetValue(7).ToString();
                        EmpDetails_DS.Tables["Emp"].Rows.Add(_dr);
                    }
                }
            }
            else if (status == 1)
            {
                sql = "select tblpay_employee.Id, tblpay_employee.Surname,tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.NetAmt, tblpay_employee.Configured,DATE_FORMAT(tblview_staffdetails.JoiningDate,'%d/%m/%Y') as JoiningDate from tblpay_employee inner join tblview_staffdetails on tblview_staffdetails.UserId= tblpay_employee.StaffId  where tblpay_employee.EmpId='" + EMPID + "'";
                 m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {

                        _dr = EmpDetails_DS.Tables["Emp"].NewRow();
                        _dr["Id"] =m_MyReader.GetValue(0).ToString();
                        _dr["Surname"] = m_MyReader.GetValue(1).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(2).ToString();
                        _dr["Gross"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = m_MyReader.GetValue(4).ToString();
                        _dr["NetAmt"] = m_MyReader.GetValue(5).ToString();
                        _dr["EmpId"] = EMPID;
                        _dr["Configured"] = m_MyReader.GetValue(6).ToString();                        
                        _dr["JoiningDate"] = m_MyReader.GetValue(7).ToString();
                        EmpDetails_DS.Tables["Emp"].Rows.Add(_dr);
                    }
                }
            }
            return EmpDetails_DS;
            
        }

        public bool IsPresentInMonthly(int Year, int month,string  EmpId, out int id, out string surname,out double BasicPay, out double gross, out double Deduction, out double NetAmt, out int status, out int Approve, out int Reject, out double AdvSal,out int CatID)
        {
            bool Present = false;
            id = 0;
            surname = "";
            gross = 0;
            Deduction = 0;
            NetAmt = 0;
            BasicPay= 0;
            status = 0;
            Approve = 0;
            Reject = 0;
            AdvSal = 0.00;
            CatID = 0;
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname,tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross, tblpay_empmonthlysalconfig.TotalDeduction, tblpay_empmonthlysalconfig.NetPay, tblpay_empmonthlysalconfig.Configured, tblpay_empmonthlysalconfig.Approval, tblpay_empmonthlysalconfig.Reject, tblpay_empmonthlysalconfig.AdvSal,tblpay_empmonthlysalconfig.CatId from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_empmonthlysalconfig.EmpId = tblpay_employee.EmpId where tblpay_empmonthlysalconfig.EmpId='" + EmpId + "' and tblpay_empmonthlysalconfig.Year=" + Year + " and tblpay_empmonthlysalconfig.MonthId= " + month + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Present = true;
                while (m_MyReader.Read())
                {
                    id = int.Parse(m_MyReader.GetValue(0).ToString());
                    surname = m_MyReader.GetValue(1).ToString();
                    double.TryParse(m_MyReader.GetValue(2).ToString(), out BasicPay);
                    gross = double.Parse(m_MyReader.GetValue(3).ToString());
                    Deduction = double.Parse(m_MyReader.GetValue(4).ToString());
                    NetAmt = double.Parse(m_MyReader.GetValue(5).ToString());
                    status = int.Parse(m_MyReader.GetValue(6).ToString());
                    Approve = int.Parse(m_MyReader.GetValue(7).ToString());
                    Reject = int.Parse(m_MyReader.GetValue(8).ToString());
                    AdvSal = int.Parse(m_MyReader.GetValue(9).ToString());
                    CatID = int.Parse(m_MyReader.GetValue(10).ToString());
                }
            }
            return Present;
        }

        public bool IsEmpPresent(string  EmpId, int _Month)
        {
            bool _Notpresent = false;
            string sql = "select * from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.MonthId=" + _Month + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Notpresent = true;
            }
            return _Notpresent;
        }

        public void DeleteMonthEmp(string  EmpId, int _Month)
        {
            string sql = "Delete from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.MonthId= " + _Month + " and tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "'";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader LoadMonthEmp(string  EmplId, int MonthId,int year)
        {
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname,tblpay_empmonthlysalconfig.BasicPay, tblpay_employee.PAN, tblpay_employee.AccNo, tblpay_empmonthlysalconfig.TotalWorking, tblpay_empmonthlysalconfig.TotalWorked, tblpay_empmonthlysalconfig.Comment from tblpay_empmonthlysalconfig inner join  tblpay_employee on tblpay_empmonthlysalconfig.EmpId = tblpay_employee.EmpId where tblpay_empmonthlysalconfig.EmpId = '" + EmplId + "' and tblpay_empmonthlysalconfig.MonthId = " + MonthId + " and `year`=" + year + "";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetMonthEarningEmp(string  EmplId, int MonthId, int Year)
        {
            DataSet _NewEarn = new DataSet();
            string sql = "select DISTINCT tblpay_monthempheadmap.Id,tblpay_monthempheadmap.HeadId, tblpay_head.HeadName, tblpay_monthempheadmap.HeadAmount, tblpay_head.DecreaseType from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Earnings') and tblpay_monthempheadmap.EmpId = '" + EmplId + "' and tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId=" + MonthId + "";
            _NewEarn = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_NewEarn.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewEarn.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewEarn;
        }

        public DataSet GetMonthDeductionEmp(string EmplId, int MonthId)
        {
            DataSet _NewEarn = new DataSet();
            string sql = "select DISTINCT tblpay_monthempheadmap.Id,tblpay_monthempheadmap.HeadId, tblpay_head.HeadName, tblpay_monthempheadmap.HeadAmount, tblpay_head.DecreaseType from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Deductions') and tblpay_monthempheadmap.EmpId = '" + EmplId + "'  and tblpay_monthempheadmap.MonthId=" + MonthId + "";
            _NewEarn = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (_NewEarn.Tables[0].Rows.Count > 0)
            {

                DataTable dtAgent = _NewEarn.Tables[0];
                DataView dataView = new DataView(dtAgent);
            }
            return _NewEarn;
        }

        public DataSet GetNonApprovedMonthEmp(int MonthId, int Year)
        {
            DataSet Approved = new DataSet();
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname,tblpay_empmonthlysalconfig.Advanceamount, tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross as Gross, tblpay_empmonthlysalconfig.TotalDeduction as Deduction , tblpay_empmonthlysalconfig.NetPay as NetAmt, tblpay_empmonthlysalconfig.Comment, tblpay_empmonthlysalconfig.EmpId, tblpay_empmonthlysalconfig.AdvSal from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId = tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.Approval= 0 and tblpay_empmonthlysalconfig.Reject= 0 and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            if (m_TransationDb != null)
            {
                Approved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Approved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Approved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Approved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Approved;
        }

        public DataSet GetApprovedMonthEmp(int MonthId, int Year)
        {
            DataSet Approved = new DataSet();
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname,tblpay_empmonthlysalconfig.Advanceamount, tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross as Gross, tblpay_empmonthlysalconfig.TotalDeduction as Deduction , tblpay_empmonthlysalconfig.NetPay as NetAmt, tblpay_empmonthlysalconfig.Comment, tblpay_empmonthlysalconfig.EmpId, tblpay_empmonthlysalconfig.AdvSal from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId = tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.Approval=1 and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " "; 
            if (m_TransationDb != null)
            {
                Approved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Approved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Approved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Approved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Approved;
        }

        public void UpdateEmpMonthSal(string EmpId, int MonthId, bool Approve, int Year,string username,string empname)
        {
            if (Approve)
            {
                string sql = "Update tblpay_empmonthlysalconfig SET tblpay_empmonthlysalconfig.Approval= 1 , tblpay_empmonthlysalconfig.Reject= 0 where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + "";
                m_MysqlDb.ExecuteQuery(sql);
                m_DbLog.LogToDb(username, "Monthly Salary Approval", "Payroll details of employee " + empname + " is approved", 1);
            }
            else
            {
                string sql = "Update tblpay_empmonthlysalconfig SET tblpay_empmonthlysalconfig.Approval= 0 , tblpay_empmonthlysalconfig.Reject= 1 where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + " ";
                m_MysqlDb.ExecuteQuery(sql);
                m_DbLog.LogToDb(username, "Employee Salary Approval", "Payroll details of employee " + empname + " is rejected", 1);
            }
        }

        public DataSet GetNonPayedMonthEmp(int MonthId,  int Year)
        {
            DataSet Approved = new DataSet();
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname, tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross as Gross, tblpay_empmonthlysalconfig.TotalDeduction as Deduction , tblpay_empmonthlysalconfig.NetPay as NetAmt, tblpay_empmonthlysalconfig.EmpId, tblpay_empmonthlysalconfig.AdvSal,tblpay_empmonthlysalconfig.Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId = tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.Payed=0 and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " and tblpay_empmonthlysalconfig.Approval = 1";
            if (m_TransationDb != null)
            {
                Approved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Approved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Approved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Approved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Approved;
        }

        public DataSet GetPayedMonthEmp(int MonthId, int Year)
        {
            DataSet Approved = new DataSet();
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname, tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross as Gross, tblpay_empmonthlysalconfig.TotalDeduction as Deduction , tblpay_empmonthlysalconfig.NetPay as NetAmt, tblpay_empmonthlysalconfig.EmpId, tblpay_empmonthlysalconfig.AdvSal,tblpay_empmonthlysalconfig.Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId = tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.Payed=1 and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            if (m_TransationDb != null)
            {
                Approved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Approved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Approved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Approved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Approved;
        }

        public void UpdateEmpMonthSalPayed(string  EmpId, int MonthId, int Year)
        {
            string sql = "Update tblpay_empmonthlysalconfig SET tblpay_empmonthlysalconfig.Payed = 1 where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and  tblpay_empmonthlysalconfig.MonthId = " + MonthId + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader CheckPayed(string  EmplId, int MonthId,int year)
        {

            string sql = "select tblpay_empmonthlysalconfig.Payed, tblpay_empmonthlysalconfig.Approval from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmplId + "' and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " and `year`="+year+"";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void AddConfig(string Config, int Val)
        {
            
            string sql = " insert into tblpay_employercontribution(ConfigName,Value1)Values ('"+ Config +"',"+ Val+" )";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public void AddAdvSal(string EmplId, double Amount, double Percent)
        {
            string sql = "";            
            sql = "Update tblpay_employee set tblpay_employee.Advance = " + Amount + ", tblpay_employee.DeductionPercent = " + Percent + " , tblpay_employee.Balance = " + Amount * 2 + " where tblpay_employee.EmpId = '" + EmplId + "'";
            m_MysqlDb.ExecuteQuery(sql);
           
        }

        public DataSet FillYearDrp()
        {
            DataSet _DrpYear = new DataSet();
            string sql = "select Distinct tblpay_empmonthlysalconfig.Year from tblpay_empmonthlysalconfig order by Year";
            _DrpYear = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return _DrpYear;
        }

        public OdbcDataReader GetPfId()
        {
            string sql = "select tblpay_head.Id from tblpay_head where tblpay_head.HeadName = 'PF'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader GetHeadEmp(int HeadId, int Month, int Year)
        {
            string sql = "select tblpay_monthempheadmap.EmpId, tblpay_employee.Surname, tblpay_monthempheadmap.HeadAmount,tblpay_employee.Designation  from tblpay_monthempheadmap inner join tblpay_employee on tblpay_employee.EmpId= tblpay_monthempheadmap.EmpId  where tblpay_monthempheadmap.HeadId = " + HeadId + " and tblpay_monthempheadmap.MonthId = " + Month + "  and tblpay_monthempheadmap.`year`="+Year+"";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader GetEmprContribution()
        {
            string sql = "select tblpay_employercontribution.Value1 from tblpay_employercontribution where tblpay_employercontribution.ConfigName = 'PF'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader GetTDSId()
        {
            string sql = "select tblpay_head.Id from tblpay_head where tblpay_head.HeadName = 'TDS'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader GetAdvanceSal(string  EmpId)
        {
            string sql = " select tblpay_employee.Advance, tblpay_employee.DeductionPercent,tblpay_employee.Balance from tblpay_employee where tblpay_employee.EmpId = '" + EmpId + "' ";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateAdvanceSalary(string  EmpId, double Bal)
        {
            string sql = " Update  tblpay_employee set tblpay_employee.Balance = " + Bal + " where tblpay_employee.EmpId = '" + EmpId + "' ";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public bool IsUpdatePresentHead(string _HeadName, int _Id)
        {
            bool _Head = true;
            string sql = "select * from tblpay_head where tblpay_head.HeadName = '" + _HeadName + "' and tblpay_head.Id <>" + _Id + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (!m_MyReader.HasRows)
            {
                _Head = false;
            }
            return _Head;
        }

        public bool IsUpdatePresentCategory(string _CatName, int _Id)
        {
            bool _Cat = true;
            string sql = "select * from tblpay_category where tblpay_category.CategoryName = '" + _CatName + "' and tblpay_category.Id <> " + _Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (!m_MyReader.HasRows)
            {
                _Cat = false;
            }
            return _Cat;
        }

        public OdbcDataReader CheckStatus(string EmplId)
        {
            string sql = "select tblpay_employee.Approval from tblpay_employee where tblpay_employee.EmpId = '" + EmplId + "'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetNonApprovedEmp()
        {
            DataSet NonApproved = new DataSet();
            string sql = "select tblpay_employee.Id, tblpay_employee.Surname, tblpay_employee.BasicPay, tblpay_employee.Gross, tblpay_employee.Deduction, tblpay_employee.Comment, tblpay_employee.NetAmt, tblpay_employee.EmpId from tblpay_employee where tblpay_employee.Approval=0 and tblpay_employee.Reject=0 and tblpay_employee.`Status`=1";
            if (m_TransationDb != null)
            {
                NonApproved = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                NonApproved = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (NonApproved.Tables[0].Rows.Count > 0)
            {

                DataTable dt = NonApproved.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return NonApproved;
        }

        public OdbcDataReader GetComments(string  Emp_Id)
        {
            string sql = "select tblpay_employee.Comment from tblpay_employee where tblpay_employee.EmpId = '" + Emp_Id + "'";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateComment(string  Emp_Id, string UpdateComment)
        {
            string sql = " Update  tblpay_employee set tblpay_employee.Comment = '" + UpdateComment + "' where tblpay_employee.EmpId = '" + Emp_Id + "' ";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetRejectedMonthEmp(int MonthId, int Year)
        {
            DataSet Rejected = new DataSet();
            string sql = "select tblpay_empmonthlysalconfig.Id, tblpay_employee.Surname, tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalGross as Gross, tblpay_empmonthlysalconfig.TotalDeduction as Deduction , tblpay_empmonthlysalconfig.NetPay as NetAmt, tblpay_empmonthlysalconfig.Comment, tblpay_empmonthlysalconfig.EmpId,tblpay_empmonthlysalconfig.Advanceamount, tblpay_empmonthlysalconfig.AdvSal from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId = tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.Reject=1 and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " ";
            if (m_TransationDb != null)
            {
                Rejected = m_TransationDb.ExecuteQueryReturnDataSet(sql);
            }
            else
            {
                Rejected = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            }
            if (Rejected.Tables[0].Rows.Count > 0)
            {

                DataTable dt = Rejected.Tables[0];
                DataView dataView = new DataView(dt);
            }
            return Rejected;
        }

        public OdbcDataReader GetMonthlyComments(string  Emp_Id, int MonthId,int Year)
        {
            string sql = "select tblpay_empmonthlysalconfig.Comment from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + Emp_Id + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + "";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateMonthlyComment(string  Emp_Id, string UpdateComment, int MonthId, int Year)
        {
            string sql = " Update  tblpay_empmonthlysalconfig set tblpay_empmonthlysalconfig.Comment = '" + UpdateComment + "' where tblpay_empmonthlysalconfig.EmpId = '" + Emp_Id + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + " ";
            m_MysqlDb.ExecuteQuery(sql);
        }

        //public void AddMonthEmpSal(int _Month, double BP, int EmpId, double Gross, double Deduction, double Netpay, int CatId)
        //{
        //    string sql = "insert into tblpay_empmonthlysalconfig(MonthId, BasicPay, EmpId,CatId,TotalGross,TotalDeduction,NetPay) values(" + _Month + "," + BP + "," + EmpId + ", " + CatId + ", " + Gross + ", " + Deduction + ", " + Netpay + ")";
        //    if (m_TransationDb != null)
        //    {
        //        m_TransationDb.ExecuteQuery(sql);
        //    }
        //    else
        //    {
        //        m_MysqlDb.ExecuteQuery(sql);
        //    }
        //}
        public string PaySlipHeader()
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

        public string PaySlipEarnings(string  EmpId, int MonthId,int Year)
        {
                    string m_PaySlipEarnings = "";
                    string BasicPay = "BASIC PAY";
                    string sql = "select DISTINCT tblpay_head.HeadName from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Earnings') and tblpay_monthempheadmap.EmpId = '" + EmpId + "' and tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);

                    m_PaySlipEarnings += "<table width=\"100%\"><tr>";
                    m_PaySlipEarnings += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + BasicPay + "";
                    m_PaySlipEarnings += "</td></tr>";
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {
                            m_PaySlipEarnings += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\">" + m_MyReader.GetValue(0).ToString().ToUpper() + "";
                            m_PaySlipEarnings += "</td></tr>";
                           
                        }
                        
                    }
                    m_PaySlipEarnings += "</table>";
                    m_MyReader.Close();
                    sql = "select tblpay_empmonthlysalconfig.Advanceamount from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.`Year`=" + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " and tblpay_empmonthlysalconfig.EmpId='" + EmpId + "'";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        double advamount =double.Parse( m_MyReader.GetValue(0).ToString());
                        if (advamount != 0)
                        {
                            m_PaySlipEarnings += "<table width=\"100%\"><tr>";
                            m_PaySlipEarnings += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">ADVANCE AMOUNT";
                            m_PaySlipEarnings += "</td></tr>";                           
                            m_PaySlipEarnings += "</table>";
                        }
                    }
                
                return m_PaySlipEarnings;
            
        }

        public string PaySlipEarnAmount(string EmpId, int MonthId, int Year)
        {


            string m_PaySlipEarnAmount = "";
            string sql;
            OdbcDataReader myreader = null;
            OdbcDataReader mybasicpayreader = null;

            string _sqlbasic = "select tblpay_empmonthlysalconfig.BasicPay from tblpay_empmonthlysalconfig  where tblpay_empmonthlysalconfig.EmpId= '" + EmpId + "' and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            mybasicpayreader = m_MysqlDb.ExecuteQuery(_sqlbasic);
            if (mybasicpayreader.HasRows)
            {
                m_PaySlipEarnAmount += "<table width=\"70%\"><tr>";
                m_PaySlipEarnAmount += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + mybasicpayreader.GetValue(0).ToString() + "";
                m_PaySlipEarnAmount += "</td></tr>";


                string _sql = "select tblpay_head.Id from tblpay_head where tblpay_head.Type='Earnings'";
                myreader = m_MysqlDb.ExecuteQuery(_sql);
                if (myreader.HasRows)
                {
                    while (myreader.Read())
                    {
                        int headId = int.Parse(myreader.GetValue(0).ToString());
                        sql = "select DISTINCT tblpay_monthempheadmap.HeadAmount from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id inner join tblpay_empmonthlysalconfig on  tblpay_empmonthlysalconfig.EmpId = tblpay_monthempheadmap.EmpId where tblpay_monthempheadmap.HeadId =" + headId + " and tblpay_monthempheadmap.EmpId = '" + EmpId + "' and tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId= " + MonthId + "";
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {

                            while (m_MyReader.Read())
                            {
                                m_PaySlipEarnAmount += "<tr><td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + m_MyReader.GetValue(0).ToString() + "";
                                m_PaySlipEarnAmount += "</td></tr>";

                            }

                        }
                    }
                }
            }

            m_PaySlipEarnAmount += "</table>";
            m_MyReader.Close();

            sql = "select tblpay_empmonthlysalconfig.Advanceamount from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.`Year`=" + Year + " and tblpay_empmonthlysalconfig.MonthId=" + MonthId + " and tblpay_empmonthlysalconfig.EmpId='" + EmpId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                double advamount = double.Parse(m_MyReader.GetValue(0).ToString());
                if (advamount > 0)
                {
                    m_PaySlipEarnAmount += "<table width=\"70%\"><tr>";
                    m_PaySlipEarnAmount += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + advamount + "";
                    m_PaySlipEarnAmount += "</td></tr>";
                    m_PaySlipEarnAmount += "</table>";
                }

            }

            return m_PaySlipEarnAmount;

        }
        public string PaySlipGross(string  EmpId, int MonthId, int Year)
        {
            string m_PaySlipGross = "";
            string sql = "select tblpay_empmonthlysalconfig.TotalGross from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {

                            m_PaySlipGross += "<table width=\"70%\"><tr>";
                            m_PaySlipGross += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + m_MyReader.GetValue(0).ToString() + "";
                            m_PaySlipGross += "</td></tr>";
                            m_PaySlipGross += "</table>";
                        }
                    }
                    m_MyReader.Close();
                
                return m_PaySlipGross;
            
        }

        public string PaySlipDeductions(string EmpId, int MonthId, int Year)
        {

            string m_PaySlipDeduction = "";
            string sql = "select DISTINCT  tblpay_head.HeadName from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.HeadId in (select tblpay_head.Id from tblpay_head where tblpay_head.Type='Deductions') and tblpay_monthempheadmap.EmpId = '" + EmpId + "' and tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId= " + MonthId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    m_PaySlipDeduction += "<table width=\"100%\"><tr>";
                    m_PaySlipDeduction += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + m_MyReader.GetValue(0).ToString().ToUpper() + "";
                    m_PaySlipDeduction += "</td></tr>";
                    m_PaySlipDeduction += "</table>";
                }
            }
            m_MyReader.Close();

            sql = "select AdvSal from tblpay_empmonthlysalconfig where EmpId =  '" + EmpId + "' and Year = " + Year + " and MonthId = " + MonthId + "";

            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                double Advamnt =double.Parse(m_MyReader.GetValue(0).ToString());
                if (Advamnt > 0)
                {

                    m_PaySlipDeduction += "<table width=\"100%\"><tr>";
                    m_PaySlipDeduction += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">ADVANCE SALARY DEDUCTION";
                    m_PaySlipDeduction += "</td></tr>";
                    m_PaySlipDeduction += "</table>";
                }
            }
            m_MyReader.Close();


            return m_PaySlipDeduction;

        }

        public string PaySlipDedAmount(string  EmpId, int MonthId, int Year)
        {

            string m_PaySlipDedAmount = "";
            string sql;
            OdbcDataReader myreader = null;
            string _sql = "select tblpay_head.Id from tblpay_head where tblpay_head.Type='Deductions'";
            myreader = m_MysqlDb.ExecuteQuery(_sql);
            if (myreader.HasRows)
            {
                while (myreader.Read())
                {
                    int headId = int.Parse(myreader.GetValue(0).ToString());
                    sql = "select DISTINCT  tblpay_monthempheadmap.HeadAmount from tblpay_monthempheadmap inner join tblpay_head on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.HeadId =" + headId + " and tblpay_monthempheadmap.EmpId = '" + EmpId + "' and tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {

                            m_PaySlipDedAmount += "<table width=\"70%\"><tr>";
                            m_PaySlipDedAmount += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + double.Parse(m_MyReader.GetValue(0).ToString()) + "";
                            m_PaySlipDedAmount += "</td></tr>";
                            m_PaySlipDedAmount += "</table>";
                        }
                    }
                }
            }
                    m_MyReader.Close();
                    sql = "select AdvSal from tblpay_empmonthlysalconfig where EmpId =  '"+EmpId+"' and Year = "+Year+" and MonthId = "+MonthId+"";

                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        double Advamnt =double.Parse(m_MyReader.GetValue(0).ToString());
                        if (Advamnt > 0)
                        {
                            m_MyReader.Read();
                            m_PaySlipDedAmount += "<table width=\"70%\"><tr>";
                            m_PaySlipDedAmount += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + double.Parse(m_MyReader.GetValue(0).ToString()) + "";
                            m_PaySlipDedAmount += "</td></tr>";
                            m_PaySlipDedAmount += "</table>";
                        }
                    }
                    m_MyReader.Close();
                return m_PaySlipDedAmount;
            
        }
        public string PaySlipTotalDeduction(string  EmpId, int MonthId, int Year)
        {

            string m_PaySlipTotalDed = "";

            string sql = "select tblpay_empmonthlysalconfig.TotalDeduction + tblpay_empmonthlysalconfig.AdvSal from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {

                            m_PaySlipTotalDed += "<table width=\"70%\"><tr>";
                            m_PaySlipTotalDed += "<td align=\"right\" style=\"font-size:12px;vertical-align:top\">" + m_MyReader.GetValue(0).ToString() + "";
                            m_PaySlipTotalDed += "</td></tr>";
                            m_PaySlipTotalDed += "</table>";
                        }
                    }
                    m_MyReader.Close();
                
                return m_PaySlipTotalDed;
            
        }
        public string PaySlipNetPay(string  EmpId, int MonthId, int Year)
        {


            string m_PaySlipNetPay = "";

            string sql = "select tblpay_empmonthlysalconfig.NetPay from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {

                            m_PaySlipNetPay += "<table width=\"100%\"><tr>";
                            m_PaySlipNetPay += "<td style=\"font-size:12px;text-align:center;vertical-align:top\">" + m_MyReader.GetValue(0).ToString() + "";
                            m_PaySlipNetPay += "</td></tr>";
                            m_PaySlipNetPay += "</table>";
                        }
                    }
                    m_MyReader.Close();
                
                return m_PaySlipNetPay;
            
        }

        public string PaySlipComments(string  EmpId, int MonthId, int Year)
        {

            string m_PaySlipComment = "";

            string sql = "select tblpay_empmonthlysalconfig.Comment from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId= " + MonthId + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        while (m_MyReader.Read())
                        {

                            m_PaySlipComment += "<table width=\"100%\"><tr>";
                            m_PaySlipComment += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + m_MyReader.GetValue(0).ToString() + "";
                            m_PaySlipComment += "</td></tr>";
                            m_PaySlipComment += "</table>";
                        }
                    }
                    m_MyReader.Close();
                
                return m_PaySlipComment;
            
        }
        public string PaySlipIdLabel()
        {

            string m_PaySlipIdLabel = "";
            string EmpCode = "EMPLOYEE CODE";
            string loc = "LOCATION";
            string PaidDays = "PAID DAYS";
            string BankAcc = "BANK A/C NO.";

            m_PaySlipIdLabel += "<table width=\"100%\"><tr>";
            m_PaySlipIdLabel += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + EmpCode + "";
            m_PaySlipIdLabel += "</td></tr>";
            m_PaySlipIdLabel += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\">" + loc + "";
            m_PaySlipIdLabel += "</td></tr>";
            m_PaySlipIdLabel += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\">" + PaidDays + "";
            m_PaySlipIdLabel += "</td></tr>";
            m_PaySlipIdLabel += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\">" + BankAcc + "";
            m_PaySlipIdLabel += "</td></tr>";
            m_PaySlipIdLabel += "</table>";

                    return m_PaySlipIdLabel;

        }


        public string PaySlipDetailId(string EmpId, int MonthId,int Year)
        {

            string m_PaySlipDetailId = "";

            string sql = "select tblpay_empmonthlysalconfig.EmpId, tblpay_employee.PresentAddress, tblpay_empmonthlysalconfig.TotalWorked, tblpay_employee.AccNo from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId =  tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    m_PaySlipDetailId += "<table width=\"100%\"><tr>";
                    m_PaySlipDetailId += "<td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(0).ToString() + "";
                    m_PaySlipDetailId += "</b></td></tr>";
                    m_PaySlipDetailId += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(1).ToString() + "";
                    m_PaySlipDetailId += "</b></td></tr>";
                    m_PaySlipDetailId += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(2).ToString() + "";
                    m_PaySlipDetailId += "</b></td></tr>";
                    m_PaySlipDetailId += "<tr><td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(3).ToString() + "";
                    m_PaySlipDetailId += "</b></td></tr>";
                    m_PaySlipDetailId += "</table>";
                }
            }
            m_MyReader.Close();

            return m_PaySlipDetailId;

        }

        public string PaySlipNameLabel()
        {

            string m_PaySlipNameLabel = "";
            string EmpName = "NAME";
            string Designation = "DESIGNATION";
            string PancardNum = "PAN NUMBER";
             
            m_PaySlipNameLabel += "<table width=\"100%\"><tr>";
            m_PaySlipNameLabel += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + EmpName + "";
            m_PaySlipNameLabel += "</td></tr>";
            m_PaySlipNameLabel += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + Designation + "";
            m_PaySlipNameLabel += "</td></tr>";
            m_PaySlipNameLabel += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + PancardNum + "";
            m_PaySlipNameLabel += "</td></tr>";

            m_PaySlipNameLabel += "</table>";


            return m_PaySlipNameLabel;

        }

        public string PaySlipDetailName(string  EmpId, int MonthId)
        {

            string m_PaySlipDetailName = "";

            string sql = "select tblpay_employee.Surname,tblpay_employee.Designation,tblpay_employee.PAN from tblpay_employee  where tblpay_employee.EmpId = '" + EmpId + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    m_PaySlipDetailName += "<table width=\"100%\"><tr>";
                    m_PaySlipDetailName += "<td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(0).ToString().ToUpper() + "";
                    m_PaySlipDetailName += "</b></td></tr>";
                    m_PaySlipDetailName += "<table width=\"100%\"><tr>";
                    m_PaySlipDetailName += "<td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(1).ToString().ToUpper() + "";
                    m_PaySlipDetailName += "</b></td></tr>";
                    m_PaySlipDetailName += "<table width=\"100%\"><tr>";
                    m_PaySlipDetailName += "<td style=\"font-size:12px;text-align:left;vertical-align:top\"><b>" + m_MyReader.GetValue(2).ToString() + "";
                    m_PaySlipDetailName += "</b></td></tr>";
                    m_PaySlipDetailName += "</table>";
                }
            }
            m_MyReader.Close();

            return m_PaySlipDetailName;

        }

        public string PaySlipRupeeInWords(string  EmpId, int MonthId, int Year)
        {

            string m_PaySlipRupeeInWords = "";
            double Netpay = 0;
            string sql = "select tblpay_empmonthlysalconfig.NetPay from tblpay_empmonthlysalconfig  where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId = " + MonthId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    Netpay = double.Parse(m_MyReader.GetValue(0).ToString());
                   
                }
            }
            m_MyReader.Close();

            string Words = Convert_Number_To_Words((long)Netpay).ToUpper();
            m_PaySlipRupeeInWords += "<table width=\"100%\"><tr>";
            m_PaySlipRupeeInWords += "<td style=\"font-size:12px;text-align:left;vertical-align:top\">" + Words + "";
            m_PaySlipRupeeInWords += "</td></tr>";

            m_PaySlipRupeeInWords += "</table>";
            return m_PaySlipRupeeInWords;

        }

        public string GetSalSlip(string EmpId, int MonthId, int Year)
        {
            StringBuilder paySlip = new StringBuilder();
            paySlip.Append("<table width=\"100%\" style=\"border-style:solid; border-color:Black\" border=\"1\">");
            paySlip.Append("<tr><td style=\"font-weight:bold;  text-align:center; border-bottom-style:solid \" colspan=\"4\">");
            paySlip.Append(PaySlipHeader());
            paySlip.Append(" </td> </tr>");


              paySlip.Append("<tr>");
              paySlip.Append("<td style=\"text-align:center; width:262px;vertical-align:top; border-style:hidden\">");
              paySlip.Append(PaySlipIdLabel());
              paySlip.Append("</td>");
              paySlip.Append("<td style=\"  text-align:center; width:262px;vertical-align:top; border-style:hidden\">");
              paySlip.Append(PaySlipDetailId(EmpId, MonthId, Year));
              paySlip.Append("</td>");
              paySlip.Append("<td style=\"  text-align:center; width:262px;vertical-align:top; border-style:hidden\">");
              paySlip.Append(PaySlipNameLabel());
              paySlip.Append("</td>");
              paySlip.Append("<td style=\"  text-align:center; width:262px;vertical-align:top; border-style:hidden\">");
              paySlip.Append(PaySlipDetailName(EmpId, MonthId));
              paySlip.Append("</td>");
              paySlip.Append("</tr>");


              string[] months = new string[] { "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
              paySlip.Append("<tr>");
              paySlip.Append("<td style=\" font-weight:bold;  text-align:center; border-bottom-style:solid; border-top-style:solid \" colspan=\"4\">PAYSLIP FOR THE MONTH OF "+ months[MonthId-1]+" - "+Year+" </td>");

              paySlip.Append("</tr>");
            
              paySlip.Append("<tr>");
              paySlip.Append("<td style=\"  text-align:left; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\">EARNINGS</td>");
              paySlip.Append("<td style=\"  text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\">AMOUNT&nbsp;</td>");
              paySlip.Append("<td style=\"  text-align:left; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\">DEDUCTIONS</td>");
              paySlip.Append(" <td style=\"  text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\">AMOUNT</td>");
              paySlip.Append("</tr>");

              paySlip.Append("<tr>");
              paySlip.Append("<td style=\"text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid;\"> <br />");
              paySlip.Append(PaySlipEarnings(EmpId,MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("<td style=\"text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid;\"><br />");
              paySlip.Append(PaySlipEarnAmount(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append(" <td style=\"text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\"><br />");
              paySlip.Append(PaySlipDeductions(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append(" <td style=\"text-align:center; width:262px; border-right-style:solid; vertical-align:top; border-bottom-style:solid\"><br />");
              paySlip.Append(PaySlipDedAmount(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("</tr>");

              paySlip.Append("<tr>");
              paySlip.Append(" <td style=\"  text-align:left; width:262px; border-right-style:solid; border-bottom-style:solid\">GROSS</td>");
              paySlip.Append(" <td style=\"  text-align:center; width:262px; border-right-style:solid; border-bottom-style:solid\">");
              paySlip.Append(PaySlipGross(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("  <td style=\"  text-align:left; width:262px; border-right-style:solid; border-bottom-style:solid\">TOTAL DEDUCTIONS</td>");
              paySlip.Append(" <td style=\"  text-align:center; width:262px; border-right-style:solid; border-bottom-style:solid\">");
              paySlip.Append(PaySlipTotalDeduction(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("</tr>");

              paySlip.Append("<tr>");
              paySlip.Append("<td style=\"text-align:left; width:262px; font-weight:bold; border-style:hidden\">NET PAY<br />  </td>");
              paySlip.Append(" <td style=\"text-align:center; width:262px; border-style:hidden\">");
              paySlip.Append(PaySlipNetPay(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("<td style=\"text-align:center; width:262px; border-style:hidden\"></td>");
              paySlip.Append("<td style=\"text-align:center; width:262px; border-style:hidden\"></td>");
              paySlip.Append("</tr>");

              paySlip.Append("<tr>");
              paySlip.Append("<td style=\"text-align:left; width:262px; font-weight:bold; border-style:hidden\">RUPEES IN WORDS:<br /> <br /> </td>");
              paySlip.Append("<td  colspan=\"3\" style=\"text-align:left ; border-style:hidden\">");
              paySlip.Append(PaySlipRupeeInWords(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("</tr>");

              paySlip.Append("<tr>");
              paySlip.Append(" <td style=\"  font-weight:bold;border-top-style:solid; border-collapse:inherit \">REMARKS:<br /><br /></td>");
              paySlip.Append("<td colspan=\"3\" style=\"text-align:left; border-top-style:solid;  \">");
              paySlip.Append(PaySlipComments(EmpId, MonthId,Year));
              paySlip.Append("</td>");
              paySlip.Append("</tr>");

              paySlip.Append("</table>");
              return paySlip.ToString();
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


        public bool NotCatFixed(int _CatID)
        {
            bool NotFixed = true;
            string sql = " select * from tblpay_employee where tblpay_employee.PayrollType = " + _CatID + "";
            if (m_TransationDb != null)
            {
              m_MyReader= m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
               m_MyReader=  m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                NotFixed = false;
            }
            return NotFixed;
        }

        public bool CheckHeadFixed(int _HeadID)
        {
            bool Fixed = false;
            string sql = "select * from tblpay_employeeheadmap where  tblpay_employeeheadmap.HeadId = " + _HeadID + "";
            if (m_TransationDb != null)
            {
               m_MyReader=  m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
               m_MyReader= m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                Fixed = true;
            }
            return Fixed;
        }

        public bool CheckHeadFixedForCategory(int _HeadID)
        {
            bool Exist = false;
            string sql = "select * from tblpay_headcategorymap where  tblpay_headcategorymap.HeadId = " + _HeadID + "";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                Exist = true;
            }
            return Exist;

        }

        public OdbcDataReader CheckHeadCatPresent(int _HeadId, int _CatId)
        {
            string sql = "select * from tblpay_headcategorymap where  tblpay_headcategorymap.HeadId = " + _HeadId + " and tblpay_headcategorymap.CategoryId = " + _CatId + "";
            if (m_TransationDb != null)
            {
                return m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                return m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public OdbcDataReader GetEmpFrmMap(int _Id)
        {
            string sql = "select tblpay_employee.EmpId from tblpay_employee where tblpay_employee.PayrollType="+_Id+"";
           // string sql = "select Distinct tblpay_employeeheadmap.EmployeeId from tblpay_employeeheadmap where  tblpay_employeeheadmap.CategoryId = " + _Id + "";
            if (m_TransationDb != null)
            {
                return m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                return m_MysqlDb.ExecuteQuery(sql);
            }
           
        }

        public OdbcDataReader EmployeeAlreadymapped(int _Id,string EmpId,int headID)
        {
            string _sql = " select Distinct tblpay_employeeheadmap.EmployeeId from tblpay_employeeheadmap where  tblpay_employeeheadmap.CategoryId = " + _Id + " and EmployeeId='" + EmpId + "' and HeadId="+headID+"";
            if (m_TransationDb != null)
            {
                return m_TransationDb.ExecuteQuery(_sql);
            }
            else
            {
                return m_MysqlDb.ExecuteQuery(_sql);
            }
        }

        public void InsertEmpHeadMap(int p, int _Id, string  _EmpId)
        {
            string sql = "insert into tblpay_employeeheadmap(EmployeeId,CategoryId,HeadId) values('" + _EmpId + "'," + _Id + ", " + p + ")";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public void AddMonthEmpSal(int Year, int MonthId, double _BasicPay, string EmplId, double _gross, double _total, double _netpay, int Cat, int _TotWorking, int _TotWorked, string Comment, double advsal)
        {

            string sql = "insert into tblpay_empmonthlysalconfig(Year, MonthId, BasicPay, EmpId,CatId,TotalGross,TotalDeduction,NetPay,TotalWorking,TotalWorked,Comment,AdvSal,status) values(" + Year + "," + MonthId + "," + _BasicPay + ",'" + EmplId + "', " + Cat + ", " + _gross + ", " + _total + ", " + _netpay + "," + _TotWorking + "," + _TotWorked + ",'" + Comment + "'," + advsal + ",1)";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }


        }

        public void DeleteEmpMonthMap(string  EmpId, int _Month, int Year)
        {
            string sql = " delete from tblpay_monthempheadmap where tblpay_monthempheadmap.Year= " + Year + " and tblpay_monthempheadmap.MonthId=" + _Month + " and tblpay_monthempheadmap.EmpId = '" + EmpId + "'";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public OdbcDataReader GetHfrmHCatMap(int CatId, string  EmpId)
        {
            string sql = "select Distinct tblpay_employeeheadmap.HeadId,tblpay_employeeheadmap.HeadAmount  from tblpay_employeeheadmap where  tblpay_employeeheadmap.CategoryId = " + CatId + " and tblpay_employeeheadmap.EmployeeId = '" + EmpId + "'";
            if (m_TransationDb != null)
            {
                return m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                return m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public void InsertMonthEmpHeadMap(int Year,int _Month, string  EmpId, int CatId, int _HeadId, double _HeadAmount)
        {
            string sql = "insert into tblpay_monthempheadmap(Year,MonthId, EmpId,CatId,HeadId,HeadAmount) values("+Year+"," + _Month + ",'" + EmpId + "'," + CatId + ", " + _HeadId + "," + _HeadAmount + ")";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public bool NotEmpPayed(string EmpId, int _Month, int Year)
        {
            bool NotPayed = true;
            string sql = "select tblpay_empmonthlysalconfig.Payed, tblpay_empmonthlysalconfig.Approval from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId = '" + EmpId + "' and tblpay_empmonthlysalconfig.Year= " + Year + " and tblpay_empmonthlysalconfig.MonthId=" + _Month + "";
            m_MyReader =  m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int Payed = int.Parse(m_MyReader.GetValue(0).ToString());
                int Approve = int.Parse(m_MyReader.GetValue(1).ToString());
                if (Payed == 1 || Approve == 1)
                {
                    NotPayed = false;
                }
            }
            return NotPayed;
        }

        public double GetAdvanceSalary(string empid)
        {

            double bal = 0;
            double advance = 0;
            string sql = "select balance, advance from tblpay_employee where EmpId = '" + empid + "'";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    bal = double.Parse(m_MyReader.GetValue(0).ToString());
                    advance = double.Parse(m_MyReader.GetValue(1).ToString());
                    if ((bal > 0) && (advance >= bal))
                    {
                        sql = "select advance*deductionpercent/100 from tblpay_employee where EmpId = '" + empid + "'";
                        if (m_TransationDb != null)
                        {
                            m_MyReader = m_TransationDb.ExecuteQuery(sql);
                        }
                        else
                        {
                            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        }

                        if (m_MyReader.HasRows)
                        {
                            while (m_MyReader.Read())
                            {
                                advance = double.Parse(m_MyReader.GetValue(0).ToString());
                            }
                        }
                    }
                    else
                    {
                        advance = 0;
                    }
                }
            }
            return advance;

        }

   

        public double GetAdvanceSalaryDeduction(string empid, int month, int year)
        {
            double advance = 0;
            string sql = "select AdvSal,Advanceamount from tblpay_empmonthlysalconfig where EmpId = '" + empid + "' and tblpay_empmonthlysalconfig.Year=" + year + " and tblpay_empmonthlysalconfig.MonthId=" + month + "";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                advance = double.Parse(m_MyReader.GetValue(0).ToString());
            }           
            return advance;
        }

        public double GetAdvanceSalaryOnMonth(string empid, int month, int year)
        {
            double advance = 0;
            string sql = "select Advanceamount from tblpay_empmonthlysalconfig where EmpId = '" + empid + "' and tblpay_empmonthlysalconfig.Year=" + year + " and tblpay_empmonthlysalconfig.MonthId=" + month + "";
            if (m_TransationDb != null)
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {

                double.TryParse(m_MyReader.GetValue(0).ToString(), out advance);
            }
            else
            {
                advance = 0;
            }
            return advance;
        }


        public void DeleteMappedHead(int HeadId,int _catId)
        { 
            string _sql="";
            _sql = "Delete from tblpay_headcategorymap where tblpay_headcategorymap.HeadId=" + HeadId + " and tblpay_headcategorymap.CategoryId="+_catId+"";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(_sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(_sql);
            }

            _sql = " Delete from tblpay_employeeheadmap where tblpay_employeeheadmap.HeadId=" + HeadId + " and tblpay_employeeheadmap.CategoryId=" + _catId + "";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(_sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(_sql);
            }
        }
         public void AddAdvanceToMonthlyConfig(int MonthId,string EmpID,double Percent,double Amount)
        {
            string sql = "Update tblpay_empmonthlysalconfig set tblpay_empmonthlysalconfig.Advanceamount=" + Amount + ",tblpay_empmonthlysalconfig.Deductionpercent=" + Percent + " where tblpay_empmonthlysalconfig.EmpId='" + EmpID + "' and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }
        


        public OdbcDataReader GetAdvancesalOfMonth(int MonthId,string EmpId)
        {
            string sql = "select tblpay_empmonthlysalconfig.Advanceamount,tblpay_empmonthlysalconfig.Deductionpercent,AdvSal from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId='" + EmpId + "' and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public double GetAdvancesalOfThisMonth(int MonthId, string EmplId)
        {
            double advamount=0.0;
            string sql = "select tblpay_empmonthlysalconfig.Advanceamount,tblpay_empmonthlysalconfig.Deductionpercent from tblpay_empmonthlysalconfig where tblpay_empmonthlysalconfig.EmpId='" + EmplId + "' and tblpay_empmonthlysalconfig.MonthId=" + MonthId + "";
            if (m_TransationDb != null)
            {
              m_MyReader=  m_TransationDb.ExecuteQuery(sql);
              if (m_MyReader.HasRows)
              {
                  advamount =double.Parse(m_MyReader.GetValue(0).ToString());
              }

            }
            else
            {
               m_MyReader= m_MysqlDb.ExecuteQuery(sql);
               if (m_MyReader.HasRows)
               {
                   advamount = double.Parse(m_MyReader.GetValue(0).ToString());
               }
            }
            return advamount;
        }



        public void GetAdvanceDetails(string EmplId, int MonthId, int Year, out double AdvDeduction, out double advnceamnt)
        {
            AdvDeduction = 0;
            advnceamnt = 0;

            string sql = "";
            sql = "select AdvSal,Advanceamount from tblpay_empmonthlysalconfig where Year="+Year+" and MonthId="+MonthId+" and EmpId='"+EmplId+"' and status=1";

            if (m_TransationDb != null)
            {
                m_MyReader= m_TransationDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    AdvDeduction =double.Parse(m_MyReader.GetValue(0).ToString());
                    advnceamnt = double.Parse(m_MyReader.GetValue(1).ToString());

                }
            }
            else
            {
                m_MyReader=m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    AdvDeduction = double.Parse(m_MyReader.GetValue(0).ToString());
                    advnceamnt = double.Parse(m_MyReader.GetValue(1).ToString());

                }
            }
        }

        public void UpdateAdvDeduction(double advdedction, string EmplId, int MonthId, int Year)
        {
            string sql = "";
            sql = "Update tblpay_empmonthlysalconfig Set AdvSal=" + advdedction + " where Year="+Year+" and  MonthId="+MonthId+" and  EmpId='"+EmplId+"'";
            if (m_TransationDb != null)
            {
                m_TransationDb.ExecuteQuery(sql);
            }
            else
            {
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public DataSet GenerateMonthlyPayrollReport(int year, int FromMonthId, int ToMonthId, int PayrollType, int _payrollHeadId)
        {          
            DataSet MonthlyPayrollReport_ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            MonthlyPayrollReport_ds.Tables.Add(new DataTable("MonthlyReport"));
            dt = MonthlyPayrollReport_ds.Tables["MonthlyReport"];
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Surname");
            dt.Columns.Add("PresentAddress");
            dt.Columns.Add("BasicPay");
            dt.Columns.Add("Earnings");
            dt.Columns.Add("Deduction");
            dt.Columns.Add("TotalGross");
            dt.Columns.Add("NetPay");
            dt.Columns.Add("Advanceamount");
            dt.Columns.Add("AdvSal");

           // DataSet MonthPayroll_Ds = new DataSet();
            string sql = "";
            double totalded=0.0;
            if (PayrollType == 0 && ((_payrollHeadId == 0) || (_payrollHeadId ==- 1)))
            {
                sql = "SELECT   Surname,tblpay_employee.EmpId,PresentAddress,tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalDeduction,tblpay_empmonthlysalconfig.TotalGross,tblpay_empmonthlysalconfig.NetPay,AdvSal,Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId= tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.`status`=1 and tblpay_empmonthlysalconfig.`Year`=" + year + " AND tblpay_empmonthlysalconfig.`MonthId` between " + FromMonthId + " and " + ToMonthId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        double total = 0.0;
                        GetEarnings(year, FromMonthId, ToMonthId, PayrollType, _payrollHeadId, m_MyReader.GetValue(1).ToString(), out total,out totalded);
                        _dr = MonthlyPayrollReport_ds.Tables["MonthlyReport"].NewRow();
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString();
                        _dr["EmpId"] = m_MyReader.GetValue(1).ToString();
                        _dr["PresentAddress"] = m_MyReader.GetValue(2).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = m_MyReader.GetValue(4).ToString();
                        _dr["TotalGross"] = m_MyReader.GetValue(5).ToString();
                        _dr["NetPay"] = m_MyReader.GetValue(6).ToString();
                        _dr["AdvSal"] = m_MyReader.GetValue(7).ToString();
                        _dr["Advanceamount"] = m_MyReader.GetValue(8).ToString();
                        _dr["Earnings"] = total;
                        MonthlyPayrollReport_ds.Tables["MonthlyReport"].Rows.Add(_dr);
                    }
                }
            }
            else if (PayrollType > 0 && ((_payrollHeadId == 0) || (_payrollHeadId == -1)))
            {
                sql = "SELECT   Surname,tblpay_employee.EmpId,PresentAddress,tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalDeduction,tblpay_empmonthlysalconfig.TotalGross,tblpay_empmonthlysalconfig.NetPay,AdvSal,Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId= tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.`status`=1 and tblpay_empmonthlysalconfig.`Year`=" + year + " AND tblpay_empmonthlysalconfig.`MonthId` between " + FromMonthId + " and " + ToMonthId + " and tblpay_empmonthlysalconfig.CatId="+PayrollType+"";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        double total = 0.0;
                        GetEarnings(year, FromMonthId, ToMonthId, PayrollType, _payrollHeadId, m_MyReader.GetValue(1).ToString(), out total, out totalded);
                        _dr = MonthlyPayrollReport_ds.Tables["MonthlyReport"].NewRow();
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString();
                        _dr["EmpId"] = m_MyReader.GetValue(1).ToString();
                        _dr["PresentAddress"] = m_MyReader.GetValue(2).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = m_MyReader.GetValue(4).ToString();
                        _dr["TotalGross"] = m_MyReader.GetValue(5).ToString();
                        _dr["NetPay"] = m_MyReader.GetValue(6).ToString();
                        _dr["AdvSal"] = m_MyReader.GetValue(7).ToString();
                        _dr["Advanceamount"] = m_MyReader.GetValue(8).ToString();
                        _dr["Earnings"] = total;
                        MonthlyPayrollReport_ds.Tables["MonthlyReport"].Rows.Add(_dr);
                    }
                }
            }
            else if (PayrollType == 0 && _payrollHeadId > 0)
            {
                sql = "SELECT   Surname,tblpay_employee.EmpId,PresentAddress,tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalDeduction,tblpay_empmonthlysalconfig.TotalGross,tblpay_empmonthlysalconfig.NetPay,AdvSal,Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId= tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.`status`=1 and tblpay_empmonthlysalconfig.`Year`=" + year + " AND tblpay_empmonthlysalconfig.`MonthId` between " + FromMonthId + " and " + ToMonthId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        double total = 0.0;
                        GetEarnings(year, FromMonthId, ToMonthId, PayrollType, _payrollHeadId, m_MyReader.GetValue(1).ToString(), out total, out totalded);
                        _dr = MonthlyPayrollReport_ds.Tables["MonthlyReport"].NewRow();
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString();
                        _dr["EmpId"] = m_MyReader.GetValue(1).ToString();
                        _dr["PresentAddress"] = m_MyReader.GetValue(2).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = totalded;
                        _dr["TotalGross"] = m_MyReader.GetValue(5).ToString();
                        _dr["NetPay"] = m_MyReader.GetValue(6).ToString();
                        _dr["AdvSal"] = m_MyReader.GetValue(7).ToString();
                        _dr["Advanceamount"] = m_MyReader.GetValue(8).ToString();
                        _dr["Earnings"] = total;
                        MonthlyPayrollReport_ds.Tables["MonthlyReport"].Rows.Add(_dr);
                    }
                }
            }
            else if (PayrollType > 0 && _payrollHeadId > 0)
            {
                sql = "SELECT   Surname,tblpay_employee.EmpId,PresentAddress,tblpay_empmonthlysalconfig.BasicPay, tblpay_empmonthlysalconfig.TotalDeduction,tblpay_empmonthlysalconfig.TotalGross,tblpay_empmonthlysalconfig.NetPay,AdvSal,Advanceamount from tblpay_empmonthlysalconfig inner join tblpay_employee on tblpay_employee.EmpId= tblpay_empmonthlysalconfig.EmpId where tblpay_empmonthlysalconfig.`status`=1 and tblpay_empmonthlysalconfig.`Year`=" + year + " AND tblpay_empmonthlysalconfig.`MonthId` between " + FromMonthId + " and " + ToMonthId + " and tblpay_empmonthlysalconfig.CatId=" + PayrollType + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        double total = 0.0;
                        GetEarnings(year, FromMonthId, ToMonthId, PayrollType, _payrollHeadId, m_MyReader.GetValue(1).ToString(), out total, out totalded);
                        _dr = MonthlyPayrollReport_ds.Tables["MonthlyReport"].NewRow();
                        _dr["Surname"] = m_MyReader.GetValue(0).ToString();
                        _dr["EmpId"] = m_MyReader.GetValue(1).ToString();
                        _dr["PresentAddress"] = m_MyReader.GetValue(2).ToString();
                        _dr["BasicPay"] = m_MyReader.GetValue(3).ToString();
                        _dr["Deduction"] = totalded;
                        _dr["TotalGross"] = m_MyReader.GetValue(5).ToString();
                        _dr["NetPay"] = m_MyReader.GetValue(6).ToString();
                        _dr["AdvSal"] = m_MyReader.GetValue(7).ToString();
                        _dr["Advanceamount"] = m_MyReader.GetValue(8).ToString();
                        _dr["Earnings"] = total;
                        MonthlyPayrollReport_ds.Tables["MonthlyReport"].Rows.Add(_dr);
                    }
                }
            }
           

            return MonthlyPayrollReport_ds;
           
        }

        private void GetEarnings(int year, int FromMonthId, int ToMonthId, int PayrollType, int _payrollHeadId,string EmpId,out double total,out double totalded)
        {
            totalded = 0.0;
            OdbcDataReader m_earningsreader = null;
            double headamt = 0.0;
            total=0.0;  
            string sql = "";           
            if (_payrollHeadId == 0)
            {
                sql = "select distinct tblpay_monthempheadmap.HeadAmount, tblpay_head.HeadName from tblpay_head inner join tblpay_monthempheadmap on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.EmpId='" + EmpId + "' and tblpay_monthempheadmap.`Year`=" + year + " and tblpay_monthempheadmap.MonthId between " + FromMonthId + " and " + ToMonthId + " and  tblpay_head.`Type`='Earnings'";
                m_earningsreader = m_MysqlDb.ExecuteQuery(sql);
                if (m_earningsreader.HasRows)
                {
                    while (m_earningsreader.Read())
                    {
                        double.TryParse(m_earningsreader.GetValue(0).ToString(), out headamt);
                        total = total + headamt;
                    }
                }
            }
            else
            {
                sql = "select distinct tblpay_monthempheadmap.HeadAmount, tblpay_head.HeadName, tblpay_head.`Type` from tblpay_head inner join tblpay_monthempheadmap on tblpay_monthempheadmap.HeadId= tblpay_head.Id where tblpay_monthempheadmap.EmpId='" + EmpId + "' and tblpay_monthempheadmap.`Year`=" + year + " and tblpay_monthempheadmap.MonthId between " + FromMonthId + " and " + ToMonthId + " and tblpay_monthempheadmap.HeadId=" + _payrollHeadId + "";                
                m_earningsreader = m_MysqlDb.ExecuteQuery(sql);
                if (m_earningsreader.HasRows)
                {
                    string Type = m_earningsreader.GetValue(2).ToString();
                    if (Type == "Earnings")
                    {
                        while (m_earningsreader.Read())
                        {
                            double.TryParse(m_earningsreader.GetValue(0).ToString(), out headamt);
                            total = total + headamt;
                        }
                    }
                    else if (Type == "Deductions")
                    {
                        while (m_earningsreader.Read())
                        {
                            double.TryParse(m_earningsreader.GetValue(0).ToString(), out headamt);
                            totalded = totalded + headamt;
                        }
                    }
                }
            }
       

        }

       

        public string GetEmpId(int staffId,int monthId,int Year)
        {
            OdbcDataReader m_empreader = null;
            string EmpId = "";
            string sql = "";
            string _sql = "";
            sql = "Select EmpId from tblpay_employee where StaffId="+staffId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                
                _sql = "select Id from tblpay_empmonthlysalconfig where Year=" + Year + " and MonthId=" + monthId + " and EmpId='" + m_MyReader.GetValue(0).ToString() + "'";
                m_empreader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_empreader.HasRows)
                {
                    EmpId = m_MyReader.GetValue(0).ToString();
                }
                
            }
            return EmpId;
        }

        public int GetRoleId(int staffId)
        {
            int RoleId = 0;
            string sql = "";
            sql = "select tbluser.RoleId from tbluser where tbluser.Id="+staffId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                RoleId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return RoleId;
        }

        public OdbcDataReader GetHeadName(int payrollType)
        {
            string sql="";
           
            if (payrollType > 0)
            {

                sql = " select Id,HeadName from tblpay_head where tblpay_head.id in(select tblpay_headcategorymap.HeadId from tblpay_headcategorymap where tblpay_headcategorymap.CategoryId="+payrollType+")";
                return m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                sql = "select Id,HeadName from tblpay_head";
                return m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public void UpdatePayEmployee(int _Id)
        {

          
            string sql = "", catsql = "",headAmountsql = "";
            OdbcDataReader HeadAmountReader = null;
            OdbcDataReader EmpReader = null;

            sql = " UPDATE tblpay_employee AS t inner JOIN (select Id,BasicPay from tblpay_category )  AS m ON t.PayrollType = m.Id SET t.BasicPay = m.BasicPay, t.Gross=m.BasicPay , t.NetAmt=m.BasicPay, t.Deduction=0 where t.PayrollType = " + _Id + "";
            m_MyReader = m_TransationDb.ExecuteQuery(sql);


            catsql = "select DISTINCT tblpay_employeeheadmap.EmployeeId from tblpay_employeeheadmap where tblpay_employeeheadmap.CategoryId="+_Id+"";
            EmpReader = m_TransationDb.ExecuteQuery(catsql);
            if (EmpReader.HasRows)
            {
                while (EmpReader.Read())
                {
                    double Amount = 0;
                    headAmountsql = "select sum(tblpay_employeeheadmap.HeadAmount) from tblpay_employeeheadmap WHERE tblpay_employeeheadmap.HeadId in(select tblpay_head.Id from tblpay_head where tblpay_head.Type ='Earnings')and tblpay_employeeheadmap.EmployeeId='" + EmpReader.GetValue(0).ToString() + "'";
                    HeadAmountReader = m_TransationDb.ExecuteQuery(headAmountsql);
                    if (HeadAmountReader.HasRows)
                    {
                        double.TryParse(HeadAmountReader.GetValue(0).ToString(), out Amount);
                        sql = "  Update tblpay_employee   SET tblpay_employee.Gross = tblpay_employee.Gross + " + Amount + " where tblpay_employee.PayrollType = " + _Id + " and  tblpay_employee.EmpId='" + EmpReader.GetValue(0).ToString() + "'";
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                        //UPDATE tblpay_employee AS t inner JOIN (select Id,BasicPay from tblpay_category )  AS m ON t.PayrollType = m.Id SET t.Gross = m.BasicPay, t.Gross=m.BasicPay , t.NetAmt=m.BasicPay, t.Deduction=0 where t.PayrollType = " + _Id + "";
                    }

                    headAmountsql = "select sum(tblpay_employeeheadmap.HeadAmount) from tblpay_employeeheadmap WHERE tblpay_employeeheadmap.HeadId in(select tblpay_head.Id from tblpay_head where tblpay_head.Type ='Deductions')and tblpay_employeeheadmap.EmployeeId='" + EmpReader.GetValue(0).ToString() + "'";
                    HeadAmountReader = m_TransationDb.ExecuteQuery(headAmountsql);
                    if (HeadAmountReader.HasRows)
                    {
                        Amount = 0;
                        double.TryParse(HeadAmountReader.GetValue(0).ToString(), out Amount);
                        sql = " Update tblpay_employee SET tblpay_employee.NetAmt = tblpay_employee.Gross-" + Amount + ",tblpay_employee.Deduction=" + Amount + " where tblpay_employee.PayrollType = " + _Id + " and  tblpay_employee.EmpId='" + EmpReader.GetValue(0).ToString() + "'";
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                        //UPDATE tblpay_employee AS t inner JOIN (select Id,BasicPay from tblpay_category )  AS m ON t.PayrollType = m.Id SET t.Gross = m.BasicPay, t.Gross=m.BasicPay , t.NetAmt=m.BasicPay, t.Deduction=0 where t.PayrollType = " + _Id + "";
                    }
                    else
                    {
                        sql = " Update tblpay_employee SET tblpay_employee.NetAmt = tblpay_employee.Gross  where tblpay_employee.PayrollType = " + _Id + "  and  tblpay_employee.EmpId='" + EmpReader.GetValue(0).ToString() + "'";
                        m_MyReader = m_TransationDb.ExecuteQuery(sql);
                    }

                }
            }
            
        }
    }
}
