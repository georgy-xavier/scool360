<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="RFreaderRegisteration.aspx.cs" Inherits="WinEr.RFreaderRegisteration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<div class="container skin1">
 <table cellpadding="0" cellspacing="0" class="containerTable">
  <tr >
   <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/book_search.png" 
                        Height="28px" Width="29px" /> </td>
   <td class="n">Register RF-Reader</td>
   <td class="ne"> </td>
  </tr>
  <tr >
	<td class="o"> </td>
	<td class="c" >
                   
	 <div style="min-height:200px;">
	  <br />
	  <div style="float:right">
	  
	
         <asp:ImageButton ID="Img_Unregistered" runat="server" 
             ImageUrl="~/Pics/book_warning.png" Width="30px" 
             onclick="Img_Unregistered_Click" />
	  
         <asp:LinkButton ID="Lnk_Unregistered" runat="server" 
             onclick="Lnk_Unregistered_Click">Show Unregistered RF-Readers</asp:LinkButton>
	  
	   </div>
	   <center>
           <asp:Label ID="lbl_errormsg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
	   </center>
	    <br />
	        <br />
	        
	   <asp:GridView ID="Grid_Registered"   runat="server" DataKeyNames="EquipmentId" 
          AutoGenerateColumns="False"   Width="100%" BackColor="White" 
             BorderColor="#DEDFDE" BorderStyle="None" 
          BorderWidth="1px" CellPadding="4" ForeColor="Black"  GridLines="Vertical" 
             onrowediting="Grid_Registered_RowEditing"  >                         
          <FooterStyle BackColor="#CCCC99" />
          <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
          <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
          <RowStyle BackColor="Transparent" />
          <Columns>
             <asp:BoundField DataField="EquipmentId" HeaderText="EquipmentId"  /> 
             <asp:BoundField DataField="Location" HeaderText="Location"/>                
             <asp:BoundField DataField="Description" HeaderText="Description" />
             <asp:BoundField DataField="UseType" HeaderText="InTime / OutTime"/>
             <asp:BoundField DataField="User" HeaderText="STUDENT / STAFF"/>
             <asp:BoundField DataField="Device" HeaderText="Device Model"/>
             <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Font-Bold="true"/>
             <asp:CommandField EditText="&lt;img src='Pics/Edit.png' width='30px' border=0 title='Edit'&gt;" 
                 ShowEditButton="True" HeaderText="Edit"  ItemStyle-Width="50px"  >
             </asp:CommandField>
          </Columns>
          <HeaderStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" CssClass="HeaderStyle"/>
          <AlternatingRowStyle BackColor="White" />
         </asp:GridView>
	   
	  
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
       
       
    <asp:Panel ID="Panel5" runat="server"> <%--Edit  Register  RFreader--%>
                       
   <asp:Button runat="server" ID="Button3" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_EditRFreader"   runat="server" CancelControlID="Btn_EditCancel"   PopupControlID="Panel6" TargetControlID="Button3" BackgroundCssClass="modalBackground"  />
   <asp:Panel ID="Panel6" runat="server" style="display:none;"> <%--style="display:none;"--%>
   <div class="container skin1" style="width:500px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
     <tr >
      <td class="no">
          <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/edit.png"  Height="28px" Width="29px" />
       </td>
         <td class="n">
         
          <span style="color:Black">Edit RF-Reader</span>
               
         </td>
      <td class="ne">&nbsp;</td>
     </tr>
     <tr >
      <td class="o"> </td>
      <td class="c" >
      
          <br/>
          
                
                
                <table width="100%" cellspacing="5">
                 <tr>
                  <td style="width:50%;" align="right">
                    Equipment ID : 
                  </td>
                   <td style="width:50%;">
                       <asp:Label ID="Lbl_EditRFReaderId" runat="server" Text="" class="control-label" ForeColor="Black" Font-Bold="true"> </asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Location : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_EditLocation" runat="server" Width="110px" class="form-control" MaxLength="150"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_EditLocation">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Description : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_Edit_Description" runat="server" Width="110px" class="form-control" MaxLength="245" Height="40px" TextMode="MultiLine"  ></asp:TextBox>
                       <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_Edit_Description">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    In / OutTime : 
                  </td>
                   <td style="width:50%;">
                       <asp:DropDownList ID="Drp_EditTime" Width="113px" class="form-control" runat="server">
                        <asp:ListItem Text="InTime" Value="0"></asp:ListItem>
                        <asp:ListItem Text="OutTime" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="2" Selected="True"></asp:ListItem>
                       </asp:DropDownList>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Student / Staff : 
                  </td>
                   <td style="width:50%;">
                         <asp:DropDownList ID="Drp_EditUser" class="form-control" Width="113px" runat="server">
                        <asp:ListItem Text="Student" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="2" Selected="True"></asp:ListItem>
                       </asp:DropDownList>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    IP : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_EditIP" runat="server" Width="110px" class="form-control" MaxLength="50"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" 
                           runat="server" Enabled="True" FilterType="Numbers,Custom"  FilterMode="ValidChars" ValidChars="."
                            TargetControlID="Txt_EditIP">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Port : 
                  </td>
                   <td style="width:50%;">
                        <asp:TextBox ID="Txt_EditPort" runat="server" Width="110px" class="form-control" MaxLength="14"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                           runat="server" Enabled="True" FilterType="Numbers"
                            TargetControlID="Txt_EditPort">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                  <tr>
                  <td style="width:50%;" align="right">
                    Device Model : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_EditDeviceModel" runat="server" Width="110px" class="form-control" MaxLength="150"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_EditDeviceModel">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Label ID="Lbl_EditRFreaderMSG" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Button ID="Btn_UpdateRFreader" runat="server" Text="Update" 
                          class="btn btn-info" onclick="Btn_UpdateRFreader_Click"/>
                      &nbsp;
                      <asp:Button ID="Btn_DeleteRFreader" runat="server" Text="Delete" 
                          Class="btn btn-info"  
                          OnClientClick="return confirm('Are you sure, you want to remove the RF-Reader?')" 
                          onclick="Btn_DeleteRFreader_Click" />
                      
                      &nbsp;
                      <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" Class="btn btn-info" />
                  </td>
                 </tr>
                </table>
                 
                 
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
       
   
                   
    <asp:Panel ID="Panel3" runat="server"> <%-- Register Unregistered RFreader--%>
                       
   <asp:Button runat="server" ID="Button2" Class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_EnterEFDetails"   runat="server" CancelControlID="Btn_Cancel"   PopupControlID="Panel4" TargetControlID="Button2" BackgroundCssClass="modalBackground"  />
   <asp:Panel ID="Panel4" runat="server" style="display:none;"> <%--style="display:none;"--%>
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
     <tr >
      <td class="no">
          <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png"  Height="28px" Width="29px" />
       </td>
         <td class="n">
         
          <span style="color:Black">Enter Details of RF-Reader</span>
               
         </td>
      <td class="ne">&nbsp;</td>
     </tr>
     <tr >
      <td class="o"> </td>
      <td class="c" >
      
          <br/>
          
                
                
                <table width="100%" cellspacing="5">
                 <tr>
                  <td style="width:50%;" align="right">
                    Equipment ID : 
                  </td>
                   <td style="width:50%;">
                       <asp:Label ID="Lbl_EquipmentId" runat="server" Text="" class="control-label" ForeColor="Black" Font-Bold="true"> </asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Location : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_Location" runat="server" Width="110px" Class="form-control" MaxLength="150"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_Location">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Description : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_Description" runat="server" Width="110px" Class="form-control" MaxLength="245" Height="40px" TextMode="MultiLine"  ></asp:TextBox>
                       <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_Description">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    In / OutTime : 
                  </td>
                   <td style="width:50%;">
                       <asp:DropDownList ID="Drp_Time" Width="113px" Class="form-control" runat="server">
                        <asp:ListItem Text="InTime" Value="0"></asp:ListItem>
                        <asp:ListItem Text="OutTime" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="2" Selected="True"></asp:ListItem>
                       </asp:DropDownList>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Student / Staff : 
                  </td>
                   <td style="width:50%;">
                         <asp:DropDownList ID="Drp_User" Width="113px" Class="form-control" runat="server">
                        <asp:ListItem Text="Student" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="2" Selected="True"></asp:ListItem>
                       </asp:DropDownList>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    IP : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_IP" runat="server" Width="110px" Class="form-control" MaxLength="50"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                           runat="server" Enabled="True" FilterType="Numbers,Custom"  FilterMode="ValidChars" ValidChars="."
                            TargetControlID="Txt_IP">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Port : 
                  </td>
                   <td style="width:50%;">
                        <asp:TextBox ID="Txt_Port" runat="server" Width="110px" Class="form-control" MaxLength="14"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" 
                           runat="server" Enabled="True" FilterType="Numbers"
                            TargetControlID="Txt_Port">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td style="width:50%;" align="right">
                    Device Model : 
                  </td>
                   <td style="width:50%;">
                       <asp:TextBox ID="Txt_DeviceModel" runat="server" Width="110px" Class="form-control" MaxLength="150"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="Txt_DeviceModel">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Label ID="lbl_Details_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Button ID="Btn_Register" runat="server" Text="Register" 
                          Class="btn btn-info" onclick="Btn_Register_Click" />
                      &nbsp;
                      <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-info" />
                  </td>
                 </tr>
                </table>
                 
                 
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
       
                    
                    
    <asp:Panel ID="Panel1" runat="server"> <%-- Unregistered--%>
                       
   <asp:Button runat="server" ID="Button1" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_Unregistered"   runat="server" CancelControlID="Img_close"     PopupControlID="Panel2" TargetControlID="Button1"  BackgroundCssClass="modalBackground" />
   <asp:Panel ID="Panel2" runat="server" style="display:none;"> <%--style="display:none;"--%>
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
     <tr >
      <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/book_warning.png"  Height="28px" Width="29px" /> </td>
         <td class="n">
         
              <table width="100%">
               <tr>
                <td>
                 <span style="color:Black">Unregistered</span>
                </td>
                <td align="right">
                    <asp:ImageButton ID="Img_close" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                </td>
               </tr>
              </table>
               
         </td>
      <td class="ne">&nbsp;</td></tr><tr >
      <td class="o"> </td>
      <td class="c" >
      
          <br/>
         <center>
           <asp:Label ID="lbl_unregistered_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
         </center>      
                
                <asp:GridView ID="Grid_Unregisterered" DataKeyNames="RFReaderID"  runat="server" 
                        AutoGenerateColumns="False"   Width="100%" BackColor="White" 
              BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black"  
              GridLines="Vertical" onrowediting="Grid_Unregisterered_RowEditing">                         
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="Transparent" />
                        <Columns>
                            <asp:BoundField DataField="RFReaderID" HeaderText="EquipmentId"  /> 

                            <asp:CommandField EditText="&lt;img src='Pics/accept.png' width='30px' border=0 title='Register'&gt;" 
                              ShowEditButton="True" HeaderText="Register"  ItemStyle-Width="50px"  >
                             </asp:CommandField>
                         </Columns>
                         <HeaderStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" CssClass="HeaderStyle"/>
                        <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                
                 
                 
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
                    
             </ContentTemplate> 
    </asp:UpdatePanel> 

<div class="clear"></div>
</div>
</asp:Content>
