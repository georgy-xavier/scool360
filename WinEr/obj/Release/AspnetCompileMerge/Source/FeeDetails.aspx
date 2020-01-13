<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" Inherits="FeeDetails"  Codebehind="FeeDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 183px;
        }
        .style3
        {
        }
        .style4
        {
            width: 148px;
        }
        .noscreen
        {
            height: 0px;
        }
        .style5
        {
            
        }
        .tablehead
        {
            border-style: inherit;
             border-width: thin;
              border-color: #000000;
               background-color: #D4D4D4; 
        }
        .rowgreen
        {
           border: thin solid #339933;
            background-color: #E8FFF0 ;
        }
        .rowyellow
        {
           border: thin solid #CC9900;
            background-color: #FFFFCC; 
        }
        .rowred
        {
           border: thin solid #CC9900;
            background-color: #FFE3BB; 
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
</asp:UpdateProgress>
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Fee Schedule Details</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<div id="topstrip">
					    <table class="style1">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Fee"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" class="control-label"  Text="Frequency"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Yearly"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" class="control-label" ForeColor="White" 
                                        Text="Associated to"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Student"></asp:Label>
                                </td>
                            </tr>
                        </table>
					<br/>
					</div>
					
					
              
              
                    <asp:Panel ID="Pnl_feedetailarea" runat="server">
                    
                    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                  <ContentTemplate>
                     <table class="style1">
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td colspan="4">
                                  &nbsp;</td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  Class Name</td>
                              <td>
                                  <asp:DropDownList ID="Drp_className" runat="server" AutoPostBack="True" class="form-control"
                                      Width="150px" onselectedindexchanged="Drp_className_SelectedIndexChanged">
                                  </asp:DropDownList>
                              </td>
                              <td colspan="4">
                                  <img alt="" src="images/accept.png" style="height: 24px; width: 27px" /><b>Fee 
                                  scheduled for all students</b> </td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  <asp:Label ID="Label_NextBatch" runat="server" class="control-label" Text="Batch"></asp:Label></td>
                              <td>
                               <asp:RadioButtonList ID="Rdo_Batch" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Rdo_Batch_SelectedIndexChanged">
                                      <asp:ListItem Text="Current" Value="0" Selected="True"></asp:ListItem>
                                      <asp:ListItem Text="Next" Value="1" ></asp:ListItem>
                                 </asp:RadioButtonList>
                              </td>
                              <td colspan="4">
                                  <img alt="" src="images/warning.png" style="height: 22px; width: 22px" /><b> Fee 
                                  scheduled only for few students</b></td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td colspan="4">
                                  <img alt="" src="images/cross.png" style="height: 22px; width: 20px" /> <b>Fee 
                                  not scheduled</b></td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td style="background-color: #E8FFF0">
                                  All paid</td>
                              <td style="background-color: #FFE3BB">
                                  Last date over</td>
                              <td style="background-color: #FFFFCC">
                                  Active</td>
                              <td>
                                  Inactive</td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td colspan="4">
                                  &nbsp;</td>
                          </tr>
                      </table>
                    
                    <div class="linestyle"></div>   
                      
                      <div id="Feeschtable" runat="server">
                      
                         <table class="style1">
                         
                       
                             <tr class="tablehead">
                                 <td >
                                     Period</td>
                                 <td>
                                     Status</td>
                                 <td>
                                     Amount</td>
                                 <td>
                                     No of Stud</td>
                                 <td>
                                     No of Stud<br />
                                     fee sche</td>
                                 <td>
                                     Due Date</td>
                                 <td>
                                     Last Date</td>
                                 <td>
                                     Total</td>
                                 <td>
                                     No of Studt<br />
                                     Paid</td>
                                 <td>
                                     Total<br />
                                     Deduction</td>
                                 <td>
                                     Total Fine</td>
                                 <td>
                                     Amount<br />
                                     Collected</td>
                                 <td>
                                     Balance<br />
                                     Amount</td>
                             </tr>
                             <tr class="rowgreen">
                                 <td>
                                     Janvery-march</td>
                                 <td>
                                     <img alt="" src="images/accept.png" style="height: 37px; width: 36px" /></td>
                                 <td>
                                     2000</td>
                                 <td>
                                     30</td>
                                 <td>
                                     30</td>
                                 <td>
                                     5/5/2008</td>
                                 <td>
                                     6/6/2008</td>
                                 <td>
                                     60000</td>
                                 <td>
                                     30</td>
                                 <td>
                                     10000</td>
                                 <td>
                                     0</td>
                                 <td>
                                     50000</td>
                                 <td>
                                     0</td>
                             </tr>
                             <tr class="rowyellow">
                                 <td >
                                     Aprl - June</td>
                                 <td>
                                     <img alt="" src="images/warning.png" style="height: 37px; width: 36px" /></td>
                                 <td>
                                     2000</td>
                                 <td>
                                     30</td>
                                 <td>
                                     20</td>
                                 <td>
                                     5/5/2008</td>
                                 <td>
                                     6/6/2008</td>
                                 <td>
                                     40000</td>
                                 <td>
                                     10</td>
                                 <td>
                                     0</td>
                                 <td>
                                     200</td>
                                 <td>
                                     20200</td>
                                 <td>
                                     20000</td>
                             </tr>
                             <tr>
                                 <td>
                                     July - Sept</td>
                                 <td>
                                     <img alt="" src="images/cross.png" style="height: 37px; width: 36px" /></td>
                                 <td>
                                     0</td>
                                 <td>
                                     30</td>
                                 <td>
                                     0</td>
                                 <td>
                                     nil</td>
                                 <td>
                                     nil</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                             </tr>
                             <tr>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                         </table>
                         
                     </div>
                         <br/>  
                    
	        <asp:Panel ID="pnl_FeeRule" runat="server" >
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                  <ContentTemplate>
                  <h5>Fee Rule Details</h5>
                  
                   <div class="linestyle"></div>  
                   <asp:GridView ID="Grd_VewRuleDetails" runat="server" 
                   Visible="true" 
                      Width="100%" AutoGenerateColumns="false"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" >
                  
                     <Columns>
                     
                        <asp:BoundField DataField="RuleName" HeaderText ="Rule" /> 
                       <%-- <asp:BoundField DataField="Amount" HeaderText ="Amount" />  --%> 
                        <asp:BoundField DataField="ClassName" HeaderText ="Class Name" />    
                  <%-- <asp:TemplateField HeaderText="Description">
                   <ItemTemplate>
                   <asp:TextBox ID="Txt_Desc" runat="server" Width="98%"></asp:TextBox>
                     <ajaxToolkit:FilteredTextBoxExtender ID="Txt_WornDays_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_Desc" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                            </ajaxToolkit:FilteredTextBoxExtender>
                   </ItemTemplate>
                   </asp:TemplateField>--%>
                    
                 <%-- <asp:CommandField ShowDeleteButton="false" /> --%>
                  
                     </Columns>
                    <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                 </asp:GridView>
                    <%--<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n" style="color:Black"></td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >    
				                                                                                                              
                    
           		
				</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>--%>
	
	             
                  </ContentTemplate>
                    </asp:UpdatePanel>
	
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

