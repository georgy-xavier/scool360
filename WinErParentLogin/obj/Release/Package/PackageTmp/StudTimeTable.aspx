<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="StudTimeTable.aspx.cs" Inherits="WinErParentLogin.StudTimeTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .MyTable
        {
            width:100%;
            
            border: thin solid #333333;
        }
        .MyDay
        {
            background-color: #97FFB1;
        }
        .DataCell
        {
           font-size:11px; 
        }
        
        .TxtBoxStyle
        {
            border:none;
        }
        
        .style1
        {
            height: 21px;
        }
        .style2
        {
            font-size: 11px;
            height: 21px;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="width:900px">
<div style="padding:10px 10px 10px 10px;">
<table width="100%">
 <tr>
  <td style="width:50%" align="left" valign="bottom">
  
      <asp:LinkButton ID="Lnk_Previous" runat="server" onclick="Lnk_Previous_Click">Previous Week</asp:LinkButton>
  
  </td>
  <td style="width:50%" align="right" valign="bottom">
  
    <asp:LinkButton ID="Lnk_Next" runat="server" onclick="Lnk_Next_Click">Next Week</asp:LinkButton>
  <asp:ImageButton ID="Img_Export" runat="server"  ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="30px" Height="35px" OnClick="Img_Export_Click"/>
  </td>
 </tr>
 <tr>
   <td colspan="2">
 
     <div runat="server" id="TimetableDiv">

<table border="1" cellpadding="1" cellspacing="5" class="MyTable">
   <tr><th>DAYS/PERIODS</th><th>P1</th><th>P2</th><th>P3</th><th>P4</th><th>P5</th></tr>
   <tr style="background-color: #97FFB1;" ><td><b>MON , 17/1/2011</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>Class : Nursery B<br>Subject : Maths</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr>
   <tr><td><b>TUE , 18/1/2011<b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>Class : Nursery B<br>Subject : Maths</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr>
   <tr><td><b>WED , 19/1/2011<b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr>
   <tr><td><b>THU , 20/1/2011<b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr>
   <tr><td><b>FRI , 21/1/2011<b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr>
   <tr><td><b>SAT , 22/1/2011<b></td><td><b>FREE</b></td><td><b>FREE</b></td><td><b>FREE</b></td><td><b>FREE</b></td><td class="style2"><b>FREE</b></td></tr>
   <tr><td><b>SUN , 23/1/2011<b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td><td class="DataCell"><b>FREE</b></td></tr></table>
 
 </div>
 <div id="DivBottom_Color" runat="server">
<table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px" width="100%">
                       <tr>
                         <td style="background-color:#c6ffc6;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left">
                             Today</td>

                         <td style="background-color:#ffcc00;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Holiday
                         </td>

                         <td style="background-color:#ffc1c1;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left" >
                            Not Batch Day
                         </td>
                         <td >
                            &nbsp;
                         </td>
                         <td >
                         
                         </td>
                       </tr>
                     </table>
 </div>
   </td>
 
 </tr>
</table>


 
 </div>
 <asp:HiddenField ID="Hd_Date" runat="server" />
</div>
</asp:Content>
