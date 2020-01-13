<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentCancelEnrollment.aspx.cs" Inherits="WinEr.StudentCancelEnrollment" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
			
	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
				<div class="card0">
					<div class="cardHd">Cancel Enrollment</div>
					<div class="row stntStripBody">
							<div class="_stdntTopStrip"></div>
					</div>
				</div>
			</div>
		<br>
		<div class="row">
			<div class="card0" >
				<table class="containerTable">
				<%-- <tr >
					<td class="no"> </td>
					<td class="n">Cancel Enrollment</td>
					<td class="ne"> </td>
				</tr>--%>
				<tr >
					<td class="o"> </td>
					<td class="c" >
				
					<asp:Panel runat="server" ID="Pnl_DeleteArea">
						<table width="100%">
							<tr>
									<td style="width:180px" align="right">&nbsp;</td>
								<td style="width:350px">
									<asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" Text="" class="control-label"></asp:Label>
									</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td style="width:180px" align="right">&nbsp;</td>
								<td style="width:350px">
									<asp:Label ID="lbl_ErrorMsg" runat="server" ForeColor="Red" Text="" class="control-label"></asp:Label>
									</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
						
								<tr>
									<td align="right" style="width:180px">
										Enter Reason :</td>
									<td style="width:350px">
										<asp:TextBox ID="Txt_Reason" runat="server" MaxLength="250" 
											TextMode="MultiLine" ValidationGroup="Confirm" Width="198px" class="form-control"></asp:TextBox>
										<ajaxToolkit:FilteredTextBoxExtender ID="Txt_ReligionFilteredTextBoxExtender1" 
											runat="server" Enabled="True" FilterMode="InvalidChars" 
											InvalidChars="'/\#@!*$|^=" TargetControlID="Txt_Reason">
										</ajaxToolkit:FilteredTextBoxExtender>
										<asp:RequiredFieldValidator ID="Txt_ReasonRequiredFieldValidator6" 
											runat="server" ControlToValidate="Txt_Reason" ErrorMessage="Enter Reason" 
											ValidationGroup="Confirm"></asp:RequiredFieldValidator>
									</td>
									<td>
									</td>
									<td>
									</td>
							</tr>
						
								<tr>
								<td></td>
								<td align="left">
									<asp:Button ID="Btn_Delete" ValidationGroup="Confirm" runat="server"  Text="Delete" class="btn btn-danger" OnClick="Btn1_Delete_Click"/>
										<asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
										onclick="Btn_Cancel_Click" class="btn btn-primary" /></td>
								<td></td>
								<td></td>
							</tr>
						
						</table>
					</asp:Panel>
				   
				   
							<asp:Button ID="PopUp_Button" runat="server" class="btn btn-info" style="display:none;" />
						<%--<ajaxToolkit:ConfirmButtonExtender ID="Btn_Delete_ConfirmButtonExtender" 
							runat="server"  Enabled="True" TargetControlID="Btn_Delete" 
							DisplayModalPopupID="Btn_Save_ModalPopupExtender">
							</ajaxToolkit:ConfirmButtonExtender>--%>
							<%--<ajaxToolkit:ModalPopupExtender ID="Btn_Save_ModalPopupExtender" runat="server" TargetControlID="PopUp_Button"  PopupControlID="PNL1"  CancelControlID="ButtonCancel" />--%>
								<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1"   runat="server" CancelControlID="ButtonCancel" PopupControlID="PNL1" TargetControlID="PopUp_Button"  />

						<asp:Panel ID="PNL1" runat="server" style="display:none;width:350px;">
				   
						<div class="container skin5"  style="width:200">
						<table cellpadding="0" cellspacing="0" class="containerTable">
							<tr >
								<td class="no"> </td>
								<td class="n"><span style="color:White">Message</span></td>
								<td class="ne"> </td>
							</tr>
							<tr >
								<td class="o"> </td>
								<td class="c" >
								
										Are you sure You want to Cancel the Enrollment?
										<br /><br />
										<div style="text-align:center;">
											<asp:Button ID="ButtonOk" runat="server" Text="Yes" Width="50px" class="btn btn-success"
												onclick="Btn_Delete_Click" />
											<asp:Button ID="ButtonCancel" runat="server" Text="No" Width="50px" class="btn btn-danger" />
										</div>
								
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
		</div>
		</div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>		  
	<WC:MSGBOX id="WC_MessageBox" runat="server" />             
</asp:Content>
