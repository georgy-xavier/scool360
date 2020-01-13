<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="ViewBookHistory.aspx.cs" Inherits="WinEr.ViewBookHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<div id="right">

<div class="label">Library Manager</div>
<div id="SubLibMenu" runat="server">
		
 </div>
</div>

<div id="left">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:Panel ID="Pnl_mainarea" runat="server">
    
        

	    <div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/book_accept.png"  
                        Height="28px" Width="29px" />  </td>
				<td class="n">Book History</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<br />
				
				<asp:Panel ID="Topdata" runat="server">
				<table width="100%"  class="tablelist">
				    <tr >
				        <td class="leftside">Book Name :</td>
				        <td class="rightside">
                            <asp:Label ID="Lbl_BookName" runat="server" Font-Bold="True"  ForeColor="Black"></asp:Label></td>
				    </tr>
				    <tr >
				        <td  class="leftside">Current Status :</td>
				        <td  class="rightside">
                            <asp:Label ID="Lbl_Status" runat="server" Font-Bold="True"   ForeColor="Black"></asp:Label></td>
				    </tr>
				    <tr>
				        <td colspan="2" align="center">
                            <asp:Label ID="Lbl_Message" runat="server"  ForeColor="Red"></asp:Label>
				        </td>
				    </tr>
				</table>
				</asp:Panel>
				<div class="linestyle"></div>
				     <br />
				  <asp:Panel ID="Pnl_history"    runat="server">
				  
				 <div style=" overflow:auto; height: 304px;">
				    <asp:GridView  ID="GrdBkHistory" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" 
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4" OnRowDataBound="Grd_Bkhistoy_RowDataBound"   OnRowDeleting="Grd_Bkhistoy_RowDeleting"
                           ForeColor="Black" GridLines="Vertical" Width="100% " 
                         PageSize="30"  DataKeyNames="Id">
                           
                           <Columns>
                               <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="BookId" HeaderText="Book Id" />
                                <asp:BoundField DataField="UserId" HeaderText="User Name" />
                                <asp:BoundField DataField="UserTypeId" HeaderText="User Type" />
                                <asp:BoundField DataField="TakenDate" HeaderText="Issued Date" />
                                <asp:BoundField DataField="ReturnedDate" HeaderText="Returned Date"   />  
                                <asp:BoundField DataField="FineAmount" HeaderText="FineAmount" />
                                <asp:BoundField DataField="Comment" HeaderText="Comment" />
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                                     </ItemTemplate>
                                 <ControlStyle ForeColor="#FF3300" />
                             </asp:TemplateField>
                            </Columns>

                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
                             <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                             <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                             <EditRowStyle Font-Size="Medium" />    
                       </asp:GridView>
                 </div>
				  </asp:Panel>
       
				<br />
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
</div>
<div class="clear"></div>
</div>
</asp:Content>
