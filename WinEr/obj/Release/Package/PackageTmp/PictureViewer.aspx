<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureViewer.aspx.cs" Inherits="WinEr.PictureViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN" "http://www.w3.org/TR/html4/frameset.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title>School</title>
    <script type="text/javascript">
function proceed(){
   opener.location.reload(true);
   self.close();
}
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <table width="100%" cellspacing="10">
     <tr>
      <td align="right">
        
          <asp:Button ID="Btn_Back" runat="server" Text="Back" Width="100px"  OnClientClick="proceed()" />
        
      </td>
     </tr>
     <tr>
      <td>
        
      <iframe id="yourid" runat="server"  width="100%" height="500px" scrolling="auto" frameborder="1" > </iframe>
      
      </td>
     </tr>
    </table>
   

    

     
    </div>
    </form>
</body>
</html>
