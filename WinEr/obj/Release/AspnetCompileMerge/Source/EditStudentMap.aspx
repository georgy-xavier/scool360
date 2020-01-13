<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EditStudentMap.aspx.cs" Inherits="WinEr.EditStudentMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript" >
        function SelectAllSTudentEdit(cbstudentselectedit) 
        {
            var gridViewCtl = document.getElementById('<%=Grd_studentEditMap.ClientID%>');
            var Status = cbstudentselectedit.checked;
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
				<td class="n">Edit Student Group</td>
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
    <tr>
    <td class="leftside">Class:</td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudentClass" runat="server" class="form-control" Width="203px"
    OnSelectedIndexChanged="Drp_StudentClass_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>  </td>
    </tr>
    <tr>
  
    <td class="leftside">Group From</td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudGroupFrom" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="Drp_StudGroupFrom_SelectedIndexChanged"
    runat="server" Width="203px"></asp:DropDownList>  </td>
    </tr>
    <tr>    
    <td class="leftside">Group To:</td>
    <td class="rightside"> <asp:DropDownList ID="Drp_StudentGroup" runat="server" class="form-control" Width="203px"></asp:DropDownList>  </td>
    </tr>
    <tr>   
    <td class="leftside"></td>
    <td class="rightside">
    <asp:Label ID="Lbl_StudErr" runat="server" ForeColor="Red" Text="" class="control-label" Font-Bold="false"> </asp:Label>
    </td>
    </tr>
    <tr>  
    <td class="leftside"></td>
    <td class="rightside"> 
    <asp:Button ID="Btn_StudGroupUpdate" runat="server" Text="Update" 
                OnClick="Btn_StudGroupUpdate_Click" Class="btn btn-info" Width="90px"/>
     <asp:Button ID="Btn_deletestudent" runat="server" Text="Remove" 
                OnClick="Btn_deletestudent_Click" Class="btn btn-info" Width="90px"/>
     </td>
    </tr>
    </table>
    
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
                             <asp:TemplateField HeaderText="Select"  HeaderStyle-Width="15px">
 
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

