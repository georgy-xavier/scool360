<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodReceiptReport.aspx.cs" Inherits="WinEr.GoodReceiptReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
    <div id="GoodsReport" runat="server"></div>
    <%--<table width="100%"><tr><td colspan="10" style="font-size:24px;text-align:center;height:40px;font-weight:bold">St.Francis ICSE School </td></tr><tr><td colspan="10" style="font-size:20px;text-align:center;height:35px;font-weight:bold">Hongasandra, Begur Road,
Bangalore-560 068.
PH.:080 -25731347
</td></tr></table>--%>
<%--<table border="1px" width="100%"><tr><td align="center"><b>GOODS RECEIPT REPORT</b>
</td></tr></table>--%>
<%--<table border="1px" width="100%"><tr><td colspan="2" align="left">
<b>Supplier Name:M&M</b></td><td align="left">Bill No:54</td><td align="right">Date:04/10/2011</td></tr>
<tr><td align="left"><b>Items</b></td><td align="center"><b>Purchasing Cost</b></td><td align="center">
<b>Count</b></td><td align="center"><b>Cost</b></td></tr><tr><td align="left">Tie</td>
<td align="center">25</td><td align="center">5</td><td align="center">125</td></tr>
<tr>
<td colspan="3"></td><td align="center"><b>Total Cost:125<b></td></tr></table>--%>
    </div>
    </form>
</body>
</html>
