<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="TDSReport.aspx.cs" Inherits="WinEr.TDSReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate> 
<div id="contents" >
<div class="container skin1">
  <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">TDS REPORT</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <table style="width:100%">
                <tr>
                <td align="right">
                <div class="form-inline">
                                    <asp:Label ID="Lbl_Year" runat="server" Text="Select Year" Font-Bold="true"></asp:Label>&nbsp;
                    <asp:DropDownList ID="Drp_Year" runat="server" Width="143px" class="form-control"
                        AutoPostBack="True" >
                    </asp:DropDownList>
                    </div>

                </td>
                <td class="leftside" style="width:250px" >
                
                <div class="form-inline">
                  <asp:Label ID="Lbl_Month" runat="server" Text="Select Month" Font-Bold="true"></asp:Label>&nbsp;
                   <asp:DropDownList ID="Drp_Month" runat="server" Width="143px" class="form-control"
                        AutoPostBack="True" >
                    </asp:DropDownList>
                     </div>
                   
                </td>
                <td class="rightside">
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Button ID="BtnShow" runat="server" Text="Show" class="btn btn-primary" onclick="BtnShow_Click" 
                         />&nbsp;
                    <asp:Button ID="BtnExcel" runat="server" Text="Export to Excel" 
                        class="btn btn-primary" onclick="BtnExcel_Click"
                         />
                </td>
               
                </tr>
                </table>
                <div class="linestyle"></div>  
                <div style="min-height:250px;">
                     <asp:Panel ID="Pnl_EmpPay" runat="server" Visible="false">
                     <div style="min-height:250px;" >
                     <asp:GridView DataKeyNames="EmpId" ID="Grd_EmpPay" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  
                   CellPadding="3" CellSpacing="2" Font-Size="12px"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        <asp:BoundField DataField="EmpId"  HeaderText="Id" ItemStyle-Width="20px" />
                        <asp:BoundField DataField="Surname"  HeaderText="Employee Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Designation"  HeaderText="Designation" ItemStyle-Width="100px" />
                         <asp:BoundField DataField="Total"  HeaderText="Total" ItemStyle-Width="100px" />
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
                  </div>   
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
    <WC:MSGBOX id="WC_MessageBox" runat="server" />                  
 </div>
 </div>
</ContentTemplate>
<Triggers >
    <asp:PostBackTrigger ControlID="BtnExcel"/>
</Triggers>
</asp:UpdatePanel>
</asp:Content>
