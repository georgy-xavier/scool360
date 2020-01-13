<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventorySalesItems.aspx.cs" Inherits="WinEr.InventorySalesItems" %>
<%@ Register TagPrefix="WC" TagName="ITEMSELECTION" Src="~/WebControls/InvItemSelectionControl.ascx"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">
 function openIncpopup(strOpen) {
         open(strOpen, "Info", "status=1, width=600, height=400,resizable = 1");
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="left">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
<ProgressTemplate><div id="progressBackgroundFilter"> </div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div> </ProgressTemplate></asp:UpdateProgress>

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate> 
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n"><table width="100%"><tr><td>Sale Items </td><td align="right"><div class="form-inline">	Select Location 
                    <asp:DropDownList ID="drp_location" runat="server" Width="150px" class="form-control"
                        onselectedindexchanged="drp_location_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></div></td></tr></table>
				</td>
				<td class="ne" ></td>
			</tr>
			<tr >
				<td class="o"></td>
				<td class="c" >				
				    <table width="100%">
				        <tr>
				        <td rowspan="3"> <WC:ITEMSELECTION id="WC_selectItem" runat="server" />  </td>
				        
				        <td> 
				        
				   
                            
                            <table class="tablelist" width="100%">
                            <tr>
                            <td class="leftside">
                                 
				        Customer Name</td><td  class="rightside"><asp:TextBox ID="Txt_Customer" runat="server" Width="150px" class="form-control"></asp:TextBox>				
        				    <asp:RequiredFieldValidator ID="Req_Customer" runat="server" ValidationGroup="sale"
                            ControlToValidate="Txt_Customer" ErrorMessage="Enter value"></asp:RequiredFieldValidator>                                     
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                            runat="server" Enabled="True" FilterType="Custom" 
                            FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\" 
                            TargetControlID="Txt_Customer">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            </tr>
                              <tr>
				        <td class="leftside">
				            Date</td><td  class="rightside"><asp:TextBox ID="Txt_SaleDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="Txt_SaleDate_MaskedEditExtender" runat="server" Enabled="True" TargetControlID="Txt_SaleDate"
                            MaskType="Date"  CultureName="en-GB" AutoComplete="true" Mask="99/99/9999"      UserDateFormat="DayMonthYear">    </cc1:MaskedEditExtender>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7"
                            ControlToValidate="Txt_SaleDate" ValidationGroup="sale"  Display="None"
                            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"  TargetControlID="RegularExpressionValidator7" HighlightCssClass="validatorCalloutHighlight" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ValidationGroup="sale"
                            ControlToValidate="Txt_SaleDate" ErrorMessage="Enter Date"></asp:RequiredFieldValidator>
                        </td>
                        </tr>
                        <tr>
				        <td  class="leftside">    Description </td><td  class="rightside"><asp:TextBox ID="txt_saleDescription" class="form-control" runat="server" Text =""  TextMode="MultiLine" Width="150px" MaxLength="250"> </asp:TextBox>
                            
				        </td>
				        </tr>
				        <tr>
				        <td  class="leftside">Total Amount
				        </td><td  class="rightside">
				        <asp:TextBox runat="server" ID="Txt_saletotalAmnt" Width="150px" class="form-control"
                                ForeColor="Gray" Enabled="false"></asp:TextBox>
				        </td>
				        </tr>
				     
                            </table>
                            
                                                              
				        </td>
				        </tr>
				      
                    </table>
				
			
				<hr />
        <asp:Panel ID="pnl_ItemArea" runat="server">
        <div style="min-height:250px">
				<asp:GridView ID="Grd_saleitem" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True" AllowSorting="true"
                      PageSize="10" Width="100%" 
                   
                      onselectedindexchanged="Grd_saleitem_SelectedIndexChanged" >
               <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                  
                    <asp:BoundField DataField="id" HeaderText="ItemId" HeaderStyle-HorizontalAlign="Center"  />   
                     <asp:BoundField DataField="Category" HeaderText="CategoryId" HeaderStyle-HorizontalAlign="Center"  />                         
                   <asp:BoundField DataField="ItemName" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="Categoryname" HeaderText="Category" HeaderStyle-HorizontalAlign="Center"/>
                   <asp:BoundField DataField="Stock" HeaderText="Available stock" HeaderStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="Count" HeaderText="Count" HeaderStyle-HorizontalAlign="Center"/>
           
                 <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"/>
           
                 <asp:BoundField DataField="Total" HeaderText="Total" HeaderStyle-HorizontalAlign="Center"/>
               
     
                                         <asp:CommandField SelectText="&lt;img src='Pics/DeleteRed.png' width='30px' border=0 title='Remove'&gt;" 
                           ShowSelectButton="True" HeaderText="Remove"  ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center"  >
                        </asp:CommandField>
                   
                   </Columns>       
                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />            
            
            </asp:GridView>
     
				  <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
				  </div>
				<table width="100%">
				   <tr>
				        <td colspan="2" align="center">
				            <asp:Button ID="Btn_Salesave" runat="server" Text="Sale" 
                        class="btn btn-success" ValidationGroup="Sale"
                                Width="100px" onclick="Btn_Salesave_Click" />
            <asp:Button ID="Btn_saleCancel" runat="server" Text="Cancel" class="btn btn-danger" 
                                 onclick="Btn_saleCancel_Click" /></td></tr>
				       
				         
				</table>
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
   
   

    </ContentTemplate>
        </asp:UpdatePanel>
   
</div>
</asp:Content>
