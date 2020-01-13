using System;
using System.Data.Odbc;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data;
using WinBase;
using System.Web.UI.WebControls;
using System.Web;



namespace WinEr
{
    public partial class DownloadDoc : System.Web.UI.Page
    {
        private ConfigManager MyConfigMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;

        protected void Page_init(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyConfigMang = MyUser.GetConfigObj();
            if (MyConfigMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(14))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadClass();
                    LoadStudentsDetailsToDropDown();
                    // SubjectTypeLoading();

                }
            }
        }
        private void LoadClass()
        {
            Drp_class.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName FROM tblclass where tblclass.Status =1 AND tblclass.Id IN (SELECT tblclass.Id from tblclass where tblclass.ParentGroupID IN (SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgrouprelation ON tblgroup.Id=tblgrouprelation.ChildId INNER JOIN tblgroupusermap ON tblgrouprelation.ParentId = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + " UNION SELECT DISTINCT tblgroup.Id FROM tblgroup INNER JOIN tblgroupusermap ON tblgroup.Id = tblgroupusermap.GroupId WHERE  tblgroupusermap.UserId=" + MyUser.UserId + ")) ORDER BY tblclass.Standard,tblclass.ClassName";
            OdbcDataReader MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(sql);

            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_class.Items.Add(li);
                }
            }
            else
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("No Class Found", "-1");
                Drp_class.Items.Add(li);
            }



        }
        private void LoadStudentsDetailsToDropDown()
        {

            if (Drp_class.SelectedValue != "-1")
            {
                int ClassId = 0;
                int.TryParse(Drp_class.SelectedValue.ToString(), out ClassId);

                Drp_Student.Items.Clear();
                string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
                OdbcDataReader MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(Sql);

                if (MyReader.HasRows)
                {
                    //System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                    //Drp_Student.Items.Add(li);

                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Student.Items.Add(Li);
                    }
                }
                else
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Student Not Found", "-1");
                    Drp_Student.Items.Add(li);
                }
            }
            else
            {

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                Drp_Student.Items.Add(li);
            }
        }
        private void LoadDocToDropDown()
        {

            if (Drp_Student.SelectedValue != "-1")
            {
                int StudId = 0;
                int.TryParse(Drp_Student.SelectedValue.ToString(), out StudId);

                Drp_Doc.Items.Clear();



                MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
                using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
                {
                    //string mop = "";
                    _MyODBCConn.Open();
                    string sql = "select Id,DocName from insertdoc where insertdoc.EmpCode = " + StudId + "";
                    OdbcDataReader MyReader = _MysqlDb.ExecuteQuery(sql);

                    if (MyReader.HasRows)
                    {
                       

                        while (MyReader.Read())
                        {
                            System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                            Drp_Doc.Items.Add(Li);
                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Document Not Found", "-1");
                        Drp_Doc.Items.Add(li);
                    }
                    //string Sql = "select tblview_student.Id, tblview_student.StudentName from tblview_student INNER join tblclass on tblview_student.ClassId=tblclass.Id where tblview_student.ClassId=" + ClassId + " and  tblview_student.LIve=1  and tblview_student.RollNo<>-1  order by  tblview_student.StudentName ASC";
                    //OdbcDataReader MyReader = MyConfigMang.m_MysqlDb.ExecuteQuery(Sql);
                }

            }
            else
            {

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("All Student", "0");
                Drp_Student.Items.Add(li);
            }

        }
        protected void Drp_SelectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsDetailsToDropDown();
        }

        protected void Drp_Student_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDocToDropDown();
        }

        protected void Btn_ShowDoc_Click(object sender, EventArgs e)
        {
            objSchool = (SchoolClass)Session[WinerConstants.SessionSchool];
            int StudId = 0;
            int SchoolId = WinerUtlity.GetSchoolId(objSchool);
            string docName = Drp_Doc.SelectedItem.Text;
            int.TryParse(Drp_Student.SelectedValue.ToString(), out StudId);
            int docId = int.Parse(Drp_Doc.SelectedValue.ToString());
            string strPath="";
            string file = "";
            string fileName = StudId + "_" + SchoolId +"_" + docName;
            string storeLocation = "";
            string path = MyUser.FilePath + "\\UpImage\\"+ fileName;
            MysqlClass _MysqlDb = new MysqlClass(ConfigurationSettings.AppSettings["DMSConnectionInfo"]);
            using (OdbcConnection _MyODBCConn = new OdbcConnection(ConfigurationSettings.AppSettings["DMSConnectionInfo"]))
            {
                //string mop = "";
                _MyODBCConn.Open();
                string sql = "select StoreLocation,Resume from insertdoc where insertdoc.Id = " + docId + "";
                OdbcDataReader MyReader = _MysqlDb.ExecuteQuery(sql);

                if (MyReader.HasRows)
                {


                    while (MyReader.Read())
                    {
                        System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                        Drp_Doc.Items.Add(Li);
                        storeLocation = MyReader.GetValue(0).ToString();
                        file = MyReader.GetValue(1).ToString();
                    }
                }
            }
            if(storeLocation == "FileExplorer")
            {
                string filePath = path;
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            }
           else
            {
                string dbpath = MyUser.FilePath + "\\UpImage\\" + fileName;
                Byte[] bytes = Convert.FromBase64String(file);
                File.WriteAllBytes(dbpath, bytes);
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(dbpath));
                Response.WriteFile(dbpath);
                Response.End();

            }


            

            //WebClient req = new WebClient();
            //HttpResponse response = HttpContext.Current.Response;
            ////string filePath = lblresume.Text;
            //response.Clear();
            //response.ClearContent();
            //response.ClearHeaders();
            //response.Buffer = true;
            //response.AddHeader("Content-Disposition", "attachment;filename=Filename.extension");
            //byte[] data = req.DownloadData(Server.MapPath(path));
            //response.BinaryWrite(data);
            //response.End();
            //{
            //    Response.ContentType = "Application/image";
            //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName +"");
            //    Response.TransmitFile(Server.MapPath(path));
            //    Response.End();
            //}



            //strPath = Server.MapPath(MyUser.FilePath + "\\UpImage\\"+ fileName);
            //if (!string.IsNullOrEmpty(strPath))
            //{


            //    FileInfo file = new FileInfo(strPath);
            //    if (file.Exists)
            //    {


            //        Response.Clear();
            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);


            //        Response.AddHeader("Content-Length", file.Length.ToString());
            //        Response.ContentType = "application/octet-stream";


            //        Response.WriteFile(file.FullName);


            //        Response.End();


            //    }


            //}

        }



        }

}