<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="DeleteGroup"  Codebehind="DeleteGroup.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">
			<div class="content">

				<h1> Delete Group</h1>

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
                    <asp:Panel ID="Panel1" runat="server"  >
                       
                       
                        
                        <br />
                        &nbsp;<asp:Label ID="Lbl_FailureNote" runat="server" Font-Size="Small" 
                            ForeColor="Red" Height="27px" Width="636px"></asp:Label>
                       
                        <br />
                        <div style=" overflow:scroll;width: 617px; height: 200px;">
                        <asp:GridView DataKeyNames="Id" ID="Grd_Grp" runat="server" 
                                AutoGenerateColumns="False"  OnRowDataBound="GridView1_RowDataBound"  
                                OnRowDeleting="GridView1_RowDeleting" BackColor="LightGoldenrodYellow" 
                                BorderColor="Tan" BorderWidth="1px" CellPadding="2" 
                                ForeColor="Black"  Width="593px" GridLines="None" CellSpacing="3">
                            <RowStyle Font-Size="Small" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" />
                <asp:BoundField DataField="GroupName" HeaderText="GroupName" />
                <asp:BoundField DataField="ModifiedDate" HeaderText="ModifiedDate" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                    </ItemTemplate>
                    <ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
            </Columns>
                            <FooterStyle BackColor="Tan" />
                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                                HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                            <HeaderStyle BackColor="Tan" Font-Bold="True" Font-Size="Small" 
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
                        </div>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				

				<h1>Group</h1>
				<ul>
					<li><a href="CreateGroup.aspx">Create Group</a></li><li><a href="ManageGroup.aspx">Manage Group</a></li><li><a href="#">Delete Group</a></li><li><a href="AddMembers.aspx">Manage Members</a></li></ul>
					
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

