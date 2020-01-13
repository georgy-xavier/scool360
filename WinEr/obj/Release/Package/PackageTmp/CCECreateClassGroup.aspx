<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCECreateClassGroup.aspx.cs" Inherits="WinEr.CCECreateClassGroup1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function Validation() {
        var ChkBox1 = document.getElementById('<%=CheckBox1.ClientID%>');
        var ChkBox2 = document.getElementById('<%=CheckBox2.ClientID%>');
        var Upload1 = document.getElementById('<%=FileUpload1.ClientID%>');
        var Upload2 = document.getElementById('<%=FileUpload2.ClientID%>');
        ChkBox1.checked = false;
        ChkBox2.checked = false;
        Upload1.disabled = false;
        Upload2.disabled = false;
    }

    var validFilesTypes = ["xml"];

    function CheckExtension(file) {
        /*global document: false */
        var isValidFile = false;
        var filePath = file.value;
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }

        if (!isValidFile) {
            file.value = null;
            alert("Invalid File. Valid extensions are:\n\n" + validFilesTypes.join(", "));
        }
        return isValidFile;
    }

</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
    <ProgressTemplate>
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
                                   CCE&nbsp; Group Configuration</td>
                              <td class="ne">
                              </td>
                          </tr>
               
              <tr>
                <td class="o"></td>
                <td class="c" >
                
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                
                     <table class="tablelist">
                     
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                     
                     <tr>
                     <td class="leftside">Group Name</td>
                     <td class="rightside">
                         <asp:TextBox ID="Txt_groupname" runat="server" class="form-control" Width="250px"></asp:TextBox>
                           <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amt_FilteredTextBoxExtender" 
                           runat="server" Enabled="True" TargetControlID="Txt_groupname"  FilterType="Custom"  FilterMode="InvalidChars" InvalidChars=".!@#$%^&;*()~?><|\';:">
                           </ajaxToolkit:FilteredTextBoxExtender>
                  

   <br />
                     </td>
                     
                     </tr>
                     <tr>
                     <td class="leftside">&nbsp;Conversion Type</td>
                     <td class="rightside" style="height: 22px">
                         <asp:DropDownList ID="Drp_conversion" runat="server" class="form-control" AutoPostBack="true" Width="250px">
                         </asp:DropDownList>
                         <br />
                     </td>
                     </tr>
                     <tr>
                     <td class="leftside">&nbsp;Grade Master</td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_grademaster" runat="server" AutoPostBack="true" class="form-control col-lg-6" Width="250px">
                         </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                          <asp:Button ID="Btn_Add" runat="server" Class="btn btn-primary col-lg-6" style="width:100px" Text="Create" onclick="Btn_Add_Click" ToolTip="create map" />
                      <br />
                     </td>
                     </tr>
                     <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr> 
                     
                     <tr>
                     <td align="left">&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Label" Visible="false" class="control-label" ForeColor="Red"></asp:Label></td>
                     <td class="rightside"></td>
                     </tr>
                  
                    </table>
                    
                     <div id="divgrid" runat="server">
                     <table class="tablelist">
                        <tr>
                     <td colspan="2">
                     <hr />
                     </td>
                     </tr>  
                     <tr>
                     <td colspan="2">
                     <br />
                     </td>
                     </tr> 
                     <tr>
                     <td colspan="2" align="center">
                     <asp:GridView ID="Grd_CCE" runat="server"  AutoGenerateColumns="false" 
                                OnRowCommand="grd_courseRowCommand" >
                                <Columns>
                                <asp:BoundField DataField="GroupId" HeaderText="Id"/>
                                <asp:BoundField DataField="GroupName" HeaderText="Group Name"/>
                                <asp:BoundField DataField="ConvertionTypeId" HeaderText="CoversionTypeId"/>
                                <asp:BoundField DataField="ConvertionType" HeaderText="Coversion Type"/>
                                <asp:BoundField DataField="GradeMasterId" HeaderText="GradeMasterId"/>
                                <asp:BoundField DataField="GradeMaster" HeaderText="Grade Master"/>
                                <asp:BoundField DataField="TermXMLFile" HeaderText="Termwise Config"/>
                                <asp:BoundField DataField="ConsoldateXMLFile" HeaderText="Consolidate Config"/>
                                                 
                                <asp:buttonfield buttontype="Link"  commandname="ConfigEdit" ControlStyle-Width="50px"  text="&lt;img src='Pics/edit.png' width='25px' Hight='25px' border=0 title='Edit'&gt;"
                                 ItemStyle-ForeColor="Black"  HeaderText="Edit"   >
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>
                                
                                <asp:TemplateField ItemStyle-Width="60px" HeaderText="Remove" Visible="false">
                                <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" ImageUrl="Pics/DeleteRed.png" runat="server"  Width="30px"  CommandName="Remove"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Remove"  />
                                </ItemTemplate>
                                </asp:TemplateField>
                               
                                </Columns>
                            </asp:GridView>
                     </td>
                     </tr>
                     </table>
                     </div>
                     
                     <asp:Panel ID="Pnl_MessageBox" runat="server">
  <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server"  CancelControlID="Btn_magok" 
    PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
   <div class="container skin5" style="width:600px; top:500px;left:500px;" >
      <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Edit Group Configuration</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
            
            <div style="text-align:center;">
                        <table class="tablelist">
                           <tr>
                           <td colspan="2" align="center"><br /></td>
                           </tr>
                           <tr>
                           <td class="leftside">Group Name
                           </td>
                           <td class="rightside">
                               <asp:TextBox ID="Txt_editgroupname" runat="server" class="form-control" Width="250px"></asp:TextBox>
                           </td>
                           </tr>
                            <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                           <tr>
                           <td class="leftside">
                           Conversion Type
                           </td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_editconverisontype" runat="server" class="form-control" Width="250px">
                               </asp:DropDownList>
                           </td>
                           </tr>
                            <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                           <tr>
                           <td class="leftside">Grade Master</td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_editgrademaster" runat="server" class="form-control" Width="250px">
                               </asp:DropDownList>
                           </td>
                           </tr>
                            <tr>
                     <td class="leftside"></td>
                     <td class="rightside"></td>
                     </tr>
                        
                           <tr>
                           <td class="leftside"></td>
                           <td class="rightside">
                             <asp:CheckBox ID="CheckBox1" AutoPostBack="true" runat="server" Checked="false" OnCheckedChanged="CheckBox1_CheckedChanged"/> If You want update Termwise XML File <br />
                             <asp:FileUpload ID="FileUpload1" runat="server" onchange="return CheckExtension(this);" /> <br /> 
                             <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="CheckBox2_CheckedChanged"/> If You want update Consolidate XML File<br />
                             <asp:FileUpload ID="FileUpload2" runat="server" onchange="return CheckExtension(this);"/> 
                           </td>
                           </tr>
                           
                            <tr>
                            <td colspan="2" align="center"><br /></td>
                            </tr>
                           <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="Lbl_Err" Visible="false" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
                            </td>
                           </tr>
                           
                           <tr>
                           <td align="center" colspan="2">
                           <asp:Button ID="Btn_Create" runat="server" Text="Update" ToolTip="Update" Class="btn btn-success" OnClick="Btn_Create_Click"/>
                           <asp:Button ID="Btn_magok" runat="server" Text="Close"  ToolTip="Close" Class="btn btn-danger"  OnClientClick="Validation()"/>
                           </td>
                           </tr>
                        <tr>
                        <td colspan="2" align="center"><br /></td>
                        </tr>
                        </table>
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
              This group is asign to many class.If you want remove this group name.
            </td>
            </tr>
            <tr>
            <td colspan="2">
                <asp:Label ID="Lbl_groupid" Visible="false" class="control-label" runat="server" Text="Label"></asp:Label>
            </td>
            </tr>
            
           
            </table>
             <br />
         
            </div>
              <div style="text-align:center;">
              <asp:Button ID="Btn_yes" runat="server" Class="btn btn-info" Text="Yes" ToolTip="Remove selected group" OnClick="Btn_yes_Click"/>
              <asp:Button ID="Btn_no" runat="server" Class="btn btn-info" Text="No"  ToolTip="Cancel"/>
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
                
                </td>
                <td class="e"></td>
                </tr>
             
              <tr>
               <td class="so"></td>
               <td class="s"></td>
               <td class="se"></td>
               </tr>
             </table>
       </div>
       <WC:MSGBOX id="WC_MessageBox" runat="server"/>   
      </ContentTemplate>
      <Triggers>
      <asp:PostBackTrigger ControlID="Btn_Create" />
      </Triggers>
      </asp:UpdatePanel>
      
      
</asp:Content>
