<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ClassStrengthReport.aspx.cs" Inherits="WinEr.ClassStrengthReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
          .ClassAttendanceBack
        {
            margin:9px 9px 9px 9px;
            height:200px;
            padding:5px 10px 10px 5px;
            
            border:solid 1px #4a4a4a;
            -moz-border-radius: 8px;
           -webkit-border-radius: 8px;
           -khtml-border-radius: 8px;
            border-radius: 8px;
        }
        h4
        {
            text-decoration:underline;
            font-size:small;
        }
        .TableHead
        {
         border:solid 1px black;    
         padding-left:12px;
         font-weight:bold;
         color:White;
         background-color:gray;
        }
        .TableCellClass
        {
          text-align:right;  
          font-weight:bold; 
          padding:3px 10px 0px 10px; 
          font-size:12px;
          border:solid 1px gray;
          color:Black;
        }
 
        .Total
        {
          text-align:left;  
         font-weight:bold;
         padding:3px 10px 0px 10px; 
         color:White;
         background-color:gray;
         font-size:12px;
        }
         .TableCell
        {
          text-align:left;  
          font-weight:bold;
          color:Black;  
          padding:3px 10px 0px 10px; 
          border:solid 1px gray ;
          font-size:12px;
        }


 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
 



        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
               
            
          <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Class Student Strength</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                   
                   
                <div style="min-height:250px">
                   
                 <table width="100%" cellspacing="5">
                  
                  <tr>
                   <td style="width:50%" align="right">
                    <asp:Label ID="Label3" runat="server" Text="Select Start Date : " class="control-label"></asp:Label>
                   </td>
                   <td  style="width:50%">
                        <asp:TextBox ID="Txt_StartDate" runat="server"  Width="170px" class="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server"  
                               CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_StartDate" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>  
                       <asp:RegularExpressionValidator ID="Txt_StartDateDateRegularExpressionValidator3" 
                           runat="server" ControlToValidate="Txt_StartDate" Display="None" 
                           ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                           ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                      <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2" 
                           TargetControlID="Txt_StartDateDateRegularExpressionValidator3"
                          HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                   </td>
                  </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                  <tr>
                   <td style="width:50%" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Select End Date : " class="control-label"></asp:Label>
                   </td>
                   <td  style="width:50%">
                        <asp:TextBox ID="Txt_EndDate" runat="server"  Width="170px" class="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                               CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_EndDate" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>  
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                           runat="server" ControlToValidate="Txt_EndDate" Display="None" 
                           ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                           ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"  />
                      <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                           TargetControlID="RegularExpressionValidator1"
                          HighlightCssClass="validatorCalloutHighlight" Enabled="True" />
                   </td>
                  </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                   <tr>
                    <td style="width:50%" align="right">
                      

                   
                    </td>
                    <td style="width:50%"  align="left">
                      
                          <asp:Button ID="Btn_Load" runat="server" Text="Load" Class="btn btn-primary" 
                              onclick="Btn_Load_Click" />
                      
                         &nbsp;
                      <asp:Button ID="Btn_Excel" runat="server" Text="Export" Class="btn btn-primary" onclick="Btn_Excel_Click" 
                               />
                         </td>
                  </tr>
                  <tr>
                   <td colspan="2" align="center">
                   
                       <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                   
                   </td>
                   </tr>

                 </table>
                   
                   <div id="div_ClassStrength" runat="server">
                     <%--<div style="width:900px;overflow:auto">
                      <table width="100%" cellspacing="0"> 
                         <tr>  
                           <td class="style1">  Class </td>  
                           <td class="style1">   Strength  </td> 
                           <td class="style1">   22/09/2011  </td> 
                         </tr> 
                         <tr> 
                          <td class="TableCellClass"> NURSERY : </td> 
                          <td class="TableCell">  0 </td>  
                          <td class="TableCell">  0 </td>  
                         </tr> 
                         <tr> 
                          <td class="TableCellClass"> LKG : </td>  
                          <td class="TableCell">  26 </td>  
                           <td class="TableCell">  8 </td>
                         </tr> 
                         <tr> 
                          <td class="Total"> Total : </td>  
                          <td class="Total">  26 </td>  
                          <td class="Total">  8 </td>
                         </tr> 
                       </table> 
                       </div>--%>  
                     
                     </div>
                   
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
            
            
              
              
    <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">  <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="Are you sure you want" class="control-label"></asp:Label>
                <br /><br />
                
                <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Class="btn btn-info"/>
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


    <div class="clear"></div>
    </div>
</asp:Content>
