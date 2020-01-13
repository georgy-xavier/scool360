<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="FailedStudentReport"  Codebehind="FailedStudentReport.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
		.style4
		{
		}
		.style5
		{
			width: 139px;
		}
		.style6
		{
			width: 136px;
		}
		.style7
		{
			width: 283px;
		}
		.style9
		{
			width: 144px;
		}
		</style>
		
		<script type="text/javascript" language="javascript">
		    function CancelClick() {
		    }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div id="contents" style="min-width:1000px;">

<div id="right">

<div class="label">Exam Info</div>
<div id="SubExammngMenu" runat="server">
		
 </div>
</div>

<div id="left" >

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
				<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
				<ContentTemplate> 
  
   
	
	
	
	
	   
		
	<asp:Panel ID="Panel3" runat="server" >    
	<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Generate Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				
				<div id="topstrip">
				 <asp:Panel ID="Panel2" runat="server">
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
				<table width="100%">
				<tr>
				<td></td>
				<td style="text-align:right;">&nbsp;</td>
				</tr>
				
				<tr>
				<td></td>
				<td style="text-align:right;">

                    <asp:Button ID="Btn_supplimentaryreport" runat="server" onclick="lnk_supplimentaryreport_Click" Text="Supplimentary Report" visible="false"
					   class="btn btn-primary" />
                   <%-- <asp:LinkButton ID="lnk_supplimentaryreport" runat="server" Visible="false" onclick="lnk_supplimentaryreport_Click">Supplimentary Report</asp:LinkButton>--%>
				  <asp:Button ID="Btn_Gen" runat="server" onclick="Btn_Gen_Click" Text="Generate" 
					   class="btn btn-primary" />&nbsp;&nbsp;
					<asp:Button ID="Btn_Update" runat="server" onclick="Btn_Update_Click" 
						style="margin-left: 0px" Text="Save"  Class="btn btn-primary"  />
					
				</td>
				</tr>
				
				<tr>
				<td></td>
				<td style="text-align:right;"></td>
				</tr>
				</table>
				
	
	
	<asp:Panel ID="PanelError" runat="server" Visible="False">
	<div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/restore.png" 
						Height="28px" Width="29px" /> </td>
				<td class="n">Note</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
				<br />
					 <asp:Label ID="Lbl_message" runat="server" Font-Bold="True" ForeColor="Red" class="control-label"></asp:Label>
					 
					   &nbsp;<asp:LinkButton ID="lnk_manageExam" runat="server" onclick="lnk_manageExam_Click">Map Now</asp:LinkButton>
					   <asp:LinkButton ID="lnk_ScheduleExam" runat="server" onclick="lnk_Schedule_Click">Schedule Now</asp:LinkButton>
					   <asp:LinkButton ID="lnk_Marks" runat="server" onclick="lnk_Marks_Click">Enter Marks</asp:LinkButton>
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
	
	
					<asp:Panel ID="Pnl_report" runat="server">
					
				 <div class="roundbox">
						<table width="100%">
						<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
						<tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
	   <img alt="" src="elements/users.png" width="45" height="45" /></td>
	<td><h3>Student Report</h3></td>
	<td style="text-align:right;">
	  <asp:ImageButton ID="Img_ExportToExcel" runat="server" onclick="Img_ExportToExcel_Click"  ImageUrl="Pics/Excel.png" 
			  Width="35" Height="35" ToolTip="Export To Excel"/>
		 
		 </td>
	</tr></table>
		
<div class="linestyle"></div>     
		 
		 <asp:Panel ID="Pnl_studreport" runat="server">
						
				  <div style=" overflow:auto;max-height: 400px;">
						<asp:GridView ID="Grd_CreateReport" runat="server" AutoGenerateColumns="False"  OnRowDataBound="Grd_CreateReport_RowDataBound"
							Width="97%" BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px">
							<Columns>
								<asp:BoundField DataField="Id" HeaderText="Student Id" />
								<asp:BoundField DataField="StudentName" HeaderText="Student Name" />
								<asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="50px" />
								<asp:TemplateField HeaderText="Total Mark" ItemStyle-Width="50px">
									<ItemTemplate>
										
										<asp:Label ID="Lbl_Total" runat="server" Height="20" Width="45" class="control-label"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Max Mark" ItemStyle-Width="50px">
									<ItemTemplate>
										
										  <asp:Label ID="Lbl_Max" runat="server" Height="20" Width="45" class="control-label"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Average" ItemStyle-Width="40px">
									<ItemTemplate>
									   
										 <asp:Label ID="Lbl_Avg" runat="server" Height="20" Width="45" class="control-label"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Grade" ItemStyle-Width="40px">
									<ItemTemplate>
									   
										<asp:Label ID="Lbl_Grade" runat="server" Height="20" Width="45" class="control-label"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Result" ItemStyle-Width="75px">
									<ItemTemplate>
									   
										  <asp:Label ID="Lbl_Result" runat="server" Height="20" Width="60" class="control-label"></asp:Label> 
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Rank" ItemStyle-Width="40px">
									<ItemTemplate>
										
										 <asp:Label ID="Lbl_Rank" runat="server" Height="20" Width="45" class="control-label"></asp:Label> 
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Remarks" ItemStyle-Width="120px">
									<ItemTemplate>
										<asp:TextBox ID="Txt_Remark" runat="server" Height="20" Width="120" MaxLength="100" class="form-control"></asp:TextBox>
										 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
											 runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'/\" 
										  TargetControlID="Txt_Remark" />
										
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
					</div>
		   </asp:Panel>
		 
		  </td><td class="centerright"></td></tr>
						<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
						</table>
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
	
	 </asp:Panel>
	
	 
		
			
<WC:MSGBOX id="WC_MessageBox" runat="server" />   
	
	  </ContentTemplate>
	  <Triggers><asp:PostBackTrigger ControlID="Img_ExportToExcel" /></Triggers>
 </asp:UpdatePanel> 
</div>

<div class="clear"></div>
</div>

</asp:Content>