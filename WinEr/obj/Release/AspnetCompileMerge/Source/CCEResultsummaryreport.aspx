<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEResultsummaryreport.aspx.cs" Inherits="WinEr.CCEResultsummaryreport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
         <ProgressTemplate>
         <div id="progressBackgroundFilter"></div>
              <div id="processMessage">
                <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>
              </div>
         </ProgressTemplate>
    </asp:UpdateProgress>
 
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
          <ContentTemplate>
          <div class="container skin1">
          
           <table cellpadding="0" cellspacing="0" class="containerTable" width="150%">
           
             <tr>
             <td class="no">
              <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
             </td>
             <td class="n">
                      CCE Exam Summary Report</td>
             <td class="ne">
             </td>
             </tr>
             
             <tr>
             <td class="o"></td>
             <td class="c">
             <asp:Panel ID="Pnl_ExamConstraints" runat="server">
             
             <div ID="div1" runat="server">
             <table class="tablelist">
             
              <tr>
              <td class="leftside">Select Term :&nbsp;</td>
              <td class="rightside">
              <asp:DropDownList ID="Drp_term" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="Drp_term_OnSelectedIndexChanged" Width="140px">
              </asp:DropDownList>
              </td>
              </tr>
              
              <tr align="center"><td colspan="2"><br /><hr /><br /></td></tr>
              
             </table>
             </div>
             
             </asp:Panel>
             
             </td>
             <td class="e"></td>
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
 
</div>

</asp:Content>
