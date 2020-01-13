<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="HomeWork.aspx.cs" Inherits="WinErParentLogin.HomeWork" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
     
       <ContentTemplate>
        <div > 
       <br />
      <div id="InnerHtml" runat="server">
      </div>
       <br />
       </ContentTemplate>
       </asp:UpdatePanel>
</asp:Content>
