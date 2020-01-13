using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using WinBase;

namespace WinEr
{
    public partial class Announcements : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
       
        private KnowinUser MyUser;
        private ConfigManager MyConfiMang;
        private SMSManager MysmsMang;
        private MysqlClass mysql;
        //private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MysmsMang = MyUser.GetSMSMngObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(404))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClassDetails();
                    LoadType();
                }

                DateTime dt = DateTime.Now;
                TimeSpan ts = dt.TimeOfDay;
            }
        }
        private void LoadClassDetails()
        {
            Drp_Class.Items.Clear();
            ListItem li;


            //string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + m_myId + ") ORDER BY tblstandard.Id,tblclass.ClassName";
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass  INNER JOIN tblstandard ON tblclass.Standard = tblstandard.Id where tblclass.Status=1 AND tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId  UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId ) ORDER BY tblstandard.Id,tblclass.ClassName";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Class", "-1");
                Drp_Class.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Class.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("Not Found", "-1");
                Drp_Class.Items.Add(li);
            }
            Drp_Class.SelectedValue = "-1";
        }
        private void LoadType()
        {
            Drp_Type.Items.Clear();
            ListItem li;

            string sql = "SELECT Id,Type from tbl_annoucement_type";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                li = new ListItem("Select Type","-1");
                Drp_Type.Items.Add(li);
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_Type.Items.Add(li);
                }

            }

        }

        protected void Drp_Class_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdBtLstSelectCtgry1.SelectedIndex == 2)
            { Panel_Students.Visible = true; }
            lblstudentmsg.Visible = false;
            Chkb_studnts.Items.Clear();
            ListItem li = new ListItem();


            string sql = "SELECT tblstudent.Id,tblstudent.StudentName from tblstudent where LastClassId=" + Drp_Class.SelectedValue;
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Chkb_studnts.Items.Add(li);
                }
                
            }
            else
            {
                //li = new ListItem("No Students", "-1");
                //Chkb_studnts.Items.Add(li);
                Panel_Students.Visible = false;
                lblstudentmsg.Visible = true;
            }
        }
        int RefType;
        protected void RdBtLstSelectCtgry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblstudentmsg.Visible = false;

            if (RdBtLstSelectCtgry1.SelectedItem.Text == "Selected Students")
            {
                lblSC.Visible = true;
                Drp_Class.Visible = true;
                if (Drp_Class.SelectedValue != "-1" && lblstudentmsg.Visible == false)
                {                   
                    Panel_Students.Visible = true;
                    for (int i = 0; i < Chkb_studnts.Items.Count; i++)
                    {
                        Chkb_studnts.Items[i].Selected = false;

                    }
                }
            }
            else if (RdBtLstSelectCtgry1.SelectedItem.Text == "All")
            {
                
                Panel_Students.Visible = false;
                lblSC.Visible = false;
                Drp_Class.Visible = false;
            }
            else if (RdBtLstSelectCtgry1.SelectedItem.Text == "Class")
            {               
                    lblSC.Visible = true;
                    Drp_Class.Visible = true;
                    Panel_Students.Visible = false;
                    for (int i = 0; i < Chkb_studnts.Items.Count; i++)
                    {
                        Chkb_studnts.Items[i].Selected = true;
                    }
                
            }
        }

        protected void Btn_save_Click(object sender, EventArgs e)
        {

            lblmsg.Visible = false;
            bool IsDone = false;
            int AnnId;
            string Title = txt_Title.Text;
            string body = txt_body.Text;
            string Extlnk = txt_link.Text.Replace("watch?v=", "embed/");
            DateTime _now = System.DateTime.Now;
            //string  Cdate=MyUser.GerFormatedDatVal(_now);
            DateTime expdt;

            if (txt_ExpDate.Text.Trim() != "")
            {
                //expdt = DateTime.Parse(txt_ExpDate.Text);

                expdt = MyUser.GetDareFromText(txt_ExpDate.Text);


            }
            else
            {
                lblmsg.Visible = true; lblmsg.Text = "Enter Expriy Date.."; return;
            }
            //            string ExpDate = expdt.ToString();

            if (RdBtLstSelectCtgry1.SelectedItem.Text == "Class")
            {
                for (int i = 0; i < Chkb_studnts.Items.Count; i++)
                {
                    Chkb_studnts.Items[i].Selected = true;
                }
            }

            GetRefType();

            if (Drp_Type.SelectedValue != "-1")
            {
                if (RefType != 0)
                {
                    if (CheckStudentSelection() > 0)
                    {
                        if (txt_Title.Text.Trim() != "")
                        {
                            if ((Drp_Type.SelectedValue == "1" && txt_link.Text.Trim() != "") || (Drp_Type.SelectedValue == "2" && FileUpload1.HasFile == true))
                            {
                                if (Drp_Type.SelectedValue == "2")
                                {
                                    if (FileUpload1.PostedFile.ContentLength > 2000000)
                                    {
                                        lblmsg.Visible = true;
                                        lblmsg.Text = "FileSize Exceeds the Limits.Please Try uploading smaller size.";
                                        goto Pass;
                                    }
                                    if (!SaveImage(out Extlnk))
                                    {
                                        lblmsg.Visible = true;
                                        lblmsg.Text = "Something went wrong. Please try later.";
                                        goto Pass;
                                    }
                                }
                                SaveAnnouncement(Title, body, Extlnk, _now, expdt, RefType, Convert.ToInt16(Drp_Type.SelectedValue));
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Announcements", "A New  Announcement " + txt_Title.Text + "  is Added.", 1);
                                AnnId = GetAnnID();
                                if (RdBtLstSelectCtgry1.SelectedIndex == 2)
                                {
                                    Mapstudent(AnnId);
                                }
                                if (RdBtLstSelectCtgry1.SelectedIndex == 1)
                                {
                                    Update_Announcement(AnnId, int.Parse(Drp_Class.SelectedValue));
                                }

                                WC_MessageBox.ShowMssage("Sent successfully");

                            Pass:
                                clear();
                            }
                            else
                            {
                                if (Drp_Type.SelectedValue == "1")
                                    lbllink.Text = "Please enter Link..";

                                if (Drp_Type.SelectedValue == "2")
                                    LblImageErr.Text = "Please select choose one image.";
                            }
                        }
                        else
                        {
                            lbltitle.Text = "Please Enter Title..";
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Any Student.";
                    }
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Select Category";
                }
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select Announcement Type..";
            }
        }

        private bool SaveImage(out string FileName)
        {
            FileName = DateTime.Today.Date.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("Hmmss") + ".jpg";
            try
            {
                HttpPostedFile myFile = FileUpload1.PostedFile;
                int nFileLen = myFile.ContentLength;
                byte[] myData = new byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                SchoolClass objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
                string FilePath = WinerUtlity.GetAbsoluteFilePath(objSchool, Server.MapPath(""));
                FilePath += "\\UpImage\\";
                FilePath += FileName;
                FileStream newFile = new FileStream(FilePath, FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void GetRefType()
        {
            if (RdBtLstSelectCtgry1.SelectedIndex == 0)
            {
                RefType = 1;
            }
            else if (RdBtLstSelectCtgry1.SelectedIndex == 1)
            {
                RefType = 2;
            }
            else if (RdBtLstSelectCtgry1.SelectedIndex == 2)
            {
                RefType = 3;
            }
        }

        private void SaveAnnouncement(string Title,string Body,string Extlnk,DateTime Createdate,DateTime  ExpDate,int _RefType,int ExtType)
        {
            string sql = "Insert into tbl_announcemnts(Title,Body,ExternalLink,CreatedDatetime,ExpiryDatetime,RefType,RefId,ExternalType) values('" + Title + "','" + Body + "','" + Extlnk + "','" + Createdate.ToString("s") + "','" + ExpDate.ToString("s") + "','" + _RefType + "','-1','" + ExtType + "')";
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        }
        private void Update_Announcement(int Announcement_Id,int Class_Id)
        {
            string sql = "update tbl_announcemnts set RedId=" + Class_Id + " where Id=" + Announcement_Id  + "";  
            MyStudMang.m_MysqlDb.ExecuteQuery(sql);
        }
        private int GetAnnID()
        {
            int ID;
            string sql = "select max(Id) from tbl_announcemnts";
            MydataSet = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                ID = Convert.ToInt16(MydataSet.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                ID = 1;
            }
            return ID;
        }
        private void Mapstudent(int id)
        {
           
            for(int i=0;i<Chkb_studnts.Items.Count;i++)
            {

                if(Chkb_studnts.Items[i].Selected==true)
                {
                    int StudentID=Convert.ToInt16(Chkb_studnts.Items[i].Value);
                    string sql = "Insert into tbl_annoucement_studentmap(AnnId,StudentId) value("+id+","+StudentID+")";
                    MyStudMang.m_MysqlDb.ExecuteQuery(sql);
                }
            }
        }

        private int CheckStudentSelection()
        {
            int msg = 1;
            int StudentCount = 0;
            if (RdBtLstSelectCtgry1.SelectedIndex == 2)
            {
                for (int i = 0; i < Chkb_studnts.Items.Count; i++)
                {
                    if (Chkb_studnts.Items[i].Selected == false)
                    {
                        StudentCount = StudentCount + 1;
                    }
                }
                if (StudentCount == Chkb_studnts.Items.Count)
                {
                    msg = -1;
                }
                else
                {
                    msg = 1;
                }
            }
            return msg;
        }

        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void clear()
        {
            txt_body.Text = "";
            txt_ExpDate.Text = "";
            txt_Title.Text = "";
            txt_link.Text = "";
            Drp_Type.SelectedValue ="-1";
            Drp_Class.SelectedValue = "-1";
            Panel_Students.Visible = false;
        }

        protected void txt_Title_TextChanged(object sender, EventArgs e)
        {
            if (txt_Title.Text.Trim() != "")
            {
                lbltitle.Visible = false;
            }
        }

        protected void txt_link_TextChanged(object sender, EventArgs e)
        {
            if (txt_link.Text.Trim() != "")
            {
                lbllink.Visible = false;
            }
        }
    }
}
