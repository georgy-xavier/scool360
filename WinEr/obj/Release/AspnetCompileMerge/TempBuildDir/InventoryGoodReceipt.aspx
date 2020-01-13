<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryGoodReceipt.aspx.cs" Inherits="WinEr.InventoryGoodReceipt" %>
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
				<td class="n"><table width="100%"><tr><td>Purchase Items </td><td align="right"><div class="form-inline">	Select Location 
				
                    <asp:DropDownList ID="drp_location" runat="server" Width="150px" class="form-control" 
                        onselectedindexchanged="drp_location_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        </td></tr></table>
				
				</td>
				<td class="ne" ></td>
			</tr>
			<tr >
				<td class="o"></td>
				<td class="c" >				
				    <table width="100%">
				        <tr>
				        <td rowspan="3"  valign="top"> <WC:ITEMSELECTION id="WC_selectItem" runat="server" />  </td>
				        
				        <td valign="top"> 
				        
				   
                            
                            <table class="tablelist" width="100%">
                            <tr>
                            <td class="leftside">
                                 
				        Vendor Name</td><td  class="rightside"><asp:DropDownList ID="Drp_SelectVendor" class="form-control" runat="server" Width="150px"></asp:DropDownList>                    
                     <asp:LinkButton ID="Lnk_AddVendor" runat="server" 
                         onclick="Lnk_AddVendor_Click" >Add New</asp:LinkButton>
                            </td>
                            </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                              <tr>
				        <td class="leftside">
				            Date</td><td  class="rightside"><asp:TextBox ID="Txt_purchasedate" class="form-control" runat="server" Width="150px" ></asp:TextBox>
                            <cc1:MaskedEditExtender ID="Txt_SaleDate_MaskedEditExtender" runat="server" Enabled="True" TargetControlID="Txt_purchasedate"
                            MaskType="Date"  CultureName="en-GB" AutoComplete="true" Mask="99/99/9999"      UserDateFormat="DayMonthYear">    </cc1:MaskedEditExtender>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7"
                            ControlToValidate="Txt_purchasedate" ValidationGroup="sale"  Display="None"
                            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"  TargetControlID="RegularExpressionValidator7" HighlightCssClass="validatorCalloutHighlight" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ValidationGroup="sale"
                            ControlToValidate="Txt_purchasedate" ErrorMessage="Enter Date"></asp:RequiredFieldValidator>
                        </td>
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
				        <td  class="leftside">    Description </td><td  class="rightside"><asp:TextBox ID="txt_purchaseDescription" class="form-control" runat="server" Text =""  TextMode="MultiLine" Width="200px" MaxLength="250"> </asp:TextBox>
                            
				        </td>
				        </tr>
				        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

				        <tr>
				        <td  class="leftside">
				        Total Amount</td><td  class="rightside"><asp:TextBox runat="server" ID="Txt_PurchasingCost" class="form-control"
                                ForeColor="Gray" Enabled="false"></asp:TextBox>
				           </td></tr>
				       
				          
                            </table>
                            
                                                              
				        </td>
				        </tr>
				      
                    </table>
				
			
				<hr />
				
        <asp:Panel ID="pnl_ItemArea" runat="server">
       
                  <div style="min-height:250px">             
                               
				<asp:GridView ID="Grd_purchase" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True" AllowSorting="true"
                      PageSize="10" Width="100%" 
                   
                      onselectedindexchanged="Grd_purchase_SelectedIndexChanged" >
               <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                  
                    <asp:BoundField DataField="id" HeaderText="ItemId" HeaderStyle-HorizontalAlign="Center"  />   
                     <asp:BoundField DataField="Category" HeaderText="CategoryId" HeaderStyle-HorizontalAlign="Center"  />                         
                   <asp:BoundField DataField="ItemName" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="Categoryname" HeaderText="Category" HeaderStyle-HorizontalAlign="Center"/>
                     <asp:BoundField DataField="Count" HeaderText="Count" HeaderStyle-HorizontalAlign="Center"/>
               
                 <asp:BoundField DataField="Amount" HeaderText="Purchasing Cost" HeaderStyle-HorizontalAlign="Center"/>
           
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
				
				<td align="center">
				 <asp:Button ID="Btn_purchase" runat="server" Text="Purchase" 
                        class="btn btn-success" ValidationGroup="Sale"
                                Width="100px" onclick="Btn_purchase_Click" />
            <asp:Button ID="Btn_PurchaseCancel" runat="server" Text="Cancel" class="btn btn-danger" onclick="Btn_PurchaseCancel_Click" /> 
              
				</td>
				
				</tr>
				
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
   
   
<asp:Panel ID="Pnl_AddNewVendor" runat="server">
<asp:Button runat="server" ID="Btn_Addvendor" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_AddVendor"  runat="server" CancelControlID="Btn_VendorCancel"  
BackgroundCssClass="modalBackground"
PopupControlID="Pnl_AddVendor" TargetControlID="Btn_Addvendor"  />
<asp:Panel ID="Pnl_AddVendor" runat="server"  style="display:none;"><%--style="display:none;" --%>
                         <div class="container skin5" style="width:500px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">   </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="New Vendor"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Panel ID="Pnl_AddVendorNew" runat="server">
                <table width="100%" class="tablelist">                
                <tr>
                <td class="leftside">Vendor Name</td><td class="rightside"><asp:TextBox ID="Txt_VendorName" class="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="Req_Vendorname" runat="server" ControlToValidate="Txt_VendorName"
                ErrorMessage="Enter name" ValidationGroup="vendorSave"></asp:RequiredFieldValidator></td></tr><tr>
                <td class="leftside">City</td><td class="rightside"><asp:TextBox ID="Txt_City" class="form-control" runat="server"></asp:TextBox></tr><tr>
                <td class="leftside">Email Address</td><td class="rightside"><asp:TextBox ID="Txt_Email" class="form-control" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator   
            ID="RegularExpressionValidator8"  
            runat="server"   
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  
            ControlToValidate="Txt_Email"  
            ErrorMessage="Invalid email!"  ValidationGroup="vendorSave" > </asp:RegularExpressionValidator>  </td>
  
                                </tr>
                <tr>
                <td class="leftside">Mobile Number</td><td class="rightside"><asp:TextBox ID="Txt_MobileNumber" class="form-control" runat="server"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PhNo_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Custom, Numbers" 
                        ValidChars="+"  TargetControlID="Txt_MobileNumber">
                 </ajaxToolkit:FilteredTextBoxExtender>
                </tr>
                 <tr>
                <td class="leftside">Address</td><td class="rightside"><asp:TextBox ID="Txt_Address" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox></td></tr><tr>
                <td colspan="2" align="center">
                <asp:Label ID="Lbl_VendorErr" runat="server" ForeColor="Red"></asp:Label></td></tr></table></asp:Panel><div style="text-align:center;">
                             <asp:Button ID="Btn_VendorSave" runat="server" Text="Save" class="btn btn-success"  ValidationGroup="vendorSave"
                                 onclick="Btn_VendorSave_Click"/>
                            <asp:Button ID="Btn_VendorCancel" runat="server" Text="Cancel" class="btn btn-danger"
                                 onclick="Btn_VendorCancel_Click"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel> 
                        
    </ContentTemplate>
        </asp:UpdatePanel>
   
</div>

</asp:Content>
