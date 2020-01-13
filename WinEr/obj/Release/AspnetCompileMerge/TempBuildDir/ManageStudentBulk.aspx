<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ManageStudentBulk.aspx.cs" Inherits="WinEr.ManageStudentBulk" %>
<%@ Register TagPrefix="WC" TagName="MANAGESTUDENT" Src="WebControls/ManageStudentControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>WinEr</title>
    <%--<link href="css files/wschoolstyle.css" rel="stylesheet" type="text/css" />--%>
    <link rel="shortcut icon" href="images/winerlogo.ico" />
    
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen,projection"/>
   <link rel='stylesheet' type='text/css' href='css files/winroundbox.css' title="style" media="screen"/>

  <%--<link href="css files/Orangestyle.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="css files/Orangemenu.css" />
 <script type='text/javascript' src="js files/winermenuOrange.js"></script>--%>

<script language="javascript" type="text/javascript">
    function PageRelorad() {

        window.opener.location.reload();


    }
    function CloseWindow() {

        window.close();

    }
  
</script>
    
</head><body>
    <form id="form1" runat="server">
  
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<div runat="server"   >
       <WC:MANAGESTUDENT id="WC_ManageStudent" runat="server"  />  
    </div>
    </form>
</body>
</html>
