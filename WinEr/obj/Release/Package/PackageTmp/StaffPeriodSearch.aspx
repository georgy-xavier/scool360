<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StaffPeriodSearch.aspx.cs" Inherits="WinEr.StaffPeriodSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
   <div id="contents">
      <asp:panel ID="Panel2" runat="server">
      <div class="container skin1" style="min-height:400px">
         <table cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
               <td class="no"> </td>
               <td class="n">Search staffs</td>
               <td class="ne"> </td>
            </tr>
            <tr >
               <td class="o"> </td>
               <td class="c" >
                  <br />
                  <asp:Panel ID="TimeTable" runat="server">
                     <div style="text-align:right">
                        <asp:ImageButton ID="Img_Export" runat="server"  ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px" OnClick="Img_Export_Click"/>
                     </div>
                     <table class="tablelist">
                        <tr>
                           <td class="leftside">Class</td>
                           <td class="rightside">
                              <asp:DropDownList ID="Drp_Class" runat="server" Width="160px" TabIndex="0">
                              </asp:DropDownList>
                           </td>
                        </tr>
                        <tr>
                           <td class="leftside">Day</td>
                           <td class="rightside">
                              <asp:DropDownList ID="Drp_Day" runat="server" Width="160px" TabIndex="1">
                              </asp:DropDownList>
                           </td>
                        </tr>
                        <tr>
                           <td class="leftside">Period</td>
                           <td class="rightside">
                              <asp:DropDownList ID="Drp_Period" runat="server" Width="160px" TabIndex="2">
                              </asp:DropDownList>
                           </td>
                        </tr>
                        <tr>
                           <td class="leftside">Status</td>
                           <td class="rightside">
                              <asp:RadioButtonList ID="Rdo_Status" runat="server" RepeatDirection="Horizontal">
                                 <asp:ListItem Text="Engaged" Value="0" Selected="True"></asp:ListItem>
                                 <asp:ListItem Text="Free" Value="1"></asp:ListItem>
                              </asp:RadioButtonList>
                           </td>
                        </tr>
                        <tr>
                           <td class="leftside"></td>
                           <td>
                              <asp:Button ID="Btn_Find" runat="server" Text="Search" Width="111px" 
                                 CssClass="graysearch" onclick="Btn_Find_Click" />
                           </td>
                        </tr>
                     </table>
                </asp:Panel>
                  <br />
                  <asp:Panel ID="Pnl_Staff" runat="server" Visible="false">
                        <div class="roundbox">
                           <table width="100%" >
                              <tr>
                                 <td class="topleft"></td>
                                 <td class="topmiddle"></td>
                                 <td class="topright"></td>
                              </tr>
                              <tr>
                                 <td class="centerleft"></td>
                                 <td class="centermiddle">
                                    <div style="min-height:400px; overflow:auto">
                                       <asp:GridView  ID="Grd_Staff" runat="server" BackColor="White" AutoGenerateColumns="False" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"  CellPadding="4" 
                                          ForeColor="Black" GridLines="Vertical" Width="100% "  AllowSorting="true" onsorting="Grd_Staff_Sorting">
                                          <Columns>
                                             <asp:BoundField DataField="Staff" HeaderText="Staff"  SortExpression="Staff" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                             <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                             <asp:BoundField DataField="Day" HeaderText="Day" SortExpression="Day" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                             <asp:BoundField DataField="Period" HeaderText="Period" SortExpression="Period" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                          </Columns>
                                          <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                          <EditRowStyle Font-Size="Medium" />
                                          <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                          <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                                          <RowStyle BackColor="White"  BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                                       </asp:GridView>
                                    </div>
                                 </td>
                                 <td class="centerright"></td>
                              </tr>
                              <tr>
                                 <td class="bottomleft"></td>
                                 <td class="bottommiddile"></td>
                                 <td class=" bottomright"></td>
                              </tr>
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
      </asp:panel>
   </div>
</asp:Content>