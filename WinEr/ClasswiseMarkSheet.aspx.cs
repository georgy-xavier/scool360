using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

namespace WinEr
{
    public partial class ClasswiseMarkSheet : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MydataSet;
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
            else if (!MyUser.HaveActionRignt(831))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {
                    AddClassToDropDownClass();
                    //some initlization
                }
            }
        }


        private void AddClassToDropDownClass()
        {
            DropDownClass.Items.Clear();
            MydataSet = MyUser.MyAssociatedClass();
            if (MydataSet != null && MydataSet.Tables != null && MydataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in MydataSet.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                    DropDownClass.Items.Add(li);
                }
            }
            else
            {
                ListItem li = new ListItem("No Class Present", "-1");
                DropDownClass.Items.Add(li);
            }
            DropDownClass.SelectedIndex = 0;
          
        }



        protected void Btn_Generate_Click(object sender, EventArgs e)
        {
            if (DropDownClass.SelectedValue != "-1")
            {
                string _ExcelStr = GetMarkSheetExcel();
                if (_ExcelStr != "")
                {

                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment; filename=MarkEntrySheet.xls");
                    Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                    Response.Write("<head>");
                    Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                    Response.Write("<!--[if gte mso 9]><xml>");
                    Response.Write("<x:ExcelWorkbook>");
                    Response.Write("<x:ExcelWorksheets>");
                    Response.Write("<x:ExcelWorksheet>");
                    Response.Write("<x:Name>Year Attendance Report</x:Name>");
                    Response.Write("<x:WorksheetOptions>");
                    Response.Write("<x:Print>");
                    Response.Write("<x:ValidPrinterInfo/>");
                    Response.Write("</x:Print>");
                    Response.Write("</x:WorksheetOptions>");
                    Response.Write("</x:ExcelWorksheet>");
                    Response.Write("</x:ExcelWorksheets>");
                    Response.Write("</x:ExcelWorkbook>");
                    Response.Write("</xml>");
                    Response.Write("<![endif]--> ");
                    Response.Write(_ExcelStr);
                    Response.Write("</head>");
                    Response.Flush();
                    Response.End();

                }
                else
                {
                    lbl_msg.Text = "No student in selected class";
                }
            }
            else
            {
                lbl_msg.Text = "Please select class";
            }
        }

        private string GetMarkSheetExcel()
        {
            string TableSTR = "";

            string _studentlistSTR = "";


            string sql = "SELECT tblstudentclassmap.RollNo,tblstudent.StudentName from tblstudent INNER JOIN tblstudentclassmap on tblstudentclassmap.StudentId=tblstudent.Id WHERE tblstudent.Status=1 AND tblstudentclassmap.BatchId=" + MyUser.CurrentBatchId + " AND tblstudentclassmap.ClassId=" + DropDownClass.SelectedValue + " Order by tblstudentclassmap.RollNo ASC";
            MyReader = MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            if (MyReader.HasRows)
            {
                while (MyReader.Read())
                {
                    _studentlistSTR = _studentlistSTR + "<tr valign=\"middle\">  <td align=\"center\" style=\"height:20px;border:solid 1px gray\">  " + MyReader.GetValue(0).ToString() + " </td> <td style=\"height:20px;border:solid 1px gray\"> &nbsp;&nbsp; " + MyReader.GetValue(1).ToString() + "  </td>  <td  align=\"center\"  style=\"height:20px;border:solid 1px gray\"> </td> </tr> ";
                }
            }

            if (_studentlistSTR != "")
            {
                TableSTR = "<table width=\"800px\" cellspacing=\"0\"> <tr> <td align=\"center\" valign=\"middle\" style=\"border:solid 2px gray;height:40px;font-size:20px;font-weight:bolder;color:Black\">  " + MyUser.SchoolName + "  </td> </tr> <tr> <td style=\"border:solid 2px gray;\">  <table width=\"100%\" cellspacing=\"0\">  <tr> <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">   Class : <b>" + DropDownClass.SelectedItem.Text + " </b> </td>  <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">    Exam : <b>___________</b>  </td>  <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">  Date : <b>___________</b> </td>  </tr> <tr>  <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">  Subject : <b>___________</b>  </td>  <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">  Teacher : <b>___________</b>  </td>  <td  align=\"center\" valign=\"middle\" style=\"width:33%;border:solid 1px gray;height:30px\">   Max Mark : <b>___________</b> </td> </tr>  </table> </td></tr>  <tr>  <td style=\"border:solid 2px gray;\">  <table width=\"100%\" cellspacing=\"0\">  <tr>  <td   align=\"center\" valign=\"middle\"  style=\"height:30px;width:10%;background-color:Gray;color:White;font-weight:bolder\">  ROLL NO </td> <td valign=\"middle\"  style=\"height:30px;width:65%;background-color:Gray;color:White;font-weight:bolder\">   &nbsp;&nbsp;&nbsp; STUDENT NAME</td> <td   align=\"center\" valign=\"middle\"  style=\"height:30px;width:25%;background-color:Gray;color:White;font-weight:bolder\">  MARKS  </td>  </tr> " + _studentlistSTR + " <tr valign=\"middle\">           <td align=\"center\" style=\"height:25px;border:solid 1px gray\">                        </td>            <td align=\"right\" style=\"height:25px;border:solid 1px gray;color:Black;font-weight:bolder\">                 GRAND TOTAL &nbsp;&nbsp;           </td>            <td  align=\"center\"  style=\"height:25px;border:solid 1px gray\">                      </td>          </tr>         </table>               </td>     </tr>     <tr><td align=\"right\" valign=\"bottom\" style=\"border:solid 2px gray;height:50px;\">          Signature&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></table>";
            }
            return TableSTR;
        }
    }
}
