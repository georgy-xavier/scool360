<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSLogg.aspx.cs" Inherits="WinEr.SMSLogg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
         
<asp:Panel ID="Panel1" runat="server" >

    
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">SMS Logs</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
		
		          <table width="100%" cellspacing="5">
		            <tr>
		             <td align="left">
                         <asp:LinkButton ID="Link_SelectAll" runat="server" 
                             onclick="Link_SelectAll_Click">Select All</asp:LinkButton>
		             </td>
		             <td align="right" >
                          
                          <asp:Button ID="Btn_Remove" runat="server" Text="Remove" 
                              onclick="Btn_Remove_Click" class="btn btn-info"/>
                          &nbsp;     
                          <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
                              onclick="Btn_Cancel_Click" class="btn btn-info"/>
		              </td>
		            </tr>
		            <tr>
		             <td colspan="2">
		                   <asp:GridView ID="Grd_SMSLogg" runat="server" 
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="20"
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                            GridLines="Vertical" onpageindexchanging="Grd_SMSLogg_PageIndexChanging">                         
                            <FooterStyle BackColor="#CCCC99" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                            <RowStyle BackColor="Transparent" />
                            <Columns>
                                 <asp:TemplateField HeaderText="Select"  ItemStyle-Width="20px">
                                <ItemTemplate   >
                                    <asp:CheckBox ID="Checksms" runat="server"  />
                                </ItemTemplate>
                         <ControlStyle ForeColor="#1AA4FF" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                         </asp:TemplateField>
                                <asp:BoundField DataField="Id" HeaderText="Id" />   
                                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo"  />  
                                <asp:BoundField DataField="Message" HeaderText="Message" ItemStyle-Width="500px" />     
                                <asp:BoundField DataField="ScheduledTime" HeaderText="Scheduled Time"/>                        
                                <asp:BoundField DataField="Status" HeaderText="Status"  />       
                               
                            </Columns>
                            <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
		             </td>
		            </tr>   

		          </table>
		<asp:Label ID="lbl_msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
					
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



          </ContentTemplate>
            </asp:UpdatePanel>
<div class="clear"></div>
</div>
</asp:Content>
