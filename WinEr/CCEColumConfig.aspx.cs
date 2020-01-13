using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WinEr
{
    public partial class CCEColumConfig : System.Web.UI.Page
    {
        private ExamManage MyExamMang;
        private KnowinUser MyUser;
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
            else
            {
                if (!IsPostBack)
                {
                    //some initlization
                    Load_PoPupDefaultvalues();
                    load_Grid();
                    Btn_Add.Focus();

                }
            }

        }

        /// <summary>
        /// here current colum configuration loading from grid
        /// </summary>
        private void load_Grid()
        {
            Grd_CCE.DataSource = null;
            Grd_CCE.DataBind();
            string sql = "SELECT tblcce_colconfig.Id as Id,tblcce_classgroup.Id as GroupId,tblcce_classgroup.GroupName as GroupName,tblcce_term.Id as TremId,tblcce_term.TermName as TermName,tblcce_colconfig.ExamName as ExamName,tblcce_colconfig.TableName as TableName,tblcce_colconfig.ColName as ColumnName,tblcce_colconfig.ExamMaxMark as MaxMark from tblcce_classgroup INNER JOIN tblcce_colconfig ON tblcce_colconfig.GroupId=tblcce_classgroup.Id INNER JOIN tblcce_term ON tblcce_term.Id=tblcce_colconfig.TermId";
            MydataSet = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (MydataSet.Tables[0].Rows.Count > 0)
            {
                Grd_CCE.Columns[0].Visible = true;
                Grd_CCE.Columns[1].Visible = true;
                Grd_CCE.Columns[3].Visible = true;
                Grd_CCE.DataSource = MydataSet;
                Grd_CCE.DataBind();
                Grd_CCE.Columns[0].Visible = false;
                Grd_CCE.Columns[1].Visible = false;
                Grd_CCE.Columns[3].Visible = false;
                Griddiv.Visible = true;
                Label1.Visible = false;
            }
            else
            {
                Griddiv.Visible =false;
                Label1.Visible = true;
                Label1.Text = "Create column configuration!";
            }
           
        }


        /// <summary>
        /// after clicking this button create column configuration popup will show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            MPE_MessageBox.Show();
        }

        /// <summary>
        /// this function loading with all popup controls values
        /// </summary>
        private void Load_PoPupDefaultvalues()
        {
            Txt_Exam.Text = "";
            Txt_maxmark.Text = "";


            #region Load Group drop down
            Drp_Groupname.Items.Clear();
            string sql = "SELECT tblcce_classgroup.Id as GroupId,tblcce_classgroup.GroupName as GroupName from tblcce_classgroup";
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem li;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    li = new ListItem(dr["GroupName"].ToString(), dr["GroupId"].ToString());
                    Drp_Groupname.Items.Add(li);
                }
            }
            else
            {
                li = new ListItem("NO Data Found", "0");
                Drp_Groupname.Items.Add(li);
            }
            #endregion

            #region Load Term drop down
            Drp_Termname.Items.Clear();
            sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName from tblcce_term";
            DataSet _dsterm=MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            ListItem _literm;
            if (_dsterm.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _dsterm.Tables[0].Rows)
                {
                    _literm = new ListItem(dr["TermName"].ToString(), dr["Id"].ToString());
                    Drp_Termname.Items.Add(_literm);
                }
            }
            else
            {
                _literm = new ListItem("NO Data Found", "0");
                Drp_Termname.Items.Add(_literm);
            }
            #endregion

            #region load table details 
            Drp_Tablename.Items.Clear();
            Drp_Tablename.Items.Add("tblcce_mark");
            Drp_Tablename.Items.Add("tblcce_result");
            #endregion

            #region load Column details
            LoadColumConfig(Drp_Tablename.SelectedItem.Text);  
            #endregion

        }

        protected void Drp_Tablename_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Tablename = Drp_Tablename.SelectedItem.Text;
            LoadColumConfig(Tablename);
            MPE_MessageBox.Show();
           
        }
       
        /// <summary>
        /// load all columns from column dropdown
        /// </summary>
        /// <param name="Tablename"></param>
        public void LoadColumConfig(string Tablename)
        {
            Drp_Columnname.Items.Clear();
       

            int Columncount_marktbl = 0;
            int Columncount_resulttbl = 0;
            if (Tablename == "tblcce_mark")
            {

                Columncount_marktbl = GetTableColumnCount("tblcce_mark");
                if (Columncount_marktbl != 0)
                {
                    for (int i = 1; i <= Columncount_marktbl; i++)
                    {
                        Drp_Columnname.Items.Add("Col" + i);
                    }
                }
                else
                    Drp_Columnname.Items.Add("No Column Found!");


            }
            else
            {
                Columncount_resulttbl = GetTableColumnCount("tblcce_result");
                if (Columncount_resulttbl != 0)
                {
                    for (int i = 1; i <= Columncount_resulttbl; i++)
                    {
                        Drp_Columnname.Items.Add("RCol" + i);
                    }
                }
                else
                    Drp_Columnname.Items.Add("No Column Found!");
            }

        }

        /// <summary>
        /// get tblecce_result and tblcce_mark tables columns 
        /// </summary>
        /// <param name="Tablename"></param>
        /// <returns></returns>
        private int GetTableColumnCount(string Tablename)
        {
            int count = 0;
            string sql = "select * from " + Tablename;
            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (ds != null || ds.Tables[0] != null || ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    count++;
                }
                count = count - 3;
            }
            return count;

        }

        /// <summary>
        /// this event will create column configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Create_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
            string _Err ="S";
            string Examname="",Tablename="",Columnname="";
            int Groupid = 0,TermId=0;
            double MaxMark=0.00;
            if (Txt_Exam.Text == "")
            {
                _Err = "Enter the exam name!";
            }
            else if(Txt_maxmark.Text=="")
            {
                _Err = "Enter the Maxmimum Exam Mark!";
            }
            else
            {                
                try
                {
                    Examname = Txt_Exam.Text.Trim();
                    Groupid = int.Parse(Drp_Groupname.SelectedValue);
                    TermId = int.Parse(Drp_Termname.SelectedValue);
                    Tablename = Drp_Tablename.SelectedItem.Text;
                    Columnname = Drp_Columnname.SelectedItem.Text;
                    MaxMark = double.Parse(Txt_maxmark.Text);
                    Validation_And_Mapping(out _Err, Columnname, Tablename, Groupid, TermId, Examname, MaxMark, Groupid);
                    logger.LogToFile("CCE column configuration",Examname+" column configuration is created", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE column configuration", Examname + " column configuration is created", 1);

                }
                catch (Exception ex)
                {
                    _Err = ex.Message;
                    logger.LogToFile("CCE column configuration", "throws Error" + _Err, 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                    MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE column configuration", "throws Error" + _Err, 1);

                }

                if (_Err != "")
                {
                    load_Grid();
                    Load_PoPupDefaultvalues();
                }
            }
            if (_Err != "s")
                WC_MessageBox.ShowMssage(_Err);
        }

        /// <summary>
        /// while creating column configration it will check with validation
        /// </summary>
        /// <param name="_Err"></param>
        /// <param name="Columnname"></param>
        /// <param name="Tablename"></param>
        /// <param name="GroupId"></param>
        /// <param name="TermId"></param>
        /// <param name="Examname"></param>
        /// <param name="MaxMark"></param>
        /// <param name="Groupid"></param>
        /// <returns></returns>
        private bool Validation_And_Mapping(out string _Err, string Columnname, string Tablename, int GroupId, int TermId, string Examname, double MaxMark, int Groupid)
        {
            string sql = "";            
            bool IsExist = false;
            _Err = "";
            sql = "SELECT tblcce_colconfig.ColName from tblcce_colconfig WHERE tblcce_colconfig.ColName='" + Columnname + "' AND tblcce_colconfig.TableName='" + Tablename + "' AND tblcce_colconfig.GroupId=" + GroupId;
            DataSet Exam = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (Exam.Tables[0].Rows.Count > 0)
            {
                _Err = "This configuration is Exist!";
                IsExist = true;
            }
            if (!IsExist)
            {
                sql = "INSERT into tblcce_colconfig (tblcce_colconfig.GroupId,tblcce_colconfig.TermId,tblcce_colconfig.ExamName,tblcce_colconfig.TableName,tblcce_colconfig.ColName,tblcce_colconfig.ExamMaxMark) VALUES (" + Groupid + "," + TermId + ",'" + Examname + "','" + Tablename + "','" + Columnname + "'," + MaxMark + ") ";
                _Err = "Column configuration created sucessfully";
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
            }
            return IsExist;
        }

        protected void grd_courseRowCommand(object sender, GridViewCommandEventArgs e)//Create Config and Edit Config
        {
            int Index = Convert.ToInt32(e.CommandArgument);
            Lbl_groupid.Text = Index.ToString();
            Btn_no.Focus();
            MPE_yesornoMessageBox.Show();
        }

        protected void Btn_yes_Click(object sender, EventArgs e)
        {
            CLogging logger = CLogging.GetLogObject();
           
            string _Err = "";
            string sql = "";
            int _id = 0;
            int _Groupid = 0;
            int _Termid = 0;
            string _columnname = "", _Examname = "";
            int Index = 0;
            if (Lbl_groupid.Text != "")
                Index=int.Parse(Lbl_groupid.Text.ToString());
            _id = int.Parse(Grd_CCE.Rows[Index].Cells[0].Text);
            _Groupid = int.Parse(Grd_CCE.Rows[Index].Cells[1].Text);
            _Termid = int.Parse(Grd_CCE.Rows[Index].Cells[3].Text);
            _columnname = Grd_CCE.Rows[Index].Cells[7].Text;
            _Examname = Grd_CCE.Rows[Index].Cells[5].Text;
            if (_Groupid != 0 && _id != 0)
            {
                sql = "DELETE FROM tblcce_colconfig WHERE tblcce_colconfig.Id=" + _id + " AND tblcce_colconfig.GroupId=" + _Groupid + " AND tblcce_colconfig.TermId=" + _Termid + " AND tblcce_colconfig.ExamName='" + _Examname + "' AND tblcce_colconfig.ColName='" + _columnname + "'";
                MyExamMang.m_MysqlDb.ExecuteQuery(sql);
                load_Grid();
                _Err = "Selected configuration is deleted sucessfully!";
                logger.LogToFile("CCE column configuration", Grd_CCE.Rows[Index].Cells[5].Text.ToString() + " column onfiguration  removed", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE column configuration", _Examname +" column onfiguration  removed", 1);
            }
            else
            {
                _Err = "Selected configuration is not deleted sucessfully!";
                logger.LogToFile("CCE column configuration", Grd_CCE.Rows[Index].Cells[5].Text.ToString() + " column onfiguration  removing is failed", 'E', CLogging.PriorityEnum.LEVEL_VERY_IMPORTANT, MyUser.LoginUserName);
                MyUser.m_DbLog.LogToDb(MyUser.UserName, "CCE column configuration", _Examname + " column onfiguration  removing is failed", 1);

            }
            WC_MessageBox.ShowMssage(_Err);
 
        }
   
    }
}


