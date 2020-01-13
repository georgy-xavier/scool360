<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentPannel.aspx.cs" Inherits="WinEr.CommentPannel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title></title>
	<link href="css_bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
	<link href="css_bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" />
	<script src="js_bootstrap/bootstrap.js" type="text/javascript"></script>

	<script src="js_bootstrap/bootstrap.min.js" type="text/javascript"></script>
	
	<style type="text/css">
		
	.maindiv
	{
		border:gray 1px solid;
		-moz-border-radius: 5px;
		_border-radius: 5px;
		-ms-border-radius: 5px;
		ms-border-radius: 5px;
		-border-radius: 5px;
		border-radius:5px;
	}
	
	.mainhead
	{
		width:100%;
		height:20px;
		font-size:14px;
		font-weight:bold;
		text-align:left;
		background-color:Gray;
		color:White;
	}
	
	.line
	{
		width:100%;
		border:gray 1px solid;  
		height:1px;
	}
	.firstraw
	{
		
		height:auto;
		background-color:#eaeaea;
		color:#000 ;
	}
	.secondraw
	{
	   
		height:auto;
		background-color:white;
		color:Gray;
	}
	.userId
	{
		width:50px;
		vertical-align:top;
		text-align:center;
	  
		border:gray 1px solid;
	   
	}
	.time
	{
		width:100%;
		text-align:right;
		height:15px;
		font-size:12px;
		color:Black;
		
	}
	 
	.content
	{
		 width:100%;
		padding:5px;
	   text-align:left;
	} 
	.description
	{

		vertical-align:top;
		 
		text-align:left;
		height:auto;
		
	}
	.buttonalign
	{
		padding:10px;
	}
	
	#CommentDetails
	{
		max-height:290px;
		_height:290px;
		 height:290px;
		overflow:auto;
	}
	 .newComment
	{
		border:gray 1px solid;
		-moz-border-radius: 5px;
		_border-radius: 5px;
		-ms-border-radius: 5px;
		ms-border-radius: 5px;
		-border-radius: 5px;
		border-radius:5px;
	}
	
	</style>
	 <script type="text/javascript">

	  function pagereload() {
			window.opener.location.reload();
		}
	  </script>
	
</head>
<body>
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
  
	<div>
	<asp:Panel Id="Pnl_Details" runat="server">
	<div class="maindiv">
	  <asp:HiddenField ID="Hdn_thredId" runat="server" /> <asp:HiddenField ID="Hdn_Type" runat="server" /> <asp:HiddenField ID="Hdn_UsedId" runat="server" />
		<div class="mainhead"><asp:Label ID="Lbl_Heading" Text="" runat="server" ForeColor="White" Font-Bold="true"></asp:Label>
		  
		</div>
		<asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="red"></asp:Label>
		<div id="CommentDetails" runat="server">
	   
	   
	   
	 
		<div class="line" ></div>
		</div>
		
		<br />
		 </div>
	  
	</asp:Panel>
	<asp:Panel ID="Pnl_Err" runat="server">
	<asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red" ></asp:Label>
	</asp:Panel>
	<asp:Panel ID="Pnl_AddNew" runat="server">
	<div class="newComment" >
	  <table width="100%">
	  <tr>
	  <td>
	  <asp:TextBox ID="Txt_Messsage" Text="" TextMode="MultiLine" class="form-control" Width="450px" Height="70px" runat="server"></asp:TextBox>
	  </td>
	  <td>
	  <asp:Button ID="Btn_AddNew" runat="server" Text="Add Comment"  class="btn btn-primary"
					onclick="Btn_AddNew_Click"  /> 
	  </td>
	  </tr>
	  <tr>
	  <td colspan="2">
	   <asp:Label ID="Lbl_Comment" runat="server" Text="" ForeColor="Red" ></asp:Label>
	  </td>
	  </tr>
	  </table>
	</div>
	</asp:Panel>
	
	</div>
	 </ContentTemplate>
 </asp:UpdatePanel>
	
	</form>
</body>
</html>
