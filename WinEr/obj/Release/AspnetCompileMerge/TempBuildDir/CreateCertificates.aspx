<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CreateCertificates.aspx.cs" Inherits="WinEr.CreateCertificates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
    <div id="right"><div class="label">Student Info</div><div id="SubStudentMenu" runat="server">  </div></div>
    <div id="left">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr><td align="center"><b>Please Wait...</b><br /><br /><img src="images/indicator-big.gif" alt=""/></td></tr> </table></div></ProgressTemplate>
        </asp:UpdateProgress>       
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate>
                <div id="StudentTopStrip" runat="server"> 
                    <div id="winschoolStudentStrip">
                        <table class="NewStudentStrip" width="100%">
                            <tr><td class="left1"></td>
                                <td class="middle1" >
                                    <table>
                                        <tr><td><img alt="" src="images/img.png" width="82px" height="76px" /></td>
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
                                                   <%-- <tr>
                                                        <td colspan="11"><hr /></td>
                                                    </tr>--%>
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
                                                        <td class="DBvalue"> 100</td>
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
                                <td class="right1">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="container skin1" >
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr><td class="no"> </td><td class="n">Configure Certificate</td><td class="ne"></td></tr>
                        <tr >
                        <td class="o"></td>
                        <td class="c">
                            <br />
                                <div align="center" style="border-bottom:solid 1px gray;">
                                    <asp:Label ID="Lbl_SelectType" runat="server" Text="Select Certificate Type"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:DropDownList ID="Drp_Type" runat="server" Width="250px"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="Btn_Show" runat="server" Width="100px" Text="Show"  onclick="Btn_Show_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="Btn_Print" runat="server" Width="100px" Text="Finish"  onclick="Btn_Print_Click" />
                                   
                                </div>
                                 <br />
                                  <center>
                                <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red"></asp:Label>    
                                </center>
                                
                                <br />
                                <div align="left" id="EntryPart" runat="server"  >
                                    <asp:Label ID="Lbl_E1" runat="server" Text="Entry 1 :"></asp:Label> <asp:TextBox   ID="Txt_SchoolCode" runat="server" Width="100px" MaxLength="25"></asp:TextBox><asp:ImageButton ID="ImgE1" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                    <br />
                                    <asp:Label ID="Lbl_E2" runat="server" Text="Entry 2 :"></asp:Label> <asp:TextBox   ID="Txt_Estd" runat="server"  Width="100px"  MaxLength="25"></asp:TextBox><asp:ImageButton ID="ImgE2" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                    <br />
                                    <asp:Label ID="Lbl_E3" runat="server" Text="Entry 3 :"></asp:Label> <asp:TextBox ID="Txt_Date" runat="server" Width="100px"  MaxLength="25"></asp:TextBox>
                                           <ajaxToolkit:MaskedEditExtender ID="Txt_Dob_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_Date">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <span style="color:Blue">DD/MM/YYYY</span>
        
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Txt_Date" ErrorMessage="You Must enter Date"></asp:RequiredFieldValidator>
                   
                     <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                ControlToValidate="Txt_Date"
                                Display="None" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                TargetControlID="DobDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                               
      
                                    <br />
                                    <asp:Label ID="Lbl_E4" runat="server" Text="Entry 4 :"></asp:Label>
                                    <asp:DropDownList ID="Drp_Passed" runat="server" Width="103px">
                                               <asp:ListItem Enabled="true" Text="passed" Value="0"></asp:ListItem>
                                               <asp:ListItem Text="failed" Value="1"></asp:ListItem>
                                           </asp:DropDownList><asp:ImageButton ID="ImgE4" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                           <br />
                                           <asp:Label ID="Lbl_E5" runat="server" Text="Entry 5 :"></asp:Label>
                                        <asp:TextBox ID="Txt_Batch" runat="server" Text=""  MaxLength="25" Width="100px"></asp:TextBox><asp:ImageButton ID="ImgE5" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                        <br />
                                        <asp:Label ID="Lbl_E6" runat="server" Text="Entry 6 :"></asp:Label>
                                        <asp:TextBox ID="Txt_role" runat="server" Text=""  MaxLength="25" Width="100px"></asp:TextBox><asp:ImageButton ID="ImgE6" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                        <br />
                                        <asp:Label ID="Lbl_E7" runat="server" Text="Entry 7 :"></asp:Label>
                                         <asp:TextBox ID="Txt_Num" runat="server" Text=""  MaxLength="25" Width="100px"></asp:TextBox><asp:ImageButton ID="ImgE7" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                        <br />  
                                          <asp:Label ID="Label1" runat="server" Text="Entry 8 :"></asp:Label>
                                         <asp:TextBox ID="Txt_Division" runat="server" Text=""  MaxLength="25" Width="100px"></asp:TextBox><asp:ImageButton ID="ImgE8" runat="server"  ImageUrl="~/images/cross.png" Width="15px" Height="15px" />
                                </div>
                            <asp:Panel ID="Pnl_Certificate" runat="server">
                            
                                <div class="certificate">
                                    <div class="logo"> <asp:ImageButton ID="ImgBtn"  runat="server" width="50px" height="50px" /></div>
                                    <div class="Address" runat="server">
                                       <div align="center" style="font-size:22px; font-weight:bold" >  <asp:Label ID="Lbl_CollegeName" runat="server" Text="" ></asp:Label></div>
                                        
                                        <div align="center" style="font-size:12px"><asp:Label ID="Lbl_SchoolCode" runat="server" Text="School Code :"></asp:Label><asp:Label ID="lbl_Entry1" runat="server" Text="Entry 1" ForeColor="Red"> </asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lbl_Estd" runat="server" Text="Estd :"></asp:Label>
                                        <asp:Label ID="Lbl_Entry2" runat="server" Text="Entry 2" ForeColor="Red"></asp:Label>
                                        <br />
                                        <asp:Label ID="Lbl_ClgAdd" runat="server" Text=""></asp:Label>
                                         </div>
                                         
                                    </div>
                                    <div class="admissionnumber">
                                        <asp:Label ID="Lbl_Admission" runat="server" Text="Admission Number :" ></asp:Label><asp:Label ID="Lbl_AdmissionNum"  Font-Bold="true" runat="server"></asp:Label>
                                    </div>
                                    <div class="certificatedate">
                                        <asp:Label ID="Lbl_Dt" runat="server" Text="Date :"></asp:Label><asp:Label ID="Lbl_Entry3" runat="server" Text="Entry 3" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div Id="certificateName" runat="server" align="center" style="font-size:20px;font-weight:bold">
                                    
                                    </div>
                                    <br />
                                    <div Id="Description"  style="clear:both; text-align:justify;" runat="server">
                                    <%--<div class="certificatedes"  style="clear:both; text-align:justify;">
                                        Certified that  Sri/Miss. <asp:Label ID="Lbl_Name" runat="Server" Text=""  Font-Bold="true" ></asp:Label>
                                       &nbsp; Son/daughter of  Mr./Mrs/Late.	 <asp:Label ID="Lbl_FatherName" runat="Server" Text=""  Font-Bold="true" ></asp:Label>.<div id="ResidenceVal" runat="server" style="clear:both">
                                        <asp:Label ID="Lbl_Residence" Text="a resident of " runat="server"></asp:Label> &nbsp;<asp:Label ID="Lbl_ResValues" runat="server" Text="" Font-Bold="true" ></asp:Label>
                                        </div><div id="Pnl_Exam" runat="server" style="float:right;clear:both;z-index:-1; "> 
                                        &nbsp;
                                        <asp:Label ID="Lbl_Entry4" runat="server" Text="Entry 4" ForeColor="Red" ></asp:Label>
                                           &nbsp;the
                                           <asp:Label ID="Txt_ExamName" runat="Server" Text=" SSLC"></asp:Label>
                                           &nbsp;,&nbsp;
                                            <asp:Label ID="Lbl_Entry5" runat="server" Text="Entry 5" ForeColor="Red"></asp:Label>
                                          
                                           . Of
                                           <asp:Label ID="Lbl_BordofExam" runat="Server" Font-Bold="true" 
                                               Text="State Board of Examination"></asp:Label>
                                           &nbsp;as a regular/Private candidate from
                                           <asp:Label ID="Lbl_SchoolName" runat="Server" Font-Bold="true"></asp:Label>
                                           &nbsp;and was placed in the &nbsp;
                                           <asp:Label ID="Lbl_Standard" runat="Server" Font-Bold="true" Text=""></asp:Label>
                                           &nbsp; division.
                                           <asp:Label ID="Lbl_Role" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="Lbl_Entry6" runat="server" Text="Entry 6" ForeColor="Red"></asp:Label>
                                           
                                           &nbsp;
                                           <asp:Label ID="Lbl_And" runat="server" Text=" and "></asp:Label>
                                           &nbsp;
                                             <asp:Label ID="Lbl_Entry7" runat="server" Text="Entry 7 " ForeColor="Red"></asp:Label>
                                          
                                           .</div> 
                                           <div ID="Pnl_General" runat="server" Visible="false">is currently studying in this school  in class
                                               <asp:Label ID="Lbl_Study" runat="server"  Font-Bold="true" Text=""></asp:Label>
                                           </div>
                                           <br />
                                         
                                        <div>
                                        His/her date of birth according to the record provided by his/her parents is 
                                        <asp:Label ID="Lbl_Dob" runat="server" Text=""  Font-Bold="true"></asp:Label>
                                        </div>
                                       
                                    </div>--%>
                                    </div>
                                    <div class="behaviour">
                                        <asp:TextBox ID="Txt_Behave" runat="server"  TextMode="MultiLine" Width="100%" Height="70px" Text=""></asp:TextBox>

                                    </div>
                                    <table width="100%" class="certificatefooter">
                                    <tr>
                                    <td align="left" style="width:33%"> <asp:Label ID="Lbl_Footer1" runat="server" Text=""></asp:Label>
                                     
                                       
                                    </td>
                                    <td align="center"  style="width:33%"> <asp:Label ID="Lbl_Footer2" runat="server" Text=""></asp:Label>
                                    
                                    </td>
                                    <td align="right"  style="width:33%"> <asp:Label ID="Lbl_Footer3" runat="server" Text=""></asp:Label>
                                   
                                    
                                    
                                    </td>
                                    </tr>
                                    </table>
                                </div>
                               
                              </asp:Panel>  
                            <br />
                        </td>
                        <td class="e"></td>
                        </tr>
                        <tr ><td class="so"> </td><td class="s"></td><td class="se"></td></tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="clear"></div>
</div>

</asp:Content>
