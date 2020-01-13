<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EditStudFeeSchdule.aspx.cs" Inherits="WinEr.WebForm8"  %>
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
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Edit Fee Schedule</td>
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
                                    &nbsp;</td>
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
					<br />
                  <asp:Panel ID="Pnl_Details" runat="server">
                      <table class="tablelist">
                        
                          <tr>
                              <td  class="leftside">
                                  Class Name<span style="color:Red">*</span></td>
                              <td >
                                  <asp:DropDownList ID="Drp_class2" runat="server" class="form-control"  Width="122px" 
                                      AutoPostBack="True" onselectedindexchanged="Drp_class2_SelectedIndexChanged">
                                  </asp:DropDownList>
                              </td>
                              <td  class="leftside">
                                  Period<span style="color:Red">*</span></td>
                              <td>
                                  <asp:DropDownList ID="Drp_Perod2" runat="server"  class="form-control" Width="122px" 
                                      AutoPostBack="True" onselectedindexchanged="Drp_Perod2_SelectedIndexChanged">
                                  </asp:DropDownList>
                              </td>
                          </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                         
                          <tr>
                              <td  class="leftside">
                                  Due Date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_DueStud" runat="server"  Width="120px" class="form-control"></asp:TextBox>
                                   <ajaxToolkit:CalendarExtender
                                      ID="Txt_DueStud_CalendarExtender1" runat="server" TargetControlID="Txt_DueStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                               <asp:RegularExpressionValidator ID="Txt_DueStud_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_DueStud" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />   
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_DueStud_DateRegularExpressionValidator3" />
                              <td  class="leftside">
                                  Last date<span style="color:Red">*</span></td>
                              <td>
                                  <asp:TextBox ID="Txt_LastStud" runat="server" Width="120px" class="form-control"></asp:TextBox>
                                  <ajaxToolkit:CalendarExtender
                                      ID="Txt_LastStud_CalendarExtender1" runat="server" TargetControlID="Txt_LastStud" CssClass="cal_Theme1" Format="dd/MM/yyyy">
                                  </ajaxToolkit:CalendarExtender>
                              </td>
                              
                                      <asp:RegularExpressionValidator ID="Txt_LastStud_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_LastStud" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                  <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                      runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                                      TargetControlID="Txt_LastStud_DateRegularExpressionValidator3" />
                                  
                          </tr>
                         <tr>
                            <td class="leftside"></td> 
                            <td></td>
                            <td class="leftside"><asp:Label ID="Label_NextBatch" runat="server" Text="Batch"></asp:Label></td>
                            <td>
                                  <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" type="radio" OnSelectedIndexChanged="Rdo_Batch_SelectedIndexChanged">
                                      <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                                 </asp:RadioButtonList>
                            </td>
                         </tr>
                          <tr>
                              <td >
                                  <asp:TextBox ID="Txt_schduleId" runat="server" Height="25px" Visible="False" 
                                      Width="79px"></asp:TextBox>
                              </td>
                              <td >
                                  &nbsp;</td>
                              <td  colspan="2">
                                  <asp:Button ID="Btn_Update" runat="server"  class="btn btn-primary"
                                      Text="Update" Width="80px" onclick="Btn_Update_Click" />
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="Btn_Cancel1" runat="server"  class="btn btn-danger"
                                      Text="Cancel" Width="80px" onclick="Btn_Cancel1_Click" />
                              </td>
                          </tr>
                      </table>

                  </asp:Panel>

                 
                  <asp:Panel ID="Pnl_AssStud" runat="server">
                 
                 
                 
                 
                   <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		
                 <div style=" overflow:auto; max-height: 400px;">
                        <asp:GridView ID="Grd_Amound" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="97%" OnRowDataBound="Grd_Amound_RowDataBound" 
                                  BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                <asp:BoundField DataField="RollNo" HeaderText="Roll No" />
                                <asp:BoundField DataField="Sex" HeaderText="Sex" />
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Amound" runat="server" Height="20" MaxLength="15" Text="" class="form-control"
                                            Width="75"></asp:TextBox>
                                      
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amound_FilteredTextBoxExtender" 
                                           FilterType="Custom, Numbers"  ValidChars="."  runat="server" Enabled="True" TargetControlID="Txt_Amound">
                                        </ajaxToolkit:FilteredTextBoxExtender>
                                      
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Txt_Status" runat="server" Height="20"  Text="" class="form-control"
                                            Width="100" ReadOnly="true"></asp:TextBox>
                                
                                       
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
                 
           </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	           
                 

                  </asp:Panel>
                  <br />
            
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
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary" Width="50px"/>
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

</div>

<div class="clear"></div>
</div>

</asp:Content>

