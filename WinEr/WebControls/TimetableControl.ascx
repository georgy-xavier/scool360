<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimetableControl.ascx.cs" Inherits="WinEr.WebControls.TimetableControl" %>
<asp:HiddenField ID="HiddenClassId" runat="server" />
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<link href="css files/MasterStyle.css" rel="stylesheet" type="text/css" />
<link href="css files/winbuttonstyle.css" rel="stylesheet" type="text/css" />
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>

<table width="100%" cellspacing="10">
           <tr>
                                   
             <td align="center">
                                   
                                    <table width="900px" cellspacing="0">
                                      <tr>
                                       <td colspan="6">
                                       
                                        <asp:GridView ID="Grid_TTConfig" runat="server"   Visible="true" 
                                             CellPadding="4" ForeColor="Black" GridLines="Both"  AutoGenerateColumns="False" 
                                             Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" 
                                             BorderWidth="1px" >
                                             <RowStyle BackColor="#F7F7DE" />
                                             <Columns>    
                                                <asp:BoundField DataField="Subject" HeaderText="Subject" >   
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                 </asp:BoundField>
                                                <asp:BoundField DataField="Staff" HeaderText="Staff" />   
                                                <asp:BoundField DataField="NoPeriods" HeaderText="NO of Periods"  />  
                                                <asp:BoundField DataField="AllotedPeriods" HeaderText="Allotted Periods" /> 
                                              </Columns>
                                              <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                              <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                              <HeaderStyle BackColor="#6e6e6e" Font-Bold="True" Font-Size="13px" ForeColor="White" Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                                              <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Gray" Font-Bold="true"  HorizontalAlign="Left" Height="20px" />
                                              <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                              <EditRowStyle Font-Size="Medium" />     
                                           </asp:GridView>
                                       
                                       </td>
                                      </tr>
                                      <tr>
                                               <td class="SubHeaderStyle" align="right" style="width:25%;">
                                                 Total Configured Periods :

                                                  
                                               </td>
                                               <td align="left" class="SubHeaderStyle"  style="width:10%;">
                                               
                                                   <asp:Label ID="Lbl_ConfiguredPeriods" runat="server" Text="" Font-Bold="true"></asp:Label>
                                               
                                               </td>

                                                <td class="SubHeaderStyle"  align="right" style="width:20%;">
                                                  Allotted Periods :

                                                  
                                               </td>
                                                 <td align="left" class="SubHeaderStyle" style="width:10%;">
                                                 
                                                 <asp:Label ID="Lbl_AllotedPeriods" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                 
                                                 </td>
                                                <td class="SubHeaderStyle" align="right" style="width:20%;">
                                                  Free Periods :

                                                  
                                               </td>
                                               <td align="left" class="SubHeaderStyle" style="width:10%;">
                                                 
                                                 <asp:Label ID="Lbl_FreePeriods" runat="server" Text="" Font-Bold="true"></asp:Label>
                                               
                                               </td>

                                      </tr>
                                     </table>
                                   
             </td>
          </tr>
          <tr>
             <td align="center">
                                 <br />
                                     <div style="width:900px; overflow:auto">
                                     
                                      <DayPilot:DayPilotScheduler ID="DayPilot_TimeTable" runat="server" 
                                        HeaderFontSize="8pt" HeaderHeight="30"  
                                        DataStartField="start" 
                                        DataEndField="end" 
                                        DataTextField="name" 
                                        DataValueField="id" 
                                        DataResourceField="resource" 
                                        DataBarColorField="barColor"
                                        EventHeight="50" 
                                        EventFontSize="11px" 
                                        CellDuration="1440" 
                                        CellWidth="100"
                                        HoverColor="AliceBlue"
                                        BorderColor="Black"
                                        BorderWidth="2" FreeTimeClickJavaScript="alert('Period Not Configured')"
                                        Days="8" Font-Bold="True" Font-Size="Large"  DurationBarVisible="False"
                                        EventClickHandling="PostBack" EventClickJavaScript="TimePeriodClick" BorderStyle="None" 
                                        oneventclick="TimeTPeriodClick" HourBorderColor="Black" EventBorderColor="Gray" EventBackColor="Black" >
                                        
                                      </DayPilot:DayPilotScheduler>
                                     </div>
        </td>
                                
    </tr>
 </table>
 
 <asp:HiddenField ID="Hdn_ClassPeriodId" runat="server" />
 <asp:HiddenField ID="Hdn_Staff" runat="server" />
 <asp:HiddenField ID="Hdn_Subject" runat="server" />
 <asp:HiddenField ID="Hdn_DayId" runat="server" />
 <asp:HiddenField ID="Hdn_Period" runat="server" />
 
  <asp:Button runat="server" ID="Button3" style="display:none"/>
                            <ajaxToolkit:ModalPopupExtender ID="MPE_SchedulePeriod"   runat="server" CancelControlID="Btn_Cancel" PopupControlID="Panel3" TargetControlID="Button3"  BackgroundCssClass="modalBackground"  />
                                <asp:Panel ID="Panel3" runat="server" style="display:none;">
                                    <div class="container skin1" style="width:400px; top:400px;left:200px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/configure1.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:Black">Schedule Period</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >      
                                                      
                                                      <table width="100%" cellspacing="10">
                                                        <tr>
                                                            <td>
                                                              Subject :
                                                            </td>
                                                            <td> 
                                                                 <asp:DropDownList ID="Drp_Subject" runat="server" Width="140px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Subject_SelectedIndexChanged">
                                                                 </asp:DropDownList>
                                                            </td>
                                                         </tr>
                                                         <tr>
                                                            <td>
                                                             Staff :
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="Drp_Staff" runat="server" Width="140px" class="form-control">
                                                                 </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                         <td colspan="2" align="center">
                                                         
                                                             <asp:Label ID="lbl_SchError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                         
                                                         </td>
                                                        </tr>
                                                      </table>
                                                     
                                                     
                                                     <br /><br />
                                                      
                                                      <div style="text-align:center;">    
                                                            <asp:Button ID="Btn_SchedulePeriod" runat="server" Text="Save" Width="90px" class="btn btn-info"
                                                                onclick="Btn_SchedulePeriod_Click"  />                   
                                                           <%--  <asp:Button ID="Btn_FreeStaff" runat="server" Text="FreeStaffs" Width="50px" OnClick="Btn_FreeStaff_Save"/>  --%>
                                                            <asp:Button ID="Btn_Cancel" runat="server" class="btn btn-danger" Text="Close" Width="90px"/>
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
                             
                             
                             
    <asp:Button runat="server" ID="Button1" style="display:none"/>
       <ajaxToolkit:ModalPopupExtender ID="MPE_FreePeriod"   runat="server" CancelControlID="Button2" PopupControlID="Panel1" TargetControlID="Button1" BackgroundCssClass="modalBackground"  />
       <asp:Panel ID="Panel1" runat="server" style="display:none;">
        <div class="container skin1" style="width:400px; top:400px;left:200px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/configure1.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:Black">Free Period!</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >      

                                                     <asp:Label ID="Lbl_TimePop" runat="server"  ForeColor="Red"></asp:Label>
                                                     
                                                     <table width="100%" cellspacing="10">
                                                        <tr>
                                                            <td align="right">
                                                              Subject :
                                                            </td>
                                                            <td> 
                                                                <asp:Label ID="Lbl_SelectedSubject" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                            </td>
                                                         </tr>
                                                         <tr>
                                                            <td  align="right">
                                                             Staff :
                                                            </td>
                                                            <td>
                                                                 <asp:Label ID="Lbl_SelectedStaff" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                      </table>
                                                      <br />
                                                       <asp:Label ID="Label3" runat="server" Text="Make selected period free ?" Font-Bold="true" ForeColor="Black"></asp:Label>
                                                     
                                                     
                                                      <br />
                                                     <br />
                                                      
                                                      <div style="text-align:center;">    
                                                            <asp:Button ID="Btn_CancelPeriod" runat="server" Text="Ok" Width="90px" class="btn btn-info" OnClick="Btn_CancelPeriod_Save"/>                   
                                                           <%--  <asp:Button ID="Btn_FreeStaff" runat="server" Text="FreeStaffs" Width="50px" OnClick="Btn_FreeStaff_Save"/>  --%>
                                                            <asp:Button ID="Button2" runat="server" Text="Close" class="btn btn-danger" Width="90px"/>
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