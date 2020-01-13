<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="FixStudent.aspx.cs" Inherits="WinEr.FixStudent" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
 



        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
               
            
          <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Fix Student Name</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                <table width="100%">
                 <tr>
                     <td align="right" style="width:50%">
                        Enter symbol
                      </td>
                      <td>
                          <asp:TextBox ID="Txt_Symbol" runat="server" Text=""></asp:TextBox>
                      </td>
                 </tr>
                 <tr>
                      <td align="right" style="width:50%">
                        Click to fix student Names
                      </td>
                      <td>
                          <asp:Button ID="Btn_Fix" runat="server" Text="Fix"  CssClass="graysearch" 
                              onclick="Btn_Fix_Click"/>
                      </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                   
                      <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red"></asp:Label>
                  </td>
                 </tr>
                </table>
                    
                   
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
            
            
              
              
    <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">  <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="Are you sure you want"></asp:Label>
                <br /><br />
                
                <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" CssClass="grayok"/>
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


    <div class="clear"></div>
    </div>
</asp:Content>
