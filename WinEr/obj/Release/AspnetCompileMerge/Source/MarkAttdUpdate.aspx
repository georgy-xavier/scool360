<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkAttdUpdate.aspx.cs" Inherits="WinEr.MarkAttdUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>WinEr</title>
   
   <script type="text/javascript">

         function Openerpagereload() {
             window.opener.window.location.reload();
         }



         function max() {
             window.moveTo(0, 0);
             window.resizeTo(screen.availWidth, screen.availHeight);
         }
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
  
      <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
    
     <ProgressTemplate>
                
                
                        <div id="progressBackgroundFilter">

                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td align="center">

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
    
</asp:UpdateProgress>
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate> 
    
    
    <table width="100%" cellspacing="10" style="border:solid 1px gray;">
     <tr>
      <td style="width:50%;" align="right">
       Student Name : 
      </td>
      <td style="width:50%;" align="left">
      
          <asp:Label ID="Lbl_StudentName" runat="server" Text="" Font-Bold="true"></asp:Label>
      
      </td>
     </tr>
     <tr>
      <td style="width:50%;" align="right">
       Class Name : 
      </td>
      <td style="width:50%;" align="left">
      
          <asp:Label ID="Lbl_ClassName" runat="server" Text="" Font-Bold="true"></asp:Label>
      
      </td>
     </tr>
          <tr>
      <td style="width:50%;" align="right">
       Date : 
      </td>
      <td style="width:50%;" align="left">
      
          <asp:Label ID="Lbl_Date" runat="server" Text="" Font-Bold="true"></asp:Label>
      
      </td>
     </tr>
     <tr>
      <td style="width:50%;" align="right">
       Present Status : 
      </td>
      <td style="width:50%;" align="left">
      
          <asp:DropDownList ID="Drp_PresentStatus" runat="server" Width="180px">
          <asp:ListItem Value="-1" Text="Not Marked"></asp:ListItem>
          <asp:ListItem Value="0" Text="Absent"></asp:ListItem>
          <asp:ListItem Value="1" Text="ForeNoon"></asp:ListItem>
          <asp:ListItem Value="2" Text="AfterNoon"></asp:ListItem>
          <asp:ListItem Value="3" Text="Full Day"></asp:ListItem>
          </asp:DropDownList>
      
      </td>
     </tr>
     <tr>
      <td colspan="2" align="center">
       
          <asp:Button ID="Btn_Update" runat="server" Text="Update" Width="80" 
              onclick="Btn_Update_Click" />
          
          &nbsp;&nbsp;
          
          <asp:Button ID="Btn_Close" runat="server" Text="Close" Width="80" 
              onclick="Btn_Close_Click" OnClientClick="window.close()" />
       
      </td>
     </tr>
    </table>
    <asp:HiddenField runat="server" ID="Hd_DateString" />
    <asp:HiddenField runat="server" ID="Hd_ClassId" />
    <asp:HiddenField runat="server" ID="Hd_PeriodId" />
     <asp:HiddenField runat="server" ID="Hd_HolidayStatus" />
     
      <WC:MSGBOX id="WC_MessageBox" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>
     
    </div>
    </form>
</body>
</html>
