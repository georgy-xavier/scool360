<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ViewEmployeeSalary.aspx.cs" Inherits="WinEr.ViewEmployeeSalary"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title></title>

      <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style" media="screen" />
    <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style" media="screen" />
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" />
   <%-- <link rel="stylesheet" type="text/css" href="css files/wschoolstyle.css" title="style" media="screen" />--%>
    <script type="text/javascript">

        function PageRelorad() {
         window.opener.location.reload(true);
        }        
        function isIE() {
            return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
        }
//        function ChangeNetPay() {
//            var gross = document.getElementById('txt_Gross');            
//            var Total = document.getElementById('Txt_Total');
//            var Netpay = document.getElementById('txt_NetPay');
//            alert(gross.value);
//            Netpay.value = parseFloat(gross.value) - parseFloat(Total.value);
//        } 

        function PerCalOnHeadChange(e) {
//            var KeyID;
//            if (window.event) { //IE

//                KeyID = event.keyCode
//            }
//            else  // Netscape/Firefox/Opera
//            {
//                KeyID = e.keyCode;
//            }

//            if (KeyID == 8) {
//                return true;
//            }
//            if (KeyID == 46) {
//                return true;
//            }
//            if (KeyID < 48 || KeyID > 105) {
//                return false;
//            }
//            else {
//                if (KeyID > 57) {
//                    if (KeyID < 96) {
//                        return false;
//                    }
//                }
//            }

            var grdEarn = document.getElementById('Grd_Earning');
            var grdDed = document.getElementById('Grd_Deduction');
            var BP = document.getElementById('txt_EditPay');
            var HiddenEarnType = document.getElementById('HdnEarnType');
            var HiddenDedType = document.getElementById('HdnDedType');
            var HiddenDedPercent = document.getElementById('HdnDedPer');
            var HiddenEarnPercent = document.getElementById('HdnEarnPer');


            var Sum = 0;
            var EarnTypeArray = HiddenEarnType.value.split(",");
            var EarnPerArray = HiddenEarnPercent.value.split(",");
            var DedTypeArray = HiddenDedType.value.split(",");
            var DedPerArray = HiddenDedPercent.value.split(",");
            var GrdEarnSum = 0;
            var GrdDedSum = 0;

            var TxtGross = document.getElementById('txt_Gross');
            var TxtTotal = document.getElementById('Txt_Total');
            var TxtNetPay = document.getElementById('txt_NetPay');
            var Txt_advsaldeduction = document.getElementById('Txt_advsaldeduction');
            TxtGross.value = 0;
            TxtTotal.value = 0;
            TxtNetPay.value = 100;
            var Earnper = 0;
            var advded = 0;



            if (grdEarn != null) {
                for (var i = 1; i < grdEarn.rows.length; i++) {
                    var GrdEarnAmount = grdEarn.rows[i].cells[1].children[0];
                    Earnper = parseFloat(GrdEarnAmount.value * 100) / parseFloat(BP.value);
                    EarnPerArray[i - 1] = Earnper.toFixed(2);
                    if (parseFloat(GrdEarnAmount.value).toString() == "NaN") {

                        GrdEarnAmount.value = 0;


                    }
                    GrdEarnAmount.value = parseFloat(GrdEarnAmount.value);
                    GrdEarnSum = parseFloat(GrdEarnAmount.value) + parseFloat(GrdEarnSum);

                    var test = grdEarn.rows[i].cells[2].children[0];
                    test.value = Earnper;

                }
          
            
            
            
            if (EarnPerArray.length < grdEarn.rows.length-1) {
                EarnPerArray.push(0);
            } 
        }
            var newEarn = "";
            var sep = "";
            if (EarnPerArray != null) {
                for (i = 0; i < EarnPerArray.length; i++) {
                    newEarn = newEarn + sep + EarnPerArray[i];
                    sep = ",";
                }
            }

            HiddenEarnPercent.value = newEarn;           
            TxtTotal.value = parseFloat(GrdDedSum);

           TxtGross.value = parseFloat(GrdEarnSum) + parseFloat(BP.value);

           var Dedper = 0;
           if (grdDed != null) {
               for (i = 1; i < grdDed.rows.length; i++) {
                   var GrdDedAmount = grdDed.rows[i].cells[1].children[0];
                   Dedper = parseFloat(GrdDedAmount.value * 100) / parseFloat(BP.value);
                   DedPerArray[i - 1] = Dedper.toFixed(2);
                   if (parseFloat(GrdDedAmount.value).toString() == "NaN") {

                       GrdDedAmount.value = 0;


                   }
                   GrdDedAmount.value = parseFloat(GrdDedAmount.value);
                   GrdDedSum = parseFloat(GrdDedAmount.value) + parseFloat(GrdDedSum);
                   test = grdDed.rows[i].cells[2].children[0];
                   test.value = Dedper;


               }

               if (DedPerArray.length < grdDed.rows.length - 1) {
                   DedPerArray.push(0);
               }
           }
            var newDed = "";
            sep = "";
            if (DedPerArray != null) {
                for (i = 0; i < DedPerArray.length; i++) {
                    newDed = newDed + sep + DedPerArray[i];
                    sep = ",";
                }
            }
           
            HiddenDedPercent.value = newDed;

            
            TxtTotal.value = parseFloat(GrdDedSum);

            try {
                if (Txt_advsaldeduction != null) {
                    advded = Txt_advsaldeduction.value;
                }
                Sum = parseFloat(TxtGross.value) - parseFloat(TxtTotal.value) - parseFloat(advded);
                TxtNetPay.value = Sum;
                if (TxtNetPay.value < 0) {
                    alert("Net pay cannot be less than zero");
                }

            }
            catch (err) {
                TxtNetPay.value = "Nil";
            }
            return true;
        }
        
        function Calculations() {
//            var KeyID;
//            if (window.event) { //IE

//                KeyID = event.keyCode
//            }
//            else  // Netscape/Firefox/Opera
//            {
//                KeyID = e.keyCode;
//            }

//            if (KeyID == 8) {
//                return true;
//            }
//            if (KeyID == 46) {
//                return true;
//            }
//            if (KeyID < 48 || KeyID > 105) {
//                return false;
//            }
//            else {
//                if (KeyID > 57) {
//                    if (KeyID < 96) {
//                        return false;
//                    }
//                }
//            //            }



            var grdEarn = document.getElementById('Grd_Earning');
            var grdDed = document.getElementById('Grd_Deduction');
            var BP = document.getElementById('txt_EditPay');
            var HiddenEarnType = document.getElementById('HdnEarnType');
            var HiddenDedType = document.getElementById('HdnDedType');
            var HiddenDedPercent = document.getElementById('HdnDedPer');
            var HiddenEarnPercent = document.getElementById('HdnEarnPer');
           
            
            var Sum = 0;
            var EarnTypeArray = HiddenEarnType.value.split(",");
            var EarnPerArray = HiddenEarnPercent.value.split(",");
            var DedTypeArray = HiddenDedType.value.split(",");
            var DedPerArray = HiddenDedPercent.value.split(",");
            var GrdEarnSum = 0;
            var GrdDedSum = 0;

            var TxtGross = document.getElementById('txt_Gross');
            var TxtTotal = document.getElementById('Txt_Total');
            var TxtNetPay = document.getElementById('txt_NetPay');
            TxtGross.value = 0;
            TxtTotal.value = 0;
            TxtNetPay.value = 100;
            var per = 0;
            if (BP.value == "") {
                BP.value = 0;
            }
            else {
                BP.value = parseFloat(BP.value);
            }
           
            if (grdEarn != null) {
                for (var i = 1; i < grdEarn.rows.length; i++) {
                    var GrdEarnAmount = grdEarn.rows[i].cells[1].children[0];
                    if (EarnTypeArray[i - 1] == 1) {
                       
                        
                        GrdEarnAmount.value = (parseFloat(parseFloat(BP.value) * parseFloat(EarnPerArray[i - 1])) / 100).toFixed(2);
                    }

                    GrdEarnSum = parseFloat(GrdEarnAmount.value) + parseFloat(GrdEarnSum);

                }
            }
            
            TxtGross.value = parseFloat(GrdEarnSum) + parseFloat(BP.value);


            if (grdDed != null) {
                for (var i = 1; i < grdDed.rows.length; i++) {
                    var GrdDedAmount = grdDed.rows[i].cells[1].children[0];
                    if (DedTypeArray[i - 1] == 1) {
                        GrdDedAmount.value = (parseFloat(parseFloat(BP.value) * parseFloat(DedPerArray[i - 1])) / 100).toFixed(2);
                       

                    }

                    GrdDedSum = parseFloat(GrdDedAmount.value) + parseFloat(GrdDedSum);


                }
            }
           TxtTotal.value = parseFloat(GrdDedSum);
        
            try {
              

                Sum = parseFloat(TxtGross.value) - parseFloat(TxtTotal.value);
                
                    TxtNetPay.value = Sum.toFixed(2);
                 
            }
            catch (err) 
            {
                TxtNetPay.value = "Nil";
            }



           }




        function GetBp(e) {
        
//             var KeyID;
//         if (window.event) { //IE

//             KeyID = event.keyCode
//         }
//         else  // Netscape/Firefox/Opera
//         {
//             KeyID = e.keyCode;
//         }
//         
//         if (KeyID == 8) {
//             return true;
//         }
//          if (KeyID == 46) {
//             return true;
//         }
//         if (KeyID < 48 || KeyID > 105) {
//             return false;
//         }
//         else {
//             if (KeyID > 57) {
//                 if (KeyID < 96) {
//                     return false;
//                 }
//             }
//         }

              //var TxtBP = document.getElementById('<%#txt_Pay.ClientID%>');  
             //var TxtDisplayBp = document.getElementById('<%#txt_EditPay.ClientID%>');           
            //var TxtWrkng = document.getElementById('<%#txt_TotWkng.ClientID%>');
            //var TxtWrked = document.getElementById('<%#txt_TotWked.ClientID%>');

            var TxtBP = document.getElementById('txt_Pay').value;
            var TxtWrkng = document.getElementById('txt_TotWkng').value;
            var TxtDisplayBp = document.getElementById('txt_EditPay');
            var TxtWrked = document.getElementById('txt_TotWked').value;
            var TxtGross = document.getElementById('txt_Gross');
            var TxtNetPay = document.getElementById('txt_NetPay');
            var Diff = 0;
            var PerDaySal = 0;
            var newBp = 0;
            
            if (parseFloat(TxtWrked) != null && parseFloat(TxtWrkng) != null) {
                if (parseFloat(TxtWrkng) < parseFloat(TxtWrked)) {
                    
                   alert("Working Days must be Greater than Worked days");
                    TxtWrked.value = "";
                }
                else 
                {
                    if (TxtWrked == "") {
                        TxtWrked = 0;
                    }
                    TxtWrked = parseFloat(TxtWrked);
                    Diff = parseFloat(TxtWrkng) - parseFloat(TxtWrked);

                    if (parseFloat(TxtBP) != null && parseFloat(TxtBP) != 0) {

                        if (parseFloat(TxtWrkng) != 0) 
                        {
                            PerDaySal = parseFloat(TxtBP) / parseFloat(TxtWrkng);
                            newBp = parseFloat(TxtBP) - (parseFloat(Diff) * parseFloat(PerDaySal));
                            TxtDisplayBp.value = parseFloat(newBp.toFixed(2));
                            TxtGross.value = parseFloat(newBp.toFixed(2));
                            TxtNetPay.value = parseFloat(newBp.toFixed(2));
                            
                        }
                     }
                   
                }
            }
            Calculations();

        }
        function AdvSalRsCalculation(e) {
//            var KeyID;
//            if (window.event) { //IE

//                KeyID = event.keyCode
//            }
//            else  // Netscape/Firefox/Opera
//            {
//                KeyID = e.keyCode;
//            }

//            if (KeyID == 8) {
//                return true;
//            }
//            if (KeyID == 46) {
//                return true;
//            }
//            if (KeyID < 48 || KeyID > 105) {
//                return false;
//            }
//            else {
//                if (KeyID > 57) {
//                    if (KeyID < 96) {
//                        return false;
//                    }
//                }
//            }

//            var TxtAmount = document.getElementById('<%#Txt_Amount.ClientID%>');
//            var TxtPercent = document.getElementById('<%#Txt_Per.ClientID%>');
            //            var TxtRs = document.getElementById('<%#Txt_Rs.ClientID%>');
            var TxtAmount = document.getElementById('Txt_Amount').value;
            var TxtPercent = document.getElementById('Txt_Per').value;
            var TxtRs = document.getElementById('Txt_Rs');
            var Advnce = document.getElementById('Hdn_Advance');
            var Advnceded = document.getElementById('Hdn_advaded');
            var RsAmount = 0;


            if (TxtAmount == "") {

                alert("Please Enter Amount");
            }
            else {
                if (Advnceded.value == "") {
                    Advnce.value = TxtAmount;
                    RsAmount = parseFloat(TxtAmount) * (parseFloat(TxtPercent) / 100);
                    TxtRs.value = RsAmount.toFixed(2);
                }
            }
        }


        function AdvSalPerCalculation(e) {

            var KeyID;
            if (window.event) { //IE

                KeyID = event.keyCode
            }
            else  // Netscape/Firefox/Opera
            {
                KeyID = e.keyCode;
            }

            if (KeyID == 8 || KeyID == 46) {
            
                return true;
            }
            
            if (KeyID < 48 || KeyID > 105) {
                return false;
            }
            else {
                if (KeyID > 57) {
                    if (KeyID < 96) {
                        return false;
                    }
                }
            }
//            var TxtAmount = document.getElementById('<%#Txt_Amount.ClientID%>');
//            var TxtPercent = document.getElementById('<%#Txt_Per.ClientID%>');
            //            var TxtRs = document.getElementById('<%#Txt_Rs.ClientID%>');
            var TxtAmount = document.getElementById('Txt_Amount');
            var TxtPercent = document.getElementById('Txt_Per');
            var TxtRs = document.getElementById('Txt_Rs');
            var Advnce = document.getElementById('Hdn_Advance');
            
            var Per = 0;
            if (TxtAmount.value == "") {
                alert("Please Enter Amount");
            }
            else {
                Advnce.value = TxtAmount.value;
                Per = (parseFloat(TxtRs.value) * 100) / parseFloat(TxtAmount.value);
                TxtPercent.value = parseFloat(Per.toFixed(2));
            }
            
        }




    </script>
     
    <style type="text/css">
        .style1
        {
            width: 387px;
        }
    </style>
     
