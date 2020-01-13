<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="True" Inherits="ManageRole"  Codebehind="ManageRole.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main">
	
		<div class="left">

			<div class="content">

				<h1>
                    Manage Role Action</h1>

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


                <asp:UpdatePanel ID="pnlAjaxUpdate" runat="server">
                <ContentTemplate>
<asp:Panel id="Panel1" runat="server" Width="753px"><BR />

<TABLE style="WIDTH: 623px; HEIGHT: 295px"><TBODY>
<TR>
<TD style="HEIGHT: 62px" colSpan=2>&nbsp;Select Module&nbsp;<BR />&nbsp;<BR />
<asp:DropDownList id="Drp_Module" runat="server" Width="230px" OnSelectedIndexChanged="Drp_Module_SelectedIndexChanged" AutoPostBack="True">
 </asp:DropDownList><BR /><BR /><BR />&nbsp;Module Action</TD>
                    
<TD style="WIDTH: 374px; HEIGHT: 62px">&nbsp;Select Role <BR /><BR />
<asp:DropDownList id="Drp_Role" runat="server" Width="230px" 
 OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp; 
  <asp:ImageButton ImageAlign="AbsMiddle" ID="Img_Excel" runat="server" 
ImageUrl="~/Pics/Excel.png" 
 Width="30px" Height="30px" onclick="Img_Excel_Click" 
                                        ToolTip="Export to Excel"  />
 <BR /><BR />&nbsp;Role&nbsp; Action</TD>
 </TR>
  <TR>
<TD style="WIDTH: 374px; HEIGHT: 223px">
 <DIV style="OVERFLOW: scroll; WIDTH: 400px; HEIGHT: 400px; BACKGROUND-COLOR: gainsboro">
<asp:CheckBoxList id="ChkBoxModuAction" runat="server" Width="198px" ForeColor="Black" Font-Size="Small">
</asp:CheckBoxList></DIV></TD>
<TD style="WIDTH: 188px; HEIGHT: 223px"><asp:Button id="Btn_Add" onclick="Button2_Click" runat="server" Width="70px"
 Text=">>"></asp:Button> <BR /><BR />
<asp:Button id="Btn_Remove" onclick="Btn_Remove_Click" runat="server" Width="70px" Text="<<">
</asp:Button></TD><TD style="WIDTH: 300px; HEIGHT: 223px">
<DIV style="OVERFLOW: scroll; WIDTH: 400px; HEIGHT: 400px; BACKGROUND-COLOR: gainsboro">
<asp:CheckBoxList id="ChkBoxRoleAction" runat="server" Width="188px" ForeColor="Black" Font-Size="Small"
 Font-Bold="False">
</asp:CheckBoxList></DIV></TD></TR></TBODY></TABLE><BR /></asp:Panel> 
</ContentTemplate>
        <Triggers><asp:PostBackTrigger ControlID="Img_Excel" /></Triggers>
                    </asp:UpdatePanel>

			
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Role</h1>
				<ul>
					<li><a href="CreateRole.aspx">Create Role</a></li><li><a href="#">Manage Role</a></li><li><a href="DeleteRole.aspx">Delete Role</a></li></ul>
			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>
	<div class="stripes"><span></span></div>
</asp:Content>

