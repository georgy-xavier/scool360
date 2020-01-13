<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="StudentCircular.aspx.cs" Inherits="WinEr.StudentCircular" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Watermark {
            color: #999999;
            font-size: medium;
            vertical-align: bottom;
            text-align: center;
            font-family: Times New Roman;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>



        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>



                <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no"></td>
                            <td class="n">Selective Circular</td>
                            <td class="ne"></td>
                        </tr>
                        <tr>
                            <td class="o"></td>
                            <td class="c">


                                <div style="width: 100%; background-color: Black; color: White; height: 30px; font-weight: bold; vertical-align: middle; padding-top: 5px">
                                    <marquee behavior="scroll" direction="left" scrollamount="4">
						 Please send sms using template format. Sms send without template format may not be delivered to all numbers. You can refer to different templates in sms circular page.
					   </marquee>
                                </div>
                                <br />

                                <ajaxToolkit:TabContainer runat="server" ID="Tabs" Width="100%"
                                    CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="0">

                                    <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Parent" Visible="true">
                                        <HeaderTemplate>
                                            <asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/business_user.png" />Parent
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <br />
                                            <center><table><tr><td >Select Template </td><td ><asp:DropDownList ID="Drp_parentTemplate" runat="server" class="form-control" Width="300px" AutoPostBack="True"
								onselectedindexchanged="Drp_parentTemplate_SelectedIndexChanged"></asp:DropDownList></td><td class="style1"></td></tr><tr><td valign="top">Message </td><td><asp:TextBox ID="Txt_Message" runat="server" Width="400px" class="form-control" Height="80px" TextMode="MultiLine"  MaxLength="600"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
								   runat="server" Enabled="True" TargetControlID="Txt_Message" 
								FilterMode="InvalidChars" InvalidChars="'\"></ajaxToolkit:FilteredTextBoxExtender><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Txt_Message"
									Display="Dynamic" ErrorMessage="<br>Please limit to 160 characters"
								   ValidationExpression="[\s\S]{1,599}"></asp:RegularExpressionValidator><br /><asp:TextBox ID="txtnewlanguage" runat="server" class="form-control" Width="400px" Height="80px" TextMode="MultiLine"  MaxLength="300"></asp:TextBox></td><td valign="top"  ><asp:Button ID="Btn_CheckConn" runat="server" Text="Check Connection"   Class="btn btn-primary" OnClick="Btn_CheckConnection_Click" /><br /><br /><asp:Button ID="btnConvert" runat="server" Text="Convert to Native language"   Class="btn btn-primary" OnClick="btnconvert_Click" /><asp:Button ID="Btn_Send" runat="server" Text="Send"  Class="btn btn-success" OnClick="Btn_Send_Click" width="152px"/></td></tr></table></center>
                                            <br />
                                            <center><asp:Panel ID="Pnl_Students" runat="server"><table><tr><td>Class </td><td><asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="180px" 
										AutoPostBack="True" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td><td>&#160;&#160;&#160;&#160; </td><td>Student </td><td><asp:DropDownList ID="Drp_Student" runat="server" class="form-control" Width="180px"></asp:DropDownList></td><td>&#160;&#160;&#160;&#160; </td><td><asp:Button ID="Btn_Add" runat="server" Text="Add" OnClick="Btn_Add_Click" 
										Enabled="False"  Class="btn btn-primary"/></td></tr></table><div style="text-align:center;width:100%;"><asp:Label ID="Lbl_Message" runat="server" class="control-label" ForeColor="Red" ></asp:Label>&#160; &#160; <asp:LinkButton ID="Lnk_Retry" ToolTip="Retry SMS" ForeColor="Blue" Font-Size="12px" 
								 runat="server" onclick="Lnk_Retry_Click">Retry</asp:LinkButton></div><asp:Panel ID="Pnl_studGrid" runat="server"><div style=" overflow:auto; height: 204px;"><asp:GridView  ID="GridStudents" runat="server" CellPadding="4" AutoGenerateColumns="False" 
				ForeColor="Black" GridLines="Vertical"  OnRowDeleting="GridStudents_RowDeleting"
				Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px"><RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" 
						HorizontalAlign="Left" /><FooterStyle BackColor="#BFBFBF" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" /><SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" /><HeaderStyle BackColor="#E9E9E9" Font-Bold="True" ForeColor="Black" 
					HorizontalAlign="Left" Font-Size="11px" /><AlternatingRowStyle BackColor="White" /><Columns><asp:BoundField DataField="Id" HeaderText ="Id" /><asp:BoundField DataField="Name" HeaderText="Name" /><asp:BoundField DataField="ClassId" HeaderText="ClassId" /><asp:BoundField DataField="Class" HeaderText="Class" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="Lnk_Del"  CommandName="Delete" runat="server">Delete</asp:LinkButton>
                            </ItemTemplate><ControlStyle ForeColor="#FF3300" />

                        </asp:TemplateField>
                     </Columns><EditRowStyle Font-Size="Medium" /></asp:GridView></div></asp:Panel></asp:Panel></center>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>

                                    <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Staff" Visible="true">
                                        <HeaderTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/user4.png" />staff
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <br />
                                            <center><table><tr><td >Select Template </td><td ><asp:DropDownList ID="Drp_staffTemplates" runat="server" class="form-control" Width="180px" AutoPostBack="true"
								onselectedindexchanged="Drp_StaffTemplate_SelectedIndexChanged"></asp:DropDownList></td><td></td></tr><tr><td valign="top">Message </td><td><asp:TextBox ID="Txt_Staff_Message" runat="server" Width="400px" class="form-control" Height="80px" TextMode="MultiLine"  ></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
								   runat="server" Enabled="True" TargetControlID="Txt_Staff_Message" 
								FilterMode="InvalidChars" InvalidChars="'\"></ajaxToolkit:FilteredTextBoxExtender><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Txt_Staff_Message"
									Display="Dynamic" ErrorMessage="<br>Please limit to 160 characters"
								   ValidationExpression="[\s\S]{1,299}"></asp:RegularExpressionValidator><ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1"  WatermarkCssClass="Watermark"
									  runat="server" Enabled="True" WatermarkText="Enter The Message" TargetControlID="Txt_Staff_Message"></ajaxToolkit:TextBoxWatermarkExtender></td><td valign="top"><asp:Button ID="Btn_Staff_CheckConnection" runat="server" Text="Check Connection" Class="btn btn-primary"   OnClick="Btn_Staff_CheckConnection_Click" /><br /><br /><asp:Button ID="Btn_Staff_Send" runat="server" Text="Send" Class="btn btn-success" OnClick="Btn_Staff_Send_Click" width="152px"/></td></tr></table></center>
                                            <center><asp:Panel ID="Panel1" runat="server"><table><tr><td>Staff </td><td><asp:DropDownList ID="Drp_Staff" runat="server" class="form-control" Width="160px"></asp:DropDownList></td><td></td><td><asp:Button ID="Btn_Staff_Add" runat="server" Text="Add" 
										OnClick="Btn_Staff_Add_Click" Class="btn btn-primary"/></td></tr></table><div style="text-align:center;width:100%;"><asp:Label ID="Lbl_Staff_msg" runat="server" class="control-label" ForeColor="Red" ></asp:Label>&#160; &#160; <asp:LinkButton ID="Lnk_retrystaff" ToolTip="Retry SMS" ForeColor="Blue" Font-Size="12px" 
								 runat="server" onclick="Lnk_retrystaff_Click">Retry</asp:LinkButton></div><asp:Panel ID="Panel_Staff_Grid" runat="server"><div style=" overflow:auto; height: 204px;"><asp:GridView  ID="GridStaff" runat="server" CellPadding="4" AutoGenerateColumns="False" 
				ForeColor="Black" GridLines="Vertical"  OnRowDeleting="GridStaff_RowDeleting"
				Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px" ><RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" 
						HorizontalAlign="Left" /><FooterStyle BackColor="#BFBFBF" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" /><SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" /><HeaderStyle BackColor="#E9E9E9" Font-Bold="True" ForeColor="Black" 
					HorizontalAlign="Left" Font-Size="11px" /><AlternatingRowStyle BackColor="White" /><Columns><asp:BoundField DataField="Id" HeaderText ="Id" /><asp:BoundField DataField="Name" HeaderText="Name" /><asp:TemplateField HeaderText="Delete"><ItemTemplate><asp:LinkButton ID="Lnk_Del" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" /></asp:TemplateField></Columns><EditRowStyle Font-Size="Medium" /></asp:GridView></div></asp:Panel></asp:Panel></center>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>

                                    <ajaxToolkit:TabPanel runat="server" ID="mobileapp" HeaderText="Mobile App" Visible="true">
                                        <HeaderTemplate>
                                            <asp:Image ID="Image3" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/user4.png" />Mobile App
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <br />
                                            <center>
               <div id="wrningMsg" runat="server" class="errmsgSH alert alert-danger alert-dismissable" visible="true">
                   <%--<a href="#" class="close" data-dismiss="alert" aria-label="close">×</a> --%>
                   <%--<strong>Something went wrong ! </strong>--%>
                   <asp:Label ID="Lbl_wrningMsg" runat="server">Send SMS to Parents who are not registered with WINER Parent App!</asp:Label>

               </div>
               <table><tr><td >Select Template </td><td ><asp:DropDownList ID="drp_parentlisttemplate" runat="server" class="form-control" Width="300px" AutoPostBack="True"
								onselectedindexchanged="Drp_parentTemplate_SelectedIndexChanged"></asp:DropDownList></td><td class="style1"></td></tr><tr><td valign="top">Message </td><td><asp:TextBox ID="txt_mobileappmsg" runat="server" Width="400px" class="form-control" Height="80px" TextMode="MultiLine"  MaxLength="300"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
								   runat="server" Enabled="True" TargetControlID="txt_mobileappmsg" 
								FilterMode="InvalidChars" InvalidChars="'\"></ajaxToolkit:FilteredTextBoxExtender><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_mobileappmsg"
									Display="Dynamic" ErrorMessage="<br>Please limit to 160 characters"
								   ValidationExpression="[\s\S]{1,599}"></asp:RegularExpressionValidator></td><td valign="top"  ><asp:Button ID="checkconnection" runat="server" Text="Check Connection"   Class="btn btn-primary" OnClick="Btn_CheckConnection_Click" /><br /><br /><asp:Button ID="sendbutton" runat="server" Text="Send"  Class="btn btn-success" OnClick="sendbutton_Click" width="152px"/></td></tr></table></center>
                                            <br />
                                            <center><asp:Panel ID="Panel2" runat="server"><table><tr><td>Class </td><td><asp:DropDownList ID="class_dropdown" runat="server" class="form-control" Width="180px" 
										AutoPostBack="True" OnSelectedIndexChanged="class_dropdown_SelectedIndexChanged"></asp:DropDownList></td><td>&#160;&#160;&#160;&#160; </td><td>Student </td><td><asp:DropDownList ID="DropDown_student" runat="server" class="form-control" Width="180px"></asp:DropDownList></td><td>&#160;&#160;&#160;&#160; </td><td><asp:Button ID="Button_add" runat="server" Text="Add" OnClick="Button_Add_Click" 
										Enabled="False"  Class="btn btn-primary"/></td></tr></table><div style="text-align:center;width:100%;"><asp:Label ID="Lbl_Message1" runat="server" class="control-label" ForeColor="Red" ></asp:Label>&#160; &#160; <asp:LinkButton ID="Link_retry" ToolTip="Retry SMS" ForeColor="Blue" Font-Size="12px" 
								 runat="server" onclick="Link_Retry_Click">Retry</asp:LinkButton></div><asp:Panel ID="panel_student" runat="server"><div style=" overflow:auto; height: 204px;"><asp:GridView  ID="grid_student" runat="server" CellPadding="4" AutoGenerateColumns="False" 
				ForeColor="Black" GridLines="Vertical"  OnRowDeleting="grid_students_RowDeleting"
				Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px"><RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" 
						HorizontalAlign="Left" /><FooterStyle BackColor="#BFBFBF" ForeColor="Black" /><PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" /><SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" /><HeaderStyle BackColor="#E9E9E9" Font-Bold="True" ForeColor="Black" 
					HorizontalAlign="Left" Font-Size="11px" /><AlternatingRowStyle BackColor="White" /><Columns><asp:BoundField DataField="Id" HeaderText ="Id" /><asp:BoundField DataField="Name" HeaderText="Name" /><asp:BoundField DataField="ClassId" HeaderText="ClassId" /><asp:BoundField DataField="Class" HeaderText="Class" /><asp:TemplateField HeaderText="Delete"><ItemTemplate><asp:LinkButton ID="Lnk_Del"  CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" /></asp:TemplateField></Columns><EditRowStyle Font-Size="Medium" /></asp:GridView></div></asp:Panel></asp:Panel></center>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>


                                </ajaxToolkit:TabContainer>
                                <br />


                            </td>
                            <td class="e"></td>
                        </tr>
                        <tr>
                            <td class="so"></td>
                            <td class="s"></td>
                            <td class="se"></td>
                        </tr>
                    </table>
                </div>

                <asp:Panel ID="Panel3" runat="server">

                    <asp:Button runat="server" ID="Button_main" class="btn btn-info" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="MPE_Message" runat="server" CancelControlID="Button_MainOk"
                        PopupControlID="PanelMain" TargetControlID="Button_main" BackgroundCssClass="modalBackground" />
                    <asp:Panel ID="PanelMain" runat="server" Style="display: none;">
                        <div class="container skin1" style="width: 400px; top: 400px; left: 400px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr>
                                    <td class="no">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png"
                                            Height="28px" Width="29px" />
                                    </td>
                                    <td class="n"><span style="color: Black">Message</span></td>
                                    <td class="ne"></td>
                                </tr>
                                <tr>
                                    <td class="o"></td>
                                    <td class="c">
                                        <div style="font-weight: bold">

                                            <center>
			            <div id="DivMainMessage" runat="server">
				 
			            </div>
		            </center>

                                        </div>

                                        <br />
                                        <br />
                                        <div style="text-align: center;">

                                            <asp:Button ID="Button_MainOk" runat="server" Text="OK" Class="btn btn-info" Width="80px" />
                                        </div>
                                    </td>
                                    <td class="e"></td>
                                </tr>
                                <tr>
                                    <td class="so"></td>
                                    <td class="s"></td>
                                    <td class="se"></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </div>
                    </asp:Panel>
                </asp:Panel>


            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear"></div>
    </div>
</asp:Content>
