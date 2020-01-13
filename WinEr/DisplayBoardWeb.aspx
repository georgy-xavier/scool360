<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayBoardWeb.aspx.cs" Inherits="WinEr.DisplayBoardWeb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WinER School</title>
    
        <script type="text/javascript" src="js files/jquery.tools.min.js"></script>
    <style type="text/css">
        a:link {text-decoration: none;} 
        a:visited {text-decoration: none;} 
        a:hover {text-decoration: underline;}
        

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
          width:98%;
      }
      
      
      .EventSlide
      {
          width:98%;
      }
      
            
      .AttendanceSlide
      {
          width:98%;
      }
    
      .Dashboardcss
        {
            background:#fff url(images/h300.png) repeat-x;
            background-position:top;
            border:solid 1px gray;
            color:Black;
         }
       </style>
     
     <style type="text/css">
     
  /* container for slides */
.images {
	border:1px solid #ccc;
	position:relative;	
	
	width:99%;
	float:left;	
	margin:5px;
	cursor:pointer;
    
	
	/* CSS3 tweaks for modern browsers */
	-moz-border-radius:5px;
	-webkit-border-radius:5px;
	-moz-box-shadow:0 0 25px #666;
	-webkit-box-shadow:0 0 25px #666;	
}

/* single slide */
.images div {
	display:none;
	position:absolute;
	top:0;
	left:0;		
	margin:3px;
	padding:5px 5px 5px 5px;
	height:435px;
	font-size:12px;
}

/* header */
.images h3 {
	font-size:22px;
	font-weight:normal;
	margin:0 0 20px 0;
	color:#456;
}

/* tabs (those little circles below slides) */
.slidetabs {
	clear:both;
}

/* single tab */
.slidetabs a {
	width:8px;
	height:8px;
	float:left;
	margin:3px;
	background:url(../images/slide.gif) 0 0 no-repeat;
	display:block;
	font-size:1px;		
}

/* mouseover state */
.slidetabs a:hover {
	background-position:top;      
}

/* active state (current page state) */
.slidetabs a.current {
	background-position:bottom;     
} 	


/* prev and next buttons */
.forward, .backward {
	cursor:pointer;	
}



/* disabled navigational button. is not needed when tabs are configured with rotate: true */
.disabled {
	visibility:hidden !important;		
}
     
     </style>
       


    
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="JSDIV" runat="server">
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
    </div>
    
        
    
    <div>
            <table width="100%" cellspacing="0">  <%--Content Slider--%>
 
         <tr>
          <td >
             <!-- container for the slides -->
             
            <div id="ImagesDiv" runat="server"  >
            
                          
             <center>       
                       <br /> <br />
                        <br />
                         <br />
                         
                          <br />
                          <br />
              <h1> WRONG CREDENTIALS </h1>

            </center>   
            
            </div>
             
            
          
          </td>
         </tr>
         <tr>
         
           <td style="padding-left:5px;" align="left" >
           
            <table width="99%">
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
    </div>
    </form>
</body>
</html>
