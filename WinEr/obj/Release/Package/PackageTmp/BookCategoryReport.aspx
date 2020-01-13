<%@ Page  Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="BookCategoryReport.aspx.cs" Inherits="WinEr.BookCategoryReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        
        
        .style1
        {
            width: 100%;
        }
        .searchmanagement
        {
            height:150px;
            overflow:scroll;
        }
       
        .BookDetails
        {
            min-height:250px;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate> 
                <div class="container skin1" >
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                    <tr >
                        <td class="no"> </td>
                        <td class="n">Category Wise Book Reprt</td>
                        <td class="ne"> </td>
                    </tr>
                    <tr >
                        <td class="o"> </td>
                        <td class="c" >
                        <asp:Panel ID="Pnl_SelectPart" runat="server">
                            <table width="100%">
                                <tr>
                                <td valign="top">
                                <div class="container skin1" style="width:500px" >
                                    <table cellpadding="0" cellspacing="0" class="containerTable">
                                    <tr >
                                        <td class="no"> </td>
                                        <td class="n">Select Category</td>
                                        <td class="ne"> </td>
                                    </tr>
                                    <tr >
                                        <td class="o"> </td>
                                        <td class="c"  style="height:80px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Select Category&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="Drp_Category" runat="server" Width="150px" class="form-control"> </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Btn_ShowReport" Text="Show" runat="server" Width="110px" Class="btn btn-primary" OnClick="Btn_ShowReport_Click" />
                                                    </td>
                                                </tr>
                                               
                                                
                                            </table>
                                            
                                            <br />
                                            <table >
                                                    <tr>
                                                        <td class="leftside" >
                                                            Selected Category&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbl_SelectedCategory" runat="server" Font-Bold="true" Text="0"  BackColor="White" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td class="leftside" >
                                                            Total Books&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbl_TotalBooks" runat="server" Font-Bold="true" Text="0"  BackColor="White" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    
                                                     <tr>
                                                        <td class="leftside"  style="width:50%">
                                                            Issued Books&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_IssuedBooks" runat="server" Font-Bold="true" Text="0" BackColor="White" ForeColor="Black"></asp:Label>
                                                        </td>
                                                         <td class="leftside"  style="width:50%">
                                                            Issued By Teacherd&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbl_TeachersIssued" runat="server" Font-Bold="true" BackColor="White" Text="0" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td class="leftside"  style="width:50%">
                                                            Issued By Students&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lbl_StudentIssuedBooks" runat="server" Font-Bold="true"  Text="0" BackColor="White" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                     <tr>    
                                                    <td colspan="2">
                                                        <asp:Label ID="Lbl_CategoryErr" Text="" runat="server" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                </table>
                                            
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
                                </td>
                              
                                </tr>
                            </table>
                         
                            
                        </asp:Panel>
                        <asp:Panel ID="Pnl_display" runat="server">
                            <asp:GridView  ID="GrdVew_Books" runat="server" 
                            AutoGenerateColumns="False"  AllowPaging="true" PageSize="20"
                            ForeColor="Black" GridLines="Vertical" 
                            BackColor="#EBEBEB"   Width="100%"
                            BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                            RowStyle-BorderColor="#BFBFBF"
                            RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px"
                            CellPadding="3" CellSpacing="2" Font-Size="12px" >
                                <Columns>              
                                    <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                                    <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                    <asp:BoundField DataField="Copies" HeaderText="No of Copies" />
                                    
                                </Columns>
                            <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                            <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                            <EditRowStyle Font-Size="Medium" />
                            <SelectedRowStyle BackColor="White" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                            <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                            HorizontalAlign="Left" />
                            <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  
                            Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
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
        </asp:UpdatePanel>
        <div class="clear"></div>
    </div>
</asp:Content>
