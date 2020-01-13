using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using WinBase;
using WinEr;
using RavSoft.GoogleTranslator;
using System.Net;
using System.Net.NetworkInformation;
public partial class SchoolDetails : System.Web.UI.Page
{
    private ConfigManager MyConfiMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private SchoolClass objSchool = null;
//    private DataSet MydataSet;
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];

        if (MyUser.SELECTEDMODE == 1)
        {
            this.MasterPageFile = "~/WinerStudentMaster.master";

        }
        else if (MyUser.SELECTEDMODE == 2)
        {

            this.MasterPageFile = "~/WinerSchoolMaster.master";
        }
        if (WinerUtlity.NeedCentrelDB())
        {
            if (Session[WinerConstants.SessionSchool] == null)
            {
                Response.Redirect("Logout.aspx");
            }
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyConfiMang = MyUser.GetConfigObj();
        if (MyConfiMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(37))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                Lnk_changelogo.Text = "Change Logo";
                LoadSchoolData();
                LoadSchoolLogo();
                loadLanguages();
            }
        }
    }

    private void loadLanguages()
    {
        if (HaveNetConnection())
        {
            drplanguages.DataSource = Translator.Languages.ToArray();
            drplanguages.DataBind();
            setUpdatedLanguageValues();
           // drplanguages.SelectedIndex = 15;
            
        }
        else
        {
             drplanguages.Items.Add(new ListItem("English","Eng"));
        }

    }

    private void setUpdatedLanguageValues()
    {
        string Sql = "SELECT  Value FROM tblconfiguration WHERE Module='SMS' and Name='Native Language'";
        MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(Sql);
        
        if (MyReader.HasRows)
        {
            ListItem l=drplanguages.Items.FindByText(MyReader["Value"].ToString());
            drplanguages.SelectedValue = l.Value;
        }
    }

    private bool HaveNetConnection()
    {
        HttpWebRequest objReq;
        HttpWebResponse objRes;
        try
        {
            objReq = (HttpWebRequest)HttpWebRequest.Create("http://www.google.com");
            objRes = (HttpWebResponse)objReq.GetResponse();
            if (objRes.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }
        catch (WebException ex)
        {
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
  

    private void LoadSchoolLogo()
    {
        //string ImageUrl;
        //string Sql = "SELECT LogoUrl FROM tblschooldetails WHERE Id=1";
        //MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(Sql);
        //if (MyReader.HasRows)
        //{

        //    ImageUrl =WinerUtlity.GetRelativeFilePath(objSchool)+"ThumbnailImages/"+ MyReader.GetValue(0).ToString();
           
        //}
        //else
        //{
        //    ImageUrl = "images/img.png";
        //}

        Img_Holder.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
        Img_schoolimgHolder.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=SchoolImage";

    }

    private void LoadSchoolData()
    {        
        string Sql = "SELECT * FROM tblschooldetails WHERE Id=1";
        MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Txt_SchoolName.Text = MyReader.GetValue(2).ToString();
            Txt_TempName.Text = MyReader.GetValue(2).ToString();
            Txt_address.Text = MyReader.GetValue(3).ToString();
            Txt_TempAddress.Text =  MyReader.GetValue(3).ToString();
            Txt_Syllabus.Text = MyReader.GetValue(4).ToString();
            Txt_MediumOfIns.Text = MyReader.GetValue(5).ToString();
            Txt_Desc.Text = MyReader.GetValue(6).ToString();
        }
    }

    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        if ((Txt_TempName.Text.Trim() != Txt_SchoolName.Text.Trim())||(Txt_TempAddress.Text.Trim()!= Txt_address.Text.Trim()))
        {
            Lbl_UpdateCon.Text = "If you change the SCHOOL NAME or ADDRESS then software license will expire and you will get signed out. Do you want to continue";
            this.MPE_UpdateConfirm.Show();
        }
        else
        {
            UpdateSchoolDetails();
            WC_MessageBox.ShowMssage("School Details Updated");
        }
    }

    protected void Btn_UpdateOk_Click(object sender, EventArgs e)
    {
        UpdateSchoolDetails();
        SignOut();
    }

    private void UpdateSchoolDetails()
    {
        try
        {
            if (Txt_SchoolName.Text.Trim() != "")
            {

                MyConfiMang.AddSchoolDetails(Txt_SchoolName.Text.Replace("\'", "\\'"), Txt_address.Text.Replace("\'", "\\'"), Txt_Syllabus.Text, Txt_MediumOfIns.Text, Txt_Desc.Text,drplanguages.SelectedItem.Text);
                Lbl_UpMessage.Text = "Details Saved";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Update School Details", "School details are updated", 1);  
            }
            else
            {
                Lbl_msg.Text = "Enter the School Name";
                this.MPE_MessageBox.Show();
            }
        }
        catch
        {
            Lbl_msg.Text = "Please refresh the page and try again...";
            this.MPE_MessageBox.Show();
        }
    }

    private void SignOut()
    {
        if (Session["UserObj"] != null)
        {
            KnowinUser User = (KnowinUser)Session["UserObj"];
            User.m_DbLog.LogToDb(User.UserName, "User Logout", "User " + User.UserName + " Loged Out", 1);
            User.Dispose();
            Session["UserObj"] = null;
        }

        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        Response.Redirect("Default.aspx");
    }

    private void AddPhoto()
    {
        try
        {
            string ImageUrl = "";
            string ImageName = FileUp_Photo1.FileName.ToString();

            string path = Server.MapPath("TemporaryFileManager");
            FileUp_Photo1.SaveAs(path + "\\" + ImageName);

            string ThumbnailPath = (path + "\\" + "School" + ImageName);

            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(path + "\\" + ImageName))
            {
                Size ThumbNailSize;
                if(Lbl_hidentxt.Text.ToString()=="1")
                     ThumbNailSize= NewImageSize(Img.Height, Img.Width,500);
                else
                     ThumbNailSize = NewImageSize(Img.Height, Img.Width,1000);
               
                using (System.Drawing.Image ImgThnail =
                new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                {
                    ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                    ImgThnail.Dispose();
                }
                Img.Dispose();

            }
            ImageUrl = "School" + ImageName;
         
            byte[] imagebytes = General.getImageinBytefromImage(ThumbnailPath);

            ImageUploaderClass imgUpload = new ImageUploaderClass(objSchool);

            imgUpload.UpdateSchoolLogo(imagebytes, Lbl_hidentxt.Text.ToString());
           
            if (ImageName != "")
            {
                 File.Delete(path + "\\" + ImageName);
                 File.Delete(path + "\\" + "School" + ImageName);
            }

        }
        catch
        {

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
    
    private bool ValidImageFile()
    {
        bool fileOK = false;
        string fileExtension = System.IO.Path.GetExtension(FileUp_Photo1.FileName).ToLower();
        string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
        for (int i = 0; i < allowedExtensions.Length; i++)
        {
            if (fileExtension == allowedExtensions[i])
            {
                fileOK = true;
            }
        }
        return fileOK;
    }
    
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConfigurationHome.aspx");
    }
    private void Loadpopup(string _type)
    {
        Lbl_hidentxt.Text = "2";
        if (_type == "Logo")
        {
            Lbl_hidentxt.Text = "1";
        }
        lbl_popuptitle.Text = "Upload " + _type;
        lbl_uploadlbl.Text = "Upload " + _type;
        Lbl_UpMessage.Text = "";
        MPE_UploadPhoto.Show();
    }
    protected void Lnk_changelogo_Click(object sender, EventArgs e)
    {
        Loadpopup("Logo");      
    }

   

    protected void IMGBtn_UpLogo_Click(object sender, EventArgs e)
    {
        Loadpopup("Logo");  
    }

    protected void IMGBtn_Uploginimg_Click(object sender, ImageClickEventArgs e)
    {
        Loadpopup("School Image");  
    }

    protected void Lnk_changeschoolimg_Click(object sender, EventArgs e)
    {
        Loadpopup("School Image");  
    }

    protected void Btn_UploadImage_Click(object sender, EventArgs e)
    {
        //string SchoolName = Txt_SchoolName.Text;
        if (FileUp_Photo1.PostedFile != null && !ValidImageFile())
        {
            Lbl_UpMessage.Text = "File type cannot be uploaded";
            //this.MPE_MessageBox.Show();
            MPE_UploadPhoto.Show();
        }
        else if (FileUp_Photo1.PostedFile != null && ValidImageFile())
        {
            AddPhoto();
            Response.Redirect("SchoolDetails.aspx");
        }        
    }



    
}