<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="StaffTimeTable.aspx.cs" Inherits="WinEr.StaffTimeTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .MyTable
        {
            width:100%;
            
            border: thin solid #333333;
        }
        .MyDay
        {
            background-color: #97FFB1;
        }
        .DataCell
        {
           font-size:11px; 
        }
        
        .TxtBoxStyle
        {
            border:none;
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
  
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
        <div id="contents">
          <div id="right">

                <div class="label">Staff Manager</div>
                    <div id="SubStaffMenu" runat="server">
		
                 </div>
          </div>
          <div id="left">
            
            <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                      <%-- <tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
            <asp:panel ID="Panel2"  runat="server"> 
    
            <div class="container skin1" style="min-height:400px">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">View Time Table</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
			<br />
			<asp:Panel ID="TimeTable" runat="server">
			
			
			<table width="100%">
			<tr>
			 <td>
			     <table>
			      <tr>
			      <td>
			      
			       <asp:ImageButton runat="Server" ID="Img_Caneder" ImageUrl="~/Pics/calendar_empty.png" 
                          Width="30px" AlternateText="Click to show calendar" 
                          onclick="Img_Caneder_Click" /><br />
                  
                   
			      </td>
			      
			      <td>
			      
			          &nbsp;</td>
			      
			         
			      
			     
			      <td>
			      
			          &nbsp;</td>
			     </tr>
			     </table>
			 		
			     
			   
              
                 
                   
                 
			 
			 </td>
			 
			 <td align="right">
			 
			 <asp:ImageButton ID="Img_Export" runat="server"  ToolTip="Export to excel" ImageUrl="~/Pics/Excel.png" Width="30px" Height="35px" OnClick="Img_Export_Click"/>
			 </td>
			 </tr>
			</table>
			
			 <center>
			
                <asp:Label ID="Lbl_label" runat="server" Text="Select a week" Font-Bold="true" Font-Size="13" Visible="false"></asp:Label>
			
			
	             <asp:Calendar ID="Calendar1" runat="server" CssClass="calendar" 
                              OnSelectionChanged="Calendar1_SelectionChanged" Visible="false" Width="300px">
                              <TodayDayStyle BackColor="#FF6600" BorderColor="#FF6666" BorderStyle="Solid" 
                                  BorderWidth="2px" />
                              <SelectedDayStyle BackColor="#FBE694" CssClass="selected" ForeColor="Black" />
                              <TitleStyle BackColor="White" />
                              <OtherMonthDayStyle ForeColor="#ACA899" />
                          </asp:Calendar>
			
              </center> 
			
			<asp:Panel ID="panelData" runat="server">

			<center>
				<div id="MyTimeTableDiv" runat="server"> 
				    <%--<table class="tablelist" border="1" cellpadding="1" cellspacing="5" 
                        style="border: thin solid #333333;background-color: #FFFFFF" >
				        <tr >
				            <th>
                                MONDAY</th>
                            <th>
                                TUESDAY</th>
                            <th style="background-color: #97FFB1">
                                WEDNESDAY</th>
                            <th>
                                THURSDAY</th>
                            <th>
                                FRIDAY</th>
                            <th >
                                SATURDAY</th>
                          </tr>
                            <tr>
                                <td class="style2">
                                    ENGLISH/ONE A</td>
                                <td class="style2">
                                   MALAYALAM / TWO B</td>
                                <td style="background-color: #97FFB1" class="style2">
                                    4345</td>
                                <td class="style2">
                                    4345</td>
                                <td class="style2">
                                    4345</td>
                                <td class="style2">
                                    4345</td>
                            </tr>
                            <tr>
                                <td >
                                    4345</td>
                                <td >
                                    4345</td>
                                <td style="background-color: #97FFB1" >
                                    SOCIAL STUDIES / SEVEN H</td>
                                <td >
                                    4345</td>
                                <td >
                                    4345</td>
                                <td>
                                    4345</td>
                            </tr>
                            <tr >
                                <td style="background-color: #D1D1D1">
                                    4345</td>
                                <td>
                                    4345</td>
                                <td style="background-color: #97FFB1">
                                    4345</td>
                                <td>
                                    MALAYALAM / TWO B</td>
                                <td>
                                    4345</td>
                                <td>
                                    4345</td>
                            </tr>
                            <tr>
                                <td>
                                    4345</td>
                                <td>
                                    4345</td>
                                <td style="background-color: #97FFB1">
                                    MALAYALAM / TWO B</td>
                                <td>
                                    4345</td>
                                <td>
                                     MALAYALAM / TWO B</td>
                                <td>
                                    MALAYALAM / TWO B</td>
                            </tr>
			
				    </table>--%>	
				</div>
				<table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px" width="100%">
                       <tr>
                         <td style="background-color:#c6ffc6;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left">
                             Today</td>

                         <td style="background-color:#ffcc00;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left">
                            Holiday
                         </td>

                         <td style="background-color:#ffc1c1;border:solid 1px Black;width:15px;height:15px;">
                            &nbsp;
                         </td>
                         <td align="left" >
                            Not Batch Day
                         </td>
                         <td >
                            &nbsp;
                         </td>
                         <td >
                            </td>
                       </tr>
                     </table>
			</center>	
			
			</asp:Panel>
			
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
            
          </div>
      </div>
   </ContentTemplate>
   <Triggers >
    <asp:PostBackTrigger ControlID="Img_Export" />
   </Triggers>
  </asp:UpdatePanel>
           
           
            
</asp:Content>
