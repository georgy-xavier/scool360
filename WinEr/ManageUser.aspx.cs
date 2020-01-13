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

public partial class ManageUser : System.Web.UI.Page
{
    private KnowinUserAction MyUserAction;
   
    private OdbcDataReader MyReader = null;
    private KnowinUser User;
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
        User = (KnowinUser)Session["UserObj"];
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
                Panel1.Visible = false;
                Panel7.Visible = false;
                Lbl_Message.Visible = false;
                AddRoleToDrpList();
                SetDrpLogin();
            }
        }
    }
    private void SetDrpLogin()
    {
        if (int.Parse(Drp_Loginright.SelectedValue.ToString()) == 1)
        {
            Panel2.Visible = true;
            LkBtn_Resetpw.Text = "Reset Password";
        }
        else
        {
            Panel2.Visible = false;
        }
    }
    
    private void AddRoleToDrpList()
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
            }
        }
    }
    
    protected void Btn_UpdateUser_Click(object sender, EventArgs e)
    {
        DateTime _DateOfBirth;
        if (Txt_SurName.Text.Trim() == "" || Txt_email.Text.Trim() == "")
        {
            Lbl_FailureNote.Text = "One Or more fields are empty";
        }
        else
        {
            if (MyUserAction.VaidateEmail(Txt_email.Text.ToString()))
            {
                if (int.Parse(Drp_Loginright.SelectedValue.ToString()) == 1 && (LkBtn_Resetpw.Text == "Hide"))
                {
                    if (Txt_Password1.Text != "" && Txt_Password1.Text == Txt_Password2.Text)
                    {
                        MyUserAction.UpdateUser(Txt_Password1.Text.ToString(), Txt_email.Text.ToString(), Txt_SurName.Text.ToString(), int.Parse(Drp_UserRole.SelectedValue), int.Parse(Drp_Loginright.SelectedValue), int.Parse(Txt_UserId.Text.ToString()));
                        if (LkBtn_UserDetails.Text == "Hide Details")
                        {
                            if (Txt_Address.Text != "" && Txt_Dob.Text != "" && Txt_Ph.Text != "")
                            {
                                _DateOfBirth = DateTime.Parse(Txt_Dob.Text.ToString());
                              //  _DateOfBirth =   (Txt_Dob.Text.ToString());

                                if (MyUserAction.HasData(int.Parse(Txt_UserId.Text.ToString())))
                                {
                                    String Sql = "UPDATE tbluserdetails SET Address='" + Txt_Address.Text.ToString() + "',DOB='" + _DateOfBirth.Date.ToString("s") + "',Phone='" + Txt_Ph.Text.ToString() + "' WHERE UserId='" + int.Parse(Txt_UserId.Text.ToString()) + "'";
                                    MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
                                }
                                else
                                {
                                    String Sql = "INSERT INTO tbluserdetails(UserId,Address,DOB,Phone) VALUES( '" + int.Parse(Txt_UserId.Text.ToString()) + "', '" + Txt_Address.Text.ToString() + "', '" + _DateOfBirth.Date.ToString("s") + "','" + Txt_Ph.Text.ToString() + "')";
                                    MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
                                }
                            }
                            else
                            {
                                Lbl_FailureNote.Text = "Some of the fields are Empty";
                            }

                        }
                        Lbl_FailureNote.Text = "User is Updated";

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
                    MyUserAction.UpdateUser1(Txt_email.Text.ToString(), Txt_SurName.Text.ToString(), int.Parse(Drp_UserRole.SelectedValue), int.Parse(Drp_Loginright.SelectedValue), int.Parse(Txt_UserId.Text.ToString()));

                    if (LkBtn_UserDetails.Text == "Hide Details")
                    {
                        if (Txt_Address.Text != "" && Txt_Dob.Text != "" && Txt_Ph.Text != "")
                        {
                            _DateOfBirth = DateTime.Parse(Txt_Dob.Text.ToString());
                            if (MyUserAction.HasData(int.Parse(Txt_UserId.Text.ToString())))
                            {
                                String Sql = "UPDATE tbluserdetails SET Address='" + Txt_Address.Text.ToString() + "',DOB='" + _DateOfBirth.Date.ToString("s") + "',Phone='" + Txt_Ph.Text.ToString() + "' WHERE UserId='" + int.Parse(Txt_UserId.Text.ToString()) + "'";
                                MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
                            }
                            else
                            {
                                String Sql = "INSERT INTO tbluserdetails(UserId,Address,DOB,Phone) VALUES( '" + int.Parse(Txt_UserId.Text.ToString()) + "', '" + Txt_Address.Text.ToString() + "', '" + _DateOfBirth.Date.ToString("s") + "','" + Txt_Ph.Text.ToString() + "')";
                                MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
                            }
                            Lbl_FailureNote.Text = "User is Updated";
                        }
                        else
                        {
                            Lbl_FailureNote.Text = "Some of the fields are Empty";
                        }
                    }
                    Lbl_FailureNote.Text = "User is Updated";
                }
            }
        }
    }
    protected void Panel2_Load(object sender, EventArgs e)
    {
    }
    protected void Drp_Loginright_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDrpLogin();
    }
    protected void LkBtn_Resetpw_Click(object sender, EventArgs e)
    {
        if (Panel4.Visible == false)
        {
            Panel4.Visible = true;
            LkBtn_Resetpw.Text = "Hide";

        }
        else
        {
            Panel4.Visible = false;
            LkBtn_Resetpw.Text = "Reset Password";

        }
    }   
    protected void LkBtn_UserDetails_Click(object sender, EventArgs e)
    {
        DateTime Dob;
        if (Panel3.Visible == false)
        {
            Panel3.Visible = true;
            LkBtn_UserDetails.Text = "Hide Details";

        }
            
        else
        {
            Panel3.Visible = false;
            LkBtn_UserDetails.Text = "Show More Details";
            LnkBtn_Details.Visible = false;

        }
        if (LkBtn_UserDetails.Text == "Hide Details")
        {
            LnkBtn_Details.Visible = false;
        }
        String Sql = "SELECT Address,DOB,Phone FROM tbluserdetails WHERE UserId='"+ Txt_UserId.Text + "'";
        MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            Txt_Address.Text = MyReader.GetValue(0).ToString();
            Dob = DateTime.Parse(MyReader.GetValue(1).ToString());
            Txt_Dob.Text = Dob.Date.ToString("MM-dd-yyyy");           
            Txt_Ph.Text = MyReader.GetValue(2).ToString();
        }
        
    }
    
    private void DisplayImage(int _UserId)
    {
        //ImgBtn_User.ImageUrl = User.GetImageUrl("UserImage", _UserId);
        ImgBtn_User.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + _UserId + "&type=UserImage";
      
    }
    protected void Btn_DeleteUser_Click(object sender, EventArgs e)
    {
        string Sql, ImgName;
        int UserId = int.Parse(Txt_UserId.Text.ToString());
        if (MyUserAction.NotaManagerOrAdmin(UserId))  //  TO CHECK WHETHER USER IS A MANAGER OR NOT AND ID=1
        {
            Sql = "DELETE FROM tbluser WHERE Id=" + UserId;
            MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
            if (MyUserAction.TblUserdetailsHasData(UserId))  //TO CHECK WHETHER tbluseerdetails HAS ANY DATA
            {
                Sql = "DELETE FROM tbluserdetails WHERE UserId=" + UserId;
                MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
            }
            if (MyUserAction.UseHasImage(UserId,out ImgName))
            {
                Sql = "DELETE FROM tblfileurl WHERE Type='UserImage' AND UserId=" + UserId;
                MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
                try
                {
                    if (ImgName != "")
                    {
                        File.Delete(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+ "\\ThumbnailImages\\" + ImgName);
                    }
                }
                catch
                {

                }
            }
            Lbl_FailureNote.Text = "The User is deleted";
            Response.Redirect("ManageUser.aspx");
        }
        else
        {
            Lbl_FailureNote.Text = "The User is a manager of a group and cannot be deleted";
        }
        
    }

   
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageUser.aspx");
    }
    protected void Btn_Upload_Click(object sender, EventArgs e)
    {
        if(Txt_UserId.Text.Trim() == "")
        {
            Lbl_FailureNote.Text = "The associated user object have been deleted";
        }
        else if (FileUp_User.PostedFile == null)
        {
            Lbl_Upload.Text = "Select an image to upload";
        }
        else if (FileUp_User.PostedFile != null && !ValidImageFile())
        {
            Lbl_Upload.Text = "File type cannot be uploaded";

        }       
        else
        {
            AddPhoto(int.Parse(Txt_UserId.Text.ToString()));
            Lbl_Upload.Text = "Image is Updated";
            Panel1.Visible = false;
            Panel7.Visible = false;
        }
        
    }

    private void AddPhoto(int _UserID)
    {
        string ImageUrl = "";
        String Sql = "";
        string preimage = "";
        string ImageName = FileUp_User.FileName.ToString();
        FileUp_User.SaveAs(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"UpImage\\" + ImageName);
        
        string ThumbnailPath = (WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"\\ThumbnailImages\\" + "User" + _UserID.ToString() + ImageName);
        using (System.Drawing.Image Img = System.Drawing.Image.FromFile(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"\\UpImage\\" + ImageName))
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
        ImageUrl = "User" + _UserID + ImageName;

        if (MyUserAction.HasImage(_UserID, out preimage))
        {  
            Sql = "UPDATE tblfileurl SET FilePath='" + ImageUrl + "' WHERE UserId=" + _UserID;
        }
        else
        {
            Sql = "INSERT INTO tblfileurl(UserId,FilePath,Type) VALUES(" + _UserID + ", '" + ImageUrl + "','UserImage')";
        }
        MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
        if (ImageName != "")
        {
            File.Delete(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"\\UpImage\\" + ImageName);
        }
        if (preimage != "")
        {
            File.Delete(WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""))+"\\ThumbnailImages\\" + preimage);
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
        string fileExtension = System.IO.Path.GetExtension(FileUp_User.FileName).ToLower();
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
    protected void LnkBtn_Details_Click(object sender, EventArgs e)
    {
        {

            String Sql = "SELECT tu.Id,tu.SurName,tu.EmailId,tu.RoleId,tu.CanLogin,tr.Type FROM tbluser tu INNER JOIN tblrole tr ON tu.RoleId=tr.Id AND tr.Type <> 'Staff' WHERE tu.UserName='" + Txt_LoginName.Text + "'";
            MyReader = MyUserAction.m_MysqlDb.ExecuteQuery(Sql);
            if (MyReader.HasRows)
            {
                if (Lbl_Message.Visible == true)
                {
                    Lbl_Message.Visible = false;
                }
                Panel1.Visible = true;
                Panel7.Visible = true;
                Txt_UserId.Text = MyReader.GetValue(0).ToString();
                Txt_SurName.Text = MyReader.GetValue(1).ToString();
                Txt_email.Text = MyReader.GetValue(2).ToString();
                Drp_UserRole.SelectedValue = MyReader.GetValue(3).ToString();
                Drp_Loginright.SelectedValue = MyReader.GetValue(4).ToString();
                SetDrpLogin();
                DisplayImage(int.Parse(Txt_UserId.Text.ToString()));
            }
            else
            {
                if (Panel1.Visible == true)
                {
                    Panel1.Visible = false;
                }
                Lbl_Message.Text = "No user found.Please try again";
                Lbl_Message.Visible = true;
            }
            LkBtn_UserDetails.Text = "Show More Details";
        }
    }
}
