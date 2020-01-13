<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CertificateIssueReport.aspx.cs" Inherits="WinEr.CertificateIssueReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 42px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

           <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div id="progressBackgroundFilter">
                </div>
                <div id="processMessage">
                    <table style="height: 100%; width: 100%">
                        <tr>
                            <td align="center">
                                <b>Please Wait...</b><br />
                                <br />
                                <img src="images/indicator-big.gif" alt="" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>

<div class="container skin1" >
<table   cellpadding="0" cellspacing="0" class="containerTable">
<tr >
<td class="no"></td>
<td class="n">Certificate Issue Report</td>
<td class="ne"> </td>
</tr>
<tr >
<td class="o"> </td>
<td class="c" >
<br />
<asp:Panel ID="Pnl_Filter" runat="server">


<table width="100%" cellspacing="15">
                       <tr>
                         <td align="right" style="width:25%">Select Class:</td>
                         <td style="width:25%" align="left">
                                 <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="160px">
                                 </asp:DropDownList>
                         </td>
                         <td align="right" style="width:20%">
                                     Time Period:</td>
                          <td align="left" style="width:30%">
                                      <asp:DropDownList ID="Drp_Period" runat="server" AutoPostBack="True" class="form-control"
                                          onselectedindexchanged="Drp_Period_SelectedIndexChanged" Width="160px">
                                      <asp:ListItem Selected="True">Today</asp:ListItem>
                                      <asp:ListItem>Last Week</asp:ListItem>
                                      <asp:ListItem>This Month</asp:ListItem>
                                      <asp:ListItem>Manual</asp:ListItem>
                                  </asp:DropDownList>
                          </td>
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                        <tr>

                           <td align="right">Start Date:</td>
                           <td align="left">
                                     <asp:TextBox ID="Txt_SDate" runat="server" class="form-control" Width="160px"></asp:TextBox>
                                      <ajaxToolkit:MaskedEditExtender ID="Txt_SDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_SDate" CultureAMPMPlaceholder="AM;PM" 
                                              CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                              CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                              CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                        
                                         <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                                ControlToValidate="Txt_SDate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                                TargetControlID="DobDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                      ControlToValidate="Txt_SDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                           </td>
                             <td align="right"> End Date:</td>
                             <td align="left">
                                           <asp:TextBox ID="Txt_EDate" runat="server" class="form-control" Width="160px"></asp:TextBox>
                                          
                                          
                                           <ajaxToolkit:MaskedEditExtender ID="Txt_EDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_EDate" CultureAMPMPlaceholder="AM;PM" 
                                               CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                               CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                               CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                              
                                     <asp:RegularExpressionValidator runat="server" ID="Txt_EDate_RegularExpressionValidator1"
                                                ControlToValidate="Txt_EDate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender4"
                                                TargetControlID="Txt_EDate_RegularExpressionValidator1"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                                
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                      ControlToValidate="Txt_EDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                           </td>
                          </tr>
                          <tr>

                            <td align="right">
                                      Issue User:
                           </td>
                           <td align="left">
                                       <asp:DropDownList ID="Drp_User" runat="server"
                                        class="form-control"  Width="160px" >
                                      </asp:DropDownList>
                           </td>
                              <td align="right">
                                      Certificate Type:
                           </td>
                           <td align="left">
                                       <asp:DropDownList ID="Drp_Certificate" runat="server" class="form-control"
                                          Width="160px" >
                                      </asp:DropDownList>
                           </td>
                         </tr>
                         <tr>    
                                          
                            <td align="center" colspan="4" class="style1">  <asp:Button ID="Btn_Show" runat="server" Text="Show" 
                                     onclick="Btn_Show_Click" Class="btn btn-primary" />
                                      &nbsp;&nbsp;
                                  <asp:Button ID="Btn_export" runat="server" Class="btn btn-primary" 
                                      onclick="Btn_export_Click" Text="Export To Excel" Width="130px" />
                             
                             </td>
                           
                          </tr>
                         
                              <tr>
                              <td colspan="4" align="center"> 
                                  <asp:Label ID="Lbl_Err" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                              </td>
                           </tr>
                      </table>
</asp:Panel>
<table width="100%">
</table>
<asp:Panel ID="Pnl_Report" runat="server" >
<div >
<div class="linestyle"></div>  
<br />

<asp:GridView ID="Grd_Report" AutoGenerateColumns="false"
runat="server"
OnSelectedIndexChanged="Grd_Report_SelectedIndexChanged" 
OnPageIndexChanging="Grd_Report_PageIndexChanging" 
Width="100%" AllowPaging="true" 
PageSize="10"  BackColor="#EBEBEB"
BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
CellPadding="3" CellSpacing="2" Font-Size="12px">

<Columns>
<asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="50px" />
<asp:BoundField DataField="Student_Id"  HeaderText="Student_Id" ItemStyle-Width="50px" />
<asp:BoundField DataField="Student_Name"  HeaderText="Student Name" ItemStyle-Width="50px" />
<asp:BoundField DataField="Class_Name"  HeaderText="Class Name" ItemStyle-Width="50px" />
<asp:BoundField DataField="Certificate_Name"  HeaderText="Certificate Name" ItemStyle-Width="100px" />  
<asp:BoundField DataField="Created_Time"  HeaderText="Created Date" ItemStyle-Width="100px" />  
<asp:BoundField DataField="Created_User"  HeaderText="Created User" ItemStyle-Width="100px" />  

                 <asp:CommandField ControlStyle-Width="30px" HeaderText="Print" 
                        ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" ItemStyle-Width="30px"  
                        SelectText="&lt;img src='Pics/print1.png' width='30px' border=0 title='Select To View'&gt;" 
                        ShowSelectButton="True">
                        
                        <ControlStyle Width="30px" />
                        <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                        </asp:CommandField>                      
</Columns>

<PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
<FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
<EditRowStyle Font-Size="Medium" />
<SelectedRowStyle BackColor="White" ForeColor="Black" />
<PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
<HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Center" />
<RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Center" />
</asp:GridView>
</div>
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
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="Btn_export"/>
</Triggers>
    </asp:UpdatePanel> 
<div class="clear"></div>
</asp:Content>
