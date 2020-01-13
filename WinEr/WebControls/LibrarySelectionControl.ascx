<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LibrarySelectionControl.ascx.cs" Inherits="WinEr.WebControls.BookCopiesControl" %>
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<asp:Panel ID="Pnl_BookDetails" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_BookDetails"  runat="server" CancelControlID="Img_Close"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_book" TargetControlID="Btn_hdnmessagetgt"   />
                          <asp:Panel ID="Pnl_book" runat="server" style="display:none;" >
                         <div class="container skin1" style="width:600px; height:450px;overflow:auto;top:400px;left:400px">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">
                  </td>
            <td class="n" align="right"><asp:ImageButton ID="Img_Close" runat="server"  ImageUrl="../images/cross.png"  Height="21px" Width="21px" AlternateText="Close"/>
               </td>
            <td class="ne">
             
           
            </td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               <asp:Label ID="Lbl_SearchType" runat="server" Text="" Visible="false"></asp:Label>
               
               <asp:Panel ID="Pnl_BookCopies" runat="server">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Name" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Name" runat="server" Text="" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="Lbl_Select" runat="server" Text=""></asp:Label></td>
                             <td></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                            <asp:GridView ID="Grd_Books" runat="server" ForeColor="Black" 
                                GridLines="Vertical"  AutoGenerateColumns="False"  AllowPaging="true" PageSize="20"
                                 onselectedindexchanged="Grd_Books_SelectedIndexChanged"
                         Width="100%" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                   CellPadding="3" CellSpacing="2" Font-Size="12px" onpageindexchanging="Grd_Books_PageIndexChanging"
                                 > 
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id"  ItemStyle-Width="50px"/>  
                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="275px" />
                                <asp:BoundField DataField="Val2" HeaderText=""  ItemStyle-Width="200px"/>
                                 <asp:BoundField DataField="Val3" HeaderText=""  ItemStyle-Width="75px"/>
                                 <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnEdit"  runat="server" CommandName="Select"  ImageUrl="~/Pics/accept_page.png" Width="30px" Height="30px"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                             </Columns> 
                              <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" /> 
                            <RowStyle BackColor="#F7F7DE" />
                            <FooterStyle BackColor="#CCCC99" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                          
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        
        
                            </td>
                             
                        </tr>
                        
                    </table>
               </asp:Panel>
               <asp:Panel ID="Pnl_ShowError" runat="server" >
               <div align="center">
                    <asp:Label ID="Lbl_Error" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
               </asp:Panel>
               
              
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>                  
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>