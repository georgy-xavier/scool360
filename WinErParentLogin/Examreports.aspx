<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="Examreports.aspx.cs" Inherits="WinErParentLogin.Examreports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate> <div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" > <tr> <td align="center"><b>Please Wait...</b><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table> </div></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate>        
        <div >
            <table width="100%">
				<tr align="right">
				<td >
                    <asp:ImageButton ID="Img_Search" runat="server" ImageUrl="~/Pics/Class.png" ImageAlign="AbsMiddle"  Width="30px" Height="30px"/>
                 <asp:LinkButton ID="Lnk_PreviousPerformance" runat="server" 
                        Text="Previous Performance" onclick="Lnk_PreviousPerformance_Click"></asp:LinkButton>
                        </td>
				</tr>
			</table>
			<asp:Label ID="Lbl_indexammsg" runat="server"></asp:Label>
			<asp:GridView ID="Grd_ExamList" runat="server" AllowPaging="True"  AutoGenerateColumns="False"
                CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                onpageindexchanging="Grd_ExamList_PageIndexChanging" OnRowDeleting="Grd_ExamList_RowDeleting" 
                onselectedindexchanged="Grd_ExamList_SelectedIndexChanged" 
                Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" >
   
                <Columns>
                    <asp:BoundField DataField="ExamSchId" HeaderText="Id" />
                    <asp:BoundField DataField="ExamSchId" HeaderText="Id" />
                    <asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
                    <asp:BoundField DataField="Period"  HeaderText="Period" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;img src='Pics/ViewPdf.png' width='30px' border=0 &gt;" HeaderText="Grade Report" />
                    <asp:CommandField ShowSelectButton="True" SelectText="&lt;img src='Pics/full_page.png' width='30px' border=0 &gt;"  HeaderText="Mark Report" />
              
                </Columns>
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
              <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
              <HeaderStyle BackColor="#E9E9E9"  Font-Bold="True" Font-Size="11px"  ForeColor="Black"   HorizontalAlign="Left" />
              <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        
               <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
               <EditRowStyle Font-Size="Medium" /> 
            </asp:GridView>
    
        </div>                    
    </ContentTemplate>
    </asp:UpdatePanel>
</div>

</asp:Content>
