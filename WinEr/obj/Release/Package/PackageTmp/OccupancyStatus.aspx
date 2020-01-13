<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="OccupancyStatus.aspx.cs" Inherits="WinEr.OccupancyStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="js files/jquery-1.4.4.min.js"></script>
<script type="text/javascript">
    $(document).ready(function() {
        //Code goes here
        //Set default open/close settings
        $('.acc_container').hide(); //Hide/close all containers
        //$('.acc_trigger:last').addClass('active').next().show(); //Add "active" class to first trigger, then show/open the immediate next container

        //On Click
        $('.acc_trigger').click(function() {
            if ($(this).next().is(':hidden')) { //If immediate next container is closed...
                $('.acc_trigger').removeClass('active').next().slideUp(); //Remove all "active" state and slide up the immediate next container
                $(this).toggleClass('active').next().slideDown(); //Add "active" state to clicked trigger and slide down the immediate next container
            }
            else {
                $('.acc_trigger').removeClass('active').next().slideUp(); //Remove all "active" state and slide up the immediate next container
            }
            return false; //Prevent the browser jump to the link anchor
        });

    });


    function Reload(page) {
        window.location = page;
    }

</script>
<style type="text/css">
h2.acc_trigger {
	padding: 0;	margin: 0 0 5px 0;
	background: url(images/accordinhead.png) no-repeat;
	height: 26px;line-height:20px;
	width: 100%;
	font-size: 2em;
	font-weight: normal;
	float: left;
	border: 1px solid gray;
}
h2.acc_trigger a {
	color: #fff;
	text-decoration: none;
	display: block;
	padding: 0 0 0 50px;
}
h2.acc_trigger a:hover {
	color: #ccc;
}
h2.active {background-position: left bottom;}
h2.acc_trigger a.active 
{
    color: #ccc;
}
.acc_container {
	overflow: hidden;
	font-size: 1.2em;
	font-weight:bold;
    width: 100%;
	clear: both;
	background: #f0f0f0;
	border: 1px solid #d6d6d6;
	-webkit-border-bottom-right-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	-moz-border-radius-bottomright: 5px;
	-moz-border-radius-bottomleft: 5px;
	border-bottom-right-radius: 5px;
	border-bottom-left-radius: 5px;
}
.acc_container .block {
	padding: 5px;
	min-height:100px;
	vertical-align:middle;
}

.RoomStyleWhite
{
    font-size: 0.7em;
    width:100%;
    height:130px;
    background: #fff;
    border: 1px solid #d6d6d6;
    -webkit-border-bottom-right-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	-moz-border-radius-bottomright: 5px;
	-moz-border-radius-bottomleft: 5px;
	border-bottom-right-radius: 5px;
	border-bottom-left-radius: 5px;
}

.RoomStyleRed
{
    font-size: 0.7em;
    width:100%;
    height:130px;
    background: #feeeeb;
    border: 1px solid #d6d6d6;
    -webkit-border-bottom-right-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	-moz-border-radius-bottomright: 5px;
	-moz-border-radius-bottomleft: 5px;
	border-bottom-right-radius: 5px;
	border-bottom-left-radius: 5px;
}
.RoomStyleGreen
{
    font-size: 0.7em;
    width:100%;
    height:130px;
    background: #f5fdf2;
    border: 1px solid #d6d6d6;
    -webkit-border-bottom-right-radius: 5px;
	-webkit-border-bottom-left-radius: 5px;
	-moz-border-radius-bottomright: 5px;
	-moz-border-radius-bottomleft: 5px;
	border-bottom-right-radius: 5px;
	border-bottom-left-radius: 5px;
}

.RoomNameStyle
{
    font-size: 1.1em;
    width:30%;font-weight:bold;color:Red;
}

.RoomTypeStyle
{
    font-size: 0.8em;
    width:70%;font-weight:bold;
}

.RoomStatusStyle
{
    font-size: 1.1em;
    width:50%;border:solid 1px gray;color:Green;font-weight:bold;
}
.RoomStatusanchor
{
    color:Green;
    font-weight:bold;
}
.RoomImageStyle
{
    border:solid 1px gray;
}


.Roomleft
{
    font-size: 0.8em;
    width:50%;
    text-align:right;
}

