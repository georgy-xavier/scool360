<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AddReligion.aspx.cs" Inherits="WinEr.AddReligion" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="progressBackgroundFilter">
            </div>
            <div id="processMessage">
                <table style="height: 100%; width: 100%">
                    <tr>
                        <td align="center">
                            <b>Please Wait...</b><br />
                            <br />
                            <img src="images/indicator-big.gif" alt="" />
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <div class="container">
                    <div class="col-lg-12">
                        <div class="row well" style="background-color: white;">
                            <div class="row">

                                <h4>Religion Manager</h4>


                            </div>
                        </div>
                        <div class="row well" style="background-color: white; display: flex; justify-content: center;">
                            <div class="row">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="Txt_Religion">Religion:</label>
                                        <asp:TextBox ID="Txt_Religion" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="alert-danger" ID="RequiredSubject" runat="server" ControlToValidate="Txt_Religion" ErrorMessage="Religion required" ValidationGroup="AddReligion"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <asp:LinkButton ID="Btn_AddReligion" CssClass="btn btn-primary" runat="server" ValidationGroup="AddReligion" OnClick="Btn_AddReligion_click"><span class="glyphicon glyphicon-plus">&nbsp;Add</span></asp:LinkButton>
                                    </div>
                                </div>
                                <asp:Label ID="Lbl_ReligionError" Text="" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            </div>
                        <div class="row well" style="background-color: white; display: flex; justify-content: center;">
                            <div class="col-lg-12">
                                <asp:Label ID="Lbl_ReligionErr" runat="server" Text=""  class="control-label" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="Grd_Religion" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="True" PageSize="20"
                                    OnRowDeleting="Grd_Religion_RowData_Delete" ForeColor="Black" GridLines="Vertical" OnPageIndexChanging="Grd_Religion_Category_PageIndexChanging"
                                    OnRowDataBound="Grd_Religion_RowDataBound" OnSelectedIndexChanged="Grd_ReligionSelectedIndexChanged"
                                    BackColor="#EBEBEB" Width="100%" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" CellSpacing="2" Font-Size="12px">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="Religion" HeaderText="Religion" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgEditReligion" runat="server" CommandName="Select" ImageUrl="~/Pics/edit.png" Width="30px" Height="30px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnDeleteReligion" runat="server" CommandName="Delete" ImageUrl="~/Pics/DeleteRed.png" Width="30px" Height="30px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                    <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                                    <EditRowStyle Font-Size="Medium" />
                                    <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />

                                    <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                        HorizontalAlign="Center" Height="20px" VerticalAlign="Middle" />
                                    <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" Font-Bold="False"
                                        HorizontalAlign="Left" VerticalAlign="Top" BorderStyle="Solid"
                                        BorderWidth="1px" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
             <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
            
        <div class="clear"></div>
         <asp:Button runat="server" ID="Btn_EditReligion" class="btn btn-info" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_EditReligion"  runat="server"  PopupControlID="Pnl_msg" TargetControlID="Btn_EditReligion"  CancelControlID="Btn_CancelReligion" BackgroundCssClass="modalBackground"/>
             <asp:Panel ID="Pnl_msg" runat="server"   style="display:none; min-height:300px;_height:300px;">
                <div class="container skin1" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Edit Religion</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" >
                            <asp:Label ID="Lbl_ReligionID" runat="server" Text="" class="control-label" Visible="false"></asp:Label><table width="100%">
                                    <tr>
                                        <td class="leftside">
                                            Religion Name
                                        </td>
                                        <td class="rightside">
                                            <asp:TextBox ID="Txt_EditReligionName" runat="server" Text="" class="form-control" Width="150px"></asp:TextBox>
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_EditReligionNameFilteredTextBoxExtender1" 
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                                        TargetControlID="Txt_EditReligionName">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                            </td></tr>
                                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                            
                                            <tr>
                                        <td></td>
                                        <td align="center">
                                            <asp:Button ID="Btn_UpdateReligion" runat="server" class="btn btn-success" Text="Update" OnClick="Btn_UpdateReligion_Click"  /> &nbsp;&nbsp;
                                            <asp:Button ID="Btn_CancelReligion" runat="server" Text="Cancel"  class="btn btn-danger"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="Lbl_EditReliigon_Error" class="control-label" runat="server" Text=""></asp:Label></td></tr></table></td><td class="e"> </td>
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
         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
