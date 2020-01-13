<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="GenerateBarcode.aspx.cs" Inherits="WinEr.WebForm14" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

<div id="contents">
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px"  style="min-height:350px"; >
			<tr >
				<td class="no"></td>
				<td class="n">Generate Barcode</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
					<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
                    
                    <div style="min-height:200px">
                   
                    <center>
                    <asp:Panel ID="Pnl_search_conditions" runat="server">
                    <br />
                    <table width="100%" cellspacing="5">
                    <tr>
                    <td align="right" style="width:15%">Select Period</td>
                    <td style="width:15%" align="left">
                    <asp:DropDownList ID="Drplist_period" runat="server" Width="150" class="form-control"
                            AutoPostBack="True" 
                            onselectedindexchanged="Drplist_period_SelectedIndexChanged">
                    <asp:ListItem Text="Today" Value="0"></asp:ListItem>
                    <asp:ListItem Text="This Week" Value="1"></asp:ListItem>
                    <asp:ListItem Text="This Month" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Manual" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                     <td align="right" style="width:15%">From Date:</td>
                    <td style="width:15%" align="left">
                        <asp:TextBox ID="Txt_frmdate" runat="server" Width="150" class="form-control"></asp:TextBox>
                           <ajaxToolkit:MaskedEditExtender ID="Txt_SDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_frmdate" CultureAMPMPlaceholder="AM;PM" 
                                              CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                              CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                              CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                                         <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                                ControlToValidate="Txt_frmdate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                                TargetControlID="DobDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                      ControlToValidate="Txt_frmdate" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    <td align="right" style="width:15%">To Date:</td>
                    <td style="width:15%" align="left">
                    <asp:TextBox ID="Txt_todate" runat="server" Width="150" class="form-control"></asp:TextBox>
                     <ajaxToolkit:MaskedEditExtender ID="Txt_EDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_todate" CultureAMPMPlaceholder="AM;PM" 
                                               CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                               CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                               CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                              
                                     <asp:RegularExpressionValidator runat="server" ID="Txt_EDate_RegularExpressionValidator1"
                                                ControlToValidate="Txt_todate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender4"
                                                TargetControlID="Txt_EDate_RegularExpressionValidator1"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                                
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                      ControlToValidate="Txt_todate" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                 
                   <td align="right" style="width:15%">
                       Category
                      </td>
                      <td style="width:15%" align="left">
                       <asp:DropDownList ID="Drp_catagory" runat="server" class="form-control" Width="150px">
                                  </asp:DropDownList>
                      </td>
                       <td align="right" style="width:15%">Search By:</td>
                    <td style="width:15%" align="left">
                    <asp:DropDownList ID="Drp_search" runat="server" Width="150px" class="form-control"
                            AutoPostBack="True" 
                            onselectedindexchanged="Drp_search_SelectedIndexChanged">
                    <asp:ListItem Text="Any" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Book Name" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Publisher" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Author" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                    <td align="right" style="width:15%">
                        <asp:Label ID="lbl_select" runat="server" Text="" ></asp:Label></td>
                    <td style="width:15%" align="left">
                    <asp:TextBox ID="Txt_name" runat="server" Width="150" class="form-control"></asp:TextBox>
                    </td>
                 <%--   </tr>
                    <tr>
                 
                       <td align="right" style="width:15%">
                      Type
                      </td>
                      <td style="width:15%" align="left">
                        <asp:DropDownList ID="Drp_type" runat="server" Width="150px" 
                                     >
                                  </asp:DropDownList>
                      </td>--%>
                   <%--<td id="Publisher" align="right">Enter Publisher Name:</td>
                      <asp:TextBox ID="Txt_pulisher" runat="server"></asp:TextBox>
                    </td>
                      <td align="right">Enetr Author Name:</td>
                    <td align="left">
                     <asp:TextBox ID="Txt_author" runat="server"></asp:TextBox>
                    </td>--%>
                 <%--   </tr>
                    <tr>--%>
                 
                    </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    </table>
                    <br />
                    <table width="100%">
                    
                    <tr>
                     <td align="center"><asp:Button ID="Btn_Show" runat="server" 
                    Text="SHOW"  Class="btn btn-primary" onclick="Btn_Show_Click" />
                  <%--  <asp:Button ID="Btn_ShowExcelReport" runat="server" Width="75px" Text="EXPORT" 
                            CssClass="grayexcel" onclick="Btn_ShowExcelReport_Click" />--%></td> 
                    
                    </tr>
                   
                    </table>  
                    <table width="100%"><tr><td align="center"><asp:Label ID="Lbl_err" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
                    </asp:Panel>
                    </center>
                    <asp:Panel ID="Pnl_Showfound" runat="server">                    
                      <div class="linestyle"></div>
                    <table width="100%">
                    <tr>
                    <td align="center" style="width:50%">
                    <asp:Label ID="lbl_bookfound"  runat="server" Text="No of Books Found:" Font-Size="Medium"></asp:Label>
                   <asp:Label ID="Lbl_count" runat="server" Text="" Font-Size="Medium" ForeColor="Green"></asp:Label>
                      </td>
                       <td style="width:50%" align="left">
                           <asp:Button ID="Btn_Generate" runat="server" Text="Generate Barcodes" class="btn btn-primary" onclick="Btn_Generate_Click" />
                      </td>
                    </tr>
                    </asp:Panel>
                    </div>
                    
                     </ContentTemplate>
                   <%--  <Triggers >
                         <asp:PostBackTrigger ControlID="Btn_ShowExcelReport"/>
                     </Triggers>--%>
                    </asp:UpdatePanel>
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

<div class="clear"></div>
</div>
</asp:Content>
