<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="FeescheduleReport.aspx.cs" Inherits="WinEr.FeescheduleReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents"  style="min-width:950px;">
<div id="right">

<div id="sidebar2">
<h2>Fee Manager</h2>
<div id="FeeMenu" runat="server">

</div>

</div>

<div id="SubStudentMenu" runat="server">
		
 </div>
 <div id="ActionInfo" runat="server">
 
</div>
</div>

 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 
 <div id="left" >
    <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Fee Schedule Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <asp:Panel ID="panel1" runat="server" Height="200px">
                    <table>
                     <tr>
                            <td>
                                <br /></td>
                        </tr>
                        <tr>
                        <td style="width:200px"></td>
                        <td></td>
                            <td>
                                <asp:Label ID="lblclass" runat="server" Text="Select Class" Font-Bold="true" ></asp:Label></td>
                         <td>
                             <asp:DropDownList ID="Drp_Class" runat="server" Width="200px" 
                                 onselectedindexchanged="Drp_Class_SelectedIndexChanged">
                             </asp:DropDownList>
                         </td>
                         <td>  
                             <asp:ImageButton ID="Img_PdfExport" runat="server"    Width="45px" Height="45px" 
                   ImageUrl="~/Pics/ViewPdf.png" onclick="Img_PdfExport_Click" /></td>
                        </tr>
                        
                        <tr>
                        <td>
                            <asp:Label ID="lbl_Error" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label></td>
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
 </div>
 
 <div class="clear"></div>
</div>
</asp:Content>
