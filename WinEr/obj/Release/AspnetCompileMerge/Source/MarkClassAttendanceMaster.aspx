<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MarkClassAttendanceMaster.aspx.cs" Inherits="WinEr.MarkClassAttendanceMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .Nextmonth
 {
	 padding:0px 10px 0px 10px;
 }
	 @media screen and (max-device-width: 790px) {
	 
	 #right{
		 display:none !important;
	 }

	 }
	 #topstrip{
		 margin-bottom:20px;
	 }
 </style>
 <script type="text/javascript">

	 function LoadPopup() {

		var modalPopupBehavior = $find('programmaticModalPopupBehavior');
		modalPopupBehavior.show();
	}

	function LoadFuturePopup() {

		var modalPopupBehavior = $find('futureprogrammaticModalPopupBehavior');
		modalPopupBehavior.show();
	} 

	function LoadnotBatchPopup() {

		var modalPopupBehavior = $find('NotBatchprogrammaticModalPopupBehavior');
		modalPopupBehavior.show();
	}

	$(function () {
		$("_submnLdr").html(circleLoader);
		loadMarkAttendanceSubMenu();
	});

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
	<div class="container-fluid cudtomContFluid">   
		
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
				<div class="card0">
					<div class="cardHd">Student Attendance Form</div>
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
 <center>  

<div class="card0" >
		<table class="containerTable">
				<td class="c" >
				
				  <center> 

				  
				  <div id="topstrip">
						<table  width="100%" style="color:White">
						 <tr>                 
							<td align="right" style="width:40%" >
								<asp:Label ID="Label1" runat="server" Text="Class Name :  " class="control-label"></asp:Label>
							</td>
							<td align="left"  style="width:60%" >

								<asp:DropDownList ID="Drp_ClassName" runat="server" Width="180px" class="form-control" AutoPostBack="true"
									onselectedindexchanged="Drp_ClassName_SelectedIndexChanged" >
								 </asp:DropDownList>
							</td>
							</tr>
							</table>
						  
						 </div>
				  
				   <asp:Calendar ID="Calendar1" runat="server"
									onselectionchanged="Calendar1_SelectionChanged" BackColor="White"  CssClass="mark_attendanceCal_Global"
									BorderColor="Black" BorderStyle="Solid" CellSpacing="0" 
									Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth" 
									ondayrender="Calendar1_DayRender"  
									onvisiblemonthchanged="Calendar1_VisibleMonthChanged">
									<SelectedDayStyle BackColor="#f9f7aa" ForeColor="Black" />
									<TodayDayStyle BackColor="White" ForeColor="Black"  BorderColor="Red" BorderWidth="2" BorderStyle="Solid"/>
									<OtherMonthDayStyle ForeColor="#999999"/>
									<DayStyle BackColor="White"  BorderColor="Black" BorderWidth="1" BorderStyle="Solid" CssClass="mark_attendanceCalender"/>
									<NextPrevStyle  Font-Size="8pt" ForeColor="Black"  CssClass="Nextmonth"  />
									<DayHeaderStyle  Font-Size="8pt" ForeColor="#333333"  CssClass="mark_attendanceCal_Week"
										Height="50px" />
									<TitleStyle BackColor="#ffffff" BorderStyle="Solid" Height="50px" BorderColor="Black" BorderWidth="1"  
										Font-Size="12pt" ForeColor="Black"  />
								</asp:Calendar>
						<hr>	
					  
					   <div class="row">
							<div class="col-md-2 col-xs-6"><span class="calStyleMap" style="background-color:red;"></span><div>Attendance Not Marked</div></div>
							<div class="col-md-3 col-xs-6"><span class="calStyleMap" style="background-color:#a4d805;"></span><div>Attendance Marked Full Day</div></div>
							<div class="col-md-2 col-xs-6"><span class="calStyleMap" style="background-color:#ffcc00;"></span><div>Holiday</div></div>
							<div class="col-md-2 col-xs-6"><span class="calStyleMap" style="background-color:#ffc1c1;"></span><div>Day not in current Batch</div></div>
							<div class="col-md-3 col-xs-6"><span class="calStyleMap" style="background-color:#66ccff;"></span><div> Attendance Marked Half Day</div></div>
					   </div>	
					  <hr>
					  <div class="row">
									<asp:LinkButton ID="Lnk_Unmarked" runat="server" ToolTip="Goto Unmarked Day" onclick="Lnk_Unmarked_Click"></asp:LinkButton>
									<asp:LinkButton ID="Lnk_Today" runat="server" ToolTip="Goto Today" onclick="Lnk_Today_Click" >Goto Today</asp:LinkButton>
									<asp:Label ID="lbldate" runat="server" Text=""></asp:Label>
							</div>
					  <hr>
				   </center>   
					
				</td>
		</table>
	</div>
	
	</center> 
	<asp:Panel ID="Pnl_MessageBox" runat="server">
					   
   <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok" 
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground" BehaviorID="programmaticModalPopupBehavior" />
   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:Black">Message</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
				<center>
				<asp:Label ID="Lbl_msg" runat="server" Text="Selected day is holiday" Font-Bold="true"></asp:Label>
				</center>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
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
	
	
	
	<asp:Panel ID="Panel1" runat="server">
					   
   <asp:Button runat="server" ID="Button1" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="M_Future"  runat="server" CancelControlID="Btn_ok" 
								  PopupControlID="Panel2" TargetControlID="Button1" BackgroundCssClass="modalBackground" BehaviorID="futureprogrammaticModalPopupBehavior" />
   <asp:Panel ID="Panel2" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
						Height="28px" Width="29px" /> </td>
			<td class="n"><span style="color:Black">Message</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
				<center>
				<asp:Label ID="Label2" runat="server" Text="You have selected a future day" Font-Bold="true"></asp:Label>
				</center>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_ok" runat="server" Text="OK" class = "btn btn-primary" Width="50px"/>
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
	
	
	<asp:Panel ID="Panel3" runat="server">
					   
   <asp:Button runat="server" ID="Button2" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1"  runat="server" CancelControlID="Btn_ok" 
								  PopupControlID="Panel4" TargetControlID="Button2"  BackgroundCssClass="modalBackground" BehaviorID="NotBatchprogrammaticModalPopupBehavior" />
   <asp:Panel ID="Panel4" runat="server" style="display:none;">
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
				<center>
				<asp:Label ID="Label3" runat="server" Text="Selected date is not within current batch" Font-Bold="true"></asp:Label>
				</center>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Button3" runat="server" Text="OK" class = "btn btn-primary" Width="50px"/>
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





	  
  <asp:HiddenField ID="Hidden_StandardId" runat="server" />

					 
				
 </ContentTemplate>
 </asp:UpdatePanel>               
				</div>
			</div>
		</div>
		<div class="col-lg-2 col-md-2">
			<div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" class="_submnLdr"></div></div></div>         
		 </div>	
	</div>
			  
</asp:Content>
