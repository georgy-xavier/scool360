<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="TcIssueReport.aspx.cs" Inherits="WinEr.TcIssueReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
</ajaxToolkit:ToolkitScriptManager> 
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
                     
<div>
    <div id="contents">



 
<div class="container skin1" style=" width:100%;" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> 
                    <img alt="" src="Pics/delete_page.png" width="30" height="30" /></td>
                <td class="n">TC Issue Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >

            <table width="100%">
            <tr>
            <td>
              <div class="roundboxorange" style="width:100%;">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				        <div style="min-height:75px;">
				        
              <table width="100%">              
              <tr>
              <td colspan="2">
              <h5><span style="color:#366092;">PeriodWise Search </span></h5><div class="linestyle"></div>
              </td>
              </tr>
              <tr>
              <td align="right"  style="width:50%;">
              Time Period
              </td>
              <td align="left"  style="width:50%;" >
               <asp:DropDownList ID="Drp_Timeperiod" runat="server" class="form-control" AutoPostBack="True" onselectedindexchanged="Drp_Timeperiod_SelectedIndexChanged" 
                                    Width="150px"    >
                                   <asp:ListItem>Today</asp:ListItem>
                                   <asp:ListItem>This Month</asp:ListItem>
                                   <asp:ListItem>Last Week</asp:ListItem>
                                   <asp:ListItem>Manual</asp:ListItem>
                                   </asp:DropDownList>
              </td>
              </tr>
              </table>
               <div id="datesArea" runat="server" style="width:100%;">
                 <table  width="100%">     
              <tr>
              <td align="right"  style="width:50%;">
              Start Date
              </td>
              <td align="left"  style="width:50%;" >
               <asp:TextBox ID="txt_StartDate" runat="server" class="form-control" Text=""></asp:TextBox>
                                   <ajaxToolkit:TextBoxWatermarkExtender ID="txt_StartDateTBWME"  TargetControlID="txt_StartDate"  runat="server" Enabled="True" WatermarkText="dd/mm/yyyy"> 
                                    </ajaxToolkit:TextBoxWatermarkExtender>                                   
                                    <br />
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_StartDate" Format="dd/MM/yyyy" runat="server" Enabled="True" CssClass="cal_Theme1"  >  </ajaxToolkit:CalendarExtender>
                                      <asp:RegularExpressionValidator ID="txt_StartDateREV"  ControlToValidate="txt_StartDate"  runat="server"  Display="None"  ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                                       <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_StartDate" runat="server"   MaskType="Date"  CultureName="en-GB" AutoComplete="true"  Mask="99/99/9999"  UserDateFormat="DayMonthYear"  Enabled="True" > </ajaxToolkit:MaskedEditExtender>    
                                       <ajaxToolkit:ValidatorCalloutExtender ID="txt_StartDateVCE"  TargetControlID="txt_StartDateREV"   runat="server" HighlightCssClass="validatorCalloutHighlight"    Enabled="True" />
                                   
              </td>
              </tr>
              <tr>
              
              <td align="right"  style="width:50%;">
              End Date
              </td>
              <td align="left"  style="width:50%;">
               <asp:TextBox ID="txt_endDate" runat="server" class="form-control" Text=""></asp:TextBox>
                                   <ajaxToolkit:TextBoxWatermarkExtender ID="txt_endDateTBWME"  TargetControlID="txt_endDate"  runat="server" Enabled="True" WatermarkText="dd/mm/yyyy"> 
                                    </ajaxToolkit:TextBoxWatermarkExtender> 
                                    
                                    <br />
                                   <ajaxToolkit:CalendarExtender ID="txt_endDateCE" TargetControlID="txt_endDate" Format="dd/MM/yyyy" runat="server" Enabled="True" CssClass="cal_Theme1"  >  </ajaxToolkit:CalendarExtender>
                                   <asp:RegularExpressionValidator ID="txt_endDateREV"  ControlToValidate="txt_endDate"  runat="server"  Display="None"  ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"   ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                                   <ajaxToolkit:MaskedEditExtender ID="txt_endDateMEE" TargetControlID="txt_endDate" runat="server"   MaskType="Date"  CultureName="en-GB" AutoComplete="true"  Mask="99/99/9999"  UserDateFormat="DayMonthYear"  Enabled="True" > </ajaxToolkit:MaskedEditExtender>    
                                   <ajaxToolkit:ValidatorCalloutExtender ID="txt_endDateVCE"  TargetControlID="txt_endDateREV"   runat="server" HighlightCssClass="validatorCalloutHighlight"    Enabled="True" />
                                   
              </td>
             
              </tr>
              </table>
              </div>
              <table width="100%">
              
              <tr>
               <td align="center">
               <asp:Label ID="lbl_error" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
               </td>
              </tr>
              <tr>
              <td align="center">
              <asp:Button  ID="Btn_search" runat="server" Text="SEARCH" 
                      onclick="Btn_search_Click" Class="btn btn-primary" />
              
              </td>
              </tr>
              </table>
				        
				           </div>
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
              </td>
               
              
            <td valign="top" >
            
            <div class="roundboxorange">
		                <table >
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
				        <div style="min-height:75px;">
            
            <asp:Panel ID="pnl_qk" runat="server" DefaultButton="btn_qkSrch">
            <table width="100%">
            <tr>
            <td colspan="2" align="left">
                    <h5><span style="color:#366092;"> Quick Search : </span></h5><div class="linestyle"></div>
            </td>
            </tr>
            <tr>
            <td >
              <asp:TextBox ID="txt_TCNo" runat="server" Text="" class="form-control"
                     ></asp:TextBox>
                 <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_ChkNo_TextBoxWatermarkExtender" WatermarkText="TC No"
                            runat="server" Enabled="True" TargetControlID="txt_TCNo">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ChkNo_FilteredTextBoxExtender" 
                              runat="server" Enabled="True" FilterType="Custom"  FilterMode="InvalidChars" InvalidChars="!@#$%^&*()_+={}][|';:\"  TargetControlID="txt_TCNo">
                         </ajaxToolkit:FilteredTextBoxExtender> 
                            
                    
                     <ajaxToolkit:AutoCompleteExtender ID="Txt_Search_AutoCompleteExtender"  
                      runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetTCNo"  ServicePath="WinErWebService.asmx"  
                       TargetControlID="txt_TCNo" MinimumPrefixLength="1" >
                  </ajaxToolkit:AutoCompleteExtender>   
                </td>
                <td>
                    <asp:Button ID="btn_qkSrch" runat="server" Text="Search" Class="btn btn-primary" 
                        onclick="btn_qkSrch_Click" />
                </td>
            </tr>
            </table>
            </asp:Panel>
            
				        
				           </div>
				        </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
            </td>
            </tr>
                
                </table>
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
<div id="studentTcArea" runat="server" >


