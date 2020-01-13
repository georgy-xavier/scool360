<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ClassAttDetailedReport.aspx.cs" Inherits="WinEr.ClassAttDetailedReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <style type="text/css">
         .TableHeaderStyle
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           background-color:#666666;
           font-weight:bold;
           color:White;
           text-align:center;
           padding:10px 10px 10px 10px;

         
        }
        .SubHeaderStyle
        {
           background-color:Gray;
           color:White;
           font-weight:bolder;
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
           text-align:left;
           min-width:180px;
        }
        .CellStyle
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           color:#333333;
        }
     
     
     </style>
     
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" >

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
        <table   cellpadding="0" cellspacing="0" class="containerTable">
          <tr >
            <td class="no"> </td>
            <td class="n">Detailed Attendance Report</td>
            <td class="ne"> </td>
          </tr>
          <tr >
             <td class="o"> </td>
             <td class="c" >  
             
             
              <asp:Panel ID="Panel2" runat="server">
                      <table width="100%" cellspacing="15">
                       <tr>
                         <td align="right" style="width:25%">Select Class:</td>
                         <td style="width:25%" align="left">
                                 <asp:DropDownList ID="Drp_ClassSelectDetailed" runat="server" Width="160px" class="form-control" AutoPostBack="true" 
                                     onselectedindexchanged="Drp_ClassSelectDetailed_SelectedIndexChanged">
                                 </asp:DropDownList>
                         </td>
                         <td align="right" style="width:20%">
                                     Time Period:</td>
                                      
                          <td align="left" style="width:30%">
                                      <asp:DropDownList ID="Drp_Select_Period" runat="server" AutoPostBack="True" class="form-control"
                                          onselectedindexchanged="Drp_Select_Period_SelectedIndexChanged" Width="160px">
                                                                           <asp:ListItem>Today</asp:ListItem>
                                      <asp:ListItem>Last Week</asp:ListItem>
                                      <asp:ListItem>Month Wise</asp:ListItem>
                                      <asp:ListItem>Manual</asp:ListItem>
                                  </asp:DropDownList>
                          </td>
                          
                        </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>

                           <td align="right">Start Date:</td>
                           <td align="left">
                                     <asp:TextBox ID="Txt_SDate" runat="server" class="form-control" style="width:160px"></asp:TextBox>
                                      <ajaxToolkit:MaskedEditExtender ID="Txt_SDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_SDate" CultureAMPMPlaceholder="AM;PM" 
                                              CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                              CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                              CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                        
                                         <asp:RegularExpressionValidator runat="server" ID="DobDateRegularExpressionValidator3"
                                                ControlToValidate="Txt_SDate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                                TargetControlID="DobDateRegularExpressionValidator3"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                      ControlToValidate="Txt_SDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                           </td>
                             <td align="right"> End Date:</td>
                             <td align="left">
                                           <asp:TextBox ID="Txt_EDate" runat="server" class="form-control" style="width:160px"></asp:TextBox>
                                          
                                          
                                           <ajaxToolkit:MaskedEditExtender ID="Txt_EDate_MaskedEditExtender" runat="server"  
                                                        MaskType="Date"  CultureName="en-GB"
                                                        Mask="99/99/9999"
                                                        UserDateFormat="DayMonthYear"
                                                        Enabled="True" 
                                                        TargetControlID="Txt_EDate" CultureAMPMPlaceholder="AM;PM" 
                                               CultureCurrencySymbolPlaceholder="£" CultureDateFormat="DMY" 
                                               CultureDatePlaceholder="/" CultureDecimalPlaceholder="." 
                                               CultureThousandsPlaceholder="," CultureTimePlaceholder=":">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    
                              
                                     <asp:RegularExpressionValidator runat="server" ID="Txt_EDate_RegularExpressionValidator1"
                                                ControlToValidate="Txt_EDate"
                                                Display="None" 
                                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                ErrorMessage="<b>Invalid Field</b><br />Date contains invalid characters" />
                                               <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender4"
                                                TargetControlID="Txt_EDate_RegularExpressionValidator1"
                                                HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                                                
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                      ControlToValidate="Txt_EDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                           </td>
                          </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                          <tr>

                            <td align="right">
                                      Select Month:
                           </td>
                           <td align="left">
                                       <asp:DropDownList ID="Drp_Select_Month" runat="server" AutoPostBack="True" class="form-control"
                                          OnSelectedIndexChanged="DrpChangedValue" Width="160px" >
                                      </asp:DropDownList>
                           </td>
                         </tr>
                         <tr>    
                                          
                            <td align="center" colspan="4">  <asp:Button ID="Btn_Show" runat="server" Text="Show" 
                                     onclick="Btn_Show_Click" Class="btn btn-primary" />
                             
                                      &nbsp;
                             
                                      <asp:Button ID="Btn_Excel" runat="server" onclick="Btn_Excel_Click"
                                         Text="Export" Class="btn btn-primary" />
                             </td>
                           
                          </tr>
                         
                              <tr>
                              <td colspan="4"> 
                                  <asp:Label ID="Lbl_Err" runat="server" ForeColor="Red" class="control-label"></asp:Label>
                              </td>
                           </tr>
                           <tr>
                              <td colspan="4">
                              
                              <asp:Panel ID="Pnl_ExamResults" runat="server">
                                <div style="width:1050px;  overflow:auto">
                                    <div id="GridDiv" runat="server">
                                   
                                    </div> 
          
                                </div>
                                </asp:Panel>                            
                             </td>
                           </tr>
                      </table>
                                                  
                      </asp:Panel>
             
             
             </td>
             <td class="e"> </td>
          </tr>
          <tr>
              <td class="o">
                     &nbsp;</td>
              <td class="c">
                     &nbsp;</td>
              <td class="e">
                      &nbsp;</td>
           </tr>
           <tr >
               <td class="so"> </td>
               <td class="s"></td>
               <td class="se"> </td>
           </tr>
        </table>
   </div> 
             
  </ContentTemplate>
  <Triggers>
   <asp:PostBackTrigger ControlID="Btn_Excel" />
  
  </Triggers>
  </asp:UpdatePanel>
</asp:Content>
