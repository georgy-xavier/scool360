<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="AbsDlyFeeReport.aspx.cs"  Inherits="WinEr.AbsDlyFeeReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
			</ajaxToolkit:ToolkitScriptManager>  
<div id="contents" style="min-height:300px;">

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
 <div class="container skin1">
		<table   cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Class-wise Fee Report</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"  > </td>
				<td class="c"  >
				
					 <center>
						   <table>
						   <tr>
						   <td>
						
						   </td></tr>
						   <tr>
							<td>
							From
							</td>
							<td>
									  <asp:TextBox ID="Txt_From" runat="server" class="form-control" Width="170px"></asp:TextBox>
										
							</td>
							<ajaxToolkit:CalendarExtender ID="Txt_From_CalendarExtender" runat="server" 
											Enabled="True" TargetControlID="Txt_From" CssClass="cal_Theme1" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
										<asp:RegularExpressionValidator ID="Txt_From_RegularExpressionValidator" 
										   runat="server" ControlToValidate="Txt_From" Display="None" 
										   ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
										  ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
										TargetControlID="Txt_From_RegularExpressionValidator"  HighlightCssClass="validatorCalloutHighlight" />
										 <asp:RequiredFieldValidator ID="Txt_From_ReqFieldValidator" runat="server" ControlToValidate="Txt_From" ErrorMessage="Enter From Date" ValidationGroup="Save"></asp:RequiredFieldValidator>
						   </tr>
							<tr>
					 <td class="leftside"><br /></td>
					 <td class="rightside"><br /></td>
					 </tr>
								<tr>
									<td>
										To
									</td>
									<td>
										<asp:TextBox ID="Txt_To" runat="server" class="form-control" Width="170px"></asp:TextBox>
									   
									</td>
									 <ajaxToolkit:CalendarExtender ID="Txt_Day_CalendarExtender" runat="server" 
											Enabled="True" TargetControlID="Txt_To" CssClass="cal_Theme1" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
										 <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator" 
										   runat="server" ControlToValidate="Txt_To" Display="None" 
										   ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
										  ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
										TargetControlID="Txt_To_DateRegularExpressionValidator"  HighlightCssClass="validatorCalloutHighlight" />
										<asp:RequiredFieldValidator ID="Txt_To_RequiredFieldValidator1" runat="server" ControlToValidate="Txt_To" ErrorMessage="Enter To Date" ValidationGroup="Save"></asp:RequiredFieldValidator>
								</tr>
								 <tr>
					 <td class="leftside"><br /></td>
					 <td class="rightside"><br /></td>
					 </tr>
								<tr>
								
									<td></td>
									<td align="left">
										   <asp:Button ID="Btn_Show" Text="Generate" Class="btn btn-primary"  runat="server"  ValidationGroup="Save"
											onclick="Btn_Show_Click"/>
									</td>
									<td><asp:ImageButton ID="Img_Export" runat="server" OnClick="Img_Export_Click"  ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px"/></td>
								</tr>
						   </table> 
						   
						  <div class="linestyle"></div>
						  <div style="text-align:center">
						   <asp:Label runat="server" ID="Lbl_Message" class="control-label"></asp:Label>
						  </div>
						   <asp:Panel ID="Pnl_AbsRpt" runat="server">
							<div style=" overflow:auto; max-height: 500px;">
						 <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
				<asp:GridView  ID="GridFees" runat="server" CellPadding="4" AutoGenerateColumns="true" 
				ForeColor="Black" GridLines="Vertical"      Width="100%"  BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"  BorderWidth="1px" >
				<RowStyle BackColor="#F7F7DE" />
				<FooterStyle BackColor="#CCCC99" />
				<PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
				<SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
				<HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
					HorizontalAlign="Left" />
				<AlternatingRowStyle BackColor="White" />
				
				 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
						 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
						 <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
						 HorizontalAlign="Left" />
						 <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
					   HorizontalAlign="Left" />                                          
					   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
					   <EditRowStyle Font-Size="Medium" />     
			</asp:GridView>
		  </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile">&nbsp;</td><td class=" bottomright"></td></tr>
		</table>
		</div>	      
				</div>
						   </asp:Panel>
					 </center>        
				</td>
				<td class="e"  > </td>
			</tr>
			<tr >
				<td class="so" > </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>
	 </ContentTemplate>
	  <Triggers>
<asp:PostBackTrigger ControlID="Img_Export"/>
</Triggers>
						
   </asp:UpdatePanel>
 <div class="clear"></div>
 
</div> 
   
</asp:Content>
