<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" CodeBehind="TimeTableHome.aspx.cs" Inherits="WinEr.TimeTableHome" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	

 <script type="text/javascript">

	 var GarageDoor = {

		 scrollY: 0,
		 scrollX: 0,

		 setBindings: function(id) {
			 $("#" + id + " .mouse").each(function(i) {
				 $(this).bind("mouseenter", function(e) {
					 GarageDoor.hideDoor(this);
				 });
				 $(this).bind("mouseleave", function(e) {
					 GarageDoor.showDoor(this);
				 });

				 $(this).bind("click", function(e) {

					 if (this.parentNode.id != '#') {
						 window.open(this.parentNode.id);
					 }
				 });
			 });
		 },

		 hideDoor: function(obj) {
			 var xs = GarageDoor.scrollX;
			 var ys = GarageDoor.scrollY;

			 $(obj).parent().find(".Redoverlay").animate({
				 opacity: 0.5
			 }, 450);


			 $(obj).parent().find(".Greenoverlay").animate({
				 opacity: 0.5
			 }, 450);

		 },

		 showDoor: function(obj) {
			 $(obj).parent().find(".Redoverlay").animate({
				 opacity: 1
			 }, 450);


			 $(obj).parent().find(".Greenoverlay").animate({
				 opacity: 1
			 }, 450);
			 
			 
		 }
	 }
 
 </script>
  

 <style type="text/css">

	 .garagedoor a:active, .under_header a:active, .header a:focus, .under_header a:focus {
		 outline: none;
		 -moz-outline-style: none;
		 outline-style: none;
	 }

.garagedoor img {
	border: 0; display: block;	
}

.item .mouse 
{
	width:130px;
	margin:0;padding:0;border:0;display:block;left:0;position:absolute;cursor:pointer;top:0;
}

.item .mouse img {
	margin:0;padding:0;
}

.dummyitem
{
	width: 197px;
	color: black;
	overflow: hidden;
	display: block;
	overflow: hidden;
	position: relative;
	margin: 0;
	padding: 0;
	float: left;
	background-repeat: no-repeat;
	text-decoration: none;
	top: 0;
	left: 0;
}

.dummyitem .mouse 
{
	width:130px;
	margin:0;padding:0;border:0;display:block;left:0;position:absolute;top:0;
}

.dummyitem .mouse img {
	margin:0;padding:0;
}

.dummyitem .underlay {
	color: black;

}

.item {
	width:100%;
	color: black;

	overflow:hidden;display:block;position:relative;
	margin:0;padding:0;float:left;background-repeat:no-repeat;text-decoration: none;
		 top: 0;
		 left: 0;
	 }

.item .underlay {
	color: black;

}

