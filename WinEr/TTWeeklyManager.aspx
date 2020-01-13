<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="TTWeeklyManager.aspx.cs" Inherits="WinEr.TTWeeklyManager" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style type="text/css">
.temp
{
  text-align:center;
}

.Panelcss
{
    min-height:400px;
}
 /* calendar */
.calendar {
	background-color: white;	
	font-family: Tahoma;
	font-size: 8pt;
}

.calendar td 
{
	font-family: Tahoma;
	font-size: 8pt;
	padding: 2px 2px 2px 2px;
}

.calendar table  {
	background-color: #9EBEF5;
}

.calendar tr td a {
	text-decoration: none;
}

.calendar td a {
	text-decoration: none;
}

.calendar td.today a 
{
    border: solid 1px red;   
}

.calendar td.selected  
{
    background-color: #FBE694;
}
  
    .style1
    {
        width: 230px;
    }
  
</style>


<script type="text/javascript">
    function DayPilotClick(Id) {
        if (Id != '-1' && Id != '-2') {
            mywindow = window.open('TTModifier.aspx?Id=' + Id + '', 'Info', 'status=1, width=900, height=400,resizable = 1');
            mywindow.moveTo(300, 100);
        }
        else {
            if (Id == '-1') {
                alert("Selected Day Is Holiday");
            }
            else if (Id == '-2') {
                alert("Selected Day Is Not A Batch Day");
            }
        }
    }
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>


          <asp:panel ID="Panel2"  runat="server"> 
    
            <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Weekly Time Table</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				
	             <div id="topstrip">
                <table  width="100%" style="color:White">
                 <tr>                 
                    <td align="right" style="width:15%" >
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Class : " ></asp:Label>
                    </td>
                    <td align="left"  style="width:15%" >

                        <asp:DropDownList ID="Drp_ClassName" runat="server" Width="181px" AutoPostBack="true" class="form-control"
                            onselectedindexchanged="Drp_ClassName_SelectedIndexChanged" >
                         </asp:DropDownList>
                    </td>
                     <td align="right"  style="width:15%">
                         
                     </td>
                     <td align="left"  style="width:15%">
                         
                     </td>
                     <td>
                     </td>
                    </tr>
                    </table>
                  
                 </div>
					<br />
				
		 <asp:Panel ID="Pnl_calanderview" runat="server">
                   
              <table >
               <tr>
                <td valign="top" class="style1">
                    <asp:ImageButton ID="Img_Calender" runat="server" 
                        ImageUrl="~/Pics/calendar.png" Width="40" onclick="Img_Calender_Click" />
                    <asp:LinkButton ID="LinkButtonCalender" runat="server"
                         ForeColor="Black" onclick="LinkButtonCalender_Click">View Calender</asp:LinkButton>
    
                </td>
                <td valign="top" align="left" rowspan="2" >
                    <table width="100%" style="border:solid 1px Black;">
                     <tr>
                      <td align="left">
                        
                          <asp:ImageButton ID="ImgBtn_Left" runat="server" ImageUrl="~/images/leftarrow.png" 
                              Width="20px" onclick="ImgBtn_Left_Click" />
                        
                      </td>
                      <td align="center">
                      
                         <asp:Label ID="Lbl_SchedulerHeader" runat="server" Text="" Font-Bold="true"></asp:Label>
                         
                      </td>
                      <td align="right">
                          <asp:ImageButton ID="ImgBtn_Right" runat="server" ImageUrl="~/images/rightarrow.png" 
                              Width="20px" onclick="ImgBtn_Right_Click" />
                      </td>
                     </tr>
                    </table>
                    
                  
                          
                    <DayPilot:DayPilotScheduler ID="DayPlot_Monthly" runat="server"
                        HeaderFontSize="8pt" HeaderHeight="30" 
                        DataStartField="start" 
                        DataEndField="end" 
                        DataTextField="name" 
                        DataValueField="Id" 
                        DataResourceField="Period" 
                        DataBarColorField="barColor"
                        EventHeight="30"
                        EventFontSize="11px" 
                        CellDuration="1440" 
                        CellWidth="90"
                        NonBusinessBackColor="White"
                        BackColor="White"
                        Days="7"  
                        FreeTimeClickJavaScript="mywindow=window.open('MarkClassDailyAttendance.aspx?Start={0}&Resource={1}','Info','status=1, width=900, height=500,resizable = 1');mywindow.moveTo(300,100);" 
                        EventClickJavaScript="DayPilotClick('{0}');"
                        BorderColor="Black" EventBackColor="White" EventBorderColor="#f3f3f3" 
                        HourBorderColor="Black" DurationBarVisible="False" ForeColor="Black" 
                        BorderStyle="None" DurationBarColor="Black" HourNameBorderColor="#ffffff" 
                        >
                       
                    </DayPilot:DayPilotScheduler>
                     
                     <table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px" width="100%">
                       <tr>
                        <td style="width:15px;height:15px;background-color:White;border:solid 1px Black;">
                             &nbsp;
                         </td>
                         <td>
                             Not Configured</td>
                         <td style="width:15px;height:15px;background-color:#c6ffc6;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                             Configured</td>

                         <td style="width:15px;height:15px;background-color:#ffcc00;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                            Holiday
                         </td>
                         <td style="width:15px;height:15px;background-color:Red;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                             Staff Absent
                         </td>

                         <td style="width:15px;height:15px;background-color:#ffc1c1;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                            Not Batch Day
                         </td>
                         <td >
                            &nbsp;
                         </td>
                         <td>
                            </td>
                       </tr>
                     </table>
                     

                </td>
               </tr>    
                  <tr>
                      <td class="style1" valign="top">
                        
                          <asp:Calendar ID="Calendar1" runat="server" CssClass="calendar" 
                              OnSelectionChanged="Calendar1_SelectionChanged" Visible="false" Width="200px">
                              <TodayDayStyle BackColor="#FF6600" BorderColor="#FF6666" BorderStyle="Solid" 
                                  BorderWidth="2px" />
                              <SelectedDayStyle BackColor="#FBE694" CssClass="selected" ForeColor="Black" />
                              <TitleStyle BackColor="White" />
                              <OtherMonthDayStyle ForeColor="#ACA899" />
                          </asp:Calendar>
                      </td>
                  </tr>
              </table>   
       </asp:Panel>
                  
        <asp:Panel ID="Panel_Msg" runat="server" Visible="false">
          
          <br />
          
           <center>
            <asp:Label ID="Label2" runat="server" Text=" Selected Class Has NO Period Available" Font-Bold="true" Font-Size="Larger" ForeColor="Gray" ></asp:Label>
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
          
   </asp:panel> 
           
           <asp:HiddenField ID="Hd_ClassId" runat="server" />
           
  </ContentTemplate>      
  </asp:UpdatePanel>   
</asp:Content>
