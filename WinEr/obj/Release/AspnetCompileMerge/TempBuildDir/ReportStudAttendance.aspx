<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ReportStudAttendance.aspx.cs" Inherits="WinEr.ReportStudAttendance"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        
        <div id="right">

<div id="sidebar2">
<h2>Student Manager</h2>
<div id="StudentMenu" runat="server">

</div>
<div id="ActionInfo" runat="server">
 
</div>
</div>
</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
       
        
        <div id="left" style="min-height:450px;">
      <asp:Panel ID="dailyGrid" runat="server"> 
        <div class="container skin1" style="max-width:800px">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n"> Student Attendance Register</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                   <br />
                   <div style="text-align:left">
                    <table width="100%">
                     <tr>
                          <td style="width:100px">Select the Mode:</td> 
                           <td style="width:170px"> <asp:DropDownList ID="Drp_selectdays" runat="server" Width="163px" AutoPostBack="True" 
                                 OnSelectedIndexChanged="selectDifferMode">
                             </asp:DropDownList>
                           </td>
                      </tr>
                      <tr>
                         <td style="width:100px">Select the Class:</td>
                         <td style="width:170px"> <asp:DropDownList ID="Drp_SelectTheClass" runat="server" Width="163px" AutoPostBack="True"
                              OnSelectedIndexChanged="selectDifferclass">
                               </asp:DropDownList  >  
                               </td>
                               <td style="width:70px">
                                <asp:Button ID="Btn_UpdateAttandance" runat="server" Text="  Save " 
                                 onclick="Btn_UpdateAttandance_Click" />
                                 </td>
                                 <td>
                                <asp:Button ID="Btn_Edit" runat="server" Text="  Update " 
                                 onclick="Btn_Edit_Click" style="height: 26px" />
                            </td>
                         </tr>
                         </table>
                         </div>
                         <asp:Panel ID="Pnl_Daily" runat="server">
                         <div id="daymode" runat="server">         
                          <asp:Calendar ID="Cal_DateEntry" runat="server" BackColor="White" 
                              BorderColor="Black" 
                              Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="150px" 
                              Width="100%" BorderStyle="Solid" CellSpacing="1" 
                              NextPrevFormat="ShortMonth" onselectionchanged="Calendar1_SelectionChanged" >
                              <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                              <TodayDayStyle BackColor="#999999" ForeColor="White" />
                              <OtherMonthDayStyle ForeColor="#999999" />
                              <DayStyle BackColor="#CCCCCC" />
                              <NextPrevStyle Font-Size="8pt" ForeColor="White" Font-Bold="True" />
                              <DayHeaderStyle Font-Bold="True" Height="8pt" Font-Size="8pt" 
                                  ForeColor="#333333" />
                              <TitleStyle BackColor="#333399" Font-Bold="True" Font-Size="12pt" 
                                  ForeColor="White" BorderStyle="Solid" Height="12pt" />
                          </asp:Calendar>
                          
                       <br />
                     <div  style="overflow:scroll; height: 390px;">   
                    
                    <asp:GridView ID="Grd_StaffAttandence" runat="server" 
                        AutoGenerateColumns="False" 
                        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">                         
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="#F7F7DE" />
                        <Columns>
                            
                            <asp:BoundField DataField="Id" HeaderText="Id"  />
                            <asp:BoundField DataField="StudentName"  HeaderText="Name"  />
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_daily" runat="server" Checked="true"/>
                                </ItemTemplate>
                         <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                            
                        </Columns>
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                         <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                     </div>
                   </div>
                    </asp:Panel>   
                      
                      <asp:Panel ID="Pnl_Weekly"  runat="server">  
                        <table width="100%">
                        <tr  id="weekinput" runat="server">
                             <td style="width:100px" >Select Date:&nbsp;</td><td style="width:120px"><asp:TextBox ID="Txt_statrDate" runat="server" Width="160px" 
                               
                              AutoPostBack="True"  
                              ontextchanged="Txt_statrDate_TextChanged" ></asp:TextBox></td><td>
                                  <asp:Label ID="Lbl_from" runat="server" Text="From:  "></asp:Label><asp:Label ID="Lbl_startDate" runat="server" Text="" ForeColor="Red"></asp:Label><asp:Label ID="Lbl_to" runat="server" Text="  To:   "></asp:Label><asp:Label ID="Lbl_EndDate" runat="server" Text="" ForeColor="Red"></asp:Label></td></tr></table>
                         <ajaxToolkit:CalendarExtender ID="Txt_statrDate_CalendarExtender" 
                           runat="server" Enabled="True" TargetControlID="Txt_statrDate" CssClass="cal_Theme1">
                       </ajaxToolkit:CalendarExtender>
                       
                                      
                    <div id="weekmode" runat="server" style=" overflow:scroll; height: 390px;">           
                    <asp:GridView ID="Grd_SelectedWeek" runat="server" 
                        AutoGenerateColumns="False" 
                        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical"> 
                        
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="#F7F7DE" />
                        <Columns>
                            
                            <asp:BoundField DataField="Id" HeaderText="Id"  />
                            <asp:BoundField DataField="StudentName"  HeaderText="Name"  />
                            
                            <asp:TemplateField HeaderText="Mon">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_mon" runat="server" Checked="true" />
                                </ItemTemplate>
                         <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Tue">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_tu" runat="server" Checked="true" />
                                </ItemTemplate>
                         
                             <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Wed">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_wed" runat="server" Checked="true" />
                                </ItemTemplate>
                         
                             <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Thu">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_thu" runat="server" Checked="true"/>
                                </ItemTemplate>
                         
                             <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Fri">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_fri" runat="server" Checked="true"/>
                                </ItemTemplate>
                         
                             <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Sat">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_status_sat" runat="server" Checked="true"/>
                                </ItemTemplate>
                         
                             <ControlStyle ForeColor="#FF3300" />
                         </asp:TemplateField>
                
                        </Columns>
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                         <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                     
                   </div>
                      </asp:Panel>
                </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> 
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </asp:Panel>
 
           
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
