using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using System.Xml;
using System.Text;

namespace WinEr
{
    public partial class RegisteredStudentDetails1 : System.Web.UI.Page
    {
        private StudentManagerClass MyStudMang;
        private FeeManage MyFeeMang;
        private KnowinUser MyUser;
        private OdbcDataReader MyReader = null;
        private DataSet MyDataSet = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            MyFeeMang = MyUser.GetFeeObj();
            if (MyStudMang == null)
            {
                Response.Redirect("RoleErr.htm");
                //no rights for this user.
            }
            if (Session["StudId"] == null)
            {
                Response.Redirect("ViewRegisteredStudents.aspx");
            }
            if (!MyUser.HaveActionRignt(606))
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {


                if (!IsPostBack)
                {

                    bool Tempvalid = false;
                    string TempStudId = "";
                    if (Request.QueryString["TempStudId"] != null)
                    {
                      TempStudId=  Request.QueryString["TempStudId"].ToString();
                        if (TempStudId !="")
                        {
                            Tempvalid = true;
                        }
                        if (Tempvalid)
                        {
                            if (Request.QueryString["ClassId"].ToString() != "")
                            {
                                int ClassId = int.Parse(Request.QueryString["ClassId"].ToString());
                                if (ClassId == 0)
                                {
                                    Session["ClassId"] = "";
                                }
                                else
                                {
                                    Session["ClassId"] = ClassId;
                                }
                            }
                            LoadAllDetails(TempStudId);
                            LoadCoustomFields();
                        }
                    }
                }
            }
        }

