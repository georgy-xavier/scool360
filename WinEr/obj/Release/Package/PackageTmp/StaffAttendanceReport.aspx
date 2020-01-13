<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StaffAttendanceReport.aspx.cs" Inherits="WinEr.StaffAttendanceReport" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.leftLabel
        {
            font-weight:normal;
            text-align:right;
        }
        .leftFields
        {
            font-weight:bolder;
            color:Black;
            width: 140px;
        }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
   <div id="right">

<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>

</div>
     <div id="left">
      <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
     <br />
   
         <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Attendance Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                   
                <asp:Panel ID="Pnl_attendanceReportarea" runat="server">
                           <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"
                        CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True">
               
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="YEARLY" Visible="true" >
                <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart.png" /><b>YEARLY</b></HeaderTemplate>         
                 <ContentTemplate>                    
                               
                   <div style="min-height:200px">
                         
                         <div id="divGrid" runat="server">
                          <table width="100%">
                           <tr>
                            <td class="SubHeaderStyle">
                             
                            </td>
                            <td  class="SubHeaderStyle">
                            
                             Working Days
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Full Day
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Half Day
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Absent
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Approved Leave
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Other Leave
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             Percentage
                            
                            </td>
                           </tr> 
                           <tr>
                            <td class="TableHeaderStyle">
                                January
                            </td>
                            <td  class="CellStyle">
                            
                             22
                            
                            </td>
                             <td  class="CellStyle">
                            
                             15
                            
                            </td>
                             <td  class="CellStyle">
                            
                             3
                            
                            </td>
                             <td  class="CellStyle">
                            
                             2
                            
                            </td>
                             <td  class="CellStyle">
                            
                             0
                            
                            </td>
                             <td  class="CellStyle">
                            
                             0
                            
                            </td>
                             <td  class="CellStyle">
                            
                             90
                            
                            </td>
                           </tr>
                           <tr>
                            <td class="TableHeaderStyle">
                                February
                            </td>
                            <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="CellStyle">
                            
                             afsddf
                            
                            </td>
                           </tr>
                           <tr>
                            <td class="CellStyle">
                                Total
                            </td>
                            <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                             <td  class="SubHeaderStyle">
                            
                             afsddf
                            
                            </td>
                           </tr>
                          </table>
                         </div> 
                         
                   </div>      
                               
                               
                                
                 </ContentTemplate>  
                 </ajaxToolkit:TabPanel>
                
                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="DETAILS">
                <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/calendar.png" /><b>DETAILS</b></HeaderTemplate>                  
                    <ContentTemplate>
                        
                     <div style="min-height:200px">
                         
                          <table width="100%">
                           <tr>
                            <td style="width:50%;" align="right">
                               Select Month
                            </td>
                            <td>
                                <asp:DropDownList ID="Drp_Month" runat="server" Width="140px" AutoPostBack="true"
                                    onselectedindexchanged="Drp_Month_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>
                           </tr>
                          </table>
                         
                         <br />
                         
                         <div style="height:250px;overflow:auto">
                         
                         <div id="DivDetails" runat="server">
                           
                           <table width="100%" cellspacing="0">
                            <tr>
                             <td class="SubHeaderStyle">
                               Date
                             </td>
                             <td class="SubHeaderStyle">
                               Attendance Status
                             </td>
                             <td class="SubHeaderStyle">
                             
                              In Time
                             
                             </td>
                             <td class="SubHeaderStyle">
                             
                              out Time
                              
                             </td>
                            </tr>
                            <tr>
                             <td class="HeaderStyle">
                               01/03/2011
                             </td>
                             <td class="CellStyle">
                               FullDay
                             </td>
                             <td class="CellStyle">
                             
                              09:02:10
                             
                             </td>
                             <td class="CellStyle">
                             
                             17:00:00
                              
                             </td>
                            </tr>
                            <tr>
                             <td class="HeaderStyle">
                               02/03/2011
                             </td>
                             <td class="CellStyle">
                               Absent
                             </td>
                             <td class="CellStyle">
                             
                              _:_:_
                             
                             </td>
                             <td class="CellStyle">
                             
                             _:_:_
                              
                             </td>
                            </tr>
                           </table>
                           
                         </div>
                        </div>
                         
                     </div>             

                    </ContentTemplate>     
               </ajaxToolkit:TabPanel>
                
             </ajaxToolkit:tabcontainer>
                           
                           
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
   
    <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                                <table   cellpadding="0" cellspacing="0" class="containerTable">
                                    <tr >
                                        <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                                                    Height="28px" Width="29px" /> </td>
                                        <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
                                        <td class="o"> </td>
                                        <td class="c" >
                                           
                                            <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
                                                    <div style="text-align:center;">
                                                        
                                                        <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
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
     
    </div>
    <div class="clear"></div>
    </div>
</asp:Content>
