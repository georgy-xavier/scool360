<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="BookTransactionReport.aspx.cs" Inherits="WinEr.BookTransactionReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
                                <asp:Panel ID="Pn_Search" runat="server">                                
                    <div class="container skin1" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">                        
                            <tr >
                            <td class="no"></td><td class="n">Book Transaction Report</td><td class="ne"></td></tr>
                            <tr >
                            <td class="o"></td>
                            <td class="c">
                            <div style="min-height:200px">
                            <asp:Panel ID="Pnl_Search" runat="server">
                            <center>                            
                          <table width="500px">                          
                          <tr>
                          <td class="leftside">Select Period</td>
                          <td class="rightside"><asp:DropDownList ID="Drp_Date" runat="server" Width="150px" class="form-control"
                                  AutoPostBack="True" onselectedindexchanged="Drp_Date_SelectedIndexChanged">
                            <asp:ListItem Text="Today" Value="0"></asp:ListItem>
                             <asp:ListItem Text="This Month" Value="1">
                             </asp:ListItem>
                             <asp:ListItem Text="Manual" Value="2">
                             </asp:ListItem>
                            </asp:DropDownList></td>
                             <td class="leftside">From</td>
                            <td class="rightside">
                            <asp:TextBox ID="Txt_FromDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                             <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_FromDate" Format="dd/MM/yyyy">
                           </cc1:CalendarExtender>  
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
                            <td class="leftside">Select Category</td>
                            <td class="rightside"><asp:DropDownList ID="Drp_Category" runat="server" class="form-control" Width="150px">
                            </asp:DropDownList></td>
                           
                          
                                    <td class="leftside">To</td> 
                            <td class="rightside"><asp:TextBox ID="Txt_ToDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                             <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ToDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
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
                             <td class="leftside"><%--Select Book Name<--%></td>
                             <td class="rightside"><%--<asp:TextBox Width="150px" ID="Txt_BookName" runat="server"></asp:TextBox>--%>
                            <%-- <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender" 
                            runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetIsssuedBookId"  UseContextKey="true"
                            ServicePath="WinErWebService.asmx"  TargetControlID="Txt_BookName" MinimumPrefixLength="1">
                            </ajaxToolkit:AutoCompleteExtender>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Search_TextBoxWatermarkExtender" runat="server"
                            Enabled="True" TargetControlID="Txt_BookName" WatermarkText=" Enter Data">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Search_FilteredTextBoxExtender" runat="server"
                            Enabled="True" TargetControlID="Txt_BookName" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>--%><asp:Button ID="Btn_Search" runat="server" Text="SHOW" class="btn btn-primary" 
                                    Width="102px" onclick="Btn_Search_Click"/>
                            </td>
                            <td></td>
                            <td></td>
                            </tr>
                            
                            </table>
                         
                            </center>
                            </asp:Panel>
                                                   
                            <asp:Panel ID="Pnl_ShowReport" runat="server">
                              <div class="linestyle"></div>   
                            <table width="100%">
                            <tr>
                            <td align="right">                            
                            <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                            Width="42px" onclick="img_export_Excel_Click" ></asp:ImageButton>
                            
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <asp:GridView  ID="Grd_Report" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" 
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4" 
                           ForeColor="Black" GridLines="Vertical" Width="100% " 
                         PageSize="30">
                           
                           <Columns> 
                                <asp:BoundField DataField="BookId" HeaderText="Book Id" />
                                <asp:BoundField DataField="UserId" HeaderText="User Id" />
                                <asp:BoundField DataField="UserTypeId" HeaderText="User Type" />
                                <asp:BoundField DataField="BookName" HeaderText="Book Name" />  
                                <asp:BoundField DataField="TakenDate" HeaderText="Issued Date" />                                
                                 <asp:BoundField DataField="Author" HeaderText="Author" />  
                                  <asp:BoundField DataField="Edition" HeaderText="Edition" />  
                                   <asp:BoundField DataField="Publisher" HeaderText="Publisher" />  
                                   <asp:BoundField DataField="CatogoryName" HeaderText="Category Name" /> 
                                   <asp:BoundField DataField="UserName" HeaderText="User Name" />                                    
                                   <asp:BoundField DataField="UserType" HeaderText="User Type" />                                      
                            </Columns>

                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
                             <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                             <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                             <EditRowStyle Font-Size="Medium" />    
                       </asp:GridView>
                            </td>
                            </tr>
                            </table>
                            </asp:Panel>
                            </div>
                             </td>
                             <td class="e"></td>
                            </tr>
                            <tr ><td class="so"> </td>
                            <td class="s"></td>
                            <td class="se"></td>
                            </tr>
                        </table>
                    </div> 
                </asp:Panel>  
                
                   <WC:MSGBOX id="WC_MessageBox" runat="server" />                     
            </ContentTemplate>
            <Triggers >
                         <asp:PostBackTrigger ControlID="img_export_Excel"/>
                     </Triggers>
        </asp:UpdatePanel>
    </div>
				
				
</asp:Content>
