using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;

using WinBase;
using System.Data;

namespace WinEr
{
    public partial class CCEClassGroupMap : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private DataSet MydataSet;
        private SchoolClass objSchool = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyExamMang = MyUser.GetExamObj();
            if (MyExamMang == null)
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
                if (!IsPostBack)
                {
                    //some initlization
                    LoadDefaultValues();
                    Drp_Groupselect.Focus();
                }
            }
        }

        /// <summary>
        /// loading control values
        /// </summary>
        private void LoadDefaultValues()
        {
            Load_GroupDropDown();
            Load_TermDropDown();
            Load_ClassDropDown();
            load_Grid();
        }

        private void Load_TermDropDown()
        {
            Drp_term.Items.Clear();
            string sql = "SELECT tblcce_term.Id,tblcce_term.TermName from tblcce_term";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["TermName"].ToString(), drcls["Id"].ToString());
                    Drp_term.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_term.Items.Add(li);
            }
        }

        private void Load_ClassDropDown()
        {
            Drp_ClassSelect.Items.Clear();
            string sql = "SELECT tblclass.Id,tblclass.ClassName from tblclass where tblclass.Status=1 ORDER BY tblclass.ClassName";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["ClassName"].ToString(),drcls["Id"].ToString());
                    Drp_ClassSelect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_ClassSelect.Items.Add(li);
            }

        }

        private void Load_GroupDropDown()
        {
            Drp_Groupselect.Items.Clear();
            string sql = "SELECT tblcce_classgroup.Id,tblcce_classgroup.GroupName from tblcce_classgroup ORDER BY tblcce_classgroup.Id;";
            DataSet Ds_class = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (Ds_class != null && Ds_class.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drcls in Ds_class.Tables[0].Rows)
                {
                    li = new ListItem(drcls["GroupName"].ToString(), drcls["Id"].ToString());
                    Drp_Groupselect.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_Groupselect.Items.Add(li);
            }
        }

        private void load_Grid()
        {
            string _Errmsg = "";
            try
            {
                Grd_CCE.DataSource = null;
                int _GroupId = int.Parse(Drp_Groupselect.SelectedValue);
                if (_GroupId != 0)
                {
                    //string sql = "SELECT tblcce_classgroupmap.GroupId as GroupId,tblcce_classgroupmap.ClassId as ClassId,tblcce_classgroup.GroupName as GroupName,tblclass.ClassName as ClassName from tblcce_classgroupmap inner JOIN tblcce_classgroup ON tblcce_classgroup.Id=tblcce_classgroupmap.GroupId inner JOIN tblclass ON tblclass.Id=tblcce_classgroupmap.ClassId WHERE tblcce_classgroupmap.GroupId=" + _GroupId;
                    string sql = "SELECT tblcce_classgroupmap.GroupId as GroupId,tblcce_classgroupmap.ClassId as ClassId,tblcce_term.Id as TermId,tblcce_classgroup.GroupName as GroupName,tblclass.ClassName as ClassName,tblcce_term.TermName as TermName from tblcce_classgroupmap inner JOIN tblcce_classgroup ON tblcce_classgroup.Id=tblcce_classgroupmap.GroupId inner JOIN tblclass ON tblclass.Id=tblcce_classgroupmap.ClassId inner join tblcce_term on tblcce_term.Id=tblcce_classgroupmap.Termid WHERE tblcce_classgroupmap.GroupId="+_GroupId;
                    MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                    if (MydataSet.Tables[0].Rows.Count > 0)
                    {
                        Grd_CCE.Columns[0].Visible = true;
                        Grd_CCE.Columns[1].Visible = true;
                        Grd_CCE.Columns[2].Visible = true;
                        Grd_CCE.Columns[3].Visible = true;
                        Grd_CCE.DataSource = MydataSet;
                        Grd_CCE.DataBind();
                        Grd_CCE.Columns[0].Visible = false;
                        Grd_CCE.Columns[1].Visible = false;
                        Grd_CCE.Columns[2].Visible = false;
                        Grd_CCE.Columns[3].Visible = false;
                        divgrid.Visible = true;
                        Label1.Visible = false;
                    }
                    else
                    {
                        divgrid.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = "No Data Found!";
                    }

                }
            }
            catch (Exception ex)
            {
                _Errmsg = ex.Message;
            }
            if (_Errmsg != "")
            {
                WC_MessageBox.ShowMssage(_Errmsg);

            }
        }

        /// <summary>
        /// this event removing class group map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grd_studentlist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Lbl_groupid.Text=Convert.ToString(e.CommandArgument);
            MPE_yesornoMessageBox.Show();
        }
     
        protected void Grd_CCE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_CCE.PageIndex = e.NewPageIndex;
            load_Grid();
        }

        /// <summary>
        /// create class group map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_map_Click(object sender, EventArgs e)
        {            
            string _Msg = "";
            int _GroupId = int.Parse(Drp_Groupselect.SelectedValue);
            int _ClassId = int.Parse(Drp_ClassSelect.SelectedValue);
            int _Termid = int.Parse(Drp_term.SelectedValue);
            if (_GroupId == 0)
                _Msg = "Group is not found!.Create new group on group config";
            else if (_ClassId == 0)
                _Msg = "Class is not found";
            else if (_Termid == 0)
                _Msg = "Term is not found";
           
            //Edited By Mobin
            if (_Msg == "")
            {
                if (!Validation_And_Mapping(_GroupId, _ClassId, _Termid, out _Msg))
                {
                    load_Grid();
                }
            }

            WC_MessageBox.ShowMssage(_Msg);
        }

        /// <summary>
        /// while creating this function checking validation
        /// </summary>
        /// <param name="_GroupId"></param>
        /// <param name="_ClassId"></param>
        /// <param name="_Termid"></param>
        /// <param name="_RtnMsg"></param>
        /// <returns></returns>
        private bool Validation_And_Mapping(int _GroupId, int _ClassId,int _Termid,out string _RtnMsg)
        {
            CLogging logger = CLogging.GetLogObject();
            //Edited By Mobin
            bool _IsExist=false;
            _RtnMsg = "";
            string sql = "SELECT tblcce_classgroupmap.GroupId as Groupid from tblcce_classgroupmap where  tblcce_classgroupmap.ClassId=" + _ClassId + " and tblcce_classgroupmap.GroupId=" + _GroupId + " and tblcce_classgroupmap.Termid="+_Termid;
            DataSet Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                string str_id=Ds.Tables[0].Rows[0]["Groupid"].ToString();
                int _id=0;
                if(str_id!="")
                    _id=int.Parse(str_id);
                if(str_id!="" && _id>0)
                    _IsExist = true;
                _RtnMsg = "This class is already mapped from another group!";
            }
            if (!_IsExist)
            {
                sql = "INSERT into tblcce_classgroupmap(ClassId,GroupId,Termid) VALUES (" + _ClassId + "," + _GroupId + "," + _Termid + ")";
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                _RtnMsg = "Class mapped sucessfully!";
                logger.LogToFile("Class Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " is mapped from " + Drp_Groupselect.SelectedItem.Text.ToString(), 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " is mapped from " + Drp_Groupselect.SelectedItem.Text.ToString(), 1);

            }
            return _IsExist;
        }

        protected void Drp_Groupselect_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_Grid();
        }
       
        protected void Grd_CCE_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// this event while removing class group map it will ask if remove sure or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_yes_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err = "";
            string sql = "";
            string _Groupid = "0";
            string _Classid = "0";
            int index = 0;
            int.TryParse(Lbl_groupid.Text.ToString(),out index);
            _Groupid = Grd_CCE.Rows[index].Cells[0].Text;
            _Classid = Grd_CCE.Rows[index].Cells[1].Text;
            if (_Groupid != "0" && _Classid != "0")
            {
                sql = "SELECT tblstudent.Id from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id where tblstudentclassmap.ClassId=" + _Classid;
                DataSet StuId_Ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (StuId_Ds.Tables[0].Rows.Count > 0 && StuId_Ds != null)
                {
                    int _StuId = 0;
                    foreach (DataRow dr in StuId_Ds.Tables[0].Rows)
                    {
                        int.TryParse(dr[0].ToString(), out _StuId);
                        sql = "DELETE from tblcce_descriptive where tblcce_descriptive.StudentId=" + _StuId;
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        sql = "DELETE from tblcce_mark where tblcce_mark.StudentId=" + _StuId;
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                        sql = "DELETE from tblcce_result where tblcce_result.StudentId=" + _StuId;
                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                    }
                }
                sql = "DELETE FROM tblcce_classgroupmap WHERE tblcce_classgroupmap.ClassId=" + _Classid + " AND tblcce_classgroupmap.GroupId=" + _Groupid;
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                _Err = "Selected map deleted sucessfully";
                logger.LogToFile("Class Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " removed from " + Drp_Groupselect.SelectedItem.Text.ToString(), 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class Configuration", Drp_ClassSelect.SelectedItem.Text.ToString() + " removed from " + Drp_Groupselect.SelectedItem.Text.ToString(), 1);
                load_Grid();
            }
            else
            {
                _Err = "Selected map not deleted sucessfully!";
                logger.LogToFile("Class configuration", "throws Error" + _Err, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Class Configuration", "throws Error" + _Err, 1);

            }
            WC_MessageBox.ShowMssage(_Err);
        }

    }
}
