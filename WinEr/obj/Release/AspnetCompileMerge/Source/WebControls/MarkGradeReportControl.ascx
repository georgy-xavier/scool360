<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkGradeReportControl.ascx.cs" Inherits="WinEr.WebControls.MarkGradeReportControl" %>
<asp:Panel ID="Panel1" runat="server">
  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
    <ProgressTemplate><div id="progressBackgroundFilter"></div>
    <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center">
    <b>Please Wait...</b><br /><br /><img src="../images/indicator-big.gif" alt=""/></td></tr></table></div></ProgressTemplate>
    </asp:UpdateProgress>
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
    <link href="css files/MasterStyle.css" rel="stylesheet" type="text/css" />
    <link href="css files/winbuttonstyle.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
        <asp:HiddenField ID="Hdn_StudentID" runat="server" Value="0" />

            <div class="container skin1" >
                <table cellpadding="0" cellspacing="0" class="containerTable">
                    <tr>
                        <td class="no"><img alt="" src="../Pics/report1.png" height="25" width="25" /></td>
                        <td class="n">Mark-Grade Report</td>
                        <td class="ne"></td>
                    </tr>
                    <tr>
                        <td class="o"></td>
                        <td class="c">
                            <asp:Panel ID="Pnl_Select" runat="server" >
                            <div style="height:400px"> 
                                <table class="tablelist" width="100%">
                                    <tr>
                                        <td class="leftside">
                                            Select Class
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList ID="Drp_Class" runat="server" Width="153px" class="form-control"
                                                onselectedindexchanged="Drp_Class_SelectedIndexChanged" 
                                                AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>
                                      





                                     <tr>
                                        <td class="leftside">
                                            Select Exam Type
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_ExamType" runat="server" Width="153px" class="form-control"
                                                AutoPostBack="True" onselectedindexchanged="Drp_ExamType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                   





                                    <tr>
                                        <td class="leftside">
                                            Select Main Exam
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_Exam" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                     





                                     <tr>
                                        <td class="leftside">
                                            Select Co-curricular Activities
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_Cocurricular" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    





                                    
                                     <tr  runat="server" id="CharacterTraitsRow">
                                        <td class="leftside">
                                            Select Character Traits
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_CharecterTraits" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                     




                                    
                                      <tr>
                                        <td class="leftside">
                                            Select Student
                                        </td>
                                        <td class="rightside">
                                            <asp:DropDownList Id="Drp_StudentList" runat="server" Width="153px" class="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                     



                                    <tr>
                                    
                                        <td class="leftside" valign="top">
                                        Reports Contains
                                        </td>
                                        <td class="rightside">
                                            <asp:CheckBoxList ID="ChkBxList" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="ChkBxList_SelectedIndexChanged" >
                                            <asp:ListItem Text="Header" Value="Header" ></asp:ListItem>
                                            <asp:ListItem Text="Main Report" Value="MainReport" Selected="True"></asp:ListItem>
                                            <asp:ListItem  Selected="True" Text="Co-Curricular Activities and Character Traits" Value="Co-CurricularActivitiesandCharacterTraits"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Performance" Value="Performance"></asp:ListItem>
                                             <asp:ListItem Selected="True" Text="Average" Value="Average"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Teacher's Remark" Value="TeachersRemark"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        </tr>
                                        




                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" class="control-label"></asp:Label>
                                            </td>
                                    <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="Btn_Create" runat="server" Class="btn btn-info" 
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

