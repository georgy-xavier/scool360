<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" CodeBehind="IsseueBook.aspx.cs" Inherits="WinEr.IsseueBook"  %>
<%@ Register TagPrefix="WC" TagName="BOOKCOPIES" Src="~/WebControls/LibrarySelectionControl.ascx"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	 
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <div id="contents">

  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate> 
  <asp:Panel ID="Pnl_totalarea" runat="server" style="min-height:400px;">
  <asp:Panel ID="Pnl_useridsearch" DefaultButton="Btn_continue" runat="server" >
	
<div class="container skin1" style="margin-left: 10%;" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/user1.png" 
						Height="28px" Width="29px" />  </td>
				<td class="n">Issue Books</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<div  style="width:100%"  >
				<table width="100%">
					<tr>
						<td align="left">
						<asp:RadioButtonList ID="RdoBtn_UserType" runat="server" 
						RepeatDirection="Horizontal" AutoPostBack="True"  
						onselectedindexchanged="RdoBtn_UserType_SelectedIndexChanged" >
					
						</asp:RadioButtonList>
						</td>
						<td align="left">
						 <asp:DropDownList ID="Drp_SearchBy" runat="server" class="form-control" Width="200px" 
					AutoPostBack="True" onselectedindexchanged="Drp_SearchBy_SelectedIndexChanged" >
					<asp:ListItem Value="0">Admission No</asp:ListItem>
					<asp:ListItem Value="1" Selected="True">Student Name</asp:ListItem>
				   
				</asp:DropDownList>
						</td>
					</tr>
				</table>
				
				
				
				
				</div>
					<div id="newsearch">
			  <p >
			  <asp:TextBox ID="Txt_userid" runat="server" CssClass="search" ></asp:TextBox>
				  <ajaxToolkit:AutoCompleteExtender ID="Txt_userid_AutoCompleteExtender"   UseContextKey="true"
					  runat="server" DelimiterCharacters="" Enabled="True" ServicePath="WinErWebService.asmx"  ServiceMethod="GetUserName" MinimumPrefixLength="1"
					  TargetControlID="Txt_userid">
				  </ajaxToolkit:AutoCompleteExtender>
			   <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_userid_TextBoxWatermarkExtender" 
								runat="server" Enabled="True" TargetControlID="Txt_userid" WatermarkText="Enter Keyword">
							</ajaxToolkit:TextBoxWatermarkExtender>
			  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_userid_FilteredTextBoxExtender1" 
								runat="server" Enabled="True" TargetControlID="Txt_userid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
							</ajaxToolkit:FilteredTextBoxExtender>    
				&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_continue" runat="server"  Text="Continue" class="btn btn-primary" 
					  Width="75px" onclick="Btn_continue_Click" /></p>
			</div>
		
					
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

	<asp:Panel ID="Pnl_userdetails" runat="server" >
	<table >
	<tr>
	<td valign="top">
	<div class="container skin1"   style="margin-left: 11%;">
		<table   cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image9" runat="server" ImageUrl="~/Pics/info.png" 
						Height="28px" Width="29px" />  </td>
				<td class="n">User Details </td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<asp:Panel id ="pnl_contents" runat="server"  >
							<br />
							
							<asp:TextBox ID="Txt_SaveUser" runat="server" Visible="false" class="form-control"></asp:TextBox>
				  <asp:TextBox ID="BKNumber" runat="server" Visible="false"></asp:TextBox>
				  
						  
				<center>
					<table class="tablelist" width="100%">
					
						<tr>
							<td  class="leftside">
								Name<span style="color:Red">*</span></td>
							<td class="rightside">
								<asp:Label ID="Lbl_username" runat="server" Text="" Font-Bold="true"></asp:Label>
								
							</td>
							</tr><tr>
							<td  class="leftside">
								Type
							</td>
							<td class="rightside">
								<asp:Label ID="lbl_UserType" runat="server" Text="" Font-Bold="true"></asp:Label>
							   
							</td>
						</tr>
				  
						<tr>
							<td  class="leftside">
								<asp:Label ID="Lbl_Class" runat="server" Text="Class"></asp:Label>  </td>
							<td class="rightside">
							  
								<asp:Label ID="Lbl_class_department" runat="server" Text="" Font-Bold="true"></asp:Label>
							</td>
						   
						</tr>
					 
					</table>
					</center>
						</asp:Panel>
						<asp:Panel id ="Panel2" runat="server"  >
							<br />
							<asp:Label ID="Lbl_GridMessage" runat="server"></asp:Label>
				   
					<div  style=" overflow:auto; max-height:200px;">
					<asp:GridView  ID="Grd_ViewBook" runat="server" BackColor="White" 
						AutoGenerateColumns="False" 
						   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
						CellPadding="4" 
						   ForeColor="Black" GridLines="Vertical" Width="100%" >
						   
						   <Columns>
							   
								<asp:BoundField DataField="BookNo" HeaderText="Book No" />
								<asp:BoundField DataField="BookName" HeaderText="BookName"  />
								<asp:BoundField DataField="Author" HeaderText="Author" />
								 <asp:BoundField DataField="Action" HeaderText="Action" />
								 <asp:BoundField DataField="Date" HeaderText="Date" />
								 
																					  
							</Columns>
							 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
							 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
							 <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
							   ForeColor="Black"  HorizontalAlign="Left" />                                                                          
							 <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
							 <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
							 <EditRowStyle Font-Size="Medium" />     
					   </asp:GridView>
					   </div>
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
	
	</td>
	</tr>
	</table>
  
	
	
	</asp:Panel>
				  

  
	<asp:Panel ID="Pnl_bookarea" runat="server" CssClass="BookDetails" >
  

				 <asp:Panel ID="Panel1" DefaultButton="Btn_addbook" runat="server" >
				 <table width="100%">
					<tr>
						<td>
						<div class="container skin1" style="width:550px" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/add_page.png" 
						Height="28px" Width="29px" />  </td>
				<td class="n">Select Book</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<table>
				<tr>
				<td>Search By </td>
				<td>  <asp:DropDownList ID="Drp_booksearch" runat="server" class="form-control" Width="200px"
				 AutoPostBack="true" OnSelectedIndexChanged="Drp_booksearch_selectedIndexChanged" >
					<asp:ListItem Value="0" Selected="True">Book Name</asp:ListItem>
				  
				   <asp:ListItem Value="1" >Book No</asp:ListItem>
				   
				</asp:DropDownList></td>
				</tr>
				</table>
					<div class="newsearchbox">
			  <p >
			  <asp:TextBox ID="Txt_bookid" runat="server" CssClass="search"></asp:TextBox>
				  <ajaxToolkit:AutoCompleteExtender ID="Txt_bookid_AutoCompleteExtender"  CompletionListCssClass="searchmanagement"   
					  runat="server" DelimiterCharacters="" Enabled="True" ServicePath="WinErWebService.asmx"  ServiceMethod="GetBookId" MinimumPrefixLength="1"
					  TargetControlID="Txt_bookid">
				  </ajaxToolkit:AutoCompleteExtender>
			   <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_bookid_TextBoxWatermarkExtender1" 
								runat="server" Enabled="True" TargetControlID="Txt_bookid" WatermarkText="Enter Keyword">
							</ajaxToolkit:TextBoxWatermarkExtender>
			  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_bookid_FilteredTextBoxExtender1" 
								runat="server" Enabled="True" TargetControlID="Txt_bookid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
							</ajaxToolkit:FilteredTextBoxExtender>
						
				&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_addbook" runat="server"  Text="Add" Class="btn btn-primary" 
					  Width="75px" onclick="Btn_addbook_Click" /></p>
			</div>
		
					
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
						</td>
						<td>
					  
						
						 </td>
					</tr>
				 </table>


 
	
				 </asp:Panel>
				  
					<asp:Panel ID="Pnl_bookgrid" runat="server">
					
				   <div class="container skin1"  >
		<table   cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/Pics/add_to_folder.png" 
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
						onrowdeleting="GrdBooks_RowDeleting" >
						   
						   <Columns>
							   
								<asp:BoundField DataField="BookNo" HeaderText="Book No" />
								<asp:BoundField DataField="BookName" HeaderText="BookName"  />
								<asp:BoundField DataField="Author" HeaderText="Author" />
								 
								<asp:TemplateField HeaderText="Action">
									<ItemTemplate>                                       
										<asp:DropDownList ID="Drp_Status" runat="server" Width="75" class="form-control">
										</asp:DropDownList>                                                                                               
									</ItemTemplate>
								</asp:TemplateField>                               
								<asp:CommandField ShowDeleteButton="True" DeleteText="Cancel" />                                                             
							</Columns>
							 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
							 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
							 <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
							 <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"   HorizontalAlign="Left" />                                                                       
							 <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
							 <EditRowStyle Font-Size="Medium" />     
					   </asp:GridView>
					  
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
					   <table width="100%">
						<tr>
							<td align="right">
								<asp:Button ID="Btn_Issue" runat="server" Text="Issue / Book" Width="111px" 
									onclick="Btn_Issue_Click" class="btn btn-primary"/> </td>
						</tr>
					</table> 
				  </asp:Panel>
				
			  
			
	  
			</asp:Panel>
		  
		   <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
	   <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
		   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
				<div class="container skin1" style="width:400px; top:400px;left:200px" >
					<table   cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
							</td>
							<td class="n">alert!</td>
							<td class="ne">&nbsp;</td>
					   </tr>
					   <tr >
							<td class="o"> </td>
							<td class="c" >               
								<asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
								<br /><br />
								<div style="text-align:center;">                          
									<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-primary"/>
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
	
	
	  
		 
		  <asp:Button runat="server" ID="Btn_IMBox" style="display:none"/>
		   <ajaxToolkit:ModalPopupExtender ID="MPE_Issue"  runat="server" CancelControlID="BTn_IssueCancel"  PopupControlID="Pnl_message_Issue" TargetControlID="Btn_IMBox"  />
				<asp:Panel ID="Pnl_message_Issue" runat="server" style="display:none;" DefaultButton="Btn_IssueBk">
				<div class="container skin1" style="width:400px; top:400px;left:200px" >
					<table   cellpadding="0" cellspacing="0" class="containerTable">
						<tr >
							<td class="no"><asp:Image ID="Image8" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
							</td>
							<td class="n">alert!</td>
							<td class="ne">&nbsp;</td>
					   </tr>
					   <tr >
							<td class="o"> </td>
							<td class="c" >               
								<asp:Label ID="Label_issue" runat="server" Text=""></asp:Label>
								<br /><br />
								<div style="text-align:center;">   
									<asp:Button ID="Btn_IssueBk" runat="server" Text="Yes"  OnClick="Btn_IssueBk_click" class="btn btn-success"/>               
									<asp:Button ID="BTn_IssueCancel" runat="server" Text="No" Width="50px" class="btn btn-danger"/>
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
	   
		<WC:BOOKCOPIES id="WC_bookcopies" runat="server" /> 
		   <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
  </ContentTemplate>
 </asp:UpdatePanel>


<div class="clear"></div>

</div>
</asp:Content>
