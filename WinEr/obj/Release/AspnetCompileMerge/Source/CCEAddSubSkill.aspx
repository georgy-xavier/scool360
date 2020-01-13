<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEAddSubSkill.aspx.cs" Inherits="WinEr.CCEAddSubSkill" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
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
             <table class="containerTable">
             
             <tr>
                              <td class="no">
                                  <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                              </td>
                              <td class="n">
                                  Add Subject Skill</td>
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
                  <td class="leftside">Skill Name</td>
                  <td class="rightside">
                  <div class="form-group">
                   <asp:TextBox ID="Txt_Skillname" runat="server" class="form-control col-lg-6" Width="250px"></asp:TextBox>
                       <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amt_FilteredTextBoxExtender" 
                           runat="server" Enabled="True" TargetControlID="Txt_Skillname"  FilterType="Custom"  FilterMode="InvalidChars" InvalidChars=".!@#$%^&;*()~?><|\';:">
                           </ajaxToolkit:FilteredTextBoxExtender>
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:Button ID="Btn_add" runat="server" Text="Add" Class="btn btn-primary col-lg-6" 
                          ToolTip="Add" onclick="Btn_add_Click" style="width:100px"/> 
                          </div>
                  </td>
                  
                  </tr>
                  <tr>
                  <td colspan="2" align="left">
                      <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" class="control-label" Visible="false"></asp:Label>
                  </td>
                  </tr>
                  </table>

                  
                 
                   <table id="GridTable" runat="server" class="tablelist">
                   <tr>
                   <td align="center"><br /><hr /><br /></td>
                   </tr>
                   <tr>
                   <td align="center">
                    <div id="divgrid" runat="server" style="width:auto;height:300px;overflow:auto">
                   <asp:GridView ID="Grd_CCE" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                        CellPadding="3" CellSpacing="2" Font-Size="12px" Width="100%" 
                        OnRowCommand="Grd_CCE_RowCommand" >
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="SkillName" HeaderText="Skill Name" />
                       
                        
                         <asp:TemplateField ItemStyle-Width="30px" HeaderText="Remove">
                         <ItemTemplate>
                             <asp:ImageButton ID="ImageButton1" ImageUrl="Pics/DeleteRed.png" runat="server"  Width="30px"  CommandName="Remove"
                             CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Remove" OnClientClick="return confirm('This skill mapped . Are you sure remove this class map?')" />
                         </ItemTemplate>
                         </asp:TemplateField>
                         
                        
                    </Columns>
                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                    <EditRowStyle Font-Size="Medium" />
                    <SelectedRowStyle BackColor="White" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                      <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                   </asp:GridView>
                   </div>
                   </td>
                   </tr>
                   </table>
                 
                
                  
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
      <WC:MSGBOX id="WC_MessageBox" runat="server" />     
       </ContentTemplate>
    </asp:UpdatePanel>
    
     
</div>
</asp:Content>
