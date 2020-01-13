<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="True" CodeBehind="Home.aspx.cs" Inherits="WinErParentLogin.Home" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script type="text/javascript" src="js files/jquery.tools.min.js"></script>
	<link rel="stylesheet" type="text/css" href="css files/ContentSlide.css"/>

<%--    
<%--   <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
  <script type="text/javascript" src="js files/jquery.fitvids.js"></script>
  <script type="text/javascript" src="js files/jquery.bxslider.js"></script>
  <link href="css files/jquery.bxslider.css" rel="stylesheet" />--%><%--
<script type="text/javascript">
    $(document).ready(function() 
    {
        $('.bxslider').bxSlider({
        video: true,
        infiniteLoop: false,   
        hideControlOnEnd: true,
        mode: 'fade',
        captions: true
        });
    });
</script>--%>

    <style type="text/css">
        a:link {text-decoration: none;} 
        a:visited {text-decoration: none;} 
        a:hover {text-decoration: underline;} 
        .PaperStyle
        {
            background-image:url(images/paperdesignblue.jpg);
            background-repeat:no-repeat;
        } 

        #BirthdayBack
        {
            width:250px;
            
            padding:5px 10px 10px 5px;
            background:#fff url(images/h300.png) repeat-x;
            border:solid 1px #4a4a4a;
            -moz-border-radius: 8px;
           -webkit-border-radius: 8px;
           -khtml-border-radius: 8px;
            border-radius: 8px;
        }
        .BirthdayData
        {
            min-height:140px;
            height:140px;
            overflow:auto;
            font-size:11px;
        }
        #BirthdayHeading
        {
            font-weight:bold;
            color:Black;
            border-bottom:solid 1px black;
        }
         #BirthdayHeadingImage
        {
            font-weight:bold;
            border-bottom:solid 1px black;
        }
        .StudentName
        {
            width:60%;
            height:20px;
            border-bottom:solid 1px gray;
        }
        
         .Day
         {
             width:40%;
              border-bottom:solid 1px gray;
         }
               
        .divdescription
        {
            color:Black;
        }
         .leftside
        {
            text-align:right;
            font-weight:lighter;
        }
         .rightside
        {
            
            
            font-weight:bolder;
            color:Black;
        }

         .Nextmonth
        {
            padding:0px 10px 0px 10px;
        }
    
      .BirthdaySlide
      {
          /*background-image:url(pics/h300.png);*/
          width:98%;
          height:225px;
          overflow:auto
      }
      
      
      .EventSlide
      {
          /*background-image:url(pics/h300.png);*/
          width:98%;
          height:225px;
          overflow:auto
      }
    .divstyles
{
    width:auto;
    height:auto;
    border:solid 1px #4a4a4a;
    -moz-border-radius: 8px;
   -webkit-border-radius: 8px;
   -khtml-border-radius: 8px;
    border-radius: 8px;
    
}
.btnheading
{
    background:#FFF url("../images/button.jpg")  no-repeat;
    width:178px;
    height:46px;
    text-align:center;
    font-size:13px;
    color:#4a4a4a;
    line-height:45px;
    font-weight:700;
    
}
    
   .subheading
   {

    height:15;
    font-size:14px;

    padding-left :10px;
   }
   
   .incidenttables
   {
        width:195px;
        height:250px;
        padding:10px;
        float:left;
      overflow:auto;
        margin:5px;

        -khtml-box-shadow:10px 10px 5px #888888;
        -moz-box-shadow:10px 10px 5px #888888;
        -webkit-box-shadow:10px 10px 5px #888888;
        box-shadow:10px 10px 5px #888888;



       
   }
    .eventarea
   {

       clear:both;
       margin-top:20px;
   }
   .incidentarea
   {
       width:100%;
       margin-bottom:10px;
       padding-bottom:20px;
       min-height:280px;
       
       
   }
   .incidentheading
   {
       font-size:14px;
       font-weight:bold;
       text-align:center;
     background-color:#FFF8DC;

        padding:3px;
    
   }
   .GeneralAnnouncement
   {
        width:97%;
        height:275px;
        padding:10px;
        float:left;    
        margin:5px;
        -khtml-box-shadow:10px 10px 5px #888888;
        -moz-box-shadow:10px 10px 5px #888888;
        -webkit-box-shadow:10px 10px 5px #888888;
        box-shadow:10px 5px 5px #888888;   
   }
   .academicainc
   {
        border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
  
        
   }
   .medicalinc
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
   }
      .deciplenary
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
   }
         .otherinc
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
   }
  
   
   .authenticationactivationelement
   {
       text-align:center;
      
        
         background-color:Silver;
	   filter:alpha(opacity=90);
	   opacity:0.9;

       
	    position:fixed;
	    z-index:999;
    
    width:100%;
    height:100%;
    top:0;
    left:0;
      
        font-size:14px;
        padding:5px;
       color:#888888;

   }
  .announcement
   {
        border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
  
        
   }
   
  .messagewindowarea
  {
       text-align:center;
      
        border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
         background-color:#FFF8DC;
	
	    z-index:999;
    
    width:500;
    height:500;
      background-position:bottom;
	    position:fixed;
        right:0px;
        bottom:0px;
      
        font-size:14px;
        padding:5px;
        font-weight:bold;
    

  }
   
        .style2
        {
            width: 154px;
        }
        .Div_Line
        {
            margin-left:2%;
            margin-right:2%;
            height:10px;
            color:Blue;
        }
        .Div_Subject
        {
           margin-left:5%;
         
           
        }
         .Div_Expire_Dt
        {
           margin-left:70%;
         
           
        }
      LinkButton:hover {
    color:#FF6600;
    font-size:15px;
    
            }
   .ontop {
				z-index: 888;
				width: 100%;
				height: 100%;
				top: 0;
				left: 0;
				display: none;
				position: absolute;				
				background-color: #cccccc;
				color: #aaaaaa;
				opacity: .4;
				filter: alpha(opacity = 50);
			}
			#popup 
			{
			    z-index: 999;
				width: 900px;
				height: 1000px;
				position: absolute;
				color: #000000;
				/* To align popup window at the center of screen*/
				top: 40%;
				left: 40%;
				margin-top: -180px;
				margin-left: -280px;
				opacity: 1;
				filter: alpha(opacity = 100);
				display: none;
			}
  </style>
  <script type="text/javascript">



