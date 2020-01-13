<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ViewBill.aspx.cs" Inherits="WinEr.WebForm11"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        function PrintDiv() {
           
            var divContentsTable = document.getElementById("feegrid").innerHTML;
            var printWindow = window.open('', '', '', 'resizable=no');
            printWindow.document.write('<html><head></style><title>Fee Bill</title> <script type="text/javascript">function FinalPrint() { document.getElementById("printButton").style.visibility="hidden"; window.print(); this.window.close();} </scrip' + "t" + '><link href="css_bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" /><link href="~/Styles/Site.css" rel="stylesheet" type="text/css" /> <link rel="stylesheet" type="text/css" href="css files/whitetheme.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/campusstyle.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" /><link rel="stylesheet" type="text/css" href="css files/winroundbox.css" title="style"  media="screen"/><link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/><link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen" />');
            printWindow.document.write('</head><body style="border-style: solid; border-width: 1px;"><div style="margin:20px;"><br />');            
            printWindow.document.write('<p style="text-decoration: underline;text-align: left;font-size: 15px;"><strong>Bills</strong></p>' + divContentsTable);//student table content
            printWindow.document.write('<br /><br /><button style="height: 40px; width:120px; margin:auto;display:block" id="printButton" onclick="FinalPrint()">Print This Report</button></input></div></body></html>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<div >
<div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no">
                    <img alt="" src="Pics/Details.png" height="30" width="30" /> </td>
				<td class="n">View And Cancel Bills</td>
				<td class="ne"> </td>
			</tr>
			
			<tr >
				<td class="o"> </td>
				<td class="c" >
	     	            
      	            <br />
      	            <asp:Panel ID="Pnl_Amount" runat="server">
        <table width="100%" >
           
          
            <tr>
                <td>
                    From<span style="color:Red">*</span></td>
                <td>
                    <asp:TextBox ID="Txt_from" runat="server" class="form-control" Width="160px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" 
                         Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="Txt_from">
                    </ajaxToolkit:CalendarExtender>
                  
                </td>
                <td>
                    To<span style="color:Red">*</span></td>
                <td>
                    <asp:TextBox ID="Txt_To" runat="server" class="form-control" Width="160px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender" runat="server" 
                         Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="Txt_To">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
           
            
            
            
            
            
       <tr>
        <td  colspan="2">
                                      <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
                                                        runat="server" ControlToValidate="Txt_from" Display="None" 
                                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />
                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                TargetControlID="Txt_fromRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" 
                                Enabled="True" />
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
        <td >Class </td>
        <td ><asp:DropDownList ID="Drp_Class" runat="server"  Width="160px" class="form-control"
                                      onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList>
        </td>
       <td >
       Bill Type
       </td>
       <td >
            <asp:RadioButtonList ID="Rdo_BillType" runat="server" AutoPostBack="True" 
                onselectedindexchanged="Rdo_BillType_SelectedIndexChanged" 
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="1">Paid</asp:ListItem>
                <asp:ListItem Value="2">Canceled</asp:ListItem>
            </asp:RadioButtonList>
       </td>
          
      </tr>
      
      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
    
      
     <tr>
        <td >Staff</td>
        <td ><asp:DropDownList ID="Drp_CollectedUser" runat="server"  class="form-control" Width="160px"></asp:DropDownList></td>
        
        
         <td >Fee Type</td>
        <td >
        <asp:DropDownList ID="Drp_FeeType" runat="server"  Width="160px" class="form-control" onselectedindexchanged="Drp_FeeType_SelectedIndexChanged"  
             AutoPostBack="true"  >
            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Rgular fee" Value="1" ></asp:ListItem>
            <asp:ListItem Text="Joining fee" Value="2" ></asp:ListItem>
        </asp:DropDownList>
        </td>
     </tr>
     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
     <tr>
        <td colspan="3"><br /></td>
     </tr>
     <tr>
        <td ></td>
        <td >
            <asp:HiddenField ID="Hdn_BillNo" runat="server" />
            <asp:HiddenField ID="Hdn_StdId" runat="server" />
        </td>
        <td align="right" colspan="2">
        <asp:Button ID="Btn_getAmount" runat="server" onclick="Btn_getAmount_Click"  Text="Show Bills" Class="btn btn-success" />
        &nbsp;&nbsp;&nbsp;
         <asp:Button ID="Img_Excel" runat="server" Enabled="False" Class="btn btn-primary"    
                Text="Export to excel" onclick="Img_Excel_Click" ></asp:Button>
            &nbsp;&nbsp;&nbsp;
             <asp:Button ID="btn_pdf" runat="server" Enabled="false" Class="btn btn-primary"    
                Text="Export to PDF" OnClientClick="PrintDiv()" ></asp:Button>
        
       </td>
     </tr>
    </table>
    

       

    <asp:Panel ID="Pnl_Bills" runat="server">
   


        <div class="linestyle"></div>
        <div id="feegrid">
    <div  style="text-align:center">
        <asp:Label ID="Lbl1_BillMessage" class="control-label" runat="server"></asp:Label>
    </div>
        
    <asp:Panel ID="Pnl_CollectedAmt" runat="server">
        
    <span style="color:Black; font-size:large">Collected Amount :</span>  
        <asp:Label ID="Lbl_Amt" Font-Size="Large" ForeColor="Chocolate" class="control-label" runat="server"></asp:Label> 
   </asp:Panel>
    
    <br/>

    
   
                           <asp:GridView ID="Grid_fee3" runat="server" CellPadding="4"    AutoGenerateColumns="false" AllowSorting="true"
                            ForeColor="Black" GridLines="Vertical" Width="100%" 
                            AllowPaging="True" DataKeyNames="Id,BillNo" onpageindexchanging="Grid_fee3_PageIndexChanging" PageSize="20"  OnSorting="Grd_FeeBillSorting"  onrowdeleting="Grid_fee3_onrowdelete" OnRowDataBound="Grid_fee3_RowDataBound"  
                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"    BorderWidth="1px" onrowediting="Grid_fee3_RowEditing">
                          <Columns>
                               <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                               <asp:BoundField DataField="name" HeaderText ="Name"  ItemStyle-Width="150px" SortExpression="name"/> 
                               <asp:BoundField DataField="class" HeaderText ="Class"  ItemStyle-Width="50px" SortExpression="class"/> 
                               <asp:BoundField DataField="Amount" HeaderText ="Amount" ItemStyle-Width="100" /> 
                               <asp:BoundField DataField="PaidDate" HeaderText ="Paid Date" ItemStyle-Width="100" /> 
                               <asp:BoundField DataField="BillNo" HeaderText ="BillNo" ItemStyle-Width="100" SortExpression="BillNo"/> 
                                <asp:TemplateField HeaderText="Cancelled Date" ItemStyle-Width="90">
                                                 <ItemTemplate>
                                                     <asp:Label ID="Lbl_CanDate" class="control-label" runat="server"></asp:Label>
                                                 </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Reason">
                                                 <ItemTemplate>
                                                     <asp:Label ID="Lbl_Reason" class="control-label" runat="server"></asp:Label>
                                                 </ItemTemplate>
                               </asp:TemplateField>
                            <asp:CommandField ItemStyle-Width="40" EditText="&lt;img src='pics/Details.png' height='35px' width='35px' border=0 title='Select bill to view'&gt;" 
                             ShowEditButton="True" HeaderText="View" />
    
                             <asp:TemplateField HeaderText="Cancel" ItemStyle-Width="40">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgCancel" runat="server" CommandArgument='<%# Eval("BillNo") %>' CommandName="Delete" ImageUrl="~/Pics/delete_page.png"  Width="35px" Height="35px"/>
                                   
                                </ItemTemplate>  
                             </asp:TemplateField> 
                          </Columns>
                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                            HorizontalAlign="Left" />
                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                     </asp:GridView>
   </div>
   </asp:Panel>
    <br/>

      	            
      	            
      	         </asp:Panel>     
      	            
					
				</td>
				<td class="e"> </td>
			</tr>
			<tr>
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>
	
	
	
	       <WC:MSGBOX id="WC_MessageBox" runat="server" />     
                     

					<asp:Panel ID="Pnl_CancelBill" runat="server">
                         <asp:Button runat="server" ID="HDNBtn_Cancel" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_CancelBill"  runat="server" CancelControlID="Btn_BillCancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Bill" TargetControlID="HDNBtn_Cancel"  />
                          <asp:Panel ID="Pnl_Bill" runat="server" style="display:none;">
                         <div class="container skin5"  >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Reason:</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    
                                        <asp:Label ID="Lbl_BillMessage" runat="server" class="control-label" Text=""></asp:Label>
                                        <div style="text-align:center">
                   
                                        <asp:TextBox ID="Txt_CancelReason" runat="server" Width="300" Height="60" TextMode="MultiLine" class="form-control" MaxLength="100"></asp:TextBox>
                                        <br />
                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_CancelReason_FilteredTextBoxExtender"
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="Txt_CancelReason">
                                         </ajaxToolkit:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="CancelFeeBills" ControlToValidate="Txt_CancelReason" ErrorMessage="Enter Reason"></asp:RequiredFieldValidator>
                                 </div>
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_CancelBill"  OnClick="Btn_CancelBill_Click" Class="btn btn-info" runat="server" Text="Ok"  ValidationGroup="CancelFeeBills" />     
                                            <asp:Button ID="Btn_BillCancel" runat="server" Text="Cancel" Class="btn btn-info"  />
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
</div>
</ContentTemplate>
<Triggers >
    <asp:PostBackTrigger ControlID="Img_Excel"/>
</Triggers>
 </asp:UpdatePanel>                   

<div class="clear"></div>
</div>
</asp:Content>