</head>
<body>
    <form id="form1" runat="server">
      <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
   
    <div>
    
  
   
    <asp:Panel ID="Pnl_EditEmp" runat="server" HorizontalAlign="Center"> <%-- style="display:none"--%>
     <div  class="container skin1" style="width:600px; height:auto;">     
         <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/configure1.png" 
                        Height="28px" Width="29px" />
            </td>
           <td class="n">EDIT EMPLOYEE SALARY</td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
          
<table class="tablelist" >

                    <tr>
                        <td  class="leftside" style="width:269px;">
                            <asp:Label ID="Label4" runat="server" Text="Employee Name" 
                                Font-Bold="True"></asp:Label><span style="color:Red">*</span>
                               </td> <td class="rightside">
                            <asp:TextBox ID="Txt_EditName" runat="server" MaxLength="50" Width="160px" 
                                TabIndex="1"></asp:TextBox>
                               <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\01234567879!@#$%^&*()_+=-" 
                                TargetControlID="Txt_EditName">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" 
                                ValidationGroup="Edit" ControlToValidate="Txt_EditName" 
                                ErrorMessage="Enter Name"></asp:RequiredFieldValidator>
                        </td>
                       
                         </tr>
                    <tr>
                          <td  class="leftside" style="width:269px;">
                            <asp:Label ID="Lbl_Payroll" runat="server" Text="PayrollType" 
                                Font-Bold="True"></asp:Label>
                        </td> <td class="rightside">
                        <asp:DropDownList ID="Drp_PayCat" runat="server" Width="164px" AutoPostBack="True" 
                                 onselectedindexchanged="Drp_PayCat_SelectedIndexChanged" TabIndex="2">
                        </asp:DropDownList>
                  
                        </td>
               
                </tr>
                    <tr>
                        <td class="leftside" style="width:269px;">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Basic Pay"></asp:Label>
                            <span style="color:Red">*</span>
                            </td><td class="rightside">
                            <asp:TextBox 
                                ID="txt_EditPay" runat="server"  Width="160px" MaxLength="10" 
                                TabIndex="3" onkeyup="Calculations()" ></asp:TextBox>
                                <asp:TextBox ID="txt_Pay" runat="server"  Width="40px" MaxLength="10" 
                                CssClass="noscreen"
                                 ></asp:TextBox> <%--CssClass="noscreen"--%>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender83" 
                                     runat="server" Enabled="True" TargetControlID="txt_EditPay" 
                                FilterMode="ValidChars" FilterType="Custom" ValidChars = "0123456789." >
                                 </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator83" runat="server" 
                                ValidationGroup="Edit" ControlToValidate="txt_EditPay" ErrorMessage="Enter 

