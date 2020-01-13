<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="IsssueCertificate.aspx.cs" Inherits="IsssueCertificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript">
		window.onbeforeunload = function () { return; }
		$(function () {
		    studentDetails.getSubMenu();
		    studentDetails.getTopDt();
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	  <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
			
	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
			<div class="card0">
		<div class="cardHd">Issue Transfer Certificate ( T.C ) </div>
		<div class="row stntStripBody">
			<div class="_stdntTopStrip"></div>
	    </div>
	</div>
		</div>
			<br> 
			  <div class="row"> 
			<div class="card0">
				<table class="containerTable">
				   <%-- <tr>
						<td class="no"></td>
						<td class="n">Issue TC</td>
						<td class="ne"></td>
					</tr>--%>
					<tr>
						<td class="o"></td>
						<td class="c">




							<asp:Panel ID="Panel1" runat="server"
								HorizontalAlign="Left">
								<br />
								<br />
								<table>
									<tr>
										<td class="style4">Name of the School</td>
										<td class="style4">
											<asp:TextBox ID="Txt_SchoolName" runat="server" Enabled="False" class="form-control" Width="160px"></asp:TextBox>

										</td>
										<td colspan="2">
											<asp:TextBox ID="Text_Batch" runat="server" class="form-control" Visible="False"></asp:TextBox>
											<asp:TextBox ID="Text_session" runat="server" class="form-control" Visible="False"></asp:TextBox>
										</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4">Admission No</td>
										<td>
											<asp:TextBox ID="Txt_AdmissionNo" runat="server" class="form-control" Width="160px" Enabled="false"></asp:TextBox>

										</td>
										<td class="style3">Cumulative Record No</td>
										<td class="style3">
											<asp:TextBox ID="Txt_Cumulative" runat="server" class="form-control" Width="160px"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
												runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
												InvalidChars="'\" TargetControlID="Txt_Cumulative">
											</ajaxToolkit:FilteredTextBoxExtender>


										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4">Name of the Student</td>
										<td class="style4">
											<asp:TextBox ID="Txt_PupilName" runat="server" Enabled="False" class="form-control" Width="160px"></asp:TextBox>
										</td>

										<td>
											<asp:Label ID="Lbl_TCNo" runat="server" class="control-label" Text="Tc Number"></asp:Label>
											&nbsp; 
										</td>
										<td>
											<asp:TextBox ID="txt_TCNo" runat="server" class="form-control"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="txt_TCNo_FilteredTextBoxExtender10"
												runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
												InvalidChars="'\" TargetControlID="txt_TCNo">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4">Sex</td>
										<td>
											<asp:TextBox ID="Txt_Sex" runat="server" Width="160px" class="form-control" Enabled="False"></asp:TextBox>
										</td>
										<td class="style3">Name of Father</td>
										<td class="style3">
											<asp:TextBox ID="Txt_NameOfFather" runat="server" class="form-control" Width="160px"></asp:TextBox>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td>Nationality&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										</td>
										<td>
											<asp:TextBox ID="Txt_Nationality" runat="server" class="form-control" Width="160px">Indian</asp:TextBox>
										</td>
										<td>Mother Name
										</td>
										<td>
											<asp:TextBox ID="Txt_MotherName" class="form-control" Width="160px" runat="server"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td>&nbsp;Address</td>
										<td>
											<asp:TextBox ID="Txt_ResAddress" TextMode="MultiLine" Width="160px" Height="100px" class="form-control" runat="server"></asp:TextBox>
										</td>

									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td>Religion</td>
										<td>
											<asp:TextBox ID="Txt_Religion" runat="server" Width="160px" class="form-control"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTxt_Religion"
												runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
												InvalidChars="'/\" TargetControlID="Txt_Religion">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>

										<td colspan="2">Caste</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_Cast" runat="server" Width="160px" class="form-control"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTxt_Cast"
												runat="server" Enabled="True" TargetControlID="Txt_Cast" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>


										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td colspan="2">Whether the candidate belongs to scheduled caste or scheduled tribe</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_CastType" runat="server" Width="160px" class="form-control"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Date of Birth</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_Dob" runat="server" Enabled="False" Width="160px" class="form-control"></asp:TextBox>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Standard in which the Student was studying at the time of leaving the school</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_CurrentStd" runat="server" Enabled="true" class="form-control"
												Width="160px"></asp:TextBox>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>

									<tr>
										<td class="style4" colspan="2">Date of admission /promotion to the standard  in which the Student was studying at the time of leaving the school</td>
										<td colspan="2">
											<asp:TextBox ID="txt_LastClassDate" runat="server" Enabled="true" class="form-control"
												Width="160px"></asp:TextBox>
											<ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="cal_Theme1"
												runat="server" Enabled="True" TargetControlID="txt_LastClassDate" Format="dd/MM/yyyy">
											</ajaxToolkit:CalendarExtender>
											<%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5"
								ControlToValidate="Txt_DateOfAdmission0"
								Display="None"
								ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
											<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
												runat="server" ControlToValidate="txt_LastClassDate" Display="None"
												ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
												ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
											<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender5"
												TargetControlID="RegularExpressionValidator5"
												HighlightCssClass="validatorCalloutHighlight" />

										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>


									<tr>
										<td class="style4" colspan="2" valign="top">If the student is in Higher Standard,The Language Studied</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_LangStd" runat="server" Height="68px" class="form-control"
												OnTextChanged="TextBox4_TextChanged" TextMode="MultiLine" Width="205px"
												MaxLength="50"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="Txt_WornDays_FilteredTextBoxExtender"
												runat="server" Enabled="True" TargetControlID="Txt_LangStd" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4">Medium of Instruction</td>
										<td>
											<asp:TextBox ID="Txt_MediumOfIns" runat="server" Width="160px" class="form-control">
											</asp:TextBox>
										</td>
										<td>Syllabus</td>
										<td>
											<asp:TextBox ID="Txt_Syllabus" runat="server" class="form-control" Width="160px"></asp:TextBox>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td colspan="2">Subjects Studied
										</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_subjects" TextMode="MultiLine" runat="server" class="form-control"
												Width="205px" Height="100px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Date of Admission or Date of Promotion to that class</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_DateOfAdmission0" runat="server" class="form-control" Width="160px"></asp:TextBox>
										</td>
										<ajaxToolkit:CalendarExtender ID="Txt_DateOfAdmission0_CalendarExtender1" CssClass="cal_Theme1"
											runat="server" Enabled="True" TargetControlID="Txt_DateOfAdmission0" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
										<%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5"
								ControlToValidate="Txt_DateOfAdmission0"
								Display="None"
								ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
										<asp:RegularExpressionValidator ID="RegularExpressionValidator5"
											runat="server" ControlToValidate="Txt_DateOfAdmission0" Display="None"
											ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
											ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender4"
											TargetControlID="RegularExpressionValidator5"
											HighlightCssClass="validatorCalloutHighlight" />
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Whether qualified for Promotion to a higher standard</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_Quali_Promo0" runat="server" Width="160px" class="form-control"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
												runat="server" Enabled="True" TargetControlID="Txt_Quali_Promo0" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Whether the Student has paid all the fees </td>
										<td colspan="2">
											<asp:TextBox ID="Txt_Feesdue0" runat="server" Width="160px" class="form-control" MaxLength="20"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
												runat="server" Enabled="True" TargetControlID="Txt_Feesdue0" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2" valign="top">Fee concessions if any (Nature &amp; period to be specified)</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_FeeCon" runat="server" Height="80px" class="form-control" TextMode="MultiLine"
												Width="325px"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
												runat="server" Enabled="True" TargetControlID="Txt_FeeCon" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2" valign="top">Scholarships, if any
			   <br />
											(Nature &amp; period to be specified)</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_Scholarship" runat="server" class="form-control" Height="80px"
												TextMode="MultiLine" Width="325px" MaxLength="100"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
												runat="server" Enabled="True" TargetControlID="Txt_Scholarship" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Whether medically Examined or not</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_MedicalyExmnd0" runat="server" Width="160px" class="form-control"
												MaxLength="75"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
												runat="server" Enabled="True" TargetControlID="Txt_MedicalyExmnd0" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Date of Student&#39;s Last Attendance at school <span class="style6">&nbsp;&nbsp; *</span></td>
										<td colspan="2">
											<asp:TextBox ID="Txt_LastAttendance0" runat="server" class="form-control" Width="160px"></asp:TextBox>
											&nbsp
			 <asp:RequiredFieldValidator ID="rqvalid1" runat="server" ControlToValidate="Txt_LastAttendance0"
				 ErrorMessage="Last Attendance">
			 
			 </asp:RequiredFieldValidator>
										</td>
										<ajaxToolkit:CalendarExtender ID="Txt_LastAttendance0_CalendarExtender" CssClass="cal_Theme1"
											runat="server" Enabled="True" TargetControlID="Txt_LastAttendance0" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
										<%--<asp:RegularExpressionValidator runat="server" ID="DolDateRegularExpressionValidator3"
								ControlToValidate="Txt_LastAttendance0"
								Display="None"
								ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
										<asp:RegularExpressionValidator ID="DolDateRegularExpressionValidator3"
											runat="server" ControlToValidate="Txt_LastAttendance0" Display="None"
											ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
											ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
											TargetControlID="DolDateRegularExpressionValidator3"
											HighlightCssClass="validatorCalloutHighlight" />
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Date on which the application for the transfer&nbsp; certificate was received<span
											class="style6">&nbsp;&nbsp;&nbsp; *</span></td>
										<td colspan="2">
											<asp:TextBox ID="Txt_AppTcDate0" runat="server" class="form-control" Width="160px"></asp:TextBox>

										</td>
										<ajaxToolkit:CalendarExtender ID="Txt_AppTcDate0_CalendarExtender1" CssClass="cal_Theme1"
											runat="server" Enabled="True" TargetControlID="Txt_AppTcDate0" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
										<%--<asp:RegularExpressionValidator runat="server" ID="DateRegularExpressionValidator1"
								ControlToValidate="Txt_AppTcDate0"
								Display="None"
								ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
										<asp:RegularExpressionValidator ID="DateRegularExpressionValidator1"
											runat="server" ControlToValidate="Txt_AppTcDate0" Display="None"
											ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
											ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
											TargetControlID="DateRegularExpressionValidator1"
											HighlightCssClass="validatorCalloutHighlight" />
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Date of issue of Transfer certificate <span class="style6">&nbsp; *</span></td>
										<td colspan="2">
											<asp:TextBox ID="Txt_TcDate0" runat="server" Width="160px" class="form-control"></asp:TextBox>
											<ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="cal_Theme1"
												runat="server" Enabled="True" TargetControlID="Txt_TcDate0" Format="dd/MM/yyyy">
											</ajaxToolkit:CalendarExtender>
										</td>
										<%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2"
								ControlToValidate="Txt_TcDate0"
								Display="None"
								ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
								ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
										<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
											runat="server" ControlToValidate="Txt_TcDate0" Display="None"
											ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
											ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
										<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
											TargetControlID="RegularExpressionValidator2"
											HighlightCssClass="validatorCalloutHighlight" />
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">No: of School days up to the date of leaving</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_TotalSchoolDays0" runat="server" Width="160px" class="form-control"
												MaxLength="4"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
												runat="server" Enabled="True" TargetControlID="Txt_TotalSchoolDays0" FilterType="Numbers">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">No: of school days the Student attended</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_DaysAttended0" runat="server" Width="160px" class="form-control" MaxLength="4"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
												runat="server" Enabled="True" TargetControlID="Txt_DaysAttended0" FilterType="Numbers">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
										<td class="style5">&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Reason for leaving
										</td>
										<td colspan="2">
											<asp:TextBox ID="txt_Reson" runat="server" Width="160px" class="form-control" TextMode="MultiLine"></asp:TextBox>

										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Last Exam Details
										</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_LastExamDetails" runat="server" Width="160px" class="form-control" TextMode="MultiLine"></asp:TextBox>

										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">School to which pupil intends proceeding
										</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_newschoolName" runat="server" Width="160px" class="form-control"></asp:TextBox>

										</td>
									</tr>
									<tr>
										<td class="leftside">
											<br>
										</td>
										<td class="rightside">
											<br>
										</td>
									</tr>

									<tr>
										<td class="style4" colspan="2" valign="top">Character and Conduct</td>
										<td colspan="3">
											<asp:TextBox ID="Txt_CC" runat="server" Height="50px" TextMode="MultiLine" class="form-control" MaxLength="100"
												Width="325px"></asp:TextBox>
											<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
												runat="server" Enabled="True" TargetControlID="Txt_CC" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
											</ajaxToolkit:FilteredTextBoxExtender>
										</td>
									</tr>
									<tr>
										<td class="style4" colspan="2">Student Enrollment Number
										</td>
										<td colspan="2">
											<asp:TextBox ID="Txt_enrollment" runat="server" Width="160px" class="form-control"></asp:TextBox>

										</td>
									</tr>
									<tr>
										<td class="style4" colspan="5" valign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="Lbl_Message1"
											runat="server" ForeColor="Red"></asp:Label>
										</td>
									</tr>
									<hr>
									<tr>
										<td class="style4" colspan="5" valign="top" align="center">
										  
								   
					  <%--<asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" Text="Save" 
				   Width="111px" />
				   &nbsp;
					<asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
				   Text="Cancel" Width="111px" />--%>


											<%--  <ajaxToolkit:ConfirmButtonExtender ID="Btn_Cancel_ConfirmButtonExtender" 
							runat="server"  Enabled="True" TargetControlID="Btn_Cancel"
							DisplayModalPopupID="Btn_Cancel_ModalPopupExtender">
						</ajaxToolkit:ConfirmButtonExtender>
						
						<ajaxToolkit:ModalPopupExtender ID="Btn_Cancel_ModalPopupExtender" runat="server" TargetControlID="Btn_Cancel" PopupControlID="PNL" OkControlID="ButtonYes" CancelControlID="ButtonNo" />
				  <asp:Panel ID="PNL" runat="server" style="display:none; width:300px;">
						
						<div class="container skin5"  style="width:200" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n"><span style="color:White">Message</span></td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					The student will not be moved to history. Are you sure you want to cancel the operation?
						<br /><br />
						<div style="text-align:right;">
							<asp:Button ID="ButtonYes" runat="server" Text="Yes" Width="50px" />
							<asp:Button ID="ButtonNo" runat="server" Text="No" Width="50px" />
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
						
					   
					</asp:Panel>
				   
			  
				   
					 <ajaxToolkit:ConfirmButtonExtender ID="Btn_Save_ConfirmButtonExtender" 
						runat="server"  Enabled="True" TargetControlID="Btn_Save"
						DisplayModalPopupID="Btn_Save_ModalPopupExtender">
						</ajaxToolkit:ConfirmButtonExtender>
					  
						<ajaxToolkit:ModalPopupExtender ID="Btn_Save_ModalPopupExtender" runat="server" TargetControlID="Btn_Save"  PopupControlID="PNL1"  CancelControlID="ButtonCancel" />
				   <asp:Panel ID="PNL1" runat="server" style="display:none;width:300px;">
				   
				   <div class="container skin5"  style="width:200">
					<table cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"> </td>
							<td class="n"><span style="color:White">Message</span></td>
							<td class="ne"> </td>
						</tr>
						<tr >
							<td class="o"> </td>
							<td class="c" >
								
								 Student will be moved to history.Do u want to proceed anyway?
									<br /><br />
									<div style="text-align:right;">
										<asp:Button ID="ButtonOk" runat="server" Text="Yes" Width="50px" 
											onclick="ButtonOk_Click" />
										<asp:Button ID="ButtonCancel" runat="server" Text="No" Width="50px" />
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
				   
					   
					</asp:Panel>--%>
					
										</td>
									</tr>

								</table>
								<hr />
								<div class="row text-center">
									  <asp:HiddenField ID="Hdn_QueryString" runat="server" />
									<div class="col-md-6" data-placement="bottom" data-toggle="tooltip" title="Click to print TC">
										 <asp:ImageButton ID="Img_Export" runat="server" Width="45px" Height="45px" 
												ImageUrl="~/Pics/full_page.png" OnClick="Img_Export_Click"  />
									<p>Print T.C</p>
										</div><div class="col-md-6" data-placement="bottom" data-toggle="tooltip" title="Click to Export TC to PDF format">
											&nbsp;  
			<asp:ImageButton ID="Img_PdfExport" runat="server" Width="45px" Height="45px"
				ImageUrl="~/Pics/ViewPdf.png" OnClick="Img_PdfExport_Click" />
									  <p>Export to PDF</p></div>
											&nbsp;
									
									</div>
								<br />
							</asp:Panel>


							<asp:Panel ID="Panel2" runat="server">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Label ID="Lbl_Message" runat="server" Style="color: #FF0000" class="control-label"></asp:Label>
								<br />

								<br />
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	<asp:Button ID="Btn_ClearFeeDue" runat="server" Text="Proceed Anyway" class="btn btn-success"
		OnClick="Btn_ClearFeeDue_Click" />
								&nbsp;
	<asp:Button ID="Btn_ProCancel" runat="server" OnClick="Btn_ProCancel_Click" class="btn btn-danger"
		Text="Cancel" />
								<br />
							</asp:Panel>




						</td>
						<td class="e"></td>
					</tr>
					<tr>
						<td class="so"></td>
						<td class="s"></td>
						<td class="se"></td>
					</tr>
				</table>
		   
			<asp:Button runat="server" ID="Btn_ConfirmHistory" class="btn btn-info" Style="display: none" />
			<ajaxToolkit:ModalPopupExtender ID="MPE_HistoryConfirm"
				runat="server"
				PopupControlID="Pnl_Confirm" TargetControlID="Btn_ConfirmHistory" />
			<asp:Panel ID="Pnl_Confirm" runat="server" Style="display: none">
				<div class="container skin5" style="width: 400px; top: 400px; left: 400px">
					<table cellpadding="0" cellspacing="0" class="containerTable">
						<tr>
							<td class="no"></td>
							<td class="n"><span style="color: White">alert!</span></td>
							<td class="ne">&nbsp;</td>
						</tr>
						<tr>
							<td class="o"></td>
							<td class="c">

								<asp:Label ID="lbl_HistoryMsg" runat="server" class="control-label" Text="Are you sure to move the student to History?"></asp:Label>
								<br />
								<br />
								<div style="text-align: center;">

									<asp:Button ID="Btn_History_Yes" runat="server" Text="Yes" Width="50px" OnClick="Btn_History_Yes_Click" class="btn btn-success" />
									<asp:Button ID="Btn_History_No" runat="server" Text="No" Width="50px" OnClick="Btn_History_No_Click" class="btn btn-danger" />
								</div>
							</td>
							<td class="e"></td>
						</tr>
						<tr>
							<td class="so"></td>
							<td class="s"></td>
							<td class="se"></td>
						</tr>
					</table>
					<br />
					<br />



				</div>

			</asp:Panel>
			<asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" Style="display: none" />
			<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"
				runat="server"
				PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
			<asp:Panel ID="Pnl_msg" runat="server" Style="display: none">
				<div class="container skin5" style="width: 400px; top: 400px; left: 400px">
					<table cellpadding="0" cellspacing="0" class="containerTable">
						<tr>
							<td class="no"></td>
							<td class="n"><span style="color: White">alert!</span></td>
							<td class="ne">&nbsp;</td>
						</tr>
						<tr>
							<td class="o"></td>
							<td class="c">

								<asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
								<br />
								<br />
								<div style="text-align: center;">

									<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-info"
										OnClick="Btn_magok_Click" />
									<asp:Button ID="Btn_Redirect" runat="server" Text="OK" Width="50px" class="btn btn-info"
										Visible="false" OnClick="Btn_Redirect_Click" />
								</div>
							</td>
							<td class="e"></td>
						</tr>
						<tr>
							<td class="so"></td>
							<td class="s"></td>
							<td class="se"></td>
						</tr>
					</table>
					<br />
					<br />



				</div>

			</asp:Panel>
				</div>
				  </div>
 </div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>


