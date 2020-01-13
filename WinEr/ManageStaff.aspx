<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" Inherits="ManageStaff"  Codebehind="ManageStaff.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    
     </script>
        
    <style type="text/css">
 
 .TdRight
        {
            text-align:left;
           
        }
        .TdLeft
        {
             text-align:right;
              
        }
        .Asteric
        {
            color:Red;
        }
        </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <div id="contents" >
<div id="right">

<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>
</div>

 
<div id="left">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

   


     <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                      <%-- <tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
      
      
      
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no"> <img alt="" src="Pics/Staff/mypc_write.png" height="35" width="35" /></td>
                <td class="n">Manage Staff Details</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
  
                   
                      <ajaxToolkit:TabContainer runat="server" ID="Tabs" 
                          CssClass="ajax__tab_yuitabview-theme" Width="100%" ActiveTabIndex="0" >
                                        
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/business_user.png" /> <b> GENERAL</b>
                                             </HeaderTemplate>
                                            <ContentTemplate>
                                             <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
                <ProgressTemplate>               
                
                        <div id="progressBackgroundFilter">

                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
</asp:UpdateProgress>
                                              <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                                                <ContentTemplate>
                                             <asp:Panel ID="Panel4" runat="server" DefaultButton="Btn_Update">
    <table class="tablelist">
        <tr>
            <td >
                &nbsp;</td>
            <td >
                &nbsp;</td>
        </tr>
        <tr>
            <td class="leftside">
                Staff Name<span  class="Asteric">*</span> :</td>
            <td  class="rightside">
                <asp:TextBox ID="Txt_StaffName" runat="server" class="form-control" Width="160px" MaxLength="45"></asp:TextBox>
                 <ajaxToolkit:FilteredTextBoxExtender
                   ID="FilteredTextBoxExtender1"
                   runat="server"
                   TargetControlID="Txt_StaffName"
                   FilterType="Custom"
                   FilterMode="InvalidChars"
                   InvalidChars="'/\"
            />
                </td>
            
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                <td class="leftside">
                Gender
                </td>
                <td class="rightside">
                    <asp:RadioButtonList ID="RbLst_Sex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="Male">Male</asp:ListItem>
                        <asp:ListItem Value="Female">Female</asp:ListItem>
                    </asp:RadioButtonList>
                   
                    
                </td>
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
                <td class="leftside">
                    D.O.B<span class="Asteric">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Dob" runat="server" Width="160px" class="form-control"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" CssClass="cal_Theme1"
                        Enabled="True" TargetControlID="Txt_Dob" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>
                </td>
               
                    <%--<asp:RegularExpressionValidator runat="server" ID="DateRegularExpressionValidator3"
                        ControlToValidate="Txt_Dob"
                        Display="None"
                        ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
                        ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                        <asp:RegularExpressionValidator ID="DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_Dob" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                   <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
                        TargetControlID="DateRegularExpressionValidator3"
                        HighlightCssClass="validatorCalloutHighlight" />
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
            <td class="leftside">
                    Role:</td>
            <td class="rightside">
                    <asp:DropDownList ID="Drp_SelectRole" runat="server" class="form-control" 
                        Width="160px">
                    </asp:DropDownList>
                </td>
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
            <td class="leftside">
                    Experience in Year<span  class="Asteric">*</span>:</td>
            <td class="rightside">
                    <asp:TextBox ID="Txt_Experience" runat="server" Width="160px" class="form-control" MaxLength="10"    ></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ExperienceFilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="Txt_Experience">
                    </ajaxToolkit:FilteredTextBoxExtender>
                </td>
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
            <td class="leftside">
                    Designation:</td>
            <td class="rightside">
                    <asp:TextBox ID="Txt_Desig" runat="server" Width="160px" class="form-control" MaxLength="45"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender
                   ID="FilteredTextBoxExtender2"
                   runat="server"
                   TargetControlID="Txt_Desig"
                   FilterType="Custom"
                   FilterMode="InvalidChars"
                   InvalidChars="'/\"
            />

                </td>
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
            <td class="leftside">
                    Joining Date<span class="Asteric">*</span> :</td>
            <td class="rightside">
                    <asp:TextBox ID="Txt_JoiningDate" runat="server" Width="160px" class="form-control"></asp:TextBox>
                                         <ajaxToolkit:CalendarExtender ID="Txt_JoiningDate_CalendarExtender"  CssClass="cal_Theme1"
                        runat="server" Enabled="True" TargetControlID="Txt_JoiningDate" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>
                     <%--<asp:RegularExpressionValidator runat="server" ID="joinDateRegularExpressionValidator3"
                                ControlToValidate="Txt_JoiningDate"
                                Display="None"
                                ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                <asp:RegularExpressionValidator ID="joinDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_JoiningDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                             <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="joinDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                </td>

          </tr>
          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
                
                <td valign="top" class="leftside">
                    Address<span class="Asteric">*</span> :</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Address" runat="server" class="form-control" Height="40px" TextMode="MultiLine" 
                        Width="160px" MaxLength="245"></asp:TextBox>
                        
                <ajaxToolkit:FilteredTextBoxExtender
                   ID="FilteredTextBoxExtender_address"
                   runat="server"
                   TargetControlID="Txt_Address"
                   FilterType="Custom"
                   FilterMode="InvalidChars"
                   InvalidChars="'\"
                        />
                </td>
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
            <td class="leftside">
                    Group Name:</td>
            <td class="rightside">
                    <asp:DropDownList ID="Drp_Group" runat="server" class="form-control" 
                        Width="160px">
                    </asp:DropDownList>
                </td>
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            
            <tr >
                <td class="leftside">
                    Phone No:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_PhNo" runat="server" MaxLength="25" class="form-control" Width="160px"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PhNoFilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Custom, Numbers" 
                        TargetControlID="Txt_PhNo" ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender>
                </td>
              
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
                <td class="leftside">
                    E-Mail</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Email" runat="server" Width="160px" class="form-control"  MaxLength="45"></asp:TextBox>                 
                                      <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_Email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                </td>

           
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
            
                <td class="leftside">
                    Staff Id/LoginName:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_StaffId" runat="server" class="form-control" Enabled="False" Width="160px"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>

                <td valign="top" class="leftside" >
                    Educational Qualification :</td>
                    
                <td class="rightside">
                    <asp:TextBox ID="Txt_EduQuli" runat="server" Height="40px" class="form-control" TextMode="MultiLine" 
                        Width="160px" MaxLength="245"></asp:TextBox>
                        
            <ajaxToolkit:FilteredTextBoxExtender
                   ID="FilteredTextBoxExtender3"
                   runat="server"
                   TargetControlID="Txt_EduQuli"
                   FilterType="Custom"
                   FilterMode="InvalidChars"
                   InvalidChars="'\"
                        />
                </td>
            </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>

                <td valign="top" class="leftside" >
                    Aadhar Number :</td>
                    
                <td class="rightside">
                    <asp:TextBox ID="Txt_Aadhar" runat="server" Height="40px" class="form-control" Width="160px" MaxLength="245"></asp:TextBox>
                        
         
                </td>
            </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>

                <td valign="top" class="leftside" >
                    PAN Number :</td>
                    
                <td class="rightside">
                    <asp:TextBox ID="Txt_PAN" runat="server" Height="40px" class="form-control" Width="160px" MaxLength="245"></asp:TextBox>
                        
      
                </td>
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
                 <td class="leftside">
                 </td>
                <td class="rightside">
                    <asp:RadioButtonList ID="Rdb_Login" runat="server" AutoPostBack="True"  RepeatDirection="Horizontal"
                        onselectedindexchanged="Rdb_Login_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">Cannot login</asp:ListItem>
                        <asp:ListItem Value="1">Can Login</asp:ListItem>
                    </asp:RadioButtonList>
                    &nbsp;&nbsp;<asp:LinkButton ID="Lnk_Reset" runat="server" Visible="false" OnClick="Lnk_reset_Click">Reset Password</asp:LinkButton>
                </td>
                
            </tr>
            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
            <tr>
                 <td colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Visible="False">
                        <table class="tablelist" >
                            <tr>
                                <td class="leftside">New Password:</td>
                                <td class="rightside"> 
                                     <asp:TextBox ID="Txt_Password" runat="server" class="form-control" TextMode="Password" Width="160px" 
                                     MaxLength="25"></asp:TextBox>
                                     
                                     <asp:RegularExpressionValidator runat="server" ID="PassWordRegularExpressionValidator1"
                                      ControlToValidate="Txt_PassWord"  Display="None"
                                ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                            ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                                 

                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender10"
                                TargetControlID="PassWordRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                                </td>                                
                            </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                            <tr>
                                 <td class="leftside"> Re-Enter Password :</td>
                                 <td class="rightside">  
                                   <asp:TextBox ID="Txt_RePassword" runat="server" class="form-control" TextMode="Password" 
                                   Width="160px" MaxLength="25"></asp:TextBox>
                                   
                                    <asp:RegularExpressionValidator runat="server" ID="ConPassWordRegularExpressionValidator1"
                                ControlToValidate="Txt_RePassword"
                                Display="None"
                                ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                            ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                                  <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="ConPassWordRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                                 </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style=" padding-left:20%" >
                   <asp:Panel ID="Pnl_Subjects" runat="server">
                         <table>
                            <tr>
                                <td > All Subjects</td>
                                <td></td>
                                <td > Subject Handled By Staff</td>
                            </tr>
                             <tr>
                                 <td >
                    <div style="OVERFLOW: auto; WIDTH: 200px; HEIGHT: 150px; BACKGROUND-COLOR: gainsboro">
                        <asp:CheckBoxList ID="ChkBox_AllsSub" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="170px">
                        </asp:CheckBoxList>
                    </div>
                                </td>
                <td >
                    <asp:Button ID="Btn_Add" runat="server" Text="&gt;&gt;" Width="50px" class="btn btn-primary"
                        onclick="Btn_Add_Click" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="Btn_Remove" runat="server" Text="&lt;&lt;" class="btn btn-primary"
                        Width="50px" onclick="Btn_Remove_Click" />
                </td>
                <td >
                    
                    <div style="OVERFLOW: auto; WIDTH: 200px; HEIGHT: 150px; BACKGROUND-COLOR: gainsboro">
                        <asp:CheckBoxList ID="ChkBox_AssSub" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="170px">
                        </asp:CheckBoxList>
                    </div>
                </td>
                         </tr>
                 </table>
               </asp:Panel>
                     
                </td>
            </tr>
            <tr>
            <td colspan="2" align="center">
                            <asp:Panel ID="PnlPayrollYesNo" runat="server" Width="100%">
               <table class="tablelist">
               <tr>
               <td>
            <tr>
                <td class="leftside">
                   Payroll active?
                </td>
                <td class="rightside">
                    <asp:RadioButton ID="RdbYes" runat="server" Text="Yes" GroupName="Payroll" 
                        AutoPostBack="True" oncheckedchanged="RdbYes_CheckedChanged"/>&nbsp;
                    <asp:RadioButton ID="RdbNo" runat="server" Text="No" GroupName="Payroll" 
                        AutoPostBack="True" oncheckedchanged="RdbNo_CheckedChanged"/>
                </td>
            </tr>
            <tr>
            <td style="width:100%" colspan="2">
             <asp:Panel ID="PnlPayrollDetails" runat="server" Width="100%">
              <table class="tablelist">
               <tr>
               <td>
            <tr>
                
                <td class="leftside">
                    Employee Id</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtEmpId" runat="server" Width="160px" class="form-control" MaxLength="10"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" 
                      runat="server" 
                      Enabled="True" 
                      FilterType="Custom" 
                      FilterMode="InvalidChars" 
                      InvalidChars="`~!@#$%^&*()=+|[]{};:,./<>?'\" TargetControlID="TxtEmpId">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="CreateBtn" ControlToValidate="TxtEmpId" ErrorMessage="You Must enter Employee ID"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Payroll Type</td>
                <td class="rightside">
                    <asp:DropDownList ID="DrpPayroll" runat="server" class="form-control" Width="160px">
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    PAN number</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtPan" runat="server" Width="160px" MaxLength="15" class="form-control"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                      TargetControlID="TxtPan" InvalidChars=";:',.?/~`!@#$%^&*()_+|\][>-+}{<">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                               <%--  <asp:RegularExpressionValidator ID="reg" runat="server" Display="Static" ValidationGroup="CreateBtn"  ControlToValidate="TxtPan"
ErrorMessage="Please Enter Valid Pan Number"
ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"></asp:RegularExpressionValidator>--%>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="CreateBtn" ControlToValidate="TxtPan" ErrorMessage="You Must enter PAN number"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Bank Name</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtBankName" runat="server" Width="160px" MaxLength="50" class="form-control"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Name_FilteredTextBoxExtender1" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\0123456789" TargetControlID="TxtBankName">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="CreateBtn" ControlToValidate="TxtBankName" ErrorMessage="You Must enter Bank name"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Bank Account no.</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtAcc" runat="server" Width="160px" MaxLength="20" class="form-control"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" 
                                     runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom" 
                                     ValidChars="0123456789" TargetControlID="TxtAcc">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="CreateBtn" ControlToValidate="TxtAcc" ErrorMessage="You Must enter account number"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
             </td>
            </tr>
            </table>
            </asp:Panel>
            </td>
            </tr>
            </td>
            </tr>
            </table>
             </asp:Panel>

            
            </td>
            </tr>
            <tr>
                
                 <td colspan="2" align="center">   
                    <asp:Label ID="Lbl_Failue" runat="server" ForeColor="#FF3300" Height="16px"></asp:Label>
                </td>
           </tr>
           <tr>
                <td  colspan="2" align="center">
                    
                    <asp:Button ID="Btn_Update" runat="server" onclick="Btn_Update_Click" ValidationGroup="CreateBtn"
                        Text="Save" class="btn btn-success" />
                        &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                        Text="Cancel"   class="btn btn-danger" />
                </td>
            </tr>

         </table>
          
       </asp:Panel>
       
 <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
                                             </ContentTemplate>
                                                 </asp:UpdatePanel>
                                           </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/image_icon.png" /> <b> UPLOAD PHOTO</b>
                                             </HeaderTemplate>
                                            <ContentTemplate>
                                            <br />
                                             <asp:Panel ID="Panel1" runat="server" DefaultButton="Btn_Upload">
                                             <table class="tablelist">
                                             <tr>
                                             <td class="leftside">
                                                 &nbsp;</td>
                                             <td class="rightside">
                                                 &nbsp;</td>
                                             </tr>
                                                 <tr>
                                                     <td class="leftside">
                                                         &nbsp;</td>
                        
                                                     <td class="rightside" align="right">
                                                     
                                                         <asp:LinkButton ID="Lnk_Cam" runat="server" onclick="Lnk_Cam_Click">Capture Photos</asp:LinkButton>
                                                       
                                                         &nbsp;</td>
                                                 </tr>
                                                 <tr>
                                                     <td class="leftside">
                                                         Select Photo
                                                     </td>
                                                     <td class="rightside">
                                                         <asp:FileUpload ID="FileUp_Photo" runat="server" Height="25px" />
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td class="leftside">
                                                         &nbsp;</td>
                                                     <td class="rightside">
                                                         &nbsp;</td>
                                                 </tr>
                                                  <tr>
                                                     <td class="leftside">
                                                         &nbsp;</td>
                                                     <td class="rightside">
                                                         <asp:Label ID="lbl_Upmessage" runat="server" ForeColor="Red"></asp:Label>
                                                        
                                                         </td>
                                                 </tr>
                                                 
                                                 <tr>
                                                     <td class="leftside">
                                                         &nbsp;</td>
                                                     <td class="rightside">
                                                         <asp:Button ID="Btn_Upload" runat="server" Text="Upload" 
                                                     onclick="Btn_Upload_Click"  class="btn btn-success" />&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="Btn_UpCancel" runat="server" onclick="Btn_UPLoadCancel_Click" Text="Cancel" 
                                                   class="btn btn-danger" />
                                                         
                                                         </td>
                                                 </tr>
                                                 <tr>
                                                     <td class="leftside">
                                                         &nbsp;</td>
                                                     <td class="rightside">
                                                         &nbsp;</td>
                                                 </tr>
                                             </table>
                                          
                                         </asp:Panel>
                                           </ContentTemplate>
                                           
                                        </ajaxToolkit:TabPanel>
                       </ajaxToolkit:TabContainer>

                </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>



      
</div>

<div class="clear"></div>
</div>
</asp:Content>
