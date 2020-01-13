using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.Odbc;
using System.IO;
using WinBase;

namespace WinEr
{
    public partial class silverlitepicturecapture : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;
        private StaffManager MyStaffMang;
        private SchoolClass objSchool = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }

            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyStaffMang = MyUser.GetStaffObj();
            if (MyStaffMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else
            {
                if (WinerUtlity.NeedCentrelDB())
                {
                    if (Session[WinerConstants.SessionSchool] == null)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                    objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                }
                if (!IsPostBack)
                {

                    imageDisplay();
                }
            }
        }
        public void imageDisplay()
        {
            Image1.ImageUrl = "~/Pics/user6.png";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string _id = "", _SaveType = "" ;

            if (Session["SaveType"] != null)
            {
                _SaveType = Session["SaveType"].ToString();
            }


            if (Session[_SaveType] != null)
            {
                _id = Session[_SaveType].ToString();
            }
            string file = _SaveType+ _id + ".jpg";
            Image1.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool)+"UpImage/" + file;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            string _id = "", _SaveType = "";

            

            if (Image1.ImageUrl == "~/Pics/user6.png")
            {

                if (Session["SaveType"] != null)
                {
                    _SaveType = Session["SaveType"].ToString();
                }


                if (Session[_SaveType] != null)
                {
                    _id = Session[_SaveType].ToString();
                }

                string file = _SaveType + _id + ".jpg";
                if (file != null)
                {
                    string strpath = Server.MapPath(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + file);
                    System.IO.File.Delete(strpath);
                    Label1.Visible = true;
                    Label1.Text = file + "DELETED SUCESSFULLY!";
                }
                else
                {
                    Label1.Text = file + "NOT DELETED SUCESSFULLY!";
                    Image1.ImageUrl = WinerUtlity.GetRelativeFilePath(objSchool)+"UpImage/showimage.jpg";

                }
            }

            else
            {
                Label1.Text = "";
                Label1.Visible = true;
                Label1.Text = "TAKE THE PICTURE!";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string _id = "", _SaveType = "";

            try
            {


                if (Session["SaveType"] != null)
                {
                    _SaveType = Session["SaveType"].ToString();
                }


                if (Session[_SaveType] != null)
                {
                    _id = Session[_SaveType].ToString();
                }

                if (_SaveType == "StudId")
                {
                    AddStudentPhoto(_id);


                }
                else if (_SaveType == "StaffId")
                {
                    AddStaffPhoto(_id);


                }

            }
            catch
            {

            }
            
        }

        private void AddStudentPhoto(string _StudentId)
        {
            string ImageUrl = "";
            String Sql = "";
            string preimage = "";
            int UsrId;
            string ImageName = "StudId" + _StudentId + ".jpg";
            //FileUp_Student.SaveAs(MyUser.FilePath + "\\UpImage\\" +ImageName );
            // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
            string ThumbnailPath = (MyUser.FilePath + "\\ThumbnailImages\\" + "Student" + _StudentId.ToString() + ImageName);
            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(MyUser.FilePath + "\\UpImage\\" + ImageName))
            {
                Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 450);
                using (System.Drawing.Image ImgThnail =
                new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                {
                    ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                    ImgThnail.Dispose();
                }
                Img.Dispose();
                // File.Delete(Server.MapPath("../UpImage/" + ImageName));
                //DeleteFile(strVirtualPath);
            }
            ImageUrl = "Student" + _StudentId + ImageName;

            UsrId = int.Parse(_StudentId);

            if (MyStudMang.HasImage(UsrId, out preimage))
            {
                Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE Type='StudentImage' AND UserId=" + _StudentId;
            }
            else
            {
                Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _StudentId + ", '" + ImageUrl + "','StudentImage')";
            }
            OdbcDataReader MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(Sql);
            if (ImageName != "")
            {
                File.Delete(MyUser.FilePath + "\\UpImage\\" + ImageName);
            }
            if ((preimage != "") && (preimage != ImageUrl))
            {
                File.Delete(MyUser.FilePath + "\\ThumbnailImages\\" + preimage);
            }

        }


        private void AddStaffPhoto(string _StaffId)
        {
            string ImageUrl = "";
            String Sql = "";
            string preimage = "";
            int UsrId;
            string ImageName = "StaffId" + _StaffId + ".jpg";
            //FileUp_Photo.SaveAs(MyUser.FilePath + "\\UpImage\\" + ImageName);
            // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
            string ThumbnailPath = (MyUser.FilePath + "\\ThumbnailImages\\" + "Staff" + _StaffId.ToString() + ImageName);
            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(MyUser.FilePath + "\\UpImage\\" + ImageName))
            {
                Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 450);
                using (System.Drawing.Image ImgThnail =
                new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                {
                    ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                    ImgThnail.Dispose();
                }
                Img.Dispose();
                // File.Delete(Server.MapPath("../UpImage/" + ImageName));
                //DeleteFile(strVirtualPath);
            }
            ImageUrl = "Staff" + _StaffId + ImageName;

            UsrId = int.Parse(_StaffId);
            //dominic change the functions
            if (MyStaffMang.HasImage(UsrId))
            {
                Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE UserId=" + _StaffId + " AND Type='StaffImage'";
            }
            else
            {
                Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _StaffId + ", '" + ImageUrl + "','StaffImage')";
            }
            OdbcDataReader MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
            if (ImageName != "")
            {
                File.Delete(MyUser.FilePath + "\\UpImage\\" + ImageName);
            }
            if ((preimage != "") && (preimage != ImageUrl))
            {
                File.Delete(MyUser.FilePath + "\\ThumbnailImages\\" + preimage);
            }

        }

        private Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
        {
            Size NewSize;
            double tempval;

            if (OriginalHeight > FormatSize && OriginalWidth > FormatSize)
            {
                if (OriginalHeight > OriginalWidth)
                    tempval = FormatSize / Convert.ToDouble(OriginalHeight);
                else
                    tempval = FormatSize / Convert.ToDouble(OriginalWidth);

                NewSize = new Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
            }
            else
                NewSize = new Size(OriginalWidth, OriginalHeight); return NewSize;
        }

        private System.Data.Odbc.OdbcDataReader ExecuteSql(string sql)
        {
            System.Data.Odbc.OdbcConnection m_MyODBCConn = new System.Data.Odbc.OdbcConnection("");
            m_MyODBCConn.Open();
            System.Data.Odbc.OdbcCommand m_cmd = new System.Data.Odbc.OdbcCommand(sql, m_MyODBCConn);
            return m_cmd.ExecuteReader();
        }
    }
}
