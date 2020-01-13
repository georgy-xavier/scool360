<%@ Page Language="C#" AutoEventWireup="True"   CodeBehind="MessageThreadAccdmics.aspx.cs" Inherits="WinEr.MessageThreadAccdmics" %>

<!DOCTYPE html>
<html lang="en-us">
	<head id="Head1" runat="server">
	  <meta charset="utf-8" content=""/>
	  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
	  <meta name="viewport" content="width=device-width, initial-scale=1"/>
	  <meta http-equiv="Page-Enter" content="blendTrans(Duration=0)"/>
	  <meta http-equiv="Page-Exit" content="blendTrans(Duration=0)"/>
	  <meta name="theme-color" content="#006be4"><!-- Chrome, Firefox OS and Opera -->
	  <meta name="msapplication-navbutton-color" content="#006be4"> <!-- Windows Phone -->
	  <meta name="apple-mobile-web-app-status-bar-style" content="#006be4"> <!-- iOS Safari -->
	  <meta name="description" content="Winer School management Sytem"/>
	  <meta name="author" content="TEAM WINCERON"/>
	  <title>MESSAGES</title>
	<link rel="shortcut icon" href="images/winerlogo.ico" />
	
	<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
	 <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/>
	 <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen,projection"/>
   <link rel='stylesheet' type='text/css' href='css files/winroundbox.css' title="style" media="screen"/>
		<script src="js%20files/jquery_v3.3.1.js"></script>
	<script src="js%20files/WINER_v5_Script.js"></script>
	<script src="js%20files/clienDataProcessor.js"></script>
		<script type="text/javascript">
			loadCounts.parentMsg();
			function pagereload(_msg) {
				if (_msg != '') {
					alert(_msg);
				}
				window.opener.location.reload();
			}
		function CloseWindow() {
			window.close();
		}
		function InvalidChars(e) {
			var txt = document.getElementById('txt_message');
			var KeyAscii = GetKeyCode(e);
			if (KeyAscii == 39 || KeyAscii == 47) {
				return false;
			}
			
		}
		function GetKeyCode(e) {
			var KeyAscii;
			if (window.event) // IE
			{
				KeyAscii = e.keyCode
			}
			else if (e.which) // Netscape/Firefox/Opera
			{
				KeyAscii = e.which
			}
			return KeyAscii;
		}
		</script>

	
</head><body>
	<form id="form1" runat="server">
  

<asp:ScriptManager ID="ScriptManager1" runat="server" />
 
   <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
	   
	   
	 <ProgressTemplate>
				
				
						<div id="progressBackgroundFilter">

						</div>

						<div id="processMessage">

						<table style="height:100%;width:100%" >

						<tr>

						<td align="center">

						<b>Please Wait...</b><br />

						<br />

						<img src="images/indicator-big.gif" alt=""/></td>

						</tr>

						</table>

						</div>
										
					  
				</ProgressTemplate>
	
	</asp:UpdateProgress>
	
	  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
	   <ContentTemplate>
	 <asp:Panel Width="100%" runat="server" ID="pnlMain" >
	 <table class="tablelist" >
	 <tr>
	 <td >
	 <div style="max-height:400px;overflow:auto"   >
	 <asp:GridView ID="grdThreads" EnableTheming="false" Width="100%" runat="server" 
			 AutoGenerateColumns="false"  >
	 <Columns>
	 <asp:BoundField DataField="Id"  />
	 <asp:BoundField DataField="FromUserId"  />
	 <asp:BoundField DataField="FromUSerType"  />
	 <asp:TemplateField>
	 <ItemTemplate>
	 <table width="100%" >
	 
	 <tr >
	 
	 <td style="width:50%" align="left" >
	 From : <%#Eval("Name")%>
	 </td>
	 <td align="left" >
	 <%#Eval("Date")%>
	 </td>
	 </tr>
	 <tr>
	 <td colspan="2" align="left" >     
	 <hr />
	 Subject : <%#Eval("Subject")%>
	 <hr />
	 </td>
	 </tr>
	 <tr>
	 <td colspan="2" align="left" >    
	 <div style="max-width:350px;min-height:80px;overflow:auto">
	 <%#Eval("Description")%>
	 </div> 
	 
	 </td>
	 </tr>
	 
	 
	 </table>
	 </ItemTemplate>
	 </asp:TemplateField>
	 </Columns>
	 </asp:GridView>
	 </div>
	 </td>
	 </tr>
  
	   <tr>
	 <td style="background-color:Silver" >
	 <table width="100%" >
	 <tr>
	 <td align="center" valign="middle" >
	 Subject : <asp:TextBox ID="txt_subj" runat="server" MaxLength="250" Width="450px" onkeypress="return InvalidChars(event)" ></asp:TextBox>
	  <asp:RequiredFieldValidator ID="Req_Vendorname" runat="server" ControlToValidate="txt_subj"
				ErrorMessage="*" ValidationGroup="ValidSend"></asp:RequiredFieldValidator>
 <%--<asp:RequiredFieldValidator ID="rqd_subj" runat="server"  
	  ControlToValidate="txt_subj" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>--%>
	  
		   
	 </td>
	 </tr>
	 <tr>
	 <td align="center" valign="top" >
	 
	 Message: <asp:TextBox ID="txt_message" TextMode="MultiLine" MaxLength="500" Height="80px" Width="450px"  onkeypress="return InvalidChars(event)" runat="server" ></asp:TextBox>
	 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_message"
				ErrorMessage="*" ValidationGroup="ValidSend"></asp:RequiredFieldValidator>
   <%--<asp:RequiredFieldValidator ID="rqd_msg" runat="server"
	  ControlToValidate="txt_message" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>--%>
											  
	 
									   
	 </td>
	 </tr>
	 <tr>
	 <td  align="center"  >
		<asp:Button ID="btn_msg" runat="server" Width="100px" Text="Send"  ValidationGroup="ValidSend" 
			 onclick="btn_msg_Click"  />
		&nbsp;&nbsp;
		<asp:Button ID="btn_cncl" runat="server" Width="100px" Text="Cancel" OnClientClick="CloseWindow()"  />
	 </td>
	 </tr> 
	 </table>     
	 </td>
	 </tr>

	 
	 </table>
	 </asp:Panel>
<%--
			<WC:MSGBOX ID="MSGBOX" runat="server" />--%>
</ContentTemplate>
</asp:UpdatePanel>

	</form>
				   

</body>
</html>