//#region edit 

//        protected void Drp_TablenameEdit_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            //string Tablename = Drp_TablenameEdit.SelectedItem.Text;
//            //LoadEditColumConfig(Tablename);
//            //MPE_MessageBoxEdit.Show();
//        }
       
//        protected void Btn_CreateEdit_Click(object sender, EventArgs e)
//        {
//            string _Err = "S";
//            string Examname = "", Tablename = "", Columnname = "";
//            int Groupid = 0, TermId = 0;
//            double MaxMark = 0.00;
//            if (Txt_ExamEdit.Text == "")
//            {
//                _Err = "Enter the exam name!";
//            }
//            else if (Txt_maxmarkEdit.Text == "")
//            {
//                _Err = "Enter the Maxmimum Exam Mark!";
//            }
//            else
//            {
//                string _message = "S";
//                try
//                {
//                    string sql = "";
//                    Examname = Txt_ExamEdit.Text.Trim();
//                    Groupid = int.Parse(Drp_GroupnameEdit.SelectedValue);
//                    TermId = int.Parse(Drp_TermnameEdit.SelectedValue);
//                    Tablename = Drp_TablenameEdit.SelectedItem.Text;
//                    Columnname = Drp_ColumnnameEdit.SelectedItem.Text;
//                    MaxMark = double.Parse(Txt_maxmarkEdit.Text);
//                    /*
//                    if (!Validation(out _Err, Columnname, Tablename, Groupid))
//                    {
//                        sql = "UPDATE tblcce_colconfig SET tblcce_colconfig.GroupId=" + Groupid + ",tblcce_colconfig.TermId=" + TermId + ",tblcce_colconfig.ExamName='" + Examname + "',tblcce_colconfig.TableName='" + Tablename + "',tblcce_colconfig.ColName='" + Columnname + "',tblcce_colconfig.ExamMaxMark=" + MaxMark + " WHERE tblcce_colconfig.Id=" + ColumconfigId + "";
//                        _message = "Column configuration updated sucessfully";
//                        MyExamMang.m_MysqlDb.ExecuteQuery(sql);
//                    }
//                     * */
//                }
//                catch (Exception ex)
//                {
//                    _message = ex.Message;
//                }
//                if (_message != "S")
//                {
//                    load_Grid();
//                    WC_MessageBox.ShowMssage(_message);

