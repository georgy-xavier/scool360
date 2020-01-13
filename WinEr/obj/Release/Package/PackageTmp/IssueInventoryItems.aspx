<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="IssueInventoryItems.aspx.cs" Inherits="WinEr.IssueInventoryItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
    <style type="text/css">
        .style1
        {
            height: 32px;
        }
    </style>
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
       <div class="container skin1">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Issue Item
                </td>
                <td class="ne"> </td>
                </tr>
        <tr >
                <td class="o"> </td>
                <td>
                 <table width="800px" >
                <tr>
                <td class="style1">CustomerName</td>
                <td class="rightside" style="height: 32px; width: 237px;">
                    <asp:TextBox ID="txt_name" runat="server"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Name" ControlToValidate="txt_name"></asp:RequiredFieldValidator></td>
                
                <td class="style1">Item</td>
                <td class="rightside" style="height: 32px; width: 500px;">
                    <asp:DropDownList ID="Drp_item" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="Drp_item_SelectedIndexChanged">
                    </asp:DropDownList>
                    Stock: 
                    <asp:Label ID="lbl_stock" runat="server" Text=""></asp:Label>&nbsp;&nbsp; Cost: <asp:Label ID="lbl_cost" runat="server" Text=""></asp:Label>
                    &nbsp;</td>
                </tr>
                 <tr>
                <td class="style7">Address</td>
                <td class="rightside" style="height: 32px; width: 237px;"><asp:TextBox ID="txt_adres" runat="server"></asp:TextBox></td></td>
               
                <td class="style7">Count</td>
                <td class="rightside" style="height: 32px; width: 420px;"> 
                    <asp:TextBox ID="txt_itmcount" runat="server" AutoPostBack="True" 
                        ontextchanged="txt_itmcount_TextChanged" ></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Count" ControlToValidate="txt_itmcount"></asp:RequiredFieldValidator>&nbsp; TotalCost: <asp:Label ID="lbl_tcost" runat="server" Text=""></asp:Label> 
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_itmcount" >
                    </ajaxToolkit:FilteredTextBoxExtender></td>
                        </td>
                </tr> 
                
                 <tr>
                <td class="style10">Phone</td>
                
                <td class="rightside" style="height: 30px; width: 237px;"><asp:TextBox ID="txt_phone" runat="server"></asp:TextBox> <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_phone" >
                    </ajaxToolkit:FilteredTextBoxExtender></td>
              
                
                <td colspan="2" align="center" class="style9">
                        <asp:Button ID="btn_issue" runat="server" Text="Issue Item" 
                        onclick="btn_issue_Click" />
                        
                       </td>
                </tr>
                </table>
                
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
              
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
     </ContentTemplate>
</asp:UpdatePanel>         
</asp:Content>
