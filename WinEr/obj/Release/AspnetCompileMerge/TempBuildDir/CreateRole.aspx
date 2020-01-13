<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="CreateRole"  Codebehind="CreateRole.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1>Create Role</h1>

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
                        Role Name &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; :&nbsp;
                        <asp:TextBox ID="Txt_RoleName" runat="server" Width="128px" MaxLength="24" 
                            Height="22px"></asp:TextBox>&nbsp; <br />
                        <br />
                        Role Discription :&nbsp;
                        <asp:TextBox ID="Txt_RoleDisc" runat="server" Height="30px" Width="266px" 
                            TextMode="MultiLine" MaxLength="254"></asp:TextBox>
                        <br />
                        <br />
                        Role Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;
                        <asp:DropDownList ID="Drp_RoleType" runat="server" Height="22px" Width="128px">
                            <asp:ListItem Selected="True">General</asp:ListItem>
                            <asp:ListItem>Staff</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;<asp:Label ID="Lbl_FailureNote" runat="server" Font-Size="Small"
                        ForeColor="Red" Height="27px" Width="425px"></asp:Label><br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp;&nbsp;
                        <asp:Button ID="Btn_CreateRole" runat="server" OnClick="Btn_CreateRole_Click" Text="Create Role"
                         Width="117px" /><br />
                        <asp:RequiredFieldValidator runat="server" ID="NReq"
                            ControlToValidate="Txt_RoleName"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A name is required." />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="NReqE"
                            TargetControlID="NReq"
                            HighlightCssClass="validatorCalloutHighlight" />
                         
                        <asp:RequiredFieldValidator runat="server" ID="PNReq"
                            ControlToValidate="Txt_RoleDisc"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A Description is required.<br /></div>" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqE"
                            TargetControlID="PNReq"
                            HighlightCssClass="validatorCalloutHighlight"
                            Width="350px" />
                        <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_RoleName"
                                Display="None"
                                ValidationExpression="(\w(\s)?)+"
                                ErrorMessage="<b>Invalid Field</b><br />Name should not contains such characters" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                          
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>

			
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Role</h1>
				<ul>
					<li><a href="#">Create Role</a></li><li><a href="ManageRole.aspx">Manage Role</a></li><li><a href="DeleteRole.aspx">Delete Role</a></li></ul>
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