.RoomRight
{
     width:50%;
    font-size: 1.1em;
    font-weight:bold;
}

.StudentListStyle
{
    border:solid 1px gray;
}
.OverOccupied
{
    color:Black;
    background-color:Red;
}

.FullOccupied
{
     color:Black; 
    background-color:#64ffb1;
}

.HalfOccupied
{
    color:Black;
    background-color:#ffd8b0;
}


.EmptyStyle
{
    color:Black;
    background-color:White;
}

.OutStyle
{
    color:Black;
    background-color:#ffd8b0;
}

#InsideBorder
{
    border-style:ridge;
    border-color:White;
    border-width:2px;
}
.ProgressStyle
{
    padding:1%;
    border-style:ridge;
    border-color:Gray;
    border-width:2px;
    -moz-border-radius: 12px;
    -webkit-border-radius: 12px;
   -khtml-border-radius: 12px;
     border-radius: 12px;
}


.ProgressManyStyle
{
    border-style:ridge;
    border-color:Gray;
    border-width:2px;
    width:200px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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



<asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>  
   <div id="left">      
    <div class="container skin1" >
     <table   cellpadding="0" cellspacing="0" class="containerTable">
     <tr >
       <td class="no"> </td>
       <td class="n">Route Allotment Status</td>
       <td class="ne"> </td>
      </tr>
      <tr >
        <td class="o"> </td>
        <td class="c" >
        
         <div style="min-height:300px;">
                           
                            <br /> 
               <div id="FloorDiv" runat="server">
                             
<h2 class="acc_trigger">  <table width="100%" cellspacing="0"> <tr> <td align="left"  valign="middle">   
  <a  style="color:Gray; font-size:large;" href="#">500D</a>    </td> <td align="right" valign="middle"> <div class="ProgressManyStyle" > 
  <table width="100%"  cellspacing="0">  <tr>    <td class="EmptyStyle"  style="width:100%; font-size:small; font-weight:bold" align="center">Occupied:12, Capacity:60 </td> </tr>  
    </table></div>     </td>    </tr>   </table>     </h2><div class="acc_container"><div class="block">			 
     <table width="20%" cellspacing="10">  <tr>    <td style="width:19%"> <div class="RoomStyleRed"> 
       <table  width="100%" cellspacing="5">   <tr><td align="center" class="RoomNameStyle" style="color:Gray" colspan="2">  Mng07.30 
 </td>   </tr>  <tr>  <td colspan="2" style="padding:1px;">
            <table width="100%"  cellspacing="0">   <tr>   <td align="center" class="RoomStatusStyle"> 
 <a href="javascript:mywindow=window.open('ScheduleTranportationFee.aspx?RoomId=1','Info','status=1,width=950, height=300,resizable = 1,scrollbars=1');mywindow.moveTo(100,100);" 
 title="View Students" style="color:Red;font-weight:bold;background-image:none" >  EMPTY  </a> </td> <td  align="center" class="RoomImageStyle"> 
 <a href="javascript:mywindow=window.open('ScheduleTranportationFee.aspx?RoomId=1','Info','status=1, width=950, height=300,resizable = 1,scrollbars=1');mywindow.moveTo(100,100);" style="background-image:none"> 
  <img src="Pics/students.png" width="30px" alt="Select" title="View Students"  style="border:none"/>   </a>  </td>  </tr>   </table>  </td>  
   </tr> <tr>  <td class="Roomleft">  Occupancy: </td>  <td class="RoomRight">  0 </td> </tr>  <tr>  <td class="Roomleft"> 
    Vacancy: </td>  <td  class="RoomRight">  60</td></tr>  </table> &nbsp;  </div> </td></tr></table>	
    	 </div></div><h2 class="acc_trigger">  <table width="100%" cellspacing="0"> <tr> <td align="left"  valign="middle">    
    	  <a href="#" >500S</a>    </td> <td align="right" valign="middle"> <div class="ProgressManyStyle" > 
    	  <table width="100%"  cellspacing="0">    </table></div>     </td>    </tr>   </table>   
    	    </h2><div class="acc_container">
<div class="block">			<center> NO ROOM FOUND IN SELECTED FLOOR </center>		 </div></div> 
              </div>
                  
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
 </div>
     
 </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
