<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvItemSelectionControl.ascx.cs" Inherits="WinEr.WebControls.InvItemSelectionControl" %>
<link rel="stylesheet" type="text/css" href="css files/winbuttonstyleblue.css" title="style"  media="screen"/>
<link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>

<asp:Panel ID="Pnl_SelectItems" runat="server" >
			   
			   <asp:Panel ID="Pnl_items" runat="server">
				   
					<table  class="tablelist" width="100%">
				<tr><td  class="leftside">Item Name</td> 
					<td  class="rightside">
						<asp:TextBox ID="Txt_itemName" runat="server" Width="150px" AutoPostBack="true" class="form-control" OnTextChanged="ItemSelected_TextChanged"></asp:TextBox>
						 <asp:LinkButton ID="lnk_PickItem" runat="server" onclick="lnk_PickItem_Click">Pick Item</asp:LinkButton>
							<ajaxToolkit:AutoCompleteExtender ID="Txt_itemName_AutoCompleteExtender" runat="server"
							DelimiterCharacters="" Enabled="True" ServiceMethod="GetItemList" ServicePath="~/WinErWebService.asmx"
							UseContextKey="false" TargetControlID="Txt_itemName" MinimumPrefixLength="1"   OnClientItemSelected=""> </ajaxToolkit:AutoCompleteExtender>
					</td></tr>
					<tr id="Itemdetails" runat="server">
					<td colspan="2">
						<table class="tablelist" width="100%">
						<tr>
						<td  class="leftside">Item Name</td><td class="rightside"> <asp:Label ID="lblName" runat="server" Text=""></asp:Label></td>
							
						</tr>
						  <tr>
						<td  class="leftside">Category</td><td class="rightside"><asp:Label ID="lblCategory" runat="server" Text=""></asp:Label></td>
							
						</tr>
						   <tr>
						<td  class="leftside">Available Count</td><td class="rightside"><asp:Label ID="lblCount" runat="server" Text=""></asp:Label></td>
							
						</tr>
						</table>
						
					</td>
					
					</tr>
					<tr>
					<td colspan="2">
					<asp:Panel runat="server" DefaultButton="btn_Add">
					
					<table class="tablelist" width="100%">
					<tr  id="countrow" runat="server">
					  <td   class="leftside">
					   Count
					</td>
					  <td  class="rightside">
						<asp:TextBox ID="Txt_sale_count" runat="server" class="form-control" Width="150px" Text="0" OnTextChanged="Txt_sale_count_TextChanged" MaxLength="8"></asp:TextBox>
						<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="Txt_sale_count" FilterType="Numbers" FilterMode="ValidChars" ValidChars="0123456789"> </ajaxToolkit:FilteredTextBoxExtender>
					
					</td>
					</tr>
					 <tr id="AdjustmentArea" runat="server"> 
					<td colspan="2" >
					<table width="100%">
					  <tr>
					  <td   class="leftside">
					   Adjustment Mode
					</td>
					<td  class="rightside">
					  <asp:RadioButtonList ID="Rdb_Type" runat="server" RepeatDirection="Vertical" 
						AutoPostBack="True" 
						onselectedindexchanged="Rdb_Type_SelectedIndexChanged" >
				<asp:ListItem Text="Decrease" Value="1"></asp:ListItem>
				<asp:ListItem Text="Increase" Value="0"></asp:ListItem>
				
				</asp:RadioButtonList>
					</td>
					</tr>
					</table></td>
					</tr>
					</table>
					</asp:Panel>
					</td>
					</tr>
					
				   
					<tr>
				  
					 <td colspan="2" align="center">
					 <br /> 
						 <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
						
					 </td>
					</tr>
					<tr>
					<td colspan="2" align="center">
						<asp:Button ID="btn_Add" runat="server" Text="Add to list" onclick="btn_Add_Click" class="btn btn-primary" />
					</td>
					</tr>
				   
				
				</table>
			   </asp:Panel>
			
			   
			  <table><tr><td><asp:HiddenField ID="hdn_Needcount" runat="server" Value="0" /></td>
<td><asp:HiddenField ID="hdnCount" runat="server" Value="0"  /></td>
<td><asp:HiddenField ID="hdnItemId" runat="server" Value="0" /></td>
				 <td> <asp:HiddenField ID="hdncategory" runat="server" /></td><td> <asp:HiddenField ID="hnd_ispurchase" Value="0" runat="server" /><asp:HiddenField ID="hdn_isItemAdjustment" Value="0" runat="server" /></td></tr>
			  </table>
 

