<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudAttdConfig.aspx.cs" Inherits="WinEr.StudAttdConfig" %>
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
   <td class="no"> </td>
   <td class="n">Student Attendance Configuration</td>
   <td class="ne"> </td>
  </tr>
  <tr >
	<td class="o"> </td>
	<td class="c" >
                   
	 <div style="min-height:200px;">
	 
	 <table width="100%"  cellspacing="10px">

	        <tr>
	         <td align="right">
	         
	          StudentId Lower Limit : 
	         
	         </td>
	         <td align="left">
	          <asp:TextBox ID="Txt_stdid_lower" runat="server" Width="150px" MaxLength="10" class="form-control"></asp:TextBox>
	            <ajaxToolkit:FilteredTextBoxExtender ID="IncedentFilteredTextBoxExtender1"
                       runat="server"   TargetControlID="Txt_stdid_lower" FilterType="Numbers"  />     
	         </td>
	        </tr>
	         <tr>
	         <td align="right">
	         
	          StudentId Upper Limit : 
	         
	         </td>
	         <td align="left">
	          <asp:TextBox ID="Txt_stdid_upper" runat="server" Width="150px" class="form-control" MaxLength="10"></asp:TextBox>
	           <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                       runat="server"   TargetControlID="Txt_stdid_upper" FilterType="Numbers"  />     
	         </td>
	        </tr>
	        <tr>
	         <td colspan="2"  align="center">
	           <asp:Label ID="Label3" runat="server" class="control-label" Text="Need different equipment for In and Out time"></asp:Label>
                 <asp:RadioButtonList ID="Rdb_needDiffEquipment" runat="server" 
                    RepeatDirection="Horizontal">
                 <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                 <asp:ListItem Text="No" Value="0"  Selected="true"></asp:ListItem>
                </asp:RadioButtonList>
	         
	         </td>
	        </tr>
	       	   <tr>
	                <td colspan="2" align="center">
            	    
                        <asp:Label ID="lbl_Errormsg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
            	    
	                </td>
	                </tr>
	                <tr>
	                 <td colspan="2" align="center">
                        <asp:Button ID="Btn_Save" runat="server" Text="Save" class="btn btn-success" ValidationGroup="Update" 
                            onclick="Btn_Save_Click" />
                        
                         &nbsp;
                        
                         <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger" 
                            onclick="Btn_Cancel_Click" />
	                </td>
	               </tr>
	      </table>
	 
	  <br />
	  <table width="100%" cellspacing="5" style="border:solid 2px gray;background-color:#E9ECFA">
	   <tr>
	    <td  style="width:50%" align="right">
	     Select Group : 
	    </td>
	    <td  style="width:25%" align="left">
	     <asp:DropDownList ID="Drp_ParentGroup" runat="server" Width="200px" class="form-control"
                     AutoPostBack="true" 
                     onselectedindexchanged="Drp_ParentGroup_SelectedIndexChanged">
                 </asp:DropDownList>
	    </td>
	    <td style="width:25%" align="right">
	    
	           <asp:Button ID="Btn_SaveGroupConfigs" runat="server" Text="Save" class="btn btn-primary" ValidationGroup="Update" 
                            onclick="Btn_SaveGroupConfigs_Click" />
	     
	    
	    </td>
	   </tr>
	   
	   <tr>
	    <td style="width:50%">
	      <table width="100%" cellspacing="10px">
	        <tr>
	         <td style="width:50%;" align="right">
	           Start InTime : 
	         </td>
    	     
	         <td style="width:50%;">
                 <asp:TextBox ID="Txt_StartIntime" runat="server" Width="110px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:MaskedEditExtender ID="Txt_TimePup_MaskedEditExtender1" 
                       runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                       MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                       OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_StartIntime" />
                  <ajaxToolkit:MaskedEditValidator ID="Txt_TimePup_MaskedEditValidator1" 
                       runat="server" ControlExtender="Txt_TimePup_MaskedEditExtender1" 
                       ControlToValidate="Txt_StartIntime" Display="Dynamic" EmptyValueBlurredText="*" 
                       EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                       InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                       TooltipMessage="*" ValidationGroup="Update" />
                  </ItemTemplate>
	         </td>
    	   
	        </tr>
	        <tr> 
	         <td align="right">
	            Late InTime : 
	         </td>
    	     
	         <td >
	           <asp:TextBox ID="Txt_LateInTime" runat="server" Width="110px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" 
                       runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                       MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                       OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_LateInTime" />
                  <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" 
                       runat="server" ControlExtender="MaskedEditExtender1" 
                       ControlToValidate="Txt_LateInTime" Display="Dynamic" EmptyValueBlurredText="*" 
                       EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                       InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                       TooltipMessage="*" ValidationGroup="Update" />
                  </ItemTemplate>
	         </td>
	        </tr>
	        <tr> 
	         <td align="right">
	             End InTime : 
	         </td>
    	     
	         <td>
	             <asp:TextBox ID="Txt_EndInTime" runat="server" Width="110px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" 
                       runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                       MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                       OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_EndInTime" />
                  <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" 
                       runat="server" ControlExtender="MaskedEditExtender2" 
                       ControlToValidate="Txt_EndInTime" Display="Dynamic" EmptyValueBlurredText="*" 
                       EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                       InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                       TooltipMessage="*" ValidationGroup="Update" />
                  </ItemTemplate>
	         </td>
	        </tr>
	        
	       </table>
	    </td>
	    <td style="width:50%" valign="top" colspan="2">
	    
	      <table width="100%"  cellspacing="10px">
	       <tr>
	        <td colspan="2"  align="center">
	         
                <asp:Label ID="Label1" runat="server" Text="Need OutTime" class="control-label"></asp:Label>
	         
	         
                <asp:RadioButtonList ID="Rdb_needOOutTime" runat="server" 
                    RepeatDirection="Horizontal" AutoPostBack="true" 
                    onselectedindexchanged="Rdb_needOOutTime_SelectedIndexChanged">
                 <asp:ListItem Text="Yes" Value="1" Enabled="true"></asp:ListItem>
                 <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
	         
	        </td>
	       </tr>
	       	        <tr>
	         <td style="width:50%;" align="right">
	           Start OutTime : 
	         </td>
    	     
	         <td style="width:50%;">
                 <asp:TextBox ID="Txt_StartOutTime" runat="server" Width="110px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" 
                       runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                       MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                       OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_StartOutTime" />
                  <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator3" 
                       runat="server" ControlExtender="MaskedEditExtender3" 
                       ControlToValidate="Txt_StartOutTime" Display="Dynamic" EmptyValueBlurredText="*" 
                       EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                       InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                       TooltipMessage="*" ValidationGroup="Update" />
                  </ItemTemplate>
	         </td>
    	   
	        </tr>
	        <tr> 
	         <td align="right">
	            End OutTime : 
	         </td>
    	     
	         <td >
	           <asp:TextBox ID="Txt_EndOutTime" runat="server" Width="110px" class="form-control"></asp:TextBox>
                  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" 
                       runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                       MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                       OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_EndOutTime" />
                  <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator4" 
                       runat="server" ControlExtender="MaskedEditExtender4" 
                       ControlToValidate="Txt_EndOutTime" Display="Dynamic" EmptyValueBlurredText="*" 
                       EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                       InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                       TooltipMessage="*" ValidationGroup="Update" />
                  </ItemTemplate>
	         </td>
	        </tr>
	      </table>
	    
	    </td>

	   </tr>

	   <tr>
	                <td colspan="3" align="center">
            	    
                        <asp:Label ID="Lbl_ErrorMasgForGroupDetails" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
            	    
	                </td>
       </tr>

	  </table>
	  
	   
	  
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
       
   
           <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none" class="btn btn-info"/>
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
                    
             </ContentTemplate> 
    </asp:UpdatePanel> 

<div class="clear"></div>
</div>
</asp:Content>
