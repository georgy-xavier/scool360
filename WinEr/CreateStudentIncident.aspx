<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateStudentIncident.aspx.cs" Inherits="WinEr.WebForm15"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" />

	<style type="text/css">
		.style1
		{
			color: #FF0000;
		}
	</style>
	<script type="text/javascript">
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
		<div class="cardHd">Report Student Incidents</div>
		<div class="row stntStripBody">
			<div class="_stdntTopStrip"></div>
		</div>
	</div>
		</div>
			<br>    
			 <div class="row">
				<div class="card0" style="min-height:400px;">
					<table class="containerTable">
				   <%-- <tr >
						<td class="no"><img alt="" src="images/indnt_srch5.png" width="35" height="35" /> </td>
						<td class="n">Report Student Incident</td>
						<td class="ne"> </td>
					</tr>--%>
					<tr >
						<td class="o"> </td>
						<td class="c" >
							 <asp:Panel ID="Pnl_mainarea" runat="server">
								<table width="100%" class="tablelist">
							
									<tr>
										<td class="leftside">
											Incident Type<span class="style1">*</span>
										</td>
										<td class="rightside">
											<asp:DropDownList ID="Drp_InceType" runat="server" Width="160px" class="form-control" AutoPostBack="true" 
												onselectedindexchanged="Drp_InceType_SelectedIndexChanged">
											</asp:DropDownList>
											&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
											<asp:Image ID="Img_Up" runat="server" ImageUrl="images/pt1_up.png" Width="30px" Height="30px" />
											<asp:Image ID="Img_Down" runat="server" ImageUrl="images/pt1_dwn.png" Width="30px" Height="30px" />
												<asp:Label ID="lbl_PointText" runat="server" class="control-label" Font-Bold="false" Text="Points :"></asp:Label>
												<asp:Label ID="lbl_Points" runat="server" class="control-label" Text="" Font-Bold="true"></asp:Label>
										
											<asp:HiddenField ID="Hdn_NeedApproval" runat="server" />
										</td>
										</tr>
							
									  <tr>
										<td>
											&nbsp;</td>
										<td> &nbsp;</td>
									</tr>
									  <tr>
										<td class="leftside">Incident Title </td>                   
										<td class="rightside">
											<asp:DropDownList ID="Drp_Title" runat="server" Width="304px" AutoPostBack="true" class="form-control"
												onselectedindexchanged="Drp_Title_SelectedIndexChanged" >
											</asp:DropDownList>    
									
										</td> 
						   
									  </tr>
									  <tr>
										  <td>
											  &nbsp;</td>
										  <td >
											  &nbsp;</td>
									</tr>
									  <tr>
										<td class="leftside"> Incident Description<span class="style1">*</span></td>
										<td class="rightside">
											<asp:TextBox ID="Txt_Dese" runat="server" Width="300px" Height="50px" class="form-control"
												MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
								
										  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Dese_FilteredTextBoxExtender1" 
											  runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
											  InvalidChars="'\/" TargetControlID="Txt_Dese">
										  </ajaxToolkit:FilteredTextBoxExtender>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="Txt_Dese" ErrorMessage="*"></asp:RequiredFieldValidator>
									</td>    
									  </tr>
									<tr>
										<td>
											&nbsp;</td>
										<td >
											&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											Incident Date<span class="style1">*</span></td>
										<td class="rightside">
											<asp:TextBox ID="Txt_Date" runat="server" Width="160px" class="form-control"></asp:TextBox>
							   
										<ajaxToolkit:CalendarExtender ID="Txt_Date_CalendarExtender" runat="server" 
											CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_Date" Format="dd/MM/yyyy">
										</ajaxToolkit:CalendarExtender>
							  
											<asp:RegularExpressionValidator ID="Txt_Date_RegularExpressionValidator3" 
																runat="server" ControlToValidate="Txt_Date" Display="None" 
																ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
																 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
																 />
										<ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
											runat="Server" HighlightCssClass="validatorCalloutHighlight" 
											TargetControlID="Txt_Date_RegularExpressionValidator3" />
											<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Date" ErrorMessage="*"></asp:RequiredFieldValidator>
								 </td>
									</tr>
									<tr>
										<td>
											&nbsp;</td>
										<td>
											&nbsp;</td>
									</tr>
									<tr>
										<td class="leftside">
											&nbsp;</td>
										<td class="rightside">
											<asp:Button ID="Btn_Create" runat="server" Text="Create" Class="btn btn-primary"
												onclick="Btn_Create_Click"/>
											&nbsp;&nbsp;&nbsp;
											<asp:Button ID="Btn_CrAndApr" runat="server" Text="Create &amp; Approve" 
												onclick="Btn_CrAndApr_Click" Class="btn btn-success"  Width="130px" />
										</td>
									</tr>
									<tr>
										<td>
											&nbsp;</td>
										<td >
											&nbsp;</td>
									</tr>
								</table>
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
	<WC:MSGBOX id="WC_MessageBox" runat="server" /> 
</asp:Content>
