using OTPLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace OTPLibrary
{
    public class DAL
    {
        private OdbcConnection _MyODBCConn;
        public DAL(string connectionString)
        {
            _MyODBCConn = new OdbcConnection(connectionString);
            _MyODBCConn.Open();
        }

        public int InsertOTP(OTPDomain entity)
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;

            //string dd = ConfigurationSettings.AppSettings["ConnectionInfo"];
            //con = new MySqlConnection(dd);
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);

            string sql = "";
            string sql1 = "";
            string sql2 = "";
            int userid = entity.EntityId;
            string username = entity.Entityname;
            string created = entity.CreatedAt.ToString();
            string expires = entity.ExpiresAt.ToString();
            string otp = entity.Value.ToString();

            sql2 = "delete from tblotp where UserName = '" + username + "'";
            //cmd = new MySqlCommand(sql2, con);
            //cmd.ExecuteNonQuery();
            m_MyReader = _MysqlDb.ExecuteQuery(sql2);


            sql = "insert into tblotp(UserId,UserName,Createdat,Expiresat,OTP) values(" + userid + ",'" + username + "','" + created + "','" + expires + "','"+otp+"')";
           // cmd = new MySqlCommand(sql, con);
            //cmd.ExecuteNonQuery();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            sql1 = "select Id from tblotp where UserName='" + username + "' and Createdat='" + created + "' and OTP='" + otp + "'";
            m_MyReader = _MysqlDb.ExecuteQuery(sql1);

            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                string otpid = m_MyReader.GetValue(0).ToString();
                return int.Parse(otpid);
            }

            else
                return 1;
            
           
        }

        public bool ValidateOTP(OTPDomain entity)
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;
            string sql = "";
            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);
            //cmd = con.CreateCommand();
            sql = "select Id from tblotp where UserName='" + entity.Entityname + "' and OTP='" + entity.enterdotp + "' and Expiresat>'" + entity.currenttime + "'";
            //MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //adap.Fill(ds);
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                return true;
                //string sql = "";
                //sql = "select Id from tblotp where UserName='" + entity.Entityname + "' and OTP='" + entity.enterdotp + "' and Expiresat>'" + entity.currenttime + "'";
                //cmd = new MySqlCommand(sql, con);
                //string id = cmd.ExecuteNonQuery().ToString();
                //if (id == "")
                //{
                //    return false;
                //}
                //// TODO: Check whether same OTP exist and it is not expired
                //else
            }
            else
            return false;
        }
        public string msgtemplate(OTPDomain entity)
        {
            OdbcDataReader m_MyReader = null;
            string msg="";
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);

            string sql = "SELECT Templatevalue FROM tblotptemplate WHERE Id=1";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                msg = m_MyReader.GetValue(0).ToString();
            }

            return msg;
        }
        public string getusername()
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();
            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);

            string str = "";
            string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='UserName'";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                str = m_MyReader.GetValue(0).ToString();
            }
            return str;
            
        }
        public string url()
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();
            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);
            string str = "";
            string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='SendURL'";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                str = m_MyReader.GetValue(0).ToString();
            }
            return str;

        }
        public string getpassword()
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);
            string str = "";
            string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Password'";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                str = m_MyReader.GetValue(0).ToString();
            }
            return str;

        }
        public string numadd()
        {
            OdbcDataReader m_MyReader = null;
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);
            string str = "";
            string sql = "SELECT Value FROM tblsmsconfig WHERE `Type`='Need Additional Number'";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                str = m_MyReader.GetValue(0).ToString();
            }
            return str;

        }
        public OTPDomain GetOTP(OTPDomain entity)
        {
            OdbcDataReader m_MyReader = null;
            string username = entity.Entityname;
            MySqlConnection con;
            MySqlCommand cmd;

            //con = new MySqlConnection("Data Source=localhost;Database=rajivdb;User ID=root;Password=win");
            //con.Open();

            MysqlClasslib _MysqlDb = new MysqlClasslib(_MyODBCConn);
            string str = "";
            string sql = "select OTP,Expiresat from tblotp   where UserName='" + username + "' order by Id DESC LIMIT 1";
            //cmd = new MySqlCommand(sql, con);
            //MySqlDataReader MyReader;
            //MyReader = cmd.ExecuteReader();
            m_MyReader = _MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                m_MyReader.Read();
                str = m_MyReader.GetValue(0).ToString();
                string expires = m_MyReader.GetValue(1).ToString();
                entity.retrievedotp = str;
                entity.ExpiresAt = Convert.ToDateTime(expires);
            }
           
            // TODO: get related OTP data from database
            return entity;
        }
    }
}
