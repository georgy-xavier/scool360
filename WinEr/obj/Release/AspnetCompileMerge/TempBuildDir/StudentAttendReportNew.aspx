<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentAttendReportNew.aspx.cs" Inherits="WinEr.StudentAttendReportNew" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
		
		.leftLabel
		{
			width:35%;
			font-weight:normal;
			text-align:right;
		}
		.rightLbl
		{
			font-weight:normal;
			text-align:left;
		}
		.leftFields
		{
			 width:20%;
			font-weight:bolder;
			color:Black;
			width: 140px;
		}
		.GridCells
		{
			padding:10px;
		}
		.HeaderStyle
		{
			 
			  margin:20px;
		}
		.TopData
		{
			height:25px;
		}
		.GridRight
		{
			font-weight:bolder;
			padding-left:30px;
			border:solid 1px gray;
		}
		.GridLeft
		{
			font-weight:lighter;
			padding-left:30px;
			border:solid 1px gray;
		}
		 .GridLeftHead
		 {
			 font-weight:bolder;
			 background-color:#666666;
			 color:White;
		 }
		 .GridRightHead
		 {
			 font-weight:bolder;
			 background-color:#666666;
			  color:White;
		 }
		
	</style>
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
					<div class="cardHd">Student Attendance Report</div>
					<div class="row stntStripBody">
							<div class="_stdntTopStrip"></div>
					</div>
				</div>
			</div>
			<br>
			 <div class="row">
				 <div class="card0" >
					<table  class="containerTable">
		   <%-- <tr >
				<td class="no"> </td>
				<td class="n">Attendance Report</td>
				<td class="ne"> </td>
			</tr>--%>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				   
				   
				   
<asp:Panel ID="Pnl_attendanceReportarea" runat="server">
 <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%" 
		CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True" 
		ActiveTabIndex="2"  >
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="YEARLY" Visible="true" >
	<HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart.png" /><b>YEARLY</b></HeaderTemplate>         
	<ContentTemplate> 
	 
	   <br />
					<asp:Panel ID="Pnl_generalArea" runat="server">
					
					
					<table width="100%">
							  <tr class="TopData">
									  <td colspan="3">
										  <asp:LinkButton ID="LinkBtn_Yearly" runat="server" Font-Size="9" 
											  onclick="LinkBtn_Yearly_Click" >Check Yearwise Detailed Report</asp:LinkButton></td>
								  <td class="leftLabel">
									  Batch:</td>
								  <td  class="leftFields">
									  <asp:Label ID="Lbl_batch" runat="server" class="control-label"></asp:Label></td>
							  </tr>
							  <tr>
								  <td colspan="5">
								 
								   <div class="linestyle">                  
									</div>
									 </td>
							  </tr>
							  <tr>
								  
									  <td rowspan="10"  valign="top">
									  
								  <web:chartcontrol id="chartcontrol_yearly" runat="server" Width="350px" 
										GridLines="None" BorderStyle="None" TopPadding="0" ChartPadding="0" 
										YCustomEnd="0" YCustomStart="0" YValuesInterval="0" Padding="5" 
											  BorderWidth="0px" Height="150px">
										<Border DashStyle="Dot" /><Background Color="LightSteelBlue" /><PlotBackground ForeColor="White" />
										<ChartTitle StringFormat="Center,Near,Character,LineLimit" />
										<Legend Font="Tahoma, 7pt, style=Bold"></Legend>
										<Charts><Web:PieChart Explosion="8" Legend="Some Legend" Name="yearly_Chart">
												   <DataLabels Font="Tahoma, 8pt, style=Bold" Separator=": " ShowXTitle="True" 
													   Visible="True">
															  <Border Color="Blue" />
															  <Background ForeColor="White" />
													</DataLabels>
													<Shadow Color="Gray" Visible="True" />
												  </Web:PieChart>
										 </Charts>
										 <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
										 <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
										 <XTitle StringFormat="Center,Near,Character,LineLimit" />
										 <YTitle StringFormat="Center,Near,Character,LineLimit" />
									   </web:chartcontrol>
									  </td>
								  <td  rowspan="10" valign="top">
									  </td>

								  
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Working Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_no_workingdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Full Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_no_presentdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
										<tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Half Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_no_halfdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Absent Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_no_absent_day" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Holidays:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_no_holiday" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Attendance Percentage:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_total_persent" runat="server" Text="80%" class="control-label"
										  ForeColor="#FF3300"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  </td>
								  <td>
									  </td>
								  <td>
									  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td colspan="2">
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
								  
									  &nbsp;</td>
								  <td colspan="2">
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
								  <td>
									  &nbsp;</td>
							  </tr>
							  
						  </table>
						  
						<asp:Panel ID="Pnl_yearlyBar" runat="server">
						
					 <div class="newsubheading">Monthly Attendance Percentage
					</div>    
					 <div class="linestyle">
									  </div>
					<br />
					  <table width="100%">
					   <tr>
						 <td style="width:90%">
						 
						  <web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="110" HasChartLegend="False"
							   id="chartcontrol_Monthlypersent"
							   runat="server" BorderWidth="0px" Padding="5" TopPadding="0" 
							Width="700px" YCustomStart="0" YValuesInterval="0" Height=""><Background Color="LightSteelBlue" />
							<ChartTitle StringFormat="Center,Near,Character,LineLimit"  />
							<XAxisFont StringFormat="Center,Near,Character,LineLimit" />
							<YAxisFont StringFormat="Far,Near,Character,LineLimit" />
							<XTitle StringFormat="Center,Near,Character,LineLimit" />
							<YTitle StringFormat="Center,Near,Character,LineLimit" />
							</web:chartcontrol>
						 
						 </td>
						 <td valign="top" align="center">
						   
							 <asp:ImageButton ID="Img_excel" runat="server" Height="47px" 
								 ImageUrl="~/Pics/Excel.png" OnClick="Img_excel_Click" ToolTip="Export to Excel" 
								 Width="42px" />
						   
						 </td>
					   </tr>
					  </table>
						  
					
				   
					 </asp:Panel> 
					</asp:Panel>
				   

		  
