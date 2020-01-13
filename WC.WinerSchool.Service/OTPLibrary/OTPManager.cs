using OTPLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Net;
using System.Data;

namespace OTPLibrary
{
    public class OTPManager
    {
        private List<int> used = new List<int>();
        private Random random = new Random();
        private int ExpiringMinutes = 10;
        private string mSendURL = "http://122.166.5.17/desk2web/SendSMS.aspx?UserName=($UN$)&password=($PS$)&MobileNo=($MN$)&SenderID=($SID$)&CDMAHeader=($CID$)&Message=($SMS$)&isFlash=($FLS$)";
        private string musername = "";
        private string mpassword = "";
        private string numberadd = "";
        private string mDeliveryStatus = "Delivered";
        private DAL myDAL;

        public OTPManager(string _connectionString, int _ExpiringMinutes = 10)
        {
            myDAL = new DAL(_connectionString);
            ExpiringMinutes = _ExpiringMinutes;
        }

        /// <summary>
        /// For generating OTP
        /// </summary>
        /// <param name="entityId">entityId</param>
        /// <returns>int</returns>
        public int GenerateOTP(OTPclass otpvalues)
        {
            OTPDomain entity = new OTPDomain
            {
                Entityname = otpvalues.username,
                EntityId = otpvalues.entityid,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(ExpiringMinutes),
                phonenumber = otpvalues.phonenumber
            };
            entity.msg = myDAL.msgtemplate(entity);
            entity.Value = GenerateRandomOTP();
            entity.OTPId = myDAL.InsertOTP(entity);
            SentOTP(entity);

            return entity.OTPId;

        }       

        /// <summary>
        /// Validate user entry
        /// </summary>
        /// <param name="entityId">entityId</param>
        /// <param name="OTPId">OTPId</param>
        /// <param name="value">value</param>
        /// <returns>bool</returns>
        //public bool Validate(int entityId, int OTPId, int value)
       public bool Validate(OTPclass otpvalues)
        {
            OTPDomain entity = new OTPDomain { 
            EntityId = otpvalues.entityid,
            Entityname = otpvalues.username,
            currenttime=otpvalues.currenttime,
            enterdotp=otpvalues.enterdotp
            //OTPId = otpvalues.OTPId,
            //Value = otpvalues.value
            };

            return myDAL.ValidateOTP(entity);
        }

        /// <summary>
        /// For resenting OTP
        /// </summary>
        /// <param name="entityId">entityId</param>
        /// <param name="OTPId">OTPId</param>
        /// <returns>int</returns>
        //public int ReSendOTP(int entityId, int OTPId)
       public int ReSendOTP(OTPclass otpvalues)
        {
            OTPDomain entity = new OTPDomain
            {
                Entityname = otpvalues.username,
                phonenumber = otpvalues.phonenumber,
                EntityId = otpvalues.entityid
                //OTPId = OTPId,
            };
            entity.msg = myDAL.msgtemplate(entity);
            OTPDomain retreivedOTP = myDAL.GetOTP(entity);
            if (retreivedOTP.ExpiresAt > DateTime.Now)
            {
                SentOTP(retreivedOTP); 
            }
            else
            {
                retreivedOTP.OTPId = GenerateOTP(otpvalues);
                
            }
           
            return retreivedOTP.OTPId;
        }

        private int GenerateRandomOTP()
        {          
            int current = random.Next(1000, 9999);

            while (used.Contains(current))
            {
                current = random.Next(1000, 9999);
            }
                
            used.Add(current);

            if(used.Count>50)
            {
                used.RemoveAt(0);
            }

            return current;
        }

        private void SentOTP(OTPDomain entity)
        {
            string message = "";
            string OTP=entity.Value.ToString();
            if (OTP == "0")
            {
                message = entity.msg.Replace("($OTP$)", entity.retrievedotp);
            }
            else
            {
                 message = entity.msg.Replace("($OTP$)", OTP);
            }
            //string message = entity.Value.ToString();
            string phonenumber = entity.phonenumber;
            string SenderId = "WINER";
            musername = myDAL.getusername();
            mpassword = myDAL.getpassword();
            numberadd = myDAL.numadd();
            mSendURL = myDAL.url();
            string url = mSendURL;
            url = url.Replace("($UN$)", musername);
            url = url.Replace("($PS$)", mpassword);
            url = url.Replace("($CID$)", phonenumber);
            url = url.Replace("($SID$)", SenderId);
            //if (entity.retrievedotp == null)
            //{
                url = url.Replace("($SMS$)", message);
            //}
            //else
            //{
            //    url = url.Replace("($SMS$)", entity.retrievedotp);
            //}
            url = url.Replace("($MN$)", phonenumber);
            url = url.Replace("($FLS$)", "true");
            sendsms(url);
            







            //TODO: Send SMS and Email
        }
        private bool sendsms(string URL)
        {
           string mSMSReply = "";
           string mError = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                mSMSReply = reader.ReadToEnd();
                if (mSMSReply.ToUpperInvariant().Contains(mDeliveryStatus.ToUpperInvariant()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
