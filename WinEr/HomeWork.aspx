<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="HomeWork.aspx.cs" Inherits="WinEr.HomeWork" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            text-align: right;
            font-weight: lighter;
            height: 30px;
        }
        </style>
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
         
     <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate> 
             <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no"> </td>
                <td class="n">Assign Home Work</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <br />
              <table cellspacing="10"   width="100%">
		     <tr>
		     <td class="leftside" align="right">
		     <asp:Label ID="lbl_category" runat="server" Text="Select Category:"></asp:Label>
		     </td>
		     <td>
		    
                
                 
                 
                                                <div class="radio radio-primary">
                                                 <asp:RadioButtonList ID="Rdb_Selection_type" class="form-actions" runat="server" AutoPostBack="true"
                                                     RepeatDirection="Horizontal" TabIndex="2" OnSelectedIndexChanged="Rdb_Selection_type_SelectedIndexChanged">
                                                     <asp:ListItem Text="Class Wise" Selected="True" Value="1"></asp:ListItem>
                                                     <asp:ListItem Text="Student Wise " Value="2"></asp:ListItem>
                                                 </asp:RadioButtonList>
                                                 </div>
                 
                 
                 
                 
                            
             </td>
             <tr>
             <td class="style2" align="right">
                       <asp:Label ID="Label1" runat="server" Text=" Select Class :"></asp:Label>
                    </td>
                    <td class="rightside" style="height: 30px">
                    <asp:DropDownList ID="Drp_class" runat="server" AutoPostBack="true" class="form-control" Width="250px"   OnSelectedIndexChanged="Drp_class_SelectedIndexChanged" >
                        </asp:DropDownList> <asp:Label ID="lblstudentmsg" runat="server" ForeColor="Red" Text=" No Students In Selected Class." 
                            Visible="False"></asp:Label>
                    </td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td>
                    </td>
                    <td>
                   
                 
                       <asp:Panel ID="Panel_Students" runat="server"  
                           style="height:150px;Width:250px; overflow:auto;" visible="false"  >
                      <asp:CheckBoxList ID="Chkb_students" runat="server" class="form-actions" RepeatDirection="Vertical">
                    </asp:CheckBoxList>
                    </asp:Panel>
                    </td>
                    <tr>
                    <td class="leftside" align="right">
                     <asp:Label ID="Label2" runat="server" class="control-label" Text="Enter Subject :"></asp:Label>
                     <asp:Label ID="Label11" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
                  
                    </td>
                    <td class="rightside">
                       <asp:TextBox ID="Txt_Search" runat="server" class="form-control" Width="250px">
                     </asp:TextBox>                                               
                    <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender" runat="server"
                    DelimiterCharacters="" Enabled="True" ServiceMethod="Get_Subject_Name" ServicePath="WinErWebService.asmx"
                    UseContextKey="false" TargetControlID="Txt_Search" MinimumPrefixLength="1">
                    </ajaxToolkit:AutoCompleteExtender>
                    <ajaxToolkit:FilteredTextBoxExtender ID="Exam_nameFilteredTextBoxExtender1" runat="server"
                    TargetControlID="Txt_Search" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\" />
                    </td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td class="leftside" align="right">
                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Description Of Home Work :"></asp:Label>
                    <asp:Label ID="Label5" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="rightside">
                    <asp:TextBox ID="Txt_Description" runat="server" class="form-control" TextMode="MultiLine" Width="250px"></asp:TextBox>
                            
                    </td>   
                             
                       </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td class="leftside" align="right">
                     <asp:Label ID="Label4" runat="server" class="control-label" Text="Expected Date Of Completion :"></asp:Label>
                     <asp:Label ID="Label6" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="rightside">
                                               <asp:TextBox ID="Txt_Dt" runat="server" class="form-control" Width="250px"></asp:TextBox>
                           <ajaxToolkit:MaskedEditExtender ID="Txt_SDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_Dt" CultureAMPMPlaceholder="AM;PM" 
                                              CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                              CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                              CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <asp:Label ID="lblexpdate" runat="server" class="control-label" Text="DD/MM/YYYY"></asp:Label>
                                         <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                                ControlToValidate="Txt_Dt"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                                TargetControlID="DobDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                       
                    </td>
                    </tr>
                    </table>
                    <center>
                   
                    <asp:Label ID="lbl_Msg"  class="control-label" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Button ID="Btn_Save" runat="server" Text="Save" OnClick="Btn_Save_click" class="btn btn-success" /> 
                      &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="Btn_Clear" runat="server" CausesValidation="false" 
                                 class="btn btn-danger" onclick="Btn_Clear_Click" Text="Clear" />
                    </center>
                
                       </td>
                <td class="e"> </td>
            </tr>
            <tr >
                <td class="so"> </td>
                <td class="s"></td>
                <td class="se"> </td>

            </tr>
        </table>
               <WC:MSGBOX id="WC_MessageBox" runat="server"/>   
             </div>
             </ContentTemplate>
             </asp:UpdatePanel>
</asp:Content>
