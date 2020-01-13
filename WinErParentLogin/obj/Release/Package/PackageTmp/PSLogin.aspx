<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSLogin.aspx.cs" Inherits="WinErParentLogin.PSLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css files/BlueLoginstyle.css" title="style" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="content">
            <div class="topheader" id="CollegeName">
                <img alt="" src="images/winlogo.gif" /> WINCERON SOFTWARE TECHNOLOGIES PVT.LTD.
            </div>  
            <div class="login">
                 <asp:Login ID="Client_Login" runat="server" Height="150px" 
                        onauthenticate="Client_Login_Authenticate" Width="275px" 
                        ForeColor="Black" >
                        <LoginButtonStyle Height="25px" Width="70px" />
                        <LayoutTemplate>
                            <table border="0" cellpadding="1" cellspacing="0" 
                                style="border-collapse:collapse;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" style="height:70px;width:400px;">
                                            
                                            <tr>
                                               
                                                <td>
                                                    <asp:TextBox ID="UserName" runat="server" CssClass="logintextbox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                        ToolTip="User Name is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                                </td>
                                            
                                          
                                                <td>
                                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="logintextbox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                        ControlToValidate="Password" ErrorMessage="Password is required." 
                                                        ToolTip="Password is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td >
                                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                                </td>
                                                <td align="left" style="padding-left:72px;">
                                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="loginbutton"   
                                                        ValidationGroup="Client_Login"  />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color:Red;">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                           
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <LabelStyle HorizontalAlign="Left" VerticalAlign="Bottom"  />
                        <TitleTextStyle Font-Bold="True" Font-Size="16px" ForeColor="White" HorizontalAlign="Left" />
                </asp:Login> 
            </div>
            
              <div class="footer">
  Powered By <a href="http://www.winceron.com/">Winceron Software Technologies </a>
  </div>
        </div>
    </div>
    </form>

            
</body>
</html>
