<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" CodeBehind="UploadDetails.aspx.cs" Inherits="WinEr.UploadDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
<div id="right">
<div class="label">Class Manager                                                     
  </div>
<div id="SubClassMenu" runat="server">		
 </div>

</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
   <div id="left">
     <div class="container skin1 "  >
       <table cellpadding="0" cellspacing="0" class="containerTable">
         <tr >
          <td  class="no">  <img alt="" src="Pics/fileimport.png" width="30" height="30" /></td>
          <td  class="n" style="font-variant:small-caps;font-weight:bold;" align="left">Import Student </td>
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
                                <asp:ImageButton ID="Template" runat="server" Height="35px" 
                            ImageUrl="Pics/Excel.png" Width="35px" onclick="Btn_template_click"/>
                           <%-- <a href="UpImage/STUDENTS%20LIST%20TEMPLATE.xls" target="_blank" 
                               title="Download Excel Format">
                           <img alt="" height="35px" src="Pics/Excel.png" 
                               style="vertical-align:middle;border-style:none" width="35px" /></a> --%>
                          
                               
                    </td></tr>
                    <tr><td colspan="2"><div class="linestyle"></div> </td></tr>
                     <tr><td colspan="2">&nbsp;</td></tr>
             
                   <%--<tr>--%>
                   <div class="col-lg-12">
                    <div class="col-lg-6">
                      
                           <asp:Label ID="Lbl_Message" class="col-lg-4" runat="server" text="Select an Excel File" ></asp:Label>
                           <asp:FileUpload ID="FileUpload_Excel" class="col-lg-8" runat="server" Height="20px" />
                           </div>
                           <div class="col-lg-6">
                           <asp:Button ID="Btn_UploadDetails" runat="server"  
                               Class="btn btn-primary" Text="Upload" onclick="Btn_UploadDetails_Click" />
                               
                           &nbsp;&nbsp<asp:Button ID="btn_upload" runat="server" onclick="btn_upload_Click" Class="btn btn-success"
                               Text="Save Details" />
                               </div>
                       <%--</td> --%>    
                       </div>                 
                  <%-- </tr>--%>
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
                                   
                               
                                <div style="overflow:auto;width:1000px;max-height:350px">
                                <asp:GridView ID="Grd_CorrectDetails" runat="server" AutoGenerateColumns="false" 
                                BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                                CellPadding="3" CellSpacing="2" Font-Size="12px"  Width="100%">
                                  
                                    <Columns>
                                      <%--  //StudentName(0)  Sex(1)  DOB(2)  Father/GuardianName(3)  Religion(4)  Caste(5)  AddressPermanent(6) StudentType(7)     AdmissionNo(8) AdmissionDate(9) JoiningBatch(10)    ReligionId(11)  CasteId(12)  StudentTypeId(13)  JoiningBatchId(14)--%>
                                    <asp:BoundField DataField="StudentName" HeaderText="StudentName" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                       <asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="50px" ControlStyle-Width="50px"/>
                                       <asp:BoundField DataField="DOB" DataFormatString="{0:d}" HeaderText="DOB" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="Father/GuardianName" HeaderText="Father Name" ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                       <asp:BoundField DataField="Religion" HeaderText="Religion" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="Caste" HeaderText="Caste" ItemStyle-Width="50px" ControlStyle-Width="50px"/>
                                       <asp:BoundField DataField="AddressPermanent" HeaderText="Address" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                       <asp:BoundField DataField="StudentType" HeaderText="Student Type" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="AdmissionNo" HeaderText="Admission No" ItemStyle-Width="80px" ControlStyle-Width="80px"/>
                                       <asp:BoundField DataField="AdmissionDate" DataFormatString="{0:d}" HeaderText="Admission Date" ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                       <asp:BoundField DataField="JoiningBatch" HeaderText="Joining Batch" ItemStyle-Width="60px" ControlStyle-Width="60px" />
                                       <asp:BoundField DataField="ReligionId" HeaderText="ReligionId" />
                                       <asp:BoundField DataField="CasteId" HeaderText="CasteId" />
                                       <asp:BoundField DataField="StudentTypeId" HeaderText="StudentTypeId" />
                                       <asp:BoundField DataField="JoiningBatchId" HeaderText="JoiningBatchId" />
                                       
                                        <asp:BoundField DataField="NewAdmission" HeaderText="New Admission" />
                                       <asp:BoundField DataField="UseBus" HeaderText="Using Bus" />
                                       <asp:BoundField DataField="UseHostel" HeaderText="Using Hostel" />
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
                                                          <div  style="overflow:auto;width:1000px; max-height:350px" >    
                                                          <asp:GridView ID="Grd_UnCorrectDtls" runat="server" AutoGenerateColumns="false"   BackColor="#EBEBEB" 
                                                            BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"      CellPadding="3" 
                                                            CellSpacing="2" Font-Size="12px"  Width="100%"  onselectedindexchanged="Grd_UnCorrectDtls_SelectedIndexChanged"  onrowdatabound="Grd_UnCorrectDtls_RowDataBound">                         
                                                     
                                                         <Columns>         
                                                     
                        <%--// SlNo(0)  StudentName(1)  Sex(2)  DOB(3)  Father/GuardianName(4)  Religion(5)  Caste(6)  AddressPermanent(7) StudentType(8)  AdmissionNo(9) AdmissionDate(10) JoiningBatch(11)   Description(12)--%>
                                                           <asp:BoundField DataField="SlNo"  HeaderText="SlNo"  />
                                                         <asp:BoundField DataField="StudentName"  HeaderText="StudentName"  
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px" />
                                                         <asp:BoundField DataField="Sex"  HeaderText="Sex" ItemStyle-Width="50px" 
                                                                 ControlStyle-Width="50px"/>
                                                          <asp:BoundField DataField="DOB"  HeaderText="DOB" ItemStyle-Width="60px" 
                                                                 ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="Father/GuardianName"  HeaderText="Father Name" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                          <asp:BoundField DataField="Religion"  HeaderText="Religion" ItemStyle-Width="80px" 
                                                                 ControlStyle-Width="80px" />
                                                          <asp:BoundField DataField="Caste"  HeaderText="Caste"  ItemStyle-Width="60px" 
                                                                 ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="AddressPermanent"  HeaderText="Address"  
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>
                                                           <asp:BoundField DataField="StudentType"  HeaderText="Student Type" 
                                                                 ItemStyle-Width="70px" ControlStyle-Width="70px"/>                                              
                                                          <asp:BoundField DataField="AdmissionNo"  HeaderText="Admission No" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>            
                                                          <asp:BoundField DataField="AdmissionDate"  HeaderText="Admission Date" 
                                                                 ItemStyle-Width="60px" ControlStyle-Width="60px"/>
                                                          <asp:BoundField DataField="JoiningBatch"  HeaderText="Joining Batch" 
                                                                 ItemStyle-Width="70px" ControlStyle-Width="70px" />  
                                                          <asp:BoundField DataField="Description"  HeaderText="Description" 
                                                                 ItemStyle-Width="100px" ControlStyle-Width="100px"/>  
                                                         
                                                           <asp:BoundField DataField="NewAdmission" HeaderText="New Admission" />
                                                           <asp:BoundField DataField="UseBus" HeaderText="Using Bus" />
                                                           <asp:BoundField DataField="UseHostel" HeaderText="Using Hostel" />
                                                                <asp:CommandField HeaderText="Edit" ShowSelectButton="True" 
                                                                 ItemStyle-Width="30" 
                                                                 
                                                                 SelectText="&lt;img src='pics/hand.png' height='25px' width='25px' border=0 title='Edit'&gt;" />   
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
              PopupControlID="Pnl_popupmsg"  TargetControlID="Btn_popupeditregion"    Enabled="True"  />
             <asp:Panel ID="Pnl_popupmsg" runat="server" DefaultButton="Btn_save"  style="display:none"><%--  style="display:none"--%>
                 <div class="container skin1" style="width: 1000px;  overflow:auto" > 
                                                 <table cellpadding="0" cellspacing="0" class="containerTable">
                                                  <tr >
                                                    <td class="no"><img alt="" src="Pics/edit.png" width="30" height="30" /> </td>
                                                    <td class="n">Edit Details</td>
                                                    <td class="ne"> </td>
                                                   </tr>
                                                  <tr >
                                                    <td class="o"> </td>
                                                    <td class="c" style="height:400px;overflow:scroll;border:#4a4a4a 1px solid " >
                                                    <div style="height:400px; overflow:auto">
                                                    <table width="100%">
                                                           <tr style="border-bottom:solid 1px Gray;">
                                                           <td colspan="3" align="right">
                                                             
                                                              Mistake found while uploading student : 
                                                             
                                                           </td>
                                                           <td colspan="3" align="left">
                                                               <asp:Label ID="lblerrordescription"  runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                            </td>
                                                          </tr>
                                                           <tr valign="top">
                                                               <td style="width:16%" align="right">
                                                                   StudentName : </td>
                                                               <td style="width:16%">
                                                                   <asp:TextBox ID="txtstudname" runat="server" Width="170px" class="form-control"></asp:TextBox>
                                                                   <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="txtstudname">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Edit" ControlToValidate="txtstudname" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                               <td style="width:16%"  align="right">
                                                                   FatherName : </td>
                                                               <td style="width:16%">
                                                                   <asp:TextBox ID="txtfathername" runat="server" Width="150px" class="form-control"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="1234567890!@#$%^&*()_+=-{}][|';:\"  TargetControlID="txtfathername">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Edit" ControlToValidate="txtfathername" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                                <td style="width:16%"  align="right"> Sex : </td>
                                                               <td style="width:16%">
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
                                                                   <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" Width="170px" class="form-control"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="!@$%^*_+={}';\"  TargetControlID="txtaddress">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Edit" ControlToValidate="txtaddress" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                   
                                                               </td>
                                                          
                                                               <td  align="right">
                                                                   D.O.B : </td>
                                                               <td>
                                                                <asp:TextBox ID="txt_Dob" runat="server"  Width="148px" class="form-control"></asp:TextBox>
                                                                 <ajaxToolkit:CalendarExtender ID="Txt_Dob_CalendarExtender" runat="server" 
                                                                  CssClass="cal_Theme1" Enabled="True" TargetControlID="txt_Dob" Format="dd/MM/yyyy">
                                                                </ajaxToolkit:CalendarExtender>
                                                              
                                                                        <asp:RegularExpressionValidator ID="DobDateRegularExpressionValidator" 
                                                        runat="server" ControlToValidate="Txt_Dob" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         /> 
                                                                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                                                            TargetControlID="DobDateRegularExpressionValidator"
                                                                            HighlightCssClass="validatorCalloutHighlight" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Edit" ControlToValidate="txt_Dob" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                                <td  align="right">
                                                                   Admission Date : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtadmdate" runat="server"  Width="138px" class="form-control"></asp:TextBox>
                                                                   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                                       CssClass="cal_Theme1" Enabled="True" TargetControlID="txtadmdate" Format="dd/MM/yyyy">
                                                                   </cc1:CalendarExtender>
                                                               
                                                                     <asp:RegularExpressionValidator ID="admdateRegularExpressionValidator" 
                                                        runat="server" ControlToValidate="Txt_Dob" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />  
                                                                   <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="Server" 
                                                                       HighlightCssClass="validatorCalloutHighlight" 
                                                                       TargetControlID="admdateRegularExpressionValidator" />
                                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Edit" ControlToValidate="txtadmdate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                              
                                                           </tr>
                                                            <tr><td colspan="6"> &nbsp;</td></tr>
                                                           
                                                           <tr id="Tr1"  valign="top" visible="false" runat="server">
                                                               <td  align="right">
                                                                   Entered Religion : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtreligion" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                               <td  align="right">
                                                                   Entered caste : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtcast" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                               <td  align="right">
                                                                   Entered Joining Batch : </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtjoinbatch" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                           </tr>
                                                           
                                                           <tr  valign="top">
                                                             
                                                               <td  align="right">
                                                                   Religion : </td>
                                                               <td>
                                                                   <asp:DropDownList ID="DrpRelegion" runat="server" Width="170px" class="form-control">
                                                                   </asp:DropDownList>
                                                               </td>
                                                               <td  align="right">
                                                                   Caste : </td>
                                                               <td>
                                                                   <asp:DropDownList ID="Drpcast" runat="server" Width="150px" class="form-control">
                                                                   </asp:DropDownList>
                                                               </td>
                                                                <td  align="right">
                                                                   JoiningBatch : </td>
                                                               <td>
                                                                   <asp:DropDownList ID="Drpjoinbatch" runat="server" Width="140px" class="form-control">
                                                                   </asp:DropDownList>
                                                               </td>
                                                           </tr>
                                                           
                                                           <tr><td colspan="6"> &nbsp;</td></tr>
                                                           
                                                           <tr id="Tr2"  valign="top" visible="false" runat="server">
                                                               <td  align="right">
                                                                   Entered Student Type
                                                               </td>
                                                               <td>
                                                                   <asp:TextBox ID="txtstudtype" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                               <td  align="right">
                                                                   Entered Admission No:
                                                               </td>
                                                               <td>
                                                                   
                                                                   <asp:TextBox ID="txtenteredadno" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                               <td  align="right">
                                                                 Entered New-Admission
                                                               </td>
                                                               <td>
                                                                 <asp:TextBox ID="TextBox1" runat="server" Enabled="false"  Width="138px" class="form-control"></asp:TextBox>
                                                               </td>
                                                           </tr>
                                                           
                                                           <tr  valign="top">
                                                              
                                                               <td  align="right">
                                                                   Student Type : </td>
                                                               <td >
                                                                   <asp:DropDownList ID="Drpstudtype" runat="server" Width="170px" class="form-control">
                                                                   </asp:DropDownList>
                                                                   </td>                                                       
                                                                <td  align="right">
                                                                   AdmissionNo : </td>
                                                               <td >
                                                                   <asp:TextBox ID="txtadmno" runat="server"  Width="148px" class="form-control"></asp:TextBox>
                                                              
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="txtadmno_FilteredTextBoxExtender" 
                                                                runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                                                InvalidChars="'\" TargetControlID="txtadmno">
                                                               </ajaxToolkit:FilteredTextBoxExtender>
                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="Edit" ControlToValidate="txtadmno" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                               <td  align="right">
                                                                 New-Admission? : </td>
                                                               <td>
                                                            <asp:RadioButtonList ID="Rdb_NewAdmission" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem  Value="1" Selected="True">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                               </td>
                                                               
                                                           </tr>
                                                            <tr><td colspan="6"> &nbsp;</td></tr>
                                                            <tr  valign="top">
                                                               <td  align="right">
                                                                   Using Bus? : </td>
                                                               <td>
                                                            <asp:RadioButtonList ID="Rdb_UseBus" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem  Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                               </td>
                                                                <td align="right">
                                                                   Using Hostel? : </td>
                                                               <td>
                                                                    <asp:RadioButtonList ID="Rdb_UseHostel" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem  Value="1" >Yes</asp:ListItem>
                                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                               </td>
                                                                <td style="display:none" align="right" >
                                                                   SlNO: </td>
                                                               <td style="display:none" visible="false">
                                                                   <asp:TextBox ID="txtslno" runat="server" Width="170px" class="form-control"></asp:TextBox>
                                                                   <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="False" FilterType="Custom"  
                                                                   FilterMode="InvalidChars" InvalidChars="!@#$%^&*()_+=-{}][|';:\"  TargetControlID="txtslno">
                                                                </ajaxToolkit:FilteredTextBoxExtender>
                                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="Edit" ControlToValidate="txtslno" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                               </td>
                                                               <td>
                                                                   </td>
                                                               <td>
                                                                  
                                                               </td>
                                                           </tr>
                                                           <tr>
                                                           <td colspan="6">
                                                            </td>
                                                           </tr>
                                                            
                                                         
                                                           <tr>
                                                           <td colspan="6"  align="center" >
                                                               <asp:Label ID="lblediterror" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                           </td>
                                                           </tr>
                                                           <tr><td colspan="6"  align="left" > <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder></td></tr>
                                                           <tr>
                                                           <td colspan="6"  align="center" >
                                                                  
                                                                  
                                                              <table> 
                                                           <tr>
                                                               <td align="center" >
                                                                   <asp:Label ID="lblselectedrowslno" runat="server" Visible="false" Text=""></asp:Label>
                                                                   <asp:Button ID="Btn_save" runat="server" onclick="Btn_save_Click" Text="Save"  ValidationGroup="Edit" Class="btn btn-success"/>&nbsp;&nbsp;&nbsp;
                                                                    <asp:Button ID="Btn_msgexit" runat="server" Text="Exit" Class="btn btn-danger" />
                                                               </td>
                                                           </tr>
                                                         </table>
                                                         
                                                           </td>
                                                           </tr>
                                                         </table> 
                                                        
                                           
                                                            <div style="overflow:auto;width:500px;display:none; max-height:150px" visible="false">
                                                            <asp:GridView ID="Grd_CorrectDtls" runat="server" AutoGenerateColumns="true" Visible="false" 
                                                                       BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                                                                       CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%">
                                                                       <FooterStyle BackColor="#CCCC99" />
                                                                       <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                       <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                       <RowStyle BackColor="#F7F7DE" />
                                                                       <Columns>
                                                                          
                                                                          <asp:BoundField DataField="StudentName" HeaderText="StudentName" />
                                                                           <asp:BoundField DataField="Sex" HeaderText="Sex" />
                                                                           <asp:BoundField DataField="DOB" DataFormatString="{0:d}" HeaderText="DOB" />
                                                                           <asp:BoundField DataField="Father/GuardianName" HeaderText="Father Name" />
                                                                           <asp:BoundField DataField="Religion" HeaderText="Religion" />
                                                                           <asp:BoundField DataField="Caste" HeaderText="Caste" />
                                                                           <asp:BoundField DataField="AddressPermanent" HeaderText="Address" />
                                                                           <asp:BoundField DataField="StudentType" HeaderText="StudentType" />
                                                                           <asp:BoundField DataField="AdmissionNo" HeaderText="AdmissionNo" />
                                                                           <asp:BoundField DataField="AdmissionDate" HeaderText="AdmissionDate" />
                                                                           <asp:BoundField DataField="JoiningBatch" HeaderText="JoiningBatch" />
                                                                           <asp:BoundField DataField="ReligionId" HeaderText="ReligionId" />
                                                                           <asp:BoundField DataField="CasteId" HeaderText="CasteId" />
                                                                           <asp:BoundField DataField="StudentTypeId" HeaderText="StudentTypeId" />
                                                                           <asp:BoundField DataField="JoiningBatchId" HeaderText="JoiningBatchId" />
                                                                           
                                                                           <asp:BoundField DataField="NewAdmission" HeaderText="NewAdmission" />
                                                                           <asp:BoundField DataField="UseBus" HeaderText="Using Bus" />
                                                                           <asp:BoundField DataField="UseHostel" HeaderText="Using Hostel" />
                                                                       </Columns>
                                                                       <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                                                       <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                       <AlternatingRowStyle BackColor="White" />
                                                                   </asp:GridView>
                                                             
                                                             
                                                            
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
               
                   
                 
               
               
               </div>
               
    </div> 
  </ContentTemplate>
  <Triggers>
  <asp:PostBackTrigger  ControlID="Btn_UploadDetails"/>
  <asp:PostBackTrigger  ControlID="Template"/>
  </Triggers>
</asp:UpdatePanel>

<div class="clear">

</div>
    
</div>
</asp:Content>
