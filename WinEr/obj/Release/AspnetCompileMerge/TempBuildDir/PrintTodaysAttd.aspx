<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintTodaysAttd.aspx.cs" Inherits="WinEr.PrintTodaysAttd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
  .LeftBorder
  {

  }
  .RightBorder
  {

  }
    .ClassHeading
    {
     border:solid 1px black;
     background-color:Gray;
     color:White;
     font-weight:bolder;
     height:30px;
     width:50%;
    }
    
    .StudentRow
    {
      border:solid 1px black;
      font-weight:bolder;
      height:25px;
    }
 </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>
              
               <table width="800px" cellspacing="0">
                <tr>
                 <td class="LeftBorder" align="right" style="width:25%;">
                     Date :&nbsp;&nbsp; 
                 </td>
                 <td class="RightBorder" align="left" style="width:25%;">
                  
                     <asp:Label ID="lbl_Date" runat="server" Text="" Font-Bold="true"></asp:Label>
                 
                 </td>
                 <td  class="LeftBorder" align="right" style="width:25%;">
                     Time :&nbsp;&nbsp; 
                 </td>
                 <td class="RightBorder" align="left" style="width:25%;">
                   <asp:Label ID="lbl_Time" runat="server" Text="" Font-Bold="true"></asp:Label>
                 </td>
                </tr>
               </table>
              
              <div id="Div_Report" runat="server">
              
              </div>
              
              <%--<table width="800px">
               <tr>
                <td>
                 
                 <table width="100%" cellspacing="0">
                  <tr>
                      <td align="center" valign="middle" class="ClassHeading" >
                        ClassName : VII A
                      </td>
                      <td align="center" valign="middle" class="ClassHeading" >
                        Attendance : 42/45
                      </td>
                  </tr>
                  <tr>
                   <td align="left" colspan="2" class="StudentRow" style="font-size:14px;color:Black;text-decoration:underline;padding-left:10px;">
                        Absent List
                   </td>
                  </tr>
                  <tr>
                     <td align="center" valign="middle" class="StudentRow" >
                        1. Arun Sunny
                     </td>
                     <td align="center" valign="middle" class="StudentRow" >
                        2. Arun Thomas
                     </td>
                  </tr>
                   <tr>
                     <td align="center" valign="middle" class="StudentRow" >
                        3. Manju Nath
                     </td>
                     <td align="center" valign="middle" class="StudentRow" >
                      
                     </td>
                  </tr>
                  <tr>
                   <td colspan="2" class="StudentRow">
                   
                    
                    <table width="100%">
                     <tr>
                      <td align="right" style="height:30px">
                         
                          Verified &amp; Approved By&nbsp;&nbsp;
                         
                      </td>
                     </tr>
                     <tr>
                      <td  style="height:30px">
                      
                      </td>
                     </tr>
                     <tr>
                      <td  align="right" style="height:30px">
                      
                          Signature&nbsp;&nbsp;
                      </td>
                     </tr>
                    </table>
                   
                   
                   </td>
                  </tr>
                 </table>
                 
                </td>
               </tr>
              </table>--%>
             
             </center>
    </div>
    </form>
</body>
</html>
