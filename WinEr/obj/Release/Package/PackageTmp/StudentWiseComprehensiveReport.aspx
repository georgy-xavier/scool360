<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentWiseComprehensiveReport.aspx.cs" Inherits="WinEr.StudentWiseComprehensiveReport" %>
<%@ Register TagPrefix="WC" TagName="COMPREHENSIVEREPORT" Src="WebControls/ComprehensiveReportControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
		.IncBlock{ height: 220px;}
		.IncBlock a {color: #546078; text-decoration: none; }
		.redcol{color:Red;}
		.style1{width: 100%;}
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
			<div class="card0 stntStripBody">
		<div class="cardHd">Student Comprehensive Report</div>
		 <%-- student top div --%>
						<div class="row">
							<div class="_stdntTopStrip"></div>
						</div>
	</div>
		</div>
			<br> 
			<div class="row"> 
			<WC:COMPREHENSIVEREPORT id="WC_ComprehensiveReport" runat="server" />   
			</div>                         
		</div>
		 <div class="col-lg-2 col-md-2">
			 <div class="_subMenuItems subMnuStyle"><div class="card0" style="min-height:80vh;"><div style="margin-top:40vh;" id="submnLdr"></div></div></div>
	    </div>
	</div>
</asp:Content>
