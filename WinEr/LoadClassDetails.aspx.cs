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

public partial class LoadClassDetails : System.Web.UI.Page
{
    private ClassOrganiser MyClassMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
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
    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        MyUser = (KnowinUser)Session["UserObj"];

        MyClassMang = MyUser.GetClassObj();

        if (!MyUser.AnyClassExists())
        {
            Response.Redirect("ErrorRedirect.aspx?Error=You cannot use this page without creating any class&&NextPage=Create Class&&URL=CreateClass.aspx");
        }
        if (MyClassMang == null)
        {
            Response.Redirect("Default.aspx");
            //no rights for this user.
        }
        else
        {
            if (!IsPostBack)
            {
                //AddClassNameToDrpList();
                //some initlization
                Load_ClassDetails();
            }
        }
    }
    private void Load_ClassDetails()
    {
        string FirstClassId = "", FirstClassName="";
        bool FirstClassIdStores = false;
        bool FirstClassNameStores = false;
        string DocString = GetDocString("ClassDock");
        string[] Temp = DocString.Split('-');
        string[] PageName = Temp[0].Split(',');
        string[] Link = Temp[1].Split(',');
        string[] ImageUrl = Temp[2].Split(',');
        string TableStr = "<br /><div class=\"container-fluid\"><div class=\"row\">";
        string DocItem = "<div class=\"container-fluid well\"><div class=\"row\">", ClassStr = "";
        int No_Students = 0, TotalSeats = 0, Boys = 0, Girls = 0;
        string TeacherName = "";
        int Count = 1;
        MydataSet = MyUser.MyAssociatedClass();
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            string StudentsString = "";
            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                No_Students = GetNo_StudentsInClass(dr[0].ToString());
                TeacherName = GetTeacherName(dr[0].ToString());
                TotalSeats = GetSeatCount(dr[0].ToString());
                //StudentsString = No_Students.ToString();
                //if (No_Students > 0)
                //{
                    Boys = GetNo_Boys(dr[0].ToString());
                    Girls = No_Students - Boys;
                    StudentsString = StudentsString + "<div style=\"cursor: pointer;\" class=\"col-md-3 col-xs-6\" onclick='GoToOption(\"" + dr[0].ToString() + "\",\"" + dr[1].ToString() + "\")'><div  id=" + dr[0].ToString() + " class=\"well ClsSeln\" style=\"padding:2px;box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.16), 0 2px 1px 0px rgba(0, 0, 0, 0.12);\"><h4 style=\"text-align: -webkit-center;\">" + dr[1].ToString() + "</h4><div id=\"horizontal\"><div><i class=\"fa fa-users\" style=\"color: #3F51B5;\"></i> Students : " + No_Students + "</div><div> <i class=\"fa fa-male\" style=\"color:#2196F3;\"></i> : " + Boys + "</div><div><i class=\"fa fa-female\" style=\"color:hotpink;\"></i> : " + Girls + "</div></div></a><hr />";
                    StudentsString += "<p>Teacher :"+ TeacherName +"</p><p>Total seat : " + TotalSeats + "</p>";
                    StudentsString += "</div> </div>";
                //}
                //<p>classId : " + dr[0].ToString() + "</p>
                if (!FirstClassIdStores)
                {
                    FirstClassId = dr[0].ToString();
                    FirstClassIdStores = true;
                }
                if (!FirstClassNameStores)
                {
                    FirstClassName = dr[1].ToString();
                    FirstClassNameStores = true;
                }
                Count++;
              
            }
            ClassStr = StudentsString;
        }
        DocItem += "<div class=\"col-md-2 col-xs-12\"><div class=\"row\">Selected Class</div><br /><div class=\"row\"><h4><div id=\"Clsname\"></div></h4></div><div class=\"row\"><h6 onclick='GoToCls()' style=\"cursor:pointer;color:blue;text-decoration:underline;\">View Class</h6></div></div>";
        for (int i = 0; i < PageName.Length; i++)
        {
            if (i > 0)
            {
                DocItem += "<div class=\"col-md-1 col-xs-6\" style=\"cursor:pointer;\"><div class=\"dock-item\" onclick='GoToPage(\"" + Link[i] + "\")'><img src=\"" + ImageUrl[i] + "\" style=\"width:60%;\" alt=\"" + PageName[i] + "\" title=\"" + PageName[i] + "\" /><p style=\"margin:0px;\">" + PageName[i] + "</p></div></div>";
            }
        }
        DocItem += "</div></div><hr />";
        string _InnerHtml = "<script type=\"text/javascript\">$(function() {initCookieId(\"" + FirstClassName + "\",\"" + FirstClassId + "\")}); function GoToOption(Frmid,FrmName){var ClsId1; if(Frmid){ClsId1 = Frmid;}document.cookie =\"SlctClsnm=\"+FrmName+\";\";document.cookie =\"SlctClsId=\"+ClsId1+\";\";$(\"#Clsname\").empty().prepend(FrmName);}; $(document).ready(function(){loadCls(\"" + FirstClassId + "\",\"" + FirstClassName + "\");});</script>";
        this.javascriptId.InnerHtml = _InnerHtml;
        TableStr = DocItem + TableStr + ClassStr + "</div></div>";
        this.ClassDetails.InnerHtml = TableStr;
        if (Session["ClassId"] != null)
        {
            int ClassId;
            if (int.TryParse(Session["ClassId"].ToString(), out ClassId))
            {
                FirstClassId = ClassId.ToString();
            }
        }
    }

    private int GetNo_Boys(string ClassId)
    {
        int Count = 0;
        string sql = "select count(stud.Sex) as totalMale from tblstudent stud inner join tblstudentclassmap map on map.StudentId=stud.Id  where stud.Sex='Male' and map.ClassId=" + ClassId + " and stud.Status=1 and map.BatchId=" + MyUser.CurrentBatchId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out Count);
        }
        return Count;
    }

    private int GetSeatCount(string ClassId)
    {
        int Count = 0;
        string sql = "SELECT TotalSeats FROM tblclass where Id=" + ClassId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out Count);
        }
        return Count;
    }

    private string GetTeacherName(string ClassId)
    {
        string Name = "None";
        string sql = "select tbluser.SurName from tbluser inner join tblclass on tblclass.ClassTeacher= tbluser.Id  where tblclass.Id=" + ClassId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Name = MyReader.GetValue(0).ToString();
        }
        return Name;
    }

    private int GetNo_StudentsInClass(string ClassId)
    {
        int Count = 0;
        string sql = "select count(tblstudentclassmap.StudentId) as totalStud from  tblclass  inner join tblstudentclassmap  on tblstudentclassmap.ClassId=tblclass.Id  inner join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId inner join tblstandard on tblstandard.Id = tblclass.Standard where tblclass.Id=" + ClassId + " and tblstudent.Status=1 AND tblstudentclassmap.batchId=" + MyUser.CurrentBatchId;
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out Count);
        }
        return Count;
    }

    private string GetDocString(string Type)
    {
        string JointString = "";
        string PageName = "Class Details", Link = "ClassDetails.aspx", Imgurl = "Pics/Details.png";
        string sql = "SELECT DISTINCT tbldockaction.DisplayName, tbldockaction.Link,tbldockaction.Imgurl FROM tbldockaction INNER JOIN  tblroleactionmap ON tbldockaction.ActionId = tblroleactionmap.ActionId INNER JOIN tblmodule ON tblmodule.Id=tblroleactionmap.ModuleId WHERE tbldockaction.`Type`='" + Type + "' AND  tblroleactionmap.RoleId=" + MyUser.UserRoleId + " AND tblmodule.`ModuleType`in(" + MyUser.SELECTEDMODE + ",3) ORDER BY tbldockaction.`Order`";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                PageName = PageName + "," + MyReader.GetValue(0).ToString();
                Link = Link + "," + MyReader.GetValue(1).ToString();
                Imgurl = Imgurl + "," + MyReader.GetValue(2).ToString();
            }
        }
        JointString = PageName + "-" + Link + "-" + Imgurl;
        return JointString;
    }





    # region old ClassSearch

    //private void AddClassNameToDrpList()
    //{
    //    Drp_ClassName.Items.Clear();

    //    MydataSet = MyUser.MyAssociatedClass();
    //    if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in MydataSet.Tables[0].Rows)
    //        {

    //            ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
    //            Drp_ClassName.Items.Add(li);

    //        }

    //    }
    //    else
    //    {
    //        ListItem li = new ListItem("No Class present", "-1");
    //        Drp_ClassName.Items.Add(li);
    //    }
    //    Drp_ClassName.SelectedIndex = 0;
    //}
    //protected void Btn_Load_Click(object sender, EventArgs e)
    //{
    //    if (Drp_ClassName.SelectedValue.ToString() != "-1")
    //    {
    //        int i_SelectedClassId = int.Parse(Drp_ClassName.SelectedValue.ToString());
    //        Session["ClassId"] = i_SelectedClassId;

    //        Response.Redirect("ClassDetails.aspx");
    //    }
    //    else
    //    {
    //        Lbl_msg.Text = "No Class is selected";
    //        this.MPE_MessageBox.Show();
    //    }
    //}

    # endregion
}
