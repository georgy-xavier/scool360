<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="RFswipingRecord.aspx.cs" Inherits="WinEr.RFswipingRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
 



        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
               
            
          <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">RF Swiping Record</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                <div style="min-height:250px">
                   
                 <table width="100%" cellspacing="5">
                  <tr>
                    <td style="width:50%" align="right">
                      
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Class : "></asp:Label>
                   
                    </td>
                    <td style="width:50%"  align="left">
                      
                        <asp:DropDownList ID="Drp_Class" runat="server" Width="200px" class="form-control" AutoPostBack="true"
                            onselectedindexchanged="Drp_Class_SelectedIndexChanged">
                        </asp:DropDownList>
                   
                    </td>
                  </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   <tr>
                    <td style="width:50%" align="right">
                      
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Student : "></asp:Label>
                   
                    </td>
                    <td style="width:50%"  align="left">
                      
                        <asp:DropDownList ID="Drp_Student" runat="server" Width="200px" class="form-control">
                        </asp:DropDownList>
                   
                    </td>
                  </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                  <tr>
                   <td style="width:50%" align="right">
                    <asp:Label ID="Label3" runat="server" Text="Delete Upto : " class="control-label"></asp:Label>
                   </td>
                   <td>
                        <asp:TextBox ID="Txt_StartDate" runat="server" class="form-control"  Width="200px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                               CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>  
                       <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                           runat="server" ControlToValidate="Txt_StartDate" Display="None" 
                           ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                           ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                      <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                           TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                          HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                   </td>
                  </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   <tr>
                    <td style="width:50%" align="right">
                      

                   
                    </td>
                    <td style="width:50%"  align="left">
                      
                          <asp:Button ID="Btn_Load" runat="server" Text="Load" Class="btn btn-primary" 
                              onclick="Btn_Load_Click" />
                      
                         &nbsp;
                      
                         <asp:Button ID="Btn_ClearAll" runat="server" Text="Delete All" 
                              Class="btn btn-danger" 
                              OnClientClick="return confirm('Are you sure, you want to delete swipe records of all student')" 
                              onclick="Btn_ClearAll_Click" />
                   
                    </td>
                  </tr>
                  <tr>
                   <td colspan="2" align="center">
                   
                       <asp:Label ID="lbl_error" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
                   
                   </td>
                   </tr>
                   <tr>
                   <td colspan="2" align="right">
                   <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px"  OnClick="img_export_Excel_Click"></asp:ImageButton>
                   </td>
                   </tr>
                  <tr>
                   <td colspan="2">
                   
                    <asp:GridView ID="Grd_SwipeLogg" runat="server" 
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                            GridLines="Vertical" 
                           onpageindexchanging="Grd_SwipeLogg_PageIndexChanging">                         
                            <FooterStyle BackColor="#CCCC99" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                            <RowStyle BackColor="Transparent" />
                            <Columns>
                                <asp:BoundField DataField="ActionDate" HeaderText="Date" />   
                                <asp:BoundField DataField="RFReaderName" HeaderText="RFReader Name"  />  
                                <asp:BoundField DataField="RFReaderType" HeaderText="Status" />           
                            </Columns>
                            <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                   
                   </td>
                  </tr>
                 </table>
                   
                   
                   
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
            
            
              
              
    <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" class="btn btn-info" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">  <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text="Are you sure you want"></asp:Label>
                <br /><br />
                
                <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Class="btn btn-info"/>
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
 </asp:Panel>


    <div class="clear"></div>
    </div>
</asp:Content>
