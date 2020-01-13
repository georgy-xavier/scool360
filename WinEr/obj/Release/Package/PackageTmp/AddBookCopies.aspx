<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" CodeBehind="AddBookCopies.aspx.cs" Inherits="WinEr.AddBookCopies"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"   >

    <div id="contents" >

<div id="right">

<div class="label">Library Manager</div>
<div id="SubLibMenu" runat="server">
		
 </div>
</div>

<div id="left">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:Panel ID="Pnl_mainarea" runat="server">
    
        
<div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/book_accept.png" 
                        Height="28px" Width="29px" />  </td>
				<td class="n">Add Book Copies</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<br />
				<div style="min-height:100px">
				<asp:Panel ID ="Pnl_main" runat="server"  >
				    <table width="100%"  class="tablelist">
				        <tr>
				            <td></td>
				            <td></td>
				        </tr>
				      
				        
				        <tr>
				            <td class="leftside">Book Name</td>
				            <td  class="rightside">
				            
                                 <asp:Label ID="Lbl_BkName" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
                                 </td>
                     </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                     <tr>
				            <td class="leftside">
                                Enter Count</td>
                            <td class="rightside">
                                <asp:TextBox ID="Txt_Count" runat="server" Width="160px" class="form-control" MaxLength="2" ></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Count_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="Txt_Count">
                    </ajaxToolkit:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_Count" ErrorMessage="Enter number of copies"></asp:RequiredFieldValidator>
                                
                            </td>
				        </tr>
				        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

				        <tr id="BarcodeSection" runat="server">
				        <td class="leftside">
				        Barcode
				        </td>
				        <td class="rightside">
				       
                            <asp:TextBox ID="Txt_Barcode" runat="server" ReadOnly="true" class="form-control" Width="160px" ></asp:TextBox>
                              <ajaxToolkit:FilteredTextBoxExtender ID="Txt_barcode_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="Txt_Barcode" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                        </ajaxToolkit:FilteredTextBoxExtender>
				          
				        </td>
				        
				        </tr>
				        <tr>
                            
                            <td>
                                &nbsp;
                                </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                  
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="Btn_Continue" runat="server" onclick="Btn_Continue_Click" 
                                    Text="Continue" class="btn btn-success"/>
                                
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger"
                                    onclick="Btn_Cancel_Click" />
                            </td>
                        </tr>
				    
				    </table>
				</asp:Panel>
				</div>
				      <br />
				      <asp:Panel ID="Pnl_bookgrid" runat="server"  DefaultButton="Btn_Save" >
                     
                     
        <div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Books</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
                    <asp:Panel ID="Pnl_AddCopies" runat="server" DefaultButton="Btn_Focus" >
                    
                       <asp:Button ID="Btn_Focus" runat="server"  Text="" Width="1px" Height="1px"  BorderStyle="None"   
                       UseSubmitBehavior="false" OnClientClick="javascript: return Calculate();"   /> 
                       
					   <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="false"
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4"  
                           ForeColor="Black" GridLines="Vertical" Width="100%" >
                           
                           <Columns>
                                <asp:BoundField DataField="Count" HeaderText="Count" />
                               
                                 <asp:TemplateField HeaderText="Book Id">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_BookId" runat="server" class="form-control" MaxLength="15" Text="" 
                                            Width="160"></asp:TextBox>
                                
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_BookId_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="Txt_BookId" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                       
                              
                                       
                                    </ItemTemplate>
                                   </asp:TemplateField>
                                   
                                     <asp:TemplateField HeaderText="Barcode">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Grd_BarCode"  runat="server"  class="form-control"  Text=""  Width="160"></asp:TextBox>
                                
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Grd_BarCode_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="Txt_Grd_BarCode" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                       
                              
                                       
                                    </ItemTemplate>
                                   </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rack Id">
                                    <ItemTemplate>
                                        
                                        <asp:DropDownList ID="Drp_rack" runat="server" Width="128" class="form-control" >
                                        </asp:DropDownList>
                                       
                                         <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server"
                                    TargetControlID="Drp_rack" PromptCssClass="ListSearchExtenderPrompt"
                                    QueryPattern="Contains" QueryTimeout="2000">
                                </ajaxToolkit:ListSearchExtender>
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <RowStyle BackColor="#F7F7DE" />
                           <FooterStyle BackColor="#CCCC99" />
                           <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                           <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                           <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                               HorizontalAlign="Left" />
                           <AlternatingRowStyle BackColor="White" />
                       </asp:GridView>
					</asp:Panel>
					   <br />
					   <div align="right">
					   <asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" 
                                    Text="Save" class="btn btn-primary" />&nbsp;&nbsp;&nbsp;
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
				        
				<br />
				</td>
				<td class="e"> </td>
			</tr>
			<tr>
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
                                    <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary"/>
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
	
    </asp:Panel>
</div>
<div class="clear"></div>
</div>
</asp:Content>
