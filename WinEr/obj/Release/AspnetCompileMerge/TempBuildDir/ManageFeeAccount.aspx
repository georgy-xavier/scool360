<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ManageFeeAccount.aspx.cs" Inherits="WinEr.WebForm2"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
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
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
					<img alt="" src="Pics/Manage.png" width="35px" /></td>
				<td class="n">Manage Fee</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					
			  <asp:Panel ID="Pnl_FeeList" runat="server"  >
	   <div style="min-height:450px;">     
		
		<asp:GridView ID="Grd_FeeList" runat="server" AllowPaging="True" AutoGenerateColumns="False"  onpageindexchanging="Grd_ExamList_PageIndexChanging" 
		OnRowDataBound ="Grd_ExamList_RowDataBound" 
		Width="100%" OnRowCommand="Grd_FeeList_Header_RowCommand"
		BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px" PageSize="20">
	   
		<Columns>
		<%--    <asp:CommandField  SelectText="&lt;img src='Pics/currency_dollargreen.png' width='30px' border=0 title='View Fee Details'&gt;"  
							ShowSelectButton="True" >
							<ItemStyle Width="35px" />
							</asp:CommandField>--%>
							 <asp:buttonfield buttontype="Link" commandname="View"  text="&lt;img src='Pics/currency_dollargreen.png' width='30px' border=0 title='View Fee Details'&gt;" 
								 ItemStyle-ForeColor="Black"  HeaderText="Select"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
								<ItemStyle ForeColor="Black" HorizontalAlign="Center" />
								</asp:buttonfield>
			<asp:BoundField DataField="Id" HeaderText="Fee Id" />
			<asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
			 <asp:TemplateField HeaderText="Description">
				<ItemTemplate>
				<div runat="server" id="divdisc" style="Width:300px;max-height:50px;overflow:auto;">
				<%# Eval("Desciptrion")%>
				</div>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="FreequencyName" HeaderText="Frequency" ItemStyle-Width="100px" />
			<asp:BoundField DataField="Name" HeaderText="Associated To" ItemStyle-Width="100px" />
			<asp:BoundField DataField="FeeType" HeaderText="Fee Type" ItemStyle-Width="100px" />
			<asp:TemplateField  HeaderText="Fees Header" Visible="false">
			<ItemTemplate>
		   <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("Id") %>'
			CommandName="select" Text='<%#Eval("Header")%>' Font-Size="Medium"></asp:LinkButton>
			</ItemTemplate>
			</asp:TemplateField>
		  </Columns>
		  <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
				  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
				  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
		 </asp:GridView>
				  <asp:Label ID="Lbl_Note" runat="server" class="control-label" ForeColor="Red"></asp:Label>
			 <br />
	   </div> 
		</asp:Panel>
	   
	   <br />
			 <br />
					
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



<asp:Button runat="server" ID="Btn_PopUp" class="btn btn-info" style="display:none"/>
							<ajaxToolkit:ModalPopupExtender ID="MPE_FeesGroupPopUp"   runat="server" CancelControlID="Btn_Cancel" 
							PopupControlID="Pnl_FeesGroupPopUp" TargetControlID="Btn_PopUp" BackgroundCssClass="modalBackground" />
								<asp:Panel ID="Pnl_FeesGroupPopUp" runat="server" style="display:none">
									<div class="container skin5" style="width:700px; top:400px;left:200px" >
										<table   cellpadding="0" cellspacing="0" class="containerTable">
											<tr >
												 <td class="no"><asp:Image ID="Image1" runat="server" 
														 ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" />
												 </td>
												 <td class="n"><span style="color:White">Map Fees Group Header</span></td>
												 <td class="ne">&nbsp;</td>
											 </tr>
											 <tr >
												  <td class="o"> </td>
												  <td class="c" >             
												   <asp:Panel ID="Pnl_edt_fees_group" runat="server">
					  <table class="tablelist">
					   <tr>
							  <td class="leftside">
								  &nbsp;</td>
							  <td class="rightside">
								  &nbsp;</td>
						  </tr>
						 <tr>
							  <td class="leftside">
								   Fees Name :</td>
							  <td class="rightside">
								  <asp:Label ID="Lbl_Fees" runat="server" Text="" class="control-label" ForeColor="Blue"></asp:Label></td>
						  </tr>
							<tr>
							  <td class="leftside">
								  &nbsp;</td>
							  <td class="rightside">
								  &nbsp;</td>
						  </tr>
					  <tr><td class="leftside">
					   Fees Group Name <span style="color:Red;">*</span>
					  </td>
					  <td class="rightside">
						  <asp:DropDownList ID="Drplist_Header" class="form-control" runat="server" Width="200px">
						  </asp:DropDownList>
								
													
					  </td>
					  </tr>
					<tr>
							  <td class="leftside">
								  &nbsp;</td>
							  <td class="rightside">
								  &nbsp;</td>
						  </tr>
					  <tr>
					  <td class="leftside"></td>
					  <td class="rightside">
						<asp:Button ID="Btn_Update" runat="server" Text="Save" Class="btn btn-info"
									  onclick="Btn_Update_Click"  />
								  &nbsp;&nbsp; 
						<asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
								   onclick="Btn_Clear_Click" Class="btn btn-info"/>
								  
					  </td>
					  </tr>
	
					  </table>
					  <center>
					  <br/>
					  <asp:Label ID="Lbl_Edt_Msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
					  <br />
					  </center>
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
										 <br /><br />                                                
									</div>
							 </asp:Panel>
			  
		
  
	  <WC:MSGBOX id="WC_MessageBox" runat="server" />      


		
			
	   
	 </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
