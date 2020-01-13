<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="ViewStudFeeDetails.aspx.cs" Inherits="WinEr.ViewStudFeeDetails"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">
		function preventBack() { window.history.forward(); }
		setTimeout("preventBack()", 0);
		window.onunload = function () { null };
		$(function () {
		    studentDetails.getSubMenu();
		    studentDetails.getTopDt();
		});
	 </script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></ajaxToolkit:ToolkitScriptManager>

	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
				<div class="card0">
					<div class="cardHd">Student Fee Details</div>
					<div class="row stntStripBody">
			<div class="_stdntTopStrip"></div>
	    </div>
				</div>
			</div>
			<br>
			 <div class="row">
				<div class="card0" >
					<table class="containerTable">
			<%--<tr >
				<td class="no"> </td>
				<td class="n">Student Fee Details</td>
				<td class="ne"> </td>
			</tr>--%>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
			  
				 <br />
			   <asp:Panel ID="Panel1" runat="server" BorderColor="Black" >
   
		  <ajaxToolkit:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_yuitabview-theme" 
					   Width="100%" ActiveTabIndex="1"  >
	   
	   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
				<HeaderTemplate>
					 <asp:Image ID="Image4" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/page_process2.png" /> <b>FEE TRANSACTIONS</b></HeaderTemplate>
						   <ContentTemplate>
						   
				 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
				<ContentTemplate>
						   
						   
							   <asp:Panel ID="Panel3" runat="server" style="min-height:400px">  
						  <br /> 
							<table style="width:100%" class="tablelist">
							
							<tr>
							 <td class="leftside">Select Batch :</td>
							 <td class="rightside">
								 <asp:DropDownList ID="Drp_Batch" runat="server" Width="180px"  AutoPostBack="True" class="form-control"
									 onselectedindexchanged="Drp_Batch_SelectedIndexChanged">
								 </asp:DropDownList>
							 </td>
							</tr>
						   
							<tr>
							 <td class="leftside">Select Type :</td>
							 <td class="rightside">
								 <asp:RadioButtonList ID="Rdb_FeeType" runat="server" AutoPostBack="True"
									 RepeatDirection="Horizontal"    
									 onselectedindexchanged="Rdb_FeeType_SelectedIndexChanged">
								 <asp:ListItem Selected="True" Value="0">ALL</asp:ListItem>
								  <asp:ListItem  Value="1">Regular Fee</asp:ListItem>
								   <asp:ListItem Value="2">Joining Fee</asp:ListItem>
								 </asp:RadioButtonList>
							 </td>
							</tr>
							<tr  align="right">
								<td colspan="2"> 
								<asp:ImageButton ID="ImgExportAll" runat="server"  OnClick="ImgExportAll_Click" ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px" />
							   </td>
						   </tr>
					  
						  </table>
						 <div class="linestyle"></div>
									  
					<asp:Label ID="Lbl_TransAllMsg" runat="server" class="control-label"></asp:Label>
				   <br/>                    
					
					<asp:GridView ID="Grd_TransactionsAll" runat="server" AllowPaging="True" 
					   AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
					   BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
					   GridLines="Vertical"        Width="100%"    PageSize="20" 
						onselectedindexchanged="Grd_TransactionsAll_SelectedIndexChanged" OnPageIndexChanging="Grd_TransactionsAll_PageIndexChanging" >
					   <Columns>
						 
					   
							  <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
						   <asp:BoundField DataField="Period" HeaderText="Period" />
						   <asp:BoundField DataField="BatchName" HeaderText="Batch" />
						   <asp:BoundField DataField="AccountType" HeaderText="Type" />
						   <asp:BoundField DataField="Amount" HeaderText="Amount" />
						   <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
						   <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
						   <asp:BoundField DataField="Type" HeaderText="Type" />
						   <asp:BoundField DataField="BatchId" HeaderText="Batch Id" />
							   
						   <asp:CommandField  HeaderText="View" SelectText="&lt;img src='pics/Details.png' height='30px' width='35px' border=0 title='Select bill to View'&gt;" 
							 ShowSelectButton="True" >                       
						   <ItemStyle Width="40px" />
						   </asp:CommandField>
					   </Columns>
					   
						<EditRowStyle Font-Size="Medium" />
					   
					  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
						<HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
							ForeColor="Black" HorizontalAlign="Left" />
						<PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
						<RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
							ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				   </asp:GridView>        
		
							   </asp:Panel>
							   
							   
			   </ContentTemplate>
				<Triggers>
				 <asp:PostBackTrigger ControlID="ImgExportAll" />
				 
				 </Triggers>
			   </asp:UpdatePanel>
						   </ContentTemplate>
		 </ajaxToolkit:TabPanel>
	  
	   <ajaxToolkit:TabPanel runat="server" ID="TabFeeToPay" HeaderText="Signature and Bio">
				<HeaderTemplate>
					 <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/dollar.png" /> <b>FEE TO PAY</b></HeaderTemplate>
						   <ContentTemplate>
							 <br />
							   <asp:Panel ID="Pnl_FeeToPay" runat="server" Height="400px" DefaultButton="Btn_feeexport">
												
					  <asp:UpdatePanel ID="UpdatePanel" runat="server">
					   <ContentTemplate>
					   
					  
			  
					   <table class="style1">
						<tr>
							<td >
								<asp:CheckBox ID="chkBoxAll" runat="server" AutoPostBack="True" 
									oncheckedchanged="chkBoxAll_CheckedChanged" Text="All Fee" /></td>
							<td >
								<asp:Button ID="Btn_feeexport" runat="server" onclick="Button1_Click" 
									Text="Export To Excel"  class="btn btn-primary"  /></td>
							<td align="center">
							  <b> Total Amount</b> 
							  <asp:TextBox ID="Txt_Amount" 
									runat="server" ReadOnly="True" BackColor="#FFFFCC" Font-Size="Medium" OnClientClick="return false" Width="100px" class="form-control"
									ForeColor="Black">0</asp:TextBox>&nbsp;&nbsp;&nbsp;
							</td>
						</tr>
					</table>
				   
				   <br/>
				   <asp:Label ID="Lbl_feeMessage" runat="server" Text="" class="control-label"></asp:Label>
					<asp:GridView  ID="Grd_Feetopay" runat="server" BackColor="White" 
						AutoGenerateColumns="False" 
						   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
						CellPadding="4" 
						   ForeColor="Black" GridLines="Vertical" Width="100% " AllowPaging="True" onpageindexchanging="Grd_Feetopay_PageIndexChanging" 
						>
						   
						   <Columns>
							   
						<asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
						<asp:BoundField DataField="BatchName" HeaderText="Batch" />
						<asp:BoundField DataField="Period" HeaderText="For Period" />
						<asp:BoundField DataField="Status" HeaderText="Status" />
						<asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" />
						
						<asp:BoundField DataField="LastDate" HeaderText="Last Date" />
								  
																					  
							</Columns>

					  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
				  <RowStyle BackColor="White"  BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
					   </asp:GridView>  
					   
						
					   </ContentTemplate>
					   <Triggers>
						<asp:PostBackTrigger ControlID="Btn_feeexport" />
					   </Triggers>
					  </asp:UpdatePanel>      
		
							   </asp:Panel>
						   </ContentTemplate>
		 </ajaxToolkit:TabPanel> 
			   <ajaxToolkit:TabPanel runat="server" ID="TabPanel_FeeAdvance" HeaderText="Fee Advance">
				<HeaderTemplate>
					 <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/currencygreen.png" /> <b>FEE ADVANCE</b></HeaderTemplate>
						   <ContentTemplate>
						   <asp:Panel ID="Panel2" runat="server" style="min-height:400px">  
						 
								<table width="100%"><tr>
		<td style="width:48px;">
	   <img alt="" src="Pics/dollar.png" width="45" height="45" /></td>
	<td><h3>Total Advance: 
		<asp:Label ID="Lbl_TotalAdvance" runat="server" Text="0" class="control-label"></asp:Label></h3></td>
	<td style="text-align:right;">
		<asp:ImageButton ID="Btn_exporttoAdvexel" runat="server"  Width="35px" 
			Height="35px" ToolTip="Export To Excel"
			  ImageUrl="Pics/Excel.png" onclick="Btn_exporttoAdvexel_Click" />
		 </td>
	</tr></table>
						 <div class="linestyle"></div>
									  
					<asp:Label ID="Label_AdvNote" runat="server" class="control-label"></asp:Label>
				   <br/>                    
					
					<asp:GridView ID="GridView_Advance" runat="server" 
					   AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
					   BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
					   GridLines="Vertical"  Width="100%"    
						 >
					   <Columns>
					   
							 <asp:BoundField DataField="FeeName" HeaderText="Fee Name" />
						   <asp:BoundField DataField="PeriodName" HeaderText="Period" />
						   <asp:BoundField DataField="BatchName" HeaderText="Batch" />
						   <asp:BoundField DataField="Amount" HeaderText="Amount" />
						
							   
					   </Columns>
					   
						<EditRowStyle Font-Size="Medium" />
					   
					  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
						<HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
							ForeColor="Black" HorizontalAlign="Left" />
						<PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
						<RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
							ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				   </asp:GridView>        
		
							   </asp:Panel>
							  
		 </ContentTemplate>
		 </ajaxToolkit:TabPanel> 
   </ajaxToolkit:TabContainer>
   
   
		   <br />
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
			 </div>
		  </div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>		  

  <WC:MSGBOX id="WC_MessageBox" runat="server" />      
