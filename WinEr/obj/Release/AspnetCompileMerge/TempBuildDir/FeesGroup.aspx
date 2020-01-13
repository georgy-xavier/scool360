<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="FeesGroup.aspx.cs" Inherits="WinEr.FeesGroup" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
            font-weight: lighter;
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/add_page.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">Add Fees Group Header</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  <asp:Panel ID="Pnl_Feesgroup" runat="server">
                      <table class="tablelist">
                      <tr>
                      <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>
                      <tr><td class="leftside">
                       Fees Group Name <span style="color:Red;">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_name" runat="server" Width="195px" class="form-control" MaxLength="50"></asp:TextBox>
                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_name_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_name" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                
                                                    
                      </td>
                      </tr>
                         
                     <tr><td class="leftside">
                         <asp:Label ID="Lbl_gateway" runat="server" class="control-label" Text="Payment Gateway"></asp:Label>
                      </td>
                      <td class="rightside">
                      <asp:DropDownList ID="Drplist_Gateway" runat="server" class="form-control" Width="200px">
                      </asp:DropDownList>        
                      </td>
                      </tr>
                      <tr><td class="leftside">
                        Need To Show in Parent Login
                      </td>
                      <td class="rightside">
                          <asp:CheckBox ID="Chkb_enable" runat="server" Checked="true" />
                      </td>
                      </tr>
                          <tr><td class="leftside">
                      Use Defalut Logo and Address
                      </td>
                      <td class="rightside">
                      <asp:CheckBox ID="Chkb_Default" runat="server" Checked="true" OnCheckedChanged="Chkb_Default_CheckedChanged" AutoPostBack="true" />
                      </td>
                      </tr>
                    </table>
                    <asp:Panel ID="Pnl_Logo" runat="server">
                    <table class="tablelist">
                      <tr>
                      <td class="leftside">
                      Add Logo
                      </td>
                      <td class="rightside">
                          <asp:FileUpload ID="Fileup_Logo" runat="server" />
                      </td>
                      </tr>
                      <tr>
                      <td class="leftside">
                      Address
                      </td>
                      <td class="rightside">
                     <asp:TextBox ID="Txt_Address" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                      </td>
                      </tr>
                      </table>
                       </asp:Panel>
                    <table class="tablelist">
                       <tr>
                      <td class="leftside"></td>
                      <td class="rightside">
                      </td>
                      </tr>
                      <tr>
                      <td class="leftside"></td>
                      <td class="rightside">
                        <asp:Button ID="Btn_Add" runat="server" Text="Add" Class="btn btn-info"
                                      onclick="Btn_Add_Click"  />
                                  &nbsp;&nbsp; 
                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
                                   onclick="Btn_Cancel_Click" Class="btn btn-danger"/>
                                  
                      </td>
                      </tr>
    
                      </table>
                    
                      <br/>
                      <center>
                      <asp:Label ID="Lbl_Msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
                      </center>
                      <br />
                <center>
                <asp:Panel ID="Pnl_Grid_Header" runat="server">
                <asp:GridView runat="server" ID="Grd_Fees_Header" AutoGenerateColumns="false" BackColor="#EBEBEB"
                     BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px" Width="100%" 
                     onpageindexchanging="Grd_Fees_Header_PageIndexChanging" 
                     AllowPaging="true"
                     PageSize="5" 
                     OnRowCommand="Grd_Fees_Header_RowCommand" >
			      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <Columns>   
                  <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="360px" />              
                  <asp:BoundField DataField="Name" HeaderText="Name"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>  
                  <asp:BoundField DataField="GateWay" HeaderText="GateWay"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                        <asp:buttonfield buttontype="Link" commandname="select"  text="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                                 ItemStyle-ForeColor="Black"  HeaderText="Edit"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>
                                 <asp:buttonfield buttontype="Link"  commandname="Remove"  text="&lt;img src='Pics/DeleteRed.png' width='40px' border=0 title='Select to View'&gt;"
                                 ItemStyle-ForeColor="Black"  HeaderText="Remove"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>

                  </Columns>                  
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
			    </asp:GridView>
                 </asp:Panel>
                  </center>
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
    
                      <asp:Button runat="server" ID="Btn_PopUp" style="display:none"/>
	                        <ajaxToolkit:ModalPopupExtender ID="MPE_FeesGroupPopUp"   runat="server" CancelControlID="Btn_Cancel" 
	                        PopupControlID="Pnl_FeesGroupPopUp" TargetControlID="Btn_PopUp" BackgroundCssClass="modalBackground" />
	                            <asp:Panel ID="Pnl_FeesGroupPopUp" runat="server" style="display:none">
                                    <div class="container skin5" style="width:700px; top:400px;left:200px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image1" runat="server" 
                                                         ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:White">Edit Fees Group</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >             
                                                   <asp:Panel ID="Pnl_edt_fees_group" runat="server">
                      <table class="tablelist">
                       <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>

                      <tr><td class="leftside">
                       Fees Group Name <span style="color:Red;">*</span>
                      </td>
                      <td class="rightside">
                       <asp:TextBox ID="Txt_fees_group" runat="server" Width="195px" class="form-control" MaxLength="50"></asp:TextBox>
                         <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                  runat="server" Enabled="True" TargetControlID="Txt_fees_group" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                
                                                    
                      </td>
                      </tr>
                      <tr>
                      <td class="leftside">
                        <asp:Label ID="Lbl_Edt_Gateway" runat="server" class="control-label" Text="Payment Gateway"></asp:Label>
                      </td>
                      <td class="rightside">
                          <asp:DropDownList ID="Drplist_edt_Gateway" runat="server" class="form-control" Width="200px">
                          </asp:DropDownList>
                           
                      </td>
                      </tr> 
                  
                      <tr><td class="leftside">
                        Need To Show in Parent Login
                      </td>
                      <td class="rightside">
                          <asp:CheckBox ID="Chkb_Edt_Parent" runat="server" Checked="true" />
                      </td>
                      </tr>
                        <tr><td class="leftside">
                        Use Defalut Logo and Address
                      </td>
                      <td class="rightside">
                          <asp:CheckBox ID="Chkbedt_default" runat="server" AutoPostBack="true" OnCheckedChanged="Chkbedt_default_OnCheckedChanged" />
                      </td>
                      </tr>
                      <tr>
                      <td class="leftside">
                        Address
                      </td>
                      <td class="rightside">
                          <asp:TextBox ID="Txt_Edt_Address" runat="server" Width="200px" class="form-control" TextMode="MultiLine"></asp:TextBox>
                      </td>
                      </tr>
                      <tr>
                      <td class="leftside">
                      </td>
                      <td class="rightside">
                      
                          
                          <table>
                          <tr>
                          <td>
                         
                          <asp:Image ID="Img_Logo" runat="server" Height="150px" Width="150px" />
                          
                          </td>
                          </tr>
                           <tr>
                          <td>
                          
                          <asp:LinkButton ID="lnk_changeLogo" runat="server" Text="Change Photo" OnClick="lnk_changeLogo_click"></asp:LinkButton>
                          
                          </td>
                          </tr>
                          <tr>
                          <td >
                          <asp:Panel ID="pnl_Edt_Logo" runat="server">
                          <asp:FileUpload ID="FileUp_Photo" runat="server"  />
                           </asp:Panel>
                       
                          </td>
                          </tr>
                     
                          </table>
                         
                          </td>
                          </tr>
                       
                     
                      <tr>
                      <td class="leftside"></td>
                      <td class="rightside">
                        <asp:Button ID="Btn_Update" runat="server" Text="Save" Class="btn btn-info"
                                      onclick="Btn_Update_Click"  />
                                  &nbsp;&nbsp; 
                        <asp:Button ID="Btn_Clear" runat="server" Text="Cancel" 
                                   onclick="Btn_Clear_Click" Class="btn btn-info"/>
                                  
                      </td>
                      </tr>
    
                      </table>
                      <center>
                      <br/>
                      <asp:Label ID="Lbl_Edt_Msg" runat="server" Text=""  class="control-label" ForeColor="Red"></asp:Label>
                      <br />
                      </center>
                  </asp:Panel>
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
                                 <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
    
</ContentTemplate>
 <Triggers>
  <asp:PostBackTrigger  ControlID="Btn_Add"/>
    <asp:PostBackTrigger  ControlID="Btn_Update"/>
 </Triggers>
 </asp:UpdatePanel>
</div>

</asp:Content>
