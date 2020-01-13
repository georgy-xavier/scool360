<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" CodeBehind="UserArea.aspx.cs" Inherits="WinEr.WebForm23" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.style1
		{
			width: 185px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
	 <div id="contents">
		 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
			</ajaxToolkit:ToolkitScriptManager>  
	   <div id ="User1" runat="server"  style="min-height:450px">
	
	<div class="container skin1" style="min-height:400px;">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">User Area</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<table width="100%"  >
			<tr>
				<td class="style1" >
				
				<!-- Div for photo !-->
				
				 <div class="container skin6" style="width:190px" >
		<table   cellpadding="0" cellspacing="0" class="containerTable">
		
			<tr >
				<td class="o"> </td>
				<td class="c" >
				   
					  <asp:Panel ID="Panel5" runat="server">
					  
						  <asp:Image ID="Img_Holder" runat="server" BorderWidth="0px"  Height="150px" Width="150px" ImageUrl="~/images/user.png"/>  
						</asp:Panel>
				   
				   
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>

				<!-- End of photo div !-->
				 
				</td>
				<td>
					<div id ="PersonData" runat="server">
						
					</div>
				</td>

			</tr>
			</table>
				
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		 </table>
		 </div>
	  
	   </div>
  </div>      
		
   
<div class="clear"></div>     
		
		
	</div>
</asp:Content>
