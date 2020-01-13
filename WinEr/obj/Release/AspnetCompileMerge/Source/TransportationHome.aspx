<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="TransportationHome.aspx.cs" Inherits="WinEr.TransportationHome" %>
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



<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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



  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate>
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="30px" Width="30px" />  </td>
                <td class="n">TRANSPORTATION MANAGER</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
             
                  
                  <asp:Panel ID="Pnl_SearchArea" runat="server">
				<table cellspacing="10" width="95%">
                  <tr>
          
            <td style="width:300px;">
     
                    <div class="roundboxorange">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				        <div style="min-height:75px;">
				          <asp:Panel ID="pnl_searchbytepe" runat="server" DefaultButton="Img_Search">
				           <h3>Search Vehicle </h3>
				           <table>
				           <tr>
				             <td> 
                                <asp:Label ID="Label1" runat="server" Text="Vehicle Type"></asp:Label>
                             </td>
                             <td>
                                <asp:DropDownList ID="Drp_Categories" class="form-control" runat="server" Width="100px">
                                </asp:DropDownList>
                                 
                                 <asp:ImageButton ID="Img_Search" runat="server" 
                                     ImageUrl="~/images/SearchIcon.jpg" Width="20px" Height="20px" 
                                     onclick="Img_Search_Click" />
                                    
                            </td>
				           </tr>
				           <%--<tr>
				            <td class="style2" >
				            </td>
                             <td class="style2">
				               <asp:CheckBox ID="ChkWarning" runat="server" Text=" Warning Level" Checked="false" />
				           
                               
				            </td>
				           
				           </tr>--%>
				           </table>
				
                           </asp:Panel>
				    </div>
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
         
            </td>
                 
            <td >
                <div class="roundboxorange">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				         <div style="min-height:75px;">
				          <asp:Panel ID="panel_search" runat="server" DefaultButton="Img_Go">
				           <h3>Quick Search</h3>
				           <table><tr>
				             <td><asp:Label ID="Label2" runat="server" class="control-label" Text="Search By VehicleNo"></asp:Label>
				             </td>
				             <td>
				                     
				             </td>
				           </tr>
				           <tr>
				            <td colspan="2">
				                <asp:TextBox ID="TxtSearch" runat="server" class="form-control"></asp:TextBox>	
				                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="TxtSearch" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>

                                <ajaxToolkit:TextBoxWatermarkExtender ID="TxtSearch_TextBoxWatermarkExtender" 
                                    runat="server" Enabled="True" TargetControlID="TxtSearch" WatermarkText="Search" >
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <cc1:AutoCompleteExtender ID="TxtSearch_AutoCompleteExtender" 
                                                  runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetVehicle" ServicePath="WinErWebService.asmx"  
                                                  TargetControlID="TxtSearch" UseContextKey="true"  MinimumPrefixLength="1">
                                              </cc1:AutoCompleteExtender>
                                <asp:ImageButton ID="Img_Go" runat="server" ImageUrl="~/images/SearchIcon.jpg" 
                                    Width="20px" Height="20px" onclick="Img_Go_Click"/>
				            </td>
				           
				           </tr>
				           </table>
				
                           </asp:Panel>
				    </div>
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
                
            </td>
            
            <td>
            
              <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		   
		    <div style="min-height:75px;">
		    
		    <table class="style1">
                <%--<tr>
                    <td class="tdleft">
                                                        
        
                         <img  alt="" src="Pics/tag_green.png"  width="30px" /> </td>
                    <td class="tdRight">
                        <h4>
                            <asp:Label ID="lbl_type" runat="server" Text=""></asp:Label></h4></td>
                </tr>--%>
                <tr>
                    <td class="tdleft" >
                     <img  alt="" src="Pics/tag_green.png" height="15px"/> 
                        No of Vehicles:</td>
                    <td class="tdRight">
                        <asp:Label ID="lbl_VehicleNo" class="control-label" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdleft">
                        <img alt="" src="Pics/warning.png" width="14px" />No of Vehicle Types:</td>
                    <td class="tdRight">
                        <asp:Label ID="lbl_VehicleTypeNo" runat="server" class="control-label" Text="0"></asp:Label></td>
                </tr>
            </table>
		
	     </div>
		</td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		
		
		</div>	
            
            </td>
            
          </tr>
          
                 <tr >   
                 <td align="left">
                     <asp:Image ID="Img_Add" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" />
                     <asp:LinkButton ID="Lnk_AddNewVehicle" runat="server" CssClass="grayadd" 
                         Height="22px" onclick="Lnk_AddNewVehicle_Click" >ADD NEW VEHICLE</asp:LinkButton></td>
          <td align="right" colspan="2">
           
          </td>
          
          </tr>
          
                 </table>	
					</asp:Panel>
                  
                  
                  <asp:Panel ID="Pnl_VehicleList" runat="server">
            <div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">     
                <br />    
                <table width="100%"><tr>
		<td style="width:48px;">
		  <img alt="" src="Pics/evolution-tasks.png" width="45" height="45" />
       </td>
	<td><h3>VEHICLE LIST</h3></td>
	<td style="text-align:right;">
	
         </td>
	</tr></table>
		
