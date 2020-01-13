<%@ Page  Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="True" CodeBehind="BooksFineReport.aspx.cs" Inherits="WinEr.BooksFineReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
       <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>  
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>  
                
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
               <asp:Panel ID="Pn_Search" runat="server" DefaultButton="Btn_Show">                                
                    <div class="container skin1" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr ><td class="no"></td><td class="n">Search Types</td><td class="ne"></td></tr>
                            <tr ><td class="o"></td><td class="c">
                            
                            <table>
                                <tr>
                                    <td style="font-weight:bold">Select Date Filter </td>
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="RdBtnType" runat="server"  RepeatDirection="Horizontal" CellSpacing="13"
                                            onselectedindexchanged="RdBtnType_SelectedIndexChanged" AutoPostBack="true">
                                         <asp:ListItem Text="Today" Selected="True" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Last Week" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Last Month" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Selected Date" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="4"></asp:ListItem>
                                        
                                        </asp:RadioButtonList>
                                       
                                    </td>
                                    
                                    </tr>
                                    <tr>
                                    <td>
                                    <br />
                                    </td>
                                    </tr>
                                    <tr>
                                    <td valign="top" style="width:15%" align="right">
                                    Start Date &nbsp;&nbsp;:&nbsp;</td><td>
                                      <asp:TextBox ID="Txt_StartDate" runat="server" Width="160px" class="form-control" TabIndex="3" ToolTip="DD/MM/YYYY"></asp:TextBox> 
                                                 <ajaxToolkit:MaskedEditExtender ID="Txt_StartDate_MaskedEditExtender" runat="server"   
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_StartDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
        <br />
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_StartDate" ErrorMessage="You must enter startdate"></asp:RequiredFieldValidator>
                   
                     <asp:RegularExpressionValidator runat="server" ID="StartDateRegularExpressionValidator3"
                                ControlToValidate="Txt_StartDate"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="StartDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" /><br/>
                               <br />
                               </td><td valign="top"  style="width:15%" align="right">
                               End Date&nbsp;&nbsp;:&nbsp;</td><td valign="top">   <asp:TextBox ID="Txt_EndDate" class="form-control" runat="server" Width="160px" TabIndex="3" ToolTip="DD/MM/YYYY"></asp:TextBox> 
                                                   <ajaxToolkit:MaskedEditExtender ID="Txt_EndDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_EndDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
        <br />
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_EndDate" ErrorMessage="You must enter end date"></asp:RequiredFieldValidator>
                   
                     <asp:RegularExpressionValidator runat="server" ID="EndDateRegularExpressionValidator1"
                                ControlToValidate="Txt_EndDate"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" /><br/>
                               
                               
                                    </td>
                                    <td valign="top" style="width:15%;" align="center">
                                    <asp:Button ID="Btn_Show" Text="Show" runat="server" class="btn btn-primary" 
                                            onclick="Btn_Show_Click"  />
                                    </td>
                                    </tr>
                                    <tr>
                                    <td colspan="5">
                                        <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                    </tr>
                                
                            </table>
                            
                            
                             </td><td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                        </table>
                    </div> 
                </asp:Panel>   
                 <asp:Panel ID="Pnl_Content" runat="server" >                                
                    <div class="container skin1" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr ><td class="no"></td><td class="n">Fine Details</td><td class="ne"></td></tr>
                            <tr ><td class="o"></td><td class="c">
                           
                           <div align="right">
                                        <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
                                        Width="42px" onclick="Btn_Export_Click"></asp:ImageButton>
                                    </div>
                                    <asp:GridView ID="GrdBooks" runat="server" BackColor="White" AutoGenerateColumns="False"
                                    BorderColor="#BFBFBF" BorderStyle="Solid" Font-Size="12px" PageSize="20"
                                    BorderWidth="1px" CellPadding="5"   OnPageIndexChanging="GrdBooks_PageIndexChanging"
                                    ForeColor="Black" GridLines="Vertical"
                                    Width="100%" AllowPaging="True">
                                    <Columns>
                                        

                                        <asp:BoundField DataField="BookNo" HeaderText="Book Code" />
                                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                        <asp:BoundField DataField="Author" HeaderText="Author" />
                                        <asp:BoundField DataField="Edition" HeaderText="Edition" />
                                       
                                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" />
                                        <asp:BoundField DataField="ReturnDate" HeaderText="Returned Date" />
                                        <asp:BoundField DataField="USerType" HeaderText="User Type" />
                                        <asp:BoundField DataField="Username" HeaderText="User Name" />
                                        <asp:BoundField DataField="Fine" HeaderText="Fine" />
                                        
                                    </Columns>
                                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                    <EditRowStyle Font-Size="Medium" />
                                    <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="12px" ForeColor="Black"  HorizontalAlign="Left" />
                                    <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="12px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" Height="20px" />
                                    <AlternatingRowStyle BorderColor="#BFBFBF" />
                                    </asp:GridView>
                                <br />
                                <p align="right">
                               Total Fine&nbsp;&nbsp;:&nbsp;&nbsp; <asp:Label ID="Lbl_TotalFine" Text="" runat="server" Font-Bold="true"></asp:Label>
                               </p>
                             </td><td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                        </table>
                    </div> 
                </asp:Panel>
               <WC:MSGBOX id="WC_MessageBox" runat="server" />         
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="img_export_Excel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
