<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="CreateGroup"  Codebehind="CreateGroup.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Create Group</h1>
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
				
                    <asp:Panel ID="Panel1" runat="server" Width="753px" >
                        <br />
                        &nbsp;Group Name &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; : &nbsp; &nbsp; 
                        <asp:TextBox ID="Txt_GpName" runat="server" Width="204px"></asp:TextBox><br />
                        <br />
                         <ajaxToolkit:FilteredTextBoxExtender ID="FilteredText_Name" 
                                runat="server" Enabled="True" TargetControlID="Txt_GpName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        &nbsp;Group Discription : &nbsp; &nbsp; 
                        <asp:TextBox ID="Txt_GpDiscr" runat="server" Height="36px" Width="418px" 
                            TextMode="MultiLine"></asp:TextBox><br />
                            <ajaxToolkit:FilteredTextBoxExtender ID="Flt_GpDiscr" 
                                runat="server" Enabled="True" TargetControlID="Txt_GpDiscr" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
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
                        <asp:Button ID="Btn_CreateGroup" runat="server" Height="23px" OnClick="Btn_CreateGroup_Click"
                            Text="Create Group" Width="80px" CssClass="submitnew" /><br />
                        <br />
                        <div class="stripes"><span></span></div>
                        <br />
                        &nbsp;Groups Present &nbsp; &nbsp; &nbsp; :<br />
                        <br />
                        <asp:RequiredFieldValidator runat="server" ID="NReq_name"
                            ControlToValidate="Txt_GpName"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A name is required." />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="NReqE"
                            TargetControlID="NReq_name"
                            HighlightCssClass="validatorCalloutHighlight" />
                        <asp:RequiredFieldValidator runat="server" ID="PNReq_disc"
                            ControlToValidate="Txt_GpDiscr"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A Description is required.<br /></div>" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqE"
                            TargetControlID="PNReq_disc"
                            HighlightCssClass="validatorCalloutHighlight"
                            Width="350px" />
                        <asp:GridView ID="Grd_Group" runat="server" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Size="Small"
                            ForeColor="Black" GridLines="Horizontal" Width="100%">
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

				<h1>Group</h1>
				<ul>
					<li><a href="#">Create Group</a></li><li><a href="ManageGroup.aspx">Manage Group</a></li><li><a href="DeleteGroup.aspx">Delete Group</a></li><li><a href="AddMembers.aspx">Manage Members</a></li></ul>
					
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

