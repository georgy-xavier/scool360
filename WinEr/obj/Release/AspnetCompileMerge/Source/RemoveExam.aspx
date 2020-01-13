<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="RemoveExam"  Codebehind="RemoveExam.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style type="text/css">

		.style1
		{
			width: 100%;
		}
		.style5
		{
			width: 14px;
		}
		.style2
		{
			width: 104px;
		}
		.style4
		{
		}
		.style3
		{
			width: 403px;
		}
		</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">

<div id="right">

<div class="label">Exam Info</div>
<div id="SubExammngMenu" runat="server">
		
 </div>
  </div>
 

<div id="left">

  
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Remove Exam</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"> </td>
				<td class="c" >
					
				<asp:Panel ID="Panel1" runat="server">
				<div id="topstrip">
				 <asp:Panel ID="Panel2" runat="server">
									<table style="width:100%;" >
										<tr>
											<td >&nbsp;</td>
											<td >
												<asp:Label ID="Lbl_ExamName" runat="server"  ForeColor="White" class="control-label"
													Text="University Exam"></asp:Label>
											</td>
											<td class="TblStrip2"> 
												<asp:Label ID="Lbl_Examtypelb" runat="server" ForeColor="White" class="control-label"
													Text="Exam Type"></asp:Label>
											</td>
											<td>
												<asp:Label ID="Lbl_ExamType" runat="server"  ForeColor="White" class="control-label"
													Text="MAIN"></asp:Label>
											</td>
										</tr>
										<tr>
											<td >&nbsp;</td>
											<td  >:</td>
											<td>
												<asp:Label ID="Lbl_freqlb" runat="server" class="control-label"
													Text="Exam Frequency" ForeColor="White"></asp:Label>
											</td>
											<td>
												<asp:Label ID="Lbl_Frequency" runat="server"  ForeColor="White" class="control-label"
													Text="Monthly"></asp:Label>
											</td>
										</tr>
										<tr>
											<td >&nbsp;</td>
											<td>&nbsp;</td>
											<td >&nbsp;</td>
											<td>&nbsp;</td>
										</tr>
									</table>
								</asp:Panel>
				 
				 
				 </div>
				
		<table class="style1" >
			
			<tr>
				<td class="style5">
					&nbsp;</td>
				<td class="style2">
					&nbsp;</td>
				<td class="style4">
					&nbsp;</td>
				<td class="style3">
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td class="style5">
					&nbsp;</td>
				<td class="style2">
					&nbsp;</td>
				<td class="style4">
					&nbsp;</td>
				<td class="style3">
					&nbsp;</td>
				<td>
					<asp:Button ID="btnremove" runat="server" onclick="btnremove_Click" 
						Text="Remove" Class="btn btn-danger" />
					<ajaxToolkit:ConfirmButtonExtender ID="BtnDeleteExam_ConfirmButtonExtender" 
						runat="server" DisplayModalPopupID="BtnDeleteExam_ModalPopupExtender" 
						Enabled="True" TargetControlID="btnremove">
					</ajaxToolkit:ConfirmButtonExtender>
					<ajaxToolkit:ModalPopupExtender ID="BtnDeleteExam_ModalPopupExtender" 
						runat="server" CancelControlID="ButtonCancel" OkControlID="ButtonOk" 
						PopupControlID="PNL" TargetControlID="btnremove" />
						
				</td>
			</tr>
			<tr>
				<td class="style5">
					&nbsp;</td>
				<td class="style2">
					&nbsp;</td>
				<td class="style4" colspan="3">
					&nbsp;</td>
			</tr>
		</table>
	</asp:Panel>	
				<asp:Panel ID="PNL" runat="server" style="display:none; ">
				
				
				<div class="container skin5" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:White;font-size:large">alert!</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
				Are you sure you want to Delete this Exam?
						<br /><br />
						<div style="text-align:center;">
							<asp:Button ID="ButtonOk" runat="server" class="btn btn-info" Text="Yes" Width="50px" />
							<asp:Button ID="ButtonCancel" runat="server" Text="No" class="btn btn-danger"  Width="50px" />
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

<div>
 <asp:Button runat="server" ID="hiddenTargetControlForModalPopup1" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_ExamMessage" 
								  runat="server" CancelControlID="Bn2_no" 
								  PopupControlID="Pnl_ExamMessage" TargetControlID="hiddenTargetControlForModalPopup1"  />
						  <asp:Panel ID="Pnl_ExamMessage" runat="server" style="display:none;">
						 <div class="container skinAlert" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" 
						Height="28px" Width="29px" /></td>
			<td class="n"><span style="color:White;font-size:large">Alert!</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
				<asp:Label ID="Lbl_altmessage" runat="server" Text="Label" class="control-label"></asp:Label> 

						<br /><br />
						<div style="text-align:center;">
						   
							<asp:Button ID="Bn2_no" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
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
   
</div>

<asp:Panel ID="Pnl_MessageBox" runat="server">
					   
						 <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
								  runat="server"  
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
						  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
						 <div class="container skin5" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:White">Message</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			   
				<asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
						<br /><br />
						<div style="text-align:center;">
						 <asp:Button ID="Btn_Finish" runat="server" Text="OK" Width="50px" class="btn btn-info" onclick="Btn_Finish_Click"/>   
							
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
	
	



 <br />
	</div>
<div class="clear"></div>
	   
</div>
</asp:Content>

