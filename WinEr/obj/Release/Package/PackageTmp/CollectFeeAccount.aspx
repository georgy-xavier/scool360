<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="CollectFeeAccount.aspx.cs" Inherits="WinEr.WebForm9" %>

<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="OTHERFEE" Src="~/WebControls/OtherFeeControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="FEEADVANCE" Src="~/WebControls/FeeAdvanceControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="SETTLEADVANCE" Src="~/WebControls/AdvanceSettelmentControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<script type="text/ecmascript">
//    function CancelBill() {
//    }

</script>--%>


    <style type="text/css">
        .lefttd {
            width: 50%;
        }

        .righttd {
            width: 50%;
        }

        .labelcss {
            border: none;
        }

        .PanelStyle {
            visibility: hidden;
        }

        #Text2 {
            width: 126px;
            height: 24px;
        }

        #Div2 {
            height: 89px;
        }

        .newtable {
            width: 878px;
        }

        .style17 {
            border-color: #999966;
            border-style: double none double double;
            border-width: thick;
        }

        .style22 {
            width: 873px;
        }

        .style23 {
            width: 373px;
        }

        .textarea {
            border-style: double double double none;
            border-color: #999966;
            border-width: thick;
        }
    </style>


    <script type="text/javascript">

        function isIE() {
            return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent);
        }
        function OtherCalculations() {

            var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
            var Sum = 0;
            var arrieramount;
            var Deduction_value;
            var Fine_value;
            var arrier;
            var amountpaid;
            var _balance;
            var _checked = false;
            var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
            var Btn_payfee = document.getElementById('<%=Btn_payfee.ClientID%>');
            var TxtTotatAmount = document.getElementById('<%=TxtTotatAmount.ClientID%>');
            var Txt_AmountPaying = document.getElementById('<%=Txt_AmountPaying.ClientID%>');
            var Txt_Balance = document.getElementById('<%=Txt_Balance.ClientID%>');
            var Lbl_MessageError = document.getElementById('<%=Lbl_MessageError.ClientID%>');
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
                    _checked = true;
                    debugger;
                    var Tx_dudection = gridViewCtl.rows[i].cells[7].children[0];
                    var Arrier = gridViewCtl.rows[i].cells[8].children[0];
                    var Txt_fine = gridViewCtl.rows[i].cells[9].children[0];
                    var Txt_amntpaid = gridViewCtl.rows[i].cells[10].children[0];
                     
                    Arrier.value = parseFloat(gridViewCtl.rows[i].cells[5].textContent);
                   
                    try {
                        arrieramount = parseFloat(Arrier.value);

                        if (!(arrieramount >= 0)) {
                            //alert("This amount is not valid");
                            Arrier.value = 0;
                            arrieramount = 0;
                        }
                        else {
                            Arrier.value = arrieramount;
                           // Txt_amntpaid.value = arrieramount;
                        }
                    }
                    catch (err) {
                        Arrier.value = 0;
                        arrieramount = 0;
                    }
                    try {

                        Deduction_value = parseFloat(Tx_dudection.value);
                        if (!(Deduction_value >= 0)) {
                            //alert("This amount is not valid");
                            Tx_dudection.value = 0;
                            Deduction_value = 0;
                        }
                        else {
                            Tx_dudection.value = Deduction_value;
                            Arrier.value = Arrier.value - Tx_dudection.value;
                           //Txt_amntpaid.value = Arrier.value;
                        }
                    }
                    catch (err) {
                        Tx_dudection.value = 0;
                        Deduction_value = 0;
                        //  Txt_amntpaid.value = 0;

                    }
                    try {
                        Fine_value = parseFloat(Txt_fine.value);
                        if (!(Fine_value >= 0)) {
                            //alert("This amount is not valid");
                            Txt_fine.value = 0;
                            Fine_value = 0;
                        }
                        else {
                            Txt_fine.value = Fine_value;
                            Arrier = Arrier.value;
                            var totalfine = parseFloat(Arrier) + parseFloat(Fine_value);
                            Arrier = totalfine;
                            gridViewCtl.rows[i].cells[8].children[0].value = totalfine;
                            //Txt_amntpaid.value = totalfine;
                        }
                    }
                    catch (err) {
                        Txt_fine.value = 0;
                        Fine_value = 0;
                        //Txt_amntpaid.value = 0;
                    }
                    try {
                        amountpaid = parseFloat(Txt_amntpaid.value);
                        if (!(amountpaid >= 0)) {
                            //alert("This amount is not valid");
                            Txt_amntpaid.value = 0;
                            amountpaid = 0;
                        }
                        else {
                            Txt_amntpaid.value = amountpaid
                        }
                    }
                    catch (err) {
                        Txt_amntpaid.value = 0;
                        amountpaid = 0;
                    }
                    try {

                        if (Txt_amntpaid.value >= 0) {
                            if (Txt_amntpaid.value == 0) {
                                Txt_amntpaid.value = gridViewCtl.rows[i].cells[8].children[0].value;
                            }
                            if (!Tx_dudection.value > 0) {


                                arrier = parseFloat(gridViewCtl.rows[i].cells[5].textContent) - parseFloat(Txt_amntpaid.value);
                                gridViewCtl.rows[i].cells[8].children[0].value = arrier;
                            }
                            else if (Tx_dudection.value == 0) {
                                arrier = parseFloat((Arrier) - parseFloat(Txt_amntpaid.value));
                                if (arrier < 0) {
                                    alert("Amount paid should not be greater than total amount");
                                    Txt_amntpaid.value = 0;
                                }
                                else {
                                    gridViewCtl.rows[i].cells[8].children[0].value = arrier;
                                }

                            }
                            else if (Tx_dudection.value > 0 && Txt_fine.value > 0) {
                                arrier = parseFloat(Arrier) - parseFloat(Txt_amntpaid.value);
                                if (arrier < 0) {
                                    //alert("Amount paid should not be greater than total amount");
                                    Txt_amntpaid.value = 0;
                                }
                                else {
                                    gridViewCtl.rows[i].cells[8].children[0].value = arrier;
                                }
                            }
                            else if (Tx_dudection.value > 0) {
                                arrier = parseFloat(Arrier) - parseFloat(Txt_amntpaid.value);
                                if (arrier < 0) {
                                    //alert("Amount paid should not be greater than total amount");
                                    Txt_amntpaid.value = 0;
                                }
                                else {
                                    gridViewCtl.rows[i].cells[8].children[0].value = arrier;
                                }
                            }



                        }

                        else {
                            TxtTotatAmount.value = 0;
                            Txt_Balance.value = 0;
                        }
                    }
                    catch (err) {
                        gridViewCtl.rows[i].cells[8].children[0] = 0;
                    }

                    if (parseFloat(gridViewCtl.rows[i].cells[5].textContent) < Deduction_value) {
                        alert("Deduction should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else if (parseFloat(gridViewCtl.rows[i].cells[5].textContent) < arrieramount) {
                        alert("Arrear should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else if (parseFloat(gridViewCtl.rows[i].cells[5].textContent) < parseFloat(Deduction_value)) {
                        alert("Arrear + Deduction should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else if (parseFloat(gridViewCtl.rows[i].cells[5].textContent) < arrieramount
                        ) {
                        alert("Amount Paid should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }

                    else {

                        Sum = parseFloat(Sum + parseFloat(parseFloat(parseFloat(parseFloat(gridViewCtl.rows[i].cells[5].textContent) - Deduction_value) - gridViewCtl.rows[i].cells[8].children[0].value) + Fine_value));
                        if (Sum >= 0) {
                            TxtTotatAmount.value = Sum;
                        }
                        else {
                            //alert("This amount is not valid");
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

            }
            if (_checked == true && Txt_Balance.value == 0) {
                Btn_payfee.disabled = false;
            }



        }



        function ExplorerCalculations() {
            var gridViewCtl = document.getElementById('<%=GridViewAllFee.ClientID%>');
            var Sum = 0;
            var arrieramount;
            var Deduction_value;
            var Fine_value;
            var _balance;
            var _checked = false;
            var BtnBill = document.getElementById('<%=BtnBill.ClientID%>');
         var Btn_payfee = document.getElementById('<%=Btn_payfee.ClientID%>');
            var TxtTotatAmount = document.getElementById('<%=TxtTotatAmount.ClientID%>');
            var Txt_AmountPaying = document.getElementById('<%=Txt_AmountPaying.ClientID%>');
            var Txt_Balance = document.getElementById('<%=Txt_Balance.ClientID%>');
            var Lbl_MessageError = document.getElementById('<%=Lbl_MessageError.ClientID%>');
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
                    _checked = true;
                    var Tx_dudection = gridViewCtl.rows[i].cells[7].children[0];
                    var Arrier = gridViewCtl.rows[i].cells[8].children[0];
                    var Txt_fine = gridViewCtl.rows[i].cells[9].children[0];

                    try {
                        arrieramount = parseFloat(Arrier.value);
                        if (!(arrieramount >= 0)) {
                            //alert("This amount is not valid");
                            Arrier.value = 0;
                            arrieramount = 0;
                        }
                        else {
                            Arrier.value = arrieramount
                        }
                    }
                    catch (err) {
                        Arrier.value = 0;
                        arrieramount = 0;
                    }
                    try {

                        Deduction_value = parseFloat(Tx_dudection.value);
                        if (!(Deduction_value >= 0)) {
                            //alert("This amount is not valid");
                            Tx_dudection.value = 0;
                            Deduction_value = 0;
                        }
                        else {
                            Tx_dudection.value = Deduction_value
                        }
                    }
                    catch (err) {
                        Tx_dudection.value = 0;
                        Deduction_value = 0;

                    }
                    try {
                        Fine_value = parseFloat(Txt_fine.value);
                        if (!(Fine_value >= 0)) {
                            //alert("This amount is not valid");
                            Txt_fine.value = 0;
                            Fine_value = 0;
                        }
                        else {
                            Txt_fine.value = Fine_value
                        }
                    }
                    catch (err) {
                        Txt_fine.value = 0;
                        Fine_value = 0;
                    }

                    if (parseFloat(gridViewCtl.rows[i].cells[5].innerText) < Deduction_value) {
                        alert("Deduction should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else if (parseFloat(gridViewCtl.rows[i].cells[5].innerText) < arrieramount) {
                        alert("Arrear should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else if (parseFloat(gridViewCtl.rows[i].cells[5].innerText) < parseFloat(arrieramount + Deduction_value)) {
                        alert("Arrear + Deduction should not be greater than amount");
                        TxtTotatAmount.value = 0;
                        Txt_Balance.value = "Nil";
                        break;
                    }
                    else {

                        Sum = parseFloat(Sum + parseFloat(parseFloat(parseFloat(parseFloat(gridViewCtl.rows[i].cells[5].innerText) - Deduction_value) - arrieramount) + Fine_value));
                        if (Sum >= 0) {
                            TxtTotatAmount.value = Sum;
                        }
                        else {
                            alert("This amount is not valid");
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

            }
            if (_checked == true && Txt_Balance.value == 0) {
                Btn_payfee.disabled = false;
            }
        }

        function Calculate() {

            if (isIE()) {
                // alert("Explorer");
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
                 var AmountPaying = Txt_AmountPaying.value;
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
             else if (number == 2) {
                 Pnl_paymod.style.visibility = "visible";
                 rb0.checked = false;
                 rb1.checked = false;
                 rb3.checked = false;
                 Lbl_Id.value = "DD No";
             }
             else {
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
         if (true) {
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
          return true;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contents" style="min-width: 1200px">


        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <%--<script type="text/javascript">
                  var xPos, yPos;
                  var prm = Sys.WebForms.PageRequestManager.getInstance();
                  prm.add_beginRequest(BeginRequestHandler);
                  prm.add_endRequest(EndRequestHandler);
                  function BeginRequestHandler(sender, args) {
                      xPos = $get('Div_Particular').scrollLeft;
                      yPos = $get('Div_Particular').scrollTop;
                  }
                  function EndRequestHandler(sender, args) {
                      $get('Div_Particular').scrollLeft = xPos;
                      $get('Div_Particular').scrollTop = yPos;
                  }
</script>--%>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate>


                <div id="progressBackgroundFilter">
                </div>

                <div id="processMessage">

                    <table style="height: 100%; width: 100%">

                        <tr>

                            <td align="center">

                                <b>Please Wait...</b><br />

                                <br />

                                <img src="images/indicator-big.gif" alt="" /></td>

                        </tr>

                    </table>

                </div>


            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate>

                <asp:Panel ID="PanelSelect" runat="server">
                    <div class="container skin1" style="width: 580px; padding-left: 70px">
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr>
                                <td class="no">
                                    <img alt="" src="Pics/currencygreen.png" height="30" width="30" /></td>
                                <td class="n">Collect Fee</td>
                                <td class="ne"></td>
                            </tr>
                            <tr>
                                <td class="o"></td>
                                <td class="c">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnShow">


                                        <table width="100%" cellspacing="10">
                                            <tr>
                                                <td class="lefttd" align="right">
                                                    <asp:Label ID="LblClass" runat="server" Text="Class" class="control-label" Width="125px"></asp:Label>
                                                </td>
                                                <td class="righttd" align="left">
                                                    <asp:DropDownList ID="DropDownClass" runat="server" AutoPostBack="True" class="form-control"
                                                        Width="185px" OnSelectedIndexChanged="DropDownClass_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="leftside">
                                                    <br />
                                                </td>
                                                <td class="rightside">
                                                    <br />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="lefttd" align="right">
                                                    <asp:Label ID="LblStdId" runat="server" Style="margin-bottom: 0px" class="control-label"
                                                        Text="Roll No:" Width="125px"></asp:Label>
                                                </td>
                                                <td class="righttd" align="left">
                                                    <asp:DropDownList ID="DropDownStudentId" runat="server" AutoPostBack="True" class="form-control"
                                                        Width="185px" OnSelectedIndexChanged="DropDownStudentId_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="leftside">
                                                    <br />
                                                </td>
                                                <td class="rightside">
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lefttd" align="right">
                                                    <asp:Label ID="LblStudName" runat="server" Text="Student Name" class="control-label" Width="125px"></asp:Label>
                                                </td>
                                                <td class="righttd" align="left">
                                                    <asp:DropDownList ID="Drp_Studname" runat="server" class="form-control" AutoPostBack="True"
                                                        Width="185px" OnSelectedIndexChanged="Drp_Studname_SelectedIndexChanged">
                                                    </asp:DropDownList>


                                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server"
                                                        TargetControlID="Drp_Studname" PromptCssClass="ListSearchExtenderPrompt"
                                                        QueryPattern="Contains" QueryTimeout="2000">
                                                    </ajaxToolkit:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="leftside">
                                                    <br />
                                                </td>
                                                <td class="rightside">
                                                    <br />
                                                </td>
                                            </tr>

                                            <tr id="RowStudentId" runat="server">
                                                <td class="lefttd" align="right">
                                                    <asp:Label ID="Lbl_StudentID" runat="server" Style="margin-bottom: 0px" class="control-label"
                                                        Text="Student Id:" Width="125px"></asp:Label>
                                                </td>
                                                <td class="righttd" align="left">
                                                    <asp:DropDownList ID="Drp_StudentId" runat="server" AutoPostBack="True" class="form-control"
                                                        Width="185px" OnSelectedIndexChanged="Drp_StudentId_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>

                                                <td class="lefttd" align="right">
                                                    <asp:CheckBox ID="chkBoxAll" runat="server" Text="All Fees" />
                                                    <%-- AutoPostBack="True" oncheckedchanged="chkBoxAll_CheckedChanged"--%>
                                                </td>
                                                <td class="righttd" align="left">
                                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click"
                                                        Text="Show Fees" Class="btn btn-primary" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Label ID="Lbl_info" runat="server" class="control-label" ForeColor="#FF3300"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </asp:Panel>
                                </td>
                                <td class="e"></td>
                            </tr>
                            <tr>
                                <td class="so"></td>
                                <td class="s"></td>
                                <td class="se"></td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="PanelStudent" runat="server" Visible="false" Width="1080px" Style="padding-left: 150px">
                                <asp:HiddenField ID="HiddenField_StudentId" runat="server" Value="0" />
                                <asp:HiddenField ID="HiddenField_ClassId" runat="server" Value="0" />
                                <div id="StudentTopStrip" runat="server">

                                    <div id="winschoolStudentStrip">
                                        <table class="NewStudentStrip" width="100%">
                                            <tr>
                                                <td class="left1"></td>
                                                <td class="middle1">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <img alt="" src="images/img.png" width="82px" height="76px" />
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <table width="500">
                                                                    <tr>
                                                                        <td class="attributeValue">Name</td>
                                                                        <td></td>
                                                                        <td>:</td>
                                                                        <td></td>
                                                                        <td class="DBvalue">Arun Sunny</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="11">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="attributeValue">Class</td>
                                                                        <td></td>
                                                                        <td>:</td>
                                                                        <td></td>
                                                                        <td class="DBvalue">BDS</td>

                                                                        <td class="attributeValue">Admission No</td>
                                                                        <td></td>
                                                                        <td>:</td>
                                                                        <td></td>
                                                                        <td class="DBvalue">100</td>

                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="attributeValue">Class No</td>
                                                                        <td></td>
                                                                        <td>:</td>
                                                                        <td></td>
                                                                        <td class="DBvalue">100</td>

                                                                        <td class="attributeValue">Age</td>
                                                                        <td></td>
                                                                        <td>:</td>
                                                                        <td></td>
                                                                        <td class="DBvalue">22</td>
                                                                    </tr>

                                                                </table>
                                                            </td>
                                                        </tr>


                                                    </table>
                                                </td>

                                                <td class="right1"></td>

                                            </tr>
                                        </table>

                                    </div>
                                </div>

                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Pnl_feearea" runat="server" DefaultButton="Btn_payfee" Style="width: 900px; padding-left: 120px">
                                <div class="container skin1" style="width: 1000px;">
                                    <table cellpadding="0" cellspacing="0" class="containerTable">
                                        <tr>
                                            <td class="no"></td>
                                            <td class="n">

                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">Fee Due 
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                </table>





                                            </td>
                                            <td class="ne"></td>
                                        </tr>
                                        <tr>
                                            <td class="o"></td>
                                            <td class="c">

                                                <table width="100%">
                                                    <tr>
                                                        <td><b>
                                                            <asp:Label ID="Label_AdvanceMsg" runat="server" class="control-label" Text="Total Advance : "></asp:Label>
                                                            <asp:Label ID="Label_TotalAdvance" runat="server" class="control-label" Text="0" ForeColor="#FF3300"></asp:Label>

                                                            &nbsp;
                          &nbsp;
                          
                    <asp:Label ID="Label_TransportationMsg" runat="server" class="control-label" Text="Transportation Route : " Visible="false"></asp:Label>
                                                            <asp:Label ID="Label_TransportationRoute" runat="server" class="control-label" Text="Route" ForeColor="#FF3300" Visible="false"></asp:Label>
                                                        </b>

                                                        </td>
                                                        <td style="text-align: right">

                                                            <asp:CheckBox ID="Chk_AllFees" runat="server" Text="All Fees" AutoPostBack="true"
                                                                OnCheckedChanged="Chk_AllFees_CheckedChanged" />

                                                            &nbsp;
				
				  <asp:Button ID="Btn_advanceSettelment" runat="server" ToolTip="Advance Settlement" class="btn btn-primary"
                      Text="Advance Settlement" OnClick="Btn_advanceSettelment_Click"></asp:Button>&nbsp;&nbsp;&nbsp;
				  <asp:Button ID="Lnk_Advance" runat="server" Class="btn btn-primary" OnClick="Lnk_Advance_Click" Text="Advance"></asp:Button>&nbsp;&nbsp;&nbsp;
				     	    <asp:Button ID="Lnk_OtherFee" runat="server" Class="btn btn-primary" OnClick="Lnk_OtherFee_Click" Text="OtherFee"></asp:Button>&nbsp;&nbsp;&nbsp;
				   	        <asp:Button ID="Link_CollectFee" runat="server" Class="btn btn-danger" OnClick="Link_CollectFee_Click" Text="Back"></asp:Button>

                                                        </td>
                                                    </tr>

                                                </table>
                                               
                                                
                                                <br />
                                                <asp:Panel ID="Panel_FeeDataArea" runat="server" Visible="true">

                                                    <div style="overflow: auto; max-height: 275px; width: 920px;" id="Div_Particular">
                                                        <asp:GridView ID="GridViewAllFee" runat="server" AutoGenerateColumns="False"
                                                            CellPadding="3" CellSpacing="2" ForeColor="Black" GridLines="Vertical" Width="900px"
                                                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">

                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBoxUpdate" runat="server" onclick="Calculate()" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)" />
                                                                    </HeaderTemplate>

                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SchId" HeaderText="Scheduled ID" />
                                                                <asp:BoundField DataField="Id" HeaderText="Student Fee ID" />
                                                                <asp:BoundField DataField="AccountName" HeaderText="Fee Name" />
                                                                <asp:BoundField DataField="BatchName" HeaderText="Batch" ItemStyle-Width="80" />
                                                                <asp:BoundField DataField="Period" HeaderText="For Period" ItemStyle-Width="80" />
                                                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="80" />
                                                                <asp:BoundField DataField="BalanceAmount" HeaderText="Balance" ItemStyle-Width="80" />
                                                                <asp:BoundField DataField="LastDate" HeaderText="Last Date" ItemStyle-Width="80" />
                                                                <asp:TemplateField HeaderText="Deduction" ItemStyle-Width="80">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TxtDeduction" runat="server" onkeyup="Calculate()" class="form-control"
                                                                            MaxLength="8" Text="0" Width="75"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="TxtDeduction_FilteredTextBoxExtender0"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                            TargetControlID="TxtDeduction" ValidChars=".">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Arrear" ItemStyle-Width="80">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="Txtarrier" runat="server" MaxLength="8" onkeyup="Calculate()" class="form-control" Enabled="false"
                                                                            Text="0" Width="75"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txtarrier_FilteredTextBoxExtender0"
                                                                            runat="server" Enabled="true" FilterType="Custom, Numbers"
                                                                            TargetControlID="Txtarrier" ValidChars=".">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Fine" ItemStyle-Width="80">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TxtFine" runat="server" MaxLength="8" onkeyup="Calculate()" class="form-control"
                                                                            Text="0" Width="75"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="TxtFine_FilteredTextBoxExtender0"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                            TargetControlID="TxtFine" ValidChars=".">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount Paid" ItemStyle-Width="80">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="Txtamntpaid" runat="server" MaxLength="8" onkeyup="Calculate()" class="form-control"
                                                                            Text="0" Width="75"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txtamntpaid_FilteredTextBoxExtender0"
                                                                            runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                            TargetControlID="Txtamntpaid" ValidChars=".">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Regular" HeaderText="Regular" />
                                                                <asp:BoundField DataField="CollectionType" HeaderText="CollectionType" />
                                                                <asp:BoundField DataField="PeriodId" HeaderText="Period" />
                                                                <asp:BoundField DataField="FeeId" HeaderText="FeeId" />
                                                                <asp:BoundField DataField="BatchId" HeaderText="BatchId" />
                                                                <asp:BoundField DataField="Duedate" HeaderText="Due date" />
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                HorizontalAlign="Left" />
                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                HorizontalAlign="Left" />

                                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                            <EditRowStyle Font-Size="Medium" />
                                                        </asp:GridView>
                                                        <br />
                                                    </div>

                                                    <br />
                                                    <div>

                                                        <table width="100%" cellspacing="5">

                                                            <tr id="tax_area" runat="server" visible="false">
                                                                <td style="width: 75%" align="right">
                                                                    <asp:Label ID="lbl_tax" runat="server" Text="Tax:" Width="125px" class="control-label"></asp:Label>
                                                                </td>
                                                                <td style="width: 25%" align="left">
                                                                    <asp:DropDownList ID="Drp_Tax" runat="server" AutoPostBack="True" class="form-control"
                                                                         OnSelectedIndexChanged="Drp_Tax_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             <tr id="tax_amnt" runat="server" visible="false">
                                                                <td style="width: 75%" align="right">
                                                                    <asp:Label ID="lbl_Taxamnt" runat="server" Text="Tax Amount:" Width="125px" class="control-label"></asp:Label>
                                                                </td>
                                                                <td style="width: 25%" align="left">
                                                                      <input id="Txt_TaxAmnt" runat="server" onkeydown="return false" class="form-control"
                                                                        style="background-color: #CCCCCC; color: Black; border: Double 1px black"
                                                                        type="text" />
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </div>
                                <table width="100%" cellspacing="5">
                                    <tr>
                                        <td style="width: 25%">

                                            <u>Mode Of Payment </u>

                                        </td>
                                        <td style="width: 25%">

                                            <u>Other Reference </u>

                                        </td>
                                        <td style="width: 25%" align="right">
                                            <asp:Label ID="LblAmount" runat="server" Text="Total Amount:" Width="125px" class="control-label"></asp:Label>
                                        </td>
                                        <td style="width: 25%" align="left">
                                            <input id="TxtTotatAmount" runat="server" onkeydown="return false" class="form-control"
                                                style="background-color: #CCCCCC; color: Black; border: Double 1px black"
                                                type="text" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton0" runat="server" Checked="true"
                                                onclick="Rdb_Changing0()" Text="Cash" />
                                        </td>
                                        <td rowspan="4">
                                            <asp:TextBox ID="Txt_OtherReference" runat="server" TextMode="MultiLine" class="form-control" MaxLength="240" Width="100%" Height="60px"></asp:TextBox>
                                        </td>
                                        <td align="right">Balance:</td>
                                        <td align="left">
                                            <input id="Txt_Balance" runat="server" onkeydown="return false" class="form-control"
                                                style="background-color: #CCCCCC; color: Black; border: Double 1px black"
                                                type="text" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton1" runat="server" Checked="false"
                                                onclick="Rdb_Changing1()" Text="Cheque" />
                                        </td>

                                        <td align="right">Amount Paid</td>
                                        <td align="left">
                                            <asp:TextBox ID="Txt_AmountPaying" runat="server" class="form-control"
                                                onkeyup="CalculateAmountPaying()"></asp:TextBox>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_AmountPaying_TextBoxWatermarkExtender"
                                                runat="server" Enabled="True" TargetControlID="Txt_AmountPaying"
                                                WatermarkText="Enter Amount">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton2" runat="server" Checked="false"
                                                onclick="Rdb_Changing2()" Text="Demand Draft" />
                                        </td>

                                        <td align="right">Date</td>
                                        <td align="left">
                                            <asp:TextBox ID="Txt_Pay_Date" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="Txt_Pay_Date_CalendarExtender" runat="server"
                                                CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy"
                                                TargetControlID="Txt_Pay_Date">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="PayDateRegularExpressionValidator3"
                                                runat="server" ControlToValidate="Txt_Pay_Date" Display="None"
                                                ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                                TargetControlID="PayDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton3" runat="server" Checked="false"
                                                onclick="Rdb_Changing3()" Text="NEFT" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">

                                            <asp:Panel ID="Pnl_paymod" runat="server" Width="878px" CssClass="PanelStyle">
                                                <table width="100%" cellspacing="0">
                                                    <tr>
                                                        <td class="style17" align="right">
                                                            <%--<asp:Label ID="Lbl_Id" runat="server" Text="Label"></asp:Label>--%>
                                                            <asp:TextBox ID="TextBoxLabel" runat="server" BorderStyle="None" Width="100px" ReadOnly="true"></asp:TextBox>

                                                            <asp:Label ID="Lbl_Mand1" runat="server" Text="*" ForeColor="Red" class="control-label"></asp:Label>
                                                        </td>
                                                        <td class="textarea" align="left">
                                                            <asp:TextBox ID="Txt_paymentid" runat="server" MaxLength="40" class="form-control"></asp:TextBox>
                                                        </td>
                                                        <td class="style17" align="right">Bank Name 
                                                                                <asp:Label ID="Lbl_Mand2" runat="server" Text="*" ForeColor="Red" class="control-label"></asp:Label></td>
                                                        <td class="textarea" align="left">
                                                            <asp:TextBox ID="Txt_bank" runat="server" MaxLength="90" Width="300px" class="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="color: red; font-size: medium;">Note : Balance should be zero for the payment of the fee.
                                        </td>
                                    </tr>
                                </table>

                            </asp:Panel>
                            <asp:Panel ID="Panel_Complete" runat="server" Visible="false">


                                <table width="100%">
                                    <tr>
                                        <td style="font-size: 16px; width: 100%" align="center">No more due present for the student
            
            <br />
                                            <br />
                                        </td>

                                    </tr>
                                </table>


                            </asp:Panel>
                            <div class="linestyle"></div>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txt_bank"
                                runat="server" Enabled="True" TargetControlID="Txt_paymentid" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Filtered_Txtbank"
                                runat="server" Enabled="True" TargetControlID="Txt_bank" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>

                            <table class="style22">
                                <tr>
                                    <td class="style23">&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Lbl_FeeBillMessage" runat="server" ForeColor="Red" class="control-label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        <asp:TextBox ID="TxtBillNo" runat="server" Visible="False" class="form-control"></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="Btn_payfee" runat="server" OnClientClick="return ValidatePay()" OnClick="Btn_payfee_Click"
                                            Text="Pay Fee" Class="btn btn-success" />
                                        &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnBill" runat="server" OnClick="BtnBill_Click" Text="View Bill" Class="btn btn-primary" />
                                        &nbsp;&nbsp;&nbsp;
                             <asp:Button ID="BtnCancel" runat="server" OnClick="BtnCancel_Click"
                                 Text="Cancel Bill" Class="btn btn-danger" Enabled="False" />
                                        <ajaxToolkit:ConfirmButtonExtender ID="BtnCancel_ConfirmButtonExtender" OnClientCancel="CancelBill" ConfirmText="Are you sure you want to cancel the bill"
                                            runat="server" Enabled="True" TargetControlID="BtnCancel">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                    </td>
                                </tr>
                            </table>

                            <br />

                        </td>
                        <td class="e"></td>
                    </tr>
                    <tr>
                        <td class="so"></td>
                        <td class="s"></td>
                        <td class="se"></td>
                    </tr>
                </table>
                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>








                <asp:Panel ID="Pnl_MessageError" runat="server">

                    <asp:Button runat="server" ID="Btn_hiddentratet2" class="btn btn-info" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="MPE_MessageError" BackgroundCssClass="modalBackground"
                        runat="server" CancelControlID="Btn__MessageError_ok"
                        PopupControlID="Pnl_MessageError_ok" TargetControlID="Btn_hiddentratet2" />
                    <asp:Panel ID="Pnl_MessageError_ok" runat="server" Style="display: none;">
                        <div class="container skinAlert" style="width: 400px; top: 400px; left: 400px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr>
                                    <td class="no"></td>
                                    <td class="n"><span style="color: White; font-size: 18px"><b>alert!</b></span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="o"></td>
                                    <td class="c">

                                        <asp:Label ID="Lbl_MessageError" runat="server" Text="" class="control-label"></asp:Label><br />
                                        <br />
                                        <div style="text-align: center;">

                                            <asp:Button ID="Btn__MessageError_ok" runat="server" class="btn btn-info" Text="OK" Width="50px" />
                                        </div>
                                    </td>
                                    <td class="e"></td>
                                </tr>
                                <tr>
                                    <td class="so"></td>
                                    <td class="s"></td>
                                    <td class="se"></td>
                                </tr>
                            </table>
                            <br />
                            <br />



                        </div>
                    </asp:Panel>
                </asp:Panel>

                <WC:MSGBOX ID="WC_MessageBox" runat="server" />

                <asp:Panel ID="Pnl_CancelBill" runat="server">
                    <asp:Button runat="server" ID="Btn_Cancel" class="btn btn-danger" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="MPE_CancelBill" runat="server" CancelControlID="Btn_NewCancelCancel" BackgroundCssClass="modalBackground"
                        PopupControlID="Pnl_Bill" TargetControlID="Btn_Cancel" />
                    <asp:Panel ID="Pnl_Bill" runat="server" Style="display: none;">
                        <div class="container skin5" style="width: 400px; top: 400px; left: 400px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr>
                                    <td class="no"></td>
                                    <td class="n"><span style="color: White">Message</span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="o"></td>
                                    <td class="c">
                                        <br />
                                        <asp:Label ID="Lbl_BillMessage" runat="server" class="control-label" Text=""></asp:Label><div style="text-align: center">
                                            Reason : 
                                        <asp:TextBox ID="Txt_CancelReason" runat="server" Width="160" Height="30" TextMode="MultiLine" class="form-control" MaxLength="100"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="Txt_CancelReason_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom"
                                            InvalidChars="'/\" TargetControlID="Txt_CancelReason">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="CancelFeeBillReason" ControlToValidate="Txt_CancelReason" ErrorMessage="Enter Reason"></asp:RequiredFieldValidator>
                                        </div>
                                        <br />
                                        <div style="text-align: center;">
                                            <asp:Button ID="Btn_CancelBill" OnClick="Btn_CancelBill_Click" runat="server" Text="Ok" ValidationGroup="CancelFeeBillReason" Class="btn btn-info" />

                                            <asp:Button
                                                ID="Btn_NewCancelCancel" runat="server" Text="Cancel" Class="btn btn-info" />
                                        </div>
                                    </td>
                                    <td class="e"></td>
                                </tr>
                                <tr>
                                    <td class="so"></td>
                                    <td class="s"></td>
                                    <td class="se"></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </div>
                    </asp:Panel>
                </asp:Panel>


                <WC:OTHERFEE ID="OtherFeeBox" runat="server" />

                <WC:FEEADVANCE ID="FeeAdvanceBox" runat="server" />
                <WC:SETTLEADVANCE ID="WC_SETTLEADVANCE" runat="server" />


            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="clear"></div>


    </div>
</asp:Content>


