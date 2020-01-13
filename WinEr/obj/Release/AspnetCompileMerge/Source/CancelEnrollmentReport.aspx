<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CancelEnrollmentReport.aspx.cs" Inherits="WinEr.CancelEnrollmentReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no">              
                       <img alt="" src="Pics/Staff/mypc_close.png"  width="30" height="30" /></td>
                <td class="n">Canceled Enrollment Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table width="100%">
                    
                     <tr>
                            <td align="right">From Date</td>
                            <td>
                            <asp:TextBox ID="Txt_from" runat="server" Width="170px" class="form-control"></asp:TextBox>                    
                    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" 
                                      Enabled="True" TargetControlID="Txt_from" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                                
                                    <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
                                        runat="server" ControlToValidate="Txt_from" Display="None" 
                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />

                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                TargetControlID="Txt_fromRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" 
                                Enabled="True" />
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                            <td align="right">To Date</td>
                            <td>
                              <asp:TextBox ID="Txt_To" runat="server" class="form-control" Width="170px"></asp:TextBox>
                                                  
                    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender1" runat="server" 
                                      Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                        
                          <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
                                runat="server" ControlToValidate="Txt_To" Display="None" 
                                ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                 /> 
            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="server" HighlightCssClass="validatorCalloutHighlight"                                       
            TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                            <td align="right">Canceled User </td>
                            <td> 
                                <asp:DropDownList ID="Drp_CanceledUser" runat="server" class="form-control"  Width="170px" ></asp:DropDownList>                                
                                    
                            </td></tr>
                           
                        <tr>
                         <td  style="width:350px">
                             <asp:Label ID="Lbl_Message" runat="server" class="control-label"  ForeColor="Red"></asp:Label>
                         </td>
                            <td   >
                                <asp:Button ID="Btn_Generate" runat="server" Text="Show Report" Class="btn btn-primary" 
                                    onclick="Btn_Generate_Click" ToolTip="Show Report"/>&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="Img_Export" runat="server" Width="35px" Height="35px" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Pics/Excel.png" onclick="Img_Export_Click" ToolTip ="Export to Excel"/>
                                
                            </td>
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_CanceledEnrollment" runat="server">
                     <div style="height:auto;  overflow:auto">
                     <br />
                        <asp:GridView ID="Grd_CanceledEnrollment" AutoGenerateColumns="false" runat="server"  CellPadding="4"  Width="100%"
                 BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                 onselectedindexchanged="Grd_Student_SelectedIndexChanged" CellSpacing="2" Font-Size="12px">
                        <Columns>                        
                        <asp:BoundField DataField="Id"/>
                        <asp:BoundField DataField="StudentName"  HeaderText="Student Name" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="175px"/>
                        <asp:BoundField DataField="Reason" HeaderText="Reason" ItemStyle-Width="175px"/>
                        <asp:BoundField DataField="CanceledUser" HeaderText="Canceled User" ItemStyle-Width="100px"/>
                            <asp:CommandField  ItemStyle-Font-Bold="true" ItemStyle-Width="50px"
        ItemStyle-Font-Size="Smaller" 
        SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='View Details'&gt;" 
        ShowSelectButton="True">
        <ControlStyle />
        <ItemStyle Font-Bold="True" Font-Size="Smaller" />
    </asp:CommandField>

                        </Columns>
             <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />

                      
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
    
        
    <div class="clear"></div>
    </div>
</asp:Content>
