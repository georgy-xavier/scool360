<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="True" Inherits="ViewStaffs"  Codebehind="ViewStaffs.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
        }
        .style3
        {
            width: 115px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">



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

 
     
    <asp:Panel ID="Panel1" defaultbutton="Btn_Search" runat="server">
    
	<table width="95%"><tr>
	<td>
	<div class="container skin1" style="width:588px" >
		<table cellpadding="0" cellspacing="0" class="containerTable" >
			<tr >
				<td class="no"> <img alt="" src="Pics/Staff/network_search.png" height="35" width="35" /> </td>
				<td class="n">View Staffs</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<table class="style1" >
        <tr>
            <td class="style3">
                Search by:</td>
            <td>
                <asp:DropDownList ID="Drp_SearchBy" runat="server" class="form-control" Height="34px" Width="250px" 
                    AutoPostBack="True" onselectedindexchanged="Drp_SearchBy_SelectedIndexChanged">
                    <asp:ListItem Value="0">StaffId /Login Name</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">Staff Name</asp:ListItem>
                    <asp:ListItem Value="2">Handled Subject</asp:ListItem>
                    <asp:ListItem Value="3">Designation</asp:ListItem>
                    <asp:ListItem Value="4">Experience Greater Than</asp:ListItem>
                    <asp:ListItem Value="5">Experience Less Than</asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
    </tr>
        <tr>
            <td class="style2" colspan="2" 
                style="text-align: center; background-color: #FFFFFF;">
            <div id="newsearch" class="form-inline">
              <p>  <asp:TextBox ID="Txt_Search1" runat="server" class="form-control" Width="50%"></asp:TextBox>
                  <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Search1_TextBoxWatermarkExtender" 
                      runat="server" Enabled="True" WatermarkText="All Staff" TargetControlID="Txt_Search1">
                  </ajaxToolkit:TextBoxWatermarkExtender>
                  <ajaxToolkit:AutoCompleteExtender ID="Txt_Search1_AutoCompleteExtender" 
                      runat="server" DelimiterCharacters="" Enabled="True" ServicePath="WinErWebService.asmx"  UseContextKey="true"
                      TargetControlID="Txt_Search1" ServiceMethod="GetStaffName"  MinimumPrefixLength="1">
                  </ajaxToolkit:AutoCompleteExtender>
                
                  <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_Search1" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                  
                <asp:Button ID="Btn_Search" runat="server"  Text="Search" 
                     onclick="Btn_Search_Click" class="btn btn-primary" ToolTip="Search Staff"/></p>
            </div>
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
	</td>
         
	<td align="right" valign="bottom">
	
          <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" ImageUrl="~/Pics/Excel.png" Height="47px" 
                  Width="42px" onclick="img_export_Excel_Click"></asp:ImageButton>
	</td>
	</tr></table>
	   
     </asp:Panel> 
    

      <asp:Panel ID="Pnl_stafflist" runat="server" >
      <div class="TMOrange">
		<table class = "tablelist" >
		<tr><td class="topleft"></td><td class="topmiddle">Staff List</td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td>
		<td class="centermiddle">   
      
      <asp:GridView ID="Grd_StaffList" runat="server" CellPadding="4" AutoGenerateColumns="False" 
        ForeColor="Black"  OnRowDataBound="Grd_StaffList_RowDataBound"
        GridLines="Vertical" Width="100%" AllowPaging="True" 
            onpageindexchanging="Grd_StaffList_PageIndexChanging" 
            onselectedindexchanged="Grd_StaffList_SelectedIndexChanged" AllowSorting="true"
            OnSorting="Grd_StaffList_Sorting"
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="none" BorderWidth="1px" >
        
        <Columns>
            <asp:CommandField  SelectText="&lt;img src='Pics/search_page.png' width='30px' border=0 title='View Staff Details'&gt;"  
                            ShowSelectButton="True" >
                            <ItemStyle Width="40px" />
                            </asp:CommandField>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="Img_staffImage" runat="server" Width="60px" Height="70px" ImageUrl=""/>  
                            </ItemTemplate>
                        </asp:TemplateField>
            <asp:BoundField DataField="SurName" HeaderText="SurName" SortExpression="SurName"/>
            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
            <asp:BoundField DataField="RoleName" HeaderText="RoleName" SortExpression="RoleName" />
         </Columns> 
         
         
        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:GridView>
      
      </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class="bottomright"></td></tr>
		</table>
		</div>
      <%--
      <div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Staff List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					
    
    
    
    

					
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%>
    
</asp:Panel>


     <WC:MSGBOX id="WC_MessageBox" runat="server" />    


<br/>


</ContentTemplate>

          <Triggers>
          <asp:PostBackTrigger ControlID="img_export_Excel" />
          </Triggers>
            </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>

