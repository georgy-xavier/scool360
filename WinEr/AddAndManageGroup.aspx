<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AddAndManageGroup.aspx.cs" Inherits="WinEr.AddAndManageGroup" %>

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

                <table style="height: 100%; width: 100%">

                    <tr>

                        <td align="center">

                            <b>Please Wait...</b><br />

                            <br />

                            <img src="images/indicator-big.gif" alt="" /></td>

                    </tr>

                </table>

            </div>


        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div id="contents">
                <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable" width="900px">
                        <tr>
                            <td class="no"></td>
                            <td class="n">Manage Group</td>
                            <td class="ne"></td>
                        </tr>
                        <tr>
                            <td class="o"></td>
                            <td class="c">


                                <asp:Panel ID="Pnl_TopArea" runat="server">
                                    <table class="tablelist" width="100%">
                                        <tr>
                                            <td align="left">
                                                <img id ="Img_AddUser" runat="server" alt="" src="Pics/add.png" width="25" height="20"/>
                                               <%-- <asp:Image ID="Img_AddUser" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" Style="vertical-align: top" />--%>
                                                <asp:LinkButton ID="Lnk_AddNewGroup" runat="server" Text="Add New Group" OnClick="Lnk_AddNewGroup_Click"></asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Pnl_AddNewGroup" runat="server">
                                                    <tr>
                                                        <td class="leftside">Group Name</td>
                                                        <td class="rightside">
                                                            <asp:TextBox ID="Txt_GroupName" runat="server" Width="200px" class="form-control" MaxLength="15"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="leftside">
                                                            <br />
                                                        </td>
                                                        <td class="rightside">
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="leftside">Description</td>
                                                        <td class="rightside">
                                                            <asp:TextBox ID="Txt_GroupDescription" runat="server" class="form-control" Width="200px" TextMode="MultiLine" MaxLength="150"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="leftside"></td>
                                                        <td class="rightside">
                                                            <asp:CheckBox ID="Chk_AddMoreGroup" runat="server" Text="Add more group" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="leftside"></td>
                                                        <td class="rightside">
                                                            <asp:Button ID="Btn_Add" runat="server" Text="Add"
                                                                Class="btn btn-primary" OnClick="Btn_Add_Click" />
                                                            <asp:Button ID="Btn_Update" runat="server" Text="Update"
                                                                Class="btn btn-primary" OnClick="Btn_Update_Click" />
                                                            <asp:HiddenField ID="Hdn_GroupId"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                                <div class="linestyle"></div>
                                <br />
                                <asp:Panel ID="Pnl_Show" runat="server">
                                    <table width="100%">
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <tr>
                                            <td align="right"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                            <asp:GridView ID="Grd_Group" runat="server" 
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True" 
                      PageSize="10"   Width="100%" onpageindexchanging="Grd_Group_PageIndexChanging"
                              OnSelectedIndexChanged="Grd_Group_SelectedIndexChanged" 
                               OnRowDeleting="Grd_Group_RowDeleting" >
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="Id" HeaderText="Id"/>
                      <asp:BoundField DataField="GroupName" HeaderText="Group Name"/>  
                        <asp:BoundField DataField="Description" HeaderText="Description"/>                     
                        <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="100px"
                       ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller"  ItemStyle-HorizontalAlign="Center"
                       SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                              ShowSelectButton="True">
                            <ControlStyle />
                                    <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                        </asp:CommandField>
                         <asp:TemplateField HeaderText="Delete" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                   <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                        </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                   </asp:TemplateField>
                      
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <WC:MSGBOX ID="WC_MessageBox" runat="server" />


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

                <div class="clear"></div>
            </div>
        </ContentTemplate>
        <%-- <Triggers>
 <asp:PostBackTrigger ControlID="Btn_Excel" />
 </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
