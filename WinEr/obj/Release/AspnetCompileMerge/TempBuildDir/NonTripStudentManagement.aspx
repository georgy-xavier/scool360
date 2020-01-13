<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="NonTripStudentManagement.aspx.cs" Inherits="WinEr.NonTripStudentManagement" %>
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
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">NON TRIP STUDENTS REPORT</td>
                <td class="ne"> </td>
            </tr>
             <tr >
                <td class="o"></td>
                <td class="c">
                <asp:Panel ID="Pnl_SearchArea" runat="server" Width="100%">
                <table class="tablelist">
                
                <tr>
                <td class="leftside"><br /></td>
                <td class="rightside"><br /></td>
                </tr>
                
                <tr>
                <td class="leftside">Select Class</td>
                <td class="rightside">
                     <asp:DropDownList ID="Drp_class" runat="server" AutoPostBack="true" class="form-control"
                                       Width="200px" onselectedindexchanged="drp_class_SelectedIndexChanged">
                     </asp:DropDownList>
                </td>
                </tr>
                
                <tr>
                <td class="leftside"><br /></td>
                <td class="rightside"><br /></td>
                </tr>
                <tr>
                <td class="leftside"></td>
                <td class="leftside">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/back.png" 
                        Width="60px" Height="40px" ToolTip="Go to back" onclick="ImageButton1_Click"/>
                </td>
                </tr>
                
                </table>
                </asp:Panel>
                
                <asp:Panel ID="pnl_grid" runat="server" Width="100%">
                <table class="tablelist">
                
                <tr>
                <td align="center" colspan="2">
                <hr />
                </td>
                </tr>
                
                <tr>
                <td class="rightside">
                </td>
                <td class="leftside">
                    <asp:ImageButton ID="img_export_Excel"  ToolTip="Export to Excel" 
                             ImageUrl="~/Pics/Excel.png" runat="server" 
                             Height="47px" Width="42px" OnClick="img_export_Excel_Click"/>
                </td>
                </tr>
                
                <tr>
                <td class="rightside">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total no of students : &nbsp
                <asp:Label ID="Lbl_total" runat="server" Text="0" ForeColor="Red"></asp:Label>
                </td>
                <td class="rightside"></td>
                </tr>
                
                <tr>
                <td colspan="2" align="center">
                <asp:GridView ID="grdResult" runat="server"  
                                                      GridLines="None" Width="100%"
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                     <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
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
   <asp:PostBackTrigger ControlID="img_export_Excel"/>
   </Triggers>
   
</asp:UpdatePanel>
</asp:Content>
