<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="Staffresign"  Codebehind="Staffresign.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
       
       
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">
<div id="right">


<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>

</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<div id="left">
     <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
    <br />
    
    
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no"><img alt="" src="Pics/Staff/mypc_close.png" height="35" width="35" /> </td>
                <td class="n">Resign Staff</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                   
                  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
 <ContentTemplate>
   <asp:Panel ID="Panel1" runat="server">
    <table width="100%">
        
        
        <tr>
            <td  align="right">
                Reason</td>
            <td >
                <asp:DropDownList ID="DrpDownReason" runat="server" AutoPostBack="True" 
                    class="form-control" Width="160px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
            <td  align="right" valign="top" >
                Description</td>
            <td >
                <asp:TextBox ID="txtDis" runat="server" Height="40px" class="form-control" TextMode="MultiLine" 
                    Width="160px" MaxLength="300"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender 
                       ID="FilteredTextBoxExtender2"
                       runat="server"
                       TargetControlID="txtDis"
                       FilterType="Custom"
                       FilterMode="InvalidChars"
                      InvalidChars="'\"
                     />
            </td>
            
           
           
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
        <tr>
        <td align="right" valign="top">Relieving Date</td>
        <td> <asp:TextBox ID="Txt_RelievingDate" runat="server" Width="160px" class="form-control"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txt_date_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="Txt_RelievingDate" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>                        
                        <ajaxToolkit:MaskedEditExtender ID="txt_date_MaskedEditExtender" runat="server" 
                          MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                Mask="99/99/9999"
                                                UserDateFormat= "DayMonthYear"
                                                Enabled="True"
                            TargetControlID="Txt_RelievingDate">
                        </ajaxToolkit:MaskedEditExtender>
                        <span style="color:Blue" id="dtfromfomat" runat="server"  >DD/MM/YYYY</span>
        </td>
        
        </tr>
        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
       
        <tr>
            <td>
                &nbsp;</td>
            <td >
                <asp:TextBox ID="TxtName" runat="server" class="form-control" Visible="false"></asp:TextBox>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
            </td>
            <td >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="BtnResigd" runat="server" onclick="BtnResigd_Click" 
                    Text="Resign" class="btn btn-success" />
                      &nbsp;
                <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                    Text="Cancel" class="btn btn-danger" />  
                  <ajaxToolkit:ConfirmButtonExtender ID="BtnResigd_ConfirmButtonExtender" 
                            runat="server"  Enabled="True" TargetControlID="BtnResigd"
                            DisplayModalPopupID="BtnResigd_ModalPopupExtender">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <ajaxToolkit:ModalPopupExtender ID="BtnResigd_ModalPopupExtender" runat="server" TargetControlID="BtnResigd" PopupControlID="PNL" OkControlID="ButtonYes" CancelControlID="ButtonNo" />
                  <asp:Panel ID="PNL" runat="server" style="display:none; ">
                       
                <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White;font-size:large">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                       
                       
                        Staff will be move to history.Do you want to proceed anyway?
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:Button ID="ButtonYes" runat="server" Text="Yes" class="btn btn-success" />
                            <asp:Button ID="ButtonNo" runat="server" Text="No" class="btn btn-danger"/>
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
                    </asp:Panel>
                    
            
            </td>
        </tr>
        <tr>
        <td colspan="2" align="center">
        <asp:Label ID="Lbl_Advsal" runat="server" ForeColor="Red"></asp:Label>
        </td>
        </tr>
    </table>
    

                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-success"/>
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
                         
                     
                     
                     
                              
                            <asp:Button runat="server" ID="Btn_hdnmessagetgt1" style="display:none"/> 
              <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox1" 
                                  runat="server" 
                                  PopupControlID="Pnl_msg1" TargetControlID="Btn_hdnmessagetgt1"  />
                                        <asp:Panel ID="Pnl_msg1" runat="server" style="display:none;">  
                                             <div class="container skin5" style="width:400px; top:400px;left:400px" >
                                                    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_Message" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="UpdateOk" runat="server" Text="Ok" class="btn btn-success" OnClick="UpdateOk_Click"/>
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
 </ContentTemplate>
  </asp:UpdatePanel> 
                   
                   
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
<div class="clear"></div>
     
</div>
</asp:Content>

