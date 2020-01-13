
<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AdvanceStatement.aspx.cs" Inherits="WinEr.AdvanceStatement" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
                           
                           
 <div id="contents">
 
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">   <img alt="" src="Pics/evolution-tasks.png" width="30" height="30" /> </td>
				<td class="n">Advance Statement Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
<div style="min-height:300px;">
    <asp:Panel ID="Pnl_SelectionArea" runat="server">
    

<table class="tablelist">
<tr>
<td class="leftside">
    &nbsp;</td>
<td class="rightside">
    &nbsp;</td>
</tr>
    <tr>
        <td class="leftside">
            From
        </td>
        <td class="rightside">
            <asp:TextBox ID="Txt_from" runat="server" Width="170px" class="form-control"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" 
                CssClass="cal_Theme1" Enabled="True" Format="dd/MM/yyyy" 
                TargetControlID="Txt_from">
            </ajaxToolkit:CalendarExtender>
            <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
                runat="server" ControlToValidate="Txt_from" Display="None" 
                ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" 
                runat="server" Enabled="True" HighlightCssClass="validatorCalloutHighlight" 
                TargetControlID="Txt_fromRegularExpressionValidator3" />
        </td>
    </tr>
     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
<tr>
<td class="leftside">
To
</td>

<td class="rightside">
    <asp:TextBox ID="Txt_To" runat="server" Width="170px" class="form-control"></asp:TextBox>
                    
                    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender" runat="server" 
                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                      <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_To" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         /> 
            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="server" HighlightCssClass="validatorCalloutHighlight" 
                                      
            TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />
</td>
</tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
<tr>
<td class="leftside">
Type
</td>
<td class="rightside">
  <asp:DropDownList ID="Drp_Type" runat="server" AutoPostBack="True" class="form-control" 
               
                Width="170px">
                <asp:ListItem Selected="True" Text="All" Value="-1"></asp:ListItem>
                <asp:ListItem Text="CREDIT" Value="1"></asp:ListItem>
                <asp:ListItem Text="DEBIT" Value="0"></asp:ListItem>
</asp:DropDownList>
</td>
</tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
<tr>
<td class="leftside">
User
</td>
<td class="rightside">
  <asp:DropDownList ID="Drp_CollectedUser" runat="server"  class="form-control"
                Width="170px"></asp:DropDownList>
</td>
</tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
    <tr>
        <td class="leftside">
            &nbsp;</td>
        <td class="rightside">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="leftside">
            &nbsp;</td>
        <td class="rightside">
            <asp:Button ID="Btn_Load" runat="server" Class="btn btn-primary" 
                 Text="Load" onclick="Btn_Load_Click" />
        
        </td>
    </tr>
</table>
        
 
      </asp:Panel>  
        <br/>
   
 
  
    <asp:Panel ID="Pnl_trans" runat="server" Visible="False">
           
      
        <asp:Panel ID="Panel2" runat="server">
          
                 <table width="100%" >
        
     <tr>
     <td > </td>       
     <td>
        
             
     </td>
     <td style="text-align:right;">
     <asp:ImageButton ID="Btn_exporttoexel" runat="server"  Width="35" Height="35" ToolTip="Export To Excel"
             ImageUrl="Pics/Excel.png" onclick="Btn_exporttoexel_Click" />
     </td>
     </tr>
    </table>
                 <div class="linestyle"></div>
    
                            <asp:GridView ID="Grd_AdvanceItems"  runat="server"  AutoGenerateColumns="false"
             GridLines="Vertical" Width="100%" 
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px"
                   PageSize="30" AllowPaging="True" onpageindexchanging="Grd_AdvanceItems_PageIndexChanging" 
                         >
                        <Columns>
                        
                          <asp:BoundField DataField="StudentName" HeaderText="Student" />
                             <asp:BoundField DataField="FeeName" HeaderText="Fee Name" />
                              <asp:BoundField DataField="PeriodName" HeaderText="Period" />
                               <asp:BoundField DataField="BatchName" HeaderText="Batch" ItemStyle-Width="65px" />
                               
                               <asp:BoundField DataField="CreatedUser" HeaderText="Created By" />
                              <asp:BoundField DataField="CreatedDate" HeaderText="Date" ItemStyle-Width="70px" />
                               <asp:BoundField DataField="BillNo" HeaderText="BillNo" ItemStyle-Width="75px" />
                             <asp:BoundField DataField="Name" HeaderText="Type" ItemStyle-Width="75px" />
                             <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="100px" />
                             <asp:BoundField DataField="AdvanceBalance" HeaderText="Advance Account" />
                             
                        </Columns>
           <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
        </asp:GridView>
                      
	       </asp:Panel>
	

    </asp:Panel>
        
 <WC:MSGBOX id="WC_MessageBox" runat="server" />    
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
              


<div class="clear"></div>
</div>

  </ContentTemplate>
  <Triggers>
     <asp:PostBackTrigger ControlID="Btn_exporttoexel"/>   
     </Triggers>                
   </asp:UpdatePanel>
</asp:Content>
