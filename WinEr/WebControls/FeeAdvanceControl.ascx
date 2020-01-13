<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="FeeAdvanceControl.ascx.cs" Inherits="WinEr.WebControls.FeeAdvanceControl" %>
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<link href="css files/MasterStyle.css" rel="stylesheet" type="text/css" />
<link href="css files/winbuttonstyle.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_Msgbx" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_FeeAdvance"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_Msgbx"  />
                          <asp:Panel ID="Pnl_msg" runat="server" DefaultButton="Btn_Save" style="display:none;">
                         <div class="container skin5" style="width:500px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Advance" class="control-label"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <br />
               <table class="tablelist">
                 <tr>
                        <td class="leftside">FeeType</td>
                         <td class="rightside">
                             <asp:RadioButtonList ID="Rdo_FeeType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdo_FeeType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="Regular" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                             </asp:RadioButtonList>
                         </td>
                 </tr>
                 
                 <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
                 <tr>
                         <td class="leftside">FeeName</td>
                         <td class="rightside">
                             <asp:DropDownList ID="Drp_FeeName" runat="server" Width="162px"  class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Drp_FeeName_SelectedIndexChanged">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
                  <tr>
                         <td class="leftside">Batch</td>
                         <td class="rightside">
                             <asp:DropDownList ID="Drp_Batch" runat="server" Width="162px" class="form-control">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
                 <tr>
                         <td class="leftside">
                             <asp:Label ID="Lbl_DrpPeriod" runat="server" Text="Period" class="control-label"></asp:Label>
                         </td>
                         <td class="rightside">
                             <asp:DropDownList ID="Drp_Period" runat="server" Width="162px" class="form-control">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
                 <tr>
                         <td class="leftside">
                          <asp:Label ID="Lbl_TxtPeriod" runat="server" Text="Period" class="control-label"></asp:Label>
                         </td>
                         <td class="rightside">
                             <asp:TextBox ID="Txt_Period" runat="server" Width="160px"  MaxLength="30" class="form-control"></asp:TextBox>
                         </td>
                             <asp:RequiredFieldValidator ID="Txt_Period_reqVal" ErrorMessage="Enter Period" ValidationGroup="SaveAdv" ControlToValidate="Txt_Period" runat="server"></asp:RequiredFieldValidator>
                                          <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Period_FilteredTextBoxExtender" 
                                          runat="server" Enabled="True" TargetControlID="Txt_Period" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                 </tr>
                 <tr>
                          <td class="leftside">Amount</td>
                          <td class="rightside"> 
                              <asp:TextBox ID="Txt_Amt" runat="server" Width="160px" MaxLength="10" class="form-control"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="Txt_Amt_RequiredFieldValidator" ErrorMessage="Enter Amount" ValidationGroup="SaveAdv" ControlToValidate="Txt_Amt" runat="server"></asp:RequiredFieldValidator>
                          </td>
                           <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amt_FilteredTextBoxExtender" 
                           runat="server" Enabled="True" TargetControlID="Txt_Amt"   FilterType="Custom,Numbers" FilterMode="ValidChars" ValidChars=".">
                           </ajaxToolkit:FilteredTextBoxExtender>
                 </tr>
                 <tr>
                    <td class="leftside"></td>
                    <td class="rightside">
                        <asp:Button ID="Btn_Save" runat="server" Text="Add" Width="80px" OnClick="Btn_Save_Click"  Class="btn btn-primary"  ValidationGroup="SaveAdv"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_magok" runat="server" Text="Cancel" Width="80px" Class="btn btn-danger"/>
                    </td>
                 </tr>
               </table>
                       <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
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