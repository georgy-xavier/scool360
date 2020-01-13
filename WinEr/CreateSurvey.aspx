 <%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateSurvey.aspx.cs" Inherits="WinEr.CreateSurvey" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
         $(function () {
            $("#datepicker").datepicker({
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());;
        });

        $(function () {
        $("#datepicker1").datepicker({
            autoclose: true,
            todayHighlight: true
        }).datepicker('update', new Date());;
        });



    </script>
    <script type="text/javascript">
    function DigitsOnly(e)
    {
        if(((e.keyCode >= 48) && (e.keyCode <= 57)) || ((e.keyCode >= 97) && (e.keyCode <= 122)) || ((e.keyCode >= 65) && (e.keyCode <= 90)) || (e.keyCode == 32))
        {
            return true;
        }
        else
        {
            e.preventDefault();
            alert("Special Characters not allowed");
            return false;
        }
    }
    function DeleteConfirmation()
    {
         if (confirm("Are you sure,you want to delete selected record ?")==true)
         return true;
         else
         return false;
    }

</script>
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
                <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no"></td>
                            <td class="n">Create Survey</td>
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
                                            <asp:Label ID="lbl_Surveyname" runat="server" Text="Survey Name " class="control-label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Surveyname" runat="server" class="form-control" Width="180px" onkeypress="DigitsOnly(event)" MaxLength="25" autocomplete="off"></asp:TextBox>
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
                                            <asp:Label ID="lbl_Lastdate" runat="server" Text="Last Date " class="control-label"></asp:Label>
                                        </td>
                                        <td>


                                                                <asp:TextBox ID="Txt_SurveyDate" runat="server" class="form-control" Width="180px"></asp:TextBox>
                                                                <ajaxToolkit:MaskedEditExtender ID="Txt_SurveyDate_MaskedEditExtender" runat="server"
                                                                    MaskType="Date" CultureName="en-GB" AutoComplete="true"
                                                                    Mask="99/99/9999"
                                                                    UserDateFormat="DayMonthYear"
                                                                    Enabled="True"
                                                                    TargetControlID="Txt_SurveyDate">
                                                                </ajaxToolkit:MaskedEditExtender>
                                                                <span style="color: Blue">DD/MM/YYYY</span>

                                                              <%-- //<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_SurveyDate" ErrorMessage="You Must enter D.O.B"></asp:RequiredFieldValidator>--%>

                                                                <asp:RegularExpressionValidator runat="server" ID="Txt_SurveyDateDateRegularExpressionValidator3"
                                                                    ControlToValidate="Txt_SurveyDate"
                                                                    Display="None"
                                                                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                                                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                                                    TargetControlID="Txt_SurveyDateDateRegularExpressionValidator3"
                                                                    HighlightCssClass="validatorCalloutHighlight" />
                                                                <br />

                                          <%--   <div id="datepicker1" class="input-group date"  data-date-format="dd-mm-yyyy" style="width: 180px;">
                                                                    <asp:TextBox ID="Txt_SurveyDate" runat="server" class="form-control" Width="180px" TabIndex="12"></asp:TextBox>
                                                                    <ajaxToolkit:MaskedEditExtender ID="Txt_SurveyDate_MaskedEditExtender1" runat="server"
                                                                        MaskType="Date" CultureName="en-GB" AutoComplete="true"
                                                                        Mask="99-99-9999"
                                                                        UserDateFormat="DayMonthYear"
                                                                        Enabled="True"
                                                                        TargetControlID="Txt_SurveyDate">
                                                                    </ajaxToolkit:MaskedEditExtender>
                                                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>

                                                                </div>--%>



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
                                        <td class="leftside">&nbsp;</td>
                                        <td class="rightside">
                                            <asp:Button ID="Btn_Add" runat="server" Text="Add" Class="btn btn-success" OnClick="Btn_Add_Click" />&nbsp;&nbsp;
                                            <%--<asp:Button ID="Btn_clear" runat="server" Text="Clear" Class="btn btn-success" OnClick="Btn_clear_Click" />--%>
                                            <asp:Button ID="Btn_Update" runat="server" Text="Update" Class="btn btn-primary" Visible="false" OnClick="Btn_Update_Click" TabIndex="3" />
                                            <asp:HiddenField ID="SurveyId" runat="server" />
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
                                              <%--  <asp:Label ID="Lbl_err" runat="server" class="control-label" style=align:center ForeColor="Red"></asp:Label>--%>
                                            <br />
                                        </td>
                                        <td class="rightside">
                                            <br />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="Pnl_Housedisplay" runat="server">
                                        <div class="linestyle"></div>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Lbl_House" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <asp:GridView ID="Grd_SurveyCreated" runat="server"
                                                        AutoGenerateColumns="false" BackColor="#EBEBEB"
                                                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"
                                                        CellPadding="3" CellSpacing="2" Font-Size="15px" AllowPaging="True"
                                                        PageSize="10" Width="100%" OnSelectedIndexChanged="Grd_SurveyCreated_SelectedIndexChanged" OnPageIndexChanging="Grd_SurveyCreated_PageIndexChanging" OnRowDeleting="Grd_SurveyCreated_RowDeleting">

                                                        <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                        <EditRowStyle Font-Size="Medium" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                                            <asp:BoundField DataField="Survey_name" HeaderText="Survey Name" />
                                                            <asp:BoundField DataField="Last_Date" HeaderText="Last Date" />
                                                            <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="100px"
                                                                ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center"
                                                                SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                                                                ShowSelectButton="True">
                                                                <ControlStyle />
                                                                <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" OnClientClick="return DeleteConfirmation()" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ControlStyle ForeColor="#FF3300" />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                            HorizontalAlign="Left" />
                                                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                            HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                </td>
                        </tr>
                                            <div style="text-align:center">
                                                <asp:Label ID="Lbl_err" runat="server" class="control-label" style=align:center ForeColor="Red"></asp:Label>
                                            </div>
                                            
                        </table>
                   </asp:Panel>
                    </td>
                            <tr>
                                <td class="e"></td>
                            </tr>
                            <tr>
                                <td class="so"></td>
                                <td class="s"></td>
                                <td class="se"></td>
                            </tr>
                        </tr>
                         
                    </table>
                       
                </div>
                <WC:MSGBOX ID="WC_MessageBox" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="clear"></div>
    </div>
</asp:Content>
