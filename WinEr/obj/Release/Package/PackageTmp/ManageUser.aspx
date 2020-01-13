<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="ManageUser"  Codebehind="ManageUser.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Manage User</h1>

				 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
				 <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdate">
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
                 <asp:Panel ID="Panel5" runat="server"  Width="618px">
                     &nbsp; LogIn Name &nbsp; &nbsp; &nbsp; &nbsp; :
                     <asp:TextBox ID="Txt_LoginName" runat="server" Width="168px"></asp:TextBox>
                     &nbsp;<asp:Button ID="LnkBtn_Details" runat="server" 
                         onclick="LnkBtn_Details_Click" Text="Show Details" Width="111px" />
&nbsp;<asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" Height="39px" 
                         Width="200px"></asp:Label>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                     <asp:TextBox ID="Txt_UserId" runat="server" Visible="False" Width="161px"></asp:TextBox>
                     &nbsp;
                     <br />
                     <br />
                </asp:Panel>
                 <asp:UpdatePanel ID="pnlAjaxUpdate" runat="server">
                 <ContentTemplate>                                
                    <asp:Panel ID="Panel1" runat="server" Width="753px">
                    <table>
                        <tr>
                            <td>
                    
                                &nbsp;Sur Name &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;:
                        <asp:TextBox ID="Txt_SurName" runat="server" Width="168px"></asp:TextBox>&nbsp;<br />
                        <br />
                        &nbsp;E-Mail id &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; :
                        <asp:TextBox ID="Txt_email" runat="server" Width="168px"></asp:TextBox>&nbsp;<br />
                        <br />
                        &nbsp;User Role&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; :
                        <asp:DropDownList ID="Drp_UserRole" runat="server" Width="168px">
                        </asp:DropDownList>&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                        <br />
                        <br />
                        &nbsp;&nbsp;Login Right&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :
                        <asp:DropDownList ID="Drp_Loginright" runat="server" AutoPostBack="True"
                            Width="102px" onselectedindexchanged="Drp_Loginright_SelectedIndexChanged"  
                            >
                            <asp:ListItem Value="1">Can Login</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">Cannot Login</asp:ListItem>
                        </asp:DropDownList>
                            </td>
                            <td style="width: 63px"></td>
                            <td>
                                <asp:ImageButton ID="ImgBtn_User" runat="server" Height="154px" 
                                    Width="154px" />
                                
                            </td>
                         </tr>
                      </table>
                        
                        <asp:Panel ID="Panel2" runat="server" OnLoad="Panel2_Load" Width="752px">
                            <br />
                            &nbsp;<asp:LinkButton ID="LkBtn_Resetpw" runat="server" 
                                OnClick="LkBtn_Resetpw_Click">Reset Password</asp:LinkButton>
                            <asp:Panel ID="Panel4" runat="server" Visible="False" Width="751px">
                                &nbsp;<br />
                                &nbsp;New Password &nbsp;&nbsp; :
                                <asp:TextBox ID="Txt_Password1" runat="server" TextMode="Password" 
                                    Width="162px"></asp:TextBox>
                                &nbsp; Re-Enter Password :
                                <asp:TextBox ID="Txt_Password2" runat="server" TextMode="Password" 
                                    Width="163px"></asp:TextBox>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        &nbsp;<asp:LinkButton ID="LkBtn_UserDetails" runat="server" OnClick="LkBtn_UserDetails_Click">Add Details</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Lbl_Details" runat="server" ForeColor="Red" Width="155px"></asp:Label>
                        <br />
                        <asp:Panel ID="Panel3" runat="server"  Width="752px" Visible="False">
                            <br />
                            &nbsp;Address &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; :
                            <asp:TextBox ID="Txt_Address" runat="server" Height="41px" TextMode="MultiLine" Width="274px"></asp:TextBox><br />
                            <br />
                            &nbsp;Date of Birth &nbsp; &nbsp;&nbsp; :
                            <asp:TextBox ID="Txt_Dob" runat="server" Width="153px"></asp:TextBox>
                            &nbsp;&nbsp; Phone &nbsp; &nbsp;No&nbsp; :
                            <asp:TextBox ID="Txt_Ph" runat="server" Width="173px"></asp:TextBox><br />
                            <br />
                            &nbsp;</asp:Panel>
                        &nbsp;<br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp;&nbsp;
                        <asp:Label ID="Lbl_FailureNote" runat="server" Font-Size="Small" ForeColor="Red"
                            Height="27px" Width="425px"></asp:Label><br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp;&nbsp;
                        <asp:Button ID="Btn_UpdateUser" runat="server" Text="Update User" Width="111px" OnClick="Btn_UpdateUser_Click" />
                        
                        
                        &nbsp;</asp:Panel>
                     </ContentTemplate>
                     </asp:UpdatePanel>
                
                <asp:Panel ID = "Panel7" runat="server" >
                <br /><br />&nbsp;Change Image&nbsp;&nbsp;&nbsp; :
                    <asp:FileUpload ID="FileUp_User" runat="server" />
                    &nbsp;&nbsp;
                    <asp:Button ID="Btn_Upload" runat="server" Text="Upload" Width="111px" 
                        onclick="Btn_Upload_Click" />
                    &nbsp;<asp:Label ID="Lbl_Upload" runat="server" Height="22px" Width="180px" 
                        ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:Button ID="Btn_DeleteUser" runat="server" onclick="Btn_DeleteUser_Click" 
                            Text="Delete User" Width="111px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Width="111px" 
                            onclick="Btn_Cancel_Click" />
                    
                </asp:Panel>
			
			</div>

		</div>

		<div class="right">

			<div class="subnav">
           <h1>User</h1>
				<ul>
					<li><a href="CreateUser.aspx">Create User</a></li><li><a href="#">Manage User</a></li></ul>

			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