Value"></asp:RequiredFieldValidator>
</td>
                        
                       
                                             
                    </tr>
                    <tr>
                        <td class="leftside" style="width:269px;">
                            <asp:Label ID="LblMarry" runat="server" Font-Bold="True" Text="PAN"></asp:Label>
                            </td><td  class="rightside">
                            <asp:TextBox ID="Txt_Pan" runat="server" Width="160px" TabIndex="4" 
                                MaxLength="15"></asp:TextBox> 
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                      TargetControlID="Txt_Pan" InvalidChars=";:',.?/~`!@#$%^&*()_+|\][>-+}{<">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                               <asp:RegularExpressionValidator ID="reg" runat="server" Display="Static" ValidationGroup="Edit"  ControlToValidate="Txt_Pan"
ErrorMessage="Please Enter Valid Pan Number"
ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="Edit" ControlToValidate="Txt_Pan" ErrorMessage="Enter PAN number"></asp:RequiredFieldValidator>
                                 
                        </td>
               
                    </tr>
                    <tr>
                        <td  class="leftside" style="width:269px;">
                            <asp:Label ID="Label14" runat="server" Font-Bold="True" Text="Bank Account Number" Width="160px"></asp:Label>
                            <span style="color:Red">*</span>
                            </td><td  class="rightside">
                                                      
                            <asp:TextBox ID="txt_EditBank" runat="server" Width="160px" MaxLength="15" 
                                TabIndex="5"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" 
                                     runat="server" Enabled="True" TargetControlID="txt_EditBank" 
                                FilterMode="ValidChars" FilterType="Custom" ValidChars = "0123456789" >
                                     
                                 </ajaxToolkit:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ValidationGroup="Edit" ControlToValidate="txt_EditBank"  
                                ErrorMessage="Enter AccountNo"></asp:RequiredFieldValidator>
                        </td>
                    
                      </tr>
                      <tr><td class="leftside" style="width:269px;"><asp:Label ID="Lbl_ResignDate" Font-Bold="True" runat="server" Text="Resign Date"></asp:Label></td>
                      <td class="rightside"><asp:TextBox runat="server" Width="160px" ID="Txt_resignDate"></asp:TextBox></td></tr>
                      <tr>
                        <td  class="leftside" style="width:269px;">
                            <asp:Label ID="LblComment" runat="server" Font-Bold="True" Text="Comment" Width="160px"></asp:Label>
                             </td> <td  class="rightside">
                            <asp:TextBox ID="TxtComment" runat="server" Width="160px" TextMode="MultiLine" 
                                TabIndex="5" ></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\;-" TargetControlID="TxtComment">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                              
                        </td>
               
                      </tr>
                   
                      
                      <tr>
                        <td  class="leftside" style="width:269px;">
                            <asp:Label ID="Lbl_TotWked" runat="server" Font-Bold="True" Text="Total Worked Days" Width="160px"></asp:Label>
                              </td><td  class="rightside">
                            <asp:TextBox ID="txt_TotWked" runat="server" Width="160px" MaxLength="3"  Text="25" onkeyup="GetBp(event)"
                                TabIndex="5"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" 
                                     runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom,Numbers" 
                                     ValidChars="Numbers" TargetControlID="txt_TotWked">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" 
                                ValidationGroup="Edit" ControlToValidate="txt_TotWked"  
                                ErrorMessage="Enter Days"></asp:RequiredFieldValidator>
                        </td>
                              
                          
                      </tr>
                      
                         <tr>
                        <td  class="leftside" style="width:269px;">
                            <asp:Label ID="Lbl_TotWkng" runat="server" Font-Bold="True" Text="Total Working Days" Width="160px"></asp:Label>
                              </td><td class="rightside">
                            <asp:TextBox ID="txt_TotWkng" runat="server" Width="160px" MaxLength="3" Text="25" 
                                TabIndex="5"  onkeyup="GetBp(event)"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" 
                                     runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom,Numbers" 
                                     ValidChars="Numbers" TargetControlID="txt_TotWkng">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" 
                                ValidationGroup="Edit" ControlToValidate="txt_TotWkng"  
                                ErrorMessage="Enter Days"></asp:RequiredFieldValidator>
                        </td>
              
                      </tr>
                      <tr>
                      <td class="leftside" style="width:269px;"><asp:Label Font-Bold="True" ForeColor="Brown" ID="Lbl_PreviousAdvamount" runat="server" Text="Previous Advance Amount"></asp:Label></td>
                      <td class="rightside"><asp:TextBox runat="server" Width="160px" ForeColor="Brown" ID="Txt_previousAdvamount"></asp:TextBox></td>
                      </tr>
                      
                      <tr>
                      <td class="leftside" style="width:269px;"><asp:Label Font-Bold="True" ForeColor="Brown" ID="Lbl_advdeduction" runat="server" Text="Advace salary deduction"></asp:Label></td>
                      <td class="rightside"><asp:TextBox runat="server" Width="160px" ForeColor="Brown" ID="Txt_advsaldeduction"></asp:TextBox></td>
                      </tr>

                 </table>  
