<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorRedirect.aspx.cs" Inherits="WinEr.ErrorRedirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <br />
    
    <br />
    <center>
    
       
      <asp:Label ID="Label4" runat="server" Text="You cannot use this page without creating any class" Font-Bold="true" Font-Size="14"></asp:Label>
     <br />
    
    <br />
    
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" >Go Back</asp:LinkButton>
    </center>
    <asp:HiddenField ID="Hd_URL" runat="server" />
    </div>
    </form>
</body>
</html>
