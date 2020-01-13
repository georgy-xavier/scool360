<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEClassGroupMap.aspx.cs" Inherits="WinEr.CCEClassGroupMap" %>
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
                                  CCE Class Configuration</td>
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
                     <td class="leftside">Group Name</td>
                     <td class="rightside">
                      <asp:DropDownList ID="Drp_Groupselect" runat="server" AutoPostBack="True" 
                                                             Width="250px" class="form-control" OnSelectedIndexChanged="Drp_Groupselect_SelectedIndexChanged">      
                       </asp:DropDownList>
                     </td>
                     </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     
                     
                     <tr>
                     <td class="leftside">
                     &nbsp;Select Term
                     </td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_term" runat="server" class="form-control"  Width="250px">
                         </asp:DropDownList>
                     </td>
                     </tr>
                     
                    
                     
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                     
                     
                     <tr>
                      <td class="leftside">&nbsp;Class Name</td>
                      <td class="rightside">
                      <asp:DropDownList ID="Drp_ClassSelect" runat="server" 
                                                   class="form-control col-lg-6"    Width="250px">
                                                  </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                   <asp:Button ID="Btn_map" runat="server" Text="Add" Class="btn btn-primary col-lg-6" style="width:100px"
                                     ToolTip="Add" onclick="Btn_map_Click"/>
                      </td>
                     </tr>

                     <tr>
                     <td align="left">&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" class="control-label" Visible="false"></asp:Label></td>
                     <td class="rightside"></td>
                     </tr>
                     
                     </table>
                     
                  <div id="divgrid" runat="server" style="width:auto;height:300px;overflow:auto">
                     <table class="tablelist">
                         <caption>
                             <hr />
                             <br />
                             <tr>
                                 <td align="center">
                                     <asp:GridView ID="Grd_CCE" runat="server" AllowPaging="false" 
                                         AutoGenerateColumns="False" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                                         BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                                         Font-Size="12px" OnRowCommand="Grd_studentlist_RowCommand" PageSize="10" 
                                         Width="100%">
                                         <Columns>
                                             <asp:BoundField DataField="GroupId" HeaderText="Group Id" />
                                             <asp:BoundField DataField="ClassId" HeaderText="Class Id" />
                                             <asp:BoundField DataField="TermId" HeaderText="Class Id" />
                                             <asp:BoundField  DataField="GroupName" HeaderText="Group Name" />
                                             <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                             <asp:BoundField DataField="TermName" HeaderText="Term Name" />
                                             <asp:TemplateField HeaderText="Remove" ItemStyle-Width="30px">
                                                 <ItemTemplate>
                                                     <asp:ImageButton ID="ImageButton1" runat="server" 
                                                         CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                                         CommandName="Remove" ImageUrl="Pics/DeleteRed.png" 
                                                         ToolTip="Remove" Width="30px" />
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                         </Columns>
                                         <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" 
                                             PreviousPageText="&lt;&lt;" />
                                         <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                         <EditRowStyle Font-Size="Medium" />
                                         <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                         <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                             ForeColor="Black" HorizontalAlign="Left" />
                                         <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                             ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                                     </asp:GridView>
                                 </td>
                             </tr>
                         </caption>
                     
                     </table>
                  </div>
                  
                  <asp:Panel ID="YesOrNoMessageBox" runat="server">
  <asp:Button ID="Btn_header" runat="server" class="btn btn-info" style="display:none" />
  <ajaxToolkit:ModalPopupExtender ID="MPE_yesornoMessageBox" runat="server" 
    CancelControlID="Btn_no" PopupControlID="Pnl_yesornomsg" TargetControlID="Btn_header"/>
 
   <asp:Panel ID="Pnl_yesornomsg" runat="server" style="display:none;">
   <div class="container skin5" style="width:400px; top:600px;left:500px;" >
      <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"></td>
            <td class="n"><span style="color:White">Alert Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
         <tr >
            <td class="o"> </td>
            <td class="c" >
            <div style="text-align:left">
            <br />
            <table>
           
            <tr>
            <td>
             <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" 
                        Height="28px" Width="29px" />
            </td>
            <td>
              This class mapped.If you want remove this mapping.
            </td>
            </tr>
            <tr>
            <td colspan="2">
                <asp:Label ID="Lbl_groupid" Visible="false" runat="server" class="control-label" Text="Label"></asp:Label>
            </td>
            </tr>
            
           
            </table>
             <br />
         
            </div>
              <div style="text-align:center;">
              <asp:Button ID="Btn_yes" runat="server" Class="btn btn-success" Text="Yes" ToolTip="Remove selected group" OnClick="Btn_yes_Click"/>
              <asp:Button ID="Btn_no" runat="server" Class="btn btn-danger" Text="No"  ToolTip="Cancel"/>
              <br />
              <br />
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
   </div>
   </asp:Panel>
    
      
  
  </asp:Panel>
                    
                    
                      
                   
                     
                     
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
    
    <div class="clear"></div>
</div>

</asp:Content>
