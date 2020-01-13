<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeeBillWithLogoAndHeader.aspx.cs" Inherits="WinEr.FeeBillWithLogoAndHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <style type="text/css">

        .bordercls
        {
            border: 1px solid #000000;
        }
        </style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
  
    <asp:Panel ID="Pnl_Header" runat="server">
    <center>
    <table style="margin: inherit; padding: inherit; color: black; width: 600px; border: thin solid #000000;" align="center">    
    <tr>
    <td>
    <table width="100%">
    <tr>
    <td>
      <asp:Image ID="Img_Header" runat="server" Height="80px" Width="100%" ImageUrl="~/ThumbnailImages/schoolImage.jpg" />
    </td>    
    </tr>
    <tr>
    <td align="center"><h5>FEE RECEIPT</h5></td>
    </tr>
     </table>
     
             <table  width="100%">
                            <tr>
                                <td align="left" >
                                    <asp:Label ID="LblStudName" runat="server" Text="Student Name" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="LblName" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="LblRollNo" runat="server" Text="Roll Number" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblRno" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" >
                                    <asp:Label ID="LblAdNo" runat="server" Text="Admission Number" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="LblAdminno" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="lblClass" runat="server" Text="Class" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="LblCls" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" >
                                    <asp:Label ID="LblDate" runat="server" Text="Paid Date" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="lblpDate" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                                <td  align="left">
                                    <asp:Label ID="Lbl_Bill" runat="server" Font-Bold="True" Text="Bill Id"></asp:Label>
                                </td>
                                <td align="left">
                        
                                    <asp:Label ID="Lbl_BillId" runat="server"></asp:Label>
                        
                                </td>
                           </tr>
                            <tr>
                            <td align="left" class="style5">
                                    <asp:Label ID="Lbl_Staff" runat="server" Text="Collected Staff" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" class="style6">
                                    <asp:Label ID="Lbl_StaffName" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                           </tr>
                           <tr>
                                <td  align="left">
                                        <asp:Label ID="Lbl_BankName" runat="server" Text="BankName" Font-Bold="True"></asp:Label>
                                </td>
                                <td  align="left">
                                    <asp:Label ID="Lbl_BankNameData" runat="server"></asp:Label>
                                </td>
                                <td  align="left">
                                     <asp:Label ID="Lbl_DDNo" runat="server" Font-Bold="True" Text="DD/Cheque No"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Lbl_DDNoData" runat="server"></asp:Label>
                                </td>
                          </tr>
                     </table>
     
     <%--<table width="100%">
     
     <tr>
     <td align="left"><asp:Label ID="Lbl_No" runat="server" Text="<b>No:</b>"></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_NoValue" runat="server" Text=""></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_Date" runat="server" Text="<b>Date</b>"></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_DateValue" runat="server" Text=""></asp:Label></td>
     </tr>
      <tr>
     <td align="left"><asp:Label ID="Lbl_StudentName" runat="server" Text="<b>Name Of Student</b>"></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_StudentNameValue" runat="server" Text=""></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_Class" runat="server" Text="<b>Class</b>"></asp:Label></td>
       <td align="left"><asp:Label ID="Lbl_ClassValue" runat="server" Text=""></asp:Label></td>
     </tr>
     </table>--%>
     
     
       <div id="FeeDetails" runat="server">
    
    <%-- <table width="100%" style="border-collapse:collapse;"   align="center">
     <tr>
     <td class="bordercls" align="center" ><b>Particulars</b></td>
     <td class="bordercls" align="center"><b>Amount(SR)</b></td>
     </tr>
       <tr >
     <td class="bordercls" align="left">Registration Fee</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td  class="bordercls" align="left">Admission Fee</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Tution Fee</td>
     <td  class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls"  align="left">Term Fee(1/11/111)</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Extra Curricular Fee</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Transportation Fee</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Book</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Uniform</td>
     <td class="bordercls" align="center"></td>
     </tr>       
       <tr>
     <td class="bordercls" align="left">Accessories</td>
     <td class="bordercls" align="center"></td>
     </tr>
       <tr>
     <td class="bordercls" align="left">Others(..................)</td>
     <td class="bordercls" align="center"></td>
     </tr>
      <tr>
     <td class="bordercls" align="right"><b>Total</b></td>
     <td class="bordercls" align="center"><asp:Label ID="LblAmt" runat="server" Text=""></asp:Label></td>
     </tr>
     <tr>
     <td class="bordercls" colspan="2" width="100%">In Words:...........................</td>
     </tr>
     </table>  --%>
     
     </div>
     
     <table width="100%">
     <tr>
     <td align="right"><asp:Label ID="Lbl_Accountant" runat="server" Text="Accountant"></asp:Label></td>
     </tr>
       <tr>
     <td width="100%">
      <asp:Image ID="Img_Footer" runat="server" Height="80px" Width="100%" ImageUrl="~/ThumbnailImages/winceron.jpg" />
     </td>
     </tr>
     </table>
     
     </td></tr>
    </table>
    </center>
    </asp:Panel> 
    </div>
    </form>
</body>
</html>
