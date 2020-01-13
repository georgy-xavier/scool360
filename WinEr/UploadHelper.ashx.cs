using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;

namespace TakingMyPicture.Web
{
    /// <summary>
    /// Summary description for UploadHelper
    /// </summary>
    /// 


    public class UploadHelper : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            //   string UserName = Convert.ToString(HttpContext.Current.Session["StudentId"]);
            string _Id = "", _SaveType = "" ;
            if (HttpContext.Current.Session["SaveType"] != null)
            {
                _SaveType = HttpContext.Current.Session["SaveType"].ToString();
                if (_SaveType != "")
                {

                    if (HttpContext.Current.Session[_SaveType] != null)
                    {
                        _Id = HttpContext.Current.Session[_SaveType].ToString();
                    }
                    string file = _SaveType+ _Id + ".jpg";
                    string uploadPath = context.Server.MapPath("~/UpImage/");
                    string ErrorMessage = "";
                    string _Encstring = context.Request.QueryString["ref"];
                    byte[] Dec = System.Convert.FromBase64String(_Encstring);
                    string Dncstring = System.Text.Encoding.Unicode.GetString(Dec);

                    if (Dncstring == "winceron")
                    {
                        try
                        {
                            if (File.Exists(uploadPath + file))
                            {
                                File.Delete(uploadPath + file);
                            }
                            using (FileStream stream = File.Create(uploadPath + file))
                            {

                                byte[] bytes = new byte[4096]; // assuming the file size will not be more than 4 MB, the default size of the request

                                int bytesToRead = 0;

                                while ((bytesToRead =

                                context.Request.InputStream.Read(bytes, 0, bytes.Length)) != 0)
                                {

                                    stream.Write(bytes, 0, bytesToRead);

                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = ex.Message;
                        }
                        if (File.Exists(uploadPath + file))
                        {
                            context.Response.Write("success");
                        }
                        else
                        {

                            context.Response.Write("File could not be saved. Please contact administrator. Message : " + ErrorMessage);
                        }

                    }
                    else
                    {
                        context.Response.Write("Un authorized user!");
                    }

                }
                else
                {
                    context.Response.Write("Unidentified user type!");
                }
            }
            else
            {
                context.Response.Write("Unidentified user type!");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}