<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ApproveIncedents.aspx.cs" Inherits="WinEr.WebForm22"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" >

        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_Incident.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />    
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
   

    <div id="contents">
        
               
        <div class="container skin1" style="min-height:400px;">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><img alt="" src="images/accept.png" width="35" height="35" /> </td>
				<td class="n">Approve Incident</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			         <asp:Panel ID="Pnl_mainarea" runat="server" style="min-height:300px">
                        
                        <br />
                        <table width="100%">
                            <tr>
                                <td> <%--<asp:LinkButton ID="Lnk_Select" runat="server" onclick="Lnk_Select_Click">Select All</asp:LinkButton>--%></td>
                                <td  align="right"><asp:Button ID="Btn_Approve" runat="server" Text="Approve"  Class="btn btn-success" onclick="Btn_Approve_Click"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn_Reject" runat="server" Text="Reject" class="btn btn-danger"
                                        onclick="Btn_Reject_Click" />
                                </td>                                
                            </tr>
                            <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lbl_ApproveMessage" runat="server" Text=""></asp:Label></td>
                            </tr>
                        </table>
                       <div class="linestyle"></div>
                        <asp:GridView ID="Grd_Incident" runat="server" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  
                    Width="100%"   onselectedindexchanged="Grd_Incident_SelectedIndexChanged"  PageSize="30"
                    onpageindexchanging="Grd_Incident_PageIndexChanging" BackColor="White"  
                             BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                             onsorting="Grd_Incident_Sorting">
                  <%-- OnRowDataBound = "Grd_IncidentDataBound"--%>
                    <Columns>
                  
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:CheckBox id ="Chk_Incident" runat="server" />
                        </ItemTemplate>
                          <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="IncidentId" HeaderText="Id" />
                        <asp:BoundField DataField="Title" HeaderText=" Title" ItemStyle-Width="200px" SortExpression="Title"/>  
                        <asp:BoundField DataField="Description" HeaderText=" Description" ItemStyle-Width="350px" SortExpression="Description"/>                   
                        <asp:BoundField DataField="Type" HeaderText=" Type" ItemStyle-Width="65px" SortExpression="Type"/>
                        <asp:BoundField DataField="Point" HeaderText="Point" ItemStyle-Width="30px" SortExpression="Point"/>
                        <asp:TemplateField HeaderText ="Created User" ItemStyle-Width="70px">
                        <ItemTemplate>
                           <asp:Label ID="Lbl_CreatedUser" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>
                       <asp:TemplateField HeaderText ="Created for" ItemStyle-Width="100px">
                        <ItemTemplate>
                           <asp:Label ID="Lbl_PupilName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText ="Type" ItemStyle-Width="35px">
                        <ItemTemplate>
                           <asp:Label ID="Lbl_PupilType" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                      </asp:TemplateField>
                       
                     <asp:CommandField ItemStyle-Width="30px" ShowSelectButton="True"  HeaderText="Details" SelectText="&lt;img src='pics/hand.png' width='30px' border=0 title='Select incident to be Approved Rejected'&gt;"/>
                       
                    </Columns>
                     <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                   
                </asp:GridView>
                        <br />
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
       
       
	                     <asp:Button runat="server" ID="Btn_PopUp" style="display:none"/>
	                        <ajaxToolkit:ModalPopupExtender ID="MPE_IncidentPopUp"   runat="server" CancelControlID="Btn_IncP_Cancel" 
	                        PopupControlID="Pnl_IncidentPopUp" TargetControlID="Btn_PopUp" BackgroundCssClass="modalBackground" />
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
                                                                    <asp:TextBox ID="Txt_Type" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created User</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedUser" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Incident Date</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_IncidentDate" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created Date</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedDate" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Created for</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_ReportedTo" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                                <td>
                                                                    <asp:Label ID="Lbl_Class" runat="server" Text="Class"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_Class" runat="server" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                             <tr>
                                                                 <td>
                                                                      Type</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserType" runat="server" ReadOnly="True" Width="160px"></asp:TextBox>
                                                                 </td>
                                                                 <td>
                                                                     &nbsp;</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserId" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                                                                     <asp:TextBox ID="Txt_IncidentId" runat="server" Visible ="false"></asp:TextBox>
                                                                 </td>
                                                             </tr>
                                                            <tr>
                                                                <td>Title</td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="Txt_Title" runat="server" ReadOnly="True" Width="505px"></asp:TextBox></td>
                                                            </tr>
                                                           
                                                            <tr>
                                                            <td>Description</td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="Txt_Desc" runat="server" Height="50px" ReadOnly="True" 
                                                                    TextMode="MultiLine" Width="505px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td colspan ="3" align="center">
                                                                <asp:Button ID="Btn_PopUpApprove" runat="server" Text="Approve" 
                                                                        onclick="Btn_PopUpApprove_Click" Class="btn btn-info" />&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="Btn_popUpCancel" runat="server" Text="Reject" 
                                                                        onclick="Btn_popUpCancel_Click" Class="btn btn-info"/>&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="Btn_IncP_Cancel" runat="server" Text="Cancel" Class="btn btn-info" />
                                                                
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
	   
	    <WC:MSGBOX id="WC_MessageBox" runat="server" />  
	    
        <div class="clear"></div>
    </div>
    
    </ContentTemplate>
    
    </asp:UpdatePanel>
</asp:Content>
