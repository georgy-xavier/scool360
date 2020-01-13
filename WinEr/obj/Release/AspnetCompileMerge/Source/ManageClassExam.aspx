<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="ManageClassExam.aspx.cs" Inherits="WinEr.ManageClassExam" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="AGGREGATEPOPUP" Src="~/WebControls/NeedSubjectGroupPassMark.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" />


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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



    <div id="contents">
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate>
                <div id="right">
                    <div class="label">Exam Info</div>
                    <div id="SubExammngMenu" runat="server">
                    </div>
                </div>
                <div id="left">

                    <div class="container skin1">
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr>
                                <td class="no"></td>
                                <td class="n">Manage Class Exam</td>
                                <td class="ne"></td>
                            </tr>
                            <tr>
                                <td class="o"></td>
                                <td class="c">
                                    <div id="topstrip">

                                        <asp:Panel ID="Panel1" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <asp:Label ID="Lbl_ExamName" runat="server" ForeColor="White" class="control-label"
                                                            Text="University Exam"></asp:Label>
                                                    </td>
                                                    <td class="TblStrip2">
                                                        <asp:Label ID="Lbl_Examtypelb" runat="server" ForeColor="White" class="control-label"
                                                            Text="Exam Type"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lbl_ExamType" runat="server" ForeColor="White" class="control-label"
                                                            Text="MAIN"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>:</td>
                                                    <td>
                                                        <asp:Label ID="Lbl_freqlb" runat="server" class="control-label"
                                                            Text="Exam Frequency" ForeColor="White"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lbl_Frequency" runat="server" ForeColor="White" class="control-label"
                                                            Text="Monthly"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>



                                    </div>
                                    <br />

                                    <ajaxToolkit:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_yuitabview-theme"
                                        Width="100%" ActiveTabIndex="0">

                                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                                <asp:Image ID="Image3" runat="server" Height="18px" ImageUrl="Pics/add.png" Width="20px" />
                                                <b>CREATE</b>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Panel ID="panl1" runat="server" DefaultButton="Btn_Add">
                                                    <div style="min-height: 350px;">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" Text="Class Name : " Width="100px" class="control-label"></asp:Label></td>
                                                                <td>
                                                                    <asp:DropDownList ID="Drp_ClassName" runat="server" Width="125px" class="form-control"
                                                                        OnSelectedIndexChanged="Drp_ClassName_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList></td>
                                                                <td colspan="5"></td>
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
                                                                <td></td>
                                                                <td></td>
                                                                <td colspan="5"></td>
                                                            </tr>

                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label4" runat="server" Text="Subject:" class="control-label"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" Width="125px" ID="Drp_Sub" class="form-control">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Text="Pass Mark:" class="control-label"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_Min" runat="server" Width="75px" MaxLength="6" class="form-control"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" Enabled="True"
                                                                        runat="server"
                                                                        FilterType="Custom, Numbers" ValidChars="." TargetControlID="Txt_Min" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label2" runat="server" Text="Max Mark:" class="control-label"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_Max" runat="server" Width="75px" class="form-control" MaxLength="5"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender_SLA" Enabled="True"
                                                                        runat="server"
                                                                        FilterType="Custom, Numbers" ValidChars="." TargetControlID="Txt_Max" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="Btn_Add" runat="server" Text="Add" Class="btn btn-primary"
                                                                        OnClick="Btn_Add_Click" /></td>
                                                            </tr>


                                                            <tr>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td colspan="5">&#160;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:GridView ID="Grd_Exam"
                                                                        AutoGenerateColumns="False"
                                                                        BackColor="#EBEBEB"
                                                                        BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                                                                        CellPadding="3" CellSpacing="2" Font-Size="12px"
                                                                        Width="100%" OnSelectedIndexChanged="Grd_Exam_SelectedIndexChanged"
                                                                        runat="server">
                                                                        <Columns>
                                                                            <asp:CommandField HeaderText="Remove"
                                                                                SelectText="&lt;img src='Pics/DeleteRed.png' width='20px' height='20px' border=0 title='Select To Remove'&gt;"
                                                                                ShowSelectButton="True">
                                                                                <ItemStyle Font-Size="Smaller" Width="42px" />
                                                                            </asp:CommandField>
                                                                            <asp:BoundField DataField="Subject Id" HeaderText="Subject Id" />
                                                                            <asp:BoundField DataField="Subject Name" HeaderText="Subject Name/Subject code" />
                                                                            <asp:BoundField DataField="Max Mark" HeaderText="Max Mark" />
                                                                            <asp:BoundField DataField="Min Mark" HeaderText="Min Mark" />
                                                                            <asp:BoundField DataField="Subject Group Name" HeaderText="Subject Group" />
                                                                        </Columns>
                                                                        <EditRowStyle Font-Size="Medium" />
                                                                        <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                                                                        <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                                                                        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                                                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                                                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td colspan="5">&#160;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">&#160;</td>
                                                                <td>
                                                                    <asp:Button ID="Btn_Create" runat="server"
                                                                        OnClick="Btn_Create_Click" Text="Save" Visible="False" Class="btn btn-primary" /></td>
                                                                <td>
                                                                    <asp:Button ID="Btn_Cancel" runat="server"
                                                                        OnClick="Btn_Cancel_Click" Text="Cancel" Visible="False" Class="btn btn-primary" /></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>

                                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                                <asp:Image ID="Image1" runat="server" Height="18px" ImageUrl="Pics/edit.png" Width="20px" /><b>MANAGE</b></HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Panel ID="pnl32" runat="server" DefaultButton="Btn_AddNew">
                                                    <div style="min-height: 350px; width: 100%;">
                                                        <table class="style1">
                                                            <tr>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label9" runat="server" Text="Class Name : "></asp:Label></td>
                                                                <td>
                                                                    <asp:DropDownList ID="Drp_Clas" runat="server" AutoPostBack="True"
                                                                        class="form-control" OnSelectedIndexChanged="Drp_Clas_SelectedIndexChanged"
                                                                        Width="125px">
                                                                    </asp:DropDownList></td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                                <td>&#160;</td>
                                                            </tr>
                                                            <asp:Panel ID="PnlCntrls" runat="server">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label10" runat="server" Text="Subject : "></asp:Label></td>
                                                                    <td>
                                                                        <asp:DropDownList ID="Drp_Subj" runat="server" Width="125px" class="form-control"
                                                                            AutoPostBack="True" OnSelectedIndexChanged="Drp_Subj_SelectedIndexChanged">
                                                                        </asp:DropDownList></td>
                                                                    <td>
                                                                        <asp:Label ID="Label11" runat="server" Text="Pass Mark : " class="control-label"></asp:Label></td>
                                                                    <td>
                                                                        <asp:TextBox ID="Txt_Pass" runat="server" Width="125px" MaxLength="5" class="form-control"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="Txt_Pass_FilteredTextBoxExtender"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="."
                                                                            TargetControlID="Txt_Pass">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label12" runat="server" Text="Max Mark : "></asp:Label></td>
                                                                    <td>
                                                                        <asp:TextBox ID="Txt_Maxm" runat="server" Width="125px" MaxLength="5" class="form-control"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="Txt_Maxm_FilteredTextBoxExtender"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="."
                                                                            TargetControlID="Txt_Maxm">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td colspan="2">
                                                                        <asp:Button ID="Btn_AddNew" runat="server"
                                                                            OnClick="Btn_AddNew_Click" Text="Add" Class="btn btn-primary" />&#160;&#160;
                                                                        <asp:Button ID="Btn_Delete" runat="server"
                                                                            OnClick="Btn_Delete_Click" Text="Remove" Class="btn btn-danger" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td colspan="3">&#160;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6">
                                                                        <asp:GridView ID="Grd_EditExam" runat="server" AutoGenerateColumns="False"
                                                                            BackColor="#EBEBEB" BorderColor="#BFBFBF" AutoGenerateSelectButton="true"
                                                                            BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2"
                                                                            Font-Size="12px" OnSelectedIndexChanged="Grd_EditExam_SelectedIndexChanged"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="SubId" HeaderText="Subject Id" />
                                                                                <asp:BoundField DataField="subject_name" HeaderText="Subject Name/Code " />
                                                                                <asp:BoundField DataField="MinMark" HeaderText="Pass Mark" />
                                                                                <asp:BoundField DataField="MaxMark" HeaderText="Max Mark" />
                                                                            </Columns>
                                                                            <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                                            <EditRowStyle Font-Size="Medium" />
                                                                            <SelectedRowStyle BackColor="LightPink" ForeColor="Black" />
                                                                            <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td colspan="3">&#160;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td>&#160;</td>
                                                                    <td colspan="2">
                                                                        <asp:Button ID="Btn_DeleteClass" runat="server" Class="btn btn-danger"
                                                                            OnClick="Btn_DeleteClass_Click" Text="Delete" /><ajaxToolkit:ConfirmButtonExtender ID="BtnDeleteExam_ConfirmButtonExtender"
                                                                                runat="server" DisplayModalPopupID="Btn_DeleteClass_ModalPopupExtender"
                                                                                Enabled="True" TargetControlID="Btn_DeleteClass">
                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                        <ajaxToolkit:ModalPopupExtender ID="Btn_DeleteClass_ModalPopupExtender"
                                                                            runat="server" CancelControlID="ButtonCancel" OkControlID="ButtonOk"
                                                                            PopupControlID="PNL" TargetControlID="Btn_DeleteClass" />
                                                                        &#160;&#160;
                                                                        <asp:Button ID="Btn_Can" runat="server"
                                                                            OnClick="Btn_Cancel_Click" Text="Back" Class="btn btn-primary" /></td>
                                                                </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Lbl_Note" runat="server" ForeColor="Red" class="control-label" Font-Bold="true"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CXmId" runat="server" Visible="False" class="form-control"></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="PNL" runat="server" Style="display: none;">
                                                    <div class="container skin5" style="width: 400px; top: 400px; left: 400px">
                                                        <table cellpadding="0" cellspacing="0" class="containerTable">
                                                            <tr>
                                                                <td class="no">
                                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/elements/comment-edit-48x48.png"
                                                                        Height="28px" Width="29px" /></td>
                                                                <td class="n"><span style="color: White; font-size: large">alert!</span></td>
                                                                <td class="ne">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="o"></td>
                                                                <td class="c">Are you sure you want to Delete this Exam for this class?
                                                                    <br />
                                                                    <br />
                                                                    <div style="text-align: center;">
                                                                        <asp:Button ID="ButtonOk" runat="server" Text="Yes" class="btn btn-info" Width="50px" /><asp:Button ID="ButtonCancel" runat="server" Text="No" class="btn btn-info" Width="50px" /></div>
                                                                </td>
                                                                <td class="e"></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="so"></td>
                                                                <td class="s"></td>
                                                                <td class="se"></td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <br />
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>



                                    </ajaxToolkit:TabContainer>







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
                </div>

                <%--Message Box--%>
                <WC:MSGBOX ID="WC_MessageBox" runat="server" />
                <WC:AGGREGATEPOPUP ID="WC_Aggregategroup" runat="server" />
                <div class="clear"></div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</asp:Content>
