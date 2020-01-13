<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearTransactions.aspx.cs" Inherits="WinEr.ClearTransactions"  MasterPageFile="~/WinErStudentMaster.master"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
    function cancel() {
    }
</script>
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
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <img alt="" src="Pics/accept.png" width="30" height="30" /> </td>
				<td class="n">Bill Clearence</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"> </td>
				<td class="c" >
					
					 <ajaxToolkit:TabContainer runat="server" ID="Tabs" 
                         CssClass="ajax__tab_yuitabview-theme" Width="100%" >
                          <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                                <asp:Image ID="Image3" runat="server" Height="18px" ImageUrl="Pics/folder11.png" Width="20px" /><b>PENDING BILLS</b>
                          </HeaderTemplate>
                        <ContentTemplate>
                        <asp:UpdatePanel ID="updtTab1" runat="server" >
                        <ContentTemplate>
                        <div style="min-height:300px">
					        <asp:Panel ID="Pnl_Transaction" runat="server">
					    <br />
					    <table width="100%">
					        <tr>
					            <td colspan="2" align="center">
                                    <asp:RadioButtonList ID="Rdb_StudentType_needClerance" runat="server" AutoPostBack="true" 
                                        RepeatDirection="Horizontal" 
                                        onselectedindexchanged="Rdb_StudentType_needClerance_SelectedIndexChanged">
                                    <asp:ListItem Text="Regular Student" Value="0" Selected="True"></asp:ListItem>
                                     <asp:ListItem Text="Registered Student" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
					            </td>
					         
					         
					         </tr> 
					         <tr>
					         
					        
					            <td style="width:50%" align="right">
					                Select Class : 
					            </td>
					            <td>
                                    <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="160px" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="Drp_Class_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </td>
                             </tr>
                             <tr>
                                <td align="right">
                                   
                                    <asp:Label ID="lbl_selectstudent" runat="server" class="control-label" Text="Select Student : "></asp:Label>
                                    
                                </td>
					            <td>
                                    <asp:DropDownList ID="Drp_Student" runat="server" Width="160px" class="form-control" 
                                        AutoPostBack="True" onselectedindexchanged="Drp_Student_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                             </tr>
                             
                             <tr>
                             <td>
                             </td>
                             <td> 
                                 <asp:Button ID="Btn_Search" runat="server" Text="Pending transactions" class="btn btn-info"
                                     onclick="Btn_Search_Click"  Visible="False"/>
                             </td>
                            </tr>
                            <tr>
                             <td colspan="2" align="center"></td>
					        </tr>
					    </table>
                                <asp:Label ID="Lbl_ClearanceMessage" runat="server" class="control-label" ForeColor="Red"></asp:Label>
					    
					    
					    <asp:Panel ID="Pnl_pending" runat="server">
					               <div style=" overflow:auto;height: 373px;">
					     <table width="100%">
					         <tr>
					            <td align="left">
                                     <asp:LinkButton ID="Lnk_select" runat="server" OnClick="Lnk_select_Click">All</asp:LinkButton>  
                                </td>
                                <td align="right" style="padding-right:25px"> 
                                                  <asp:ImageButton ID="Btn_ClearBill" runat="server"  onclick="Btn_ClearBill_Click" Width="35px" ImageUrl="~/images/accept.png" AlternateText="Clear bill"/>
                                                   &nbsp;&nbsp;&nbsp;
                                                  <asp:ImageButton ID="Img_Cancel" runat="server"  onclick="Img_Cancel_Click" Width="35px" ImageUrl="~/Pics/block.png" AlternateText="Cancel bill"/>
                                 <asp:Button ID="Img_Cancel1" runat="server" Visible="false" />
                                                  <ajaxToolkit:ConfirmButtonExtender ID="Img_Cancel_ConfirmButtonExtender"  
                                                      runat="server" ConfirmText="Are you sure you want to cancel the transaction?" Enabled="True" TargetControlID="Img_Cancel1" OnClientCancel="cancel">
                                                  </ajaxToolkit:ConfirmButtonExtender>
                                 
                               </td>
                            </tr>
                        </table>
                  <asp:GridView ID="Grd_Pending" runat="server" CellPadding="4" 
                      ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                      Width="97%" 
                    
                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                      BorderWidth="1px" 
                       >
                        <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox" runat="server" 
                                     />
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
                <asp:BoundField DataField="StudentName" HeaderText="Name" />
                <asp:BoundField DataField="PaymentMode" HeaderText="PaymentMode" />
                <asp:BoundField DataField="PaymentModeId" HeaderText="Cheque/DD No" />
                <asp:BoundField DataField="BankName" HeaderText="Bank" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="date" DataFormatString="{0:d}" />
               
                             
                    </Columns>  
                   
                      <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                             <RowStyle BackColor="White" BorderColor="Olive" VerticalAlign="Top"  Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                  </asp:GridView>
                  </div>
					    </asp:Panel>
					</asp:Panel>
					    </div>
					  
	<asp:Button ID="btn_delconfirm" runat="server" class="btn btn-info" style="display:none" />
	<ajaxToolkit:ModalPopupExtender ID="MPE_confirmDelete" runat="server" TargetControlID="btn_delconfirm" PopupControlID="PNL"  BackgroundCssClass="modalBackground" CancelControlID="ButtonNo" />
                  <asp:Panel ID="PNL" runat="server" style="display:none;">
                        <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:large">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c"> 
                        Are you sure to Continue ?
                        <br /><br />
                        <div style="text-align:right;">
                            <asp:Button ID="ButtonYes" runat="server" Text="Yes" OnClick="btn_Cnfirm_click" class="btn btn-info" Width="50px" />
                            <asp:Button ID="ButtonNo" runat="server" Text="No" class="btn btn-info" Width="50px"/>
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
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="Img_Cancel" />
        <asp:PostBackTrigger ControlID="Btn_ClearBill" />
        </Triggers>
                    </asp:UpdatePanel>
					    </ContentTemplate>
					    </ajaxToolkit:TabPanel>
					    
					    <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                                <asp:Image ID="Image1" runat="server" Height="18px" ImageUrl="Pics/folder_accept.png" Width="20px" /><b>CLEARED BILLS</b>
                          </HeaderTemplate>
                        <ContentTemplate>
                         <div style="min-height:300px">
					        <asp:Panel ID="Pnl_ClearedTransactions" runat="server">
					     <br />
					    <table width="100%">
					         <tr>
					            <td colspan="2" align="center">
                                    <asp:RadioButtonList ID="RDB_StudTypeCleranceDone" runat="server" AutoPostBack="True" 
                                        RepeatDirection="Horizontal" 
                                        onselectedindexchanged="RDB_StudTypeCleranceDone_SelectedIndexChanged" >
                                    <asp:ListItem Text="Regular Student" Value="0" Selected="True"></asp:ListItem>
                                     <asp:ListItem Text="Registered Student" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
					            </td>
					         
					         
					         </tr>
					        <tr>
					            <td style="width:50%" align="right">
					                Select Class : 
					            </td>
					            <td>
                                    <asp:DropDownList ID="Drp_Class1" runat="server" Width="160px" class="form-control"
                                        AutoPostBack="True" 
                                        onselectedindexchanged="Drp_Class1_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </td>
                             </tr>
                             <tr>
                                <td align="right">
                                 <asp:Label ID="lbl_clearedStudent" runat="server" class="control-label" Text="Select Student : "></asp:Label>
                                </td>
					            <td>
                                    <asp:DropDownList ID="Drp_Name1" runat="server" Width="160px" class="form-control"
                                        AutoPostBack="True" onselectedindexchanged="Drp_Name1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                             </tr>
                             <tr>
                                <td align="center" colspan="2">
                                    <asp:RadioButtonList ID="Rdo_Options" runat="server"  Visible="False"
                                        RepeatDirection="Horizontal" AutoPostBack="True" 
                                        onselectedindexchanged="Rdo_Options_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                                        <asp:ListItem Value="1">This Week</asp:ListItem>
                                        <asp:ListItem Value="2">This Month</asp:ListItem>
                                        <asp:ListItem Value="3">Manual</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                             </tr>
                             <tr>
                                <td  align="center" colspan="2">
                                    <asp:TextBox ID="Txt_Startdate" runat="server" class="form-control" Visible="False"></asp:TextBox>
                               
                                    <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Startdate_TextBoxWatermarkExtender"  WatermarkText="Enter Startdate"
                                        runat="server" Enabled="True" TargetControlID="Txt_Startdate">
                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                      <ajaxToolkit:CalendarExtender ID="Txt_StartDate_CalendarExtender"  CssClass="cal_Theme1"
                                    runat="server" Enabled="True" TargetControlID="Txt_Startdate" Format="dd/MM/yyyy" >
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator runat="server" ID="Txt_StartdateRegularExpressionValidator"
                                    ControlToValidate="Txt_Startdate"
                                    Display="None" 
                                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                 <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="Exam_ValidatorCalloutExtender"
                                    TargetControlID="Txt_StartdateRegularExpressionValidator"
                                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />  
                                
                                
                                </td>
                                        
                                
                             </tr>
                              <tr>
                                 <td align="center" colspan="2">
                                     <asp:TextBox ID="Txt_EndDate" runat="server" class="form-control" Visible="False"></asp:TextBox>
                                      <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_EndDate_TextBoxWatermarkExtender" WatermarkText="Enter Enddate"
                                         runat="server" Enabled="True" TargetControlID="Txt_EndDate">
                                     </ajaxToolkit:TextBoxWatermarkExtender>
                                
                                      <ajaxToolkit:CalendarExtender ID="Txt_EndDate_CalendarExtender" Format="dd/MM/yyyy"  CssClass="cal_Theme1"
                                        runat="server" Enabled="True" TargetControlID="Txt_EndDate">
                                    </ajaxToolkit:CalendarExtender>
                                     <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                    ControlToValidate="Txt_EndDate"
                                    Display="None" 
                                   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                 <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                    TargetControlID="RegularExpressionValidator1"
                                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />   
                                 
                                 </td>
                                      
                             </tr>
                             <tr>
                             <td  colspan="2" align="center"> 
                                 <asp:Button ID="Btn_View" runat="server" Text="View" class="btn btn-info" Visible="False"
                                     onclick="Btn_View_Click" /></td>
					        </tr>
					        <tr>
					        <td  colspan="2" align="center">
                                <asp:Label ID="Lbl_viewMessage" runat="server" class="control-label" ForeColor="Red"></asp:Label>
					        </td>
					        </tr>
					    </table>
					    <asp:Panel ID="Pnl_View" runat="server">
					               <div style=" overflow:auto;min-height: 350px;">
					    <asp:GridView ID="GrdView" runat="server" CellPadding="4" 
                      ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                      Width="97%" OnSelectedIndexChanged="GrdView_SelectedIndexChanged"
                    
                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                      BorderWidth="1px" >
                        <Columns>
                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                 <asp:BoundField DataField="StudentName" HeaderText="Name" />
                <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
                <asp:BoundField DataField="PaymentMode" HeaderText="PaymentMode" />
                <asp:BoundField DataField="PaymentModeId" HeaderText="Check/DD No" />
                <asp:BoundField DataField="BankName" HeaderText="Bank" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="date" DataFormatString="{0:d}" />
                <asp:CommandField SelectText="&lt;img src='pics/Details.png' width='30px' border=0 title='Select bill to view'&gt;" 
                  ShowSelectButton="True" HeaderText="BILL" />
                             
                    </Columns>  
                   
                     <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                                ForeColor="Black" HorizontalAlign="Left" />
                             <RowStyle BackColor="White" BorderColor="Olive" VerticalAlign="Top"  Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                  </asp:GridView>
                                  </div>
                    </asp:Panel>
					        </asp:Panel>
					        </div>
					    </ContentTemplate>
					    </ajaxToolkit:TabPanel>
					    
					</ajaxToolkit:TabContainer>
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
	
	
	
	<WC:MSGBOX id="WC_MessageBox" runat="server" />        

        </ContentTemplate>
        
                    </asp:UpdatePanel>

<div class="clear"></div>
   
                    
</div>
</asp:Content>
