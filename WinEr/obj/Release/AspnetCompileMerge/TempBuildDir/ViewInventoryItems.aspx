<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ViewInventoryItems.aspx.cs" Inherits="WinEr.ViewInventoryItems" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">



<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
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
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <img alt="" src="Pics/Misc-Box.png" height="35" width="35" /> </td>
				<td class="n">View Inventory Items</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<br />
				<div style="min-height:200px">
				<table width="100%">
				<tr>
				<td align="center" >
				<asp:Label ID="Lbl_err" runat="server" ForeColor="Red"></asp:Label>
				</td>
				</tr>
				</table>
				<table width="100%">
				<tr><td align="center"><asp:Label ID="Lbl_Caption" runat="server" Font-Bold="true"></asp:Label></td></tr>
				</table>
				<br />
				
				<asp:Panel ID="Pnl_ViewInventoryItems" runat="server">
				<center>
				<table width="800px">
				<tr>
				<td>
				   <asp:GridView ID="Grd_Items" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true" 
                     PageSize="10" 
                       Width="800px" onpageindexchanging="Grd_Items_PageIndexChanging"   >
                   
                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                    <EditRowStyle Font-Size="Medium" />                           
                    <Columns>
                       <asp:BoundField DataField="Id" HeaderText="Id"/>
                       <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                       <asp:BoundField DataField="Description" HeaderText="Description"  />
                       <asp:BoundField DataField="Category" HeaderText="Category" />
                       <asp:BoundField DataField="Stock" HeaderText="Total stock"  />
                    </Columns>

                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                 </asp:GridView>
				</td>
				</tr>
				</table>
				</center>
				</asp:Panel>
				
				</div>
				
				 <WC:MSGBOX id="WC_MessageBox" runat="server" />  
					
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

<div class="clear"></div>

</div>


</asp:Content>
