<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCertificate.aspx.cs" Inherits="WinEr.PrintCertificate" %>
<%@ PreviousPageType VirtualPath="~/CreateCertificates.aspx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
    body
    {
        font-family:Times New Roman Baltic;
    }
    
    .page
    {
        width:680px;
        margin:0 auto;
        position:relative;
        padding:10px;      
    }
    </style>
    
        <script language="javascript" type="text/javascript" >
        setTimeout("window.print()", 1000); 
        </script>
    
</head>
<body>
    <form id="form1" runat="server">
   

     <div id="DivData" runat="server">
      

      
  
    
    </div>
    </form>
</body>
</html>
