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
using WinBase;
public partial class Staffresign : System.Web.UI.Page
{
    private StaffManager MyStaffMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    //private DataSet MydataSet;
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

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_init(object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["StaffId"] == null)
        {
            Response.Redirect("ViewStaffs.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyStaffMang = MyUser.GetStaffObj();
        if (MyStaffMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(33))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
                string _SubMenuStr;
                _SubMenuStr = MyStaffMang.GetSubStaffMangMenuString(MyUser.UserRoleId);
                this.SubStaffMenu.InnerHtml = _SubMenuStr;
                //some initlization
                AddValues();
                AddValueToDrpDown();            
                LoaduserTopData();
                Lbl_Advsal.Text = "";
                BtnResigd.Enabled = true;
                if (PayrollActive())
                {
                    double Advance = GetAdvanceSal();
                    if (Advance > 0)
                    {
                        Lbl_Advsal.Text = "Staff has to return advance salary..For Details check payroll section!";
                        BtnResigd.Enabled = false;
                    }

                }
            }
        }

    }

    private double GetAdvanceSal()
    {
        string sqladvsal = "";
        double advsal = 0;
        OdbcDataReader AdvsalReader = null;
        sqladvsal = "select tblpay_employee.Balance from tblpay_employee where tblpay_employee.StaffId=" + int.Parse(Session["StaffId"].ToString()) + "";
        AdvsalReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sqladvsal);
        if (AdvsalReader.HasRows)
        {
            double.TryParse(AdvsalReader.GetValue(0).ToString(),out advsal);
        }

        return advsal;
    }

    private bool PayrollActive()
    {
        string sql = "";
        OdbcDataReader PayIdReader = null;
        bool active = false;
        sql = "select tblpay_employee.Id from tblpay_employee where tblpay_employee.StaffId=" + int.Parse(Session["StaffId"].ToString()) + "";
        PayIdReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (PayIdReader.HasRows)
        {
            active = true;
        }
        
        return active;
    }
    private void LoaduserTopData()
    {

        string _Studstrip = MyStaffMang.ToStripString(int.Parse(Session["StaffId"].ToString()), "Handler/ImageReturnHandler.ashx?id=" + int.Parse(Session["StaffId"].ToString()) + "&type=StaffImage");
        this.StudentTopStrip.InnerHtml = _Studstrip;
    }

    private void AddValueToDrpDown()
    {
        String Sql = "SELECT Id,Reason FROM tblresignreason";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        while (MyReader.Read())
        {
            ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
            DrpDownReason.Items.Add(li);
        }
    }

    private void AddValues()
    {
        String Sql = "SELECT SurName FROM tbluser WHERE Id=" + int.Parse(Session["StaffId"].ToString());
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(Sql);
        if (MyReader.HasRows)
        {
            MyReader.Read();
            TxtName.Text = MyReader.GetValue(0).ToString();          
        }
        
    }
   

    protected void BtnResigd_Click(object sender, EventArgs e)
    {
       // BtnResigd.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnResigd, "").ToString());
        Resign();
    }

    private void Resign()
    {
        string _msg = "";
        if (IsClassTeacher(int.Parse(Session["StaffId"].ToString())))
        {
            Lbl_msg.Text = "Cannot resign because selected person is a class teacher. Please assign some other staff for the class.";
            this.MPE_MessageBox.Show();
        }
        else if (IsGroupManager(int.Parse(Session["StaffId"].ToString())))
        {
            Lbl_msg.Text = "Cannot resign because selected person is a group manager. Please assign some other user for the group ";
            this.MPE_MessageBox.Show();
        }
        else if (AnyPendingBooks(int.Parse(Session["StaffId"].ToString())))
        {
            Lbl_msg.Text = "Cannot resign because selected person has not returned all books to the library";
            this.MPE_MessageBox.Show();
        }
        else if (IsStaffAssigned_InTimetable(int.Parse(Session["StaffId"].ToString()),out _msg))
        {
            Lbl_msg.Text = _msg;
            this.MPE_MessageBox.Show();
        }
        else
        {
            string _name = TxtName.Text.ToString();
            int _id = int.Parse(Session["StaffId"].ToString());
            int _reasonid = int.Parse(DrpDownReason.SelectedValue.ToString());
            string _dis = txtDis.Text;
            DateTime _resigndate = General.GetDateTimeFromText(Txt_RelievingDate.Text);
            string ErrorMessage = "";
            if (MyStaffMang.resignstaff(_name, _id, _reasonid, _dis, _resigndate, out ErrorMessage))
            {
                MyUser.UpdateGroupMapDetails(_id, 1);
                Lbl_Message.Text = _name + " is moved to history";
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Resign Staff ", "The details Of a resigned Staff " + _name + " is entered", 1);
                this.MPE_MessageBox1.Show();
            }
            else
            {
                Lbl_msg.Text = ErrorMessage;
                this.MPE_MessageBox.Show();
            }
        }

    }

    private bool IsStaffAssigned_InTimetable(int StaffId,out string _msg)
    {
        bool valid = false;
        _msg = "";
        int count = 0;
        string sql = "select COUNT(ClassPeriodId) from tbltime_master  where StaffId=" + StaffId;
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out count);
            if (count > 0)
            {
                valid = true;
                _msg="Cannot resign because selected person has periods assigned for him in general timetable. Assign his periods to some other staff";
            }
        }

        if (!valid)
        {
            sql = "select COUNT(SubjectId) from tbltime_submaster  where StaffId=" + StaffId+" AND Day>'"+DateTime.Now.Date.ToString("s")+"'";
            MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                int.TryParse(MyReader.GetValue(0).ToString(), out count);
                if (count > 0)
                {
                    valid = true;
                    _msg = "Cannot resign because selected person has future periods assigned for him in weekly timetable. Assign his periods to some other staff";
                }
            }
        }

        return valid;
    }

    private bool IsGroupManager(int StaffId)
    {
        bool valid = false;
        int count = 0;
        string sql = "select COUNT(GroupName) from tblgroup  where ManagerId=" + StaffId;
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out count);
            if (count > 0)
            {
                valid = true;
            }
        }
        return valid;
    }

    private bool IsClassTeacher(int StaffId)
    {
        bool valid = false;
        int count = 0;
        string sql = "select count(classId) from tblclassschedule where ClassTeacherId=" + StaffId;
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out count);
            if (count > 0)
            {
                valid = true;
            }
        }
        return valid;
    }

    private bool AnyPendingBooks(int _StaffId)
    {
        bool valid = false;
        int count = 0;
        string sql = "select COUNT(tblbookissue.BookNo) from tblbookissue where tblbookissue.UserId=" + _StaffId + " and tblbookissue.UserType=2";
        MyReader = MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            int.TryParse(MyReader.GetValue(0).ToString(), out count);
            if (count > 0)
            {
                valid = true;
            }
        }
        return valid;
    }

    protected void UpdateOk_Click(object sender, EventArgs e)
    {
        if (MyUser.UserId != int.Parse(Session["StaffId"].ToString()))
        {
            Session["StaffId"] = null;
            Response.Redirect("ViewStaffs.aspx");
        }
        else
        {
            Session["StaffId"] = null;
            Response.Redirect("RoleErr.htm");
        }
    }
    
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffDetails.aspx");
    }
}
