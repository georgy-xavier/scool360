<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GenerateExamResult.aspx.cs" Inherits="WinEr.GenerateExamResult" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 
 <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
         <ProgressTemplate>
                
                
                        <div id="progressBackgroundFilter">

 
 
                        </div>

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
       <div class="container skin1" >
             <table cellpadding="0" cellspacing="0" class="containerTable">
               <tr>
                 <td class="no">
                 <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                 </td>
                 <td class="n">
                 Generate Exam Result</td>
                 <td class="ne">
                 </td>
                 </tr>
                <tr >
                <td class="o"> </td>
                <td class="c" >
                <br />
                
                
                 <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                     <table class="tablelist">
                     <tr>
                     <td class="leftside">Select Term</td>
                     <td class="rightside">
                      <asp:DropDownList ID="Drp_TermSelect" runat="server" AutoPostBack="True"  Width="140px">  
                        <asp:ListItem Value="0" Text="Term I"></asp:ListItem>  
                        <asp:ListItem Value="1" Text="Term II"></asp:ListItem>                                    
                      </asp:DropDownList>
                     </td>
                     </tr>
                     <tr>
                     <td colspan="2">
                     <br />
                     </td>
                     </tr>
                     <tr>
                      <td  class="leftside">
                      </td>
                      <td class="rightside">
                       <asp:Button ID="Btn_Generate" runat="server" Text="Generate" CssClass="grayempty"
                                   ToolTip="create map" onclick="Btn_Generate_Click" />
                      </td>
                    
                     </tr>
                     </table>
                 </asp:Panel>
                     
            
                <br />
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
       <WC:MSGBOX  ID="WC_MessageBox" runat="server" />
       </ContentTemplate>
  </asp:UpdatePanel>
</div>
</asp:Content>
