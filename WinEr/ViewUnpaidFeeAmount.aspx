<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ViewUnpaidFeeAmount.aspx.cs" Inherits="WinEr.ViewUnpaidFeeAmount" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">      
        
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grid_Stud.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
    </script>

 <style type="text/css">
        .Novisibility
        {
            
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
<div id="contents">

    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Unpaid Fees List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<div align="center">
				<asp:Panel ID="Pnl_TopArea" runat="server">
				<table width="700px">
				<tr>
				<td align="right">
				Fee Type
				</td>
				<td align="left">
				<asp:DropDownList ID="Drp_Fee" runat="server" Width="160px" class="form-control" onselectedindexchanged="Drp_Period_SelectedIndexChanged"                     
                AutoPostBack="True"
				></asp:DropDownList>
				</td> 
				
				</tr>
				 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				<tr>
				<td align="right">
				Class
				</td>
				<td align="left">
				<asp:DropDownList ID="Drp_Class" runat="server" Width="160px" class="form-control"
				></asp:DropDownList>
				</td> 				
				</tr>
				 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				<tr>
				<td align="right">
				Select Period
				</td>
				<td align="left">
				<asp:DropDownList ID="Drp_Period" runat="server" Width="160px" class="form-control">				
				</asp:DropDownList>
				</td>				
				</tr>
				 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
				<tr>
				<td align="right"></td> 
				<td align="left"><asp:Button ID="Btn_Add" runat="server" Width="90px" Text="Add" class="btn btn-primary"
                        onclick="Btn_Add_Click" /></td> 
				</tr>
				</table>
				</asp:Panel>
				</div>
				
				<asp:Panel ID="Pnl_AddedFees" runat="server">
				<table width="100%">
				<tr>
				<td>
	   <asp:GridView ID="Grd_AddedFees" runat="server" AutoGenerateColumns="false" CellPadding="4"
	   
                       ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                         <Columns>
                         
                            <asp:BoundField DataField="FeeId" HeaderText="Fee Id"/>
                            <asp:BoundField DataField="PeriodId" HeaderText="Period Id"/>
                            <asp:BoundField DataField="ClassId" HeaderText="Class Id"/>                            
                            <asp:BoundField DataField="FeeName" HeaderText="Fee" />
                            <asp:BoundField DataField="PeriodName" HeaderText="Period" />
                         </Columns>
                         <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                         <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                         <RowStyle BackColor="White" BorderColor="Olive" Height="20px" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                                                                                                            
                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                      </asp:GridView>
                </td>
				</tr>
      
<tr>
<td align="right" valign="top" style="padding-right:20px"> 

 <asp:CheckBox ID="Chk_DueFee" runat="server"  Text="Due Fees"  Checked="false" />

    &nbsp;&nbsp;&nbsp;

 <asp:Button ID="Btn_Show" runat="server" Width="90px" class="btn btn-info"
        Text="Show" onclick="Btn_Show_Click" />
      
        &nbsp;
      
        </td>
</tr>                    
                      </table>
				</asp:Panel>
				  <asp:Panel runat="server" ID="Pnl_SmStext" Visible ="false">
                          <div class="linestyle"></div>
                        <table class="tablelist">
                            <tr>
                                <td valign="middle" class="leftside">&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="middle" class="leftside"><span style="font-size:medium;">SMS Text</span></td>
                                <td class="leftside"
                                    style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                    <asp:TextBox ID="Txt_SmsText" runat="server" Width="500px" class="form-control" Height="80px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                </td>
                                <td rowspan="2" valign="top">
   	                       <div style="height:150px;overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Representations of keywords"></asp:Label>
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
   	                    
                        </td>
                            </tr>
                            <tr>
                                <td></td>
                                  <td></td>
                                <td align="center"  style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                                 
                                    <asp:Button ID="Btn_Send" runat="server"  Width="111px" Class="btn btn-primary" Text="Send SMS"   OnClick="Btn_Send_Click"/>
                                </td>
                                <ajaxToolkit:ConfirmButtonExtender ID="Btn_Send_ConfirmButtonExtender" runat="server" TargetControlID="Btn_Send" ConfirmText="Are you sure you want to send the SMS"  OnClientCancel="CancelClick"></ajaxToolkit:ConfirmButtonExtender>
                                
                                <td></td>
                            </tr>
                        </table>
                           <div class="linestyle"></div>
                     </asp:Panel>
				 <asp:Panel ID="Pnl_DisplayArea" runat="server">
				 <table width="100%">
				 <tr>
				 <td align="right"> <asp:Button ID="btn_excel" runat="server" Class="btn btn-info"  
                            Text="Export" onclick="btn_excel_Click"/></td>
				 </tr>
				 <tr>
				 <td>
				 
                   <div style=" overflow:auto; height: 300px;">
                    
                      <asp:GridView ID="Grid_Stud" runat="server" AutoGenerateColumns="false" CellPadding="4"                        
                       ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                         <Columns>
                         
                            <asp:BoundField DataField="Id" HeaderText="Id"  ControlStyle-CssClass="Novisibility" ItemStyle-CssClass="Novisibility"/>
                              <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server"  Checked="true" />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)"/>
                            </HeaderTemplate>                            
                         </asp:TemplateField>
                              <asp:BoundField DataField="StudentName" HeaderText="Name" />
                            <asp:BoundField DataField="RollNo" HeaderText="RollNo" />
                            <asp:BoundField DataField="ClassName" HeaderText="Class" />                        
                            <asp:BoundField DataField="BalanceAmount" HeaderText="Balance" />
                               <asp:BoundField DataField="OfficePhNo" HeaderText="Phone Number" />
                            
                         </Columns>
                         <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                         <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                         <RowStyle BackColor="White" BorderColor="Olive" Height="20px" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                                                                                                            
                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                      </asp:GridView>
                   </div>
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
	
 <WC:MSGBOX id="WC_MessageBox" runat="server" />     
<div class="clear"></div>
</div>

 </ContentTemplate>
 
 <Triggers>
 <asp:PostBackTrigger ControlID="btn_excel" />
 </Triggers>
                    </asp:UpdatePanel>
</asp:Content>
