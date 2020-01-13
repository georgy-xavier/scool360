<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="UserWiseBookIssueReport.aspx.cs" Inherits="WinEr.UserWiseBookIssueReport" %>
<%@ Register TagPrefix="WC" TagName="BOOKCOPIES" Src="~/WebControls/LibrarySelectionControl.ascx"%>
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
                    <div class="container skin1" style="width:580px" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr ><td class="no"></td><td class="n">Search Types</td><td class="ne"></td></tr>
                            <tr ><td class="o"></td><td class="c">
                                    <table>
                                        <tr>
                                        <td>
                                         <table width="100%">
                                                <tr>
                                                
                                                    <td align="left"><div class="form-inline">
                                                    Search By:&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:DropDownList ID="Drp_Quick" runat="server" Width="160px" class="form-control" AutoPostBack="True" onselectedindexchanged="Drp_SearchBy_SelectedIndexChanged">
                                                            <asp:ListItem  Selected="True" Text="Student"  Value="1"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Staff"></asp:ListItem>  
                                                        </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                 
                                                </tr>
                                            </table>
                                          
                                            <div id="Div1" class="newsearchbox">
                                                <p>
                                                    <asp:TextBox ID="Txt_Search" runat="server" CssClass="search"></asp:TextBox>
                                                    
                                                     <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender" 
                                                    runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetUserName"  UseContextKey="true"
                                                    ServicePath="WinErWebService.asmx"  TargetControlID="Txt_Search" MinimumPrefixLength="1">
                                                  </ajaxToolkit:AutoCompleteExtender>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Search_TextBoxWatermarkExtender" runat="server"
                                                        Enabled="True" TargetControlID="Txt_Search" WatermarkText=" Enter Data">
                                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Search_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="Txt_Search" FilterMode="InvalidChars" InvalidChars="'\">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Search" runat="server" Text="Search" class="btn btn-primary"
                                                       OnClick="Btn_QuickSearch_Click" /></p>
                                            </div>
                                            
                                        </td>
                                        </tr>
                                       <tr>
                                       <td >
                                           
                                           <asp:RadioButtonList ID="Rdo_btnUserDetails" runat="server" 
                                               RepeatDirection="Horizontal" >
                                           <asp:ListItem Text="All Details" Value="0"></asp:ListItem>
                                           <asp:ListItem  Text="Issued Book Details" Value="1"></asp:ListItem>
                                           <asp:ListItem  Text="Returned Book Details" Value="2"></asp:ListItem>
                                           </asp:RadioButtonList>
                                           <br />
                                           <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red"></asp:Label><br />
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
                                    ForeColor="Black" GridLines="Vertical"
                                    Width="100%" AllowPaging="True">
                                    <Columns>
                                        

                                        <asp:BoundField DataField="BookNo" HeaderText="Book Code" />
                                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                        <asp:BoundField DataField="Author" HeaderText="Author" />
                                        <asp:BoundField DataField="Edition" HeaderText="Edition" />
                                        <asp:BoundField DataField="Category" HeaderText="Category" />
                                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" />
                                        <asp:BoundField DataField="ReturnDate" HeaderText="Returned Date " />
                                        <asp:BoundField DataField="Fine" HeaderText="Fine" />
                                        
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
                                <div align="right">
                                <table>
                                <tr>
                                <td><asp:Label ID="Lbl_Tottal" Text="Amount For Pay : " runat="server"></asp:Label></td>
                                <td><asp:Label ID="Lbl_TottalFine" Text="" runat="server" Font-Bold="true"></asp:Label></td>
                                </tr>
                                <tr>
                                <td><asp:Label ID="Lbl_Paid" Text="Amount Paid : " runat="server" ></asp:Label></td>
                                <td><asp:Label ID="Lbl_PaidFine" Text="" runat="server" Font-Bold="true"></asp:Label></td>
                                </tr>
                               
                                
                                </table>

                              
                               </div>
                                </td><td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                        </table>
                    </div> 
                </asp:Panel>     
                <WC:BOOKCOPIES id="WC_bookcopies" runat="server" />    
                 <WC:MSGBOX id="WC_MessageBox" runat="server" />         
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="img_export_Excel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
