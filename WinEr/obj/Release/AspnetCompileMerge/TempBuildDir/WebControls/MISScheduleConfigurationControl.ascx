<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MISScheduleConfigurationControl.ascx.cs" Inherits="WinErParentLogin.WebControls.MISScheduleConfigurationControl" %>
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<asp:Panel ID="Pnl_MISMessageBox" runat="server">
<asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
  
 <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
 <div class="container skin1" style="width:800px; top:400px;left:400px"  >
 <table   cellpadding="0" cellspacing="0" class="containerTable">
 <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:Black">
                <asp:Label ID="Lbl_Head" runat="server" Text="MIS Schedule Report"></asp:Label></span></td>
            <td class="ne"></td>
 </tr>
 <tr >
            <td class="o"> </td>
            <td class="c" align="center" >
             <div id="HtmlDiv" runat="server">
              <table >
          <tr>
                         <td>MIS Schedule Name</td>
                         <td><asp:TextBox ID="Txt_misschedulename" runat="server" Width="240px"></asp:TextBox>
				    </td>
                         </tr>
          <tr>
                         <td>Schedule Time </td>
                         <td>
                         <asp:RadioButtonList ID="Radiobtn_periodtype" runat="server" 
                           Width="240" RepeatDirection="Horizontal"  >
                           <asp:ListItem Text="Daily"  Value="0" Selected="True"></asp:ListItem>  
                           <asp:ListItem Text="Weekly" Value="1"></asp:ListItem>  
                           <asp:ListItem Text="Mothly" Value="2"></asp:ListItem>  
                           </asp:RadioButtonList>
                         </td>
                         </tr>
          <tr>
                         <td>E-Mail Ids</td>
                         <td><asp:TextBox ID="Txt_emailaddress" runat="server" Width="240px" TextMode="MultiLine" Height="50px" MaxLength="10000"></asp:TextBox>
</td>
                     
                         
                         </tr>
          <tr>
                         <td> Report Names</td>
                         <td>
                          <asp:CheckBoxList ID="Chkboxlist_reportname" runat="server" Width="240px"   
                                    Height="50px" >
                              </asp:CheckBoxList>
                         </td>
                         </tr>
          <tr>
          <td></td>
          <td align="right">
              <asp:Label ID="Lbl_err" runat="server" Font-Bold="true" Font-Size="XX-Small" ForeColor="Red" Text="Label"></asp:Label></td>
          </tr>
          <tr>
          <td colspan="2" align="center">
           <asp:Button ID="Btn_Update" runat="server" Text="Update"  Width="75px" OnClick="Btn_Update_Click" />
          <asp:Button ID="Btn_magok" runat="server" Text="Cancel" Width="75px"/>
          </td>
          </tr>
                         
  </table>
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
 </div>
 </asp:Panel>                                 
</asp:Panel>
