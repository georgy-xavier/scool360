<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEColumConfig.aspx.cs" Inherits="WinEr.CCEColumConfig" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <style type="text/css">
     .popupbuttonstyle
     {
         background: #E0691A url(../winbuttons/submit1.gif) no-repeat;
         border: 0px;
         padding: 2px 0px;
         text-align:center;
         padding-left:5px;
         height:24px;
         color: #000;
         font: bold 11px Arial, Sans-Serif;
         background-position:left;
         width:50px;
         padding-right:5px;
     }
 </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                  &nbsp;CCE Column Config</td>
                              <td class="ne">
                              </td>
                          </tr>
                
                <tr >
                <td class="o"></td>
                <td class="c">
                
                 <table class="tablelist">

                 <tr>
                 <td align="left">
                 <asp:Button ID="Btn_Add" runat="server" onclick="Btn_Add_Click" 
                        Text="Add" class="btn btn-primary" ToolTip="Click"  />
                        <br />
                 </td>
                 </tr>
                 <tr>
                 <td align="left">
                 &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Label" class="control-label" Visible="false" ForeColor="Red"></asp:Label>
                 
                 </td>
                 </tr>
           
                 <tr>
                 <td align="center">
                
                 <div id="Griddiv" runat="server" style="width:auto;height:300px;overflow:auto">
                 <br />
                 <hr />
                 <br />
                 <asp:GridView ID="Grd_CCE" runat="server" Width="100%"  AutoGenerateColumns="false" 
                                OnRowCommand="grd_courseRowCommand"  >
                    <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" />
                        <asp:BoundField DataField="GroupId" HeaderText="Group Id" />
                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                        <asp:BoundField DataField="TremId" HeaderText="Term Id" />
                        <asp:BoundField DataField="TermName" HeaderText="Term Name" />
                        <asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
                        <asp:BoundField DataField="TableName" HeaderText="Table Name" />
                        <asp:BoundField DataField="ColumnName" HeaderText="Column Name" />
                        <asp:BoundField DataField="MaxMark"  HeaderText="Maximum Exam Mark" />
   
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="Remove">
                        <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" ImageUrl="Pics/DeleteRed.png" runat="server"  Width="30px"  CommandName="Remove"
                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Remove"/>
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
  
  <asp:Panel ID="Pnl_MessageBox" runat="server">
  <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server"  CancelControlID="Btn_magok" 
    PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
   <div class="container skin5" style="width:400px; top:400px;left:400px" >
      <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Column Configuration</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >

                        <br /><br />
                        <div style="text-align:center;">
                        <table class="tablelist">
                           <tr>
                           <td class="leftside"></td>
                           <td class="rightside">
                           </td>
                           </tr>
                           <tr>
                           <td class="leftside">Exam Name</td>
                           <td class="rightside">
                               <asp:TextBox ID="Txt_Exam" runat="server" Width="160px" class="form-control" MaxLength="150" ToolTip="Enter the Exam name"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amt_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" TargetControlID="Txt_Exam"  FilterType="Custom"  FilterMode="InvalidChars" InvalidChars=".!@#$%^&;*()~?><|\';:">
                                </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                           <tr>
                           <td class="leftside">Enter Max Exam Mark</td>
                           <td class="rightside">
                               <asp:TextBox ID="Txt_maxmark" runat="server" Width="160px" MaxLength="3" class="form-control" ToolTip="Enter the mark"></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_maxmark_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="Txt_maxmark">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside">Group Name</td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_Groupname" runat="server" class="form-control" Width="160px" >
                               </asp:DropDownList>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside">Term Name</td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_Termname" runat="server" class="form-control" Width="160px">
                               </asp:DropDownList>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside">Table Name</td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_Tablename" runat="server" Width="160px" class="form-control" OnSelectedIndexChanged="Drp_Tablename_SelectedIndexChanged" AutoPostBack="true">
                               </asp:DropDownList>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside">Column Name</td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_Columnname" runat="server" class="form-control" Width="160px" >
                               </asp:DropDownList>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside"></td>
                           <td class="rightside">
                           </td>
                           </tr>
                           
                        </table>
                        <br />
                        <table class="tablelist">
                        <tr>
                        <td class="leftside">
                        </td>
                        <td class="rightside">
                         <asp:Button ID="Btn_Create" runat="server" Text="Create" Class="btn btn-success" ToolTip="Create Column config" onclick="Btn_Create_Click"/>
                         <asp:Button ID="Btn_magok" runat="server" Text="Close" Class="btn btn-danger" ToolTip="Close" />
                        </td>
                        </tr>
                        </table>
                        </div>
                        <br /><br />
                         
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
            <td class="o"></td>
            <td class="c">
            <div style="text-align:left">
            <table>
           
            <tr>
            <td>
             <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/alert.png" 
                        Height="28px" Width="29px" />
            </td>
            <td>
              Are you sure remove this colum configuration.
            </td>
            </tr>
            <tr>
            <td colspan="2">
                <asp:Label ID="Lbl_groupid" Visible="false" runat="server" class="control-label" Text="Label"></asp:Label>
            </td>
            </tr>
            
            </table>
             </div>
             
             <div style="text-align:center;">
              <asp:Button ID="Btn_yes" runat="server" Class="btn btn-info" Text="Yes" ToolTip="Remove selected group" OnClick="Btn_yes_Click"/>
              <asp:Button ID="Btn_no" runat="server" Class="btn btn-info" Text="No"  ToolTip="Cancel"/>
              <br />
              <br />
              </div>
            </td>
            <td class="e"></td>
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
  
  
   
       
</asp:Content>
