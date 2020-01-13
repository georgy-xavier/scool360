using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
namespace WinBase
{
    public class LibraryManagClass : KnowinGen
    {

        public MysqlClass m_MysqlDb;
        private OdbcDataReader m_MyReader = null;
        private DataSet m_Dataset = null;
        private string m_LibMenuStr;
        private string m_SubLibMenuStr;
        public LibraryManagClass(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
            m_LibMenuStr = "";
            m_SubLibMenuStr = "";
        }

        ~LibraryManagClass()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            } if (m_MyReader != null)
            {
                m_MyReader = null;

            }

        }

        public string GetLibraryMangMenuString(int _roleid)
        {

            string _MenuStr;
            if (m_LibMenuStr == "")
            {


                _MenuStr = "<ul><li><a href=\"LlbraryHome.aspx\">Library Home</a></li>";

                string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=22 AND tblaction.ActionType='Link' order by tblaction.Order";

                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    while (m_MyReader.Read())
                    {


                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
                m_MyReader.Close();
                m_LibMenuStr = _MenuStr;


            }
            else
            {
                _MenuStr = m_LibMenuStr;
            }

            return _MenuStr;

        }
        public int AddBookMaster(string _Bookname, string _auther, string _Publisher, int _year, string _edition, int _type, int _catagory, string _count, double _Price, string Barcode, int Barcode_Type)
        {
            int _BookId = -1;
            string _Now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = " insert into tblbookmaster(BookName,Author,Publisher,Year,Edition,TypeId,CatagoryId,CreatedDate,Cost,Barcode,BarcodeType) values ('" + _Bookname + "','" + _auther + "','" + _Publisher + "'," + _year + ",'" + _edition + "'," + _type + "," + _catagory + ", '" + _Now + "'," + _Price + ",'" + Barcode + "','" + Barcode_Type + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            sql = "select Id from tblbookmaster where BookName='" + _Bookname + "' and Author='" + _auther + "' and Edition= '" + _edition + "' and CreatedDate='" + _Now + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _BookId = int.Parse(m_MyReader.GetValue(0).ToString());
            }

            m_MyReader.Close();

            return _BookId;
        }

        public int GetlastBookId()
        {
            int _BookId;
            string sql = "select tblbooks.Id from tblbooks order by tblbooks.Id desc";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _BookId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            else
            {
                _BookId = 0;
            }
            m_MyReader.Close();
            return _BookId;
        }

        public bool AutoBookId()
        {
            bool _valid = false;
            int _value;
            string sql = "select Value1 from tbllibconfig where ConfigName='AutoBookId'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _value = int.Parse(m_MyReader.GetValue(0).ToString());
                if (_value == 1)
                {
                    _valid = true;
                }
            }
            m_MyReader.Close();
            return _valid;
        }



        public int AddBooks(int B_Id,int BookId, string _BookNo, int rack, string Barcode)
        {
            int Id = -1;
            string sql = "insert into tblbooks(Id,BookId,RackId,BookNo,Barcode)values(" + B_Id + "," + BookId + "," + rack + ",'" + _BookNo + "','" + Barcode + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            Id = 1;
            return Id;
        }

        public bool UserExists(string _UserId, int _UserType, int _Searchtype)
        {

            bool _valid = false;
            if (_UserType == 1 && _Searchtype == 0)
            {
                string sql = "select tblstudent.AdmitionNo from tblstudent where tblstudent.AdmitionNo='" + _UserId + "' and tblstudent.`Status`=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            else if (_UserType == 1 && _Searchtype == 1)
            {
                string sql = "select tblstudent.StudentName from tblstudent where tblstudent.StudentName='" + _UserId + "' and tblstudent.`Status`=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }

            else if (_UserType == 2 && _Searchtype == 0)
            {
                string sql = "select tbluser.UserName from tbluser where tbluser.UserName='" + _UserId + "' and tbluser.`Status`=1 and tbluser.RoleId <> 1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            else if (_UserType == 2 && _Searchtype == 1)
            {
                string sql = "select tbluser.SurName from tbluser where tbluser.SurName='" + _UserId + "' and tbluser.`Status`=1 and tbluser.RoleId <> 1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        //public void AddUser(string _UserId, string _UserName, string _Desc, int _usertype, string _classDept)
        //{
        //    string sql = "insert into tblbookusermaster(UserId,UserName,UserType,Class,`Desc`) values ('" + _UserId + "','" + _UserName + "'," + _usertype + ",'" + _classDept + "','" + _Desc + "')";
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //}

        public bool IsbookExists(string _BookNo)
        {
            bool _valid = false;
            string sql = "select tblbooks.Id from tblbooks where tblbooks.BookNo='" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            return _valid;
        }

        public bool BookIdExists(int _Book_Id)
        {
            bool _valid = true;
            string sql = "select tblbooks.Id from tblbooks where tblbooks.BookNo=" + _Book_Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            return _valid;

        }

        public void IssueBook(string _UserId, string _BookId, int _UserType)
        {
            string _Now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into tblbookissue(BookNo,UserId,DateOfIssue,UserType) values ('" + _BookId + "','" + _UserId + "','" + _Now + "'," + _UserType + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void Book_Book(string _UserId, string _BookId, int _UserType)
        {
            string _Now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into tblbookbooking(UserId,bookId,DateOfBooking,UserType) values ('" + _UserId + "','" + _BookId + "','" + _Now + "'," + _UserType + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public string GetTheCurrentStatus(string _BookNo)
        {
            string _status = "None";
            string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo='" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _status = "Issued";
            }
            sql = "select tblbookbooking.Id from tblbookbooking where tblbookbooking.bookId='" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _status = "Booked";
            }
            return _status;
        }

        public bool Bookssued(string _BookNo, string _UserId)
        {
            bool _valid = false;
            string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo ='" + _BookNo + "' and tblbookissue.UserId='" + _UserId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            return _valid;
        }

        public int BookAlreadyissued(string _BookNo)
        {
            int _valid = 0;
            string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo ='" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = 1;
            }
            return _valid;
        }

        public int BookAlreadBooked(string _BookNo)
        {
            int _valid = 0;
            string sql = "select tblbookbooking.Id from tblbookbooking where tblbookbooking.bookId ='" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = 1;
            }
            return _valid;
        }

        /*  public bool BookBooked(string _BookNo, string _UserId)
          {

              bool _valid = false;
              string sql = "select tblbookbooking.Id from tblbookbooking where tblbookbooking.bookId ='" + _BookNo + "'and tblbookbooking.UserId='" + _UserId + "'";
              m_MyReader = m_MysqlDb.ExecuteQuery(sql);
              if (m_MyReader.HasRows)
              {
                  _valid = true;
              }
              return _valid;
          }*/

        public string GetTheCurrentViewGridStatus(string _BookNo, string _UserId, int _UserType)
        {
            string _status = "None";
            string sql = "";
            //select tblbookissue.Id from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId  where tblbookissue.BookNo='456' and tblstudent.AdmitionNo='A2' and tblbookissue.UserType=1
            if (_UserType == 1)
            {
                sql = "select tblbookissue.Id from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookNo + "' and tblstudent.AdmitionNo='" + _UserId + "' and tblbookissue.UserType=" + _UserType + "";
            }
            //sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo='" + _BookNo + "' and tblbookissue.UserId='" + _UserId + "'";
            else if (_UserType == 2)
            {
                sql = "select tblbookissue.Id from tblbookissue inner join tbluser on tbluser.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tblbookissue.UserType=" + _UserType + "";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _status = "Issued";

            }
            if (_UserType == 1)
            {
                sql = "select tblbookbooking.Id from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "' and tblstudent.AdmitionNo='" + _UserId + "' and tblbookbooking.UserType=1";
            }
            else if (_UserType == 2)
            {
                sql = "select tblbookbooking.Id from tblbookbooking  inner join tbluser on tbluser.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tblbookbooking.UserType=2";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _status = "Booked";
            }

            return _status;
        }


        public string GetTheCurrentViewGridDate(string _BookNo, string _UserId, int _UserType)
        {
            //DateTime _date;
            string _date1 = "";
            string sql = "";
            if (_UserType == 1)
            {
                sql = "select tblbookissue.DateOfIssue from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId where tblbookissue.BookNo='" + _BookNo + "' and tblbookissue.UserType=" + _UserType + " and tblstudent.AdmitionNo='" + _UserId + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _date1 = m_MyReader.GetValue(0).ToString();
                    //_date1 = _date.ToString();
                }
                sql = "select tblbookbooking.DateOfBooking from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "' and tblbookbooking.UserType=" + _UserType + " and tblstudent.AdmitionNo='" + _UserId + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _date1 = m_MyReader.GetValue(0).ToString();
                    //_date1 = _date.ToString();
                }
            }
            else if (_UserType == 2)
            {
                sql = "select tblbookissue.DateOfIssue from tblbookissue inner join tbluser on tbluser.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tblbookissue.UserType=" + _UserType + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _date1 = m_MyReader.GetValue(0).ToString();
                    //_date1 = _date.ToString();
                }
                sql = "select tblbookbooking.DateOfBooking from tblbookbooking  inner join tbluser on tbluser.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tblbookbooking.UserType=" + _UserType + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _date1 = m_MyReader.GetValue(0).ToString();
                    //_date1 = _date.ToString();
                }
            }

            return _date1;
        }

        public bool BookIssued(string _bookid)
        {
            bool _valid = false;
            string sql = "select tblbookissue.BookNo from tblbookissue where tblbookissue.BookNo='" + _bookid + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            return _valid;

        }

        public double GetFine(string _BookId,int type)
        {
            double days;
            double _Tday = 0;
            string _issuedDate = "";
            double Fine=0.0;
            // DateTime _IssuedDate;
            DateTime _Toady, IssDate;
            //string _Now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _Toady = DateTime.Now;
            string sql = "select tblbookissue.DateOfIssue from tblbookissue where tblbookissue.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _issuedDate = m_MyReader.GetValue(0).ToString();
            
            IssDate = DateTime.Parse(_issuedDate);
            TimeSpan span = _Toady - IssDate;
            days = span.TotalDays;
            int Maxdays = GetMaxDays();
            if (days > Maxdays)
            {
                _Tday = days - Maxdays;
                Fine = RetriveFine(_Tday, type);
            }
            else
            {
                Fine = 0.0;
            }
            }
            return Fine;
        }

        public int GetMaxDays()
        {
            int _Max = 0;
            string sql = "";
            sql = "select tbllibconfig.Value2 from tbllibconfig where ConfigName='Fine'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Max = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return _Max;
        }

        private double RetriveFine(double days,int type)
        {
            double _amount = 0;
            double _Fine;
            string sql = "";
            if (type == 1)
            {
               sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.ConfigName='Fine'";
            }
            else
            {
                sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.ConfigName='StaffFine'";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                if (Convert.ToString(m_MyReader.GetValue(0))!="")
                {
                    _amount = double.Parse(m_MyReader.GetValue(0).ToString());
                }
            } if (days > 0 && days < 1)
                days = 1;
            _Fine = _amount * Math.Round(days);
            return _Fine;
        }

        public void CollectBook(string _BookId)
        {
            string sql = "delete from tblbookissue where tblbookissue.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        /* public bool BookedNotByUSer(string _BookNo, string _userId, out string _User)
         {
             bool _valid = false;
             _User="";
             string sql = "select tblbookbooking.Id from tblbookbooking  where tblbookbooking.bookId='" +_BookNo + "' and tblbookbooking.UserId = '" + _userId +"'";
             m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             if (m_MyReader.HasRows)
             {
                 _valid = true;
             }
             if (!_valid)
             {
                 sql = "select tblbookbooking.UserId from tblbookbooking  where tblbookbooking.bookId='" + _BookNo +"'";
                 m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                 if (m_MyReader.HasRows)
                 {
                     _User = m_MyReader.GetValue(0).ToString();
                 }
             }
             return _valid;
         }*/

        public bool DifferentUserBooked(string _UserId, string _BookNo, out string _User, int _UserType)
        {
            bool _valid = true;
            int _bookedusertypeid = 0;
            _User = "";
            string sql = "";
            string _UserUniqiid = GetUserUniqueIdFrombooking(_BookNo, out _bookedusertypeid);
            if (_bookedusertypeid == _UserType)
            {
                if (_UserUniqiid.CompareTo(_UserId) == 0)
                {
                    _valid = false;
                }

                else
                {
                    _valid = true;
                    _User = _UserUniqiid;
                }
            }

            int Type,UserId;
           Type= GetUserTypeFromBookTable(_BookNo,out   UserId);
            if (_valid)
            {
                if (Type == 1)
                {
                    sql = "select tblstudent.StudentName from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "' ";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        _User = m_MyReader.GetValue(0).ToString();
                    }
                }
                else if (Type == 2)
                {
                    sql = "select tbluser.UserName from tblbookbooking inner join tbluser on tbluser.Id = tblbookbooking.UserId where tblbookbooking.bookId='" + _BookNo + "'";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        _User = m_MyReader.GetValue(0).ToString();
                    }
                }

            }
            return _valid;
        }

        private string GetUserUniqueIdFrombooking(string _BookNo,out int UserTypeId)
        {
            string UserId = "";
            string sql = "";
            int UserUnId;
            UserTypeId = GetUserTypeFromBookTable(_BookNo, out UserUnId);
            if (UserTypeId != 0)
            {
                if (UserTypeId == 1)
                {
                    sql = "select tblstudent.Id from tblstudent where tblstudent.Id=" + UserUnId + " and tblstudent.`Status`=1";
                }
                else if (UserTypeId == 2)
                {
                    sql = "select tbluser.Id from tbluser where tbluser.Id = '" + UserUnId + "' and tbluser.`Status`=1";
                }
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    UserId = m_MyReader.GetValue(0).ToString();
                }
            }
            else
            {
                UserId = "None";
            }
            return UserId;
        }

        private int GetUserTypeFromBookTable(string _BookNo, out int UserUnId)
        {
            int UserTypeId = 0;
            UserUnId = -1;
            string sql = "select UserType,UserId from tblbookbooking where bookId = '" + _BookNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                UserTypeId = int.Parse(m_MyReader.GetValue(0).ToString());
                UserUnId = int.Parse(m_MyReader.GetValue(1).ToString());
            }
            return UserTypeId;
        }

        //private int GetbookedUserId(string _BookNo)
        //{

        //     int id=-1;
        //     string sql = "select DISTINCT tblstudent.Id from tblbookbooking inner join tblstudent on tblbookbooking.UserId = tblstudent.Id where tblstudent.`Status`=1 and tblbookbooking.UserType=" + Type + " and tblbookbooking.bookId='" + _BookNo + "'";
        //     m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //     if (m_MyReader.HasRows)
        //     {
        //         id = int.Parse(m_MyReader.GetValue(0).ToString()); 
        //     }
        //     return id;
        //}

        public bool SameUserBooked(string _UserId, string _BookNo, int _UserType)
        {
            bool _valid = false;
            if (_UserType == 1)
            {
                string sql = "select tblbookbooking.Id from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblbookbooking.bookId ='" + _BookNo + "'and tblstudent.Id='" + _UserId + "' and tblstudent.`Status`=1 and tblbookbooking.UserType=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            else if (_UserType == 2)
            {
                string sql = "select tblbookbooking.Id from tblbookbooking inner join tbluser on tbluser.Id = tblbookbooking.UserId where tblbookbooking.bookId ='" + _BookNo + "'and tbluser.Id='" + _UserId + "' and tbluser.`Status`=1 and tblbookbooking.UserType=2";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            return _valid;
        }

        public void RemoveBooking(string _UserId, string BookId, int _UserType)
        {
            //if (_UserType == 1)
            //{
            //string sql = "delete from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId where tblstudent.AdmitionNo='" + _UserId + "' and tblbookbooking.bookId ='" + BookId + "' and tblbookbooking.UserType=1";
            string sql = "delete from tblbookbooking where tblbookbooking.bookId=" + BookId + " and tblbookbooking.UserType=" + _UserType + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            // }
            // else if (_UserType == 2)
            // {
            //string sql = "delete from tblbookbooking inner join tbluser on tbluser.Id = tblbookbooking.UserId where tbluser.UserName='" + _UserId + "' and tblbookbooking.bookId ='" + BookId + "' and tblbookbooking.UserType=2";
            // string sql = "delete from tblbookbooking where tblbookbooking.bookId=" + BookId + " and tblbookbooking.UserType=" + _UserType + "";
            // m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            // }
        }

        public bool IssuedToSameUser(string _UserId, string _BookNo, int _UserType)
        {
            bool _valid = false;
            if (_UserType == 1)
            {

                //string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo ='" + _BookNo + "' and tblbookissue.UserId='" + _UserId + "'";
                //string sql = "select tblbookissue.Id from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookNo + "' and tblstudent.AdmitionNo='" + _UserId + "' and tblstudent.`Status`=1 and tblbookissue.UserType=1";
                string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.UserId=" + _UserId + " and tblbookissue.BookNo=" + _BookNo + " and tblbookissue.UserType=1";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }
            else if (_UserType == 2)
            {
               // string sql = "select tblbookissue.Id from tblbookissue inner join tbluser on tbluser.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tbluser.`Status`=1 and tblbookissue.UserType=2";
                string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.UserId=" + _UserId + " and tblbookissue.BookNo=" + _BookNo + " and tblbookissue.UserType=2";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }
            }

            return _valid;
        }
        //public bool BookedToSameUser(string _UserId, string _BookNo, int _UserType)
        //{
        //    bool _valid = false;
        //    if (_UserType == 1)
        //    {

        //        //string sql = "select tblbookissue.Id from tblbookissue where tblbookissue.BookNo ='" + _BookNo + "' and tblbookissue.UserId='" + _UserId + "'";
        //        //string sql = "select tblbookbooking.Id from tblbookbooking inner join tblstudent on tblstudent.Id = tblbookbooking.UserId  where tblbookbooking.bookId='" + _BookNo + "' and tblstudent.AdmitionNo='" + _UserId + "' and tblstudent.`Status`=1 and tblbookbooking.UserType=1";
        //        string sql = "select tblbookbooking.Id from tblbookbooking where tblbookbooking.UserId=" + _UserId + " and tblbookbooking.bookId=" + _BookNo + " and tblbookbooking.UserType=1";
        //        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //        if (m_MyReader.HasRows)
        //        {
        //            _valid = true;
        //        }
        //    }
        //    else if (_UserType == 2)
        //    {
        //        //string sql = "select tblbookbooking.Id from tblbookbooking inner join tbluser on tbluser.Id = tblbookbooking.UserId  where tblbookbooking.bookId='" + _BookNo + "' and tbluser.UserName='" + _UserId + "' and tbluser.`Status`=1 and tblbookbooking.UserType=2";
        //        string sql = "select tblbookbooking.Id from tblbookbooking where tblbookbooking.UserId=" + _UserId + " and tblbookbooking.bookId=" + _BookNo + " and tblbookbooking.UserType=2";
        //        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //        if (m_MyReader.HasRows)
        //        {
        //            _valid = true;
        //        }
        //    }

        //    return _valid;
        //}

        public void SaveFine(string _Fine, string _MaxDay,string _Fine_staff)
        {
            string sql = "update tbllibconfig set Value1='" + _Fine + "', Value2='" + _MaxDay + "' where tbllibconfig.ConfigName='Fine'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            sql = "update tbllibconfig set Value1='" + _Fine_staff + "' where tbllibconfig.ConfigName='StaffFine'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void SaveBookType(string _Type, string _Type_Desc)
        {
            string sql = "insert into tblbooktype(TypeName,`Desc`) values ('" + _Type + "','" + _Type_Desc + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void SaveBookCategory(string _Cat, string _Desc)
        {
            string sql = "insert into tblbookcatogory(CatogoryName,`Disc`) values ('" + _Cat + "','" + _Desc + "')";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public bool categoryExists(string _category)
        {
            bool _valid = true;
            string sql = "select tblbookcatogory.Id from tblbookcatogory where tblbookcatogory.CatogoryName='" + _category + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            return _valid;

        }

        public bool TypeExists(string _Type)
        {
            bool _valid = true;
            string sql = "select tblbooktype.Id from tblbooktype where  tblbooktype.TypeName ='" + _Type + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            return _valid;
        }

        public bool DeleteTypeById(int _TypeId, out string _message)
        {
            _message = "";
            string sql = "select tblbookmaster.Id from tblbookmaster where tblbookmaster.TypeId=" + _TypeId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _message = "Book(s) present under this type. Please assign some other type to them";
                return false;
            }
            else
            {
                sql = "delete from tblbooktype where tblbooktype.Id=" + _TypeId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Type is deleted";
                return true;
            }
        }

        public bool DeleteCategoryById(int _CatId, out string _message)
        {
            _message = "";
            string sql = "select tblbookmaster.Id from tblbookmaster where tblbookmaster.CatagoryId=" + _CatId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _message = "Book(s) present under this Category. Please assign some other Category to them";
                return false;
            }
            else
            {
                sql = "delete from tblbookcatogory where tblbookcatogory.Id=" + _CatId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Category is deleted";
                return true;
            }
        }

        public string GetSubLibraryMangMenuString(int _roleid)
        {
            string _MenuStr;
            if (m_SubLibMenuStr == "")
            {
                _MenuStr = "<ul class=\"block\"><li><a href=\"BookSearchDetails.aspx\">Book Details</a></li>";
                string sql = "SELECT DISTINCT tblaction.ActionName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND tblroleactionmap.ModuleId=22 AND tblaction.ActionType='SubLink'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {

                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
                m_MyReader.Close();
                m_SubLibMenuStr = _MenuStr;
            }
            else
            {
                _MenuStr = m_SubLibMenuStr;
            }
            return _MenuStr;
        }

        public int UpdateBooks(string _name, string _auther, string _Publisher, int year, string _Edition, int _Type, int _category, int _id, int _RackId, string _BookNo, double _Price,string Barcode,int Barcode_type)
        {
            int Id = -1;
            string sql = "update tblbookmaster set BookName='" + _name + "', Author='" + _auther + "', Publisher='" + _Publisher + "',Year=" + year + ", Edition='" + _Edition + "',TypeId=" + _Type + ",CatagoryId=" + _category + " , Cost=" + _Price + "  where Id=" + _id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            Id = 1;
            sql = "update tblbooks set RackId='" + _RackId + "' where BookNo='" + _BookNo + "' and BookId='" + _id + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            Id = 1;
            if (Barcode_type==2)
            {
                sql = "update tblbookmaster set Barcode='" + Barcode + "' where Id=" + _id + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                Id = 1;
                sql = "update tblbooks set Barcode='" + Barcode + "' where BookId='" + _id + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                Id = 1;
            }
            return Id;
        }


        public int GetbookId(string _BookNo)
        {
            int bookId = 0;
            string sql = "select tblbookmaster.Id from tblbookmaster inner join tblbooks on tblbooks.BookId = tblbookmaster.Id where tblbooks.BookNo='" + _BookNo + "' ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                bookId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            m_MyReader.Close();
            return bookId;
        }

        public int GetBkId(string _BookId)
        {
            int BkId = -1;
            string sql = "select tblbookmaster.Id from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                BkId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            m_MyReader.Close();
            return BkId;
        }

        public void DeleteBook(string _BookId)
        {
            string sql = "delete from tblbookhistory where tblbookhistory.BookId =" + _BookId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
             sql = "delete from tblbooks where tblbooks.BookNo='" + _BookId + "'";

            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void DeleteAlBkCopy(int _BkId)
        {

            DeleteBookHistory(_BkId);

            string sql = "delete from tblbooks where tblbooks.BookId=" + _BkId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            sql = "delete from tblbookmaster where tblbookmaster.Id=" + _BkId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        private void DeleteBookHistory(int _BkId)
        {
            string sql = "select tblbooks.BookNo from tblbooks where tblbooks.BookId=" + _BkId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    string sql_delete = "delete from tblbookhistory where tblbookhistory.BookId =" +int.Parse(m_MyReader.GetValue(0).ToString());
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql_delete);
                }

            }

        }

        public void CancelBookin(string BookId)
        {
            string sql = "delete from tblbookbooking where tblbookbooking.bookId='" + BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public bool NoBooksIssued(int _BkId)
        {
            string sql = "select tblbooks.BookNo from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo = tblbooks.BookNo where tblbookmaster.Id=" + _BkId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                return false;
            }
            return true;
        }

        public bool NobooksBooked(int BkId)
        {
            string sql = "select tblbooks.BookNo from tblbooks inner join tblbookmaster on tblbookmaster.Id = tblbooks.BookId inner join tblbookbooking on tblbookbooking.bookId = tblbooks.BookNo where tblbookmaster.Id=" + BkId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                return true;
            }
            return false;
        }

        public void CancelAllBooking(int BkId)
        {
            OdbcDataReader m_MyReader1 = null;
            //string []array=new string[100];
            //int i = 0;
            //int length;
            string sql = "SELECT tblbookbooking.bookId from tblbookbooking inner join tblbooks on tblbooks.BookNo = tblbookbooking.bookId  where tblbooks.BookId = " + BkId + "";
            m_MyReader1 = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader1.HasRows)
            {
                while (m_MyReader1.Read())
                {
                    // array[i] = m_MyReader.GetValue(0).ToString();
                    //i++;
                    CancelBookin(m_MyReader1.GetValue(0).ToString());
                }
            }
            //length = array.Length;
            //for (i = 0; i < length; i++)
            //// {
            //CancelBookin(array[i]);
            //}
        }

        public string GetTakenByUser(string _BookId)
        {
            string _UserId;
            string sql = "select tblbookissue.UserId from tblbookissue where tblbookissue.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _UserId = m_MyReader.GetValue(0).ToString();
            }
            else
            {
                _UserId = "None";
            }
            return _UserId;
        }

        public string GetIssuedDate(string _BookId)
        {
            string _Date = "";
            string sql = "select tblbookissue.DateOfIssue from tblbookissue where tblbookissue.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Date = m_MyReader.GetValue(0).ToString();
            }
            return _Date;
        }

        public bool RackExists(string _RackNo)
        {
            bool _valid = true;
            string sql = "select tblbookrack.Id from tblbookrack where tblbookrack.RackName='" + _RackNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            return _valid;
        }

        public void AddRack(string _RackNo, string _Desc)
        {
            string sql = "insert into tblbookrack(RackName,`Desc`,CatogoryId,TypeId,MaxBookCount) values ('" + _RackNo + "','" + _Desc + "',NULL,NULL,NULL)";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        //public void UpdateUserdetails(string _UsId, string _UserName, string _Desc, int _usertype, string _classDept)
        //{
        //    string sql = "update tblbookusermaster set UserName='" + _UserName + "', UserType=" + _usertype + ",Class='" + _classDept + "', `Desc`='" + _Desc + "' where UserId='" + _UsId + "'";
        //    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //    m_MyReader.Close();
        //}

        public bool DeleteRackById(int _RackId, out string _message)
        {
            _message = "";
            string sql = "select tblbooks.RackId from tblbooks where tblbooks.RackId=" + _RackId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _message = "Book(s) present in this Rack. Please assign some other Rack to them";
                return false;
            }
            else
            {
                sql = "delete from tblbookrack where tblbookrack.Id=" + _RackId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                _message = "Rack is deleted";
                return true;
            }
        }

        public string GetDetailsString(int _ActionId)
        {
            string _strdetails = "";

            string sql = "SELECT `tblaction`.`ActionName`, `tblaction`.`Description` FROM `tblaction` WHERE `tblaction`.`Id` =" + _ActionId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();

                _strdetails = "<br/><h2>" + m_MyReader.GetValue(0).ToString() + "</h2><br /><p>" + m_MyReader.GetValue(1).ToString() + "</p><br/>";

            }

            m_MyReader.Close();

            return _strdetails;
        }

        public void UpdateCategory(string _CategoryName, string _Desc, int _CatId)
        {
            string sql = "update tblbookcatogory set tblbookcatogory.CatogoryName='" + _CategoryName + "', tblbookcatogory.Disc='" + _Desc + "' where tblbookcatogory.Id=" + _CatId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateType(string _TypeName, string _Desc, int _TypeId)
        {
            string sql = "update tblbooktype set tblbooktype.TypeName='" + _TypeName + "', tblbooktype.Desc='" + _Desc + "' where tblbooktype.Id=" + _TypeId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateRack(string _RackNo, string _Desc, int _RackId)
        {
            string sql = "update tblbookrack set tblbookrack.RackName='" + _RackNo + "', tblbookrack.Desc='" + _Desc + "' where tblbookrack.Id=" + _RackId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }



        public int GetUserId(string _UserUniqeId, int _UserType)
        {
            int UseId = -1;
            string sql = "";
            if (_UserType == 1)
            {
                sql = "select tblstudent.Id from tblstudent where tblstudent.Id='" + _UserUniqeId + "' and tblstudent.`Status`=1";

            }
            else if (_UserType == 2)
            {
                sql = "select tbluser.Id from tbluser where tbluser.Id = '" + _UserUniqeId + "' and tbluser.`Status`=1";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                UseId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return UseId;
        }

        public void ReportHistory(string _BookId, string _UserId, string _UserType, string _IssuedDate, string _FineCollected, string _Comment, int _batchid)
        {
            DateTime Now = DateTime.Today;
            DateTime Date = General.GetDateTimeFromText(_IssuedDate);
            string sql = "insert into tblbookhistory (BookId,UserId,UserTypeId,FineAmount,TakenDate,ReturnedDate,Comment,CurrentBatchId) values(" + _BookId + ",'" + _UserId + "','" + _UserType + "'," + _FineCollected + ",'" + Date.ToString("s") + "','" + Now.Date.ToString("s") + "','" + _Comment + "'," + _batchid + ")";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }





        public string GetUserUniqueId(string _BookId, out string _Name)
        {
            _Name = "";
            string UserId = "";
            string sql = "";
            int UserUnId;
            int UserTypeId = GetUserTypeId(_BookId, out UserUnId);
            if (UserTypeId != 0)
            {
                if (UserTypeId == 1)
                {
                    sql = "select tblstudent.Id,tblstudent.StudentName from tblstudent where tblstudent.Id=" + UserUnId + " and tblstudent.`Status`=1";
                }
                else if (UserTypeId == 2)
                {
                    sql = "select tbluser.Id,tbluser.UserName from tbluser where tbluser.Id = '" + UserUnId + "' and tbluser.`Status`=1";
                }
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    UserId = m_MyReader.GetValue(0).ToString();
                    _Name = m_MyReader.GetValue(1).ToString();
                }
            }
            else
            {
                UserId = "None";
                _Name = "None";
            }
            return UserId;
        }


        private int GetUserTypeId(string _BookId, out int _UserId)
        {
            int UserTypeId = 0;
            _UserId = -1;
            string sql = "select UserType,UserId from tblbookissue where BookNo = '" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                UserTypeId = int.Parse(m_MyReader.GetValue(0).ToString());
                _UserId = int.Parse(m_MyReader.GetValue(1).ToString());
            }
            return UserTypeId;
        }


        public string GetUserUniqueName(string _BookId)
        {
            string UserId = "";
            string sql = "";
            int UserUnId;
            int UserTypeId = GetUserTypeId(_BookId, out UserUnId);
            if (UserTypeId != 0)
            {
                if (UserTypeId == 1)
                {
                    sql = "select tblstudent.StudentName from tblstudent where tblstudent.Id=" + UserUnId + " and tblstudent.`Status`=1";
                }
                else if (UserTypeId == 2)
                {
                    sql = "select tbluser.UserName from tbluser where tbluser.Id = '" + UserUnId + "' and tbluser.`Status`=1";
                }
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    UserId = m_MyReader.GetValue(0).ToString();
                }
            }
            else
            {
                UserId = "None";
            }
            return UserId;
        }
        public string GetBookingUserUniqueId(string _BookId)
        {
            string UserId = "";
            string sql = "";
            int UserUnId;
            int UserTypeId = GetUserIdFromBookingTbl(_BookId, out UserUnId);
            if (UserTypeId != 0)
            {
                if (UserTypeId == 1)
                {
                    sql = "select tblstudent.StudentName from tblstudent where tblstudent.Id=" + UserUnId + " and tblstudent.`Status`=1";
                }
                else if (UserTypeId == 2)
                {
                    sql = "select tbluser.UserName from tbluser where tbluser.Id = '" + UserUnId + "' and tbluser.`Status`=1";
                }
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    UserId = m_MyReader.GetValue(0).ToString();
                }
            }
            else
            {
                UserId = "None";
            }
            return UserId;
        }

        private int GetUserIdFromBookingTbl(string _BookId, out int _UserId)
        {
            int UserTypeId = 0;
            _UserId = -1;
            string sql = "select tblbookbooking.UserId, tblbookbooking.UserType from tblbookbooking where tblbookbooking.bookId = '" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                UserTypeId = int.Parse(m_MyReader.GetValue(1).ToString());
                _UserId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return UserTypeId;
        }

        public string GetBookName(string _BookId)
        {
            string _BookName = "";
            string sql = "select tblbookmaster.BookName from tblbooks inner join  tblbookmaster on tblbookmaster.Id = tblbooks.BookId where tblbooks.BookNo='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _BookName = m_MyReader.GetValue(0).ToString();
            }
            return _BookName;
        }

        public string GetbookData(string _BookId)
        {
            string _BookData = "";

            string _User = "";
            int UserTypeId = 0;
            if (BookIssued(_BookId))
            {
                _BookData = "The book is taken by ";
                UserTypeId = GetTypeId(_BookId);
                _User = GetUserName(_BookId, UserTypeId);
                _BookData = _BookData + _User;

            }
            else
            {
                _BookData = "The book is available in the library";
            }

            return _BookData;
        }

        private string GetUserName(string _BookId, int UserTypeId)
        {
            string sql = "", UserUniqId = "";
            if (UserTypeId == 1)
            {
                sql = "select tblstudent.AdmitionNo,tblstudent.StudentName from tblbookissue inner join tblstudent on tblstudent.Id = tblbookissue.UserId  where tblbookissue.BookNo='" + _BookId + "' and tblstudent.`Status`=1 and tblbookissue.UserType=1";
                UserUniqId = "Student : ";
            }
            else if (UserTypeId == 2)
            {
                sql = "select tbluser.UserName,tbluser.SurName from tblbookissue inner join tbluser on  tbluser.Id = tblbookissue.UserId where tblbookissue.BookNo = '" + _BookId + "' and tbluser.`Status`=1 and tblbookissue.UserType=2";
                UserUniqId = "Staff : ";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                UserUniqId = m_MyReader.GetValue(1).ToString() + " ( " + m_MyReader.GetValue(0).ToString()+" )";
            }
            return UserUniqId;
        }

        private int GetTypeId(string _BookId)
        {
            int TypeId = 0;
            string sql = "select UserType from tblbookissue where BookNo = '" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                TypeId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return TypeId;
        }

        public bool HistoryExists(string _BookId)
        {
            bool _valid = false;
            string sql = "select BookId from tblbookhistory where BookId='" + _BookId + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = true;
            }
            return _valid;
        }

        public void DeleteBookHistory(int _HistoryID, out string _message)
        {
            _message = "";
            string sql = "delete from tblbookhistory where Id=" + _HistoryID + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            _message = "History data is deleted";
        }

        public void AddBookLimit(string _Limit,string _Limit2)
        {
            string sql = "update tbllibconfig set Value1='" + _Limit + "',Value2='" + _Limit2 + "' where tbllibconfig.FieldName='Limit'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            m_MyReader.Close();
        }

        public bool IssueLimNotExceeds(int _UserId, int _UserType)
        {
            bool _valid = true;
            int IssueCount = GetIssuedCount(_UserId, _UserType);
            int MaxCount = GetBookLimit(_UserType);
            int Diff = MaxCount - IssueCount;
            if ((Diff == 0) || (Diff < 0))
            {
                _valid = false;
            }
            return _valid;
        }
        public bool Book_already_issued(int _UserId, int _UserType,int book_no)
        {
            bool _valid =true;
            string sql = "";
            if (_UserType == 1)
            {
                sql = "select tblbookissue.Id from tblbookissue where tblbookissue.UserId=" + _UserId + " and tblbookissue.BookNo=" + book_no + " and tblbookissue.UserType=1";
            }
            else
            {
                sql = "select tblbookissue.Id from tblbookissue where tblbookissue.UserId=" + _UserId + " and tblbookissue.BookNo=" + book_no + " and tblbookissue.UserType=2";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = false;
            }
            return _valid;
        }

        private int GetBookLimit(int _Type)
        {
            int count = 0;
            string sql = "";
            if (_Type == 1)
            {
                sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='Limit'";
            }
            else
            {
                sql = "select tbllibconfig.Value2 from tbllibconfig where tbllibconfig.FieldName='Limit'";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                try
                {
                    count = int.Parse(m_MyReader.GetValue(0).ToString());
                }
                catch
                {
                    count = 0;
                }
            }
            return count;
        }

        private int GetIssuedCount(int _UserId, int _UserType)
        {
            int Count = 0;
            bool _valid;
            string sql = "select count(tblbookissue.Id) from tblbookissue where tblbookissue.UserId='" + _UserId + "' and tblbookissue.UserType=" + _UserType + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
                if (_valid)
                {
                    return Count;
                }
            }
            return Count;
        }

        public bool BookingLimNotExceeds(int _UserId, int _UserType)
        {
            bool _valid = true;
            int IssueCount = GetBookedCount(_UserId, _UserType);
            int MaxCount = GetBookLimit(_UserType);
            int Diff = MaxCount - IssueCount;
            if ((Diff == 0) || (Diff < 0))
            {
                _valid = false;
            }
            return _valid;
        }

        private int GetBookedCount(int _UserId, int _UserType)
        {
            int Count = 0;
            bool _valid;
            string sql = "select count(tblbookbooking.Id) from tblbookbooking where tblbookbooking.UserId='" + _UserId + "' and tblbookbooking.UserType=" + _UserType + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out Count);
                if (_valid)
                {
                    return Count;
                }
            }
            return Count;
        }

        public bool IsBarcodeActive()
        {
            int ActiveVal = 0;
            bool Active = false;
            string sqlActive = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='ActiveBarcode'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlActive);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out ActiveVal);
                if (ActiveVal == 1)
                    Active = true;
            }
            return Active;
        }




        public bool UniqueNumber()
        {
            // if this function is return true all copies contain different code
            int UniqueNum = 0;
            bool Unique = true;
            string sqlActive = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='UniqueBarcode'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlActive);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out UniqueNum);
                if (UniqueNum == 2)
                    Unique = false;
            }
            return Unique;
        }

        public bool BarcodeExistinMaster(string _Barcode)
        {
            string sql_barcode = "select tblbookmaster.Id from tblbookmaster where tblbookmaster.Barcode='" + _Barcode + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql_barcode);
            if (m_MyReader.HasRows)
            {
                return true;
            }
            else return false;
        }

        public bool isBarcodeExist(string Barcode, int BookId, string BookName)
        {
            string sqlbarcode = "";

            if (BookId == 0)
            {
                sqlbarcode = "select distinct  tblbooks.Id from tblbooks where tblbooks.Barcode='" + Barcode + "'";
            }
            else if (BookId != 0 && String.IsNullOrEmpty(BookName))
            {
                sqlbarcode = "select distinct  tblbooks.Id from tblbooks where tblbooks.Barcode='" + Barcode + "' and tblbooks.Id<>" + BookId;
            }
            else
            {
                sqlbarcode = " select distinct tblbooks.Id from tblbooks inner  join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbooks.Barcode='" + Barcode + "' and tblbookmaster.BookName<>'" + BookName + "'";

            }

            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbarcode);
            if (m_MyReader.HasRows)
                return true;
            return false;
        }


        public bool BarcodeExistinCopies(string _Barcode)
        {
            string sqlbarcode = "";
            sqlbarcode = "select distinct  tblbooks.Id from tblbooks where tblbooks.Barcode='" + _Barcode + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbarcode);
            if (m_MyReader.HasRows)
                return true;
            return false;
        }

        public int BookCount(int CategoryId)
        {
            int Total = 0;

            string sql = "select count(tblbooks.Id) from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId where tblbookmaster.CatagoryId=" + CategoryId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out  Total);
            }
            return Total;
        }

        public int IssuedBooks(int CategoryId)
        {
            int Total = 0;
            string sql = "select count(tblbooks.Id) from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo= tblbooks.BookNo where tblbookmaster.CatagoryId=" + CategoryId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out  Total);
            }
            return Total;
        }

        public int IssuedBooksbyTeaschers(int CategoryId)
        {
            int Total = 0;
            string sql = "select count(tblbooks.Id) from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo= tblbooks.BookNo where tblbookmaster.CatagoryId=" + CategoryId + " and tblbookissue.UserType=2";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out  Total);
            }
            return Total;

        }

        public int IssuedBooksbyStudent(int CategoryId)
        {
            int Total = 0;
            string sql = "select count(tblbooks.Id) from tblbooks inner join tblbookmaster on tblbookmaster.Id= tblbooks.BookId inner join tblbookissue on tblbookissue.BookNo= tblbooks.BookNo where tblbookmaster.CatagoryId=" + CategoryId + " and tblbookissue.UserType=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out  Total);
            }
            return Total;
        }

        public System.Data.DataSet GetBooksFromCategoryId(int CategoryId)
        {
            string sql = "select tblbookmaster.Id, tblbookmaster.BookName from tblbookmaster ";
            if (CategoryId != 0)
                sql = sql + " where tblbookmaster.CatagoryId=" + CategoryId;

            m_Dataset = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return m_Dataset;

        }

        public string GetBookCount(int BookId)
        {
            string sql = "select tblbookmaster.Id, count(tblbooks.Id) from tblbookmaster inner join tblbooks on tblbooks.BookId= tblbookmaster.Id where tblbookmaster.Id=" + BookId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                return (m_MyReader.GetValue(1).ToString());
            }
            return "0";
        }

        public OdbcDataReader GetCategories()
        {
            string sql = "select tblbookcatogory.Id, tblbookcatogory.CatogoryName from tblbookcatogory ";
            return m_MysqlDb.ExecuteQuery(sql);

        }

        public bool BarCodeisSame(string _Barcode, int BkId)
        {
            int MasterId = 0;
            bool valid = false;
            string sql = "select distinct tblbooks.BookId from tblbooks where tblbooks.BookNo=" + BkId;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out MasterId);
                string sql1 = "select distinct tblbooks.BookId from tblbooks where tblbooks.BookId=" + BkId + " and tblbooks.Barcode='" + _Barcode + "'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql1);

                if (m_MyReader.HasRows)
                {
                    valid = true;
                }

            }

            return valid;
        }

        public string GetUserName(string Type, string Id)
        {
            string sql = "",Name="";
            if (Type == "1" || Type=="Student")
            {
                sql = "select tblstudent.StudentName from tblstudent where tblstudent.Id=" + Id + " and tblstudent.`Status`=1";

            }
            else if (Type == "2" || Type == "Staff")
            {
                sql = "select tbluser.UserName from tbluser where tbluser.Id = '" + Id + "' and tbluser.`Status`=1";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Name = m_MyReader.GetValue(0).ToString();
            }
            return Name;
        }







        public DataSet GetDetails(DateTime _Fromdate, DateTime _Todate, int _CatId)
        {
            OdbcDataReader Details_Reader = null;
            DataSet BookTransaction_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            BookTransaction_Ds.Tables.Add(new DataTable("BookTransactionReport"));
            dt = BookTransaction_Ds.Tables["BookTransactionReport"];
            dt.Columns.Add("BookId");
            dt.Columns.Add("UserId");
            dt.Columns.Add("UserTypeId");
            dt.Columns.Add("TakenDate");
            dt.Columns.Add("BookName");
            dt.Columns.Add("Author");
            dt.Columns.Add("Publisher");
            dt.Columns.Add("Edition");
            dt.Columns.Add("CatogoryName");
            dt.Columns.Add("UserName");
            dt.Columns.Add("UserType");
            //BookName,Author,Publisher,Edition,UserName
            string sql = "";

            sql = "SELECT tblbookhistory.BookId, tblbookhistory.UserTypeId,DATE_FORMAT( tblbookhistory.TakenDate, '%d/%m/%Y') as TakenDate,UserId FROM tblbookhistory  WHERE tblbookhistory.TakenDate between '" + _Fromdate.ToString("s") + "' AND '" + _Todate.ToString("s") + "'";
            OdbcDataReader   M_Reader = m_MysqlDb.ExecuteQuery(sql);
            //DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (M_Reader.HasRows)
                {
                    while (M_Reader.Read())
                    {

                        Details_Reader = GetDetailsOfBook(int.Parse(M_Reader.GetValue(0).ToString()), _CatId);
                        if (Details_Reader.HasRows)
                        {
                            string s = M_Reader.GetValue(1).ToString();
                            _dr = BookTransaction_Ds.Tables["BookTransactionReport"].NewRow();
                            _dr["BookId"] = M_Reader.GetValue(0).ToString();
                            _dr["UserId"] = M_Reader.GetValue(3).ToString();
                            _dr["UserTypeId"] = M_Reader.GetValue(1).ToString();
                            _dr["TakenDate"] = M_Reader.GetValue(2).ToString();
                            _dr["BookName"] = Details_Reader.GetValue(0).ToString();
                            _dr["Author"] = Details_Reader.GetValue(1).ToString();
                            _dr["Edition"] = Details_Reader.GetValue(2).ToString();
                            _dr["Publisher"] = Details_Reader.GetValue(3).ToString();
                            _dr["CatogoryName"] = Details_Reader.GetValue(4).ToString();
                            _dr["UserName"] = "";
                            _dr["UserType"] = "";
                            BookTransaction_Ds.Tables["BookTransactionReport"].Rows.Add(_dr);
                        }
                    }
                }

            sql = "SELECT tblbookissue.BookNo, tblbookissue.UserType,DATE_FORMAT( tblbookissue.DateOfIssue, '%d/%m/%Y') as Issuedate,UserId FROM tblbookissue  WHERE tblbookissue.DateOfIssue between '" + _Fromdate.ToString("s") + "' AND '" + _Todate.ToString("s") + "'";
             M_Reader = m_MysqlDb.ExecuteQuery(sql);
            //DataSet Dt = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (M_Reader.HasRows)
            {
                while (M_Reader.Read())
                {

                    Details_Reader = GetDetailsOfBook(int.Parse(M_Reader.GetValue(0).ToString()), _CatId);
                    if (Details_Reader.HasRows)
                    {
                        string s = M_Reader.GetValue(1).ToString();
                        _dr = BookTransaction_Ds.Tables["BookTransactionReport"].NewRow();
                        _dr["BookId"] = M_Reader.GetValue(0).ToString();
                        _dr["UserId"] = M_Reader.GetValue(3).ToString();
                        _dr["UserTypeId"] = M_Reader.GetValue(1).ToString();
                        _dr["TakenDate"] = M_Reader.GetValue(2).ToString();
                        _dr["BookName"] = Details_Reader.GetValue(0).ToString();
                        _dr["Author"] = Details_Reader.GetValue(1).ToString();
                        _dr["Edition"] = Details_Reader.GetValue(2).ToString();
                        _dr["Publisher"] = Details_Reader.GetValue(3).ToString();
                        _dr["CatogoryName"] = Details_Reader.GetValue(4).ToString();
                        _dr["UserName"] = "";
                        _dr["UserType"] = "";
                        BookTransaction_Ds.Tables["BookTransactionReport"].Rows.Add(_dr);
                    }
                }
            }
              
            return BookTransaction_Ds;

        }

        private DataSet GetUserNameAndType(DataSet reportDs)
        {
            if (reportDs != null && reportDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in reportDs.Tables[0].Rows)
                {
                    string usertype = "";
                    string username = "";
                    GetUserNameAndTypeOfEach(int.Parse(dr["UserTypeId"].ToString()),int.Parse(dr["UserId"].ToString()), out  usertype, out username);

                    dr["BookId"] = dr["BookId"].ToString();
                    dr["UserId"] = dr["UserId"].ToString();
                    dr["UserTypeId"] = dr["UserTypeId"].ToString();
                    dr["TakenDate"] = dr["TakenDate"].ToString();
                    dr["BookName"] = dr["BookName"].ToString();
                    dr["Author"] = dr["Author"].ToString();
                    dr["Edition"] = dr["Edition"].ToString();
                    dr["Publisher"] = dr["Publisher"].ToString();
                    dr["CatogoryName"] = dr["CatogoryName"].ToString();
                    dr["UserName"] =username ;
                    dr["UserType"] = usertype;

                }
            }
            return reportDs;
        }

        private void GetUserNameAndTypeOfEach(int UserTypeId, int userId, out string usertype, out string username)
        {
            string sql = "";
            username = "";
            usertype = "";
            if (UserTypeId == 1)
            {
                sql = "select tblstudent.StudentName from tblstudent where tblstudent.Id=" + userId + " and tblstudent.`Status`=1";
                usertype = "Student";

            }
            else if (UserTypeId == 2)
            {
                sql = "select tbluser.UserName from tbluser where tbluser.Id = '" + userId + "' and tbluser.`Status`=1";
                usertype = "Staff";

            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                username = m_MyReader.GetValue(0).ToString();
            }
           
        }

        private OdbcDataReader GetDetailsOfBook(int BookId,int CatId)
        {
            
            string sql = "";
            if (CatId == 0)
            {
                sql = "select   tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher, tblbookcatogory.CatogoryName from tblbookmaster inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId  where tblbookmaster.id in(select tblbooks.BookId from tblbooks where tblbooks.bookno=" + BookId + ")";
            }
            else
            {
                sql = "select tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher, tblbookcatogory.CatogoryName from tblbookmaster inner join tblbookcatogory on tblbookcatogory.Id= tblbookmaster.CatagoryId  where tblbookmaster.id in(select tblbooks.BookId from tblbooks where tblbooks.bookno=" + BookId + ") and  tblbookmaster.CatagoryId ="+CatId+"";
            }
           return  m_MyReader = m_MysqlDb.ExecuteQuery(sql);           
        }

        public DataSet GetReport(DateTime _Fromdate, DateTime _Todate, int _CatId)
        {
            DataSet Report_Ds = new DataSet();
            DataSet Book_Ds = GetDetails(_Fromdate, _Todate, _CatId);
           Report_Ds= GetUserNameAndType(Book_Ds);

            return Report_Ds;
        }

        public DataSet GetBookDetails(int catid)
        {
            string sql = "", subsql = "";
            DataSet Bookds = new DataSet();
            OdbcDataReader Obj_Reader = null;
            sql = " select tblbookmaster.Id, tblbookmaster.BookName, tblbookmaster.Author, tblbookmaster.Edition, tblbookmaster.Publisher, tblbookmaster.Cost, tblbookcatogory.CatogoryName  from tblbookmaster inner join tblbookcatogory on tblbookmaster.CatagoryId= tblbookcatogory.Id";
            if (catid > 0)
            {
                sql = sql + "  where tblbookmaster.CatagoryId=" + catid + "";
            }
            Bookds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Bookds != null && Bookds.Tables[0].Rows.Count > 0)
            {
                Bookds.Tables[0].Columns.Add("TotalStk");
                Bookds.Tables[0].Columns.Add("AvailStk");
                foreach (DataRow dr in Bookds.Tables[0].Rows)
                {
                    subsql = "select count(tblbooks.Id) from tblbooks where bookid="+dr["Id"].ToString()+"";
                    Obj_Reader = m_MysqlDb.ExecuteQuery(subsql);
                    if (Obj_Reader.HasRows)
                    {
                        dr["TotalStk"] = Obj_Reader.GetValue(0).ToString();
                    }
                    else
                    {
                        dr["TotalStk"] = "0";
                    }
                    Obj_Reader.Close();
                    subsql = "select count(tblbooks.Id) from tblbooks where bookid=" + dr["Id"].ToString() + " and bookno not in (select tblbookissue.BookNo from tblbookissue)";
                    Obj_Reader = m_MysqlDb.ExecuteQuery(subsql);
                    if (Obj_Reader.HasRows)
                    {
                        dr["AvailStk"] = Obj_Reader.GetValue(0).ToString();
                    }
                    else
                    {
                        dr["AvailStk"] = "0";
                    }
                    Obj_Reader.Close();
                }
            }
            return Bookds;
        }
        //sai added for check barcode type manual or automatic
        public bool Isbarcodetype_automatic()
        {
            bool _automatic = false;
            int barcode_type = 0;
            string sqlbarcode_type = "";
            sqlbarcode_type = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodeAutomatic'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbarcode_type);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out barcode_type);
            }
            if (barcode_type == 1)
            {
                _automatic = true;
            }
            return _automatic;
        }
        //sai added generate barcode code
        public string Get_barcodeprefix()
        {
            string _barcodeprefix = "";
            string sqlbarcode = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='Barcodeprefix'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbarcode);
            if (m_MyReader.HasRows)
            {
                _barcodeprefix = m_MyReader.GetValue(0).ToString();
            }
            return _barcodeprefix;
        }
        public int get_barcodemincount()
        {
            int _barcodecount = 0;
            string sqlbar_count = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodeMinInput'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbar_count);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _barcodecount);
            }
            return _barcodecount;

        }
        public int get_barcodemaxcount()
        {
            int _barcodemaxcount = 0;
            string sqlbar_maxcount = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodeMaxInput'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbar_maxcount);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _barcodemaxcount);
            }
            return _barcodemaxcount;

        }
        public int get_barcode_imagesize()
        {
            int _barcodesize = 0;
            string sqlbar_size = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodeImagesize'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbar_size);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _barcodesize);
            }
            return _barcodesize;
        }
        private DataSet _getbookids(bool _checkunique, int _book_masterid)
        {
            string sql_getids = "";
            DataSet ds_bookids = new DataSet();
            if (_checkunique)
            {
                sql_getids = "select tblbooks.Id from tblbooks where tblbooks.BookId=" + _book_masterid + "";
                ds_bookids = m_MysqlDb.ExecuteQueryReturnDataSet(sql_getids);
            }
            else
            {
                sql_getids = "select tblbooks.BookId from tblbooks where tblbooks.BookId=" + _book_masterid + "";
                ds_bookids = m_MysqlDb.ExecuteQueryReturnDataSet(sql_getids);
            }
            return ds_bookids;
        }
        public int get_barcodetype_copies(int master_id)
        {
            int BR_type = 0;
            string sqlbar_size = "select tblbookmaster.BarcodeType from tblbookmaster where tblbookmaster.Id=" + master_id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbar_size);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out BR_type);
            }
            return BR_type;
        }
        public string barcode_copies_nonunique(int master_bookid)
        {
            string _barcode_text = "";
            string sqlbar_size = "select tblbookmaster.Barcode from tblbookmaster where tblbookmaster.Id=" + master_bookid + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sqlbar_size);
            if (m_MyReader.HasRows)
            {
                _barcode_text = m_MyReader.GetValue(0).ToString();
            }
            return _barcode_text;
        }
        public void AddBookcopies_Automatic(int _Book_Id, string _Barcode,int br_type)
        {
            if (br_type == 3)
            {
                for (int i = 1; (BarcodeExistinMaster(_Barcode) || BarcodeExistinCopies(_Barcode)); i++)
                {
                    _Barcode = "" + i.ToString() + "" + _Barcode;
                }
            }
            string sql = "update tblbooks set Barcode='" + _Barcode + "' where tblbooks.Id=" + _Book_Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        public string generate_Br_input_add_copies(int book_id, out string err_msg)
        {
            err_msg = "";
            string _barcode_input = "";
            int _digitcount = 0;
            string _barcode_zeros = "";
            bool _check_automatic = Isbarcodetype_automatic();
            string _barcodeprefix = Get_barcodeprefix();
            int min_barcodecount = get_barcodemincount();
            int _prefix_length = _barcodeprefix.Length;
            int _required_digits = 0;


            _digitcount = book_id.ToString().Length;
            _required_digits = min_barcodecount - (_prefix_length + _digitcount);
            if (_required_digits > 0)
            {
                for (int i = 1; i <= _required_digits; i++)
                {
                    _barcode_zeros = _barcode_zeros + "0";
                }
                _barcode_input = _barcodeprefix + _barcode_zeros + book_id;
                _barcode_zeros = "";

            }
            else
            {
                _barcode_input = _barcodeprefix + book_id;

            }
            return _barcode_input;
        }
        public DataSet Generate_barcodeinput(int bookmaster_id, out string _errmsg)
        {
            _errmsg = "";
            string _barcode_input = "";
            string book_id = "";
            int _digitcount = 0;
            string _barcode_zeros = "";
            bool _check_automatic = Isbarcodetype_automatic();
            bool _is_unique = UniqueNumber();
            string _barcodeprefix = Get_barcodeprefix();
            int min_barcodecount = get_barcodemincount();
            int _prefix_length = _barcodeprefix.Length;
            int _required_digits = 0;

            DataSet ds_bookids = new DataSet();
            ds_bookids = _getbookids(_is_unique, bookmaster_id);
            DataSet ds_barcode = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Bookid");
            dt.Columns.Add("Barcodetext");
            DataRow dr2 = null;
            if (_check_automatic)
            {
                if (ds_bookids != null && ds_bookids.Tables[0] != null && ds_bookids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_bookids.Tables[0].Rows)
                    {
                        book_id = dr[0].ToString();
                        _digitcount = book_id.Length;
                        _required_digits = min_barcodecount - (_prefix_length + _digitcount);
                        if (_required_digits > 0)
                        {
                            for (int i = 1; i <= _required_digits; i++)
                            {
                                _barcode_zeros = _barcode_zeros + "0";
                            }
                            _barcode_input = _barcodeprefix + _barcode_zeros + book_id;
                            dr2 = dt.NewRow();
                            dr2["Bookid"] = book_id;
                            dr2["Barcodetext"] = _barcode_input;
                            dt.Rows.Add(dr2);
                            _barcode_zeros = "";

                        }
                        else
                        {
                            _barcode_input = _barcodeprefix + book_id;
                            dr2 = dt.NewRow();
                            dr2["Bookid"] = book_id;
                            dr2["Barcodetext"] = _barcode_input;
                            dt.Rows.Add(dr2);

                        }
                    }
                    ds_barcode.Tables.Add(dt);
                }
                else
                {
                    _errmsg = "Not found realative books";
                }
            }
            else
            {
                DataSet ds_manual = new DataSet();
                ds_manual = _getbook_Barcode_manual(bookmaster_id);
                if (ds_manual != null && ds_manual.Tables[0] != null && ds_manual.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr3 in ds_manual.Tables[0].Rows)
                    {
                        book_id = dr3["Id"].ToString();
                        _barcode_input = dr3["Barcode"].ToString();
                        dr2 = dt.NewRow();
                        dr2["Bookid"] = book_id;
                        dr2["Barcodetext"] = _barcode_input;
                        dt.Rows.Add(dr2);
                    }
                    ds_barcode.Tables.Add(dt);
                }
                else
                {
                    _errmsg = "Not found manual realative books";
                }
            }
            return ds_barcode;

        }
        public bool _ishorizontal()
        {
            int _horizontal = 0;
            bool _Ishorizontal = true;
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='Ishorizontal'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _horizontal);
                if (_horizontal != 1)
                {
                    _Ishorizontal = false;
                }
            }
            return _Ishorizontal;
        }
        private bool _Ispdfpage_custom()
        {
            int _custom = 0;
            bool _Iscustom = false;
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='pageisCustom'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _custom);
                if (_custom == 1)
                {
                    _Iscustom = true;
                }
            }
            return _Iscustom;
        }
        public void Get_pdfpagetype(out double _pagewidth, out double _pageheight, out string _error)
        {
            _error = "";
            _pagewidth = 0;
            _pageheight = 0;
            string _pagetype = "";
            if (_Ispdfpage_custom())
            {
                string sqlcustom = "select tbllibconfig.Value1,tbllibconfig.Value2 from tbllibconfig where tbllibconfig.FieldName='pagesize'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sqlcustom);
                if (m_MyReader.HasRows)
                {
                    double.TryParse(m_MyReader.GetValue(0).ToString(), out _pageheight);
                    double.TryParse(m_MyReader.GetValue(1).ToString(), out _pagewidth);
                    if (_pageheight == 0 || _pagewidth == 0)
                    {
                        _error = "please check page height and width in configuration";
                    }
                }
            }
            else
            {
                string sql_default = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='BarcodePagetype'";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql_default);
                if (m_MyReader.HasRows)
                {
                    _pagetype = m_MyReader.GetValue(0).ToString();
                    if (_pagetype.ToString().Equals("A4", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _pageheight = 842;
                        _pagewidth = 595;
                    }
                    //for pagetype=A3
                    else if (_pagetype.ToString().Equals("A3", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _pageheight = 1190;
                        _pagewidth = 842;
                    }
                    //for pagetype=A5
                    else if (_pagetype.ToString().Equals("A5", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _pageheight = 595;
                        _pagewidth = 420;
                    }
                       //Default page size A4
                    else
                    {
                        _pageheight = 842;
                        _pagewidth = 595;
                    }
                }
                else
                {
                    _error = "please check page type in configuration";
                }
            }
        }
        public bool _Is_text_needed()
        {
            int _need = 0;
            bool _Isneeded = true;
            string sql = "select tbllibconfig.Value1 from tbllibconfig where tbllibconfig.FieldName='NeedBarcodeText'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _need);
                if (_need != 1)
                {
                    _Isneeded = false;
                }
            }
            return _Isneeded;
        }
        //add book details for automatic barcode
        public void AddBooks_Automatic(int _Book_Id, string _Barcode)
        {
            for(int i=1;(BarcodeExistinMaster(_Barcode) || BarcodeExistinCopies(_Barcode));i++)
            {
                _Barcode = "" + i.ToString() + "" + _Barcode;
            }
            string sql = "update tblbooks set Barcode='" + _Barcode + "' where tblbooks.Id=" + _Book_Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        public void Update_Barcode_automatic(int _Book_Id, string _Barcode)
        {
         
            string sql = "update tblbooks set Barcode='" + _Barcode + "' where tblbooks.BookId=" + _Book_Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        }
        public void Addbarcode_bookmaster(int bookid_master, string _Barcode,out string change_BR)
        {
            change_BR = "";
            for (int i = 1; (BarcodeExistinMaster(_Barcode)); i++)
            {
                _Barcode = "" + i.ToString() + "" + _Barcode;
            }
            change_BR = _Barcode;
            string sql = "update tblbookmaster set Barcode='" + _Barcode + "' where tblbookmaster.Id=" + bookid_master + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        }
        private DataSet _getbook_Barcode_manual(int _book_masterid)
        {
            string sql_getids = "";
            DataSet ds_barcodemanual = new DataSet();
            sql_getids = "select tblbooks.Id,tblbooks.Barcode from tblbooks where tblbooks.BookId=" + _book_masterid + "";
            ds_barcodemanual = m_MysqlDb.ExecuteQueryReturnDataSet(sql_getids);
            return ds_barcodemanual;
        }
        public string Get_Book_Name(int book_id)
        {
            string Book_Name = "";
            string sql_bkname = "select tblbookmaster.BookName FROM tblbookmaster where tblbookmaster.Id=" + book_id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql_bkname);
            if (m_MyReader.HasRows)
            {
                Book_Name = m_MyReader.GetValue(0).ToString();
            }
            return Book_Name;
        }

    }

}
