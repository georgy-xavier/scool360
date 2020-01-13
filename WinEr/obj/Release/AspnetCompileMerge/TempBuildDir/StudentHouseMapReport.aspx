<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentHouseMapReport.aspx.cs" Inherits="WinEr.StudentHouseMapReport" %>
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
            <tr >
                <td class="no"> </td>
                <td class="n">Student-House Map Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <asp:Panel ID="Pnl_TopArea" runat="server">
                <table width="100%" class="tablelist">
                <tr>
                <td class="leftside">Select House</td>
                <td class="rightside"><asp:DropDownList ID="Drp_House" runat="server" class="form-control" Width="173px"></asp:DropDownList></td>
                </tr>
                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                <tr>
                <td class="leftside">Select Class</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server"  class="form-control" Width="173px"></asp:DropDownList></td>
                </tr>
                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                   <tr>
                <td class="leftside">Select Gender</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Gender" runat="server" class="form-control" Width="173px">
                 <asp:ListItem Text="All" Value="0">
                 </asp:ListItem>
                <asp:ListItem Text="Male" Value="1">
                 </asp:ListItem>
                  <asp:ListItem Text="Female" Value="2">
                 </asp:ListItem>
                </asp:DropDownList></td>
                </tr>
                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                  <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Show" class="btn btn-primary" onclick="Btn_Show_Click" /></td>
                </tr>
                    <tr>
                <td colspan="2"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label></td>
                </tr>
                </table>
                </asp:Panel>
                
                   <asp:Panel ID="Pnl_Show" runat="server">
                <div class="linestyle"></div>
                <table width="100%">
                <tr>
                <td align="left">
                 <asp:Label ID="Lbl_total" runat="server" Text="" class="control-label"></asp:Label> 
                </td>
                <td align="right">  
               
            <%--    onpageindexchanging="Grd_Items_PageIndexChanging"--%>
                <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px" onclick="img_export_Excel_Click" ></asp:ImageButton></td>
                    </tr>
                <tr>
                <td colspan="2">
                            <asp:GridView ID="Grd_Report" runat="server" 
                                AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="15"   Width="100%" 
                     OnPageIndexChanging="Grd_Report_PageIndexChanging" >
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>
                      <asp:BoundField DataField="studid" HeaderText="Stud Id"/>
                      <asp:BoundField DataField="houseid" HeaderText="House Id"/>
                      <asp:BoundField DataField="StudentName" HeaderText="Student Name"/>
                      <asp:BoundField DataField="Sex" HeaderText="Gender" />
                      <asp:BoundField DataField="ClassName" HeaderText="Class Name"/>                      
                      <asp:BoundField DataField="HouseName" HeaderText="House Name"/>
                      <asp:BoundField DataField="Address" HeaderText="Address"/>
                      
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
</asp:Content>
