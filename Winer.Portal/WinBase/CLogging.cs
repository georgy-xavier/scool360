using System;
using System.Web;
//using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Collections.Generic;

public class CLogging
{
    public enum PriorityEnum { LEVEL_VERY_IMPORTANT = 1, LEVEL_IMPORTANT, LEVEL_MEDIUM_IMPORTANT, LEVEL_LESS_IMPORTANT, LEVEL_DEBUG, LEVEL_DETAIL };

    struct LogStruct
    {

        public DateTime dTime;
        public string lThreadId;
        public string sUser;
        public string Message;
        public char Type;
    };
    //private Queue<LogStruct> FileLog= new Queue<LogStruct>;
    //privete Queue<LogStruct> DbQLog;
    private Queue<LogStruct> FileLog = null;
    public static int m_iSysPriority = 10;
    private static CLogging m_SLogging = null;
    public static string m_sFilePath = null;
    private bool m_bStop = false;
    private static int m_iRecyclePeriod = 30; //File will be deleted after 30 days .
    private static int m_iRecycleSize = (1024*1000); //New File is created 
    private Thread workerThread = null;
    //private string m_UserName = null;
    private static bool m_ThreadStopped = false;
    private static string m_strFileName = "LogFile.log";
    FileStream m_fsLogFile;
    StreamWriter m_swLogFile;
    // Keep construtor as private.This will avoid direct Class creation. 
    public bool StopLog
    {
        set
        {
            m_bStop = true;
        }

    }
    public bool Initialize()
    {
        if (workerThread != null)
        {
            //worker thread is already running just return true
            return true;
        }

        ThreadStart st = new ThreadStart(LoggingThread);
        workerThread = new Thread(st);

        FileLog = new Queue<LogStruct>();
        // set flag to indicate worker thread is active

        
        return true;
    }
    public bool StartLogger()
    {

        //rename old File if already exist
        /*           check m_sFilePath + m_strFileName file exist
         * if exist rename the file as FilePath+Winlog_dateTimestamp.log
         
        */       //File.
        File_rename(m_sFilePath + m_strFileName, m_sFilePath, true);
        // start the thread
        workerThread.Start();
        return true;

    }

 /*   public static int SysPriority
    {
        get
        {
            return m_iSysPriority;
        }
        set
        {
            m_iSysPriority = SysPriority;
        }
    }
*/
  /*  public static string FilePath
    {
        get
        {
            return m_sFilePath;
        }
        set
        {
            m_sFilePath = FilePath;
        }
    }
*/
    public static CLogging GetLogObject()
    {
        if (m_SLogging == null)
        {
            if (m_ThreadStopped == true) return null;

            m_SLogging = new CLogging();
            if (m_SLogging.Initialize() == false)
                m_SLogging = null;
        }
        return m_SLogging;
    }
    private void LoggingThread()
    {
        LogStruct LoggItem;
        while (m_bStop == false)
        {
            if (FileLog.Count > 0)
            {
                LoggItem = FileLog.Dequeue();
                string LogStr;
                //LogStr = LoggItem.dTime.ToString("DD/MM/YYYY HH:MI:SS");
                LogStr = LoggItem.dTime.ToString("dd-MM-yyyy HH:mm:ss") + "\t" + LoggItem.lThreadId + "\t" + LoggItem.Type + "\t" + LoggItem.sUser + "\t" + LoggItem.Message;
                //Windows.Forms.MessageBox.Show(LoggItem.Message);
                m_fsLogFile = new FileStream(m_sFilePath + m_strFileName, FileMode.Append, FileAccess.Write);
                m_swLogFile = new StreamWriter(m_fsLogFile);
                m_swLogFile.WriteLine(LogStr);
                m_swLogFile.Close();
                m_fsLogFile.Close();

                //Check File size . If it is over the m_recycle size then rename and recreate file.
            }
            else
            {
                Thread.Sleep(2000);
                File_rename(m_sFilePath + m_strFileName, m_sFilePath, false);
            }
            if (m_bStop==false )
            Thread.Sleep(100); 
        }

        m_ThreadStopped = true;
        m_SLogging = null;
    }

    private bool CreateLogs(string strLogPath)
    {
        try
        {
            m_fsLogFile = new FileStream(strLogPath, FileMode.Create, FileAccess.ReadWrite);
            m_swLogFile = new StreamWriter(m_fsLogFile);
            m_swLogFile.WriteLine("************************ Win Log **************************");
            m_swLogFile.WriteLine("LogTime \t ThreadID \t Type \t User \t Message\n");
            m_swLogFile.Close();
            m_fsLogFile.Close();

        }
        catch 
        {
            //Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }


    public static int RecyclePeriod
    {
        get
        {
            return m_iRecyclePeriod;
        }
        set
        {
            m_iRecyclePeriod = RecyclePeriod;
        }
    }

    public void LogToDb(string StrMessage, char Type)
    {
        throw new System.NotImplementedException();
    }
    /*Log the message to File. Type . E-Error. I- Information,W-Warning.
    Priority (1-10) highest priority is 1  ****/
    public void LogToFile(string functionStr, string StrMessage, char Type, PriorityEnum Priority, string UserName)
    {
        LogStruct LoggeItem;
        int prio = (int)Priority;
        if (m_iSysPriority >= prio)
        {
            LoggeItem = new LogStruct();
            LoggeItem.dTime = DateTime.Now;
            LoggeItem.Type = Type;
            LoggeItem.lThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            LoggeItem.Message = functionStr+ " " + StrMessage;
            LoggeItem.sUser = UserName;
            FileLog.Enqueue(LoggeItem);
        }

    }

    private CLogging()
    {
        //throw new System.NotImplementedException();
        if (m_sFilePath == null)
        {
            m_sFilePath = "C:\\";
        }

    }

    ~CLogging()
    {
        FileLog = null;
        //throw new System.NotImplementedException();
    }

    private int File_rename(string FilepathOld, string FilepathNew, bool Force)
    {
        FileInfo fi = new FileInfo(FilepathOld);
        string NewFileName = FilepathNew + "Win" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".Log";
        if (fi.Exists)
        {
            if ((Force) || (fi.Length >= m_iRecycleSize))
            {
                fi.MoveTo(NewFileName);
                CreateLogs(m_sFilePath + m_strFileName);
            }
        }
        return 1;
    } 

}
