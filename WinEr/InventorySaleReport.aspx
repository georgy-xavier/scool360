<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventorySaleReport.aspx.cs" Inherits="WinEr.InventorySaleReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Sales Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
               
               <asp:Panel ID="Pnl_InitialDetails" runat="server"  HorizontalAlign="Left">
                <center>
               <table width="800px" style="outline">
               <tr>
               <td align="right">Select Category</td>
               <td align="left"><asp:DropDownList ID="Drp_Category" runat="server" Width="153px" class="form-control" 
                       AutoPostBack="True" onselectedindexchanged="Drp_Category_SelectedIndexChanged"></asp:DropDownList></td>  
                       <td align="right">Select Item</td>
               <td align="left"><asp:DropDownList ID="Drp_Item" runat="server" Width="153px" class="form-control"></asp:DropDownList></td> 
               <td align="right">Select Period</td>
               <td align="left"><asp:DropDownList ID="Drp_Period" runat="server" Width="153px" class="form-control"
                       AutoPostBack="True" onselectedindexchanged="Drp_Period_SelectedIndexChanged">
                <asp:ListItem Text="This Month" Value="0">
                 </asp:ListItem>
                 <asp:ListItem Text="Last Week" Value="1">
                 </asp:ListItem>
                 <asp:ListItem Text="Manual" Value="2">
                 </asp:ListItem>
               </asp:DropDownList></td>                                  
               </tr> 
               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

               
               <tr id="RowManualperiod" runat="server">
                <td align="right">From</td>
                <td align="left">
                <asp:TextBox ID="Txt_FromDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Show" ControlToValidate="Txt_FromDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                 <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_FromDate" Format="dd/MM/yyyy">
               </cc1:CalendarExtender>  
              
                <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                   runat="server" ControlToValidate="Txt_FromDate" Display="None" 
                   ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                   <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                    TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                </td>  
                  <td align="right">To</td> 
               <td align="left">
               <asp:TextBox ID="Txt_ToDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                 <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ToDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Show" ControlToValidate="Txt_ToDate" ErrorMessage="*"></asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                         runat="server" ControlToValidate="Txt_ToDate" Display="None" 
                         ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                   <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                    TargetControlID="Txt_EndDateRegularExpressionValidator1"
                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
              </td>
              <td></td>
              <td ></td>
               </tr>
               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

               <tr>
               <td></td>
               <td ></td>
               <td></td>
               <td align="left"><asp:Button ID="Btn_Show" runat="server" Text="Show" class="btn btn-primary"  ValidationGroup="Show"
                       onclick="Btn_Show_Click" /></td>
               <td></td>
               </tr>
               <tr>
               <td></td>
               <td ></td>
               <td></td>
               <td align="left"><asp:Label ID="Lbl_SaleErr" runat="server" ForeColor="Red"></asp:Label></td>
                    <td></td>
               </tr>
               </table>
                </center>
               </asp:Panel> 
              
               
               
                <asp:Panel ID="Pnl_Show" runat="server">
                <div class="linestyle"></div>
                <table width="100%">
                <tr> 
                <td>
                Total Sales:<asp:Label ID="lblTamount" runat="server" Font-Bold="true" Font-Size="Medium">0</asp:Label>
                 </td>
                <td align="right">   
            <%--    onpageindexchanging="Grd_Items_PageIndexChanging"--%>
                <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px" onclick="img_export_Excel_Click"></asp:ImageButton></td>
                    </tr>
                <tr>
                <td>
                            <asp:GridView ID="Grd_InventorySaleReport" runat="server" 
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%" 
                                onpageindexchanging="Grd_InventorySaleReport_PageIndexChanging">
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="Id" HeaderText="Id"/>
                      <asp:BoundField DataField="ItemId" HeaderText="ItemId"/>
                      <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                      <asp:BoundField DataField="Category" HeaderText="Item Category"/>                      
                      <asp:BoundField DataField="TotalCost" HeaderText="Total Cost"/>
                      <asp:BoundField DataField="Quantity" HeaderText="Quantity"/>
                      <asp:BoundField DataField="ActionDate" HeaderText="Date"/>
                       <asp:BoundField DataField="Comment" HeaderText="Description"/>
                      
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
