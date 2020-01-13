using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
using System.IO;
using System.Configuration;

namespace WinEr
{
    public partial class EmailCircular : System.Web.UI.Page
    {

        private StudentManagerClass MyStudMang;
        private EmailManager Obj_Email;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;
        private SchoolClass objSchool = null;
        private MysqlClass _Mysqlobj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            Obj_Email = MyUser.GetEmailObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (!MyUser.HaveActionRignt(873))
            {
                Response.Redirect("RoleErr.htm");
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
                    if (objSchool != null)
                        _Mysqlobj = new MysqlClass(objSchool.ConnectionString);
                }
                else
                {
                    _Mysqlobj = new MysqlClass(WinerUtlity.SingleSchoolConnectionString);
                }
                lblattacherror.Text = "";
                if (!IsPostBack)
                {
                    LoadClassToDropDown();
                    LoadTypeDropDown();
                    Session["Attachmentfilelisttable"] = null;
                    loadattachmentfiles();
                   
                }
            }
        }
        private void LoadTypeDropDown()
        {
            DataSet TypeDs = new DataSet();
            ListItem li;
            Drp_Template.Items.Clear();
            TypeDs = MyStudMang.GetGeneralEmailTemplate();
            if (TypeDs != null && TypeDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Type", "0");
                Drp_Template.Items.Add(li);
                foreach (DataRow dr in TypeDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["Type"].ToString(), dr["Id"].ToString());
                    Drp_Template.Items.Add(li);
                }

            }
            else
            {
                li = new ListItem("None", "-1");
                Drp_Template.Items.Add(li);
            }
        }


        private void LoadClassToDropDown()
        {
            DataSet ClassDs = new DataSet();
            ListItem li;
            ClassDs = Obj_Email.LoadClassDetails();
            if (ClassDs != null && ClassDs.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("All", "0");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in ClassDs.Tables[0].Rows)
                {
                    li = new ListItem(dr["ClassName"].ToString(), dr["Id"].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("None", "-1");
            }
        }

        protected void Btn_SendEmail_Click(object sender, EventArgs e)
        {
            OdbcDataReader EmailReader = null;
            int success = 1;
            string attach1 = "", attach2 = "", attach3 = "";
          
            try
            {
                DataTable dtattach = new DataTable();
                if (Session["Attachmentfilelisttable"] != null)
                {
                    dtattach = (DataTable)Session["Attachmentfilelisttable"];
                    for (int i = 0; i < dtattach.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            attach1 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                        }
                        if (i == 1)
                        {
                            attach2 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                        }
                        if (i == 2)
                        {
                            attach3 = dtattach.Rows[i]["RepositoryFileName"].ToString();
                        }
                    }
                }
                if (Txt_EmailSubject.Text != "" && Editor_Body.Content != "")
                {
                    Obj_Email.CreateTansationDb();
                    if (Rdb_CheckType.SelectedValue == "0") //Staff
                    {
                        EmailReader = Obj_Email.GetEmailStaffId("");
                        if (EmailReader.HasRows)
                        {
                            while (EmailReader.Read())
                            {
                                if (EmailReader.GetValue(0).ToString().Replace("&nbsp;", "") != "")
                                {
                                    string EmailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                                    EmailBody = Obj_Email.SeperatorReplace(int.Parse(EmailReader.GetValue(1).ToString()), 0, EmailBody);
                                    Obj_Email.InsertDataToAutoEmailListwithattachment(EmailReader.GetValue(0).ToString(), EmailReader.GetValue(1).ToString(), Txt_EmailSubject.Text, EmailBody, 1, attach1, attach2, attach3);
                                }
                            }
                        }
                        else
                        {
                            success = 0;
                            Obj_Email.EndFailTansationDb();
                            loadattachmentfiles();
                            WC_MsgBox.ShowMssage("No email Id is available");
                        }

                    }
                    else if (Rdb_CheckType.SelectedValue == "1") //Parent
                    {
                        EmailReader = Obj_Email.GetEmailParentId(int.Parse(Drp_Class.SelectedValue), -1);
                        if (EmailReader.HasRows)
                        {
                            while (EmailReader.Read())
                            {
                                if (EmailReader.GetValue(0).ToString().Replace("&nbsp;", "") != "")
                                {
                                    string EmailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                                    EmailBody = Obj_Email.SeperatorReplace(0, int.Parse(EmailReader.GetValue(1).ToString()), EmailBody);
                                    Obj_Email.InsertDataToAutoEmailListwithattachment(EmailReader.GetValue(0).ToString(), EmailReader.GetValue(1).ToString(), Txt_EmailSubject.Text, EmailBody, 2, attach1, attach2, attach3);
                                }
                            }
                        }
                        else
                        {
                            success = 0;
                            Obj_Email.EndFailTansationDb();
                            loadattachmentfiles();
                            WC_MsgBox.ShowMssage("No email Id is available");
                        }
                    }
                    else if (Rdb_CheckType.SelectedValue == "2") //Student
                    {
                        EmailReader = Obj_Email.GetEmailStudentId(int.Parse(Drp_Class.SelectedValue));
                        if (EmailReader.HasRows)
                        {
                            while (EmailReader.Read())
                            {
                                if (EmailReader.GetValue(0).ToString().Replace("&nbsp;", "") != "")
                                {
                                    string EmailBody = Editor_Body.Content.Replace("'", "").Replace("\\", "");
                                    EmailBody = Obj_Email.SeperatorReplace(0, int.Parse(EmailReader.GetValue(1).ToString()), EmailBody);
                                    Obj_Email.InsertDataToAutoEmailListwithattachment(EmailReader.GetValue(0).ToString(), EmailReader.GetValue(1).ToString(), Txt_EmailSubject.Text, Editor_Body.Content.Replace("'", "").Replace("\\", ""), 3, attach1, attach2, attach3);
                                }
                            }
                        }
                        else
                        {
                            success = 0;
                            Obj_Email.EndFailTansationDb();
                            loadattachmentfiles();
                            WC_MsgBox.ShowMssage("No email Id is available");
                        }
                    }
                }
                else
                {
                    loadattachmentfiles();
                    WC_MsgBox.ShowMssage("Enter email subject and body");
                    success = 0;
                }
                if (success == 1)
                {
                    Obj_Email.EndSucessTansationDb();
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "Email", "Emails sent to "+ Rdb_CheckType.SelectedItem.Text +"", 1);
                    WC_MsgBox.ShowMssage("Email has been sent successfully");
                    Txt_EmailSubject.Text = "";
                    Editor_Body.Content = "";
                    LoadClassToDropDown();
                    LoadTypeDropDown();
                    Session["Attachmentfilelisttable"] = null;
                    loadattachmentfiles();
                }
            }
            catch (Exception ex)
            {
                success = 0;
                WC_MsgBox.ShowMssage("Cannot send,Please refresh the page and try again later");
                Obj_Email.EndFailTansationDb();
                loadattachmentfiles();
            }
          
           
        }

        protected void Drp_Template_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (int.Parse(Drp_Template.SelectedValue) > 0)
            {
                sql = "Select tbl_generalemailtemplate.Subject, tbl_generalemailtemplate.Body,Enabled from tbl_generalemailtemplate where tbl_generalemailtemplate.Id=" + int.Parse(Drp_Template.SelectedValue) + "";
                MyReader = Obj_Email.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_EmailSubject.Text = MyReader.GetValue(0).ToString().Replace("&nbsp;", "");
                    Editor_Body.Content = MyReader.GetValue(1).ToString();
                }
            }
               
            else
            {
                Txt_EmailSubject.Text = "";
                Editor_Body.Content = "";
            }
            loadattachmentfiles();
        }
        protected void Btnattach_Click(object sender, EventArgs e)
        {
            string filename = "";
            string Rep_filename = "";


            try
            {
                if (attachmentvalidations(out filename, out Rep_filename))
                {

                    getattachfilestogrid(filename, Rep_filename);
                }        
            }
            catch (Exception ed)
            {
                WC_MsgBox.ShowMssage(ed.Message);
            }
        }
        private void getattachfilestogrid(string fname,string repsitoryfname)
        {
            //code modified :DOminic
             DataTable dtsession = new DataTable();
             DataRow dr = null;
             if (Session["Attachmentfilelisttable"] != null)
             {
                 dtsession = (DataTable)Session["Attachmentfilelisttable"];
                 
             }
             else
             {
                 dtsession.Columns.Add("FileName", typeof(System.String));
                 dtsession.Columns.Add("RepositoryFileName", typeof(System.String));       
                
             }

             dr = dtsession.NewRow();
             dr[0] = fname.ToString();
             dr[1] = repsitoryfname.ToString();
             dtsession.Rows.Add(dr);

             Session["Attachmentfilelisttable"] = dtsession;
             loadattachmentfiles();



/*************************************/
            //DataTable dt = new DataTable();
            //dt.Columns.Add("FileName", typeof(System.String));
            //dt.Columns.Add("RepositoryFileName", typeof(System.String));
            //DataRow dr = null;
            //dr = dt.NewRow();
            //dr[0] = fname.ToString();
            //dr[1] = repsitoryfname.ToString();
            //dt.Rows.Add(dr);
            //DataTable mergedt = new DataTable();
            //DataTable dtsession =new DataTable();
            //if (Session["Attachmentfilelisttable"] != null)
            //{
            //    dtsession = (DataTable)Session["Attachmentfilelisttable"];
               
            //    //if (dtsession.Rows.Count > 0)
            //    //{

            //        mergedt.Merge(dtsession);
            //        mergedt.Merge(dt);
                

            //    //}
            //}
            //else
            //{
            //    mergedt.Merge(dt);
             
            //}
            //Session["Attachmentfilelisttable"] =mergedt;
            //loadattachmentfiles();
       
        }
        private void loadattachmentfiles()
        {
              DataTable dtgrid =new DataTable();
              if (Session["Attachmentfilelisttable"] != null)
              {
                  pnlattachment.Visible = true;
                  dtgrid = (DataTable)Session["Attachmentfilelisttable"];
                  Grd_attachment.Visible = true;
                  Grd_attachment.DataSource = dtgrid;
                  Grd_attachment.DataBind();
                  Grd_attachment.Columns[2].Visible = false;
              }
              else
              {
                  pnlattachment.Visible = false;
                  Grd_attachment.Visible =false;
              }

        }
        protected void Grd_attachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Grd_attachment.Columns[2].Visible =true;
                string filename = Grd_attachment.DataKeys[e.RowIndex].Values["FileName"].ToString();
                string filerep = Grd_attachment.DataKeys[e.RowIndex].Values["RepositoryFileName"].ToString();
                //string pathdelete = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath("")) + "\\AttachmentFiles\\";
                //string pathdelete = Server.MapPath("") + "\\" + ConfigurationSettings.AppSettings["EmailAttachmentFilePath"] + "//";
                string pathdelete = Server.MapPath("") + "\\" + "Email Attachments\\";
                string filepathdelete = pathdelete + filerep;
                if (File.Exists(filepathdelete))
                {
                    File.Delete(filepathdelete);
                }
                DataTable dtdelete = new DataTable();
                if (Session["Attachmentfilelisttable"] != null)
                {
                    dtdelete = (DataTable)Session["Attachmentfilelisttable"];
                    dtdelete.Rows[e.RowIndex].Delete();
                }
                Session["Attachmentfilelisttable"] = dtdelete;
                Grd_attachment.Columns[2].Visible =false;
                loadattachmentfiles();
                lblattacherror.Text = ""+filename+" Removed from Attachment";
                
            }
            catch (Exception eg)
            {
                lblattacherror.Text = eg.Message;
            }

        }

        private bool attachmentvalidations( out string filenametosave,out string repfilename)
        {
            bool isvalid = false;
            int checkaccessfile = 0;
            filenametosave = "";
            repfilename = "";
            string savepath = "";
            //string pathfilesave =Server.MapPath("") + "\\" + ConfigurationSettings.AppSettings["EmailAttachmentFilePath"] + "//";
            string pathfilesave = Server.MapPath("") + "\\"+"Email Attachments\\";
            if (!Directory.Exists(pathfilesave))
            {
                Directory.CreateDirectory(pathfilesave);
            }
            if (fileUploadattachments.HasFile)
            {
                string filename = System.IO.Path.GetFileName(fileUploadattachments.FileName);
                filenametosave = filename.ToString();
                string fileExtension = Path.GetExtension(filename);
                fileExtension = fileExtension.ToLower();
                string[] fnmeparts = filename.Split('.');
                string curdate = System.DateTime.Now.ToString();
                repfilename = objSchool.SchoolId.ToString() + "_" + fnmeparts[0].ToString() + "_" + curdate + "." + fnmeparts[1].ToString();
                repfilename = repfilename.Replace('/', '_').Replace(':', '_');
                savepath = pathfilesave + repfilename;
                string[] acceptedFileTypes = new string[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".gif", ".png", ".txt", ".xls", ".xlsx" };
                for (int i = 0; i <= acceptedFileTypes.Length-1; i++)
                {
                    if (fileExtension == acceptedFileTypes[i])
                    {
                        checkaccessfile = 1;
                    }
                }
               
                if (checkaccessfile == 1)
                {
                    if (fileUploadattachments.PostedFile.ContentLength <= 1048576)
                    {
                        DataTable dtattachlist = (DataTable)Session["Attachmentfilelisttable"];
                      
                        if (dtattachlist != null)
                        {
                            if (dtattachlist.Rows.Count < 3)
                            {
                                bool contains = dtattachlist.AsEnumerable().Any(row => filename == row.Field<String>("FileName"));
                                if (contains)
                                {
                                    lblattacherror.Text = "file already attached";

                                }
                                else
                                {
                                    isvalid = true;
                                   
                                    fileUploadattachments.SaveAs(savepath);

                                }
                            }
                            else
                            {
                              
                                lblattacherror.Text = "maximum file attachment count reached";
                            }
                        }
                        else
                        {
                            isvalid = true;
                            fileUploadattachments.SaveAs(savepath);
                        }

                    }
                    else
                    {
                      
                        lblattacherror.Text = "Your browsing file not attached,Please select size below 1MB";
                    }
                }

                else
                {
                   
                    lblattacherror.Text = "Your browsing file not supported for attachments";
                }
            }
            else
            {
                lblattacherror.Text = "Please browse file";
            }
            return isvalid;
        }
     

    }
}