</asp:Content>




<%--<ajaxToolkit:TabPanel runat="server" ID="TabPaidFee" HeaderText="Signature and Bio">
				<HeaderTemplate>
					 <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /> <b>FEE TRANSACTIONS</b></HeaderTemplate>
						   <ContentTemplate>
							 <br />
							   <asp:Panel ID="Pnl_PaidFee" runat="server" Height="400px">                                              
		
		
							<table style="width:100%">
							<tr>
							  <td >
								  From<span style="color:Red">*</span></td>
							  <td>
								  <asp:TextBox ID="Txt_from" runat="server"></asp:TextBox>
								
							  </td>
							  <td >
								  To<span style="color:Red">*</span></td>
							  <td>
								  <asp:TextBox ID="Txt_To" runat="server"></asp:TextBox>
								  <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender" runat="server" CssClass="cal_Theme1"
									  Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy">
								  </ajaxToolkit:CalendarExtender>
							  </td>
					  
								<td align="right"> 
								<asp:Button ID="Btn_Show" runat="server" Text="Show" OnClick="Btn_Show_Click" CssClass="graysearch" /> &nbsp;&nbsp;&nbsp;
						   
								<asp:ImageButton ID="Img_Export" runat="server" ImageAlign="AbsMiddle"  ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px" OnClick="Img_Export_Click"/>
							   </td>
						   </tr>
					  
						  </table>
						  
							<ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" CssClass="cal_Theme1"
									  Enabled="True" TargetControlID="Txt_from" Format="dd/MM/yyyy">
								  </ajaxToolkit:CalendarExtender>
							 <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
														runat="server" ControlToValidate="Txt_from" Display="None" 
														ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
														 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
														 />
							 <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
								TargetControlID="Txt_fromRegularExpressionValidator3"
								HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
				 
							  <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
														runat="server" ControlToValidate="Txt_To" Display="None" 
														ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
														 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
														 />
								  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
									  runat="server" HighlightCssClass="validatorCalloutHighlight" 
									  TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />
									  
					<asp:Label ID="Lbl_TransMessage" runat="server"></asp:Label>
				   <br/>                    
					
					<asp:GridView ID="Grd_Transactions" runat="server" AllowPaging="True"   onpageindexchanging="Grd_Transactions_PageIndexChanging"
					   AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
					   BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
					   GridLines="Vertical" onselectedindexchanged="Grd_Transactions_SelectedIndexChanged1" 
					   Width="100%"   AutoGenerateSelectButton="True" PageSize="20">
					   <Columns>
						   <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
						   <asp:BoundField DataField="Period" HeaderText="Period" />
						   <asp:BoundField DataField="BatchName" HeaderText="Batch" />
						   <asp:BoundField DataField="Accounttype" HeaderText="Type" />
						   <asp:BoundField DataField="Amount" HeaderText="Amount" />
						   <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
						   <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
						   
					   </Columns>
					   
					  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
				  <RowStyle BackColor="White"  Height="25px" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
				   </asp:GridView>        
		
							   </asp:Panel>
						   </ContentTemplate>
		 </ajaxToolkit:TabPanel>--%>
