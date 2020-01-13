<%@ Page Language="C#" AutoEventWireup="True" Inherits="_LoginPage" Codebehind="LoginPage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Winer Login</title>
    <link rel="shortcut icon" href="images/winerlogo.ico" />
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
    <script type="text/javascript" src="js files/jquery.js"></script>
    <script type="text/javascript" src="js files/thickbox.js"></script>
    <link rel="stylesheet" href="css files/thickbox.css" type="text/css" media="screen" />
        <link rel="stylesheet" href="css files/NewLoginStyle.css" type="text/css" media="screen" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
         .leftstyle
        {
            text-align:right;
            color:Black;
            font-weight:bolder;
        }
        .Rightstyle
        {
           
            color:Red;
            font-weight:bolder;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <table width="100%">
    <tr>
     <td  class="leftpart" align="left" valign="top"> 
      <div class="topborder">
     <table width="100%">
     <tr>
     <td style="width:20%" align="right">
     <img src="images/winersmalllogo - Copy.png" />
      </td>
     <td  align="left">   STFRANCIS SCHOOL ICSE SCHOOL</td>
     </tr>
     </table>
      
    
     
           
    </div>
          <div class="blueline"></div>
          
          <br /><br />
          <div class="mainimages">
          <img src="#" width="100%"  height="250px;"/>
          </div>
          
          
     </td>
      <td style="width:367px" valign="top">
      <div class="rightpart" >
  
    <div class="newlogo">
    <img src="images/l_newlogo.jpg" alt="" />
    </div>
    <br />
    <br /><br />
  
      <div class="newlogin">
        
      
           <asp:Login ID="Client_Login" runat="server" Height="150px" 
                        onauthenticate="Client_Login_Authenticate" Width="275px" Font-Names="Arial" 
               Font-Size="11pt" >
            <LoginButtonStyle 
                Height="25px" Width="70px" />
               <LayoutTemplate>
                   <table border="0" cellpadding="1" cellspacing="0" 
                       style="border-collapse:collapse;">
                       <tr>
                           <td>
                               <table border="0" cellpadding="0" style="height:150px;width:275px;">
                                   <tr>
                                       <td align="left" colspan="2" style="font-weight:bold ;font-size:14px;">
                                           Log In</td>
                                   </tr>
                                   <tr>
                                       <td align="left" valign="top" style="font-size:12px;">
                                           <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name</asp:Label>
                                       </td>
                                       <td  valign="top">
                                           <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                               ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                               ToolTip="User Name is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="left" valign="top">
                                           <asp:Label ID="PasswordLabel" style="font-size:12px;" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                       </td>
                                       <td  valign="top">
                                           <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                               ControlToValidate="Password" ErrorMessage="Password is required." 
                                               ToolTip="Password is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                       </td>
                                   </tr>
                                 
                                   <tr>
                                       <td align="center" colspan="2" style="color:Red;">
                                           <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                       </td>
                                   </tr>
                                   <tr>
                               
                                       <td align="center" colspan="2">
                                     
                                           <asp:Button ID="LoginButton" runat="server" CommandName="Login" Height="25px" 
                                               Text="Log In" ValidationGroup="Client_Login" Width="70px" />
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                   </table>
               </LayoutTemplate>
            <LabelStyle HorizontalAlign="Left" VerticalAlign="Bottom"  />
               <TitleTextStyle 
                   HorizontalAlign="Left" />
        </asp:Login>
          <br />
            <asp:LinkButton ID="Lnk_Adminlogin" runat="server" 
                    onclick="Lnk_Adminlogin_Click" ToolTip="Admin Sign In" > &raquo; Administrator</asp:LinkButton>
           &nbsp;&nbsp;&nbsp;&nbsp;
             &nbsp;&nbsp;&nbsp;&nbsp;
               &nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;
      
            <asp:LinkButton
                ID="Lnk_Registeration" runat="server" onclick="Lnk_Registeration_Click">&raquo; Register License</asp:LinkButton>
                <br />

             <asp:LinkButton
                ID="Lnk_Lisencedetails" runat="server" onclick="Lnk_Lisencedetails_Click" >&raquo; License Details</asp:LinkButton> 
                 
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   &nbsp;&nbsp;&nbsp;&nbsp;
                     &nbsp;&nbsp;&nbsp;&nbsp;
             
             <a href="http://winer.in" title="Home">&raquo; Winer Home</a>     
          </div>
       
       <div class="footer">
       <a href="#">Winer.in</a><br />
       <div class="orangeline"></div>
       </div>
       
      </div>
      </td>
    </tr>
   
    </table>
    
    
    



<asp:Panel ID="Pnl_viewlicence" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_viewlicence" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_viewlicence" 
                                  runat="server" CancelControlID="Btn_viewlicenceOk" 
                                  PopupControlID="Pnl_viewlicenceArea" TargetControlID="Btn_viewlicence"  />
                          <asp:Panel ID="Pnl_viewlicenceArea" runat="server" style="display:none" >
                         <div class="container stiky" style="width:400px;"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">License Details</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <table class="style1">
                    <tr>
                        <td class="leftstyle">
                            Software:</td>
                        <td class="Rightstyle"> 
                            <asp:Label ID="LBl_software" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="leftstyle">
                            Version:</td>
                        <td class="Rightstyle">
                            <asp:Label ID="Lbl_Version" runat="server"></asp:Label>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td class="leftstyle">
                            Installion Date:</td>
                        <td class="Rightstyle">
                            <asp:Label ID="Lbl_installionDate" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="leftstyle">
                            &nbsp;User License:</td>
                        <td class="Rightstyle">
                            <asp:Label ID="Lbl_usercount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="leftstyle">
                            Expire Date:</td>
                        <td class="Rightstyle">
                            <asp:Label ID="Lbl_expiredate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="leftstyle">
                            Days Left:</td>
                        <td class="Rightstyle">
                            <asp:Label ID="Lbl_Dayesleft" runat="server"></asp:Label>
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
                            <asp:Button ID="Btn_viewlicenceOk" runat="server" Text="Ok" Width="70px" />
                            
                        </td>
                    </tr>
                </table>
                
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>

<asp:Panel ID="Pnl_Registeration" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_Registeration" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_Registeration" 
                                  runat="server" CancelControlID="Btn_RegCancel" 
                                  PopupControlID="Pnl_RegisterationArea" TargetControlID="Btn_Registeration"  />
                          <asp:Panel ID="Pnl_RegisterationArea" runat="server" style="display:none" >
                         <div class="container stiky" style="width:450px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/unlock.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Enter License</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" 
                                Text="System Key:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Lbl_getKey" runat="server"  Font-Bold="True" 
                                ForeColor="#CC3300" Text="gdfg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                Text="License Key:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_EnterKey" runat="server" Height="53px" Width="264px" 
                                TextMode="MultiLine"></asp:TextBox>
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
                        <asp:Button ID="Btn_Register" runat="server" Text="Register" Width="70px" 
                                onclick="Btn_Register_Click" />&nbsp;&nbsp;
                            <asp:Button ID="Btn_RegCancel" runat="server" Text="Cancel" Width="70px" />
                            
                            
                        </td>
                    </tr>
                </table>
                
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>

<asp:Panel ID="Pnl_UploadRegisteration" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_HdnUploadlice" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AddressLicense" 
                                  runat="server" CancelControlID="Btn_AddressRegCancel" 
                                  PopupControlID="Pnl_AddressLicenceArea" TargetControlID="Btn_HdnUploadlice"  />
                          <asp:Panel ID="Pnl_AddressLicenceArea" runat="server" style="display:none" >
                         <div class="container stiky" style="width:450px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/unlock.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Register License</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_DownLoad" runat="server" Font-Bold="True" ForeColor="Black" 
                                Text="System Key:"></asp:Label>
                        </td>
                        <td>
                            <asp:LinkButton ID="Lnk_DownloadKey" runat="server" 
                                onclick="Lnk_DownloadKey_Click">Download</asp:LinkButton>
                        
                            <asp:ImageButton ID="Img_DownloadLicenseKey" runat="server" 
                                ImageUrl="Pics/download.png" Height="30" Width="30" 
                                onclick="Img_DownloadLicenseKey_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Regkey" runat="server" Font-Bold="True" ForeColor="Black" 
                                Text="Registeration Key:"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUpload_License" runat="server" />
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
                           
                           <asp:Button ID="Btn_UpRegister" runat="server" Text="Register" Width="70px" 
                                onclick="Btn_UpRegister_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="Btn_AddressRegCancel" runat="server" Text="Cancel" Width="70px" />
                            
                            
                        </td>
                    </tr>
                </table>
                
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>

    <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Black"></asp:Label><br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
                        </div>
            </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>                 
                        </asp:Panel>


</form>
</body>
</html>
