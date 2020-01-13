<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="VehicleDetails.aspx.cs" Inherits="WinEr.VehicleDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
       
        .style2
        {
            
        }
        .style3
        {
            
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<div id="right">

<div class="label">Vehicle Info</div>
<div id="SubTransMenu" runat="server">
		
 </div>
</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
</asp:UpdateProgress>--%>
   
   <div id="left">
      <div class="roundboxorange">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
		                
		                <table>
		                <tr>
		                <td align="left" style="width:350px">Vehicle Registration No. : <asp:Label ID="lbl_RegNo" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                <td style="width:50px"></td>
		                <td align="right">Vehicle No. : <asp:Label ID="lbl_VehicleNo" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                </tr>
		                <tr>
		                <td style="width:350px">Vehicle Type : <asp:Label ID="lbl_VehicleType" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                <td style="width:50px"></td>
		                <td align="right">No of Seats : <asp:Label ID="lbl_TotalSeats" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                </tr>
		                </table>
		                
		                
		                </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
	 </div>
   
    
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
        
        
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">Manage Vehicle Details</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  
                  <asp:Panel ID="Pnl_EditItem" runat="server">
                 <div class="container skin1" >
              
		
				   <table class="tablelist">
				      <tr>
                             <td class="leftside" >
                              Vehicle Type
                            </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_Category" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:DropDownList ID="Drp_EditCategory" runat="server"  class="form-control" Width="205px" ></asp:DropDownList>
                                  <asp:LinkButton ID="Lnk_EditnewCategory" runat="server" onclick="Lnk_EditnewCategory_Click" 
                                       >Add New</asp:LinkButton>
                            </td>                    
                        </tr>  
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                              

                        <tr>
                            <td class="leftside" style="width:300px">
                               Vehicle Register No
                               </td>
                            <td class="rightside">
                                <asp:Label ID="lbl_RegisterNo" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_EditRegNo" runat="server" Text ="" Width="200px" class="form-control" MaxLength="40"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditRegNo" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txt_EditRegNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server"  ControlToValidate="txt_EditRegNo"  Display="Dynamic" ValidationGroup="VgUpdate" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                       
                        <tr>
                             <td class="leftside">
                              Vehicle No
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_EditVehicleNo" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_EditVehicleNo" runat="server" Text ="" Width="200px" MaxLength="40" class="form-control"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditVehicleNo" Display="Dynamic" ErrorMessage="<br>Maximum  250 characters"  ValidationExpression="[\s\S]{1,250}"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditVehicleNo" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txt_EditVehicleNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                    
                         <tr>
                             <td class="leftside">
                              Total Seats
                                </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_EditTotalSeats" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:TextBox ID="txt_EditSeats" runat="server" Width="200px" MaxLength="8" class="form-control"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_EditSeats"> </cc1:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditSeats" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                                  </td></tr>
                                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                           <tr>
                             <td class="leftside">
                           Mileage
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_mileage" runat="server" Font-Bold="true"></asp:Label>
                               <asp:TextBox ID="txt_EditMileage" runat="server" Text ="" Width="200px" class="form-control" MaxLength="8"></asp:TextBox> 
                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" FilterType="Custom,Numbers" ValidChars="." TargetControlID="txt_EditMileage"> </cc1:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditMileage" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                       <tr><td><br /></td><td><br /></td></tr>
                    
                                  
                         <tr>
                             <td class="leftside">
                             <asp:Button ID="Btn_EditVehicle" runat="server" Text="Edit"  class="btn btn-primary" 
                                     onclick="Btn_EditVehicle_Click" />
                                <asp:Button ID="Btn_UpdateVehicle" runat="server" Text="Update"
                                         ValidationGroup="VgUpdate"  class="btn btn-success" 
                                     onclick="Btn_UpdateVehicle_Click" />  
                                </td>
                              <td class="rightside">
                                      
                                   <asp:Button ID="Btn_Delete" runat="server" Text="Delete"  class="btn btn-danger" 
                                       onclick="Btn_Delete_Click"   />
                               <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" class="btn btn-primary" 
                                       onclick="Btn_EditCancel_Click"  />
                            </td>
                        </tr>
                        
                    </table>
				
	            </div>
                  </asp:Panel>
                  
                  
                  
                    <asp:Button runat="server" ID="Btn_HidAdd_New_Category" style="display:none"/>
           <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox_AddNewCategory"   runat="server"  PopupControlID="Pnl_AddNewCategory" TargetControlID="Btn_HidAdd_New_Category"  />
              <asp:Panel ID="Pnl_AddNewCategory" runat="server" style="display:none">
 <div id="newprocess" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Create New Category</td>
            <td class="ne">&nbsp;</td>
       </tr>
       <tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                            <td>
                                Enter Type Name  
                               <br />  
                            <asp:TextBox ID="txt_new_category" runat="server" Text="" Width="200px" class="form-control"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_new_category" ErrorMessage="Please enter values"></asp:RequiredFieldValidator>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txt_new_category" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator4"  runat="server"  ControlToValidate="txt_new_category"  Display="Dynamic" ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                 <asp:Label ID="Lbl_MsgCreateCategory" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                 <asp:Button ID="Btn_Add_new_cat" runat="server" Text="Save" class="btn btn-success" ValidationGroup="VgSaveCategory" onclick="Btn_Add_new_cat_Click"/>
                                  <asp:Button ID="btn_cancel" runat="server" Text="Cancel" class="btn btn-danger" />  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
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
 </div>
</asp:Panel>
                  
               
               <asp:Panel ID="Pnl_DeleteConfirm" runat="server">
                       
                       <%--  <asp:Button runat="server" ID="Button4" style="display:none"/>--%>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteConfirm" 
                                  runat="server" CancelControlID="Btn_DeleteNo" 
                                  PopupControlID="Pnl_ConfirmDelete" TargetControlID="Btn_Delete"  />
                          <asp:Panel ID="Pnl_ConfirmDelete" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="lbl_delmsg" runat="server" Text="Are you sure to delete the item?"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_DeleteYes" runat="server" Text="Yes" class="btn btn-success" 
                                onclick="Btn_DeleteYes_Click"/>
                             <asp:Button ID="Btn_DeleteNo" runat="server" Text="No"  class="btn btn-danger"/>
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>
                  
                  
                  <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
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
    <br />
                   
</div>
       </asp:Panel>                 
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

      
            
         
   
</div>
<div class="clear"></div>
</div>
</asp:Content>
