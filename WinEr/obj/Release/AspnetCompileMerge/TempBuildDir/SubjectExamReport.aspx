<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="SubjectExamReport.aspx.cs" Inherits="WinEr.SubjectExamReport" %>
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
                                  Subject Wise Exam Report</td>
                              <td class="ne">
                              </td>
                          </tr>
                          <tr>
                              <td class="o">
                              </td>
                              <td class="c">
                                  <br />
                                  <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                                      <table class="tablelist">
                                          <tr>
                                              <td class="leftside" >
                                                  Select Class</td>
                                              <td class="rightside">
                                                  <asp:DropDownList ID="Drp_ClassSelect" runat="server" AutoPostBack="True" class="form-control"
                                                      onselectedindexchanged="Drp_ClassSelect_SelectedIndexChanged" Width="140px">
                                                  </asp:DropDownList>
                                              </td>
                                          </tr>
                                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                          <tr>
                                              <td class="leftside">
                                                  Select Subject</td>
                                              <td class="rightside">
                                                  <asp:DropDownList ID="Drp_Subject" runat="server" AutoPostBack="True" class="form-control"
                                                     Width="140px">
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
                                                 <td valign="top" class="leftside">
                                                     Choose Exams</td>
                                              <td  class="rightside">
                                                  <asp:GridView ID="grd_exams" runat="server" AutoGenerateColumns="False" 
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                                                      
                                                  <Columns>
                                                  <asp:BoundField DataField="Type" HeaderText="Type" />
                                                  <asp:BoundField DataField="Id" HeaderText="Id" />
                                                  <asp:TemplateField HeaderText=" ">
                                                  <ItemTemplate>
                                                      <asp:CheckBox ID="chk_select" runat="server" />
                                                  </ItemTemplate>
                                                  </asp:TemplateField>
                                                    <asp:BoundField DataField="ExamName" HeaderText="Exam Name"  />

                                                  </Columns>
                                                      <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                                                  </asp:GridView>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td colspan="2">
                                                  <asp:Label ID="Lbl_Message" runat="server" class="control-label"></asp:Label></td>
                                          </tr>
                                          
                                          <tr>
                                              <td class="leftside">
                                                 
                                              </td>
                                              <td class="rightside">
                                                  <asp:Button ID="Btn_Generate" runat="server" OnClick="Btn_Generate_Click" 
                                                      Text="Show" Class="btn btn-primary" ToolTip=" Show Report"/>
                                                  &nbsp;&nbsp;&nbsp;
                                                  <asp:ImageButton ID="Img_Export" runat="server" Height="35px" 
                                                      ImageUrl="~/Pics/Excel.png" OnClick="Img_Export_Click" Width="35px" ToolTip="Export to Excel"/>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td class="leftside">
                                                  &nbsp;</td>
                                              <td class="rightside">
                                                  &nbsp;</td>
                                          </tr>
                                          <tr>
                                              <td colspan="2">
                                                  <asp:GridView ID="grdResult" runat="server"  
                                                      GridLines="None" Width="100%"
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                     <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                                                  </asp:GridView>
                                              </td>
                                          </tr>
                                      </table>
                                  </asp:Panel>
                              </td>
                              <td class="e">
                              </td>
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



<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" align="center" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary" Width="50px"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>

        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="Img_Export"/>
        </Triggers>
    </asp:UpdatePanel>
        
        
          <div class="clear"></div>

</div>






</asp:Content>
