<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="WinerAboutPage.aspx.cs" Inherits="WinEr.WinerAboutPage" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
                                  About the Scool360</td>
                              <td class="ne">
                              </td>
                          </tr>
              
              <tr >
                <td class="o"></td>
                <td class="c">
                <br />
                
                 <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    
                     <table class="tablelist">
                     
                     <tr>
                     <td class="leftside">Product Name :&nbsp</td>
                     <td class="rightside"><asp:Label ID="Label1" runat="server" Text="Scool360" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="leftside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">DataBase Version :&nbsp</td>
                     <td class="rightside">
             
                         <asp:Label ID="Lbl_db" runat="server" Text="Label" ForeColor="Red"  Font-Bold="true" Font-Size="Small"></asp:Label>
                 
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="leftside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Patch Version : &nbsp</td>
                     <td class="rightside">
                      <asp:Label ID="Lbl_patch" runat="server" Text="Label" ForeColor="Red"  Font-Bold="true" Font-Size="Small"></asp:Label>
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="leftside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Scool360 Version :&nbsp</td>
                     <td class="rightside">
                   
                         <asp:Label ID="Lbl_winer" runat="server" Text="Label" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                   
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="leftside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Last Update Date :&nbsp;</td>
                     <td class="rightside">
                         <asp:Label ID="Lbl_date" runat="server" Text="Label" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="leftside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Latest Changes :&nbsp</td>
                     <td class="rightside">
                     
                     </td>
                     </tr>
                     </table>
                     <div id="Table" runat="server" >
                     </div>
                    
                      
                 </asp:Panel>
                 
                 <br />
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
          <WC:MSGBOX  ID="WC_MessageBox" runat="server" />
       </ContentTemplate>
     </asp:UpdatePanel>
</div>
</asp:Content>
