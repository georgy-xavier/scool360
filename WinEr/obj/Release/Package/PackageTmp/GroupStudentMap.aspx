<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GroupStudentMap.aspx.cs" Inherits="WinEr.GroupStudentMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <script type="text/javascript" >
          function SelectAllSTudent(cbstudentselect)
          {
            var gridViewCtl = document.getElementById('<%=grd_studList.ClientID%>');
            var Status = cbstudentselect.checked;
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
		<td class="n">Group Student Map</td>
		<td class="ne"> </td>
	</tr>
	<tr >
		<td class="o"> </td>
		<td class="c" >
				
           <asp:Panel ID="Pnl_StudentGroupMap" runat="server">
      <table width="100%">    
     <tr>
     <td align="center">
    <table width="100%" class="tablelist">
    <tr id="rowclass" runat="server">
    <td class="leftside">Class:</td>
    <td class="rightside"> 
    <asp:DropDownList ID="Drp_StudentClass" runat="server" Width="203px" class="form-control"
         OnSelectedIndexChanged="Drp_StudentClass_SelectedIndexChanged" AutoPostBack="true" >
    </asp:DropDownList> 
    </td>
    </tr>
    <tr id="rowgroupfrom" runat="server">
  
    <td class="leftside">Group:</td>
    <td class="rightside"> 
    <asp:DropDownList ID="Drp_StudentGroup" runat="server" class="form-control" Width="203px"></asp:DropDownList>  </td>
    </tr>
    <tr>   
    <td class="leftside"></td>
    <td class="rightside">
    <asp:Label ID="Lbl_StudErr" runat="server" ForeColor="Red" Text="" class="control-label" Font-Bold="false"> </asp:Label>
    </td>
    </tr>
    <tr>  
    <td class="leftside"></td>
    <td class="rightside"> <asp:Button ID="Btn_StudentGroupMap" runat="server" Text="Add" 
        OnClick="Btn_StudentGroupMap_Click" Class="btn btn-info" Width="90px"/>
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
                                 <asp:BoundField DataField="Groups" HeaderText="Present Group" />
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

