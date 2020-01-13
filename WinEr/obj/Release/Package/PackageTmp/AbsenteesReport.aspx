<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AbsenteesReport.aspx.cs" Inherits="WinEr.AbsenteesReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
              
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_AbsenteesReport.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
    </script>
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

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
<div id="contents">
<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Absentees Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
	<asp:Panel ID="Pnl_LateComerReport" runat="server">
				<table width="100%" cellspacing="10">
				<tr>
				<td align="right">Select Class</td>
				<td align="left">
				<asp:DropDownList ID="Drp_Classname" runat="server" Width="170px" class="form-control"></asp:DropDownList>
				</td>
				<td align="right">From Date</td>
				 <td><asp:TextBox ID="Txt_FromDate" runat="server" Width="170px" class="form-control"></asp:TextBox>
				  <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_FromDate" Format="dd/MM/yyyy">
                           </cc1:CalendarExtender>  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_FromDate"
                               ValidationGroup="show"  ErrorMessage="*"></asp:RequiredFieldValidator> 
                            <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                               runat="server" ControlToValidate="Txt_FromDate" Display="None" 
                               ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
				 </td>
				</tr>
				<tr>
				<td align="right">Select Period</td>
				<td align="left">
				<asp:DropDownList ID="Drp_Period" runat="server" Width="170px" class="form-control" onselectedindexchanged="Drp_Period_SelectedIndexChanged"
                       
                        AutoPostBack="True">
				<asp:ListItem Text="Today">
				</asp:ListItem>
				<asp:ListItem Text="Last Week">
				</asp:ListItem>
				<asp:ListItem Text="This Month">
				</asp:ListItem>
				<asp:ListItem Text="Manual">
				</asp:ListItem>
				</asp:DropDownList>
				</td>
				
				 <td align="right">End Date</td>
				 <td align="left">
				 <asp:TextBox ID="Txt_ToDate" runat="server" class="form-control" Width="170px" ></asp:TextBox>
				  <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ToDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_ToDate"
                               ValidationGroup="show"  ErrorMessage="*"></asp:RequiredFieldValidator> 
                                <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                                     runat="server" ControlToValidate="Txt_ToDate" Display="None" 
                                     ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                     ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                TargetControlID="Txt_EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
				
				 </td>
				</tr>
				
				<tr>
				<td></td>
				<td><asp:Button ID="Btn_Show" runat="server" Text="Show"  Class="btn btn-primary"  ValidationGroup="show"
				onclick="Btn_Show_Click"/><%--    ontextchanged=" Txt_ToDate_TextChanged"--%>
                        &nbsp;
                        <asp:Button ID="Btn_Excel" runat="server"   Text="Export" Class="btn btn-primary" 
                         onclick="Btn_Excel_Click" /></td>
			    <td></td>
				<td>
				</td>
				
				
				</tr>
				<tr><td align="center" colspan="4"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label></td></tr>
				</table>
				</asp:Panel> 
			  <asp:Panel runat="server" ID="Pnl_SmStext" Visible ="false">
                          <div class="linestyle"></div>
                        <table class="tablelist">
                            <tr>
                                <td valign="middle" class="leftside">&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="middle" class="leftside"><span style="font-size:medium;">SMS Text</span></td>
                                <td class="leftside"
                                    style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:TextBox ID="Txt_SmsText" runat="server" Width="500px" Height="80px" class="form-control" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                                <td rowspan="2" valign="top">
   	                       <div style="height:150px;overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Representations of keywords"></asp:Label>
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
                            </tr>
                            <tr>
                                <td></td>
                                  <td></td>
                                <td align="center"  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                 
                                    <asp:Button ID="Btn_Send" runat="server"  Width="111px" Class="btn btn-primary" Text="Send SMS"  onclick="Btn_Send_Click" />
                                </td>
                                <ajaxToolkit:ConfirmButtonExtender ID="Btn_Send_ConfirmButtonExtender" runat="server" TargetControlID="Btn_Send" ConfirmText="Are you sure you want to send the SMS"></ajaxToolkit:ConfirmButtonExtender>
                                
                                <td></td>
                            </tr>
                        </table>
                           <div class="linestyle"></div>
                     </asp:Panel>
      
				<asp:Panel ID="Pnl_ShowReport" runat="server">				           
				<table width="100%">
				<tr>
				<td>
				<asp:GridView ID="Grd_AbsenteesReport" runat="server"    
				 CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" PageSize="10" AllowPaging="True" 
                            OnSelectedIndexChanged="Grd_AbsenteesReport_SelectedIndexChanged"
                            OnPageIndexChanging="Grd_AbsenteesReport_PageIndexChanging">
				            <RowStyle BackColor="White" />
				           <%--  OnPageIndexChanging="Grd_LateComerReport_PageIndexChanging"--%>
				            <Columns>
				             <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server" Checked="true" />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                            
                         </asp:TemplateField>
                                 <asp:BoundField DataField="StudentName" HeaderText="Student Name" /> 
                                 <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                                 <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                 <asp:BoundField DataField="Date" HeaderText="Date" />
                               <asp:CommandField SelectText="&lt;img src='pics/SMS.png' width='30px' border=0 title='Click to get password'&gt;" 
                                   ShowSelectButton="True" HeaderText="SMS"  ItemStyle-Width="40"/>
                                     <asp:BoundField DataField="studid" HeaderText="stud id" />
                                 
                            </Columns>
				            <FooterStyle BackColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                          
                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
				</asp:GridView>
				</td> 
				</tr>
				</table>
				 </asp:Panel>
  <asp:Panel ID="pnl_Demo" runat="server">
                <asp:Button ID="Button1" runat="server" class="btn btn-info" Style="display: none;"/>
            <ajaxToolkit:ModalPopupExtender ID="MPE_SMSPOPUP" runat="server" 
                PopupControlID="Panel3" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Panel3" runat="server" Style="display: none;"><%--Style="display: none;"--%>
                <div class="container skin5" style="width: 700px;">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/SMS.png" Height="28px"
                                    Width="29px" />
                            </td>
                            <td class="n"> <span style="color: White">SMS</span></td>
                            <td class="ne">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="o">
                            </td>
                            <td class="c">
                            
                               <asp:Panel ID="Pnl_Sms" runat="server" Visible="true">
            
                   <table width="80%">
                            <tr>
                                <td valign="middle" class="leftside">&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="middle" class="leftside"><span style="font-size:medium;">Text</span></td>
                                <td class="leftside"
                                    style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:TextBox ID="Txt_InSmStext" runat="server" Width="350px" class="form-control" Height="80px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                                <td rowspan="2" valign="top">
   	                       <div style="height:150px; width:200px; overflow:auto">
   	                       <center>
                                  <asp:Label ID="Lbl_Smsmsg" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Keywords"></asp:Label>
                                  <asp:HiddenField ID="Hdn_StudentId" runat="server" />
                                  <asp:HiddenField ID="Hdn_Studname" runat="server" />
                                  <asp:HiddenField ID="Hdn_tdate" runat="server" />
   	                        <div id="Div_ind" runat="server">
                                   
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
                            </tr>                           
                            <tr>
                            
                                <td></td>
                                <td></td>
                                <td align="center"  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                 
                                    <asp:Button ID="Btn_IndSMS" runat="server"   Class="btn btn-primary"  Text="Send SMS"  OnClick="Btn_IndSMS_Click" />&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Btn_magok" runat="server" Text="Cancel"   Class="btn btn-primary"  OnClick="Btn_magok_Click" />
                                </td>
                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Btn_IndSMS" ConfirmText="Are you sure you want to send the SMS" ></ajaxToolkit:ConfirmButtonExtender>
                                
                                <td></td>
                            </tr>
                        </table> 
                </asp:Panel>
                                <div style="text-align: center;">
                                    <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
                                   
                                </div>
                            </td>
                            <td class="e">
                            </td>
                        </tr>
                        <tr>
                            <td class="so">
                            </td>
                            <td class="s">
                            </td>
                            <td class="se">
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </asp:Panel>
                </asp:Panel>
                 <WC:MSGBOX id="WC_MessageBox" runat="server" />    
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
 </ContentTemplate>
 <Triggers>
 <asp:PostBackTrigger ControlID="Btn_Excel" />
 </Triggers>
                    </asp:UpdatePanel>
</asp:Content>
