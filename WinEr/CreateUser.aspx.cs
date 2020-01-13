using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using WinEr;
using WinBase;

public partial class CreateUser : System.Web.UI.Page
{
    private KnowinUserAction MyUserAction;
    private OdbcDataReader MyReader = null;
    private SchoolClass objSchool = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        KnowinUser User = (KnowinUser)Session["UserObj"];
        MyUserAction = User.GetUserActionObj();
        if (MyUserAction == null)
        {
            Response.Redirect("Default.aspx");
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
            //some initlization
            if (!IsPostBack)
            {
                Panel4.Visible = false;
                AddRoleToCbList(0);
            }
        }
    }
    private void AddRoleToCbList(int _intex)
    {

        Drp_UserRole.Items.Clear();
        string sql = "SELECT Id,RoleName FROM tblrole WHERE Type <> 'Staff'";
        MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_UserRole.Items.Add(li);
                Drp_UserRole.SelectedIndex = _intex;
            }
        }

    }
    protected void Panel2_Load(object sender, EventArgs e)
    {
        if (int.Parse(Drp_Loginright.SelectedValue.ToString()) == 1)
        {
            Panel2.Visible = true;
        }
        else
        {
            Panel2.Visible = false;
        }
    }
   
    protected void Btn_CreateUser_Click(object sender, EventArgs e)
    {
        int UserId=-1;
        if (Txt_Username.Text.Trim() == "" || Txt_email.Text.Trim() == "" || Txt_LoginName.Text.Trim() == "")
        {
            Lbl_FailureNote.Text = "One Or more fields are empty";
        }
        else
        {
            if (MyUserAction.ValidadLoginName(Txt_LoginName.Text.ToString()))
            {
                if (MyUserAction.VaidateEmail(Txt_email.Text.ToString()))
                {

                    if (int.Parse(Drp_Loginright.SelectedValue.ToString()) == 1)
                    {
                        if (Txt_Password1.Text != "" && Txt_Password1.Text == Txt_Password2.Text)
                        {
                            UserId=MyUserAction.CreateNewUser(Txt_LoginName.Text.ToString(), Txt_Password1.Text.ToString(), Txt_email.Text.ToString(), Txt_Username.Text.ToString(), int.Parse(Drp_UserRole.SelectedValue), int.Parse(Drp_Loginright.SelectedValue));

                            if (LkBtn_UserDetails.Text == "Hide Details")
                            {
                                MyUserAction.AddDetails(Txt_Address.Text, Txt_Dob.Text, Txt_Ph.Text);

                            }
                            Lbl_FailureNote.Text = "User is Created";
                            clear();
                        }
                        else if (Txt_Password1.Text.Trim() == "")
                        {
                            Lbl_FailureNote.Text = "Please enter a valide password...";
                        }
                        else
                        {
                            Lbl_FailureNote.Text = "Password mismatch...";
                        }
                    }
                    else
                    {

                        UserId=MyUserAction.CreateNewUser(Txt_LoginName.Text.ToString(), Txt_Password1.Text.ToString(), Txt_email.Text.ToString(), Txt_Username.Text.ToString(), int.Parse(Drp_UserRole.SelectedValue), int.Parse(Drp_Loginright.SelectedValue));
                        if (LkBtn_UserDetails.Text == "Hide Details")
                        {
                            MyUserAction.AddDetails(Txt_Address.Text, Txt_Dob.Text, Txt_Ph.Text);

                        }
                        Lbl_FailureNote.Text = "User is Created";
                        
                    }
                }
                else
                {
                    Lbl_FailureNote.Text = "Please enter a valide Email id...";
                }
                Txt_UserId.Text = UserId.ToString();
                Panel4.Visible = true;
                Panel1.Visible = false;
                Panel5.Visible = false;
            }
                        
            else
            {
                Lbl_FailureNote.Text = "Login Name entered is already present, please try another..";
            }
        }
        
        
        
    }

    
    protected void LkBtn_UserDetails_Click(object sender, EventArgs e)
    {
        if (Panel3.Visible == false)
        {
            Panel3.Visible = true;
            LkBtn_UserDetails.Text = "Hide Details";

        }
        else
        {
            Panel3.Visible = false;
            LkBtn_UserDetails.Text = "Add Details";

        }
    }
    protected void Txt_Username_TextChanged(object sender, EventArgs e)
    {
       
    }
    void clear()
    {
        Txt_Username.Text = "";
        Txt_Password1.Text = "";
        Txt_Ph.Text = "";
        Txt_email.Text = "";
        Txt_Dob.Text = "";
        Txt_Address.Text = "";
        Txt_LoginName.Text = "";
    }

    protected void Drp_Loginright_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Upload_Click(object sender, EventArgs e)
    {
        if (Txt_UserId.Text.Trim() == "")
        {
            Lbl_FailureNote.Text = "The user should be created first";
        }
        else if (FileUp_Image.PostedFile != null && !ValidImageFile())
        {
            Lbl_Image.Text = "File type cannot be uploaded";
            Lbl_FailureNote.Text = "";
        }
        else
        {
            AddPhoto(int.Parse(Txt_UserId.Text.ToString()));
            Lbl_Image.Text = "Image is Uploaded";
            Lbl_FailureNote.Text = "";
            Panel1.Visible = true;
            Panel5.Visible = true;
            Panel4.Visible = false;
            UserClear();
        }
    }

    private void UserClear()
    {
        Txt_Username.Text = "";
        Txt_LoginName.Text = "";
        Txt_email.Text = "";
        Txt_Address.Text = "";
        Txt_Dob.Text = "";
        Txt_Ph.Text = "";
    }
    private void AddPhoto(int _UserId)
    {

        string ImageUrl = "";
        String Sql = "";
        string ImageName = FileUp_Image.FileName.ToString();
        FileUp_Image.SaveAs(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + ImageName);
        // string strVirtualPath="http://localhost:1334/WinSchool/UpImage/" + ImageName;
        string ThumbnailPath = (WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\ThumbnailImages\\" + "User" + _UserId.ToString() + ImageName);
        using (System.Drawing.Image Img = System.Drawing.Image.FromFile(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + ImageName))
        {
            Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 150);
            using (System.Drawing.Image ImgThnail =
            new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
            {
                ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                ImgThnail.Dispose();
            }
            Img.Dispose();
           
        }
     
        ImageUrl = "User" + _UserId + ImageName;
        Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _UserId + ", '" + ImageUrl + "','UserImage')";
        MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
        if (ImageName != "")
        {
            File.Delete(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\UpImage\\" + ImageName);
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
        string fileExtension = System.IO.Path.GetExtension(FileUp_Image.FileName).ToLower();
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
    protected void BttnCancel_Click(object sender, EventArgs e)
    {
        UserClear();
    }
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        Panel5.Visible = true;
        Panel4.Visible = false;
        Lbl_FailureNote.Text = "";
        UserClear();
    }
}
