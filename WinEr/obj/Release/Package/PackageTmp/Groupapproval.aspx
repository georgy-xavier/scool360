<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Groupapproval.aspx.cs" Inherits="WinEr.Groupapproval" %>
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
				<td class="n">Approve Group</td>
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
                    Width="100%" PageSize="30"
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
                        <asp:BoundField DataField="id" HeaderText="Id" />
                        <asp:BoundField DataField="Name" HeaderText=" Student Name" ItemStyle-Width="200px" SortExpression="Title"/>  
                        <asp:BoundField DataField="Stud_Id" HeaderText=" StudentId" ItemStyle-Width="350px" SortExpression="Description"/>                   
                        <asp:BoundField DataField="GroupName" HeaderText=" Group Name" ItemStyle-Width="65px" SortExpression="Type"/>
                        <asp:BoundField DataField="GroupId" HeaderText=" Group Id" ItemStyle-Width="65px" SortExpression="Type"/>
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
       
       
	    <WC:MSGBOX id="WC_MessageBox" runat="server" />  
	    
        <div class="clear"></div>
    </div>
    
    </ContentTemplate>
    
    </asp:UpdatePanel>
</asp:Content>
