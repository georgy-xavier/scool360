<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailLog.aspx.cs" Inherits="WinEr.EmailLog" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
         
    <div id="contents">
         

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate>
  
        <div class="container skin1"  >
         <table  cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no"></td>
                <td class="n">Email Log</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <asp:Panel ID="Pnl_InitialData" runat="server">
                <center>
                <table width="500px" class="tablelist">
                <tr>
                <td class="leftside">Select Type</td>
                <td class="rightside"><asp:RadioButtonList ID="Rdb_Type" runat="server" 
                        RepeatDirection="Horizontal" AutoPostBack="True" 
                        onselectedindexchanged="Rdb_Type_SelectedIndexChanged">
                <asp:ListItem Text="Staff" Value="0"></asp:ListItem>
                <asp:ListItem Text="Parent" Value="1"></asp:ListItem>
                </asp:RadioButtonList></td>
                </tr>
                 <tr id="RowStaff" runat="server">
                <td class="leftside">Staff</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_Staff" runat="server" class="form-control" Width="170px" 
                        onselectedindexchanged="Drp_Staff_SelectedIndexChanged" 
                        AutoPostBack="True"></asp:DropDownList>
                </td>
                </tr>
                 <tr  id="RowParent" runat="server">
                <td class="leftside">Parent</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_Parent" runat="server" class="form-control" Width="170px" 
                        onselectedindexchanged="Drp_Parent_SelectedIndexChanged" 
                        AutoPostBack="True"></asp:DropDownList>
                </td>
                </tr>
                 <tr >
                <td class="leftside"></td>
                <td class="rightside"><asp:Label ID="Lbl_Msg" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                </td> 
                </tr>                 
                </table>
                </center>
                </asp:Panel>
                
                <asp:Panel ID="Pnl_LogDisplay" runat="server">
                   <table width="100%" cellspacing="5">
		            <tr>
		             <td>
                         <asp:LinkButton ID="Link_SelectAll" runat="server" onclick="Link_SelectAll_Click" 
                            >Select All</asp:LinkButton>
		             </td>
		            </tr>
		            <tr>
		             <td><%--AllowPaging="true" PageSize="20"   <FooterStyle BackColor="#CCCC99" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                            <RowStyle BackColor="Transparent" />  <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />--%>
                             <div id="Div_Staff" style="overflow:auto;max-height:500px">
		                   <asp:GridView ID="Grd_EmailLog" runat="server" 
                            AutoGenerateColumns="false" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                            GridLines="Vertical" 
                             onselectedindexchanged="Grd_EmailLog_SelectedIndexChanged" >                         
                         
                            <Columns>
                                 <asp:TemplateField HeaderText="Select"  ItemStyle-Width="20px">
                                <ItemTemplate   >
                                    <asp:CheckBox ID="Checksms" runat="server"  />
                                </ItemTemplate>
                         <ControlStyle ForeColor="#1AA4FF" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                         </asp:TemplateField>
                                <asp:BoundField DataField="Id" HeaderText="Id" />   
                                <asp:BoundField DataField="EmailAddress" HeaderText="Email Id"  />  
                                <asp:BoundField DataField="EmailSubject" HeaderText="Subject" />                       
                                <asp:BoundField DataField="TimeTosend" HeaderText="Date"  /> 
                                <asp:BoundField DataField="SendStatus" HeaderText="Status"  />            
                                <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true" HeaderText="View" 
                                    ItemStyle-Font-Size="Smaller" 
                                    SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select To View'&gt;" 
                                    ShowSelectButton="True">
                                    <ControlStyle />
                                    <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                </asp:CommandField>
                            </Columns>
                          
                        </asp:GridView>
                        </div>
		                 </td>
		            </tr>   
		           
		          </table>
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
     
      
      <div class="clear"></div>
     
     
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
         <asp:Panel ID="Panel3" runat="server">
                         <asp:Button runat="server" ID="Button2" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MpE_ShowEmailBody"  runat="server" 
                         BackgroundCssClass="modalBackground"
                                  PopupControlID="Panel4" TargetControlID="Button2" CancelControlID="Btn_EmailCancel"  />
  <asp:Panel ID="Panel4" runat="server" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:300px;left:400px"  >                       
                     
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label1" runat="server" class="control-label" Text="Email Body"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               <asp:Panel ID="Panel_Email_Details" runat="server">
            <div style=" overflow:auto;min-height:150px">
                   <div id="EmailBody" runat="server">
                   
                   </div>
                  
		    </div> 
		    <table width="100%" >
		    <tr>
		    <td align="center">
		    <asp:Button ID="Btn_EmailCancel"  runat="server" Text="Cancel" Class="btn btn-info"/>
		    </td>
		    </tr>
		    </table>		    
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
     </ContentTemplate>      
     <%--<Triggers>
               <asp:PostBackTrigger ControlID="Img_Export"/>
      </Triggers>--%> 
                 
        </asp:UpdatePanel> 
</div>
</asp:Content>
