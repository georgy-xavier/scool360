<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StaffAvailability.aspx.cs" Inherits="WinEr.StaffAvailability" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

<div id="contents">
<div id="right">
   

<%--<div id="sidebar2">

</div>--%>
<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>
</div>


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
                           Arun Sunny</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Class</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           BDS</td>
                       
                       <td class="attributeValue">Admission No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td></td>
                       </tr>
                       <tr>
                       <td class="attributeValue">Class No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
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
<div class="container skin1" >
<table cellpadding="0" cellspacing="0" class="containerTable">
<tr >
 <td class="no">   </td>
 <td class="n" align="left">Staff Availability</td>
 <td class="ne"> </td>
</tr>
<tr >
<td class="o"> </td>
<td class="c" >
 
 <table width="100%">
 
  <tr>
   <td align="center">
   <br />
   
   <asp:GridView ID="Grd_StaffAvailability" runat="server"   Visible="true"
                             CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="False" 
                             Width="80%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                             BorderWidth="1px">
                                <RowStyle BackColor="#F7F7DE" />
                                <Columns>   
                                 <asp:BoundField DataField="PeriodId" HeaderText="PeriodId"/>
                                <asp:BoundField DataField="FrequencyName" HeaderText="Period"   HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        ControlStyle-Width="100px">
                                    <ControlStyle Width="100px" />
                                 </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Monday" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_AvlMon" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Tuesday">
                                    <ItemTemplate>
                                         <asp:CheckBox ID="Chk_AvlTues" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Wednesday">
                                    <ItemTemplate>
                                           <asp:CheckBox ID="Chk_AvlWed" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Thursday">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_AvlThur" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Friday">
                                    <ItemTemplate>
                                         <asp:CheckBox ID="Chk_AvlFri" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Saturday">
                                    <ItemTemplate>
                                          <asp:CheckBox ID="Chk_AvlSat" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px" HeaderText="Sunday" ItemStyle-Height="30px">
                                    <ItemTemplate>
                                         <asp:CheckBox ID="Chk_AvlSun" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle BorderWidth="1px" Width="62px" />
                                </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                               <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                               <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="12px" ForeColor="Black"
                               HorizontalAlign="Left" />
                               <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                               HorizontalAlign="Center" />
                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                    </asp:GridView>
   
   </td>
  </tr>
  
  <tr>
   <td align="center">
   
     <br />
     
    
       <asp:Button ID="Btn_Save" runat="server" Text="Save" class="btn btn-success"
           onclick="Btn_Save_Click" />
       
       &nbsp;&nbsp;
       
       <asp:Button ID="Btn_Retore" runat="server" Text="Reset"  OnClientClick="return confirm('You are about to restore default staff availability. Do you want to continue ?');" 
           onclick="Btn_Retore_Click" class="btn btn-primary"/>
       
       
       &nbsp;&nbsp;
       
       
      <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger" 
           onclick="Btn_Cancel_Click" />
   </td>
  </tr>
  <tr>
   <td align="center">
   
       <asp:Label ID="Lbl_Error" runat="server" Text="" ForeColor="Red"></asp:Label>
   
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

<asp:HiddenField ID="Hdn_StaffId" runat="server" />

 <WC:MSGBOX id="WC_MessageBox" runat="server" />
 <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  style="display:none;">   
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o" style="height: 58px"> </td>
            <td class="c" style="height: 58px" align="center" >
               
                 <asp:Label ID="Lbl_msgAlert" runat="server" Text="" Font-Bold="true"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-success" OnClientClick="pageback()"/>
                        </div>
            </td>
            <td class="e" style="height: 58px"> </td>
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
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
