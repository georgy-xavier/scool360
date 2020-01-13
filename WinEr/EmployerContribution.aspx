<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="EmployerContribution.aspx.cs" Inherits="WinEr.EmployerContribution" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate> 
<div id="contents" >
<div class="container skin1">
  <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">EMPLOYER CONTRIBUTION</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <table class=" tablelist">
                <tr>
                
                <td class="leftside" >
                    <asp:Label ID="Lbl_Config" runat="server" Text="Configuration Name"></asp:Label>
                   
                   
                </td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Config" runat="server"></asp:TextBox>
                    <asp:Label ID="LblPf" runat="server" Text="(Write PF, to generate pf report)" Font-Bold="true"></asp:Label>
                
                 
                </td>
               
                </tr>
                
                <tr>
                <td class="leftside" >
                    <asp:Label ID="Lbl_Value" runat="server" Text="Value"></asp:Label>
                </td>
                <td class="rightside">
                    <asp:TextBox ID="Txt_Value" runat="server"></asp:TextBox>
                </td>
                </tr>
                   <tr>
                <td class="leftside" >
                    
                </td>
                <td class="rightside">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" Class="btn btn-info"
                        onclick="BtnSave_Click"/>
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
                     
 </div>
 <WC:MSGBOX id="WC_MessageBox" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
