<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkStaffAttendancePage.aspx.cs" Inherits="WinEr.MarkStaffAttendancePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
       <title>School</title>
 
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
   
     <script type="text/javascript" src="js files/RollNoListItem.js"></script>

    <script language="JavaScript" type="text/javascript">

     var Page;

     var postBackElement;

     function pageLoad()

    {      

      Page = Sys.WebForms.PageRequestManager.getInstance();  

      Page.add_beginRequest(OnBeginRequest);

       Page.add_endRequest(endRequest);

    }

 

    function OnBeginRequest(sender, args)

    {       

      postBackElement = args.get_postBackElement();     

      postBackElement.disabled = true;

    }  

    

     function endRequest(sender, args)

    {      

      postBackElement.disabled = false;

    } 
      
     
  </script>

    <style type="text/css">
        
        
        body{
         width:1000px;
         margin:0 auto;
         position:relative;
         background :#FFF;
         margin-top:5px;
         border-style:outset;
         border-width:thin;
         border-color:Gray;
        }
        
        .GVFixedHeader {  position:relative ; 
        top:auto;
         z-index: 10; 
        }


        
       .modalBackgroundtest {
	    background-color:Black;
	    filter:alpha(opacity=90);
	    opacity:0.7;
      }
      .BottomBorder
      {
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
      }
     .LeftBottomBorder
     {
         border-left-style:outset;
         border-left-width:thin;
         border-left-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
        .RightBottomBorder
     {
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
     #InnerStructure
     {
         border-left-style:outset;
         border-left-width:thin;
         border-left-color:Gray;
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
         width:100%;
         height:200px;
         padding-top:20px;
         
     }
      #heading
      {
        	background-image: url(images/TopStrip.jpg);
        	width:300px;
        	height:30px;
        	color:White;
        	margin-bottom:10px;
      }
      .style1
      {
          border-left: thin outset Gray;
          border-top: thin solid Gray;
          border-bottom: thin solid Gray;
          width: 12%;
      }
      
      
  input.grayverylong{
		background:url(winbuttons/submit1.gif);
		border: 0px;
		padding: 4px 0px;
		text-align:center;
		padding-left:10px;
		height:24px;
		color: #000;
		font: bold 11px Arial, Sans-Serif;
		background-position:left;

		padding-right:10px;
	}
	
	  input.grayverylong:hover  {
		background: url(winbuttons/submit.gif);
		border: 0px;
		padding: 4px 0px;
		text-align:center;
		padding-left:10px;
		color: #FFF;
		height:24px;
	    font: bold 11px Arial, Sans-Serif;
	    background-position:left;

		padding-right:10px;
	    
   }
  </style>
    
  <script type="text/javascript">
      function ShouldUnmark() {
          var Value = document.getElementById("HiddenDate").value;
          var Message = "You are about to cancel the attendance marked for " + Value + ". Are you sure about continuing?";
          return confirm(Message);
      }
  </script>  
  
</head>
<body>
    <form id="form1" runat="server" >
    <div>
    
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
            
    <asp:Panel ID="Panel1" runat="server" >
        <table width="100%">
           <tr>
             <td>
                <div id="heading">
                   <center>
                      <h3>
                       Staff Attendance Marking
                     </h3>
                   </center>
                  </div>
             </td>
             <td valign="top" align="right">
                <div>
                   <asp:Button ID="Btn_Back" runat="server" Text="Back" Width="100px" class="btn btn-primary"  OnClientClick="window.location='MarkStaffAttendance.aspx'"/>
                  </div>
             </td>
           </tr> 
        </table>
          
         
       
          <table style="font-size:13px; width: 100%;" cellspacing="0">

                
            <tr style="height:40px;">                 
                    <td align="right" style="width:25%" >
                        
                    </td>
                    <td align="left"  style="width:25%" >

                        
                     
                    
                    </td>

                     
                    <td align="right" style="width:25%"  class="LeftBottomBorder" >
                        <asp:Label ID="Label3" runat="server" Text="Selected Date : " ></asp:Label>
                    </td>
                    <td align="left"  style="width:25%"  class="RightBottomBorder">
                        
                        <asp:Label ID="lbl_Date" runat="server" Text="." Font-Bold="true" Font-Size="15px"></asp:Label>
                    
                    </td>

                     
                    </tr>
                 
            <tr>
              <td id="InnerStructure" colspan="4" valign="top" >
              
                 
                     
              
                 <asp:Panel ID="Panel_Updating" runat="server">
                 
                   <table  width="100%" cellspacing="10">

                    <tr>
                         <td  align="right">
                         <asp:Button ID="Btn_markall" runat="server" Text="Mark Full Day"  Width="100" class="btn btn-primary"
                                onclick="Btn_markall_Click"/> 
                            &nbsp;
                            <asp:Button ID="Btn_Update" runat="server" Text="Update"  Width="100" class="btn btn-primary" ValidationGroup="Update"
                                onclick="Btn_Update_Click"/> 
                                
                             &nbsp; 
                                
                             <asp:Button ID="Btn_DeleteMarking" runat="server" class="btn btn-primary"
                                 Text="Cancel Attendance Marked" OnClientClick="return ShouldUnmark()" 
                                 onclick="Btn_DeleteMarking_Click" />
                                
                             &nbsp;
                                
                           <asp:Button ID="Btn_CancelUpdate" runat="server" Text="Reset" 
                                 class="btn btn-primary" OnClientClick="window.location.reload()"  Width="100" />
                      </td>
                    </tr>
                    <tr>
                    
                      <td >
                       <div style="height:350px;overflow:auto">
                       
                        <center>
                           <asp:Label ID="lbl_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
                       </center>
                       
                        <asp:GridView ID="Grd_Staff" runat="server" CellPadding="4" ForeColor="Black"  GridLines="Both" AutoGenerateColumns="False"
                           Width="98%"   BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="PresentStatus" HeaderText="Status" />
                                <asp:BoundField DataField="InTime" HeaderText="InTime" />
                                <asp:BoundField DataField="OutTime" HeaderText="OutTime" />
                                <asp:BoundField DataField="SurName" HeaderText="Staff Name" />
                                <asp:TemplateField HeaderText="Present Status" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        
                                        <asp:DropDownList ID="Drp_GridStatus" runat="server">
                                        </asp:DropDownList>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="In Time" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        
                                       <asp:TextBox ID="Txt_InTime" runat="server" Width="80px" Text="00:00:00"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="Txt_TimePup_MaskedEditExtender1" 
                                            runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                                            MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_InTime" />
                                        <ajaxToolkit:MaskedEditValidator ID="Txt_TimePup_MaskedEditValidator1" 
                                            runat="server" ControlExtender="Txt_TimePup_MaskedEditExtender1" 
                                            ControlToValidate="Txt_InTime" Display="Dynamic" EmptyValueBlurredText="*" 
                                            EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                            InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                            TooltipMessage="Input a time" ValidationGroup="Update" />
                                                </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Out Time" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        
                                        <asp:TextBox ID="Txt_OutTime" runat="server"  Width="80px" Text="00:00:00"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="Txt_TimePup_MaskedEditExtender2" 
                                            runat="server" AcceptAMPM="false" ErrorTooltipEnabled="True" Mask="99:99:99" 
                                            MaskType="Time" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                            OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_OutTime" />
                                        <ajaxToolkit:MaskedEditValidator ID="Txt_TimePup_MaskedEditValidator2" 
                                            runat="server" ControlExtender="Txt_TimePup_MaskedEditExtender2" 
                                            ControlToValidate="Txt_OutTime" Display="Dynamic" EmptyValueBlurredText="*" 
                                            EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                            InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                            TooltipMessage="Input a time" ValidationGroup="Update" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="White" />
                            <FooterStyle BackColor="#CCCC99" />
                           
                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White"  CssClass="GVFixedHeader"
                                HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="White" />
                            
                        </asp:GridView>
                        
                        
                       </div> 
                       
                       <center>
                       
                           <asp:Label ID="Lbl_GridMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        
                       </center>
                      </td>
                    
                    </tr>
                   </table>
                 
                 </asp:Panel>
                   
              </td>
             </tr>
              
             </table>
                 
    

    <asp:HiddenField ID="HiddenDate" runat="server" />
                  
                  
   <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackgroundtest"  />
                          <asp:Panel ID="Pnl_msg" runat="server"  style="display:none;">   
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o" style="height: 58px"> </td>
            <td class="c" style="height: 58px" align="center" >
               
                 <asp:Label ID="Lbl_msgAlert" runat="server" Text="" Font-Bold="true"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" OnClientClick="window.close()"/>
                        </div>
            </td>
            <td class="e" style="height: 58px"> </td>
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


  <asp:Panel ID="Panel2" runat="server">
                       
 <asp:Button runat="server" ID="Button2" style="display:none"/>
 <ajaxToolkit:ModalPopupExtender ID="M_Finish" 
                                  runat="server" 
                                  PopupControlID="Panel3" TargetControlID="Button2" BackgroundCssClass="modalBackgroundtest"  />
 <asp:Panel ID="Panel3" runat="server"  style="display:none;">   
  <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o" style="height: 58px"> </td>
            <td class="c" style="height: 58px" align="center" >
                <table>
                  <tr>
                    <td>
                      <asp:Label ID="Lbl_FinishMsg" runat="server" Text="" ></asp:Label>
                    </td>
                  </tr>
                   <tr>
                    <td>
                       <asp:Label ID="lbl_MissRollNos" runat="server" Text=""  ></asp:Label>
                    </td>
                  </tr>
                     <tr>
                    <td>
                       <asp:Label ID="Lbl_ErrorRolLNo" runat="server" Text=""  ></asp:Label>
                    </td>
                  </tr>
                   <tr>
                    <td>
                       <br />
                       
                        <asp:Label ID="Lbl_Link" runat="server" Text="" Visible="false" ></asp:Label>
                           <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_Ok" runat="server" Text="OK" Width="50px"  OnClientClick="window.location.reload()" />
                        </div>
                    </td>
                  </tr>
                </table>
                 
                    
                       
                   
            </td>
            <td class="e" style="height: 58px"> </td>
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

 </asp:Panel>
 
     <WC:MSGBOX id="WC_MessageBox" runat="server" />
 </ContentTemplate>
 </asp:UpdatePanel>
    
    
    </div>
    </form>
</body>
</html>
