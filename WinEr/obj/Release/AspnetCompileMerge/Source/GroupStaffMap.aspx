<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GroupStaffMap.aspx.cs" Inherits="WinEr.GroupStaffMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <script type="text/javascript" >

        function SelectAll(cbSelectAll) 
        {
            var gridViewCtl = document.getElementById('<%=Grd_StaffList.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++)
            {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }

    </script>
    
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
<div id="contents">
<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Group Staff Map</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >   
     
     <asp:Panel ID="Pnl_StaffGruopMap" runat="server">
     <table width="100%" >    
     <tr>
     <td align="center" >
    <table width="100%" class="tablelist">
    <tr  id="rowadd" runat="server">
    <td id="Td3" class="leftside" runat="server">
        Group:         
    </td>
    <td id="Td4" class="rightside" runat="server">
    <asp:DropDownList ID="Drp_StaffGroup" runat="server" Width="203px" class="form-control"></asp:DropDownList>
    </td>
    </tr>

    <tr>
    <td class="leftside"></td>
    <td class="rightside">
    <asp:Button ID="Btn_StaffGroupMap" runat="server" Text="Add" Width="90px" 
    Class="btn btn-info" OnClick="Btn_StaffGroupMap_Click"/>
    </td>
    </tr>
    </table>

   
     
     </td>
     </tr>
      <tr>     
     <td align="center">
     <asp:Label ID="Lbl_ErrStaffMap" runat="server" class="control-label" ForeColor="Red"></asp:Label>
     </td>
     </tr>
     <tr>     
     <td>
     <br />    
     <asp:GridView ID="Grd_StaffList" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"  
                    AllowPaging="True" PageSize="15"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      OnPageIndexChanging="Grd_StaffList_PageIndexChanging"
                      Width="100%" >
        
        <Columns>         
          <asp:TemplateField HeaderText="Select" HeaderStyle-Width="15px" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
            <asp:BoundField DataField="SurName" HeaderText="SurName" />
            <asp:BoundField DataField="UserName" HeaderText="User Name" />
            <asp:BoundField DataField="RoleName" HeaderText="RoleName" />
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Groups" HeaderText="Presnt Group" />   
         </Columns> 
         
         
         <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" 
             Font-Size="11px" ForeColor="Black"
                                                                                
             HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
            </asp:GridView>
     </td>
     </tr>
     
     </table>
     </asp:Panel>
     	
				 <WC:MSGBOX id="WC_MessageBox" runat="server" />   
            
				
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

<div class="clear"></div>
</div>
 </ContentTemplate>
<%-- <Triggers>
 <asp:PostBackTrigger ControlID="Btn_Excel" />
 </Triggers>--%>
                    </asp:UpdatePanel>
</asp:Content>

