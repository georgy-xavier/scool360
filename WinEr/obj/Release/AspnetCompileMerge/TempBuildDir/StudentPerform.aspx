<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Codebehind="StudentPerform.aspx.cs" Inherits="StudentPerform"  %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	  <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>


	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
				<div class="card0">
					<div class="cardHd">Student Exam Performance</div>
					<div class="row stntStripBody">
			            <div class="_stdntTopStrip"></div>
	                </div>
				</div>
			</div>
			<br>
			 <div class="row">
				<div class="card0" >
					<div class="card0" >
						<table  class="containerTable">
			<%--<tr >
				<td class="no"> </td>
				<td class="n">Student performance</td>
				<td class="ne"> </td>
			</tr>--%>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					  <br />
				<table width="100%">
				<tr align="right">
				<td >
					<asp:ImageButton ID="Img_Search" runat="server" ImageUrl="~/Pics/book_search.png" ImageAlign="AbsMiddle" OnClick="Lnk_PreviousPerformance_Click" Width="30px" Height="30px"/>
				 <asp:LinkButton ID="Lnk_PreviousPerformance" runat="server" 
						Text="Previous Performance" onclick="Lnk_PreviousPerformance_Click"></asp:LinkButton>
						</td>
				</tr>
				</table>
		
		<ajaxToolkit:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_yuitabview-theme"  ActiveTabIndex="0"
						  Width="100%"  >
										
										<ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Signature and Bio">
											<HeaderTemplate>
											 <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /> <b> PERIODWISE</b></HeaderTemplate>
											<ContentTemplate>
												<asp:Panel ID="Pnl_Induexam" runat="server" Height="500px">
					
					<br/>
					  <asp:Label ID="Lbl_indexammsg" runat="server"></asp:Label>
					<asp:GridView ID="Grd_ExamList" runat="server" AllowPaging="True"  AutoGenerateColumns="False"
		CellPadding="4" ForeColor="Black" GridLines="Vertical" 
		onpageindexchanging="Grd_ExamList_PageIndexChanging" OnRowDeleting="Grd_ExamList_RowDeleting" 
		onselectedindexchanged="Grd_ExamList_SelectedIndexChanged" 
		Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
														>
	   
		<Columns>
		   <asp:BoundField DataField="ExamSchId" HeaderText="Id" />
			<asp:BoundField DataField="ExamSchId" HeaderText="Id" />
			<asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
			<asp:BoundField DataField="Period"  HeaderText="Period" />
			<asp:CommandField ShowDeleteButton="True" DeleteText="&lt;img src='Pics/ViewPdf.png' width='30px' border=0 &gt;" HeaderText="Pdf Report" />
			 <asp:CommandField ShowSelectButton="True" SelectText="&lt;img src='Pics/full_page.png' width='30px' border=0 &gt;"  HeaderText="Html Report" />
			  
		</Columns>
		   <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
														  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
														  <HeaderStyle BackColor="#E9E9E9" 
							Font-Bold="True" Font-Size="11px" 
							ForeColor="Black"
																												
							HorizontalAlign="Left" />
														   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
																											HorizontalAlign="Left" />
																											
					   <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
						  <EditRowStyle Font-Size="Medium" /> 
	</asp:GridView>
					
					</asp:Panel>
											</ContentTemplate>
										</ajaxToolkit:TabPanel>
									 
									 
										<ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
											<HeaderTemplate>
											 <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /> <b>CONSOLIDATE</b></HeaderTemplate>
											<ContentTemplate>
												<asp:Panel ID="ConsolidateRpt" runat="server">
												<br />
												
											 
												
												<table width="100%">
												<tr>
												<td>&nbsp;</td>
												<td>
													&nbsp;</td>
												
												
												</tr>
												
												 <tr>
												<td class="style1">Exam Type
												</td>
												<td>
											   <asp:DropDownList ID="Drp_ExamType" runat="server" AutoPostBack="True" Width="160px" class="form-control"
																	onselectedindexchanged="Drp_Examlist_SelectedindexChanged" >
																</asp:DropDownList>
												</td>
												
												
												</tr>
												
												<tr>
												<td  class="style1">Exam </td>
												 <td >
																<asp:DropDownList ID="Drp_Exam" runat="server" AutoPostBack="True" class="form-control"
																	OnSelectedIndexChanged="Drp_Exam_SelectedindexChanged" Width="160px">
																</asp:DropDownList>
													</td>
															
														</tr>
														
														<tr>
														<td>
																 &nbsp;</td>
																<td>
																	&nbsp;</td>
														
														</tr>
														
													<tr>
														<td>
														</td>
														<td>
															<asp:Button ID="Btn_Report" runat="server" OnClick="Btn_Report_Click" class="btn btn-primary"
																Text="Generate"  />  
																&nbsp;&nbsp;&nbsp;&nbsp;
																<asp:ImageButton ID="Img_Export" runat="server" Height="25px" 
																ImageUrl="~/Pics/Excel-icon.png" OnClick="Img_Export_Click" 
																ToolTip="Export to Excel" />&nbsp;&nbsp;&nbsp;
															<asp:ImageButton ID="Img_PdfExport" runat="server" Height="25px" 
																ImageUrl="~/Pics/ViewPdf.png" OnClick="Img_PdfExport_Click"  
																TabIndex="1" ToolTip="Export to PDF" />
														</td>
													</tr>
													<tr>
														<td colspan="2" align="center">
															<asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label></td>
													</tr>
													<tr>
														<td>
															
														</td>
														<td>
													  
														</td>
													</tr>
														
												</table>
												
											   
											  
												<asp:Panel ID="Pnl_ExamGraph" runat="server" Visible="False">
												<br />
												<div class="newsubheading">
												Performance Chart.
													</div>
															 <div class="linestyle">                  
											</div>
											
											 <br />
												  Select Condition:  <asp:DropDownList ID="Drp_SelectList" runat="server" 
														AutoPostBack="True" class="form-control"
														onselectedindexchanged="Drp_SelectList_SelectedIndexChanged" Width="120px">
													</asp:DropDownList> <br /><br />
												<web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart"
											   runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
													Width="440px" YCustomStart="0" YValuesInterval="0"><Background Color="LightSteelBlue" /><ChartTitle StringFormat="Center,Near,Character,LineLimit"  /><XAxisFont StringFormat="Center,Near,Character,LineLimit" /><YAxisFont StringFormat="Far,Near,Character,LineLimit" /><XTitle StringFormat="Center,Near,Character,LineLimit" /><YTitle StringFormat="Center,Near,Character,LineLimit" /></web:chartcontrol>
												 </asp:Panel>
												
													
													<br />
													
												  <asp:Panel ID="Pnl_EportGrid"  runat="server">
												  <div >
												   
														<asp:GridView ID="EportGrid" runat="server" 
														CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" 
														BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
		   
														   <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
														  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
														  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
																												HorizontalAlign="Left" />
														   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
																											HorizontalAlign="Left" />
																											
					   <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
						  <EditRowStyle Font-Size="Medium" /> 
														</asp:GridView>
												   </div>
												 </asp:Panel>
													
												</asp:Panel>
												
												
											</ContentTemplate>
											
										</ajaxToolkit:TabPanel>
		   </ajaxToolkit:TabContainer>	
	
					
					
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
		  </div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>		  
	 <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
</asp:Content>