//                }
//            }
//            if (_Err != "S")
//            {
//                WC_MessageBox.ShowMssage(_Err);
//            }
//        }

//        private void Load_EditPoPupDefaultvalues(int Index)
//        {
//            Txt_ExamEdit.Text = Grd_CCE.Rows[Index].Cells[5].Text;
//            Txt_maxmarkEdit.Text = Grd_CCE.Rows[Index].Cells[8].Text;


//            #region Load Group drop down
//            Drp_GroupnameEdit.Items.Clear();
//            string sql = "SELECT tblcce_classgroup.Id as GroupId,tblcce_classgroup.GroupName as GroupName from tblcce_classgroup";
//            DataSet ds = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
//            ListItem li;
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                foreach (DataRow dr in ds.Tables[0].Rows)
//                {
//                    li = new ListItem(dr["GroupName"].ToString(), dr["GroupId"].ToString());
//                    Drp_GroupnameEdit.Items.Add(li);
//                }
//            }
//            else
//            {
//                li = new ListItem("NO Data Found", "0");
//                Drp_GroupnameEdit.Items.Add(li);
//            }
//            #endregion

//            #region Load Term drop down
//            Drp_TermnameEdit.Items.Clear();
//            sql = "SELECT tblcce_term.Id as Id,tblcce_term.TermName as TermName from tblcce_term";
//            DataSet _dsterm = MyExamMang.m_MysqlDb.ExecuteQueryReturnDataSet(sql);
//            ListItem _literm;
//            if (_dsterm.Tables[0].Rows.Count > 0)
//            {
//                foreach (DataRow dr in _dsterm.Tables[0].Rows)
//                {
//                    _literm = new ListItem(dr["TermName"].ToString(), dr["Id"].ToString());
//                    Drp_TermnameEdit.Items.Add(_literm);
//                }
//            }
//            else
//            {
//                _literm = new ListItem("NO Data Found", "0");
//                Drp_TermnameEdit.Items.Add(_literm);
//            }
//            #endregion

