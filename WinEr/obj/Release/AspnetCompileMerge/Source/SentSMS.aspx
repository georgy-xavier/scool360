<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SentSMS.aspx.cs" Inherits="WinEr.SentSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
       <title>WinEr</title>

    <link rel="shortcut icon" href="images/winerlogo.ico" />
    
  
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
     
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen,projection"/>
   <link rel='stylesheet' type='text/css' href='css files/winroundbox.css' title="style" media="screen"/>

  <link href="css files/Orangestyle.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="css files/Orangemenu.css" />
 <script type='text/javascript' src="js files/winermenuOrange.js"></script>
    <script language="JavaScript" type="text/javascript">

      
     var Page;

     var postBackElement;

     function pageLoad()

    {      

      Page = Sys.WebForms.PageRequestManager.getInstance();  

      Page.add_beginRequest(OnBeginRequest);

       Page.add_endRequest(endRequest);

    }

 

    function OnBeginRequest(sender, args)

    {       

      postBackElement = args.get_postBackElement();     

      postBackElement.disabled = true;

    }  

    

     function endRequest(sender, args)

    {      

      postBackElement.disabled = false;

  }
  
  function roll_over(img_name, img_src) {
      document[img_name].src = img_src;
  }

  </script>
   
   
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
    
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
           
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                
                
           <asp:Panel ID="PanelSent" runat="server"  Visible="false">

              <div style="margin:30px;padding:20px;border:solid 1px gray;">
                  <asp:Label ID="Lbl_Sent" runat="server" Text="dsds" ForeColor="#009900"></asp:Label>
              </div>

           </asp:Panel>  

       
           <asp:Panel ID="Panel_Unsent" runat="server"  Visible="false">

              <div style="margin:30px;padding:20px;border:solid 1px gray;">
              
                <table width="100%">
                 <tr>
                  <td >
                   <asp:Label ID="Lbl_Unsent" runat="server" Text="dsds" ForeColor="#d50000"></asp:Label>
                  </td>

                 </tr>
                 <tr>
                  <td align="center">
                  
                      <asp:Button ID="Btn_SendtoRemaining" runat="server" Text="Send SMS to UnSent Numbers" />
                        &nbsp;
                        <asp:Button ID="Btn_SentSMSAll" runat="server" Text="Send SMS to All" />
                  
                  </td>

                 </tr>
                </table>
                  
              </div>

           </asp:Panel>  
	

          </ContentTemplate>
            </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
