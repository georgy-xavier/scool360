<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Announcements.aspx.cs" Inherits="WinEr.Announcements" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
    <style type="text/css">
        .style6
        {
            height: 26px;
        }
        .style7
        {
            width: 322px;
        }
        .style8
        {
            height: 26px;
            width: 322px;
        }
        .style9
        {
            height: 10px;
        }
        .style10
        {
            width: 322px;
            height: 30px;
        }
        .style11
        {
            height: 30px;
        }
        .hide
        {
            display:none;
        }
        </style>
                   	        

    
<script type="text/javascript">
    function myScript() {
        var drpVal = $("select[data-drp='drp']").val();

        if (drpVal == "1") {
            $('#rowLink').removeClass('hide');
            $('#rowFileUpload').addClass('hide');
        }
        else if (drpVal == "2") {
            $('#rowLink').addClass('hide');
            $('#rowFileUpload').removeClass('hide');
        }
        else {
            $('#rowLink').addClass('hide');
            $('#rowFileUpload').addClass('hide');
        }
    }
</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
           
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>               
                
                        <div id="progressBackgroundFilter">

                        </div>

                        <div id="processMessage">

                        <table style="height:100%;width:100%" >

                        <tr>

                        <td +->

                        <b>Please Wait...</b><br />

                        <br />

                        <img src="images/indicator-big.gif" alt=""/></td>

                        </tr>

                        </table>

                        </div>
                                        
                      
                </ProgressTemplate>
