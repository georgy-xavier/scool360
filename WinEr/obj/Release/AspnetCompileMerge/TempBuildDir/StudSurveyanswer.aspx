<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudSurveyanswer.aspx.cs" Inherits="WinEr.StudSurveyanswer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <br />
 <br />
    <br />
    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" Width="250px" Height="30px"></asp:DropDownList>
     <br />
    <br />

    <asp:GridView ID="Grd_StuSurvey" runat="server" Width="1500px" AutoGenerateColumns="False">

          <Columns>
                  
                  
                        <asp:BoundField DataField="ID" HeaderText="Id" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center"  />
                       <asp:BoundField DataField="GroupName" HeaderText=" Group Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="center"/>  
                        <asp:BoundField DataField="StudentName" HeaderText=" Student Name" ItemStyle-Width="200px" SortExpression="Title" ItemStyle-HorizontalAlign="center"/>  
                        <asp:BoundField DataField="Question" HeaderText=" Question" ItemStyle-Width="350px" SortExpression="Description" ItemStyle-HorizontalAlign="center"/>                   
                        <asp:BoundField DataField="ANSWER" HeaderText=" ANSWER" ItemStyle-Width="65px" SortExpression="Type" ItemStyle-HorizontalAlign="center"/>
                        
                    </Columns>


    </asp:GridView>

</asp:Content>
