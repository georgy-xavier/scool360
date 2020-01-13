<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdminLogin" Codebehind="AdminLogin.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="css files/adminstyle.css" media="screen"/>
</head>
<body style="text-align:center">
   <div style="text-align:center ">
<form id="form1" runat="server" style="text-align:center">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  
        <div style="width:100%">
        <center>
        <br />
        <asp:UpdatePanel ID="AdminLoginPanal" runat="server">
            <ContentTemplate>
            <asp:Login ID="Admin_Login" runat="server" BackColor="#404040" BorderColor="Silver" BorderStyle="Solid"
                BorderWidth="1px" Font-Names="Verdana" Font-Size="10pt" Height="145px" OnAuthenticate="Login1_Authenticate" Width="328px" ForeColor="White">
                <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <LayoutTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" style="width: 328px; height: 145px">
                                    <tr>
                                        <td align="center" colspan="2" style="font-weight: bold; color: black; background-color: #999999">
                                            Administrator
                                            Log In</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" ForeColor="White">User Name:</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Admin_Login">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="160px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Admin_Login">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                        </td>
                                    </tr>
                                    <tr>
                                    
                                        <td align="center" colspan="2" style="color: red">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2" style="height: 25px">
                                            &nbsp;<asp:Button ID="LoginButton"
                                                runat="server" CommandName="Login" Text="Log In" ValidationGroup="Admin_Login" Width="82px" OnClick="LoginButton_Click" />
                                            &nbsp;<asp:Button ID="Cmd_Cancel" runat="server" OnClick="Cmd_Cancel_Click1" Text="Cancel"
                                                Width="83px" />&nbsp;
                                                 
                                        </td>
                                       
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:Login>
            
           </ContentTemplate>                          
      </asp:UpdatePanel>
      </center>
    </div>
    

    
    </form>
</div>
</body>
</html>
