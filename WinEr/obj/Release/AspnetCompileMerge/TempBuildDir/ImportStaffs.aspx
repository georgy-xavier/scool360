<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ImportStaffs.aspx.cs" Inherits="WinEr.ImportStaffs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />   
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage">
            <table style="height:100%;width:100%" >
            <tr><td align="center"><b>Please Wait...</b>
            <br />
            <br />
            <img src="images/indicator-big.gif" alt=""/></td></tr></table></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
   <div id="left">
     <div class="container skin1 "  >
       <table cellpadding="0" cellspacing="0" class="containerTable">
         <tr >
          <td  class="no">  <img alt="" src="Pics/fileimport.png" width="30" height="30" /></td>
          <td  class="n" style="font-variant:small-caps;font-weight:bold;" align="left">Import Staff </td>
          <td  class="ne"> </td>
          </tr>
          <tr >
          <td class="o" > </td>
          <td class="c">   
           <asp:Panel ID="Home" runat="server" style="min-height:150px" DefaultButton="btn_upload">
           <table width="100%" >
           <tr>
           <td colspan="2" align="right">                    
           <asp:Label ID="Label1" runat="server" Font-Bold="false" ForeColor="#993300" 
                               Text="Download Template"></asp:Label>
           <a href="UpImage/STAFF%20LISTTEMPLATE.xls" target="_blank" 
                               title="Download Excel Format">
          <img alt="" height="35px" src="Pics/Excel.png" 
                               style="vertical-align:middle;border-style:none" width="35px" /></a>
                               
                    </td>
                    </tr>
                    <tr><td colspan="2"><div class="linestyle"></div> </td></tr>
                     <tr><td colspan="2">&nbsp;</td></tr>
             
                   <%--<tr>
                       <td align="center" colspan="2">--%>
                       <div class="col-lg-12">
                       
                        <div class="form-inline" align="center">
                        <div class="col-lg-6" align="center">
                        
                        <asp:Label ID="selectexcel" runat="server" class="control-label col-lg-6" Text="Select an Excel File"></asp:Label>
                           <asp:FileUpload ID="FileUpload_Excel" class="col-lg-6" runat="server" Height="20px" />
                        </div>
                           
                           <div class="col-lg-6" align="left">
                           <asp:Button ID="Btn_UploadDetails" runat="server"  
                               Class="btn btn-primary" Text="Upload" onclick="Btn_UploadDetails_Click" />
                               
                          <asp:Button ID="btn_upload" runat="server" onclick="btn_upload_Click" Class="btn btn-success"
                               Text="Save Details" />
                           </div>
                          
                         </div>      
                        </div>
                        </div>

                    <%--   </td>                      
                   </tr>--%>
                   <tr><td colspan="2">&nbsp;</td></tr>
                   <tr>
                       <td align="center" colspan="2">
                           <asp:Label ID="lblerror" runat="server" Font-Bold="True" ForeColor="#993300"></asp:Label>
                           <asp:Label ID="lblcorrect" runat="server" Text=""></asp:Label>
                          <asp:Label ID="lbluncorrect" runat="server" Text=""></asp:Label>
                       </td>
                   </tr>
                 
                    </table> 
             </asp:Panel>
                </td>
       <td class="e" ></td>
       </tr>
       <tr >
       <td class="so"></td>
       <td class="s"> </td>
      <td class="se"> </td>
      </tr>
      </table>
    
     </div>
     
                                                        
                                                        
               <div id="CorrectExceldtls" runat="server">
                      <div class="container skin1">
                    <table cellpadding="0" cellspacing="0" class="containerTable">
                          <tr >
                            <td class="no"><img alt="" src="Pics/accept.png" width="30" height="30" /> </td>
                            <td class="n">Correct Details</td>
                            <td class="ne"> </td>
                           </tr>
                              <tr >
                                <td class="o"> </td>
                                <td class="c" >
                                     <%--* tbluser :  :UserName,Password,EmailId,SurName,LastLogin,CreationTime,RoleId,CanLogin,Status
                                         *
                                         *tblstaffdetails::    UserId,JoiningDate,Address,Sex,Experience,ExpDescription,Designation,PhoneNumber,EduQualifications,Dob
                                         --%>
                               
                               <div style="overflow:auto;width:950px;max-height:350px">
                                <asp:GridView ID="Grd_CorrectDetails" runat="server" AutoGenerateColumns="false" 
                                BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                                CellPadding="3" CellSpacing="2" Font-Size="12px"  Width="100%">
                                  
                                    <Columns>
                                    
                                    <asp:BoundField DataField="SurName" HeaderText="SurName" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                     <asp:BoundField DataField="UserName" HeaderText="UserName" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                     <asp:BoundField DataField="EmailId"  HeaderText="EmailId" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="Password" HeaderText="Password" ItemStyle-Width="50px" ControlStyle-Width="50px"/>
                                       <asp:BoundField DataField="Role" HeaderText="Role" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                       <asp:BoundField DataField="JoiningDate" HeaderText="Joining Date" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="50px" ControlStyle-Width="50px"/>
                                       <asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                          <asp:BoundField DataField="Experience" HeaderText="Experience" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                             <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                                <asp:BoundField DataField="Designation" HeaderText="Designation" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                                   <asp:BoundField DataField="Dob" HeaderText="Dob" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                                   <asp:BoundField DataField="EduQualifications" HeaderText="EduQualifications" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                                   <asp:BoundField DataField="IsLogin(yes/no)" HeaderText="IsLogin" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                     
                                      </Columns>
                                  
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" /> 
                                </asp:GridView>
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
</div>
               
               
              <div id="UnCorrectExcelDtls" runat="server">
               <div class="container skin1"  >
                                                 <table cellpadding="0" cellspacing="0" class="containerTable">
                                                  <tr >
                                                    <td class="no"><img alt="" src="images/cross.png" width="30" height="30" /> </td>
                                                    <td class="n">Incorrect Details</td>
                                                    <td class="ne"> </td>
                                                   </tr>
                                                   <tr >
                                                    <td class="o"> </td>
                                                    <td class="c" >  
                                                     <div  style="overflow:auto;width:950px; max-height:350px"> 
                                                          <asp:GridView ID="Grd_UnCorrectDtls" runat="server" AutoGenerateColumns="false"   BackColor="#EBEBEB" 
                                                            BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"      CellPadding="3" 
                                                            CellSpacing="2" Font-Size="12px"  Width="100%" OnSelectedIndexChanged="Grd_UnCorrectDtls_SelectedIndexChanged"    onrowdatabound="Grd_UnCorrectDtls_RowDataBound" >                         
                                                     
                                                         <Columns>         
                                                     
                        <%--// SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   Description(12)--%>
                                                           <asp:BoundField DataField="SlNo"  HeaderText="SlNo" />
                                                         <asp:BoundField DataField="SurName"  HeaderText="SurName"  
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px" />
                                                         <asp:BoundField DataField="UserName"  HeaderText="UserName" ItemStyle-Width="50px" 
                                                                 ControlStyle-Width="50px"/>
                                                          <asp:BoundField DataField="EmailId"  HeaderText="EmailId" ItemStyle-Width="60px" 
                                                                 ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="Password"  HeaderText="Password" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                          <asp:BoundField DataField="Role"  HeaderText="Role" ItemStyle-Width="80px" 
                                                                 ControlStyle-Width="80px" />
                                                          <asp:BoundField DataField="JoiningDate"  HeaderText="JoiningDate"  ItemStyle-Width="60px" 
                                                                 ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="Address"  HeaderText="Address"  
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                           <asp:BoundField DataField="Sex"  HeaderText="Sex" 
                                                                 ItemStyle-Width="70px" ControlStyle-Width="70px"/>                                              
                                                          <asp:BoundField DataField="Experience"  HeaderText="Experience" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>            
                                                          <asp:BoundField DataField="PhoneNumber"  HeaderText="PhoneNumber" 
                                                                 ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="Designation"  HeaderText="Designation" 
                                                                 ItemStyle-Width="70px" ControlStyle-Width="70px" />  
                                                          <asp:BoundField DataField="Dob"  HeaderText="Dob" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>  
                                                           <asp:BoundField DataField="EduQualifications"  HeaderText="EduQualifications" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/> 
                                                                  <asp:BoundField DataField="IsLogin(yes/no)"  HeaderText="IsLogin" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                                      <asp:BoundField DataField="Description"  HeaderText="Description" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                            <%--    <asp:CommandField ButtonType="Image" SelectImageUrl="pics/hand.png"  SelectText="Edit" ShowSelectButton="True" ItemStyle-Width="30"/> --%>
                                                          <%--<asp:CommandField HeaderText="Edit" ShowSelectButton="True" ButtonType="Link" 
                                                                 ItemStyle-Width="30" 
                                                                 
                                                                 SelectText="&lt;img src='pics/hand.png' height='25px' width='25px' border=0 title='Edit'&gt;" />--%>
                                                                   <asp:CommandField ButtonType="Image" HeaderText="Edit" SelectImageUrl="pics/hand.png" SelectText="Select" ShowSelectButton="True" ControlStyle-Height="25px" ControlStyle-Width="25px" />
                                      
                                                       </Columns>
                                                                     
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" 
                                                             VerticalAlign="Top" /> 
                                                       </asp:GridView>   
                                                  
                                                                                
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
              </div>
               
               
               
              <div id="corretionArea" runat="server" style="width:100%">
               
                <asp:Button runat="server" ID="Btn_popupeditregion" style="display:none"/>
             <cc1:modalpopupextender ID="MPE_PopUpMessageBox"   runat="server"  CancelControlID="Btn_msgexit" BackgroundCssClass="modalBackground"
              PopupControlID="Pnl_popup"  TargetControlID="Btn_popupeditregion"    Enabled="True"  />
             <asp:Panel ID="Pnl_popup" runat="server" DefaultButton="Btn_save"  style="display:none">
                 <div class="container skin1" style="width: 900px;  overflow:auto ;" > 
                                                 <table cellpadding="0" cellspacing="0" class="containerTable">
                                                  <tr >
                                                    <td class="no"><img alt="" src="Pics/edit.png" width="30" height="30" /> </td>
                                                    <td class="n">Edit Details</td>
                                                    <td class="ne"> </td>
                                                   </tr>
                                                  <tr >
                                                    <td class="o"> </td>
                                                    <td class="c" style="height:380px;overflow:scroll;border:#4a4a4a 1px solid " >
                                                    <div style="height:380px; overflow:auto">
                                                    <table width="100%">
                                                           <tr style="border-bottom:solid 1px Gray;">
                                                           <td colspan="3" align="right">
                                                             
                                                              Mistake found while uploading staff : 
                                                             
                                      
                                                           <td colspan="3" align="left">
                                                               <asp:Label ID="lblerrordescription"  runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                            </td>
                                                          </tr>
                                                          <tr>
                                                          <td>
                                                          <br />
                                                          <br />
                                                          <br />
        
                                                          </td>
                                                          </tr>
                                                           <tr valign="top">
                                                               <td style="width:15%" align="right">
                                                                   Surname : </td>
                                                               <td style="width:15%">
                                                                   <asp:TextBox ID="txtsurname" runat="server" Width="140px" class="form-control"></asp:TextBox>
                                                                   <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="txtsurname">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Edit" ControlToValidate="txtsurname" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                               <td style="width:15%"  align="right">
                                                                   UserName : </td>
                                                               <td style="width:15%">
                                                                   <asp:TextBox ID="txtusername" runat="server" Width="140px" class="form-control"> </asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="!@#$%^&*()_+=-{}][|';:\"  TargetControlID="txtusername">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Edit" ControlToValidate="txtusername" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                                <td style="width:15%"  align="right"> Sex : </td>
                                                               <td style="width:15%">
                                                                   <asp:RadioButtonList ID="RdBtnLstSex" runat="server" 
                                                                       RepeatDirection="Horizontal">
                                                                   <asp:ListItem Value="0">male</asp:ListItem>
                                                                   <asp:ListItem Value="1">female</asp:ListItem>
                                                                   </asp:RadioButtonList>     
                                                               </td>
                                                           </tr>
                                                           <tr><td colspan="6"> &nbsp;</td></tr>
                                                           <tr  valign="top"> 
                                                           <td  align="right">
                                                                   address : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" Width="140px" class="form-control"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="!@$%^*_+={}';\"  TargetControlID="txtaddress">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                  <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Edit" ControlToValidate="txtaddress" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                                   
                                                               </td>
                                                          
                                                               <td  align="right">
                                                                   D.O.B : </td>
                                                               <td>
                                                                <asp:TextBox ID="txt_Dob" runat="server"  Width="140px" class="form-control"></asp:TextBox>
                                                                 <ajaxToolkit:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" 
                                                                  CssClass="cal_Theme1" Enabled="True" TargetControlID="txt_Dob" Format="dd/MM/yyyy">
                                                                </ajaxToolkit:CalendarExtender>
                                                              
                                                                        <asp:RegularExpressionValidator ID="DobDateRegularExpressionValidator" 
                                                        runat="server" ControlToValidate="txt_Dob" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         /> 
                                                                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                                                            TargetControlID="DobDateRegularExpressionValidator"
                                                                            HighlightCssClass="validatorCalloutHighlight" />
                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Edit" ControlToValidate="txt_Dob" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                                <td  align="right">
                                                                   joing date Date : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtjndate" runat="server"  Width="140px" class="form-control"></asp:TextBox>
                                                                   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                                       CssClass="cal_Theme1" Enabled="True" TargetControlID="txtjndate" Format="dd/MM/yyyy">
                                                                   </cc1:CalendarExtender>
                                                               
                                                                     <asp:RegularExpressionValidator ID="jndateRegularExpressionValidator" 
                                                        runat="server" ControlToValidate="txtjndate" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />  
                                                                   <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="Server" 
                                                                       HighlightCssClass="validatorCalloutHighlight" 
                                                                       TargetControlID="jndateRegularExpressionValidator" />
                                                                      <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Edit" ControlToValidate="txtjndate" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                              
                                                           </tr>
                                                
                                                            <tr><td colspan="6"> &nbsp;</td></tr>
                                                           <tr valign="top">
                                                               <td  align="right">
                                                                   Email : </td>
                                                               <td>
                                                                 <asp:TextBox ID="txtemail" runat="server" Width="140px" class="form-control"></asp:TextBox>
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="Edit" ControlToValidate="txtemail" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                              <td  align="right">
                                                                   Phone No  : </td>
                                                               <td>
                                                                 <asp:TextBox ID="txtphone" runat="server" Width="140px" class="form-control"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"  
                                                                   FilterType="Numbers" TargetControlID="txtphone">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                  <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="Edit" ControlToValidate="txtphone" ErrorMessage="*"></asp:RequiredFieldValidator> --%>

                                                               </td>
                                                               <td  align="right">
                                                                   Role : </td>
                                                               <td>
                                                                <asp:DropDownList ID="drplistrolw" runat="server" Width="140px" class="form-control">
                                                                   </asp:DropDownList>

                                                               </td>
                                                           </tr>
                                                            <tr><td colspan="6"> &nbsp;</td></tr>
                                                           <tr  valign="top">
                                                             
                                                               <td  align="right">
                                                                    Experience: </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtexp" runat="server" Width="140px" class="form-control"></asp:TextBox>
                                                                    
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"  
                                                                   FilterType="Custom,numbers" ValidChars="." TargetControlID="txtexp">
                                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                               </td>
                                                              <td  align="right">
                                                                   Disignation:
                                                               </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtdisgn" runat="server" Width="140px" class="form-control"></asp:TextBox>
                              
                                                                  <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="Edit" ControlToValidate="txtdisgn" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                                               </td>
                                                                <td  align="right">
                                                                   Qualification : </td>
                                                               <td>
                                                                  <asp:TextBox ID="txteduqualification" runat="server" Width="140px" class="form-control"></asp:TextBox>
                              
                                                                  <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="Edit" ControlToValidate="txteduqualification" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                                               </td>
                                                           </tr>
                                                           
                                                            <tr><td colspan="6"> &nbsp;</td></tr>
                                                           <tr  valign="top">
                                                           <td  align="right">
                                                                 IsLogin:
                                                               </td>
                                                        <td style="width:16%">
                                                                   <asp:RadioButtonList ID="Rdbislogin" runat="server" 
                                                                       RepeatDirection="Horizontal">
                                                                   <asp:ListItem Value="0">No</asp:ListItem>
                                                                   <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                   </asp:RadioButtonList>     
                                                               </td>
                                                               <td  align="right">
                                                              
                                                               </td>
                                                               
                                                              
                                                               <td>
                                                                
                                                               </td>
                                                           </tr>
                                                           <tr ID="Tr3" runat="server" valign="top" visible="false">
                                                          <td align="right">
                                                              Entered Role :
        
                                                          </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtrole" runat="server" Enabled="false" Width="140px" class="form-control"></asp:TextBox>
                                                               </td>
                                                          </tr>
                                                           <tr>
                                                               <td colspan="6">
                                                               </td>
                                                           </tr>
                                                           <tr>
                                                               <td align="center" colspan="6">
                                                                   <asp:Label ID="lblediterror" runat="server" Font-Bold="true" ForeColor="Red" 
                                                                       Text=""></asp:Label>
                                                               </td>
                                                           </tr>
                                                           <%--  <tr><td colspan="6"  align="left" > <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder></td></tr>
                                                                       <tr>--%>
                                                           <tr>
                                                          <td>
                                                          <br />
                                                          <br />
                                                          <br />
        
                                                          </td>
                                                          </tr>
                                                           <tr>
                                                           <td colspan="6"  align="center" >  
                                                              <table> 
                                                           <tr>
                                                               <td align="center">
                                                                   <asp:Label ID="lblselectedrowslno" runat="server" Visible="false" Text=""></asp:Label>
                                                                  <asp:Button ID="Btn_save" runat="server" Text="Save" OnClick="Btn_save_Click" ValidationGroup="Edit" Class="btn btn-success"/>&nbsp;&nbsp;&nbsp;
                                                                    <asp:Button ID="Btn_msgexit" runat="server" Text="Exit" Class="btn btn-danger" />
                                                               </td>
                                                           </tr>
                                                         </table>
                                                           </td>
                                                           </tr>
                                                         </table> 
                                           
                      
                                                            
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
               </div>
               
    </div> 
  </ContentTemplate>
  <Triggers>
  <asp:PostBackTrigger  ControlID="Btn_UploadDetails"/>
  </Triggers>
</asp:UpdatePanel>

<div class="clear">

</div>
    

</asp:Content>
