<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="ViewRegisteredStudents.aspx.cs" Inherits="WinEr.ViewRegisteredStudents" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
   .Watermarkcss
   {
       color:Gray;
   }
  </style>
  
  <script type="text/javascript">
      function SelectAll(cbSelectAll) {
          debugger;
          var gridViewCtl = document.getElementById('<%=Grd_studentlist.ClientID%>');
          var Status = cbSelectAll.checked;
          for (var i = 1; i < gridViewCtl.rows.length; i++) {

              var cb = gridViewCtl.rows[i].cells[0].children[0].children[0];
              cb.checked = Status;
          }
          // Calculate();
      }
      function openIncpopup(strOpen) {
          open(strOpen, "Info", "status=1, width=800, height=600,resizable = 1");
      }
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    &nbsp;<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
         
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
         
    <div id="contents">
         

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
     <ContentTemplate>
    
   
           
           
           
      <asp:Panel ID="Pnl_TempList" runat="server" DefaultButton="Btn_TempSearch" >
              
        <div class="container skin1"  >
         <table  cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no"><img alt="" src="Pics/search_female_user.png" height="35" width="35" />  </td>
                <td class="n">Registered Students</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
              <div >
              <center>
                    <table width="800px">
                    
                     <tr>
                        <td align="center" id="Colstaus" runat="server">
                                <asp:RadioButtonList class="form-actions" ID="Rdb_Type" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">Registered Students</asp:ListItem>
                                <asp:ListItem  Value="1">Enrolled/Removed Students</asp:ListItem>
                                </asp:RadioButtonList>                                 
                         </td>
                         <td align="right">Status:</td>
                         <td align="left"><asp:DropDownList ID="Drp_Status" class="form-control" runat="server" Width="180px"></asp:DropDownList></td>
                        <td  align="right">
                             Batch:
                           </td>
                           <td  align="left">
                             <asp:DropDownList ID="Drp_batch" class="form-control" runat="server" Width="170px" >
                                </asp:DropDownList>
                           </td>
                            <td  align="right">
                               Standard:
                           </td>
                           <td  align="left">
                               <asp:DropDownList ID="Drp_PopUp_Standardlist" class="form-control" runat="server" Width="170px" AutoPostBack="true" OnSelectedIndexChanged="Drp_PopUpStd_SelectedIndex">
                                </asp:DropDownList>
                           </td>
                           
                        </tr>
                        
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                        
                        <tr>
                       
                         <td align="right">Class:</td>
                         <td align="left"><asp:DropDownList ID="Drp_PopUp_Class" class="form-control" runat="server" Width="180px" >
                                 </asp:DropDownList></td>
                        <td  align="right">Name:
                            
                           </td>
                           <td  align="left">
                             <asp:TextBox ID="txt_TempName" Text="" class="form-control" runat="server" Width="170px" Height="34px"></asp:TextBox>
                             <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Startdate_TextBoxWatermarkExtender"  WatermarkText="All" 
                                   runat="server" Enabled="True" TargetControlID="txt_TempName">
                               </ajaxToolkit:TextBoxWatermarkExtender>
                           </td>
                            <td  align="center" colspan="2">
                             <asp:Button ID="Btn_ChangeStatus" runat="server" class="btn btn-primary" Text="Edit Status" Width="100px" 
                                           onclick="Btn_ChangeStatus_Click" />
                          
                               <asp:Button ID="Btn_TempSearch" runat="server" class="btn btn-primary" 
                                        onclick="Btn_TempSearch_Click" Text="Search" />
                           </td>
                           
                        </tr>
                        <tr>
                        <td colspan="6" align="center">
                       <asp:Label ID="Lbl_Msg" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                        </td>
                        </tr>
                        </table>
                        
                           
                            <div class="linestyle"></div>
                            <table width="100%">
                                                            <tr>
                                    <td>
                                        <asp:Label ID="lbl_PaymentType" class="control-label" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_TempError" class="control-label" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    <td align="right" style="width:10%">
                                        <asp:ImageButton ID="Img_AltImage" runat="server" Height="35px" 
                                            ImageUrl="~/Pics/users11.png" onclick="Img_AltImage_Click" 
                                            ToolTip="View Allotments" Width="35px" />
                                    </td>
                                    <td align="left" style="width:20%">
                                        <asp:LinkButton ID="Lnk_Allotment" class="form-button" runat="server" OnClick="ViewAloment_Click">View Allotments</asp:LinkButton>
                                    </td>
                                    <td align="right" style="width:10%">
                                        <asp:ImageButton ID="Img_Export" runat="server" Height="35px" 
                                            ImageUrl="~/Pics/Excel.png" onclick="Img_Export_Click" 
                                            ToolTip="Export to Excel" Visible="false" Width="35px" />
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="4" align="left">
                                <asp:Label ID="Lbl_StudentCount" class="control-label" runat="server"></asp:Label>
                               <asp:Button ID="Btn_SendMessage" runat="server" class="btn btn-primary" Text="Send SMS" Width="90px" 
                                        onclick="Btn_SendMessage_Click"/>
                                </td>
                                </tr>
                                <tr>
                                    <td colspan="4"><%--OnPageIndexChanging="Grd_Student_PageIndexChanging" --%>
                                        <div style="overflow:auto;min-height:200px;max-height: 550px;">
                                            <asp:GridView ID="Grd_studentlist" runat="server" AllowPaging="false" 
                                                AutoGenerateColumns="false" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                                                BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                                                Font-Size="12px" 
                                                onrowcommand="Grd_studentlist_RowCommand" 
                                                onrowdeleting="Grd_studentlist_RowDeleting"   
                                                onrowediting="Grd_studentlist_RowEditing" 
                                                onselectedindexchanged="Grd_studentlist_SelectedIndexChanged" PageSize="10" 
                                                Width="100%">
                                                <Columns>
                                         <asp:TemplateField   HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px"> 
                                        <HeaderTemplate > 
                                                <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" 
                                                            onclick="SelectAll(this)" Text=" All" />
                                        </HeaderTemplate>
                                        <ItemTemplate  >
                                                <asp:CheckBox ID="CheckBoxUpdate" class="form-actions" runat="server"/>
                                        </ItemTemplate> 
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left"  />
                            
                                        </asp:TemplateField>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                    <asp:BoundField DataField="TempId" HeaderText="TemporaryId" 
                                                        ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="standard" HeaderText="Standard" 
                                                        ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ClassName" HeaderText="Class" ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Rank" HeaderText="Rank" ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Batch" HeaderText="Batch" ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="75" >
                                                        <ItemStyle Width="75px" />
                                                    </asp:BoundField>    
                                                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" /> 
                                                     <asp:BoundField DataField="CommentThreadId" HeaderText="Comment Thread Id" />                                             
                                                    <asp:CommandField DeleteText="&lt;img src='Pics/edit.png' width='30px' border=0 title='Edit Details' &gt;" 
                                                        HeaderText="Edit" ItemStyle-Width="35" ShowDeleteButton="True" >
                                                        
                                                        <ItemStyle Width="35px" />
                                                    </asp:CommandField>
                                                        
                                                    <asp:TemplateField HeaderText="Add Comment"  >
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Img_Comment" ImageUrl="~/Pics/comments.png" Width="30px" 
                                                            runat="server" onclick="Img_Comment_Click" ></asp:ImageButton>                              
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>                                                    
                                                    <asp:CommandField HeaderText="Fee" ItemStyle-Width="35" 
                                                        SelectText="&lt;img src='pics/dollar.png' width='30px' border=0 title='Collect Fee'&gt;" 
                                                        ShowSelectButton="True" >
                                                        <ItemStyle Width="35px" />
                                                    </asp:CommandField>
                                                    <asp:CommandField EditText="&lt;img src='Pics/DeleteRed.png' width='30px' border=0 title='Delete' &gt;" 
                                                        HeaderText="Delete" ItemStyle-Width="35" ShowEditButton="true" >
                                                        <ItemStyle Width="35px" />
                                                    </asp:CommandField>
                                                    <asp:ButtonField buttontype="Link" commandname="ViewStudent" HeaderText="Show" 
                                                        ItemStyle-Width="35" 
                                                        
                                                        text="&lt;img src='Pics/Details.png' width='30px' border=0 title='View Details' &gt;" >
                                                        <ItemStyle Width="35px" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField buttontype="Link" commandname="Enroll" HeaderText="Enroll" 
                                                        ItemStyle-Width="35" 
                                                        
                                                        text="&lt;img src='Pics/accept.png' width='30px' border=0 title='Enroll' &gt;" >
                                                        <ItemStyle Width="35px" />
                                                    </asp:ButtonField>
                                                    
                                                     
                                                  
                                                    
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
                                        </div>
                                    </td>
                                </tr>
                            </table>
                             </center>
              </div>
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
      </asp:Panel>

        


      
        <asp:Button runat="server" ID="Btn_RankEdit" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  
                                  runat="server"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_RankEdit"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none">
                         <div class="container skin5" style="width:600px; top:210px;left:400px; height:550px; " >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Edit Details</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
             <div  style="height:500px; overflow:scroll">
               <table class="tablelist">
                     
                         <tr>
                             <td class="leftside">
                                 Full Name<span style="color:Red">*</span>
                             </td>
                             <td class="rightside">
                                 <asp:TextBox ID="txt_EditName"  class="form-control" runat="server" Width="180px" MaxLength="59" 
                                     TabIndex="1"></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="txt_EditName">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Edit" ControlToValidate="txt_EditName" ErrorMessage="*"></asp:RequiredFieldValidator>
                             </td>
                        </tr>
                         <tr>     
                          <td class="leftside">Father Name </td>
                     <td class="rightside">
                         <asp:TextBox ID="txt_EditFatherName" class="form-control" runat="server" Width="180px" MaxLength="59" 
                             TabIndex="2"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="txt_EditFatherName">
                                     </ajaxToolkit:FilteredTextBoxExtender>
                     </td>
                          
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                         
                           <tr>
                           <td class="leftside">
                                 Mother Name
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="txtMotherName" class="form-control" runat="server"  Width="180px" TabIndex="11"> </asp:TextBox>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                     <tr>
                      <td class="leftside">
                                 Sex<span style="color:Red">*</span> </td>
                             <td class="rightside">
                             
                                                <div class="radio radio-primary">
                                                 <asp:RadioButtonList ID="Rdb_EditSex" class="form-actions" runat="server" 
                                                     RepeatDirection="Horizontal" TabIndex="3" Width="160px">
                                                     <asp:ListItem Selected="True" Value="0">Male</asp:ListItem>
                                                     <asp:ListItem Value="1">Female</asp:ListItem>
                                                 </asp:RadioButtonList>
                                                 </div>
                             
                             
                             
                             
                                 <%--<asp:RadioButtonList ID="Rdb_EditSex"  runat="server" 
                                     RepeatDirection="Horizontal" TabIndex="3" Width="160px">
                                     <asp:ListItem Selected="True" Value="0">Male</asp:ListItem>
                                     <asp:ListItem Value="1">Female</asp:ListItem>
                                 </asp:RadioButtonList>--%>
                             </td>
                     
                       </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                        <tr>
                         <td class="leftside">
                             Standard<span style="color:Red">*</span> <br />
                         </td>
                          <td class="rightside">
                         <asp:DropDownList ID="Drp_EditStd" class="form-control" runat="server" 
                              TabIndex="4" Width="180px" AutoPostBack="True" 
                                  onselectedindexchanged="Drp_EditStd_SelectedIndexChanged" >
                         </asp:DropDownList> 
                         </td>                                       
                     </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                       <tr>
                         <td class="leftside">
                             Class<span style="color:Red">*</span> <br />
                         </td>
                          <td class="rightside">
                         <asp:DropDownList ID="Drp_EditClass" class="form-control" runat="server" 
                              TabIndex="5" Width="180px" >
                         </asp:DropDownList> 
                         </td>                                       
                     </tr>
                     
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                     
                     <tr>
                     
                      <td class="leftside">
                               Academic Year<span style="color:Red">*</span> </td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_EditYear" class="form-control" runat="server" Width="180px" 
                             TabIndex="6" >
                         </asp:DropDownList>
                         <br />
                             
                     </td>
                     </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                     <tr>
                     
                             <td class="leftside">
                                 Interview Rank<span style="color:Red">*</span></td>
                             <td class="rightside">
                                   <asp:TextBox ID="txt_EditRank" runat="server" class="form-control" MaxLength="7" Width="180px" 
                                       TabIndex="7"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_EditRank" 
                        ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Edit" ControlToValidate="txt_EditRank" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                       
                       
                      </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                     <tr>
                     
                             <td class="leftside">
                                 Phone</td>
                             <td class="rightside">
                                   <asp:TextBox ID="txt_EditPhone" class="form-control" runat="server" MaxLength="14" Width="180px" 
                                       TabIndex="8"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_EditPhone" 
                        ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender></td>
                       
                      </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                       <tr>
                           <td class="leftside">
                                 DOB
                           </td>
                             <td  class="rightside">
                                    <asp:TextBox ID="Txt_Dob" class="form-control" runat="server" Width="180px" TabIndex="9"></asp:TextBox> 
                                    <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"  
                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                        Mask="99/99/9999"
                                        UserDateFormat="DayMonthYear"
                                        Enabled="True" 
                                        TargetControlID="Txt_Dob">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <span style="color:Blue">DD/MM/YYYY</span>

                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Dob" ErrorMessage="You Must enter D.O.B"  ValidationGroup="SubmitDetails"  > </asp:RequiredFieldValidator>
                   
                                    <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                ControlToValidate="Txt_Dob"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="DobDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" /><br/>
                               
      
                               
                                </td>
                                
                          </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                         <tr>
                           
                      <td class="leftside" > Address</td>
                     <td class="rightside">
                         <asp:TextBox ID="txt_EditAddress" class="form-control" runat="server" TextMode="MultiLine" Width="180px" 
                             MaxLength="240" Height="51px" TabIndex="9"></asp:TextBox>
                    
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="txt_EditAddress">
                                     </ajaxToolkit:FilteredTextBoxExtender>  
                                      </td>
                       </tr>      
                        <tr>
                        <td colspan="2" align="center">
                        &nbsp;</td>
                     </tr> 
                     
                       <tr>
                       <td colspan="2"> 
                       <center>
                           <asp:LinkButton ID="Show_MoreInfo" runat="server" onclick="Show_MoreInfo_Click">Add More Details</asp:LinkButton>
                         
                           
                           <asp:LinkButton ID="Hide_MoreInfo" runat="server" Visible="false" 
                               onclick="Hide_MoreInfo_Click">Hide More Details</asp:LinkButton>
                         
                           
                       </center>
                       </td>
                       </tr>
                      
                       <tr>
                       <td colspan="2">
                           <asp:Panel ID="Panel_MoreInfo" runat="server" Visible="false">                          
                          <table class="tablelist">
                          
                        
                           
                           <tr>
                           <td class="leftside">
                                 Location
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Location" class="form-control" runat="server"  Width="180px" TabIndex="12"> </asp:TextBox>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                           <tr>
                           <td class="leftside">
                                 State
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_State" class="form-control" runat="server"  Width="180px" TabIndex="13"> </asp:TextBox>
                           </td>
                           </tr>
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                            <tr>
                           <td class="leftside">
                                 Nationality
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_nationality" class="form-control" runat="server"  Width="180px" TabIndex="14"> </asp:TextBox>
                           </td>
                           </tr>   
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>   
                           
                           <tr>
                           <td class="leftside">
                                 Pin Code
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_PinCode" class="form-control" runat="server"  Width="180px" TabIndex="15" Text="0"> </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                 Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_PinCode" 
                                    ValidChars="+">
                                </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                                                                           
                          
                           <tr>
                           <td class="leftside">
                                 Blood Group
                           </td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_BloodGrp" class="form-control" runat="server"  Width="180px"      TabIndex="16">
                                      </asp:DropDownList>
                           </td>
                           </tr>    
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>                                                                   
                           
                            <tr>
                           <td class="leftside">
                                 Mother Tongue
                           </td>
                           <td class="rightside">
                                 <asp:DropDownList ID="Drp_MotherTongue" class="form-control" runat="server" Width="180px"      TabIndex="17">
                                 </asp:DropDownList>
                           </td>
                           </tr>    
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Fathers Education Qualification
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_father_educ" class="form-control" runat="server" Width="180px" TabIndex="18"> </asp:TextBox>
                           </td>
                           </tr>    
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Motrhers Education Qualification
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Mothers_educ" class="form-control" runat="server" Width="180px" TabIndex="19"> </asp:TextBox>
                           </td>
                           </tr>    
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Fathers Occupation
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_fathers_Ocuptn" class="form-control" runat="server" Width="180px" TabIndex="20"> </asp:TextBox>
                           </td>
                           </tr>    
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Mothers Occupation
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_mothers_Ocuptn" class="form-control" runat="server" Width="180px" TabIndex="20"> </asp:TextBox>
                           </td>
                           </tr> 
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Annual Income
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_annual_incum" class="form-control" runat="server" MaxLength="7" Width="180px" Text="0" TabIndex="21"> </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" 
                                 Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_annual_incum" 
                                    ValidChars="+">
                                </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr>  
                           
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>    
                           
                            <tr>
                           <td class="leftside">
                                 Email ID
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Email" class="form-control" runat="server" Width="180px" MaxLength="45"      TabIndex="22"></asp:TextBox>
                                
                                <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_Email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                                
                                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                           </td>
                           </tr> 
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                           
                           <tr>
                           
                           <td class="leftside">
                           Previous Board
                           </td>
                           <td class="rightside"> 
                             <asp:TextBox ID="txtPreviousBoard" class="form-control" runat="server" TextMode="MultiLine"  Width="180px"    TabIndex="23"></asp:TextBox>
                           </td>
                           
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           <tr>
                           <td class="leftside">
                           Personal interview
                           </td>
                            <td class="rightside">
                                <asp:RadioButtonList ID="rdoInterView"  runat="server" AutoPostBack="true"      TabIndex="24"
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="rdoInterView_SelectedIndexChanged">
                                <asp:ListItem Text="Attended" Value="Attended"></asp:ListItem>
                                <asp:ListItem Text="Not Attended" Value="NotAttended"></asp:ListItem>
                                <asp:ListItem Text="Provisional Admmission" Value="ProvAdm"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                           <tr>
                           <td colspan="2">
                           <asp:Panel ID="pnlInterviewDetails" runat="server">
                           <table class="tablelist" width="100%">
                           <tr>
                           <td class="leftside"> Date of Interview</td>
                           <td  class="rightside">
                                    <asp:TextBox ID="txtDOI" runat="server" class="form-control" Width="180px" TabIndex="25"></asp:TextBox> 
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                        Mask="99/99/9999"
                                        UserDateFormat="DayMonthYear"
                                        Enabled="True" 
                                        TargetControlID="txtDOI">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <span style="color:Blue">DD/MM/YYYY</span>

                                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                ControlToValidate="txtDOI"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="DobDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" /><br/>
                               
      
                               
                                </td>
                           
                           </tr>
                           <tr>
                           <td class="leftside">
                           Teacher's remarks
                           </td>
                           <td class="rightside">
                               <asp:TextBox ID="txtTeacherRemark" class="form-control" runat="server" TextMode="MultiLine"   Width="180px"   TabIndex="26"></asp:TextBox>
                           </td>
                           
                           </tr>
                           <tr>
                           <td class="leftside">
                            HM's remarks</td>
                            <td class="rightside">
                            <asp:TextBox ID="txtHMRemark" class="form-control" runat="server" TextMode="MultiLine"  Width="180px"    TabIndex="27"></asp:TextBox></td>
                           </tr>
                            <tr>
                            <td  class="leftside">
                             Principal Remarks 
                            </td>
                            <td  class="rightside">
                                <asp:TextBox ID="txtPrincipalRemark" class="form-control" runat="server" TextMode="MultiLine" Width="180px" TabIndex="28"></asp:TextBox>                            
                            </td>                            
                            </tr>
                            <tr>
                            <td  class="leftside">
                            Result
                            </td>
                            <td class="rightside">
                                <asp:RadioButtonList ID="rdoResult"  runat="server" RepeatDirection="Horizontal"  TabIndex="29">
                                <asp:ListItem Selected="True"  Text="Selected" Value="Selected"></asp:ListItem>
                                <asp:ListItem   Text="Not Selected" Value="NotSelected"></asp:ListItem>
                                <asp:ListItem   Text="Hold" Value="Hold"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            </tr>
                             </table>
                            </asp:Panel>
                           </td>
                           </tr>
                             <tr>
                           <td  class="leftside">
                           Remark
                           
                           </td>
                           <td class="rightside">
                               <asp:TextBox ID="txtStudRemark" runat="server" class="form-control" TextMode="MultiLine" Width="180px"  TabIndex="30"></asp:TextBox>
                           </td>
                           </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                            <asp:Panel ID="Pnl_custumarea" runat="server">
                    
              
                     
                     
                     
                     
                     
                           </table>
                           
                            
                           </asp:Panel>
                       </td></tr> 
                       
                        
                
                <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
                 <div class="linestyle">  </div> 
                                
                    </asp:Panel> 
                           
                     <tr>
                     <td >
                        &nbsp;</td>
                        <td  align="center">
                        <asp:Button ID="Btn_magok" runat="server" class="btn btn-success" Text="Update"  ValidationGroup="Edit"
                                onclick="Btn_magok_Click"/>
                            <asp:Button ID="Btn_RankCancel" runat="server" class="btn btn-danger" runat="server" Text="Cancel" onclick="Btn_RankCancel_click" /></td>
                            
                         <asp:HiddenField ID="Hdn_TempId" runat="server" />
                     </tr>
                     </table>
               
               
              <%--  <asp:Label ID="lbl_TempId" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lbl_StudentName" runat="server" Text="" Font-Bold="true" Height="22px"></asp:Label>  <br />
                <asp:Label ID="lbl_Rank" runat="server" Text="Interview Rank" Font-Bold="true" Height="22px"></asp:Label>   
                <asp:TextBox ID="txt_StudRank" runat="server"></asp:TextBox>
                                   
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_StudRank" 
                        ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="a" ControlToValidate="txt_StudRank" ErrorMessage="Enter Rank"></asp:RequiredFieldValidator>
                 
                        <div style="text-align:center;">
                            
                            
                        </div>--%>
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



    
        <asp:Panel ID="Panel2" runat="server">
                         <asp:Button runat="server" class="btn btn-info" ID="Btn_StudCancel" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteStudent"  runat="server"   BackgroundCssClass="modalBackground"

