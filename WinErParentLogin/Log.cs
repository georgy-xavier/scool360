using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace UpService
{
     class Log
    {

        private static string m_sFilePath = ConfigurationSettings.AppSettings["LogPath"];
        private static string m_strFileName = "Parent_Login.log";
        private static long mLevel = 5;
        private static long mFilesize =100000;
        public static void LogMessage(long Level, String str)
        {
            try
            {
                FileStream m_fsLogFile;
                StreamWriter m_swLogFile;

                if (mLevel < Level)
                {
                    string LogStr = String.Format("{0} - {1}", DateTime.Now, str);
                    if (!File.Exists(m_sFilePath + m_strFileName))
                    {
                        //string newPath = System.IO.Path.Combine("C:\\", "Log");
                        System.IO.Directory.CreateDirectory(m_sFilePath);
                    }
                    else
                    {
                        //Log.LogMessage(6, m_sFilePath + m_strFileName);  
                        FileInfo _fInfo = new FileInfo(m_sFilePath + m_strFileName);
                        //Log.LogMessage(6, _fInfo.Length .ToString());  
                        if (_fInfo.Length > mFilesize)
                        {
                            File.Move(m_sFilePath + m_strFileName, m_sFilePath + string.Format("LogFile-{0:yyyy-MM-dd_hh-mm-ss-tt}.log", DateTime.UtcNow));
                        }

                    }
                    
                    m_fsLogFile = new FileStream(m_sFilePath + m_strFileName, FileMode.Append, FileAccess.Write);
                    m_swLogFile = new StreamWriter(m_fsLogFile);
                    m_swLogFile.WriteLine(LogStr);
                    m_swLogFile.Close();
                    m_fsLogFile.Close();
                }
            }
            catch
            {
               
                
            }

        }

    }
}
