<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="RegisterTempStudent.aspx.cs" Inherits="WinEr.RegisterTempStudent" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
     .LeftStyle
     {
         text-align:right;
     }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
         
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
    <ContentTemplate> 
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" />  
    
    <asp:Panel ID="Pnl1" runat="server" >
             <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no">
                    <img alt="" src="Pics/add_female_user.png" height="35" width="35" /> </td>
                <td class="n">Register Student</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >

                    
        <asp:Panel ID="Panel1" runat="server">
                     
                     <table class="tablelist">
                     <tr>
                     <td class="leftside"> <br /></td>
                     <td class="rightside"> <br /></td>
                     </tr>
                     
                         <tr>
                             <td class="leftside">
                                 Full Name<span style="color:Red">*</span>
                             </td>
                             <td class="rightside">
                                 <asp:TextBox ID="Txt_Name" class="form-control" runat="server" Width="250px" MaxLength="59" 
                                     TabIndex="1"></asp:TextBox>
                                 
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_Name">
                           </ajaxToolkit:FilteredTextBoxExtender>  
                                 
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SubmitDetails" ControlToValidate="Txt_Name" ErrorMessage="Enter Name"></asp:RequiredFieldValidator>
                             </td>
                        </tr>
                         <tr>     
                          <td class="leftside">Father Name </td>
                     <td class="rightside">
                         <asp:TextBox ID="Txt_FatherName" runat="server" class="form-control" Width="250px" MaxLength="59" 
                             TabIndex="2"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="Txt_FatherName">
                           </ajaxToolkit:FilteredTextBoxExtender>  
                     </td>
                          
                         </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                         <tr>
                           <td class="leftside">
                                 Mother Name
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="txtMotherName" runat="server" class="form-control"  Width="250px" TabIndex="3"> </asp:TextBox>
                           </td>
                           </tr>
                     <tr>
                      <td class="leftside">
                                 Sex<span style="color:Red">*</span> </td>
                             <td class="rightside">
                             <div class="radio radio-primary">
                                 <asp:RadioButtonList ID="RadioBtn_Sex" class="form-actions" runat="server" 
                                     RepeatDirection="Horizontal" TabIndex="4" Width="250px">
                                     <asp:ListItem Selected="True">Male</asp:ListItem>
                                     <asp:ListItem>Female</asp:ListItem>
                                 </asp:RadioButtonList>
                                 </div>
                             </td>
                     
                       </tr>
                        <tr>
                         <td class="leftside">
                             Standard<span style="color:Red">*</span> <br />
                         </td>
                          <td class="rightside">
                         <asp:DropDownList ID="Drp_Standard" class="form-control" runat="server" 
                             Height="38px" TabIndex="5" Width="250px" AutoPostBack="True" 
                                  onselectedindexchanged="Drp_Standard_SelectedIndexChanged">
                         </asp:DropDownList> 
                         </td>                                       
                     </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                       <tr>
                         <td class="leftside">
                             Class<span style="color:Red">*</span> <br />
                         </td>
                          <td class="rightside">
                         <asp:DropDownList ID="Drp_Class" class="form-control" runat="server" 
                             Height="38px" TabIndex="6" Width="250px">
                         </asp:DropDownList> 
                         </td>                                       
                     </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    
                     
                     <tr>
                     
                      <td class="leftside">
                               Academic Year<span style="color:Red">*</span> </td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_AccYear" class="form-control" runat="server" Width="250px" 
                             TabIndex="7" >
                         </asp:DropDownList>
                         <br />
                             
                     </td>
                     </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Interview Rank<span style="color:Red">*</span></td>
                             <td class="rightside">
                                   <asp:TextBox ID="txt_InterviewMark" runat="server" class="form-control" MaxLength="3" Width="250px" 
                                       TabIndex="8"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txt_InterviewMark" 
                        ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="SubmitDetails" ControlToValidate="txt_InterviewMark" ErrorMessage="Enter Rank"></asp:RequiredFieldValidator>
                    </td>
                       
                       
                      </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Phone</td>
                             <td class="rightside">
                                   <asp:TextBox ID="Txt_Ph" runat="server" class="form-control" MaxLength="14" Width="250px" 
                                       TabIndex="9"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="OfficePh" runat="server" 
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_Ph" 
                        ValidChars="+">
                    </ajaxToolkit:FilteredTextBoxExtender></td>
                       
                       
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                           <td class="leftside">
                                 DOB<span style="color:Red">*</span>
                           </td>
                             <td  class="rightside">
                                    <asp:TextBox ID="Txt_Dob" runat="server" class="form-control" Width="250px" TabIndex="10"></asp:TextBox> 
                                    <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"  
                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                        Mask="99/99/9999"
                                        UserDateFormat="DayMonthYear"
                                        Enabled="True" 
                                        TargetControlID="Txt_Dob">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <span style="color:Blue">DD/MM/YYYY</span>

                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Dob" ErrorMessage="You Must enter D.O.B"  ValidationGroup="SubmitDetails"  > </asp:RequiredFieldValidator>
                   
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
                           
                      <td class="leftside" > Address</td>
                     <td class="rightside">
                         <asp:TextBox ID="TxtAddress" runat="server" class="form-control" TextMode="MultiLine" Width="250px" 
                             MaxLength="240" Height="51px" TabIndex="11" ></asp:TextBox>
                         
                     </td>
                        <ajaxToolkit:FilteredTextBoxExtender ID="TxtAddress_FilteredTextBoxExtender3" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="TxtAddress">
                                     </ajaxToolkit:FilteredTextBoxExtender>  
                       </tr>
                      
                       <tr>
                       <td colspan="2"> 
                       <center>
                           <asp:LinkButton ID="Show_MoreInfo" class="form-button" runat="server" onclick="Show_MoreInfo_Click">Add More Details</asp:LinkButton>
                         
                           
                           <asp:LinkButton ID="Hide_MoreInfo" runat="server" Visible="false" 
                               onclick="Hide_MoreInfo_Click">Hide More Details</asp:LinkButton>
                         
                           
                       </center>
                       </td>
                       </tr>
                      
                       <tr>
                       <td colspan="2">
                           <asp:Panel ID="Panel_MoreInfo" runat="server" Visible="false">                          
                          <table class="tablelist">
                          
                        
                           
                           
                           <tr>
                           <td class="leftside">
                                 Location
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Location" runat="server" class="form-control" Width="250px" TabIndex="12"> </asp:TextBox>
                           </td>
                           </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           
                           <tr>
                           <td class="leftside">
                                 State
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_State" runat="server" class="form-control"  Width="250px" TabIndex="13"> </asp:TextBox>
                           </td>
                           </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                           <td class="leftside">
                                 Nationality
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_nationality" runat="server" class="form-control"  Width="250px" TabIndex="14"> </asp:TextBox>
                           </td>
                           </tr>    
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           <tr>
                           <td class="leftside">
                                 Pin Code
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_PinCode" runat="server" class="form-control" Width="250px" TabIndex="15" Text="0"> </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                 Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_PinCode" 
                                    ValidChars="+">
                                </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                                                           
                          
                           <tr>
                           <td class="leftside">
                                 Blood Group
                           </td>
                           <td class="rightside">
                               <asp:DropDownList ID="Drp_BloodGrp" runat="server" class="form-control" Height="38px" Width="250px"      TabIndex="16">
                                      </asp:DropDownList>
                           </td>
                           </tr>                                                                     
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                           <td class="leftside">
                                 Mother Tongue
                           </td>
                           <td class="rightside">
                                 <asp:DropDownList ID="Drp_MotherTongue" runat="server" class="form-control" Width="250px"      TabIndex="17">
                                 </asp:DropDownList>
                           </td>
                           </tr>    
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                           <td class="leftside">
                                 Fathers Education Qualification
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_father_educ" runat="server" class="form-control" Width="250px" TabIndex="18"> </asp:TextBox>
                           </td>
                           </tr> 
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>   
                           
                            <tr>
                           <td class="leftside">
                                 Motrhers Education Qualification
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Mothers_educ" runat="server" class="form-control" Width="250px" TabIndex="19"> </asp:TextBox>
                           </td>
                           </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>    
                           
                            <tr>
                           <td class="leftside">
                                 Fathers Occupation
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_fathers_Ocuptn" runat="server" class="form-control" Width="250px" TabIndex="20"> </asp:TextBox>
                           </td>
                           </tr> 
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                             <tr>
                           <td class="leftside">
                                 Mothers Occupation
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_mothers_Ocuptn" runat="server" class="form-control" Width="250px" TabIndex="20"> </asp:TextBox>
                           </td>
                           </tr> 
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                           
                            <tr>
                           <td class="leftside">
                                 Annual Income
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_annual_incum" runat="server" class="form-control" MaxLength="7" Width="250px" Text="0" TabIndex="21"> </asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                 Enabled="True" FilterType="Custom, Numbers" TargetControlID="Txt_annual_incum" 
                                    ValidChars="+">
                                </ajaxToolkit:FilteredTextBoxExtender>
                           </td>
                           </tr> 
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>   
                           
                            <tr>
                           <td class="leftside">
                                 Email ID
                           </td>
                           <td class="rightside">
                                <asp:TextBox ID="Txt_Email" runat="server" class="form-control" Width="250px" MaxLength="45"      TabIndex="22"></asp:TextBox>
                                
                                <asp:RegularExpressionValidator runat="server" ID="PNRegEx"
                                ControlToValidate="Txt_Email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                                
                                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                           </td>
                           </tr> 
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           
                           <tr>
                           
                           <td class="leftside">
                           Previous Board
                           </td>
                           <td class="rightside"> 
                             <asp:TextBox ID="txtPreviousBoard" runat="server" class="form-control" TextMode="MultiLine"   Width="250px" TabIndex="18"></asp:TextBox>
                           </td>
                           
                           </tr>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           <tr>
                           <td class="leftside">
                           Personal interview
                           </td>
                            <td class="rightside">
                            <div class="radio radio-primary">
                                <asp:RadioButtonList ID="rdoInterView" runat="server"  AutoPostBack="true"      TabIndex="24"
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="rdoInterView_SelectedIndexChanged">
                                <asp:ListItem Text="Attended" Selected="True" Value="Attended"></asp:ListItem>
                                <asp:ListItem Text="Not Attended" Value="NotAttended"></asp:ListItem>
                                <asp:ListItem Text="Provisional Admmission" Value="ProvAdm"></asp:ListItem>
                                </asp:RadioButtonList>
                                </div>
                            </td>
                           </tr>
                             
                           <tr>
                           <td colspan="2">
                           <asp:Panel ID="pnlInterviewDetails" runat="server">
                           <table class="tablelist" width="100%">
                           <tr>
                           <td class="leftside"> Date of Interview</td>
                           <td  class="rightside">
                                    <asp:TextBox ID="txtDOI" runat="server" class="form-control" Width="250px" TabIndex="25"></asp:TextBox> 
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                        Mask="99/99/9999"
                                        UserDateFormat="DayMonthYear"
                                        Enabled="True" 
                                        TargetControlID="txtDOI">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <span style="color:Blue">DD/MM/YYYY</span>

                                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                ControlToValidate="txtDOI"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="DobDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" /><br/>
                               
      
                               
                                </td>
                           
                           </tr>
                           <tr>
                           <td class="leftside">
                           Teacher's remarks
                           </td>
                           <td class="rightside">
                               <asp:TextBox ID="txtTeacherRemark" runat="server" class="form-control" TextMode="MultiLine"  Width="250px" TabIndex="18"></asp:TextBox>
                           </td>
                           
                           </tr>
                           <tr>
                           <td class="leftside">
                            HM's remarks</td>
                            <td class="rightside">
                            <asp:TextBox ID="txtHMRemark" runat="server" class="form-control" TextMode="MultiLine"   Width="250px" TabIndex="18"></asp:TextBox></td>
                           </tr>
                            <tr>
                            <td  class="leftside">
                             Principal Remarks 
                            </td>
                            <td  class="rightside">
                                <asp:TextBox ID="txtPrincipalRemark" class="form-control" runat="server" TextMode="MultiLine" Width="250px" TabIndex="18"></asp:TextBox>                            
                            </td>                            
                            </tr>
                            <tr>
                            <td  class="leftside">
                            Result
                            </td>
                            <td class="rightside">
                            <div class="radio radio-primary">
                                <asp:RadioButtonList ID="rdoResult" class="form-actions" runat="server" RepeatDirection="Horizontal"  TabIndex="29">
                                <asp:ListItem Selected="True"  Text="Selected" Value="Selected"></asp:ListItem>
                                <asp:ListItem   Text="Not Selected" Value="NotSelected"></asp:ListItem>
                                <asp:ListItem   Text="Hold" Value="Hold"></asp:ListItem>
                                </asp:RadioButtonList>
                                </div>
                            </td>
                            </tr>
                             </table>
                            </asp:Panel>
                           </td>
                           </tr>
                             <tr>
                           <td  class="leftside">
                           Remark
                           
                           </td>
                           <td class="rightside">
                               <asp:TextBox ID="txtStudRemark" runat="server" class="form-control" TextMode="MultiLine"  Width="250px" TabIndex="18"></asp:TextBox>
                           </td>
                             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           </tr>
                         
                           </table>
                            
                         
                           
                          <asp:Panel ID="Pnl_custumarea" runat="server" >
                    
               <div class="newsubheading">
                    Extra details
                    </div>
                
                <asp:PlaceHolder ID="myPlaceHolder" runat="server" ></asp:PlaceHolder>
                 <div class="linestyle" >  </div> 
                                
                    </asp:Panel> 
                           
                             
                             </asp:Panel>
                       </td></tr>
                      
                       <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="Lbl_Message" class="control-label" runat="server"  ForeColor="Red"></asp:Label>
                            </td>
                       </tr>
                
                <tr>
                     <td>
                         <asp:HiddenField ID="Hdn_studid" runat="server" />
                         <asp:HiddenField ID="Hdn_Standid" runat="server" />
                    </td>
                     <td>
                        <asp:Button ID="Btn_Save" runat="server" Text="Save" class="btn btn-success"  ValidationGroup="SubmitDetails"  
                             onclick="Btn_Save_Click" ToolTip="Save"  TabIndex="31"/>&nbsp;&nbsp;&nbsp;
                        <%--<asp:Button ID="Btn_Cancel" runat="server" Text="Cancel"  CssClass="graycancel" TabIndex="10" ToolTip="Cancel"
                             onclick="Btn_Cancel_Click"/>--%>
                             <input id="Reset1" type="reset" runat="server" class="btn btn-danger" title="Reset" />
                        
                     </td>
                    
                     </tr>
                         
                     
                     </table>
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

    </asp:Panel>

       


    
      </ContentTemplate>

    </asp:UpdatePanel> 


<div class="clear"></div>
</div>

</asp:Content>
