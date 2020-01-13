<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="True" CodeBehind="MessageHome.aspx.cs" Inherits="WinErParentLogin.MessageHome" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">
           function openIncpopup(strOpen) {
               open(strOpen, "Info", "status=1, width=700, scrollbars = 0,  height=600,resizable = 1");
           }
</script>
<script type="text/javascript">


    window.onload = function WindowLoad(event) {
    $("#composemailarea").hide();
    };
    
    

    function showmsgpopup() {
        $("#composemailarea").slideToggle("slow");
    }
    function closemsgpopup() {
        $("#composemailarea").slideToggle("slow");
    }
    function showstatus() {
        $("#composemailarea").show();
    }

      </script>
      <style type="text/css">
          
      .mailarea
     {
        width:450px;
        float:right;
        height:450px;
        background-color:#0fdedc; 
	    border:2px solid #337ab7;
        border-radius:10px;
        background-position:bottom;
	    position:fixed;
        right:0px;
        bottom:0px;
        font-size:14px;
        padding:10px;
        font-weight:bold;
        color:#834C24;
        cursor:pointer;
        margin-right:2px;
        z-index:1100;
       
   
     }
      </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
     
       <ContentTemplate>
       <div  style="width:300px"  ><table>
  <tr>
  <td><a class="topmenu" href="MessageHome.aspx">Inbox</a></td><td><a  class="topmenu" href="ComposeMessage.aspx">New Message</a></td>
  </tr>
  
  </table>
   
                 
  </div>
  
  
  
<table width="95%" ><%--style="border-top:#4a4a4a thin solid">--%>
 <tr>
  <td valign="top"> 
     <div class="container skin1" style="width:900px;">
        <table   cellpadding="0" cellspacing="0" class="containerTable" style="width:850px;">
            <tr>
                <td class="no">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/e-mail.png" 
                        Height="29px" Width="29px" /></td>
                <td class="n">
                    MESSAGES
                        </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   <div style="min-height:300px;">
                   
                   <table class="tablelist">
				<tr>
			
				<td valign="bottom" >
                    <asp:Label ID="Lbl_ApprovellistCount" runat="server" Text="No messages found" 
                        ForeColor="#FF9900"></asp:Label>
				</td>
				
				<td style="text-align:right;">
			
                    &nbsp;&nbsp;&nbsp;
                                             
				</td>
				</tr>
				</table>
				
			
                       <asp:Label ID="Lbl_Note" runat="server" Text=""></asp:Label>


					
	<asp:GridView ID="GrdMessage"  runat="server" AutoGenerateColumns="False" 
        AllowPaging="true"
        Width="100%" PageSize="25" 
        onpageindexchanging="GrdMessage_PageIndexChanging" 
        onselectedindexchanged="GrdMessage_SelectedIndexChanged" 
         OnRowDataBound="GrdMessage_RowDataBound" >
        <PagerSettings Position="TopAndBottom" />
        
        <Columns>
                
      <asp:BoundField DataField="ThreadId" />                       
      <asp:BoundField DataField="SurName" HeaderText="Staff" />
      <asp:BoundField DataField="Subject" HeaderText="Subject"  ItemStyle-Width="100px" />
      <asp:BoundField DataField="Description" HeaderText="Last Message" ItemStyle-Width="250px"/>             
      <asp:BoundField DataField="Date" HeaderText="Date" />
                   
       <asp:CommandField HeaderText="Detail View"  SelectText="&lt;img src='Pics/hand.png' width='25px' Hight='25px' border=0 title='Detail View'&gt;"  
                    ShowSelectButton="True" >
                   <ItemStyle Width="30px" />
                    </asp:CommandField>                      
                            
              
      </Columns>
      
    </asp:GridView>
                   
                   </div>
           
                   
                </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div> 
      
      </td>
  
 </tr>
 
 
</table>

<div id="composemailarea" class="mailarea"  style="display:none;">
<asp:Panel Width="100%" runat="server" ID="pnlMain" >
     
     <div style="height:250px;overflow:auto"   >
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
     
     <table width="100%" >
     <tr>
     <td align="left" valign="middle" >
     Subject : <asp:TextBox ID="txt_subj" runat="server" MaxLength="250" Width="400px"></asp:TextBox>
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
     <td align="left" valign="top" >
     
     Message: <asp:TextBox ID="txt_message" TextMode="MultiLine" MaxLength="500" Height="80px" Width="400px" runat="server" ></asp:TextBox>
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
     <td >
         <asp:Label ID="lblerror" runat="server" Text=""></asp:Label></td>
     </tr>
     <tr>
     <td  align="center"  >
        <asp:Button ID="btn_msg" runat="server" class="btn btn-primary" Text="Send"  ValidationGroup="ValidSend" 
             onclick="btn_msg_Click"  />
        &nbsp;&nbsp;
        <input type="button" value="Close" class="btn btn-danger" onclick="return closemsgpopup()" />

     </td>
     </tr> 
     </table>     
    <asp:HiddenField ID="hdnthredid" runat="server" Value="0"/>
     </asp:Panel>

</div>
<WC:MSGBOX ID="MSGBOX" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
