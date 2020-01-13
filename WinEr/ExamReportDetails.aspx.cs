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

namespace WinEr
{
    public partial class WebForm21 : System.Web.UI.Page
    {
        private WinErSearch MySearchMang;
        private KnowinUser MyUser;        
        //private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }            

            MyUser = (KnowinUser)Session["UserObj"];
            MySearchMang = MyUser.GetSearchObj();
           
            if (MySearchMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
          
            else if (!MyUser.HaveActionRignt(49))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    if ((Request.QueryString["BatchId"].ToString() != "") && (Request.QueryString["ExamId"].ToString() != ""))
                    {
                        try
                        {                           
                            int BatchId = int.Parse(Request.QueryString["BatchId"]);
                            int ExamId = int.Parse(Request.QueryString["ExamId"]);
                            if (BatchId !=-1)
                            {
                                LoadStudDetails(ExamId, BatchId);
                                //some initialisations
                            }
                        }
                        catch
                        {

                        }
                    }             
                }
            }
        }    
        
        private void LoadStudDetails(int ExamId,int BatchId)
        {
            if (MySearchMang.ExamSchduled(ExamId,BatchId))
            {
                string sql = "select stud.Id,stud.StudentName,map.RollNo,mark.TotalMark,mark.TotalMax,mark.Avg,mark.Grade,mark.Result,mark.Rank,mark.Remark FROM tblstudent stud inner join tblstudentclassmap map on map.StudentId=stud.Id inner join tblstudentmark mark on mark.StudId=stud.Id inner join tblexamschedule sexam on sexam.Id=mark.ExamSchId where map.BatchId=" + BatchId + "  and  sexam.ExamId=" + ExamId + " and sexam.BatchId=" + BatchId + " and sexam.status='Completed' order by map.RollNo ASC";
                MydataSet = MySearchMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                if (MydataSet.Tables[0].Rows.Count > 0)
                {
                    Grd_CreateReport.Columns[0].Visible = true;
                    Grd_CreateReport.DataSource = MydataSet;
                    Grd_CreateReport.DataBind();
                    Grd_CreateReport.Columns[0].Visible = false;
                }
            }
            else
            {
                Lbl_msg.Text = "No details are present";
                this.MPE_MessageBox.Show();
            }
        }

        protected void Grd_CreateReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = e.Row.Cells[2].Text;
                if (value == "-1")
                {
                    e.Row.Cells[2].Text = "No Roll No.";
                }
            }
        }
    }
}