<div class="linestyle"></div>  

<table style="Width:100%">
<tr>
<td>
<br /><br />
</td>
</tr>
<tr>

<td align="right" style="width:269px">
    <asp:LinkButton ID="Lnk_SalAdvance" runat="server" 
        onclick="Lnk_SalAdvance_Click">Advance Salary</asp:LinkButton> &nbsp;
    <asp:Button ID="Btn_AddEarning" runat="server" Text="Add Earnings" 
        CssClass="grayadd" Width="120px" onclick="Btn_AddEarning_Click" 
        TabIndex="6"/>
        
       
        
</td>
<td align="right">  <asp:Button ID="Btn_AddDeduction" runat="server" Text="Add Deduction" 
        CssClass="grayadd" Width="120px" onclick="Btn_AddDeduction_Click" 
        TabIndex="6"/></td>


</tr>
</table>

<table style="Width:100%">
<tr>
<td align="right"  >
<asp:GridView DataKeyNames="HeadId" ID="Grd_Earning" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="5"  BackColor="#EBEBEB" OnRowDataBound="Grd_Earning_RowDataBound"
                     OnRowDeleting="Grd_Earning_RowDeleting"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  
                   CellPadding="3" CellSpacing="2" Font-Size="12px"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        
                         <asp:BoundField DataField="HeadId"  HeaderText="Head Id" ItemStyle-Width="20px" />
                           
                        <asp:BoundField DataField="HeadName"  HeaderText="Earnings" ItemStyle-Width="100px" />
                         
                                
                         <asp:TemplateField HeaderText="Amount" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%--<asp:TextBox ID="Txt_Earamount" runat="server" Width="50px"  MaxLength="8"  Text='<%# Eval("HeadAmount") %>' onkeyup="return PerCalOnHeadChange()"></asp:TextBox>   --%>
                        <input type="text" id="Txt_Earamount" runat="server" value='<%# Eval("HeadAmount") %>' style="width:50px" onkeyup="return PerCalOnHeadChange(event)" />
                       
                    </ItemTemplate>
                    </asp:TemplateField>
                    
                    
                    <asp:TemplateField HeaderText="Percent" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:TextBox ID="Txt_EarnPercent" runat="server" Width="50px"  MaxLength="8"  Text="0"  ></asp:TextBox>
                        
                    </ItemTemplate>
                    </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Remove" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("HeadId") %>' CommandName="Delete" runat="server">Remove</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                <asp:BoundField DataField="DecreaseType"  HeaderText="Decrease Type" ItemStyle-Width="100px"  />
                     
                        </Columns>
                        
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>

