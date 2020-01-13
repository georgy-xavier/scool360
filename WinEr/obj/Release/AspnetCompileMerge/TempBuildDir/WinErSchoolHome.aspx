<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="WinErSchoolHome.aspx.cs" Inherits="WinEr.WinErSchoolHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link rel="stylesheet" type="text/css" href="css files/ContentSlide.css"/>
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
       $(document).ready(function () {
           
           $("#StaffDiv,#HomeInfo").html(circleLoader);
           $('#Lbl_SchoolName').html('<div class="animated_bg_short_loder rectangle"></div><div class="animated_bg_short_loder loderline"></div>');
           $("#ImagesDiv").html('<br><div class="animated_bg_short_loder circle"></div><div class="animated_bg_short_loder rectangle"></div><br>');
           loadInstDt();//load rom common method
           winerSchoolHome_aspx.loadDt();
       });
     
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true" />
   <%-- Update Panel should not be used in this page--%>
   <asp:Panel runat="server" Width="100%"  ID="Panel3" HorizontalAlign="Center">
    <div class="customContainer">
        <div id="HomeschName" class="customContainer card0">
            <div class="col-lg-2">
                <asp:Image ID="Img_Logo" runat="server" ImageUrl="Handler/ImageReturnHandler.ashx?id=1&type=Logo" Height="100px" Width="100px" />
            </div>
            <div class="col-lg-8">
                <div class="row" style="margin-top: 20px;font-weight: bolder;">
                    <div id="Lbl_SchoolName" class="control-label" style="font-Size:Large;color:#003366;"></div>
                </div>
                <div class="row" >
                    <div id="Lbl_Subhead" class="control-label" style="font-Size:Small;color:#003366;"></div>
                </div>
                <div class="row">
                    <hr>
                    <h4 style="color: #9E9E9E;font-weight: 100;">School Manager Dashboard</h4>
                </div>
            </div>
            <div class="col-lg-2">
                <div class="text-center Custtooltip" style="margin-top: 20px;">
                    <i class="fa fa-desktop" style="font-size: 38px; color: #3F51B5;"></i>
                    <br>
                    <label class="control-label">Switch Dashboard</label><br>
                    <%--<asp:LinkButton ID="principaldashboard" runat="server" OnClick="principaldashboard_Click" CssClass="switchdash">Principal  Dashboard</asp:LinkButton>--%>
                    <a href="PrincipalDashboard.aspx" class="switchdash" data-placement="top" data-toggle="tooltip" title="Click here to view Principal Dashboard">Principal  Dashboard</a>
                </div>
            </div>
        </div>
    </div>
