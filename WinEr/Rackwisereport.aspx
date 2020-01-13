<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="Rackwisereport.aspx.cs" Inherits="WinEr.Rackwisereport" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .searchmanagement {
            height: 150px;
            overflow: scroll;
        }

        .BookDetails {
            min-height: 250px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate>
                <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr>
                            <td class="no"></td>
                            <td class="n">Rack Wise Book Report</td>
                            <td class="ne"></td>
                        </tr>
                        <tr>
                            <td class="o"></td>
                            <td class="c">
                                <asp:Panel ID="Pnl_SelectPart" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <div class="container skin1" style="width: 500px">
                                                    <table cellpadding="0" cellspacing="0" class="containerTable">


                                                        <div class="form-group">
                                                            <label for="Txt_MsgContent">Select Rack:</label>
                                                            <asp:DropDownList ID="Drp_Rack" runat="server" Width="150px" class="form-control"></asp:DropDownList>
                                                            <asp:LinkButton ID="Btn_ShowReport" CssClass="btn btn-primary" runat="server" OnClick="Btn_ShowReport_Click"><span class="glyphicon glyphicon-search">&nbsp;Show</span></asp:LinkButton>
                                                        </div>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Lbl_CategoryErr" Text="" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                                 </td>
                        </tr>
                         </table>
                </div>
                                
                
                        <asp:Panel ID="Pnl_display" runat="server">
                            <div class="text-right">
                        <contenttemplate>          
                          <%--  <asp:ImageButton ID="img_export_Excel"  runat="server" data-placement="top" data-toggle="tooltip" title="Export serch result to excel"  ImageUrl="~/Pics/Excel.png"
                                Width="30px" OnClick="img_export_Excel_Click">
                            </asp:ImageButton>&nbsp;&nbsp;--%>
                           <asp:ImageButton ID="img_export_Excel" runat="server"  ImageUrl="~/Pics/Excel.png"
			 OnClick="img_export_Excel_Click" Width="40px" ToolTip="Export to Excel" />
                        </contenttemplate>
                                </div>

                            <div style=" overflow:auto; height: 392px;">
		<asp:GridView ID="GrdVew_Books" runat="server" CellPadding="4" ForeColor="Black" 
			GridLines="Vertical" Width="97%" AutoGenerateColumns="False" 
			BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
			<Columns>
				<asp:BoundField DataField="Id" HeaderText="Id" />
				
				  <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                    <asp:BoundField DataField="Author" HeaderText="Author" />
                                    <asp:BoundField DataField="Publisher" HeaderText="Publisher" />
                                    <asp:BoundField DataField="Year" HeaderText="Year" />
                                    <asp:BoundField DataField="RackName" HeaderText="RackName" />
			   
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
	
                            <%--<asp:GridView ID="GrdVew_Books" runat="server"

                                AutoGenerateColumns="False" AllowPaging="true" PageSize="20"
                                ForeColor="Black" GridLines="Vertical"
                                BackColor="#EBEBEB" Width="100%"
                                BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                                RowStyle-BorderColor="#BFBFBF"
                                RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px"
                                CellPadding="3" CellSpacing="2" Font-Size="12px"
                                OnPageIndexChanging="GrdVew_Books_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                    <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                    <asp:BoundField DataField="Author" HeaderText="Author" />
                                    <asp:BoundField DataField="Publisher" HeaderText="Publisher" />
                                    <asp:BoundField DataField="Year" HeaderText="Year" />
                                    <asp:BoundField DataField="RackName" HeaderText="RackName" />

                                </Columns>
                                <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                                <EditRowStyle Font-Size="Medium" />
                                <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                    HorizontalAlign="Left" />
                                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                    Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:GridView>--%>
                        </asp:Panel>

              <%--  </td>--%>
                        <td class="e"></td>
               <%-- </tr>--%>
                    <tr>
                        <td class="so"></td>
                        <td class="s"></td>
                        <td class="se"></td>
                    </tr>
                </table>
                </div>
                <WC:MSGBOX ID="WC_MessageBox" runat="server" />
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="img_export_Excel" />
                    
            </Triggers>
        </asp:UpdatePanel>
        <div class="clear"></div>
    </div>
</asp:Content>