<div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Students Issue TC</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div id="grd_studcontents" style="overflow:auto;min-height:250px;" >
                <table width="95%">
                <tr>
                <td align="right">
                   
              <asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
                                    Width="45px" Height="45px" onclick="Img_Excel_Click"  ToolTip="Export to Excel"   />
                </td>
                </tr>
                </table>
    <asp:GridView ID="Grd_Student" runat="server" AutoGenerateColumns="false" 
        Font-Size="12px"  Width="100%"  BackColor="#e2e2c5"
    BorderColor="#e2e2c5" BorderStyle="None" BorderWidth="1px" CellSpacing="2" 
                        onselectedindexchanged="Grd_Student_SelectedIndexChanged" AllowPaging="True" onpageindexchanging="Grd_Student_PageIndexChanging" 
    onrowdeleting="Grd_Student_RowDeleting" 
    OnRowEditing="Grd_Student_Editing"
    >
    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
    <EditRowStyle Font-Size="Medium" />
    <Columns>
      <asp:BoundField DataField="Id" HeaderText="Id"  />
      <asp:BoundField DataField="StudentName"  HeaderText="Student Name" HeaderStyle-HorizontalAlign="Left"   />
      <%--<asp:BoundField DataField="ClassName"  HeaderText="Class Name"  />--%>
      <asp:BoundField DataField="GardianName"  HeaderText="Guardian Name" HeaderStyle-HorizontalAlign="Left"  />
      <asp:BoundField DataField="AdmitionNo"  HeaderText="Admission No"  HeaderStyle-HorizontalAlign="Left"/>
      <asp:BoundField DataField="Sex"  HeaderText="Sex"  HeaderStyle-HorizontalAlign="Left" />
      <asp:BoundField DataField="DateOfLeaving"  HeaderText="Date Of Leaving" HeaderStyle-HorizontalAlign="Left" />        
    <asp:CommandField  ItemStyle-Font-Bold="true" HeaderText="View Details"
        ItemStyle-Font-Size="Smaller" 
        SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='View Details'&gt;" 
        ShowSelectButton="True">
        <ControlStyle />
        <ItemStyle Font-Bold="True" Font-Size="Smaller" />
    </asp:CommandField>
        <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true"  HeaderText="View TC"
        ItemStyle-Font-Size="Smaller" 
        DeleteText="&lt;img src='Pics/ViewPdf.png' width='35px' border=0 title='View TC'&gt;" 
        ShowDeleteButton="True">
        <ControlStyle />
        <ItemStyle Font-Bold="True" Font-Size="Smaller" />
    </asp:CommandField>
       <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true"  HeaderText="View TC"
        ItemStyle-Font-Size="Smaller" 
        EditText="&lt;img src='Pics/full_page.png' width='35px' border=0 title='View TC'&gt;" 
        ShowEditButton="True">
        <ControlStyle />
        <ItemStyle Font-Bold="True" Font-Size="Smaller" />
    </asp:CommandField>

        <%--<asp:TemplateField HeaderText="Edit TC"  >
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Img_Comment" ImageUrl="~/Pics/comments.png" Width="30px" 
                                                            runat="server" onclick="editTc_Click" ></asp:ImageButton>                              
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>  --%> 
     </Columns>
     <SelectedRowStyle BackColor="#ebebeb" Font-Bold="True" ForeColor="Black"  />
                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
     <HeaderStyle BackColor="#e8e8e8" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
     <RowStyle BackColor="White" BorderColor="Olive" ForeColor="Black" Font-Bold="false" Font-Size="12px"  HorizontalAlign="Left" />
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
<p style="text-align:center;">
<asp:Label ID="lbl_studentTcAreaMsg" runat="server" class="control-label" ForeColor="Red"></asp:Label>
</p>
<div id="StaffResignArea" runat="server" >
</div>

    <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" CancelControlID="Btn_magok"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"> </td>
                            <td class="n"><span style="color:White">Student Details</span></td>
                            <td class="ne">&nbsp;</td>
                        </tr>
                        <tr >
                            <td class="o"> </td>
                            <td class="c" >
                               
                                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                                        <div id="studdtls" runat="server">
                                        
                                        </div>
                                        
                                        <br />
                                        <div style="text-align:center;">
                                            
                                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-info" Width="50px" 
                                              />
                                        
                                        </div>
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

</div>
</div>

 </ContentTemplate>
 <Triggers>
<asp:PostBackTrigger ControlID="Img_Excel" />
</Triggers>
 </asp:UpdatePanel>

</asp:Content>
