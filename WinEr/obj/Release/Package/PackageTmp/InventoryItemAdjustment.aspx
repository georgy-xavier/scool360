<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryItemAdjustment.aspx.cs" Inherits="WinEr.InventoryItemAdjustment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Inventory Item Adjustment</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <asp:Panel ID="Pnl_ItemAdjustment" runat="server">
                <table width="100%" class="tablelist">
                <tr>
                <td class="leftside">ItemName</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_ItemName" runat="server" Width="153px"></asp:DropDownList>
                </td>
                </tr>               
                <tr>
                <td class="leftside">Count</td> 
                <td class="rightside"><asp:TextBox ID="Txt_Count" Width="150px" runat="server"  
                       ></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="Flt_txtpurcasecount" 
                       runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="Txt_Count"> </cc1:FilteredTextBoxExtender>
                       </td>
                </tr>       
                 <tr>
                <td class="leftside">Location</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_Location" runat="server" Width="153px" 
                        onselectedindexchanged="Drp_Location_SelectedIndexChanged" 
                        AutoPostBack="True"></asp:DropDownList>
                </td>
                </tr>  
                <tr id="Row_lctnItemCount" runat="server">
                <td class="leftside">Available Item Count</td> 
                <td class="rightside"><asp:TextBox ID="Txt_LctnItemCount" Width="150px" runat="server"  
                       ></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                       runat="server" Enabled="True" FilterType="Numbers"  TargetControlID="Txt_Count"> </cc1:FilteredTextBoxExtender>
                       </td>
                </tr>        
                <tr> 
                <td class="leftside">Select type</td>
                <td class="rightside">
                <asp:RadioButtonList ID="Rdb_Type" runat="server" RepeatDirection="Vertical" 
                        AutoPostBack="True" 
                        onselectedindexchanged="Rdb_Type_SelectedIndexChanged" >
                <asp:ListItem Text="Adjustment/Decrease" Value="1"></asp:ListItem>
                <asp:ListItem Text="Adjustment/Increase" Value="0"></asp:ListItem>
                
                </asp:RadioButtonList></td>
                </tr> 
                <tr>
                <td class="leftside">Reason</td>
                <td class="rightside"><asp:TextBox ID="Txt_reason" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Label ID="Lbl_AdjustErr" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>     
                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_AdjustItem" runat="server" Width="90px" 
                        Text="Save" onclick="Btn_AdjustItem_Click" />
                </td>
                </tr>    
                </table>
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

<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  DefaultButton="Btn_magok" style="display:none;" ><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_MsgText" runat="server" Text="Message"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:HiddenField ID="Hdn_ItemID" runat="server" />
                            <asp:Button ID="Btn_magok" runat="server" Text="Yes" Width="50px"/>
                            <asp:Button ID="Btn_MsgCancel" runat="server" Text="No" Width="50px" onclick="Btn_MsgCancel_Click" 
                                />
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
  </ContentTemplate>
  <%--<Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  </Triggers>--%>
  </asp:UpdatePanel>

<div class="clear"></div>
</asp:Content>
