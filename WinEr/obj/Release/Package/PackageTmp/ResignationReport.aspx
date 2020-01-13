<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ResignationReport.aspx.cs" Inherits="WinEr.ResignationReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">



<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
</ajaxToolkit:ToolkitScriptManager>  
<div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Staff Resignation Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >

              <table width="100%">
            
              <tr>
              <td align="right"  style="width:50%;">
              Time Period
              </td>
              <td align="left"  style="width:50%;" >
               <asp:DropDownList ID="Drp_Timeperiod" runat="server" class="form-control" AutoPostBack="True" onselectedindexchanged="Drp_Timeperiod_SelectedIndexChanged" 
                                    Width="170px"    >
                                   <asp:ListItem>Today</asp:ListItem>
                                   <asp:ListItem>This Month</asp:ListItem>
                                   <asp:ListItem>Last Week</asp:ListItem>
                                   <asp:ListItem>Manual</asp:ListItem>
                                   </asp:DropDownList>
              </td>
              </tr>
              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
              </table>
               <div id="datesArea" runat="server">
                 <table  width="100%">     
              <tr>
              <td align="right"  style="width:50%;">
              Start Date
              </td>
              <td align="left"  style="width:50%;" >
               <asp:TextBox ID="txt_StartDate" runat="server" Text="" Width="170px" class="form-control"></asp:TextBox>
                                   <ajaxToolkit:TextBoxWatermarkExtender ID="txt_StartDateTBWME"  TargetControlID="txt_StartDate"  runat="server" Enabled="True" WatermarkText="dd/mm/yyyy"> 
                                    </ajaxToolkit:TextBoxWatermarkExtender>                                   
                                    <br />
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_StartDate" Format="dd/MM/yyyy" runat="server" Enabled="True" CssClass="cal_Theme1"  >  </ajaxToolkit:CalendarExtender>
                                      <asp:RegularExpressionValidator ID="txt_StartDateREV"  ControlToValidate="txt_StartDate"  runat="server"  Display="None"  ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                                       <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_StartDate" runat="server"   MaskType="Date"  CultureName="en-GB" AutoComplete="true"  Mask="99/99/9999"  UserDateFormat="DayMonthYear"  Enabled="True" > </ajaxToolkit:MaskedEditExtender>    
                                       <ajaxToolkit:ValidatorCalloutExtender ID="txt_StartDateVCE"  TargetControlID="txt_StartDateREV"   runat="server" HighlightCssClass="validatorCalloutHighlight"    Enabled="True" />
                                   
              </td>
              </tr>
              <tr>
              
              <td align="right"  style="width:50%;">
              End Date
              </td>
              <td align="left"  style="width:50%;">
               <asp:TextBox ID="txt_endDate" runat="server" Text="" Width="170px" class="form-control"></asp:TextBox>
                                   <ajaxToolkit:TextBoxWatermarkExtender ID="txt_endDateTBWME"  TargetControlID="txt_endDate"  runat="server" Enabled="True" WatermarkText="dd/mm/yyyy"> 
                                    </ajaxToolkit:TextBoxWatermarkExtender> 
                                    
                                    <br />
                                   <ajaxToolkit:CalendarExtender ID="txt_endDateCE" TargetControlID="txt_endDate" Format="dd/MM/yyyy" runat="server" Enabled="True" CssClass="cal_Theme1"  >  </ajaxToolkit:CalendarExtender>
                                   <asp:RegularExpressionValidator ID="txt_endDateREV"  ControlToValidate="txt_endDate"  runat="server"  Display="None"  ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                                   <ajaxToolkit:MaskedEditExtender ID="txt_endDateMEE" TargetControlID="txt_endDate" runat="server"   MaskType="Date"  CultureName="en-GB" AutoComplete="true"  Mask="99/99/9999"  UserDateFormat="DayMonthYear"  Enabled="True" > </ajaxToolkit:MaskedEditExtender>    
                                   <ajaxToolkit:ValidatorCalloutExtender ID="txt_endDateVCE"  TargetControlID="txt_endDateREV"   runat="server" HighlightCssClass="validatorCalloutHighlight"    Enabled="True" />
                                   
              </td>
             
              </tr>
              </table>
              </div>
              <table width="100%">
              
              <tr>
               <td colspan="2" align="center">
               <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
               </td>
              </tr>
              <tr>
              <td colspan="2" align="center">
              <asp:Button  ID="Btn_search" runat="server" Text="SEARCH"  Class="btn btn-primary"
                      onclick="Btn_search_Click"/>
              
              </td>
              </tr>
              </table>
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
    
<p style="text-align:center;">
<asp:Label ID="lbl_StaffResignArea" runat="server" ForeColor="Red" class="control-label"></asp:Label>
</p>
<div id="StaffResignArea" runat="server" >

<div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Resigned Staffs List</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div id="grd_staffContents" style="overflow:auto;min-height:250px;" >
<asp:GridView ID="Grd_staff" runat="server" AutoGenerateColumns="false" 
        Font-Size="12px"  Width="100%"  BackColor="#e2e2c5"
    BorderColor="#e2e2c5" BorderStyle="None" BorderWidth="1px" CellSpacing="2"  AllowPaging="True" onpageindexchanging="Grd_staff_PageIndexChanging" 
       >
    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
    <EditRowStyle Font-Size="Medium" />
    <Columns>      
      <asp:BoundField DataField="UserId" HeaderText="UserId"  />
      <asp:BoundField DataField="UserName"  HeaderText="User Name"  />
      <asp:BoundField DataField="Reason"  HeaderText="Reason"  />
      <asp:BoundField DataField="Discription"  HeaderText="Description"  />
      <asp:BoundField DataField="ResignDate"  HeaderText="ResignDate"  />
     <%-- <asp:BoundField DataField="Sex"  HeaderText="Sex"  />
      <asp:BoundField DataField="DateOfLeaving"  HeaderText="Date Of Leaving"  />--%>
     <%-- <asp:CommandField ShowSelectButton="true"/>             --%>
    
     </Columns>
     <SelectedRowStyle BackColor="#ebebeb" Font-Bold="True" ForeColor="Black"  />
     <PagerStyle ForeColor="Black" HorizontalAlign="Center" BackColor="#bfbfbf"/>
     <HeaderStyle BackColor="#e8e8e8" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
     <RowStyle BackColor="White" BorderColor="Olive" ForeColor="Black" Font-Bold="false" Font-Size="12px"  HorizontalAlign="Left" />
   </asp:GridView>
        </div>
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


</div>

    <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" CancelControlID="Btn_magok"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"> </td>
                            <td class="n"><span style="color:White">Student Details</span></td>
                            <td class="ne">&nbsp;</td>
                        </tr>
                        <tr >
                            <td class="o"> </td>
                            <td class="c" >
                               
                                <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
                                        <div id="studdtls" runat="server">
                                        
                                        </div>
                                        
                                        <br />
                                        <div style="text-align:center;">
                                            
                                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-primary" Text="OK"  
                                              />
                                        
                                        </div>
                            </td>
                            <td class="e"> </td>
                        </tr>
                        <tr>
                            <td class="so"> </td>
                            <td class="s"> </td>
                            <td class="se"> </td>
                        </tr>
                    </table>
    <br /><br />
                        
                           
                       
</div>
                       
                        </asp:Panel> 

</div>

</asp:Content>