function showmsgpopup() {
    $("#messagewindow").slideToggle("slow");
}
function closemsgpopup() {
    $("#messagewindow").slideToggle("slow");
}
function showstatus() {
    $("#messagewindow").show();
}

      </script>  
      
  
    <script type="text/javascript">

            function LoadPopup(name) {
                var Desc;
                var PanelHide = document.getElementById('<%=PanelHide.ClientID%>');
                var Hd_Event = PanelHide.innerHTML;
                var Array1 = Hd_Event.split('$%$');
                for (var i = 0; i < Array1.length; i++) {
                    var strArray = Array1[i].split('*-*');
                    if (strArray[0] == name) {
                        Desc = strArray[1];
                    }
                }
                
                var HtmlControl = document.getElementById('HtmlID');
                HtmlControl.innerHTML = '<table width="100%" cellspacing="10"> <tr>   <td style="font-weight:bold;color:Black" align="left"> ' + name + '  </td> </tr>  <tr>  <td style="border-top:solid 1px gray;" align="left" valign="top"> <div style="height:110px;overflow:auto">   ' + Desc + '  </div>   </td>   </tr></table>';
                var modalPopupBehavior = $find('EventModalPopupBehavior');
                modalPopupBehavior.show();
            }
            </script>   

    <script type="text/javascript">
        $(function() {
            $(".slidetabs").tabs(".images > div", {

                // enable "cross-fading" effect
                effect: 'fade',
                fadeOutSpeed: "slow",

                // start from the beginning after the last tab
                rotate: true

                // use the slideshow plugin. It accepts its own configuration
            }).slideshow({autoplay: true, interval:10000});
        });

    </script>
     
         <script type="text/javascript">
             function pop(div) {
                 document.getElementById(div).style.display = 'block';
                 document.getElementById('popup').style.display = 'block';
             }
             function hide(div) {
                 document.getElementById(div).style.display = 'none';
                 document.getElementById('popup').style.display = 'none';
             }
             //To detect escape button
             document.onkeydown = function(evt) {
                 evt = evt || window.event;
                 if (evt.keyCode == 27) {
                     hide('popDiv');
                 }
             };

             function ViewAnnouncementImage(e) {
                 $('#PopupImage').attr('src', $(e).children('img').attr('src'));
                 pop('popDiv');
                 return false;
             }

             function HidePopup() {
                 hide('popDiv');
                 return false;
             }
		</script>
         
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" onload="myFunction()" >
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<div>

