<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GroupApplicationFrom.aspx.cs" Inherits="Scool360student.GroupApplicationFrom" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager2" runat="server" />
    <div id="contents">

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
                <asp:Panel ID="panel_application" CssClass="row" runat="server">

                    <div class="container skin1">
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr>
                                <td class="no"></td>
                                <td class="n">Group Application Form</td>
                                <td class="ne"></td>
                            </tr>
                            <tr>
                                <td class="o"></td>
                                <td class="c">

                                    <table class="tablelist">
                                        <tr>

                                            <td class="leftside">&nbsp;</td>
                                            <td class="rightside">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="leftside">
                                                <asp:Label ID="Label1" runat="server" Text="Id" class="control-label"></asp:Label>
                                            </td>
                                            <td class="rightside">
                                                <asp:TextBox ID="Txt_ExamName" runat="server" MaxLength="25" ValidationGroup="Group" class="form-control"
                                                    Width="180px" ReadOnly='true'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtenderWFName"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars"
                                                    InvalidChars="'/\~`!@#$%^&amp;*()-=+{}[]|;:&gt;&lt;,.?"
                                                    TargetControlID="Txt_ExamName" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Group" ControlToValidate="Txt_ExamName" ErrorMessage="Enter Group Name"></asp:RequiredFieldValidator>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="leftside">
                                                <asp:Label ID="Label4" runat="server" Text="Name" class="control-label"></asp:Label>
                                            </td>
                                            <td class="rightside">
                                                <asp:TextBox ID="TextBox1" runat="server" MaxLength="25" ValidationGroup="Group" class="form-control"
                                                    Width="180px" ReadOnly='true'></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars"
                                                    InvalidChars="'/\~`!@#$%^&amp;*()-=+{}[]|;:&gt;&lt;,.?"
                                                    TargetControlID="Txt_ExamName" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Group" ControlToValidate="TextBox1" ErrorMessage="Enter Group Name"></asp:RequiredFieldValidator>

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

                                            <td class="leftside">
                                                <asp:Label ID="Label3" runat="server" Text="Group Type " class="control-label"></asp:Label>
                                            </td>
                                            <td class="rightside">
                                                <asp:DropDownList ID="Drp_PeriodType" runat="server" Width="180px" class="form-control">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>



                                        <tr>
                                            <td class="leftside"></td>
                                            <td class="rightside">
                                                <asp:Button ID="Btn_Create" runat="server" Text="Create" Class="btn btn-success" ValidationGroup="Group" OnClick="Btn_Create_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" />
                                            </td>
                                        </tr>
                                    </table>

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

                </asp:Panel>

                <asp:Panel ID="PanelApproval" CssClass="row" runat="server" Visible="false">



                    <div class="well" style="background-color: white; display: flex; justify-content: center;">
                        <div class="row">
                            <div class="form-group" >
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Image ID="Img_Add" ImageUrl="~/Pics/add.png" Width="25px" Height="20px" runat="server" />
                                        <asp:LinkButton ID="lnk_grpsubmit" runat="server" CssClass="grayadd"
                                            Height="22px" OnClick="lnk_grpsubmit_Click">Change Request</asp:LinkButton>
                                      
                                    </td>
                                </tr>
                             
                            </table>
                                </div>
                            <br>
                            </br>
                            <div class="form-group">
                                <label for="Txt_Subject">Application waiting for approval</label>

                            </div>


                        </div>
                    </div>


                </asp:Panel>

                  <asp:Panel ID="pnl_grpapprvd" CssClass="row" runat="server" Visible="false">



                    <div class="well" style="background-color: white; display: flex; justify-content: center;">
                        <div class="row">
                            <div class="form-group" >
                            <table width="100%">
                                <tr>
                                  
                                </tr>
                             
                            </table>
                                </div>
                            <br>
                            </br>
                            <div class="form-group">
                                <label for="Txt_Subject">You are already part of a group</label>

                            </div>


                        </div>
                    </div>


                </asp:Panel>

                <WC:MSGBOX ID="WC_MessageBox" runat="server" />
                <asp:Panel ID="Pnl_CancelBill" runat="server">
                    <asp:Button runat="server" ID="Button1" class="btn btn-danger" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="MPE_CancelBill" runat="server" CancelControlID="Btn_NewCancelCancel" BackgroundCssClass="modalBackground"
                        PopupControlID="Pnl_Bill" TargetControlID="Btn_Cancel" />
                    <asp:Panel ID="Pnl_Bill" runat="server" Style="display: none;">
                        <div class="container skin5" style="width: 400px; top: 400px; left: 400px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr>
                                    <td class="no"></td>
                                    <td class="n"><span style="color: White">Message</span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="o"></td>
                                    <td class="c">
                                        <br />
                                        <asp:Label ID="Lbl_BillMessage" runat="server" class="control-label" Text=""></asp:Label>

                                        <br />
                                        <div style="text-align: center;">
                                            <asp:Button ID="Btn_CancelBill" OnClick="Btn_CancelBill_Click" runat="server" Text="Ok" ValidationGroup="CancelFeeBillReason" Class="btn btn-info" />


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
                            <br />
                            <br />
                        </div>
                    </asp:Panel>
                </asp:Panel>





            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="clear"></div>
    </div>

</asp:Content>

