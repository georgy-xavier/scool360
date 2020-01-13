<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgBoxControl.ascx.cs" Inherits="Scool360student.MsgBoxControl" %>
<link rel="stylesheet" type="text/css" href="../css files/mbContainer.css" title="style"  media="screen"/>

<asp:Panel ID="Pnl_MessageBox" runat="server">
<asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">
                <asp:Label ID="Lbl_Head" runat="server" Text="Message"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                            
                       
                        <div style="text-align:center;">
                        <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Black"></asp:Label>
                             <br />
                              <br />
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary"/>
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