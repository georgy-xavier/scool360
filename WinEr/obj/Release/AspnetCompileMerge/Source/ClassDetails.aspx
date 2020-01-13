<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" Inherits="ClassDetails"  Codebehind="ClassDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style type="text/css">
		.Tdleft
		{
			text-align:right;
			color:Black;
		}
		.TdRight
		{
			text-align:left;
			color:Black;
			font-weight:bold;
		}
	</style>
	 <script language="javascript" type="text/javascript">
	 function openIncpopup(strOpen) {
		 open(strOpen, "Info", "status=1, width=600, height=450,resizable = 1");
	 }
	 function openIncedents(strOpen) {
		 open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
	 }
</script>
<style type="text/css">
		.IncBlock
		{
			height: 220px;
			
		}
		 .IncBlock a 
	   {
   
	 color: #546078; text-decoration: none;  
		 }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<div id="contents">
<div id="right">



<div class="label">Class Manager</div>
<div id="SubClassMenu" runat="server">
		
 </div>

</div>

<div id="left" style="text-align:left">

  
  
	
	<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><img alt="" src="Pics/Class.png" width="30" height="30" /> </td>
				<td class="n">Class Details</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			
					  <ajaxToolkit:TabContainer runat="server" ID="Tabs" Width="100%" 
						   CssClass="ajax__tab_yuitabview-theme" >
								   
										
										
										 <ajaxToolkit:TabPanel runat="server" ID="Tab_Details" HeaderText="Signature and Bio">
											<HeaderTemplate>
											 <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/full_page.png" /><b>DETAILS</b>
											</HeaderTemplate>
											<ContentTemplate>
												<asp:Panel ID="Panel1" runat="server" BorderColor="Black" >
   
		<table width="100%">
			<tr>
				<td class="Tdleft">
					Class Name&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
				  
					<asp:Label ID="Lbl_Class" runat="server" Text="Label"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft">
					&nbsp;Standard&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
					
						 <asp:Label ID="Lbl_Standard" runat="server" Text="Label"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft">
					Number of Students&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
				   
						<asp:Label ID="Lbl_NoStud" runat="server" Text="Label"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft">
					&nbsp;Class Teacher&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
					
						<asp:Label ID="Lbl_ClassTeacher" runat="server" Text="Not Assigned"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft">
					Number of Boys&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
					
						<asp:Label ID="Lbl_NoBoys" runat="server" Text="Label"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft">
					Number of Girls&nbsp;&nbsp;&nbsp;:
				</td>
				<td class="TdRight">
				  
						<asp:Label ID="Lbl_nogirls" runat="server" Text="Label"></asp:Label>   
				</td>
			</tr>
			<tr>
				<td class="Tdleft" valign="top">
					Subjects for this Class&nbsp;&nbsp;&nbsp;</td>
				<td class="TdRight">
					<asp:ListBox ID="LstSubjects" runat="server" 
						Height="182px" Width="172px" BackColor="White" ForeColor="Black" 
						TabIndex="6"></asp:ListBox>
				</td>
				
			</tr>
		   
		</table>
	   
	</asp:Panel>
											</ContentTemplate>
										</ajaxToolkit:TabPanel>
							 
										<ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Signature and Bio">
											<HeaderTemplate>
											 <asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/users11.png" /><b>STUDENTS LIST</b>
											</HeaderTemplate>
											<ContentTemplate>
												<asp:Panel ID="Pnl_studlist" runat="server">
				  
					<div id="export"  style=" text-align: right">
					
					
						<asp:ImageButton ID="Img_StudentPreview" runat="server"  
							ImageUrl="~/Pics/group.png" Width="40px" OnClick="Img_StudentPreview_Click" 
							ToolTip="View Student"/>
							
						&nbsp;<asp:ImageButton ID="img_editstud" runat="server"  
							ImageUrl="~/Pics/edit student.png" Width="40px" OnClick="Img_StudentEdit_Click" 
							ToolTip="Edit Student"/>
							

							
							
						&nbsp;<asp:ImageButton ID="Btn_Export" runat="server"  ImageUrl="~/Pics/Excel.png"
			 OnClick="Btn_Export_Click" Width="40px" ToolTip="Export to Excel" />
		 
						&nbsp;<asp:ImageButton ID="Img_GovtExport" runat="server"  ImageUrl="~/Pics/Excel-icon.png"
			  Width="40px" ToolTip="Export to Excel In Govt Format" onclick="Img_GovtExport_Click" />    
			 
						&nbsp;</div> 
	   <div class="newsubheading">Students List</div>
	   <div class="linestyle"></div>
				
	<div style=" overflow:auto; height: 392px;">
		<asp:GridView ID="Grd_Students" runat="server" CellPadding="4" ForeColor="Black" 
			GridLines="Vertical" Width="97%" AutoGenerateColumns="False" 
			BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
			<Columns>
				<asp:BoundField DataField="Id" HeaderText="Student Id" />
				
				<asp:TemplateField>
							<ItemTemplate>
								<asp:Image ID="Img_studImage" runat="server" Width="45px" Height="50px" />  
							</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Roll Number">
				   <ItemTemplate>
					  
					   <asp:Label  ID="Lbl_RollNumber" runat="server" Text="0"  Width="128"  ></asp:Label>   
				   </ItemTemplate>  
				</asp:TemplateField>
				<asp:BoundField DataField="StudentName" HeaderText="Student Name" />
				<asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" />  
				<asp:BoundField DataField="Sex" HeaderText="Sex" />      
			   
			 </Columns>  
			<AlternatingRowStyle BackColor="White" />
			<FooterStyle BackColor="#CCCC99" />
			<HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
				HorizontalAlign="Left" />
			<PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
			<RowStyle BackColor="#F7F7DE" />
			<SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
		</asp:GridView>
		</div>
		
		 </asp:Panel>
											</ContentTemplate>
										</ajaxToolkit:TabPanel>
										
										<%--<ajaxToolkit:TabPanel runat="server" ID="Tab_Incident" HeaderText="Signature and Bio">
											<HeaderTemplate>
											 <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>INCIDENTS</b>
											</HeaderTemplate>
											<ContentTemplate>
												<div id="ClassIncident" runat="server">
												</div>
											</ContentTemplate>
										</ajaxToolkit:TabPanel>--%>
										
					</ajaxToolkit:TabContainer>

					
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
	
	
	
   <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
								  runat="server" CancelControlID="Btn_magok" 
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
						  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
						 <div class="container skin5" style="width:400px; top:400px;left:200px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" 
						Height="28px" Width="29px" />
			 </td>
			<td class="n"><span style="color:White">alert!</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			   
				<asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
						</div>
			</td>
			<td class="e"> </td>
		</tr>
		<tr>
			<td class="so"> </td>
			<td class="s"> </td>
			<td class="se"> </td>
		</tr>
	</table>
	<br /><br />                                                
</div>
			 </asp:Panel> 
	   
		
   


</div>

<div class="clear"></div>

</div>

</asp:Content>

