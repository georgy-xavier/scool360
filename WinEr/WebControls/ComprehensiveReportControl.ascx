<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ComprehensiveReportControl.ascx.cs" Inherits="WinEr.WebControls.ComprehensiveReportControl" %>
<asp:Panel ID="Panel1" runat="server">
 <%-- <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />--%>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet"><ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="../images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate></asp:UpdateProgress>
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
    <link href="css files/MasterStyle.css" rel="stylesheet" type="text/css" />
    <link href="css files/winbuttonstyle.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
        <asp:HiddenField ID="Hdn_StudentID" runat="server" Value="0" />

            <div class="card0" >
                <table class="containerTable">
                   <%-- <tr>
                        <td class="no"><img alt="" src="../Pics/report1.png" height="25" width="25" /></td>
                        <td class="n">Comprehensive Report</td>
                        <td class="ne"></td>
                    </tr>--%>
                    <tr>
                        <td class="o"></td>
                        <td class="c">
                            <asp:Panel ID="Pnl_Select" runat="server" >
                            <div style="min-height: 80vh;"> 
                                <table class="tablelist" width="100%">
                                    <tr>
                                        <td class="leftside">
                                            Select Class
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList ID="Drp_Class" runat="server" Width="153px" 
                                                onselectedindexchanged="Drp_Class_SelectedIndexChanged" 
                                                AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="leftside">
                                            Select Exam Type
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_ExamType" runat="server" Width="153px" 
                                                AutoPostBack="True" onselectedindexchanged="Drp_ExamType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="leftside">
                                            Select Exam
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_Exam" runat="server" Width="153px" ></asp:DropDownList>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td class="leftside">
                                            Select Student
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_StudentList" runat="server" Width="153px" ></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                    
                                        <td class="leftside" valign="top">
                                        Type of Reports
                                        </td>
                                        <td class="rightside">
                                            <asp:CheckBoxList ID="ChkBxList" runat="server" AutoPostBack="true" >
                                            </asp:CheckBoxList>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td class="leftside">
                                        Report Return Date
                                        </td>
                                        <td class="rightside">
                                            <asp:TextBox ID="Txt_ReturnDate" runat="server" Width="150px" ></asp:TextBox>
                                               <ajaxToolkit:CalendarExtender ID="Txt_ReturnDate_CalendarExtender" runat="server" 
                                                CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_ReturnDate" Format="dd/MM/yyyy">
                                              </ajaxToolkit:CalendarExtender>
                                          
                                              <asp:RegularExpressionValidator ID="ExamDateRegularExpressionValidator3" 
                                                                        runat="server" ControlToValidate="Txt_ReturnDate" Display="None" 
                                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                                         />  
                                             <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="Exam_ValidatorCalloutExtender"
                                                TargetControlID="ExamDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" /> 
                                        </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="Btn_Create" runat="server" Class="btn btn-primary" 
                                                    onclick="Btn_Create_Click" Text="Create" Width="100px" />
                                            </td>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
