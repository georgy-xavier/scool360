<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="ExamTimeTable.aspx.cs" Inherits="WinErParentLogin.ExamTimeTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
    <tr>
        <td align="right"> Select Exam</td>
        <td align="left">
        <div class="form-inline">
            <asp:DropDownList ID="drpExam" runat="server" class="form-control" >
            </asp:DropDownList>
            <asp:Button ID="btn_Show" runat="server" Text="Show" onclick="btn_Show_Click"  class="btn btn-primary"/>
            </div>
        </td>
    </tr>
</table>
<asp:GridView ID="Grd_ExamSchdule" runat="server" AutoGenerateColumns="False" 
          
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" Width="700" >
            <Columns>
                <asp:BoundField DataField="subject_name" HeaderText="Subject" />
                <asp:BoundField DataField="SubjectCode" HeaderText="Subject Code" ItemStyle-Width="100px" />
                <asp:BoundField DataField="ExamDate" HeaderText="Exam Date" ItemStyle-Width="75px" />
                <asp:BoundField DataField="StartTime" HeaderText="Start Time" ItemStyle-Width="75px" />
                <asp:BoundField DataField="EndTime" HeaderText="End Time" ItemStyle-Width="75px" />
                <asp:BoundField DataField="MinMark" HeaderText="Pass Mark" ItemStyle-Width="70px" />
                <asp:BoundField DataField="MaxMark" HeaderText="Max Mark" ItemStyle-Width="70px" />
            </Columns>
             <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
           </asp:GridView>
</asp:Content>
