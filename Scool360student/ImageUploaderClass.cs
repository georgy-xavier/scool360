using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WinBase;
using System.Data;
using System.IO;


namespace Scool360student
{

    public class ImageUploaderClass
    {
        private SchoolClass objSchool = null;
        private static string connectionString = null;

         public ImageUploaderClass(SchoolClass obj)
        {
            if (obj != null)
            {
                //Server=localhost;Database=srinidhidb;User=root;Password=win
                connectionString = getMysqlConnectionString(obj.ConnectionString);
            }
            else
            {
                connectionString = getMysqlConnectionString("");
            }

        }

        private string getMysqlConnectionString(string odbcConnectionString)
        {
            string MySqlConString = "";
            if (String.IsNullOrEmpty(odbcConnectionString))
            {
                odbcConnectionString = WinerUtlity.SingleSchoolConnectionString;
            }
            string[] connectionParameters = odbcConnectionString.Split(';');

            if (connectionParameters.Length > 0)
            {
                MySqlConString = "Server=" + connectionParameters[1].Split('=')[1] + ";";
                MySqlConString = MySqlConString + "Database=" + connectionParameters[2].Split('=')[1] + ";";
                MySqlConString = MySqlConString + "Uid=" + connectionParameters[3].Split('=')[1] + ";";
                MySqlConString = MySqlConString + "Pwd=" + connectionParameters[4].Split('=')[1] + ";";
            }
            return MySqlConString;


        }

        internal bool UpdateImageFile(byte[] imagebytes, int Id, string type)
        {
            try
            {
                MySqlConnection _mySqlConn = new MySqlConnection(connectionString);

                string sql = "UPDATE tblfileurl SET FileBytes=@imgage WHERE UserId=@Id AND Type=@ImageType";

                MySqlCommand m_cmd = new MySqlCommand(sql, _mySqlConn);
                m_cmd.Parameters.AddWithValue("@imgage", imagebytes);
                m_cmd.Parameters.AddWithValue("@Id", Id);
                m_cmd.Parameters.AddWithValue("@ImageType", type);

                if (_mySqlConn.State == ConnectionState.Closed)
                    _mySqlConn.Open();
                m_cmd.ExecuteNonQuery();
                _mySqlConn.Close();
                _mySqlConn.Dispose();
                return true;
            }
            catch
            {
                return false;
            }

        }

