using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WinBase;
using System.Data.Odbc;
using System.Text;

namespace WinEr
{
    public partial class CommentPannel : System.Web.UI.Page
    {
        private KnowinUser MyUser;
        private StudentManagerClass MyStudMang;

        int CommentThredId=-1;
     
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (Session["UserObj"] == null)
            {
                Response.Redirect("sectionerr.htm");
            }
            MyUser = (KnowinUser)Session["UserObj"];
            MyStudMang = MyUser.GetStudentObj();
            if (MyUser==null)
            {
                Response.Redirect("RoleErr.htm");
            }
            else
            {
                if (!IsPostBack)
                {


                    CommentManagementClass _CommentObj = new CommentManagementClass(MyStudMang.m_MysqlDb);
                    //int _CommentthreadId = _CommentObj.AddComment("Fee editing", _ApprovelRequestObj.REASON, _ApprovelRequestObj.CREATEDUSER);
                    Pnl_Details.Visible = false;

                    Pnl_Err.Visible = false;
                    Lbl_Err.Text = "";
                    Lbl_Msg.Text = "";
                    string Type = "";
                    int userId = 0;
                    int CommentId=0;
                    if (Request.QueryString["CommentId"] != null)
                    {
                        int.TryParse(Request.QueryString["CommentId"], out CommentThredId);
                        Hdn_thredId.Value = CommentThredId.ToString();

                        if (CommentThredId != 0)
                        {
                            LoadComments(CommentThredId);
                        }
                        else
                        {
                            
                            if (Request.QueryString["Type"] != null)
                            {
                                Type = Request.QueryString["Type"].ToString();
                                Hdn_Type.Value = Type.ToString();
                                if (Request.QueryString["UserId"] != null)
                                {
                                    int.TryParse(Request.QueryString["UserId"].ToString(), out userId);
                                    if (CommentExistUnderThisCreatedUser(Type, userId, out CommentId) && CommentId!=0)
                                    {
                                        Response.Redirect("CommentPannel.aspx?CommentId="+CommentId);
                                    }
                                    else
                                    {
                                        Hdn_UsedId.Value = userId.ToString();
                                        Pnl_Details.Visible = true;
                                        Pnl_Err.Visible = false;
                                        Pnl_AddNew.Visible = true;
                                        Lbl_Msg.Text = "No comments found.";
                                    }
                                }
                                else
                                {
                                    Pnl_Details.Visible = false;
                                    Pnl_Err.Visible = true;
                                    Pnl_AddNew.Visible = false;
                                    Lbl_Msg.Text = "";
                                    Lbl_Err.Text = "User id does not found.";
                                }
                            }
                            else
                            {
                                Pnl_Details.Visible = false;
                                Pnl_Err.Visible = true;
                                Pnl_AddNew.Visible = false;
                                Lbl_Err.Text = "Comment type does not found.";
                            }
                          

                        }

                    }
                    else
                    {
                        Pnl_Details.Visible = false;
                        Pnl_Err.Visible = true;
                        Lbl_Err.Text = "Comment id doesn't found.";
                        Pnl_AddNew.Visible = false;
                    }
                     
                }
            }
        }

        private bool CommentExistUnderThisCreatedUser(string Type, int userId, out int CommentId)
        {
            bool Exist = false;
            CommentId = 0;
            string sql = "";

            CommentManagementClass _CommentObj = new CommentManagementClass(MyStudMang.m_MysqlDb);
            switch (Type)
            {
                case "Student":
                   
                    sql = "select tblstudent.CommentThreadId from tblstudent  where tblstudent.Id=" + userId;
                    OdbcDataReader _Reader = _CommentObj.m_MysqlDb.ExecuteQuery(sql);
                    if (_Reader.HasRows)
                    {
                        Exist = true;
                        int.TryParse(_Reader.GetValue(0).ToString(), out CommentId);

                    }

                    break;

                case "TempStudent":

                    sql = " select tbltempstdent.CommentThreadId from  tbltempstdent  where tbltempstdent.Id=" + userId;
                    OdbcDataReader _Readernew = _CommentObj.m_MysqlDb.ExecuteQuery(sql);
                    if (_Readernew.HasRows)
                     {
                         Exist = true;
                         int.TryParse(_Readernew.GetValue(0).ToString(), out CommentId);

                     }

                    break;

                default:
                    Lbl_Comment.Text = "Type does not match.";
                    break;


            }
            
            return Exist;
        }

        

