<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="EditClassFeeSchdule.aspx.cs" Inherits="WinEr.WebForm7"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .style1
        {
            width: 171px;
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
				<td class="n">Edit Fee Schedule</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<asp:Panel ID="Panel1" runat="server" DefaultButton="Btn_UpdateAll" >
				
				<div id="topstrip">
					    <table class="tablelist">
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
				
			<br />
		
                   <table class="tablelist">
                   	 <tr>
                              <td >
                                  Class <span style="color:Red">*</span></td>
                              <td >
                                  <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" AutoPostBack="True" 
                                       onselectedindexchanged="Drp_Class_SelectedIndexChanged" 
                                      Width="160px">
                                  </asp:DropDownList>
                              </td>
                              <td >
                                  &nbsp;</td>
                              <td >
                                  Period<span style="color:Red">*</span></td>
                              <td class="style1">
                                  <asp:DropDownList ID="Drp_Perod1" runat="server" class="form-control" AutoPostBack="True" 
                                       onselectedindexchanged="Drp_Perod1_SelectedIndexChanged" 
                                     Width="160px">
                                  </asp:DropDownList>
                              </td>
                              <td>   <asp:Label ID="Label_NextBatch" runat="server" class="control-label" Text="Batch"></asp:Label></td>
                              <td>
                                   <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdo_Batch_SelectedIndexChanged">
                                      <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                                  </asp:RadioButtonList>
                              </td>
                          </tr>
                        </table>
                    
                     <div class="linestyle"></div> 
                    
                          <table class="tablelist"> 
                           <tr>
                              <td class="leftside">
                                  Due Date<span style="color:Red">*</span>
                                  </td>
                              <td class="rightside">
                                  <asp:TextBox ID="Txt_From" runat="server" Wrap="False" class="form-control" Width="180px"></asp:TextBox> 
                                  <br />                         
                              </td>
                              <asp:RegularExpressionValidator ID="Txt_From_RegularExpressionValidator"  runat="server" ControlToValidate="Txt_From" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                              <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"   MaskType="Date"
                                                           CultureName="en-GB" AutoComplete="true" Mask="99/99/9999" UserDateFormat="DayMonthYear" Enabled="True"  TargetControlID="Txt_From">
                                                           </ajaxToolkit:MaskedEditExtender>    
                                                         
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" 
                                                        runat="server" HighlightCssClass="validatorCalloutHighlight" 
                                                        TargetControlID="Txt_From_RegularExpressionValidator" Enabled="True" />
                                                        <asp:RequiredFieldValidator ID="Txt_From_ReqVal" runat="server" ErrorMessage="Enter Date" ValidationGroup="Show" ControlToValidate="Txt_From"></asp:RequiredFieldValidator>
                        
                        </tr>  
                          
                      <tr>       
                              <td class="leftside">
                                  Last Date<span style="color:Red">*</span></td>
                              <td class="rightside">
                                 <asp:TextBox ID="Txt_To" runat="server" class="form-control" Width="180px"></asp:TextBox>       
                                 <br />                  
                              </td>
                               <asp:RegularExpressionValidator ID="Txt_To_RegularExpressionValidator" 
                                                         runat="server" ControlToValidate="Txt_To" Display="None" ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                                                         
                               <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"   MaskType="Date"  CultureName="en-GB" 
                                                           AutoComplete="true"  Mask="99/99/9999"  UserDateFormat="DayMonthYear"  Enabled="True" TargetControlID="Txt_To">
                                                           </ajaxToolkit:MaskedEditExtender>    
                                                         
                                                       <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" 
                                                        runat="server" HighlightCssClass="validatorCalloutHighlight" 
                                                        TargetControlID="Txt_To_RegularExpressionValidator" Enabled="True" />
                                                        <asp:RequiredFieldValidator ID="Txt_To_ReqFieldVal" runat="server" ErrorMessage="Enter Date" ValidationGroup="Show" ControlToValidate="Txt_To"></asp:RequiredFieldValidator>
                              </tr>     
                            <tr>   
                                 <td class="leftside">
                            Amount
                                 </td>
                              <td class="rightside">
                                      <asp:TextBox ID="Txt_New_Amount" runat="server" class="form-control" Width="180px"> </asp:TextBox>
                                      <ajaxToolkit:FilteredTextBoxExtender ID="Txt_New_Amount_FilteredTextBoxExtender" 
                                      runat="server" Enabled="True" FilterType="Custom, Numbers"  TargetControlID="Txt_New_Amount" ValidChars=".">
                                      </ajaxToolkit:FilteredTextBoxExtender>
                              <br /> 
                              </td>
                             
                              </tr>     
                            <tr> 
                             <td class="leftside">
                                
                              </td>   
                              <td class="rightside">
                                  <asp:Button ID="Btn_applyAll" runat="server" Text="Apply All"  OnClick="Btn_applyAll_Click" Class="btn btn-primary"/>&nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="Btn_UpdateAll" runat="server" Text="Save" ValidationGroup="Show"  OnClick="Btn_UpdateAll_Click" Class="btn btn-success"/>
                              </td>
                          </tr>
                   </table>
                <br />
                     <div class="linestyle"></div> 
                      <asp:Label ID="lbl_Message" runat="server" class="control-label" ></asp:Label>
                     <!-- *****************************************************************************************!-->
                  <asp:Panel ID="Pnl_Students" runat="server" Visible="false">
                    <div style="max-height:400px;overflow:auto;">
                                                            <asp:GridView ID="GridViewAllFee" runat="server" AutoGenerateColumns="False" 
                                                                CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" 
                                                                BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                                                                <AlternatingRowStyle BackColor="White" />
                                                                <Columns>
                                                                   
                                                                    <asp:BoundField DataField="SchId" HeaderText="ScheduledId" />
                                                                    <asp:BoundField DataField="StudId" HeaderText="StudId" />
                                                                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-Width="200px"  ControlStyle-Width="150px"/>
                                                                  
                                                                    <asp:BoundField DataField="ScheduledAmt" HeaderText="Scheduled Amt" ItemStyle-Width="80px"  ControlStyle-Width="100px"/>
                                                                    <asp:BoundField DataField="Paid" HeaderText="Paid Amt" ItemStyle-Width="80px"  ControlStyle-Width="100px"/>
                                                                    <asp:BoundField DataField="Balance" HeaderText="Balance Amt" ItemStyle-Width="80px"  ControlStyle-Width="100px" />
                                                                    <asp:TemplateField HeaderText="New Amt" ItemStyle-Width="80px"  ControlStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="Txt_NewAmt" runat="server"  MaxLength="8"  Text='<%#Eval("ScheduledAmt") %>' class="form-control" Width="75px"></asp:TextBox>
                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewAmt_FilteredTextBoxExtender" 
                                                                                runat="server" Enabled="True" FilterType="Custom, Numbers"  TargetControlID="Txt_NewAmt" ValidChars=".">
                                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                     
                                                                  
                                                                    
                                                                </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                          <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                            HorizontalAlign="Left" />                                                          
                                                          <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                          <EditRowStyle Font-Size="Medium" />     
                                                            </asp:GridView>
                        </div>
                  </asp:Panel>
                  <!--******************************************************************************************** !-->
            
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
            
           
  </ContentTemplate>
 </asp:UpdatePanel> 
</div>

<div class="clear"></div>
</div>

</asp:Content>


