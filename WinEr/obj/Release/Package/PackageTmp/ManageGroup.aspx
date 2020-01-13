<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="ManageGroup"  Codebehind="ManageGroup.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Manage Group</h1>

				<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
				<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
                        <br />
                        &nbsp;Group Name &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; : &nbsp; &nbsp; 
                        <asp:TextBox ID="Txt_GpName" runat="server" Width="204px"></asp:TextBox>
                        &nbsp; &nbsp; Group Id &nbsp; : &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_GpId" runat="server" Enabled="False" Width="110px"></asp:TextBox><br />                     
                        <br />
                        &nbsp;Group Discription : &nbsp; &nbsp; 
                        <asp:TextBox ID="Txt_GpDiscr" runat="server" Height="36px" Width="413px" TextMode="MultiLine"></asp:TextBox><br />
                        <br />
                        &nbsp;Group Parent &nbsp; &nbsp; &nbsp;&nbsp; : &nbsp; &nbsp; 
                        <asp:DropDownList ID="Drp_ParentList" runat="server" Width="151px">
                        </asp:DropDownList>&nbsp;&nbsp; Group Manager &nbsp; &nbsp;:&nbsp;&nbsp;<asp:DropDownList ID="Drp_UserList" runat="server" Width="159px">
                        </asp:DropDownList><br />
                        <br />
                        &nbsp;Group Type &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; : &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="Drp_GroupTypeList" runat="server" Width="150px">
                        </asp:DropDownList><br />
                        <br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
       
                        <asp:Label ID="Lbl_FailureNote" runat="server" Font-Size="Small" ForeColor="Red"
                            Height="27px" Width="425px"></asp:Label><br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        
                        
                        <asp:Button ID="Btn_UpdateGroup" runat="server" Height="36px" OnClick="Btn_CreateGroup_Click"
                            Text="Update Group" Width="172px" /><br />
                        <br />
                        <div class="stripes"><span></span></div>
                        <br />
                        &nbsp;Groups Present &nbsp; &nbsp; &nbsp; : &nbsp;&nbsp;Select a group from the grid
                        for modification<br />
                        <br />
                        <asp:GridView ID="Grd_Group" runat="server" 
                            OnRowDataBound="Grd_Group_RowDataBound" BackColor="LightGoldenrodYellow" 
                            BorderColor="Tan" BorderWidth="1px" CellPadding="2" Font-Size="Small"
                            ForeColor="Black" GridLines="None" Width="737px" 
                            AutoGenerateSelectButton="True" 
                            OnSelectedIndexChanged="Grd_Group_SelectedIndexChanged">
                            <FooterStyle BackColor="Tan" />
                            <EditRowStyle Font-Size="Small" />
                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor= "red"/>
                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                                HorizontalAlign="Center" />
                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                        </asp:GridView>
                    </asp:Panel>
                    </ContentTemplate>
                   </asp:UpdatePanel>

			
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Group</h1>
				<ul>
					<li><a href="CreateGroup.aspx">Create Group</a></li><li><a href="#">Manage Group</a></li><li><a href="DeleteGroup.aspx">Delete Group</a></li><li><a href="AddMembers.aspx">Manage Members</a></li></ul>
					
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

