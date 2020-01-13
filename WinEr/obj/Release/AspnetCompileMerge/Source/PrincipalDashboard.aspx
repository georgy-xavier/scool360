<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="PrincipalDashboard.aspx.cs" Inherits="WinEr.PrincipalDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.minheight {
			min-height:420px;
		}
		.viewmoreStyle{
			font-weight: 100;
			font-size: 14px;
			padding: 5px;
			color: #607D8B;
		}
		.noData {
			
			padding-top: 60px;
			text-align:  center;
			font-size: 20px;
			color: #9E9E9E;
			font-weight:  100;

		}
		.ClassAttendanceBack {
			border: 1px solid;
			border-color: #e5e6e9 #dfe0e4 #d0d1d5;
			background-color: white;
			border-radius: 0;
			margin: 9px 0 9px 0;
			padding: 15px 0 10px 0;
		}
		.panelHead1 {
		   line-height: 30px;
			background-color: #3F51B5;
		}
		h4 {
			font-size: medium;
			color: #969696;
		}

	</style>


	<script>
		$(document).ready(function () {
			$("#div_ClassStrength,#div_StudentLateComers,#div_StaffTodaysAbsentees").html(cardloader);
			$("#div_usingtransportation,#div_usinghostel,#div_StaffExitVilation,#div_StudentExitVilation").html(cardloader);
			$("#Lbl_SchoolName").html(circleLoader);
			//$('#SubPreLoader').show();
			PageMethods.LoadSchoolData(OnSuccess8, OnLoadDataError);
			function OnSuccess8(response, userContext, methodName) {

				$("#Lbl_SchoolName").html(response[0]);
				$("#Lbl_Subhead").html(response[1]);
			}
			PageMethods.Load_ClassStrength(OnSuccess1, OnLoadDataError);
			function OnSuccess1(response, userContext, methodName) {
			  $("#div_ClassStrength").html(response);
			}
			PageMethods.Load_StudentLateComers(OnSuccess2, OnLoadDataError);
			function OnSuccess2(response, userContext, methodName) {
				$("#div_StudentLateComers").html(response);
			}
			PageMethods.Load_StudentExitVilation(OnSuccess3, OnLoadDataError);
			function OnSuccess3(response, userContext, methodName) {
				$("#div_StudentExitVilation").html(response);
			}
			PageMethods.Load_StudentUsingTransportation(OnSuccess4, OnLoadDataError);
			function OnSuccess4(response, userContext, methodName) {
				$("#div_usingtransportation").html(response);
			}
			PageMethods.Load_StudentUsingHostel(OnSuccess5, OnLoadDataError);
			function OnSuccess5(response, userContext, methodName) {
				$("#div_usinghostel").html(response);
			}
			PageMethods.Load_StaffExitVilation(OnSuccess6, OnLoadDataError);
			function OnSuccess6(response, userContext, methodName) {
				$("#div_StaffExitVilation").html(response);
			}
			PageMethods.Load_StaffAbsentees(OnSuccess7, OnLoadDataError);
			function OnSuccess7(response, userContext, methodName) {
				$("#div_StaffTodaysAbsentees").html(response);
			}
			
		});
	  function OnLoadDataError(response, userContext, methodName) {
		  //TODOD:
	  }

	  </script>
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<ajaxToolkit:ToolkitScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true" />
	<%-- Update Panel should not be used in this page--%>
	<div id="contents">
		<asp:Panel runat="server" Width="100%"  ID="Panel3" HorizontalAlign="Center">
		<div id="HomeschName" class="card0 customContainer row">
			<div class="col-lg-2">
				<asp:Image ID="Img_Logo" runat="server" ImageUrl="Handler/ImageReturnHandler.ashx?id=1&type=Logo" Height="100px" Width="100px" />
			</div>
			<div class="col-lg-8">
				<div class="row" style="margin-top: 20px;font-weight: bolder;">
					<div id="Lbl_SchoolName" class="control-label" style="font-Size:Large;color:#003366;"></div>
				</div>
				<div class="row" >
					<div id="Lbl_Subhead" class="control-label" style="font-Size:Small;color:#003366;"></div>
				</div>
				<div class="row" >
					<hr><h4 style="color: #9E9E9E;font-weight: 100;">Principal DashBoard</h4>
				</div>
			</div>
			<div class="col-lg-2">
				<div class="text-center Custtooltip" style="margin-top:20px;">
					<i class="fa fa-dashboard" style="font-size: 48px;color: #3F51B5;"></i><br>
					<label class="control-label">Switch Dashboard </label><br>
					<asp:LinkButton ID="generaldashboard" runat="server" ForeColor="Blue" OnClick="generaldashboard_Click">General  Dashboard</asp:LinkButton>
					<span class="Custtooltiptext">Click here to view General Dashboard</span>
				</div>
			</div>
		</div>
	 
		</asp:Panel>
		<hr>
		<asp:Panel runat="server" Width="100%" ID="Panel2">

			<div class="customContainer">
				<div class="well" style="background-color:white;">
					<div style="font-size: medium; font-weight: 300;"><i class="fa fa-user-o"></i>&nbsp; Staff Details</div>
				</div>
				<div class="row">
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">TODAYS ABSENTEES</h4>
							<br />
							<div id="div_StaffTodaysAbsentees" class="panel"></div>
							 <div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="#" class="viewmoreStyle">....</a>
							</div>
						</div>
					</div>
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">EXIT VIOLATION </h4>
							<br />
							<div id="div_StaffExitVilation" class="panel"></div>
							<div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="LateComersReport.aspx" class="viewmoreStyle">View More Details..</a></div>
						</div>
					</div>
				</div>
			</div>
			<hr />
			<div class="customContainer">
				<div class="well" style="background-color:white;">
					<div style="font-size: medium; font-weight: 300;"><i class="fa fa-user-o"></i>&nbsp; Student Details</div>
				</div>
				
				<div class="row">
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">CLASS STRENGTH</h4>
							<br />
							<div id="div_ClassStrength" class="panel"></div>
							<div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="#" class="viewmoreStyle">....</a>
							</div>
						</div>
					</div>
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">LATE COMERS</h4>
							
							<br />
							<div id="div_StudentLateComers" class="panel"></div>
							<div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="LateComersReport.aspx" class="viewmoreStyle">View More Details..</a>

							</div>
						</div>
					</div>
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">EXIT VIOLATION </h4>
							
							<br />
							<div id="div_StudentExitVilation" class="panel"></div>
							 <div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="#" class="viewmoreStyle">....</a>
							</div>
						</div>
					</div>
					<div class="col-md-6 col-xs-12">
						<div class=" ClassAttendanceBack minheight">
							<h4 class="text-center">TRANSPORTATION</h4>
							<br />
							<div id="div_usingtransportation" class="panel"></div>
							<div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="ExitViolationReport.aspx" class="viewmoreStyle">View More Details..</a>
							</div>
						</div>
					</div>
					<div class="col-md-6 col-xs-12">
						<div class="ClassAttendanceBack minheight">
							<h4 class="text-center">HOSTEL</h4>
							<br />
							<div id="div_usinghostel" class="panel"></div>
							 <div style="text-align: right; font-size: larger; color: darkblue;">
								<a href="#" class="viewmoreStyle">....</a>
							</div>
						</div>
					</div>
				</div>
			</div>
			<hr>
			
		</asp:Panel>
	</div>
</asp:Content>