<table>
<tr>
<td>
<div class="eventarea" style="float:left;width: 50%;padding-left:110px;" >

     <table width="550">
     <tr>
       
     
       
       <td valign="top"> 
        
        <table width="100%" cellspacing="0">  <%--Content Slider--%>
 
         <tr>
          <td >
             <!-- container for the slides -->
             
            <div id="ImagesDiv" runat="server">
            
                             
            
            </div>
             
            
          
          </td>
         </tr>
         <tr>
         
           <td style="padding-left:5px;" align="left">
           
            <table width="96%">
              <tr>
                  <td style="width:50%;padding-left:50px;border:solid 2px gray;" align="center">                    
                              <!-- the tabs -->
                   <div id="SlideTabsDiv" runat="server">
                           
   
                           
                   </div>
                              
               
               </td>
                <td style="width:50%; padding-top:5px;padding-left:40px;border:solid 2px gray;">
                        <img  alt=""   onclick="$(&quot;.slidetabs&quot;).data(&quot;slideshow&quot;).play();"    src="Pics/play%20buttons/black_play.png"   style="width:20px;height:20px;cursor:pointer;float:left" title="Play" />
                        <img  alt=""   onclick="$(&quot;.slidetabs&quot;).data(&quot;slideshow&quot;).stop();"   src="Pics/play%20buttons/black_stop.png"   style="width:20px;height:20px;cursor:pointer;float:left" title="Stop" />
                         
                         <a class="backward">
                            <img alt="" src="Pics/play buttons/black_rew.png" 
                                style="width:20px;height:20px;cursor:pointer;float:left" title="prev" /> </a> 
                           
                        <a class="forward">
                            <img alt="" src="Pics/play buttons/black_ffw.png" 
                                style="width:20px;height:20px;cursor:pointer;float:left" title="next" /> </a>
                        
                       
                           
                       
                   </td>
                
                 </tr> 
               </table>
           
           
           </td>
               
   
         </tr>
        </table>
     

       </td>
     </tr></table>
   
      
       
        
        <%--<asp:Panel runat="server" 
        ID="Pnl_aboutUs" HorizontalAlign="Center" >

               
                     <div class="divstyles" id="divdescription" style="overflow:auto;padding:10px 10px 10px 10px;" align="left">
            	<b>About Us</b><br />
                
         
		                       <div Id="Description" runat="server"  style="text-align:justify" > </div>
            		
		            </div>
  
            </asp:Panel>--%>
           
     

      <h5 style="color:Blue;"></h5>
      

</div>
</td>
</tr>
<%--<tr>
<td>
<div class="Subheadingsection">
<asp:Label ID="Lbl_PageHeaderAnnounce" runat="server" Text="Announcements" Font-Size="18px" Font-Underline="true" ></asp:Label>        
</div>
</td>
</tr>--%>

<tr>
<td style="padding-top:10px;">
<%--<div class="GeneralAnnouncement announcement" style="width:400px;">--%>
       <%--<p  class="incidentheading announcement">Home Work</p>
       <p id="P1" runat="server"></p>--%>
       <div class="panel panel-default" style="width:370px;height:275px;float:left;">
 
        <div class="panel-heading">Home Work</div>
  
       <div class="panel-body">
         <%--<br />--%>
          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnl_Home" runat="server">
         <asp:Panel ID="Pnl_Home_More" runat="server" Visible="false">
         <div style="text-align:right;">
             <asp:LinkButton ID="Lnk_Home" runat="server" st Font-Underline="true" OnClick="Lnk_Home_Click">More Details</asp:LinkButton>
         </div>
         </asp:Panel>
      <div id="InnerHtml" runat="server" style="height:160px; overflow:auto;">

      </div>
       </asp:Panel>
         </ContentTemplate>
         </asp:UpdatePanel>
         </div>
         </div>
        <%-- </div>--%>
<%-- <div class="GeneralAnnouncement announcement" style="width:400px;">--%>
      <%-- <p  class="incidentheading announcement">General Announcement</p>--%>
       
       <div class="panel panel-default" style="width:370px;height:275px;float:right;">
 
        <div class="panel-heading">General Announcement</div>
  
       <div class="panel-body">
       
       <p id="vdo" runat="server">No Recent Videos.</p>
         <br />
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div id="Videos" runat="server">

</div>
        
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>
    
</td>
</tr>
<%--<br />

