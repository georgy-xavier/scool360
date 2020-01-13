<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEclasssubjectmap.aspx.cs" Inherits="WinEr.CCEclasssubjectmap" %>
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
             <table cellpadding="0" cellspacing="0" class="containerTable">
             <tr>
                              <td class="no">
                                  <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                              </td>
                              <td class="n">
                                  Subject Configuration</td>
                              <td class="ne">
                              </td>
                          </tr>
             <tr >
               <td class="o"></td>
               <td class="c">
               <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                <div id="div1" runat="server">
                
                <table class="tablelist">
                 <tr>
                  <td class="leftside">Select Class</td>
                  <td class="rightside">
                     <asp:DropDownList ID="Drp_ClassSelect" runat="server" AutoPostBack="True" class="form-control col-lg-6"
                                       Width="250px">
                                       </asp:DropDownList>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:Button ID="Btn_Continue" runat="server" Text="Continue" Class="btn btn-primary col-lg-6"
                                     ToolTip="Click" style="width:100px" onclick="Btn_Continue_Click"/>
                                     
                                                 
                  </td>
                  </tr>
                  <tr>
                  <td align="left">
                      <asp:Label ID="Label1" runat="server" Text="Label" class="control-label" ForeColor="Red" Visible="false"></asp:Label></td>
                  <td align="right"></td>
                  </tr>
                </table>
                
                </div>
                
                 <div id="divgrid" runat="server">
                 <table class="tablelist">
                 <tr>
                 <td align="left">    
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class Name : <asp:Label ID="Lbl_classname" runat="server" Text="" class="control-label" ForeColor="Red" Font-Size="Small"></asp:Label></td>
                 </tr>
                 <tr>
                 <td align="center"><br /><hr /></td>
                 </tr>
                  <tr>
                   <td align="center">
                   
                    <div style="OVERFLOW: auto; WIDTH: 500px; HEIGHT: 230px; BACKGROUND-COLOR:gainsboro">
                    <br />
                     <asp:GridView ID="Grd_CCEstudent" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                                         BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="0.50px">
                        
                        <Columns>
                        <asp:TemplateField>
                        <HeaderTemplate>
                        <asp:CheckBox ID="ChkSelect" AutoPostBack="true" runat="server" OnCheckedChanged="ChkSelect_OnCheckedChanged"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_temselect" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_temselect_OnCheckedChanged"/>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="SubjectName" HeaderText="Subject Name" />
                        <asp:TemplateField HeaderText="OrderNo" ItemStyle-Width="150">
                        <ItemTemplate>
                                        <asp:TextBox ID="Txt_Mark" runat="server" Height="20" class="form-control" MaxLength="3" Text="0" AutoPostBack="true"
                                             Width="100" OnTextChanged="Txt_Mark_TextChanged"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Mark_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_Mark" ValidChars="">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                         </asp:GridView>
                    <br />
                    </div>
                    
                  </td>
                  </tr>
                 <tr>
                 <td align="center"><br /></td>
                 </tr>
                  <tr>
                  <td  align="center">
                      <asp:Button ID="Btn_update" runat="server" Text="Update"  ToolTip="Update" 
                          Class="btn btn-success" onclick="Btn_update_Click"/>
                      &nbsp;&nbsp;&nbsp;
                      <asp:Button ID="Btn_cancel" runat="server" Text="Cancel" ToolTip="Cancel" 
                          Class="btn btn-danger" onclick="Btn_cancel_Click" />
                  </td>
                  </tr>

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
        <WC:MSGBOX id="WC_MessageBox" runat="server" />     
       </ContentTemplate>
       </asp:UpdatePanel>
     
</div>
    
</asp:Content>
