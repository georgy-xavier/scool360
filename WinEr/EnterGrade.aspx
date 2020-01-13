<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Inherits="EnterGrade"  Codebehind="EnterGrade.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script type="text/javascript">

		var GradeSet;
		function LoadInitialData (Gradedataset) {
			if (Gradedataset != "") {
				GradeSet = eval('(' + Gradedataset + ');');
			}
		  
		}


		function Calculate() {

			var flag = 0;
			var gridViewCtl = document.getElementById('<%=Grd_Entermarks.ClientID%>');

			for (var i = 1; i < gridViewCtl.rows.length; i++) {

				var Tx_Grade = gridViewCtl.rows[i].cells[2].children[0];
				Tx_Grade.style.backgroundColor = 'White';
				Tx_Grade.title = "Enter Mark";
				var _parsed_value = 0;
				
				if (Tx_Grade.value != "") {
					_parsed_value = Tx_Grade.value;


					var Tx_Mark = gridViewCtl.rows[i].cells[3].children[0];
					for (var l = 0; l < GradeSet.Table.length; l++) {

						if (_parsed_value == GradeSet.Table[l].Grade) {


							Tx_Mark.value = GradeSet.Table[l].AvgMark
							flag = 1;
							break;

						}
						else {
							if (_parsed_value == "-") {
								Tx_Mark.value = "a"
								flag = 1;
								break;
							}
							else if ((_parsed_value != "")) {
								flag = 0;
							}
						}

					}
				}

				if ((flag == 0) && (_parsed_value != "") && (_parsed_value != "-")) {
					alert("Enter correct Grade");
					Tx_Grade.style.backgroundColor ="Red"
				}
				else {
					flag = 0;
				}
			}
			
			
		   
		}
	</script>    
		
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
 
 
 
   <asp:Panel ID="Panel3" runat="server" DefaultButton="Btn_Update">
		  <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Enter Marks</td>
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
		<table class="style1" >
			
			<tr>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td align="right" >
					<asp:Label ID="Lbl_maxmark" runat="server" Text="Maximum Mark" class="control-label"></asp:Label>
					:</td>
				<td align="left" >
					<asp:TextBox ID="Txt_Maxmark" runat="server" ReadOnly="True" Width="128px" class="form-control"></asp:TextBox>
					<br />
				</td>
				<td align="right" >
					Subject :</td>
				<td >
					<asp:DropDownList ID="Drp_Subject" runat="server" AutoPostBack="True" class="form-control"
						 onselectedindexchanged="Drp_Subject_SelectedIndexChanged" 
						Width="160px">
					</asp:DropDownList>
				</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td >
					&nbsp;</td>
				<td >
					&nbsp;</td>
				<td >
					&nbsp;</td>
				<td >
					<asp:TextBox ID="Txt_markCol" runat="server" Visible="False" class="form-control"></asp:TextBox>
				</td>
				<td>
					&nbsp;</td>
			</tr>
			<tr>
				<td >
					</td>
				<td >
					
					</td>
				<td align="right">
					<asp:Button ID="Btn_Update" runat="server" onclick="Btn_Update_Click1" Class="btn btn-success" 
						Text="Save"  />
					
				</td>
				<td align="left">
					&nbsp;&nbsp;
					<asp:Button ID="Btn_Undo" runat="server" onclick="Btn_Undo_Click" Text="Reset" Class="btn btn-danger" 
						/>
				</td>
				<td >
				
					  &nbsp;</td>
			</tr>
			
		</table>
				   
					
					
				  
		<asp:Panel ID="PanelError" runat="server" Visible="False">
	<div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/restore.png" 
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
	
				<asp:Panel ID="Pnl_studentlist" runat="server">
	   <div class="roundbox">
						<table width="100%">
						<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
						<tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
	   <img alt="" src="elements/users.png" width="45" height="45" /></td>
	<td><h3>Student List</h3></td>
		<td style="text-align:right;visibility:hidden;">GradeReport
		<asp:ImageButton ID="Btn_GradeExportToExcel" runat="server"  Width="35" Height="35" ToolTip="Export To Excel"
		   ImageUrl="Pics/Excel.png" onclick="Btn_GradeExportToExcel_Click" />
		MarkReport
		<asp:ImageButton ID="Btn_exporttoexel" runat="server"  Width="35" Height="35" ToolTip="Export To Excel"
			onclick="Btn_exporttoexel_Click"  ImageUrl="Pics/Excel.png" />
		 </td>
	</tr></table>
		
<div class="linestyle"></div>             
		<div style=" overflow:auto;max-height: 400px;">
						<asp:GridView ID="Grd_Entermarks" runat="server" AutoGenerateColumns="False" 
							Width="97%" OnRowDataBound="Grd_Entermarks_RowDataBound"
						   BackColor="#EBEBEB"
				   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
				   CellPadding="3" CellSpacing="2" Font-Size="12px">
							<Columns>
								<asp:BoundField DataField="Id" HeaderText="Student Id" />
								<asp:BoundField DataField="StudentName" HeaderText="Student Name" />
								<asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="100" />
								<asp:TemplateField HeaderText="Grade" ItemStyle-Width="100">
									<ItemTemplate>
										<asp:TextBox ID="Txt_Grade" runat="server" Height="20" MaxLength="6" class="form-control" onkeyup="Calculate()"
							   Width="100"></asp:TextBox>
										<ajaxToolkit:FilteredTextBoxExtender ID="Txt_Grade_FilteredTextBoxExtender" 
											runat="server" Enabled="True" FilterType= "Custom, UppercaseLetters ,Numbers" 
											TargetControlID="Txt_Grade" ValidChars="-+">
										</ajaxToolkit:FilteredTextBoxExtender>
									</ItemTemplate>
								</asp:TemplateField>
								 <asp:TemplateField HeaderText="Average %" ItemStyle-Width="100">
									<ItemTemplate>
										<asp:TextBox ID="Txt_Mark" runat="server" Height="20" MaxLength="6"  class="form-control" Enabled="false"
											Width="100"></asp:TextBox>
										<ajaxToolkit:FilteredTextBoxExtender ID="Txt_Mark_FilteredTextBoxExtender" 
											runat="server"  FilterType="Custom, Numbers" 
											TargetControlID="Txt_Mark" ValidChars=".a">
										</ajaxToolkit:FilteredTextBoxExtender>
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
	<Triggers>
			<asp:PostBackTrigger ControlID="Btn_exporttoexel" />
		   <asp:PostBackTrigger ControlID="Btn_GradeExportToExcel" />
   </Triggers>
	
 </asp:UpdatePanel>
   
</div>

<div class="clear"></div>
</div>
</asp:Content>

