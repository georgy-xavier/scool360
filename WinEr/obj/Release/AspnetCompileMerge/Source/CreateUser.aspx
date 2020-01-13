<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="CreateUser"  Codebehind="CreateUser.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1> Create User</h1>

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
                        &nbsp;Name &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;:&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Username" runat="server" Width="189px" Height="17px" 
                            MaxLength="24" ontextchanged="Txt_Username_TextChanged"></asp:TextBox>&nbsp;<asp:TextBox 
                            ID="Txt_UserId" runat="server" Visible="False"></asp:TextBox>
                        <br />
                        &nbsp;<br />
                        &nbsp;E-Mail id &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Txt_email" runat="server" Width="189px" Height="17px" 
                            MaxLength="49"></asp:TextBox>&nbsp;<br />
                        &nbsp;<br />
                        &nbsp;User Role&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; :&nbsp;&nbsp;
                        <asp:DropDownList ID="Drp_UserRole" runat="server" Width="166px" Height="20px">
                        
                        </asp:DropDownList>&nbsp;Log In Rights &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                        &nbsp; &nbsp; :
                        <asp:DropDownList ID="Drp_Loginright" runat="server" AutoPostBack="True"
                            Width="102px" Height="20px" 
                            onselectedindexchanged="Drp_Loginright_SelectedIndexChanged">
                            <asp:ListItem Value="1">Can Login</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">Cannot Login</asp:ListItem>
                        </asp:DropDownList><br />
                        <br />
                        &nbsp;LogIn Name &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;:&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="Txt_LoginName" runat="server" Width="162px" Height="17px"></asp:TextBox>
                        ( Unique User Identifier)<br />
                        <asp:Panel ID="Panel2" runat="server" Width="752px" OnLoad="Panel2_Load">
                            <br />
                            &nbsp;Password&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Password1" runat="server" TextMode="Password" 
                                Width="162px" Height="17px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         
                          
                            <asp:RegularExpressionValidator runat="server" ID="PassWordRegularExpressionValidator1"
                                ControlToValidate="Txt_Password1"
                                Display="None"   ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"
                                ErrorMessage="Must contain 6 characters,at least one lower case letter and one digit." />
                              <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender10"
                                TargetControlID="PassWordRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                            <br />
                            <br />
                            &nbsp;Confirm Password :&nbsp;
                            
                            <asp:TextBox ID="Txt_Password2" runat="server" TextMode="Password" 
                                Width="163px" Height="17px"></asp:TextBox>
                            <br />
                        </asp:Panel>
                        &nbsp;<br />
                        &nbsp;<asp:LinkButton ID="LkBtn_UserDetails" runat="server" OnClick="LkBtn_UserDetails_Click">Add Details</asp:LinkButton><br />
                        <asp:Panel ID="Panel3" runat="server"  Width="752px" Visible="False">
                            <br />
                            &nbsp;Address &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Address" runat="server" Height="41px" TextMode="MultiLine" Width="274px"></asp:TextBox><br />
                            <br />
                            &nbsp;Date of Birth &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Dob" runat="server" Width="153px" Height="17px"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Txt_TaskStartdate_CalendarExtender" 
                                    runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="Txt_Dob" >
                            </ajaxToolkit:CalendarExtender>
                            &nbsp;&nbsp; Phone &nbsp; &nbsp;No&nbsp; :
                            <asp:TextBox ID="Txt_Ph" runat="server" Width="173px" Height="17px"></asp:TextBox><br />
                            <br />
                            </asp:Panel>
                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;</asp:Panel>
                     
                         <asp:RequiredFieldValidator runat="server" ID="NReq"
                            ControlToValidate="Txt_Username"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A name is required." />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="NReqE"
                            TargetControlID="NReq"
                            HighlightCssClass="validatorCalloutHighlight" />
                         <asp:RequiredFieldValidator runat="server" ID="PNReq"
                            ControlToValidate="Txt_LoginName"
                            Display="None"
                            ErrorMessage="<b>Required Field Missing</b><br />A Login name is required.<br /></div>" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqE"
                            TargetControlID="PNReq"
                            HighlightCssClass="validatorCalloutHighlight"
                            Width="350px" />
                         <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                ControlToValidate="Txt_Username"
                                Display="None"
                                ValidationExpression="(\w(\s)?)+"
                                ErrorMessage="<b>Invalid Field</b><br />Name should not contains such characters" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="RegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2"
                                ControlToValidate="Txt_LoginName"
                                Display="None"
                                ValidationExpression="(\w(\s)?)+"
                                ErrorMessage="<b>Invalid Field</b><br />Name should not contains such characters" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="RegularExpressionValidator2"
                                HighlightCssClass="validatorCalloutHighlight" /> 
                        <%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3"
                                ControlToValidate="Txt_Dob"
                                Display="None"
                                ValidationExpression="(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_Dob" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
                                TargetControlID="RegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                       <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4"
                                ControlToValidate="Txt_Ph"
                                Display="None"
                                ValidationExpression="(\+)?([-\._\(\) ]?[\d]{3,20}[-\._\(\) ]?){2,10}"
                                ErrorMessage="<b>Invalid Field</b><br />Phone number contains invalid characters" />
                       <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender4"
                                TargetControlID="RegularExpressionValidator4"
                                HighlightCssClass="validatorCalloutHighlight" />  
                    </ContentTemplate>
                    </asp:UpdatePanel>
                      
                      <asp:Panel ID="Panel5" runat="server" >
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Button ID="Btn_CreateUser" runat="server" Text="Create User" Width="111px" OnClick="Btn_CreateUser_Click" />
                      &nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Button ID="BttnReset" runat="server" Text="Reset" Width="111px" 
                              onclick="BttnCancel_Click" />
                      </asp:Panel>
                      <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:Label 
                            ID="Lbl_FailureNote" runat="server" Font-Size="Small" ForeColor="Red"
                                Height="27px" Width="325px"></asp:Label>
                                <br />
                               
                <asp:Panel ID="Panel4" runat="server" >
                <br />
                        
                        &nbsp;Upload&nbsp;Image&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;&nbsp;&nbsp;<asp:FileUpload 
                            ID="FileUp_Image" runat="server" Height="23px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Upload" runat="server" 
                            Text="Upload" Width="111px" onclick="Btn_Upload_Click" />
                        &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Width="111px" 
                        onclick="Btn_Cancel_Click" />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Lbl_Image" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                </asp:Panel> 
                    
			</div>

		</div>

		<div class="right">

			<div class="subnav">
           <h1>User</h1>
				<ul>
					<li><a href="#">Create User</a></li><li><a href="ManageUser.aspx">Manage User</a></li></ul>

			</div>

		</div>

		<div class="clearer"><span></span></div>
<div class="stripes"><span></span></div>
	</div>
	
</asp:Content>

              
