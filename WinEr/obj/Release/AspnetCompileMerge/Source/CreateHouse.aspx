<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateHouse.aspx.cs" Inherits="WinEr.CreateHouse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<div id="contents">
<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Create House</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<%--<table width="100%" class="tablelist">
					<tr>			       
					<td class="rightside">
					<asp:Image ID="Img_Add" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" />
					<asp:LinkButton ID="Lnk_AddNewHouse" runat="server" 
					 Font-Bold="true" Text="Add New House" 
						  > </asp:LinkButton>
					</td>
					<td class="leftside"></td>
					</tr>
				</table>--%>
				
			  <asp:Panel ID="Pnl_AddHouse" runat="server">			                     
			  <div class="roundbox">
				<table width="100%">
					<tr>
					<td class="leftside">House Name</td>
					<td class="rightside"><asp:TextBox ID="Txt_Housename" class="form-control" runat="server" Width="240px"></asp:TextBox></td>        		   
					</tr>
					<tr>
					 <td class="leftside"><br /></td>
					 <td class="rightside"><br /></td>
					 </tr>  
					<tr> 
					<td></td>
					<td align="left">
						<asp:Button ID="Btn_Add" runat="server" Text="Add" Width="90px" class="btn btn-primary" onclick="Btn_Add_Click" 
							/>
							<asp:Button ID="Btn_Update" runat="server" Text="Update" Width="90px" class="btn btn-primary" onclick="Btn_Update_Click" 
							/></td></tr>
					 <tr>           
						
					<td align="left" colspan="2"> 
					<asp:Label ID="Lbl_err" runat="server" class="control-label" ForeColor="Red"></asp:Label>
					<asp:HiddenField ID="Hdn_HouseId" runat="server" />
					</td></tr>
				</table>
			  </div>
			  </asp:Panel>
			  
			  <asp:Panel ID="Pnl_Housedisplay" runat="server">			 
				<div class="linestyle"></div>
				<table width="100%">
				<tr>   
				 <td align="center"><asp:Label ID="Lbl_House" runat="server" ForeColor="Red"></asp:Label>
				 </td>
				 </tr>
				<tr>
				<td align="center">
				<asp:GridView runat="server" ID="Grd_House" AutoGenerateColumns="false" BackColor="#EBEBEB"
						BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
					 CellPadding="3" CellSpacing="2" Font-Size="15px" Width="100%" 
						AllowPaging="true" PageSize="7" 
						OnRowDeleting="Grd_House_RowDeleting"
						OnRowDataBound="Grd_House_RowDataBound"
						OnPageIndexChanging="Grd_House_PageIndexChanging" 
						  onselectedindexchanged="Grd_House_SelectedIndexChanged" >
				
				  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <Columns>   
				  <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="360px" />              
				  <asp:BoundField DataField="HouseName" HeaderText="House Name"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>  
				  <asp:TemplateField HeaderText="Delete" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
				   <ItemTemplate>
					<asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
						</ItemTemplate><ControlStyle ForeColor="#FF3300" />
				   </asp:TemplateField>
						 <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" ControlStyle-Width="30px" 
						 ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller"
						  SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
							  ShowSelectButton="True">
							<ControlStyle />
									<ItemStyle Font-Bold="True" Font-Size="Smaller" />
						</asp:CommandField>
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

<div class="clear"></div>
</div>
 </ContentTemplate>
 <%--<Triggers>
 <asp:PostBackTrigger ControlID="btn_excel" />
 </Triggers>--%>
					</asp:UpdatePanel>
</asp:Content>