</td>

<td align="right" style="vertical-align:top;">
<asp:GridView DataKeyNames="HeadId" ID="Grd_Deduction" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" OnRowDataBound="Grd_Deduction_RowDataBound"
                     OnRowDeleting="Grd_Deduction_RowDeleting"
                             PageSize="5"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
        onselectedindexchanged="Grd_Deduction_SelectedIndexChanged"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                       
                          <asp:BoundField DataField="HeadId"  HeaderText="Head Id" ItemStyle-Width="20px" />  
                        <asp:BoundField DataField="HeadName"  HeaderText="Deduction" ItemStyle-Width="100px" />
                         
                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="30px" >
                    <ItemTemplate>
                        <%--<asp:TextBox ID="Txt_Dedamount" runat="server" Width="50px"  MaxLength="8"  Text='<%# Eval("HeadAmount") %>' onkeyup="return PerCalOnHeadChange()"></asp:TextBox>   --%>
                        <input type="text" id="Txt_Dedamount" runat="server" value='<%# Eval("HeadAmount") %>' style="width:50px" onkeyup="return PerCalOnHeadChange(event)"  />
                        <%--onclick="return Txt_Dedamount_onclick(event)"--%>
                    </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Percent" ItemStyle-Width="30px" >
                    <ItemTemplate>
                        
                        <asp:TextBox ID="Txt_DedPercent" runat="server" Width="50px"  MaxLength="8"  Text="0" ></asp:TextBox>  
                    </ItemTemplate>
                    </asp:TemplateField>          
                         
                       <asp:TemplateField HeaderText="Remove" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("HeadId") %>' CommandName="Delete" runat="server">Remove</asp:LinkButton>
                        </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                <asp:BoundField DataField="DecreaseType"  HeaderText="Decrease Type" ItemStyle-Width="100px" />
                     
                        </Columns>
                        
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
</td>
</tr>
<tr>
<td>
<br />
</td>
</tr>

