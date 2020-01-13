<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="TCConfig.aspx.cs" Inherits="WinEr.TCConfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">
    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr></table></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            
                <div class="container skin1" >
		            <table cellpadding="0" cellspacing="0" class="containerTable">
			            <tr >
				            <td class="no"> </td>
				            <td class="n">Configure TC</td>
				            <td class="ne"> </td>
			            </tr>
			            <tr >
			            
				            <td class="o"> </td>
				            <td class="c" >
                                <asp:Label ID="Lbl_Err" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
				            <br />
				            <div>
				                <div style="float:left;width:650px;">
				                    <HTMLEditor:Editor ID="Editor_TCFormat"   runat="server"  Height="500px" Width="100%" />
				                </div> 
				                <div style="float:right;width:280px; " align="center">
				                    <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Representations of keywords"></asp:Label>
				                    <br />
				                    <div id="Sepretor" runat="server"  style="height:480px;overflow:auto; border:solid 1px #a0a0a0"  >
    				                
				                    </div>
                                    
				                </div>
				             </div>				               
				             
				             <div align="right" >
                                 <asp:Button ID="Btn_Update" runat="server" Text="Update" class="btn btn-info" Onclick="Btn_Update_Click"/>
				             </div>
				             <div align="center">
				                <asp:Label ID="Lbl_UpdationError" runat="server" class="control-label" ForeColor="Red"></asp:Label>
				             </div>
				                
				                
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

    <div class="clear"></div>
</div>
</asp:Content>
