<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ManageStudentControl.ascx.cs" Inherits="WinEr.WebControls.ManageStudentControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_SearchSiblings.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
        // Calculate();
    }

    function SelectTrans(cbtransAll) {
        var gridViewCtl = document.getElementById('<%=Grd_TransFee.ClientID%>');
        var Status = cbtransAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
        // Calculate();
    }
    function openIncpopup(strOpen) {
        open(strOpen, "Info", "status=1, width=600, height=400,resizable = 1");
    }
         </script>
<asp:HiddenField ID="Hdn_StudentID" runat="server" Value="0" />
        
      <div class="card0" style="width: 100%;">
        <table class="containerTable" style="width:100%;">
         <%--   <tr >
                <td class="no"> </td>
                <td class="n">Manage Student</td>
                <td class="ne"> </td>
            </tr>--%>
            <tr >
                <td class="o"> </td>
                <td class="c" >                                 
                       <asp:Panel ID="Panel1" runat="server">
                  

                          
                           <asp:Panel ID="Pnl_userdetailstabarea" runat="server">
                           <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  ActiveTabIndex="0"
                        CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True" >
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Promotion" >
                <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/user4.png" /><b>GENERAL</b></HeaderTemplate>         
                

           <ContentTemplate> 
                       <asp:UpdateProgress ID="UpdateProgress3" runat="server" 
                           AssociatedUpdatePanelID="UpdatePanel2">
                           <ProgressTemplate>
                               <div ID="progressBackgroundFilter">
                               </div>
                               <div ID="processMessage">
                                   <table style="height:100%;width:100%">
                                       <tr>
                                           <td align="center">
                                               <b>Please Wait...</b><br />
                                               <br />
                                               <img alt="" src="images/indicator-big.gif" /></td>
                                       </tr>
                                   </table>
                               </div>
                           </ProgressTemplate>
                       </asp:UpdateProgress>
                       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                           <ContentTemplate>
                               <asp:Panel ID="Pnl_basicDetails" runat="server" 
                                   DefaultButton="Btn_UpdateGeneraldetails">
                                   <div class="linestyle">
                                   </div>
                                   <table class="tablelist">
                                       <tr>
                                           <td>
                                               &nbsp;</td>
                                           <td>
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Student Name:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_Name" runat="server" CausesValidation="True" 
                                                   MaxLength="50" TabIndex="1" Width="160px" class="form-control"></asp:TextBox>
                                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                                   Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                                   InvalidChars="1234567890!@#$%^&amp;*()_+=-{}][|';:\" TargetControlID="Txt_Name">
                                               </cc1:FilteredTextBoxExtender>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                   ControlToValidate="Txt_Name" ErrorMessage="You Must enter a name"></asp:RequiredFieldValidator>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Sex:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                           <div class="radio radio-primary">
                                               <asp:RadioButtonList ID="RadioBtn_Sex" runat="server" 
                                                   RepeatDirection="Horizontal" TabIndex="2" Width="160px">
                                                   <asp:ListItem Selected="True">Male</asp:ListItem>
                                                   <asp:ListItem>Female</asp:ListItem>
                                               </asp:RadioButtonList>
                                               </div>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               D.O.B:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_Dob" runat="server" TabIndex="3" Width="160px" class="form-control"></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                   ControlToValidate="Txt_Dob" ErrorMessage="You Must enter D.O.B"></asp:RequiredFieldValidator>
                                               <cc1:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" 
                                                   CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy" 
                                                   TargetControlID="Txt_Dob">
                                               </cc1:CalendarExtender>
                                               <%--<asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                ControlToValidate="Txt_Dob"
                                Display="None" 
                                ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                               <asp:RegularExpressionValidator ID="DobDateRegularExpressionValidator3" 
                                                   runat="server" ControlToValidate="Txt_Dob" Display="None" 
                                                   ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                               <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" 
                                                   Enabled="True" HighlightCssClass="validatorCalloutHighlight" 
                                                   TargetControlID="DobDateRegularExpressionValidator3" />
                                               <br />
                                               <%--<asp:CompareValidator ID="Comparevalidator1" runat="server" ErrorMessage="The D.O.B must be less than Joining date"
   ControlToValidate="Txt_Dob" type="Date" Operator="GreaterThan"
   ControlToCompare="Txt_JoiningDate" />--%>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Father/Guardian Name:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_FGName" runat="server" MaxLength="45" TabIndex="4" class="form-control"
                                                   Width="160px"></asp:TextBox>
                                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                   Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                                   InvalidChars="1234567890!@#$%^&amp;*()_+=-{}][|';:\" 
                                                   TargetControlID="Txt_FGName">
                                               </cc1:FilteredTextBoxExtender>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                   ControlToValidate="Txt_FGName" 
                                                   ErrorMessage="You Must enter Father/Guardian name"></asp:RequiredFieldValidator>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Address(Permanent):<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_Address" runat="server" Height="77px" MaxLength="200" class="form-control"
                                                   TabIndex="5" TextMode="MultiLine" Width="160px"></asp:TextBox>
                                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" 
                                                   Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                                   TargetControlID="Txt_Address">
                                               </cc1:FilteredTextBoxExtender>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                   ControlToValidate="Txt_Address" 
                                                   ErrorMessage="You Must enter Address(Permanent)"></asp:RequiredFieldValidator>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Joining Batch:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_JoinBatch" runat="server" TabIndex="6" class="form-control" Width="164px">
                                               </asp:DropDownList>
                                           </td>
                                       </tr>
                                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                       <tr>
                                           <td class="leftside">
                                               Joining Standard:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_JoinStandard" runat="server" TabIndex="6" class="form-control"
                                                   Width="164px">
                                               </asp:DropDownList>
                                           </td>
                                       </tr>
                                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                       <tr>
                                           <td class="leftside">
                                               Date of Admission:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_JoiningDate" runat="server" TabIndex="7" Width="160px" class="form-control"></asp:TextBox>
                                               <cc1:CalendarExtender ID="Txt_JoiningDate_CalendarExtender" runat="server" 
                                                   CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy" 
                                                   TargetControlID="Txt_JoiningDate">
                                               </cc1:CalendarExtender>
                                               <%--<asp:RegularExpressionValidator runat="server" ID="DoJDateRegularExpressionValidator3"
                                ControlToValidate="Txt_JoiningDate"
                                Display="None"
                                ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />--%>
                                               <asp:RegularExpressionValidator ID="DoJDateRegularExpressionValidator3" 
                                                   runat="server" ControlToValidate="Txt_JoiningDate" Display="None" 
                                                   ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                               <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" 
                                                   Enabled="True" HighlightCssClass="validatorCalloutHighlight" 
                                                   TargetControlID="DoJDateRegularExpressionValidator3" />
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                   ControlToValidate="Txt_JoiningDate" 
                                                   ErrorMessage="You Must enter Date of Admission"></asp:RequiredFieldValidator>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Religion:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_Religion" runat="server" AutoPostBack="True" class="form-control"
                                                   TabIndex="8" Width="160px">
                                                   <%--onselectedindexchanged="Drp_Religion_SelectedIndexChanged" --%>
                                               </asp:DropDownList>
                                           </td>
                                       </tr>
                                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                       
                                       <tr>
                                           <td class="leftside">
                                               Caste:<span class="redcol">*</span></td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_Caste" runat="server" TabIndex="9" Width="160px" class="form-control">
                                               </asp:DropDownList>
                                           </td>
                                       </tr>
                                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                       <%--<tr>
                                <td class="leftside">
                                    <asp:Label ID="Lbl_newCaste" runat="server" Text="New Caste" Visible="false"></asp:Label>
                                </td>
                                <td class="rightside">
                                    <asp:TextBox ID="Txt_NewCaste" runat="server" Visible="false" Width="160px" MaxLength="30"></asp:TextBox>
                                </td>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewCaste_FilteredTextBoxExtender" 
                                      runat="server" Enabled="True" TargetControlID="Txt_NewCaste" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender> 
                            </tr>--%>
                                       <tr>
                                           <td class="leftside">
                                               AdmissionNo:<span class="redcol">*</span> &nbsp; &nbsp; &nbsp;
                                           </td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_AdmissionNo" runat="server" MaxLength="30" TabIndex="9" class="form-control"
                                                   Width="160px"></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="Txt_AdmissionNo_RequiredFieldValidator" 
                                                   runat="server" ControlToValidate="Txt_AdmissionNo" 
                                                   ErrorMessage="You Must enter Admission number"></asp:RequiredFieldValidator>
                                               <asp:HiddenField ID="Hdn_AdmissionNo" runat="server" />
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="leftside">
                                               Class</td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_Class" runat="server" Width="160px" class="form-control">
                                               </asp:DropDownList>
                                               <asp:HiddenField ID="Hdn_ClassId" runat="server" />
                                           </td>
                                       </tr>
                                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                       <tr>
                                           <td class="leftside">
                                               Student Id
                                               <asp:Label ID="Lbl_Manstudid" runat="server" ForeColor="Red" Text="" class="control-label"></asp:Label>
                                           </td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_StudentId" runat="server" MaxLength="30" TabIndex="9" class="form-control" 
                                                   Width="160px"></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                                   ControlToValidate="Txt_StudentId"></asp:RequiredFieldValidator>
                                               <asp:HiddenField ID="Hdn_StudMandatory" runat="server" />
                                           </td>
                                       </tr>
                                       
                                       
                                       
                                       
                                       
                                       
                                       
                                       <tr>
                                           <td>
                                               &nbsp;</td>
                                           <td>
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <asp:Button ID="Btn_UpdateGeneraldetails" runat="server" Class="btn btn-primary" 
                                                   onclick="Btn_UpdateGeneraldetails_Click" Text="Update" />
                                               </td>
                                       </tr>
                                   </table>
                                   <div class="linestyle">
                                   </div>
                               </asp:Panel>
                               <div>
                                   <asp:Button ID="Btn_hdnmessagetgt" runat="server" style="display:none" class="btn btn-primary"/>
                                   <cc1:ModalPopupExtender ID="MPE_MessageBox" runat="server" 
                                       BackgroundCssClass="modalBackground" CancelControlID="Btn_magok" 
                                       PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
                                   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                                       <div class="container skin5" style="width:400px; top:400px;left:400px">
                                           <table cellpadding="0" cellspacing="0" class="containerTable">
                                               <tr>
                                                   <td class="no">
                                                   </td>
                                                   <td class="n">
                                                       <span style="color:White">alert!</span></td>
                                                   <td class="ne">
                                                       &nbsp;</td>
                                               </tr>
                                               <tr>
                                                   <td class="o">
                                                   </td>
                                                   <td class="c"> 
                                                       <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <div style="text-align:center;">
                                                           <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-primary"/>
                                                       </div>
                                                   </td>
                                                   <td class="e">
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td class="so">
                                                   </td>
                                                   <td class="s">
                                                   </td>
                                                   <td class="se">
                                                   </td>
                                               </tr>
                                           </table>
                                           <br />
                                           <br />
                                       </div>
                                   </asp:Panel>
                               </div>
                           </ContentTemplate>
                           <Triggers>
                               <asp:PostBackTrigger ControlID="Btn_UpdateGeneraldetails" />
                           </Triggers>
                       </asp:UpdatePanel>
