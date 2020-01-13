<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="TeachersFeedback.aspx.cs" Inherits="WinErParentLogin.TeachersFeedback" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="border:#4a4a4a 2px solid;height:auto; padding:10px;" >
 <table width="99%"><tr><td align="right">Select period</td><td align="left"><asp:DropDownList ID="drp_period" runat="server" Width="120"> 
     
     </asp:DropDownList>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnshow" runat="server" Text="Show" 
         onclick="btnshow_Click" Width="75" class="grayempty"   />
      </td></tr>
      <tr> 
      <td colspan="2" align="center">
          <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
      </td>
      </tr>
      </table>
    <table width="100%" runat="server" id="mainarea">
        <tr>
        <td colspan="3" align="center" > 
            <asp:Label ID="lbl_schoolName" runat="server" Text="" Font-Size="16px"  Font-Bold="true"></asp:Label></td>
        </tr>
        <tr>
        
        <td align="center" style="width:33%">Student Name : <asp:Label ID="lblstudent" runat="server" Text="" Font-Bold="true"></asp:Label></td>
        <td align="center"  style="width:33%">Class : <asp:Label ID="lbl_class" runat="server" Text=""  Font-Bold="true"></asp:Label></td>
          <td align="center"  style="width:33%">Month : <asp:Label ID="lbl_month" runat="server" Text=""  Font-Bold="true"></asp:Label></td>
        </tr>
            <tr>
        <td colspan="3" align="center" style="font-size:16px"> <br /><br />PERFORMANCE REPORT</td>
        </tr>
    </table>
    <br /> <br />
    <hr />
     <br />
   <div id="Performance" runat="server">
   </div><br /><br />
   <p style="text-align:right">
       Submitted by <asp:Label ID="lbl_staff" runat="server" Text=""></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </p>
    
 </div>
</asp:Content>
