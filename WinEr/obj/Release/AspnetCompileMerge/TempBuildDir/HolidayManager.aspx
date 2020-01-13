<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="HolidayManager.aspx.cs" Inherits="WinEr.HolidayManager" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .PopupStyle
  {
      padding-top:100px;
      z-index:1000px;
  }
           .Nextmonth
        {
            padding:0px 10px 0px 10px;
        }
        .CalenderBackRed
        {
            background-image:url(images/CalenderBackRed.png);
            background-repeat:no-repeat;
            width:40px;
            height:40px;
        }
         .CalenderBackBlue
        {
            background-image:url(images/CalenderBackblue.png);
            background-repeat:no-repeat;
            width:40px;
            height:40px;
        }
        .CalenderTop
        {
            color:White;
            font-weight:bold;
            font-size:8;
            height:10px;
            width:40px;
        }
        
        .CalenderData
        {

            color:Black;
            font-weight:bold;
            font-size:10;
            width:40px;
        }
.HeaderStyle
{
    visibility:hidden;
    border:none;
}
 </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">

 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>  
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate>
<div>
 <div class="container skin1" style="min-width:800px;">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">
                
                   <table width="100%">
                    <tr>
                     <td align="left">
                      
                       Calender Manager
                      
                     </td>
                     <td align="right">
                     
                        <asp:LinkButton ID="Lnk_EventFilter" runat="server" ForeColor="Blue" Font-Size="9" Font-Bold="true"
                                 onclick="Lnk_EventFilter_Click">Switch To Calender View</asp:LinkButton>
                     
                     </td>
                    </tr>
                   </table>
                   
                
                
                </td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c">
                
                
              <asp:Panel ID="Pnl_CalenderPanel" runat="server"  Visible="false">
              
                    <table   style="margin-top:10px" width="100%">
                        <tr>
                           <td style="width:15%" align="right">
                              <asp:Label ID="lblmessg1" runat="server" class="control-label" Text="Select Class"></asp:Label>
                            </td>
                             <td  style="width:15%" align="left">
                                <asp:DropDownList ID="Drp_class" runat="server" AutoPostBack="True" Width="160px" class="form-control"
                                    onselectedindexchanged="Drp_class_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>                          
                            <td align="right">
                            
                                &nbsp;
                                <asp:LinkButton ID="Lnk_DefaultHoliday" runat="server" 
                                    onclick="Lnk_DefaultHoliday_Click">Select Default Holiday</asp:LinkButton>
                            
                            
                            </td>
                           
                           
                        </tr>
                        <tr>
                        
                           <td colspan="3" align="center">
                           
                             <asp:Calendar ID="Calendar1" runat="server" Height="300px" Width="90%" 
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
                           
                           </td>
                         
                        </tr>
                    </table>
                
                

                
                </asp:Panel>
                
                
                <asp:Panel ID="Pnl_FilterPanel" runat="server" >
                
                 <div style="min-height:400px;">
                
                    <table class="tablelist">
                    
                    
                   
                    <tr>
                    <td class="leftside">Type : </td>
                     <td class="rightside"> 
                     <asp:DropDownList ID="Drp_Type" runat="server" Width="160" class="form-control">
                            <asp:ListItem Value="0" Text="Events and Holidays" Selected="True"></asp:ListItem>
                             <asp:ListItem Value="1" Text="Events"></asp:ListItem>
                              <asp:ListItem Value="2" Text="Holidays"></asp:ListItem>
                       </asp:DropDownList></td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td class="leftside"> Duration : </td>
                      <td class="rightside"><asp:DropDownList ID="Drp_FilterType" runat="server" class="form-control" Width="160px"   
                           AutoPostBack="true" 
                           onselectedindexchanged="Drp_FilterType_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="This Year"  Selected="True"></asp:ListItem>
                            <asp:ListItem Value="2" Text="This Month"></asp:ListItem>
                            <asp:ListItem Value="1" Text="This Week"></asp:ListItem>
                             <asp:ListItem Value="3" Text="Next Week"></asp:ListItem>
                             <asp:ListItem Value="4" Text="Manual"></asp:ListItem>
                       </asp:DropDownList></td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td class="leftside"> From :</td>
                      <td class="rightside"><asp:TextBox ID="TxtStartDate" runat="server" Enabled="false" class="form-control" Width="160px"></asp:TextBox>
                   
                                <ajaxToolkit:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="TxtStartDate" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>  
                                <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="TxtStartDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                          ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="TxtStartDate">
                                                    </ajaxToolkit:MaskedEditExtender>   
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" /></td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td class="leftside">To : </td>
                      <td class="rightside"> <asp:TextBox ID="TxtEndDate" runat="server"  Enabled="false" class="form-control" Width="160px"></asp:TextBox>
                     
                                
                                  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="TxtEndDate" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>  
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                                        runat="server" ControlToValidate="TxtEndDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                          ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="TxtEndDate">
                                                    </ajaxToolkit:MaskedEditExtender>   
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="RegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                                
                         &nbsp;&nbsp;
                         
                         
                                </td>
                    </tr>
                    
                    <tr><td class="leftside">&nbsp;</td>
                    <td class="rightside"><asp:Button ID="Btn_Load" runat="server" Text="Load" Class="btn btn-primary" OnClick="Btn_Load_Click"/>       </td>
                    </tr>
                    <tr><td colspan="2" align="left">
                    
                       <asp:ImageButton ID="Img_Excel" runat="server" ImageUrl="~/Pics/Excel.png" 
                           Width="30"  ToolTip="Export To Excel" onclick="Img_Excel_Click" ImageAlign="AbsMiddle"/>
                        <asp:LinkButton ID="Lnk_Export" runat="server" onclick="Lnk_Export_Click">Export To Excel</asp:LinkButton>
                      &nbsp;
                            &nbsp;
                      <asp:ImageButton ID="Img_AddEvent" runat="server" ImageAlign="AbsMiddle"
                           ImageUrl="~/images/CalenderBackblue.png" Width="25" ToolTip="Add Event" 
                           onclick="Img_AddEvent_Click" />
                       <asp:LinkButton ID="Lnk_AddEvent" runat="server" onclick="Lnk_AddEvent_Click">Add Event</asp:LinkButton>
                    &nbsp;
                            &nbsp;
                      <asp:ImageButton ID="Img_AddHoliday" runat="server" ImageAlign="AbsMiddle"
                           ImageUrl="~/images/CalenderBackRed.png" Width="25" ToolTip="Add Holiday" 
                           onclick="Img_AddHoliday_Click" />
                      <asp:LinkButton ID="Lnk_AddHoliday" runat="server" 
                           onclick="Lnk_AddHoliday_Click">Add Holiday</asp:LinkButton>
                    
                    </td></tr>
                    
                     <tr><td colspan="2"><div id="Div1" class="linestyle" runat="server"></div></td></tr>
                    <tr><td colspan="2" align="center"><asp:Label ID="Lbl_ListGrid_Msg" runat="server" class="control-label" Text="" ForeColor="Red"></asp:Label></td></tr>
                    
                     <tr><td colspan="2">
                     <asp:GridView ID="Grid_EventList" runat="server" AutoGenerateColumns="False" SkinID="GrayOnlyFooterstyle"
                           Width="100%"  AllowPaging="True" GridLines="Horizontal" BorderStyle="None"
                           onselectedindexchanged="Grid_EventList_SelectedIndexChanged" 
                           onpageindexchanging="Grid_EventList_PageIndexChanging" >
                                 <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id"/>
                                    <asp:BoundField DataField="Date" HeaderText="Date"/>
                                    <asp:BoundField DataField="Type" HeaderText="Type"/>
                                    <asp:TemplateField ItemStyle-Width="110px" HeaderText="Date">
                                    <ItemTemplate>
                                    
                                       <%# Eval("Divinfo")%>
                                        
                                    </ItemTemplate>
                                        <ItemStyle Width="110px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Font-Bold="true"/>
                                    <asp:CommandField ItemStyle-Width="105" 
                                         SelectText="&lt;img src='pics/edit.png' width='30px' border=0 title='Edit'&gt;"  
                                         ShowSelectButton="True" HeaderText="Edit" >
                                        <ItemStyle Width="105px" />
                                     </asp:CommandField>
                                 </Columns>
                               <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5"   PreviousPageText="&lt;&lt;" />
                               <PagerStyle  Font-Bold="true" />
                             <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" /> 
                              <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="25px" HorizontalAlign="Left" />                                                   
                                 <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                 <EditRowStyle Font-Size="Medium" />   
                      </asp:GridView>
                     </td></tr>
                    
                    </table>          
                
                 </div>
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

