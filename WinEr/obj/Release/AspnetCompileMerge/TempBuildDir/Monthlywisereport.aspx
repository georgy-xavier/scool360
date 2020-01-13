<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="Monthlywisereport.aspx.cs" Inherits="WinEr.Monthlywisereport" %>
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

<div id="contents">
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px"  style="min-height:350px"; >
			<tr >
				<td class="no"></td>
				<td class="n">Monthly Payroll Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
					<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
                    
                    <div style="min-height:200px">
                   
                    <center>
                    <asp:Panel ID="Pnl_MothnlywisePayrollReport" runat="server">
                    <table width="700px">
                    <tr>
                    <td class="leftside">Select Period</td>
                    <td class="rightside">
                    <asp:DropDownList ID="Drp_SelectPeriod" runat="server" Width="153px" class="form-control"
                            AutoPostBack="True" 
                            onselectedindexchanged="Drp_SelectPeriod_SelectedIndexChanged">
                    <asp:ListItem Text="This Month" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Last Month" Value="1"></asp:ListItem>
                    <asp:ListItem Text="This Year" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Manual" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                     <td class="leftside">Select Type</td>
                    <td class="rightside">
                    <asp:DropDownList ID="Drp_PayrollType" runat="server" Width="153px" class="form-control" 
                            AutoPostBack="True" 
                            onselectedindexchanged="Drp_PayrollType_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                    <td class="leftside">Select Head</td>
                    <td class="rightside">
                    <asp:DropDownList ID="Drp_Head" runat="server" Width="153px" class="form-control">
                    </asp:DropDownList>
                    </td>
                    </tr>
                    <tr id="SelectperiodRow" runat="server"> 
                     <td class="leftside">year</td>
                    <td class="rightside">
                    <asp:DropDownList ID="Drp_Year" runat="server" Width="153px" 
                            onselectedindexchanged="Drp_Year_SelectedIndexChanged" class="form-control" AutoPostBack="True">
                    </asp:DropDownList>
                    </td>
                   <td align="right">From</td>
                    <td align="left"><asp:DropDownList ID="Drp_FromMonth" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                    </td>
                   
                      <td align="right">To</td>
                    <td align="left"><asp:DropDownList ID="Drp_ToMonth" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                    </td>
                    </tr>
                   
                    </table>
                    <br />
                    <table width="100%">
                    
                    <tr>
                     <td align="center"><asp:Button ID="Btn_reportShow" runat="server" 
                    Text="SHOW" class="btn btn-primary" onclick="Btn_reportShow_Click" />
                    <asp:Button ID="Btn_ShowExcelReport" runat="server"  Text="EXPORT" 
                            class="btn btn-primary" onclick="Btn_ShowExcelReport_Click" /></td> 
                    
                    </tr>
                   
                    </table>  
                    <div class="linestyle"></div>
                    <table width="100%"><tr><td align="center"><asp:Label ID="Lbl_monthpayrollreport_err" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
                    </asp:Panel>
                    </center>
                    
                    <asp:Panel ID="Pnl_ShowReport" runat="server">                    
                    
                    <table width="100%">
                    <tr>
                    <td>
                        <asp:GridView DataKeyNames="EmpId" ID="Grd_MonthlyPayrollReport" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="True" OnPageIndexChanging="Grd_MonthlyPayrollReport_PageIndexChanging"
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  
                   CellPadding="3" CellSpacing="2" Font-Size="12px"> 
                   <Columns>
                   <%--BasicPay,TotalGross,TotalDeduction,NetPay,AdvSal,Advanceamount,Surname,EmpId,PresentAddress--%>
                   <asp:BoundField DataField="EmpId"  HeaderText="Id" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="Surname"  HeaderText="Employee Name" ItemStyle-Width="20px" />
                    <asp:BoundField DataField="PresentAddress"  HeaderText="Address" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="Earnings"  HeaderText="Total Earnings" ItemStyle-Width="20px" />                   
                   <asp:BoundField DataField="Deduction"  HeaderText="Total Deduction" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="TotalGross"  HeaderText="Gross Salary" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="NetPay"  HeaderText="Net Salary" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="Advanceamount"  HeaderText="Advance Amount" ItemStyle-Width="20px" />
                   <asp:BoundField DataField="AdvSal"  HeaderText="Advance Deduction" ItemStyle-Width="20px" />                   
                   </Columns>
                   
                   
                   
                   <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    
                    </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    
                  
                    
                    </div>
                    
                     </ContentTemplate>
                     <Triggers >
                         <asp:PostBackTrigger ControlID="Btn_ShowExcelReport"/>
                     </Triggers>
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