</ContentTemplate>  
                

</ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Promotion"  >
                <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>OTHERS</b></HeaderTemplate>                 
           <ContentTemplate>
           
               <asp:UpdateProgress ID="UpdateProgress2" runat="server" 
                   AssociatedUpdatePanelID="UpdatePanel1">
                   <ProgressTemplate>
                       <div ID="progressBackgroundFilter">
                       </div>
                       <div ID="processMessage">
                           <table style="height:100%;width:100%">
                               <tr>
                                   <td align="center">
                                       <b>Please Wait...</b><br />
                                       <br />
                                       <img alt="" src="images/indicator-big.gif" /></td>
                               </tr>
                           </table>
                       </div>
                   </ProgressTemplate>
               </asp:UpdateProgress>
               <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                   <ContentTemplate>
                       <asp:Panel ID="Pnl_otherdetails" runat="server" 
                           DefaultButton="Btn_updateotherdetails">
                           <div class="newsubheading">
                               Personal details&nbsp;&nbsp;
                           </div>
                           <div class="linestyle">
                           </div>
                           <table class="tablelist">
                               <tr>
                                   <td class="leftside">
                                   </td>
                                   <td class="rightside">
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td class="leftside">
                                       Using School Bus?</td>
                                   <td class="rightside">
                                   <div class="radio radio-primary">
                                       <asp:RadioButtonList ID="Rdb_NeedBus" runat="server" AutoPostBack="true" 
                                           OnSelectedIndexChanged="Rdb_NeedBus_SelectedIndexChanged" 
                                           RepeatDirection="Horizontal">
                                           <asp:ListItem Value="1"> Yes </asp:ListItem>
                                           <asp:ListItem Value="0"> No</asp:ListItem>
                                       </asp:RadioButtonList>
                                       </div>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Using Hostel?</td>
                                   <td class="rightside">
                                   <div class="radio radio-primary">
                                       <asp:RadioButtonList ID="Rdb_NeedHostel" runat="server" 
                                           RepeatDirection="Horizontal">
                                           <asp:ListItem Value="1"> Yes </asp:ListItem>
                                           <asp:ListItem Value="0"> No</asp:ListItem>
                                       </asp:RadioButtonList>
                                       </div>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Blood Group:</td>
                                   <td class="rightside">
                                       <asp:DropDownList ID="Drp_BloodGrp" runat="server" class="form-control" Width="160px">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                       <tr>
                                           <td class="leftside">
                                               Aadhar Number
                                               <asp:Label ID="Lbl_aadharno" runat="server" ForeColor="Red" Text="" class="control-label"></asp:Label>
                                           </td>
                                           <td class="rightside">
                                               <asp:TextBox ID="Txt_aadharno" runat="server" MaxLength="30" TabIndex="9" class="form-control" 
                                                   Width="160px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                           TargetControlID="Txt_aadharno"></cc1:FilteredTextBoxExtender>
                                           </td>
                                       </tr>
                               
                               
                               
                               
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Nationality:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_Nationality" runat="server" MaxLength="20" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                           TargetControlID="Txt_Nationality"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Mother Tongue:</td>
                                   <td class="rightside">
                                       <asp:DropDownList ID="Drp_MotherTongue" runat="server" Width="160px" class="form-control">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Mother&#39;s Name:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_MotherName" runat="server" MaxLength="45" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                           TargetControlID="Txt_MotherName"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Father&#39;s Educational Qualification:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_FatherEduQuali" runat="server" MaxLength="45" class="form-control"
                                           Width="160px"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                           TargetControlID="Txt_FatherEduQuali"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Mother&#39;s Educational &nbsp;Qualification:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_MotherEduQuali" runat="server" MaxLength="45" class="form-control"
                                           Width="160px"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                           TargetControlID="Txt_MotherEduQuali"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Father&#39;s Occupation:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_FatherOccupation" runat="server" MaxLength="45" class="form-control"
                                           Width="160px"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                           TargetControlID="Txt_FatherOccupation"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Mother&#39;s Occupation:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_MotherOccupation" runat="server" MaxLength="45" class="form-control"
                                           Width="160px"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                           TargetControlID="Txt_MotherOccupation"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Annual Income:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_AnualIncome" runat="server" MaxLength="10" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="AnualIncome1" runat="server" Enabled="True" 
                                           FilterType="Numbers" TargetControlID="Txt_AnualIncome"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Address (Present):</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_Address_Present" runat="server" Height="77px" class="form-control"
                                           MaxLength="200" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="Txt_Address_Present_FilteredTextBoxExtender" 
                                           runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                           TargetControlID="Txt_Address_Present"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Location:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_Location" runat="server" MaxLength="45" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                           TargetControlID="Txt_Location"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       State:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_State" runat="server" MaxLength="20" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" 
                                           Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                           TargetControlID="Txt_State"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Pin Code:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_pin" runat="server" MaxLength="7" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="Txt_pin_FilteredTextBoxExtender" 
                                           runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_pin"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Residence Phone Number:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_ResidencePh" runat="server" MaxLength="14" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="ResidentPhone" runat="server" Enabled="True" 
                                           FilterType="Custom, Numbers" TargetControlID="Txt_ResidencePh" ValidChars="+"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Mobile Number:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_OfficePh" runat="server" MaxLength="10" Width="160px" class="form-control"></asp:TextBox>
                                       <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                           ControlToValidate="Txt_OfficePh" ErrorMessage="Invalid number" 
                                           SetFocusOnError="true" ValidationExpression="\d{10}|d{0}" 
                                           ValidationGroup="SaveOtherData"></asp:RegularExpressionValidator>
                                       <cc1:FilteredTextBoxExtender ID="OfficePh" runat="server" Enabled="True" 
                                           FilterType="Custom, Numbers" TargetControlID="Txt_OfficePh" ValidChars=""></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Secondary Mobile Number:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_SecondaryPh" runat="server" MaxLength="10" Width="160px" class="form-control"></asp:TextBox>
                                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                           ControlToValidate="Txt_SecondaryPh" ErrorMessage="Invalid number" 
                                           SetFocusOnError="true" ValidationExpression="\d{10}|d{0}" 
                                           ValidationGroup="SaveOtherData"></asp:RegularExpressionValidator>
                                       <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" 
                                           Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_SecondaryPh" 
                                           ValidChars=""></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Email :</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_Email" runat="server" MaxLength="45" Width="160px" class="form-control"></asp:TextBox>
                                       <asp:RegularExpressionValidator ID="PNRegEx" runat="server" 
                                           ControlToValidate="Txt_Email" Display="None" 
                                           ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Please E mail id in the currect format (xxx@xxx.xxx)" 
                                           ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})| d{0} $"
                                           ValidationGroup="SaveOtherData" />
                                       <cc1:ValidatorCalloutExtender ID="PNReqEx" runat="server" Enabled="True" 
                                           HighlightCssClass="validatorCalloutHighlight" TargetControlID="PNRegEx" />
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Number of Brothers:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_NoBro" runat="server" MaxLength="5" Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="Txt_NoBro_FilteredTextBoxExtender1" 
                                           runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_NoBro"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Number of Sisters:</td>
                                   <td class="rightside">
                                       <asp:TextBox ID="Txt_NoSys" runat="server"  MaxLength="5" 
                                           Width="160px" class="form-control"></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender ID="Txt_NoSys_FilteredTextBoxExtender1" 
                                           runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_NoSys"></cc1:FilteredTextBoxExtender>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       1st Language Wishes to take:</td>
                                   <td class="rightside">
                                       <asp:DropDownList ID="Drp_FirstLanguage" runat="server" Width="160px" class="form-control">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                       Student Category:</td>
                                   <td class="rightside">
                                       <asp:DropDownList ID="Drp_StudentType" runat="server" Width="160px" class="form-control">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                               <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                               <tr>
                                   <td class="leftside">
                                   </td>
                                   <td>
                                       <asp:CheckBox ID="Chk_NewAdmin" runat="server" Text="New admission" />
                                   </td>
                               </tr>
                               <tr>
                                   <td class="leftside">
                                       &nbsp;</td>
                                   <td>
                                       &nbsp;</td>
                               </tr>
                           </table>
                           <asp:Panel ID="Pnl_SiblingsDetails" runat="server">
                               <table class="tablelist">
                                   <tr>
                                       <td align="center">
                                           <asp:Label ID="Lbl_Sib" runat="server" class="control-label"></asp:Label>
                                       </td>
                                       <td class="rightside">
                                       </td>
                                   </tr>
                                   <tr>
                                       <td align="center" colspan="2">
                                           <asp:Panel ID="Pnl_SibDisplay" runat="server">
                                               <asp:GridView ID="GrdSiblings" runat="server" AutoGenerateColumns="false" 
                                                   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                                   ForeColor="Black" GridLines="Vertical" 
                                                   onselectedindexchanged="GrdSiblings_SelectedIndexChanged" Width="500px">
                                                   <Columns>
                                                       <asp:BoundField DataField="Id" HeaderText="Id" />
                                                       <asp:BoundField DataField="StudentName" HeaderText="Name" />
                                                       <asp:BoundField DataField="GardianName" HeaderText="Guardian Name" />
                                                       <asp:CommandField HeaderText="Delete" 
                                                           SelectText="&lt;img src='Pics/Deletered.png' width='25px' border=0 title='Delete'&gt;" 
                                                           ShowSelectButton="True">
                                                           <ItemStyle Width="35px" />
                                                       </asp:CommandField>
                                                   </Columns>
                                               </asp:GridView>
                                               <br />
                                           </asp:Panel>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td class="leftside">
                                       </td>
                                       <td class="rightside">
                                           <asp:LinkButton ID="Lnk_AddSiblings" runat="server" 
                                               onclick="Lnk_AddSiblings_Click" Text="Add Siblings">
                            </asp:LinkButton>
                                       </td>
                                   </tr>
                               </table>
                           </asp:Panel>
                           <div class="linestyle">
                           </div>
                           <asp:Panel ID="Pnl_custumarea" runat="server">
                               <div class="newsubheading">
                                   Extra details&nbsp;&nbsp;
                               </div>
                               <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
                               <div class="linestyle">
                               </div>
                           </asp:Panel>
                           <table class="tablelist">
                               <tr>
                                   <td class="leftside">
                                       &nbsp;</td>
                                   <td class="rightside">
                                       <asp:Button ID="Btn_updateotherdetails" runat="server" Class="btn btn-primary" 
                                           onclick="Btn_updateotherdetails_Click" Text="Update" 
                                           ValidationGroup="SaveOtherData" />
                                   </td>
                               </tr>
                           </table>
                       </asp:Panel>
                       <div>
                           <asp:Button ID="Btn_hdnmessagetgt1" runat="server" style="display:none" class="btn btn-primary" />
                           <cc1:ModalPopupExtender ID="MPE_MessageBox1" runat="server" 
                               BackgroundCssClass="modalBackground" CancelControlID="Btn_magok1" 
                               PopupControlID="Pnl_msg1" TargetControlID="Btn_hdnmessagetgt1" />
                           <asp:Panel ID="Pnl_msg1" runat="server" style="display:none;">
                               <div class="container skin5" style="width:400px; top:400px;left:400px">
                                   <table cellpadding="0" cellspacing="0" class="containerTable">
                                       <tr>
                                           <td class="no">
                                           </td>
                                           <td class="n">
                                               <span style="color:White">alert!</span></td>
                                           <td class="ne">
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td class="o">
                                           </td>
                                           <td class="c">
                                               <asp:Label ID="Lbl_msg1" runat="server" Text="" class="control-label"></asp:Label>
                                               <br />
                                               <br />
                                               <div style="text-align:center;">
                                                   <asp:Button ID="Btn_magok1" runat="server" Text="OK" Width="50px" class="btn btn-success"/>
                                               </div>
                                           </td>
                                           <td class="e">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="so">
                                           </td>
                                           <td class="s">
                                           </td>
                                           <td class="se">
                                           </td>
                                       </tr>
                                   </table>
                                   <br />
                                   <br />
                               </div>
                           </asp:Panel>
                       </div>
                       <div>
                           <asp:Button ID="Button1" runat="server" style="display:none" class="btn btn-primary"/>
                           <cc1:ModalPopupExtender ID="MPE_ADDSIBLINGS" runat="server" 
                               CancelControlID="Btn_Cancel" PopupControlID="Pnl_AddSiblingsPopup" 
                               TargetControlID="Button1" />
                           <asp:Panel ID="Pnl_AddSiblingsPopup" runat="server" style="display:none;">
                               <%-- style="display:none;"--%> &nbsp; &nbsp; &nbsp;
                               <div class="container skin5" style="width:600px; top:600px;left:200px">
                                   <table cellpadding="0" cellspacing="0" class="containerTable">
                                       <tr>
                                           <td class="no">
                                           </td>
                                           <td class="n">
                                               <span style="color:White">Search!</span></td>
                                           <td class="ne">
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td class="o">
                                           </td>
                                           <td class="c">
                                               <asp:Panel ID="Pnl_AddSibInitial" runat="server">
                                                   <div style=" overflow:auto;min-height:100px">
                                                       <table width="100%">
                                                           <tr>
                                                               <td>
                                                                   Student Name&nbsp;&nbsp;
                                                                   <asp:TextBox ID="Txt_StudentName" runat="server" class="form-control"> </asp:TextBox>
                                                                   <%--<cc1:AutoCompleteExtender ID="Txt_StudentName_AutoCompleteExtender" runat="server" 
                                                                       DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                                                       ServiceMethod="GetStudentNameData" ServicePath="~/WinErWebService.asmx" 
                                                                       TargetControlID="Txt_StudentName" UseContextKey="true"></cc1:AutoCompleteExtender>--%>
                                                                       
                                                                  <ajaxToolkit:AutoCompleteExtender ID="Txt_StudentName_AutoCompleteExtender" runat="server"
                                                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetStudentNameData" ServicePath="~/WinErWebService.asmx"
                                                                        UseContextKey="false" TargetControlID="Txt_StudentName" MinimumPrefixLength="1">
                                                                    </ajaxToolkit:AutoCompleteExtender>     
                                                                       
                                                                       
                                                                       
                                                               </td>
                                                               <td>
                                                                   Parent Name&nbsp;&nbsp;
                                                                   <asp:TextBox ID="Txt_ParentName" runat="server" class="form-control"> </asp:TextBox>
                                                                   <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" 
                                                                       DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                                                       ServiceMethod="GetGurardianNameData" ServicePath="~/WinErWebService.asmx" 
                                                                       TargetControlID="Txt_ParentName" UseContextKey="true"></cc1:AutoCompleteExtender>
                                                               </td>
                                                               <td>
                                                                   Phone No:&nbsp;&nbsp;
                                                                   <asp:TextBox ID="Txt_PhoneNum" runat="server" class="form-control"> </asp:TextBox>
                                                                   <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" 
                                                                       DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                                                       ServiceMethod="GetPhoneNumberData" ServicePath="~/WinErWebService.asmx" 
                                                                       TargetControlID="Txt_PhoneNum" UseContextKey="true"></cc1:AutoCompleteExtender>
                                                                   <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                                       Enabled="True" FilterType="Numbers" TargetControlID="Txt_PhoneNum"></cc1:FilteredTextBoxExtender>
                                                               </td>
                                                           </tr>
                                                           <tr>
                                                               <td>
                                                               </td>
                                                               <td>
                                                               </td>
                                                               <td>
                                                                   <asp:Button ID="Btn_Search" runat="server" onclick="Btn_Search_Click" class="btn btn-primary"
                                                                       Text="Search" Width="100px" />
                                                               </td>
                                                           </tr>
                                                           <tr>
                                                               <td align="center" colspan="3">
                                                                   <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label>
                                                               </td>
                                                           </tr>
                                                           <tr>
                                                               <td align="center" colspan="3">
                                                                   <asp:Panel ID="Pnl_SearchSiblingsDisplay" runat="server">
                                                                       <asp:GridView ID="Grd_SearchSiblings" runat="server" AllowPaging="true" 
                                                                           AutoGenerateColumns="false" BorderColor="#DEDFDE" BorderStyle="None" 
                                                                           BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                                                                           onpageindexchanging="Grd_SearchSiblings_PageIndexChanging" PageSize="5" 
                                                                           Width="500px">
                                                                           <Columns>
                                                                               <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                                                                                   ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40">
                                                                                   <HeaderTemplate>
                                                                                       <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" 
                                                                                           OnClick="SelectAll(this)" Text=" All" />
                                                                                   </HeaderTemplate>
                                                                                   <ItemTemplate>
                                                                                       <asp:CheckBox ID="CheckBoxUpdate" runat="server" />
                                                                                   </ItemTemplate>
                                                                                   <HeaderStyle HorizontalAlign="Left" />
                                                                                   <ItemStyle HorizontalAlign="Left" Width="40px" />
                                                                               </asp:TemplateField>
                                                                               <asp:BoundField DataField="Id" HeaderText="Id" />
                                                                               <asp:BoundField DataField="StudentName" HeaderText="Name" />
                                                                               <asp:BoundField DataField="GardianName" HeaderText="Guardian Name" />
                                                                           </Columns>
                                                                       </asp:GridView>
                                                                       <br />
                                                                   </asp:Panel>
                                                               </td>
                                                           </tr>
                                                       </table>
                                                   </div>
                                               </asp:Panel>
                                               <br />
                                               <br />
                                               <div style="text-align:center;">
                                                   <asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" Text="Save" class="btn btn-success"
                                                       Width="90px" />
                                                   <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Width="90px" class="btn btn-danger" />
                                               </div>
                                           </td>
                                           <td class="e">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="so">
                                           </td>
                                           <td class="s">
                                           </td>
                                           <td class="se">
                                           </td>
                                       </tr>
                                   </table>
                                   <br />
                                   <br />
                               </div>
                           </asp:Panel>
                       </div>
                       <div>
                           <asp:Button ID="Button2" runat="server" style="display:none" class="btn btn-primary" />
                           <cc1:ModalPopupExtender ID="MPE_TRANSFEE" runat="server" 
                               CancelControlID="Btn_TransCancel" PopupControlID="Pnl_TansfeeAlert" 
                               TargetControlID="Button2" />
                           <asp:Panel ID="Pnl_TansfeeAlert" runat="server" style="display:none;">
                               <%-- style="display:none;"--%> &nbsp; &nbsp; &nbsp;
                               <div class="container skin5" style="width:600px; top:600px;left:200px">
                                   <table cellpadding="0" cellspacing="0" class="containerTable">
                                       <tr>
                                           <td class="no">
                                           </td>
                                           <td class="n">
                                               <span style="color:White">Alert!</span></td>
                                           <td class="ne">
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td class="o">
                                           </td>
                                           <td class="c">
                                               <asp:Label ID="Lbl_TransMsg" runat="server" Font-Bold="false" ForeColor="Red" class="control-label"
                                                   Text=""></asp:Label>
                                               <br />
                                               <br />
                                               <asp:GridView ID="Grd_TransFee" runat="server" AllowPaging="true" 
                                                   AutoGenerateColumns="false" BorderColor="#DEDFDE" BorderStyle="None" 
                                                   BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                                                   OnPageIndexChanging="Grd_SearchSiblings_PageIndexChanging" PageSize="6" 
                                                   Width="500px">
                                                   <Columns>
                                                       <asp:BoundField DataField="Id" HeaderText="Id" />
                                                       <asp:TemplateField HeaderText="transfee" ItemStyle-Width="30px">
                                                           <ItemTemplate>
                                                               <asp:CheckBox ID="ChkFee" runat="server" />
                                                           </ItemTemplate>
                                                           <HeaderTemplate>
                                                               <asp:CheckBox ID="cbtransAll" runat="server" Checked="false" 
                                                                   onclick="SelectTrans(this)" Text="All" />
                                                           </HeaderTemplate>
                                                       </asp:TemplateField>
                                                       <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" />
                                                       <asp:BoundField DataField="Period" HeaderText="Month" />
                                                   </Columns>
                                               </asp:GridView>
                                               <div style="text-align:center;">
                                                   <asp:HiddenField ID="Hdn_TransFee" runat="server" />
                                                   <asp:Label ID="Lbl_TransErrMsg" runat="server" Font-Bold="false" class="control-label"
                                                       ForeColor="Red"></asp:Label>
                                                   <br />
                                                   <asp:Button ID="Btn_TransOk" runat="server" OnClick="Btn_TransOk_Click" class="btn btn-success"
                                                       Text="Remove" Width="90px" />
                                                   <asp:Button ID="Btn_TransCancel" runat="server" Text="Cancel" Width="90px" class="btn btn-danger" />
                                               </div>
                                           </td>
                                           <td class="e">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td class="so">
                                           </td>
                                           <td class="s">
                                           </td>
                                           <td class="se">
                                           </td>
                                       </tr>
                                   </table>
                                   <br />
                                   <br />
                               </div>
                           </asp:Panel>
                       </div>
                   </ContentTemplate>
                   <Triggers>
                       <asp:PostBackTrigger ControlID="Btn_updateotherdetails" />
                   </Triggers>
               </asp:UpdatePanel>

