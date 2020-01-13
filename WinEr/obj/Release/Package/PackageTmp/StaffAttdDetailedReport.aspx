<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StaffAttdDetailedReport.aspx.cs" Inherits="WinEr.StaffAttdDetailedReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .TableHeaderStyle {
            border-color: #dacdcd;
            border-style: solid;
            border-width: 1px;
            background-color: #eeeeee;
            font-weight: bold;
            color:#080808;
            text-align: center;
            padding: 10px 10px 10px 10px;
        }

        .SubHeaderStyle {
            background-color: white;
            color: black;
            font-weight: bolder;
            border-color: #dacdcd;
            border-style: solid;
            border-width: 1px;
            padding-left: 10px;
            padding-right: 10px;
            text-align: left;
            min-width: 180px;
        }

        .CellStyle {
            border-color: #dacdcd;
            border-style: solid;
            border-width: 1px;
            padding-left: 10px;
            color: #333333;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">

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

            <div class="container skin1">
                <div class="row well" style="background-color: white;">
                    <div class="row">

                        <h4>Staff Attendance Report</h4>

                        <hr />
                    </div>
                    <div id="wrningMsg" runat="server" class="errmsgSH alert alert-danger alert-dismissable" visible="false">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
                        <strong>Something went wrong ! </strong>
                        <asp:Label ID="Lbl_wrningMsg" runat="server"></asp:Label>
                    </div>
                    <div class="row" style="background-color: white; display: grid; justify-content: center;">
                        <div class="form-group">
                            <div class="col-md-6">
                                <div class="form-inline">
                                    <label class="control-label">Time Period</label>
                                    <asp:DropDownList ID="Drp_Select_Period" runat="server" AutoPostBack="True" class="form-control"
                                        OnSelectedIndexChanged="Drp_Select_Period_SelectedIndexChanged">
                                        <asp:ListItem>Today</asp:ListItem>
                                        <asp:ListItem>Last Week</asp:ListItem>
                                        <asp:ListItem>Month Wise</asp:ListItem>
                                        <asp:ListItem>Manual</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-inline">
                                    <label class="control-label">Select Month</label>
                                    <asp:DropDownList ID="Drp_Select_Month" runat="server" AutoPostBack="True" class="form-control"
                                        OnSelectedIndexChanged="Drp_Select_Month_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <label class="control-label">Start Date</label>
                                <asp:TextBox ID="Txt_SDate" runat="server" class="form-control" Style="width: 160px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="Txt_SDate_MaskedEditExtender" runat="server"
                                    MaskType="Date" CultureName="en-GB"
                                    Mask="99/99/9999"
                                    UserDateFormat="DayMonthYear"
                                    Enabled="True"
                                    TargetControlID="Txt_SDate" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                    CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                    ControlToValidate="Txt_SDate"
                                    Display="None"
                                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                    TargetControlID="DobDateRegularExpressionValidator3"
                                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="Txt_SDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="control-label">End Date</label>
                                <asp:TextBox ID="Txt_EDate" runat="server" class="form-control" Style="width: 160px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="Txt_EDate_MaskedEditExtender" runat="server"
                                    MaskType="Date" CultureName="en-GB"
                                    Mask="99/99/9999"
                                    UserDateFormat="DayMonthYear"
                                    Enabled="True"
                                    TargetControlID="Txt_EDate" CultureAMPMPlaceholder="AM;PM"
                                    CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY"
                                    CultureDatePlaceholder="/" CultureDecimalPlaceholder="."
                                    CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:RegularExpressionValidator runat="server" ID="Txt_EDate_RegularExpressionValidator1"
                                    ControlToValidate="Txt_EDate"
                                    Display="None"
                                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender4"
                                    TargetControlID="Txt_EDate_RegularExpressionValidator1"
                                    HighlightCssClass="validatorCalloutHighlight" Enabled="True" />

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="Txt_EDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group">
                                <asp:Button ID="Btn_show" runat="server" Text="Show" class="btn btn-primary" OnClick="Btn_show_Click" />
                                &nbsp;
                                <asp:Button ID="Btn_Excel" runat="server" OnClick="Btn_Excel_Click"
                                    Text="Export" Class="btn btn-primary" />
                            </div>
                            <div id="errmsg" runat="server" class="errmsgSH alert alert-danger alert-dismissable" visible="false">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
                                <strong>Error ! </strong>
                                <asp:Label ID="Lbl_Err" runat="server"></asp:Label>
                            </div>
                            <%--<asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label>--%>

                        </div>
                        <div>
                            <asp:Panel ID="Pnl_ExamResults" runat="server">
                                <div style="width: 1050px; overflow: auto">
                                    <div id="GridDiv" runat="server">
                                    </div>

                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Excel" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