<%--messagebox--%>

            <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground"  />
                         <asp:Panel ID="Pnl_msg" runat="server" style="display:none"><%-- style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n">
            
            <table width="100%">
                 <tr>
                  <td>
                   <span style="color:Black"><span style="color:Black">Message</span></span>
                  </td>
                  
                  <td align="right">
                      <asp:LinkButton ID="LinkButton_AddEvent"  Font-Size="10" ForeColor="Blue" runat="server" Visible="false" onclick="Lnk_GotoEvent_Click">Manage Event</asp:LinkButton>
                  </td>
                 </tr>
                
                </table>
            
            </td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
              
                        <div style="text-align:center;">
                              <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                            <asp:Button ID="Btn_magok" runat="server" Text="Ok" class="btn btn-info" Width="100px"/>
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

<%--add holiday--%>
  <asp:Button runat="server" ID="btnaddholidaypnl" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_addholiday" runat="server"  
                         PopupControlID="pnl_addholiday" TargetControlID="btnaddholidaypnl" BackgroundCssClass="modalBackground" />
                         <asp:Panel ID="pnl_addholiday" runat="server" style="display:none;"> <%-- style="display:none;"--%>
                         <div class="container skin1" style="width:400px;top:400px;left:400px;" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                                                Height="28px" Width="29px" /> </td>
                                    <td class="n"><span style="color:Black">Manage Holiday</span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                       
                                       <table width="100%">
                                       <tr>
                                       <td align="center">
                                       No Of Days
                                      </td>
                                      <td>
                                        <asp:TextBox ID="Txt_number" runat="server" MaxLength="8" class="form-control"
                                            style="text-align:center" ></asp:TextBox>
                                        <asp:ImageButton ID="img1" runat="server" AlternateText="Down" Height="15" 
                                            ImageUrl="images/down.gif" Width="15" />
                                        <asp:ImageButton ID="img2" runat="server" AlternateText="Up" Height="15" 
                                            ImageUrl="images/up.gif" Width="15" />
                                        <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender4" runat="server" 
                                            Minimum="1" Maximum="20" RefValues="" ServiceDownMethod="" ServiceUpMethod="" 
                                            TargetButtonDownID="img1" TargetButtonUpID="img2" TargetControlID="Txt_number" 
                                            Width="80" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_number_FilteredTextBoxExtender1" 
                                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_number">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                      
                                          
                                       </td> 
                                       <td align="left">
                                         <asp:CheckBox ID="Chk_defaultHolidayStatus" runat="server" Text="Include Default Holiday" />
                                       </td>                                    
                                       </tr>
                                       <tr>
                                       <td align="center">
                                       Description
                                        
                                    </td> 
                                    <td colspan="2">
                                       <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" class="form-control" MaxLength="250" Width="250px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                         runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'/\" TargetControlID="txtdescription">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                                         
                                       </td>
                                       </tr>
                                       <tr>
                                       <td>
                                       
                                           <asp:Label ID="lblstartdate" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                                       
                                       </td>
                                       <td  colspan="2">
                                       <asp:Label ID="lblmode" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                                       &nbsp;&nbsp;&nbsp;
                                          <asp:Label ID="lblclass" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                                       </td>
                                       </tr>
                                       <tr>
                                       <td colspan="3" align="center" style="color:Red">
                                          <asp:Label ID="lblerror" runat="server" class="control-label" Text=""></asp:Label>
                                       
                                       </td>
                                       </tr>
                                       </table>
                                                <div style="text-align:center;">                                                    
                                                    <asp:Button ID="btn_addholiday" runat="server" Text="Ok" Width="80px" class="btn btn-info"                                                        onclick="btn_addholiday_Click"/>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btn_cancelholiday" runat="server" Text="Close" Width="80px" class="btn btn-info"
                                                        onclick="btn_cancelholiday_Click"/>
                                                    
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
        
 
                      
                      
       <%--edit holiday details --%>       
       
       <asp:Button runat="server" ID="btneditdetails" style="display:none" class="btn btn-info"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_editdetailsofholiday" runat="server" PopupControlID="pnl_editholidaydetails" TargetControlID="btneditdetails" BackgroundCssClass="modalBackground" />
                         <asp:Panel ID="pnl_editholidaydetails" runat="server" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin1" style="width:450px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n">
                
                <table width="100%">
                 <tr>
                  <td>
                   <span style="color:Black">Edit Holiday Details</span>
                  </td>
                  
                  <td align="right">
                      <asp:LinkButton ID="Lnk_GotoEvent"  Font-Size="10" ForeColor="Blue" runat="server" onclick="Lnk_GotoEvent_Click">Manage Event</asp:LinkButton>
                  </td>
                 </tr>
                
                </table>
            
               </td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <table width="100%">
                <tr>
                <td>
                Holiday Assigned To

                
                </td>
                <td>
                   <asp:Label ID="lblallclass" runat="server" Font-Bold="true" class="control-label" Text=""></asp:Label> 
                
                </td>
                </tr>
                <tr>
                <td>
                  <asp:Label ID="lbl_date" runat="server" Text="" class="control-label" Visible="false"></asp:Label>           
                 <asp:Label ID="lbl_edit_holidayid" runat="server" class="control-label" Text="" Visible="false"></asp:Label>                        
                &nbsp;<asp:Label ID="lbl_editmode" runat="server" Text="" Visible="false" class="control-label"></asp:Label>
                
                </td>
                <td>
                <asp:Label ID="lbl_editclass" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
              
                </td>
                </tr>
                <tr>
                <td>
                 <asp:Label ID="lbldesc" runat="server" class="control-label" Text="Change Holiday Description"></asp:Label> 
                </td>
                <td>
                  <asp:TextBox ID="txt_editdescription" runat="server" class="form-control"  MaxLength="250" TextMode="MultiLine"></asp:TextBox> 
                  <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'/\" TargetControlID="txt_editdescription">
                           </ajaxToolkit:FilteredTextBoxExtender>
                   
                </td>
                </tr>
                </table>
              
                            
                                    
              <div style="text-align:center;margin:10px">   
                             <asp:Button ID="btn_updatedetls" runat="server" Text="Update" Class="btn btn-info" 
                                 onclick="btn_updatedetls_Click" /> 
              
                            &nbsp;&nbsp;
                             <asp:Button ID="btn_deletedtls" runat="server" Text="Delete" Class="btn btn-info" 
                                onclick="btn_deletedtls_Click"/>        
                                   &nbsp;&nbsp;
                             <asp:Button ID="Btn_cancelediting" runat="server" Text="Close" Class="btn btn-info" 
                                 onclick="Btn_cancelediting_Click"/>                         
                            <br />
                        <asp:Label ID="lblupdateerror" runat="server" ForeColor="#FF3300" class="control-label"></asp:Label> 
                            
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
                        
                        
                        
       <%--Default holiday details --%>       
       
       <asp:Button runat="server" ID="Button1" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="M_DefaultHoliday" runat="server" PopupControlID="Panel1" CancelControlID="Btn_Cancel" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
                         <asp:Panel ID="Panel1" runat="server" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin1" style="width:450px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Manage Default Holiday</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                
              
              <table width="100%" cellspacing="10px">
              
               <tr>
                 <td align="center">
                   <asp:GridView ID="Grd_Defaultholiday" runat="server" CellPadding="4" ForeColor="Black"  GridLines="Horizontal" AutoGenerateColumns="False" EnableTheming="false"
                                 Width="70%"   BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px">
                                 <Columns>
                                   <asp:TemplateField HeaderText="Student" ItemStyle-Width="60px"   >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxStudent" runat="server" />
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff" ItemStyle-Width="60px" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxStaff" runat="server" />
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status"/>
                                    <asp:BoundField DataField="StaffStatus" HeaderText="StaffStatus"/>
                                    <asp:BoundField DataField="Id" HeaderText="Id"/>
                                    <asp:BoundField DataField="Day" HeaderText="Day"  ItemStyle-HorizontalAlign="Left" />
                                 </Columns>
                               <RowStyle BackColor="White" />
                               <FooterStyle BackColor="#CCCC99"/>
                               <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                               <AlternatingRowStyle BackColor="White" />
                             </asp:GridView>
                     <asp:Label ID="Lbl_Gridmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                 </td>
               </tr>
               <tr>
                 <td align="center" >
                    <asp:Button ID="Btn_DefaultHoliday_update" runat="server" Text="Update" 
                               class="btn btn-info" onclick="Btn_DefaultHoliday_update_Click" 
                                  /> 
                            &nbsp;&nbsp;
                    <asp:Button ID="Btn_Cancel" runat="server" Text="Close" class="btn btn-info" />
                 </td>
               </tr>
              
              
              </table>              
                                    

                         
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
                        
                        
                        
                        
                          <%--Select Day --%>       
       
 <asp:Button runat="server" ID="Button2" class="btn btn-info" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_SelectDay" runat="server" PopupControlID="Panel2" TargetControlID="Button2" BackgroundCssClass="modalBackground" />
 <asp:Panel ID="Panel2" runat="server" style="display:none;"><%--style="display:none;"--%>
 <div class="container skin1" style="width:450px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Select Action</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                
              
                 <table width="100%" cellspacing="10">
                  <tr>
                   <td>
                       <asp:Label ID="Label1" runat="server" Text="Select action to perform" class="control-label" ForeColor="Black"></asp:Label>
                     
                     
                   </td>
                  </tr>
                  <tr>
                    <td>
                      <asp:RadioButtonList ID="Rdb_Type" runat="server">
                       <asp:ListItem Text="Manage Holiday" Value="0" Selected="True"></asp:ListItem>
                       <asp:ListItem Text="Manage Events" Value="1"></asp:ListItem>
                      </asp:RadioButtonList>
                   </td>
                  </tr>
                  <tr>
                   <td align="center">
                       <asp:Button ID="Btn_Continue" runat="server" Text="Continue" class="btn btn-info"
                           onclick="Btn_Continue_Click" />
                       
                       &nbsp;
                       
                       <asp:Button ID="Btn_Close" runat="server" Text="Close" class="btn btn-info" 
                           onclick="Btn_Close_Click" />
                   </td>
                  </tr>
                 </table>
                                    
              
                         
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
         
         
         
         
                           <%--Add Events --%>       
       
 <asp:Button runat="server" ID="Button3" class="btn btn-info" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_AddEvent" runat="server" PopupControlID="Panel3" TargetControlID="Button3" BackgroundCssClass="modalBackground" />
 <asp:Panel ID="Panel3" runat="server" style="display:none;" CssClass="PopupStyle"><%--style="display:none;"--%>
 <div class="container skin1" style="width:750px; top:400px;left:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Add Events</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                
              
                 <table width="100%" cellspacing="10">

                  <tr>
                   <td style="width:25%" align="right">
                    Event Name
                   </td>
                   <td  style="width:25%">
                   
                       <asp:TextBox ID="Txt_EventName" runat="server" Width="140px" class="form-control" MaxLength="25"></asp:TextBox>
                      <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'/\" TargetControlID="Txt_EventName">
                           </ajaxToolkit:FilteredTextBoxExtender>
                   </td>
                   <td style="width:25%" align="right">
                     No. of Event days
                   </td>
                   <td  style="width:25%">
                     <asp:TextBox ID="Txt_NoEventDays" runat="server" Width="140px" class="form-control" MaxLength="2"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_NoEventDays">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                   </td>
                  </tr>
                  <tr>
                   <td style="width:25%" align="right">
                   
                   </td>
                   <td  style="width:25%">
                       <asp:CheckBox ID="Chk_DefaultHolidayForEvent" runat="server" Text="Include Default Holiday" Checked="false" />
                   </td>

                   <td style="width:25%" align="right">
                   
                   </td>
                   <td  style="width:25%">
                       <asp:CheckBox ID="Chk_Publish" runat="server" Text="Publish" Checked="false" />
                   </td>
                  </tr>
                   <tr>
                   <td colspan="4" align="left">
                    Event Description

                   
                      <HTMLEditor:Editor ID="Txt_EventDescriptionHtml" runat="server" Height="150px" Width="100%" />
                     
                   
                   </td>
                  </tr>
                  
                   
                  <tr>
                   <td colspan="4" align="center">
                       <asp:Label ID="Lbl_EventMsg" runat="server" Text="" ForeColor="Red" class="control-label" Font-Bold="true"></asp:Label>
                   </td>
                  </tr>
                  <tr>
                   <td colspan="4" align="center">
                     
                       <asp:Button ID="Btn_AddEvent" runat="server" Text="Add Event" 
                           class="btn btn-info" onclick="Btn_AddEvent_Click" />
                       
                       &nbsp;
                       
                       <asp:Button ID="Btn_EventCLose1" runat="server" Text="Close" 
                           class="btn btn-info" onclick="Btn_EventCLose1_Click" />
                   
                   </td>
                  </tr>
                 </table>
                                    
              
                         
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
       
       
       
       
                                  <%--Edit Events --%>       
       
 <asp:Button runat="server" class="btn btn-info" ID="Button4" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_EditEvent" runat="server" PopupControlID="Panel4" TargetControlID="Button4" BackgroundCssClass="modalBackground" />
 <asp:Panel ID="Panel4" runat="server"  style="display:none;" CssClass="PopupStyle"><%--style="display:none;"--%>
 <div class="container skin1" style="width:750px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image7" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n">
            
             <table width="100%">
                 <tr>
                  <td>
                   <span style="color:Black">Edit Events</span>
                  </td>
                  
                  <td align="right">
                      <asp:LinkButton ID="Lnk_GotoHoliday" runat="server"  Font-Size="10" ForeColor="Blue"
                          onclick="Lnk_GotoHoliday_Click">Manage Holiday</asp:LinkButton>
                  </td>
                 </tr>
                
                </table>
            
            </td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                
                <asp:HiddenField ID="Hdn_EventId" runat="server" />
              
                 <table width="100%" cellspacing="10">
                  <tr>
                   <td style="width:25%" align="right">
                    Event Name
                   </td>
                   <td  style="width:25%">
                   
                       <asp:TextBox ID="Txt_EditEventName" class="form-control" runat="server" Width="140px" MaxLength="25"></asp:TextBox>
                       <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'/\" TargetControlID="Txt_EditEventName">
                           </ajaxToolkit:FilteredTextBoxExtender>
                   
                   </td>
                    <td style="width:25%" align="right">
                   
                   </td>
                   <td  style="width:25%">
                       <asp:CheckBox ID="Chk_EditEventPublish" runat="server" Text="Publish" Checked="false" />
                   </td>
                  </tr>
                  
                   <tr>
                   <td colspan="4" align="left">
                    Event Description

                       <HTMLEditor:Editor ID="Txt_EditEventDescriptionHtml" runat="server" Height="150px" Width="100%" />
                   </td>
                  </tr>
                  

                  <tr>
                   <td colspan="4" align="center">
                       <asp:Label ID="Lbl_EventUpdateMsg" runat="server" class="control-label" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                   </td>
                  </tr>
                  <tr>
                   <td colspan="4" align="center">
                     
                       <asp:Button ID="Btn_UpdateEvent" runat="server" Text="Update" class="btn btn-info" 
                           onclick="Btn_UpdateEvent_Click"  />
                       
                       &nbsp;
                       
                       <asp:Button ID="Btn_DeleteEvent" runat="server" Text="Delete" class="btn btn-info"
                           onclick="Btn_DeleteEvent_Click"  />
                       
                        &nbsp;
                       
                       <asp:Button ID="Btn_EventClose2" runat="server" Text="Close"  class="btn btn-info"
                           onclick="Btn_EventClose2_Click"/>
                   
                   </td>
                  </tr>
                 </table>
                                    
              
                         
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
        
        
                                 <%--Delete Events --%>       
       
 <asp:Button runat="server" ID="Button5" class="btn btn-info" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="Mpe_DeleteEvent" runat="server" PopupControlID="Panel5" TargetControlID="Button5" BackgroundCssClass="modalBackground" />
 <asp:Panel ID="Panel5" runat="server" style="display:none;"><%--style="display:none;"--%>
 <div class="container skin1" style="width:450px; top:500px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image8" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Delete Events</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                
              <table width="100%">
               <tr>
                <td style="padding-left:70px;">
                  
                    <asp:RadioButtonList ID="Rdb_EventDeleteionType" runat="server" CellSpacing="10">
                      <asp:ListItem Text="Delete the Event for selected day" Value="0" Selected="True"></asp:ListItem>
                      <asp:ListItem Text="Delete the Entire Event" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                
                </td>
               </tr>
               <tr>
                <td align="center">
                    <asp:Button ID="Btn_DeleteButton" runat="server" Text="Delete" 
                        class="btn btn-info" onclick="Btn_DeleteButton_Click" />
                    
                    &nbsp;<asp:Button ID="Btn_EventClose3" runat="server" Text="Close" 
                        class="btn btn-info" onclick="Btn_EventClose3_Click" />
                    
                </td>
               </tr>
              </table>
              
                 
                                    
              
                         
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
           
</div>
    <WC:MSGBOX id="WC_MessageBox" runat="server" />                  

 <asp:HiddenField ID="Hd_SelectedDate" runat="server" />

      </ContentTemplate> 
      <Triggers>
       <asp:PostBackTrigger ControlID="Img_Excel" />
       <asp:PostBackTrigger ControlID="Lnk_Export" />
      </Triggers>
    </asp:UpdatePanel>
<div class="clear"></div>
</div>
</asp:Content>
