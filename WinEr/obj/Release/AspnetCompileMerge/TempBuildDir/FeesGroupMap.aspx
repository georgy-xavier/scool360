<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="FeesGroupMap.aspx.cs" Inherits="WinEr.FeesGroupMap" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
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
                <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>
                       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/add_page.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">Fees Group Header Map</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                    <asp:Panel ID="Pnl_Header_Map" runat="server">
                       <table class="tablelist">
                       <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>
                      <tr><td class="leftside">
                      Select Fees Group Header 
                      </td>
                      <td class="rightside">
                          <asp:DropDownList ID="Drplist_Header" runat="server" Width="200px">
                          </asp:DropDownList>                                 
                      </td>
                      </tr>
                        <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>
                             <tr><td class="leftside">
                      Select Fees To Map 
                      </td>
                      <td class="rightside">
                          <asp:DropDownList ID="Drp_Fees" runat="server" Width="200px">
                          </asp:DropDownList>                                 
                      </td>
                      </tr>
                       <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                  &nbsp;</td>
                          </tr>
                            <tr>
                              <td class="leftside">
                                  &nbsp;</td>
                              <td class="rightside">
                                <asp:Button ID="Btn_Map" runat="server" Text="Map" CssClass="grayok"
                                      onclick="Btn_Map_Click"  />
                             </td>
                          </tr>
                          
                      </table>
                    </asp:Panel>
                            <br />
                <center>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="No Data Exists!" ForeColor="Orange"></asp:Label>
                <asp:Panel ID="Pnl_Grid_Fees" runat="server">
                <asp:GridView runat="server" ID="Grd_Fees" AutoGenerateColumns="false" BackColor="#EBEBEB"
                     BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px" Width="100%" 
                     onpageindexchanging="Grd_Fees_PageIndexChanging" 
                     AllowPaging="true"
                     PageSize="5" 
                     OnRowCommand="Grd_Fees_RowCommand" >
			      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <Columns>   
                  <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="360px" />              
                  <asp:BoundField DataField="Name" HeaderText="Fees Header"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>  
                  <asp:BoundField DataField="AccountName" HeaderText="Fees Name"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                                 <asp:buttonfield buttontype="Link"  commandname="Remove"  text="&lt;img src='Pics/DeleteRed.png' width='40px' border=0 title='Select to View'&gt;"
                                 ItemStyle-ForeColor="Black"  HeaderText="UnMap"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>

                  </Columns>                  
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
			    </asp:GridView>
                 </asp:Panel>
                  </center>
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
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
                </ContentTemplate>
                </asp:UpdatePanel>
</asp:Content>
