<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ExamDashboard.aspx.cs" Inherits="WinEr.ExamDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
		 
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
		 
	<div id="contents" style="min-height:500px;">
		 

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
	<ContentTemplate>  

<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
					 <img alt="" src="Pics/evolution-tasks.png" width="35" height="35" /></td>
				<td class="n" align="left">Exam Dashboard</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<div style="min-height:200px">
				
				

				 <table width="100%">
				  <tr>
				   <td align="right" style="width:15%">
					Class : 
				   </td>
				   <td style="width:15%">
					   <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="160px">
					   </asp:DropDownList>
				   </td>
				   <td align="right" style="width:15%">
					Exam : 
				   </td>
				   <td style="width:15%">
					  <asp:DropDownList ID="Drp_Exam" runat="server" class="form-control" Width="160px">
					   </asp:DropDownList>
				   </td>
				   <td align="right" style="width:15%">
					Status : 
				   </td>
				   <td style="width:15%">
					  <asp:DropDownList ID="Drp_Status" runat="server" class="form-control" Width="160px">
					   <asp:ListItem Text="All" Value="0"></asp:ListItem>
					   <asp:ListItem Text="Non Published" Value="1"  Selected="True"></asp:ListItem>
					  </asp:DropDownList>
				   </td>
				   <td align="center">
				   
					   <asp:Button ID="Btn_Load" runat="server" Text="Load" Class="btn btn-primary" 
						   onclick="Btn_Load_Click" />
				   
				   </td>
				  </tr>
				 </table>

				 <br />
				 
				  <center>
					  <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
				  </center>
				  <asp:GridView ID="Grd_ExamDashboard" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
					BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" AllowPaging="true" PageSize="10"
					CellPadding="3" CellSpacing="2" Font-Size="12px" Width="100%" 
						onselectedindexchanged="Grd_ExamDashboard_SelectedIndexChanged" 
						onpageindexchanging="Grd_ExamDashboard_PageIndexChanging" >
					<Columns>
						<asp:BoundField DataField="ExamScheduleId" HeaderText="ExamScheduleId" />
						<asp:BoundField DataField="ExamId" HeaderText="ExamId" />
						<asp:BoundField DataField="ClassId" HeaderText="ClassId" />
						<asp:BoundField DataField="PeriodId" HeaderText="PeriodId" />
						<asp:BoundField DataField="Page" HeaderText="Page" />
						
						<asp:BoundField DataField="ExamName" HeaderText="ExamName" />
						<asp:BoundField DataField="ClassName" HeaderText="Class" />
						<asp:BoundField DataField="Status" HeaderText="Status" />
						<asp:BoundField DataField="Description" HeaderText="Description" />
						<asp:CommandField SelectText="&lt;img src='Pics/configure1.png' width='30px' border=0 title='Continue'&gt;" 
						   ShowSelectButton="True" HeaderText="Continue"  ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center"  >
						</asp:CommandField>
					</Columns>
					<PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
					<FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
					<EditRowStyle Font-Size="Medium" />
					<SelectedRowStyle BackColor="White" ForeColor="Black" />
					<PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
					<HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
					  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
				   </asp:GridView>
					
				
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
		 
		<div class="clear">
		</div></ContentTemplate>
		</asp:UpdatePanel>
	</div>
</asp:Content>

