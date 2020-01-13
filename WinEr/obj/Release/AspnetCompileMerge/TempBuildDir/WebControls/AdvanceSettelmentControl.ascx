<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceSettelmentControl.ascx.cs" Inherits="WinEr.WebControls.AdvanceSettelmentControl" %>
<asp:HiddenField ID="HiddenField_StudentId" runat="server" Value="0" />
<asp:Panel ID="Pnl_AdvanceSettelmentBox" runat="server">
                         <asp:Button runat="server" ID="Btn_AdvSettelBtn" style="display:none" class="btn btn-info"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AdvanceSettelment"  runat="server" CancelControlID="Btn_AdvCancelCan"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_AdvanceSettelmentmsg" TargetControlID="Btn_AdvSettelBtn"  />
                          <asp:Panel ID="Pnl_AdvanceSettelmentmsg" runat="server" style="display:none">
                         <div class="container skin5" style="width:700px;"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/delete_page.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Advance Settlement" class="control-label"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <br />
               <table class="tablelist">
               <tr>
               <td>
               Select Fee Schedule :
               </td>
               <td>
                   <asp:DropDownList ID="Drp_FeeSchedule" Width="450px" class="form-control" runat="server">
                   </asp:DropDownList>
               </td>
               </tr>
                   <tr>
                       <td>
                           &nbsp;</td>
                       <td>
                           &nbsp;</td>
                   </tr>
                   <tr>
               <td>
                 Select Advance :
               </td>
               <td>
                 <asp:DropDownList ID="Drp_Advance" Width="450px" class="form-control" runat="server">
                   </asp:DropDownList>
               </td>
               </tr>
                   <tr>
                       <td>
                           &nbsp;</td>
                       <td>
                           &nbsp;</td>
                   </tr>
                   <tr>
                       <td>
                           &nbsp;</td>
                       <td>
                           <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                       </td>
                   </tr>
                 <tr>
                    <td ></td>
                    <td >
                        <asp:Button ID="Btn_AdvcancelSave" runat="server" Text="Save" Width="80px" 
                            Class="btn btn-info"  ValidationGroup="CancelAdv" 
                            onclick="Btn_AdvcancelSave_Click"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_AdvCancelCan" runat="server" Text="Cancel" Class="btn btn-info" Width="80px"/>
                    </td>
                 </tr>
               </table>
                      
                    
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
                        
                        
                        
                        <div id="ConfermArea">
                        
                         <ajaxToolkit:ConfirmButtonExtender ID="Btn_AdvcancelSave_ConfirmButtonExtender" 
                        runat="server" DisplayModalPopupID="Btn_AdvcancelSave_ModalPopupExtender" 
                        Enabled="True" TargetControlID="Btn_AdvcancelSave">
                    </ajaxToolkit:ConfirmButtonExtender>
                    <ajaxToolkit:ModalPopupExtender ID="Btn_AdvcancelSave_ModalPopupExtender" BackgroundCssClass="modalBackground"
                        runat="server" CancelControlID="ButtonCancel" OkControlID="ButtonOk" 
                        PopupControlID="PNL" TargetControlID="Btn_AdvcancelSave" />
                        <asp:Panel ID="PNL" runat="server" style="display:none; ">
                
                
                <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/comments.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:large">confirm!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                Are you sure you want to settle the advance ?
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:Button ID="ButtonOk" runat="server" Class="btn btn-info" Text="Yes"  />
                            <asp:Button ID="ButtonCancel" Class="btn btn-info" runat="server" Text="No"  />
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