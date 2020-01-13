<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CertficateManager.aspx.cs" Inherits="WinEr.CertficateManager" %>
 <%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .LeftCss
  {
	text-align:right;
	width:25%;

  }
   .RightCss
  {
	text-align:left;
	width:25%;
  }
 </style>
 <script type="text/javascript">
	 $(function () {
		 studentDetails.getSubMenu();
		 studentDetails.getTopDt();
	 });
	 function CheckTextValues() {
		 var Hd_txtno = document.getElementById('<%=Hd_txtno.ClientID%>');
		 var Hd_Textvalues = document.getElementById('<%=Hd_Textvalues.ClientID%>');
		 var _txtbox_no = 0;
		 var seperator = "";
		 var _id = "";
		 var _value = "";
		 try {
			 _txtbox_no = parseInt(Hd_txtno.value);
		 }
		 catch (e) {
			 _txtbox_no = 0;
		 }

		 if (_txtbox_no > 0) {
			 for (var i = 1; i <= _txtbox_no; i++) {
				 var newtext = document.getElementById('txt_' + i);
				 var m_value = newtext.value;
				 _id = _id + seperator + 'txt_' + i;
				 _value = _value + seperator + m_value;
				 seperator="#$#"
			 }
			 Hd_Textvalues.value = _id + '@#@' + _value;
		 }
		 
	 }
	 
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
			
	<div class="container-fluid cudtomContFluid">   
	<div class="col-lg-10 col-md-10 col-xs-12">
		<div class="row">
			<div class="card0">
				<div class="cardHd">Generate Student Certificates</div>
				<div class="row stntStripBody">
					<div class="_stdntTopStrip"></div>
				</div>
			 </div>
		</div>
	<br>

	 <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
			<ProgressTemplate>
				<div id="progressBackgroundFilter">
				</div>
				<div id="processMessage">
					<table style="height: 100%; width: 100%">
						<tr>
							<td align="center">
								<b>Please Wait...</b><br />
								<br />
								<img src="images/indicator-big.gif" alt="" />
							</td>
						</tr>
					</table>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
	 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		 <ContentTemplate>
			
	  <div class="card0"  >
		<table class="containerTable" >
			<%--<tr >
				<td class="no"> </td>
				<td class="n">Generate Certificate</td>
				<td class="ne"> </td>
			</tr>--%>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			
				
				<div style="min-height:300px;">
				 
				 
				  <table cellspacing="5" width="100%">
				   <tr>
					<td class="LeftCss" valign="top">
					 Select Certificate Type
					</td>
					<td class="RightCss" valign="top">
						<asp:DropDownList ID="Drp_CertificateType" runat="server" class="form-control" Width="180px">
						</asp:DropDownList>
					</td>
					<td class="LeftCss" valign="top">

						<asp:Button ID="Btn_Generate" runat="server" Text="Generate" 
							Class="btn btn-primary" onclick="Btn_Generate_Click" />
					  </td>      
					<td class="RightCss" valign="top">
						<asp:CheckBox ID="NeedBoarder" runat="server" Text="Need Boarder" Checked="false" />
						&nbsp;<asp:ImageButton ID="Img_Print" runat="server" ImageUrl="~/Pics/print1.png"  OnClientClick="CheckTextValues()"
							Width="30px" Height="30px" onclick="Img_Print_Click" />
					</td>
				   </tr>
				   <tr>
					<td colspan="4">
					 
					 <asp:Panel ID="Panel_Content" runat="server" Visible="false">
					 
					 <div style="border:solid 1px gray;padding:20px;color:Black">
					  <div id="DivCertificateContent" runat="server">
						
						<input type="text" id="txt_1" style="width:100%;height:30px"/>
						
					  </div>
					 </div>
					 
					 </asp:Panel>
					</td>
				   </tr>
				  </table>
				 
				 
				 
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
	   <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
	   <asp:HiddenField ID="Hd_txtno" runat="server" />
	   <asp:HiddenField ID="Hd_Textvalues" runat="server" />
		 </ContentTemplate>
		 </asp:UpdatePanel>   
	 </div>   
	<div class="col-lg-2 col-md-2">
			  <!--  <div class="label">Student Info</div>-->
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	</div>


</div>
  
</asp:Content>
