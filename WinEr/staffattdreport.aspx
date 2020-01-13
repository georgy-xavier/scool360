<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="staffattdreport.aspx.cs" Inherits="WinEr.staffattdreport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" >

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
            <td class="no"> </td>
            <td class="n">Staff Attendance Report</td>
            <td class="ne"> </td>
          </tr>
          <tr >
             <td class="o"> </td>
             <td class="c" >  
             
                <table width="100%" cellspacing="10">
                  <tr>
                   <%-- <td align="right" style="width:25%">Select Staff</td>
                    <td align="left" style="width:25%">
                       <asp:DropDownList ID="Drp_StaffSelect" runat="server" class="form-control" Width="170px">
                         </asp:DropDownList>
                    </td>--%>
                     <td align="right"  style="width:25%">
                        Time Period</td>
                     <td  align="left" style="width:25%">
                         <asp:DropDownList ID="Drp_Period" runat="server" AutoPostBack="True" class="form-control"
                            onselectedindexchanged="Drp_Period_SelectedIndexChanged" Width="170px">
                            <asp:ListItem>Today</asp:ListItem>
                            <asp:ListItem>Last Week</asp:ListItem>
                            <asp:ListItem>This Month</asp:ListItem>
                            <asp:ListItem>Current Batch</asp:ListItem>
                            <asp:ListItem>Manual</asp:ListItem>
                            
                          </asp:DropDownList>
                       </td>
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                        <td align="right">
                                  From Date</td>
                        <td  align="left">
                           <asp:TextBox ID="Txt_StartDate" runat="server"  Width="170px" class="form-control"></asp:TextBox>
                           <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
                           </cc1:CalendarExtender>  
                            <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                               runat="server" ControlToValidate="Txt_StartDate" Display="None" 
                               ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                           </td>
                           <td align="right">
                               To Date</td>
                           <td  align="left">
                               <asp:TextBox ID="Txt_EndDate" runat="server"  Width="170px" class="form-control"></asp:TextBox>
                               <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
                                <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                                     runat="server" ControlToValidate="Txt_EndDate" Display="None" 
                                     ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                     ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                TargetControlID="Txt_EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                            </td>  
                          </tr>
                          <tr>
                            <td align="center"  colspan="4" >  
                                <asp:Button ID="Btn_showreport" runat="server" Text="Show" 
                                   onclick="Btn_showreport_Click" Class="btn btn-primary" />
                                   
                                   &nbsp;<asp:Button ID="Btn_export_Excel" runat="server" Text="Export" 
                                  Class="btn btn-primary" onclick="Btn_export_Excel_Click" />
                                 
                            </td>
                           </tr>
                           <tr>
                              <td align="center" colspan="4">
                                <br />
                                <asp:Label ID="lblerror" runat="server" ForeColor="Red" class="control-label"></asp:Label>
                              </td>
                           </tr>
                         </table>  
                       <br />              
                       <div style="width:100%";>
                         <asp:GridView ID="Grd_Attenndancerept" runat="server" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" PageSize="15" AllowPaging="True" 
                            onpageindexchanging="Grd_Attenndancerept_PageIndexChanging">
                            <RowStyle BackColor="White" />
                            <Columns>
                                 <asp:BoundField DataField="Name" HeaderText="Staff Name" />
                                 <asp:BoundField DataField="WorkingDays" HeaderText="Working Days" />
                                 <asp:BoundField DataField="PresentDays" HeaderText="Present Days" />
                                 <asp:BoundField DataField="HalfDays" HeaderText="Half Days" />
                                 <asp:BoundField DataField="AbsentDays" HeaderText="Absent Days" />
                                 <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                                 
                            </Columns>
                            <FooterStyle BackColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                           </asp:GridView>
                         </div>

             </td>
             <td class="e"> </td>
          </tr>
          <tr>
              <td class="o">
                     &nbsp;</td>
              <td class="c">
                     &nbsp;</td>
              <td class="e">
                      &nbsp;</td>
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
   <asp:PostBackTrigger ControlID="Btn_export_Excel" />
  </Triggers>
  </asp:UpdatePanel>
</asp:Content>
