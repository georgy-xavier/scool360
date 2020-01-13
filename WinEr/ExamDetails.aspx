<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Inherits="ExamDetails"  Codebehind="ExamDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style type="text/css">
		.style6
		{
			height: 29px;
		}
		</style>
		
		<script type="text/javascript">
		 function CancelClick(){
		 }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
	
  <asp:UpdatePanel ID="pnlAjaxUpdaet"   runat="server" >
   <ContentTemplate>
   

	
	<div id="contents">
		<div id="right">
			<div class="label">Exam Info</div>
			<div id="SubExammngMenu" runat="server">		
			</div>
		</div>
		<div id="left">
			 
				<div class="container skin1" >
					<table cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"> </td>
							<td class="n">Exam Details</td>
							<td class="ne"> </td>
						</tr>
						<tr >
							<td class="o"> </td>
							<td class="c" >
				 <div id="topstrip">
				 <asp:Panel ID="Panel1" runat="server">
									<table style="width:100%;" >
										<tr>
											
											<td style="width: 266px;">
												<asp:Label ID="Lbl_ExamName" runat="server"  ForeColor="White" class="control-label"
													Text="University Exam"></asp:Label>
											</td>
                                            <td>&nbsp;</td>
                                            
											
										</tr>
                                      </table>
                                        <table style="width:100%;" >
										<tr>
											<td >&nbsp;</td>
                                            <td >&nbsp;</td>
											<td style="width:70px;"> 
												<asp:Label ID="Lbl_Examtypelb" runat="server" ForeColor="White" class="control-label"
													Text="Exam Type :"></asp:Label>
											</td>
                                           
                                            <td >&nbsp;</td>
											<td>
												<asp:Label ID="Lbl_ExamType" runat="server" ForeColor="White" class="control-label"
													Text="MAIN"></asp:Label>
											</td>                                            
                                            
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                            <td align="right">
												<asp:Label ID="Lbl_freqlb" runat="server" class="control-label"
													Text="Exam Frequency :" ForeColor="White"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                           
											<td>
												<asp:Label ID="Lbl_Frequency" runat="server" ForeColor="White" class="control-label"
													Text="Monthly"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td align="right" >
												<asp:Label ID="Label1" runat="server" ForeColor="White" Text="Class :" class="control-label"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                            
											<td>
												<asp:DropDownList ID="DrpClassName" runat="server" Width="165px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpClassName_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
                                            <td align="right">
												<asp:Label ID="Label2" runat="server" ForeColor="White" Text="Period :" class="control-label"></asp:Label>
											</td>                                           
                                            <td >&nbsp;</td>
											<td>
												<asp:DropDownList ID="DrpExamPeriod" runat="server" Width="140px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpExamPeriod_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
											
										</tr>
									
										<tr>
					 <td class="leftside"><br /></td>
					 <td class="rightside"><br /></td>
					 </tr>
										
									</table>
								</asp:Panel>
				 
				 
				 </div>
								
	
								<asp:Panel ID="Pnl_note" runat="server" Font-Bold="True">
									<div class="container skin6"  >
										<table cellpadding="0" cellspacing="0" class="containerTable">
											<tr>
												<td class="no">
													<asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" Height="28px" Width="29px" />
												</td>
												<td class="n">
													Note
												</td>
												<td class="ne"> </td>
											</tr>
											<tr>
												<td class="o"></td>
												<td class="c" >
					
													<br />
													<asp:Label ID="Lbl_Not" runat="server" style="color: #FF0000" class="control-label"
															   Text="">
													</asp:Label>
													&nbsp;<asp:LinkButton ID="lnk_manageExam" runat="server"
														onclick="lnk_manageExam_Click">Map Now</asp:LinkButton>
														<asp:LinkButton ID="lnk_ScheduleExam" runat="server"
														onclick="lnk_Schedule_Click">Schedule Now</asp:LinkButton>
														
														<asp:LinkButton ID="lnk_mark" runat="server"
														onclick="lnk_mark_Click">Enter Marks</asp:LinkButton>
														
														<asp:LinkButton ID="lnk_report" runat="server"
														onclick="lnk_report_Click">Generate Report </asp:LinkButton>
													<br />
												
												</td>
												<td class="e"> </td>
											</tr>
											<tr >
												<td class="so"></td>
												<td class="s"></td>
												<td class="se"></td>
											</tr>
										</table>
									</div>
								</asp:Panel>
								
								<asp:Panel ID="Pnl_schedul" runat="server" >
								
				   <div class="roundbox">
					<table width="100%" >
						<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
						<tr><td class="centerleft"></td><td class="centermiddle">              
	<table width="100%"><tr>
	<td style="width:48px;">
		<img alt="" src="Pics/timetable.gif" width="45" height="45" /></td>
	<td><h3>Time Table</h3></td>
	<td style="text-align:right;">
		 <asp:Button ID="Btn_admitcard" runat="server" onclick="Btn_generatadmitcard" class="btn btn-primary" Visible="true"
					 style="margin-right: 10px; padding-right:10px" Text="Generate Admit Card"    />
        <asp:Button ID="Btn_supplimentaryexam" runat="server" onclick="lnk_supplimentaryexam_Click"  class="btn btn-primary" Visible="false"
					 style="margin-right: 10px; padding-right:10px" Text="Supplimentary Exam"    />
      
		<asp:ImageButton ID="Btn_exporttoexel" runat="server"  Width="35" Height="35" ToolTip="Export To Excel"
			  ImageUrl="Pics/Excel.png" onclick="Btn_exporttoexel_Click" /></td>
	</tr></table>
		
