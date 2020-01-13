<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="True" CodeBehind="ServiceFeedBack.aspx.cs" Inherits="WinErParentLogin.ServiceFeedBack" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"><script type="text/javascript">


    window.onload = function WindowLoad(event) {
    $("#composemailarea").hide();
    $("#composenew").hide();
    
    };
    
    

    function showthread() {
        $("#composemailarea").show();
        $("#composenew").hide();
    }
    function closethread() {
        $("#composemailarea").slideToggle("slow");
        $("#composenew").hide();
    }
    function showthreaddirect() {
        $("#composemailarea").show();
        $("#composenew").hide();
    }
    
    function shownewmessagearea() {
        $("#composenew").slideToggle("slow");
        $("#composemailarea").hide();
        
    }
    function hidenewmessagearea() {
        $("#composenew").slideToggle("slow");
        $("#composemailarea").hide();
    }
    function showdirectnewmessage() {
        $("#composenew").show();
        $("#composemailarea").hide();
    } 

      </script>
<style type="text/css">
          
      .mailarea
     {
        width:450px;
        float:right;
        height:500px;
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
     .newmailarea
       {
        width:450px;
        float:right;
        height:250px;
        background-color:#FFF8DC; 
	    border:2px solid silver;
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
        z-index:1200;
       
   
     }
      </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
     
       <ContentTemplate>
<div>

  
   <input type="button" value="Compose" class="btn btn-primary" onclick="return shownewmessagearea()" />

  <div class="container skin1" style="width:850px;">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/email-icon.gif" 
                        Height="29px" Width="29px" /></td>
                <td class="n">
                    Feed Backs
                        </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   <div style="min-height:300px;">
<asp:GridView ID="GrdMessage"  runat="server" AutoGenerateColumns="False" 
        AllowPaging="true"
        Width="100%" PageSize="25" 
        onpageindexchanging="GrdMessage_PageIndexChanging" 
        onselectedindexchanged="GrdMessage_SelectedIndexChanged" 
         OnRowDataBound="GrdMessage_RowDataBound" >
        <PagerSettings Position="TopAndBottom" />
        
        <Columns>
                
      <asp:BoundField DataField="ThreadId" />                       

      <asp:BoundField DataField="ServiceHeading" HeaderText="Subject"  ItemStyle-Width="100px" />
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
</div>


<div id="composemailarea" class="mailarea" style="display:none;">
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
     Subject : <%#Eval("Heading")%>
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
     Subject : <asp:TextBox ID="txt_subj" runat="server" MaxLength="250" Width="400px" class="form-control"></asp:TextBox>
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
     
     Message: <asp:TextBox ID="txt_message" TextMode="MultiLine" MaxLength="500" Height="80px" Width="400px" runat="server" class="form-control"></asp:TextBox>
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
         <asp:Label ID="lblerror" runat="server" Text="fsdfsdf"></asp:Label></td>
     </tr>
     <tr>
     <td  align="center"  >
        <asp:Button ID="btn_msg" runat="server" class="btn btn-primary" Text="Send"  ValidationGroup="ValidSend" 
             onclick="btn_msg_Click"  />
        &nbsp;&nbsp;
        <input type="button" value="Close" class="btn btn-danger" onclick="return closethread()" />

     </td>
     </tr> 
     </table>     
    <asp:HiddenField ID="hdnthredid" runat="server" Value="0"/>
     </asp:Panel>

</div>


<div id="composenew" class="newmailarea" style="display:none;">
<asp:Panel Width="100%" runat="server" ID="Panel1" >
 
     
     <table width="100%" >
     <tr>
     <td align="left" valign="middle" >
     Subject : 
         <asp:DropDownList ID="drp_servicetype" runat="server" Width="350px" class="form-control">
         </asp:DropDownList>
     </td>
     </tr>
     <tr>
     <td align="left" valign="top" >
     
     Message: <asp:TextBox ID="txtnewmessage" TextMode="MultiLine" MaxLength="700" Height="100px" Width="350px" runat="server" class="form-control"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  
      ControlToValidate="txtnewmessage" ErrorMessage="*" ValidationGroup="Validnew" ></asp:RequiredFieldValidator>
     <ajaxToolkit:FilteredTextBoxExtender
                                           ID="FilteredTextBoxExtender4"
                                           runat="server"
                                           TargetControlID="txtnewmessage"
                                           FilterType="Custom"
                                           FilterMode="InvalidChars"
                                           InvalidChars="'\"
                                         />
     </td>
     </tr>
     <tr>
     <td >
         <asp:Label ID="lblerr" runat="server" ></asp:Label></td>
     </tr>
     <tr>
     <td  align="center"  >
        <asp:Button ID="btnsendnewmessage" runat="server" class="btn btn-primary" Text="Send"  ValidationGroup="Validnew"
             onclick="btnsendnewmessage_Click"  />
        &nbsp;&nbsp;
        <input type="button" value="Close" class="btn btn-danger" onclick="return hidenewmessagearea()" />

     </td>
     </tr> 
     </table>     

     </asp:Panel>

</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
