<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryIssueItemReport.aspx.cs" Inherits="WinEr.InventoryIssueItemReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                
                 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate>
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Issue Item Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
               <asp:Panel ID="Pnl_InitialArea" runat="server">               
               <center>
               <table width="700px">
               <tr>
                <td align="right">Select Type</td>
                <td align="left">
               <asp:RadioButtonList ID="Rbd_SelectType" runat="server" Width="130px"  AutoPostBack="true" 
               OnSelectedIndexChanged="Rbd_SelectType_SelectedIndexChanged"
                RepeatDirection="Horizontal">
               <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
               <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
               </asp:RadioButtonList>
               </td>               
               <td align="right">Item Name</td>
               <td align="left"><asp:DropDownList ID="Drp_Item" runat="server" Width="153px" class="form-control"></asp:DropDownList></td>
                    <td align="right">Period</td>
               <td align="left"><asp:DropDownList ID="Drp_Period" runat="server" Width="153px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_Period_SelectedIndexChanged">
               <asp:ListItem Text="This Month" Value="0">
                 </asp:ListItem>
                 <asp:ListItem Text="Last Week" Value="1">
                 </asp:ListItem>
                 <asp:ListItem Text="Manual" Value="2">
                 </asp:ListItem>
               </asp:DropDownList></td>
               </tr>
               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
          
               
               <td align="right">Start Date</td>
               <td align="left">      <asp:TextBox ID="Txt_FromDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Show" ControlToValidate="Txt_FromDate" ErrorMessage="*"></asp:RequiredFieldValidator>
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
                     <td align="right">End Date</td>
               <td align="left"><asp:TextBox ID="Txt_ToDate" runat="server"  class="form-control" Width="150px"></asp:TextBox>
                 <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ToDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Show" ControlToValidate="Txt_ToDate" ErrorMessage="*"></asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                         runat="server" ControlToValidate="Txt_ToDate" Display="None" 
                         ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                   <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                    TargetControlID="Txt_EndDateRegularExpressionValidator1"
                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" /></td>
                    
                    <td></td>                     <td align="left"><asp:Button ID="Btn_show" runat="server" Text="Show" ValidationGroup="Show" class="btn btn-primary" OnClick="Btn_show_Click"  />
               </td>
               </tr>          
               <tr id="RowStudent" runat="server">
               <td align="right">Class</td>
               <td align="left"><asp:DropDownList ID="Drp_Class" runat="server" Width="153px" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td>
               <td align="right">Student Name</td>
               <td align="left"><asp:DropDownList ID="Drp_Student" runat="server" Width="153px" class="form-control"></asp:DropDownList></td>
               </tr>
        
               <tr>
               <td align="right" ><asp:Label ID="Lbl_Staff" runat="server" Text="Staff"></asp:Label></td>
               <td align="left" ><asp:DropDownList ID="Drp_StaffName" runat="server" Width="153px" class="form-control"></asp:DropDownList></td>
               <td></td>
               
               </tr>
  
               <tr>
               <td>
               </td>
               
                <td>
               </td>
                <td>
               </td>
            
               </tr>
                            <tr>
               <td colspan="6" align="center">
               <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"> </asp:Label>
               </td>
               </tr>
               </table>
              </center>
               </asp:Panel>
               
               <asp:Panel ID="Pnl_DisplayStudent" runat="server">
               <table width="100%">
                 <tr>
                 <td align="left"><asp:Label ID="Lbl_TotalCost" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                <td align="right">   
            <%--    onpageindexchanging="Grd_Items_PageIndexChanging"--%>
                <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px"  OnClick="img_export_Excel_Click"
                    Width="42px"></asp:ImageButton></td>
                    </tr>
               <tr>
               <td colspan="2">
                 <asp:GridView ID="Grd_StudentIssuereport" runat="server"  onpageindexchanging="Grd_StudentIssuereport_PageIndexChanging"
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%">
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="BookId" HeaderText="BookId"/>
                      <asp:BoundField DataField="StudId" HeaderText="StudId"/>
                      <asp:BoundField DataField="ClassId" HeaderText="ClassId" />
                      <asp:BoundField DataField="ItemName" HeaderText="Item Name"/>                      
                      <asp:BoundField DataField="StudentName" HeaderText="Student Name"/>
                      <asp:BoundField DataField="ClassName" HeaderText="Class Name"/>
                      <asp:BoundField DataField="IssueDate" HeaderText="Issue Date"/>
                      <asp:BoundField DataField="Count" HeaderText="Count"/>
                       <asp:BoundField DataField="Cost" HeaderText="Cost"/>
                      
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
               </td>
               </tr>
               </table>
               </asp:Panel>
               
                            <asp:Panel ID="Pnl_DiaplayStaff" runat="server">
               <table width="100%">
                 <tr>
                <td align="right">   
            <%--    onpageindexchanging="Grd_Items_PageIndexChanging"--%>
                <asp:ImageButton ID="Img_StaffExcel" ToolTip="Export to Excel" runat="server"  OnClick="Img_StaffExcel_Click"
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px"></asp:ImageButton></td>
                    </tr>
               <tr>
               <td>
                 <asp:GridView ID="Grd_StaffIssueReport" runat="server"  onpageindexchanging="Grd_StaffIssueReport_PageIndexChanging"
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%">
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="Id" HeaderText="Id"/>
                      <asp:BoundField DataField="ItemId" HeaderText="ItemId"/>  
                      <asp:BoundField DataField="ItemName" HeaderText="Item Name" />                                          
                      <asp:BoundField DataField="StaffName" HeaderText="Staff Name"/>
                      <asp:BoundField DataField="Quantity" HeaderText="Quantity"/> 
                      <asp:BoundField DataField="ActionDate" HeaderText="Date"/>
                      
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
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
    
    </ContentTemplate>
  <Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  <asp:PostBackTrigger ControlID="Img_StaffExcel" />
  </Triggers>
  </asp:UpdatePanel>

<div class="clear"></div>
</asp:Content>
