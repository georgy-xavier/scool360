<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" EnableTheming="false" AutoEventWireup="True" CodeBehind="SearchIncident.aspx.cs" Inherits="WinEr.SearchIncident" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
	function openIncpopup(strOpen) {
		open(strOpen, "Info", "status=1, width=600, height=450,resizable = 1");
	}
	function openIncedents(strOpen) {
		open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
	}
</script>
<style type="text/css">
 /*Header and Pager styles*/
 .PagerStyle /*Common Styles*/
{
	background-position:center;
	border-bottom:solid 1px #999;
}

.PagerStyle table
{
	text-align:center;
	margin:auto;
}
.PagerStyle table td
{
	border:0px;
	padding:5px;
}
.PagerStyle td
{
	border-top: #999999 1px solid;
}
.PagerStyle a
{
	color:#333;
	text-decoration:none;
	padding:2px 10px 2px 10px;
	border-top:solid 1px #fff;
	border-right:solid 1px #999;
	border-bottom:solid 1px #999;
	border-left:solid 1px #fff;
}
.PagerStyle span
{
	font-weight:bold;
	color:#333;
	text-decoration:none;
	padding:2px 10px 2px 10px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  
	<div id="contents" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
			</ajaxToolkit:ToolkitScriptManager>  
		  
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate> 
		  
		  <asp:Panel ID="Panel2" defaultbutton="Btn_Search" runat="server" > 
	<table width="90%" align="right"> 
	<tr>
	
	<td align="left" valign="bottom" style="width:50%;">
				<div class="container skin1 knsrchBox" style="width:100%;">
			<table width="100%" ><tr>
	<td width="50%">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Knowledge Search</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<table width="100%">
	  <%--  <tr>
			
			<td style="text-align: center; background-color: #FFFFFF;">
				<asp:RadioButton ID="rdio_stud" runat="server" Text="Student" Checked="true" GroupName="Type" />
				<asp:RadioButton ID="rdio_staf" runat="server" Text="Staff" GroupName="Type" />
				
			</td>
		</tr>--%>
		<tr>
			<td  
				style="text-align: center; background-color: #FFFFFF;">
			<div id="newsearch">
			  <p>  <asp:TextBox ID="Txt_Search" runat="server" Class="form-control" style="width:400px;margin-left: 9%;"></asp:TextBox></p>
				  <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender" 
					  runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetIncedents" ServicePath="WinErWebService.asmx"  
					  UseContextKey="true" TargetControlID="Txt_Search" MinimumPrefixLength="1">
				  </ajaxToolkit:AutoCompleteExtender>
			   <ajaxToolkit:FilteredTextBoxExtender
					   ID="IncedentFilteredTextBoxExtender1"
					   runat="server"
					   TargetControlID="Txt_Search"
					   FilterType="Custom"
					   FilterMode="InvalidChars"
					   InvalidChars="'\"  />
					 
			</div>
			</td>
		</tr>
		<tr>
		<td>
		<div id="Div1" runat="server">
		
		<ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
			HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
			ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40"  
			TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
		   <Panes>
			<ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
				<Header ><a href="" >Search Tips</a></Header>
				<Content>
				<table><tr><td align="left">
				1. Search by Name/Title/Type . <i>eg.Arun,Academic, Examination, Sports. etc.</i>
				<p>2. Search with Name + Keywords. <i>eg. John Examination, Tom Promotion. etc. </i></p>
				<p>3. Quick Search give results only if keyword in Incident Title/Type </i></p>
				<p>4. Deep Search includes search of keywords in Incident Description also </i></p>
				<p>* <i>Quick Search</i> will be more faster than <i>Deep Search</i></p>
				</td></tr></table>
				</Content>
			</ajaxToolkit:AccordionPane>
			</Panes>
			</ajaxToolkit:Accordion>
			</div>
		</td>
		</tr>
		<tr>
		<td style="text-align: center; background-color: #FFFFFF;">
			<asp:CheckBox ID="chk_history" runat="server" Text = "Include History" />
		</td>
		</tr>
		<tr>
		<td style="text-align: center; background-color: #FFFFFF;">
		 <asp:Button ID="Btn_Search" runat="server"  Text="Quick Search" Class="btn btn-primary" 
					 onclick="Btn_Search_Click" />
		 <asp:Button ID="Button1" runat="server"  Text="Deep Search" Class="btn btn-primary" 
					 onclick="Btn_DeepSearch_Click" />
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
		</td>
		
	</tr>
	</table>
	</div>      

	</td>
	
		<td align="left" valign="bottom" style="width:40%;">
			 <asp:ImageButton ID="img_rslt" runat="server" ImageUrl="~/images/indnt_srch.png" Height="47px" 
				  Width="42px" ></asp:ImageButton>
   &nbsp; 
   <asp:Label ID="lbl_srch_cnt" runat="server" Text=" 620 " ForeColor="DarkBlue" class="control-label"></asp:Label>
   <asp:Label ID="lbl_srch_ok" runat="server" Text=" Records Found...!" ForeColor="DarkOrange" class="control-label"></asp:Label>
			
		</td>
	
		<td align="right" valign="bottom" style="width:10%;">
		<asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
				  Width="42px" onclick="img_export_Excel_Click"></asp:ImageButton>
		
		</td>
	</tr>
	</table>
		  
		   </asp:Panel> 
		   
<%--           <asp:Panel ID="Pnl_Advanced" runat="server">
					   
						 <asp:Button runat="server" ID="Btn_Adv" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_AdvancedSearch" 
								  runat="server" CancelControlID="Btn_Cancel" 
								  PopupControlID="Pnl_AdvSearch" TargetControlID="Btn_Adv"  />
						  <asp:Panel ID="Pnl_AdvSearch" runat="server" >
						 <div class="container skin5" style="width:420px;" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> </td>
			<td class="n"><span style="color:White">Advanced Search</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			 
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
						</asp:Panel>
--%>          
		  
		<asp:Panel ID="Pnl_MessageBox" runat="server">
					   
						 <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
						 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
								  runat="server" CancelControlID="Btn_magok" 
								  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
						  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
						 <div class="container skin5" style="width:400px; top:400px;left:400px" >
	<table   cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
			<td class="no"> </td>
			<td class="n"><span style="color:White">Message</span></td>
			<td class="ne">&nbsp;</td>
		</tr>
		<tr >
			<td class="o"> </td>
			<td class="c" >
			   
				<asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
						<br /><br />
						<div style="text-align:center;">
							
							<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-primary"/>
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
						</asp:Panel>               
			
			
			 <%--<asp:Panel ID="Pnl_Searchresultq" runat="server">
			
			<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Incidents List</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"> </td>
				<td class="c" >
					
				
		 <br /> 
			 
			  
			 
				
			 <br />
		   
			 
		   
					
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
			
			 </asp:Panel>   --%>
&nbsp;<asp:Panel runat="server" Width="90%" 
		ID="Pnl_Searchresult" HorizontalAlign="Center">
<table class="tablelist">
<tr>
<!-- TMOrange -->
<td>
		 <div class="TMOrange">
		<table class = "tablelist" >
		<tr><td class="topleft"></td><td class="topmiddle">Incident List</td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td>
		<td class="centermiddle">    
				   <div Id="Description" runat="server"  >
				   
				   <asp:Label runat="server" ID="lbl_srch" Text="" ForeColor="DarkOrange" class="control-label"></asp:Label>
									   <asp:GridView ID="Grd_Incedent" runat="server" AllowPaging="True"
						CellPadding="4" ForeColor="Black"  AutoGenerateColumns="False" 
						OnPageIndexChanging="Grd_Incedent_PageIndexChanging" OnSelectedIndexChanged="Grd_Incedent_SelectedIndexChanged"
						Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
										   ShowHeader="False">
						<RowStyle BackColor="#F7F7DE" />
						<Columns>
							 <asp:BoundField DataField="Id" HeaderText="Id"  />
							 <asp:BoundField DataField="StudId" HeaderText="StudId"  />
							 <asp:BoundField DataField="Type" HeaderText="Type"  />
							  <asp:TemplateField>
		<ItemTemplate>
	<div id = "DivDetailsInGrid" runat="server"  style="width:100%;">
	<table width="100%" ><tr><td align="center">
	<table width="100%" >
	<tr>
	<td style="width:18%;">
	<table>
	<tr>
	<td align="center">
   <a href= '#'> <img src="images/stdnt.png" alt="No Pics" style="width:85px;height:105px;border-style:solid;border-width:1px;border-color:Black;" /> </a>
	</td>
	</tr>
	<tr>
	<td align="center">
	</td>
	</tr>
	<tr>
	<td align="center">
	 <a href= '~/item.aspx?sku=<%# Eval("StudId") %>'> More Incedents </a>
	</td>
	</tr>
	</table>
	</td>
	<td style="width:85%;" align="left" valign= "middle"  >
	<table>
	<tr>
	<td valign="top" >
	<a style="text-decoration:none;color:Red " href= '~/item.aspx?sku=<%# Eval("StudId") %>'><h5> Rahul R </h5> </a></td>
	
	<td valign="bottom" align="right" style="font-size:smaller;" >staff</td>
	</tr>
	<tr>
	<td  >
	<h5>Class Rank</h5>
	<div id="Div2" runat="server">Incident Date: 19/01/2011</div>
	</td>
	</tr>
	<tr>
	<td  >    
	<div id="Div3" class="linestyle" runat="server"></div>
	</td>
	</tr>
	<tr>
	<td >
	 Secured 3 marks out of 100 for the WS Exam.The result is Failed and the grade is F .Rank obtained is 0.
	</td>
	</tr>
	</table>
	</td>
	</tr>
	</table>
	</td></tr></table>
	</div>
		
		</ItemTemplate>
		</asp:TemplateField>
							<asp:TemplateField>
							  <ItemTemplate>
							<table width="100%"><tr><td style="width:100%;" align="center">
							  <%# Eval("Info") %>
							  </td></tr></table>
							</ItemTemplate>
							</asp:TemplateField>
							 <asp:TemplateField>
							  <ItemTemplate>
							<table width="100%"><tr><td style="width:100%;" align="center">
							  <%# Eval("Point") %>
							  </td></tr></table>
							</ItemTemplate>
							</asp:TemplateField>
							
						  <%--   <asp:TemplateField>
							  <ItemTemplate>
							<table width="100%"><tr><td style="width:100%;" align="center">
							  
							<table><tr><td><img alt = "sd" src="images/pt1_up.png" style="width:30px;height:30px;"/></td><td  style=" color:Green;">10</td></tr></table>
							<table><tr><td><img alt = "sd" src="images/pt1_dwn.png" style="width:30px;height:30px;"/></td><td  style=" color:Red;">10</td></tr></table>
							<table><tr><td><img alt = "sd" src="images/pt_up.gif" style="width:30px;height:30px;"/></td><td style=" color:Green;">10</td></tr></table>
							<table><tr><td><img alt = "sd" src="images/pt_dwn.gif" style="width:30px;height:30px;"/></td><td style=" color:Red;">10</td></tr></table>
							<table><tr><td><img alt = "sd" src="images/pt1_up.png" style="width:25px;height:25px;"/></td><td style=" color:Green;">10</td></tr></table>
							<table><tr><td><img alt = "sd" src="images/pt1_dwn.png" style="width:25px;height:25px;"/></td><td style=" color:Red;">10</td></tr></table>
							  </td></tr></table>
							</ItemTemplate>
							</asp:TemplateField>--%>
							
							 <asp:CommandField  SelectText="&lt;img src='Pics/search_page.png' width='30px' border=0 title='View Incident Details'&gt;"  
							ShowSelectButton="True" >
							<ItemStyle Width="40px" />
							</asp:CommandField>
						</Columns>
									<FooterStyle BackColor="#CCCC99"  />
									<PagerStyle BackColor="#F7F7DE" CssClass="PagerStyle"/>
									<HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
									<AlternatingRowStyle BackColor="White" />
					</asp:GridView>

					</div>
		</td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class="bottomright"></td></tr>
		</table>
		</div>
</td>
</tr>
</table>
</asp:Panel>
	  </ContentTemplate>
	  <Triggers>
	  <asp:PostBackTrigger  ControlID="img_export_Excel"/>
	  <%--<asp:AsyncPostBackTrigger ControlID="img_export_Excel" />--%>
	  </Triggers>
			</asp:UpdatePanel>

</div>
</asp:Content>
