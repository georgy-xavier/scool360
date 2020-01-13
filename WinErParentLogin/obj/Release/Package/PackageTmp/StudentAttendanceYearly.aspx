<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="StudentAttendanceYearly.aspx.cs" Inherits="WinErParentLogin.StudentAttendanceYearly" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
     <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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

         <table width="100%">
          <tr>
           <td align="right">
               <asp:ImageButton ID="ImageButton_Back" runat="server" ToolTip="BACK"  Width="45px" Height="45px"  PostBackUrl="~/AttendanceReport.aspx" ImageUrl="~/images/back.png" />
             <asp:ImageButton ID="Img_excel" runat="server" Height="45px" 
                                 ImageUrl="~/Pics/Excel.png" OnClick="Img_excel_Click" ToolTip="Export to Excel" 
                                 Width="45px" />
           </td>
          </tr>
          <tr>
             <td>
                       <br />
                  
             <asp:GridView ID="Grd_Students" runat="server" CellPadding="4" ForeColor="Black"  GridLines="Both" AutoGenerateColumns="False" EnableTheming="false"
                      BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" Width="100%"   >
                     <Columns>
                        <asp:BoundField DataField="Batch" HeaderText="Batch/Class" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="WorkingDays" HeaderText="WorkingDays" />
                        <asp:BoundField DataField="PresentDays" HeaderText="PresentDays"  />
                        <asp:BoundField DataField="HalfDays" HeaderText="HalfDays"  />
                        <asp:BoundField DataField="AbsentDays" HeaderText="AbsentDays" />
                        <asp:BoundField DataField="Holidays" HeaderText="Holidays" />
                        <asp:BoundField DataField="Percentage" HeaderText="Percent(%)"/>
                        <asp:TemplateField HeaderText="Details"  ItemStyle-Width="300px" >
                         <ItemTemplate>
                         
                          <table width="100%">
                           <tr>
                            <td align="right" style="width:50%;color:Gray">
                             No of Working Days : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;width:50%;" >
                              <%# Eval("WorkingDays")%>
                            </td>
                           </tr>
                            <tr>
                            <td align="right" style="color:Gray">
                             No of Present Days : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;">
                              <%# Eval("PresentDays")%>
                            </td>
                           </tr>
                           <tr>
                            <td align="right" style="color:Gray">
                             No of Half Days : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;">
                              <%# Eval("HalfDays")%>
                            </td>
                           </tr>
                           <tr>
                            <td align="right" style="color:Gray">
                             No of Absent Days : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;">
                              <%# Eval("AbsentDays")%>
                            </td>
                           </tr>
                           <tr>
                            <td align="right" style="color:Gray">
                             No of Holidays : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;">
                              <%# Eval("Holidays")%>
                            </td>
                           </tr>
                           <tr>
                            <td align="right" style="color:Gray">
                             Percentage(%) : 
                            </td>
                            <td align="left" style="color:Black;font-weight:bold;">
                              <%# Eval("Percentage")%>
                            </td>
                           </tr>
                          </table>
                         
                         </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status"  ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="White" >
                        <HeaderTemplate >
                            <asp:LinkButton ID="Link_ChartDetails" runat="server" ForeColor="Red" BackColor="White" Width="100%"
                              onclick="Link_ChartDetails_Click">Show Details</asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                    
                            <Web:ChartControl id="chartcontrol" runat="server" Width="195px"  
                                 Height="120px" HasChartLegend="False"
                                 GridLines="None" BorderStyle="None" TopPadding="0" ChartPadding="0" 
                                 YCustomEnd="0" YCustomStart="0" YValuesInterval="0" 
                                 Padding="0" BorderWidth="0px">
                                 <Border DashStyle="Solid"  />
                                 <Background Color="LightSteelBlue" />
                                 <PlotBackground ForeColor="White" />
                                 <ChartTitle StringFormat="Center,Near,Character,LineLimit" />
                                 <Legend Font="Tahoma, 8.25pt, style=Bold" Position="Bottom" ></Legend>
                                 <Charts><Web:PieChart Explosion="8" Legend="Some Legend" Name="Chart">
                                 <DataLabels Font="Tahoma, 8pt, style=Bold" 
                                         ForeColor="White" Separator=": " ShowXTitle="True" Visible="True">
                                 <Border Color="Blue" />
                                 <Background Color="DimGray" ForeColor="White" />
                                 </DataLabels>
                                 <Shadow Color="Gray" Visible="True" />
                                 </Web:PieChart>
                                 </Charts>
                                 <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
                                 <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                                 <XTitle StringFormat="Center,Near,Character,LineLimit" />
                                 <YTitle StringFormat="Center,Near,Character,LineLimit" />
                                 </Web:ChartControl>
                                   
                           </ItemTemplate>
                          </asp:TemplateField>
                         </Columns>
                                 
                        <RowStyle BackColor="White" VerticalAlign="Top" />
                        <FooterStyle BackColor="#CCCC99"/>
                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                        <AlternatingRowStyle BackColor="White" />
                  </asp:GridView>
            <asp:Label ID="lbl_gridmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                  
            </td>
          </tr>
          <tr>
             <td align="center">
           <br />

            <Web:ChartControl id="chartcontrol_persent" BorderStyle="Outset" BorderWidth="5px" ChartPadding="30" YCustomEnd="110" HasChartLegend="False"
                runat="server" Padding="5" TopPadding="20" Background-Color="#fdeca8"
                Width="680px" YCustomStart="0" YValuesInterval="0" Height=""><Background Color="LightSteelBlue" />
                <ChartTitle StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 12pt, style=Bold"   ForeColor="Black" Text="Attendance Percentage" />
                <XAxisFont StringFormat="Center,Near,Character,LineLimit"  Font="Tahoma, 8pt, style=Bold" />
                <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                <XTitle StringFormat="Center,Near,Character,LineLimit" />
                <YTitle StringFormat="Center,Near,Character,LineLimit" />
            </Web:ChartControl>
            </td>
          </tr>
         </table>          

              
           
    
            
        
     <WC:MSGBOX ID="MSGBOX" runat="server" />
 
 <asp:Panel ID="Panel1" runat="server">
                       
 <asp:Button runat="server" ID="Button1" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="ShowChartDetais"  runat="server" CancelControlID="ButtonOk"  PopupControlID="Panel2" TargetControlID="Button1"  BackgroundCssClass="modalBackground" />
  <asp:Panel ID="Panel2" runat="server" style="display:none;">  <%--style="display:none;"--%>
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/chart.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Chart Details</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                   <table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px" width="100%">
                       <tr>
                        <td style="width:15px;height:15px;background-color:Green;border:solid 1px Black;">
                             &nbsp;
                         </td>
                         <td align="left">
                           Present Days
                         </td>
                         <td></td>
                         <td style="width:15px;height:15px;background-color:Red;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Absent Days
                         </td>
                         </tr>
                        <tr>
                         <td style="width:15px;height:15px;background-color:Silver;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Half Days
                         </td>
                         <td></td>
                         <td style="width:15px;height:15px;background-color:Yellow;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Holidays
                         </td>
                       </tr>
                     </table>
               
                        <div style="text-align:center;">
                            
                            <asp:Button ID="ButtonOk" runat="server" Text="OK" Width="100px"/>
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
  
  </ContentTemplate>
   <Triggers>
    <asp:PostBackTrigger ControlID="Img_excel" />
   </Triggers>
  </asp:UpdatePanel> 
    </div>
</asp:Content>
