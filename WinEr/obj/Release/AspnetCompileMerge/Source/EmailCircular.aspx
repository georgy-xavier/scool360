<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailCircular.aspx.cs" Inherits="WinEr.EmailCircular" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
 <%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   &nbsp;&nbsp;

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
		 
	<div id="contents">
		 

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
	<ContentTemplate>
  
  <%--<div id="samplediv" style="width:300px;height:200px;background-color:Gray; display:none"><br /><br /></div>--%>
		<div class="container skin1"  >
		 <table  cellpadding="0" cellspacing="0" class="containerTable">
			<tr >

				<td class="no"><img alt="" src="Pics/mail_send1.png" height="35" width="35" />  </td>
				<td class="n">Email Home</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				<asp:Panel ID="Pnl_EmailInitial" runat="server">
				<center>
				<table width="550px">
				
				
				<tr>
				<td align="right">Class:</td>
				<td  align="left">
				<asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="170px">   
				</asp:DropDownList>
				</td>
				<td  align="right">Type:</td>
				<td  align="left">
				<asp:RadioButtonList ID="Rdb_CheckType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="5">
				  <asp:ListItem Text="All Staff" Value="0" Selected="True"></asp:ListItem> 
				  <asp:ListItem Text="Parent" Value="1"></asp:ListItem>
				<%--  <asp:ListItem Text="Student" Value="2" ></asp:ListItem>--%>
				 </asp:RadioButtonList>
				</td>
				
				</tr>
				
				<tr>
				<td align="right">Template:</td>
				<td  align="left">
				<asp:DropDownList ID="Drp_Template" runat="server" class="form-control" Width="170px" 
						AutoPostBack="True" onselectedindexchanged="Drp_Template_SelectedIndexChanged">   
				</asp:DropDownList>
				</td>
				<td  align="right">
			   
				</td>  
				<td align="left">
				 <asp:Button ID="Btn_SendEmail" runat="server" Text="Send" Width="90px" ValidationGroup="Send" class="btn btn-primary"  OnClick="Btn_SendEmail_Click"/>
				</td>             
				</tr>
			   
				</table>
				</center>
				</asp:Panel>
								
				   <asp:Panel ID="Panel_Email_Details" runat="server">
					<%-- <table width="95%">
					 <tr>
		   
					 </td>
					 </tr>
					 </table>--%>                                  
					  <table width="100%">
					  <tr>
					  <td style="width:400px" align="left" valign="bottom">
							<br />
						  <b>Subject:</b>
						  <asp:TextBox ID="Txt_EmailSubject" runat="server" class="form-control"  
											   Width="400px"></asp:TextBox>
						  
	  
					  </td>
					  <td style="width:400px;padding:5px;border:solid 1px brown;" align="left">
					  
					  <div>
								  <asp:Panel ID="pnlattachfiles" runat="server" 
									   Width="400px">
									  <asp:FileUpload ID="fileUploadattachments" class="col-lg-8" runat="server"  />
									  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:Button ID="Btnattach" runat="server" class="btn btn-primary col-lg-4" OnClick="Btnattach_Click" Width="100px"
									   Text="Attach File" />
								  </asp:Panel>
								  <br />
							  <asp:Label ID="lblattacherror" runat="server" class="control-label" Text="" ForeColor="Red"></asp:Label>
						 </div>
						 
					  <asp:Panel ID="pnlattachment" runat="server">
								   <asp:GridView ID="Grd_attachment" runat="server" AutoGenerateColumns="false"  ShowHeader="false"
									   BackColor="White" BorderStyle="None" DataKeyNames="FileName,RepositoryFileName" 
									   onrowdeleting="Grd_attachment_RowDeleting">
									   <Columns>
										   <asp:TemplateField >
											   <ItemTemplate>
												   <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" 
													   Height="20px" ImageUrl="Pics/DeleteRed.png" ToolTip="Delete" Width="20px" />
											   </ItemTemplate>
										   </asp:TemplateField>
										   <asp:BoundField DataField="FileName" ItemStyle-Width="250px"/>
										   <asp:BoundField DataField="RepositoryFileName" />
									   </Columns>
								   </asp:GridView>
							   </asp:Panel>
					  </td>
					  </tr>
					  </table>
					  
					   
				 
					
			  
				<div class="roundbox" >
					  <table width="100%">
					   <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
					   <tr><td class="centerleft"></td><td class="centermiddle">
		
					   <h5>Email Body</h5>
						<div class="linestyleNew"> </div>
						 <br />
						   <HTMLEditor:Editor ID="Editor_Body" runat="server" Height="300px"  
											Width="100%" />
											
											
					  </td><td class="centerright"></td></tr>
					  <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
					  </table>
		
		
					</div>
			
		  </asp:Panel>
		 
				 </td>
						 <td class="e"></td>
						 </tr>
					
					<tr>
						<td class="so">
						</td>
						<td class="s">
						</td>
						<td class="se">
						</td>
			 </tr>
					
					</table>
			 
	 </div>
	 
	  
	  <div class="clear"></div>
	 
	 
				
	<WC:MSGBOX id="WC_MsgBox" runat="server" /> 
		
	 </ContentTemplate>  
		
	  <Triggers>
			   <asp:PostBackTrigger ControlID="Btnattach"/>
	  </Triggers>
				 
		</asp:UpdatePanel> 
		
  


</div>
</asp:Content>
