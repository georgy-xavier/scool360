using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;

namespace WinEr
{
    public partial class SMSLogs : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private KnowinUser MyUser;
        private SMSManager MysmsMang;
        private GroupManager MyGroup;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyGroup = MyUser.GetGroupManagerObj();
            MysmsMang = MyUser.GetSMSMngObj();

            if (MyGroup == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            else if (MysmsMang == null)
            {
                Response.Redirect("RoleErr.htm");
            }            
            else
            {
                if (!IsPostBack)
                {
                    LoadDate();
                }
            }
        }

        private void LoadDate()
        {
            Txt_To.Text = System.DateTime.Now.Date.Day + "/" + System.DateTime.Now.Date.Month + "/" + System.DateTime.Now.Date.Year;
            Txt_from.Text = "1/" +System.DateTime.Now.Date.Month+"/"+System.DateTime.Now.Date.Year;
        }

        protected void Grd_SMSList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grd_SMSList.PageIndex = e.NewPageIndex;
            LoadValues();  
        }

        protected void Btn_show_Click(object sender, EventArgs e)
        {
            LoadValues();                        
        }

        private void LoadValues()
        {
            DateTime dt_from = General.GetDateTimeFromText(Txt_from.Text.ToString());
            DateTime dt_to = General.GetDateTimeFromText(Txt_To.Text.ToString());
            
            int type = int.Parse(Rdb_CheckType.SelectedValue);
            
            string sql = "";
            DataSet dt = null;
            Grd_SMSList.Columns[0].Visible = true;
            Grd_SMSList.Columns[1].Visible = true;
            Grd_SMSList.Columns[2].Visible = true;
           
            if (type == 0)
            {
 
                                       
                sql = "select tbluser.SurName as StaffName, tblautosms.Message, date_Format( tblautosms.SentDate, '%d/%m/%Y')as Date , Null as ParentName,Null as StudentName from tblautosms inner join tblstaffdetails on tblstaffdetails.PhoneNumber = tblautosms.PhoneNumber  inner join tbluser on tbluser.id= tblstaffdetails.UserId where tblautosms.UserType=0 and tblautosms.Status=1";
                sql = sql + " and  tblautosms.SentDate between Date('" + dt_from.ToString("s") + "') and Date('" + dt_to.ToString("s") + "')";
                dt = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                {
                    Grd_SMSList.DataSource = dt;
                    Grd_SMSList.DataBind();

                }
                else
                {
                    Grd_SMSList.DataSource = null;
                    Grd_SMSList.DataBind();
                }

                Grd_SMSList.Columns[0].Visible = true;
                Grd_SMSList.Columns[1].Visible = false;
                Grd_SMSList.Columns[2].Visible = false;
            }
            else if (type == 1) 
            {
                sql = @"select null as StaffName,  tblstudent.GardianName  as ParentName, tblstudent.studentName  as StudentName, tblautosms.Message, date_Format( tblautosms.SentDate, '%d/%m/%Y') as Date
                        from tblautosms inner join tblstudent on ( tblstudent.ResidencePhNo = tblautosms.PhoneNumber or tblstudent.OfficePhNo = tblautosms.PhoneNumber)
                        where tblautosms.UserType=1 and tblautosms.Status=1 ";
                sql = sql + " and  tblautosms.SentDate between Date('" + dt_from.ToString("s") + "') and Date('" + dt_to.ToString("s") + "')";

                dt = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                {
                    Grd_SMSList.DataSource = dt;
                    Grd_SMSList.DataBind();
                }
                else
                {
                    Grd_SMSList.DataSource = null;
                    Grd_SMSList.DataBind();
                }

                Grd_SMSList.Columns[0].Visible = false;
                Grd_SMSList.Columns[1].Visible = true;
                Grd_SMSList.Columns[2].Visible = true;
            }
            else if (type == 2) 
            {
                sql = @"select null as StaffName,  tblstudent.GardianName  as ParentName, tblstudent.studentName as StudentName, tblautosms.Message, date_Format( tblautosms.SentDate, '%d/%m/%Y') as Date
                    from tblautosms inner join tblstudent on ( tblstudent.ResidencePhNo = tblautosms.PhoneNumber or tblstudent.OfficePhNo = tblautosms.PhoneNumber)
                    where tblautosms.UserType=2 and tblautosms.Status=1";
                sql = sql + " and  tblautosms.SentDate between Date('" + dt_from.ToString("s") + "') and Date('" + dt_to.ToString("s") + "')";
               
                dt = MysmsMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);

                if (dt != null && dt.Tables != null && dt.Tables[0].Rows.Count > 0)
                {
                    Grd_SMSList.DataSource = dt;
                    Grd_SMSList.DataBind();
                }
                else
                {
                    Grd_SMSList.DataSource = null;
                    Grd_SMSList.DataBind();
                }
                Grd_SMSList.Columns[0].Visible = false;
                Grd_SMSList.Columns[1].Visible = false;
                Grd_SMSList.Columns[2].Visible = true;
            }

          

          
        }
    }
}
