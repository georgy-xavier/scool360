<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ViewStaffIncidenceByType.aspx.cs" Inherits="WinEr.WebForm27" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
   <div id="right">


<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>
 
</div>
   
   <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
</asp:UpdateProgress>--%>
  
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
  
     <div id="left">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
             <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
              <br />
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="images/indnt_srch.png" width="35" height="35" />  </td>
                <td class="n">View Staff Incidents</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                   <asp:Panel ID="IncidentData"  runat="server">
                   
                   <table width="100%">
                    <tr>
                        <td align="left" style="width:170px"><asp:DropDownList ID="Drp_IncType" runat="server" class="form-control" Width="160" 
                                AutoPostBack="True" onselectedindexchanged="Drp_IncType_SelectedIndexChanged">
                            </asp:DropDownList></td>
                            
                             <td align="right" >
                                 <asp:RadioButtonList ID="Rdb_Batch" runat="server" RepeatDirection="Horizontal" 
                                     onselectedindexchanged="Rdb_Batch_SelectedIndexChanged" AutoPostBack="true">
                                 <asp:ListItem Value="2" >ALL</asp:ListItem>
                                 <asp:ListItem Value="0" Selected="True">Current Batch</asp:ListItem>
                                 <asp:ListItem Value="1">Previous Batch</asp:ListItem>
                                 </asp:RadioButtonList></td>
                                <td align="left"> <asp:DropDownList ID="Drp_PreviousBatch" class="form-control" runat="server" AutoPostBack="true" 
                                        Width="100px" onselectedindexchanged="Drp_PreviousBatch_SelectedIndexChanged">
                                 </asp:DropDownList></td>
                             
                        <td align="right" style="width:80px"> <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-danger"
                                        onclick="Btn_Delete_Click" />
                            <asp:TextBox ID="Text_Hidden" runat="server" Visible="false"></asp:TextBox>    </td>
                    </tr>
                   
                   </table>
                        
                        <br />
                        
                   </asp:Panel> 
                <div class="linestyle"></div>
             
             
            <table width="100%">
                  <tr>
                        <td align="center"><asp:Label ID="lbl_viewIncidentMsg" runat="server" ForeColor="Red" Text=""></asp:Label></td>
                    </tr>
                    
                      <tr>
                        <td><asp:Panel ID ="Pnl_incidentGrid" runat="server">
                            <table width="100%">
                            <tr>
                                <td> <asp:LinkButton ID="Lnk_Select" runat="server" onclick="Lnk_Select_Click">Select All</asp:LinkButton></td>
                                <td  align="right" style="padding-right:20px">
                             
                                <asp:Label ID="lbl_Points" runat="server"  Text="Total Points :"></asp:Label>
                                <asp:Image ID="Img_Points" runat="server" Height="15px" Width="15px" ImageAlign="AbsMiddle" />
                                <asp:Label ID="lbl_TotalPoints" runat="server" Font-Bold="true" Text="0"></asp:Label>
                               </td>                                                
                            </tr>
                        </table>
                            <asp:GridView ID="Grd_Incident" runat="server" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" AutoGenerateColumns="False" AllowPaging="True"  
                    Width="100%" AllowSorting="true" 
                    onselectedindexchanged="Grd_Incident_SelectedIndexChanged" 
                    onpageindexchanging="Grd_Incident_PageIndexChanging" BackColor="White" 
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" PageSize="15" 
                                onsorting="Grd_Incident_Sorting">
                    <Columns>
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:CheckBox id ="Chk_Incident" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="Title" HeaderText=" Title" SortExpression="Title" ItemStyle-Width="200px"/>                     
                        <asp:BoundField DataField="Type" HeaderText=" Incident Type" SortExpression="Type" ItemStyle-Width="60px"/>
                        <asp:BoundField DataField="Point" HeaderText="Point" SortExpression="Point" ItemStyle-Width="30px"/>
                        <asp:BoundField DataField="IncidentDate" HeaderText="Incident Date" SortExpression="IncidentDate" ItemStyle-Width="30px"/>
                            
                            <asp:TemplateField HeaderText ="Point" ItemStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Image ID="Img_Point" runat="server" Height="15px" Width="15px" ImageAlign="Middle" />
                            <asp:Label ID="lbl_Point" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>
                              
                       <%--<asp:TemplateField HeaderText ="Created for" ItemStyle-Width="90px">
                        <ItemTemplate>
                           <asp:Label ID="Lbl_PupilName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText ="Type" ItemStyle-Width="50px">
                        <ItemTemplate>
                           <asp:Label ID="Lbl_PupilType" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>--%>
                       
                        <asp:CommandField  ItemStyle-Width="25px" ShowSelectButton="True" SelectText="&lt;img src='pics/hand.png' width='30px' border=0 title='Select incident to be Approved Rejected'&gt;"/>
                       
                    </Columns>
                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                    
                </asp:GridView>
                        </asp:Panel></td>
                    </tr>
            </table>            
             
                   
                             
                  
                  <asp:Button runat="server" ID="Btn_Message" style="display:none"/>
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Confirm"   runat="server" CancelControlID="Btn_Cnfirm_Cancel" PopupControlID="Pnl_Confirm" TargetControlID="Btn_Message"  />
                                <asp:Panel ID="Pnl_Confirm" runat="server" style="display:none;">
                                    <div class="container skin5" style="width:400px; top:400px;left:200px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:White">alert!</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >             
                                                     <asp:Label ID="Lbl_Confirm" runat="server" Text=""></asp:Label>
                                                     <br /><br />
                                                      <div style="text-align:center;">    
                                                            <asp:Button ID="Btn_ConfirmDelete" runat="server" Text="Yes" class="btn btn-success" OnClick="Btn_Confirm_Click" />        
                                                            <asp:Button ID="Btn_Cnfirm_Cancel" runat="server" Text="No" class="btn btn-danger"/>
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
                  
                  <asp:Button runat="server" ID="Btn_PopUp" style="display:none"/>
	                        <ajaxToolkit:ModalPopupExtender ID="MPE_IncidentPopUp"   runat="server" CancelControlID="Btn_IncP_Cancel" BackgroundCssClass="modalBackground" PopupControlID="Pnl_IncidentPopUp" TargetControlID="Btn_PopUp"  />
	                            <asp:Panel ID="Pnl_IncidentPopUp" runat="server" style="display:none">
                                    <div class="container skin5" style="width:700px; top:400px;left:200px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image1" runat="server" 
                                                         ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:White">View Incident</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >             
                                                     <asp:Label ID="Lbl_IncidentPopUup" runat="server" Text=""></asp:Label>
                                                     <br />
                                                      <div >
                                                        <table width="100%">
                                                            
                                                             <tr>
                                                                <td>IncidentType</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_Type" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created User</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedUser" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Incident Date</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_IncidentDate" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created Date</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedDate" runat="server" Width="180px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Created for</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_ReportedTo" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>
                                                                    <asp:Label ID="Lbl_Class" runat="server" Text="Class"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_Class" runat="server" Width="180px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                             <tr>
                                                                 <td>
                                                                      Type</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserType" runat="server" ReadOnly="True" Width="180px" class="form-control"></asp:TextBox>
                                                                 </td>
                                                                 <td>
                                                                     &nbsp;</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserId" runat="server" Visible="False" Wrap="False" class="form-control"></asp:TextBox>
                                                                     <asp:TextBox ID="Txt_IncidentId" runat="server" Visible ="false" class="form-control"></asp:TextBox>
                                                                 </td>
                                                             </tr>
                                                            <tr>
                                                                <td>Title</td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="Txt_Title" runat="server" ReadOnly="True" Width="500px" class="form-control"></asp:TextBox></td>
                                                            </tr>
                                                           
                                                            <tr>
                                                            <td>Description</td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="Txt_Desc" runat="server" Height="50px" ReadOnly="True" class="form-control"
                                                                    TextMode="MultiLine" Width="505px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan ="4" align="center">
                                                                <asp:Button ID="Btn_DeletePopUp" runat="server" Text="Delete" class="btn btn-danger" OnClick="Btn_DeletePopup_Click"/>
                                                                 <asp:Button ID="Btn_IncP_Cancel" runat="server" Text="OK" class="btn btn-primary"/>
                                                                </td>
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
                                         <br /><br />                                                
                                    </div>
                             </asp:Panel>
                    
                              
                <br />
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
    
    </div>
    
     <WC:MSGBOX id="WC_MessageBox" runat="server" />  
     
    </ContentTemplate>
 
</asp:UpdatePanel>
    <div class="clear"></div>
    </div>
</asp:Content>
