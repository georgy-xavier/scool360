<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="ViewExams"  Codebehind="ViewExams.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents" style="min-height:500px">

      

            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
            
            
      <asp:panel ID="Panel2" defaultbutton="Btn_Search" runat="server"> 
    
                        <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Exam</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<table >
					    <tr>
					       <td style="padding-left:100px" >Exam Name</td>
					        <td >
                                <asp:TextBox ID="Txt_ExamName" runat="server" class="form-control"> </asp:TextBox>
                                <ajaxToolkit:AutoCompleteExtender ID="Txt_ExamName_AutoCompleteExtender" 
                                runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetExamNameData" ServicePath="~/WinErWebService.asmx"  
                                TargetControlID="Txt_ExamName" UseContextKey="true"   MinimumPrefixLength="1" >
                               </ajaxToolkit:AutoCompleteExtender>
                            </td>
                            <td style="padding-left:200px">Select Exam Frequency</td>
                            <td >
                                <asp:DropDownList ID="Drp_Exam_Frequency" runat="server" Width="162px" class="form-control" 
                                     TabIndex="2">
                                </asp:DropDownList>
                             </td>
					    </tr>
					     <tr>
					        <td>&nbsp;</td>
					        <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
					    </tr>
					   
					   
					   
					    <tr>
					        
					        <td>&nbsp</td>
					        <td colspan="2">
                                <asp:Label ID="Lbl_Message" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                            </td>
					    </tr>
					    <tr>
                            <td colspan="2"> </td>
                               
                            <td align="center">
                                <asp:Button ID="Btn_Search" runat="server" onclick="Btn_Search_Click" 
                                    Text="Search" Class="btn btn-primary" TabIndex="6" />
                                <%--&nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                                    Text="Cancel" Class="btn btn-danger" TabIndex="7" />--%>
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
          
                   </asp:panel>       
            
            

<div class="container skin1" id="viewexamlist" runat="server">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
                     <img alt="" src="elements/chart-48x48.png" width="35" height="35" /></td>
				<td class="n">Exam List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<%--<div class="linestyle"></div> --%>






<div style="min-height:450px">








 <asp:Panel ID="Pnl_ExamList" runat="server" >
 
   
 
 <asp:GridView ID="Grd_ExamList" AllowPaging="True" AutoGenerateColumns="False" 
         onselectedindexchanged="Grd_ExamList_SelectedIndexChanged" OnRowDataBound ="Grd_ExamList_RowDataBound" 
                                              runat="server"  Width="100%" onpageindexchanging="Grd_ExamList_PageIndexChanging"
                                             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" PageSize="20"
                                               >
                                   
                                    <Columns>
                                        <asp:CommandField  SelectText="&lt;img src='Pics/bookgreen.png' width='30px' border=0 title='View Exam Details'&gt;"  
                            ShowSelectButton="True" >
                            <ItemStyle Width="35px" />
                            </asp:CommandField>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="ExamName" HeaderText="Exam Name"  />
                                        <asp:BoundField DataField="Name" HeaderText="Frequency" 
                                            ItemStyle-Width="100px" >
                                        <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sbject_type" HeaderText="Type" 
                                            ItemStyle-Width="200px" >
                                        
                                        </asp:BoundField>
                                    </Columns>
                                       <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:GridView>
                                
                                
                                
                                
                                
                                
                                 <asp:Label ID="Lbl_note" runat="server" Font-Bold="True" ForeColor="#FF3300" class="control-label"></asp:Label> 
</asp:Panel>
</div>
					
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

 <%--<div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;"> <img alt="" src="elements/chart-48x48.png" width="45" height="45" />
      </td>
	<td><h3>Exam List</h3></td>
	<td style="text-align:right;">
	
		 
         </td>
	</tr></table>
		


 </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>--%>    
		         
            
        
        <div class="clear">
        </div>
    </div>
</asp:Content>