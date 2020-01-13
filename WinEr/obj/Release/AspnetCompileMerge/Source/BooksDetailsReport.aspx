<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="BooksDetailsReport.aspx.cs" Inherits="WinEr.BooksDetailsReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>  
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>  
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!--
                Search  Critereas
                   Issued Books -All And Due Date
                   Category Wise
                   Text Wise- 
                   User Wise-
                -->
                <asp:Panel ID="Pn_Search" runat="server" DefaultButton="Btn_Search">                                
                    <div class="container skin1" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr ><td class="no"></td><td class="n">Search Types</td><td class="ne"></td></tr>
                            <tr ><td class="o"></td><td class="c">
                                    <table width="100%">
                                        <tr>
                                       
                                            <td style="width:15%; text-align:right;">Publisher Name:</td>
                                            <td style="width:25%; text-align:left;"> 
                                                <asp:TextBox ID="Txt_publisher" runat="server"  Width="150px" class="form-control"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="Txt_publisher_AutoCompleteExtender" 
                                                runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Publisher"  UseContextKey="true"
                                                ServicePath="WinErWebService.asmx"  TargetControlID="Txt_publisher" MinimumPrefixLength="1">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_publisher_TextBoxWatermarkExtender" runat="server"
                                                Enabled="True" TargetControlID="Txt_publisher" WatermarkText=" Enter Data">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_publisher_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="Txt_publisher" FilterMode="InvalidChars" InvalidChars="'\">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                               <td style="width:15%; text-align:right;">Author Name:</td>
                                            <td style="width:25%; text-align:left;">
                                                <asp:TextBox ID="Txt_author" runat="server"  Width="150px" class="form-control"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="Txt_author_AutoCompleteExtender1" 
                                                runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Autor"  UseContextKey="true"
                                                ServicePath="WinErWebService.asmx"  TargetControlID="Txt_author" MinimumPrefixLength="1">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_author_WatermarkExtender1" runat="server"
                                                Enabled="True" TargetControlID="Txt_author" WatermarkText=" Enter Data">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_author_FilteredTextBoxExtender1" runat="server"
                                                Enabled="True" TargetControlID="Txt_author" FilterMode="InvalidChars" InvalidChars="'\">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                          
                                            <td style="width:20%; text-align:center;"><asp:Button ID="Btn_Search" runat="server" Text="Search" class="btn btn-primary"  OnClick="Btn_Search_OnClick"/></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                            <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td><td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                        </table>
                    </div> 
                </asp:Panel>  
                
                
                <asp:Panel ID="Pnl_BookDetails" runat="server" >
                    <div class="container skin1" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr ><td class="no"></td><td class="n">Books Details</td><td class="ne"></td></tr>
                            <tr ><td class="o"></td><td class="c">
                             
                                    <div align="right">
                                        <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
                                        Width="42px" onclick="Btn_Export_Click"></asp:ImageButton>
                                    </div>
                                    <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="False"
                                     BorderColor="#BFBFBF" BorderStyle="Solid" Font-Size="12px" PageSize="20"
                                    BorderWidth="1px" CellPadding="5"
                                    OnPageIndexChanging="GrdBooks__PageIndexChanging" ForeColor="Black" GridLines="Vertical"
                                    Width="100%" AllowPaging="True">
                                    <Columns>
                                        

                                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                       
                                        <asp:BoundField DataField="Author" HeaderText="Author" />
                                        <asp:BoundField DataField="Publisher" HeaderText="Publisher" />
                                        <asp:BoundField DataField="Edition" HeaderText="Edition" />
                                        <asp:BoundField DataField="Category" HeaderText="Category" /> 
                                         <asp:BoundField DataField="Count" HeaderText="Count" />
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
                                <br />
                                </td><td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                        </table>
                    </div> 
                </asp:Panel>    
                  <WC:MSGBOX id="WC_MessageBox" runat="server" />                     
            </ContentTemplate>
             <Triggers >
       <asp:PostBackTrigger ControlID="img_export_Excel" />
       </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>
