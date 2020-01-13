<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="SearchFeeAccount.aspx.cs" Inherits="WinEr.WebForm12"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

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
				<td class="n">Fee Transactions</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					

    <asp:Panel ID="Pnl_Amount" runat="server">
        <br />

<center>
        <table  width="800px">
            



            <tr>
                <td class="leftside">From</td>
                <td class="rightside"><asp:TextBox ID="Txt_from" runat="server" Width="170px" class="form-control"></asp:TextBox>
                    
                    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                      Enabled="True" TargetControlID="Txt_from" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    

                </td>
                <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_from" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />

                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                TargetControlID="Txt_fromRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" 
                                Enabled="True" />

                <td class="leftside">To</td>
                
                  <td class="rightside">
                    <asp:TextBox ID="Txt_To" runat="server" Width="170px" class="form-control"></asp:TextBox>
                    
                    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender" runat="server" 
                        CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    
                </td>

       </tr>
             

       <tr>
           

        <td  colspan="4">
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
            Class
        </td>
          

        <td class="rightside"><asp:DropDownList ID="Drp_Class3" runat="server" Width="170px" class="form-control"
                                      onselectedindexchanged="Drp_Class3_SelectedIndexChanged"></asp:DropDownList>
            


        </td>
          

        <td class="leftside">Fee Type</td>
          

        <td  class="rightside">

            <asp:DropDownList ID="Drp_FeeType" runat="server" AutoPostBack="True"  class="form-control"
                 OnSelectedIndexChanged="Drp_FeeType_SelectedIndexChanged" 
                Width="170px">
                <asp:ListItem Selected="True" Text="All" Value="0"></asp:ListItem>
                <asp:ListItem Text="Rgular fee" Value="1"></asp:ListItem>
                <asp:ListItem Text="Joining fee" Value="2"></asp:ListItem>
</asp:DropDownList>
            

 </td>
          

      </tr>
    
             <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
 

    
      <tr>
          

      <td class="leftside">
          Account</td>
          

      <td class="rightside"><asp:DropDownList ID="Drp_Account" runat="server" class="form-control"  Width="170px" onselectedindexchanged="Drp_Account_SelectedIndexChanged"></asp:DropDownList>
          
          </td>
          
                  <td class="leftside">Collected User</td>
         

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
          <asp:Label ID="LblCourse0" runat="server" class="control-label">Fees</asp:Label>
          </td>
          

      <td class="rightside">

          <asp:RadioButtonList ID="RdBtLstSelectCtgry1" runat="server" 
              AutoPostBack="True" ForeColor="Black" RepeatDirection="Horizontal" 
              onselectedindexchanged="RdBtLstSelectCtgry1_SelectedIndexChanged">
              <asp:ListItem Selected="True">All</asp:ListItem>
              <asp:ListItem>Select Category</asp:ListItem>
          </asp:RadioButtonList>          
          </td>
         </tr>
         <tr>

        <td class="leftside" colspan="2">
        <asp:Panel ID="Panel_feeCatgry" visible="false" runat="server" style="height:150px; overflow:auto;">
        <center>                  
            <asp:CheckBoxList ID="ChkBx_feeNameInAmt" runat="server" >
            </asp:CheckBoxList>
            </center>
            </asp:Panel>
        </td>
     </tr>
            

     <tr>
         

         

        <td colspan="3" class="leftside">
            

        <asp:Button ID="Btn_getAmount" runat="server" onclick="Btn_getAmount_Click"  
                Text="Get Amount" Class="btn btn-primary" />
            
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Btn_ExportAmount" runat="server" Enabled="False" Class="btn btn-primary"   OnClick="Btn_Exportamount" Text="Transactions"  ></asp:Button>&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Btn_Export" runat="server" Enabled="False" Class="btn btn-primary"    
                Text="Bills" onclick="Btn_Export_Click" ></asp:Button>
                <%--<table>
                <tr>
                <td>Transaction Report &nbsp;&nbsp;&nbsp;</td>
                <td>All Students Fee Report</td>
                </tr>
                <tr>
                <td align="center"><asp:ImageButton ID="Img_Transaction" OnClick="Btn_Exportamount" runat="server" ImageUrl="~/Pics/Excel.png" Width="40px" Height="40px" ToolTip="Transaction Report" /></td>
                <td align="center"> <asp:ImageButton ID="Img_Export" OnClick="Img_Export_Click" runat="server" ImageUrl="~/Pics/Excel.png" Width="40px" Height="40px" ToolTip="All Students Fee Report" /></td>
                </tr>
                </table>
                --%>
           
         </td>
         

     </tr>
            
            

            

    </table>
 </center>   
      </asp:Panel>  
        <br/>
 
    <asp:Panel ID="Pnl_trans" runat="server" Visible="False">
         <div class="linestyle"></div>  
   
        <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
            <asp:Panel ID="Panel2" runat="server">
          
                 <table >
        
     <tr>
     <td > Total Amount Collected : </td>       
     <td>
             <asp:Label ID="Txt_total" runat="server" Font-Bold="true" class="control-label" Font-Size="Medium">0</asp:Label>
             
     </td>
     </tr>
    </table>
                 <br />
 
                            
                         <asp:GridView ID="Grid_fee3" runat="server" CellPadding="4" 
                          ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True" 
                          onpageindexchanging="Grid_fee3_PageIndexChanging" PageSize="25" 
                          BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                          
                              
              
                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                         </asp:GridView>
                  
	       </asp:Panel>
		</td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>


    </asp:Panel>
        
 <WC:MSGBOX id="WC_MessageBox" runat="server" />    

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
<asp:PostBackTrigger ControlID="Btn_ExportAmount"/>
<asp:PostBackTrigger ControlID="Btn_Export"/>
</Triggers>
                        
                    </asp:UpdatePanel>
                
                   <%-- <table><tr><td colspan="3" style="text-align:center;">Fee Bill</td></tr><tr><td>Created Date:</td><td>From: TO: </td></tr></table>--%>
</asp:Content>
