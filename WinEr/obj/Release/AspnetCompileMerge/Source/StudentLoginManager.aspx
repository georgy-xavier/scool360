<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentLoginManager.aspx.cs" Inherits="WinEr.StudentLoginManager" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_ParentList.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[6].children[0];
            cb.checked = Status;
        }
    }
    function CancelClick() {
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
                
                <div class="container skin1" style="min-height:300px">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Student Login Management</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:300px">
                <br />
                <center>
                <table width="90%">
                    <tr>
                        <td>Class</td>
                        <td>
                            <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="160px" >
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="Btn_Show" runat="server" Text="Show"  Class="btn btn-success" OnClick="Btn_Show_Click"/>
                        </td>
                        <td>
                             <asp:Button ID="Btn_GenCred" runat="server" Text="Credentials" Width="110px"  Class="btn btn-primary" Enabled="false" />
                        </td>
                        <td>
                         <asp:Button ID="Btn_SendSMS" runat="server" Text="SMS"  Class="btn btn-primary" OnClick="Btn_SendSMS_Click" Enabled="false"/>
                        </td>
                        <td>
                            <asp:Button ID="Btn_SaveChanges" runat="server" Text="Save"  Visible="false" Class="btn btn-primary" OnClick="Btn_SaveChanges_Click"/>
                        </td>
                     
                    </tr>
                </table>
                </center>
                <br />
         <%--       <asp:Panel ID="Pnl_SMS" runat="server" Visible="false">
                <div class="linestyle"></div>
                   <table class="tablelist">
                            <tr>
                                <td valign="middle" class="leftside">&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="middle" class="leftside"><span style="font-size:medium;">SMS Text</span></td>
                                <td class="leftside"
                                    style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:TextBox ID="Txt_SmsText" runat="server" Width="500px" Height="80px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                                <td rowspan="2" valign="top">
   	                       <div style="height:150px;overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" runat="server" Text="Representations of keywords"></asp:Label>
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
                            </tr>
                            <tr>
                                <td></td>
                                  <td></td>
                                <td align="center"  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                 
                                    <asp:Button ID="Btn_Send" runat="server"  Width="111px" CssClass="grayok" 
                                        Text="Send SMS" onclick="Btn_Send_Click"  />
                                </td>
                                <ajaxToolkit:ConfirmButtonExtender ID="Btn_Send_ConfirmButtonExtender" runat="server" TargetControlID="Btn_Send" ConfirmText="Are you sure you want to send the SMS" OnClientCancel="CancelClick"></ajaxToolkit:ConfirmButtonExtender>
                                
                                <td></td>
                            </tr>
                        </table> 
                </asp:Panel>--%>
                
                <asp:Panel ID="Pnl_Identifier" runat="server" Visible="false">
                  <div class="linestyle"></div>
                <table width="80%">
                <tr>
                    <td>
                        <asp:Image ID="Img_Crdentials" runat="server" ImageUrl="~/images/f93204.jpg" />-Credentials Notgenerated
                    </td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/0277fb.jpg" />-Credentials Generated
                    </td>
                     <td>
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/04fd3f.jpg" />-Credentials Sent
                    </td>
                </tr>
                </table>
                </asp:Panel>
                
                <asp:Panel ID="Pnl_ParentList" runat="server">
                
              
                    <asp:Label ID="Lbl_Message" runat="server" class="control-label"></asp:Label>
                 <div style=" overflow:auto;max-height: 375px;" id="Div_Particular">
                <asp:GridView ID="Grd_ParentList" runat="server" AutoGenerateColumns="False"
                    CellPadding="3" CellSpacing="2" ForeColor="Black" GridLines="Vertical" Width="99%" 
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                         OnSelectedIndexChanged="Grd_ParentList_SelectedIndexChanged"
                         onrowdatabound="GrdParent_RowDataBound" 
                         onrowdeleting="GrdParent_RowDelete">
                   
                    <Columns>
                         <asp:BoundField DataField="StudId" HeaderText="StudentId"  />
                         <asp:TemplateField  ItemStyle-Width="20" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate>
                                <asp:Image ID="Img_ParentStatus" runat="server"  ImageUrl="~/images/04fd3f.jpg"/>
                            </ItemTemplate>
                         </asp:TemplateField>
                         <asp:BoundField DataField="StudentName" HeaderText="StudentName" ItemStyle-Width="120"/> 
                         <asp:BoundField DataField="AdmissionNo" HeaderText="AdmissionNo"  ItemStyle-Width="120"/>
                         <asp:BoundField DataField="OfficePhNo" HeaderText="UserName" ItemStyle-Width="65"/> 
                         <asp:BoundField DataField="EmailId" HeaderText="EmailId" ItemStyle-Width="120"/> 
                         <asp:BoundField DataField="SentCredentials" HeaderText="Sent"/> 
                         <asp:BoundField DataField="CanLogin" HeaderText="CanLogin"/> 
                         <asp:BoundField DataField="ParentId" HeaderText="ParentId"/> 
                         <asp:BoundField DataField="UserId" HeaderText="UserId"/> 
                           <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="50"/> 
                           <asp:BoundField DataField="LastLogin" HeaderText="Last Login"/> 
                           
                          <asp:TemplateField  ItemStyle-Width="60" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="Chk_Login" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="Login" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                        </asp:TemplateField>
                          <asp:CommandField SelectText="&lt;img src='pics/Details.png' width='30px' border=0 title='Click to view details'&gt;" 
                                   ShowSelectButton="True" HeaderText="Details"  ItemStyle-Width="40"/>
                          <asp:CommandField DeleteText="&lt;img src='pics/SMS.png' width='30px' border=0 title='Click to get password'&gt;" 
                                   ShowDeleteButton="True" HeaderText="SMS"  ItemStyle-Width="40"/>
                    </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />                                                                                      
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />                                                                                 
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />     
                </asp:GridView>
                    <br />
            </div>   
                </asp:Panel>
                </div>
                </td>
                <td class="e"> </td>
            </tr>
            <tr>
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
                
                <WC:MSGBOX ID="WC_MessageBox" runat="server" />
                

                <asp:Panel ID="Pnl_GenCred" runat="server">
            <ajaxToolkit:ModalPopupExtender ID="MPE_GenCredentials" runat="server" CancelControlID="Btn_GenerateCredentials_Cancel"
                PopupControlID="Pnl_Cred" TargetControlID="Btn_GenCred" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Pnl_Cred" runat="server" Style="display: none;">
                  <div class="container skin5" style="width:400px">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/Pics/unlock.png" Height="28px"  Width="29px" /> </td>
                <td class="n"><span style="color:White;">Generate Credentials</span> </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
              <center>
                               <table class="tablelist">
                                <tr>
                                    <td>&nbsp;</td>
                                    <td >
                                        <asp:RadioButtonList ID="Rdo_GenCredOption" runat="server" RepeatDirection="Horizontal">
                                          <asp:ListItem Value="1" Text="All" Selected="False"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Selected Class" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                 <td>&nbsp;</td>
                                     <td></td>
                                </tr>
                                <tr>
                                      <td>&nbsp;</td>
                                    <td>
                                         <asp:Button ID="Btn_GenerateCredentials" runat="server" Text="Generate" Width="80px"  Class="btn btn-info" OnClick="Btn_GenerateCredentials_Click"/>&nbsp;&nbsp;
                                         <asp:Button ID="Btn_GenerateCredentials_Cancel" runat="server" Text="Cancel" Width="80px" Class="btn btn-info" />
                                    </td>
                                </tr>
                               </table>
                   </center>        
                    </td>
                <td class="e"> </td>
            </tr>
            <tr>
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
              
            </asp:Panel>
                </asp:Panel>
                
                
               
                
                 <asp:Panel ID="pnl_Demo" runat="server">
                <asp:Button ID="Button1" runat="server" Class="btn btn-info" Style="display: none;"/>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                PopupControlID="Panel3" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Panel3" runat="server" Style="display: none;">
                <div class="container skin5" style="width: 700px;">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/SMS.png" Height="28px"
                                    Width="29px" />
                            </td>
                            <td class="n"> <span style="color: White">SMS</span></td>
                            <td class="ne">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="o">
                            </td>
                            <td class="c">
                            
                               <asp:Panel ID="Panel1" runat="server" Visible="true">
            
                   <table width="80%">
                            <tr>
                                <td valign="middle" class="leftside">&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="middle" class="leftside"><span style="font-size:medium;">Text</span></td>
                                <td class="leftside"
                                    style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:TextBox ID="Txt_InSmStext" runat="server" Width="350px" Height="80px" class="form-control" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                                <td rowspan="2" valign="top">
   	                       <div style="height:150px; width:200px; overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label2" Font-Bold="true" Font-Underline="true" runat="server" class="control-label" Text="Keywords"></asp:Label>
   	                        <div id="Div_ind" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
                            </tr>
                            <tr>
                                    <td >
                                    
                                   </td>
                                   <td >
                                   
                                   </td>
                                   <td  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:RadioButtonList ID="Rdo_ClassList" runat="server" RepeatDirection="Horizontal">
                                         <asp:ListItem Value="1" Text="All Class" Selected="False"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Selected Class" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
                                   </td> 
                                   
                            </tr>
                            <tr>
                                    <td >
                                    
                                   </td>
                                   <td >
                                   
                                   </td>
                                   <td  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:RadioButtonList ID="Rdo_SMSSendOption" runat="server" RepeatDirection="Horizontal">
                                         <asp:ListItem Value="1" Text="All parents" Selected="False"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Only needed" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:HiddenField ID="Hdn_StudentId" Value="0" runat="server" />
                                   </td> 
                                   
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td align="center"  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                 
                                    <asp:Button ID="Btn_IndSMS" runat="server"  Width="111px" Class="btn btn-info"  Text="Send SMS" onclick="Btn_Send_Click" />&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Btn_magok" runat="server" Text="Cancel" Width="111px"  Class="btn btn-info" onclick="Btn_magok_Click"/>
                                </td>
                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Btn_IndSMS" ConfirmText="Are you sure you want to send the SMS" OnClientCancel="CancelClick"></ajaxToolkit:ConfirmButtonExtender>
                                
                                <td></td>
                            </tr>
                        </table> 
                </asp:Panel>
                                <div style="text-align: center;">
                                    <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                                   
                                </div>
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
                    <br />
                    <br />
                </div>
            </asp:Panel>
                </asp:Panel>
                
                
                
                <asp:Panel ID="Panel2" runat="server">
                    <asp:Button ID="Button2" runat="server" Text="Button" Class="btn btn-info" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Details" runat="server" CancelControlID="Btn_SaveDetails_Cancel"
                PopupControlID="Panel4" TargetControlID="Button2" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Panel4" runat="server"  Style="display: none;" >
                <div class="container skin5" style="width:600px;" >
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                             <tr>
                            <td class="no"> <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/Details.png" Height="28px"  Width="29px" />  </td>
                            <td class="n"> <span style="color: White">Details</span></td>
                            <td class="ne">&nbsp;</td>
                        </tr>
                        <tr>
                           <td class="o"></td>
                            <td class="c">
                            <br/>
                            <center>
                            <table class="tablelist">
                                
                                 <tr>
                                    <td class="leftside">Parent</td>
                                    <td class="rightside">
                                     
                                    
                                        <asp:TextBox ID="Txt_ParentName" runat="server" class="form-control" BorderStyle="None" ReadOnly="true" Width="200"></asp:TextBox>
                                    </td>
                                 </tr>
                                 <tr >
                                    <td class="leftside">Student</td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_Student" runat="server" BorderStyle="None" class="form-control" ReadOnly="true" Width="200"></asp:TextBox>
                                    </td>
                                 </tr>
                                   <tr >
                                    <td class="leftside">Last Login</td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_logindate" runat="server" BorderStyle="None" class="form-control" ReadOnly="true" Width="200"></asp:TextBox>
                                    </td>
                                 </tr>
                                 
                                   <tr>
                                    <td class="leftside">UserId/MobNo</td>
                                    <td class="rightside"> 
                                    <asp:TextBox ID="Txt_USerId" runat="server" class="form-control" ></asp:TextBox>
                                    
                                 
                                    
                                    </td>
                                   
                                 </tr>
                                 
                                  <tr>
                                    <td class="leftside">Gmail Id</td>
                                    <td class="rightside"> 
                                    <asp:TextBox ID="Txt_EmailId" runat="server"  Enabled="true" class="form-control"></asp:TextBox>
                                          
                        
                                    </td>
                                      <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                     runat="server" Enabled="True"  FilterType="Custom,Numbers" 
                                     ValidChars="@._-"   TargetControlID="Txt_USerId">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 </tr>
                                 
                                 
                                 <tr>
                                    <td class="leftside">
                                        
                                    </td>
                                    <td class="rightside">
                                        <asp:CheckBox ID="Chk_EdtCanLogin" Text="CanLogin" runat="server" />
                                    </td>
                                 </tr>
                               </table>
                            </center>
                            <table class="tablelist">
                                <tr>
                                    <td class="leftside"> <asp:LinkButton ID="Lnk_GenPass" runat="server" OnClick="Lnk_GenPass_Click" ValidationGroup="Saveme">Show Password</asp:LinkButton></td>
                                    <td class="rightside">   <asp:Label ID="Lbl_Password" class="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="leftside"></td>
                                     <td class="rightside">
                                         <asp:Label ID="Lbl_DetailsMessage" class="control-label" runat="server"></asp:Label>
                                     </td>
                                </tr>
                                 <tr>
                                     <td class="leftside">
                                         <asp:HiddenField ID="Hdn_PassStudentId" Value="0" runat="server" />
                                         <asp:HiddenField ID="Hdn_ParentId" Value="0" runat="server" />
                                     </td>
                                      <td class="rightside">
                                           <asp:Button ID="Btn_SaveDetails" runat="server" Text="Save" ValidationGroup="Saveme" OnClick="Btn_SaveDetails_Click" Width="80px" class="btn btn-info" />
                                           <asp:Button ID="Btn_RemoveCand" runat="server" class="btn btn-danger" Text="Remove"   
                                                onclick="Btn_RemoveCand_Click" />
                                           <asp:Button ID="Btn_SaveDetails_Cancel" runat="server" Text="Cancel"  Width="80px" class="btn btn-warning" />
                                     </td>
                                 </tr>
                                 </table>
                            <br/>
                            </td>
                          <td class="e">  </td>
                         
                        </tr>
                        <tr>
                            <td class="so"></td>
                            
                            <td class="s"></td>
                        
                            <td class="se"></td>
                          
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </asp:Panel>
                </asp:Panel>
                
                
            </ContentTemplate>
                </asp:UpdatePanel>
   </div>
</asp:Content>
