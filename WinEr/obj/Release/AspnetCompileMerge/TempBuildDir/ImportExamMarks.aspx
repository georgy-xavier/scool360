<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ImportExamMarks.aspx.cs" Inherits="WinEr.ImportExamMarks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.style1
		{
			height: 18px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="contents">

<div id="right">

<div class="label">Exam Info</div>
<div id="SubExammngMenu" runat="server">
		
 </div>
</div>

<div id="left">
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
 
 
 
 
   
   <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Enter Marks</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				

	
	   <div id="topstrip">
				 <asp:Panel ID="Panel2" runat="server">
									<table style="width:100%;" >
										<tr>
											
											<td style="width: 266px;">
												<asp:Label ID="Lbl_ExamName" runat="server"  ForeColor="White" class="control-label"
													Text="University Exam"></asp:Label>
											</td>
                                            <td>&nbsp;</td>
                                            
											
										</tr>
                                      </table>
                                        <table style="width:100%;" >
										<tr>
											<td >&nbsp;</td>
                                            <td >&nbsp;</td>
											<td style="width:70px;"> 
												<asp:Label ID="Lbl_Examtypelb" runat="server" ForeColor="White" class="control-label"
													Text="Exam Type :"></asp:Label>
											</td>
                                           
                                            <td >&nbsp;</td>
											<td>
												<asp:Label ID="Lbl_ExamType" runat="server" ForeColor="White" class="control-label"
													Text="MAIN"></asp:Label>
											</td>                                            
                                            
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                             <td >&nbsp;</td>
                                            <td align="right">
												<asp:Label ID="Lbl_freqlb" runat="server" class="control-label"
													Text="Exam Frequency :" ForeColor="White"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                           
											<td>
												<asp:Label ID="Lbl_Frequency" runat="server" ForeColor="White" class="control-label"
													Text="Monthly"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td >&nbsp;</td>
                                            <td align="right" >
												<asp:Label ID="Label1" runat="server" ForeColor="White" Text="Class :" class="control-label"></asp:Label>
											</td>
                                            <td >&nbsp;</td>
                                            
											<td>
												<asp:DropDownList ID="DrpClassName" runat="server" Width="165px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpClassName_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
                                            <td align="right">
												<asp:Label ID="Label2" runat="server" ForeColor="White" Text="Period :" class="control-label"></asp:Label>
											</td>                                           
                                            <td >&nbsp;</td>
											<td>
												<asp:DropDownList ID="DrpExamPeriod" runat="server" Width="140px" class="form-control"
													AutoPostBack="True" onselectedindexchanged="DrpExamPeriod_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
											
										</tr>
									
										<tr>
					 <td class="leftside"><br /></td>
					 <td class="rightside"><br /></td>
					 </tr>
										
									</table>
								</asp:Panel>
				 
				 
				 </div>
		
				   
					
					
				  
		<table width="100%">
						 <tr>
						  <td align="right" style="width:50%;">
						   Select an Excel File
						  </td>
						  <td align="left">
							<asp:FileUpload ID="FileUpload_Excel" runat="server" Height="20px" />
						   
						  </td>
						 </tr>
						 <tr>
						  <td align="left">
						   
							  &nbsp;</td>
						   <td align="left">
						   
							  <asp:CheckBox ID="chk_removemarks" runat="server" Checked="false" 
								   Text="Clear Existing Mark" />
								  
 
							  </td>
						 </tr>
						 <tr>
						  <td align="right">
							   
						  </td>
						  <td align="left">
 
 
								&nbsp;
 
 
							   
 
								<asp:Button ID="Btn_UploadDetails" runat="server" Class="btn btn-success" 
									onclick="Btn_UploadDetails_Click" Text="Upload" OnClientClick="confirm('Are you sure you want to upload excel. If yes, please wait for uploading process to complete.')" />
 
							   &nbsp;   &nbsp;
							   
 
								<asp:Button ID="Btn_Template" runat="server" Text="Download Template" 
									Class="btn btn-primary" onclick="Btn_Template_Click" Width="170px" />
 
							   
 
						  </td>
						 </tr>
						 <tr>
						  <td align="center" colspan="2">
						   <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" ></asp:Label>
						  </td>
						 </tr>
						</table>
	
					
					
					
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>
   
   
   
	</ContentTemplate>
	<Triggers>

			 <asp:PostBackTrigger ControlID="Btn_UploadDetails" />
			 <asp:PostBackTrigger ControlID="Btn_Template" />
   </Triggers>
	
 </asp:UpdatePanel>
   
</div>

<div class="clear"></div>
</div>
</asp:Content>