        private void LoadComments(int CommentThredId)
        {
            Pnl_Details.Visible = true;
            CommentManagementClass _CommentObj = new CommentManagementClass(MyStudMang.m_MysqlDb);
            Lbl_Err.Text = "";
            if (_CommentObj.IsExist(CommentThredId))
            {
                Hdn_thredId.Value = CommentThredId.ToString();
               
                OdbcDataReader Myreader = _CommentObj.ReadHeading(CommentThredId);
                if (Myreader.HasRows)
                {
                    Lbl_Heading.Text = Myreader.GetValue(1).ToString();
                }
                StringBuilder Comments = _CommentObj.GetComments(CommentThredId, MyUser);
                CommentDetails.InnerHtml = Comments.ToString();
            }
            else
            {
               
                Lbl_Msg.Text = "Comments doesn't found.";
            }
        }

        protected void Btn_AddNew_Click(object sender, EventArgs e)
        {
            Lbl_Comment.Text = "";
            Lbl_Msg.Text = "";
            CommentManagementClass _CommentObj = new CommentManagementClass(MyStudMang.m_MysqlDb);
             if (Txt_Messsage.Text != "")
             {
                 if (Hdn_thredId.Value != "0")
                 {
                     AddNewComments(_CommentObj);
                     
                 }
                 else
                 {
                     AddNewThread(_CommentObj);
                 }

             }
             else
             {
                 Lbl_Comment.Text = "Enter comments for adding.";
             }
           
             ScriptManager.RegisterClientScriptBlock(this.pnlAjaxUpdaet, this.pnlAjaxUpdaet.GetType(), "AnyScript", "pagereload();", true);
        }

        private void AddNewThread(CommentManagementClass _CommentObj)
        {
            string Type = Hdn_Type.Value;
            int Id = 0;
            int.TryParse(Hdn_UsedId.Value, out Id);
            string sql;
            int _thrdId = 0;
            switch (Type)
            {
                case "Student":
                    _thrdId=_CommentObj.CreateNewCommentReturnThredId("Student Details", Txt_Messsage.Text.ToString().Trim(), MyUser.LoginUserName);

                    sql = "update tblstudent set tblstudent.CommentThreadId=" + _thrdId + " where tblstudent.Id=" + Id ;
                    _CommentObj.m_MysqlDb.ExecuteQuery(sql);
                break;

                case "TempStudent":
                    _thrdId=_CommentObj.CreateNewCommentReturnThredId("Temporary Student Details", Txt_Messsage.Text.ToString().Trim(), MyUser.LoginUserName);
                    sql = "update tbltempstdent set tbltempstdent.CommentThreadId=" + _thrdId + " where tbltempstdent.Id=" + Id;
                    _CommentObj.m_MysqlDb.ExecuteQuery(sql);

                break;

                default:
                Lbl_Comment.Text = "Type does not match.";
                break;

                  
            }
            

            Response.Redirect("CommentPannel.aspx?CommentId=" + _thrdId);
        }

        

        private void AddNewComments(CommentManagementClass _CommentObj)
        {
            int _ThredId = int.Parse(Hdn_thredId.Value.ToString().Trim());
            _CommentObj.AddComment(_ThredId, Txt_Messsage.Text.ToString().Trim(), MyUser.LoginUserName);
          
            Response.Redirect("CommentPannel.aspx?CommentId=" + _ThredId);
            Txt_Messsage.Text = "";
            Lbl_Comment.Text = "Comments successfully added. ";
           
        }
     
    }
}
