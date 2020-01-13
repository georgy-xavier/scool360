<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeeBillWithLogoAndHeader.aspx.cs" Inherits="WinEr.FeeBillWithLogoAndHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head runat="server">
    <title></title>
     <style type="text/css">

        .bordercls
        {
            border: 1px solid #000000;
        }
          .linestyle
        {
            width:100%;
            height:1px;
            margin-top:5px;
            margin-bottom:5px;
            background-color:Gray;
        }
        


        </style>
        
  



</head>
<body onload="window.print();" >
    <form id="form1" runat="server">
    <div>
  
    <asp:Panel ID="Pnl_Header" runat="server">
    <center>
    <table style="margin: inherit; padding: inherit; color: black; width:800px; border: 2px solid #000000;" align="center">    
    <tr>
    <td >
     <asp:Image ID="Img_Header" runat="server" Height="135px"  Width="888px"   />
  
      <br />
    </td>    
    </tr>
    
    <tr>
    
    <td style="border-bottom:solid 2px #000000" align="center"><b>FEE RECEIPT</b></td>
    </tr>
   
<tr>
<td>
<table width="100%">
    
<tr >
<td align="left" >
<asp:Label ID="LblStudName" runat="server" Text="Student Name:" Font-Bold="True"></asp:Label>
</td>
<td align="left" >
<asp:Label ID="LblName" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
</td>
<td  align="left">
<asp:Label ID="Lbl_Bill" runat="server" Font-Bold="True" Text="Bill Number:"></asp:Label>
</td>
<td align="left">

<asp:Label ID="Lbl_BillId" runat="server"></asp:Label>

</td>
<%--<td align="left" >
<asp:Label ID="LblRollNo" runat="server" Text="Roll Number" Font-Bold="True"></asp:Label>
</td>--%>
<%--<td align="left">
<asp:Label ID="lblRno" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
</td>--%>
</tr>


 <tr>
<td align="left" >
<asp:Label ID="LblAdNo" runat="server" Text="Admission Number:" Font-Bold="True"></asp:Label>
</td>
<td align="left" >
<asp:Label ID="LblAdminno" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
</td>
<td align="left" >
<asp:Label ID="LblDate" runat="server" Text="Bill Date:" Font-Bold="True"></asp:Label>
</td>
<td align="left" >
<asp:Label ID="lblpDate" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
</td>

</tr>
  
<tr>
<td align="left" >
<asp:Label ID="lblClass" runat="server" Text="Class:" Font-Bold="True"></asp:Label>
</td>
<td align="left">
<asp:Label ID="LblCls" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
</td>   
<td  align="left">
<asp:Label ID="Lbl_Batch" runat="server" Font-Bold="True" Text="Batch:"></asp:Label>
</td>
<td align="left">
<asp:Label ID="Lbl_BatchName" runat="server"></asp:Label>
</td>
</tr>
</table>
</td>
</tr>

 

<tr>
<td style="border-bottom:solid 2px #000000" align="center"></td>
</tr>

<tr>
<td>

