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
using System.IO;
public partial class CreateClass : System.Web.UI.Page
{
    
    private ClassOrganiser MyClassMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];

        if (MyUser.SELECTEDMODE == 2)
        {
            this.MasterPageFile = "~/WinerSchoolMaster.master";

        }
        else if (MyUser.SELECTEDMODE == 1)
        {
            this.MasterPageFile = "~/WinerStudentMaster.master";
        }

    }

    protected void Page_init(object sender, EventArgs e)
    {

        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyClassMang = MyUser.GetClassObj();
        if (MyClassMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(16))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {


            if (!IsPostBack)
            {
              
                AddStandardToDrpList(0);
                //AddBatchToDrpList(0);
                //AddClassTeacherToDrpList(0);
                AddpParentGrpToDrpList(0);
               // AddClassRoomNmbrToDrpList(0);
                if (MyUser.HaveModule(3))
                {
                    AddSubjectsToList();
                    Pnl_subject.Visible = true;
                }
                else
                {
                    Pnl_subject.Visible = false;
                }
                //some initlization

            }
        }
    }
    private void AddSubjectsToList()
    {
        ChkBox_AllsSub.Items.Clear();
        string sql = "SELECT Id,subject_name FROM tblsubjects";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                
                ChkBox_AllsSub.Items.Add(li);
           }
        }

    }
  

    private void AddpParentGrpToDrpList(int _intex)
    {
        Drp_parent.Items.Clear();
        DataSet myDataset;
        myDataset = MyUser.MyAssociatedGroups();
        if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in myDataset.Tables[0].Rows)
            {

                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                Drp_parent.Items.Add(li);

            }
    
        }
        else
        {

            ListItem li = new ListItem("No Groups", "-1");
            Drp_parent.Items.Add(li);
            
        }
        Drp_parent.SelectedIndex = 0;
    }
  private void AddStandardToDrpList(int _intex)
    {

        Drp_Stand.Items.Clear();

        string sql = "SELECT Id,Name FROM tblstandard";
        MyReader = MyClassMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            while (MyReader.Read())
            {
                ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                Drp_Stand.Items.Add(li);
            }
            Drp_Stand.SelectedIndex = _intex;
        }
    }
    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ChkBox_AllsSub.Items.Count; i++)
        {
            if (ChkBox_AllsSub.Items[i].Selected)
            {
                ChkBox_AllsSub.Items[i].Selected = false;
                ChkBox_Classsubject.Items.Add(ChkBox_AllsSub.Items[i]);
                ChkBox_AllsSub.Items.Remove(ChkBox_AllsSub.Items[i]);
                i--;
            }
        }
    }
    protected void Btn_Remove_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
        {
            if (ChkBox_Classsubject.Items[i].Selected)
            {
                ChkBox_Classsubject.Items[i].Selected = false;
                ChkBox_AllsSub.Items.Add(ChkBox_Classsubject.Items[i]);
                ChkBox_Classsubject.Items.Remove(ChkBox_Classsubject.Items[i]);
                i--;
            }
        }

    }

    protected void Btn_CreateClass_Click1(object sender, EventArgs e)
    {

        if (Txt_Name.Text.Trim() == "")
        {
            Lbl_msg.Text = "One or more fields are empty";
        }
        else if (!MyClassMang.AvailableClassName(Txt_Name.Text.ToString()))
        {
            Lbl_msg.Text = "Class name already exists";
        }
        else if (Drp_parent.SelectedValue.ToString()=="-1")
        {
            Lbl_msg.Text = "Class Must need a parent group";
        }
        else if ((ChkBox_Classsubject.Items.Count == 0) && (MyUser.HaveModule(3)))
        {
            Lbl_msg.Text = "Please select subjects for this class";
        }
        else
        {
            int _ClassiD;
            _ClassiD=MyClassMang.CreateClass(Txt_Name.Text.ToUpper(), int.Parse(Drp_Stand.SelectedValue.ToString()), int.Parse(Drp_parent.SelectedValue.ToString()), int.Parse(txt_TotalSeats.Text));
            if (MyUser.HaveModule(3))
            {
                AddClassSubjects(_ClassiD);
            }
            MyUser.ClearAssociatedClass();
            Lbl_msg.Text = "Class is Created";
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Create Class", "A new Class " + Txt_Name.Text + " is created", 1,1);
            Clear();
        }
        MPE_MessageBox.Show();
    }

    private void Clear()
    {
        Txt_Name.Text = "";
        txt_TotalSeats.Text = "";
        AddSubjectsToList();
        ChkBox_Classsubject.Items.Clear();
        
    }
    private void AddClassSubjects(int _classid)
    {
        for (int i = 0; i < ChkBox_Classsubject.Items.Count; i++)
        {
            MyClassMang.AddSujectToClass(int.Parse(ChkBox_Classsubject.Items[i].Value.ToString()), _classid);

        }

    }
    protected void But_Reset_Click(object sender, EventArgs e)
    {
        Clear();
       

    }
    protected void Btn_link_Click(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        Lbl_msg.Text = pageName;
        MPE_MessageBox.Show();

    }
}
