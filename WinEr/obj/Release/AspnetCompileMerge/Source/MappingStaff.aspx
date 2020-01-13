<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MappingStaff.aspx.cs" Inherits="WinEr.MappingStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px"  style="min-height:350px"; >
			<tr >
				<td class="no"></td>
				<td class="n">Map Teachers To Class</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                    <ContentTemplate>
                    <div style="min-height:200px";>
                        <table class="tablelist" >                        
                            <tr>                            
                            <td class="leftside">
                            <asp:Label ID="lbl_Subject" runat="server" Text="Subject" class="control-label" Font-Size="Small"></asp:Label>
                            </td>
                            <td class="rightside">                            
                            <asp:DropDownList ID="drp_subject" runat="server" Width="185px" class="form-control"
                                    onselectedindexchanged="drp_subject_SelectedIndexChanged" 
                                    AutoPostBack="True"></asp:DropDownList>
                            </td> 
                                                     
                            </tr>
                            <tr><td></td> <td align="left">
                         <asp:Label ID="lbl_err" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                        </td>  
                        </tr>
                        </table>
                        <div class="linestyle"></div>
                        <asp:Panel ID="pnlmapstaff" runat="server">                        
                        <table width="100%">                        
                        <tr align="right">                                               
                        <td align="right">
                        <asp:Button ID="btn_Save" runat="server" Text="Save" class="btn btn-success" onclick="btn_Save_Click" 
                                 />
                        </td>
                        </tr>
                        </table>
                        <table width="100%">
                        <tr>
                        <td>
                        <asp:GridView ID="Grd_MappingStaff" runat="server" CellPadding="4" 
                                    AutoGenerateColumns="False" Width="100%" 
                                onselectedindexchanged="Grd_MappingStaff_SelectedIndexChanged" >
                        <Columns>
                        <asp:BoundField DataField="ClassId" HeaderText="Class Id"  />
                        <asp:BoundField DataField="ClassName" HeaderText="Class Name"  />
                        <%--<asp:BoundField DataField="Staff1Id" HeaderText="Staff1Id" />
                        <asp:BoundField DataField="Staff2Id" HeaderText="Staff2Id" />--%>
                        <asp:TemplateField HeaderText="Staff 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                        <asp:DropDownList ID="drp_staff1" runat="server" Width="153px" AutoPostBack="True" class="form-control"
                                onselectedindexchanged="drp_staff1_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Staff 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                        <asp:DropDownList ID="drp_staff2" runat="server" Width="153px" AutoPostBack="True" class="form-control"
                                onselectedindexchanged="drp_staff2_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        </Columns>
                        
                        </asp:GridView>
                        </td>
                        </tr>
                        </table>
                         </asp:Panel>
                        </div>
                       
                    </ContentTemplate>
                    </asp:UpdatePanel>
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

<div class="clear"></div>
</div>
</asp:Content>
