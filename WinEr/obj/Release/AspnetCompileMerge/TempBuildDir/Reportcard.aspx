﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Reportcard.aspx.cs" Inherits="WinEr.Reportcard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage">
                <table style="height: 100%; width: 100%">
                    <tr>
                        <td align="center"><b>Please Wait...</b><br />
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
                <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no"></td>
                            <td class="n"> Report Card S1 </td>
                            <td class="ne"></td>
                        </tr>
                        <tr>
                            <td class="o"></td>
                            <td class="c">
                                <asp:Panel ID="Pnl_All" runat="server">
                                    <div align="center">
                                        <table width="100%">
                                            <tr>
				                    
				                    <td class="leftside">
                                        &nbsp;</td>
				                    <td class="rightside">
                                        &nbsp;</td>
				                   </tr>
                                            <tr>
                                                <td class="leftside">
                                                    <asp:Label ID="lbl_Selectclass" runat="server" style="margin:20px" Text="Select Class" class="control-label"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="Drp_SelectClass" runat="server" class="form-control" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="Drp_SelectClass_SelectedIndexChanged"></asp:DropDownList>
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
                                                    <asp:Label ID="Lbl_Reportname" runat="server" style="margin:20px" Text="Report Name" class="control-label" ></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_ReportName" runat="server" Width="200px" class="form-control" Text="Report Card"></asp:TextBox>

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
                                                    <asp:Label ID="Lbl_selectstudent" runat="server" style="margin:20px" Text="Select Student" class="control-label"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="Drp_SelectStudent" runat="server" class="form-control" Width="200px"></asp:DropDownList>
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
                                                <td></td>
                                                <td align="left">
                                                    <asp:Button ID="Btn_ExamReport" runat="server" Text="Create Report" class="btn btn-primary" OnClick="Btn_ExamReport_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Label ID="Lbl_Err" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