<tr>
                        <td align="right" style="vertical-align:middle;">
                            <asp:Label ID="Lbl_Gross" runat="server" Text="Gross" Font-Bold="true"></asp:Label>&nbsp;
                           
                             <input ID="txt_Gross" runat="server" onkeydown="return false" 
                            style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                            type="text" /><br />
                            
                                
                        </td>
                        <td align="right" style="vertical-align:middle">
                               <asp:Label ID="Lbl_Total" runat="server" Text="Total Deduction" Font-Bold="true"></asp:Label>&nbsp;
                            <input ID="Txt_Total" runat="server" onkeydown="return false" 
                            style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                            type="text" />            
                        </td>
                        
      </tr>
     
      <tr>
      
      <td align="right">
      <asp:Label ID="Lbl_NetPay" runat="server" Text="Net Pay" Font-Bold="true"></asp:Label>&nbsp;
                           
                            <input ID="txt_NetPay" runat="server" onkeydown="return false" 
                            style="background-color:#CCCCCC;color:Black;border:Double 1px black" 
                            type="text" />
                            
      </td>
      <td>
          &nbsp;</td>
      </tr>
</table>
<div class="linestyle"></div> 
<table style="Width:100%">
      
      <tr><td align="center"><asp:Label ID="lbl_error" runat="server"></asp:Label></td></tr>
      

      <tr>
                        
                        <td style="text-align:center">
                      
                            <asp:Button ID="Btn_Update" runat="server" Width="90px" ValidationGroup="Edit" 
                                Text="Update" onclick="Btn_Update_Click" CssClass="graysave" TabIndex="8"/> &nbsp;
                                   
                            <asp:Button ID="Btn_EditCancel" runat="server" Text="Close"  Width="90px" CssClass="graycancel"
                                OnClientClick="javascript:window.close();" TabIndex="9"/>        
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
    </div>
  </asp:Panel>
        
        
    <asp:Panel ID="Pnl_AddEarn" runat="server">               
  <asp:Button runat="server" ID="Button3" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_AddEarn_popup" 
                                  runat="server"  PopupControlID="Panel1" TargetControlID="Button3" CancelControlID="Btn_Add_Cancel" BackgroundCssClass="modalBackground" />
   <asp:Panel ID="Panel1" runat="server"  DefaultButton="Btn_Add" style="display:none;" >   <%--style="display:none;" --%>                      
    <div  class="container skin6" style="width:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable" >
        <tr >
            <td class="no"> </td>
            <td class="n">Add Earnings Head</td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <center>
                  <table cellspacing="10">
                    <tr>
                     <td align="left">
                         <asp:Label ID="Lbl_AddName" runat="server" Text="Select Earnings" Font-Bold="true"></asp:Label>
                     </td>
                     <td>
                         <asp:DropDownList ID="Drp_AddEarn" runat="server">
                         </asp:DropDownList>
                     </td>
                   </tr>
                   <tr>
                     <td align="right">
                         <asp:Button ID="Btn_Add" runat="server" Text="ADD" CssClass="grayadd" 
                             onclick="Btn_Add_Click"/>
                     </td>
                     <td align="left">
                         <asp:Button ID="Btn_Add_Cancel" runat="server" Text="Cancel" CssClass="graycancel" />
                     </td>
                    </tr>
                    <tr>
                      <td colspan="2">
                          <asp:Label ID="lbl_Addmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                      </td>
                    </tr>
                  </table>
                </center>                   
                    
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


