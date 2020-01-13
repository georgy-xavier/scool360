<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryStockReport.aspx.cs" Inherits="WinEr.InventoryStockReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

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
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Item Stock Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <center>
                <asp:Panel ID="Pnl_StockReport" runat="server">
                <table width="100%" class="tablelist">
                 <tr>
                <td class="leftside">Location Name</td>
                <td class="rightside"><asp:DropDownList ID="Drp_LocationName" class="form-control" runat="server" 
                        Width="153px" AutoPostBack="True" 
                        onselectedindexchanged="Drp_LocationName_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                 <tr>
                <td class="leftside">Item Category</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Category" runat="server" class="form-control"
                        Width="153px" AutoPostBack="True" 
                        onselectedindexchanged="Drp_Category_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">Item Name</td>
                <td class="rightside"><asp:DropDownList ID="Drp_ItemName" runat="server" class="form-control"
                        Width="153px" AutoPostBack="True" 
                        onselectedindexchanged="Drp_ItemName_SelectedIndexChanged"></asp:DropDownList></td>
                </tr> 
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                  <tr><td class="leftside"></td><td class="rightside"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red">
                </asp:Label></td></tr>
                
                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show" 
                        class="btn btn-primary" onclick="Btn_Show_Click" /></td>
                
                </tr>
                
               
                </table>
                </asp:Panel>
                <asp:Panel ID="Pnl_Show" runat="server">
                <table width="100%">
                <tr><td align="right">   
                <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px" onclick="img_export_Excel_Click" ></asp:ImageButton></td></tr>
                <tr>
                <td>
                            <asp:GridView ID="Grd_Items" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%" onpageindexchanging="Grd_Items_PageIndexChanging">
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="Id" HeaderText="LocationId"/>
                      <asp:BoundField DataField="Id" HeaderText="ItemID"/>
                      <asp:BoundField DataField="LocationName" HeaderText="LocationName" />
                      <asp:BoundField DataField="ItemName" HeaderText="Item Name"/>                      
                      <asp:BoundField DataField="Category" HeaderText="Category"/>
                      <asp:BoundField DataField="TotalStock" HeaderText="Total Stock"/>
                      <asp:BoundField DataField="AvailableStock" HeaderText="Available Stock"/>
                      <asp:BoundField DataField="Price" HeaderText="Price"/>
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
                </td>
                </tr>
                </table>
                </asp:Panel>                
                </center>                
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
</div>
</asp:Content>
