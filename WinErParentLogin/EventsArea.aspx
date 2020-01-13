<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="EventsArea.aspx.cs" Inherits="WinErParentLogin.EventsArea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        .style1
        {
            width: 100%;
            color:Black;
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

  </style>
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
        .style1
        {
            width: 100%;
            color:Black;
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
</style>
  <script type="text/javascript">

            function LoadPopup(name) {
                var Desc;
                
                var PanelHide = document.getElementById('<%=PanelHide.ClientID%>');
                var Hd_Event = PanelHide.innerHTML;
             
                var Array1 = Hd_Event.split('$%$');
              
                for (var i = 0; i < Array1.length; i++) {
                    alert(Array1[i]);
                    var strArray = Array1[i].split('*-*');
                    alert(name);
                    
                    if (strArray[0] == name) {
                        Desc = strArray[1];
                    }
                }

               
                var HtmlControl = document.getElementById('<%=HtmlID.ClientID%>');
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
            }).slideshow();
        });

    </script>
         
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   <div style="visibility:hidden">
<asp:Panel ID="PanelHide" runat="server">
<p id="CalenderDataHide" runat="server">
</p>

</asp:Panel>

</div>

<div style="width:250px;text-align:left;padding-top:25px" class="PaperStyle">

          
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
          

           
</div>         
           <asp:Panel ID="Panel4" runat="server">
                       
   <asp:Button runat="server" ID="Button2" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MpE_Event"  runat="server" CancelControlID="Button3" PopupControlID="Panel5" TargetControlID="Button2"  BackgroundCssClass="modalBackground" BehaviorID="EventModalPopupBehavior" />
   <asp:Panel ID="Panel5" runat="server" style="display:none;">
   <div class="container skin1" style="width:600px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Event</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <center>
               
                
               
                <p><b id="HtmlID" runat="server">dude</b></p> 
                
                </center>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button3" runat="server" Text="OK" Width="50px"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />                  
  </div>
 </asp:Panel>                 
</asp:Panel>
</asp:Content>
