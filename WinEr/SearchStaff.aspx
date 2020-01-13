<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SearchStaff.aspx.cs" Inherits="WinEr.WebForm17" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .tablestyle
    {
       
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  

                    <div class="container skin1" style="width:565px">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Search History Staff</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				        <table >
                       
                    
                        <tr>
                            <td  colspan="2" 
                                style="text-align: center; background-color: #FFFFFF;">
                            <div id="newsearch" class="form-inline">
                                    
                                      <p>  <asp:TextBox ID="Txt_StaffName" runat="server" class="form-control" style="width:350px" ></asp:TextBox>
                                          <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_StaffName_TextBoxWatermarkExtender" 
                                              runat="server" Enabled="True" TargetControlID="Txt_StaffName" WatermarkText="Enter Staff Name">
                                          </ajaxToolkit:TextBoxWatermarkExtender>
                                          <ajaxToolkit:AutoCompleteExtender ID="Txt_StaffName_AutoCompleteExtender" 
                                              runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetStaffHistoryName" ServicePath="WinErWebService.asmx"  
                                               TargetControlID="Txt_StaffName" MinimumPrefixLength="1">
                                          </ajaxToolkit:AutoCompleteExtender>
                                       <ajaxToolkit:FilteredTextBoxExtender
                                               ID="Exam_nameFilteredTextBoxExtender1"
                                               runat="server"
                                               TargetControlID="Txt_StaffName"
                                               FilterType="Custom"
                                               FilterMode="InvalidChars"
                                               InvalidChars="'\"
                                             />
                                             
                                        <asp:Button ID="Btn_Search" runat="server"  Text="Search" Class="btn btn-primary"
                                             onclick="Btn_Search_Click" /></p>
                                    </div>
                            </td>
                        </tr>
                    </table> 
                                    <%----%>
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
	   <asp:Panel ID="Pnl_stafflist" runat="server">
	    <div class="TMOrange">
		<table class = "tablelist" >
		<tr><td class="topleft"></td><td class="topmiddle">Staff List</td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td>
		<td class="centermiddle"> 
		
		<asp:GridView ID="Grd_Staff" runat="server" 
                     AutoGenerateColumns="False" AllowPaging="True" 
                    AutoGenerateSelectButton="True" Width="100%"  OnRowDataBound="Grd_Staff_RowDataBound"
                    onselectedindexchanged="GridView1_SelectedIndexChanged" 
                    onpageindexchanging="Grd_Staff_PageIndexChanging" 
                           CellPadding="4" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="none" BorderWidth="1px">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Staff ID" />
                        <asp:TemplateField ItemStyle-Width="62px" ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:Image ID="Img_staffImage" runat="server" Width="60px" Height="70px" ImageUrl=""/>  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SurName" HeaderText="Staff Name" />
                          <asp:BoundField DataField="Designation" HeaderText="Designation" />
                        
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
	   
    
 </asp:Panel>
                            
  <WC:MSGBOX id="WC_MessageBox" runat="server" />       
                    

<div class="clear"></div>
</div>
</asp:Content>
