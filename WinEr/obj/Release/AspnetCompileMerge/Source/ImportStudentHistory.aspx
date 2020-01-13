<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ImportStudentHistory.aspx.cs" Inherits="WinEr.ImportStudentHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate>
                <div id="progressBackgroundFilter"></div>
                <div id="processMessage">
                    <table style="height:100%;width:100%" >
                        <tr>
                            <td align="center">
                            <b>Please Wait...</b><br /><br />
                            <img src="images/indicator-big.gif" alt=""/></td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>       
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate> 
                <div class="container skin1" >
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                        <tr ><td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/fileimport.png"  Height="28px" Width="29px" /> </td>
                            <td class="n">Import Student History</td>
                            <td class="ne"> </td></tr>
                        <tr>
                            <td class="o"> </td>
                            <td class="c" >
                                <table width="100%" >
                                    <tr>
                                        <td>
                                        <div class="col-lg-12">            
                                                <%--<div  align="center" style="padding-top:10px;width:100%">--%>
                                                <div class="col-lg-6" >
                                                <asp:FileUpload ID="FileUploadstudent_excel"  runat="server" />
                                                </div>
                                                <div class="col-lg-6">
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" Width="100px" class="btn btn-primary" onclick="btnUpload_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" class="btn btn-success"  onclick="btnSave_Click" /> 
                                                <asp:Label ID="Label1" runat="server" Font-Bold="false" class="control-label" ForeColor="#993300" 
                                                   Text="Download Template"></asp:Label>
                                               <a href="UpImage/historystudents.xls" target="_blank" 
                                                   title="Download Excel Format">
                                               <img alt="" height="35px" src="Pics/Excel.png" 
                                                   style="vertical-align:middle;border-style:none" width="35px" /></a>
                                                <br /><asp:Label ID="lblerror" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                                            <%--</div>--%>
                                            </div>
                                          <div id="RecordsDetails" runat="server" align="left">
                                                 
                    </div>      
                     </div>
 
                                        </td>
                                     </tr>   
                                </table>
                            </td>
                            <td class="e"> </td>
                        </tr>
                        <tr ><td class="so"> </td><td class="s"></td><td class="se"> </td></tr>
                    </table>
                </div>
                <asp:Panel ID="pnlValidrecords" runat="server">
                    <div class="container skin1" style="height:200;overflow:auto" >
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr >
                                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/folder_accept.png"  Height="28px" Width="29px" /> </td>
                                <td class="n"><span style="font-size:16px;font-weight:bold">Correct Details</span></td>
                                <td class="ne"> </td>
                            </tr>
                            <tr >
                                <td class="o"> </td>
                                <td class="c" >
                                
                                 <div  style="height:300px;overflow:auto">
                                     <asp:GridView ID="grdincorrectdetails" runat="server" AllowPaging="false" 
                                         AutoGenerateColumns="False" BorderWidth="3px" CellPadding="4" Font-Size="12px" 
                                         onselectedindexchanged="grdincorrectdetails_SelectedIndexChanged" PageSize="5" 
                                         Width="100%">
                                         <Columns>
                                             <asp:BoundField DataField="slno" HeaderText="slno" />
                                             <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                             <asp:BoundField DataField="sex" HeaderText="Sex" />
                                             <asp:BoundField DataField="Date Of Birth" DataFormatString="{0:d}" 
                                                 HeaderText="Date Of Birth" />
                                             <asp:BoundField DataField="Father/Guardian Name" HeaderText="Father Name" />
                                             <asp:BoundField DataField="relogion" HeaderText="Relogion" />
                                             <asp:BoundField DataField="caste" HeaderText="caste" />
                                             <asp:BoundField DataField="Admission No" HeaderText="Admission Number" />
                                             <asp:BoundField DataField="Date Of Leaving" DataFormatString="{0:d}" 
                                                 HeaderText="Date Of Leaving" />
                                             <asp:BoundField DataField="Last Class" HeaderText="Last Class" />
                                             <asp:BoundField DataField="Joining Batch" HeaderText="Batch" />
                                             <asp:BoundField DataField="Reason For Leaving" 
                                                 HeaderText="Reason for Leaving" />
                                             <asp:BoundField DataField="TC Number" HeaderText="TC Number" />
                                             <asp:BoundField DataField="JoiningBatchId" HeaderText="JoiningBatchId" />
                                             <asp:BoundField DataField="JoiningClassId" HeaderText="JoiningClassId" />
                                             <asp:BoundField DataField="Description" HeaderText="Description" />
                                             <asp:BoundField DataField="ReligionId" HeaderText="JoiningBatchId" />
                                             <asp:BoundField DataField="CastId" HeaderText="JoiningClassId" />
                                             <asp:TemplateField HeaderText="Student Details" SortExpression="Name">
                                                 <ItemTemplate>
                                                     <h4>
                                                         <%#Eval("StudentName")%></h4>
                                                     <%#Eval("Sex")%><br />
                                                     Date of Birth : <%#Eval("Date Of Birth")%>
                                                     <br />
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Course Details">
                                                 <ItemTemplate>
                                                     Admission No :<b><%#Eval("Admission No")%></b><br />
                                                     Joining Batch :<b><%#Eval("Joining Batch")%></b>
                                                     <br />
                                                     Date Of Leaving :<b><%#Eval("Date Of Leaving")%></b>
                                                     <br />
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Other Details ">
                                                 <ItemTemplate>
                                                     Last Class :<b><%#Eval("Last Class")%></b>
                                                     <br />
                                                     TC Number :<b><%#Eval("TC Number")%></b><br />
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:CommandField SelectText="Correct" ShowSelectButton="True" />
                                         </Columns>
                                     </asp:GridView>
                                    <asp:GridView ID="grdcorrectdetails" Font-Size="12px" runat="server" AutoGenerateColumns="False" 
                                    BorderWidth="3px" CellPadding="4"  GridLines="Vertical" Width="100%" AllowPaging="false" PageSize="5">
                                    <Columns>    
                                        <asp:BoundField DataField="slno" HeaderText="slno" />
                                        <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                        <asp:BoundField DataField="sex" HeaderText="Sex" />
                                        <asp:BoundField DataField="Date Of Birth" HeaderText="Date Of Birth" DataFormatString="{0:d}"/>
                                        <asp:BoundField DataField="Father/Guardian Name" HeaderText="Father Name" /> 
                                        <asp:BoundField DataField="relogion" HeaderText="Relogion" />
                                        <asp:BoundField DataField="caste" HeaderText="caste" />
                                        <asp:BoundField DataField="Admission No" HeaderText="Admission Number" />
                                        <asp:BoundField DataField="Date Of Leaving" HeaderText="Date Of Leaving" DataFormatString="{0:d}"/>
                                        <asp:BoundField DataField="Last Class" HeaderText="Last Class" />
                                        <asp:BoundField DataField="Joining Batch" HeaderText="Batch" />   
                                        <asp:BoundField DataField="Reason For Leaving" HeaderText="Reason for Leaving" /> 
                                        <asp:BoundField DataField="TC Number" HeaderText="TC Number" /> 
                                        <asp:BoundField DataField="JoiningBatchId" HeaderText="JoiningBatchId" /> 
                                        <asp:BoundField DataField="JoiningClassId" HeaderText="JoiningClassId" /> 
                                         <asp:BoundField DataField="ReligionId" HeaderText="JoiningBatchId" /> 
                                        <asp:BoundField DataField="CastId" HeaderText="JoiningClassId" /> 
                                        
                                        <asp:TemplateField  HeaderText="Student Details" SortExpression="Name">
                                            <ItemTemplate>
                                                <h4><%#Eval("StudentName")%></h4>
                                                    <%#Eval("Sex")%><br />
                                                    Date of Birth : <%#Eval("Date Of Birth")%> <br />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="Course Details">
                                            <ItemTemplate>
                                                Admission No :<b><%#Eval("Admission No")%></b><br />
                                                Joining Batch :<b><%#Eval("Joining Batch")%></b> <br />
                                                Date Of Leaving :<b><%#Eval("Date Of Leaving")%></b> <br />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField  HeaderText="Other Details ">
                                            <ItemTemplate>
                                                Last Class :<b><%#Eval("Last Class")%></b> <br />
                                                TC Number :<b><%#Eval("TC Number")%></b><br />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Label ID="lblcorrect" class="control-label" runat="server"></asp:Label> 
                                 </div>   
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
                <asp:Panel ID="pnlInvalidrecords" runat="server">
                    <div class="container skin1">
                        <table cellpadding="0" cellspacing="0" class="containerTable">
                            <tr >
                                <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/delete_folderqq.png"  Height="28px" Width="29px" />  </td>
                                <td class="n"><span style="font-size:16px;font-weight:bold">InCorrect Details</span></td>
                                <td class="ne"> </td>
                            </tr>
                            <tr >
                                <td class="o"> </td>
                                <td class="c" >
                                 <div  style="height:300px;overflow:auto">
                                    <asp:Label ID="lblincorrect" runat="server"></asp:Label>
                                    </div>
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
                <!--MessageBox-->
               <asp:Button runat="server" ID="Btn_Edit" style="display:none"/>
                <ajaxToolkit:ModalPopupExtender ID="MPE_popup"  runat="server"  PopupControlID="Pnl_msg6" TargetControlID="Btn_Edit"  CancelControlID="Btn_msgexit" BackgroundCssClass="modalBackground" />
                
                    <asp:Panel ID="Pnl_msg6" runat="server" style=" min-height:300px;_height:300px;display:none;" ><%--display:none;--%>
                        <div class="container skin1" style="width:400px; top:400px;left:100px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"></td><td class="n"></td><td class="ne"></td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                    <td class="c" > 
                                        <asp:Label ID="lblselectedrowslno" runat="server" class="control-label" Visible="false" ></asp:Label>
                                    <table class="tablelist" width="100%">
                                        <tr>
                                            <td colspan="2" align="center">
                                              <asp:Label ID="lblerrordescription"  runat="server" class="control-label" Text="" ForeColor="Red"></asp:Label>
                                              <div class="linestyle"></div>
                                            </td>
                                            
                                        </tr>
                                        
                                        <tr>
                                            <td class="leftside"> StudentName :</td>
                                            <td class="rightside"><asp:TextBox ID="txtstudname" runat="server" Width="150px"></asp:TextBox>                                                                                                                                   
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" class="form-control" ControlToValidate="txtstudname" ErrorMessage="Enter Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                           
                                            </td>
                                        </tr>
                                        <tr >
                                            <td  class="leftside">Sex :</td>
                                            <td class="rightside">
                                                <asp:RadioButtonList ID="RdBtnLstSex" runat="server" class="form-actions" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">male</asp:ListItem>
                                                    <asp:ListItem Value="1">female</asp:ListItem>
                                                </asp:RadioButtonList>     
                                            </td>
                                       </tr> 
                                        <tr>
		                                    <td class="leftside">Date of Birth : </td>
		                                    <td class="rightside"> 
		                                    
		                                        <asp:TextBox ID="txt_Dob" Text="10/10/2010" runat="server" class="form-control" Width="150px"></asp:TextBox> 
                                                <ajaxToolkit:MaskedEditExtender ID="txt_Dob_MaskedEditExtender" runat="server"  
                                                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                Mask="99/99/9999"
                                                UserDateFormat="DayMonthYear"
                                                Enabled="True" 
                                                TargetControlID="txt_Dob">
                                                </ajaxToolkit:MaskedEditExtender>
                                                <span style="color:Blue">DD/MM/YYYY</span>

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Dob" ErrorMessage="You Must enter D.O.B" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"  ValidationGroup="Save"
                                                ControlToValidate="txt_Dob"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                                <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                                TargetControlID="DobDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                
                              
                                            </td>
		                                </tr>
		                                
		                                <tr>
		                                <td class="leftside">
		                                Father Name : </td>
		                                 <td class="rightside">
		                                   <asp:TextBox ID="Tx_FatherName" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                            
		                                 </td>
		                                </tr>
                                        <tr>
                                            <td class="leftside">Admission Number :</td>
                                            <td  class="rightside">
                                                <asp:TextBox ID="txtadmissionno" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtadmissionno" ErrorMessage="You must enter admission No." ValidationGroup="Save"></asp:RequiredFieldValidator>
                                           
                                            </td>
                                        </tr>
                                       <tr>
		                                    <td class="leftside">Religion :</td>
		                                    <td class="rightside">
		                                    <asp:DropDownList ID="Drp_Religoin" Width="153px" class="form-control" runat="server">
		                                    </asp:DropDownList>
                                            
		                                 </td>
		                                </tr>
		                                <tr>
		                                <td class="leftside">
		                                Caste : </td>
		                                 <td class="rightside">
		                                   <asp:DropDownList ID="Drp_Cast" Width="153px" class="form-control" runat="server">
		                                   </asp:DropDownList>
                                            
		                                 </td>
		                                </tr>
		                                <tr>
		                                <td class="leftside">
		                                Date of Leaving
		                                </td>
		                                <td class="rightside">
		                                    <asp:TextBox ID="Txt_DOL" Text="10/10/2010" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                Mask="99/99/9999"
                                                UserDateFormat="DayMonthYear"
                                                Enabled="True" 
                                                TargetControlID="Txt_DOL">
                                                </ajaxToolkit:MaskedEditExtender>
                                                <span style="color:Blue">DD/MM/YYYY</span>
                                                
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"   ValidationGroup="Save" runat="server" ControlToValidate="Txt_DOL" ErrorMessage="You Must enter date of leaving"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="DolDateRegularExpressionValidator3" ValidationGroup="Save"
                                                ControlToValidate="Txt_DOL"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                                TargetControlID="DolDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" />
		                                 </td>
		                                </tr>
		                                
		                                 <tr>
		                                <td class="leftside">
		                                Last Class :</td>
		                                 <td class="rightside">
		                                  <asp:DropDownList ID="Drp_Class" class="form-control" Width="153px" runat="server">
		                                  </asp:DropDownList>
                                            
		                                 </td>
		                                </tr>
		                                   <tr>
		                                <td class="leftside">
		                                Joining Batch :</td>
		                                 <td class="rightside">
		                                  <asp:DropDownList ID="Joining_Batch" class="form-control" Width="153px" runat="server">
		                                  </asp:DropDownList>
                                            
		                                 </td>
		                                </tr>
		                                 <tr>
		                                <td class="leftside">
		                                TC No :</td>
		                                 <td class="rightside">
		                                  <asp:TextBox ID="Txt_Tcno" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                            
		                                 </td>
		                                </tr>
		                                 <tr>
		                                <td class="leftside">
		                                Reason for Leaving  :</td>
		                                 <td class="rightside">
		                                  <asp:TextBox ID="Txt_reason" runat="server" Width="150px" class="form-control" TextMode="MultiLine"> </asp:TextBox>
                                            
		                                 </td>
		                                </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                               
                                                <asp:Button ID="Btn_savetodatabase" runat="server"   ValidationGroup="Save" class="btn btn-info" onclick="Btn_savetodatabase_Click"  Text="Save"  Width="100px" />
                                             <asp:Button ID="Btn_msgexit" runat="server" Text="Exit" class="btn btn-info"  Width="100px" /> <br />
                                                <asp:Label ID="lbluncorrect" runat="server" class="control-label"  ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />  
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
                
                 <asp:Button runat="server" ID="Btn_Details" class="btn btn-info" style="display:none"/>
                <ajaxToolkit:ModalPopupExtender ID="MPE_Details"  runat="server"  PopupControlID="Pnl_Details" TargetControlID="Btn_Details"  CancelControlID="Btn_Close" BackgroundCssClass="modalBackground" />
                    <asp:Panel ID="Pnl_Details" runat="server" style="display:none">
                        <div class="container skin1" style="width:700px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="font-size:16px;font-weight:bold">Existing student list</span></td>
                                    <td class="ne"> </td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                        <td class="c" >
                                        <div align="center">
                                        Some students already exist.If no need to create these students please select the students from the list and click Delete button and click Continue. Other wise click Continue button.If you click Continue button these students  will create again.
                                        </div>
                                        <div>
                                        <asp:GridView ID="GrdStudlist" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical"   >                         

                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />

                                        <Columns>         
                                        <asp:TemplateField >
                                        <ItemTemplate>
                                        <asp:CheckBox ID="Chk_Select" runat="server"  />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SlNo"  HeaderText="List Order "   />
                                        <asp:BoundField DataField="StudentName"  HeaderText="StudentName"   />
                                        <asp:BoundField DataField="sex"  HeaderText="sex"/>
                                        <asp:BoundField DataField="Date Of Birth"  HeaderText="Date Of Birth"/>
                                        <asp:BoundField DataField="Admission No"  HeaderText="Admission No"/>
                                        </Columns>
                                        </asp:GridView>
                                        <br />
                                        <div align="right">
                                        <asp:Button ID="Btn_Continue" runat="server" Text="Continue" class="btn btn-info"  OnClick="Btn_Continue_Click"/>
                                        <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-info" OnClick="Btn_Delete_Click"/>
                                        <asp:Button ID="Btn_Close" runat="server" class="btn btn-info" Text="Close" 
                                          />
                                        </div>
                                        <div align="center">
                                        <asp:Label Id="Lbl_DetailsError" class="control-label" runat="server"></asp:Label>
                                        </div>
                                        </div>

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

              
              
               <asp:Button runat="server" ID="Btn_msgpop" class="btn btn-info" style="display:none"/>
                <ajaxToolkit:ModalPopupExtender ID="MPE_MSG_Pop"  runat="server"  PopupControlID="Pnl_MSG_pop" TargetControlID="Btn_msgpop"  CancelControlID="Btn_Ok" BackgroundCssClass="modalBackground" />
                    <asp:Panel ID="Pnl_MSG_pop" runat="server" style="display:none">
                        <div class="container skin1" style="width:700px">
                            <table cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="font-size:16px;font-weight:bold">Existing student list</span></td>
                                    <td class="ne"> </td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                        <td class="c" >
                                        <div align="center">
                                            <asp:Label ID="Lbl_Message" class="control-label" runat="server" Text=""></asp:Label>
                                          </div>
                                        <div>
                                        
                                        <div align="right">
                                          <asp:Button ID="Btn_Ok" runat="server" class="btn btn-info" Text="Close"  Width="100px"/>
                                        </div>
                                        
                                        </div>

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
            <Triggers>
            <asp:PostBackTrigger  ControlID="btnUpload"/>
          </Triggers>
        </asp:UpdatePanel> 
    </div>
</asp:Content>

