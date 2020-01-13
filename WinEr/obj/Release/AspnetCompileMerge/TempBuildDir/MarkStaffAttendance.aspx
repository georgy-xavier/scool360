<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="MarkStaffAttendance.aspx.cs" Inherits="WinEr.MarkStaffAttendance" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .Nextmonth
 {
     padding:0px 10px 0px 10px;
 }
 </style>
 <script type="text/javascript">

     function LoadPopup() {

        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.show();
    }

    function LoadFuturePopup() {

        var modalPopupBehavior = $find('futureprogrammaticModalPopupBehavior');
        modalPopupBehavior.show();
    } 

    function LoadnotBatchPopup() {

        var modalPopupBehavior = $find('NotBatchprogrammaticModalPopupBehavior');
        modalPopupBehavior.show();
    }
 
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
       <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdateattendance">
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

<asp:UpdatePanel ID="pnlAjaxUpdateattendance" runat="server">
 <ContentTemplate>
 

      <center>  

<div class="container skin1" style="width:470px" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/images/user.png" 
                        Height="28px" Width="29px" /> </td>
				<td class="n" align="left">Staff Attendance Form</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				  <center> 
			    	<table   width="95%">
				

				  <tr>

				  <td  align="center">
				  
				  
				  
				   <asp:Calendar ID="Calendar1" runat="server" Height="300px" Width="100%" 
                                    onselectionchanged="Calendar1_SelectionChanged" BackColor="White" 
                                    BorderColor="Black" BorderStyle="Solid" CellSpacing="0" Font-Names="Verdana" 
                                    Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth" 
                                    ondayrender="Calendar1_DayRender"  
                                    onvisiblemonthchanged="Calendar1_VisibleMonthChanged">
                                    <SelectedDayStyle BackColor="#f9f7aa" ForeColor="Black" />
                                    <TodayDayStyle BackColor="White" ForeColor="Black"  BorderColor="Red" BorderWidth="2" BorderStyle="Solid"/>
                                    <OtherMonthDayStyle ForeColor="#999999"/>
                                    <DayStyle BackColor="White"  BorderColor="Black" BorderWidth="1" BorderStyle="Solid" />
                                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black"  CssClass="Nextmonth"  />
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" 
                                        Height="8pt" />
                                    <TitleStyle BackColor="#ffffff" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" Font-Bold="True" 
                                        Font-Size="12pt" ForeColor="Black"  />
                                </asp:Calendar>
                                
                       <table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px" width="100%">
                       <tr>
                        <td style="width:15px;height:15px;background-color:White;border:solid 1px Black;">
                             &nbsp;
                         </td>
                         <td align="left">
                            Attendance Not Marked
                         </td>
                         <td></td>
                         <td style="width:15px;height:15px;background-color:#a4d805;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Attendance Marked
                         </td>
                         </tr>
                        <tr>
                         <td style="width:15px;height:15px;background-color:#ffcc00;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Holiday
                         </td>
                         <td></td>
                         <td style="width:15px;height:15px;background-color:#ffc1c1;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Invalid Batch Day
                         </td>
                       </tr>

                        
                         <tr >
                              <td align="center" colspan="3">
                           
                           
                             <asp:LinkButton ID="Lnk_Unmarked" runat="server" ToolTip="Goto Unmarked Day" onclick="Lnk_Unmarked_Click"></asp:LinkButton>
                           

                             <asp:Label ID="lbldate" runat="server" Text=""></asp:Label>
                         </td>
                          <td align="center" colspan="2">
                           
                             <asp:LinkButton ID="Lnk_Today" runat="server" ToolTip="Goto Today" onclick="Lnk_Today_Click" >Goto Today</asp:LinkButton>
                           </td>
                        </tr>
                         
                     </table>
				  </td>

				  

				 </tr>
				
				</table>
                   
			       </center>   
					
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
	
	</center> 
	<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
   <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground" BehaviorID="programmaticModalPopupBehavior" />
   <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <center>
                <asp:Label ID="Lbl_msg" runat="server" Text="Selected day is holiday" class="control-label" Font-Bold="true"></asp:Label>
                </center>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-info" Text="OK" Width="50px"/>
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
	
	
	
	<asp:Panel ID="Panel1" runat="server">
                       
   <asp:Button runat="server" ID="Button1" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="M_Future"  runat="server" CancelControlID="Btn_ok" 
                                  PopupControlID="Panel2" TargetControlID="Button1" BackgroundCssClass="modalBackground" BehaviorID="futureprogrammaticModalPopupBehavior" />
   <asp:Panel ID="Panel2" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <center>
                <asp:Label ID="Label2" runat="server" Text="You have selected a future day" class="control-label" Font-Bold="true"></asp:Label>
                </center>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_ok" runat="server" class="btn btn-info" Text="OK" Width="50px"/>
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
	
	
	<asp:Panel ID="Panel3" runat="server">
                       
   <asp:Button runat="server" ID="Button2" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1"  runat="server" CancelControlID="Btn_ok" 
                                  PopupControlID="Panel4" TargetControlID="Button2"  BackgroundCssClass="modalBackground" BehaviorID="NotBatchprogrammaticModalPopupBehavior" />
   <asp:Panel ID="Panel4" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <center>
                <asp:Label ID="Label3" runat="server" Text="Selected date is not within current batch" class="control-label" Font-Bold="true"></asp:Label>
                </center>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button3" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
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

 </ContentTemplate>
</asp:UpdatePanel>

        <div class="clear"></div>
    </div>
</asp:Content>