        internal bool InsertImageFile(byte[] imagebytes, int Id, string imgType)
        {
            try
            {
                MySqlConnection _mySqlConn = new MySqlConnection(connectionString);

                string sql = "INSERT INTO tblfileurl(UserId,FileBytes,Type) VALUES(@Id ,@imgage,@imgType)";
                MySqlCommand m_cmd = new MySqlCommand(sql, _mySqlConn);
                m_cmd.Parameters.AddWithValue("@Id", Id);
                m_cmd.Parameters.AddWithValue("@imgage", imagebytes);
                m_cmd.Parameters.AddWithValue("@imgType", imgType);
              
                if (_mySqlConn.State == ConnectionState.Closed)
                    _mySqlConn.Open();
                m_cmd.ExecuteNonQuery();
                _mySqlConn.Close();
                _mySqlConn.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public byte[] getImageBytes(int Id, string imgType)
        {
            MySqlConnection _mySqlConn = new MySqlConnection(connectionString);
            MySqlCommand command = null;
            try
            {
                 byte[] image =null;

                if(imgType=="Logo")
                {
                    command = new MySqlCommand("select tblschooldetails.Logo  from tblschooldetails ", _mySqlConn);
                }
                else if (imgType == "SchoolImage")
                {
                    command = new MySqlCommand("select tblschooldetails.SchoolImages  from tblschooldetails", _mySqlConn);
                }
                else if (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage")
                {
                    command = new MySqlCommand("select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId=" + Id + " and tblfileurl.`Type` ='" + imgType + "'", _mySqlConn);

                }
             

                 if (_mySqlConn.State != ConnectionState.Open)
                     _mySqlConn.Open();
                MySqlDataReader _reader = command.ExecuteReader();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                       image= (byte[])_reader.GetValue(0);
                       
                    }
                }
                _mySqlConn.Close();

                if (image == null && (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage"))
                {
                    command = new MySqlCommand("select tblfileurl_history.FileBytes from tblfileurl_history where tblfileurl_history.UserId=" + Id + " and tblfileurl_history.`Type` ='" + imgType + "'", _mySqlConn);

                    if (_mySqlConn.State != ConnectionState.Open)
                        _mySqlConn.Open();
                    _reader = command.ExecuteReader();
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (_reader["FileBytes"] != null)
                                image = (byte[])_reader["FileBytes"];

                        }
                    }
                    _mySqlConn.Close();

                }
                if ((image == null || image.Length<=0) && (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage"))
                {
                    // dominic. suppose we are not getting any images byte from history also, 
                    //then we are return a default image reading from tblfileurl 
                    command = new MySqlCommand("select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId=0 and tblfileurl.`Type` ='DefaultImage'", _mySqlConn);

                    if (_mySqlConn.State != ConnectionState.Open)
                        _mySqlConn.Open();
                    _reader = command.ExecuteReader();
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (_reader["FileBytes"] != null)
                                image = (byte[])_reader["FileBytes"];

                        }
                    }
                    _mySqlConn.Close();
                }
                       
                     
               
                _mySqlConn.Clone();
               return image;
            }
            catch
            {
                if (_mySqlConn.State!= ConnectionState.Closed)
                {
                    _mySqlConn.Close();
                }

                return null;

            }
            
        }


        public byte[] getImageBytesStud(int Id, string imgType, int classid)
        {
            MySqlConnection _mySqlConn = new MySqlConnection(connectionString);
            MySqlCommand command = null;
            try
            {
                byte[] image = null;

                if (imgType == "Logo")
                {
                    command = new MySqlCommand("select tblschooldetails.Logo  from tblschooldetails ", _mySqlConn);
                }
                else if (imgType == "SchoolImage")
                {
                    command = new MySqlCommand("select tblschooldetails.SchoolImages  from tblschooldetails", _mySqlConn);
                }
                else if (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage")
                {
                    command = new MySqlCommand("select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId=" + Id + " and tblfileurl.`Type` ='" + imgType + "' and tblfileurl.class=" + classid + "", _mySqlConn);

                }


                if (_mySqlConn.State != ConnectionState.Open)
                    _mySqlConn.Open();
                MySqlDataReader _reader = command.ExecuteReader();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        image = (byte[])_reader.GetValue(0);

                    }
                }
                _mySqlConn.Close();

                if (image == null && (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage"))
                {
                    command = new MySqlCommand("select tblfileurl_history.FileBytes from tblfileurl_history where tblfileurl_history.UserId=" + Id + " and tblfileurl_history.`Type` ='" + imgType + "'and tblfileurl.class=" + classid + "", _mySqlConn);

                    if (_mySqlConn.State != ConnectionState.Open)
                        _mySqlConn.Open();
                    _reader = command.ExecuteReader();
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (_reader["FileBytes"] != null)
                                image = (byte[])_reader["FileBytes"];

                        }
                    }
                    _mySqlConn.Close();

                }
                if ((image == null || image.Length <= 0) && (imgType == "StaffImage" || imgType == "StudentImage" || imgType == "UserImage"))
                {
                    // dominic. suppose we are not getting any images byte from history also, 
                    //then we are return a default image reading from tblfileurl 
                    command = new MySqlCommand("select tblfileurl.FileBytes from tblfileurl where tblfileurl.UserId=0 and tblfileurl.`Type` ='DefaultImage'", _mySqlConn);

                    if (_mySqlConn.State != ConnectionState.Open)
                        _mySqlConn.Open();
                    _reader = command.ExecuteReader();
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (_reader["FileBytes"] != null)
                                image = (byte[])_reader["FileBytes"];

                        }
                    }
                    _mySqlConn.Close();
                }



                _mySqlConn.Clone();
                return image;
            }
            catch
            {
                if (_mySqlConn.State != ConnectionState.Closed)
                {
                    _mySqlConn.Close();
                }

                return null;

            }

        }



        internal void UpdateSchoolLogo(byte[] imagebytes,string Txt)
        {
            try
            {
                MySqlConnection _mySqlConn = new MySqlConnection(connectionString);

                string sql = "";
                if (Txt == "1")
                    sql = "UPDATE tblschooldetails SET Logo=@imgage";
                else
                    sql = " UPDATE tblschooldetails SET SchoolImages=@imgage";
                

                MySqlCommand m_cmd = new MySqlCommand(sql, _mySqlConn);
                m_cmd.Parameters.AddWithValue("@imgage", imagebytes);

                if (_mySqlConn.State == ConnectionState.Closed)
                    _mySqlConn.Open();
                m_cmd.ExecuteNonQuery();
                _mySqlConn.Close();
                _mySqlConn.Dispose();

            }
            catch(Exception ex)
            {
                string dffd = ex.Message;
            }
        }

       
    }
}
