<%@ Page Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="MessagePage.aspx.cs" Inherits="WinErParentLogin.MessagePage"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   <asp:UpdateProgress ID="UpdateProgress2" runat="server"  AssociatedUpdatePanelID="UpdatePanel1"><progresstemplate>
        <div id="progressBackgroundFilter"></div><div id="processMessage">
            <table style="height:100%;width:100%"><tr>
              <td align="center"><b>Please Wait...</b><br /><br />
                                  <img alt="" src="images/indicator-big.gif" /></td></tr></table></div>
       </progresstemplate>
   </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
      <div style="float:left;vertical-align:top;" >
          <asp:ImageButton ID="Imgbtn_ShowPanel" runat="server" 
              ImageUrl="~/Pics/mail1.png" Width="30px" Height="30px" 
              onclick="Imgbtn_ShowPanel_Click" />
          <asp:LinkButton ID="Lnk_ShowPAnel" runat="server" onclick="Lnk_ShowPAnel_Click">Send Message</asp:LinkButton>
      </div>
      <br />
      <br />
      <br />
      <div class="linestyle">                  
       </div>
      <asp:Panel ID="PanelSend" runat="server" Visible="false" >
      
               <table class="tablelist" cellspacing="5">
                                <tr>
                                    <td class="leftside">
                                        &nbsp;</td>
                                    <td class="rightside">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="leftside">
                                        Subject</td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_Subject" runat="server"  Height="20px" Width="400px" ></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="leftside" valign="top">
                                        Message</td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_Message" runat="server" TextMode="MultiLine"  Height="60px" Width="400px" ></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="leftside">
                                        &nbsp;</td>
                                    <td class="rightside">
                                        <asp:Label ID="LblFailureNotice" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="leftside">
                                        
                                        &nbsp;</td>
                                    <td class="rightside">
                    
                                        <asp:Button ID="Btn_SendMessage" runat="server" Text="Send" CssClass="grayok" 
                                            onclick="Btn_SendMessage_Click"  />
                                    &nbsp;
                                     <asp:Button ID="Btn_Clear" runat="server" Text="Clear" CssClass="graycancel" 
                                            onclick="Btn_Clear_Click"  />
                                    </td>
                                </tr>
                            </table>
             
            
      </asp:Panel>
      
      
      <asp:GridView ID="Grd_TransactionsAll" runat="server" AllowPaging="True" 
          AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
          BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
          GridLines="Vertical"        Width="100%"    PageSize="20"  >
          <Columns>
             <asp:BoundField DataField="Fee Name" HeaderText="Fee Name" />
             <asp:BoundField DataField="Period" HeaderText="Period" />
             <asp:BoundField DataField="BatchName" HeaderText="Batch" />
             <asp:BoundField DataField="AccountType" HeaderText="Type" />
             <asp:BoundField DataField="Amount" HeaderText="Amount" />
           </Columns>
                       
          <EditRowStyle Font-Size="Medium" />
          <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
          <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
          <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
          <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px"    ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
          <SelectedRowStyle BackColor="White" ForeColor="Black" />
         </asp:GridView>
      
    </ContentTemplate>
   </asp:UpdatePanel>
      
</asp:Content>