<br />--%>
<tr>
<td>
<div class="incidentarea" style="float:left;width:100%;">

  <table width="100%" cellspacing="0">
     <tr>
      <td>
      <div class="panel panel-default" style="width:190px;height:275px;float:left;">
 
        <div class="panel-heading">Academic Achivements</div>
  
       <div class="panel-body" style="overflow: auto;max-height: 230px;">
      
    <%--<div class="incidenttables academicainc">--%>
  
    <%--<p  class="incidentheading academicainc">Academic Achivements</p>--%>
    <br />
    <p id="academicarea" runat="server" >
   <%-- <b>sdsdfgft</b><br />
    Test--%>
    </p>
    </div>
      </div>
      </td>
      <td>
      <div class="panel panel-default" style="width:190px;height:275px;float:left;">
 
        <div class="panel-heading">Medical Reports</div>
  
       <div class="panel-body" style="overflow: auto;max-height: 230px;">
          <%--<div class="incidenttables medicalinc">
       <p  class="incidentheading medicalinc">Medical Reports</p>--%>
         <br />
    <p id="medicalarea" runat="server" >
  <%--  <b>sdsdfgft</b><br />
    Test--%>
    </p>
    </div>
    </div>
      
      </td>
      <td>
      <div class="panel panel-default" style="width:190px;height:275px;float:right;">
 
        <div class="panel-heading">Disciplinary Actions</div>
  
      <div class="panel-body" style="overflow: auto;max-height: 230px;">
           <%--<div class="incidenttables deciplenary  ">
       <p  class="incidentheading  deciplenary ">Disciplinary Actions</p>--%>
         <br />
    <p id="displinaryarea" runat="server" >
   <%-- <b>sdsdfgft</b><br />
    Test--%>
    </p>
    
    </div>
    </div>
      
      </td>
      <td>
         <div class="panel panel-default" style="width:190px;height:275px;float:right;">
 
        <div class="panel-heading">Others</div>
  
      <div class="panel-body" style="overflow: auto;max-height: 230px;">
      
       <%--<div class="incidenttables otherinc">
          <p  class="incidentheading otherinc">Others</p>--%>
            <br />
    <p id="otherarea" runat="server" >
 <%--   <b>sdsdfgft</b><br />
    Test--%>
    </p>
    </div>
    </div>
      
      </td>
      
     </tr>
    </table>
    
   



      
</div>
</td>
</tr>
</table>






        
 <div class="messagewindowarea" id="messagewindow" style="display:none;">        

     
                   
             <table class="tablelist" width="100%;background-color:#FFF8DC;">
                <tr>
                    <td class="rightside" colspan="2">
                        <b>Send message to admin</b>
                  
                        </td>
                </tr>              
              
                <tr>
                    <td class="leftside">
                      <span style="font-weight:bold">   Subject</span></td>
                    <td class="rightside">
                        <asp:TextBox ID="txt_subject" runat="server" width="300px" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqd_subj" runat="server"  
      ControlToValidate="txt_subject" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
       <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender1"
                                           runat="server"
                                           TargetControlID="txt_subject"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                      <span style="font-weight:bold">    Description</span></td>
                    <td class="rightside">
                     <asp:TextBox ID="txt_descrpn" runat="server" width="300px" Height="150px" TextMode="MultiLine" class="form-control"></asp:TextBox>
     <asp:RequiredFieldValidator ID="rqd_msg" runat="server"  
      ControlToValidate="txt_descrpn" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
       <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender2"
                                           runat="server"
                                           TargetControlID="txt_descrpn"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
                      </td>
                </tr>
                <tr>
                    <td class="leftside">
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Label ID="LblFailureNotice" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        
                        &nbsp;</td>
                    <td class="rightside" align="center">
                        <asp:Button ID="Btn_Send"  runat="server" class="btn btn-primary" ValidationGroup="ValidSend"   
                            Text="Send" onclick="Btn_Send_Click"  /> 
                            
                        &nbsp; &nbsp; 

                        <input type="button"  id="btn_cancel" class="btn btn-danger" onclick='return closemsgpopup();'
                            value="Cancel"  /> 
                     </td>
                </tr>
            </table><br />
            
        

</div>
   
  
  
  
   



<div style="visibility:hidden">
<asp:Panel ID="PanelHide" runat="server">
<p id="CalenderDataHide" runat="server">
</p>

</asp:Panel>

</div>

<%--<div style="width:250px;text-align:left;padding-top:25px" class="PaperStyle">

          
            <asp:Calendar ID="Calendar1" runat="server" Height="225px" Width="245px"
            onselectionchanged="Calendar1_SelectionChanged" BackColor="White" 
            BorderColor="Black" BorderStyle="Solid" CellSpacing="0" Font-Names="Verdana" 
            Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth" 
            ondayrender="Calendar1_DayRender"  
            onvisiblemonthchanged="Calendar1_VisibleMonthChanged">
            <SelectedDayStyle BackColor="#f9f7aa" ForeColor="Black" />
            <TodayDayStyle BackColor="White" ForeColor="Black"  BorderColor="Red" BorderWidth="2" BorderStyle="Solid"/>
            <OtherMonthDayStyle ForeColor="#999999"/>
            <DayStyle BackColor="White"  BorderColor="Black" BorderWidth="1" BorderStyle="Solid" />
            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black"  CssClass="Nextmonth"  />
            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" 
                                                Height="8pt" />
            <TitleStyle BackColor="#ffffff" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" Font-Bold="True" 
                                                Font-Size="12pt" ForeColor="Black"  />
            </asp:Calendar>
          

           
             </div>--%>
 
</div>

<div id="popDiv" onclick="HidePopup()" class="ontop">

</div>
			<div id="popup">
				<img id="PopupImage" width="800"/>
			</div>
</asp:Content>

