<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Inherits="CreateSubjectType" Title="Create Exam Type" Codebehind="CreateExamType.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">




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
				<td class="n">Create Exam Type</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			
					<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" DefaultButton="BtnAddSubTyp" >
					
					<table class="tablelist">
					<tr>
					<td class="leftside">&nbsp;</td>
					<td class="rightside">
					    &nbsp;</td>
					</tr>
					    <tr>
                            <td class="leftside">
                                Enter Exam Type</td>
                            <td class="rightside">
                                <asp:TextBox ID="TxtSubTypeName" runat="server" MaxLength="30" Width="325px" class="form-control"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                    runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'/\" 
                                    TargetControlID="TxtSubTypeName" />
                            </td>
                        </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
					<tr>
					<td class="leftside">Enter Description</td>
					<td class="rightside" >
					 <asp:TextBox ID="TxtSubTypeDescription" runat="server" Height="40px" Width="325px" class="form-control" MaxLength="100"></asp:TextBox>
       <ajaxToolkit:FilteredTextBoxExtender
           ID="FilteredTextBoxExtender1"
           runat="server"
           TargetControlID="TxtSubTypeDescription"
           FilterType="Custom"
           FilterMode="InvalidChars"
          InvalidChars="'/\"
       />
					</td>
					</tr>
					    <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td class="rightside">
                                &nbsp;</td>
                        </tr>
					<tr>
					<td class="leftside"></td>
					<td class="rightside">
					 <asp:Button ID="BtnAddSubTyp" runat="server" Text="Create" Class="btn btn-success"
            onclick="BtnAddSubTyp_Click"  />
					</td>
					</tr>
					    <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td class="rightside">
                                &nbsp;</td>
                        </tr>
					</table>
       
<div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
       <img alt="" src="Pics/full_page.png" width="45" height="45" /></td>
	<td><h3>Exam Type List</h3></td>
	<td style="text-align:right;">
	
		 
         </td>
	</tr></table>
		
<div class="linestyle"></div> 

<div style=" overflow:auto;width: 100%;max-height: 300px;">
            <asp:GridView ID="GridSubjectType" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                OnRowDataBound="GridView1_RowDataBound" 
                OnRowDeleting="GridView1_RowDeleting" Width="97%"
                
                  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                
                
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="sbject_type" HeaderText="Exam Type"  />
                    <asp:BoundField DataField="TypeDisc" HeaderText="Exam Type Description" ItemStyle-Width="250px" />
                    <asp:TemplateField  ItemStyle-Width="40px">
                        <ItemTemplate>
                            <%--<asp:ImageButton ID="Img_ButtonDelete" runat="server" CommandArgument='<%# Eval("Id") %>' ImageUrl="Pics/DeleteRed.png" Height="30" Width="30" />--%>
                            <asp:LinkButton ID="LinkButton1" runat="server" 
                                 CommandName="Delete">Delete</asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle ForeColor="#FF3300" />
                    </asp:TemplateField>
                </Columns>
                
                  <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:GridView>
        </div>
    </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div> 
        
        
        
    </asp:Panel>
					
					
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
     
              <WC:MSGBOX id="WC_MessageBox" runat="server" />    
                    
                
    
    </ContentTemplate> 
    </asp:UpdatePanel> 
 
  <div class="clear"></div>
  </div>
  
</asp:Content>

