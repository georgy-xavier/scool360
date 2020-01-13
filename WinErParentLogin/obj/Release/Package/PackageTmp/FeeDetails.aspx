<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="True" CodeBehind="FeeDetails.aspx.cs" Inherits="WinErParentLogin.FeeDetails" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script type="text/javascript">
 function SelectAll(cbSelectAll) {
     var gridViewCtl = document.getElementById('<%=Grd_Feetopay.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
            Calculate();
        }
        function Calculate() {
            var Hdn_Amount = document.getElementById('<%=Hdn_TotalAmount.ClientID%>');
            var txt_Amount = document.getElementById('<%=Txt_Amount.ClientID%>');
            txt_Amount.value = "";
            var gridViewCtl = document.getElementById('<%=Grd_Feetopay.ClientID%>');
            var total=0;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                if (cb.checked == true) {
                    var Amount = parseFloat(gridViewCtl.rows[i].cells[0].children[1].value);
                    var fine = parseFloat(gridViewCtl.rows[i].cells[7].childNodes[0].data);
                    total = parseFloat(parseFloat(total) + Amount + fine);
                    
                }
            }
            txt_Amount.value = total;
            Hdn_Amount.value = total;

        }

</script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

         <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:Panel ID="Panel1" runat="server" BorderColor="Black" >

          <ajaxToolkit:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_yuitabview-theme"  
                       Width="100%" ActiveTabIndex="1" >
       
       <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                <HeaderTemplate>
                     <asp:Image ID="Image4" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/page_process2.png" /> <b>FEE TRANSACTIONS</b></HeaderTemplate>
                           <ContentTemplate>
                               <asp:Panel ID="Panel3" runat="server" style="min-height:400px;" >  
                          <br /> 
                            <table style="width:100%" class="tablelist">
                            
                            <tr>
                             <td class="leftside">Select Batch :</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="Drp_Batch" runat="server" Width="150px" class="form-control" AutoPostBack="True"
                                     onselectedindexchanged="Drp_Batch_SelectedIndexChanged">
                                 </asp:DropDownList>
                             </td>
                            </tr>
                           
                            <tr>
                             <td class="leftside">
                                 Select Type: </td>
                             <td class="rightside">
                             <div class="radio radio-primary">
                                 <asp:RadioButtonList ID="Rdb_FeeType" class="form-actions" runat="server" 
                                     RepeatDirection="Horizontal" TabIndex="4" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="Rdb_FeeType_SelectedIndexChanged">
                                     <asp:ListItem Selected="True" Text="ALL" Value="0"></asp:ListItem>
                                     <asp:ListItem  Text="Regular Fee" Value="1"></asp:ListItem>
                                     <asp:ListItem  Text="Joining Fee" Value="2"></asp:ListItem>
                                 </asp:RadioButtonList>
                               </div>
                             </td>
                           
                            </tr>
                             <tr>
                             <td class="leftside">Select Bill Type :</td>
                             <td class="rightside">
                             <div class="radio radio-primary">
                                 <asp:RadioButtonList ID="Rdb_BillType" runat="server" class="form-actions" AutoPostBack="True" OnSelectedIndexChanged="Rdb_BillType_SelectedIndexChanged"
                                     RepeatDirection="Horizontal">
                                  <asp:ListItem  Value="1" Selected="True">Cleared</asp:ListItem>
                                   <asp:ListItem Value="2">UnCleared</asp:ListItem>
                                 </asp:RadioButtonList>
                                 </div>
                             </td>
                            </tr> 
                            
                            <tr  align="right">
                                <td colspan="2"> 
                                <asp:ImageButton ID="ImgExportAll" runat="server"  OnClick="ImgExportAll_Click" ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px" />
                               </td>
                           </tr>
                      
                          </table>
                         <div class="linestyle"></div>
                                      
                    <asp:Label ID="Lbl_TransAllMsg" runat="server"></asp:Label>
                   <br/>                    
                    
                    <asp:GridView ID="Grd_TransactionsAll" runat="server" AllowPaging="True" 
                       AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
                       BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                       GridLines="Vertical"        Width="100%"    PageSize="20" 
                        onselectedindexchanged="Grd_TransactionsAll_SelectedIndexChanged" 
                        OnPageIndexChanging="Grd_TransactionsAll_PageIndexChanging" >
                       <Columns>                        
                      
                              <asp:BoundField DataField="Fee Name" HeaderText="Fee Name" />
                           <asp:BoundField DataField="Period" HeaderText="Period" />
                           <asp:BoundField DataField="BatchName" HeaderText="Batch" />
                           <asp:BoundField DataField="AccountType" HeaderText="Type" />
                           <asp:BoundField DataField="Amount" HeaderText="Amount" />
                           <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                           <asp:BoundField DataField="BillNo" HeaderText="BillNo" />
                           <asp:BoundField DataField="Type" HeaderText="Type" />
                           <asp:BoundField DataField="BatchId" HeaderText="Batch Id" />
                                 <asp:CommandField SelectText="&lt;img src='pics/Details.png' width='30px' border=0 title='Select bill to view'&gt;" 
                  ShowSelectButton="True" HeaderText="BILL" />
                       </Columns>
                       
                        <EditRowStyle Font-Size="Medium" />
                       
                      <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                        <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                            ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                   </asp:GridView>        
        
                               </asp:Panel>
                           </ContentTemplate>
         </ajaxToolkit:TabPanel>
      
       <ajaxToolkit:TabPanel runat="server" ID="TabFeeToPay" HeaderText="Signature and Bio">
                <HeaderTemplate>
                     <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/dollar.png" /> <b>FEE TO PAY</b></HeaderTemplate>
                           <ContentTemplate>
                             <br />
                               <asp:Panel ID="Pnl_FeeToPay" runat="server" style="min-height:400px;">
                                                
                                    
                  <asp:GridView runat="server" ID="Grd_Fees_Header" AutoGenerateColumns="False" BackColor="#EBEBEB"
                     BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px" Width="100%" 
                     onpageindexchanging="Grd_Fees_Header_PageIndexChanging" 
                     AllowPaging="True"
                     PageSize="5" 
                     OnRowCommand="Grd_Fees_Header_RowCommand" EnableModelValidation="True"
                      >
			      <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <Columns>   
                  <asp:BoundField DataField="Id"  HeaderText="Id" >              
                      <ItemStyle Width="360px" />
                      </asp:BoundField>
                  <asp:BoundField DataField="Name" HeaderText="Fees Group Name">
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                  <asp:BoundField DataField="BalanceAmount" HeaderText="Amount"> 
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                  <asp:BoundField HeaderText="Fine"> 
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                  <asp:BoundField HeaderText="Total">
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                  <asp:buttonfield commandname="select"  
                          text="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"  
                          HeaderText="Details" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>
                                 <asp:buttonfield  commandname="Payment" 
                          text="&lt;img src='Pics/payment.jpg' width='40px' border=0 title='Payment Online'&gt;"  
                          HeaderText="Pay" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                </asp:buttonfield>

                  </Columns>                  
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
			    </asp:GridView>
       
        
                               </asp:Panel>
                               <asp:TextBox ID="Txt_Id" runat="server" class="form-control" Visible="False"></asp:TextBox>
                               <asp:TextBox ID="Txt_Name" runat="server" class="form-control" Visible="False"></asp:TextBox>
                               <asp:TextBox ID="Txt_TAmount" runat="server" class="form-control" Visible="False"></asp:TextBox>
                               <center>
                                   <asp:Label ID="Lbl_Header" runat="server" Text="gfjhgj" class="control-label" ForeColor="Red"></asp:Label>
                               </center>
                          
				
                           </ContentTemplate>
         </ajaxToolkit:TabPanel> 
                
   </ajaxToolkit:TabContainer>
   
   
           <br />
        </asp:Panel>
                                     <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DD"  runat="server" CancelControlID="Btn_Cancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_FeeInfo" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_FeeInfo" runat="server"  DefaultButton="Btn_Cancel" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:600px; top:300px;left:300px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Fees Details"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
           <center>
            <asp:Panel ID="Pnl_FeeDetails" runat="server">
                
                                 <table class="style1" width="100%">
                 
                        
                        <tr>
                            <td >
                                <%--<asp:CheckBox ID="chkBoxAll" runat="server" AutoPostBack="True"  Checked="false"
                                    oncheckedchanged="chkBoxAll_CheckedChanged" Text="All Fee" Visible=false />--%></td>
                           
                            <td align="right">
                              <b> Total Amount</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                              <asp:TextBox ID="Txt_Amount"  
                                    runat="server"   Font-Size="Medium" class="form-control"
                                    ForeColor="Black" Enabled="false"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                     <asp:HiddenField ID="Hdn_Billviewneeded" runat="server" />
                      <asp:HiddenField ID="Hdn_TotalAmount" runat="server" /> 
                            </td>
                            <td style="width:120px" align="right">
                                <asp:Button ID="Btn_feeexport" runat="server" onclick="Button1_Click" 
                                    Text="Export To Excel"  class="btn btn-primary" /></td>
                        </tr>
                    </table>
                   <table width="100%">
                   <tr>
                   <td align="center"> <asp:Label  ID="Lbl_feeMessage" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label></td></tr>
                   <tr><td align="right">  
                   <asp:Button  ID="Btn_DD" runat="server" Text="Pay Fee" class="btn btn-primary" onclick="Btn_DD_Click" /> 
                     </td></tr>
                   </table>
                  
                   
                    <asp:GridView  ID="Grd_Feetopay" runat="server" BackColor="White" 
                        AutoGenerateColumns="False" 
                           BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="4" 
                           ForeColor="Black" GridLines="Vertical" Width="100% " AllowPaging="True" onpageindexchanging="Grd_Feetopay_PageIndexChanging" 
                        >
                           
                           <Columns>
                                   <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server"    Checked="true"
                                onClick="Calculate()" />
                                
                                     <asp:HiddenField   runat="server" ID="Txt_Bal" Value='<%#Eval("BalanceAmount") %>' />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                           
                        </asp:TemplateField>
                        <asp:BoundField DataField="Fee Name" HeaderText="Fee Name" />
                        <asp:BoundField DataField="BatchName" HeaderText="Batch" />
                        <asp:BoundField DataField="Period" HeaderText="For Period" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" />
                        <asp:BoundField DataField="LastDate" HeaderText="Last Date" />
                        
                         <asp:BoundField DataField="SchId" HeaderText="Schedule Id" />
                         <asp:BoundField DataField="PeriodId" HeaderText="Period Id" />
                         <asp:BoundField DataField="FeeId" HeaderText="Fee Id" />
                         <asp:BoundField DataField="BatchId" HeaderText="Batch Id" />
                         <asp:BoundField DataField="DueDate" HeaderText="Due Date" />
                          <asp:BoundField DataField="FeeStudentid" HeaderText="FeeStudentid" />
                          
                           <asp:BoundField DataField="Fine" HeaderText="Fine" />
                                  
                                                                                      
                            </Columns>

                      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                       </asp:GridView> 
                       
                       <br />
                       
                       <center>
                           <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger"  OnClick="Btn_Cancel_Click"/>
                       </center>
               
               </asp:Panel>
               </center>
                        <br /><br />
                        <div style="text-align:center;">
                           
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
 
<WC:MSGBOX ID="MSGBOX" runat="server" />
</asp:Content>
