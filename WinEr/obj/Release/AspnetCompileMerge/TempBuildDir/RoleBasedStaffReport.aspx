<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="RoleBasedStaffReport.aspx.cs" Inherits="WinEr.RoleBasedStaffReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no">
                    <img alt="" src="Pics/Staff/network_zoom.png" width="30" height="30" /> </td>
                <td class="n">Role Based Staff Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table width="100%">
                   <tr>
                            <%--<td align="right" style="width:350px">Select Role </td>
                            <td style="width:280px"> 
                                  
                                </td>--%>
                                <td class="leftside">Select Role</td>
                     <td class="rightside"><asp:DropDownList ID="Drp_Role" runat="server"  Width="152px" class="form-control"></asp:DropDownList></td>
                                </tr>
                                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                <tr> 
                                <td class="leftside"><br /></td>
                     <td class="rightside"><asp:Button ID="Btn_Generate" runat="server" Text="Show Report" Class="btn btn-primary"
                                     onclick="Btn_Generate_Click"  /></td> 
                               
                                <%--    <td align="left">
                                    
                            </td>--%></tr>
                           
                        <tr>
                         <td  colspan="3" align="center">
                             <asp:Label ID="Lbl_Message" runat="server"   ForeColor="Red"></asp:Label>
                         </td>
                        </tr>
                        <tr>
                         <td  colspan="3" align="center">
                            &nbsp;
                         </td>
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_StaffDetails" runat="server" Visible="false">
                 <div class="linestyle">                  
                    </div>

                                    <div style="text-align:right"> <asp:ImageButton ID="Img_Export" runat="server" Width="35px" Height="35px"  ToolTip="Export to Excel"
                                    ImageUrl="~/Pics/Excel.png" onclick="Img_Export_Click"/></div>
                     <div style="height:auto;  overflow:auto">
                        <asp:GridView ID="Grd_StaffDetails" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="30" onpageindexchanging="Grd_StaffDetails_PageIndexChanging" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                             
                        <Columns>
                        <asp:BoundField DataField="StaffName"  HeaderText="Staff Name" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="Gender"  HeaderText="Gender" ItemStyle-Width="50px" />                        
                        <asp:BoundField DataField="Address"  HeaderText="Address" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="RoleName"  HeaderText="Role Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="UserName"  HeaderText="UserName" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="DOB"  HeaderText="DOB" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="JoiningDate"  HeaderText="Joining Date" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-Width="80px"/>
                        </Columns>
                        
                       <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
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
    
        
    <div class="clear"></div>
    </div>
    
</asp:Content>
