<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MessageInbox.aspx.cs" Inherits="WinErAcademics.MessageInbox" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	   <script language="javascript" type="text/javascript">
	       loadCounts.parentMsg();
	       function openIncpopup(strOpen) {
	           window.open(strOpen, '_blank');
			  // open(strOpen, "Info", "status=1, width=700, scrollbars = 0,  height=600,resizable = 1");
		   }
</script>

	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="ConfigMenu" runat="server">

</div>

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
<%--    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
--%>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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

 <div class="container skin1" >
		<table   cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/folder_accept.png" 
						Height="29px" Width="29px" /></td>
				<td class="n">
					Inbox</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				   <div style="min-height:300px;">
				   
				   <table class="tablelist">
				<tr>
				<td style="width:20px;" valign="bottom" >
				<img alt="" src="Pics/full_page.png" height="18" width="18" />
				</td>
				<td valign="bottom" >
					<asp:Label ID="Lbl_ApprovellistCount" runat="server" Text="" 
						ForeColor="#FF9900"></asp:Label>
				</td>
				
				<td style="text-align:right;">
			
									
				</td>
				</tr>
				
				<tr>
				<td colspan="3" align="center" >
					
					
					</td>

				</tr>
				</table>
				<div class="linestyle"></div>
					   <asp:Label ID="Lbl_Note" runat="server" Text=""></asp:Label>
					<asp:GridView ID="GrdMessage"  runat="server" AutoGenerateColumns="False" 
						AllowPaging="true"
						Width="100%" PageSize="25" 
						onpageindexchanging="GrdMessage_PageIndexChanging" 
						onselectedindexchanged="GrdMessage_SelectedIndexChanged"   
						OnRowDataBound="GrdMessage_RowDataBound" >
						<PagerSettings Position="TopAndBottom" />
		
		<Columns>
		 
		   

					  
				<asp:BoundField DataField="ThreadId" />

		   
			  <asp:BoundField DataField="Name" HeaderText="Student" />
			  <asp:BoundField DataField="Subject" HeaderText="Subject" />
			  <asp:BoundField DataField="Description" HeaderText="Last Message" ItemStyle-Wrap="true" />
			  
			  <asp:BoundField DataField="Date" HeaderText="Date" />
			   
			  <asp:BoundField DataField="readStatus" HeaderText="Status" />
			 
	  
						
			   <asp:CommandField HeaderText="View Details"  SelectText="&lt;img src='Pics/hand.png' width='25px' Hight='25px' border=0 title='View Details'&gt;"  
							ShowSelectButton="True" >
							<ItemStyle Width="30px" />
							</asp:CommandField>
						 
   
							
			  
		</Columns>
	  
	</asp:GridView>
				   
				   </div>
		   
				   
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
<WC:MSGBOX id="WC_MessageBox" runat="server" />

 


 

 </ContentTemplate>
 </asp:UpdatePanel>     
</asp:Content>

