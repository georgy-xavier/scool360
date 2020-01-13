<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentCombinedExamReport.aspx.cs" Inherits="WinEr.StudentCombinedExamReport" %>
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
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/evolution-tasks.png" height="35" width="35" /> </td>
                <td class="n">Student Combained Exam Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table class="tablelist">
                    
                     <tr>
                     <td class="leftside">&nbsp;</td>
                     <td class="rightside">
                         &nbsp;</td>
                         
                     </tr>
                        
                        <tr>
                            <td class="leftside">
                                Select Class</td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_ClassSelect" runat="server" AutoPostBack="true" class="form-control"
                                    onselectedindexchanged="Drp_ClassSelect_SelectedIndexChanged" Width="160px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td class="rightside">
                                &nbsp;</td>
                        </tr>
                        
                        <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td class="rightside">
                                <asp:Button ID="Btn_Generate" runat="server"  Text="Show Report" Class="btn btn-primary" onclick="Btn_Generate_Click" /></td>
                        </tr>
                        
                        <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td class="rightside">
                                &nbsp;</td>
                        </tr>
                        
                        
                        
                  </table>
                  <div class="linestyle"></div>   
                   <asp:Label ID="Lbl_Message" runat="server" class="control-label"></asp:Label>
                   <asp:Panel ID="Pnl_ExamResults" runat="server" class="control-label">
                     <div style="max-height:200px;;max-height:450px;overflow:auto">
                          <asp:GridView ID="Grd_CreateReport" runat="server" AutoGenerateColumns="False" 
                            
                             Width="97%" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                            <Columns>
                            
                              <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_Exam" runat="server" Checked="true"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExamSchdId" HeaderText="Exam ScheduleId" />
                                <asp:BoundField DataField="IsCombined" HeaderText="IsCombined" />
                                <asp:BoundField DataField="ExamName" HeaderText="Exam Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                                
                                 <asp:TemplateField HeaderText="Cumulative Performance" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Cumulative" runat="server"  MaxLength="6" Text="0" class="form-control"
                                            Width="75"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Mark_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_Cumulative" ValidChars=".">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExamId" HeaderText="ExamId" />
                                  <asp:BoundField DataField="PeriodId" HeaderText="PeriodId" />
                            </Columns>
                              <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>                              
                    </div>
                 </asp:Panel>
                </asp:Panel>
      
                 
                
                 </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
        </ContentTemplate>
   </asp:UpdatePanel>

   
    <div class="clear"></div>
    </div>
    
</asp:Content>