<%--    <hr />--%>
    <div class="row customContainer">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="thumbnail card0" style="border-radius:0;padding:10px;padding-top: 10px;min-height:350px;max-height:350px;margin-top: 20px;">
                <%--<div class="col-lg-6" style="width:250px;text-align:left" class="PaperStyle">--%>
                <%-- <div>--%>
                <div style="height: 35px" class="form-inline">
                    <a href="HolidayManager.aspx" class="AddEvnt" title="Click to add a new event" style="color: #3F51B5;">
                        <span><i class="fa fa-plus-square-o"></i></span>&nbsp;Add Event
                    </a>
                </div>
                <asp:Calendar ID="Calendar1" runat="server" Height="290px" Width="100%"
                         BackColor="White"
                        BorderStyle="Solid" BorderColor="White" CellSpacing="0" Font-Names="sans-serif"
                        Font-Size="11pt" ForeColor="Black" NextPrevFormat="ShortMonth"
                        OnDayRender="Calendar1_DayRender"
                        OnVisibleMonthChanged="Calendar1_VisibleMonthChanged">
                        <SelectedDayStyle BackColor="#f9f7aa" ForeColor="Black" />
                        <TodayDayStyle  ForeColor="white" BorderStyle="Double" BorderWidth="1px" BorderColor="#4107a8" CssClass="calTodBorderWidth" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <DayStyle BackColor="White" BorderWidth="1" BorderStyle="Solid" BorderColor="White" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black" CssClass="Nextmonth" />
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333"
                            Height="8pt" />
                        <TitleStyle BackColor="#ffffff" BorderStyle="Solid" BorderColor="White" BorderWidth="1" Font-Bold="True"
                            Font-Size="12pt" ForeColor="Black" />
                </asp:Calendar>
                </div>
            </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div id="sliderCard" class="thumbnail card0" style="border-radius:0;padding:10px;padding-top:10px;min-height:350px;max-height:350px;margin-top: 20px;">
                <div id="ImagesDiv"></div>
                <div class="row">
                    <div class="SlideArrows col-lg-3 col-md-3 col-xs-6" style="padding-top:10px;display:none; ">
                        <a class="backward"><i class="fa fa-angle-left" style="font-size:25px;color:#3F51B5;padding:10px;"></i></a>
                        <a class="forward"><i class="fa fa-angle-right" style="font-size:25px;color:#3F51B5;"></i></a>	
                    </div>
                    <div class="col-lg-9 col-md-9 col-xs-6" style="margin-top:20px;">
                        <div id="SlideTabsDiv"></div>
                    </div>					
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
                <div id="birthDCard" class="thumbnail card0" style="border-radius:0;padding:2px;padding-top:10px;min-height:350px;max-height:350px;margin-top: 20px;">
                        <asp:Panel ID="Panel2" runat="server">
                            <div id="BirthdayBack">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width: 50%;" id="BirthdayHeading">
                                            <i class="fa fa-birthday-cake" style="font-size: 25px;margin-right: 20px;color: #009688;"></i>Birthday List
                                        </td>
                                                
                                        <td style="width: 30%;" class="BirthdayHeadingImage">
                                            <a href="BirthdayList.aspx" class="AddEvnt" title="Click to Send BirthDay Wishes" style="color: #3F51B5;">
                                                <span><i class="fa fa-envelope-o"></i></span>&nbsp;Send Wishes
                                            </a>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left">
                                                <ajaxToolkit:tabcontainer runat="server" ID="Tabcontainer1" Width="100%" 
                                                CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True" >
                                              
                                                    <ajaxToolkit:TabPanel runat="server" ID="TabStaffBirthDay" HeaderText="Staff" Visible="true" Width="50%">
                                                            <HeaderTemplate>
                                                                <div id="staffHead" style="font-size: 20px;margin-bottom: -20px;width: 120px;">
                                                                    <i id="stafHd" class="fa fa-users"></i><span><p>Staff Birthday</p></span>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <div id="StaffDiv"></div>
                                                            </ContentTemplate>
                                                        </ajaxToolkit:TabPanel>
                                                </ajaxToolkit:tabcontainer>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
            </div>
    </div>
    <hr>
    <div id="HomeInfo"></div>
    <hr>
    </asp:Panel>
   <asp:Panel ID="Panel1" runat="server">
      <asp:Button runat="server" ID="Button2" style="display:none"/>
      <ajaxToolkit:ModalPopupExtender ID="MpE_Event"  runat="server" CancelControlID="Button3" PopupControlID="Panel4" TargetControlID="Button2"  BackgroundCssClass="modalBackground" BehaviorID="EventModalPopupBehavior" />
      <asp:Panel ID="Panel4" runat="server" style="display:none;">
         <div class="container skin1" style="width:600px; top:400px;left:400px" >
            <table   cellpadding="0" cellspacing="0" class="containerTable">
               <tr >
                  <td class="no">
                     <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" />
                  </td>
                  <td class="n"><span style="color:Black">Event</span></td>
                  <td class="ne">&nbsp;</td>
               </tr>
               <tr >
                  <td class="o"> </td>
                  <td class="c" >
                     <center>
                        <p><b id="HtmlID">dude</b></p>
                     </center>
                     <br /><br />
                     <div style="text-align:center;">
                        <asp:Button ID="Button3" runat="server" class="btn btn-info" Text="OK" />
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
   <div class="clear"></div>
   <div style="visibility:hidden">
      <asp:Panel ID="PanelHide" runat="server">
         <p id="CalenderDataHide" runat="server"></p>
      </asp:Panel>
   </div>
</asp:Content>