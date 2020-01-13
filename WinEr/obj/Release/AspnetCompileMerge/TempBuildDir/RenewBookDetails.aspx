<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="RenewBookDetails.aspx.cs" Inherits="WinEr.RenewBookDetails" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

 <div id="left" style="width:100%" >
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate> 
         <asp:Panel ID="Pnl_Renewdetails"  runat="server" >
            <div class="container skin1"  >
                <table cellpadding="0" cellspacing="0" class="containerTable">
	                <tr>
		                <td class="no"><asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/Manage.png" 
                           Height="28px"  Width="29px" />  </td>
		                <td class="n">Collect Books </td>
		                <td class="ne"> </td>
	                </tr>
	                <tr >
		                <td class="o"> </td>
		                <td class="c" >
		                <table width="100%">
		                    <tr>
		                        <td align="right">Select Category &nbsp; </td>
		                       
		                         <td>
		                        <asp:DropDownList Id="Drp_SelCategory" runat="server" Width="150px">
		                        </asp:DropDownList> &nbsp; &nbsp;
		                            <asp:Button ID="Btn_Show" runat="server" Text="Show" Width="110px" CssClass="graysearch" OnClick="Btn_Show_Click" />
		                        </td>
		                    </tr>
		                    <tr>
		                    <td>
		                    <asp:Label Id="Lbl_Error" Text="" runat="server" ForeColor="Red"></asp:Label>
		                    </td>
		                    
		                    </tr>
		                </table>
		                
			                </td>
		                <td class="e"> </td>
	                </tr>
	                <tr>
		                <td class="so"> </td>
		                <td class="s"></td>
		                <td class="se"> </td>
	                </tr>
                </table>
            </div>
            <asp:Panel ID="Pnl_Details" runat="server">
            
            <div class="container skin1"  >
                <table cellpadding="0" cellspacing="0" class="containerTable">
	                <tr>
		                <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/Manage.png" 
                           Height="28px"  Width="29px" />  </td>
		                <td class="n">Issued Book details</td>
		                <td class="ne"> </td>
	                </tr>
	                <tr >
		                <td class="o"> </td>
		                <td class="c" >
		                
		                <asp:GridView  ID="Grd_Books" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4"   ForeColor="Black" GridLines="Vertical" Width="100%" >
                           
                           <Columns>
                              
                                <asp:BoundField DataField="BookNo" HeaderText="Book Id" />
                                <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                <asp:BoundField DataField="UserId" HeaderText="User Id" />
                                
                                <asp:BoundField DataField="UserType" HeaderText="User Type" />
                                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" />
                                <asp:BoundField DataField="Fine" HeaderText="Fine" />
                                
                                
                            </Columns>

                             <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                             <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                             <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
                             <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                             <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                             <EditRowStyle Font-Size="Medium" />    
                       </asp:GridView>
		                
		                </td>
		                <td class="e"> </td>
	                </tr>
	                <tr>
		                <td class="so"> </td>
		                <td class="s"></td>
		                <td class="se"> </td>
	                </tr>
                </table>
            </div>
            </asp:Panel>
         </asp:Panel>
          <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
    </ContentTemplate>
 </asp:UpdatePanel>
</div>

<div class="clear"></div>
</div>
</asp:Content>
