<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IdcardPrint.aspx.cs" Inherits="WinEr.IdcardPrint" %>

   
    <style type="text/css">
      .IdCard1
      {
           
         
      } 
      .Tdstyle
      {
          color:Black;
          background-color:White;
      }
      .TdInfo
      {
          border-color:Black;
          border-top-color:Black;
      }
    </style>
  
    
    <div id="IdCard" class="IdCard1">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <table>
    <tr>
    <td>
    <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderWidth="1px" 
            Height="225px" Width="345px">
    <table class="Tdstyle" align="center">
        <tr>
            <td style="background-color: #4CACE6" align="center">
            
                <asp:Image ID="Img_Logo" runat="server" Height="49px" Width="62px" />
            
            </td>
            <td style="background-color: #4CACE6" colspan="2">
                <asp:Label ID="Lbl_SchoolName" runat="server" Font-Bold="True" 
                    Font-Size="Medium" ForeColor="White"></asp:Label>
            </td>
        </tr>
        <tr>
        <td colspan="3" style="background-color: #4CACE6">
            <asp:Label ID="Lbl_Address" runat="server" Font-Bold="True" ForeColor="White" 
                Font-Size="Small"></asp:Label>
        </td>
        </tr>
        <tr>
            <td rowspan="5" valign="top">
                <asp:Image ID="Img_Student" runat="server" Height="95px" Width="107px" />
            </td>
            <td>
                Name</td>
            <td>
                <asp:Label ID="Lbl_StudentName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                S/D of</td>
            <td>
                <asp:Label ID="Lbl_Parent" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                DOB</td>
            <td>
                <asp:Label ID="Lbl_DOB" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Ad.No
            </td>
            <td>
                <asp:Label ID="Lbl_AdmissionNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
    </td>
    <td>
    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderWidth="1px" 
            Height="225px" Width="345px">
        <table class="Tdstyle">
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Blood Group</td>
                <td>
                    <asp:Label ID="Lbl_BloodGRp" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Address</td>              
                <td>
                    <asp:Label ID="Lbl_AddressBack" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="TdInfo" colspan="2">
                    This card is the property of &nbsp;<asp:Label ID="Lbl_School" runat="server"></asp:Label>
                    .&nbsp;In case the card is found, the founder may kindly return to the address 
                    mentioned. </td>
            </tr>
        </table>
    </asp:Panel>
    </td>
    </tr>
    
    </table>



            
            
