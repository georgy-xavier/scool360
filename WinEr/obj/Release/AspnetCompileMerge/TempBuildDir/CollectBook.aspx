<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" CodeBehind="CollectBook.aspx.cs" Inherits="WinEr.CollectBook"  %>
<%@ Register TagPrefix="WC" TagName="BOOKCOPIES" Src="~/WebControls/LibrarySelectionControl.ascx"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">

 <div id="left" style="width:100%" >
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate> 
    

       <asp:Panel ID="Pnl_bookarea" runat="server" style="min-height:400px;">
  

                 <asp:Panel ID="Panel1" DefaultButton="Btn_addbook" runat="server" >
                 

<div class="container skin1" style="width:550px" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/Manage.png" 
                   Height="28px"  Width="29px" />  </td>
				<td class="n">Collect Books </td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<table>
				<tr>
				<td>Search By </td>
				<td>  <asp:DropDownList ID="Drp_booksearch" runat="server"  Width="200px" class="form-control"
				 AutoPostBack="true" OnSelectedIndexChanged="Drp_booksearch_selectedIndexChanged" >
                    <asp:ListItem Value="0" Selected="True">Book Name</asp:ListItem>
                  
                   <asp:ListItem Value="1" >Book No</asp:ListItem>
                  
                </asp:DropDownList></td>
				</tr>
				</table>
					
					
					
					
					<div  id="newsearch">
              <p >
              <asp:TextBox ID="Txt_bookid" runat="server" CssClass="search" ></asp:TextBox>
                  <ajaxToolkit:AutoCompleteExtender ID="Txt_bookid_AutoCompleteExtender" 
                      runat="server" DelimiterCharacters="" Enabled="True" ServicePath="WinErWebService.asmx"  ServiceMethod="GetIsssuedBookId" MinimumPrefixLength="1" 
                      TargetControlID="Txt_bookid">
                  </ajaxToolkit:AutoCompleteExtender>
               <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_bookid_TextBoxWatermarkExtender1" 
               runat="server" Enabled="True" TargetControlID="Txt_bookid" WatermarkText="Enter Keyword">
               </ajaxToolkit:TextBoxWatermarkExtender>
            
                        
                &nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_addbook" runat="server"  Text="Add" 
                      Class="btn btn-primary" onclick="Btn_addbook_Click" /></p>
                      </div>
      	  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_bookid_FilteredTextBoxExtender1" 
              runat="server" Enabled="True" TargetControlID="Txt_bookid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
               </ajaxToolkit:FilteredTextBoxExtender>
					
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
 
    
                 </asp:Panel>
                  
                    <asp:Panel ID="Pnl_bookgrid" runat="server" >
                    
                   <div class="container skin1" style="max-width:100%" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/Pics/book.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">Book List </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  
               <asp:GridView  ID="GrdBooks" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" 
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4"
                           ForeColor="Black" GridLines="Vertical" Width="100% " 
                        onrowdeleting="GrdBooks_RowDeleting"  OnRowCommand="GrdBooks_RowCommand">
                           
                           <Columns>
                              
                                <asp:BoundField DataField="BookNo" HeaderText="Book No" />
                                <asp:BoundField DataField="BookName" HeaderText="BookName" />
                                
                                <asp:TemplateField HeaderText="UserID">
                                    <ItemTemplate>                                       
                                        
                                        <asp:Label ID="Lbl_UserId" runat="server" Text=""></asp:Label>                                                                                     
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>                                       
                                        
                                        <asp:Label ID="Lbl_UserName" runat="server" Text=""></asp:Label>                                                                                     
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                
                               
                                <asp:BoundField DataField="USerType" HeaderText="UserType" />
                                 <asp:TemplateField HeaderText="Issued Date">
                                    <ItemTemplate>                                       
                                        
                                        <asp:Label ID="Lbl_Date" runat="server" Text=""></asp:Label>                                                                                     
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField HeaderText="Fine (Rs)">
                                    <ItemTemplate>                                       
                                        
                                        <asp:Label ID="Lbl_Fine" runat="server" Text=""></asp:Label>                                                                                     
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                  <asp:TemplateField HeaderText="Comment">
                                    <ItemTemplate>                                       
                                        <asp:TextBox ID="Txt_Comment" runat="server"  MaxLength="45" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_Txt_comment" 
                                runat="server" Enabled="True" TargetControlID="Txt_Comment" WatermarkText="EnterYour Comment">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender_Txt_comment" 
                                runat="server" Enabled="True" TargetControlID="Txt_Comment" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                             
                               
                                 <asp:ButtonField ItemStyle-Width="30" HeaderText="Renewal" ButtonType="Link" CommandName="Renew" Text="&lt;img src='Pics/index.jpg' width='30px' border=0 title='Renew Book' &gt;">
                                                <ItemStyle Width="30px" />
                                  </asp:ButtonField>
                                   <asp:CommandField ShowDeleteButton="True"  DeleteText="Cancel" HeaderText="Cancel"/>
                               
                            </Columns>

                             <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
                             <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                             <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                             <EditRowStyle Font-Size="Medium" />    
                       </asp:GridView>
                      <br />
                      <div align="right">
                      <asp:Label ID="Label_Fine" runat="server" Text="Total Fine" Font-Bold="True"></asp:Label> 
                      &nbsp; &nbsp;<asp:Label ID="Lbl_TottalFine" runat="server" Font-Bold="False"></asp:Label>
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
            <table align="right">
                <tr>
                <td align="right">
                    </td>
                    <td align="right">
                      
                    </td>
                  
                    <td align="right">
                        <asp:Button ID="Btn_Collect" runat="server" Text="Collect" 
                            onclick="Btn_Collect_Click" Class="btn btn-primary" /></td>
                </tr>
            </table>
                  
                  </asp:Panel>
                      
                     
            
      
            </asp:Panel>
        
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

        <WC:BOOKCOPIES id="WC_bookcopies" runat="server" /> 
           <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
            </ContentTemplate>
 </asp:UpdatePanel>
</div>

<div class="clear"></div>
</div>
</asp:Content>

