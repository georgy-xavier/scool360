using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Configuration;

namespace WinBase
{
    public class RegistrationClass
    {

        public MysqlClass m_MysqlDb;
       // private WinEncryption MyEncription = new WinEncryption();
        private OdbcDataReader m_MyReader = null;
        private string m_ConnectionStr = "";
        OdbcDataReader Myreader = null;
        public RegistrationClass(string _ConnectionStr)
        {
            m_ConnectionStr = _ConnectionStr;
        }
        

        public DataSet GetReligion()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet ReligionDs = new DataSet();
            string sql = "";
            sql = "SELECT Id,Religion FROM tblreligion where Religion <>'Other' ";
            ReligionDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return ReligionDs;
        }

        public DataSet GetCaste()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet CasteDs = new DataSet();
            string sql = "";
            sql = "select tblcast.Id, tblcast.castname from tblcast  where tblcast.castname <>'Other' order by tblcast.castname";
            CasteDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return CasteDs;
        }

        public DataSet GetBloodGroup()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet BloodGroupDs = new DataSet();
            string sql = "";
            sql = "SELECT Id,GroupName FROM tblbloodgrp";
            BloodGroupDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return BloodGroupDs;
        }

        public DataSet GetMotherTongue()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet MotherTongueDs = new DataSet();
            string sql = "";
            sql = "SELECT Id,Language FROM tbllanguage";
            MotherTongueDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return MotherTongueDs;
        }

        public DataSet GetStandard()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet StandardDs = new DataSet();
            string sql = "";
            sql = "SELECT DISTINCT tblstandard.Id, tblstandard.Name from tblstandard";
            StandardDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return StandardDs;
        }

        public DataSet GetCurrentBatch()
        {
            string sql = "";
            DataSet CurrentDS = new DataSet();
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            sql = "SELECT Id,BatchName FROM tblbatch WHERE Created=1 AND Status=1";
            CurrentDS = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return CurrentDS;
        }

        public DataSet GetNextBatch(int CurrentBatchId)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            DataSet NextBatchDs = new DataSet();
            string sql = "";
            int Id = CurrentBatchId + 1;
            sql = "SELECT Id,BatchName FROM tblbatch WHERE Id=" + Id + "";
            NextBatchDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            m_MysqlDb.CloseConnection();
            return NextBatchDs;
        }

        private static int DEFAULT_MIN_PASSWORD_LENGTH = 6;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 8;
        public int[] image = new int[6];
        public string usercode = "";
        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        //private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        //private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "1234567890abcefgijkmnopqrstwABCDEFGHMNSTWXYZ@%";
        //private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";



        public void GenerateDynamicPassword(out string _pwd)
        {
            _pwd = "";
            _pwd = Generate();

        }

        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH);
        }

        public static string Generate(int minLength,
                                     int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] 
        {
         
            PASSWORD_CHARS_NUMERIC.ToCharArray()
         
         
        };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;


            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;


            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];


            int nextCharIdx;


            int nextGroupIdx;


            int nextLeftGroupsOrderIdx;


            int lastCharIdx;


            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;


            for (int i = 0; i < password.Length; i++)
            {

                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];


                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;


                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;

                else
                {

                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }

                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {

                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    lastLeftGroupsOrderIdx--;
                }
            }


            return new string(password);
        }


        private string _Host = ConfigurationSettings.AppSettings["smtp.Host"];
        private string _Id = ConfigurationSettings.AppSettings["smtp.Credentials.Id"];
        private string _Password = ConfigurationSettings.AppSettings["smtp.Credentials.Password"];
        private string _From = ConfigurationSettings.AppSettings["Email_From"];
        private string _To = ConfigurationSettings.AppSettings["Email_To"];

        private void SendMail(int _Id, string _pwd, out string _message)
        {
            _message = "";
            string _CC = "";
            string _Subject = "Partners registration details from Winceron Software Technologies Pvt. Ltd ";
            string _attachment = "";
            string msg = "";
            string sql = "select tbl_reseller.Email, tbl_reseller.Name from tbl_reseller where tbl_reseller.id=" + _Id;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                //string _Body = " Dear " + m_MyReader.GetValue(1).ToString() + ",<br/><br/>Thank you registering our partership .We would like to inform you that your PIN is <b>  " + _pwd + " </b>  for the UserName " + m_MyReader.GetValue(0).ToString() + ".<br> Please login by using  this PIN to confirm and complete your account registration. You can change this  password on your first login. <br>Regards,<br/>Sales Team,<br/>Winceron Software Technologies Pvt.Ltd<br/> Bangalore.<br/><br/>For any enquiry: Visit <a href=\"www.winceron.com\">Winceron.com</a> <br>E-mail : info@winceron.com<br>Phone: 080 2572 5972.<br>";
                string _Body = " Dear Mr./Mrs. " + m_MyReader.GetValue(1).ToString() + ",<br/>Welcome to Winceron Partner network! <br /><br/> Following are your login credentials details to your partner account. <br /><br /> Username: " + m_MyReader.GetValue(0).ToString() + " <br /> Password :" + _pwd + " <br /><br/> Login Link :  <a href=\"http://partners.winceron.com/default.aspx\">http://partners.winceron.com/default.aspx</a> <br /><br/> After successful login please change your password.<br />In case of loss of password, use forgot password option to recover the password, and new password will be send  to your email id.<br /><br/>We recommend that you carefully read the following information. Please print this document and retain it for future use.<br /><br/>If there is anything else we can do for you, be sure to let us know. We are always open to suggestions on how to improve our service.<br /><br /><br /><b><u>WINCERON Reseller Guideline</u></b><br /><br /><b>HOW TO CONTACT US</b><br /><br />Communication is the key to successful collaboration. The following targeted e-mail addresses were set up specifically to improve communication.<br />Please use the appropriate address to enable us to respond to your requests as quickly as possible.<br /><br/>For questions regarding our service, your key generator, or your product settings.<br /><a href=\"support@winceron.com\">support@winceron.com</a><br /><br />For questions regarding payment or accounts.<br /><a href=\"finance@winceron.com\">finance@winceron.com</a><br /><br />For questions regarding customer orders, Sales etc.<br /><a href=\"sales@winceron.com\">sales@winceron.com</a><br /><br />In case of any dispute or issues write to<br /> ss@winceron.com<br /><br /><b>HOW YOUR CUSTOMERS CAN CONTACT US</b><br /><br />In case of any support or any queries on software customer can raise a ticket at <a href=\" http://support.winceron.com/\"> http://support.winceron.com/ </a>.<br />Our Support engineers will reply within 1 business day.  They can also call our direct support numbers from our website for any urgent support issues.<br />Every customer is provided with an ID. It is mandatory to mention customer id and name with every communications<br /> ";
                _Body = _Body + "<br /><b>WINCERON- SALES POLICIES</b><br /><ol><li>Clearly understand the Customer requirement, avoid any false commitments</li><li>Every software should be 100% implementable.</li><li>Increase sales through customer satisfaction and reference.</li></ol><br /><b>CHANNEL PARTNER – RESELLER RESPONSIBILITY</b><br /><br />Following are the major responsibilities of reseller.<br /><ol><li>Clearly understand WINCERON’S Sales Policies.</li><li>Generate lead, meet prospects, Regular follow-up, conversion ,Payment collection.</li><li>Software implementation support and other operation support for customer to achieve 100% implementation.</li></ol><br /><b>SALES PROCESS - BILLING</b><br /><br />Following are the two models of billing which can be performed.<br /><ol><li><i>Billing by WINCERON</i></li><li><i>Billing by Channel Partner.</i></li></ol><br /><i>1. Billing by WINCERON</i><br /><p>In this case the quotation and billing should be directly from WINCERON.<br />Customer shall provide the payment directly to WINCERON. <br />Commission will be paid to channel partner within 30 days of full payment receipt</p><br /><i>2. Billing by Channel Partner.</i><br /><p>In this case WINCERON will bill the reseller and reseller can bill customer directly.<br />WINCERON will release temporary license on receipt of the advance and P.O.<br />Reseller should transfer the full amount with in 2 weeks of installation irrespective of status of<br />payment.Permanent license is released only after the complete payment.</p><br />Reseller should not bill more than 150% of MRP of the software. Any other services such as data entry,<br />implementation support, consultation can be charged extra.<br /><br /><b>MARKETING-SUPPORT</b><br /><br />WINCERON will provide required softwares video demo and other brochures as CD or as emails to resellers.<br />Resellers can create brochures or other marketing documents using WINCERON brochures.<br /><br /><b>SALES- LEAD GENERATION</b><br /><br />Based on the overall performance of resellers WINCERON will provide the sales leads to reseller for their <br />allocated region. This should be kept confidential and at any point should not be passed to any third party.<br /><br /><b>SALES- CONFLICT</b><br /><br />To avoid any sales conflict it is advisable to inform WINCERON on the latest updates of the approached <br />customers and the stage of the sales process. This can be as a weekly or bi-weekly report.<br />";
                _Body = _Body + "<br />In case if two resellers targeting same customer the first one who informed WINCERON will be given the priority.<br />In case of any conflict or disputes WINCERON will try to solve it amicably. Based on case to case WINCERON<br />might split the commission based on the level of the presales.<br /><br /><b>DELIVERY METHOD</b><br /><br />On Receipt of the P.O from customer reseller should send same or raise P.O to WINCERON. On Commercial <br />clearance of P.O WINCERON will inform reseller on the approximate installation date/time.<br /><br />Reseller should collect the required pre-installation information’s from customer. Software will be installed<br />through internet using remote desktop connection tools. Software training is provided through phone and remote<br />desktop.<br /><br />In case of any onsite installation and training from WINCERON, TA & DA to be provided by customer or by the <br />reseller.<br /><br />Any further implementation support such as data entry, consultation etc can be performed and charged extra by <br />the reseller <br /><br />Regards,<br />Sales Team,<br />Winceron Software Technologies Pvt.Ltd<br />Bangalore.";


                Send_Email(m_MyReader.GetValue(0).ToString(), _CC, _Subject, _Body, _attachment, out _message);
            }
            else
            {
                _message = "Cannot send the mail.Please try agiain";
            }


        }

        public void Send_Email(string To, string CC, string Subject, string Body, string Attachment, out string Message)
        {
            Message = "";
            try
            {
                if (To == "")
                {
                    To = _To;
                }
                MailMessage mailMsg = new MailMessage(_From, To);
                if (CC != "")
                {
                    string[] str_cc = CC.Split(',');
                    for (int i = 0; i < str_cc.Length; i++)
                    {
                        mailMsg.CC.Add(new MailAddress(str_cc[i]));
                    }


                }

                mailMsg.Subject = Subject;
                mailMsg.Body = Body;
                //mailMsg.Body = "<html><body>Testing <b>123</b/>....</body></html>";
                mailMsg.IsBodyHtml = true;
                if (Attachment != "")
                {
                    mailMsg.Attachments.Add(new Attachment(Attachment));
                }

                //mailMsg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = _Host;

                smtp.Credentials = new System.Net.NetworkCredential(_Id, _Password);

                smtp.Send(mailMsg);
                Message = "Email has been sent successfully";
                mailMsg.Attachments.Clear();
            }

            catch (Exception ex)
            {
                Message = ex.Message;
            }

        }

        public void EnterDetailsToTable(string studentName, DateTime Dob, string MobileNumber, string emailId, string GuardianName, string  GenderValue, int standard, string Address, int BatchId,string TempId,int Rank)
        {
            string sql = "";
            string _pwd = "";
            int LoginId=0;
            m_MysqlDb = new MysqlClass(m_ConnectionStr);      
            //Name,TempId,Fathername,Gender,DOB,Standard,JoiningBatch,Address,PhoneNumber,Email,CreatedDate,Status,AdmissionStatusId,Rank
            sql = "insert into tbltempstdent(Name,TempId,Fathername,Gender,DOB,Standard,JoiningBatch,Address,PhoneNumber,Email,CreatedDate,Status,AdmissionStatusId,Rank) values('" + studentName + "','" + TempId + "','" + GuardianName + "','" + GenderValue + "','" + Dob.ToString("s") + "'," + standard + "," + BatchId + ",'" + Address + "','" + MobileNumber + "','" + emailId + "','" + System.DateTime.Now.Date.ToString("s") + "',1,1," + Rank + ")";
            m_MysqlDb.ExecuteQuery(sql);
            GenerateDynamicPassword(out _pwd);

            //string _RealPass = MyEncription.Encrypt(_pwd.ToString());
            //InsertDataToAdmissionLogin(_RealPass, MobileNumber, m_MysqlDb, out LoginId);
            sql = "select Id from tbltempstdent where TempId='"+TempId+"'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                sql = "Update tbltempstdent set LoginId="+LoginId +" where Id="+int.Parse(m_MyReader.GetValue(0).ToString())+" and TempId='"+TempId+"'";
                m_MysqlDb.ExecuteQuery(sql);
            }

            m_MysqlDb.CloseConnection();

        }

        private void InsertDataToAdmissionLogin(string _pwd, string MobileNumber,MysqlClass m_MysqlDb,out int LoginId)
        {
            string sql="";
            LoginId = 0;
            sql = "select Id,Password from tbl_admissionlogindetails where UserName='" + MobileNumber + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out LoginId);
            }
            else
            {

                sql = "insert into tbl_admissionlogindetails(UserName,Password) values('" + MobileNumber + "','" + _pwd + "')";
                m_MysqlDb.ExecuteQuery(sql);
                sql = "select Max(Id) from tbl_admissionlogindetails";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int.TryParse(m_MyReader.GetValue(0).ToString(), out LoginId);
                }
            }
          
        
        }

        public int GetMaxIdOfTempStudent()
        {

            int MaxId = 0;
            m_MysqlDb = new MysqlClass(m_ConnectionStr); 
            string sql = "select max(Id) from tbltempstdent";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                bool valid = int.TryParse(m_MyReader.GetValue(0).ToString(), out MaxId);
            }
            m_MysqlDb.CloseConnection();
            return MaxId;

        }

        public string GetPass()
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string pass = "";
            string sql = "select tbl_admissionlogindetails.Password,tbl_admissionlogindetails.Id from tbl_admissionlogindetails where tbl_admissionlogindetails.UserName ='8892459368'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                pass = m_MyReader.GetValue(0).ToString();
            }
            return pass;
        }

        public DataSet GetStudentDetails(int Id)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string sql = "";
            DataSet StudentdetailsDs = new DataSet();
            //DATE_FORMAT( tbltempstdent.DOB, '%d/%m/%Y') As DOB
            sql = "select Id,TempId,Name,Fathername,Gender,DATE_FORMAT( tbltempstdent.DOB, '%d/%m/%Y') As DOB,Standard,JoiningBatch,PhoneNumber,Email,DATE_FORMAT( tbltempstdent.CreatedDate, '%d/%m/%Y') As CreatedDate,`Status` from tbltempstdent where tbltempstdent.LoginId=" + Id + "";
            StudentdetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            StudentdetailsDs = GetDetailsDS(StudentdetailsDs, m_MysqlDb);
            m_MysqlDb.CloseConnection();
            return StudentdetailsDs;
        }

        private DataSet GetDetailsDS(DataSet StudentdetailsDs, MysqlClass m_MysqlDb)
        {
            string sql="";
            if (StudentdetailsDs != null && StudentdetailsDs.Tables[0].Rows.Count > 0)
            {
                StudentdetailsDs.Tables[0].Columns.Add("Standard1");
                StudentdetailsDs.Tables[0].Columns.Add("JoiningBatch1");
                StudentdetailsDs.Tables[0].Columns.Add("Status1");
                foreach (DataRow dr in StudentdetailsDs.Tables[0].Rows)
                {
                    sql="select tblstandard.Name from tblstandard where tblstandard.Id="+int.Parse(dr["Standard"].ToString())+"";
                    m_MyReader=m_MysqlDb.ExecuteQuery(sql);
                    if(m_MyReader.HasRows)
                    {
                        dr["Standard1"] = m_MyReader.GetValue(0).ToString();
                    }
                    sql = "select tblbatch.BatchName from tblbatch where tblbatch.Id=" + int.Parse(dr["JoiningBatch"].ToString()) + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        dr["JoiningBatch1"] = m_MyReader.GetValue(0).ToString();
                    }
                    sql = "select tbl_admissionstatus.`Status` from tbl_admissionstatus where tbl_admissionstatus.Id=" + int.Parse(dr["Status"].ToString()) + "";
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                    if (m_MyReader.HasRows)
                    {
                        dr["Status1"] = m_MyReader.GetValue(0).ToString().ToUpper();
                    }
                    
                }
            }
            return StudentdetailsDs;
        }

        public void UpdatePassword(int Id, string _encriptednewpwd)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string sql = "";
            sql = "Update tbl_admissionlogindetails set tbl_admissionlogindetails.Password='"+_encriptednewpwd+"' where Id="+Id+"";
            m_MysqlDb.ExecuteQuery(sql);
            m_MysqlDb.CloseConnection();
        }

        public void InsertXmlTextToTable(string _TempId, string _StudentXmlString)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string sql = "";
            sql = "insert into tbl_xmlstring(XMLString,TempId,PermanentId,StudentType) values('"+_StudentXmlString+"','"+_TempId+"',0,0)";
            m_MysqlDb.ExecuteQuery(sql);
            m_MysqlDb.CloseConnection();
        }

        public void UpdateXmlTextToTable(string _TempId, string _StudentXmlString)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string sql = "";
            sql = "Update tbl_xmlstring set XMLString='" + _StudentXmlString + "' where TempId='" + _TempId + "'";
            //sql = "insert into tbl_xmlstring(XMLString,TempId,PermanentId,StudentType) values('" + _StudentXmlString + "','" + _TempId + "',0,0)";
            m_MysqlDb.ExecuteQuery(sql);
            m_MysqlDb.CloseConnection();
        }
        public DataSet GetStudentDetailsfromTempId(string Id)
        {
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            string sql = "";
            DataSet StudentdetailsDs = new DataSet();
            //DATE_FORMAT( tbltempstdent.DOB, '%d/%m/%Y') As DOB
            sql = "select tbltempstdent.Id,tbltempstdent.TempId,Name,Fathername,Gender,DATE_FORMAT( tbltempstdent.DOB, '%d/%m/%Y') As DOB,Standard,JoiningBatch,PhoneNumber,Email,DATE_FORMAT( tbltempstdent.CreatedDate, '%d/%m/%Y') As CreatedDate,`AdmissionStatusId`,tbltempstudentfileurl.FileURL as StudentURL ,tbl_admissionstatus.`Status` as AdmissionStatus from tbltempstdent inner join tbltempstudentfileurl on tbltempstudentfileurl.TempId= tbltempstdent.TempId inner join tbl_admissionstatus on tbl_admissionstatus.Id= tbltempstdent.AdmissionStatusId where tbltempstdent.TempId='" + Id + "'";
            StudentdetailsDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            m_MysqlDb.CloseConnection();
            return StudentdetailsDs;
        }

        public void UpdateStudentDetails(string studentName, DateTime Dob, string MobileNumber, string emailId, string GuardianName, string GenderValue, int standard, string Address, int BatchId, string _TempId)
        {
            string sql = "";
            string _pwd = "";
            int LoginId = 0;
            m_MysqlDb = new MysqlClass(m_ConnectionStr);
            //Name,TempId,Fathername,Gender,DOB,Standard,JoiningBatch,Address,PhoneNumber,Email,CreatedDate,Status,AdmissionStatusId,Rank
            //sql = "insert into tbltempstdent(Name,TempId,Fathername,Gender,DOB,Standard,JoiningBatch,Address,PhoneNumber,Email,CreatedDate,Status,AdmissionStatusId,Rank) values('" + studentName + "','" + TempId + "','" + GuardianName + "','" + GenderValue + "','" + Dob.ToString("s") + "'," + standard + "," + BatchId + ",'" + Address + "','" + MobileNumber + "','" + emailId + "','" + System.DateTime.Now.Date.ToString("s") + "',1,1," + Rank + ")";
            sql = "Update tbltempstdent Set Name='" + studentName + "',Fathername='" + GuardianName + "',Gender='" + GenderValue + "',DOB='" + Dob.ToString("s") + "',Standard=" + standard + ",JoiningBatch=" + BatchId + ",Address='" + Address + "',PhoneNumber='" + MobileNumber + "',Email='" + emailId + "',CreatedDate='" + System.DateTime.Now.Date.ToString("s") + "' where TempId='"+_TempId+"'"; 
            m_MysqlDb.ExecuteQuery(sql);
            m_MysqlDb.CloseConnection();
        }
    }
}
