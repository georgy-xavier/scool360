<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="Addbook.aspx.cs" Inherits="WinEr.Addbook"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type='text/javascript'>
    function Calculate() {
        var gridViewCtl = document.getElementById('<%=GrdBooks.ClientID%>');
        for (var i = 1; i < gridViewCtl.rows.length; i++) {
            var Txt_Barcode = gridViewCtl.rows[i].cells[2].children[0];
            if (Txt_Barcode.value == "") {
                Txt_Barcode.focus();
                return false;
            }
        }
    }    
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/add_page.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">Add New Book</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  <asp:Panel ID="Pnl_bookdetails" runat="server"  DefaultButton="Btn_Continue">
                      <table class="tablelist">
                       <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>
                      <tr><td class="leftside">
                       Book Name<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_name" runat="server" Width="195px" MaxLength="50" class="form-control"></asp:TextBox>
                         
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_name" ErrorMessage="Enter Book Name"></asp:RequiredFieldValidator>
                                                    
                      </td>
                      </tr>
                         
                      <tr><td class="leftside">
                        Author Name<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                      <asp:TextBox ID="Txt_auther" runat="server" Width="195px" MaxLength="50" class="form-control"></asp:TextBox>
                       <ajaxToolkit:FilteredTextBoxExtender ID="Txt_auther_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_auther" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_auther" ErrorMessage="Enter Author Name"></asp:RequiredFieldValidator>
                                   <ajaxToolkit:AutoCompleteExtender ID="Txt_publisher_AutoCompleteExtender" 
                                                runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Autor"  UseContextKey="true"
                                                ServicePath="WinErWebService.asmx"  TargetControlID="Txt_auther" MinimumPrefixLength="1">
                                                </ajaxToolkit:AutoCompleteExtender>
                                 
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                Enabled="True" TargetControlID="Txt_auther" FilterMode="InvalidChars" InvalidChars="\">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                               
                      </td>
                      </tr>
                      <tr><td class="leftside">
                        Publisher<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_Publisher" runat="server" Width="195px" MaxLength="50" class="form-control"></asp:TextBox>
                         
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Txt_Publisher" ErrorMessage="Enter Publisher Name"></asp:RequiredFieldValidator>
                                     <ajaxToolkit:AutoCompleteExtender ID="Txt_author_AutoCompleteExtender1" 
                                                runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetBook_Publisher"  UseContextKey="true"
                                                ServicePath="WinErWebService.asmx"  TargetControlID="Txt_Publisher" MinimumPrefixLength="1">
                                                </ajaxToolkit:AutoCompleteExtender>
                 
                                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_author_FilteredTextBoxExtender1" runat="server"
                                                Enabled="True" TargetControlID="Txt_Publisher" FilterMode="InvalidChars" InvalidChars="\">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                      </td>
                      </tr>
                      <tr><td class="leftside">
                      Year
                      </td>
                      <td class="rightside">
                        <asp:TextBox ID="Txt_year" runat="server" Width="195px" MaxLength="6" class="form-control"></asp:TextBox>
                           <ajaxToolkit:FilteredTextBoxExtender ID="Txt_year_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars=""  TargetControlID="Txt_year">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr><td class="leftside">
                      Edition
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_edition" runat="server" Width="195px" MaxLength="5" class="form-control"></asp:TextBox>
                        
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        
                      <tr><td class="leftside">
                      Type<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                        <asp:DropDownList ID="Drp_type" runat="server" Width="195px" class="form-control"
                                      onselectedindexchanged="Drp_type_SelectedIndexChanged">
                                  </asp:DropDownList>
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        
                      <tr><td class="leftside">
                       Category<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                       <asp:DropDownList ID="Drp_catagory" runat="server" Width="195px" class="form-control">
                                  </asp:DropDownList>
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      
                      <tr><td class="leftside">
                       Count<span class="style5">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_count" runat="server" Width="195px" MaxLength="2" class="form-control"></asp:TextBox>
                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_count_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars=""  TargetControlID="Txt_count">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Txt_count" ErrorMessage="Enter Book Count"></asp:RequiredFieldValidator>
                      </td>
                      </tr>
                      
                      
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      
                       <tr><td class="leftside">
                       Price
                       </td>
                      <td class="rightside"> 
                       <asp:TextBox ID="Txt_Price" runat="server"  Width="195px" MaxLength="8" class="form-control"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender_Txt_Price" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars="."  TargetControlID="Txt_Price">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           <tr><td class="leftside">
                       Book Sl No
                       </td>
                      <td class="rightside"> 
                       <asp:TextBox ID="Txt_bookslno" runat="server"  Width="195px"  class="form-control"></asp:TextBox>
                       
                      </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr id="BarCodeField" runat="server">
                      <td class="leftside">
                      <asp:Label ID="Lbl_BarCode" runat="server" Text="Barcode"></asp:Label><span class="style5">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_Barcode" runat="server" Width="195px" class="form-control"
                                    ></asp:TextBox>
                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Barcode_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom" TargetControlID="Txt_Barcode" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Txt_Barcode" ErrorMessage="Enter Barcodre "></asp:RequiredFieldValidator>
                      </td>
                      </tr>
                           <tr><td class="leftside"></td>
                      <td class="rightside">
                      <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                      </td>
                      </tr>
                           <tr><td class="leftside"></td>
                      <td class="rightside">
                        <asp:Button ID="Btn_Continue" runat="server" Text="Continue" Class="btn btn-primary"
                                      onclick="Btn_Continue_Click"  />
                                  &nbsp;&nbsp;
                                  
                      </td>
                      </tr>
                          
                         
                      </table>
                      
                      <asp:Panel ID="Pnl_bookgrid" runat="server" DefaultButton="Btn_save">
                     
        <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/add_page.png" 
                        Height="28px" Width="29px" /></td>
				<td class="n">Books</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					 <asp:Panel ID="Pnl_AddCopies" runat="server" DefaultButton="Btn_Focus" >
                    
                      <asp:Button ID="Btn_Focus" runat="server"  Text="" Width="1px" Height="1px"  BorderStyle="None"   
                       UseSubmitBehavior="false" OnClientClick="javascript: return Calculate();"   />
					   <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="false" Width="100%" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" PageSize="20" >
                           
                           <Columns>
                                <asp:BoundField DataField="Count" HeaderText="Count" />
                               
                                 <asp:TemplateField HeaderText="Book Id">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_BookId" runat="server"  MaxLength="15" Text="" class="form-control"
                                            Width="160"></asp:TextBox>
                                
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_BookId_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="Txt_BookId" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                       
                              
                                       
                                    </ItemTemplate>
                                   </asp:TemplateField>
                             <asp:TemplateField HeaderText="Barcode">
                                   <ItemTemplate>
                                    <asp:TextBox ID="Txt_Grd_Barcode" runat="server"  MaxLength="15" Text="" class="form-control"
                                            Width="160"></asp:TextBox>
                                
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Barcode_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="Txt_Grd_Barcode" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                       
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   
                                <asp:TemplateField HeaderText="Rack Id">
                                    <ItemTemplate>
                                        
                                        <asp:DropDownList ID="Drp_rack" runat="server" Width="128"  class="form-control">
                                        </asp:DropDownList>
                                       
                     <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server"
                TargetControlID="Drp_rack" PromptCssClass="ListSearchExtenderPrompt"
                QueryPattern="Contains" QueryTimeout="2000">
            </ajaxToolkit:ListSearchExtender>
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                       </asp:GridView>
					</asp:Panel>
					   <br />
					  
					   <div align="right">
					   <asp:Button ID="Btn_save" runat="server" Text="Save"  Class="btn btn-primary"
                                      onclick="Btn_save_Click" />
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
          
         </asp:Panel>
        
                    
        
    <br/>
    
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

 <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
       <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
           <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                <div class="container skin5" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
                            </td>
                            <td class="n"><span style="color:White">alert!</span></td>
                            <td class="ne">&nbsp;</td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" >               
                                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                                <br /><br />
                                <div style="text-align:center;">                          
                                    <asp:Button ID="Btn_magok" runat="server" Text="OK" Class="btn btn-primary"/>
                                </div>
                           </td>
                           <td class="e"> </td>
                       </tr>
                       <tr>
                            <td class="so"> </td>
                            <td class="s"> </td>
                            <td class="se"> </td>
                      </tr>
                </table>
                <br /><br />                                                
            </div>
         </asp:Panel> 
         
         
         
<div class="clear"></div>
</div>
</asp:Content>

