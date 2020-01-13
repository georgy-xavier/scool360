<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" Codebehind="CreateStaff.aspx.cs" Inherits="CreateStaff"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">

    // disables the button specified and sets its style to a disabled "look".

    function disableButtonOnClick(oButton, sButtonText, sCssClass)

    {

        // set button to disabled so you can't click on it.

        oButton.disabled = true; 

        // change the text of the button.

        oButton.value = sButtonText; 

        // IE uses className for the css property.

        oButton.setAttribute('className', sCssClass); 

        // Firefox, Safari use class for the css property.  (doesn't hurt to do both).

        oButton.setAttribute('class', sCssClass);

    }

</script>
    <style type="text/css">
         
        .disabled_button
        {
        color:#aca899;
        background-color:#efefef;
        border:solid 1px #c0c0c0;
        }
        .TdRight
        {
            text-align:right;
        }
        .TdLeft
        {
             text-align:left;
        }
        .Astrick
        {
            color:Red;
        }
        </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="contents">


 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
    
    
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/Staff/mypc_add.png" height="35" width="35" /> </td>
                <td class="n">Create Staff</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                   
                      
                <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>
       <asp:Panel ID="Panel1" runat="server" DefaultButton="Btn_Create" >
      
        <table class="tablelist">
             
            <tr>
                
                <td class="leftside">
                     &nbsp;</td>
                <td  class="rightside">
                    &nbsp;</td>
            </tr>
             <tr>
                 <td class="leftside">
                     Name :<span class="Astrick">*</span></td>
                 <td class="rightside">
                     <asp:TextBox ID="Txt_StaffName" runat="server" MaxLength="25" class="form-control" Width="250px"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                         ControlToValidate="Txt_StaffName" ErrorMessage="You Must enter a name" 
                         ValidationGroup="CreateBtn"></asp:RequiredFieldValidator>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_StaffName">
                           </ajaxToolkit:FilteredTextBoxExtender>
                 </td>
                
            </tr>
             <tr>
                <td class="leftside">
                    Staff Id/LoginName:<span class="Astrick">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_StaffId" runat="server" Width="250px" class="form-control" MaxLength="25"></asp:TextBox>
                   
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_StaffId" ValidationGroup="CreateBtn" ErrorMessage="You Must enter a Login Id"></asp:RequiredFieldValidator>  
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                runat="server" Enabled="True" TargetControlID="Txt_StaffId" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                    </ajaxToolkit:FilteredTextBoxExtender>
                </td>
                 
            </tr>
            <tr>
                <td class="leftside">
                    D.O.B<span class="Astrick">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Dob" runat="server" class="form-control" Width="250px"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="CreateBtn" ControlToValidate="Txt_Dob" ErrorMessage="You Must enter DOB"></asp:RequiredFieldValidator>     
                 
                        <%-- <ajaxToolkit:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="Txt_Dob" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>--%>
                         <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_Dob">
                                                    </ajaxToolkit:MaskedEditExtender>
                              <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                ControlToValidate="Txt_Dob"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
                                TargetControlID="DobDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />                       
        
                </td>
        
                  
            </tr>
            <tr>
                <td class="leftside" c>
                    Joining Date :<span class="Astrick">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_JoiningDate" runat="server" class="form-control" Width="250px"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="CreateBtn" ControlToValidate="Txt_JoiningDate" ErrorMessage="You Must enter Joining date"></asp:RequiredFieldValidator>     
                 
                               <%--   <ajaxToolkit:CalendarExtender ID="Txt_JoiningDate_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="Txt_JoiningDate" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>--%>
                    
                      <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_JoiningDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                         <asp:RegularExpressionValidator runat="server" ID="DoJDateRegularExpressionValidator3"
                                ControlToValidate="Txt_JoiningDate"
                                Display="None"
                                 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="DoJDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                </td>

                    
                    <%--<asp:RegularExpressionValidator runat="server" ID="joinDateRegularExpressionValidator3"
                                ControlToValidate="Txt_JoiningDate"
                                Display="None"
                                ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                      <%--   <asp:RegularExpressionValidator ID="joinDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_JoiningDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />       
                             <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="joinDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />--%>
            </tr>
            <tr>
                
                <td class="leftside">
                    Address :<span class="Astrick">*</span></td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Address" runat="server" class="form-control" Height="40px" TextMode="MultiLine" 
                        Width="250px" MaxLength="245"></asp:TextBox>
                        
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="CreateBtn" ControlToValidate="Txt_Address" ErrorMessage="You Must enter address"></asp:RequiredFieldValidator>          
                                   <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_Address" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
               </ajaxToolkit:FilteredTextBoxExtender>
                </td>

            </tr>
            <tr>
             <td class="leftside">Gender : </td>
                <td class="rightside">
                
                
                                        <div class="radio radio-primary">
                                                 <asp:RadioButtonList ID="RbLst_Sex" class="form-actions" runat="server" 
                                                     RepeatDirection="Horizontal"  Width="160px">
                                                     <asp:ListItem Selected="True" Value="Male">Male</asp:ListItem>
                                                     <asp:ListItem Value="Female">Female</asp:ListItem>
                                                 </asp:RadioButtonList>
                                        </div>
                
                
                  
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Experience in Years:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Experience" runat="server" class="form-control" Width="250px" MaxLength="5"></asp:TextBox>
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
                    <asp:TextBox ID="Txt_Desig" runat="server" class="form-control" Width="250px" MaxLength="50"></asp:TextBox>   
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                runat="server" Enabled="True" TargetControlID="Txt_Desig" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                </ajaxToolkit:FilteredTextBoxExtender>
                </td>

                    
           </tr>
             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
            <tr>
                <td class="leftside">
                    Phone No:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_PhNo" runat="server" class="form-control" Width="250px" MaxLength="25"></asp:TextBox>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PhNo_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                        ValidChars="+"  TargetControlID="Txt_PhNo">
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
                    <asp:TextBox ID="Txt_Email" runat="server" class="form-control" Width="250px" MaxLength="45"></asp:TextBox>
                     <asp:RegularExpressionValidator runat="server" ID="PNRegEx" ValidationGroup="CreateBtn"
                                ControlToValidate="Txt_Email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1" 
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                </td>

                    
                   
                    
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
           
            <tr>
                
                <td class="leftside" >
                    Educational
                    <br />
                    Qualification :</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_EduQuli" runat="server" class="form-control" Height="40px" TextMode="MultiLine" 
                        Width="250px" MaxLength="245"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                runat="server" Enabled="True" TargetControlID="Txt_EduQuli" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                </ajaxToolkit:FilteredTextBoxExtender>
                </td>

           </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
           
            <tr>
                
                <td class="leftside" >
                    Aadhar Number:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Aadhar" runat="server" class="form-control" Width="250px" MaxLength="45"></asp:TextBox>                                     
                </td>

           </tr>
               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
           
            <tr>
                
                <td class="leftside" >
                    PAN Number:</td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Pan" runat="server" class="form-control" Width="250px" MaxLength="45"></asp:TextBox>                                       
                </td>

           </tr>
             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
            <tr>
            <td class="leftside">LogIn Provision</td>
                <td class="rightside">
                    <asp:RadioButtonList ID="Rdb_Login" class="form-actions" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="Rdb_Login_SelectedIndexChanged" 
                        RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="0">Cannot login</asp:ListItem>
                        <asp:ListItem Value="1">Can Login</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
               
                <td  colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Visible="False">
                        <table  class="tablelist">
                            <tr>
                                <td class="leftside">
                                    Enter PassWord :</td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_PassWord" runat="server" style="margin-left: 0px"
                                        TextMode="Password" class="form-control" Width="250px" MaxLength="15"></asp:TextBox>  
                                        
                                        <asp:RegularExpressionValidator runat="server" ID="PassWordRegularExpressionValidator1"  ValidationGroup="CreateBtn"
                                ControlToValidate="Txt_PassWord"
                                Display="None"
                                ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit."  />
                                 


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
                                <td class="leftside">
                                    Confirm Password :</td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_Confirm" runat="server" style="margin-left: 0px" 
                                       class="form-control" Width="250px" TextMode="Password" EnableTheming="False" MaxLength="15"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="ConPassWordRegularExpressionValidator1"  ValidationGroup="CreateBtn"
                                ControlToValidate="Txt_Confirm"  Display="None"
                                           ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                            ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                                 <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                            TargetControlID="ConPassWordRegularExpressionValidator1"
                                            HighlightCssClass="validatorCalloutHighlight" />
                                </td>
                                         

                            </tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
               
                <td  class="leftside">
                    Role:<span class="Astrick">*</span></td>
                <td  class="rightside">
                    <asp:DropDownList ID="Drp_SelectRole" class="form-control" runat="server" Height="35px" 
                        Width="250px">
                    </asp:DropDownList>
                </td>
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
          
            <tr>
                
                <td class="leftside">
                    Select Group</td>
                <td class="rightside">
                    <asp:DropDownList ID="Drp_ParentGroup" class="form-control" runat="server" Width="250px">
                    </asp:DropDownList>
                </td>
            </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
            <tr>
            <td style="width:100%" colspan="2">
                <asp:Panel ID="PnlPayrollYesNo" runat="server" Width="100%">
               <table class="tablelist">
               <tr>
               <td>
            <tr>
                <td class="leftside">
                   Payroll active?
                </td>
                <td class="rightside">
                    <asp:RadioButton ID="RdbYes" runat="server" class="form-actions" Text="Yes" GroupName="Payroll" 
                        AutoPostBack="True" oncheckedchanged="RdbYes_CheckedChanged"/>&nbsp;
                    <asp:RadioButton ID="RdbNo" runat="server" Text="No" calss="form-actions" GroupName="Payroll" 
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
                    <asp:TextBox ID="TxtEmpId" runat="server" class="form-control" Width="250px" MaxLength="10"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" 
                      runat="server" 
                      Enabled="True" 
                       FilterMode="InvalidChars"
                      FilterType="Numbers"  InvalidChars="`~!@#$%^&*()=+|[]{};:,./<>?'\"
                     TargetControlID="TxtEmpId">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                  <%--InvalidChars="`~!@#$%^&*()=+|[]{};:,./<>?'\" --%>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="CreateBtn" ControlToValidate="TxtEmpId" ErrorMessage="You Must enter Employee ID"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Payroll Type</td>
                <td class="rightside">
                    <asp:DropDownList ID="DrpPayroll" runat="server" class="form-control" Width="250px">
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    PAN number</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtPan" runat="server" class="form-control" Width="250px" MaxLength="15"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                      TargetControlID="TxtPan" InvalidChars=";:',.?/~`!@#$%^&*()_+|\][>-+}{<">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 <asp:RegularExpressionValidator ID="reg" runat="server" Display="Static" ValidationGroup="CreateBtn"  ControlToValidate="TxtPan"
ErrorMessage="Please Enter Valid Pan Number"
ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"></asp:RegularExpressionValidator>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                     ValidationGroup="CreateBtn" ControlToValidate="TxtPan" ErrorMessage="You Must enter PAN number">
                    </asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Bank Name</td>
                <td class="rightside">
                    <asp:TextBox ID="TxtBankName" runat="server" class="form-control" Width="250px" MaxLength="50"></asp:TextBox>
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
                    <asp:TextBox ID="TxtAcc" runat="server" class="form-control" Width="250px" MaxLength="20"></asp:TextBox>
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
                <td class="leftside">
                    &nbsp;</td>
                <td class="rightside">
                    &nbsp;</td>
            </tr>
            
           <tr >
           <td colspan="2">
            
           <asp:Panel ID="Pnl_Subjects" runat="server">
             <table class="tablelist">
            <tr>
              
                <td align="center" >
                    All Subjects</td>
                <td >
                </td>
                <td >
                    Subject Handled By Staff</td>
            </tr>
            <tr>
               
                <td class="TdRight" style=" padding-left:15%">
                    <div style="OVERFLOW: auto; WIDTH: 200px; HEIGHT: 150px; BACKGROUND-COLOR: gainsboro; text-align:left">
                        <asp:CheckBoxList ID="ChkBox_AllsSub" class="form-actions" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="170px">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td >
                    
                    <asp:Button ID="Btn_Add" runat="server" class="btn btn-info" Text="&gt;&gt;" Width="60px" 
                        onclick="Btn_Add_Click" />
                    <br />
                    <br />
                    <br />
                    
                    <asp:Button ID="Btn_Remove" runat="server" class="btn btn-info" Text="&lt;&lt;" 
                        Width="60px" onclick="Btn_Remove_Click" />
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
             
           <td class="leftside"></td>
           <td class="rightside"></td>
           </tr>
           <tr>
           <td class="leftside"></td>
           <td class="rightside">
            <asp:Label ID="Lbl_Failue" runat="server" class="control-label" ForeColor="#FF3300"></asp:Label>
           </td>
           </tr>
           
             <tr>
             
           <td class="leftside"></td>
           <td class="rightside"></td>
           </tr>
            <tr>
             
           <td class="leftside"></td>
           <td class="rightside">
           
            <asp:Button ID="Btn_Create" runat="server" onclick="Btn_Create_Click" ValidationGroup="CreateBtn"
           Text="Create" class="btn btn-success"/>
       &nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Reset" runat="server" onclick="Btn_Reset_Click" 
           Text="Reset" class="btn btn-primary" />
               &nbsp;&nbsp;&nbsp;
       <asp:TextBox ID="Txt_UserId" runat="server" class="form-control" Visible="False"></asp:TextBox>
       
       <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                runat="server" Enabled="True" TargetControlID="Txt_UserId" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                            </ajaxToolkit:FilteredTextBoxExtender>
           
           </td>
           </tr>
            <tr>
             
           <td class="leftside"></td>
           <td class="rightside"></td>
           </tr>
            </table>
           
    </asp:Panel>
 
        
      <WC:MSGBOX id="WC_MessageBox" runat="server" />   
       
       <div> 
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_LastMessage" BackgroundCssClass="modalBackground"
                                  runat="server" 
                                  PopupControlID="Pnl_lastInfo" TargetControlID="hiddenTargetControlForModalPopup2"  />
                          <asp:Panel ID="Pnl_lastInfo" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr>
            <td class="no"> <asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:larger">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr>
            <td class="o">
             </td>
            <td class="c" >
                Staff is created. Do you want to upload photo for the staff?
               
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:Button ID="Btn_Yes" runat="server" Text="Yes" class="btn btn-info" onclick="Btn_Yes_Click"/>
                            <asp:Button ID="Btn_No" runat="server" Text="No" class="btn btn-info"  OnClick="Btn_No_Click"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
                       
                        </asp:Panel> 
    </div>

  </ContentTemplate>
  </asp:UpdatePanel>
  
   
    <asp:Panel ID="Panel3" runat="server">
  
            
             <asp:Button runat="server" ID="hiddenTargetControlForMUploadImage" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_UploadPhoto" BackgroundCssClass="modalBackground"
                                  runat="server" 
                                  PopupControlID="Pnl_UploadImage" TargetControlID="hiddenTargetControlForMUploadImage"  />
                          <asp:Panel ID="Pnl_UploadImage" runat="server" style="display:none">
                         <div class="container skin5" style="width:600px; top:400px;left:400px">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr>
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/image_icon.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;">Upload photo</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr>
            <td class="o"> </td>
            <td class="c" >
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                <asp:Label ID="Lbl_UpMessage" runat="server" class="control-label" Text="" ForeColor="Red"></asp:Label>
                <asp:Panel ID="PannelUp" runat="server"><br />

<br />&nbsp;&nbsp;&nbsp;&nbsp;Upload Photo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload 
        ID="FileUp_User" runat="server" Height="25px" />
   <br />
   <br />
         <div style="text-align:center;">
          <asp:Button ID="Btn_Upload" runat="server" Text="Upload" class="btn btn-info" 
        onclick="Btn_UploadStaff_Click" />
             &nbsp;&nbsp;<asp:Button ID="Btn_UpCancel" runat="server" onclick="Btn_Cancel_Click" 
                 Text="Cancel" class="btn btn-info" />
                  &nbsp;&nbsp;
             <asp:Button ID="Btn_GoToStaff" runat="server" Text="View staff details" class="btn btn-info" Width="150px"  OnClick="Btn_GoToStaff_Click"/>
                        
                        </div>

</asp:Panel>
               
                        <br />
                       
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
             
</div>
                       
                        </asp:Panel>               
                     
    </asp:Panel>


                   
                   
                   
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

<div class="clear"></div>
</div>
</asp:Content>

