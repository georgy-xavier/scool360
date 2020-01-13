<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudDivTransfer.aspx.cs" Inherits="WinEr.StudDivTransfer" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.InfoBorder {
			border: solid 1px #c4c4c4;
			width: 99%;
			margin-top: 5px;
			max-height: 500px;
			overflow: auto;
		}


		.ModuleHeading {
			padding: 15px 0px 0px 10px;
			font-size: 13px;
			font-weight: bold;
		}

		.Itemsleft {
			font-size: 11px;
			font-weight: bold;
			font-style: italic;
			border-bottom: solid 1px #eee;
			padding-left: 5px;
			width: 70%;
		}


		.Itemsright {
			font-size: 12px;
			font-weight: bolder;
			font-style: italic;
			border-bottom: solid 1px #eee;
			padding-right: 10px;
			width: 30%;
		}

		.trstyle {
		}
	</style>
	 <script type="text/javascript">
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
		            <div class="cardHd">Change Student Class</div>
		            <%-- student top div --%>
		            <div class="row stntStripBody">
			            <div class="_stdntTopStrip"></div>
		            </div>
	            </div>
		    </div>
			<br> 
			<div class="row"> 
			<div class="card0">
				<table class="containerTable">
				   <%-- <tr>
						<td class="no"></td>
						<td class="n">Change Class</td>
						<td class="ne"></td>
					</tr>--%>
					<tr>
						<td class="o"></td>
						<td class="c">


							<div style="min-height: 300px">

								<asp:Panel ID="Panel_Select" runat="server">

									<table width="100%" cellspacing="10">
										<tr>
											<td style="width: 50%" align="right">Select New Class : 
											</td>
											<td style="width: 50%" align="left">
												<table>
													<tr>
														<td>
															<asp:DropDownList ID="Drp_NewClass" runat="server" class="form-control" Width="180px">
															</asp:DropDownList>
														</td>
														<td>
															<asp:CheckBox ID="Chk_Class" runat="server" Checked="true" AutoPostBack="true"
																Text="Same Standard Only" OnCheckedChanged="Chk_Class_CheckedChanged" />
														</td>
													</tr>
												</table>

											</td>
										</tr>


										<tr>
											<td class="leftside">
												<br />
											</td>
											<td class="rightside">
												<br />
											</td>
										</tr>


										<tr>
											<td style="width: 50%" align="right">Reason: 
											</td>
											<td style="width: 50%" align="left">
												<asp:TextBox ID="Txt_Remark" runat="server" Width="180px" MaxLength="150" class="form-control" TextMode="MultiLine" Height="50px"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td class="leftside">
												<br />
											</td>
											<td class="rightside">
												<br />
											</td>
										</tr>
										<tr>
											<td colspan="2" align="center">

												<asp:Button ID="Btn_Switch" runat="server" Text="Switch" Class="btn btn-primary"
													OnClick="Btn_Switch_Click" />

												&nbsp; &nbsp;<asp:Button ID="Btn_Cancel" runat="server" Text="Cancel"
													Class="btn btn-danger" OnClick="Btn_Cancel_Click" />

											</td>
										</tr>
										<tr>
											<td colspan="2" align="center">

												<asp:Label ID="Lbl_mainmsg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>

											</td>
										</tr>
									</table>

								</asp:Panel>



								<asp:Panel ID="Panel_Information" runat="server" Visible="false">

									<br />

									<h4 style="color: #444">For Your Information </h4>
									<div class="InfoBorder">

										<table width="100%">
											<tr>
												<td class="ModuleHeading">New class selected is <u>
													<asp:Label ID="lbl_newclass" runat="server" class="control-label" Text=""></asp:Label></u>
												</td>
												<td></td>
											</tr>
											<tr>
												<td class="ModuleHeading">
													<u>EXAM</u>
												</td>
												<td></td>
											</tr>
											<tr class="trstyle">
												<td class="Itemsleft">
													<asp:Label ID="Lbl_Exam1" runat="server" class="control-label" Text=""></asp:Label>
												</td>
												<td class="Itemsright" align="center">

													<asp:RadioButtonList ID="Rdb_Exam1" runat="server" RepeatDirection="Horizontal">
													</asp:RadioButtonList>

												</td>
											</tr>

											<tr class="trstyle">
												<td class="Itemsleft">
													<asp:Label ID="Lbl_Exam2" runat="server" class="control-label" Text=""></asp:Label>
												</td>
												<td class="Itemsright" align="center">

													<asp:RadioButtonList ID="Rdb_Exam2" runat="server" RepeatDirection="Horizontal">
													</asp:RadioButtonList>

												</td>
											</tr>

											<tr>
												<td class="Itemsleft"></td>
												<td class="Itemsright"></td>
											</tr>

											<tr>
												<td class="ModuleHeading">
													<u>FEES</u>
												</td>
												<td></td>
											</tr>
											<tr class="trstyle">
												<td class="Itemsleft">

													<asp:Label ID="Lbl_Fees1" runat="server" class="control-label" Text=""></asp:Label>

												</td>
												<td class="Itemsright" align="center">


													<asp:RadioButtonList ID="Rdb_Fees1" runat="server" RepeatDirection="Horizontal">
													</asp:RadioButtonList>

												</td>
											</tr>



											<tr class="trstyle">
												<td class="Itemsleft">

													<asp:Label ID="Lbl_Fees2" runat="server" class="control-label" Text=""></asp:Label>

												</td>
												<td class="Itemsright" align="center">


													<asp:RadioButtonList ID="Rdb_Fees2" runat="server" RepeatDirection="Horizontal">
													</asp:RadioButtonList>

												</td>
											</tr>
											<tr>
												<td class="Itemsleft"></td>
												<td class="Itemsright"></td>
											</tr>
											<tr>
												<td class="ModuleHeading">
													<u>ATTENDANCE</u>
												</td>
												<td></td>
											</tr>
											<tr class="trstyle">
												<td class="Itemsleft">
													<asp:Label ID="Lbl_Attendance1" runat="server" class="control-label" Text=""></asp:Label>
												</td>
												<td class="Itemsright" align="center">

													<asp:RadioButtonList ID="Rdb_Attedance1" runat="server" RepeatDirection="Horizontal">
													</asp:RadioButtonList>

												</td>
											</tr>
											<tr>
												<td class="Itemsleft"></td>
												<td class="Itemsright"></td>
											</tr>
										</table>

										<br />

										<br />


									</div>

									<br />


									<table width="100%">
										<tr>
											<td align="right">

												<asp:Button ID="Btn_Confirm" runat="server" Text="Confirm" Class="btn btn-success" OnClientClick="return confirm('Are you sure, you want to change student class');"
													OnClick="Btn_Confirm_Click" />


												&nbsp;
				
				
			<asp:Button ID="Bnt_CancelSelection" runat="server" Text="Cancel"
				Class="btn btn-danger" OnClick="Bnt_CancelSelection_Click" />

												&nbsp;
		
											</td>
										</tr>
									</table>


								</asp:Panel>



							</div>



						</td>
						<td class="e"></td>
					</tr>
					<tr>
						<td class="so"></td>
						<td class="s"></td>
						<td class="se"></td>
					</tr>
				</table>
			</div>
			<asp:Panel ID="Pnl_MessageBox" runat="server">
				<asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" Style="display: none" />
				<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server"
					BackgroundCssClass="modalBackground"
					PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
				<asp:Panel ID="Pnl_msg" runat="server" DefaultButton="Btn_magok" Style="display: none;">
					<%--style="display:none;"--%>
					<div class="container skin5" style="width: 400px; top: 400px; left: 400px">
						<table cellpadding="0" cellspacing="0" class="containerTable">
							<tr>
								<td class="no">
									<asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png"
										Height="28px" Width="29px" />
								</td>
								<td class="n"><span style="color: White">
									<asp:Label ID="Lbl_Head" runat="server" Text="Message" class="control-label"></asp:Label></span></td>
								<td class="ne">&nbsp;</td>
							</tr>
							<tr>
								<td class="o"></td>
								<td class="c">

									<asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>

									<div id="HtmlDiv" runat="server">
									</div>
									<br />
									<br />
									<div style="text-align: center;">

										<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-info"
											OnClick="Btn_magok_Click" />

									</div>
								</td>
								<td class="e"></td>
							</tr>
							<tr>
								<td class="so"></td>
								<td class="s"></td>
								<td class="se"></td>
							</tr>
						</table>
						<br />
						<br />



					</div>
				</asp:Panel>
			</asp:Panel>
			</div>

			<WC:MSGBOX ID="WC_MessageBox" runat="server" />
		</div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>
