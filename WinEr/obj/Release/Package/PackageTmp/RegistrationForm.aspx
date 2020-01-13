<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="RegistrationForm.aspx.cs" Inherits="WinEr.RegistrationForm" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style" media="screen" />
    <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style" media="screen" />
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div> 
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel runat="server" ID="UpdatePnl_RegistrationForm">
<ContentTemplate>

    <asp:Panel ID="Pnl_RegistrationForm" runat="server">
    <center> 
    
    <table width="100%" >
        <tr >
           
            <td align="center">
            <asp:Label ID="Lbl_Heading" runat="server" Text="REGISTERED STUDENT DETAILS" Font-Bold="true"></asp:Label>
            </td>
        </tr> 
        </table>
        <table>
        <tr>
   <td valign="top">
       
        <div id="Photo" runat="server">
        <img src="ThumbnailImages/DefaultImage.jpg" width="60px" />
        
        
        </div>
        
   </td>
   <td valign="top">
    <div style=" overflow:auto; max-height: 500px; width:100%">
        <table width="100%" >
        <tr >
           
            <td align="left">
            <asp:Label ID="Lbl_PersonalInfo" runat="server" Text="Personal Informations" Font-Bold="true"></asp:Label>
            </td>
            <td></td>
        </tr> 
       
            <tr>
                <td align="right">Student Name:<span style="color:Red;font-size:14px;" >*</span></td>
                <td align="left"><asp:TextBox ID="Txt_StudentName" runat="server" Width="180px" Height="15px" ></asp:TextBox>
                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars"  InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_StudentName">
                 </ajaxToolkit:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_StudentName" ValidationGroup="Tempsearch" ErrorMessage="Enter name"></asp:RequiredFieldValidator>
                </td>
            </tr>
           
    
     <tr>
            <td align="right">D.O.B:<span style="color:Red;font-size:14px;" >*</span></td>
            <td align="left">
           <asp:TextBox ID="Txt_Dob" runat="server" Width="180px" Height="15px"  ></asp:TextBox> 
            <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"  
                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                Mask="99/99/9999"
                UserDateFormat="DayMonthYear"
                Enabled="True" 
                TargetControlID="Txt_Dob">
            </ajaxToolkit:MaskedEditExtender>
            <span style="color:Blue; font-size:x-small" >DD/MM/YYYY
                </span>
                
            &nbsp;&nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Tempsearch" ControlToValidate="Txt_Dob" ErrorMessage="Enter D.O.B"></asp:RequiredFieldValidator>
                           
            <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
            ControlToValidate="Txt_Dob"
            Display="None" 
            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
            TargetControlID="DobDateRegularExpressionValidator3"
            HighlightCssClass="validatorCalloutHighlight" /><br/>
            </td>         
    </tr>
    <tr>
    
       
            <td align="right">Age :<span style="color:Red;font-size:14px;" >*</span></td>
            <td align="left">
            <asp:TextBox ID="Txt_Age" runat="server" Width="180px" Height="15px"  ></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender id="FilteredTextBoxExtendertxtAge" runat="server" 
                        Enabled="True" FilterType="Numbers" TargetControlID="Txt_Age" >
                    </ajaxToolkit:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="RqvAge" runat="server" ValidationGroup="Tempsearch" ControlToValidate="Txt_Age" ErrorMessage="Enter age"></asp:RequiredFieldValidator>
                </td>           
    </tr>
    <tr>
                <td align="right">Place Of Birth:</td>
                <td align="left"><asp:TextBox ID="Txt_PlaceOfBirth" runat="server" Width="180px" Height="15px" ></asp:TextBox>
                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars"  InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_PlaceOfBirth">
                 </ajaxToolkit:FilteredTextBoxExtender>
                
                </td>
   </tr>
   <tr>
            <td align="right">Permanent Address:<span style="color:Red;font-size:14px;" >*</span>
            </td>
            <td align="left"><asp:TextBox ID="Txt_PermanentAddress" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Tempsearch" ControlToValidate="Txt_PermanentAddress" ErrorMessage="Enter Address"></asp:RequiredFieldValidator>
            </td>
    </tr>
       <tr>
            <td align="right">Present Address:
            </td>
            <td align="left"><asp:TextBox ID="Txt_PresentAddress" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
            </td>
    </tr>
    
    
     <tr>
            <td align="right">Mobile Number:<span style="color:Red;font-size:14px;" >*</span>
            </td>
            <td align="left"><asp:TextBox ID="Txt_MobileNumber" runat="server" Width="180px" Height="15px" ></asp:TextBox>
            <ajaxToolkit:FilteredTextBoxExtender id="OfficePh" runat="server" 
                        Enabled="True" FilterType="Numbers" TargetControlID="Txt_MobileNumber" >
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator runat="server" id="RegularExpressionValidator1"
                                ControlToValidate="Txt_MobileNumber" ValidationGroup="Tempsearch"
                                Display="None"
                                ValidationExpression="^[0-9]{10,12}"
                                ErrorMessage="Invalid Mobile No" />
                    <ajaxToolkit:ValidatorCalloutExtender runat="Server" id="ValidextndrMobile"
                                TargetControlID="RegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />   
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Txt_MobileNumber" ValidationGroup="Tempsearch" ErrorMessage="Enter Mobile Number"></asp:RequiredFieldValidator>     
            </td>
    </tr>
    
     <tr>
            <td align="right">Residence Phone Numbers:
            </td>
            <td align="left"><asp:TextBox ID="Txt_ResidencePhNo" runat="server" Width="180px" Height="15px"></asp:TextBox>
            <ajaxToolkit:FilteredTextBoxExtender id="FilteredTextBoxExtender1" runat="server" 
                        Enabled="True" FilterType="Numbers" TargetControlID="Txt_ResidencePhNo" >
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator runat="server" id="RegularExpressionValidator2"
                                ControlToValidate="Txt_ResidencePhNo"
                                Display="None"
                                ValidationExpression="^[0-9]{10,12}"
                                ErrorMessage="Invalid Mobile No" />
                    <ajaxToolkit:ValidatorCalloutExtender runat="Server" id="ValidatorCalloutExtender1"
                                TargetControlID="RegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" />        
            </td>
    </tr>
     <tr>
            <td align="right">Class For Admission:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_standard" runat="server" Height="22px" Width="183px" >
                                      </asp:DropDownList>      
            </td>
    </tr>
    <tr>
            <td align="right">Admission For:
            </td>
            <td align="left"><asp:CheckBox ID="Chk_CurrentBatch" runat="server" Checked="true" 
                    AutoPostBack="True" oncheckedchanged="Chk_CurrentBatch_CheckedChanged" />Current Batch 
            </td>
    </tr>
     <tr id="RownextBatch" runat="server">
            <td align="right">Next Batch:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_NextBatch" runat="server" Height="22px" Width="183px" >
                                      </asp:DropDownList>  
                <asp:HiddenField ID="Hdn_BatchId" runat="server" />    
            </td>
    </tr>
    <tr>
            <td align="right">Gender:</td>
            <td align="left">
            <asp:RadioButtonList ID="Rbd_Gender" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Male" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Female" Value="1"></asp:ListItem>
            </asp:RadioButtonList>
            
            </td>
    </tr> 
     <tr>
            <td align="right">Category:</td>
            <td align="left">
            <asp:RadioButtonList ID="Rbd_Category" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="SC" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="ST" Value="1"></asp:ListItem>
            <asp:ListItem Text="OBC" Value="2"></asp:ListItem>
            <asp:ListItem Text="GEN" Value="3"></asp:ListItem>
            </asp:RadioButtonList>
            
            </td>
    </tr>   
       
     <tr>
            <td align="right">Blood Group:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_BloodGroup" runat="server" Height="22px" Width="183px" >
                                      </asp:DropDownList>      
            </td>
    </tr>
     <tr>
            <td align="right">Nationality:
            </td>
            <td align="left"><asp:TextBox ID="Txt_Nationality" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
     <tr>
            <td align="right">Mother Tongue:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_MotherTongue" runat="server" Height="22px" Width="183px" >
                                      </asp:DropDownList>    
            </td>
    </tr>
    <tr>
            <td align="right">Religion:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_religion" runat="server" Width="183px" 
                    AutoPostBack="True" onselectedindexchanged="Drp_religion_SelectedIndexChanged"></asp:DropDownList>
            </td>
    </tr>
     <tr id="RowOtherReligion" runat="server">
            <td align="right">Enter Religion:<span style="color:Red;font-size:14px;" >*</span>
            </td>
            <td align="left"><asp:TextBox ID="Txt_OtherReligion" runat="server" Height="15px"
                    Width="180px" ></asp:TextBox>
             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ReligionFilteredTextBoxExtender1" 
            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
            TargetControlID="Txt_OtherReligion">
        </ajaxToolkit:FilteredTextBoxExtender>
         <asp:RequiredFieldValidator ID="Txt_ReligionRequiredFieldValidator6"   ValidationGroup="Tempsearch" runat="server" Enabled="true" ControlToValidate="Txt_OtherReligion" ErrorMessage="Enter a Religion"></asp:RequiredFieldValidator>
            </td>
    </tr>
     <tr>
            <td align="right">Caste:
            </td>
            <td align="left"><asp:DropDownList ID="Drp_Caste" runat="server" Width="183px"></asp:DropDownList>
            </td>
    </tr>
    
    <tr>
             <td align="left">
            <asp:Label ID="Lbl_ParentsInfo" runat="server" Text="Parent's Details" Font-Bold="true"></asp:Label>
            </td>
            <td></td>
    </tr>
    <tr>
            <td align="right">Father/Guardian Name:<span style="color:Red;font-size:14px;" >*</span>
            </td>
            <td align="left"><asp:TextBox ID="Txt_GuardianName" runat="server" Width="180px" Height="15px"></asp:TextBox>        
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3"   ValidationGroup="Tempsearch" runat="server" Enabled="true" ControlToValidate="Txt_GuardianName" ErrorMessage="Enter Guardian Name"></asp:RequiredFieldValidator>
            </td>
    </tr>
    <tr>
    <td align="right">Profession / Designation:
    </td>
       <td align="left"><asp:TextBox ID="txt_FatherProfession" runat="server" Width="180px" Height="15px"></asp:TextBox>        
     </td>
    </tr>
        <tr>
    <td align="right">Annual Income:
    </td>
       <td align="left"><asp:TextBox ID="txt_AnnualIncome" runat="server" Width="180px" Height="15px"></asp:TextBox>   
         <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"  FilterType="Custom" 
            runat="server" Enabled="True" FilterMode="ValidChars" ValidChars=".0123456789" 
            TargetControlID="txt_AnnualIncome">
        </ajaxToolkit:FilteredTextBoxExtender>     
     </td>
    </tr>
    <tr>
            <td align="right">Email Id:<span style="color:Red;font-size:14px;" >*</span>
            </td>
            <td align="left"><asp:TextBox ID="Txt_EmailId" runat="server" Width="180px" Height="15px"></asp:TextBox>
             <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_EmailId"
                                Display="None" ValidationGroup="Tempsearch"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />  
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Txt_EmailId" ValidationGroup="Tempsearch" ErrorMessage="Enter Email Id"></asp:RequiredFieldValidator>     
            </td>
    </tr>
    <tr>
     <td align="right">Office Address</td>
     <td align="left"><asp:TextBox ID="txt_FathersOffcaddress" runat="server" Width="250px" Height="75px" TextMode="MultiLine"></asp:TextBox>
             </td>
    
    </tr>
    <tr>
            <td align="right">Mother's Name:
            </td>
            <td align="left"><asp:TextBox ID="Txt_MotherName" runat="server" Width="180px" Height="15px"></asp:TextBox>        
            </td>
    </tr>
 <tr>
    <td align="right">Profession / Designation:
    </td>
       <td align="left"><asp:TextBox ID="Txt_MothersProfession" runat="server" Width="180px" Height="15px"></asp:TextBox>        
     </td>
    </tr>
        <tr>
    <td align="right">Annual Income:
    </td>
       <td align="left"><asp:TextBox ID="txt_mothersIncome" runat="server" Width="180px" Height="15px"></asp:TextBox>   
         <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"  FilterType="Custom" 
            runat="server" Enabled="True" FilterMode="ValidChars" ValidChars="0123456789." 
            TargetControlID="txt_mothersIncome">
        </ajaxToolkit:FilteredTextBoxExtender>     
     </td>
    </tr>
    <tr>
            <td align="right">Email Id:
            </td>
            <td align="left"><asp:TextBox ID="txt_Mothersemail" runat="server" Width="180px" Height="15px"></asp:TextBox>
         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender5"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />  
                                   </td>
    </tr>
    <tr>
     <td align="right">Office Address</td>
     <td align="left"><asp:TextBox ID="Txt_MothersOffcAddress" runat="server" Width="250px" Height="75px" TextMode="MultiLine"></asp:TextBox>
             </td>
             </tr>
   
     <tr>
            <td align="right">State:
            </td>
            <td align="left"><asp:TextBox ID="Txt_State" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
    <tr>
            <td align="right">City:
            </td>
            <td align="left"><asp:TextBox ID="Txt_Location" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
   
     <tr>
            <td align="right">Pin Code:
            </td>
            <td align="left"><asp:TextBox ID="Txt_pin" runat="server" Width="180px"  Height="15px" MaxLength="7"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_pin_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers"  
                        TargetControlID="Txt_pin">
                    </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>
    
    
    <%-- <tr>
            <td align="right">Father's Educational Qualification:
            </td>
            <td align="left"><asp:TextBox ID="Txt_FatherEduQualification" runat="server" Width="180px" Height="15px" 
                        MaxLength="45"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                runat="server" Enabled="True" TargetControlID="Txt_FatherEduQualification" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>
    <tr>
            <td align="right">Mother's Educational Qualification:
            </td>
            <td align="left"><asp:TextBox ID="Txt_MotherEduQualification" Height="15px" runat="server" MaxLength="45" 
                                Width="180px"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" 
                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'\" 
                                TargetControlID="Txt_MotherEduQualification">
                            </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>--%>
   <%--  <tr>
            <td align="right">Father's Occupation:
            </td>
            <td align="left"><asp:TextBox ID="Txt_FatherOccupation" runat="server" Width="180px" Height="15px"
                        MaxLength="45"></asp:TextBox>
                          <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                runat="server" Enabled="True" TargetControlID="Txt_FatherOccupation" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                            </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>
     <tr>
            <td align="right">Annual Income:
            </td>
            <td align="left"> <asp:TextBox ID="Txt_AnualIncome" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="AnualIncome1" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="Txt_AnualIncome">
                    </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>--%>
    <tr>
             <td align="left">
            <asp:Label ID="Lbl_LastSchoolDetails" runat="server" Text="Last School Details" Font-Bold="true"></asp:Label>
            </td>
            <td></td>
    </tr>
    <tr>
            <td align="right">School Name:
            </td>
            <td align="left"><asp:TextBox ID="Txt_LastSchoolName" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
    <tr>
            <td align="right">School Address:
            </td>
            <td align="left"><asp:TextBox ID="Txt_LastSchoolAddress" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
            </td>
    </tr>
    <tr>
            <td align="right">TC Number:
            </td>
            <td align="left"><asp:TextBox ID="Txt_TcNumber" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
    <tr>
            <td align="right">TC Date</td>
            <td align="left">
           <asp:TextBox ID="Txt_TcDate" runat="server" Width="180px" Height="15px"  ></asp:TextBox> 
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                Mask="99/99/9999"
                UserDateFormat="DayMonthYear"
                Enabled="True" 
                TargetControlID="Txt_TcDate">
            </ajaxToolkit:MaskedEditExtender>
            <span style="color:Blue; font-size:x-small" >DD/MM/YYYY
                </span>    
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3"
            ControlToValidate="Txt_TcDate"
            Display="None" 
            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender3"
            TargetControlID="DobDateRegularExpressionValidator3"
            HighlightCssClass="validatorCalloutHighlight" /><br/>
            </td>         
    </tr>
    <tr>
            <td align="right">Reason For Leaving:
            </td>
            <td align="left"><asp:TextBox ID="Txt_ReasonLeaving" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
   <tr>
            <td align="right">Class Passed:
            </td>
            <td align="left"><asp:TextBox ID="Txt_LastClass" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
    <tr>
            <td align="right">Date Of Leaving</td>
            <td align="left">
           <asp:TextBox ID="Txt_LeavingDate" runat="server" Width="180px" Height="15px"  ></asp:TextBox> 
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"  
                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                Mask="99/99/9999"
                UserDateFormat="DayMonthYear"
                Enabled="True" 
                TargetControlID="Txt_LeavingDate">
            </ajaxToolkit:MaskedEditExtender>
            <span style="color:Blue; font-size:x-small" >DD/MM/YYYY
                </span>    
            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4"
            ControlToValidate="Txt_LeavingDate"
            Display="None" 
            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
            <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender4"
            TargetControlID="DobDateRegularExpressionValidator3"
            HighlightCssClass="validatorCalloutHighlight" /><br/>
            </td>         
    </tr>
    <tr>
            <td align="right">Remarks:
            </td>
            <td align="left"><asp:TextBox ID="Txt_LastClassRemarks" runat="server" Width="180px" Height="15px"></asp:TextBox>
            </td>
    </tr>
      <tr>
             <td align="left">
            <asp:Label ID="Lbl_OtherInfo" runat="server" Text="Other Details" Font-Bold="true"></asp:Label>
            </td>
            <td></td>
    </tr>
    
    <tr>
            <td align="right">Identification Mark:
            </td>
            <td align="left"><asp:TextBox ID="Txt_Identificationmark"  Height="15px" runat="server" Width="180px"></asp:TextBox>
            </td>
    </tr>
