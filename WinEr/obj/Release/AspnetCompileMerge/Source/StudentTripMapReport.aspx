<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StudentTripMapReport.aspx.cs" Inherits="WinEr.StudentTripMapReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            <tr>
                <td class="no"></td>
                <td class="n">Student Trip Map Report</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                
                <asp:Panel ID="Pnl_TopArea" runat="server">
                <table width="100%" class="tablelist">
                <tr>
                <td class="leftside">Class</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" Width="180px" class="form-control"></asp:DropDownList></td>
                
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                  <tr>
                <td class="leftside">Destination</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Destination" runat="server" Width="180px" class="form-control"></asp:DropDownList></td>
                
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Label ID="Lbl_Msg" runat="server" ForeColor="Red" Font-Bold="false"></asp:Label></td>
                
                </tr>
                   <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show" class="btn btn-primary"  OnClick="Btn_Show_Click" /></td>
                
                </tr>
                
                </table>
                </asp:Panel>
                
                 <asp:Panel ID="Pnl_Show" runat="server">
                <div class="linestyle"></div>
                <table width="100%">
                <tr>
                <td align="left">                
                </td>
                <td align="right">  
               
            <%--    onpageindexchanging="Grd_Items_PageIndexChanging"--%>
                <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px" onclick="img_export_Excel_Click" ></asp:ImageButton></td>
                    </tr>
                <tr>
                <td colspan="2">
                            <asp:GridView ID="Grd_TripStudentMapReport" runat="server" 
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="15"   Width="100%" 
                     OnPageIndexChanging="Grd_TripStudentMapReport_PageIndexChanging" >
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="StudentName" HeaderText="Student Name"/>                      
                      <asp:BoundField DataField="Destination" HeaderText="Destination"/>
                       <asp:BoundField DataField="FromtripName" HeaderText="From Trip"/>
                       <asp:BoundField DataField="ToTripName" HeaderText="To Trip"/>
                        
                      <asp:BoundField DataField="ToTripId" HeaderText="ToTripId"/>
                       <asp:BoundField DataField="FromTripId" HeaderText="FromTripId"/>
                       <asp:BoundField DataField="RouteName" HeaderText="Route"/>
                       
                      
                      
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
                </td>
                </tr>
                </table>
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
   <Triggers>
   <asp:PostBackTrigger ControlID="img_export_Excel" />
   </Triggers>
  </asp:UpdatePanel>
<div class="clear"></div>
</div>

              
</asp:Content>