<asp:Panel ID="Pnl_BookDetails" runat="server">
 <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_SelectItem"  runat="server" CancelControlID="Img_Close"  BackgroundCssClass="modalBackground"
		  PopupControlID="Pnl_book" TargetControlID="Btn_hdnmessagetgt"   />
  <asp:Panel ID="Pnl_book" runat="server" style="display:none;" >
 <div class="container skin1" style="width:500px; height:450px;overflow:auto;top:400px;left:400px">
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no">
				  </td>
			<td class="n" align="right">
				<asp:ImageButton ID="Img_Close" runat="server"  ImageUrl="../images/cross.png"  Height="15px"  AlternateText="Close"/><asp:LinkButton ID="Lnk_Close" runat="server" Font-Size="12px">Close </asp:LinkButton>
			   </td>
			<td class="ne">
			 
		   
		   
			</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			  <asp:Panel ID="Panel1" runat="server">
					<table  class="tablelist" width="100%">
				<tr><td  class="leftside">Category Name</td> 
					<td  class="rightside">
						<asp:DropDownList ID="Drp_category" runat="server" Width="130px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_category_SelectedIndexChanged">
						</asp:DropDownList>
				   </td>
				   </tr>
				   
				   <tr><td  class="leftside">Item Name</td> 
					<td  class="rightside">
						<asp:DropDownList ID="Drp_ItemName" runat="server" Width="130px" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="Drp_ItemName_SelectedIndexChanged">
						</asp:DropDownList>
				   </td>
				   </tr>
				   
					<tr id="ItemdetailsPopup" runat="server">
					<td colspan="2">
						<table class="tablelist" width="100%">
						<tr>
						<td  class="leftside">Item Name</td><td class="rightside"> <asp:Label ID="lbl_popup_itemnName" runat="server" Text=""></asp:Label></td>
							
						</tr>
						  <tr>
						<td  class="leftside">Category</td><td class="rightside"><asp:Label ID="Lbl_popup_categorey" runat="server" Text=""></asp:Label></td>
							
						</tr>
						   <tr>
						<td  class="leftside">Available Count</td><td class="rightside"><asp:Label ID="lbl_popup_availablecount" runat="server" Text=""></asp:Label></td>
							
						</tr>
						</table>
						
					</td>
					
					</tr>
					<tr>
					<td colspan="2" align="center">
					<asp:Panel ID="pnl_count_adjustarea" runat="server" DefaultButton="Btn_AddItem">
					<table  width="100%">
					<tr  id="popupacountarea" runat="server">
					  <td   class="leftside">
					   Count
					</td>
					  <td  class="rightside">
						<asp:TextBox ID="txt_popupcount" runat="server" Width="133px" Text="0" class="form-control" OnTextChanged="Txt_sale_count_TextChanged"></asp:TextBox>
						<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="Txt_sale_count" FilterType="Numbers" FilterMode="ValidChars" ValidChars="0123456789"> </ajaxToolkit:FilteredTextBoxExtender>
					
					</td>
					</tr>
					<tr id="adjustmentPopuparea" runat="server"> 
					<td colspan="2" >
					<table width="100%">
					  <tr>
					  <td   class="leftside">
					   Adjustment Mode
					</td>
					<td  class="rightside">
					  <asp:RadioButtonList ID="rdoPopupAdjustment" runat="server" RepeatDirection="Vertical" 
						AutoPostBack="True" 
						onselectedindexchanged="rdoPopupAdjustment_SelectedIndexChanged" >
				<asp:ListItem Text="Decrease" Value="1"></asp:ListItem>
				<asp:ListItem Text="Increase" Value="0"></asp:ListItem>
				
				</asp:RadioButtonList>
					</td>
					</tr>
					</table></td>
					</tr>
					   <tr>
				  <td colspan="2" align="center">
						<asp:CheckBox ID="Chk_more" runat="server"   Text="Continue Adding"/>
					</td>
					</tr>
					
					</table>
					</asp:Panel>
					</td>
					</tr>
				   
				   
					<tr>
					 <td colspan="2" align="center"> <br />
						 <asp:Label ID="lbl_popuperror" runat="server" ForeColor="Red" Text=""></asp:Label>
					 </td>
					</tr>
					<tr>
					<td colspan="2" align="center">
						<asp:Button ID="Btn_AddItem" runat="server" Text="Add to list" class="btn btn-primary" onclick="Btn_AddItem_Click"  />
					</td>
					</tr>
				   
				
				</table>
			   </asp:Panel>
			</td>
			<td class="e"> </td>
		</tr>
		<tr>
			<td class="so"> </td>
			<td class="s"> </td>
			<td class="se"> </td>
		</tr>
	</table>                                                                    
	</div>
	
</asp:Panel>                 
					
	   </asp:Panel>                 
		 
		 </asp:Panel>           