<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="GroupUserMap.aspx.cs" Inherits="WinEr.GroupUserMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <script type="text/javascript" >

        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_StaffList.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
        function SelectAllEdit(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grd_EditStaffGroup.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }

        function SelectAllSTudent(cbstudentselect) {
            var gridViewCtl = document.getElementById('<%=grd_studList.ClientID%>');
            var Status = cbstudentselect.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }

        function SelectAllSTudentEdit(cbstudentselectedit) {
            var gridViewCtl = document.getElementById('<%=Grd_studentEditMap.ClientID%>');
            var Status = cbstudentselectedit.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

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
				<td class="n">Group User Map</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				            <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  
         CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="0" >
               
               
                  <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Manual"  Visible="true" >
    <HeaderTemplate>
    <img alt="" src="Pics/Staff/network_search.png" height="25" width="25" />
    Staff</HeaderTemplate>     
     <ContentTemplate>     
     
     <asp:Panel ID="Pnl_StaffGruopMap" runat="server">
     <table width="100%" >    
     <tr>
     <td align="center" >
    <table width="100%" class="tablelist">
    <tr>     
           <td  colspan="2" align="right"> 
    <asp:Image ID="Img_AddUser" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
    <asp:LinkButton ID="Lnk_AddNewUer" runat="server" CssClass="grayadd" 
    Height="22px" onclick="Lnk_AddNewUer_Click" >Add User</asp:LinkButton>
    <asp:Image ID="Img_EditUser"  ImageUrl="~/Pics/edit.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
    <asp:LinkButton ID="Lnk_EditUser" runat="server" CssClass="grayadd" 
    Height="22px" onclick="Lnk_EditUser_Click" >Edit User</asp:LinkButton>    
    </td>
    </tr>
        <tr  runat="server" id="rowEdit">
    <td  class="leftside" runat="server">
    <asp:Label ID="Lbl_StaffFromGroup" runat="server" Text="Group From:"></asp:Label>     
    </td>
    <td class="rightside" runat="server">
    <asp:DropDownList ID="Drp_StaffGroupFrom" runat="server" Width="203px"
    OnSelectedIndexChanged="Drp_StaffGroupFrom_SelectedIndexChanged" 
            AutoPostBack="True">  
    </asp:DropDownList>  
    </td>
    </tr>
    <tr  id="rowadd" runat="server">
    <td class="leftside" runat="server">     
    <asp:Label ID="Lbl_StaffToGroup" runat="server"></asp:Label>     
    </td>
    <td class="rightside" runat="server">
    <asp:DropDownList ID="Drp_StaffGroup" runat="server" Width="203px"></asp:DropDownList>
    </td>
    </tr>

    <tr>
    <td class="leftside"></td>
    <td class="rightside">
    <asp:Button ID="Btn_StaffGroupMap" runat="server" Text="Add" Width="90px" 
    Class="btn btn-info" OnClick="Btn_StaffGroupMap_Click"/>
    <asp:Button ID="Btn_StaffGroupUpdate" runat="server" Text="Update" Width="90px" 
    Class="btn btn-info" OnClick="Btn_StaffGroupUpdate_Click"/>
     <asp:Button ID="Btn_DeleteStaff" runat="server" Text="Delete" Width="90px" 
    Class="btn btn-info" OnClick="Btn_DeleteStaff_Click"/>
    </td>
    </tr>
    </table>

   
     
     </td>
     </tr>
      <tr>     
     <td align="center">
     <asp:Label ID="Lbl_ErrStaffMap" runat="server" ForeColor="Red"></asp:Label>
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
          <asp:TemplateField HeaderText="Select" >
                             
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
     
        <tr>     
     <td>
     <br />    
     <asp:GridView ID="Grd_EditStaffGroup" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"  
                    AllowPaging="True" PageSize="15"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      OnPageIndexChanging="Grd_EditStaffGroup_PageIndexChanging"
                      Width="100%" >
        
        <Columns>         
          <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAllEdit(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
            <asp:BoundField DataField="SurName" HeaderText="SurName" />
            <asp:BoundField DataField="UserName" HeaderText="User Name" />
            <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
            
                <asp:BoundField DataField="Id" HeaderText="Id" />
                 <asp:BoundField DataField="GroupId" HeaderText="Group Id" />
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
     
     </ContentTemplate>      
     </ajaxToolkit:TabPanel>
     
      <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Manual"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/images/stdnt.png"/>
    Student</HeaderTemplate>  
     <ContentTemplate> 
     <asp:Panel ID="Pnl_StudentGroupMap" runat="server">
      <table width="100%">    
     <tr>
     <td align="center">
    <table width="100%" class="tablelist">
    <tr>
    <td align="right" colspan="2">
    <asp:Image ID="Img_StudentMapAdd" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
    <asp:LinkButton ID="Lnk_StudentMapAdd" runat="server" CssClass="grayadd" 
    OnClick="Lnk_StudentMapAdd_Click"
    Height="22px"  >Add User</asp:LinkButton>
    <asp:Image ID="Img_StudentMapEdit"  ImageUrl="~/Pics/edit.png" Width="25px" Height="20px" runat="server" style="vertical-align:top" />
    <asp:LinkButton ID="Lnk_StudentMapEdit" runat="server" CssClass="grayadd" 
    OnClick="Lnk_StudentMapEdit_Click"
    Height="22px" >Edit User</asp:LinkButton>
    </td>
    </tr>
    <tr id="rowclass" runat="server">
    <td class="leftside">Class:</td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudentClass" runat="server" Width="203px"
    OnSelectedIndexChanged="Drp_StudentClass_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>  </td>
    </tr>
    <tr id="rowgroupfrom" runat="server">
  
    <td class="leftside">Group From</td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudGroupFrom" AutoPostBack="true" OnSelectedIndexChanged="Drp_StudGroupFrom_SelectedIndexChanged"
    runat="server" Width="203px"></asp:DropDownList>  </td>
    </tr>
    <tr id="rowgroupto" runat="server">    
    <td class="leftside"><asp:Label ID="LblStudGroup" runat="server" Text=""></asp:Label></td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudentGroup" runat="server" Width="203px"></asp:DropDownList>  </td>
    </tr>
    <tr>   
    <td class="leftside"></td>
    <td class="rightside">
    <asp:Label ID="Lbl_StudErr" runat="server" ForeColor="Red" Text="" Font-Bold="false"> </asp:Label>
    </td>
    </tr>
    <tr>  
    <td class="leftside"></td>
    <td class="rightside"> <asp:Button ID="Btn_StudentGroupMap" runat="server" Text="Add" 
    OnClick="Btn_StudentGroupMap_Click" Class="btn btn-info" Width="90px"/>
    <asp:Button ID="Btn_StudGroupUpdate" runat="server" Text="Update" 
    OnClick="Btn_StudGroupUpdate_Click" Class="btn btn-info" Width="90px"/>
     <asp:Button ID="Btn_deletestudent" runat="server" Text="Delete" 
    OnClick="Btn_deletestudent_Click" Class="btn btn-info" Width="90px"/>
     </td>
    </tr>
    </table>
    
     </td>
     </tr>
     
     <tr>
				<td>
				 <asp:GridView ID="grd_studList" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"                         
                    AllowPaging="true" PageSize="15"
                    OnPageIndexChanging="grd_studList_PageIndexChanging"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      Width="100%" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                             
                              <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAllSTudent(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                              <asp:BoundField DataField="RollNo" HeaderText="Roll No" />
                              <asp:BoundField DataField="StudentName" HeaderText="Name"  />
                               <asp:BoundField DataField="Sex" HeaderText="Sex"  />
                               <asp:BoundField DataField="Address" HeaderText="Address"  />
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
              </asp:GridView>
                </td>
				</tr>
     
      
     <tr>
				<td>
				 <asp:GridView ID="Grd_studentEditMap" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"                         
                    AllowPaging="true" PageSize="15"
                    OnPageIndexChanging="grd_studList_PageIndexChanging"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      Width="100%" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                             
                              <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAllSTudentEdit(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                              <asp:BoundField DataField="RollNo" HeaderText="Roll No" />
                              <asp:BoundField DataField="StudentName" HeaderText="Name"  />
                               <asp:BoundField DataField="Sex" HeaderText="Sex"  />
                                <asp:BoundField DataField="GroupName" HeaderText="Group Name"  />
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                 <asp:BoundField DataField="GroupId" HeaderText="Group Id"  />
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
              </asp:GridView>
                </td>
				</tr>
				
     </table>
     </asp:Panel>
     
     </ContentTemplate>  
   </ajaxToolkit:TabPanel>	     
    </ajaxToolkit:tabcontainer>		
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
