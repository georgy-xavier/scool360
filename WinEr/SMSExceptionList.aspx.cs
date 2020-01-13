using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;

namespace WinEr
{
    public partial class SMSExceptionList : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;

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
                Response.Redirect("sectionerr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else if (!MyUser.HaveActionRignt(928))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadConfigurationDropDown();
                    LoadClassToDropDown();
                }
            }
        }

        private void LoadClassToDropDown()
        {
            Drp_SearcgClass.Items.Clear();
            DataSet MydataSet = new DataSet();
            ListItem li = new ListItem("All Class", "0");
            Drp_SearcgClass.Items.Add(li);

            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {

                    li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    Drp_SearcgClass.Items.Add(li);

                }

            }
            else
            {
                li = new ListItem("No Class present", "-1");
                Drp_SearcgClass.Items.Add(li);
            }
        }

        protected void Drp_Config_SelectedIndexCahnged(object sender, EventArgs e)
        {
          
            try
            {
                LoadStudentExceptionList();
            }
            catch
            {
                Lbl_Msg.Text = "Can't load details..";
            }
        }

        private void LoadStudentExceptionList()
        {
            int configid = 0;
            DataSet StudentExceptionList = new DataSet();
            int.TryParse(Drp_Config.SelectedValue, out configid);
            StudentExceptionList = MysmsMang.GetStudentExceptionList(configid);
            BindGridView(StudentExceptionList);
        }

        protected void Btn_AddNew_Click(object sender, EventArgs e)
        {
            int configvalue = 0;
            int.TryParse(Drp_Config.SelectedValue, out configvalue);
            if (configvalue <= 0)
            {
                Lbl_Msg.Text = "Select any configuration..";
            }
            else
            {
                Lbl_Err.Text = "";
                Btn_Save.Visible = false;
                Pnl_student.Visible = false;
                Grd_Searchstudent.DataSource = null;
                Grd_Searchstudent.DataBind();   
                MPE_ADDNEWSTUDEENTS.Show();
            }

        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            try
            {                
                LoadStudents();
            }
            catch (Exception)
            {
                Lbl_Err.Text = "Can't load student..";
            }
            MPE_ADDNEWSTUDEENTS.Show();

        }

        protected void Btn_Remove_Click(object sender, EventArgs e)
        {
            CheckBox chk = new CheckBox();
            int chcount = 0;
            try
            {
                foreach (GridViewRow gr in GridStudentExceptionList.Rows)
                {
                    chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    if (chk.Checked)
                    {
                        chcount++;
                       MysmsMang.RemoveStudentfromExceptionList(gr.Cells[2].Text);
                    }
                }
                if (chcount == 0)
                {
                    Lbl_Msg.Text = "Please select any student";
                }
                else
                {
                    Lbl_Msg.Text = "Student has been removed successfully...";
                    LoadStudentExceptionList();
                }
            }
            catch (Exception)
            {
                Lbl_Msg.Text = "Can't remove student,Please try later..";
            }
        }

        protected void GridStudentExceptionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridStudentExceptionList.PageIndex = e.NewPageIndex;
            LoadStudentExceptionList();
        }

        private void BindGridView(DataSet StudentExceptionList)
        {
            Lbl_Msg.Text = "";
            if (StudentExceptionList != null && StudentExceptionList.Tables[0].Rows.Count > 0)
            {                
                GridStudentExceptionList.Columns[1].Visible = true;
                GridStudentExceptionList.Columns[2].Visible = true;
                GridStudentExceptionList.DataSource = StudentExceptionList;
                GridStudentExceptionList.DataBind();
                GridStudentExceptionList.Columns[1].Visible = false;
                GridStudentExceptionList.Columns[2].Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "No students found in exception list..!";
                GridStudentExceptionList.DataSource = null;
                GridStudentExceptionList.DataBind();
            }
        }

        private void LoadConfigurationDropDown()
        {
            OdbcDataReader MyReader =null; 
            Drp_Config.Items.Clear();
            string sql = "SELECT Id,`Type` FROM tblsmsoptionconfig WHERE ShortName<>'' AND SetVisible=1";
            MyReader = MysmsMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Drp_Config.Items.Add(new ListItem("Select SMS Configurations", "0"));
                while (MyReader.Read())
                {

                    ListItem li = new ListItem(MyReader.GetValue(1).ToString(), MyReader.GetValue(0).ToString());
                    Drp_Config.Items.Add(li);
                }
            }
            else
            {
                Drp_Config.Items.Add(new ListItem("No SMS Configuration Presents", "-1"));
            }
        }

        private void LoadStudents()
        {
            // throw new NotImplementedException();
            string sql = "";
            Lbl_Err.Text = "";
            DataSet StudentDs = new DataSet();
            string Tempsql = "";
            int classid = 0;
            int.TryParse(Drp_SearcgClass.SelectedValue, out classid);
            if (Txt_StudentName.Text != "")
            {
                Tempsql = " and StudentName='" + Txt_StudentName.Text + "'";

            }
            if (Txt_ParentName.Text != "")
            {
               
                    Tempsql = Tempsql + " and GardianName='" + Txt_ParentName.Text + "'";
                
            }
            if (Txt_PhoneNum.Text != "")
            {
                  Tempsql = Tempsql + " and OfficePhNo='" + Txt_PhoneNum.Text + "'";
               

            }
            if (classid>0)
            {
                Tempsql = Tempsql + " and LastClassId=" + classid + "";


            }
            sql = "select Id,StudentName,OfficePhNo from tblstudent where Id not in (select tblsmsexceptionlist.ParentId from tblsmsexceptionlist  where tblsmsexceptionlist.ConfigId="+Drp_Config.SelectedValue+") " + Tempsql;
            StudentDs = MyStudMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            if (StudentDs != null && StudentDs.Tables[0].Rows.Count > 0)
            {
                Btn_Save.Visible = true;
                Pnl_student.Visible = true;
                Grd_Searchstudent.Columns[1].Visible = true;
                Grd_Searchstudent.DataSource = StudentDs;
                Grd_Searchstudent.DataBind();
                Grd_Searchstudent.Columns[1].Visible = false; ;

            }
            else
            {
                Btn_Save.Visible = false;
                Lbl_Err.Text = "No students found";
                Pnl_student.Visible = false;
                Grd_Searchstudent.DataSource = null;
                Grd_Searchstudent.DataBind();
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            //ParentId,PhoneNumber,ConfigId
            int configid = 0, parentid = 0;
            int chkcount = 0;
            CheckBox chk = new CheckBox();
            string phonenumber = "";
            try
            {
                foreach (GridViewRow gr in Grd_Searchstudent.Rows)
                {
                    chk = (CheckBox)gr.FindControl("CheckBoxUpdate");
                    if (chk.Checked)
                    {
                        chkcount++;
                        phonenumber = gr.Cells[3].Text.Replace("&nbsp;","");
                        int.TryParse(gr.Cells[1].Text.Replace("&nbsp;",""), out parentid);
                        int.TryParse(Drp_Config.SelectedValue, out configid);
                        MysmsMang.InsertInToExceptionList(phonenumber, parentid, configid);
                    }
                }
                if (chkcount == 0)
                {
                    Lbl_Err.Text = "Select any student..";

                }
                else
                {
                    WC_MessageBox.ShowMssage("Student added to exception list successfully..!");
                    LoadStudentExceptionList();
                    LoadStudents();
                }
            }
            catch (Exception)
            {
                Lbl_Err.Text = "Can't save,Please try later..!";
            }
            MPE_ADDNEWSTUDEENTS.Show();

        }

    }
}
