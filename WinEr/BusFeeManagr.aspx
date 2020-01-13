<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="BusFeeManagr.aspx.cs" Inherits="WinEr.BusFeeManagr" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">   
    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_BusFeeMangr.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }

    }
</script>
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
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="30px" Width="30px" />  </td>
                <td class="n">Bus Fee Management</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <br />
                <div style="min-height:200px">
                <table width="100%">
                <tr>
                <td align="center">
                <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label>
                </td>
                </tr>
                </table>
                <asp:Panel ID="Pnl_ListOfDestination" runat="server">
                <table width="100%">
                <tr>
                <td>
                <center>
                 <asp:GridView ID="Grd_BusFeeMangr" runat="server" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="#EBEBEB" 
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                        CellSpacing="2" Font-Size="15px" Width="100%"  PageSize="20"
                        onpageindexchanging="Grd_BusFeeMangr_PageIndexChanging">
                        <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                        <EditRowStyle Font-Size="Medium" />
                        <Columns>
                         <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                         <ItemTemplate>
                        <asp:CheckBox ID="ChkFee" runat="server" />
                         </ItemTemplate>
                       <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id"/>
                        <asp:BoundField DataField="Destination" HeaderText="Destination"/>
                        <asp:BoundField DataField="Distance" HeaderText="Distance"/>
                        <asp:TemplateField HeaderText="Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">                                              
                          <ItemTemplate>                                       
                            <asp:TextBox ID="Txt_Cost" class="form-control" runat="server"
                            MaxLength="8" Width="75px"></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" 
                                     runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom" 
                                     ValidChars="0123456789" TargetControlID="Txt_Cost">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                         </ItemTemplate>
                         </asp:TemplateField>                               
                        </Columns>
                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                 </asp:GridView>
                 </center>
                </td>
                </tr>
                <tr>
                <td align="center">
                <asp:Button ID="Btn_Save" runat="server" Text="Save" Class="btn btn-success" 
                        onclick="Btn_Save_Click" />
                <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" 
                        onclick="Btn_Cancel_Click" />
                </td>
                </tr>
                </table>
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
  </ContentTemplate>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
