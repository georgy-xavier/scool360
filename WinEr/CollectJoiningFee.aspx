<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CollectJoiningFee.aspx.cs" Inherits="WinEr.CollectJoiningFee" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="OTHERFEE" Src="~/WebControls/OtherFeeControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="FEEADVANCE" Src="~/WebControls/FeeAdvanceControl.ascx"%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <style type="text/css">
  .Tablecss
  {
      border-style:outset;
      border-color:Gray;
      border-width:2px;
      background-color:#222222;
  }
  .tdcss
  {
       color:#eeeeee;
  }
    .labelcss
    {
        border:none;
        background-color:Black;
        color:#eeeeee;
    }
    .GridFeildCss
    {
        padding-left:20px;
    }
    .PanelStyle
    {
        visibility:hidden;
    }
       
  
 </style>
 
 <script type="text/javascript">

     function isIE() {
         return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
     }
     function OtherCalculations() {
         var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
         var Sum = 0;
         var _balance;
         var _checked = false;
         var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
         var Btn_payfee = document.getElementById('<%=Btn_payfee.ClientID%>');
         var TxtTotatAmount = document.getElementById('<%=TxtTotatAmount.ClientID%>');
         var Txt_AmountPaying = document.getElementById('<%=Txt_AmountPaying.ClientID%>');
         var Txt_Balance = document.getElementById('<%=Txt_Balance.ClientID%>');
         BtnBill.disabled = true;
         TxtTotatAmount.value = 0;
         try {
             if (Txt_AmountPaying.value == "Enter Amount") {

                 Txt_Balance.value = 0;
             }
             else {

                 _balance = Sum - parseFloat(Txt_AmountPaying.value);
                 Txt_Balance.value = _balance;
             }
         }
         catch (err) {
             Txt_Balance.value = "Nil";
         }
         Btn_payfee.disabled = true;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             if (cb.checked == true) {
                 Sum = Sum + parseFloat(gridViewCtl.rows[i].cells[2].textContent);
                 if (Sum >= 0) {
                     TxtTotatAmount.value = Sum;
                 }
                 else {
                     //Lbl_MessageError.Text = "This amount is not valid";
                     // this.MPE_MessageError.Show();
                     TxtTotatAmount.value = 0;
                     Txt_Balance.value = "Nil";
                     break;
                 }
                 try {
                     if (Txt_AmountPaying.value == "Enter Amount") {
                         Txt_Balance.value = TxtTotatAmount.value;
                     }
                     else {
                         _balance = Sum - parseFloat(Txt_AmountPaying.value);
                         Txt_Balance.value = _balance;
                     }
                 }
                 catch (err) {
                     Txt_Balance.value = "Nil";
                 }
             }
         }

         if (_checked == true && Txt_Balance.value == 0) {
             Btn_payfee.disabled = false;
         }
       
     }



     function ExplorerCalculations() {
         var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
         var Sum = 0;
         var _balance;
         var _checked = false;
         var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
         var Btn_payfee = document.getElementById('<%=Btn_payfee.ClientID%>');
         var TxtTotatAmount = document.getElementById('<%=TxtTotatAmount.ClientID%>');
         var Txt_AmountPaying = document.getElementById('<%=Txt_AmountPaying.ClientID%>');
         var Txt_Balance = document.getElementById('<%=Txt_Balance.ClientID%>');
         BtnBill.disabled = true;
         TxtTotatAmount.value = 0;
         try {
             if (Txt_AmountPaying.value == "Enter Amount") {

                 Txt_Balance.value = 0;
             }
             else {

                 _balance = Sum - parseFloat(Txt_AmountPaying.value);
                 Txt_Balance.value = _balance;
             }
         }
         catch (err) {
             Txt_Balance.value = "Nil";
         }
         Btn_payfee.disabled = true;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             if (cb.checked == true) {
                 Sum = Sum + parseFloat(gridViewCtl.rows[i].cells[2].innerText);
                 if (Sum >= 0) {
                     TxtTotatAmount.value = Sum;
                 }
                 else {
                     //Lbl_MessageError.Text = "This amount is not valid";
                     // this.MPE_MessageError.Show();
                     TxtTotatAmount.value = 0;
                     Txt_Balance.value = "Nil";
                     break;
                 }
                 try {
                     if (Txt_AmountPaying.value == "Enter Amount") {
                         Txt_Balance.value = TxtTotatAmount.value;
                     }
                     else {
                         _balance = Sum - parseFloat(Txt_AmountPaying.value);
                         Txt_Balance.value = _balance;
                     }
                 }
                 catch (err) {
                     Txt_Balance.value = "Nil";
                 }
             }
         }

         if (_checked == true && Txt_Balance.value == 0) {
             Btn_payfee.disabled = false;
         }


     }

     function Calculate() {

         if (isIE()) {
             //alert("Explorer");
             ExplorerCalculations();
         }
         else {
             //alert("Other");
             OtherCalculations();
         }

     }

     function CalculateAmountPaying() {
     
         var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
         var Txt_AmountPaying = document.getElementById('<%=Txt_AmountPaying.ClientID%>');
         var Txt_Balance = document.getElementById('<%=Txt_Balance.ClientID%>');
         var TxtTotatAmount = document.getElementById('<%=TxtTotatAmount.ClientID%>');
         var Btn_payfee = document.getElementById('<%=Btn_payfee.ClientID%>');
         BtnBill.disabled = true;

         try {
             var _balance;
             if (Txt_AmountPaying.value == "") {

                 Txt_Balance.value = TxtTotatAmount.value;
             }
             else {
                 var AmountPaying = parseFloat(Txt_AmountPaying.value);
                 if (AmountPaying >= 0) {
                     Txt_AmountPaying.value = AmountPaying;
                 }
                 _balance = parseFloat(TxtTotatAmount.value) - AmountPaying;
                 Txt_Balance.value = _balance;
             }
         }
         catch (err) {
             Txt_Balance.value = "Nil";
         }

         if (IfCheched() && Txt_Balance.value == 0) {
             Btn_payfee.disabled = false;
         }
         else {
             Btn_payfee.disabled = true;
         }

     }

     function IfCheched() {
         var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
         var _checked = false;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             if (cb.checked == true) {
                 _checked = true;
             }
         }
         return _checked;
     }


     function RdbChangingPanels(number) {
         var Lbl_Id = document.getElementById('<%=TextBoxLabel.ClientID%>');
         var rb0 = document.getElementById('<%=RadioButton0.ClientID%>');
         var rb1 = document.getElementById('<%=RadioButton1.ClientID%>');
         var rb2 = document.getElementById('<%=RadioButton2.ClientID%>');
         var rb3 = document.getElementById('<%=RadioButton3.ClientID%>');
         var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
         var Pnl_paymod = document.getElementById('<%=Pnl_paymod.ClientID%>');
         if (Pnl_paymod != undefined) {
             BtnBill.disabled = true;
             if (number == 0) {
                 Pnl_paymod.style.visibility = "hidden";
                 rb1.checked = false;
                 rb2.checked = false;
                 rb3.checked = false;
             }
             else if (number == 1) {
                 Pnl_paymod.style.visibility = "visible";
                 rb0.checked = false;
                 rb2.checked = false;
                 rb3.checked = false;
                 Lbl_Id.value = "Cheque No";
             }
             else if(number==2) {
                 Pnl_paymod.style.visibility = "visible";
                 rb0.checked = false;
                 rb1.checked = false;
                 rb3.checked = false;
                 Lbl_Id.value = "DD No";
             }
             else
             {
                 Pnl_paymod.style.visibility = "visible";
                 rb0.checked = false;
                 rb1.checked = false;
                 rb2.checked = false;
                 Lbl_Id.value = "Transaction No";
             }

         }

     }

     function Rdb_Changing0() {

         RdbChangingPanels(0);
     }
     function Rdb_Changing1() {

         RdbChangingPanels(1);
     }
     function Rdb_Changing2() {

         RdbChangingPanels(2);
     }
     function Rdb_Changing3() {

         RdbChangingPanels(3);
     }

     function ValidatePay() {
         var Txt_paymentid = document.getElementById('<%=Txt_paymentid.ClientID%>');
         var Txt_bank = document.getElementById('<%=Txt_bank.ClientID%>');
         var rb1 = document.getElementById('<%=RadioButton1.ClientID%>');
         var rb2 = document.getElementById('<%=RadioButton2.ClientID%>');
         var rb3 = document.getElementById('<%=RadioButton3.ClientID%>');
         if (rb1.checked == true) {
             if (Txt_paymentid.value == "" || Txt_bank.value == "") {
                 alert("Please Enter Cheque Details");
                 return false;
             }
         }
         else if (rb2.checked == true) {
             if (Txt_paymentid.value == "" || Txt_bank.value == "") {
                 alert("Please Enter DD Details");
                 return false;
             }
         }
         else if (rb3.checked == true) {
             if (Txt_paymentid.value == "" || Txt_bank.value == "") {
                 alert("Please Enter NEFT Details");
                 return false;
             }
         }

     }
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
         Calculate();
     }
 </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
         
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
         
    <div id="contents">
         
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate> 
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
  
           
<asp:Panel ID="Pnl_feearea" runat="server" DefaultButton="Btn_payfee">
<center>

