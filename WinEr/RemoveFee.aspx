<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Inherits="RemoveFee"  Codebehind="RemoveFee.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<div id="right">

<div class="label">Fee Manager</div>
<div id="SubFeeMenu" runat="server">
		
 </div>
</div>

<div id="left">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    

      
      
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Remove Fee</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<asp:Panel ID="Panel1" runat="server" >
					<div id="topstrip">
					    <table class="tablelist">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Fee"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" class="control-label" Text="Frequency"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Yearly"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" ForeColor="White" class="control-label"
                                        Text="Associated to"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" ForeColor="White" class="control-label"
                                        Text="Student"></asp:Label>
                                </td>
                            </tr>
                        </table>
					<br/>
					</div>
              <asp:Panel ID="Panel2" runat="server" Width="100%" DefaultButton="Btn_edit" >
              <br />
                  <table  class="tablelist">
                      <tr>
                          <td  class="leftside">
                              Fee Name</td>
                          <td  class="rightside">
                              <asp:TextBox ID="Txt_feename" runat="server" class="form-control" Width="250px"></asp:TextBox>
                                
                          </td>
                         
                      </tr>
                      
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                      
                      <tr>
                          <td class="leftside">
                              Description</td>
                          <td class="rightside">
                              <asp:TextBox ID="Txt_Desc" runat="server" Height="70px" class="form-control" TextMode="MultiLine" 
                                  Width="250px" MaxLength="490"></asp:TextBox>
                                 
                          </td>
                      </tr>
                      
                      <tr>
                          <td class="leftside"></td>
                          <td class="rightside">
                              <asp:Label ID="Lbl_Note" runat="server" BackColor="White" class="control-label" ForeColor="#CC0000"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                      <tr>
                          <td class="leftside">
                              &nbsp;</td>
                         
                          <td class="rightside">
                          
                           <asp:Button ID="Btn_edit" runat="server" Text="Update" Class="btn btn-success"
                                  onclick="Btn_edit_Click" />
                              &nbsp;&nbsp;
                              <asp:Button ID="Btn_remove" runat="server" onclick="Btn_remove_Click" 
                                  Text="Remove" Class="btn btn-danger" />
                              <ajaxToolkit:ConfirmButtonExtender ID="Btn_remove_ConfirmButtonExtender" 
                            runat="server"  Enabled="True" TargetControlID="Btn_remove"
                            DisplayModalPopupID="Btn_remove_ModalPopupExtender">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <ajaxToolkit:ModalPopupExtender ID="Btn_remove_ModalPopupExtender" runat="server" TargetControlID="Btn_remove" PopupControlID="PNL" OkControlID="ButtonYes" CancelControlID="ButtonNo" />
                              <asp:Panel ID="PNL" runat="server" style="display:none">
                                  <div class="container skin5" style="width:400px; top:270px;left:170px">
                                      <table cellpadding="0" cellspacing="0" class="containerTable">
                                          <tr>
                                              <td class="no">
                                                  <asp:Image ID="Image6" runat="server" Height="28px" 
                                                      ImageUrl="~/elements/comment-edit-48x48.png" Width="29px" />
                                              </td>
                                              <td class="n">
                                                  <span style="color:White">Message</span></td>
                                              <td class="ne">
                                              </td>
                                          </tr>
                                          <tr>
                                              <td class="o">
                                              </td>
                                              <td class="c">
                                                  This will permanently remove the fee. Are you sure you want to remove the fee?
                                                  <br />
                                                  <br />
                                                  <div style="text-align:right;">
                                                      <asp:Button ID="ButtonYes" runat="server" Text="Yes" class="btn btn-success" Width="50px" />
                                                      <asp:Button ID="ButtonNo" runat="server" Text="No" class="btn btn-danger" Width="50px" />
                                                  </div>
                                              </td>
                                              <td class="e">
                                              </td>
                                          </tr>
                                          <tr>
                                              <td class="so">
                                              </td>
                                              <td class="s">
                                              </td>
                                              <td class="se">
                                              </td>
                                          </tr>
                                      </table>
                                  </div>
                              </asp:Panel>
                                  
                             
                                  
                          </td>
                      </tr>
                  </table>
              </asp:Panel>
                  
                  <br />
                  <br />
                 
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
               
              
            
        <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_LastMessage" 
                                  runat="server" 
                                  PopupControlID="Pnl_lastInfo" TargetControlID="hiddenTargetControlForModalPopup2"  />
                          <asp:Panel ID="Pnl_lastInfo" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr>
            <td class="no"> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr>
            <td class="o"> </td>
            <td class="c" >
                <asp:Label ID="Lbl_feedeleted" runat="server" class="control-label" Text=""></asp:Label>
               
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:Button ID="Btn_Finish" runat="server" Text="Finish" Width="50px" class="btn btn-success" onclick="Btn_Finish_Click"/>
                            
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
         
         
         
         <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"> <span style="color:White;font-size:larger"><b>Message!</b></span>  </td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-success" Text="OK" Width="50px"/>
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
         

</div>

<div class="clear"></div>
</div>

</asp:Content>

