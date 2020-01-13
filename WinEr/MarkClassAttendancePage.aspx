<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkClassAttendancePage.aspx.cs" MasterPageFile="WinErStudentMaster.master" Inherits="WinEr.MarkClassAttendancePage" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="js%20files/RollNoListItem.js?v=5.0.0"></script>
	<script>
		var Page;

		var postBackElement;

		function pageLoad() {

			Page = Sys.WebForms.PageRequestManager.getInstance();

			Page.add_beginRequest(OnBeginRequest);

			Page.add_endRequest(endRequest);

		}



		function OnBeginRequest(sender, args) {

			postBackElement = args.get_postBackElement();

			postBackElement.disabled = true;

		}



		function endRequest(sender, args) {

			postBackElement.disabled = false;

		}


	</script>
	<style type="text/css">
		#RdBtn_Type {
			width: 30%;
			margin-left: 36%;
		}

		@media screen and (max-device-width: 790px) {

			#RdBtn_Type {
				width: 100%;
				margin-left: 0;
			}
		}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
				$("#Txt_RollNo").focus();
		});

			var oSelectedListBox;

			var BaseArguments;
			var ItemArray = new Array();

			var Id;


			function TextOnClick(e) {
				var KeyID;
				if (window.event) { //IE

					KeyID = event.keyCode
				}
				else  // Netscape/Firefox/Opera
				{
					KeyID = e.keyCode;
				}
				if (KeyID == 13) {
					AddRollNo();
					return false;
				}
				if (KeyID == 8) {
					return true;
				}
	  
				if (KeyID < 48 || KeyID > 105) {

				
					return false;
				}
				else {
					if (KeyID > 57) {
						if (KeyID < 96) {
							return false;
						}
					}
				}
			}
			function AddRollNo() {
				document.getElementById("<%= Lbl_msg.ClientID %>").value = "";
				document.getElementById("Txt_RollNo").style.backgroundColor = 'White';
				document.getElementById("Txt_RollNo").title = "Enter RollNo";
			
				var RollNo = document.getElementById("Txt_RollNo").value.toUpperCase();
				if (RollNo == "") {
					document.getElementById("Txt_RollNo").style.backgroundColor = 'Red';
					document.getElementById("Txt_RollNo").title = "No data present for adding";
					return;
				}

				if (oSelectedListBox == undefined) {

					BaseArguments = {
						Base: document.getElementById('Selectedbase'),
						Name: "Marked",
						//Width: 130,
						NormalItemColor: null,
						NormalItemBackColor: '#ffffff',
						AlternateItemColor: null,
						AlternateItemBackColor: '#ffffff',
						SelectedItemColor: null,
						SelectedIItemBackColor: null,
						HoverItemColor: null,
						HoverItemBackColor: null,
						HoverBorderdColor: null,
						ClickEventHandler: OnClick
					};
					oSelectedListBox = new ListBox(BaseArguments);
				}

				if (Item_Exits(RollNo)) {
					document.getElementById("Txt_RollNo").style.backgroundColor = 'Red';
					document.getElementById("Txt_RollNo").title = RollNo + " already entered";
					Refill_List();
				}
				else {

					ItemArray.unshift(RollNo);
					Refill_List();

				}
				document.getElementById("Txt_RollNo").value = "";
				if (document.activeElement != document.body) document.activeElement.blur();
				document.getElementById("Txt_RollNo").focus();
				$('#Txt_RollNo').focus();
				//document.getElementById("Box1").focus();
			}
			function Item_Exits(RollNo) {

				for (i = 0; i < ItemArray.length; i++) {
					if (ItemArray[i].toLowerCase() == RollNo.toLowerCase()) {
						return true;
					}

				}
				return false;

			}

			function Refill_List() {
				oSelectedListBox.DeleteItems();

				for (i = ItemArray.length-1; i >= 0; i--) {
					oSelectedListBox.AddItem(ItemArray[i], i);
				}
				if (ItemArray.length == 0) {
					oSelectedListBox.Dispose();
					oSelectedListBox = undefined;
				}
			}


			function Cancel() {
				if (oSelectedListBox != undefined) {
					oSelectedListBox.Dispose();
				}
				oSelectedListBox = undefined;
				if (document.getElementById("Txt_RollNo") != undefined) {
					document.getElementById("Txt_RollNo").value = "";
					document.getElementById("Txt_RollNo").style.backgroundColor = 'White';
					document.getElementById("Txt_RollNo").title = "Enter RollNo";
				}
				document.getElementById("<%= Lbl_msg.ClientID %>").value = "";
				ItemArray = new Array();
		  
			}

			function BeforeSave() {

				var name = "", value = "";
				var symbol = "";
				if (ItemArray.length != 0) {
					for (i = 0; i < ItemArray.length; i++) {
						name = name + symbol + ItemArray[i];
						symbol = '/';
					}
					document.getElementById("<%= HiddenValue.ClientID %>").value = name;
					Cancel();
				}
				else {
					alert("No data present for saving");
					return false;
				}

			}

			var OnClick = function(Sender, EventArgs) {
				var Message = "";
				var ItemId;
				Message = EventArgs.Text;
				ItemId = EventArgs.ItemIndex;
				var r = confirm("Remove " + Message + " from the list?", 4, 3)
				if (r == true) {
					ItemArray.splice(ItemId, 1);
				}
				Refill_List()
			}



			function ShowErrors() {
				var errors = document.getElementById("<%= HiddenValue.ClientID %>").value;
				var es = errors.split('/');
				alert(es);
			}

			function Check_Enter(e) {
				var KeyID;
				if (window.event) { //IE

					KeyID = event.keyCode
				}
				else  // Netscape/Firefox/Opera
				{
					KeyID = e.keyCode;
				}
				if (KeyID == 13) {
					return false;
				}
			}


			function ShouldUnmark() {
				var Value = document.getElementById("<%= HiddenDate.ClientID %>").value;
				var Message = "You are about to cancel the attendance marked for " + Value+". Are you sure about continuing?";
				return confirm(Message);
			}

			function pageresize() {
				window.resizeTo(420, 240);
			}


			</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
	<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">

		<ProgressTemplate>


			<div id="progressBackgroundFilter">
			</div>

			<div id="processMessage">

				<table style="height: 100%; width: 100%">

					<tr>

						<td align="center">

							<b>Please Wait...</b><br />

							<br />

							<img src="images/indicator-big.gif" alt="" /></td>

					</tr>

				</table>

			</div>


		</ProgressTemplate>

	</asp:UpdateProgress>
	<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
		<ContentTemplate>

			<div class="container-fluid" style="background-color: white; padding-bottom: 20vh;">
		
				<div class="row">
					<div id="heading">
						<center>
						<h4>Student Attendance Marking</h4>
						 </center>
					</div>
					<div>
						<asp:Button ID="Btn_Back" runat="server" Text="Back" Width="100px" Class="btn btn-primary" OnClientClick="window.location='MarkClassAttendanceMaster.aspx'" />
					</div>
					<hr>
				</div>

				<div class="row">
					<div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
						  <div class="well">
						  <p>Selected Class</p>
						<asp:Label ID="Lbl_Class" runat="server" Text="." Font-Bold="true" class="control-label"></asp:Label>
					</div></div>
					<div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
						
							  <div class="well">
								<%--<asp:Label ID="Label2" runat="server" Text="Select Attendance Mode" class="control-label"></asp:Label>--%>
							<p>Select Attendance Mode</p>
								<asp:RadioButtonList ID="RdBtn_Type" runat="server" RepeatDirection="Horizontal" style="margin:auto;">
									<asp:ListItem Selected="True" Text="Full Day" Value="3"> </asp:ListItem>
									<asp:ListItem Text="ForeNoon" Value="1"> </asp:ListItem>
									<asp:ListItem Text="AfterNoon" Value="2"> </asp:ListItem>
								</asp:RadioButtonList>
							</div>
					</div>
					<div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
						<div class="well">
						  <p>Selected Date</p>
						<asp:Label ID="lbl_Date" runat="server" Text="." Font-Bold="true" class="control-label"></asp:Label>
					</div>
						</div>
				</div>


				<tr>
					<td id="InnerStructure" colspan="4" valign="top">

						<asp:Panel ID="Panel_Saving" runat="server" Visible="false" CssClass="BottomBorder">


							<%--<asp:Label ID="Label4" runat="server" Text="ENTER RollNo" class="control-label"></asp:Label>--%>




							

							<div class="form-inline">
								<asp:Panel ID="Panel_RollNo" runat="server">
									<input type="text" id="Txt_RollNo" placeholder="Enter Roll Number" value="" maxlength="10" title="Enter RollNo" class="form-control" style="width: 160px" onkeydown="return TextOnClick(event)" />

									<input type="button" id="Btn_Add" value="Add" style="width: 100px" class="btn btn-primary" onmousedown="AddRollNo()" />
								</asp:Panel>
								<asp:LinkButton ID="Lnk_RollNoVisibilty" runat="server" CssClass="btn btn-default" OnClick="Lnk_RollNoVisibilty_Click">Enter Absent Students RollNo</asp:LinkButton>
									<%--<asp:ImageButton ID="Img_Cancel" runat="server" ImageUrl="~/images/cross.png" OnClientClick="Cancel()" Width="20" />--%>
								<asp:LinkButton ID="Lnk_Cancel" runat="server" OnClientClick="Cancel()"
									ForeColor="Black" CssClass="btn-danger">Clear</asp:LinkButton>
							</div>
							<br>
							<div style="overflow: auto;">
								<div id="Selectedbase"></div>
							</div>
							<br>




						
							&nbsp;&nbsp;
							<asp:Label ID="Lbl_msg" CssClass="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
							
							<asp:Button ID="Btn_All_Present" runat="server" Text="All Present" Width="110" Class="btn btn-primary" OnClick="Btn_All_Present_Click" />
							
							&nbsp;&nbsp;<asp:Button ID="Btn_Cancel" runat="server" Text="Reset" Width="110" Class="btn btn-danger" OnClientClick="Cancel()"
								OnClick="Btn_Cancel_Click" />
								<hr>
							
							<asp:Button ID="Btn_Save" runat="server" Text="Save" Width="110" Class="btn btn-success"
								OnClientClick="return BeforeSave()" OnClick="Btn_Save_Click" />

							


						</asp:Panel>


						<asp:Panel ID="Panel_Updating" runat="server">

							<table width="100%" cellspacing="10">

								<tr>
									<td align="">
										<asp:Button ID="Btn_Update" runat="server" Text="Update" Width="100" Class="btn btn-success"
											OnClick="Btn_Update_Click" />

										&nbsp; 
								
					<asp:Button ID="Btn_DeleteMarking" runat="server" Class="btn btn-danger"
						Text="Cancel Attendance Marked" OnClientClick="return ShouldUnmark()"
						OnClick="Btn_DeleteMarking_Click" />

										&nbsp;
								
				<asp:Button ID="Btn_CancelUpdate" runat="server" Text="Reset" Class="btn btn-primary" OnClientClick="window.location.reload()" Width="100" />
									</td>
								</tr>
								<tr>

									<td>
										<div style="height: 350px; overflow: auto;margin-top:10px;">

											<center>
												<asp:Label ID="Lbl_GridMsgCommon" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
											</center>

											<asp:GridView ID="Grd_Students" runat="server" CellPadding="4" ForeColor="Black"
												GridLines="Both" AutoGenerateColumns="False" DataKeyNames="Id,StudentName,PresentStatus"
												Width="98%" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
												OnRowEditing="Grd_Students_RowEditing">
												<Columns>
													<asp:BoundField DataField="Id" HeaderText="Id" />
													<asp:BoundField DataField="PresentStatus" HeaderText="Status"  ItemStyle-HorizontalAlign="Center" />
													<asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="80px"  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
													<asp:BoundField DataField="StudentName" HeaderText="Student Name"  ItemStyle-Font-Size="14px"  ItemStyle-HorizontalAlign="Center" />
													

													<asp:TemplateField HeaderText="Present Status" ItemStyle-Width="120px">
														<ItemTemplate>

															<asp:DropDownList ID="Drp_GridStatus" runat="server" class="form-control">
															</asp:DropDownList>

														</ItemTemplate>
													</asp:TemplateField>
													<asp:CommandField ItemStyle-Width="40" EditText="&lt;img src='pics/orange-folder-image.png' height='35px' width='35px' border=0 title='Select to view'&gt;"
														ShowEditButton="True" HeaderText="View" />
												</Columns>
												<RowStyle BackColor="White" />
												<FooterStyle BackColor="#CCCC99" />

												<HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" CssClass="GVFixedHeader"
													HorizontalAlign="Left" />
												<AlternatingRowStyle BackColor="White" />

											</asp:GridView>


										</div>

										<center>

											<asp:Label ID="Lbl_GridMsg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>

										</center>
									</td>

								</tr>
							</table>

						</asp:Panel>

					</td>
				</tr>

				</table>


			<asp:HiddenField ID="HiddenValue" runat="server" />
				<asp:HiddenField ID="HiddenClassAttendance" runat="server" />
				<asp:HiddenField ID="HiddenDate" runat="server" />
				<asp:HiddenField ID="Hidden_StandardId" runat="server" />
				<asp:HiddenField ID="HiddenStatus" runat="server" />
				<asp:HiddenField ID="HiddenLink" runat="server" />
				<asp:HiddenField ID="HiddenEmptyLink" runat="server" />



				<asp:Panel ID="Pnl_MessageBox" runat="server">

					<asp:Button runat="server" ID="Btn_hdnmessagetgt" Style="display: none" />
					<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"
						runat="server" CancelControlID="Btn_magok"
						PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackgroundtest" />
					<asp:Panel ID="Pnl_msg" runat="server" Style="display: none;">
						<div class="container skin1">
							<table cellpadding="0" cellspacing="0" class="containerTable">
								<tr>
									<td class="no"></td>
									<td class="n"><span style="color: Black">Message</span></td>
									<td class="ne">&nbsp;</td>
								</tr>
								<tr>
									<td class="o" style="height: 58px"></td>
									<td class="c" style="height: 58px" align="center">

										<asp:Label ID="Lbl_msgAlert" runat="server" Text="" Font-Bold="true" class="control-label"></asp:Label>
										<br />
										<br />
										<div style="text-align: center;">

											<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-primary" OnClientClick="window.close()" />
										</div>
									</td>
									<td class="e" style="height: 58px"></td>
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


				<asp:Panel ID="Panel2" runat="server">

					<asp:Button runat="server" ID="Button2" Style="display: none" />
					<ajaxToolkit:ModalPopupExtender ID="M_Finish"
						runat="server"
						PopupControlID="Panel3" TargetControlID="Button2" BackgroundCssClass="modalBackgroundtest" />
					<asp:Panel ID="Panel3" runat="server" Style="display: none;">
						<div class="container skin1">
							<table cellpadding="0" cellspacing="0" class="containerTable">
								<tr>
									<td class="no"></td>
									<td class="n"><span style="color: Black">Message</span></td>
									<td class="ne">&nbsp;</td>
								</tr>
								<tr>
									<td class="o" style="height: 58px"></td>
									<td class="c" style="height: 58px" align="center">
										<table>
											<tr>
												<td>
													<asp:Label ID="Lbl_FinishMsg" runat="server" Text="" class="control-label"></asp:Label>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="lbl_MissRollNos" runat="server" Text="" class="control-label"></asp:Label>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="Lbl_ErrorRolLNo" runat="server" Text="" class="control-label"></asp:Label>
												</td>
											</tr>
											<tr>
												<td>
													<br />

													<asp:Label ID="Lbl_Link" runat="server" Text="" Visible="false" class="control-label"></asp:Label>
													<div style="text-align: center;">

														<asp:Button ID="Btn_Ok" runat="server" Text="OK" Width="50px" class="btn btn-primary" OnClientClick="window.location.reload()" />
													</div>
												</td>
											</tr>
										</table>




									</td>
									<td class="e" style="height: 58px"></td>
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
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
