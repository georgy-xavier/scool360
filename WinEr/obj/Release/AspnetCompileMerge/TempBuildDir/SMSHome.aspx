<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSHome.aspx.cs" Inherits="WinEr.SMSHome" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
		.Watermark
		{
			color:#999999;
			font-size:medium;
			vertical-align:bottom;
			text-align:center;
			font-family:Times New Roman;
		}
	   
	</style>
	<script type="text/javascript">
		function CheckDrp() {

		   var ss= document.getElementById('<%=Drp_Template.ClientID%>');
		   if (ss.value == "-1") {
			   return confirm('SMS send without selecting template may not be delivered to all numbers. Are you sure to continue?');
		   }
		   else {
			   return true;
		   }   
		   
		}
	
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
			</ajaxToolkit:ToolkitScriptManager>  
		   
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
	   <asp:Panel ID="Panel1" runat="server" >

	
		<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">SMS Home</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					 <table cellspacing="10"   width="100%">
					  <tr>
					  <td colspan="3" style="background-color:Black;color:White;height:30px;font-weight:bold">
					  
					   <marquee behavior="scroll" direction="left" scrollamount="5">
						 Please select template for sending sms. Sms without selecting template may not be delivered to all numbers. Before sending, replace symbol ($ $) parts with correct data in template.
					   </marquee>
					  </td>
					 </tr>
					 <tr> 
						<td align="center">
						<table align="center">
						<tr align="right">
						<td>
						   <asp:Label ID="Label2" runat="server" class="control-label col-lg-5" Text="Select Template:"></asp:Label>
							 <asp:DropDownList ID="Drp_Template" runat="server" class="form-control col-lg-7" Width="160px" AutoPostBack="true"
								onselectedindexchanged="Drp_Template_SelectedIndexChanged">
							</asp:DropDownList>
						</td>
						 <td >
						 <div class="radio radio-primary">
							   <asp:RadioButtonList ID="Rdb_CheckType" class="form-actions" runat="server" 
									 RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="20" 
									 AutoPostBack="true" 
									 onselectedindexchanged="Rdb_CheckType_SelectedIndexChanged" >
								  <asp:ListItem Text="All Staff" Value="0"></asp:ListItem> 
								  <asp:ListItem Text="Parent" Value="1" Selected="True"></asp:ListItem>
								  <asp:ListItem Text="Student" Value="2"></asp:ListItem>
								 </asp:RadioButtonList>
								 </div>
							 </td>
							 
							  <td align="right">
								<asp:Label ID="lbl_cls" runat="server" class="control-label" Text="Select Class Type:"></asp:Label>
								</td>
								<td align="left">
								<div class="radio radio-primary">
								<asp:RadioButtonList ID="RdBtLstSelectCtgry1" runat="server" class="form-actions"
									AutoPostBack="True"  RepeatDirection="Horizontal" CellSpacing="20"
									onselectedindexchanged="RdBtLstSelectCtgry1_SelectedIndexChanged">
									<asp:ListItem Selected="True">All</asp:ListItem>
									<asp:ListItem>only selected class</asp:ListItem>
									</asp:RadioButtonList>
									</div>
									 </td>
									</tr>
									</table>
								
								</td>
										  
					   </tr>
					   <tr >
						 <td align="center">
						 <table>
							<tr align="center">
							 <td style="width:700px;" align="center">
							 <asp:TextBox ID="Txt_Message" runat="server" TextMode="MultiLine" Width="600px" Height="125px" CssClass="txt_align"></asp:TextBox>
							<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
								   runat="server" Enabled="True" TargetControlID="Txt_Message" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
								  </ajaxToolkit:FilteredTextBoxExtender>
							 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Txt_Message"
									Display="Dynamic" ErrorMessage="<br>Please limit to 160 characters"
								   ValidationExpression="[\s\S]{1,299}"></asp:RegularExpressionValidator>
								   
							<ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Message_TextBoxWatermarkExtender"  WatermarkCssClass="Watermark"
									  runat="server" Enabled="True" WatermarkText="Enter The Message" TargetControlID="Txt_Message">
								  </ajaxToolkit:TextBoxWatermarkExtender>
								  </td>
								  <td align="left">
									 <asp:Panel ID="Panel_class" visible="false" runat="server" style="height:150px;Width:200px; overflow:auto;">
							   <asp:CheckBoxList ID="Chkb_class" runat="server" RepeatDirection="Vertical" 
											 Height="20px" >
							   </asp:CheckBoxList>
							   </asp:Panel>
								  </td>
								  </tr>
									  </table>
						 </td>

					   </tr>
					   <tr>
						 <td >
							<table>
							<tr>
							<td style="width:1400px;" align="center">
							   <asp:Button ID="btnConvert" runat="server" Class="btn btn-primary" 
								   onclick="btnConvert_Click" Text="Native Language " Visible="false" 
								   width="138px" />
						   </td>
						  <td>
						   </td>
						 </tr>
						   </table>
						   </td>
						   </tr>
						<tr id="ntvelng" runat="server">
						<td>
						<table>
							<tr>
						 <td style="width:1120px;" align="center" >
					
							 <asp:TextBox ID="txtNativelanguage" runat="server" TextMode="MultiLine" class="form-control" Width="600px" Height="125px"></asp:TextBox>
							  <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtendertxtNativelanguage"  WatermarkCssClass="Watermark"
									  runat="server" Enabled="True" WatermarkText="The Message in native language" TargetControlID="txtNativelanguage">
								  </ajaxToolkit:TextBoxWatermarkExtender>
						 </td>

					   </tr>
					   </table>
					   </td>
					   </tr>
					   
						<tr>
						 <td align="center" >
							 <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red" class="control-label" Font-Bold="true"></asp:Label>
							 &nbsp;  &nbsp;
							 <asp:LinkButton ID="Lnk_Retry" ToolTip="Retry SMS" ForeColor="Blue" Font-Size="12px" 
								 runat="server" onclick="Lnk_Retry_Click">Retry</asp:LinkButton>
						 </td>
					   </tr> 
					   <tr>
					   <td  align="center" >
		
						 
								   &nbsp;&nbsp;
								<asp:Button ID="Btn_Send"  runat="server" onclick="Btn_Send_Click"   Class="btn btn-success" OnClientClick="return CheckDrp()"
									 Text="Send"/>
							 
								 &nbsp;&nbsp;<asp:Button ID="Btn_CheckConnection" runat="server" Text="Check Connection"  Class="btn btn-primary" 
								 onclick="Btn_CheckConnection_Click"  />
								 
								  &nbsp;&nbsp;
								 
								 <asp:Button ID="Btn_Clear" runat="server" onclick="Btn_Clear_Click" 
									 Text="Clear"  Class="btn btn-danger"/>
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
		  
			 </asp:Panel>  
			 
		 


	<asp:Panel ID="Panel3" runat="server">
					   
   <asp:Button runat="server" ID="Button_main" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_Message"  runat="server" CancelControlID="Button_MainOk" 
								  PopupControlID="PanelMain" TargetControlID="Button_main"  BackgroundCssClass="modalBackground" />
   <asp:Panel ID="PanelMain" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:Black">Message</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			 <div style="font-weight:bold">
			 
			  <center>
				 <div id="DivMainMessage" runat="server">
				 
				 </div>
				</center>
			 
			 </div>
			   
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Button_MainOk" runat="server" Text="OK" Class="btn btn-info" Width="80px"/>
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
<div class="clear"></div>
</div>
</asp:Content>





