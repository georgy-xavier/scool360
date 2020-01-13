<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="WinErParentLogin.ChangePassword" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
             <table class="tablelist">
                <tr>
                    <td class="leftside">
                        &nbsp;</td>
                    <td class="rightside">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="leftside">
                         Old Password</td>
                    <td class="rightside">
                        <asp:TextBox ID="TxtOldPwd" runat="server" TextMode="Password" class="form-control" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TxtOldPwd_ReqVal" runat="server" ErrorMessage="Enter old password" ValidationGroup="SaveDetails" ControlToValidate="TxtOldPwd"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                         New Password</td>
                    <td class="rightside">
                        <asp:TextBox ID="TxtNewPwd" runat="server" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="NewPassWordRegularExpressionValidator1"
                                ControlToValidate="TxtNewPwd"
                                Display="None"
                                ValidationExpression="^([1-zA-Z0-1@.\s]{1,255})$"
                                ErrorMessage="<b>Invalid Field</b><br />Password contains invalid characters" />
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
                        <asp:TextBox ID="TxtConfirmPwd" runat="server" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="ConfirmPassWordRegularExpressionValidator1"
                                ControlToValidate="TxtConfirmPwd"
                                Display="None"
                                ValidationExpression="^([1-zA-Z0-1@.\s]{1,255})$"
                                ErrorMessage="<b>Invalid Field</b><br />Password contains invalid characters" />
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
                        <asp:Button ID="BtnChngePwd" class="btn btn-primary" runat="server" ValidationGroup="SaveDetails" onclick="BtnChngePwd_Click"  Width="90px"
                            Text="Change"  /> 
                        <ajaxToolkit:ConfirmButtonExtender ID="BtnChngePwd_ConfirmButtonExtender"  ConfirmText="Are you sure you want to change the password?"
                            runat="server" Enabled="True" TargetControlID="BtnChngePwd">
                        </ajaxToolkit:ConfirmButtonExtender>
                        &nbsp; &nbsp; 

                            <input id="Reset1" type="reset" class="btn btn-danger" runat="server" title="Reset"  style="width:90px"/>
                    </td>
                </tr>
            </table>
            <WC:MSGBOX ID="MSGBOX" runat="server" />
</asp:Content>
