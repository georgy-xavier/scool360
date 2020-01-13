<%@ Page  Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AssesmentReport.aspx.cs" Inherits="WinEr.AssesmentReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: right;
            font-weight: lighter;
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      
       <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate>
                <div id="progressBackgroundFilter"></div>
                <div id="processMessage">
                    <table style="height:100%;width:100%" >
                        <tr>
                            <td align="center">
                            <b>Please Wait...</b><br /><br />
                            <img src="images/indicator-big.gif" alt=""/></td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>     
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate> 
            <asp:Panel ID="pp1" runat="server" >
            <div class="container skin1" >
		    <table cellpadding="0" cellspacing="0" class="containerTable">
			    <tr>
				    <td class="no"> </td>
				    <td class="n">Assessment Report</td>
				    <td class="ne"> </td>
			    </tr>
			    <tr >
				    <td class="o"> </td>
				    <td class="c">
				        <table class="tablelist">
                              <tr>
                                    <td class="leftside">Select class</td>
                                    <td class="rightside"><asp:DropDownList ID="Drp_ClassSelect" runat="server" class="form-control"
                                            AutoPostBack="True" TabIndex="1"
                                            Width="170px" 
                                            onselectedindexchanged="Drp_ClassSelect_SelectedIndexChanged">
                                        </asp:DropDownList></td></tr>
                                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                                <tr>
                                    <td class="leftside">Select exam type</td>
                                    <td class="rightside"><asp:DropDownList ID="Drp_ExamType" runat="server" AutoPostBack="True" TabIndex="2"
                                          class="form-control"  Width="170px">
                                        </asp:DropDownList></td>
                                </tr>
                                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                               
                               <tr>
                               <td class="leftside">Select Student</td>
                                    <td class="rightside"><asp:DropDownList ID="drp_Student" runat="server" AutoPostBack="True" TabIndex="3"
                                          class="form-control"  Width="170px">
                                        </asp:DropDownList></td>
                               </tr>
                                
                                 <tr>
                                 <td class="leftside">Attendance marked dates</td>
                                 
                                 <td></td>
                                 </tr>
                                  <tr> 
                                     <td class="leftside">Start Date   </td>
                                    <td class="rightside">
                                          <asp:TextBox ID="Txt_startdate" runat="server" Width="170px" class="form-control" TabIndex="4"></asp:TextBox> 
                                                    <ajaxToolkit:MaskedEditExtender ID="Txt_startdate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_startdate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <span style="color:Blue">DD/MM/YYYY</span>
        
                                         
                                            <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                            ControlToValidate="Txt_startdate"
                                            Display="None" 
                                            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                           <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender2"
                                            TargetControlID="DobDateRegularExpressionValidator3"
                                            HighlightCssClass="validatorCalloutHighlight" /><br/>
                                             <asp:RequiredFieldValidator ID="Txt_From_ReqFieldValidator" runat="server" ControlToValidate="Txt_startdate" ErrorMessage="Enter  Date" ValidationGroup="show"></asp:RequiredFieldValidator>
      
                               
                               
                                    </td> 
                                  </tr> 
                                  <tr><td class="style1"> End Date</td>
                                  <td class="rightside" style="height: 25px">
                                        <asp:TextBox ID="Txt_EndDate" runat="server" Width="170px" class="form-control" TabIndex="5"></asp:TextBox> 
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_EndDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <span style="color:Blue">DD/MM/YYYY</span>
        
                                         
                                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                            ControlToValidate="Txt_EndDate"
                                            Display="None" 
                                            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                            ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                           <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender1"
                                            TargetControlID="RegularExpressionValidator1"
                                            HighlightCssClass="validatorCalloutHighlight" /><br/>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_EndDate" ErrorMessage="Enter  Date" ValidationGroup="show"></asp:RequiredFieldValidator>
      
                               
                               
                                    </td>
                                  </tr> 
                                 
                                   
                                    <tr>
                                    <td></td>
                                    <td >
                                        <asp:Button ID="Btn_Generate" runat="server" Text="Generate" ValidationGroup="show" TabIndex="6"
                                            onclick="Btn_Generate_Click" Class="btn btn-primary" 
                                           /></td>
                                          
                                     </tr>
                                     
                                <tr>
                                
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" class="control-label"></asp:Label>
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
	        </asp:Panel>
          
         <div class="clear"></div>
         </ContentTemplate>
       
        </asp:UpdatePanel>
    </div>
</asp:Content>