<div id="FeeDetails" runat="server" style="width:100%;">  
        <%--<table width="100%"  style="border-collapse:collapse;"  align="center">
        <tr> 
        <td  align="center" style="border-right:1px solid #000000"  ><b>SL No</b>     </td>
        <td align="center" style="border-right:1px solid #000000"   ><b>Fee Particulars</b></td>
        <td  align="center"  style="border-right:1px solid #000000" ><b>Balance Amount</b></td>
        <td align="center"   ><b>Amount Paid</b></td>
        </tr>  
         <tr>
         <td style="border-bottom:solid 1px #000000" colspan="4"></td>
        </tr> 
        <tr>
        <td style="border-right: 1px solid #000000;" > 1 </td>
        <td  style="border-right: 1px solid #000000;padding-left:20px"  align="left">monthly fee(July:2012-2013 )</td>
        <td  style="border-right: 1px solid #000000;"  align="left">0</td>
        <td  align="center">1000</td>
        </tr>
        <tr>
        <td  style="border-right: 1px solid #000000;"  align="center"> </td>
        <td  style="border-right: 1px solid #000000;padding-left:20px"  align="center">monthly fee-Fine(July:2012-2013 )</td>
        <td  style="border-right: 1px solid #000000;"  align="center">0</td> <td   align="center" >243 </td>
        </tr> 
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center"> 2 </td> 
        <td  style="border-right: 1px solid #000000;padding-left:20px"  align="center">monthly fee(August:2012-2013 )</td>
        <td  style="border-right: 1px solid #000000;"  align="center">0</td><td  align="center">1000</td>
        </tr>
        <tr>
        <td  style="border-right: 1px solid #000000;"  align="center"> </td>
        <td  style="border-right: 1px solid #000000;"  align="center">monthly fee-Fine(August:2012-2013 )</td>
        <td  style="border-right: 1px solid #000000;"  align="center">0</td> <td   align="center" >243 </td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center"> 3 </td>
        <td  style="border-right: 1px solid #000000;"  align="center">monthly fee(September:2012-2013 )</td> 
        <td  style="border-right: 1px solid #000000;"  align="center">0</td><td  align="center">1000</td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td> 
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td> 
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  align="center">&nbsp;&nbsp;</td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp; &nbsp;</td> 
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  align="center">   </td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp; &nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  align="center">&nbsp;&nbsp;</td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp; &nbsp;</td> 
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  align="center">&nbsp;&nbsp;</td>
        </tr>
        <tr>
        <td style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  style="border-right: 1px solid #000000;"  align="center">&nbsp;&nbsp;</td>
        <td  align="center">&nbsp;&nbsp;</td>
        </tr>  
        <tr>
        <td style="border-bottom:solid 2px #000000" colspan="4"></td>
        </tr>
        <tr>
        <td   colspan="2"></td>
        <td  align="center" style="border-right:1px solid #000000"><b>Total:</td>
        <td   align="center"><b></b>3486</td>
        </tr>
        <tr>
        <td style="border-bottom:solid 1px #000000" colspan="4"></td>
        </tr>
        <tr>
        <td   colspan="2"></td>
        <td  align="center" style="border-right:1px solid #000000"><b>By Cash:</td>
        <td  align="center"><b>3486</b></td>
        </tr>
        <tr>
        <td style="border-bottom:solid 1px #000000" colspan="4"></td>
        </tr>
        <tr>
        <td  colspan="4" width="100%">In Words: THREE  THOUSAND  FOUR  HUNDRED  AND  EIGHTY  SIX  ONLY</td>
        </tr>
        <tr>
        <td style="border-bottom:solid 1px #000000" colspan="4"></td>
        </tr>
        <tr>
        <td   colspan="2" style="border-right:1px solid #000000"> Condition : <br />
        * This receipt is valid only with the seal and signature of the authorised person of the above institution.</td>
        <td   align="center" colspan="2" rowspan="2">Authorised Signature</td>
        </tr>
        <tr>
        <td style="border-bottom:solid 1px #000000" colspan="2"></td>
        </tr>
        <tr>
        <td  colspan="2"> Bill Issued By: admin &nbsp;&nbsp;&nbsp; Issued Date: 22/10/2012</td>
        </tr>
        <tr>
        <td style="border-bottom:solid 1px #000000" colspan="4"></td>
        </tr>
        </table>--%>

</div>
</td></tr>
        




<tr>
<td width="100%">
<asp:Image ID="Img_Footer" runat="server" Height="135px" Width="888px"  />
</td>
</tr>



       
        </table>
        </center>
        
<%--<tr>
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
                          </tr>--%>
     
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
        
<%--<tr>
                            <td align="left" class="style5">
                                    <asp:Label ID="Lbl_Staff" runat="server" Text="Collected Staff" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" class="style6">
                                    <asp:Label ID="Lbl_StaffName" runat="server" BackColor="White" ForeColor="Black"></asp:Label>
                                </td>
                                
                                
<%--<tr><td style="border-right: 1px solid #000000;"  align="center"> 2 </td> 
<td  style="border-right: 1px solid #000000;"  align="center">monthly fee(August:2012-2013 )</td>
<td  style="border-right: 1px solid #000000;"  align="center">0</td><td  align="center">1000</td>
</tr>--%>

<%--<tr><td  style="border-right: 1px solid #000000;"  align="center"> </td>
<td  style="border-right: 1px solid #000000;"  align="center">monthly fee-Fine(August:2012-2013 )</td>
<td  style="border-right: 1px solid #000000;"  align="center">0</td> <td   align="center" >243 </td>
</tr>--%> 

<%--<tr><td style="border-right: 1px solid #000000;"  align="center"> 3 </td> 
<td  style="border-right: 1px solid #000000;"  align="center">monthly fee(September:2012-2013 )</td> 
<td  style="border-right: 1px solid #000000;"  align="center">0</td><td  align="center">1000</td>
</tr>
                               
                           </tr>--%>
        
        </asp:Panel> 
        </div>
        </form>
</body>
</html>
