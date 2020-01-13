<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaySlip.aspx.cs" Inherits="WinEr.PaySlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
            border-collapse: collapse;
            border: 1px solid #000000;
        }
    </style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>

   <div id="Div_Payslip" runat="server">  
   </div>

    </div>
    </form>
   
</body>
</html>
