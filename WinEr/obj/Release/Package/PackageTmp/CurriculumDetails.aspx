<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="CurriculumDetails.aspx.cs" Inherits="CurriculumDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	<script type="text/javascript">
		function openIncpopup(strOpen) {
			open(strOpen, "Info", "status=1, width=600, height=450,resizable = 1");
		}
		function openIncedents(strOpen) {
			open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
		}
		$(function () {
		    studentDetails.getSubMenu();
		    studentDetails.getTopDt();
		});
	</script>
	<style type="text/css">
		.IncBlock {
			height: 220px;
		}

			.IncBlock a {
				color: #546078;
				text-decoration: none;
			}
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	  <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
			
	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
			<div class="card0">
		<div class="cardHd">Student Curriculum Details</div>
		<div class="row stntStripBody">
			<div class="_stdntTopStrip"></div>
	    </div>
	</div>
		</div>
			<br> 
			<div class="row"> 
			<div class="card0">
				<table class="containerTable">
					<%--<tr>
						<td class="no"></td>
						<td class="n">Curriculum Details</td>
						<td class="ne"></td>
					</tr>--%>
					<tr>
						<td class="o" style="height: 65px"></td>
						<td class="c" style="height: 65px">
							<h4><span style="color: #366092;">Admission Details </span></h4>
							<div class="linestyle"></div>
							<asp:Panel Width="100%" runat="server" ID="pnl1">
								<table class="tablelist">
									<%--style="height:200px;"--%>
									<tr>
										<td class="leftside">Date Of Joining</td>
										<td class="rightside">
											<asp:Label ID="lbl_DOJ" runat="server" Text="10/10/2010" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Joining Batch</td>
										<td class="rightside">
											<asp:Label ID="lbl_JnBatch" runat="server" Text="2008-2010" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Joining Standard</td>
										<td class="rightside">
											<asp:Label ID="lbl_JnStd" runat="server" Text="A" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Joining Class</td>
										<td class="rightside">
											<asp:Label ID="lbl_JnClass" runat="server" Text="LKG" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Created User</td>
										<td class="rightside" style="height: 16px">
											<asp:Label ID="lbl_CrUser" runat="server" Text="Arun" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Created Date</td>
										<td class="rightside">
											<asp:Label ID="lbl_crDate" runat="server" Text="10/10/2010" class="control-label"></asp:Label></td>
									</tr>
									<tr>
										<td class="leftside">Temporary ID</td>
										<td class="rightside">
											<asp:Label ID="lbl_TempID" runat="server" Text="30010" class="control-label"></asp:Label></td>
									</tr>
								</table>
							</asp:Panel>
							<br />
							<h4><span style="color: #366092;">Career Details </span></h4>
							<div class="linestyle"></div>

							<asp:Panel Width="100%" runat="server" ID="Panel1" HorizontalAlign="Center">
								<asp:GridView ID="Grd_Carrier" runat="server" AutoGenerateColumns="false"
									GridLines="Vertical" Width="100%"
									BackColor="#EBEBEB"
									BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
									CellPadding="3" CellSpacing="2" Font-Size="12px" HorizontalAlign="Center">
									<Columns>

										<asp:BoundField DataField="Batch" HeaderText="Batch" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
										<asp:BoundField DataField="Standard" HeaderText="Standard" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70px" />
										<asp:BoundField DataField="Class" HeaderText="Class" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
										<asp:BoundField DataField="Result" HeaderText="Result" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
										<%--      <asp:BoundField DataField="Teacher" HeaderText="Class Teacher" HeaderStyle-HorizontalAlign="Left"/>
										--%>
									</Columns>
									<FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
									<HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
									<RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
								</asp:GridView>
							</asp:Panel>
							<br />
							<br />
							<asp:Panel ID="Pnl_Tc" runat="server">
								<table width="100%">
									<tr>
										<td valign="bottom">
											<h4><span style="color: #366092;">TC Details </span></h4>
										</td>
										<td valign="bottom" align="right">
											<asp:Button ID="Btn_View" runat="server" Class="btn btn-info"
												OnClick="Btn_View_Click" Text="View TC" />
										</td>
									</tr>
								</table>
								<div class="linestyle"></div>
								<asp:Panel Width="100%" runat="server" ID="Panel2">


									<table class="tablelist">
										<%--style="height:200px;"--%>
										<tr>
											<td class="leftside">TC No</td>
											<td class="rightside">
												<asp:Label ID="lbl_TcNo" runat="server" Text="TC:1020" class="control-label"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftside">Date of Leaving</td>
											<td class="rightside">
												<asp:Label ID="lbl_DOL" runat="server" Text="10/10/2010" class="control-label"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftside">Last Class</td>
											<td class="rightside">
												<asp:Label ID="lbl_LastClass" runat="server" Text="3A" class="control-label"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftside">Last Batch</td>
											<td class="rightside">
												<asp:Label ID="lbl_LastBatch" runat="server" Text="2010-2011" class="control-label"></asp:Label>
											</td>
										</tr>
									</table>


								</asp:Panel>
							</asp:Panel>
							&nbsp;</td>

						<td class="e" style="height: 65px"></td>
					</tr>
					<tr>
						<td class="so"></td>
						<td class="s"></td>
						<td class="se"></td>
					</tr>
				</table>
			</div>   
				</div>                      
		</div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>

