<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True"    CodeBehind="LlbraryHome.aspx.cs" Inherits="WinEr.LlbraryHome"  %>
<%@ Register TagPrefix="WC" TagName="BOOKCOPIES" Src="~/WebControls/LibrarySelectionControl.ascx"%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
          
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
         



   
          
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="Pnl_srch" runat="server" DefaultButton="Btn_Search">
                            
                            <div class="container skin1" style="width:100%;" >
                                <table cellpadding="0" cellspacing="0" class="containerTable">
                                    <tr>
                                        <td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/tag_blue.png" Height="28px" Width="29px" /></td>
                                        <td class="n"></td>
                                        <td class="ne"></td>
                                    </tr>
                                    <tr>
                                        <td class="o">   </td>
                                        <td class="c" style="height:108px">
                                        
                                        <table>
                                        <tr>
                                        <td>
                                         <table width="100%;" >
                                                <tr>
                                                    <td align="left">
                                             
                                                 Search By:
                                                 </td><td>
                                                 <asp:DropDownList ID="Drp_Quick" runat="server" AutoPostBack="True" class="form-control" 
                                                     onselectedindexchanged="Drp_SearchBy_SelectedIndexChanged" Width="160px">
                                                     <asp:ListItem Value="0">Book No</asp:ListItem>
                                                     <asp:ListItem Selected="True" Value="1">Book Name</asp:ListItem>
                                                     <asp:ListItem Value="2">Author</asp:ListItem>
                                                     <asp:ListItem Value="3">Publisher</asp:ListItem>
                                                     <asp:ListItem Value="4">Book Sl No</asp:ListItem>
                                                 </asp:DropDownList>
                                             
                                                    </td>
                                                   
                                                       
                                                            <td align="right">
                                                                <asp:LinkButton ID="Lnk_Detail" runat="server" onclick="Lnk_Detail_Click" 
                                                                    Text="Detailed Search"> </asp:LinkButton>
                                                            </td>
                                                    </tr>
                                           
                                                
                                            </table>
                                          
                                            <div id="Div1" class="newsearchbox">
                                                <p>
                                                    <asp:TextBox ID="Txt_Search" runat="server" CssClass="search" class="form-control"></asp:TextBox>
                                                    
                                                     <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender" 
                                                    runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBookData"  UseContextKey="true"
                                                    ServicePath="WinErWebService.asmx"  TargetControlID="Txt_Search" MinimumPrefixLength="1">
                                                  </ajaxToolkit:AutoCompleteExtender>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Search_TextBoxWatermarkExtender" runat="server"
                                                        Enabled="True" TargetControlID="Txt_Search" WatermarkText=" Enter Keyword">
                                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Search_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="Txt_Search" FilterMode="InvalidChars" InvalidChars="'\">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Search" runat="server" Text="Search" Class="btn btn-primary"
                                                       OnClick="Btn_QuickSearch_Click" /></p>
                                            </div>
                                            
                                        </td>
                                        </tr>
                                       
                                        
                                        </table>
                                            
                                        </td>
                                        <td class="e">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="so"> </td>
                                        <td class="s"></td>
                                        <td class="se"> </td>
                                    </tr>
                                </table>
                            </div>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <div class="container skin1">
                                <table cellpadding="0" cellspacing="0" class="containerTable"  style="width:55%;">
                                    <tr>
                                        <td class="no">
                                            <asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/search_page.png" />
                                        </td>
                                        <td class="n">
                                            Books Details
                                        </td>
                                        <td class="ne">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="o">
                                        </td>
                                        <td class="c">
                                            <table width="100%" >
                                                <tr>
                                                    <td align="right">
                                                        Total books&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                    <asp:Label ID="Lbl_Books" runat="server" ForeColor="Black" Font-Bold="true" class="control-label" ></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        No of issued books&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                     <asp:Label ID="Lbl_IssuedBooks" runat="server" ForeColor="Black" Font-Bold="true" class="control-label"></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        No of books issued for the staffs&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                    
                                                     <asp:Label ID="Lbl_BksStaff" runat="server" ForeColor="Black" Font-Bold="true" class="control-label"></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        No of books issued for the students&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                    <asp:Label ID="Lbl_BksStudents" runat="server" ForeColor="Black" Font-Bold="true" class="control-label"></asp:Label>
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        No of books booked&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                    <asp:Label ID="Lbl_BkdBooks" runat="server" ForeColor="Black" Font-Bold="true" class="control-label"></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        No of available books&nbsp; :&nbsp;
                                                    </td>
                                                    <td>
                                                    <asp:Label ID="Lbl_AvailableBooks" runat="server" ForeColor="Black" Font-Bold="true" class="control-label"></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="e">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="so">
                                        </td>
                                        <td class="s">
                                        </td>
                                        <td class="se">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                
             
                
                 <WC:BOOKCOPIES id="WC_bookcopies" runat="server" /> 
                <asp:Panel ID="Pnl_bookgrid" runat="server">
                                    
                                        <div class="container skin1" style="max-width: 999px">
                                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                                <tr>
                                                    <td class="no">
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Pics/book.png" Height="28px" Width="29px" />
                                                    </td>
                                                    <td class="n">
                                                        Book List
                                                    </td>
                                                    <td class="ne">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="o">
                                                    </td>
                                                    <td class="c">
                                                        <table width="100%">
                                                                <tr>
                                                                      <td  style="border-bottom:#4a4a4a thin solid;" >
                                                                         <table width="100%">
                                                                                   <tr>
                                                                         
                                                                         <td style="width:250px">Number of Books </td>
                                                                         <td>
                                                                         <asp:Label ID="Lbl_Srch_Total" runat="server" Text="" Font-Bold="true" class="control-label" ForeColor="Black"></asp:Label>
                                                                         </td>
                                                                         <td style="width:250px">Number of Issued Book</td>
                                                                         <td style="width:50px">
                                                                             <asp:Label ID="Lbl_Srch_IssuedBooks" runat="server" Text="" Font-Bold="true" class="control-label" ForeColor="Black"></asp:Label>
                                                                             </td>
                                                                          <td rowspan="2" align="right">
                                                                           <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
                                                                                Width="42px" onclick="Btn_Export_Click"></asp:ImageButton>
                                                                             </td>
                                                                         </tr>
                                                                         <tr>
                                                                         <td style="width:250px">No of Books issued for the staffs</td>
                                                                         <td>
                                                                             <asp:Label ID="Lbl_Srch_StaffIssued" runat="server" Text="" Font-Bold="true" ForeColor="Black" class="control-label"></asp:Label>
                                                                              </td>
                                                                         <td style="width:250px">No of Books issued for the students</td>
                                                                         <td> <asp:Label ID="Lbl_Srch_StudIssued" runat="server" Text="" Font-Bold="true" ForeColor="Black" class="control-label"></asp:Label>
                                                                         </td>
                                                                         </tr>
                                                                         </table>
                                                                             
                                                                      </td >
                                                                 </tr>
                                                                 <tr>
                                                                 <td><br /></td>
                                                                 </tr>
                                                        </table>
                                                        <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="False"
                                                            OnRowDataBound="GrdBooks_RowDataBound" BorderColor="#BFBFBF" BorderStyle="Solid" Font-Size="12px" PageSize="20"
                                                            BorderWidth="1px" CellPadding="5"  OnSelectedIndexChanged="GrdBooks_SelectedIndexChanged"
                                                            OnPageIndexChanging="GrdBooks__PageIndexChanging" ForeColor="Black" GridLines="Vertical"
                                                            Width="100%" AllowPaging="True">
                                                        
                                                                     
                                                                       
                                                            <Columns>
                                                               <asp:CommandField  SelectText="&lt;img src='Pics/bookgreen.png' width='30px' border=0 title='View Fee Details'&gt;"  
                                                                    ShowSelectButton="True" >
                                                                    <ItemStyle Width="35px" />
                                                               </asp:CommandField>
                                                                                            
                                                    
                                                                <asp:BoundField DataField="BookNo" HeaderText="Book No" />
                                                                <asp:BoundField DataField="BookName" HeaderText="BookName" />
                                                                <asp:BoundField DataField="Author" HeaderText="Author" />
                                                                <asp:BoundField DataField="Publisher" HeaderText="Publisher" />
                                                                <asp:BoundField DataField="Bookslno" HeaderText="Book slno" />
                                                              
                                                                <asp:TemplateField HeaderText="TakenBy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_TakenBy" runat="server" Text="" class="control-label"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="BookedBy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_bookedBy" runat="server" Text="" class="control-label"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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
                                                    </td>
                                                    <td class="e">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="so">
                                                    </td>
                                                    <td class="s">
                                                    </td>
                                                    <td class="se">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                
          
                    
            
            <asp:Button runat="server" ID="Btn_hdnmessagetgt" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server" CancelControlID="Btn_magok" 
                PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
            <asp:Panel ID="Pnl_msg" runat="server" Style="display: none;">
                <div class="container skin1" style="width: 400px; top: 400px; left: 200px">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" Height="28px"
                                    Width="29px" />
                            </td>
                            <td class="n">
                                alert!
                            </td>
                            <td class="ne">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="o">
                            </td>
                            <td class="c">
                                <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
                                <br />
                                <br />
                                <div style="text-align: center;">
                                    <asp:Button ID="Btn_magok" runat="server" Text="OK"  class="btn btn-primary" />
                                </div>
                            </td>
                            <td class="e">
                            </td>
                        </tr>
                        <tr>
                            <td class="so">
                            </td>
                            <td class="s">
                            </td>
                            <td class="se">
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </asp:Panel>
     
     
       <asp:Panel ID="Pnl_Advanced" runat="server">
     
     <asp:Button runat="server" ID="BtnAdvanceSearch" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="MPE_AdvanceSearch" runat="server" CancelControlID="Btn_Close" 
            BackgroundCssClass="modalBackground"
                PopupControlID="Pnl_Advance" TargetControlID="BtnAdvanceSearch" />
            <asp:Panel ID="Pnl_Advance" runat="server" Style="display: none;" >
            
          
            
            
                <div class="container skin1" style="width:600px; top: 400px; left: 200px">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no">
                                
                            </td>
                            <td class="n">
                               Detail Book Search
                            </td>
                            <td class="ne">
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="o">
                            </td>
                            <td class="c">
                             <asp:Panel ID="Pnl_DetailedSearch" runat="server" >
                                        
                                            <table  width="100%" >
                                           <tr>
                                                            <td>
                                                                Book Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_BkName" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender_BkName" 
                                                                            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBookName"  
                                                                            ServicePath="WinErWebService.asmx"  TargetControlID="Txt_BkName" MinimumPrefixLength="1">
                                                                          </ajaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                            
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                Enabled="True" TargetControlID="Txt_BkName" FilterType="Custom" FilterMode="InvalidChars"
                                                                InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            <td>
                                                                Book Id
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_BkId" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                                            </td>
                                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender_BookId" 
                                                                            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBookData"  
                                                                            ServicePath="WinErWebService.asmx"  TargetControlID="Txt_BkId" MinimumPrefixLength="1">
                                                                          </ajaxToolkit:AutoCompleteExtender>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_BkId_FilteredTextBoxExtender1" runat="server"
                                                                Enabled="True" TargetControlID="Txt_BkId" FilterType="Custom" FilterMode="InvalidChars"
                                                                InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </tr>
                                                        
                                                        
                                                        
                                                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                                                        
                                                        <tr>
                                                            <td>
                                                                Author Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_Author" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                                            </td>
                                                            
                                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender_Author" 
                                                                            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Autor"  
                                                                            ServicePath="WinErWebService.asmx"  TargetControlID="Txt_Author" MinimumPrefixLength="1">
                                                                          </ajaxToolkit:AutoCompleteExtender>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Author_FilteredTextBoxExtender1" runat="server"
                                                                Enabled="True" TargetControlID="Txt_Author" FilterType="Custom" FilterMode="InvalidChars"
                                                                InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            <td>
                                                                Publisher
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_Publisher" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                                            </td>
                                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender_Pblisher" 
                                                                            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Publisher"  
                                                                            ServicePath="WinErWebService.asmx"  TargetControlID="Txt_Publisher" MinimumPrefixLength="1">
                                                                          </ajaxToolkit:AutoCompleteExtender>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Publisher_FilteredTextBoxExtender1"
                                                                runat="server" Enabled="True" TargetControlID="Txt_Publisher" FilterType="Custom"
                                                                FilterMode="InvalidChars" InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </tr>
                                                        
                                                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                                                        
                                                        
                                                        <tr>
                                                            <td>
                                                                Category
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Cat" runat="server" Width="201px" class="form-control">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Type
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Type" runat="server" Width="201px" class="form-control">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                                    <tr>
                                                            <td>
                                                                Book Sl No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_bookslno" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                                            </td>
                                                    </tr>



                                                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                                                        <tr>
                                                            <td colspan="4" align="center">
                                                               
                                                           
                                                                 <asp:Button ID="Btn_AdvanceSearch" runat="server" OnClick="Btn_AdvanceSearch_Click" Text="Search"   class="btn btn-primary"/>&nbsp;&nbsp;&nbsp;
                                                                 <asp:Button ID="Btn_Close" runat="server" Text="Close"  class="btn btn-danger"/>
                                                            </td>
                                                        </tr>
                                            </table>
                                            <asp:Label ID="lbl_Err" Text="" runat="server" ForeColor="Red"></asp:Label>
                                            </asp:Panel>
                            </td>
                            <td class="e">
                            </td>
                        </tr>
                        <tr>
                            <td class="so">
                            </td>
                            <td class="s">
                            </td>
                            <td class="se">
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </asp:Panel>
     </asp:Panel>
    
        <div class="clear"> </div>
  </ContentTemplate>
       <Triggers >
       <asp:PostBackTrigger ControlID="img_export_Excel" />
       </Triggers>
</asp:UpdatePanel>
</div>
</asp:Content>
