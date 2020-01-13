<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Codebehind="ManageStudent.aspx.cs" Inherits="ManageStudent"  %>
<%@ Register TagPrefix="WC" TagName="MANAGESTUDENT" Src="WebControls/ManageStudentControl.ascx" %>
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
	<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"></ajaxToolkit:ToolkitScriptManager>
	<div class="container-fluid cudtomContFluid">   
		<div class="col-lg-10 col-md-10 col-xs-12">
			<div class="row">
				<div class="card0">
					<div class="cardHd">Edit Student Details</div>
					<div class="row stntStripBody">
						<div class="_stdntTopStrip"></div>
					</div>
				</div>
			</div>
			<br>
			 <div class="row">
				 <WC:MANAGESTUDENT id="WC_ManageStudent" runat="server" />  
			 </div>
		  </div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>


