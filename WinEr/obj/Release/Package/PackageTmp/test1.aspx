<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="SalesTracker.test1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    
<%--    <script type="text/javascript">
    var myVar=setInterval(function(){myTimer()},1000);
    function myTimer()
    {
        alert("Hai");
    }
    </script>--%>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>

        
           <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <!-- UpdateUpanel let the progress can be updated without updating the whole page (partial update). -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <center>
         <br />
         <br />
         <br />
         <br />
         <br />
             <asp:Label ID="lbProgress" runat="server" Text="please wait." Font-Bold="true" Font-Size="Large"></asp:Label><br />
         </center>     
        <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="1000" 
            ontick="Timer1_Tick">
        </asp:Timer>
    
    
        </ContentTemplate>

    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
