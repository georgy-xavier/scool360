<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" Inherits="ManageBatch"  Codebehind="ManageBatch.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    

 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
                <br />
<center>

<div class="container skin1" style="width:450px;">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no">
                    <img alt="" src="Pics/calendar_empty.png" height="35" width="35" /> </td>
                <td class="n">Create New Academic Year</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
             
              
                   <asp:Panel ID="Pnl_BatchDetails" runat="server">
              
              
                <table class="tablelist">
                    <tr >
                        <td colspan="2" style="text-align:center;">
                            
                       
                            <asp:Label ID="Lbl_currentBATCH" runat="server" Text="2009-2010" class="control-label"
                                Font-Bold="True" Font-Size="Medium"></asp:Label>
                            &nbsp;&nbsp;&nbsp;<img alt="" src="Pics/next.png" 
                                style="height: 33px; width: auto" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Lbl_nextbatch" runat="server" Text="2010-2011" Font-Bold="True" class="control-label"
                                Font-Size="Medium"></asp:Label>
                        </td>
                       
                       
                    </tr>
                   <tr>
                   <td colspan="2">&nbsp;<asp:HiddenField ID="HiddenField_Newbatchid" runat="server" Visible="False" /></td>
                   </tr>
                    <tr>
                        
                        <td class="leftside">
                            Starting Date<span style="color:Red">*</span></td>
                        <td class="rightside">
                            <asp:TextBox ID="Txt_startdate" runat="server" class="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Txt_startdate_CalendarExtender" 
                                runat="server" CssClass="cal_Theme1" Enabled="True" 
                                TargetControlID="Txt_startdate" Format="dd/MM/yyyy">
                            </ajaxToolkit:CalendarExtender>
                             <asp:RequiredFieldValidator ID="Txt_startdateRequiredFieldValidator2" runat="server" ControlToValidate="Txt_startdate" ErrorMessage="*" ValidationGroup="NewBatch"></asp:RequiredFieldValidator>
                        </td>
                        
                       
                    </tr>
                    
                    <tr>
                        <td class="leftside">
                            Ending Date<span style="color:Red">*</span></td>
                        <td class="rightside">
                            <asp:TextBox ID="Txt_EndDate" runat="server" class="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Txt_EndDate_CalendarExtender" runat="server" 
                                CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
                            </ajaxToolkit:CalendarExtender>
                             <asp:RequiredFieldValidator ID="Txt_EndDateRequiredFieldValidator1" runat="server" ControlToValidate="Txt_EndDate" ErrorMessage="*" ValidationGroup="NewBatch"></asp:RequiredFieldValidator>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2">
                          
                                <asp:RegularExpressionValidator ID="StartDateRegularExpressionValidator3" 
                                runat="server" ControlToValidate="Txt_startdate" Display="None" 
                                ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                />
                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" 
                                runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                TargetControlID="StartDateRegularExpressionValidator3" />
                                <asp:RegularExpressionValidator ID="EndDateRegularExpressionValidator1" 
                                 runat="server" ControlToValidate="Txt_EndDate" Display="None" 
                                 ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                             />
                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                TargetControlID="EndDateRegularExpressionValidator1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">

                              <asp:Label ID="Lbl_note" runat="server" ForeColor="Red" class="control-label">                          
                              </asp:Label>
                        </td>
                    </tr>
                    <tr> 
                       <td></td>                      
                        <td>
                            <asp:Button ID="Btn_createbatch" runat="server"  OnClick="Btn_createbatch_Click" ValidationGroup="NewBatch"   Text="Create" Class="btn btn-primary" />
                            
                        </td>
                       
                    </tr>
                   <tr>
                   <td colspan="2">&nbsp;</td>
                   </tr>
                </table>
               

                                   <asp:Button ID="Btn_PopUpTarget" runat="server" class="btn btn-info" style="display:none;"/>
                            <ajaxToolkit:ModalPopupExtender ID="Btn_createbatch_MPE"  runat="server" CancelControlID="ButtonNo"  BackgroundCssClass="modalBackground"
                            PopupControlID="PNL" TargetControlID="Btn_PopUpTarget" />
                           <asp:Panel ID="PNL" runat="server" style="display:none;">
                           <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                 <tr >
                                 <td class="no"> <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" /> </td>
                                 <td class="n"><span style="color:White;font-size:large">alert!</span></td>
                                 <td class="ne">&nbsp;</td>
                                 </tr>
                                 <tr >
                                     <td class="o"> </td>
                                     <td class="c" >
                                      Creating a new batch will start a new calendar year. All the data in the previous year will be moved to history. The newly created batch will be activated as the current batch. Are you sure you want to proceed?
                                      <br/><br/>
                                        <div style="text-align:center;">
                                         <asp:Button ID="ButtonYes" runat="server" Text="Yes" Width="50px" class="btn btn-info" onclick="Btn1_yes_Click" />
                                         <asp:Button ID="ButtonNo" runat="server" Text="No" class="btn btn-info" Width="50px"/>
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
                        
                        
                        
                        
                                   <asp:Button ID="Button_Popup_Promotion" runat="server" class="btn btn-info" style="display:none;"/>
                            <ajaxToolkit:ModalPopupExtender ID="Button_promotion_popup"  runat="server" CancelControlID="ButtonPromotionNo"  BackgroundCssClass="modalBackground"
                            PopupControlID="Popup_promotion" TargetControlID="Button_Popup_Promotion" />
                           <asp:Panel ID="Popup_promotion" runat="server" style="display:none;">
                           <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                 <tr >
                                 <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" /> </td>
                                 <td class="n"><span style="color:White;font-size:large">alert!</span></td>
                                 <td class="ne">&nbsp;</td>
                                 </tr>
                                 <tr >
                                     <td class="o"> </td>
                                     <td class="c" >
                                      Some students are in promotion list.Do you want to transfer that students to history?
                                      <br/><br/>
                                        <div style="text-align:center;">
                                         <asp:Button ID="ButtonPromotionYes" runat="server" Text="Yes" class="btn btn-info" Width="50px" onclick="BtnPromotion_yes_Click" />
                                         <asp:Button ID="ButtonPromotionNo" runat="server" Text="No" class="btn btn-info" Width="50px"/>
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
  <WC:MSGBOX id="WC_MessageBox" runat="server" />     
</center>
</ContentTemplate>
 </asp:UpdatePanel>
<div class="clear"></div>
</div>
<br/>
<br/>
<br/>
<br/>
<br/>

</asp:Content>