</ContentTemplate>  
				

</ajaxToolkit:TabPanel>
				
 <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="MONTHLY"  >
	 <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/calendar.png" /><b>MONTHLY</b></HeaderTemplate>                 
  <ContentTemplate>
  
  
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdate2" >
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
  
	 <asp:UpdatePanel ID="pnlAjaxUpdate2" runat="server">
	  <ContentTemplate>

				<br /> 
					  <asp:Panel ID="Pnl_monthlyreport" runat="server">
					  
					   
						  <table width="100%">
							  <tr>
								
									  <td rowspan="13" valign="top">
									  
										 <asp:Calendar ID="Calendar1" runat="server" Height="200px" Width="250px"
											onselectionchanged="Calendar1_SelectionChanged" BackColor="White" 
											BorderColor="Black" BorderStyle="Solid" CellSpacing="0" Font-Names="Verdana" 
											Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth" 
											ondayrender="Calendar1_DayRender"  
											onvisiblemonthchanged="Calendar1_VisibleMonthChanged">
											<SelectedDayStyle BackColor="#f9f7aa" ForeColor="Black" />
											<TodayDayStyle BackColor="White" ForeColor="Black"  BorderColor="Red" BorderWidth="2" BorderStyle="Solid"/>
											<OtherMonthDayStyle ForeColor="#999999"/>
											<DayStyle BackColor="White"  BorderColor="Black" BorderWidth="1" BorderStyle="Solid" />
											<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black"  CssClass="Nextmonth"  />
											<DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" 
												Height="8pt" />
											<TitleStyle BackColor="#ffffff" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" Font-Bold="True" 
												Font-Size="12pt" ForeColor="Black"  />
										</asp:Calendar>
									  
								  
									  </td>
					   
								  
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Working Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_TotalWorkingDay" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Full Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_presentdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Half Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_halfdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Absent Days:</td>
								  <td class="leftFields">
									  <asp:Label ID="lbl_absentdays" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Number of Holidays:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_holiday" runat="server" Text="0" class="control-label"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
								  <td class="leftLabel">
									  Attendance Percentage:</td>
								  <td class="leftFields">
									  <asp:Label ID="Lbl_attendancepersent" runat="server" Text="80%" class="control-label"
										  ForeColor="#FF3300"></asp:Label>
								  </td>
							  </tr>
							  <tr>
								  
								  <td colspan="3" rowspan="7" align="center">
								   <br />
									<web:chartcontrol id="chartcontrol_montnly" runat="server" Width="350px"
										GridLines="None" BorderStyle="None" TopPadding="0" ChartPadding="0" 
										YCustomEnd="0" YCustomStart="0" YValuesInterval="0" Padding="5" 
											  BorderWidth="0px" Height="150px">
										<Border DashStyle="Dot" />
										<Background Color="LightSteelBlue" /><PlotBackground ForeColor="White" />
										 <ChartTitle StringFormat="Center,Near,Character,LineLimit" />
										 <Legend Font="Tahoma, 7pt, style=Bold" Position="Right"></Legend>
										 <Charts><Web:PieChart Explosion="15" Legend="Some Legend" Name="Monthly_Chart">
										 <DataLabels Font="Tahoma, 8pt, style=Bold" 
															  ForeColor="White" Separator=": " ShowXTitle="True" Visible="True">
															  <Border Color="Blue" />
															  <Background Color="DimGray" ForeColor="White" />
															  </DataLabels>
															  <Shadow Color="Gray" Visible="True" />
															  </Web:PieChart>
										 </Charts>
										 <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
										 <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
										 <XTitle StringFormat="Center,Near,Character,LineLimit" />
										 <YTitle StringFormat="Center,Near,Character,LineLimit" />
									   </web:chartcontrol>
									  </td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
							  <tr>
								  <td>
									  &nbsp;</td>
							  </tr>
						  </table>
					  
					   
						  <br/> 
					  
					
					  
							
						</asp:Panel>
				 

	  </ContentTemplate>
	 </asp:UpdatePanel>
  </ContentTemplate>     

 </ajaxToolkit:TabPanel>
				
  <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="DETAILED"  >
   <HeaderTemplate><asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/book.png" /><b>DETAILED</b></HeaderTemplate>                 
	<ContentTemplate>
	  <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdate1" >
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
 <asp:UpdatePanel ID="pnlAjaxUpdate1" runat="server">
  <ContentTemplate> 
   <asp:Panel ID="Panel1" runat="server" >   
  
			<br />
						<table width="100%" >
							  <tr>
							  
							  <td align="right" style="width:20%">
									 Time Period</td>
								<td align="left"  style="width:20%">
									  <asp:DropDownList ID="Drp_Period" runat="server" AutoPostBack="True" class="form-control"
										  onselectedindexchanged="Drp_Period_SelectedIndexChanged" Width="170px">
									  <asp:ListItem>Today</asp:ListItem>
									  <asp:ListItem>Last Week</asp:ListItem>
									  <asp:ListItem>Month Wise</asp:ListItem>
									  <asp:ListItem>Manual</asp:ListItem>
								  </asp:DropDownList></td>
								<td align="right"  style="width:20%">
									  Select Month
								 </td>

								<td align="left"  style="width:20%">
									   <asp:DropDownList ID="Drp_Select_Month" runat="server" AutoPostBack="True" class="form-control" 
										   Width="170px" OnSelectedIndexChanged="Drp_Select_Month_SelectedIndexChanged" >
									  </asp:DropDownList>
								</td>
							  </tr>
							  
							  <tr>
							   <td colspan="4">
								 <br />
							   
							   </td>
							  </tr>
							  
							  <tr>
									  <td align="right" >
										 Start Date
									   </td>
									   <td align="left" >
										 <asp:TextBox ID="Txt_StartDate" runat="server" Width="170px" class="form-control"></asp:TextBox>
									  
											 <ajaxToolkit:CalendarExtender ID="Txt_StartDate_CalendarExtender" runat="server" 
													CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
												</ajaxToolkit:CalendarExtender>  
									   
											 <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
												 ControlToValidate="Txt_StartDate"
												Display="None" 
												ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
												ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
											   <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
												TargetControlID="DobDateRegularExpressionValidator3"
												HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
									   </td>
										 
									   <td  align="right" >
										  End Date 
										  </td>
										  <td align="left" >
											<asp:TextBox ID="Txt_EndDate" runat="server" class="form-control" Width="170px"></asp:TextBox>
											  <ajaxToolkit:CalendarExtender ID="Txt_EndDateCalendarExtender1" runat="server" 
												 CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
											  </ajaxToolkit:CalendarExtender> 
										  
												 <asp:RegularExpressionValidator runat="server" ID="Txt_EndDate_RegularExpressionValidator1"
															ControlToValidate="Txt_EndDate"
															Display="None" 
															ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
															ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
														   <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
															TargetControlID="DobDateRegularExpressionValidator3"
															HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
																	  
										  </td>
										  </tr>
							 <tr>
							   <td colspan="4">
								 <br />
							   
							   </td>
							  </tr>
										   <tr>
											  <td colspan="2">
												  
												  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
													  ControlToValidate="Txt_StartDate" ErrorMessage="You Must Enter Start Date"></asp:RequiredFieldValidator>
												  <br />
												  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
													  ControlToValidate="Txt_EndDate" ErrorMessage="You Must Enter End Date"></asp:RequiredFieldValidator>
												  
											   </td>
											   <td  align="center" colspan="2">
													<asp:Button ID="Btn_Show" runat="server" Text="Show" onclick="Btn_Show_Click" 
													  Class="btn btn-primary" />
													 &nbsp;
													 <asp:Button ID="Btn_ShowRFDetails" runat="server" Text="RF Details" 
														Class="btn btn-primary" onclick="Btn_ShowRFDetails_Click"/>
													 &nbsp;
													 <asp:Button ID="Btn_Excel" runat="server" onclick="Btn_Excel_Click"
																   Text="Export"  Class="btn btn-primary" />                                                         
											   </td>
											  
											  </tr>
											  <tr>
											  <td colspan="4"> 
												  <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label>
											  </td>
											  
											  </tr>
											  <tr>
											  <td colspan="4">
											  
											  <asp:Panel ID="Pnl_ExamResults" runat="server" >
											  
												<div style="padding-top:20px;width:100%; max-height:300px;overflow:auto">                  
													<center>  
													 <div   style="width:97%;">
													   <div id="AttendanceDetailsDiv" runat="server">
														 
													   </div>
													  </div>
													 </center>                   
												</div>
													 
																	   
																				  
													</asp:Panel>
												  
												  </td>
												 
												  </tr>
												   </table>

			  
			   </asp:Panel>
	   </ContentTemplate> 
	   <Triggers>
		<asp:PostBackTrigger ControlID="Btn_Excel" />
	   </Triggers>         
  </asp:UpdatePanel>  
</ContentTemplate>     

	 </ajaxToolkit:TabPanel>
	 
				</ajaxToolkit:tabcontainer>
						   
						   
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
	  
	<asp:Panel ID="Pnl_MessageBox" runat="server">
					   
						 <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
								  runat="server" CancelControlID="Btn_magok" 
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
						  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
						 <div class="container skin1" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:Black">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
			<td class="o"> </td>
			<td class="c" >
			   
				<asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label><br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_magok" runat="server" class="btn btn-info"
							 Text="OK" Width="50px"/>
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

</asp:Content>
