<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeeBillNew.aspx.cs" Inherits="Scool360student.FeeBillNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">

        .style9
        {
            border: 1px solid #000000;
        }

        .style8
        {
            text-decoration: underline;
            font-weight: bold;
        }
        .tdStyle
        {
            border-style: none;
        }
       .FullBoarder
       {
            border: 1px solid #000000;
       }
    
        </style>
</head>
<body onload="window.print();">
    
    
    
    <form id="form1" runat="server">
    
    
    
    <table align="center" class="style9" width="100%">
        <tr>
            <td >
                <div style="">
    
                    <asp:Panel ID="Panel7" runat="server" BorderColor="Black" >
        
                        <table  width="100%" cellspacing="0">
                            <tr >
                                <td align="center" style="width:20%;">
                                    <asp:Image ID="Img_logo" runat="server" Height="80px" Width="80px" />
                                </td>
                                <td align= "center" style="width:65%;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label 
                                ID="Lbl_schoolname" runat="server" Font-Size="X-Large"></asp:Label>
                                <br/>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label 
                                        ID="Lbi_subHead" runat="server" Font-Size="Medium" 
                                ForeColor="#666666" Font-Bold="True"></asp:Label>
                                &nbsp;
                                </td>
                                 <td  style="width:15%;">&nbsp;</td>
                           </tr>
                        </table>
                        <div style="text-align:center">
                        <span class="style8">FEE BILL<br />
                        </span>
                        </div>
       
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" >
       
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
                                     <asp:Label ID="Lbl_No" runat="server" Font-Bold="True" Text=""></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Lbl_DDNoData" runat="server"></asp:Label>
                                </td>
                          </tr>
                     </table>
    
                </asp:Panel>
                <br />
                <div id="FeeDetails" runat="server">
                </div>
                <table width="100%" cellspacing="0" >
                    <tr>
                              
                        <td align="right" colspan="2" class="FullBoarder">
                        
                        &nbsp;
                            <asp:Label ID="Lbltotal" runat="server" Text="Total Amount" Font-Bold="True"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="center"  class="FullBoarder">
                            &nbsp;&nbsp;<asp:Label ID="LblAmt" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="bottom" style="padding-bottom:20px;">
                      
                      <td align="right" style="width:30%">
                        Total Amount in Words :
                      </td>
                      <td align="left">
                        
                          <asp:Label ID="lbl_amountWorlds" runat="server" Text="" Font-Bold="true"></asp:Label>
                      
                      </td>
                      <td class="FullBoarder" style="height:80px" align="center" valign="top" >
                         Accountant
                      </td>
                    </tr>
              </table>

            </div>
           </td>
        </tr>
    </table>
   
    </form>
   
</body>
</html>
