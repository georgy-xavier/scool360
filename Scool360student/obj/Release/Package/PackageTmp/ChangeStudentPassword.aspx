<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="ChangeStudentPassword"  Codebehind="ChangeStudentPassword.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<asp:Panel ID="Panel1" runat="server" DefaultButton="BtnChngePwd">
 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/lock.png" 
                        Height="28px" Width="29px" /></td>
                <td class="n">
                Change Password
                        </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                   
             <table class="tablelist">
                <tr>
                    <td class="leftside">
                        &nbsp;</td>
                    <td class="rightside">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="leftside">
                        Enter Old Password</td>
                    <td class="rightside">
                        <asp:TextBox ID="TxtOldPwd" class="form-control" Width="260px" runat="server" TextMode="Password" Text=""></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                runat="server" Enabled="True" TargetControlID="TxtOldPwd" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                </ajaxToolkit:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="TxtOldPwd_ReqVal" runat="server" ErrorMessage="Enter old password" ValidationGroup="SaveDetails" ControlToValidate="TxtOldPwd"></asp:RequiredFieldValidator>
                                
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        Enter New Password</td>
                    <td class="rightside">
                        <asp:TextBox ID="TxtNewPwd" runat="server" Width="260px" class="form-control" TextMode="Password" Text=""></asp:TextBox>
                        
                        <asp:RegularExpressionValidator runat="server" ID="NewPassWordRegularExpressionValidator1"
                                ControlToValidate="TxtNewPwd"
                                Display="None"
                                ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="NewPassWordRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                                <asp:RequiredFieldValidator ID="TxtNewPwd_RequiredFieldValidator" runat="server" ErrorMessage="Enter new password" ValidationGroup="SaveDetails" ControlToValidate="TxtNewPwd"></asp:RequiredFieldValidator>
                        
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        Confirm Password</td>
                    <td class="rightside">
                        <asp:TextBox ID="TxtConfirmPwd" runat="server" Width="260px" class="form-control" TextMode="Password" Text=""></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="ConfirmPassWordRegularExpressionValidator1"
                                ControlToValidate="TxtConfirmPwd"
                                Display="None"
                                ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="ConfirmPassWordRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                        <asp:RequiredFieldValidator ID="TxtConfirmPwd_RequiredFieldValidator" runat="server" ErrorMessage="Confirm password" ValidationGroup="SaveDetails" ControlToValidate="TxtConfirmPwd"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Label ID="LblFailureNotice" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Button ID="BtnChngePwd" runat="server" class="btn btn-info" ValidationGroup="SaveDetails" onclick="BtnChngePwd_Click" 
                            Text="Change"  /> &nbsp; &nbsp; &nbsp;
                    <%--    <asp:Button ID="BtnCancel" runat="server" Text="Reset" 
                            onclick="BtnCancel_Click" CssClass="graycancel" />--%>
                            <input type="reset" runat="server" class="btn btn-danger" title="Reset"  />
                    </td>
                </tr>
            </table>
 <br />     <ajaxToolkit:ConfirmButtonExtender ID="BtnChngePwd_ConfirmButtonExtender" 
                            runat="server"  Enabled="True" TargetControlID="BtnChngePwd"
                            DisplayModalPopupID="BtnChngePwd_ModalPopupExtender">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <ajaxToolkit:ModalPopupExtender ID="BtnChngePwd_ModalPopupExtender" runat="server" TargetControlID="BtnChngePwd" PopupControlID="PNL" OkControlID="ButtonYes" BackgroundCssClass="modalBackground" CancelControlID="ButtonNo" />
                  <asp:Panel ID="PNL" runat="server" style="display:none;">
                        <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:large">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c"> 
                        Are you sure you want to change password ?
                        <br /><br />
                        <div style="text-align:right;">
                            <asp:Button ID="ButtonYes" class="btn btn-info" runat="server" Text="Yes" Width="50px" />
                            <asp:Button ID="ButtonNo" class="btn btn-info" runat="server" Text="No" Width="50px"/>
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
            <br />
             <br />
                   
                   
                   
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

</asp:Panel>
      <WC:MSGBOX id="WC_MessageBox" runat="server" />    

<div class="clear"></div>
</div>
</asp:Content>