<div class="linestyle"></div>
		<asp:GridView ID="Grd_ExamSchdule" runat="server" AutoGenerateColumns="False" 
		   BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px" Width="100%" >
			<Columns>
				<asp:BoundField DataField="subject_name" HeaderText="Subject" />
				<asp:BoundField DataField="SubjectCode" HeaderText="Subject Code" ItemStyle-Width="100px" />
				<asp:BoundField DataField="ExamDate" HeaderText="Exam Date" ItemStyle-Width="75px" />
				<asp:BoundField DataField="StartTime" HeaderText="Start Time" ItemStyle-Width="75px" />
				<asp:BoundField DataField="EndTime" HeaderText="End Time" ItemStyle-Width="75px" />
				<asp:BoundField DataField="MinMark" HeaderText="Pass Mark" ItemStyle-Width="50px" />
				<asp:BoundField DataField="MaxMark" HeaderText="Max Mark" ItemStyle-Width="50px" />
			</Columns>
			 <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
				  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
				  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
		   </asp:GridView>
						</td><td class="centerright"></td></tr>
						<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
						</table>
						</div>
						
		 
	  <%--<div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Img_time" runat="server" ImageUrl="~/elements/time.png" 
						Height="28px" Width="29px" /> </td>
				<td class="n">Exam Time Table</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					
		
	   
	
					
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%></asp:Panel>


<asp:Panel ID="PanelExamStudaDetails" runat="server" > 
<div class="roundbox">
<table width="100%">
<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
<tr><td class="centerleft"></td><td class="centermiddle">              
  <table width="100%"><tr>
   <td style="width:48px;">
	  <img alt="" src="Pics/chart.png" width="45" height="45" /></td>
	<td><h3>Exam Report</h3></td>
	<td style="text-align:right;">
			<asp:Label ID="Lbl_PublishMessage"  runat="server" Font-Size="Large" 
				ForeColor="#0066FF"></asp:Label>
			<asp:Button ID="Btn_Publish" runat="server" onclick="Btn_Publish_Click" class="btn btn-primary" Visible="false"
					 style="margin-left: 10px" Text="Publish"    />
					  <ajaxToolkit:ConfirmButtonExtender ID="Btn_Publish_ConfirmButtonExtender"  OnClientCancel="CancelClick"
											runat="server" ConfirmText="Are you sure you want to publish the result" Enabled="True" TargetControlID="Btn_Publish">
					  </ajaxToolkit:ConfirmButtonExtender>&nbsp;&nbsp;
			<asp:ImageButton ID="ImageButton1" runat="server" onclick="Btn_Exp2_Click"  ImageUrl="Pics/Excel.png" 
			  Width="35" Height="35" ToolTip="Export To Excel"/>
   </td>
 </tr>
</table>
		
