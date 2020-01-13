<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="ScheduleStudFee.aspx.cs" Inherits="WinEr.WebForm5" Title="Schedule Fee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
      .style1
      {
      width: 100%;
      }
      .style6
      {
      width: 146px;
      }
      .style7
      {
      }
      .style8
      {
      }
   </style>
   <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="contents">
      <div id="right">
         <div class="label">Fee Manager</div>
         <div id="SubFeeMenu" runat="server">
         </div>
      </div>
      <div id="left">
         <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
         <div class="container skin1" >
            <table cellpadding="0" cellspacing="0" class="containerTable">
               <tr >
                  <td class="no"> </td>
                  <td class="n">Schedule Fee</td>
                  <td class="ne"> </td>
               </tr>
               <tr >
                  <td class="o"> </td>
                  <td class="c" >
                     <asp:Panel ID="Panel" runat="server">
                        <div id="topstrip">
                           <table class="style1">
                              <tr>
                                 <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" Font-Bold="True" ForeColor="White" 
                                       Text="Fee"></asp:Label>
                                 </td>
                                 <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" Text="Frequency"></asp:Label>
                                 </td>
                                 <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" ForeColor="White" 
                                       Text="Yearly"></asp:Label>
                                 </td>
                              </tr>
                              <tr>
                                 <td>
                                    &nbsp;
                                 </td>
                                 <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" ForeColor="White" 
                                       Text="Associated to"></asp:Label>
                                 </td>
                                 <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" ForeColor="White" 
                                       Text="Student"></asp:Label>
                                 </td>
                              </tr>
                           </table>
                           <br/>
                        </div>
                        <%--<ajaxToolkit:TabContainer runat="server" ID="Tabs"  Width="100%"
                           CssClass="ajax__tab_yuitabview-theme" 
                           	>
                            
                           <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                           
                            <asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="Pics/user1.png" /> 
                             <b>CLASS WISE</b>
                            </HeaderTemplate>
                            
                           <ContentTemplate>
                           
                           <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                           <ContentTemplate> 
                           	<asp:Panel ID="Pnl_StudentwiseSchedule" runat="server">
                              
                           
                            <table class="tablelist">
                           	
                           	  <tr>
                           		  <td >
                           			  Class Name<span style="color:Red">*</span></td>
                           		  <td >
                           			  <asp:DropDownList ID="Drp_class2" runat="server" Height="22px" Width="122px" 
                           				  AutoPostBack="True" onselectedindexchanged="Drp_class2_SelectedIndexChanged">
                           			  </asp:DropDownList>
                           		  </td>
                           		  <td >
                           			  Period<span style="color:Red">*</span></td>
                           		  <td>
                           			  <asp:DropDownList ID="Drp_Perod2" runat="server" Height="22px" Width="122px" 
                           				  AutoPostBack="True" onselectedindexchanged="Drp_Perod2_SelectedIndexChanged">
                           			  </asp:DropDownList>
                           		  </td>
                           	  </tr>
                           	 
                           	  <tr>
                           		  <td >
                           			  Due Date<span style="color:Red">*</span></td>
                           		  <td >
                           			  <asp:TextBox ID="Txt_DueStud" runat="server" Width="120px"></asp:TextBox>
                           			   <ajaxToolkit:CalendarExtender
                           				  ID="Txt_DueStud_CalendarExtender1" runat="server" TargetControlID="Txt_DueStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                           			  </ajaxToolkit:CalendarExtender>
                           			  
                           		  </td>
                           		   <asp:RegularExpressionValidator ID="Txt_DueStudDateRE" 
                           									runat="server" ControlToValidate="Txt_DueStud" Display="None" 
                           									ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                           									 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                           									 />
                           			  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                           				  runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                           				  TargetControlID="Txt_DueStudDateRE" />
                           		  <td >
                           			  Last date<span style="color:Red">*</span></td>
                           		  <td> 
                           			  <asp:TextBox ID="Txt_LastStud" runat="server" Width="120px"></asp:TextBox>
                           			  <ajaxToolkit:CalendarExtender
                           				  ID="Txt_LastStud_CalendarExtender1" runat="server" TargetControlID="Txt_LastStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                           			  </ajaxToolkit:CalendarExtender>
                           		  </td>
                           		   <asp:RegularExpressionValidator ID="Txt_LastStudRE" 
                           									runat="server" ControlToValidate="Txt_LastStud" Display="None" 
                           									ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                           									 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                           									 />
                           			  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                           				  runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                           				  TargetControlID="Txt_LastStudRE" />
                           	  </tr>
                           	  
                           	  <tr>
                           		  <td colspan="2">
                           
                           			  <asp:Label ID="Lbl_note" runat="server" ForeColor="#FF3300"></asp:Label>
                           		  </td>
                           			  <td>
                           			 Batch</td>
                           		  <td>
                           			  <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdo_Batch_SelectedIndexChanged">
                           				  <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                           				  <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                           			  </asp:RadioButtonList>
                           			 </td>
                           		  
                           	  </tr>
                           	  <tr>
                           		  <td colspan="2">
                           		  <div id="StudCountdiv" runat="server">
                           			   <img src="Pics/user1.png" alt="" width="25px" height="22px" /> <span style="color:#FF9900"><b>10, Students Found</b></span>
                           		  </div>
                           		</td>
                           		  <td  colspan="2">
                           			  <asp:Button ID="Btn_Schdule1" runat="server" onclick="Btn_Schdule1_Click" 
                           				  Text="Schedule" Width="80px" />
                           			 &nbsp;&nbsp;&nbsp;
                           			  <asp:Button ID="Btn_Cancel1" runat="server" onclick="Btn_Cancel1_Click" 
                           				  Text="Cancel" Width="80px" />
                           		  </td>
                           	  </tr>
                           	
                            </table>
                            
                           
                            <asp:Panel ID="Pnl_AssStud" runat="server">
                           
                           
                           
                           	 <div class="roundbox">
                           <table width="100%">
                           <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
                           <tr><td class="centerleft"></td><td class="centermiddle">
                           
                           
                           <div style=" overflow:auto; max-height: 600px;">
                           	<asp:GridView ID="Grd_Amound" runat="server" AutoGenerateColumns="False" 
                           		CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="97%" OnRowDataBound="Grd_Amound_RowDataBound"
                           			  BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                           		<Columns>
                           			<asp:BoundField DataField="Id" HeaderText="Student Id" />
                           			<asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                           			<asp:BoundField DataField="RollNo" HeaderText="RollNo" />
                           			<asp:BoundField DataField="Sex" HeaderText="Sex" />
                           		   
                           			<asp:BoundField DataField="TypeName" HeaderText="Seat Type" />
                           			<asp:TemplateField HeaderText="Amount">
                           				<ItemTemplate>
                           					<asp:TextBox ID="Txt_Amound" runat="server" Height="20" MaxLength="10" Text="" 
                           						Width="75"></asp:TextBox>
                           
                           					<ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amound_FilteredTextBoxExtender" 
                           					   FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_Amound">
                           					</ajaxToolkit:FilteredTextBoxExtender>
                           
                           				</ItemTemplate>
                           			</asp:TemplateField>
                           		</Columns>
                           		<SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                           		<PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                           		<HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                               
                           		<RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                              
                           		<FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           		<EditRowStyle Font-Size="Medium" />     
                           	</asp:GridView>
                           </div>
                           
                           </td><td class="centerright"></td></tr>
                           <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
                           </table>
                           </div>	      
                           
                           
                            </asp:Panel>
                            <br />
                           </asp:Panel>
                           
                           <asp:Panel ID="Pnl_MessageBox" runat="server">
                              
                           	 <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                           	 <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                           			  runat="server" CancelControlID="Btn_magok" 
                           			  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                           	  <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                           	 <div class="container skin5" style="width:400px; top:400px;left:400px" >
                           <table   cellpadding="0" cellspacing="0" class="containerTable">
                           <tr >
                           <td class="no"> </td>
                           <td class="n"><span style="color:White">Message</span></td>
                           <td class="ne">&nbsp;</td>
                           </tr>
                           <tr >
                           <td class="o"> </td>
                           <td class="c" >
                            
                           <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                           	<br /><br />
                           	<div style="text-align:center;">
                           		
                           		<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
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
                           	</asp:Panel>       
                           
                           </ContentTemplate>
                           </asp:UpdatePanel> 
                           </ContentTemplate>
                           
                           </ajaxToolkit:TabPanel>
                           
                           <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                           
                            <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="Pics/users11.png" /> 
                             <b>ADVANCED</b>
                            </HeaderTemplate>
                            
                           <ContentTemplate>
                           
                           
                           </ContentTemplate>
                           
                           </ajaxToolkit:TabPanel>
                           
                            </ajaxToolkit:TabContainer>--%>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                           <ProgressTemplate>
                              <div id="progressBackgroundFilter">
                              </div>
                              <div id="processMessage">
                                 <table style="height:100%;width:100%" >
                                    <tr>
                                       <td align="center">
                                          <b>Please Wait...</b><br />
                                          <br />
                                          <img src="images/indicator-big.gif" alt=""/>
                                       </td>
                                    </tr>
                                 </table>
                              </div>
                           </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                           <ContentTemplate>
                              <asp:Panel ID="Pan_advfeeschdule" runat="server">
                                 <table class="tablelist">
                                    <tr>
                                       <td>
                                          &nbsp;
                                       </td>
                                       <td>
                                          &nbsp;
                                       </td>
                                       <td >
                                          &nbsp;
                                       </td>
                                       <td>
                                          &nbsp;
                                       </td>
                                    </tr>
                                    <tr>
                                       <td>
                                          Period
                                       </td>
                                       <td>
                                          <asp:DropDownList ID="Drp_periodNew" runat="server" AutoPostBack="True" class="form-control"
                                             onselectedindexchanged="Drp_periodNew_SelectedIndexChanged" 
                                             Width="122px">
                                          </asp:DropDownList>
                                       </td>
                                       <td>
                                          <asp:Label ID="Label_NextBatch" runat="server" Text="Batch"></asp:Label>
                                       </td>
                                       <td>
                                          <div class="radio radio-primary">
                                             <asp:RadioButtonList ID="Rdo_Batch1" class="form-actions" runat="server" 
                                                RepeatDirection="Horizontal" TabIndex="4" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="Rdo_Batch1_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="Current" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Next" Value="1"></asp:ListItem>
                                             </asp:RadioButtonList>
                                          </div>
                                       </td>
                                    </tr>
                                 </table>
                                 <table class="tablelist">
                                    <tr valign="top">
                                       <td style="width:50%">
                                          <div class="" >
                                             <table cellpadding="0" cellspacing="0" class="containerTable">
                                                <tr >
                                                   <td class="no"> </td>
                                                   <td class="n">Search Area</td>
                                                   <td class="ne"> </td>
                                                </tr>
                                                <tr >
                                                   <td class="o"> </td>
                                                   <td class="c">
                                                      <div style="min-height:150px">
                                                         <table class="style1">
                                                            <tr>
                                                               <td>Class Name</td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_className1" runat="server" AutoPostBack="True" 
                                                                     class="form-control" Width="128px" 
                                                                     onselectedindexchanged="Drp_className1_SelectedIndexChanged">
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Sex</td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_Sex" runat="server" Width="128px" AutoPostBack="True" class="form-control"
                                                                     onselectedindexchanged="Drp_Sex_SelectedIndexChanged">
                                                                     <asp:ListItem>Any</asp:ListItem>
                                                                     <asp:ListItem>Male</asp:ListItem>
                                                                     <asp:ListItem>Female</asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Admission Type
                                                               </td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_StudType" runat="server" Width="128px" AutoPostBack="True" class="form-control" onselectedindexchanged="Drp_StudType_SelectedIndexChanged">
                                                                     <asp:ListItem Text="Any" Value="0" Selected="True"></asp:ListItem>
                                                                     <asp:ListItem Text="Regular" Value="1"></asp:ListItem>
                                                                     <asp:ListItem Text="New Admission" Value="2"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Student Type
                                                               </td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_Seatype" runat="server" Width="128px" class="form-control" AutoPostBack="True" onselectedindexchanged="Drp_Seatype_SelectedIndexChanged">
                                                                     <asp:ListItem Text="Any" Value="0" Selected="True"></asp:ListItem>
                                                                     <asp:ListItem Text="Government Seat" Value="1"></asp:ListItem>
                                                                     <asp:ListItem Text="Management Seat" Value="2"></asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Caste</td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_Cast" runat="server" Width="128px" class="form-control"
                                                                     AutoPostBack="True" onselectedindexchanged="Drp_Cast_SelectedIndexChanged">
                                                                     <asp:ListItem Selected="True" Value="0">Any</asp:ListItem>
                                                                     <asp:ListItem Value="3">SC</asp:ListItem>
                                                                     <asp:ListItem Value="4">ST</asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Using College Bus</td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_CollegeBus" runat="server" Width="128px" class="form-control"
                                                                     AutoPostBack="True" onselectedindexchanged="Drp_Cast_SelectedIndexChanged">
                                                                     <asp:ListItem Selected="True" Value="-1">Any</asp:ListItem>
                                                                     <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                     <asp:ListItem Value="0">No</asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td class="leftside"><br></td>
                                                               <td class="rightside"><br></td>
                                                            </tr>
                                                            <tr>
                                                               <td>Using Hostel</td>
                                                               <td>
                                                                  <asp:DropDownList ID="Drp_Hostel" runat="server" Width="128px" class="form-control"
                                                                     AutoPostBack="True" onselectedindexchanged="Drp_Cast_SelectedIndexChanged">
                                                                     <asp:ListItem Selected="True" Value="-1">Any</asp:ListItem>
                                                                     <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                     <asp:ListItem Value="0">No</asp:ListItem>
                                                                  </asp:DropDownList>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td>
                                                                  &nbsp;
                                                               </td>
                                                               <td>
                                                                  &nbsp;
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td colspan="2">
                                                                  <div id="StudCountString" runat="server">
                                                                     <img src="Pics/user1.png" alt="" width="25px" height="22px" /> <span style="color:#FF9900"><b>
                                                                     0 Students Found</b></span>
                                                                  </div>
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
                                       </td>
                                       <td style="width:50%">
                                          <asp:Panel ID="Pnl_DataArea" DefaultButton="Btn_AddFee" runat="server" Enabled="false">
                                             <div class="" >
                                                <table cellpadding="0" cellspacing="0" class="containerTable">
                                                   <tr >
                                                      <td class="no"> </td>
                                                      <td class="n">Data Area</td>
                                                      <td class="ne"> </td>
                                                   </tr>
                                                   <tr >
                                                      <td class="o"> </td>
                                                      <td class="c">
                                                         <div style="min-height:150px">
                                                            <table class="style1">
                                                               <tr>
                                                                  <td>Amount</td>
                                                                  <td>
                                                                     <div class="form-inline">
                                                                        <asp:TextBox ID="Txt_amount1" runat="server" Width="100px" MaxLength="10" class="form-control"></asp:TextBox>
                                                                        <asp:Button ID="Btn_AddFee" runat="server" Text="Add" class="btn btn-primary"
                                                                           onclick="Btn_AddFee_Click" />
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_amount1_FilteredTextBoxExtender" 
                                                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_amount1">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                     </div>
                                                                  </td>
                                                               </tr>
                                                               <tr>
                                                                  <td class="leftside"><br></td>
                                                                  <td class="rightside"><br></td>
                                                               </tr>
                                                               <tr>
                                                                  <td>Due Date</td>
                                                                  <td>
                                                                     <asp:TextBox ID="Txt_NewDuetdt" Width="100px" runat="server" class="form-control"></asp:TextBox>
                                                                     <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_NewDuetdt" Format="dd/MM/yyyy">
                                                                     </ajaxToolkit:CalendarExtender>
                                                                  </td>
                                                               </tr>
                                                               <tr>
                                                                  <td class="leftside"><br></td>
                                                                  <td class="rightside"><br></td>
                                                               </tr>
                                                               <tr>
                                                                  <td>Last date
                                                                  </td>
                                                                  <td>
                                                                     <asp:TextBox ID="Txt_NewLastdt" Width="100px" runat="server" class="form-control"></asp:TextBox>
                                                                     <ajaxToolkit:CalendarExtender
                                                                        ID="CalendarExtender2" runat="server" TargetControlID="Txt_NewLastdt" Format="dd/MM/yyyy"
                                                                        CssClass="cal_Theme1" Enabled="True">
                                                                     </ajaxToolkit:CalendarExtender>
                                                                  </td>
                                                               </tr>
                                                               <tr>
                                                                  <td class="leftside"><br></td>
                                                                  <td class="rightside"><br></td>
                                                               </tr>
                                                               <tr>
                                                                  <td>
                                                                     <%--<asp:RegularExpressionValidator ID="RE_Txt_NewDuetdt" 
                                                                        runat="server" ControlToValidate="Txt_NewDuetdt" Display="None" 
                                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                                        ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$" />--%>
                                                                     <asp:RegularExpressionValidator ID="RE_Txt_NewDuetdt" 
                                                                        runat="server" ControlToValidate="Txt_NewDuetdt" Display="None" 
                                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                                        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                                        />
                                                                     <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" 
                                                                        runat="server" HighlightCssClass="validatorCalloutHighlight" 
                                                                        TargetControlID="RE_Txt_NewDuetdt" Enabled="True" />
                                                                  </td>
                                                                  <td>
                                                                  </td>
                                                               </tr>
                                                               <tr>
                                                                  <td class="leftside"><br></td>
                                                                  <td class="rightside"><br></td>
                                                               </tr>
                                                               <tr>
                                                                  <td>
                                                                     <%--<asp:RegularExpressionValidator ID="RE_Txt_NewLastdt" runat="server" 
                                                                        ControlToValidate="Txt_NewLastdt" Display="None" 
                                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                                        ValidationExpression="^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$"></asp:RegularExpressionValidator>--%>
                                                                     <asp:RegularExpressionValidator ID="RE_Txt_NewLastdt" 
                                                                        runat="server" ControlToValidate="Txt_NewLastdt" Display="None" 
                                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                                        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                                        />
                                                                     <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" 
                                                                        runat="server" Enabled="True" HighlightCssClass="validatorCalloutHighlight" 
                                                                        TargetControlID="RE_Txt_NewLastdt">
                                                                     </ajaxToolkit:ValidatorCalloutExtender>
                                                                  </td>
                                                                  <td>
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
                                       </td>
                                    </tr>
                                    <tr>
                                       <td colspan="2" align="center">
                                          <asp:Label ID="Lbl_Note1" runat="server"  ForeColor="#FF3300"></asp:Label>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td>&nbsp;</td>
                                       <td>
                                          <asp:Button ID="BtnSch2" runat="server" onclick="BtnSch2_Click"  Text="Schedule" class="btn btn-success" />
                                          &nbsp;&nbsp;
                                          <asp:Button ID="BtnCnsl2" runat="server" Text="Cancel" Width="80px" onclick="BtnCnsl2_Click" class="btn btn-danger" />
                                       </td>
                                    </tr>
                                 </table>
                                 <asp:Panel ID="Pnl_Studscreacharea" runat="server">
                                    <div class="roundbox">
                                       <table width="100%">
                                          <tr>
                                             <td class="topleft"></td>
                                             <td class="topmiddle"></td>
                                             <td class="topright"></td>
                                          </tr>
                                          <tr>
                                             <td class="centerleft"></td>
                                             <td class="centermiddle">
                                                <div style=" overflow:auto; max-height: 400px;">
                                                   <asp:GridView ID="Grd_ScrechStud" runat="server" AutoGenerateColumns="False" 
                                                      CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="97%" 
                                                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                                                      BorderWidth="1px">
                                                      <AlternatingRowStyle BackColor="White" />
                                                      <Columns>
                                                         <asp:BoundField DataField="Id" HeaderText="Student Id" />
                                                         <asp:BoundField DataField="classid" HeaderText="classid" />
                                                         <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                                         <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                                                         <asp:BoundField DataField="Sex" HeaderText="Sex" />
                                                         <asp:BoundField DataField="TypeName" HeaderText="Seat Type" />
                                                         <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                               <asp:TextBox ID="Txt_NewAmound" runat="server" Height="20" MaxLength="10" Text="" class="form-control"
                                                                  Width="75"></asp:TextBox>
                                                               <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewAmound_FilteredTextBoxExtender" 
                                                                  FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_NewAmound">
                                                               </ajaxToolkit:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                         </asp:TemplateField>
                                                      </Columns>
                                                      <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                      <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                      <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"   HorizontalAlign="Left" />
                                                      <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                                                      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                      <EditRowStyle Font-Size="Medium" />
                                                   </asp:GridView>
                                                </div>
                                             </td>
                                             <td class="centerright"></td>
                                          </tr>
                                          <tr>
                                             <td class="bottomleft"></td>
                                             <td class="bottommiddile"></td>
                                             <td class=" bottomright"></td>
                                          </tr>
                                       </table>
                                    </div>
                                 </asp:Panel>
                                 <br />
                              </asp:Panel>
                           </ContentTemplate>
                        </asp:UpdatePanel>
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
      </div>
      <div class="clear"></div>
   </div>
</asp:Content>