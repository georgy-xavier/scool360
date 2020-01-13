<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adminlogin.aspx.cs" Inherits="WinErParentLogin.Adminlogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Admin Login</title>
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <br />
     <br />
      <br />
       <br />
       <center>
    <div>
    
        <asp:Login ID="Login1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" 
            BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
            Font-Size="0.8em" ForeColor="#333333" onauthenticate="Login1_Authenticate">
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
   
    </div>
    </center>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate>                               
            <div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr> <td align="center">  <b>Please Wait...</b><br /> <br /> <img src="images/indicator-big.gif" alt=""/></td> </tr> </table></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate>
            
            <asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_Msgbx" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_SelectStudent"  runat="server" CancelControlID="Btn_magok"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_Msgbx"  />
                          <asp:Panel ID="Pnl_msg" runat="server" DefaultButton="Btn_Select" ><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Select Student"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <br />
               <table  cellspacing="5" width="100%">
                 <tr>
                        <td class="leftside">Class Name</td>
                         <td class="rightside">
                                    <asp:DropDownList ID="Drp_Class" runat="server" Width="162px" AutoPostBack="true" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                         <td class="leftside">Student</td>
                         <td class="rightside">
                             <asp:DropDownList ID="Drp_Student" runat="server" Width="162px">
                             </asp:DropDownList>
                         </td>
                 </tr>
                 <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="Btn_Select" runat="server" Text="Select" Width="80px" OnClick="Btn_Select_Click"  CssClass="grayadd"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_magok" runat="server" Text="Cancel" Width="80px" CssClass="graycancel"/>
                    </td>
                 </tr>
               </table>
                       <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br />
                        
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
            </ContentTemplate>
            </asp:UpdatePanel>
     
    </form>
</body>
</html>
