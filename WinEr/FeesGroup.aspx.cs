using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Drawing;
using System.IO;

namespace WinEr
{
    public partial class FeesGroup : System.Web.UI.Page
    {
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private static string Fees_Group_Name = "";
        private static int Select_Id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyFeeMang = MyUser.GetFeeObj();
            if (MyFeeMang == null)
            {
                Response.Redirect("sectionerr.htm");
            
            }
            else if (!MyUser.HaveActionRignt(3019))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    Load_Heaser_Groups();
                    Load_Payment_Gateways();
                    Pnl_Logo.Visible = false;
                }
            }
            if (!Is_Online_Payment_Enable())
            {
                Lbl_gateway.Visible = false;
                Drplist_Gateway.Visible = false;
                Drplist_edt_Gateway.Visible = false;
                Lbl_Edt_Gateway.Visible = false;
            }
            else
            {
                Lbl_gateway.Visible = true;
                Drplist_Gateway.Visible = true;
                Drplist_edt_Gateway.Visible = true;
                Lbl_Edt_Gateway.Visible = true;
            }
        }

        private bool Is_Online_Payment_Enable()
        {
            bool enable = false;
            string sql = "";
            sql = "select tblconfiguration.Value from tblconfiguration where tblconfiguration.Name='Online_Payment'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    if (int.Parse(MyReader.GetValue(0).ToString()) == 1)
                    {
                        enable = true;
                    }
                }
            }
            return enable;
        }
        private void Load_Payment_Gateways()
        {
            DataSet ds_Headers = new DataSet();
            string sql = "select * from tbl_feesgatewayconfig ";
            ds_Headers = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds_Headers != null && ds_Headers.Tables[0] != null && ds_Headers.Tables[0].Rows.Count > 0)
            {
                Drplist_Gateway.Items.Clear();
                Drplist_edt_Gateway.Items.Clear();
                Drplist_Gateway.DataSource = ds_Headers;
                Drplist_edt_Gateway.DataSource = ds_Headers;
                Drplist_Gateway.DataValueField = "Id";
                Drplist_Gateway.DataTextField = "GateWay";
                Drplist_Gateway.DataBind();
                Drplist_edt_Gateway.DataValueField = "Id";
                Drplist_edt_Gateway.DataTextField = "GateWay";
                Drplist_edt_Gateway.DataBind();
             
            }
            else
            {
                ListItem li = new ListItem("No Payment Gateway Exist To Map", "0");
                Drplist_Gateway.Items.Add(li);
                Drplist_edt_Gateway.Items.Add(li);
            }
        }
        protected void Chkb_Default_CheckedChanged(object sender, EventArgs e)
        {
            if (Chkb_Default.Checked)
            {
                Pnl_Logo.Visible = false;
            }
            else
            {
                Pnl_Logo.Visible = true;
            }
        }
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            string Msg = "";
            string Logo_Path = "";
            int Enable = 0;
            try
            {
                if (Chkb_enable.Checked)
                {
                    Enable = 1;
                }
                if (_Validations(out Msg))
                {
                    DataSet MyDataSet = new DataSet();
                    int Header_Id = 0;
                    string Address = "";
                    string ImageUrl = "";
                    int Pay_Gateway_Id = 0;
                    string sql = "";
                    if (Is_Online_Payment_Enable())
                    {
                        int.TryParse(Drplist_Gateway.SelectedValue, out Pay_Gateway_Id);
                        sql = "Insert into tbl_feesgrouphead (Name,GateWay_Id,Is_Enable) values('" + Txt_name.Text + "'," + Pay_Gateway_Id + "," + Enable + " )";
                    }
                    else
                    {
                        sql = "Insert into tbl_feesgrouphead (Name,Is_Enable) values('" + Txt_name.Text + "'," + Enable + " )";
                    }
                    MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    sql = "select Id from tbl_feesgrouphead where Name='" + Txt_name.Text + "'";
                    MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                        {

                            int.TryParse(dr["Id"].ToString(), out Header_Id);

                        }
                        if (!Chkb_Default.Checked)
                        {
                            string ImageName = Fileup_Logo.FileName.ToString();
                            Fileup_Logo.SaveAs(Server.MapPath("UpImage\\") + ImageName);
                            string ThumbnailPath = (Server.MapPath("ThumbnailImages\\") + "FeesGroup" + Header_Id.ToString() + ImageName);
                            using (System.Drawing.Image Img = System.Drawing.Image.FromFile(Server.MapPath("UpImage\\") + ImageName))
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

                            ImageUrl = "FeesGroup" + Header_Id.ToString() + ImageName;
                          
                                Address = Txt_Address.Text;
                                sql = "update tbl_feesgrouphead set File_Path='" + ImageUrl + "',Address='" + Address + "',IsDefault_Logo=0 where Id=" + Header_Id + "";
                                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                            else
                            {
                                Get_Default_School_Details(out ImageUrl, out Address);
                                sql = "update tbl_feesgrouphead set File_Path='" + ImageUrl + "',Address='" + Address + "',IsDefault_Logo=1 where Id=" + Header_Id + "";
                                MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                            }
                            Load_Heaser_Groups();
                            WC_MsgBox.ShowMssage("Fees Group Header created successfully");
                            MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fees Group", "A new fee Group " + Txt_name.Text.ToString() + "  is created", 1);
                            Clear_Fields();

                     
                    }
                 
                   
                }
                else
                {
                    Lbl_Msg.Text = Msg;
                }
            }
            catch (Exception et)
            {
                Lbl_Msg.Text = et.Message;
            }

        }
        private void Clear_Fields()
        {
            Txt_name.Text = "";
            Chkb_enable.Checked = true;
            Chkb_Default.Checked = true;
            Txt_Address.Text = "";
            //Txt_Add.Text = "";
            Lbl_Msg.Text = "";
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
            string fileExtension = System.IO.Path.GetExtension(Fileup_Logo.FileName).ToLower();
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
        protected void Grd_Fees_Header_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
                
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int.TryParse(Grd_Fees_Header.Rows[index].Cells[0].Text, out Select_Id);
                Fees_Group_Name = Grd_Fees_Header.Rows[index].Cells[1].Text;
                Load_Edit_Details(Select_Id);
                pnl_Edt_Logo.Visible = false;
                Lbl_Edt_Msg.Text = "";
                
                MPE_FeesGroupPopUp.Show();

            }
            if (e.CommandName == "Remove")
            {
                string Msg="";
                string Header_Name = "";
                int index = Convert.ToInt32(e.CommandArgument);
                int.TryParse(Grd_Fees_Header.Rows[index].Cells[0].Text, out Select_Id);
                Header_Name = Grd_Fees_Header.Rows[index].Cells[1].Text;
                if (!Valid_Remove(out Msg))
                {
                    Remove_Group_Fees_Header(Select_Id);
                    Load_Heaser_Groups();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fees Group", "fees Group:" + Header_Name + "  is Removed", 1);
                    WC_MsgBox.ShowMssage("Fees Group Header Removed Successfully");
                }
                else
                {
                    WC_MsgBox.ShowMssage(Msg);
                }
            }
        }
        protected void Grd_Fees_Header_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_Fees_Header.PageIndex = e.NewPageIndex;
            Load_Heaser_Groups();
            Lbl_Msg.Text = "";
        }
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            //Txt_Add.Text = "";
            Txt_name.Text = "";
            Txt_fees_group.Text = "";
            
        }
        private void Load_Heaser_Groups()
        {
           DataSet MyDataSet = new DataSet();
           string sql = "select tbl_feesgrouphead.Id,tbl_feesgrouphead.Name,tbl_feesgatewayconfig.GateWay from tbl_feesgrouphead inner join tbl_feesgatewayconfig on tbl_feesgrouphead.GateWay_Id=tbl_feesgatewayconfig.Id";
           MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
           if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
           {
               Grd_Fees_Header.Columns[0].Visible = true;
               if (!Is_Online_Payment_Enable())
               {
                   Grd_Fees_Header.Columns[2].Visible = true;
               }
               Grd_Fees_Header.DataSource = MyDataSet;
               Grd_Fees_Header.DataBind();
               Grd_Fees_Header.Columns[0].Visible = false;
               if (!Is_Online_Payment_Enable())
               {
                   Grd_Fees_Header.Columns[2].Visible = false;
               }
           }
           else
           {
               Grd_Fees_Header.DataSource = null;
               Grd_Fees_Header.DataBind();
           }
        }
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string Msg = "";
            string Logo_Path = "";
            int Enable = 0;
            string ImageUrl = "";
            int Pay_Gateway_Id = 0;
            string sql="";
            try
            {
                if (Fileup_Logo.HasFile)
                {
                    Logo_Path = Fileup_Logo.FileName.ToString();
                }
                if (Chkb_enable.Checked)
                {
                    Enable = 1;
                }
                if (Edit_Validations(out Msg))
                {
                    if (Is_Online_Payment_Enable())
                    {
                        int.TryParse(Drplist_edt_Gateway.SelectedValue, out Pay_Gateway_Id);
                        sql = "Update tbl_feesgrouphead set Name='" + Txt_fees_group.Text + "',GateWay_Id=" + Pay_Gateway_Id + ",Is_Enable=" + Enable + " where Id=" + Select_Id + " ";
                    }
                    else
                    {
                        sql = "Update tbl_feesgrouphead set Name='" + Txt_fees_group.Text + "',Is_Enable=" + Enable + " where Id=" + Select_Id + " ";
                    }
                    MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                    if (FileUp_Photo.PostedFile != null && _ValidImageFile())
                    {

                      
                        string ImageName = FileUp_Photo.FileName.ToString();
                        Fileup_Logo.SaveAs(Server.MapPath("UpImage\\") + ImageName);
                        string ThumbnailPath = (Server.MapPath("ThumbnailImages\\") + "FeesGroup" + Select_Id.ToString() + ImageName);
                        if(File.Exists(ThumbnailPath))
                        {
                            File.Delete(ThumbnailPath);
                        }
                        using (System.Drawing.Image Img = System.Drawing.Image.FromFile(Server.MapPath("UpImage\\") + ImageName))
                        {
                            Size ThumbNailSize = NewImageSize(Img.Height, Img.Width, 150);
                            using (System.Drawing.Image ImgThnail =
                            new Bitmap(Img, ThumbNailSize.Width, ThumbNailSize.Height))
                            {
                                ImgThnail.Save(ThumbnailPath, Img.RawFormat);
                                ImgThnail.Dispose();
                            }
                            Img.Dispose();
                            ImageUrl = "FeesGroup" + Select_Id.ToString() + ImageName;
                            Img_Logo.ImageUrl = "ThumbnailImages/" + ImageUrl.ToString();
                        }
                    }
                    if (!Chkbedt_default.Checked)
                    {
                        if (ImageUrl != "")
                        {
                            sql = "update tbl_feesgrouphead set File_Path='" + ImageUrl + "',Address='" + Txt_Edt_Address.Text + "',IsDefault_Logo=0 where Id=" + Select_Id + "";
                        }
                        else
                        {
                            sql = "update tbl_feesgrouphead set Address='" + Txt_Edt_Address.Text + "',IsDefault_Logo=0 where Id=" + Select_Id + "";
                        }
                        MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        Txt_Edt_Address.Text = Txt_Edt_Address.Text;
                        Txt_Edt_Address.Enabled = true;
                    }
                    else
                    {
                        string Address = "";
                        Get_Default_School_Details(out ImageUrl, out Address);
                        sql = "update tbl_feesgrouphead set File_Path='" + ImageUrl + "',Address='" + Address + "',IsDefault_Logo=1 where Id=" + Select_Id + "";
                        MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
                        Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
                        if (Address.Contains("<br/>"))
                        {
                            Address = Address.Replace("<br/>", " ");
                        }
                        Txt_Edt_Address.Text = Address.ToString();
                        Txt_Edt_Address.Enabled = false;
                    }
                     Load_Heaser_Groups();
                     MyUser.m_DbLog.LogToDb(MyUser.UserName, "Fees Group", "fees Group :" + Txt_fees_group.Text + "  is updated", 1);
                     Lbl_Edt_Msg.Text = " Fees Header Group Updated";

                   
                }
                else
                {
                    Lbl_Edt_Msg.Text = Msg;
                }
            }
            catch (Exception et)
            {
                Lbl_Edt_Msg.Text = et.Message;
            }
            MPE_FeesGroupPopUp.Show();
        }
        private bool _ValidImageFile()
        {
            bool fileOK = false;
            string fileExtension = System.IO.Path.GetExtension(FileUp_Photo.FileName).ToLower();
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
        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            MPE_FeesGroupPopUp.Hide();
        }
        protected void lnk_changeLogo_click(object sender, EventArgs e)
        {
            pnl_Edt_Logo.Visible = true;
            MPE_FeesGroupPopUp.Show();
        }

        private bool _Validations(out string Msg)
        {
            Msg = "";
            bool valide = true;
            if (Txt_name.Text == "")
            {
                Msg = "Please enter Fees Group Name";
                valide = false;
            }
            else if (Is_Online_Payment_Enable() && int.Parse(Drplist_Gateway.SelectedValue)==0)
            {
                Msg = "Please select payment gateway";
                valide = false;
            }
            else if (Is_Name_Exists(Txt_name.Text))
            {
                Msg = "Fees Group Name Already Exists";
                valide = false;
            }
            else if (!Chkb_Default.Checked)
            {
                if (!Fileup_Logo.HasFile)
                {
                    Msg = "Please upload Logo";
                    valide = false;
                }
                else if (Txt_Address.Text == "")
                {
                    Msg = "Please Enter Address";
                    valide = false;
                }
                else if (!ValidImageFile())
                {
                    Msg = "Please upload valid logo";
                    valide = false;
                }
            }

            return valide;
        }
        private bool Edit_Validations(out string Msg)
        {
            Msg = "";
            bool valide = true;
            if (Txt_fees_group.Text == "")
            {
                Msg = "Please enter Fees Group Name";
                valide = false;
            }
            else if (Txt_Edt_Address.Text=="")
            {
                Msg = "Please enter Address";
                valide = false;
            }
            else if (Txt_fees_group.Text !=Fees_Group_Name)
            {
                if (Is_Name_Exists(Txt_fees_group.Text))
                {
                    Msg = "Fees Group Name Already Exists";
                    valide = false;
                }
            }

            return valide;
        }
        private bool Valid_Remove(out string Msg)
        {
            Msg = "";
            bool valide = true;
            if (!Is_Header_MapWithFees(Select_Id))
            {
                Msg = "Fees Group Name is mapped with some fees";
                valide = false;
            }
            else if (Select_Id == -1)
            {
                Msg = "This is default fees group,so cann't delete";
                valide = false;
            }
            return valide;
        }
        private bool Is_Name_Exists(string Name)
        {
            bool Is_Exist = false;
            string sql = "SELECT * from tbl_feesgrouphead where Name='"+ Name +"'";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Is_Exist = true;
            }
            return Is_Exist;
        }
        private bool Is_Header_MapWithFees(int Header_Id)
        {
            bool Is_Exist = false;
            string sql = "SELECT * from tblfeeaccount where Header_Id=" + Header_Id + "";
            MyReader = MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Is_Exist = true;
            }
            return Is_Exist;
        }
        private void Remove_Group_Fees_Header(int Header_Id)
        {
            string sql = "Delete from tbl_feesgrouphead where Id=" + Header_Id + "";
            MyFeeMang.m_MysqlDb.ExecuteQuery(sql);
        }
        private void Load_Edit_Details(int Group_Id)
        {
            DataSet MyDataSet = new DataSet();
            Txt_fees_group.Text = "";
            Txt_Edt_Address.Text = "";
            Chkb_Edt_Parent.Checked = false;
            string sql = "select tbl_feesgrouphead.Name,tbl_feesgrouphead.File_Path,tbl_feesgrouphead.Address,tbl_feesgrouphead.Is_Enable,tbl_feesgatewayconfig.GateWay,tbl_feesgrouphead.GateWay_Id,tbl_feesgrouphead.IsDefault_Logo from tbl_feesgrouphead INNER JOIN tbl_feesgatewayconfig ON tbl_feesgrouphead.GateWay_Id=tbl_feesgatewayconfig.Id where tbl_feesgrouphead.Id=" + Group_Id + "";
            MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet!=null && MyDataSet.Tables[0]!=null && MyDataSet.Tables[0].Rows.Count >0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    string Address = "";
                    string Image_Url = "";
                    Address = dr["Address"].ToString();
                    if (Address.Contains("<br />"))
                    {
                        Address = Address.Replace("<br />", " ");
                    }
                    Txt_fees_group.Text = dr["Name"].ToString();
                    Txt_Edt_Address.Text = Address.ToString();
                    Img_Logo.ImageUrl = "ThumbnailImages/"+dr["File_Path"].ToString();
                    if (int.Parse(dr["Is_Enable"].ToString()) == 1)
                    {
                        Chkb_Edt_Parent.Checked = true;
                    }
                    if (dr["GateWay_Id"].ToString() != "")
                    {
                        Drplist_edt_Gateway.SelectedValue = dr["GateWay_Id"].ToString();
                    }
                    if (int.Parse(dr["IsDefault_Logo"].ToString()) == 1)
                    {
                        string _Address = "";
                        Chkbedt_default.Checked = true;
                        pnl_Edt_Logo.Visible = false;
                        
                        lnk_changeLogo.Visible = false;
                        Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
                        Get_Default_School_Details(out Image_Url, out _Address);
                        if (Address.Contains("<br/>"))
                        {
                            Address = Address.Replace("<br/>", " ");
                        }
                        Txt_Edt_Address.Text = Address.ToString();
                        Txt_Edt_Address.Enabled = false;
                    }
                    else
                    {

                        Chkbedt_default.Checked = false;
                        pnl_Edt_Logo.Visible = true;
                        Txt_Edt_Address.Enabled = true;
                        lnk_changeLogo.Visible = true;
                    }
                }
            }
        }
        private void Get_Default_School_Details(out string Logo_Path,out string Address)
        {
            Logo_Path = "";
            Address = "";
            DataSet MyDataSet = new DataSet();
            string sql = "select LogoUrl,Address from tblschooldetails";
            MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {
                    Logo_Path = dr["LogoUrl"].ToString();
                    Address = dr["Address"].ToString();
                }
            }

        }
        protected void Chkbedt_default_OnCheckedChanged(object sender, EventArgs e)
        {
            if (Chkbedt_default.Checked)
            {
                string _Address = "";
                string Image_Url = "";
                Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
                Get_Default_School_Details(out Image_Url, out _Address);
                if (_Address.Contains("<br/>"))
                {
                    _Address = _Address.Replace("<br/>", " ");
                }
                Txt_Edt_Address.Text = _Address.ToString();
                pnl_Edt_Logo.Visible = false;
                Txt_Edt_Address.Enabled = false;
                lnk_changeLogo.Visible = false;
            }
            else
            {
                DataSet MyDataSet = new DataSet();
                string sql = "select tbl_feesgrouphead.Name,tbl_feesgrouphead.File_Path,tbl_feesgrouphead.Address,tbl_feesgrouphead.Is_Enable,tbl_feesgatewayconfig.GateWay,tbl_feesgrouphead.GateWay_Id,tbl_feesgrouphead.IsDefault_Logo from tbl_feesgrouphead INNER JOIN tbl_feesgatewayconfig ON tbl_feesgrouphead.GateWay_Id=tbl_feesgatewayconfig.Id where tbl_feesgrouphead.Id=" + Select_Id + "";
                MyDataSet = MyFeeMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MyDataSet != null && MyDataSet.Tables[0] != null && MyDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                    {
                        string Address = "";
                        if (int.Parse(dr["IsDefault_Logo"].ToString()) == 1)
                        {
                         
                            string Image_Url = "";
                            Img_Logo.ImageUrl = "Handler/ImageReturnHandler.ashx?id=1&type=Logo";
                            Get_Default_School_Details(out Image_Url, out Address);
                        }
                        else
                        {
                            Address = dr["Address"].ToString();
                            Img_Logo.ImageUrl = "ThumbnailImages/" + dr["File_Path"].ToString();
                        }
                        if (Address.Contains("<br/>"))
                        {
                            Address = Address.Replace("<br/>", " ");
                        }

                        Txt_Edt_Address.Text = Address;
                        
                    }
                }
                pnl_Edt_Logo.Visible = false;
                Txt_Edt_Address.Enabled = true;
                lnk_changeLogo.Visible = true;
            }
            MPE_FeesGroupPopUp.Show();
        }
    }
}
