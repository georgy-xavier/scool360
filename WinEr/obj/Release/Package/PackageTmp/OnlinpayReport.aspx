<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="OnlinpayReport.aspx.cs" Inherits="WinEr.OnlinpayReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
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


<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Online Payment Transaction</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					


        <br />

<center>
        <table cellspacing="10">
            


      <tr>
          
        <td class="leftside">Select Fee Header :</td>
         <td  class="rightside">

            <asp:DropDownList ID="Drp_FeeType" runat="server"  
                class="form-control" 
                Width="170px">
            
          </asp:DropDownList>
          
         </td>
        <td class="leftside">
            Select Class :
        </td>
        <td class="rightside"><asp:DropDownList ID="Drp_Class3" runat="server" 
                                   class="form-control"  Width="170px"></asp:DropDownList>
        </td>
                  <td class="leftside"> Select Status:</td>
         <td  class="rightside">

            <asp:DropDownList ID="Drp_list_Status" runat="server"  
                class="form-control" 
               Width="170px">
          </asp:DropDownList>
          
         </td>
      </tr>
           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
      </tr> 
      <tr>
         <td class="leftside">
                        Time Period :</td>
             <td  class="rightside">
                         <asp:DropDownList ID="Drp_Period" runat="server" AutoPostBack="True" class="form-control"
                            onselectedindexchanged="Drp_Period_SelectedIndexChanged" Width="170px">
                            <asp:ListItem>Today</asp:ListItem>
                            <asp:ListItem>Last Week</asp:ListItem>
                            <asp:ListItem>This Month</asp:ListItem>
                            <asp:ListItem>Manual</asp:ListItem>
                            
                          </asp:DropDownList>
                       </td>

                 <td class="leftside">
                                  From Date :</td>
                        <td class="rightside">
                           <asp:TextBox ID="Txt_StartDate" runat="server"  Width="170px" class="form-control" Enabled="False"></asp:TextBox>
                           <cc1:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
                           </cc1:CalendarExtender>  
                            <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                               runat="server" ControlToValidate="Txt_StartDate" Display="None" 
                               ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                                TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                           </td>
                             <td class="leftside">
                               To Date :</td>
                           <td  class="rightside">
                               <asp:TextBox ID="Txt_EndDate" runat="server"  Width="170px" class="form-control" Enabled="False"></asp:TextBox>
                               <cc1:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" 
                                    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>  
                                <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" 
                                     runat="server" ControlToValidate="Txt_EndDate" Display="None" 
                                     ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                     ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                TargetControlID="Txt_EndDateRegularExpressionValidator1"
                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                            </td>  

       </tr>
 
             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
              <tr>
    <td colspan="6" align="center">
     <br />
      <asp:Button ID="Btn_getAmount" runat="server" onclick="Btn_getAmount_Click"  
                Text="Show" Class="btn btn-primary" />
            
     &nbsp;&nbsp;&nbsp;
        <asp:Button ID="Btn_Export" runat="server" Class="btn btn-primary" 
                Text="Excel" onclick="Btn_Export_Click" ></asp:Button>
     
    </td>
    </tr>   
     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                  <tr>
    <td colspan="6" align="center">
       <asp:Label ID="lbl_Msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
    </td>
    </tr>
            

    </table>
 </center>   
   
        <br/>
        <asp:Panel ID="Pnl_Show" runat="server">
               
        <center>
            <br/>
         <div class="linestyle"></div>  
   
  
                <br/>
              
 
                            
                         <asp:GridView ID="Grid_fee3" runat="server" CellPadding="4" 
                          ForeColor="Black" GridLines="Vertical" Width="100%" AutoGenerateColumns="false" AllowPaging="True" 
                          onpageindexchanging="Grid_fee3_PageIndexChanging" PageSize="15" 
                          BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                          
                              <Columns>
                                 <asp:BoundField DataField="Header_Name" HeaderText="Header Name" />
                           <asp:BoundField DataField="Fee_Name" HeaderText="Fees Name" />
                           <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                           <asp:BoundField DataField="Parent_Name" HeaderText="Payer Name" />
                           <asp:BoundField DataField="ClassName" HeaderText="Class" />
                          <%-- <asp:BoundField DataField="Period" HeaderText="Period" />--%>
                           <asp:BoundField DataField="ActionDate" HeaderText="Paid Date" />
                           <asp:BoundField DataField="Amount" HeaderText="Amount" />
                           <asp:BoundField DataField="Fine" HeaderText="Fine" />
                           <asp:BoundField DataField="Biil_No" HeaderText="Bill_No" />
                           <asp:BoundField DataField="Status" HeaderText="Status" />
                              </Columns>
              
                       <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                         </asp:GridView>
                  
	      </center> 

        
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
              


<div class="clear"></div>
</div>
<WC:MSGBOX id="WC_MessageBox" runat="server" />  
      </ContentTemplate>
      <Triggers>

<asp:PostBackTrigger ControlID="Btn_Export"/>
</Triggers>
                        
</asp:UpdatePanel>
                
                   <%-- <table><tr><td colspan="3" style="text-align:center;">Fee Bill</td></tr><tr><td>Created Date:</td><td>From: TO: </td></tr></table>--%>
</asp:Content>
