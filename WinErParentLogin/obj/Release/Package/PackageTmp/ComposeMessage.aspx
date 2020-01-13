<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="ComposeMessage.aspx.cs" Inherits="WinErParentLogin.ComposeMessage" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
       
       
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
    <table width="100%" ><%--style="border-top:#4a4a4a thin solid">--%>
 <tr>
  <td valign="top"> 
  <div  style="width:300px" >
          <table>
          <tr><td><a class="topmenu" href="MessageHome.aspx">Inbox</a></td><td><a  class="topmenu" href="ComposeMessage.aspx">New Message</a></td> 
         </tr>
          </table>
   
                
  </div>
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/e-mail.png"
                        Height="29px" Width="29px" /></td>
                <td class="n">
                    Compose</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   <div style="min-height:300px;">
                   
             <table class="tablelist">
                       
                
                <tr>
                    <td class="leftside">
                         Subject</td>
                    <td class="rightside">
                        <asp:TextBox ID="txt_subject" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqd_subj" runat="server"  
      ControlToValidate="txt_subject" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
       <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender1"
                                           runat="server"
                                           TargetControlID="txt_subject"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        Description</td>
                    <td class="rightside">
                     <asp:TextBox ID="txt_descrpn" runat="server" class="form-control" Height="100px" TextMode="MultiLine" ></asp:TextBox>
     <asp:RequiredFieldValidator ID="rqd_msg" runat="server"  
      ControlToValidate="txt_descrpn" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
       <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender2"
                                           runat="server"
                                           TargetControlID="txt_descrpn"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
                      </td>
                </tr>
                <tr>
                    <td class="leftside">
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Label ID="LblFailureNotice" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="leftside">
                        
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Button ID="Btn_Send"  runat="server" ValidationGroup="ValidSend"   Width="90px"
                            Text="Send" onclick="Btn_Send_Click"  class="btn btn-primary"/> 
                            
                        &nbsp; &nbsp; 

                        <asp:Button ID="btn_cancel"  runat="server" Width="90px"
                            Text="Cancel"   class="btn btn-danger"/> 
                     </td>
                </tr>
                  <tr>
                    <td class="leftside">
                        
                        &nbsp;</td>
                    <td class="rightside">
                        <asp:Label ID="lbl_message" runat="server" Text="" ForeColor="Red"></asp:Label>
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
      
      </td>
  
 </tr>
 
 
</table>
            <WC:MSGBOX ID="MSGBOX" runat="server" />
        </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
