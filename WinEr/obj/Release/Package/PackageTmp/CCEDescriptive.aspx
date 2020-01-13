<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEDescriptive.aspx.cs" Inherits="WinEr.CCEDescriptive" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
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
                                  CCE Descriptive</td>
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
                     <td class="leftside">Select Class Name</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" AutoPostBack="true" 
                             Width="250px" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     
                    
                     <tr>
                     <td class="leftside"> Select Part</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_part" runat="server" class="form-control" AutoPostBack="true" 
                          Width="250px" OnSelectedIndexChanged="Drp_part_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>

                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Select Subject Name</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_subject" runat="server" class="form-control" AutoPostBack="true"
                            Width="250px" OnSelectedIndexChanged="Drp_subject_SelectedIndexChanged">
                         </asp:DropDownList>
                     </td>
                     </tr>
                    
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     <tr>
                      <td class="leftside">Select Skill Name</td>
                      <td class="rightside" style="height: 22px">
                          <asp:DropDownList ID="Drp_skill" runat="server" class="form-control" AutoPostBack="true"
                               Width="250px" OnSelectedIndexChanged="Drp_skill_SelectedIndexChanged">
                          </asp:DropDownList>
                          
                           &nbsp;
                          
                           <asp:LinkButton ID="Lnk_CCEGrade" runat="server"  onclick="Lnk_CCEGrade_Click">Import CCE Grade</asp:LinkButton>
                
                      
                      </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>

                     </table>
                     
                     <table class="tablelist" id="Gridview_Table" runat="server">
                     
                     
                  
                     <tr>
                     <td colspan="2"><hr /></td>
                     </tr>
                     <tr>
                     <td class="leftside">
                     </td>
                     <td class="rightside">
                     </td>
                     </tr>
                     
                     <tr>
                     <td colspan="2">
                      <div style="width:auto;height:250px;overflow:auto;" >
                        <asp:GridView ID="Grd_CCEMark" runat="server" AutoGenerateColumns="false"
                            Width="97%" 
                           BackColor="#EBEBEB" OnRowDataBound="Grd_CCEMark_RowDataBound"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                            <Columns>
                                <asp:BoundField DataField="StudentId" HeaderText="Student Id" />
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                <asp:BoundField DataField="StudentRollNo" HeaderText="Student RollNo" />
                                
                                <asp:TemplateField HeaderText="DescriptiveIndicator" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txt_DescriptiveIndicator" runat="server" class="form-control" Height="20" TextMode="MultiLine"
                                    MaxLength="6" Text="" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="TermOne" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_TermOne" runat="server" class="form-control" Height="20" MaxLength="3" Text=""  
                                            Width="100" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="TermTwo" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txt_TermTwo" runat="server" Height="20" class="form-control" MaxLength="3" Text=""
                                     Width="100"></asp:TextBox>
                                </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="TermThree" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txt_TermThree" runat="server" class="form-control" Height="20" MaxLength="3"
                                       Text="" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                         <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                      </div>
                        
                     </td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">
                     </td>
                     <td class="rightside">
                     </td>
                     </tr>
                     
                      <tr>
                     <td colspan="2"><hr /></td>
                     </tr>
                     
                     <tr>
                      <td class="leftside"></td>
                      <td class="rightside">
                      <asp:Button ID="Btn_Save" runat="server" Text="Save" Class="btn btn-success"
                                     ToolTip="save cce grade" onclick="Btn_Save_Click" />
                          &nbsp;<asp:Button ID="Btn_Clear" runat="server" Text="Clear" Class="btn btn-danger"
                                   ToolTip="clear all cce grade" onclick="Btn_Clear_Click"/>              
                      </td>
                      </tr>
                     <tr>
                     <td class="leftside">
                     </td>
                     <td class="rightside">
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
       <WC:MSGBOX id="WC_MessageBox" runat="server" />     
       </ContentTemplate>
       
    </asp:UpdatePanel>
</div>
</asp:Content>
