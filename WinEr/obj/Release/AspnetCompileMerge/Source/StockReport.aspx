<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StockReport.aspx.cs" Inherits="WinEr.StockReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no">
                  </td>
                <td class="n">Stock Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c">
                
                <asp:Panel ID="Pnl_Search" runat="server">
                <table class='tablelist'>
                <tr>
                <td class="leftside">Category</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Category" runat="server" Width="250px"></asp:DropDownList></td>
                </tr>
                 <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show" Width="90px" OnClick="Btn_Show_Click" /></td>
                </tr>
                <tr>
                <td colspan="2"><asp:Label ID="Lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                </table>
                </asp:Panel>
                
<asp:Panel ID="Pnl_Dispaly" runat="server">
    <div align="right">
    <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
     onclick="Btn_Export_Click" Width="42px"></asp:ImageButton></div>
    <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="False"
    BorderColor="#BFBFBF" BorderStyle="Solid" Font-Size="12px" PageSize="20"
   OnPageIndexChanging="GrdBooks__PageIndexChanging"  BorderWidth="1px" CellPadding="5"
 ForeColor="Black" GridLines="Vertical"
    Width="100%" AllowPaging="True">
    <Columns>
    
        <asp:BoundField DataField="Id" HeaderText="Id" />
        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
        <asp:BoundField DataField="CatogoryName" HeaderText="Catogory Name" />
        <asp:BoundField DataField="Author" HeaderText="Author" />
        <asp:BoundField DataField="Edition" HeaderText="Edition" />
        <asp:BoundField DataField="Publisher" HeaderText="Publisher" />
        <asp:BoundField DataField="Cost" HeaderText="Price" />
        <asp:BoundField DataField="TotalStk" HeaderText="Total Stock" />
        <asp:BoundField DataField="AvailStk" HeaderText="Avail Stock" />

    </Columns>
    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
    <EditRowStyle Font-Size="Medium" />
    <SelectedRowStyle BackColor="White" ForeColor="Black" />
    <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="12px" ForeColor="Black"  HorizontalAlign="Left" />
    <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="12px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" Height="20px" />
    <AlternatingRowStyle BorderColor="#BFBFBF" />
    </asp:GridView>
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
<WC:MSGBOX id="WC_MessageBox" runat="server" />  
 </ContentTemplate>
   <Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  </Triggers>
  </asp:UpdatePanel>

<div class="clear"></div>
</asp:Content>
