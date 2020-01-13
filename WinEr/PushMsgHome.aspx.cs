using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WinEr
{
    public partial class PushMsgHome : System.Web.UI.Page
    {
        private OdbcDataReader m_MyReader = null;
        public MysqlClass m_MysqlDb;

        //private OdbcDataReader m_MyReader;
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        //private OdbcDataReader MyReader = null;
        private DataSet MydataSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(404))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    // MysmsMang.InitClass();
                    //Load_DrpClass();
                    //sai added

                    Load_Class();
                    PnlCls.Style.Add("display", "none");
                    Lbl_OnradioSelect.Text = "MESSAGE FOR ALL";
                    Lbl_subMsg.Text = "This Message will send to every registered Apps on your institution";
                    string SchId = Request.Cookies["WIN#SD#$APP"].Value;

                    Lbl_school_ID.Text = "School ID for App. Registration is : " + SchId + " ";
                    //
                    // Load_DrpTamplate();
                    // LoadNativeLanguageArea();
                    if (Session["ClassId"] != null)
                    {
                        int ClassId;
                        if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
                        {
                            //  RdBtLstSelectCtgry1.SelectedIndex = 1;
                            //  Panel_class.Visible = true;
                            // Chkb_class.SelectedValue = ClassId.ToString();

                        }
                    }
                    // Lnk_Retry.Visible = false;
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt.TimeOfDay;
                }
            }

        }
        public static string ConvertDataTableToHTML(DataTable dt)
        {

            string html = "<table id='myTable' class='display' cellspacing='0' width='100%'>";
            //add header row
            html += "<thead><tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</thead></tr>";
            html += "<tbody>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "<tbody>";
            html += "</table>";
            return html;
        }
        private void Load_Class()
        {
            Chkb_class.Items.Clear();
            ListItem li = new ListItem();

            MydataSet = MyUser.MyAssociatedClass();

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = new DataTable();

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = dr[1].ToString();
                    item.Value = dr[0].ToString();
                    //item.Selected = Convert.ToBoolean(dr["IsSelected"]);
                    Chkb_class.Items.Add(item);
                }
            }
            else
            {
                li = new ListItem("No Class present", "-1");
                Chkb_class.Items.Add(li);
            }
            //Chkb_class.SelectedIndex = 0;
        }
        protected void Rdb_CheckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_OnradioSelect.Text = "";
            sucsMsg.Visible = false;
            errmsg.Visible = false;
            wrningMsg.Visible = false;

            if (Rdb_CheckType.SelectedValue == "0")//all
            {
                Lbl_OnradioSelect.Text = "MESSAGE FOR ALL";
                Lbl_subMsg.Text = "This Message will send to every registered Apps on your institution";
                Panel_class.Visible = false;
                Panel_Send_Content.Visible = true;
                BtnNextdnt.Visible = false;
                Panel_students.Visible = false;
                PnlCls.Style.Add("display", "none");
            }
            else if (Rdb_CheckType.SelectedValue == "1")//selected class
            {
                Lbl_OnradioSelect.Text = "MESSAGE FOR SPECIFIC CLASSES";
                Lbl_subMsg.Text = "This Message will send to all students of selected classes";
                Panel_class.Visible = true;
                BtnNextdnt.Visible = false;
                Panel_students.Visible = false;
                PnlCls.Style.Add("display", "block");
                PnlClsAlign.Style.Add("left", "33%");
            }
            else if (Rdb_CheckType.SelectedValue == "2")//selected students
            {
                Lbl_OnradioSelect.Text = "MESSAGE FOR SPECIFIC STUDENTS";
                Lbl_subMsg.Text = "This Message will send to selected Students";
                BtnNextdnt.Visible = true;
                Panel_class.Visible = true;
                Panel_students.Visible = true;
                PnlCls.Style.Add("display", "block");
                PnlClsAlign.Style.Add("left", "0");
            }
        }
        protected void LoadStudntClick(object sendr, EventArgs e)
        {
            Chkb_Studnt.Enabled = true;
            Panel_students.Visible = true;
            Chkb_Studnt.Items.Clear();
            bool ItemSelected = false;
            List<String> SelectdClassList = new List<string>();
            foreach (ListItem item in Chkb_class.Items)
            {
                if (item.Selected)
                {
                    SelectdClassList.Add(item.Value);
                    ItemSelected = true;
                }
            }
            if (ItemSelected == true)
            {
                String SelectClassItem = String.Join(",", SelectdClassList.ToArray());
                if (!(SelectClassItem == ""))
                {
                    string sql = "SELECT tblstudent.StudentName,tblstudent.Id FROM tblstudent inner join tbldevice on tbldevice.DeviceName = tblstudent.OfficePhNo WHERE tblstudent.LastClassId IN (" + SelectClassItem + ");";
                    MydataSet = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt1 = new DataTable();
                        foreach (DataRow dr in MydataSet.Tables[0].Rows)
                        {
                            ListItem item = new ListItem();
                            item.Text = dr[0].ToString();
                            item.Value = dr[1].ToString();
                            //item.Selected = Convert.ToBoolean(dr["IsSelected"]);
                            Chkb_Studnt.Items.Add(item);
                        }
                    }
                    else
                    {
                        string am = "No students founds";
                        Chkb_Studnt.Items.Add(am);
                        Chkb_Studnt.Enabled = false;
                        //Chkb_Studnt.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                string am = "Select class first";
                Chkb_Studnt.Items.Add(am);
                Chkb_Studnt.Enabled = false;
            }   //Chkb_Studnt.SelectedIndex = 0;
        }


        protected void PushSendClick(object sendr, EventArgs e)
        {
            string studAdNo = "";
            sucsMsg.Visible = false;
            errmsg.Visible = false;
            wrningMsg.Visible = false;
            if (Drp_MsgType.SelectedValue == "0")
            {
                Lbl_err.Text = "Please select the type of notification to send";
                errmsg.Visible = true;
            }
            else
            {

                string Subject = Txt_Subject.Text, NotiType = Drp_MsgType.SelectedValue, MsgContent = Txt_MsgContent.Text;//message data
                string ImageLink = "";
                string DocLink = "";

                string UserName = MyUser.LoginUserName;
                string Audience = ""; //ALL_STUDENTS//SELECTED_CLASS //SELECTED_STUDENTS
                if (Rdb_CheckType.SelectedValue == "0")
                {
                    String SelectClassItem = "";
                    Audience = "ALL_STUDENTS";
                    //string DeviceIDFromDB = MysmsMang.GetPushDeviceList(Audience, SelectClassItem);
                    DataSet DeviceIDFromDB = MysmsMang.GetPushDeviceList(Audience, SelectClassItem);
                    if (DeviceIDFromDB.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable ff in DeviceIDFromDB.Tables)
                        {
                            foreach (DataRow dr in ff.Rows)
                            {
                                studAdNo = dr["AdmitionNo"].ToString();
                                string StudentName = dr["StudentName"].ToString();
                                string GardianName = dr["GardianName"].ToString();
                                string DeviceUniqueId = dr["DeviceUniqueId"].ToString();
                                string DeviceOs = dr["DeviceOs"].ToString();
                                // string status = SendPushNotification(StudentName, GardianName, DeviceUniqueId, DeviceOs, Subject, NotiType, MsgContent);//sending to payload
                                string status = SendPushNotification(DeviceUniqueId, DeviceOs, NotiType, GardianName, UserName, ImageLink, DocLink, MsgContent, Subject, StudentName);
                                updateDb(studAdNo, MsgContent, NotiType, Subject);
                                MsgFinalStatus(status);
                            }
                        }

                    }
                    else
                    {
                        Lbl_err.Text = "No registered devices found";
                        errmsg.Visible = true;
                    }
                }
                else if (Rdb_CheckType.SelectedValue == "1")//selected class
                {
                    Audience = "SELECTED_CLASS";
                    List<String> SelectdClassList = new List<string>();
                    bool selected = false;
                    foreach (ListItem item in Chkb_class.Items)
                    {
                        if (item.Selected)//class id
                        {
                            SelectdClassList.Add(item.Value);
                            selected = true;
                        }
                    }
                    if (selected == false)
                    {
                        Lbl_err.Text = "Please select a class";
                        errmsg.Visible = true;
                    }
                    else
                    {
                        String SelectdClassListItems = String.Join(",", SelectdClassList.ToArray());
                        DataSet DeviceIDFromDB = MysmsMang.GetPushDeviceList(Audience, SelectdClassListItems);

                        if (DeviceIDFromDB.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataTable ff in DeviceIDFromDB.Tables)
                            {
                                foreach (DataRow dr in ff.Rows)
                                {
                                    studAdNo = dr["AdmitionNo"].ToString();
                                    string StudentName = dr["StudentName"].ToString();
                                    string GardianName = dr["GardianName"].ToString();
                                    string DeviceUniqueId = dr["DeviceUniqueId"].ToString();
                                    string DeviceOs = dr["DeviceOs"].ToString();
                                    //string status = SendPushNotification(StudentName, GardianName, DeviceUniqueId, DeviceOs, Subject, NotiType, MsgContent);//sending to payload
                                    string status = SendPushNotification(DeviceUniqueId, DeviceOs, NotiType, GardianName, UserName, ImageLink, DocLink, MsgContent, Subject, StudentName);
                                    updateDb(studAdNo, MsgContent, NotiType, Subject);
                                    MsgFinalStatus(status);
                                }
                            }

                        }
                        else
                        {
                            Lbl_err.Text = "No registered devices found";
                            errmsg.Visible = true;
                        }
                    }
                }
                else if (Rdb_CheckType.SelectedValue == "2")//selected students
                {
                    Audience = "SELECTED_STUDENTS";
                    List<String> SelectdStudList = new List<string>();
                    bool selectedCls = false;
                    bool selectedStdnt = false;
                    foreach (ListItem item in Chkb_class.Items)
                    {
                        if (item.Selected)
                        {
                            SelectdStudList.Add(item.Value);
                            selectedCls = true;
                        }
                    }
                    foreach (ListItem item in Chkb_Studnt.Items)
                    {
                        if (item.Selected)
                        {
                            SelectdStudList.Add(item.Value);
                            selectedStdnt = true;
                        }
                    }
                    if (!(selectedCls == true))
                    {
                        //errmsg.Style.Add("display", "block");
                        Lbl_err.Text = "Please select Class";
                        errmsg.Visible = true;

                    }
                    else if (!(selectedStdnt == true))
                    {
                        //errmsg.Style.Add("display", "block");
                        Lbl_err.Text = "Please select Student";
                        errmsg.Visible = true;

                    }
                    else
                    {

                        String SelectdStudListItem = String.Join(",", SelectdStudList.ToArray());
                        DataSet DeviceIDFromDB = MysmsMang.GetPushDeviceList(Audience, SelectdStudListItem);

                        if (DeviceIDFromDB.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataTable ff in DeviceIDFromDB.Tables)
                            {
                                foreach (DataRow dr in ff.Rows)
                                {
                                    studAdNo = dr["AdmitionNo"].ToString();
                                    string StudentName = dr["StudentName"].ToString();
                                    string GardianName = dr["GardianName"].ToString();
                                    string DeviceUniqueId = dr["DeviceUniqueId"].ToString();
                                    string DeviceOs = dr["DeviceOs"].ToString();
                                    string status = SendPushNotification(DeviceUniqueId, DeviceOs, NotiType, GardianName, UserName, ImageLink, DocLink, MsgContent, Subject, StudentName);
                                    updateDb(studAdNo, MsgContent, NotiType, Subject);
                                    //string status = SendPushNotification(StudentName, GardianName, DeviceUniqueId, DeviceOs, Subject, NotiType, MsgContent);//sending to payload
                                    MsgFinalStatus(status);
                                }
                            }

                        }
                        else
                        {
                            Lbl_err.Text = "No registered devices found";
                            errmsg.Visible = true;
                        }

                    }

                }
            }
        }


        private string GetStudentName(string phno)
        {
            string StdntName = "", sql = "";
            sql = "SELECT tblstudent.StudentName FROM tblstudent WHERE tblstudent.Status = 1 and  tblstudent.OfficePhNo = '" + phno + "'";
            m_MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    StdntName = m_MyReader.GetValue(0).ToString();

                }
            }
            return StdntName;
        }
        private string GetParentName(string phno)
        {
            string PrntName = "", sql = "";
            sql = "SELECT tblstudent.GardianName FROM tblstudent WHERE tblstudent.Status = 1 and  tblstudent.OfficePhNo = '" + phno + "'";
            m_MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    PrntName = m_MyReader.GetValue(0).ToString();
                }
            }
            return PrntName;
        }
        private void MsgFinalStatus(string status)
        {
            if (status != "")
            {
                var obj = JObject.Parse(status);
                string s = (string)obj["success"];
                string f = (string)obj["failure"];
                if (f == "0")
                {
                    sucsMsg.Visible = true;
                    Lbl_succesMsg.Text = "All " + s + " mesages sent Successfully";
                }
                else if ((f == "0") && (s == "0"))
                {
                    Lbl_wrningMsg.Text = "Cloud server not reachable.Try again later";
                    wrningMsg.Visible = true;
                }
                else if ((f != "0") && (s == "0"))
                {
                    Lbl_err.Text = "Sending Failed.No any mesages send";
                    errmsg.Visible = true;
                }
                else
                {
                    wrningMsg.Visible = true;
                    Lbl_wrningMsg.Text = "" + s + " messages send succesfully.And " + f + " messages failed to send.";
                }
            }
            else
            {
                Lbl_err.Text = "Sending Failed.No any mesages send";
                errmsg.Visible = true;
            }

        }

        public string SendPushNotification(string DeviceID, string DeviceOs, string NotiType, string ParentName, string NotificFrom, string ImageLink, string DocLink, string messageData, string title, string StudentName)
        {
            // var DemoPhone = ConfigurationManager.AppSettings["DemoPhone"];
            // string image = "";
            // string doc = "";
            string RtnStr = "";
            if (DeviceOs == "iOS")
            {
                var message = new
                {
                    to = DeviceID,
                    notification = new
                    {
                        title = "Winceron International School",//school name
                        body = " Hi " + ParentName + ", " + messageData + "",//half of body notiication.
                    },
                    priority = "high",
                    content_available = true,
                    notId = DateTime.Now.ToString("yyMMddHHmmssff"),
                    data = new
                    {
                        title = "WinEr School",
                        message = " Hi " + ParentName + ", " + messageData + "",
                        notHead = title,
                        date = DateTime.Now.ToString() + " IST",
                        notiType = NotiType, //general, fees, attendance, homeWork,promotional, timetable
                        notiFrom = NotificFrom,//sender name
                        relStudnt = StudentName,//NAme of student message related
                        notId = DateTime.Now.ToString("yyMMddHHmmssff")
                        // image = image,//image online public link
                        //  NotiDoc = doc//online http link of file//google drive etc.
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(message);
                RtnStr = sendMessage(json);
            }
            else if (DeviceOs == "Android")
            {
                //var message = new
                //{
                //    to = DeviceID,
                //    data = new
                //    {
                //        title = "Scool360",
                //        notHead = title,
                //        message = " Hi " + ParentName + ", " + messageData + "",

                //        style = "inbox",//inbox,picture
                //       // picture = "",
                //        summaryText = "Hi new notification regarding your child",
                //        force_start = '1',
                //        content_available = '1',
                //        date = DateTime.Now.ToString() + " IST",
                //        notiType = NotiType, //general, fees, attendance, homeWork,promotional, timetable
                //        notiFrom = NotificFrom,//sender name
                //        relStudnt = StudentName,//NAme of student message related
                //        notId = DateTime.Now.ToString("yyMMddHHmmssff")
                //        // image = image,//image online public link
                //        //  NotiDoc = doc//online http link of file//google drive etc.
                //        // notiVideo = "I9ba-WmVWco",//online http link of file//google drive etc.
                //        // notiAudio = "",//online http link of file//google drive etc.
                //        // notiOthData = "This is data1 data",//Any other data.
                //    }

                //};

                var message = new
                {
                    to = DeviceID,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = " Hi " + ParentName + ", " + messageData + "",
                        title = "Scool360",
                        badge = 1
                    },
                };




                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(message);
                json = json.Replace("content_available", "content-available").Replace("force_start", "force-start");
                RtnStr = sendMessage(json);

                // MyUser.m_DbLog.LogToDb(MyUser.UserName, "PUSH MESSAGE (MOBILE APP) TO PARENT : " + ParentName, "Message : " + json, 1);
            }
            else if (DeviceOs == "web")
            {
                var message = new
                {
                    registration_ids = DeviceID,
                    data = new
                    {
                        title = "WinEr School",
                        notHead = title,
                        message = " Hi " + ParentName + ", " + messageData + "",
                        ledColor = "[0, 0, 255, 0]",
                        style = "picture",//inbox,picture
                        //   picture = "http://winer.in/files/koppal-fort.jpg",
                        summaryText = "Hi new notification regarding your child",
                        force_start = '1',
                        content_available = '1',
                        date = DateTime.Now.ToString() + " IST",
                        notiType = NotiType, //general, fees, attendance, homeWork,promotional, timetable
                        notiFrom = NotificFrom,//sender name
                        relStudnt = StudentName,//NAme of student message related
                        notId = DateTime.Now.ToString("yyMMddHHmmssff")
                        // image = image,//image online public link
                        //  NotiDoc = doc//online http link of file//google drive etc.
                        // notiVideo = "I9ba-WmVWco",//online http link of file//google drive etc.
                        //notiAudio = "",//online http link of file//google drive etc.
                        // notiOthData = "This is data1 data",//Any other data.
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(message);
                json = json.Replace("content_available", "content-available").Replace("force_start", "force-start");
                RtnStr = sendMessage(json);
                // MyUser.m_DbLog.LogToDb(MyUser.UserName, "PUSH MESSAGE (MOBILE APP) TO PARENT : " + ParentName, "Message : " + json, 1);
            }
            return RtnStr;
        }

        public void updateDb(string studId, string msgCntnt, string notiType, string subject)
        {
            string sql = "";

            sql = "insert into tblpushmsg (StudAdmNo,message,notitype,subject) values('" + studId + "','" + msgCntnt + "','" + notiType + "','" + subject + "')";
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);

        }


        protected string sendMessage(string json)
        {
            string RtnStr = "";
            try
            {

                string applicationID = ConfigurationManager.AppSettings["FireBaseAppID"];
                string senderId = ConfigurationManager.AppSettings["FirebaseSenderID"];
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                RtnStr = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RtnStr = ex.Message.ToString();
            }
            return RtnStr;
        }
    }
}