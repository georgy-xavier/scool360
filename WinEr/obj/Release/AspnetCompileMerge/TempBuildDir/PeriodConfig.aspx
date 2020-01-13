<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="PeriodConfig.aspx.cs" Inherits="WinEr.PeriodConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
   .HeaderStyle
   {
       margin-left:30px;
   }
     
 </style>

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
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Period Configuration</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
                   
					
					
					
					<table width="100%">
					 <tr>
					  <td align="left">
					  
                          <asp:ImageButton ID="Img_Add" runat="server" ImageUrl="~/Pics/add.png" 
                              Width="30px" onclick="Img_Add_Click"  />
					   
                          <asp:LinkButton ID="Lnk_Add" runat="server" onclick="Lnk_Add_Click">Add New Period</asp:LinkButton>
					     
					  </td>
					 </tr>
					 <tr>
					  <td>
					  
                       <br />

                       <asp:GridView ID="Grid_Periods" DataKeyNames="PeriodId"  runat="server" 
                        AutoGenerateColumns="False"   Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black"  GridLines="Vertical" onrowediting="Grid_Periods_RowEditing"  >                         
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="Transparent" />
                        <Columns>
                            <asp:BoundField DataField="PeriodId" HeaderText="PeriodId"  /> 
                            <asp:BoundField DataField="FrequencyName" HeaderText="Period Name"/>                
                            <asp:BoundField DataField="FromTime" HeaderText="From Time" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ToTime" HeaderText="To Time" ItemStyle-Width="100px" />
                            <asp:CommandField EditText="&lt;img src='Pics/edit.png' width='30px' border=0 title='Edit'&gt;" 
                              ShowEditButton="True" HeaderText="Edit"  ItemStyle-Width="50px"  >
                             </asp:CommandField>
                         </Columns>
                         <HeaderStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" CssClass="HeaderStyle"/>
                        <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>

					  
					  </td>
					 </tr>					
					</table>
					
					
					
					
					
					   
	
					
					
					
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
       
   
           <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label><br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-info"/>
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
           
           
 <%-- ADD PERIOD      --%>
           
 <asp:Panel ID="Panel3" runat="server">
  <asp:Button runat="server" ID="Button2" class="btn btn-info" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_Add"  runat="server" CancelControlID="ImageButton2" BackgroundCssClass="modalBackground"
      PopupControlID="Panel4" TargetControlID="Button2"  />
   <asp:Panel ID="Panel4" runat="server" style="display:none;"> <%--style="display:none;"--%>
    <div class="container skin1" style="width:500px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n">
               
                 <table width="100%">
                   <tr>
                     <td align="left">
                          <span style="color:Black">Add Period</span>
                      </td>
                      <td align="right">
                           <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                      </td>                  
                   </table>
            </td>
            <td class="ne">&nbsp;</td>
            </tr><tr >
            <td class="o"> </td>
            <td class="c" >
               

               
               <table width="100%" cellspacing="10">
                <tr>
                 <td align="right" style="width:50%">
                 
                  Period : 
                 
                 </td>
                 <td >
                 
                     <asp:TextBox ID="Txt_AddPeriod" runat="server" Font-Bold="true" class="form-control" MaxLength="9"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="@#$%^&*;?'/\"
                            TargetControlID="Txt_AddPeriod">
                           </ajaxToolkit:FilteredTextBoxExtender>
                 </td>
                 <td>
                    
                 </td>
               </tr>
               <tr>
                 <td style="width:50%" align="right">
                 
                  From Time
                 
                 </td>
                 <td>
                 
                   <asp:TextBox ID="Txt_AddFromTime" runat="server" Font-Bold="true" class="form-control"></asp:TextBox>
                   
                 </td>
                 <td>
                 
                 <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" 
                                runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99" 
                                MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_AddFromTime" />
                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" 
                                runat="server" ControlExtender="MaskedEditExtender2" 
                                ControlToValidate="Txt_AddFromTime" Display="Dynamic" EmptyValueBlurredText="*" 
                                EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                TooltipMessage="Input a time" ValidationGroup="Add" />
                 
                 
                  
                 
                 </td>
               </tr>
               <tr>
                 <td style="width:50%" align="right">
                  To Time
                 </td>
                 <td>
                 
                   <asp:TextBox ID="Txt_AddToTime" runat="server" Font-Bold="true" class="form-control"></asp:TextBox>
                    
                 </td>
                 <td>
                 
                  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" 
                                runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99" 
                                MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_AddToTime" />
                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator3" 
                                runat="server" ControlExtender="MaskedEditExtender3" 
                                ControlToValidate="Txt_AddToTime" Display="Dynamic" EmptyValueBlurredText="*" 
                                EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                TooltipMessage="Input a time" ValidationGroup="Add" />
                 
                 </td>
               </tr>
               <tr>
                 <td  align="center" colspan="3">
                 
                     <asp:Button ID="Btn_Add" runat="server" Text="Add" class="btn btn-info" 
                         ValidationGroup="Add" onclick="Btn_Add_Click"  />
                 
                 </td>
                </tr>
                
                <tr>
                 <td colspan="3" align="center">
                     <asp:Label ID="Lbl_AddError" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
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
           
           
<%-- EDIT PERIOD      --%>
           
 <asp:Panel ID="Panel1" runat="server">
  <asp:Button runat="server" ID="Button1" class="btn btn-info" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_Edit"  runat="server" CancelControlID="ImageButton1" BackgroundCssClass="modalBackground"
      PopupControlID="Panel2" TargetControlID="Button1"  />
   <asp:Panel ID="Panel2" runat="server" style="display:none;"> <%--style="display:none;"--%>
    <div class="container skin1" style="width:500px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n">
               
                 <table width="100%">
                   <tr>
                     <td align="left">
                          <span style="color:Black">Edit Period</span>
                      </td>
                      <td align="right">
                           <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                      </td>                  
                   </table>
            </td>
            <td class="ne">&nbsp;</td>
            </tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
               <asp:HiddenField ID="Hd_PeriodId" runat="server" />
               
               <table width="100%" cellspacing="10">
                <tr>
                 <td style="width:50%;" align="right">
                 
                  Period : 
                 
                 </td>
                 <td>
                 
                     <asp:TextBox ID="Txt_Period" runat="server" Font-Bold="true" class="form-control" MaxLength="9"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                           runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="@#$%^&*;?'/\"
                            TargetControlID="Txt_Period">
                           </ajaxToolkit:FilteredTextBoxExtender>
                 </td>
                 <td>
                 
                 
                 </td>
               </tr>
               <tr>
                 <td style="width:50%;" align="right">
                 
                  From Time
                 
                 </td>
                 <td>
                 
                   <asp:TextBox ID="Txt_From" runat="server" Font-Bold="true" class="form-control"></asp:TextBox>
                   
                 </td>
                 <td>
                  
                  
                  <ajaxToolkit:MaskedEditExtender ID="Txt_TimePup_MaskedEditExtender1" 
                                runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99" 
                                MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_From" />
                            <ajaxToolkit:MaskedEditValidator ID="Txt_TimePup_MaskedEditValidator1" 
                                runat="server" ControlExtender="Txt_TimePup_MaskedEditExtender1" 
                                ControlToValidate="Txt_From" Display="Dynamic" EmptyValueBlurredText="*" 
                                EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                TooltipMessage="Input a time" ValidationGroup="Enter" />
                 
                 </td>
               </tr>
               <tr>
                 <td style="width:50%" align="right">
                  To Time
                 </td>
                 <td>
                 
                   <asp:TextBox ID="Txt_To" runat="server" Font-Bold="true" class="form-control"></asp:TextBox>
                    
                 </td>
                 <td>
                 
                     <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" 
                                runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99" 
                                MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_To" />
                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" 
                                runat="server" ControlExtender="MaskedEditExtender1" 
                                ControlToValidate="Txt_To" Display="Dynamic" EmptyValueBlurredText="*" 
                                EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                TooltipMessage="Input a time" ValidationGroup="Enter" /> 
                 
                 </td>
               </tr>
               <tr>
                 <td colspan="3"  align="center">
                 
                     <asp:Button ID="Btn_Update" runat="server" Text="Update" class="btn btn-info" ValidationGroup="Enter" 
                         onclick="Btn_Update_Click" />
                 
                 </td>
                </tr>

                <tr>
                 <td colspan="3" align="center">
                     <asp:Label ID="Lbl_EditError" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
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
           
            
             </ContentTemplate> 
    </asp:UpdatePanel> 

<div class="clear"></div>
</div>
</asp:Content>
