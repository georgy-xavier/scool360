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
using WinBase;

//Web.Generic.DataGridTools 

public partial class ManageBulkStudent : System.Web.UI.Page
{
    private ClassOrganiser MyClassMang;
    private KnowinUser MyUser;
    private OdbcDataReader MyReader = null;
    private DataSet MydataSet;
    private Incident Myincident;
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
        if (Session["UserObj"] == null)
        {
            Response.Redirect("sectionerr.htm");
        }
        if (Session["ClassId"] == null)
        {
            Response.Redirect("LoadClassDetails.aspx");
        }
        MyUser = (KnowinUser)Session["UserObj"];
        if (MyUser == null)
        {
            Response.Redirect("RoleErr.htm");
        }
        MyClassMang = MyUser.GetClassObj();
        Myincident = MyUser.GetIncedentObj();
        if (MyClassMang == null)
        {
            Response.Redirect("sectionerr.htm");
            //no rights for this user.
        }
        else
        {
            LoadStudents();

            if (!IsPostBack)
            {
                //string _MenuStr;
                //_MenuStr = MyClassMang.GetClassMangSubMenuString(MyUser.UserRoleId, MyUser.SELECTEDMODE);
                TopDetails();
                LoadStudents();

                if (!MyUser.HaveActionRignt(4))
                {
                    // Img_StudentPreview.Visible = false;
                }
            }
        }
    }

    private void TopDetails()
    {
        FillClassInDrp();
        drp_selectClass.SelectedValue = Session["ClassId"].ToString();
        LoadStudentsNo();
    }

    private void LoadStudentsNo()
    {
        int students = 0, girls = 0, boys = 0;
       MyClassMang.getClassStuds(int.Parse(Session["ClassId"].ToString()),MyUser.CurrentBatchId,out students,out boys, out girls);
     
              lbl_students.Text = students.ToString();
              lbl_boys.Text = boys.ToString();
              lbl_girls.Text = girls.ToString();
    }

    private void FillClassInDrp()
    {
        drp_selectClass.Items.Clear();
        MydataSet = MyUser.MyAssociatedClass();
        if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in MydataSet.Tables[0].Rows)
            {
                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                drp_selectClass.Items.Add(li);
            }
        }
        else
        {
            ListItem li = new ListItem("No Class Present", "-1");
            drp_selectClass.Items.Add(li);
        }
    }

    private void LoadStudents()
    {
        string sql = "SELECT tblstudent.Id,tblstudent.StudentName,tblstudent.AdmitionNo,tblstudent.Sex, DATE_FORMAT( tblstudent.DOB, '%d/%m/%Y') as DOB, tblstudent.GardianName, tblstudent.ResidencePhNo, tblstudent.OfficePhNo,tblbloodgrp.GroupName, tblstudent.Religion as ReligionId , tblstudent.Cast as CastId ,BustOption.Option AS `SchoolBus`,HostelOption.Option AS `Hostel` from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id inner join tblbloodgrp on tblbloodgrp.Id = tblstudent.BloodGroup inner join tbloptionmaster BustOption on BustOption.Id=tblstudent.UseBus inner join tbloptionmaster HostelOption on HostelOption.Id=tblstudent.UseHostel WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + int.Parse(Session["ClassId"].ToString()) + " Order by tblstudentclassmap.RollNo ASC";
        MydataSet = MyClassMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        if (MydataSet.Tables[0].Rows.Count > 0)
        {
            BindReligionCast(MydataSet);
            Grd_Students.Columns[0].Visible = true;
            Grd_Students.DataSource = MydataSet;
            Grd_Students.DataBind();
            Grd_Students.Columns[0].Visible = false;
            Pnl_studlist.Visible = true;
            LoadOtherDetails();

            if (Session["SelectedIndex"] != null)
            {
                int _selectedIndex=-1;
                if (int.TryParse(Session["SelectedIndex"].ToString(), out _selectedIndex))
                {
                    Grd_Students.SelectedIndex = _selectedIndex;
                }
            }

        }
        else
        {
            Pnl_studlist.Visible = false;
            //Lbl_msg.Text = "No Students found in the Class";
            //this.MPE_MessageBox.Show();
        }
    }

    private void BindReligionCast(DataSet MydataSet)
    {

        MydataSet.Tables[0].Columns.Add("Religion");
        MydataSet.Tables[0].Columns.Add("CastName");
        string Religion, cast;
        foreach (DataRow dr in MydataSet.Tables[0].Rows)
        {
            dr["Religion"] = MyClassMang.ReligionName(dr["ReligionId"].ToString());
            dr["CastName"] = MyClassMang.CastName(dr["CastId"].ToString());
        }

    }

    private void LoadOtherDetails()
    {
        foreach (GridViewRow gv in Grd_Students.Rows)
        {

            Image Img_stud = (Image)gv.FindControl("Img_studImage");
            Label Lbl_RollNumber = (Label)gv.FindControl("Lbl_RollNumber");

            Img_stud.ImageUrl = "Handler/ImageReturnHandler.ashx?id=" + int.Parse(gv.Cells[0].Text.ToString()) + "&type=StudentImage";
          
            Lbl_RollNumber.Text = MyClassMang.GetRollnumber(int.Parse(gv.Cells[0].Text.ToString()), int.Parse(Session["ClassId"].ToString()), MyUser.CurrentBatchId);
        }
    }


    protected void Img_StudentPreview_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("StudentImageSlider.aspx");
    }


    protected void Grd_Students_SelectedIndexChanged(object sender, EventArgs e)
    {
        // int RegID = int.Parse(Grd_Students.SelectedRow.Cells[1].Text.ToString());
        if (Grd_Students.Rows.Count > 0)
        {
            Session["SelectedIndex"] = Grd_Students.SelectedIndex;
            int id = int.Parse(Grd_Students.SelectedRow.Cells[0].Text.ToString());
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "AnyScriptNameYouLike", "openIncpopup('ManageStudentBulk.aspx?StudId=" + id + "');", true);

        }
    }

    protected void Btn_UpdateGeneraldetails_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_updateotherdetails_Click(object sender, EventArgs e)
    {

    }

    protected void drp_selectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ClassId"]=drp_selectClass.SelectedItem.Value;
        LoadStudents();
        LoadStudentsNo();
    }
} 
