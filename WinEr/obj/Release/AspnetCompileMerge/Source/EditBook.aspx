<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="EditBook.aspx.cs" Inherits="WinEr.EditBook"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
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
    
        <asp:Panel runat="server" DefaultButton="Btn_BkUpdate">
<div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/book_accept.png" 
                        Height="28px" Width="29px" />  </td>
				<td class="n">Edit Book</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<br />
				
				
				        <table class="tablelist" >
                          
                          <tr>
                              <td  class="leftside">
                                  Book Name<span class="style1">*</span></td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_name" runat="server" Width="200px" class="form-control" MaxLength="50"></asp:TextBox>
                              </td>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_name_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_name" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                              <td class="leftside">
                                  Author Name<span class="style1">*</span></td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_auther" runat="server" Width="200px" class="form-control" MaxLength="50"></asp:TextBox>
                              </td>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_auther_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_auther" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                              <td class="leftside">
                                  Publisher<span class="style1">*</span></td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_Publisher" runat="server" Width="200px" MaxLength="50" class="form-control"></asp:TextBox>
                              </td>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Publisher_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_Publisher" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                              <td class="leftside">
                                  Year</td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_year" runat="server" Width="200px" class="form-control" MaxLength="6"></asp:TextBox>
                              </td>
                              
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_year_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars=""  TargetControlID="Txt_year">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         
                          <tr>
                              <td class="leftside">
                                  Edition</td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_edition" runat="server" Width="200px"  class="form-control" MaxLength="5"></asp:TextBox>
                              </td>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_edition_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars="-,/,."  TargetControlID="Txt_edition">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                              <td class="leftside">
                                  Type</td>
                              <td class="rightside">
                                  <asp:DropDownList ID="Drp_type" runat="server" class="form-control" Width="200px">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         
                          <tr>
                              <td class="leftside">
                                  Category</td>
                              <td class="rightside">
                                  <asp:DropDownList ID="Drp_catagory" runat="server" class="form-control" Width="200px">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                             <td class="leftside">Rack</td>
                             <td class="rightside"> 
                                 <asp:DropDownList ID="Drp_Rack" runat="server" class="form-control" Width="200px">
                                 </asp:DropDownList>
                             </td>
                          </tr>
                          
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          
                          
                            <tr>
                                <td class="leftside">
                                    Price</td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_Price" runat="server" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                </td>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender_Price" 
                                 runat="server" Enabled="True" FilterType="Custom, Numbers"
                                 ValidChars="."  TargetControlID="Txt_Price">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                          </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          
                          
                            <tr>
                                <td class="leftside">
                                    Book Sl No</td>
                                <td class="rightside">
                                    <asp:TextBox ID="txt_bookslno" runat="server" Width="200px" MaxLength="15" class="form-control"></asp:TextBox>
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
                       <asp:TextBox ID="Txt_Barcode" runat="server" Width="200px" class="form-control"
                                    ></asp:TextBox>
                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Barcode_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" FilterType="Custom" TargetControlID="Txt_Barcode" FilterMode="InvalidChars" InvalidChars="'\">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Txt_Barcode" ErrorMessage="Enter Barcodre "></asp:RequiredFieldValidator>
                      </td>
                      </tr>
                          <tr>
                              <td class="leftside">
                              </td>
                              <td class="rightside">
                                  <asp:Button ID="Btn_BkUpdate" runat="server" onclick="Btn_BkUpdate_Click" 
                                      Text="Update" class="btn btn-success" />
                                  &nbsp;<asp:Button ID="Btn_BkCancel" runat="server" onclick="Btn_BkCancel_Click" 
                                      Text="Cancel" class="btn btn-danger" />
                              </td>
                          </tr>
                      </table>
				        
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
