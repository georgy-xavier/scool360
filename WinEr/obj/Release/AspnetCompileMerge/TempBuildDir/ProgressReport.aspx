<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ProgressReport.aspx.cs" Inherits="WinEr.ProgressReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function handleChange() {

        var cb = document.getElementById("<%= Chk_signature.ClientID %>");
        if (cb.checked == true) {
            Show();
        } else {
        Hide();
        }
    }
    function Show() {
        document.getElementById("<%= Txt.ClientID %>").style.display = "";
        document.getElementById("<%= Label1.ClientID %>").style.display = "";
    }

    function Hide() {
        document.getElementById("<%= Txt.ClientID %>").style.display = "none";
        document.getElementById("<%= Label1.ClientID %>").style.display = "none";
    }
</script>
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
             <tr>
                              <td class="no">
                                  <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                              </td>
                              <td class="n">
                                  Progress Report</td>
                              <td class="ne">
                              </td>
                          </tr>
              
                <tr >
                <td class="o"> </td>
                <td class="c">
                
                 <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                 
                 <table width="100%">
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
                 <td align="right" style="width:35%;">
                  Select Class&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:15%;">
                  <asp:DropDownList ID="Drp_SelectClass" runat="server" AutoPostBack="true" class="form-control" Width="160px" onselectedindexchanged="Drp_SelectClass_SelectedIndexChanged"></asp:DropDownList>
                  </td>
                 <td align="right" style="width:15%;">
                 Select Exam&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                  <td align="left" style="width:35%;">
                  <asp:DropDownList ID="Drp_SelectExam" runat="server" Width="160px" class="form-control"></asp:DropDownList>
                  </td>
                 </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
                 <td align="right" style="width:35%;">
                  Select Student&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:15%;">
                 <asp:DropDownList ID="Drp_SelectStudent" runat="server" Width="160px" class="form-control"></asp:DropDownList>
                 </td>
                 <td align="right" style="width:15%;">
                 Show Subject Group&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                  <td align="left" style="width:35%;">
                 <asp:CheckBox ID="Chk_SubGroup" runat="server"  Checked="true"/>
                  </td>
                 </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
                 <td align="right" style="width:35%;">
                 Need Percentage of mark&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:15%;">
                 <asp:CheckBox ID="Chk_needpercentage" runat="server" Checked="true"/>
                 </td>
                 <td align="right" style="width:15%;">
                 Need Rank&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:35%;">
                 <asp:CheckBox ID="Chk_needrank" runat="server" Checked="true"/>
                 </td>
                 </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
                 <td align="right" style="width:35%;">
                 Need Attendance&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:15%;">
                  <asp:CheckBox ID="Chk_needattendance" runat="server" Checked="true"/>
                 </td>
                 <td align="right" style="width:15%;">
                 Reg.No&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:35%;">
                  <asp:CheckBox ID="Chk_regno" runat="server" Checked="true"/>
                 </td>
                 </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
                 <td align="right" style="width:35%;">
                 Need to Change Signature&nbsp;&nbsp;:&nbsp;&nbsp;
                 </td>
                 <td align="left" style="width:15%;">
                  <asp:CheckBox ID="Chk_signature"  runat="server" AutoPostBack="true" OnCheckedChanged="ChckedChanged" Checked="false"/>
                 </td>
                 <td align="right" style="width:15%;">
                     <asp:Label ID="Label1"  runat="server" Text="Enter the Signature Text : " class="control-label"></asp:Label>
                 
                 </td>
                 <td align="left" style="width:35%;">
                  <asp:TextBox ID="Txt" runat="server"  TextMode="MultiLine" class="form-control" Width="250px" placeholder="Signature of the Class Teacher$Parent's Sign$Signature of the Principal"></asp:TextBox>
                 </td>
            
                 </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 
                 <tr>
                 <td align="right" colspan="2">
                 <asp:Button ID="Btn_Exam" runat="server" Text="Create Report" Class="btn btn-primary" OnClick="Btn_Exam_Click"/>
                 
                 </td>
               
                 <td align="right" style="width:20%;">
                 </td>
                 <td align="left" style="width:30%;">
                 </td>
                 </tr>
                 
                 <tr><td colspan="4"></td></tr>
                 
                 <tr>
               <td align="center" colspan="4">
               <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
               </td>
                 </tr>
                 
                 
                 </table>  
               
                 </asp:Panel>
                </td>
                <td class="e"> </td>
                </tr>
             <tr>
                              <td class="so">
                              </td>
                              <td class="s">
                              </td>
                              <td class="se">
                              </td>
                          </tr>
             </table>
            </div>		                     
        </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