</asp:UpdateProgress>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
       <asp:Panel ID="Panel1" runat="server" >

    
        <div class="container skin1" >
		<table align="center" cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">General Announcements</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				 <table cellspacing="10"   width="100%">
				  <tr>
					  <td align="right" class="style7">
					  
					   <%--<marquee behavior="scroll" direction="left" scrollamount="5">
					     Please select template for sending sms. Sms without selecting template may not be delivered to all numbers. Before sending, replace symbol ($ $) parts with correct data in template.
					   </marquee>--%><asp:Label ID="Label7" class="control-label" runat="server" Text="Title:"></asp:Label>
					      <asp:Label ID="Label8" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
					      </td>
					      
					      <td >
                              <asp:TextBox ID="txt_Title" runat="server" class="form-control" Width="250px" 
                                CausesValidation="true" ValidationGroup="Req" 
                                  ontextchanged="txt_Title_TextChanged"></asp:TextBox>
                                  <ajaxToolkit:FilteredTextBoxExtender ID="Exam_nameFilteredTextBoxExtender1" runat="server"
                                                                        TargetControlID="txt_Title" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\" />
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                  ControlToValidate="txt_Title" ErrorMessage="Please Enter Title Here.."
                                  ValidationGroup="Req" 
                                  SetFocusOnError="True" ></asp:RequiredFieldValidator>
                              <asp:Label ID="lbltitle" runat="server" ForeColor="Red"></asp:Label>
                      </td>
			        </tr>
			        <tr>
			        
			        <td align="right" class="style7">			        
			            <asp:Label ID="Label3" runat="server" class="control-label"  Text="Body:"></asp:Label>
			            </td>
			           <td >
                           <asp:TextBox ID="txt_body" runat="server" class="form-control" Height="73px" TextMode="MultiLine" 
                               Width="250px"></asp:TextBox>
                           </td>
			        </tr>
			        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
			        <tr>
			        <td align="right" class="style7">
                        <asp:Label ID="lbl_cls" runat="server" class="control-label"  Text="Select Category:"></asp:Label>
                        
                        </td>
                        <td>
                        <div class="radio radio-primary">
                            <asp:RadioButtonList ID="RdBtLstSelectCtgry1" class="form-actions" runat="server" 
                                AutoPostBack="True" CellSpacing="2" 
                                onselectedindexchanged="RdBtLstSelectCtgry1_SelectedIndexChanged" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">All</asp:ListItem>
                                <asp:ListItem >Class</asp:ListItem>
                                <asp:ListItem>Selected Students</asp:ListItem>
                            </asp:RadioButtonList>
                            </div>
                        </td>
			        </tr>
			        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
			        <tr>
			        <td align="right" class="style7">
			            <asp:Label ID="lblSC" runat="server" Text="Select Class:" class="control-label" Visible="False"></asp:Label>
			        
			        </td>
			        <td>
			        
			            <asp:DropDownList ID="Drp_Class" runat="server" 
                            onselectedindexchanged="Drp_Class_SelectedIndexChanged" class="form-control" Width="250px" 
                            Visible="False" AutoPostBack="True">
                        </asp:DropDownList>
			        
			            <asp:Label ID="lblstudentmsg" runat="server" class="control-label" ForeColor="Red" Text="No Students In Selected Class." 
                            Visible="False"></asp:Label>
			        
			        </td>
			        </tr>
			        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
			        <tr>
			        <td class="style7">			       
			            			        
			        </td>
			        <td>
			        <asp:Panel ID="Panel_Students" runat="server" 
                            style="height:150px;Width:200px; overflow:auto;" visible="false">
                            <asp:CheckBoxList ID="Chkb_studnts" runat="server" class="form-actions" RepeatDirection="Vertical">
                            </asp:CheckBoxList>
                        </asp:Panel>
			        </td>
			        </tr>
			        <tr>
			        <td align="right" class="style10">
			        
			        
			            <asp:Label ID="Label6" runat="server" class="control-label" Text="Select Type:"></asp:Label>
			        
			        
			            <asp:Label ID="Label9" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
			        
			        
			        </td>
			        <td class="style11" >
			        
			            <asp:DropDownList ID="Drp_Type" runat="server" class="form-control" Width="250px" data-drp="drp" onchange="myScript()">
                        </asp:DropDownList>
			        
			        </td>
			        </tr>
			        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
			    
				       <tr id="rowLink" class="hide">
				            <td align="right" class="style7">
				           <asp:Label ID="Label4" runat="server" class="control-label" Text="Link:"></asp:Label>
				           <asp:Label ID="Label11" runat="server" ForeColor="Red" class="control-label" Text="*"></asp:Label>
				          </td>
				          
				          
				          <td>
				              <asp:TextBox ID="txt_link" runat="server" class="form-control" Width="250px" 
                                 CausesValidation="true" ValidationGroup="Req" 
                                  ontextchanged="txt_link_TextChanged"></asp:TextBox>
				              
				              <asp:Label ID="lbllink" runat="server" class="control-label" ForeColor="Red"></asp:Label>
				          </td>				          
				       </tr>
				       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				       
				       <tr id="rowFileUpload" class="hide">
				       <td align="right" class="style7">
				           <asp:Label ID="Label1" runat="server" class="control-label" Text="Image:"></asp:Label>
				           <asp:Label ID="Label2" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
				          </td>
				          
				          <td>
                              <asp:FileUpload ID="FileUpload1" runat="server" class="form-control" Width="250px" />
                              <asp:RegularExpressionValidator ID="rexp" runat="server" ControlToValidate="FileUpload1" 
                                ErrorMessage="Only .jpg, .png,and .jpeg"
                                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])$)|.*\.([jJ][pP][eE][gG])"></asp:RegularExpressionValidator>
                                <asp:Label ID="LblImageErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
				          </td>
				          
				       </tr>
				       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
				       
					         <tr>
                                 <td align="right" class="style7">
                                     <asp:Label ID="Label5" runat="server" class="control-label" Text="Expiry Date:"></asp:Label>
                                     <asp:Label ID="Label10" runat="server" class="control-label" ForeColor="Red" Text="*"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txt_ExpDate" class="form-control" Width="250px" runat="server"></asp:TextBox>
                                     <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="txt_ExpDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <asp:Label ID="lblexpdate" runat="server" Text="DD/MM/YYYY"></asp:Label>
                                                    <asp:RegularExpressionValidator runat="server" ID="DoJDateRegularExpressionValidator3"
                                ControlToValidate="txt_ExpDate"
                                Display="None"
                                 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                        <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                TargetControlID="DoJDateRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" />
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_ExpDate" ErrorMessage="You Must enter Expiry Date."></asp:RequiredFieldValidator>
                                   
                                 </td>
                     </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                     <tr>
                         <td class="style7">
                         </td>
                         <td>
                             <asp:Button ID="Btn_save" runat="server" class="btn btn-success" 
                                 onclick="Btn_save_Click" Text="Send" />
                             &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="Btn_Clear" runat="server" CausesValidation="false" 
                                 class="btn btn-danger" onclick="Btn_Clear_Click" Text="Clear" />
                         </td>
                     </tr>
					         
					         <tr>
					         <td class="style8">
					         </td>
					         <td class="style6">
                                 <asp:Label ID="lblmsg" runat="server" class="control-label" ForeColor="#FF3300"></asp:Label>
                                 </td>
					         </tr>
					         <tr>
					         <td class="style7">
					         </td>
					         <td></td>
					         </tr>
					        
				 </table>
					  <WC:MSGBOX id="WC_MessageBox" runat="server"/>   			
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
	</ContentTemplate>
<Triggers>
          
             <asp:PostBackTrigger ControlID="Btn_save" />
   </Triggers>
</asp:UpdatePanel>
            
<div class="clear"></div>
</div>


</asp:Content>
