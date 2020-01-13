<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="SheduleExam" Codebehind="ScheduleExam.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	  
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
			<tr >
				<td class="no"> </td>
				<td class="n">Schedule Exam</td>
				
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
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
												<asp:Label ID="Lbl_freqlb" runat="server"  class="control-label"
													Text="Exam Frequency" ForeColor="White"></asp:Label>
											</td>
											<td>
												<asp:Label ID="Lbl_Frequency" runat="server"  ForeColor="White" class="control-label"
													Text="Monthly"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
											<td >
												<asp:Label ID="Label1" runat="server" ForeColor="White" Text="Class" class="control-label"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="DrpClassName" runat="server" Width="128px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpClassName_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
											<td>
												<asp:Label ID="Label2" runat="server" ForeColor="White" Text="Period" class="control-label"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="DrpExamPeriod" runat="server" Width="128px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpExamPeriod_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
											<td>
												<asp:Label ID="Label3" runat="server" ForeColor="White" Text="Grade Master"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="DrpGradeMaster" runat="server" Width="128px" class="form-control"
													AutoPostBack="True" >
												</asp:DropDownList>
											</td>
										</tr>
									</table>
				 </asp:Panel>
				 
				 
				 </div>
				
 
  <div class="roundbox">
						<table width="100%">
						<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
						<tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
	   <img alt="" src="Pics/timetable.gif" width="45" height="45" /></td>
	<td><h3>Time Slot</h3></td>
	<td style="text-align:right;"><asp:ImageButton ID="Btn_exporttoexel" runat="server"  Width="35" Height="35"
			onclick="Btn_exporttoexel_Click"  ImageUrl="Pics/Excel.png" ToolTip="Export To Excel"/></td>
	</tr></table>
		
<div class="linestyle"></div>
 
 <asp:Panel ID="Panel1" runat="server" Width="100%" >
  <table width="100%">
  <tr>
  <td>
	
  </td>
  <td> 
					  
  </td>
  <td ></td>
  </tr>
 
  <tr>
  <td colspan="3">
	  <asp:Label ID="Lbl_note" runat="server" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
	 &nbsp; <asp:LinkButton ID="lnk_assign" runat="server" OnClick="lnk_manageExam_Click"> Map Now</asp:LinkButton>
	<br />
  </td>
  </tr>
  <tr>
  <td style="width:100%" colspan="3">
	<asp:GridView ID="Grd_ExamSchdule" runat="server"  AutoGenerateColumns="False" Width="100%" 
		   BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px">
		<Columns>
				<asp:BoundField DataField="Id" HeaderText="SubjectId" />
				<asp:BoundField DataField="subject_name" HeaderText="Subject" />
				<asp:BoundField DataField="SubjectCode" HeaderText="Subject Code" ItemStyle-Width="120px" />
				<%--<asp:BoundField DataField="subject_code" HeaderText="Subject Code" />--%>
						<asp:TemplateField HeaderText="Exam Date" ItemStyle-Width="100px">
						 <ItemTemplate>
							 <asp:TextBox ID="Txt_ExamDate" runat="server" Text="" Width="100px" class="form-control"  ></asp:TextBox>    
							   <ajaxToolkit:CalendarExtender ID="Txt_ExamDate_CalendarExtender" runat="server" 
								CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ExamDate" Format="dd/MM/yyyy">
							  </ajaxToolkit:CalendarExtender>
							<%--<asp:RegularExpressionValidator runat="server" ID="ExamDateRegularExpressionValidator3"
								ControlToValidate="Txt_ExamDate"
								Display="None" 
								ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
							  <asp:RegularExpressionValidator ID="ExamDateRegularExpressionValidator3" 
														runat="server" ControlToValidate="Txt_ExamDate" Display="None" 
														ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
														 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
														 />  
							 <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="Exam_ValidatorCalloutExtender"
								TargetControlID="ExamDateRegularExpressionValidator3"
								HighlightCssClass="validatorCalloutHighlight" />                              
								
					
							</ItemTemplate>  
						</asp:TemplateField>
						<asp:TemplateField  HeaderText="Time Slot" ItemStyle-Width="180px">
							<ItemTemplate>
								<asp:DropDownList ID="Drp_TimeSlot" runat="server" Width="180px" class="form-control">
								</asp:DropDownList>
							</ItemTemplate>
						</asp:TemplateField>
						 <asp:TemplateField  HeaderText="Subject Order" ItemStyle-Width="180px">
							<ItemTemplate>
								<asp:TextBox ID="txt_SubjectOrder" runat="server" Width="80px" class="form-control" Text="0" ></asp:TextBox>
						   <ajaxToolkit:FilteredTextBoxExtender ID="txt_SubjectOrder_FilteredTextBoxExtender" 
											runat="server" Enabled="True" FilterType="Numbers" 
											TargetControlID="txt_SubjectOrder" ></ajaxToolkit:FilteredTextBoxExtender>
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
	<br />
	&nbsp;&nbsp;<br />
	<br />
	<div style="text-align:center;">
	  
	<asp:Button ID="Btn_ScheduleExam" runat="server" Text="Schedule" Class="btn btn-success"
		onclick="Btn_ScheduleExam_Click" />
	   
	   
	&nbsp;<asp:Button ID="btn_UpdateSchdule" runat="server" Text="Edit" Class="btn btn-primary"
		 onclick="btn_UpdateSchdule_Click" />
		 &nbsp;<asp:Button 
		ID="Btn_clear" runat="server"  Text="Reset"  Class="btn btn-danger"
		onclick="Btn_clear_Click" />
		<asp:TextBox ID="Txt_CXmId" runat="server" Visible="false" class="form-control"></asp:TextBox>
	<br />
	   <br />
	   </div> 
	   </td>
	   </tr>  
	 </table>	
	   </asp:Panel>
	   </td><td class="centerright"></td></tr>
						<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
						</table>
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
		
			   <WC:MSGBOX  ID="WC_MessageBox" runat="server" />   
					   
		</ContentTemplate>
		<Triggers><asp:PostBackTrigger ControlID="Btn_exporttoexel" /></Triggers>
	</asp:UpdatePanel>
	
</div>

<div class="clear">
   
</div>

</div>
</asp:Content>

