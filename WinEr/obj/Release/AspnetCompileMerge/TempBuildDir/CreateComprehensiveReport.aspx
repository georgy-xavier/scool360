<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="CreateComprehensiveReport.aspx.cs" Inherits="WinEr.CreateComprehensiveReport" %>
<%@ Register TagPrefix="WC" TagName="COMPREHENSIVEREPORT" Src="WebControls/ComprehensiveReportControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
 
              <WC:COMPREHENSIVEREPORT id="WC_Comprehensicereport" runat="server" />  
       
    <div class="clear"></div>
</div>
</asp:Content>
