<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="HouseStudentMap.aspx.cs" Inherits="WinEr.HouseStudentMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" >

        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=grd_studList.ClientID%>');
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
				<td class="n">Map Student To House</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				        <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  
         CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="2" >
               
               
                  <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Manual"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image1" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/next.png" />
    Automatic</HeaderTemplate>                         
    
     <ContentTemplate> 
     
     <table>
     <tr>
     <td>
     <asp:Label ID="lbl_AutoErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
     </td>
     </tr>
     </table>
     <asp:Panel ID="Pnl_AutoStudList" runat="server">
     <asp:Label ID="Lbl_config" runat="server"></asp:Label>
     <br />
     <br />
     <div align="right" style="width:400px">
      <asp:Button ID="Btn_AutoMap" runat="server" Width="90px" class="btn btn-primary" Text="Assign" 
             onclick="Btn_AutoMap_Click" />    
     </div>
     <br />
     <table width="100%" class="tablelist">         
     <tr>
     <td class="leftside">
    Total Students:
     </td>
     <td class="rightside"><asp:Label ID="Lbl_Totstud" class="control-label" runat="server" ></asp:Label></td>
     </tr>
      <tr>
     <td class="leftside"> 
     Total Mapped:  
     </td>
     <td class="rightside"><asp:Label ID="Lbl_Totmapstud" class="control-label" runat="server"></asp:Label></td>
     </tr>
        <tr>
     <td class="leftside"> 
     Total Unmapped:  
     </td>
     <td class="rightside"><asp:Label ID="Lbl_Totunmapstud" class="control-label" runat="server"></asp:Label></td>
     </tr>
     <tr>
     <td>
     
     </td>
     </tr>
     </table>
     </asp:Panel>
     
     </ContentTemplate>  
   </ajaxToolkit:TabPanel>
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Manual"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/edit.png" />
    Manual</HeaderTemplate>                         
    
     <ContentTemplate> 
				
				<asp:Panel ID="Pnl_TopArea" runat="server">
				<table width="100%" class="tablelist" runat="server" id="Tbl_Add">
				<tr>
				<td  class="leftside">
				
				Select Class</td>
				<td  class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" class="form-control" AutoPostBack="true"
                        Width="173px" onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList>
				</td>
				</tr>
				<tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				<tr>
				<td class="leftside">
				Select House
				</td>
				<td class="rightside">
				<asp:DropDownList ID="Drp_House" runat="server" class="form-control" Width="173px"></asp:DropDownList>
				</td> 			
				</tr>
			
				<tr>
				<td class="leftside">
				</td>
				<td class="rightside">
				<asp:Label ID="Lbl_Err" runat="server" class="control-label" ForeColor="Red"></asp:Label></td>
				</tr>
				</table>
				
				<table class="tablelist" runat="server" id="Tbl_Edit">
				
				</table>
				</asp:Panel>
				<br />
				<br />
				
				<div class="linestyle"></div>
				
				<asp:Panel ID="Pnl_Studlist" runat="server">
				<table width="100%" class="tablelist">	
				<tr>
				<td align="right">
				<asp:Button ID="Btn_Map" runat="server" Text="Assign" Width="90px" class="btn btn-primary"
                        onclick="Btn_Map_Click" />
				</td>
				</tr>			
				<tr>
				<td>
				 <asp:GridView ID="grd_studList" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                          OnPageIndexChanging="grd_studList_PageIndexChanging"
                    AllowPaging="true" PageSize="15"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      Width="100%" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Height="50px" />
                              <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                              <asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="40px"   SortExpression="RollNo"/>
                              <asp:BoundField DataField="StudentName" HeaderText="Name"  SortExpression="StudentName"/>
                               <asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="40px"    SortExpression="Sex" />
                               <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="100px"  />
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
              </asp:GridView>
                </td>
				</tr></table></asp:Panel>
				
      </ContentTemplate>  
   </ajaxToolkit:TabPanel>
   
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="EditMap"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image2" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/edit student.png" />
    EditMap</HeaderTemplate>                         
    
     <ContentTemplate>
               <div class="container skin1"  >
       
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Student-House Map Edit</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
               <asp:Panel ID="Panel1" runat="server">
                <table width="100%" class="tablelist">
               <tr>
                <td class="leftside">Select House</td>
                <td class="rightside"><asp:DropDownList ID="DropDownListhouse" runat="server" class="form-control" Width="173px"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                <tr>
                <td class="leftside">Select Class</td>
                <td class="rightside"><asp:DropDownList ID="DropDownListclass" runat="server" class="form-control" Width="173px"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   <tr>
                <td class="leftside">Select Gender</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Gender" runat="server" class="form-control" Width="173px">
                 <asp:ListItem Text="All" Value="0">
                 </asp:ListItem>
                <asp:ListItem Text="Male" Value="1">
                 </asp:ListItem>
                  <asp:ListItem Text="Female" Value="2">
                 </asp:ListItem>
                </asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                  <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show"  class="btn btn-primary"
                        Width="90px" onclick="btnshow_click" /></td>
                </tr>
                    <tr>
                <td colspan="2"><asp:Label ID="Label1" runat="server" class="control-label" ForeColor="Red"></asp:Label></td>
                </tr>
                </table>
                </asp:Panel> 

				
				<div class="linestyle"></div>
				
				<asp:Panel ID="Paneleditmap" runat="server">
				<table width="100%" class="tablelist">	
				<tr>
				<td align="right">
				<asp:Button ID="Buttonsave" runat="server" Text="Save" Width="90px" class="btn btn-info"
                        onclick="Btn_save_Click" />
				</td>
				<tr>
				<td align="right"><asp:Label ID="lblcheck" runat="server" class="control-label" ForeColor="Red"></asp:Label></td>
				</tr>
				</tr>			
				<tr>
				<td>
				 <asp:GridView ID="GridViewmap" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                          OnPageIndexChanging="GridViewmap_PageIndexChanging"
                          
                    AllowPaging="true" PageSize="15"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      Width="100%" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                              <asp:TemplateField  HeaderText="All">
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 Select
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                             <asp:BoundField DataField="studid" HeaderText="Stud Id"/>
                             <asp:BoundField DataField="houseid" HeaderText="House Id"/>
                             <asp:BoundField DataField="StudentName" HeaderText="Student Name"/>
                             <asp:BoundField DataField="Sex" HeaderText="Gender" />
                             <asp:BoundField DataField="ClassName" HeaderText="Class Name"/>                      
                             <asp:BoundField DataField="HouseName" HeaderText="House Name"/>
                             <%--<asp:BoundField DataField="Address" HeaderText="Address"/>--%>
                             <asp:TemplateField HeaderText="Select house" >
                             
                             <ItemTemplate>
                             
                             <asp:DropDownList ID="DropDownListeditmap" runat="server" class="form-control" Width="193px"></asp:DropDownList>
                             </ItemTemplate>
                       
                             </asp:TemplateField>
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
              </asp:GridView>
                </td>
			
				</tr></table></asp:Panel>
				</td><td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
		</div>
				
     </ContentTemplate>
   </ajaxToolkit:TabPanel>

    </ajaxToolkit:tabcontainer>
			
			    
				</td><td class="e"> </td>
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
 <%--<Triggers>
 <asp:PostBackTrigger ControlID="btn_excel" />
 </Triggers>--%></asp:Content>