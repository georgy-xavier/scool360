﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WinBase;


namespace WinEr.Handler
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private SchoolClass objSchool = null;
        ImageUploaderClass imgobj;
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request.QueryString["type"].ToString();

            string id = context.Request.QueryString["id"].ToString();
            string classid = context.Request.QueryString["class"].ToString();

            if (WinerUtlity.NeedCentrelDB())
            {
                if (objSchool == null && HttpContext.Current.Session[WinerConstants.SessionSchool] != null)
                {
                    objSchool = (SchoolClass)HttpContext.Current.Session[WinerConstants.SessionSchool];
                }
            }
            if (imgobj == null)
            {
                imgobj = new ImageUploaderClass(objSchool);
            }
            byte[] Photo = imgobj.getImageBytesStud(int.Parse(id), type, int.Parse(classid));
            if (Photo != null && Photo.Length > 0)
            {
                HttpResponse response = context.Response;

                response.OutputStream.Write(Photo, 0, Photo.Length);
                response.Flush();
                //context.Response.BinaryWrite((byte[])Photo);
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