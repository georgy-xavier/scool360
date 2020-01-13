<%@ Page Language="C#" AutoEventWireup="True"   CodeBehind="MessageThread.aspx.cs" Inherits="WinEr.MessageThread" %>
<%--<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Parent Login</title>
    <%--<link href="css files/wschoolstyle.css" rel="stylesheet" type="text/css" />--%>
    <link rel="shortcut icon" href="images/winerlogo.ico" />
    
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen,projection"/>
   <link rel='stylesheet' type='text/css' href='css files/winroundbox.css' title="style" media="screen"/>

        <script language="javascript" type="text/javascript">
            function pagereload(_msg) {
                if (_msg != '') {
                    alert(_msg);
                }
            window.opener.location.reload();
        }
        function CloseWindow() {
            window.close();
        }

        </script>

<style type="text/css">
 .MessageRow
 {
     border-bottom:solid 1px Red;
     margin:10px;
 }
</style>
    
</head><body>
    <form id="form1" runat="server">
  
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
<%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
 
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
     <asp:Panel Width="100%" runat="server" ID="pnlMain" >
     <table class="tablelist" >
     <tr>
     <td >
     <div style="max-height:400px;overflow:auto"   >
     <asp:GridView ID="grdThreads" EnableTheming="false" Width="100%" runat="server" 
             AutoGenerateColumns="false"   >
             
     <Columns>
     <asp:BoundField DataField="Id"  />
     <asp:BoundField DataField="FromUserId"  />
     <asp:BoundField DataField="FromUSerType"  />
     <asp:TemplateField>
     <ItemTemplate>
     <div class="MessageRow">
     <table width="100%" >
     
     <tr >
     
     <td style="width:50%" align="left" >
     From : <%#Eval("Name")%>
     </td>
     <td align="left" >
     <%#Eval("Date")%>
     </td>
     </tr>
     <tr>
     <td colspan="2" align="left" runat="server"  id="cellsub" >     
     <hr />
     Subject : <%#Eval("Subject")%>
     <hr />
     </td>
     </tr>
     <tr>
     <td colspan="2" align="left" >    
     <div style="max-width:350px;min-height:80px;overflow:auto">
     <%#Eval("Description")%>
     </div> 
     
     </td>
     </tr>
     
     
     </table>
     </div>
     </ItemTemplate>
     </asp:TemplateField>
     </Columns>
     </asp:GridView>
     </div>
     </td>
     </tr>
     
     <tr>
     <td colspan="2" style="background-color:LightBlue" >
     <table width="100%" >
     <tr>
     <td align="center" valign="middle" >
     Subject : <asp:TextBox ID="txt_subj" runat="server" MaxLength="250" Width="450px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="rqd_subj" runat="server"  
      ControlToValidate="txt_subj" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
       <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender1"
                                           runat="server"
                                           TargetControlID="txt_subj"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
     </td>
     </tr>
     <tr>
     <td align="center" valign="top" >
     
     Message: <asp:TextBox ID="txt_message" TextMode="MultiLine" MaxLength="500" Height="80px" Width="450px" runat="server" ></asp:TextBox>
     <asp:RequiredFieldValidator ID="rqd_msg" runat="server"  
      ControlToValidate="txt_message" ErrorMessage="*" ValidationGroup="ValidSend" ></asp:RequiredFieldValidator>
     <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender2"
                                           runat="server"
                                           TargetControlID="txt_message"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
     </td>
     </tr>
     <tr>
     <td  align="center"  >
        <asp:Button ID="btn_msg" runat="server" CssClass="grayempty" Width="100px" Text="Send"  ValidationGroup="ValidSend" 
             onclick="btn_msg_Click"  />
        &nbsp;&nbsp;
        <asp:Button ID="btn_cncl" runat="server" CssClass="grayempty" Width="100px" Text="Cancel" OnClick="CancelClick"  />
     </td>
     </tr> 
     </table>     
     </td>
     </tr>
     </table>
     </asp:Panel>
<%--
            <WC:MSGBOX ID="MSGBOX" runat="server" />--%>
</ContentTemplate>
</asp:UpdatePanel>

    </form>
                   

</body>
</html>
