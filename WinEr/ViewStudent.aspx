<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="ViewStudent"  Codebehind="ViewStudent.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">
<div id="right">

<div id="sidebar2">
<h2>Student Manager</h2>
<div id="StudentMenu" runat="server">

</div>

</div>
</div>

<div id="left">
<h1>View Student</h1>

<div>
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
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                        OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5" 
                        Width="525px">
                        <PagerSettings 
    Position="Bottom"  Mode="NumericFirstLast" 
     />

                    </asp:GridView>
                    <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btnLoadData" runat="server" Text="Fill GridView" OnClick="btnLoadData_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

</div>

<div class="clear"></div>
</div>
</asp:Content>

