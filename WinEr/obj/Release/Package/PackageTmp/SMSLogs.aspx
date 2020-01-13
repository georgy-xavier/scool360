<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSLogs.aspx.cs" Inherits="WinEr.SMSLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </ajaxToolkit:ToolkitScriptManager>          
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
    <ProgressTemplate> <div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate>
</asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" >
                <div class="container skin1" >
		            <table cellpadding="0" cellspacing="0" class="containerTable">
			            <tr ><td class="no"> </td><td class="n">SMS REPORT</td><td class="ne"> </td></tr>
			            <tr >
				            <td class="o"> </td>
				            <td class="c" >
				            
				                <table class="tablelist">
				                    <tr>
				                        <td class="leftside">Select SMS Type</td><td class="rightside"> 
				                            <asp:RadioButtonList ID="Rdb_CheckType" runat="server" 
                                            RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="20"  >
                                            <asp:ListItem Text="All Staff" Value="0"></asp:ListItem> 
                                            <asp:ListItem Text="Parent" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Student" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                           
                                        </td>
				                    </tr>
				                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				                    <tr>
				                        <td class="leftside"> 
				                            From  </td><td class="rightside"> <asp:TextBox ID="Txt_from" runat="server" class="form-control" Width="170px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" 
                                                        CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy"          TargetControlID="Txt_from">
                                                    </ajaxToolkit:CalendarExtender>
                                                        <asp:RegularExpressionValidator ID="Txt_from_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_from" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                                            runat="server" HighlightCssClass="validatorCalloutHighlight"      TargetControlID="Txt_from_DateRegularExpressionValidator3" Enabled="True" />
            
            
                    </td></tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                                              <td class="leftside">    To  </td><td class="rightside">     <asp:TextBox ID="Txt_To" runat="server" class="form-control" Width="170px"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                            CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy" 
                                                            TargetControlID="Txt_To">
                                                        </ajaxToolkit:CalendarExtender>
                    
                                                        <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_To" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                                        runat="server" HighlightCssClass="validatorCalloutHighlight"                                       
                                                        TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />
				                        </td>
				                    </tr>
				                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				                    <tr>
				                    <td colspan="2" align="center">
				                     <asp:Button ID="Btn_show" runat="server" Text="Show" class="btn btn-primary" onclick="Btn_show_Click" />
				                    </td>
				                    
				                    </tr>
				                </table>
				                <asp:Panel ID="showarea" runat="server">
				                    <asp:GridView ID="Grd_SMSList" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="false" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                                    OnPageIndexChanging="Grd_SMSList_PageIndexChanging"
                                    Font-Size="15px" PageSize="15" Width="100%">
                                    <EmptyDataTemplate>Reports not found</EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center"/>
                                        <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                        <EditRowStyle Font-Size="Medium" />
                                        <Columns>
                                            <asp:BoundField DataField="StaffName" HeaderText="Staff Name" />
                                            <asp:BoundField DataField="ParentName" HeaderText="Parent Name" />
                                            <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                            <asp:BoundField DataField="Date" HeaderText="Date" />
                                            <asp:BoundField DataField="Message" HeaderText="Message" />
                                            
                                        </Columns>
                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                    
                                    </asp:GridView>
				                
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
            </asp:Panel>                  
        </ContentTemplate>
    </asp:UpdatePanel>
<div class="clear"></div>
</div>


</asp:Content>
