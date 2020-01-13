<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="CreateNewStudent.aspx.cs" Inherits="WinEr.CreateNewStudent" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .redcol {
            color: Red;
        }

        .style3 {
            font-size: small;
            width: 100%;
        }
    </style>


    <script type="text/javascript">
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_Fees.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }

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
        function modalpopup() {
            $('#modalmessage').modal('show');
        }
        function myFunction() {
            window.location = "CreateNewStudent.aspx";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="contents">


        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
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

                            <td class="no">
                                <img alt="" src="Pics/add_female_user.png" width="35" height="35" />
                            </td>
                            <td class="n">Create Student</td>
                            <td class="ne"></td>
                        </tr>
                        <tr>
                            <td class="o"></td>
                            <td class="c">




                                <asp:Panel ID="Pnl_mainarea" runat="server">

                                    <asp:Wizard ID="Wzd_StudCreation" runat="server" ActiveStepIndex="0"
                                        Width="100%" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderWidth="1px"
                                        CancelButtonImageUrl="~/Pics/delete_page.png" CancelButtonType="Image"
                                        CancelDestinationPageUrl="~/CreateNewStudent.aspx" DisplayCancelButton="True"
                                        FinishCompleteButtonImageUrl="~/Pics/save.png" FinishCompleteButtonType="Image"
                                        FinishPreviousButtonImageUrl="~/Pics/back.png" FinishPreviousButtonType="Image"
                                        Font-Names="Verdana" Font-Size="0.8em"
                                        StartNextButtonImageUrl="~/Pics/next.png" StartNextButtonType="Image"
                                        StepNextButtonImageUrl="~/Pics/next.png" StepNextButtonType="Image"
                                        StepPreviousButtonImageUrl="~/Pics/back.png"
                                        StepPreviousButtonType="Image" EnableTheming="True"
                                        OnFinishButtonClick="Wzd_StudCreation_FinishButtonClick">
                                        <StepStyle Font-Size="0.8em" ForeColor="#333333" />
                                        <StartNextButtonStyle Height="40px" />
                                        <FinishCompleteButtonStyle Height="40px" />
                                        <StepNextButtonStyle Height="40px" />
                                        <FinishPreviousButtonStyle Height="40px" />
                                        <WizardSteps>
                                            <asp:WizardStep ID="WizardBasic" runat="server" Title="Basic Details">
                                                <asp:Panel ID="Pnl_Basicdetails" runat="server">


                                                    <table class="style3" cellspacing="5">
                                                        <tr>
                                                            <td style="width: 50px;">&nbsp;</td>
                                                            <td>
                                                                <asp:HiddenField ID="Hdn_ststus" runat="server" Value="0" Visible="false" />
                                                                <asp:HiddenField ID="Hdn_Studid" runat="server" Value="" Visible="false" />
                                                            </td>
                                                            <td align="right">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="Img_AddTempStd" runat="server" class="form-button" ValidationGroup="Tempsearch" Visible="false"
                                                                                ImageUrl="~/Pics/search_female_user.png" Width="45px" Height="45px"
                                                                                OnClick="Img_AddTempStd_Click" ToolTip="Add from Temp list " /></td>
                                                                        <td>
                                                                            <asp:LinkButton ID="Lnk_AddTemp" runat="server" ValidationGroup="Tempsearch" Visible="false" OnClick="Lnk_AddTemp_Click">Registered List</asp:LinkButton></td>
                                                                    </tr>
                                                                </table>



                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50px;">&nbsp;</td>
                                                            <td>Student Name (Full)<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_Name" runat="server" class="form-control" MaxLength="50" TabIndex="1"
                                                                    Width="250px"></asp:TextBox>

                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\" TargetControlID="Txt_Name">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_Name" ErrorMessage="You Must enter a name"></asp:RequiredFieldValidator>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>Sex <span class="redcol">*</span></td>
                                                            <td>

                                                                <div class="radio radio-primary">
                                                                    <asp:RadioButtonList ID="RadioBtn_Sex" class="form-actions" runat="server"
                                                                        RepeatDirection="Horizontal" TabIndex="2" Width="160px">
                                                                        <asp:ListItem Selected="True">Male</asp:ListItem>
                                                                        <asp:ListItem>Female</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </div>


                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>D.O.B<span class="redcol">*</span></td>
                                                            <td>


                                                                <asp:TextBox ID="Txt_Dob" runat="server" class="form-control" Width="250px" TabIndex="3"></asp:TextBox>
                                                                <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"
                                                                    MaskType="Date" CultureName="en-GB" AutoComplete="true"
                                                                    Mask="99/99/9999"
                                                                    UserDateFormat="DayMonthYear"
                                                                    Enabled="True"
                                                                    TargetControlID="Txt_Dob">
                                                                </ajaxToolkit:MaskedEditExtender>
                                                                <span style="color: Blue">DD/MM/YYYY</span>

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Dob" ErrorMessage="You Must enter D.O.B"></asp:RequiredFieldValidator>

                                                                <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                                                    ControlToValidate="Txt_Dob"
                                                                    Display="None"
                                                                    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                                    ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                                                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                                                    TargetControlID="DobDateRegularExpressionValidator3"
                                                                    HighlightCssClass="validatorCalloutHighlight" />
                                                                <br />



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
                                                            <td>&nbsp;</td>
                                                            <td>Father/Guardian Name <span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_FGName" runat="server" class="form-control" Width="250px" MaxLength="45"
                                                                    TabIndex="4"></asp:TextBox>

                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\" TargetControlID="Txt_FGName">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Txt_FGName" ErrorMessage="You Must enter Father/Guardian name"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>Mother&#39;s Name</td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_MotherName" runat="server" class="form-control" Width="250px" MaxLength="45" TabIndex="5"></asp:TextBox>
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                                    runat="server" Enabled="True" TargetControlID="Txt_MotherName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
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
                                                            <td>&nbsp;</td>
                                                            <td>Religion<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Religion" class="form-control" runat="server" AutoPostBack="True"
                                                                     Width="250px" TabIndex="5">
                                                                </asp:DropDownList>

                                                                <%--<asp:Button class="mdl-button mdl-js-button mdl-button--fab mdl-button--mini-fab" ID="btnAddreligion" runat="server" Text="+" OnClick="btn_addReligion"></asp:Button>--%>
                                                            </td>
                                                        </tr>
                                                        <%--  <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Label ID="Lbl_Religion" runat="server"
                                                                    Text="Enter Religion"></asp:Label>
                                                                <asp:Label ID="Lbltmp1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_Religion" runat="server" class="form-control" Width="250px" TabIndex="6"></asp:TextBox>

                                                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ReligionFilteredTextBoxExtender1"
                                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\"
                                                                    TargetControlID="Txt_Religion">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="Txt_ReligionRequiredFieldValidator6" runat="server" Enabled="False" ControlToValidate="Txt_Religion" ErrorMessage="You Must enter a Religion"></asp:RequiredFieldValidator>

                                                            </td>

                                                        </tr>--%>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Label ID="Lbl_Caste" runat="server" class="control-label" Text="Caste"></asp:Label>
                                                                <asp:Label ID="Lbltmp2" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Caste" class="form-control" runat="server"
                                                                    Width="250px" TabIndex="7">
                                                                </asp:DropDownList><%--onselectedindexchanged="Drp_Cast_SelecteIndexchang"--%></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="leftside">
                                                                <br />
                                                            </td>
                                                            <td class="rightside">
                                                                <br />
                                                            </td>
                                                        </tr>


                                                        <%--<tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                   <asp:Label ID="Label_Caste" runat="server" Text="Enter Caste"></asp:Label>
                                                    <asp:Label ID="Lbltmp3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                                <td>
                                                     <asp:TextBox ID="Txt_Cast" runat="server" Width="160px" MaxLength="20" 
                                                         TabIndex="8"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_Cast">
                           </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="Txt_CastRequiredFieldValidator6" runat="server" Enabled="False" ControlToValidate="Txt_Cast" ErrorMessage="You Must enter a Cast"></asp:RequiredFieldValidator>
                            </td>
                                            </tr>--%>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>Address(Permanent)<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_Address" runat="server" class="form-control" Height="77px" Width="250px"
                                                                    TextMode="MultiLine" MaxLength="200" TabIndex="9"></asp:TextBox>
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender16"
                                                                    runat="server" Enabled="True" TargetControlID="Txt_Address" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Txt_Address" ErrorMessage="You Must enter Address(Permanent)"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:CheckBox ID="Chk_newadminsion" class="form-actions" runat="server" Checked="True"
                                                                    Text="New Admission" AutoPostBack="True"
                                                                    OnCheckedChanged="Chk_newadminsion_CheckedChanged" TabIndex="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>Joining Batch<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_JoinBatch" class="form-control" runat="server" Width="250px" TabIndex="11">
                                                                </asp:DropDownList>
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
                                                            <td>&nbsp;</td>
                                                            <td>Joining Standard<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_JoinStandard" class="form-control" runat="server" Width="250px" TabIndex="11">
                                                                </asp:DropDownList>
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
                                                            <td>&nbsp;</td>
                                                            <td>Standard<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Std" runat="server" class="form-control" Width="250px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="Drp_Std_SelectedIndexChanged" TabIndex="13">
                                                                </asp:DropDownList></td>
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
                                                            <td>&nbsp;</td>
                                                            <td>Class<span class="redcol">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="Drp_Class" class="form-control" runat="server" Width="250px"
                                                                    AutoPostBack="True"
                                                                    OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged" TabIndex="14">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lbl_ClassMsg" runat="server" class="control-label" ForeColor="Red" Text=""></asp:Label>

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
                                                            <td>&nbsp;</td>
                                                            <td>Date of Admission<span class="redcol">*</span></td>
                                                            <td>
                                                                <div id="datepicker1" class="input-group date" data-date-format="dd-mm-yyyy" style="width: 250px;">
                                                                    <asp:TextBox ID="Txt_JoiningDate" runat="server" class="form-control" Width="210px" TabIndex="12"></asp:TextBox>
                                                                    <ajaxToolkit:MaskedEditExtender ID="Txt_JoiningDate_MaskedEditExtender1" runat="server"
                                                                        MaskType="Date" CultureName="en-GB" AutoComplete="true"
                                                                        Mask="99-99-9999"
                                                                        UserDateFormat="DayMonthYear"
                                                                        Enabled="True"
                                                                        TargetControlID="Txt_JoiningDate">
                                                                    </ajaxToolkit:MaskedEditExtender>
                                                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>

                                                                </div>




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
                                                            <td>&nbsp;</td>
                                                            <td>Student Id
                                                                <asp:Label ID="Lbl_Manstudid" class="control-label" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:HiddenField ID="Hdn_ManStudId" runat="server" />
                                                                <asp:TextBox ID="Txt_StudentId" class="form-control" runat="server" Width="250px" TabIndex="15"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Txt_StudentId" ErrorMessage="You Must enter Student Id"></asp:RequiredFieldValidator>


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
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Label ID="LblAdmission" runat="server" class="control-label" Text="Admission Number"></asp:Label>
                                                                <asp:Label ID="Lbltemp4" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="Txt_AdminNo" runat="server" class="form-control" Width="250px" MaxLength="20"
                                                                    TabIndex="15">   </asp:TextBox>
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender17"
                                                                    runat="server" Enabled="True" TargetControlID="Txt_AdminNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="Txt_AdminNoRequiredFieldValidator6" runat="server" ControlToValidate="Txt_AdminNo" ErrorMessage="You Must enter an Admission No"></asp:RequiredFieldValidator>
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
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Label ID="LblAadhar" runat="server" class="control-label" Text="Aadhar Number"></asp:Label>

                                                            <td>
                                                                <asp:TextBox ID="Txt_AadharNo" runat="server" class="form-control" Width="250px" MaxLength="20"
                                                                    TabIndex="15">   </asp:TextBox>
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                                                    runat="server" Enabled="True" TargetControlID="Txt_AadharNo" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel ID="Pnl_Studnamemessage" runat="server">


                                                        <asp:Button runat="server" ID="Btn_hdnmessagetgt2" class="btn btn-info" Style="display: none" />
                                                        <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox3" CancelControlID="Btn_Yes"
                                                            runat="server"
                                                            PopupControlID="Pnl_msg2" TargetControlID="Btn_hdnmessagetgt2" />
                                                        <asp:Panel ID="Pnl_msg2" runat="server" DefaultButton="Btn_Yes" Style="display: none;">
                                                            <div class="container skin5" style="width: 400px; top: 400px; left: 200px">
                                                                <table cellpadding="0" cellspacing="0" class="containerTable">
                                                                    <tr>
                                                                        <td class="no"></td>
                                                                        <td class="n"><span style="color: White">alert!</span></td>
                                                                        <td class="ne">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="o"></td>
                                                                        <td class="c">

                                                                            <asp:Label ID="Lbl_Sudentname" class="control-label" runat="server" Text=""></asp:Label>
                                                                            <br />
                                                                            <br />
                                                                            <div style="text-align: center;">
                                                                                <asp:Button ID="Btn_Yes" class="btn btn-info" runat="server" Text="OK" Width="75" />

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
                                                            </div>
                                                        </asp:Panel>

                                                    </asp:Panel>
                                                </asp:Panel>
                                            </asp:WizardStep>
                                            <asp:WizardStep ID="WizardOther" runat="server" Title="Other Details">
                                                <table class="style3">
                                                    <tr>
                                                        <td style="width: 50px;">&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>Using School Bus?</td>
                                                        <td>

                                                            <div class="radio radio-primary">
                                                                <asp:RadioButtonList ID="Rdb_NeedBus" class="form-actions" runat="server"
                                                                    RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>




                                                            <%--<asp:RadioButtonList ID="Rdb_NeedBus" class="form-actions" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" >Yes</asp:ListItem>
                                                 <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>--%>
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
                                                        <td>&nbsp;</td>
                                                        <td>Using Hostel?</td>
                                                        <td>

                                                            <div class="radio radio-primary">
                                                                <asp:RadioButtonList ID="Rdb_NeedHostel" class="form-actions" runat="server"
                                                                    RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>


                                                            <%--<asp:RadioButtonList ID="Rdb_NeedHostel" class="form-actions" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" >Yes</asp:ListItem>
                                                 <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>--%>
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
                                                        <td>&nbsp;</td>
                                                        <td>Blood Group</td>
                                                        <td>
                                                            <asp:DropDownList ID="Drp_BloodGrp" class="form-control" runat="server" Width="250px">
                                                            </asp:DropDownList></td>
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
                                                        <td>&nbsp;</td>
                                                        <td>Nationality</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_Nationality" runat="server" class="form-control" MaxLength="20" Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\"
                                                                TargetControlID="Txt_Nationality">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Mother Tongue</td>
                                                        <td>
                                                            <asp:DropDownList ID="Drp_MotherTongue" class="form-control" runat="server" Width="250px">
                                                            </asp:DropDownList></td>
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
                                                        <td>&nbsp;</td>
                                                        <td>Father&#39;s Educational 
                    Qualification</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_FatherEduQuali" class="form-control" runat="server" Width="250px"
                                                                MaxLength="45"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                                runat="server" Enabled="True" TargetControlID="Txt_FatherEduQuali" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Mother&#39;s Educational &nbsp;Qualification</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_MotherEduQuali" class="form-control" runat="server" MaxLength="45"
                                                                Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'\"
                                                                TargetControlID="Txt_MotherEduQuali">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Father&#39;s Occupation</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_FatherOccupation" class="form-control" runat="server" Width="250px"
                                                                MaxLength="45"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                                runat="server" Enabled="True" TargetControlID="Txt_FatherOccupation" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Mother&#39;s Occupation</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_MotherOccupation" class="form-control" runat="server" Width="250px"
                                                                MaxLength="45"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                                                runat="server" Enabled="True" TargetControlID="Txt_MotherOccupation" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Annual Income</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_AnualIncome" class="form-control" runat="server" Width="250px" MaxLength="10"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="AnualIncome1"
                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="Txt_AnualIncome">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Address (Present)</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_Address_Present" class="form-control" runat="server" Height="77px"
                                                                MaxLength="200" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Address_Present_FilteredTextBoxExtender"
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
                                                                InvalidChars="'\" TargetControlID="Txt_Address_Present">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Location</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_Location" runat="server" class="form-control" MaxLength="45" Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender14"
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\"
                                                                TargetControlID="Txt_Location">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>State</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_State" runat="server" class="form-control" MaxLength="20" Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender15"
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
                                                                InvalidChars="'/\" TargetControlID="Txt_State">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Pin Code</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_pin" runat="server" class="form-control" Width="250px" MaxLength="7"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_pin_FilteredTextBoxExtender"
                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="Txt_pin">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Residence Phone Number</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_ResidencePh" runat="server" class="form-control" Width="250px" MaxLength="14"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ResidentPhone"
                                                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_ResidencePh">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2"
                                                                ControlToValidate="Txt_ResidencePh"
                                                                Display="None"
                                                                ValidationExpression="^[0-9]{8,14}"
                                                                ErrorMessage="Invalid Phone No" />
                                                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidextndrPhone"
                                                                TargetControlID="RegularExpressionValidator2"
                                                                HighlightCssClass="validatorCalloutHighlight" />
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
                                                        <td>&nbsp;</td>
                                                        <td>Mobile Number:</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_OfficePh" runat="server" class="form-control" MaxLength="14" Width="250px"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="OfficePh" runat="server"
                                                                Enabled="True" FilterType="Numbers" TargetControlID="Txt_OfficePh">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                                                ControlToValidate="Txt_OfficePh"
                                                                Display="None"
                                                                ValidationExpression="^[0-9]{10,12}"
                                                                ErrorMessage="Invalid Mobile No" />
                                                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidextndrMobile"
                                                                TargetControlID="RegularExpressionValidator1"
                                                                HighlightCssClass="validatorCalloutHighlight" />

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
                                                        <td>&nbsp;</td>
                                                        <td>Email Id</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_Email" runat="server" class="form-control" Width="250px" MaxLength="45"></asp:TextBox>
                                                            <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                                                ControlToValidate="Txt_Email"
                                                                Display="None"
                                                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                                                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                                                TargetControlID="PNRegEx"
                                                                HighlightCssClass="validatorCalloutHighlight" />
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
                                                        <td>&nbsp;</td>
                                                        <td>Number of Brothers</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_NoBro" runat="server" class="form-control" Width="250px" MaxLength="2">
                                                            </asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NoBro_FilteredTextBoxExtender1"
                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="Txt_NoBro">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>Number of Sisters</td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_NoSys" runat="server" class="form-control" Width="250px" MaxLength="2"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NoSys_FilteredTextBoxExtender1"
                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="Txt_NoSys">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
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
                                                        <td>&nbsp;</td>
                                                        <td>1st Language Wishes to take</td>
                                                        <td>
                                                            <asp:DropDownList ID="Drp_FirstLanguage" class="form-control" runat="server" Width="250px">
                                                            </asp:DropDownList></td>
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
                                                        <td>&nbsp;</td>
                                                        <td>Student Category</td>
                                                        <td>
                                                            <asp:DropDownList ID="Drp_StudentType" class="form-control" runat="server" Width="250px">
                                                            </asp:DropDownList>
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
                                                </table>
                                            </asp:WizardStep>

                                            <asp:WizardStep ID="WizardCustom" runat="server" Title="Custom Fields">
                                                <table class="style3">
                                                    <tr>
                                                        <td style="width: 50px;">&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td colspan="2">
                                                            <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:WizardStep>


                                            <asp:WizardStep ID="WizardSchedule" runat="server" Title="Schedule Fee">
                                                <div class="container skin1" style="width: 1057px;">
                                                    <table cellpadding="0" cellspacing="0" class="containerTable">
                                                        <tr>

                                                            <td class="no"></td>
                                                            <td class="n">Schedule fee</td>
                                                            <td class="ne"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="o"></td>
                                                            <td class="c">

                                                                <table class="style3">
                                                                    <tr>
                                                                        <td style="width: 50px;">&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <div>
                                                                                <asp:Label ID="Lbl_Fee" runat="server"></asp:Label>
                                                                            </div>
                                                                            <asp:Panel ID="Pnl_Feesched" runat="server">

                                                                                <div style="text-align: left" visible="false">
                                                                                    <asp:LinkButton ID="Lnk_Select" runat="server" class="form-button" Visible="false" OnClick="LnkSelBtn_Click"></asp:LinkButton>

                                                                                </div>

                                                                                <asp:GridView ID="Grd_Fees" runat="server" AutoGenerateColumns="False"
                                                                                    CellPadding="3" CellSpacing="2" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                                                                                    <Columns>

                                                                                        <asp:TemplateField ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="Chk_Fee" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)" />
                                                                                            </HeaderTemplate>

                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" Width="40px" />

                                                                                        </asp:TemplateField>




                                                                                        <asp:BoundField DataField="Id" HeaderText="Feeschid" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="FeeId" HeaderText="Feeid" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="AccountName" HeaderText="Fee" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="100px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Batch" HeaderText="Batch" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Duedate" HeaderText="Due date" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="LastDate" HeaderText="Last Date" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-Width="80">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle Width="80px" />
                                                                                        </asp:BoundField>
                                                                                    </Columns>

                                                                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                                                    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                        HorizontalAlign="Left" />
                                                                                    <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                        HorizontalAlign="Left" />

                                                                                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                                                    <EditRowStyle Font-Size="Medium" />
                                                                                </asp:GridView>


                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">&nbsp;</td>
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


                                            </asp:WizardStep>

                                        </WizardSteps>


                                        <SideBarTemplate>
                                            <asp:DataList ID="SideBarList" runat="server" OnItemDataBound="SideBarList_ItemDataBound">
                                                <ItemTemplate>
                                                    <!-- Return false when linkbutton is clicked -->
                                                    <asp:LinkButton ID="SideBarButton" OnClientClick="return false" class="form-button" ForeColor="White" runat="server"></asp:LinkButton>
                                                </ItemTemplate>
                                                <SelectedItemStyle Font-Bold="true" ForeColor="White" />
                                            </asp:DataList>
                                        </SideBarTemplate>

                                        <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana"
                                            ForeColor="White" />
                                        <NavigationButtonStyle BackColor="White" BorderColor="#507CD1"
                                            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
                                            ForeColor="#284E98" />
                                        <SideBarStyle BackColor="#507CD1" Font-Size="1.1em" HorizontalAlign="Center"
                                            VerticalAlign="Top" Width="20%" BorderStyle="Inset" Font-Bold="False"
                                            Font-Names="Times New Roman" />
                                        <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid"
                                            BorderWidth="2px" Font-Bold="True" Font-Size="0.9em" ForeColor="White"
                                            HorizontalAlign="Center" />
                                        <StepPreviousButtonStyle Height="40px" />
                                        <CancelButtonStyle Height="40px" />
                                    </asp:Wizard>




                                </asp:Panel>

                                <asp:Panel ID="Pnl_studentPhoto" runat="server" BorderColor="Black"
                                    Visible="false">

                                    <div class="newsubheading">
                                        Change Photo&nbsp;&nbsp;
                                    </div>
                                    <div class="linestyle">
                                    </div>
                                    <table class="style1">

                                        <tr>
                                            <td class="leftside">Upload Photo</td>
                                            <td>
                                                <asp:FileUpload ID="FileUp_Student" runat="server" ViewStateMode="Enabled" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="leftside">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="leftside"></td>
                                            <td>
                                                <asp:Button ID="Btn_Upload" runat="server" Class="btn btn-primary"
                                                    OnClick="Btn_Upload_Click" Text="Upload" />
                                            </td>
                                        </tr>
                                    </table>


                                </asp:Panel>


                                <asp:Panel ID="Pnl_StudentDtails" runat="server" Visible="False">

                                    <div id="PersonData" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="Hdn_RollNumber" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="Hdn_StudentId" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="Hdn_TempId" runat="server" Value="0" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td colspan="3">
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/user4.png"
                                                        Width="57px" />
                                                    <b>Student is Created with the following Details...</b><%--</td>--%><%--<td>--%>
                                                    
                                                </td>
                                                <tr>
                                                    <td style="background-color: #C2D5FC; width: 25%;">
                                                        <b>Name</b>
                                                    </td>
                                                    <td style="background-color: #C2D5FC; width: 25%;">
                                                        <asp:Label ID="Lbl_name_ds" class="control-label" runat="server" Text="Label"></asp:Label>

                                                    </td>
                                                    <td style="background-color: #C2D5FC; width: 25%;">
                                                        <b>Father/Guardian Name</b></td>
                                                    <td style="background-color: #C2D5FC; width: 25%;">
                                                        <asp:Label ID="Lbl_Father_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>Sex</b>
                                                    </td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_Sex_ds" runat="server" class="control-label" Text="Label"></asp:Label>

                                                    </td>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>Standard</b></td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_Standard_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>Admission No</b></td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_Admission_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>Class</b></td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_Class_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td></td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>DOB</b></td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_DOB_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                    <td style="background-color: #C2D5FC">
                                                        <b>Joining Batch</b></td>
                                                    <td style="background-color: #C2D5FC">
                                                        <asp:Label ID="Lbl_Joining_ds" runat="server" class="control-label" Text="Label"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td align="center" colspan="2">

                                                    <asp:ImageButton ID="Img_viewuser" class="form-control" runat="server" Height="70px"
                                                        ImageUrl="~/Pics/search_female_user.png" Width="70px"
                                                        OnClick="Img_viewuser_Click" />
                                                    &nbsp;&nbsp;
                                         <asp:ImageButton ID="Img_addnewuser" class="form-control" runat="server" Height="70px"
                                             ImageUrl="~/Pics/add_female_user.png" Width="70px"
                                             OnClick="Img_addnewuser_Click" />
                                                    <asp:ImageButton ID="Img_CollectFee" class="form-control" runat="server" Height="60px" Visible="false"
                                                        ImageUrl="~/Pics/dollar.png" Width="60px"
                                                        OnClick="Img_CollectFee_Click" />
                                                    <asp:Button ID="photoUpload" Class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--accent" runat="server" OnClick="btn_photoUpload" Text="Upload Photo" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>

                                </asp:Panel>

                                <!-- temp List!-->
                                <asp:Button ID="popup_tempstudents" runat="server" class="btn btn-info" Style="display: none" ValidationGroup="Tempsearch" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_tempstudents"
                                    runat="server" CancelControlID="Btn_canceltempstudents" BackgroundCssClass="modalBackground"
                                    PopupControlID="tempstudentspanel" TargetControlID="popup_tempstudents" />
                                <asp:Panel ID="tempstudentspanel" runat="server" Style="display: none;">
                                    <div class="container skin1">
                                        <table cellpadding="0" cellspacing="0" class="containerTable" width="600px">
                                            <tr>
                                                <td class="no">
                                                    <img alt="" src="Pics/search_female_user.png" width="35" height="35" />
                                                </td>
                                                <td class="n"><span style="color: Black">Temp Students List</span></td>
                                                <td class="ne">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="o"></td>
                                                <td class="c">
                                                    <br />
                                                    <asp:Panel ID="pnl_Srch" runat="server" DefaultButton="Btn_Search" Style="width: 250px;">
                                                        <%--               <asp:Label ID="lbl_StudentName" runat="server" Text="Enter Student Name" Height="22px" Font-Bold="true" Visible="false" ForeColor="Red"></asp:Label>&nbsp;
                                                        --%>
                                                        <asp:RadioButtonList ID="RdBtn_StdListType" class="form-actions" runat="server" ValidationGroup="Tempsearch"
                                                            RepeatDirection="Horizontal" Font-Bold="true" AutoPostBack="true"
                                                            OnSelectedIndexChanged="RdBtn_StdListType_SelectedIndexChanged"
                                                            TabIndex="1">
                                                            <asp:ListItem Selected="True">Shortlisted</asp:ListItem>
                                                            <asp:ListItem>On Waiting List</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <br />

                                                        <asp:Label ID="lblstd" runat="server" Text="Standard" class="control-label" Font-Bold="true"></asp:Label>

                                                        <asp:DropDownList ID="Drp_PopUp_Standardlist" runat="server"
                                                            AutoPostBack="true" ValidationGroup="Tempsearch" class="form-control"
                                                            OnSelectedIndexChanged="Drp_PopUpStd_SelectedIndex" TabIndex="2">
                                                        </asp:DropDownList>

                                                        <asp:Label ID="lblclass" runat="server" Text="Class" Font-Bold="true"></asp:Label>

                                                        <asp:DropDownList ID="Drp_PopUp_Class" class="form-control" runat="server"
                                                            ValidationGroup="Tempsearch"
                                                            OnSelectedIndexChanged="Drp_PopUp_Class_SelectedIndexChanged" TabIndex="3">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lbl_Name" runat="server" Text="Name" class="control-label" Font-Bold="true"></asp:Label>
                                                        <asp:TextBox ID="txt_StudentName" runat="server" class="form-control" Text="" TabIndex="4"></asp:TextBox>

                                                        &nbsp;
                <asp:Button ID="Btn_Search" runat="server" class="btn btn-primary" Text="Search"
                    OnClick="Btn_Search_Click" ValidationGroup="Tempsearch" TabIndex="5" />&nbsp;&nbsp;&nbsp;
              <%--  <asp:Button ID="Btn_AllList" runat="server" Text="ALL" Width="70px" OnClick="Btn_AllList_Click" ValidationGroup="Tempsearch"
                    Enabled="false" />--%>

                                                        <br />
                                                        <br />
                                                    </asp:Panel>
                                                    <div class="linestyle"></div>
                                                    <div style="text-align: center">
                                                        <asp:Label ID="Lbl_TempMessage" class="control-label" runat="server"></asp:Label>
                                                    </div>
                                                    <asp:Label ID="lbl_StudentName1" class="control-label" runat="server" Text="No Students Found" Height="22px" Font-Bold="true" Visible="false" ForeColor="Red"></asp:Label>&nbsp;

               <asp:GridView ID="Grd_TempStudents" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                   Width="100%"
                   OnSelectedIndexChanged="Grd_TempStudents_SelectedIndexChanged" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                   <Columns>
                       <asp:BoundField DataField="Id" HeaderText="Id" />
                       <asp:BoundField DataField="TempId" HeaderText="Temp Id" ItemStyle-Width="100px" ControlStyle-Width="100px" />
                       <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="150px" ControlStyle-Width="100px" />
                       <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-Width="100px" ControlStyle-Width="50px" />
                       <asp:BoundField DataField="ClassName" HeaderText="Class" ItemStyle-Width="100px" ControlStyle-Width="60px" />
                       <asp:BoundField DataField="Rank" HeaderText="Rank" ItemStyle-Width="50px" ControlStyle-Width="30px" />
                       <asp:CommandField SelectText="&lt;img src='pics/hand.png' width='30px' border=0 title='Click to select'&gt;"
                           ShowSelectButton="True" HeaderText="Select" ItemStyle-Width="35" />

                   </Columns>
                   <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <SelectedRowStyle BackColor="White" ForeColor="Black" />
                   <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                   <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
               </asp:GridView>
                                                    <br />
                                                    <p style="text-align: center">
                                                        <asp:Button ID="Btn_canceltempstudents" class="btn btn-danger" runat="server" Text="CANCEL"
                                                            ValidationGroup="CancelValGroup" TabIndex="6" />
                                                    </p>
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

                                <!-- temp List!-->


                                <asp:Button runat="server" ID="Btn_hdnmessagetgt" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"
                                    runat="server" CancelControlID="Btn_magok"
                                    PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
                                <asp:Panel ID="Pnl_msg" runat="server" DefaultButton="Btn_magok" Style="display: none;">
                                    <div class="container skin5" style="width: 400px; top: 400px; left: 200px">
                                        <table cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr>
                                                <td class="no">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png"
                                                        Height="28px" Width="29px" />
                                                </td>
                                                <td class="n"><span style="color: White">alert!</span></td>
                                                <td class="ne">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="o"></td>
                                                <td class="c">

                                                    <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                                                    <br />
                                                    <br />
                                                    <div style="text-align: center;">

                                                        <asp:Button ID="Btn_magok" runat="server" class="btn btn-info" Text="OK" Width="50px" />
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
                <div class="modal fade" role="dialog" id="modalmessage">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close fa-3x" data-dismiss="modal">&times;</button>
                                <h4>
                                    <asp:Label class="control-label" ID="Lbl_Head" runat="server" Text="Message"></asp:Label></h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="Label_text" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="modal-footer">
                                <%-- <asp:Button ID="Btn_ok" class="btn btn-info" data-dismiss="modal"  Text="OK" Width="50px" OnClick="myFunction()" />--%>
                                <button id="Btn_ok" class="btn btn-info" data-dismiss="modal" onclick="myFunction()" style="width: 100px;">Ok</button>
                            </div>

                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Btn_Upload" />
            </Triggers>
        </asp:UpdatePanel>



        <div class="clear"></div>
    </div>

</asp:Content>
