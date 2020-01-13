<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherFeeControl.ascx.cs" Inherits="WinEr.WebControls.OtherFeeControl" %>
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<link href="css files/MasterStyle.css" rel="stylesheet" type="text/css" />
<link href="css files/winbuttonstyle.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_Msgbx" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_OtherFee"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_Msgbx"  />
                          <asp:Panel ID="Pnl_msg" runat="server" DefaultButton="Btn_Save" style="display:none;">
                         <div class="container skin5" style="width:500px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Other Fee"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <br />
               <table class="tablelist">
                 <tr>
                         <td class="leftside">FeeName</td>
                         <td class="rightside">
                             <asp:DropDownList ID="Drp_FeeName" runat="server" class="form-control" Width="162px">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                          <td class="leftside">Amount</td>
                          <td class="rightside"> 
                              <asp:TextBox ID="Txt_Amt" runat="server" Width="160px" MaxLength="10" class="form-control"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="Txt_Amt_RequiredFieldValidator" ErrorMessage="Enter Amount" ValidationGroup="Saveme" ControlToValidate="Txt_Amt" runat="server"></asp:RequiredFieldValidator>
                          </td>
                           <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amt_FilteredTextBoxExtender" 
                           runat="server" Enabled="True" TargetControlID="Txt_Amt"   FilterType="Custom,Numbers" FilterMode="ValidChars" ValidChars=".">
                           </ajaxToolkit:FilteredTextBoxExtender>
                 </tr>
                 <tr>
                    <td class="leftside"></td>
                    <td class="rightside">
                        <asp:Button ID="Btn_Save" runat="server" Text="Add" Width="80px" Class="btn btn-info"  OnClick="Btn_Save_Click" ValidationGroup="Saveme"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_magok" runat="server" Text="Cancel" Class="btn btn-danger" Width="80px"/>
                    </td>
                 </tr>
               </table>
                       <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br />
                        
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