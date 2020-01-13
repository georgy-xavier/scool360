<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NeedSubjectGroupPassMark.ascx.cs" Inherits="WinEr.WebControls.NeedSubjectGroupPassMark" %>
<link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
<link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css"title="style"  media="screen"/>
<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/> <%--style="display:none"--%>
                        
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" 
                         CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                         PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
 
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;" DefaultButton="Btn_magok">
                         <div id="Popupwindow" runat="server" class="container skin5">
                         
    <table   cellpadding="0" cellspacing="0" class="containerTable" style="width:100%;">
        <tr >
            <td class="no"></td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Create Aggregate Subject Group"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c"  align="center">
            
             <asp:Label ID="Lbl_err" runat="server" Text="" ></asp:Label>


                <div id="HtmlDiv" runat="server">
                       
                       
                     &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                     Pass Mark : <asp:Label ID="Lbl_passmark" Font-Bold="true" Font-Size="Medium" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                
                      <asp:GridView ID="Grd_CCEstudent" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                                     BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="0.50px">
                        
                        <Columns>
                        <asp:TemplateField>
                        <HeaderTemplate>
                        <asp:CheckBox ID="ChkSelect" AutoPostBack="true" runat="server" OnCheckedChanged="ChkSelect_OnCheckedChanged"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_temselect" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_temselect_OnCheckedChanged"/>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="SubjectName" HeaderText="Subject Name" />
                        <asp:TemplateField HeaderText="Minimum Pass Mark" ItemStyle-Width="150">
                        <ItemTemplate>
                                        <asp:TextBox ID="Txt_Mark" runat="server"  MaxLength="3" Text="0" class="form-control" AutoPostBack="true"
                                             Width="100" OnTextChanged="Txt_Mark_TextChanged"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Mark_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_Mark" ValidChars=".a">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                        </asp:TemplateField>
                        
                        </Columns>
                        
                       </asp:GridView>
                 
                       
                </div>
                <br />
                <br />
                <div style="text-align:center;">
                            <asp:Button ID="Btn_save" Class="btn btn-info"  runat="server" Text="Save" onclick="Btn_save_Click"/>
                            <asp:Button ID="Btn_magok" Class="btn btn-danger" runat="server" Text="Cancel" />
                            <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text="Label" Visible="false"></asp:Label>
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