CancelControlID="BtnDeleteCancal" 
                                  PopupControlID="Pnl_StudCancel" TargetControlID="Btn_StudCancel"  />
                          <asp:Panel ID="Pnl_StudCancel" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Confirm Delete</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                      <div style="text-align:left;">
                                        Are you sure you want to delete the student?
                                      </div>
                                      <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_DeleteStudent" class="btn btn-primary" runat="server" Text="Yes"    
                                                onclick="Btn_DeleteStudent_Click"/>     
                                            <asp:Button ID="BtnDeleteCancal" class="btn btn-danger" runat="server" Text="Cancel"  />
                                            <asp:HiddenField ID="Hdn_DeleteTempId" runat="server" />
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
         
         
<asp:Panel ID="Panel1" runat="server">
                         <asp:Button runat="server" class="btn btn-info" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_changeStatus"  runat="server" 
                         BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_ChangeStatus" TargetControlID="Button1" CancelControlID="Btn_Cancel"  />
                                
                          <asp:Panel ID="Pnl_ChangeStatus" runat="server"  
                  style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:500px;left:400px"  >                       
                           
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label4" runat="server" Text="Change Status"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
            <div style="height:90px">
               <asp:Panel ID="Pnl_Status" runat="server">
               <center>
               <table width="100%" class="tablelist">
                <tr>
                <td class="leftside">Status</td>
                <td class="rightside"><asp:DropDownList ID="Drp_ChangeStatus" class="form-control"  AutoPostBack="true" runat="server" Width="180px" OnSelectedIndexChanged="Drp_ChangeStatus_SelectedIndexChanged">
                </asp:DropDownList></td>
                </tr>  
                <tr>
                <td class="leftside">
                </td>
                <td class="rightside">
                <asp:Label ID="Lbl_ErrMsg" runat="server" ForeColor="Red"> </asp:Label>
                </td>
                </tr>
                <tr> 
                               
                <td class="leftside">
                </td>
                <td class="rightside">
                
                </td>
                </tr>  
                
                <tr> 
                               
                <td align="center" colspan="2">
                <asp:Button ID="Btn_SendMail" runat="server" class="btn btn-primary" Text="Send Mail" Width="75px" 
                        onclick="Btn_SendMail_Click" />
                <asp:Button ID="Btn_SendSms" runat="server" class="btn btn-primary" Text="Send SMS" Width="75px" Visible="false" />
               
                <asp:Button ID="Btn_UpdateStatus" runat="server" class="btn btn-primary" Text="Update" Width="70px" OnClick="Btn_UpdateStatus_Click"/>
                <asp:Button ID="Btn_Cancel" runat="server" class="btn btn-danger" Text="Cancel" Width="70px"/>
                </td>
                </tr>                           
               </table>
               <br />
                    
               </center>
               
               </asp:Panel>
               
         
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
  
  <asp:Panel ID="Panel3" runat="server">
                         <asp:Button runat="server" ID="Button2" class="btn btn-primary" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1"  runat="server" 
                         BackgroundCssClass="modalBackground"
                                  PopupControlID="Panel4" TargetControlID="Button2" CancelControlID="Btn_EmailCancel"  />
  <asp:Panel ID="Panel4" runat="server"  
                   style="display:none;" ><%--style="display:none;"--%>
                         <div class="container skin5" style="width:900px; top:500px;left:400px"  >                       
                           
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label1" runat="server" Text="Send Mail"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               <asp:Panel ID="Panel_Email_Details" runat="server">
                     <table width="95%">
                     <tr>
                     <td align="right"><asp:Button ID="Btn_SendEmail" class="btn btn-primary" runat="server" Text="Send" Width="90px" OnClick="Btn_SendEmail_Click"/>
                     <asp:Button ID="Btn_EmailCancel" runat="server" class="btn btn-primary" Text="Cancel" Width="90px" />
                     </td>
                     </tr>
                     </table>                                  
              
		              <table width="100%">
		              <tr>
		              <td style="font-size:small;font-weight:bold;color:Black"><b>Subject:</b>	
                     <asp:TextBox ID="Txt_EmailSubject" runat="server" class="form-control" Width="755px" BorderStyle="Inset" Height="20px"></asp:TextBox>
                     </td></tr>
		              </table>
                         
                <div class="roundbox" >
		              <table width="100%">
		               <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		               <tr><td class="centerleft"></td><td class="centermiddle">
		
		               <h5>Email Body</h5>
                        <div class="linestyleNew"> </div>
                         <br />
                           <HTMLEditor:Editor ID="Editor_Body" runat="server" Height="300px" 
                                            Width="100%" />
                                            
                                            
                      </td><td class="centerright"></td></tr>
		              <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		              </table>
		
		
		            </div>
		    
		    </asp:Panel> 
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
		    
		       <asp:Panel ID="Panel5" runat="server">
                         <asp:Button runat="server" class="btn btn-info" ID="Button3" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="Mpe_SendMEssage"  runat="server"   BackgroundCssClass="modalBackground"

