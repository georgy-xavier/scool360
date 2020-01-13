<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" Inherits="PromotStudents"  Codebehind="PromotStudents.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 70px;
        }
        .style3
        {
            width: 144px;
        }
        .style4
        {
            width: 68px;
        }
        .style5
        {
            width: 165px;
        }
        .style7
        {
            width: 70px;
            height: 20px;
        }
        .style8
        {
            height: 20px;
        }
        .style9
        {
            width: 68px;
            height: 20px;
        }
        .style10
        {
            width: 165px;
            height: 20px;
        }
        .style11
        {
            height: 20px;
        }
    </style>
    
    <style type="text/css">
        #Text2
        {
            width: 126px;
            height: 24px;
        }
    .style1
    {
        width: 100%;
    }
    .style2
    {
    }
    .style3
    {
    }
    .style4
    {
        width: 189px;
    }
    .style5
    {
        width: 150px;
        height: 36px;
    }
    .style6
    {
        width: 198px;
        height: 36px;
    }
    .style7
    {
        width: 189px;
        height: 36px;
    }
    .style8
    {
        height: 36px;
    }
    #Div2
    {
        height: 89px;
    }
    .style10
    {
    }
     .newtable
    {
        width: 878px;
    }
    .style12
    {
        width: 165px;
    }
    .style16
    {
    }
    .style17
    {
        
  
         border-color: #999966;
         border-style: double none double double;
         border-width:thick;
    
         
    }
    .style18
    {
        width: 113px;
    }
    .style19
    {
        width: 125px;
    }
    .style20
    {
        width: 162px;
    }
    .style21
    {
        width: 120px;
    }
    .style22
    {
        width: 873px;
    }
    .style23
    {
        width: 373px;
    }
    .textarea
    {
        border-style:double double double none;
         border-color: #999966; 
         border-width:thick;
         text-align: center;
        
        
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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

                
<div class="container skin1" style="min-height:300px">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Manual Promotion</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <asp:Panel ID="Pnl_FullData" runat="server" style="min-height:300px">
               
                       <table class="style1">
                        <tr><td colspan="4">&nbsp;</td></tr>
                    <tr>
                        <td align="right" style="width:200px" >
                            From : </td>
                        <td >
                            <asp:DropDownList ID="Drp_ClassFrom" runat="server"  Width="128px" class="form-control"
                                AutoPostBack="True" onselectedindexchanged="Drp_ClassFrom_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td >
                            To</td>
                        <td >
                            <asp:DropDownList ID="Drp_ClassTo" runat="server"  class="form-control" Width="128px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="Btn_Promote" runat="server" Text="Promote" 
                                onclick="Btn_Promote_Click"  class="btn btn-primary"/>
                        </td>
                        <td>
                            <asp:Button ID="Btn_History" runat="server" Text="History" 
                                onclick="Btn_History_Click"  class="btn btn-primary"/>
                        </td>
                    </tr>
                      <tr><td colspan="4">&nbsp;</td></tr>
                </table>
              
                        <div class="linestyle" runat="server"></div>
                    
                       <asp:Panel ID="Pnl_Dataarea" runat="server">
                       
            <asp:Panel ID="Pnl_MTDArea" runat="server">
               <asp:LinkButton ID="Lnk_select" runat="server" onclick="Lnk_select_Click">Select All</asp:LinkButton>   
                  <br />
<%--OnRowDataBound="Grd_Protionlist_RowDataBound" onrowcommand="GrdStudent_RowCommand"--%>
              <div style=" overflow:auto;max-height: 450px;">
                  <asp:GridView ID="Grd_Protionlist" runat="server" CellPadding="4" 
                      ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                      Width="97%" 
                      onselectedindexchanged="Grd_Protionlist_SelectedIndexChanged1" 
                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                      BorderWidth="1px" onrowdeleting="Grd_Protionlist_RowDeleting" 
                      onrowediting="Grd_Protionlist_RowEditing">
                        <Columns>
                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                 <asp:TemplateField HeaderText="Select" ItemStyle-Width="80px">
                 
                         <ItemTemplate>
                         
                             <asp:CheckBox ID="Chh_Stud" runat="server" ItemStyle-Width="40px"/>  
                            </ItemTemplate>  
                        </asp:TemplateField>
                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                <asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="100px"/>      
                        
                        
                        <asp:TemplateField HeaderText="Result" ItemStyle-Width="120px">
                         <ItemTemplate>
                             <asp:DropDownList ID="Drp_result" runat="server"  class="form-control" Width="80px">
                             <asp:ListItem Selected="True" Value="1">Pass</asp:ListItem>
                             <asp:ListItem Value="0">Fail</asp:ListItem>
                             </asp:DropDownList>  
                            </ItemTemplate>  
                        </asp:TemplateField>
                       
                        <asp:CommandField EditText="&lt;img src='Pics/hand.png' width='30px' border=0 title='Student Details'&gt;" ShowEditButton="True"  HeaderText="Student Details" ItemStyle-Width="90px"/>
                        
                            <asp:CommandField SelectText="&lt;img src='pics/certificate.png' width='30px' border=0 title='Select Student to issue Tc'&gt;" ShowSelectButton="True"  HeaderText="Issue TC" ItemStyle-Width="70px"/>
                            
                             <asp:CommandField DeleteText="&lt;img src='Pics/dollar.png' width='30px' border=0 title='Pay Fee'&gt;" ShowDeleteButton="True"  HeaderText="Pay Fee" ItemStyle-Width="70px"/>
                       <%--  <asp:TemplateField HeaderText="Pay Fee">
                        <ItemTemplate>
                     
                         <asp:ImageButton Width="30px" runat="server" ImageUrl="~/Pics/dollar.png" ID="Btn_PayFee"  CommandArgument='<%# Eval("Id")%>'  CommandName="PayFee" />
                        </ItemTemplate>
                      </asp:TemplateField>      --%>
                    </Columns>  
                      <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                      <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                      <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />                                                                                       
                      <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" Height="20px" HorizontalAlign="Left" />                                                                                  
                      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                      <EditRowStyle Font-Size="Medium" />     
                  </asp:GridView>
              </div>	
	        </asp:Panel>
	      
             
             <%--<asp:Panel ID="Pnl_MessageBox" runat="server">
                    <asp:Button runat="server" ID="Btn_Fee" style="display:none"/>
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1_Fee" runat="server" CancelControlID="BtnCancel" 
                                                    PopupControlID="Pnl_Feemsg" TargetControlID="Btn_Fee"  />
                    <asp:Panel ID="Pnl_Feemsg" runat="server" style="display:none">
                        <div class="container skin5" >
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no">
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                                                   Height="28px" Width="29px" />
                                    </td>
                                    <td class="n"><span style="color:White">Message</span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    
                                    <asp:HiddenField ID="Hidden_Batch" runat="server" />
                                    <asp:HiddenField ID="Hidden_StudentId" runat="server" />
                                    
                                     <asp:Panel ID="FeeGrid" runat="server">
                                     
                                        <asp:Panel ID="Panel3" runat="server">
     
       
            <br />
            

            
            <div style=" overflow:auto; height: 200px">
                <asp:GridView ID="GridViewAllFee" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%"
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server" AutoPostBack="True" 
                                   oncheckedchanged="CheckBoxUpdate_CheckedChanged"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SchId" HeaderText="Scheduled ID" />
                        <asp:BoundField DataField="Id" HeaderText="Student Fee ID" />
                        <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
                        <asp:BoundField DataField="BatchName" HeaderText="Batch" />
                        <asp:BoundField DataField="Period" HeaderText="For Period" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="BalanceAmount" HeaderText="Balance" />
                        
                        <asp:BoundField DataField="LastDate" HeaderText="Last Date" />
                        <asp:TemplateField HeaderText="Deduction">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtDeduction" runat="server" AutoPostBack="True" 
                                    MaxLength="8" ontextchanged="TxtDeduction_TextChanged" Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="TxtDeduction_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtDeduction" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrear">
                            <ItemTemplate>
                                <asp:TextBox ID="Txtarrier" runat="server" AutoPostBack="True" MaxLength="8" 
                                    ontextchanged="Txtarrier_TextChanged" Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txtarrier_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="Txtarrier" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fine">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtFine" runat="server" AutoPostBack="True" MaxLength="8" 
                                    ontextchanged="TxtFine_TextChanged" Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="TxtFine_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtFine" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </div>
             </asp:Panel>
               <br />
               
               <table class="newtable">
                     <tr>
                         <td class="style18">
                             <asp:Label ID="LblAmount" runat="server" Text="Total Amount" Width="125px"></asp:Label>
                         </td>
                         <td class="style16">
                             <asp:TextBox ID="TxtTotatAmount" runat="server" BackColor="#CCCCCC" 
                                 ForeColor="Black" ReadOnly="True" BorderStyle="Double">0</asp:TextBox>
                         </td>
                         <td class="style19">
                             Balance</td>
                         <td class="style12">
                             <asp:TextBox ID="Txt_Balance" runat="server" BorderStyle="Double" BackColor="#CCCCCC"
                                 ForeColor="Black" ReadOnly="True">0</asp:TextBox>
                         </td>
                         <td class="style17">
                             Amount Paid</td>
                         <td class="textarea">
                             <asp:TextBox ID="Txt_AmountPaying" runat="server" AutoPostBack="True"
                                 ontextchanged="Txt_AmountPaying_TextChanged"></asp:TextBox>
                             <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_AmountPaying_TextBoxWatermarkExtender" 
                                 runat="server" Enabled="True" TargetControlID="Txt_AmountPaying" WatermarkText="Enter Amount">
                             </ajaxToolkit:TextBoxWatermarkExtender>
                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_AmountPaying_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="Txt_AmountPaying" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                         </td>
                     </tr>
                     <tr>
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style16">
                             &nbsp;</td>
                         <td class="style19">
                             &nbsp;</td>
                         <td colspan="3">
                             &nbsp;</td>
                     </tr>
                     <tr>
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style16" colspan="5">
                             Note : Balance should be zero for the payment of the fee.</td>
                     </tr>
                     <tr>
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style16" colspan="5">
                             &nbsp;</td>
                     </tr>
                     <tr >
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style17">
                             Mode Of Payment</td>
                         <td class="textarea" colspan="2">
                            <asp:RadioButtonList ID="Rdb_PaymentMode" runat="server" 
                                 RepeatDirection="Horizontal" AutoPostBack="True" 
                                 onselectedindexchanged="Rdb_PaymentMode_SelectedIndexChanged" 
                                 Width="240px">
                                 <asp:ListItem Selected="True" Value="0">Cash</asp:ListItem>
                                 <asp:ListItem Value="1">Cheque</asp:ListItem>
                                 <asp:ListItem Value="2">Demand Draft</asp:ListItem>
                             </asp:RadioButtonList>
                             </td>
                         <td class="style17"  >
                          
                             Date</td>
                             <td class="textarea">
                                 <asp:TextBox ID="Txt_Pay_Date" runat="server"></asp:TextBox>
                         </td>
                             <ajaxToolkit:CalendarExtender ID="Txt_Pay_Date_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="Txt_Pay_Date" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>
               
                                <asp:RegularExpressionValidator ID="PayDateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_Pay_Date" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                             <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="PayDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                     </tr>
                     <tr>
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style16">
                             &nbsp;</td>
                         <td class="style19">
                             &nbsp;</td>
                         <td colspan="3">
                             &nbsp;</td>
                     </tr>
                     <tr>
                         <td class="style18">
                             &nbsp;</td>
                         <td class="style10" colspan="5">
                             &nbsp;</td>
                     </tr>
                 </table>
               
                 <asp:Panel ID="Pnl_paymod" runat="server" Height="64px" 
                     Visible="False">
                     <table class="style1" >
                         <tr>
                             <td class="style17">
                                 <asp:Label ID="Lbl_Id" runat="server" Text="Label"></asp:Label>
                                 <asp:Label ID="Lbl_Mand1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                 </td>
                             <td class="textarea">
                                 <asp:TextBox ID="Txt_paymentid" runat="server" MaxLength="40"></asp:TextBox>
                             </td>
                             <td class="style17">
                                 Bank Name  <asp:Label ID="Lbl_Mand2" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
                             <td class="textarea">
                                 <asp:TextBox ID="Txt_bank" runat="server" MaxLength="90" Width="300px"></asp:TextBox>
                             </td>
                         </tr>
                         <tr>
                             <td class="style19">
                                 &nbsp;</td>
                             <td class="style20">
                                 &nbsp;</td>
                             <td class="style21">
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                     </table>
                 </asp:Panel>
    
        <br /><ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txt_bank" 
                                runat="server" Enabled="True" TargetControlID="Txt_paymentid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txtbank" 
                                runat="server" Enabled="True" TargetControlID="Txt_bank" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                 
                 <table class="style22">
                     <tr>
                         <td class="style23">
                             &nbsp;</td>
                         <td>
                             <asp:Label ID="Lbl_FeeBillMessage" runat="server" ForeColor="Red" ></asp:Label>
                             </td>
                     </tr>
                     <tr>
                         <td >
                             <asp:TextBox ID="TxtBillNo" runat="server" Visible="False"></asp:TextBox>
                         </td>
                         <td>
                             <asp:Button ID="Btn_payfee" runat="server" onclick="Btn_payfee_Click" 
                                 Text="Pay Fee" Width="100px" />
                             &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnBill" runat="server"  onclick="BtnBill_Click" Text="Generate Bill" Width="100px" />
                             &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="Btn_CancelBill" runat="server"  onclick="Btn_CancelBill_Click" Text="CancelBill" Width="100px"  Visible="false"/>&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnCancel" runat="server" 
                                 Text="Cancel" Width="100px" />
                         </td>
                     </tr>
                 </table>
               
               
                                     </asp:Panel>
                                    
                                    
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
                </asp:Panel>--%></asp:Panel>
              
                      <table width="100%" runat="server">
                   <tr>
                          <td align="center">
                              <asp:Label ID="Lbl_note" class="control-label" runat="server"></asp:Label>
                          </td>
                      </tr>
              
              </table>
              
              </asp:Panel>
                </td>
                <td class="e"> </td>
            </tr>
            <tr>
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
             
           <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server"
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                             
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-info" Text="OK" Width="50px" onclick="Btn_magok_click"/>
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
        
        <%--<asp:Panel ID="Pnl_CancelBill" runat="server">
                         <asp:Button runat="server" ID="Btn_Cancel" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_CancelBill"  runat="server" CancelControlID="Btn_BillCancel" 
                                  PopupControlID="Pnl_Bill" TargetControlID="Btn_BillCancel"  />
                          <asp:Panel ID="Pnl_Bill" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Message</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                        <asp:Label ID="Lbl_BillMessage" runat="server" Text=""></asp:Label>
                                        <div style=" text-align:center">
                                        Reason : 
                                        <asp:TextBox ID="Txt_CancelReason" runat="server" Width="160" Height="30" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_CancelReason_FilteredTextBoxExtender" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="Txt_CancelReason">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="CancelFeeBills" ControlToValidate="Txt_CancelReason" ErrorMessage="Enter Reason"></asp:RequiredFieldValidator>
                                 </div>
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_Can"  OnClick="Btn_Cancel_Click" runat="server" Text="Ok"  ValidationGroup="CancelFeeBills" Width="100px"/>     
                                            <asp:Button ID="Btn_BillCancel" runat="server" Text="Cancel" Width="100px" />
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
                 </asp:Panel>--%>   
            
 

</ContentTemplate>
 </asp:UpdatePanel>
<div class="clear"></div>
</div>
</asp:Content>