        private void LoadAllDetails(string _StudId)
        {
            string Innersql = "";
            string ClassName = "";
            if (Session["ClassId"].ToString() != "")
            {
                int ClassId = int.Parse(Session["ClassId"].ToString());
                Innersql = " INNER JOIN tblclass ON tblclass.Id=tbltempstdent.Class ";
                ClassName = "tblclass.ClassName,";

            }
            string sql = "SELECT tbltempstdent.Name,tbltempstdent.TempId,tbltempstdent.Gender,tblstandard.Name," + ClassName + "tblbatch.BatchName,tbltempstdent.Fathername,tbltempstdent.Address,tbltempstdent.PhoneNumber,tbltempstdent.Rank,tbltempstdent.CreatedUserName,MotherName,Location,Pin,State,Nationality,BloodGroup,MotherTongue,FatherEduQualification,MotherEduQualification,FatherOccupation,MotherOccupation,AnnualIncome,Remark,PersionalInterview,DateOfInterView,TeacherRemark,HMRemark,PrincipalRemark,Result,PreviousBoard,Email,Date_Format(DOB,'%d/%m/%Y') as DOB  FROM tbltempstdent " + Innersql + " INNER JOIN tblstandard ON tblstandard.Id=tbltempstdent.Standard INNER JOIN tblbatch ON tblbatch.Id=tbltempstdent.JoiningBatch WHERE tbltempstdent.TempId='" + _StudId + "'";
            MyReader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            
            if (MyReader.HasRows)
            {
                StringBuilder Sb = new StringBuilder();
                if (Session["ClassId"].ToString() != "")
                {
                    lbl_FullName.Text = MyReader.GetValue(0).ToString();
                    Lbl_RegisterId.Text = MyReader.GetValue(1).ToString();
                    lbl_Sex.Text = MyReader.GetValue(2).ToString();
                    lbl_standard.Text = MyReader.GetValue(3).ToString();
                    lbl_Class.Text = MyReader.GetValue(4).ToString();
                    Lbl_AcademicYear.Text = MyReader.GetValue(5).ToString();
                    lbl_Father.Text = MyReader.GetValue(6).ToString();
                    lblAdress.InnerHtml = MyReader.GetValue(7).ToString();
                    lbl_phone.Text = MyReader.GetValue(8).ToString();
                    lbl_Rank.Text = MyReader.GetValue(9).ToString();
                    Lbl_CreatedBy.Text = MyReader.GetValue(10).ToString();
                    
                    Sb.Append("<div><table width=\"600px\">");
                    if(MyReader["Email"].ToString()=="")
                        Sb.Append("<tr><td class=\"leftside\" style=\"width:50%\">Email</td><td class=\"rightside\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
                    else
                    Sb.Append("<tr><td class=\"leftside\">Email</td><td class=\"rightside\">" + MyReader["Email"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">DOB</td><td class=\"rightside\">" + MyReader["DOB"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Name</td><td class=\"rightside\">" + MyReader["MotherName"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Pin</td><td class=\"rightside\">" + MyReader["Pin"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">State</td><td class=\"rightside\">" + MyReader["State"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Nationality</td><td class=\"rightside\">" + MyReader["Nationality"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Father Edu Qualification</td><td class=\"rightside\">" + MyReader["FatherEduQualification"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Edu Qualification</td><td class=\"rightside\">" + MyReader["MotherEduQualification"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Father Occupation</td><td class=\"rightside\">" + MyReader["FatherOccupation"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Occupation</td><td class=\"rightside\">" + MyReader["MotherOccupation"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Annual Income</td><td class=\"rightside\">" + MyReader["AnnualIncome"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Remark</td><td class=\"rightside\">" + MyReader["Remark"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Persional Interview</td><td class=\"rightside\">" + MyReader["PersionalInterview"] + "</td></tr>");
                   
                    if (MyReader["PersionalInterview"].ToString() == "Attended")
                    {

                        Sb.Append("<tr><td class=\"leftside\">Date of Interview</td><td class=\"rightside\">" + MyReader["DateOfInterView"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">Teacher Remark</td><td class=\"rightside\">" + MyReader["TeacherRemark"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">HM Remark</td><td class=\"rightside\">" + MyReader["HMRemark"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">Principal Remark</td><td class=\"rightside\">" + MyReader["PrincipalRemark"] + "</td></tr>");
                    }
                    if (MyReader["PreviousBoard"].ToString() != "")
                    {
                        Sb.Append("<tr><td class=\"leftside\">Previous Board</td><td class=\"rightside\">" + MyReader["PreviousBoard"] + "</td></tr>");
                    }

                    Sb.Append("</table></div>");
                }
                else
                {
                    lbl_FullName.Text = MyReader.GetValue(0).ToString();
                    Lbl_RegisterId.Text = MyReader.GetValue(1).ToString();
                    lbl_Sex.Text = MyReader.GetValue(2).ToString();
                    lbl_standard.Text = MyReader.GetValue(3).ToString();
                    lbl_Class.Text = "Not Assigned";
                    Lbl_AcademicYear.Text = MyReader.GetValue(4).ToString();
                    lbl_Father.Text = MyReader.GetValue(5).ToString();
                    lblAdress.InnerHtml = MyReader.GetValue(6).ToString();
                    lbl_phone.Text = MyReader.GetValue(7).ToString();
                    lbl_Rank.Text = MyReader.GetValue(8).ToString();
                    Lbl_CreatedBy.Text = MyReader.GetValue(9).ToString();
                    
                    Sb.Append("<div><table width=\"100%\">");
                    if (MyReader["Email"].ToString() == "")
                        Sb.Append("<tr><td class=\"leftside\">Email</td><td class=\"rightside\">&dnsp;&dnsp;&dnsp;&dnsp;&dnsp;&dnsp;</td></tr>");
                    else
                        Sb.Append("<tr><td class=\"leftside\">Email</td><td class=\"rightside\">" + MyReader["Email"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">DOB</td><td class=\"rightside\">" + MyReader["DOB"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Name</td><td class=\"rightside\">" + MyReader["MotherName"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Pin</td><td class=\"rightside\">" + MyReader["Pin"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">State</td><td class=\"rightside\">" + MyReader["State"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Nationality</td><td class=\"rightside\">" + MyReader["Nationality"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Father Edu Qualification</td><td class=\"rightside\">" + MyReader["FatherEduQualification"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Edu Qualification</td><td class=\"rightside\">" + MyReader["MotherEduQualification"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Father Occupation</td><td class=\"rightside\">" + MyReader["FatherOccupation"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Mother Occupation</td><td class=\"rightside\">" + MyReader["MotherOccupation"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Annual Income</td><td class=\"rightside\">" + MyReader["AnnualIncome"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Remark</td><td class=\"rightside\">" + MyReader["Remark"] + "</td></tr>");
                    Sb.Append("<tr><td class=\"leftside\">Persional Interview</td><td class=\"rightside\">" + MyReader["PersionalInterview"] + "</td></tr>");

                    if (MyReader["PersionalInterview"].ToString() == "Attended")
                    {

                        Sb.Append("<tr><td class=\"leftside\">Date of Interview</td><td class=\"rightside\">" + MyReader["DateOfInterView"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">Teacher Remark</td><td class=\"rightside\">" + MyReader["TeacherRemark"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">HM Remark</td><td class=\"rightside\">" + MyReader["HMRemark"] + "</td></tr>");
                        Sb.Append("<tr><td class=\"leftside\">Principal Remark</td><td class=\"rightside\">" + MyReader["PrincipalRemark"] + "</td></tr>");
                    }
                    if (MyReader["PreviousBoard"].ToString() != "")
                    {
                        Sb.Append("<tr><td class=\"leftside\">Previous Board</td><td class=\"rightside\">" + MyReader["PreviousBoard"] + "</td></tr>");
                    }

                    Sb.Append("</table></div>");
                }
                DetailsView.InnerHtml = Sb.ToString();
            }
        }
        private void LoadCoustomFields()
        {
            int CustfieldCount = MyStudMang.CoustomFieldCount;
            if (CustfieldCount == 0)
            {
                Pnl_custumarea.Visible = false;
            }
            else
            {

                DataSet _CustomFields = MyStudMang.GetCuestomFields();
                if (_CustomFields != null && _CustomFields.Tables != null && _CustomFields.Tables[0].Rows.Count > 0)
                {

                    int i = 0;
                    Table tbl = new Table();

                    myPlaceHolder.Controls.Add(tbl);
                    tbl.CssClass = "tablelist";

                    foreach (DataRow dr_fieldData in _CustomFields.Tables[0].Rows)
                    {

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();

                        tc1.Text = dr_fieldData[1].ToString() + ":";
                        tc1.CssClass = "leftside";

                        Label Lblcostom = new Label();
                        Lblcostom.Text = MyStudMang.GetCustomFieldRegStdnt(dr_fieldData[0].ToString(), Session["StudId"].ToString());
                        Lblcostom.ForeColor = System.Drawing.Color.Black;
                        Lblcostom.Font.Bold = true;
                        Lblcostom.ID = "myLbl" + i.ToString();
                        tc2.Controls.Add(Lblcostom);
                        tc2.CssClass = "rightside";

                        tr.Cells.Add(tc1);
                        tr.Cells.Add(tc2);

                        tbl.Rows.Add(tr);
                        i++;
                    }
                }

            }
        }

        protected void Lnk_ViewRegform_Click(object sender, EventArgs e)
        {
            string TempId = Lbl_RegisterId.Text; OdbcDataReader Xmlreader = null;
            string sql = "";
            Session["tempId"] = TempId;
            sql = "select tbl_xmlstring.Id, tbl_xmlstring.XMLString from tbl_xmlstring  where tbl_xmlstring.TempId='" + TempId + "'";
            Xmlreader = MyStudMang.m_MysqlDb.ExecuteQuery(sql);
            if (Xmlreader.HasRows)
            {
                XmlDocument doc = new XmlDocument();
                string StrXml = Xmlreader.GetValue(1).ToString();
                DataSet ReaderDs = new DataSet();

                doc.LoadXml(StrXml);
                XmlNodeReader Nodereader = new XmlNodeReader(doc);
                Nodereader.MoveToContent();
                while (Nodereader.Read())
                {
                    ReaderDs.ReadXml(Nodereader);

                }
                Session["ViewDs"] = ReaderDs;

                Response.Redirect("RegistrationForm.aspx?TempId=" + TempId + "");
                //ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScriptNameYouLike", "openIncpopup('RegistrationForm.aspx?TempId=" + TempId + "');", true);

            }
        }
    }
}
