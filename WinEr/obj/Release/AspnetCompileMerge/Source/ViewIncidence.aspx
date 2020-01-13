<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewIncidence.aspx.cs" Inherits="WinEr.ViewIncidence" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
    <br /><br />
      <div style="text-align:right; padding-right:20px">
        <asp:Button ID="Btn_Cancel" runat="server"   Width="111px" Text="Close"  class="btn btn-danger"
            OnClientClick="javascript:window.close();" />
    </div>
    <br />
    <div id="Pupil"  runat="server" style="border: thin solid #3399FF">
    <table width="100%">
        <tr>
            <td style="width:auto" align="center">
                <asp:Image ID="Img_PupilImage" runat="server"  Width="90px" Height="90px"/>
            </td>
            <td>
            <div id="PupilData" runat="server" style="width:150;height:300;overflow:auto">
                <table width="100%">
                <tr>
                        <td></td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="background-color: #BAD0FC">
                            <asp:Label ID="Lbl_Name" runat="server" Text="Pupil name"></asp:Label></td>
                        <td style="background-color: #BAD0FC">
                            <asp:Label ID="Lbl_PupilName" runat="server" Text=""></asp:Label></td>
                        
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="Lbl_SexName" runat="server" Text="Sex"></asp:Label>
                        <asp:Label ID="Lbl_Standard" runat="server" Text="Standard"></asp:Label>
                    </td>
                        <td>
                            <asp:Label ID="Lbl_Sex" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="background-color: #C1D3FB">
                            <asp:Label ID="Lbl_Age1" runat="server" Text="Age"></asp:Label>
                            <asp:Label ID="Lbl_Teacher" runat="server" Text="Class Teacher"></asp:Label>
                        </td>
                        <td style="background-color: #C1D3FB">
                            <asp:Label ID="Lbl_Age" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                    
                </table>
            </div>
            </td>
        </tr>
    </table>
    </div>
    <div id ="Incident" runat="server" style="border: thin solid #3399FF">
                <h4> <span style="color:#366092;"> <asp:Label runat="server" ID="lbl_incHead"></asp:Label> </span> </h4> 
        
    <table width="100%" border="0">

        <tr>
            <td style="background-color: #C3D5FD">Incident Date</td>
            <td style="background-color: #C3D5FD">
                <asp:Label ID="Lbl_Date" runat="server" Text=""></asp:Label></td>
            <td style="background-color: #C3D5FD"  align="right">Incident Reported By</td>
            <td style="background-color: #C3D5FD" align="center"><asp:Label ID="Lbl_RportedBy" runat="server" Text="" ></asp:Label></td>           
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>
                &nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>           
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <div id="IncDesc" runat="server">
                </div>
            </td>
        </tr>
    </table>
    
    </div>
  
    </form>
</body>
</html>