CancelControlID="Btn_MsgCancel" 
                                  PopupControlID="Pnl_SendMessage" TargetControlID="Button3" />
                          <asp:Panel ID="Pnl_SendMessage" runat="server" style="display:none">
                         <div class="container skin5" style="width:550px; top:400px;left:400px;" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Send SMS</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <asp:Panel ID="Pnl_Message" runat="server">
                                    <table width="100%" class="tablelist" height="100px">
                                    <tr>
                                       <td class="leftside">
                            Select Template
                        </td>
                        <td class="rightside">
                           <asp:DropDownList ID="Drp_Template" runat="server" class="form-control" Width="303px" AutoPostBack="true"
                                onselectedindexchanged="Drp_Template_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                                    </tr>
                                    <tr>
                                    <td class="leftside">Message</td>
                                    <td class="rightside"><asp:TextBox ID="Txt_Message" class="form-control" runat="server" Width="300px" Height="100px" TextMode="MultiLine"></asp:TextBox> </td>
                                    </tr>
                                       <tr>
                                    <td class="leftside"></td>
                                    <td class="rightside">
                                    <asp:Label ID="Lbl_Messageerr" runat="server"  ForeColor="Red"></asp:Label> 
                                    </td>
                                    </tr>
                                    </table>
                                     </asp:Panel>
                                      <br />
                                      
                                        <div style="text-align:center;">
                                               <asp:Button ID="Btn_MessageSend" class="btn btn-primary" runat="server" Text="Send"   OnClick="Btn_MessageSend_Click" width="128px"/>
                                            <asp:Button ID="Btn_MsgCancel" runat="server" Text="Cancel"  class="btn btn-danger"  />
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

<div class="clear"></div>
     
     
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
        
     </ContentTemplate>            
     <Triggers>
               <asp:PostBackTrigger ControlID="Img_Export"/>
      </Triggers> 
                 
        </asp:UpdatePanel> 
        



</div>
</asp:Content>
