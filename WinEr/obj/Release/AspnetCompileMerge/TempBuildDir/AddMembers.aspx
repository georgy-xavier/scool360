<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="AddMembers"  Codebehind="AddMembers.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Manage Group Members</h1>
				<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>
<asp:Panel id="Panel1" runat="server" Width="753px"><br /><table style="WIDTH: 623px; HEIGHT: 295px"><tbody><tr>
        <td style="HEIGHT: 62px" colspan=2>&nbsp;<br />
            <asp:Label ID="Lbl_Note" runat="server" Font-Size="Small" 
        ForeColor="Red" Height="20px" Width="318px"></asp:Label>
            <br />
            <br /><br />&nbsp;Select Users<asp:Label ID="Label1" runat="server" 
        Text="Label"></asp:Label>
        </TD><td style="WIDTH: 374px; HEIGHT: 62px">&nbsp;Select Group<br /><br />
            <asp:DropDownList id="Drp_Group" runat="server" Width="230px" OnSelectedIndexChanged="Drp_Group_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList><br />
            <br />&nbsp;Group Members</td></tr>
        <tr><td style="WIDTH: 300px; HEIGHT: 250px">
            <div style="OVERFLOW: scroll; WIDTH: 230px; HEIGHT: 230px; BACKGROUND-COLOR: Gray"><asp:CheckBoxList id="ChkBoxUser" runat="server" Width="188px" ForeColor="Black" Font-Size="Small" Font-Bold="False">
                                    </asp:CheckBoxList></DIV></TD>
            <td style="WIDTH: 188px; HEIGHT: 250px">
                <asp:Button id="Btn_Add" onclick="Button2_Click" runat="server" Width="98px" Text="ADD >>"></asp:Button> 
                <br /><br />
                <asp:Button id="Btn_Remove" onclick="Btn_Remove_Click" runat="server" Width="98px" Text="<< Remove"></asp:Button></TD>
            <td style="WIDTH: 374px; HEIGHT: 250px">
                <div style="OVERFLOW: scroll; WIDTH: 230px; HEIGHT: 230px; BACKGROUND-COLOR: Gray"><asp:CheckBoxList id="ChkBoxGrpMemb" runat="server" Width="198px" ForeColor="Black" Font-Size="Small">
                                    </asp:CheckBoxList></div></td></tr></tbody></table><br /></asp:Panel> 
</ContentTemplate>
                    </asp:UpdatePanel>

			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Group</h1>
				<ul>
					<li><a href="CreateGroup.aspx">Create Group</a></li><li><a href="ManageGroup.aspx">Manage Group</a></li><li><a href="DeleteGroup.aspx">Delete Group</a></li><li><a href="#">Manage Members</a></li></ul>
					
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

