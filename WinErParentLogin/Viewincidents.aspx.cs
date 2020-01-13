using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using WinBase;
using System.Text;
using System.Drawing;
namespace WinErParentLogin
{
    public partial class Viewincidents : System.Web.UI.Page
    {

        private ParentInfoClass MyParentInfo;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MyParentObj"] == null)
            {
                Response.Redirect("sectionerr.htm");

            }
            MyParentInfo = (ParentInfoClass)Session["MyParentObj"];
            if (MyParentInfo == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            if (!IsPostBack)
            {
                loadincidents();
              
                setDrpIndex();
        
                LoadPreviousBatchesToDropDown();
                Drp_PreviousBatch.Enabled = false;
                Drp_PreviousBatch.SelectedValue = MyParentInfo.CurrentBatchId.ToString();
                Label MyHeader = (Label)this.Master.FindControl("Lbl_PageHeader");
                MyHeader.Text = "View Incidents";
            }
        }

       
        private void LoadPreviousBatchesToDropDown()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
            Drp_PreviousBatch.Items.Clear();
            string sql = "select tblbatch.BatchName, tblbatch.Id from tblbatch where tblbatch.Id BETWEEN (select tblview_student.JoinBatch from tblview_student where tblview_student.Id=" + MyParentInfo.StudentId + ") and " + MyParentInfo.CurrentBatchId + " order by tblbatch.Id desc";
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    ListItem li = new ListItem(MyReader.GetValue(0).ToString(), MyReader.GetValue(1).ToString());
                    Drp_PreviousBatch.Items.Add(li);
                }
            }

            ReleaseResourse(_mysqlObj, MyIncident);
        }

        private void ReleaseResourse(MysqlClass _mysqlObj, Incident MyIncident)
        {
            _mysqlObj.CloseConnection();
            _mysqlObj = null;
            MyIncident = null;
        }



      

        private void AddDetailsTopopUp(int IncidentId)
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
          

            DateTime _IncedentDate;
            DateTime _CreatedDate;
            string sql = "";
            sql = "select tblincedenttype.`Type`, tblview_user.SurName , tblview_incident.UserType, tblview_incident.CreatedDate , tblview_incident.IncedentDate , tblview_incident.Title , tblview_incident.Description , tblview_incident.AssoUser,tblclass.ClassName from tblview_incident inner join tblclass on tblclass.Id=tblview_incident.ClassId  inner join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tblview_user on tblview_user.Id = tblview_incident.CreatedUserId where tblview_incident.Id = " + IncidentId + " and   tblincedenttype.IncidentType='NORMAL'";

       
            MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                Lbl_Class.Visible = true;
                Txt_Class.Visible = true;
                Txt_Type.Text = MyReader.GetValue(0).ToString();
                Txt_CreatedUser.Text = MyReader.GetValue(1).ToString();
                Txt_UserType.Text = MyReader.GetValue(2).ToString();
                _IncedentDate = DateTime.Parse(MyReader.GetValue(4).ToString());
                //_IncedentDate =MyUser.GetDareFromText(MyReader.GetValue(4).ToString());

                Txt_IncidentDate.Text = _IncedentDate.ToString("dd/MM/yyyy");
                _CreatedDate = DateTime.Parse(MyReader.GetValue(3).ToString());
                //_CreatedDate = MyUser.GetDareFromText(MyReader.GetValue(3).ToString());

                Txt_CreatedDate.Text = _CreatedDate.ToString("dd/MM/yyyy");
                Txt_Title.Text = MyReader.GetValue(5).ToString();
                Txt_Desc.Text = MyReader.GetValue(6).ToString();
                Txt_UserId.Text = MyReader.GetValue(7).ToString();
                Txt_Class.Text = MyReader.GetValue(8).ToString();
                Txt_IncidentId.Text = IncidentId.ToString();
            }
            if ((Txt_UserType.Text.Trim() == "student") && (Txt_UserId.Text.Trim() != ""))
            {
               
                sql = "select tblview_student.StudentName from tblview_student WHERE tblview_student.Id = " + int.Parse(Txt_UserId.Text.Trim()) ;
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
            else if ((Txt_UserType.Text.Trim().ToLowerInvariant() == "staff") && ((Txt_UserId.Text.Trim() != "")))
            {
                Lbl_Class.Visible = false;
                Txt_Class.Visible = false;
                sql = "select tblview_user.SurName from  tblview_user WHERE tblview_user.Id = " + int.Parse(Txt_UserId.Text.Trim()) + "";
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(1).ToString();
                }
            }
            else if ((Txt_UserType.Text.Trim() == "Class") && ((Txt_UserId.Text.Trim() != "")))
            {
                Lbl_Class.Visible = false;
                Txt_Class.Visible = false;
                sql = " select tblclass.ClassName from tblclass inner join tblview_incident on tblview_incident.AssoUser=tblclass.Id WHERE tblview_incident.Id=" + IncidentId + "";
  
                MyReader = MyIncident.m_MysqlDb.ExecuteQuery(sql);
                if (MyReader.HasRows)
                {
                    Txt_ReportedTo.Text = MyReader.GetValue(0).ToString();
                    //Txt_Class.Text = MyReader.GetValue(0).ToString();
                }
            }
            ReleaseResourse(_mysqlObj, MyIncident);
        }

      
      
        private void setDrpIndex()
        {
            string TypeId = "";

            try
            {
                TypeId = Request.QueryString["id"].ToString();
            }
            catch
            {

            }
            if (TypeId != "")
            {
                int Index = int.Parse(TypeId);
                
            }
        }

 

  

        

        protected void Rdb_Batch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Rdb_Batch.SelectedValue == "0" || Rdb_Batch.SelectedValue == "2")
            {
                Drp_PreviousBatch.SelectedValue = MyParentInfo.CurrentBatchId.ToString();
                Drp_PreviousBatch.Enabled = false;
            }
            else
            {
                Drp_PreviousBatch.Enabled = true;
            }
          
            loadincidents();
        }

        protected void Drp_PreviousBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            loadincidents();
        }



   



        ////////////////////////////////////////////////
        private void loadincidents()
        {
            MysqlClass _mysqlObj = new MysqlClass(WinerUtlity.GetConnectionString(MyParentInfo.SchoolObject));
            Incident MyIncident = new Incident(_mysqlObj);
            string sql = "";
            DataSet MyDataSet = new DataSet();
            StringBuilder academic = new StringBuilder();
            StringBuilder medical = new StringBuilder();
            StringBuilder Disciplinary = new StringBuilder();
            StringBuilder other = new StringBuilder();
            lbl_TotalPoints.ForeColor = Color.FromName("#ddb104");
            Img_Points.ImageUrl = "Pics/Points.png";
            lbl_TotalPoints.Text = "0";
            academicarea.InnerHtml = academic.ToString();
            medicalarea.InnerHtml = medical.ToString();
            displinaryarea.InnerHtml = Disciplinary.ToString();
            otherarea.InnerHtml = other.ToString();
            // academic area
            //select tblincedenttype.`Type`, tblview_user.SurName , tblview_incident.UserType, tblview_incident.CreatedDate , tblview_incident.IncedentDate , tblview_incident.Title , tblview_incident.Description , tblview_incident.AssoUser,tblclass.ClassName from tblview_incident inner join tblclass on tblclass.Id=tblview_incident.ClassId  inner join tblincedenttype on tblincedenttype.Id = tblview_incident.TypeId inner join tblview_user on tblview_user.Id = tblview_incident.CreatedUserId where tblview_incident.Id = "
            sql = @"  select tblincedenttype.`Type` as instype, tblview_user.SurName ,
                tblview_incident.UserType, tblview_incident.CreatedDate , tblview_incident.IncedentDate , tblview_incident.Title , 
                tblview_incident.Description , tblview_incident.AssoUser,tblclass.ClassName,tblview_incident.`Point` from tblview_incident 
                inner join tblclass on      tblclass.Id=tblview_incident.ClassId  inner join tblincedenttype on 
                tblincedenttype.Id = tblview_incident.TypeId inner join  tblview_user on tblview_user.Id = tblview_incident.CreatedUserId where 
                 tblincedenttype.IncidentType='NORMAL' and  tblview_incident.AssoUser = " + MyParentInfo.StudentId;

            if (Rdb_Batch.SelectedValue == "0")
            {
                sql = sql + " and tblview_incident.BatchId=" + MyParentInfo.CurrentBatchId;
            }
            else if (Rdb_Batch.SelectedValue == "1")
            {
                sql = sql + " and tblview_incident.BatchId=" + Drp_PreviousBatch.SelectedValue;
            }


            sql = sql + "   order by tblview_incident.CreatedDate desc";

            MyDataSet = MyIncident.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            double point = 0;
            if (MyDataSet != null && MyDataSet.Tables != null && MyDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MyDataSet.Tables[0].Rows)
                {   
                    
                    if( string.Compare(dr["instype"].ToString(),"Academic")==0)
                    {
                        academic.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                        academic.Append("Point :" + dr["Point"].ToString() + "<br /> ");
                        academic.Append(dr["Description"].ToString() + "<br />");
                        academic.Append("Date :" + dr["IncedentDate"].ToString() + "<br /> ");
                        academic.Append("Created By:" + dr["SurName"].ToString() + "<br /> <hr><br /> ");
                    }
                    else if( string.Compare(dr["instype"].ToString(), "Medical")==0)
                    {

                        medical.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                        medical.Append("Point :" + dr["Point"].ToString() + "<br /> ");
                        medical.Append(dr["Description"].ToString() + "<br />");
                        medical.Append("Date :" + dr["IncedentDate"].ToString() + "<br /> ");
                        medical.Append("Created By:" + dr["SurName"].ToString() + "<br /> <hr><br /> ");


                    }
                    else if ( string.Compare(dr["instype"].ToString(),"Disciplinary")==0)
                    {


                        Disciplinary.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                        Disciplinary.Append("Point :" + dr["Point"].ToString() + "<br /> ");
                        Disciplinary.Append(dr["Description"].ToString() + "<br />");
                        Disciplinary.Append("Date :" + dr["IncedentDate"].ToString() + "<br /> ");
                        Disciplinary.Append("Created By:" + dr["SurName"].ToString() + "<br /> <hr><br /> ");


                    }

                    else if ( string.Compare(dr["instype"].ToString(),"Others")==0)
                    {
                        
                        other.Append("<b>" + dr["Title"].ToString() + "</b><br />");
                        other.Append("Point :" + dr["Point"].ToString() + "<br /> ");
                        other.Append(dr["Description"].ToString() + "<br />");
                        other.Append("Date :" + dr["IncedentDate"].ToString() + "<br /> ");
                        other.Append("Created By:" + dr["SurName"].ToString() + "<br /> <hr><br /> ");


                    }
                    point = point + double.Parse(dr["Point"].ToString());
                }

                academicarea.InnerHtml=academic.ToString();
                medicalarea.InnerHtml=medical.ToString();
                displinaryarea.InnerHtml=Disciplinary.ToString();
                otherarea.InnerHtml = other.ToString();
                lbl_TotalPoints.Text = point.ToString();

                if (point < 0)
                {
                    lbl_TotalPoints.ForeColor = Color.Red;
                    Img_Points.ImageUrl = "Pics/Points red.png";
                }
               

            }

        }
    }
}



