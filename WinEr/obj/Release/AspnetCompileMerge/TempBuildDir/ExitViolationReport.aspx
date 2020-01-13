<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ExitViolationReport.aspx.cs" Inherits="WinEr.ExitViolationReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
<div id="contents">
<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Exit Violation Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<asp:Panel ID="Pnl_ExitViolation" runat="server">
				<table width="100%" cellspacing="10">
				<tr>
				
				<td align="right">Select Type</td>
				
				<td align="left"> <asp:RadioButtonList ID="RbLst_Type" runat="server" 
                        RepeatDirection="Horizontal" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="RbLst_Type_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Staff">Student</asp:ListItem>
                        <asp:ListItem Value="Student">Staff</asp:ListItem>
                    </asp:RadioButtonList></td>
                    <td></td>
				<td></td>
				
				</tr>
				 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				<tr>
				<td align="right">Class Name</td>
				<td align="left"><asp:DropDownList ID="Drp_ClassName" runat="server" Width="170px" class="form-control"></asp:DropDownList></td>
				<td align="right">Start date</td>
				<td><asp:TextBox ID="Txt_FromDate" runat="server" class="form-control" Width="170px"></asp:TextBox>
				  <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_FromDate" Format="dd/MM/yyyy">
                           </cc1:CalendarExtender>  
                            <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                               runat="server" ControlToValidate="Txt_FromDate" Display="None" 
                               ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" /></td>
				
				</tr>
				 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				<tr>
				<td align="right">Select Period</td>
			    <td align="left"><asp:DropDownList ID="Drp_Period" runat="server" Width="170px" class="form-control"
                        AutoPostBack="True" onselectedindexchanged="Drp_Period_SelectedIndexChanged">
			    
				<asp:ListItem Text="Last Day">
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
			    <td align="left"><asp:TextBox ID="Txt_ToDate" runat="server" class="form-control" Width="170px"
                        ontextchanged="Txt_ToDate_TextChanged"></asp:TextBox>
			     <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ToDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
                                <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                                     runat="server" ControlToValidate="Txt_ToDate" Display="None" 
                                     ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                     ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                TargetControlID="Txt_EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" /></td>
				</tr>
				
				<tr><td></td>
				<td align="right"><asp:Button ID="Btn_Show" runat="server" Text="Show"  class="btn btn-primary" 
                        onclick="Btn_Show_Click" />
				<asp:Button ID="Btn_Excel"  runat="server" Text="Export" class="btn btn-primary"
                        onclick="Btn_Excel_Click" /></td>
				<td></td>
				<td></td></tr>
				<tr><td colspan="4" align="center"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label></td>
				</tr><tr>
				<td colspan="4" align="center"><asp:Label ID="Lbl_report_err" runat="server" ForeColor="Red" class="control-label"></asp:Label></td></tr>
				</table>
				</asp:Panel>
				
				<asp:Panel ID="Pnl_ShowReport" runat="server">
				<table width="100%">
				<tr>
				<td>
				<asp:GridView ID="Grd_ExitViolationReport" runat="server"    
				 CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" PageSize="10" AllowPaging="True"  OnPageIndexChanging="Grd_ExitViolationReport_PageIndexChanging">
				            <RowStyle BackColor="White" />
				           <%-- Id,ClassAttendanceId,StudentId,PresentStatus,ApproveStatus,ApproveId,InTime,OutTime,IsLate,LateValue--%>
				            <Columns>
                                 <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-Width="500px" /> 
                                 <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                 
                            </Columns>
				            <FooterStyle BackColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
				</asp:GridView>
				</td> 
				</tr>
				</table>
				
				 </asp:Panel>
				 	<asp:Panel ID="Pnl_StaffReport" runat="server">
				<table width="100%">
				<tr>
				<td>
				<asp:GridView ID="Grd_StaffExitViolationReport" runat="server"    
				 CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" PageSize="10" AllowPaging="True" OnPageIndexChanging="Grd_StaffExitViolationReport_PageIndexChanging"  >
				            <RowStyle BackColor="White" />
				           <%-- Id,ClassAttendanceId,StudentId,PresentStatus,ApproveStatus,ApproveId,InTime,OutTime,IsLate,LateValue--%>
				            <Columns>
                               <asp:BoundField DataField="SurName" HeaderText="Staff Name" />
                               <asp:BoundField DataField="RoleName" HeaderText="Role Name" />
                                 
                            </Columns>
				            <FooterStyle BackColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
				</asp:GridView>
				</td> 
				</tr>
				</table>
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

<div class="clear"></div>
</div>
 </ContentTemplate>
 <Triggers>
 <asp:PostBackTrigger ControlID="Btn_Excel" />
 </Triggers>
                    </asp:UpdatePanel>
				
</asp:Content>