<asp:Panel ID="Pnl_AddDed" runat="server">               
  <asp:Button runat="server" ID="Button4" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_AddDed_popup" 
                                  runat="server"  PopupControlID="Panel2" TargetControlID="Button4" CancelControlID="Btn_AddD_Cancel" BackgroundCssClass="modalBackground" />
   <asp:Panel ID="Panel2" runat="server"  DefaultButton="Btn_AddD" style="display:none;">   <%--style="display:none;" --%>                      
    <div  class="container skin6" style="width:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable" >
        <tr >
            <td class="no"> </td>
            <td class="n">Add Deductions Head</td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <center>
                  <table cellspacing="10">
                    <tr>
                     <td align="left">
                         <asp:Label ID="Lbl_AddDName" runat="server" Text="Select Deductions" Font-Bold="true"></asp:Label>
                     </td>
                     <td>
                         <asp:DropDownList ID="Drp_AddDed" runat="server">
                         </asp:DropDownList>
                     </td>
                   </tr>
                   <tr>
                     <td align="right">
                         <asp:Button ID="Btn_AddD" runat="server" Text="ADD" CssClass="grayadd" 
                             onclick="Btn_AddD_Click"/>
                     </td>
                     <td align="left">
                         <asp:Button ID="Btn_AddD_Cancel" runat="server" Text="Cancel" CssClass="graycancel" />
                     </td>
                    </tr>
                    <tr>
                      <td colspan="2">
                          <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                      </td>
                    </tr>
                  </table>
                </center>                   
                    
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