.item .Redoverlay {
	/*background-image:url(images/Red.jpg);*/
	background-color: rgba(255, 101, 52, 0.82);
	  background-repeat:repeat;
	  color:White;
	width: 209px;
	  height:40px;
	  overflow:hidden;
	  text-align:center;
	  font-weight:bolder;
	  vertical-align:middle;
	  font-size:13px;    
	  padding-top:10px;  
}
.dummyitem .Yellowoverlay {
	background-color: #f2d96d;
	background-repeat: repeat;
	color: Black;
	width: 209px;
	height: 40px;
	overflow: hidden;
	text-align: center;
	font-weight: bolder;
	vertical-align: middle;
	font-size: 13px;
	padding-top: 10px;
	cursor: help;
}
.item .Greenoverlay {
	  /*background-image:url(images/grn.jpg);*/
	   background-color: #00a922;
	  background-repeat:repeat;
	  color:White;
	 width: 209px;
	  height:40px;
	  overflow:hidden;
	  text-align:center;
	  font-weight:bolder;
	  vertical-align:middle;
	  font-size:13px; 
	  padding-top:10px;     
}

 
 .LeftStyle
 {
	 color:Gray;
	 font-weight:bold;
	 text-align:right;
	 width:80%;
	 font-size:11px;
 }
 .RightStyle
 {
	 color:Black;
	 font-weight:bold;
	 text-align:left;
	  width:20%;
 }

	.ClassStyle
	{
			width: 95%;
	border: solid 1px gainsboro;
	text-align: center;
	}

 
 </style>
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	  <div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
			</ajaxToolkit:ToolkitScriptManager>  
	   
		  
		   
			
		  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
				<ContentTemplate>

		  <asp:panel ID="Panel2"  runat="server"> 
	
			<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Configure Time Table </td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<br />
				  
				  <table width="100%">
				   <tr>
					<td>
					  <div style="float:right;">
					   <table cellspacing="0">
						   <tr>
							  <td align="center" valign="middle" style="width:20px;height:20px;background-color: #00a922;border-radius: 10px;">
								  
							 </td>
							 <td align="left"  style="padding:5px 10px 5px 10px;">
								   Fully Configured
							  </td>

								<td  align="center" valign="middle" style="width:20px;height:20px;background-color: #ff5f2c;border-radius: 10px;">
									
								 </td>
								<td align="left" style="padding:5px 10px 5px 10px;">
									   Partially Configured
								</td>

								 <td  align="center" valign="middle" style="width:20px;height:20px;background-color: #f2d96d;border-radius: 10px;">
									  
								 </td>
								 <td align="left" style="padding:5px 10px 5px 10px;">
									   Period Not Configured
								 </td>
							  </tr>
						 </table>
					  </div>
					  <br />
					</td>
				   </tr>
					<tr>
					<td>
					  <div id="DivClass" runat="server">
		  
		 
				
				<%--<div class="garagedoor" id="garagedoor">	
				
			 <table width="100%" cellspacing="10">
			  <tr>
			   <td >
			   
					
				 <table class="ClassStyle">
				  <tr>
				   <td colspan="2">
				   
				   <div title="UKG" class="item" id="link1">
					 <div class="underlay">
						
						</div>
					<div class="Redoverlay" > UKG </div>
					<div class="mouse"><img src="images/nothing.gif"  alt=""> &nbsp;</div>
				   </div>
				   
				   </td>
				  </tr>
				  <tr>
				   <td class="LeftStyle">
					
					Total Subjects : 
					
					
				   </td>
				   <td class="RightStyle">
					 
					 11
					 
				   </td>
				  </tr>
				  <tr>
				   <td class="LeftStyle">
					
					Total Staffs : 
					

					
				   </td>
				   <td class="RightStyle">
					 
					 7
					 
				   </td>
				  </tr>
				  <tr>
				   <td class="LeftStyle">
					
					Periods Alloted : 
					

					
				   </td>
				   <td class="RightStyle">
					 
					 35
					 
				   </td>
				  </tr>
				  <tr>
				   <td class="LeftStyle">
					
					Free Periods : 
					

					
				   </td>
				   <td class="RightStyle">
					 
					 6
					 
				   </td>
				  </tr>
				 </table>	
					

					
			   </td>
				<td>
				<div title="ONE A" class="item" id="link2">
						<div class="underlay">
						
						</div>
						<div class="Yellowoverlay" > ONE A </div>
						<div class="mouse"><img src="images/nothing.gif" alt=""> &nbsp;</div>
					</div>
			   </td>
			   
			   <td>
				<div title="FOUR A" class="item" id="link3">
						<div class="underlay">
						
						</div>
						<div class="Greenoverlay" > FOUR A </div>
						<div class="mouse"><img src="images/nothing.gif" alt=""> &nbsp;</div>
					</div>
			   </td>
			   
			  </tr>
			 </table>
					
					
  

				

				</div>--%>
				
		 </div>
					</td>
				   </tr>
				  </table>
					
			 
						

				
		  
		 
				
				
<script type="text/javascript">
	GarageDoor.scrollY = -55;
	GarageDoor.setBindings('garagedoor');
</script>
				   
				  <br />
				  
				  
				  
	
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
		  
		   </asp:panel> 
		   

	   </ContentTemplate>
	   </asp:UpdatePanel>


<div class="clear"></div>
  </div>
</asp:Content>