<div class="linestyle"></div>  
          <asp:Label ID="Lbl_NoteErr" runat="server" Text=""></asp:Label>       
           <div >
                <asp:GridView ID="Grd_Vehicles" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%"   onpageindexchanging="Grd_Vehicles_PageIndexChanging"  OnSorting="Grd_Vehicles_Sorting"  onselectedindexchanged="Grd_Vehicles_SelectedIndexChanged" >
                   
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                           <EditRowStyle Font-Size="Medium" />
                                                           <Columns>
                                                           
                                                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" HeaderStyle-HorizontalAlign="Left" />
                                                              <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No"  SortExpression="VehicleNo" HeaderStyle-HorizontalAlign="Left"/>
                                                              <asp:BoundField DataField="RegNo" HeaderText="RegNo"  SortExpression="RegNo" HeaderStyle-HorizontalAlign="Left"/>
                                                               <asp:BoundField DataField="VehicleType" HeaderText="Type" SortExpression="VehicleType" HeaderStyle-HorizontalAlign="Left" />
                                                              <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity"  HeaderStyle-HorizontalAlign="Left"/>
                                                              <asp:BoundField DataField="Milage" HeaderText="Milage" SortExpression="Milage" HeaderStyle-HorizontalAlign="Left" />
                                                              <asp:BoundField DataField="Trips" HeaderText="Trips" SortExpression="Trips" HeaderStyle-HorizontalAlign="Left"/>
                                                              <asp:BoundField DataField="Distance" HeaderText="Total Km/Day" SortExpression="Distance" HeaderStyle-HorizontalAlign="Left" />
                                                              
                                                               <asp:TemplateField HeaderText="TripNames" HeaderStyle-HorizontalAlign="Left">
                                                               <ItemTemplate>
                                                               <%# Eval("TripNames")%>
                                                               </ItemTemplate>
                                                               </asp:TemplateField>
                                                               
                                                               <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select To View'&gt;"
                                                                      ShowSelectButton="True">
                                                                    <ControlStyle />
                                                                            <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                                                </asp:CommandField>
                                                          </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                              </asp:GridView>
                
                </div>    
          
           </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div> 
          
            </asp:Panel>
                  
                    <asp:Panel ID="Pnl_AddVehicle" runat="server" Visible="false" >
                 <div class="container skin1" >
              
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <asp:Image ID="Image1" runat="server" ImageUrl="Pics/add.png" 
                        Height="30px" Width="30px" />
                    </td>
				<td class="n">ADD NEW VEHICLE</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				        <br />
				        <br />
				   <table class="tablelist">
				         <tr>
                             <td class="leftside" >
                              Vehicle Type
                            </td>
                              <td class="rightside">
                                <asp:DropDownList ID="Drp_VehicleType" runat="server"  class="form-control" Width="200px" ></asp:DropDownList>
                                  <asp:LinkButton ID="LnkBtn_CreateType" runat="server" 
                                      onclick="LnkBtn_CreateType_Click">Add New</asp:LinkButton>
                            </td>                    
                        </tr>                       

				    <tr>
                            <td class="leftside">
                               Vehicle Registration No
                               </td>
                            <td class="rightside">
                                <asp:TextBox ID="txt_RegistrationNo" runat="server" Text ="" class="form-control" Width="200px" MaxLength="40"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_RegistrationNo" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txt_RegistrationNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server"  ControlToValidate="txt_RegistrationNo"  Display="Dynamic" ValidationGroup="VgSave" ErrorMessage="<br>Maximum 40 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                        <tr>
                            <td class="leftside">
                               Vehicle No
                               </td>
                            <td class="rightside">
                                <asp:TextBox ID="txt_VehicleNo" runat="server" Text ="" class="form-control" Width="200px" MaxLength="40"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_VehicleNo" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="txt_VehicleNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2"  runat="server"  ControlToValidate="txt_VehicleNo"  Display="Dynamic" ValidationGroup="VgSave" ErrorMessage="<br>Maximum 40 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                    
                         <tr>
                             <td class="leftside">
                              No of Seats
                                </td>
                              <td class="rightside">
                                  <asp:TextBox ID="txt_SeatNo" runat="server" Text ="" Width="200px" class="form-control" MaxLength="3"></asp:TextBox>
                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="txt_SeatNo"> </cc1:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_SeatNo" ErrorMessage="Enter a Number"></asp:RequiredFieldValidator>
                                  </td></tr>
                           <tr>
                             <td class="leftside">
                           Mileage
                            </td>
                              <td class="rightside">
                               <asp:TextBox ID="txt_Mileage" runat="server" Text ="" Width="200px" class="form-control" MaxLength="8"></asp:TextBox> 
                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="."  TargetControlID="txt_Mileage"> </cc1:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_Mileage" ErrorMessage="Enter Mileage"></asp:RequiredFieldValidator>
                            </td>
                    
                        </tr>
                        
                        
                        <tr>
                             <td class="leftside">
                             Add More Items?
                            </td>
                            <td class="rightside">
                                <asp:CheckBox ID="Chk_AddMore" Text="Yes" Checked="false" runat="server" /></td>
                                </tr> 
                                   <tr>
                             <td class="leftside">
                             <br />
                                </td>
                              <td class="rightside">
                              <br />
                               </td>
                            </tr>
                                  
                         <tr>
                             <td class="leftside">
                                </td>
                              <td class="rightside">
                              <asp:Button ID="Btn_SaveVehicle" runat="server" Text="Save" Class="btn btn-info"
                                         ValidationGroup="VgSave" onclick="Btn_SaveVehicle_Click" />
                               <asp:Button ID="Btn_VehicleCancel" runat="server" Text="Cancel" 
                                      Class="btn btn-danger" onclick="Btn_VehicleCancel_Click"
                                         />
                            </td>
                        </tr>
                    </table>
				<br />
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
                            <asp:TextBox ID="txt_new_category" runat="server" Text="" class="form-control" Width="200px"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_new_category" ErrorMessage="Please enter values"></asp:RequiredFieldValidator>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txt_new_category" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator4"  runat="server"  ControlToValidate="txt_new_category"  Display="Dynamic" ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                 <asp:Label ID="Lbl_MsgCreateCategory" runat="server" class="control-label" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                 <asp:Button ID="Btn_Add_new_cat" runat="server" Text="Save" Class="btn btn-info" ValidationGroup="VgSaveCategory" onclick="Btn_Add_new_cat_Click"/>
                                  <asp:Button ID="btn_cancel" runat="server" Text="Cancel" Class="btn btn-danger" />  
                                
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
               
                <asp:Label ID="Lbl_msg" runat="server" Text="" ></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-info" Text="OK" Width="50px"/>
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

    </ContentTemplate>
  </asp:UpdatePanel>
            

<div class="clear"></div>
</div>
</asp:Content>