<asp:Panel ID="Pnl_AdvSal" runat="server">               
  <asp:Button runat="server" ID="Button41" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_SalAdv_popup" 
                                  runat="server"  PopupControlID="PnlSalAdv" TargetControlID="Button41" CancelControlID="BtnCancel" BackgroundCssClass="modalBackground" />
   <asp:Panel ID="PnlSalAdv" runat="server"  DefaultButton="Btn_AddSalAdv"  style="display:none;">   <%--style="display:none;" --%>                      
    <div  class="container skin6" runat="server" style="width:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable" width="100%" >
        <tr >
            <td class="no"> </td>
            <td class="n">Advance Salary Management</td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <center>
                  <table cellspacing="10" width="100%">
                    <tr> 
                     <td align="right" >
                         <asp:Label ID="Lbl_AdvAmount" runat="server" Text="Advance Amount" Font-Bold="true" Width="140px"></asp:Label>
                     </td>
                     <td align="left">
                         <asp:TextBox ID="Txt_Amount" onkeyup="AdvSalRsCalculation(event)" runat="server"></asp:TextBox>  
                     </td>
                   </tr>
                   <tr>
                     <td  style="width:100px; text-align:right;">
                         <asp:Label ID="Lbl_Amount" runat="server" Text="Per Month(Rs/%)" Font-Bold="true" ></asp:Label>
                     </td>
                     <td align="left" style="width:250px;">
                   <%--  onkeyup="AdvSalPerCalculation(event)"
                   onkeyup="AdvSalRsCalculation--%>
                         <asp:TextBox ID="Txt_Rs" runat="server" Width="80px"   onkeyup="AdvSalPerCalculation(event)"></asp:TextBox>
<asp:TextBox ID="Txt_Per" runat="server" Width="40px"  onkeyup="AdvSalRsCalculation(event)"></asp:TextBox>
<asp:Label ID="Lbl_Per" runat="server" Text="%" Font-Bold="true"></asp:Label>
                     </td>
                   </tr>
                   
                   <tr>
                     <td align="right" >
                         <asp:Button ID="Btn_AddSalAdv" runat="server" Text="ADD" CssClass="grayadd" onclick="Btn_AddSalAdv_Click"                           
                         OnClientClick="ChangeNetPay(event)"    />
                              <%-- OnClientClick="PerCalOnHeadChange(event)"--%>
                     </td>
                     <td align="left">
                         <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="graycancel" 
                             onclick="BtnCancel_Click" />
                     </td>
                    </tr>
                    <tr>
                      <td colspan="2">
                          <asp:Label ID="Label3" runat="server" Text="" ForeColor="Red"></asp:Label>
                      </td>
                    </tr>
                    <tr><td>
                        <asp:HiddenField ID="Hdn_advaded" runat="server" />
                    </td></tr>
                  </table>
                </center>                   
                    
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

        <asp:HiddenField ID="HdnEarnType" runat="server" />
        <asp:HiddenField ID="HdnDedType" runat="server" />
        <asp:HiddenField ID="HdnEarnPer" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="HdnDedPer" runat="server" />
        <asp:HiddenField ID="Hdn_Advance" runat="server" />
        <asp:HiddenField ID="Hdn_previousadv" runat="server" />
        <asp:HiddenField ID="Hdn_previouspercentage" runat="server" />
        <asp:HiddenField ID="Hdn_CheckAdv" runat="server" />
        <asp:HiddenField ID="Hdn_Dedamnt" runat="server" />
        <asp:HiddenField ID="Hdn_earnamnt" runat="server" />
        
    </div>
    
    <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
       </ContentTemplate>
    
    </asp:UpdatePanel>
      
    </form>
</body>
</html>
