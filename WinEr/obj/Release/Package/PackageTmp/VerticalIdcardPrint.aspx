<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerticalIdcardPrint.aspx.cs" Inherits="WinEr.VerticalIdcardPrint" %>
 <style type="text/css">
    .table1
    {
        color: Black;
    } 
 </style>
  <div id="VerticalIDCard" runat="server">
             <asp:Panel ID="Panel6" runat="server">
                 <table class="table1">
                    <tr>
                        <td><asp:Panel ID="Panel7" runat="server" Height="330px" Width="210px" 
                                BorderColor="Black" BorderWidth="1px">
                             <table>
                                 <tr>
                                      <td align="center" style="background-color: #4CACE6">
                                          <asp:Image ID="Img_Logo1" runat="server" Height="61px" Width="58px" />
                                      </td>
                                      <td style="background-color: #4CACE6" align="center">
                                          <asp:Label ID="Lbl_Ver_SchoolName" runat="server" Font-Bold="True" 
                                              Font-Size="Medium" ForeColor="White"></asp:Label></td>
                                </tr>
                                <tr>
                                        <td colspan="2" align="center" style="background-color: #4CACE6">
                                            <asp:Label ID="Lbl_Ver_SchoolAddress" runat="server" ForeColor="White"></asp:Label>
                                        </td>
                               </tr>
                              
                                 <tr>
                                     <td align="center" colspan="2" valign="top">
                                         <asp:Image ID="Img_Student1" runat="server" Height="108px" />
                                     </td>
                                 </tr>
                                 <tr>
                                    <td>Name</td>
                                    <td>
                                        <asp:Label ID="Lbl_Ver_StudName" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         S/D of</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_Parent" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         DOB</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_DOB" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         Ad.No</td>
                                     <td>
                                         <asp:Label ID="Lbl_Ver_AdNo" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                            </table>
                        </asp:Panel></td>
                        <td><asp:Panel ID="Panel4" runat="server" Height="330px" Width="210px" Wrap="False" 
                                BorderColor="Black" BorderWidth="1px">
                                <table>
                                    <tr>
                                        <td></td> 
                                        <td></td>    
                                    </tr>
                                    <tr>
                                        <td>Blood Group</td>
                                        <td>
                                            <asp:Label ID="Lbl_Ver_BldGrp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address</td>
                                        <td>
                                            <asp:Label ID="Lbl_Ver_StdAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            This card is the property of
                                            <asp:Label ID="Lbl_Ver_SchoolName1" runat="server"></asp:Label>
                                            . In case the card is found, the finder may kindly return to the school address 
                                            mentioned</td>
                                        
                                    </tr>
                                </table>
                            </asp:Panel></td>
                    </tr>
                </table>
             </asp:Panel>
         </div>

