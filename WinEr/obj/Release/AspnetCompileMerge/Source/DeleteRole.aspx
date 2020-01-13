<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="DeleteRole"  Codebehind="DeleteRole.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Delete Role</h1>

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
                    <asp:Panel ID="Panel1" runat="server" Width="753px">
                        &nbsp;Role Name : &nbsp;<asp:TextBox ID="TxtRoleName" runat="server" Width="158px" Enabled="False"></asp:TextBox>
                        <asp:TextBox ID="TxtRoleId" runat="server" Enabled="False" Visible="False" Width="43px"></asp:TextBox>
                        &nbsp;
                        <asp:Button ID="BtnDeleteRole" runat="server" Height="22px" Text="Delete Role" Width="104px" OnClick="BtnDeleteRole_Click" /><br />
                        <br />
                        <ajaxToolkit:ConfirmButtonExtender ID="BtnDeleteRole_ConfirmButtonExtender" 
                            runat="server"  Enabled="True" TargetControlID="BtnDeleteRole"
                            DisplayModalPopupID="BtnDeleteRole_ModalPopupExtender">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <ajaxToolkit:ModalPopupExtender ID="BtnDeleteRole_ModalPopupExtender" runat="server" TargetControlID="BtnDeleteRole" PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" />
                        <asp:Panel ID="PNL" runat="server" style="display:none; width:200px; background-color:White; color:Black ; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                        Are you sure you want to Delete this Role?
                        <br /><br />
                        <div style="text-align:right;">
                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                            <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                        </div>
                    </asp:Panel>
                        <br />                     
                        <br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        <asp:Label ID="Lbl_FailureNote" runat="server" Font-Size="Small" ForeColor="Red"
                        
                            Height="27px" Width="425px"></asp:Label>
                            <div class="stripes"><span></span></div>
                        <br />
                        &nbsp;Roles Present &nbsp; &nbsp; &nbsp; : &nbsp;&nbsp;Select a role from the grid
                        for Deletion<br />
                        <br />
                        <asp:GridView ID="Grd_Role" runat="server" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Size="Small"
                            ForeColor="Black" GridLines="Horizontal" Width="737px" AutoGenerateSelectButton="True" OnSelectedIndexChanged="Grd_Role_SelectedIndexChanged">
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <EditRowStyle Font-Size="Small" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                       </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>

			
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Role</h1>
				<ul>
					<li><a href="CreateRole.aspx">Create Role</a></li><li><a href="ManageRole.aspx">Manage Role</a></li><li><a href="#">Delete Role</a></li></ul>
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