//            #region load table details
//            Drp_TablenameEdit.Items.Clear();
//            Drp_TablenameEdit.Items.Add("tblcce_mark");
//            Drp_TablenameEdit.Items.Add("tblcce_result");
//            #endregion

//            #region load Column details
//            LoadEditColumConfig(Drp_TablenameEdit.SelectedItem.Text);
//            #endregion
//        }

//        protected void Grd_CCE_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string _Err = "";
//            try
//            {
//                string sql = "";
//                int _id = 0;
//                int _Groupid = 0;
//                int _Termid = 0;
//                string _columnname = "", _Examname = "";
//                int selectedrowindex = Grd_CCE.SelectedIndex;

//                _id = int.Parse(Grd_CCE.Rows[selectedrowindex].Cells[0].Text);
//                _Groupid = int.Parse(Grd_CCE.Rows[selectedrowindex].Cells[1].Text);
//                _Termid = int.Parse(Grd_CCE.Rows[selectedrowindex].Cells[3].Text);
//                _columnname = Grd_CCE.Rows[selectedrowindex].Cells[7].Text;
//                _Examname = Grd_CCE.Rows[selectedrowindex].Cells[5].Text;
//                if (_Groupid != 0 && _id != 0)
//                {
//                    sql = "DELETE FROM tblcce_colconfig WHERE tblcce_colconfig.Id=" + _id + " AND tblcce_colconfig.GroupId=" + _Groupid + " AND tblcce_colconfig.TermId=" + _Termid + " AND tblcce_colconfig.ExamName='" + _Examname + "' AND tblcce_colconfig.ColName='" + _columnname + "'";
//                    MyExamMang.m_MysqlDb.ExecuteQuery(sql);
//                    load_Grid();
//                    _Err = "Selected configuration is deleted sucessfully!";
//                }
//                else
//                    _Err = "Selected configuration is not deleted sucessfully!";
//            }
//            catch (Exception ex)
//            {
//                _Err = "Selected configuration is not deleted sucessfully!" + ex.Message;
//            }
//            WC_MessageBox.ShowMssage(_Err);
//        }

//        public void LoadEditColumConfig(string Tablename)
//        {
//            Drp_ColumnnameEdit.Items.Clear();


//            int Columncount_marktbl = 0;
//            int Columncount_resulttbl = 0;
//            if (Tablename == "tblcce_mark")
//            {

//                Columncount_marktbl = GetTableColumnCount("tblcce_mark");
//                if (Columncount_marktbl != 0)
//                {
//                    for (int i = 1; i <= Columncount_marktbl; i++)
//                    {
//                        Drp_ColumnnameEdit.Items.Add("Col" + i);
//                    }
//                }
//                else
//                    Drp_ColumnnameEdit.Items.Add("No Column Found!");


