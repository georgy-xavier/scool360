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
using WinEr;
using WinBase;
using System.Collections.Generic;
public partial class ManageBatch : System.Web.UI.Page
{
    private StudentManagerClass MyStudMang;
    private ConfigManager MyConfiMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    public MysqlClass m_MysqlDb = null;

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
            Response.Redirect("sectionerr.htm");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        MyConfiMang = MyUser.GetConfigObj();
        if (MyConfiMang == null)
        {
            Response.Redirect("RoleErr.htm");
            //no rights for this user.
        }
        else if (!MyUser.HaveActionRignt(29))
        {
            Response.Redirect("RoleErr.htm");
        }
        else
        {
            if (!IsPostBack)
            {
                LoadBatch();
                //if (MyConfiMang.HaveStudentsTopromote(MyUser.CurrentBatchId))
                //{
                //    Lbl_note.Text = "Few students of last batch are not promoted yet. Please promote those students";
                //    Btn_createbatch.Enabled = false;
                //}
                //else
                //{
                //    Lbl_note.Text = "";
                //    Btn_createbatch.Enabled = true;
                //}
            }
        }
    }
    private bool havestudenttopromote()
    {
        if (MyConfiMang.HaveStudentsTopromote(MyUser.CurrentBatchId))
        {
            //Lbl_note.Text = "Few students of last batch are not promoted yet. Please promote those students";
            Btn_createbatch.Enabled = false;
            return true;
        }
        else
        {
            Lbl_note.Text = "";
            Btn_createbatch.Enabled = true;
            return false;
        }

    }


    private void LoadBatch()
    {
        //Label lb = (Label)Page.Master.FindControl("Lbl_currentBATCH");
        Lbl_currentBATCH.Text = MyUser.CurrentBatchName;
        string sql = "SELECT Id,BatchName FROM tblbatch where LastbatchId=" + MyUser.CurrentBatchId;
        MyReader = MyConfiMang.m_MysqlDb.ExecuteQuery(sql);
        if (MyReader.HasRows)
        {
            Lbl_nextbatch.Text = MyReader.GetValue(1).ToString();
            HiddenField_Newbatchid.Value = MyReader.GetValue(0).ToString();
        }
        MyReader.Close();

        Lbl_currentBATCH.Text = MyUser.CurrentBatchName;
    }
    protected void Btn1_yes_Click(object sender, EventArgs e)
    {
        StartNewBatch();
    }

    private void StartNewBatch()
    {
        string _message;

        if (MyConfiMang.CreateBatch(int.Parse(HiddenField_Newbatchid.Value), Txt_startdate.Text.ToString(), Txt_EndDate.Text.ToString(), MyUser.CurrentBatchId, out _message))
        {
            WC_MessageBox.ShowMssage("New batch is created and activated");
            MyUser.LoadCurrentbatchId();
            LoadBatch();
            MyUser.m_DbLog.LogToDbNoti(MyUser.UserName, "Batch Creation", "Batch " + MyUser.CurrentBatchName + " is Created", 1,1);
            ResetSessions();
            // ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "window.open(\"ManageBatch.aspx\");", true);
        }
        else
        {
            WC_MessageBox.ShowMssage(_message);
        }
    }

    private void ResetSessions()
    {
        if (ConfigurationSettings.AppSettings["NeedAppicationObject"] == "1")
        {
            if (Application["UserList"] != null)
            {
                List<UserInfoClass> _UaerList = new List<UserInfoClass>();
                foreach (UserInfoClass _UserObj in _UaerList) // Loop through List with foreach
                {
                    _UserObj.NeedSessionOut = true;

                }
                Application.Lock();
                Application["UserList"] = _UaerList;
                Application.UnLock();
            }

        }
    }
    protected void BtnPromotion_yes_Click(object sender, EventArgs e)
    {
       
        StartNewBatch();

        Btn_createbatch.Enabled = true;
    }

    protected void Btn_createbatch_Click(object sender, EventArgs e)
    {

        if (havestudenttopromote())
        {
            Button_promotion_popup.Show();
        }
        else
        {
            Btn_createbatch_MPE.Show();
        }
    }
}