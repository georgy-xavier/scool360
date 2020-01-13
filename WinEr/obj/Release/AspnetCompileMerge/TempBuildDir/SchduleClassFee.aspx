<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="SchduleClassFee.aspx.cs" Inherits="WinEr.WebForm4" Title="Schedule Fee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style7
        {
        }
        .style10
        {
        }
        .style11
        {
            width: 152px;
        }
        .style12
        {
            width: 193px;
        }
        .style13
        {
        }
        .style14
        {
            width: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<div id="right">

<div class="label">Fee Manager</div>
<div id="SubFeeMenu" runat="server">
		
 </div>
</div>

<div id="left">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>               


<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Schedule Fee</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
					<asp:Panel ID="Panel" runat="server">
					<div id="topstrip">
					    <table class="style1">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Fee"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" class="control-label" Text="Frequency"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Yearly"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" class="control-label" ForeColor="White" 
                                        Text="Associated to"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Student"></asp:Label>
                                </td>
                            </tr>
                        </table>
					<br/>
					</div>
                    
                  <asp:Panel ID="Pnl_Details" runat="server" DefaultButton="Btn_Schdule">
                  
                      <table class="style1">
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  <asp:LinkButton ID="Lnk_btn_select" runat="server" 
                                      onclick="Lnk_btn_select_Click" Text="Select All"></asp:LinkButton>
                              </td>
                              <td>
                                 <asp:Label ID="Label_NextBatch" runat="server" class="control-label" Text="Batch"></asp:Label></td>
                              <td>
                                  <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdo_Batch_SelectedIndexChanged">
                                      <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                                  </asp:RadioButtonList>
                                 </td>
                          </tr>
                          <tr>
                              <td>
                                  Class Name<span style="color:Red">*</span></td>
                              <td rowspan="5">
                                  <div style="OVERFLOW: scroll; WIDTH: 240px; HEIGHT: 203px; BACKGROUND-COLOR: #EBEBEB">
                                      <asp:CheckBoxList ID="ChkBox_Class" runat="server" Font-Bold="False" 
                                          Font-Size="Small" ForeColor="Black" Width="200px">
                                      </asp:CheckBoxList>
                                  </div>
                              </td>
                              <td>
                                  Period<span style="color:Red">*</span></td>
                              <td>
                                  <asp:DropDownList ID="Drp_Perod1" runat="server" class="form-control" AutoPostBack="True" 
                                       onselectedindexchanged="Drp_Perod1_SelectedIndexChanged" 
                                      Width="180px">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  Due Date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_dudate" runat="server" Wrap="False" class="form-control" Width="180px"></asp:TextBox>
                                  <ajaxToolkit:CalendarExtender ID="Txt_dudate_CalendarExtender1" runat="server" 
                                      CssClass="cal_Theme1" TargetControlID="Txt_dudate" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  Last Date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_lastdate" runat="server" class="form-control" Width="180px"></asp:TextBox>
                                  <ajaxToolkit:CalendarExtender ID="Txt_lastdate_CalendarExtender1" 
                                      runat="server" CssClass="cal_Theme1" TargetControlID="Txt_lastdate" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  Amount<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_amount" runat="server" MaxLength="15" class="form-control" Width="180px"></asp:TextBox>
                                  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_amount_FilteredTextBoxExtender" 
                                      runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                      TargetControlID="Txt_amount" ValidChars=".">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td colspan="2">
                               
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td colspan="2">
                                  <asp:Button ID="Btn_Schdule" runat="server"  
                                      onclick="Btn_Schdule_Click" Text="Schedule" Class="btn btn-success" />
                                  &nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                                       Text="Cancel" Class="btn btn-danger" />
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                      </table>
                      
                     <%--<asp:RegularExpressionValidator ID="Txt_dudateDateRegularExpressionValidator3" 
                                      runat="server" ControlToValidate="Txt_dudate" Display="None" 
                                      ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                      ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$" />--%>
                                      <asp:RegularExpressionValidator ID="Txt_dudateDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_dudate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_dudateDateRegularExpressionValidator3" />
                                  <%--<asp:RegularExpressionValidator ID="Txt_lastdateDateRegularExpressionValidator3" 
                                      runat="server" ControlToValidate="Txt_lastdate" Display="None" 
                                      ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                      ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$" />--%>
                                      <asp:RegularExpressionValidator ID="Txt_lastdateDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_lastdate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_lastdateDateRegularExpressionValidator3" />
                  <br/>
                  </asp:Panel>
                  <br />
                  
                  <br />
            
              </asp:Panel>
					
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

      <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-success" Text="OK" Width="50px"/>
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
              
      
      
      
      
      
      
      
      <asp:Panel ID="Panel1_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn1_Message" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_FeeMessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg1" TargetControlID="Btn1_Message"  />
                          <asp:Panel ID="Pnl_msg1" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Label1_FeeMessage" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button2" runat="server" class="btn btn-success" Text="OK" Width="50px"/>
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
      
      
      
      
            
            
  </ContentTemplate>
 </asp:UpdatePanel> 
</div>

<div class="clear"></div>
</div>

</asp:Content>

