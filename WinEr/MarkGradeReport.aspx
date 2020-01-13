<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MarkGradeReport.aspx.cs" Inherits="WinEr.MarkGradeReport" %>
<%@ Register TagPrefix="WC" TagName="MARKGRIDREPORT" Src="WebControls/MarkGradeReportControl.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="contents">
 
              <WC:MARKGRIDREPORT id="WC_MarkGradeReport" runat="server" />  
       
    <div class="clear"></div>
</div>
</asp:Content>