<div class="container skin1" style="width:800px">
<table cellpadding="0" cellspacing="0" class="containerTable">
<tr >
	<td class="no"> </td>
	<td class="n">
	  <table width="100%">
	   <tr>
	    <td align="left" style="width:50%">
	      Fee Due
	    </td>
	    <td  align="right" style="width:50%">
	       <asp:LinkButton ID="Link_CollectFee" runat="server"  Font-Size="12px" ForeColor="Green" Font-Underline="true"  OnClientClick="javascript:history.go(-1);return false;">Back</asp:LinkButton>
	    </td>
	   </tr>
	  </table>
	 
	
	</td>
	<td class="ne"> </td>
</tr>
<tr >
   <td class="o"> </td>
   <td class="c" >


<table width="100%" cellspacing="10" class="Tablecss">
        <tr>
         <td align="right" class="tdcss" style="width:15%;">
          Student Name : 
         </td>
         <td  align="left" style="color:White;font-weight:bold">
             <asp:Label ID="Lbl_StudentName" class="control-label" runat="server" Text=""></asp:Label>
         </td>
         <td  align="right" class="tdcss" style="width:15%;">
          Registration Id : 
         </td>
         <td  align="left" style="color:White;font-weight:bold">
             <asp:Label ID="Lbl_RegistrationId" runat="server" class="control-label" Text=""></asp:Label>
         </td>
        <td  align="right" class="tdcss" style="width:15%;">
          Gender : 
         </td>
         <td  align="left" style="color:White;font-weight:bold">
          <asp:Label ID="Lbl_Gender" runat="server" class="control-label" Text=""></asp:Label>
         </td>
        </tr>
        
        <tr>

         <td  align="right" class="tdcss">
           Standard : 
         </td>
         <td  align="left" style="color:White;font-weight:bold">
           <asp:Label ID="Lbl_Standard" runat="server" class="control-label" Text=""></asp:Label>
         </td>
        <td  align="right" class="tdcss">
          Class Name :
         </td>
         <td  align="left" style="color:White;font-weight:bold">
           <asp:Label ID="Lbl_ClassName" runat="server" class="control-label" Text=""></asp:Label>
         </td>
         <td align="right" class="tdcss">
          Joining Batch : 
         </td>
         <td  align="left" style="color:White;font-weight:bold">
          <asp:Label ID="Lbl_JoiningBatch" runat="server" class="control-label" Text=""></asp:Label>
         </td>
        </tr>
       </table>
       
       <div style="text-align:right">
	                	    <asp:Button ID="Lnk_Advance" runat="server"  Class="btn btn-primary" onclick="Lnk_Advance_Click" Text="Advance"></asp:Button>&nbsp;&nbsp;&nbsp;
				     	    <asp:Button ID="Lnk_OtherFee" runat="server" Class="btn btn-primary" onclick="Lnk_OtherFee_Click" Text="OtherFee"></asp:Button>&nbsp;&nbsp;&nbsp;
				   	        
	            </div>
    <asp:Panel ID="Panel_FeeDataArea" runat="server">
     
        <br />
            <div style=" overflow:auto; height: 170px;">
                <asp:GridView ID="GridViewAllFee" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="Black" GridLines="Both" Width="100%" 
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                    <RowStyle BackColor="White" HorizontalAlign="Left"/>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server"  onclick="Calculate()" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                        </asp:TemplateField>
                      
                        <asp:BoundField DataField="Id" HeaderText="Student Fee ID" />
                        <asp:BoundField DataField="AccountName" HeaderText="Fee Name" ItemStyle-CssClass="GridFeildCss" />
                        <asp:BoundField DataField="Amount" HeaderText="Balance" ItemStyle-CssClass="GridFeildCss" />
                        <asp:BoundField DataField="Regular" HeaderText="Regular" ItemStyle-CssClass="GridFeildCss" />
                        <asp:BoundField DataField="CollectionType" HeaderText="CollectionType" ItemStyle-CssClass="GridFeildCss" />
                        <asp:BoundField DataField="PeriodId" HeaderText="Period" ItemStyle-CssClass="GridFeildCss" /> 
                        <asp:BoundField DataField="FeeId" HeaderText="FeeId"/> 
                        <asp:BoundField DataField="PeriodName" HeaderText="PeriodName"/> 
                        <asp:BoundField DataField="BatchId" HeaderText="BatchId"/> 
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </div>
      
        <br />
    
                 <table width="100%" >
                     <tr>
                         <td align="right">
                             Total Amount : 
                         </td>
                         <td  align="left">
                             <input ID="TxtTotatAmount" runat="server" onkeydown="return false" 
                            style="background-color:#CCCCCC;color:Black;border:Double 1px black" class="form-control" 
                            type="text" />
                         </td>
                         <td align="right">
                             Balance : </td>
                         <td align="left">
                             <input ID="Txt_Balance" runat="server" onkeydown="return false" 
                            style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                            type="text" class="form-control"/>
                         </td>
                         <td align="right">
                             Amount Paid : </td>
                         <td  align="left">
                             <asp:TextBox ID="Txt_AmountPaying" runat="server" onkeyup="CalculateAmountPaying()" class="form-control"></asp:TextBox>
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
                         <td>
                             &nbsp;</td>
                         <td >
                             &nbsp;</td>
                         <td class="style1" >
                             &nbsp;</td>
                         <td colspan="3">
                             &nbsp;</td>
                     </tr>
                     <tr>
                         <td >
                             &nbsp;</td>
                         <td colspan="5">
                             Note : Balance should be zero for the payment of the fee.</td>
                     </tr>
                     <tr>
                         <td >
                             &nbsp;</td>
                         <td  colspan="5">
                             &nbsp;</td>
                     </tr>
                     <tr >
                         <td >
                             &nbsp;</td>
                         <td >
                             Mode Of Payment</td>
                         <td  colspan="4">
                         <asp:RadioButton ID="RadioButton0" runat="server" Checked="true" 
                            onclick="Rdb_Changing0()" Text="Cash" />
                            <asp:RadioButton ID="RadioButton1" runat="server" Checked="false" 
                            onclick="Rdb_Changing1()" Text="Cheque" />
                            <asp:RadioButton ID="RadioButton2" runat="server" Checked="false" 
                            onclick="Rdb_Changing2()" Text="Demand Draft" />
                             <asp:RadioButton ID="RadioButton3" runat="server" Checked="false" 
                            onclick="Rdb_Changing3()" Text="NEFT" />
                            <%--<asp:RadioButtonList ID="Rdb_PaymentMode" runat="server" 
                                 RepeatDirection="Horizontal" AutoPostBack="True" 
                                 onselectedindexchanged="Rdb_PaymentMode_SelectedIndexChanged" 
                                 Width="240px">
                                 <asp:ListItem Selected="True" Value="0">Cash</asp:ListItem>
                                 <asp:ListItem Value="1">Cheque</asp:ListItem>
                                 <asp:ListItem Value="2">Demand Draft</asp:ListItem>
                             </asp:RadioButtonList>--%>
                          </td>
                         <td align="right" >
                          
                             Date:</td>
                             <td align="left" >
                                 <asp:TextBox ID="Txt_Pay_Date" runat="server"></asp:TextBox>
                         </td>
                             <ajaxToolkit:CalendarExtender ID="Txt_Pay_Date_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="Txt_Pay_Date" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                    </ajaxToolkit:CalendarExtender>
                    <%--<asp:RegularExpressionValidator runat="server" ID="PayDateRegularExpressionValidator3"
                                ControlToValidate="Txt_Pay_Date"
                                Display="None"
                               
                                ValidationExpression="^((((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9]))))[\-\/\s]?\d{2}(([02468][048])|([13579][26])))|(((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))[\-\/\s]?\d{2}(([02468][1235679])|([13579][01345789]))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
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
                      <td colspan="6" style="height:30px;">
                        <asp:Panel ID="Pnl_paymod" runat="server" CssClass="PanelStyle">
                        <table  width="100%" class="Tablecss">
                         <tr>
                             <td align="right" style="width:25%">
                                 <%--<asp:Label ID="Lbl_Id" runat="server" Text="Label"></asp:Label>--%>
                                  <asp:TextBox ID="TextBoxLabel" runat="server" BorderStyle="None" CssClass="labelcss" Width="100px" ReadOnly="true"></asp:TextBox>
                                 <asp:Label ID="Lbl_Mand1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                 </td>
                             <td align="left" style="width:25%">
                                 <asp:TextBox ID="Txt_paymentid" runat="server" MaxLength="40"></asp:TextBox>
                             </td>
                             <td align="right" class="tdcss" style="width:25%;">
                                 Bank Name  <asp:Label ID="Lbl_Mand2" runat="server" Text="*" ForeColor="Red"></asp:Label></td>
                             <td align="left" style="width:25%">
                                 <asp:TextBox ID="Txt_bank" runat="server" MaxLength="90" Width="150px"></asp:TextBox>
                             </td>
                         </tr>
                         
                     </table>
                 </asp:Panel>
                      </td>
                     </tr>
                 </table>
    
                 
    
        <br /><ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txt_bank" 
                                runat="server" Enabled="True" TargetControlID="Txt_paymentid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txtbank" 
                                runat="server" Enabled="True" TargetControlID="Txt_bank" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                 
     </asp:Panel>              
    <asp:Panel ID="Panel_Complete"  runat="server" Visible="false">
  
      
        <table width="100%">
         <tr>
          <td style="font-size:16px;width:100%" align="center">
            No more due present for the student
            
            <br />
             <br />
          </td>
 
         </tr>
        </table>
         
   
    </asp:Panel>
                 
                 
                 
                 
                 <table width="100%">
                     <tr>
                         <td align="center">
                             <asp:Label ID="Lbl_FeeBillMessage" runat="server" ForeColor="Red" ></asp:Label>
                              <asp:TextBox ID="TxtBillNo" runat="server" Visible="False" class="form-control"></asp:TextBox>
                             </td>
                     </tr>
                     <tr>

                         <td align="center">
                            
                         
                             <asp:Button ID="Btn_payfee" runat="server" Text="Pay Fee" Width="125px" OnClientClick="return ValidatePay()" OnClick="Btn_payfee_Click" Class="btn btn-primary" />
     
                             &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnBill" runat="server"  onclick="BtnBill_Click" Text="Generate Bill" Width="125px" Class="btn btn-primary" />
                             &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnCancelBill" runat="server"   OnClick="BtnCancelBill_Click" Class="btn btn-danger"
                                 Text="Cancel Bill" Width="125px" />&nbsp;&nbsp;&nbsp;
                            <%-- <asp:Button ID="Btn_NewCollect" runat="server" Text="CollectNew" Width="125px"  OnClick="Btn_NewCollect_Click" CssClass="grayok" />   --%>
                         </td>
                     </tr>
                 </table>
                 
        <br />
        
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
	