<%--<ajaxToolkit:TabPanel runat="server" ID="Tab_JoiningFee" HeaderText="Signature and Bio">
				<HeaderTemplate>
					 <asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /> <b>JOINING FEE</b></HeaderTemplate>
						   <ContentTemplate>
							 <br />
							   <asp:Panel ID="Panel2" runat="server" Height="400px">                                              
		
		
					<asp:Label ID="lbl_JoinMessage" runat="server"></asp:Label>
				   <br/>                    
					
					<asp:GridView ID="Grd_JoinFeeDetails" runat="server" AllowPaging="True"  
					   AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
					   BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
					   GridLines="Vertical" Width="100%"   AutoGenerateSelectButton="True" PageSize="20" 
									   OnPageIndexChanging="Grd_JoinFeeDetails_PageIndexChanging" 
									   OnSelectedIndexChanged="Grd_JoinFeeDetails_SelectedIndexChanged">
					   <Columns>
						   <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
						   <asp:BoundField DataField="BatchName" HeaderText="Batch" />
						   <asp:BoundField DataField="AccountType" HeaderText="Type" />
						   <asp:BoundField DataField="Amount" HeaderText="Amount" />
						   <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
						   <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
						   
					   </Columns>
					   
					  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
							HorizontalAlign="Left" />
				  <RowStyle BackColor="White"  Height="25px" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
				   </asp:GridView>        
		
							   </asp:Panel>
						   </ContentTemplate>
		 </ajaxToolkit:TabPanel>--%>