<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" CodeBehind="ViewUserActions.aspx.cs" Inherits="WinEr.ViewUserActions" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
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
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/evolution-tasks.png" width="30" height="30" /></td>
                <td class="n">User Logs</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                   
                   
                 
                      <table class="style1">
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
                          <tr>
                              <td>
                                  User Type</td>
                              <td>
                                  <asp:DropDownList ID="Drp_UserType" runat="server" AutoPostBack="True" class="form-control"
                                      onselectedindexchanged="Drp_UserType_SelectedIndexChanged" Width="160px">
                                      <asp:ListItem Value="0">All</asp:ListItem>
                                      <asp:ListItem Value="1">Staff</asp:ListItem>
                                      <asp:ListItem Value="2">Parent</asp:ListItem>
                                      <asp:ListItem Value="3">Student</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  User Name</td>
                              <td>
                                  <asp:DropDownList ID="Drp_UserName" runat="server" AutoPostBack="True" class="form-control"
                                      onselectedindexchanged="Drp_UserName_SelectedIndexChanged" Width="160px">
                                  </asp:DropDownList>
                                  <cc1:ListSearchExtender ID="Drp_UserNameListSearchExtender" runat="server" 
                                      PromptCssClass="ListSearchExtenderPrompt" QueryPattern="Contains" 
                                      QueryTimeout="2000" TargetControlID="Drp_UserName">
                                  </cc1:ListSearchExtender>
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
                          <tr>
                              <td>
                                  Action</td>
                              <td>
                                  <asp:DropDownList ID="Drp_Action" runat="server" Width="160px" class="form-control"
                                      AutoPostBack="True" onselectedindexchanged="Drp_Action_SelectedIndexChanged">
                                  </asp:DropDownList>
                                   <cc1:ListSearchExtender ID="Drp_Action_ListSearchExtender" runat="server" 
                                    PromptCssClass="ListSearchExtenderPrompt" QueryPattern="Contains" 
                                    QueryTimeout="2000" TargetControlID="Drp_Action">
                                </cc1:ListSearchExtender>
                              </td>
                              <td>
                                  Time Period</td>
                              <td>
                                  <asp:DropDownList ID="Drp_Period" runat="server" AutoPostBack="True" class="form-control" Width="160px"
                                      onselectedindexchanged="Drp_Period_SelectedIndexChanged">
                                      <asp:ListItem>Today</asp:ListItem>
                                      <asp:ListItem>Last Week</asp:ListItem>
                                      <asp:ListItem>This Month</asp:ListItem>
                                      <asp:ListItem>Manual</asp:ListItem>
                                  </asp:DropDownList>
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
                          <tr>
                              <td>
                                  From</td>
                              <td>
                                  <asp:TextBox ID="Txt_StartDate" runat="server" class="form-control" width="160px"></asp:TextBox>
                                   <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
                            <%--<asp:RegularExpressionValidator runat="server" ID="Txt_StartDateDateRegularExpressionValidator3"
                                ControlToValidate="Txt_StartDate"
                                Display="None" 
                                ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_StartDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                              </td>
                              <td>
                                  To</td>
                              <td>
                                  <asp:TextBox ID="Txt_EndDate" runat="server" class="form-control" Width="160px"></asp:TextBox>
                                   <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
                                   <%--<asp:RegularExpressionValidator runat="server" ID="Txt_EndDateRegularExpressionValidator1"
                                ControlToValidate="Txt_EndDate"
                                Display="None" 
                                ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                                                        runat="server" ControlToValidate="Txt_EndDate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="Txt_EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
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
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td colspan="2">
                                  <asp:Button ID="Btn_View" runat="server" Class="btn btn-primary" 
                                      onclick="Btn_View_Click" Text="View Report" />
                                  &nbsp;&nbsp;
                                  <asp:Button ID="Btn_export" runat="server" Class="btn btn-primary" 
                                      onclick="Btn_export_Click" Text="Export To Excel" />
                                      
                                        &nbsp;&nbsp;
                                  <asp:Button ID="Btn_ModuleUsageChart" runat="server" Class="btn btn-primary" 
                                      onclick="Btn_ModuleUsageChart_Click" Text="Module Chart"/>
                              </td>
                          </tr>
                          <tr>
                              <td colspan="4" align="center"> 
                                  <asp:Label ID="lblerror" runat="server" class="control-label" Text=""></asp:Label></td>
                          </tr>
                      </table>
                   
          <asp:Panel ID="Pnl_UserAction" runat="server">  
          <div class="linestyle"></div>       
                 <div>
        <asp:GridView ID="Grd_Actions"  runat="server"  AutoGenerateColumns="false"
             GridLines="Vertical" Width="100%" 
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px"
                   PageSize="50" AllowPaging="True" 
                        onpageindexchanging="Grd_Actions_PageIndexChanging" >
                        <Columns>
                            
                             <asp:BoundField DataField="UserName" HeaderText="User Name" />
                             <asp:BoundField DataField="Action" HeaderText="Action" />
                             <asp:BoundField DataField="Time" HeaderText="Time" ItemStyle-Width="130px" />
                             <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="500px" />
                             
                        </Columns>
           <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
        </asp:GridView>
        
        
        
             <br />
        </div>
               </asp:Panel>      
                   
                   <web:chartcontrol id="chartcontrol_modulesuse" runat="server" Width="650px"  Visible="false"
                                        GridLines="None" BorderStyle="None" TopPadding="0" ChartPadding="0" 
                                        YCustomEnd="0" YCustomStart="0" YValuesInterval="0" Padding="5" 
                                              BorderWidth="0px" Height="450px">
                                        <Border DashStyle="Dot" /><Background Color="LightSteelBlue" /><PlotBackground ForeColor="White" />
                                        <ChartTitle StringFormat="Center,Near,Character,LineLimit" />
                                        <Legend Font="Tahoma, 7pt, style=Bold"></Legend>
                                        <Charts><Web:PieChart Explosion="8" Legend="Some Legend" Name="yearly_Chart">
                                                   <DataLabels Font="Tahoma, 8pt, style=Bold" Separator=": " ShowXTitle="True" 
                                                       Visible="True">
                                                              <Border Color="Blue" />
                                                              <Background ForeColor="White" />
                                                    </DataLabels>
                                                    <Shadow Color="Gray" Visible="True" />
                                                  </Web:PieChart>
                                         </Charts>
                                         <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
                                         <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                                         <XTitle StringFormat="Center,Near,Character,LineLimit" />
                                         <YTitle StringFormat="Center,Near,Character,LineLimit" />
                                       </web:chartcontrol>
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
<div>

            
            
            

 <%--<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Actions</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"> </td>
				<td class="c" >
					
		
        
        
					
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%>
</div>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="Btn_export"/>
</Triggers>
</asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