//            }
//            else
//            {
//                Columncount_resulttbl = GetTableColumnCount("tblcce_result");
//                if (Columncount_resulttbl != 0)
//                {
//                    for (int i = 1; i <= Columncount_resulttbl; i++)
//                    {
//                        Drp_ColumnnameEdit.Items.Add("RCol" + i);
//                    }
//                }
//                else
//                    Drp_ColumnnameEdit.Items.Add("No Column Found!");
//            }

//        }

//        #endregion

//#region edit design
//<asp:Panel ID="Pnl_MessageBoxEdit" runat="server">
//  <asp:Button runat="server" ID="Btn_hdnmessagetgtEdit" style="display:none"/>
//  <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBoxEdit" runat="server"  CancelControlID="Btn_magokEdit" 
//    PopupControlID="Pnl_msgEdit" TargetControlID="Btn_hdnmessagetgtEdit"  />
//   <asp:Panel ID="Pnl_msgEdit" runat="server" style="display:none;">
//   <div class="container skin5" style="width:400px; top:400px;left:400px" >
//      <table   cellpadding="0" cellspacing="0" class="containerTable">
//        <tr >
//            <td class="no"><asp:Image ID="Image4Edit" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
//                        Height="28px" Width="29px" /> </td>
//            <td class="n"><span style="color:White">Edit Column Configuration</span></td>
//            <td class="ne">&nbsp;</td>
//        </tr>
//        <tr >
//            <td class="o"> </td>
//            <td class="c" >

//                        <br /><br />
//                        <div style="text-align:center;">
//                        <table class="tablelist">
//                           <tr>
//                           <td class="leftside"></td>
//                           <td class="rightside">
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Exam Name</td>
//                           <td class="rightside">
//                               <asp:TextBox ID="Txt_ExamEdit" runat="server" Width="160px" MaxLength="150" ToolTip="Enter the Exam name"></asp:TextBox>
//                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
//                                 runat="server" Enabled="True" TargetControlID="Txt_ExamEdit"  FilterType="Custom"  FilterMode="InvalidChars" InvalidChars=".!@#$%^&;*()~?><|\';:">
//                                </ajaxToolkit:FilteredTextBoxExtender>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Enter Max Exam Mark</td>
//                           <td class="rightside">
//                               <asp:TextBox ID="Txt_maxmarkEdit" runat="server" Width="160px" MaxLength="3" ToolTip="Enter the mark" AutoPostBack="true"></asp:TextBox>
//                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_maxmarkEdit_FilteredTextBoxExtender" 
//                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
//                                            TargetControlID="Txt_maxmarkEdit" ValidChars=".a">
//                                        </ajaxToolkit:FilteredTextBoxExtender>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Group Name</td>
//                           <td class="rightside">
//                               <asp:DropDownList ID="Drp_GroupnameEdit" runat="server" Width="160px" >
//                               </asp:DropDownList>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Term Name</td>
//                           <td class="rightside">
//                               <asp:DropDownList ID="Drp_TermnameEdit" runat="server" Width="160px">
//                               </asp:DropDownList>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Table Name</td>
//                           <td class="rightside">
//                               <asp:DropDownList ID="Drp_TablenameEdit" runat="server" Width="160px" OnSelectedIndexChanged="Drp_TablenameEdit_SelectedIndexChanged" AutoPostBack="true">
//                               </asp:DropDownList>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside">Column Name</td>
//                           <td class="rightside">
//                               <asp:DropDownList ID="Drp_ColumnnameEdit" runat="server" Width="160px" >
//                               </asp:DropDownList>
//                           </td>
//                           </tr>
//                           <tr>
//                           <td class="leftside"></td>
//                           <td class="rightside">
//                           </td>
//                           </tr>
                           
//                        </table>
//                        <br />
//                        <table class="tablelist">
//                        <tr>
//                        <td class="leftside">
//                        </td>
//                        <td class="rightside">
//                         <asp:Button ID="Btn_CreateEdit" runat="server" Text="Update" CssClass="popupbuttonstyle" ToolTip="Update Column config" onclick="Btn_CreateEdit_Click"/>
//                         <asp:Button ID="Btn_magokEdit" runat="server" Text="Close" CssClass="popupbuttonstyle" ToolTip="Close" />
//                        </td>
//                        </tr>
//                        </table>
//                        </div>
//                        <br /><br />
                         
//            </td>
//            <td class="e"> </td>
//        </tr>
//        <tr>
//            <td class="so"> </td>
//            <td class="s"> </td>
//            <td class="se"> </td>
//        </tr>
//    </table>
//   </div>
//   </asp:Panel>
//  </asp:Panel>
//#endregion