</center>
</asp:Panel>        
                       

         
    <asp:HiddenField ID="Hdn_studid" runat="server" />
    <asp:HiddenField ID="Hdn_Standid" runat="server" />
    <asp:HiddenField ID="Hdn_ClassId" runat="server" />

         
         
      <asp:Panel ID="Pnl_CancelBill" runat="server">
                         <asp:Button runat="server" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_CancelBill"  runat="server" CancelControlID="Btn_BillCancel" 
                                  PopupControlID="Pnl_Bill" TargetControlID="Btn_BillCancel"  />
                          <asp:Panel ID="Pnl_Bill" runat="server" style="display:none">
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="CancelFeeBills" ControlToValidate="Txt_CancelReason" ErrorMessage="Enter Reason"></asp:RequiredFieldValidator>
                                 </div>
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_CancelBill"  OnClick="Btn_CancelBill_Click" runat="server" Text="Ok" class="btn btn-primary" ValidationGroup="CancelFeeBills" Width="100px"/>     
                                            <asp:Button ID="Btn_BillCancel" runat="server" Text="Cancel" Width="100px" class="btn btn-danger" />
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
             <WC:OTHERFEE ID="OtherFeeBox" runat="server" />
          
           <WC:FEEADVANCE ID="FeeAdvanceBox" runat="server" />
     </ContentTemplate>
   </asp:UpdatePanel>
    </div>
</asp:Content>
