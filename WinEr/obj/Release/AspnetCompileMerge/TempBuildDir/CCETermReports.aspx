<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCETermReports.aspx.cs" Inherits="WinEr.TermReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet"><ProgressTemplate>                               
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /> <br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>          
    </ProgressTemplate></asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="container skin1">
                <table cellpadding="0" cellspacing="0" class="containerTable">
                    <tr><td class="no"></td>
                        <td class="n"></td>
                        <td class="ne"></td>
                    </tr>
                    <tr>
                        <td class="o"></td>
                        <td class="c">
                            <div align="center">
                                <table width="100%">
                                    <tr>
                                        <td  align="right">Select Class</td>
                                        <td  align="left"><asp:DropDownList ID="Drp_SelectClass" runat="server" AutoPostBack="true"  Width="203px" onselectedindexchanged="Drp_SelectClass_SelectedIndexChanged"></asp:DropDownList></td>
                                    </tr>
                                      
                                    <tr>
                                        <td  align="right">Select Exam Type</td>
                                        <td  align="left"><asp:DropDownList ID="Drp_SelectExam" runat="server"  AutoPostBack="true" 
                                                Width="203px" onselectedindexchanged="Drp_SelectExam_SelectedIndexChanged"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td  align="right">Report Name</td>
                                        <td  align="left"><asp:TextBox ID="Txt_ReportName" runat="server" Width="200px"></asp:TextBox></td>
                                    </tr>
                                     <tr>
                                        <td  align="right">Select Co-Scholastic Area</td>
                                        <td  align="left"><asp:DropDownList ID="Drp_Scholastic" runat="server" Width="203px"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td  align="right">Select Student</td>
                                        <td  align="left"><asp:DropDownList ID="Drp_SelectStudent" runat="server" Width="203px"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td  align="right">Health Informations</td>
                                       <td  align="left"><asp:CheckBox ID="Chk_HealthInfo" runat="server"/></td>
                                    </tr>
                                     <tr>
                                        <td  align="right">Self Awareness </td>
                                       <td  align="left"><asp:CheckBox ID="Chk_Self_Awareness" runat="server"  /></td>
                                    </tr>
                                     <tr>
                                        <td></td>
                                        <td  align="left"><asp:Button ID="Btn_ExamReport" runat="server" Text="Create Report" OnClick="Btn_ExamReport_Click"/></td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" align="center"><asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                </table>
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
        </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
