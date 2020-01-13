<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateDocType.aspx.cs" Inherits="WinEr.CreateDocType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet"><ProgressTemplate>                               
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /> <br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>          
    </ProgressTemplate></asp:UpdateProgress>
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="container skin1">
                <table cellpadding="0" cellspacing="0" class="containerTable">
                    <tr><td class="no"></td>
                        <td class="n"></td>
                        <td class="ne"></td>
                    </tr>
                    <tr>
                        <td class="o"></td>
                        <td class="c">
                            <div align="center">
                                <table width="100%">
                                    <tr>
                                        <td  align="right">Create Document Type</td>
                                        <td  align="left">
                                            
                                            <asp:TextBox ID="Txt_DocType" runat="server" Width="240px" class="form-control" MaxLength="50" TabIndex="1"></asp:TextBox>
                                        </td>
                                    </tr>
                                       
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                    
                                    <tr>
                                        <td  align="right">Is Mandatory</td>
                                        <td  align="left">
                                            <asp:DropDownList ID="Drp_Mandatory" runat="server" class="form-control" Width="200px">
                                                 <asp:ListItem Value="M">Yes</asp:ListItem>  
                                               <asp:ListItem Value="O">No</asp:ListItem>  

                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                  
                                     <tr>
                                        <td></td>
                                        <td  align="left"><asp:Button ID="Btn_CreateDocType" runat="server" Text="Create DocTyoe" class="btn btn-primary" OnClick="Btn_CreateDocType_Click"/></td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" align="center"><asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>                                                
                        </td>
                        <td class="e"></td>
                    </tr>
                    <tr>
                        <td class="so"></td>
                        <td class="s"></td>
                        <td class="se"></td>
                    </tr>
                </table>
            </div>	
            
                <div>
                                   <asp:Button ID="Btn_hdnmessagetgt" runat="server" style="display:none" class="btn btn-primary"/>
                                   <cc1:ModalPopupExtender ID="MPE_MessageBox" runat="server" 
                                       BackgroundCssClass="modalBackground" CancelControlID="Btn_magok" 
                                       PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
                                   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                                       <div class="container skin5" style="width:400px; top:400px;left:400px">
                                           <table cellpadding="0" cellspacing="0" class="containerTable">
                                               <tr>
                                                   <td class="no">
                                                   </td>
                                                   <td class="n">
                                                       <span style="color:White">Message</span></td>
                                                   <td class="ne">
                                                       &nbsp;</td>
                                               </tr>
                                               <tr>
                                                   <td class="o">
                                                   </td>
                                                   <td class="c"> 
                                                       <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <div style="text-align:center;">
                                                           <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-primary"/>
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
                                           <br />
                                           <br />
                                       </div>
                                   </asp:Panel>
                               </div>	                
            	                     
        </ContentTemplate> 
         <Triggers><asp:PostBackTrigger ControlID="Btn_CreateDocType" />
   <%--  <asp:PostBackTrigger ControlID="Drp_SelectList" />
      <asp:PostBackTrigger ControlID="Drp_Class" />--%>
    </Triggers>  
    </asp:UpdatePanel>
</asp:Content>
