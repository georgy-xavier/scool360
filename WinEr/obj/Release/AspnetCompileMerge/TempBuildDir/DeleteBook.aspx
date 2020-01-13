<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="DeleteBook.aspx.cs" Inherits="WinEr.DeleteBook"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">

<div id="right">

<div class="label">Library Manager</div>
<div id="SubLibMenu" runat="server">
		
 </div>
</div>

<div id="left">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:Panel ID="Pnl_mainarea" runat="server">
    
        
        <div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/book_accept.png" 
                        Height="28px" Width="29px" />  </td>
				<td class="n">Delete Book</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >				
				<br />
				<asp:Panel ID="Pnl_Main" runat="server">
				   <table width="100%" class="tablelist">
				    <tr>
				        <td></td>
				        <td></td>
				    </tr>
				     <tr>
				        <td class="leftside">Book Name :</td>
				        <td class="rightside">  
                            <asp:Label ID="Lbl_BkName" runat="server" Text=""  Font-Bold="true"  ForeColor="Black"></asp:Label>
                             </td>
				    </tr>
				    <tr>
				        <td class="leftside">Book Code :</td>
				        <td class="rightside">  
                            <asp:Label ID="Lbl_BookCode" runat="server" Text=""  Font-Bold="true"  ForeColor="Black"></asp:Label>
                             </td>
				    </tr>
				    </table> <div class="linestyle"></div><table width="100%" class="tablelist">
				       
                       <tr>
                           <td  class="leftside">
                               &nbsp;</td>
                           <td  class="rightside">
                               <asp:RadioButtonList ID="Rdo_List" runat="server">
                                   <asp:ListItem Selected="True" Value="0">Delete only this book</asp:ListItem>
                                   <asp:ListItem Value="1">Delete all copies of this book</asp:ListItem>
                               </asp:RadioButtonList>
                           </td>
                       </tr>
                       <tr>
                           <td>
                               &nbsp;</td>
                           <td>
                               &nbsp;</td>
                       </tr>
                       <tr>
                           <td>
                               &nbsp;</td>
                           <td>
                               <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-primary" 
                                   onclick="Btn_Delete_Click" />
                               &nbsp;&nbsp;
                               <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger" 
                                   onclick="Btn_Cancel_Click" />
                           </td>
                       </tr>
				   </table> 
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
	
	     
       <ajaxToolkit:ModalPopupExtender ID="MPE_Conformation"  runat="server" CancelControlID="Btn_magok1"  PopupControlID="Pnl_msg1" TargetControlID="Btn_Delete"  />
           <asp:Panel ID="Pnl_msg1" runat="server" style="display:none;">
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
                                Are you sure you want to delete the Book(s).  
                                <br /><br />
                                <div style="text-align:center;"> 
                                     
                                    <asp:Button ID="Btn_Del" runat="server" Text="Yes" class="btn btn-success" OnClick=" Btn_Delete_Click"/>               
                                    <asp:Button ID="Btn_magok1" runat="server" Text="No" class="btn btn-danger"/>
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
	
	
	     <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
       <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
           <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                <div class="container skin5" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
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
         
             <asp:Button runat="server" ID="Btn_MesageDelete" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_Cancel"  runat="server"  PopupControlID="Pnl_msg2" TargetControlID="Btn_MesageDelete"  />
             <asp:Panel ID="Pnl_msg2" runat="server" style="display:none">
                <div class="container skin5" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
                            </td>
                            <td class="n"><span style="color:White">alert!</span></td>
                            <td class="ne">&nbsp;</td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" >               
                                <asp:Label ID="Lbl_DelMs" runat="server" Text=""></asp:Label>
                                <br /><br />
                                <div style="text-align:center;">                          
                                    <asp:Button ID="Btn_DelOk" runat="server" Text="OK" class="btn btn-primary" 
                                        onclick="Btn_DelOk_Click" />
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