<%--    
    
     <tr>
            <td align="right">Number Of Brothers:
            </td>
            <td align="left"><asp:TextBox ID="Txt_NumBrother" runat="server" Width="180px" Height="15px"></asp:TextBox>
             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NoBro_FilteredTextBoxExtender1" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="Txt_NumBrother">
                    </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>
    <tr>
            <td align="right">Number Of Sisters:
            </td>
            <td align="left"><asp:TextBox ID="Txt_NumSister" runat="server" Width="180px" Height="15px"></asp:TextBox>
             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                        runat="server" Enabled="True" FilterType="Numbers"
                        TargetControlID="Txt_NumSister">
                    </ajaxToolkit:FilteredTextBoxExtender>
            </td>
    </tr>--%>
    <tr>
    
    <td align="right">Sibling in the same school </td>
    <td align="left"> 
     <asp:RadioButtonList ID="rdo_sibling" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="No" Value="1"></asp:ListItem>
            </asp:RadioButtonList>
            
    </td>
    
    </tr>
      <tr>
    
    <td align="right">If yes, please give the Admission No and the class </td>
    <td align="left"> 
     <asp:TextBox ID="Txt_SiblingAdmissionNo"   Height="15px" runat="server" Width="180px"></asp:TextBox>
            
    </td>
    </tr>
       <tr>
    
    <td align="right">Mode of transport </td>
     <td align="left">
      <asp:RadioButtonList ID="rdo_transport" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="School" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Private" Value="1"></asp:ListItem>
              <asp:ListItem Text="Own" Value="2"></asp:ListItem>
            </asp:RadioButtonList>
            
            
    </td>
    
    </tr>
    
     <tr>     
            <td></td>      
            <td align="left">
                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label></td>
    </tr>
    
     <tr>     
            <td></td>      
            <td align="left">
                <asp:ImageButton ID="Img_Save" runat="server"   ValidationGroup="Tempsearch"
                    ImageUrl="~/Pics/save.png" Width="45px" Height="45px" 
                     ToolTip="Register" onclick="Img_Save_Click"/>
                     </td>
    </tr>
    </table>
    </div>
    </td></tr></table>
    </div>
    </center>
    </asp:Panel>
      <WC:MSGBOX id="WC_MessageBox" runat="server" />    
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