</ContentTemplate>     

     </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Promotion"  >
                <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/img.png" /><b>CHANGE STUDENT IMAGE</b></HeaderTemplate>                 
<ContentTemplate>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
                <ProgressTemplate>
                    <div ID="progressBackgroundFilter">
                    </div>
                    <div ID="processMessage">
                        <table style="height:100%;width:100%">
                            <tr>
                                <td align="center">
                                    <b>Please Wait...</b><br />
                                    <br />
                                    <img alt="" src="images/indicator-big.gif" /></td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
</asp:UpdateProgress>
   
         <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
             <ContentTemplate>
                 <asp:Panel ID="Panel2" runat="server" BorderColor="Black" 
                     DefaultButton="Btn_Upload">
                     <div class="newsubheading">
                         Change Photo&nbsp;&nbsp;
                     </div>
                     <div class="linestyle">
                     </div>
                     <table class="style1">
                         <tr>
                            <td class="leftside">
                                              Select class:<span class="redcol"></span></td>
                                           <td class="rightside">
                                               <asp:DropDownList ID="Drp_selectClass" runat="server" TabIndex="6" class="form-control" Width="164px">
                                               </asp:DropDownList>
                                           </td>
                         </tr>
                         <tr>
                             <td class="leftside">
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                         <tr>
                             <td class="leftside">
                                 Upload Photo</td>
                             <td>
                                 <asp:FileUpload ID="FileUp_Student" runat="server" />
                             </td>
                         </tr>
                         <tr>
                             <td class="leftside">
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                         <tr>
                             <td class="leftside">
                             </td>
                             <td>
                                 <asp:Button ID="Btn_Upload" runat="server" Class="btn btn-primary" 
                                     OnClick="Btn_Upload_Click" Text="Upload" />
                             </td>
                         </tr>
                     </table>
                 </asp:Panel>
                
                 <div>
                     <asp:Button ID="Btn_hdnmessagetgt2" runat="server" style="display:none" class="btn btn-primary"/>
                     <cc1:ModalPopupExtender ID="MPE_MessageBox2" runat="server" 
                         BackgroundCssClass="modalBackground" CancelControlID="Btn_magok2" 
                         PopupControlID="Pnl_msg2" TargetControlID="Btn_hdnmessagetgt2" />
                     <asp:Panel ID="Pnl_msg2" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px">
                             <table cellpadding="0" cellspacing="0" class="containerTable">
                                 <tr>
                                     <td class="no">
                                     </td>
                                     <td class="n">
                                         <span style="color:White">alert!</span></td>
                                     <td class="ne">
                                         &nbsp;</td>
                                 </tr>
                                 <tr>
                                     <td class="o">
                                     </td>
                                     <td class="c">
                                         <asp:Label ID="Lbl_msg2" runat="server" Text="" class="control-label"></asp:Label>
                                         <br />
                                         <br />
                                         <div style="text-align:center;">
                                             <asp:Button ID="Btn_magok2" runat="server" Text="OK" Width="50px" class="btn btn-primary"/>
                                         </div>
                                     </td>
                                     <td class="e">
                                     </td>
                                 </tr>
                                 <tr>
                                     <td class="so">
                                     </td>
                                     <td class="s">
                                     </td>
                                     <td class="se">
                                     </td>
                                 </tr>
                             </table>
                             <br />
                             <br />
                         </div>
                     </asp:Panel>
                 </div>
             </ContentTemplate>
             <Triggers>
                 <asp:PostBackTrigger ControlID="Btn_Upload" />
             </Triggers>
    </asp:UpdatePanel>
              
                               
            
            
</ContentTemplate>
                
</ajaxToolkit:TabPanel>

                </ajaxToolkit:tabcontainer>
                           
                           
                           </asp:Panel>
                    
                          
        
                    </asp:Panel>
                        
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