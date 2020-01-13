<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ViewIncidenceByType.aspx.cs" Inherits="WinEr.WebForm26" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
	function openIncpopup(strOpen) {
		open(strOpen, "Info", "status=1, width=600, height=450,resizable = 1");
	}
	function openIncedents(strOpen) {
		open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
	}

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
					<div class="cardHd">View Student Incidents</div>
						<div class="row stntStripBody">
			<div class="_stdntTopStrip"></div>
		</div>
				</div>
			</div>
			<br>
			<div class="row">
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
						  <div class="card0" >
							<table class="containerTable">
								<%--<tr >
									<td class="no"><img alt="" src="images/indnt_srch.png" width="35" height="35" /> </td>
									<td class="n">View Student Incidents</td>
									<td class="ne"> </td>
								</tr>--%>
								<tr >
									<td class="o"> </td>
									<td class="c" >
				
									<div style="min-height:250px;">
									<br />
										<asp:Panel ID="IncidentData"  runat="server">
										<table width="100%">
										<tr>
											<td align="left" style="width:170px"><asp:DropDownList ID="Drp_IncType" runat="server" Width="160" class="form-control"
													AutoPostBack="True" onselectedindexchanged="Drp_IncType_SelectedIndexChanged">
												</asp:DropDownList></td>
							
													<td align="right" >
														<asp:RadioButtonList ID="Rdb_Batch" runat="server" RepeatDirection="Horizontal" 
															onselectedindexchanged="Rdb_Batch_SelectedIndexChanged" AutoPostBack="true">
															<asp:ListItem Value="2" >ALL</asp:ListItem>
														<asp:ListItem Value="0" Selected="True">Current Batch</asp:ListItem>
														<asp:ListItem Value="1">Previous Batch</asp:ListItem>
														</asp:RadioButtonList></td>
													<td align="left"> <asp:DropDownList ID="Drp_PreviousBatch" runat="server" AutoPostBack="true" class="form-control"
															Width="100px" onselectedindexchanged="Drp_PreviousBatch_SelectedIndexChanged">
														</asp:DropDownList></td>
							 
											<td align="right" style="width:80px"> <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-danger"
															onclick="Btn_Delete_Click" />
												<asp:TextBox ID="Text_Hidden" runat="server" Visible="false" class="btn btn-info"></asp:TextBox>    </td>
										</tr>
					
										</table>
					   
						
											<br />
						
										</asp:Panel> 
										<div class="linestyle"></div>
				   
										<table width="100%">
										<tr>
											<td align="center"><asp:Label ID="lbl_viewIncidentMsg" runat="server" ForeColor="Red" Text=""></asp:Label></td>
										</tr>
										<tr>
											<td><asp:Panel ID ="Pnl_incidentGrid" runat="server">
												<table width="100%">
												<tr>
													<td> <asp:LinkButton ID="Lnk_Select" runat="server" onclick="Lnk_Select_Click">Select All</asp:LinkButton></td>
													<td  align="right" style="padding-right:20px">
													<asp:Label ID="lbl_Points" runat="server"  Text="Total Points :" class="control-label"></asp:Label>
														<asp:Image ID="Img_Points" runat="server" Height="15px" Width="15px" class="control-label" ImageAlign="AbsMiddle"  />
													<asp:Label ID="lbl_TotalPoints" runat="server" Font-Bold="true" class="control-label" Text="0"></asp:Label>
													</td>                                                
												</tr>
											</table>
						
												<asp:GridView ID="Grd_Incident" runat="server" CellPadding="4" ForeColor="Black" 
										GridLines="Vertical" AutoGenerateColumns="False"   
										Width="100%"  AllowSorting="true"
										onselectedindexchanged="Grd_Incident_SelectedIndexChanged" 
										onpageindexchanging="Grd_Incident_PageIndexChanging" BackColor="White" AllowPaging="True"
											BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" PageSize="15" 
													onsorting="Grd_Incident_Sorting">
						   
										<Columns>
										<asp:TemplateField ItemStyle-Width="20px">
											<ItemTemplate>
												<asp:CheckBox id ="Chk_Incident" runat="server" />
											</ItemTemplate>
										</asp:TemplateField>
											<asp:BoundField DataField="Id" HeaderText="Id" />
											<asp:BoundField DataField="Title" HeaderText=" Title" SortExpression="Title" ItemStyle-Width="180px" />                     
											<asp:BoundField DataField="Type" HeaderText=" Incident Type" SortExpression="Type" ItemStyle-Width="60px"/>
											<asp:BoundField DataField="Point" HeaderText=" Point" SortExpression="Point" />
						
											<asp:TemplateField HeaderText ="Point" ItemStyle-Width="30px">
											<ItemTemplate>
												<asp:Image ID="Img_Point" runat="server" Height="15px" Width="15px" ImageAlign="Middle" />
													<asp:Label ID="lbl_Point" runat="server" Text=""></asp:Label>
											</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="IncidentDate" HeaderText=" Incident Date" ItemStyle-Width="40px" SortExpression="IncidentDate" />
											<asp:BoundField DataField="SurName" HeaderText="Created User" SortExpression="SurName" ItemStyle-Width="50px"/>                    
											<%--<asp:TemplateField HeaderText ="Created for" ItemStyle-Width="90px">
											<ItemTemplate>
												<asp:Label ID="Lbl_PupilName" runat="server" Text=""></asp:Label>
											</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText ="Type" ItemStyle-Width="40px">
											<ItemTemplate>
												<asp:Label ID="Lbl_PupilType" runat="server" Text=""></asp:Label>
											</ItemTemplate>
											</asp:TemplateField>--%>
					   
											<asp:CommandField ItemStyle-Width="25px" ShowSelectButton="True" SelectText="&lt;img src='pics/hand.png' width='30px' border=0 title='View Incident'&gt;"/>
					   
										</Columns>
											<PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
										<FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
										<EditRowStyle Font-Size="Medium" />
										<SelectedRowStyle BackColor="White" ForeColor="Black" />
										<PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
										<HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
										<RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
					
									</asp:GridView>
											</asp:Panel></td>
										</tr>
										</table>
				  
										<asp:Button runat="server" ID="Btn_Message" class="btn btn-info" style="display:none"/>
												<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Confirm"   runat="server"  BackgroundCssClass="modalBackground" CancelControlID="Btn_Cnfirm_Cancel" PopupControlID="Pnl_Confirm" TargetControlID="Btn_Message"  />
													<asp:Panel ID="Pnl_Confirm" runat="server" style="display:none;">
														<div class="container skin5" style="width:400px; top:400px;left:200px" >
															<table   cellpadding="0" cellspacing="0" class="containerTable">
																<tr >
																		<td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
																		</td>
																		<td class="n"><span style="color:White">alert!</span></td>
																		<td class="ne">&nbsp;</td>
																	</tr>
																	<tr >
																		<td class="o"> </td>
																		<td class="c" >             
																			<asp:Label ID="Lbl_Confirm" runat="server" Text="" class="control-label"></asp:Label>
																			<br /><br />
																			<div style="text-align:center;">    
																				<asp:Button ID="Btn_ConfirmDelete" runat="server" Text="Yes" Class="btn btn-info" OnClick="Btn_Confirm_Click" />        
																				<asp:Button ID="Btn_Cnfirm_Cancel" runat="server" Text="No" Class="btn btn-info"/>
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
				  
										<asp:Button runat="server" ID="Btn_PopUp" style="display:none"/>
												<ajaxToolkit:ModalPopupExtender ID="MPE_IncidentPopUp"   runat="server" CancelControlID="Btn_IncP_Cancel" BackgroundCssClass="modalBackground" PopupControlID="Pnl_IncidentPopUp" TargetControlID="Btn_PopUp"  />
													<asp:Panel ID="Pnl_IncidentPopUp" runat="server" style="display:none">
														<div class="container skin5" style="width:700px; top:400px;left:200px" >
															<table   cellpadding="0" cellspacing="0" class="containerTable">
																<tr >
																		<td class="no"><asp:Image ID="Image1" runat="server" 
																				ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" />
																		</td>
																		<td class="n"><span style="color:White">View Incident</span></td>
																		<td class="ne">&nbsp;</td>
																	</tr>
																	<tr >
																		<td class="o"> </td>
																		<td class="c" >             
																			<asp:Label ID="Lbl_IncidentPopUup" runat="server" class="control-label" Text=""></asp:Label>
																			<br />
																			<div >
																			<table width="100%">
															
																					<tr>
																					<td>IncidentType</td>
																					<td >
																						<asp:TextBox ID="Txt_Type" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																					<td>Created User</td>
																					<td>
																						<asp:TextBox ID="Txt_CreatedUser" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																				</tr>
																				<tr>
																					<td>Incident Date</td>
																					<td >
																						<asp:TextBox ID="Txt_IncidentDate" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																					<td>Created Date</td>
																					<td>
																						<asp:TextBox ID="Txt_CreatedDate" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																				</tr>
																				<tr>
																					<td>Created for</td>
																					<td >
																						<asp:TextBox ID="Txt_ReportedTo" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																					<td>
																						<asp:Label ID="Lbl_Class" runat="server" Text="Class" class="control-label"></asp:Label></td>
																					<td>
																						<asp:TextBox ID="Txt_Class" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
																				</tr>
																					<tr>
																						<td>
																							Type</td>
																						<td>
																							<asp:TextBox ID="Txt_UserType" runat="server" class="form-control" ReadOnly="True" Width="180px"></asp:TextBox>
																						</td>
																						<td>
																							&nbsp;</td>
																						<td>
																							<asp:TextBox ID="Txt_UserId" runat="server" Visible="False" class="form-control" Wrap="False"></asp:TextBox>
																							<asp:TextBox ID="Txt_IncidentId" runat="server"  class="form-control" Visible ="false"></asp:TextBox>
																						</td>
																					</tr>
																				<tr>
																					<td>Title</td>
																					<td colspan="3">
																						<asp:TextBox ID="Txt_Title" runat="server" ReadOnly="True" class="form-control" Width="505px"></asp:TextBox></td>
																				</tr>
														   
																				<tr>
																				<td>Description</td>
																				<td colspan="3">
																					<asp:TextBox ID="Txt_Desc" runat="server" Height="50px" ReadOnly="True" class="form-control"
																						TextMode="MultiLine" Width="505px"></asp:TextBox></td>
																				</tr>
																				<tr>
																					<td colspan ="4" align="center">
																						<asp:Button ID="Btn_DeletePopUp" runat="server" Text="Delete" Class="btn btn-info" OnClick="Btn_DeletePopup_Click"/>&nbsp;&nbsp;&nbsp;
																					<asp:Button ID="Btn_IncP_Cancel" runat="server" Text="OK" Class="btn btn-info"/>
																					</td>
																				</tr>
																			</table>                        
															
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
						<WC:MSGBOX id="WC_MessageBox" runat="server" />  
					</ContentTemplate>
					
				</asp:UpdatePanel>
			</div>
		 </div>   
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>