<div class="linestyle"></div>
	   

			<asp:GridView ID="Grd_CreateReport" runat="server" AutoGenerateColumns="False"
						   BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px"
							Width="100%"  AllowSorting="True" 
						onsorting="Grd_CreateReport_Sorting" OnRowDataBound="Grd_CreateReport_RowDataBound">
				<Columns>
				

					<asp:BoundField DataField="Id" HeaderText="Student Id" />
					<asp:BoundField DataField="StudentName" HeaderText="Student Name" SortExpression="StudentName"> </asp:BoundField>
					<asp:BoundField DataField="RollNo" HeaderText="Roll No" SortExpression="RollNo" ItemStyle-Width="40px" />
					<asp:BoundField DataField="TotalMark" HeaderText="Total Mark" SortExpression="TotalMark" ItemStyle-Width="50px" />
					<asp:BoundField DataField="TotalMax" HeaderText="Max Mark" ItemStyle-Width="50px" />
					<asp:BoundField DataField="Avg" HeaderText="Avg." SortExpression="Avg" ItemStyle-Width="50px" />
					<asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" ItemStyle-Width="40px" />
					<asp:BoundField DataField="Result" HeaderText="Result" SortExpression="Result" ItemStyle-Width="75px" />
					<asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" ItemStyle-Width="40px" />
					<asp:BoundField DataField="Remark" HeaderText="Remarks" SortExpression="Remark" ItemStyle-Width="150px" />
				   
				</Columns>
			   <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
				  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
				  <EditRowStyle Font-Size="Medium" />
				  <SelectedRowStyle BackColor="White" ForeColor="Black" />
				  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
				  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
				  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
			</asp:GridView>
	   
		
						</td><td class="centerright"></td></tr>
						<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
						</table>
						</div> 
						   
						
						   
		   <%--<div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/users.png" 
						Height="28px" Width="29px" /> </td>
				<td class="n"></td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
				
	   
	  
			
	
					
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%></asp:Panel>    
   
   
  
   
	
		
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
		<div class="clear"></div>
	</div>
	
	
 <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none" class="btn btn-info"/>
	   <ajaxToolkit:ModalPopupExtender ID="MPE_PublishExam"  runat="server" PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground"  />
		   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;"> <%-- style="display:none;"--%>
				<div class="container skin1" style="width:400px; top:400px;left:200px" >
					<table   cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
							</td>
							<td class="n"><span style="color:Black">Publish Exam Report</span></td>
							<td class="ne">&nbsp;</td>
					   </tr>
					   <tr >
							<td class="o"> </td>
							<td class="c" >    
								
								<table cellspacing="10" width="100%">
								 <tr>
								   <td>
									   <asp:CheckBox ID="Chk_PublishSMS" runat="server" Text="Send SMS" />
								   </td>
								 </tr>
								 <tr>
								   <td class="style6">
									   <asp:CheckBox ID="Chk_PublishIncident" runat="server" Text="Store Incidents" />
								   </td>
								 </tr>
								 <tr>
								  <td align="center">
									<asp:Button ID="Btn_ok" runat="server" Text="OK"  Class="btn btn-primary" 
										  onclick="Btn_ok_Click"/>
								  </td>
								 </tr>
								</table>           
								<asp:Label ID="lbl_examschuleId" runat="server" Text="" Visible="false" class="control-label"></asp:Label>
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
		 
		
		  <asp:Button runat="server" ID="Button1" style="display:none"/>
	   <ajaxToolkit:ModalPopupExtender ID="Message"  runat="server" PopupControlID="Panel2" CancelControlID="Button2" TargetControlID="Button1" BackgroundCssClass="modalBackground"  />
		   <asp:Panel ID="Panel2" runat="server" style="display:none;" >  
				<div class="container skin1" style="width:400px; top:400px;left:200px" >
					<table   cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
							</td>
							<td class="n"><span style="color:Black">Alert!</span></td>
							<td class="ne">&nbsp;</td>
					   </tr>
					   <tr >
							<td class="o"> </td>
							<td class="c" >    
								
								<table cellspacing="10" width="100%">
								 <tr>
								   <td>
									   <asp:Label ID="Label4" runat="server" class="control-label" Text="You cannot publish exam report without selecting any method"></asp:Label>
								   </td>
								 </tr>
								 <tr>
								  <td align="center">
									<asp:Button ID="Button2" runat="server" Text="OK"  Class="btn btn-primary" />
								  </td>
								 </tr>
								</table>           
								<asp:Label ID="Label3" runat="server" Text="" Visible="false" class="control-label"></asp:Label>
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
		 
		 
		 
	  </ContentTemplate>
	  <Triggers>
	  
	   <asp:PostBackTrigger ControlID="Btn_exporttoexel" />
	   <asp:PostBackTrigger ControlID="ImageButton1" />
	  </Triggers>
  </asp:UpdatePanel>  
</asp:Content>