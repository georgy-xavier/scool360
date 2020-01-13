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
    public partial class MappingStaff : System.Web.UI.Page
    {
        private StaffManager MyStaffMang;
        //private WinBase.Payroll Mypay;
        private KnowinUser MyUser;
        private OdbcDataReader Myreader = null;


        #region Events

            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["UserObj"] == null)
                {
                    Response.Redirect("sectionerr.htm");
                }
                MyUser = (KnowinUser)Session["UserObj"];
                MyStaffMang = MyUser.GetStaffObj();
                if (MyStaffMang == null)
                {
                    Response.Redirect("RoleErr.htm");
                    //no rights for this user.
                }
                else if (!MyUser.HaveActionRignt(837))
                {
                    Response.Redirect("RoleErr.htm");
                }
                else
                {
                    lbl_err.Text = "";
                    if (!IsPostBack)
                    {
                        lbl_err.Text = "";
                        pnlmapstaff.Visible = false;
                        LoadSubject();
                    }
                }
            }

            protected void drp_subject_SelectedIndexChanged(object sender, EventArgs e)
            {
                lbl_err.Text = "";
                if (int.Parse(drp_subject.SelectedValue) > 0)
                {
                    FillGridView();
                }
                else
                {
                    pnlmapstaff.Visible = false;
                    Grd_MappingStaff.DataSource = null;
                    Grd_MappingStaff.DataBind();
                }


            }

            protected void btn_Save_Click(object sender, EventArgs e)
            {
                DropDownList Drpstaff1 = new DropDownList();
                DropDownList Drpstaff2 = new DropDownList();
                int temp = 0;
                foreach (GridViewRow gr in Grd_MappingStaff.Rows)
                {
                    Drpstaff1 = (DropDownList)gr.FindControl("drp_staff1");
                    Drpstaff2 = (DropDownList)gr.FindControl("drp_staff2");
                    TableCell tableCell = (TableCell)Drpstaff1.Parent;
                    GridViewRow row = (GridViewRow)tableCell.Parent;
                    Grd_MappingStaff.SelectedIndex = row.RowIndex;
                    if (int.Parse(Drpstaff1.SelectedValue) == -1 && int.Parse(Drpstaff2.SelectedValue) != -1)
                    {
                        lbl_err.Text = "Select staff 1 for class "+Grd_MappingStaff.Rows[row.RowIndex].Cells[1].Text.ToString()+"";
                        temp = 1;
                        break;
                    }                   
                    if ((int.Parse(Drpstaff1.SelectedValue) != -1 && int.Parse(Drpstaff2.SelectedValue) != -1))
                    {

                        if (int.Parse(Drpstaff1.SelectedValue) == int.Parse(Drpstaff2.SelectedValue))
                        {
                            lbl_err.Text = "You cannot add same staff as staff1 and staff2 for " + Grd_MappingStaff.Rows[row.RowIndex].Cells[1].Text.ToString() + "";
                            temp = 1;
                            break;
                        }                        
                    }
                    
                }
                if (temp!=1)
                {
                    SaveDataToTable();
                    lbl_err.Text = "All datas are saved";
                }              
                
            }

            protected void Grd_MappingStaff_SelectedIndexChanged(object sender, EventArgs e)
            {
                
            }

            protected void drp_staff2_SelectedIndexChanged(object sender, EventArgs e)
            {
                DropDownList drpstaff = (DropDownList)sender;
                DropDownList drp1 = new DropDownList();
                DropDownList drp2 = new DropDownList();
                TableCell tableCell = (TableCell)drpstaff.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                Grd_MappingStaff.SelectedIndex = row.RowIndex;
                drp1 = (DropDownList)Grd_MappingStaff.Rows[row.RowIndex].FindControl("drp_staff1");
                drp2 = (DropDownList)Grd_MappingStaff.Rows[row.RowIndex].FindControl("drp_staff2");

                if (int.Parse(drp1.SelectedValue.ToString()) == -1 && int.Parse(drp2.SelectedValue.ToString()) != -1)
                {
                    lbl_err.Text = "Select staff 2 only after selecting staff 1";
                }
                //else
                //{
                //    lbl_err.Text = "";
                //}
                if ((int.Parse(drp2.SelectedValue) != -1 && int.Parse(drp1.SelectedValue) != -1))
                {
                    if (int.Parse(drp2.SelectedValue) == int.Parse(drp1.SelectedValue))
                    {
                        lbl_err.Text = "You cannot add same staff as staff1 and staff2";
                    }
                }

            }

            protected void drp_staff1_SelectedIndexChanged(object sender, EventArgs e)
            {
                //DropDownList drpstaff = (DropDownList)sender;
                //DropDownList drp1 = new DropDownList();
                //DropDownList drp2 = new DropDownList();
                //TableCell tableCell = (TableCell)drpstaff.Parent;
                //GridViewRow row = (GridViewRow)tableCell.Parent;
                //Grd_MappingStaff.SelectedIndex = row.RowIndex;
                //drp1 = (DropDownList)Grd_MappingStaff.Rows[row.RowIndex].FindControl("drp_staff1");
                //drp2 = (DropDownList)Grd_MappingStaff.Rows[row.RowIndex].FindControl("drp_staff2");
                //if (int.Parse(drp1.SelectedValue.ToString()) != -1 && int.Parse(drp2.SelectedValue.ToString()) != -1)
                //{
                //    lbl_err.Text = "";
                //}              
            }

        #endregion

        #region Functions

            private void SaveDataToTable()
            {
                DropDownList Drpstaff1 = new DropDownList();
                DropDownList Drpstaff2 = new DropDownList();
                string _sql = "";
                string sql = "";
                //sql = "Delete From tblclasssubmap where  subjectId="+drp_subject.SelectedValue+"";
                //MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
               
                if (int.Parse(drp_subject.SelectedValue) != -1)
                {
                    foreach (GridViewRow gr in Grd_MappingStaff.Rows)
                    {
                        Drpstaff1 = (DropDownList)gr.FindControl("drp_staff1");
                        Drpstaff2 = (DropDownList)gr.FindControl("drp_staff2");
                        string staffname1 = Drpstaff1.SelectedItem.ToString();
                        string staffname2 = Drpstaff2.SelectedItem.ToString();
                        string subjectname = drp_subject.SelectedItem.ToString();
                        sql = "Delete From tblclassstaffmap where subjectId=" + drp_subject.SelectedValue + " and ClassId=" + gr.Cells[0].Text.ToString() + "";
                        MyStaffMang.m_MysqlDb.ExecuteQuery(sql);
                        //_sql = "Insert into tblclasssubmap(ClassId,SubjectId) values(" + gr.Cells[0].Text.ToString() + "," + drp_subject.SelectedValue + ")";
                        //MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);
                        _sql = "Insert into tblclassstaffmap(ClassId,SubjectId,StaffId) values(" + gr.Cells[0].Text.ToString() + "," + drp_subject.SelectedValue + "," + Drpstaff1.SelectedValue + ")";
                        MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);                       
                        _sql = "Insert into tblclassstaffmap(ClassId,SubjectId,StaffId) values(" + gr.Cells[0].Text.ToString() + "," + drp_subject.SelectedValue + "," + Drpstaff2.SelectedValue + ")";
                        MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);
                        if (int.Parse(Drpstaff1.SelectedValue) != -1 || int.Parse(Drpstaff2.SelectedValue) != -1)
                        {
                            if (int.Parse(Drpstaff1.SelectedValue) != -1 && int.Parse(Drpstaff2.SelectedValue) != -1)
                            {
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Assign staff to class", "Staff named " + staffname1 + ","+staffname2+" is assigned to class " + gr.Cells[1].Text.ToString() + " for the subject " + subjectname + "", 1);
                            }
                            else if (int.Parse(Drpstaff1.SelectedValue) != -1)
                            {
                                MyUser.m_DbLog.LogToDb(MyUser.UserName, "Assign staff to class", "Staff named " + staffname1 + " is assigned to class " + gr.Cells[1].Text.ToString() + " for the subject " + subjectname + "", 1);
                            }
                           
                        }                        
                    }


                }
            }
           
            private void FillGridView()
            {
                
                DataSet Classds = new DataSet();
                Grd_MappingStaff.Columns[0].Visible = true;
                string _sql = "select tblclass.ClassName, tblclass.Id as ClassId from tblclass inner join tblclasssubmap  on tblclass.Id= tblclasssubmap.ClassId where tblclasssubmap.SubjectId="+drp_subject.SelectedValue+" order by tblclass.ClassName";
                Classds = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                if (Classds != null && Classds.Tables[0].Rows.Count > 0)
                {
                    Grd_MappingStaff.DataSource = Classds;
                    Grd_MappingStaff.DataBind();
                    Grd_MappingStaff.Columns[0].Visible = false;
                    pnlmapstaff.Visible = true;

                }
                else
                {
                    Grd_MappingStaff.DataSource = null;
                    Grd_MappingStaff.DataBind();
                    pnlmapstaff.Visible = false;
                    lbl_err.Text = "No class found";
                }

                FillDropDown();

            }

            private void FillDropDown()
            {
                DataSet Staffds = new DataSet();
                DataSet ds = new DataSet();
                string _sql = "select tbluser.SurName, tblstaffdetails.UserId from tblstaffdetails inner join tbluser on tblstaffdetails.UserId= tbluser.Id inner join tblstaffsubjectmap on tblstaffdetails.UserId= tblstaffsubjectmap.StaffId where tbluser.Status=1 and tblstaffsubjectmap.SubjectId="+drp_subject.SelectedValue+" order by tbluser.SurName";
                Staffds = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                DropDownList drpstaff1 = new DropDownList();
                DropDownList drpstaff2 = new DropDownList();
                ListItem li;
                foreach (GridViewRow gr in Grd_MappingStaff.Rows)
                {
                    drpstaff1 = (DropDownList)gr.FindControl("drp_staff1");
                    drpstaff2 = (DropDownList)gr.FindControl("drp_staff2");
                    drpstaff1.Items.Add(new ListItem("Select staff", "-1"));
                    drpstaff2.Items.Add(new ListItem("Select staff", "-1"));
                    foreach (DataRow dr in Staffds.Tables[0].Rows)
                    {                       
                        li = new ListItem(dr["SurName"].ToString(), dr["UserId"].ToString());
                        drpstaff1.Items.Add(li);
                        li = new ListItem(dr["SurName"].ToString(), dr["UserId"].ToString());
                        drpstaff2.Items.Add(li);

                    }
                    _sql = "select staffid from tblclassstaffmap where classId=" + gr.Cells[0].Text.ToString() + " and SubjectId=" + drp_subject.SelectedValue + "";
                    ds = MyStaffMang.m_MysqlDb.ExecuteQueryReturnDataSet(_sql);
                    if (ds == null)
                        continue;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        drpstaff1.SelectedValue  = ds.Tables[0].Rows[0]["staffid"].ToString();                      

                    }

                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        drpstaff2.SelectedValue = ds.Tables[0].Rows[1]["staffid"].ToString();
                    }                  
                    
                }
                    
            }

            private void LoadSubject()
                {
                    string _sql = "select tblsubjects.Id, tblsubjects.subject_name from tblsubjects order by tblsubjects.subject_name";
                    Myreader = MyStaffMang.m_MysqlDb.ExecuteQuery(_sql);
                    if (Myreader.HasRows)
                    {
                        drp_subject.Items.Add(new ListItem("select subject", "0"));
                        while (Myreader.Read())
                        {                           
                            ListItem li = new ListItem(Myreader.GetValue(1).ToString(), Myreader.GetValue(0).ToString());
                            drp_subject.Items.Add(li);

                        }
                    }
                    else
                    {

                        drp_subject.Items.Add(new ListItem("No subject Found", "-1"));
                    }
                }

        #endregion
    }
}